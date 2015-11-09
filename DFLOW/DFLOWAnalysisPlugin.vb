Imports atcData
Imports atcBatchProcessing
Imports atcUtility
Imports MapWinUtility

Public Class DFLOWAnalysisPlugin

    '
    ' Notes on behavior:
    '
    ' 1) Save method invokes batch operation
    '
    ' 2) Show method invokes interactive operation
    '
    ' 3) Show method logic:
    '    - Create instance of frmDFLOWResults
    '    - frmDFLOWResults.New method invokes atcDataManager.UserSelectData to get time series
    '    - frmDFLOWResults.New method then invokes frmDFLOWResults.UserSpecifyDFLOWArgs to get season, period, xQy, and xBy parameters
    '    - frmDFLOWResults.UserSpecifyDFLOWArgs creates an instance of frmDFLOWArgs and invokes frmDFLOWArgs.AskUser
    '
    ' 4) Transitions:
    '   "File|Select Data" brings up atcDataManager.UserSelectData
    '   "File|Specify Inputs" brings up frmDFLOWResults.UserSpecifyDFLOWArgs [showmodal?]

    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::DFLOW"
        End Get
    End Property

    Public Overrides ReadOnly Property Author() As String
        Get
            Return "LimnoTech"
        End Get
    End Property
    
    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)


        ' "BATCH" VERSION

        If aTimeseriesGroup IsNot Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            ' Do computation driven by contents of paramarry

            If UBound(aOption) > 0 Then DFLOWCalcs.fBioPeriod = aOption(0) Else DFLOWCalcs.fBioPeriod = 1
            If UBound(aOption) > 1 Then DFLOWCalcs.fBioYears = aOption(1) Else DFLOWCalcs.fBioYears = 3
            If UBound(aOption) > 2 Then DFLOWCalcs.fBioCluster = aOption(2) Else DFLOWCalcs.fBioCluster = 120
            If UBound(aOption) > 3 Then DFLOWCalcs.fBioExcursions = aOption(3) Else DFLOWCalcs.fBioExcursions = 5
            If UBound(aOption) > 4 Then DFLOWCalcs.fStartMonth = aOption(4) Else DFLOWCalcs.fStartMonth = 4
            If UBound(aOption) > 5 Then DFLOWCalcs.fStartDay = aOption(5) Else DFLOWCalcs.fStartDay = 1
            If UBound(aOption) > 6 Then DFLOWCalcs.fFirstYear = aOption(6) Else DFLOWCalcs.fFirstYear = 0
            If UBound(aOption) > 7 Then DFLOWCalcs.fLastYear = aOption(7) Else DFLOWCalcs.fLastYear = 0

            DFLOWCalcs.fEndDay = DFLOWCalcs.fStartDay - 1
            If DFLOWCalcs.fEndDay = 0 Then
                DFLOWCalcs.fEndMonth = DFLOWCalcs.fStartMonth - 1
                If DFLOWCalcs.fStartMonth = 0 Then DFLOWCalcs.fEndMonth = 12
                Dim pLastDayOfMonth() As Integer = {99, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
                DFLOWCalcs.fEndDay = pLastDayOfMonth(DFLOWCalcs.fEndMonth)
            End If

            DFLOWCalcs.fNonBioType.Clear()
            DFLOWCalcs.fNonBioType.Add(0)
            DFLOWCalcs.fAveragingPeriod = 7
            DFLOWCalcs.fReturnPeriod = 10

            Dim lForm As New frmDFLOWResults(aTimeseriesGroup)
            If lForm.DialogResult <> Windows.Forms.DialogResult.Cancel Then
                SaveFileString(aFileName, lForm.ToString)
            End If
        End If
    End Sub

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object

        'Start branching for batch mode
        Show = Nothing
        Dim lBatchTitle As String = "Batch Run DFLOW"
        Dim lChoice As String = Logger.MsgCustomOwned("Please choose analysis approach below:", _
                                                      "DFLOW", _
                                                      Nothing, _
                                                      New String() {"Interactive", "Batch File", "Batch Map"})
        If lChoice = "Batch File" Then
            Dim lfrmBatch As New frmBatch()
            lfrmBatch.Text = "DFLOW: Batch Run"
            lfrmBatch.ShowDialog()
            Return Nothing
        ElseIf lChoice = "Batch Map" Then
            Dim lStationsAreSelected As Boolean = False
            Dim lMapLayer As MapWindow.Interfaces.Layer = Nothing
            For Each lMapLayer In pMapWin.Layers
                If lMapLayer.Name.ToLower.Contains("nwis daily discharge stations") Then
                    If lMapLayer.SelectedShapes.NumSelected < 2 Then
                        Logger.Msg("Please select more than 1 stream gages for batch process." & vbCrLf & vbCrLf & _
                                   "Layer: " & lMapLayer.Name, lBatchTitle)
                    End If
                    lStationsAreSelected = True
                    Exit For
                End If
            Next
            If lStationsAreSelected Then
                Dim lSelectedStationIDs As New atcCollection()
                Dim lShp As New MapWinGIS.Shapefile()
                If lShp.Open(lMapLayer.FileName, Nothing) Then
                    Dim lFieldIndex As Integer = -99
                    Dim lFieldIndex_nm As Integer = -99
                    Dim lRecordIndex As Integer = 0
                    For I As Integer = 0 To lShp.NumFields - 1
                        If lShp.Field(I).Name.ToLower() = "site_no" Then
                            lFieldIndex = I
                            'Exit For
                        End If
                        If lShp.Field(I).Name.ToLower() = "station_nm" Then
                            lFieldIndex_nm = I
                        End If
                        If lFieldIndex >= 0 AndAlso lFieldIndex_nm >= 0 Then
                            Exit For
                        End If
                    Next
                    If lFieldIndex < 0 Then
                        Logger.Msg("NWIS daily flow layer lack station ID field.", lBatchTitle)
                        Return Nothing
                    End If
                    Dim lStationId As String = ""
                    Dim lStationNm As String = ""
                    For I As Integer = 0 To lMapLayer.SelectedShapes.NumSelected - 1
                        lRecordIndex = lMapLayer.SelectedShapes(I).ShapeIndex()
                        lStationId = lShp.CellValue(lFieldIndex, lRecordIndex).ToString()
                        lStationNm = lShp.CellValue(lFieldIndex_nm, lRecordIndex).ToString()
                        If Not lSelectedStationIDs.Keys.Contains(lStationId) Then
                            lSelectedStationIDs.Add(lStationId, lStationNm)
                        End If
                    Next
                End If
                If lSelectedStationIDs.Count > 0 Then
                    Logger.Msg("The batch is selecting the following stations for the batch run." & vbCrLf & _
                               StationListing(lSelectedStationIDs), _
                               lBatchTitle)
                End If
                Dim lfrmBatchMap As New frmBatchMap()
                lfrmBatchMap.Initiate(lSelectedStationIDs)
                lfrmBatchMap.ShowDialog()
                Return Nothing 'for now
            Else
                Logger.Msg("Could not find stream gage station map layer: NWIS Daily Discharge Stations.", lBatchTitle)
                'Return Nothing
            End If
        End If

        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup
        If lTimeseriesGroup Is Nothing Then lTimeseriesGroup = New atcTimeseriesGroup

        If lTimeseriesGroup.Count = 0 Then 'ask user to specify some Data
            atcDataManager.UserSelectData("Select Data For DFLOW Analysis", lTimeseriesGroup, Nothing, True)
        End If

        If lTimeseriesGroup.Count > 0 Then
            Dim lForm As New frmDFLOWResults(lTimeseriesGroup)
            If lForm.DialogResult <> Windows.Forms.DialogResult.Cancel Then
                lForm.Show()
            End If
        End If
        Return Nothing
    End Function

    Private Function StationListing(ByVal aList As atcCollection) As String
        Dim lStationList As New Text.StringBuilder()
        Dim lStationCount As Integer
        For lStationCount = 0 To aList.Count - 1
            If lStationCount > 9 Then
                Exit For
            End If
            Dim lStationId As String = aList.Keys(lStationCount)
            Dim lStationNm As String = aList.ItemByIndex(lStationCount)
            lStationList.AppendLine((lStationCount + 1) & ": " & lStationId & ", " & lStationNm)
        Next
        If aList.Count - 1 > lStationCount Then
            lStationList.AppendLine("Skip...")
            Dim lStationId As String = aList.Keys(aList.Count - 1)
            Dim lStationNm As String = aList.ItemByIndex(aList.Count - 1)
            lStationList.AppendLine(aList.Count & ": " & lStationId & ", " & lStationNm)
        End If
        Return lStationList.ToString()
    End Function

    Public Sub ComputexBy(ByVal aTimeseriesGroup As atcTimeseriesGroup, ByVal x As Integer, ByVal y As Integer)
        For Each lTimeSeries As atcTimeseries In aTimeseriesGroup

            Dim lNTimeSeries As atcTimeseries = lTimeSeries.Clone
            Dim n As Integer = lTimeSeries.numValues ' option base 1
            Debug.Print(lTimeSeries.Value(1), lTimeSeries.Value(n))
            If Double.IsNaN(lTimeSeries.Value(n)) Then

            End If

            ' N-day average
            ' Iterate

            lNTimeSeries.Clear()

        Next
    End Sub
End Class

