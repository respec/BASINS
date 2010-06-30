<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDrawSelectShape_v2
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
    	Me.btnDone = New System.Windows.Forms.Button
    	Me.lblInstructions = New System.Windows.Forms.Label
    	Me.rdobtnOutlets = New System.Windows.Forms.RadioButton
    	Me.rdobtnInlets = New System.Windows.Forms.RadioButton
    	Me.chkbxRes = New System.Windows.Forms.CheckBox
    	Me.chkbxSrc = New System.Windows.Forms.CheckBox
    	Me.SuspendLayout
    	'
    	'btnDone
    	'
    	Me.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK
    	Me.btnDone.Location = New System.Drawing.Point(180, 7)
    	Me.btnDone.Name = "btnDone"
    	Me.btnDone.Size = New System.Drawing.Size(75, 23)
    	Me.btnDone.TabIndex = 0
    	Me.btnDone.Text = "Done"
    	Me.btnDone.UseVisualStyleBackColor = true
    	'
    	'lblInstructions
    	'
    	Me.lblInstructions.Location = New System.Drawing.Point(10, 7)
    	Me.lblInstructions.Name = "lblInstructions"
    	Me.lblInstructions.Size = New System.Drawing.Size(164, 28)
    	Me.lblInstructions.TabIndex = 1
    	Me.lblInstructions.Text = "Hold Control and Click to Select Outlets near or on reaches."
    	'
    	'rdobtnOutlets
    	'
    	Me.rdobtnOutlets.AutoSize = true
    	Me.rdobtnOutlets.Checked = true
    	Me.rdobtnOutlets.Location = New System.Drawing.Point(12, 38)
    	Me.rdobtnOutlets.Name = "rdobtnOutlets"
    	Me.rdobtnOutlets.Size = New System.Drawing.Size(58, 17)
    	Me.rdobtnOutlets.TabIndex = 2
    	Me.rdobtnOutlets.TabStop = true
    	Me.rdobtnOutlets.Text = "Outlets"
    	Me.rdobtnOutlets.UseVisualStyleBackColor = true
    	'
    	'rdobtnInlets
    	'
    	Me.rdobtnInlets.AutoSize = true
    	Me.rdobtnInlets.Location = New System.Drawing.Point(157, 38)
    	Me.rdobtnInlets.Name = "rdobtnInlets"
    	Me.rdobtnInlets.Size = New System.Drawing.Size(50, 17)
    	Me.rdobtnInlets.TabIndex = 3
    	Me.rdobtnInlets.Text = "Inlets"
    	Me.rdobtnInlets.UseVisualStyleBackColor = true
    	'
    	'chkbxRes
    	'
    	Me.chkbxRes.AutoSize = true
    	Me.chkbxRes.Location = New System.Drawing.Point(13, 61)
    	Me.chkbxRes.Name = "chkbxRes"
    	Me.chkbxRes.Size = New System.Drawing.Size(102, 17)
    	Me.chkbxRes.TabIndex = 5
    	Me.chkbxRes.Text = "Reservoir Outlet"
    	Me.chkbxRes.UseVisualStyleBackColor = true
    	'
    	'chkbxSrc
    	'
    	Me.chkbxSrc.Location = New System.Drawing.Point(157, 57)
    	Me.chkbxSrc.Name = "chkbxSrc"
    	Me.chkbxSrc.Size = New System.Drawing.Size(104, 24)
    	Me.chkbxSrc.TabIndex = 6
    	Me.chkbxSrc.Text = "Point Source"
    	Me.chkbxSrc.UseVisualStyleBackColor = true
    	'
    	'frmDrawSelectShape_v2
    	'
    	Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
    	Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    	Me.ClientSize = New System.Drawing.Size(259, 84)
    	Me.Controls.Add(Me.chkbxSrc)
    	Me.Controls.Add(Me.chkbxRes)
    	Me.Controls.Add(Me.rdobtnInlets)
    	Me.Controls.Add(Me.rdobtnOutlets)
    	Me.Controls.Add(Me.lblInstructions)
    	Me.Controls.Add(Me.btnDone)
    	Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
    	Me.Name = "frmDrawSelectShape_v2"
    	Me.ShowInTaskbar = false
    	Me.Text = "Click Done to Return"
    	Me.TopMost = true
    	Me.ResumeLayout(false)
    	Me.PerformLayout
    End Sub
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents lblInstructions As System.Windows.Forms.Label
    Friend WithEvents rdobtnOutlets As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnInlets As System.Windows.Forms.RadioButton
    Friend WithEvents chkbxRes As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxSrc As System.Windows.Forms.CheckBox
End Class
