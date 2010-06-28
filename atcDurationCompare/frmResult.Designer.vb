<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmResult
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmResult))
        Me.txtReport = New System.Windows.Forms.TextBox
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSelectData = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAnalysis = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtReport
        '
        Me.txtReport.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtReport.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReport.Location = New System.Drawing.Point(0, 24)
        Me.txtReport.Multiline = True
        Me.txtReport.Name = "txtReport"
        Me.txtReport.ReadOnly = True
        Me.txtReport.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtReport.Size = New System.Drawing.Size(521, 383)
        Me.txtReport.TabIndex = 5
        Me.txtReport.WordWrap = False
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSelectData, Me.mnuSave, Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuSelectData
        '
        Me.mnuSelectData.Name = "mnuSelectData"
        Me.mnuSelectData.Size = New System.Drawing.Size(152, 22)
        Me.mnuSelectData.Text = "Select Data"
        '
        'mnuSave
        '
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.Size = New System.Drawing.Size(152, 22)
        Me.mnuSave.Text = "Save"
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(152, 22)
        Me.mnuExit.Text = "Exit"
        '
        'mnuEdit
        '
        Me.mnuEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCopy})
        Me.mnuEdit.Name = "mnuEdit"
        Me.mnuEdit.Size = New System.Drawing.Size(37, 20)
        Me.mnuEdit.Text = "Edit"
        '
        'mnuCopy
        '
        Me.mnuCopy.Name = "mnuCopy"
        Me.mnuCopy.Size = New System.Drawing.Size(110, 22)
        Me.mnuCopy.Text = "Copy"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Name = "mnuAnalysis"
        Me.mnuAnalysis.Size = New System.Drawing.Size(58, 20)
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(40, 20)
        Me.mnuHelp.Text = "Help"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuEdit, Me.mnuAnalysis, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(521, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'frmResult
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 407)
        Me.Controls.Add(Me.txtReport)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmResult"
        Me.Text = "frmResult"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtReport As System.Windows.Forms.TextBox
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSelectData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAnalysis As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
End Class
