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
        Me.gbClassLimits = New System.Windows.Forms.GroupBox
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.lblNumClassLimits = New System.Windows.Forms.Label
        Me.radioLogrithmic = New System.Windows.Forms.RadioButton
        Me.radioArithmetic = New System.Windows.Forms.RadioButton
        Me.txtNumClasses = New atcControls.atcText
        Me.gbClassLimits.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbClassLimits
        '
        Me.gbClassLimits.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbClassLimits.Controls.Add(Me.txtNumClasses)
        Me.gbClassLimits.Controls.Add(Me.btnGenerate)
        Me.gbClassLimits.Controls.Add(Me.lblNumClassLimits)
        Me.gbClassLimits.Controls.Add(Me.radioLogrithmic)
        Me.gbClassLimits.Controls.Add(Me.radioArithmetic)
        Me.gbClassLimits.Location = New System.Drawing.Point(3, 3)
        Me.gbClassLimits.Name = "gbClassLimits"
        Me.gbClassLimits.Size = New System.Drawing.Size(139, 186)
        Me.gbClassLimits.TabIndex = 16
        Me.gbClassLimits.TabStop = False
        Me.gbClassLimits.Text = "Class Limits Generation"
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(58, 157)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerate.TabIndex = 4
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'lblNumClassLimits
        '
        Me.lblNumClassLimits.AutoSize = True
        Me.lblNumClassLimits.Location = New System.Drawing.Point(6, 21)
        Me.lblNumClassLimits.Name = "lblNumClassLimits"
        Me.lblNumClassLimits.Size = New System.Drawing.Size(47, 13)
        Me.lblNumClassLimits.TabIndex = 2
        Me.lblNumClassLimits.Text = "Number:"
        '
        'radioLogrithmic
        '
        Me.radioLogrithmic.AutoSize = True
        Me.radioLogrithmic.Location = New System.Drawing.Point(6, 42)
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
        Me.radioArithmetic.Location = New System.Drawing.Point(6, 19)
        Me.radioArithmetic.Name = "radioArithmetic"
        Me.radioArithmetic.Size = New System.Drawing.Size(71, 17)
        Me.radioArithmetic.TabIndex = 0
        Me.radioArithmetic.TabStop = True
        Me.radioArithmetic.Text = "Arithmetic"
        Me.radioArithmetic.UseVisualStyleBackColor = True
        Me.radioArithmetic.Visible = False
        '
        'txtNumClasses
        '
        Me.txtNumClasses.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtNumClasses.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtNumClasses.DefaultValue = ""
        Me.txtNumClasses.HardMax = 100000
        Me.txtNumClasses.HardMin = 0
        Me.txtNumClasses.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtNumClasses.Location = New System.Drawing.Point(58, 16)
        Me.txtNumClasses.MaxWidth = 20
        Me.txtNumClasses.Name = "txtNumClasses"
        Me.txtNumClasses.NumericFormat = "0.#####"
        Me.txtNumClasses.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtNumClasses.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtNumClasses.SelLength = 0
        Me.txtNumClasses.SelStart = 0
        Me.txtNumClasses.Size = New System.Drawing.Size(75, 20)
        Me.txtNumClasses.SoftMax = 100000
        Me.txtNumClasses.SoftMin = 0
        Me.txtNumClasses.TabIndex = 5
        Me.txtNumClasses.ValueDouble = 0
        Me.txtNumClasses.ValueInteger = 0
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

End Class
