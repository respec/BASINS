Imports atcData
Imports atcSeasons
Imports atcUtility
Imports MapWinUtility

<System.Runtime.InteropServices.ComVisible(False)> Public Class frmVariation
    Inherits System.Windows.Forms.Form

    Private Const AllSeasons As String = "All Seasons"
    Private Const pClickMe As String = "<click to specify>"

    Private pVariation As Variation
    Private pSeasonsAvailable As New atcCollection
    Private pSeasons As atcSeasonBase
    Friend WithEvents txtVaryPET As System.Windows.Forms.TextBox
    Friend WithEvents lblPET As System.Windows.Forms.Label
    Friend WithEvents lblAirTemp As System.Windows.Forms.Label
    Friend WithEvents btnViewData As System.Windows.Forms.Button
    Friend WithEvents btnViewPET As System.Windows.Forms.Button
    Friend WithEvents tabs As System.Windows.Forms.TabControl
    Friend WithEvents tabComputation As System.Windows.Forms.TabPage
    Friend WithEvents tabSeasonal As System.Windows.Forms.TabPage
    Friend WithEvents tabPET As System.Windows.Forms.TabPage
    Friend WithEvents radioFunctionAddEvents As System.Windows.Forms.RadioButton
    Friend WithEvents radioFunctionMultiply As System.Windows.Forms.RadioButton
    Friend WithEvents radioFunctionAdd As System.Windows.Forms.RadioButton
    Friend WithEvents tabEvents As System.Windows.Forms.TabPage
    Friend WithEvents cboAboveBelow As System.Windows.Forms.ComboBox
    Friend WithEvents lblDuringEvent As System.Windows.Forms.Label
    Friend WithEvents txtGap As System.Windows.Forms.TextBox
    Friend WithEvents cboGapUnits As System.Windows.Forms.ComboBox
    Friend WithEvents lblGap As System.Windows.Forms.Label
    Friend WithEvents txtThreshold As System.Windows.Forms.TextBox
    Friend WithEvents lblThreshold As System.Windows.Forms.Label
    Friend WithEvents chkEvents As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasons As System.Windows.Forms.CheckBox
    Friend WithEvents radioFunctionAddVolume As System.Windows.Forms.RadioButton
    Friend WithEvents cboAddRemovePer As System.Windows.Forms.ComboBox
    Friend WithEvents cboYearStartMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearStart As System.Windows.Forms.Label
    Friend WithEvents txtYearStartDay As System.Windows.Forms.TextBox
    Friend WithEvents groupFunction As System.Windows.Forms.GroupBox
    Friend WithEvents groupMinMax As System.Windows.Forms.GroupBox

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
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
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
        Me.cboSeasons = New System.Windows.Forms.ComboBox
        Me.lstSeasons = New System.Windows.Forms.ListBox
        Me.btnSeasonsAll = New System.Windows.Forms.Button
        Me.btnSeasonsNone = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblName = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.btnScript = New System.Windows.Forms.Button
        Me.txtVaryPET = New System.Windows.Forms.TextBox
        Me.lblPET = New System.Windows.Forms.Label
        Me.lblAirTemp = New System.Windows.Forms.Label
        Me.btnViewData = New System.Windows.Forms.Button
        Me.btnViewPET = New System.Windows.Forms.Button
        Me.tabs = New System.Windows.Forms.TabControl
        Me.tabComputation = New System.Windows.Forms.TabPage
        Me.groupMinMax = New System.Windows.Forms.GroupBox
        Me.txtYearStartDay = New System.Windows.Forms.TextBox
        Me.cboYearStartMonth = New System.Windows.Forms.ComboBox
        Me.lblYearStart = New System.Windows.Forms.Label
        Me.cboAddRemovePer = New System.Windows.Forms.ComboBox
        Me.groupFunction = New System.Windows.Forms.GroupBox
        Me.radioFunctionAdd = New System.Windows.Forms.RadioButton
        Me.radioFunctionMultiply = New System.Windows.Forms.RadioButton
        Me.radioFunctionAddEvents = New System.Windows.Forms.RadioButton
        Me.radioFunctionAddVolume = New System.Windows.Forms.RadioButton
        Me.tabEvents = New System.Windows.Forms.TabPage
        Me.chkEvents = New System.Windows.Forms.CheckBox
        Me.cboAboveBelow = New System.Windows.Forms.ComboBox
        Me.lblDuringEvent = New System.Windows.Forms.Label
        Me.txtGap = New System.Windows.Forms.TextBox
        Me.cboGapUnits = New System.Windows.Forms.ComboBox
        Me.lblGap = New System.Windows.Forms.Label
        Me.txtThreshold = New System.Windows.Forms.TextBox
        Me.lblThreshold = New System.Windows.Forms.Label
        Me.tabSeasonal = New System.Windows.Forms.TabPage
        Me.chkSeasons = New System.Windows.Forms.CheckBox
        Me.tabPET = New System.Windows.Forms.TabPage
        Me.tabs.SuspendLayout()
        Me.tabComputation.SuspendLayout()
        Me.groupMinMax.SuspendLayout()
        Me.groupFunction.SuspendLayout()
        Me.tabEvents.SuspendLayout()
        Me.tabSeasonal.SuspendLayout()
        Me.tabPET.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtIncrement
        '
        Me.txtIncrement.Location = New System.Drawing.Point(77, 67)
        Me.txtIncrement.Name = "txtIncrement"
        Me.txtIncrement.Size = New System.Drawing.Size(71, 20)
        Me.txtIncrement.TabIndex = 12
        Me.txtIncrement.Text = "0.05"
        '
        'lblIncrement
        '
        Me.lblIncrement.AutoSize = True
        Me.lblIncrement.BackColor = System.Drawing.Color.Transparent
        Me.lblIncrement.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblIncrement.Location = New System.Drawing.Point(14, 70)
        Me.lblIncrement.Name = "lblIncrement"
        Me.lblIncrement.Size = New System.Drawing.Size(57, 13)
        Me.lblIncrement.TabIndex = 11
        Me.lblIncrement.Text = "Increment:"
        Me.lblIncrement.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMax
        '
        Me.txtMax.Location = New System.Drawing.Point(77, 43)
        Me.txtMax.Name = "txtMax"
        Me.txtMax.Size = New System.Drawing.Size(71, 20)
        Me.txtMax.TabIndex = 10
        Me.txtMax.Text = "1.1"
        '
        'txtMin
        '
        Me.txtMin.Location = New System.Drawing.Point(77, 19)
        Me.txtMin.Name = "txtMin"
        Me.txtMin.Size = New System.Drawing.Size(71, 20)
        Me.txtMin.TabIndex = 9
        Me.txtMin.Text = "0.9"
        '
        'lblMaximum
        '
        Me.lblMaximum.AutoSize = True
        Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
        Me.lblMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMaximum.Location = New System.Drawing.Point(17, 46)
        Me.lblMaximum.Name = "lblMaximum"
        Me.lblMaximum.Size = New System.Drawing.Size(54, 13)
        Me.lblMaximum.TabIndex = 9
        Me.lblMaximum.Text = "Maximum:"
        Me.lblMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMinimum
        '
        Me.lblMinimum.AutoSize = True
        Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMinimum.Location = New System.Drawing.Point(20, 22)
        Me.lblMinimum.Name = "lblMinimum"
        Me.lblMinimum.Size = New System.Drawing.Size(51, 13)
        Me.lblMinimum.TabIndex = 7
        Me.lblMinimum.Text = "Minimum:"
        Me.lblMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblData
        '
        Me.lblData.AutoSize = True
        Me.lblData.BackColor = System.Drawing.Color.Transparent
        Me.lblData.Location = New System.Drawing.Point(13, 41)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(69, 13)
        Me.lblData.TabIndex = 3
        Me.lblData.Text = "Data to Vary:"
        Me.lblData.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtVaryData
        '
        Me.txtVaryData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVaryData.Location = New System.Drawing.Point(88, 38)
        Me.txtVaryData.Name = "txtVaryData"
        Me.txtVaryData.Size = New System.Drawing.Size(285, 20)
        Me.txtVaryData.TabIndex = 4
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.Enabled = False
        Me.cboSeasons.ItemHeight = 13
        Me.cboSeasons.Location = New System.Drawing.Point(3, 36)
        Me.cboSeasons.MaxDropDownItems = 20
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(401, 21)
        Me.cboSeasons.TabIndex = 14
        '
        'lstSeasons
        '
        Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSeasons.Enabled = False
        Me.lstSeasons.IntegralHeight = False
        Me.lstSeasons.Location = New System.Drawing.Point(3, 63)
        Me.lstSeasons.Name = "lstSeasons"
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(401, 190)
        Me.lstSeasons.TabIndex = 15
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Enabled = False
        Me.btnSeasonsAll.Location = New System.Drawing.Point(3, 259)
        Me.btnSeasonsAll.Name = "btnSeasonsAll"
        Me.btnSeasonsAll.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsAll.TabIndex = 16
        Me.btnSeasonsAll.Text = "All"
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Enabled = False
        Me.btnSeasonsNone.Location = New System.Drawing.Point(341, 259)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsNone.TabIndex = 17
        Me.btnSeasonsNone.Text = "None"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(280, 382)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 24)
        Me.btnOk.TabIndex = 19
        Me.btnOk.Text = "Ok"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(358, 382)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 24)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "Cancel"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.Location = New System.Drawing.Point(13, 15)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(65, 13)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Input Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(88, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(342, 20)
        Me.txtName.TabIndex = 2
        '
        'btnScript
        '
        Me.btnScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnScript.Location = New System.Drawing.Point(178, 382)
        Me.btnScript.Name = "btnScript"
        Me.btnScript.Size = New System.Drawing.Size(96, 24)
        Me.btnScript.TabIndex = 18
        Me.btnScript.Text = "Open Script..."
        '
        'txtVaryPET
        '
        Me.txtVaryPET.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVaryPET.Location = New System.Drawing.Point(79, 32)
        Me.txtVaryPET.Name = "txtVaryPET"
        Me.txtVaryPET.Size = New System.Drawing.Size(270, 20)
        Me.txtVaryPET.TabIndex = 6
        '
        'lblPET
        '
        Me.lblPET.AutoSize = True
        Me.lblPET.BackColor = System.Drawing.Color.Transparent
        Me.lblPET.Location = New System.Drawing.Point(6, 35)
        Me.lblPET.Name = "lblPET"
        Me.lblPET.Size = New System.Drawing.Size(67, 13)
        Me.lblPET.TabIndex = 21
        Me.lblPET.Text = "PET to Vary:"
        Me.lblPET.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblAirTemp
        '
        Me.lblAirTemp.AutoSize = True
        Me.lblAirTemp.BackColor = System.Drawing.Color.Transparent
        Me.lblAirTemp.Location = New System.Drawing.Point(6, 15)
        Me.lblAirTemp.Name = "lblAirTemp"
        Me.lblAirTemp.Size = New System.Drawing.Size(346, 13)
        Me.lblAirTemp.TabIndex = 22
        Me.lblAirTemp.Text = "If the data being varied is air temperature, PET can be computed from it:"
        Me.lblAirTemp.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnViewData
        '
        Me.btnViewData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewData.Location = New System.Drawing.Point(379, 38)
        Me.btnViewData.Name = "btnViewData"
        Me.btnViewData.Size = New System.Drawing.Size(51, 21)
        Me.btnViewData.TabIndex = 5
        Me.btnViewData.Text = "View"
        '
        'btnViewPET
        '
        Me.btnViewPET.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewPET.Location = New System.Drawing.Point(355, 32)
        Me.btnViewPET.Name = "btnViewPET"
        Me.btnViewPET.Size = New System.Drawing.Size(51, 21)
        Me.btnViewPET.TabIndex = 7
        Me.btnViewPET.Text = "View"
        '
        'tabs
        '
        Me.tabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabs.Controls.Add(Me.tabComputation)
        Me.tabs.Controls.Add(Me.tabEvents)
        Me.tabs.Controls.Add(Me.tabSeasonal)
        Me.tabs.Controls.Add(Me.tabPET)
        Me.tabs.Location = New System.Drawing.Point(12, 65)
        Me.tabs.Name = "tabs"
        Me.tabs.SelectedIndex = 0
        Me.tabs.Size = New System.Drawing.Size(418, 311)
        Me.tabs.TabIndex = 23
        '
        'tabComputation
        '
        Me.tabComputation.Controls.Add(Me.groupMinMax)
        Me.tabComputation.Controls.Add(Me.groupFunction)
        Me.tabComputation.Location = New System.Drawing.Point(4, 22)
        Me.tabComputation.Name = "tabComputation"
        Me.tabComputation.Padding = New System.Windows.Forms.Padding(3)
        Me.tabComputation.Size = New System.Drawing.Size(410, 285)
        Me.tabComputation.TabIndex = 0
        Me.tabComputation.Text = "Computation"
        Me.tabComputation.UseVisualStyleBackColor = True
        '
        'groupMinMax
        '
        Me.groupMinMax.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupMinMax.Controls.Add(Me.txtMin)
        Me.groupMinMax.Controls.Add(Me.txtIncrement)
        Me.groupMinMax.Controls.Add(Me.txtYearStartDay)
        Me.groupMinMax.Controls.Add(Me.txtMax)
        Me.groupMinMax.Controls.Add(Me.cboYearStartMonth)
        Me.groupMinMax.Controls.Add(Me.lblIncrement)
        Me.groupMinMax.Controls.Add(Me.lblYearStart)
        Me.groupMinMax.Controls.Add(Me.lblMaximum)
        Me.groupMinMax.Controls.Add(Me.cboAddRemovePer)
        Me.groupMinMax.Controls.Add(Me.lblMinimum)
        Me.groupMinMax.Location = New System.Drawing.Point(6, 127)
        Me.groupMinMax.Name = "groupMinMax"
        Me.groupMinMax.Size = New System.Drawing.Size(398, 152)
        Me.groupMinMax.TabIndex = 23
        Me.groupMinMax.TabStop = False
        '
        'txtYearStartDay
        '
        Me.txtYearStartDay.Location = New System.Drawing.Point(154, 121)
        Me.txtYearStartDay.Name = "txtYearStartDay"
        Me.txtYearStartDay.Size = New System.Drawing.Size(44, 20)
        Me.txtYearStartDay.TabIndex = 21
        Me.txtYearStartDay.Text = "1"
        Me.txtYearStartDay.Visible = False
        '
        'cboYearStartMonth
        '
        Me.cboYearStartMonth.FormattingEnabled = True
        Me.cboYearStartMonth.Items.AddRange(New Object() {"Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Nov", "Dec"})
        Me.cboYearStartMonth.Location = New System.Drawing.Point(77, 120)
        Me.cboYearStartMonth.Name = "cboYearStartMonth"
        Me.cboYearStartMonth.Size = New System.Drawing.Size(71, 21)
        Me.cboYearStartMonth.TabIndex = 20
        Me.cboYearStartMonth.Visible = False
        '
        'lblYearStart
        '
        Me.lblYearStart.AutoSize = True
        Me.lblYearStart.BackColor = System.Drawing.Color.Transparent
        Me.lblYearStart.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblYearStart.Location = New System.Drawing.Point(9, 123)
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
        Me.cboAddRemovePer.Location = New System.Drawing.Point(77, 93)
        Me.cboAddRemovePer.Name = "cboAddRemovePer"
        Me.cboAddRemovePer.Size = New System.Drawing.Size(121, 21)
        Me.cboAddRemovePer.TabIndex = 18
        Me.cboAddRemovePer.Visible = False
        '
        'groupFunction
        '
        Me.groupFunction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupFunction.Controls.Add(Me.radioFunctionAdd)
        Me.groupFunction.Controls.Add(Me.radioFunctionMultiply)
        Me.groupFunction.Controls.Add(Me.radioFunctionAddEvents)
        Me.groupFunction.Controls.Add(Me.radioFunctionAddVolume)
        Me.groupFunction.Location = New System.Drawing.Point(6, 6)
        Me.groupFunction.Name = "groupFunction"
        Me.groupFunction.Size = New System.Drawing.Size(398, 115)
        Me.groupFunction.TabIndex = 22
        Me.groupFunction.TabStop = False
        Me.groupFunction.Text = "Function"
        '
        'radioFunctionAdd
        '
        Me.radioFunctionAdd.AutoSize = True
        Me.radioFunctionAdd.Location = New System.Drawing.Point(12, 42)
        Me.radioFunctionAdd.Name = "radioFunctionAdd"
        Me.radioFunctionAdd.Size = New System.Drawing.Size(168, 17)
        Me.radioFunctionAdd.TabIndex = 13
        Me.radioFunctionAdd.Tag = "Constant to add to each value"
        Me.radioFunctionAdd.Text = "Add a Constant to Each Value"
        Me.radioFunctionAdd.UseVisualStyleBackColor = True
        '
        'radioFunctionMultiply
        '
        Me.radioFunctionMultiply.AutoSize = True
        Me.radioFunctionMultiply.Location = New System.Drawing.Point(12, 19)
        Me.radioFunctionMultiply.Name = "radioFunctionMultiply"
        Me.radioFunctionMultiply.Size = New System.Drawing.Size(186, 17)
        Me.radioFunctionMultiply.TabIndex = 14
        Me.radioFunctionMultiply.Tag = "Constant to multiply each value by"
        Me.radioFunctionMultiply.Text = "Multiply Each Value by a Constant"
        Me.radioFunctionMultiply.UseVisualStyleBackColor = True
        '
        'radioFunctionAddEvents
        '
        Me.radioFunctionAddEvents.AutoSize = True
        Me.radioFunctionAddEvents.Location = New System.Drawing.Point(12, 65)
        Me.radioFunctionAddEvents.Name = "radioFunctionAddEvents"
        Me.radioFunctionAddEvents.Size = New System.Drawing.Size(244, 17)
        Me.radioFunctionAddEvents.TabIndex = 15
        Me.radioFunctionAddEvents.Tag = "Volume to add (negative to remove)"
        Me.radioFunctionAddEvents.Text = "Add/Remove Events to Reach Target Volume"
        Me.radioFunctionAddEvents.UseVisualStyleBackColor = True
        '
        'radioFunctionAddVolume
        '
        Me.radioFunctionAddVolume.AutoSize = True
        Me.radioFunctionAddVolume.Location = New System.Drawing.Point(12, 88)
        Me.radioFunctionAddVolume.Name = "radioFunctionAddVolume"
        Me.radioFunctionAddVolume.Size = New System.Drawing.Size(214, 17)
        Me.radioFunctionAddVolume.TabIndex = 16
        Me.radioFunctionAddVolume.Tag = "Volume to add (negative to remove)"
        Me.radioFunctionAddVolume.Text = "Multiply Values to Reach Target Volume"
        Me.radioFunctionAddVolume.UseVisualStyleBackColor = True
        Me.radioFunctionAddVolume.Visible = False
        '
        'tabEvents
        '
        Me.tabEvents.Controls.Add(Me.chkEvents)
        Me.tabEvents.Controls.Add(Me.cboAboveBelow)
        Me.tabEvents.Controls.Add(Me.lblDuringEvent)
        Me.tabEvents.Controls.Add(Me.txtGap)
        Me.tabEvents.Controls.Add(Me.cboGapUnits)
        Me.tabEvents.Controls.Add(Me.lblGap)
        Me.tabEvents.Controls.Add(Me.txtThreshold)
        Me.tabEvents.Controls.Add(Me.lblThreshold)
        Me.tabEvents.Location = New System.Drawing.Point(4, 22)
        Me.tabEvents.Name = "tabEvents"
        Me.tabEvents.Size = New System.Drawing.Size(410, 285)
        Me.tabEvents.TabIndex = 3
        Me.tabEvents.Text = "Events"
        Me.tabEvents.UseVisualStyleBackColor = True
        '
        'chkEvents
        '
        Me.chkEvents.AutoSize = True
        Me.chkEvents.Location = New System.Drawing.Point(6, 13)
        Me.chkEvents.Name = "chkEvents"
        Me.chkEvents.Size = New System.Drawing.Size(211, 17)
        Me.chkEvents.TabIndex = 24
        Me.chkEvents.Text = "Apply computation only to these events"
        Me.chkEvents.UseVisualStyleBackColor = True
        '
        'cboAboveBelow
        '
        Me.cboAboveBelow.Enabled = False
        Me.cboAboveBelow.FormattingEnabled = True
        Me.cboAboveBelow.Items.AddRange(New Object() {"Above", "Below"})
        Me.cboAboveBelow.Location = New System.Drawing.Point(52, 36)
        Me.cboAboveBelow.Name = "cboAboveBelow"
        Me.cboAboveBelow.Size = New System.Drawing.Size(69, 21)
        Me.cboAboveBelow.TabIndex = 18
        '
        'lblDuringEvent
        '
        Me.lblDuringEvent.AutoSize = True
        Me.lblDuringEvent.Enabled = False
        Me.lblDuringEvent.Location = New System.Drawing.Point(313, 65)
        Me.lblDuringEvent.Name = "lblDuringEvent"
        Me.lblDuringEvent.Size = New System.Drawing.Size(81, 13)
        Me.lblDuringEvent.TabIndex = 23
        Me.lblDuringEvent.Text = "during an event"
        '
        'txtGap
        '
        Me.txtGap.Enabled = False
        Me.txtGap.Location = New System.Drawing.Point(127, 62)
        Me.txtGap.Name = "txtGap"
        Me.txtGap.Size = New System.Drawing.Size(66, 20)
        Me.txtGap.TabIndex = 20
        Me.txtGap.Text = "0"
        '
        'cboGapUnits
        '
        Me.cboGapUnits.Enabled = False
        Me.cboGapUnits.FormattingEnabled = True
        Me.cboGapUnits.Location = New System.Drawing.Point(199, 62)
        Me.cboGapUnits.Name = "cboGapUnits"
        Me.cboGapUnits.Size = New System.Drawing.Size(108, 21)
        Me.cboGapUnits.TabIndex = 21
        '
        'lblGap
        '
        Me.lblGap.AutoSize = True
        Me.lblGap.Enabled = False
        Me.lblGap.Location = New System.Drawing.Point(3, 65)
        Me.lblGap.Name = "lblGap"
        Me.lblGap.Size = New System.Drawing.Size(99, 13)
        Me.lblGap.TabIndex = 22
        Me.lblGap.Text = "Allow Gaps of up to"
        '
        'txtThreshold
        '
        Me.txtThreshold.Enabled = False
        Me.txtThreshold.Location = New System.Drawing.Point(127, 36)
        Me.txtThreshold.Name = "txtThreshold"
        Me.txtThreshold.Size = New System.Drawing.Size(66, 20)
        Me.txtThreshold.TabIndex = 19
        Me.txtThreshold.Text = "0"
        '
        'lblThreshold
        '
        Me.lblThreshold.AutoSize = True
        Me.lblThreshold.Enabled = False
        Me.lblThreshold.Location = New System.Drawing.Point(3, 39)
        Me.lblThreshold.Name = "lblThreshold"
        Me.lblThreshold.Size = New System.Drawing.Size(40, 13)
        Me.lblThreshold.TabIndex = 17
        Me.lblThreshold.Text = "Events"
        '
        'tabSeasonal
        '
        Me.tabSeasonal.Controls.Add(Me.chkSeasons)
        Me.tabSeasonal.Controls.Add(Me.cboSeasons)
        Me.tabSeasonal.Controls.Add(Me.lstSeasons)
        Me.tabSeasonal.Controls.Add(Me.btnSeasonsNone)
        Me.tabSeasonal.Controls.Add(Me.btnSeasonsAll)
        Me.tabSeasonal.Location = New System.Drawing.Point(4, 22)
        Me.tabSeasonal.Name = "tabSeasonal"
        Me.tabSeasonal.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSeasonal.Size = New System.Drawing.Size(410, 285)
        Me.tabSeasonal.TabIndex = 1
        Me.tabSeasonal.Text = "Seasonal"
        Me.tabSeasonal.UseVisualStyleBackColor = True
        '
        'chkSeasons
        '
        Me.chkSeasons.AutoSize = True
        Me.chkSeasons.Location = New System.Drawing.Point(6, 13)
        Me.chkSeasons.Name = "chkSeasons"
        Me.chkSeasons.Size = New System.Drawing.Size(263, 17)
        Me.chkSeasons.TabIndex = 25
        Me.chkSeasons.Text = "Apply computation only to values in these seasons"
        Me.chkSeasons.UseVisualStyleBackColor = True
        '
        'tabPET
        '
        Me.tabPET.Controls.Add(Me.lblAirTemp)
        Me.tabPET.Controls.Add(Me.btnViewPET)
        Me.tabPET.Controls.Add(Me.lblPET)
        Me.tabPET.Controls.Add(Me.txtVaryPET)
        Me.tabPET.Location = New System.Drawing.Point(4, 22)
        Me.tabPET.Name = "tabPET"
        Me.tabPET.Size = New System.Drawing.Size(410, 285)
        Me.tabPET.TabIndex = 2
        Me.tabPET.Text = "PET"
        Me.tabPET.UseVisualStyleBackColor = True
        '
        'frmVariation
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(442, 418)
        Me.Controls.Add(Me.tabs)
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
        Me.tabs.ResumeLayout(False)
        Me.tabComputation.ResumeLayout(False)
        Me.groupMinMax.ResumeLayout(False)
        Me.groupMinMax.PerformLayout()
        Me.groupFunction.ResumeLayout(False)
        Me.groupFunction.PerformLayout()
        Me.tabEvents.ResumeLayout(False)
        Me.tabEvents.PerformLayout()
        Me.tabSeasonal.ResumeLayout(False)
        Me.tabSeasonal.PerformLayout()
        Me.tabPET.ResumeLayout(False)
        Me.tabPET.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Function AskUser(ByRef aVariation As Variation) As Boolean
        pVariation = aVariation.Clone

        If pVariation.DataSets Is Nothing Then pVariation.DataSets = New atcDataGroup

        cboSeasons.Items.Add(AllSeasons)
        cboGapUnits.Items.AddRange(atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitNames)

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
                Me.Close()
            End If
        Catch ex As Exception
            Logger.Msg(ex.Message, "Could not create variation")
        End Try
    End Sub

    Private Function VariationFromForm(ByVal aVariation As Variation) As Boolean
        With aVariation
            .Name = txtName.Text
            If txtVaryData.Text.Equals(pClickMe) Then
                Logger.Msg("No data was selected", "Need Data To Vary")
                Return False
            End If

            If radioFunctionAdd.Checked Then
                .Operation = "Add"
            ElseIf radioFunctionMultiply.Checked Then
                .Operation = "Multiply"
            ElseIf radioFunctionAddEvents.Checked Then
                .Operation = "AddEvents"
            ElseIf radioFunctionAddVolume.Checked Then
                .Operation = "AddVolume"
            End If
            .EventsPer = cboAddRemovePer.Text

            .UseEvents = chkEvents.Checked
            If .UseEvents Then
                .EventThreshold = CDbl(txtThreshold.Text)
                If cboAboveBelow.Text = "Above" Then
                    .EventHigh = True
                Else
                    .EventHigh = False
                End If
                .EventGapDisplayUnits = cboGapUnits.Text
                .EventDaysGapAllowed = CDbl(txtGap.Text) / atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitFactor(cboGapUnits.SelectedIndex)
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

        End With
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
            Select Case .Operation
                Case "Add" : radioFunctionAdd.Checked = True
                Case "Multiply" : radioFunctionMultiply.Checked = True
                Case "AddEvents" : radioFunctionAddEvents.Checked = True
                Case "AddVolume" : radioFunctionAddVolume.Checked = True
            End Select
            If Not Double.IsNaN(.Min) Then txtMin.Text = .Min
            If Not Double.IsNaN(.Max) Then txtMax.Text = .Max
            If Not Double.IsNaN(.Increment) Then txtIncrement.Text = .Increment
            UpdateDataText(txtVaryData, pVariation.DataSets)
            UpdateDataText(txtVaryPET, pVariation.PETdata)

            cboAddRemovePer.Text = .EventsPer

            chkEvents.Checked = .UseEvents
            If .UseEvents Then
                If .EventHigh Then
                    cboAboveBelow.SelectedIndex = 0
                Else
                    cboAboveBelow.SelectedIndex = 1
                End If
                txtThreshold.Text = .EventThreshold

                Dim lGapUnitIndex As Integer = Array.IndexOf(atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitNames, .EventGapDisplayUnits)
                If lGapUnitIndex >= 0 Then
                    cboGapUnits.Text = .EventGapDisplayUnits
                    txtGap.Text = .EventDaysGapAllowed * atcSynopticAnalysis.atcSynopticAnalysisPlugin.TimeUnitFactor(lGapUnitIndex)
                End If
            Else
                chkEvents.Checked = False
            End If

            If .Seasons Is Nothing Then
                cboSeasons.SelectedIndex = 0
            Else
                pSettingFormSeason = True
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

    Private Sub chkEvents_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEvents.CheckedChanged
        EnableEvents(chkEvents.Checked)
    End Sub

    Private Sub chkSeasons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasons.CheckedChanged
        EnableSeasons(chkSeasons.Checked)
    End Sub

    Private Sub EnableEvents(ByVal aEnable As Boolean)
        lblThreshold.Enabled = aEnable
        cboAboveBelow.Enabled = aEnable
        txtThreshold.Enabled = aEnable
        lblGap.Enabled = aEnable
        txtGap.Enabled = aEnable
        cboGapUnits.Enabled = aEnable
        lblDuringEvent.Enabled = aEnable
    End Sub

    Private Sub EnableSeasons(ByVal aEnable As Boolean)
        cboSeasons.Enabled = aEnable
        lstSeasons.Enabled = aEnable
        btnSeasonsAll.Enabled = aEnable
        btnSeasonsNone.Enabled = aEnable
    End Sub

    Private Sub radioFunction_CheckedChanged(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs) Handles _
                                            radioFunctionAdd.CheckedChanged, _
                                            radioFunctionMultiply.CheckedChanged, _
                                            radioFunctionAddEvents.CheckedChanged, _
                                            radioFunctionAddVolume.CheckedChanged
        Dim lChk As Windows.Forms.RadioButton = sender
        If lChk.Checked Then
            groupMinMax.Text = lChk.Tag
            If lChk.Text.IndexOf("Volume") >= 0 Then
                cboAddRemovePer.Visible = True
                If cboAddRemovePer.SelectedIndex < 0 Then
                    cboAddRemovePer.SelectedIndex = 0
                End If
            Else
                cboAddRemovePer.Visible = False
            End If
            ShowYearStart()
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

End Class
