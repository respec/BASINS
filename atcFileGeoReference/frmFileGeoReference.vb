Imports MapWinUtility
Imports atcMwGisUtility.GisUtil

Public Class frmFileGeoReference
    Private pRecordIndex As Integer = 0 'zero-based index of current record in georef layer
    Private pAddingPoint As Boolean = False
    Private pAddingFiles As Boolean = False
    Private pLayerIndexChanged As Boolean = False

    Private pRandomPlacement As Boolean = False
    Private pRandom As New Random

    Private pBitmap As Drawing.Bitmap

    Private Const pGeographicProjection As String = "+proj=longlat +datum=NAD83"
    Private Const pDefaultLayerName As String = "photos"
    Private Const pAddAnnotationField As String = "Add Annotation field..."
    Private Const pAnnotationFieldName As String = "Annotation"

    Public Event AddPointToggle(ByVal aAdding As Boolean)
    Public Event CreateNewGeoRefLayer()

    Friend ReadOnly Property AnnotationFieldName() As String
        Get
            Return pAnnotationFieldName
        End Get
    End Property

    Friend ReadOnly Property DefaultLayerName() As String
        Get
            Return pDefaultLayerName
        End Get
    End Property

    Public Sub PopulateLayers(Optional ByVal aDocumentLayerIndexName As String = pDefaultLayerName)
        Dim lSetSelectedIndex As Integer = -1
        cboLayer.Items.Clear()
        For lLayerIndex As Integer = 0 To NumLayers - 1
            If LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.PointShapefile Then
                Dim lLayerName As String = LayerName(lLayerIndex)
                If IsField(lLayerIndex, "File") Or _
                   IsField(lLayerIndex, "URL") Or _
                   IsField(lLayerIndex, "Document") Then
                    cboLayer.Items.Add(lLayerName)
                    If lLayerName.ToLower.Equals(aDocumentLayerIndexName.ToLower) Then
                        lSetSelectedIndex = cboLayer.Items.Count - 1
                    End If
                End If
            End If
        Next
        If lSetSelectedIndex = -1 Then
            lSetSelectedIndex = cboLayer.Items.IndexOf(pDefaultLayerName)
        End If
        cboLayer.Items.Add("Create new layer...")
        If lSetSelectedIndex >= 0 Then
            cboLayer.SelectedIndex = lSetSelectedIndex
        End If
        If NumSelectedFeatures(DocumentLayerIndex) > 0 Then
            pRecordIndex = IndexOfNthSelectedFeatureInLayer(0, DocumentLayerIndex)
        Else
            pRecordIndex = 0
            SetSelectedFeature(DocumentLayerIndex, pRecordIndex)
        End If
        SetFormFromFields()
    End Sub

    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLayer.SelectedIndexChanged
        pLayerIndexChanged = True
        Dim lCbo As System.Windows.Forms.ComboBox = sender
        If lCbo.SelectedIndex = lCbo.Items.Count - 1 Then
            RaiseEvent CreateNewGeoRefLayer() 'This causes a new call to this routine with the new index
        Else
            Dim lLayerIndex As Integer = LayerIndex(lCbo.Items(lCbo.SelectedIndex).ToString)

            If lLayerIndex >= 0 Then 'have a selected layer to make current
                LayerVisible = True
                With cboFields
                    .SelectedIndex = -1
                    .Items.Clear()
                    cboAnnotate.Items.Clear()
                    .Text = ""
                    Dim lFieldName As String
                    For lFieldIndex As Integer = 0 To NumFields(lLayerIndex) - 1
                        If FieldType(lFieldIndex) = MapWinGIS.FieldType.STRING_FIELD Then
                            lFieldName = FieldName(lFieldIndex, lLayerIndex)
                            .Items.Add(lFieldName)
                            If lFieldName.ToLower.Equals("file") Then
                                .SelectedIndex = .Items.Count - 1
                            End If
                            If Not lFieldName.ToLower.Equals("file") AndAlso _
                               Not lFieldName.ToLower.Equals("date") Then
                                cboAnnotate.Items.Add(lFieldName)
                                If lFieldName.ToLower.Equals(pAnnotationFieldName.ToLower) Then
                                    cboAnnotate.SelectedIndex = cboAnnotate.Items.Count - 1
                                End If
                            End If
                        End If
                    Next
                    If .Items.Count > 0 Then
                        If .SelectedIndex < 0 Then .SelectedIndex = 0 'Default to first field
                    Else
                        Logger.Msg("No Fields Available for Links")
                    End If
                    If cboAnnotate.SelectedIndex < 0 Then
                        cboAnnotate.Items.Add(pAddAnnotationField)
                        cboAnnotate.SelectedIndex = cboAnnotate.Items.Count - 1
                    End If
                    pRecordIndex = 0
                    SetFormFromFields()
                End With
            End If
        End If
        pLayerIndexChanged = False
    End Sub

    Friend ReadOnly Property DocumentLayerIndex() As Integer
        Get
            If cboLayer.Text.Length > 0 Then
                Return LayerIndex(cboLayer.Text)
            Else
                Return -1 'the current layer
            End If
        End Get
    End Property

    Friend Sub RefreshRecordInfo(ByVal aRecordIndex As Integer)
        pRecordIndex = aRecordIndex
        SetFormFromFields()
    End Sub

    Private Sub btnPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        If pRecordIndex > 0 Then
            pRecordIndex -= 1
        Else
            pRecordIndex = NumFeatures - 1
        End If
        ClearSelectedFeatures(DocumentLayerIndex)
        SetSelectedFeature(DocumentLayerIndex, pRecordIndex)
        SetFormFromFields()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        pRecordIndex += 1
        If pRecordIndex >= NumFeatures Then
            pRecordIndex = 0 'NumFeatures - 1
        End If
        ClearSelectedFeatures(DocumentLayerIndex)
        SetSelectedFeature(DocumentLayerIndex, pRecordIndex)
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
        Dim lFileName As String = aFilename
        If Not lFileName.StartsWith("http://") Then
            lFileName = "file://" & lFileName
        End If

        SetFeatureValue(DocumentLayerIndex, _
                        FieldIndex(DocumentLayerIndex, cboFields.Text), _
                        pRecordIndex, lFileName)

        If aFilename.ToLower.EndsWith(".jpg") Then
            Dim lExif As New ExifWorks(pBitmap)
            Dim lValue As String = ""
            If IsField(DocumentLayerIndex, "date") Then
                Try
                    lValue = Nothing
                    If lExif.IsPropertyDefined(ExifWorks.TagNames.ExifDTOrig) Then
                        lValue = lExif.DateTimeOriginal().ToString
                        Logger.Dbg("Read date from EXIF: " & lValue)
                    Else
                        lValue = System.IO.File.GetCreationTime(aFilename).ToString
                    End If
                    SetFeatureValue(DocumentLayerIndex, _
                                    FieldIndex(DocumentLayerIndex, "date"), _
                                    pRecordIndex, _
                                    lValue)
                Catch ex As Exception
                    Logger.Dbg("Exception trying to read EXIF: " & ex.Message)
                End Try
            End If

            If IsField(DocumentLayerIndex, pAnnotationFieldName) Then
                Try
                    If lExif.IsPropertyDefined(ExifWorks.TagNames.ImageTitle) Then
                        lValue = lExif.Title
                        Logger.Dbg("Read " & pAnnotationFieldName & " from EXIF Title: " & lValue)
                    ElseIf lExif.IsPropertyDefined(ExifWorks.TagNames.ExifUserComment) Then
                        lValue = lExif.UserComment
                        Logger.Dbg("Read " & pAnnotationFieldName & " from EXIF User Comment: " & lValue)
                    End If
                Catch ex As Exception
                    Logger.Dbg("Exception trying to read EXIF: " & ex.Message)
                End Try

                If Not lValue Is Nothing AndAlso lValue.Length > 0 Then
                    SetFeatureValue(DocumentLayerIndex, _
                                    FieldIndex(DocumentLayerIndex, pAnnotationFieldName), _
                                    pRecordIndex, _
                                    lValue)
                End If
            End If

            If IsField(DocumentLayerIndex, "latitude") Then
                Try
                    If lExif.IsPropertyDefined(ExifWorks.TagNames.GpsLatitude) Then
                        lValue = lExif.DateTimeOriginal().ToString
                        Logger.Dbg("Read latitude from EXIF: " & lValue)
                        SetFeatureValue(DocumentLayerIndex, _
                                        FieldIndex(DocumentLayerIndex, "latitude"), _
                                        pRecordIndex, _
                                        lValue)
                    End If
                Catch ex As Exception
                    Logger.Dbg("Exception trying to read EXIF: " & ex.Message)
                End Try
            End If

            If IsField(DocumentLayerIndex, "longitude") Then
                Try
                    If lExif.IsPropertyDefined(ExifWorks.TagNames.GpsLongitude) Then
                        lValue = lExif.DateTimeOriginal().ToString
                        Logger.Dbg("Read longitude from EXIF: " & lValue)
                        SetFeatureValue(DocumentLayerIndex, _
                                        FieldIndex(DocumentLayerIndex, "longitude"), _
                                        pRecordIndex, _
                                        lValue)
                    End If
                Catch ex As Exception
                    Logger.Dbg("Exception trying to read EXIF: " & ex.Message)
                End Try
            End If

        End If
    End Sub

    Friend Sub SetFormFromDocument(ByVal aFilename As String)
        txtLocation.Text = aFilename
        btnLaunch.Visible = False
        btnBrowse.Visible = False
        If aFilename.Length = 0 Then
            If cboFields.Text.ToLower = "url" Then
                lblStatus.Text = "Type the web link in the Location text box"
                txtLocation.Focus()
            Else
                lblStatus.Text = "Click the Browse button to specify the geo referenced file"
            End If
            lblStatus.Visible = True
            pbxImage.Visible = False
            btnBrowse.Visible = True
        ElseIf aFilename.StartsWith("http") Then
            lblStatus.Text = "Web Link"
            lblStatus.Visible = True
            pbxImage.Visible = False
            btnLaunch.Visible = True
        ElseIf Not IO.File.Exists(aFilename) Then
            lblStatus.Text = "File not found"
            lblStatus.Visible = True
            pbxImage.Visible = False
            btnBrowse.Visible = True
        Else
            lblStatus.Visible = False
            Try
                pBitmap = Nothing
                pBitmap = DirectCast(System.Drawing.Bitmap.FromFile(aFilename), System.Drawing.Bitmap)
            Catch
            End Try
            If pBitmap Is Nothing Then 'handle non images 
                btnLaunch.Visible = True
                pbxImage.Visible = False
            Else
                pbxImage.Image = pBitmap
                pbxImage.Visible = True
            End If

            Me.Refresh()

            If txtDate.Text.Length = 0 Then
                txtDate.Text = IO.File.GetCreationTime(aFilename).ToString
            End If
        End If
    End Sub

    Friend Sub SetFormFromFields()
        If Not pAddingFiles Then
            Dim lNumFeatures As Integer = NumFeatures(DocumentLayerIndex)
            If lNumFeatures < 1 Then
                lblRecordInfo.Text = "No Records"
                txtLocation.Visible = False
                lblLocation.Visible = False
                lblDate.Visible = False
                txtDate.Visible = False
                txtAnnotation.Text = ""
                pbxImage.Visible = False
                pRecordIndex = 0
            Else
                If pRecordIndex >= lNumFeatures Then
                    pRecordIndex = lNumFeatures - 1
                End If
                lblRecordInfo.Text = "Record " & pRecordIndex + 1 & " of " & lNumFeatures

                If IsField(DocumentLayerIndex, "date") Then
                    txtDate.Text = FieldValue(DocumentLayerIndex, pRecordIndex, FieldIndex(DocumentLayerIndex, "date"))
                Else
                    txtDate.Text = ""
                End If

                If IsField(DocumentLayerIndex, cboFields.Text) Then
                    Dim lFilename As String = FieldValue(DocumentLayerIndex, pRecordIndex, FieldIndex(DocumentLayerIndex, cboFields.Text))
                    If lFilename.StartsWith("file://") Then
                        lFilename = lFilename.Substring(7)
                    End If
                    SetFormFromDocument(lFilename)
                    lblLocation.Visible = True
                    txtLocation.Visible = True
                Else
                    lblStatus.Text = "Select a field containing document links"
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
                Dim lAnnotationField As String = cboAnnotate.Text
                If lAnnotationField.Length > 0 AndAlso lAnnotationField <> pAddAnnotationField Then
                    txtAnnotation.Text = FieldValue(DocumentLayerIndex, pRecordIndex, FieldIndex(DocumentLayerIndex, lAnnotationField))
                End If
            End If
        End If
    End Sub

    Private Sub txtLocation_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLocation.LostFocus
        If cboFields.Text.ToLower = "url" Then
            Dim lUrl As String = txtLocation.Text
            Dim lPrefix As String = "http://"
            If Not lUrl.StartsWith(lPrefix) Then
                lUrl = lPrefix & lUrl
                SetFieldsFromDocument(lUrl)
                txtLocation.Text = lUrl
            End If
        Else
            Dim lFilename As String = txtLocation.Text
            If FileExists(lFilename) Then
                SetFieldsFromDocument(lFilename)
            Else 'file not found, ask user  
                UserSelectDocument()
            End If
        End If
    End Sub

    Private Sub cboFields_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFields.SelectedIndexChanged
        SetFormFromFields()
    End Sub

    Private Sub cboAnnotate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAnnotate.SelectedIndexChanged
        Dim lCbo As System.Windows.Forms.ComboBox = sender
        If Not pLayerIndexChanged Then
            If lCbo.Text = pAddAnnotationField Then
                Dim lLayerIndex As Integer = LayerIndex(cboLayer.Items(cboLayer.SelectedIndex).ToString)
                If lLayerIndex >= 0 Then
                    AddField(lLayerIndex, pAnnotationFieldName, MapWinGIS.FieldType.STRING_FIELD, 80)
                    lCbo.Items.Add(pAnnotationFieldName)
                    lCbo.Items.Remove(pAddAnnotationField)
                    lCbo.SelectedIndex = lCbo.Items.Count - 1
                End If
            End If
            SetFormFromFields()
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        pbxImage.Visible = False
        btnLaunch.Visible = False
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
        If NumFeatures > lSaveNumFeatures Then 'Added a shape
            pRecordIndex = NumFeatures() - 1
            SetSelectedFeature(DocumentLayerIndex, pRecordIndex)
        End If
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
            RaiseEvent AddPointToggle(value)
        End Set
    End Property

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        ClearSelectedFeatures(DocumentLayerIndex)
        If NumFeatures > pRecordIndex Then RemoveFeature(DocumentLayerIndex, pRecordIndex)
        If pRecordIndex >= NumFeatures Then
            pRecordIndex = NumFeatures - 1
        End If
        SetFormFromFields()
        SetSelectedFeature(DocumentLayerIndex, pRecordIndex)
    End Sub

    Private Sub pbxImage_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbxImage.DoubleClick
        Dim lExif As New ExifWorks(txtLocation.Text)
        Logger.Msg(lExif.ToString, MsgBoxStyle.OkOnly, "Image Metadata")
        lExif.Dispose()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aFileName"></param>
    ''' <param name="aX">units - project projection</param>
    ''' <param name="aY">units - project projection</param>
    ''' <remarks></remarks>
    Friend Sub AddFile(ByVal aFileName As String, Optional ByVal aX As Double = 0, Optional ByVal aY As Double = 0)
        Dim lGeocodedFilename As String = IO.Path.Combine(IO.Path.GetDirectoryName(aFileName), IO.Path.GetFileNameWithoutExtension(aFileName) & ".geo" & IO.Path.GetExtension(aFileName))
        Dim lExif As ExifWorks = Nothing
        Dim lExifHasLL As Boolean = False
        Dim lSaveNumFeatures As Integer = NumFeatures
        Logger.Dbg("Adding file '" & aFileName & "'")
        Dim lY As Double = aY
        Dim lX As Double = aX
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
                lLatitude = lGeoExif.GpsLatitude
                lLongitude = lGeoExif.GpsLongitude
                lY = lLatitude
                lX = lLongitude
                ProjectPoint(lX, lY, pGeographicProjection, ProjectProjection)
                AddPoint(DocumentLayerIndex, lX, lY)
                lExifHasLL = True
            End If
        Catch ex As Exception
            Logger.Dbg("Exception trying to read EXIF: " & ex.Message)
        End Try

        'TODO: handle non images

        If lX = 0 OrElse lY = 0 Then 'Did not get lat/lon from EXIF or arguments
            If pRandomPlacement Then 'Place near center of current view
                Dim lXMax As Double = MapExtentXmax
                Dim lXMin As Double = MapExtentXmin
                Dim lYMax As Double = MapExtentYmax
                Dim lYMin As Double = MapExtentYmin
                lX = lXMin + (lXMax - lXMin) * (0.4 + pRandom.NextDouble / 10)
                lY = lYMin + (lYMax - lYMin) * (0.4 + pRandom.NextDouble / 10)
                AddPoint(DocumentLayerIndex, lX, lY)
            Else 'Ask user to click to place point
                If UserAddPoint() Then
                    PointXY(DocumentLayerIndex, NumFeatures - 1, lX, lY)
                End If
            End If
        ElseIf Not lExifHasLL Then
            AddPoint(DocumentLayerIndex, lX, lY)
        End If

        If (Not lExif Is Nothing) AndAlso (Not lExifHasLL) Then 'Set lat/lon in image to newly geocoded point (the last point in the file)
            If Not ProjectProjection.Equals(pGeographicProjection) Then
                Logger.Dbg("Projecting new point from (" & lX & ", " & lY & ") in '" & ProjectProjection() & "' to '" & pGeographicProjection & "'")
                ProjectPoint(lX, lY, ProjectProjection, pGeographicProjection)
            End If
            lLatitude = lY
            lLongitude = lX
            Logger.Dbg("Setting EXIF GPS lon/lat = " & lLongitude & ", " & lLatitude)
            lExif.GpsLatitude = lLatitude
            lExif.GpsLongitude = lLongitude
            Try
                pBitmap.Save(lGeocodedFilename)
                Logger.Dbg("Saved geocoded file '" & lGeocodedFilename & "'")
            Catch ex As Exception
                Logger.Dbg("Failed to save geocoded file '" & lGeocodedFilename & "' - " & ex.Message)
            End Try
        End If

        If NumFeatures > lSaveNumFeatures Then 'Added a shape
            pRecordIndex = NumFeatures() - 1
            SetFieldsFromDocument(aFileName)
            SetSelectedFeature(DocumentLayerIndex, pRecordIndex)
            SetFormFromFields()
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
        ElseIf e.Data.GetDataPresent(Windows.Forms.DataFormats.Text) AndAlso _
               e.Data.GetData(Windows.Forms.DataFormats.Text).ToString.StartsWith("http://") Then
            AddFile(e.Data.GetData(Windows.Forms.DataFormats.Text).ToString)
        End If
    End Sub

    Private Sub Form_DragEnter( _
        ByVal sender As Object, ByVal e As Windows.Forms.DragEventArgs) _
        Handles Me.DragEnter, pbxImage.DragEnter

        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            e.Effect = Windows.Forms.DragDropEffects.All
        ElseIf e.Data.GetDataPresent(Windows.Forms.DataFormats.Text) Then
            e.Effect = Windows.Forms.DragDropEffects.All
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pAddingFiles = False
        AddingPoint = False
    End Sub

    Private Sub txtAnnotation_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAnnotation.TextChanged
        If NumFeatures(DocumentLayerIndex) > 0 Then
            Dim lAnnotationField As String = cboAnnotate.Text
            If lAnnotationField.Length > 0 AndAlso lAnnotationField <> pAddAnnotationField Then
                SetFeatureValue(DocumentLayerIndex, FieldIndex(DocumentLayerIndex, lAnnotationField), pRecordIndex, txtAnnotation.Text)
            End If

            If Not pBitmap Is Nothing AndAlso FileExists(txtLocation.Text) Then
                Dim lGeocodedFilename As String = IO.Path.Combine(IO.Path.GetDirectoryName(txtLocation.Text), IO.Path.GetFileNameWithoutExtension(txtLocation.Text) & ".geo.jpg")
                Dim lExif As New ExifWorks(pBitmap)
                If Not lExif.Title = txtAnnotation.Text Then
                    lExif.Title = txtAnnotation.Text
                    Try
                        pBitmap.Save(lGeocodedFilename)
                        Logger.Dbg("Saved geocoded file '" & lGeocodedFilename & "'")
                    Catch ex As Exception
                        Logger.Dbg("Failed to save geocoded file '" & lGeocodedFilename & "' - " & ex.Message)
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub btnLaunch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLaunch.Click
        Logger.Dbg("FileGeoReference: Launch File or URL: " & txtLocation.Text)
        Dim lProcess As New Process
        lProcess.StartInfo.FileName = txtLocation.Text
        lProcess.Start()
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        UserSelectDocument()
    End Sub
End Class