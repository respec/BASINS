<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAttributes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAttributes))
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnNdayAdd = New System.Windows.Forms.Button
        Me.txtNday = New System.Windows.Forms.TextBox
        Me.btnDefaults = New System.Windows.Forms.Button
        Me.lstAttributes = New System.Windows.Forms.ListBox
        Me.radioHigh = New System.Windows.Forms.RadioButton
        Me.radioLow = New System.Windows.Forms.RadioButton
        Me.lblNDay = New System.Windows.Forms.Label
        Me.grpNday = New System.Windows.Forms.GroupBox
        Me.txtReturnPeriod = New System.Windows.Forms.TextBox
        Me.lblReturn = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.grpPercentile = New System.Windows.Forms.GroupBox
        Me.btnPercentileAdd = New System.Windows.Forms.Button
        Me.txtPercentile = New System.Windows.Forms.TextBox
        Me.lblPercentile = New System.Windows.Forms.Label
        Me.grpNday.SuspendLayout()
        Me.grpPercentile.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.AutoSize = True
        Me.btnRemove.Location = New System.Drawing.Point(12, 517)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(118, 23)
        Me.btnRemove.TabIndex = 30
        Me.btnRemove.Text = "Remove Selected"
        '
        'btnNdayAdd
        '
        Me.btnNdayAdd.Location = New System.Drawing.Point(194, 38)
        Me.btnNdayAdd.Name = "btnNdayAdd"
        Me.btnNdayAdd.Size = New System.Drawing.Size(43, 24)
        Me.btnNdayAdd.TabIndex = 29
        Me.btnNdayAdd.Text = "Add"
        '
        'txtNday
        '
        Me.txtNday.Location = New System.Drawing.Point(9, 41)
        Me.txtNday.Name = "txtNday"
        Me.txtNday.Size = New System.Drawing.Size(39, 20)
        Me.txtNday.TabIndex = 28
        '
        'btnDefaults
        '
        Me.btnDefaults.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDefaults.Location = New System.Drawing.Point(136, 516)
        Me.btnDefaults.Name = "btnDefaults"
        Me.btnDefaults.Size = New System.Drawing.Size(118, 24)
        Me.btnDefaults.TabIndex = 31
        Me.btnDefaults.Text = "Reset to Defaults"
        '
        'lstAttributes
        '
        Me.lstAttributes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstAttributes.IntegralHeight = False
        Me.lstAttributes.Location = New System.Drawing.Point(12, 12)
        Me.lstAttributes.Name = "lstAttributes"
        Me.lstAttributes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstAttributes.Size = New System.Drawing.Size(308, 357)
        Me.lstAttributes.TabIndex = 27
        Me.lstAttributes.Tag = "NDay"
        '
        'radioHigh
        '
        Me.radioHigh.AutoSize = True
        Me.radioHigh.Checked = True
        Me.radioHigh.Location = New System.Drawing.Point(63, 23)
        Me.radioHigh.Name = "radioHigh"
        Me.radioHigh.Size = New System.Drawing.Size(47, 17)
        Me.radioHigh.TabIndex = 34
        Me.radioHigh.TabStop = True
        Me.radioHigh.Text = "High"
        Me.radioHigh.UseVisualStyleBackColor = True
        '
        'radioLow
        '
        Me.radioLow.AutoSize = True
        Me.radioLow.Location = New System.Drawing.Point(63, 46)
        Me.radioLow.Name = "radioLow"
        Me.radioLow.Size = New System.Drawing.Size(45, 17)
        Me.radioLow.TabIndex = 35
        Me.radioLow.Text = "Low"
        Me.radioLow.UseVisualStyleBackColor = True
        '
        'lblNDay
        '
        Me.lblNDay.AutoSize = True
        Me.lblNDay.Location = New System.Drawing.Point(6, 23)
        Me.lblNDay.Name = "lblNDay"
        Me.lblNDay.Size = New System.Drawing.Size(31, 13)
        Me.lblNDay.TabIndex = 33
        Me.lblNDay.Text = "Days"
        '
        'grpNday
        '
        Me.grpNday.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpNday.Controls.Add(Me.txtReturnPeriod)
        Me.grpNday.Controls.Add(Me.lblReturn)
        Me.grpNday.Controls.Add(Me.btnNdayAdd)
        Me.grpNday.Controls.Add(Me.radioHigh)
        Me.grpNday.Controls.Add(Me.txtNday)
        Me.grpNday.Controls.Add(Me.radioLow)
        Me.grpNday.Controls.Add(Me.lblNDay)
        Me.grpNday.Location = New System.Drawing.Point(12, 375)
        Me.grpNday.Name = "grpNday"
        Me.grpNday.Size = New System.Drawing.Size(250, 74)
        Me.grpNday.TabIndex = 37
        Me.grpNday.TabStop = False
        Me.grpNday.Text = "Add N-Day Attribute"
        '
        'txtReturnPeriod
        '
        Me.txtReturnPeriod.Location = New System.Drawing.Point(119, 41)
        Me.txtReturnPeriod.Name = "txtReturnPeriod"
        Me.txtReturnPeriod.Size = New System.Drawing.Size(69, 20)
        Me.txtReturnPeriod.TabIndex = 36
        '
        'lblReturn
        '
        Me.lblReturn.AutoSize = True
        Me.lblReturn.Location = New System.Drawing.Point(116, 23)
        Me.lblReturn.Name = "lblReturn"
        Me.lblReturn.Size = New System.Drawing.Size(72, 13)
        Me.lblReturn.TabIndex = 37
        Me.lblReturn.Text = "Return Period"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(260, 516)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(61, 24)
        Me.btnOk.TabIndex = 38
        Me.btnOk.Text = "Ok"
        '
        'grpPercentile
        '
        Me.grpPercentile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpPercentile.Controls.Add(Me.btnPercentileAdd)
        Me.grpPercentile.Controls.Add(Me.txtPercentile)
        Me.grpPercentile.Controls.Add(Me.lblPercentile)
        Me.grpPercentile.Location = New System.Drawing.Point(12, 455)
        Me.grpPercentile.Name = "grpPercentile"
        Me.grpPercentile.Size = New System.Drawing.Size(250, 55)
        Me.grpPercentile.TabIndex = 39
        Me.grpPercentile.TabStop = False
        Me.grpPercentile.Text = "Add Percentile Attribute"
        '
        'btnPercentileAdd
        '
        Me.btnPercentileAdd.Location = New System.Drawing.Point(194, 17)
        Me.btnPercentileAdd.Name = "btnPercentileAdd"
        Me.btnPercentileAdd.Size = New System.Drawing.Size(43, 24)
        Me.btnPercentileAdd.TabIndex = 29
        Me.btnPercentileAdd.Text = "Add"
        '
        'txtPercentile
        '
        Me.txtPercentile.Location = New System.Drawing.Point(71, 20)
        Me.txtPercentile.Name = "txtPercentile"
        Me.txtPercentile.Size = New System.Drawing.Size(39, 20)
        Me.txtPercentile.TabIndex = 28
        '
        'lblPercentile
        '
        Me.lblPercentile.AutoSize = True
        Me.lblPercentile.Location = New System.Drawing.Point(6, 23)
        Me.lblPercentile.Name = "lblPercentile"
        Me.lblPercentile.Size = New System.Drawing.Size(54, 13)
        Me.lblPercentile.TabIndex = 33
        Me.lblPercentile.Text = "Percentile"
        '
        'frmAttributes
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(332, 552)
        Me.Controls.Add(Me.grpPercentile)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grpNday)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnDefaults)
        Me.Controls.Add(Me.lstAttributes)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(340, 240)
        Me.Name = "frmAttributes"
        Me.Text = "Attributes"
        Me.grpNday.ResumeLayout(False)
        Me.grpNday.PerformLayout()
        Me.grpPercentile.ResumeLayout(False)
        Me.grpPercentile.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnNdayAdd As System.Windows.Forms.Button
    Friend WithEvents txtNday As System.Windows.Forms.TextBox
    Friend WithEvents btnDefaults As System.Windows.Forms.Button
    Friend WithEvents lstAttributes As System.Windows.Forms.ListBox
    Friend WithEvents radioHigh As System.Windows.Forms.RadioButton
    Friend WithEvents radioLow As System.Windows.Forms.RadioButton
    Friend WithEvents lblNDay As System.Windows.Forms.Label
    Friend WithEvents grpNday As System.Windows.Forms.GroupBox
    Friend WithEvents txtReturnPeriod As System.Windows.Forms.TextBox
    Friend WithEvents lblReturn As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grpPercentile As System.Windows.Forms.GroupBox
    Friend WithEvents btnPercentileAdd As System.Windows.Forms.Button
    Friend WithEvents txtPercentile As System.Windows.Forms.TextBox
    Friend WithEvents lblPercentile As System.Windows.Forms.Label
End Class
