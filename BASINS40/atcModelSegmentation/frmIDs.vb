Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Public Class frmIDs

    Dim gSubbasinLayerName As String = ""

    Public Sub SetSubbasinsLayer(ByVal aSubbasinLayerName As String)
        gSubbasinLayerName = aSubbasinLayerName
    End Sub

    Private Sub frmIDs_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim lSubbasinLayerName As String = gSubbasinLayerName
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(lSubbasinLayerName)

        'check to see if modelseg field is on subbasins layer, add if not 
        Dim lModelSegFieldIndex As Integer = -1
        If GisUtil.IsField(lSubbasinLayerIndex, "ModelSeg") Then
            lModelSegFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, "ModelSeg")
        Else
            'need to add it
            lModelSegFieldIndex = GisUtil.AddField(lSubbasinLayerIndex, "ModelSeg", 0, 40)
        End If

        txtIDs.Text = ""
        If GisUtil.NumSelectedFeatures(lSubbasinLayerIndex) > 0 Then
            'set text in text id field
            For lIndex As Integer = 1 To GisUtil.NumSelectedFeatures(lSubbasinLayerIndex)
                If Len(txtIDs.Text) = 0 Then
                    txtIDs.Text = GisUtil.FieldValue(lSubbasinLayerIndex, GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex - 1, lSubbasinLayerIndex), lModelSegFieldIndex)
                Else
                    If txtIDs.Text <> GisUtil.FieldValue(lSubbasinLayerIndex, GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex - 1, lSubbasinLayerIndex), lModelSegFieldIndex) Then
                        'already something here, and this one is different
                        txtIDs.Text = "<multiple>"
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lSubbasinLayerName As String = gSubbasinLayerName
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(lSubbasinLayerName)
        Dim lModelSegFieldIndex As Integer = GisUtil.FieldIndex(lSubbasinLayerIndex, "ModelSeg")

        'change to hourglass cursor
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If GisUtil.NumSelectedFeatures(lSubbasinLayerIndex) > 0 Then
            'set text in dbf
            GisUtil.StartSetFeatureValue(lSubbasinLayerIndex)
            For lIndex As Integer = 1 To GisUtil.NumSelectedFeatures(lSubbasinLayerIndex)
                GisUtil.SetFeatureValueNoStartStop(lSubbasinLayerIndex, lModelSegFieldIndex, GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex - 1, lSubbasinLayerIndex), txtIDs.Text)
            Next
            GisUtil.StopSetFeatureValue(lSubbasinLayerIndex)
        End If

        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Close()
    End Sub
End Class