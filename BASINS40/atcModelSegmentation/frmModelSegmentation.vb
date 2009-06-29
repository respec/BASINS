Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Friend Class frmModelSegmentation

    Event OpenTableEditor(ByVal aLayerName As String)
    Event TableEdited()

    Private pSubbasinLayerNameUserPrompt As String = "Select Subbasins Layer Shapefile"
    Private pMetStationsLayerNameUserPrompt As String = "Select Met Stations Layer Shapefile"
    Private pTitle As String = "BASINS Model Segmentation"

    Private Sub cmdAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAssign.Click
        Dim lSubbasinLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        If lSubbasinLayerName = pSubbasinLayerNameUserPrompt Then
            Logger.Msg(pSubbasinLayerNameUserPrompt, MsgBoxStyle.Critical, pTitle)
        Else
            Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(lSubbasinLayerName)

            Dim lMetLayerName As String = cboMetStations.Items(cboMetStations.SelectedIndex)
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

                Dim lModelSegFieldIndex As Integer = MetSegFieldIndex(lSubbasinLayerIndex)

                'build local collection of selected features in met station layer
                Dim lMetStationsSelected As New atcCollection
                If cbxUseSelected.Checked Then
                    For lIndex As Integer = 1 To GisUtil.NumSelectedFeatures(lMetLayerIndex)
                        lMetStationsSelected.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex - 1, lMetLayerIndex))
                    Next
                End If
                If lMetStationsSelected.Count = 0 Then
                    'no met stations selected, act as if all are selected
                    For lIndex As Integer = 1 To GisUtil.NumFeatures(lMetLayerIndex)
                        lMetStationsSelected.Add(lIndex - 1)
                    Next
                End If

                'loop through each subbasin and assign to nearest met station
                Dim lSubBasinX As Double, lSubBasinY As Double
                Dim lMetSegX As Double, lMetSegY As Double
                GisUtil.StartSetFeatureValue(lSubbasinLayerIndex)
                For lSubBasinIndex As Integer = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                    'do progress
                    Logger.Progress(lSubBasinIndex, GisUtil.NumFeatures(lSubbasinLayerIndex))
                    System.Windows.Forms.Application.DoEvents()
                    'get centroid of this subbasin
                    GisUtil.ShapeCentroid(lSubbasinLayerIndex, lSubBasinIndex - 1, lSubBasinX, lSubBasinY)
                    'now find the closest met station
                    Dim lShortestDistance As Double = Double.MaxValue
                    Dim lClosestMetStationIndex As Integer = -1
                    Dim lDistance As Double = 0.0
                    For lMetStationIndex As Integer = 0 To lMetStationsSelected.Count - 1
                        GisUtil.PointXY(lMetLayerIndex, lMetStationsSelected(lMetStationIndex), lMetSegX, lMetSegY)
                        'calculate the distance
                        'lDistance = Math.Sqrt(((Math.Abs(lSubBasinX) - Math.Abs(lMetSegX)) ^ 2) + _
                        '                      ((Math.Abs(lSubBasinY) - Math.Abs(lMetSegY)) ^ 2))
                        lDistance = Math.Sqrt(((lSubBasinX - lMetSegX) ^ 2) + _
                                              ((lSubBasinY - lMetSegY) ^ 2))
                        If lDistance < lShortestDistance Then
                            lShortestDistance = lDistance
                            lClosestMetStationIndex = lMetStationsSelected(lMetStationIndex)
                        End If
                    Next
                    If lClosestMetStationIndex > -1 Then 'set ModelSeg attribute
                        Dim lModelSegText As String = ""
                        Dim lLocationFieldIndex As Integer = -1
                        If GisUtil.IsField(lMetLayerIndex, "Location") Then
                            lLocationFieldIndex = GisUtil.FieldIndex(lMetLayerIndex, "Location")
                            lModelSegText = GisUtil.FieldValue(lMetLayerIndex, lClosestMetStationIndex, lLocationFieldIndex)
                        End If
                        If GisUtil.IsField(lMetLayerIndex, "Stanam") Then
                            lLocationFieldIndex = GisUtil.FieldIndex(lMetLayerIndex, "Stanam")
                            If lModelSegText.Length > 0 Then
                                lModelSegText = lModelSegText & ":"
                            End If
                            lModelSegText = lModelSegText & GisUtil.FieldValue(lMetLayerIndex, lClosestMetStationIndex, lLocationFieldIndex)
                        End If
                        If lModelSegText.Length = 0 Then
                            lModelSegText = CStr(lClosestMetStationIndex)
                        End If
                        GisUtil.SetFeatureValueNoStartStop(lSubbasinLayerIndex, lModelSegFieldIndex, lSubBasinIndex - 1, lModelSegText)
                    End If
                Next
                GisUtil.StopSetFeatureValue(lSubbasinLayerIndex)
                RaiseEvent TableEdited()
            End If
        End If
    End Sub

    Private Function MetSegFieldIndex(ByVal aSubbasinLayerIndex As Integer) As Integer
        'check to see if modelseg field is on subbasins layer, add if not 
        Dim lModelSegFieldIndex As Integer = -1
        If GisUtil.IsField(aSubbasinLayerIndex, "ModelSeg") Then
            lModelSegFieldIndex = GisUtil.FieldIndex(aSubbasinLayerIndex, "ModelSeg")
        Else  'need to add it
            lModelSegFieldIndex = GisUtil.AddField(aSubbasinLayerIndex, "ModelSeg", 0, 40)
        End If
        Return lModelSegFieldIndex
    End Function

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
        Dim lModelSegFieldIndex As Integer = MetSegFieldIndex(lSubbasinLayerIndex)  'add field if is does not yet exist

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
            Dim lModelSegFieldIndex As Integer = MetSegFieldIndex(lSubbasinLayerIndex)

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

End Class