Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Public Class frmModelSegmentation

    Private Sub cmdAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAssign.Click
        Dim frmAssign As New frmAssignMet
        frmAssign.SetSubbasinsLayer(cboSubbasins.Items(cboSubbasins.SelectedIndex))
        frmAssign.Show()
    End Sub

    Private Sub frmModelSegmentation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim lLayerIndex As Integer
        Dim lDefaultLayerIndex As Integer = -1
        Dim lLayerName As String

        For lLayerIndex = 0 To GisUtil.NumLayers() - 1
            lLayerName = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 3 Then
                'This is a Polygon Shapefile 
                cboSubbasins.Items.Add(lLayerName)
                If GisUtil.CurrentLayer = lLayerIndex Then
                    'this layer is selected, default to it
                    lDefaultLayerIndex = cboSubbasins.Items.Count - 1
                ElseIf (UCase(lLayerName) = "SUBBASINS" Or InStr(lLayerName, "Watershed Shapefile") > 0) And lDefaultLayerIndex < 0 Then
                    'this looks like a reasonable default layer
                    lDefaultLayerIndex = cboSubbasins.Items.Count - 1
                End If
            End If
        Next

        If lDefaultLayerIndex > -1 Then
            'have a default layer
            cboSubbasins.SelectedIndex = lDefaultLayerIndex
        End If
        If cboSubbasins.Items.Count > 0 And cboSubbasins.SelectedIndex < 0 Then
            'default to first layer if nothing more fitting has been found
            cboSubbasins.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmdInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInput.Click
        Dim lSubbasinLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(lSubbasinLayerName)

        If GisUtil.NumSelectedFeatures(lSubbasinLayerIndex) = 0 Then
            'nothing selected in specified layer, let user know this is a problem
            MsgBox("Nothing is selected in layer '" & lSubbasinLayerName & "'.", MsgBoxStyle.Information, "Input Segment IDs for Selected Problem")
        Else
            Dim frmIDs As New frmIDs
            frmIDs.SetSubbasinsLayer(lSubbasinLayerName)
            frmIDs.Show()
        End If
    End Sub

    Private Sub cmdDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDisplay.Click
        Dim lSubbasinLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(lSubbasinLayerName)

        If Not GisUtil.IsField(lSubbasinLayerIndex, "ModelSeg") Then
            'can't do themeatic display without a modelseg field
            MsgBox("Cannot display thematic map until model segments have been defined.", MsgBoxStyle.Information, "Display Segments Themeatically Problem")
            Exit Sub
        End If

        Dim lModelSegFieldIndex As Integer = -1
        lModelSegFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, "ModelSeg")

        'save original coloring scheme in case we want to return to it
        Dim lColoringScheme As Object = GisUtil.GetColoringScheme(lSubbasinLayerIndex)
        'do the renderer
        GisUtil.UniqueValuesRenderer(lSubbasinLayerIndex, lModelSegFieldIndex)
        If MsgBox("Do you want to keep this thematic map?", MsgBoxStyle.OkCancel, "Display Segments Themeatically") = MsgBoxResult.Cancel Then
            'revert to original renderer
            GisUtil.ColoringScheme(lSubbasinLayerIndex) = lColoringScheme
        End If

    End Sub
End Class