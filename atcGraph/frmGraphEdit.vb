Imports System.Windows.Forms

Public Class frmGraphEdit

    Event Apply()

    Private pPane As ZedGraph.GraphPane
    Private pCurveParent As TreeNode

    Public Sub Initialize(ByVal aPane As ZedGraph.GraphPane)
        pPane = aPane

        pCurveParent = treeCategories.Nodes.Add("Curve")
        Dim iCurve As Integer = 1
        For Each lCurve As ZedGraph.CurveItem In pPane.CurveList
            If lCurve.Label.Text.Length > 0 Then
                pCurveParent.Nodes.Add(lCurve.Label.Text).Tag = lCurve
            Else
                pCurveParent.Nodes.Add("Curve " & iCurve).Tag = lCurve
            End If
            iCurve += 1
        Next

        treeCategories.Nodes.Add("X Axis")
        treeCategories.Nodes.Add("Y Axis")
        treeCategories.Nodes.Add("Right Axis") 'Y2Axis
        treeCategories.Nodes.Add("Background") 'Chart.Fill
        treeCategories.Nodes.Add("Border")     'Chart.Border
        treeCategories.Nodes.Add("Legend")

    End Sub

    Private Sub treeCategories_AfterSelect(ByVal sender As System.Object, ByVal e As TreeViewEventArgs) Handles treeCategories.AfterSelect
        With treeCategories.SelectedNode
            If pCurveParent.Equals(.Parent) Then
                EditCurve(.Tag)
            Else
                Select Case .Text
                    Case "X Axis" : EditAxis(pPane.XAxis)
                    Case "Y Axis" : EditAxis(pPane.YAxis)
                    Case "Right Axis" : EditAxis(pPane.Y2Axis)
                    Case "Background" 'Chart.Fill
                    Case "Border"     'Chart.Border
                    Case "Legend"
                End Select
            End If
        End With
    End Sub

    Private Sub EditAxis(ByVal aAxis As ZedGraph.Axis)

    End Sub

    Private Sub EditCurve(ByVal aCurve As ZedGraph.CurveItem)
        panelCurveColor.Tag = aCurve
        panelCurveColor.BackColor = aCurve.Color
        chkCurveVisible.Checked = aCurve.IsVisible
        If aCurve.IsY2Axis Then
            radioCurveYRight.Checked = True
        Else
            radioCurveYLeft.Checked = True
        End If
        ShowPanel(panelCurve)
    End Sub

    Private Sub panelCurveColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelCurveColor.Click
        Dim cDlg As New ColorDialog
        cDlg.Color = panelCurveColor.BackColor
        cDlg.AnyColor = True
        cDlg.FullOpen = True
        If (cDlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            panelCurveColor.BackColor = cDlg.Color
        End If
    End Sub

    Private Sub ShowPanel(ByVal aPanel As Panel)
        For Each lPanel As Object In splitHorizontally.Panel2.Controls
            If lPanel.GetType.Equals(panelCurve.GetType) Then
                If lPanel.Equals(aPanel) Then
                    lPanel.Dock = DockStyle.Fill
                    lPanel.Visible = True
                Else
                    lPanel.Visible = False
                End If
            End If
        Next
    End Sub

    Private Sub buttonCurveApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCurveApply.Click
        Dim lCurve As ZedGraph.CurveItem = panelCurve.Tag
        lCurve.Color = panelCurveColor.BackColor
        lCurve.IsVisible = chkCurveVisible.Checked
        lCurve.IsY2Axis = radioCurveYRight.Checked
        RaiseEvent Apply()
    End Sub
End Class