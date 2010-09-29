<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModelSegmentation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModelSegmentation))
        Me.fraSubbasins = New System.Windows.Forms.GroupBox
        Me.cmdViewMap = New System.Windows.Forms.Button
        Me.cmdEditTable = New System.Windows.Forms.Button
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.fraMetStations = New System.Windows.Forms.GroupBox
        Me.cmdAssign = New System.Windows.Forms.Button
        Me.cbxUseSelected = New System.Windows.Forms.CheckBox
        Me.cboMetStations = New System.Windows.Forms.ComboBox
        Me.cmdThiessen = New System.Windows.Forms.Button
        Me.fraSubbasins.SuspendLayout()
        Me.fraMetStations.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraSubbasins
        '
        Me.fraSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraSubbasins.Controls.Add(Me.cmdViewMap)
        Me.fraSubbasins.Controls.Add(Me.cmdEditTable)
        Me.fraSubbasins.Controls.Add(Me.cboSubbasins)
        Me.fraSubbasins.Location = New System.Drawing.Point(9, 10)
        Me.fraSubbasins.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.fraSubbasins.Name = "fraSubbasins"
        Me.fraSubbasins.Padding = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.fraSubbasins.Size = New System.Drawing.Size(301, 86)
        Me.fraSubbasins.TabIndex = 11
        Me.fraSubbasins.TabStop = False
        Me.fraSubbasins.Text = "Subbasins Layer"
        '
        'cmdViewMap
        '
        Me.cmdViewMap.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdViewMap.Location = New System.Drawing.Point(203, 51)
        Me.cmdViewMap.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdViewMap.Name = "cmdViewMap"
        Me.cmdViewMap.Size = New System.Drawing.Size(82, 23)
        Me.cmdViewMap.TabIndex = 13
        Me.cmdViewMap.Text = "View Map"
        Me.cmdViewMap.UseVisualStyleBackColor = True
        '
        'cmdEditTable
        '
        Me.cmdEditTable.Location = New System.Drawing.Point(14, 51)
        Me.cmdEditTable.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdEditTable.Name = "cmdEditTable"
        Me.cmdEditTable.Size = New System.Drawing.Size(82, 23)
        Me.cmdEditTable.TabIndex = 12
        Me.cmdEditTable.Text = "Edit Table"
        Me.cmdEditTable.UseVisualStyleBackColor = True
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Location = New System.Drawing.Point(14, 24)
        Me.cboSubbasins.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(273, 21)
        Me.cboSubbasins.TabIndex = 11
        '
        'fraMetStations
        '
        Me.fraMetStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraMetStations.Controls.Add(Me.cmdThiessen)
        Me.fraMetStations.Controls.Add(Me.cmdAssign)
        Me.fraMetStations.Controls.Add(Me.cbxUseSelected)
        Me.fraMetStations.Controls.Add(Me.cboMetStations)
        Me.fraMetStations.Location = New System.Drawing.Point(9, 101)
        Me.fraMetStations.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.fraMetStations.Name = "fraMetStations"
        Me.fraMetStations.Padding = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.fraMetStations.Size = New System.Drawing.Size(301, 145)
        Me.fraMetStations.TabIndex = 13
        Me.fraMetStations.TabStop = False
        Me.fraMetStations.Text = "Met Stations Layer"
        '
        'cmdAssign
        '
        Me.cmdAssign.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAssign.Location = New System.Drawing.Point(14, 71)
        Me.cmdAssign.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdAssign.Name = "cmdAssign"
        Me.cmdAssign.Size = New System.Drawing.Size(272, 28)
        Me.cmdAssign.TabIndex = 15
        Me.cmdAssign.Text = "Assign Met Stations To Subbasins By Proximity"
        Me.cmdAssign.UseVisualStyleBackColor = True
        '
        'cbxUseSelected
        '
        Me.cbxUseSelected.AutoSize = True
        Me.cbxUseSelected.Location = New System.Drawing.Point(26, 49)
        Me.cbxUseSelected.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cbxUseSelected.Name = "cbxUseSelected"
        Me.cbxUseSelected.Size = New System.Drawing.Size(134, 17)
        Me.cbxUseSelected.TabIndex = 14
        Me.cbxUseSelected.Text = "Use Selected Features"
        Me.cbxUseSelected.UseVisualStyleBackColor = True
        '
        'cboMetStations
        '
        Me.cboMetStations.AllowDrop = True
        Me.cboMetStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMetStations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMetStations.Location = New System.Drawing.Point(14, 24)
        Me.cboMetStations.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cboMetStations.Name = "cboMetStations"
        Me.cboMetStations.Size = New System.Drawing.Size(273, 21)
        Me.cboMetStations.TabIndex = 11
        '
        'cmdThiessen
        '
        Me.cmdThiessen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdThiessen.Location = New System.Drawing.Point(13, 103)
        Me.cmdThiessen.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdThiessen.Name = "cmdThiessen"
        Me.cmdThiessen.Size = New System.Drawing.Size(272, 28)
        Me.cmdThiessen.TabIndex = 16
        Me.cmdThiessen.Text = "Compute Thiessen Polygons"
        Me.cmdThiessen.UseVisualStyleBackColor = True
        '
        'frmModelSegmentation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(323, 273)
        Me.Controls.Add(Me.fraMetStations)
        Me.Controls.Add(Me.fraSubbasins)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "frmModelSegmentation"
        Me.Text = "BASINS Model Segmentation Specifier"
        Me.fraSubbasins.ResumeLayout(False)
        Me.fraMetStations.ResumeLayout(False)
        Me.fraMetStations.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fraSubbasins As System.Windows.Forms.GroupBox
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
    Friend WithEvents fraMetStations As System.Windows.Forms.GroupBox
    Friend WithEvents cbxUseSelected As System.Windows.Forms.CheckBox
    Friend WithEvents cboMetStations As System.Windows.Forms.ComboBox
    Friend WithEvents cmdEditTable As System.Windows.Forms.Button
    Friend WithEvents cmdViewMap As System.Windows.Forms.Button
    Friend WithEvents cmdAssign As System.Windows.Forms.Button
    Friend WithEvents cmdThiessen As System.Windows.Forms.Button
End Class
