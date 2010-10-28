<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWDM
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
        Me.btnOk = New System.Windows.Forms.Button
        Me.RadioIndividual = New System.Windows.Forms.RadioButton
        Me.lblMessage = New System.Windows.Forms.Label
        Me.RadioAddNew = New System.Windows.Forms.RadioButton
        Me.RadioDontAdd = New System.Windows.Forms.RadioButton
        Me.txtFilenameNew = New System.Windows.Forms.TextBox
        Me.btnBrowseNew = New System.Windows.Forms.Button
        Me.btnBrowseExisting = New System.Windows.Forms.Button
        Me.txtFilenameExisting = New System.Windows.Forms.TextBox
        Me.RadioAddExisting = New System.Windows.Forms.RadioButton
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(488, 136)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 9
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'RadioIndividual
        '
        Me.RadioIndividual.AutoSize = True
        Me.RadioIndividual.Checked = True
        Me.RadioIndividual.Location = New System.Drawing.Point(27, 48)
        Me.RadioIndividual.Name = "RadioIndividual"
        Me.RadioIndividual.Size = New System.Drawing.Size(238, 17)
        Me.RadioIndividual.TabIndex = 1
        Me.RadioIndividual.TabStop = True
        Me.RadioIndividual.Text = "Add individual files (one per station) to project"
        Me.RadioIndividual.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Location = New System.Drawing.Point(12, 18)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(95, 13)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "After downloading "
        '
        'RadioAddNew
        '
        Me.RadioAddNew.AutoSize = True
        Me.RadioAddNew.Location = New System.Drawing.Point(27, 71)
        Me.RadioAddNew.Name = "RadioAddNew"
        Me.RadioAddNew.Size = New System.Drawing.Size(153, 17)
        Me.RadioAddNew.TabIndex = 2
        Me.RadioAddNew.TabStop = True
        Me.RadioAddNew.Text = "Add data to new WDM file:"
        Me.RadioAddNew.UseVisualStyleBackColor = True
        '
        'RadioDontAdd
        '
        Me.RadioDontAdd.AutoSize = True
        Me.RadioDontAdd.Location = New System.Drawing.Point(27, 117)
        Me.RadioDontAdd.Name = "RadioDontAdd"
        Me.RadioDontAdd.Size = New System.Drawing.Size(149, 17)
        Me.RadioDontAdd.TabIndex = 8
        Me.RadioDontAdd.TabStop = True
        Me.RadioDontAdd.Text = "Do not add data to project"
        Me.RadioDontAdd.UseVisualStyleBackColor = True
        '
        'txtFilenameNew
        '
        Me.txtFilenameNew.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilenameNew.Location = New System.Drawing.Point(201, 69)
        Me.txtFilenameNew.Name = "txtFilenameNew"
        Me.txtFilenameNew.Size = New System.Drawing.Size(281, 20)
        Me.txtFilenameNew.TabIndex = 3
        '
        'btnBrowseNew
        '
        Me.btnBrowseNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseNew.Location = New System.Drawing.Point(488, 69)
        Me.btnBrowseNew.Name = "btnBrowseNew"
        Me.btnBrowseNew.Size = New System.Drawing.Size(75, 20)
        Me.btnBrowseNew.TabIndex = 4
        Me.btnBrowseNew.Text = "Browse..."
        Me.btnBrowseNew.UseVisualStyleBackColor = True
        '
        'btnBrowseExisting
        '
        Me.btnBrowseExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseExisting.Location = New System.Drawing.Point(488, 92)
        Me.btnBrowseExisting.Name = "btnBrowseExisting"
        Me.btnBrowseExisting.Size = New System.Drawing.Size(75, 20)
        Me.btnBrowseExisting.TabIndex = 7
        Me.btnBrowseExisting.Text = "Browse..."
        Me.btnBrowseExisting.UseVisualStyleBackColor = True
        '
        'txtFilenameExisting
        '
        Me.txtFilenameExisting.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilenameExisting.Location = New System.Drawing.Point(201, 92)
        Me.txtFilenameExisting.Name = "txtFilenameExisting"
        Me.txtFilenameExisting.Size = New System.Drawing.Size(281, 20)
        Me.txtFilenameExisting.TabIndex = 6
        '
        'RadioAddExisting
        '
        Me.RadioAddExisting.AutoSize = True
        Me.RadioAddExisting.Location = New System.Drawing.Point(27, 94)
        Me.RadioAddExisting.Name = "RadioAddExisting"
        Me.RadioAddExisting.Size = New System.Drawing.Size(168, 17)
        Me.RadioAddExisting.TabIndex = 5
        Me.RadioAddExisting.TabStop = True
        Me.RadioAddExisting.Text = "Add data to existing WDM file:"
        Me.RadioAddExisting.UseVisualStyleBackColor = True
        '
        'frmWDM
        '
        Me.AcceptButton = Me.btnOk
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(575, 171)
        Me.Controls.Add(Me.btnBrowseExisting)
        Me.Controls.Add(Me.txtFilenameExisting)
        Me.Controls.Add(Me.RadioAddExisting)
        Me.Controls.Add(Me.btnBrowseNew)
        Me.Controls.Add(Me.txtFilenameNew)
        Me.Controls.Add(Me.RadioDontAdd)
        Me.Controls.Add(Me.RadioAddNew)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.RadioIndividual)
        Me.Controls.Add(Me.btnOk)
        Me.Name = "frmWDM"
        Me.Text = " Processing Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents RadioIndividual As System.Windows.Forms.RadioButton
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents RadioAddNew As System.Windows.Forms.RadioButton
    Friend WithEvents RadioDontAdd As System.Windows.Forms.RadioButton
    Friend WithEvents txtFilenameNew As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseNew As System.Windows.Forms.Button
    Friend WithEvents btnBrowseExisting As System.Windows.Forms.Button
    Friend WithEvents txtFilenameExisting As System.Windows.Forms.TextBox
    Friend WithEvents RadioAddExisting As System.Windows.Forms.RadioButton
End Class
