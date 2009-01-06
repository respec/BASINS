<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddPoint
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
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.cboReach = New System.Windows.Forms.ComboBox
        Me.cboPollutant = New System.Windows.Forms.ComboBox
        Me.cboFac = New System.Windows.Forms.ComboBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.AtcText2 = New atcControls.atcText
        Me.AtcText1 = New atcControls.atcText
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Scenario"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Reach"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Pollutant"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 95)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Facility"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 126)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Daily Load"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(84, 15)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "PT-"
        '
        'cboReach
        '
        Me.cboReach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReach.FormattingEnabled = True
        Me.cboReach.Location = New System.Drawing.Point(86, 37)
        Me.cboReach.Name = "cboReach"
        Me.cboReach.Size = New System.Drawing.Size(259, 21)
        Me.cboReach.TabIndex = 7
        '
        'cboPollutant
        '
        Me.cboPollutant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPollutant.FormattingEnabled = True
        Me.cboPollutant.Location = New System.Drawing.Point(86, 64)
        Me.cboPollutant.Name = "cboPollutant"
        Me.cboPollutant.Size = New System.Drawing.Size(259, 21)
        Me.cboPollutant.TabIndex = 8
        '
        'cboFac
        '
        Me.cboFac.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFac.FormattingEnabled = True
        Me.cboFac.Location = New System.Drawing.Point(86, 91)
        Me.cboFac.Name = "cboFac"
        Me.cboFac.Size = New System.Drawing.Size(259, 21)
        Me.cboFac.TabIndex = 9
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(184, 168)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(101, 26)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(75, 168)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(101, 26)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'AtcText2
        '
        Me.AtcText2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText2.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText2.DefaultValue = ""
        Me.AtcText2.HardMax = -999
        Me.AtcText2.HardMin = -999
        Me.AtcText2.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText2.Location = New System.Drawing.Point(86, 118)
        Me.AtcText2.MaxWidth = 20
        Me.AtcText2.Name = "AtcText2"
        Me.AtcText2.NumericFormat = "0.#####"
        Me.AtcText2.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText2.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.AtcText2.SelLength = 1
        Me.AtcText2.SelStart = 0
        Me.AtcText2.Size = New System.Drawing.Size(73, 29)
        Me.AtcText2.SoftMax = -999
        Me.AtcText2.SoftMin = -999
        Me.AtcText2.TabIndex = 10
        Me.AtcText2.ValueDouble = 0
        Me.AtcText2.ValueInteger = 0
        '
        'AtcText1
        '
        Me.AtcText1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText1.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText1.DefaultValue = ""
        Me.AtcText1.HardMax = -999
        Me.AtcText1.HardMin = -999
        Me.AtcText1.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText1.Location = New System.Drawing.Point(108, 11)
        Me.AtcText1.MaxWidth = 20
        Me.AtcText1.Name = "AtcText1"
        Me.AtcText1.NumericFormat = "0.#####"
        Me.AtcText1.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText1.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText1.SelLength = 1
        Me.AtcText1.SelStart = 0
        Me.AtcText1.Size = New System.Drawing.Size(73, 20)
        Me.AtcText1.SoftMax = -999
        Me.AtcText1.SoftMin = -999
        Me.AtcText1.TabIndex = 5
        Me.AtcText1.ValueDouble = 0
        Me.AtcText1.ValueInteger = 0
        '
        'frmAddPoint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(361, 206)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.AtcText2)
        Me.Controls.Add(Me.cboFac)
        Me.Controls.Add(Me.cboPollutant)
        Me.Controls.Add(Me.cboReach)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.AtcText1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmAddPoint"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "WinHSPF - Create Point Source"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents AtcText1 As atcControls.atcText
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cboReach As System.Windows.Forms.ComboBox
    Friend WithEvents cboPollutant As System.Windows.Forms.ComboBox
    Friend WithEvents cboFac As System.Windows.Forms.ComboBox
    Friend WithEvents AtcText2 As atcControls.atcText
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
End Class
