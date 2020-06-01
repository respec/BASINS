<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFilterData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFilterData))
        Me.tabFilters = New System.Windows.Forms.TabControl()
        Me.tabSubsetByDate = New System.Windows.Forms.TabPage()
        Me.atcSelectedDates = New atcData.atcChooseDataGroupDates()
        Me.tabSeasons = New System.Windows.Forms.TabPage()
        Me.chkEnableSeasons = New System.Windows.Forms.CheckBox()
        Me.lblGroupSeasons = New System.Windows.Forms.Label()
        Me.txtGroupSeasons = New System.Windows.Forms.TextBox()
        Me.radioSeasonsGroup = New System.Windows.Forms.RadioButton()
        Me.radioSeasonsCombine = New System.Windows.Forms.RadioButton()
        Me.radioSeasonsSeparate = New System.Windows.Forms.RadioButton()
        Me.lblSeasons = New System.Windows.Forms.Label()
        Me.lblSeasonType = New System.Windows.Forms.Label()
        Me.btnSeasonsNone = New System.Windows.Forms.Button()
        Me.btnSeasonsAll = New System.Windows.Forms.Button()
        Me.lstSeasons = New System.Windows.Forms.ListBox()
        Me.cboSeasons = New System.Windows.Forms.ComboBox()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.grpValueEventOptions = New System.Windows.Forms.GroupBox()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.lblValueAfterGap = New System.Windows.Forms.Label()
        Me.lblValueAfterDuration = New System.Windows.Forms.Label()
        Me.lblValueAfterSum = New System.Windows.Forms.Label()
        Me.txtEventGap = New System.Windows.Forms.TextBox()
        Me.txtValueDuration = New System.Windows.Forms.TextBox()
        Me.txtValueSum = New System.Windows.Forms.TextBox()
        Me.lblValueGap = New System.Windows.Forms.Label()
        Me.lblValueSum = New System.Windows.Forms.Label()
        Me.lblValueDuration = New System.Windows.Forms.Label()
        Me.lblValueGapUnits = New System.Windows.Forms.Label()
        Me.lblValueDurationUnits = New System.Windows.Forms.Label()
        Me.lblValueAfterMaximum = New System.Windows.Forms.Label()
        Me.lblValueAfterMinimum = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtValueMaximum = New System.Windows.Forms.TextBox()
        Me.lblMinimum = New System.Windows.Forms.Label()
        Me.txtValueMinimum = New System.Windows.Forms.TextBox()
        Me.chkEvents = New System.Windows.Forms.CheckBox()
        Me.lblValueMinimum = New System.Windows.Forms.Label()
        Me.tabChangeTimeStep = New System.Windows.Forms.TabPage()
        Me.cboTimeUnits = New System.Windows.Forms.ComboBox()
        Me.cboAggregate = New System.Windows.Forms.ComboBox()
        Me.txtTimeStep = New System.Windows.Forms.TextBox()
        Me.chkEnableChangeTimeStep = New System.Windows.Forms.CheckBox()
        Me.tabTimeseriesMath = New System.Windows.Forms.TabPage()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.clbMathOpns = New System.Windows.Forms.CheckedListBox()
        Me.btnAddMathOp = New System.Windows.Forms.Button()
        Me.cboConstant = New System.Windows.Forms.ComboBox()
        Me.cboMathOp = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.clbTimeseries = New System.Windows.Forms.CheckedListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.tabFilters.SuspendLayout()
        Me.tabSubsetByDate.SuspendLayout()
        Me.tabSeasons.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.grpValueEventOptions.SuspendLayout()
        Me.tabChangeTimeStep.SuspendLayout()
        Me.tabTimeseriesMath.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabFilters
        '
        Me.tabFilters.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabFilters.Controls.Add(Me.tabSubsetByDate)
        Me.tabFilters.Controls.Add(Me.tabSeasons)
        Me.tabFilters.Controls.Add(Me.TabPage1)
        Me.tabFilters.Controls.Add(Me.tabChangeTimeStep)
        Me.tabFilters.Controls.Add(Me.tabTimeseriesMath)
        Me.tabFilters.Location = New System.Drawing.Point(0, 0)
        Me.tabFilters.Name = "tabFilters"
        Me.tabFilters.SelectedIndex = 0
        Me.tabFilters.Size = New System.Drawing.Size(595, 513)
        Me.tabFilters.TabIndex = 0
        '
        'tabSubsetByDate
        '
        Me.tabSubsetByDate.Controls.Add(Me.atcSelectedDates)
        Me.tabSubsetByDate.Location = New System.Drawing.Point(4, 22)
        Me.tabSubsetByDate.Name = "tabSubsetByDate"
        Me.tabSubsetByDate.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSubsetByDate.Size = New System.Drawing.Size(587, 487)
        Me.tabSubsetByDate.TabIndex = 0
        Me.tabSubsetByDate.Text = "Subset By Date"
        Me.tabSubsetByDate.UseVisualStyleBackColor = True
        '
        'atcSelectedDates
        '
        Me.atcSelectedDates.Location = New System.Drawing.Point(9, 30)
        Me.atcSelectedDates.Margin = New System.Windows.Forms.Padding(4)
        Me.atcSelectedDates.Name = "atcSelectedDates"
        Me.atcSelectedDates.Size = New System.Drawing.Size(313, 105)
        Me.atcSelectedDates.TabIndex = 27
        '
        'tabSeasons
        '
        Me.tabSeasons.Controls.Add(Me.chkEnableSeasons)
        Me.tabSeasons.Controls.Add(Me.lblGroupSeasons)
        Me.tabSeasons.Controls.Add(Me.txtGroupSeasons)
        Me.tabSeasons.Controls.Add(Me.radioSeasonsGroup)
        Me.tabSeasons.Controls.Add(Me.radioSeasonsCombine)
        Me.tabSeasons.Controls.Add(Me.radioSeasonsSeparate)
        Me.tabSeasons.Controls.Add(Me.lblSeasons)
        Me.tabSeasons.Controls.Add(Me.lblSeasonType)
        Me.tabSeasons.Controls.Add(Me.btnSeasonsNone)
        Me.tabSeasons.Controls.Add(Me.btnSeasonsAll)
        Me.tabSeasons.Controls.Add(Me.lstSeasons)
        Me.tabSeasons.Controls.Add(Me.cboSeasons)
        Me.tabSeasons.Location = New System.Drawing.Point(4, 22)
        Me.tabSeasons.Name = "tabSeasons"
        Me.tabSeasons.Size = New System.Drawing.Size(587, 487)
        Me.tabSeasons.TabIndex = 2
        Me.tabSeasons.Text = "Split Into Time Periods"
        Me.tabSeasons.UseVisualStyleBackColor = True
        '
        'chkEnableSeasons
        '
        Me.chkEnableSeasons.AutoSize = True
        Me.chkEnableSeasons.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkEnableSeasons.Location = New System.Drawing.Point(8, 6)
        Me.chkEnableSeasons.Name = "chkEnableSeasons"
        Me.chkEnableSeasons.Size = New System.Drawing.Size(131, 17)
        Me.chkEnableSeasons.TabIndex = 47
        Me.chkEnableSeasons.Text = "Split Into Time Periods"
        Me.chkEnableSeasons.UseVisualStyleBackColor = True
        '
        'lblGroupSeasons
        '
        Me.lblGroupSeasons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblGroupSeasons.AutoSize = True
        Me.lblGroupSeasons.Location = New System.Drawing.Point(264, 466)
        Me.lblGroupSeasons.Name = "lblGroupSeasons"
        Me.lblGroupSeasons.Size = New System.Drawing.Size(63, 13)
        Me.lblGroupSeasons.TabIndex = 46
        Me.lblGroupSeasons.Text = "time periods"
        '
        'txtGroupSeasons
        '
        Me.txtGroupSeasons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtGroupSeasons.Location = New System.Drawing.Point(220, 464)
        Me.txtGroupSeasons.Name = "txtGroupSeasons"
        Me.txtGroupSeasons.Size = New System.Drawing.Size(33, 20)
        Me.txtGroupSeasons.TabIndex = 41
        '
        'radioSeasonsGroup
        '
        Me.radioSeasonsGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioSeasonsGroup.AutoSize = True
        Me.radioSeasonsGroup.Location = New System.Drawing.Point(8, 464)
        Me.radioSeasonsGroup.Margin = New System.Windows.Forms.Padding(2)
        Me.radioSeasonsGroup.Name = "radioSeasonsGroup"
        Me.radioSeasonsGroup.Size = New System.Drawing.Size(207, 17)
        Me.radioSeasonsGroup.TabIndex = 40
        Me.radioSeasonsGroup.Text = "Separate time series for each group of "
        Me.radioSeasonsGroup.UseVisualStyleBackColor = True
        '
        'radioSeasonsCombine
        '
        Me.radioSeasonsCombine.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioSeasonsCombine.AutoSize = True
        Me.radioSeasonsCombine.Location = New System.Drawing.Point(8, 422)
        Me.radioSeasonsCombine.Margin = New System.Windows.Forms.Padding(2)
        Me.radioSeasonsCombine.Name = "radioSeasonsCombine"
        Me.radioSeasonsCombine.Size = New System.Drawing.Size(321, 17)
        Me.radioSeasonsCombine.TabIndex = 38
        Me.radioSeasonsCombine.Text = "One time series containing all values from selected time periods"
        Me.radioSeasonsCombine.UseVisualStyleBackColor = True
        '
        'radioSeasonsSeparate
        '
        Me.radioSeasonsSeparate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioSeasonsSeparate.AutoSize = True
        Me.radioSeasonsSeparate.Checked = True
        Me.radioSeasonsSeparate.Location = New System.Drawing.Point(8, 443)
        Me.radioSeasonsSeparate.Margin = New System.Windows.Forms.Padding(2)
        Me.radioSeasonsSeparate.Name = "radioSeasonsSeparate"
        Me.radioSeasonsSeparate.Size = New System.Drawing.Size(259, 17)
        Me.radioSeasonsSeparate.TabIndex = 39
        Me.radioSeasonsSeparate.TabStop = True
        Me.radioSeasonsSeparate.Text = "Separate time series for each selected time period"
        Me.radioSeasonsSeparate.UseVisualStyleBackColor = True
        '
        'lblSeasons
        '
        Me.lblSeasons.AutoSize = True
        Me.lblSeasons.Location = New System.Drawing.Point(5, 69)
        Me.lblSeasons.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSeasons.Name = "lblSeasons"
        Me.lblSeasons.Size = New System.Drawing.Size(119, 13)
        Me.lblSeasons.TabIndex = 45
        Me.lblSeasons.Text = "Time periods to include:"
        '
        'lblSeasonType
        '
        Me.lblSeasonType.AutoSize = True
        Me.lblSeasonType.Location = New System.Drawing.Point(5, 31)
        Me.lblSeasonType.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSeasonType.Name = "lblSeasonType"
        Me.lblSeasonType.Size = New System.Drawing.Size(68, 13)
        Me.lblSeasonType.TabIndex = 44
        Me.lblSeasonType.Text = "Time Periods"
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Location = New System.Drawing.Point(267, 396)
        Me.btnSeasonsNone.Margin = New System.Windows.Forms.Padding(2)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(57, 22)
        Me.btnSeasonsNone.TabIndex = 37
        Me.btnSeasonsNone.Text = "None"
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Location = New System.Drawing.Point(8, 396)
        Me.btnSeasonsAll.Margin = New System.Windows.Forms.Padding(2)
        Me.btnSeasonsAll.Name = "btnSeasonsAll"
        Me.btnSeasonsAll.Size = New System.Drawing.Size(57, 22)
        Me.btnSeasonsAll.TabIndex = 36
        Me.btnSeasonsAll.Text = "All"
        '
        'lstSeasons
        '
        Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSeasons.IntegralHeight = False
        Me.lstSeasons.Location = New System.Drawing.Point(8, 84)
        Me.lstSeasons.Margin = New System.Windows.Forms.Padding(2)
        Me.lstSeasons.Name = "lstSeasons"
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(316, 308)
        Me.lstSeasons.TabIndex = 35
        Me.lstSeasons.Tag = "Seasons"
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.Location = New System.Drawing.Point(8, 46)
        Me.cboSeasons.Margin = New System.Windows.Forms.Padding(2)
        Me.cboSeasons.MaxDropDownItems = 20
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(316, 21)
        Me.cboSeasons.TabIndex = 34
        Me.cboSeasons.Tag = "SeasonType"
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.grpValueEventOptions)
        Me.TabPage1.Controls.Add(Me.lblValueAfterMaximum)
        Me.TabPage1.Controls.Add(Me.lblValueAfterMinimum)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.txtValueMaximum)
        Me.TabPage1.Controls.Add(Me.lblMinimum)
        Me.TabPage1.Controls.Add(Me.txtValueMinimum)
        Me.TabPage1.Controls.Add(Me.chkEvents)
        Me.TabPage1.Controls.Add(Me.lblValueMinimum)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(587, 487)
        Me.TabPage1.TabIndex = 3
        Me.TabPage1.Text = "Filter By Value"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'grpValueEventOptions
        '
        Me.grpValueEventOptions.Controls.Add(Me.RadioButton1)
        Me.grpValueEventOptions.Controls.Add(Me.RadioButton2)
        Me.grpValueEventOptions.Controls.Add(Me.lblValueAfterGap)
        Me.grpValueEventOptions.Controls.Add(Me.lblValueAfterDuration)
        Me.grpValueEventOptions.Controls.Add(Me.lblValueAfterSum)
        Me.grpValueEventOptions.Controls.Add(Me.txtEventGap)
        Me.grpValueEventOptions.Controls.Add(Me.txtValueDuration)
        Me.grpValueEventOptions.Controls.Add(Me.txtValueSum)
        Me.grpValueEventOptions.Controls.Add(Me.lblValueGap)
        Me.grpValueEventOptions.Controls.Add(Me.lblValueSum)
        Me.grpValueEventOptions.Controls.Add(Me.lblValueDuration)
        Me.grpValueEventOptions.Controls.Add(Me.lblValueGapUnits)
        Me.grpValueEventOptions.Controls.Add(Me.lblValueDurationUnits)
        Me.grpValueEventOptions.Location = New System.Drawing.Point(8, 110)
        Me.grpValueEventOptions.Name = "grpValueEventOptions"
        Me.grpValueEventOptions.Size = New System.Drawing.Size(571, 144)
        Me.grpValueEventOptions.TabIndex = 66
        Me.grpValueEventOptions.TabStop = False
        Me.grpValueEventOptions.Text = "Event Options"
        Me.grpValueEventOptions.Visible = False
        '
        'RadioButton1
        '
        Me.RadioButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(138, 101)
        Me.RadioButton1.Margin = New System.Windows.Forms.Padding(2)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(306, 17)
        Me.RadioButton1.TabIndex = 68
        Me.RadioButton1.Text = "One time series containing all values that meet these criteria"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Checked = True
        Me.RadioButton2.Location = New System.Drawing.Point(138, 122)
        Me.RadioButton2.Margin = New System.Windows.Forms.Padding(2)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(192, 17)
        Me.RadioButton2.TabIndex = 69
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "Separate time series for each event"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'lblValueAfterGap
        '
        Me.lblValueAfterGap.AutoSize = True
        Me.lblValueAfterGap.BackColor = System.Drawing.Color.Transparent
        Me.lblValueAfterGap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValueAfterGap.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueAfterGap.Location = New System.Drawing.Point(251, 22)
        Me.lblValueAfterGap.Name = "lblValueAfterGap"
        Me.lblValueAfterGap.Size = New System.Drawing.Size(285, 13)
        Me.lblValueAfterGap.TabIndex = 67
        Me.lblValueAfterGap.Text = "Merge adjacent events if time between them is not too long"
        '
        'lblValueAfterDuration
        '
        Me.lblValueAfterDuration.AutoSize = True
        Me.lblValueAfterDuration.BackColor = System.Drawing.Color.Transparent
        Me.lblValueAfterDuration.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValueAfterDuration.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueAfterDuration.Location = New System.Drawing.Point(251, 74)
        Me.lblValueAfterDuration.Name = "lblValueAfterDuration"
        Me.lblValueAfterDuration.Size = New System.Drawing.Size(211, 13)
        Me.lblValueAfterDuration.TabIndex = 66
        Me.lblValueAfterDuration.Text = "Events shorter than this will not be included"
        '
        'lblValueAfterSum
        '
        Me.lblValueAfterSum.AutoSize = True
        Me.lblValueAfterSum.BackColor = System.Drawing.Color.Transparent
        Me.lblValueAfterSum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValueAfterSum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueAfterSum.Location = New System.Drawing.Point(251, 48)
        Me.lblValueAfterSum.Name = "lblValueAfterSum"
        Me.lblValueAfterSum.Size = New System.Drawing.Size(290, 13)
        Me.lblValueAfterSum.TabIndex = 65
        Me.lblValueAfterSum.Text = "Events with values totaling less than this will not be included"
        '
        'txtEventGap
        '
        Me.txtEventGap.Location = New System.Drawing.Point(138, 19)
        Me.txtEventGap.Name = "txtEventGap"
        Me.txtEventGap.Size = New System.Drawing.Size(66, 20)
        Me.txtEventGap.TabIndex = 48
        '
        'txtValueDuration
        '
        Me.txtValueDuration.Location = New System.Drawing.Point(138, 71)
        Me.txtValueDuration.Name = "txtValueDuration"
        Me.txtValueDuration.Size = New System.Drawing.Size(66, 20)
        Me.txtValueDuration.TabIndex = 50
        '
        'txtValueSum
        '
        Me.txtValueSum.Location = New System.Drawing.Point(138, 45)
        Me.txtValueSum.Name = "txtValueSum"
        Me.txtValueSum.Size = New System.Drawing.Size(66, 20)
        Me.txtValueSum.TabIndex = 49
        '
        'lblValueGap
        '
        Me.lblValueGap.AutoSize = True
        Me.lblValueGap.BackColor = System.Drawing.Color.Transparent
        Me.lblValueGap.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueGap.Location = New System.Drawing.Point(8, 26)
        Me.lblValueGap.Name = "lblValueGap"
        Me.lblValueGap.Size = New System.Drawing.Size(93, 13)
        Me.lblValueGap.TabIndex = 51
        Me.lblValueGap.Text = "Allow Gaps Up To"
        '
        'lblValueSum
        '
        Me.lblValueSum.AutoSize = True
        Me.lblValueSum.BackColor = System.Drawing.Color.Transparent
        Me.lblValueSum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueSum.Location = New System.Drawing.Point(8, 48)
        Me.lblValueSum.Name = "lblValueSum"
        Me.lblValueSum.Size = New System.Drawing.Size(119, 13)
        Me.lblValueSum.TabIndex = 52
        Me.lblValueSum.Text = "Minimum Sum of Values"
        '
        'lblValueDuration
        '
        Me.lblValueDuration.AutoSize = True
        Me.lblValueDuration.BackColor = System.Drawing.Color.Transparent
        Me.lblValueDuration.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueDuration.Location = New System.Drawing.Point(8, 74)
        Me.lblValueDuration.Name = "lblValueDuration"
        Me.lblValueDuration.Size = New System.Drawing.Size(122, 13)
        Me.lblValueDuration.TabIndex = 53
        Me.lblValueDuration.Text = "Minimum Event Duration"
        '
        'lblValueGapUnits
        '
        Me.lblValueGapUnits.AutoSize = True
        Me.lblValueGapUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblValueGapUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueGapUnits.Location = New System.Drawing.Point(210, 23)
        Me.lblValueGapUnits.Name = "lblValueGapUnits"
        Me.lblValueGapUnits.Size = New System.Drawing.Size(35, 13)
        Me.lblValueGapUnits.TabIndex = 54
        Me.lblValueGapUnits.Text = "Hours"
        Me.lblValueGapUnits.Visible = False
        '
        'lblValueDurationUnits
        '
        Me.lblValueDurationUnits.AutoSize = True
        Me.lblValueDurationUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblValueDurationUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueDurationUnits.Location = New System.Drawing.Point(210, 74)
        Me.lblValueDurationUnits.Name = "lblValueDurationUnits"
        Me.lblValueDurationUnits.Size = New System.Drawing.Size(35, 13)
        Me.lblValueDurationUnits.TabIndex = 55
        Me.lblValueDurationUnits.Text = "Hours"
        Me.lblValueDurationUnits.Visible = False
        '
        'lblValueAfterMaximum
        '
        Me.lblValueAfterMaximum.AutoSize = True
        Me.lblValueAfterMaximum.BackColor = System.Drawing.Color.Transparent
        Me.lblValueAfterMaximum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValueAfterMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueAfterMaximum.Location = New System.Drawing.Point(218, 71)
        Me.lblValueAfterMaximum.Name = "lblValueAfterMaximum"
        Me.lblValueAfterMaximum.Size = New System.Drawing.Size(239, 13)
        Me.lblValueAfterMaximum.TabIndex = 65
        Me.lblValueAfterMaximum.Text = "Values greater than Maximum will not be included"
        '
        'lblValueAfterMinimum
        '
        Me.lblValueAfterMinimum.AutoSize = True
        Me.lblValueAfterMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblValueAfterMinimum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValueAfterMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueAfterMinimum.Location = New System.Drawing.Point(218, 45)
        Me.lblValueAfterMinimum.Name = "lblValueAfterMinimum"
        Me.lblValueAfterMinimum.Size = New System.Drawing.Size(221, 13)
        Me.lblValueAfterMinimum.TabIndex = 64
        Me.lblValueAfterMinimum.Text = "Values less than Minimum will not be included"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label1.Location = New System.Drawing.Point(31, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(323, 13)
        Me.Label1.TabIndex = 62
        Me.Label1.Text = "All Parameters are optional and will not be used if blank"
        '
        'txtValueMaximum
        '
        Me.txtValueMaximum.Location = New System.Drawing.Point(146, 68)
        Me.txtValueMaximum.Name = "txtValueMaximum"
        Me.txtValueMaximum.Size = New System.Drawing.Size(66, 20)
        Me.txtValueMaximum.TabIndex = 59
        '
        'lblMinimum
        '
        Me.lblMinimum.AutoSize = True
        Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMinimum.Location = New System.Drawing.Point(31, 71)
        Me.lblMinimum.Name = "lblMinimum"
        Me.lblMinimum.Size = New System.Drawing.Size(81, 13)
        Me.lblMinimum.TabIndex = 58
        Me.lblMinimum.Text = "Maximum Value"
        '
        'txtValueMinimum
        '
        Me.txtValueMinimum.Location = New System.Drawing.Point(146, 42)
        Me.txtValueMinimum.Name = "txtValueMinimum"
        Me.txtValueMinimum.Size = New System.Drawing.Size(66, 20)
        Me.txtValueMinimum.TabIndex = 47
        '
        'chkEvents
        '
        Me.chkEvents.AutoSize = True
        Me.chkEvents.BackColor = System.Drawing.Color.Transparent
        Me.chkEvents.Location = New System.Drawing.Point(8, 6)
        Me.chkEvents.Name = "chkEvents"
        Me.chkEvents.Size = New System.Drawing.Size(223, 17)
        Me.chkEvents.TabIndex = 45
        Me.chkEvents.Text = "Include values only in the following range:"
        Me.chkEvents.UseVisualStyleBackColor = False
        '
        'lblValueMinimum
        '
        Me.lblValueMinimum.AutoSize = True
        Me.lblValueMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblValueMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblValueMinimum.Location = New System.Drawing.Point(31, 45)
        Me.lblValueMinimum.Name = "lblValueMinimum"
        Me.lblValueMinimum.Size = New System.Drawing.Size(78, 13)
        Me.lblValueMinimum.TabIndex = 46
        Me.lblValueMinimum.Text = "Minimum Value"
        '
        'tabChangeTimeStep
        '
        Me.tabChangeTimeStep.Controls.Add(Me.cboTimeUnits)
        Me.tabChangeTimeStep.Controls.Add(Me.cboAggregate)
        Me.tabChangeTimeStep.Controls.Add(Me.txtTimeStep)
        Me.tabChangeTimeStep.Controls.Add(Me.chkEnableChangeTimeStep)
        Me.tabChangeTimeStep.Location = New System.Drawing.Point(4, 22)
        Me.tabChangeTimeStep.Name = "tabChangeTimeStep"
        Me.tabChangeTimeStep.Padding = New System.Windows.Forms.Padding(3)
        Me.tabChangeTimeStep.Size = New System.Drawing.Size(587, 487)
        Me.tabChangeTimeStep.TabIndex = 1
        Me.tabChangeTimeStep.Text = "Change Time Step"
        Me.tabChangeTimeStep.UseVisualStyleBackColor = True
        '
        'cboTimeUnits
        '
        Me.cboTimeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTimeUnits.FormattingEnabled = True
        Me.cboTimeUnits.Location = New System.Drawing.Point(70, 29)
        Me.cboTimeUnits.Name = "cboTimeUnits"
        Me.cboTimeUnits.Size = New System.Drawing.Size(71, 21)
        Me.cboTimeUnits.TabIndex = 34
        '
        'cboAggregate
        '
        Me.cboAggregate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAggregate.Location = New System.Drawing.Point(147, 29)
        Me.cboAggregate.Name = "cboAggregate"
        Me.cboAggregate.Size = New System.Drawing.Size(123, 21)
        Me.cboAggregate.TabIndex = 35
        '
        'txtTimeStep
        '
        Me.txtTimeStep.Location = New System.Drawing.Point(33, 29)
        Me.txtTimeStep.Name = "txtTimeStep"
        Me.txtTimeStep.Size = New System.Drawing.Size(31, 20)
        Me.txtTimeStep.TabIndex = 33
        Me.txtTimeStep.Text = "1"
        '
        'chkEnableChangeTimeStep
        '
        Me.chkEnableChangeTimeStep.AutoSize = True
        Me.chkEnableChangeTimeStep.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkEnableChangeTimeStep.Location = New System.Drawing.Point(8, 6)
        Me.chkEnableChangeTimeStep.Name = "chkEnableChangeTimeStep"
        Me.chkEnableChangeTimeStep.Size = New System.Drawing.Size(133, 17)
        Me.chkEnableChangeTimeStep.TabIndex = 32
        Me.chkEnableChangeTimeStep.Text = "Change Time Step To:"
        Me.chkEnableChangeTimeStep.UseVisualStyleBackColor = True
        '
        'tabTimeseriesMath
        '
        Me.tabTimeseriesMath.Controls.Add(Me.Label5)
        Me.tabTimeseriesMath.Controls.Add(Me.clbMathOpns)
        Me.tabTimeseriesMath.Controls.Add(Me.btnAddMathOp)
        Me.tabTimeseriesMath.Controls.Add(Me.cboConstant)
        Me.tabTimeseriesMath.Controls.Add(Me.cboMathOp)
        Me.tabTimeseriesMath.Controls.Add(Me.Label4)
        Me.tabTimeseriesMath.Controls.Add(Me.Label3)
        Me.tabTimeseriesMath.Controls.Add(Me.clbTimeseries)
        Me.tabTimeseriesMath.Controls.Add(Me.Label2)
        Me.tabTimeseriesMath.Location = New System.Drawing.Point(4, 22)
        Me.tabTimeseriesMath.Name = "tabTimeseriesMath"
        Me.tabTimeseriesMath.Size = New System.Drawing.Size(587, 487)
        Me.tabTimeseriesMath.TabIndex = 4
        Me.tabTimeseriesMath.Text = "Timeseries Math"
        Me.tabTimeseriesMath.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 264)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Math Operations:"
        '
        'clbMathOpns
        '
        Me.clbMathOpns.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clbMathOpns.CheckOnClick = True
        Me.clbMathOpns.FormattingEnabled = True
        Me.clbMathOpns.Location = New System.Drawing.Point(11, 280)
        Me.clbMathOpns.Name = "clbMathOpns"
        Me.clbMathOpns.Size = New System.Drawing.Size(568, 199)
        Me.clbMathOpns.TabIndex = 7
        '
        'btnAddMathOp
        '
        Me.btnAddMathOp.Location = New System.Drawing.Point(305, 221)
        Me.btnAddMathOp.Name = "btnAddMathOp"
        Me.btnAddMathOp.Size = New System.Drawing.Size(120, 23)
        Me.btnAddMathOp.TabIndex = 6
        Me.btnAddMathOp.Text = "Add Math Operation"
        Me.btnAddMathOp.UseVisualStyleBackColor = True
        '
        'cboConstant
        '
        Me.cboConstant.FormattingEnabled = True
        Me.cboConstant.Location = New System.Drawing.Point(431, 31)
        Me.cboConstant.Name = "cboConstant"
        Me.cboConstant.Size = New System.Drawing.Size(121, 21)
        Me.cboConstant.TabIndex = 5
        '
        'cboMathOp
        '
        Me.cboMathOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMathOp.FormattingEnabled = True
        Me.cboMathOp.Location = New System.Drawing.Point(304, 30)
        Me.cboMathOp.Name = "cboMathOp"
        Me.cboMathOp.Size = New System.Drawing.Size(121, 21)
        Me.cboMathOp.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(431, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Constant:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(301, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Math Operator:"
        '
        'clbTimeseries
        '
        Me.clbTimeseries.CheckOnClick = True
        Me.clbTimeseries.FormattingEnabled = True
        Me.clbTimeseries.Location = New System.Drawing.Point(11, 31)
        Me.clbTimeseries.Name = "clbTimeseries"
        Me.clbTimeseries.Size = New System.Drawing.Size(287, 214)
        Me.clbTimeseries.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Select Timeseries:"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnCancel.Location = New System.Drawing.Point(503, 519)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 24)
        Me.btnCancel.TabIndex = 99
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnOk.Location = New System.Drawing.Point(417, 519)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(80, 24)
        Me.btnOk.TabIndex = 98
        Me.btnOk.Text = "Ok"
        '
        'frmFilterData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(595, 548)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.tabFilters)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFilterData"
        Me.Text = "Filter Data"
        Me.tabFilters.ResumeLayout(False)
        Me.tabSubsetByDate.ResumeLayout(False)
        Me.tabSeasons.ResumeLayout(False)
        Me.tabSeasons.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.grpValueEventOptions.ResumeLayout(False)
        Me.grpValueEventOptions.PerformLayout()
        Me.tabChangeTimeStep.ResumeLayout(False)
        Me.tabChangeTimeStep.PerformLayout()
        Me.tabTimeseriesMath.ResumeLayout(False)
        Me.tabTimeseriesMath.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents atcSelectedDates As atcData.atcChooseDataGroupDates
    Friend WithEvents tabFilters As System.Windows.Forms.TabControl
    Friend WithEvents tabSubsetByDate As System.Windows.Forms.TabPage
    Friend WithEvents tabChangeTimeStep As System.Windows.Forms.TabPage
    Friend WithEvents cboTimeUnits As System.Windows.Forms.ComboBox
    Friend WithEvents cboAggregate As System.Windows.Forms.ComboBox
    Friend WithEvents txtTimeStep As System.Windows.Forms.TextBox
    Friend WithEvents chkEnableChangeTimeStep As System.Windows.Forms.CheckBox
    Friend WithEvents tabSeasons As System.Windows.Forms.TabPage
    Friend WithEvents lblGroupSeasons As System.Windows.Forms.Label
    Friend WithEvents txtGroupSeasons As System.Windows.Forms.TextBox
    Friend WithEvents radioSeasonsGroup As System.Windows.Forms.RadioButton
    Friend WithEvents radioSeasonsCombine As System.Windows.Forms.RadioButton
    Friend WithEvents radioSeasonsSeparate As System.Windows.Forms.RadioButton
    Friend WithEvents lblSeasons As System.Windows.Forms.Label
    Friend WithEvents lblSeasonType As System.Windows.Forms.Label
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
    Friend WithEvents chkEnableSeasons As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents txtValueMaximum As System.Windows.Forms.TextBox
    Friend WithEvents lblMinimum As System.Windows.Forms.Label
    Friend WithEvents lblValueDurationUnits As System.Windows.Forms.Label
    Friend WithEvents lblValueGapUnits As System.Windows.Forms.Label
    Friend WithEvents lblValueDuration As System.Windows.Forms.Label
    Friend WithEvents lblValueSum As System.Windows.Forms.Label
    Friend WithEvents lblValueGap As System.Windows.Forms.Label
    Friend WithEvents txtValueMinimum As System.Windows.Forms.TextBox
    Friend WithEvents txtEventGap As System.Windows.Forms.TextBox
    Friend WithEvents chkEvents As System.Windows.Forms.CheckBox
    Friend WithEvents lblValueMinimum As System.Windows.Forms.Label
    Friend WithEvents txtValueSum As System.Windows.Forms.TextBox
    Friend WithEvents txtValueDuration As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblValueAfterMinimum As System.Windows.Forms.Label
    Friend WithEvents grpValueEventOptions As System.Windows.Forms.GroupBox
    Friend WithEvents lblValueAfterMaximum As System.Windows.Forms.Label
    Friend WithEvents lblValueAfterGap As System.Windows.Forms.Label
    Friend WithEvents lblValueAfterDuration As System.Windows.Forms.Label
    Friend WithEvents lblValueAfterSum As System.Windows.Forms.Label
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents tabTimeseriesMath As System.Windows.Forms.TabPage
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents clbTimeseries As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnAddMathOp As System.Windows.Forms.Button
    Friend WithEvents cboConstant As System.Windows.Forms.ComboBox
    Friend WithEvents cboMathOp As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents clbMathOpns As System.Windows.Forms.CheckedListBox
End Class
