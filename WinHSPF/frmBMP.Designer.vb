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
        Me.cboReach = New System.Windows.Forms.ComboBox
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.fraBMPDet = New System.Windows.Forms.GroupBox
        Me.cmdBMPEffic = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblContributing = New System.Windows.Forms.Label
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdUpdateUCI = New System.Windows.Forms.Button
        Me.agdSource = New atcControls.atcGrid
        Me.atxBMPDesc = New atcControls.atcText
        Me.atxBMPId = New atcControls.atcText
        Me.fraBMPDet.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 18)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(249, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Summary or Reach below BMP:"
        '
        'cboReach
        '
        Me.cboReach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReach.FormattingEnabled = True
        Me.cboReach.Location = New System.Drawing.Point(277, 15)
        Me.cboReach.Margin = New System.Windows.Forms.Padding(4)
        Me.cboReach.Name = "cboReach"
        Me.cboReach.Size = New System.Drawing.Size(425, 24)
        Me.cboReach.TabIndex = 1
        '
        'cmdAdd
        '
        Me.cmdAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdAdd.ImageIndex = 0
        Me.cmdAdd.ImageList = Me.ImageList1
        Me.cmdAdd.Location = New System.Drawing.Point(47, 86)
        Me.cmdAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(125, 27)
        Me.cmdAdd.TabIndex = 3
        Me.cmdAdd.Text = "&Add BMP"
        Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "add.png")
        Me.ImageList1.Images.SetKeyName(1, "remove.png")
        Me.ImageList1.Images.SetKeyName(2, "noteedit.png")
        '
        'cmdDelete
        '
        Me.cmdDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdDelete.ImageIndex = 1
        Me.cmdDelete.ImageList = Me.ImageList1
        Me.cmdDelete.Location = New System.Drawing.Point(47, 121)
        Me.cmdDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(125, 27)
        Me.cmdDelete.TabIndex = 4
        Me.cmdDelete.Text = "&Delete BMP"
        Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDelete.UseVisualStyleBackColor = True
        '
        'fraBMPDet
        '
        Me.fraBMPDet.Controls.Add(Me.cmdBMPEffic)
        Me.fraBMPDet.Controls.Add(Me.atxBMPDesc)
        Me.fraBMPDet.Controls.Add(Me.atxBMPId)
        Me.fraBMPDet.Controls.Add(Me.Label5)
        Me.fraBMPDet.Controls.Add(Me.Label4)
        Me.fraBMPDet.Location = New System.Drawing.Point(191, 63)
        Me.fraBMPDet.Margin = New System.Windows.Forms.Padding(4)
        Me.fraBMPDet.Name = "fraBMPDet"
        Me.fraBMPDet.Padding = New System.Windows.Forms.Padding(4)
        Me.fraBMPDet.Size = New System.Drawing.Size(711, 111)
        Me.fraBMPDet.TabIndex = 5
        Me.fraBMPDet.TabStop = False
        Me.fraBMPDet.Text = "Current BMP Details"
        '
        'cmdBMPEffic
        '
        Me.cmdBMPEffic.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.cmdBMPEffic.ImageIndex = 2
        Me.cmdBMPEffic.ImageList = Me.ImageList1
        Me.cmdBMPEffic.Location = New System.Drawing.Point(201, 26)
        Me.cmdBMPEffic.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdBMPEffic.Name = "cmdBMPEffic"
        Me.cmdBMPEffic.Size = New System.Drawing.Size(193, 27)
        Me.cmdBMPEffic.TabIndex = 9
        Me.cmdBMPEffic.Text = "&Edit Removal Efficiency"
        Me.cmdBMPEffic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdBMPEffic.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 66)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 17)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Description"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(72, 32)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(21, 17)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "ID"
        '
        'lblContributing
        '
        Me.lblContributing.AutoSize = True
        Me.lblContributing.Location = New System.Drawing.Point(12, 197)
        Me.lblContributing.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblContributing.Name = "lblContributing"
        Me.lblContributing.Size = New System.Drawing.Size(180, 17)
        Me.lblContributing.TabIndex = 7
        Me.lblContributing.Text = "Contributing Sources to < >"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(161, 561)
        Me.cmdClose.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(135, 32)
        Me.cmdClose.TabIndex = 24
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdUpdateUCI
        '
        Me.cmdUpdateUCI.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdUpdateUCI.Location = New System.Drawing.Point(16, 561)
        Me.cmdUpdateUCI.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdUpdateUCI.Name = "cmdUpdateUCI"
        Me.cmdUpdateUCI.Size = New System.Drawing.Size(135, 32)
        Me.cmdUpdateUCI.TabIndex = 23
        Me.cmdUpdateUCI.Text = "&Update UCI"
        Me.cmdUpdateUCI.UseVisualStyleBackColor = True
        '
        'agdSource
        '
        Me.agdSource.AllowHorizontalScrolling = True
        Me.agdSource.AllowNewValidValues = False
        Me.agdSource.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdSource.CellBackColor = System.Drawing.Color.Empty
        Me.agdSource.Fixed3D = False
        Me.agdSource.LineColor = System.Drawing.Color.Empty
        Me.agdSource.LineWidth = 0.0!
        Me.agdSource.Location = New System.Drawing.Point(16, 228)
        Me.agdSource.Margin = New System.Windows.Forms.Padding(4)
        Me.agdSource.Name = "agdSource"
        Me.agdSource.Size = New System.Drawing.Size(1004, 305)
        Me.agdSource.Source = Nothing
        Me.agdSource.TabIndex = 6
        '
        'atxBMPDesc
        '
        Me.atxBMPDesc.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxBMPDesc.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.atxBMPDesc.DefaultValue = ""
        Me.atxBMPDesc.HardMax = -999
        Me.atxBMPDesc.HardMin = -999
        Me.atxBMPDesc.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxBMPDesc.Location = New System.Drawing.Point(108, 64)
        Me.atxBMPDesc.Margin = New System.Windows.Forms.Padding(4)
        Me.atxBMPDesc.MaxWidth = 20
        Me.atxBMPDesc.Name = "atxBMPDesc"
        Me.atxBMPDesc.NumericFormat = "0.#####"
        Me.atxBMPDesc.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxBMPDesc.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxBMPDesc.SelLength = 0
        Me.atxBMPDesc.SelStart = 0
        Me.atxBMPDesc.Size = New System.Drawing.Size(405, 39)
        Me.atxBMPDesc.SoftMax = -999
        Me.atxBMPDesc.SoftMin = -999
        Me.atxBMPDesc.TabIndex = 4
        Me.atxBMPDesc.ValueDouble = 0
        Me.atxBMPDesc.ValueInteger = 0
        '
        'atxBMPId
        '
        Me.atxBMPId.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxBMPId.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.atxBMPId.DefaultValue = ""
        Me.atxBMPId.HardMax = -999
        Me.atxBMPId.HardMin = -999
        Me.atxBMPId.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxBMPId.Location = New System.Drawing.Point(108, 30)
        Me.atxBMPId.Margin = New System.Windows.Forms.Padding(4)
        Me.atxBMPId.MaxWidth = 20
        Me.atxBMPId.Name = "atxBMPId"
        Me.atxBMPId.NumericFormat = "0.#####"
        Me.atxBMPId.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxBMPId.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxBMPId.SelLength = 0
        Me.atxBMPId.SelStart = 0
        Me.atxBMPId.Size = New System.Drawing.Size(64, 21)
        Me.atxBMPId.SoftMax = -999
        Me.atxBMPId.SoftMin = -999
        Me.atxBMPId.TabIndex = 3
        Me.atxBMPId.ValueDouble = 0
        Me.atxBMPId.ValueInteger = 0
        '
        'frmBMP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1040, 608)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdUpdateUCI)
        Me.Controls.Add(Me.lblContributing)
        Me.Controls.Add(Me.agdSource)
        Me.Controls.Add(Me.fraBMPDet)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cboReach)
        Me.Controls.Add(Me.Label1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmBMP"
        Me.Text = "WinHSPF - Best Management Practices Editor"
        Me.fraBMPDet.ResumeLayout(False)
        Me.fraBMPDet.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboReach As System.Windows.Forms.ComboBox
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents cmdDelete As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents fraBMPDet As System.Windows.Forms.GroupBox
    Friend WithEvents agdSource As atcControls.atcGrid
    Friend WithEvents lblContributing As System.Windows.Forms.Label
    Friend WithEvents atxBMPDesc As atcControls.atcText
    Friend WithEvents atxBMPId As atcControls.atcText
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmdBMPEffic As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdUpdateUCI As System.Windows.Forms.Button
End Class
