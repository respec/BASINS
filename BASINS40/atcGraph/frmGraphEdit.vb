Imports System.Windows.Forms
Imports ZedGraph

Public Class frmGraphEdit

    Event Apply()

    Private pPane As GraphPane
    Private pCurveParent As TreeNode

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal aPane As GraphPane)
        pPane = aPane

        pCurveParent = treeCategories.Nodes.Add("Curve")
        Dim iCurve As Integer = 1
        For Each lCurve As CurveItem In pPane.CurveList
            If lCurve.Label.Text.Length > 0 Then
                pCurveParent.Nodes.Add(lCurve.Label.Text).Tag = lCurve
            Else
                pCurveParent.Nodes.Add("Curve " & iCurve).Tag = lCurve
            End If
            iCurve += 1
        Next
        pCurveParent.Expand()

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

    Private Sub EditAxis(ByVal aAxis As Axis)
        panelAxis.Tag = aAxis
        panelAxisColor.BackColor = aAxis.Color
        checkAxisVisible.Checked = aAxis.IsVisible
        ShowPanel(panelAxis)
    End Sub

    Private Sub EditCurve(ByVal aCurve As CurveItem)
        panelCurve.Tag = aCurve
        panelCurveColor.BackColor = aCurve.Color
        checkCurveVisible.Checked = aCurve.IsVisible
        If aCurve.IsY2Axis Then
            radioCurveYRight.Checked = True
        Else
            radioCurveYLeft.Checked = True
        End If
        radioCurveLineStep.Checked = aCurve.IsLine
        radioCurveBar.Checked = aCurve.IsBar
        'aCurve.IsPie
        ShowPanel(panelCurve)
    End Sub

    Private Sub panelCurveColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelCurveColor.Click
        Dim cDlg As New ColorDialog
        cDlg.Color = panelCurveColor.BackColor
        cDlg.AnyColor = True
        cDlg.FullOpen = True
        If (cDlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            panelCurveColor.BackColor = cDlg.Color
            If checkApply.Checked Then ApplyCurveEdits()
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

    Private Sub buttonCurveApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ApplyCurveEdits()
    End Sub

    Private Sub ApplyCurveEdits()

        Dim lCurve As CurveItem = panelCurve.Tag
        If Not lCurve Is Nothing Then
            Dim lCurveIndex As Integer = pPane.CurveList.IndexOf(lCurve)

            If radioCurveLineStep.Checked OrElse radioCurveLineNoStep.Checked OrElse radioCurveLineForward.Checked Then
                Dim lNew As LineItem
                If lCurve.IsLine Then 'Curve is already a line
                    lNew = lCurve
                Else                  'Change curve into a line
                    pPane.CurveList.RemoveAt(lCurveIndex)
                    lNew = New LineItem(lCurve.Label.Text, lCurve.Points, lCurve.Color, SymbolType.None)
                    pPane.CurveList.Insert(lCurveIndex, lNew)
                    panelCurve.Tag = lNew
                End If
                If radioCurveLineStep.Checked And lNew.Line.StepType <> StepType.RearwardStep Then
                    lNew.Line.StepType = StepType.RearwardStep
                End If
                If radioCurveLineNoStep.Checked And lNew.Line.StepType <> StepType.NonStep Then
                    lNew.Line.StepType = StepType.NonStep
                End If
                If radioCurveLineForward.Checked And lNew.Line.StepType <> StepType.ForwardStep Then
                    lNew.Line.StepType = StepType.ForwardStep
                End If
            End If

            If radioCurveBar.Checked Then
                Dim lNew As BarItem
                If lCurve.IsBar Then 'Curve is already a bar
                    lNew = lCurve
                Else                 'Change curve into bar
                    pPane.CurveList.RemoveAt(lCurveIndex)
                    lNew = New BarItem(lCurve.Label.Text, lCurve.Points, lCurve.Color)
                    pPane.CurveList.Insert(lCurveIndex, lNew)
                    panelCurve.Tag = lNew
                End If
            End If

            If Not lCurve.Color.Equals(panelCurveColor.BackColor) Then
                lCurve.Color = panelCurveColor.BackColor
            End If

            If Not lCurve.IsVisible = checkCurveVisible.Checked Then
                lCurve.IsVisible = checkCurveVisible.Checked
            End If

            If Not lCurve.IsY2Axis = radioCurveYRight.Checked Then
                lCurve.IsY2Axis = radioCurveYRight.Checked
            End If

            RaiseEvent Apply()
        End If
    End Sub

    Private Sub checkCurveVisible_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkCurveVisible.CheckedChanged
        If checkApply.Checked Then ApplyCurveEdits()
    End Sub

    Private Sub radioCurveY_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioCurveYLeft.CheckedChanged, radioCurveYRight.CheckedChanged
        If checkApply.Checked Then ApplyCurveEdits()
    End Sub

    Private Sub radioCurveStyle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioCurveLineStep.CheckedChanged, radioCurveBar.CheckedChanged
        If checkApply.Checked Then ApplyCurveEdits()
    End Sub

    Private Sub buttonAxisApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonAxisApply.Click
        ApplyAxisEdits()
    End Sub

    Private Sub ApplyAxisEdits()

        Dim lAxis As Axis = panelAxis.Tag
        If Not lAxis Is Nothing Then

            If Not lAxis.Color.Equals(panelAxisColor.BackColor) Then
                lAxis.Color = panelAxisColor.BackColor
            End If

            If Not lAxis.IsVisible = checkCurveVisible.Checked Then
                lAxis.IsVisible = checkCurveVisible.Checked
            End If

            RaiseEvent Apply()
        End If
    End Sub

    Private Sub panelAxisColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelAxisColor.Click
        Dim cDlg As New ColorDialog
        cDlg.Color = panelAxisColor.BackColor
        cDlg.AnyColor = True
        cDlg.FullOpen = True
        If (cDlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            panelAxisColor.BackColor = cDlg.Color
            If checkApply.Checked Then ApplyCurveEdits()
        End If
    End Sub
End Class