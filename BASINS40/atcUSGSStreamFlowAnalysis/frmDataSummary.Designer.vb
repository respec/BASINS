<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDataSummary
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDataSummary))
        Me.txtDataSummary = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'txtDataSummary
        '
        Me.txtDataSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDataSummary.Location = New System.Drawing.Point(0, 0)
        Me.txtDataSummary.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDataSummary.Multiline = True
        Me.txtDataSummary.Name = "txtDataSummary"
        Me.txtDataSummary.ReadOnly = True
        Me.txtDataSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDataSummary.Size = New System.Drawing.Size(416, 526)
        Me.txtDataSummary.TabIndex = 0
        '
        'frmDataSummary
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(416, 526)
        Me.Controls.Add(Me.txtDataSummary)
        Me.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmDataSummary"
        Me.Text = "Data Summary"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDataSummary As System.Windows.Forms.TextBox
End Class
