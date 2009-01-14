<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImportPoint
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImportPoint))
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.lblFile = New System.Windows.Forms.TextBox
        Me.cmdFile = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cboFac = New System.Windows.Forms.ComboBox
        Me.cboReach = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.txtScen = New atcControls.atcText
        Me.SuspendLayout()
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'lblFile
        '
        Me.lblFile.BackColor = System.Drawing.SystemColors.Control
        Me.lblFile.Location = New System.Drawing.Point(94, 14)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(259, 20)
        Me.lblFile.TabIndex = 0
        '
        'cmdFile
        '
        Me.cmdFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFile.ImageIndex = 0
        Me.cmdFile.ImageList = Me.ImageList1
        Me.cmdFile.Location = New System.Drawing.Point(12, 12)
        Me.cmdFile.Name = "cmdFile"
        Me.cmdFile.Size = New System.Drawing.Size(76, 22)
        Me.cmdFile.TabIndex = 1
        Me.cmdFile.Text = "Open File"
        Me.cmdFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdFile.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "toolbar-openicon.png")
        '
        'cboFac
        '
        Me.cboFac.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFac.FormattingEnabled = True
        Me.cboFac.Location = New System.Drawing.Point(94, 105)
        Me.cboFac.Name = "cboFac"
        Me.cboFac.Size = New System.Drawing.Size(259, 21)
        Me.cboFac.TabIndex = 18
        '
        'cboReach
        '
        Me.cboReach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReach.FormattingEnabled = True
        Me.cboReach.Location = New System.Drawing.Point(94, 77)
        Me.cboReach.Name = "cboReach"
        Me.cboReach.Size = New System.Drawing.Size(259, 21)
        Me.cboReach.TabIndex = 16
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(92, 55)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 13)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "PT-"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 109)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Facility"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(23, 81)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 13)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Reach"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(23, 55)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(49, 13)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Scenario"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(192, 160)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(101, 26)
        Me.cmdCancel.TabIndex = 22
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(83, 160)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(101, 26)
        Me.cmdOK.TabIndex = 21
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'txtScen
        '
        Me.txtScen.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtScen.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.txtScen.DefaultValue = ""
        Me.txtScen.HardMax = -999
        Me.txtScen.HardMin = -999
        Me.txtScen.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtScen.Location = New System.Drawing.Point(116, 51)
        Me.txtScen.MaxWidth = 20
        Me.txtScen.Name = "txtScen"
        Me.txtScen.NumericFormat = "0.#####"
        Me.txtScen.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtScen.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtScen.SelLength = 1
        Me.txtScen.SelStart = 0
        Me.txtScen.Size = New System.Drawing.Size(73, 20)
        Me.txtScen.SoftMax = -999
        Me.txtScen.SoftMin = -999
        Me.txtScen.TabIndex = 14
        Me.txtScen.ValueDouble = 0
        Me.txtScen.ValueInteger = 0
        '
        'frmImportPoint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(377, 200)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cboFac)
        Me.Controls.Add(Me.cboReach)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtScen)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cmdFile)
        Me.Controls.Add(Me.lblFile)
        Me.Name = "frmImportPoint"
        Me.Text = "WinHSPF - Import Point Source"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblFile As System.Windows.Forms.TextBox
    Friend WithEvents cmdFile As System.Windows.Forms.Button
    Friend WithEvents cboFac As System.Windows.Forms.ComboBox
    Friend WithEvents cboReach As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtScen As atcControls.atcText
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
End Class
