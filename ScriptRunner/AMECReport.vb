Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcBasinsObsWQ
Imports atcSeasons
Imports atcGraph
Imports ZedGraph

Imports MapWindow.Interfaces
Imports MapWinUtility

Public Module AMECReport
    Private gProjectDir As String = "C:\BASINS\modelout\AMEC\"
    Private gOutputDir As String = gProjectDir & "Outfiles\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lTab As String = Microsoft.VisualBasic.vbTab
        Dim lCrLf As String = Microsoft.VisualBasic.vbCrLf ' Chr(13) & Chr(10)
        Dim lDateRange As String = "1/1/1973 - 1/1/2003" 'Edit this to match observed and simulated data, TODO: automatically extract from UCI file
        Dim lLocnArray() As String = {"LOW", "UP"}
        Dim lSimDsnArray() As String = {"1101", "1102"}
        Dim lCurObsCons As String = "Al_mg/L"
        Dim lWdmFileName As String = gProjectDir & "Beaver-Naph.wdm"
        Dim lDbfFileName As String = gProjectDir & "Al.dbf"
        Dim lReportFilename As String = gOutputDir & "Seasonal_Phenols_Summary.txt"

        Dim lSeasonStartArray() As String = {"11/1", "4/1", "6/1", "9/1", "1/1"}
        Dim lSeasonEndArray() As String = {"4/1", "6/1", "9/1", "11/1", "12/31"}
        Dim lSeasonNameArray() As String = {"Winter", "Spring", "Summer", "Fall", "Annual"}

        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("Project Dir", gProjectDir)
            .Add("Output Dir", gOutputDir)
            .Add("Date Range", lDateRange)
            .Add("Constituent", lCurObsCons)
            .Add("WDM file name", lWdmFileName)
            .Add("Simulated DSNs", String.Join(",", lSimDsnArray))
            .Add("DBF file name", lDbfFileName)
            .Add("Locations", String.Join(",", lLocnArray))
            .Add("Save Report As", lReportFilename)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("User Specified Parameters", lUserParms) Then
            With lUserParms
                gProjectDir = .ItemByKey("Project Dir")
                gOutputDir = .ItemByKey("Output Dir")
                lDateRange = .ItemByKey("Date Range")
                lCurObsCons = .ItemByKey("Constituent")
                lWdmFileName = .ItemByKey("WDM file name")
                lSimDsnArray = .ItemByKey("Simulated DSNs").ToString.Split(",")
                lDbfFileName = .ItemByKey("DBF file name")
                lLocnArray = .ItemByKey("Locations").ToString.Split(",")
                lReportFilename = .ItemByKey("Save Report As")
            End With

            ChDriveDir(gProjectDir)

            'open WDM file
            Dim lWdmDataSource As New atcDataSourceWDM()
            lWdmDataSource.Open(lWdmFileName)
            'open DBF file
            Dim lDbfDataSource As New atcDataSourceBasinsObsWQ()
            lDbfDataSource.Open(lDbfFileName)

            Dim lConcentrationsTable As New atcTableDelimited
            With lConcentrationsTable
                .Delimiter = lTab
                .NumFields = (lLocnArray.Length * lSeasonStartArray.Length * 4) + 1
                .NumHeaderRows = 3
                For lLocationIndex As Integer = 0 To lLocnArray.GetUpperBound(0)
                    If lLocationIndex > 0 Then
                        .Header(1) &= lTab
                        .Header(2) &= lTab
                        .Header(3) &= lTab
                    End If
                    .Header(1) &= lLocnArray(lLocationIndex)
                    For lSeasonIndex As Integer = 0 To lSeasonNameArray.GetUpperBound(0)
                        .Header(1) &= lTab & lTab & lTab & lTab
                        .Header(2) &= lSeasonNameArray(lSeasonIndex) & " (" & lSeasonStartArray(lSeasonIndex) & "-" & lSeasonEndArray(lSeasonIndex) & ")" & lTab & lTab & lTab & lTab
                        .Header(3) &= "Simulated" & lTab & lTab & "Observed" & lTab & lTab
                    Next
                Next

                Dim lFieldIndex As Integer = 1
                Dim lPlotValues() As Double
                Dim lValuesProbability() As Double
                Dim XScaleMaxValue As Double
                For lIndex As Integer = 0 To lLocnArray.GetUpperBound(0)
                    'this will need a better search if obs data contains multiple constituents
                    Dim lObsData As atcTimeseries = lDbfDataSource.DataSets.FindData("LOCN", lLocnArray(lIndex))(0)
                    Dim lSimData As atcTimeseries = lWdmDataSource.DataSets.FindData("ID", lSimDsnArray(lIndex))(0)
                    Dim lColumnStart As Integer = lIndex * lSeasonStartArray.Length * 4
                    If lIndex > 0 Then lColumnStart += 1 'TODO: more than 2 locations
                    For lSeasonIndex As Integer = 0 To lSeasonStartArray.GetUpperBound(0)
                        Dim lZgc As ZedGraphControl = CreateZgc()
                        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
                        With lPane.XAxis
                            .Type = AxisType.Linear
                            .Scale.MaxAuto = True
                            .Title.Text = lCurObsCons
                        End With
                        With lPane.YAxis
                            .Type = AxisType.Linear
                            .Scale.MaxAuto = True
                            .Title.Text = "Cumulative Probability"
                        End With

                        PutTimeseriesInGrid(lSimData, lConcentrationsTable, lFieldIndex, _
                                            lSeasonNameArray(lSeasonIndex), _
                                            lSeasonStartArray(lSeasonIndex), _
                                            lSeasonEndArray(lSeasonIndex), _
                                            lValuesProbability, _
                                            lPlotValues)
                        XScaleMaxValue = lPlotValues(lPlotValues.Length - 1)
                        lFieldIndex += 2

                        'Dim lCurveColor As Color = Color.Blue
                        Dim lCurve As LineItem = Nothing

                        lCurve = lPane.AddCurve("Simulated Data", lPlotValues, lValuesProbability, _
                                                Drawing.Color.Red, SymbolType.None)

                        lCurve.Line.IsVisible = True

                        PutTimeseriesInGrid(lObsData, lConcentrationsTable, lFieldIndex, _
                                            lSeasonNameArray(lSeasonIndex), _
                                            lSeasonStartArray(lSeasonIndex), _
                                            lSeasonEndArray(lSeasonIndex), _
                                            lValuesProbability, _
                                            lPlotValues)
                        If lPlotValues(lPlotValues.Length - 1) > XScaleMaxValue Then
                            XScaleMaxValue = lPlotValues(lPlotValues.Length - 1)
                        End If
                        lFieldIndex += 2
                        lCurve = lPane.AddCurve("Observed Data", lPlotValues, lValuesProbability, _
                                               Drawing.Color.Blue, SymbolType.Triangle)
                        lCurve.Symbol.Size = 8
                        lCurve.Symbol.Fill.IsVisible = True
                        lCurve.Line.IsVisible = False

                        Scalit(0, 1.0, False, lPane.YAxis.Scale.Min, lPane.YAxis.Scale.Max)
                        'Scalit(0, XScaleMaxValue, False, lPane.XAxis.Scale.Min, lPane.XAxis.Scale.Max)
                        With lPane.Legend
                            .IsVisible = True
                            .Position = LegendPos.Float
                            .Location = New Location(0.6, 0.8, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
                            .IsHStack = False
                            .Border.IsVisible = False
                            .Fill.IsVisible = False
                            .FontSpec.Size = 13
                            .FontSpec.IsBold = True
                            .FontSpec.Border.IsVisible = False
                        End With

                        lPane.Title.Text = lCurObsCons & " - " & lLocnArray(lIndex) & " - " & lSeasonNameArray(lSeasonIndex)
                        lZgc.SaveIn(gOutputDir & lLocnArray(lIndex) & "_" & lSeasonNameArray(lSeasonIndex) & ".png")
                        lZgc.Dispose()

                    Next
                    lFieldIndex += 1
                Next

                Dim lWdmFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lWdmFileName)
                'Dim lMsg As New atcUCI.HspfMsg
                'lMsg.Open("hspfmsg.mdb")
                'Dim lHspfUci As New atcUCI.HspfUci
                'lHspfUci.FastReadUciForStarter(lMsg, IO.Path.ChangeExtension(lWdmFileName, ".uci"))
                'Dim lReport As String = "Observed Concentrations for " & lCurObsCons & vbCrLf _
                '                      & "Dates: " & lHspfUci.GlobalBlock.RunPeriod & vbCrLf _
                '                      & "   Run Made: " & lWdmFileInfo.LastWriteTime & vbCrLf _
                '                      & "   Run Title: " & lHspfUci.GlobalBlock.RunInf.Value & vbCrLf & vbCrLf _
                '                      & .ToString

                Dim lReport As String = "Observed Concentrations for " & lCurObsCons & lCrLf _
                                      & "Dates: " & lDateRange & lCrLf _
                                      & "   Run Made: " & lWdmFileInfo.LastWriteTime & lCrLf _
                                      & .ToString

                SaveFileString(lReportFilename, lReport)
                Logger.Message("Wrote " & lReportFilename, "AMEC Report", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.None,Windows.Forms.DialogResult.OK)
            End With
        End If
    End Sub

    Private Sub PutTimeseriesInGrid(ByVal aTimeseries As atcTimeseries, _
                                    ByVal aGrid As atcTableDelimited, _
                                    ByVal aGridColumn As Integer, _
                                    ByVal aSeasonName As String, _
                                    ByVal aSeasonStart As String, _
                                    ByVal aSeasonEnd As String, _
                                    ByRef lValuesProbability() As Double, _
                                    ByRef lPlotValues() As Double _
                                    )
        Dim lValues() As Double

        Dim lValueIndex As Integer
        If aSeasonName = "Annual" Then
            lValues = aTimeseries.Values.Clone
        Else
            Dim lSeasonData As atcSeasonsYearSubset
            Dim lStartMonthDay() As String = aSeasonStart.Split("/")
            Dim lEndMonthDay() As String = aSeasonEnd.Split("/")
            lSeasonData = New atcSeasonsYearSubset(lStartMonthDay(0), lStartMonthDay(1), lEndMonthDay(0), lEndMonthDay(1))
            Dim lSimTimser As atcTimeseries = lSeasonData.Split(aTimeseries, Nothing).ItemByKey(1)
            lValues = lSimTimser.Values
        End If
        Dim lNumNonMissingValues As Integer = 0
        Dim lNonMissingValues(lValues.GetUpperBound(0)) As Double
        For lValueIndex = 0 To lValues.GetUpperBound(0)
            If Not Double.IsNaN(lValues(lValueIndex)) AndAlso lValues(lValueIndex) > 0.00005 Then
                lNonMissingValues(lNumNonMissingValues) = lValues(lValueIndex)
                lNumNonMissingValues += 1
            End If
        Next
        ReDim Preserve lNonMissingValues(lNumNonMissingValues - 1)
        ReDim lValuesProbability(lNumNonMissingValues - 1)
        ReDim lPlotValues(lNumNonMissingValues - 1)
        System.Array.Sort(lNonMissingValues)
        With aGrid
            .CurrentRecord = 1
            .FieldName(aGridColumn) = "Value"
            .FieldName(aGridColumn + 1) = "Prob"
            Dim lNonMissingIndex As Integer = 1
            Dim lDivideBy As Integer = lNumNonMissingValues + 1
            For lValueIndex = 0 To lNumNonMissingValues - 1
                .Value(aGridColumn) = DoubleToString(lNonMissingValues(lValueIndex), , "#0.0000")
                .Value(aGridColumn + 1) = DoubleToString(lNonMissingIndex / lDivideBy, , "#0.0000")
                lPlotValues(.CurrentRecord - 1) = lNonMissingValues(lValueIndex)
                lValuesProbability(.CurrentRecord - 1) = lNonMissingIndex / lDivideBy
                .CurrentRecord += 1
                lNonMissingIndex += 1
            Next
        End With



    End Sub
    
End Module