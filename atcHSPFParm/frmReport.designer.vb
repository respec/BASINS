<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReport))
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdAll = New System.Windows.Forms.Button
        Me.cbxUCI = New System.Windows.Forms.CheckBox
        Me.cmdSet = New System.Windows.Forms.Button
        Me.cmdWrite = New System.Windows.Forms.Button
        Me.cdSetOut = New System.Windows.Forms.SaveFileDialog
        Me.lblFile = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(145, 125)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(101, 26)
        Me.cmdClose.TabIndex = 24
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdAll
        '
        Me.cmdAll.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdAll.Location = New System.Drawing.Point(205, 93)
        Me.cmdAll.Name = "cmdAll"
        Me.cmdAll.Size = New System.Drawing.Size(101, 26)
        Me.cmdAll.TabIndex = 23
        Me.cmdAll.Text = "Write All to File"
        Me.cmdAll.UseVisualStyleBackColor = True
        '
        'cbxUCI
        '
        Me.cbxUCI.AutoSize = True
        Me.cbxUCI.Location = New System.Drawing.Point(9, 16)
        Me.cbxUCI.Margin = New System.Windows.Forms.Padding(2)
        Me.cbxUCI.Name = "cbxUCI"
        Me.cbxUCI.Size = New System.Drawing.Size(153, 17)
        Me.cbxUCI.TabIndex = 27
        Me.cbxUCI.Text = "Report tables in UCI format"
        Me.cbxUCI.UseVisualStyleBackColor = True
        '
        'cmdSet
        '
        Me.cmdSet.Location = New System.Drawing.Point(9, 49)
        Me.cmdSet.Name = "cmdSet"
        Me.cmdSet.Size = New System.Drawing.Size(101, 26)
        Me.cmdSet.TabIndex = 29
        Me.cmdSet.Text = "Set File"
        Me.cmdSet.UseVisualStyleBackColor = True
        '
        'cmdWrite
        '
        Me.cmdWrite.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdWrite.Location = New System.Drawing.Point(89, 93)
        Me.cmdWrite.Name = "cmdWrite"
        Me.cmdWrite.Size = New System.Drawing.Size(101, 26)
        Me.cmdWrite.TabIndex = 30
        Me.cmdWrite.Text = "Add Table/Parm to File"
        Me.cmdWrite.UseVisualStyleBackColor = True
        '
        'cdSetOut
        '
        Me.cdSetOut.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
        '
        'lblFile
        '
        Me.lblFile.AutoSize = True
        Me.lblFile.Location = New System.Drawing.Point(116, 55)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(43, 13)
        Me.lblFile.TabIndex = 31
        Me.lblFile.Text = "<none>"
        '
        'frmReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(391, 160)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.cmdSet)
        Me.Controls.Add(Me.cbxUCI)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdAll)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "HSPFParm Report"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdAll As System.Windows.Forms.Button
    Friend WithEvents cbxUCI As System.Windows.Forms.CheckBox
    Friend WithEvents cmdSet As System.Windows.Forms.Button
    Friend WithEvents cmdWrite As System.Windows.Forms.Button
    Friend WithEvents cdSetOut As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lblFile As System.Windows.Forms.Label
End Class
