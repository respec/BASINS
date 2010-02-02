<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDuration
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
        Me.txtReport = New System.Windows.Forms.TextBox
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAnalysis = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtReport
        '
        Me.txtReport.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtReport.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReport.Location = New System.Drawing.Point(12, 30)
        Me.txtReport.Multiline = True
        Me.txtReport.Name = "txtReport"
        Me.txtReport.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtReport.Size = New System.Drawing.Size(715, 568)
        Me.txtReport.TabIndex = 4
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuAnalysis})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(739, 24)
        Me.MenuStrip1.TabIndex = 5
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSave})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuSave
        '
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuSave.Size = New System.Drawing.Size(147, 22)
        Me.mnuSave.Text = "&Save"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Name = "mnuAnalysis"
        Me.mnuAnalysis.Size = New System.Drawing.Size(58, 20)
        Me.mnuAnalysis.Text = "Analysis"
        '
        'frmDuration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(739, 610)
        Me.Controls.Add(Me.txtReport)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmDuration"
        Me.Text = "Duration Analysis"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtReport As System.Windows.Forms.TextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAnalysis As System.Windows.Forms.ToolStripMenuItem
End Class
