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
        Me.pbxImage = New System.Windows.Forms.PictureBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnPrev = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.grpDocument = New System.Windows.Forms.GroupBox
        Me.txtDate = New System.Windows.Forms.TextBox
        Me.lblDate = New System.Windows.Forms.Label
        Me.txtLocation = New System.Windows.Forms.TextBox
        Me.lblLocation = New System.Windows.Forms.Label
        Me.lblRecordInfo = New System.Windows.Forms.Label
        Me.cboAnnotate = New System.Windows.Forms.ComboBox
        Me.lblAnnotate = New System.Windows.Forms.Label
        Me.txtAnnotation = New System.Windows.Forms.TextBox
        CType(Me.pbxImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDocument.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblLayer
        '
        Me.lblLayer.AutoSize = True
        Me.lblLayer.Location = New System.Drawing.Point(15, 17)
        Me.lblLayer.Name = "lblLayer"
        Me.lblLayer.Size = New System.Drawing.Size(48, 17)
        Me.lblLayer.TabIndex = 1
        Me.lblLayer.Text = "Layer:"
        Me.lblLayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboLayer
        '
        Me.cboLayer.DropDownHeight = 300
        Me.cboLayer.FormattingEnabled = True
        Me.cboLayer.IntegralHeight = False
        Me.cboLayer.Location = New System.Drawing.Point(68, 14)
        Me.cboLayer.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cboLayer.Name = "cboLayer"
        Me.cboLayer.Size = New System.Drawing.Size(197, 24)
        Me.cboLayer.TabIndex = 2
        '
        'lblFields
        '
        Me.lblFields.AutoSize = True
        Me.lblFields.Location = New System.Drawing.Point(15, 49)
        Me.lblFields.Name = "lblFields"
        Me.lblFields.Size = New System.Drawing.Size(110, 17)
        Me.lblFields.TabIndex = 3
        Me.lblFields.Text = "Document Field:"
        Me.lblFields.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboFields
        '
        Me.cboFields.DropDownHeight = 300
        Me.cboFields.FormattingEnabled = True
        Me.cboFields.IntegralHeight = False
        Me.cboFields.Location = New System.Drawing.Point(132, 45)
        Me.cboFields.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cboFields.Name = "cboFields"
        Me.cboFields.Size = New System.Drawing.Size(133, 24)
        Me.cboFields.TabIndex = 4
        '
        'pbxImage
        '
        Me.pbxImage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbxImage.Location = New System.Drawing.Point(12, 121)
        Me.pbxImage.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pbxImage.Name = "pbxImage"
        Me.pbxImage.Size = New System.Drawing.Size(795, 363)
        Me.pbxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbxImage.TabIndex = 10
        Me.pbxImage.TabStop = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(15, 97)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(63, 20)
        Me.lblStatus.TabIndex = 12
        Me.lblStatus.Text = "Status"
        Me.lblStatus.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(15, 133)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(144, 31)
        Me.btnCancel.TabIndex = 14
        Me.btnCancel.Text = "Cancel Add"
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Remove File Reference Point")
        Me.btnCancel.UseVisualStyleBackColor = True
        Me.btnCancel.Visible = False
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(381, 55)
        Me.btnRemove.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(27, 31)
        Me.btnRemove.TabIndex = 23
        Me.btnRemove.Text = "-"
        Me.ToolTip1.SetToolTip(Me.btnRemove, "Remove Document")
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(348, 55)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(27, 31)
        Me.btnAdd.TabIndex = 22
        Me.btnAdd.Text = "+"
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Add Document")
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnPrev
        '
        Me.btnPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrev.Location = New System.Drawing.Point(281, 55)
        Me.btnPrev.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(27, 31)
        Me.btnPrev.TabIndex = 21
        Me.btnPrev.Text = "<"
        Me.ToolTip1.SetToolTip(Me.btnPrev, "Previous Record")
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNext.Location = New System.Drawing.Point(315, 55)
        Me.btnNext.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(27, 31)
        Me.btnNext.TabIndex = 20
        Me.btnNext.Text = ">"
        Me.ToolTip1.SetToolTip(Me.btnNext, "Next Record")
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'grpDocument
        '
        Me.grpDocument.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDocument.Controls.Add(Me.txtDate)
        Me.grpDocument.Controls.Add(Me.lblDate)
        Me.grpDocument.Controls.Add(Me.btnRemove)
        Me.grpDocument.Controls.Add(Me.btnAdd)
        Me.grpDocument.Controls.Add(Me.btnPrev)
        Me.grpDocument.Controls.Add(Me.btnNext)
        Me.grpDocument.Controls.Add(Me.txtLocation)
        Me.grpDocument.Controls.Add(Me.lblLocation)
        Me.grpDocument.Controls.Add(Me.lblRecordInfo)
        Me.grpDocument.Location = New System.Drawing.Point(273, 15)
        Me.grpDocument.Margin = New System.Windows.Forms.Padding(4)
        Me.grpDocument.Name = "grpDocument"
        Me.grpDocument.Padding = New System.Windows.Forms.Padding(4)
        Me.grpDocument.Size = New System.Drawing.Size(529, 101)
        Me.grpDocument.TabIndex = 17
        Me.grpDocument.TabStop = False
        Me.grpDocument.Text = "Current Document"
        '
        'txtDate
        '
        Me.txtDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDate.Location = New System.Drawing.Point(80, 55)
        Me.txtDate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtDate.Name = "txtDate"
        Me.txtDate.Size = New System.Drawing.Size(195, 22)
        Me.txtDate.TabIndex = 25
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Location = New System.Drawing.Point(31, 59)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(42, 17)
        Me.lblDate.TabIndex = 24
        Me.lblDate.Text = "Date:"
        Me.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtLocation
        '
        Me.txtLocation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLocation.Location = New System.Drawing.Point(80, 26)
        Me.txtLocation.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtLocation.Name = "txtLocation"
        Me.txtLocation.Size = New System.Drawing.Size(441, 22)
        Me.txtLocation.TabIndex = 19
        '
        'lblLocation
        '
        Me.lblLocation.AutoSize = True
        Me.lblLocation.Location = New System.Drawing.Point(7, 30)
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.Size = New System.Drawing.Size(66, 17)
        Me.lblLocation.TabIndex = 18
        Me.lblLocation.Text = "Location:"
        Me.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRecordInfo
        '
        Me.lblRecordInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRecordInfo.AutoSize = True
        Me.lblRecordInfo.Location = New System.Drawing.Point(413, 64)
        Me.lblRecordInfo.Name = "lblRecordInfo"
        Me.lblRecordInfo.Size = New System.Drawing.Size(53, 17)
        Me.lblRecordInfo.TabIndex = 17
        Me.lblRecordInfo.Text = "# x of y"
        '
        'cboAnnotate
        '
        Me.cboAnnotate.DropDownHeight = 300
        Me.cboAnnotate.FormattingEnabled = True
        Me.cboAnnotate.IntegralHeight = False
        Me.cboAnnotate.Location = New System.Drawing.Point(132, 75)
        Me.cboAnnotate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cboAnnotate.Name = "cboAnnotate"
        Me.cboAnnotate.Size = New System.Drawing.Size(133, 24)
        Me.cboAnnotate.TabIndex = 19
        '
        'lblAnnotate
        '
        Me.lblAnnotate.AutoSize = True
        Me.lblAnnotate.Location = New System.Drawing.Point(11, 79)
        Me.lblAnnotate.Name = "lblAnnotate"
        Me.lblAnnotate.Size = New System.Drawing.Size(114, 17)
        Me.lblAnnotate.TabIndex = 18
        Me.lblAnnotate.Text = "Annotation Field:"
        Me.lblAnnotate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAnnotation
        '
        Me.txtAnnotation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAnnotation.BackColor = System.Drawing.SystemColors.Window
        Me.txtAnnotation.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtAnnotation.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAnnotation.Location = New System.Drawing.Point(16, 489)
        Me.txtAnnotation.Multiline = True
        Me.txtAnnotation.Name = "txtAnnotation"
        Me.txtAnnotation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtAnnotation.Size = New System.Drawing.Size(790, 22)
        Me.txtAnnotation.TabIndex = 20
        Me.txtAnnotation.Text = "Annotation"
        Me.txtAnnotation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'frmFileGeoReference
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(819, 510)
        Me.Controls.Add(Me.txtAnnotation)
        Me.Controls.Add(Me.cboAnnotate)
        Me.Controls.Add(Me.lblAnnotate)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.pbxImage)
        Me.Controls.Add(Me.cboFields)
        Me.Controls.Add(Me.lblFields)
        Me.Controls.Add(Me.cboLayer)
        Me.Controls.Add(Me.lblLayer)
        Me.Controls.Add(Me.grpDocument)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmFileGeoReference"
        Me.Text = "File Geo Reference"
        CType(Me.pbxImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDocument.ResumeLayout(False)
        Me.grpDocument.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblLayer As System.Windows.Forms.Label
    Friend WithEvents cboLayer As System.Windows.Forms.ComboBox
    Friend WithEvents lblFields As System.Windows.Forms.Label
    Friend WithEvents cboFields As System.Windows.Forms.ComboBox
    Friend WithEvents pbxImage As System.Windows.Forms.PictureBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpDocument As System.Windows.Forms.GroupBox
    Friend WithEvents txtDate As System.Windows.Forms.TextBox
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents txtLocation As System.Windows.Forms.TextBox
    Friend WithEvents lblLocation As System.Windows.Forms.Label
    Friend WithEvents lblRecordInfo As System.Windows.Forms.Label
    Friend WithEvents cboAnnotate As System.Windows.Forms.ComboBox
    Friend WithEvents lblAnnotate As System.Windows.Forms.Label
    Friend WithEvents txtAnnotation As System.Windows.Forms.TextBox
End Class
