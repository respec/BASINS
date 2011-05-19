Imports atcData
Imports atcSeasons
Imports atcUtility
Imports MapWinUtility

Public Class frmEndpoint
    Inherits System.Windows.Forms.Form

    Private pNaN As Double = GetNaN()

    Private pNotNumberString As String = "<none>"
    Private pVariation As atcVariation
    Private pSeasonsAvailable As New atcCollection
    Private pSeasons As atcSeasonBase
    Private pAllSeasons As Integer()

    Friend WithEvents lblOperation As System.Windows.Forms.Label
    Friend WithEvents txtOperation As System.Windows.Forms.TextBox
    Friend WithEvents panelOperation As System.Windows.Forms.Panel
    Friend WithEvents chkSeasons As System.Windows.Forms.CheckBox
    Friend WithEvents grpEvents As System.Windows.Forms.GroupBox
    Friend WithEvents lblVolumeUnits As System.Windows.Forms.Label
    Friend WithEvents lblThresholdUnits As System.Windows.Forms.Label
    Friend WithEvents lblDurationUnits As System.Windows.Forms.Label
    Friend WithEvents lblGapUnits As System.Windows.Forms.Label
    Friend WithEvents lblDuration As System.Windows.Forms.Label
    Friend WithEvents lblVolume As System.Windows.Forms.Label
    Friend WithEvents lblGap As System.Windows.Forms.Label
    Friend WithEvents txtEventThreshold As System.Windows.Forms.TextBox
    Friend WithEvents txtEventGap As System.Windows.Forms.TextBox
    Friend WithEvents chkEvents As System.Windows.Forms.CheckBox
    Friend WithEvents lblThreshold As System.Windows.Forms.Label
    Friend WithEvents txtEventVolume As System.Windows.Forms.TextBox
    Friend WithEvents txtEventDuration As System.Windows.Forms.TextBox
    Friend WithEvents btnSelectAttributes As System.Windows.Forms.Button

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
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents lblAttribute As System.Windows.Forms.Label
    Friend WithEvents lblData As System.Windows.Forms.Label
    Friend WithEvents grpHighlight As System.Windows.Forms.GroupBox
    Friend WithEvents txtMin As System.Windows.Forms.TextBox
    Friend WithEvents lblMinimum As System.Windows.Forms.Label
    Friend WithEvents txtLowColor As System.Windows.Forms.TextBox
    Friend WithEvents lblLowColor As System.Windows.Forms.Label
    Friend WithEvents txtHighColor As System.Windows.Forms.TextBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtData As System.Windows.Forms.TextBox
    Friend WithEvents lblHighColor As System.Windows.Forms.Label
    Friend WithEvents cboAttribute As System.Windows.Forms.ComboBox
    Friend WithEvents txtMax As System.Windows.Forms.TextBox
    Friend WithEvents lblMaximum As System.Windows.Forms.Label
    Friend WithEvents txtDefaultColor As System.Windows.Forms.TextBox
    Friend WithEvents lblDefaultColor As System.Windows.Forms.Label
    Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEndpoint))
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.lblAttribute = New System.Windows.Forms.Label()
        Me.lblData = New System.Windows.Forms.Label()
        Me.txtData = New System.Windows.Forms.TextBox()
        Me.grpHighlight = New System.Windows.Forms.GroupBox()
        Me.txtHighColor = New System.Windows.Forms.TextBox()
        Me.lblHighColor = New System.Windows.Forms.Label()
        Me.txtLowColor = New System.Windows.Forms.TextBox()
        Me.lblLowColor = New System.Windows.Forms.Label()
        Me.txtMax = New System.Windows.Forms.TextBox()
        Me.txtMin = New System.Windows.Forms.TextBox()
        Me.lblMaximum = New System.Windows.Forms.Label()
        Me.lblMinimum = New System.Windows.Forms.Label()
        Me.txtDefaultColor = New System.Windows.Forms.TextBox()
        Me.lblDefaultColor = New System.Windows.Forms.Label()
        Me.cboAttribute = New System.Windows.Forms.ComboBox()
        Me.grpSeasons = New System.Windows.Forms.GroupBox()
        Me.chkSeasons = New System.Windows.Forms.CheckBox()
        Me.cboSeasons = New System.Windows.Forms.ComboBox()
        Me.lstSeasons = New System.Windows.Forms.ListBox()
        Me.btnSeasonsAll = New System.Windows.Forms.Button()
        Me.btnSeasonsNone = New System.Windows.Forms.Button()
        Me.lblOperation = New System.Windows.Forms.Label()
        Me.txtOperation = New System.Windows.Forms.TextBox()
        Me.panelOperation = New System.Windows.Forms.Panel()
        Me.grpEvents = New System.Windows.Forms.GroupBox()
        Me.lblVolumeUnits = New System.Windows.Forms.Label()
        Me.lblThresholdUnits = New System.Windows.Forms.Label()
        Me.lblDurationUnits = New System.Windows.Forms.Label()
        Me.lblGapUnits = New System.Windows.Forms.Label()
        Me.lblDuration = New System.Windows.Forms.Label()
        Me.lblVolume = New System.Windows.Forms.Label()
        Me.lblGap = New System.Windows.Forms.Label()
        Me.txtEventThreshold = New System.Windows.Forms.TextBox()
        Me.txtEventGap = New System.Windows.Forms.TextBox()
        Me.chkEvents = New System.Windows.Forms.CheckBox()
        Me.lblThreshold = New System.Windows.Forms.Label()
        Me.txtEventVolume = New System.Windows.Forms.TextBox()
        Me.txtEventDuration = New System.Windows.Forms.TextBox()
        Me.btnSelectAttributes = New System.Windows.Forms.Button()
        Me.grpHighlight.SuspendLayout()
        Me.grpSeasons.SuspendLayout()
        Me.panelOperation.SuspendLayout()
        Me.grpEvents.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.Location = New System.Drawing.Point(12, 19)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(83, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Endpoint Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(101, 16)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(240, 20)
        Me.txtName.TabIndex = 1
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(275, 618)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 24)
        Me.btnCancel.TabIndex = 23
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(197, 618)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 24)
        Me.btnOk.TabIndex = 22
        Me.btnOk.Text = "Ok"
        '
        'lblAttribute
        '
        Me.lblAttribute.AutoSize = True
        Me.lblAttribute.BackColor = System.Drawing.Color.Transparent
        Me.lblAttribute.Location = New System.Drawing.Point(46, 67)
        Me.lblAttribute.Name = "lblAttribute"
        Me.lblAttribute.Size = New System.Drawing.Size(49, 13)
        Me.lblAttribute.TabIndex = 4
        Me.lblAttribute.Text = "Attribute:"
        Me.lblAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblData
        '
        Me.lblData.AutoSize = True
        Me.lblData.BackColor = System.Drawing.Color.Transparent
        Me.lblData.Location = New System.Drawing.Point(45, 43)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(50, 13)
        Me.lblData.TabIndex = 2
        Me.lblData.Text = "Data set:"
        Me.lblData.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtData
        '
        Me.txtData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtData.Location = New System.Drawing.Point(101, 40)
        Me.txtData.Name = "txtData"
        Me.txtData.Size = New System.Drawing.Size(240, 20)
        Me.txtData.TabIndex = 3
        '
        'grpHighlight
        '
        Me.grpHighlight.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpHighlight.Controls.Add(Me.txtHighColor)
        Me.grpHighlight.Controls.Add(Me.lblHighColor)
        Me.grpHighlight.Controls.Add(Me.txtLowColor)
        Me.grpHighlight.Controls.Add(Me.lblLowColor)
        Me.grpHighlight.Controls.Add(Me.txtMax)
        Me.grpHighlight.Controls.Add(Me.txtMin)
        Me.grpHighlight.Controls.Add(Me.lblMaximum)
        Me.grpHighlight.Controls.Add(Me.lblMinimum)
        Me.grpHighlight.Controls.Add(Me.txtDefaultColor)
        Me.grpHighlight.Controls.Add(Me.lblDefaultColor)
        Me.grpHighlight.Location = New System.Drawing.Point(12, 96)
        Me.grpHighlight.Name = "grpHighlight"
        Me.grpHighlight.Size = New System.Drawing.Size(335, 160)
        Me.grpHighlight.TabIndex = 6
        Me.grpHighlight.TabStop = False
        Me.grpHighlight.Text = "Highlight Values"
        '
        'txtHighColor
        '
        Me.txtHighColor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHighColor.BackColor = System.Drawing.Color.OrangeRed
        Me.txtHighColor.Location = New System.Drawing.Point(144, 120)
        Me.txtHighColor.Name = "txtHighColor"
        Me.txtHighColor.Size = New System.Drawing.Size(185, 20)
        Me.txtHighColor.TabIndex = 16
        '
        'lblHighColor
        '
        Me.lblHighColor.BackColor = System.Drawing.Color.Transparent
        Me.lblHighColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblHighColor.Location = New System.Drawing.Point(16, 120)
        Me.lblHighColor.Name = "lblHighColor"
        Me.lblHighColor.Size = New System.Drawing.Size(120, 17)
        Me.lblHighColor.TabIndex = 15
        Me.lblHighColor.Text = "Color Higher Values:"
        Me.lblHighColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtLowColor
        '
        Me.txtLowColor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLowColor.BackColor = System.Drawing.Color.DeepSkyBlue
        Me.txtLowColor.Location = New System.Drawing.Point(144, 72)
        Me.txtLowColor.Name = "txtLowColor"
        Me.txtLowColor.Size = New System.Drawing.Size(185, 20)
        Me.txtLowColor.TabIndex = 12
        '
        'lblLowColor
        '
        Me.lblLowColor.BackColor = System.Drawing.Color.Transparent
        Me.lblLowColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblLowColor.Location = New System.Drawing.Point(16, 72)
        Me.lblLowColor.Name = "lblLowColor"
        Me.lblLowColor.Size = New System.Drawing.Size(120, 17)
        Me.lblLowColor.TabIndex = 11
        Me.lblLowColor.Text = "Color Lower Values:"
        Me.lblLowColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMax
        '
        Me.txtMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMax.Location = New System.Drawing.Point(144, 96)
        Me.txtMax.Name = "txtMax"
        Me.txtMax.Size = New System.Drawing.Size(185, 20)
        Me.txtMax.TabIndex = 14
        Me.txtMax.Text = "<none>"
        '
        'txtMin
        '
        Me.txtMin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMin.Location = New System.Drawing.Point(144, 48)
        Me.txtMin.Name = "txtMin"
        Me.txtMin.Size = New System.Drawing.Size(185, 20)
        Me.txtMin.TabIndex = 10
        Me.txtMin.Text = "<none>"
        '
        'lblMaximum
        '
        Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
        Me.lblMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMaximum.Location = New System.Drawing.Point(16, 96)
        Me.lblMaximum.Name = "lblMaximum"
        Me.lblMaximum.Size = New System.Drawing.Size(120, 17)
        Me.lblMaximum.TabIndex = 13
        Me.lblMaximum.Text = "Maximum Value:"
        Me.lblMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMinimum
        '
        Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMinimum.Location = New System.Drawing.Point(16, 48)
        Me.lblMinimum.Name = "lblMinimum"
        Me.lblMinimum.Size = New System.Drawing.Size(120, 17)
        Me.lblMinimum.TabIndex = 9
        Me.lblMinimum.Text = "Minimum Value:"
        Me.lblMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDefaultColor
        '
        Me.txtDefaultColor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefaultColor.BackColor = System.Drawing.Color.White
        Me.txtDefaultColor.Location = New System.Drawing.Point(144, 24)
        Me.txtDefaultColor.Name = "txtDefaultColor"
        Me.txtDefaultColor.Size = New System.Drawing.Size(185, 20)
        Me.txtDefaultColor.TabIndex = 8
        '
        'lblDefaultColor
        '
        Me.lblDefaultColor.BackColor = System.Drawing.Color.Transparent
        Me.lblDefaultColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblDefaultColor.Location = New System.Drawing.Point(16, 24)
        Me.lblDefaultColor.Name = "lblDefaultColor"
        Me.lblDefaultColor.Size = New System.Drawing.Size(120, 17)
        Me.lblDefaultColor.TabIndex = 7
        Me.lblDefaultColor.Text = "Default Color:"
        Me.lblDefaultColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboAttribute
        '
        Me.cboAttribute.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAttribute.Location = New System.Drawing.Point(101, 64)
        Me.cboAttribute.MaxDropDownItems = 20
        Me.cboAttribute.Name = "cboAttribute"
        Me.cboAttribute.Size = New System.Drawing.Size(133, 21)
        Me.cboAttribute.TabIndex = 5
        '
        'grpSeasons
        '
        Me.grpSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSeasons.Controls.Add(Me.chkSeasons)
        Me.grpSeasons.Controls.Add(Me.cboSeasons)
        Me.grpSeasons.Controls.Add(Me.lstSeasons)
        Me.grpSeasons.Controls.Add(Me.btnSeasonsAll)
        Me.grpSeasons.Controls.Add(Me.btnSeasonsNone)
        Me.grpSeasons.Location = New System.Drawing.Point(12, 441)
        Me.grpSeasons.Name = "grpSeasons"
        Me.grpSeasons.Size = New System.Drawing.Size(335, 171)
        Me.grpSeasons.TabIndex = 17
        Me.grpSeasons.TabStop = False
        Me.grpSeasons.Text = "Seasons"
        '
        'chkSeasons
        '
        Me.chkSeasons.AutoSize = True
        Me.chkSeasons.Location = New System.Drawing.Point(6, 19)
        Me.chkSeasons.Name = "chkSeasons"
        Me.chkSeasons.Size = New System.Drawing.Size(172, 17)
        Me.chkSeasons.TabIndex = 40
        Me.chkSeasons.Text = "Only include values in selected"
        Me.chkSeasons.UseVisualStyleBackColor = True
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.ItemHeight = 13
        Me.cboSeasons.Location = New System.Drawing.Point(184, 17)
        Me.cboSeasons.MaxDropDownItems = 20
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(145, 21)
        Me.cboSeasons.TabIndex = 18
        '
        'lstSeasons
        '
        Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSeasons.IntegralHeight = False
        Me.lstSeasons.Location = New System.Drawing.Point(6, 44)
        Me.lstSeasons.MultiColumn = True
        Me.lstSeasons.Name = "lstSeasons"
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(323, 90)
        Me.lstSeasons.TabIndex = 19
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Location = New System.Drawing.Point(6, 140)
        Me.btnSeasonsAll.Name = "btnSeasonsAll"
        Me.btnSeasonsAll.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsAll.TabIndex = 20
        Me.btnSeasonsAll.Text = "All"
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Location = New System.Drawing.Point(266, 140)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsNone.TabIndex = 21
        Me.btnSeasonsNone.Text = "None"
        '
        'lblOperation
        '
        Me.lblOperation.AutoSize = True
        Me.lblOperation.BackColor = System.Drawing.Color.Transparent
        Me.lblOperation.Location = New System.Drawing.Point(27, 9)
        Me.lblOperation.Name = "lblOperation"
        Me.lblOperation.Size = New System.Drawing.Size(56, 13)
        Me.lblOperation.TabIndex = 5
        Me.lblOperation.Text = "Operation:"
        Me.lblOperation.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtOperation
        '
        Me.txtOperation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOperation.Location = New System.Drawing.Point(89, 6)
        Me.txtOperation.Name = "txtOperation"
        Me.txtOperation.Size = New System.Drawing.Size(240, 20)
        Me.txtOperation.TabIndex = 6
        '
        'panelOperation
        '
        Me.panelOperation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelOperation.Controls.Add(Me.txtOperation)
        Me.panelOperation.Controls.Add(Me.lblOperation)
        Me.panelOperation.Location = New System.Drawing.Point(12, 60)
        Me.panelOperation.Name = "panelOperation"
        Me.panelOperation.Size = New System.Drawing.Size(335, 30)
        Me.panelOperation.TabIndex = 24
        Me.panelOperation.Visible = False
        '
        'grpEvents
        '
        Me.grpEvents.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEvents.Controls.Add(Me.lblVolumeUnits)
        Me.grpEvents.Controls.Add(Me.lblThresholdUnits)
        Me.grpEvents.Controls.Add(Me.lblDurationUnits)
        Me.grpEvents.Controls.Add(Me.lblGapUnits)
        Me.grpEvents.Controls.Add(Me.lblDuration)
        Me.grpEvents.Controls.Add(Me.lblVolume)
        Me.grpEvents.Controls.Add(Me.lblGap)
        Me.grpEvents.Controls.Add(Me.txtEventThreshold)
        Me.grpEvents.Controls.Add(Me.txtEventGap)
        Me.grpEvents.Controls.Add(Me.chkEvents)
        Me.grpEvents.Controls.Add(Me.lblThreshold)
        Me.grpEvents.Controls.Add(Me.txtEventVolume)
        Me.grpEvents.Controls.Add(Me.txtEventDuration)
        Me.grpEvents.Location = New System.Drawing.Point(12, 262)
        Me.grpEvents.Name = "grpEvents"
        Me.grpEvents.Size = New System.Drawing.Size(335, 173)
        Me.grpEvents.TabIndex = 25
        Me.grpEvents.TabStop = False
        Me.grpEvents.Text = "Events"
        '
        'lblVolumeUnits
        '
        Me.lblVolumeUnits.AutoSize = True
        Me.lblVolumeUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblVolumeUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblVolumeUnits.Location = New System.Drawing.Point(277, 124)
        Me.lblVolumeUnits.Name = "lblVolumeUnits"
        Me.lblVolumeUnits.Size = New System.Drawing.Size(21, 13)
        Me.lblVolumeUnits.TabIndex = 44
        Me.lblVolumeUnits.Text = "SV"
        Me.lblVolumeUnits.Visible = False
        '
        'lblThresholdUnits
        '
        Me.lblThresholdUnits.AutoSize = True
        Me.lblThresholdUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblThresholdUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblThresholdUnits.Location = New System.Drawing.Point(277, 45)
        Me.lblThresholdUnits.Name = "lblThresholdUnits"
        Me.lblThresholdUnits.Size = New System.Drawing.Size(26, 13)
        Me.lblThresholdUnits.TabIndex = 43
        Me.lblThresholdUnits.Text = "V/T"
        Me.lblThresholdUnits.Visible = False
        '
        'lblDurationUnits
        '
        Me.lblDurationUnits.AutoSize = True
        Me.lblDurationUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblDurationUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblDurationUnits.Location = New System.Drawing.Point(277, 150)
        Me.lblDurationUnits.Name = "lblDurationUnits"
        Me.lblDurationUnits.Size = New System.Drawing.Size(14, 13)
        Me.lblDurationUnits.TabIndex = 42
        Me.lblDurationUnits.Text = "T"
        Me.lblDurationUnits.Visible = False
        '
        'lblGapUnits
        '
        Me.lblGapUnits.AutoSize = True
        Me.lblGapUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblGapUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblGapUnits.Location = New System.Drawing.Point(277, 98)
        Me.lblGapUnits.Name = "lblGapUnits"
        Me.lblGapUnits.Size = New System.Drawing.Size(14, 13)
        Me.lblGapUnits.TabIndex = 41
        Me.lblGapUnits.Text = "T"
        Me.lblGapUnits.Visible = False
        '
        'lblDuration
        '
        Me.lblDuration.AutoSize = True
        Me.lblDuration.BackColor = System.Drawing.Color.Transparent
        Me.lblDuration.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblDuration.Location = New System.Drawing.Point(24, 150)
        Me.lblDuration.Name = "lblDuration"
        Me.lblDuration.Size = New System.Drawing.Size(105, 13)
        Me.lblDuration.TabIndex = 40
        Me.lblDuration.Text = "Total duration above"
        '
        'lblVolume
        '
        Me.lblVolume.AutoSize = True
        Me.lblVolume.BackColor = System.Drawing.Color.Transparent
        Me.lblVolume.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblVolume.Location = New System.Drawing.Point(24, 124)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(172, 13)
        Me.lblVolume.TabIndex = 39
        Me.lblVolume.Text = "Sum of values exceeding threshold"
        '
        'lblGap
        '
        Me.lblGap.AutoSize = True
        Me.lblGap.BackColor = System.Drawing.Color.Transparent
        Me.lblGap.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblGap.Location = New System.Drawing.Point(24, 98)
        Me.lblGap.Name = "lblGap"
        Me.lblGap.Size = New System.Drawing.Size(85, 13)
        Me.lblGap.TabIndex = 38
        Me.lblGap.Text = "Allow gaps up to"
        '
        'txtEventThreshold
        '
        Me.txtEventThreshold.Location = New System.Drawing.Point(205, 42)
        Me.txtEventThreshold.Name = "txtEventThreshold"
        Me.txtEventThreshold.Size = New System.Drawing.Size(66, 20)
        Me.txtEventThreshold.TabIndex = 27
        Me.txtEventThreshold.Text = "0"
        '
        'txtEventGap
        '
        Me.txtEventGap.Location = New System.Drawing.Point(205, 95)
        Me.txtEventGap.Name = "txtEventGap"
        Me.txtEventGap.Size = New System.Drawing.Size(66, 20)
        Me.txtEventGap.TabIndex = 29
        Me.txtEventGap.Text = "0"
        '
        'chkEvents
        '
        Me.chkEvents.AutoSize = True
        Me.chkEvents.BackColor = System.Drawing.Color.Transparent
        Me.chkEvents.Location = New System.Drawing.Point(6, 19)
        Me.chkEvents.Name = "chkEvents"
        Me.chkEvents.Size = New System.Drawing.Size(227, 17)
        Me.chkEvents.TabIndex = 24
        Me.chkEvents.Text = "Only include values in the following Events"
        Me.chkEvents.UseVisualStyleBackColor = False
        '
        'lblThreshold
        '
        Me.lblThreshold.AutoSize = True
        Me.lblThreshold.BackColor = System.Drawing.Color.Transparent
        Me.lblThreshold.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblThreshold.Location = New System.Drawing.Point(24, 45)
        Me.lblThreshold.Name = "lblThreshold"
        Me.lblThreshold.Size = New System.Drawing.Size(103, 13)
        Me.lblThreshold.TabIndex = 25
        Me.lblThreshold.Text = "Exceeding threshold"
        '
        'txtEventVolume
        '
        Me.txtEventVolume.Location = New System.Drawing.Point(205, 121)
        Me.txtEventVolume.Name = "txtEventVolume"
        Me.txtEventVolume.Size = New System.Drawing.Size(66, 20)
        Me.txtEventVolume.TabIndex = 33
        Me.txtEventVolume.Text = "0"
        '
        'txtEventDuration
        '
        Me.txtEventDuration.Location = New System.Drawing.Point(205, 147)
        Me.txtEventDuration.Name = "txtEventDuration"
        Me.txtEventDuration.Size = New System.Drawing.Size(66, 20)
        Me.txtEventDuration.TabIndex = 36
        Me.txtEventDuration.Text = "0"
        '
        'btnSelectAttributes
        '
        Me.btnSelectAttributes.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectAttributes.Location = New System.Drawing.Point(240, 64)
        Me.btnSelectAttributes.Name = "btnSelectAttributes"
        Me.btnSelectAttributes.Size = New System.Drawing.Size(101, 21)
        Me.btnSelectAttributes.TabIndex = 7
        Me.btnSelectAttributes.Text = "Manage Attributes"
        Me.btnSelectAttributes.UseVisualStyleBackColor = True
        '
        'frmEndpoint
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(359, 654)
        Me.Controls.Add(Me.btnSelectAttributes)
        Me.Controls.Add(Me.grpEvents)
        Me.Controls.Add(Me.lblAttribute)
        Me.Controls.Add(Me.grpSeasons)
        Me.Controls.Add(Me.cboAttribute)
        Me.Controls.Add(Me.grpHighlight)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.txtData)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblData)
        Me.Controls.Add(Me.panelOperation)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmEndpoint"
        Me.Text = "Endpoint"
        Me.grpHighlight.ResumeLayout(False)
        Me.grpHighlight.PerformLayout()
        Me.grpSeasons.ResumeLayout(False)
        Me.grpSeasons.PerformLayout()
        Me.panelOperation.ResumeLayout(False)
        Me.panelOperation.PerformLayout()
        Me.grpEvents.ResumeLayout(False)
        Me.grpEvents.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Function AskUser(Optional ByVal aVariation As atcVariation = Nothing) As Boolean
        pVariation = aVariation.Clone

        If pVariation.DataSets Is Nothing Then pVariation.DataSets = New atcTimeseriesGroup

        If pVariation.IsInput Then
            panelOperation.Visible = True
            grpSeasons.Visible = False
        Else
            Dim lAttributeNames As New Generic.List(Of String)
            Dim lSettingNames As String = GetSetting("BasinsCAT", "Settings", "Attributes", "")
            If lSettingNames.Length > 0 Then
                lAttributeNames.AddRange(lSettingNames.Split(vbLf))
            Else
                For Each lAttribute As atcAttributeDefinition In atcDataAttributes.AllDefinitions
                    If lAttribute.TypeString.ToLower.Equals("double") _
                       AndAlso atcData.atcDataAttributes.IsSimple(lAttribute) _
                       AndAlso Not lAttribute.Name.EndsWith("*") _
                       AndAlso Not lAttribute.Name.Contains("Date") _
                       AndAlso Not lAttribute.Name.Contains("Last") _
                       AndAlso lAttribute.Category <> "N-day and Frequency" _
                       AndAlso lAttribute.Calculated Then
                        lAttributeNames.Add(lAttribute.Name)
                    End If
                Next
            End If

            cboAttribute.Items.AddRange(lAttributeNames.ToArray)

            'cboSeasons.Items.Add(AllSeasons)
            pSeasonsAvailable = atcSeasonPlugin.AllSeasonTypes
            For Each lSeasonType As Type In pSeasonsAvailable
                Dim lSeasonTypeShortName As String = atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name)
                Select Case lSeasonTypeShortName
                    Case "Calendar Year", "Water Year", "Month"
                        cboSeasons.Items.Add(lSeasonTypeShortName & "s")
                End Select
            Next
        End If

        FormFromVariation()

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pVariation.CopyTo(aVariation)
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub txtData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtData.Click
        UserSelectData()
    End Sub

    Private Sub txtData_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtData.KeyPress
        UserSelectData()
    End Sub

    Private Sub UserSelectData()
        Dim lData As atcTimeseriesGroup = atcDataManager.UserSelectData("Select data for endpoint", pVariation.DataSets)
        If Not lData Is Nothing Then
            pVariation.DataSets = lData
            UpdateDataText(txtData, lData)
        End If
    End Sub

    Private Sub UpdateDataText(ByVal aTextBox As Windows.Forms.TextBox, _
                               ByVal aGroup As atcTimeseriesGroup)
        If Not aGroup Is Nothing AndAlso aGroup.Count > 0 Then
            aTextBox.Text = aGroup.ItemByIndex(0).ToString
            If aGroup.Count > 1 Then aTextBox.Text &= " (and " & aGroup.Count - 1 & " more)"
        Else
            aTextBox.Text = "<click to select data>"
        End If
    End Sub

    Private Sub SetAllSeasons()
        If pSeasons.AllSeasons.Length = 0 Then
            Dim lTimeseries As atcTimeseries = pVariation.DataSets.ItemByIndex(0)
            pAllSeasons = pSeasons.AllSeasonsInDates(lTimeseries.Dates.Values)
        Else
            pAllSeasons = pSeasons.AllSeasons
        End If
    End Sub

    Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
        If Not pSettingFormSeason Then
            pSeasons = Nothing
            pAllSeasons = Nothing
            lstSeasons.Items.Clear()
            'If cboSeasons.Text <> AllSeasons Then
            Try
                pSeasons = SelectedSeasonType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
                SetAllSeasons()
                'For Each lSeasonIndex As Integer In pSeasons.AllSeasons
                '    pSeasons.SeasonSelected(lSeasonIndex) = True
                'Next
                RefreshSeasonsList()
            Catch ex As Exception
                Logger.Dbg("Could not create new seasons for '" & cboSeasons.Text & "': " & ex.ToString)
            End Try
            'End If
        End If
    End Sub

    Private Sub RefreshSeasonsList()
        Try
            Dim lMaxWidth As Integer = 0
            Dim lSeasonName As String
            lstSeasons.Items.Clear()
            For Each lSeasonIndex As Integer In pAllSeasons
                lSeasonName = pSeasons.SeasonName(lSeasonIndex)
                If lSeasonName.Length > lMaxWidth Then lMaxWidth = lSeasonName.Length
                lstSeasons.Items.Add(lSeasonName)
                lstSeasons.SetSelected(lstSeasons.Items.Count - 1, pSeasons.SeasonSelected(lSeasonIndex))
            Next

            lstSeasons.ColumnWidth = lstSeasons.CreateGraphics().MeasureString("X", lstSeasons.Font).Width * (lMaxWidth + 1)
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
            Dim lSeasonName As String = atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name)
            If lSeasonName.Equals(cboSeasons.Text) OrElse lSeasonName.Equals(cboSeasons.Text & "s") OrElse cboSeasons.Text.Equals(lSeasonName & "s") Then
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

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If VariationFromForm(pVariation) Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            Logger.Msg(ex.Message, "Could not create variation")
        End Try
    End Sub

    Private Function VariationFromForm(ByVal aVariation As atcVariation) As Boolean
        Try
            If txtName.Text.Trim.Length = 0 Then
                Logger.Msg("Name was not entered", "Name is required")
                Return False
            End If
            With aVariation
                .Name = txtName.Text.Trim
                .Operation = cboAttribute.Text

                If Not .IsInput Then
                    .UseEvents = chkEvents.Checked
                    If .UseEvents Then

                        .EventThreshold = CDbl(txtEventThreshold.Text)
                        .EventHigh = True
                        .EventDaysGapAllowed = CDbl(txtEventGap.Text) / 24 'atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitFactor(cboEventGapUnits.SelectedIndex)

                        If CDbl(txtEventVolume.Text) > 0 Then 'chkEventVolume.Checked Then
                            .EventVolumeHigh = True
                            .EventVolumeThreshold = CDbl(txtEventVolume.Text)
                        Else
                            .EventVolumeThreshold = pNaN
                        End If

                        If CDbl(txtEventDuration.Text) > 0 Then 'chkEventDuration.Checked Then
                            .EventDurationHigh = True
                            .EventDurationDays = CDbl(txtEventDuration.Text) / 24 'atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitFactor(cboEventDurationUnits.SelectedIndex)
                        Else
                            .EventDurationDays = pNaN
                        End If
                    End If

                    If chkSeasons.Checked Then
                        .Seasons = pSeasons
                        If Not pSeasons Is Nothing Then
                            For Each lSeasonIndex As Integer In pAllSeasons
                                pSeasons.SeasonSelected(lSeasonIndex) = lstSeasons.SelectedItems.Contains(pSeasons.SeasonName(lSeasonIndex))
                            Next
                        End If
                    Else
                        .Seasons = Nothing
                    End If
                End If

                If Not Double.TryParse(txtMin.Text, .Min) Then
                    .Min = GetNaN()
                End If

                If Not Double.TryParse(txtMax.Text, .Max) Then
                    .Max = GetNaN()
                End If

                .ColorAboveMax = txtHighColor.BackColor
                .ColorBelowMin = txtLowColor.BackColor
                .ColorDefault = txtDefaultColor.BackColor
            End With
        Catch ex As Exception
            Logger.Msg(ex.Message, "Could not create endpoint")
            Return False
        End Try
        Return True
    End Function

    Private Sub UserSelectColor(ByVal txt As Windows.Forms.TextBox)
        Dim cdlg As New Windows.Forms.ColorDialog
        cdlg.Color = txt.BackColor
        If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            txt.BackColor = cdlg.Color
            txt.Text = cdlg.Color.Name
        End If
    End Sub

    Private Sub txtDefaultColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDefaultColor.Click
        UserSelectColor(txtDefaultColor)
    End Sub

    Private Sub txtDefaultColor_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDefaultColor.KeyPress
        UserSelectColor(txtDefaultColor)
    End Sub

    Private Sub txtHighColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHighColor.Click
        UserSelectColor(txtHighColor)
    End Sub

    Private Sub txtHighColor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHighColor.KeyPress
        UserSelectColor(txtHighColor)
    End Sub

    Private Sub txtLowColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLowColor.Click
        UserSelectColor(txtLowColor)
    End Sub

    Private Sub txtLowColor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLowColor.KeyPress
        UserSelectColor(txtLowColor)
    End Sub

    Private Sub FormFromVariation()
        With pVariation
            txtName.Text = .Name
            txtOperation.Text = .Operation
            cboAttribute.Text = .Operation
            If Double.IsNaN(.Min) Then
                txtMin.Text = pNotNumberString
            Else
                txtMin.Text = CStr(.Min)
            End If
            If Double.IsNaN(.Max) Then
                txtMax.Text = pNotNumberString
            Else
                txtMax.Text = CStr(.Max)
            End If
            txtHighColor.BackColor = .ColorAboveMax
            txtHighColor.Text = .ColorAboveMax.Name

            txtLowColor.BackColor = .ColorBelowMin
            txtLowColor.Text = .ColorBelowMin.Name

            txtDefaultColor.BackColor = .ColorDefault
            txtDefaultColor.Text = .ColorDefault.Name

            UpdateDataText(txtData, pVariation.DataSets)

            If .IsInput Then
                grpEvents.Visible = False
                grpSeasons.Visible = False
            Else
                If Not .UseEvents Then
                    EnableEvents(False)
                Else
                    chkEvents.Checked = True
                    txtEventThreshold.Text = .EventThreshold
                    txtEventGap.Text = DoubleToString(.EventDaysGapAllowed * 24) 'atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitFactor(lGapUnitIndex)
                    If Double.IsNaN(.EventDurationDays) Then
                        txtEventDuration.Text = "0"
                    Else
                        txtEventDuration.Text = DoubleToString(.EventDurationDays * 24)
                    End If
                    If Double.IsNaN(.EventVolumeThreshold) Then
                        txtEventVolume.Text = "0"
                    Else
                        txtEventVolume.Text = DoubleToString(.EventVolumeThreshold)
                    End If
                End If

                If .Seasons Is Nothing Then
                    EnableSeasons(False)
                Else
                    pSettingFormSeason = True
                    chkSeasons.Checked = True
                    cboSeasons.Text = atcSeasonPlugin.SeasonClassNameToLabel(.Seasons.GetType.Name)
                    pSeasons = .Seasons
                    SetAllSeasons()
                    RefreshSeasonsList()
                    pSettingFormSeason = False
                End If
            End If
        End With
    End Sub

    Private Sub frmEndpoint_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Analysis\Climate Assessment Tool.html")
        End If
    End Sub

    Private Sub chkEvents_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEvents.CheckedChanged
        EnableEvents(chkEvents.Checked)
    End Sub

    Private Sub chkSeasons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasons.CheckedChanged
        EnableSeasons(chkSeasons.Checked)
    End Sub

    Private Sub EnableEvents(ByVal aEnable As Boolean)
        For Each lControl As Windows.Forms.Control In grpEvents.Controls
            If lControl IsNot chkEvents Then
                lControl.Enabled = aEnable
            End If
        Next
    End Sub

    Private Sub EnableSeasons(ByVal aEnable As Boolean)
        For Each lControl As Windows.Forms.Control In grpSeasons.Controls
            If lControl IsNot chkSeasons Then
                lControl.Enabled = aEnable
            End If
        Next

        If aEnable Then
            cboSeasons_SelectedIndexChanged(Nothing, Nothing)
        Else
            pSeasons = Nothing
        End If
    End Sub

    Private Sub btnSelectAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectAttributes.Click
        Dim lAttributesForm As New frmAttributes
        Dim lAttributes(cboAttribute.Items.Count - 1) As String
        For lItemIndex As Integer = lAttributes.GetUpperBound(0) To 0 Step -1
            lAttributes(lItemIndex) = cboAttribute.Items(lItemIndex)
        Next
        If lAttributesForm.AskUser(lAttributes) Then
            cboAttribute.Items.Clear()
            cboAttribute.Items.AddRange(lAttributes)
        End If
    End Sub
End Class
