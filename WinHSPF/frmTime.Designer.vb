<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTime
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
        Me.txtEndMinute = New atcControls.atcText
        Me.txtStartMinute = New atcControls.atcText
        Me.txtEndHour = New atcControls.atcText
        Me.txtStartHour = New atcControls.atcText
        Me.txtEndDay = New atcControls.atcText
        Me.txtStartDay = New atcControls.atcText
        Me.txtEndMonth = New atcControls.atcText
        Me.txtStartMonth = New atcControls.atcText
        Me.txtEndYear = New atcControls.atcText
        Me.txtStartYear = New atcControls.atcText
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.agdMet = New atcControls.atcGrid
        Me.Cancel = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtEndMinute
        '
        Me.txtEndMinute.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndMinute.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndMinute.DefaultValue = "0"
        Me.txtEndMinute.HardMax = 59
        Me.txtEndMinute.HardMin = 0
        Me.txtEndMinute.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndMinute.Location = New System.Drawing.Point(326, 49)
        Me.txtEndMinute.MaxWidth = 0
        Me.txtEndMinute.Name = "txtEndMinute"
        Me.txtEndMinute.NumericFormat = "0.#####"
        Me.txtEndMinute.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndMinute.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndMinute.SelLength = 1
        Me.txtEndMinute.SelStart = 0
        Me.txtEndMinute.Size = New System.Drawing.Size(55, 20)
        Me.txtEndMinute.SoftMax = 0
        Me.txtEndMinute.SoftMin = 0
        Me.txtEndMinute.TabIndex = 40
        Me.txtEndMinute.ValueDouble = 0
        Me.txtEndMinute.ValueInteger = 0
        '
        'txtStartMinute
        '
        Me.txtStartMinute.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartMinute.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartMinute.DefaultValue = "0"
        Me.txtStartMinute.HardMax = 59
        Me.txtStartMinute.HardMin = 0
        Me.txtStartMinute.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartMinute.Location = New System.Drawing.Point(326, 25)
        Me.txtStartMinute.MaxWidth = 0
        Me.txtStartMinute.Name = "txtStartMinute"
        Me.txtStartMinute.NumericFormat = "0.#####"
        Me.txtStartMinute.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMinute.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMinute.SelLength = 1
        Me.txtStartMinute.SelStart = 0
        Me.txtStartMinute.Size = New System.Drawing.Size(55, 20)
        Me.txtStartMinute.SoftMax = 0
        Me.txtStartMinute.SoftMin = 0
        Me.txtStartMinute.TabIndex = 35
        Me.txtStartMinute.ValueDouble = 0
        Me.txtStartMinute.ValueInteger = 0
        '
        'txtEndHour
        '
        Me.txtEndHour.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndHour.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndHour.DefaultValue = "0"
        Me.txtEndHour.HardMax = 24
        Me.txtEndHour.HardMin = 0
        Me.txtEndHour.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndHour.Location = New System.Drawing.Point(264, 49)
        Me.txtEndHour.MaxWidth = 0
        Me.txtEndHour.Name = "txtEndHour"
        Me.txtEndHour.NumericFormat = "0.#####"
        Me.txtEndHour.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndHour.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndHour.SelLength = 1
        Me.txtEndHour.SelStart = 0
        Me.txtEndHour.Size = New System.Drawing.Size(55, 20)
        Me.txtEndHour.SoftMax = 0
        Me.txtEndHour.SoftMin = 0
        Me.txtEndHour.TabIndex = 39
        Me.txtEndHour.ValueDouble = 0
        Me.txtEndHour.ValueInteger = 0
        '
        'txtStartHour
        '
        Me.txtStartHour.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartHour.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartHour.DefaultValue = "0"
        Me.txtStartHour.HardMax = 24
        Me.txtStartHour.HardMin = 0
        Me.txtStartHour.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartHour.Location = New System.Drawing.Point(264, 25)
        Me.txtStartHour.MaxWidth = 0
        Me.txtStartHour.Name = "txtStartHour"
        Me.txtStartHour.NumericFormat = "0.#####"
        Me.txtStartHour.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartHour.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartHour.SelLength = 1
        Me.txtStartHour.SelStart = 0
        Me.txtStartHour.Size = New System.Drawing.Size(55, 20)
        Me.txtStartHour.SoftMax = 0
        Me.txtStartHour.SoftMin = 0
        Me.txtStartHour.TabIndex = 34
        Me.txtStartHour.ValueDouble = 0
        Me.txtStartHour.ValueInteger = 0
        '
        'txtEndDay
        '
        Me.txtEndDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndDay.DefaultValue = "1"
        Me.txtEndDay.HardMax = 31
        Me.txtEndDay.HardMin = 1
        Me.txtEndDay.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndDay.Location = New System.Drawing.Point(202, 49)
        Me.txtEndDay.MaxWidth = 0
        Me.txtEndDay.Name = "txtEndDay"
        Me.txtEndDay.NumericFormat = "0.#####"
        Me.txtEndDay.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndDay.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndDay.SelLength = 1
        Me.txtEndDay.SelStart = 0
        Me.txtEndDay.Size = New System.Drawing.Size(55, 20)
        Me.txtEndDay.SoftMax = 0
        Me.txtEndDay.SoftMin = 0
        Me.txtEndDay.TabIndex = 38
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
        Me.txtStartDay.Location = New System.Drawing.Point(202, 25)
        Me.txtStartDay.MaxWidth = 0
        Me.txtStartDay.Name = "txtStartDay"
        Me.txtStartDay.NumericFormat = "0.#####"
        Me.txtStartDay.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartDay.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartDay.SelLength = 1
        Me.txtStartDay.SelStart = 0
        Me.txtStartDay.Size = New System.Drawing.Size(55, 20)
        Me.txtStartDay.SoftMax = 0
        Me.txtStartDay.SoftMin = 0
        Me.txtStartDay.TabIndex = 32
        Me.txtStartDay.ValueDouble = 1
        Me.txtStartDay.ValueInteger = 1
        '
        'txtEndMonth
        '
        Me.txtEndMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndMonth.DefaultValue = "1"
        Me.txtEndMonth.HardMax = 12
        Me.txtEndMonth.HardMin = 1
        Me.txtEndMonth.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndMonth.Location = New System.Drawing.Point(141, 51)
        Me.txtEndMonth.MaxWidth = 0
        Me.txtEndMonth.Name = "txtEndMonth"
        Me.txtEndMonth.NumericFormat = "0.#####"
        Me.txtEndMonth.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndMonth.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndMonth.SelLength = 1
        Me.txtEndMonth.SelStart = 0
        Me.txtEndMonth.Size = New System.Drawing.Size(55, 20)
        Me.txtEndMonth.SoftMax = 0
        Me.txtEndMonth.SoftMin = 0
        Me.txtEndMonth.TabIndex = 37
        Me.txtEndMonth.ValueDouble = 1
        Me.txtEndMonth.ValueInteger = 1
        '
        'txtStartMonth
        '
        Me.txtStartMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartMonth.DefaultValue = "1"
        Me.txtStartMonth.HardMax = 12
        Me.txtStartMonth.HardMin = 1
        Me.txtStartMonth.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartMonth.Location = New System.Drawing.Point(140, 25)
        Me.txtStartMonth.MaxWidth = 0
        Me.txtStartMonth.Name = "txtStartMonth"
        Me.txtStartMonth.NumericFormat = "0.#####"
        Me.txtStartMonth.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMonth.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartMonth.SelLength = 1
        Me.txtStartMonth.SelStart = 0
        Me.txtStartMonth.Size = New System.Drawing.Size(55, 20)
        Me.txtStartMonth.SoftMax = 0
        Me.txtStartMonth.SoftMin = 0
        Me.txtStartMonth.TabIndex = 30
        Me.txtStartMonth.ValueDouble = 1
        Me.txtStartMonth.ValueInteger = 1
        '
        'txtEndYear
        '
        Me.txtEndYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtEndYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtEndYear.DefaultValue = "0"
        Me.txtEndYear.HardMax = 9999
        Me.txtEndYear.HardMin = 0
        Me.txtEndYear.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtEndYear.Location = New System.Drawing.Point(78, 49)
        Me.txtEndYear.MaxWidth = 0
        Me.txtEndYear.Name = "txtEndYear"
        Me.txtEndYear.NumericFormat = "0.#####"
        Me.txtEndYear.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtEndYear.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtEndYear.SelLength = 1
        Me.txtEndYear.SelStart = 0
        Me.txtEndYear.Size = New System.Drawing.Size(55, 20)
        Me.txtEndYear.SoftMax = 3000
        Me.txtEndYear.SoftMin = 1000
        Me.txtEndYear.TabIndex = 36
        Me.txtEndYear.ValueDouble = 0
        Me.txtEndYear.ValueInteger = 0
        '
        'txtStartYear
        '
        Me.txtStartYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtStartYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtStartYear.DefaultValue = "0"
        Me.txtStartYear.HardMax = 9999
        Me.txtStartYear.HardMin = 0
        Me.txtStartYear.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtStartYear.Location = New System.Drawing.Point(78, 25)
        Me.txtStartYear.MaxWidth = 0
        Me.txtStartYear.Name = "txtStartYear"
        Me.txtStartYear.NumericFormat = "0.#####"
        Me.txtStartYear.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtStartYear.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtStartYear.SelLength = 1
        Me.txtStartYear.SelStart = 0
        Me.txtStartYear.Size = New System.Drawing.Size(55, 20)
        Me.txtStartYear.SoftMax = 3000
        Me.txtStartYear.SoftMin = 1000
        Me.txtStartYear.TabIndex = 29
        Me.txtStartYear.Tag = "1111"
        Me.txtStartYear.ValueDouble = 0
        Me.txtStartYear.ValueInteger = 0
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(334, 9)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(39, 13)
        Me.Label13.TabIndex = 45
        Me.Label13.Text = "Minute"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(276, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(30, 13)
        Me.Label12.TabIndex = 44
        Me.Label12.Text = "Hour"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(216, 9)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(26, 13)
        Me.Label11.TabIndex = 43
        Me.Label11.Text = "Day"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(149, 9)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(37, 13)
        Me.Label10.TabIndex = 42
        Me.Label10.Text = "Month"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(91, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(29, 13)
        Me.Label9.TabIndex = 41
        Me.Label9.Text = "Year"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(40, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "End:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(40, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 13)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Start:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.cmdApply)
        Me.GroupBox1.Controls.Add(Me.cmdEdit)
        Me.GroupBox1.Controls.Add(Me.cmdAdd)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.agdMet)
        Me.GroupBox1.Location = New System.Drawing.Point(19, 84)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(384, 370)
        Me.GroupBox1.TabIndex = 46
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Met Segments"
        '
        'cmdApply
        '
        Me.cmdApply.Location = New System.Drawing.Point(155, 30)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.Size = New System.Drawing.Size(67, 25)
        Me.cmdApply.TabIndex = 4
        Me.cmdApply.Text = "A&pply"
        Me.cmdApply.UseVisualStyleBackColor = True
        '
        'cmdEdit
        '
        Me.cmdEdit.Location = New System.Drawing.Point(82, 30)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(67, 25)
        Me.cmdEdit.TabIndex = 3
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.UseVisualStyleBackColor = True
        '
        'cmdAdd
        '
        Me.cmdAdd.Location = New System.Drawing.Point(9, 30)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(67, 25)
        Me.cmdAdd.TabIndex = 2
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Connections"
        '
        'agdMet
        '
        Me.agdMet.AllowHorizontalScrolling = True
        Me.agdMet.AllowNewValidValues = False
        Me.agdMet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdMet.CellBackColor = System.Drawing.Color.Empty
        Me.agdMet.Fixed3D = False
        Me.agdMet.LineColor = System.Drawing.Color.Empty
        Me.agdMet.LineWidth = 0.0!
        Me.agdMet.Location = New System.Drawing.Point(13, 88)
        Me.agdMet.Name = "agdMet"
        Me.agdMet.Size = New System.Drawing.Size(357, 271)
        Me.agdMet.Source = Nothing
        Me.agdMet.TabIndex = 0
        '
        'Cancel
        '
        Me.Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Cancel.Location = New System.Drawing.Point(217, 471)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(76, 25)
        Me.Cancel.TabIndex = 6
        Me.Cancel.Text = "&Cancel"
        Me.Cancel.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button2.Location = New System.Drawing.Point(129, 471)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(76, 25)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "&OK"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'frmTime
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(422, 508)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.txtEndMinute)
        Me.Controls.Add(Me.txtStartMinute)
        Me.Controls.Add(Me.txtEndHour)
        Me.Controls.Add(Me.txtStartHour)
        Me.Controls.Add(Me.txtEndDay)
        Me.Controls.Add(Me.txtStartDay)
        Me.Controls.Add(Me.txtEndMonth)
        Me.Controls.Add(Me.txtStartMonth)
        Me.Controls.Add(Me.txtEndYear)
        Me.Controls.Add(Me.txtStartYear)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Name = "frmTime"
        Me.Text = "WinHSPF - Simulation Time and Meteorologic Data"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtEndMinute As atcControls.atcText
    Friend WithEvents txtStartMinute As atcControls.atcText
    Friend WithEvents txtEndHour As atcControls.atcText
    Friend WithEvents txtStartHour As atcControls.atcText
    Friend WithEvents txtEndDay As atcControls.atcText
    Friend WithEvents txtStartDay As atcControls.atcText
    Friend WithEvents txtEndMonth As atcControls.atcText
    Friend WithEvents txtStartMonth As atcControls.atcText
    Friend WithEvents txtEndYear As atcControls.atcText
    Friend WithEvents txtStartYear As atcControls.atcText
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents agdMet As atcControls.atcGrid
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdEdit As System.Windows.Forms.Button
    Friend WithEvents cmdApply As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class
