<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDurationHydrographControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDurationHydrographControl))
        Me.lstPctExceed = New atcControls.atcManagedList
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSelectData = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem
        Me.lblExceedance = New System.Windows.Forms.Label
        Me.btnReport = New System.Windows.Forms.Button
        Me.btnGraph = New System.Windows.Forms.Button
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstPctExceed
        '
        Me.lstPctExceed.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstPctExceed.CurrentValues = New Double(-1) {}
        Me.lstPctExceed.Location = New System.Drawing.Point(12, 44)
        Me.lstPctExceed.Name = "lstPctExceed"
        Me.lstPctExceed.Size = New System.Drawing.Size(213, 292)
        Me.lstPctExceed.TabIndex = 0
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(237, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSelectData, Me.mnuExit})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'mnuSelectData
        '
        Me.mnuSelectData.Name = "mnuSelectData"
        Me.mnuSelectData.Size = New System.Drawing.Size(140, 22)
        Me.mnuSelectData.Text = "Select Data"
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(140, 22)
        Me.mnuExit.Text = "Exit"
        '
        'lblExceedance
        '
        Me.lblExceedance.AutoSize = True
        Me.lblExceedance.Location = New System.Drawing.Point(13, 28)
        Me.lblExceedance.Name = "lblExceedance"
        Me.lblExceedance.Size = New System.Drawing.Size(140, 13)
        Me.lblExceedance.TabIndex = 2
        Me.lblExceedance.Text = "Select Percent Exceedance"
        '
        'btnReport
        '
        Me.btnReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReport.Location = New System.Drawing.Point(150, 342)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(75, 23)
        Me.btnReport.TabIndex = 3
        Me.btnReport.Text = "Report"
        Me.btnReport.UseVisualStyleBackColor = True
        '
        'btnGraph
        '
        Me.btnGraph.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGraph.Location = New System.Drawing.Point(69, 342)
        Me.btnGraph.Name = "btnGraph"
        Me.btnGraph.Size = New System.Drawing.Size(75, 23)
        Me.btnGraph.TabIndex = 4
        Me.btnGraph.Text = "Graph"
        Me.btnGraph.UseVisualStyleBackColor = True
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'frmDurationHydrographControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(237, 377)
        Me.Controls.Add(Me.btnGraph)
        Me.Controls.Add(Me.btnReport)
        Me.Controls.Add(Me.lblExceedance)
        Me.Controls.Add(Me.lstPctExceed)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmDurationHydrographControl"
        Me.Text = "Duration Hydrograph"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstPctExceed As atcControls.atcManagedList
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSelectData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblExceedance As System.Windows.Forms.Label
    Friend WithEvents btnReport As System.Windows.Forms.Button
    Friend WithEvents btnGraph As System.Windows.Forms.Button
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
