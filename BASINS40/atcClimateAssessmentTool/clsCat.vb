Imports atcData
Imports atcUtility
Imports atcControls

Imports MapWinUtility
Imports System.Windows.Forms

Public Class clsCat
    Public WithEvents Model As clsCatModel
    Public Inputs As New Generic.List(Of atcVariation)
    Public Endpoints As New Generic.List(Of atcVariation)
    Public PreparedInputs As New Generic.List(Of String) 'TODO: allow selected?
    Public SaveAll As Boolean = False
    Public ShowEachRunProgress As Boolean = False
    Public ModifiedModelName As String = "Modified"
    Public ResultsGrid As New atcControls.atcGridSource
    Public ResultsRow As Integer
    Public TimePerRun As Double = 0 'Time each run takes in seconds
    Public RunModel As Boolean = True 'True to actually run the model, false to just look for already computed results

    Private pLastPrivateMemory As Integer
    Private pLastGcMemory As Integer

    Friend Const CLIGEN_NAME As String = "Cligen"
    Friend Const StartFolderVariable As String = "{StartFolder}"
    Friend Const CurDirVariable As String = "{CurDir}"
    Friend Const RunTitle As String = "Run"
    Friend Const ResultsFixedRows As Integer = 4

    Public Event Loaded()
    Public Event Started()
    Public Event StatusUpdate(ByVal aStatus As String)
    Public Event StartIteration(ByVal aIteration As Integer)
    Public Sub StartIterationMessage(ByVal aIteration As Integer) 'default message for batch runs
        Dim lString As String = "StartIteration " & aIteration + 1 & " of " & TotalIterations & _
                            " (" & CInt(((100 * aIteration) / TotalIterations)).ToString & "%)"
        If aIteration > 0 Then
            lString &= " TimeToComplete " & FormatTime((TotalIterations - aIteration) * TimePerRun)
        End If
        Logger.Dbg(lString)
    End Sub
    Public Event UpdateResults()
    Public Event BaseModelSet(ByVal aModelName As String)

    Public Property XML() As String
        Get
            Dim lXML As String = ""
            Dim lVariation As atcVariation

            lXML &= "<SaveAll>" & SaveAll & "</SaveAll>" & vbCrLf

            lXML &= "<ShowEachRun>" & ShowEachRunProgress & "</ShowEachRun>" & vbCrLf
            lXML &= "<ModifiedModelName>" & ModifiedModelName & "</ModifiedModelName>" & vbCrLf

            lXML &= Model.XML

            If PreparedInputs.Count = 0 Then
                lXML &= "<Variations>" & vbCrLf
                For Each lVariation In Inputs
                    lXML &= lVariation.XML
                Next
                lXML &= "</Variations>" & vbCrLf
            Else
                lXML &= "<PreparedInputs>"
                For Each lPreparedInput As String In PreparedInputs
                    lXML &= "<PreparedInput>" & ToXML(lPreparedInput) & "</PreparedInput>" & vbCrLf
                Next
                'For Each lPreparedInput As String In PreparedInputs
                '    lXML &= "<PreparedInput selected=""" & lstInputs.CheckedItems.Contains(lPreparedInput)
                '    lXML &= """>" & lPreparedInput & "</PreparedInput>" & vbCrLf
                'Next
                lXML &= "</PreparedInputs>"
            End If

            lXML &= "<Endpoints>" & vbCrLf
            For Each lVariation In Endpoints
                lXML &= lVariation.XML
            Next
            lXML &= "</Endpoints>" & vbCrLf

            Dim lStartFolder As String = CurDir()
            lXML = ReplaceStringNoCase(lXML, lStartFolder, StartFolderVariable)
            If lXML.Contains(StartFolderVariable) Then
                lXML = "<StartFolder>" & ToXML(lStartFolder) & "</StartFolder>" & vbCrLf & lXML
            End If

            lXML = "<BasinsCAT>" & vbCrLf & lXML & "</BasinsCAT>" & vbCrLf
            Return lXML
        End Get

        Set(ByVal newValue As String)
            Try
                Dim lXMLdoc As New Xml.XmlDocument
StartOver:
                lXMLdoc.LoadXml(newValue)
                Dim lNode As Xml.XmlNode = lXMLdoc.FirstChild
                If lNode.Name.ToLower.Equals("basinscat") Then
                    For Each lXML As Xml.XmlNode In lNode.ChildNodes
                        Dim lVariation As atcVariation
                        Dim lChild As Xml.XmlNode = lXML.FirstChild
                        Select Case lXML.Name.ToLower
                            Case "startfolder" 'Replace start folder in all XML if present
                                Dim lStartFolder As String = lXML.InnerText
                                If lStartFolder.ToLower = CurDirVariable.ToLower Then
                                    lStartFolder = CurDir()
                                End If
                                newValue = ReplaceString(newValue, lXML.OuterXml, "")
                                newValue = ReplaceString(newValue, StartFolderVariable, lStartFolder)
                                GoTo StartOver
                            Case "saveall"
                                SaveAll = (lXML.InnerText.ToLower = "true")
                            Case "showeachrun"
                                ShowEachRunProgress = (lXML.InnerText.ToLower = "true")
                            Case "modifiedmodelname"
                                ModifiedModelName = lXML.InnerText

                                'TODO: generic case for any model type that creates correct Model and sets XML, 
                                '      BaseModel (and anything model specific) gets set inside XML property set
                            Case "uci"
                                Model = New clsCatModelHSPF
                                Model.BaseModel = (AbsolutePath(lChild.InnerText, CurDir))
                            Case "inp"
                                Model = New clsCatModelSWMM
                                Model.BaseModel = (AbsolutePath(lChild.InnerText, CurDir))
                            Case "swat"
                                Model = New clsCatModelSWAT
                                Model.BaseModel = (AbsolutePath(lChild.InnerText, CurDir))

                            Case "preparedinputs"
                                PreparedInputs.Clear()
                                For Each lChild In lXML.ChildNodes
                                    PreparedInputs.Add(lChild.InnerText)
                                Next
                            Case "variations"
                                Inputs.Clear()
                                For Each lChild In lXML.ChildNodes
                                    If lChild.InnerXml.IndexOf(CLIGEN_NAME) >= 0 Then
                                        lVariation = New VariationCligen
                                    Else
                                        lVariation = New atcVariation
                                    End If
                                    lVariation.XML = lChild.OuterXml
                                    If Not lVariation.IsInput Then
                                        lVariation.IsInput = True
                                        Logger.Dbg("Assigned IsInput to loaded variation '" & lVariation.Name & "'")
                                    End If
                                    Inputs.Add(lVariation)
                                Next
                            Case "endpoints"
                                Endpoints.Clear()
                                For Each lChild In lXML.ChildNodes
                                    lVariation = New atcVariation
                                    lVariation.XML = lChild.OuterXml
                                    'Used to keep input variations in endpoints, skip them
                                    If Not lVariation.IsInput Then
                                        Endpoints.Add(lVariation)
                                    End If
                                Next
                        End Select
                    Next
                    RaiseEvent Loaded()
                End If
            Catch e As Exception
                Logger.Msg("Could not load XML:" & vbCrLf & e.Message & vbCrLf & vbCrLf & newValue, "CAT XML Problem")
            End Try
        End Set
    End Property

    Public ReadOnly Property TotalIterations() As Integer
        Get
            Dim lTotalIterations As Integer = 1
            If PreparedInputs.Count = 0 Then
                lTotalIterations = 1
                For Each lVariation As atcVariation In Inputs
                    If lVariation.Selected AndAlso lVariation.Iterations > 1 Then
                        lTotalIterations *= lVariation.Iterations
                    End If
                Next
            Else
                lTotalIterations = PreparedInputs.Count
            End If
            Return lTotalIterations
        End Get
    End Property

    Friend Shared Function OpenDataSource(ByVal aFilename As String) As atcTimeseriesSource
        Dim lDataSource As atcTimeseriesSource = atcDataManager.DataSourceBySpecification(aFilename)
        If lDataSource IsNot Nothing Then 'already open
            Return lDataSource
        ElseIf FileExists(aFilename) Then
            Select Case IO.Path.GetExtension(aFilename).Substring(1).ToLower 'test letters after .
                Case "wdm" : lDataSource = New atcWDM.atcDataSourceWDM
                Case "hbn" : lDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
                Case "pcp" : lDataSource = New atcTimeseriesSWAT.atcTimeseriesSWAT
                Case "tmp" : lDataSource = New atcTimeseriesSWAT.atcTimeseriesSWAT
                Case "hru" : lDataSource = New atcTimeseriesSWAT.atcTimeseriesSWAT
                Case "rch" : lDataSource = New atcTimeseriesSWAT.atcTimeseriesSWAT
                Case "sub" : lDataSource = New atcTimeseriesSWAT.atcTimeseriesSWAT
                Case "out" : lDataSource = New atcTimeseriesSWMM5Output.atcDataSourceTimeseriesSWMM5Output
                Case "inp" : lDataSource = New atcSWMM.atcSWMMProject
                Case Else
                    Throw New ApplicationException("Could not open '" & aFilename & "' in frmCAT:OpenDataSource")
            End Select

            lDataSource.Specification = aFilename
            atcDataManager.OpenDataSource(lDataSource, lDataSource.Specification, Nothing)
            Return lDataSource
        End If
        Return Nothing
    End Function

    Public Function StartRun() As Boolean
        g_Running = True
        Dim lSelectedVariations As Generic.List(Of atcVariation) = SelectedVariations(Inputs)
        'Dim lSelectedEndpoints As Generic.List(Of atcVariation) = SelectedVariations(Endpoints)

        'InitResultsGrid(lSelectedVariations, lSelectedEndpoints)

        RaiseEvent Started()

        Dim lRuns As Integer = 0
        Run(lSelectedVariations, _
            lRuns, 0, Nothing)

        g_Running = False
        Return True
    End Function

    Public Function SelectedVariations(ByVal aAllVariations As Generic.List(Of atcVariation)) As Generic.List(Of atcVariation)
        Dim lSelectedVariations As New Generic.List(Of atcVariation)
        If PreparedInputs.Count = 0 Then
            For Each lVariation As atcVariation In aAllVariations
                If lVariation.Selected Then
                    lSelectedVariations.Add(lVariation)
                End If
            Next
        End If
        Return lSelectedVariations
    End Function

    Public Sub InitResultsGrid()
        InitResultsGrid(SelectedVariations(Inputs), SelectedVariations(Endpoints))
    End Sub

    Public Sub InitResultsGrid(ByVal aSelectedVariations As Generic.List(Of atcVariation), _
                               ByVal aSelectedEndpoints As Generic.List(Of atcVariation))
        ResultsGrid = New atcGridSource
        With ResultsGrid
            Dim lColumn As Integer = 1
            .FixedRows = ResultsFixedRows
            .FixedColumns = 1
            .Columns = 1
            For Each lVariation As atcVariation In aSelectedVariations
                Select Case lVariation.Operation
                    Case "Hamon", "Penman-Monteith"
                        'Skip PET variations in results table
                    Case Else
                        .Columns += 1
                End Select
            Next
            .CellValue(0, 0) = RunTitle

            For Each lVariation As atcVariation In aSelectedEndpoints
                .Columns += lVariation.DataSets.Count
            Next
            .Rows = ResultsFixedRows + 1
            If SaveAll Then .Columns += 1 : .CellValue(0, .Columns - 1) = "Saved Results"

            lColumn = 1

            If PreparedInputs.Count = 0 Then
                'header for attributes
                For Each lVariation As atcVariation In aSelectedVariations
                    Select Case lVariation.Operation
                        Case "Hamon", "Penman-Monteith"
                            'Skip PET variations in results table
                        Case Else
                            LabelResultsColumn(lColumn, lVariation, Nothing)
                            lColumn += 1
                    End Select
                Next
            End If

            Dim lFirstEndpointColumn As Integer = lColumn

            For Each lVariation As atcVariation In aSelectedEndpoints
                For Each lDataset As atcDataSet In lVariation.DataSets
                    LabelResultsColumn(lColumn, lVariation, lDataset)
                    lColumn += 1
                Next
            Next

            .CellValue(.FixedRows, 0) = "base"
            .CellColor(.FixedRows, 0) = Drawing.SystemColors.Control
            PopulateEndpoints(.FixedRows, Nothing)

            ResultsRow = ResultsFixedRows + 1
        End With
    End Sub

    Private Sub LabelResultsColumn(ByVal aColumn As Integer, ByVal aVariation As atcVariation, ByVal aDataset As atcDataSet)
        With ResultsGrid
            .CellValue(0, aColumn) = aVariation.Name
            If aVariation.Operation IsNot Nothing Then
                .CellValue(1, aColumn) = aVariation.Operation
            End If

            If aVariation.Seasons IsNot Nothing Then
                .CellValue(2, aColumn) = aVariation.Seasons.ToString
            End If

            If aDataset IsNot Nothing Then
                .CellValue(3, aColumn) = aDataset.ToString
            Else
                .CellValue(3, aColumn) = "Current Value"
            End If
        End With
    End Sub

    Private Function ResultsGridColumn(ByVal aVariation As atcVariation, ByVal aDataset As atcTimeseries) As Integer
        With ResultsGrid
            Dim lOperation As String = Nothing
            Dim lSeason As String = Nothing
            Dim lDataset As String = "Current Value"
            If aVariation.Operation IsNot Nothing Then lOperation = aVariation.Operation.ToString
            If aVariation.Seasons IsNot Nothing Then lSeason = aVariation.Seasons.ToString
            If aDataset IsNot Nothing Then lDataset = aDataset.ToString
            Dim lColumn As Integer = 1
            While lColumn < .Columns
                If .CellValue(0, lColumn) = aVariation.Name AndAlso _
                   .CellValue(1, lColumn) = lOperation AndAlso _
                   .CellValue(2, lColumn) = lSeason AndAlso _
                   .CellValue(3, lColumn) = lDataset Then

                    Return lColumn
                End If
                lColumn += 1
            End While

            'Column not found, create a new column for this variation/dataset
            .Columns += 1
            If aDataset Is Nothing Then 'Insert new input change in first column
                lColumn = 1
                For lRow As Integer = 0 To .Rows - 1
                    For lMoveToColumn As Integer = .Columns - 1 To lColumn + 1 Step -1
                        .CellValue(lRow, lMoveToColumn) = .CellValue(lRow, lMoveToColumn - 1)
                    Next
                    .CellValue(lRow, lColumn) = Nothing
                Next
            ElseIf SaveAll Then 'Insert new column as second-to-last column
                lColumn -= 1
                For lRow As Integer = 0 To .Rows - 1 'Move results path column so it stays far right
                    .CellValue(lRow, .Columns - 1) = .CellValue(lRow, lColumn)
                    .CellValue(lRow, lColumn) = Nothing
                Next
            Else
                'Add new column as far right column
            End If

            LabelResultsColumn(lColumn, aVariation, aDataset)

            Return lColumn
        End With
    End Function

    Private Sub Run(ByVal aVariations As Generic.List(Of atcVariation), _
                    ByRef aIteration As Integer, _
                    ByRef aStartVariationIndex As Integer, _
                    ByRef aModifiedData As atcTimeseriesGroup)

        If Not g_Running Then
            RaiseEvent StatusUpdate("Stopping Run")
        Else
            Logger.Dbg("Run Variation " & aStartVariationIndex & " of " & aVariations.Count)
            If aModifiedData Is Nothing Then
                aModifiedData = New atcTimeseriesGroup
            End If
            If Model Is Nothing OrElse Model.BaseModel Is Nothing Then
                Logger.Dbg("ModelNotSet")
            Else
                Dim lCurDir As String = IO.Path.GetDirectoryName(Model.BaseModel)
                Logger.Dbg("ChangeCurDirTo " & lCurDir)
                Try
                    ChDriveDir(lCurDir)
                Catch lEx As Exception
                    Logger.Dbg("UnableToChangeDirectory " & lEx.Message)
                End Try
            End If
            Logger.Dbg("WorkingDir " & My.Computer.FileSystem.CurrentDirectory)

            If aStartVariationIndex = 0 AndAlso PreparedInputs.Count = 0 Then
                With ResultsGrid
                    .CellValue(ResultsRow, 0) = aIteration + 1
                    .CellColor(ResultsRow, 0) = Drawing.SystemColors.Control
                    Dim lColumn As Integer = .FixedColumns
                    For Each lVariation As atcVariation In Inputs
                        If lVariation.Selected Then
                            Select Case lVariation.Operation
                                Case "Hamon", "Penman-Monteith"
                                    'Skip PET variations in results table
                                Case Else
                                    .CellValue(ResultsRow, lColumn) = "Computing..."
                                    lColumn += 1
                            End Select
                        End If
                    Next
                End With
                RaiseEvent UpdateResults()
            End If

            If aStartVariationIndex >= aVariations.Count Then 'All variations have values, do a model run
NextIteration:
                Dim lPreparedInput As String
                Dim lModifiedModelName As String

                If PreparedInputs.Count = 0 Then
                    lPreparedInput = ""
                    lModifiedModelName = ModifiedModelName
                    If SaveAll Then
                        lModifiedModelName &= "-" & Format(aIteration + 1, "000")
                    End If
                Else
                    lPreparedInput = PreparedInputs.Item(aIteration)
                    lModifiedModelName = IO.Path.GetFileNameWithoutExtension(PathNameOnly(lPreparedInput))
                End If

                RaiseEvent StartIteration(aIteration)
                TimePerRun = Now.ToOADate

                'TODO: don't assume Model.BaseModel is a filename, find results another way
                Dim lModifiedFolder As String = PathNameOnly(AbsolutePath(Model.BaseModel, CurDir)) & g_PathChar & lModifiedModelName
                Dim lThisRunVariations As String = ""

                Dim lColumn As Integer
                With ResultsGrid
                    lColumn = .FixedColumns

                    .CellColor(ResultsRow, 0) = Drawing.SystemColors.Control
                    For lRunningColumn As Integer = lColumn To .Columns - 1
                        .CellValue(ResultsRow, lRunningColumn) = "Running..."
                    Next

                    If PreparedInputs.Count = 0 Then
                        .CellValue(ResultsRow, 0) = aIteration + 1
                        For Each lVariation As atcVariation In aVariations
                            Select Case lVariation.Operation
                                Case "Hamon", "Penman-Monteith"
                                    'Skip PET variations in results table
                                Case Else
                                    lColumn = ResultsGridColumn(lVariation, Nothing)
                                    .CellValue(ResultsRow, lColumn) = Format(lVariation.CurrentValue, "0.####")
                                    lThisRunVariations &= lVariation.ToString & " = " & .CellValue(ResultsRow, lColumn) & vbCrLf
                                    lColumn += 1
                            End Select
                        Next
                    Else
                        .CellValue(ResultsRow, 0) = IO.Path.GetFileNameWithoutExtension(PathNameOnly(PreparedInputs.Item(aIteration)))
                    End If

                    If SaveAll AndAlso .CellValue(0, .Columns - 1) = "Saved Results" Then
                        .CellValue(ResultsRow, .Columns - 1) = lModifiedFolder
                    End If
                    RaiseEvent UpdateResults()
                End With

                Dim lResults As atcCollection = Model.ModelRun(lModifiedModelName, aModifiedData, lPreparedInput, RunModel, ShowEachRunProgress, False)
                If lResults Is Nothing OrElse lResults.Count = 0 Then
                    Logger.Dbg("Null model results from ModelRun")
                Else
                    TimePerRun = (Now.ToOADate - TimePerRun) * 24 * 60 * 60 'Convert days to seconds
                End If

                PopulateEndpoints(ResultsRow, lResults)

                'Remove any remaining cells still displaying Running status.
                ' this happens when running with different inputs/endpoints than before and not clearing results grid
                For lRunningColumn As Integer = ResultsGrid.FixedColumns To ResultsGrid.Columns - 1
                    If ResultsGrid.CellValue(ResultsRow, lRunningColumn) = "Running..." Then
                        ResultsGrid.CellValue(ResultsRow, lRunningColumn) = Nothing
                        RaiseEvent UpdateResults()
                    End If
                Next

                ResultsRow += 1

                If SaveAll Then
                    If lThisRunVariations.Length > 0 Then
                        SaveFileString(lModifiedFolder & g_PathChar & "Changes.txt", lThisRunVariations)
                    End If
                    SaveFileString(lModifiedFolder & g_PathChar & "Results.txt", ResultsGrid.ToString)
                Else
                    SaveFileString(lModifiedFolder & ".Results.txt", ResultsGrid.ToString)
                End If

                'Close any open results
                For Each lSpecification As String In lResults
                    lSpecification = lSpecification.ToLower
                    Dim lMatchDataSource As atcTimeseriesSource = atcDataManager.DataSourceBySpecification(lSpecification)
                    If lMatchDataSource IsNot Nothing Then
                        'lMatchDataSource.clear 'TODO: want to make sure we don't have a memory leak here
                        atcDataManager.RemoveDataSource(lMatchDataSource)
                        lMatchDataSource.Clear()
                        System.GC.Collect()
                        System.GC.WaitForPendingFinalizers()
                    End If
                Next

                aIteration += 1
                If g_Running AndAlso Not PreparedInputs.Count = 0 AndAlso aIteration < PreparedInputs.Count Then
                    GoTo NextIteration
                End If
            Else 'Need to loop through values for next variation
                Dim lVariation As atcVariation = aVariations.Item(aStartVariationIndex)
                With lVariation
                    Logger.Dbg("StartVariation " & .Name)
                    'save how this variation's datasets looked before we modified them
                    Dim lOriginalDatasets As atcTimeseriesGroup = .DataSets.Clone
                    Logger.Dbg("OriginalDatasetsCount " & lOriginalDatasets.Count)
                    Dim lOriginalPETtemperature As atcTimeseriesGroup = .PETtemperature.Clone
                    Dim lOriginalPETprecip As atcTimeseriesGroup = .PETprecipitation.Clone

                    'data modified before this variation plus data modified by this one
                    Dim lAllModifiedData As atcTimeseriesGroup = aModifiedData.Clone
                    Logger.Dbg("ModifiedDatasetsCount " & lAllModifiedData.Count)

                    For lDataSetIndex As Integer = 0 To .DataSets.Count - 1
                        Dim lSourceDataSet As atcTimeseries = .DataSets(lDataSetIndex)
                        Dim lModifiedIndex As Integer = lAllModifiedData.Keys.IndexOf(lSourceDataSet)
                        If lModifiedIndex >= 0 Then
                            .DataSets.Item(lDataSetIndex) = lAllModifiedData.ItemByIndex(lModifiedIndex)
                            lAllModifiedData.RemoveAt(lModifiedIndex)
                        End If
                    Next

                    For lDataSetIndex As Integer = 0 To .PETtemperature.Count - 1
                        Dim lSourceDataSet As atcTimeseries = .PETtemperature(lDataSetIndex)
                        Dim lModifiedIndex As Integer = lAllModifiedData.Keys.IndexOf(lSourceDataSet)
                        If lModifiedIndex >= 0 Then
                            .PETtemperature.Item(lDataSetIndex) = lAllModifiedData.ItemByIndex(lModifiedIndex)
                        End If
                    Next

                    For lDataSetIndex As Integer = 0 To .PETprecipitation.Count - 1
                        Dim lSourceDataSet As atcTimeseries = .PETprecipitation(lDataSetIndex)
                        Dim lModifiedIndex As Integer = lAllModifiedData.Keys.IndexOf(lSourceDataSet)
                        If lModifiedIndex >= 0 Then
                            .PETprecipitation.Item(lDataSetIndex) = lAllModifiedData.ItemByIndex(lModifiedIndex)
                        End If
                    Next

                    'Start varying data
                    Logger.Dbg("AboutToStartIteration")
                    Dim lNewlyModified As atcTimeseriesGroup = .StartIteration
                    Logger.Dbg("DoneStartIteration")

                    While g_Running AndAlso lNewlyModified IsNot Nothing
                        'Remove existing modified data also modified by this variation
                        'Most cases of this were handled above when creating lReModifiedData, 
                        'but side-effect computation like PET still needs removing here
                        For Each lKey As Object In lNewlyModified.Keys
                            Logger.Dbg("Remove " & lKey.ToString)
                            lAllModifiedData.RemoveByKey(lKey)
                        Next

                        Logger.Dbg("Add " & lNewlyModified.Count)
                        lAllModifiedData.Add(lNewlyModified)

                        'We have handled a variation, now recursively handle more input variations or run the model
                        Run(aVariations, _
                            aIteration, _
                            aStartVariationIndex + 1, _
                            lAllModifiedData)

                        lAllModifiedData.Remove(lNewlyModified)
                        lNewlyModified = .NextIteration
                    End While

                    .DataSets = lOriginalDatasets
                    .PETtemperature = lOriginalPETtemperature
                    .PETprecipitation = lOriginalPETprecip
                End With
            End If
        End If
    End Sub

    Private Sub PopulateEndpoints(ByVal aRow As Integer, ByVal aResults As atcCollection)
        With ResultsGrid
            For Each lEndpoint As atcVariation In Endpoints
                System.GC.Collect()
                System.GC.WaitForPendingFinalizers()
                If lEndpoint.Selected Then
                    For Each lOldData As atcTimeseries In lEndpoint.DataSets
                        Dim lData As atcTimeseries = Nothing
                        Dim lColumn As Integer = ResultsGridColumn(lEndpoint, lOldData)
                        If aResults Is Nothing Then
                            lData = lOldData
                        Else
                            .CellValue(aRow, lColumn) = "Opening..."
                            Dim lGroup As atcTimeseriesGroup = Nothing
                            Dim lOriginalDataSpec As String = lOldData.Attributes.GetValue("History 1", "").Substring(10)
                            Dim lResultDataSpec As String = aResults.ItemByKey(IO.Path.GetFileName(lOriginalDataSpec).ToLower.Trim)
                            If lResultDataSpec Is Nothing Then
                                .CellValue(aRow, lColumn) = "ResultsDataSpec is Nothing for " & lOldData.ToString
                                Logger.Dbg(.CellValue(aRow, lColumn))
                            Else
                                Dim lResultDataSource As atcTimeseriesSource = OpenDataSource(lResultDataSpec)
                                If lResultDataSource Is Nothing Then
                                    .CellValue(aRow, lColumn) = "ResultsDataSource is Nothing for " & lResultDataSpec.ToString
                                    Logger.Dbg(.CellValue(aRow, lColumn))
                                Else
                                    lGroup = lResultDataSource.DataSets.FindData("ID", lOldData.Attributes.GetValue("ID"), 1)
                                    If lGroup IsNot Nothing AndAlso lGroup.Count > 0 Then
                                        lData = lGroup.Item(0)
                                    End If
                                End If
                            End If
                        End If
                        If lData IsNot Nothing Then
                            'TODO: add change of time step to atcVariation, use it here to change time step if specified

                            lData = lEndpoint.SplitData(lData, Nothing).ItemByIndex(0)

                            .CellValue(aRow, lColumn) = lData.Attributes.GetFormattedValue(lEndpoint.Operation)
                            If .ColorCells Then
                                If Not IsNumeric(.CellValue(aRow, lColumn)) Then
                                    .CellColor(aRow, lColumn) = lEndpoint.ColorDefault
                                Else
                                    Dim lValue As Double = lData.Attributes.GetValue(lEndpoint.Operation)
                                    If Not Double.IsNaN(lEndpoint.Min) AndAlso lValue < lEndpoint.Min Then
                                        .CellColor(aRow, lColumn) = lEndpoint.ColorBelowMin
                                    ElseIf Not Double.IsNaN(lEndpoint.Max) AndAlso lValue > lEndpoint.Max Then
                                        .CellColor(aRow, lColumn) = lEndpoint.ColorAboveMax
                                    Else
                                        .CellColor(aRow, lColumn) = lEndpoint.ColorDefault
                                    End If
                                End If
                            End If
                        Else
                            .CellValue(aRow, lColumn) = "No Data for ID " & lOldData.Attributes.GetValue("ID")
                            Logger.Dbg(.CellValue(aRow, lColumn))
                        End If
                    Next
                End If
            Next
        End With
        RaiseEvent UpdateResults()
    End Sub

    Friend Function MemUsage() As String
        System.GC.Collect()
        System.GC.WaitForPendingFinalizers()
        Dim lPrivateMemory As Integer = Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20)
        Dim lGcMemory As Integer = System.GC.GetTotalMemory(True) / (2 ^ 20)
        MemUsage = "Megabytes: " & lPrivateMemory & " (" & Format((lPrivateMemory - pLastPrivateMemory), "+0;-0") & ") " _
                   & " GC: " & lGcMemory & " (" & Format((lGcMemory - pLastGcMemory), "+0;-0") & ") "
        pLastPrivateMemory = lPrivateMemory
        pLastGcMemory = lGcMemory
    End Function

    Private Sub Model_BaseModelSet(ByVal aBaseModel As String) Handles Model.BaseModelSet
        RaiseEvent BaseModelSet(aBaseModel)
    End Sub
End Class
