<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIndex
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
        Me.txtXIndex = New atcControls.atcText
        Me.txtYIndex = New atcControls.atcText
        Me.lblXIndex = New System.Windows.Forms.Label
        Me.lblYIndex = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtXIndex
        '
        Me.txtXIndex.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtXIndex.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtXIndex.DefaultValue = "1"
        Me.txtXIndex.HardMax = -999
        Me.txtXIndex.HardMin = 1
        Me.txtXIndex.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtXIndex.Location = New System.Drawing.Point(120, 26)
        Me.txtXIndex.MaxWidth = 20
        Me.txtXIndex.Name = "txtXIndex"
        Me.txtXIndex.NumericFormat = "0"
        Me.txtXIndex.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtXIndex.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtXIndex.SelLength = 0
        Me.txtXIndex.SelStart = 0
        Me.txtXIndex.Size = New System.Drawing.Size(75, 28)
        Me.txtXIndex.SoftMax = -999
        Me.txtXIndex.SoftMin = -999
        Me.txtXIndex.TabIndex = 0
        Me.txtXIndex.ValueDouble = 1
        Me.txtXIndex.ValueInteger = 1
        '
        'txtYIndex
        '
        Me.txtYIndex.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtYIndex.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtYIndex.DefaultValue = "1"
        Me.txtYIndex.HardMax = -999
        Me.txtYIndex.HardMin = 1
        Me.txtYIndex.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtYIndex.Location = New System.Drawing.Point(120, 64)
        Me.txtYIndex.MaxWidth = 20
        Me.txtYIndex.Name = "txtYIndex"
        Me.txtYIndex.NumericFormat = "0"
        Me.txtYIndex.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtYIndex.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtYIndex.SelLength = 0
        Me.txtYIndex.SelStart = 0
        Me.txtYIndex.Size = New System.Drawing.Size(75, 28)
        Me.txtYIndex.SoftMax = -999
        Me.txtYIndex.SoftMin = -999
        Me.txtYIndex.TabIndex = 1
        Me.txtYIndex.ValueDouble = 1
        Me.txtYIndex.ValueInteger = 1
        '
        'lblXIndex
        '
        Me.lblXIndex.AutoSize = True
        Me.lblXIndex.Location = New System.Drawing.Point(15, 26)
        Me.lblXIndex.Name = "lblXIndex"
        Me.lblXIndex.Size = New System.Drawing.Size(102, 13)
        Me.lblXIndex.TabIndex = 2
        Me.lblXIndex.Text = "X (Longitude) Index:"
        '
        'lblYIndex
        '
        Me.lblYIndex.AutoSize = True
        Me.lblYIndex.Location = New System.Drawing.Point(24, 64)
        Me.lblYIndex.Name = "lblYIndex"
        Me.lblYIndex.Size = New System.Drawing.Size(93, 13)
        Me.lblYIndex.TabIndex = 3
        Me.lblYIndex.Text = "Y (Latitude) Index:"
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(18, 113)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(120, 113)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmIndex
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(214, 156)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblYIndex)
        Me.Controls.Add(Me.lblXIndex)
        Me.Controls.Add(Me.txtYIndex)
        Me.Controls.Add(Me.txtXIndex)
        Me.MaximizeBox = False
        Me.Name = "frmIndex"
        Me.Text = "Specify Subset"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtXIndex As atcControls.atcText
    Friend WithEvents txtYIndex As atcControls.atcText
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Private WithEvents lblXIndex As System.Windows.Forms.Label
    Private WithEvents lblYIndex As System.Windows.Forms.Label
End Class
