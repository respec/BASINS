Imports System
Imports System.Drawing
Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph
Imports System.Collections

Public Class BoxWhiskerItem
    Public Label As String
    Public Values As New List(Of Double)
    Public Max As Double = 0.0
    Public Min As Double = 0.0
    Public Mean As Double = 0.0
    Public Count As Double = Values.Count


    Public SortedList As New List(Of Double)







End Class

Public Module modGraphBoxWhiskers

    Public Sub CreateGraph_BoxAndWhisker(items As Generic.List(Of BoxWhiskerItem))

        Dim lZgc As New ZedGraphControl

        Dim myPane As GraphPane = lZgc.GraphPane

        lZgc.Width = 1024
        lZgc.Height = 768

        myPane.XAxis.Type = AxisType.Ordinal

        Dim xLab As New List(Of String)
        Dim yValues() As Double = {}
        Dim ItemsForEachLabel As Int16 = 0
        For Each item As BoxWhiskerItem In items
            xLab.Add(item.Label)
            For Each yValue As Double In item.Values
                yValues(ItemsForEachLabel) = yValue
                ItemsForEachLabel += 1

            Next

            'Dim myBar As HiLowBarItem = myPane.AddHiLowBar(item.Label, Nothing, yValues, Nothing, Color.Black)





        Next



        Dim list As New PointPairList()
        Dim i As Integer, x As Double, y As Double
        For i = 0 To 19
            x = i
            y = Math.Sin(x * 5 / 8.0)
            list.Add(x, y)
        Next i


        ' Horizontal box and whisker chart 
        ' yval is the vertical position of the box & whisker 
        Dim yval As Double = 0.3
        ' pct5 = 5th percentile value 
        Dim pct5 As Double = 5
        ' pct25 = 25th percentile value 
        Dim pct25 As Double = 40
        ' median = median value 
        Dim median As Double = 55
        ' pct75 = 75th percentile value 
        Dim pct75 As Double = 80
        ' pct95 = 95th percentile value 
        Dim pct95 As Double = 95

        ' Draw the box 
        Dim list2 As New PointPairList()
        list2.Add(pct25, yval, median)
        list2.Add(median, yval, pct75)
        Dim myBar As HiLowBarItem = myPane.AddHiLowBar("box", list2, Color.Black)
        ' set the size of the box (in points, scaled to graph size) 
        'myBar.Bar.Size = 20
        myBar.Bar.Fill.IsVisible = False
        'myPane.Bar.BarBase = BarBase.Y


        ' Draw the whiskers 
        Dim xwhisk() As Double = {pct5, pct25, PointPair.Missing, pct75, pct95}
        Dim ywhisk() As Double = {yval, yval, yval, yval, yval}
        Dim list3 As New PointPairList()
        list3.Add(xwhisk, ywhisk)
        Dim mywhisk As LineItem = myPane.AddCurve("whisker", list3, Color.Black, SymbolType.None)

        lZgc.AxisChange()
        lZgc.SaveIn("C:\Dev\BoxWhiskersTest.png")

        lZgc.Dispose()
    End Sub
End Module
