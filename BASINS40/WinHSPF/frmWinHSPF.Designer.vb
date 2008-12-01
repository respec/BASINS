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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWinHSPF))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FunctionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ReachEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LandUseEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InputDataEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PollutantToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.AQUATOXToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BMPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HSPFparmToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StarterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.OpenToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.PrintToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.HelpToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.cmdReach = New System.Windows.Forms.ToolStripButton
        Me.cmdTime = New System.Windows.Forms.ToolStripButton
        Me.cmdLandUse = New System.Windows.Forms.ToolStripButton
        Me.cmdControl = New System.Windows.Forms.ToolStripDropDownButton
        Me.EditControlCardsWithToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.EditControlCardsWithTablesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditWithToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdPollutant = New System.Windows.Forms.ToolStripButton
        Me.cmdPoint = New System.Windows.Forms.ToolStripButton
        Me.cmdToolStripInputEditor = New System.Windows.Forms.ToolStripButton
        Me.cmdToolStripOutput = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton10 = New System.Windows.Forms.ToolStripButton
        Me.cmdRunHSPF = New System.Windows.Forms.ToolStripButton
        Me.ToolStripContainer = New System.Windows.Forms.ToolStripContainer
        Me.SchematicDiagram = New ctlSchematic
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.ToolStripContainer.ContentPanel.SuspendLayout()
        Me.ToolStripContainer.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.FunctionsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(862, 27)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.CloseToolStripMenuItem, Me.SaveAsToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(45, 23)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(165, 24)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(165, 24)
        Me.SaveAsToolStripMenuItem.Text = "Save As..."
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(48, 23)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'FunctionsToolStripMenuItem
        '
        Me.FunctionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReachEditorToolStripMenuItem, Me.LandUseEditorToolStripMenuItem, Me.InputDataEditorToolStripMenuItem, Me.PollutantToolStripMenuItem, Me.ToolStripSeparator3, Me.AQUATOXToolStripMenuItem, Me.BMPToolStripMenuItem, Me.HSPFparmToolStripMenuItem, Me.StarterToolStripMenuItem})
        Me.FunctionsToolStripMenuItem.Name = "FunctionsToolStripMenuItem"
        Me.FunctionsToolStripMenuItem.Size = New System.Drawing.Size(88, 23)
        Me.FunctionsToolStripMenuItem.Text = "Functions"
        '
        'ReachEditorToolStripMenuItem
        '
        Me.ReachEditorToolStripMenuItem.Name = "ReachEditorToolStripMenuItem"
        Me.ReachEditorToolStripMenuItem.Size = New System.Drawing.Size(224, 24)
        Me.ReachEditorToolStripMenuItem.Text = "Reach Editor"
        '
        'LandUseEditorToolStripMenuItem
        '
        Me.LandUseEditorToolStripMenuItem.Name = "LandUseEditorToolStripMenuItem"
        Me.LandUseEditorToolStripMenuItem.Size = New System.Drawing.Size(224, 24)
        Me.LandUseEditorToolStripMenuItem.Text = "Land Use Editor"
        '
        'InputDataEditorToolStripMenuItem
        '
        Me.InputDataEditorToolStripMenuItem.Name = "InputDataEditorToolStripMenuItem"
        Me.InputDataEditorToolStripMenuItem.Size = New System.Drawing.Size(224, 24)
        Me.InputDataEditorToolStripMenuItem.Text = "Input Data Editor"
        '
        'PollutantToolStripMenuItem
        '
        Me.PollutantToolStripMenuItem.Name = "PollutantToolStripMenuItem"
        Me.PollutantToolStripMenuItem.Size = New System.Drawing.Size(224, 24)
        Me.PollutantToolStripMenuItem.Text = "Pollutant Selection"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(221, 6)
        '
        'AQUATOXToolStripMenuItem
        '
        Me.AQUATOXToolStripMenuItem.Name = "AQUATOXToolStripMenuItem"
        Me.AQUATOXToolStripMenuItem.Size = New System.Drawing.Size(224, 24)
        Me.AQUATOXToolStripMenuItem.Text = "AQUATOX"
        '
        'BMPToolStripMenuItem
        '
        Me.BMPToolStripMenuItem.Name = "BMPToolStripMenuItem"
        Me.BMPToolStripMenuItem.Size = New System.Drawing.Size(224, 24)
        Me.BMPToolStripMenuItem.Text = "BMP"
        '
        'HSPFparmToolStripMenuItem
        '
        Me.HSPFparmToolStripMenuItem.Name = "HSPFparmToolStripMenuItem"
        Me.HSPFparmToolStripMenuItem.Size = New System.Drawing.Size(224, 24)
        Me.HSPFparmToolStripMenuItem.Text = "HSPFparm"
        '
        'StarterToolStripMenuItem
        '
        Me.StarterToolStripMenuItem.Name = "StarterToolStripMenuItem"
        Me.StarterToolStripMenuItem.Size = New System.Drawing.Size(224, 24)
        Me.StarterToolStripMenuItem.Text = "Starter"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(53, 23)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(152, 24)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'ContentPanel
        '
        Me.ContentPanel.AutoScroll = True
        Me.ContentPanel.Size = New System.Drawing.Size(862, 483)
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.OpenToolStripButton, Me.SaveToolStripButton, Me.PrintToolStripButton, Me.toolStripSeparator2, Me.HelpToolStripButton})
        Me.ToolStrip2.Location = New System.Drawing.Point(3, 27)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(131, 25)
        Me.ToolStrip2.TabIndex = 0
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'NewToolStripButton
        '
        Me.NewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.NewToolStripButton.Image = CType(resources.GetObject("NewToolStripButton.Image"), System.Drawing.Image)
        Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripButton.Name = "NewToolStripButton"
        Me.NewToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.NewToolStripButton.Text = "&New"
        '
        'OpenToolStripButton
        '
        Me.OpenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.OpenToolStripButton.Image = CType(resources.GetObject("OpenToolStripButton.Image"), System.Drawing.Image)
        Me.OpenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenToolStripButton.Name = "OpenToolStripButton"
        Me.OpenToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.OpenToolStripButton.Text = "&Open"
        '
        'SaveToolStripButton
        '
        Me.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SaveToolStripButton.Image = CType(resources.GetObject("SaveToolStripButton.Image"), System.Drawing.Image)
        Me.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripButton.Name = "SaveToolStripButton"
        Me.SaveToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.SaveToolStripButton.Text = "&Save"
        '
        'PrintToolStripButton
        '
        Me.PrintToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.PrintToolStripButton.Image = CType(resources.GetObject("PrintToolStripButton.Image"), System.Drawing.Image)
        Me.PrintToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PrintToolStripButton.Name = "PrintToolStripButton"
        Me.PrintToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.PrintToolStripButton.Text = "&Print"
        '
        'toolStripSeparator2
        '
        Me.toolStripSeparator2.Name = "toolStripSeparator2"
        Me.toolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'HelpToolStripButton
        '
        Me.HelpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.HelpToolStripButton.Image = CType(resources.GetObject("HelpToolStripButton.Image"), System.Drawing.Image)
        Me.HelpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.HelpToolStripButton.Name = "HelpToolStripButton"
        Me.HelpToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.HelpToolStripButton.Text = "He&lp"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmdReach, Me.cmdTime, Me.cmdLandUse, Me.cmdControl, Me.cmdPollutant, Me.cmdPoint, Me.cmdToolStripInputEditor, Me.cmdToolStripOutput, Me.ToolStripButton10, Me.cmdRunHSPF})
        Me.ToolStrip1.Location = New System.Drawing.Point(134, 27)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(248, 27)
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'cmdReach
        '
        Me.cmdReach.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdReach.Image = CType(resources.GetObject("cmdReach.Image"), System.Drawing.Image)
        Me.cmdReach.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdReach.Name = "cmdReach"
        Me.cmdReach.Size = New System.Drawing.Size(23, 24)
        Me.cmdReach.Text = "Reach Editor"
        '
        'cmdTime
        '
        Me.cmdTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdTime.Image = CType(resources.GetObject("cmdTime.Image"), System.Drawing.Image)
        Me.cmdTime.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.cmdTime.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdTime.Name = "cmdTime"
        Me.cmdTime.Size = New System.Drawing.Size(25, 24)
        Me.cmdTime.Text = "Simulation Time and Meteorologic Data"
        '
        'cmdLandUse
        '
        Me.cmdLandUse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdLandUse.Image = CType(resources.GetObject("cmdLandUse.Image"), System.Drawing.Image)
        Me.cmdLandUse.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdLandUse.Name = "cmdLandUse"
        Me.cmdLandUse.Size = New System.Drawing.Size(23, 24)
        Me.cmdLandUse.Text = "Land Use Editor"
        '
        'cmdControl
        '
        Me.cmdControl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdControl.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditControlCardsWithToolStripMenuItem, Me.ToolStripSeparator1, Me.EditControlCardsWithTablesToolStripMenuItem, Me.EditWithToolStripMenuItem})
        Me.cmdControl.Image = CType(resources.GetObject("cmdControl.Image"), System.Drawing.Image)
        Me.cmdControl.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdControl.Name = "cmdControl"
        Me.cmdControl.Size = New System.Drawing.Size(29, 24)
        Me.cmdControl.Text = "Control Cards"
        Me.cmdControl.ToolTipText = "Control Cards"
        '
        'EditControlCardsWithToolStripMenuItem
        '
        Me.EditControlCardsWithToolStripMenuItem.Enabled = False
        Me.EditControlCardsWithToolStripMenuItem.Name = "EditControlCardsWithToolStripMenuItem"
        Me.EditControlCardsWithToolStripMenuItem.Size = New System.Drawing.Size(266, 24)
        Me.EditControlCardsWithToolStripMenuItem.Text = "Edit Control Cards With:"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(263, 6)
        '
        'EditControlCardsWithTablesToolStripMenuItem
        '
        Me.EditControlCardsWithTablesToolStripMenuItem.Name = "EditControlCardsWithTablesToolStripMenuItem"
        Me.EditControlCardsWithTablesToolStripMenuItem.Size = New System.Drawing.Size(266, 24)
        Me.EditControlCardsWithTablesToolStripMenuItem.Text = "Tables"
        '
        'EditWithToolStripMenuItem
        '
        Me.EditWithToolStripMenuItem.Name = "EditWithToolStripMenuItem"
        Me.EditWithToolStripMenuItem.Size = New System.Drawing.Size(266, 24)
        Me.EditWithToolStripMenuItem.Text = "Descriptions"
        '
        'cmdPollutant
        '
        Me.cmdPollutant.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdPollutant.Image = CType(resources.GetObject("cmdPollutant.Image"), System.Drawing.Image)
        Me.cmdPollutant.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdPollutant.Name = "cmdPollutant"
        Me.cmdPollutant.Size = New System.Drawing.Size(23, 24)
        Me.cmdPollutant.Text = "Pollutant Selection"
        '
        'cmdPoint
        '
        Me.cmdPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdPoint.Image = CType(resources.GetObject("cmdPoint.Image"), System.Drawing.Image)
        Me.cmdPoint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdPoint.Name = "cmdPoint"
        Me.cmdPoint.Size = New System.Drawing.Size(23, 24)
        Me.cmdPoint.Text = "Point Sources"
        '
        'cmdToolStripInputEditor
        '
        Me.cmdToolStripInputEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdToolStripInputEditor.Image = CType(resources.GetObject("cmdToolStripInputEditor.Image"), System.Drawing.Image)
        Me.cmdToolStripInputEditor.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdToolStripInputEditor.Name = "cmdToolStripInputEditor"
        Me.cmdToolStripInputEditor.Size = New System.Drawing.Size(23, 24)
        Me.cmdToolStripInputEditor.Text = "Input Data Editor"
        '
        'cmdToolStripOutput
        '
        Me.cmdToolStripOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdToolStripOutput.Image = CType(resources.GetObject("cmdToolStripOutput.Image"), System.Drawing.Image)
        Me.cmdToolStripOutput.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdToolStripOutput.Name = "cmdToolStripOutput"
        Me.cmdToolStripOutput.Size = New System.Drawing.Size(23, 24)
        Me.cmdToolStripOutput.Text = "Output"
        Me.cmdToolStripOutput.ToolTipText = "Output"
        '
        'ToolStripButton10
        '
        Me.ToolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton10.Image = CType(resources.GetObject("ToolStripButton10.Image"), System.Drawing.Image)
        Me.ToolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton10.Name = "ToolStripButton10"
        Me.ToolStripButton10.Size = New System.Drawing.Size(23, 24)
        Me.ToolStripButton10.Text = "ToolStripButton10"
        '
        'cmdRunHSPF
        '
        Me.cmdRunHSPF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdRunHSPF.Image = CType(resources.GetObject("cmdRunHSPF.Image"), System.Drawing.Image)
        Me.cmdRunHSPF.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdRunHSPF.Name = "cmdRunHSPF"
        Me.cmdRunHSPF.Size = New System.Drawing.Size(23, 24)
        Me.cmdRunHSPF.Text = "Run HSPF"
        '
        'ToolStripContainer
        '
        '
        'ToolStripContainer.ContentPanel
        '
        Me.ToolStripContainer.ContentPanel.AutoScroll = True
        Me.ToolStripContainer.ContentPanel.Controls.Add(Me.SchematicDiagram)
        Me.ToolStripContainer.ContentPanel.Size = New System.Drawing.Size(862, 453)
        Me.ToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer.LeftToolStripPanelVisible = False
        Me.ToolStripContainer.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer.Name = "ToolStripContainer"
        Me.ToolStripContainer.RightToolStripPanelVisible = False
        Me.ToolStripContainer.Size = New System.Drawing.Size(862, 507)
        Me.ToolStripContainer.TabIndex = 5
        Me.ToolStripContainer.Text = "ToolStripContainer2"
        '
        'ToolStripContainer.TopToolStripPanel
        '
        Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.MenuStrip1)
        Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.ToolStrip2)
        Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.ToolStrip1)
        '
        'SchematicDiagram
        '
        Me.SchematicDiagram.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SchematicDiagram.Location = New System.Drawing.Point(0, 0)
        Me.SchematicDiagram.Name = "SchematicDiagram"
        Me.SchematicDiagram.Size = New System.Drawing.Size(862, 453)
        Me.SchematicDiagram.TabIndex = 0
        Me.SchematicDiagram.UCI = Nothing
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(165, 24)
        Me.CloseToolStripMenuItem.Text = "Close"
        '
        'frmWinHSPF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(862, 507)
        Me.Controls.Add(Me.ToolStripContainer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmWinHSPF"
        Me.Tag = "Hydrological Simulation Program - Fortran (HSPF)"
        Me.Text = "Hydrological Simulation Program - Fortran (HSPF)"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ToolStripContainer.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer.ResumeLayout(False)
        Me.ToolStripContainer.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FunctionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReachEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LandUseEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InputDataEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PollutantToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents NewToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents OpenToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents PrintToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HelpToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents cmdReach As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdTime As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdLandUse As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdControl As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents EditControlCardsWithToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EditControlCardsWithTablesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditWithToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdPollutant As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdPoint As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdToolStripInputEditor As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdToolStripOutput As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdRunHSPF As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton10 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripContainer As System.Windows.Forms.ToolStripContainer
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AQUATOXToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BMPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HSPFparmToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StarterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SchematicDiagram As ctlSchematic
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
