<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtLabel = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblProgramName = New System.Windows.Forms.Label()
        Me.lblAquaTerraURL = New System.Windows.Forms.LinkLabel()
        Me.lblProgramURL = New System.Windows.Forms.LinkLabel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(201, 484)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(121, 31)
        Me.cmdClose.TabIndex = 3
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "basins.gif")
        Me.ImageList1.Images.SetKeyName(1, "atcLogo.gif")
        Me.ImageList1.Images.SetKeyName(2, "logo.png")
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(14, 312)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(450, 151)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        '
        'txtLabel
        '
        Me.txtLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLabel.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtLabel.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtLabel.CausesValidation = False
        Me.txtLabel.Location = New System.Drawing.Point(12, 29)
        Me.txtLabel.Margin = New System.Windows.Forms.Padding(2)
        Me.txtLabel.Multiline = True
        Me.txtLabel.Name = "txtLabel"
        Me.txtLabel.ReadOnly = True
        Me.txtLabel.Size = New System.Drawing.Size(426, 101)
        Me.txtLabel.TabIndex = 3
        Me.txtLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblAquaTerraURL)
        Me.GroupBox2.Controls.Add(Me.PictureBox2)
        Me.GroupBox2.Location = New System.Drawing.Point(14, 145)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(450, 161)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Developed by AQUA TERRA Division of RESPEC"
        '
        'lblProgramName
        '
        Me.lblProgramName.AutoSize = True
        Me.lblProgramName.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgramName.Location = New System.Drawing.Point(223, 44)
        Me.lblProgramName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblProgramName.Name = "lblProgramName"
        Me.lblProgramName.Size = New System.Drawing.Size(182, 42)
        Me.lblProgramName.TabIndex = 20
        Me.lblProgramName.Text = "WinHSPF"
        '
        'lblAquaTerraURL
        '
        Me.lblAquaTerraURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAquaTerraURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblAquaTerraURL.Location = New System.Drawing.Point(157, 124)
        Me.lblAquaTerraURL.Name = "lblAquaTerraURL"
        Me.lblAquaTerraURL.Size = New System.Drawing.Size(287, 18)
        Me.lblAquaTerraURL.TabIndex = 6
        Me.lblAquaTerraURL.TabStop = True
        Me.lblAquaTerraURL.Text = "http://www.respec.com/service-area/water-environment/"
        Me.lblAquaTerraURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblProgramURL
        '
        Me.lblProgramURL.AutoSize = True
        Me.lblProgramURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblProgramURL.LinkArea = New System.Windows.Forms.LinkArea(0, 83)
        Me.lblProgramURL.Location = New System.Drawing.Point(26, 113)
        Me.lblProgramURL.Name = "lblProgramURL"
        Me.lblProgramURL.Size = New System.Drawing.Size(432, 13)
        Me.lblProgramURL.TabIndex = 21
        Me.lblProgramURL.TabStop = True
        Me.lblProgramURL.Text = "http://www2.epa.gov/exposure-assessment-models/basins-user-information-and-guidan" &
    "ce"
        Me.lblProgramURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PictureBox1
        '
        Me.PictureBox1.ErrorImage = CType(resources.GetObject("PictureBox1.ErrorImage"), System.Drawing.Image)
        Me.PictureBox1.Image = Global.WinHSPF.My.Resources.Resources.BasinsLogo
        Me.PictureBox1.Location = New System.Drawing.Point(26, 13)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(135, 97)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 22
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.WinHSPF.My.Resources.Resources.logo
        Me.PictureBox2.Location = New System.Drawing.Point(12, 28)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(71, 114)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 5
        Me.PictureBox2.TabStop = False
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(486, 527)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblProgramURL)
        Me.Controls.Add(Me.lblProgramName)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.GroupBox1)
        Me.KeyPreview = True
        Me.Name = "frmAbout"
        Me.Text = "About WinHSPF"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtLabel As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblProgramName As Label
    Friend WithEvents lblAquaTerraURL As LinkLabel
    Friend WithEvents lblProgramURL As LinkLabel
    Friend WithEvents PictureBox1 As PictureBox
End Class
