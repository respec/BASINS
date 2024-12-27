Imports System
Imports System.Drawing
Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph
Imports System.Collections
Imports System.Linq

Public Class BoxWhiskerItem

    ''' <summary>
    ''' Location (may be Watershed Name?)
    ''' </summary>
    Public Location As String
    ''' <summary>
    ''' Generally UCI file name
    ''' </summary>
    Public Scenario As String
    ''' <summary>
    ''' Start and end date of the simulation
    ''' </summary>
    Public TimeSpan As String
    ''' <summary>
    ''' Name of the constituent 
    ''' </summary>
    Public Constituent As String
    ''' <summary>
    ''' Unit used in the model
    ''' </summary>
    Public Units As String
    'Public Labels As New List(Of String)
    'Public Values As New List(Of Double)
    ''' <summary>
    ''' A collection where Land Use Name is key and list of values for each land use.
    ''' </summary>
    Public LabelValueCollection As New atcCollection

    'Public SortedList As New List(Of Double)


End Class

Public Module modGraphBoxWhiskers

    Public Sub CreateGraph_BoxAndWhisker(items As BoxWhiskerItem, aOutputFileName As String, Optional aTitle As String = "")

        Dim lZgc As New ZedGraph.ZedGraphControl()
        lZgc.Width = 1024
        lZgc.Height = 768

        Dim lGrapher As New atcGraph.clsGraphBoxWhisker(Nothing, lZgc, True)
        If aTitle.Length > 1 Then
            lGrapher.Title = aTitle
        Else
            lGrapher.Title = "Box-Whisker plot of " & items.Constituent & " loading rate from all land uses in " & items.Scenario & " model." &
            vbCrLf & items.TimeSpan
        End If

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
            .Add(Color.Aqua)
            .Add(Color.Black)
            .Add(Color.BlanchedAlmond)
            .Add(Color.CornflowerBlue)
            .Add(Color.DarkOrange)
            .Add(Color.Gray)
            .Add(Color.HotPink)

        End With
        lGrapher.DataColors = data_colors


        'specify the orientation angle of the x-axis label texts
        '-90 (default) is vertical orientation, 0 is horizontal
        lGrapher.XLabelAngle = -90

        'specify Y axis title
        'if not specified, then default Y axis title will be constructed
        lGrapher.YTitle = items.Constituent & " " & items.Units

        Dim values As New List(Of Double)
        For Each key As String In items.LabelValueCollection.Keys
            For Each value As Double In items.LabelValueCollection.ItemByKey(key)
                values.Add(value)
            Next
        Next


        Dim YMinScale As Double = values.ToArray.Min
        Dim YMaxScale As Double = values.ToArray.Max
        If YMaxScale / YMinScale > 100 Then
            lGrapher.YUseLog = True
            lGrapher.ZedGraphCtrl.GraphPane.YAxis.Scale.Min = Math.Pow(10, Math.Floor(Math.Log10(YMinScale))) / 10
            lGrapher.ZedGraphCtrl.GraphPane.YAxis.Scale.Max = Math.Pow(10, Math.Ceiling(Math.Log10(YMaxScale))) * 10

            'This is not the most elegant way to select better scales for Fecal Coliform or E.Coli Graph,
            'but it is better than what we had. Anurag - 12/26/2024

        End If

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
