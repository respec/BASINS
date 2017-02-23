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
        lGrapher.DatasetsCollection = items.LabelValueCollection
        'Dim locations As New ArrayList()
        ''locations = items.LabelValueCollection.Keys
        'With locations
        '    .Add("ForestAB")
        '    .Add("IDev Med")
        'End With
        'Dim lValues_Forest As Double() = {1.637, 2.119, 3.93, 0.932, 5.006, 0.479, 3.529, 2.542, 2.344, 2.272, 0.314, 1.966, 1.295, 1.755, 0.498, 0.154, 0.608}
        'Dim lValues_IDev As Double() = {2.417, 3.378, 3.997, 1.464, 6.383, 0.76, 4.07, 2.902, 2.687, 3.688, 0.4, 3.061, 1.929, 2.629, 0.765, 0.364, 0.745}

        ''generate dataset group to hold the data
        'Dim lCol As New atcCollection()
        'For Each key As String In items.LabelValueCollection.Keys
        '    lCol.Add(key, items.LabelValueCollection.ItemByKey(key))
        'Next
        ''lCol.Add(locations(0), lValues_Forest)
        ''lCol.Add(locations(1), lValues_IDev)

        'lGrapher.DatasetsCollection = lCol


        'lGrapher.DatasetsCollection = items.LabelValueCollection
        lGrapher.Title = "Title"
        lGrapher.OutputFile = aOutputFileName

        'Set the color of the data series
        'if no color are supplied, then all boxes are black in color
        Dim data_colors As New List(Of Color)
        With data_colors
            .Add(Color.AliceBlue)
            .Add(Color.Aqua)
            .Add(Color.Aquamarine)
            .Add(Color.Azure)
            .Add(Color.Beige)
            .Add(Color.Bisque)
            .Add(Color.BlanchedAlmond)
            .Add(Color.Brown)
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
