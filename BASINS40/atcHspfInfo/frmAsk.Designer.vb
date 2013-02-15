<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmAsk
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdEnd As System.Windows.Forms.Button
	Public WithEvents lbl2 As System.Windows.Forms.Label
	Public WithEvents lbl As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmAsk))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdEnd = New System.Windows.Forms.Button
		Me.lbl2 = New System.Windows.Forms.Label
		Me.lbl = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.Text = "Process Not Ending"
		Me.ClientSize = New System.Drawing.Size(354, 96)
		Me.Location = New System.Drawing.Point(4, 20)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmAsk"
		Me.cmdEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEnd.Text = "&End Process Now"
		Me.cmdEnd.Size = New System.Drawing.Size(113, 25)
		Me.cmdEnd.Location = New System.Drawing.Point(120, 64)
		Me.cmdEnd.TabIndex = 0
		Me.cmdEnd.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdEnd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEnd.CausesValidation = True
		Me.cmdEnd.Enabled = True
		Me.cmdEnd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEnd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEnd.TabStop = True
		Me.cmdEnd.Name = "cmdEnd"
		Me.lbl2.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lbl2.Size = New System.Drawing.Size(337, 17)
		Me.lbl2.Location = New System.Drawing.Point(8, 32)
		Me.lbl2.TabIndex = 2
		Me.lbl2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lbl2.BackColor = System.Drawing.SystemColors.Control
		Me.lbl2.Enabled = True
		Me.lbl2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lbl2.Cursor = System.Windows.Forms.Cursors.Default
		Me.lbl2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lbl2.UseMnemonic = True
		Me.lbl2.Visible = True
		Me.lbl2.AutoSize = False
		Me.lbl2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lbl2.Name = "lbl2"
		Me.lbl.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lbl.Size = New System.Drawing.Size(337, 17)
		Me.lbl.Location = New System.Drawing.Point(8, 8)
		Me.lbl.TabIndex = 1
		Me.lbl.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lbl.BackColor = System.Drawing.SystemColors.Control
		Me.lbl.Enabled = True
		Me.lbl.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lbl.Cursor = System.Windows.Forms.Cursors.Default
		Me.lbl.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lbl.UseMnemonic = True
		Me.lbl.Visible = True
		Me.lbl.AutoSize = False
		Me.lbl.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lbl.Name = "lbl"
		Me.Controls.Add(cmdEnd)
		Me.Controls.Add(lbl2)
		Me.Controls.Add(lbl)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class