<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSpecifySubtract
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecifySubtract))
        Me.pnlButtons = New System.Windows.Forms.Panel
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.radioTsMinusNumber = New System.Windows.Forms.RadioButton
        Me.radioTS1MinusTS2 = New System.Windows.Forms.RadioButton
        Me.radioNumberMinusTS = New System.Windows.Forms.RadioButton
        Me.txtTimeseries1 = New System.Windows.Forms.TextBox
        Me.lblTimeseries1 = New System.Windows.Forms.Label
        Me.btnSelectTimeseries1 = New System.Windows.Forms.Button
        Me.btnSelectTimeseries2 = New System.Windows.Forms.Button
        Me.lblTimeseries2 = New System.Windows.Forms.Label
        Me.txtTimeseries2 = New System.Windows.Forms.TextBox
        Me.lblNumber = New System.Windows.Forms.Label
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.pnlButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.btnCancel)
        Me.pnlButtons.Controls.Add(Me.btnOk)
        Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlButtons.Location = New System.Drawing.Point(0, 234)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(693, 39)
        Me.pnlButtons.TabIndex = 16
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(601, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 24)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(515, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(80, 24)
        Me.btnOk.TabIndex = 3
        Me.btnOk.Text = "Ok"
        '
        'radioTsMinusNumber
        '
        Me.radioTsMinusNumber.AutoSize = True
        Me.radioTsMinusNumber.Checked = True
        Me.radioTsMinusNumber.Location = New System.Drawing.Point(12, 12)
        Me.radioTsMinusNumber.Name = "radioTsMinusNumber"
        Me.radioTsMinusNumber.Size = New System.Drawing.Size(126, 17)
        Me.radioTsMinusNumber.TabIndex = 19
        Me.radioTsMinusNumber.TabStop = True
        Me.radioTsMinusNumber.Text = "Time Series - Number"
        Me.radioTsMinusNumber.UseVisualStyleBackColor = True
        '
        'radioTS1MinusTS2
        '
        Me.radioTS1MinusTS2.AutoSize = True
        Me.radioTS1MinusTS2.Location = New System.Drawing.Point(12, 35)
        Me.radioTS1MinusTS2.Name = "radioTS1MinusTS2"
        Me.radioTS1MinusTS2.Size = New System.Drawing.Size(162, 17)
        Me.radioTS1MinusTS2.TabIndex = 20
        Me.radioTS1MinusTS2.Text = "Time Series 1 - Time Series 2"
        Me.radioTS1MinusTS2.UseVisualStyleBackColor = True
        '
        'radioNumberMinusTS
        '
        Me.radioNumberMinusTS.AutoSize = True
        Me.radioNumberMinusTS.Location = New System.Drawing.Point(12, 58)
        Me.radioNumberMinusTS.Name = "radioNumberMinusTS"
        Me.radioNumberMinusTS.Size = New System.Drawing.Size(126, 17)
        Me.radioNumberMinusTS.TabIndex = 21
        Me.radioNumberMinusTS.Text = "Number - Time Series"
        Me.radioNumberMinusTS.UseVisualStyleBackColor = True
        '
        'txtTimeseries1
        '
        Me.txtTimeseries1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTimeseries1.Location = New System.Drawing.Point(95, 112)
        Me.txtTimeseries1.Name = "txtTimeseries1"
        Me.txtTimeseries1.Size = New System.Drawing.Size(505, 20)
        Me.txtTimeseries1.TabIndex = 22
        '
        'lblTimeseries1
        '
        Me.lblTimeseries1.AutoSize = True
        Me.lblTimeseries1.Location = New System.Drawing.Point(12, 115)
        Me.lblTimeseries1.Name = "lblTimeseries1"
        Me.lblTimeseries1.Size = New System.Drawing.Size(62, 13)
        Me.lblTimeseries1.TabIndex = 23
        Me.lblTimeseries1.Text = "Time Series"
        '
        'btnSelectTimeseries1
        '
        Me.btnSelectTimeseries1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectTimeseries1.Location = New System.Drawing.Point(606, 110)
        Me.btnSelectTimeseries1.Name = "btnSelectTimeseries1"
        Me.btnSelectTimeseries1.Size = New System.Drawing.Size(75, 23)
        Me.btnSelectTimeseries1.TabIndex = 24
        Me.btnSelectTimeseries1.Text = "Select"
        Me.btnSelectTimeseries1.UseVisualStyleBackColor = True
        '
        'btnSelectTimeseries2
        '
        Me.btnSelectTimeseries2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectTimeseries2.Location = New System.Drawing.Point(606, 139)
        Me.btnSelectTimeseries2.Name = "btnSelectTimeseries2"
        Me.btnSelectTimeseries2.Size = New System.Drawing.Size(75, 23)
        Me.btnSelectTimeseries2.TabIndex = 27
        Me.btnSelectTimeseries2.Text = "Select"
        Me.btnSelectTimeseries2.UseVisualStyleBackColor = True
        Me.btnSelectTimeseries2.Visible = False
        '
        'lblTimeseries2
        '
        Me.lblTimeseries2.AutoSize = True
        Me.lblTimeseries2.Location = New System.Drawing.Point(12, 144)
        Me.lblTimeseries2.Name = "lblTimeseries2"
        Me.lblTimeseries2.Size = New System.Drawing.Size(71, 13)
        Me.lblTimeseries2.TabIndex = 26
        Me.lblTimeseries2.Text = "Time Series 2"
        Me.lblTimeseries2.Visible = False
        '
        'txtTimeseries2
        '
        Me.txtTimeseries2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTimeseries2.Location = New System.Drawing.Point(95, 141)
        Me.txtTimeseries2.Name = "txtTimeseries2"
        Me.txtTimeseries2.Size = New System.Drawing.Size(505, 20)
        Me.txtTimeseries2.TabIndex = 25
        Me.txtTimeseries2.Visible = False
        '
        'lblNumber
        '
        Me.lblNumber.AutoSize = True
        Me.lblNumber.Location = New System.Drawing.Point(12, 170)
        Me.lblNumber.Name = "lblNumber"
        Me.lblNumber.Size = New System.Drawing.Size(44, 13)
        Me.lblNumber.TabIndex = 29
        Me.lblNumber.Text = "Number"
        '
        'txtNumber
        '
        Me.txtNumber.Location = New System.Drawing.Point(95, 167)
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.Size = New System.Drawing.Size(129, 20)
        Me.txtNumber.TabIndex = 28
        '
        'frmSpecifySubtract
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(693, 273)
        Me.Controls.Add(Me.lblNumber)
        Me.Controls.Add(Me.txtNumber)
        Me.Controls.Add(Me.btnSelectTimeseries2)
        Me.Controls.Add(Me.lblTimeseries2)
        Me.Controls.Add(Me.txtTimeseries2)
        Me.Controls.Add(Me.btnSelectTimeseries1)
        Me.Controls.Add(Me.lblTimeseries1)
        Me.Controls.Add(Me.txtTimeseries1)
        Me.Controls.Add(Me.radioNumberMinusTS)
        Me.Controls.Add(Me.radioTS1MinusTS2)
        Me.Controls.Add(Me.radioTsMinusNumber)
        Me.Controls.Add(Me.pnlButtons)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSpecifySubtract"
        Me.Text = "Subtract"
        Me.pnlButtons.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents radioTsMinusNumber As System.Windows.Forms.RadioButton
    Friend WithEvents radioTS1MinusTS2 As System.Windows.Forms.RadioButton
    Friend WithEvents radioNumberMinusTS As System.Windows.Forms.RadioButton
    Friend WithEvents txtTimeseries1 As System.Windows.Forms.TextBox
    Friend WithEvents lblTimeseries1 As System.Windows.Forms.Label
    Friend WithEvents btnSelectTimeseries1 As System.Windows.Forms.Button
    Friend WithEvents btnSelectTimeseries2 As System.Windows.Forms.Button
    Friend WithEvents lblTimeseries2 As System.Windows.Forms.Label
    Friend WithEvents txtTimeseries2 As System.Windows.Forms.TextBox
    Friend WithEvents lblNumber As System.Windows.Forms.Label
    Friend WithEvents txtNumber As System.Windows.Forms.TextBox
End Class
