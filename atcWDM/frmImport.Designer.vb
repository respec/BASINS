<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImport
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
        Me.lblScript = New System.Windows.Forms.Label
        Me.txtScript = New System.Windows.Forms.TextBox
        Me.txtDataFiles = New System.Windows.Forms.TextBox
        Me.lblDataFiles = New System.Windows.Forms.Label
        Me.btnBrowseFiles = New System.Windows.Forms.Button
        Me.txtSaveIn = New System.Windows.Forms.TextBox
        Me.grpSaveIn = New System.Windows.Forms.GroupBox
        Me.lblSaveFolder = New System.Windows.Forms.Label
        Me.txtSaveFolder = New System.Windows.Forms.TextBox
        Me.btnBrowseSaveIn = New System.Windows.Forms.Button
        Me.radioWDMone = New System.Windows.Forms.RadioButton
        Me.radioWDMeach = New System.Windows.Forms.RadioButton
        Me.btnImport = New System.Windows.Forms.Button
        Me.btnBrowseScript = New System.Windows.Forms.Button
        Me.grpSaveIn.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblScript
        '
        Me.lblScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblScript.AutoSize = True
        Me.lblScript.Location = New System.Drawing.Point(12, 261)
        Me.lblScript.Name = "lblScript"
        Me.lblScript.Size = New System.Drawing.Size(99, 13)
        Me.lblScript.TabIndex = 3
        Me.lblScript.Text = "Import Using Script:"
        '
        'txtScript
        '
        Me.txtScript.AllowDrop = True
        Me.txtScript.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtScript.Location = New System.Drawing.Point(117, 258)
        Me.txtScript.Name = "txtScript"
        Me.txtScript.Size = New System.Drawing.Size(449, 20)
        Me.txtScript.TabIndex = 4
        '
        'txtDataFiles
        '
        Me.txtDataFiles.AllowDrop = True
        Me.txtDataFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDataFiles.Location = New System.Drawing.Point(12, 39)
        Me.txtDataFiles.Multiline = True
        Me.txtDataFiles.Name = "txtDataFiles"
        Me.txtDataFiles.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtDataFiles.Size = New System.Drawing.Size(635, 211)
        Me.txtDataFiles.TabIndex = 2
        '
        'lblDataFiles
        '
        Me.lblDataFiles.AutoSize = True
        Me.lblDataFiles.Location = New System.Drawing.Point(12, 15)
        Me.lblDataFiles.Name = "lblDataFiles"
        Me.lblDataFiles.Size = New System.Drawing.Size(275, 13)
        Me.lblDataFiles.TabIndex = 0
        Me.lblDataFiles.Text = "Text File(s) to import. Drag files into box below or Browse:"
        '
        'btnBrowseFiles
        '
        Me.btnBrowseFiles.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseFiles.Location = New System.Drawing.Point(572, 10)
        Me.btnBrowseFiles.Name = "btnBrowseFiles"
        Me.btnBrowseFiles.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseFiles.TabIndex = 1
        Me.btnBrowseFiles.Text = "Browse"
        Me.btnBrowseFiles.UseVisualStyleBackColor = True
        '
        'txtSaveIn
        '
        Me.txtSaveIn.AllowDrop = True
        Me.txtSaveIn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSaveIn.Location = New System.Drawing.Point(188, 41)
        Me.txtSaveIn.Name = "txtSaveIn"
        Me.txtSaveIn.Size = New System.Drawing.Size(366, 20)
        Me.txtSaveIn.TabIndex = 11
        '
        'grpSaveIn
        '
        Me.grpSaveIn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSaveIn.Controls.Add(Me.lblSaveFolder)
        Me.grpSaveIn.Controls.Add(Me.txtSaveFolder)
        Me.grpSaveIn.Controls.Add(Me.btnBrowseSaveIn)
        Me.grpSaveIn.Controls.Add(Me.radioWDMone)
        Me.grpSaveIn.Controls.Add(Me.radioWDMeach)
        Me.grpSaveIn.Controls.Add(Me.txtSaveIn)
        Me.grpSaveIn.Location = New System.Drawing.Point(12, 285)
        Me.grpSaveIn.Name = "grpSaveIn"
        Me.grpSaveIn.Size = New System.Drawing.Size(635, 68)
        Me.grpSaveIn.TabIndex = 6
        Me.grpSaveIn.TabStop = False
        Me.grpSaveIn.Text = "Save In"
        '
        'lblSaveFolder
        '
        Me.lblSaveFolder.AutoSize = True
        Me.lblSaveFolder.Location = New System.Drawing.Point(187, 21)
        Me.lblSaveFolder.Name = "lblSaveFolder"
        Me.lblSaveFolder.Size = New System.Drawing.Size(140, 13)
        Me.lblSaveFolder.TabIndex = 8
        Me.lblSaveFolder.Text = "Folder to save WDM files in:"
        '
        'txtSaveFolder
        '
        Me.txtSaveFolder.AllowDrop = True
        Me.txtSaveFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSaveFolder.Location = New System.Drawing.Point(333, 18)
        Me.txtSaveFolder.Name = "txtSaveFolder"
        Me.txtSaveFolder.Size = New System.Drawing.Size(221, 20)
        Me.txtSaveFolder.TabIndex = 9
        '
        'btnBrowseSaveIn
        '
        Me.btnBrowseSaveIn.AllowDrop = True
        Me.btnBrowseSaveIn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseSaveIn.Location = New System.Drawing.Point(560, 40)
        Me.btnBrowseSaveIn.Name = "btnBrowseSaveIn"
        Me.btnBrowseSaveIn.Size = New System.Drawing.Size(69, 20)
        Me.btnBrowseSaveIn.TabIndex = 12
        Me.btnBrowseSaveIn.Text = "Browse"
        Me.btnBrowseSaveIn.UseVisualStyleBackColor = True
        '
        'radioWDMone
        '
        Me.radioWDMone.AutoSize = True
        Me.radioWDMone.Location = New System.Drawing.Point(6, 42)
        Me.radioWDMone.Name = "radioWDMone"
        Me.radioWDMone.Size = New System.Drawing.Size(118, 17)
        Me.radioWDMone.TabIndex = 10
        Me.radioWDMone.Text = "All in one WDM file:"
        Me.radioWDMone.UseVisualStyleBackColor = True
        '
        'radioWDMeach
        '
        Me.radioWDMeach.AutoSize = True
        Me.radioWDMeach.Checked = True
        Me.radioWDMeach.Location = New System.Drawing.Point(6, 19)
        Me.radioWDMeach.Name = "radioWDMeach"
        Me.radioWDMeach.Size = New System.Drawing.Size(175, 17)
        Me.radioWDMeach.TabIndex = 7
        Me.radioWDMeach.TabStop = True
        Me.radioWDMeach.Text = "WDM named after each text file"
        Me.radioWDMeach.UseVisualStyleBackColor = True
        '
        'btnImport
        '
        Me.btnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnImport.Location = New System.Drawing.Point(12, 359)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(75, 23)
        Me.btnImport.TabIndex = 99
        Me.btnImport.Text = "Import"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'btnBrowseScript
        '
        Me.btnBrowseScript.AllowDrop = True
        Me.btnBrowseScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseScript.Location = New System.Drawing.Point(572, 256)
        Me.btnBrowseScript.Name = "btnBrowseScript"
        Me.btnBrowseScript.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseScript.TabIndex = 5
        Me.btnBrowseScript.Text = "Select"
        Me.btnBrowseScript.UseVisualStyleBackColor = True
        '
        'frmImport
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(659, 394)
        Me.Controls.Add(Me.btnBrowseScript)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.btnBrowseFiles)
        Me.Controls.Add(Me.txtDataFiles)
        Me.Controls.Add(Me.lblDataFiles)
        Me.Controls.Add(Me.txtScript)
        Me.Controls.Add(Me.lblScript)
        Me.Controls.Add(Me.grpSaveIn)
        Me.Name = "frmImport"
        Me.Text = "Import To WDM"
        Me.grpSaveIn.ResumeLayout(False)
        Me.grpSaveIn.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblScript As System.Windows.Forms.Label
    Friend WithEvents txtScript As System.Windows.Forms.TextBox
    Friend WithEvents txtDataFiles As System.Windows.Forms.TextBox
    Friend WithEvents lblDataFiles As System.Windows.Forms.Label
    Friend WithEvents btnBrowseFiles As System.Windows.Forms.Button
    Friend WithEvents txtSaveIn As System.Windows.Forms.TextBox
    Friend WithEvents grpSaveIn As System.Windows.Forms.GroupBox
    Friend WithEvents radioWDMone As System.Windows.Forms.RadioButton
    Friend WithEvents radioWDMeach As System.Windows.Forms.RadioButton
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnBrowseScript As System.Windows.Forms.Button
    Friend WithEvents btnBrowseSaveIn As System.Windows.Forms.Button
    Friend WithEvents lblSaveFolder As System.Windows.Forms.Label
    Friend WithEvents txtSaveFolder As System.Windows.Forms.TextBox
End Class
