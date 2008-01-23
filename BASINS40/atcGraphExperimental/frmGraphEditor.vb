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
        SetComboFromTexts()
        SetControlsFromPane()
        Me.Show()
        chkAutoApply.Checked = lAutoApply
    End Sub

    Private Sub SetComboFromCurves()
        comboWhichCurve.Items.Clear()
        For Each lCurve As CurveItem In pPane.CurveList
            comboWhichCurve.Items.Add(lCurve.Label.Text)
        Next
    End Sub

    Private Sub SetComboFromTexts()
        For Each lItem As ZedGraph.GraphObj In pPane.GraphObjList
            If lItem.GetType.Name = "TextObj" Then
                Dim lText As TextObj = lItem
                comboWhichText.Items.Add(lText.Text)
            End If
        Next
    End Sub

    Private Sub SetControlsFromPane()
        If comboWhichCurve.Items.Count > 0 Then comboWhichCurve.SelectedIndex = 0
        SetControlsFromAxis(AxisFromCombo())
        Select Case pPane.XAxis.Type
            Case AxisType.Exponent, AxisType.Linear, AxisType.Log
                grpLineEquation.Visible = True
            Case Else
                grpLineEquation.Visible = False
        End Select
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
        If comboWhichCurve.SelectedIndex >= 0 AndAlso Not pPane Is Nothing AndAlso comboWhichCurve.SelectedIndex < pPane.CurveList.Count Then
            SetControlsFromCurve(pPane.CurveList(comboWhichCurve.SelectedIndex))
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
            Dim lInt As Integer
            aCurve.Label.Text = txtCurveLabel.Text
            aCurve.IsY2Axis = radioCurveYaxisRight.Checked
            'TODO: If radioCurveYaxisAuxiliary.Checked then [move to aux pane]

            aCurve.Color = txtCurveColor.BackColor
            aCurve.Line.IsVisible = chkCurveLineVisible.Checked
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

    Private Sub SetLegendFromControls()
        With pPane.Legend
            .IsVisible = comboLegendLocation.Text <> "None"
            Select Case comboLegendLocation.Text
                Case "Bottom" : .Position = LegendPos.Bottom
                Case "BottomCenter" : .Position = LegendPos.BottomCenter
                Case "BottomFlushLeft" : .Position = LegendPos.BottomFlushLeft
                Case "Float" : .Position = LegendPos.Float
                Case "InsideBotLeft" : .Position = LegendPos.InsideBotLeft
                Case "InsideBotRight" : .Position = LegendPos.InsideBotRight
                Case "InsideTopLeft" : .Position = LegendPos.InsideTopLeft
                Case "InsideTopRight" : .Position = LegendPos.InsideTopRight
                Case "Left" : .Position = LegendPos.Left
                Case "Right" : .Position = LegendPos.Right
                Case "Top" : .Position = LegendPos.Top
                Case "TopCenter" : .Position = LegendPos.TopCenter
                Case "TopFlushLeft" : .Position = LegendPos.TopFlushLeft
            End Select
        End With
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        SetAxisFromControls(AxisFromCombo())
        If comboWhichCurve.SelectedIndex >= 0 Then
            SetCurveFromControls(pPane.CurveList(comboWhichCurve.SelectedIndex))
            SetComboFromCurves()
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
        chkAxisMinorTicsVisible.CheckedChanged, _
        chkCurveLineVisible.CheckedChanged, _
        chkCurveSymbolVisible.CheckedChanged

        If chkAutoApply.Checked Then btnApply_Click(Nothing, Nothing)
    End Sub

    Private Sub txtLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            txtAxisLabel.TextChanged, txtCurveLabel.TextChanged
        If chkAutoApply.Checked Then btnApply_Click(Nothing, Nothing)
    End Sub

    Private Sub txtNumeric_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
              txtCurveWidth.TextChanged, txtCurveSymbolSize.TextChanged
        If chkAutoApply.Checked AndAlso IsNumeric(sender.Text) Then btnApply_Click(Nothing, Nothing)
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

    Private Sub comboWhichCurve_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboWhichCurve.SelectedIndexChanged
        If comboWhichCurve.SelectedIndex >= 0 AndAlso pPane.CurveList.Count > comboWhichCurve.SelectedIndex Then
            SetControlsFromCurve(pPane.CurveList(comboWhichCurve.SelectedIndex))
        End If
    End Sub

    Private Sub cboCurveAxis_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If chkAutoApply.Checked Then btnApply_Click(Nothing, Nothing)
    End Sub

    Private Sub pZgc_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pZgc.MouseClick
        Select Case tabsCategory.SelectedTab.Text
            Case "Legend"
                With pPane.Legend
                    .Position = LegendPos.Float
                    .Location = New Location(e.X / pZgc.Width, e.Y / pZgc.Height, CoordType.PaneFraction)
                    .IsVisible = True
                End With
                comboLegendLocation.Text = "Float"
                RaiseEvent Apply()
        End Select
    End Sub

    Private Sub pZgc_ZoomEvent(ByVal sender As ZedGraph.ZedGraphControl, ByVal oldState As ZedGraph.ZoomState, ByVal newState As ZedGraph.ZoomState) Handles pZgc.ZoomEvent
        SetControlsMinMax(AxisFromCombo())
    End Sub

    Private Sub btnLineEquationAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLineEquationAdd.Click
        Dim lNewLine As LineItem = AddLine(pPane, txtLineAcoef.Text, txtLineBcoef.Text, Drawing2D.DashStyle.Solid, "Y = " & txtLineAcoef.Text & " X + " & txtLineBcoef.Text)
        SetComboFromCurves()
        RaiseEvent Apply()
    End Sub

    Private Sub btnLineConstantYAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLineConstantYAdd.Click
        Dim lNewLine As LineItem = AddLine(pPane, 0, txtLineYconstant.Text, Drawing2D.DashStyle.Solid, "Y = " & txtLineYconstant.Text)
        SetComboFromCurves()
        tabCurves.Select()
        RaiseEvent Apply()
    End Sub

    Private Sub cboCurveSymbolType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCurveSymbolType.SelectedIndexChanged
        If chkAutoApply.Checked Then btnApply_Click(Nothing, Nothing)
    End Sub

    Private Sub txtAxisDisplayMinimum_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAxisDisplayMinimum.TextChanged
        If chkAutoApply.Checked AndAlso IsNumeric(txtAxisDisplayMinimum.Text) Then
            Dim lTemp As Double
            If Double.TryParse(txtAxisDisplayMinimum.Text, lTemp) Then
                Dim lAxis As Axis = AxisFromCombo()
                If Not lAxis Is Nothing AndAlso lAxis.Scale.Min <> lTemp AndAlso lTemp < lAxis.Scale.Max Then
                    lAxis.Scale.Min = lTemp
                    RaiseEvent Apply()
                End If
            End If
        End If
    End Sub

    Private Sub txtAxisDisplayMaximum_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAxisDisplayMaximum.TextChanged
        If chkAutoApply.Checked AndAlso IsNumeric(txtAxisDisplayMaximum.Text) Then
            Dim lTemp As Double
            If Double.TryParse(txtAxisDisplayMaximum.Text, lTemp) Then
                Dim lAxis As Axis = AxisFromCombo()
                If Not lAxis Is Nothing AndAlso lAxis.Scale.Max <> lTemp AndAlso lTemp > lAxis.Scale.Min Then
                    lAxis.Scale.Max = lTemp
                    RaiseEvent Apply()
                End If
            End If
        End If
    End Sub

    Private Sub comboLegendLocation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboLegendLocation.SelectedIndexChanged
        If chkAutoApply.Checked Then
            SetLegendFromControls()
            RaiseEvent Apply()
        End If
    End Sub

    Private Sub btnTextAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextAdd.Click
        Dim lText As New TextObj("Confidential", 0.85, -0.03)
        lText.Location.CoordinateFrame = CoordType.PaneFraction
        '// rotate the text 15 degrees
        'text.FontSpec.Angle = 15.0F;
        '// Text will be red, bold, and 16 point
        'text.FontSpec.FontColor = Color.Red;
        'text.FontSpec.IsBold = true;
        'text.FontSpec.Size = 16;
        'Disable the border and background fill options for the text
        lText.FontSpec.Border.IsVisible = False
        lText.FontSpec.Fill.IsVisible = False
        lText.Location.AlignH = AlignH.Left
        lText.Location.AlignV = AlignV.Top
        pPane.GraphObjList.Add(lText)
    End Sub

    Private Sub comboWhichText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboWhichText.SelectedIndexChanged
        txtText.Text = comboWhichText.Text
    End Sub
End Class