<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPollutant
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
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cbxSelect = New System.Windows.Forms.CheckBox
        Me.clbPollutants = New System.Windows.Forms.CheckedListBox
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(198, 338)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(135, 32)
        Me.cmdCancel.TabIndex = 24
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(43, 338)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(135, 32)
        Me.cmdOK.TabIndex = 23
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cbxSelect
        '
        Me.cbxSelect.AutoSize = True
        Me.cbxSelect.Checked = True
        Me.cbxSelect.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxSelect.Location = New System.Drawing.Point(12, 20)
        Me.cbxSelect.Name = "cbxSelect"
        Me.cbxSelect.Size = New System.Drawing.Size(144, 21)
        Me.cbxSelect.TabIndex = 27
        Me.cbxSelect.Text = "Select/Deselect All"
        Me.cbxSelect.UseVisualStyleBackColor = True
        '
        'clbPollutants
        '
        Me.clbPollutants.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clbPollutants.CheckOnClick = True
        Me.clbPollutants.FormattingEnabled = True
        Me.clbPollutants.Location = New System.Drawing.Point(27, 47)
        Me.clbPollutants.Name = "clbPollutants"
        Me.clbPollutants.Size = New System.Drawing.Size(318, 276)
        Me.clbPollutants.TabIndex = 26
        '
        'frmPollutant
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(372, 385)
        Me.Controls.Add(Me.cbxSelect)
        Me.Controls.Add(Me.clbPollutants)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmPollutant"
        Me.Text = "WinHSPF -Pollutant Selection"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cbxSelect As System.Windows.Forms.CheckBox
    Friend WithEvents clbPollutants As System.Windows.Forms.CheckedListBox
End Class
