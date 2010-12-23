Imports atcData
Imports atcSeasons
Imports atcUtility
Imports MapWinUtility

<System.Runtime.InteropServices.ComVisible(False)> Public Class frmVariation
    Inherits System.Windows.Forms.Form

    Private pNaN As Double = GetNaN()
    Private pSettingFormSeason As Boolean = False

    Private pFunctionLabels As String() = {"Change Temperature", "Multiply Existing Values by a Number", "Add/Remove Storm Events", "Add/Remove Volume in Extreme Events"}
    Private pFunctionGroupLabels As String() = {"Degrees to add to each existing temperature value", "Number to multiply existing data by", "Percent Change in Volume", "Percent Change in Volume"}
    Private pFunctionOperations As String() = {"Add", "Multiply", "AddEvents", "Intensify"}
    Private pFunctionUnits As String() = {"degrees", "multiplication factor", "%", "%"}

    Private pVariation As atcVariation
    Private pSeasonTypesAvailable As New atcCollection
    Private pSeasons As atcSeasonBase
    Private pAllSeasons As Integer()

#Region " Windows Form Designer generated code "

    Friend WithEvents txtVaryPET As System.Windows.Forms.TextBox
    Friend WithEvents btnViewData As System.Windows.Forms.Button
    Friend WithEvents btnViewPET As System.Windows.Forms.Button
    Friend WithEvents txtEventGap As System.Windows.Forms.TextBox
    Friend WithEvents txtEventThreshold As System.Windows.Forms.TextBox
    Friend WithEvents chkEvents As System.Windows.Forms.CheckBox
    Friend WithEvents txtEventDuration As System.Windows.Forms.TextBox
    Friend WithEvents txtEventVolume As System.Windows.Forms.TextBox
    Friend WithEvents cboFunction As System.Windows.Forms.ComboBox
    Friend WithEvents lblFunction As System.Windows.Forms.Label
    Friend WithEvents lblThreshold As System.Windows.Forms.Label
    Friend WithEvents chkSeasons As System.Windows.Forms.CheckBox
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
    Friend WithEvents lblIncrement2 As System.Windows.Forms.Label
    Friend WithEvents lblValueUnitsMaximum As System.Windows.Forms.Label
    Friend WithEvents grpMinMax As System.Windows.Forms.GroupBox
    Friend WithEvents grpEvents As System.Windows.Forms.GroupBox
    Friend WithEvents lblPET As System.Windows.Forms.Label
    Friend WithEvents lblVolume As System.Windows.Forms.Label
    Friend WithEvents lblGap As System.Windows.Forms.Label
    Friend WithEvents lblVolumeUnits As System.Windows.Forms.Label
    Friend WithEvents lblThresholdUnits As System.Windows.Forms.Label
    Friend WithEvents lblDurationUnits As System.Windows.Forms.Label
    Friend WithEvents lblGapUnits As System.Windows.Forms.Label
    Friend WithEvents lblDuration As System.Windows.Forms.Label
    Friend WithEvents lblValueUnitsMinimum As System.Windows.Forms.Label
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents lblVolumePercent As System.Windows.Forms.Label
    Friend WithEvents txtVolumePercent As System.Windows.Forms.TextBox
    Friend WithEvents lblVolumePercent2 As System.Windows.Forms.Label
    Friend WithEvents radioSingle As System.Windows.Forms.RadioButton
    Friend WithEvents radioIterate As System.Windows.Forms.RadioButton
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox

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
        Me.txtEventDuration = New System.Windows.Forms.TextBox
        Me.txtEventVolume = New System.Windows.Forms.TextBox
        Me.chkEvents = New System.Windows.Forms.CheckBox
        Me.txtEventGap = New System.Windows.Forms.TextBox
        Me.txtEventThreshold = New System.Windows.Forms.TextBox
        Me.cboFunction = New System.Windows.Forms.ComboBox
        Me.lblFunction = New System.Windows.Forms.Label
        Me.lblThreshold = New System.Windows.Forms.Label
        Me.chkSeasons = New System.Windows.Forms.CheckBox
        Me.btnSeasonsNone = New System.Windows.Forms.Button
        Me.btnSeasonsAll = New System.Windows.Forms.Button
        Me.grpSeasons = New System.Windows.Forms.GroupBox
        Me.lstSeasons = New System.Windows.Forms.ListBox
        Me.cboSeasons = New System.Windows.Forms.ComboBox
        Me.lblIncrement2 = New System.Windows.Forms.Label
        Me.lblValueUnitsMaximum = New System.Windows.Forms.Label
        Me.grpMinMax = New System.Windows.Forms.GroupBox
        Me.radioSingle = New System.Windows.Forms.RadioButton
        Me.radioIterate = New System.Windows.Forms.RadioButton
        Me.lblValueUnitsMinimum = New System.Windows.Forms.Label
        Me.txtVolumePercent = New System.Windows.Forms.TextBox
        Me.lblVolumePercent = New System.Windows.Forms.Label
        Me.lblVolumePercent2 = New System.Windows.Forms.Label
        Me.grpEvents = New System.Windows.Forms.GroupBox
        Me.lblVolumeUnits = New System.Windows.Forms.Label
        Me.lblThresholdUnits = New System.Windows.Forms.Label
        Me.lblDurationUnits = New System.Windows.Forms.Label
        Me.lblGapUnits = New System.Windows.Forms.Label
        Me.lblDuration = New System.Windows.Forms.Label
        Me.lblVolume = New System.Windows.Forms.Label
        Me.lblGap = New System.Windows.Forms.Label
        Me.lblPET = New System.Windows.Forms.Label
        Me.grpSeasons.SuspendLayout()
        Me.grpMinMax.SuspendLayout()
        Me.grpEvents.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtIncrement
        '
        Me.txtIncrement.Location = New System.Drawing.Point(75, 94)
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
        Me.lblIncrement.Location = New System.Drawing.Point(6, 97)
        Me.lblIncrement.Name = "lblIncrement"
        Me.lblIncrement.Size = New System.Drawing.Size(57, 13)
        Me.lblIncrement.TabIndex = 17
        Me.lblIncrement.Text = "Increment:"
        '
        'txtMax
        '
        Me.txtMax.Location = New System.Drawing.Point(75, 68)
        Me.txtMax.Name = "txtMax"
        Me.txtMax.Size = New System.Drawing.Size(71, 20)
        Me.txtMax.TabIndex = 16
        Me.txtMax.Text = "1.1"
        '
        'txtMin
        '
        Me.txtMin.Location = New System.Drawing.Point(75, 42)
        Me.txtMin.Name = "txtMin"
        Me.txtMin.Size = New System.Drawing.Size(71, 20)
        Me.txtMin.TabIndex = 14
        Me.txtMin.Text = "0.9"
        '
        'lblMaximum
        '
        Me.lblMaximum.AutoSize = True
        Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
        Me.lblMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMaximum.Location = New System.Drawing.Point(6, 71)
        Me.lblMaximum.Name = "lblMaximum"
        Me.lblMaximum.Size = New System.Drawing.Size(54, 13)
        Me.lblMaximum.TabIndex = 15
        Me.lblMaximum.Text = "Maximum:"
        '
        'lblMinimum
        '
        Me.lblMinimum.AutoSize = True
        Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMinimum.Location = New System.Drawing.Point(6, 45)
        Me.lblMinimum.Name = "lblMinimum"
        Me.lblMinimum.Size = New System.Drawing.Size(51, 13)
        Me.lblMinimum.TabIndex = 13
        Me.lblMinimum.Text = "Minimum:"
        '
        'lblData
        '
        Me.lblData.AutoSize = True
        Me.lblData.BackColor = System.Drawing.Color.Transparent
        Me.lblData.Location = New System.Drawing.Point(12, 42)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(118, 13)
        Me.lblData.TabIndex = 2
        Me.lblData.Text = "Existing Data to Modify:"
        '
        'txtVaryData
        '
        Me.txtVaryData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVaryData.Location = New System.Drawing.Point(136, 38)
        Me.txtVaryData.Name = "txtVaryData"
        Me.txtVaryData.Size = New System.Drawing.Size(212, 20)
        Me.txtVaryData.TabIndex = 3
        Me.txtVaryData.Tag = "<click to specify data to modify>"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(255, 565)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 24)
        Me.btnOk.TabIndex = 45
        Me.btnOk.Text = "Ok"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(333, 565)
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
        Me.lblName.Size = New System.Drawing.Size(98, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Modification Name:"
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(136, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(212, 20)
        Me.txtName.TabIndex = 1
        '
        'btnScript
        '
        Me.btnScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnScript.Location = New System.Drawing.Point(153, 565)
        Me.btnScript.Name = "btnScript"
        Me.btnScript.Size = New System.Drawing.Size(96, 24)
        Me.btnScript.TabIndex = 44
        Me.btnScript.Text = "Open Script..."
        Me.btnScript.Visible = False
        '
        'txtVaryPET
        '
        Me.txtVaryPET.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVaryPET.Location = New System.Drawing.Point(136, 64)
        Me.txtVaryPET.Name = "txtVaryPET"
        Me.txtVaryPET.Size = New System.Drawing.Size(212, 20)
        Me.txtVaryPET.TabIndex = 6
        Me.txtVaryPET.Tag = "<click to specify PET to replace>"
        '
        'btnViewData
        '
        Me.btnViewData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewData.Location = New System.Drawing.Point(354, 37)
        Me.btnViewData.Name = "btnViewData"
        Me.btnViewData.Size = New System.Drawing.Size(51, 21)
        Me.btnViewData.TabIndex = 4
        Me.btnViewData.Text = "View"
        '
        'btnViewPET
        '
        Me.btnViewPET.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewPET.Location = New System.Drawing.Point(354, 64)
        Me.btnViewPET.Name = "btnViewPET"
        Me.btnViewPET.Size = New System.Drawing.Size(51, 21)
        Me.btnViewPET.TabIndex = 7
        Me.btnViewPET.Text = "View"
        '
        'txtEventDuration
        '
        Me.txtEventDuration.Location = New System.Drawing.Point(139, 120)
        Me.txtEventDuration.Name = "txtEventDuration"
        Me.txtEventDuration.Size = New System.Drawing.Size(66, 20)
        Me.txtEventDuration.TabIndex = 36
        Me.txtEventDuration.Text = "0"
        '
        'txtEventVolume
        '
        Me.txtEventVolume.Location = New System.Drawing.Point(139, 94)
        Me.txtEventVolume.Name = "txtEventVolume"
        Me.txtEventVolume.Size = New System.Drawing.Size(66, 20)
        Me.txtEventVolume.TabIndex = 33
        Me.txtEventVolume.Text = "0"
        '
        'chkEvents
        '
        Me.chkEvents.AutoSize = True
        Me.chkEvents.BackColor = System.Drawing.Color.Transparent
        Me.chkEvents.Location = New System.Drawing.Point(6, 19)
        Me.chkEvents.Name = "chkEvents"
        Me.chkEvents.Size = New System.Drawing.Size(238, 17)
        Me.chkEvents.TabIndex = 24
        Me.chkEvents.Text = "Vary precipitation only in the following Events"
        Me.chkEvents.UseVisualStyleBackColor = False
        '
        'txtEventGap
        '
        Me.txtEventGap.Location = New System.Drawing.Point(139, 68)
        Me.txtEventGap.Name = "txtEventGap"
        Me.txtEventGap.Size = New System.Drawing.Size(66, 20)
        Me.txtEventGap.TabIndex = 29
        Me.txtEventGap.Text = "0"
        '
        'txtEventThreshold
        '
        Me.txtEventThreshold.Location = New System.Drawing.Point(139, 42)
        Me.txtEventThreshold.Name = "txtEventThreshold"
        Me.txtEventThreshold.Size = New System.Drawing.Size(66, 20)
        Me.txtEventThreshold.TabIndex = 27
        Me.txtEventThreshold.Text = "0"
        '
        'cboFunction
        '
        Me.cboFunction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFunction.FormattingEnabled = True
        Me.cboFunction.Location = New System.Drawing.Point(136, 90)
        Me.cboFunction.Name = "cboFunction"
        Me.cboFunction.Size = New System.Drawing.Size(212, 21)
        Me.cboFunction.TabIndex = 9
        '
        'lblFunction
        '
        Me.lblFunction.AutoSize = True
        Me.lblFunction.BackColor = System.Drawing.Color.Transparent
        Me.lblFunction.Location = New System.Drawing.Point(12, 93)
        Me.lblFunction.Name = "lblFunction"
        Me.lblFunction.Size = New System.Drawing.Size(78, 13)
        Me.lblFunction.TabIndex = 8
        Me.lblFunction.Text = "How to Modify:"
        '
        'lblThreshold
        '
        Me.lblThreshold.AutoSize = True
        Me.lblThreshold.BackColor = System.Drawing.Color.Transparent
        Me.lblThreshold.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblThreshold.Location = New System.Drawing.Point(24, 45)
        Me.lblThreshold.Name = "lblThreshold"
        Me.lblThreshold.Size = New System.Drawing.Size(111, 13)
        Me.lblThreshold.TabIndex = 25
        Me.lblThreshold.Text = "Hourly intensity above"
        '
        'chkSeasons
        '
        Me.chkSeasons.AutoSize = True
        Me.chkSeasons.Location = New System.Drawing.Point(6, 18)
        Me.chkSeasons.Name = "chkSeasons"
        Me.chkSeasons.Size = New System.Drawing.Size(123, 17)
        Me.chkSeasons.TabIndex = 39
        Me.chkSeasons.Text = "Vary only in selected"
        Me.chkSeasons.UseVisualStyleBackColor = True
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Location = New System.Drawing.Point(324, 131)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsNone.TabIndex = 43
        Me.btnSeasonsNone.Text = "None"
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Location = New System.Drawing.Point(6, 131)
        Me.btnSeasonsAll.Name = "btnSeasonsAll"
        Me.btnSeasonsAll.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsAll.TabIndex = 42
        Me.btnSeasonsAll.Text = "All"
        '
        'grpSeasons
        '
        Me.grpSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSeasons.Controls.Add(Me.lstSeasons)
        Me.grpSeasons.Controls.Add(Me.cboSeasons)
        Me.grpSeasons.Controls.Add(Me.chkSeasons)
        Me.grpSeasons.Controls.Add(Me.btnSeasonsAll)
        Me.grpSeasons.Controls.Add(Me.btnSeasonsNone)
        Me.grpSeasons.Location = New System.Drawing.Point(12, 399)
        Me.grpSeasons.Name = "grpSeasons"
        Me.grpSeasons.Size = New System.Drawing.Size(393, 160)
        Me.grpSeasons.TabIndex = 38
        Me.grpSeasons.TabStop = False
        Me.grpSeasons.Text = "Seasons"
        '
        'lstSeasons
        '
        Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSeasons.FormattingEnabled = True
        Me.lstSeasons.IntegralHeight = False
        Me.lstSeasons.Location = New System.Drawing.Point(6, 41)
        Me.lstSeasons.MultiColumn = True
        Me.lstSeasons.Name = "lstSeasons"
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(381, 79)
        Me.lstSeasons.TabIndex = 41
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.FormattingEnabled = True
        Me.cboSeasons.Location = New System.Drawing.Point(135, 16)
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(252, 21)
        Me.cboSeasons.TabIndex = 40
        '
        'lblIncrement2
        '
        Me.lblIncrement2.AutoSize = True
        Me.lblIncrement2.BackColor = System.Drawing.Color.Transparent
        Me.lblIncrement2.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblIncrement2.Location = New System.Drawing.Point(152, 97)
        Me.lblIncrement2.Name = "lblIncrement2"
        Me.lblIncrement2.Size = New System.Drawing.Size(230, 13)
        Me.lblIncrement2.TabIndex = 19
        Me.lblIncrement2.Text = "Increase this much each iteration from Minimum"
        '
        'lblValueUnitsMaximum
        '
        Me.lblValueUnitsMaximum.AutoSize = True
        Me.lblValueUnitsMaximum.BackColor = System.Drawing.Color.Transparent
        Me.lblValueUnitsMaximum.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblValueUnitsMaximum.Location = New System.Drawing.Point(152, 71)
        Me.lblValueUnitsMaximum.Name = "lblValueUnitsMaximum"
        Me.lblValueUnitsMaximum.Size = New System.Drawing.Size(0, 13)
        Me.lblValueUnitsMaximum.TabIndex = 22
        '
        'grpMinMax
        '
        Me.grpMinMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpMinMax.Controls.Add(Me.radioSingle)
        Me.grpMinMax.Controls.Add(Me.radioIterate)
        Me.grpMinMax.Controls.Add(Me.lblValueUnitsMinimum)
        Me.grpMinMax.Controls.Add(Me.lblValueUnitsMaximum)
        Me.grpMinMax.Controls.Add(Me.lblMinimum)
        Me.grpMinMax.Controls.Add(Me.lblIncrement2)
        Me.grpMinMax.Controls.Add(Me.lblMaximum)
        Me.grpMinMax.Controls.Add(Me.lblIncrement)
        Me.grpMinMax.Controls.Add(Me.txtMax)
        Me.grpMinMax.Controls.Add(Me.txtIncrement)
        Me.grpMinMax.Controls.Add(Me.txtMin)
        Me.grpMinMax.Location = New System.Drawing.Point(12, 117)
        Me.grpMinMax.Name = "grpMinMax"
        Me.grpMinMax.Size = New System.Drawing.Size(393, 120)
        Me.grpMinMax.TabIndex = 10
        Me.grpMinMax.TabStop = False
        Me.grpMinMax.Text = "Multiply each value by a number"
        '
        'radioSingle
        '
        Me.radioSingle.AutoSize = True
        Me.radioSingle.Checked = True
        Me.radioSingle.Location = New System.Drawing.Point(9, 19)
        Me.radioSingle.Name = "radioSingle"
        Me.radioSingle.Size = New System.Drawing.Size(94, 17)
        Me.radioSingle.TabIndex = 11
        Me.radioSingle.TabStop = True
        Me.radioSingle.Text = "Single Change"
        Me.radioSingle.UseVisualStyleBackColor = True
        '
        'radioIterate
        '
        Me.radioIterate.AutoSize = True
        Me.radioIterate.Location = New System.Drawing.Point(109, 19)
        Me.radioIterate.Name = "radioIterate"
        Me.radioIterate.Size = New System.Drawing.Size(100, 17)
        Me.radioIterate.TabIndex = 12
        Me.radioIterate.Text = "Iterate Changes"
        Me.radioIterate.UseVisualStyleBackColor = True
        '
        'lblValueUnitsMinimum
        '
        Me.lblValueUnitsMinimum.AutoSize = True
        Me.lblValueUnitsMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblValueUnitsMinimum.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblValueUnitsMinimum.Location = New System.Drawing.Point(152, 45)
        Me.lblValueUnitsMinimum.Name = "lblValueUnitsMinimum"
        Me.lblValueUnitsMinimum.Size = New System.Drawing.Size(0, 13)
        Me.lblValueUnitsMinimum.TabIndex = 23
        '
        'txtVolumePercent
        '
        Me.txtVolumePercent.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVolumePercent.Location = New System.Drawing.Point(300, 17)
        Me.txtVolumePercent.Name = "txtVolumePercent"
        Me.txtVolumePercent.Size = New System.Drawing.Size(22, 20)
        Me.txtVolumePercent.TabIndex = 25
        Me.txtVolumePercent.Text = "10"
        Me.txtVolumePercent.Visible = False
        '
        'lblVolumePercent
        '
        Me.lblVolumePercent.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblVolumePercent.AutoSize = True
        Me.lblVolumePercent.BackColor = System.Drawing.Color.Transparent
        Me.lblVolumePercent.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblVolumePercent.Location = New System.Drawing.Point(250, 21)
        Me.lblVolumePercent.Name = "lblVolumePercent"
        Me.lblVolumePercent.Size = New System.Drawing.Size(44, 13)
        Me.lblVolumePercent.TabIndex = 24
        Me.lblVolumePercent.Text = "Change"
        Me.lblVolumePercent.Visible = False
        '
        'lblVolumePercent2
        '
        Me.lblVolumePercent2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblVolumePercent2.AutoSize = True
        Me.lblVolumePercent2.BackColor = System.Drawing.Color.Transparent
        Me.lblVolumePercent2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblVolumePercent2.Location = New System.Drawing.Point(325, 21)
        Me.lblVolumePercent2.Name = "lblVolumePercent2"
        Me.lblVolumePercent2.Size = New System.Drawing.Size(64, 13)
        Me.lblVolumePercent2.TabIndex = 26
        Me.lblVolumePercent2.Text = "% of volume"
        Me.lblVolumePercent2.Visible = False
        '
        'grpEvents
        '
        Me.grpEvents.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEvents.Controls.Add(Me.lblVolumePercent)
        Me.grpEvents.Controls.Add(Me.txtVolumePercent)
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
        Me.grpEvents.Controls.Add(Me.lblVolumePercent2)
        Me.grpEvents.Controls.Add(Me.txtEventVolume)
        Me.grpEvents.Controls.Add(Me.txtEventDuration)
        Me.grpEvents.Location = New System.Drawing.Point(12, 243)
        Me.grpEvents.Name = "grpEvents"
        Me.grpEvents.Size = New System.Drawing.Size(393, 150)
        Me.grpEvents.TabIndex = 23
        Me.grpEvents.TabStop = False
        Me.grpEvents.Text = "Events"
        '
        'lblVolumeUnits
        '
        Me.lblVolumeUnits.AutoSize = True
        Me.lblVolumeUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblVolumeUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblVolumeUnits.Location = New System.Drawing.Point(211, 97)
        Me.lblVolumeUnits.Name = "lblVolumeUnits"
        Me.lblVolumeUnits.Size = New System.Drawing.Size(38, 13)
        Me.lblVolumeUnits.TabIndex = 44
        Me.lblVolumeUnits.Text = "inches"
        '
        'lblThresholdUnits
        '
        Me.lblThresholdUnits.AutoSize = True
        Me.lblThresholdUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblThresholdUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblThresholdUnits.Location = New System.Drawing.Point(211, 45)
        Me.lblThresholdUnits.Name = "lblThresholdUnits"
        Me.lblThresholdUnits.Size = New System.Drawing.Size(29, 13)
        Me.lblThresholdUnits.TabIndex = 43
        Me.lblThresholdUnits.Text = "in/hr"
        '
        'lblDurationUnits
        '
        Me.lblDurationUnits.AutoSize = True
        Me.lblDurationUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblDurationUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblDurationUnits.Location = New System.Drawing.Point(211, 123)
        Me.lblDurationUnits.Name = "lblDurationUnits"
        Me.lblDurationUnits.Size = New System.Drawing.Size(33, 13)
        Me.lblDurationUnits.TabIndex = 42
        Me.lblDurationUnits.Text = "hours"
        '
        'lblGapUnits
        '
        Me.lblGapUnits.AutoSize = True
        Me.lblGapUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblGapUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblGapUnits.Location = New System.Drawing.Point(211, 71)
        Me.lblGapUnits.Name = "lblGapUnits"
        Me.lblGapUnits.Size = New System.Drawing.Size(33, 13)
        Me.lblGapUnits.TabIndex = 41
        Me.lblGapUnits.Text = "hours"
        '
        'lblDuration
        '
        Me.lblDuration.AutoSize = True
        Me.lblDuration.BackColor = System.Drawing.Color.Transparent
        Me.lblDuration.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblDuration.Location = New System.Drawing.Point(24, 123)
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
        Me.lblVolume.Location = New System.Drawing.Point(24, 97)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(101, 13)
        Me.lblVolume.TabIndex = 39
        Me.lblVolume.Text = "Total volume above"
        '
        'lblGap
        '
        Me.lblGap.AutoSize = True
        Me.lblGap.BackColor = System.Drawing.Color.Transparent
        Me.lblGap.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblGap.Location = New System.Drawing.Point(24, 72)
        Me.lblGap.Name = "lblGap"
        Me.lblGap.Size = New System.Drawing.Size(85, 13)
        Me.lblGap.TabIndex = 38
        Me.lblGap.Text = "Allow gaps up to"
        '
        'lblPET
        '
        Me.lblPET.AutoSize = True
        Me.lblPET.BackColor = System.Drawing.Color.Transparent
        Me.lblPET.Location = New System.Drawing.Point(12, 68)
        Me.lblPET.Name = "lblPET"
        Me.lblPET.Size = New System.Drawing.Size(76, 13)
        Me.lblPET.TabIndex = 5
        Me.lblPET.Text = "Compute PET:"
        '
        'frmVariation
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(417, 601)
        Me.Controls.Add(Me.grpEvents)
        Me.Controls.Add(Me.grpMinMax)
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
        Me.Controls.Add(Me.lblPET)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmVariation"
        Me.Text = "Modify Existing Data"
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

    Public Function AskUser(ByRef aVariation As atcVariation) As Boolean
        pVariation = aVariation.Clone

        If pVariation.DataSets Is Nothing Then pVariation.DataSets = New atcTimeseriesGroup

        cboFunction.Items.Clear()
        cboFunction.Items.AddRange(pFunctionLabels)

        pSeasonTypesAvailable = atcSeasonPlugin.AllSeasonTypes
        For Each lSeasonType As Type In pSeasonTypesAvailable
            Dim lSeasonTypeShortName As String = atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name)
            Select Case lSeasonTypeShortName
                Case "Calendar Year", "Water Year", "Month"
                    cboSeasons.Items.Add(lSeasonTypeShortName & "s")
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
        Dim lData As atcTimeseriesGroup = atcDataManager.UserSelectData("Select data to vary", pVariation.DataSets)
        If Not lData Is Nothing Then
            pVariation.DataSets = lData
            UpdateDataText(txtVaryData, lData)
        End If
    End Sub

    Private Sub UserSelectPET()
        Dim lData As atcTimeseriesGroup = atcDataManager.UserSelectData("Select PET to replace with values computed from air temperature", pVariation.PETdata)
        If Not lData Is Nothing Then
            pVariation.PETdata = lData
            UpdateDataText(txtVaryPET, lData)
        End If
    End Sub

    Private Sub UpdateDataText(ByVal aTextBox As Windows.Forms.TextBox, _
                               ByVal aGroup As atcTimeseriesGroup)
        If Not aGroup Is Nothing AndAlso aGroup.Count > 0 Then
            aTextBox.Text = aGroup.ItemByIndex(0).ToString
            If aGroup.Count > 1 Then aTextBox.Text &= " (and " & aGroup.Count - 1 & " more)"
        Else
            aTextBox.Text = aTextBox.Tag
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
            'If cboSeasons.Text <> AllSeasons Then
            Try
                pSeasons = SelectedSeasonType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
                SetAllSeasons()
                'For Each lSeasonIndex As Integer In pAllSeasons
                '    pSeasons.SeasonSelected(lSeasonIndex) = True
                'Next
                RefreshSeasonsList()
            Catch ex As Exception
                Logger.Dbg("Could not create new seasons for '" & cboSeasons.Text & "': " & ex.ToString)
                lstSeasons.Items.Clear()
            End Try
            'Else
            '    lstSeasons.Items.Clear()
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
                If pSeasons.SeasonSelected(lSeasonIndex) Then
                    lstSeasons.SetSelected(lstSeasons.Items.Count - 1, True)
                End If
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
        Dim lSeasonLookingFor As String = cboSeasons.Text
        'trim trailing "s"
        If lSeasonLookingFor.EndsWith("s") Then lSeasonLookingFor = lSeasonLookingFor.Substring(0, lSeasonLookingFor.Length - 1)
        For Each lSeasonType As Type In pSeasonTypesAvailable
            Dim lSeasonName As String = atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name)
            If lSeasonName.Equals(lSeasonLookingFor) OrElse lSeasonName.Equals(lSeasonLookingFor & "s") Then
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

    Private Function VariationFromForm(ByVal aVariation As atcVariation) As Boolean
        Try
            If txtName.Text.Trim.Length = 0 Then
                Logger.Msg("Name was not entered", "Name is required")
                Return False
            End If
            With aVariation
                .Name = txtName.Text.Trim
                If txtVaryData.Text.Equals(txtVaryData.Tag) Then
                    Logger.Msg("No data was selected", "Need Data To Vary")
                    Return False
                End If

                .Operation = pFunctionOperations(cboFunction.SelectedIndex)

                .UseEvents = chkEvents.Checked
                If .UseEvents Then

                    If IsNumeric(txtVolumePercent.Text) Then
                        .IntensifyVolumeFraction = 1 + (CDbl(txtVolumePercent.Text) / 100)
                    Else
                        .IntensifyVolumeFraction = pNaN
                    End If

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

                .Seasons = pSeasons
                If Not pSeasons Is Nothing Then
                    For Each lSeasonIndex As Integer In pAllSeasons
                        pSeasons.SeasonSelected(lSeasonIndex) = lstSeasons.SelectedItems.Contains(pSeasons.SeasonName(lSeasonIndex))
                    Next
                End If

                Try
                    .Min = CDbl(txtMin.Text)
                Catch
                    Logger.Msg("Minimum value must be a number", "Non-numeric value")
                    Return False
                End Try

                If radioIterate.Checked Then
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
        Dim lVariationTemplate As New atcVariation

        If VariationFromForm(lVariationTemplate) Then
            With lOpenDialog
                .Title = "Select Script"
                .Filter = "VB.Net *.vb|*.vb|C Sharp *.cs|*.cs|All Files|*.*"
                Try
                    .FilterIndex = CInt(GetSetting("BasinsCAT", "Variation", "ScriptExtIndex", 1))
                Catch
                    .FilterIndex = 1
                End Try
                .FileName = GetSetting("BasinsCAT", "Variation", "ScriptFilename", ReplaceString(Me.Text, " ", "_") & ".vb")
                If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    SaveSetting("BasinsCAT", "Variation", "ScriptExtIndex", .FilterIndex)
                    SaveSetting("BasinsCAT", "Variation", "ScriptFilename", .FileName)
                    Dim lErrors As String = ""
                    Try
                        Dim lAssembly As System.Reflection.Assembly = Scripting.PrepareScript(FileExt(.FileName), Nothing, .FileName, lErrors, "")
                        If lErrors.Length = 0 Then
                            pVariation = Scripting.Run(lAssembly, lErrors, lVariationTemplate)
                        End If
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
                Case "Add" : cboFunction.SelectedIndex = 0
                Case "Multiply" : cboFunction.SelectedIndex = 1
                Case "AddEvents" : cboFunction.SelectedIndex = 2
                Case "Intensify" : cboFunction.SelectedIndex = 3
            End Select

            If Double.IsNaN(.IntensifyVolumeFraction) Then
                txtVolumePercent.Text = ""
            Else
                txtVolumePercent.Text = DoubleToString((.IntensifyVolumeFraction - 1) * 100)
            End If

            EnableIterative(.Max > .Min)
            If Not Double.IsNaN(.Min) Then txtMin.Text = DoubleToString(.Min)
            If Not Double.IsNaN(.Max) Then txtMax.Text = DoubleToString(.Max)
            If Not Double.IsNaN(.Increment) Then txtIncrement.Text = DoubleToString(.Increment)

            EnableEvents(.UseEvents)

            If .UseEvents Then
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
                EnableSeasons(True)
                cboSeasons.Text = atcSeasonPlugin.SeasonClassNameToLabel(.Seasons.GetType.Name)
                pSeasons = .Seasons
                SetAllSeasons()
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
                atcDataManager.ShowDisplay("", pVariation.DataSets)
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
                atcDataManager.ShowDisplay("", pVariation.PETdata)
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

    Private Sub radioIterate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioIterate.CheckedChanged, radioSingle.CheckedChanged
        EnableIterative(radioIterate.Checked)
    End Sub

    Private Sub EnableIterative(ByVal aEnable As Boolean)
        If radioIterate.Checked <> aEnable Then radioIterate.Checked = aEnable
        If radioSingle.Checked = aEnable Then radioSingle.Checked = Not aEnable

        If aEnable Then
            lblMinimum.Text = "Minimum"
        Else
            lblMinimum.Text = "Value"
        End If
        lblMaximum.Visible = aEnable
        lblValueUnitsMaximum.Visible = aEnable
        txtMax.Visible = aEnable
        lblIncrement.Visible = aEnable
        lblIncrement2.Visible = aEnable
        txtIncrement.Visible = aEnable

    End Sub

    Private Sub EnableEvents(ByVal aEnable As Boolean)
        If chkEvents.Checked <> aEnable Then chkEvents.Checked = aEnable

        lblThreshold.Visible = aEnable
        txtEventThreshold.Visible = aEnable
        lblThresholdUnits.Visible = aEnable

        lblGap.Visible = aEnable
        txtEventGap.Visible = aEnable
        lblGapUnits.Visible = aEnable

        lblVolume.Visible = aEnable
        txtEventVolume.Visible = aEnable
        lblVolumeUnits.Visible = aEnable

        lblDuration.Visible = aEnable
        txtEventDuration.Visible = aEnable
        lblDurationUnits.Visible = aEnable

        SetVolumePercentVisible()
    End Sub

    Private Sub EnableSeasons(ByVal aEnable As Boolean)
        If chkSeasons.Checked <> aEnable Then chkSeasons.Checked = aEnable

        cboSeasons.Visible = aEnable
        lstSeasons.Visible = aEnable
        btnSeasonsAll.Visible = aEnable
        btnSeasonsNone.Visible = aEnable

        If aEnable Then
            cboSeasons_SelectedIndexChanged(Nothing, Nothing)
        Else
            pSeasons = Nothing
        End If
    End Sub

    Private Sub cboFunction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFunction.SelectedIndexChanged
        FunctionChanged()
    End Sub

    Private Sub FunctionChanged()
        grpMinMax.Text = pFunctionGroupLabels(cboFunction.SelectedIndex)
        lblValueUnitsMinimum.Text = pFunctionUnits(cboFunction.SelectedIndex)
        lblValueUnitsMaximum.Text = lblValueUnitsMinimum.Text
        Select Case pFunctionOperations(cboFunction.SelectedIndex)
            Case "Intensify", "AddEvents"
                If Not chkEvents.Checked Then chkEvents.Checked = True
        End Select
        SetVolumePercentVisible()
    End Sub

    Private Sub SetVolumePercentVisible()
        Dim lVisible As Boolean = chkEvents.Checked AndAlso _
                                  cboFunction.SelectedIndex > -1 AndAlso _
                                  pFunctionOperations(cboFunction.SelectedIndex).Equals("Intensify")
        lblVolumePercent.Visible = lVisible
        lblVolumePercent2.Visible = lVisible
        txtVolumePercent.Visible = lVisible
    End Sub
End Class
