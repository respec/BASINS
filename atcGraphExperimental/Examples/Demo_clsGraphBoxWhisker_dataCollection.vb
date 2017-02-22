'API Demonstration for calling the new BoxWhisker plot directly
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
Dim locations As New ArrayList()
With locations
    .Add("ForestAB")
    .Add("IDev Med")
End With
Dim lValues_Forest As Double() = {1.637,2.119,3.93,0.932,5.006,0.479,3.529,2.542,2.344,2.272,0.314,1.966,1.295,1.755,0.498,0.154,0.608}
Dim lValues_IDev As Double() = {2.417,3.378,3.997,1.464,6.383,0.76,4.07,2.902,2.687,3.688,0.4,3.061,1.929,2.629,0.765,0.364,0.745}

'generate dataset group to hold the data
Dim lCol as New atcCollection()
lCol.Add(locations(0), lValues_Forest)
lCol.Add(locations(1), lValues_IDev)

'instantiate the graphing engine
Dim lGrapher As New atcGraph.clsGraphBoxWhisker(Nothing, lZedgraph, True)
lGrapher.DatasetsCollection = lCol

'give a full path name for your graph file
lGrapher.OutputFile = "C:\temp\test\boxwhisker.png" 

'give a title for the plot
lGrapher.Title = "Sediment Loading Rate Range"

'Set the color of the data series
'if no color are supplied, then all boxes are black in color
Dim data_colors As New List(Of Color)
With data_colors
    .Add(Color.Red)
    .Add(Color.Aqua)
End With
lGrapher.DataColors = data_colors


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
'  boxwhisker_collection_labels.png --> lGrapher.ShowLegend = False
'  boxwhisker_collection_legend.png --> lGrapher.ShowLegend = True
