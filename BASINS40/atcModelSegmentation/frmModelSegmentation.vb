Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Public Class frmModelSegmentation

    Private Sub cmdAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAssign.Click
        Dim frmAssign As New frmAssignMet
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
End Class