Imports atcUtility
Imports atcData
Imports atcTimeseriesBaseflow
Imports atcSWSTAT
Imports MapWinUtility

Public Class BatchInputNames
    'Public Shared SDATE As String = "SDATE"
    'Public Shared EDATE As String = "EDATE"
    'Public Shared STARTDATE As String = "StartDate"
    'Public Shared ENDDATE As String = "EndDate"
    Public Shared BFMethod As String = "BFMethod"
    'Public Shared BFMethods As String = "BFMethods"
    'Public Shared ReportBy As String = "ReportBy"
    Public Shared ReportByCY As String = "CY"
    Public Shared ReportByWY As String = "WY"
    'Public Shared BFI_NDayScreen As String = "BFI_NDayScreen"
    'Public Shared BFI_TurnPtFrac As String = "BFI_TurnPtFrac"
    'Public Shared BFI_RecessConst As String = "BFI_RecessConst"
    Public Shared DataDir As String = "DataDir"
    Public Shared UseCache As String = "USECACHE"
    Public Shared OUTPUTDIR As String = "OUTPUTDIR"
    Public Shared OUTPUTPrefix As String = "OUTPUTPrefix"
    Public Shared SAVERESULT As String = "SAVERESULT"

    Public Shared BFM_HYFX As String = "HYFX"
    Public Shared BFM_HYLM As String = "HYLM"
    Public Shared BFM_HYSL As String = "HYSL"
    Public Shared BFM_PART As String = "PART"
    Public Shared BFM_BFIS As String = "BFIS"
    Public Shared BFM_BFIM As String = "BFIM"

    'Public Shared STREAMFLOW As String = "Streamflow"
End Class

Public Class clsBatchSpec

    Public ListBatchOpns As atcCollection

    Public Sub New(ByVal aProgressbar As Windows.Forms.ProgressBar, ByVal aTextField As Windows.Forms.TextBox)
        gProgressBar = aProgressbar
        gTextStatus = aTextField
    End Sub

    Public Sub New(ByVal aSpecFilename As String)
        MyBase.New(aSpecFilename)
    End Sub

    Public Sub New()

    End Sub

    Public Overrides Sub PopulateScenarios()
        If IO.File.Exists(SpecFilename) Then
            If ListBatchOpns Is Nothing Then
                ListBatchOpns = New atcCollection()
            End If
            If ListBatchUnitsData Is Nothing Then
                ListBatchUnitsData = New atcCollection()
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
            Case atcTimeseriesBaseflow.BFInputNames.StartDate.ToLower
                Dim lDates(5) As Integer
                Dim lDate As DateTime
                If Date.TryParse(lArr(1), lDate) Then
                    lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
                    lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
                    lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
                    lDates(3) = 0
                    lDates(4) = 0
                    lDates(5) = 0
                    GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.StartDate, lDates)
                End If
            Case atcTimeseriesBaseflow.BFInputNames.EndDate.ToLower
                Dim lDates(5) As Integer
                Dim lDate As DateTime
                If Date.TryParse(lArr(1), lDate) Then
                    lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
                    lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
                    lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
                    lDates(3) = 24
                    lDates(4) = 0
                    lDates(5) = 0
                    GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.EndDate, lDates)
                End If
            Case BatchInputNames.BFMethod.ToLower
                Dim lMethods As atcCollection = GlobalSettings.GetValue(atcTimeseriesBaseflow.BFInputNames.BFMethods)

                Dim lMethod As atcTimeseriesBaseflow.BFMethods = 0
                Select Case lArr(1).ToUpper()
                    Case BatchInputNames.BFM_HYFX
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPFixed
                    Case BatchInputNames.BFM_HYLM
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPLocMin
                    Case BatchInputNames.BFM_HYSL
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPSlide
                    Case BatchInputNames.BFM_PART
                        lMethod = atcTimeseriesBaseflow.BFMethods.PART
                    Case BatchInputNames.BFM_BFIS
                        lMethod = atcTimeseriesBaseflow.BFMethods.BFIStandard
                    Case BatchInputNames.BFM_BFIM
                        lMethod = atcTimeseriesBaseflow.BFMethods.BFIModified
                End Select
                If lMethod > 0 Then
                    If lMethods Is Nothing Then
                        lMethods = New atcCollection()
                        'lMethods.Add(lArr(1))
                        lMethods.Add(lMethod)
                        GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.BFMethods, lMethods)
                    Else
                        'If Not lMethods.Contains(lArr(1)) Then
                        '    lMethods.Add(lArr(1))
                        'End If
                        If Not lMethods.Contains(lMethod) Then
                            lMethods.Add(lMethod)
                        End If
                    End If
                End If
            Case BatchInputNames.DataDir.ToLower
                If Not String.IsNullOrEmpty(lArr(1)) AndAlso IO.Directory.Exists(lArr(1)) Then
                    DownloadDataDirectory = lArr(1)
                End If
                GlobalSettings.Add(BatchInputNames.DataDir, lArr(1))
            Case BatchInputNames.UseCache.ToLower
                Dim lUseCache As Boolean = False
                If Not String.IsNullOrEmpty(lArr(1)) AndAlso lArr(1).ToLower = "yes" Then
                    lUseCache = True
                End If
                GlobalSettings.Add(BatchInputNames.UseCache, lUseCache)
            Case atcTimeseriesBaseflow.BFInputNames.BFIReportby.ToLower
                Dim lReportBySpec As String = lArr(1).Trim().ToUpper()
                If lReportBySpec = BatchInputNames.ReportByWY Then
                    GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.BFIReportby, atcTimeseriesBaseflow.BFInputNames.BFIReportbyWY)
                ElseIf lReportBySpec = BatchInputNames.ReportByCY Then
                    GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.BFIReportby, atcTimeseriesBaseflow.BFInputNames.BFIReportbyCY)
                End If
            Case Else
                GlobalSettings.Add(lArr(0), lArr(1))
        End Select
    End Sub

    ''' <summary>
    ''' Create list of BF Units
    ''' </summary>
    ''' <param name="aSpec"></param>
    ''' <remarks></remarks>
    Public Overrides Sub SpecifyGroup(ByVal aSpec As String, ByVal aBaseflowOpnCount As Integer)
        Dim lArr() As String = aSpec.Split(Delimiter)

        If lArr.Length < 2 Then Return
        If String.IsNullOrEmpty(lArr(0)) Then Return

        Dim lListBatchUnits As atcCollection = ListBatchOpns.ItemByKey(aBaseflowOpnCount)
        If lListBatchUnits Is Nothing Then
            lListBatchUnits = New atcCollection()
            ListBatchOpns.Add(aBaseflowOpnCount, lListBatchUnits)
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
            Case atcTimeseriesBaseflow.BFInputNames.StartDate.ToLower
                Dim lDates(5) As Integer
                Dim lDate As DateTime
                If Date.TryParse(lArr(1), lDate) Then
                    lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
                    lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
                    lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
                    lDates(3) = 0
                    lDates(4) = 0
                    lDates(5) = 0
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        Dim lSDate() As Integer = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.StartDate)
                        If lSDate Is Nothing Then
                            lStation.BFInputs.Add(atcTimeseriesBaseflow.BFInputNames.StartDate, lDates)
                        Else
                            For I = 0 To 5
                                lSDate(I) = lDates(I)
                            Next
                        End If
                    Next
                End If
            Case atcTimeseriesBaseflow.BFInputNames.EndDate.ToLower
                Dim lDates(5) As Integer
                Dim lDate As DateTime
                If Date.TryParse(lArr(1), lDate) Then
                    lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
                    lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
                    lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
                    lDates(3) = 24
                    lDates(4) = 0
                    lDates(5) = 0
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        Dim lEDate() As Integer = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.EndDate)
                        If lEDate Is Nothing Then
                            lStation.BFInputs.Add(atcTimeseriesBaseflow.BFInputNames.EndDate, lDates)
                        Else
                            For I = 0 To 5
                                lEDate(I) = lDates(I)
                            Next
                        End If
                    Next
                End If
            Case BatchInputNames.BFMethod.ToLower
                Dim lMethod As atcTimeseriesBaseflow.BFMethods = 0
                Select Case lArr(1).Trim().ToUpper()
                    Case BatchInputNames.BFM_HYFX
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPFixed
                    Case BatchInputNames.BFM_HYLM
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPLocMin
                    Case BatchInputNames.BFM_HYSL
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPSlide
                    Case BatchInputNames.BFM_PART
                        lMethod = atcTimeseriesBaseflow.BFMethods.PART
                    Case BatchInputNames.BFM_BFIS
                        lMethod = atcTimeseriesBaseflow.BFMethods.BFIStandard
                    Case BatchInputNames.BFM_BFIM
                        lMethod = atcTimeseriesBaseflow.BFMethods.BFIModified
                End Select
                If lMethod > 0 Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        Dim lMethods As atcCollection = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFMethods)
                        If lMethods Is Nothing Then
                            lMethods = New atcCollection()
                            lMethods.Add(lMethod)
                            lStation.BFInputs.Add(atcTimeseriesBaseflow.BFInputNames.BFMethods, lMethods)
                        Else
                            If Not lMethods.Contains(lMethod) Then
                                lMethods.Add(lMethod)
                            End If
                        End If
                    Next
                End If
            Case atcTimeseriesBaseflow.BFInputNames.BFIReportby.ToLower
                Dim lReportBySpec As String = lArr(1).Trim().ToUpper()
                If lReportBySpec = BatchInputNames.ReportByWY Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.BFIReportby, atcTimeseriesBaseflow.BFInputNames.BFIReportbyWY)
                    Next
                ElseIf lReportBySpec = BatchInputNames.ReportByCY Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.BFIReportby, atcTimeseriesBaseflow.BFInputNames.BFIReportbyCY)
                    Next
                End If
            Case atcTimeseriesBaseflow.BFInputNames.BFINDayScreen.ToLower
                Dim lplen As Double
                If Double.TryParse(lArr(1).Trim(), lplen) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.BFINDayScreen, lplen)
                    Next
                End If
            Case atcTimeseriesBaseflow.BFInputNames.BFITurnPtFrac.ToLower
                Dim lTurnPt As Double
                If Double.TryParse(lArr(1).Trim(), lTurnPt) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.BFITurnPtFrac, lTurnPt)
                    Next
                End If
            Case atcTimeseriesBaseflow.BFInputNames.BFIRecessConst.ToLower
                Dim lReConst As Double
                If Double.TryParse(lArr(1).Trim(), lReConst) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.BFIRecessConst, lReConst)
                    Next
                End If
            Case BatchInputNames.OUTPUTDIR.ToLower
                Dim lOutputDir As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lOutputDir) AndAlso IO.Directory.Exists(lOutputDir) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(BatchInputNames.OUTPUTDIR, lOutputDir)
                    Next
                End If
            Case BatchInputNames.OUTPUTPrefix.ToLower
                Dim lOutputPrefix As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lOutputPrefix) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(BatchInputNames.OUTPUTPrefix, lOutputPrefix)
                    Next
                End If
        End Select
    End Sub

    Public Overrides Sub DoBatch()
        If Not String.IsNullOrEmpty(Message) AndAlso Message.ToLower.StartsWith("error") Then
            Logger.Msg("Please address following issues before running batch:" & vbCrLf & Message, _
                       "Base-flow Separation Batch")
            Exit Sub
        Else
            Message = ""
        End If
        Dim lOutputDir As String = GlobalSettings.GetValue(BatchInputNames.OUTPUTDIR, "")
        If Not IO.Directory.Exists(lOutputDir) Then
            Try
                Dim lDirInfo As New IO.DirectoryInfo(lOutputDir)
                Dim ldSecurity As System.Security.AccessControl.DirectorySecurity = lDirInfo.GetAccessControl()
                MkDirPath(lOutputDir)
            Catch ex As Exception
                'RaiseEvent StatusUpdate("0,0,Cannot create output directory: " & vbCrLf & lOutputDir)
                UpdateStatus("Cannot create output directory: " & vbCrLf & lOutputDir, , True)
                Exit Sub
            End Try
        End If
        Dim lOutputDirWritable As Boolean = True
        Try
            Dim lSW As IO.StreamWriter = Nothing
            Try
                lSW = New IO.StreamWriter(IO.Path.Combine(lOutputDir, "z.txt"), False)
                lSW.WriteLine("1")
                lSW.Flush()
                lSW.Close()
                lSW = Nothing
                IO.File.Delete(IO.Path.Combine(lOutputDir, "z.txt"))
            Catch ex As Exception
                If lSW IsNot Nothing Then
                    lSW.Close()
                    lSW = Nothing
                End If
                lOutputDirWritable = False
            End Try
        Catch ex As Exception
            lOutputDirWritable = False
        End Try

        If Not lOutputDirWritable Then
            'RaiseEvent StatusUpdate("0,0,Can not write to output directory: " & vbCrLf & lOutputDir)
            'Windows.Forms.Application.DoEvents()
            UpdateStatus("Can not write to output directory: " & vbCrLf & lOutputDir, , True)
            Return
        End If

        Dim lTotalBFOpn As Integer = 0
        For Each lKey As Integer In ListBatchOpns.Keys
            For Each lstation As clsBatchUnitStation In ListBatchOpns.ItemByKey(lKey)
                lTotalBFOpn += 1
            Next
        Next
        gProgressBar.Minimum = 0
        gProgressBar.Maximum = lTotalBFOpn
        gProgressBar.Step = 1
        Dim lBFOpnCount As Integer = 1
        Dim lConfigFile As IO.StreamWriter = Nothing
        For Each lBFOpnId As Integer In ListBatchOpns.Keys
            Dim lBFOpn As atcCollection = ListBatchOpns.ItemByKey(lBFOpnId)
            Dim lBFOpnDir As String = IO.Path.Combine(lOutputDir, "BF_Opn_" & lBFOpnId)
            MkDirPath(lBFOpnDir)

            For Each lStation As clsBatchUnitStation In lBFOpn
                Dim lOutputPrefix As String = lStation.BFInputs.GetValue(BatchInputNames.OUTPUTPrefix, "")
                If String.IsNullOrEmpty(lOutputPrefix) Then lOutputPrefix = "BF_" & lStation.StationID
                'each station has its own directory
                Dim lStationOutDir As String = IO.Path.Combine(lBFOpnDir, "Station_" & lStation.StationID)
                MkDirPath(lStationOutDir)
                Dim lTsFlow As atcTimeseries = GetStationStreamFlowData(lStation, ListBatchUnitsData)
                If lTsFlow IsNot Nothing Then
                    Try
                        lStation.CalcBF = New atcTimeseriesBaseflow.atcTimeseriesBaseflow()
                        Dim lTsFlowGroup As New atcTimeseriesGroup()
                        lTsFlowGroup.Add(lTsFlow)
                        With lStation.BFInputs
                            .SetValue(atcTimeseriesBaseflow.BFInputNames.Streamflow, lTsFlowGroup)
                            Dim lDates() As Integer = .GetValue(atcTimeseriesBaseflow.BFInputNames.StartDate)
                            .SetValue(atcTimeseriesBaseflow.BFInputNames.StartDate, Date2J(lDates))
                            lDates = .GetValue(atcTimeseriesBaseflow.BFInputNames.EndDate)
                            .SetValue(atcTimeseriesBaseflow.BFInputNames.EndDate, Date2J(lDates))
                            .SetValue(atcTimeseriesBaseflow.BFInputNames.DrainageArea, lStation.StationDrainageArea)
                            .SetValue("BatchRun", True)
                        End With
                        If lStation.CalcBF.Open("baseflow", lStation.BFInputs) Then
                            OutputDir = lStationOutDir
                            OutputFilenameRoot = lStation.BFInputs.GetValue(BatchInputNames.OUTPUTPrefix, "BF")
                            MethodsLastDone = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFMethods)
                            ASCIICommon(lTsFlow)
                        End If
                        lStation.Message &= lStation.CalcBF.BF_Message.Trim()
                    Catch ex As Exception
                        lStation.Message &= "Error: Base-flow separation and/or reporting failed." & vbCrLf
                    End Try
                Else
                    lStation.Message &= "Error: flow data is missing." & vbCrLf
                End If
                'RaiseEvent StatusUpdate(lBFOpnCount & "," & lTotalBFOpn & "," & "Base-flow Separation for station: " & lStation.StationID & " (" & lBFOpnCount & " out of " & lTotalBFOpn & ")")
                UpdateStatus("Base-flow Separation for station: " & lStation.StationID & " (" & lBFOpnCount & " out of " & lTotalBFOpn & ")", True)
                lBFOpnCount += 1
            Next
            'If lStationFoundData IsNot Nothing Then Exit For
            lConfigFile = New IO.StreamWriter(IO.Path.Combine(lBFOpnDir, "Config.txt"), False)
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
        Next
        Dim lSummary As New IO.StreamWriter(IO.Path.Combine(lOutputDir, "BF_Log_" & SafeFilename(DateTime.Now()) & ".txt"), False)
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
        UpdateStatus("Base-flow Separation Complete for " & lTotalBFOpn & " Stations in " & ListBatchOpns.Count & " groups.", True)
    End Sub

    Public Overrides Function ParametersToText(ByVal aMethod As ANALYSIS, ByVal aArgs As atcDataAttributes) As String
        Dim lText As String = ""
        Select Case aMethod
            Case ANALYSIS.ITA
                lText = ParametersToTextSWSTAT(aArgs)
            Case ANALYSIS.DFLOW
                lText = ParametersToTextDFLOW(aArgs)
        End Select
        Return lText
    End Function

    Private Function ParametersToTextDFLOW(ByVal aArgs As atcDataAttributes) As String
        'If aArgs Is Nothing Then Return ""
        'Dim loperation As String = aArgs.GetValue("Operation", "")
        'Dim lgroupname As String = aArgs.GetValue("Group", "")
        'Dim lSetGlobal As Boolean = (loperation.ToLower = "globalsetparm")

        'Dim lText As New Text.StringBuilder()
        'If loperation.ToLower = "groupsetparm" Then
        '    lText.AppendLine("BASE-FLOW")
        '    Dim lStationInfo As ArrayList = aArgs.GetValue(InputNames.StationInfo)
        '    If lStationInfo IsNot Nothing Then
        '        For Each lstation As String In lStationInfo
        '            lText.AppendLine(lstation)
        '        Next
        '    End If
        'ElseIf lSetGlobal Then
        '    lText.AppendLine("GLOBAL")
        'End If

        'Dim lStartDate As Double = aArgs.GetValue(BFInputNames.StartDate, Date2J(2014, 8, 20, 0, 0, 0))
        'Dim lEndDate As Double = aArgs.GetValue(BFInputNames.EndDate, Date2J(2014, 8, 20, 24, 0, 0))
        'Dim lDates(5) As Integer
        'J2Date(lStartDate, lDates)
        'lText.AppendLine("STARTDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
        'J2Date(lEndDate, lDates)
        'timcnv(lDates)
        'lText.AppendLine("ENDDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))

        'If aArgs.ContainsAttribute(BFInputNames.BFMethods) Then
        '    Dim lMethods As ArrayList = aArgs.GetValue(BFInputNames.BFMethods)
        '    For Each lMethod As BFMethods In lMethods
        '        Select Case lMethod
        '            Case BFMethods.PART
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_PART)
        '            Case BFMethods.HySEPFixed
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYFX)
        '            Case BFMethods.HySEPLocMin
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYLM)
        '            Case BFMethods.HySEPSlide
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYSL)
        '            Case BFMethods.BFIStandard
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_BFIS)
        '            Case BFMethods.BFIModified
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_BFIM)
        '        End Select
        '    Next
        'ElseIf lSetGlobal Then
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_PART)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYFX)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYLM)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYSL)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_BFIS)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_BFIM)
        'End If

        'If aArgs.ContainsAttribute(BFInputNames.BFITurnPtFrac) Then
        '    Dim lBFITurnPtFrac As Double = aArgs.GetValue(BFInputNames.BFITurnPtFrac)
        '    lText.AppendLine(BFInputNames.BFITurnPtFrac & vbTab & lBFITurnPtFrac)
        'ElseIf lSetGlobal Then
        '    lText.AppendLine(BFInputNames.BFITurnPtFrac & vbTab & "0.9")
        'End If
        'If aArgs.ContainsAttribute(BFInputNames.BFINDayScreen) Then
        '    Dim lBFINDayScreen As Double = aArgs.GetValue(BFInputNames.BFINDayScreen)
        '    lText.AppendLine(BFInputNames.BFINDayScreen & vbTab & lBFINDayScreen)
        'ElseIf lSetGlobal Then
        '    lText.AppendLine(BFInputNames.BFINDayScreen & vbTab & "5")
        'End If
        'If aArgs.ContainsAttribute(BFInputNames.BFIRecessConst) Then
        '    Dim lBFIRecessConst As Double = aArgs.GetValue(BFInputNames.BFIRecessConst)
        '    lText.AppendLine(BFInputNames.BFIRecessConst & vbTab & lBFIRecessConst)
        'ElseIf lSetGlobal Then
        '    lText.AppendLine(BFInputNames.BFIRecessConst & vbTab & "0.97915")
        'End If

        'If aArgs.ContainsAttribute(BFInputNames.BFIReportby) Then
        '    Dim lBFIReportBy As String = aArgs.GetValue(BFInputNames.BFIReportby, "")
        '    Select Case lBFIReportBy
        '        Case BFInputNames.BFIReportbyCY
        '            lText.AppendLine(BFInputNames.BFIReportby & vbTab & BatchInputNames.ReportByCY)
        '        Case BFInputNames.BFIReportbyWY
        '            lText.AppendLine(BFInputNames.BFIReportby & vbTab & BatchInputNames.ReportByWY)
        '    End Select
        'ElseIf lSetGlobal Then
        '    lText.AppendLine(BFInputNames.BFIReportby & vbTab & BatchInputNames.ReportByCY)
        'End If

        'If lSetGlobal Then
        '    Dim lDatadir As String = aArgs.GetValue(BatchInputNames.DataDir, "")
        '    If lDatadir = "" Then
        '        lDatadir = txtDataDir.Text.Trim()
        '        If IO.Directory.Exists(lDatadir) Then
        '            lText.AppendLine(BatchInputNames.DataDir & vbTab & lDatadir)
        '        End If
        '    End If
        'End If

        'Dim lOutputDir As String = aArgs.GetValue(BatchInputNames.OUTPUTDIR, "")
        'Dim lOutputPrefix As String = aArgs.GetValue(BatchInputNames.OUTPUTPrefix, "")
        'lText.AppendLine(BatchInputNames.OUTPUTDIR & vbTab & lOutputDir)
        'lText.AppendLine(BatchInputNames.OUTPUTPrefix & vbTab & lOutputPrefix)

        'If loperation.ToLower = "groupsetparm" Then
        '    lText.AppendLine("END BASE-FLOW")
        'ElseIf lSetGlobal Then
        '    lText.AppendLine("END GLOBAL")
        'End If
        'Return lText.ToString()
    End Function

    Private Function ParametersToTextSWSTAT(ByVal aArgs As atcDataAttributes) As String
        If aArgs Is Nothing Then Return ""
        Dim loperation As String = aArgs.GetValue("Operation", "")
        Dim lgroupname As String = aArgs.GetValue("Group", "")
        Dim lSetGlobal As Boolean = (loperation.ToLower = "globalsetparm")

        Dim lText As New Text.StringBuilder()
        If loperation.ToLower = "groupsetparm" Then
            lText.AppendLine("SWSTAT")
            Dim lStationInfo As ArrayList = aArgs.GetValue(GlobalInputNames.StationsInfo)
            If lStationInfo IsNot Nothing Then
                For Each lstation As String In lStationInfo
                    lText.AppendLine(lstation)
                Next
            End If
        ElseIf lSetGlobal Then
            lText.AppendLine("GLOBAL")
        End If

        Dim lStartDate As Double = aArgs.GetValue(BFInputNames.StartDate, Date2J(2014, 8, 20, 0, 0, 0))
        Dim lEndDate As Double = aArgs.GetValue(BFInputNames.EndDate, Date2J(2014, 8, 20, 24, 0, 0))
        Dim lDates(5) As Integer
        J2Date(lStartDate, lDates)
        lText.AppendLine("STARTDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
        J2Date(lEndDate, lDates)
        timcnv(lDates)
        lText.AppendLine("ENDDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))

        If aArgs.ContainsAttribute(atcSWSTAT.modUtil.InputNames.Method) Then
            Dim lMethods As ArrayList = aArgs.GetValue(atcSWSTAT.modUtil.InputNames.Method)
            For Each lMethod As atcSWSTAT.modUtil.InputNames.ITAMethod In lMethods
                Select Case lMethod
                    Case atcSWSTAT.InputNames.ITAMethod.FREQUECYGRID
                        lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.FREQUECYGRID.ToString())
                    Case atcSWSTAT.InputNames.ITAMethod.FREQUENCYGRAPH
                        lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.FREQUENCYGRAPH.ToString())
                    Case atcSWSTAT.InputNames.ITAMethod.NDAYTIMESERIES
                        lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.NDAYTIMESERIES.ToString())
                    Case atcSWSTAT.InputNames.ITAMethod.TRENDLIST
                        lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.TRENDLIST.ToString())
                End Select
            Next
        ElseIf lSetGlobal Then
            lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.FREQUECYGRID.ToString())
            lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.FREQUENCYGRAPH.ToString())
            lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.NDAYTIMESERIES.ToString())
            lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.TRENDLIST.ToString())
        End If

        If lSetGlobal Then
            lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & "HIGH")
            Dim lHighStart As String = atcSWSTAT.InputNames.HighFlowSeasonStart
            Dim lHighEnd As String = atcSWSTAT.InputNames.HighFlowSeasonEnd
            lText.AppendLine(lHighStart & vbTab & aArgs.GetValue(lHighStart, ""))
            lText.AppendLine(lHighEnd & vbTab & aArgs.GetValue(lHighEnd, ""))
            lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & "LOW")
            Dim lLowStart As String = atcSWSTAT.InputNames.LowFlowSeasonStart
            Dim lLowEnd As String = atcSWSTAT.InputNames.LowFlowSeasonEnd
            lText.AppendLine(lLowStart & vbTab & aArgs.GetValue(lLowStart, ""))
            lText.AppendLine(lLowEnd & vbTab & aArgs.GetValue(lLowEnd, ""))
        ElseIf aArgs.ContainsAttribute(atcSWSTAT.InputNames.HighLow) Then
            Dim lCondition As String = aArgs.GetValue(atcSWSTAT.InputNames.HighLow)
            If Not String.IsNullOrEmpty(lCondition) Then
                If lCondition.ToLower().Contains("high") Then
                    lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & "HIGH")
                    Dim lHighStart As String = atcSWSTAT.InputNames.HighFlowSeasonStart
                    Dim lHighEnd As String = atcSWSTAT.InputNames.HighFlowSeasonEnd
                    lText.AppendLine(lHighStart & vbTab & aArgs.GetValue(lHighStart, ""))
                    lText.AppendLine(lHighEnd & vbTab & aArgs.GetValue(lHighEnd, ""))
                ElseIf lCondition.ToLower().Contains("low") Then
                    lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & "LOW")
                    Dim lLowStart As String = atcSWSTAT.InputNames.LowFlowSeasonStart
                    Dim lLowEnd As String = atcSWSTAT.InputNames.LowFlowSeasonEnd
                    lText.AppendLine(lLowStart & vbTab & aArgs.GetValue(lLowStart, ""))
                    lText.AppendLine(lLowEnd & vbTab & aArgs.GetValue(lLowEnd, ""))
                End If
            End If
            'lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & aArgs.GetValue(atcSWSTAT.InputNames.HighLow))
        End If
        'The high/low option will dictate the starting and ending dates

        If aArgs.ContainsAttribute(atcSWSTAT.InputNames.Logarithmic) Then
            Dim log As String = "YES"
            If Not aArgs.GetValue(atcSWSTAT.InputNames.Logarithmic) Then log = "NO"
            lText.AppendLine(atcSWSTAT.InputNames.Logarithmic & vbTab & log)
            'ElseIf lSetGlobal Then
            '    lText.AppendLine(atcSWSTAT.InputNames.Logarithmic & vbTab & "YES")
        End If

        If aArgs.ContainsAttribute(atcSWSTAT.InputNames.MultiNDayPlot) Then
            Dim mplot As String = "YES"
            If Not aArgs.GetValue(atcSWSTAT.InputNames.MultiNDayPlot) Then mplot = "NO"
            lText.AppendLine(atcSWSTAT.InputNames.MultiNDayPlot & vbTab & mplot)
            'ElseIf lSetGlobal Then
            '    lText.AppendLine(atcSWSTAT.InputNames.MultiNDayPlot & vbTab & "NO")
        End If

        If aArgs.ContainsAttribute(atcSWSTAT.InputNames.MultiStationPlot) Then
            Dim mplot As String = "YES"
            If Not aArgs.GetValue(atcSWSTAT.InputNames.MultiStationPlot) Then mplot = "NO"
            lText.AppendLine(atcSWSTAT.InputNames.MultiStationPlot & vbTab & mplot)
            'ElseIf lSetGlobal Then
            '    lText.AppendLine(atcSWSTAT.InputNames.MultiStationPlot & vbTab & "NO")
        End If

        If aArgs.ContainsAttribute(atcSWSTAT.InputNames.NDays) Then
            Dim lNDays As atcCollection = aArgs.GetValue(atcSWSTAT.InputNames.NDays, Nothing)
            Dim lNdaysText As String = ""
            If lNDays IsNot Nothing Then
                For Each lNday As Double In lNDays.Keys
                    If lNDays.ItemByKey(lNday) Then
                        lNdaysText &= Int(lNday) & ","
                    End If
                Next
                lText.AppendLine(atcSWSTAT.InputNames.NDays & vbTab & lNdaysText.TrimEnd(","))
            End If
        End If

        If aArgs.ContainsAttribute(atcSWSTAT.InputNames.ReturnPeriods) Then
            Dim lRPs As atcCollection = aArgs.GetValue(atcSWSTAT.InputNames.ReturnPeriods, Nothing)
            Dim lRPsText As String = ""
            If lRPs IsNot Nothing Then
                For Each lRP As Double In lRPs.Keys
                    If lRPs.ItemByKey(lRP) Then
                        lRPsText &= lRP & ","
                    End If
                Next
                lText.AppendLine(atcSWSTAT.InputNames.ReturnPeriodText & vbTab & lRPsText.TrimEnd(","))
            End If
        End If

        If lSetGlobal Then
            Dim lDatadir As String = aArgs.GetValue(atcSWSTAT.InputNames.DataDir, "")
            If IO.Directory.Exists(lDatadir) Then
                lText.AppendLine(atcSWSTAT.InputNames.DataDir & vbTab & lDatadir)
            End If
        End If

        Dim lOutputDir As String = aArgs.GetValue(atcSWSTAT.InputNames.OutputDir, "")
        Dim lOutputPrefix As String = aArgs.GetValue(atcSWSTAT.InputNames.OutputPrefix, "")
        lText.AppendLine(atcSWSTAT.InputNames.OutputDir & vbTab & lOutputDir)
        lText.AppendLine(atcSWSTAT.InputNames.OutputPrefix & vbTab & lOutputPrefix)

        If loperation.ToLower = "groupsetparm" Then
            lText.AppendLine("END SWSTAT")
        ElseIf lSetGlobal Then
            lText.AppendLine("END GLOBAL")
        End If
        Return lText.ToString()
    End Function

    Private Function ParametersToTextBF(ByVal aArgs As atcDataAttributes) As String
        'If aArgs Is Nothing Then Return ""
        'Dim loperation As String = aArgs.GetValue("Operation", "")
        'Dim lgroupname As String = aArgs.GetValue("Group", "")
        'Dim lSetGlobal As Boolean = (loperation.ToLower = "globalsetparm")

        'Dim lText As New Text.StringBuilder()
        'If loperation.ToLower = "groupsetparm" Then
        '    lText.AppendLine("BASE-FLOW")
        '    Dim lStationInfo As ArrayList = aArgs.GetValue("StationInfo")
        '    If lStationInfo IsNot Nothing Then
        '        For Each lstation As String In lStationInfo
        '            lText.AppendLine(lstation)
        '        Next
        '    End If
        'ElseIf lSetGlobal Then
        '    lText.AppendLine("GLOBAL")
        'End If

        'Dim lStartDate As Double = aArgs.GetValue(BFInputNames.StartDate, Date2J(2014, 8, 20, 0, 0, 0))
        'Dim lEndDate As Double = aArgs.GetValue(BFInputNames.EndDate, Date2J(2014, 8, 20, 24, 0, 0))
        'Dim lDates(5) As Integer
        'J2Date(lStartDate, lDates)
        'lText.AppendLine("STARTDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
        'J2Date(lEndDate, lDates)
        'timcnv(lDates)
        'lText.AppendLine("ENDDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))

        'If aArgs.ContainsAttribute(BFInputNames.BFMethods) Then
        '    Dim lMethods As ArrayList = aArgs.GetValue(BFInputNames.BFMethods)
        '    For Each lMethod As BFMethods In lMethods
        '        Select Case lMethod
        '            Case BFMethods.PART
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_PART)
        '            Case BFMethods.HySEPFixed
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYFX)
        '            Case BFMethods.HySEPLocMin
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYLM)
        '            Case BFMethods.HySEPSlide
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYSL)
        '            Case BFMethods.BFIStandard
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_BFIS)
        '            Case BFMethods.BFIModified
        '                lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_BFIM)
        '        End Select
        '    Next
        'ElseIf lSetGlobal Then
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_PART)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYFX)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYLM)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_HYSL)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_BFIS)
        '    lText.AppendLine("BFMethod" & vbTab & BatchInputNames.BFM_BFIM)
        'End If

        'If aArgs.ContainsAttribute(BFInputNames.BFITurnPtFrac) Then
        '    Dim lBFITurnPtFrac As Double = aArgs.GetValue(BFInputNames.BFITurnPtFrac)
        '    lText.AppendLine(BFInputNames.BFITurnPtFrac & vbTab & lBFITurnPtFrac)
        'ElseIf lSetGlobal Then
        '    lText.AppendLine(BFInputNames.BFITurnPtFrac & vbTab & "0.9")
        'End If
        'If aArgs.ContainsAttribute(BFInputNames.BFINDayScreen) Then
        '    Dim lBFINDayScreen As Double = aArgs.GetValue(BFInputNames.BFINDayScreen)
        '    lText.AppendLine(BFInputNames.BFINDayScreen & vbTab & lBFINDayScreen)
        'ElseIf lSetGlobal Then
        '    lText.AppendLine(BFInputNames.BFINDayScreen & vbTab & "5")
        'End If
        'If aArgs.ContainsAttribute(BFInputNames.BFIRecessConst) Then
        '    Dim lBFIRecessConst As Double = aArgs.GetValue(BFInputNames.BFIRecessConst)
        '    lText.AppendLine(BFInputNames.BFIRecessConst & vbTab & lBFIRecessConst)
        'ElseIf lSetGlobal Then
        '    lText.AppendLine(BFInputNames.BFIRecessConst & vbTab & "0.97915")
        'End If

        'If aArgs.ContainsAttribute(BFInputNames.BFIReportby) Then
        '    Dim lBFIReportBy As String = aArgs.GetValue(BFInputNames.BFIReportby, "")
        '    Select Case lBFIReportBy
        '        Case BFInputNames.BFIReportbyCY
        '            lText.AppendLine(BFInputNames.BFIReportby & vbTab & BatchInputNames.ReportByCY)
        '        Case BFInputNames.BFIReportbyWY
        '            lText.AppendLine(BFInputNames.BFIReportby & vbTab & BatchInputNames.ReportByWY)
        '    End Select
        'ElseIf lSetGlobal Then
        '    lText.AppendLine(BFInputNames.BFIReportby & vbTab & BatchInputNames.ReportByCY)
        'End If

        'If lSetGlobal Then
        '    Dim lDatadir As String = aArgs.GetValue(BatchInputNames.DataDir, "")
        '    If lDatadir = "" Then
        '        lDatadir = txtDataDir.Text.Trim()
        '        If IO.Directory.Exists(lDatadir) Then
        '            lText.AppendLine(BatchInputNames.DataDir & vbTab & lDatadir)
        '        End If
        '    End If
        'End If

        'Dim lOutputDir As String = aArgs.GetValue(BatchInputNames.OUTPUTDIR, "")
        'Dim lOutputPrefix As String = aArgs.GetValue(BatchInputNames.OUTPUTPrefix, "")
        'lText.AppendLine(BatchInputNames.OUTPUTDIR & vbTab & lOutputDir)
        'lText.AppendLine(BatchInputNames.OUTPUTPrefix & vbTab & lOutputPrefix)

        'If loperation.ToLower = "groupsetparm" Then
        '    lText.AppendLine("END BASE-FLOW")
        'ElseIf lSetGlobal Then
        '    lText.AppendLine("END GLOBAL")
        'End If
        'Return lText.ToString()
    End Function

    Public Overrides Sub Clear()
        MyBase.Clear()
        If ListBatchOpns IsNot Nothing Then
            For Each lCollection As atcCollection In ListBatchOpns
                For Each lStation As clsBatchUnitStation In lCollection
                    If lStation.StreamFlowData IsNot Nothing Then
                        lStation.StreamFlowData.Clear()
                        lStation.StreamFlowData = Nothing
                    End If
                    lStation = Nothing
                Next
                lCollection.Clear()
                lCollection = Nothing
            Next
            ListBatchOpns.Clear()
        End If
    End Sub
End Class
