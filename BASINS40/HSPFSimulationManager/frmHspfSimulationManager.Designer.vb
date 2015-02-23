<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHspfSimulationManager
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHspfSimulationManager))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.panelSchematic = New System.Windows.Forms.Panel()
        Me.mainSchematic = New HSPFSimulationManager.ctlSchematic()
        Me.btnMedio = New System.Windows.Forms.Button()
        Me.btnMedinaBelowLake = New System.Windows.Forms.Button()
        Me.btnMedinaDiversionLake = New System.Windows.Forms.Button()
        Me.btnMedinaLake = New System.Windows.Forms.Button()
        Me.btnUSAR = New System.Windows.Forms.Button()
        Me.btnLSAR = New System.Windows.Forms.Button()
        Me.btnSalado = New System.Windows.Forms.Button()
        Me.btnLeon = New System.Windows.Forms.Button()
        Me.btnMedinaAboveLake = New System.Windows.Forms.Button()
        Me.btnCibolo = New System.Windows.Forms.Button()
        Me.MenuStrip1.SuspendLayout()
        Me.panelSchematic.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1253, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'panelSchematic
        '
        Me.panelSchematic.AutoScroll = True
        Me.panelSchematic.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.panelSchematic.Controls.Add(Me.btnMedio)
        Me.panelSchematic.Controls.Add(Me.btnMedinaBelowLake)
        Me.panelSchematic.Controls.Add(Me.btnMedinaDiversionLake)
        Me.panelSchematic.Controls.Add(Me.btnMedinaLake)
        Me.panelSchematic.Controls.Add(Me.btnUSAR)
        Me.panelSchematic.Controls.Add(Me.btnLSAR)
        Me.panelSchematic.Controls.Add(Me.btnSalado)
        Me.panelSchematic.Controls.Add(Me.btnLeon)
        Me.panelSchematic.Controls.Add(Me.btnMedinaAboveLake)
        Me.panelSchematic.Controls.Add(Me.btnCibolo)
        Me.panelSchematic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelSchematic.Location = New System.Drawing.Point(0, 24)
        Me.panelSchematic.Name = "panelSchematic"
        Me.panelSchematic.Size = New System.Drawing.Size(1253, 708)
        Me.panelSchematic.TabIndex = 2
        '
        'mainSchematic
        '
        Me.mainSchematic.AutoScroll = True
        Me.mainSchematic.Location = New System.Drawing.Point(332, 27)
        Me.mainSchematic.Name = "mainSchematic"
        Me.mainSchematic.Size = New System.Drawing.Size(745, 708)
        Me.mainSchematic.TabIndex = 3
        '
        'btnMedio
        '
        Me.btnMedio.AutoSize = True
        Me.btnMedio.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnMedio.Image = Global.HSPFSimulationManager.My.Resources.Resources.Medio_Simple_50
        Me.btnMedio.Location = New System.Drawing.Point(227, 156)
        Me.btnMedio.Name = "btnMedio"
        Me.btnMedio.Size = New System.Drawing.Size(74, 111)
        Me.btnMedio.TabIndex = 9
        Me.btnMedio.Text = "Medio"
        Me.btnMedio.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnMedio.UseVisualStyleBackColor = False
        '
        'btnMedinaBelowLake
        '
        Me.btnMedinaBelowLake.AutoSize = True
        Me.btnMedinaBelowLake.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnMedinaBelowLake.Image = Global.HSPFSimulationManager.My.Resources.Resources.MedinaBelowLake_Simple_50
        Me.btnMedinaBelowLake.Location = New System.Drawing.Point(227, 340)
        Me.btnMedinaBelowLake.Name = "btnMedinaBelowLake"
        Me.btnMedinaBelowLake.Size = New System.Drawing.Size(184, 111)
        Me.btnMedinaBelowLake.TabIndex = 8
        Me.btnMedinaBelowLake.Text = "Medina Below Lake"
        Me.btnMedinaBelowLake.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnMedinaBelowLake.UseVisualStyleBackColor = False
        '
        'btnMedinaDiversionLake
        '
        Me.btnMedinaDiversionLake.AutoSize = True
        Me.btnMedinaDiversionLake.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnMedinaDiversionLake.Image = Global.HSPFSimulationManager.My.Resources.Resources.MedinaDiversionLake_Simple_50
        Me.btnMedinaDiversionLake.Location = New System.Drawing.Point(83, 297)
        Me.btnMedinaDiversionLake.Name = "btnMedinaDiversionLake"
        Me.btnMedinaDiversionLake.Size = New System.Drawing.Size(126, 61)
        Me.btnMedinaDiversionLake.TabIndex = 7
        Me.btnMedinaDiversionLake.Text = "Medina Diversion Lake"
        Me.btnMedinaDiversionLake.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnMedinaDiversionLake.UseVisualStyleBackColor = False
        '
        'btnMedinaLake
        '
        Me.btnMedinaLake.AutoSize = True
        Me.btnMedinaLake.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnMedinaLake.Image = Global.HSPFSimulationManager.My.Resources.Resources.MedinaLake_Simple_50
        Me.btnMedinaLake.Location = New System.Drawing.Point(90, 156)
        Me.btnMedinaLake.Name = "btnMedinaLake"
        Me.btnMedinaLake.Size = New System.Drawing.Size(113, 111)
        Me.btnMedinaLake.TabIndex = 6
        Me.btnMedinaLake.Text = "Medina Lake"
        Me.btnMedinaLake.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnMedinaLake.UseVisualStyleBackColor = False
        '
        'btnUSAR
        '
        Me.btnUSAR.AutoSize = True
        Me.btnUSAR.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnUSAR.Image = CType(resources.GetObject("btnUSAR.Image"), System.Drawing.Image)
        Me.btnUSAR.Location = New System.Drawing.Point(438, 311)
        Me.btnUSAR.Name = "btnUSAR"
        Me.btnUSAR.Size = New System.Drawing.Size(164, 204)
        Me.btnUSAR.TabIndex = 5
        Me.btnUSAR.Text = "USAR"
        Me.btnUSAR.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnUSAR.UseVisualStyleBackColor = False
        '
        'btnLSAR
        '
        Me.btnLSAR.AutoSize = True
        Me.btnLSAR.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnLSAR.Image = CType(resources.GetObject("btnLSAR.Image"), System.Drawing.Image)
        Me.btnLSAR.Location = New System.Drawing.Point(634, 297)
        Me.btnLSAR.Name = "btnLSAR"
        Me.btnLSAR.Size = New System.Drawing.Size(298, 257)
        Me.btnLSAR.TabIndex = 4
        Me.btnLSAR.Text = "LSAR"
        Me.btnLSAR.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnLSAR.UseVisualStyleBackColor = False
        '
        'btnSalado
        '
        Me.btnSalado.AutoSize = True
        Me.btnSalado.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnSalado.Image = CType(resources.GetObject("btnSalado.Image"), System.Drawing.Image)
        Me.btnSalado.Location = New System.Drawing.Point(470, 92)
        Me.btnSalado.Name = "btnSalado"
        Me.btnSalado.Size = New System.Drawing.Size(99, 175)
        Me.btnSalado.TabIndex = 3
        Me.btnSalado.Text = "Salado"
        Me.btnSalado.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSalado.UseVisualStyleBackColor = False
        '
        'btnLeon
        '
        Me.btnLeon.AutoSize = True
        Me.btnLeon.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnLeon.Image = CType(resources.GetObject("btnLeon.Image"), System.Drawing.Image)
        Me.btnLeon.Location = New System.Drawing.Point(332, 92)
        Me.btnLeon.Name = "btnLeon"
        Me.btnLeon.Size = New System.Drawing.Size(104, 175)
        Me.btnLeon.TabIndex = 2
        Me.btnLeon.Text = "Leon"
        Me.btnLeon.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnLeon.UseVisualStyleBackColor = False
        '
        'btnMedinaAboveLake
        '
        Me.btnMedinaAboveLake.AutoSize = True
        Me.btnMedinaAboveLake.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnMedinaAboveLake.Image = Global.HSPFSimulationManager.My.Resources.Resources.MedinaAboveLake_Simple_50
        Me.btnMedinaAboveLake.Location = New System.Drawing.Point(12, 13)
        Me.btnMedinaAboveLake.Name = "btnMedinaAboveLake"
        Me.btnMedinaAboveLake.Size = New System.Drawing.Size(263, 111)
        Me.btnMedinaAboveLake.TabIndex = 1
        Me.btnMedinaAboveLake.Text = "Medina Above Lake"
        Me.btnMedinaAboveLake.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnMedinaAboveLake.UseVisualStyleBackColor = False
        '
        'btnCibolo
        '
        Me.btnCibolo.AutoSize = True
        Me.btnCibolo.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnCibolo.Image = CType(resources.GetObject("btnCibolo.Image"), System.Drawing.Image)
        Me.btnCibolo.Location = New System.Drawing.Point(662, 13)
        Me.btnCibolo.Name = "btnCibolo"
        Me.btnCibolo.Size = New System.Drawing.Size(234, 232)
        Me.btnCibolo.TabIndex = 0
        Me.btnCibolo.Text = "Cibolo"
        Me.btnCibolo.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnCibolo.UseVisualStyleBackColor = False
        '
        'frmHspfSimulationManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1253, 732)
        Me.Controls.Add(Me.panelSchematic)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.mainSchematic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmHspfSimulationManager"
        Me.Text = "SARA HSPF Simulation Manager"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.panelSchematic.ResumeLayout(False)
        Me.panelSchematic.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents panelSchematic As System.Windows.Forms.Panel
    Friend WithEvents btnSalado As System.Windows.Forms.Button
    Friend WithEvents btnLeon As System.Windows.Forms.Button
    Friend WithEvents btnMedinaAboveLake As System.Windows.Forms.Button
    Friend WithEvents btnCibolo As System.Windows.Forms.Button
    Friend WithEvents btnLSAR As System.Windows.Forms.Button
    Friend WithEvents btnUSAR As System.Windows.Forms.Button
    Friend WithEvents btnMedinaLake As System.Windows.Forms.Button
    Friend WithEvents btnMedinaDiversionLake As System.Windows.Forms.Button
    Friend WithEvents btnMedinaBelowLake As System.Windows.Forms.Button
    Friend WithEvents btnMedio As System.Windows.Forms.Button
    Friend WithEvents mainSchematic As HSPFSimulationManager.ctlSchematic

End Class
