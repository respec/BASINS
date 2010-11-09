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
    Public ResultsGrid As New atcControls.atcGridSource
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
    Public Event UpdateResults(ByVal aResultsFilename As String)
    Public Event BaseScenarioSet(ByVal aScenarioName As String)

    Public Property XML() As String
        Get
            Dim lXML As String = ""
            Dim lVariation As atcVariation

            lXML &= "<SaveAll>" & SaveAll & "</SaveAll>" & vbCrLf

            lXML &= "<ShowEachRun>" & ShowEachRunProgress & "</ShowEachRun>" & vbCrLf

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
                    lXML &= "<PreparedInput>" & lPreparedInput & "</PreparedInput>" & vbCrLf
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
                lXML = "<StartFolder>" & lStartFolder & "</StartFolder>" & vbCrLf & lXML
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
                            Case "uci"
                                If Model Is Nothing Then Model = New clsCatModelHSPF
                                Model.BaseScenario = (AbsolutePath(lChild.InnerText, CurDir))
                                'TODO: generic case for any model type that creates correct Model and sets XML, 
                                '      BaseScenario (and anything model specific) gets set inside XML property set
                            Case "inp"
                                If Model Is Nothing Then Model = New clsCatModelSWMM
                                Model.BaseScenario = (AbsolutePath(lChild.InnerText, CurDir))
                            Case "swat"
                                If Model Is Nothing Then Model = New clsCatModelSWAT
                                Model.BaseScenario = (AbsolutePath(lChild.InnerText, CurDir))
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

    Public Function StartRun(ByVal aModifiedScenario As String) As Boolean
        g_Running = True
        Dim lSelectedVariations As New Generic.List(Of atcVariation)
        If PreparedInputs.Count = 0 Then
            For Each lVariation As atcVariation In Inputs
                If lVariation.Selected Then
                    lSelectedVariations.Add(lVariation)
                End If
            Next
        End If

        ResultsGrid = New atcGridSource
        With ResultsGrid
            Dim lUsingSeasons As Boolean = False
            Dim lColumn As Integer = 1
            .FixedRows = ResultsFixedRows
            .FixedColumns = 1
            .Columns = 1 + lSelectedVariations.Count
            .CellValue(0, 0) = RunTitle

            For Each lVariation As atcVariation In Endpoints
                If lVariation.Selected Then
                    .Columns += lVariation.DataSets.Count
                End If
            Next
            .Rows = 5
            lColumn = 1

            If PreparedInputs.Count = 0 Then
                'header for attributes
                For Each lVariation As atcVariation In lSelectedVariations
                    .CellValue(0, lColumn) = lVariation.Name
                    .CellValue(1, lColumn) = lVariation.Operation
                    .CellValue(2, lColumn) = "Current Value"
                    If Not lVariation.Seasons Is Nothing Then
                        .CellValue(3, lColumn) = lVariation.Seasons.ToString
                    End If
                    lColumn += 1
                Next
            End If

            For Each lVariation As atcVariation In Endpoints
                If lVariation.Selected Then
                    For Each lDataset As atcDataSet In lVariation.DataSets
                        .CellValue(0, lColumn) = lVariation.Name
                        If Not lVariation.Operation Is Nothing Then
                            .CellValue(1, lColumn) = lVariation.Operation
                        End If
                        .CellValue(2, lColumn) = lDataset.ToString
                        If Not lVariation.Seasons Is Nothing Then
                            .CellValue(3, lColumn) = lVariation.Seasons.ToString
                        End If
                        lColumn += 1
                    Next
                End If
            Next
        End With
        RaiseEvent Started()

        Dim lRuns As Integer = 0
        Run(aModifiedScenario, _
            lSelectedVariations, _
            lRuns, 0, Nothing)

        g_Running = False
        Return True
    End Function

    Private Sub Run(ByVal aModifiedScenarioName As String, _
                    ByVal aVariations As Generic.List(Of atcVariation), _
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
            If Model Is Nothing OrElse Model.BaseScenario Is Nothing Then
                Logger.Dbg("ModelNotSet")
            Else
                Dim lCurDir As String = IO.Path.GetDirectoryName(Model.BaseScenario)
                Logger.Dbg("ChangeCurDirTo " & lCurDir)
                Try
                    ChDriveDir(lCurDir)
                Catch lEx As Exception
                    Logger.Dbg("UnableToChangeDirectory " & lEx.Message)
                End Try
            End If
            Logger.Dbg("WorkingDir " & My.Computer.FileSystem.CurrentDirectory)

            If aStartVariationIndex >= aVariations.Count Then 'All variations have values, do a model run
NextIteration:
                Dim lPreparedInput As String
                Dim lModifiedScenarioName As String

                If PreparedInputs.Count = 0 Then
                    lPreparedInput = ""
                    lModifiedScenarioName = aModifiedScenarioName
                    If SaveAll Then
                        lModifiedScenarioName &= "-" & aIteration + 1
                    End If
                Else
                    lPreparedInput = PreparedInputs.Item(aIteration)
                    lModifiedScenarioName = IO.Path.GetFileNameWithoutExtension(PathNameOnly(lPreparedInput))
                End If

                RaiseEvent StartIteration(aIteration)
                TimePerRun = Now.ToOADate
                Dim lResults As atcCollection = Model.ScenarioRun(lModifiedScenarioName, aModifiedData, lPreparedInput, RunModel, ShowEachRunProgress, False)
                If lResults Is Nothing OrElse lResults.Count = 0 Then
                    Logger.Dbg("Null scenario results from ScenarioRun")
                Else
                    TimePerRun = (Now.ToOADate - TimePerRun) * 24 * 60 * 60 'Convert days to seconds
                End If

                With ResultsGrid
                    Dim lRow As Integer = aIteration + .FixedRows
                    Dim lColumn As Integer = .FixedColumns
                    Dim lVariation As atcVariation

                    If PreparedInputs.Count = 0 Then
                        .CellValue(lRow, 0) = aIteration + 1
                        For Each lVariation In Inputs
                            If lVariation.Selected Then
                                .CellValue(lRow, lColumn) = Format(lVariation.CurrentValue, "0.####")
                                lColumn += 1
                            End If
                        Next
                    Else
                        .CellValue(lRow, 0) = IO.Path.GetFileNameWithoutExtension(PathNameOnly(PreparedInputs.Item(aIteration)))
                        '.CellValue(lRow, 0) = IO.Path.GetFileNameWithoutExtension(PathNameOnly(PreparedInputs.Item(lstInputs.CheckedIndices.Item(aIteration))))
                    End If
                    .CellColor(lRow, 0) = Drawing.SystemColors.Control

                    For Each lVariation In Endpoints
                        System.GC.Collect()
                        System.GC.WaitForPendingFinalizers()
                        If lVariation.Selected Then
                            For Each lOldData As atcTimeseries In lVariation.DataSets
                                .CellValue(lRow, lColumn) = "RunFailed 1"
                                Dim lGroup As atcTimeseriesGroup = Nothing
                                Dim lOriginalDataSpec As String = lOldData.Attributes.GetValue("History 1", "").Substring(10)
                                Dim lResultDataSpec As String = lResults.ItemByKey(IO.Path.GetFileName(lOriginalDataSpec).ToLower.Trim)
                                If lResultDataSpec Is Nothing Then
                                    Logger.Dbg("ResultsDataSpec is Nothing for " & lOldData.ToString)
                                    .CellValue(lRow, lColumn) = "RunFailed"
                                Else
                                    Dim lResultDataSource As atcTimeseriesSource = OpenDataSource(lResultDataSpec)
                                    If lResultDataSource Is Nothing Then
                                        Logger.Dbg("ResultsDataSource is Nothing for " & lResultDataSpec.ToString)
                                        .CellValue(lRow, lColumn) = "RunFailed 2"
                                    Else
                                        lGroup = lResultDataSource.DataSets.FindData("ID", lOldData.Attributes.GetValue("ID"), 1)
                                        If Not (lGroup Is Nothing) AndAlso lGroup.Count > 0 Then
                                            Dim lData As atcTimeseries = lGroup.Item(0)

                                            'TODO: add change of time step to atcVariation, use it here to change time step if specified

                                            lData = lVariation.SplitData(lData, Nothing).ItemByIndex(0)

                                            .CellValue(lRow, lColumn) = lData.Attributes.GetFormattedValue(lVariation.Operation)
                                            If .ColorCells Then
                                                If Not IsNumeric(.CellValue(lRow, lColumn)) Then
                                                    .CellColor(lRow, lColumn) = lVariation.ColorDefault
                                                Else
                                                    Dim lValue As Double = lGroup.Item(0).Attributes.GetValue(lVariation.Operation)
                                                    If Not Double.IsNaN(lVariation.Min) AndAlso lValue < lVariation.Min Then
                                                        .CellColor(lRow, lColumn) = lVariation.ColorBelowMin
                                                    ElseIf Not Double.IsNaN(lVariation.Max) AndAlso lValue > lVariation.Max Then
                                                        .CellColor(lRow, lColumn) = lVariation.ColorAboveMax
                                                    Else
                                                        .CellColor(lRow, lColumn) = lVariation.ColorDefault
                                                    End If
                                                End If
                                            End If
                                        Else
                                            Logger.Dbg("No Data for ID " & lOldData.Attributes.GetValue("ID") & _
                                                       " Count " & lResultDataSource.DataSets.Count)
                                            .CellValue(lRow, lColumn) = "RunFailed 3"
                                        End If
                                    End If
                                End If
                                lColumn += 1
                            Next
                        End If
                    Next
                End With
                'TODO: don't assume Model.BaseScenario is a filename, find results another way
                RaiseEvent UpdateResults(PathNameOnly(Model.BaseScenario) & g_PathChar & lModifiedScenarioName & ".results.txt")

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

                    'data modified before this variation plus data modified by this one
                    Dim lAllModifiedData As atcTimeseriesGroup = aModifiedData.Clone
                    Logger.Dbg("ModifiedDatasetsCount " & lAllModifiedData.Count)

                    For lDataSetIndex As Integer = 0 To .DataSets.Count - 1
                        Logger.Dbg("Loop " & lDataSetIndex)
                        Dim lSourceDataSet As atcTimeseries = .DataSets(lDataSetIndex)
                        Dim lModifiedIndex As Integer = lAllModifiedData.Keys.IndexOf(lSourceDataSet)
                        If lModifiedIndex >= 0 Then
                            .DataSets.Item(lDataSetIndex) = lAllModifiedData.ItemByIndex(lModifiedIndex)
                            lAllModifiedData.RemoveAt(lModifiedIndex)
                        End If
                    Next

                    'Start varying data
                    Logger.Dbg("AboutToStartIteration")
                    Dim lNewlyModified As atcTimeseriesGroup = .StartIteration
                    Logger.Dbg("DoneStartIteration")

                    While g_Running And Not lNewlyModified Is Nothing
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
                        Run(aModifiedScenarioName, _
                            aVariations, _
                            aIteration, _
                            aStartVariationIndex + 1, _
                            lAllModifiedData)

                        lAllModifiedData.Remove(lNewlyModified)
                        lNewlyModified = .NextIteration
                    End While

                    .DataSets = lOriginalDatasets
                End With
            End If
        End If
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

    Private Sub Model_BaseScenarioSet(ByVal aBaseScenario As String) Handles Model.BaseScenarioSet
        RaiseEvent BaseScenarioSet(aBaseScenario)
    End Sub
End Class
