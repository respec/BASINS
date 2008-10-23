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
        Me.SSTabPIR.SuspendLayout()
        Me.SuspendLayout()
        '
        'SSTabPIR
        '
        Me.SSTabPIR.Controls.Add(Me.TabPage1)
        Me.SSTabPIR.Controls.Add(Me.TabPage2)
        Me.SSTabPIR.Controls.Add(Me.TabPage3)
        Me.SSTabPIR.Location = New System.Drawing.Point(19, 12)
        Me.SSTabPIR.Name = "SSTabPIR"
        Me.SSTabPIR.SelectedIndex = 0
        Me.SSTabPIR.Size = New System.Drawing.Size(812, 443)
        Me.SSTabPIR.TabIndex = 0
        Me.SSTabPIR.Tag = ""
        '
        'TabPage1
        '
        Me.TabPage1.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(804, 417)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Pervious Land"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(670, 314)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Impervious Land"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(670, 314)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Reaches/Reservoirs"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'frmActivityAll
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(853, 553)
        Me.Controls.Add(Me.SSTabPIR)
        Me.Name = "frmActivityAll"
        Me.Text = "frmActivityAll"
        Me.SSTabPIR.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SSTabPIR As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
End Class
