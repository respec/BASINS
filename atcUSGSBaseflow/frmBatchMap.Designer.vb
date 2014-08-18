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
        Me.cmsGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsGroupRemove = New System.Windows.Forms.ToolStripMenuItem
        Me.cmsPlotDur = New System.Windows.Forms.ToolStripMenuItem
        Me.cmsGroupSetGlobal = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1.SuspendLayout()
        Me.cmsGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Save Download Data:"
        '
        'txtDataDir
        '
        Me.txtDataDir.Location = New System.Drawing.Point(131, 10)
        Me.txtDataDir.Name = "txtDataDir"
        Me.txtDataDir.Size = New System.Drawing.Size(298, 20)
        Me.txtDataDir.TabIndex = 1
        '
        'btnBrowseDataDir
        '
        Me.btnBrowseDataDir.Location = New System.Drawing.Point(436, 8)
        Me.btnBrowseDataDir.Name = "btnBrowseDataDir"
        Me.btnBrowseDataDir.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseDataDir.TabIndex = 2
        Me.btnBrowseDataDir.Text = "Browse"
        Me.btnBrowseDataDir.UseVisualStyleBackColor = True
        '
        'lstStations
        '
        Me.lstStations.DisplayMember = ""
        Me.lstStations.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstStations.LeftLabel = "Available:"
        Me.lstStations.Location = New System.Drawing.Point(16, 37)
        Me.lstStations.MoveDownTip = "Move Item Down In List"
        Me.lstStations.MoveUpTip = "Move Item Up In List"
        Me.lstStations.Name = "lstStations"
        Me.lstStations.RightLabel = "Selected:"
        Me.lstStations.Size = New System.Drawing.Size(413, 151)
        Me.lstStations.SortMember = Nothing
        Me.lstStations.TabIndex = 3
        '
        'treeBFGroups
        '
        Me.treeBFGroups.Location = New System.Drawing.Point(16, 211)
        Me.treeBFGroups.Name = "treeBFGroups"
        Me.treeBFGroups.Size = New System.Drawing.Size(137, 173)
        Me.treeBFGroups.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 195)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Batch Groups"
        '
        'btnCreateGroup
        '
        Me.btnCreateGroup.Location = New System.Drawing.Point(435, 61)
        Me.btnCreateGroup.Name = "btnCreateGroup"
        Me.btnCreateGroup.Size = New System.Drawing.Size(75, 36)
        Me.btnCreateGroup.TabIndex = 6
        Me.btnCreateGroup.Text = "Create Group"
        Me.btnCreateGroup.UseVisualStyleBackColor = True
        '
        'btnPlotDuration
        '
        Me.btnPlotDuration.Location = New System.Drawing.Point(436, 104)
        Me.btnPlotDuration.Name = "btnPlotDuration"
        Me.btnPlotDuration.Size = New System.Drawing.Size(75, 23)
        Me.btnPlotDuration.TabIndex = 7
        Me.btnPlotDuration.Text = "Durations"
        Me.btnPlotDuration.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnParmForm)
        Me.GroupBox1.Controls.Add(Me.txtParameters)
        Me.GroupBox1.Location = New System.Drawing.Point(160, 195)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(349, 189)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Group Parameters"
        '
        'btnParmForm
        '
        Me.btnParmForm.Location = New System.Drawing.Point(6, 160)
        Me.btnParmForm.Name = "btnParmForm"
        Me.btnParmForm.Size = New System.Drawing.Size(75, 23)
        Me.btnParmForm.TabIndex = 1
        Me.btnParmForm.Text = "Form View"
        Me.btnParmForm.UseVisualStyleBackColor = True
        '
        'txtParameters
        '
        Me.txtParameters.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParameters.Location = New System.Drawing.Point(7, 16)
        Me.txtParameters.Multiline = True
        Me.txtParameters.Name = "txtParameters"
        Me.txtParameters.Size = New System.Drawing.Size(336, 138)
        Me.txtParameters.TabIndex = 0
        '
        'btnDoBatch
        '
        Me.btnDoBatch.Location = New System.Drawing.Point(434, 390)
        Me.btnDoBatch.Name = "btnDoBatch"
        Me.btnDoBatch.Size = New System.Drawing.Size(75, 23)
        Me.btnDoBatch.TabIndex = 9
        Me.btnDoBatch.Text = "Do Batch"
        Me.btnDoBatch.UseVisualStyleBackColor = True
        '
        'btnSaveSpecs
        '
        Me.btnSaveSpecs.Location = New System.Drawing.Point(353, 390)
        Me.btnSaveSpecs.Name = "btnSaveSpecs"
        Me.btnSaveSpecs.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveSpecs.TabIndex = 10
        Me.btnSaveSpecs.Text = "Save Specs"
        Me.btnSaveSpecs.UseVisualStyleBackColor = True
        '
        'cmsGroup
        '
        Me.cmsGroup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsGroupRemove, Me.cmsPlotDur, Me.cmsGroupSetGlobal})
        Me.cmsGroup.Name = "cmsGroup"
        Me.cmsGroup.Size = New System.Drawing.Size(181, 92)
        '
        'cmsGroupRemove
        '
        Me.cmsGroupRemove.Name = "cmsGroupRemove"
        Me.cmsGroupRemove.Size = New System.Drawing.Size(180, 22)
        Me.cmsGroupRemove.Text = "Remove"
        '
        'cmsPlotDur
        '
        Me.cmsPlotDur.Name = "cmsPlotDur"
        Me.cmsPlotDur.Size = New System.Drawing.Size(180, 22)
        Me.cmsPlotDur.Text = "Plot Duration"
        '
        'cmsGroupSetGlobal
        '
        Me.cmsGroupSetGlobal.Name = "cmsGroupSetGlobal"
        Me.cmsGroupSetGlobal.Size = New System.Drawing.Size(180, 22)
        Me.cmsGroupSetGlobal.Text = "Set Global Parameters"
        '
        'frmBatchMap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 423)
        Me.Controls.Add(Me.btnSaveSpecs)
        Me.Controls.Add(Me.btnDoBatch)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnPlotDuration)
        Me.Controls.Add(Me.btnCreateGroup)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.treeBFGroups)
        Me.Controls.Add(Me.lstStations)
        Me.Controls.Add(Me.btnBrowseDataDir)
        Me.Controls.Add(Me.txtDataDir)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBatchMap"
        Me.Text = "Base-flow Separation Batch From Map"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.cmsGroup.ResumeLayout(False)
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
    Friend WithEvents cmsGroup As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmsGroupRemove As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsPlotDur As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsGroupSetGlobal As System.Windows.Forms.ToolStripMenuItem
End Class
