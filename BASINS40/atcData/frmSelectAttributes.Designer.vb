<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSelectAttributes
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.chkCalculated = New System.Windows.Forms.CheckBox()
        Me.chkNotCalculated = New System.Windows.Forms.CheckBox()
        Me.ctlSelect = New atcControls.ATCoSelectListSortByProp()
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(288, 334)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "Ok"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(358, 334)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        '
        'chkCalculated
        '
        Me.chkCalculated.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkCalculated.AutoSize = True
        Me.chkCalculated.Checked = True
        Me.chkCalculated.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCalculated.Location = New System.Drawing.Point(12, 318)
        Me.chkCalculated.Name = "chkCalculated"
        Me.chkCalculated.Size = New System.Drawing.Size(106, 17)
        Me.chkCalculated.TabIndex = 4
        Me.chkCalculated.Text = "Show Calculated"
        Me.chkCalculated.UseVisualStyleBackColor = True
        '
        'chkNotCalculated
        '
        Me.chkNotCalculated.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkNotCalculated.AutoSize = True
        Me.chkNotCalculated.Checked = True
        Me.chkNotCalculated.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNotCalculated.Location = New System.Drawing.Point(12, 341)
        Me.chkNotCalculated.Name = "chkNotCalculated"
        Me.chkNotCalculated.Size = New System.Drawing.Size(126, 17)
        Me.chkNotCalculated.TabIndex = 5
        Me.chkNotCalculated.Text = "Show Not Calculated"
        Me.chkNotCalculated.UseVisualStyleBackColor = True
        '
        'ctlSelect
        '
        Me.ctlSelect.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ctlSelect.DisplayMember = ""
        Me.ctlSelect.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctlSelect.LeftLabel = "Available:"
        Me.ctlSelect.Location = New System.Drawing.Point(12, 12)
        Me.ctlSelect.MoveDownTip = "Move Item Down In List"
        Me.ctlSelect.MoveUpTip = "Move Item Up In List"
        Me.ctlSelect.Name = "ctlSelect"
        Me.ctlSelect.RightLabel = "Selected:"
        Me.ctlSelect.Size = New System.Drawing.Size(410, 300)
        Me.ctlSelect.SortMember = Nothing
        Me.ctlSelect.TabIndex = 0
        '
        'frmSelectAttributes
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(434, 370)
        Me.Controls.Add(Me.chkNotCalculated)
        Me.Controls.Add(Me.chkCalculated)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.ctlSelect)
        Me.Name = "frmSelectAttributes"
        Me.Text = "Select Attributes"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ctlSelect As atcControls.ATCoSelectListSortByProp
    Friend WithEvents btnOk As Windows.Forms.Button
    Friend WithEvents btnCancel As Windows.Forms.Button
    Friend WithEvents chkCalculated As Windows.Forms.CheckBox
    Friend WithEvents chkNotCalculated As Windows.Forms.CheckBox
End Class
