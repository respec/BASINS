Imports atcMwGisUtility

Public Class frmSelectGrid

    Friend pParentForm As Object

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        pParentForm.SetGridLayerIndexForSlope(GisUtil.LayerIndex(cboGrids.Items(cboGrids.SelectedIndex)))
        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(lLayerIndex) = 4 Then
                cboGrids.Items.Add(GisUtil.LayerName(lLayerIndex))
            End If
        Next
        If cboGrids.Items.Count > 0 Then
            cboGrids.SelectedIndex = 0
        End If
    End Sub

    Public Sub InitializeForm(ByVal aParentForm As Object)
        pParentForm = aParentForm
    End Sub

End Class