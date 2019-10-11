Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class BFBatchInputNames
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
    Inherits clsBatch
    Implements IBatchProcessing

    Public ListBatchBaseflowOpns As atcCollection

    Public Sub New(ByVal aProgressbar As System.Windows.Forms.ProgressBar, ByVal aTextField As System.Windows.Forms.TextBox)
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
            If ListBatchBaseflowOpns Is Nothing Then
                ListBatchBaseflowOpns = New atcCollection()
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
        For Each lBFOpn As atcCollection In ListBatchBaseflowOpns
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
        '    If lDirOpenDiag.ShowDialog = System.Windows.Forms.DialogResult.OK Then
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
        '    For Each lBFOpn As atcCollection In ListBatchBaseflowOpns
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
            Case BFBatchInputNames.BFMethod.ToLower
                Dim lMethods As atcCollection = GlobalSettings.GetValue(atcTimeseriesBaseflow.BFInputNames.BFMethods)

                Dim lMethod As atcTimeseriesBaseflow.BFMethods = 0
                Select Case lArr(1).ToUpper()
                    Case BFBatchInputNames.BFM_HYFX
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPFixed
                    Case BFBatchInputNames.BFM_HYLM
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPLocMin
                    Case BFBatchInputNames.BFM_HYSL
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPSlide
                    Case BFBatchInputNames.BFM_PART
                        lMethod = atcTimeseriesBaseflow.BFMethods.PART
                    Case BFBatchInputNames.BFM_BFIS
                        lMethod = atcTimeseriesBaseflow.BFMethods.BFIStandard
                    Case BFBatchInputNames.BFM_BFIM
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
            Case BFBatchInputNames.DataDir.ToLower
                If Not String.IsNullOrEmpty(lArr(1)) AndAlso IO.Directory.Exists(lArr(1)) Then
                    DownloadDataDirectory = lArr(1)
                End If
                GlobalSettings.Add(BFBatchInputNames.DataDir, lArr(1))
            Case BFBatchInputNames.UseCache.ToLower
                Dim lUseCache As Boolean = False
                If Not String.IsNullOrEmpty(lArr(1)) AndAlso lArr(1).ToLower = "yes" Then
                    lUseCache = True
                End If
                GlobalSettings.Add(BFBatchInputNames.UseCache, lUseCache)
            Case atcTimeseriesBaseflow.BFInputNames.BFIReportby.ToLower
                Dim lReportBySpec As String = lArr(1).Trim().ToUpper()
                If lReportBySpec = BFBatchInputNames.ReportByWY Then
                    GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.BFIReportby, atcTimeseriesBaseflow.BFInputNames.BFIReportbyWY)
                ElseIf lReportBySpec = BFBatchInputNames.ReportByCY Then
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

        Dim lListBatchUnits As atcCollection = ListBatchBaseflowOpns.ItemByKey(aBaseflowOpnCount)
        If lListBatchUnits Is Nothing Then
            lListBatchUnits = New atcCollection()
            ListBatchBaseflowOpns.Add(aBaseflowOpnCount, lListBatchUnits)
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
            Case BFBatchInputNames.BFMethod.ToLower
                Dim lMethod As atcTimeseriesBaseflow.BFMethods = 0
                Select Case lArr(1).Trim().ToUpper()
                    Case BFBatchInputNames.BFM_HYFX
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPFixed
                    Case BFBatchInputNames.BFM_HYLM
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPLocMin
                    Case BFBatchInputNames.BFM_HYSL
                        lMethod = atcTimeseriesBaseflow.BFMethods.HySEPSlide
                    Case BFBatchInputNames.BFM_PART
                        lMethod = atcTimeseriesBaseflow.BFMethods.PART
                    Case BFBatchInputNames.BFM_BFIS
                        lMethod = atcTimeseriesBaseflow.BFMethods.BFIStandard
                    Case BFBatchInputNames.BFM_BFIM
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
                If lReportBySpec = BFBatchInputNames.ReportByWY Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.BFIReportby, atcTimeseriesBaseflow.BFInputNames.BFIReportbyWY)
                    Next
                ElseIf lReportBySpec = BFBatchInputNames.ReportByCY Then
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
            Case BFBatchInputNames.OUTPUTDIR.ToLower
                Dim lOutputDir As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lOutputDir) AndAlso IO.Directory.Exists(lOutputDir) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(BFBatchInputNames.OUTPUTDIR, lOutputDir)
                    Next
                End If
            Case BFBatchInputNames.OUTPUTPrefix.ToLower
                Dim lOutputPrefix As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lOutputPrefix) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(BFBatchInputNames.OUTPUTPrefix, lOutputPrefix)
                    Next
                End If
        End Select
    End Sub

    Public Overrides Sub DoBatch()
    End Sub

    Public Overrides Function ParametersToText(ByVal aMethod As ANALYSIS, ByVal aArgs As atcDataAttributes) As String
        Dim lText As String = ""
        Select Case aMethod
            Case ANALYSIS.ITA
                'lText = ParametersToTextSWSTAT(aArgs)
            Case ANALYSIS.DFLOW
                'lText = ParametersToTextDFLOW(aArgs)
        End Select
        Return lText
    End Function

    'Private Function ParametersToTextBF(ByVal aArgs As atcDataAttributes) As String
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
    '                lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_PART)
    '            Case BFMethods.HySEPFixed
    '                lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYFX)
    '            Case BFMethods.HySEPLocMin
    '                lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYLM)
    '            Case BFMethods.HySEPSlide
    '                lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYSL)
    '            Case BFMethods.BFIStandard
    '                lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIS)
    '            Case BFMethods.BFIModified
    '                lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIM)
    '        End Select
    '    Next
    'ElseIf lSetGlobal Then
    '    lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_PART)
    '    lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYFX)
    '    lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYLM)
    '    lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYSL)
    '    lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIS)
    '    lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIM)
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
    '            lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByCY)
    '        Case BFInputNames.BFIReportbyWY
    '            lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByWY)
    '    End Select
    'ElseIf lSetGlobal Then
    '    lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByCY)
    'End If

    'If lSetGlobal Then
    '    Dim lDatadir As String = aArgs.GetValue(BFBatchInputNames.DataDir, "")
    '    If lDatadir = "" Then
    '        lDatadir = txtDataDir.Text.Trim()
    '        If IO.Directory.Exists(lDatadir) Then
    '            lText.AppendLine(BFBatchInputNames.DataDir & vbTab & lDatadir)
    '        End If
    '    End If
    'End If

    'Dim lOutputDir As String = aArgs.GetValue(BFBatchInputNames.OUTPUTDIR, "")
    'Dim lOutputPrefix As String = aArgs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
    'lText.AppendLine(BFBatchInputNames.OUTPUTDIR & vbTab & lOutputDir)
    'lText.AppendLine(BFBatchInputNames.OUTPUTPrefix & vbTab & lOutputPrefix)

    'If loperation.ToLower = "groupsetparm" Then
    '    lText.AppendLine("END BASE-FLOW")
    'ElseIf lSetGlobal Then
    '    lText.AppendLine("END GLOBAL")
    'End If
    'Return lText.ToString()
    'End Function

    Public Overrides Sub Clear()
        MyBase.Clear()
        If ListBatchBaseflowOpns IsNot Nothing Then
            For Each lCollection As atcCollection In ListBatchBaseflowOpns
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
            ListBatchBaseflowOpns.Clear()
        End If
    End Sub
End Class
