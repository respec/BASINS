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
        Me.atxValue = New atcControls.atcText
        Me.txtScen = New atcControls.atcText
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 18)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Scenario"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 50)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Reach"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 84)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Pollutant"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 117)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Facility"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 155)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 17)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Daily Load"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(112, 18)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(31, 17)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "PT-"
        '
        'cboReach
        '
        Me.cboReach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReach.FormattingEnabled = True
        Me.cboReach.Location = New System.Drawing.Point(115, 46)
        Me.cboReach.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboReach.Name = "cboReach"
        Me.cboReach.Size = New System.Drawing.Size(344, 24)
        Me.cboReach.TabIndex = 7
        '
        'cboPollutant
        '
        Me.cboPollutant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPollutant.FormattingEnabled = True
        Me.cboPollutant.Location = New System.Drawing.Point(115, 79)
        Me.cboPollutant.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboPollutant.Name = "cboPollutant"
        Me.cboPollutant.Size = New System.Drawing.Size(344, 24)
        Me.cboPollutant.TabIndex = 8
        '
        'cboFac
        '
        Me.cboFac.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFac.FormattingEnabled = True
        Me.cboFac.Location = New System.Drawing.Point(115, 112)
        Me.cboFac.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboFac.Name = "cboFac"
        Me.cboFac.Size = New System.Drawing.Size(344, 24)
        Me.cboFac.TabIndex = 9
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(245, 207)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(135, 32)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(100, 207)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(135, 32)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'atxValue
        '
        Me.atxValue.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxValue.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.atxValue.DefaultValue = ""
        Me.atxValue.HardMax = -999
        Me.atxValue.HardMin = -999
        Me.atxValue.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxValue.Location = New System.Drawing.Point(115, 145)
        Me.atxValue.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.atxValue.MaxWidth = 20
        Me.atxValue.Name = "atxValue"
        Me.atxValue.NumericFormat = "0.#####"
        Me.atxValue.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxValue.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.atxValue.SelLength = 0
        Me.atxValue.SelStart = 0
        Me.atxValue.Size = New System.Drawing.Size(97, 36)
        Me.atxValue.SoftMax = -999
        Me.atxValue.SoftMin = -999
        Me.atxValue.TabIndex = 10
        Me.atxValue.ValueDouble = 0
        Me.atxValue.ValueInteger = 0
        '
        'txtScen
        '
        Me.txtScen.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtScen.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.txtScen.DefaultValue = ""
        Me.txtScen.HardMax = -999
        Me.txtScen.HardMin = -999
        Me.txtScen.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtScen.Location = New System.Drawing.Point(144, 14)
        Me.txtScen.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtScen.MaxWidth = 20
        Me.txtScen.Name = "txtScen"
        Me.txtScen.NumericFormat = "0.#####"
        Me.txtScen.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtScen.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtScen.SelLength = 0
        Me.txtScen.SelStart = 0
        Me.txtScen.Size = New System.Drawing.Size(97, 25)
        Me.txtScen.SoftMax = -999
        Me.txtScen.SoftMin = -999
        Me.txtScen.TabIndex = 5
        Me.txtScen.ValueDouble = 0
        Me.txtScen.ValueInteger = 0
        '
        'frmAddPoint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(481, 254)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.atxValue)
        Me.Controls.Add(Me.cboFac)
        Me.Controls.Add(Me.cboPollutant)
        Me.Controls.Add(Me.cboReach)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtScen)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
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
    Friend WithEvents txtScen As atcControls.atcText
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cboReach As System.Windows.Forms.ComboBox
    Friend WithEvents cboPollutant As System.Windows.Forms.ComboBox
    Friend WithEvents cboFac As System.Windows.Forms.ComboBox
    Friend WithEvents atxValue As atcControls.atcText
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
End Class
