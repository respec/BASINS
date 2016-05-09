﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatchMap))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDataDir = New System.Windows.Forms.TextBox()
        Me.btnBrowseDataDir = New System.Windows.Forms.Button()
        Me.treeBFGroups = New System.Windows.Forms.TreeView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnCreateGroup = New System.Windows.Forms.Button()
        Me.btnPlotDuration = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnParmForm = New System.Windows.Forms.Button()
        Me.txtParameters = New System.Windows.Forms.TextBox()
        Me.btnDoBatch = New System.Windows.Forms.Button()
        Me.btnSaveSpecs = New System.Windows.Forms.Button()
        Me.cmsNode = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsGlobalSetParm = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsGroupSetParm = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsPlotDur = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnDownload = New System.Windows.Forms.Button()
        Me.btnGroupPlot = New System.Windows.Forms.Button()
        Me.btnGroupRemove = New System.Windows.Forms.Button()
        Me.btnGroupGroup = New System.Windows.Forms.Button()
        Me.btnGroupGlobal = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.chkGetNewest = New System.Windows.Forms.CheckBox()
        Me.lstStations = New atcControls.ATCoSelectListSortByProp()
        Me.GroupBox1.SuspendLayout()
        Me.cmsNode.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Data Path:"
        '
        'txtDataDir
        '
        Me.txtDataDir.Location = New System.Drawing.Point(77, 10)
        Me.txtDataDir.Name = "txtDataDir"
        Me.txtDataDir.Size = New System.Drawing.Size(288, 20)
        Me.txtDataDir.TabIndex = 1
        '
        'btnBrowseDataDir
        '
        Me.btnBrowseDataDir.Location = New System.Drawing.Point(371, 8)
        Me.btnBrowseDataDir.Name = "btnBrowseDataDir"
        Me.btnBrowseDataDir.Size = New System.Drawing.Size(61, 23)
        Me.btnBrowseDataDir.TabIndex = 2
        Me.btnBrowseDataDir.Text = "Browse"
        Me.btnBrowseDataDir.UseVisualStyleBackColor = True
        '
        'treeBFGroups
        '
        Me.treeBFGroups.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.treeBFGroups.Location = New System.Drawing.Point(16, 211)
        Me.treeBFGroups.Name = "treeBFGroups"
        Me.treeBFGroups.Size = New System.Drawing.Size(237, 161)
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
        Me.btnCreateGroup.Location = New System.Drawing.Point(444, 62)
        Me.btnCreateGroup.Name = "btnCreateGroup"
        Me.btnCreateGroup.Size = New System.Drawing.Size(75, 36)
        Me.btnCreateGroup.TabIndex = 6
        Me.btnCreateGroup.Text = "Create Group"
        Me.btnCreateGroup.UseVisualStyleBackColor = True
        '
        'btnPlotDuration
        '
        Me.btnPlotDuration.Location = New System.Drawing.Point(444, 104)
        Me.btnPlotDuration.Name = "btnPlotDuration"
        Me.btnPlotDuration.Size = New System.Drawing.Size(75, 38)
        Me.btnPlotDuration.TabIndex = 7
        Me.btnPlotDuration.Text = "Plot Data" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Timespan"
        Me.btnPlotDuration.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btnParmForm)
        Me.GroupBox1.Controls.Add(Me.txtParameters)
        Me.GroupBox1.Location = New System.Drawing.Point(259, 195)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(346, 177)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Group Parameters"
        '
        'btnParmForm
        '
        Me.btnParmForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnParmForm.Location = New System.Drawing.Point(6, 148)
        Me.btnParmForm.Name = "btnParmForm"
        Me.btnParmForm.Size = New System.Drawing.Size(75, 23)
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
        Me.txtParameters.Location = New System.Drawing.Point(7, 16)
        Me.txtParameters.Multiline = True
        Me.txtParameters.Name = "txtParameters"
        Me.txtParameters.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtParameters.Size = New System.Drawing.Size(333, 126)
        Me.txtParameters.TabIndex = 0
        '
        'btnDoBatch
        '
        Me.btnDoBatch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDoBatch.Location = New System.Drawing.Point(530, 378)
        Me.btnDoBatch.Name = "btnDoBatch"
        Me.btnDoBatch.Size = New System.Drawing.Size(75, 23)
        Me.btnDoBatch.TabIndex = 9
        Me.btnDoBatch.Text = "Do Batch"
        Me.btnDoBatch.UseVisualStyleBackColor = True
        '
        'btnSaveSpecs
        '
        Me.btnSaveSpecs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveSpecs.Location = New System.Drawing.Point(449, 378)
        Me.btnSaveSpecs.Name = "btnSaveSpecs"
        Me.btnSaveSpecs.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveSpecs.TabIndex = 10
        Me.btnSaveSpecs.Text = "Save Specs"
        Me.btnSaveSpecs.UseVisualStyleBackColor = True
        '
        'cmsNode
        '
        Me.cmsNode.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsGlobalSetParm, Me.cmsGroupSetParm, Me.cmsPlotDur, Me.cmsRemove})
        Me.cmsNode.Name = "cmsGroup"
        Me.cmsNode.Size = New System.Drawing.Size(190, 92)
        '
        'cmsGlobalSetParm
        '
        Me.cmsGlobalSetParm.Name = "cmsGlobalSetParm"
        Me.cmsGlobalSetParm.Size = New System.Drawing.Size(189, 22)
        Me.cmsGlobalSetParm.Text = "Set Global Parameters"
        '
        'cmsGroupSetParm
        '
        Me.cmsGroupSetParm.Name = "cmsGroupSetParm"
        Me.cmsGroupSetParm.Size = New System.Drawing.Size(189, 22)
        Me.cmsGroupSetParm.Text = "Set Group Parameters"
        '
        'cmsPlotDur
        '
        Me.cmsPlotDur.Name = "cmsPlotDur"
        Me.cmsPlotDur.Size = New System.Drawing.Size(189, 22)
        Me.cmsPlotDur.Text = "Plot Data Timespan"
        '
        'cmsRemove
        '
        Me.cmsRemove.Name = "cmsRemove"
        Me.cmsRemove.Size = New System.Drawing.Size(189, 22)
        Me.cmsRemove.Text = "Remove"
        '
        'btnDownload
        '
        Me.btnDownload.Location = New System.Drawing.Point(438, 8)
        Me.btnDownload.Name = "btnDownload"
        Me.btnDownload.Size = New System.Drawing.Size(75, 23)
        Me.btnDownload.TabIndex = 11
        Me.btnDownload.Text = "Download"
        Me.btnDownload.UseVisualStyleBackColor = True
        '
        'btnGroupPlot
        '
        Me.btnGroupPlot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGroupPlot.Location = New System.Drawing.Point(187, 378)
        Me.btnGroupPlot.Name = "btnGroupPlot"
        Me.btnGroupPlot.Size = New System.Drawing.Size(111, 23)
        Me.btnGroupPlot.TabIndex = 19
        Me.btnGroupPlot.Text = "Plot Data Timespan"
        Me.btnGroupPlot.UseVisualStyleBackColor = True
        '
        'btnGroupRemove
        '
        Me.btnGroupRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGroupRemove.Location = New System.Drawing.Point(125, 378)
        Me.btnGroupRemove.Name = "btnGroupRemove"
        Me.btnGroupRemove.Size = New System.Drawing.Size(56, 23)
        Me.btnGroupRemove.TabIndex = 18
        Me.btnGroupRemove.Text = "Remove"
        Me.btnGroupRemove.UseVisualStyleBackColor = True
        '
        'btnGroupGroup
        '
        Me.btnGroupGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGroupGroup.Location = New System.Drawing.Point(69, 378)
        Me.btnGroupGroup.Name = "btnGroupGroup"
        Me.btnGroupGroup.Size = New System.Drawing.Size(50, 23)
        Me.btnGroupGroup.TabIndex = 17
        Me.btnGroupGroup.Text = "Group"
        Me.btnGroupGroup.UseVisualStyleBackColor = True
        '
        'btnGroupGlobal
        '
        Me.btnGroupGlobal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGroupGlobal.Location = New System.Drawing.Point(16, 378)
        Me.btnGroupGlobal.Name = "btnGroupGlobal"
        Me.btnGroupGlobal.Size = New System.Drawing.Size(47, 23)
        Me.btnGroupGlobal.TabIndex = 16
        Me.btnGroupGlobal.Text = "Global"
        Me.btnGroupGlobal.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.Location = New System.Drawing.Point(304, 378)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(52, 23)
        Me.btnRefresh.TabIndex = 20
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'chkGetNewest
        '
        Me.chkGetNewest.AutoSize = True
        Me.chkGetNewest.Location = New System.Drawing.Point(519, 12)
        Me.chkGetNewest.Name = "chkGetNewest"
        Me.chkGetNewest.Size = New System.Drawing.Size(68, 17)
        Me.chkGetNewest.TabIndex = 46
        Me.chkGetNewest.Text = "Get New"
        Me.chkGetNewest.UseVisualStyleBackColor = True
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
        Me.lstStations.Size = New System.Drawing.Size(422, 151)
        Me.lstStations.SortMember = Nothing
        Me.lstStations.TabIndex = 3
        '
        'frmBatchMap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(617, 411)
        Me.Controls.Add(Me.chkGetNewest)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.btnGroupPlot)
        Me.Controls.Add(Me.btnGroupRemove)
        Me.Controls.Add(Me.btnGroupGroup)
        Me.Controls.Add(Me.btnGroupGlobal)
        Me.Controls.Add(Me.btnDownload)
        Me.Controls.Add(Me.btnSaveSpecs)
        Me.Controls.Add(Me.btnDoBatch)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnPlotDuration)
        Me.Controls.Add(Me.btnCreateGroup)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.treeBFGroups)
        Me.Controls.Add(Me.lstStations)
        Me.Controls.Add(Me.txtDataDir)
        Me.Controls.Add(Me.btnBrowseDataDir)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBatchMap"
        Me.Text = "Base-flow Separation Batch From Map"
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
    Friend WithEvents btnGroupPlot As Windows.Forms.Button
    Friend WithEvents btnGroupRemove As Windows.Forms.Button
    Friend WithEvents btnGroupGroup As Windows.Forms.Button
    Friend WithEvents btnGroupGlobal As Windows.Forms.Button
    Friend WithEvents btnRefresh As Windows.Forms.Button
    Friend WithEvents chkGetNewest As Windows.Forms.CheckBox
End Class
