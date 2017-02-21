'API Demonstration for calling the new BoxWhisker plot directly
'The code will generate a bmp file for your plot

'create new zedgraph control
Dim lZedgraph As New ZedGraph.ZedGraphControl()

'set its size
lZedgraph.Width = 640
lZedgraph.Height = 480

'generate timeseries group to hold the data
Dim lCol as New atcTimeseriesGroup()
Dim lTs1 as atcTimeseries = lSomeTser1
Dim lTs2 as atcTimeseries = lSomeTser2
lCol.Add(lTs1)
lCol.Add(lTs2)

'instantiate the graphing engine
Dim lGrapher As New atcGraph.clsGraphBoxWhisker(lCol, lZedgraph, True)

'give a full path name for your graph file, use .bmp format
lGrapher.OutputFile = "C:\temp\test\boxwhisker.bmp" 

'give a title for the plot
lGrapher.Title = "Sediment Loading Rate Range"

'Set the color of the data series
'if no color are supplied, then all boxes are black in color
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
lGrapher.DataColors = data_colors

'construct x-axis box category labels
'be sure to have exactly the SAME number of label entries as the number of
'datasets in the timeseries group and in the SAME order
'if these labels are not provided, then the datasets' locations are used as 
'labels instead
Dim locations As New ArrayList()
With locations
    .Add("ForestAB")
    .Add("ForestCD")
    .Add("Herb Wet")
    .Add("Wood Wet")
    .Add("Grass AB")
    .Add("Grass CD")
    .Add("Past AB ")
    .Add("Past CD ")
    .Add("Crop AB ")
    .Add("Crop CD ")
    .Add("Crop Dr ")
    .Add("Dev Open")
    .Add("Dev Low ")
    .Add("Dev Med ")
    .Add("IDev Open")
    .Add("IDev Low")
    .Add("IDev Med")
End With

'specify the orientation angle of the x-axis label texts
'-90 (default) is vertical orientation, 0 is horizontal
lGrapher.XLabelAngle = -90

'specify Y axis title
'if not specified, then default Y axis title will be constructed
lGrapher.YTitle = "My kinda quantities"

'specify if to use legend
'True: use legend to show box category instead of x-axis labels
'False: use x-axis labels but not legend
lGrapher.ShowLegend = True

'call routine to make the graph
'input argument: 
'   True, write the graph to output file previously specified
'   False, don't write to output file              
lGrapher.SetUpGraph(True)

'Final note: all of the above clsGraphBoxWhisker attributes are optional
'if none is specified, then the default values will be used
'Example plots are:
'  boxwhisker_labels.bmp --> lGrapher.ShowLegend = False
'  boxwhisker_legend.bmp --> lGrapher.ShowLegend = True
