<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBMP
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBMP))
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.cmdFile = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Button1 = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmdBMPEffic = New System.Windows.Forms.Button
        Me.AtcText2 = New atcControls.atcText
        Me.AtcText1 = New atcControls.atcText
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.AtcGrid1 = New atcControls.atcGrid
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdUpdateUCI = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(190, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Summary or Reach below BMP:"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(208, 12)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(320, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'cmdFile
        '
        Me.cmdFile.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdFile.ImageIndex = 0
        Me.cmdFile.ImageList = Me.ImageList1
        Me.cmdFile.Location = New System.Drawing.Point(35, 70)
        Me.cmdFile.Name = "cmdFile"
        Me.cmdFile.Size = New System.Drawing.Size(94, 22)
        Me.cmdFile.TabIndex = 3
        Me.cmdFile.Text = "&Add BMP"
        Me.cmdFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFile.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "add.png")
        Me.ImageList1.Images.SetKeyName(1, "remove.png")
        Me.ImageList1.Images.SetKeyName(2, "noteedit.png")
        '
        'Button1
        '
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.ImageIndex = 1
        Me.Button1.ImageList = Me.ImageList1
        Me.Button1.Location = New System.Drawing.Point(35, 98)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(94, 22)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "&Delete BMP"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmdBMPEffic)
        Me.GroupBox1.Controls.Add(Me.AtcText2)
        Me.GroupBox1.Controls.Add(Me.AtcText1)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Location = New System.Drawing.Point(143, 51)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(533, 90)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Current BMP Details"
        '
        'cmdBMPEffic
        '
        Me.cmdBMPEffic.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.cmdBMPEffic.ImageIndex = 2
        Me.cmdBMPEffic.ImageList = Me.ImageList1
        Me.cmdBMPEffic.Location = New System.Drawing.Point(151, 21)
        Me.cmdBMPEffic.Name = "cmdBMPEffic"
        Me.cmdBMPEffic.Size = New System.Drawing.Size(145, 22)
        Me.cmdBMPEffic.TabIndex = 9
        Me.cmdBMPEffic.Text = "&Edit Removal Efficiency"
        Me.cmdBMPEffic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdBMPEffic.UseVisualStyleBackColor = True
        '
        'AtcText2
        '
        Me.AtcText2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText2.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText2.DefaultValue = ""
        Me.AtcText2.HardMax = -999
        Me.AtcText2.HardMin = -999
        Me.AtcText2.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText2.Location = New System.Drawing.Point(81, 52)
        Me.AtcText2.MaxWidth = 20
        Me.AtcText2.Name = "AtcText2"
        Me.AtcText2.NumericFormat = "0.#####"
        Me.AtcText2.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText2.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText2.SelLength = 1
        Me.AtcText2.SelStart = 0
        Me.AtcText2.Size = New System.Drawing.Size(304, 32)
        Me.AtcText2.SoftMax = -999
        Me.AtcText2.SoftMin = -999
        Me.AtcText2.TabIndex = 4
        Me.AtcText2.ValueDouble = 0
        Me.AtcText2.ValueInteger = 0
        '
        'AtcText1
        '
        Me.AtcText1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText1.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText1.DefaultValue = ""
        Me.AtcText1.HardMax = -999
        Me.AtcText1.HardMin = -999
        Me.AtcText1.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText1.Location = New System.Drawing.Point(81, 24)
        Me.AtcText1.MaxWidth = 20
        Me.AtcText1.Name = "AtcText1"
        Me.AtcText1.NumericFormat = "0.#####"
        Me.AtcText1.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText1.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText1.SelLength = 1
        Me.AtcText1.SelStart = 0
        Me.AtcText1.Size = New System.Drawing.Size(48, 17)
        Me.AtcText1.SoftMax = -999
        Me.AtcText1.SoftMin = -999
        Me.AtcText1.TabIndex = 3
        Me.AtcText1.ValueDouble = 0
        Me.AtcText1.ValueInteger = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Description"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(54, 26)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(18, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "ID"
        '
        'AtcGrid1
        '
        Me.AtcGrid1.AllowHorizontalScrolling = True
        Me.AtcGrid1.AllowNewValidValues = False
        Me.AtcGrid1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGrid1.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGrid1.Fixed3D = False
        Me.AtcGrid1.LineColor = System.Drawing.Color.Empty
        Me.AtcGrid1.LineWidth = 0.0!
        Me.AtcGrid1.Location = New System.Drawing.Point(12, 185)
        Me.AtcGrid1.Name = "AtcGrid1"
        Me.AtcGrid1.Size = New System.Drawing.Size(753, 249)
        Me.AtcGrid1.Source = Nothing
        Me.AtcGrid1.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 160)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(135, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Contributing Sources to < >"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(121, 457)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(101, 26)
        Me.cmdClose.TabIndex = 24
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdUpdateUCI
        '
        Me.cmdUpdateUCI.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdUpdateUCI.Location = New System.Drawing.Point(12, 457)
        Me.cmdUpdateUCI.Name = "cmdUpdateUCI"
        Me.cmdUpdateUCI.Size = New System.Drawing.Size(101, 26)
        Me.cmdUpdateUCI.TabIndex = 23
        Me.cmdUpdateUCI.Text = "&Update UCI"
        Me.cmdUpdateUCI.UseVisualStyleBackColor = True
        '
        'frmBMP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(780, 495)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdUpdateUCI)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.AtcGrid1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.cmdFile)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmBMP"
        Me.Text = "WinHSPF - Best Management Practices Editor"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents cmdFile As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents AtcGrid1 As atcControls.atcGrid
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents AtcText2 As atcControls.atcText
    Friend WithEvents AtcText1 As atcControls.atcText
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmdBMPEffic As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdUpdateUCI As System.Windows.Forms.Button
End Class
