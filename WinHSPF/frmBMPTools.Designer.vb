<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBMPTools
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBMPTools))
        Me.cmdLID = New System.Windows.Forms.Button
        Me.cmdOpen = New System.Windows.Forms.Button
        Me.cboID = New System.Windows.Forms.ComboBox
        Me.lblReach = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdLID
        '
        Me.cmdLID.Location = New System.Drawing.Point(28, 79)
        Me.cmdLID.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdLID.Name = "cmdLID"
        Me.cmdLID.Size = New System.Drawing.Size(97, 38)
        Me.cmdLID.TabIndex = 11
        Me.cmdLID.Text = "LID Controls Tool"
        Me.cmdLID.UseVisualStyleBackColor = True
        '
        'cmdOpen
        '
        Me.cmdOpen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOpen.Location = New System.Drawing.Point(162, 79)
        Me.cmdOpen.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdOpen.Name = "cmdOpen"
        Me.cmdOpen.Size = New System.Drawing.Size(97, 38)
        Me.cmdOpen.TabIndex = 12
        Me.cmdOpen.Text = "Sewer and Open Channel Tool"
        Me.cmdOpen.UseVisualStyleBackColor = True
        '
        'cboID
        '
        Me.cboID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboID.FormattingEnabled = True
        Me.cboID.Location = New System.Drawing.Point(28, 38)
        Me.cboID.Name = "cboID"
        Me.cboID.Size = New System.Drawing.Size(231, 21)
        Me.cboID.TabIndex = 13
        '
        'lblReach
        '
        Me.lblReach.AutoSize = True
        Me.lblReach.Location = New System.Drawing.Point(25, 22)
        Me.lblReach.Name = "lblReach"
        Me.lblReach.Size = New System.Drawing.Size(56, 13)
        Me.lblReach.TabIndex = 14
        Me.lblReach.Text = "Reach ID:"
        '
        'frmBMPTools
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 142)
        Me.Controls.Add(Me.lblReach)
        Me.Controls.Add(Me.cboID)
        Me.Controls.Add(Me.cmdOpen)
        Me.Controls.Add(Me.cmdLID)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBMPTools"
        Me.Text = "BMP Reach Toolkit"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdLID As System.Windows.Forms.Button
    Friend WithEvents cmdOpen As System.Windows.Forms.Button
    Friend WithEvents cboID As System.Windows.Forms.ComboBox
    Friend WithEvents lblReach As System.Windows.Forms.Label
End Class
