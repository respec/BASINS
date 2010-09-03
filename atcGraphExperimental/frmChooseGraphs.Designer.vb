<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChooseGraphs
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChooseGraphs))
        Me.lstChooseGraphs = New System.Windows.Forms.CheckedListBox
        Me.lblChooseGraphs = New System.Windows.Forms.Label
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAll = New System.Windows.Forms.Button
        Me.btnNone = New System.Windows.Forms.Button
        Me.cbxMultiple = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'lstChooseGraphs
        '
        Me.lstChooseGraphs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstChooseGraphs.CheckOnClick = True
        Me.lstChooseGraphs.IntegralHeight = False
        Me.lstChooseGraphs.Location = New System.Drawing.Point(12, 12)
        Me.lstChooseGraphs.Name = "lstChooseGraphs"
        Me.lstChooseGraphs.Size = New System.Drawing.Size(316, 131)
        Me.lstChooseGraphs.TabIndex = 0
        '
        'lblChooseGraphs
        '
        Me.lblChooseGraphs.AutoSize = True
        Me.lblChooseGraphs.Location = New System.Drawing.Point(12, 9)
        Me.lblChooseGraphs.Name = "lblChooseGraphs"
        Me.lblChooseGraphs.Size = New System.Drawing.Size(0, 13)
        Me.lblChooseGraphs.TabIndex = 1
        '
        'btnGenerate
        '
        Me.btnGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerate.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnGenerate.Location = New System.Drawing.Point(253, 171)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerate.TabIndex = 4
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(172, 171)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAll
        '
        Me.btnAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAll.Location = New System.Drawing.Point(12, 171)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(61, 23)
        Me.btnAll.TabIndex = 1
        Me.btnAll.Text = "All"
        Me.btnAll.UseVisualStyleBackColor = True
        '
        'btnNone
        '
        Me.btnNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNone.Location = New System.Drawing.Point(79, 171)
        Me.btnNone.Name = "btnNone"
        Me.btnNone.Size = New System.Drawing.Size(61, 23)
        Me.btnNone.TabIndex = 2
        Me.btnNone.Text = "None"
        Me.btnNone.UseVisualStyleBackColor = True
        '
        'cbxMultiple
        '
        Me.cbxMultiple.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbxMultiple.AutoSize = True
        Me.cbxMultiple.Location = New System.Drawing.Point(192, 149)
        Me.cbxMultiple.Name = "cbxMultiple"
        Me.cbxMultiple.Size = New System.Drawing.Size(110, 17)
        Me.cbxMultiple.TabIndex = 5
        Me.cbxMultiple.Text = "Multiple WQ Plots"
        Me.cbxMultiple.UseVisualStyleBackColor = True
        '
        'frmChooseGraphs
        '
        Me.AcceptButton = Me.btnGenerate
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(340, 206)
        Me.Controls.Add(Me.cbxMultiple)
        Me.Controls.Add(Me.btnNone)
        Me.Controls.Add(Me.btnAll)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.lblChooseGraphs)
        Me.Controls.Add(Me.lstChooseGraphs)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmChooseGraphs"
        Me.Text = "Choose Graphs to Create"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstChooseGraphs As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblChooseGraphs As System.Windows.Forms.Label
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnAll As System.Windows.Forms.Button
    Friend WithEvents btnNone As System.Windows.Forms.Button
    Friend WithEvents cbxMultiple As System.Windows.Forms.CheckBox
End Class
