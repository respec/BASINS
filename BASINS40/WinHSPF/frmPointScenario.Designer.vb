<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPointScenario
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.atxNew = New atcControls.atcText
        Me.cboBase = New System.Windows.Forms.ComboBox
        Me.atxMult = New atcControls.atcText
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(52, 22)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Base on Scenario"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(52, 60)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(136, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "New Scenario Name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(52, 95)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Multiplier"
        '
        'atxNew
        '
        Me.atxNew.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxNew.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.atxNew.DefaultValue = ""
        Me.atxNew.HardMax = -999
        Me.atxNew.HardMin = -999
        Me.atxNew.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxNew.Location = New System.Drawing.Point(197, 54)
        Me.atxNew.Margin = New System.Windows.Forms.Padding(4)
        Me.atxNew.MaxWidth = 20
        Me.atxNew.Name = "atxNew"
        Me.atxNew.NumericFormat = "0.#####"
        Me.atxNew.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxNew.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxNew.SelLength = 0
        Me.atxNew.SelStart = 0
        Me.atxNew.Size = New System.Drawing.Size(172, 28)
        Me.atxNew.SoftMax = -999
        Me.atxNew.SoftMin = -999
        Me.atxNew.TabIndex = 3
        Me.atxNew.ValueDouble = 0
        Me.atxNew.ValueInteger = 0
        '
        'cboBase
        '
        Me.cboBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBase.FormattingEnabled = True
        Me.cboBase.Location = New System.Drawing.Point(197, 17)
        Me.cboBase.Margin = New System.Windows.Forms.Padding(4)
        Me.cboBase.Name = "cboBase"
        Me.cboBase.Size = New System.Drawing.Size(171, 24)
        Me.cboBase.TabIndex = 5
        '
        'atxMult
        '
        Me.atxMult.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxMult.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.atxMult.DefaultValue = ""
        Me.atxMult.HardMax = -999
        Me.atxMult.HardMin = -999
        Me.atxMult.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxMult.Location = New System.Drawing.Point(197, 89)
        Me.atxMult.Margin = New System.Windows.Forms.Padding(4)
        Me.atxMult.MaxWidth = 20
        Me.atxMult.Name = "atxMult"
        Me.atxMult.NumericFormat = "0.#####"
        Me.atxMult.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxMult.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxMult.SelLength = 0
        Me.atxMult.SelStart = 0
        Me.atxMult.Size = New System.Drawing.Size(172, 28)
        Me.atxMult.SoftMax = -999
        Me.atxMult.SoftMin = -999
        Me.atxMult.TabIndex = 6
        Me.atxMult.ValueDouble = 0
        Me.atxMult.ValueInteger = 0
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(216, 137)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(135, 32)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(71, 137)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(135, 32)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'frmPointScenario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(421, 183)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.atxMult)
        Me.Controls.Add(Me.cboBase)
        Me.Controls.Add(Me.atxNew)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmPointScenario"
        Me.Text = "WinHSPF - Create Point Source Scenario"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents atxNew As atcControls.atcText
    Friend WithEvents cboBase As System.Windows.Forms.ComboBox
    Friend WithEvents atxMult As atcControls.atcText
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
End Class
