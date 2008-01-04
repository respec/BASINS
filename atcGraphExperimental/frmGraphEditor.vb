Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports ZedGraph

Public Class frmGraphEditor

    Public Event Apply()
    Private pPane As GraphPane

    ''' <summary>
    ''' Show this form for editing the specified GraphPane
    ''' </summary>
    ''' <param name="aPane">pane to edit</param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Sub Edit(ByVal aPane As GraphPane)
        Dim lAutoApply As Boolean = chkAutoApply.Checked
        chkAutoApply.Checked = False

        pPane = aPane
        cboWhichCurve.Items.Clear()
        For Each lCurve As CurveItem In pPane.CurveList
            cboWhichCurve.Items.Add(lCurve.Label.Text)
        Next
        SetControlsFromPane()
        Me.Show()
        chkAutoApply.Checked = lAutoApply
    End Sub

    Private Sub SetControlsFromPane()
        If cboWhichCurve.Items.Count > 0 Then cboWhichCurve.SelectedIndex = 0
        SetControlsFromAxis(AxisFromCombo())
    End Sub

    Private Function AxisFromCombo() As Axis
        If Not pPane Is Nothing Then
            If radioAxisBottom.Checked Then
                Return pPane.XAxis
            ElseIf radioAxisLeft.Checked Then
                Return pPane.YAxis
            ElseIf radioAxisRight.Checked Then
                Return pPane.Y2Axis
            ElseIf radioAxisAux.Checked Then
                'TODO: find aux axis
            End If
        End If
        Return Nothing
    End Function

    Private Sub SetControlsFromAxis(ByVal aAxis As Axis)
        If Not aAxis Is Nothing Then
            txtAxisLabel.Text = aAxis.Title.Text
            txtAxisDisplayMinimum.Text = aAxis.Scale.Min.ToString()
            txtAxisDisplayMaximum.Text = aAxis.Scale.Max.ToString()
            Select Case aAxis.Type
                Case AxisType.DateDual
                    radioAxisTime.Checked = True
                    txtAxisDisplayMinimum.Text = XDate.ToString(aAxis.Scale.Min)
                    txtAxisDisplayMaximum.Text = XDate.ToString(aAxis.Scale.Max)
                Case AxisType.Linear
                    radioAxisLinear.Checked = True
                    radioAxisLogarithmic.Enabled = True
                    radioAxisLinear.Enabled = True
                Case AxisType.Log
                    radioAxisLogarithmic.Checked = True
                    radioAxisLogarithmic.Enabled = True
                    radioAxisLinear.Enabled = True
                Case AxisType.Probability
                    radioAxisProbability.Checked = True
            End Select
            chkAxisMajorGridVisible.Checked = aAxis.MajorGrid.IsVisible
            txtAxisMajorGridColor.BackColor = aAxis.MajorGrid.Color
            chkAxisMajorTicsVisible.Checked = aAxis.MajorTic.IsInside
            chkAxisMinorGridVisible.Checked = aAxis.MinorGrid.IsVisible
            txtAxisMinorGridColor.BackColor = aAxis.MinorGrid.Color
            chkAxisMinorTicsVisible.Checked = aAxis.MinorTic.IsInside
        End If
    End Sub

    Private Sub SetAxisFromControls(ByVal aAxis As Axis)
        If Not aAxis Is Nothing Then
            With aAxis
                If radioAxisTime.Checked Then
                    'TODO: parse min/max date from textboxes
                ElseIf radioAxisLinear.Checked Then
                    If aAxis.Type <> AxisType.Linear Then aAxis.Type = AxisType.Linear
                ElseIf radioAxisLogarithmic.Checked Then
                    If aAxis.Type <> AxisType.Log Then aAxis.Type = AxisType.Log
                ElseIf radioAxisProbability.Checked Then
                    'TODO:?           
                End If
                Dim lTemp As Double
                If Double.TryParse(txtAxisDisplayMinimum.Text, lTemp) Then
                    aAxis.Scale.Min = lTemp
                End If
                If Double.TryParse(txtAxisDisplayMaximum.Text, lTemp) Then
                    aAxis.Scale.Max = lTemp
                End If
                aAxis.Title.Text = txtAxisLabel.Text
                aAxis.MajorGrid.IsVisible = chkAxisMajorGridVisible.Checked
                aAxis.MajorGrid.Color = txtAxisMajorGridColor.BackColor
                aAxis.MajorTic.IsInside = chkAxisMajorTicsVisible.Checked
                aAxis.MinorGrid.IsVisible = chkAxisMinorGridVisible.Checked
                aAxis.MinorGrid.Color = txtAxisMinorGridColor.BackColor
                aAxis.MinorTic.IsInside = chkAxisMinorTicsVisible.Checked
            End With
        End If
    End Sub

    Private Sub SetControlsFromSelectedCurve()
        If cboWhichCurve.SelectedIndex >= 0 AndAlso Not pPane Is Nothing AndAlso cboWhichCurve.SelectedIndex < pPane.CurveList.Count Then
            SetControlsFromCurve(pPane.CurveList(cboWhichCurve.SelectedIndex))
        End If
    End Sub

    Private Sub SetControlsFromCurve(ByVal aCurve As CurveItem)
        If Not aCurve Is Nothing Then
            txtCurveLabel.Text = aCurve.Label.Text
            If aCurve.IsY2Axis Then
                radioCurveYaxisRight.Checked = True
            Else
                radioCurveYaxisLeft.Checked = True
            End If

            txtCurveColor.BackColor = aCurve.Color
            If TypeOf aCurve Is LineItem Then
                lblCurve.Visible = True
                txtCurveWidth.Visible = True

                txtCurveWidth.Text = (CType(aCurve, LineItem)).Line.Width.ToString()
            Else
                lblCurve.Visible = False
                txtCurveWidth.Visible = False
            End If
        End If
    End Sub

    Private Sub SetCurveFromControls(ByVal aCurve As CurveItem)
        If Not aCurve Is Nothing Then
            aCurve.Label.Text = txtCurveLabel.Text
            aCurve.IsY2Axis = radioCurveYaxisRight.Checked
            'TODO: If radioCurveYaxisAuxiliary.Checked then [move to aux pane]
            aCurve.Color = txtCurveColor.BackColor
            If TypeOf aCurve Is LineItem Then
                Dim lWidth As Integer
                If Integer.TryParse(txtCurveWidth.Text, lWidth) Then
                    CType(aCurve, LineItem).Line.Width = lWidth
                End If
            End If
        End If
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        SetAxisFromControls(AxisFromCombo())
        If cboWhichCurve.SelectedIndex >= 0 Then
            SetCurveFromControls(pPane.CurveList(cboWhichCurve.SelectedIndex))
        End If
        RaiseEvent Apply()
    End Sub

    Private Sub cboWhichAxis_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetControlsFromSelectedCurve()
    End Sub

    Private Sub txtColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles txtAxisMinorGridColor.Click, txtAxisMajorGridColor.Click, txtCurveColor.Click
        ChooseTextBoxBackColor(sender)
    End Sub

    Private Sub ChooseTextBoxBackColor(ByVal aTextBox As TextBox)
        Dim lColorDialog As ColorDialog = New ColorDialog()
        lColorDialog.Color = aTextBox.BackColor
        If lColorDialog.ShowDialog() = DialogResult.OK Then
            aTextBox.BackColor = lColorDialog.Color
            If chkAutoApply.Checked Then btnApply_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub chkAny_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        chkAxisMajorGridVisible.CheckedChanged, _
        chkAxisMajorTicsVisible.CheckedChanged, _
        chkAxisMinorGridVisible.CheckedChanged, _
        chkAxisMinorTicsVisible.CheckedChanged

        If chkAutoApply.Checked Then btnApply_Click(Nothing, Nothing)
    End Sub

    Private Sub txtNotColor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            txtAxisLabel.TextChanged, txtCurveLabel.TextChanged, txtCurveWidth.TextChanged
        If chkAutoApply.Checked Then btnApply_Click(Nothing, Nothing)
    End Sub

    Private Sub radioAxisLocation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            radioAxisBottom.CheckedChanged, radioAxisLeft.CheckedChanged, radioAxisRight.CheckedChanged, radioAxisAux.CheckedChanged
        SetControlsFromAxis(AxisFromCombo())
    End Sub

    Private Sub radioGeneral_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            radioAxisLinear.CheckedChanged, radioAxisLogarithmic.CheckedChanged, _
            radioCurveYaxisLeft.CheckedChanged, radioCurveYaxisRight.CheckedChanged, radioCurveYaxisAuxiliary.CheckedChanged

        If chkAutoApply.Checked Then
            Dim lRadio As RadioButton = sender
            If lRadio.Checked Then 'Only apply for newly checked, not also for unchecked
                btnApply_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub cboWhichCurve_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWhichCurve.SelectedIndexChanged
        If cboWhichCurve.SelectedIndex >= 0 AndAlso pPane.CurveList.Count > cboWhichCurve.SelectedIndex Then
            SetControlsFromCurve(pPane.CurveList(cboWhichCurve.SelectedIndex))
        End If
    End Sub

    Private Sub cboCurveAxis_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If chkAutoApply.Checked Then btnApply_Click(Nothing, Nothing)
    End Sub

End Class