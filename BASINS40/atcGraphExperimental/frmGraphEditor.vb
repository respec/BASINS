Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Imports atcUtility
Imports ZedGraph

Public Class frmGraphEditor

    Public Event Apply()

    Private pPane As GraphPane
    Private WithEvents pZgc As ZedGraphControl

    ''' <summary>
    ''' Show this form for customizing the specified ZedGraphControl
    ''' </summary>
    ''' <param name="aZgc">graph control to edit</param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Sub Edit(ByVal aZgc As ZedGraphControl)
        Dim lAutoApply As Boolean = chkAutoApply.Checked
        chkAutoApply.Checked = False

        pZgc = aZgc
        pPane = aZgc.MasterPane.PaneList(aZgc.MasterPane.PaneList.Count - 1)
        SetComboFromCurves()
        SetControlsFromPane()
        Me.Show()
        chkAutoApply.Checked = lAutoApply
    End Sub

    Private Sub SetComboFromCurves()
        cboWhichCurve.Items.Clear()
        For Each lCurve As CurveItem In pPane.CurveList
            cboWhichCurve.Items.Add(lCurve.Label.Text)
        Next
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
            Select Case aAxis.Type
                Case AxisType.DateDual
                    radioAxisTime.Checked = True
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
            SetControlsMinMax(aAxis)
            chkAxisMajorGridVisible.Checked = aAxis.MajorGrid.IsVisible
            txtAxisMajorGridColor.BackColor = aAxis.MajorGrid.Color
            chkAxisMajorTicsVisible.Checked = aAxis.MajorTic.IsInside
            chkAxisMinorGridVisible.Checked = aAxis.MinorGrid.IsVisible
            txtAxisMinorGridColor.BackColor = aAxis.MinorGrid.Color
            chkAxisMinorTicsVisible.Checked = aAxis.MinorTic.IsInside
        End If
    End Sub

    Private Sub SetControlsMinMax(ByVal aAxis As Axis)
        If Not aAxis Is Nothing Then
            If radioAxisTime.Checked Then
                txtAxisDisplayMinimum.Text = XDate.ToString(aAxis.Scale.Min)
                txtAxisDisplayMaximum.Text = XDate.ToString(aAxis.Scale.Max)
            ElseIf radioAxisLinear.Checked Then
                txtAxisDisplayMinimum.Text = DoubleToString(aAxis.Scale.Min)
                txtAxisDisplayMaximum.Text = DoubleToString(aAxis.Scale.Max)
            ElseIf radioAxisLogarithmic.Checked Then
                txtAxisDisplayMinimum.Text = DoubleToString(aAxis.Scale.Min)
                txtAxisDisplayMaximum.Text = DoubleToString(aAxis.Scale.Max)
            ElseIf radioAxisProbability.Checked Then
                txtAxisDisplayMinimum.Text = DoubleToString(aAxis.Scale.Min)
                txtAxisDisplayMaximum.Text = DoubleToString(aAxis.Scale.Max)
            End If
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

    Private Sub SetControlsFromCurve(ByVal aCurve As LineItem)
        If Not aCurve Is Nothing Then
            On Error Resume Next
            txtCurveLabel.Text = aCurve.Label.Text
            If aCurve.IsY2Axis Then
                radioCurveYaxisRight.Checked = True
            Else
                radioCurveYaxisLeft.Checked = True
            End If

            txtCurveColor.BackColor = aCurve.Color
            chkCurveLineVisible.Checked = aCurve.Line.IsVisible
            txtCurveWidth.Text = aCurve.Line.Width.ToString()

            chkCurveSymbolVisible.Checked = aCurve.Symbol.IsVisible
            txtCurveSymbolSize.Text = aCurve.Symbol.Size
            cboCurveSymbolType.Text = System.Enum.GetName(aCurve.Symbol.GetType, aCurve.Symbol)
            'Select Case aCurve.Symbol.Type
            '    Case SymbolType.Square : cboCurveSymbolType.Text = "Square"
            '    Case SymbolType.Diamond : cboCurveSymbolType.Text = "Diamond"
            '    Case SymbolType.Triangle : cboCurveSymbolType.Text = "Triangle"
            '    Case SymbolType.Circle : cboCurveSymbolType.Text = "3"
            '    Case SymbolType.XCross : cboCurveSymbolType.Text = "4"
            '    Case SymbolType.Plus : cboCurveSymbolType.Text = "5"
            '    Case SymbolType.Star : cboCurveSymbolType.Text = "6"
            '    Case SymbolType.TriangleDown : cboCurveSymbolType.Text = "7"
            '    Case SymbolType.HDash : cboCurveSymbolType.Text = "8"
            '    Case SymbolType.VDash : cboCurveSymbolType.Text = "9"
            '    Case SymbolType.None : cboCurveSymbolType.Text = "10"
            'End Select
        End If
    End Sub

    Private Sub SetCurveFromControls(ByVal aCurve As LineItem)
        If Not aCurve Is Nothing Then
            On Error Resume Next
            aCurve.Label.Text = txtCurveLabel.Text
            aCurve.IsY2Axis = radioCurveYaxisRight.Checked
            'TODO: If radioCurveYaxisAuxiliary.Checked then [move to aux pane]
            aCurve.Color = txtCurveColor.BackColor
            Dim lInt As Integer
            If Integer.TryParse(txtCurveWidth.Text, lInt) Then aCurve.Line.Width = lInt

            aCurve.Symbol.IsVisible = chkCurveSymbolVisible.Checked
            If Integer.TryParse(txtCurveSymbolSize.Text, lInt) Then aCurve.Symbol.Size = lInt

            aCurve.Symbol.Type = System.Enum.Parse(aCurve.Symbol.Type.GetType, cboCurveSymbolType.Text)
            'Select Case cboCurveSymbolType.Text
            '    Case "Square" : aCurve.Symbol.Type = SymbolType.Square
            '    Case "Diamond" : aCurve.Symbol.Type = SymbolType.Diamond
            '    Case "Triangle" : aCurve.Symbol.Type = SymbolType.Triangle
            '    Case "Circle" : aCurve.Symbol.Type = SymbolType.Circle
            '    Case "XCross" : aCurve.Symbol.Type = SymbolType.XCross
            '    Case "Plus" : aCurve.Symbol.Type = SymbolType.Plus
            '    Case "Star" : aCurve.Symbol.Type = SymbolType.Star
            '    Case "TriangleDown" : aCurve.Symbol.Type = SymbolType.TriangleDown
            '    Case "HDash" : aCurve.Symbol.Type = SymbolType.HDash
            '    Case "VDash" : aCurve.Symbol.Type = SymbolType.VDash
            '    Case "None" : aCurve.Symbol.Type = SymbolType.None
            'End Select
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
            txtAxisLabel.TextChanged, txtCurveLabel.TextChanged, txtCurveWidth.TextChanged, txtCurveSymbolSize.TextChanged
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

    Private Sub pZgc_ZoomEvent(ByVal sender As ZedGraph.ZedGraphControl, ByVal oldState As ZedGraph.ZoomState, ByVal newState As ZedGraph.ZoomState) Handles pZgc.ZoomEvent
        SetControlsMinMax(AxisFromCombo())
    End Sub

    Private Sub btnLineEquationAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLineEquationAdd.Click
        Dim lNewLine As LineItem = AddLine(pPane, txtLineAcoef.Text, txtLineBcoef.Text, Drawing2D.DashStyle.Solid, "Y = " & txtLineAcoef.Text & " X + " & txtLineBcoef.Text)
        SetComboFromCurves()
    End Sub

    Private Sub btnLineConstantYAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLineConstantYAdd.Click
        Dim lNewLine As LineItem = AddLine(pPane, 0, txtLineYconstant.Text, Drawing2D.DashStyle.Solid, "Y = " & txtLineYconstant.Text)
        SetComboFromCurves()
    End Sub
End Class