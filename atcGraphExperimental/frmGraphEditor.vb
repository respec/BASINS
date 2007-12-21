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

        pPane = aPane
        cboWhichCurve.Items.Clear()
        For Each lCurve As CurveItem In pPane.CurveList
            cboWhichCurve.Items.Add(lCurve.Label.Text)
        Next
        SetControlsFromPane()
        Me.Show()
    End Sub

    Private Sub SetControlsFromPane()
        cboWhichAxis.SelectedIndex = 0
        If cboWhichCurve.Items.Count > 0 Then cboWhichCurve.SelectedIndex = 0
        SetControlsFromAxis(AxisFromCombo())
        'If pPane.CurveList.Count > 0 Then
        '    SetControlsFromCurve(pPane.CurveList(0))
        'End If
    End Sub

    Private Function AxisFromCombo() As Axis
        Select Case cboWhichAxis.SelectedIndex
            Case 0 : Return pPane.XAxis
            Case 1 : Return pPane.YAxis
            Case 2 : Return pPane.Y2Axis
            Case Else
                Throw New ApplicationException("Axis type not yet supported: " + cboWhichAxis.Text)
        End Select
    End Function

    Private Sub SetControlsFromAxis(ByVal aAxis As Axis)
        If Not aAxis Is Nothing Then
            txtAxisLabel.Text = aAxis.Title.Text
            txtAxisDisplayMinimum.Text = aAxis.Scale.Min.ToString()
            txtAxisDisplayMaximum.Text = aAxis.Scale.Max.ToString()
            Select Case aAxis.Type
                Case AxisType.DateDual
                    cboAxisType.SelectedIndex = 0
                    txtAxisDisplayMinimum.Text = XDate.ToString(aAxis.Scale.Min)
                    txtAxisDisplayMaximum.Text = XDate.ToString(aAxis.Scale.Max)
                    Exit Sub
                Case AxisType.Linear
                    cboAxisType.SelectedIndex = 1
                Case AxisType.Log
                    cboAxisType.SelectedIndex = 2
                    'case AxisType.Probability: cboAxisType.SelectedIndex = 3; break;              
            End Select
            chkAxisMajorGridVisible.Checked = aAxis.MajorGrid.IsVisible
            txtAxisMajorGridColor.BackColor = aAxis.MajorGrid.Color
            chkAxisMinorGridVisible.Checked = aAxis.MinorGrid.IsVisible
            txtAxisMinorGridColor.BackColor = aAxis.MinorGrid.Color
        End If
    End Sub

    Private Sub SetAxisFromControls(ByVal aAxis As Axis)
        If Not aAxis Is Nothing Then
            aAxis.Title.Text = txtAxisLabel.Text
            Dim lTemp As Double
            If Double.TryParse(txtAxisDisplayMinimum.Text, lTemp) Then
                aAxis.Scale.Min = lTemp
            End If
            If Double.TryParse(txtAxisDisplayMaximum.Text, lTemp) Then
                aAxis.Scale.Max = lTemp
            End If
            Dim lNewType As AxisType = AxisType.Linear
            Select Case cboAxisType.SelectedIndex
                Case 0
                    lNewType = AxisType.DateDual
                    'TODO: parse min/max date from textboxes
                    Exit Sub
                Case 1
                    lNewType = AxisType.Linear
                Case 2
                    lNewType = AxisType.Log
                    'case 3: lNewType = AxisType.Probability; break;           
            End Select
            If aAxis.Type <> lNewType Then
                aAxis.Type = lNewType
            End If
            aAxis.MajorGrid.IsVisible = chkAxisMajorGridVisible.Checked
            aAxis.MajorGrid.Color = txtAxisMajorGridColor.BackColor
            aAxis.MinorGrid.IsVisible = chkAxisMinorGridVisible.Checked
            aAxis.MinorGrid.Color = txtAxisMinorGridColor.BackColor
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
            cboCurveAxis.SelectedIndex = IIf((aCurve.IsY2Axis), 1, 0)
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
            If aCurve.IsY2Axis <> (cboCurveAxis.SelectedIndex = 1) Then
                'TODO: move curve to other axis
            End If
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

    Private Sub cboWhichAxis_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWhichAxis.SelectedIndexChanged
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
        End If
    End Sub

End Class