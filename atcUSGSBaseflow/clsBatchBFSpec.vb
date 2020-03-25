﻿Imports atcUtility
Imports atcData
Imports atcBatchProcessing
Imports MapWinUtility
Imports System.Windows.Forms

Public Class BFBatchInputNames
    'Public Shared SDATE As String = "SDATE"
    'Public Shared EDATE As String = "EDATE"
    'Public Shared STARTDATE As String = "StartDate"
    'Public Shared ENDDATE As String = "EndDate"
    Public Shared BFMethod As String = "BFMethod"
    'Public Shared BFMethods As String = "BFMethods"
    Public Shared ReportBy As String = "ReportBy"
    Public Shared ReportByCY As String = "CY"
    Public Shared ReportByWY As String = "WY"
    Public Shared ReportByCalendar As String = "Calendar"
    Public Shared ReportByWater As String = "Water"
    'Public Shared BFI_NDayScreen As String = "BFI_NDayScreen"
    'Public Shared BFI_TurnPtFrac As String = "BFI_TurnPtFrac"
    'Public Shared BFI_RecessConst As String = "BFI_RecessConst"
    Public Shared DataDir As String = "DataDir"
    Public Shared UseCache As String = "USECACHE"
    Public Shared OUTPUTDIR As String = "OUTPUTDIR"
    Public Shared OUTPUTPrefix As String = "OUTPUTPrefix"
    Public Shared SAVERESULT As String = "SAVERESULT"
    Public Shared FullSpanDuration As String = "FullSpanDuration"
    Public Shared Graph As String = "Graphs"

    Public Shared BFM_HYFX As String = "HYFX"
    Public Shared BFM_HYLM As String = "HYLM"
    Public Shared BFM_HYSL As String = "HYSL"
    Public Shared BFM_PART As String = "PART"
    Public Shared BFM_BFIS As String = "BFIS"
    Public Shared BFM_BFIM As String = "BFIM"
    Public Shared BFM_BFLOW As String = "DF1P" '"BFLOW"
    Public Shared BFM_TwoPRDF As String = "DF2P" '"TwoPRDF"

    'Public Shared STREAMFLOW As String = "Streamflow"
    ''' <summary>
    ''' !!!! Incomplete list of inputs !!!!
    ''' </summary>
    ''' <param name="aSpecialSet"></param>
    ''' <param name="aCommonSet"></param>
    ''' <remarks></remarks>
    Public Shared Sub BuildInputSet(ByRef aSpecialSet As atcDataAttributes, ByVal aCommonSet As atcDataAttributes)
        If aSpecialSet Is Nothing Then
            aSpecialSet = New atcDataAttributes()
        End If
        If aCommonSet Is Nothing Then
            With aSpecialSet
                If .GetValue(BFMethod) Is Nothing Then .SetValue(BFMethod, New atcCollection())
            End With
        Else
            With aSpecialSet
                If .GetValue(BFMethod) Is Nothing Then .SetValue(BFMethod, aCommonSet.GetValue(BFMethod, New atcCollection()))
                If .GetValue(OUTPUTDIR) Is Nothing Then .SetValue(OUTPUTDIR, aCommonSet.GetValue(OUTPUTDIR, ""))
                If .GetValue(OutputPrefix) Is Nothing Then .SetValue(OutputPrefix, aCommonSet.GetValue(OutputPrefix, ""))
                If .GetValue(DataDir) Is Nothing Then .SetValue(DataDir, aCommonSet.GetValue(DataDir, ""))
            End With
        End If
    End Sub
End Class

Public Class clsBatchBFSpec

    Public GlobalSettings As atcData.atcDataAttributes

    Public SpecFilename As String

    Public ListBatchBaseflowOpns As atcCollection
    Public ListBatchUnitsData As atcCollection 'of clsBatchUnitBF, station id/location as key

    Public Delimiter As String = vbTab

    Public Message As String

    Public DownloadDataDirectory As String = ""
    Public MessageTimeseries As String = ""
    Public MessageParameters As String = ""

    Public Event StatusUpdate(ByVal aMsg As String)
    Public gProgressBar As ProgressBar = Nothing
    Public gTextStatus As TextBox = Nothing

    Public DownloadStations As atcCollection

    Public Sub New(ByVal aProgressbar As ProgressBar, ByVal aTextField As TextBox)
        gProgressBar = aProgressbar
        gTextStatus = aTextField
    End Sub

    Public Sub New(ByVal aSpecFilename As String)
        SpecFilename = aSpecFilename
    End Sub

    Public Sub New()

    End Sub

    Public Sub PopulateScenarios()
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

                If lOneLine.StartsWith("BASE-FLOW") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("END BASE-FLOW") Then
                            lReachedEnd = True
                            lBaseflowOpnCounter += 1
                            Exit While
                        End If
                        SpecifyBaseFlow(lOneLine, lBaseflowOpnCounter)
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
    Private Sub MergeSpecs()
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
    Private Sub SpecifyGlobal(ByVal aSpec As String)
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
                    Case BFBatchInputNames.BFM_BFLOW
                        lMethod = atcTimeseriesBaseflow.BFMethods.BFLOW
                    Case BFBatchInputNames.BFM_TwoPRDF
                        lMethod = atcTimeseriesBaseflow.BFMethods.TwoPRDF
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
            Case atcTimeseriesBaseflow.BFInputNames.Reportby.ToLower
                Dim lReportBySpec As String = lArr(1).Trim().ToUpper()
                If lReportBySpec = BFBatchInputNames.ReportByWater.ToUpper() Then
                    GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.Reportby, atcTimeseriesBaseflow.BFInputNames.ReportbyWY)
                ElseIf lReportBySpec = BFBatchInputNames.ReportByCalendar.ToUpper() Then
                    GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.Reportby, atcTimeseriesBaseflow.BFInputNames.ReportbyCY)
                End If
            Case atcTimeseriesBaseflow.BFInputNames.FullSpanDuration.ToLower()
                Dim lDoDuration As String = lArr(1).Trim().ToUpper()
                If lDoDuration = "YES" Then
                    GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.FullSpanDuration, True)
                Else
                    GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.FullSpanDuration, False)
                End If
            Case atcTimeseriesBaseflow.BFInputNames.Graph.ToLower()
                Dim lGraphTypes As String = lArr(1).Trim().ToUpper()
                GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.Graph, lGraphTypes)
            Case Else
                GlobalSettings.Add(lArr(0), lArr(1))
        End Select
    End Sub

    ''' <summary>
    ''' Create list of BF Units
    ''' </summary>
    ''' <param name="aSpec"></param>
    ''' <remarks></remarks>
    Private Sub SpecifyBaseFlow(ByVal aSpec As String, ByVal aBaseflowOpnCount As Integer)
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
                    Case BFBatchInputNames.BFM_BFLOW
                        lMethod = atcTimeseriesBaseflow.BFMethods.BFLOW
                    Case BFBatchInputNames.BFM_TwoPRDF
                        lMethod = atcTimeseriesBaseflow.BFMethods.TwoPRDF
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
            Case atcTimeseriesBaseflow.BFInputNames.Reportby.ToLower
                Dim lReportBySpec As String = lArr(1).Trim().ToUpper()
                If lReportBySpec = BFBatchInputNames.ReportByWater.ToUpper() Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.Reportby, atcTimeseriesBaseflow.BFInputNames.ReportbyWY)
                    Next
                ElseIf lReportBySpec = BFBatchInputNames.ReportByCalendar.ToUpper() Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.Reportby, atcTimeseriesBaseflow.BFInputNames.ReportbyCY)
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
            Case atcTimeseriesBaseflow.BFInputNames.TwoParamEstMethod.ToLower()
                Dim lParamEstMethod As atcTimeseriesBaseflow.clsBaseflow2PRDF.ETWOPARAMESTIMATION = atcTimeseriesBaseflow.clsBaseflow2PRDF.ETWOPARAMESTIMATION.ECKHARDT
                If Not String.IsNullOrEmpty(lArr(1).Trim()) Then
                    Dim lMethodText As String = lArr(1).Trim().ToLower()
                    If lMethodText.StartsWith("custom") Then
                        lParamEstMethod = atcTimeseriesBaseflow.clsBaseflow2PRDF.ETWOPARAMESTIMATION.CUSTOM
                    End If
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.TwoParamEstMethod, lParamEstMethod)
                    Next
                End If
            Case atcTimeseriesBaseflow.BFInputNames.BFLOWFilter.ToLower()
                Dim lBeta As Double
                If Double.TryParse(lArr(1).Trim(), lBeta) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.BFLOWFilter, lBeta)
                    Next
                End If
            Case atcTimeseriesBaseflow.BFInputNames.TwoPRDFRC.ToLower()
                Dim lRC As Double
                If Double.TryParse(lArr(1).Trim(), lRC) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFRC, lRC)
                    Next
                End If
            Case atcTimeseriesBaseflow.BFInputNames.TwoPRDFBFImax.ToLower()
                Dim lBFImax As Double
                If Double.TryParse(lArr(1).Trim(), lBFImax) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFBFImax, lBFImax)
                    Next
                End If
            Case atcTimeseriesBaseflow.BFInputNames.FullSpanDuration.ToLower()
                Dim lDoDurationText As String = lArr(1).Trim().ToUpper()
                Dim lDoDuration As Boolean = False
                If lDoDurationText = "YES" Then
                    lDoDuration = True
                End If
                For Each lStation As clsBatchUnitStation In lListBatchUnits
                    lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.FullSpanDuration, lDoDuration)
                Next
            Case BFBatchInputNames.OUTPUTDIR.ToLower
                Dim lOutputDir As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lOutputDir) Then
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
            Case BFBatchInputNames.Graph.ToLower()
                Dim lAttribValue As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lAttribValue) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(BFBatchInputNames.Graph, lAttribValue)
                    Next
                End If
            Case Else
                Dim lAttribValue As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lAttribValue) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(lArr(0), lAttribValue)
                    Next
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Only call this at time BF is done
    ''' </summary>
    ''' <param name="aStation"></param>
    ''' <param name="aPreserve">if work off the original Ts or off a clone</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetStationStreamFlowData(ByVal aStation As clsBatchUnitStation,
                                              Optional ByVal aPreserve As Boolean = False,
                                              Optional ByVal aIncludeProvisional As Boolean = False) As atcTimeseries
        Dim lDataFilename As String = ""
        If ListBatchUnitsData.Keys.Contains(aStation.StationID) Then
            lDataFilename = ListBatchUnitsData.ItemByKey(aStation.StationID)
        Else
            Dim lDataDir As String = GlobalSettings.GetValue(BFBatchInputNames.DataDir, "")
            Dim lDataFilenameInCache As String = IO.Path.Combine(lDataDir, "NWIS\NWIS_discharge_" & aStation.StationID & ".rdb")
            Dim lUseCached As Boolean = GlobalSettings.GetValue(BFBatchInputNames.UseCache, False)
            If lUseCached AndAlso IO.File.Exists(lDataFilenameInCache) Then
                aStation.SiteInfoDir = BFBatchInputNames.DataDir
                aStation.StationDataFilename = lDataFilenameInCache
                aStation.ReadData()
                lDataFilename = aStation.StationDataFilename
            Else
                'first download data, then read it
                lDataDir = aStation.StationDataFilename
                If lDataDir = "" Then
                    lDataDir = GlobalSettings.GetValue(BFBatchInputNames.DataDir, "")
                    If lDataDir = "" Then
                        Dim lOutDirDiag As New System.Windows.Forms.FolderBrowserDialog()
                        lOutDirDiag.Description = "Select Directory to Save All Downloaded Streamflow Data"
                        If lOutDirDiag.ShowDialog = DialogResult.OK Then
                            lDataDir = lOutDirDiag.SelectedPath
                            GlobalSettings.SetValue(BFBatchInputNames.DataDir, lDataDir)
                        End If
                        If String.IsNullOrEmpty(lDataDir) Then
                            Return Nothing
                        End If
                    ElseIf Not IO.Directory.Exists(lDataDir) Then
                        Try
                            IO.Directory.CreateDirectory(lDataDir)
                        Catch ex As Exception
                            Return Nothing
                        End Try
                    End If
                End If

                aStation.SiteInfoDir = lDataDir
                aStation.DownloadData()
                lDataFilename = aStation.StationDataFilename
            End If
        End If

        Dim lDataReady As Boolean = False
        If aStation.DataSource Is Nothing Then
            aStation.DataSource = New atcTimeseriesRDB.atcTimeseriesRDB()
            If aStation.DataSource.Open(lDataFilename) Then
                lDataReady = True
            End If
        End If
        If Not lDataReady Then
            If aStation.DataSource.DataSets.Count > 0 Then
                lDataReady = True
            Else
                'Already opened in the project
                For Each lDataSource As atcDataSource In atcDataManager.DataSources
                    Dim lSpec As String = lDataSource.Specification
                    If String.Compare(lDataFilename.Trim(), lSpec.Trim(), True) = 0 Then
                        lDataReady = True
                        aStation.DataSource.DataSets.AddRange(lDataSource.DataSets)
                        Exit For
                    End If
                Next
            End If
        End If
        If lDataReady Then
            Dim lGroup As atcTimeseriesGroup = Nothing
            If aStation.DataSource.DataSets(0).Attributes.GetValue("Constituent", "") = "Streamflow" Then
                lGroup = aStation.DataSource.DataSets.FindData("Constituent", "Streamflow")
            ElseIf aStation.DataSource.DataSets(0).Attributes.GetValue("Constituent", "") = "FLOW" Then
                lGroup = aStation.DataSource.DataSets.FindData("Constituent", "FLOW")
            End If

            If lGroup IsNot Nothing AndAlso lGroup.Count > 0 Then
                Dim lTsFlow As atcTimeseries = Nothing
                If aPreserve Then
                    lTsFlow = lGroup(0)
                    'Return lGroup(0)
                Else
                    lTsFlow = lGroup(0).Clone()
                    'aStation.DataSource.Clear()
                    'aStation.DataSource = Nothing
                    'Return lTsFlow
                End If
                aStation.BFInputs.SetValue("StaNam", lTsFlow.Attributes.GetValue("StaNam", "Unknown"))
                If aIncludeProvisional Then
                    Return lTsFlow
                Else
                    If HasProvisionalValues(lTsFlow) Then
                        Dim lProvisionalTS As atcTimeseries = Nothing
                        Dim lNonProvisionalTS As atcTimeseries = Nothing
                        SplitProvisional(lTsFlow, lProvisionalTS, lNonProvisionalTS)
                        If lNonProvisionalTS IsNot Nothing Then
                            If lProvisionalTS IsNot Nothing Then
                                lProvisionalTS.Clear()
                                lProvisionalTS = Nothing
                            End If
                            Return lNonProvisionalTS
                        End If
                    Else
                        Return lTsFlow
                    End If
                End If
            End If
        End If
        Return Nothing
    End Function

    Private Function SetupOutputDirectory(ByVal aOutputDir As String) As Boolean
        If Not IO.Directory.Exists(aOutputDir) Then
            Try
                'Dim lDirInfo As New IO.DirectoryInfo(aOutputDir)
                'Dim ldSecurity As System.Security.AccessControl.DirectorySecurity = lDirInfo.GetAccessControl()
                MkDirPath(aOutputDir)
            Catch ex As Exception
                'RaiseEvent StatusUpdate("0,0,Cannot create output directory: " & vbCrLf & lOutputDir)
                UpdateStatus("Cannot create output directory: " & vbCrLf & aOutputDir, , True)
                Return False
            End Try
        End If
        Dim lOutputDirWritable As Boolean = True
        Try
            Dim lSW As IO.StreamWriter = Nothing
            Try
                lSW = New IO.StreamWriter(IO.Path.Combine(aOutputDir, "z.txt"), False)
                lSW.WriteLine("1")
                lSW.Flush()
                lSW.Close()
                lSW = Nothing
                IO.File.Delete(IO.Path.Combine(aOutputDir, "z.txt"))
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
            UpdateStatus("Can not write to output directory: " & vbCrLf & aOutputDir, , True)
            Return False
        End If
        Return True
    End Function

    Public Sub DoBatch()
        If Not String.IsNullOrEmpty(Message) AndAlso Message.ToLower.StartsWith("error") Then
            Logger.Msg("Please address following issues before running batch:" & vbCrLf & Message,
                       "Base-flow Separation Batch")
            Exit Sub
        Else
            Message = ""
        End If
        Dim lOutputDir As String = GlobalSettings.GetValue(BFBatchInputNames.OUTPUTDIR, "")
        If Not SetupOutputDirectory(lOutputDir) Then
            Exit Sub
        End If

        Dim lTotalStations As Integer = 0
        For Each lKey As Integer In ListBatchBaseflowOpns.Keys
            For Each lstation As clsBatchUnitStation In ListBatchBaseflowOpns.ItemByKey(lKey)
                lTotalStations += 1
            Next
        Next
        Dim lTotalBFOpn As Integer = ListBatchBaseflowOpns.Count
        gProgressBar.Minimum = 0
        gProgressBar.Maximum = lTotalStations
        gProgressBar.Step = 1
        Dim lBFOpnCount As Integer = 1
        Dim lConfigFile As IO.StreamWriter = Nothing
        For Each lBFOpnId As Integer In ListBatchBaseflowOpns.Keys
            Dim lBFOpn As atcCollection = ListBatchBaseflowOpns.ItemByKey(lBFOpnId)
            Dim lbatchUnitStation As clsBatchUnitStation = lBFOpn.ItemByIndex(0)
            Dim lBFOpnOutputDir As String = lbatchUnitStation.BFInputs.GetValue(BFBatchInputNames.OUTPUTDIR, "")
            Dim lBFOpnDir As String = IO.Path.Combine(lOutputDir, "BF_Opn_" & lBFOpnId)
            If String.IsNullOrEmpty(lBFOpnOutputDir) Then
                lBFOpnDir = IO.Path.Combine(lOutputDir, "BF_Opn_" & lBFOpnId)
            Else
                lBFOpnDir = IO.Path.Combine(lBFOpnOutputDir, "BF_Opn_" & lBFOpnId)
            End If
            MkDirPath(lBFOpnDir)

            For Each lStation As clsBatchUnitStation In lBFOpn
                Dim lOutputPrefix As String = lStation.BFInputs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
                If String.IsNullOrEmpty(lOutputPrefix) Then lOutputPrefix = "BF_" & lStation.StationID
                'each station has its own directory
                Dim lStationOutDir As String = IO.Path.Combine(lBFOpnDir, "Station_" & lStation.StationID)
                MkDirPath(lStationOutDir)
#If GISProvider = "DotSpatial" Then
                'could set aPreserve and aNonProvisional to True
                Dim lTsFlow As atcTimeseries = GetStationStreamFlowData(lStation)
#Else
                Dim lTsFlow As atcTimeseries = GetStationStreamFlowData(lStation)
#End If
                If lTsFlow IsNot Nothing Then
                    Try
                        Dim CalcBF As atcTimeseriesBaseflow.atcTimeseriesBaseflow = New atcTimeseriesBaseflow.atcTimeseriesBaseflow()
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
                        If CalcBF.Calculate("baseflow", lStation.BFInputs) Then
                            OutputDir = lStationOutDir
                            OutputFilenameRoot = lStation.BFInputs.GetValue(BFBatchInputNames.OUTPUTPrefix, "BF")
                            MethodsLastDone = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFMethods)
                            ASCIICommon(lTsFlow)
                        End If
                        lStation.Message &= CalcBF.BF_Message.Trim()
                    Catch ex As Exception
                        lStation.Message &= "Error: Base-flow separation and/or reporting failed." & vbCrLf
                    End Try
                Else
                    lStation.Message &= "Error: flow data is missing." & vbCrLf
                End If
                'RaiseEvent StatusUpdate(lBFOpnCount & "," & lTotalBFOpn & "," & "Base-flow Separation for station: " & lStation.StationID & " (" & lBFOpnCount & " out of " & lTotalBFOpn & ")")
                UpdateStatus("Base-flow Separation for station: " & lStation.StationID & " (" & lBFOpnCount & " out of " & lTotalStations & ")", True)
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
        For Each lBFOpnId As Integer In ListBatchBaseflowOpns.Keys
            Dim lBFOpn As atcCollection = ListBatchBaseflowOpns.ItemByKey(lBFOpnId)
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
        UpdateStatus("Base-flow Separation Complete for " & lTotalStations & " Stations in " & ListBatchBaseflowOpns.Count & " groups.", True)
    End Sub

    '{
    Public Sub DoBatchIntermittent()
        If Not String.IsNullOrEmpty(Message) AndAlso Message.ToLower.StartsWith("error") Then
#If GISProvider = "DotSpatial" Then
            Console.WriteLine("Please address following issues before running batch:" & vbCrLf & Message)
#Else
            Logger.Msg("Please address following issues before running batch:" & vbCrLf & Message,
                       "Base-flow Separation Batch")
#End If
            Exit Sub
        Else
            Message = ""
        End If
        Dim lOutputDir As String = GlobalSettings.GetValue(BFBatchInputNames.OUTPUTDIR, "")
        If Not SetupOutputDirectory(lOutputDir) Then
            Exit Sub
        End If

        Dim lTotalStations As Integer = 0
        For Each lKey As Integer In ListBatchBaseflowOpns.Keys
            For Each lstation As clsBatchUnitStation In ListBatchBaseflowOpns.ItemByKey(lKey)
                lTotalStations += 1
            Next
        Next
        Dim lTotalBFOpnGroups As Integer = ListBatchBaseflowOpns.Count
        'gProgressBar.Minimum = 0
        'gProgressBar.Maximum = lTotalBFOpn
        'gProgressBar.Step = 1
        Dim lDateFormat As New atcDateFormat()
        With lDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        Dim lBFOpnCount As Integer = 1
        Dim lConfigFile As IO.StreamWriter = Nothing
        Dim lDrainageArea As Double = 0.0
        '{Loop through each batch group, lBFOpn
        For Each lBFOpnId As Integer In ListBatchBaseflowOpns.Keys
#If GISProvider = "DotSpatial" Then
            Console.WriteLine("Batch Group: " & lBFOpnId)
#Else
            Logger.Status("Batch Group: " & lBFOpnId)
            Logger.Progress(lBFOpnCount, lTotalBFOpnGroups)
#End If
            Dim lBFOpn As atcCollection = ListBatchBaseflowOpns.ItemByKey(lBFOpnId)
            Dim lbatchUnitStation As clsBatchUnitStation = lBFOpn.ItemByIndex(0)
            Dim lBFOpnOutputDir As String = lbatchUnitStation.BFInputs.GetValue(BFBatchInputNames.OUTPUTDIR, "")
            Dim lBFOpnDir As String = IO.Path.Combine(lOutputDir, "BF_Opn_" & lBFOpnId)
            If String.IsNullOrEmpty(lBFOpnOutputDir) Then
                lBFOpnDir = IO.Path.Combine(lOutputDir, "BF_Opn_" & lBFOpnId)
            Else
                lBFOpnDir = IO.Path.Combine(lBFOpnOutputDir, "BF_Opn_" & lBFOpnId)
            End If
            MkDirPath(lBFOpnDir)

            Dim lArr() As String = Nothing
            Dim lGraphs As String = ""
            Using lProgressLevel As New ProgressLevel
                Dim lStationCtr As Integer = 1
                Dim lStationsCount As Integer = lBFOpn.Count
                '{stations within each lBFOpn
                For Each lStation As clsBatchUnitStation In lBFOpn
                    'UpdateStatus("Base-flow Separation for station: " & lStation.StationID & " (" & lBFOpnCount & " out of " & lTotalBFOpn & ")", True)
                    Logger.Status("Base-flow Separation for station: " & lStation.StationID)
                    Logger.Progress(lStationCtr, lStationsCount)
                    Dim lOutputPrefix As String = lStation.BFInputs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
                    If String.IsNullOrEmpty(lOutputPrefix) Then lOutputPrefix = "BF_" & lStation.StationID
                    'each station has its own directory
                    Dim lStationOutDir As String = IO.Path.Combine(lBFOpnDir, "Station_" & lStation.StationID)
                    MkDirPath(lStationOutDir)
                    Dim linclude_provisional As String = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.INCLUDE_PROVISIONAL_DATA, "")
                    Dim lTsFlow As atcTimeseries = Nothing
                    If linclude_provisional = "YES" OrElse linclude_provisional = "yes" Then
                        lTsFlow = GetStationStreamFlowData(lStation, , True)
                    Else
                        lTsFlow = GetStationStreamFlowData(lStation)
                    End If
                    If lTsFlow IsNot Nothing Then
                        Try
                            lDrainageArea = lTsFlow.Attributes.GetValue("Drainage Area", -99)
                            'Force use drainage area user specified in the batch run spec file (has to be positive value)
                            'Essentially, user specified positive DA value takes precedence over original streamflow time series's DA
                            If lDrainageArea <> lStation.StationDrainageArea AndAlso lStation.StationDrainageArea > 0 Then
                                lDrainageArea = lStation.StationDrainageArea
                            End If
                            'break up into continuous periods
                            Dim lDates() As Integer = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.StartDate)
                            Dim lAnalysis_Start As Double = Date2J(lDates)
                            lDates = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.EndDate)
                            Dim lAnalysis_End As Double = Date2J(lDates)
                            Dim lTserFullDateRange As New atcTimeseries(Nothing)
                            With lTserFullDateRange
                                .Dates = New atcTimeseries(Nothing)
                                .Dates.Values = NewDates(lAnalysis_Start, lAnalysis_End, atcTimeUnit.TUDay, 1)
                                .numValues = lTserFullDateRange.Dates.numValues
                                .SetInterval(atcTimeUnit.TUDay, 1)
                                For I As Integer = 1 To .numValues
                                    .Value(I) = -99.0
                                Next
                            End With

                            Dim lFlowStart As Double = lTsFlow.Dates.Value(0)
                            Dim lFlowEnd As Double = lTsFlow.Dates.Value(lTsFlow.numValues)
                            'Calculate overall RC and BFImax for the DF2P analysis using the full data record
                            'over the user specified analysis duration
                            Dim lMethods = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFMethods)
                            If lMethods.Contains(atcTimeseriesBaseflow.BFMethods.TwoPRDF) Then
                                Dim lParamEstMode = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.TwoParamEstMethod,
                                                                               atcTimeseriesBaseflow.clsBaseflow2PRDF.ETWOPARAMESTIMATION.CUSTOM)
                                If lParamEstMode = atcTimeseriesBaseflow.clsBaseflow2PRDF.ETWOPARAMESTIMATION.ECKHARDT Then
                                    Dim lTsAnalysis As atcTimeseries = SubsetByDate(lTsFlow, lAnalysis_Start, lAnalysis_End, Nothing)
                                    If lTsAnalysis IsNot Nothing AndAlso lTsAnalysis.Attributes.GetValue("Count Positive") > 31 Then
                                        Dim lDF2P As New atcTimeseriesBaseflow.clsBaseflow2PRDF()
                                        Try
                                            lDF2P.CalculateBFImax_RC(lTsAnalysis)
                                            lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFRC, lDF2P.RC)
                                            lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFBFImax, lDF2P.BFImax)
                                        Catch ex As Exception
                                            lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFRC, 0.978)
                                            lStation.BFInputs.SetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFBFImax, 0.8)
                                        End Try
                                    End If
                                End If
                            End If

                            'Examine full range Tser and flow Tser's duration
                            'ToDo: could prevent going forward at this point if determine 
                            'there is no flow data in the analysis duration.
                            'If TSerOverlap(lTserFullDateRange, lTsFlow) Then
                            'Else
                            'End If

                            Dim lTsAnalysisGroup As New atcTimeseriesGroup()
                            If lTsFlow.Attributes.GetValue("Count missing") > 0 Then
                                If lTsFlow.Attributes.GetValue("Point") Then
                                    lTsFlow.Attributes.SetValue("Point", False)
                                End If
                                Dim ctr As Integer = 1
                                For I As Integer = 1 To lTsFlow.numValues
                                    If Not Double.IsNaN(lTsFlow.Value(I)) Then
                                        lFlowStart = lTsFlow.Dates.Value(I - 1)
                                        While (I <= lTsFlow.numValues AndAlso Not Double.IsNaN(lTsFlow.Value(I)))
                                            I = I + 1
                                        End While
                                        lFlowEnd = lTsFlow.Dates.Value(I - 1) 'need to record the end of the last time step
                                        If (lFlowEnd - lFlowStart) >= 31 Then
                                            Dim lTs As atcTimeseries = SubsetByDate(lTsFlow, lFlowStart, lFlowEnd, Nothing)
                                            lTs.Attributes.SetValue("period", ctr)
                                            lTsAnalysisGroup.Add(ctr, lTs)
                                            ctr += 1
                                        End If
                                    End If
                                Next
                            Else
                                'cuz lTsFlow for a station has to be used later, the clone will be cleared after batch bf
                                lTsAnalysisGroup.Add(lTsFlow.Clone())
                            End If

                            Dim lTsFlowGroup As New atcTimeseriesGroup()
                            Dim CalcBF As atcTimeseriesBaseflow.atcTimeseriesBaseflow = New atcTimeseriesBaseflow.atcTimeseriesBaseflow()
                            For Each lTsChunk As atcTimeseries In lTsAnalysisGroup
                                lTsFlowGroup.Add(lTsChunk)
                                With lStation.BFInputs
                                    'Ensure user specified positive drainage area value is used for both streamflow and base-flow time series
                                    'for the calculation of depth
                                    For Each lTsz As atcTimeseries In lTsFlowGroup
                                        If lTsz.Attributes.GetValue("Location") = lStation.StationID Then
                                            If lTsz.Attributes.GetValue("Drainage Area") <> lStation.StationDrainageArea AndAlso lStation.StationDrainageArea > 0 Then
                                                lTsz.Attributes.SetValue("Drainage Area", lStation.StationDrainageArea)
                                            End If
                                        End If
                                    Next
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.Streamflow, lTsFlowGroup)
                                    lFlowStart = lTsChunk.Dates.Value(0)
                                    lFlowEnd = lTsChunk.Dates.Value(lTsChunk.numValues)
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.StartDate, lFlowStart) 'lAnalysis_Start) 'Date2J(lDates))
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.EndDate, lFlowEnd) 'lAnalysis_End) 'Date2J(lDates))
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.DrainageArea, lStation.StationDrainageArea)
                                    .SetValue("BatchRun", True)
                                End With
                                If CalcBF.Calculate("baseflow", lStation.BFInputs) Then
                                    OutputDir = lStationOutDir
                                    OutputFilenameRoot = lStation.BFInputs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
                                    Dim lCtr As Integer = lTsChunk.Attributes.GetValue("period", 0)
                                    If String.IsNullOrEmpty(OutputFilenameRoot) Then
                                        OutputFilenameRoot = "BF_period_" & lCtr.ToString()
                                    Else
                                        OutputFilenameRoot &= "_period_" & lCtr.ToString()
                                    End If
                                    MethodsLastDone = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFMethods)
                                    ASCIICommon(lTsChunk, lStation.BFInputs)
                                End If
                                lStation.Message &= CalcBF.BF_Message.Trim()
                                lTsFlowGroup.Clear()
                            Next 'period
                            'after the results are written, then merge, the intermittent time series can be cleared
                            'the lTsAnalysisGroup contains the chuncky or original time series' clone (if only 1 period), these will be cleared
                            Dim lBFReportGroups As atcDataAttributes = MergeBaseflowResults(lTserFullDateRange, lTsAnalysisGroup, "Daily", True)
                            With lBFReportGroups
                                .SetValue("AnalysisStart", lAnalysis_Start)
                                .SetValue("AnalysisEnd", lAnalysis_End)
                                .SetValue("Drainage Area", lDrainageArea)
                                .SetValue("ReportGroupsAvailable", True)
                                .SetValue("ReportFileSuffix", "fullspan")
                                .SetValue("ForFullSpan", True)
                                .SetValue(BFBatchInputNames.ReportBy, lStation.BFInputs.GetValue(BFBatchInputNames.ReportBy, "calendar"))
                                .SetValue(BFBatchInputNames.FullSpanDuration, lStation.BFInputs.GetValue(BFBatchInputNames.FullSpanDuration, False))

                                'passing in parameter information for common ascii report generation
                                .SetValue(atcTimeseriesBaseflow.BFInputNames.BFMethods, lMethods)
                                If lMethods.Contains(atcTimeseriesBaseflow.BFMethods.BFIStandard) Then
                                    Dim lFrac = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFITurnPtFrac, Double.NaN) '"BFIFrac"
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.BFITurnPtFrac, lFrac)
                                End If
                                If lMethods.Contains(atcTimeseriesBaseflow.BFMethods.BFIModified) Then
                                    Dim lK1Day = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFIRecessConst, Double.NaN) '"BFIK1Day"
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.BFIRecessConst, lK1Day) '"BFIK1Day"
                                End If
                                If lMethods.Contains(atcTimeseriesBaseflow.BFMethods.BFIStandard) OrElse lMethods.Contains(atcTimeseriesBaseflow.BFMethods.BFIModified) Then
                                    Dim lNDay = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFINDayScreen, Double.NaN) '"BFINDay"
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.BFINDayScreen, lNDay) '"BFINDay"
                                    Dim lBFIYearBasis As String = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFIReportby, "") '"BFIReportby"
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.BFIReportby, lBFIYearBasis) '"BFIReportby"
                                End If
                                If lMethods.Contains(atcTimeseriesBaseflow.BFMethods.BFLOW) Then
                                    Dim lalpha = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.BFLOWFilter, Double.NaN)
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.BFLOWFilter, lalpha)
                                End If
                                If lMethods.Contains(atcTimeseriesBaseflow.BFMethods.TwoPRDF) Then
                                    Dim lRC = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFRC, Double.NaN)
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFRC, lRC)
                                    Dim lBFImax = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFBFImax, Double.NaN)
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.TwoPRDFBFImax, lBFImax)
                                    Dim lDF2PMethod = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.TwoParamEstMethod, atcTimeseriesBaseflow.clsBaseflow2PRDF.ETWOPARAMESTIMATION.NONE)
                                    .SetValue(atcTimeseriesBaseflow.BFInputNames.TwoParamEstMethod, lDF2PMethod)
                                End If
                                .SetValue("OutputDir", lStationOutDir)
                                .SetValue("StaNam", lStation.BFInputs.GetValue("StaNam", "Unknown"))
                            End With
                            'Dim lTmpGroup As New atcTimeseriesGroup()
                            'lTmpGroup.Add(lTsFlow)
                            'lTmpGroup.Add(lTserFullDateRange)
                            Logger.Dbg("mem0: " & MemUsage())
                            Dim lTsFlowFullRange As atcTimeseries = MergeBaseflowTimeseries(lTserFullDateRange, lTsFlow, False, True)  'MergeTimeseries(lTmpGroup, True)
                            Logger.Dbg("mem1: " & MemUsage())
                            If lTsFlowFullRange IsNot Nothing AndAlso lTsFlowFullRange.Attributes.GetValue("count positive") > 0 Then
                                AdjustDatesOfReportingTimeseries(lTsFlowFullRange, lBFReportGroups)
                                Logger.Dbg("mem2: " & MemUsage())
                                ASCIICommon(lTsFlowFullRange, lBFReportGroups)
                                Logger.Dbg("mem3: " & MemUsage())
                                lGraphs = lStation.BFInputs.GetValue(atcTimeseriesBaseflow.BFInputNames.Graph, "")
                                If Not String.IsNullOrEmpty(lGraphs) Then
                                    lArr = lGraphs.Split(",")
                                    For Each lGraphType As String In lArr
                                        Select Case lGraphType.ToLower()
                                            Case "logtimeseries", "timeseries", "duration"
                                                DoBFGraphTimeseriesGWWatch(lGraphType, lTsFlowFullRange, lBFReportGroups)
                                        End Select
                                    Next
                                End If
                                Logger.Dbg("mem4: " & MemUsage())
                            Else
                                Try
                                    lDateFormat.Midnight24 = False
                                    Dim lAttr_From As String = lDateFormat.JDateToString(lAnalysis_Start)
                                    lDateFormat.Midnight24 = True
                                    Dim lAttr_To As String = lDateFormat.JDateToString(lAnalysis_End)
                                    lStation.Message &= "No flow data within the analysis duration (" & lAttr_From & "-" & lAttr_To & "), hence no fullspan outputs."
                                Catch ex As Exception
                                    lStation.Message &= "No flow data within the analysis duration, hence no fullspan outputs."
                                End Try
                            End If
                            lTsFlowFullRange.Clear()
                            lTsFlowFullRange = Nothing
                            Logger.Dbg("mem5: " & MemUsage())
                        Catch ex As Exception
                            lStation.Message &= vbCrLf & "Error: Base-flow separation and/or reporting failed:" & vbCrLf & ex.Message & vbCrLf
                            If ex.Message.Contains("out of range") Then
                                lStation.Message &= "(Suggestion: check analysis starting and ending dates)" & vbCrLf
                            End If
                        End Try
                    Else
                        lStation.Message &= "Note: no flow data, hence no outputs." & vbCrLf
                    End If
                    'RaiseEvent StatusUpdate(lBFOpnCount & "," & lTotalBFOpn & "," & "Base-flow Separation for station: " & lStation.StationID & " (" & lBFOpnCount & " out of " & lTotalBFOpn & ")")
                    'UpdateStatus("Base-flow Separation for station: " & lStation.StationID & " (" & lBFOpnCount & " out of " & lTotalBFOpn & ")", True)
                    lStationCtr += 1
                    Logger.Dbg("mem6: " & MemUsage())
                Next 'lStation}
            End Using
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
            lBFOpnCount += 1
        Next 'lBFOpnId}
        Dim lSummary As New IO.StreamWriter(IO.Path.Combine(lOutputDir, "BF_Log_" & SafeFilename(DateTime.Now()) & ".txt"), False)
        For Each lBFOpnId As Integer In ListBatchBaseflowOpns.Keys
            Dim lBFOpn As atcCollection = ListBatchBaseflowOpns.ItemByKey(lBFOpnId)
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
        'UpdateStatus("Base-flow Separation Complete for " & lTotalBFOpn & " Stations in " & ListBatchBaseflowOpns.Count & " groups.", True)
#If GISProvider = "DotSpatial" Then
        Console.WriteLine("Base-flow Separation Complete for " & lTotalStations & " Stations in " & ListBatchBaseflowOpns.Count & " groups.", MsgBoxStyle.Information, "Batch Run Base-flow Separation")
#Else
        Logger.Status("")
        Logger.Msg("Base-flow Separation Complete for " & lTotalStations & " Stations in " & ListBatchBaseflowOpns.Count & " groups.", MsgBoxStyle.Information, "Batch Run Base-flow Separation")
#End If
    End Sub '}

    Public Shared Function TSerOverlap(ByVal aFullTser As atcTimeseries, ByVal aPartialTser As atcTimeseries) As Boolean
        If aFullTser Is Nothing Then Return False
        If aPartialTser Is Nothing Then Return False

        Dim lCommonStart As Double
        Dim lCommonEnd As Double
        Dim lFirstStart As Double
        Dim lastEnd As Double
        Dim lPRGroup As New atcTimeseriesGroup()
        lPRGroup.Add(aFullTser)
        lPRGroup.Add(aPartialTser)
        Dim lHasCommon As Boolean = CommonDates(lPRGroup, lFirstStart, lastEnd, lCommonStart, lCommonEnd)
        If lHasCommon Then
            Return True
        Else
            Dim lFullStart As Double = aFullTser.Dates.Value(0)
            Dim lFullEnd As Double = aFullTser.Dates.Value(aFullTser.numValues)
            Dim lPartialStart As Double = aPartialTser.Dates.Value(0)
            Dim lPartialEnd As Double = aPartialTser.Dates.Value(aPartialTser.numValues)

            If lPartialEnd <= lFullStart OrElse lPartialStart >= lFullEnd Then
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Private Sub UpdateStatus(ByVal aMsg As String, Optional ByVal aAppend As Boolean = False, Optional ByVal aResetProgress As Boolean = False)
        If gProgressBar IsNot Nothing Then
            If aResetProgress Then
                gProgressBar.Minimum = 0
                gProgressBar.Maximum = 0
                gProgressBar.PerformStep()
                gProgressBar.Refresh()
            Else
                gProgressBar.PerformStep()
            End If
        End If
        If gTextStatus IsNot Nothing Then
            If aAppend Then
                gTextStatus.AppendText(aMsg & vbCrLf)
            Else
                gTextStatus.Text = aMsg
            End If
        End If
    End Sub

    Public Sub Clear()
        If ListBatchUnitsData IsNot Nothing Then ListBatchUnitsData.Clear()
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
