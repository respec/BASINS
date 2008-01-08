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
        Me.atcDataGroupDates = New atcData.atcChooseDataGroupDates
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
        Me.lstChooseGraphs.Size = New System.Drawing.Size(316, 122)
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
        Me.btnGenerate.Location = New System.Drawing.Point(253, 231)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerate.TabIndex = 2
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(172, 231)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'atcDataGroupDates
        '
        Me.atcDataGroupDates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atcDataGroupDates.CommonEnd = 1.7976931348623157E+308
        Me.atcDataGroupDates.CommonStart = -1.7976931348623157E+308
        Me.atcDataGroupDates.DataGroup = Nothing
        Me.atcDataGroupDates.FirstStart = 1.7976931348623157E+308
        Me.atcDataGroupDates.LastEnd = -1.7976931348623157E+308
        Me.atcDataGroupDates.Location = New System.Drawing.Point(12, 140)
        Me.atcDataGroupDates.Name = "atcDataGroupDates"
        Me.atcDataGroupDates.OmitAfter = -1
        Me.atcDataGroupDates.OmitBefore = -1
        Me.atcDataGroupDates.Size = New System.Drawing.Size(316, 85)
        Me.atcDataGroupDates.TabIndex = 4
        '
        'frmChooseGraphs
        '
        Me.AcceptButton = Me.btnGenerate
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(340, 266)
        Me.Controls.Add(Me.atcDataGroupDates)
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
    Friend WithEvents atcDataGroupDates As atcData.atcChooseDataGroupDates
End Class
