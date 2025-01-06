Imports System
Imports System.Drawing
Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph
Imports System.Collections

Public Class BarGraphItem

    Public Location As String
    Public Scenario As String
    Public TimeSpan As String
    Public Constituent As String
    Public Units As String
    Public LabelValueCollection As New atcCollection
    'One value for each label
End Class

Public Module modBarGraph

    Public Sub CreateGraph_BarGraph(items As BarGraphItem, aOutputFileName As String)

        Dim lZgc As New ZedGraph.ZedGraphControl()
        lZgc.Width = 1024
        lZgc.Height = 768

        Dim lGrapher As New atcGraph.clsGraphBar(Nothing, lZgc, True)
        lGrapher.ShowOutliers = False
        lGrapher.DatasetsCollection = items.LabelValueCollection

        Select Case items.Constituent
            Case "Sediment"
                lGrapher.Title = "Sediment load allocation for all the sources at " & vbCrLf & items.Location & " in " &
                items.Scenario & " model. " & vbCrLf & items.TimeSpan
            Case "TP"
                lGrapher.Title = "Total Phosphorus load allocation for all the sources at " & vbCrLf & items.Location & " in " &
                items.Scenario & " model. " & vbCrLf & items.TimeSpan
            Case "TN"
                lGrapher.Title = "Total Nitrogen load allocation for all the sources at " & vbCrLf & items.Location & " in " &
                items.Scenario & " model. " & vbCrLf & items.TimeSpan
            Case "NO2NO3"
                lGrapher.Title = "Nitrite and Nitrate as Nitrogen load allocation for all the sources at " & vbCrLf & items.Location & " in " &
                items.Scenario & " model. " & vbCrLf & items.TimeSpan
            Case "TAM"
                lGrapher.Title = "Total Ammonia as Nitrogen load allocation for all the sources at " & vbCrLf & items.Location & " in " &
                items.Scenario & " model. " & vbCrLf & items.TimeSpan
            Case "ORTHO P"
                lGrapher.Title = "Ortho-Phosphorus as Phosphorus load allocation for all the sources at " & vbCrLf & items.Location & " in " &
                items.Scenario & " model. " & vbCrLf & items.TimeSpan
            Case "TKN"
                lGrapher.Title = "TKN as Nitrogen load allocation for all the sources at " & vbCrLf & items.Location & " in " &
                items.Scenario & " model. " & vbCrLf & items.TimeSpan
        End Select

        lGrapher.OutputFile = aOutputFileName

        'Set the color of the data series
        'if no color are supplied, then all boxes are black in color
        Dim data_colors As New List(Of Color)
        With data_colors
            .Add(Color.Blue)
            .Add(Color.Red)
            .Add(Color.Green)
            .Add(Color.Yellow)
            .Add(Color.HotPink)
            .Add(Color.DarkMagenta)
            .Add(Color.DarkOliveGreen)
            .Add(Color.DarkSalmon)
            .Add(Color.Black)
            .Add(Color.Indigo)
            .Add(Color.Cyan)
            .Add(Color.DarkGray)
            .Add(Color.DarkMagenta)
            .Add(Color.DarkGoldenrod)
            .Add(Color.DarkTurquoise)
            .Add(Color.DeepPink)
            .Add(Color.DodgerBlue)
            .Add(Color.Gold)
            .Add(Color.Indigo)
            .Add(Color.Crimson)
            .Add(Color.CornflowerBlue)
            .Add(Color.DarkKhaki)
            .Add(Color.ForestGreen)
            .Add(Color.LemonChiffon)
        End With
        lGrapher.DataColors = data_colors


        'specify the orientation angle of the x-axis label texts
        '-90 (default) is vertical orientation, 0 is horizontal
        lGrapher.XLabelAngle = -90

        'specify Y axis title
        'if not specified, then default Y axis title will be constructed
        lGrapher.YTitle = items.Constituent & " " & items.Units

        'specify if to use legend
        'True: use legend to show box category instead of x-axis labels
        'False: use x-axis labels but not legend
        lGrapher.ShowLegend = False

        'call routine to make the graph
        'input argument: 
        '   True, write the graph to output file previously specified
        '   False, don't write to output file              
        lGrapher.SetUpGraph(True)

        'Final note: all of the above clsGraphBoxWhisker attributes are optional
        'if none is specified, then the default values will be used
        'Example plots are:
        '  boxwhisker_collection_labels.png --> lGrapher.ShowLegend = False
        '  boxwhisker_collection_legend.png --> lGrapher.ShowLegend = True




        lZgc.Dispose()
    End Sub
End Module
