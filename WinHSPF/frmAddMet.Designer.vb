<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddMet
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.cboName = New System.Windows.Forms.ComboBox
        Me.agdMet = New atcControls.atcGrid
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.lblName = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name:"
        '
        'cboName
        '
        Me.cboName.FormattingEnabled = True
        Me.cboName.Location = New System.Drawing.Point(53, 10)
        Me.cboName.Name = "cboName"
        Me.cboName.Size = New System.Drawing.Size(308, 21)
        Me.cboName.TabIndex = 1
        '
        'agdMet
        '
        Me.agdMet.AllowHorizontalScrolling = True
        Me.agdMet.AllowNewValidValues = False
        Me.agdMet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdMet.CellBackColor = System.Drawing.Color.Empty
        Me.agdMet.Fixed3D = False
        Me.agdMet.LineColor = System.Drawing.Color.Empty
        Me.agdMet.LineWidth = 0.0!
        Me.agdMet.Location = New System.Drawing.Point(11, 43)
        Me.agdMet.Name = "agdMet"
        Me.agdMet.Size = New System.Drawing.Size(488, 249)
        Me.agdMet.Source = Nothing
        Me.agdMet.TabIndex = 2
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdCancel.Location = New System.Drawing.Point(263, 313)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(76, 25)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdOK.Location = New System.Drawing.Point(175, 313)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(76, 25)
        Me.cmdOK.TabIndex = 7
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(59, 14)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 9
        Me.lblName.Text = "Name:"
        '
        'frmAddMet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(515, 350)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.agdMet)
        Me.Controls.Add(Me.cboName)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmAddMet"
        Me.Text = "WinHSPF - Add Met Segment"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboName As System.Windows.Forms.ComboBox
    Friend WithEvents agdMet As atcControls.atcGrid
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents lblName As System.Windows.Forms.Label
End Class
