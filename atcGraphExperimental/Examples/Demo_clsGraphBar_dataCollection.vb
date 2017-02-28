'API Demonstration for calling the new Bar plot directly
'The code will generate a bmp file for your plot

'create new zedgraph control
Dim lZedgraph As New ZedGraph.ZedGraphControl()

'set its size
lZedgraph.Width = 640
lZedgraph.Height = 480

'construct x-axis box category labels
'be sure to have exactly the SAME number of label entries as the number of
'datasets in the timeseries group and in the SAME order
'if these labels are not provided, then the datasets' locations are used as 
'labels instead
Dim lBarData As New atcCollection()
With lBarData
    .Add("P:Forest - AB", 0.146)
    .Add("P:Forest - CD", 0.219)
    .Add("P:Emerg Herb Wetland", 0)
    .Add("P:Woody Wetlands", 0.001)
    .Add("P:Grassland - AB", 1.04)
    .Add("P:Grassland - CD", 1.625)
    .Add("P:Pasture - AB", 1.026)
    .Add("P:Pasture - CD", 1.606)
    .Add("P:Cropland - AB", 1.437)
    .Add("P:Cropland - CD", 2.692)
    .Add("P:Cropland-Drained", 0.312)
    .Add("P:Dev, Open Space", 1.846)
    .Add("P:Dev, Low Intensity", 2.205)
    .Add("P:Dev, Medium Intensit", 2.216)
    .Add("I:Dev, Open Space", 0.985)
    .Add("I:Dev, Low Intensity", 1.833)
    .Add("I:Dev, Medium Intensit", 2.449)
End With
Dim data_colors As New List(Of Color)
With data_colors
    .Add(Color.Red)
    .Add(Color.Aqua)
    .Add(Color.Bisque)
    .Add(Color.Blue)
    .Add(Color.Crimson)
    .Add(Color.DarkGoldenrod)
    .Add(Color.DarkGreen)
    .Add(Color.DarkMagenta)
    .Add(Color.DarkOrchid)
    .Add(Color.DeepPink)
    .Add(Color.DeepSkyBlue)
    .Add(Color.DarkSlateBlue)
    .Add(Color.DarkSalmon)
    .Add(Color.DarkOliveGreen)
    .Add(Color.Yellow)
    .Add(Color.YellowGreen)
    .Add(Color.Turquoise)
End With

'instantiate the graphing engine
Dim lGrapher As New atcGraph.clsGraphBar(Nothing, lZedgraph, True)
lGrapher.DatasetsCollection = lBarData

'give a full path name for your graph file
lGrapher.OutputFile = "C:\temp\test\bar_label.png"

'give a title for the plot
lGrapher.Title = "Mean Sediment Loading Rate"

'Set the color of the data series
'if no color are supplied, then all boxes are black in color
lGrapher.DataColors = data_colors


'specify the orientation angle of the x-axis label texts
'-90 (default) is vertical orientation, 0 is horizontal
lGrapher.XLabelAngle = -90

'specify Y axis title
'if not specified, then default Y axis title will be constructed
lGrapher.YTitle = "Sediment loading tons/ac"

'specify if to use legend
'True: use legend to show box category instead of x-axis labels
'False: use x-axis labels but not legend
lGrapher.ShowLegend = False

'call routine to make the graph
'input argument: 
'   True, write the graph to output file previously specified
'   False, don't write to output file
lGrapher.SetUpGraph(True)
