<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTextReport
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
        Me.lblTxtReportTitle = New System.Windows.Forms.Label
        Me.txtReport = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblTxtReportTitle
        '
        Me.lblTxtReportTitle.AutoSize = True
        Me.lblTxtReportTitle.Location = New System.Drawing.Point(12, 9)
        Me.lblTxtReportTitle.Name = "lblTxtReportTitle"
        Me.lblTxtReportTitle.Size = New System.Drawing.Size(33, 13)
        Me.lblTxtReportTitle.TabIndex = 0
        Me.lblTxtReportTitle.Text = "Title: "
        '
        'txtReport
        '
        Me.txtReport.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtReport.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReport.Location = New System.Drawing.Point(12, 25)
        Me.txtReport.Multiline = True
        Me.txtReport.Name = "txtReport"
        Me.txtReport.Size = New System.Drawing.Size(581, 350)
        Me.txtReport.TabIndex = 1
        '
        'frmTextReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(605, 387)
        Me.Controls.Add(Me.txtReport)
        Me.Controls.Add(Me.lblTxtReportTitle)
        Me.Name = "frmTextReport"
        Me.Text = "Text Report"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTxtReportTitle As System.Windows.Forms.Label
    Friend WithEvents txtReport As System.Windows.Forms.TextBox
End Class
