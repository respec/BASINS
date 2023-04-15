Imports atcUtility
Imports atcData
Imports atcBatchProcessing
Imports MapWinUtility

Public Class clsBatchSpec
    Inherits clsBatch

    Public Sub New(ByVal aProgressbar As System.Windows.Forms.ProgressBar, ByVal aTextField As System.Windows.Forms.TextBox)
        gProgressBar = aProgressbar
        gTextStatus = aTextField
    End Sub

    Public Sub New()
    End Sub

    Public Property RunModeConsole As Boolean = False
    Public Property SilentConsole As Boolean = True

    Public Overrides Sub PopulateScenarios()
        If IO.File.Exists(SpecFilename) Then
            If ListBatchOpns Is Nothing Then
                ListBatchOpns = New atcCollection()
            Else
                ListBatchOpns.Clear()
            End If
            If ListBatchUnitsData Is Nothing Then
                ListBatchUnitsData = New atcCollection()
            Else
                ListBatchUnitsData.Clear()
            End If
            If DownloadStations Is Nothing Then
                DownloadStations = New atcCollection()
            End If
            Dim lOneLine As String
            Dim lSR As New IO.StreamReader(SpecFilename)

            Dim lBaseflowOpnCounter As Integer = 1
            While Not lSR.EndOfStream
                lOneLine = lSR.ReadLine()
                If lOneLine.Contains("***") Then Continue While 'bypass comments
                If lOneLine.Trim() = "" Then Continue While 'bypass blank lines

                If lOneLine.StartsWith("GLOBAL") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("END GLOBAL") Then
                            lReachedEnd = True
                            Exit While
                        End If
                        SpecifyGlobal(lOneLine)
                    End While
                    If lReachedEnd Then Continue While
                End If

                If lOneLine.StartsWith("SWSTAT") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("END SWSTAT") Then
                            lReachedEnd = True
                            lBaseflowOpnCounter += 1
                            Exit While
                        End If
                        SpecifyGroup(lOneLine, lBaseflowOpnCounter)
                    End While
                    If lReachedEnd Then Continue While
                End If

                If lOneLine.StartsWith("OTHER") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("END OTHER") Then
                            lReachedEnd = True
                            Exit While
                        End If
                        'SpecifyOther(lOneLine)
                    End While
                    If lReachedEnd Then Continue While
                End If
            End While
            lSR.Close()
            lSR = Nothing
        End If

        MergeSpecs()
    End Sub

    ''' <summary>
    ''' Merge global setting with individual station setting
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub MergeSpecs()
        For Each lBFOpn As atcCollection In ListBatchOpns
            For Each lStation As clsBatchUnitStation In lBFOpn
                If lStation.NeedToDownloadData Then
                    If Not DownloadStations.Keys.Contains(lStation.StationID) Then
                        DownloadStations.Add(lStation.StationID, lStation.StationID)
                    End If
                End If
                For Each lgDef As atcDefinedValue In GlobalSettings
                    With lStation.BFInputs
                        If .GetValue(lgDef.Definition.Name) Is Nothing Then
                            .SetValue(lgDef.Definition.Name, lgDef.Value)
                        End If
                    End With
                Next
            Next
        Next

        'Below is to download all stations data at once
        'Dim lContinueDataDownload As Boolean = True
        'clsBatchUtil.SiteInfoDir = GlobalSettings.GetValue("DataDir")
        'If Not IO.Directory.Exists(clsBatchUtil.SiteInfoDir) Then
        '    Dim lDirOpenDiag As New System.Windows.Forms.FolderBrowserDialog()
        '    lDirOpenDiag.Description = "Specify Directory To Save Downloaded Streamflow Data"
        '    If lDirOpenDiag.ShowDialog = Windows.Forms.DialogResult.OK Then
        '        clsBatchUtil.SiteInfoDir = lDirOpenDiag.SelectedPath
        '    Else
        '        Message = "Error: No data folder is specified for downloading data. Batch run stopped."
        '        UpdateStatus(Message, , True)
        '        lContinueDataDownload = False
        '    End If
        'End If
        'If lContinueDataDownload Then
        '    Try
        '        clsBatchUtil.DownloadData(DownloadStations)
        '    Catch ex As Exception

        '    End Try

        '    Dim lDownloadFailedStations As New atcCollection()
        '    For Each lBFOpn As atcCollection In ListBatchOpns
        '        For Each lStation As clsBatchUnitStation In lBFOpn
        '            If DownloadStations.Contains(lStation.StationID) Then
        '                For Each lStationId As String In DownloadStations
        '                    If lStation.StationID = lStationId Then
        '                        Dim lDataPath As String = IO.Path.Combine(clsBatchUtil.SiteInfoDir, "NWIS\NWIS_discharge_" & lStationId & ".rdb")
        '                        If IO.File.Exists(lDataPath) Then
        '                            lStation.StationDataFilename = lDataPath
        '                            lStation.NeedToDownloadData = False
        '                        Else
        '                            'Download failed for this station
        '                            lStation.StationDataFilename = ""
        '                            lStation.NeedToDownloadData = True
        '                            lDownloadFailedStations.Add(lStation.StationID, lStation.StationID)
        '                        End If
        '                    End If
        '                Next
        '            End If
        '        Next
        '    Next
        '    If lDownloadFailedStations.Count > 0 Then
        '        Message = "Error: Data Download failed for the following stations:" & vbCrLf
        '        For Each lStationID As String In lDownloadFailedStations
        '            Message &= lStationID & vbCrLf
        '        Next
        '        UpdateStatus(Message, , True)
        '    End If
        'End If
    End Sub

    ''' <summary>
    ''' This routine save a line of global setting
    ''' differentiate by first keyword on the line
    ''' </summary>
    ''' <param name="aSpec"></param>
    ''' <remarks></remarks>
    Public Overrides Sub SpecifyGlobal(ByVal aSpec As String)
        If GlobalSettings Is Nothing Then GlobalSettings = New atcDataAttributes()
        Dim lArr() As String = aSpec.Split(Delimiter)
        If String.IsNullOrEmpty(lArr(1).Trim()) Then Return
        Dim lAttribName As String = lArr(0).Trim()
        If GlobalSettings.GetValue(lAttribName) IsNot Nothing Then Return

        Select Case lAttribName.ToLower
            'Case atcTimeseriesBaseflow.BFInputNames.StartDate.ToLower
            '    Dim lDates(5) As Integer
            '    Dim lDate As DateTime
            '    If Date.TryParse(lArr(1), lDate) Then
            '        lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
            '        lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
            '        lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
            '        lDates(3) = 0
            '        lDates(4) = 0
            '        lDates(5) = 0
            '        GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.StartDate, lDates)
            '    End If
            'Case atcTimeseriesBaseflow.BFInputNames.EndDate.ToLower
            '    Dim lDates(5) As Integer
            '    Dim lDate As DateTime
            '    If Date.TryParse(lArr(1), lDate) Then
            '        lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
            '        lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
            '        lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
            '        lDates(3) = 24
            '        lDates(4) = 0
            '        lDates(5) = 0
            '        GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.EndDate, lDates)
            '    End If
            Case InputNames.Method.ToLower()
                Dim lMethods As atcCollection = GlobalSettings.GetValue(InputNames.Methods)

                Dim lMethod As InputNames.ITAMethod = 0
                Select Case lArr(1).ToUpper()
                    Case InputNames.ITAMethod.FREQUECYGRID.ToString().ToUpper()
                        lMethod = InputNames.ITAMethod.FREQUECYGRID
                    Case InputNames.ITAMethod.FREQUENCYGRAPH.ToString().ToUpper()
                        lMethod = InputNames.ITAMethod.FREQUENCYGRAPH
                    Case InputNames.ITAMethod.NDAYTIMESERIES.ToString().ToUpper()
                        lMethod = InputNames.ITAMethod.NDAYTIMESERIES
                    Case InputNames.ITAMethod.TRENDLIST.ToString().ToUpper()
                        lMethod = InputNames.ITAMethod.TRENDLIST
                End Select
                If lMethod > 0 Then
                    If lMethods Is Nothing Then
                        lMethods = New atcCollection()
                        lMethods.Add(lMethod)
                        GlobalSettings.Add(InputNames.Methods, lMethods)
                    Else
                        If Not lMethods.Contains(lMethod) Then
                            lMethods.Add(lMethod)
                        End If
                    End If
                End If
            Case InputNames.DataDir.ToLower
                If Not String.IsNullOrEmpty(lArr(1)) AndAlso IO.Directory.Exists(lArr(1)) Then
                    DownloadDataDirectory = lArr(1)
                End If
                GlobalSettings.Add(InputNames.DataDir, lArr(1))
                'Case InputNames.UseCache.ToLower
                '    Dim lUseCache As Boolean = False
                '    If Not String.IsNullOrEmpty(lArr(1)) AndAlso lArr(1).ToLower = "yes" Then
                '        lUseCache = True
                '    End If
                '    GlobalSettings.Add(BatchInputNames.UseCache, lUseCache)
            Case Else
                If GlobalSettings.ContainsAttribute(lArr(0)) Then
                    GlobalSettings.SetValue(lArr(0), lArr(1))
                Else
                    GlobalSettings.Add(lArr(0), lArr(1))
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Create list of SWSTAT Units
    ''' </summary>
    ''' <param name="aSpec"></param>
    ''' <remarks></remarks>
    Public Overrides Sub SpecifyGroup(ByVal aSpec As String, ByVal aOpnCount As Integer)
        Dim lArr() As String = aSpec.Split(Delimiter)

        If lArr.Length < 2 Then Return
        If String.IsNullOrEmpty(lArr(0)) Then Return

        Dim lListBatchUnits As atcCollection = ListBatchOpns.ItemByKey(aOpnCount)
        If lListBatchUnits Is Nothing Then
            lListBatchUnits = New atcCollection()
            ListBatchOpns.Add(aOpnCount, lListBatchUnits)
        End If

        Select Case lArr(0).ToLower
            Case "station"
                Dim lArrStation() As String = lArr(1).Split(",")
                Dim lStationId As String = lArrStation(0).Trim()
                Dim lStation As clsBatchUnitStation = lListBatchUnits.ItemByKey(lStationId)
                If lStation Is Nothing Then
                    lStation = New clsBatchUnitStation()
                    lStation.StationID = lStationId
                    lListBatchUnits.Add(lStationId, lStation)
                End If
                If lArrStation.Length >= 2 Then
                    Dim lStationDA As Double
                    If Double.TryParse(lArrStation(1).Trim(), lStationDA) AndAlso lStationDA > 0 Then
                        lStation.StationDrainageArea = lStationDA
                    End If
                End If
                If lArrStation.Length >= 3 Then
                    Dim lStationDatafilename As String = lArrStation(2).Trim()
                    If Not String.IsNullOrEmpty(lStationDatafilename) AndAlso IO.File.Exists(lStationDatafilename) Then
                        lStation.StationDataFilename = lStationDatafilename
                        lStation.NeedToDownloadData = False
                        If Not ListBatchUnitsData.Keys.Contains(lStationId) Then
                            ListBatchUnitsData.Add(lStationId, lStationDatafilename)
                        End If
                    End If
                End If
                'Case InputNames.StartDate.ToLower
                '    Dim lDates(5) As Integer
                '    Dim lDate As DateTime
                '    If Date.TryParse(lArr(1), lDate) Then
                '        lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
                '        lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
                '        lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
                '        lDates(3) = 0
                '        lDates(4) = 0
                '        lDates(5) = 0
                '        For Each lStation As clsBatchUnitStation In lListBatchUnits
                '            Dim lSDate() As Integer = lStation.BFInputs.GetValue(InputNames.StartDate)
                '            If lSDate Is Nothing Then
                '                lStation.BFInputs.Add(InputNames.StartDate, lDates)
                '            Else
                '                For I = 0 To 5
                '                    lSDate(I) = lDates(I)
                '                Next
                '            End If
                '        Next
                '    End If
                'Case InputNames.EndDate.ToLower
                '    Dim lDates(5) As Integer
                '    Dim lDate As DateTime
                '    If Date.TryParse(lArr(1), lDate) Then
                '        lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
                '        lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
                '        lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
                '        lDates(3) = 24
                '        lDates(4) = 0
                '        lDates(5) = 0
                '        For Each lStation As clsBatchUnitStation In lListBatchUnits
                '            Dim lEDate() As Integer = lStation.BFInputs.GetValue(InputNames.EndDate)
                '            If lEDate Is Nothing Then
                '                lStation.BFInputs.Add(InputNames.EndDate, lDates)
                '            Else
                '                For I = 0 To 5
                '                    lEDate(I) = lDates(I)
                '                Next
                '            End If
                '        Next
                '    End If
            Case InputNames.Method.ToLower
                Dim lMethod As InputNames.ITAMethod = 0
                Select Case lArr(1).Trim().ToUpper()
                    Case InputNames.ITAMethod.FREQUECYGRID.ToString().ToUpper()
                        lMethod = InputNames.ITAMethod.FREQUECYGRID
                    Case InputNames.ITAMethod.FREQUENCYGRAPH.ToString().ToUpper()
                        lMethod = InputNames.ITAMethod.FREQUENCYGRAPH
                    Case InputNames.ITAMethod.NDAYTIMESERIES.ToString().ToUpper()
                        lMethod = InputNames.ITAMethod.NDAYTIMESERIES
                    Case InputNames.ITAMethod.TRENDLIST.ToString().ToUpper()
                        lMethod = InputNames.ITAMethod.TRENDLIST
                End Select
                If lMethod > 0 Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        Dim lMethods As atcCollection = lStation.BFInputs.GetValue(InputNames.Methods)
                        If lMethods Is Nothing Then
                            lMethods = New atcCollection()
                            lMethods.Add(lMethod)
                            lStation.BFInputs.Add(InputNames.Methods, lMethods)
                        Else
                            If Not lMethods.Contains(lMethod) Then
                                lMethods.Add(lMethod)
                            End If
                        End If
                    Next
                End If
            Case InputNames.OutputDir.ToLower
                Dim lOutputDir As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lOutputDir) AndAlso IO.Path.IsPathRooted(lOutputDir) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(InputNames.OutputDir, lOutputDir)
                    Next
                End If
            Case InputNames.OutputPrefix.ToLower
                Dim lOutputPrefix As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lOutputPrefix) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(InputNames.OutputPrefix, lOutputPrefix)
                    Next
                End If
            Case Else
                For Each lStation As clsBatchUnitStation In lListBatchUnits
                    If lStation.BFInputs.ContainsAttribute(lArr(0)) Then
                        lStation.BFInputs.SetValue(lArr(0), lArr(1))
                    Else
                        lStation.BFInputs.Add(lArr(0), lArr(1))
                    End If
                Next
        End Select
    End Sub

    Public Overrides Sub DoBatch()
        If Not String.IsNullOrEmpty(Message) AndAlso Message.ToLower.StartsWith("error") Then
            If RunModeConsole Then
                Console.WriteLine("Please address following issues before running batch:" & vbCrLf & Message,
                       "Batch Run SWSTAT")
            Else
                Logger.Msg("Please address following issues before running batch:" & vbCrLf & Message,
                       "Batch Run SWSTAT")
            End If
            Exit Sub
        Else
            Message = ""
        End If
        Dim lGlobalOutputDir As String = GlobalSettings.GetValue(InputNames.OutputDir, "")
        Dim lGlobalOutputDirReady As Boolean = True
        MkDirPath(lGlobalOutputDir)
        If Not IO.Directory.Exists(lGlobalOutputDir) Then
            If RunModeConsole Then
                Console.WriteLine("Can not create global output directory: " & vbCrLf & lGlobalOutputDir)
            Else
                UpdateStatus("Can not create global output directory: " & vbCrLf & lGlobalOutputDir, , True)
            End If
            'Exit Sub
            lGlobalOutputDirReady = False
        End If
        Dim lOutputDirWritable As Boolean = Util.IsDirectoryWritable(lGlobalOutputDir)
        If Not lOutputDirWritable Then
            'RaiseEvent StatusUpdate("0,0,Can not write to output directory: " & vbCrLf & lOutputDir)
            'Windows.Forms.Application.DoEvents()
            If RunModeConsole Then
                If Not SilentConsole Then Console.WriteLine("Can not write to global output directory: " & vbCrLf & lGlobalOutputDir)
            Else
                UpdateStatus("Can not write to global output directory: " & vbCrLf & lGlobalOutputDir, , True)
            End If
            'Exit Sub
            lGlobalOutputDirReady = False
        End If

        Dim lTotalBFOpn As Integer = 0
        For Each lKey As Integer In ListBatchOpns.Keys
            For Each lstation As clsBatchUnitStation In ListBatchOpns.ItemByKey(lKey)
                lTotalBFOpn += 1
            Next
        Next
        If Not RunModeConsole Then
            gProgressBar.Minimum = 0
            gProgressBar.Maximum = lTotalBFOpn
            gProgressBar.Step = 1
        End If
        Dim lBFOpnCount As Integer = 0
        Dim lConfigFile As IO.StreamWriter = Nothing
        For Each lBFOpnId As Integer In ListBatchOpns.Keys
            Dim lBFOpn As atcCollection = ListBatchOpns.ItemByKey(lBFOpnId)
            Dim lOutputDir = lGlobalOutputDir
            Dim lBFOpnDirInGlobalDir As String = IO.Path.Combine(lOutputDir, "ITFA_Opn_" & lBFOpnId)
            If lGlobalOutputDirReady Then
                MkDirPath(lBFOpnDirInGlobalDir)
            End If

            'Build a TserGroup to do ITFA
            Dim lDataGroup As New atcTimeseriesGroup()
            For Each lStation As clsBatchUnitStation In lBFOpn
                'Dim lOutputPrefix As String = lStation.BFInputs.GetValue(InputNames.OutputPrefix, "")
                'If String.IsNullOrEmpty(lOutputPrefix) Then lOutputPrefix = "ITFA_" & lStation.StationID

                Dim linclude_provisional As String = lStation.BFInputs.GetValue(InputNames.IncludeProvisionalData, "")
                Dim lTsFlow As atcTimeseries = Nothing
                If String.Compare(linclude_provisional, "YES", True) = 0 Then
                    lTsFlow = GetStationStreamFlowData(lStation, ListBatchUnitsData, , True)
                Else
                    lTsFlow = GetStationStreamFlowData(lStation, ListBatchUnitsData)
                End If
                If lTsFlow IsNot Nothing Then
                    lDataGroup.Add(lTsFlow)
                Else
                    lStation.Message &= "Note: no flow data, hence no outputs." & vbCrLf
                End If
            Next
            If lDataGroup.Count > 0 Then
                Dim lStation0 As clsBatchUnitStation = lBFOpn(0)
                Dim lStation0OutputDir As String = lStation0.BFInputs.GetValue(InputNames.OutputDir)
                If Not String.IsNullOrEmpty(lStation0OutputDir) AndAlso IO.Path.IsPathRooted(lStation0OutputDir) Then
                    MkDirPath(lStation0OutputDir)
                End If
                If IO.Directory.Exists(lStation0OutputDir) Then
                    OutputDir = lStation0OutputDir
                Else
                    OutputDir = lBFOpnDirInGlobalDir
                End If
                Dim lInputArgs As atcDataAttributes = lStation0.BFInputs
                'lInputArgs.SetValue("Timeseries", lDataGroup)
                Dim lNDays(0) As Double
                Dim lReturnPeriods(0) As Double
                InputNames.GetNdayReturnPeriodAttributes(lInputArgs, lNDays, lReturnPeriods)
                If (lNDays IsNot Nothing AndAlso lNDays.Length > 0) AndAlso
                   (lReturnPeriods IsNot Nothing AndAlso lReturnPeriods.Length > 0) Then
                    OutputFilenameRoot = lStation0.BFInputs.GetValue(InputNames.OutputPrefix, "ITFA")
                    Dim lHighFlowFlagLabel As String = lStation0.BFInputs.GetValue(InputNames.HighLowText, "LOW")
                    Dim lHighFlowFlag As Boolean = True
                    If lHighFlowFlagLabel = "LOW" OrElse lHighFlowFlagLabel = "low" Then
                        lHighFlowFlag = False
                    End If
                    'MethodsLastDone = lStation0.BFInputs.GetValue(InputNames.ITAMethod.FREQUECYGRID)
                    Try
                        Dim lSW As IO.StreamWriter = Nothing
                        'Do NDay first
                        If lHighFlowFlag Then
                            Dim lNDayHighTserListing As String = InputNames.CalculateNDayTser(lDataGroup, lInputArgs, lNDays, True)
                            lSW = New IO.StreamWriter(IO.Path.Combine(OutputDir, "NDayHighFlowTimeseries.txt"), False)
                            lSW.WriteLine(lNDayHighTserListing)
                            lSW.Flush()
                            lSW.Close()
                            lSW = Nothing
                            If Not RunModeConsole Then
                                UpdateStatus(IO.Path.GetFileName(OutputDir) & "-Done NDay High flow Calculation.", True)
                            End If
                        Else
                            Dim lNDayLowTserListing As String = InputNames.CalculateNDayTser(lDataGroup, lInputArgs, lNDays, False)
                            lSW = New IO.StreamWriter(IO.Path.Combine(OutputDir, "NDayLowFlowTimeseries.txt"), False)
                            lSW.WriteLine(lNDayLowTserListing)
                            lSW.Flush()
                            lSW.Close()
                            lSW = Nothing
                            If Not RunModeConsole Then
                                UpdateStatus(IO.Path.GetFileName(OutputDir) & "-Done NDay Low flow Calculation.", True)
                            End If
                        End If

                        'Do Frequency second
                        If lHighFlowFlag Then
                            Dim lFreqGridHigh As String = InputNames.DoFrequencyGrid(lDataGroup, lInputArgs, lNDays, lReturnPeriods, True)
                            lSW = New IO.StreamWriter(IO.Path.Combine(OutputDir, "FrequencyGridHighFlow.txt"), False)
                            lSW.WriteLine(lFreqGridHigh)
                            lSW.Flush()
                            lSW.Close()
                            lSW = Nothing
                            If Not RunModeConsole Then
                                UpdateStatus(IO.Path.GetFileName(OutputDir) & "-Done Frequency High Flow Calculation.", True)
                            End If
                        Else
                            Dim lFreqGridLow As String = InputNames.DoFrequencyGrid(lDataGroup, lInputArgs, lNDays, lReturnPeriods, False)
                            lSW = New IO.StreamWriter(IO.Path.Combine(OutputDir, "FrequencyGridLowFlow.txt"), False)
                            lSW.WriteLine(lFreqGridLow)
                            lSW.Flush()
                            lSW.Close()
                            lSW = Nothing
                            If Not RunModeConsole Then
                                UpdateStatus(IO.Path.GetFileName(OutputDir) & "-Done Frequency Low Flow Calculation.", True)
                            End If
                        End If

                        'Dim lNDayValuesLowDone As Boolean = InputNames.CalculateNDayValues(lDataGroup, lInputArgs, lNDays, lReturnPeriods, False)
                        'Dim lFreqSource As atcFrequencyGridSource = New atcFrequencyGridSource(lDataGroup, lNDays, lReturnPeriods)
                        'Dim lCheckFreqSrcMsg As String = InputNames.CheckFreqGrid(lFreqSource)
                        'lSW = New IO.StreamWriter(IO.Path.Combine(OutputDir, "FrequencyGridReport.txt"), False)
                        'lSW.WriteLine(lFreqSource.CreateReport())
                        'lSW.Flush()
                        'lSW.Close()
                        'lSW = Nothing
                        'UpdateStatus(IO.Path.GetFileName(OutputDir) & "-Done frequency grid.", True)

                        'Do Trend analysis once for radioHigh.Checked once for not
                        If lHighFlowFlag Then
                            Dim lTrendHighGridText As String = InputNames.TrendAnalysis(lDataGroup, lInputArgs, lNDays, True)
                            lSW = New IO.StreamWriter(IO.Path.Combine(OutputDir, "TrendAnalysisHighFlowReport.txt"), False)
                            lSW.WriteLine(lTrendHighGridText)
                            lSW.Flush()
                            lSW.Close()
                            lSW = Nothing
                            lTrendHighGridText = ""
                            If Not RunModeConsole Then
                                UpdateStatus(IO.Path.GetFileName(OutputDir) & "-Done trend high flow analysis.", True)
                            End If
                        Else
                            Dim lTrendLowGridText = InputNames.TrendAnalysis(lDataGroup, lInputArgs, lNDays, False)
                            lSW = New IO.StreamWriter(IO.Path.Combine(OutputDir, "TrendAnalysisLowFlowReport.txt"), False)
                            lSW.WriteLine(lTrendLowGridText)
                            lSW.Flush()
                            lSW.Close()
                            lSW = Nothing
                            lTrendLowGridText = ""
                            If Not RunModeConsole Then
                                UpdateStatus(IO.Path.GetFileName(OutputDir) & "-Done trend low flow analysis.", True)
                            End If
                        End If

                        'Do freq graphs
                        If lHighFlowFlag Then
                            InputNames.DoFrequencyGraph(OutputDir, lDataGroup, lInputArgs, lNDays, lReturnPeriods, True)
                            If Not RunModeConsole Then
                                UpdateStatus(IO.Path.GetFileName(OutputDir) & "-Done frequency graph for high flow.", True)
                            End If
                        Else
                            InputNames.DoFrequencyGraph(OutputDir, lDataGroup, lInputArgs, lNDays, lReturnPeriods, False)
                            If Not RunModeConsole Then
                                UpdateStatus(IO.Path.GetFileName(OutputDir) & "-Done frequency graph for low flow.", True)
                            End If
                        End If
                        'ASCIICommon(lTsFlow)
                        'lStation.Message &= lStation.CalcBF.BF_Message.Trim()
                    Catch ex As Exception

                    End Try
                    'If lStationFoundData IsNot Nothing Then Exit For
                    lConfigFile = New IO.StreamWriter(IO.Path.Combine(OutputDir, "Config.txt"), False)
                    For Each lStation As clsBatchUnitStation In lBFOpn
                        Dim lDataFilename As String = ListBatchUnitsData.ItemByKey(lStation.StationID)
                        If String.IsNullOrEmpty(lDataFilename) Then
                            lDataFilename = lStation.StationDataFilename
                            If IO.File.Exists(lDataFilename) Then
                                ListBatchUnitsData.Add(lStation.StationID, lDataFilename)
                            End If
                        End If
                        lConfigFile.WriteLine("Station " & lStation.StationID & ", " & lStation.StationDrainageArea & ", " & lDataFilename)
                    Next
                    For Each lAttrib As atcDefinedValue In CType(lBFOpn.ItemByIndex(0), clsBatchUnitStation).BFInputs
                        Dim lName As String = lAttrib.Definition.Name
                        Select Case lName.ToLower()
                            Case "startdate", "enddate"
                                Dim lDates(5) As Integer
                                atcUtility.J2Date(lAttrib.Value, lDates)
                                If lName.ToLower() = "enddate" Then
                                    timcnv(lDates)
                                End If
                                lConfigFile.WriteLine(lName & ", " & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
                            Case Else
                                lConfigFile.WriteLine(lName & ", " & lAttrib.Value.ToString())
                        End Select
                    Next
                    lConfigFile.Flush()
                    lConfigFile.Close()
                    lConfigFile = Nothing
                End If

            End If

            'RaiseEvent StatusUpdate(lBFOpnCount & "," & lTotalBFOpn & "," & "Base-flow Separation for station: " & lStation.StationID & " (" & lBFOpnCount & " out of " & lTotalBFOpn & ")")
            lBFOpnCount += lBFOpn.Count
            If RunModeConsole Then
                If Not SilentConsole Then Console.WriteLine("SWSTAT Batch Run Group " & lBFOpnId & " (" & lBFOpnCount & " out of total of " & lTotalBFOpn & " analysis)")
            Else
                UpdateStatus("SWSTAT Batch Run Group " & lBFOpnId & " (" & lBFOpnCount & " out of total of " & lTotalBFOpn & " analysis)", True)
            End If

            'Free up memory
            For Z As Integer = 0 To lDataGroup.Count - 1
                For A As Integer = 0 To lDataGroup(Z).Attributes.Count - 1
                    With lDataGroup(Z).Attributes(A)
                        If .Arguments IsNot Nothing Then .Arguments.Clear()
                        If .Value.GetType().Name = "atcTimeseries" Then
                            .Value.Clear()
                        End If
                    End With
                Next
                lDataGroup(Z).Clear()
            Next

            For Each lStation As clsBatchUnitStation In lBFOpn
                lStation.Clear()
            Next
        Next

        If Not RunModeConsole Then
            gProgressBar.Minimum = gProgressBar.Maximum
            gProgressBar.PerformStep()
        End If

        Dim lSummary As New IO.StreamWriter(IO.Path.Combine(lGlobalOutputDir, "ITFA_Log_" & SafeFilename(DateTime.Now()) & ".txt"), False)
        For Each lBFOpnId As Integer In ListBatchOpns.Keys
            Dim lBFOpn As atcCollection = ListBatchOpns.ItemByKey(lBFOpnId)
            lSummary.WriteLine("Batch Run Group ***  " & lBFOpnId & "  ***")
            Dim lHasError As Boolean = False
            For Each lStation As clsBatchUnitStation In lBFOpn
                If Not String.IsNullOrEmpty(lStation.Message) Then
                    lSummary.WriteLine("---- Station: " & lStation.StationID & "----")
                    lSummary.WriteLine(lStation.Message)
                    lSummary.WriteLine("---------------------------------------------")
                    If lStation.Message.ToLower().Contains("error") Then lHasError = True
                End If
            Next
            If lHasError Then
                lSummary.WriteLine("End Batch Run Group " & lBFOpnId & ", Has errors.")
            Else
                lSummary.WriteLine("End Batch Run Group " & lBFOpnId & ", Successful")
            End If
        Next
        lSummary.Flush()
        lSummary.Close()
        lSummary = Nothing
        If RunModeConsole Then
            If Not SilentConsole Then Console.WriteLine("Batch Run Complete for " & lTotalBFOpn & " analyses in " & ListBatchOpns.Count & " groups.", True)
        Else
            UpdateStatus("Batch Run Complete for " & lTotalBFOpn & " analyses in " & ListBatchOpns.Count & " groups.", True)
        End If
    End Sub
End Class
