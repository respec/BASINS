Imports atcData
Imports atcUtility
Imports atcControls

Imports MapWinUtility
Imports System.Windows.Forms

Public Class clsCat
    Public Inputs As New Generic.List(Of atcVariation)
    Public Endpoints As New Generic.List(Of atcVariation)
    Public PreparedInputs As Generic.List(Of String)
    Public SaveAll As Boolean = False
    Public ShowEachRunProgress As Boolean = False
    Public BaseScenario As String = ""
    Public ResultsGrid As New atcControls.atcGridSource
    Public TimePerRun As Double = 0 'Time each run takes in seconds

    Friend Const CLIGEN_NAME As String = "Cligen"
    Friend Const StartFolderVariable As String = "{StartFolder}"
    Friend Const RunTitle As String = "Run"
    Friend Const ResultsFixedRows As Integer = 4

    Public Event Started()
    Public Event StatusUpdate(ByVal aStatus As String)
    Public Event StartIteration(ByVal aIteration As Integer)
    Public Event UpdateResults(ByVal aIteration As Integer, ByVal aResults As atcCollection, ByVal aResultsFilename As String)

    Public Property XML() As String
        Get
            Dim lXML As String = ""
            Dim lVariation As atcVariation

            lXML &= "<SaveAll>" & SaveAll & "</SaveAll>" & vbCrLf

            lXML &= "<ShowEachRun>" & ShowEachRunProgress & "</ShowEachRun>" & vbCrLf

            lXML &= "<UCI>" & vbCrLf
            lXML &= "  <FileName>" & BaseScenario & "</FileName>" & vbCrLf
            lXML &= "</UCI>" & vbCrLf

            If PreparedInputs Is Nothing Then
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
                                newValue = ReplaceString(newValue, lXML.OuterXml, "")
                                newValue = ReplaceString(newValue, StartFolderVariable, lStartFolder)
                                GoTo StartOver
                            Case "saveall"
                                SaveAll = (lXML.InnerText.ToLower = "true")
                            Case "showeachrun"
                                ShowEachRunProgress = (lXML.InnerText.ToLower = "true")
                            Case "uci"
                                OpenUCI(AbsolutePath(lChild.InnerText, CurDir))
                            Case "preparedinputs"
                                If PreparedInputs Is Nothing Then
                                    PreparedInputs = New Generic.List(Of String)
                                Else
                                    PreparedInputs.Clear()
                                End If
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
                End If
            Catch e As Exception
                Logger.Msg("Could not load XML:" & vbCrLf & e.Message & vbCrLf & vbCrLf & newValue, "CAT XML Problem")
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Open data files referred to in this UCI file
    ''' </summary>
    ''' <param name="aUCIfilename">Full path of UCI file</param>
    ''' <remarks></remarks>
    Friend Sub OpenUCI(Optional ByVal aUCIfilename As String = "")
        If Not aUCIfilename Is Nothing And Not FileExists(aUCIfilename) Then
            If FileExists(aUCIfilename & ".uci") Then aUCIfilename &= ".uci"
        End If

        If aUCIfilename Is Nothing OrElse Not FileExists(aUCIfilename) Then
            Dim cdlg As New OpenFileDialog
            cdlg.Title = "Open UCI file containing base scenario"
            cdlg.Filter = "UCI files|*.uci|All Files|*.*"
            If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                aUCIfilename = cdlg.FileName
            End If
        End If

        If FileExists(aUCIfilename) Then
            BaseScenario = aUCIfilename
            Dim lUciFolder As String = PathNameOnly(aUCIfilename)
            ChDriveDir(lUciFolder)

            Dim lFullText As String = WholeFileString(aUCIfilename)
            For Each lWDMfilename As String In UCIFilesBlockFilenames(lFullText, "WDM")
                lWDMfilename = AbsolutePath(lWDMfilename, lUciFolder)
                OpenDataSource(lWDMfilename)
            Next
            For Each lBinOutFilename As String In UCIFilesBlockFilenames(lFullText, "BINO")
                lBinOutFilename = AbsolutePath(lBinOutFilename, lUciFolder)
                OpenDataSource(lBinOutFilename)
            Next
        End If
    End Sub

    Friend Function OpenDataSource(ByVal aFilename As String) As atcDataSource
        Dim lAddSource As Boolean = True
        For Each lDataSource As atcDataSource In atcDataManager.DataSources
            If lDataSource.Specification.ToLower = aFilename.ToLower Then 'already open
                Return lDataSource
            End If
        Next
        If lAddSource AndAlso FileExists(aFilename) Then
            Dim lDataSource As atcDataSource
            If aFilename.ToLower.EndsWith("wdm") Then
                lDataSource = New atcWDM.atcDataSourceWDM
            ElseIf aFilename.ToLower.EndsWith("hbn") Then
                lDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
            Else
                Throw New ApplicationException("Could not open '" & aFilename & "' in frmCAT:OpenDataSource")
            End If
            lDataSource.Specification = aFilename
            atcDataManager.OpenDataSource(lDataSource, lDataSource.Specification, Nothing)
            Return lDataSource
        End If
        Return Nothing
    End Function

    Public Function Start(ByVal aBaseScenario As String, ByVal aModifiedScenario As String, _
                          ByVal aSelectedVariations As Generic.List(Of atcVariation), _
                          ByVal aSelectedPreparedInputs As atcCollection) As Boolean
        'header for attributes
        Dim lNumInputColumns As Integer
        If aSelectedVariations Is Nothing Then
            lNumInputColumns = 0
        Else
            lNumInputColumns = aSelectedVariations.Count
        End If

        ResultsGrid = New atcGridSource
        With ResultsGrid
            Dim lUsingSeasons As Boolean = False
            Dim lColumn As Integer = 1
            .FixedRows = ResultsFixedRows
            .FixedColumns = 1
            .Columns = 1 + lNumInputColumns
            .CellValue(0, 0) = RunTitle

            For Each lVariation As atcVariation In Endpoints
                If lVariation.Selected Then
                    .Columns += lVariation.DataSets.Count
                End If
            Next
            .Rows = 5
            lColumn = 1

            If PreparedInputs Is Nothing Then
                For Each lVariation As atcVariation In aSelectedVariations
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
        'don't let winhspflt bring up message boxes
        Dim lBaseFolder As String = PathNameOnly(AbsolutePath(aBaseScenario, CurDir))
        SaveFileString(lBaseFolder & "\WinHSPFLtError.Log", "WinHSPFMessagesFollow:" & vbCrLf)

        Dim lRuns As Integer = 0
        Run(aModifiedScenario, _
             aSelectedVariations, _
            aSelectedPreparedInputs, _
            aBaseScenario, _
            lRuns, 0, Nothing)
    End Function

    Private Sub Run(ByVal aModifiedScenarioName As String, _
                    ByVal aVariations As Generic.List(Of atcVariation), _
                    ByVal aPreparedInputs As atcCollection, _
                    ByVal aBaseFileName As String, _
                    ByRef aIteration As Integer, _
                    ByRef aStartVariationIndex As Integer, _
                    ByRef aModifiedData As atcDataGroup)

        If Not g_running Then
            RaiseEvent StatusUpdate("Stopping Run")
        Else
            Logger.Dbg("Run")
            If aModifiedData Is Nothing Then
                aModifiedData = New atcDataGroup
            End If
            ChDriveDir(PathNameOnly(aBaseFileName))

            If aStartVariationIndex >= aVariations.Count Then 'All variations have values, do a model run
NextIteration:
                Dim lPreparedInput As String
                Dim lModifiedScenarioName As String

                If aPreparedInputs Is Nothing Then
                    lPreparedInput = ""
                    lModifiedScenarioName = aModifiedScenarioName
                    If SaveAll Then
                        lModifiedScenarioName &= "-" & aIteration + 1
                    End If
                Else
                    lPreparedInput = aPreparedInputs.ItemByIndex(aIteration)
                    lModifiedScenarioName = IO.Path.GetFileNameWithoutExtension(PathNameOnly(lPreparedInput))
                End If

                RaiseEvent StartIteration(aIteration)
                TimePerRun = Now.ToOADate
                Dim lResults As atcCollection = ScenarioRun(aBaseFileName, lModifiedScenarioName, aModifiedData, lPreparedInput, True, ShowEachRunProgress, False)
                If lResults Is Nothing Then
                    Logger.Dbg("Null scenario results from ScenarioRun")
                    Exit Sub
                End If
                TimePerRun = (Now.ToOADate - TimePerRun) * 24 * 60 * 60 'Convert days to seconds

                RaiseEvent UpdateResults(aIteration, lResults, PathNameOnly(aBaseFileName) & "\" & lModifiedScenarioName & ".results.txt")

                'Close any open results
                For Each lSpecification As String In lResults
                    lSpecification = lSpecification.ToLower
                    Dim lMatchDataSource As atcDataSource = Nothing
                    For Each lDataSource As atcDataSource In atcDataManager.DataSources
                        If lDataSource.Specification.ToLower = lSpecification Then
                            lMatchDataSource = lDataSource
                            Exit For
                        End If
                    Next
                    If Not lMatchDataSource Is Nothing Then
                        'lMatchDataSource.clear 'TODO: want to make sure we don't have a memory leak here
                        atcDataManager.DataSources.Remove(lMatchDataSource)
                    End If
                Next

                aIteration += 1
                If g_running AndAlso Not aPreparedInputs Is Nothing AndAlso aIteration < aPreparedInputs.Count Then
                    GoTo NextIteration
                End If
            Else 'Need to loop through values for next variation
                Dim lVariation As atcVariation = aVariations.Item(aStartVariationIndex)
                With lVariation
                    Dim lOriginalDatasets As atcDataGroup = .DataSets.Clone
                    'save version of data modified by an earlier variation if it will also be modified by this one
                    Dim lReModifiedData As New atcDataGroup

                    For lDataSetIndex As Integer = 0 To .DataSets.Count - 1
                        Dim lSourceDataSet As atcTimeseries = .DataSets(lDataSetIndex)
                        Dim lModifiedIndex As Integer = aModifiedData.Keys.IndexOf(lSourceDataSet)
                        If lModifiedIndex >= 0 Then
                            .DataSets.Item(lDataSetIndex) = aModifiedData.ItemByIndex(lModifiedIndex)
                            lReModifiedData.Add(lSourceDataSet, aModifiedData.ItemByIndex(lModifiedIndex))
                            aModifiedData.RemoveAt(lModifiedIndex)
                        End If
                    Next

                    'Start varying data
                    Dim lModifiedGroup As atcDataGroup = .StartIteration

                    While g_running And Not lModifiedGroup Is Nothing
                        'Remove existing modified data also modified by this variation
                        'Most cases of this were handled above when creating lReModifiedData, 
                        'but side-effect computation like PET still needs removing here
                        For Each lKey As Object In lModifiedGroup.Keys
                            aModifiedData.RemoveByKey(lKey)
                        Next

                        aModifiedData.Add(lModifiedGroup)

                        'We have handled a variation, now recursively handle more input variations or run the model
                        Run(aModifiedScenarioName, _
                            aVariations, _
                            aPreparedInputs, _
                            aBaseFileName, _
                            aIteration, _
                            aStartVariationIndex + 1, _
                            aModifiedData)

                        aModifiedData.Remove(lModifiedGroup)
                        lModifiedGroup = .NextIteration
                    End While

                    aModifiedData.Add(lReModifiedData)

                    .DataSets = lOriginalDatasets
                End With
            End If
        End If
    End Sub
End Class
