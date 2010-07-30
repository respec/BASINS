<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmSelectScript
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
	Public WithEvents cmdRun As System.Windows.Forms.Button
	Public WithEvents cmdWizard As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdFind As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdTest As System.Windows.Forms.Button
	Public WithEvents fraButtons As System.Windows.Forms.Panel
    Public WithEvents agdScripts As atcControls.atcGrid
	Public dlgOpenFileOpen As System.Windows.Forms.OpenFileDialog
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSelectScript))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.fraButtons = New System.Windows.Forms.Panel
		Me.cmdRun = New System.Windows.Forms.Button
		Me.cmdWizard = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdFind = New System.Windows.Forms.Button
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.cmdTest = New System.Windows.Forms.Button
        Me.agdScripts = New atcControls.atcGrid
		Me.dlgOpenFileOpen = New System.Windows.Forms.OpenFileDialog
		Me.fraButtons.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.agdScripts, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Text = "Script Selection for Import"
		Me.ClientSize = New System.Drawing.Size(656, 231)
		Me.Location = New System.Drawing.Point(3, 18)
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
		Me.Name = "frmSelectScript"
		Me.fraButtons.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraButtons.Text = "Frame1"
		Me.fraButtons.Size = New System.Drawing.Size(89, 185)
		Me.fraButtons.Location = New System.Drawing.Point(560, 8)
		Me.fraButtons.TabIndex = 7
		Me.fraButtons.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraButtons.BackColor = System.Drawing.SystemColors.Control
		Me.fraButtons.Enabled = True
		Me.fraButtons.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraButtons.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraButtons.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraButtons.Visible = True
		Me.fraButtons.Name = "fraButtons"
		Me.cmdRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRun.Text = "&Run"
		Me.cmdRun.Enabled = False
		Me.cmdRun.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdRun.Size = New System.Drawing.Size(89, 25)
		Me.cmdRun.Location = New System.Drawing.Point(0, 0)
		Me.cmdRun.TabIndex = 1
		Me.cmdRun.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRun.CausesValidation = True
		Me.cmdRun.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRun.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRun.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRun.TabStop = True
		Me.cmdRun.Name = "cmdRun"
		Me.cmdWizard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdWizard.Text = "&Edit"
		Me.cmdWizard.Enabled = False
		Me.cmdWizard.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdWizard.Size = New System.Drawing.Size(89, 25)
		Me.cmdWizard.Location = New System.Drawing.Point(0, 32)
		Me.cmdWizard.TabIndex = 2
		Me.cmdWizard.BackColor = System.Drawing.SystemColors.Control
		Me.cmdWizard.CausesValidation = True
		Me.cmdWizard.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdWizard.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdWizard.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdWizard.TabStop = True
		Me.cmdWizard.Name = "cmdWizard"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdCancel
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdCancel.Size = New System.Drawing.Size(89, 25)
		Me.cmdCancel.Location = New System.Drawing.Point(0, 160)
		Me.cmdCancel.TabIndex = 6
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdFind.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFind.Text = "&Find..."
		Me.cmdFind.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdFind.Size = New System.Drawing.Size(89, 25)
		Me.cmdFind.Location = New System.Drawing.Point(0, 64)
		Me.cmdFind.TabIndex = 3
		Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFind.CausesValidation = True
		Me.cmdFind.Enabled = True
		Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFind.TabStop = True
		Me.cmdFind.Name = "cmdFind"
		Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDelete.Text = "For&get"
		Me.cmdDelete.Enabled = False
		Me.cmdDelete.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdDelete.Size = New System.Drawing.Size(89, 25)
		Me.cmdDelete.Location = New System.Drawing.Point(0, 96)
		Me.cmdDelete.TabIndex = 4
		Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDelete.CausesValidation = True
		Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdTest.Text = "&Debug"
		Me.cmdTest.Enabled = False
		Me.cmdTest.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdTest.Size = New System.Drawing.Size(89, 25)
		Me.cmdTest.Location = New System.Drawing.Point(0, 128)
		Me.cmdTest.TabIndex = 5
		Me.cmdTest.BackColor = System.Drawing.SystemColors.Control
		Me.cmdTest.CausesValidation = True
		Me.cmdTest.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdTest.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdTest.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdTest.TabStop = True
		Me.cmdTest.Name = "cmdTest"
        Me.agdScripts.Size = New System.Drawing.Size(545, 217)
		Me.agdScripts.Location = New System.Drawing.Point(8, 8)
		Me.agdScripts.TabIndex = 0
		Me.agdScripts.Name = "agdScripts"
		Me.Controls.Add(fraButtons)
		Me.Controls.Add(agdScripts)
		Me.fraButtons.Controls.Add(cmdRun)
		Me.fraButtons.Controls.Add(cmdWizard)
		Me.fraButtons.Controls.Add(cmdCancel)
		Me.fraButtons.Controls.Add(cmdFind)
		Me.fraButtons.Controls.Add(cmdDelete)
		Me.fraButtons.Controls.Add(cmdTest)
		CType(Me.agdScripts, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraButtons.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class