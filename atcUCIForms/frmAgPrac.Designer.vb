<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAgPrac
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
        Me.lstPrac = New System.Windows.Forms.ListBox
        Me.lstSeg = New System.Windows.Forms.ListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.comboRep = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.GroupProps = New System.Windows.Forms.GroupBox
        Me.GroupLayers = New System.Windows.Forms.GroupBox
        Me.GroupPar = New System.Windows.Forms.GroupBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.AtcText4 = New atcControls.atcText
        Me.AtcText3 = New atcControls.atcText
        Me.AtcText5 = New atcControls.atcText
        Me.AtcText2 = New atcControls.atcText
        Me.txtStartMin = New atcControls.atcText
        Me.txtStartHr = New atcControls.atcText
        Me.txtStartYr = New atcControls.atcText
        Me.txtStartDay = New atcControls.atcText
        Me.txtStartMo = New atcControls.atcText
        Me.GroupProps.SuspendLayout()
        Me.GroupLayers.SuspendLayout()
        Me.GroupPar.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstPrac
        '
        Me.lstPrac.FormattingEnabled = True
        Me.lstPrac.Location = New System.Drawing.Point(25, 34)
        Me.lstPrac.Name = "lstPrac"
        Me.lstPrac.Size = New System.Drawing.Size(253, 212)
        Me.lstPrac.TabIndex = 0
        '
        'lstSeg
        '
        Me.lstSeg.FormattingEnabled = True
        Me.lstSeg.Location = New System.Drawing.Point(291, 34)
        Me.lstSeg.Name = "lstSeg"
        Me.lstSeg.Size = New System.Drawing.Size(252, 212)
        Me.lstSeg.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Agricultural practices"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(291, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Land Segments"
        '
        'Button1
        '
        Me.Button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button1.Location = New System.Drawing.Point(197, 489)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(79, 37)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button2.Location = New System.Drawing.Point(289, 489)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(79, 37)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(188, 18)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(24, 13)
        Me.Label13.TabIndex = 39
        Me.Label13.Text = "Min"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(147, 18)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(18, 13)
        Me.Label12.TabIndex = 38
        Me.Label12.Text = "Hr"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(106, 18)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(26, 13)
        Me.Label11.TabIndex = 37
        Me.Label11.Text = "Day"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(65, 18)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(22, 13)
        Me.Label10.TabIndex = 36
        Me.Label10.Text = "Mo"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(15, 18)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(29, 13)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "Year"
        '
        'comboRep
        '
        Me.comboRep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboRep.FormattingEnabled = True
        Me.comboRep.Location = New System.Drawing.Point(244, 36)
        Me.comboRep.Name = "comboRep"
        Me.comboRep.Size = New System.Drawing.Size(94, 21)
        Me.comboRep.TabIndex = 34
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(241, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 41
        Me.Label3.Text = "Repeat Interval"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(15, 73)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(99, 17)
        Me.CheckBox1.TabIndex = 44
        Me.CheckBox1.Text = "Defer if Raining"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'GroupProps
        '
        Me.GroupProps.Controls.Add(Me.txtStartMin)
        Me.GroupProps.Controls.Add(Me.CheckBox1)
        Me.GroupProps.Controls.Add(Me.comboRep)
        Me.GroupProps.Controls.Add(Me.Label9)
        Me.GroupProps.Controls.Add(Me.Label10)
        Me.GroupProps.Controls.Add(Me.Label3)
        Me.GroupProps.Controls.Add(Me.Label11)
        Me.GroupProps.Controls.Add(Me.Label12)
        Me.GroupProps.Controls.Add(Me.Label13)
        Me.GroupProps.Controls.Add(Me.txtStartHr)
        Me.GroupProps.Controls.Add(Me.txtStartYr)
        Me.GroupProps.Controls.Add(Me.txtStartDay)
        Me.GroupProps.Controls.Add(Me.txtStartMo)
        Me.GroupProps.Location = New System.Drawing.Point(24, 263)
        Me.GroupProps.Name = "GroupProps"
        Me.GroupProps.Size = New System.Drawing.Size(348, 96)
        Me.GroupProps.TabIndex = 45
        Me.GroupProps.TabStop = False
        Me.GroupProps.Text = "Properties"
        '
        'GroupLayers
        '
        Me.GroupLayers.Controls.Add(Me.Label8)
        Me.GroupLayers.Controls.Add(Me.AtcText5)
        Me.GroupLayers.Controls.Add(Me.AtcText2)
        Me.GroupLayers.Controls.Add(Me.Label14)
        Me.GroupLayers.Location = New System.Drawing.Point(23, 373)
        Me.GroupLayers.Name = "GroupLayers"
        Me.GroupLayers.Size = New System.Drawing.Size(255, 92)
        Me.GroupLayers.TabIndex = 46
        Me.GroupLayers.TabStop = False
        Me.GroupLayers.Text = "Layer Split"
        '
        'GroupPar
        '
        Me.GroupPar.Controls.Add(Me.Label4)
        Me.GroupPar.Controls.Add(Me.AtcText4)
        Me.GroupPar.Controls.Add(Me.Label6)
        Me.GroupPar.Controls.Add(Me.AtcText3)
        Me.GroupPar.Location = New System.Drawing.Point(376, 263)
        Me.GroupPar.Name = "GroupPar"
        Me.GroupPar.Size = New System.Drawing.Size(167, 96)
        Me.GroupPar.TabIndex = 47
        Me.GroupPar.TabStop = False
        Me.GroupPar.Text = "Parameter Value"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(27, 23)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(29, 13)
        Me.Label6.TabIndex = 45
        Me.Label6.Text = "Par1"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(64, 57)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(36, 13)
        Me.Label8.TabIndex = 51
        Me.Label8.Text = "Upper"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(64, 23)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(44, 13)
        Me.Label14.TabIndex = 49
        Me.Label14.Text = "Surface"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(26, 57)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(29, 13)
        Me.Label4.TabIndex = 47
        Me.Label4.Text = "Par2"
        '
        'AtcText4
        '
        Me.AtcText4.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText4.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText4.DefaultValue = 0
        Me.AtcText4.HardMax = 9999
        Me.AtcText4.HardMin = 0
        Me.AtcText4.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText4.Location = New System.Drawing.Point(65, 54)
        Me.AtcText4.MaxDecimal = 0
        Me.AtcText4.maxWidth = 0
        Me.AtcText4.Name = "AtcText4"
        Me.AtcText4.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText4.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText4.SelLength = 0
        Me.AtcText4.SelStart = 1
        Me.AtcText4.Size = New System.Drawing.Size(76, 19)
        Me.AtcText4.SoftMax = 5
        Me.AtcText4.SoftMin = 1
        Me.AtcText4.TabIndex = 46
        Me.AtcText4.Tag = "1111"
        Me.AtcText4.Value = CType(0, Long)
        '
        'AtcText3
        '
        Me.AtcText3.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText3.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText3.DefaultValue = 0
        Me.AtcText3.HardMax = 9999
        Me.AtcText3.HardMin = 0
        Me.AtcText3.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText3.Location = New System.Drawing.Point(65, 20)
        Me.AtcText3.MaxDecimal = 0
        Me.AtcText3.maxWidth = 0
        Me.AtcText3.Name = "AtcText3"
        Me.AtcText3.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText3.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText3.SelLength = 0
        Me.AtcText3.SelStart = 1
        Me.AtcText3.Size = New System.Drawing.Size(76, 19)
        Me.AtcText3.SoftMax = 5
        Me.AtcText3.SoftMin = 1
        Me.AtcText3.TabIndex = 30
        Me.AtcText3.Tag = "1111"
        Me.AtcText3.Value = CType(0, Long)
        '
        'AtcText5
        '
        Me.AtcText5.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText5.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText5.DefaultValue = 0
        Me.AtcText5.HardMax = 9999
        Me.AtcText5.HardMin = 0
        Me.AtcText5.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText5.Location = New System.Drawing.Point(110, 20)
        Me.AtcText5.MaxDecimal = 0
        Me.AtcText5.maxWidth = 0
        Me.AtcText5.Name = "AtcText5"
        Me.AtcText5.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText5.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText5.SelLength = 0
        Me.AtcText5.SelStart = 1
        Me.AtcText5.Size = New System.Drawing.Size(76, 19)
        Me.AtcText5.SoftMax = 5
        Me.AtcText5.SoftMin = 1
        Me.AtcText5.TabIndex = 48
        Me.AtcText5.Tag = "1111"
        Me.AtcText5.Value = CType(0, Long)
        '
        'AtcText2
        '
        Me.AtcText2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText2.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText2.DefaultValue = 0
        Me.AtcText2.HardMax = 9999
        Me.AtcText2.HardMin = 0
        Me.AtcText2.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText2.Location = New System.Drawing.Point(110, 54)
        Me.AtcText2.MaxDecimal = 0
        Me.AtcText2.maxWidth = 0
        Me.AtcText2.Name = "AtcText2"
        Me.AtcText2.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText2.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText2.SelLength = 0
        Me.AtcText2.SelStart = 1
        Me.AtcText2.Size = New System.Drawing.Size(76, 19)
        Me.AtcText2.SoftMax = 5
        Me.AtcText2.SoftMin = 1
        Me.AtcText2.TabIndex = 50
        Me.AtcText2.Tag = "1111"
        Me.AtcText2.Value = CType(0, Long)
        '
        'txtStartMin
        '
        Me.txtStartMin.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartMin.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartMin.DefaultValue = 0
        Me.txtStartMin.HardMax = 59
        Me.txtStartMin.HardMin = 0
        Me.txtStartMin.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartMin.Location = New System.Drawing.Point(188, 36)
        Me.txtStartMin.MaxDecimal = 0
        Me.txtStartMin.maxWidth = 0
        Me.txtStartMin.Name = "txtStartMin"
        Me.txtStartMin.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMin.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMin.SelLength = 0
        Me.txtStartMin.SelStart = 1
        Me.txtStartMin.Size = New System.Drawing.Size(35, 19)
        Me.txtStartMin.SoftMax = 0
        Me.txtStartMin.SoftMin = 0
        Me.txtStartMin.TabIndex = 33
        Me.txtStartMin.Value = CType(0, Long)
        '
        'txtStartHr
        '
        Me.txtStartHr.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartHr.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartHr.DefaultValue = 0
        Me.txtStartHr.HardMax = 24
        Me.txtStartHr.HardMin = 0
        Me.txtStartHr.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartHr.Location = New System.Drawing.Point(147, 36)
        Me.txtStartHr.MaxDecimal = 0
        Me.txtStartHr.maxWidth = 0
        Me.txtStartHr.Name = "txtStartHr"
        Me.txtStartHr.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartHr.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartHr.SelLength = 0
        Me.txtStartHr.SelStart = 1
        Me.txtStartHr.Size = New System.Drawing.Size(35, 19)
        Me.txtStartHr.SoftMax = 0
        Me.txtStartHr.SoftMin = 0
        Me.txtStartHr.TabIndex = 32
        Me.txtStartHr.Value = CType(0, Long)
        '
        'txtStartYr
        '
        Me.txtStartYr.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartYr.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartYr.DefaultValue = 0
        Me.txtStartYr.HardMax = 9999
        Me.txtStartYr.HardMin = 0
        Me.txtStartYr.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartYr.Location = New System.Drawing.Point(15, 36)
        Me.txtStartYr.MaxDecimal = 0
        Me.txtStartYr.maxWidth = 0
        Me.txtStartYr.Name = "txtStartYr"
        Me.txtStartYr.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartYr.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartYr.SelLength = 0
        Me.txtStartYr.SelStart = 1
        Me.txtStartYr.Size = New System.Drawing.Size(44, 19)
        Me.txtStartYr.SoftMax = 5
        Me.txtStartYr.SoftMin = 1
        Me.txtStartYr.TabIndex = 29
        Me.txtStartYr.Tag = "1111"
        Me.txtStartYr.Value = CType(0, Long)
        '
        'txtStartDay
        '
        Me.txtStartDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartDay.DefaultValue = 1
        Me.txtStartDay.HardMax = 31
        Me.txtStartDay.HardMin = 1
        Me.txtStartDay.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartDay.Location = New System.Drawing.Point(106, 36)
        Me.txtStartDay.MaxDecimal = 0
        Me.txtStartDay.maxWidth = 0
        Me.txtStartDay.Name = "txtStartDay"
        Me.txtStartDay.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartDay.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartDay.SelLength = 0
        Me.txtStartDay.SelStart = 1
        Me.txtStartDay.Size = New System.Drawing.Size(35, 19)
        Me.txtStartDay.SoftMax = 0
        Me.txtStartDay.SoftMin = 0
        Me.txtStartDay.TabIndex = 31
        Me.txtStartDay.Value = CType(1, Long)
        '
        'txtStartMo
        '
        Me.txtStartMo.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartMo.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartMo.DefaultValue = 1
        Me.txtStartMo.HardMax = 12
        Me.txtStartMo.HardMin = 1
        Me.txtStartMo.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartMo.Location = New System.Drawing.Point(65, 36)
        Me.txtStartMo.MaxDecimal = 0
        Me.txtStartMo.maxWidth = 0
        Me.txtStartMo.Name = "txtStartMo"
        Me.txtStartMo.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMo.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMo.SelLength = 0
        Me.txtStartMo.SelStart = 1
        Me.txtStartMo.Size = New System.Drawing.Size(35, 19)
        Me.txtStartMo.SoftMax = 0
        Me.txtStartMo.SoftMin = 0
        Me.txtStartMo.TabIndex = 30
        Me.txtStartMo.Value = CType(1, Long)
        '
        'frmAgPrac
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(565, 535)
        Me.Controls.Add(Me.GroupPar)
        Me.Controls.Add(Me.GroupLayers)
        Me.Controls.Add(Me.GroupProps)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lstSeg)
        Me.Controls.Add(Me.lstPrac)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "frmAgPrac"
        Me.Text = "Add Pre-defined Agricultural Practice"
        Me.GroupProps.ResumeLayout(False)
        Me.GroupProps.PerformLayout()
        Me.GroupLayers.ResumeLayout(False)
        Me.GroupLayers.PerformLayout()
        Me.GroupPar.ResumeLayout(False)
        Me.GroupPar.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstPrac As System.Windows.Forms.ListBox
    Friend WithEvents lstSeg As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents txtStartMin As atcControls.atcText
    Friend WithEvents txtStartHr As atcControls.atcText
    Friend WithEvents txtStartDay As atcControls.atcText
    Friend WithEvents txtStartMo As atcControls.atcText
    Friend WithEvents txtStartYr As atcControls.atcText
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents comboRep As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents GroupProps As System.Windows.Forms.GroupBox
    Friend WithEvents GroupLayers As System.Windows.Forms.GroupBox
    Friend WithEvents GroupPar As System.Windows.Forms.GroupBox
    Friend WithEvents AtcText3 As atcControls.atcText
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents AtcText5 As atcControls.atcText
    Friend WithEvents AtcText2 As atcControls.atcText
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents AtcText4 As atcControls.atcText
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
