Imports atcUtility
Imports MapWinUtility
Imports atcUCI
Imports atcData
Imports ZedGraph 'this is coming from a DLL as the original project was a C# project and not a VB project
Imports atcGraph
Imports System.Collections.Specialized

Public Module CookieCutterGraphs
    'This can probably go under atcGraph.
    Sub ReganGraphs(ByVal aHSPFUCI As HspfUci, ByVal aSDateJ As Double, ByVal aEDateJ As Double, ByVal aOutputFolder As String)
        Dim lConstituentsToGraph As New atcCollection
        Dim lOutputFolder As String = aOutputFolder & "ReganPlots\"
        Dim lWQCriteriaInmgperliter As Double = 0.0
        Dim lAcftToCubicft As Double = 43560 'Should be called from modUnits
        Dim lCubicftToLiters As Double = 28.317
        Dim lPoundsToMilligrams As Double = 453592.0
        Dim lTSTimeUnit As Integer
        Dim lCommonRESStandard As Double = 0.0
        Dim lLowerRangeRCHId As Integer = 0
        Dim lHigherRangeRCHId As Integer = 0

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
            .Add("SSED-TOT", "Total Suspended Solids (mg/L)")
            .Add("RO", "Flow (cfs)")
            .Add("BEDDEP", "Bed Depth (ft)")
            .Add("DOXCONC", "Dissolved Oxygen (mg/L)")
        End With

        Dim lLocations As New atcCollection
        Dim lScenarioResults As New atcDataSource

        Dim lWDMDataSource As New atcDataSource

        For i As Integer = 0 To aHSPFUCI.FilesBlock.Count
            If aHSPFUCI.FilesBlock.Value(i).Typ = "BINO" Then
                Dim lHspfBinFileName As String = AbsolutePath(aHSPFUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                Dim lDataSource As atcDataSource
                lDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                If lDataSource Is Nothing Then
                    If atcDataManager.OpenDataSource(lHspfBinFileName) Then
                        lDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                    End If
                End If
                If lDataSource.DataSets.Count > 1 Then
                    lLocations.AddRange(lDataSource.DataSets.SortedAttributeValues("Location"))
                    Dim lConstituentNames As New SortedSet(Of String)
                    For Each lKey As String In lConstituentsToGraph.Keys
                        lConstituentNames.Add(lKey.ToUpper)
                    Next
                    For Each lTs As atcTimeseries In lDataSource.DataSets
                        Dim ConstituentFromTS = lTs.Attributes.GetValue("Constituent").ToString.ToUpper
                        If lConstituentNames.Contains(ConstituentFromTS) Then
                            lTs = SubsetByDate(lTs, aSDateJ, aEDateJ, Nothing)
                            lScenarioResults.DataSets.Add(lTs)
                        End If
                    Next lTs
                End If

                For Each lRCHRES In lLocations
#Region "Plotting Nutrient Curves"
                    If lRCHRES.Contains("R:") Then

                        Dim lRchId = lRCHRES.split(":")(1)
                        Dim lRchresCaption As String = aHSPFUCI.OpnBlks("RCHRES").OperFromID(lRchId).Caption
                        lRchresCaption = Trim(Mid(lRchresCaption, 11))
                        Dim lTimeseriesGroup As New atcTimeseriesGroup
                        Dim lTimeSeries As atcTimeseries = Nothing
                        Dim lFoundTheTimeSeriesinWDMFile As Boolean = False

                        lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "PLANK", "BALCLA", 4, 1, lFoundTheTimeSeriesinWDMFile)
                        lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)

                        If lFoundTheTimeSeriesinWDMFile = False Then
                            'Reading the required time series from Binary file and resetting the flag to false
                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "BALCLA1")(0)
                        End If
                        lFoundTheTimeSeriesinWDMFile = False

                        If lTimeSeries IsNot Nothing Then
                            lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If lTSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Aux")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If

                        lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "PLANK", "PHYCLA", 1, 1, lFoundTheTimeSeriesinWDMFile)
                        lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)

                        If lFoundTheTimeSeriesinWDMFile = False Then
                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "PHYCLA")(0)
                        End If
                        lFoundTheTimeSeriesinWDMFile = False
                        If lTimeSeries IsNot Nothing Then
                            lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If lTSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Left")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If

                        lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "NUTRX", "DNUST", 4, 1, lFoundTheTimeSeriesinWDMFile)
                        lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)
                        If lFoundTheTimeSeriesinWDMFile = False Then
                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "PO4-CONCDIS")(0)

                        End If
                        lFoundTheTimeSeriesinWDMFile = False
                        If lTimeSeries IsNot Nothing Then
                            lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If lTSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Right")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If

                        lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "NUTRX", "DNUST", 2, 1, lFoundTheTimeSeriesinWDMFile)
                        lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)
                        If lFoundTheTimeSeriesinWDMFile = False Then
                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "TAM-CONCDIS")(0)
                        End If
                        lFoundTheTimeSeriesinWDMFile = False
                        If lTimeSeries IsNot Nothing Then
                            lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If lTSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Right")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If

                        lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "NUTRX", "DNUST", 1, 1, lFoundTheTimeSeriesinWDMFile)
                        lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)
                        If lFoundTheTimeSeriesinWDMFile = False Then
                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "NO3-CONCDIS")(0)
                        End If
                        lFoundTheTimeSeriesinWDMFile = False

                        If lTimeSeries IsNot Nothing Then
                            lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value

                            If lTSTimeUnit <= 4 Then
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
                            lAuxPane.YAxis.Title.Text = "Ben. algae (" & ChrW(956) & "g/m^2"
                            lAuxPane.YAxis.Scale.Min = 0
                            lMainPane.YAxis.Title.Text = "Phytoplankton as chlorophyll a (" & ChrW(956) & "g/L)"
                            lMainPane.YAxis.Scale.Min = 0
                            lMainPane.Y2Axis.Title.Text = "Dissolved NH4-N, NO3-N, and PO4-P (mg/L)"
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
                            lCurve.Label.Text = "Phytoplankton as chlorophyll a"
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
                            lZgc.SaveIn(lOutputFolder & "Nutrient_RCHRES_" & lRchId & ".png")
                            Logger.Dbg("Generated graph " & lOutputFolder & "Nutrient_RCHRES_" & lRchId & ".png")
                        Else
                            '                            Logger.Msg("Cannot generate Regan Plot for RCHRES" & lRchId & ". : 
                            'All timeseries are not available at the RCHRES" & lRchId & ". Therefore Regan plot will not be generated for this reach.")
                        End If
#End Region
                        'Plotting the TSS Curve
#Region "Plotting TSS Curve"

                        lTimeseriesGroup = New atcTimeseriesGroup
                        lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "HYDR", "RO", 1, 1, lFoundTheTimeSeriesinWDMFile)
                        lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)
                        If lFoundTheTimeSeriesinWDMFile = False Then
                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "RO")(0)
                        End If
                        lFoundTheTimeSeriesinWDMFile = False
                        If lTimeSeries IsNot Nothing Then
                            lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If lTSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Aux")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If

                        lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "SEDTRN", "SSED", 4, 1, lFoundTheTimeSeriesinWDMFile)
                        lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)

                        If lFoundTheTimeSeriesinWDMFile = False Then
                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "SSED-TOT")(0)
                        End If
                        lFoundTheTimeSeriesinWDMFile = False

                        If lTimeSeries IsNot Nothing Then
                            lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If lTSTimeUnit <= 4 Then
                                lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTimeSeries.Attributes.SetValue("YAxis", "Left")
                            lTimeseriesGroup.Add(lTimeSeries)
                        End If
                        lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "SEDTRN", "BEDDEP", 1, 1, lFoundTheTimeSeriesinWDMFile)
                        lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)
                        If lFoundTheTimeSeriesinWDMFile = False Then
                            lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "BEDDEP")(0)
                        End If
                        lFoundTheTimeSeriesinWDMFile = False
                        If lTimeSeries IsNot Nothing Then
                            lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                            If lTSTimeUnit <= 4 Then
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
                            lMainPane.YAxis.Title.Text = "Total Suspended Solids (mg/L)"
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
                            lCurve.Label.Text = "Total Suspended Solids (mg/L)"
                            lCurve = lMainPane.CurveList.Item(1)
                            lCurve.Line.IsVisible = True
                            lCurve.Symbol.Type = SymbolType.None
                            lCurve.Line.Color = Drawing.Color.FromName("Green")
                            lCurve.Line.Width = 1
                            lCurve.Label.Text = "Bed Depth (ft)"
                            lZgc.SaveIn(lOutputFolder & "TSS_RCHRES_" & lRchId & ".png")
                            Logger.Dbg("Generated graph " & lOutputFolder & "TSS_RCHRES_" & lRchId & ".png")
                        End If
#End Region

                        'Plotting DO Concentrations
                        Dim lTimeseriesGroupDO As New atcTimeseriesGroup
                        Dim lTimeSeriesDO As New atcTimeseries(Nothing)
                        Dim lTimeSeriesTimeUnit As Integer
                        lTimeSeriesDO = LocateTheTimeSeries(aHSPFUCI, lRchId, "OXRX", "DOX", 1, 1, lFoundTheTimeSeriesinWDMFile)
                        lTimeSeriesDO = SubsetByDate(lTimeSeriesDO, aSDateJ, aEDateJ, Nothing)

                        If lFoundTheTimeSeriesinWDMFile = False OrElse lTimeSeriesTimeUnit < 3 Then
                            lTimeSeriesDO = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "DOXCONC")(0)
                        End If
                        lTimeSeriesTimeUnit = lTimeSeriesDO.Attributes.GetDefinedValue("Time Unit").Value
                        lFoundTheTimeSeriesinWDMFile = False
                        If lTimeSeriesDO.Attributes.GetDefinedValue("Time Unit").Value < 3 Then
                            lTimeseriesGroupDO.Add(Aggregate(lTimeSeriesDO, atcTimeUnit.TUDay, 1, atcTran.TranMax))
                            lTimeseriesGroupDO.Add(Aggregate(lTimeSeriesDO, atcTimeUnit.TUDay, 1, atcTran.TranMin))

                            Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
                            Dim lGrapher As New clsGraphTime(lTimeseriesGroupDO, lZgc)
                            Dim lMainPane As GraphPane = lZgc.MasterPane.PaneList(0)

                            Dim lCurve As ZedGraph.LineItem = Nothing
                            lCurve = lMainPane.CurveList.Item(0)
                            lCurve.Line.IsVisible = True
                            lCurve.Symbol.Type = SymbolType.None
                            lCurve.Line.Color = Drawing.Color.FromName("red")
                            lCurve.Line.Width = 1
                            lCurve.Label.Text = "Maximum Daily DO Concentration"
                            lCurve = lMainPane.CurveList.Item(1)
                            lCurve.Line.IsVisible = True
                            lCurve.Symbol.Type = SymbolType.None
                            lCurve.Line.Color = Drawing.Color.FromName("green")
                            lCurve.Line.Width = 1
                            lCurve.Label.Text = "Minimum Daily DO Concentration"

                            lMainPane.YAxis.Title.Text = "Dissolved Oxygen (mg/L)"
                            lMainPane.YAxis.Scale.Min = 0
                            lMainPane.YAxis.Scale.Max = 20
                            lZgc.SaveIn(lOutputFolder & "DO_Concentration_RCHRES_" & lRCHRES.split(":")(1) & ".png")
                            Logger.Dbg("Generated graph " & lOutputFolder & "DO_Concentration_RCHRES_" & lRCHRES.split(":")(1) & ".png")
                        Else
                            Logger.Dbg("The time unit of DO timeseries is daily or greater, so max daily and min daily will not be calculated for RCHRES:" & lRchId)
                        End If

                        'Plotting Load Duration Curve
                        'Read the RES_TP_Standard.csv
#Region "Plotting RES Curve"
                        If lCommonRESStandard = 0.0 Then

                            If Not (lRchId = lLowerRangeRCHId Or lRchId = lHigherRangeRCHId Or
                                                            (lRchId > lLowerRangeRCHId AndAlso lRchId < lHigherRangeRCHId)) Then
                                lWQCriteriaInmgperliter = 0.0
                                Dim lRESStandardFileNames As New NameValueCollection
                                AddFilesInDir(lRESStandardFileNames, IO.Directory.GetCurrentDirectory, False, "*RES_TP_Standard.csv")
                                If lRESStandardFileNames.Count < 1 Then '
                                    Logger.Dbg(Now & " Custom graphs will not be produced.")
                                    Continue For
                                End If
                                Dim lGraphRecordsNew As New ArrayList()
                                Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(lRESStandardFileNames(0))
                                    Dim lines() As String = {}
                                    If System.IO.File.Exists(lRESStandardFileNames(0)) Then

                                        MyReader.TextFieldType = FileIO.FieldType.Delimited
                                        MyReader.SetDelimiters(",")
                                        Dim lCurrentRow As String()

                                        While Not MyReader.EndOfData
                                            Try
                                                If MyReader.PeekChars(10000).Contains("***") Or
                                                    MyReader.PeekChars(10000).Contains("RCHRES") Then

                                                    lCurrentRow = MyReader.ReadFields
                                                Else
                                                    lCurrentRow = MyReader.ReadFields
                                                    If lCurrentRow(0).ToLower = "all" Then
                                                        lCommonRESStandard = lCurrentRow(1)
                                                    ElseIf lCurrentRow(0).Contains("-") Then
                                                        Dim lRange As String() = lCurrentRow(0).Split("-")
                                                        lLowerRangeRCHId = CType(Trim(lRange(0)), Integer)
                                                        lHigherRangeRCHId = CType(Trim(lRange(1)), Integer)
                                                        If lRchId = lLowerRangeRCHId Or lRchId = lHigherRangeRCHId Or
                                                               (lRchId > lLowerRangeRCHId AndAlso lRchId < lHigherRangeRCHId) Then
                                                            lWQCriteriaInmgperliter = lCurrentRow(1)
                                                            Exit While
                                                        End If
                                                    ElseIf lRchId = CType(lCurrentRow(0), Integer) Then
                                                        lWQCriteriaInmgperliter = lCurrentRow(1)
                                                    End If
                                                    lGraphRecordsNew.Add(lCurrentRow)

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
                            lWQCriteriaInmgperliter = lCommonRESStandard
                        End If

                        If lWQCriteriaInmgperliter > 0.0 Then


                            lTimeseriesGroup = New atcTimeseriesGroup
                            lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "HYDR", "IVOL", 1, 1, lFoundTheTimeSeriesinWDMFile)
                            lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)
                            If lFoundTheTimeSeriesinWDMFile = False Then
                                lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "IVOL")(0)
                            End If
                            lFoundTheTimeSeriesinWDMFile = False
                            If lTimeSeries IsNot Nothing Then
                                lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                                If lTSTimeUnit <= 4 Then
                                    lTimeSeries = Aggregate(lTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                                    lTimeSeries = lTimeSeries * lWQCriteriaInmgperliter * lAcftToCubicft * lCubicftToLiters / lPoundsToMilligrams
                                    lTimeSeries.Attributes.SetValue("YAxis", "Left")
                                    lTimeseriesGroup.Add(lTimeSeries)
                                End If
                            End If
                            lTimeSeries = LocateTheTimeSeries(aHSPFUCI, lRchId, "PLANK", "TPKIF", 5, 1, lFoundTheTimeSeriesinWDMFile)
                            lTimeSeries = SubsetByDate(lTimeSeries, aSDateJ, aEDateJ, Nothing)
                            If lFoundTheTimeSeriesinWDMFile = False Then
                                lTimeSeries = lScenarioResults.DataSets.FindData("Location", lRCHRES).FindData("Constituent", "P-TOT-IN")(0)
                            End If
                            lFoundTheTimeSeriesinWDMFile = False

                            If lTimeSeries IsNot Nothing Then
                                lTSTimeUnit = lTimeSeries.Attributes.GetDefinedValue("Time Unit").Value
                                If lTSTimeUnit <= 4 Then
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
                                lCurve.Line.Color = Drawing.Color.FromName("green")
                                lCurve.Line.Width = 2
                                lCurve.Label.Text = "RES Standard (" & lWQCriteriaInmgperliter & " mg/L)"
                                lCurve = lMainPane.CurveList.Item(1)
                                lCurve.Line.IsVisible = True
                                lCurve.Symbol.Type = SymbolType.None
                                lCurve.Line.Color = Drawing.Color.FromName("red")
                                lCurve.Line.Width = 2
                                lCurve.Label.Text = "Baseline"

                                lMainPane.YAxis.Title.Text = "Total Phosphorus (lbs/day)"
                                lMainPane.YAxis.Scale.Min = 0
                                lMainPane.XAxis.Title.Text = "Percent Exceedance at " & lRchresCaption

                                lZgc.SaveIn(lOutputFolder & "LoadDurationTP_RCHRES_" & lRchId & ".png")
                                Logger.Dbg("Generated graph " & lOutputFolder & "LoadDurationTP_RCHRES_" & lRchId & ".png")
                            End If

                        End If

                    End If
#End Region

                Next lRCHRES

            End If

        Next i

        Dim lGraphFilesCount = 0
        'Dim lRCH As HspfOperation

    End Sub
    Friend Function LocateTheTimeSeries(ByVal aHSPFUCI As HspfUci,
                                         ByVal aRCHId As Integer, ByVal aGroupName As String,
                                         ByVal aMemberName As String, ByVal aMemSub1 As Integer,
                                         ByVal aMemSub2 As Integer, ByRef aFoundTheTS As Boolean) As atcTimeseries
        Dim lDataSource As New atcDataSource
        Dim lTimeSeries As atcTimeseries = Nothing
        aFoundTheTS = False
        Dim lDataID As Integer = 0
        For Each lConnection As HspfConnection In aHSPFUCI.Connections
            If lConnection.Source.VolName = "RCHRES" AndAlso lConnection.Source.VolId = aRCHId AndAlso
                                lConnection.Source.Group = aGroupName AndAlso lConnection.Source.Member = aMemberName AndAlso
                                lConnection.Source.MemSub1 = aMemSub1 AndAlso lConnection.Source.MemSub2 = aMemSub2 AndAlso
                                lConnection.Target.VolName.Contains("WDM") Then
                lDataID = lConnection.Target.VolId
                For i As Integer = 0 To aHSPFUCI.FilesBlock.Count
                    If aHSPFUCI.FilesBlock.Value(i).Typ = lConnection.Target.VolName Then
                        Dim lFileName As String = AbsolutePath(aHSPFUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                        If lDataSource Is Nothing Then
                            If atcDataManager.OpenDataSource(lFileName) Then
                                lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                            End If
                        End If
                        Exit For
                    End If
                Next
                aFoundTheTS = True
                Exit For
            End If
        Next lConnection
        If lDataID > 0 Then
            lTimeSeries = lDataSource.DataSets.FindData("ID", lDataID)(0)
            aFoundTheTS = True
        End If

        Return lTimeSeries
    End Function

End Module
