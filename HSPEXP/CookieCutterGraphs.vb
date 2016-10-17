Imports atcUtility
Imports MapWinUtility
Imports atcUCI
Imports atcData
Imports ZedGraph 'this is coming from a DLL as the original project was a C# project and not a VB project
Imports atcGraph


Public Module CookieCutterGraphs
    Sub ReganGraphs(ByVal aHSPFUCI As HspfUci, ByVal aSDateJ As Double, ByVal aEDateJ As Double, ByVal lOutputFolder As String)
        Dim lConstituentsToGraph As New atcCollection
        lOutputFolder &= "ReganPlots\"

        If Not System.IO.Directory.Exists(lOutputFolder) Then
            System.IO.Directory.CreateDirectory(lOutputFolder)
        End If
        With lConstituentsToGraph
            .Add("BALCLA1", "Benthic Algae")
            .Add("PHYCLA", "Phytoplankton as Chlorophyll a")
            .Add("PO4-CONCDIS", "Orthophosphate")
            .Add("TAM-CONCDIS", "Ammonia")
            .Add("NO3-CONCDIS", "Nitrate")
        End With

        Dim lLocations As New atcCollection
        Dim lScenarioResults As New atcDataSource
        For i As Integer = 0 To aHSPFUCI.FilesBlock.Count
            If aHSPFUCI.FilesBlock.Value(i).Typ = "BINO" Then
                Dim lHspfBinFileName As String = AbsolutePath(aHSPFUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                Dim lOpenHspfBinDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                If lOpenHspfBinDataSource Is Nothing Then
                    If atcDataManager.OpenDataSource(lHspfBinFileName) Then
                        lOpenHspfBinDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                    End If
                End If
                If lOpenHspfBinDataSource.DataSets.Count > 1 Then
                    lLocations.AddRange(lOpenHspfBinDataSource.DataSets.SortedAttributeValues("Location"))
                    Dim lConstituentNames As New SortedSet(Of String)
                    For Each lKey As String In lConstituentsToGraph.Keys

                        lConstituentNames.Add(lKey.ToUpper)
                    Next
                    For Each lTs As atcTimeseries In lOpenHspfBinDataSource.DataSets
                        Dim ConstituentFromTS = lTs.Attributes.GetValue("Constituent").ToString.ToUpper
                        If lConstituentNames.Contains(ConstituentFromTS) Then
                            lTs = SubsetByDate(lTs, aSDateJ, aEDateJ, Nothing)
                            lScenarioResults.DataSets.Add(lTs)
                        End If
                    Next lTs
                End If
            End If
        Next i
        Dim lGraphFilesCount = 0
        'Dim lRCH As HspfOperation
        For Each RCHRES In lLocations
            If RCHRES.Contains("R:") Then

                Dim lRchId = RCHRES.split(":")(1)
                Dim lTimeseriesGroup As New atcTimeseriesGroup
                Dim lTimeSeries As atcTimeseries
                lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "BALCLA1")(0)
                If lTimeSeries IsNot Nothing Then
                    lTimeSeries.Attributes.SetValue("YAxis", "Aux")
                    lTimeseriesGroup.Add(lTimeSeries)
                End If
                lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "PHYCLA")(0)
                If lTimeSeries IsNot Nothing Then
                    lTimeSeries.Attributes.SetValue("YAxis", "Left")
                    lTimeseriesGroup.Add(lTimeSeries)
                End If
                lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "PO4-CONCDIS")(0)
                If lTimeSeries IsNot Nothing Then
                    lTimeSeries.Attributes.SetValue("YAxis", "Right")
                    lTimeseriesGroup.Add(lTimeSeries)
                End If
                lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "TAM-CONCDIS")(0)
                If lTimeSeries IsNot Nothing Then
                    lTimeSeries.Attributes.SetValue("YAxis", "Right")
                    lTimeseriesGroup.Add(lTimeSeries)
                End If
                lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "NO3-CONCDIS")(0)
                If lTimeSeries IsNot Nothing Then
                    lTimeSeries.Attributes.SetValue("YAxis", "Right")
                    lTimeseriesGroup.Add(lTimeSeries)
                End If
                If lTimeseriesGroup.Count = 5 Then
                    Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
                    Dim lGrapher As New clsGraphTime(lTimeseriesGroup, lZgc)
                    Dim lMainPane As GraphPane = lZgc.MasterPane.PaneList(1)
                    Dim lAuxPane As GraphPane = lZgc.MasterPane.PaneList(0)
                    Dim lCurve As ZedGraph.LineItem = Nothing
                    lAuxPane.YAxis.Title.Text = "Benthic Algae (" & ChrW(956) & "g/m^2"
                    lAuxPane.YAxis.Scale.Min = 0
                    lMainPane.YAxis.Title.Text = "Phytoplankton as Chlorophyll a (" & ChrW(956) & "g/l)"
                    lMainPane.YAxis.Scale.Min = 0
                    lMainPane.Y2Axis.Title.Text = "NH4-N, NO3-N, and PO4-P (mg/l)"
                    lMainPane.Y2Axis.Scale.Min = 0
                    lCurve = lAuxPane.CurveList.Item(0)
                    lCurve.Line.IsVisible = True
                    lCurve.Symbol.Type = SymbolType.None
                    lCurve.Line.Color = Drawing.Color.FromName("Red")
                    lCurve.Line.Width = 1
                    lCurve.Label.Text = "Benthic Algae"
                    lCurve = lMainPane.CurveList.Item(0)
                    lCurve.Line.IsVisible = True
                    lCurve.Symbol.Type = SymbolType.None
                    lCurve.Line.Color = Drawing.Color.FromName("Red")
                    lCurve.Line.Width = 1
                    lCurve.Label.Text = "Phytoplankton as Chlorophyll a"
                    lCurve = lMainPane.CurveList.Item(1)
                    lCurve.Line.IsVisible = True
                    lCurve.Symbol.Type = SymbolType.None
                    lCurve.Line.Color = Drawing.Color.FromName("Green")
                    lCurve.Line.Width = 1
                    lCurve.Label.Text = "Dissolved PO4-P"
                    lCurve = lMainPane.CurveList.Item(2)
                    lCurve.Line.IsVisible = True
                    lCurve.Symbol.Type = SymbolType.None
                    lCurve.Line.Color = Drawing.Color.FromName("Purple")
                    lCurve.Line.Width = 1
                    lCurve.Label.Text = "Dissolved TAM-N"
                    lCurve = lMainPane.CurveList.Item(3)
                    lCurve.Line.IsVisible = True
                    lCurve.Symbol.Type = SymbolType.None
                    lCurve.Line.Color = Drawing.Color.FromName("Black")
                    lCurve.Line.Width = 1
                    lCurve.Label.Text = "Dissolved NO3-N"
                    lZgc.SaveIn(lOutputFolder & "RCHRES" & lRchId & ".png")
                Else
                    Logger.Msg("Cannot generate Regan Plot for RCHRES" & lRchId & ". : 
All timeseries are not available at the RCHRES" & lRchId & ". Therefore Regan plot will not be generated for this reach.")
                End If
            End If
        Next
    End Sub
End Module
