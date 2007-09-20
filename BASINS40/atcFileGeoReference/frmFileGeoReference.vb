Imports MapWinUtility
Imports atcMwGisUtility.GisUtil

Public Class frmFileGeoReference
    Private pRecordIndex As Integer = 0 'zero-based index of current record in georef layer
    Private pAddingPoint As Boolean = False
    Private pAddingFiles As Boolean = False

    Private pRandomPlacement As Boolean = False
    Private pRandom As New Random

    Private pBitmap As Drawing.Bitmap

    Private Const pGeographicProjection As String = "+proj=longlat +datum=NAD83"

    Public Sub PopulateLayers(Optional ByVal aCurrentLayerName As String = "photo")
        Dim lSetSelectedIndex As Integer = -1
        cboLayer.Items.Clear()
        For lLayerIndex As Integer = 0 To NumLayers - 1
            If LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.PointShapefile Then
                Dim lLayerName As String = LayerName(lLayerIndex)
                cboLayer.Items.Add(lLayerName)
                If lLayerName.ToLower.Equals(aCurrentLayerName.ToLower) OrElse InStr(lLayerName.ToLower, "photo") Then
                    lSetSelectedIndex = cboLayer.Items.Count - 1
                End If
            End If
        Next
        cboLayer.Items.Add("Create new layer...")
        If lSetSelectedIndex >= 0 Then
            cboLayer.SelectedIndex = lSetSelectedIndex
        End If
        If NumSelectedFeatures(CurrentLayer) > 0 Then
            pRecordIndex = IndexOfNthSelectedFeatureInLayer(0, CurrentLayer)
        Else
            pRecordIndex = 0
            SetSelectedFeature(CurrentLayer, pRecordIndex)
        End If
        SetFormFromFields()
    End Sub

    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLayer.SelectedIndexChanged
        Dim lCbo As System.Windows.Forms.ComboBox = sender
        If lCbo.SelectedIndex = lCbo.Items.Count - 1 Then
            CreateNewGeoRefLayer() 'This causes a new call to this routine with the new index
        Else
            Dim lLayerIndex As Integer = LayerIndex(lCbo.Items(lCbo.SelectedIndex).ToString)

            If lLayerIndex >= 0 Then 'have a selected layer to make current
                CurrentLayer = lLayerIndex
                LayerVisible = True
                With cboFields
                    .SelectedIndex = -1
                    .Items.Clear()
                    .Text = ""
                    Dim lFieldName As String
                    For lFieldIndex As Integer = 0 To NumFields - 1
                        If FieldType(lFieldIndex) = MapWinGIS.FieldType.STRING_FIELD Then
                            lFieldName = FieldName(lFieldIndex, lLayerIndex)
                            .Items.Add(lFieldName)
                            If lFieldName.ToLower.Equals("file") Then
                                .SelectedIndex = .Items.Count - 1
                            End If
                        End If
                    Next
                    If .Items.Count > 0 Then
                        If .SelectedIndex < 0 Then .SelectedIndex = 0 'Default to first field
                    Else
                        Logger.Msg("No Fields Available for Links")
                    End If
                    pRecordIndex = 0
                    SetFormFromFields()
                End With
            End If
        End If
    End Sub

    ''' <summary>Create a New GeoReferencing Point Shape Layer</summary>
    ''' <returns>Layer Index of new layer, or -1 if not created</returns>
    Private Function CreateNewGeoRefLayer() As Integer
        Dim lDialog As New System.Windows.Forms.SaveFileDialog
        With lDialog
            .DefaultExt = ".shp"
            .AddExtension = True
            .Filter = "Shape files (*.shp)|*.shp|All files (*.*)|*.*"
            .FilterIndex = 0
            .FileName = txtLocation.Text
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim lLayerCaption As String = IO.Path.GetFileNameWithoutExtension(.FileName)

                Dim lNewLayer As New MapWinGIS.Shapefile
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(.FileName))
                lNewLayer.CreateNew(.FileName, MapWinGIS.ShpfileType.SHP_POINT)

                lNewLayer.StartEditingTable()
                lNewLayer.EditInsertField(NewField("file", MapWinGIS.FieldType.STRING_FIELD, 1024), 1)
                lNewLayer.EditInsertField(NewField("date", MapWinGIS.FieldType.STRING_FIELD, 10), 2)
                lNewLayer.EditInsertField(NewField("comment", MapWinGIS.FieldType.STRING_FIELD, 1024), 3)
                lNewLayer.EditInsertField(NewField("latitude", MapWinGIS.FieldType.DOUBLE_FIELD), 4)
                lNewLayer.EditInsertField(NewField("longitude", MapWinGIS.FieldType.DOUBLE_FIELD), 5)
                lNewLayer.StopEditingTable()

                lNewLayer.StartEditingShapes()
                'lNewLayer.EditInsertShape(lShape, 0)
                lNewLayer.StopEditingShapes()

                lNewLayer.SaveAs(.FileName)
                lNewLayer.Close()

                lNewLayer = Nothing

                AddLayer(.FileName, lLayerCaption)
                PopulateLayers(lLayerCaption)
                Return LayerIndex(lLayerCaption)
            End If
        End With
        Return -1
    End Function

    Private Function NewField(ByVal aFieldName As String, _
                     Optional ByVal aType As MapWinGIS.FieldType = MapWinGIS.FieldType.STRING_FIELD, _
                     Optional ByVal aWidth As Integer = 0) As MapWinGIS.Field
        Dim lNewField As New MapWinGIS.Field
        lNewField.Name = aFieldName
        lNewField.Type = aType
        If aWidth < 1 Then
            Select Case aType
                'Case MapWinGIS.FieldType.DOUBLE_FIELD : lNewField.Width = 8
                'Case MapWinGIS.FieldType.INTEGER_FIELD : lNewField.Width = 4
                Case MapWinGIS.FieldType.STRING_FIELD : lNewField.Width = 80
            End Select
        Else
            lNewField.Width = aWidth
        End If
        Return lNewField
    End Function

    Friend Sub RefreshRecordInfo(ByVal aRecordIndex As Integer)
        pRecordIndex = aRecordIndex
        SetFormFromFields()
    End Sub

    Private Sub btnPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        If pRecordIndex > 0 Then pRecordIndex -= 1
        ClearSelectedFeatures(CurrentLayer)
        SetSelectedFeature(CurrentLayer, pRecordIndex)
        SetFormFromFields()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        pRecordIndex += 1
        If pRecordIndex >= NumFeatures Then
            pRecordIndex = NumFeatures - 1
        End If
        ClearSelectedFeatures(CurrentLayer)
        SetSelectedFeature(CurrentLayer, pRecordIndex)
        SetFormFromFields()
    End Sub

    Private Sub UserSelectDocument()
        Dim lOpenImageDialog As New System.Windows.Forms.OpenFileDialog
        With lOpenImageDialog
            .DefaultExt = ".jpg"
            .CheckFileExists = True
            .AddExtension = True
            .Filter = "jpeg files (*.jpg)|*.jpg|All files (*.*)|*.*"
            .FilterIndex = 1
            .FileName = txtLocation.Text
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                SetFormFromDocument(.FileName)
                SetFieldsFromDocument(.FileName)
            End If
        End With
    End Sub

    Private Sub SetFieldsFromDocument(ByVal aFilename As String)
        SetFeatureValue(CurrentLayer, _
                        FieldIndex(CurrentLayer, cboFields.Text), _
                        pRecordIndex, _
                        "file://" & aFilename)

        If IsField(CurrentLayer, "date") Then
            Dim lDate As String = Nothing

            Try
                Dim lExif As New ExifWorks(pBitmap)
                If lExif.IsPropertyDefined(ExifWorks.TagNames.ExifDTOrig) Then
                    lDate = lExif.DateTimeOriginal().ToString
                    Logger.Dbg("Read date from EXIF: " & lDate)
                End If
            Catch ex As Exception
                Logger.Dbg("Exception trying to read EXIF: " & ex.Message)
            End Try

            If lDate Is Nothing Then lDate = System.IO.File.GetCreationTime(aFilename).ToString

            SetFeatureValue(CurrentLayer, _
                            FieldIndex(CurrentLayer, "date"), _
                            pRecordIndex, _
                            lDate)
        End If
    End Sub

    Friend Sub SetFormFromDocument(ByVal aFilename As String)
        txtLocation.Text = aFilename
        If aFilename.Length = 0 Then
            lblStatus.Text = "Click in the Value text box to specify the file location"
            lblStatus.Visible = True
            pbxImage.Visible = False
        ElseIf Not IO.File.Exists(aFilename) Then
            lblStatus.Text = "File not found"
            lblStatus.Visible = True
            pbxImage.Visible = False
        Else
            lblStatus.Visible = False
            pBitmap = DirectCast(System.Drawing.Bitmap.FromFile(aFilename), System.Drawing.Bitmap)
            pbxImage.Image = pBitmap
            pbxImage.Visible = True
            Me.Refresh()

            If txtDate.Text.Length = 0 Then
                txtDate.Text = IO.File.GetCreationTime(aFilename).ToString
            End If

            'TODO: handle non images
            'If lFileOrURL.Length > 0 Then
            '    MapWinUtility.Logger.Dbg("FileGeoReference: Launch File or URL: " & lFileOrURL)
            '    Dim lNewProcess As New Process
            '    lNewProcess.StartInfo.FileName = lFileOrURL
            '    lNewProcess.Start()
            'End If
        End If

    End Sub

    Friend Sub SetFormFromFields()
        If Not pAddingFiles Then
            If NumFeatures < 1 Then
                lblRecordInfo.Text = "No Records"
                lblDate.Visible = False
                txtDate.Visible = False
                pbxImage.Visible = False
                pRecordIndex = 0
            Else
                If pRecordIndex >= NumFeatures Then pRecordIndex = NumFeatures - 1
                lblRecordInfo.Text = "Record " & pRecordIndex + 1 & " of " & NumFeatures

                If IsField(CurrentLayer, "date") Then
                    txtDate.Text = FieldValue(CurrentLayer, pRecordIndex, FieldIndex(CurrentLayer, "date"))
                Else
                    txtDate.Text = ""
                End If

                If IsField(CurrentLayer, cboFields.Text) Then
                    Dim lFilename As String = FieldValue(CurrentLayer, pRecordIndex, FieldIndex(CurrentLayer, cboFields.Text))
                    If lFilename.StartsWith("file://") Then lFilename = lFilename.Substring(7)
                    SetFormFromDocument(lFilename)
                Else
                    lblStatus.Text = "Select a field containing documents"
                    lblStatus.Visible = True
                    pbxImage.Visible = False
                End If

                If txtDate.Text.Length > 0 Then
                    lblDate.Visible = True
                    txtDate.Visible = True
                Else
                    lblDate.Visible = False
                    txtDate.Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub txtValue_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtLocation.MouseClick
        UserSelectDocument()
    End Sub

    Private Sub cboFields_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFields.SelectedIndexChanged
        SetFormFromFields()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        pbxImage.Visible = False
        UserAddPoint()
    End Sub

    ''' <summary>
    ''' Ask the user to click the map to specify a new point
    ''' </summary>
    ''' <returns>True if point was selected, False if cancelled</returns>
    Private Function UserAddPoint() As Boolean
        Dim lSaveNumFeatures As Integer = NumFeatures
        lblStatus.Text = "Click the map to specify the location of this item"
        AddingPoint = True
        While pAddingPoint
            System.Windows.Forms.Application.DoEvents()
        End While
        SetFormFromFields()
        Return (NumFeatures > lSaveNumFeatures) 'True if feature was added
    End Function

    Friend Property AddingPoint() As Boolean
        Get
            Return pAddingPoint
        End Get
        Set(ByVal value As Boolean)
            pAddingPoint = value
            lblStatus.Visible = pAddingPoint
            btnCancel.Visible = pAddingPoint
            grpDocument.Visible = Not pAddingPoint
        End Set
    End Property

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        ClearSelectedFeatures(CurrentLayer)
        RemoveFeature(CurrentLayer, pRecordIndex)
        If pRecordIndex >= NumFeatures Then
            pRecordIndex = NumFeatures - 1
        End If
        SetFormFromFields()
        SetSelectedFeature(CurrentLayer, pRecordIndex)
    End Sub

    Private Sub pbxImage_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbxImage.DoubleClick
        Dim lExif As New ExifWorks(txtLocation.Text)
        Logger.Msg(lExif.ToString, MsgBoxStyle.OkOnly, "Image Metadata")
    End Sub

    Private Sub AddFile(ByVal aFileName As String)
        Dim lGeocodedFilename As String = IO.Path.Combine(IO.Path.GetDirectoryName(aFileName), IO.Path.GetFileNameWithoutExtension(aFileName) & ".geo" & IO.Path.GetExtension(aFileName))
        Dim lExif As ExifWorks = Nothing
        Dim lSaveNumFeatures As Integer = NumFeatures
        Logger.Dbg("Adding file '" & aFileName & "'")
        Dim lY As Double = 0
        Dim lX As Double = 0
        Dim lLatitude As String = Nothing
        Dim lLongitude As String = Nothing
        Dim lDate As String = Nothing
        txtLocation.Text = aFileName

        SetFormFromDocument(aFileName)

        Try 'get lat/lon from EXIF tags inside image document
            lExif = New ExifWorks(pBitmap)
            Dim lGeoExif As ExifWorks = lExif
            If Not lExif.IsPropertyDefined(ExifWorks.TagNames.GpsLatitude) AndAlso FileExists(lGeocodedFilename) Then
                lGeoExif = New ExifWorks(lGeocodedFilename)
            End If
            If lGeoExif.IsPropertyDefined(ExifWorks.TagNames.GpsLatitude) Then
                lLatitude = lGeoExif.GetPropertyFormatted(ExifWorks.TagNames.GpsLatitude)
                lLongitude = lGeoExif.GetPropertyFormatted(ExifWorks.TagNames.GpsLongitude)
                lY = lLatitude
                lX = lLongitude
                ProjectPoint(lX, lY, pGeographicProjection, ProjectProjection)
                AddPoint(CurrentLayer, lX, lY)
            End If
        Catch ex As Exception
            Logger.Dbg("Exception trying to read EXIF: " & ex.Message)
        End Try

        'TODO: handle non images

        If lX = 0 OrElse lY = 0 Then 'Did not get lat/lon from EXIF
            If pRandomPlacement Then 'Place near center of current view
                Dim lXMax As Double = MapExtentXmax
                Dim lXMin As Double = MapExtentXmin
                Dim lYMax As Double = MapExtentYmax
                Dim lYMin As Double = MapExtentYmin
                lX = lXMin + (lXMax - lXMin) * (0.4 + pRandom.NextDouble / 10)
                lY = lYMin + (lYMax - lYMin) * (0.4 + pRandom.NextDouble / 10)
                AddPoint(CurrentLayer, lX, lY)
            Else 'Ask user to click to place point
                If UserAddPoint() Then
                    If Not lExif Is Nothing Then 'Set lat/lon in image to newly geocoded point
                        PointXY(CurrentLayer, pRecordIndex, lX, lY)
                        If Not ProjectProjection.Equals(pGeographicProjection) Then
                            Logger.Dbg("Projecting new point from (" & lX & ", " & lY & ") in '" & ProjectProjection() & "' to '" & pGeographicProjection & "'")
                            ProjectPoint(lX, lY, ProjectProjection, pGeographicProjection)
                        End If
                        lLatitude = lY
                        lLongitude = lX
                        Logger.Dbg("Setting EXIF GPS lon/lat = " & lLongitude & ", " & lLatitude)
                        lExif.SetCoordinateGPS(ExifWorks.TagNames.GpsLatitude, lLatitude)
                        lExif.SetCoordinateGPS(ExifWorks.TagNames.GpsLongitude, lLongitude)
                        Try
                            pBitmap.Save(lGeocodedFilename)
                            Logger.Dbg("Saved geocoded file '" & lGeocodedFilename & "'")
                        Catch ex As Exception
                            Logger.Dbg("Failed to save geocoded file '" & lGeocodedFilename & "' - " & ex.Message)
                        End Try
                    End If
                End If
            End If
        End If

        If NumFeatures > lSaveNumFeatures Then 'Added a shape
            pRecordIndex = NumFeatures() - 1
            SetFieldsFromDocument(aFileName)
            SetSelectedFeature(CurrentLayer, pRecordIndex)
        End If
    End Sub

    Private Sub Form_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop, pbxImage.DragDrop
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim lFileNames() As String = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
            pAddingFiles = True
            For Each lFilename As String In lFileNames
                AddFile(lFilename)
                If Not pAddingFiles Then Exit For
            Next
            pAddingFiles = False
            SetFormFromFields()
        End If
    End Sub

    Private Sub Form_DragEnter( _
        ByVal sender As Object, ByVal e As Windows.Forms.DragEventArgs) _
        Handles Me.DragEnter, pbxImage.DragEnter

        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            e.Effect = Windows.Forms.DragDropEffects.All
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pAddingFiles = False
        AddingPoint = False
    End Sub

End Class