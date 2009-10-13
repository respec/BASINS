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
    Private pDateFormat As atcDateFormat

    ''' <summary>
    ''' Show this form for customizing the specified ZedGraphControl
    ''' </summary>
    ''' <param name="aZgc">graph control to edit</param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Sub Edit(ByVal aZgc As ZedGraphControl)
        Dim lAutoApply As Boolean = chkAutoApply.Checked
        chkAutoApply.Checked = False

        If pDateFormat Is Nothing Then
            pDateFormat = New atcDateFormat
            With pDateFormat
                .IncludeHours = False
                .IncludeMinutes = False
                .IncludeSeconds = False
            End With
        End If

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
            If lCurve.Label.Text.Length > 0 Then
                comboWhichCurve.Items.Add(lCurve.Label.Text)
            ElseIf Not lCurve.Tag Is Nothing Then
                comboWhichCurve.Items.Add(lCurve.Tag)
            Else
                comboWhichCurve.Items.Add("curve " & comboWhichCurve.Items.Count + 1)
            End If
        Next
    End Sub

    Private Sub SetComboFromTexts()
        comboWhichText.Items.Clear()
        comboWhichText.Text = ""
        For Each lItem As ZedGraph.GraphObj In pPane.GraphObjList
            If lItem.GetType.Name = "TextObj" Then
                Dim lText As TextObj = lItem
                comboWhichText.Items.Add(lText.Text)
            End If
        Next
        If comboWhichText.Items.Count > 0 Then
            comboWhichText.SelectedIndex = comboWhichText.Items.Count - 1
        End If
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
        chkLegendOutline.Checked = pPane.Legend.Border.IsVisible
        txtLegendFontColor.BackColor = pPane.Legend.FontSpec.FontColor
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
            radioAxisTime.Enabled = False
            radioAxisLinear.Enabled = False
            radioAxisLogarithmic.Enabled = False
            radioAxisProbability.Enabled = False
            Select Case aAxis.Type
                Case AxisType.DateDual
                    radioAxisTime.Checked = True
                Case AxisType.Linear
                    radioAxisLinear.Enabled = True
                    radioAxisLogarithmic.Enabled = True
                    radioAxisLinear.Checked = True
                Case AxisType.Log
                    radioAxisLinear.Enabled = True
                    radioAxisLogarithmic.Enabled = True
                    radioAxisLogarithmic.Checked = True
                Case AxisType.Probability
                    radioAxisProbability.Checked = True
                    panelAxisType.Visible = False
                    panelProbability.Visible = True
                    Dim lProbScale As ZedGraph.ProbabilityScale = aAxis.Scale
                    Select Case lProbScale.LabelStyle
                        Case ProbabilityScale.ProbabilityLabelStyle.Percent
                            radioProbablilityPercent.Checked = True
                        Case ProbabilityScale.ProbabilityLabelStyle.Fraction
                            radioProbablilityFraction.Checked = True
                        Case ProbabilityScale.ProbabilityLabelStyle.ReturnInterval
                            radioProbablilityReturnPeriod.Checked = True
                    End Select
                    txtProbabilityDeviations.Text = DoubleToString(lProbScale.standardDeviations)
            End Select
            SetControlsMinMax(aAxis)
            chkRangeReverse.Checked = aAxis.Scale.IsReverse
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
                txtAxisDisplayMinimum.Text = pDateFormat.JDateToString(aAxis.Scale.Min)
                txtAxisDisplayMaximum.Text = pDateFormat.JDateToString(aAxis.Scale.Max)
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
        On Error Resume Next 'Set whatever we can legally set
        If Not aAxis Is Nothing Then
            With aAxis
                Dim lTemp As Double
                If radioAxisTime.Checked Then
                    'parse min/max date from textboxes
                    aAxis.Scale.Min = StringToJdate(txtAxisDisplayMinimum.Text, True)
                    aAxis.Scale.Max = StringToJdate(txtAxisDisplayMaximum.Text, False)
                ElseIf radioAxisLinear.Checked Then
                    If aAxis.Type <> AxisType.Linear Then .Type = AxisType.Linear
                ElseIf radioAxisLogarithmic.Checked Then
                    If aAxis.Type <> AxisType.Log Then .Type = AxisType.Log
                ElseIf radioAxisProbability.Checked Then
                    Dim lProbScale As ZedGraph.ProbabilityScale = aAxis.Scale
                    If radioProbablilityPercent.Checked Then
                        lProbScale.LabelStyle = ProbabilityScale.ProbabilityLabelStyle.Percent
                        .Title.Text = ReplaceStringNoCase(.Title.Text, "Fraction", "Percent")
                        .Title.Text = ReplaceStringNoCase(.Title.Text, "Return Interval", "Percent")
                    ElseIf radioProbablilityFraction.Checked Then
                        lProbScale.LabelStyle = ProbabilityScale.ProbabilityLabelStyle.Fraction
                        .Title.Text = ReplaceStringNoCase(.Title.Text, "Percent", "Fraction")
                        .Title.Text = ReplaceStringNoCase(.Title.Text, "Return Interval", "Fraction")
                    ElseIf radioProbablilityReturnPeriod.Checked Then
                        lProbScale.LabelStyle = ProbabilityScale.ProbabilityLabelStyle.ReturnInterval
                        .Title.Text = ReplaceStringNoCase(.Title.Text, "Percent", "Return Interval")
                        .Title.Text = ReplaceStringNoCase(.Title.Text, "Fraction", "Return Interval")
                    End If
                    If Double.TryParse(txtProbabilityDeviations.Text, lTemp) Then
                        lProbScale.standardDeviations = lTemp
                    End If
                End If
                If Double.TryParse(txtAxisDisplayMinimum.Text, lTemp) Then
                    .Scale.MinAuto = False
                    .Scale.Min = lTemp
                End If
                If Double.TryParse(txtAxisDisplayMaximum.Text, lTemp) Then
                    .Scale.MaxAuto = False
                    .Scale.Max = lTemp
                End If
                .Title.Text = txtAxisLabel.Text
                .Scale.IsReverse = chkRangeReverse.Checked
                .MajorGrid.IsVisible = chkAxisMajorGridVisible.Checked
                .MajorGrid.Color = txtAxisMajorGridColor.BackColor
                .MajorTic.IsInside = chkAxisMajorTicsVisible.Checked
                .MinorGrid.IsVisible = chkAxisMinorGridVisible.Checked
                .MinorGrid.Color = txtAxisMinorGridColor.BackColor
                .MinorTic.IsInside = chkAxisMinorTicsVisible.Checked
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
            .Border.IsVisible = chkLegendOutline.Checked
            .FontSpec.FontColor = txtLegendFontColor.BackColor
            '.IsVisible = comboLegendLocation.Text <> "None"
            'Select Case comboLegendLocation.Text
            '    Case "Bottom" : .Position = LegendPos.Bottom
            '    Case "BottomCenter" : .Position = LegendPos.BottomCenter
            '    Case "BottomFlushLeft" : .Position = LegendPos.BottomFlushLeft
            '    Case "Float" : .Position = LegendPos.Float
            '    Case "InsideBotLeft" : .Position = LegendPos.InsideBotLeft
            '    Case "InsideBotRight" : .Position = LegendPos.InsideBotRight
            '    Case "InsideTopLeft" : .Position = LegendPos.InsideTopLeft
            '    Case "InsideTopRight" : .Position = LegendPos.InsideTopRight
            '    Case "Left" : .Position = LegendPos.Left
            '    Case "Right" : .Position = LegendPos.Right
            '    Case "Top" : .Position = LegendPos.Top
            '    Case "TopCenter" : .Position = LegendPos.TopCenter
            '    Case "TopFlushLeft" : .Position = LegendPos.TopFlushLeft
            'End Select
        End With
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        ApplyAll()
    End Sub

    Private Sub ApplyAll()
        SetAxisFromControls(AxisFromCombo())
        SetLegendFromControls()
        If comboWhichCurve.SelectedIndex >= 0 Then
            SetCurveFromControls(pPane.CurveList(comboWhichCurve.SelectedIndex))
            SetComboFromCurves()
        End If

        If txtText.Text.Length > 0 Then
            Dim lText As TextObj = FindTextObject(txtText.Text)
            If lText Is Nothing Then
                AddTextFromControls
            End If
        End If

        RaiseEvent Apply()
    End Sub

    Private Sub cboWhichAxis_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetControlsFromSelectedCurve()
    End Sub

    Private Sub txtColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles txtAxisMinorGridColor.Click, txtAxisMajorGridColor.Click, txtCurveColor.Click, txtLegendFontColor.Click, txtTextColor.Click
        ChooseTextBoxBackColor(sender)
    End Sub

    Private Sub ChooseTextBoxBackColor(ByVal aTextBox As TextBox)
        Dim lColorDialog As ColorDialog = New ColorDialog()
        lColorDialog.Color = aTextBox.BackColor
        If lColorDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            aTextBox.BackColor = lColorDialog.Color
            If chkAutoApply.Checked Then ApplyAll()
        End If
    End Sub

    Private Sub chkAny_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        chkAxisMajorGridVisible.CheckedChanged, _
        chkAxisMajorTicsVisible.CheckedChanged, _
        chkAxisMinorGridVisible.CheckedChanged, _
        chkAxisMinorTicsVisible.CheckedChanged, _
        chkCurveLineVisible.CheckedChanged, _
        chkCurveSymbolVisible.CheckedChanged, _
        chkLegendOutline.CheckedChanged, _
        chkRangeReverse.CheckedChanged

        If chkAutoApply.Checked Then ApplyAll()
    End Sub

    Private Sub txtLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            txtAxisLabel.TextChanged, txtCurveLabel.TextChanged
        If chkAutoApply.Checked Then ApplyAll()
    End Sub

    Private Sub txtNumeric_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
              txtCurveWidth.TextChanged, txtCurveSymbolSize.TextChanged, txtProbabilityDeviations.TextChanged
        If chkAutoApply.Checked AndAlso IsNumeric(sender.Text) Then ApplyAll()
    End Sub

    Private Sub radioAxisLocation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            radioAxisBottom.CheckedChanged, radioAxisLeft.CheckedChanged, radioAxisRight.CheckedChanged, radioAxisAux.CheckedChanged
        SetControlsFromAxis(AxisFromCombo())
    End Sub

    Private Sub radioGeneral_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            radioAxisLinear.CheckedChanged, radioAxisLogarithmic.CheckedChanged, _
            radioCurveYaxisLeft.CheckedChanged, radioCurveYaxisRight.CheckedChanged, radioCurveYaxisAuxiliary.CheckedChanged, _
            radioProbablilityFraction.CheckedChanged, radioProbablilityPercent.CheckedChanged, radioProbablilityReturnPeriod.CheckedChanged

        If chkAutoApply.Checked Then
            Dim lRadio As RadioButton = sender
            If lRadio.Checked Then 'Only apply for newly checked, not also for unchecked
                ApplyAll()
            End If
        End If
    End Sub

    Private Sub comboWhichCurve_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboWhichCurve.SelectedIndexChanged
        If comboWhichCurve.SelectedIndex >= 0 AndAlso pPane.CurveList.Count > comboWhichCurve.SelectedIndex Then
            SetControlsFromCurve(pPane.CurveList(comboWhichCurve.SelectedIndex))
        End If
    End Sub

    Private Sub cboCurveAxis_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If chkAutoApply.Checked Then ApplyAll()
    End Sub

    Private Sub pZgc_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pZgc.MouseClick
        Select Case tabsCategory.SelectedTab.Text
            Case "Legend"
                With pPane.Legend
                    .Position = LegendPos.Float
                    .Location = New Location(e.X / pZgc.Width, e.Y / pZgc.Height, CoordType.PaneFraction)
                    .IsVisible = True
                End With
                'comboLegendLocation.Text = "Float"
                RaiseEvent Apply()
            Case "Text"
                If txtText.Text.Length > 0 Then
                    Dim lText As TextObj = FindTextObject(txtText.Text)
                    If lText Is Nothing Then
                        AddTextFromControls()
                        lText = FindTextObject(txtText.Text)
                    End If
                    If Not lText Is Nothing Then
                        lText.Location = New Location(e.X / pZgc.Width, e.Y / pZgc.Height, CoordType.PaneFraction)
                        RaiseEvent Apply()
                    End If
                End If
        End Select
    End Sub

    Private Sub pZgc_ZoomEvent(ByVal sender As ZedGraph.ZedGraphControl, ByVal oldState As ZedGraph.ZoomState, ByVal newState As ZedGraph.ZoomState) Handles pZgc.ZoomEvent
        SetControlsMinMax(AxisFromCombo())
    End Sub

    Private Sub AddedLineItem()
        SetComboFromCurves()
        tabsCategory.SelectTab(tabCurves)
        comboWhichCurve.SelectedIndex = comboWhichCurve.Items.Count - 1
        RaiseEvent Apply()
    End Sub

    Private Sub btnLineEquationAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLineEquationAdd.Click
        Dim lNewLine As LineItem = AddLine(pPane, txtLineAcoef.Text, txtLineBcoef.Text, Drawing2D.DashStyle.Solid, "Y = " & txtLineAcoef.Text & " X + " & txtLineBcoef.Text)
        AddedLineItem()
    End Sub

    Private Sub btnLineConstantYAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLineConstantYAdd.Click
        Dim lNewLine As LineItem = AddLine(pPane, 0, txtLineYconstant.Text, Drawing2D.DashStyle.Solid, "Y = " & txtLineYconstant.Text)
        AddedLineItem()
    End Sub

    Private Sub cboCurveSymbolType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCurveSymbolType.SelectedIndexChanged
        If chkAutoApply.Checked Then ApplyAll()
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

    'Private Sub comboLegendLocation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If chkAutoApply.Checked Then
    '        SetLegendFromControls()
    '        RaiseEvent Apply()
    '    End If
    'End Sub

    Private Sub btnTextAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextAdd.Click
        AddTextFromControls()
        RaiseEvent Apply()
    End Sub

    Private Sub AddTextFromControls()
        Dim lText As New TextObj(txtText.Text, 0.5, 0.2)
        lText.Location.CoordinateFrame = CoordType.PaneFraction
        lText.FontSpec.FontColor = txtTextColor.BackColor
        lText.FontSpec.IsBold = True
        lText.FontSpec.Size = 16
        lText.FontSpec.Border.IsVisible = False
        lText.FontSpec.Fill.IsVisible = False
        lText.Location.AlignH = AlignH.Left
        lText.Location.AlignV = AlignV.Top
        pPane.GraphObjList.Add(lText)
        SetComboFromTexts()
        comboWhichText.Text = lText.Text
    End Sub

    Private Sub comboWhichText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboWhichText.SelectedIndexChanged
        txtText.Text = comboWhichText.Text
        Dim lText As TextObj = FindTextObject(comboWhichText.Text)
        If Not lText Is Nothing Then
            txtTextColor.BackColor = lText.FontSpec.FontColor
        End If
    End Sub

    Private Function FindTextObject(ByVal aText As String) As TextObj
        For Each lItem As ZedGraph.GraphObj In pPane.GraphObjList
            If lItem.GetType.Name = "TextObj" Then
                Dim lText As TextObj = lItem
                If String.Compare(lText.Text, aText, True) = 0 Then Return lText
            End If
        Next
        Return Nothing
    End Function

    Private Sub btnTextRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextRemove.Click
        Dim lText As TextObj = FindTextObject(comboWhichText.Text)
        If Not lText Is Nothing Then
            pPane.GraphObjList.Remove(lText)
            SetComboFromTexts()
            If chkAutoApply.Checked Then RaiseEvent Apply()
        End If
    End Sub

    Private Sub txtText_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtText.KeyUp
        Dim lText As TextObj = FindTextObject(comboWhichText.Text)
        If Not lText Is Nothing AndAlso lText.Text <> txtText.Text Then
            lText.Text = txtText.Text
            SetComboFromTexts()
            If chkAutoApply.Checked Then RaiseEvent Apply()
        End If
    End Sub

    Private Sub btnLegendFont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLegendFont.Click
        If UserEditFontSpec(pPane.Legend.FontSpec) Then
            If chkAutoApply.Checked Then RaiseEvent Apply()
        End If
    End Sub

    Private Sub btnTextFont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextFont.Click
        Dim lText As TextObj = FindTextObject(comboWhichText.Text)
        If Not lText Is Nothing Then
            If UserEditFontSpec(lText.FontSpec) Then
                If chkAutoApply.Checked Then RaiseEvent Apply()
            End If
        End If
    End Sub


    ''' <summary>
    ''' Use a FontDialog to edit a ZedGraph FontSpec
    ''' </summary>
    ''' <param name="aFontSpec"></param>
    ''' <returns>True if font was edited, false if user cancelled</returns>
    Private Function UserEditFontSpec(ByRef aFontSpec As FontSpec) As Boolean
        Dim cdlg As New Windows.Forms.FontDialog
        cdlg.Font = aFontSpec.GetFont(1)
        If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            With cdlg.Font
                Dim lBorder As ZedGraph.Border = aFontSpec.Border.Clone
                aFontSpec = New FontSpec( _
                    .FontFamily.Name, .Size, aFontSpec.FontColor, _
                    .Bold, .Italic, .Underline, _
                    aFontSpec.Fill.Color, aFontSpec.Fill.Brush, aFontSpec.Fill.Type)
                aFontSpec.Border = lBorder
            End With
            Return True
        End If
        Return False
    End Function

End Class