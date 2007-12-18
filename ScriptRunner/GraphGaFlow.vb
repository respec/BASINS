Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcGraph
Imports HspfSupport
Imports MapWindow.Interfaces
Imports ZedGraph

Module GraphGaFlow
    Private Const pTestPath As String = "D:\Basins\data\03130001\flow"
    Private Const pBaseName As String = "flow"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lWdmDataSource As New atcDataSourceWDM
        lWdmDataSource.Open(lWdmFileName)
        Dim lDataGroup As New atcDataGroup

        Dim lCons As String = "Flow"

        Dim lGraphForm As atcGraph.atcGraphForm
        ChDriveDir(CurDir() & "\outfiles")
        Dim lOutFileName As String

        Dim lSDate(5) As Integer : lSDate(0) = 2000 : lSDate(1) = 1 : lSDate(2) = 1
        Dim lSDateJ As Double = Date2J(lSDate)
        Dim lEDate(5) As Integer : lEDate(0) = 2006 : lEDate(1) = 1 : lEDate(2) = 1
        Dim lEdatej As Double = Date2J(lEDate)

        Dim lTser1 As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(9) 'Chattahoochee at Buford
        lTser1.Attributes.SetValue("YAxis", "Left")
        lDataGroup.Add(SubsetByDate(lTser1, _
                                    lSDateJ, _
                                    lEdatej, Nothing))

        Dim lTser2 As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(15) 'Chattahoochee at Norcross
        lTser2.Attributes.SetValue("YAxis", "Left")
        lDataGroup.Add(SubsetByDate(lTser2, _
                                    lSDateJ, _
                                    lEdatej, Nothing))
        Dim lYMax As Double = 10000 'todo: compute this

        Dim lACoef As Double
        Dim lBCoef As Double
        Dim lRSquare As Double
        lGraphForm = New atcGraph.atcGraphForm(lDataGroup, AxisType.Linear)
        With lGraphForm.Pane
            With .XAxis
                'TODO: figures out how to make whole title go below XAxis title
                .Title.Text = "Buford" & vbCrLf & vbCrLf & _
                              "Scatter Plot" & vbCrLf & _
                              "Chattahoochee Flow (cfs)"
                .Scale.Min = 0
                .Scale.Max = lYMax
                .Scale.IsUseTenPower = False
                .Scale.MaxAuto = False
                .MajorGrid.IsVisible = True
                .MinorGrid.IsVisible = True
            End With
            .YAxis.Scale.Max = .XAxis.Scale.Max
            .YAxis.Scale.Min = .XAxis.Scale.Min
            With .YAxis
                .Title.Text = "Norcross"
                .Scale.IsUseTenPower = False
                .Scale.MaxAuto = False
                .MajorGrid.IsVisible = True
                .MinorGrid.IsVisible = True
            End With

            '45 degree line
            AddLine(lGraphForm.Pane, 1, 0, Drawing.Drawing2D.DashStyle.Dot)
            'regression line 
            FitLine(lDataGroup.ItemByIndex(0), lDataGroup.ItemByIndex(1), lACoef, lBCoef, lRSquare)
            AddLine(lGraphForm.Pane, lACoef, lBCoef)
            SaveFileString("CompareStats.txt", CompareStats(lDataGroup.ItemByIndex(0), lDataGroup.ItemByIndex(1)))

            Dim lText As New TextObj
            Dim lFmt As String = "###,##0.###"
            lText.Text = "Y = " & DoubleToString(lACoef, , lFmt) & " X + " & DoubleToString(lBCoef, , lFmt) & vbLf & _
                         "R Squared = " & DoubleToString(lRSquare, , lFmt)
            'TODO: turn off border
            lText.Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
            .GraphObjList.Add(lText)
            .CurveList(0).Label.IsVisible = False
        End With
        lOutFileName = lCons & "_scat"
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")

        With lGraphForm.Pane
            .YAxis.Type = ZedGraph.AxisType.Log
            .YAxis.Scale.Min = 100
            .XAxis.Type = ZedGraph.AxisType.Log
            .XAxis.Scale.Min = 100
            .CurveList.RemoveAt(2)
            .CurveList.RemoveAt(1)
            AddLine(lGraphForm.Pane, 1, 0, Drawing.Drawing2D.DashStyle.Dot)
            AddLine(lGraphForm.Pane, lACoef, lBCoef)
        End With
        lOutFileName = lCons & "_scat_log"
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")

        lGraphForm.Dispose()

        lGraphForm = New atcGraph.atcGraphForm(lDataGroup, AxisType.Probability)
        SetGraphSpecs(lGraphForm)
        With lGraphForm.Pane
            .YAxis.Title.Text = lCons & " (cfs)"
            .YAxis.Type = ZedGraph.AxisType.Log
            .YAxis.Scale.Min = 100
            .YAxis.Scale.Max = lYMax
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
            .XAxis.Title.Text = "Percent of Time " & lCons & " exceeded"
        End With
        lOutFileName = lCons & "_dur"
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")
        lGraphForm.Dispose()

        lGraphForm = New atcGraph.atcGraphForm(lDataGroup)
        With lGraphForm.Pane
            .YAxis.Scale.Min = 0
            .YAxis.Scale.Max = lYMax
            .YAxis.Title.Text = lCons & " (cfs)"
            .XAxis.Title.Text = "Daily Mean Flow"
            .XAxis.MajorTic.IsOutside = True
        End With
        SetGraphSpecs(lGraphForm)
        lOutFileName = lCons
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")

        With lGraphForm.Pane
            .YAxis.Type = ZedGraph.AxisType.Log
            .YAxis.Scale.Min = 100
            .YAxis.Scale.Max = lYMax
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.IsUseTenPower = False
        End With
        lOutFileName = lCons & "_log "
        lGraphForm.SaveBitmapToFile(lOutFileName & ".png")
        lGraphForm.ZedGraphCtrl.SaveIn(lOutFileName & ".emf")
        lGraphForm.Dispose()

        OpenFile(lOutFileName & ".png")

    End Sub
End Module
