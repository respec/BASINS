<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Me.pictureLogo = New System.Windows.Forms.PictureBox()
        Me.lblSARA = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.UpdateLink = New System.Windows.Forms.LinkLabel()
        CType(Me.pictureLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pictureLogo
        '
        Me.pictureLogo.Image = Global.TimeseriesUtility.My.Resources.Resources.transparent177x121
        Me.pictureLogo.Location = New System.Drawing.Point(12, 12)
        Me.pictureLogo.Name = "pictureLogo"
        Me.pictureLogo.Size = New System.Drawing.Size(119, 81)
        Me.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pictureLogo.TabIndex = 19
        Me.pictureLogo.TabStop = False
        '
        'lblSARA
        '
        Me.lblSARA.AutoSize = True
        Me.lblSARA.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSARA.Location = New System.Drawing.Point(137, 12)
        Me.lblSARA.Name = "lblSARA"
        Me.lblSARA.Size = New System.Drawing.Size(384, 24)
        Me.lblSARA.TabIndex = 20
        Me.lblSARA.Text = "San Antonio River Authority Timeseries Utility"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(138, 49)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(103, 13)
        Me.lblVersion.TabIndex = 21
        Me.lblVersion.Text = "Release 2015-08-13"
        '
        'UpdateLink
        '
        Me.UpdateLink.AutoSize = True
        Me.UpdateLink.LinkArea = New System.Windows.Forms.LinkArea(21, 39)
        Me.UpdateLink.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline
        Me.UpdateLink.Location = New System.Drawing.Point(138, 80)
        Me.UpdateLink.Name = "UpdateLink"
        Me.UpdateLink.Size = New System.Drawing.Size(308, 17)
        Me.UpdateLink.TabIndex = 22
        Me.UpdateLink.TabStop = True
        Me.UpdateLink.Text = "Updates and Support: http://aquaterra.com/TimeseriesUtility/"
        Me.UpdateLink.UseCompatibleTextRendering = True
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(541, 120)
        Me.Controls.Add(Me.UpdateLink)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblSARA)
        Me.Controls.Add(Me.pictureLogo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAbout"
        Me.Text = "About SARA Timeseries Utility"
        CType(Me.pictureLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pictureLogo As System.Windows.Forms.PictureBox
    Friend WithEvents lblSARA As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents UpdateLink As System.Windows.Forms.LinkLabel
End Class
