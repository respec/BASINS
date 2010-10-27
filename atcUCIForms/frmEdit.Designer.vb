<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEdit
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
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.panelEdit = New System.Windows.Forms.Panel
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.Location = New System.Drawing.Point(9, 219)
        Me.cmdOk.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(60, 32)
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(74, 219)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(60, 32)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdApply
        '
        Me.cmdApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.Location = New System.Drawing.Point(138, 219)
        Me.cmdApply.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.Size = New System.Drawing.Size(60, 32)
        Me.cmdApply.TabIndex = 2
        Me.cmdApply.Text = "Apply"
        Me.cmdApply.UseVisualStyleBackColor = True
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(202, 219)
        Me.cmdHelp.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(60, 32)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.Text = "Help"
        Me.cmdHelp.UseVisualStyleBackColor = True
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.Location = New System.Drawing.Point(415, 219)
        Me.cmdEdit.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(60, 32)
        Me.cmdEdit.TabIndex = 4
        Me.cmdEdit.Text = "Edit"
        Me.cmdEdit.UseVisualStyleBackColor = True
        '
        'cmdRemove
        '
        Me.cmdRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemove.Location = New System.Drawing.Point(350, 219)
        Me.cmdRemove.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.Size = New System.Drawing.Size(60, 32)
        Me.cmdRemove.TabIndex = 5
        Me.cmdRemove.Text = "Remove"
        Me.cmdRemove.UseVisualStyleBackColor = True
        '
        'cmdAdd
        '
        Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.Location = New System.Drawing.Point(286, 219)
        Me.cmdAdd.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(60, 32)
        Me.cmdAdd.TabIndex = 6
        Me.cmdAdd.Text = "Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'panelEdit
        '
        Me.panelEdit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelEdit.Location = New System.Drawing.Point(0, 0)
        Me.panelEdit.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.panelEdit.Name = "panelEdit"
        Me.panelEdit.Size = New System.Drawing.Size(484, 214)
        Me.panelEdit.TabIndex = 7
        '
        'frmEdit
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(484, 252)
        Me.Controls.Add(Me.panelEdit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdRemove)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "frmEdit"
        Me.Text = "frmEdit"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdOk As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdApply As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdEdit As System.Windows.Forms.Button
    Friend WithEvents cmdRemove As System.Windows.Forms.Button
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents panelEdit As System.Windows.Forms.Panel
End Class
