<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSaveAs
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSaveAs))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.rbnRelative = New System.Windows.Forms.RadioButton
        Me.rbnAbsolute = New System.Windows.Forms.RadioButton
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.atxName = New atcControls.atcText
        Me.cmdFile = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.atxBase = New atcControls.atcText
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 15)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(177, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "New Project (8 chars max):"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 58)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Path:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 95)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(198, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Number Data Sets As Follows:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(32, 132)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Base DSN:"
        '
        'rbnRelative
        '
        Me.rbnRelative.AutoSize = True
        Me.rbnRelative.Location = New System.Drawing.Point(304, 118)
        Me.rbnRelative.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnRelative.Name = "rbnRelative"
        Me.rbnRelative.Size = New System.Drawing.Size(77, 21)
        Me.rbnRelative.TabIndex = 5
        Me.rbnRelative.Text = "Relative"
        Me.rbnRelative.UseVisualStyleBackColor = True
        '
        'rbnAbsolute
        '
        Me.rbnAbsolute.AutoSize = True
        Me.rbnAbsolute.Checked = True
        Me.rbnAbsolute.Location = New System.Drawing.Point(304, 142)
        Me.rbnAbsolute.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnAbsolute.Name = "rbnAbsolute"
        Me.rbnAbsolute.Size = New System.Drawing.Size(81, 21)
        Me.rbnAbsolute.TabIndex = 6
        Me.rbnAbsolute.TabStop = True
        Me.rbnAbsolute.Text = "Absolute"
        Me.rbnAbsolute.UseVisualStyleBackColor = True
        '
        'txtPath
        '
        Me.txtPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPath.Location = New System.Drawing.Point(69, 50)
        Me.txtPath.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.ReadOnly = True
        Me.txtPath.Size = New System.Drawing.Size(407, 22)
        Me.txtPath.TabIndex = 7
        '
        'atxName
        '
        Me.atxName.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atxName.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.atxName.DefaultValue = " "
        Me.atxName.HardMax = -999
        Me.atxName.HardMin = -999
        Me.atxName.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxName.Location = New System.Drawing.Point(203, 11)
        Me.atxName.Margin = New System.Windows.Forms.Padding(4)
        Me.atxName.MaxWidth = 12
        Me.atxName.Name = "atxName"
        Me.atxName.NumericFormat = "0.#####"
        Me.atxName.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxName.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxName.SelLength = 1
        Me.atxName.SelStart = 0
        Me.atxName.Size = New System.Drawing.Size(119, 33)
        Me.atxName.SoftMax = -999
        Me.atxName.SoftMin = -999
        Me.atxName.TabIndex = 8
        Me.atxName.ValueDouble = 0
        Me.atxName.ValueInteger = 0
        '
        'cmdFile
        '
        Me.cmdFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFile.ImageIndex = 0
        Me.cmdFile.ImageList = Me.ImageList1
        Me.cmdFile.Location = New System.Drawing.Point(384, 6)
        Me.cmdFile.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdFile.Name = "cmdFile"
        Me.cmdFile.Size = New System.Drawing.Size(93, 32)
        Me.cmdFile.TabIndex = 9
        Me.cmdFile.Text = "Browse"
        Me.cmdFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdFile.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "open.png")
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(257, 177)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(135, 32)
        Me.cmdCancel.TabIndex = 22
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(112, 177)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(135, 32)
        Me.cmdOK.TabIndex = 21
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'atxBase
        '
        Me.atxBase.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxBase.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxBase.DefaultValue = " 1000"
        Me.atxBase.HardMax = 32000
        Me.atxBase.HardMin = 1
        Me.atxBase.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxBase.Location = New System.Drawing.Point(117, 130)
        Me.atxBase.Margin = New System.Windows.Forms.Padding(4)
        Me.atxBase.MaxWidth = 12
        Me.atxBase.Name = "atxBase"
        Me.atxBase.NumericFormat = "0.#####"
        Me.atxBase.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxBase.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxBase.SelLength = 4
        Me.atxBase.SelStart = 0
        Me.atxBase.Size = New System.Drawing.Size(119, 33)
        Me.atxBase.SoftMax = -999
        Me.atxBase.SoftMin = -999
        Me.atxBase.TabIndex = 23
        Me.atxBase.ValueDouble = 1000
        Me.atxBase.ValueInteger = 1000
        '
        'frmSaveAs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(495, 224)
        Me.Controls.Add(Me.atxBase)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdFile)
        Me.Controls.Add(Me.atxName)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.rbnAbsolute)
        Me.Controls.Add(Me.rbnRelative)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmSaveAs"
        Me.Text = "WinHSPF - Save As"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rbnRelative As System.Windows.Forms.RadioButton
    Friend WithEvents rbnAbsolute As System.Windows.Forms.RadioButton
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents atxName As atcControls.atcText
    Friend WithEvents cmdFile As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents atxBase As atcControls.atcText
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
End Class
