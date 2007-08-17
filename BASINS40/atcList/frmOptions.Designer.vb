<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
        Me.grpboxDateFormat = New System.Windows.Forms.GroupBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grpboxNumberFormat = New System.Windows.Forms.GroupBox
        Me.lblDecPla = New System.Windows.Forms.Label
        Me.lblSigDig = New System.Windows.Forms.Label
        Me.txtDecPla = New atcControls.atcText
        Me.txtSigDig = New atcControls.atcText
        Me.grpboxNumberFormat.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpboxDateFormat
        '
        Me.grpboxDateFormat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpboxDateFormat.Location = New System.Drawing.Point(12, 12)
        Me.grpboxDateFormat.Name = "grpboxDateFormat"
        Me.grpboxDateFormat.Size = New System.Drawing.Size(268, 117)
        Me.grpboxDateFormat.TabIndex = 1
        Me.grpboxDateFormat.TabStop = False
        Me.grpboxDateFormat.Text = "Date Format"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(59, 233)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(160, 233)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpboxNumberFormat
        '
        Me.grpboxNumberFormat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpboxNumberFormat.Controls.Add(Me.lblDecPla)
        Me.grpboxNumberFormat.Controls.Add(Me.lblSigDig)
        Me.grpboxNumberFormat.Controls.Add(Me.txtDecPla)
        Me.grpboxNumberFormat.Controls.Add(Me.txtSigDig)
        Me.grpboxNumberFormat.Location = New System.Drawing.Point(12, 135)
        Me.grpboxNumberFormat.Name = "grpboxNumberFormat"
        Me.grpboxNumberFormat.Size = New System.Drawing.Size(269, 84)
        Me.grpboxNumberFormat.TabIndex = 4
        Me.grpboxNumberFormat.TabStop = False
        Me.grpboxNumberFormat.Text = "Number Format"
        '
        'lblDecPla
        '
        Me.lblDecPla.AutoSize = True
        Me.lblDecPla.Location = New System.Drawing.Point(22, 45)
        Me.lblDecPla.Name = "lblDecPla"
        Me.lblDecPla.Size = New System.Drawing.Size(109, 17)
        Me.lblDecPla.TabIndex = 3
        Me.lblDecPla.Text = "Decimat Places:"
        Me.lblDecPla.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblSigDig
        '
        Me.lblSigDig.AutoSize = True
        Me.lblSigDig.Location = New System.Drawing.Point(15, 21)
        Me.lblSigDig.Name = "lblSigDig"
        Me.lblSigDig.Size = New System.Drawing.Size(116, 17)
        Me.lblSigDig.TabIndex = 2
        Me.lblSigDig.Text = "Significant Digits:"
        Me.lblSigDig.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDecPla
        '
        Me.txtDecPla.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtDecPla.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtDecPla.DefaultValue = 0
        Me.txtDecPla.HardMax = 8
        Me.txtDecPla.HardMin = 0
        Me.txtDecPla.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtDecPla.Location = New System.Drawing.Point(133, 45)
        Me.txtDecPla.MaxDecimal = 0
        Me.txtDecPla.maxWidth = 0
        Me.txtDecPla.Name = "txtDecPla"
        Me.txtDecPla.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtDecPla.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtDecPla.SelLength = 0
        Me.txtDecPla.SelStart = 1
        Me.txtDecPla.Size = New System.Drawing.Size(38, 19)
        Me.txtDecPla.SoftMax = 8
        Me.txtDecPla.SoftMin = 0
        Me.txtDecPla.TabIndex = 1
        Me.txtDecPla.Value = CType(0, Long)
        '
        'txtSigDig
        '
        Me.txtSigDig.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtSigDig.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtSigDig.DefaultValue = 1
        Me.txtSigDig.HardMax = 10
        Me.txtSigDig.HardMin = 1
        Me.txtSigDig.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtSigDig.Location = New System.Drawing.Point(133, 20)
        Me.txtSigDig.MaxDecimal = 0
        Me.txtSigDig.maxWidth = 10
        Me.txtSigDig.Name = "txtSigDig"
        Me.txtSigDig.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtSigDig.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtSigDig.SelLength = 0
        Me.txtSigDig.SelStart = 1
        Me.txtSigDig.Size = New System.Drawing.Size(38, 19)
        Me.txtSigDig.SoftMax = 10
        Me.txtSigDig.SoftMin = 1
        Me.txtSigDig.TabIndex = 0
        Me.txtSigDig.Value = CType(1, Long)
        '
        'frmOptions
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(292, 268)
        Me.Controls.Add(Me.grpboxNumberFormat)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.grpboxDateFormat)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(300, 300)
        Me.Name = "frmOptions"
        Me.Text = "List Options"
        Me.grpboxNumberFormat.ResumeLayout(False)
        Me.grpboxNumberFormat.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpboxDateFormat As System.Windows.Forms.GroupBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpboxNumberFormat As System.Windows.Forms.GroupBox
    Friend WithEvents txtDecPla As atcControls.atcText
    Friend WithEvents txtSigDig As atcControls.atcText
    Friend WithEvents lblSigDig As System.Windows.Forms.Label
    Friend WithEvents lblDecPla As System.Windows.Forms.Label
End Class
