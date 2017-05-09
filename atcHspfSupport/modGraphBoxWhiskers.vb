Imports System
Imports System.Drawing
Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph
Imports System.Collections

Public Class BoxWhiskerItem

    Public Location As String
    Public Scenario As String
    Public TimeSpan As String
    Public Constituent As String
    Public Units As String
    'Public Labels As New List(Of String)
    'Public Values As New List(Of Double)
    Public LabelValueCollection As New atcCollection

    'Public SortedList As New List(Of Double)


End Class

Public Module modGraphBoxWhiskers

    Public Sub CreateGraph_BoxAndWhisker(items As BoxWhiskerItem, aOutputFileName As String)

        Dim lZgc As New ZedGraph.ZedGraphControl()
        lZgc.Width = 1024
        lZgc.Height = 768

        Dim lGrapher As New atcGraph.clsGraphBoxWhisker(Nothing, lZgc, True)
        Select Case items.Constituent
            Case "Sediment"
                lGrapher.Title = "Box-Whisker plot of sediment loading rate from all land uses in " & items.Scenario & " model." &
                    vbCrLf & items.TimeSpan
            Case "TotalP"
                lGrapher.Title = "Box-Whisker plot of total phoshphorus loading rate from all land uses in " & items.Scenario & " model." &
                    vbCrLf & items.TimeSpan
            Case "TotalN"
                lGrapher.Title = "Box-Whisker plot of total nitrogen loading rate from all land uses in " & items.Scenario & " model." &
                    vbCrLf & items.TimeSpan
            Case Else
                Exit Sub
        End Select
        lGrapher.DatasetsCollection = items.LabelValueCollection
        lGrapher.ShowOutliers = False


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
            .Add(Color.DarkOrchid)
            .Add(Color.Chartreuse)
            .Add(Color.Crimson)
            .Add(Color.Cyan)
            .Add(Color.DarkGray)
            .Add(Color.DarkMagenta)
            .Add(Color.DarkSalmon)
            .Add(Color.DarkTurquoise)
            .Add(Color.DeepPink)
            .Add(Color.DodgerBlue)
            .Add(Color.Gold)
            .Add(Color.Indigo)
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
