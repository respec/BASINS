<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MapWindowForm
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MapWindowForm))
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblProgress = New System.Windows.Forms.Label
        Me.StripDocker = New System.Windows.Forms.ToolStripContainer
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.tlbMain = New System.Windows.Forms.ToolStrip
        Me.tbbNew = New System.Windows.Forms.ToolStripButton
        Me.tbbOpen = New System.Windows.Forms.ToolStripButton
        Me.tbbSave = New System.Windows.Forms.ToolStripButton
        Me.tbbBreak1 = New System.Windows.Forms.ToolStripSeparator
        Me.ilsToolbar = New System.Windows.Forms.ImageList(Me.components)
        Me.StripDocker.TopToolStripPanel.SuspendLayout()
        Me.StripDocker.SuspendLayout()
        Me.tlbMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(24, 64)
        Me.lblStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 17)
        Me.lblStatus.TabIndex = 11
        '
        'lblProgress
        '
        Me.lblProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(387, 64)
        Me.lblProgress.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(0, 17)
        Me.lblProgress.TabIndex = 65
        '
        'StripDocker
        '
        '
        'StripDocker.ContentPanel
        '
        Me.StripDocker.ContentPanel.Margin = New System.Windows.Forms.Padding(4)
        Me.StripDocker.ContentPanel.Size = New System.Drawing.Size(922, 55)
        Me.StripDocker.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StripDocker.Location = New System.Drawing.Point(0, 0)
        Me.StripDocker.Margin = New System.Windows.Forms.Padding(4)
        Me.StripDocker.Name = "StripDocker"
        Me.StripDocker.Size = New System.Drawing.Size(922, 104)
        Me.StripDocker.TabIndex = 66
        Me.StripDocker.Text = "StripDocker"
        '
        'StripDocker.TopToolStripPanel
        '
        Me.StripDocker.TopToolStripPanel.Controls.Add(Me.MenuStrip1)
        Me.StripDocker.TopToolStripPanel.Controls.Add(Me.tlbMain)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(922, 24)
        Me.MenuStrip1.TabIndex = 0
        '
        'tlbMain
        '
        Me.tlbMain.Dock = System.Windows.Forms.DockStyle.None
        Me.tlbMain.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.tlbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tbbNew, Me.tbbOpen, Me.tbbSave, Me.tbbBreak1})
        Me.tlbMain.Location = New System.Drawing.Point(3, 24)
        Me.tlbMain.Name = "tlbMain"
        Me.tlbMain.Size = New System.Drawing.Size(116, 25)
        Me.tlbMain.TabIndex = 5
        '
        'tbbNew
        '
        Me.tbbNew.Image = CType(resources.GetObject("tbbNew.Image"), System.Drawing.Image)
        Me.tbbNew.Name = "tbbNew"
        Me.tbbNew.Size = New System.Drawing.Size(23, 22)
        Me.tbbNew.ToolTipText = "New Project"
        '
        'tbbOpen
        '
        Me.tbbOpen.Image = CType(resources.GetObject("tbbOpen.Image"), System.Drawing.Image)
        Me.tbbOpen.Name = "tbbOpen"
        Me.tbbOpen.Size = New System.Drawing.Size(23, 22)
        Me.tbbOpen.ToolTipText = "Open Project"
        '
        'tbbSave
        '
        Me.tbbSave.Image = CType(resources.GetObject("tbbSave.Image"), System.Drawing.Image)
        Me.tbbSave.Name = "tbbSave"
        Me.tbbSave.Size = New System.Drawing.Size(23, 22)
        Me.tbbSave.ToolTipText = "Save Project"
        '
        'tbbBreak1
        '
        Me.tbbBreak1.Name = "tbbBreak1"
        Me.tbbBreak1.Size = New System.Drawing.Size(6, 25)
        '
        'ilsToolbar
        '
        Me.ilsToolbar.ImageStream = CType(resources.GetObject("ilsToolbar.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilsToolbar.TransparentColor = System.Drawing.Color.Transparent
        Me.ilsToolbar.Images.SetKeyName(0, "")
        Me.ilsToolbar.Images.SetKeyName(1, "")
        Me.ilsToolbar.Images.SetKeyName(2, "")
        Me.ilsToolbar.Images.SetKeyName(3, "")
        Me.ilsToolbar.Images.SetKeyName(4, "")
        Me.ilsToolbar.Images.SetKeyName(5, "")
        Me.ilsToolbar.Images.SetKeyName(6, "")
        Me.ilsToolbar.Images.SetKeyName(7, "")
        Me.ilsToolbar.Images.SetKeyName(8, "")
        Me.ilsToolbar.Images.SetKeyName(9, "")
        Me.ilsToolbar.Images.SetKeyName(10, "")
        Me.ilsToolbar.Images.SetKeyName(11, "")
        Me.ilsToolbar.Images.SetKeyName(12, "")
        Me.ilsToolbar.Images.SetKeyName(13, "")
        Me.ilsToolbar.Images.SetKeyName(14, "")
        Me.ilsToolbar.Images.SetKeyName(15, "")
        Me.ilsToolbar.Images.SetKeyName(16, "")
        Me.ilsToolbar.Images.SetKeyName(17, "")
        Me.ilsToolbar.Images.SetKeyName(18, "")
        Me.ilsToolbar.Images.SetKeyName(19, "")
        Me.ilsToolbar.Images.SetKeyName(20, "")
        Me.ilsToolbar.Images.SetKeyName(21, "")
        Me.ilsToolbar.Images.SetKeyName(22, "")
        Me.ilsToolbar.Images.SetKeyName(23, "")
        '
        'MapWindowForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(922, 104)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.StripDocker)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "MapWindowForm"
        Me.Text = "BASINS Light"
        Me.StripDocker.TopToolStripPanel.ResumeLayout(False)
        Me.StripDocker.TopToolStripPanel.PerformLayout()
        Me.StripDocker.ResumeLayout(False)
        Me.StripDocker.PerformLayout()
        Me.tlbMain.ResumeLayout(False)
        Me.tlbMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents StripDocker As System.Windows.Forms.ToolStripContainer
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents tlbMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tbbNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbbOpen As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbbBreak1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ilsToolbar As System.Windows.Forms.ImageList

End Class
