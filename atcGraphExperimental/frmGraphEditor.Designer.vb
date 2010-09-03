<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGraphEditor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGraphEditor))
        Me.toolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtAxisMaximum = New System.Windows.Forms.TextBox
        Me.txtAxisMinimum = New System.Windows.Forms.TextBox
        Me.lblAxisDataRange = New System.Windows.Forms.Label
        Me.txtCurveColor = New System.Windows.Forms.TextBox
        Me.lblCurveColor = New System.Windows.Forms.Label
        Me.comboWhichCurve = New System.Windows.Forms.ComboBox
        Me.txtAxisMinorGridColor = New System.Windows.Forms.TextBox
        Me.txtAxisMajorGridColor = New System.Windows.Forms.TextBox
        Me.txtAxisDisplayMaximum = New System.Windows.Forms.TextBox
        Me.txtAxisDisplayMinimum = New System.Windows.Forms.TextBox
        Me.lblAxisRange = New System.Windows.Forms.Label
        Me.txtCurveWidth = New System.Windows.Forms.TextBox
        Me.txtCurveSymbolSize = New System.Windows.Forms.TextBox
        Me.comboWhichText = New System.Windows.Forms.ComboBox
        Me.txtLegendFontColor = New System.Windows.Forms.TextBox
        Me.txtTextColor = New System.Windows.Forms.TextBox
        Me.btnTextFont = New System.Windows.Forms.Button
        Me.txtProbabilityDeviations = New System.Windows.Forms.TextBox
        Me.lblProbabilityDeviations = New System.Windows.Forms.Label
        Me.btnAxisFont = New System.Windows.Forms.Button
        Me.btnScaleFont = New System.Windows.Forms.Button
        Me.lblAxisMinorGrid = New System.Windows.Forms.Label
        Me.lblAxisMajorGrid = New System.Windows.Forms.Label
        Me.lblAxisTo = New System.Windows.Forms.Label
        Me.txtCurveLabel = New System.Windows.Forms.TextBox
        Me.lblCurveLabel = New System.Windows.Forms.Label
        Me.lblCurveYAxis = New System.Windows.Forms.Label
        Me.lblCurve = New System.Windows.Forms.Label
        Me.chkAxisMinorGridVisible = New System.Windows.Forms.CheckBox
        Me.btnApply = New System.Windows.Forms.Button
        Me.chkAxisMajorGridVisible = New System.Windows.Forms.CheckBox
        Me.label2 = New System.Windows.Forms.Label
        Me.lblAxisLabel = New System.Windows.Forms.Label
        Me.lblAxisType = New System.Windows.Forms.Label
        Me.txtAxisLabel = New System.Windows.Forms.TextBox
        Me.lblWhichAxis = New System.Windows.Forms.Label
        Me.lblCurveWidth = New System.Windows.Forms.Label
        Me.tabsCategory = New System.Windows.Forms.TabControl
        Me.tabAxes = New System.Windows.Forms.TabPage
        Me.chkRangeReverse = New System.Windows.Forms.CheckBox
        Me.radioAxisAux = New System.Windows.Forms.RadioButton
        Me.radioAxisRight = New System.Windows.Forms.RadioButton
        Me.radioAxisLeft = New System.Windows.Forms.RadioButton
        Me.radioAxisBottom = New System.Windows.Forms.RadioButton
        Me.chkAxisMinorTicsVisible = New System.Windows.Forms.CheckBox
        Me.chkAxisMajorTicsVisible = New System.Windows.Forms.CheckBox
        Me.panelProbability = New System.Windows.Forms.Panel
        Me.radioProbablilityReturnPeriod = New System.Windows.Forms.RadioButton
        Me.radioProbablilityFraction = New System.Windows.Forms.RadioButton
        Me.radioProbablilityPercent = New System.Windows.Forms.RadioButton
        Me.panelAxisType = New System.Windows.Forms.Panel
        Me.radioAxisProbability = New System.Windows.Forms.RadioButton
        Me.radioAxisLogarithmic = New System.Windows.Forms.RadioButton
        Me.radioAxisLinear = New System.Windows.Forms.RadioButton
        Me.radioAxisTime = New System.Windows.Forms.RadioButton
        Me.tabCurves = New System.Windows.Forms.TabPage
        Me.chkCurveSymbolFillVisible = New System.Windows.Forms.CheckBox
        Me.cboCurveStep = New System.Windows.Forms.ComboBox
        Me.cboCurveSymbolType = New System.Windows.Forms.ComboBox
        Me.chkCurveLineVisible = New System.Windows.Forms.CheckBox
        Me.chkCurveSymbolVisible = New System.Windows.Forms.CheckBox
        Me.lblCurveSymbolSize = New System.Windows.Forms.Label
        Me.radioCurveYaxisAuxiliary = New System.Windows.Forms.RadioButton
        Me.radioCurveYaxisRight = New System.Windows.Forms.RadioButton
        Me.radioCurveYaxisLeft = New System.Windows.Forms.RadioButton
        Me.tabLines = New System.Windows.Forms.TabPage
        Me.grpRegression = New System.Windows.Forms.GroupBox
        Me.btnLineRegressionAdd = New System.Windows.Forms.Button
        Me.grpLineYconstant = New System.Windows.Forms.GroupBox
        Me.lblLineYconstant = New System.Windows.Forms.Label
        Me.txtLineYconstant = New System.Windows.Forms.TextBox
        Me.btnLineConstantYAdd = New System.Windows.Forms.Button
        Me.grpLineEquation = New System.Windows.Forms.GroupBox
        Me.btnLineEquationAdd = New System.Windows.Forms.Button
        Me.txtLineAcoef = New System.Windows.Forms.TextBox
        Me.lblLineYEquation = New System.Windows.Forms.Label
        Me.lblLineXplus = New System.Windows.Forms.Label
        Me.txtLineBcoef = New System.Windows.Forms.TextBox
        Me.tabLegend = New System.Windows.Forms.TabPage
        Me.chkLegendOutline = New System.Windows.Forms.CheckBox
        Me.lblLegendClickToPlace = New System.Windows.Forms.Label
        Me.btnLegendFont = New System.Windows.Forms.Button
        Me.tabText = New System.Windows.Forms.TabPage
        Me.lblTextClickToPlace = New System.Windows.Forms.Label
        Me.btnTextRemove = New System.Windows.Forms.Button
        Me.btnTextAdd = New System.Windows.Forms.Button
        Me.txtText = New System.Windows.Forms.TextBox
        Me.chkAutoApply = New System.Windows.Forms.CheckBox
        Me.tabsCategory.SuspendLayout()
        Me.tabAxes.SuspendLayout()
        Me.panelProbability.SuspendLayout()
        Me.panelAxisType.SuspendLayout()
        Me.tabCurves.SuspendLayout()
        Me.tabLines.SuspendLayout()
        Me.grpRegression.SuspendLayout()
        Me.grpLineYconstant.SuspendLayout()
        Me.grpLineEquation.SuspendLayout()
        Me.tabLegend.SuspendLayout()
        Me.tabText.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtAxisMaximum
        '
        Me.txtAxisMaximum.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.txtAxisMaximum.Enabled = False
        Me.txtAxisMaximum.Location = New System.Drawing.Point(201, 86)
        Me.txtAxisMaximum.Name = "txtAxisMaximum"
        Me.txtAxisMaximum.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisMaximum.TabIndex = 45
        Me.toolTip1.SetToolTip(Me.txtAxisMaximum, "Maximum data value in any dataset using this axis")
        Me.txtAxisMaximum.Visible = False
        '
        'txtAxisMinimum
        '
        Me.txtAxisMinimum.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.txtAxisMinimum.Enabled = False
        Me.txtAxisMinimum.Location = New System.Drawing.Point(81, 86)
        Me.txtAxisMinimum.Name = "txtAxisMinimum"
        Me.txtAxisMinimum.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisMinimum.TabIndex = 44
        Me.toolTip1.SetToolTip(Me.txtAxisMinimum, "Minimum data value in any dataset using this axis")
        Me.txtAxisMinimum.Visible = False
        '
        'lblAxisDataRange
        '
        Me.lblAxisDataRange.AutoSize = True
        Me.lblAxisDataRange.Location = New System.Drawing.Point(6, 89)
        Me.lblAxisDataRange.Name = "lblAxisDataRange"
        Me.lblAxisDataRange.Size = New System.Drawing.Size(65, 13)
        Me.lblAxisDataRange.TabIndex = 15
        Me.lblAxisDataRange.Text = "Data Range"
        Me.toolTip1.SetToolTip(Me.lblAxisDataRange, "Total range of all data using this axis")
        Me.lblAxisDataRange.Visible = False
        '
        'txtCurveColor
        '
        Me.txtCurveColor.BackColor = System.Drawing.Color.LightGray
        Me.txtCurveColor.Location = New System.Drawing.Point(81, 86)
        Me.txtCurveColor.Name = "txtCurveColor"
        Me.txtCurveColor.Size = New System.Drawing.Size(91, 20)
        Me.txtCurveColor.TabIndex = 41
        Me.toolTip1.SetToolTip(Me.txtCurveColor, "Color of curve")
        '
        'lblCurveColor
        '
        Me.lblCurveColor.AutoSize = True
        Me.lblCurveColor.Location = New System.Drawing.Point(6, 89)
        Me.lblCurveColor.Name = "lblCurveColor"
        Me.lblCurveColor.Size = New System.Drawing.Size(31, 13)
        Me.lblCurveColor.TabIndex = 27
        Me.lblCurveColor.Text = "Color"
        Me.toolTip1.SetToolTip(Me.lblCurveColor, "Range of data currently displayed")
        '
        'comboWhichCurve
        '
        Me.comboWhichCurve.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboWhichCurve.FormattingEnabled = True
        Me.comboWhichCurve.Location = New System.Drawing.Point(81, 9)
        Me.comboWhichCurve.Name = "comboWhichCurve"
        Me.comboWhichCurve.Size = New System.Drawing.Size(376, 21)
        Me.comboWhichCurve.TabIndex = 0
        Me.toolTip1.SetToolTip(Me.comboWhichCurve, "Select which curve to edit")
        '
        'txtAxisMinorGridColor
        '
        Me.txtAxisMinorGridColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(225, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.txtAxisMinorGridColor.Location = New System.Drawing.Point(201, 152)
        Me.txtAxisMinorGridColor.Name = "txtAxisMinorGridColor"
        Me.txtAxisMinorGridColor.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisMinorGridColor.TabIndex = 55
        Me.txtAxisMinorGridColor.Text = "Grid Color"
        Me.toolTip1.SetToolTip(Me.txtAxisMinorGridColor, "Color of Minor Grid")
        '
        'txtAxisMajorGridColor
        '
        Me.txtAxisMajorGridColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtAxisMajorGridColor.Location = New System.Drawing.Point(201, 130)
        Me.txtAxisMajorGridColor.Name = "txtAxisMajorGridColor"
        Me.txtAxisMajorGridColor.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisMajorGridColor.TabIndex = 51
        Me.txtAxisMajorGridColor.Text = "Grid Color"
        Me.toolTip1.SetToolTip(Me.txtAxisMajorGridColor, "Color of Major Grid")
        '
        'txtAxisDisplayMaximum
        '
        Me.txtAxisDisplayMaximum.Location = New System.Drawing.Point(201, 106)
        Me.txtAxisDisplayMaximum.Name = "txtAxisDisplayMaximum"
        Me.txtAxisDisplayMaximum.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisDisplayMaximum.TabIndex = 47
        Me.toolTip1.SetToolTip(Me.txtAxisDisplayMaximum, "Maximum value currently displayed on this axis")
        '
        'txtAxisDisplayMinimum
        '
        Me.txtAxisDisplayMinimum.Location = New System.Drawing.Point(81, 106)
        Me.txtAxisDisplayMinimum.Name = "txtAxisDisplayMinimum"
        Me.txtAxisDisplayMinimum.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisDisplayMinimum.TabIndex = 46
        Me.toolTip1.SetToolTip(Me.txtAxisDisplayMinimum, "Minimum value currently displayed on this axis")
        '
        'lblAxisRange
        '
        Me.lblAxisRange.AutoSize = True
        Me.lblAxisRange.Location = New System.Drawing.Point(6, 109)
        Me.lblAxisRange.Name = "lblAxisRange"
        Me.lblAxisRange.Size = New System.Drawing.Size(69, 13)
        Me.lblAxisRange.TabIndex = 19
        Me.lblAxisRange.Text = "Zoom Range"
        Me.toolTip1.SetToolTip(Me.lblAxisRange, "Range of data currently displayed")
        '
        'txtCurveWidth
        '
        Me.txtCurveWidth.Location = New System.Drawing.Point(125, 112)
        Me.txtCurveWidth.Name = "txtCurveWidth"
        Me.txtCurveWidth.Size = New System.Drawing.Size(47, 20)
        Me.txtCurveWidth.TabIndex = 51
        Me.toolTip1.SetToolTip(Me.txtCurveWidth, "Width of curve")
        '
        'txtCurveSymbolSize
        '
        Me.txtCurveSymbolSize.Location = New System.Drawing.Point(125, 138)
        Me.txtCurveSymbolSize.Name = "txtCurveSymbolSize"
        Me.txtCurveSymbolSize.Size = New System.Drawing.Size(47, 20)
        Me.txtCurveSymbolSize.TabIndex = 54
        Me.toolTip1.SetToolTip(Me.txtCurveSymbolSize, "Width of curve")
        '
        'comboWhichText
        '
        Me.comboWhichText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboWhichText.FormattingEnabled = True
        Me.comboWhichText.Location = New System.Drawing.Point(81, 9)
        Me.comboWhichText.Name = "comboWhichText"
        Me.comboWhichText.Size = New System.Drawing.Size(376, 21)
        Me.comboWhichText.TabIndex = 12
        Me.toolTip1.SetToolTip(Me.comboWhichText, "Select which curve to edit")
        '
        'txtLegendFontColor
        '
        Me.txtLegendFontColor.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLegendFontColor.BackColor = System.Drawing.Color.LightGray
        Me.txtLegendFontColor.Location = New System.Drawing.Point(221, 158)
        Me.txtLegendFontColor.Name = "txtLegendFontColor"
        Me.txtLegendFontColor.Size = New System.Drawing.Size(74, 20)
        Me.txtLegendFontColor.TabIndex = 55
        Me.toolTip1.SetToolTip(Me.txtLegendFontColor, "Color of Font")
        '
        'txtTextColor
        '
        Me.txtTextColor.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTextColor.BackColor = System.Drawing.Color.Black
        Me.txtTextColor.Location = New System.Drawing.Point(221, 158)
        Me.txtTextColor.Name = "txtTextColor"
        Me.txtTextColor.Size = New System.Drawing.Size(74, 20)
        Me.txtTextColor.TabIndex = 15
        Me.toolTip1.SetToolTip(Me.txtTextColor, "Color of Text")
        '
        'btnTextFont
        '
        Me.btnTextFont.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTextFont.Location = New System.Drawing.Point(140, 156)
        Me.btnTextFont.Name = "btnTextFont"
        Me.btnTextFont.Size = New System.Drawing.Size(75, 23)
        Me.btnTextFont.TabIndex = 14
        Me.btnTextFont.Text = "Font"
        Me.toolTip1.SetToolTip(Me.btnTextFont, "Change Font of Text")
        Me.btnTextFont.UseVisualStyleBackColor = True
        '
        'txtProbabilityDeviations
        '
        Me.txtProbabilityDeviations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProbabilityDeviations.Location = New System.Drawing.Point(328, 4)
        Me.txtProbabilityDeviations.Name = "txtProbabilityDeviations"
        Me.txtProbabilityDeviations.Size = New System.Drawing.Size(48, 20)
        Me.txtProbabilityDeviations.TabIndex = 41
        Me.toolTip1.SetToolTip(Me.txtProbabilityDeviations, "Minimum value currently displayed on this axis")
        '
        'lblProbabilityDeviations
        '
        Me.lblProbabilityDeviations.AutoSize = True
        Me.lblProbabilityDeviations.Location = New System.Drawing.Point(246, 7)
        Me.lblProbabilityDeviations.Name = "lblProbabilityDeviations"
        Me.lblProbabilityDeviations.Size = New System.Drawing.Size(76, 13)
        Me.lblProbabilityDeviations.TabIndex = 21
        Me.lblProbabilityDeviations.Text = "Std Deviations"
        Me.toolTip1.SetToolTip(Me.lblProbabilityDeviations, "Range of data currently displayed")
        '
        'btnAxisFont
        '
        Me.btnAxisFont.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAxisFont.Location = New System.Drawing.Point(389, 62)
        Me.btnAxisFont.Name = "btnAxisFont"
        Me.btnAxisFont.Size = New System.Drawing.Size(68, 20)
        Me.btnAxisFont.TabIndex = 43
        Me.btnAxisFont.Text = "Font"
        Me.toolTip1.SetToolTip(Me.btnAxisFont, "Change Font of Text")
        Me.btnAxisFont.UseVisualStyleBackColor = True
        '
        'btnScaleFont
        '
        Me.btnScaleFont.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnScaleFont.Location = New System.Drawing.Point(298, 130)
        Me.btnScaleFont.Name = "btnScaleFont"
        Me.btnScaleFont.Size = New System.Drawing.Size(68, 20)
        Me.btnScaleFont.TabIndex = 52
        Me.btnScaleFont.Text = "Font"
        Me.toolTip1.SetToolTip(Me.btnScaleFont, "Change Font of Text")
        Me.btnScaleFont.UseVisualStyleBackColor = True
        '
        'lblAxisMinorGrid
        '
        Me.lblAxisMinorGrid.AutoSize = True
        Me.lblAxisMinorGrid.Location = New System.Drawing.Point(6, 155)
        Me.lblAxisMinorGrid.Name = "lblAxisMinorGrid"
        Me.lblAxisMinorGrid.Size = New System.Drawing.Size(60, 13)
        Me.lblAxisMinorGrid.TabIndex = 28
        Me.lblAxisMinorGrid.Text = "Minor Units"
        '
        'lblAxisMajorGrid
        '
        Me.lblAxisMajorGrid.AutoSize = True
        Me.lblAxisMajorGrid.Location = New System.Drawing.Point(6, 133)
        Me.lblAxisMajorGrid.Name = "lblAxisMajorGrid"
        Me.lblAxisMajorGrid.Size = New System.Drawing.Size(60, 13)
        Me.lblAxisMajorGrid.TabIndex = 24
        Me.lblAxisMajorGrid.Text = "Major Units"
        '
        'lblAxisTo
        '
        Me.lblAxisTo.AutoSize = True
        Me.lblAxisTo.Location = New System.Drawing.Point(179, 89)
        Me.lblAxisTo.Name = "lblAxisTo"
        Me.lblAxisTo.Size = New System.Drawing.Size(16, 13)
        Me.lblAxisTo.TabIndex = 18
        Me.lblAxisTo.Text = "to"
        Me.lblAxisTo.Visible = False
        '
        'txtCurveLabel
        '
        Me.txtCurveLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCurveLabel.Location = New System.Drawing.Point(81, 36)
        Me.txtCurveLabel.Name = "txtCurveLabel"
        Me.txtCurveLabel.Size = New System.Drawing.Size(376, 20)
        Me.txtCurveLabel.TabIndex = 16
        '
        'lblCurveLabel
        '
        Me.lblCurveLabel.AutoSize = True
        Me.lblCurveLabel.Location = New System.Drawing.Point(6, 39)
        Me.lblCurveLabel.Name = "lblCurveLabel"
        Me.lblCurveLabel.Size = New System.Drawing.Size(33, 13)
        Me.lblCurveLabel.TabIndex = 15
        Me.lblCurveLabel.Text = "Label"
        '
        'lblCurveYAxis
        '
        Me.lblCurveYAxis.AutoSize = True
        Me.lblCurveYAxis.Location = New System.Drawing.Point(6, 65)
        Me.lblCurveYAxis.Name = "lblCurveYAxis"
        Me.lblCurveYAxis.Size = New System.Drawing.Size(36, 13)
        Me.lblCurveYAxis.TabIndex = 14
        Me.lblCurveYAxis.Text = "Y Axis"
        '
        'lblCurve
        '
        Me.lblCurve.AutoSize = True
        Me.lblCurve.Location = New System.Drawing.Point(6, 12)
        Me.lblCurve.Name = "lblCurve"
        Me.lblCurve.Size = New System.Drawing.Size(35, 13)
        Me.lblCurve.TabIndex = 12
        Me.lblCurve.Text = "Curve"
        '
        'chkAxisMinorGridVisible
        '
        Me.chkAxisMinorGridVisible.AutoSize = True
        Me.chkAxisMinorGridVisible.Location = New System.Drawing.Point(129, 154)
        Me.chkAxisMinorGridVisible.Name = "chkAxisMinorGridVisible"
        Me.chkAxisMinorGridVisible.Size = New System.Drawing.Size(43, 17)
        Me.chkAxisMinorGridVisible.TabIndex = 54
        Me.chkAxisMinorGridVisible.Text = "grid"
        Me.chkAxisMinorGridVisible.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.Location = New System.Drawing.Point(405, 236)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 23)
        Me.btnApply.TabIndex = 60
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'chkAxisMajorGridVisible
        '
        Me.chkAxisMajorGridVisible.AutoSize = True
        Me.chkAxisMajorGridVisible.Location = New System.Drawing.Point(129, 132)
        Me.chkAxisMajorGridVisible.Name = "chkAxisMajorGridVisible"
        Me.chkAxisMajorGridVisible.Size = New System.Drawing.Size(43, 17)
        Me.chkAxisMajorGridVisible.TabIndex = 50
        Me.chkAxisMajorGridVisible.Text = "grid"
        Me.chkAxisMajorGridVisible.UseVisualStyleBackColor = True
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(179, 109)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(16, 13)
        Me.label2.TabIndex = 22
        Me.label2.Text = "to"
        '
        'lblAxisLabel
        '
        Me.lblAxisLabel.AutoSize = True
        Me.lblAxisLabel.Location = New System.Drawing.Point(6, 65)
        Me.lblAxisLabel.Name = "lblAxisLabel"
        Me.lblAxisLabel.Size = New System.Drawing.Size(27, 13)
        Me.lblAxisLabel.TabIndex = 13
        Me.lblAxisLabel.Text = "Title"
        '
        'lblAxisType
        '
        Me.lblAxisType.AutoSize = True
        Me.lblAxisType.Location = New System.Drawing.Point(6, 39)
        Me.lblAxisType.Name = "lblAxisType"
        Me.lblAxisType.Size = New System.Drawing.Size(31, 13)
        Me.lblAxisType.TabIndex = 12
        Me.lblAxisType.Text = "Type"
        '
        'txtAxisLabel
        '
        Me.txtAxisLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAxisLabel.Location = New System.Drawing.Point(81, 62)
        Me.txtAxisLabel.Name = "txtAxisLabel"
        Me.txtAxisLabel.Size = New System.Drawing.Size(302, 20)
        Me.txtAxisLabel.TabIndex = 42
        '
        'lblWhichAxis
        '
        Me.lblWhichAxis.AutoSize = True
        Me.lblWhichAxis.Location = New System.Drawing.Point(6, 12)
        Me.lblWhichAxis.Name = "lblWhichAxis"
        Me.lblWhichAxis.Size = New System.Drawing.Size(26, 13)
        Me.lblWhichAxis.TabIndex = 11
        Me.lblWhichAxis.Text = "Axis"
        '
        'lblCurveWidth
        '
        Me.lblCurveWidth.AutoSize = True
        Me.lblCurveWidth.Location = New System.Drawing.Point(78, 115)
        Me.lblCurveWidth.Name = "lblCurveWidth"
        Me.lblCurveWidth.Size = New System.Drawing.Size(35, 13)
        Me.lblCurveWidth.TabIndex = 30
        Me.lblCurveWidth.Text = "Width"
        '
        'tabsCategory
        '
        Me.tabsCategory.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabsCategory.Controls.Add(Me.tabAxes)
        Me.tabsCategory.Controls.Add(Me.tabCurves)
        Me.tabsCategory.Controls.Add(Me.tabLines)
        Me.tabsCategory.Controls.Add(Me.tabLegend)
        Me.tabsCategory.Controls.Add(Me.tabText)
        Me.tabsCategory.Location = New System.Drawing.Point(12, 12)
        Me.tabsCategory.Name = "tabsCategory"
        Me.tabsCategory.SelectedIndex = 0
        Me.tabsCategory.Size = New System.Drawing.Size(468, 212)
        Me.tabsCategory.TabIndex = 11
        '
        'tabAxes
        '
        Me.tabAxes.Controls.Add(Me.btnScaleFont)
        Me.tabAxes.Controls.Add(Me.btnAxisFont)
        Me.tabAxes.Controls.Add(Me.chkRangeReverse)
        Me.tabAxes.Controls.Add(Me.radioAxisAux)
        Me.tabAxes.Controls.Add(Me.radioAxisRight)
        Me.tabAxes.Controls.Add(Me.radioAxisLeft)
        Me.tabAxes.Controls.Add(Me.radioAxisBottom)
        Me.tabAxes.Controls.Add(Me.chkAxisMinorTicsVisible)
        Me.tabAxes.Controls.Add(Me.chkAxisMajorTicsVisible)
        Me.tabAxes.Controls.Add(Me.lblWhichAxis)
        Me.tabAxes.Controls.Add(Me.txtAxisMinorGridColor)
        Me.tabAxes.Controls.Add(Me.lblAxisMinorGrid)
        Me.tabAxes.Controls.Add(Me.chkAxisMinorGridVisible)
        Me.tabAxes.Controls.Add(Me.lblAxisType)
        Me.tabAxes.Controls.Add(Me.txtAxisMajorGridColor)
        Me.tabAxes.Controls.Add(Me.lblAxisLabel)
        Me.tabAxes.Controls.Add(Me.txtAxisLabel)
        Me.tabAxes.Controls.Add(Me.lblAxisMajorGrid)
        Me.tabAxes.Controls.Add(Me.lblAxisDataRange)
        Me.tabAxes.Controls.Add(Me.chkAxisMajorGridVisible)
        Me.tabAxes.Controls.Add(Me.txtAxisMinimum)
        Me.tabAxes.Controls.Add(Me.label2)
        Me.tabAxes.Controls.Add(Me.txtAxisMaximum)
        Me.tabAxes.Controls.Add(Me.txtAxisDisplayMaximum)
        Me.tabAxes.Controls.Add(Me.lblAxisTo)
        Me.tabAxes.Controls.Add(Me.txtAxisDisplayMinimum)
        Me.tabAxes.Controls.Add(Me.lblAxisRange)
        Me.tabAxes.Controls.Add(Me.panelProbability)
        Me.tabAxes.Controls.Add(Me.panelAxisType)
        Me.tabAxes.Location = New System.Drawing.Point(4, 22)
        Me.tabAxes.Name = "tabAxes"
        Me.tabAxes.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAxes.Size = New System.Drawing.Size(460, 186)
        Me.tabAxes.TabIndex = 0
        Me.tabAxes.Text = "Axes"
        Me.tabAxes.UseVisualStyleBackColor = True
        '
        'chkRangeReverse
        '
        Me.chkRangeReverse.AutoSize = True
        Me.chkRangeReverse.Location = New System.Drawing.Point(298, 108)
        Me.chkRangeReverse.Name = "chkRangeReverse"
        Me.chkRangeReverse.Size = New System.Drawing.Size(66, 17)
        Me.chkRangeReverse.TabIndex = 48
        Me.chkRangeReverse.Text = "Reverse"
        Me.chkRangeReverse.UseVisualStyleBackColor = True
        '
        'radioAxisAux
        '
        Me.radioAxisAux.AutoSize = True
        Me.radioAxisAux.Location = New System.Drawing.Point(300, 10)
        Me.radioAxisAux.Name = "radioAxisAux"
        Me.radioAxisAux.Size = New System.Drawing.Size(73, 17)
        Me.radioAxisAux.TabIndex = 37
        Me.radioAxisAux.Text = "Auxiliary Y"
        Me.radioAxisAux.UseVisualStyleBackColor = True
        '
        'radioAxisRight
        '
        Me.radioAxisRight.AutoSize = True
        Me.radioAxisRight.Location = New System.Drawing.Point(224, 10)
        Me.radioAxisRight.Name = "radioAxisRight"
        Me.radioAxisRight.Size = New System.Drawing.Size(60, 17)
        Me.radioAxisRight.TabIndex = 36
        Me.radioAxisRight.Text = "Right Y"
        Me.radioAxisRight.UseVisualStyleBackColor = True
        '
        'radioAxisLeft
        '
        Me.radioAxisLeft.AutoSize = True
        Me.radioAxisLeft.Location = New System.Drawing.Point(155, 10)
        Me.radioAxisLeft.Name = "radioAxisLeft"
        Me.radioAxisLeft.Size = New System.Drawing.Size(53, 17)
        Me.radioAxisLeft.TabIndex = 35
        Me.radioAxisLeft.Text = "Left Y"
        Me.radioAxisLeft.UseVisualStyleBackColor = True
        '
        'radioAxisBottom
        '
        Me.radioAxisBottom.AutoSize = True
        Me.radioAxisBottom.Checked = True
        Me.radioAxisBottom.Location = New System.Drawing.Point(81, 10)
        Me.radioAxisBottom.Name = "radioAxisBottom"
        Me.radioAxisBottom.Size = New System.Drawing.Size(68, 17)
        Me.radioAxisBottom.TabIndex = 34
        Me.radioAxisBottom.TabStop = True
        Me.radioAxisBottom.Text = "Bottom X"
        Me.radioAxisBottom.UseVisualStyleBackColor = True
        '
        'chkAxisMinorTicsVisible
        '
        Me.chkAxisMinorTicsVisible.AutoSize = True
        Me.chkAxisMinorTicsVisible.Location = New System.Drawing.Point(81, 155)
        Me.chkAxisMinorTicsVisible.Name = "chkAxisMinorTicsVisible"
        Me.chkAxisMinorTicsVisible.Size = New System.Drawing.Size(42, 17)
        Me.chkAxisMinorTicsVisible.TabIndex = 53
        Me.chkAxisMinorTicsVisible.Text = "tics"
        Me.chkAxisMinorTicsVisible.UseVisualStyleBackColor = True
        '
        'chkAxisMajorTicsVisible
        '
        Me.chkAxisMajorTicsVisible.AutoSize = True
        Me.chkAxisMajorTicsVisible.Location = New System.Drawing.Point(81, 133)
        Me.chkAxisMajorTicsVisible.Name = "chkAxisMajorTicsVisible"
        Me.chkAxisMajorTicsVisible.Size = New System.Drawing.Size(42, 17)
        Me.chkAxisMajorTicsVisible.TabIndex = 49
        Me.chkAxisMajorTicsVisible.Text = "tics"
        Me.chkAxisMajorTicsVisible.UseVisualStyleBackColor = True
        '
        'panelProbability
        '
        Me.panelProbability.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelProbability.Controls.Add(Me.txtProbabilityDeviations)
        Me.panelProbability.Controls.Add(Me.lblProbabilityDeviations)
        Me.panelProbability.Controls.Add(Me.radioProbablilityReturnPeriod)
        Me.panelProbability.Controls.Add(Me.radioProbablilityFraction)
        Me.panelProbability.Controls.Add(Me.radioProbablilityPercent)
        Me.panelProbability.Location = New System.Drawing.Point(74, 32)
        Me.panelProbability.Name = "panelProbability"
        Me.panelProbability.Size = New System.Drawing.Size(379, 28)
        Me.panelProbability.TabIndex = 39
        Me.panelProbability.Visible = False
        '
        'radioProbablilityReturnPeriod
        '
        Me.radioProbablilityReturnPeriod.AutoSize = True
        Me.radioProbablilityReturnPeriod.Location = New System.Drawing.Point(150, 5)
        Me.radioProbablilityReturnPeriod.Name = "radioProbablilityReturnPeriod"
        Me.radioProbablilityReturnPeriod.Size = New System.Drawing.Size(90, 17)
        Me.radioProbablilityReturnPeriod.TabIndex = 40
        Me.radioProbablilityReturnPeriod.TabStop = True
        Me.radioProbablilityReturnPeriod.Text = "Return Period"
        Me.radioProbablilityReturnPeriod.UseVisualStyleBackColor = True
        '
        'radioProbablilityFraction
        '
        Me.radioProbablilityFraction.AutoSize = True
        Me.radioProbablilityFraction.Location = New System.Drawing.Point(81, 5)
        Me.radioProbablilityFraction.Name = "radioProbablilityFraction"
        Me.radioProbablilityFraction.Size = New System.Drawing.Size(63, 17)
        Me.radioProbablilityFraction.TabIndex = 39
        Me.radioProbablilityFraction.TabStop = True
        Me.radioProbablilityFraction.Text = "Fraction"
        Me.radioProbablilityFraction.UseVisualStyleBackColor = True
        '
        'radioProbablilityPercent
        '
        Me.radioProbablilityPercent.AutoSize = True
        Me.radioProbablilityPercent.Location = New System.Drawing.Point(7, 5)
        Me.radioProbablilityPercent.Name = "radioProbablilityPercent"
        Me.radioProbablilityPercent.Size = New System.Drawing.Size(62, 17)
        Me.radioProbablilityPercent.TabIndex = 38
        Me.radioProbablilityPercent.TabStop = True
        Me.radioProbablilityPercent.Text = "Percent"
        Me.radioProbablilityPercent.UseVisualStyleBackColor = True
        '
        'panelAxisType
        '
        Me.panelAxisType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelAxisType.Controls.Add(Me.radioAxisProbability)
        Me.panelAxisType.Controls.Add(Me.radioAxisLogarithmic)
        Me.panelAxisType.Controls.Add(Me.radioAxisLinear)
        Me.panelAxisType.Controls.Add(Me.radioAxisTime)
        Me.panelAxisType.Location = New System.Drawing.Point(74, 32)
        Me.panelAxisType.Name = "panelAxisType"
        Me.panelAxisType.Size = New System.Drawing.Size(379, 28)
        Me.panelAxisType.TabIndex = 38
        '
        'radioAxisProbability
        '
        Me.radioAxisProbability.AutoSize = True
        Me.radioAxisProbability.Enabled = False
        Me.radioAxisProbability.Location = New System.Drawing.Point(226, 5)
        Me.radioAxisProbability.Name = "radioAxisProbability"
        Me.radioAxisProbability.Size = New System.Drawing.Size(73, 17)
        Me.radioAxisProbability.TabIndex = 3
        Me.radioAxisProbability.TabStop = True
        Me.radioAxisProbability.Text = "Probability"
        Me.radioAxisProbability.UseVisualStyleBackColor = True
        '
        'radioAxisLogarithmic
        '
        Me.radioAxisLogarithmic.AutoSize = True
        Me.radioAxisLogarithmic.Enabled = False
        Me.radioAxisLogarithmic.Location = New System.Drawing.Point(141, 5)
        Me.radioAxisLogarithmic.Name = "radioAxisLogarithmic"
        Me.radioAxisLogarithmic.Size = New System.Drawing.Size(79, 17)
        Me.radioAxisLogarithmic.TabIndex = 2
        Me.radioAxisLogarithmic.TabStop = True
        Me.radioAxisLogarithmic.Text = "Logarithmic"
        Me.radioAxisLogarithmic.UseVisualStyleBackColor = True
        '
        'radioAxisLinear
        '
        Me.radioAxisLinear.AutoSize = True
        Me.radioAxisLinear.Enabled = False
        Me.radioAxisLinear.Location = New System.Drawing.Point(81, 5)
        Me.radioAxisLinear.Name = "radioAxisLinear"
        Me.radioAxisLinear.Size = New System.Drawing.Size(54, 17)
        Me.radioAxisLinear.TabIndex = 1
        Me.radioAxisLinear.TabStop = True
        Me.radioAxisLinear.Text = "Linear"
        Me.radioAxisLinear.UseVisualStyleBackColor = True
        '
        'radioAxisTime
        '
        Me.radioAxisTime.AutoSize = True
        Me.radioAxisTime.Enabled = False
        Me.radioAxisTime.Location = New System.Drawing.Point(7, 5)
        Me.radioAxisTime.Name = "radioAxisTime"
        Me.radioAxisTime.Size = New System.Drawing.Size(48, 17)
        Me.radioAxisTime.TabIndex = 0
        Me.radioAxisTime.TabStop = True
        Me.radioAxisTime.Text = "Time"
        Me.radioAxisTime.UseVisualStyleBackColor = True
        '
        'tabCurves
        '
        Me.tabCurves.Controls.Add(Me.chkCurveSymbolFillVisible)
        Me.tabCurves.Controls.Add(Me.cboCurveStep)
        Me.tabCurves.Controls.Add(Me.cboCurveSymbolType)
        Me.tabCurves.Controls.Add(Me.chkCurveLineVisible)
        Me.tabCurves.Controls.Add(Me.chkCurveSymbolVisible)
        Me.tabCurves.Controls.Add(Me.lblCurveSymbolSize)
        Me.tabCurves.Controls.Add(Me.txtCurveSymbolSize)
        Me.tabCurves.Controls.Add(Me.radioCurveYaxisAuxiliary)
        Me.tabCurves.Controls.Add(Me.radioCurveYaxisRight)
        Me.tabCurves.Controls.Add(Me.radioCurveYaxisLeft)
        Me.tabCurves.Controls.Add(Me.lblCurveWidth)
        Me.tabCurves.Controls.Add(Me.comboWhichCurve)
        Me.tabCurves.Controls.Add(Me.txtCurveWidth)
        Me.tabCurves.Controls.Add(Me.txtCurveColor)
        Me.tabCurves.Controls.Add(Me.lblCurve)
        Me.tabCurves.Controls.Add(Me.lblCurveColor)
        Me.tabCurves.Controls.Add(Me.lblCurveYAxis)
        Me.tabCurves.Controls.Add(Me.txtCurveLabel)
        Me.tabCurves.Controls.Add(Me.lblCurveLabel)
        Me.tabCurves.Location = New System.Drawing.Point(4, 22)
        Me.tabCurves.Name = "tabCurves"
        Me.tabCurves.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCurves.Size = New System.Drawing.Size(460, 186)
        Me.tabCurves.TabIndex = 1
        Me.tabCurves.Text = "Curves"
        Me.tabCurves.UseVisualStyleBackColor = True
        '
        'chkCurveSymbolFillVisible
        '
        Me.chkCurveSymbolFillVisible.AutoSize = True
        Me.chkCurveSymbolFillVisible.Location = New System.Drawing.Point(335, 140)
        Me.chkCurveSymbolFillVisible.Name = "chkCurveSymbolFillVisible"
        Me.chkCurveSymbolFillVisible.Size = New System.Drawing.Size(38, 17)
        Me.chkCurveSymbolFillVisible.TabIndex = 56
        Me.chkCurveSymbolFillVisible.Text = "Fill"
        Me.chkCurveSymbolFillVisible.UseVisualStyleBackColor = True
        '
        'cboCurveStep
        '
        Me.cboCurveStep.FormattingEnabled = True
        Me.cboCurveStep.Location = New System.Drawing.Point(208, 111)
        Me.cboCurveStep.MaxDropDownItems = 11
        Me.cboCurveStep.Name = "cboCurveStep"
        Me.cboCurveStep.Size = New System.Drawing.Size(121, 21)
        Me.cboCurveStep.TabIndex = 52
        '
        'cboCurveSymbolType
        '
        Me.cboCurveSymbolType.FormattingEnabled = True
        Me.cboCurveSymbolType.Location = New System.Drawing.Point(208, 138)
        Me.cboCurveSymbolType.MaxDropDownItems = 11
        Me.cboCurveSymbolType.Name = "cboCurveSymbolType"
        Me.cboCurveSymbolType.Size = New System.Drawing.Size(121, 21)
        Me.cboCurveSymbolType.TabIndex = 55
        '
        'chkCurveLineVisible
        '
        Me.chkCurveLineVisible.AutoSize = True
        Me.chkCurveLineVisible.Location = New System.Drawing.Point(9, 114)
        Me.chkCurveLineVisible.Name = "chkCurveLineVisible"
        Me.chkCurveLineVisible.Size = New System.Drawing.Size(46, 17)
        Me.chkCurveLineVisible.TabIndex = 50
        Me.chkCurveLineVisible.Text = "Line"
        Me.chkCurveLineVisible.UseVisualStyleBackColor = True
        '
        'chkCurveSymbolVisible
        '
        Me.chkCurveSymbolVisible.AutoSize = True
        Me.chkCurveSymbolVisible.Location = New System.Drawing.Point(9, 137)
        Me.chkCurveSymbolVisible.Name = "chkCurveSymbolVisible"
        Me.chkCurveSymbolVisible.Size = New System.Drawing.Size(65, 17)
        Me.chkCurveSymbolVisible.TabIndex = 53
        Me.chkCurveSymbolVisible.Text = "Symbols"
        Me.chkCurveSymbolVisible.UseVisualStyleBackColor = True
        '
        'lblCurveSymbolSize
        '
        Me.lblCurveSymbolSize.AutoSize = True
        Me.lblCurveSymbolSize.Location = New System.Drawing.Point(78, 141)
        Me.lblCurveSymbolSize.Name = "lblCurveSymbolSize"
        Me.lblCurveSymbolSize.Size = New System.Drawing.Size(27, 13)
        Me.lblCurveSymbolSize.TabIndex = 48
        Me.lblCurveSymbolSize.Text = "Size"
        '
        'radioCurveYaxisAuxiliary
        '
        Me.radioCurveYaxisAuxiliary.AutoSize = True
        Me.radioCurveYaxisAuxiliary.Location = New System.Drawing.Point(208, 63)
        Me.radioCurveYaxisAuxiliary.Name = "radioCurveYaxisAuxiliary"
        Me.radioCurveYaxisAuxiliary.Size = New System.Drawing.Size(63, 17)
        Me.radioCurveYaxisAuxiliary.TabIndex = 40
        Me.radioCurveYaxisAuxiliary.Text = "Auxiliary"
        Me.radioCurveYaxisAuxiliary.UseVisualStyleBackColor = True
        '
        'radioCurveYaxisRight
        '
        Me.radioCurveYaxisRight.AutoSize = True
        Me.radioCurveYaxisRight.Location = New System.Drawing.Point(142, 63)
        Me.radioCurveYaxisRight.Name = "radioCurveYaxisRight"
        Me.radioCurveYaxisRight.Size = New System.Drawing.Size(50, 17)
        Me.radioCurveYaxisRight.TabIndex = 39
        Me.radioCurveYaxisRight.Text = "Right"
        Me.radioCurveYaxisRight.UseVisualStyleBackColor = True
        '
        'radioCurveYaxisLeft
        '
        Me.radioCurveYaxisLeft.AutoSize = True
        Me.radioCurveYaxisLeft.Location = New System.Drawing.Point(81, 63)
        Me.radioCurveYaxisLeft.Name = "radioCurveYaxisLeft"
        Me.radioCurveYaxisLeft.Size = New System.Drawing.Size(43, 17)
        Me.radioCurveYaxisLeft.TabIndex = 38
        Me.radioCurveYaxisLeft.Text = "Left"
        Me.radioCurveYaxisLeft.UseVisualStyleBackColor = True
        '
        'tabLines
        '
        Me.tabLines.Controls.Add(Me.grpRegression)
        Me.tabLines.Controls.Add(Me.grpLineYconstant)
        Me.tabLines.Controls.Add(Me.grpLineEquation)
        Me.tabLines.Location = New System.Drawing.Point(4, 22)
        Me.tabLines.Name = "tabLines"
        Me.tabLines.Size = New System.Drawing.Size(460, 186)
        Me.tabLines.TabIndex = 2
        Me.tabLines.Text = "Lines"
        Me.tabLines.UseVisualStyleBackColor = True
        '
        'grpRegression
        '
        Me.grpRegression.Controls.Add(Me.btnLineRegressionAdd)
        Me.grpRegression.Location = New System.Drawing.Point(3, 121)
        Me.grpRegression.Name = "grpRegression"
        Me.grpRegression.Size = New System.Drawing.Size(301, 53)
        Me.grpRegression.TabIndex = 20
        Me.grpRegression.TabStop = False
        Me.grpRegression.Text = "Regression"
        '
        'btnLineRegressionAdd
        '
        Me.btnLineRegressionAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLineRegressionAdd.Location = New System.Drawing.Point(220, 17)
        Me.btnLineRegressionAdd.Name = "btnLineRegressionAdd"
        Me.btnLineRegressionAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnLineRegressionAdd.TabIndex = 17
        Me.btnLineRegressionAdd.Text = "Add"
        Me.btnLineRegressionAdd.UseVisualStyleBackColor = True
        '
        'grpLineYconstant
        '
        Me.grpLineYconstant.Controls.Add(Me.lblLineYconstant)
        Me.grpLineYconstant.Controls.Add(Me.txtLineYconstant)
        Me.grpLineYconstant.Controls.Add(Me.btnLineConstantYAdd)
        Me.grpLineYconstant.Location = New System.Drawing.Point(3, 3)
        Me.grpLineYconstant.Name = "grpLineYconstant"
        Me.grpLineYconstant.Size = New System.Drawing.Size(301, 53)
        Me.grpLineYconstant.TabIndex = 19
        Me.grpLineYconstant.TabStop = False
        Me.grpLineYconstant.Text = "Constant Y"
        '
        'lblLineYconstant
        '
        Me.lblLineYconstant.AutoSize = True
        Me.lblLineYconstant.Location = New System.Drawing.Point(6, 24)
        Me.lblLineYconstant.Name = "lblLineYconstant"
        Me.lblLineYconstant.Size = New System.Drawing.Size(23, 13)
        Me.lblLineYconstant.TabIndex = 13
        Me.lblLineYconstant.Text = "Y ="
        '
        'txtLineYconstant
        '
        Me.txtLineYconstant.Location = New System.Drawing.Point(44, 21)
        Me.txtLineYconstant.Name = "txtLineYconstant"
        Me.txtLineYconstant.Size = New System.Drawing.Size(54, 20)
        Me.txtLineYconstant.TabIndex = 0
        Me.txtLineYconstant.Text = "1"
        Me.txtLineYconstant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnLineConstantYAdd
        '
        Me.btnLineConstantYAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLineConstantYAdd.Location = New System.Drawing.Point(220, 19)
        Me.btnLineConstantYAdd.Name = "btnLineConstantYAdd"
        Me.btnLineConstantYAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnLineConstantYAdd.TabIndex = 3
        Me.btnLineConstantYAdd.Text = "Add"
        Me.btnLineConstantYAdd.UseVisualStyleBackColor = True
        '
        'grpLineEquation
        '
        Me.grpLineEquation.Controls.Add(Me.btnLineEquationAdd)
        Me.grpLineEquation.Controls.Add(Me.txtLineAcoef)
        Me.grpLineEquation.Controls.Add(Me.lblLineYEquation)
        Me.grpLineEquation.Controls.Add(Me.lblLineXplus)
        Me.grpLineEquation.Controls.Add(Me.txtLineBcoef)
        Me.grpLineEquation.Location = New System.Drawing.Point(3, 62)
        Me.grpLineEquation.Name = "grpLineEquation"
        Me.grpLineEquation.Size = New System.Drawing.Size(301, 53)
        Me.grpLineEquation.TabIndex = 18
        Me.grpLineEquation.TabStop = False
        Me.grpLineEquation.Text = "Equation Y = aX + b"
        '
        'btnLineEquationAdd
        '
        Me.btnLineEquationAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLineEquationAdd.Location = New System.Drawing.Point(220, 19)
        Me.btnLineEquationAdd.Name = "btnLineEquationAdd"
        Me.btnLineEquationAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnLineEquationAdd.TabIndex = 13
        Me.btnLineEquationAdd.Text = "Add"
        Me.btnLineEquationAdd.UseVisualStyleBackColor = True
        '
        'txtLineAcoef
        '
        Me.txtLineAcoef.Location = New System.Drawing.Point(44, 21)
        Me.txtLineAcoef.Name = "txtLineAcoef"
        Me.txtLineAcoef.Size = New System.Drawing.Size(54, 20)
        Me.txtLineAcoef.TabIndex = 11
        Me.txtLineAcoef.Text = "1"
        Me.txtLineAcoef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblLineYEquation
        '
        Me.lblLineYEquation.AutoSize = True
        Me.lblLineYEquation.Location = New System.Drawing.Point(6, 24)
        Me.lblLineYEquation.Name = "lblLineYEquation"
        Me.lblLineYEquation.Size = New System.Drawing.Size(23, 13)
        Me.lblLineYEquation.TabIndex = 1
        Me.lblLineYEquation.Text = "Y ="
        '
        'lblLineXplus
        '
        Me.lblLineXplus.AutoSize = True
        Me.lblLineXplus.Location = New System.Drawing.Point(112, 24)
        Me.lblLineXplus.Name = "lblLineXplus"
        Me.lblLineXplus.Size = New System.Drawing.Size(26, 13)
        Me.lblLineXplus.TabIndex = 2
        Me.lblLineXplus.Text = "X  +"
        '
        'txtLineBcoef
        '
        Me.txtLineBcoef.Location = New System.Drawing.Point(150, 21)
        Me.txtLineBcoef.Name = "txtLineBcoef"
        Me.txtLineBcoef.Size = New System.Drawing.Size(54, 20)
        Me.txtLineBcoef.TabIndex = 12
        Me.txtLineBcoef.Text = "0"
        Me.txtLineBcoef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tabLegend
        '
        Me.tabLegend.Controls.Add(Me.chkLegendOutline)
        Me.tabLegend.Controls.Add(Me.lblLegendClickToPlace)
        Me.tabLegend.Controls.Add(Me.btnLegendFont)
        Me.tabLegend.Controls.Add(Me.txtLegendFontColor)
        Me.tabLegend.Location = New System.Drawing.Point(4, 22)
        Me.tabLegend.Name = "tabLegend"
        Me.tabLegend.Size = New System.Drawing.Size(460, 186)
        Me.tabLegend.TabIndex = 4
        Me.tabLegend.Text = "Legend"
        Me.tabLegend.UseVisualStyleBackColor = True
        '
        'chkLegendOutline
        '
        Me.chkLegendOutline.AutoSize = True
        Me.chkLegendOutline.Location = New System.Drawing.Point(18, 38)
        Me.chkLegendOutline.Name = "chkLegendOutline"
        Me.chkLegendOutline.Size = New System.Drawing.Size(59, 17)
        Me.chkLegendOutline.TabIndex = 51
        Me.chkLegendOutline.Text = "Outline"
        Me.chkLegendOutline.UseVisualStyleBackColor = True
        '
        'lblLegendClickToPlace
        '
        Me.lblLegendClickToPlace.AutoSize = True
        Me.lblLegendClickToPlace.Location = New System.Drawing.Point(15, 10)
        Me.lblLegendClickToPlace.Name = "lblLegendClickToPlace"
        Me.lblLegendClickToPlace.Size = New System.Drawing.Size(151, 13)
        Me.lblLegendClickToPlace.TabIndex = 14
        Me.lblLegendClickToPlace.Text = "Click on graph to place legend"
        '
        'btnLegendFont
        '
        Me.btnLegendFont.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLegendFont.Location = New System.Drawing.Point(140, 156)
        Me.btnLegendFont.Name = "btnLegendFont"
        Me.btnLegendFont.Size = New System.Drawing.Size(75, 23)
        Me.btnLegendFont.TabIndex = 53
        Me.btnLegendFont.Text = "Font"
        Me.btnLegendFont.UseVisualStyleBackColor = True
        '
        'tabText
        '
        Me.tabText.Controls.Add(Me.txtTextColor)
        Me.tabText.Controls.Add(Me.lblTextClickToPlace)
        Me.tabText.Controls.Add(Me.btnTextFont)
        Me.tabText.Controls.Add(Me.btnTextRemove)
        Me.tabText.Controls.Add(Me.comboWhichText)
        Me.tabText.Controls.Add(Me.btnTextAdd)
        Me.tabText.Controls.Add(Me.txtText)
        Me.tabText.Location = New System.Drawing.Point(4, 22)
        Me.tabText.Name = "tabText"
        Me.tabText.Size = New System.Drawing.Size(460, 186)
        Me.tabText.TabIndex = 3
        Me.tabText.Text = "Text"
        Me.tabText.UseVisualStyleBackColor = True
        '
        'lblTextClickToPlace
        '
        Me.lblTextClickToPlace.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTextClickToPlace.AutoSize = True
        Me.lblTextClickToPlace.Location = New System.Drawing.Point(3, 161)
        Me.lblTextClickToPlace.Name = "lblTextClickToPlace"
        Me.lblTextClickToPlace.Size = New System.Drawing.Size(116, 13)
        Me.lblTextClickToPlace.TabIndex = 15
        Me.lblTextClickToPlace.Text = "Click on graph to place"
        '
        'btnTextRemove
        '
        Me.btnTextRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTextRemove.Location = New System.Drawing.Point(301, 156)
        Me.btnTextRemove.Name = "btnTextRemove"
        Me.btnTextRemove.Size = New System.Drawing.Size(75, 23)
        Me.btnTextRemove.TabIndex = 16
        Me.btnTextRemove.Text = "Remove"
        Me.btnTextRemove.UseVisualStyleBackColor = True
        '
        'btnTextAdd
        '
        Me.btnTextAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTextAdd.Location = New System.Drawing.Point(382, 156)
        Me.btnTextAdd.Name = "btnTextAdd"
        Me.btnTextAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnTextAdd.TabIndex = 17
        Me.btnTextAdd.Text = "Add"
        Me.btnTextAdd.UseVisualStyleBackColor = True
        '
        'txtText
        '
        Me.txtText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtText.Location = New System.Drawing.Point(3, 36)
        Me.txtText.Multiline = True
        Me.txtText.Name = "txtText"
        Me.txtText.Size = New System.Drawing.Size(454, 114)
        Me.txtText.TabIndex = 13
        '
        'chkAutoApply
        '
        Me.chkAutoApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkAutoApply.AutoSize = True
        Me.chkAutoApply.Location = New System.Drawing.Point(282, 240)
        Me.chkAutoApply.Name = "chkAutoApply"
        Me.chkAutoApply.Size = New System.Drawing.Size(117, 17)
        Me.chkAutoApply.TabIndex = 59
        Me.chkAutoApply.Text = "Apply Automatically"
        Me.chkAutoApply.UseVisualStyleBackColor = True
        '
        'frmGraphEditor
        '
        Me.AcceptButton = Me.btnApply
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 271)
        Me.Controls.Add(Me.chkAutoApply)
        Me.Controls.Add(Me.tabsCategory)
        Me.Controls.Add(Me.btnApply)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmGraphEditor"
        Me.Text = "Graph Properties"
        Me.tabsCategory.ResumeLayout(False)
        Me.tabAxes.ResumeLayout(False)
        Me.tabAxes.PerformLayout()
        Me.panelProbability.ResumeLayout(False)
        Me.panelProbability.PerformLayout()
        Me.panelAxisType.ResumeLayout(False)
        Me.panelAxisType.PerformLayout()
        Me.tabCurves.ResumeLayout(False)
        Me.tabCurves.PerformLayout()
        Me.tabLines.ResumeLayout(False)
        Me.grpRegression.ResumeLayout(False)
        Me.grpLineYconstant.ResumeLayout(False)
        Me.grpLineYconstant.PerformLayout()
        Me.grpLineEquation.ResumeLayout(False)
        Me.grpLineEquation.PerformLayout()
        Me.tabLegend.ResumeLayout(False)
        Me.tabLegend.PerformLayout()
        Me.tabText.ResumeLayout(False)
        Me.tabText.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents toolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents txtAxisMaximum As System.Windows.Forms.TextBox
    Private WithEvents txtAxisMinimum As System.Windows.Forms.TextBox
    Private WithEvents lblAxisDataRange As System.Windows.Forms.Label
    Private WithEvents txtCurveColor As System.Windows.Forms.TextBox
    Private WithEvents lblCurveColor As System.Windows.Forms.Label
    Private WithEvents comboWhichCurve As System.Windows.Forms.ComboBox
    Private WithEvents txtAxisMinorGridColor As System.Windows.Forms.TextBox
    Private WithEvents lblAxisMinorGrid As System.Windows.Forms.Label
    Private WithEvents txtAxisMajorGridColor As System.Windows.Forms.TextBox
    Private WithEvents lblAxisMajorGrid As System.Windows.Forms.Label
    Private WithEvents txtAxisDisplayMaximum As System.Windows.Forms.TextBox
    Private WithEvents txtAxisDisplayMinimum As System.Windows.Forms.TextBox
    Private WithEvents lblAxisRange As System.Windows.Forms.Label
    Private WithEvents txtCurveWidth As System.Windows.Forms.TextBox
    Private WithEvents lblAxisTo As System.Windows.Forms.Label
    Private WithEvents txtCurveLabel As System.Windows.Forms.TextBox
    Private WithEvents lblCurveLabel As System.Windows.Forms.Label
    Private WithEvents lblCurveYAxis As System.Windows.Forms.Label
    Private WithEvents lblCurve As System.Windows.Forms.Label
    Private WithEvents chkAxisMinorGridVisible As System.Windows.Forms.CheckBox
    Private WithEvents btnApply As System.Windows.Forms.Button
    Private WithEvents chkAxisMajorGridVisible As System.Windows.Forms.CheckBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents lblAxisLabel As System.Windows.Forms.Label
    Private WithEvents lblAxisType As System.Windows.Forms.Label
    Private WithEvents txtAxisLabel As System.Windows.Forms.TextBox
    Private WithEvents lblWhichAxis As System.Windows.Forms.Label
    Private WithEvents lblCurveWidth As System.Windows.Forms.Label
    Friend WithEvents tabsCategory As System.Windows.Forms.TabControl
    Friend WithEvents tabAxes As System.Windows.Forms.TabPage
    Friend WithEvents tabCurves As System.Windows.Forms.TabPage
    Private WithEvents chkAxisMinorTicsVisible As System.Windows.Forms.CheckBox
    Private WithEvents chkAxisMajorTicsVisible As System.Windows.Forms.CheckBox
    Private WithEvents chkAutoApply As System.Windows.Forms.CheckBox
    Friend WithEvents radioAxisLeft As System.Windows.Forms.RadioButton
    Friend WithEvents radioAxisBottom As System.Windows.Forms.RadioButton
    Friend WithEvents tabLines As System.Windows.Forms.TabPage
    Friend WithEvents tabText As System.Windows.Forms.TabPage
    Friend WithEvents tabLegend As System.Windows.Forms.TabPage
    Friend WithEvents lblLineYEquation As System.Windows.Forms.Label
    Friend WithEvents txtLineAcoef As System.Windows.Forms.TextBox
    Friend WithEvents lblLineXplus As System.Windows.Forms.Label
    Friend WithEvents txtLineBcoef As System.Windows.Forms.TextBox
    Private WithEvents btnLineRegressionAdd As System.Windows.Forms.Button
    Private WithEvents btnLineConstantYAdd As System.Windows.Forms.Button
    Friend WithEvents lblLineYconstant As System.Windows.Forms.Label
    Friend WithEvents txtLineYconstant As System.Windows.Forms.TextBox
    Private WithEvents btnLineEquationAdd As System.Windows.Forms.Button
    Friend WithEvents grpLineEquation As System.Windows.Forms.GroupBox
    Friend WithEvents grpLineYconstant As System.Windows.Forms.GroupBox
    Friend WithEvents grpRegression As System.Windows.Forms.GroupBox
    Friend WithEvents radioAxisRight As System.Windows.Forms.RadioButton
    Friend WithEvents radioAxisAux As System.Windows.Forms.RadioButton
    Friend WithEvents panelAxisType As System.Windows.Forms.Panel
    Friend WithEvents radioAxisProbability As System.Windows.Forms.RadioButton
    Friend WithEvents radioAxisLogarithmic As System.Windows.Forms.RadioButton
    Friend WithEvents radioAxisLinear As System.Windows.Forms.RadioButton
    Friend WithEvents radioAxisTime As System.Windows.Forms.RadioButton
    Friend WithEvents radioCurveYaxisAuxiliary As System.Windows.Forms.RadioButton
    Friend WithEvents radioCurveYaxisRight As System.Windows.Forms.RadioButton
    Friend WithEvents radioCurveYaxisLeft As System.Windows.Forms.RadioButton
    Friend WithEvents chkCurveLineVisible As System.Windows.Forms.CheckBox
    Friend WithEvents chkCurveSymbolVisible As System.Windows.Forms.CheckBox
    Private WithEvents lblCurveSymbolSize As System.Windows.Forms.Label
    Private WithEvents txtCurveSymbolSize As System.Windows.Forms.TextBox
    Friend WithEvents cboCurveSymbolType As System.Windows.Forms.ComboBox
    Private WithEvents btnTextAdd As System.Windows.Forms.Button
    Friend WithEvents txtText As System.Windows.Forms.TextBox
    Private WithEvents lblLegendClickToPlace As System.Windows.Forms.Label
    Private WithEvents comboWhichText As System.Windows.Forms.ComboBox
    Private WithEvents btnTextRemove As System.Windows.Forms.Button
    Private WithEvents btnTextFont As System.Windows.Forms.Button
    Private WithEvents lblTextClickToPlace As System.Windows.Forms.Label
    Friend WithEvents chkLegendOutline As System.Windows.Forms.CheckBox
    Private WithEvents btnLegendFont As System.Windows.Forms.Button
    Private WithEvents txtLegendFontColor As System.Windows.Forms.TextBox
    Private WithEvents txtTextColor As System.Windows.Forms.TextBox
    Friend WithEvents panelProbability As System.Windows.Forms.Panel
    Friend WithEvents radioProbablilityReturnPeriod As System.Windows.Forms.RadioButton
    Friend WithEvents radioProbablilityFraction As System.Windows.Forms.RadioButton
    Friend WithEvents radioProbablilityPercent As System.Windows.Forms.RadioButton
    Private WithEvents chkRangeReverse As System.Windows.Forms.CheckBox
    Private WithEvents txtProbabilityDeviations As System.Windows.Forms.TextBox
    Private WithEvents lblProbabilityDeviations As System.Windows.Forms.Label
    Friend WithEvents cboCurveStep As System.Windows.Forms.ComboBox
    Private WithEvents btnAxisFont As System.Windows.Forms.Button
    Private WithEvents btnScaleFont As System.Windows.Forms.Button
    Friend WithEvents chkCurveSymbolFillVisible As System.Windows.Forms.CheckBox
End Class
