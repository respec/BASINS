<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPlot
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPlot))
        Me.cboXAxis = New System.Windows.Forms.ComboBox
        Me.cboYAxis = New System.Windows.Forms.ComboBox
        Me.cboZAxis = New System.Windows.Forms.ComboBox
        Me.lblXAxis = New System.Windows.Forms.Label
        Me.lblYAxis = New System.Windows.Forms.Label
        Me.lblZAxis = New System.Windows.Forms.Label
        Me.btnPlot = New System.Windows.Forms.Button
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblPointLabels = New System.Windows.Forms.Label
        Me.cboPointLabels = New System.Windows.Forms.ComboBox
        Me.grpSelect = New System.Windows.Forms.GroupBox
        Me.btnNoneModifications = New System.Windows.Forms.Button
        Me.btnAllModifications = New System.Windows.Forms.Button
        Me.btnNoneModels = New System.Windows.Forms.Button
        Me.btnAllModels = New System.Windows.Forms.Button
        Me.btnNoneEmission = New System.Windows.Forms.Button
        Me.btnAllEmission = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.rdoModify = New System.Windows.Forms.RadioButton
        Me.rdoModels = New System.Windows.Forms.RadioButton
        Me.rdoEmission = New System.Windows.Forms.RadioButton
        Me.rdoLanduse = New System.Windows.Forms.RadioButton
        Me.lstboEmission = New System.Windows.Forms.ListBox
        Me.lstboModifications = New System.Windows.Forms.ListBox
        Me.lstboModels = New System.Windows.Forms.ListBox
        Me.lstboLanduse = New System.Windows.Forms.ListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cboSelect = New System.Windows.Forms.ComboBox
        Me.btnDone = New System.Windows.Forms.Button
        Me.btnCancelPlot = New System.Windows.Forms.Button
        Me.grpSelect.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboXAxis
        '
        Me.cboXAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboXAxis.FormattingEnabled = True
        Me.cboXAxis.Location = New System.Drawing.Point(112, 12)
        Me.cboXAxis.Name = "cboXAxis"
        Me.cboXAxis.Size = New System.Drawing.Size(375, 21)
        Me.cboXAxis.TabIndex = 0
        '
        'cboYAxis
        '
        Me.cboYAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboYAxis.FormattingEnabled = True
        Me.cboYAxis.Location = New System.Drawing.Point(112, 39)
        Me.cboYAxis.Name = "cboYAxis"
        Me.cboYAxis.Size = New System.Drawing.Size(375, 21)
        Me.cboYAxis.TabIndex = 1
        '
        'cboZAxis
        '
        Me.cboZAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboZAxis.FormattingEnabled = True
        Me.cboZAxis.Items.AddRange(New Object() {"None"})
        Me.cboZAxis.Location = New System.Drawing.Point(112, 66)
        Me.cboZAxis.Name = "cboZAxis"
        Me.cboZAxis.Size = New System.Drawing.Size(375, 21)
        Me.cboZAxis.TabIndex = 2
        Me.cboZAxis.Text = "None"
        '
        'lblXAxis
        '
        Me.lblXAxis.AutoSize = True
        Me.lblXAxis.Location = New System.Drawing.Point(12, 15)
        Me.lblXAxis.Name = "lblXAxis"
        Me.lblXAxis.Size = New System.Drawing.Size(36, 13)
        Me.lblXAxis.TabIndex = 3
        Me.lblXAxis.Text = "X Axis"
        '
        'lblYAxis
        '
        Me.lblYAxis.AutoSize = True
        Me.lblYAxis.Location = New System.Drawing.Point(12, 42)
        Me.lblYAxis.Name = "lblYAxis"
        Me.lblYAxis.Size = New System.Drawing.Size(36, 13)
        Me.lblYAxis.TabIndex = 5
        Me.lblYAxis.Text = "Y Axis"
        '
        'lblZAxis
        '
        Me.lblZAxis.AutoSize = True
        Me.lblZAxis.Location = New System.Drawing.Point(12, 69)
        Me.lblZAxis.Name = "lblZAxis"
        Me.lblZAxis.Size = New System.Drawing.Size(36, 13)
        Me.lblZAxis.TabIndex = 6
        Me.lblZAxis.Text = "Z Axis"
        '
        'btnPlot
        '
        Me.btnPlot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPlot.Location = New System.Drawing.Point(412, 385)
        Me.btnPlot.Name = "btnPlot"
        Me.btnPlot.Size = New System.Drawing.Size(75, 23)
        Me.btnPlot.TabIndex = 7
        Me.btnPlot.Text = "Plot"
        Me.btnPlot.UseVisualStyleBackColor = True
        '
        'txtTitle
        '
        Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTitle.Location = New System.Drawing.Point(48, 359)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(442, 20)
        Me.txtTitle.TabIndex = 8
        '
        'lblTitle
        '
        Me.lblTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(15, 362)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(27, 13)
        Me.lblTitle.TabIndex = 9
        Me.lblTitle.Text = "Title"
        '
        'lblPointLabels
        '
        Me.lblPointLabels.AutoSize = True
        Me.lblPointLabels.Location = New System.Drawing.Point(12, 96)
        Me.lblPointLabels.Name = "lblPointLabels"
        Me.lblPointLabels.Size = New System.Drawing.Size(65, 13)
        Me.lblPointLabels.TabIndex = 11
        Me.lblPointLabels.Text = "Point Labels"
        '
        'cboPointLabels
        '
        Me.cboPointLabels.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPointLabels.FormattingEnabled = True
        Me.cboPointLabels.Items.AddRange(New Object() {"None"})
        Me.cboPointLabels.Location = New System.Drawing.Point(112, 93)
        Me.cboPointLabels.Name = "cboPointLabels"
        Me.cboPointLabels.Size = New System.Drawing.Size(375, 21)
        Me.cboPointLabels.TabIndex = 10
        Me.cboPointLabels.Text = "None"
        '
        'grpSelect
        '
        Me.grpSelect.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSelect.Controls.Add(Me.btnNoneModifications)
        Me.grpSelect.Controls.Add(Me.btnAllModifications)
        Me.grpSelect.Controls.Add(Me.btnNoneModels)
        Me.grpSelect.Controls.Add(Me.btnAllModels)
        Me.grpSelect.Controls.Add(Me.btnNoneEmission)
        Me.grpSelect.Controls.Add(Me.btnAllEmission)
        Me.grpSelect.Controls.Add(Me.Label2)
        Me.grpSelect.Controls.Add(Me.rdoModify)
        Me.grpSelect.Controls.Add(Me.rdoModels)
        Me.grpSelect.Controls.Add(Me.rdoEmission)
        Me.grpSelect.Controls.Add(Me.rdoLanduse)
        Me.grpSelect.Controls.Add(Me.lstboEmission)
        Me.grpSelect.Controls.Add(Me.lstboModifications)
        Me.grpSelect.Controls.Add(Me.lstboModels)
        Me.grpSelect.Controls.Add(Me.lstboLanduse)
        Me.grpSelect.Controls.Add(Me.Label1)
        Me.grpSelect.Controls.Add(Me.cboSelect)
        Me.grpSelect.Location = New System.Drawing.Point(12, 120)
        Me.grpSelect.Name = "grpSelect"
        Me.grpSelect.Size = New System.Drawing.Size(475, 233)
        Me.grpSelect.TabIndex = 14
        Me.grpSelect.TabStop = False
        Me.grpSelect.Text = "Select Data For Plot"
        '
        'btnNoneModifications
        '
        Me.btnNoneModifications.Location = New System.Drawing.Point(413, 204)
        Me.btnNoneModifications.Name = "btnNoneModifications"
        Me.btnNoneModifications.Size = New System.Drawing.Size(56, 23)
        Me.btnNoneModifications.TabIndex = 38
        Me.btnNoneModifications.Text = "None"
        Me.btnNoneModifications.UseVisualStyleBackColor = True
        '
        'btnAllModifications
        '
        Me.btnAllModifications.Location = New System.Drawing.Point(377, 204)
        Me.btnAllModifications.Name = "btnAllModifications"
        Me.btnAllModifications.Size = New System.Drawing.Size(30, 23)
        Me.btnAllModifications.TabIndex = 37
        Me.btnAllModifications.Text = "All"
        Me.btnAllModifications.UseVisualStyleBackColor = True
        '
        'btnNoneModels
        '
        Me.btnNoneModels.Location = New System.Drawing.Point(291, 204)
        Me.btnNoneModels.Name = "btnNoneModels"
        Me.btnNoneModels.Size = New System.Drawing.Size(56, 23)
        Me.btnNoneModels.TabIndex = 36
        Me.btnNoneModels.Text = "None"
        Me.btnNoneModels.UseVisualStyleBackColor = True
        '
        'btnAllModels
        '
        Me.btnAllModels.Location = New System.Drawing.Point(255, 204)
        Me.btnAllModels.Name = "btnAllModels"
        Me.btnAllModels.Size = New System.Drawing.Size(30, 23)
        Me.btnAllModels.TabIndex = 35
        Me.btnAllModels.Text = "All"
        Me.btnAllModels.UseVisualStyleBackColor = True
        '
        'btnNoneEmission
        '
        Me.btnNoneEmission.Location = New System.Drawing.Point(168, 204)
        Me.btnNoneEmission.Name = "btnNoneEmission"
        Me.btnNoneEmission.Size = New System.Drawing.Size(56, 23)
        Me.btnNoneEmission.TabIndex = 34
        Me.btnNoneEmission.Text = "None"
        Me.btnNoneEmission.UseVisualStyleBackColor = True
        '
        'btnAllEmission
        '
        Me.btnAllEmission.Location = New System.Drawing.Point(132, 204)
        Me.btnAllEmission.Name = "btnAllEmission"
        Me.btnAllEmission.Size = New System.Drawing.Size(30, 23)
        Me.btnAllEmission.TabIndex = 33
        Me.btnAllEmission.Text = "All"
        Me.btnAllEmission.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(190, 13)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "Filter CAT result and specify data label:"
        '
        'rdoModify
        '
        Me.rdoModify.AutoSize = True
        Me.rdoModify.Location = New System.Drawing.Point(377, 84)
        Me.rdoModify.Name = "rdoModify"
        Me.rdoModify.Size = New System.Drawing.Size(87, 17)
        Me.rdoModify.TabIndex = 31
        Me.rdoModify.TabStop = True
        Me.rdoModify.Text = "Modifications"
        Me.rdoModify.UseVisualStyleBackColor = True
        '
        'rdoModels
        '
        Me.rdoModels.AutoSize = True
        Me.rdoModels.Location = New System.Drawing.Point(254, 84)
        Me.rdoModels.Name = "rdoModels"
        Me.rdoModels.Size = New System.Drawing.Size(103, 17)
        Me.rdoModels.TabIndex = 30
        Me.rdoModels.TabStop = True
        Me.rdoModels.Text = "Weather Models"
        Me.rdoModels.UseVisualStyleBackColor = True
        '
        'rdoEmission
        '
        Me.rdoEmission.AutoSize = True
        Me.rdoEmission.Location = New System.Drawing.Point(132, 84)
        Me.rdoEmission.Name = "rdoEmission"
        Me.rdoEmission.Size = New System.Drawing.Size(116, 17)
        Me.rdoEmission.TabIndex = 29
        Me.rdoEmission.TabStop = True
        Me.rdoEmission.Text = "Emission Scenarios"
        Me.rdoEmission.UseVisualStyleBackColor = True
        '
        'rdoLanduse
        '
        Me.rdoLanduse.AutoSize = True
        Me.rdoLanduse.Enabled = False
        Me.rdoLanduse.Location = New System.Drawing.Point(12, 84)
        Me.rdoLanduse.Name = "rdoLanduse"
        Me.rdoLanduse.Size = New System.Drawing.Size(66, 17)
        Me.rdoLanduse.TabIndex = 28
        Me.rdoLanduse.TabStop = True
        Me.rdoLanduse.Text = "Landuse"
        Me.rdoLanduse.UseVisualStyleBackColor = True
        '
        'lstboEmission
        '
        Me.lstboEmission.FormattingEnabled = True
        Me.lstboEmission.Location = New System.Drawing.Point(132, 107)
        Me.lstboEmission.Name = "lstboEmission"
        Me.lstboEmission.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstboEmission.Size = New System.Drawing.Size(92, 95)
        Me.lstboEmission.TabIndex = 27
        '
        'lstboModifications
        '
        Me.lstboModifications.FormattingEnabled = True
        Me.lstboModifications.Location = New System.Drawing.Point(377, 107)
        Me.lstboModifications.Name = "lstboModifications"
        Me.lstboModifications.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstboModifications.Size = New System.Drawing.Size(92, 95)
        Me.lstboModifications.TabIndex = 25
        '
        'lstboModels
        '
        Me.lstboModels.FormattingEnabled = True
        Me.lstboModels.Location = New System.Drawing.Point(255, 107)
        Me.lstboModels.Name = "lstboModels"
        Me.lstboModels.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstboModels.Size = New System.Drawing.Size(92, 95)
        Me.lstboModels.TabIndex = 24
        '
        'lstboLanduse
        '
        Me.lstboLanduse.FormattingEnabled = True
        Me.lstboLanduse.Location = New System.Drawing.Point(12, 107)
        Me.lstboLanduse.Name = "lstboLanduse"
        Me.lstboLanduse.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstboLanduse.Size = New System.Drawing.Size(92, 95)
        Me.lstboLanduse.TabIndex = 23
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Selection Field"
        '
        'cboSelect
        '
        Me.cboSelect.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSelect.FormattingEnabled = True
        Me.cboSelect.Items.AddRange(New Object() {"None"})
        Me.cboSelect.Location = New System.Drawing.Point(100, 28)
        Me.cboSelect.Name = "cboSelect"
        Me.cboSelect.Size = New System.Drawing.Size(369, 21)
        Me.cboSelect.TabIndex = 15
        Me.cboSelect.Text = "None"
        '
        'btnDone
        '
        Me.btnDone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDone.Location = New System.Drawing.Point(18, 389)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(75, 23)
        Me.btnDone.TabIndex = 15
        Me.btnDone.Text = "Done"
        Me.btnDone.UseVisualStyleBackColor = True
        '
        'btnCancelPlot
        '
        Me.btnCancelPlot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancelPlot.Location = New System.Drawing.Point(99, 389)
        Me.btnCancelPlot.Name = "btnCancelPlot"
        Me.btnCancelPlot.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelPlot.TabIndex = 16
        Me.btnCancelPlot.Text = "Cancel"
        Me.btnCancelPlot.UseVisualStyleBackColor = True
        '
        'frmPlot
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(499, 420)
        Me.Controls.Add(Me.btnCancelPlot)
        Me.Controls.Add(Me.btnDone)
        Me.Controls.Add(Me.grpSelect)
        Me.Controls.Add(Me.lblPointLabels)
        Me.Controls.Add(Me.cboPointLabels)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.btnPlot)
        Me.Controls.Add(Me.lblZAxis)
        Me.Controls.Add(Me.lblYAxis)
        Me.Controls.Add(Me.lblXAxis)
        Me.Controls.Add(Me.cboZAxis)
        Me.Controls.Add(Me.cboYAxis)
        Me.Controls.Add(Me.cboXAxis)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPlot"
        Me.Text = "Select Plot"
        Me.grpSelect.ResumeLayout(False)
        Me.grpSelect.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboXAxis As System.Windows.Forms.ComboBox
    Friend WithEvents cboYAxis As System.Windows.Forms.ComboBox
    Friend WithEvents cboZAxis As System.Windows.Forms.ComboBox
    Friend WithEvents lblXAxis As System.Windows.Forms.Label
    Friend WithEvents lblYAxis As System.Windows.Forms.Label
    Friend WithEvents lblZAxis As System.Windows.Forms.Label
    Friend WithEvents btnPlot As System.Windows.Forms.Button
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblPointLabels As System.Windows.Forms.Label
    Friend WithEvents cboPointLabels As System.Windows.Forms.ComboBox
    Friend WithEvents grpSelect As System.Windows.Forms.GroupBox
    Friend WithEvents cboSelect As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lstboModifications As System.Windows.Forms.ListBox
    Friend WithEvents lstboModels As System.Windows.Forms.ListBox
    Friend WithEvents lstboLanduse As System.Windows.Forms.ListBox
    Friend WithEvents lstboEmission As System.Windows.Forms.ListBox
    Friend WithEvents rdoLanduse As System.Windows.Forms.RadioButton
    Friend WithEvents rdoModify As System.Windows.Forms.RadioButton
    Friend WithEvents rdoModels As System.Windows.Forms.RadioButton
    Friend WithEvents rdoEmission As System.Windows.Forms.RadioButton
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents btnCancelPlot As System.Windows.Forms.Button
    Friend WithEvents btnNoneEmission As System.Windows.Forms.Button
    Friend WithEvents btnAllEmission As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnNoneModifications As System.Windows.Forms.Button
    Friend WithEvents btnAllModifications As System.Windows.Forms.Button
    Friend WithEvents btnNoneModels As System.Windows.Forms.Button
    Friend WithEvents btnAllModels As System.Windows.Forms.Button
End Class
