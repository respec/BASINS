<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmSimulationNew
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
	Public WithEvents TimeStep As System.Windows.Forms.TextBox
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdSave As System.Windows.Forms.Button
	Public WithEvents cbxWASP As System.Windows.Forms.CheckBox
	Public WithEvents cbxWhAEM As System.Windows.Forms.CheckBox
    Public WithEvents Frame6 As System.Windows.Forms.GroupBox
	Public WithEvents cbxMercury As System.Windows.Forms.CheckBox
	Public WithEvents cbxSediment As System.Windows.Forms.CheckBox
	Public WithEvents cbxHydro As System.Windows.Forms.CheckBox
	Public WithEvents Frame5 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSimulationNew))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.TimeStep = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.Frame6 = New System.Windows.Forms.GroupBox
        Me.cbxWASP = New System.Windows.Forms.CheckBox
        Me.cbxWhAEM = New System.Windows.Forms.CheckBox
        Me.Frame5 = New System.Windows.Forms.GroupBox
        Me.cbxMercury = New System.Windows.Forms.CheckBox
        Me.cbxSediment = New System.Windows.Forms.CheckBox
        Me.cbxHydro = New System.Windows.Forms.CheckBox
        Me.Frame1.SuspendLayout()
        Me.Frame6.SuspendLayout()
        Me.Frame5.SuspendLayout()
        Me.SuspendLayout()
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.TimeStep)
        Me.Frame1.Controls.Add(Me.Label2)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(16, 120)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(273, 41)
        Me.Frame1.TabIndex = 4
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Time Step"
        '
        'TimeStep
        '
        Me.TimeStep.AcceptsReturn = True
        Me.TimeStep.BackColor = System.Drawing.SystemColors.Window
        Me.TimeStep.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TimeStep.Enabled = False
        Me.TimeStep.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimeStep.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TimeStep.Location = New System.Drawing.Point(152, 16)
        Me.TimeStep.MaxLength = 0
        Me.TimeStep.Name = "TimeStep"
        Me.TimeStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TimeStep.Size = New System.Drawing.Size(33, 20)
        Me.TimeStep.TabIndex = 1
        Me.TimeStep.Text = "1"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(191, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(49, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Days"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(6, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(108, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Simulation Time Step:"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(312, 41)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(81, 25)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdSave
        '
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSave.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSave.Location = New System.Drawing.Point(312, 8)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSave.Size = New System.Drawing.Size(81, 25)
        Me.cmdSave.TabIndex = 1
        Me.cmdSave.Text = "Save"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'Frame6
        '
        Me.Frame6.BackColor = System.Drawing.SystemColors.Control
        Me.Frame6.Controls.Add(Me.cbxWASP)
        Me.Frame6.Controls.Add(Me.cbxWhAEM)
        Me.Frame6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame6.Location = New System.Drawing.Point(16, 64)
        Me.Frame6.Name = "Frame6"
        Me.Frame6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame6.Size = New System.Drawing.Size(272, 50)
        Me.Frame6.TabIndex = 3
        Me.Frame6.TabStop = False
        Me.Frame6.Text = "Model Linkage Options"
        '
        'cbxWASP
        '
        Me.cbxWASP.BackColor = System.Drawing.SystemColors.Control
        Me.cbxWASP.Cursor = System.Windows.Forms.Cursors.Default
        Me.cbxWASP.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxWASP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cbxWASP.Location = New System.Drawing.Point(24, 19)
        Me.cbxWASP.Name = "cbxWASP"
        Me.cbxWASP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbxWASP.Size = New System.Drawing.Size(100, 22)
        Me.cbxWASP.TabIndex = 1
        Me.cbxWASP.Text = "WASP Linkage"
        Me.cbxWASP.UseVisualStyleBackColor = False
        '
        'cbxWhAEM
        '
        Me.cbxWhAEM.BackColor = System.Drawing.SystemColors.Control
        Me.cbxWhAEM.Cursor = System.Windows.Forms.Cursors.Default
        Me.cbxWhAEM.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxWhAEM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cbxWhAEM.Location = New System.Drawing.Point(152, 19)
        Me.cbxWhAEM.Name = "cbxWhAEM"
        Me.cbxWhAEM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbxWhAEM.Size = New System.Drawing.Size(105, 23)
        Me.cbxWhAEM.TabIndex = 2
        Me.cbxWhAEM.Text = "WhAEM Linkage"
        Me.cbxWhAEM.UseVisualStyleBackColor = False
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me.cbxMercury)
        Me.Frame5.Controls.Add(Me.cbxSediment)
        Me.Frame5.Controls.Add(Me.cbxHydro)
        Me.Frame5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(16, 8)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(272, 48)
        Me.Frame5.TabIndex = 0
        Me.Frame5.TabStop = False
        Me.Frame5.Text = "Simulation"
        '
        'cbxMercury
        '
        Me.cbxMercury.AutoSize = True
        Me.cbxMercury.BackColor = System.Drawing.SystemColors.Control
        Me.cbxMercury.Cursor = System.Windows.Forms.Cursors.Default
        Me.cbxMercury.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxMercury.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cbxMercury.Location = New System.Drawing.Point(184, 20)
        Me.cbxMercury.Name = "cbxMercury"
        Me.cbxMercury.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbxMercury.Size = New System.Drawing.Size(66, 18)
        Me.cbxMercury.TabIndex = 2
        Me.cbxMercury.Text = "Mercury"
        Me.cbxMercury.UseVisualStyleBackColor = False
        '
        'cbxSediment
        '
        Me.cbxSediment.AutoSize = True
        Me.cbxSediment.BackColor = System.Drawing.SystemColors.Control
        Me.cbxSediment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cbxSediment.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxSediment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cbxSediment.Location = New System.Drawing.Point(107, 20)
        Me.cbxSediment.Name = "cbxSediment"
        Me.cbxSediment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbxSediment.Size = New System.Drawing.Size(70, 18)
        Me.cbxSediment.TabIndex = 1
        Me.cbxSediment.Text = "Sediment"
        Me.cbxSediment.UseVisualStyleBackColor = False
        '
        'cbxHydro
        '
        Me.cbxHydro.AutoSize = True
        Me.cbxHydro.BackColor = System.Drawing.SystemColors.Control
        Me.cbxHydro.Checked = True
        Me.cbxHydro.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxHydro.Cursor = System.Windows.Forms.Cursors.Default
        Me.cbxHydro.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxHydro.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cbxHydro.Location = New System.Drawing.Point(18, 20)
        Me.cbxHydro.Name = "cbxHydro"
        Me.cbxHydro.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbxHydro.Size = New System.Drawing.Size(75, 18)
        Me.cbxHydro.TabIndex = 0
        Me.cbxHydro.Text = "Hydrology"
        Me.cbxHydro.UseVisualStyleBackColor = False
        '
        'frmSimulationNew
        '
        Me.AcceptButton = Me.cmdSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(405, 168)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.Frame6)
        Me.Controls.Add(Me.Frame5)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSimulationNew"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Simulation Options"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.Frame6.ResumeLayout(False)
        Me.Frame5.ResumeLayout(False)
        Me.Frame5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class