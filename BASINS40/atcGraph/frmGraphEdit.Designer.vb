<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGraphEdit
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
        Me.splitHorizontally = New System.Windows.Forms.SplitContainer
        Me.treeCategories = New System.Windows.Forms.TreeView
        Me.panelCurve = New System.Windows.Forms.Panel
        Me.buttonCurveApply = New System.Windows.Forms.Button
        Me.radioCurveYRight = New System.Windows.Forms.RadioButton
        Me.radioCurveYLeft = New System.Windows.Forms.RadioButton
        Me.chkCurveVisible = New System.Windows.Forms.CheckBox
        Me.lblCurveColor = New System.Windows.Forms.Label
        Me.panelCurveColor = New System.Windows.Forms.Panel
        Me.splitHorizontally.Panel1.SuspendLayout()
        Me.splitHorizontally.Panel2.SuspendLayout()
        Me.splitHorizontally.SuspendLayout()
        Me.panelCurve.SuspendLayout()
        Me.SuspendLayout()
        '
        'splitHorizontally
        '
        Me.splitHorizontally.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitHorizontally.Location = New System.Drawing.Point(0, 0)
        Me.splitHorizontally.Name = "splitHorizontally"
        '
        'splitHorizontally.Panel1
        '
        Me.splitHorizontally.Panel1.Controls.Add(Me.treeCategories)
        '
        'splitHorizontally.Panel2
        '
        Me.splitHorizontally.Panel2.Controls.Add(Me.panelCurve)
        Me.splitHorizontally.Size = New System.Drawing.Size(540, 498)
        Me.splitHorizontally.SplitterDistance = 180
        Me.splitHorizontally.TabIndex = 0
        '
        'treeCategories
        '
        Me.treeCategories.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treeCategories.Location = New System.Drawing.Point(0, 0)
        Me.treeCategories.Name = "treeCategories"
        Me.treeCategories.Size = New System.Drawing.Size(180, 498)
        Me.treeCategories.TabIndex = 1
        '
        'panelCurve
        '
        Me.panelCurve.Controls.Add(Me.buttonCurveApply)
        Me.panelCurve.Controls.Add(Me.radioCurveYRight)
        Me.panelCurve.Controls.Add(Me.radioCurveYLeft)
        Me.panelCurve.Controls.Add(Me.chkCurveVisible)
        Me.panelCurve.Controls.Add(Me.lblCurveColor)
        Me.panelCurve.Controls.Add(Me.panelCurveColor)
        Me.panelCurve.Location = New System.Drawing.Point(3, 12)
        Me.panelCurve.Name = "panelCurve"
        Me.panelCurve.Size = New System.Drawing.Size(341, 295)
        Me.panelCurve.TabIndex = 0
        '
        'buttonCurveApply
        '
        Me.buttonCurveApply.Location = New System.Drawing.Point(99, 241)
        Me.buttonCurveApply.Name = "buttonCurveApply"
        Me.buttonCurveApply.Size = New System.Drawing.Size(78, 26)
        Me.buttonCurveApply.TabIndex = 5
        Me.buttonCurveApply.Text = "Apply"
        Me.buttonCurveApply.UseVisualStyleBackColor = True
        '
        'radioCurveYRight
        '
        Me.radioCurveYRight.AutoSize = True
        Me.radioCurveYRight.Location = New System.Drawing.Point(76, 100)
        Me.radioCurveYRight.Name = "radioCurveYRight"
        Me.radioCurveYRight.Size = New System.Drawing.Size(82, 17)
        Me.radioCurveYRight.TabIndex = 4
        Me.radioCurveYRight.TabStop = True
        Me.radioCurveYRight.Text = "Right Y Axis"
        Me.radioCurveYRight.UseVisualStyleBackColor = True
        '
        'radioCurveYLeft
        '
        Me.radioCurveYLeft.AutoSize = True
        Me.radioCurveYLeft.Location = New System.Drawing.Point(76, 77)
        Me.radioCurveYLeft.Name = "radioCurveYLeft"
        Me.radioCurveYLeft.Size = New System.Drawing.Size(75, 17)
        Me.radioCurveYLeft.TabIndex = 3
        Me.radioCurveYLeft.TabStop = True
        Me.radioCurveYLeft.Text = "Left Y Axis"
        Me.radioCurveYLeft.UseVisualStyleBackColor = True
        '
        'chkCurveVisible
        '
        Me.chkCurveVisible.AutoSize = True
        Me.chkCurveVisible.Location = New System.Drawing.Point(77, 137)
        Me.chkCurveVisible.Name = "chkCurveVisible"
        Me.chkCurveVisible.Size = New System.Drawing.Size(56, 17)
        Me.chkCurveVisible.TabIndex = 2
        Me.chkCurveVisible.Text = "Visible"
        Me.chkCurveVisible.UseVisualStyleBackColor = True
        '
        'lblCurveColor
        '
        Me.lblCurveColor.AutoSize = True
        Me.lblCurveColor.Location = New System.Drawing.Point(79, 172)
        Me.lblCurveColor.Name = "lblCurveColor"
        Me.lblCurveColor.Size = New System.Drawing.Size(31, 13)
        Me.lblCurveColor.TabIndex = 1
        Me.lblCurveColor.Text = "Color"
        '
        'panelCurveColor
        '
        Me.panelCurveColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panelCurveColor.Location = New System.Drawing.Point(116, 172)
        Me.panelCurveColor.Name = "panelCurveColor"
        Me.panelCurveColor.Size = New System.Drawing.Size(117, 13)
        Me.panelCurveColor.TabIndex = 0
        '
        'frmGraphEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(540, 498)
        Me.Controls.Add(Me.splitHorizontally)
        Me.Name = "frmGraphEdit"
        Me.Text = "frmGraphEdit"
        Me.splitHorizontally.Panel1.ResumeLayout(False)
        Me.splitHorizontally.Panel2.ResumeLayout(False)
        Me.splitHorizontally.ResumeLayout(False)
        Me.panelCurve.ResumeLayout(False)
        Me.panelCurve.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents splitHorizontally As System.Windows.Forms.SplitContainer
    Friend WithEvents treeCategories As System.Windows.Forms.TreeView
    Friend WithEvents panelCurve As System.Windows.Forms.Panel
    Friend WithEvents panelCurveColor As System.Windows.Forms.Panel
    Friend WithEvents chkCurveVisible As System.Windows.Forms.CheckBox
    Friend WithEvents lblCurveColor As System.Windows.Forms.Label
    Friend WithEvents radioCurveYRight As System.Windows.Forms.RadioButton
    Friend WithEvents radioCurveYLeft As System.Windows.Forms.RadioButton
    Friend WithEvents buttonCurveApply As System.Windows.Forms.Button
End Class
