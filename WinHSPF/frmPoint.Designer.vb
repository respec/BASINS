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
        Me.Button7 = New System.Windows.Forms.Button
        Me.Button8 = New System.Windows.Forms.Button
        Me.cmdDetailsShow = New System.Windows.Forms.Button
        Me.cmdDetailsHide = New System.Windows.Forms.Button
        Me.agdMasterPoint = New atcControls.atcGrid
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cboPollutantList = New System.Windows.Forms.ComboBox
        Me.cmdFile = New System.Windows.Forms.Button
        Me.txtPollutantPath = New System.Windows.Forms.TextBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.grpSources.SuspendLayout()
        Me.menuPointSources.SuspendLayout()
        Me.grpDetails.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkAllSources
        '
        Me.chkAllSources.AutoSize = True
        Me.chkAllSources.Location = New System.Drawing.Point(16, 51)
        Me.chkAllSources.Name = "chkAllSources"
        Me.chkAllSources.Size = New System.Drawing.Size(117, 17)
        Me.chkAllSources.TabIndex = 7
        Me.chkAllSources.Text = "Select/Deselect All"
        Me.chkAllSources.UseVisualStyleBackColor = True
        '
        'grpSources
        '
        Me.grpSources.Controls.Add(Me.menuPointSources)
        Me.grpSources.Controls.Add(Me.lstPoints)
        Me.grpSources.Controls.Add(Me.chkAllSources)
        Me.grpSources.Location = New System.Drawing.Point(12, 12)
        Me.grpSources.Name = "grpSources"
        Me.grpSources.Size = New System.Drawing.Size(245, 324)
        Me.grpSources.TabIndex = 14
        Me.grpSources.TabStop = False
        Me.grpSources.Text = "Point Sources"
        '
        'menuPointSources
        '
        Me.menuPointSources.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuAddPointSources, Me.cmdScenario})
        Me.menuPointSources.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.menuPointSources.Location = New System.Drawing.Point(3, 16)
        Me.menuPointSources.Name = "menuPointSources"
        Me.menuPointSources.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.menuPointSources.Size = New System.Drawing.Size(239, 24)
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
        Me.MenuAddPointSources.Size = New System.Drawing.Size(117, 20)
        Me.MenuAddPointSources.Text = "Add Point Source"
        '
        'cmdSimpleCreate
        '
        Me.cmdSimpleCreate.Name = "cmdSimpleCreate"
        Me.cmdSimpleCreate.Size = New System.Drawing.Size(232, 22)
        Me.cmdSimpleCreate.Text = "Simple Create"
        '
        'cmdImportMustin
        '
        Me.cmdImportMustin.Name = "cmdImportMustin"
        Me.cmdImportMustin.Size = New System.Drawing.Size(232, 22)
        Me.cmdImportMustin.Text = "Import MUTSIN Format"
        '
        'cmdConvertMustin
        '
        Me.cmdConvertMustin.Name = "cmdConvertMustin"
        Me.cmdConvertMustin.Size = New System.Drawing.Size(232, 22)
        Me.cmdConvertMustin.Text = "Convert All MUTSINs in Project"
        '
        'cmdAdvancedGen
        '
        Me.cmdAdvancedGen.Name = "cmdAdvancedGen"
        Me.cmdAdvancedGen.Size = New System.Drawing.Size(232, 22)
        Me.cmdAdvancedGen.Text = "Advanced Generation"
        '
        'cmdScenario
        '
        Me.cmdScenario.Image = CType(resources.GetObject("cmdScenario.Image"), System.Drawing.Image)
        Me.cmdScenario.Name = "cmdScenario"
        Me.cmdScenario.Size = New System.Drawing.Size(112, 20)
        Me.cmdScenario.Text = "Create Scenario"
        '
        'lstPoints
        '
        Me.lstPoints.FormattingEnabled = True
        Me.lstPoints.Location = New System.Drawing.Point(18, 78)
        Me.lstPoints.Name = "lstPoints"
        Me.lstPoints.Size = New System.Drawing.Size(211, 229)
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
        Me.cmdCancel.Location = New System.Drawing.Point(137, 516)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(101, 26)
        Me.cmdCancel.TabIndex = 18
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(28, 516)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(101, 26)
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
        Me.grpDetails.Controls.Add(Me.Button7)
        Me.grpDetails.Controls.Add(Me.Button8)
        Me.grpDetails.Location = New System.Drawing.Point(283, 12)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(778, 422)
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
        Me.agdPoint.Location = New System.Drawing.Point(57, 16)
        Me.agdPoint.Name = "agdPoint"
        Me.agdPoint.Size = New System.Drawing.Size(711, 389)
        Me.agdPoint.Source = Nothing
        Me.agdPoint.TabIndex = 25
        '
        'cmdDelete
        '
        Me.cmdDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdDelete.Image = CType(resources.GetObject("cmdDelete.Image"), System.Drawing.Image)
        Me.cmdDelete.Location = New System.Drawing.Point(6, 112)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(40, 35)
        Me.cmdDelete.TabIndex = 24
        Me.cmdDelete.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Button7.Image = CType(resources.GetObject("Button7.Image"), System.Drawing.Image)
        Me.Button7.Location = New System.Drawing.Point(6, 71)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(40, 35)
        Me.Button7.TabIndex = 23
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.BackgroundImage = CType(resources.GetObject("Button8.BackgroundImage"), System.Drawing.Image)
        Me.Button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Button8.Location = New System.Drawing.Point(6, 30)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(40, 35)
        Me.Button8.TabIndex = 22
        Me.Button8.UseVisualStyleBackColor = True
        '
        'cmdDetailsShow
        '
        Me.cmdDetailsShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.cmdDetailsShow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdDetailsShow.ImageIndex = 1
        Me.cmdDetailsShow.ImageList = Me.ImageList1
        Me.cmdDetailsShow.Location = New System.Drawing.Point(157, 466)
        Me.cmdDetailsShow.Name = "cmdDetailsShow"
        Me.cmdDetailsShow.Size = New System.Drawing.Size(100, 21)
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
        Me.cmdDetailsHide.Location = New System.Drawing.Point(283, 466)
        Me.cmdDetailsHide.Name = "cmdDetailsHide"
        Me.cmdDetailsHide.Size = New System.Drawing.Size(100, 21)
        Me.cmdDetailsHide.TabIndex = 20
        Me.cmdDetailsHide.Text = "Hide Details"
        Me.cmdDetailsHide.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdDetailsHide.UseVisualStyleBackColor = True
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
        Me.agdMasterPoint.Location = New System.Drawing.Point(389, 455)
        Me.agdMasterPoint.Name = "agdMasterPoint"
        Me.agdMasterPoint.Size = New System.Drawing.Size(672, 87)
        Me.agdMasterPoint.Source = Nothing
        Me.agdMasterPoint.TabIndex = 21
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cboPollutantList)
        Me.GroupBox1.Controls.Add(Me.cmdFile)
        Me.GroupBox1.Controls.Add(Me.txtPollutantPath)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 345)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(245, 89)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Pollutant List"
        '
        'cboPollutantList
        '
        Me.cboPollutantList.BackColor = System.Drawing.SystemColors.Control
        Me.cboPollutantList.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.cboPollutantList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPollutantList.FormattingEnabled = True
        Me.cboPollutantList.Location = New System.Drawing.Point(13, 55)
        Me.cboPollutantList.MaxDropDownItems = 20
        Me.cboPollutantList.Name = "cboPollutantList"
        Me.cboPollutantList.Size = New System.Drawing.Size(219, 21)
        Me.cboPollutantList.TabIndex = 25
        '
        'cmdFile
        '
        Me.cmdFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFile.ImageIndex = 4
        Me.cmdFile.ImageList = Me.ImageList1
        Me.cmdFile.Location = New System.Drawing.Point(13, 24)
        Me.cmdFile.Name = "cmdFile"
        Me.cmdFile.Size = New System.Drawing.Size(58, 22)
        Me.cmdFile.TabIndex = 24
        Me.cmdFile.Text = "Open"
        Me.cmdFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdFile.UseVisualStyleBackColor = True
        '
        'txtPollutantPath
        '
        Me.txtPollutantPath.Location = New System.Drawing.Point(73, 25)
        Me.txtPollutantPath.Name = "txtPollutantPath"
        Me.txtPollutantPath.ReadOnly = True
        Me.txtPollutantPath.Size = New System.Drawing.Size(159, 20)
        Me.txtPollutantPath.TabIndex = 23
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmPoint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1092, 563)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdDetailsHide)
        Me.Controls.Add(Me.cmdDetailsShow)
        Me.Controls.Add(Me.agdMasterPoint)
        Me.Controls.Add(Me.grpDetails)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.grpSources)
        Me.Name = "frmPoint"
        Me.Text = "WinHSPF - Point Sources"
        Me.grpSources.ResumeLayout(False)
        Me.grpSources.PerformLayout()
        Me.menuPointSources.ResumeLayout(False)
        Me.menuPointSources.PerformLayout()
        Me.grpDetails.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkAllSources As System.Windows.Forms.CheckBox
    Friend WithEvents grpSources As System.Windows.Forms.GroupBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents cmdDelete As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
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
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtPollutantPath As System.Windows.Forms.TextBox
    Friend WithEvents cmdFile As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cboPollutantList As System.Windows.Forms.ComboBox
End Class
