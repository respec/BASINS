<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCompare
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCompare))
        Me.lblObserved = New System.Windows.Forms.Label
        Me.txtObserved = New System.Windows.Forms.TextBox
        Me.btnObserved = New System.Windows.Forms.Button
        Me.btnSimulated = New System.Windows.Forms.Button
        Me.txtSimulated = New System.Windows.Forms.TextBox
        Me.lblSimulated = New System.Windows.Forms.Label
        Me.DateChooser = New atcData.atcChooseDataGroupDates
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblTimeUnit = New System.Windows.Forms.Label
        Me.radioTUDaily = New System.Windows.Forms.RadioButton
        Me.radioTUMonthly = New System.Windows.Forms.RadioButton
        Me.radioTUYearly = New System.Windows.Forms.RadioButton
        Me.btnCompare = New System.Windows.Forms.Button
        Me.lblHelp = New System.Windows.Forms.Button
        Me.gbClassLimits = New System.Windows.Forms.GroupBox
        Me.radioArithmetic = New System.Windows.Forms.RadioButton
        Me.radioLogrithmic = New System.Windows.Forms.RadioButton
        Me.lblNumClassLimits = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.gbClassLimits.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblObserved
        '
        Me.lblObserved.AutoSize = True
        Me.lblObserved.Location = New System.Drawing.Point(12, 58)
        Me.lblObserved.Name = "lblObserved"
        Me.lblObserved.Size = New System.Drawing.Size(95, 13)
        Me.lblObserved.TabIndex = 0
        Me.lblObserved.Text = "Observed (or DS1)"
        '
        'txtObserved
        '
        Me.txtObserved.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObserved.Location = New System.Drawing.Point(125, 55)
        Me.txtObserved.Name = "txtObserved"
        Me.txtObserved.Size = New System.Drawing.Size(451, 20)
        Me.txtObserved.TabIndex = 1
        '
        'btnObserved
        '
        Me.btnObserved.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnObserved.Location = New System.Drawing.Point(582, 55)
        Me.btnObserved.Name = "btnObserved"
        Me.btnObserved.Size = New System.Drawing.Size(30, 23)
        Me.btnObserved.TabIndex = 2
        Me.btnObserved.Text = "..."
        Me.btnObserved.UseVisualStyleBackColor = True
        '
        'btnSimulated
        '
        Me.btnSimulated.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSimulated.Location = New System.Drawing.Point(582, 81)
        Me.btnSimulated.Name = "btnSimulated"
        Me.btnSimulated.Size = New System.Drawing.Size(30, 23)
        Me.btnSimulated.TabIndex = 5
        Me.btnSimulated.Text = "..."
        Me.btnSimulated.UseVisualStyleBackColor = True
        '
        'txtSimulated
        '
        Me.txtSimulated.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSimulated.Location = New System.Drawing.Point(125, 81)
        Me.txtSimulated.Name = "txtSimulated"
        Me.txtSimulated.Size = New System.Drawing.Size(451, 20)
        Me.txtSimulated.TabIndex = 4
        '
        'lblSimulated
        '
        Me.lblSimulated.AutoSize = True
        Me.lblSimulated.Location = New System.Drawing.Point(12, 84)
        Me.lblSimulated.Name = "lblSimulated"
        Me.lblSimulated.Size = New System.Drawing.Size(95, 13)
        Me.lblSimulated.TabIndex = 3
        Me.lblSimulated.Text = "Simulated (or DS2)"
        '
        'DateChooser
        '
        Me.DateChooser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateChooser.CommonEnd = 1.7976931348623157E+308
        Me.DateChooser.CommonStart = -1.7976931348623157E+308
        Me.DateChooser.DataGroup = Nothing
        Me.DateChooser.FirstStart = Double.NaN
        Me.DateChooser.LastEnd = Double.NaN
        Me.DateChooser.Location = New System.Drawing.Point(12, 110)
        Me.DateChooser.Name = "DateChooser"
        Me.DateChooser.OmitAfter = 0
        Me.DateChooser.OmitBefore = 0
        Me.DateChooser.Size = New System.Drawing.Size(600, 88)
        Me.DateChooser.TabIndex = 6
        '
        'txtTitle
        '
        Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTitle.Location = New System.Drawing.Point(125, 12)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(487, 20)
        Me.txtTitle.TabIndex = 8
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(12, 15)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(62, 13)
        Me.lblTitle.TabIndex = 7
        Me.lblTitle.Text = "Report Title"
        '
        'lblTimeUnit
        '
        Me.lblTimeUnit.AutoSize = True
        Me.lblTimeUnit.Location = New System.Drawing.Point(12, 232)
        Me.lblTimeUnit.Name = "lblTimeUnit"
        Me.lblTimeUnit.Size = New System.Drawing.Size(52, 13)
        Me.lblTimeUnit.TabIndex = 9
        Me.lblTimeUnit.Text = "Time Unit"
        '
        'radioTUDaily
        '
        Me.radioTUDaily.AutoSize = True
        Me.radioTUDaily.Checked = True
        Me.radioTUDaily.Location = New System.Drawing.Point(71, 230)
        Me.radioTUDaily.Name = "radioTUDaily"
        Me.radioTUDaily.Size = New System.Drawing.Size(48, 17)
        Me.radioTUDaily.TabIndex = 10
        Me.radioTUDaily.TabStop = True
        Me.radioTUDaily.Text = "Daily"
        Me.radioTUDaily.UseVisualStyleBackColor = True
        '
        'radioTUMonthly
        '
        Me.radioTUMonthly.AutoSize = True
        Me.radioTUMonthly.Location = New System.Drawing.Point(125, 230)
        Me.radioTUMonthly.Name = "radioTUMonthly"
        Me.radioTUMonthly.Size = New System.Drawing.Size(62, 17)
        Me.radioTUMonthly.TabIndex = 11
        Me.radioTUMonthly.Text = "Monthly"
        Me.radioTUMonthly.UseVisualStyleBackColor = True
        '
        'radioTUYearly
        '
        Me.radioTUYearly.AutoSize = True
        Me.radioTUYearly.Location = New System.Drawing.Point(193, 230)
        Me.radioTUYearly.Name = "radioTUYearly"
        Me.radioTUYearly.Size = New System.Drawing.Size(54, 17)
        Me.radioTUYearly.TabIndex = 12
        Me.radioTUYearly.Text = "Yearly"
        Me.radioTUYearly.UseVisualStyleBackColor = True
        '
        'btnCompare
        '
        Me.btnCompare.Location = New System.Drawing.Point(12, 442)
        Me.btnCompare.Name = "btnCompare"
        Me.btnCompare.Size = New System.Drawing.Size(75, 23)
        Me.btnCompare.TabIndex = 13
        Me.btnCompare.Text = "Compare"
        Me.btnCompare.UseVisualStyleBackColor = True
        '
        'lblHelp
        '
        Me.lblHelp.Location = New System.Drawing.Point(306, 442)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(75, 23)
        Me.lblHelp.TabIndex = 14
        Me.lblHelp.Text = "Help"
        Me.lblHelp.UseVisualStyleBackColor = True
        '
        'gbClassLimits
        '
        Me.gbClassLimits.Controls.Add(Me.TextBox3)
        Me.gbClassLimits.Controls.Add(Me.TextBox2)
        Me.gbClassLimits.Controls.Add(Me.TextBox1)
        Me.gbClassLimits.Controls.Add(Me.lblNumClassLimits)
        Me.gbClassLimits.Controls.Add(Me.radioLogrithmic)
        Me.gbClassLimits.Controls.Add(Me.radioArithmetic)
        Me.gbClassLimits.Location = New System.Drawing.Point(34, 270)
        Me.gbClassLimits.Name = "gbClassLimits"
        Me.gbClassLimits.Size = New System.Drawing.Size(257, 156)
        Me.gbClassLimits.TabIndex = 15
        Me.gbClassLimits.TabStop = False
        Me.gbClassLimits.Text = "Class Limits"
        '
        'radioArithmetic
        '
        Me.radioArithmetic.AutoSize = True
        Me.radioArithmetic.Location = New System.Drawing.Point(14, 32)
        Me.radioArithmetic.Name = "radioArithmetic"
        Me.radioArithmetic.Size = New System.Drawing.Size(71, 17)
        Me.radioArithmetic.TabIndex = 0
        Me.radioArithmetic.TabStop = True
        Me.radioArithmetic.Text = "Arithmetic"
        Me.radioArithmetic.UseVisualStyleBackColor = True
        '
        'radioLogrithmic
        '
        Me.radioLogrithmic.AutoSize = True
        Me.radioLogrithmic.Location = New System.Drawing.Point(14, 55)
        Me.radioLogrithmic.Name = "radioLogrithmic"
        Me.radioLogrithmic.Size = New System.Drawing.Size(79, 17)
        Me.radioLogrithmic.TabIndex = 1
        Me.radioLogrithmic.TabStop = True
        Me.radioLogrithmic.Text = "Logarithmic"
        Me.radioLogrithmic.UseVisualStyleBackColor = True
        '
        'lblNumClassLimits
        '
        Me.lblNumClassLimits.AutoSize = True
        Me.lblNumClassLimits.Location = New System.Drawing.Point(14, 79)
        Me.lblNumClassLimits.Name = "lblNumClassLimits"
        Me.lblNumClassLimits.Size = New System.Drawing.Size(113, 13)
        Me.lblNumClassLimits.TabIndex = 2
        Me.lblNumClassLimits.Text = "Number of Class Limits"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(134, 79)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 20)
        Me.TextBox1.TabIndex = 3
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(134, 105)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(100, 20)
        Me.TextBox2.TabIndex = 4
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(134, 130)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(100, 20)
        Me.TextBox3.TabIndex = 5
        '
        'frmCompare
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 501)
        Me.Controls.Add(Me.gbClassLimits)
        Me.Controls.Add(Me.lblHelp)
        Me.Controls.Add(Me.btnCompare)
        Me.Controls.Add(Me.radioTUYearly)
        Me.Controls.Add(Me.radioTUMonthly)
        Me.Controls.Add(Me.radioTUDaily)
        Me.Controls.Add(Me.lblTimeUnit)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.DateChooser)
        Me.Controls.Add(Me.btnSimulated)
        Me.Controls.Add(Me.txtSimulated)
        Me.Controls.Add(Me.lblSimulated)
        Me.Controls.Add(Me.btnObserved)
        Me.Controls.Add(Me.txtObserved)
        Me.Controls.Add(Me.lblObserved)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmCompare"
        Me.Text = "Compare"
        Me.gbClassLimits.ResumeLayout(False)
        Me.gbClassLimits.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblObserved As System.Windows.Forms.Label
    Friend WithEvents txtObserved As System.Windows.Forms.TextBox
    Friend WithEvents btnObserved As System.Windows.Forms.Button
    Friend WithEvents btnSimulated As System.Windows.Forms.Button
    Friend WithEvents txtSimulated As System.Windows.Forms.TextBox
    Friend WithEvents lblSimulated As System.Windows.Forms.Label
    Friend WithEvents DateChooser As atcData.atcChooseDataGroupDates
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblTimeUnit As System.Windows.Forms.Label
    Friend WithEvents radioTUDaily As System.Windows.Forms.RadioButton
    Friend WithEvents radioTUMonthly As System.Windows.Forms.RadioButton
    Friend WithEvents radioTUYearly As System.Windows.Forms.RadioButton
    Friend WithEvents btnCompare As System.Windows.Forms.Button
    Friend WithEvents lblHelp As System.Windows.Forms.Button
    Friend WithEvents gbClassLimits As System.Windows.Forms.GroupBox
    Friend WithEvents radioLogrithmic As System.Windows.Forms.RadioButton
    Friend WithEvents radioArithmetic As System.Windows.Forms.RadioButton
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents lblNumClassLimits As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
End Class
