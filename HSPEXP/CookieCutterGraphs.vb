Imports atcUtility
Imports MapWinUtility
Imports atcUCI
Imports atcData
Imports ZedGraph 'this is coming from a DLL as the original project was a C# project and not a VB project
Imports atcGraph
Imports System.Collections.Specialized

Public Module CookieCutterGraphs
    Sub ReganGraphs(ByVal aHSPFUCI As HspfUci, ByVal aSDateJ As Double, ByVal aEDateJ As Double, ByVal lOutputFolder As String)
        Dim lConstituentsToGraph As New atcCollection
        lOutputFolder &= "ReganPlots\"
        Dim WQCriteriaInmgperliter As Double = 0.0
        Dim acftToCubicft As Double = 43560 'Should be called from modUnits
        Dim CubicftToLiters As Double = 28.317
        Dim PoundsToMilligrams As Double = 453592.0
        Dim TSTimeUnit As Integer
        Dim CommonRESStandard As Double = 0.0
        Dim LowerRangeRCHId As Integer = 0
        Dim HigherRangeRCHId As Integer = 0

        If Not System.IO.Directory.Exists(lOutputFolder) Then
            System.IO.Directory.CreateDirectory(lOutputFolder)
        End If
        With lConstituentsToGraph
            .Add("BALCLA1", "Benthic Algae")
            .Add("PHYCLA", "Phytoplankton as Chlorophyll a")
            .Add("PO4-CONCDIS", "Orthophosphate")
            .Add("TAM-CONCDIS", "Ammonia")
            .Add("NO3-CONCDIS", "Nitrate")
            .Add("IVOL", "Water Volume")
            .Add("P-TOT-IN", "Input of Total P (Existing Conditions)")
            .Add("SSED-TOT", "Total Suspended Solids (mg/l)")
            .Add("RO", "Flow (cfs)")
            .Add("BEDDEP", "Bed Depth (ft)")
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
                For Each RCHRES In lLocations
                    If RCHRES.Contains("R:") Then

                        Dim lRchId = RCHRES.split(":")(1)
                        Dim lRchresCaption As String = aHSPFUCI.OpnBlks("RCHRES").OperFromID(lRchId).Caption
                        lRchresCaption = Trim(Mid(lRchresCaption, 11))
                        Dim lTimeseriesGroup As New atcTimeseriesGroup
                        Dim lTimeSeries As atcTimeseries
                        lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "BALCLA1")(0)

                        If lTimeSeries IsNot Nothing Then
                            TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If TSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Aux")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If
                        lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "PHYCLA")(0)

                        If lTimeSeries IsNot Nothing Then
                            TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If TSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Left")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If
                        lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "PO4-CONCDIS")(0)

                        If lTimeSeries IsNot Nothing Then
                            TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If TSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Right")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If
                        lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "TAM-CONCDIS")(0)

                        If lTimeSeries IsNot Nothing Then
                            TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If TSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Right")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If

                        lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "NO3-CONCDIS")(0)
                        If lTimeSeries IsNot Nothing Then
                            TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value

                            If TSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
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

                            lMainPane.XAxis.Title.Text = lRchresCaption

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
                            lZgc.SaveIn(lOutputFolder & "RCHRES_" & lRchId & ".png")
                        Else
                            Logger.Msg("Cannot generate Regan Plot for RCHRES" & lRchId & ". : 
All timeseries are not available at the RCHRES" & lRchId & ". Therefore Regan plot will not be generated for this reach.")
                        End If

                        'Plotting the TSS Curve


                        lTimeseriesGroup = New atcTimeseriesGroup

                        lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "RO")(0)

                        If lTimeSeries IsNot Nothing Then
                            TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If TSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Aux")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If
                        lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "SSED-TOT")(0)

                        If lTimeSeries IsNot Nothing Then
                            TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If TSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Left")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If
                        lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "BEDDEP")(0)
                        If lTimeSeries IsNot Nothing Then
                            TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If TSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Right")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If

                        If lTimeseriesGroup.Count = 3 Then
                            Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
                            Dim lGrapher As New clsGraphTime(lTimeseriesGroup, lZgc)
                            Dim lMainPane As GraphPane = lZgc.MasterPane.PaneList(1)
                            Dim lAuxPane As GraphPane = lZgc.MasterPane.PaneList(0)
                            Dim lCurve As ZedGraph.LineItem = Nothing
                            lAuxPane.YAxis.Title.Text = "Flow (cfs)"
                            lAuxPane.YAxis.Scale.Min = 0
                            lMainPane.YAxis.Title.Text = "Total Suspended Solids (mg/l)"
                            lMainPane.YAxis.Scale.Min = 0
                            lMainPane.XAxis.Title.Text = lRchresCaption
                            lMainPane.Y2Axis.Title.Text = "Bed Depth (ft)"
                            lMainPane.Y2Axis.Scale.Min = 0

                            lCurve = lAuxPane.CurveList.Item(0)
                            lCurve.Line.IsVisible = True
                            lCurve.Symbol.Type = SymbolType.None
                            lCurve.Line.Color = Drawing.Color.FromName("Red")
                            lCurve.Line.Width = 1

                            lCurve.Label.Text = "Flow (cfs)"

                            lCurve = lMainPane.CurveList.Item(0)
                            lCurve.Line.IsVisible = True
                            lCurve.Symbol.Type = SymbolType.None
                            lCurve.Line.Color = Drawing.Color.FromName("Red")
                            lCurve.Line.Width = 1
                            lCurve.Label.Text = "Total Suspended Solids (mg/l)"

                            lCurve = lMainPane.CurveList.Item(1)
                            lCurve.Line.IsVisible = True
                            lCurve.Symbol.Type = SymbolType.None
                            lCurve.Line.Color = Drawing.Color.FromName("Green")
                            lCurve.Line.Width = 1
                            lCurve.Label.Text = "Bed Depth (ft)"
                            lZgc.SaveIn(lOutputFolder & "TSS_RCHRES_" & lRchId & ".png")
                        End If


                        'Plotting Load Duration Curve
                        'Read the RES_TP_Standard.csv
                        If CommonRESStandard = 0.0 Then


                            If Not (lRchId = LowerRangeRCHId Or lRchId = HigherRangeRCHId Or
                                                        (lRchId > LowerRangeRCHId AndAlso lRchId < HigherRangeRCHId)) Then
                                WQCriteriaInmgperliter = 0.0
                                Dim lRESStandardFileNames As New NameValueCollection
                                AddFilesInDir(lRESStandardFileNames, IO.Directory.GetCurrentDirectory, False, "*RES_TP_Standard.csv")
                                If lRESStandardFileNames.Count < 1 Then '
                                    Logger.Dbg(Now & " Custom graphs will not be produced.")
                                    Continue For
                                End If
                                Dim lgraphRecordsNew As New ArrayList()
                                Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(lRESStandardFileNames(0))
                                    Dim lines() As String = {}
                                    If System.IO.File.Exists(lRESStandardFileNames(0)) Then

                                        MyReader.TextFieldType = FileIO.FieldType.Delimited
                                        MyReader.SetDelimiters(",")
                                        Dim CurrentRow As String()

                                        While Not MyReader.EndOfData
                                            Try
                                                If MyReader.PeekChars(10000).Contains("***") Or
                                                MyReader.PeekChars(10000).Contains("RCHRES") Then

                                                    CurrentRow = MyReader.ReadFields
                                                Else
                                                    CurrentRow = MyReader.ReadFields
                                                    If CurrentRow(0).ToLower = "all" Then
                                                        CommonRESStandard = CurrentRow(1)
                                                    ElseIf CurrentRow(0).Contains("-") Then
                                                        Dim Range As String() = CurrentRow(0).Split("-")
                                                        LowerRangeRCHId = CType(Trim(Range(0)), Integer)
                                                        HigherRangeRCHId = CType(Trim(Range(1)), Integer)
                                                        If lRchId = LowerRangeRCHId Or lRchId = HigherRangeRCHId Or
                                                            (lRchId > LowerRangeRCHId AndAlso lRchId < HigherRangeRCHId) Then
                                                            WQCriteriaInmgperliter = CurrentRow(1)
                                                            Exit While
                                                        End If
                                                    ElseIf lRchId = CType(CurrentRow(0), Integer) Then
                                                        WQCriteriaInmgperliter = CurrentRow(1)
                                                    End If
                                                    lgraphRecordsNew.Add(CurrentRow)

                                                End If
                                            Catch ex As Microsoft.VisualBasic.
                                                FileIO.MalformedLineException
                                                MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                                            End Try
                                        End While
                                    End If
                                End Using

                            End If


                        Else
                            WQCriteriaInmgperliter = CommonRESStandard
                        End If

                        If WQCriteriaInmgperliter > 0.0 Then


                            lTimeseriesGroup = New atcTimeseriesGroup

                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "IVOL")(0)

                            If lTimeSeries IsNot Nothing Then
                                TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                                If TSTimeUnit <= 4 Then
                                    lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                                    lTimeSeries = lTimeSeries * WQCriteriaInmgperliter * acftToCubicft * CubicftToLiters / PoundsToMilligrams
                                    lTimeSeries.Attributes.SetValue("YAxis", "Left")
                                    lTimeseriesGroup.Add(lTimeSeries)
                                End If
                            End If
                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", RCHRES).FindData("Constituent", "P-TOT-IN")(0)

                            If lTimeSeries IsNot Nothing Then
                                TSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                                If TSTimeUnit <= 4 Then
                                    lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                                    lTimeSeries.Attributes.SetValue("YAxis", "Left")
                                    lTimeseriesGroup.Add(lTimeSeries)
                                End If
                            End If
                            If lTimeseriesGroup.Count = 2 Then

                                Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
                                Dim lGrapher As New clsGraphProbability(lTimeseriesGroup, lZgc)
                                Dim lMainPane As GraphPane = lZgc.MasterPane.PaneList(0)

                                Dim lCurve As ZedGraph.LineItem = Nothing
                                lCurve = lMainPane.CurveList.Item(0)
                                lCurve.Line.IsVisible = True
                                lCurve.Symbol.Type = SymbolType.None
                                lCurve.Line.Color = Drawing.Color.FromName("orange")
                                lCurve.Line.Width = 2
                                lCurve.Label.Text = "RES Standard (" & WQCriteriaInmgperliter & " mg/l)"
                                lCurve = lMainPane.CurveList.Item(1)
                                lCurve.Line.IsVisible = True
                                lCurve.Symbol.Type = SymbolType.None
                                lCurve.Line.Color = Drawing.Color.FromName("blue")
                                lCurve.Line.Width = 2
                                lCurve.Label.Text = "Baseline"

                                lMainPane.YAxis.Title.Text = "Total Phosphorus (lbs/day)"
                                lMainPane.YAxis.Scale.Min = 0
                                lMainPane.XAxis.Title.Text = "Percent Exceedance at " & lRchresCaption

                                lZgc.SaveIn(lOutputFolder & "LoadDurationTP_RCHRES_" & lRchId & ".png")

                            End If

                        End If

                    End If



                Next RCHRES


            End If
            atcDataManager.DataSets.Clear()
        Next i
        Dim lGraphFilesCount = 0
        'Dim lRCH As HspfOperation

    End Sub
End Module
