<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmDebugScript
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
	Public WithEvents cmdStep As System.Windows.Forms.Button
	Public WithEvents cmdRun As System.Windows.Forms.Button
	Public WithEvents cmdQuit As System.Windows.Forms.Button
	Public WithEvents cmdNext As System.Windows.Forms.Button
	Public WithEvents lblDate As System.Windows.Forms.Label
	Public WithEvents lblValue As System.Windows.Forms.Label
	Public WithEvents fraButtons As System.Windows.Forms.Panel
	Public WithEvents lstCurrentScript As System.Windows.Forms.ListBox
	Public WithEvents txtCurrentLine As System.Windows.Forms.TextBox
	Public WithEvents lblCurProgramLine As System.Windows.Forms.Label
	Public WithEvents lblCurrentLine As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDebugScript))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.fraButtons = New System.Windows.Forms.Panel
		Me.cmdStep = New System.Windows.Forms.Button
		Me.cmdRun = New System.Windows.Forms.Button
		Me.cmdQuit = New System.Windows.Forms.Button
		Me.cmdNext = New System.Windows.Forms.Button
		Me.lblDate = New System.Windows.Forms.Label
		Me.lblValue = New System.Windows.Forms.Label
		Me.lstCurrentScript = New System.Windows.Forms.ListBox
		Me.txtCurrentLine = New System.Windows.Forms.TextBox
		Me.lblCurProgramLine = New System.Windows.Forms.Label
		Me.lblCurrentLine = New System.Windows.Forms.Label
		Me.fraButtons.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.Text = "Debug Script"
		Me.ClientSize = New System.Drawing.Size(626, 272)
		Me.Location = New System.Drawing.Point(4, 19)
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
		Me.Name = "frmDebugScript"
		Me.fraButtons.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraButtons.Text = "Frame1"
		Me.fraButtons.Size = New System.Drawing.Size(369, 73)
		Me.fraButtons.Location = New System.Drawing.Point(136, 192)
		Me.fraButtons.TabIndex = 4
		Me.fraButtons.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraButtons.BackColor = System.Drawing.SystemColors.Control
		Me.fraButtons.Enabled = True
		Me.fraButtons.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraButtons.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraButtons.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraButtons.Visible = True
		Me.fraButtons.Name = "fraButtons"
		Me.cmdStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdStep.Text = "&Step"
		Me.cmdStep.Size = New System.Drawing.Size(81, 25)
		Me.cmdStep.Location = New System.Drawing.Point(0, 48)
		Me.cmdStep.TabIndex = 5
		Me.cmdStep.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdStep.BackColor = System.Drawing.SystemColors.Control
		Me.cmdStep.CausesValidation = True
		Me.cmdStep.Enabled = True
		Me.cmdStep.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdStep.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdStep.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdStep.TabStop = True
		Me.cmdStep.Name = "cmdStep"
		Me.cmdRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRun.Text = "&Run"
		Me.cmdRun.CausesValidation = False
		Me.cmdRun.Size = New System.Drawing.Size(81, 25)
		Me.cmdRun.Location = New System.Drawing.Point(192, 48)
		Me.cmdRun.TabIndex = 7
		Me.ToolTip1.SetToolTip(Me.cmdRun, "Read the rest of the file without stopping")
		Me.cmdRun.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdRun.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRun.Enabled = True
		Me.cmdRun.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRun.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRun.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRun.TabStop = True
		Me.cmdRun.Name = "cmdRun"
		Me.cmdQuit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdQuit
		Me.cmdQuit.Text = "&Quit"
		Me.cmdQuit.Size = New System.Drawing.Size(81, 25)
		Me.cmdQuit.Location = New System.Drawing.Point(288, 48)
		Me.cmdQuit.TabIndex = 8
		Me.ToolTip1.SetToolTip(Me.cmdQuit, "Stop reading and discard any data that has been read")
		Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdQuit.CausesValidation = True
		Me.cmdQuit.Enabled = True
		Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdQuit.TabStop = True
		Me.cmdQuit.Name = "cmdQuit"
		Me.cmdNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNext.Text = "&Next Line"
		Me.AcceptButton = Me.cmdNext
		Me.cmdNext.Size = New System.Drawing.Size(81, 25)
		Me.cmdNext.Location = New System.Drawing.Point(96, 48)
		Me.cmdNext.TabIndex = 6
		Me.ToolTip1.SetToolTip(Me.cmdNext, "Step until the current line is finished")
		Me.cmdNext.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNext.CausesValidation = True
		Me.cmdNext.Enabled = True
		Me.cmdNext.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNext.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNext.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNext.TabStop = True
		Me.cmdNext.Name = "cmdNext"
		Me.lblDate.Text = "Date"
		Me.lblDate.Size = New System.Drawing.Size(369, 17)
		Me.lblDate.Location = New System.Drawing.Point(0, 0)
		Me.lblDate.TabIndex = 10
		Me.lblDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblDate.Enabled = True
		Me.lblDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDate.UseMnemonic = True
		Me.lblDate.Visible = True
		Me.lblDate.AutoSize = False
		Me.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDate.Name = "lblDate"
		Me.lblValue.Text = "Value"
		Me.lblValue.Size = New System.Drawing.Size(369, 17)
		Me.lblValue.Location = New System.Drawing.Point(0, 24)
		Me.lblValue.TabIndex = 9
		Me.lblValue.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblValue.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblValue.BackColor = System.Drawing.SystemColors.Control
		Me.lblValue.Enabled = True
		Me.lblValue.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblValue.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblValue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblValue.UseMnemonic = True
		Me.lblValue.Visible = True
		Me.lblValue.AutoSize = False
		Me.lblValue.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblValue.Name = "lblValue"
		Me.lstCurrentScript.Size = New System.Drawing.Size(481, 96)
		Me.lstCurrentScript.Location = New System.Drawing.Point(136, 96)
		Me.lstCurrentScript.TabIndex = 3
		Me.lstCurrentScript.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lstCurrentScript.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstCurrentScript.BackColor = System.Drawing.SystemColors.Window
		Me.lstCurrentScript.CausesValidation = True
		Me.lstCurrentScript.Enabled = True
		Me.lstCurrentScript.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstCurrentScript.IntegralHeight = True
		Me.lstCurrentScript.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstCurrentScript.SelectionMode = System.Windows.Forms.SelectionMode.One
		Me.lstCurrentScript.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstCurrentScript.Sorted = False
		Me.lstCurrentScript.TabStop = True
		Me.lstCurrentScript.Visible = True
		Me.lstCurrentScript.MultiColumn = False
		Me.lstCurrentScript.Name = "lstCurrentScript"
		Me.txtCurrentLine.AutoSize = False
		Me.txtCurrentLine.Size = New System.Drawing.Size(481, 89)
		Me.txtCurrentLine.HideSelection = False
		Me.txtCurrentLine.Location = New System.Drawing.Point(136, 0)
		Me.txtCurrentLine.MultiLine = True
		Me.txtCurrentLine.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtCurrentLine.TabIndex = 1
		Me.txtCurrentLine.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtCurrentLine.AcceptsReturn = True
		Me.txtCurrentLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrentLine.BackColor = System.Drawing.SystemColors.Window
		Me.txtCurrentLine.CausesValidation = True
		Me.txtCurrentLine.Enabled = True
		Me.txtCurrentLine.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCurrentLine.ReadOnly = False
		Me.txtCurrentLine.Maxlength = 0
		Me.txtCurrentLine.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCurrentLine.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrentLine.TabStop = True
		Me.txtCurrentLine.Visible = True
		Me.txtCurrentLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCurrentLine.Name = "txtCurrentLine"
		Me.lblCurProgramLine.Text = "Current Script Line:"
		Me.lblCurProgramLine.Size = New System.Drawing.Size(97, 17)
		Me.lblCurProgramLine.Location = New System.Drawing.Point(8, 96)
		Me.lblCurProgramLine.TabIndex = 2
		Me.lblCurProgramLine.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblCurProgramLine.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurProgramLine.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurProgramLine.Enabled = True
		Me.lblCurProgramLine.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurProgramLine.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurProgramLine.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurProgramLine.UseMnemonic = True
		Me.lblCurProgramLine.Visible = True
		Me.lblCurProgramLine.AutoSize = False
		Me.lblCurProgramLine.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurProgramLine.Name = "lblCurProgramLine"
		Me.lblCurrentLine.Text = "Current Input Line #"
		Me.lblCurrentLine.Size = New System.Drawing.Size(121, 17)
		Me.lblCurrentLine.Location = New System.Drawing.Point(8, 8)
		Me.lblCurrentLine.TabIndex = 0
		Me.lblCurrentLine.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblCurrentLine.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrentLine.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrentLine.Enabled = True
		Me.lblCurrentLine.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrentLine.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrentLine.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrentLine.UseMnemonic = True
		Me.lblCurrentLine.Visible = True
		Me.lblCurrentLine.AutoSize = False
		Me.lblCurrentLine.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrentLine.Name = "lblCurrentLine"
		Me.Controls.Add(fraButtons)
		Me.Controls.Add(lstCurrentScript)
		Me.Controls.Add(txtCurrentLine)
		Me.Controls.Add(lblCurProgramLine)
		Me.Controls.Add(lblCurrentLine)
		Me.fraButtons.Controls.Add(cmdStep)
		Me.fraButtons.Controls.Add(cmdRun)
		Me.fraButtons.Controls.Add(cmdQuit)
		Me.fraButtons.Controls.Add(cmdNext)
		Me.fraButtons.Controls.Add(lblDate)
		Me.fraButtons.Controls.Add(lblValue)
		Me.fraButtons.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class