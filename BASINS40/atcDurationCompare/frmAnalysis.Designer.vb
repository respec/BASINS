<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAnalysis
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAnalysis))
        Me.lblChooseGraphs = New System.Windows.Forms.Label
        Me.btnReport = New System.Windows.Forms.Button
        Me.btnGraph = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.gbClassLimits = New System.Windows.Forms.GroupBox
        Me.lstClassLimits = New atcControls.atcManagedList
        Me.gbChooseAnalysis = New System.Windows.Forms.GroupBox
        Me.lblAnalysisInfo = New System.Windows.Forms.TextBox
        Me.rdoAnalysisCompare = New System.Windows.Forms.RadioButton
        Me.rdoAnalysisDuration = New System.Windows.Forms.RadioButton
        Me.menuStrip = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.SelectDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CtlClassLimits1 = New atcDurationCompare.ctlClassLimits
        Me.gbClassLimits.SuspendLayout()
        Me.gbChooseAnalysis.SuspendLayout()
        Me.menuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblChooseGraphs
        '
        Me.lblChooseGraphs.AutoSize = True
        Me.lblChooseGraphs.Location = New System.Drawing.Point(12, 9)
        Me.lblChooseGraphs.Name = "lblChooseGraphs"
        Me.lblChooseGraphs.Size = New System.Drawing.Size(0, 13)
        Me.lblChooseGraphs.TabIndex = 1
        '
        'btnReport
        '
        Me.btnReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReport.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnReport.Location = New System.Drawing.Point(307, 365)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(75, 23)
        Me.btnReport.TabIndex = 4
        Me.btnReport.Text = "Report"
        Me.btnReport.UseVisualStyleBackColor = True
        '
        'btnGraph
        '
        Me.btnGraph.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGraph.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnGraph.Location = New System.Drawing.Point(226, 365)
        Me.btnGraph.Name = "btnGraph"
        Me.btnGraph.Size = New System.Drawing.Size(75, 23)
        Me.btnGraph.TabIndex = 3
        Me.btnGraph.Text = "Graph"
        Me.btnGraph.UseVisualStyleBackColor = True
        '
        'gbClassLimits
        '
        Me.gbClassLimits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbClassLimits.Controls.Add(Me.lstClassLimits)
        Me.gbClassLimits.Location = New System.Drawing.Point(165, 27)
        Me.gbClassLimits.Name = "gbClassLimits"
        Me.gbClassLimits.Size = New System.Drawing.Size(217, 200)
        Me.gbClassLimits.TabIndex = 5
        Me.gbClassLimits.TabStop = False
        Me.gbClassLimits.Text = "Class Limits"
        '
        'lstClassLimits
        '
        Me.lstClassLimits.CurrentValues = New Double(-1) {}
        Me.lstClassLimits.Location = New System.Drawing.Point(6, 19)
        Me.lstClassLimits.Name = "lstClassLimits"
        Me.lstClassLimits.Size = New System.Drawing.Size(207, 175)
        Me.lstClassLimits.TabIndex = 0
        '
        'gbChooseAnalysis
        '
        Me.gbChooseAnalysis.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbChooseAnalysis.Controls.Add(Me.lblAnalysisInfo)
        Me.gbChooseAnalysis.Controls.Add(Me.rdoAnalysisCompare)
        Me.gbChooseAnalysis.Controls.Add(Me.rdoAnalysisDuration)
        Me.gbChooseAnalysis.Location = New System.Drawing.Point(12, 233)
        Me.gbChooseAnalysis.Name = "gbChooseAnalysis"
        Me.gbChooseAnalysis.Size = New System.Drawing.Size(370, 126)
        Me.gbChooseAnalysis.TabIndex = 8
        Me.gbChooseAnalysis.TabStop = False
        Me.gbChooseAnalysis.Text = "Choose Analysis"
        '
        'lblAnalysisInfo
        '
        Me.lblAnalysisInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAnalysisInfo.BackColor = System.Drawing.SystemColors.Control
        Me.lblAnalysisInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblAnalysisInfo.Location = New System.Drawing.Point(7, 43)
        Me.lblAnalysisInfo.Multiline = True
        Me.lblAnalysisInfo.Name = "lblAnalysisInfo"
        Me.lblAnalysisInfo.ReadOnly = True
        Me.lblAnalysisInfo.Size = New System.Drawing.Size(357, 77)
        Me.lblAnalysisInfo.TabIndex = 5
        Me.lblAnalysisInfo.Text = "lblAnalysisInfo"
        '
        'rdoAnalysisCompare
        '
        Me.rdoAnalysisCompare.AutoSize = True
        Me.rdoAnalysisCompare.Location = New System.Drawing.Point(89, 20)
        Me.rdoAnalysisCompare.Name = "rdoAnalysisCompare"
        Me.rdoAnalysisCompare.Size = New System.Drawing.Size(67, 17)
        Me.rdoAnalysisCompare.TabIndex = 1
        Me.rdoAnalysisCompare.TabStop = True
        Me.rdoAnalysisCompare.Text = "Compare"
        Me.rdoAnalysisCompare.UseVisualStyleBackColor = True
        '
        'rdoAnalysisDuration
        '
        Me.rdoAnalysisDuration.AutoSize = True
        Me.rdoAnalysisDuration.Location = New System.Drawing.Point(7, 20)
        Me.rdoAnalysisDuration.Name = "rdoAnalysisDuration"
        Me.rdoAnalysisDuration.Size = New System.Drawing.Size(65, 17)
        Me.rdoAnalysisDuration.TabIndex = 0
        Me.rdoAnalysisDuration.TabStop = True
        Me.rdoAnalysisDuration.Text = "Duration"
        Me.rdoAnalysisDuration.UseVisualStyleBackColor = True
        '
        'menuStrip
        '
        Me.menuStrip.BackColor = System.Drawing.SystemColors.Control
        Me.menuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.HelpToolStripMenuItem})
        Me.menuStrip.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip.Name = "menuStrip"
        Me.menuStrip.Size = New System.Drawing.Size(394, 24)
        Me.menuStrip.TabIndex = 11
        Me.menuStrip.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectDataToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "File"
        '
        'SelectDataToolStripMenuItem
        '
        Me.SelectDataToolStripMenuItem.Name = "SelectDataToolStripMenuItem"
        Me.SelectDataToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SelectDataToolStripMenuItem.Text = "Select Data..."
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'CtlClassLimits1
        '
        Me.CtlClassLimits1.Location = New System.Drawing.Point(12, 27)
        Me.CtlClassLimits1.LowerBound = -9999
        Me.CtlClassLimits1.Name = "CtlClassLimits1"
        Me.CtlClassLimits1.NumClasses = 0
        Me.CtlClassLimits1.Size = New System.Drawing.Size(147, 200)
        Me.CtlClassLimits1.TabIndex = 9
        Me.CtlClassLimits1.UpperBound = -9999
        '
        'frmAnalysis
        '
        Me.AcceptButton = Me.btnReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(394, 400)
        Me.Controls.Add(Me.CtlClassLimits1)
        Me.Controls.Add(Me.gbChooseAnalysis)
        Me.Controls.Add(Me.btnGraph)
        Me.Controls.Add(Me.btnReport)
        Me.Controls.Add(Me.lblChooseGraphs)
        Me.Controls.Add(Me.gbClassLimits)
        Me.Controls.Add(Me.menuStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.menuStrip
        Me.Name = "frmAnalysis"
        Me.Text = "Duration/Compare Analysis"
        Me.gbClassLimits.ResumeLayout(False)
        Me.gbChooseAnalysis.ResumeLayout(False)
        Me.gbChooseAnalysis.PerformLayout()
        Me.menuStrip.ResumeLayout(False)
        Me.menuStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblChooseGraphs As System.Windows.Forms.Label
    Friend WithEvents btnReport As System.Windows.Forms.Button
    Friend WithEvents btnGraph As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents gbClassLimits As System.Windows.Forms.GroupBox
    Friend WithEvents lstClassLimits As atcControls.atcManagedList
    Friend WithEvents gbChooseAnalysis As System.Windows.Forms.GroupBox
    Friend WithEvents rdoAnalysisCompare As System.Windows.Forms.RadioButton
    Friend WithEvents rdoAnalysisDuration As System.Windows.Forms.RadioButton
    Friend WithEvents CtlClassLimits1 As atcDurationCompare.ctlClassLimits
    Friend WithEvents menuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblAnalysisInfo As System.Windows.Forms.TextBox
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
