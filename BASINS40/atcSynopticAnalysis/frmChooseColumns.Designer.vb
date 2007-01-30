<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChooseColumns
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChooseColumns))
        Me.lstColumns = New System.Windows.Forms.CheckedListBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnAll = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lstColumns
        '
        Me.lstColumns.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstColumns.CheckOnClick = True
        Me.lstColumns.FormattingEnabled = True
        Me.lstColumns.IntegralHeight = False
        Me.lstColumns.Location = New System.Drawing.Point(12, 12)
        Me.lstColumns.Name = "lstColumns"
        Me.lstColumns.Size = New System.Drawing.Size(252, 289)
        Me.lstColumns.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnCancel.Location = New System.Drawing.Point(184, 307)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 24)
        Me.btnCancel.TabIndex = 22
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnOk.Location = New System.Drawing.Point(98, 307)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(80, 24)
        Me.btnOk.TabIndex = 21
        Me.btnOk.Text = "Ok"
        '
        'btnAll
        '
        Me.btnAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAll.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnAll.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAll.Location = New System.Drawing.Point(12, 307)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(80, 24)
        Me.btnAll.TabIndex = 23
        Me.btnAll.Text = "All"
        '
        'frmChooseColumns
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(276, 343)
        Me.Controls.Add(Me.btnAll)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lstColumns)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmChooseColumns"
        Me.Text = "Choose Columns"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstColumns As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnAll As System.Windows.Forms.Button
End Class
