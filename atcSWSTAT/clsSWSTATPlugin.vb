Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class clsSWSTATPlugin
    Inherits atcData.atcDataDisplay

    Private Const pTrendName As String = "Trend"

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::USGS Surface Water Statistics (SWSTAT)::Integrated Frequency Analysis"
        End Get
    End Property

    Public Overrides ReadOnly Property Icon() As System.Drawing.Icon
        Get
            Dim lResources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSWSTAT))
            Return CType(lResources.GetObject("$this.Icon"), System.Drawing.Icon)
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
#If GISProvider = "DotSpatial" Then
#Else
        LoadPlugin("Timeseries::n-day high/low")
#End If

        'Start branching for batch mode
        Show = Nothing
        Dim lBatchTitle As String = "Batch Run SWSTAT"
        Dim lChoice As String = Logger.MsgCustomOwned("Please choose analysis approach below:", _
                                                      "Integrated Frequency Analysis", _
                                                      Nothing, _
                                                      New String() {"Interactive", "Batch File", "Batch Map"})
        If lChoice = "Batch File" Then
            Dim lfrmBatch As New frmBatch()
            ShowForm(lfrmBatch)
            Return lfrmBatch
        ElseIf lChoice = "Batch Map" Then
            Dim lStationsAreSelected As Boolean = False
#If GISProvider = "DotSpatial" Then
#Else
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
                    Logger.Msg("The batch is selecting the following stations for the batch run." & vbCrLf &
                               StationListing(lSelectedStationIDs),
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
#End If
        End If

        Dim lForm As New frmSWSTAT
        lForm.Initialize(aTimeseriesGroup, BasicAttributes, NDayAttributes, TrendAttributes)
        Return lForm
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

    Private Sub ShowForm(ByVal aForm As frmBatch)
        aForm.Initialize()
    End Sub

    Private Function BasicAttributes() As Generic.List(Of String)
        Dim lBasicAttributes As New Generic.List(Of String)
        With lBasicAttributes
            .Add("Location") '.Add("ID")
            .Add("Min")
            .Add("Max")
            .Add("Mean")
            .Add("Standard Deviation")
            .Add("Count")
            .Add("Count Missing")
        End With
        Return lBasicAttributes
    End Function

    Private Function NDayAttributes() As Generic.List(Of String)
        Dim lNDayAttributes As New Generic.List(Of String)
        With lNDayAttributes
            .Add("Location") '.Add("STAID")
            .Add("STANAM")
            .Add("Constituent")
        End With
        Return lNDayAttributes
    End Function

    Private Function TrendAttributes() As Generic.List(Of String)
        Dim lTrendAttributes As New Generic.List(Of String)
        With lTrendAttributes
            '.Add("Original ID")
            .Add("Location") '.Add("STAID")
            .Add("KENTAU")
            .Add("KENPLV")
            .Add("KENSLPL")
            .Add("From")
            .Add("To")
            .Add("Count")
            .Add("Not Used")
            .Add("Min")
            .Add("Max")
            .Add("Constituent")
        End With
        Return lTrendAttributes
    End Function


    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)

        If Not aTimeseriesGroup Is Nothing AndAlso aTimeseriesGroup.Count > 0 Then
#If GISProvider = "DotSpatial" Then
#Else
            LoadPlugin("Timeseries::n-day high/low")
#End If
            Dim lForm As New frmSWSTAT

            lForm.Initialize(aTimeseriesGroup)
            atcUtility.SaveFileString(aFileName, lForm.ToString)
            lForm.Dispose()
        End If
    End Sub
#If GISProvider = "DotSpatial" Then
#Else

    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
        pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_USGS Surface Water Statistics (SWSTAT)_" & pTrendName, _
                                                       atcDataManager.AnalysisMenuName & "_USGS Surface Water Statistics (SWSTAT)", pTrendName, Me.Icon, , , True))
    End Sub

    Private Sub LoadPlugin(ByVal aPluginName As String)
        Try
            Dim lKey As String = pMapWin.Plugins.GetPluginKey(aPluginName)
            'If Not g_MapWin.Plugins.PluginIsLoaded(lKey) Then 
            pMapWin.Plugins.StartPlugin(lKey)
        Catch e As Exception
            Logger.Dbg("Exception loading " & aPluginName & ": " & e.Message)
        End Try
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        MyBase.ItemClicked(aItemName, aHandled)
        If Not aHandled Then
            Select Case aItemName
                Case atcDataManager.AnalysisMenuName & "_USGS Surface Water Statistics (SWSTAT)_" & pTrendName
                    Dim lTimeseriesGroup As atcTimeseriesGroup = _
                      atcDataManager.UserSelectData("Select Data For Trend Analysis", _
                                                    Nothing, Nothing, True, True, Me.Icon)
                    If lTimeseriesGroup.Count > 0 Then
                        LoadPlugin("Timeseries::n-day high/low")
                        Dim lForm As New frmTrend
                        lForm.Initialize(lTimeseriesGroup, BasicAttributes, NDayAttributes, TrendAttributes)
                    End If

            End Select
        End If
    End Sub
#End If

    Public Shared Function ComputeRankedAnnualTimeseries(ByVal aTimeseriesGroup As atcTimeseriesGroup,
                                                         ByVal aNDay() As Double,
                                                         ByVal aHighFlag As Boolean,
                                                         ByVal aFirstYear As Integer,
                                                         ByVal aLastYear As Integer,
                                                         ByVal aBoundaryMonth As Integer,
                                                         ByVal aBoundaryDay As Integer,
                                                         ByVal aEndMonth As Integer,
                                                         ByVal aEndDay As Integer) As atcTimeseriesGroup
        Dim lArgs As New atcDataAttributes
        lArgs.SetValue("Timeseries", aTimeseriesGroup)

        lArgs.SetValue(InputNames.NDay, aNDay) '"NDay"
        lArgs.SetValue("HighFlag", aHighFlag)

        lArgs.SetValue(InputNames.StartYear, aFirstYear) '"FirstYear"
        lArgs.SetValue(InputNames.EndYear, aLastYear) '"LastYear"

        lArgs.SetValue(InputNames.StartMonth, aBoundaryMonth) '"BoundaryMonth"
        lArgs.SetValue(InputNames.StartDay, aBoundaryDay) '"BoundaryDay"

        lArgs.SetValue(InputNames.EndMonth, aEndMonth) '"EndMonth"
        lArgs.SetValue(InputNames.EndDay, aEndDay) '"EndDay"

        Dim lHighLow As String = "low"
        If aHighFlag Then
            lHighLow = "high"
        End If

        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        If lCalculator.Open("n-day " & lHighLow & " timeseries", lArgs) Then
            For Each lDataset As atcTimeseries In lCalculator.DataSets
                ComputeRanks(lDataset, Not aHighFlag, False)
            Next
        End If
        Return lCalculator.DataSets
    End Function

    Public Shared Function ListDefaultArray(ByVal aListTag As String) As Double()
        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        Dim lNDayHi As atcDefinedValue = lCalculator.AvailableOperations.GetDefinedValue("n-day high value")
        Dim lArgs As atcDataAttributes = lNDayHi.Arguments
        Dim lDefault As Object = lArgs.GetDefinedValue(aListTag).Definition.DefaultValue
        If IsArray(lDefault) Then
            Return lDefault
        Else
            Return Nothing
        End If
    End Function

End Class