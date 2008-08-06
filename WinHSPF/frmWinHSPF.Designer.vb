<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWinHSPF
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWinHSPF))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FunctionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ReachEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LandUseEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InputDataEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.FunctionsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(724, 26)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(40, 22)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(43, 22)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'FunctionsToolStripMenuItem
        '
        Me.FunctionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReachEditorToolStripMenuItem, Me.LandUseEditorToolStripMenuItem, Me.InputDataEditorToolStripMenuItem})
        Me.FunctionsToolStripMenuItem.Name = "FunctionsToolStripMenuItem"
        Me.FunctionsToolStripMenuItem.Size = New System.Drawing.Size(81, 22)
        Me.FunctionsToolStripMenuItem.Text = "Functions"
        '
        'ReachEditorToolStripMenuItem
        '
        Me.ReachEditorToolStripMenuItem.Name = "ReachEditorToolStripMenuItem"
        Me.ReachEditorToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ReachEditorToolStripMenuItem.Text = "Reach Editor"
        '
        'LandUseEditorToolStripMenuItem
        '
        Me.LandUseEditorToolStripMenuItem.Name = "LandUseEditorToolStripMenuItem"
        Me.LandUseEditorToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.LandUseEditorToolStripMenuItem.Text = "Land Use Editor"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(48, 22)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'InputDataEditorToolStripMenuItem
        '
        Me.InputDataEditorToolStripMenuItem.Name = "InputDataEditorToolStripMenuItem"
        Me.InputDataEditorToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.InputDataEditorToolStripMenuItem.Text = "Input Data Editor"
        '
        'frmWinHSPF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(724, 315)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmWinHSPF"
        Me.Text = "Hydrological Simulation Program - Fortran (HSPF)"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FunctionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReachEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LandUseEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InputDataEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
