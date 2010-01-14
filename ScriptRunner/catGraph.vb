Imports atcUtility
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph
Imports System.Drawing
Imports MapWindow.Interfaces
Imports System.Collections.Specialized

Enum factors
    emscen = 1
    intensity = 2
    landuse = 3
    year = 4
    model = 5
    bmp = 6
End Enum
Enum endpoints
    flow = 23
    flow7q10 = 25
    flow1hi100 = 26
    sedsumannual = 27
    sedmean = 29
    totphos = 31
    totnitro = 32
End Enum
Module catGraph
    Private pflowBase As Double = 1223.2 'cfs
    'Private pTrial As String = "Trial1"
    Private pTrial As String = "Trial2"
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lZgc As ZedGraphControl
        lZgc = CreateZgc()
        Dim lGraphSaveWidth As Integer = 1000
        Dim lGraphSaveHeight As Integer = 600
        Dim lfilename As String = ""
        If pTrial = "Trial1" Then
            lfilename = "C:\ZDocs\mono_luChange\graphs\zedGraphs\test.emf"
        ElseIf pTrial = "Trial2" Then
            lfilename = "G:\ZDocs\Graphing\test.emf"
            GoTo DOGRAPH
        End If

        Dim labelsArrayList As New ArrayList()
        Dim lemScen As String = "A2" ' <<< change this for B2

        lZgc.Width = lGraphSaveWidth
        lZgc.Height = lGraphSaveHeight

        Dim hilowList As New PointPairList
        Dim listcccm As New PointPairList
        Dim listccsr As New PointPairList
        Dim listcsir As New PointPairList
        Dim listechm As New PointPairList
        Dim listgfdl As New PointPairList
        Dim listhadc As New PointPairList
        Dim listncar As New PointPairList

        Dim lTable As New atcTableDelimited

        'Dim lModel1 As String = ""
        'Dim lfound1Model As Boolean = False

        Dim lbl As String = ""
        Dim lthisLbl As String = ""
        Dim lblCtr As Integer = 0
        Dim lhi As Double
        Dim low As Double
        Dim spl As New StockPointList()

        With lTable
            Dim lBaseDataField As Integer
            Dim lSubIdField As Integer
            .NumHeaderRows = 0
            .Delimiter = vbTab
            lBaseDataField = 6
            lSubIdField = 3
            If .OpenFile("C:\ZDocs\mono_luChange\graphs\zedGraphs\zedCatAll.txt") Then
                Dim lField As Integer
                Dim lFieldStart As Integer = 1
                Dim lVal As String

                .CurrentRecord = 1

                lhi = -9999.0
                low = 999999999.0
                Do
                    lVal = .Value(1)
                    Try
                        'Bypass lbl and title lines
                        If lVal.StartsWith("lbl") Then
                            .CurrentRecord += 1
                            Continue Do
                        End If

                        'If lfound1Model = False Then
                        '    lModel1 = .Value(factors.model)
                        '    lfound1Model = True
                        'End If

                        If .Value(1) <> lemScen Then
                            lblCtr += 1
                            hilowList.Add(lblCtr, lhi, low)
                            labelsArrayList.Add(lbl)
                            ' Create a StockPt instead of a PointPair so we can carry 6 properties
                            Dim pt As New StockPt(lblCtr, lhi, low, (lhi - low) / 2 + low, (lhi - low) / 2 + low, 100000)
                            spl.Add(pt)

                            lbl = lthisLbl
                            lhi = -9999.0
                            low = 999999999.0

                            Exit Do
                        End If

                        lbl = lthisLbl ' save the label from previous record
                        lthisLbl = .Value(factors.emscen) & " " & .Value(factors.intensity) & " " & .Value(factors.landuse) & " " & .Value(factors.year) & " " & .Value(factors.bmp)

                        If lbl <> "" And lbl <> lthisLbl Then
                            lblCtr += 1
                            hilowList.Add(lblCtr, lhi, low)
                            labelsArrayList.Add(lbl)
                            ' Create a StockPt instead of a PointPair so we can carry 6 properties
                            Dim pt As New StockPt(lblCtr, lhi, low, (lhi - low) / 2 + low, (lhi - low) / 2 + low, 100000)
                            spl.Add(pt)

                            lbl = lthisLbl
                            lhi = -9999.0
                            low = 999999999.0
                        End If

                        'could save all the values for each of the models here
                        If .Value(factors.model) = "cccm" Then listcccm.Add(lblCtr, .Value(endpoints.flow))
                        If .Value(factors.model) = "ccsr" Then listccsr.add(lblCtr, .Value(endpoints.flow))
                        If .Value(factors.model) = "csir" Then listcsir.add(lblCtr, .Value(endpoints.flow))
                        If .Value(factors.model) = "echm" Then listechm.add(lblCtr, .Value(endpoints.flow))
                        If .Value(factors.model) = "gfdl" Then listgfdl.add(lblCtr, .Value(endpoints.flow))
                        If .Value(factors.model) = "hadc" Then listhadc.add(lblCtr, .Value(endpoints.flow))
                        If .Value(factors.model) = "ncar" Then listncar.add(lblCtr, .Value(endpoints.flow))

                        'get range for this set of 7 climate models
                        If Single.Parse(.Value(endpoints.flow)) > lhi Then lhi = .Value(endpoints.flow)
                        If Single.Parse(.Value(endpoints.flow)) < low Then low = .Value(endpoints.flow)

                    Catch ex As FormatException
                        Logger.Dbg("FormatException " & .CurrentRecord & ":" & lField & ":" & .Value(lField))
                    Catch ex As Exception
                        Logger.Dbg("Stopping reading CAT output: " & ex.Message)
                        Exit Do
                    End Try
                    .CurrentRecord += 1
                Loop
            End If
        End With

        Dim lc As Integer = labelsArrayList.Count

        Dim labels(lc) As String
        For i As Integer = 0 To lc - 1
            labels(i) = labelsArrayList(i)
        Next

DOGRAPH:
        'CreateTornado(lZgc, labels, hilowList, listcccm, "Scenario", "Flow")
        'CreateTornadoOHLC(lZgc, labels, spl, listcccm, listccsr, listcsir, listechm, listgfdl, listhadc, listncar, "Scenario", "Flow")
        'CreateChart1(lZgc)
        'CreateChartHBar(lZgc)
        'CreateChartErrorBar(lZgc)
        'CreateChartOHLC(lZgc)
        'CreateGraphCandlestick(lZgc)
        CreateChartTD(lZgc)
        lZgc.SaveAs(lfilename)
        OpenFile(lfilename)
    End Sub

    Public Sub CreateTornadoOHLC(ByVal azgc As ZedGraphControl, _
                                 ByVal alabels As String(), _
                                 ByVal aspl As StockPointList, _
                                 ByVal amodelcccm As PointPairList, _
                                 ByVal amodelccsr As PointPairList, _
                                 ByVal amodelcsir As PointPairList, _
                                 ByVal amodelechm As PointPairList, _
                                 ByVal amodelgfdl As PointPairList, _
                                 ByVal amodelhadc As PointPairList, _
                                 ByVal amodelncar As PointPairList, _
                                 ByVal axtitle As String, _
                                 ByVal aytitle As String)
        Dim myPane As GraphPane = azgc.GraphPane

        ' Setup the gradient fill...
        ' Use Red for negative days and black for positive days
        Dim colors As Color() = {Color.Red, Color.Black}
        Dim myFill As New Fill(colors)
        myFill.Type = FillType.GradientByColorValue
        myFill.SecondaryValueGradientColor = Color.Empty
        myFill.RangeMin = 1
        myFill.RangeMax = 2


        Dim lpaneBgcolor As Color = Color.White
        Dim lpaneBgfill As New Fill(lpaneBgcolor)
        lpaneBgfill.Type = FillType.Solid

        ' Set the title and axis labels   
        'myPane.Title.Text = "Tornado OHLC"
        'myPane.XAxis.Title.Text = axtitle

        myPane.Title.Text = ""
        myPane.XAxis.Title.Text = ""
        myPane.YAxis.Title.Text = aytitle
        myPane.BarSettings.Base = BarBase.Y
        'myPane.Fill = lpaneBgfill
        'myPane.Fill.Type = FillType.None
        myPane.Margin.Bottom = 100

        'myPane.X2Axis.Cross = 1223.2
        'myPane.X2Axis.IsVisible = True
        'myPane.X2Axis.MajorTic.Size = 1
        'myPane.X2Axis.MinorTic.Size = 1

        ' Make a new curve with a "cccm" label
        Dim curvecccm As LineItem = myPane.AddCurve("cccm", amodelcccm, Color.Black, SymbolType.Circle)
        ' Turn off the line display, symbols only
        curvecccm.Line.IsVisible = False
        ' Fill the symbols with solid red color
        'curve.Symbol.Fill = New Fill(Color.Red)
        curvecccm.Symbol.Size = 7
        curvecccm.Symbol.Fill.Type = FillType.None

        ' Make a new curve with a "ccsr" label
        Dim curveccsr As LineItem = myPane.AddCurve("ccsr", amodelccsr, Color.Black, SymbolType.Diamond)
        ' Turn off the line display, symbols only
        curveccsr.Line.IsVisible = False
        ' Fill the symbols with solid red color
        'curve.Symbol.Fill = New Fill(Color.Red)
        curveccsr.Symbol.Size = 7
        curveccsr.Symbol.Fill.Type = FillType.None

        ' Make a new curve with a "csir" label
        Dim curvecsir As LineItem = myPane.AddCurve("csir", amodelcsir, Color.Black, SymbolType.Square)
        ' Turn off the line display, symbols only
        curvecsir.Line.IsVisible = False
        ' Fill the symbols with solid red color
        'curve.Symbol.Fill = New Fill(Color.Red)
        curvecsir.Symbol.Size = 7
        curvecsir.Symbol.Fill.Type = FillType.None

        ' Make a new curve with a "echm" label
        Dim curveechm As LineItem = myPane.AddCurve("echm", amodelechm, Color.Black, SymbolType.Triangle)
        ' Turn off the line display, symbols only
        curveechm.Line.IsVisible = False
        ' Fill the symbols with solid red color
        'curve.Symbol.Fill = New Fill(Color.Red)
        curveechm.Symbol.Size = 7
        curveechm.Symbol.Fill.Type = FillType.None

        ' Make a new curve with a "gfdl" label
        Dim curvegfdl As LineItem = myPane.AddCurve("gfdl", amodelgfdl, Color.Black, SymbolType.Plus)
        ' Turn off the line display, symbols only
        curvegfdl.Line.IsVisible = False
        ' Fill the symbols with solid red color
        'curve.Symbol.Fill = New Fill(Color.Red)
        curvegfdl.Symbol.Size = 7
        curvegfdl.Symbol.Fill.Type = FillType.None

        ' Make a new curve with a "hadc" label
        Dim curvehadc As LineItem = myPane.AddCurve("hadc", amodelhadc, Color.Black, SymbolType.HDash)
        ' Turn off the line display, symbols only
        curvehadc.Line.IsVisible = False
        ' Fill the symbols with solid red color
        'curve.Symbol.Fill = New Fill(Color.Red)
        curvehadc.Symbol.Size = 7
        curvehadc.Symbol.Fill.Type = FillType.None

        ' Make a new curve with a "ncar" label
        Dim curvencar As LineItem = myPane.AddCurve("ncar", amodelncar, Color.Black, SymbolType.Star)
        ' Turn off the line display, symbols only
        curvencar.Line.IsVisible = False
        ' Fill the symbols with solid red color
        'curve.Symbol.Fill = New Fill(Color.Red)
        curvencar.Symbol.Size = 7
        curvencar.Symbol.Fill.Type = FillType.None

        'Create the OHLC and assign it a Fill
        Dim myCurve As OHLCBarItem = myPane.AddOHLCBar("Endpoint Range", aspl, Color.Empty)
        'myCurve.Bar.GradientFill = myFill
        myCurve.Bar.IsAutoSize = True
        myCurve.Bar.Width = 1
        myCurve.Bar.Size = 0 'the open and close tic's wing size

        ' Use DateAsOrdinal to skip weekend gaps
        myPane.XAxis.Type = AxisType.Text
        'myPane.XAxis.Cross = 1223.2
        myPane.XAxis.Scale.TextLabels = alabels
        myPane.XAxis.Scale.AlignH = AlignH.Left
        'myPane.XAxis.Scale.Min = new XDate( 2006, 1, 1 );
        myPane.XAxis.Scale.FontSpec.Angle = -90
        myPane.XAxis.Scale.LabelGap = -5

        ' pretty it up a little
        'myPane.Chart.Fill = New Fill(Color.White, Color.LightGoldenrodYellow, 45.0F)
        'myPane.Fill = New Fill(Color.White, Color.FromArgb(220, 220, 255), 45.0F)
        'myPane.Chart.Fill = New Fill(Color.White)
        'myPane.Fill = New Fill(Color.White)

        myPane.Title.FontSpec.Size = 20.0F
        myPane.XAxis.Title.FontSpec.Size = 18.0F
        myPane.XAxis.Scale.FontSpec.Size = 12.0F
        myPane.XAxis.Scale.FontSpec.Family = "Courier New"
        myPane.YAxis.Title.FontSpec.Size = 18.0F
        myPane.YAxis.Scale.FontSpec.Size = 16.0F
        myPane.Legend.IsVisible = False

        AddTextFromControls("SRES A2", myPane)
        myPane.Legend.IsVisible = True
        myPane.Legend.IsHStack = True

        myPane.Legend.Position = LegendPos.TopCenter
        myPane.Legend.Border.IsVisible = True
        'myPane.Legend.Gap = 0.05
        'myPane.Legend.Position = LegendPos.Float
        'myPane.Legend.Location = New Location(0.9, 0.05, CoordType.PaneFraction)

        ' Tell ZedGraph to calculate the axis ranges
        azgc.AxisChange()
    End Sub

    Public Sub CreateTornado(ByVal azgc As ZedGraphControl, ByVal alabels As String(), ByVal ahilow As PointPairList, ByVal amodel As PointPairList, ByVal axtitle As String, ByVal aytitle As String)
        ' Call this method from the Form_Load method, passing your ZedGraphControl
        Dim myPane As GraphPane = azgc.GraphPane

        ' Set the title and axis labels
        'myPane.Title.Text = "ZedgroSoft, International" & vbLf & "Hi-Low-Close Daily Stock Chart"
        myPane.XAxis.Title.Text = axtitle
        myPane.YAxis.Title.Text = aytitle

        ' Set the title font characteristics
        myPane.Title.FontSpec.Family = "Arial"
        myPane.Title.FontSpec.IsItalic = True
        myPane.Title.FontSpec.Size = 18

        ' Make a new curve with a "cccm" label
        Dim curve As LineItem = myPane.AddCurve("cccm", amodel, Color.Black, SymbolType.Diamond)
        ' Turn off the line display, symbols only
        curve.Line.IsVisible = False
        ' Fill the symbols with solid red color
        curve.Symbol.Fill = New Fill(Color.Red)
        curve.Symbol.Size = 7

        ' Add a blue error bar to the graph
        Dim myCurve As ErrorBarItem = myPane.AddErrorBar("Endpoint Range", ahilow, Color.Blue)
        myCurve.Bar.PenWidth = 3
        myCurve.Bar.Symbol.IsVisible = True

        ' Set the XAxis to date type
        'myPane.XAxis.Type = AxisType.[Date]
        ' X axis step size is 1 day
        'myPane.XAxis.Scale.MajorStep = 1
        'myPane.XAxis.Scale.MajorUnit = DateUnit.Day
        ' tilt the x axis labels to an angle of 65 degrees
        'myPane.XAxis.Scale.FontSpec.Angle = 65
        'myPane.XAxis.Scale.FontSpec.IsBold = True
        'myPane.XAxis.Scale.FontSpec.Size = 12
        'myPane.XAxis.Scale.Format = "d MMM"
        '' make the x axis scale minimum 1 step less than the minimum data value
        'myPane.XAxis.Scale.Min = hList(0).X - 1

        ' Set the XAxis labels
        myPane.XAxis.Scale.TextLabels = alabels
        ' Set the XAxis to Text type
        myPane.XAxis.Type = AxisType.Text
        myPane.XAxis.Scale.FontSpec.Family = "Courier New"
        myPane.XAxis.Scale.FontSpec.Angle = -90

        ' Display the Y axis grid
        myPane.YAxis.MajorGrid.IsVisible = True
        'myPane.YAxis.Scale.MinorStep = 0.5

        ' Fill the axis background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.FromArgb(255, 255, 166), 90.0F)

        ' Calculate the Axis Scale Ranges
        azgc.AxisChange()
    End Sub
    ' Call this method from the Form_Load method, passing your ZedGraphControl

    Public Sub CreateChartOHLC(ByVal zgc As ZedGraphControl)
        Dim myPane As GraphPane = zgc.GraphPane

        ' Set the title and axis labels   
        myPane.Title.Text = "OHLC Chart Demo"
        myPane.XAxis.Title.Text = "Date"
        myPane.YAxis.Title.Text = "Price"

        ' Load a StockPointList with random data...
        Dim spl As New StockPointList()
        Dim rand As New Random()

        ' First day is jan 1st
        Dim xDate__1 As New XDate(2006, 1, 1)
        Dim open As Double = 50.0R
        Dim prevClose As Double = 0

        ' Loop to make 50 days of data
        For i As Integer = 0 To 49
            Dim x As Double = xDate__1.XLDate
            Dim close As Double = open + rand.NextDouble() * 10.0R - 5.0R
            Dim hi As Double = Math.Max(open, close) + rand.NextDouble() * 5.0R
            Dim low As Double = Math.Min(open, close) - rand.NextDouble() * 5.0R

            ' Create a StockPt instead of a PointPair so we can carry 6 properties
            Dim pt As New StockPt(x, hi, low, open, close, 100000)

            'if price is increasing color=black, else color=red
            If close > prevClose Then
                pt.ColorValue = 2
            Else
                pt.ColorValue = 1
            End If
            spl.Add(pt)

            prevClose = close
            open = close
            ' Advance one day
            xDate__1.AddDays(1.0R)
            ' but skip the weekends
            If XDate.XLDateToDayOfWeek(xDate__1.XLDate) = 6 Then
                xDate__1.AddDays(2.0R)
            End If
        Next

        ' Setup the gradient fill...
        ' Use Red for negative days and black for positive days
        Dim colors As Color() = {Color.Red, Color.Black}
        Dim myFill As New Fill(colors)
        myFill.Type = FillType.GradientByColorValue
        myFill.SecondaryValueGradientColor = Color.Empty
        myFill.RangeMin = 1
        myFill.RangeMax = 2

        'Create the OHLC and assign it a Fill
        Dim myCurve As OHLCBarItem = myPane.AddOHLCBar("Price", spl, Color.Empty)
        myCurve.Bar.GradientFill = myFill
        myCurve.Bar.IsAutoSize = True

        ' Use DateAsOrdinal to skip weekend gaps
        myPane.XAxis.Type = AxisType.DateAsOrdinal
        'myPane.XAxis.Scale.Min = new XDate( 2006, 1, 1 );

        ' pretty it up a little
        myPane.Chart.Fill = New Fill(Color.White, Color.LightGoldenrodYellow, 45.0F)
        myPane.Fill = New Fill(Color.White, Color.FromArgb(220, 220, 255), 45.0F)
        myPane.Title.FontSpec.Size = 20.0F
        myPane.XAxis.Title.FontSpec.Size = 18.0F
        myPane.XAxis.Scale.FontSpec.Size = 16.0F
        myPane.YAxis.Title.FontSpec.Size = 18.0F
        myPane.YAxis.Scale.FontSpec.Size = 16.0F
        myPane.Legend.IsVisible = False

        ' Tell ZedGraph to calculate the axis ranges
        zgc.AxisChange()
    End Sub

    ' Call this method from the Form_Load method, passing your ZedGraphControl
    Private Sub CreateGraphCandlestick(ByVal zgc As ZedGraphControl)
        Dim myPane = zgc.GraphPane

        ' Set the title and axis labels   
        myPane.Title.Text = "Japanese Candlestick Chart Demo"
        myPane.XAxis.Title.Text = "Trading Date"
        myPane.YAxis.Title.Text = "Share Price, $US"

        Dim spl = New StockPointList()
        Dim rand As New Random()

        ' First day is jan 1st
        Dim xDate As New XDate(2006, 1, 1)
        Dim open As Double = 50.0
        Dim i As Integer, x As Double, close As Double, hi As Double, low As Double

        For i = 0 To 49

            x = xDate.XLDate
            close = open + rand.NextDouble() * 10.0 - 5.0
            hi = Math.Max(open, close) + rand.NextDouble() * 5.0
            low = Math.Min(open, close) - rand.NextDouble() * 5.0

            Dim pt As New StockPt(x, hi, low, open, close, 100000)
            spl.Add(pt)

            open = close
            ' Advance one day
            xDate.AddDays(1.0)
            ' but skip the weekends
            If ZedGraph.XDate.XLDateToDayOfWeek(xDate.XLDate) = 6 Then xDate.AddDays(2.0)
        Next i

        Dim myCurve As JapaneseCandleStickItem
        myCurve = myPane.AddJapaneseCandleStick("trades", spl)
        myCurve.Stick.IsAutoSize = True
        myCurve.Stick.Color = Color.Blue

        ' Use DateAsOrdinal to skip weekend gaps
        myPane.XAxis.Type = AxisType.DateAsOrdinal
        myPane.XAxis.Scale.Min = New XDate(2006, 1, 1)

        ' pretty it up a little
        myPane.Chart.Fill = New Fill(Color.White, Color.LightGoldenrodYellow, 45.0F)
        myPane.Fill = New Fill(Color.White, Color.FromArgb(220, 220, 255), 45.0F)

        ' Tell ZedGraph to calculate the axis ranges
        zgc.AxisChange()
        zgc.Invalidate()

    End Sub

    ' Call this method from the Form_Load method, passing your ZedGraphControl
    Public Sub CreateChartErrorBar(ByVal zgc As ZedGraphControl)
        Dim myPane As GraphPane = zgc.GraphPane

        ' Set the titles and axis labels
        myPane.Title.Text = "Error Bar Demo Chart"
        myPane.XAxis.Title.Text = "Label"
        myPane.YAxis.Title.Text = "My Y Axis"

        ' Make up some data points based on the Sine function
        Dim list As New PointPairList()
        For i As Integer = 0 To 43
            Dim x As Double = i / 44.0R
            Dim y As Double = Math.Sin(CDbl(i) * Math.PI / 15.0R)
            Dim yBase As Double = y - 0.4
            list.Add(x, y, yBase)
        Next

        ' Generate a red bar with "Curve 1" in the legend
        Dim myCurve As ErrorBarItem = myPane.AddErrorBar("Curve 1", list, Color.Red)
        ' Make the X axis the base for this curve (this is the default)
        myPane.BarSettings.Base = BarBase.X
        myCurve.Bar.PenWidth = 1.0F
        ' Use the HDash symbol so that the error bars look like I-beams
        myCurve.Bar.Symbol.Type = SymbolType.HDash
        myCurve.Bar.Symbol.Border.Width = 0.1
        myCurve.Bar.Symbol.IsVisible = True
        myCurve.Bar.Symbol.Size = 4

        ' Fill the axis background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.LightGoldenrodYellow, 45.0F)

        ' Calculate the Axis Scale Ranges
        zgc.AxisChange()
    End Sub

    ' Call this method from the Form_Load method, passing your ZedGraphControl
    Public Sub CreateChartHBar(ByVal zgc As ZedGraphControl)
        Dim myPane As GraphPane = zgc.GraphPane

        ' Set the title and axis labels
        myPane.Title.Text = "Horizontal Bar Graph"
        myPane.XAxis.Title.Text = "Performance Factor"
        myPane.YAxis.Title.Text = "Grouping"

        ' Enter some random data values
        Dim y As Double() = {100, 115, 75, -22, 98, 40, _
         -10}
        Dim y2 As Double() = {90, 100, 95, -35, 80, 35, _
         35}
        Dim y3 As Double() = {80, 110, 65, -15, 54, 67, _
         18}

        ' Generate a bar with "Curve 1" in the legend
        Dim myCurve As BarItem = myPane.AddBar("Curve 1", y, Nothing, Color.Red)
        ' Fill the bar with a red-white-red gradient
        myCurve.Bar.Fill = New Fill(Color.Red, Color.White, Color.Red, 90.0F)

        ' Generate a bar with "Curve 2" in the legend
        myCurve = myPane.AddBar("Curve 2", y2, Nothing, Color.Blue)
        ' Fill the bar with a blue-white-blue gradient for a 3d look
        myCurve.Bar.Fill = New Fill(Color.Blue, Color.White, Color.Blue, 90.0F)

        ' Generate a bar with "Curve 3" in the legend
        myCurve = myPane.AddBar("Curve 3", y3, Nothing, Color.Green)
        ' Fill the bar with a Green-white-Green gradient for a 3d look
        myCurve.Bar.Fill = New Fill(Color.White, Color.Green, 90.0F)

        ' Draw the X tics between the labels instead of at the labels
        myPane.YAxis.MajorTic.IsBetweenLabels = True

        ' Set the XAxis to an ordinal type
        myPane.YAxis.Type = AxisType.Ordinal
        ' draw the X axis zero line
        myPane.XAxis.MajorGrid.IsZeroLine = True

        'This is the part that makes the bars horizontal
        myPane.BarSettings.Base = BarBase.Y

        ' Fill the axis background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.FromArgb(255, 255, 166), 45.0F)

        zgc.AxisChange()

        BarItem.CreateBarLabels(myPane, False, "f0")
    End Sub

    ' Call this method from the Form_Load method, passing your ZedGraphControl
    Public Sub CreateChart1(ByVal zgc As ZedGraphControl)
        Dim myPane As GraphPane = zgc.GraphPane

        ' Set the title and axis labels
        myPane.Title.Text = "ZedgroSoft, International" & vbLf & "Hi-Low-Close Daily Stock Chart"
        myPane.XAxis.Title.Text = ""
        myPane.YAxis.Title.Text = "Trading Price, $US"

        ' Set the title font characteristics
        myPane.Title.FontSpec.Family = "Arial"
        myPane.Title.FontSpec.IsItalic = True
        myPane.Title.FontSpec.Size = 18


        ' Generate some random stock price data
        Dim hList As New PointPairList()
        Dim cList As New PointPairList()
        Dim arrhigh(20) As Double
        Dim arrlow(20) As Double
        Dim arrbase(20) As Double

        Dim rand As New Random()
        ' initialize the starting close price
        Dim close As Double = 45

        For i As Integer = 45 To 64
            Dim x As Double = CDbl(New XDate(2004, 12, i - 30))
            close = close + 2.0R * rand.NextDouble() - 0.5
            Dim hi As Double = close + 2.0R * rand.NextDouble()
            Dim low As Double = close - 2.0R * rand.NextDouble()
            arrhigh(i - 45) = hi
            arrlow(i - 45) = low
            arrbase(i - 45) = close
            hList.Add(x, hi, low)
            cList.Add(x, close)
        Next


        ' Make a new curve with a "Closing Price" label
        Dim curve As LineItem = myPane.AddCurve("Closing Price", cList, Color.Black, SymbolType.Diamond)
        ' Turn off the line display, symbols only
        curve.Line.IsVisible = False
        ' Fill the symbols with solid red color
        curve.Symbol.Fill = New Fill(Color.Red)
        curve.Symbol.Size = 7

        ' Add a blue error bar to the graph
        'Dim myCurve As ErrorBarItem = myPane.AddErrorBar("Price Range", hList, Color.Blue)
        Dim myCurve As ErrorBarItem = myPane.AddErrorBar("Price Range", arrhigh, arrlow, arrbase, Color.Blue)
        myCurve.Bar.PenWidth = 3
        myCurve.Bar.Symbol.IsVisible = False

        ' Set the XAxis to date type
        myPane.XAxis.Type = AxisType.[Date]
        ' X axis step size is 1 day
        myPane.XAxis.Scale.MajorStep = 1
        myPane.XAxis.Scale.MajorUnit = DateUnit.Day
        ' tilt the x axis labels to an angle of 65 degrees
        myPane.XAxis.Scale.FontSpec.Angle = 65
        myPane.XAxis.Scale.FontSpec.IsBold = True
        myPane.XAxis.Scale.FontSpec.Size = 12
        myPane.XAxis.Scale.Format = "d MMM"
        ' make the x axis scale minimum 1 step less than the minimum data value
        myPane.XAxis.Scale.Min = hList(0).X - 1

        ' Display the Y axis grid
        myPane.YAxis.MajorGrid.IsVisible = True
        myPane.YAxis.Scale.MinorStep = 0.5

        ' Fill the axis background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.FromArgb(255, 255, 166), 90.0F)

        ' Calculate the Axis Scale Ranges
        zgc.AxisChange()
    End Sub

    Public Sub CreateChart(ByVal zgc As ZedGraphControl)
        Dim myPane As GraphPane = zgc.GraphPane

        ' Set the title and axis labels
        myPane.Title.Text = "Cat Stats"
        myPane.YAxis.Title.Text = "Big Cats"
        myPane.XAxis.Title.Text = "Population"

        ' Make up some data points
        Dim labels As String() = {"Panther", "Lion", "Cheetah", "Cougar", "Tiger", "Leopard"}
        Dim x As Double() = {100, 115, 75, 22, 98, 40}
        Dim x2 As Double() = {120, 175, 95, 57, 113, 110}
        Dim x3 As Double() = {204, 192, 119, 80, 134, 156}

        ' Generate a red bar with "Curve 1" in the legend
        Dim myCurve As BarItem = myPane.AddBar("Here", x, Nothing, Color.Red)
        ' Fill the bar with a red-white-red color gradient for a 3d look
        myCurve.Bar.Fill = New Fill(Color.Red, Color.White, Color.Red, 90.0F)

        ' Generate a blue bar with "Curve 2" in the legend
        myCurve = myPane.AddBar("There", x2, Nothing, Color.Blue)
        ' Fill the bar with a Blue-white-Blue color gradient for a 3d look
        myCurve.Bar.Fill = New Fill(Color.Blue, Color.White, Color.Blue, 90.0F)

        ' Generate a green bar with "Curve 3" in the legend
        myCurve = myPane.AddBar("Elsewhere", x3, Nothing, Color.Green)
        ' Fill the bar with a Green-white-Green color gradient for a 3d look
        myCurve.Bar.Fill = New Fill(Color.Green, Color.White, Color.Green, 90.0F)

        ' Draw the Y tics between the labels instead of at the labels
        myPane.YAxis.MajorTic.IsBetweenLabels = True

        ' Set the YAxis labels
        myPane.YAxis.Scale.TextLabels = labels
        ' Set the YAxis to Text type
        myPane.YAxis.Type = AxisType.Text

        ' Set the bar type to stack, which stacks the bars by automatically accumulating the values
        myPane.BarSettings.Type = BarType.Stack

        ' Make the bars horizontal by setting the BarBase to "Y"
        myPane.BarSettings.Base = BarBase.Y

        ' Fill the chart background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.FromArgb(255, 255, 166), 45.0F)

        ' Calculate the Axis Scale Ranges
        zgc.AxisChange()
    End Sub

    ' Call this method from the Form_Load method, passing your ZedGraphControl
    Private Sub CreateGraph(ByVal zgc As ZedGraphControl)
        Dim myMaster As MasterPane = zgc.MasterPane

        myMaster.PaneList.Clear()

        ' Set the masterpane title
        myMaster.Title.Text = "ZedGraph MasterPane Example"
        myMaster.Title.IsVisible = True

        ' Fill the masterpane background with a color gradient
        myMaster.Fill = New Fill(Color.White, Color.MediumSlateBlue, 45.0F)

        ' Set the margins to 10 points
        myMaster.Margin.All = 10

        ' Enable the masterpane legend
        myMaster.Legend.IsVisible = True
        myMaster.Legend.Position = LegendPos.TopCenter

        ' Add a priority stamp
        Dim text As New TextObj("Priority", 0.88F, 0.12F)
        text.Location.CoordinateFrame = CoordType.PaneFraction
        text.FontSpec.Angle = 15.0F
        text.FontSpec.FontColor = Color.Red
        text.FontSpec.IsBold = True
        text.FontSpec.Size = 16
        text.FontSpec.Border.IsVisible = False
        text.FontSpec.Border.Color = Color.Red
        text.FontSpec.Fill.IsVisible = False
        text.Location.AlignH = AlignH.Left
        text.Location.AlignV = AlignV.Bottom
        myMaster.GraphObjList.Add(text)

        ' Add a draft watermark
        text = New TextObj("DRAFT", 0.5F, 0.5F)
        text.Location.CoordinateFrame = CoordType.PaneFraction
        text.FontSpec.Angle = 30.0F
        text.FontSpec.FontColor = Color.FromArgb(70, 255, 100, 100)
        text.FontSpec.IsBold = True
        text.FontSpec.Size = 100
        text.FontSpec.Border.IsVisible = False
        text.FontSpec.Fill.IsVisible = False
        text.Location.AlignH = AlignH.Center
        text.Location.AlignV = AlignV.Center
        text.ZOrder = ZOrder.A_InFront
        myMaster.GraphObjList.Add(text)

        ' Initialize a color and symbol type rotator
        Dim rotator As New ColorSymbolRotator()

        ' Create some new GraphPanes
        Dim j As Integer
        For j = 0 To 4
            ' Create a new graph - rect dimensions do not matter here, since it
            ' will be resized by MasterPane.AutoPaneLayout()
            Dim myPane As New GraphPane(New Rectangle(10, 10, 10, 10), _
               "Case #" + (j + 1).ToString(), _
               "Time, Days", _
               "Rate, m/s")

            ' Fill the GraphPane background with a color gradient
            myPane.Fill = New Fill(Color.White, Color.LightYellow, 45.0F)
            myPane.BaseDimension = 6.0F

            ' Make up some data arrays based on the Sine function
            Dim list As New PointPairList()
            Dim i As Integer, x As Double, y As Double
            For i = 0 To 35
                x = i + 5
                y = 3.0 * (1.5 + Math.Sin(i * 0.2 + j))
                list.Add(x, y)
            Next i

            ' Add a curve to the Graph, use the next sequential color and symbol
            Dim myCurve As LineItem = myPane.AddCurve("Type " + j.ToString(), _
               list, rotator.NextColor, rotator.NextSymbol)
            ' Fill the symbols with white to make them opaque
            myCurve.Symbol.Fill = New Fill(Color.White)

            ' Add the GraphPane to the MasterPane
            myMaster.Add(myPane)
        Next j

        ' Tell ZedGraph to auto layout the new GraphPanes
        myMaster.SetLayout(zgc.CreateGraphics(), PaneLayout.ExplicitRow32)
        ' Calculate the Axis Scale Ranges
        zgc.AxisChange()
    End Sub

    Private Sub AddTextFromControls(ByVal alblText As String, ByVal aPane As GraphPane)
        Dim lText As New TextObj(alblText, 0.9, 0.1)
        lText.Location.CoordinateFrame = CoordType.PaneFraction
        lText.FontSpec.FontColor = Color.Black
        lText.FontSpec.IsBold = True
        lText.FontSpec.Size = 16
        lText.FontSpec.Border.IsVisible = False
        lText.FontSpec.Fill.IsVisible = False
        lText.Location.AlignH = AlignH.Left
        lText.Location.AlignV = AlignV.Top
        aPane.GraphObjList.Add(lText)
    End Sub

    ' Call this method from the Form_Load method, passing your ZedGraphControl
    Public Sub CreateChartTD(ByVal zgc As ZedGraphControl)
        Dim myPane As GraphPane = zgc.GraphPane

        ' Set the title and axis labels

        'Scenario	LowPct	HiPct
        '(8) RS	0	7.62
        '(17) AFNSTRS	-1.96	5.03
        '(21) AFRate	-5.36	3.58
        '(27) PCP	-6.38	4.32
        '(59) AFEff	-12.20	12.70
        '(64) TMP	-39.03	-5.99

        myPane.Title.Text = "N Applied"
        myPane.YAxis.Title.Text = "Scenarios"
        myPane.XAxis.Title.Text = "Percent change from baseline value (%)"
        myPane.Legend.Position = LegendPos.TopCenter
        myPane.Legend.IsVisible = False

        ' Make up some data points
        Dim labels As String() = {"(8) RS", "(17) AFNSTRS", "(21) AFRate", "(27) PCP", "(59) AFEff", "(64) TMP"}

        Dim x As Double() = {0, -1.96, -5.36, -6.38, -12.2, -39.03}
        Dim x2 As Double() = {7.62, 5.03, 3.58, 4.32, 12.7, -5.99}

        ' Generate a blue bar with "Curve 2" in the legend
        Dim myCurve As BarItem = myPane.AddBar("", x2, Nothing, Color.Blue)
        ' Fill the bar with a Blue-white-Blue color gradient for a 3d look
        myCurve.Bar.Fill = New Fill(Color.Red, Color.White, Color.Blue, 90.0F)
        myCurve.Item(5).ColorValue = 0.0

        ' Generate a red bar with "Curve 1" in the legend
        myCurve = myPane.AddBar("", x, Nothing, Color.Blue)
        ' Fill the bar with a red-white-red color gradient for a 3d look
        myCurve.Bar.Fill = New Fill(Color.Blue, Color.White, Color.Blue, 90.0F)


        ' Draw the Y tics between the labels instead of at the labels
        myPane.YAxis.MajorTic.IsBetweenLabels = True

        ' Set the YAxis labels
        myPane.YAxis.Scale.TextLabels = labels
        ' Set the YAxis to Text type
        myPane.YAxis.Type = AxisType.Text

        ' Set the bar type to stack, which stacks the bars by automatically accumulating the values
        myPane.BarSettings.Type = BarType.Overlay ' Important

        ' Make the bars horizontal by setting the BarBase to "Y"
        myPane.BarSettings.Base = BarBase.Y  ' Important

        ' Fill the chart background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.FromArgb(255, 255, 166), 45.0F)

        ' Calculate the Axis Scale Ranges
        zgc.AxisChange()
    End Sub

End Module
