<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModel))
        Me.btnBrowseUCIFile = New System.Windows.Forms.Button()
        Me.lblUCIFile = New System.Windows.Forms.Label()
        Me.txtUCI = New System.Windows.Forms.TextBox()
        Me.btnBrowseDownstream = New System.Windows.Forms.Button()
        Me.lblDownstream = New System.Windows.Forms.Label()
        Me.txtDownstream = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.btnUSAR = New System.Windows.Forms.Button()
        Me.btnRunHSPF = New System.Windows.Forms.Button()
        Me.chkRunDownstream = New System.Windows.Forms.CheckBox()
        Me.btnWinHSPF = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnBrowseUCIFile
        '
        Me.btnBrowseUCIFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseUCIFile.Location = New System.Drawing.Point(656, 12)
        Me.btnBrowseUCIFile.Name = "btnBrowseUCIFile"
        Me.btnBrowseUCIFile.Size = New System.Drawing.Size(33, 20)
        Me.btnBrowseUCIFile.TabIndex = 23
        Me.btnBrowseUCIFile.Text = "..."
        Me.btnBrowseUCIFile.UseVisualStyleBackColor = True
        '
        'lblUCIFile
        '
        Me.lblUCIFile.AutoSize = True
        Me.lblUCIFile.Location = New System.Drawing.Point(12, 15)
        Me.lblUCIFile.Name = "lblUCIFile"
        Me.lblUCIFile.Size = New System.Drawing.Size(44, 13)
        Me.lblUCIFile.TabIndex = 24
        Me.lblUCIFile.Text = "UCI File"
        '
        'txtUCI
        '
        Me.txtUCI.AllowDrop = True
        Me.txtUCI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUCI.Location = New System.Drawing.Point(105, 12)
        Me.txtUCI.Name = "txtUCI"
        Me.txtUCI.Size = New System.Drawing.Size(545, 20)
        Me.txtUCI.TabIndex = 22
        Me.txtUCI.Text = "C:\data\Salado_4yr\SaladoCreek_HSPF10_108_over_try162_hourly.uci"
        '
        'btnBrowseDownstream
        '
        Me.btnBrowseDownstream.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseDownstream.Location = New System.Drawing.Point(656, 64)
        Me.btnBrowseDownstream.Name = "btnBrowseDownstream"
        Me.btnBrowseDownstream.Size = New System.Drawing.Size(33, 20)
        Me.btnBrowseDownstream.TabIndex = 26
        Me.btnBrowseDownstream.Text = "..."
        Me.btnBrowseDownstream.UseVisualStyleBackColor = True
        '
        'lblDownstream
        '
        Me.lblDownstream.AutoSize = True
        Me.lblDownstream.Location = New System.Drawing.Point(12, 67)
        Me.lblDownstream.Name = "lblDownstream"
        Me.lblDownstream.Size = New System.Drawing.Size(87, 13)
        Me.lblDownstream.TabIndex = 27
        Me.lblDownstream.Text = "Downstream UCI"
        '
        'txtDownstream
        '
        Me.txtDownstream.AllowDrop = True
        Me.txtDownstream.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDownstream.Location = New System.Drawing.Point(105, 64)
        Me.txtDownstream.Name = "txtDownstream"
        Me.txtDownstream.Size = New System.Drawing.Size(545, 20)
        Me.txtDownstream.TabIndex = 25
        Me.txtDownstream.Text = "C:\data\USAR\BMPtoolTest_USAR014.uci"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(12, 41)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(67, 13)
        Me.lblName.TabIndex = 29
        Me.lblName.Text = "Model Name"
        '
        'txtName
        '
        Me.txtName.AllowDrop = True
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(105, 38)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(545, 20)
        Me.txtName.TabIndex = 28
        Me.txtName.Text = "Salado"
        '
        'btnUSAR
        '
        Me.btnUSAR.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUSAR.AutoSize = True
        Me.btnUSAR.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnUSAR.Image = Global.HSPFSimulationManager.My.Resources.Resources.Salado_Simple_50
        Me.btnUSAR.Location = New System.Drawing.Point(695, 12)
        Me.btnUSAR.Name = "btnUSAR"
        Me.btnUSAR.Size = New System.Drawing.Size(164, 155)
        Me.btnUSAR.TabIndex = 30
        Me.btnUSAR.Text = "Watershed Image"
        Me.btnUSAR.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnUSAR.UseVisualStyleBackColor = False
        '
        'btnRunHSPF
        '
        Me.btnRunHSPF.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRunHSPF.Location = New System.Drawing.Point(12, 145)
        Me.btnRunHSPF.Name = "btnRunHSPF"
        Me.btnRunHSPF.Size = New System.Drawing.Size(93, 23)
        Me.btnRunHSPF.TabIndex = 31
        Me.btnRunHSPF.Text = "Run HSPF"
        Me.btnRunHSPF.UseVisualStyleBackColor = True
        '
        'chkRunDownstream
        '
        Me.chkRunDownstream.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkRunDownstream.AutoSize = True
        Me.chkRunDownstream.Location = New System.Drawing.Point(111, 149)
        Me.chkRunDownstream.Name = "chkRunDownstream"
        Me.chkRunDownstream.Size = New System.Drawing.Size(194, 17)
        Me.chkRunDownstream.TabIndex = 32
        Me.chkRunDownstream.Text = "Also run downstream: USAR, LSAR"
        Me.chkRunDownstream.UseVisualStyleBackColor = True
        '
        'btnWinHSPF
        '
        Me.btnWinHSPF.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnWinHSPF.AutoSize = True
        Me.btnWinHSPF.Location = New System.Drawing.Point(12, 113)
        Me.btnWinHSPF.Name = "btnWinHSPF"
        Me.btnWinHSPF.Size = New System.Drawing.Size(93, 23)
        Me.btnWinHSPF.TabIndex = 33
        Me.btnWinHSPF.Text = "Open WinHSPF"
        Me.btnWinHSPF.UseVisualStyleBackColor = True
        '
        'frmModel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(871, 181)
        Me.Controls.Add(Me.btnWinHSPF)
        Me.Controls.Add(Me.chkRunDownstream)
        Me.Controls.Add(Me.btnRunHSPF)
        Me.Controls.Add(Me.btnUSAR)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.btnBrowseDownstream)
        Me.Controls.Add(Me.lblDownstream)
        Me.Controls.Add(Me.txtDownstream)
        Me.Controls.Add(Me.btnBrowseUCIFile)
        Me.Controls.Add(Me.lblUCIFile)
        Me.Controls.Add(Me.txtUCI)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmModel"
        Me.Text = "HSPF Model"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBrowseUCIFile As System.Windows.Forms.Button
    Friend WithEvents lblUCIFile As System.Windows.Forms.Label
    Friend WithEvents txtUCI As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseDownstream As System.Windows.Forms.Button
    Friend WithEvents lblDownstream As System.Windows.Forms.Label
    Friend WithEvents txtDownstream As System.Windows.Forms.TextBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents btnUSAR As System.Windows.Forms.Button
    Friend WithEvents btnRunHSPF As System.Windows.Forms.Button
    Friend WithEvents chkRunDownstream As System.Windows.Forms.CheckBox
    Friend WithEvents btnWinHSPF As System.Windows.Forms.Button
End Class
