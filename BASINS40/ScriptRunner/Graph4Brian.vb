Imports System
Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph

Imports MapWindow.Interfaces
Imports System.Collections.Specialized

Module Graph4Brian
    Private pBaseDrive As String = "C:"
    Private pBaseFolders As New ArrayList
    Private pTestPath As String
    Private pBaseName As String
    Private pOutputLocations As New atcCollection
    Private pGraphSaveFormat As String
    Private pGraphSaveWidth As Integer
    Private pGraphSaveHeight As Integer
    Private pGraphAnnual As Boolean = False
    Private pCurveStepType As String = "RearwardStep"
    Private pSummaryTypes As New atcCollection
    Private pPerlndSegmentStarts() As Integer
    Private pImplndSegmentStarts() As Integer
    Private pGraphWQOnly As Boolean = True
    Private pModels As New ArrayList
    Private pIntensities As New ArrayList
    Private pErrLog As String = "C:\Temp\graph4brianlog.txt"

    Private Sub Initialize()
        pGraphSaveFormat = ".png"
        pGraphSaveWidth = 1024
        pGraphSaveHeight = 768
        'Add scenario directories
        pBaseFolders.Clear()
        pBaseFolders.Add(pBaseDrive & "\Temp")
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        Dim lOutputWdmDataSource As New atcDataSourceWDM()
        Dim lOutputWDMNames As New System.Collections.Specialized.NameValueCollection
        Dim lDatagroup As New atcDataGroup

        For Each lBaseFolder As String In pBaseFolders

            AddFilesInDir(lOutputWDMNames, lBaseFolder, False, "*.wdm")
            Dim lFlow1 As atcTimeseries
            Dim lFlow2 As atcTimeseries
            lDatagroup.Clear()

            For Each lOutputWDMName As String In lOutputWDMNames
                lOutputWdmDataSource.Open(lOutputWDMName)
                lFlow1 = lOutputWdmDataSource.DataSets.ItemByKey(180)
                lFlow2 = lOutputWdmDataSource.DataSets.ItemByKey(181)

                For i As Integer = 0 To lFlow1.numValues - 1
                    If Double.IsInfinity(lFlow1.Value(i)) Then lFlow1.Value(i) = Double.NaN
                    If Double.IsInfinity(lFlow2.Value(i)) Then lFlow2.Value(i) = Double.NaN
                Next

                lFlow1.Attributes.SetValue("Scenario", "")
                lFlow2.Attributes.SetValue("Scenario", "")
                lFlow1.Attributes.SetValue("Constituent", "BASE CONDITIONS")
                lFlow2.Attributes.SetValue("Constituent", "NATURAL CONDITIONS")

                lDatagroup.Add(lFlow1)
                lDatagroup.Add(lFlow2)
                lFlow1.Clear()
                lFlow2.Clear()

            Next

            'Do the duration graphs
            Dim lprefix As String = "zBrianDur_"
            Dim lscen As String = lprefix & "Flow"
            Dim lGraphFilename As String = IO.Path.Combine(lBaseFolder, lscen) & pGraphSaveFormat
            Dim lPlotTitle As String = "Percent chance FLOW exceeded " & vbCrLf & "FLOW at RCH180"
            Dim lYTitle As String = "FLOW (cfs)"
            doDurPlot(lDatagroup, lGraphFilename, lPlotTitle, lYTitle)

            lOutputWDMNames.Clear()
        Next

    End Sub
    Private Function doDurPlot(ByRef aDatagroup As atcDataGroup, ByVal aGraphFilename As String, ByVal aPlotTitle As String, ByVal aYTitle As String) As Boolean
        Dim doneIt As Boolean = True
        Dim lp As String = ""

        Dim lZgc As ZedGraphControl
        lZgc = CreateZgc()
        Dim lGraphSaveWidth As Integer = 1000
        Dim lGraphSaveHeight As Integer = 600
        lZgc.Width = lGraphSaveWidth
        lZgc.Height = lGraphSaveHeight

        Dim lGraphDur As New clsGraphProbability(aDatagroup, lZgc)

        With lGraphDur.ZedGraphCtrl.GraphPane
            .XAxis.Title.Text = aPlotTitle
            With .XAxis
                .Scale.MaxAuto = False
                .Scale.MinAuto = False
                .MinorGrid.IsVisible = False
                .MajorGrid.IsVisible = False
                .Scale.Min = 0.001
            End With
            .YAxis.Title.Text = aYTitle
            '.YAxis.Type = AxisType.Linear
            .YAxis.MinorGrid.IsVisible = False
            .YAxis.MajorGrid.IsVisible = False

            If .YAxis.Scale.Min < 1 Then
                .YAxis.Scale.MinAuto = False
                .YAxis.Scale.Min = 1
                '.YAxis.Scale.Max = 1000000
                .AxisChange()
            End If

            '.Legend.Position = LegendPos.TopFlushLeft
            '.IsPenWidthScaled = True
            '.LineType = LineType.Stack
            '.ScaledPenWidth(50, 2)
            .CurveList.Item(0).Color = Drawing.ColorTranslator.FromHtml("#FF0000") 'Base condition: red
            .CurveList.Item(1).Color = Drawing.ColorTranslator.FromHtml("#00FF00") 'Natural condition: green

            For Each li As LineItem In .CurveList
                li.Line.Width = 2
                Dim lFS As New FontSpec
                lFS.FontColor = li.Line.Color
                li.Label.FontSpec = lFS
                li.Label.FontSpec.Border.IsVisible = False
            Next
        End With

        Try
            lZgc.SaveIn(aGraphFilename)
        Catch ex As Exception
            lp = aGraphFilename
            doneIt = False
            'Stop
        End Try
        lGraphDur.Dispose()
        lZgc.Dispose()

        If lp <> "" Then
            IO.File.AppendAllText(pErrLog, lp & vbCrLf)
        End If

        Return doneIt
    End Function
End Module
