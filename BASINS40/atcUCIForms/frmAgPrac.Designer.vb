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
        Me.chkDelay = New System.Windows.Forms.CheckBox
        Me.GroupProps = New System.Windows.Forms.GroupBox
        Me.atxMin = New atcControls.atcText
        Me.atxHr = New atcControls.atcText
        Me.atxYear = New atcControls.atcText
        Me.atxDay = New atcControls.atcText
        Me.atxMo = New atcControls.atcText
        Me.GroupLayers = New System.Windows.Forms.GroupBox
        Me.txtNA = New System.Windows.Forms.Label
        Me.txtUpper = New System.Windows.Forms.Label
        Me.atxSurface = New atcControls.atcText
        Me.atxUpper = New atcControls.atcText
        Me.txtSurface = New System.Windows.Forms.Label
        Me.GroupPar = New System.Windows.Forms.GroupBox
        Me.ParGrid = New atcControls.atcGrid
        Me.GroupProps.SuspendLayout()
        Me.GroupLayers.SuspendLayout()
        Me.GroupPar.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstPrac
        '
        Me.lstPrac.FormattingEnabled = True
        Me.lstPrac.Location = New System.Drawing.Point(24, 34)
        Me.lstPrac.Name = "lstPrac"
        Me.lstPrac.Size = New System.Drawing.Size(199, 212)
        Me.lstPrac.TabIndex = 0
        '
        'lstSeg
        '
        Me.lstSeg.FormattingEnabled = True
        Me.lstSeg.Location = New System.Drawing.Point(232, 34)
        Me.lstSeg.Name = "lstSeg"
        Me.lstSeg.Size = New System.Drawing.Size(199, 212)
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
        Me.Label2.Location = New System.Drawing.Point(229, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Land Segments"
        '
        'Button1
        '
        Me.Button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button1.Location = New System.Drawing.Point(142, 484)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(79, 37)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button2.Location = New System.Drawing.Point(234, 484)
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
        Me.Label13.Location = New System.Drawing.Point(216, 18)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(24, 13)
        Me.Label13.TabIndex = 39
        Me.Label13.Text = "Min"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(175, 18)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(18, 13)
        Me.Label12.TabIndex = 38
        Me.Label12.Text = "Hr"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(134, 18)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(26, 13)
        Me.Label11.TabIndex = 37
        Me.Label11.Text = "Day"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(93, 18)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(22, 13)
        Me.Label10.TabIndex = 36
        Me.Label10.Text = "Mo"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(43, 18)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(29, 13)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "Year"
        '
        'comboRep
        '
        Me.comboRep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboRep.FormattingEnabled = True
        Me.comboRep.Location = New System.Drawing.Point(272, 36)
        Me.comboRep.Name = "comboRep"
        Me.comboRep.Size = New System.Drawing.Size(94, 21)
        Me.comboRep.TabIndex = 34
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(269, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 41
        Me.Label3.Text = "Repeat Interval"
        '
        'chkDelay
        '
        Me.chkDelay.AutoSize = True
        Me.chkDelay.Location = New System.Drawing.Point(43, 65)
        Me.chkDelay.Name = "chkDelay"
        Me.chkDelay.Size = New System.Drawing.Size(99, 17)
        Me.chkDelay.TabIndex = 44
        Me.chkDelay.Text = "Defer if Raining"
        Me.chkDelay.UseVisualStyleBackColor = True
        '
        'GroupProps
        '
        Me.GroupProps.Controls.Add(Me.atxMin)
        Me.GroupProps.Controls.Add(Me.chkDelay)
        Me.GroupProps.Controls.Add(Me.comboRep)
        Me.GroupProps.Controls.Add(Me.Label9)
        Me.GroupProps.Controls.Add(Me.Label10)
        Me.GroupProps.Controls.Add(Me.Label3)
        Me.GroupProps.Controls.Add(Me.Label11)
        Me.GroupProps.Controls.Add(Me.Label12)
        Me.GroupProps.Controls.Add(Me.Label13)
        Me.GroupProps.Controls.Add(Me.atxHr)
        Me.GroupProps.Controls.Add(Me.atxYear)
        Me.GroupProps.Controls.Add(Me.atxDay)
        Me.GroupProps.Controls.Add(Me.atxMo)
        Me.GroupProps.Location = New System.Drawing.Point(24, 252)
        Me.GroupProps.Name = "GroupProps"
        Me.GroupProps.Size = New System.Drawing.Size(409, 93)
        Me.GroupProps.TabIndex = 45
        Me.GroupProps.TabStop = False
        Me.GroupProps.Text = "Properties"
        '
        'atxMin
        '
        Me.atxMin.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxMin.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxMin.DefaultValue = 0
        Me.atxMin.HardMax = 59
        Me.atxMin.HardMin = 0
        Me.atxMin.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxMin.Location = New System.Drawing.Point(216, 36)
        Me.atxMin.MaxDecimal = 0
        Me.atxMin.maxWidth = 0
        Me.atxMin.Name = "atxMin"
        Me.atxMin.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxMin.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxMin.SelLength = 0
        Me.atxMin.SelStart = 1
        Me.atxMin.Size = New System.Drawing.Size(35, 19)
        Me.atxMin.SoftMax = 0
        Me.atxMin.SoftMin = 0
        Me.atxMin.TabIndex = 33
        Me.atxMin.Value = CType(0, Long)
        '
        'atxHr
        '
        Me.atxHr.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxHr.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxHr.DefaultValue = 0
        Me.atxHr.HardMax = 24
        Me.atxHr.HardMin = 0
        Me.atxHr.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxHr.Location = New System.Drawing.Point(175, 36)
        Me.atxHr.MaxDecimal = 0
        Me.atxHr.maxWidth = 0
        Me.atxHr.Name = "atxHr"
        Me.atxHr.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxHr.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxHr.SelLength = 0
        Me.atxHr.SelStart = 1
        Me.atxHr.Size = New System.Drawing.Size(35, 19)
        Me.atxHr.SoftMax = 0
        Me.atxHr.SoftMin = 0
        Me.atxHr.TabIndex = 32
        Me.atxHr.Value = CType(0, Long)
        '
        'atxYear
        '
        Me.atxYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxYear.DefaultValue = 0
        Me.atxYear.HardMax = 9999
        Me.atxYear.HardMin = 0
        Me.atxYear.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxYear.Location = New System.Drawing.Point(43, 36)
        Me.atxYear.MaxDecimal = 0
        Me.atxYear.maxWidth = 0
        Me.atxYear.Name = "atxYear"
        Me.atxYear.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxYear.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxYear.SelLength = 0
        Me.atxYear.SelStart = 1
        Me.atxYear.Size = New System.Drawing.Size(44, 19)
        Me.atxYear.SoftMax = 5
        Me.atxYear.SoftMin = 1
        Me.atxYear.TabIndex = 29
        Me.atxYear.Tag = "1111"
        Me.atxYear.Value = CType(0, Long)
        '
        'atxDay
        '
        Me.atxDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxDay.DefaultValue = 1
        Me.atxDay.HardMax = 31
        Me.atxDay.HardMin = 1
        Me.atxDay.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxDay.Location = New System.Drawing.Point(134, 36)
        Me.atxDay.MaxDecimal = 0
        Me.atxDay.maxWidth = 0
        Me.atxDay.Name = "atxDay"
        Me.atxDay.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxDay.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxDay.SelLength = 0
        Me.atxDay.SelStart = 1
        Me.atxDay.Size = New System.Drawing.Size(35, 19)
        Me.atxDay.SoftMax = 0
        Me.atxDay.SoftMin = 0
        Me.atxDay.TabIndex = 31
        Me.atxDay.Value = CType(1, Long)
        '
        'atxMo
        '
        Me.atxMo.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxMo.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxMo.DefaultValue = 1
        Me.atxMo.HardMax = 12
        Me.atxMo.HardMin = 1
        Me.atxMo.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxMo.Location = New System.Drawing.Point(93, 36)
        Me.atxMo.MaxDecimal = 0
        Me.atxMo.maxWidth = 0
        Me.atxMo.Name = "atxMo"
        Me.atxMo.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxMo.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxMo.SelLength = 0
        Me.atxMo.SelStart = 1
        Me.atxMo.Size = New System.Drawing.Size(35, 19)
        Me.atxMo.SoftMax = 0
        Me.atxMo.SoftMin = 0
        Me.atxMo.TabIndex = 30
        Me.atxMo.Value = CType(1, Long)
        '
        'GroupLayers
        '
        Me.GroupLayers.Controls.Add(Me.txtNA)
        Me.GroupLayers.Controls.Add(Me.txtUpper)
        Me.GroupLayers.Controls.Add(Me.atxSurface)
        Me.GroupLayers.Controls.Add(Me.atxUpper)
        Me.GroupLayers.Controls.Add(Me.txtSurface)
        Me.GroupLayers.Location = New System.Drawing.Point(268, 351)
        Me.GroupLayers.Name = "GroupLayers"
        Me.GroupLayers.Size = New System.Drawing.Size(165, 125)
        Me.GroupLayers.TabIndex = 46
        Me.GroupLayers.TabStop = False
        Me.GroupLayers.Text = "Layer Split"
        '
        'txtNA
        '
        Me.txtNA.AutoSize = True
        Me.txtNA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNA.Location = New System.Drawing.Point(44, 56)
        Me.txtNA.Name = "txtNA"
        Me.txtNA.Size = New System.Drawing.Size(76, 13)
        Me.txtNA.TabIndex = 52
        Me.txtNA.Text = "Not Applicable"
        '
        'txtUpper
        '
        Me.txtUpper.AutoSize = True
        Me.txtUpper.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUpper.Location = New System.Drawing.Point(21, 76)
        Me.txtUpper.Name = "txtUpper"
        Me.txtUpper.Size = New System.Drawing.Size(36, 13)
        Me.txtUpper.TabIndex = 51
        Me.txtUpper.Text = "Upper"
        '
        'atxSurface
        '
        Me.atxSurface.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSurface.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSurface.DefaultValue = 0
        Me.atxSurface.HardMax = 9999
        Me.atxSurface.HardMin = 0
        Me.atxSurface.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxSurface.Location = New System.Drawing.Point(67, 33)
        Me.atxSurface.MaxDecimal = 0
        Me.atxSurface.maxWidth = 0
        Me.atxSurface.Name = "atxSurface"
        Me.atxSurface.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxSurface.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxSurface.SelLength = 0
        Me.atxSurface.SelStart = 1
        Me.atxSurface.Size = New System.Drawing.Size(76, 19)
        Me.atxSurface.SoftMax = 5
        Me.atxSurface.SoftMin = 1
        Me.atxSurface.TabIndex = 48
        Me.atxSurface.Tag = "1111"
        Me.atxSurface.Value = CType(0, Long)
        '
        'atxUpper
        '
        Me.atxUpper.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxUpper.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxUpper.DefaultValue = 0
        Me.atxUpper.HardMax = 9999
        Me.atxUpper.HardMin = 0
        Me.atxUpper.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxUpper.Location = New System.Drawing.Point(67, 73)
        Me.atxUpper.MaxDecimal = 0
        Me.atxUpper.maxWidth = 0
        Me.atxUpper.Name = "atxUpper"
        Me.atxUpper.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxUpper.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxUpper.SelLength = 0
        Me.atxUpper.SelStart = 1
        Me.atxUpper.Size = New System.Drawing.Size(76, 19)
        Me.atxUpper.SoftMax = 5
        Me.atxUpper.SoftMin = 1
        Me.atxUpper.TabIndex = 50
        Me.atxUpper.Tag = "1111"
        Me.atxUpper.Value = CType(0, Long)
        '
        'txtSurface
        '
        Me.txtSurface.AutoSize = True
        Me.txtSurface.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSurface.Location = New System.Drawing.Point(21, 36)
        Me.txtSurface.Name = "txtSurface"
        Me.txtSurface.Size = New System.Drawing.Size(44, 13)
        Me.txtSurface.TabIndex = 49
        Me.txtSurface.Text = "Surface"
        '
        'GroupPar
        '
        Me.GroupPar.Controls.Add(Me.ParGrid)
        Me.GroupPar.Location = New System.Drawing.Point(25, 351)
        Me.GroupPar.Name = "GroupPar"
        Me.GroupPar.Size = New System.Drawing.Size(237, 125)
        Me.GroupPar.TabIndex = 47
        Me.GroupPar.TabStop = False
        Me.GroupPar.Text = "Parameters"
        '
        'ParGrid
        '
        Me.ParGrid.AllowHorizontalScrolling = True
        Me.ParGrid.AllowNewValidValues = False
        Me.ParGrid.CellBackColor = System.Drawing.Color.Empty
        Me.ParGrid.LineColor = System.Drawing.Color.Empty
        Me.ParGrid.LineWidth = 0.0!
        Me.ParGrid.Location = New System.Drawing.Point(41, 17)
        Me.ParGrid.Name = "ParGrid"
        Me.ParGrid.Size = New System.Drawing.Size(154, 100)
        Me.ParGrid.Source = Nothing
        Me.ParGrid.TabIndex = 0
        '
        'frmAgPrac
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(455, 528)
        Me.Controls.Add(Me.GroupPar)
        Me.Controls.Add(Me.GroupLayers)
        Me.Controls.Add(Me.GroupProps)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lstSeg)
        Me.Controls.Add(Me.lstPrac)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmAgPrac"
        Me.Text = "Add Pre-defined Agricultural Practice"
        Me.GroupProps.ResumeLayout(False)
        Me.GroupProps.PerformLayout()
        Me.GroupLayers.ResumeLayout(False)
        Me.GroupLayers.PerformLayout()
        Me.GroupPar.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstPrac As System.Windows.Forms.ListBox
    Friend WithEvents lstSeg As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents atxMin As atcControls.atcText
    Friend WithEvents atxHr As atcControls.atcText
    Friend WithEvents atxDay As atcControls.atcText
    Friend WithEvents atxMo As atcControls.atcText
    Friend WithEvents atxYear As atcControls.atcText
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents comboRep As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkDelay As System.Windows.Forms.CheckBox
    Friend WithEvents GroupProps As System.Windows.Forms.GroupBox
    Friend WithEvents GroupLayers As System.Windows.Forms.GroupBox
    Friend WithEvents GroupPar As System.Windows.Forms.GroupBox
    Friend WithEvents txtUpper As System.Windows.Forms.Label
    Friend WithEvents atxSurface As atcControls.atcText
    Friend WithEvents atxUpper As atcControls.atcText
    Friend WithEvents txtSurface As System.Windows.Forms.Label
    Friend WithEvents ParGrid As atcControls.atcGrid
    Friend WithEvents txtNA As System.Windows.Forms.Label
End Class
