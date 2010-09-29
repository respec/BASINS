Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Friend Class frmModelSegmentation
    Implements DotSpatial.Main.IProgressHandler

    Event OpenTableEditor(ByVal aLayerName As String)
    Event TableEdited()

    Private pSubbasinLayerNameUserPrompt As String = "Select Subbasins Layer Shapefile"
    Private pMetStationsLayerNameUserPrompt As String = "Select Met Stations Layer Shapefile"
    Private pTitle As String = "BASINS Model Segmentation"

    Private Sub cmdAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAssign.Click
        Dim lSubbasinLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        Dim lMetLayerName As String = cboMetStations.Items(cboMetStations.SelectedIndex)
        If lSubbasinLayerName = pSubbasinLayerNameUserPrompt Then
            Logger.Msg(pSubbasinLayerNameUserPrompt, MsgBoxStyle.Critical, pTitle)
        Else
            If lMetLayerName = pMetStationsLayerNameUserPrompt Then
                Logger.Msg(pMetStationsLayerNameUserPrompt, MsgBoxStyle.Critical, pTitle)
            Else
                Dim lMetLayerIndex As Integer = GisUtil.LayerIndex(lMetLayerName)

                If cbxUseSelected.Checked And GisUtil.NumSelectedFeatures(lMetLayerIndex) = 0 Then
                    'nothing selected in specified layer, let user know this is a problem
                    Logger.Msg("Nothing is selected in layer '" & lMetLayerName & "'.", MsgBoxStyle.Critical, "Assign Met Segments Problem")
                    Exit Sub
                End If

                'change to hourglass cursor
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                PlugIn.AssignMetStationsByProximity(lSubbasinLayerName, lMetLayerName, cbxUseSelected.Checked)
                RaiseEvent TableEdited()
            End If
        End If
    End Sub

    Private Sub frmModelSegmentation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\Model Segmentation Specifier.html")
        End If
    End Sub

    Private Sub frmModelSegmentation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'subwatersheds
        Dim lDefaultLayerIndex As Integer = -1
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 3 Then 'Polygon Shapefile 
                cboSubbasins.Items.Add(lLayerName)
                If GisUtil.CurrentLayer = lLayerIndex Then 'layer selected, default to it
                    lDefaultLayerIndex = cboSubbasins.Items.Count - 1
                ElseIf (lLayerName.ToUpper = "SUBBASINS" Or _
                        lLayerName.IndexOf("Watershed Shapefile") >= 0 Or _
                        lLayerName.IndexOf("Catchment") >= 0) And _
                        lDefaultLayerIndex < 0 Then 'this looks like a reasonable default layer
                    lDefaultLayerIndex = cboSubbasins.Items.Count - 1
                End If
            End If
        Next
        If lDefaultLayerIndex > -1 Then 'have a default layer
            cboSubbasins.SelectedIndex = lDefaultLayerIndex
        ElseIf cboSubbasins.Items.Count = 0 Then
            Logger.Msg("No polygon layers available as potential subbasins layer", MsgBoxStyle.Critical, pTitle)
        Else 'prompt user
            cboSubbasins.Items.Insert(0, pSubbasinLayerNameUserPrompt)
            cboSubbasins.SelectedIndex = 0
        End If

        'met segments
        lDefaultLayerIndex = -1
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 1 Then 'Point Shapefile 
                cboMetStations.Items.Add(lLayerName)
                If GisUtil.CurrentLayer = lLayerIndex Then
                    'this layer is selected, default to it
                    lDefaultLayerIndex = cboMetStations.Items.Count - 1
                ElseIf (lLayerName.IndexOf("Weather Station Sites") >= 0 Or _
                        lLayerName.IndexOf("Met Stations") >= 0) And _
                        lDefaultLayerIndex < 0 Then 'reasonable default layer
                    lDefaultLayerIndex = cboMetStations.Items.Count - 1
                End If
            End If
        Next
        If lDefaultLayerIndex > -1 Then 'have a default layer
            cboMetStations.SelectedIndex = lDefaultLayerIndex
        ElseIf cboMetStations.Items.Count = 0 Then
            Logger.Msg("No point layers available as potential met stations layer", MsgBoxStyle.Critical, pTitle)
        Else 'prompt user
            cboMetStations.Items.Insert(0, pMetStationsLayerNameUserPrompt)
            cboMetStations.SelectedIndex = 0
        End If

        If cboMetStations.Items.Count = 0 Or cboSubbasins.Items.Count = 0 Then
            Me.Dispose()
        End If
    End Sub

    Private Sub cboSubbasins_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSubbasins.SelectedIndexChanged
        Dim lSubbasinLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        If lSubbasinLayerName <> pSubbasinLayerNameUserPrompt Then
            'RaiseEvent OpenTableEditor(lSubbasinLayerName)   'do we really want to do this all the time?  PD
            'Me.Focus()
        End If
    End Sub

    Private Sub cmdEditTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEditTable.Click
        Dim lSubbasinLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(lSubbasinLayerName)
        Dim lModelSegFieldIndex As Integer = PlugIn.MetSegFieldIndex(lSubbasinLayerIndex)  'add field if is does not yet exist

        If lSubbasinLayerName <> pSubbasinLayerNameUserPrompt Then
            RaiseEvent OpenTableEditor(lSubbasinLayerName)
            Me.Focus()
        End If
    End Sub

    Private Sub cmdViewMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdViewMap.Click
        Dim lSubbasinLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        If lSubbasinLayerName = pSubbasinLayerNameUserPrompt Then
            Logger.Msg(pSubbasinLayerNameUserPrompt, MsgBoxStyle.Critical, "BASINS Model Segmentation")
        Else
            Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(lSubbasinLayerName)
            Dim lModelSegFieldIndex As Integer = PlugIn.MetSegFieldIndex(lSubbasinLayerIndex)

            'save original coloring scheme in case we want to return to it
            'TODO: figure out why this returns Nothing - does not store OutlineColor, etc?
            Dim lColoringScheme As MapWinGIS.ShapefileColorScheme = GisUtil.GetColoringScheme(lSubbasinLayerIndex)
            'do the renderer
            GisUtil.UniqueValuesRenderer(lSubbasinLayerIndex, lModelSegFieldIndex)
            If Logger.Msg("Do you want to keep this thematic map?", MsgBoxStyle.YesNo, "View Map") = MsgBoxResult.No Then
                'revert to original renderer
                GisUtil.ColoringScheme(lSubbasinLayerIndex) = lColoringScheme
            End If
        End If
    End Sub

    Private Sub cmdThiessen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdThiessen.Click
        'todo: implement using selected features option
        'todo: add attributes to output thiessens
        'todo: check/remove thiessen layer that already exists

        Dim lMetLayerName As String = cboMetStations.Items(cboMetStations.SelectedIndex)
        Dim lMetLayerFileName As String = GisUtil.LayerFileName(lMetLayerName)
        Dim lTempPath As String = IO.Path.GetTempPath

        'copy the input points to a temp file because the dot spatial function insists the points not be on the map
        TryCopyShapefile(lMetLayerFileName, lTempPath)
        Dim lTempPointFileName As String = IO.Path.Combine(lTempPath, IO.Path.GetFileName(lMetLayerFileName))

        'open the points into a dot spatial feature set
        Dim lPoints As DotSpatial.Data.IFeatureSet = New DotSpatial.Data.PointShapefile(lTempPointFileName)

        'because of a strange feature of the dot spatial voronoi algorithm, need scaled-down points
        Dim lScaledDownPoints As DotSpatial.Data.IFeatureSet = New DotSpatial.Data.FeatureSet()
        Dim lScaleFactor As Integer = 100000
        For Each lFeature As DotSpatial.Data.Feature In lPoints.Features
            Dim lX As Double = lFeature.Coordinates(0).X / lScaleFactor
            Dim lY As Double = lFeature.Coordinates(0).Y / lScaleFactor
            Dim lPt As New DotSpatial.Geometries.Point(lX, lY)
            Dim lNewFeature As New DotSpatial.Data.Feature(lPt)
            lScaledDownPoints.AddFeature(lNewFeature)
        Next

        'calculate the thiessen polygons
        Dim lScaledDownPolygons As DotSpatial.Data.IFeatureSet = DotSpatial.Analysis.Voronoi.VoronoiPolygons(lScaledDownPoints, True, Nothing)

        'save the scaled down thiessen polygons
        'Dim lScaledDownOutputFile As String = IO.Path.GetDirectoryName(lMetLayerFileName) & g_PathChar & IO.Path.GetFileNameWithoutExtension(lMetLayerFileName) & "ScaledDownThiessens.shp"
        'lScaledDownPolygons.SaveAs(lScaledDownOutputFile, True)
        'TryCopy(IO.Path.GetDirectoryName(lMetLayerFileName) & g_PathChar & IO.Path.GetFileNameWithoutExtension(lMetLayerFileName) & ".prj", _
        '        IO.Path.GetDirectoryName(lScaledDownOutputFile) & g_PathChar & IO.Path.GetFileNameWithoutExtension(lScaledDownOutputFile) & ".prj")
        'GisUtil.AddLayer(lScaledDownOutputFile, "Scaled Down Thiessen Polygons")

        'find max coordinate of scaled down version to use in upscaling
        Dim lMaxCoordinate As Double = Math.Abs(lScaledDownPolygons.Envelope.Maximum.Y)
        If Math.Abs(lScaledDownPolygons.Envelope.Maximum.X) > lMaxCoordinate Then
            lMaxCoordinate = Math.Abs(lScaledDownPolygons.Envelope.Maximum.X)
        End If
        If Math.Abs(lScaledDownPolygons.Envelope.Minimum.Y) > lMaxCoordinate Then
            lMaxCoordinate = Math.Abs(lScaledDownPolygons.Envelope.Minimum.Y)
        End If
        If Math.Abs(lScaledDownPolygons.Envelope.Minimum.X) > lMaxCoordinate Then
            lMaxCoordinate = Math.Abs(lScaledDownPolygons.Envelope.Minimum.X)
        End If

        'now scale the polygons back up
        Dim lFullSizePolygons As DotSpatial.Data.IFeatureSet = New DotSpatial.Data.FeatureSet()
        lFullSizePolygons = lScaledDownPolygons
        For Each lFeature As DotSpatial.Data.Feature In lFullSizePolygons.Features
            For Each lCoordinate As DotSpatial.Geometries.Coordinate In lFeature.Coordinates
                If Math.Abs(lCoordinate.X) <= lMaxCoordinate Then  'have to check to see if we've already scaled this point
                    lCoordinate.X = lCoordinate.X * lScaleFactor
                End If
                If Math.Abs(lCoordinate.Y) <= lMaxCoordinate Then
                    lCoordinate.Y = lCoordinate.Y * lScaleFactor
                End If
            Next
        Next

        'save the thiessen polygons
        Dim lOutputFile As String = IO.Path.GetDirectoryName(lMetLayerFileName) & g_PathChar & IO.Path.GetFileNameWithoutExtension(lMetLayerFileName) & "Thiessens.shp"
        lFullSizePolygons.SaveAs(lOutputFile, True)
        TryCopy(IO.Path.GetDirectoryName(lMetLayerFileName) & g_PathChar & IO.Path.GetFileNameWithoutExtension(lMetLayerFileName) & ".prj", _
                IO.Path.GetDirectoryName(lOutputFile) & g_PathChar & IO.Path.GetFileNameWithoutExtension(lOutputFile) & ".prj")

        Dim lNewLayerName As String = "Thiessen Polygons for " & lMetLayerName
        GisUtil.AddLayer(lOutputFile, lNewLayerName)
        GisUtil.LayerVisible(lNewLayerName) = True
        TryDeleteShapefile(lTempPointFileName)
    End Sub

    Public Sub Progress(ByVal Key As String, ByVal Percent As Integer, ByVal Message As String) Implements DotSpatial.Main.IProgressHandler.Progress
        Logger.Progress(Percent, 100)
    End Sub
End Class