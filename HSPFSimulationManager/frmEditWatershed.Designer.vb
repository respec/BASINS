﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditWatershed
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEditWatershed))
        Me.btnAddScenario = New System.Windows.Forms.Button()
        Me.lblUCIFile = New System.Windows.Forms.Label()
        Me.lblDownstream = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.btnImage = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblWatershedImage = New System.Windows.Forms.Label()
        Me.btnRemoveScenario = New System.Windows.Forms.Button()
        Me.cboDownstream = New System.Windows.Forms.ComboBox()
        Me.btnConnectionReport = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.lstScenarios = New System.Windows.Forms.ListBox()
        Me.btnRenameScenario = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnAddScenario
        '
        Me.btnAddScenario.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddScenario.Location = New System.Drawing.Point(115, 193)
        Me.btnAddScenario.Name = "btnAddScenario"
        Me.btnAddScenario.Size = New System.Drawing.Size(54, 23)
        Me.btnAddScenario.TabIndex = 5
        Me.btnAddScenario.Text = "Add"
        Me.btnAddScenario.UseVisualStyleBackColor = True
        '
        'lblUCIFile
        '
        Me.lblUCIFile.AutoSize = True
        Me.lblUCIFile.Location = New System.Drawing.Point(12, 94)
        Me.lblUCIFile.Name = "lblUCIFile"
        Me.lblUCIFile.Size = New System.Drawing.Size(95, 13)
        Me.lblUCIFile.TabIndex = 24
        Me.lblUCIFile.Text = "Scenario (UCI File)"
        '
        'lblDownstream
        '
        Me.lblDownstream.AutoSize = True
        Me.lblDownstream.Location = New System.Drawing.Point(12, 41)
        Me.lblDownstream.Name = "lblDownstream"
        Me.lblDownstream.Size = New System.Drawing.Size(97, 13)
        Me.lblDownstream.TabIndex = 27
        Me.lblDownstream.Text = "Downstream Name"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(12, 15)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(90, 13)
        Me.lblName.TabIndex = 29
        Me.lblName.Text = "Watershed Name"
        '
        'txtName
        '
        Me.txtName.AllowDrop = True
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(115, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(543, 20)
        Me.txtName.TabIndex = 1
        Me.txtName.Text = "Salado"
        '
        'btnImage
        '
        Me.btnImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnImage.AutoSize = True
        Me.btnImage.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnImage.Location = New System.Drawing.Point(115, 222)
        Me.btnImage.Name = "btnImage"
        Me.btnImage.Size = New System.Drawing.Size(164, 155)
        Me.btnImage.TabIndex = 8
        Me.btnImage.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnImage.UseVisualStyleBackColor = False
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOk.AutoSize = True
        Me.btnOk.Location = New System.Drawing.Point(15, 383)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(84, 23)
        Me.btnOk.TabIndex = 9
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.AutoSize = True
        Me.btnCancel.Location = New System.Drawing.Point(105, 383)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(84, 23)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblWatershedImage
        '
        Me.lblWatershedImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblWatershedImage.AutoSize = True
        Me.lblWatershedImage.Location = New System.Drawing.Point(12, 291)
        Me.lblWatershedImage.Name = "lblWatershedImage"
        Me.lblWatershedImage.Size = New System.Drawing.Size(91, 13)
        Me.lblWatershedImage.TabIndex = 36
        Me.lblWatershedImage.Text = "Watershed Image"
        '
        'btnRemoveScenario
        '
        Me.btnRemoveScenario.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveScenario.Location = New System.Drawing.Point(175, 193)
        Me.btnRemoveScenario.Name = "btnRemoveScenario"
        Me.btnRemoveScenario.Size = New System.Drawing.Size(64, 23)
        Me.btnRemoveScenario.TabIndex = 6
        Me.btnRemoveScenario.Text = "Remove"
        Me.btnRemoveScenario.UseVisualStyleBackColor = True
        '
        'cboDownstream
        '
        Me.cboDownstream.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboDownstream.FormattingEnabled = True
        Me.cboDownstream.Location = New System.Drawing.Point(115, 38)
        Me.cboDownstream.Name = "cboDownstream"
        Me.cboDownstream.Size = New System.Drawing.Size(543, 21)
        Me.cboDownstream.TabIndex = 2
        '
        'btnConnectionReport
        '
        Me.btnConnectionReport.Location = New System.Drawing.Point(115, 65)
        Me.btnConnectionReport.Name = "btnConnectionReport"
        Me.btnConnectionReport.Size = New System.Drawing.Size(192, 23)
        Me.btnConnectionReport.TabIndex = 3
        Me.btnConnectionReport.Text = "Downstream Connection Report"
        Me.btnConnectionReport.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.AutoSize = True
        Me.btnRemove.Location = New System.Drawing.Point(195, 383)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(112, 23)
        Me.btnRemove.TabIndex = 11
        Me.btnRemove.Text = "Remove Watershed"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'lstScenarios
        '
        Me.lstScenarios.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstScenarios.IntegralHeight = False
        Me.lstScenarios.Location = New System.Drawing.Point(115, 94)
        Me.lstScenarios.Name = "lstScenarios"
        Me.lstScenarios.Size = New System.Drawing.Size(543, 93)
        Me.lstScenarios.TabIndex = 4
        '
        'btnRenameScenario
        '
        Me.btnRenameScenario.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRenameScenario.Location = New System.Drawing.Point(245, 193)
        Me.btnRenameScenario.Name = "btnRenameScenario"
        Me.btnRenameScenario.Size = New System.Drawing.Size(64, 23)
        Me.btnRenameScenario.TabIndex = 7
        Me.btnRenameScenario.Text = "Rename"
        Me.btnRenameScenario.UseVisualStyleBackColor = True
        '
        'frmEditWatershed
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(670, 418)
        Me.Controls.Add(Me.btnRenameScenario)
        Me.Controls.Add(Me.lstScenarios)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnConnectionReport)
        Me.Controls.Add(Me.cboDownstream)
        Me.Controls.Add(Me.btnRemoveScenario)
        Me.Controls.Add(Me.lblWatershedImage)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblDownstream)
        Me.Controls.Add(Me.btnAddScenario)
        Me.Controls.Add(Me.lblUCIFile)
        Me.Controls.Add(Me.btnImage)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmEditWatershed"
        Me.Text = "Edit Watershed"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAddScenario As System.Windows.Forms.Button
    Friend WithEvents lblUCIFile As System.Windows.Forms.Label
    Friend WithEvents lblDownstream As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents btnImage As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblWatershedImage As System.Windows.Forms.Label
    Friend WithEvents btnRemoveScenario As System.Windows.Forms.Button
    Friend WithEvents cboDownstream As System.Windows.Forms.ComboBox
    Friend WithEvents btnConnectionReport As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents lstScenarios As System.Windows.Forms.ListBox
    Friend WithEvents btnRenameScenario As System.Windows.Forms.Button
End Class
