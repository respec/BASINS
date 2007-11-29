Imports atcUtility
Imports atcData
Imports atcWDM
Imports HspfSupport
Imports MapWindow.Interfaces
Imports ZedGraph

Module Graph
    Private Const pTestPath As String = "C:\test\EXP_CAL\hyd_man.net"
    Private Const pBaseName As String = "hyd_man"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")
        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lWdmDataSource As New atcDataSourceWDM
        lWdmDataSource.Open(lWdmFileName)
        'open expert system
        Dim lExpertSystem As HspfSupport.ExpertSystem
        lExpertSystem = New HspfSupport.ExpertSystem(lHspfUci, lWdmDataSource)
        Dim lCons As String = "Flow"
        Dim lGraphForm As atcGraph.atcGraphForm
        Dim lOutFileName As String

        For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
            Dim lSite As String = lExpertSystem.Sites(lSiteIndex).Name
            Dim lArea As Double = lExpertSystem.Sites(lSiteIndex).Area
            Dim lDataGroup As New atcDataGroup
            Dim lSimTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(0))
            lSimTser = InchesToCfs(lSimTser, lArea)
            lDataGroup.Add(SubsetByDate(lSimTser, _
                                        lExpertSystem.SDateJ, _
                                        lExpertSystem.EDateJ, Nothing))
            Dim lObsTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(1))
            lDataGroup.Add(SubsetByDate(lObsTSer, _
                                        lExpertSystem.SDateJ, _
                                        lExpertSystem.EDateJ, Nothing))

            lGraphForm = New atcGraph.atcGraphForm(lDataGroup)
            lGraphForm.Pane.YAxis.Title.Text = lCons & " (cfs)"
            lGraphForm.Pane.XAxis.Title.Text = "Daily Mean Flow at " & lSite
            SetGraphSpecs(lGraphForm)
            lOutFileName = "outfiles\" & lCons & "_" & lSite & ".png"
            lGraphForm.SaveBitmapToFile(lOutFileName)
            With lGraphForm.Pane
                .YAxis.Type = ZedGraph.AxisType.Log
                .YAxis.Scale.Min = 1
                .YAxis.Scale.IsUseTenPower = False
            End With
            lOutFileName = "outfiles\" & lCons & "_" & lSite & "_log.png"
            lGraphForm.SaveBitmapToFile(lOutFileName)
            lGraphForm.Dispose()

            lGraphForm = New atcGraph.atcGraphForm(lDataGroup, AxisType.Linear)
            With lGraphForm.Pane
                .XAxis.Title.Text = "Observed"
                .XAxis.Type = ZedGraph.AxisType.Log
                .XAxis.Scale.Min = 1
                .XAxis.Scale.IsUseTenPower = False
                .YAxis.Title.Text = "Simulated"
                .YAxis.Type = ZedGraph.AxisType.Log
                .YAxis.Scale.Min = 1
                .YAxis.Scale.IsUseTenPower = False
                .YAxis.Scale.Max = .XAxis.Scale.Max
                .YAxis.Scale.MaxAuto = False
            End With
            lOutFileName = "outfiles\" & lCons & "_" & lSite & "_scat.png"
            lGraphForm.SaveBitmapToFile(lOutFileName)
            lGraphForm.Dispose()

            lGraphForm = New atcGraph.atcGraphForm(lDataGroup, AxisType.Probability)
            SetGraphSpecs(lGraphForm)
            With lGraphForm.Pane
                .YAxis.Title.Text = lCons & " (cfs)"
                .YAxis.Type = ZedGraph.AxisType.Log
                .YAxis.Scale.Min = 1
                .YAxis.Scale.IsUseTenPower = False
                .XAxis.Title.Text = "Percent of Time " & lCons & " exceeded at " & lSite
            End With
            lOutFileName = "outfiles\" & lCons & "_" & lSite & "_dur.png"
            lGraphForm.SaveBitmapToFile(lOutFileName)
            lGraphForm.Dispose()
            OpenFile(lOutFileName)
        Next lSiteIndex
    End Sub

    Private Sub SetGraphSpecs(ByRef lGraphForm As atcGraph.atcGraphForm)
        'lGraphForm.Size = lGraphForm.MaximumSize
        lGraphForm.WindowState = Windows.Forms.FormWindowState.Maximized
        With lGraphForm.Pane
            '.XAxis.MajorGrid.IsVisible = True
            .YAxis.Scale.Min = 0
            .YAxis.MajorGrid.IsVisible = True
            .YAxis.MinorGrid.IsVisible = True
            With .CurveList(0)
                .Label.Text = "Simulated"
                .Color = System.Drawing.Color.Red
            End With
            With .CurveList(1)
                .Label.Text = "Observed"
                .Color = System.Drawing.Color.Blue
            End With
            .Legend.Position = LegendPos.Float
            .Legend.Location = New Location(0.3, 0.05, CoordType.PaneFraction, AlignH.Right, AlignV.Top)
            .Legend.FontSpec.Size = 10
            .Legend.IsHStack = False
        End With
    End Sub
End Module
