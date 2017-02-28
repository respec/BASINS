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

    Private pShowOutliers As Boolean = False
    Public Property ShowOutliers() As Boolean
        Get
            Return pShowOutliers
        End Get
        Set(value As Boolean)
            pShowOutliers = value
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

    Private pShowXLabelColor As Boolean = False
    Public Property ShowXLabelColor() As Boolean
        Get
            Return pShowXLabelColor
        End Get
        Set(value As Boolean)
            pShowXLabelColor = value
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
            SetUpGraph()
        End If
    End Sub

    Public Overrides Property Datasets() As atcTimeseriesGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal newValue As atcTimeseriesGroup)
            If newValue IsNot Nothing AndAlso newValue.Count > 0 Then
                MyBase.Datasets = newValue
            End If
        End Set
    End Property

    Private pDatasetsCalc As atcTimeseriesGroup = Nothing
    ''' <summary>
    ''' for taking in atcCollection of data 
    ''' "Key"-->(val0, val1, val2, ...)  
    ''' </summary>
    Private pDatasetsCollection As atcCollection = Nothing
    Public Property DatasetsCollection() As atcCollection
        Get
            Return pDatasetsCollection
        End Get
        Set(value As atcCollection)
            pDatasetsCollection = value
            If value IsNot Nothing Then
                pDatasetsCalc = New atcTimeseriesGroup()
                Dim lDates(5) As Integer
                Dim lStartDate As Double = Date2J(1900, 1, 1, 0, 0, 0)
                Dim lEndDate As Double = 0
                For I As Integer = 0 To pDatasetsCollection.Count - 1
                    If pDatasetsCollection.ItemByIndex(I).GetType().BaseType.Name = "ValueType" Then
                        lEndDate = lStartDate + JulianHour * 24.0
                    Else
                        lEndDate = lStartDate + JulianHour * 24.0 * pDatasetsCollection.ItemByIndex(I).Length
                    End If
                    Dim lNewTS As New atcTimeseries(Nothing)
                    If pDatasetsCollection.ItemByIndex(I).GetType().BaseType.Name = "ValueType" Then
                        lNewTS.Values = New Double() {pDatasetsCollection.ItemByIndex(I)}
                    Else
                        lNewTS.Values = pDatasetsCollection.ItemByIndex(I)
                    End If
                    lNewTS.Dates = NewTimeseries(lStartDate, lEndDate, atcTimeUnit.TUDay, 1, Nothing)
                    pDatasetsCalc.Add(lNewTS)
                Next
            End If
        End Set
    End Property


    Public Overridable Sub SetUpGraph(Optional OutputToFile As Boolean = False)
        If Datasets Is Nothing OrElse Datasets.Count = 0 Then
            If pDatasetsCollection Is Nothing OrElse pDatasetsCollection.Count = 0 Then
                Exit Sub
            End If
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
        pZgc.GraphPane.Title.FontSpec.Size = 14
        pZgc.GraphPane.Title.FontSpec.IsBold = False
        If String.Compare(Title(), "no title", True) = 0 Then
        ElseIf String.IsNullOrEmpty(Title()) Then
            Dim lcon As String = Datasets(0).Attributes.GetValue("Constituent", "Value")
            pZgc.GraphPane.Title.Text = lcon & " Ranges"
        Else
            pZgc.GraphPane.Title.Text = Title()
        End If
        Dim listDataArrays As New Generic.List(Of Double())
        If Datasets IsNot Nothing AndAlso Datasets.Count > 0 Then
            For I As Integer = 0 To Datasets.Count - 1
                listDataArrays.Add(Datasets(I).Values)
            Next
        Else
            If pDatasetsCollection IsNot Nothing AndAlso pDatasetsCollection.Count > 0 Then
                For I As Integer = 0 To pDatasetsCollection.Count - 1
                    listDataArrays.Add(pDatasetsCollection.ItemByIndex(I))
                Next
            Else
                pZgc.Refresh()
                Exit Sub
            End If
        End If

        SetupXLabels()

        'Dim lPane As GraphPane = MyBase.pZgc.MasterPane.PaneList(0)
        'lPane.Legend.IsVisible = False
        With pZgc.GraphPane.XAxis
            '.Scale.MaxAuto = False
            If String.IsNullOrEmpty(XTitle()) Then
                If Datasets IsNot Nothing AndAlso Datasets.Count > 0 Then
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
            .MajorTic.IsOutside = False
            .MinorTic.IsOutside = False
            .MajorTic.Color = Color.DarkGray
            .MinorTic.Color = Color.DarkGray
            .Scale.FontSpec.Size = 10
            If String.IsNullOrEmpty(YTitle()) Then
                If Datasets IsNot Nothing AndAlso Datasets.Count > 0 Then
                    With Datasets(0).Attributes
                        Dim lCons As String = .GetValue("constituent")
                        Dim lUnit As String = .GetValue("Units")
                        Dim lTimeUnit As String = TimeUnitText(Datasets(0))
                        pYTitle = lCons & " " & lUnit
                    End With
                End If
            End If
            .Title.Text = YTitle() 'lTimeseriesY.ToString
            .Title.FontSpec.Size = 12
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

        If pShowLegend Then
            pZgc.GraphPane.Legend.IsHStack = True
            pZgc.GraphPane.Legend.Border.Color = Color.DarkGray
        Else
            PlotXLabels()
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
            SaveToFile()
        End If
        pZgc.Refresh()
    End Sub

    Friend Sub SetupXLabels()
        If Datasets IsNot Nothing AndAlso Datasets.Count > 0 Then
            If pXLabels Is Nothing OrElse pXLabels.Count <> Datasets.Count Then
                pXLabels = New Generic.List(Of String)
                Dim lbl As String = ""
                For I As Integer = 0 To Datasets.Count - 1
                    lbl = Datasets(I).Attributes.GetValue("Location", "QTY" & I.ToString())
                    If Not String.IsNullOrEmpty(lbl) AndAlso lbl.Length > 8 Then
                        'lbl = lbl.Substring(0, 8)
                    End If
                    pXLabels.Add(lbl)
                Next
            End If
        Else
            If pDatasetsCollection IsNot Nothing AndAlso pDatasetsCollection.Count > 0 Then
                If pXLabels Is Nothing OrElse pXLabels.Count <> pDatasetsCollection.Count Then
                    pXLabels = New Generic.List(Of String)
                    Dim lbl As String = ""
                    For I As Integer = 0 To pDatasetsCollection.Keys.Count - 1
                        lbl = pDatasetsCollection.Keys(I)
                        If Not String.IsNullOrEmpty(lbl) AndAlso lbl.Length > 8 Then
                            'lbl = lbl.Substring(0, 8)
                        End If
                        pXLabels.Add(lbl)
                    Next
                End If
            Else
                pZgc.Refresh()
                Exit Sub
            End If
        End If
    End Sub

    Friend Sub PlotXLabels()
        Dim lAngle As Single = -90
        If Not Single.IsNaN(pXLabelAngle) AndAlso pXLabelAngle <= 90 AndAlso pXLabelAngle >= -90 Then
            lAngle = pXLabelAngle
        End If
        Dim lDatasetsCount As Integer = 0
        If Datasets IsNot Nothing Then
            lDatasetsCount = Datasets.Count
        ElseIf pDatasetsCalc IsNot Nothing Then
            lDatasetsCount = pDatasetsCalc.Count
        End If
        Dim labelw As TextObj = Nothing
        Dim lBufferWid As Integer = -999
        For I As Integer = 0 To lDatasetsCount - 1
            'Dim label As TextObj = New TextObj(XLabels(I), I, XLabelBaseline, CoordType.AxisXYScale, AlignH.Center, AlignV.Center)
            Dim label As TextObj = New TextObj(XLabels(I), I, 1.01, CoordType.XScaleYChartFraction, AlignH.Left, AlignV.Center)
            label.ZOrder = ZOrder.A_InFront

            label.FontSpec.Border.IsVisible = False
            label.FontSpec.Angle = pXLabelAngle
            If pShowXLabelColor AndAlso pDataColors IsNot Nothing AndAlso pDataColors.Count >= lDatasetsCount Then
                label.FontSpec.FontColor = pDataColors(I)
            Else
                'label.FontSpec.FontColor = Color.Black
            End If

            If System.Windows.Forms.TextRenderer.MeasureText(XLabels(I), label.FontSpec.GetFont(1.0)).Width > lBufferWid Then
                lBufferWid = System.Windows.Forms.TextRenderer.MeasureText(XLabels(I), label.FontSpec.GetFont(1.0)).Width
                labelw = label
            End If
            pZgc.GraphPane.GraphObjList.Add(label)
        Next
        If labelw IsNot Nothing Then
            pZgc.GraphPane.Margin.Bottom = System.Windows.Forms.TextRenderer.MeasureText(labelw.Text, labelw.FontSpec.GetFont(1.0)).Width + 15
        End If
    End Sub

    Friend Function SaveToFile() As Boolean
        If Not String.IsNullOrEmpty(pOutputFile) AndAlso IO.Directory.Exists(IO.Path.GetDirectoryName(pOutputFile)) Then
            Try
                pZgc.GraphPane.GetImage().Save(pOutputFile)
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function

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
            If pShowOutliers Then
                'The wiskers must end on an actual data point
                barList.Add(i, ValueNearestButGreater(data(i), lowerLimit), ValueNearestButLess(data(i), upperLimit))
                'Sort out the outliers
                For Each aValue As Double In data(i)
                    If (aValue > upperLimit) Then
                        outs.Add(i, aValue)
                    End If

                    If (aValue < lowerLimit) Then
                        outs.Add(i, aValue)
                    End If
                Next
            Else
                upperLimit = lmax
                lowerLimit = lmin
                'The wiskers must end on an actual data point
                barList.Add(i, lowerLimit, upperLimit)
            End If

            Dim lColor As Color = Color.Black
            If pDataColors IsNot Nothing AndAlso pDataColors.Count >= data.Count Then
                lColor = DataColors(i)
            End If

            'Plot the items, first the median values
            Dim median As CurveItem = myPane.AddCurve("", medians, Color.Black, SymbolType.HDash)
            Dim myLine As LineItem = CType(median, LineItem)
            myLine.Line.IsVisible = False
            myLine.Symbol.Fill.Type = FillType.Solid

            'Wiskers
            Dim myerror As ErrorBarItem = myPane.AddErrorBar("", barList, Color.Black)

            'Box
            Dim myCurve As HiLowBarItem = Nothing
            If pDataColors IsNot Nothing AndAlso pDataColors.Count >= data.Count Then
                myCurve = myPane.AddHiLowBar(names(i), hiLowList, DataColors(i))
                myCurve.Bar.Fill.Type = FillType.Solid
                myCurve.Bar.Border.Color = Color.DarkGray
            Else
                myCurve = myPane.AddHiLowBar(names(i), hiLowList, Color.Black)
                myCurve.Bar.Fill.Type = FillType.None
            End If

            'Outliers
            If pShowOutliers Then
                Dim lOutlierColor As Color = Color.Black
                If pDataColors IsNot Nothing AndAlso pDataColors.Count >= data.Count Then
                    lOutlierColor = DataColors(i)
                End If
                Dim upper As CurveItem = myPane.AddCurve("", outs, lOutlierColor, SymbolType.Circle)
                Dim bLine As LineItem = CType(upper, LineItem)
                bLine.Symbol.Size = 3
                bLine.Symbol.Fill = New ZedGraph.Fill(lOutlierColor)
                bLine.Line.IsVisible = False

                'Dim label As TextObj = New TextObj(names(i), i, 2.0, CoordType.AxisXYScale, AlignH.Center, AlignV.Center)
                'Label.ZOrder = ZOrder.A_InFront
                'Label.FontSpec.Border.IsVisible = False
                'myPane.GraphObjList.Add(label)
            End If
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

    Friend Function GetStatistic(ByVal aDSIndex As Integer, ByVal aStatName As String) As Double
        Dim lStat As Double = Double.NaN
        If Datasets IsNot Nothing AndAlso Datasets.Count > 0 Then
            Dim lTs As atcTimeseries = Datasets(aDSIndex)
            lStat = lTs.Attributes.GetValue(aStatName, Double.NaN)
        ElseIf pDatasetsCalc IsNot Nothing AndAlso pDatasetsCalc.Count > 0 Then
            Dim lTs As atcTimeseries = pDatasetsCalc(aDSIndex)
            If lTs.numValues = 0 OrElse lTs.Values.Length = 1 Then
                lStat = lTs.Value(0)
            Else
                lStat = lTs.Attributes.GetValue(aStatName, Double.NaN)
            End If
        End If
        Return lStat
    End Function

    Friend Function TimeUnitText(ByVal aTser As atcTimeseries) As String
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
