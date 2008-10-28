<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmActivityAll
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
        Me.SSTabPIR = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.SSTabPIR.SuspendLayout()
        Me.SuspendLayout()
        '
        'SSTabPIR
        '
        Me.SSTabPIR.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SSTabPIR.Controls.Add(Me.TabPage1)
        Me.SSTabPIR.Controls.Add(Me.TabPage2)
        Me.SSTabPIR.Controls.Add(Me.TabPage3)
        Me.SSTabPIR.Location = New System.Drawing.Point(20, 12)
        Me.SSTabPIR.Name = "SSTabPIR"
        Me.SSTabPIR.SelectedIndex = 0
        Me.SSTabPIR.Size = New System.Drawing.Size(641, 310)
        Me.SSTabPIR.TabIndex = 0
        Me.SSTabPIR.Tag = ""
        '
        'TabPage1
        '
        Me.TabPage1.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(633, 284)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Pervious Land"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(633, 284)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Impervious Land"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(633, 284)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Reaches/Reservoirs"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdOK.Location = New System.Drawing.Point(104, 334)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(100, 30)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdCancel.Location = New System.Drawing.Point(229, 334)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(100, 30)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdApply
        '
        Me.cmdApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdApply.Location = New System.Drawing.Point(354, 334)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.Size = New System.Drawing.Size(100, 30)
        Me.cmdApply.TabIndex = 3
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.UseVisualStyleBackColor = True
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdHelp.Location = New System.Drawing.Point(479, 334)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(100, 30)
        Me.cmdHelp.TabIndex = 4
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.UseVisualStyleBackColor = True
        '
        'frmActivityAll
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(682, 375)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.SSTabPIR)
        Me.Name = "frmActivityAll"
        Me.Text = "WinHSPF - Edit All Activity"
        Me.SSTabPIR.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SSTabPIR As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdApply As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
End Class
