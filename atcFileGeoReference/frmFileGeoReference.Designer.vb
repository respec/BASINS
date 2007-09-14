<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFileGeoReference
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFileGeoReference))
        Me.lblLayer = New System.Windows.Forms.Label
        Me.cboLayer = New System.Windows.Forms.ComboBox
        Me.lblFields = New System.Windows.Forms.Label
        Me.cboFields = New System.Windows.Forms.ComboBox
        Me.lblRecordInfo = New System.Windows.Forms.Label
        Me.lblValue = New System.Windows.Forms.Label
        Me.txtValue = New System.Windows.Forms.TextBox
        Me.btnNext = New System.Windows.Forms.Button
        Me.btnPrev = New System.Windows.Forms.Button
        Me.pbxImage = New System.Windows.Forms.PictureBox
        Me.btnAdd = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.btnRemove = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.pbxImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblLayer
        '
        Me.lblLayer.AutoSize = True
        Me.lblLayer.Location = New System.Drawing.Point(29, 16)
        Me.lblLayer.Name = "lblLayer"
        Me.lblLayer.Size = New System.Drawing.Size(48, 17)
        Me.lblLayer.TabIndex = 1
        Me.lblLayer.Text = "Layer:"
        Me.lblLayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboLayer
        '
        Me.cboLayer.FormattingEnabled = True
        Me.cboLayer.Location = New System.Drawing.Point(83, 16)
        Me.cboLayer.Name = "cboLayer"
        Me.cboLayer.Size = New System.Drawing.Size(170, 24)
        Me.cboLayer.TabIndex = 2
        '
        'lblFields
        '
        Me.lblFields.AutoSize = True
        Me.lblFields.Location = New System.Drawing.Point(270, 16)
        Me.lblFields.Name = "lblFields"
        Me.lblFields.Size = New System.Drawing.Size(94, 17)
        Me.lblFields.TabIndex = 3
        Me.lblFields.Text = "File Ref Field:"
        Me.lblFields.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboFields
        '
        Me.cboFields.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFields.FormattingEnabled = True
        Me.cboFields.Location = New System.Drawing.Point(381, 16)
        Me.cboFields.Name = "cboFields"
        Me.cboFields.Size = New System.Drawing.Size(415, 24)
        Me.cboFields.TabIndex = 4
        '
        'lblRecordInfo
        '
        Me.lblRecordInfo.AutoSize = True
        Me.lblRecordInfo.Location = New System.Drawing.Point(27, 55)
        Me.lblRecordInfo.Name = "lblRecordInfo"
        Me.lblRecordInfo.Size = New System.Drawing.Size(91, 17)
        Me.lblRecordInfo.TabIndex = 5
        Me.lblRecordInfo.Text = "Record x of y"
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.Location = New System.Drawing.Point(316, 55)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(48, 17)
        Me.lblValue.TabIndex = 6
        Me.lblValue.Text = "Value:"
        Me.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtValue
        '
        Me.txtValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtValue.Location = New System.Drawing.Point(381, 55)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(414, 22)
        Me.txtValue.TabIndex = 7
        Me.txtValue.Text = "Value"
        '
        'btnNext
        '
        Me.btnNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNext.Location = New System.Drawing.Point(207, 49)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(27, 31)
        Me.btnNext.TabIndex = 8
        Me.btnNext.Text = ">"
        Me.ToolTip1.SetToolTip(Me.btnNext, "Move Next")
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrev
        '
        Me.btnPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrev.Location = New System.Drawing.Point(174, 49)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(27, 31)
        Me.btnPrev.TabIndex = 9
        Me.btnPrev.Text = "<"
        Me.ToolTip1.SetToolTip(Me.btnPrev, "Move Previous")
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'pbxImage
        '
        Me.pbxImage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbxImage.Location = New System.Drawing.Point(12, 86)
        Me.pbxImage.Name = "pbxImage"
        Me.pbxImage.Size = New System.Drawing.Size(796, 540)
        Me.pbxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbxImage.TabIndex = 10
        Me.pbxImage.TabStop = False
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(240, 49)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(27, 31)
        Me.btnAdd.TabIndex = 11
        Me.btnAdd.Text = "+"
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Add File Reference Point")
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(25, 97)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(63, 20)
        Me.lblStatus.TabIndex = 12
        Me.lblStatus.Text = "Status"
        Me.lblStatus.Visible = False
        '
        'btnRemove
        '
        Me.btnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(273, 49)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(27, 31)
        Me.btnRemove.TabIndex = 13
        Me.btnRemove.Text = "-"
        Me.ToolTip1.SetToolTip(Me.btnRemove, "Remove File Reference Point")
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'frmFileGeoReference
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(820, 652)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.pbxImage)
        Me.Controls.Add(Me.btnPrev)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.txtValue)
        Me.Controls.Add(Me.lblValue)
        Me.Controls.Add(Me.lblRecordInfo)
        Me.Controls.Add(Me.cboFields)
        Me.Controls.Add(Me.lblFields)
        Me.Controls.Add(Me.cboLayer)
        Me.Controls.Add(Me.lblLayer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFileGeoReference"
        Me.Text = "File Geo Reference"
        CType(Me.pbxImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblLayer As System.Windows.Forms.Label
    Friend WithEvents cboLayer As System.Windows.Forms.ComboBox
    Friend WithEvents lblFields As System.Windows.Forms.Label
    Friend WithEvents cboFields As System.Windows.Forms.ComboBox
    Friend WithEvents lblRecordInfo As System.Windows.Forms.Label
    Friend WithEvents lblValue As System.Windows.Forms.Label
    Friend WithEvents txtValue As System.Windows.Forms.TextBox
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents pbxImage As System.Windows.Forms.PictureBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnRemove As System.Windows.Forms.Button
End Class
