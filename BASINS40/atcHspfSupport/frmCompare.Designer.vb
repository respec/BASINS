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
        Me.SuspendLayout()
        '
        'lblObserved
        '
        Me.lblObserved.AutoSize = True
        Me.lblObserved.Location = New System.Drawing.Point(12, 15)
        Me.lblObserved.Name = "lblObserved"
        Me.lblObserved.Size = New System.Drawing.Size(53, 13)
        Me.lblObserved.TabIndex = 0
        Me.lblObserved.Text = "Observed"
        '
        'txtObserved
        '
        Me.txtObserved.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObserved.Location = New System.Drawing.Point(71, 12)
        Me.txtObserved.Name = "txtObserved"
        Me.txtObserved.Size = New System.Drawing.Size(200, 20)
        Me.txtObserved.TabIndex = 1
        '
        'btnObserved
        '
        Me.btnObserved.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnObserved.Location = New System.Drawing.Point(277, 12)
        Me.btnObserved.Name = "btnObserved"
        Me.btnObserved.Size = New System.Drawing.Size(30, 23)
        Me.btnObserved.TabIndex = 2
        Me.btnObserved.Text = "..."
        Me.btnObserved.UseVisualStyleBackColor = True
        '
        'btnSimulated
        '
        Me.btnSimulated.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSimulated.Location = New System.Drawing.Point(277, 38)
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
        Me.txtSimulated.Location = New System.Drawing.Point(71, 38)
        Me.txtSimulated.Name = "txtSimulated"
        Me.txtSimulated.Size = New System.Drawing.Size(200, 20)
        Me.txtSimulated.TabIndex = 4
        '
        'lblSimulated
        '
        Me.lblSimulated.AutoSize = True
        Me.lblSimulated.Location = New System.Drawing.Point(12, 41)
        Me.lblSimulated.Name = "lblSimulated"
        Me.lblSimulated.Size = New System.Drawing.Size(53, 13)
        Me.lblSimulated.TabIndex = 3
        Me.lblSimulated.Text = "Simulated"
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
        Me.DateChooser.Location = New System.Drawing.Point(12, 67)
        Me.DateChooser.Name = "DateChooser"
        Me.DateChooser.OmitAfter = 0
        Me.DateChooser.OmitBefore = 0
        Me.DateChooser.Size = New System.Drawing.Size(295, 88)
        Me.DateChooser.TabIndex = 6
        '
        'txtTitle
        '
        Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTitle.Location = New System.Drawing.Point(71, 161)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(236, 20)
        Me.txtTitle.TabIndex = 8
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(12, 164)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(27, 13)
        Me.lblTitle.TabIndex = 7
        Me.lblTitle.Text = "Title"
        '
        'lblTimeUnit
        '
        Me.lblTimeUnit.AutoSize = True
        Me.lblTimeUnit.Location = New System.Drawing.Point(12, 189)
        Me.lblTimeUnit.Name = "lblTimeUnit"
        Me.lblTimeUnit.Size = New System.Drawing.Size(52, 13)
        Me.lblTimeUnit.TabIndex = 9
        Me.lblTimeUnit.Text = "Time Unit"
        '
        'radioTUDaily
        '
        Me.radioTUDaily.AutoSize = True
        Me.radioTUDaily.Checked = True
        Me.radioTUDaily.Location = New System.Drawing.Point(71, 187)
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
        Me.radioTUMonthly.Location = New System.Drawing.Point(125, 187)
        Me.radioTUMonthly.Name = "radioTUMonthly"
        Me.radioTUMonthly.Size = New System.Drawing.Size(62, 17)
        Me.radioTUMonthly.TabIndex = 11
        Me.radioTUMonthly.Text = "Monthly"
        Me.radioTUMonthly.UseVisualStyleBackColor = True
        '
        'radioTUYearly
        '
        Me.radioTUYearly.AutoSize = True
        Me.radioTUYearly.Location = New System.Drawing.Point(193, 187)
        Me.radioTUYearly.Name = "radioTUYearly"
        Me.radioTUYearly.Size = New System.Drawing.Size(54, 17)
        Me.radioTUYearly.TabIndex = 12
        Me.radioTUYearly.Text = "Yearly"
        Me.radioTUYearly.UseVisualStyleBackColor = True
        '
        'btnCompare
        '
        Me.btnCompare.Location = New System.Drawing.Point(12, 264)
        Me.btnCompare.Name = "btnCompare"
        Me.btnCompare.Size = New System.Drawing.Size(75, 23)
        Me.btnCompare.TabIndex = 13
        Me.btnCompare.Text = "Compare"
        Me.btnCompare.UseVisualStyleBackColor = True
        '
        'frmCompare
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(319, 299)
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
End Class
