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
        Me.cboCurveAxis = New System.Windows.Forms.ComboBox
        Me.cboWhichCurve = New System.Windows.Forms.ComboBox
        Me.lblAxisMinorGridColor = New System.Windows.Forms.Label
        Me.txtAxisMinorGridColor = New System.Windows.Forms.TextBox
        Me.lblAxisMinorGrid = New System.Windows.Forms.Label
        Me.txtAxisMajorGridColor = New System.Windows.Forms.TextBox
        Me.lblAxisMajorGridColor = New System.Windows.Forms.Label
        Me.lblAxisMajorGrid = New System.Windows.Forms.Label
        Me.txtAxisDisplayMaximum = New System.Windows.Forms.TextBox
        Me.txtAxisDisplayMinimum = New System.Windows.Forms.TextBox
        Me.lblAxisRange = New System.Windows.Forms.Label
        Me.cboWhichAxis = New System.Windows.Forms.ComboBox
        Me.txtCurveWidth = New System.Windows.Forms.TextBox
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
        Me.grpAxis = New System.Windows.Forms.GroupBox
        Me.lblWhichAxis = New System.Windows.Forms.Label
        Me.cboAxisType = New System.Windows.Forms.ComboBox
        Me.lblCurveWidth = New System.Windows.Forms.Label
        Me.grpCurve = New System.Windows.Forms.GroupBox
        Me.grpAxis.SuspendLayout()
        Me.grpCurve.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtAxisMaximum
        '
        Me.txtAxisMaximum.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.txtAxisMaximum.Enabled = False
        Me.txtAxisMaximum.Location = New System.Drawing.Point(201, 72)
        Me.txtAxisMaximum.Name = "txtAxisMaximum"
        Me.txtAxisMaximum.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisMaximum.TabIndex = 17
        Me.toolTip1.SetToolTip(Me.txtAxisMaximum, "Maximum data value in any dataset using this axis")
        '
        'txtAxisMinimum
        '
        Me.txtAxisMinimum.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.txtAxisMinimum.Enabled = False
        Me.txtAxisMinimum.Location = New System.Drawing.Point(81, 72)
        Me.txtAxisMinimum.Name = "txtAxisMinimum"
        Me.txtAxisMinimum.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisMinimum.TabIndex = 16
        Me.toolTip1.SetToolTip(Me.txtAxisMinimum, "Minimum data value in any dataset using this axis")
        '
        'lblAxisDataRange
        '
        Me.lblAxisDataRange.AutoSize = True
        Me.lblAxisDataRange.Location = New System.Drawing.Point(6, 75)
        Me.lblAxisDataRange.Name = "lblAxisDataRange"
        Me.lblAxisDataRange.Size = New System.Drawing.Size(65, 13)
        Me.lblAxisDataRange.TabIndex = 15
        Me.lblAxisDataRange.Text = "Data Range"
        Me.toolTip1.SetToolTip(Me.lblAxisDataRange, "Total range of all data using this axis")
        '
        'txtCurveColor
        '
        Me.txtCurveColor.BackColor = System.Drawing.Color.LightGray
        Me.txtCurveColor.Location = New System.Drawing.Point(201, 99)
        Me.txtCurveColor.Name = "txtCurveColor"
        Me.txtCurveColor.Size = New System.Drawing.Size(91, 20)
        Me.txtCurveColor.TabIndex = 28
        Me.toolTip1.SetToolTip(Me.txtCurveColor, "Color of curve")
        '
        'lblCurveColor
        '
        Me.lblCurveColor.AutoSize = True
        Me.lblCurveColor.Location = New System.Drawing.Point(164, 102)
        Me.lblCurveColor.Name = "lblCurveColor"
        Me.lblCurveColor.Size = New System.Drawing.Size(31, 13)
        Me.lblCurveColor.TabIndex = 27
        Me.lblCurveColor.Text = "Color"
        Me.toolTip1.SetToolTip(Me.lblCurveColor, "Range of data currently displayed")
        '
        'cboCurveAxis
        '
        Me.cboCurveAxis.FormattingEnabled = True
        Me.cboCurveAxis.Items.AddRange(New Object() {"Left", "Right"})
        Me.cboCurveAxis.Location = New System.Drawing.Point(81, 72)
        Me.cboCurveAxis.Name = "cboCurveAxis"
        Me.cboCurveAxis.Size = New System.Drawing.Size(94, 21)
        Me.cboCurveAxis.TabIndex = 13
        Me.cboCurveAxis.Text = "Left"
        Me.toolTip1.SetToolTip(Me.cboCurveAxis, "Which Y axis this curve is measured on")
        '
        'cboWhichCurve
        '
        Me.cboWhichCurve.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboWhichCurve.FormattingEnabled = True
        Me.cboWhichCurve.Location = New System.Drawing.Point(81, 19)
        Me.cboWhichCurve.Name = "cboWhichCurve"
        Me.cboWhichCurve.Size = New System.Drawing.Size(211, 21)
        Me.cboWhichCurve.TabIndex = 0
        Me.toolTip1.SetToolTip(Me.cboWhichCurve, "Select which curve to edit")
        '
        'lblAxisMinorGridColor
        '
        Me.lblAxisMinorGridColor.AutoSize = True
        Me.lblAxisMinorGridColor.Location = New System.Drawing.Point(164, 141)
        Me.lblAxisMinorGridColor.Name = "lblAxisMinorGridColor"
        Me.lblAxisMinorGridColor.Size = New System.Drawing.Size(31, 13)
        Me.lblAxisMinorGridColor.TabIndex = 30
        Me.lblAxisMinorGridColor.Text = "Color"
        Me.toolTip1.SetToolTip(Me.lblAxisMinorGridColor, "Range of data currently displayed")
        '
        'txtAxisMinorGridColor
        '
        Me.txtAxisMinorGridColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtAxisMinorGridColor.Location = New System.Drawing.Point(201, 138)
        Me.txtAxisMinorGridColor.Name = "txtAxisMinorGridColor"
        Me.txtAxisMinorGridColor.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisMinorGridColor.TabIndex = 29
        Me.toolTip1.SetToolTip(Me.txtAxisMinorGridColor, "Color of Major Grid")
        '
        'lblAxisMinorGrid
        '
        Me.lblAxisMinorGrid.AutoSize = True
        Me.lblAxisMinorGrid.Location = New System.Drawing.Point(6, 141)
        Me.lblAxisMinorGrid.Name = "lblAxisMinorGrid"
        Me.lblAxisMinorGrid.Size = New System.Drawing.Size(55, 13)
        Me.lblAxisMinorGrid.TabIndex = 28
        Me.lblAxisMinorGrid.Text = "Minor Grid"
        Me.toolTip1.SetToolTip(Me.lblAxisMinorGrid, "Range of data currently displayed")
        '
        'txtAxisMajorGridColor
        '
        Me.txtAxisMajorGridColor.BackColor = System.Drawing.Color.LightGray
        Me.txtAxisMajorGridColor.Location = New System.Drawing.Point(201, 116)
        Me.txtAxisMajorGridColor.Name = "txtAxisMajorGridColor"
        Me.txtAxisMajorGridColor.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisMajorGridColor.TabIndex = 26
        Me.toolTip1.SetToolTip(Me.txtAxisMajorGridColor, "Color of Major Grid")
        '
        'lblAxisMajorGridColor
        '
        Me.lblAxisMajorGridColor.AutoSize = True
        Me.lblAxisMajorGridColor.Location = New System.Drawing.Point(164, 119)
        Me.lblAxisMajorGridColor.Name = "lblAxisMajorGridColor"
        Me.lblAxisMajorGridColor.Size = New System.Drawing.Size(31, 13)
        Me.lblAxisMajorGridColor.TabIndex = 25
        Me.lblAxisMajorGridColor.Text = "Color"
        Me.toolTip1.SetToolTip(Me.lblAxisMajorGridColor, "Range of data currently displayed")
        '
        'lblAxisMajorGrid
        '
        Me.lblAxisMajorGrid.AutoSize = True
        Me.lblAxisMajorGrid.Location = New System.Drawing.Point(6, 119)
        Me.lblAxisMajorGrid.Name = "lblAxisMajorGrid"
        Me.lblAxisMajorGrid.Size = New System.Drawing.Size(55, 13)
        Me.lblAxisMajorGrid.TabIndex = 24
        Me.lblAxisMajorGrid.Text = "Major Grid"
        Me.toolTip1.SetToolTip(Me.lblAxisMajorGrid, "Range of data currently displayed")
        '
        'txtAxisDisplayMaximum
        '
        Me.txtAxisDisplayMaximum.Location = New System.Drawing.Point(201, 92)
        Me.txtAxisDisplayMaximum.Name = "txtAxisDisplayMaximum"
        Me.txtAxisDisplayMaximum.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisDisplayMaximum.TabIndex = 21
        Me.toolTip1.SetToolTip(Me.txtAxisDisplayMaximum, "Maximum value currently displayed on this axis")
        '
        'txtAxisDisplayMinimum
        '
        Me.txtAxisDisplayMinimum.Location = New System.Drawing.Point(81, 92)
        Me.txtAxisDisplayMinimum.Name = "txtAxisDisplayMinimum"
        Me.txtAxisDisplayMinimum.Size = New System.Drawing.Size(91, 20)
        Me.txtAxisDisplayMinimum.TabIndex = 20
        Me.toolTip1.SetToolTip(Me.txtAxisDisplayMinimum, "Minimum value currently displayed on this axis")
        '
        'lblAxisRange
        '
        Me.lblAxisRange.AutoSize = True
        Me.lblAxisRange.Location = New System.Drawing.Point(6, 95)
        Me.lblAxisRange.Name = "lblAxisRange"
        Me.lblAxisRange.Size = New System.Drawing.Size(69, 13)
        Me.lblAxisRange.TabIndex = 19
        Me.lblAxisRange.Text = "Zoom Range"
        Me.toolTip1.SetToolTip(Me.lblAxisRange, "Range of data currently displayed")
        '
        'cboWhichAxis
        '
        Me.cboWhichAxis.FormattingEnabled = True
        Me.cboWhichAxis.Items.AddRange(New Object() {"X Bottom", "Y Left", "Y Right", "Auxiliary"})
        Me.cboWhichAxis.Location = New System.Drawing.Point(81, 19)
        Me.cboWhichAxis.Name = "cboWhichAxis"
        Me.cboWhichAxis.Size = New System.Drawing.Size(77, 21)
        Me.cboWhichAxis.TabIndex = 9
        Me.cboWhichAxis.Text = "X Bottom"
        Me.toolTip1.SetToolTip(Me.cboWhichAxis, "Select which axis to edit")
        '
        'txtCurveWidth
        '
        Me.txtCurveWidth.Location = New System.Drawing.Point(81, 99)
        Me.txtCurveWidth.Name = "txtCurveWidth"
        Me.txtCurveWidth.Size = New System.Drawing.Size(55, 20)
        Me.txtCurveWidth.TabIndex = 29
        Me.toolTip1.SetToolTip(Me.txtCurveWidth, "Width of curve")
        '
        'lblAxisTo
        '
        Me.lblAxisTo.AutoSize = True
        Me.lblAxisTo.Location = New System.Drawing.Point(179, 75)
        Me.lblAxisTo.Name = "lblAxisTo"
        Me.lblAxisTo.Size = New System.Drawing.Size(16, 13)
        Me.lblAxisTo.TabIndex = 18
        Me.lblAxisTo.Text = "to"
        '
        'txtCurveLabel
        '
        Me.txtCurveLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCurveLabel.Location = New System.Drawing.Point(81, 46)
        Me.txtCurveLabel.Name = "txtCurveLabel"
        Me.txtCurveLabel.Size = New System.Drawing.Size(211, 20)
        Me.txtCurveLabel.TabIndex = 16
        '
        'lblCurveLabel
        '
        Me.lblCurveLabel.AutoSize = True
        Me.lblCurveLabel.Location = New System.Drawing.Point(6, 49)
        Me.lblCurveLabel.Name = "lblCurveLabel"
        Me.lblCurveLabel.Size = New System.Drawing.Size(33, 13)
        Me.lblCurveLabel.TabIndex = 15
        Me.lblCurveLabel.Text = "Label"
        '
        'lblCurveYAxis
        '
        Me.lblCurveYAxis.AutoSize = True
        Me.lblCurveYAxis.Location = New System.Drawing.Point(6, 75)
        Me.lblCurveYAxis.Name = "lblCurveYAxis"
        Me.lblCurveYAxis.Size = New System.Drawing.Size(36, 13)
        Me.lblCurveYAxis.TabIndex = 14
        Me.lblCurveYAxis.Text = "Y Axis"
        '
        'lblCurve
        '
        Me.lblCurve.AutoSize = True
        Me.lblCurve.Location = New System.Drawing.Point(6, 22)
        Me.lblCurve.Name = "lblCurve"
        Me.lblCurve.Size = New System.Drawing.Size(35, 13)
        Me.lblCurve.TabIndex = 12
        Me.lblCurve.Text = "Curve"
        '
        'chkAxisMinorGridVisible
        '
        Me.chkAxisMinorGridVisible.AutoSize = True
        Me.chkAxisMinorGridVisible.Location = New System.Drawing.Point(81, 140)
        Me.chkAxisMinorGridVisible.Name = "chkAxisMinorGridVisible"
        Me.chkAxisMinorGridVisible.Size = New System.Drawing.Size(55, 17)
        Me.chkAxisMinorGridVisible.TabIndex = 27
        Me.chkAxisMinorGridVisible.Text = "visible"
        Me.chkAxisMinorGridVisible.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.Location = New System.Drawing.Point(235, 313)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 23)
        Me.btnApply.TabIndex = 10
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'chkAxisMajorGridVisible
        '
        Me.chkAxisMajorGridVisible.AutoSize = True
        Me.chkAxisMajorGridVisible.Location = New System.Drawing.Point(81, 118)
        Me.chkAxisMajorGridVisible.Name = "chkAxisMajorGridVisible"
        Me.chkAxisMajorGridVisible.Size = New System.Drawing.Size(55, 17)
        Me.chkAxisMajorGridVisible.TabIndex = 23
        Me.chkAxisMajorGridVisible.Text = "visible"
        Me.chkAxisMajorGridVisible.UseVisualStyleBackColor = True
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(179, 95)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(16, 13)
        Me.label2.TabIndex = 22
        Me.label2.Text = "to"
        '
        'lblAxisLabel
        '
        Me.lblAxisLabel.AutoSize = True
        Me.lblAxisLabel.Location = New System.Drawing.Point(6, 49)
        Me.lblAxisLabel.Name = "lblAxisLabel"
        Me.lblAxisLabel.Size = New System.Drawing.Size(27, 13)
        Me.lblAxisLabel.TabIndex = 13
        Me.lblAxisLabel.Text = "Title"
        '
        'lblAxisType
        '
        Me.lblAxisType.AutoSize = True
        Me.lblAxisType.Location = New System.Drawing.Point(164, 22)
        Me.lblAxisType.Name = "lblAxisType"
        Me.lblAxisType.Size = New System.Drawing.Size(31, 13)
        Me.lblAxisType.TabIndex = 12
        Me.lblAxisType.Text = "Type"
        '
        'txtAxisLabel
        '
        Me.txtAxisLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAxisLabel.Location = New System.Drawing.Point(81, 46)
        Me.txtAxisLabel.Name = "txtAxisLabel"
        Me.txtAxisLabel.Size = New System.Drawing.Size(211, 20)
        Me.txtAxisLabel.TabIndex = 14
        '
        'grpAxis
        '
        Me.grpAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAxis.Controls.Add(Me.lblAxisMinorGridColor)
        Me.grpAxis.Controls.Add(Me.txtAxisMinorGridColor)
        Me.grpAxis.Controls.Add(Me.lblAxisMinorGrid)
        Me.grpAxis.Controls.Add(Me.chkAxisMinorGridVisible)
        Me.grpAxis.Controls.Add(Me.txtAxisMajorGridColor)
        Me.grpAxis.Controls.Add(Me.lblAxisMajorGridColor)
        Me.grpAxis.Controls.Add(Me.lblAxisMajorGrid)
        Me.grpAxis.Controls.Add(Me.chkAxisMajorGridVisible)
        Me.grpAxis.Controls.Add(Me.label2)
        Me.grpAxis.Controls.Add(Me.txtAxisDisplayMaximum)
        Me.grpAxis.Controls.Add(Me.txtAxisDisplayMinimum)
        Me.grpAxis.Controls.Add(Me.lblAxisRange)
        Me.grpAxis.Controls.Add(Me.lblAxisTo)
        Me.grpAxis.Controls.Add(Me.txtAxisMaximum)
        Me.grpAxis.Controls.Add(Me.txtAxisMinimum)
        Me.grpAxis.Controls.Add(Me.lblAxisDataRange)
        Me.grpAxis.Controls.Add(Me.txtAxisLabel)
        Me.grpAxis.Controls.Add(Me.lblAxisLabel)
        Me.grpAxis.Controls.Add(Me.lblAxisType)
        Me.grpAxis.Controls.Add(Me.lblWhichAxis)
        Me.grpAxis.Controls.Add(Me.cboAxisType)
        Me.grpAxis.Controls.Add(Me.cboWhichAxis)
        Me.grpAxis.Location = New System.Drawing.Point(12, 12)
        Me.grpAxis.Name = "grpAxis"
        Me.grpAxis.Size = New System.Drawing.Size(298, 165)
        Me.grpAxis.TabIndex = 8
        Me.grpAxis.TabStop = False
        Me.grpAxis.Text = "Axis Properties"
        '
        'lblWhichAxis
        '
        Me.lblWhichAxis.AutoSize = True
        Me.lblWhichAxis.Location = New System.Drawing.Point(6, 22)
        Me.lblWhichAxis.Name = "lblWhichAxis"
        Me.lblWhichAxis.Size = New System.Drawing.Size(26, 13)
        Me.lblWhichAxis.TabIndex = 11
        Me.lblWhichAxis.Text = "Axis"
        '
        'cboAxisType
        '
        Me.cboAxisType.FormattingEnabled = True
        Me.cboAxisType.Items.AddRange(New Object() {"Time", "Arithmetic", "Logarithmic", "Probability"})
        Me.cboAxisType.Location = New System.Drawing.Point(201, 19)
        Me.cboAxisType.Name = "cboAxisType"
        Me.cboAxisType.Size = New System.Drawing.Size(91, 21)
        Me.cboAxisType.TabIndex = 10
        Me.cboAxisType.Text = "Logarithmic"
        '
        'lblCurveWidth
        '
        Me.lblCurveWidth.AutoSize = True
        Me.lblCurveWidth.Location = New System.Drawing.Point(6, 102)
        Me.lblCurveWidth.Name = "lblCurveWidth"
        Me.lblCurveWidth.Size = New System.Drawing.Size(35, 13)
        Me.lblCurveWidth.TabIndex = 30
        Me.lblCurveWidth.Text = "Width"
        '
        'grpCurve
        '
        Me.grpCurve.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpCurve.Controls.Add(Me.lblCurveWidth)
        Me.grpCurve.Controls.Add(Me.txtCurveWidth)
        Me.grpCurve.Controls.Add(Me.txtCurveColor)
        Me.grpCurve.Controls.Add(Me.lblCurveColor)
        Me.grpCurve.Controls.Add(Me.txtCurveLabel)
        Me.grpCurve.Controls.Add(Me.lblCurveLabel)
        Me.grpCurve.Controls.Add(Me.lblCurveYAxis)
        Me.grpCurve.Controls.Add(Me.lblCurve)
        Me.grpCurve.Controls.Add(Me.cboCurveAxis)
        Me.grpCurve.Controls.Add(Me.cboWhichCurve)
        Me.grpCurve.Location = New System.Drawing.Point(12, 183)
        Me.grpCurve.Name = "grpCurve"
        Me.grpCurve.Size = New System.Drawing.Size(298, 126)
        Me.grpCurve.TabIndex = 9
        Me.grpCurve.TabStop = False
        Me.grpCurve.Text = "Curve Properties"
        '
        'frmGraphEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(322, 348)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.grpAxis)
        Me.Controls.Add(Me.grpCurve)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmGraphEditor"
        Me.Text = "frmGraphEditor"
        Me.grpAxis.ResumeLayout(False)
        Me.grpAxis.PerformLayout()
        Me.grpCurve.ResumeLayout(False)
        Me.grpCurve.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents toolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents txtAxisMaximum As System.Windows.Forms.TextBox
    Private WithEvents txtAxisMinimum As System.Windows.Forms.TextBox
    Private WithEvents lblAxisDataRange As System.Windows.Forms.Label
    Private WithEvents txtCurveColor As System.Windows.Forms.TextBox
    Private WithEvents lblCurveColor As System.Windows.Forms.Label
    Private WithEvents cboCurveAxis As System.Windows.Forms.ComboBox
    Private WithEvents cboWhichCurve As System.Windows.Forms.ComboBox
    Private WithEvents lblAxisMinorGridColor As System.Windows.Forms.Label
    Private WithEvents txtAxisMinorGridColor As System.Windows.Forms.TextBox
    Private WithEvents lblAxisMinorGrid As System.Windows.Forms.Label
    Private WithEvents txtAxisMajorGridColor As System.Windows.Forms.TextBox
    Private WithEvents lblAxisMajorGridColor As System.Windows.Forms.Label
    Private WithEvents lblAxisMajorGrid As System.Windows.Forms.Label
    Private WithEvents txtAxisDisplayMaximum As System.Windows.Forms.TextBox
    Private WithEvents txtAxisDisplayMinimum As System.Windows.Forms.TextBox
    Private WithEvents lblAxisRange As System.Windows.Forms.Label
    Private WithEvents cboWhichAxis As System.Windows.Forms.ComboBox
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
    Private WithEvents grpAxis As System.Windows.Forms.GroupBox
    Private WithEvents lblWhichAxis As System.Windows.Forms.Label
    Private WithEvents cboAxisType As System.Windows.Forms.ComboBox
    Private WithEvents lblCurveWidth As System.Windows.Forms.Label
    Private WithEvents grpCurve As System.Windows.Forms.GroupBox
End Class
