<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchMap
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatchMap))
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtDataDir = New System.Windows.Forms.TextBox
        Me.btnBrowseDataDir = New System.Windows.Forms.Button
        Me.lstStations = New atcControls.ATCoSelectListSortByProp
        Me.treeBFGroups = New System.Windows.Forms.TreeView
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnCreateGroup = New System.Windows.Forms.Button
        Me.btnPlotDuration = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnParmForm = New System.Windows.Forms.Button
        Me.txtParameters = New System.Windows.Forms.TextBox
        Me.btnDoBatch = New System.Windows.Forms.Button
        Me.btnSaveSpecs = New System.Windows.Forms.Button
        Me.cmsNode = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsGlobalSetParm = New System.Windows.Forms.ToolStripMenuItem
        Me.cmsGroupSetParm = New System.Windows.Forms.ToolStripMenuItem
        Me.cmsPlotDur = New System.Windows.Forms.ToolStripMenuItem
        Me.cmsRemove = New System.Windows.Forms.ToolStripMenuItem
        Me.btnDownload = New System.Windows.Forms.Button
        Me.rdoSWSTAT = New System.Windows.Forms.RadioButton
        Me.rdoDFLOW = New System.Windows.Forms.RadioButton
        Me.rdoBF = New System.Windows.Forms.RadioButton
        Me.GroupBox1.SuspendLayout()
        Me.cmsNode.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 16)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Data Path:"
        '
        'txtDataDir
        '
        Me.txtDataDir.Location = New System.Drawing.Point(103, 12)
        Me.txtDataDir.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDataDir.Name = "txtDataDir"
        Me.txtDataDir.Size = New System.Drawing.Size(383, 22)
        Me.txtDataDir.TabIndex = 1
        '
        'btnBrowseDataDir
        '
        Me.btnBrowseDataDir.Location = New System.Drawing.Point(495, 10)
        Me.btnBrowseDataDir.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBrowseDataDir.Name = "btnBrowseDataDir"
        Me.btnBrowseDataDir.Size = New System.Drawing.Size(81, 28)
        Me.btnBrowseDataDir.TabIndex = 2
        Me.btnBrowseDataDir.Text = "Browse"
        Me.btnBrowseDataDir.UseVisualStyleBackColor = True
        '
        'lstStations
        '
        Me.lstStations.DisplayMember = ""
        Me.lstStations.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstStations.LeftLabel = "Available:"
        Me.lstStations.Location = New System.Drawing.Point(21, 46)
        Me.lstStations.Margin = New System.Windows.Forms.Padding(4)
        Me.lstStations.MoveDownTip = "Move Item Down In List"
        Me.lstStations.MoveUpTip = "Move Item Up In List"
        Me.lstStations.Name = "lstStations"
        Me.lstStations.RightLabel = "Selected:"
        Me.lstStations.Size = New System.Drawing.Size(551, 186)
        Me.lstStations.SortMember = Nothing
        Me.lstStations.TabIndex = 3
        '
        'treeBFGroups
        '
        Me.treeBFGroups.Location = New System.Drawing.Point(21, 260)
        Me.treeBFGroups.Margin = New System.Windows.Forms.Padding(4)
        Me.treeBFGroups.Name = "treeBFGroups"
        Me.treeBFGroups.Size = New System.Drawing.Size(181, 212)
        Me.treeBFGroups.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 240)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Batch Groups"
        '
        'btnCreateGroup
        '
        Me.btnCreateGroup.Location = New System.Drawing.Point(580, 59)
        Me.btnCreateGroup.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCreateGroup.Name = "btnCreateGroup"
        Me.btnCreateGroup.Size = New System.Drawing.Size(100, 44)
        Me.btnCreateGroup.TabIndex = 6
        Me.btnCreateGroup.Text = "Create Group"
        Me.btnCreateGroup.UseVisualStyleBackColor = True
        '
        'btnPlotDuration
        '
        Me.btnPlotDuration.Location = New System.Drawing.Point(580, 111)
        Me.btnPlotDuration.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPlotDuration.Name = "btnPlotDuration"
        Me.btnPlotDuration.Size = New System.Drawing.Size(100, 28)
        Me.btnPlotDuration.TabIndex = 7
        Me.btnPlotDuration.Text = "Durations"
        Me.btnPlotDuration.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btnParmForm)
        Me.GroupBox1.Controls.Add(Me.txtParameters)
        Me.GroupBox1.Location = New System.Drawing.Point(213, 240)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(554, 233)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Group Parameters"
        '
        'btnParmForm
        '
        Me.btnParmForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnParmForm.Location = New System.Drawing.Point(8, 197)
        Me.btnParmForm.Margin = New System.Windows.Forms.Padding(4)
        Me.btnParmForm.Name = "btnParmForm"
        Me.btnParmForm.Size = New System.Drawing.Size(100, 28)
        Me.btnParmForm.TabIndex = 1
        Me.btnParmForm.Text = "Form View"
        Me.btnParmForm.UseVisualStyleBackColor = True
        '
        'txtParameters
        '
        Me.txtParameters.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtParameters.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParameters.Location = New System.Drawing.Point(9, 20)
        Me.txtParameters.Margin = New System.Windows.Forms.Padding(4)
        Me.txtParameters.Multiline = True
        Me.txtParameters.Name = "txtParameters"
        Me.txtParameters.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtParameters.Size = New System.Drawing.Size(536, 169)
        Me.txtParameters.TabIndex = 0
        '
        'btnDoBatch
        '
        Me.btnDoBatch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDoBatch.Location = New System.Drawing.Point(668, 480)
        Me.btnDoBatch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDoBatch.Name = "btnDoBatch"
        Me.btnDoBatch.Size = New System.Drawing.Size(100, 28)
        Me.btnDoBatch.TabIndex = 9
        Me.btnDoBatch.Text = "Do Batch"
        Me.btnDoBatch.UseVisualStyleBackColor = True
        '
        'btnSaveSpecs
        '
        Me.btnSaveSpecs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveSpecs.Location = New System.Drawing.Point(560, 480)
        Me.btnSaveSpecs.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSaveSpecs.Name = "btnSaveSpecs"
        Me.btnSaveSpecs.Size = New System.Drawing.Size(100, 28)
        Me.btnSaveSpecs.TabIndex = 10
        Me.btnSaveSpecs.Text = "Save Specs"
        Me.btnSaveSpecs.UseVisualStyleBackColor = True
        '
        'cmsNode
        '
        Me.cmsNode.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsGlobalSetParm, Me.cmsGroupSetParm, Me.cmsPlotDur, Me.cmsRemove})
        Me.cmsNode.Name = "cmsGroup"
        Me.cmsNode.Size = New System.Drawing.Size(226, 100)
        '
        'cmsGlobalSetParm
        '
        Me.cmsGlobalSetParm.Name = "cmsGlobalSetParm"
        Me.cmsGlobalSetParm.Size = New System.Drawing.Size(225, 24)
        Me.cmsGlobalSetParm.Text = "Set Global Parameters"
        '
        'cmsGroupSetParm
        '
        Me.cmsGroupSetParm.Name = "cmsGroupSetParm"
        Me.cmsGroupSetParm.Size = New System.Drawing.Size(225, 24)
        Me.cmsGroupSetParm.Text = "Set Group Parameters"
        '
        'cmsPlotDur
        '
        Me.cmsPlotDur.Name = "cmsPlotDur"
        Me.cmsPlotDur.Size = New System.Drawing.Size(225, 24)
        Me.cmsPlotDur.Text = "Plot Duration"
        '
        'cmsRemove
        '
        Me.cmsRemove.Name = "cmsRemove"
        Me.cmsRemove.Size = New System.Drawing.Size(225, 24)
        Me.cmsRemove.Text = "Remove"
        '
        'btnDownload
        '
        Me.btnDownload.Location = New System.Drawing.Point(584, 10)
        Me.btnDownload.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDownload.Name = "btnDownload"
        Me.btnDownload.Size = New System.Drawing.Size(100, 28)
        Me.btnDownload.TabIndex = 11
        Me.btnDownload.Text = "Download"
        Me.btnDownload.UseVisualStyleBackColor = True
        '
        'rdoSWSTAT
        '
        Me.rdoSWSTAT.AutoSize = True
        Me.rdoSWSTAT.Location = New System.Drawing.Point(584, 147)
        Me.rdoSWSTAT.Name = "rdoSWSTAT"
        Me.rdoSWSTAT.Size = New System.Drawing.Size(135, 21)
        Me.rdoSWSTAT.TabIndex = 12
        Me.rdoSWSTAT.TabStop = True
        Me.rdoSWSTAT.Text = "Integrated Trend"
        Me.rdoSWSTAT.UseVisualStyleBackColor = True
        '
        'rdoDFLOW
        '
        Me.rdoDFLOW.AutoSize = True
        Me.rdoDFLOW.Location = New System.Drawing.Point(584, 175)
        Me.rdoDFLOW.Name = "rdoDFLOW"
        Me.rdoDFLOW.Size = New System.Drawing.Size(79, 21)
        Me.rdoDFLOW.TabIndex = 13
        Me.rdoDFLOW.TabStop = True
        Me.rdoDFLOW.Text = "DFLOW"
        Me.rdoDFLOW.UseVisualStyleBackColor = True
        '
        'rdoBF
        '
        Me.rdoBF.AutoSize = True
        Me.rdoBF.Location = New System.Drawing.Point(584, 203)
        Me.rdoBF.Name = "rdoBF"
        Me.rdoBF.Size = New System.Drawing.Size(163, 21)
        Me.rdoBF.TabIndex = 14
        Me.rdoBF.TabStop = True
        Me.rdoBF.Text = "Base-flow Separation"
        Me.rdoBF.UseVisualStyleBackColor = True
        '
        'frmBatchMap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 521)
        Me.Controls.Add(Me.rdoBF)
        Me.Controls.Add(Me.rdoDFLOW)
        Me.Controls.Add(Me.rdoSWSTAT)
        Me.Controls.Add(Me.btnDownload)
        Me.Controls.Add(Me.btnSaveSpecs)
        Me.Controls.Add(Me.btnDoBatch)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnPlotDuration)
        Me.Controls.Add(Me.btnCreateGroup)
        Me.Controls.Add(Me.treeBFGroups)
        Me.Controls.Add(Me.lstStations)
        Me.Controls.Add(Me.txtDataDir)
        Me.Controls.Add(Me.btnBrowseDataDir)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmBatchMap"
        Me.Text = "Batch Processing From Map"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.cmsNode.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDataDir As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseDataDir As System.Windows.Forms.Button
    Friend WithEvents lstStations As atcControls.ATCoSelectListSortByProp
    Friend WithEvents treeBFGroups As System.Windows.Forms.TreeView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnCreateGroup As System.Windows.Forms.Button
    Friend WithEvents btnPlotDuration As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtParameters As System.Windows.Forms.TextBox
    Friend WithEvents btnParmForm As System.Windows.Forms.Button
    Friend WithEvents btnDoBatch As System.Windows.Forms.Button
    Friend WithEvents btnSaveSpecs As System.Windows.Forms.Button
    Friend WithEvents cmsNode As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmsRemove As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsPlotDur As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsGroupSetParm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsGlobalSetParm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnDownload As System.Windows.Forms.Button
    Friend WithEvents rdoSWSTAT As System.Windows.Forms.RadioButton
    Friend WithEvents rdoDFLOW As System.Windows.Forms.RadioButton
    Friend WithEvents rdoBF As System.Windows.Forms.RadioButton
End Class
