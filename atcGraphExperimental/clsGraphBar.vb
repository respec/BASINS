Imports System.Drawing

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph

Public Class clsGraphBar
    Inherits clsGraphBoxWhisker

    Public Property AttributeName() As String = "mean"

    <CLSCompliant(False)>
    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZedGraphControl As ZedGraphControl, Optional aDelaySetup As Boolean = False)
        MyBase.New(aDataGroup, aZedGraphControl, True)
        If Not aDelaySetup Then
            SetUpGraph(True)
        End If
    End Sub

    Public Overrides Sub SetUpGraph(Optional OutputToFile As Boolean = False)
        If Datasets Is Nothing OrElse Datasets.Count = 0 Then
            If DatasetsCollection Is Nothing OrElse DatasetsCollection.Count = 0 Then
                Exit Sub
            End If
        End If
        'Set up the chart
        pZgc.GraphPane.BarSettings.Type = BarType.Overlay
        pZgc.GraphPane.XAxis.IsVisible = False
        pZgc.GraphPane.Legend.IsVisible = ShowLegend()
        pZgc.GraphPane.Title.FontSpec.Size = 14
        pZgc.GraphPane.Title.FontSpec.IsBold = False
        If String.Compare(Title(), "no title", True) = 0 Then
        ElseIf String.IsNullOrEmpty(Title()) Then
            If Datasets IsNot Nothing AndAlso Datasets.Count > 0 Then
                Dim lcon As String = Datasets(0).Attributes.GetValue("Constituent", "Value")
                pZgc.GraphPane.Title.Text = lcon & " " & AttributeName
            End If
        Else
            pZgc.GraphPane.Title.Text = Title()
        End If
        Dim listDataArrays As New Generic.List(Of Double())
        If Datasets IsNot Nothing AndAlso Datasets.Count > 0 Then
            For I As Integer = 0 To Datasets.Count - 1
                listDataArrays.Add(Datasets(I).Values)
            Next
        Else
            If DatasetsCollection IsNot Nothing AndAlso DatasetsCollection.Count > 0 Then
                Dim lmin As Double = Double.MaxValue
                For I As Integer = 0 To DatasetsCollection.Count - 1
                    If DatasetsCollection.ItemByIndex(I) > lmin Then
                        lmin = DatasetsCollection.ItemByIndex(I)
                    End If
                Next
                For I As Integer = 0 To DatasetsCollection.Count - 1
                    listDataArrays.Add(New Double() {lmin, DatasetsCollection.ItemByIndex(I)})
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
                        XTitle = lTimeUnit & " " & lCons & " at " & lLoc
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
                        YTitle = lCons & " " & lUnit
                    End With
                End If
            End If
            .Title.Text = YTitle() 'lTimeseriesY.ToString
            .Title.FontSpec.Size = 12
        End With
        'ScaleAxis(Datasets, pZgc.GraphPane.YAxis)
        BarPlot(listDataArrays, XLabels)
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

        If ShowLegend Then
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

    '{
    Private Sub BarPlot(ByVal data As Generic.List(Of Double()), ByVal names As Generic.List(Of String))
        Dim myPane As GraphPane = pZgc.GraphPane
        For i As Integer = 0 To data.Count - 1
            Dim ptList As New PointPairList()
            'Add the values
            Dim lattrValue As Double = GetStatistic(i, AttributeName)
            ptList.Add(i, lattrValue)

            Dim lColor As Color = Color.Black
            If DataColors IsNot Nothing AndAlso DataColors.Count >= data.Count Then
                lColor = DataColors(i)
            End If

            'Box
            Dim myCurve As BarItem = Nothing
            If DataColors IsNot Nothing AndAlso DataColors.Count >= data.Count Then
                myCurve = myPane.AddBar(names(i), ptList, DataColors(i))
                myCurve.Bar.Fill.Type = FillType.Solid
                myCurve.Bar.Border.Color = Color.DarkGray
            Else
                myCurve = myPane.AddBar(names(i), ptList, Color.Black)
                myCurve.Bar.Fill.Type = FillType.None
            End If
        Next
    End Sub '}

End Class
