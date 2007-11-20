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

            Dim lGraphForm As New atcGraph.atcGraphForm(lDataGroup)
            'lGraphForm.Size = lGraphForm.MaximumSize
            lGraphForm.WindowState = Windows.Forms.FormWindowState.Maximized
            With lGraphForm.Pane
                .XAxis.Title.Text = "Daily Mean Flow at " & lSite
                .XAxis.MajorGrid.IsVisible = True
                .YAxis.Title.Text = lCons & " (cfs)"
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
                Dim lOutFileName As String = "outfiles\" & lCons & "_" & lSite & ".bmp"
                lGraphForm.SaveBitmapToFile(lOutFileName)
                .YAxis.Type = ZedGraph.AxisType.Log
                .YAxis.Scale.Min = 1
                lOutFileName = "outfiles\" & lCons & "_" & lSite & "_log.bmp"
                lGraphForm.SaveBitmapToFile(lOutFileName)
            End With
        Next lSiteIndex
    End Sub
End Module
