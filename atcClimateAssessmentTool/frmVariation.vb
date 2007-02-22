Imports atcData
Imports atcSeasons
Imports atcUtility
Imports MapWinUtility

<System.Runtime.InteropServices.ComVisible(False)> Public Class frmVariation
    Inherits System.Windows.Forms.Form

    Private Const AllSeasons As String = "All Seasons"
    Private Const pClickMe As String = "<click to specify>"

    Private pNaN As Double = GetNaN()

    Private pVariation As Variation
    Private pSeasonsAvailable As New atcCollection
    Private pSeasons As atcSeasonBase
    Friend WithEvents txtVaryPET As System.Windows.Forms.TextBox
    Friend WithEvents btnViewData As System.Windows.Forms.Button
    Friend WithEvents btnViewPET As System.Windows.Forms.Button
    Friend WithEvents cboAboveBelow As System.Windows.Forms.ComboBox
    Friend WithEvents txtEventGap As System.Windows.Forms.TextBox
    Friend WithEvents cboEventGapUnits As System.Windows.Forms.ComboBox
    Friend WithEvents txtEventThreshold As System.Windows.Forms.TextBox
    Friend WithEvents chkEvents As System.Windows.Forms.CheckBox
    Friend WithEvents cboAddRemovePer As System.Windows.Forms.ComboBox
    Friend WithEvents cboYearStartMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearStart As System.Windows.Forms.Label
    Friend WithEvents txtYearStartDay As System.Windows.Forms.TextBox
    Friend WithEvents cboEventDurationUnits As System.Windows.Forms.ComboBox
    Friend WithEvents txtEventDuration As System.Windows.Forms.TextBox
    Friend WithEvents cboAboveBelowDuration As System.Windows.Forms.ComboBox
    Friend WithEvents chkEventDuration As System.Windows.Forms.CheckBox
    Friend WithEvents txtEventVolume As System.Windows.Forms.TextBox
    Friend WithEvents cboAboveBelowVolume As System.Windows.Forms.ComboBox
    Friend WithEvents chkEventVolume As System.Windows.Forms.CheckBox
    Friend WithEvents chkEventGap As System.Windows.Forms.CheckBox
    Friend WithEvents cboFunction As System.Windows.Forms.ComboBox
    Friend WithEvents lblFunction As System.Windows.Forms.Label
    Friend WithEvents chkIterative As System.Windows.Forms.CheckBox
    Friend WithEvents lblThreshold As System.Windows.Forms.Label
    Friend WithEvents chkSeasons As System.Windows.Forms.CheckBox
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
    Friend WithEvents lblIncrement2 As System.Windows.Forms.Label
    Friend WithEvents lblPET As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpMinMax As System.Windows.Forms.GroupBox
    Friend WithEvents grpEvents As System.Windows.Forms.GroupBox

    Private pSettingFormSeason As Boolean = False

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtIncrement As System.Windows.Forms.TextBox
    Friend WithEvents lblIncrement As System.Windows.Forms.Label
    Friend WithEvents txtMax As System.Windows.Forms.TextBox
    Friend WithEvents txtMin As System.Windows.Forms.TextBox
    Friend WithEvents lblMaximum As System.Windows.Forms.Label
    Friend WithEvents lblMinimum As System.Windows.Forms.Label
    Friend WithEvents lblData As System.Windows.Forms.Label
    Friend WithEvents txtVaryData As System.Windows.Forms.TextBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents btnScript As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVariation))
        Me.txtIncrement = New System.Windows.Forms.TextBox
        Me.lblIncrement = New System.Windows.Forms.Label
        Me.txtMax = New System.Windows.Forms.TextBox
        Me.txtMin = New System.Windows.Forms.TextBox
        Me.lblMaximum = New System.Windows.Forms.Label
        Me.lblMinimum = New System.Windows.Forms.Label
        Me.lblData = New System.Windows.Forms.Label
        Me.txtVaryData = New System.Windows.Forms.TextBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblName = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.btnScript = New System.Windows.Forms.Button
        Me.txtVaryPET = New System.Windows.Forms.TextBox
        Me.btnViewData = New System.Windows.Forms.Button
        Me.btnViewPET = New System.Windows.Forms.Button
        Me.chkEventGap = New System.Windows.Forms.CheckBox
        Me.cboEventDurationUnits = New System.Windows.Forms.ComboBox
        Me.txtEventDuration = New System.Windows.Forms.TextBox
        Me.cboAboveBelowDuration = New System.Windows.Forms.ComboBox
        Me.chkEventDuration = New System.Windows.Forms.CheckBox
        Me.txtEventVolume = New System.Windows.Forms.TextBox
        Me.cboAboveBelowVolume = New System.Windows.Forms.ComboBox
        Me.chkEventVolume = New System.Windows.Forms.CheckBox
        Me.chkEvents = New System.Windows.Forms.CheckBox
        Me.cboAboveBelow = New System.Windows.Forms.ComboBox
        Me.txtEventGap = New System.Windows.Forms.TextBox
        Me.cboEventGapUnits = New System.Windows.Forms.ComboBox
        Me.txtEventThreshold = New System.Windows.Forms.TextBox
        Me.txtYearStartDay = New System.Windows.Forms.TextBox
        Me.cboYearStartMonth = New System.Windows.Forms.ComboBox
        Me.lblYearStart = New System.Windows.Forms.Label
        Me.cboAddRemovePer = New System.Windows.Forms.ComboBox
        Me.cboFunction = New System.Windows.Forms.ComboBox
        Me.lblFunction = New System.Windows.Forms.Label
        Me.chkIterative = New System.Windows.Forms.CheckBox
        Me.lblThreshold = New System.Windows.Forms.Label
        Me.chkSeasons = New System.Windows.Forms.CheckBox
        Me.cboSeasons = New System.Windows.Forms.ComboBox
        Me.lstSeasons = New System.Windows.Forms.ListBox
        Me.btnSeasonsNone = New System.Windows.Forms.Button
        Me.btnSeasonsAll = New System.Windows.Forms.Button
        Me.grpSeasons = New System.Windows.Forms.GroupBox
        Me.lblIncrement2 = New System.Windows.Forms.Label
        Me.lblPET = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.grpMinMax = New System.Windows.Forms.GroupBox
        Me.grpEvents = New System.Windows.Forms.GroupBox
        Me.grpSeasons.SuspendLayout()
        Me.grpMinMax.SuspendLayout()
        Me.grpEvents.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtIncrement
        '
        Me.txtIncrement.Location = New System.Drawing.Point(75, 68)
        Me.txtIncrement.Name = "txtIncrement"
        Me.txtIncrement.Size = New System.Drawing.Size(71, 20)
        Me.txtIncrement.TabIndex = 18
        Me.txtIncrement.Text = "0.05"
        '
        'lblIncrement
        '
        Me.lblIncrement.AutoSize = True
        Me.lblIncrement.BackColor = System.Drawing.Color.Transparent
        Me.lblIncrement.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblIncrement.Location = New System.Drawing.Point(4, 71)
        Me.lblIncrement.Name = "lblIncrement"
        Me.lblIncrement.Size = New System.Drawing.Size(57, 13)
        Me.lblIncrement.TabIndex = 17
        Me.lblIncrement.Text = "Increment:"
        Me.lblIncrement.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMax
        '
        Me.txtMax.Location = New System.Drawing.Point(75, 94)
        Me.txtMax.Name = "txtMax"
        Me.txtMax.Size = New System.Drawing.Size(71, 20)
        Me.txtMax.TabIndex = 21
        Me.txtMax.Text = "1.1"
        '
        'txtMin
        '
        Me.txtMin.Location = New System.Drawing.Point(75, 42)
        Me.txtMin.Name = "txtMin"
        Me.txtMin.Size = New System.Drawing.Size(71, 20)
        Me.txtMin.TabIndex = 13
        Me.txtMin.Text = "0.9"
        '
        'lblMaximum
        '
        Me.lblMaximum.AutoSize = True
        Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
        Me.lblMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMaximum.Location = New System.Drawing.Point(4, 98)
        Me.lblMaximum.Name = "lblMaximum"
        Me.lblMaximum.Size = New System.Drawing.Size(54, 13)
        Me.lblMaximum.TabIndex = 20
        Me.lblMaximum.Text = "Maximum:"
        Me.lblMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMinimum
        '
        Me.lblMinimum.AutoSize = True
        Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMinimum.Location = New System.Drawing.Point(4, 45)
        Me.lblMinimum.Name = "lblMinimum"
        Me.lblMinimum.Size = New System.Drawing.Size(51, 13)
        Me.lblMinimum.TabIndex = 12
        Me.lblMinimum.Text = "Minimum:"
        Me.lblMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblData
        '
        Me.lblData.AutoSize = True
        Me.lblData.BackColor = System.Drawing.Color.Transparent
        Me.lblData.Location = New System.Drawing.Point(12, 42)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(69, 13)
        Me.lblData.TabIndex = 2
        Me.lblData.Text = "Data to Vary:"
        Me.lblData.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtVaryData
        '
        Me.txtVaryData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVaryData.Location = New System.Drawing.Point(88, 38)
        Me.txtVaryData.Name = "txtVaryData"
        Me.txtVaryData.Size = New System.Drawing.Size(568, 20)
        Me.txtVaryData.TabIndex = 3
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(563, 357)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 24)
        Me.btnOk.TabIndex = 45
        Me.btnOk.Text = "Ok"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(641, 357)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 24)
        Me.btnCancel.TabIndex = 46
        Me.btnCancel.Text = "Cancel"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.Location = New System.Drawing.Point(12, 15)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(65, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Input Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(88, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(568, 20)
        Me.txtName.TabIndex = 1
        '
        'btnScript
        '
        Me.btnScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnScript.Location = New System.Drawing.Point(461, 357)
        Me.btnScript.Name = "btnScript"
        Me.btnScript.Size = New System.Drawing.Size(96, 24)
        Me.btnScript.TabIndex = 44
        Me.btnScript.Text = "Open Script..."
        '
        'txtVaryPET
        '
        Me.txtVaryPET.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVaryPET.Location = New System.Drawing.Point(276, 64)
        Me.txtVaryPET.Name = "txtVaryPET"
        Me.txtVaryPET.Size = New System.Drawing.Size(380, 20)
        Me.txtVaryPET.TabIndex = 6
        '
        'btnViewData
        '
        Me.btnViewData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewData.Location = New System.Drawing.Point(662, 38)
        Me.btnViewData.Name = "btnViewData"
        Me.btnViewData.Size = New System.Drawing.Size(51, 21)
        Me.btnViewData.TabIndex = 4
        Me.btnViewData.Text = "View"
        '
        'btnViewPET
        '
        Me.btnViewPET.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewPET.Location = New System.Drawing.Point(662, 64)
        Me.btnViewPET.Name = "btnViewPET"
        Me.btnViewPET.Size = New System.Drawing.Size(51, 21)
        Me.btnViewPET.TabIndex = 7
        Me.btnViewPET.Text = "View"
        '
        'chkEventGap
        '
        Me.chkEventGap.AutoSize = True
        Me.chkEventGap.Location = New System.Drawing.Point(6, 61)
        Me.chkEventGap.Name = "chkEventGap"
        Me.chkEventGap.Size = New System.Drawing.Size(195, 17)
        Me.chkEventGap.TabIndex = 28
        Me.chkEventGap.Text = "Allow Gaps during an event of up to"
        Me.chkEventGap.UseVisualStyleBackColor = True
        '
        'cboEventDurationUnits
        '
        Me.cboEventDurationUnits.Enabled = True
        Me.cboEventDurationUnits.FormattingEnabled = True
        Me.cboEventDurationUnits.Location = New System.Drawing.Point(322, 110)
        Me.cboEventDurationUnits.Name = "cboEventDurationUnits"
        Me.cboEventDurationUnits.Size = New System.Drawing.Size(84, 21)
        Me.cboEventDurationUnits.TabIndex = 37
        Me.cboEventDurationUnits.Text = "Hours"
        '
        'txtEventDuration
        '
        Me.txtEventDuration.Enabled = True
        Me.txtEventDuration.Location = New System.Drawing.Point(250, 111)
        Me.txtEventDuration.Name = "txtEventDuration"
        Me.txtEventDuration.Size = New System.Drawing.Size(66, 20)
        Me.txtEventDuration.TabIndex = 36
        Me.txtEventDuration.Text = "0"
        '
        'cboAboveBelowDuration
        '
        Me.cboAboveBelowDuration.Enabled = True
        Me.cboAboveBelowDuration.FormattingEnabled = True
        Me.cboAboveBelowDuration.Items.AddRange(New Object() {"Above", "Below"})
        Me.cboAboveBelowDuration.Location = New System.Drawing.Point(175, 111)
        Me.cboAboveBelowDuration.Name = "cboAboveBelowDuration"
        Me.cboAboveBelowDuration.Size = New System.Drawing.Size(69, 21)
        Me.cboAboveBelowDuration.TabIndex = 35
        '
        'chkEventDuration
        '
        Me.chkEventDuration.AutoSize = True
        Me.chkEventDuration.Location = New System.Drawing.Point(6, 113)
        Me.chkEventDuration.Name = "chkEventDuration"
        Me.chkEventDuration.Size = New System.Drawing.Size(145, 17)
        Me.chkEventDuration.TabIndex = 34
        Me.chkEventDuration.Text = "Only events with duration"
        Me.chkEventDuration.UseVisualStyleBackColor = True
        '
        'txtEventVolume
        '
        Me.txtEventVolume.Enabled = True
        Me.txtEventVolume.Location = New System.Drawing.Point(250, 85)
        Me.txtEventVolume.Name = "txtEventVolume"
        Me.txtEventVolume.Size = New System.Drawing.Size(66, 20)
        Me.txtEventVolume.TabIndex = 33
        Me.txtEventVolume.Text = "0"
        '
        'cboAboveBelowVolume
        '
        Me.cboAboveBelowVolume.Enabled = True
        Me.cboAboveBelowVolume.FormattingEnabled = True
        Me.cboAboveBelowVolume.Items.AddRange(New Object() {"Above", "Below"})
        Me.cboAboveBelowVolume.Location = New System.Drawing.Point(175, 85)
        Me.cboAboveBelowVolume.Name = "cboAboveBelowVolume"
        Me.cboAboveBelowVolume.Size = New System.Drawing.Size(69, 21)
        Me.cboAboveBelowVolume.TabIndex = 32
        '
        'chkEventVolume
        '
        Me.chkEventVolume.AutoSize = True
        Me.chkEventVolume.Location = New System.Drawing.Point(6, 87)
        Me.chkEventVolume.Name = "chkEventVolume"
        Me.chkEventVolume.Size = New System.Drawing.Size(164, 17)
        Me.chkEventVolume.TabIndex = 31
        Me.chkEventVolume.Text = "Only events with total volume"
        Me.chkEventVolume.UseVisualStyleBackColor = True
        '
        'chkEvents
        '
        Me.chkEvents.AutoSize = True
        Me.chkEvents.Location = New System.Drawing.Point(6, 10)
        Me.chkEvents.Name = "chkEvents"
        Me.chkEvents.Size = New System.Drawing.Size(212, 17)
        Me.chkEvents.TabIndex = 24
        Me.chkEvents.Text = "Vary values only in the following Events"
        Me.chkEvents.UseVisualStyleBackColor = True
        '
        'cboAboveBelow
        '
        Me.cboAboveBelow.Enabled = True
        Me.cboAboveBelow.FormattingEnabled = True
        Me.cboAboveBelow.Items.AddRange(New Object() {"Above", "Below"})
        Me.cboAboveBelow.Location = New System.Drawing.Point(175, 33)
        Me.cboAboveBelow.Name = "cboAboveBelow"
        Me.cboAboveBelow.Size = New System.Drawing.Size(69, 21)
        Me.cboAboveBelow.TabIndex = 26
        '
        'txtEventGap
        '
        Me.txtEventGap.Enabled = True
        Me.txtEventGap.Location = New System.Drawing.Point(250, 59)
        Me.txtEventGap.Name = "txtEventGap"
        Me.txtEventGap.Size = New System.Drawing.Size(66, 20)
        Me.txtEventGap.TabIndex = 29
        Me.txtEventGap.Text = "0"
        '
        'cboEventGapUnits
        '
        Me.cboEventGapUnits.Enabled = True
        Me.cboEventGapUnits.FormattingEnabled = True
        Me.cboEventGapUnits.Location = New System.Drawing.Point(322, 58)
        Me.cboEventGapUnits.Name = "cboEventGapUnits"
        Me.cboEventGapUnits.Size = New System.Drawing.Size(84, 21)
        Me.cboEventGapUnits.TabIndex = 30
        Me.cboEventGapUnits.Text = "Hours"
        '
        'txtEventThreshold
        '
        Me.txtEventThreshold.Enabled = True
        Me.txtEventThreshold.Location = New System.Drawing.Point(250, 33)
        Me.txtEventThreshold.Name = "txtEventThreshold"
        Me.txtEventThreshold.Size = New System.Drawing.Size(66, 20)
        Me.txtEventThreshold.TabIndex = 27
        Me.txtEventThreshold.Text = "0"
        '
        'txtYearStartDay
        '
        Me.txtYearStartDay.Location = New System.Drawing.Point(395, 42)
        Me.txtYearStartDay.Name = "txtYearStartDay"
        Me.txtYearStartDay.Size = New System.Drawing.Size(29, 20)
        Me.txtYearStartDay.TabIndex = 16
        Me.txtYearStartDay.Text = "1"
        Me.txtYearStartDay.Visible = False
        '
        'cboYearStartMonth
        '
        Me.cboYearStartMonth.FormattingEnabled = True
        Me.cboYearStartMonth.Items.AddRange(New Object() {"Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Nov", "Dec"})
        Me.cboYearStartMonth.Location = New System.Drawing.Point(318, 41)
        Me.cboYearStartMonth.Name = "cboYearStartMonth"
        Me.cboYearStartMonth.Size = New System.Drawing.Size(71, 21)
        Me.cboYearStartMonth.TabIndex = 15
        Me.cboYearStartMonth.Visible = False
        '
        'lblYearStart
        '
        Me.lblYearStart.AutoSize = True
        Me.lblYearStart.BackColor = System.Drawing.Color.Transparent
        Me.lblYearStart.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblYearStart.Location = New System.Drawing.Point(250, 45)
        Me.lblYearStart.Name = "lblYearStart"
        Me.lblYearStart.Size = New System.Drawing.Size(62, 13)
        Me.lblYearStart.TabIndex = 19
        Me.lblYearStart.Text = "Year Starts:"
        Me.lblYearStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblYearStart.Visible = False
        '
        'cboAddRemovePer
        '
        Me.cboAddRemovePer.FormattingEnabled = True
        Me.cboAddRemovePer.Items.AddRange(New Object() {"Entire Span", "Each Year", "Each Month", "Each Day"})
        Me.cboAddRemovePer.Location = New System.Drawing.Point(155, 41)
        Me.cboAddRemovePer.Name = "cboAddRemovePer"
        Me.cboAddRemovePer.Size = New System.Drawing.Size(89, 21)
        Me.cboAddRemovePer.TabIndex = 14
        Me.cboAddRemovePer.Text = "Entire Span"
        Me.cboAddRemovePer.Visible = False
        '
        'cboFunction
        '
        Me.cboFunction.FormattingEnabled = True
        Me.cboFunction.Items.AddRange(New Object() {"Multiply Each Value by a Number", "Add a Number to Each Value", "Add/Remove Events to Reach Target Volume"})
        Me.cboFunction.Location = New System.Drawing.Point(87, 90)
        Me.cboFunction.Name = "cboFunction"
        Me.cboFunction.Size = New System.Drawing.Size(356, 21)
        Me.cboFunction.TabIndex = 9
        '
        'lblFunction
        '
        Me.lblFunction.AutoSize = True
        Me.lblFunction.BackColor = System.Drawing.Color.Transparent
        Me.lblFunction.Location = New System.Drawing.Point(12, 93)
        Me.lblFunction.Name = "lblFunction"
        Me.lblFunction.Size = New System.Drawing.Size(68, 13)
        Me.lblFunction.TabIndex = 8
        Me.lblFunction.Text = "How to Vary:"
        Me.lblFunction.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkIterative
        '
        Me.chkIterative.AutoSize = True
        Me.chkIterative.Location = New System.Drawing.Point(7, 19)
        Me.chkIterative.Name = "chkIterative"
        Me.chkIterative.Size = New System.Drawing.Size(100, 17)
        Me.chkIterative.TabIndex = 11
        Me.chkIterative.Text = "Iterate changes"
        Me.chkIterative.UseVisualStyleBackColor = True
        '
        'lblThreshold
        '
        Me.lblThreshold.AutoSize = True
        Me.lblThreshold.BackColor = System.Drawing.Color.Transparent
        Me.lblThreshold.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblThreshold.Location = New System.Drawing.Point(22, 36)
        Me.lblThreshold.Name = "lblThreshold"
        Me.lblThreshold.Size = New System.Drawing.Size(126, 13)
        Me.lblThreshold.TabIndex = 25
        Me.lblThreshold.Text = "Events containing values"
        Me.lblThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkSeasons
        '
        Me.chkSeasons.AutoSize = True
        Me.chkSeasons.Location = New System.Drawing.Point(6, 10)
        Me.chkSeasons.Name = "chkSeasons"
        Me.chkSeasons.Size = New System.Drawing.Size(220, 17)
        Me.chkSeasons.TabIndex = 39
        Me.chkSeasons.Text = "Vary values only in the following Seasons"
        Me.chkSeasons.UseVisualStyleBackColor = True
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.ItemHeight = 13
        Me.cboSeasons.Location = New System.Drawing.Point(6, 33)
        Me.cboSeasons.MaxDropDownItems = 20
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(240, 21)
        Me.cboSeasons.TabIndex = 40
        '
        'lstSeasons
        '
        Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSeasons.IntegralHeight = False
        Me.lstSeasons.Location = New System.Drawing.Point(6, 60)
        Me.lstSeasons.Name = "lstSeasons"
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(240, 166)
        Me.lstSeasons.TabIndex = 41
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Location = New System.Drawing.Point(183, 232)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsNone.TabIndex = 43
        Me.btnSeasonsNone.Text = "None"
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Location = New System.Drawing.Point(6, 232)
        Me.btnSeasonsAll.Name = "btnSeasonsAll"
        Me.btnSeasonsAll.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsAll.TabIndex = 42
        Me.btnSeasonsAll.Text = "All"
        '
        'grpSeasons
        '
        Me.grpSeasons.Controls.Add(Me.chkSeasons)
        Me.grpSeasons.Controls.Add(Me.btnSeasonsAll)
        Me.grpSeasons.Controls.Add(Me.btnSeasonsNone)
        Me.grpSeasons.Controls.Add(Me.lstSeasons)
        Me.grpSeasons.Controls.Add(Me.cboSeasons)
        Me.grpSeasons.Location = New System.Drawing.Point(461, 90)
        Me.grpSeasons.Name = "grpSeasons"
        Me.grpSeasons.Size = New System.Drawing.Size(252, 261)
        Me.grpSeasons.TabIndex = 38
        Me.grpSeasons.TabStop = False
        '
        'lblIncrement2
        '
        Me.lblIncrement2.AutoSize = True
        Me.lblIncrement2.BackColor = System.Drawing.Color.Transparent
        Me.lblIncrement2.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblIncrement2.Location = New System.Drawing.Point(155, 71)
        Me.lblIncrement2.Name = "lblIncrement2"
        Me.lblIncrement2.Size = New System.Drawing.Size(230, 13)
        Me.lblIncrement2.TabIndex = 19
        Me.lblIncrement2.Text = "Increase this much each iteration from Minimum"
        Me.lblIncrement2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPET
        '
        Me.lblPET.AutoSize = True
        Me.lblPET.BackColor = System.Drawing.Color.Transparent
        Me.lblPET.Location = New System.Drawing.Point(12, 67)
        Me.lblPET.Name = "lblPET"
        Me.lblPET.Size = New System.Drawing.Size(257, 13)
        Me.lblPET.TabIndex = 5
        Me.lblPET.Text = "Compute PET (Data to Vary must be air temperature):"
        Me.lblPET.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.Label2.Location = New System.Drawing.Point(155, 98)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(140, 13)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Stop increasing at this value"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpMinMax
        '
        Me.grpMinMax.Controls.Add(Me.chkIterative)
        Me.grpMinMax.Controls.Add(Me.Label2)
        Me.grpMinMax.Controls.Add(Me.lblMinimum)
        Me.grpMinMax.Controls.Add(Me.cboAddRemovePer)
        Me.grpMinMax.Controls.Add(Me.lblIncrement2)
        Me.grpMinMax.Controls.Add(Me.lblMaximum)
        Me.grpMinMax.Controls.Add(Me.lblYearStart)
        Me.grpMinMax.Controls.Add(Me.lblIncrement)
        Me.grpMinMax.Controls.Add(Me.cboYearStartMonth)
        Me.grpMinMax.Controls.Add(Me.txtMax)
        Me.grpMinMax.Controls.Add(Me.txtYearStartDay)
        Me.grpMinMax.Controls.Add(Me.txtIncrement)
        Me.grpMinMax.Controls.Add(Me.txtMin)
        Me.grpMinMax.Location = New System.Drawing.Point(12, 117)
        Me.grpMinMax.Name = "grpMinMax"
        Me.grpMinMax.Size = New System.Drawing.Size(431, 120)
        Me.grpMinMax.TabIndex = 10
        Me.grpMinMax.TabStop = False
        Me.grpMinMax.Text = "Number to multiply each value by"
        '
        'grpEvents
        '
        Me.grpEvents.Controls.Add(Me.cboAboveBelow)
        Me.grpEvents.Controls.Add(Me.txtEventThreshold)
        Me.grpEvents.Controls.Add(Me.cboEventGapUnits)
        Me.grpEvents.Controls.Add(Me.txtEventGap)
        Me.grpEvents.Controls.Add(Me.chkEvents)
        Me.grpEvents.Controls.Add(Me.chkEventVolume)
        Me.grpEvents.Controls.Add(Me.lblThreshold)
        Me.grpEvents.Controls.Add(Me.cboAboveBelowVolume)
        Me.grpEvents.Controls.Add(Me.chkEventGap)
        Me.grpEvents.Controls.Add(Me.txtEventVolume)
        Me.grpEvents.Controls.Add(Me.cboEventDurationUnits)
        Me.grpEvents.Controls.Add(Me.chkEventDuration)
        Me.grpEvents.Controls.Add(Me.cboAboveBelowDuration)
        Me.grpEvents.Controls.Add(Me.txtEventDuration)
        Me.grpEvents.Location = New System.Drawing.Point(12, 243)
        Me.grpEvents.Name = "grpEvents"
        Me.grpEvents.Size = New System.Drawing.Size(431, 138)
        Me.grpEvents.TabIndex = 23
        Me.grpEvents.TabStop = False
        '
        'frmVariation
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(725, 393)
        Me.Controls.Add(Me.grpEvents)
        Me.Controls.Add(Me.grpMinMax)
        Me.Controls.Add(Me.lblPET)
        Me.Controls.Add(Me.grpSeasons)
        Me.Controls.Add(Me.btnViewPET)
        Me.Controls.Add(Me.txtVaryPET)
        Me.Controls.Add(Me.lblFunction)
        Me.Controls.Add(Me.cboFunction)
        Me.Controls.Add(Me.btnViewData)
        Me.Controls.Add(Me.btnScript)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtVaryData)
        Me.Controls.Add(Me.lblData)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmVariation"
        Me.Text = "Input"
        Me.grpSeasons.ResumeLayout(False)
        Me.grpSeasons.PerformLayout()
        Me.grpMinMax.ResumeLayout(False)
        Me.grpMinMax.PerformLayout()
        Me.grpEvents.ResumeLayout(False)
        Me.grpEvents.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Function AskUser(ByRef aVariation As Variation) As Boolean
        pVariation = aVariation.Clone

        If pVariation.DataSets Is Nothing Then pVariation.DataSets = New atcDataGroup

        cboSeasons.Items.Add(AllSeasons)

        cboEventGapUnits.Items.AddRange(atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitNames)
        cboEventGapUnits.SelectedIndex = GetSetting("BASINS4", "CAT", "EventGapUnits", 0)

        cboEventDurationUnits.Items.AddRange(atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitNames)
        cboEventDurationUnits.SelectedIndex = GetSetting("BASINS4", "CAT", "EventDurationUnits", 0)

        pSeasonsAvailable = atcSeasonPlugin.AllSeasonTypes
        For Each lSeasonType As Type In pSeasonsAvailable
            Dim lSeasonTypeShortName As String = atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name)
            Select Case lSeasonTypeShortName 'TODO: handle difficult seasons
                Case "Calendar Year"
                Case "Water Year"
                Case "Year Subset"
                Case Else
                    cboSeasons.Items.Add(lSeasonTypeShortName)
            End Select
        Next

        FormFromVariation()

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pVariation.CopyTo(aVariation)
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub txtVaryData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVaryData.Click
        UserSelectData()
    End Sub

    Private Sub txtVaryData_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtVaryData.KeyPress
        UserSelectData()
    End Sub

    Private Sub txtVaryPET_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVaryPET.Click
        UserSelectPET()
    End Sub

    Private Sub txtVaryPET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtVaryPET.KeyPress
        UserSelectPET()
    End Sub

    Private Sub UserSelectData()
        Dim lData As atcDataGroup = g_DataManager.UserSelectData("Select data to vary", pVariation.DataSets)
        If Not lData Is Nothing Then
            pVariation.DataSets = lData
            UpdateDataText(txtVaryData, lData)
        End If
    End Sub

    Private Sub UserSelectPET()
        Dim lData As atcDataGroup = g_DataManager.UserSelectData("Select PET data to compute from air temperature", pVariation.PETdata)
        If Not lData Is Nothing Then
            pVariation.PETdata = lData
            UpdateDataText(txtVaryPET, lData)
        End If
    End Sub

    Private Sub UpdateDataText(ByVal aTextBox As Windows.Forms.TextBox, _
                               ByVal aGroup As atcDataGroup)
        If Not aGroup Is Nothing AndAlso aGroup.Count > 0 Then
            aTextBox.Text = aGroup.ItemByIndex(0).ToString
            If aGroup.Count > 1 Then aTextBox.Text &= " (and " & aGroup.Count - 1 & " more)"
        Else
            aTextBox.Text = pClickMe
        End If
    End Sub

    Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
        If Not pSettingFormSeason Then
            pSeasons = Nothing
            lstSeasons.Items.Clear()
            If cboSeasons.Text <> AllSeasons Then
                Try
                    pSeasons = SelectedSeasonType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
                    For Each lSeasonIndex As Integer In pSeasons.AllSeasons
                        pSeasons.SeasonSelected(lSeasonIndex) = True
                    Next
                    RefreshSeasonsList()
                Catch ex As Exception
                    Logger.Dbg("Could not create new seasons for '" & cboSeasons.Text & "': " & ex.ToString)
                End Try
            End If
        End If
    End Sub

    Private Sub RefreshSeasonsList()
        Try
            lstSeasons.Items.Clear()
            For Each lSeasonIndex As Integer In pSeasons.AllSeasons
                lstSeasons.Items.Add(pSeasons.SeasonName(lSeasonIndex))
                lstSeasons.SetSelected(lstSeasons.Items.Count - 1, pSeasons.SeasonSelected(lSeasonIndex))
            Next
            lstSeasons.TopIndex = 0
            'Loop to check what was selected - removing this reveals a bug in the list control and it forgets what was selected
            For Each lSelectedIndex As Integer In lstSeasons.SelectedIndices
                Logger.Dbg("Selected " & lSelectedIndex & " = " & lstSeasons.Items(lSelectedIndex))
            Next
            lstSeasons.Refresh()
        Catch ex As Exception
            Logger.Dbg("Could not populate season list for '" & cboSeasons.Text & "': " & ex.ToString)
        End Try
    End Sub

    Private Function SelectedSeasonType() As Type
        Dim lSeasonPlugin As New atcSeasonPlugin
        For Each lSeasonType As Type In pSeasonsAvailable
            If atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name).Equals(cboSeasons.Text) Then
                Return lSeasonType
            End If
        Next
        Return Nothing
    End Function

    Private Sub btnSeasonsAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeasonsAll.Click
        For iItem As Integer = lstSeasons.Items.Count - 1 To 0 Step -1
            lstSeasons.SetSelected(iItem, True)
        Next
    End Sub

    Private Sub btnSeasonsNone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeasonsNone.Click
        For iItem As Integer = lstSeasons.Items.Count - 1 To 0 Step -1
            lstSeasons.SetSelected(iItem, False)
        Next
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If VariationFromForm(pVariation) Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                SaveSettings()
                Me.Close()
            End If
        Catch ex As Exception
            Logger.Msg(ex.Message, "Could not create variation")
        End Try
    End Sub

    Private Sub SaveSettings()
        SaveSetting("BASINS4", "CAT", "EventGapUnits", cboEventGapUnits.SelectedIndex)
        SaveSetting("BASINS4", "CAT", "EventDurationUnits", cboEventDurationUnits.SelectedIndex)
    End Sub

    Private Function VariationFromForm(ByVal aVariation As Variation) As Boolean
        Try
            With aVariation
                .Name = txtName.Text
                If txtVaryData.Text.Equals(pClickMe) Then
                    Logger.Msg("No data was selected", "Need Data To Vary")
                    Return False
                End If

                Select Case cboFunction.Text
                    Case "Multiply Each Value by a Number"
                        .Operation = "Multiply"
                    Case "Add a Number to Each Value"
                        .Operation = "Add"
                    Case "Add/Remove Events to Reach Target Volume"
                        .Operation = "AddEvents"
                    Case Else
                        Logger.Msg("'" & cboFunction.Text & "'" & vbCrLf & "Not recognized for '" & lblFunction.Text & "'", "Unknown Operation")
                        Return False
                End Select

                .AddRemovePer = cboAddRemovePer.Text

                .UseEvents = chkEvents.Checked
                If .UseEvents Then
                    .EventThreshold = CDbl(txtEventThreshold.Text)
                    If cboAboveBelow.Text = "Above" Then
                        .EventHigh = True
                    Else
                        .EventHigh = False
                    End If
                    .EventGapDisplayUnits = cboEventGapUnits.Text
                    .EventDaysGapAllowed = CDbl(txtEventGap.Text) / atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitFactor(cboEventGapUnits.SelectedIndex)

                    If chkEventVolume.Checked Then
                        If cboAboveBelowVolume.Text = "Above" Then
                            .EventVolumeHigh = True
                        Else
                            .EventVolumeHigh = False
                        End If
                        .EventVolumeThreshold = CDbl(txtEventVolume.Text)
                    Else
                        .EventVolumeThreshold = pNaN
                    End If

                    If chkEventDuration.Checked Then
                        If cboAboveBelowDuration.Text = "Above" Then
                            .EventDurationHigh = True
                        Else
                            .EventDurationHigh = False
                        End If
                        .EventDurationDisplayUnits = cboEventDurationUnits.Text
                        .EventDurationDays = CDbl(txtEventDuration.Text) / atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitFactor(cboEventDurationUnits.SelectedIndex)
                    Else
                        .EventDurationDays = pNaN
                    End If
                End If

                .Seasons = pSeasons
                If Not pSeasons Is Nothing Then
                    For lListIndex As Integer = 0 To lstSeasons.Items.Count - 1
                        Dim lSeasonName As String = lstSeasons.Items(lListIndex)
                        For Each lSeasonIndex As Integer In pSeasons.AllSeasons
                            If pSeasons.SeasonName(lSeasonIndex) = lSeasonName Then
                                pSeasons.SeasonSelected(lSeasonIndex) = lstSeasons.SelectedIndices.Contains(lListIndex)
                                Exit For
                            End If
                        Next
                    Next
                End If

                Try
                    .Min = CDbl(txtMin.Text)
                Catch
                    Logger.Msg("Minimum value must be a number", "Non-numeric value")
                    Return False
                End Try

                If chkIterative.Checked Then
                    Try
                        .Max = CDbl(txtMax.Text)
                    Catch
                        Logger.Msg("Maximum value must be a number", "Non-numeric value")
                        Return False
                    End Try
                    Try
                        .Increment = CDbl(txtIncrement.Text)
                    Catch
                        Logger.Msg("Increment must be a number", "Non-numeric value")
                        Return False
                    End Try
                Else
                    .Max = .Min
                    .Increment = 0
                End If
            End With
        Catch e As Exception
            Logger.Msg(e.Message, "Cannot create variation")
            Return False
        End Try
        Return True
    End Function

    Private Sub btnScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScript.Click
        Dim lOpenDialog As New System.Windows.Forms.OpenFileDialog
        Dim lVariationTemplate As New Variation

        If VariationFromForm(lVariationTemplate) Then
            With lOpenDialog
                .Title = "Select Script"
                .Filter = "VB.Net *.vb|*.vb|C Sharp *.cs|*.cs|All Files|*.*"
                Try
                    .FilterIndex = CInt(GetSetting("BASINS4", "Scenario", "ScriptExtIndex", 1))
                Catch
                    .FilterIndex = 1
                End Try
                .FileName = GetSetting("BASINS4", "Scenario", "ScriptFilename", ReplaceString(Me.Text, " ", "_") & ".vb")
                If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    SaveSetting("BASINS4", "Scenario", "ScriptExtIndex", .FilterIndex)
                    SaveSetting("BASINS4", "Scenario", "ScriptFilename", .FileName)
                    Dim lErrors As String = ""
                    Try
                        pVariation = Scripting.Run(FileExt(.FileName), "", .FileName, lErrors, False, (g_MapWin), lVariationTemplate)
                    Catch ex As Exception
                        If lErrors.Length > 0 Then lErrors &= vbCrLf & vbCrLf
                        lErrors &= ex.Message
                    End Try
                    If lErrors.Length > 0 Then
                        Logger.Msg("Error running variation script" & vbCrLf & vbCrLf & lErrors)
                    Else 'ok scriped variation - update form as appropriate
                        FormFromVariation()
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub FormFromVariation()
        With pVariation
            txtName.Text = .Name
            UpdateDataText(txtVaryData, pVariation.DataSets)
            UpdateDataText(txtVaryPET, pVariation.PETdata)

            Select Case .Operation
                Case "Multiply" : cboFunction.SelectedIndex = 0
                Case "Add" : cboFunction.SelectedIndex = 1
                Case "AddEvents" : cboFunction.SelectedIndex = 2
                Case "AddVolume" : cboFunction.SelectedIndex = 3
            End Select

            EnableIterative(.Max > .Min)
            If Not Double.IsNaN(.Min) Then txtMin.Text = .Min
            If Not Double.IsNaN(.Max) Then txtMax.Text = .Max
            If Not Double.IsNaN(.Increment) Then txtIncrement.Text = .Increment
            cboAddRemovePer.Text = .AddRemovePer

            EnableEvents(.UseEvents)

            If .EventHigh Then
                cboAboveBelow.SelectedIndex = 0
            Else
                cboAboveBelow.SelectedIndex = 1
            End If

            If .EventVolumeHigh Then
                cboAboveBelowVolume.SelectedIndex = 0
            Else
                cboAboveBelowVolume.SelectedIndex = 1
            End If

            If .EventDurationHigh Then
                cboAboveBelowDuration.SelectedIndex = 0
            Else
                cboAboveBelowDuration.SelectedIndex = 1
            End If

            If .UseEvents Then
                txtEventThreshold.Text = .EventThreshold

                Dim lGapUnitIndex As Integer = Array.IndexOf(atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitNames, .EventGapDisplayUnits)
                If lGapUnitIndex >= 0 Then
                    chkEventGap.Checked = True
                    cboEventGapUnits.Text = .EventGapDisplayUnits
                    txtEventGap.Text = .EventDaysGapAllowed * atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitFactor(lGapUnitIndex)
                Else
                    chkEventGap.Checked = False
                End If
            Else
                cboAboveBelow.SelectedIndex = 0
                cboAboveBelowDuration.SelectedIndex = 0
                cboAboveBelowVolume.SelectedIndex = 0
            End If

            If .Seasons Is Nothing Then
                cboSeasons.SelectedIndex = 0
                EnableSeasons(False)
            Else
                pSettingFormSeason = True
                EnableSeasons(True)
                cboSeasons.Text = atcSeasonPlugin.SeasonClassNameToLabel(.Seasons.GetType.Name)
                pSeasons = .Seasons
                RefreshSeasonsList()
                pSettingFormSeason = False
            End If
        End With
    End Sub

    Private Sub frmVariation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Analysis\Climate Assessment Tool.html")
        End If
    End Sub

    Private Sub btnViewData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewData.Click
        Try
            If pVariation.DataSets.Count > 0 Then
                g_DataManager.ShowDisplay("", pVariation.DataSets)
            Else
                UserSelectData()
            End If
        Catch ex As Exception
            UserSelectData()
        End Try
    End Sub

    Private Sub btnViewPET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewPET.Click
        Try
            If pVariation.PETdata.Count > 0 Then
                g_DataManager.ShowDisplay("", pVariation.PETdata)
            Else
                UserSelectPET()
            End If
        Catch ex As Exception
            UserSelectPET()
        End Try
    End Sub

    Private Sub chkIterative_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIterative.CheckedChanged
        EnableIterative(chkIterative.Checked)
    End Sub

    Private Sub chkEvents_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEvents.CheckedChanged
        EnableEvents(chkEvents.Checked)
    End Sub

    Private Sub chkSeasons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasons.CheckedChanged
        EnableSeasons(chkSeasons.Checked)
    End Sub

    Private Sub EnableIterative(ByVal aEnable As Boolean)

        If chkIterative.Checked <> aEnable Then chkIterative.Checked = aEnable

        If aEnable Then
            lblMinimum.Text = "Minimum"
            'grpMinMax.Height = 120
        Else
            lblMinimum.Text = "Value"
            'grpMinMax.Height = 68
        End If
        lblMaximum.Visible = aEnable
        txtMax.Visible = aEnable
        lblIncrement.Visible = aEnable
        txtIncrement.Visible = aEnable
    End Sub

    Private Sub EnableEvents(ByVal aEnable As Boolean)

        If chkEvents.Checked <> aEnable Then chkEvents.Checked = aEnable

        If aEnable Then
            'grpEvents.Height = 138
        Else
            'grpEvents.Height = 33
        End If
        chkEventGap.Visible = aEnable
        chkEventVolume.Visible = aEnable
        chkEventDuration.Visible = aEnable

        cboAboveBelow.Visible = aEnable
        cboAboveBelowVolume.Visible = aEnable
        cboAboveBelowDuration.Visible = aEnable

        txtEventThreshold.Visible = aEnable
        txtEventGap.Visible = aEnable
        txtEventVolume.Visible = aEnable
        txtEventDuration.Visible = aEnable

        cboEventGapUnits.Visible = aEnable
        cboEventDurationUnits.Visible = aEnable
    End Sub

    Private Sub EnableSeasons(ByVal aEnable As Boolean)

        If chkSeasons.Checked <> aEnable Then chkSeasons.Checked = aEnable

        cboSeasons.Visible = aEnable
        lstSeasons.Visible = aEnable
        btnSeasonsAll.Visible = aEnable
        btnSeasonsNone.Visible = aEnable
        If aEnable Then
            'grpSeasons.Height = 261
        Else
            'grpSeasons.Height = 34
        End If
    End Sub

    Private Sub cboAddRemovePer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAddRemovePer.SelectedIndexChanged
        ShowYearStart()
    End Sub

    Private Sub ShowYearStart()
        Dim lVisible As Boolean = False
        If cboAddRemovePer.Visible AndAlso cboAddRemovePer.Text.IndexOf("Year") >= 0 Then
            lVisible = True
        End If
        lblYearStart.Visible = lVisible
        cboYearStartMonth.Visible = lVisible
        txtYearStartDay.Visible = lVisible
    End Sub

    Private Sub cboFunction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFunction.SelectedIndexChanged
        FunctionChanged()
    End Sub

    Private Sub FunctionChanged()
        Select Case cboFunction.Text
            Case "Multiply Each Value by a Number"
                grpMinMax.Text = "Number to multiply each value by"
            Case "Add a Number to Each Value"
                grpMinMax.Text = "Number to add to each value"
            Case "Add/Remove Events to Reach Target Volume"
                grpMinMax.Text = "Volume to add (negative to remove)"
        End Select

        If cboFunction.Text.IndexOf("Volume") >= 0 Then
            cboAddRemovePer.Visible = True
            If cboAddRemovePer.SelectedIndex < 0 Then
                cboAddRemovePer.SelectedIndex = 0
            End If
        Else
            cboAddRemovePer.Visible = False
        End If
        ShowYearStart()
    End Sub

    Private Sub chkEventGap_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEventGap.CheckedChanged
        If chkEventGap.Checked Then
        Else
            txtEventGap.Text = "0"
        End If
    End Sub
End Class
