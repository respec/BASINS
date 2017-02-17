Imports System.Drawing

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph

Public Class clsGraphBoxWhisker
    Inherits clsGraphBase

    Private pXLabels As Generic.List(Of String) = Nothing
    Public Property XLabels() As Generic.List(Of String)
        Get
            Return pXLabels
        End Get
        Set(value As Generic.List(Of String))
            If value IsNot Nothing Then
                If pXLabels Is Nothing Then
                    pXLabels = New Generic.List(Of String)()
                Else
                    pXLabels.Clear()
                End If
                For I As Integer = 0 To value.Count - 1
                    pXLabels.Add(value(I))
                Next
            End If
        End Set
    End Property

    Private pDataColors As Generic.List(Of Color) = Nothing
    Public Property DataColors() As Generic.List(Of Color)
        Get
            Return pDataColors
        End Get
        Set(value As Generic.List(Of Color))
            If value IsNot Nothing Then
                If pDataColors Is Nothing Then
                    pDataColors = New Generic.List(Of Color)()
                Else
                    pDataColors.Clear()
                End If
                For I As Integer = 0 To value.Count - 1
                    pDataColors.Add(value(I))
                Next
            End If
        End Set
    End Property

    Private pXTitle As String = ""
    Public Property XTitle() As String
        Get
            Return pXTitle
        End Get
        Set(value As String)
            pXTitle = value
        End Set
    End Property

    Public XLabelBaseline As Double = Double.NaN

    Private pYTitle As String = ""
    Public Property YTitle() As String
        Get
            Return pYTitle
        End Get
        Set(value As String)
            pYTitle = value
        End Set
    End Property

    Private pYUseLog As Boolean = False
    Public Property YUseLog() As Boolean
        Get
            Return pYUseLog
        End Get
        Set(value As Boolean)
            pYUseLog = value
        End Set
    End Property

    Private pTitle As String = "No Title"
    Public Property Title() As String
        Get
            Return pTitle
        End Get
        Set(value As String)
            pTitle = value
        End Set
    End Property

    Private pOutputFile As String = ""
    Public Property OutputFile() As String
        Get
            Return pOutputFile
        End Get
        Set(value As String)
            pOutputFile = value
        End Set
    End Property

    Private pShowFullRange As Boolean = True
    Public Property ShowFullRange() As Boolean
        Get
            Return pShowFullRange
        End Get
        Set(value As Boolean)
            pShowFullRange = value
        End Set
    End Property

    Private pShowLegend As Boolean = False
    Public Property ShowLegend() As Boolean
        Get
            Return pShowLegend
        End Get
        Set(value As Boolean)
            pShowLegend = value
        End Set
    End Property

    Private pXLabelAngle As Single = Single.NaN
    Public Property XLabelAngle() As Single
        Get
            Return pXLabelAngle
        End Get
        Set(value As Single)
            pXLabelAngle = value
        End Set
    End Property

    <CLSCompliant(False)>
    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZedGraphControl As ZedGraphControl, Optional aDelaySetup As Boolean = False)
        MyBase.New(aDataGroup, aZedGraphControl)
        If Not aDelaySetup Then
            SetUpGraph(True)
        End If
    End Sub

    Public Overrides Property Datasets() As atcTimeseriesGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal newValue As atcTimeseriesGroup)
            If newValue.Count > 1 Then
                MyBase.Datasets = newValue
            End If
        End Set
    End Property

    Public Sub SetUpGraph(Optional OutputToFile As Boolean = False)
        If Datasets.Count = 0 Then
            Exit Sub
        End If
        'Dim lTimeseriesX As atcTimeseries = newValue.ItemByIndex(0)
        'Dim lTimeseriesY As atcTimeseries = newValue.ItemByIndex(1)
        'If lTimeseriesX.Attributes.GetValue("timeunit") <> lTimeseriesY.Attributes.GetValue("timeunit") Then
        '    Logger.Msg("BoxWhisker plot requires two timeseries to be of same time unit.")
        '    Exit Property
        'End If

        'Set up the chart
        pZgc.GraphPane.BarSettings.Type = BarType.Overlay
        pZgc.GraphPane.XAxis.IsVisible = False
        pZgc.GraphPane.Legend.IsVisible = pShowLegend
        If String.Compare(Title(), "no title", True) = 0 Then
        ElseIf String.IsNullOrEmpty(Title()) Then
            Dim lcon As String = Datasets(0).Attributes.GetValue("Constituent", "Value")
            pZgc.GraphPane.Title.Text = lcon & " Ranges"
        Else
            pZgc.GraphPane.Title.Text = Title()
        End If
        Dim listDataArrays As New Generic.List(Of Double())
        For I As Integer = 0 To Datasets.Count - 1
            listDataArrays.Add(Datasets(I).Values)
        Next
        If pXLabels Is Nothing OrElse pXLabels.Count <> Datasets.Count Then
            pXLabels = New Generic.List(Of String)
            Dim lbl As String = ""
            For I As Integer = 0 To Datasets.Count - 1
                lbl = Datasets(I).Attributes.GetValue("Location", "QTY" & I.ToString())
                If Not String.IsNullOrEmpty(lbl) AndAlso lbl.Length > 8 Then
                    lbl = lbl.Substring(0, 8)
                End If
                pXLabels.Add(lbl)
            Next
        End If
        'Dim lPane As GraphPane = MyBase.pZgc.MasterPane.PaneList(0)
        'lPane.Legend.IsVisible = False
        With pZgc.GraphPane.XAxis
            '.Scale.MaxAuto = False
            If String.IsNullOrEmpty(XTitle()) Then
                With Datasets(0).Attributes
                    Dim lScen As String = .GetValue("scenario")
                    Dim lLoc As String = .GetValue("location")
                    Dim lCons As String = .GetValue("constituent")
                    Dim lUnit As String = .GetValue("Units")
                    Dim lTimeUnit As String = TimeUnitText(Datasets(0))
                    'Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)
                    pXTitle = lTimeUnit & " " & lCons & " at " & lLoc
                End With
            End If
            .Title.Text = XTitle()
        End With

        With pZgc.GraphPane.YAxis
            If YUseLog() Then
                .Type = AxisType.Log
            Else
                .Type = AxisType.Linear
            End If
            '.Scale.MaxAuto = False
            '.Scale.MinAuto = False
            '.MinSpace = 80
            If String.IsNullOrEmpty(YTitle()) Then
                With Datasets(0).Attributes
                    Dim lCons As String = .GetValue("constituent")
                    Dim lUnit As String = .GetValue("Units")
                    Dim lTimeUnit As String = TimeUnitText(Datasets(0))
                    pYTitle = lCons & " " & lUnit
                End With
            End If
            .Title.Text = YTitle() 'lTimeseriesY.ToString
        End With
        'ScaleAxis(Datasets, pZgc.GraphPane.YAxis)
        BoxPlot(listDataArrays, pXLabels)
        If Double.IsNaN(XLabelBaseline) Then
            With pZgc.GraphPane.YAxis
                If .Type = AxisType.Linear Then
                    XLabelBaseline = .Scale.Min - (.Scale.MajorStep * 10)
                ElseIf .Type = AxisType.Log Then
                    XLabelBaseline = .Scale.Min - (.Scale.MajorStep * 10)
                End If
                '.Scale.MinAuto = False
                '.Scale.Min = XLabelBaseline - .Scale.MajorStep * 10
            End With
        End If
        pZgc.GraphPane.BarSettings.ClusterScaleWidthAuto = False
        pZgc.GraphPane.BarSettings.MinClusterGap = 0.2
        pZgc.GraphPane.BarSettings.MinBarGap = 0.2
        'With pZgc.GraphPane.XAxis
        '    .Type = AxisType.Text
        '    .Scale.TextLabels = pXLabels.ToArray()
        '    .Scale.FontSpec.Angle = -90
        'End With
        pZgc.GraphPane.AxisChange()
        'pZgc.GraphPane.YAxis.Scale.MinGrace = Math.Abs(XLabelBaseline) + pZgc.GraphPane.YAxis.Scale.MajorStep * 5
        pZgc.GraphPane.Margin.Bottom = 80

        If pShowLegend Then
            pZgc.GraphPane.Legend.IsHStack = True
            pZgc.GraphPane.Legend.Border.Color = Color.LightGray
        Else
            'use labels
            Dim lAngle As Single = -90
            If Not Single.IsNaN(pXLabelAngle) AndAlso pXLabelAngle <= 90 AndAlso pXLabelAngle >= -90 Then
                lAngle = pXLabelAngle
            End If
            For I As Integer = 0 To Datasets.Count - 1
                'Dim label As TextObj = New TextObj(XLabels(I), I, XLabelBaseline, CoordType.AxisXYScale, AlignH.Center, AlignV.Center)
                Dim label As TextObj = New TextObj(XLabels(I), I, 1.1, CoordType.XScaleYChartFraction, AlignH.Center, AlignV.Center)
                label.ZOrder = ZOrder.A_InFront
                label.FontSpec.Border.IsVisible = False
                label.FontSpec.Angle = pXLabelAngle
                If pDataColors IsNot Nothing AndAlso pDataColors.Count = Datasets.Count Then
                    label.FontSpec.FontColor = pDataColors(I)
                Else
                    'label.FontSpec.FontColor = Color.Black
                End If
                pZgc.GraphPane.GraphObjList.Add(label)
            Next
        End If
        pZgc.GraphPane.AxisChange()
        'Dim leftMargin As Double = 10
        'Dim rightMargin As Double = 10
        'Dim topMargin As Double = 10
        'Dim bottomMargin As Double = 8
        'Dim width As Double = pZgc.GraphPane.Chart.Rect.Width
        'Dim height As Double = pZgc.GraphPane.Chart.Rect.Height
        'pZgc.GraphPane.Chart.Rect = New RectangleF(leftMargin, topMargin, width - leftMargin - rightMargin, height - topMargin - bottomMargin)

        If OutputToFile Then
            If IO.Directory.Exists(IO.Path.GetDirectoryName(pOutputFile)) Then
                'pZgc.GraphPane.GetImage().Save("C:\temp\test\boxwhisker.bmp")
                pZgc.GraphPane.GetImage().Save(pOutputFile)
            End If
        End If
        pZgc.Refresh()
    End Sub

    Private Sub BoxPlot(ByVal data As Generic.List(Of Double()), ByVal names As Generic.List(Of String))
        Dim myPane As GraphPane = pZgc.GraphPane
        For i As Integer = 0 To data.Count - 1
            'median of each array
            Dim medians As New PointPairList()
            '75th And 25th percentile, defines the box
            Dim hiLowList As New PointPairList()
            '+/- 1.5*Interquartile range, extentent of wiskers
            Dim barList As New PointPairList()
            'outliers
            Dim outs As New PointPairList()
            'Add the values
            Dim lmean As Double = GetStatistic(i, "mean")
            Dim lmin As Double = GetStatistic(i, "min")
            Dim lmax As Double = GetStatistic(i, "max")
            Dim lpct25 As Double = GetStatistic(i, "%25")
            Dim lpct75 As Double = GetStatistic(i, "%75")
            medians.Add(i, lmean)
            hiLowList.Add(i, lpct75, lpct25)
            Dim iqr As Double = 1.5 * (lpct75 - lpct25)
            Dim upperLimit As Double = lpct75 + iqr
            Dim lowerLimit As Double = lpct25 - iqr
            If ShowFullRange() Then
                upperLimit = lmax
                lowerLimit = lmin
                'The wiskers must end on an actual data point
                barList.Add(i, lowerLimit, upperLimit)
            Else
                'The wiskers must end on an actual data point
                barList.Add(i, ValueNearestButGreater(data(i), lowerLimit), ValueNearestButLess(data(i), upperLimit))
            End If
            ''Sort out the outliers
            'For Each aValue As Double In data(i)
            '    If (aValue > upperLimit) Then
            '        outs.Add(i, aValue)
            '    End If

            '    If (aValue < lowerLimit) Then
            '        outs.Add(i, aValue)
            '    End If
            'Next
            Dim lColor As Color = Color.Black
            If pDataColors IsNot Nothing AndAlso pDataColors.Count = data.Count Then
                lColor = DataColors(i)
            End If

            'Plot the items, first the median values
            Dim meadian As CurveItem = myPane.AddCurve("", medians, Color.Black, SymbolType.HDash)
            Dim myLine As LineItem = CType(meadian, LineItem)
            myLine.Line.IsVisible = False
            myLine.Symbol.Fill.Type = FillType.Solid
            'Box
            Dim myCurve As HiLowBarItem = Nothing
            If pDataColors IsNot Nothing AndAlso pDataColors.Count = data.Count Then
                myCurve = myPane.AddHiLowBar(names(i), hiLowList, DataColors(i))
                myCurve.Bar.Fill.Type = FillType.Solid
                myCurve.Bar.Border.Color = Color.LightGray
            Else
                myCurve = myPane.AddHiLowBar(names(i), hiLowList, Color.Black)
                myCurve.Bar.Fill.Type = FillType.None
            End If

            'Wiskers
            Dim myerror As ErrorBarItem = myPane.AddErrorBar("", barList, lColor)
            'Outliers
            'Dim upper As CurveItem = myPane.AddCurve("", outs, Color.Black, SymbolType.XCross)
            'Dim bLine As LineItem = CType(upper, LineItem)
            'bLine.Symbol.Size = 3
            'bLine.Line.IsVisible = False

            'Dim label As TextObj = New TextObj(names(i), i, 2.0, CoordType.AxisXYScale, AlignH.Center, AlignV.Center)
            'label.ZOrder = ZOrder.A_InFront
            'Label.FontSpec.Border.IsVisible = False
            'myPane.GraphObjList.Add(label)
        Next
    End Sub

    Private Function ValueNearestButLess(ByVal data As Double(), ByVal number As Double) As Double
        Dim lowNums As Double = Double.MinValue
        For Each n As Double In data
            If n <= number Then
                lowNums = Math.Max(n, lowNums)
            End If
        Next
        Return lowNums
    End Function

    Private Function ValueNearestButGreater(ByVal data As Double(), ByVal number As Double) As Double
        Dim lowNums As Double = Double.MaxValue
        For Each n As Double In data
            If n >= number Then
                lowNums = Math.Min(n, lowNums)
            End If
        Next
        Return lowNums
    End Function

    Private Function GetStatistic(ByVal aDSIndex As Integer, ByVal aStatName As String) As Double
        Dim lTs As atcTimeseries = Datasets(aDSIndex)
        Return lTs.Attributes.GetValue(aStatName, Double.NaN)
    End Function

    Private Function TimeUnitText(ByVal aTser As atcTimeseries) As String
        Dim lTimeUnit As String = ""
        With aTser.Attributes
            Select Case CType(.GetValue("timeunit"), atcTimeUnit)
                Case atcTimeUnit.TUYear : lTimeUnit = "Annual"
                Case atcTimeUnit.TUMonth : lTimeUnit = "Monthly"
                Case atcTimeUnit.TUDay : lTimeUnit = "Daily"
                Case atcTimeUnit.TUHour : lTimeUnit = "Hourly"
                Case atcTimeUnit.TUMinute : lTimeUnit = "Minute"
                Case atcTimeUnit.TUSecond : lTimeUnit = "Second"
                Case atcTimeUnit.TUCentury : lTimeUnit = "Century"
            End Select
        End With
        Return lTimeUnit
    End Function
End Class
