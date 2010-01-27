<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlSchematic
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.SplitHorizontal = New System.Windows.Forms.SplitContainer
        Me.SplitLegendTree = New System.Windows.Forms.SplitContainer
        Me.tabLeft = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.LegendLandSurface = New ctlLegend
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.LegendMetSegs = New ctlLegend
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.LegendPointSources = New ctlLegend
        Me.agdDetails = New atcControls.atcGrid
        Me.RightClickMenu = New System.Windows.Forms.ContextMenu
        Me.SplitHorizontal.Panel1.SuspendLayout()
        Me.SplitHorizontal.Panel2.SuspendLayout()
        Me.SplitHorizontal.SuspendLayout()
        Me.SplitLegendTree.Panel1.SuspendLayout()
        Me.SplitLegendTree.SuspendLayout()
        Me.tabLeft.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitHorizontal
        '
        Me.SplitHorizontal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitHorizontal.Location = New System.Drawing.Point(0, 0)
        Me.SplitHorizontal.Name = "SplitHorizontal"
        Me.SplitHorizontal.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitHorizontal.Panel1
        '
        Me.SplitHorizontal.Panel1.Controls.Add(Me.SplitLegendTree)
        '
        'SplitHorizontal.Panel2
        '
        Me.SplitHorizontal.Panel2.Controls.Add(Me.agdDetails)
        Me.SplitHorizontal.Size = New System.Drawing.Size(714, 449)
        Me.SplitHorizontal.SplitterDistance = 320
        Me.SplitHorizontal.TabIndex = 6
        '
        'SplitLegendTree
        '
        Me.SplitLegendTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitLegendTree.Location = New System.Drawing.Point(0, 0)
        Me.SplitLegendTree.Name = "SplitLegendTree"
        '
        'SplitLegendTree.Panel1
        '
        Me.SplitLegendTree.Panel1.Controls.Add(Me.tabLeft)
        Me.SplitLegendTree.Size = New System.Drawing.Size(714, 320)
        Me.SplitLegendTree.SplitterDistance = 208
        Me.SplitLegendTree.TabIndex = 0
        '
        'tabLeft
        '
        Me.tabLeft.Alignment = System.Windows.Forms.TabAlignment.Left
        Me.tabLeft.Controls.Add(Me.TabPage1)
        Me.tabLeft.Controls.Add(Me.TabPage2)
        Me.tabLeft.Controls.Add(Me.TabPage3)
        Me.tabLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabLeft.Location = New System.Drawing.Point(0, 0)
        Me.tabLeft.Multiline = True
        Me.tabLeft.Name = "tabLeft"
        Me.tabLeft.SelectedIndex = 0
        Me.tabLeft.Size = New System.Drawing.Size(208, 320)
        Me.tabLeft.TabIndex = 8
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.LegendLandSurface)
        Me.TabPage1.Location = New System.Drawing.Point(23, 4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(181, 312)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Land Surface"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'LegendLandSurface
        '
        Me.LegendLandSurface.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LegendLandSurface.Location = New System.Drawing.Point(3, 3)
        Me.LegendLandSurface.Margin = New System.Windows.Forms.Padding(0)
        Me.LegendLandSurface.Name = "LegendLandSurface"
        Me.LegendLandSurface.Size = New System.Drawing.Size(175, 306)
        Me.LegendLandSurface.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.LegendMetSegs)
        Me.TabPage2.Location = New System.Drawing.Point(23, 4)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(181, 312)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Met Segs"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'LegendMetSegs
        '
        Me.LegendMetSegs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LegendMetSegs.Location = New System.Drawing.Point(3, 3)
        Me.LegendMetSegs.Margin = New System.Windows.Forms.Padding(0)
        Me.LegendMetSegs.Name = "LegendMetSegs"
        Me.LegendMetSegs.Size = New System.Drawing.Size(175, 306)
        Me.LegendMetSegs.TabIndex = 1
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.LegendPointSources)
        Me.TabPage3.Location = New System.Drawing.Point(23, 4)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(181, 312)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Point Sources"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'LegendPointSources
        '
        Me.LegendPointSources.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LegendPointSources.Location = New System.Drawing.Point(3, 3)
        Me.LegendPointSources.Margin = New System.Windows.Forms.Padding(0)
        Me.LegendPointSources.Name = "LegendPointSources"
        Me.LegendPointSources.Size = New System.Drawing.Size(175, 306)
        Me.LegendPointSources.TabIndex = 1
        '
        'agdDetails
        '
        Me.agdDetails.AllowHorizontalScrolling = True
        Me.agdDetails.AllowNewValidValues = False
        Me.agdDetails.BackColor = System.Drawing.SystemColors.Control
        Me.agdDetails.CellBackColor = System.Drawing.Color.Empty
        Me.agdDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.agdDetails.Fixed3D = False
        Me.agdDetails.LineColor = System.Drawing.Color.Empty
        Me.agdDetails.LineWidth = 0.0!
        Me.agdDetails.Location = New System.Drawing.Point(0, 0)
        Me.agdDetails.Name = "agdDetails"
        Me.agdDetails.Size = New System.Drawing.Size(714, 125)
        Me.agdDetails.Source = Nothing
        Me.agdDetails.TabIndex = 6
        '
        'ctlSchematic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitHorizontal)
        Me.Name = "ctlSchematic"
        Me.Size = New System.Drawing.Size(714, 449)
        Me.SplitHorizontal.Panel1.ResumeLayout(False)
        Me.SplitHorizontal.Panel2.ResumeLayout(False)
        Me.SplitHorizontal.ResumeLayout(False)
        Me.SplitLegendTree.Panel1.ResumeLayout(False)
        Me.SplitLegendTree.ResumeLayout(False)
        Me.tabLeft.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitHorizontal As System.Windows.Forms.SplitContainer
    Friend WithEvents agdDetails As atcControls.atcGrid
    Friend WithEvents SplitLegendTree As System.Windows.Forms.SplitContainer
    Friend WithEvents tabLeft As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents LegendLandSurface As ctlLegend
    Friend WithEvents LegendMetSegs As ctlLegend
    Friend WithEvents LegendPointSources As ctlLegend
    Friend WithEvents RightClickMenu As System.Windows.Forms.ContextMenu

End Class
