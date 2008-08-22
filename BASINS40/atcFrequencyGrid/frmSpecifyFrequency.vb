Imports atcData
Imports atcTimeseriesNdayHighLow
Imports atcUtility
Imports MapWinUtility

Public Class frmSpecifyFrequency
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents panelTop As System.Windows.Forms.Panel
    Friend WithEvents panelBottom As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpRecurrence As System.Windows.Forms.GroupBox
    Friend WithEvents btnRecurrenceNone As System.Windows.Forms.Button
    Friend WithEvents btnRecurrenceAll As System.Windows.Forms.Button
    Friend WithEvents lstRecurrence As System.Windows.Forms.ListBox
    Friend WithEvents grpNday As System.Windows.Forms.GroupBox
    Friend WithEvents btnNdayNone As System.Windows.Forms.Button
    Friend WithEvents btnNdayAll As System.Windows.Forms.Button
    Friend WithEvents lstNday As System.Windows.Forms.ListBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtNdayAdd As System.Windows.Forms.TextBox
    Friend WithEvents btnNdayAdd As System.Windows.Forms.Button
    Friend WithEvents btnRecurrenceAdd As System.Windows.Forms.Button
    Friend WithEvents radioLow As System.Windows.Forms.RadioButton
    Friend WithEvents radioHigh As System.Windows.Forms.RadioButton
    Friend WithEvents chkLog As System.Windows.Forms.CheckBox
    Friend WithEvents btnNdayRemove As System.Windows.Forms.Button
    Friend WithEvents btnNdayDefault As System.Windows.Forms.Button
    Friend WithEvents btnRecurrenceDefault As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnRecurrenceRemove As System.Windows.Forms.Button
    Friend WithEvents grpYears As System.Windows.Forms.GroupBox
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblOmitBefore As System.Windows.Forms.Label
    Friend WithEvents lblOmitAfter As System.Windows.Forms.Label
    Friend WithEvents txtOmitAfterYear As System.Windows.Forms.TextBox
    Friend WithEvents txtOmitBeforeYear As System.Windows.Forms.TextBox
    Friend WithEvents grpDates As System.Windows.Forms.GroupBox
    Friend WithEvents cboStartMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearStart As System.Windows.Forms.Label
    Friend WithEvents txtEndDay As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDay As System.Windows.Forms.TextBox
    Friend WithEvents cboEndMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearEnd As System.Windows.Forms.Label
    Friend WithEvents cboYears As System.Windows.Forms.ComboBox
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents txtRecurrenceAdd As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecifyFrequency))
        Me.panelTop = New System.Windows.Forms.Panel
        Me.grpRecurrence = New System.Windows.Forms.GroupBox
        Me.btnRecurrenceDefault = New System.Windows.Forms.Button
        Me.btnRecurrenceRemove = New System.Windows.Forms.Button
        Me.lstRecurrence = New System.Windows.Forms.ListBox
        Me.btnRecurrenceAdd = New System.Windows.Forms.Button
        Me.txtRecurrenceAdd = New System.Windows.Forms.TextBox
        Me.btnRecurrenceNone = New System.Windows.Forms.Button
        Me.btnRecurrenceAll = New System.Windows.Forms.Button
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.grpNday = New System.Windows.Forms.GroupBox
        Me.btnNdayDefault = New System.Windows.Forms.Button
        Me.btnNdayRemove = New System.Windows.Forms.Button
        Me.btnNdayAdd = New System.Windows.Forms.Button
        Me.txtNdayAdd = New System.Windows.Forms.TextBox
        Me.btnNdayNone = New System.Windows.Forms.Button
        Me.btnNdayAll = New System.Windows.Forms.Button
        Me.lstNday = New System.Windows.Forms.ListBox
        Me.panelBottom = New System.Windows.Forms.Panel
        Me.chkLog = New System.Windows.Forms.CheckBox
        Me.radioLow = New System.Windows.Forms.RadioButton
        Me.radioHigh = New System.Windows.Forms.RadioButton
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtOmitAfterYear = New System.Windows.Forms.TextBox
        Me.txtOmitBeforeYear = New System.Windows.Forms.TextBox
        Me.grpYears = New System.Windows.Forms.GroupBox
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblOmitBefore = New System.Windows.Forms.Label
        Me.lblOmitAfter = New System.Windows.Forms.Label
        Me.cboYears = New System.Windows.Forms.ComboBox
        Me.grpDates = New System.Windows.Forms.GroupBox
        Me.cboStartMonth = New System.Windows.Forms.ComboBox
        Me.lblYearStart = New System.Windows.Forms.Label
        Me.txtEndDay = New System.Windows.Forms.TextBox
        Me.txtStartDay = New System.Windows.Forms.TextBox
        Me.cboEndMonth = New System.Windows.Forms.ComboBox
        Me.lblYearEnd = New System.Windows.Forms.Label
        Me.panelTop.SuspendLayout()
        Me.grpRecurrence.SuspendLayout()
        Me.grpNday.SuspendLayout()
        Me.panelBottom.SuspendLayout()
        Me.grpYears.SuspendLayout()
        Me.grpDates.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelTop
        '
        Me.panelTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelTop.Controls.Add(Me.grpRecurrence)
        Me.panelTop.Controls.Add(Me.Splitter1)
        Me.panelTop.Controls.Add(Me.grpNday)
        Me.panelTop.Location = New System.Drawing.Point(0, 0)
        Me.panelTop.Name = "panelTop"
        Me.panelTop.Size = New System.Drawing.Size(422, 497)
        Me.panelTop.TabIndex = 14
        '
        'grpRecurrence
        '
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceDefault)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceRemove)
        Me.grpRecurrence.Controls.Add(Me.lstRecurrence)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceAdd)
        Me.grpRecurrence.Controls.Add(Me.txtRecurrenceAdd)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceNone)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceAll)
        Me.grpRecurrence.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpRecurrence.Location = New System.Drawing.Point(208, 0)
        Me.grpRecurrence.Name = "grpRecurrence"
        Me.grpRecurrence.Size = New System.Drawing.Size(214, 497)
        Me.grpRecurrence.TabIndex = 7
        Me.grpRecurrence.TabStop = False
        Me.grpRecurrence.Text = "Recurrence Interval"
        '
        'btnRecurrenceDefault
        '
        Me.btnRecurrenceDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceDefault.Location = New System.Drawing.Point(152, 438)
        Me.btnRecurrenceDefault.Name = "btnRecurrenceDefault"
        Me.btnRecurrenceDefault.Size = New System.Drawing.Size(56, 20)
        Me.btnRecurrenceDefault.TabIndex = 14
        Me.btnRecurrenceDefault.Text = "Default"
        Me.ToolTip1.SetToolTip(Me.btnRecurrenceDefault, "Replace list above with default values")
        '
        'btnRecurrenceRemove
        '
        Me.btnRecurrenceRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceRemove.Location = New System.Drawing.Point(119, 438)
        Me.btnRecurrenceRemove.Name = "btnRecurrenceRemove"
        Me.btnRecurrenceRemove.Size = New System.Drawing.Size(27, 20)
        Me.btnRecurrenceRemove.TabIndex = 13
        Me.btnRecurrenceRemove.Text = "-"
        Me.ToolTip1.SetToolTip(Me.btnRecurrenceRemove, "Remove selected items from list above")
        '
        'lstRecurrence
        '
        Me.lstRecurrence.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstRecurrence.IntegralHeight = False
        Me.lstRecurrence.Location = New System.Drawing.Point(6, 19)
        Me.lstRecurrence.Name = "lstRecurrence"
        Me.lstRecurrence.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstRecurrence.Size = New System.Drawing.Size(202, 413)
        Me.lstRecurrence.TabIndex = 8
        Me.lstRecurrence.Tag = "Return Period"
        '
        'btnRecurrenceAdd
        '
        Me.btnRecurrenceAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceAdd.Location = New System.Drawing.Point(86, 438)
        Me.btnRecurrenceAdd.Name = "btnRecurrenceAdd"
        Me.btnRecurrenceAdd.Size = New System.Drawing.Size(27, 20)
        Me.btnRecurrenceAdd.TabIndex = 10
        Me.btnRecurrenceAdd.Text = "+"
        Me.ToolTip1.SetToolTip(Me.btnRecurrenceAdd, "Add Recurrence Interval in blank to list above")
        '
        'txtRecurrenceAdd
        '
        Me.txtRecurrenceAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRecurrenceAdd.Location = New System.Drawing.Point(6, 438)
        Me.txtRecurrenceAdd.Name = "txtRecurrenceAdd"
        Me.txtRecurrenceAdd.Size = New System.Drawing.Size(74, 20)
        Me.txtRecurrenceAdd.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.txtRecurrenceAdd, "Recurrence Interval to add to list")
        '
        'btnRecurrenceNone
        '
        Me.btnRecurrenceNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceNone.Location = New System.Drawing.Point(144, 467)
        Me.btnRecurrenceNone.Name = "btnRecurrenceNone"
        Me.btnRecurrenceNone.Size = New System.Drawing.Size(64, 24)
        Me.btnRecurrenceNone.TabIndex = 12
        Me.btnRecurrenceNone.Text = "None"
        Me.ToolTip1.SetToolTip(Me.btnRecurrenceNone, "Clear Selection")
        '
        'btnRecurrenceAll
        '
        Me.btnRecurrenceAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceAll.Location = New System.Drawing.Point(6, 467)
        Me.btnRecurrenceAll.Name = "btnRecurrenceAll"
        Me.btnRecurrenceAll.Size = New System.Drawing.Size(64, 24)
        Me.btnRecurrenceAll.TabIndex = 11
        Me.btnRecurrenceAll.Text = "All"
        Me.ToolTip1.SetToolTip(Me.btnRecurrenceAll, "Select All")
        '
        'Splitter1
        '
        Me.Splitter1.BackColor = System.Drawing.SystemColors.Control
        Me.Splitter1.Location = New System.Drawing.Point(200, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(8, 497)
        Me.Splitter1.TabIndex = 13
        Me.Splitter1.TabStop = False
        '
        'grpNday
        '
        Me.grpNday.Controls.Add(Me.btnNdayDefault)
        Me.grpNday.Controls.Add(Me.btnNdayRemove)
        Me.grpNday.Controls.Add(Me.btnNdayAdd)
        Me.grpNday.Controls.Add(Me.txtNdayAdd)
        Me.grpNday.Controls.Add(Me.btnNdayNone)
        Me.grpNday.Controls.Add(Me.btnNdayAll)
        Me.grpNday.Controls.Add(Me.lstNday)
        Me.grpNday.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpNday.Location = New System.Drawing.Point(0, 0)
        Me.grpNday.Name = "grpNday"
        Me.grpNday.Size = New System.Drawing.Size(200, 497)
        Me.grpNday.TabIndex = 1
        Me.grpNday.TabStop = False
        Me.grpNday.Text = "Number of Days"
        '
        'btnNdayDefault
        '
        Me.btnNdayDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayDefault.Location = New System.Drawing.Point(138, 438)
        Me.btnNdayDefault.Name = "btnNdayDefault"
        Me.btnNdayDefault.Size = New System.Drawing.Size(56, 20)
        Me.btnNdayDefault.TabIndex = 8
        Me.btnNdayDefault.Text = "Default"
        Me.ToolTip1.SetToolTip(Me.btnNdayDefault, "Replace list above with default values")
        '
        'btnNdayRemove
        '
        Me.btnNdayRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayRemove.Location = New System.Drawing.Point(105, 438)
        Me.btnNdayRemove.Name = "btnNdayRemove"
        Me.btnNdayRemove.Size = New System.Drawing.Size(27, 20)
        Me.btnNdayRemove.TabIndex = 7
        Me.btnNdayRemove.Text = "-"
        Me.ToolTip1.SetToolTip(Me.btnNdayRemove, "Remove selected items from list above")
        '
        'btnNdayAdd
        '
        Me.btnNdayAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayAdd.Location = New System.Drawing.Point(72, 438)
        Me.btnNdayAdd.Name = "btnNdayAdd"
        Me.btnNdayAdd.Size = New System.Drawing.Size(27, 20)
        Me.btnNdayAdd.TabIndex = 4
        Me.btnNdayAdd.Text = "+"
        Me.ToolTip1.SetToolTip(Me.btnNdayAdd, "Add Number of Days in blank to list above")
        '
        'txtNdayAdd
        '
        Me.txtNdayAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNdayAdd.Location = New System.Drawing.Point(6, 438)
        Me.txtNdayAdd.Name = "txtNdayAdd"
        Me.txtNdayAdd.Size = New System.Drawing.Size(54, 20)
        Me.txtNdayAdd.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.txtNdayAdd, "Number of Days to add to list")
        '
        'btnNdayNone
        '
        Me.btnNdayNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayNone.Location = New System.Drawing.Point(130, 467)
        Me.btnNdayNone.Name = "btnNdayNone"
        Me.btnNdayNone.Size = New System.Drawing.Size(64, 23)
        Me.btnNdayNone.TabIndex = 6
        Me.btnNdayNone.Text = "None"
        Me.ToolTip1.SetToolTip(Me.btnNdayNone, "Clear Selection")
        '
        'btnNdayAll
        '
        Me.btnNdayAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNdayAll.Location = New System.Drawing.Point(6, 467)
        Me.btnNdayAll.Name = "btnNdayAll"
        Me.btnNdayAll.Size = New System.Drawing.Size(64, 24)
        Me.btnNdayAll.TabIndex = 5
        Me.btnNdayAll.Text = "All"
        Me.ToolTip1.SetToolTip(Me.btnNdayAll, "Select All")
        '
        'lstNday
        '
        Me.lstNday.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstNday.IntegralHeight = False
        Me.lstNday.Location = New System.Drawing.Point(6, 19)
        Me.lstNday.Name = "lstNday"
        Me.lstNday.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstNday.Size = New System.Drawing.Size(188, 413)
        Me.lstNday.TabIndex = 2
        Me.lstNday.Tag = "NDay"
        '
        'panelBottom
        '
        Me.panelBottom.Controls.Add(Me.chkLog)
        Me.panelBottom.Controls.Add(Me.radioLow)
        Me.panelBottom.Controls.Add(Me.radioHigh)
        Me.panelBottom.Controls.Add(Me.btnOk)
        Me.panelBottom.Controls.Add(Me.btnCancel)
        Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBottom.Location = New System.Drawing.Point(0, 582)
        Me.panelBottom.Name = "panelBottom"
        Me.panelBottom.Size = New System.Drawing.Size(421, 41)
        Me.panelBottom.TabIndex = 15
        '
        'chkLog
        '
        Me.chkLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkLog.AutoSize = True
        Me.chkLog.Checked = True
        Me.chkLog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLog.Location = New System.Drawing.Point(112, 14)
        Me.chkLog.Name = "chkLog"
        Me.chkLog.Size = New System.Drawing.Size(80, 17)
        Me.chkLog.TabIndex = 20
        Me.chkLog.Text = "Logarithmic"
        Me.chkLog.UseVisualStyleBackColor = True
        '
        'radioLow
        '
        Me.radioLow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioLow.AutoSize = True
        Me.radioLow.Location = New System.Drawing.Point(9, 21)
        Me.radioLow.Name = "radioLow"
        Me.radioLow.Size = New System.Drawing.Size(45, 17)
        Me.radioLow.TabIndex = 18
        Me.radioLow.Text = "Low"
        Me.radioLow.UseVisualStyleBackColor = True
        '
        'radioHigh
        '
        Me.radioHigh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioHigh.AutoSize = True
        Me.radioHigh.Checked = True
        Me.radioHigh.Location = New System.Drawing.Point(9, 3)
        Me.radioHigh.Name = "radioHigh"
        Me.radioHigh.Size = New System.Drawing.Size(47, 17)
        Me.radioHigh.TabIndex = 17
        Me.radioHigh.TabStop = True
        Me.radioHigh.Text = "High"
        Me.radioHigh.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(269, 9)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(77, 24)
        Me.btnOk.TabIndex = 13
        Me.btnOk.Text = "Compute"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(352, 9)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "Cancel"
        '
        'txtOmitAfterYear
        '
        Me.txtOmitAfterYear.Location = New System.Drawing.Point(41, 45)
        Me.txtOmitAfterYear.Name = "txtOmitAfterYear"
        Me.txtOmitAfterYear.Size = New System.Drawing.Size(37, 20)
        Me.txtOmitAfterYear.TabIndex = 6
        Me.txtOmitAfterYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.txtOmitAfterYear, "Leave blank to use all years")
        Me.txtOmitAfterYear.Visible = False
        '
        'txtOmitBeforeYear
        '
        Me.txtOmitBeforeYear.Location = New System.Drawing.Point(41, 19)
        Me.txtOmitBeforeYear.Name = "txtOmitBeforeYear"
        Me.txtOmitBeforeYear.Size = New System.Drawing.Size(37, 20)
        Me.txtOmitBeforeYear.TabIndex = 5
        Me.txtOmitBeforeYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.txtOmitBeforeYear, "Leave blank to use all years")
        Me.txtOmitBeforeYear.Visible = False
        '
        'grpYears
        '
        Me.grpYears.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpYears.Controls.Add(Me.lblDataStart)
        Me.grpYears.Controls.Add(Me.lblDataEnd)
        Me.grpYears.Controls.Add(Me.lblOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitAfter)
        Me.grpYears.Controls.Add(Me.txtOmitAfterYear)
        Me.grpYears.Controls.Add(Me.txtOmitBeforeYear)
        Me.grpYears.Controls.Add(Me.cboYears)
        Me.grpYears.Location = New System.Drawing.Point(208, 503)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Size = New System.Drawing.Size(213, 73)
        Me.grpYears.TabIndex = 45
        Me.grpYears.TabStop = False
        Me.grpYears.Text = "Years to Include in Analysis"
        '
        'lblDataStart
        '
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(84, 22)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(121, 13)
        Me.lblDataStart.TabIndex = 45
        Me.lblDataStart.Tag = "Data Starts"
        Me.lblDataStart.Text = "Data Starts 11/22/1934"
        Me.lblDataStart.Visible = False
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(84, 48)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(118, 13)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Tag = "Data Ends"
        Me.lblDataEnd.Text = "Data Ends 11/22/1934"
        Me.lblDataEnd.Visible = False
        '
        'lblOmitBefore
        '
        Me.lblOmitBefore.AutoSize = True
        Me.lblOmitBefore.Location = New System.Drawing.Point(6, 22)
        Me.lblOmitBefore.Name = "lblOmitBefore"
        Me.lblOmitBefore.Size = New System.Drawing.Size(29, 13)
        Me.lblOmitBefore.TabIndex = 40
        Me.lblOmitBefore.Text = "Start"
        Me.lblOmitBefore.Visible = False
        '
        'lblOmitAfter
        '
        Me.lblOmitAfter.AutoSize = True
        Me.lblOmitAfter.Location = New System.Drawing.Point(6, 48)
        Me.lblOmitAfter.Name = "lblOmitAfter"
        Me.lblOmitAfter.Size = New System.Drawing.Size(26, 13)
        Me.lblOmitAfter.TabIndex = 43
        Me.lblOmitAfter.Text = "End"
        Me.lblOmitAfter.Visible = False
        '
        'cboYears
        '
        Me.cboYears.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboYears.FormattingEnabled = True
        Me.cboYears.Location = New System.Drawing.Point(6, 18)
        Me.cboYears.Name = "cboYears"
        Me.cboYears.Size = New System.Drawing.Size(202, 21)
        Me.cboYears.TabIndex = 44
        '
        'grpDates
        '
        Me.grpDates.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpDates.Controls.Add(Me.cboStartMonth)
        Me.grpDates.Controls.Add(Me.lblYearStart)
        Me.grpDates.Controls.Add(Me.txtEndDay)
        Me.grpDates.Controls.Add(Me.txtStartDay)
        Me.grpDates.Controls.Add(Me.cboEndMonth)
        Me.grpDates.Controls.Add(Me.lblYearEnd)
        Me.grpDates.Location = New System.Drawing.Point(0, 503)
        Me.grpDates.Name = "grpDates"
        Me.grpDates.Size = New System.Drawing.Size(200, 73)
        Me.grpDates.TabIndex = 65
        Me.grpDates.TabStop = False
        Me.grpDates.Text = "Year / Season Boundaries"
        '
        'cboStartMonth
        '
        Me.cboStartMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboStartMonth.Location = New System.Drawing.Point(41, 19)
        Me.cboStartMonth.MaxDropDownItems = 12
        Me.cboStartMonth.Name = "cboStartMonth"
        Me.cboStartMonth.Size = New System.Drawing.Size(88, 21)
        Me.cboStartMonth.TabIndex = 3
        Me.cboStartMonth.Text = "January"
        '
        'lblYearStart
        '
        Me.lblYearStart.AutoSize = True
        Me.lblYearStart.Location = New System.Drawing.Point(6, 22)
        Me.lblYearStart.Name = "lblYearStart"
        Me.lblYearStart.Size = New System.Drawing.Size(29, 13)
        Me.lblYearStart.TabIndex = 23
        Me.lblYearStart.Text = "Start"
        '
        'txtEndDay
        '
        Me.txtEndDay.Location = New System.Drawing.Point(135, 46)
        Me.txtEndDay.Name = "txtEndDay"
        Me.txtEndDay.Size = New System.Drawing.Size(24, 20)
        Me.txtEndDay.TabIndex = 63
        Me.txtEndDay.Text = "31"
        Me.txtEndDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtStartDay
        '
        Me.txtStartDay.Location = New System.Drawing.Point(135, 19)
        Me.txtStartDay.Name = "txtStartDay"
        Me.txtStartDay.Size = New System.Drawing.Size(24, 20)
        Me.txtStartDay.TabIndex = 4
        Me.txtStartDay.Text = "1"
        Me.txtStartDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cboEndMonth
        '
        Me.cboEndMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboEndMonth.Location = New System.Drawing.Point(41, 46)
        Me.cboEndMonth.MaxDropDownItems = 12
        Me.cboEndMonth.Name = "cboEndMonth"
        Me.cboEndMonth.Size = New System.Drawing.Size(88, 21)
        Me.cboEndMonth.TabIndex = 11
        Me.cboEndMonth.Text = "December"
        '
        'lblYearEnd
        '
        Me.lblYearEnd.AutoSize = True
        Me.lblYearEnd.Location = New System.Drawing.Point(6, 49)
        Me.lblYearEnd.Name = "lblYearEnd"
        Me.lblYearEnd.Size = New System.Drawing.Size(26, 13)
        Me.lblYearEnd.TabIndex = 61
        Me.lblYearEnd.Text = "End"
        '
        'frmSpecifyFrequency
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(421, 623)
        Me.Controls.Add(Me.grpDates)
        Me.Controls.Add(Me.grpYears)
        Me.Controls.Add(Me.panelTop)
        Me.Controls.Add(Me.panelBottom)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmSpecifyFrequency"
        Me.Text = "Select Numbers of Days and Recurrence Intervals"
        Me.panelTop.ResumeLayout(False)
        Me.grpRecurrence.ResumeLayout(False)
        Me.grpRecurrence.PerformLayout()
        Me.grpNday.ResumeLayout(False)
        Me.grpNday.PerformLayout()
        Me.panelBottom.ResumeLayout(False)
        Me.panelBottom.PerformLayout()
        Me.grpYears.ResumeLayout(False)
        Me.grpYears.PerformLayout()
        Me.grpDates.ResumeLayout(False)
        Me.grpDates.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Event Chose(ByVal aHigh As Boolean)
    Public Event Cancelled()

    Private WithEvents pDataGroup As atcDataGroup
    Private pDateFormat As atcDateFormat
    Private pOk As Boolean = False
    Private pYearStartMonth As Integer = 0
    Private pYearStartDay As Integer = 0
    Private pYearEndMonth As Integer = 0
    Private pYearEndDay As Integer = 0
    Private pFirstYear As Integer = 0
    Private pLastYear As Integer = 0

    Private pCommonStart As Double = GetMinValue()
    Private pCommonEnd As Double = GetMaxValue()

    Private Const pNoDatesInCommon As String = ": No dates in common"

    Public Function AskUser(ByVal aGroup As atcDataGroup, ByRef aChoseHigh As Boolean) As Boolean
        pDataGroup = aGroup
        Clear()
        Me.ShowDialog()
        If pOk Then aChoseHigh = radioHigh.Checked
        pDataGroup = Nothing
        Return pOk
    End Function

    Public Sub AskUser(ByVal aGroup As atcDataGroup)
        pDataGroup = aGroup
        Clear()
        Me.Show()
    End Sub

    Private Sub Clear()
        pDateFormat = New atcDateFormat
        With pDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        If GetSetting("atcFrequencyGrid", "Defaults", "HighOrLow", "High") = "High" Then
            radioHigh.Checked = True
        Else
            radioLow.Checked = True
        End If

        chkLog.Checked = (GetSetting("atcFrequencyGrid", "Defaults", "Logarithmic", "True") = "True")

        pOk = False

        LoadListSettingsOrDefaults(lstNday)
        LoadListSettingsOrDefaults(lstRecurrence)

        SeasonsYearsToForm()

        Dim lFirstDate As Double = GetMaxValue()
        Dim lLastDate As Double = GetMinValue()

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()

        Dim lAllText As String = "All"
        Dim lCommonText As String = "Common"

        For Each lDataset As atcData.atcTimeseries In pDataGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(1)
                If lThisDate < lFirstDate Then lFirstDate = lThisDate
                If lThisDate > pCommonStart Then pCommonStart = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > lLastDate Then lLastDate = lThisDate
                If lThisDate < pCommonEnd Then pCommonEnd = lThisDate
            End If
        Next
        If lFirstDate < GetMaxValue() AndAlso lLastDate > GetMinValue() Then
            lblDataStart.Text = lblDataStart.Tag & " " & pDateFormat.JDateToString(lFirstDate)
            lblDataEnd.Text = lblDataEnd.Tag & " " & pDateFormat.JDateToString(lLastDate)
            lAllText &= ": " & pDateFormat.JDateToString(lFirstDate) & " to " & pDateFormat.JDateToString(lLastDate)
        End If

        If pCommonStart > GetMinValue() AndAlso pCommonEnd < GetMaxValue() AndAlso pCommonStart < pCommonEnd Then
            lCommonText &= ": " & pDateFormat.JDateToString(pCommonStart) & " to " & pDateFormat.JDateToString(pCommonEnd)
        Else
            lCommonText &= pNoDatesInCommon
        End If

        With cboYears.Items
            .Clear()
            .Add(lAllText)
            .Add(lCommonText)
            .Add("Custom")
        End With
        cboYears.SelectedIndex = 0

    End Sub

    Private Sub LoadListSettingsOrDefaults(ByVal lst As Windows.Forms.ListBox)
        Dim lArgName As String = lst.Tag
        Dim lAvailableArray As String(,) = GetAllSettings("atcFrequencyGrid", "List." & lArgName)
        Dim lSelected As New ArrayList
        lst.Items.Clear()

        If Not lAvailableArray Is Nothing AndAlso lAvailableArray.Length > 0 Then
            Try
                For lIndex As Integer = 0 To lAvailableArray.GetUpperBound(0)
                    lst.Items.Add(lAvailableArray(lIndex, 0))
                    If lAvailableArray(lIndex, 1) = "True" Then
                        lst.SetSelected(lst.Items.Count - 1, True)
                    End If
                Next
            Catch e As Exception
                MapWinUtility.Logger.Dbg("Error retrieving saved settings: " & e.Message)
            End Try
        Else
            LoadListDefaults(lst)
        End If
    End Sub

    Private Sub LoadListDefaults(ByVal lst As Windows.Forms.ListBox)
        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        Dim lNDayHi As atcDefinedValue = lCalculator.AvailableOperations.GetDefinedValue("n-day high value")
        Dim lArgs As atcDataAttributes = lNDayHi.Arguments
        Dim lArgName As String = lst.Tag
        Dim lDefault As Object = lArgs.GetDefinedValue(lArgName).Definition.DefaultValue
        If Not lDefault Is Nothing AndAlso IsArray(lDefault) Then
            lst.Items.Clear()
            For Each lNumber As Double In lDefault
                Dim lLabel As String = Format(lNumber, "0.####")
                lst.Items.Add(lLabel)
            Next
        End If
    End Sub

    Private Sub SaveList(ByVal lst As Windows.Forms.ListBox)
        SaveSetting("atcFrequencyGrid", "List." & lst.Tag, "dummy", "")
        DeleteSetting("atcFrequencyGrid", "List." & lst.Tag)
        For lIndex As Integer = 0 To lst.Items.Count - 1
            SaveSetting("atcFrequencyGrid", "List." & lst.Tag, lst.Items(lIndex), lst.SelectedIndices.Contains(lIndex))
        Next
    End Sub

    Private Sub ClearAttributes()
        Dim lRemoveThese As New atcCollection
        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing Then
                    If lAttribute.Arguments.ContainsAttribute("Nday") OrElse _
                       lAttribute.Arguments.ContainsAttribute("Return Period") Then
                        lRemoveThese.Add(lAttribute)
                    End If
                End If
            Next
            For Each lAttribute As atcDefinedValue In lRemoveThese
                lData.Attributes.Remove(lAttribute)
            Next
        Next
    End Sub

    Private Sub SeasonsYearsToForm()
        If cboStartMonth.Items.Count > 0 Then
            cboStartMonth.SelectedIndex = pYearStartMonth - 1
            cboEndMonth.SelectedIndex = pYearEndMonth - 1
            If pYearStartDay > 0 Then txtStartDay.Text = pYearStartDay Else txtStartDay.Text = ""
            If pYearEndDay > 0 Then txtEndDay.Text = pYearEndDay Else txtEndDay.Text = ""
            If pFirstYear > 0 Then txtOmitBeforeYear.Text = pFirstYear Else txtOmitBeforeYear.Text = ""
            If pLastYear > 0 Then txtOmitAfterYear.Text = pLastYear Else txtOmitAfterYear.Text = ""
            If pFirstYear > 0 OrElse pLastYear > 0 Then
                ShowCustomYears(True)
            End If
        End If
    End Sub

    Private Sub SeasonsYearsFromForm()
        pYearStartMonth = cboStartMonth.SelectedIndex + 1
        pYearEndMonth = cboEndMonth.SelectedIndex + 1
        If IsNumeric(txtStartDay.Text) Then
            pYearStartDay = txtStartDay.Text
        Else
            pYearStartDay = 0
        End If
        If IsNumeric(txtEndDay.Text) Then
            pYearEndDay = txtEndDay.Text
        Else
            pYearEndDay = 0
        End If
        If IsNumeric(txtOmitBeforeYear.Text) Then
            pFirstYear = CInt(txtOmitBeforeYear.Text)
        Else
            pFirstYear = 0
        End If
        If IsNumeric(txtOmitAfterYear.Text) Then
            pLastYear = CInt(txtOmitAfterYear.Text)
        Else
            pLastYear = 0
        End If

    End Sub

    Private Sub ShowCustomYears(ByVal aShowCustom As Boolean)
        cboYears.Visible = Not aShowCustom
        txtOmitBeforeYear.Visible = aShowCustom
        txtOmitAfterYear.Visible = aShowCustom
        lblDataStart.Visible = aShowCustom
        lblDataEnd.Visible = aShowCustom
        lblOmitBefore.Visible = aShowCustom
        lblOmitAfter.Visible = aShowCustom
    End Sub

    Private Sub Calculate(ByVal aOperationName As String)
        ClearAttributes()
        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        Dim lArgs As New atcDataAttributes
        lArgs.SetValue("Timeseries", pDataGroup)
        lArgs.SetValue("NDay", ListToArray(lstNday))
        lArgs.SetValue("Return Period", ListToArray(lstRecurrence))
        lArgs.SetValue("LogFlg", chkLog.Checked)
        If pYearStartMonth > 0 Then lArgs.SetValue("BoundaryMonth", pYearStartMonth)
        If pYearStartDay > 0 Then lArgs.SetValue("BoundaryDay", pYearStartDay)
        If pYearEndMonth > 0 Then lArgs.SetValue("EndMonth", pYearEndMonth)
        If pYearEndDay > 0 Then lArgs.SetValue("EndDay", pYearEndDay)
        If pFirstYear > 0 Then lArgs.SetValue("FirstYear", pFirstYear)
        If pLastYear > 0 Then lArgs.SetValue("LastYear", pLastYear)

        lCalculator.Open(aOperationName, lArgs)
        lCalculator.DataSets.Clear()
        Dim lName As String = HighOrLowString()
        SaveSetting("atcFrequencyGrid", "StartMonth", lName, pYearStartMonth)
        SaveSetting("atcFrequencyGrid", "StartDay", lName, pYearStartDay)
        SaveSetting("atcFrequencyGrid", "EndMonth", lName, pYearEndMonth)
        SaveSetting("atcFrequencyGrid", "EndDay", lName, pYearEndDay)
        SaveSetting("atcFrequencyGrid", "Defaults", "HighOrLow", lName)
        SaveSetting("atcFrequencyGrid", "Defaults", "Logarithmic", chkLog.Checked.ToString)
        SaveList(lstNday)
        SaveList(lstRecurrence)
    End Sub

    'Return all selected items, or if none are selected then all items
    Private Function ListToArray(ByVal aList As System.Windows.Forms.ListBox) As Double()
        Dim lArray() As Double
        Dim lCollection As New ArrayList
        If aList.SelectedItems.Count > 0 Then
            For lIndex As Integer = 0 To aList.SelectedItems.Count - 1
                If IsNumeric(aList.SelectedItems(lIndex)) Then
                    lCollection.Add(CDbl(aList.SelectedItems(lIndex)))
                End If
            Next
        Else
            For lIndex As Integer = 0 To aList.Items.Count - 1
                If IsNumeric(aList.Items(lIndex)) Then
                    lCollection.Add(CDbl(aList.Items(lIndex)))
                End If
            Next
        End If
        ReDim lArray(lCollection.Count - 1)
        For lIndex As Integer = 0 To lCollection.Count - 1
            lArray(lIndex) = lCollection.Item(lIndex)
        Next
        Return lArray
    End Function

    Private Sub btnNdayAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayAdd.Click
        If IsNumeric(txtNdayAdd.Text) Then
            Try
                Dim lIndex As Integer = 0
                Dim lNewValue As Double = CDbl(txtNdayAdd.Text)
                While lIndex < lstNday.Items.Count AndAlso CDbl(lstNday.Items(lIndex)) < lNewValue
                    lIndex += 1
                End While
                lstNday.Items.Insert(lIndex, txtNdayAdd.Text)
                lstNday.SetSelected(lIndex, True)
            Catch ex As Exception
                Logger.Dbg("Exception adding N-day '" & txtNdayAdd.Text & "': " & ex.Message)
            End Try
        Else
            Logger.Msg("Type a number of days to add in the blank, then press the add button again")
        End If
    End Sub

    Private Sub btnRecurrenceAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceAdd.Click
        If IsNumeric(txtRecurrenceAdd.Text) Then
            Try
                Dim lIndex As Integer = 0
                Dim lNewValue As Double = CDbl(txtRecurrenceAdd.Text)
                While lIndex < lstRecurrence.Items.Count AndAlso CDbl(lstRecurrence.Items(lIndex)) < lNewValue
                    lIndex += 1
                End While
                lstRecurrence.Items.Insert(lIndex, txtRecurrenceAdd.Text)
                lstRecurrence.SetSelected(lIndex, True)
            Catch ex As Exception
                Logger.Dbg("Exception adding Recurrence '" & txtRecurrenceAdd.Text & "': " & ex.Message)
            End Try
        Else
            Logger.Msg("Type a return period to add in the blank, then press the add button again")
        End If
    End Sub

    Private Sub btnNdayRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayRemove.Click
        Dim lRemoveThese As New ArrayList
        Dim lIndex As Integer
        If lstNday.SelectedIndices.Count > 0 Then
            For lIndex = lstNday.SelectedIndices.Count - 1 To 0 Step -1
                lRemoveThese.Add(lstNday.SelectedIndices.Item(lIndex))
            Next
        Else
            For lIndex = lstNday.Items.Count - 1 To 0 Step -1
                If lstNday.Items(lIndex) = txtNdayAdd.Text Then
                    lRemoveThese.Add(lIndex)
                End If
            Next
        End If

        For Each lIndex In lRemoveThese
            lstNday.Items.RemoveAt(lIndex)
        Next
    End Sub

    Private Sub btnRecurrenceRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecurrenceRemove.Click
        Dim lRemoveThese As New ArrayList
        Dim lIndex As Integer
        If lstRecurrence.SelectedIndices.Count > 0 Then
            For lIndex = lstRecurrence.SelectedIndices.Count - 1 To 0 Step -1
                lRemoveThese.Add(lstRecurrence.SelectedIndices.Item(lIndex))
            Next
        Else
            For lIndex = lstRecurrence.Items.Count - 1 To 0 Step -1
                If lstRecurrence.Items(lIndex) = txtRecurrenceAdd.Text Then
                    lRemoveThese.Add(lIndex)
                End If
            Next
        End If

        For Each lIndex In lRemoveThese
            lstRecurrence.Items.RemoveAt(lIndex)
        Next
    End Sub

    Private Sub btnNdayDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayDefault.Click
        LoadListDefaults(lstNday)
    End Sub

    Private Sub btnRecurrenceDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceDefault.Click
        LoadListDefaults(lstRecurrence)
    End Sub

    Private Sub btnNdayAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayAll.Click
        For index As Integer = 0 To lstNday.Items.Count - 1
            lstNday.SetSelected(index, True)
        Next
    End Sub

    Private Sub btnNdayNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayNone.Click
        For index As Integer = 0 To lstNday.Items.Count - 1
            lstNday.SetSelected(index, False)
        Next
    End Sub

    Private Sub btnRecurrenceAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceAll.Click
        For index As Integer = 0 To lstRecurrence.Items.Count - 1
            lstRecurrence.SetSelected(index, True)
        Next
    End Sub

    Private Sub btnRecurrenceNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceNone.Click
        For index As Integer = 0 To lstRecurrence.Items.Count - 1
            lstRecurrence.SetSelected(index, False)
        Next
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        SeasonsYearsFromForm()
        Calculate("n-day " & HighOrLowString() & " value")
        Me.Cursor = System.Windows.Forms.Cursors.Default
        RaiseEvent Chose(radioHigh.Checked)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        RaiseEvent Cancelled()
        Close()
    End Sub

    Private Sub frmSpecifyFrequency_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Analysis\Time Series Functions\Frequency Grid.html")
        End If
    End Sub

    'Private Sub btnSelectYearsSeasons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectYearsSeasons.Click
    '    Dim lForm As New frmSpecifyYearsSeasons
    '    If lForm.AskUser("FrequencyGrid" & Me.Text, pDataGroup, pYearStartMonth, pYearStartDay, pYearEndMonth, pYearEndDay, pFirstYear, pLastYear) Then
    '        'lSeasons = New atcSeasonsYearSubset(aStartMonth, aStartDay, aEndMonth, aEndDay)
    '        Dim lName As String = HighOrLowString()
    '        SaveSetting("atcFrequencyGrid", "StartMonth", lName, pYearStartMonth)
    '        SaveSetting("atcFrequencyGrid", "StartDay", lName, pYearStartDay)
    '        SaveSetting("atcFrequencyGrid", "EndMonth", lName, pYearEndMonth)
    '        SaveSetting("atcFrequencyGrid", "EndDay", lName, pYearEndDay)
    '        RefreshSeasonsYears()
    '    End If
    'End Sub

    Private Sub radioHigh_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioHigh.CheckedChanged
        GetDefaultYearStartEnd()
    End Sub

    Private Sub radioLow_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioLow.CheckedChanged
        GetDefaultYearStartEnd()
    End Sub

    Private Sub GetDefaultYearStartEnd()
        If radioHigh.Checked Then
            pYearStartMonth = 10
            pYearStartDay = 1
            pYearEndMonth = 9
            pYearEndDay = 30
        Else
            pYearStartMonth = 4
            pYearStartDay = 1
            pYearEndMonth = 3
            pYearEndDay = 31
        End If

        Dim lName As String = HighOrLowString()
        pYearStartMonth = GetSetting("atcFrequencyGrid", "StartMonth", lName, pYearStartMonth)
        pYearStartDay = GetSetting("atcFrequencyGrid", "StartDay", lName, pYearStartDay)
        pYearEndMonth = GetSetting("atcFrequencyGrid", "EndMonth", lName, pYearEndMonth)
        pYearEndDay = GetSetting("atcFrequencyGrid", "EndDay", lName, pYearEndDay)
        SeasonsYearsToForm()
    End Sub

    Private Function HighOrLowString() As String
        If radioHigh.Checked Then
            Return "High"
        Else
            Return "Low"
        End If
    End Function

    Private Sub Splitter1_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles Splitter1.SplitterMoved
        grpDates.Width = grpNday.Width
        grpYears.Left = grpRecurrence.Left
        grpYears.Width = grpRecurrence.Width
    End Sub

    Private Sub cboYears_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboYears.SelectedIndexChanged
        Select Case cboYears.SelectedIndex
            Case 0 'All
                txtOmitBeforeYear.Text = ""
                txtOmitAfterYear.Text = ""
            Case 1 'Common
                If cboYears.Text.EndsWith(pNoDatesInCommon) Then
                    cboYears.SelectedIndex = 0
                Else
                    Dim lCurDate(5) As Integer
                    J2Date(pCommonStart, lCurDate)
                    txtOmitBeforeYear.Text = Format(lCurDate(0), "0000")
                    J2Date(pCommonEnd, lCurDate)
                    txtOmitAfterYear.Text = Format(lCurDate(0), "0000")
                End If
            Case 2 'Custom
                ShowCustomYears(True)
        End Select
    End Sub
End Class
