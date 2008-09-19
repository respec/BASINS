<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditGlobalBlock
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.lblRunInfo = New System.Windows.Forms.Label
        Me.txtRunInfo = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.comboGen = New System.Windows.Forms.ComboBox
        Me.comboSpout = New System.Windows.Forms.ComboBox
        Me.comboUnits = New System.Windows.Forms.ComboBox
        Me.comboRunFlag = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtEndMin = New atcControls.atcText
        Me.txtStartMin = New atcControls.atcText
        Me.txtEndHr = New atcControls.atcText
        Me.txtStartHr = New atcControls.atcText
        Me.txtEndDay = New atcControls.atcText
        Me.txtStartDay = New atcControls.atcText
        Me.txtEndMo = New atcControls.atcText
        Me.txtStartMo = New atcControls.atcText
        Me.txtEndYr = New atcControls.atcText
        Me.txtStartYr = New atcControls.atcText
        Me.SuspendLayout()
        '
        'lblRunInfo
        '
        Me.lblRunInfo.AutoSize = True
        Me.lblRunInfo.Location = New System.Drawing.Point(11, 12)
        Me.lblRunInfo.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblRunInfo.Name = "lblRunInfo"
        Me.lblRunInfo.Size = New System.Drawing.Size(85, 13)
        Me.lblRunInfo.TabIndex = 0
        Me.lblRunInfo.Text = "Run Information:"
        '
        'txtRunInfo
        '
        Me.txtRunInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRunInfo.Location = New System.Drawing.Point(100, 10)
        Me.txtRunInfo.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtRunInfo.Name = "txtRunInfo"
        Me.txtRunInfo.Size = New System.Drawing.Size(506, 20)
        Me.txtRunInfo.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(61, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Span"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(64, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Start:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(64, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "End:"
        '
        'comboGen
        '
        Me.comboGen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboGen.FormattingEnabled = True
        Me.comboGen.Location = New System.Drawing.Point(391, 63)
        Me.comboGen.Name = "comboGen"
        Me.comboGen.Size = New System.Drawing.Size(72, 21)
        Me.comboGen.TabIndex = 12
        '
        'comboSpout
        '
        Me.comboSpout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboSpout.FormattingEnabled = True
        Me.comboSpout.Location = New System.Drawing.Point(391, 88)
        Me.comboSpout.Name = "comboSpout"
        Me.comboSpout.Size = New System.Drawing.Size(72, 21)
        Me.comboSpout.TabIndex = 13
        '
        'comboUnits
        '
        Me.comboUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboUnits.FormattingEnabled = True
        Me.comboUnits.Location = New System.Drawing.Point(534, 88)
        Me.comboUnits.Name = "comboUnits"
        Me.comboUnits.Size = New System.Drawing.Size(72, 21)
        Me.comboUnits.TabIndex = 15
        '
        'comboRunFlag
        '
        Me.comboRunFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboRunFlag.FormattingEnabled = True
        Me.comboRunFlag.Location = New System.Drawing.Point(534, 63)
        Me.comboRunFlag.Name = "comboRunFlag"
        Me.comboRunFlag.Size = New System.Drawing.Size(72, 21)
        Me.comboRunFlag.TabIndex = 14
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(347, 85)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Special"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(342, 66)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 13)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "General:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(344, 98)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "Actions:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(494, 91)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(34, 13)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Units:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(475, 66)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 13)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "Run Flag:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(110, 45)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(29, 13)
        Me.Label9.TabIndex = 24
        Me.Label9.Text = "Year"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(158, 45)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(22, 13)
        Me.Label10.TabIndex = 25
        Me.Label10.Text = "Mo"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(197, 45)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(26, 13)
        Me.Label11.TabIndex = 26
        Me.Label11.Text = "Day"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(242, 45)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(18, 13)
        Me.Label12.TabIndex = 27
        Me.Label12.Text = "Hr"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(280, 45)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(24, 13)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "Min"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(320, 34)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(71, 13)
        Me.Label14.TabIndex = 29
        Me.Label14.Text = "Output Level:"
        '
        'txtEndMin
        '
        Me.txtEndMin.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndMin.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndMin.DefaultValue = "0"
        Me.txtEndMin.HardMax = 59
        Me.txtEndMin.HardMin = 0
        Me.txtEndMin.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndMin.Location = New System.Drawing.Point(275, 88)
        Me.txtEndMin.MaxWidth = 0
        Me.txtEndMin.Name = "txtEndMin"
        Me.txtEndMin.NumericFormat = "0.#####"
        Me.txtEndMin.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndMin.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndMin.SelLength = 1
        Me.txtEndMin.SelStart = 0
        Me.txtEndMin.Size = New System.Drawing.Size(35, 19)
        Me.txtEndMin.SoftMax = 0
        Me.txtEndMin.SoftMin = 0
        Me.txtEndMin.TabIndex = 11
        Me.txtEndMin.ValueDouble = 0
        Me.txtEndMin.ValueInteger = 0
        '
        'txtStartMin
        '
        Me.txtStartMin.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartMin.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartMin.DefaultValue = "0"
        Me.txtStartMin.HardMax = 59
        Me.txtStartMin.HardMin = 0
        Me.txtStartMin.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartMin.Location = New System.Drawing.Point(275, 63)
        Me.txtStartMin.MaxWidth = 0
        Me.txtStartMin.Name = "txtStartMin"
        Me.txtStartMin.NumericFormat = "0.#####"
        Me.txtStartMin.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMin.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMin.SelLength = 1
        Me.txtStartMin.SelStart = 0
        Me.txtStartMin.Size = New System.Drawing.Size(35, 19)
        Me.txtStartMin.SoftMax = 0
        Me.txtStartMin.SoftMin = 0
        Me.txtStartMin.TabIndex = 6
        Me.txtStartMin.ValueDouble = 0
        Me.txtStartMin.ValueInteger = 0
        '
        'txtEndHr
        '
        Me.txtEndHr.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndHr.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndHr.DefaultValue = "0"
        Me.txtEndHr.HardMax = 24
        Me.txtEndHr.HardMin = 0
        Me.txtEndHr.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndHr.Location = New System.Drawing.Point(234, 88)
        Me.txtEndHr.MaxWidth = 0
        Me.txtEndHr.Name = "txtEndHr"
        Me.txtEndHr.NumericFormat = "0.#####"
        Me.txtEndHr.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndHr.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndHr.SelLength = 1
        Me.txtEndHr.SelStart = 0
        Me.txtEndHr.Size = New System.Drawing.Size(35, 19)
        Me.txtEndHr.SoftMax = 0
        Me.txtEndHr.SoftMin = 0
        Me.txtEndHr.TabIndex = 10
        Me.txtEndHr.ValueDouble = 0
        Me.txtEndHr.ValueInteger = 0
        '
        'txtStartHr
        '
        Me.txtStartHr.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartHr.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartHr.DefaultValue = "0"
        Me.txtStartHr.HardMax = 24
        Me.txtStartHr.HardMin = 0
        Me.txtStartHr.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartHr.Location = New System.Drawing.Point(234, 63)
        Me.txtStartHr.MaxWidth = 0
        Me.txtStartHr.Name = "txtStartHr"
        Me.txtStartHr.NumericFormat = "0.#####"
        Me.txtStartHr.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartHr.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartHr.SelLength = 1
        Me.txtStartHr.SelStart = 0
        Me.txtStartHr.Size = New System.Drawing.Size(35, 19)
        Me.txtStartHr.SoftMax = 0
        Me.txtStartHr.SoftMin = 0
        Me.txtStartHr.TabIndex = 5
        Me.txtStartHr.ValueDouble = 0
        Me.txtStartHr.ValueInteger = 0
        '
        'txtEndDay
        '
        Me.txtEndDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndDay.DefaultValue = "1"
        Me.txtEndDay.HardMax = 31
        Me.txtEndDay.HardMin = 1
        Me.txtEndDay.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndDay.Location = New System.Drawing.Point(193, 88)
        Me.txtEndDay.MaxWidth = 0
        Me.txtEndDay.Name = "txtEndDay"
        Me.txtEndDay.NumericFormat = "0.#####"
        Me.txtEndDay.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndDay.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndDay.SelLength = 1
        Me.txtEndDay.SelStart = 0
        Me.txtEndDay.Size = New System.Drawing.Size(35, 19)
        Me.txtEndDay.SoftMax = 0
        Me.txtEndDay.SoftMin = 0
        Me.txtEndDay.TabIndex = 9
        Me.txtEndDay.ValueDouble = 1
        Me.txtEndDay.ValueInteger = 1
        '
        'txtStartDay
        '
        Me.txtStartDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartDay.DefaultValue = "1"
        Me.txtStartDay.HardMax = 31
        Me.txtStartDay.HardMin = 1
        Me.txtStartDay.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartDay.Location = New System.Drawing.Point(193, 63)
        Me.txtStartDay.MaxWidth = 0
        Me.txtStartDay.Name = "txtStartDay"
        Me.txtStartDay.NumericFormat = "0.#####"
        Me.txtStartDay.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartDay.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartDay.SelLength = 1
        Me.txtStartDay.SelStart = 0
        Me.txtStartDay.Size = New System.Drawing.Size(35, 19)
        Me.txtStartDay.SoftMax = 0
        Me.txtStartDay.SoftMin = 0
        Me.txtStartDay.TabIndex = 4
        Me.txtStartDay.ValueDouble = 1
        Me.txtStartDay.ValueInteger = 1
        '
        'txtEndMo
        '
        Me.txtEndMo.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndMo.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndMo.DefaultValue = "1"
        Me.txtEndMo.HardMax = 12
        Me.txtEndMo.HardMin = 1
        Me.txtEndMo.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndMo.Location = New System.Drawing.Point(152, 88)
        Me.txtEndMo.MaxWidth = 0
        Me.txtEndMo.Name = "txtEndMo"
        Me.txtEndMo.NumericFormat = "0.#####"
        Me.txtEndMo.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndMo.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndMo.SelLength = 1
        Me.txtEndMo.SelStart = 0
        Me.txtEndMo.Size = New System.Drawing.Size(35, 19)
        Me.txtEndMo.SoftMax = 0
        Me.txtEndMo.SoftMin = 0
        Me.txtEndMo.TabIndex = 8
        Me.txtEndMo.ValueDouble = 1
        Me.txtEndMo.ValueInteger = 1
        '
        'txtStartMo
        '
        Me.txtStartMo.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartMo.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartMo.DefaultValue = "1"
        Me.txtStartMo.HardMax = 12
        Me.txtStartMo.HardMin = 1
        Me.txtStartMo.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartMo.Location = New System.Drawing.Point(152, 63)
        Me.txtStartMo.MaxWidth = 0
        Me.txtStartMo.Name = "txtStartMo"
        Me.txtStartMo.NumericFormat = "0.#####"
        Me.txtStartMo.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMo.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMo.SelLength = 1
        Me.txtStartMo.SelStart = 0
        Me.txtStartMo.Size = New System.Drawing.Size(35, 19)
        Me.txtStartMo.SoftMax = 0
        Me.txtStartMo.SoftMin = 0
        Me.txtStartMo.TabIndex = 3
        Me.txtStartMo.ValueDouble = 1
        Me.txtStartMo.ValueInteger = 1
        '
        'txtEndYr
        '
        Me.txtEndYr.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndYr.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndYr.DefaultValue = "0"
        Me.txtEndYr.HardMax = 9999
        Me.txtEndYr.HardMin = 0
        Me.txtEndYr.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndYr.Location = New System.Drawing.Point(102, 88)
        Me.txtEndYr.MaxWidth = 0
        Me.txtEndYr.Name = "txtEndYr"
        Me.txtEndYr.NumericFormat = "0.#####"
        Me.txtEndYr.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndYr.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndYr.SelLength = 1
        Me.txtEndYr.SelStart = 0
        Me.txtEndYr.Size = New System.Drawing.Size(44, 19)
        Me.txtEndYr.SoftMax = 3000
        Me.txtEndYr.SoftMin = 1000
        Me.txtEndYr.TabIndex = 7
        Me.txtEndYr.ValueDouble = 0
        Me.txtEndYr.ValueInteger = 0
        '
        'txtStartYr
        '
        Me.txtStartYr.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartYr.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartYr.DefaultValue = "0"
        Me.txtStartYr.HardMax = 9999
        Me.txtStartYr.HardMin = 0
        Me.txtStartYr.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartYr.Location = New System.Drawing.Point(102, 63)
        Me.txtStartYr.MaxWidth = 0
        Me.txtStartYr.Name = "txtStartYr"
        Me.txtStartYr.NumericFormat = "0.#####"
        Me.txtStartYr.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartYr.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartYr.SelLength = 1
        Me.txtStartYr.SelStart = 0
        Me.txtStartYr.Size = New System.Drawing.Size(44, 19)
        Me.txtStartYr.SoftMax = 3000
        Me.txtStartYr.SoftMin = 1000
        Me.txtStartYr.TabIndex = 2
        Me.txtStartYr.Tag = "1111"
        Me.txtStartYr.ValueDouble = 0
        Me.txtStartYr.ValueInteger = 0
        '
        'ctlEditGlobalBlock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtEndMin)
        Me.Controls.Add(Me.txtStartMin)
        Me.Controls.Add(Me.txtEndHr)
        Me.Controls.Add(Me.txtStartHr)
        Me.Controls.Add(Me.txtEndDay)
        Me.Controls.Add(Me.txtStartDay)
        Me.Controls.Add(Me.txtEndMo)
        Me.Controls.Add(Me.txtStartMo)
        Me.Controls.Add(Me.txtEndYr)
        Me.Controls.Add(Me.txtStartYr)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.comboUnits)
        Me.Controls.Add(Me.comboRunFlag)
        Me.Controls.Add(Me.comboSpout)
        Me.Controls.Add(Me.comboGen)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtRunInfo)
        Me.Controls.Add(Me.lblRunInfo)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "ctlEditGlobalBlock"
        Me.Size = New System.Drawing.Size(617, 125)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblRunInfo As System.Windows.Forms.Label
    Friend WithEvents txtRunInfo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents comboGen As System.Windows.Forms.ComboBox
    Friend WithEvents comboSpout As System.Windows.Forms.ComboBox
    Friend WithEvents comboUnits As System.Windows.Forms.ComboBox
    Friend WithEvents comboRunFlag As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtStartYr As atcControls.atcText
    Friend WithEvents txtEndYr As atcControls.atcText
    Friend WithEvents txtStartMo As atcControls.atcText
    Friend WithEvents txtEndMo As atcControls.atcText
    Friend WithEvents txtEndDay As atcControls.atcText
    Friend WithEvents txtStartDay As atcControls.atcText
    Friend WithEvents txtEndHr As atcControls.atcText
    Friend WithEvents txtStartHr As atcControls.atcText
    Friend WithEvents txtEndMin As atcControls.atcText
    Friend WithEvents txtStartMin As atcControls.atcText

End Class
