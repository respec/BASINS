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
        Me.panelAxis = New System.Windows.Forms.Panel
        Me.lblAxisColor = New System.Windows.Forms.Label
        Me.panelAxisColor = New System.Windows.Forms.Panel
        Me.buttonAxisApply = New System.Windows.Forms.Button
        Me.checkAxisVisible = New System.Windows.Forms.CheckBox
        Me.checkApply = New System.Windows.Forms.CheckBox
        Me.panelCurve = New System.Windows.Forms.Panel
        Me.buttonCurveApply = New System.Windows.Forms.Button
        Me.lblCurveColor = New System.Windows.Forms.Label
        Me.panelCurveColor = New System.Windows.Forms.Panel
        Me.groupCurveStyle = New System.Windows.Forms.GroupBox
        Me.radioCurveLineNoStep = New System.Windows.Forms.RadioButton
        Me.radioCurveLineForward = New System.Windows.Forms.RadioButton
        Me.radioCurveBar = New System.Windows.Forms.RadioButton
        Me.radioCurveLineStep = New System.Windows.Forms.RadioButton
        Me.groupYaxis = New System.Windows.Forms.GroupBox
        Me.radioCurveYRight = New System.Windows.Forms.RadioButton
        Me.radioCurveYLeft = New System.Windows.Forms.RadioButton
        Me.checkCurveVisible = New System.Windows.Forms.CheckBox
        Me.splitHorizontally.Panel1.SuspendLayout()
        Me.splitHorizontally.Panel2.SuspendLayout()
        Me.splitHorizontally.SuspendLayout()
        Me.panelAxis.SuspendLayout()
        Me.panelCurve.SuspendLayout()
        Me.groupCurveStyle.SuspendLayout()
        Me.groupYaxis.SuspendLayout()
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
        Me.splitHorizontally.Panel2.Controls.Add(Me.checkApply)
        Me.splitHorizontally.Panel2.Controls.Add(Me.panelAxis)
        Me.splitHorizontally.Panel2.Controls.Add(Me.panelCurve)
        Me.splitHorizontally.Size = New System.Drawing.Size(557, 482)
        Me.splitHorizontally.SplitterDistance = 185
        Me.splitHorizontally.TabIndex = 0
        '
        'treeCategories
        '
        Me.treeCategories.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treeCategories.Location = New System.Drawing.Point(0, 0)
        Me.treeCategories.Name = "treeCategories"
        Me.treeCategories.Size = New System.Drawing.Size(185, 482)
        Me.treeCategories.TabIndex = 1
        '
        'panelAxis
        '
        Me.panelAxis.Controls.Add(Me.lblAxisColor)
        Me.panelAxis.Controls.Add(Me.panelAxisColor)
        Me.panelAxis.Controls.Add(Me.buttonAxisApply)
        Me.panelAxis.Controls.Add(Me.checkAxisVisible)
        Me.panelAxis.Location = New System.Drawing.Point(178, 3)
        Me.panelAxis.Name = "panelAxis"
        Me.panelAxis.Size = New System.Drawing.Size(341, 301)
        Me.panelAxis.TabIndex = 1
        Me.panelAxis.Visible = False
        '
        'lblAxisColor
        '
        Me.lblAxisColor.AutoSize = True
        Me.lblAxisColor.Location = New System.Drawing.Point(3, 223)
        Me.lblAxisColor.Name = "lblAxisColor"
        Me.lblAxisColor.Size = New System.Drawing.Size(31, 13)
        Me.lblAxisColor.TabIndex = 9
        Me.lblAxisColor.Text = "Color"
        '
        'panelAxisColor
        '
        Me.panelAxisColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panelAxisColor.Location = New System.Drawing.Point(54, 223)
        Me.panelAxisColor.Name = "panelAxisColor"
        Me.panelAxisColor.Size = New System.Drawing.Size(91, 13)
        Me.panelAxisColor.TabIndex = 8
        '
        'buttonAxisApply
        '
        Me.buttonAxisApply.Location = New System.Drawing.Point(6, 266)
        Me.buttonAxisApply.Name = "buttonAxisApply"
        Me.buttonAxisApply.Size = New System.Drawing.Size(78, 26)
        Me.buttonAxisApply.TabIndex = 5
        Me.buttonAxisApply.Text = "Apply"
        Me.buttonAxisApply.UseVisualStyleBackColor = True
        '
        'checkAxisVisible
        '
        Me.checkAxisVisible.AutoSize = True
        Me.checkAxisVisible.Location = New System.Drawing.Point(54, 26)
        Me.checkAxisVisible.Name = "checkAxisVisible"
        Me.checkAxisVisible.Size = New System.Drawing.Size(56, 17)
        Me.checkAxisVisible.TabIndex = 2
        Me.checkAxisVisible.Text = "Visible"
        Me.checkAxisVisible.UseVisualStyleBackColor = True
        '
        'checkApply
        '
        Me.checkApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.checkApply.AutoSize = True
        Me.checkApply.Location = New System.Drawing.Point(179, 462)
        Me.checkApply.Name = "checkApply"
        Me.checkApply.Size = New System.Drawing.Size(177, 17)
        Me.checkApply.TabIndex = 10
        Me.checkApply.Text = "View changes as they are made"
        Me.checkApply.UseVisualStyleBackColor = True
        '
        'panelCurve
        '
        Me.panelCurve.Controls.Add(Me.buttonCurveApply)
        Me.panelCurve.Controls.Add(Me.lblCurveColor)
        Me.panelCurve.Controls.Add(Me.panelCurveColor)
        Me.panelCurve.Controls.Add(Me.groupCurveStyle)
        Me.panelCurve.Controls.Add(Me.groupYaxis)
        Me.panelCurve.Controls.Add(Me.checkCurveVisible)
        Me.panelCurve.Location = New System.Drawing.Point(3, 12)
        Me.panelCurve.Name = "panelCurve"
        Me.panelCurve.Size = New System.Drawing.Size(341, 295)
        Me.panelCurve.TabIndex = 0
        Me.panelCurve.Visible = False
        '
        'buttonCurveApply
        '
        Me.buttonCurveApply.Location = New System.Drawing.Point(6, 259)
        Me.buttonCurveApply.Name = "buttonCurveApply"
        Me.buttonCurveApply.Size = New System.Drawing.Size(78, 26)
        Me.buttonCurveApply.TabIndex = 10
        Me.buttonCurveApply.Text = "Apply"
        Me.buttonCurveApply.UseVisualStyleBackColor = True
        '
        'lblCurveColor
        '
        Me.lblCurveColor.AutoSize = True
        Me.lblCurveColor.Location = New System.Drawing.Point(3, 223)
        Me.lblCurveColor.Name = "lblCurveColor"
        Me.lblCurveColor.Size = New System.Drawing.Size(31, 13)
        Me.lblCurveColor.TabIndex = 9
        Me.lblCurveColor.Text = "Color"
        '
        'panelCurveColor
        '
        Me.panelCurveColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panelCurveColor.Location = New System.Drawing.Point(54, 223)
        Me.panelCurveColor.Name = "panelCurveColor"
        Me.panelCurveColor.Size = New System.Drawing.Size(91, 13)
        Me.panelCurveColor.TabIndex = 8
        '
        'groupCurveStyle
        '
        Me.groupCurveStyle.Controls.Add(Me.radioCurveLineNoStep)
        Me.groupCurveStyle.Controls.Add(Me.radioCurveLineForward)
        Me.groupCurveStyle.Controls.Add(Me.radioCurveBar)
        Me.groupCurveStyle.Controls.Add(Me.radioCurveLineStep)
        Me.groupCurveStyle.Location = New System.Drawing.Point(6, 129)
        Me.groupCurveStyle.Name = "groupCurveStyle"
        Me.groupCurveStyle.Size = New System.Drawing.Size(281, 70)
        Me.groupCurveStyle.TabIndex = 7
        Me.groupCurveStyle.TabStop = False
        Me.groupCurveStyle.Text = "Style"
        '
        'radioCurveLineNoStep
        '
        Me.radioCurveLineNoStep.AutoSize = True
        Me.radioCurveLineNoStep.Location = New System.Drawing.Point(91, 19)
        Me.radioCurveLineNoStep.Name = "radioCurveLineNoStep"
        Me.radioCurveLineNoStep.Size = New System.Drawing.Size(87, 17)
        Me.radioCurveLineNoStep.TabIndex = 9
        Me.radioCurveLineNoStep.TabStop = True
        Me.radioCurveLineNoStep.Text = "Line No Step"
        Me.radioCurveLineNoStep.UseVisualStyleBackColor = True
        '
        'radioCurveLineForward
        '
        Me.radioCurveLineForward.AutoSize = True
        Me.radioCurveLineForward.Location = New System.Drawing.Point(184, 19)
        Me.radioCurveLineForward.Name = "radioCurveLineForward"
        Me.radioCurveLineForward.Size = New System.Drawing.Size(86, 17)
        Me.radioCurveLineForward.TabIndex = 8
        Me.radioCurveLineForward.TabStop = True
        Me.radioCurveLineForward.Text = "Line Forward"
        Me.radioCurveLineForward.UseVisualStyleBackColor = True
        '
        'radioCurveBar
        '
        Me.radioCurveBar.AutoSize = True
        Me.radioCurveBar.Location = New System.Drawing.Point(15, 42)
        Me.radioCurveBar.Name = "radioCurveBar"
        Me.radioCurveBar.Size = New System.Drawing.Size(41, 17)
        Me.radioCurveBar.TabIndex = 7
        Me.radioCurveBar.TabStop = True
        Me.radioCurveBar.Text = "Bar"
        Me.radioCurveBar.UseVisualStyleBackColor = True
        '
        'radioCurveLineStep
        '
        Me.radioCurveLineStep.AutoSize = True
        Me.radioCurveLineStep.Location = New System.Drawing.Point(15, 19)
        Me.radioCurveLineStep.Name = "radioCurveLineStep"
        Me.radioCurveLineStep.Size = New System.Drawing.Size(70, 17)
        Me.radioCurveLineStep.TabIndex = 6
        Me.radioCurveLineStep.TabStop = True
        Me.radioCurveLineStep.Text = "Line Step"
        Me.radioCurveLineStep.UseVisualStyleBackColor = True
        '
        'groupYaxis
        '
        Me.groupYaxis.Controls.Add(Me.radioCurveYRight)
        Me.groupYaxis.Controls.Add(Me.radioCurveYLeft)
        Me.groupYaxis.Location = New System.Drawing.Point(3, 49)
        Me.groupYaxis.Name = "groupYaxis"
        Me.groupYaxis.Size = New System.Drawing.Size(142, 74)
        Me.groupYaxis.TabIndex = 6
        Me.groupYaxis.TabStop = False
        Me.groupYaxis.Text = "Y Axis"
        '
        'radioCurveYRight
        '
        Me.radioCurveYRight.AutoSize = True
        Me.radioCurveYRight.Location = New System.Drawing.Point(51, 42)
        Me.radioCurveYRight.Name = "radioCurveYRight"
        Me.radioCurveYRight.Size = New System.Drawing.Size(50, 17)
        Me.radioCurveYRight.TabIndex = 6
        Me.radioCurveYRight.TabStop = True
        Me.radioCurveYRight.Text = "Right"
        Me.radioCurveYRight.UseVisualStyleBackColor = True
        '
        'radioCurveYLeft
        '
        Me.radioCurveYLeft.AutoSize = True
        Me.radioCurveYLeft.Location = New System.Drawing.Point(51, 19)
        Me.radioCurveYLeft.Name = "radioCurveYLeft"
        Me.radioCurveYLeft.Size = New System.Drawing.Size(43, 17)
        Me.radioCurveYLeft.TabIndex = 5
        Me.radioCurveYLeft.TabStop = True
        Me.radioCurveYLeft.Text = "Left"
        Me.radioCurveYLeft.UseVisualStyleBackColor = True
        '
        'checkCurveVisible
        '
        Me.checkCurveVisible.AutoSize = True
        Me.checkCurveVisible.Location = New System.Drawing.Point(54, 26)
        Me.checkCurveVisible.Name = "checkCurveVisible"
        Me.checkCurveVisible.Size = New System.Drawing.Size(56, 17)
        Me.checkCurveVisible.TabIndex = 2
        Me.checkCurveVisible.Text = "Visible"
        Me.checkCurveVisible.UseVisualStyleBackColor = True
        '
        'frmGraphEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(557, 482)
        Me.Controls.Add(Me.splitHorizontally)
        Me.Name = "frmGraphEdit"
        Me.Text = "frmGraphEdit"
        Me.splitHorizontally.Panel1.ResumeLayout(False)
        Me.splitHorizontally.Panel2.ResumeLayout(False)
        Me.splitHorizontally.Panel2.PerformLayout()
        Me.splitHorizontally.ResumeLayout(False)
        Me.panelAxis.ResumeLayout(False)
        Me.panelAxis.PerformLayout()
        Me.panelCurve.ResumeLayout(False)
        Me.panelCurve.PerformLayout()
        Me.groupCurveStyle.ResumeLayout(False)
        Me.groupCurveStyle.PerformLayout()
        Me.groupYaxis.ResumeLayout(False)
        Me.groupYaxis.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents splitHorizontally As System.Windows.Forms.SplitContainer
    Friend WithEvents treeCategories As System.Windows.Forms.TreeView
    Friend WithEvents panelCurve As System.Windows.Forms.Panel
    Friend WithEvents checkCurveVisible As System.Windows.Forms.CheckBox
    Friend WithEvents groupCurveStyle As System.Windows.Forms.GroupBox
    Friend WithEvents radioCurveLineStep As System.Windows.Forms.RadioButton
    Friend WithEvents groupYaxis As System.Windows.Forms.GroupBox
    Friend WithEvents radioCurveYRight As System.Windows.Forms.RadioButton
    Friend WithEvents radioCurveYLeft As System.Windows.Forms.RadioButton
    Friend WithEvents lblCurveColor As System.Windows.Forms.Label
    Friend WithEvents panelCurveColor As System.Windows.Forms.Panel
    Friend WithEvents radioCurveBar As System.Windows.Forms.RadioButton
    Friend WithEvents checkApply As System.Windows.Forms.CheckBox
    Friend WithEvents radioCurveLineNoStep As System.Windows.Forms.RadioButton
    Friend WithEvents radioCurveLineForward As System.Windows.Forms.RadioButton
    Friend WithEvents panelAxis As System.Windows.Forms.Panel
    Friend WithEvents lblAxisColor As System.Windows.Forms.Label
    Friend WithEvents panelAxisColor As System.Windows.Forms.Panel
    Friend WithEvents buttonAxisApply As System.Windows.Forms.Button
    Friend WithEvents checkAxisVisible As System.Windows.Forms.CheckBox
    Friend WithEvents buttonCurveApply As System.Windows.Forms.Button
End Class
