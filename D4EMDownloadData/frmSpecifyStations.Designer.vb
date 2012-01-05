<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSpecifyStations
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
        Me.btnOk = New System.Windows.Forms.Button
        Me.txtStations = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(216, 332)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(102, 29)
        Me.btnOk.TabIndex = 63
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'txtStations
        '
        Me.txtStations.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStations.Location = New System.Drawing.Point(12, 12)
        Me.txtStations.Multiline = True
        Me.txtStations.Name = "txtStations"
        Me.txtStations.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtStations.Size = New System.Drawing.Size(306, 314)
        Me.txtStations.TabIndex = 64
        '
        'frmSpecifyStations
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(330, 373)
        Me.Controls.Add(Me.txtStations)
        Me.Controls.Add(Me.btnOk)
        Me.Name = "frmSpecifyStations"
        Me.Text = "Specify Stations"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtStations As System.Windows.Forms.TextBox
End Class
