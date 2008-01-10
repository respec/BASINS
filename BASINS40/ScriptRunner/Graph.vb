Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcGraph
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
        'Dim lGraphForm As atcGraph.atcGraphForm
        Dim lZgc As ZedGraphControl
        Dim lPane As ZedGraph.GraphPane
        Dim lGrapher As clsGraphBase
        ChDriveDir(CurDir() & "\outfiles")
        Dim lOutFileName As String

        For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
            Dim lSite As String = lExpertSystem.Sites(lSiteIndex).Name
            Dim lArea As Double = lExpertSystem.Sites(lSiteIndex).Area
            Dim lDataGroup As New atcDataGroup
            Dim lSimTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(0))
            lSimTser = InchesToCfs(lSimTser, lArea)
            lSimTser.Attributes.SetValue("YAxis", "Left")
            lDataGroup.Add(SubsetByDate(lSimTser, _
                                        lExpertSystem.SDateJ, _
                                        lExpertSystem.EDateJ, Nothing))

            Dim lObsTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(1))
            lObsTser.Attributes.SetValue("YAxis", "Left")
            lDataGroup.Add(SubsetByDate(lObsTser, _
                                        lExpertSystem.SDateJ, _
                                        lExpertSystem.EDateJ, Nothing))
            Dim lYMax As Double = 2500 'todo: compute this

            Dim lACoef As Double
            Dim lBCoef As Double
            Dim lRSquare As Double
            lZgc = CreateZgc()
            'lZgc.Height = 496
            'lZgc.Width = 543
            lGrapher = New clsGraphScatter(lDataGroup, lZgc)
            lPane = lZgc.MasterPane.PaneList(0)
            With lPane
                With .XAxis
                    'TODO: figures out how to make whole title go below XAxis title
                    .Title.Text = "Observed" & vbCrLf & vbCrLf & _
                                  "Scatter Plot" & vbCrLf & _
                                  "Flow at Upper Marlboro (cfs)"
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
                    .Title.Text = "Simulated"
                    .Scale.IsUseTenPower = False
                    .Scale.MaxAuto = False
                    .MajorGrid.IsVisible = True
                    .MinorGrid.IsVisible = True
                End With

                '45 degree line
                AddLine(lPane, 1, 0, Drawing.Drawing2D.DashStyle.Dot, "45DegLine")
                'regression line 
                'TODO: figure out why this seems backwards!
                FitLine(lDataGroup.ItemByIndex(1), lDataGroup.ItemByIndex(0), lACoef, lBCoef, lRSquare)
                AddLine(lPane, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid, "RegLine")

                Dim lText As New TextObj
                Dim lFmt As String = "###,##0.###"
                lText.Text = "Y = " & DoubleToString(lACoef, , lFmt) & " X + " & DoubleToString(lBCoef, , lFmt) & vbLf & _
                             "R Squared = " & DoubleToString(lRSquare, , lFmt)
                'TODO: turn off border
                lText.Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
                .GraphObjList.Add(lText)
                .CurveList(0).Label.IsVisible = False
            End With
            lOutFileName = lCons & "_" & lSite & "_scat"
            lZgc.SaveIn(lOutFileName & ".png")
            lZgc.SaveIn(lOutFileName & ".emf")

            With lPane
                .YAxis.Type = ZedGraph.AxisType.Log
                .YAxis.Scale.Min = 1
                .XAxis.Type = ZedGraph.AxisType.Log
                .XAxis.Scale.Min = 1
                .CurveList.RemoveAt(2)
                .CurveList.RemoveAt(1)
                AddLine(lPane, 1, 0, Drawing.Drawing2D.DashStyle.Dot, "New45DegLine")
                AddLine(lPane, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid, "NewRegLine")
            End With
            lOutFileName = lCons & "_" & lSite & "_scat_log"
            lZgc.SaveIn(lOutFileName & ".png")
            lZgc.SaveIn(lOutFileName & ".emf")
            lGrapher.Dispose()

            lZgc = CreateZgc(lZgc)
            lGrapher = New clsGraphProbability(lDataGroup, lZgc)
            lPane = lZgc.MasterPane.PaneList(0)
            SetGraphSpecs(lZgc)
            With lPane
                .YAxis.Title.Text = lCons & " (cfs)"
                .YAxis.Type = ZedGraph.AxisType.Log
                .YAxis.Scale.Min = 1
                .YAxis.Scale.Max = lYMax
                .YAxis.Scale.MaxAuto = False
                .YAxis.Scale.IsUseTenPower = False
                .XAxis.Title.Text = "Percent of Time " & lCons & " exceeded at " & lSite
            End With
            lOutFileName = lCons & "_" & lSite & "_dur"
            lZgc.SaveIn(lOutFileName & ".png")
            lZgc.SaveIn(lOutFileName & ".emf")
            lGrapher.Dispose()

            'add precip to aux axis
            Dim lPrecTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(5))
            lPrecTser.Attributes.SetValue("YAxis", "Aux")
            lDataGroup.Add(SubsetByDate(lPrecTser, _
                                        lExpertSystem.SDateJ, _
                                        lExpertSystem.EDateJ, Nothing))
            lZgc = CreateZgc(lZgc)
            lGrapher = New clsGraphTime(lDataGroup, lZgc)
            With lZgc.MasterPane.PaneList(1)
                .YAxis.Scale.Min = 0
                .YAxis.Scale.Max = lYMax
                .YAxis.Title.Text = lCons & " (cfs)"
                .XAxis.Title.Text = "Daily Mean Flow at " & lSite
                .XAxis.MajorTic.IsOutside = True
            End With
            With lZgc.MasterPane.PaneList(0)
                .CurveList.Item(0).Color = Drawing.Color.Blue
                .CurveList.Item(0).Label.Text = "Upper Marlboro"
                .YAxis.Title.Text = "Precip (in)"
            End With
            SetGraphSpecs(lZgc)
            lOutFileName = lCons & "_" & lSite
            lZgc.SaveIn(lOutFileName & ".png")
            lZgc.SaveIn(lOutFileName & ".emf")

            With lZgc.MasterPane.PaneList(1)
                .YAxis.Type = ZedGraph.AxisType.Log
                .YAxis.Scale.Min = 1
                .YAxis.Scale.Max = lYMax
                .YAxis.Scale.MaxAuto = False
                .YAxis.Scale.IsUseTenPower = False
            End With
            lOutFileName = lCons & "_" & lSite & "_log "
            lZgc.SaveIn(lOutFileName & ".png")
            lZgc.SaveIn(lOutFileName & ".emf")
            lZgc.Dispose()
            lZgc = Nothing
            lGrapher.Dispose()

            OpenFile(lOutFileName & ".png")
        Next lSiteIndex

        GraphFtables(lHspfUci)
    End Sub

    Sub GraphFtables(ByVal aUCI As atcUCI.HspfUci)
        For Each lOperation As atcUCI.HspfOperation In aUCI.OpnBlks("RCHRES").Ids
            Dim lNRows As Integer = lOperation.FTable.Nrows
            If lNRows > 0 Then
                Dim lZgc As New ZedGraphControl
                With lZgc
                    '.Dock = DockStyle.Fill
                    .Height = 500
                    .Width = 500
                    .Visible = True
                    .IsSynchronizeXAxes = True
                    '.IsEnableHZoom = mnuViewHorizontalZoom.Checked
                    '.IsEnableVZoom = mnuViewVerticalZoom.Checked
                    '.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
                End With
                With lZgc.MasterPane
                    .Border.IsVisible = False
                    .Legend.IsVisible = False
                    .Margin.All = 10
                    .InnerPaneGap = 5
                    .IsCommonScaleFactor = True
                End With

                Dim lPaneMain As GraphPane = lZgc.MasterPane.PaneList(0)
                FormatPaneWithDefaults(lPaneMain)
                lPaneMain.Title.Text = "FTable for Reach " & lOperation.Id
                lPaneMain.XAxis.Title.Text = "Depth (ft)"
                lPaneMain.XAxis.MajorGrid.IsVisible = True
                lPaneMain.YAxis.Title.Text = "Outflow (cfs)"

                Dim lX(lNRows) As Double
                Dim lY(lNRows) As Double
                For lExit As Integer = 1 To lOperation.FTable.Ncols - 3
                    For lRow As Integer = 1 To lNRows
                        With lOperation.FTable
                            lX(lRow) = .Depth(lRow)
                            Select Case lExit
                                Case 1 : lY(lRow) = .Outflow1(lRow)
                                Case 2 : lY(lRow) = .Outflow2(lRow)
                                Case 3 : lY(lRow) = .Outflow3(lRow)
                                Case 4 : lY(lRow) = .Outflow4(lRow)
                                Case 5 : lY(lRow) = .Outflow5(lRow)
                            End Select
                        End With
                    Next
                    Dim lCurve As ZedGraph.CurveItem = lPaneMain.AddCurve("Exit " & lExit, _
                                                                          lX, lY, _
                                                                          Drawing.Color.Black, _
                                                                          SymbolType.Star)
                Next
                lZgc.SaveIn("Reach" & lOperation.Id & "_Ftable.emf")

                lPaneMain.YAxis.Type = AxisType.Log
                lPaneMain.YAxis.Scale.Min = 1
                lPaneMain.YAxis.Scale.Max = 1000000.0
                lPaneMain.YAxis.Scale.MaxAuto = False
                lZgc.SaveIn("Reach" & lOperation.Id & "_Ftable_Log.emf")
            End If
        Next
    End Sub
End Module
