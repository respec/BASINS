<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAQUATOX
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAQUATOX))
        Me.cmdWatershedFile = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdWDMFile = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cboRchres = New System.Windows.Forms.ComboBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdAQUATOX = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.lblWatershedFile = New System.Windows.Forms.Label
        Me.lblWDMFile = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdWatershedFile
        '
        Me.cmdWatershedFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdWatershedFile.ImageIndex = 0
        Me.cmdWatershedFile.ImageList = Me.ImageList1
        Me.cmdWatershedFile.Location = New System.Drawing.Point(187, 12)
        Me.cmdWatershedFile.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdWatershedFile.Name = "cmdWatershedFile"
        Me.cmdWatershedFile.Size = New System.Drawing.Size(84, 23)
        Me.cmdWatershedFile.TabIndex = 3
        Me.cmdWatershedFile.Text = "Select"
        Me.cmdWatershedFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdWatershedFile.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "toolbar-openicon.png")
        '
        'cmdWDMFile
        '
        Me.cmdWDMFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdWDMFile.ImageIndex = 0
        Me.cmdWDMFile.ImageList = Me.ImageList1
        Me.cmdWDMFile.Location = New System.Drawing.Point(187, 49)
        Me.cmdWDMFile.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdWDMFile.Name = "cmdWDMFile"
        Me.cmdWDMFile.Size = New System.Drawing.Size(84, 23)
        Me.cmdWDMFile.TabIndex = 5
        Me.cmdWDMFile.Text = "Select"
        Me.cmdWDMFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdWDMFile.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 18)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(156, 17)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "BASINS Watershed File"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(17, 55)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(116, 17)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Project WDM File"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 100)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(158, 17)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Select RCHRES to Link:"
        '
        'cboRchres
        '
        Me.cboRchres.AllowDrop = True
        Me.cboRchres.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRchres.FormattingEnabled = True
        Me.cboRchres.Location = New System.Drawing.Point(272, 96)
        Me.cboRchres.Margin = New System.Windows.Forms.Padding(4)
        Me.cboRchres.Name = "cboRchres"
        Me.cboRchres.Size = New System.Drawing.Size(216, 24)
        Me.cboRchres.TabIndex = 9
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(391, 145)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(135, 32)
        Me.cmdCancel.TabIndex = 22
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdAQUATOX
        '
        Me.cmdAQUATOX.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdAQUATOX.Location = New System.Drawing.Point(185, 145)
        Me.cmdAQUATOX.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdAQUATOX.Name = "cmdAQUATOX"
        Me.cmdAQUATOX.Size = New System.Drawing.Size(195, 32)
        Me.cmdAQUATOX.TabIndex = 21
        Me.cmdAQUATOX.Text = "&Start AQUATOX"
        Me.cmdAQUATOX.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'lblWatershedFile
        '
        Me.lblWatershedFile.AutoSize = True
        Me.lblWatershedFile.Location = New System.Drawing.Point(278, 15)
        Me.lblWatershedFile.Name = "lblWatershedFile"
        Me.lblWatershedFile.Size = New System.Drawing.Size(56, 17)
        Me.lblWatershedFile.TabIndex = 23
        Me.lblWatershedFile.Text = "<none>"
        '
        'lblWDMFile
        '
        Me.lblWDMFile.AutoSize = True
        Me.lblWDMFile.Location = New System.Drawing.Point(278, 52)
        Me.lblWDMFile.Name = "lblWDMFile"
        Me.lblWDMFile.Size = New System.Drawing.Size(56, 17)
        Me.lblWDMFile.TabIndex = 24
        Me.lblWDMFile.Text = "<none>"
        '
        'frmAQUATOX
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(699, 192)
        Me.Controls.Add(Me.lblWDMFile)
        Me.Controls.Add(Me.lblWatershedFile)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdAQUATOX)
        Me.Controls.Add(Me.cboRchres)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdWDMFile)
        Me.Controls.Add(Me.cmdWatershedFile)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmAQUATOX"
        Me.Text = "WinHSPF - AQUATOX Linkage"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdWatershedFile As System.Windows.Forms.Button
    Friend WithEvents cmdWDMFile As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboRchres As System.Windows.Forms.ComboBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdAQUATOX As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblWatershedFile As System.Windows.Forms.Label
    Friend WithEvents lblWDMFile As System.Windows.Forms.Label
End Class
