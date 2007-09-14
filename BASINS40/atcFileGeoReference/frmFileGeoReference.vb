Imports MapWinUtility
Imports atcMwGisUtility.GisUtil

Public Class frmFileGeoReference
    Private pRecordIndex As Integer = 1
    Private pAddingPoint As Boolean = False
    Private pRandom As New Random

    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLayer.SelectedIndexChanged
        Dim lCbo As System.Windows.Forms.ComboBox = sender
        Dim lLayerIndex As Integer = LayerIndex(lCbo.Items(lCbo.SelectedIndex).ToString)
        CurrentLayer = lLayerIndex

        With cboFields
            .Items.Clear()
            .Text = ""
            For lFieldIndex As Integer = 0 To NumFields - 1
                If FieldType(lFieldIndex) = MapWinGIS.FieldType.STRING_FIELD Then
                    .Items.Add(FieldName(lFieldIndex, lLayerIndex))
                End If
            Next
            If .Items.Count > 0 Then
                .SelectedIndex = 0 'TODO: use 'file', 'url', or similar
            Else
                Logger.Msg("No Fields Available for Links")
            End If
            pRecordIndex = 1
            RefreshRecordInfo()
        End With
    End Sub

    Friend Sub RefreshRecordInfo(ByVal aRecordIndex As Integer)
        pRecordIndex = aRecordIndex
        RefreshRecordInfo()
    End Sub

    Friend Sub RefreshRecordInfo()
        If pRecordIndex > NumFeatures Then pRecordIndex = NumFeatures
        If NumFeatures = 0 Then
            lblRecordInfo.Text = "No Records Available"
        Else
            lblRecordInfo.Text = "Record " & pRecordIndex & " of " & NumFeatures
            Dim lImageLocation As String = FieldValue(CurrentLayer, pRecordIndex - 1, FieldIndex(CurrentLayer, cboFields.Text))
            If lImageLocation.Length = 0 Then
                lblStatus.Text = "Click in the Value text box to specify the file location"
            Else
                txtValue.Text = lImageLocation
                'TODO: handle non images
                pbxImage.ImageLocation = lImageLocation
            End If
            ClearSelectedFeatures(CurrentLayer)
            SetSelectedFeature(CurrentLayer, pRecordIndex - 1)
        End If
    End Sub

    Private Sub btnPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        If pRecordIndex > 1 Then
            pRecordIndex -= 1
            RefreshRecordInfo()
        End If
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If pRecordIndex < NumFeatures Then
            pRecordIndex += 1
            RefreshRecordInfo()
        End If
    End Sub

    Private Sub txtValue_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtValue.MouseClick
        Dim lOpenImageDialog As New System.Windows.Forms.OpenFileDialog
        With lOpenImageDialog
            .DefaultExt = ".jpg"
            .CheckFileExists = True
            .AddExtension = True
            .Filter = "jpeg files (*.jpg)|*.jpg|All files (*.*)|*.*"
            .FilterIndex = 1
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                SetFeatureValue(CurrentLayer, _
                                FieldIndex(CurrentLayer, cboFields.Text), _
                                pRecordIndex - 1, _
                                "file://" & .FileName)
                If IsField(CurrentLayer, "date") Then
                    SetFeatureValue(CurrentLayer, _
                    FieldIndex(CurrentLayer, "date"), _
                    pRecordIndex - 1, _
                    System.IO.File.GetCreationTime(.FileName))
                End If
                RefreshRecordInfo()
            End If
        End With
    End Sub

    Private Sub cboFields_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFields.SelectedIndexChanged
        RefreshRecordInfo()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        pbxImage.Visible = False
        lblStatus.Text = "Click at the Point on the Map associated with the new Item, RightClick anywhere to Cancel"
        lblStatus.Visible = True
        pAddingPoint = True
        While pAddingPoint
            System.Windows.Forms.Application.DoEvents()
        End While
        lblStatus.Visible = False
        lblStatus.Text = ""
        pbxImage.Visible = True
        RefreshRecordInfo()
    End Sub

    Friend Property AddingPoint() As Boolean
        Get
            Return pAddingPoint
        End Get
        Set(ByVal value As Boolean)
            pAddingPoint = value
        End Set
    End Property

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        'TODO: resolve issues with deleting last record
        RemoveFeature(CurrentLayer, pRecordIndex - 1)
        RefreshRecordInfo()
    End Sub

    Private Sub pbxImage_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbxImage.DoubleClick
        Dim lExif As New ExifWorks(txtValue.Text)
        MsgBox(lExif.ToString, MsgBoxStyle.OkOnly, "Image Metadata")
    End Sub

    Private Sub Form_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop, pbxImage.DragDrop
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim MyFiles() As String
            MyFiles = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
            For Each lFilename As String In MyFiles
                Dim lY As Double = 0
                Dim lX As Double = 0
                txtValue.Text = lFilename
                'TODO: handle non images
                Try
                    pbxImage.ImageLocation = lFilename
                    Dim lExif As New ExifWorks(lFilename)
                    If lExif.IsPropertyDefined(ExifWorks.TagNames.GpsLatitude) Then
                        'Place new point at given lat/long from image
                        lY = lExif.GetPropertyFormatted(ExifWorks.TagNames.GpsLatitude)
                        lX = lExif.GetPropertyFormatted(ExifWorks.TagNames.GpsLongitude)
                        'TODO: Project Lat/Lon to current projection
                    End If
                Catch ex As Exception
                    Logger.Dbg("Exception trying to read EXIF: " & ex.Message)
                End Try

                If lX = 0 OrElse lY = 0 Then
                    'Place near center of current layer
                    Dim lXMax, lXMin, lYMax, lYMin As Double
                    atcMwGisUtility.GisUtil.ExtentsOfLayer(CurrentLayer, lXMax, lXMin, lYMax, lYMin)
                    lX = lXMin + (lXMax - lXMin) * (0.4 + pRandom.NextDouble / 10)
                    lY = lYMin + (lYMax - lYMin) * (0.4 + pRandom.NextDouble / 10)
                End If
                AddPoint(CurrentLayer, lX, lY)                
                'TODO: set file name (and maybe other fields) in shape DBF in new row
            Next
        End If
    End Sub

    Private Sub Form_DragEnter( _
        ByVal sender As Object, ByVal e As Windows.Forms.DragEventArgs) _
        Handles Me.DragEnter, pbxImage.DragEnter

        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            e.Effect = Windows.Forms.DragDropEffects.All
        End If
    End Sub

End Class