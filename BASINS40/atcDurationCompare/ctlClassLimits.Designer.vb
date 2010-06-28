<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlClassLimits
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.gbClassLimits = New System.Windows.Forms.GroupBox
        Me.txtMax = New System.Windows.Forms.TextBox
        Me.lblMax = New System.Windows.Forms.Label
        Me.txtMin = New System.Windows.Forms.TextBox
        Me.lblMin = New System.Windows.Forms.Label
        Me.txtNumClasses = New atcControls.atcText
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.lblNumClassLimits = New System.Windows.Forms.Label
        Me.radioLogrithmic = New System.Windows.Forms.RadioButton
        Me.radioArithmetic = New System.Windows.Forms.RadioButton
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkSWSTATDefault = New System.Windows.Forms.CheckBox
        Me.gbClassLimits.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbClassLimits
        '
        Me.gbClassLimits.Controls.Add(Me.chkSWSTATDefault)
        Me.gbClassLimits.Controls.Add(Me.txtMax)
        Me.gbClassLimits.Controls.Add(Me.lblMax)
        Me.gbClassLimits.Controls.Add(Me.txtMin)
        Me.gbClassLimits.Controls.Add(Me.lblMin)
        Me.gbClassLimits.Controls.Add(Me.txtNumClasses)
        Me.gbClassLimits.Controls.Add(Me.btnGenerate)
        Me.gbClassLimits.Controls.Add(Me.lblNumClassLimits)
        Me.gbClassLimits.Controls.Add(Me.radioLogrithmic)
        Me.gbClassLimits.Controls.Add(Me.radioArithmetic)
        Me.gbClassLimits.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbClassLimits.Location = New System.Drawing.Point(0, 0)
        Me.gbClassLimits.Name = "gbClassLimits"
        Me.gbClassLimits.Size = New System.Drawing.Size(147, 192)
        Me.gbClassLimits.TabIndex = 16
        Me.gbClassLimits.TabStop = False
        Me.gbClassLimits.Text = "Class Limits Generation"
        '
        'txtMax
        '
        Me.txtMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMax.Location = New System.Drawing.Point(58, 73)
        Me.txtMax.Name = "txtMax"
        Me.txtMax.Size = New System.Drawing.Size(83, 20)
        Me.txtMax.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.txtMax, "Upper bound is optional")
        '
        'lblMax
        '
        Me.lblMax.AutoSize = True
        Me.lblMax.Location = New System.Drawing.Point(22, 76)
        Me.lblMax.Name = "lblMax"
        Me.lblMax.Size = New System.Drawing.Size(30, 13)
        Me.lblMax.TabIndex = 8
        Me.lblMax.Text = "Max:"
        '
        'txtMin
        '
        Me.txtMin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMin.Location = New System.Drawing.Point(58, 46)
        Me.txtMin.Name = "txtMin"
        Me.txtMin.Size = New System.Drawing.Size(83, 20)
        Me.txtMin.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.txtMin, "Lower bound is optional")
        '
        'lblMin
        '
        Me.lblMin.AutoSize = True
        Me.lblMin.Location = New System.Drawing.Point(25, 49)
        Me.lblMin.Name = "lblMin"
        Me.lblMin.Size = New System.Drawing.Size(27, 13)
        Me.lblMin.TabIndex = 6
        Me.lblMin.Text = "Min:"
        '
        'txtNumClasses
        '
        Me.txtNumClasses.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtNumClasses.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNumClasses.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtNumClasses.DefaultValue = ""
        Me.txtNumClasses.HardMax = 100000
        Me.txtNumClasses.HardMin = 0
        Me.txtNumClasses.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtNumClasses.Location = New System.Drawing.Point(58, 19)
        Me.txtNumClasses.MaxWidth = 20
        Me.txtNumClasses.Name = "txtNumClasses"
        Me.txtNumClasses.NumericFormat = "0.#####"
        Me.txtNumClasses.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtNumClasses.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtNumClasses.SelLength = 0
        Me.txtNumClasses.SelStart = 0
        Me.txtNumClasses.Size = New System.Drawing.Size(83, 20)
        Me.txtNumClasses.SoftMax = 100000
        Me.txtNumClasses.SoftMin = 0
        Me.txtNumClasses.TabIndex = 5
        Me.txtNumClasses.ValueDouble = 0
        Me.txtNumClasses.ValueInteger = 0
        '
        'btnGenerate
        '
        Me.btnGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerate.Location = New System.Drawing.Point(66, 163)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerate.TabIndex = 4
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'lblNumClassLimits
        '
        Me.lblNumClassLimits.AutoSize = True
        Me.lblNumClassLimits.Location = New System.Drawing.Point(6, 22)
        Me.lblNumClassLimits.Name = "lblNumClassLimits"
        Me.lblNumClassLimits.Size = New System.Drawing.Size(47, 13)
        Me.lblNumClassLimits.TabIndex = 2
        Me.lblNumClassLimits.Text = "Number:"
        '
        'radioLogrithmic
        '
        Me.radioLogrithmic.AutoSize = True
        Me.radioLogrithmic.Location = New System.Drawing.Point(6, 128)
        Me.radioLogrithmic.Name = "radioLogrithmic"
        Me.radioLogrithmic.Size = New System.Drawing.Size(79, 17)
        Me.radioLogrithmic.TabIndex = 1
        Me.radioLogrithmic.TabStop = True
        Me.radioLogrithmic.Text = "Logarithmic"
        Me.radioLogrithmic.UseVisualStyleBackColor = True
        Me.radioLogrithmic.Visible = False
        '
        'radioArithmetic
        '
        Me.radioArithmetic.AutoSize = True
        Me.radioArithmetic.Location = New System.Drawing.Point(6, 140)
        Me.radioArithmetic.Name = "radioArithmetic"
        Me.radioArithmetic.Size = New System.Drawing.Size(71, 17)
        Me.radioArithmetic.TabIndex = 0
        Me.radioArithmetic.TabStop = True
        Me.radioArithmetic.Text = "Arithmetic"
        Me.radioArithmetic.UseVisualStyleBackColor = True
        Me.radioArithmetic.Visible = False
        '
        'chkSWSTATDefault
        '
        Me.chkSWSTATDefault.AutoSize = True
        Me.chkSWSTATDefault.Location = New System.Drawing.Point(9, 99)
        Me.chkSWSTATDefault.Name = "chkSWSTATDefault"
        Me.chkSWSTATDefault.Size = New System.Drawing.Size(109, 17)
        Me.chkSWSTATDefault.TabIndex = 11
        Me.chkSWSTATDefault.Text = "SWSTAT Default"
        Me.ToolTip1.SetToolTip(Me.chkSWSTATDefault, "Apply SWSTAT4.1 Default Class Limits")
        Me.chkSWSTATDefault.UseVisualStyleBackColor = True
        '
        'ctlClassLimits
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbClassLimits)
        Me.Name = "ctlClassLimits"
        Me.Size = New System.Drawing.Size(147, 192)
        Me.gbClassLimits.ResumeLayout(False)
        Me.gbClassLimits.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbClassLimits As System.Windows.Forms.GroupBox
    Friend WithEvents lblNumClassLimits As System.Windows.Forms.Label
    Friend WithEvents radioLogrithmic As System.Windows.Forms.RadioButton
    Friend WithEvents radioArithmetic As System.Windows.Forms.RadioButton
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents txtNumClasses As atcControls.atcText
    Friend WithEvents txtMin As System.Windows.Forms.TextBox
    Friend WithEvents lblMin As System.Windows.Forms.Label
    Friend WithEvents txtMax As System.Windows.Forms.TextBox
    Friend WithEvents lblMax As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents chkSWSTATDefault As System.Windows.Forms.CheckBox

End Class
