<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWASPInitialize
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWASPInitialize))
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.cmdRefresh = New System.Windows.Forms.Button
        Me.txtInfo = New System.Windows.Forms.TextBox
        Me.txtWarning = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(132, 194)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(103, 32)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(252, 194)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(103, 32)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdSelect
        '
        Me.cmdSelect.Location = New System.Drawing.Point(12, 12)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.Size = New System.Drawing.Size(103, 49)
        Me.cmdSelect.TabIndex = 2
        Me.cmdSelect.Text = "Select Upstream"
        Me.cmdSelect.UseVisualStyleBackColor = True
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Location = New System.Drawing.Point(12, 76)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(103, 49)
        Me.cmdRefresh.TabIndex = 3
        Me.cmdRefresh.Text = "Refresh Selection"
        Me.cmdRefresh.UseVisualStyleBackColor = True
        '
        'txtInfo
        '
        Me.txtInfo.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.txtInfo.Location = New System.Drawing.Point(141, 28)
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.ReadOnly = True
        Me.txtInfo.Size = New System.Drawing.Size(312, 78)
        Me.txtInfo.TabIndex = 4
        '
        'txtWarning
        '
        Me.txtWarning.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.txtWarning.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtWarning.ForeColor = System.Drawing.Color.Maroon
        Me.txtWarning.Location = New System.Drawing.Point(23, 141)
        Me.txtWarning.Multiline = True
        Me.txtWarning.Name = "txtWarning"
        Me.txtWarning.ReadOnly = True
        Me.txtWarning.Size = New System.Drawing.Size(441, 41)
        Me.txtWarning.TabIndex = 5
        '
        'frmWASPInitialize
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(486, 238)
        Me.Controls.Add(Me.txtWarning)
        Me.Controls.Add(Me.txtInfo)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWASPInitialize"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "BASINS WASP Initialization"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdSelect As System.Windows.Forms.Button
    Friend WithEvents cmdRefresh As System.Windows.Forms.Button
    Friend WithEvents txtInfo As System.Windows.Forms.TextBox
    Friend WithEvents txtWarning As System.Windows.Forms.TextBox
End Class
