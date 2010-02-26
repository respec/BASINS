<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPoint
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPoint))
        Me.chkAllSources = New System.Windows.Forms.CheckBox
        Me.grpSources = New System.Windows.Forms.GroupBox
        Me.menuPointSources = New System.Windows.Forms.MenuStrip
        Me.MenuAddPointSources = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdSimpleCreate = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdImportMustin = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdConvertMustin = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdAdvancedGen = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdScenario = New System.Windows.Forms.ToolStripMenuItem
        Me.lstPoints = New System.Windows.Forms.CheckedListBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.grpDetails = New System.Windows.Forms.GroupBox
        Me.agdPoint = New atcControls.atcGrid
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdList = New System.Windows.Forms.Button
        Me.cmdGraph = New System.Windows.Forms.Button
        Me.cmdDetailsShow = New System.Windows.Forms.Button
        Me.cmdDetailsHide = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.agdMasterPoint = New atcControls.atcGrid
        Me.grpSources.SuspendLayout()
        Me.menuPointSources.SuspendLayout()
        Me.grpDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkAllSources
        '
        Me.chkAllSources.AutoSize = True
        Me.chkAllSources.Location = New System.Drawing.Point(21, 63)
        Me.chkAllSources.Margin = New System.Windows.Forms.Padding(4)
        Me.chkAllSources.Name = "chkAllSources"
        Me.chkAllSources.Size = New System.Drawing.Size(144, 21)
        Me.chkAllSources.TabIndex = 7
        Me.chkAllSources.Text = "Select/Deselect All"
        Me.chkAllSources.UseVisualStyleBackColor = True
        '
        'grpSources
        '
        Me.grpSources.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpSources.Controls.Add(Me.menuPointSources)
        Me.grpSources.Controls.Add(Me.lstPoints)
        Me.grpSources.Controls.Add(Me.chkAllSources)
        Me.grpSources.Location = New System.Drawing.Point(16, 15)
        Me.grpSources.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSources.Name = "grpSources"
        Me.grpSources.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSources.Size = New System.Drawing.Size(327, 519)
        Me.grpSources.TabIndex = 14
        Me.grpSources.TabStop = False
        Me.grpSources.Text = "Point Sources"
        '
        'menuPointSources
        '
        Me.menuPointSources.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuAddPointSources, Me.cmdScenario})
        Me.menuPointSources.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.menuPointSources.Location = New System.Drawing.Point(4, 19)
        Me.menuPointSources.Name = "menuPointSources"
        Me.menuPointSources.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
        Me.menuPointSources.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.menuPointSources.Size = New System.Drawing.Size(319, 26)
        Me.menuPointSources.Stretch = False
        Me.menuPointSources.TabIndex = 18
        Me.menuPointSources.Text = "MenuStrip1"
        '
        'MenuAddPointSources
        '
        Me.MenuAddPointSources.BackColor = System.Drawing.SystemColors.Control
        Me.MenuAddPointSources.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmdSimpleCreate, Me.cmdImportMustin, Me.cmdConvertMustin, Me.cmdAdvancedGen})
        Me.MenuAddPointSources.ForeColor = System.Drawing.SystemColors.ControlText
        Me.MenuAddPointSources.Image = CType(resources.GetObject("MenuAddPointSources.Image"), System.Drawing.Image)
        Me.MenuAddPointSources.Name = "MenuAddPointSources"
        Me.MenuAddPointSources.Size = New System.Drawing.Size(146, 22)
        Me.MenuAddPointSources.Text = "Add Point Source"
        '
        'cmdSimpleCreate
        '
        Me.cmdSimpleCreate.Name = "cmdSimpleCreate"
        Me.cmdSimpleCreate.Size = New System.Drawing.Size(293, 22)
        Me.cmdSimpleCreate.Text = "Simple Create"
        '
        'cmdImportMustin
        '
        Me.cmdImportMustin.Name = "cmdImportMustin"
        Me.cmdImportMustin.Size = New System.Drawing.Size(293, 22)
        Me.cmdImportMustin.Text = "Import MUTSIN Format"
        '
        'cmdConvertMustin
        '
        Me.cmdConvertMustin.Name = "cmdConvertMustin"
        Me.cmdConvertMustin.Size = New System.Drawing.Size(293, 22)
        Me.cmdConvertMustin.Text = "Convert All MUTSINs in Project"
        '
        'cmdAdvancedGen
        '
        Me.cmdAdvancedGen.Name = "cmdAdvancedGen"
        Me.cmdAdvancedGen.Size = New System.Drawing.Size(293, 22)
        Me.cmdAdvancedGen.Text = "Advanced Generation"
        '
        'cmdScenario
        '
        Me.cmdScenario.Image = CType(resources.GetObject("cmdScenario.Image"), System.Drawing.Image)
        Me.cmdScenario.Name = "cmdScenario"
        Me.cmdScenario.Size = New System.Drawing.Size(138, 22)
        Me.cmdScenario.Text = "Create Scenario"
        '
        'lstPoints
        '
        Me.lstPoints.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstPoints.FormattingEnabled = True
        Me.lstPoints.Location = New System.Drawing.Point(24, 96)
        Me.lstPoints.Margin = New System.Windows.Forms.Padding(4)
        Me.lstPoints.Name = "lstPoints"
        Me.lstPoints.Size = New System.Drawing.Size(280, 395)
        Me.lstPoints.TabIndex = 17
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "back-16x16.png")
        Me.ImageList1.Images.SetKeyName(1, "next-16x16.png")
        Me.ImageList1.Images.SetKeyName(2, "add.png")
        Me.ImageList1.Images.SetKeyName(3, "create.png")
        Me.ImageList1.Images.SetKeyName(4, "open.png")
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(183, 635)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(135, 32)
        Me.cmdCancel.TabIndex = 18
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(37, 635)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(135, 32)
        Me.cmdOK.TabIndex = 17
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'grpDetails
        '
        Me.grpDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDetails.Controls.Add(Me.agdPoint)
        Me.grpDetails.Controls.Add(Me.cmdDelete)
        Me.grpDetails.Controls.Add(Me.cmdList)
        Me.grpDetails.Controls.Add(Me.cmdGraph)
        Me.grpDetails.Location = New System.Drawing.Point(377, 15)
        Me.grpDetails.Margin = New System.Windows.Forms.Padding(4)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Padding = New System.Windows.Forms.Padding(4)
        Me.grpDetails.Size = New System.Drawing.Size(1037, 519)
        Me.grpDetails.TabIndex = 19
        Me.grpDetails.TabStop = False
        Me.grpDetails.Text = "Details of <    >"
        '
        'agdPoint
        '
        Me.agdPoint.AllowHorizontalScrolling = True
        Me.agdPoint.AllowNewValidValues = False
        Me.agdPoint.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdPoint.CellBackColor = System.Drawing.Color.Empty
        Me.agdPoint.Fixed3D = False
        Me.agdPoint.LineColor = System.Drawing.Color.Empty
        Me.agdPoint.LineWidth = 0.0!
        Me.agdPoint.Location = New System.Drawing.Point(76, 20)
        Me.agdPoint.Margin = New System.Windows.Forms.Padding(4)
        Me.agdPoint.Name = "agdPoint"
        Me.agdPoint.Size = New System.Drawing.Size(948, 479)
        Me.agdPoint.Source = Nothing
        Me.agdPoint.TabIndex = 25
        '
        'cmdDelete
        '
        Me.cmdDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdDelete.Image = CType(resources.GetObject("cmdDelete.Image"), System.Drawing.Image)
        Me.cmdDelete.Location = New System.Drawing.Point(8, 138)
        Me.cmdDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(53, 43)
        Me.cmdDelete.TabIndex = 24
        Me.cmdDelete.UseVisualStyleBackColor = True
        '
        'cmdList
        '
        Me.cmdList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdList.Image = CType(resources.GetObject("cmdList.Image"), System.Drawing.Image)
        Me.cmdList.Location = New System.Drawing.Point(8, 87)
        Me.cmdList.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdList.Name = "cmdList"
        Me.cmdList.Size = New System.Drawing.Size(53, 43)
        Me.cmdList.TabIndex = 23
        Me.cmdList.UseVisualStyleBackColor = True
        '
        'cmdGraph
        '
        Me.cmdGraph.BackgroundImage = CType(resources.GetObject("cmdGraph.BackgroundImage"), System.Drawing.Image)
        Me.cmdGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdGraph.Location = New System.Drawing.Point(8, 37)
        Me.cmdGraph.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdGraph.Name = "cmdGraph"
        Me.cmdGraph.Size = New System.Drawing.Size(53, 43)
        Me.cmdGraph.TabIndex = 22
        Me.cmdGraph.UseVisualStyleBackColor = True
        '
        'cmdDetailsShow
        '
        Me.cmdDetailsShow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDetailsShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.cmdDetailsShow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdDetailsShow.ImageIndex = 1
        Me.cmdDetailsShow.ImageList = Me.ImageList1
        Me.cmdDetailsShow.Location = New System.Drawing.Point(209, 574)
        Me.cmdDetailsShow.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdDetailsShow.Name = "cmdDetailsShow"
        Me.cmdDetailsShow.Size = New System.Drawing.Size(133, 26)
        Me.cmdDetailsShow.TabIndex = 17
        Me.cmdDetailsShow.Text = "Show Details"
        Me.cmdDetailsShow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDetailsShow.UseVisualStyleBackColor = True
        '
        'cmdDetailsHide
        '
        Me.cmdDetailsHide.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDetailsHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.cmdDetailsHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDetailsHide.ImageIndex = 0
        Me.cmdDetailsHide.ImageList = Me.ImageList1
        Me.cmdDetailsHide.Location = New System.Drawing.Point(377, 574)
        Me.cmdDetailsHide.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdDetailsHide.Name = "cmdDetailsHide"
        Me.cmdDetailsHide.Size = New System.Drawing.Size(133, 26)
        Me.cmdDetailsHide.TabIndex = 20
        Me.cmdDetailsHide.Text = "Hide Details"
        Me.cmdDetailsHide.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdDetailsHide.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'agdMasterPoint
        '
        Me.agdMasterPoint.AllowHorizontalScrolling = True
        Me.agdMasterPoint.AllowNewValidValues = False
        Me.agdMasterPoint.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdMasterPoint.CellBackColor = System.Drawing.Color.Empty
        Me.agdMasterPoint.Fixed3D = False
        Me.agdMasterPoint.LineColor = System.Drawing.Color.Empty
        Me.agdMasterPoint.LineWidth = 0.0!
        Me.agdMasterPoint.Location = New System.Drawing.Point(519, 560)
        Me.agdMasterPoint.Margin = New System.Windows.Forms.Padding(4)
        Me.agdMasterPoint.Name = "agdMasterPoint"
        Me.agdMasterPoint.Size = New System.Drawing.Size(896, 107)
        Me.agdMasterPoint.Source = Nothing
        Me.agdMasterPoint.TabIndex = 21
        '
        'frmPoint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1456, 693)
        Me.Controls.Add(Me.cmdDetailsHide)
        Me.Controls.Add(Me.cmdDetailsShow)
        Me.Controls.Add(Me.agdMasterPoint)
        Me.Controls.Add(Me.grpDetails)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.grpSources)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmPoint"
        Me.Text = "WinHSPF - Point Sources"
        Me.grpSources.ResumeLayout(False)
        Me.grpSources.PerformLayout()
        Me.menuPointSources.ResumeLayout(False)
        Me.menuPointSources.PerformLayout()
        Me.grpDetails.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkAllSources As System.Windows.Forms.CheckBox
    Friend WithEvents grpSources As System.Windows.Forms.GroupBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents cmdGraph As System.Windows.Forms.Button
    Friend WithEvents cmdDelete As System.Windows.Forms.Button
    Friend WithEvents cmdList As System.Windows.Forms.Button
    Friend WithEvents agdPoint As atcControls.atcGrid
    Friend WithEvents cmdDetailsHide As System.Windows.Forms.Button
    Friend WithEvents cmdDetailsShow As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents lstPoints As System.Windows.Forms.CheckedListBox
    Friend WithEvents menuPointSources As System.Windows.Forms.MenuStrip
    Friend WithEvents MenuAddPointSources As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdSimpleCreate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdImportMustin As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdConvertMustin As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdAdvancedGen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdScenario As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents agdMasterPoint As atcControls.atcGrid
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
