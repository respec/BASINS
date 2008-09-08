<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNewFTable
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
        Me.cmdComputeFTable = New System.Windows.Forms.Button
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.cboProv = New System.Windows.Forms.ComboBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.atxChannelLength = New atcControls.atcText
        Me.atxChannelSlope = New atcControls.atcText
        Me.atxRightSideFloodPlainWidth = New atcControls.atcText
        Me.atxDrainageArea = New atcControls.atcText
        Me.atxChannelWidth = New atcControls.atcText
        Me.atxFloodplainSideSlope = New atcControls.atcText
        Me.atxChannelSideSlope = New atcControls.atcText
        Me.atxLeftSideFloodPlainWidth = New atcControls.atcText
        Me.atxMaximumFloodplainDepth = New atcControls.atcText
        Me.atxBankfullDepth = New atcControls.atcText
        Me.atxFloodplainManningsN = New atcControls.atcText
        Me.atxChannelManningsN = New atcControls.atcText
        Me.atxChannelDepth = New atcControls.atcText
        Me.AtcText12 = New atcControls.atcText
        Me.AtcText13 = New atcControls.atcText
        Me.AtcText22 = New atcControls.atcText
        Me.AtcText21 = New atcControls.atcText
        Me.AtcText1 = New atcControls.atcText
        Me.AtcText14 = New atcControls.atcText
        Me.AtcText15 = New atcControls.atcText
        Me.AtcText16 = New atcControls.atcText
        Me.AtcText17 = New atcControls.atcText
        Me.AtcText18 = New atcControls.atcText
        Me.AtcText19 = New atcControls.atcText
        Me.AtcText2 = New atcControls.atcText
        Me.AtcText20 = New atcControls.atcText
        Me.Label4 = New System.Windows.Forms.Label
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(34, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Average Channel Slope"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(33, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Channel Length, ft."
        '
        'cmdComputeFTable
        '
        Me.cmdComputeFTable.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdComputeFTable.Location = New System.Drawing.Point(183, 493)
        Me.cmdComputeFTable.Name = "cmdComputeFTable"
        Me.cmdComputeFTable.Size = New System.Drawing.Size(126, 30)
        Me.cmdComputeFTable.TabIndex = 30
        Me.cmdComputeFTable.Text = "Compute FTable"
        Me.cmdComputeFTable.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(161, 49)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(213, 21)
        Me.ComboBox1.TabIndex = 28
        '
        'Button2
        '
        Me.Button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button2.Location = New System.Drawing.Point(173, 501)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(126, 30)
        Me.Button2.TabIndex = 30
        Me.Button2.Text = "Compute FTable"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(90, 119)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(122, 13)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Mean Channel Width (ft)"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(90, 145)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(123, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Mean Channel Depth (ft)"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(90, 171)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(119, 13)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Manning N For Channel"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(90, 197)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(128, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Manning N For Floodplain"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(90, 223)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(92, 13)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Bankfull Depth (ft)"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(90, 249)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(149, 13)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "Maximum Floodplain Depth (ft)"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(90, 275)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(146, 13)
        Me.Label11.TabIndex = 11
        Me.Label11.Text = "Left Side Floodplain Width (ft)"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(90, 327)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(100, 13)
        Me.Label12.TabIndex = 12
        Me.Label12.Text = "Channel Side Slope"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(90, 353)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(109, 13)
        Me.Label13.TabIndex = 13
        Me.Label13.Text = "Floodplain Side Slope"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(90, 301)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(153, 13)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "Right Side Floodplain Width (ft)"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(19, 26)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(122, 13)
        Me.Label15.TabIndex = 3
        Me.Label15.Text = "Drainage Area (sq.Miles)"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(19, 62)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(118, 13)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "Physiographic Province"
        '
        'cboProv
        '
        Me.cboProv.FormattingEnabled = True
        Me.cboProv.Location = New System.Drawing.Point(191, 59)
        Me.cboProv.Name = "cboProv"
        Me.cboProv.Size = New System.Drawing.Size(213, 21)
        Me.cboProv.TabIndex = 28
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.atxRightSideFloodPlainWidth)
        Me.GroupBox2.Controls.Add(Me.cboProv)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.atxDrainageArea)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.atxChannelWidth)
        Me.GroupBox2.Controls.Add(Me.atxFloodplainSideSlope)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.atxChannelSideSlope)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.atxLeftSideFloodPlainWidth)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.atxMaximumFloodplainDepth)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.atxBankfullDepth)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.atxFloodplainManningsN)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.atxChannelManningsN)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.atxChannelDepth)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Location = New System.Drawing.Point(15, 94)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(456, 389)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Channel Characteristic Estimator"
        '
        'atxChannelLength
        '
        Me.atxChannelLength.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxChannelLength.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxChannelLength.DefaultValue = 0.0000099999997473787516
        Me.atxChannelLength.HardMax = 1000000000
        Me.atxChannelLength.HardMin = -1000000000
        Me.atxChannelLength.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxChannelLength.Location = New System.Drawing.Point(211, 52)
        Me.atxChannelLength.MaxDecimal = 0
        Me.atxChannelLength.maxWidth = 0
        Me.atxChannelLength.Name = "atxChannelLength"
        Me.atxChannelLength.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelLength.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelLength.SelLength = 0
        Me.atxChannelLength.SelStart = 20
        Me.atxChannelLength.Size = New System.Drawing.Size(93, 23)
        Me.atxChannelLength.SoftMax = -999
        Me.atxChannelLength.SoftMin = -999
        Me.atxChannelLength.TabIndex = 26
        Me.atxChannelLength.Value = 0.00001!
        '
        'atxChannelSlope
        '
        Me.atxChannelSlope.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxChannelSlope.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxChannelSlope.DefaultValue = 0.0000099999997473787516
        Me.atxChannelSlope.HardMax = 1000000000
        Me.atxChannelSlope.HardMin = -1000000000
        Me.atxChannelSlope.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxChannelSlope.Location = New System.Drawing.Point(211, 26)
        Me.atxChannelSlope.MaxDecimal = 0
        Me.atxChannelSlope.maxWidth = 0
        Me.atxChannelSlope.Name = "atxChannelSlope"
        Me.atxChannelSlope.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelSlope.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelSlope.SelLength = 0
        Me.atxChannelSlope.SelStart = 20
        Me.atxChannelSlope.Size = New System.Drawing.Size(93, 23)
        Me.atxChannelSlope.SoftMax = -999
        Me.atxChannelSlope.SoftMin = -999
        Me.atxChannelSlope.TabIndex = 25
        Me.atxChannelSlope.Value = 0.00001!
        '
        'atxRightSideFloodPlainWidth
        '
        Me.atxRightSideFloodPlainWidth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxRightSideFloodPlainWidth.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxRightSideFloodPlainWidth.DefaultValue = 0.0000099999997473787516
        Me.atxRightSideFloodPlainWidth.HardMax = 1000000000
        Me.atxRightSideFloodPlainWidth.HardMin = -1000000000
        Me.atxRightSideFloodPlainWidth.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxRightSideFloodPlainWidth.Location = New System.Drawing.Point(271, 296)
        Me.atxRightSideFloodPlainWidth.MaxDecimal = 0
        Me.atxRightSideFloodPlainWidth.maxWidth = 0
        Me.atxRightSideFloodPlainWidth.Name = "atxRightSideFloodPlainWidth"
        Me.atxRightSideFloodPlainWidth.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxRightSideFloodPlainWidth.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxRightSideFloodPlainWidth.SelLength = 0
        Me.atxRightSideFloodPlainWidth.SelStart = 20
        Me.atxRightSideFloodPlainWidth.Size = New System.Drawing.Size(93, 23)
        Me.atxRightSideFloodPlainWidth.SoftMax = -999
        Me.atxRightSideFloodPlainWidth.SoftMin = -999
        Me.atxRightSideFloodPlainWidth.TabIndex = 32
        Me.atxRightSideFloodPlainWidth.Value = 0.00001!
        '
        'atxDrainageArea
        '
        Me.atxDrainageArea.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxDrainageArea.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxDrainageArea.DefaultValue = 0.0000099999997473787516
        Me.atxDrainageArea.HardMax = 1000000000
        Me.atxDrainageArea.HardMin = -1000000000
        Me.atxDrainageArea.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxDrainageArea.Location = New System.Drawing.Point(195, 26)
        Me.atxDrainageArea.MaxDecimal = 0
        Me.atxDrainageArea.maxWidth = 0
        Me.atxDrainageArea.Name = "atxDrainageArea"
        Me.atxDrainageArea.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxDrainageArea.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxDrainageArea.SelLength = 0
        Me.atxDrainageArea.SelStart = 20
        Me.atxDrainageArea.Size = New System.Drawing.Size(93, 23)
        Me.atxDrainageArea.SoftMax = -999
        Me.atxDrainageArea.SoftMin = -999
        Me.atxDrainageArea.TabIndex = 27
        Me.atxDrainageArea.Value = 0.00001!
        '
        'atxChannelWidth
        '
        Me.atxChannelWidth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxChannelWidth.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxChannelWidth.DefaultValue = 0.0000099999997473787516
        Me.atxChannelWidth.HardMax = 1000000000
        Me.atxChannelWidth.HardMin = -1000000000
        Me.atxChannelWidth.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxChannelWidth.Location = New System.Drawing.Point(271, 114)
        Me.atxChannelWidth.MaxDecimal = 0
        Me.atxChannelWidth.maxWidth = 0
        Me.atxChannelWidth.Name = "atxChannelWidth"
        Me.atxChannelWidth.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelWidth.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelWidth.SelLength = 0
        Me.atxChannelWidth.SelStart = 20
        Me.atxChannelWidth.Size = New System.Drawing.Size(93, 23)
        Me.atxChannelWidth.SoftMax = -999
        Me.atxChannelWidth.SoftMin = -999
        Me.atxChannelWidth.TabIndex = 16
        Me.atxChannelWidth.Value = 0.00001!
        '
        'atxFloodplainSideSlope
        '
        Me.atxFloodplainSideSlope.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxFloodplainSideSlope.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxFloodplainSideSlope.DefaultValue = 0.0000099999997473787516
        Me.atxFloodplainSideSlope.HardMax = 1000000000
        Me.atxFloodplainSideSlope.HardMin = -1000000000
        Me.atxFloodplainSideSlope.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxFloodplainSideSlope.Location = New System.Drawing.Point(271, 348)
        Me.atxFloodplainSideSlope.MaxDecimal = 0
        Me.atxFloodplainSideSlope.maxWidth = 0
        Me.atxFloodplainSideSlope.Name = "atxFloodplainSideSlope"
        Me.atxFloodplainSideSlope.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxFloodplainSideSlope.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxFloodplainSideSlope.SelLength = 0
        Me.atxFloodplainSideSlope.SelStart = 20
        Me.atxFloodplainSideSlope.Size = New System.Drawing.Size(93, 23)
        Me.atxFloodplainSideSlope.SoftMax = -999
        Me.atxFloodplainSideSlope.SoftMin = -999
        Me.atxFloodplainSideSlope.TabIndex = 24
        Me.atxFloodplainSideSlope.Value = 0.00001!
        '
        'atxChannelSideSlope
        '
        Me.atxChannelSideSlope.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxChannelSideSlope.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxChannelSideSlope.DefaultValue = 0.0000099999997473787516
        Me.atxChannelSideSlope.HardMax = 1000000000
        Me.atxChannelSideSlope.HardMin = -1000000000
        Me.atxChannelSideSlope.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxChannelSideSlope.Location = New System.Drawing.Point(271, 322)
        Me.atxChannelSideSlope.MaxDecimal = 0
        Me.atxChannelSideSlope.maxWidth = 0
        Me.atxChannelSideSlope.Name = "atxChannelSideSlope"
        Me.atxChannelSideSlope.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelSideSlope.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelSideSlope.SelLength = 0
        Me.atxChannelSideSlope.SelStart = 20
        Me.atxChannelSideSlope.Size = New System.Drawing.Size(93, 23)
        Me.atxChannelSideSlope.SoftMax = -999
        Me.atxChannelSideSlope.SoftMin = -999
        Me.atxChannelSideSlope.TabIndex = 23
        Me.atxChannelSideSlope.Value = 0.00001!
        '
        'atxLeftSideFloodPlainWidth
        '
        Me.atxLeftSideFloodPlainWidth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxLeftSideFloodPlainWidth.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxLeftSideFloodPlainWidth.DefaultValue = 0.0000099999997473787516
        Me.atxLeftSideFloodPlainWidth.HardMax = 1000000000
        Me.atxLeftSideFloodPlainWidth.HardMin = -1000000000
        Me.atxLeftSideFloodPlainWidth.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxLeftSideFloodPlainWidth.Location = New System.Drawing.Point(271, 270)
        Me.atxLeftSideFloodPlainWidth.MaxDecimal = 0
        Me.atxLeftSideFloodPlainWidth.maxWidth = 0
        Me.atxLeftSideFloodPlainWidth.Name = "atxLeftSideFloodPlainWidth"
        Me.atxLeftSideFloodPlainWidth.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxLeftSideFloodPlainWidth.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxLeftSideFloodPlainWidth.SelLength = 0
        Me.atxLeftSideFloodPlainWidth.SelStart = 20
        Me.atxLeftSideFloodPlainWidth.Size = New System.Drawing.Size(93, 23)
        Me.atxLeftSideFloodPlainWidth.SoftMax = -999
        Me.atxLeftSideFloodPlainWidth.SoftMin = -999
        Me.atxLeftSideFloodPlainWidth.TabIndex = 22
        Me.atxLeftSideFloodPlainWidth.Value = 0.00001!
        '
        'atxMaximumFloodplainDepth
        '
        Me.atxMaximumFloodplainDepth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxMaximumFloodplainDepth.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxMaximumFloodplainDepth.DefaultValue = 0.0000099999997473787516
        Me.atxMaximumFloodplainDepth.HardMax = 1000000000
        Me.atxMaximumFloodplainDepth.HardMin = -1000000000
        Me.atxMaximumFloodplainDepth.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxMaximumFloodplainDepth.Location = New System.Drawing.Point(271, 244)
        Me.atxMaximumFloodplainDepth.MaxDecimal = 0
        Me.atxMaximumFloodplainDepth.maxWidth = 0
        Me.atxMaximumFloodplainDepth.Name = "atxMaximumFloodplainDepth"
        Me.atxMaximumFloodplainDepth.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxMaximumFloodplainDepth.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxMaximumFloodplainDepth.SelLength = 0
        Me.atxMaximumFloodplainDepth.SelStart = 20
        Me.atxMaximumFloodplainDepth.Size = New System.Drawing.Size(93, 23)
        Me.atxMaximumFloodplainDepth.SoftMax = -999
        Me.atxMaximumFloodplainDepth.SoftMin = -999
        Me.atxMaximumFloodplainDepth.TabIndex = 21
        Me.atxMaximumFloodplainDepth.Value = 0.00001!
        '
        'atxBankfullDepth
        '
        Me.atxBankfullDepth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxBankfullDepth.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxBankfullDepth.DefaultValue = 0.0000099999997473787516
        Me.atxBankfullDepth.HardMax = 1000000000
        Me.atxBankfullDepth.HardMin = -1000000000
        Me.atxBankfullDepth.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxBankfullDepth.Location = New System.Drawing.Point(271, 218)
        Me.atxBankfullDepth.MaxDecimal = 0
        Me.atxBankfullDepth.maxWidth = 0
        Me.atxBankfullDepth.Name = "atxBankfullDepth"
        Me.atxBankfullDepth.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxBankfullDepth.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxBankfullDepth.SelLength = 0
        Me.atxBankfullDepth.SelStart = 20
        Me.atxBankfullDepth.Size = New System.Drawing.Size(93, 23)
        Me.atxBankfullDepth.SoftMax = -999
        Me.atxBankfullDepth.SoftMin = -999
        Me.atxBankfullDepth.TabIndex = 20
        Me.atxBankfullDepth.Value = 0.00001!
        '
        'atxFloodplainManningsN
        '
        Me.atxFloodplainManningsN.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxFloodplainManningsN.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxFloodplainManningsN.DefaultValue = 0.0000099999997473787516
        Me.atxFloodplainManningsN.HardMax = 1000000000
        Me.atxFloodplainManningsN.HardMin = -1000000000
        Me.atxFloodplainManningsN.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxFloodplainManningsN.Location = New System.Drawing.Point(271, 192)
        Me.atxFloodplainManningsN.MaxDecimal = 0
        Me.atxFloodplainManningsN.maxWidth = 0
        Me.atxFloodplainManningsN.Name = "atxFloodplainManningsN"
        Me.atxFloodplainManningsN.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxFloodplainManningsN.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxFloodplainManningsN.SelLength = 0
        Me.atxFloodplainManningsN.SelStart = 20
        Me.atxFloodplainManningsN.Size = New System.Drawing.Size(93, 23)
        Me.atxFloodplainManningsN.SoftMax = -999
        Me.atxFloodplainManningsN.SoftMin = -999
        Me.atxFloodplainManningsN.TabIndex = 19
        Me.atxFloodplainManningsN.Value = 0.00001!
        '
        'atxChannelManningsN
        '
        Me.atxChannelManningsN.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxChannelManningsN.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxChannelManningsN.DefaultValue = 0.0000099999997473787516
        Me.atxChannelManningsN.HardMax = 1000000000
        Me.atxChannelManningsN.HardMin = -1000000000
        Me.atxChannelManningsN.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxChannelManningsN.Location = New System.Drawing.Point(271, 166)
        Me.atxChannelManningsN.MaxDecimal = 0
        Me.atxChannelManningsN.maxWidth = 0
        Me.atxChannelManningsN.Name = "atxChannelManningsN"
        Me.atxChannelManningsN.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelManningsN.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelManningsN.SelLength = 0
        Me.atxChannelManningsN.SelStart = 20
        Me.atxChannelManningsN.Size = New System.Drawing.Size(93, 23)
        Me.atxChannelManningsN.SoftMax = -999
        Me.atxChannelManningsN.SoftMin = -999
        Me.atxChannelManningsN.TabIndex = 18
        Me.atxChannelManningsN.Value = 0.00001!
        '
        'atxChannelDepth
        '
        Me.atxChannelDepth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxChannelDepth.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxChannelDepth.DefaultValue = 0.0000099999997473787516
        Me.atxChannelDepth.HardMax = 1000000000
        Me.atxChannelDepth.HardMin = -1000000000
        Me.atxChannelDepth.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxChannelDepth.Location = New System.Drawing.Point(271, 140)
        Me.atxChannelDepth.MaxDecimal = 0
        Me.atxChannelDepth.maxWidth = 0
        Me.atxChannelDepth.Name = "atxChannelDepth"
        Me.atxChannelDepth.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelDepth.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxChannelDepth.SelLength = 0
        Me.atxChannelDepth.SelStart = 20
        Me.atxChannelDepth.Size = New System.Drawing.Size(93, 23)
        Me.atxChannelDepth.SoftMax = -999
        Me.atxChannelDepth.SoftMin = -999
        Me.atxChannelDepth.TabIndex = 17
        Me.atxChannelDepth.Value = 0.00001!
        '
        'AtcText12
        '
        Me.AtcText12.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText12.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText12.DefaultValue = Nothing
        Me.AtcText12.HardMax = 0
        Me.AtcText12.HardMin = 0
        Me.AtcText12.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText12.Location = New System.Drawing.Point(201, 209)
        Me.AtcText12.MaxDecimal = 0
        Me.AtcText12.maxWidth = 0
        Me.AtcText12.Name = "AtcText12"
        Me.AtcText12.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText12.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText12.SelLength = 0
        Me.AtcText12.SelStart = 0
        Me.AtcText12.Size = New System.Drawing.Size(93, 23)
        Me.AtcText12.SoftMax = 0
        Me.AtcText12.SoftMin = 0
        Me.AtcText12.TabIndex = 16
        '
        'AtcText13
        '
        Me.AtcText13.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText13.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText13.DefaultValue = Nothing
        Me.AtcText13.HardMax = 0
        Me.AtcText13.HardMin = 0
        Me.AtcText13.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText13.Location = New System.Drawing.Point(201, 235)
        Me.AtcText13.MaxDecimal = 0
        Me.AtcText13.maxWidth = 0
        Me.AtcText13.Name = "AtcText13"
        Me.AtcText13.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText13.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText13.SelLength = 0
        Me.AtcText13.SelStart = 0
        Me.AtcText13.Size = New System.Drawing.Size(93, 23)
        Me.AtcText13.SoftMax = 0
        Me.AtcText13.SoftMin = 0
        Me.AtcText13.TabIndex = 17
        '
        'AtcText22
        '
        Me.AtcText22.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText22.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText22.DefaultValue = Nothing
        Me.AtcText22.HardMax = 0
        Me.AtcText22.HardMin = 0
        Me.AtcText22.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText22.Location = New System.Drawing.Point(201, 40)
        Me.AtcText22.MaxDecimal = 0
        Me.AtcText22.maxWidth = 0
        Me.AtcText22.Name = "AtcText22"
        Me.AtcText22.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText22.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText22.SelLength = 0
        Me.AtcText22.SelStart = 0
        Me.AtcText22.Size = New System.Drawing.Size(93, 23)
        Me.AtcText22.SoftMax = 0
        Me.AtcText22.SoftMin = 0
        Me.AtcText22.TabIndex = 26
        '
        'AtcText21
        '
        Me.AtcText21.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText21.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText21.DefaultValue = Nothing
        Me.AtcText21.HardMax = 0
        Me.AtcText21.HardMin = 0
        Me.AtcText21.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText21.Location = New System.Drawing.Point(201, 14)
        Me.AtcText21.MaxDecimal = 0
        Me.AtcText21.maxWidth = 0
        Me.AtcText21.Name = "AtcText21"
        Me.AtcText21.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText21.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText21.SelLength = 0
        Me.AtcText21.SelStart = 0
        Me.AtcText21.Size = New System.Drawing.Size(93, 23)
        Me.AtcText21.SoftMax = 0
        Me.AtcText21.SoftMin = 0
        Me.AtcText21.TabIndex = 25
        '
        'AtcText1
        '
        Me.AtcText1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText1.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText1.DefaultValue = Nothing
        Me.AtcText1.HardMax = 0
        Me.AtcText1.HardMin = 0
        Me.AtcText1.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText1.Location = New System.Drawing.Point(165, 16)
        Me.AtcText1.MaxDecimal = 0
        Me.AtcText1.maxWidth = 0
        Me.AtcText1.Name = "AtcText1"
        Me.AtcText1.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText1.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText1.SelLength = 0
        Me.AtcText1.SelStart = 0
        Me.AtcText1.Size = New System.Drawing.Size(93, 23)
        Me.AtcText1.SoftMax = 0
        Me.AtcText1.SoftMin = 0
        Me.AtcText1.TabIndex = 27
        '
        'AtcText14
        '
        Me.AtcText14.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText14.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText14.DefaultValue = Nothing
        Me.AtcText14.HardMax = 0
        Me.AtcText14.HardMin = 0
        Me.AtcText14.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText14.Location = New System.Drawing.Point(201, 261)
        Me.AtcText14.MaxDecimal = 0
        Me.AtcText14.maxWidth = 0
        Me.AtcText14.Name = "AtcText14"
        Me.AtcText14.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText14.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText14.SelLength = 0
        Me.AtcText14.SelStart = 0
        Me.AtcText14.Size = New System.Drawing.Size(93, 23)
        Me.AtcText14.SoftMax = 0
        Me.AtcText14.SoftMin = 0
        Me.AtcText14.TabIndex = 18
        '
        'AtcText15
        '
        Me.AtcText15.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText15.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText15.DefaultValue = Nothing
        Me.AtcText15.HardMax = 0
        Me.AtcText15.HardMin = 0
        Me.AtcText15.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText15.Location = New System.Drawing.Point(201, 287)
        Me.AtcText15.MaxDecimal = 0
        Me.AtcText15.maxWidth = 0
        Me.AtcText15.Name = "AtcText15"
        Me.AtcText15.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText15.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText15.SelLength = 0
        Me.AtcText15.SelStart = 0
        Me.AtcText15.Size = New System.Drawing.Size(93, 23)
        Me.AtcText15.SoftMax = 0
        Me.AtcText15.SoftMin = 0
        Me.AtcText15.TabIndex = 19
        '
        'AtcText16
        '
        Me.AtcText16.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText16.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText16.DefaultValue = Nothing
        Me.AtcText16.HardMax = 0
        Me.AtcText16.HardMin = 0
        Me.AtcText16.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText16.Location = New System.Drawing.Point(201, 313)
        Me.AtcText16.MaxDecimal = 0
        Me.AtcText16.maxWidth = 0
        Me.AtcText16.Name = "AtcText16"
        Me.AtcText16.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText16.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText16.SelLength = 0
        Me.AtcText16.SelStart = 0
        Me.AtcText16.Size = New System.Drawing.Size(93, 23)
        Me.AtcText16.SoftMax = 0
        Me.AtcText16.SoftMin = 0
        Me.AtcText16.TabIndex = 20
        '
        'AtcText17
        '
        Me.AtcText17.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText17.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText17.DefaultValue = Nothing
        Me.AtcText17.HardMax = 0
        Me.AtcText17.HardMin = 0
        Me.AtcText17.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText17.Location = New System.Drawing.Point(201, 339)
        Me.AtcText17.MaxDecimal = 0
        Me.AtcText17.maxWidth = 0
        Me.AtcText17.Name = "AtcText17"
        Me.AtcText17.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText17.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText17.SelLength = 0
        Me.AtcText17.SelStart = 0
        Me.AtcText17.Size = New System.Drawing.Size(93, 23)
        Me.AtcText17.SoftMax = 0
        Me.AtcText17.SoftMin = 0
        Me.AtcText17.TabIndex = 21
        '
        'AtcText18
        '
        Me.AtcText18.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText18.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText18.DefaultValue = Nothing
        Me.AtcText18.HardMax = 0
        Me.AtcText18.HardMin = 0
        Me.AtcText18.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText18.Location = New System.Drawing.Point(201, 365)
        Me.AtcText18.MaxDecimal = 0
        Me.AtcText18.maxWidth = 0
        Me.AtcText18.Name = "AtcText18"
        Me.AtcText18.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText18.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText18.SelLength = 0
        Me.AtcText18.SelStart = 0
        Me.AtcText18.Size = New System.Drawing.Size(93, 23)
        Me.AtcText18.SoftMax = 0
        Me.AtcText18.SoftMin = 0
        Me.AtcText18.TabIndex = 22
        '
        'AtcText19
        '
        Me.AtcText19.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText19.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText19.DefaultValue = Nothing
        Me.AtcText19.HardMax = 0
        Me.AtcText19.HardMin = 0
        Me.AtcText19.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText19.Location = New System.Drawing.Point(201, 391)
        Me.AtcText19.MaxDecimal = 0
        Me.AtcText19.maxWidth = 0
        Me.AtcText19.Name = "AtcText19"
        Me.AtcText19.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText19.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText19.SelLength = 0
        Me.AtcText19.SelStart = 0
        Me.AtcText19.Size = New System.Drawing.Size(93, 23)
        Me.AtcText19.SoftMax = 0
        Me.AtcText19.SoftMin = 0
        Me.AtcText19.TabIndex = 23
        '
        'AtcText2
        '
        Me.AtcText2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText2.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText2.DefaultValue = Nothing
        Me.AtcText2.HardMax = 0
        Me.AtcText2.HardMin = 0
        Me.AtcText2.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText2.Location = New System.Drawing.Point(201, 391)
        Me.AtcText2.MaxDecimal = 0
        Me.AtcText2.maxWidth = 0
        Me.AtcText2.Name = "AtcText2"
        Me.AtcText2.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText2.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText2.SelLength = 0
        Me.AtcText2.SelStart = 0
        Me.AtcText2.Size = New System.Drawing.Size(93, 23)
        Me.AtcText2.SoftMax = 0
        Me.AtcText2.SoftMin = 0
        Me.AtcText2.TabIndex = 32
        '
        'AtcText20
        '
        Me.AtcText20.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText20.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText20.DefaultValue = Nothing
        Me.AtcText20.HardMax = 0
        Me.AtcText20.HardMin = 0
        Me.AtcText20.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.AtcText20.Location = New System.Drawing.Point(201, 443)
        Me.AtcText20.MaxDecimal = 0
        Me.AtcText20.maxWidth = 0
        Me.AtcText20.Name = "AtcText20"
        Me.AtcText20.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.AtcText20.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.AtcText20.SelLength = 0
        Me.AtcText20.SelStart = 0
        Me.AtcText20.Size = New System.Drawing.Size(93, 23)
        Me.AtcText20.SoftMax = 0
        Me.AtcText20.SoftMin = 0
        Me.AtcText20.TabIndex = 24
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 95)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "Estimates:"
        '
        'frmNewFTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(487, 537)
        Me.Controls.Add(Me.cmdComputeFTable)
        Me.Controls.Add(Me.atxChannelLength)
        Me.Controls.Add(Me.atxChannelSlope)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmNewFTable"
        Me.Text = "Form1"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents atxChannelSlope As atcControls.atcText
    Friend WithEvents atxChannelLength As atcControls.atcText
    Friend WithEvents cmdComputeFTable As System.Windows.Forms.Button
    Friend WithEvents AtcText12 As atcControls.atcText
    Friend WithEvents AtcText13 As atcControls.atcText
    Friend WithEvents AtcText22 As atcControls.atcText
    Friend WithEvents AtcText21 As atcControls.atcText
    Friend WithEvents AtcText1 As atcControls.atcText
    Friend WithEvents AtcText14 As atcControls.atcText
    Friend WithEvents AtcText15 As atcControls.atcText
    Friend WithEvents AtcText16 As atcControls.atcText
    Friend WithEvents AtcText17 As atcControls.atcText
    Friend WithEvents AtcText18 As atcControls.atcText
    Friend WithEvents AtcText19 As atcControls.atcText
    Friend WithEvents AtcText2 As atcControls.atcText
    Friend WithEvents AtcText20 As atcControls.atcText
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents atxChannelWidth As atcControls.atcText
    Friend WithEvents atxChannelDepth As atcControls.atcText
    Friend WithEvents atxChannelManningsN As atcControls.atcText
    Friend WithEvents atxFloodplainManningsN As atcControls.atcText
    Friend WithEvents atxBankfullDepth As atcControls.atcText
    Friend WithEvents atxMaximumFloodplainDepth As atcControls.atcText
    Friend WithEvents atxLeftSideFloodPlainWidth As atcControls.atcText
    Friend WithEvents atxChannelSideSlope As atcControls.atcText
    Friend WithEvents atxFloodplainSideSlope As atcControls.atcText
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents atxRightSideFloodPlainWidth As atcControls.atcText
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents atxDrainageArea As atcControls.atcText
    Friend WithEvents cboProv As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
