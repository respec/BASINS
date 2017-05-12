<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAbout
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Me.lblSARA = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.pictureLogo = New System.Windows.Forms.PictureBox()
        Me.txtDisclaimer = New System.Windows.Forms.TextBox()
        CType(Me.pictureLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblSARA
        '
        Me.lblSARA.AutoSize = True
        Me.lblSARA.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSARA.Location = New System.Drawing.Point(137, 12)
        Me.lblSARA.Name = "lblSARA"
        Me.lblSARA.Size = New System.Drawing.Size(466, 24)
        Me.lblSARA.TabIndex = 20
        Me.lblSARA.Text = "San Antonio River Authority HSPF Simulation Manager"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(138, 49)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(103, 13)
        Me.lblVersion.TabIndex = 21
        Me.lblVersion.Text = "Release 2017-05-12"
        '
        'pictureLogo
        '
        Me.pictureLogo.Image = Global.HSPFSimulationManager.My.Resources.Resources.transparent177x121
        Me.pictureLogo.Location = New System.Drawing.Point(12, 12)
        Me.pictureLogo.Name = "pictureLogo"
        Me.pictureLogo.Size = New System.Drawing.Size(119, 81)
        Me.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pictureLogo.TabIndex = 19
        Me.pictureLogo.TabStop = False
        '
        'txtDisclaimer
        '
        Me.txtDisclaimer.BackColor = System.Drawing.SystemColors.Menu
        Me.txtDisclaimer.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDisclaimer.CausesValidation = False
        Me.txtDisclaimer.Enabled = False
        Me.txtDisclaimer.Location = New System.Drawing.Point(24, 109)
        Me.txtDisclaimer.Multiline = True
        Me.txtDisclaimer.Name = "txtDisclaimer"
        Me.txtDisclaimer.Size = New System.Drawing.Size(579, 137)
        Me.txtDisclaimer.TabIndex = 23
        Me.txtDisclaimer.Text = resources.GetString("txtDisclaimer.Text")
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 239)
        Me.Controls.Add(Me.txtDisclaimer)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblSARA)
        Me.Controls.Add(Me.pictureLogo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAbout"
        Me.Text = "About SARA HSPF Simulation Manager"
        CType(Me.pictureLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pictureLogo As System.Windows.Forms.PictureBox
    Friend WithEvents lblSARA As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents txtDisclaimer As TextBox
End Class
