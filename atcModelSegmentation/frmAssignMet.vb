Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Public Class frmAssignMet

    Private Sub frmAssignMet_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim lLayerIndex As Integer
        Dim lDefaultLayerIndex As Integer = -1
        Dim lLayerName As String

        For lLayerIndex = 0 To GisUtil.NumLayers() - 1
            lLayerName = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 1 Then
                'This is a Point Shapefile 
                cboMetStations.Items.Add(lLayerName)
                If GisUtil.CurrentLayer = lLayerIndex Then
                    'this layer is selected, default to it
                    lDefaultLayerIndex = cboMetStations.Items.Count - 1
                ElseIf (InStr(lLayerName, "Weather Station Sites") > 0 Or InStr(lLayerName, "Met Stations") > 0) And lDefaultLayerIndex < 0 Then
                    'this looks like a reasonable default layer
                    lDefaultLayerIndex = cboMetStations.Items.Count - 1
                End If
            End If
        Next

        If lDefaultLayerIndex > -1 Then
            'have a default layer
            cboMetStations.SelectedIndex = lDefaultLayerIndex
        End If
        If cboMetStations.Items.Count > 0 And cboMetStations.SelectedIndex < 0 Then
            'default to first layer if nothing more fitting has been found
            cboMetStations.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmdAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAssign.Click

        Dim lMetLayerName As String = cboMetStations.Items(cboMetStations.SelectedIndex)
        Dim lMetLayerIndex As String = GisUtil.LayerIndex(lMetLayerName)

        If cbxUseSelected.Checked And GisUtil.NumSelectedFeatures(lMetLayerIndex) = 0 Then
            'nothing selected in specified layer, let user know this is a problem
            MsgBox("Nothing is selected in layer '" & lMetLayerName & "'.", MsgBoxStyle.Information, "Assign Met Segments Problem")
            Exit Sub
        End If

    End Sub
End Class