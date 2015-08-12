<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
        Me.grpDateFormat = New System.Windows.Forms.GroupBox()
        Me.radioOrderJulian = New System.Windows.Forms.RadioButton()
        Me.radioOrderDMY = New System.Windows.Forms.RadioButton()
        Me.radioOrderMDY = New System.Windows.Forms.RadioButton()
        Me.radioOrderYMD = New System.Windows.Forms.RadioButton()
        Me.chkMidnight24 = New System.Windows.Forms.CheckBox()
        Me.chkMonthNames = New System.Windows.Forms.CheckBox()
        Me.chk2digitYears = New System.Windows.Forms.CheckBox()
        Me.chkSeconds = New System.Windows.Forms.CheckBox()
        Me.chkMinutes = New System.Windows.Forms.CheckBox()
        Me.chkHours = New System.Windows.Forms.CheckBox()
        Me.chkDays = New System.Windows.Forms.CheckBox()
        Me.chkMonths = New System.Windows.Forms.CheckBox()
        Me.chkYears = New System.Windows.Forms.CheckBox()
        Me.lblTimeSeparator = New System.Windows.Forms.Label()
        Me.txtTimeSeparator = New System.Windows.Forms.TextBox()
        Me.lblDateSeparator = New System.Windows.Forms.Label()
        Me.txtDateSeparator = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grpValueFormat = New System.Windows.Forms.GroupBox()
        Me.lblFormat = New System.Windows.Forms.Label()
        Me.txtExpFormat = New System.Windows.Forms.TextBox()
        Me.lblExpFormat = New System.Windows.Forms.Label()
        Me.lblCantFit = New System.Windows.Forms.Label()
        Me.txtCantFit = New System.Windows.Forms.TextBox()
        Me.txtMaxWidth = New System.Windows.Forms.TextBox()
        Me.lblMaxWidth = New System.Windows.Forms.Label()
        Me.txtSignificantDigits = New System.Windows.Forms.TextBox()
        Me.txtFormat = New System.Windows.Forms.TextBox()
        Me.lblSigDig = New System.Windows.Forms.Label()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.grpDateFormat.SuspendLayout()
        Me.grpValueFormat.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDateFormat
        '
        Me.grpDateFormat.Controls.Add(Me.radioOrderJulian)
        Me.grpDateFormat.Controls.Add(Me.radioOrderDMY)
        Me.grpDateFormat.Controls.Add(Me.radioOrderMDY)
        Me.grpDateFormat.Controls.Add(Me.radioOrderYMD)
        Me.grpDateFormat.Controls.Add(Me.chkMidnight24)
        Me.grpDateFormat.Controls.Add(Me.chkMonthNames)
        Me.grpDateFormat.Controls.Add(Me.chk2digitYears)
        Me.grpDateFormat.Controls.Add(Me.chkSeconds)
        Me.grpDateFormat.Controls.Add(Me.chkMinutes)
        Me.grpDateFormat.Controls.Add(Me.chkHours)
        Me.grpDateFormat.Controls.Add(Me.chkDays)
        Me.grpDateFormat.Controls.Add(Me.chkMonths)
        Me.grpDateFormat.Controls.Add(Me.chkYears)
        Me.grpDateFormat.Controls.Add(Me.lblTimeSeparator)
        Me.grpDateFormat.Controls.Add(Me.txtTimeSeparator)
        Me.grpDateFormat.Controls.Add(Me.lblDateSeparator)
        Me.grpDateFormat.Controls.Add(Me.txtDateSeparator)
        Me.grpDateFormat.Location = New System.Drawing.Point(9, 11)
        Me.grpDateFormat.Margin = New System.Windows.Forms.Padding(2)
        Me.grpDateFormat.Name = "grpDateFormat"
        Me.grpDateFormat.Padding = New System.Windows.Forms.Padding(2)
        Me.grpDateFormat.Size = New System.Drawing.Size(216, 318)
        Me.grpDateFormat.TabIndex = 0
        Me.grpDateFormat.TabStop = False
        Me.grpDateFormat.Text = "Date Format"
        '
        'radioOrderJulian
        '
        Me.radioOrderJulian.AutoSize = True
        Me.radioOrderJulian.Location = New System.Drawing.Point(21, 87)
        Me.radioOrderJulian.Name = "radioOrderJulian"
        Me.radioOrderJulian.Size = New System.Drawing.Size(113, 17)
        Me.radioOrderJulian.TabIndex = 4
        Me.radioOrderJulian.TabStop = True
        Me.radioOrderJulian.Text = "Days starting 1900"
        Me.radioOrderJulian.UseVisualStyleBackColor = True
        '
        'radioOrderDMY
        '
        Me.radioOrderDMY.AutoSize = True
        Me.radioOrderDMY.Location = New System.Drawing.Point(21, 64)
        Me.radioOrderDMY.Name = "radioOrderDMY"
        Me.radioOrderDMY.Size = New System.Drawing.Size(102, 17)
        Me.radioOrderDMY.TabIndex = 3
        Me.radioOrderDMY.TabStop = True
        Me.radioOrderDMY.Text = "Day Month Year"
        Me.radioOrderDMY.UseVisualStyleBackColor = True
        '
        'radioOrderMDY
        '
        Me.radioOrderMDY.AutoSize = True
        Me.radioOrderMDY.Location = New System.Drawing.Point(21, 41)
        Me.radioOrderMDY.Name = "radioOrderMDY"
        Me.radioOrderMDY.Size = New System.Drawing.Size(102, 17)
        Me.radioOrderMDY.TabIndex = 2
        Me.radioOrderMDY.TabStop = True
        Me.radioOrderMDY.Text = "Month Day Year"
        Me.radioOrderMDY.UseVisualStyleBackColor = True
        '
        'radioOrderYMD
        '
        Me.radioOrderYMD.AutoSize = True
        Me.radioOrderYMD.Location = New System.Drawing.Point(21, 18)
        Me.radioOrderYMD.Name = "radioOrderYMD"
        Me.radioOrderYMD.Size = New System.Drawing.Size(102, 17)
        Me.radioOrderYMD.TabIndex = 1
        Me.radioOrderYMD.TabStop = True
        Me.radioOrderYMD.Text = "Year Month Day"
        Me.radioOrderYMD.UseVisualStyleBackColor = True
        '
        'chkMidnight24
        '
        Me.chkMidnight24.AutoSize = True
        Me.chkMidnight24.Location = New System.Drawing.Point(93, 179)
        Me.chkMidnight24.Name = "chkMidnight24"
        Me.chkMidnight24.Size = New System.Drawing.Size(110, 17)
        Me.chkMidnight24.TabIndex = 13
        Me.chkMidnight24.Text = "Midnight as 24:00"
        Me.chkMidnight24.UseVisualStyleBackColor = True
        '
        'chkMonthNames
        '
        Me.chkMonthNames.AutoSize = True
        Me.chkMonthNames.Location = New System.Drawing.Point(93, 133)
        Me.chkMonthNames.Name = "chkMonthNames"
        Me.chkMonthNames.Size = New System.Drawing.Size(92, 17)
        Me.chkMonthNames.TabIndex = 12
        Me.chkMonthNames.Text = "Month Names"
        Me.chkMonthNames.UseVisualStyleBackColor = True
        '
        'chk2digitYears
        '
        Me.chk2digitYears.AutoSize = True
        Me.chk2digitYears.Location = New System.Drawing.Point(93, 110)
        Me.chk2digitYears.Name = "chk2digitYears"
        Me.chk2digitYears.Size = New System.Drawing.Size(101, 17)
        Me.chk2digitYears.TabIndex = 11
        Me.chk2digitYears.Text = "Two Digit Years"
        Me.chk2digitYears.UseVisualStyleBackColor = True
        '
        'chkSeconds
        '
        Me.chkSeconds.AutoSize = True
        Me.chkSeconds.Location = New System.Drawing.Point(21, 225)
        Me.chkSeconds.Name = "chkSeconds"
        Me.chkSeconds.Size = New System.Drawing.Size(68, 17)
        Me.chkSeconds.TabIndex = 10
        Me.chkSeconds.Text = "Seconds"
        Me.chkSeconds.UseVisualStyleBackColor = True
        '
        'chkMinutes
        '
        Me.chkMinutes.AutoSize = True
        Me.chkMinutes.Location = New System.Drawing.Point(21, 202)
        Me.chkMinutes.Name = "chkMinutes"
        Me.chkMinutes.Size = New System.Drawing.Size(63, 17)
        Me.chkMinutes.TabIndex = 9
        Me.chkMinutes.Text = "Minutes"
        Me.chkMinutes.UseVisualStyleBackColor = True
        '
        'chkHours
        '
        Me.chkHours.AutoSize = True
        Me.chkHours.Location = New System.Drawing.Point(21, 179)
        Me.chkHours.Name = "chkHours"
        Me.chkHours.Size = New System.Drawing.Size(54, 17)
        Me.chkHours.TabIndex = 8
        Me.chkHours.Text = "Hours"
        Me.chkHours.UseVisualStyleBackColor = True
        '
        'chkDays
        '
        Me.chkDays.AutoSize = True
        Me.chkDays.Location = New System.Drawing.Point(21, 156)
        Me.chkDays.Name = "chkDays"
        Me.chkDays.Size = New System.Drawing.Size(50, 17)
        Me.chkDays.TabIndex = 7
        Me.chkDays.Text = "Days"
        Me.chkDays.UseVisualStyleBackColor = True
        '
        'chkMonths
        '
        Me.chkMonths.AutoSize = True
        Me.chkMonths.Location = New System.Drawing.Point(21, 133)
        Me.chkMonths.Name = "chkMonths"
        Me.chkMonths.Size = New System.Drawing.Size(61, 17)
        Me.chkMonths.TabIndex = 6
        Me.chkMonths.Text = "Months"
        Me.chkMonths.UseVisualStyleBackColor = True
        '
        'chkYears
        '
        Me.chkYears.AutoSize = True
        Me.chkYears.Location = New System.Drawing.Point(21, 110)
        Me.chkYears.Name = "chkYears"
        Me.chkYears.Size = New System.Drawing.Size(53, 17)
        Me.chkYears.TabIndex = 5
        Me.chkYears.Text = "Years"
        Me.chkYears.UseVisualStyleBackColor = True
        '
        'lblTimeSeparator
        '
        Me.lblTimeSeparator.AutoSize = True
        Me.lblTimeSeparator.Location = New System.Drawing.Point(18, 287)
        Me.lblTimeSeparator.Name = "lblTimeSeparator"
        Me.lblTimeSeparator.Size = New System.Drawing.Size(79, 13)
        Me.lblTimeSeparator.TabIndex = 16
        Me.lblTimeSeparator.Text = "Time Separator"
        '
        'txtTimeSeparator
        '
        Me.txtTimeSeparator.Location = New System.Drawing.Point(108, 284)
        Me.txtTimeSeparator.Name = "txtTimeSeparator"
        Me.txtTimeSeparator.Size = New System.Drawing.Size(15, 20)
        Me.txtTimeSeparator.TabIndex = 17
        Me.txtTimeSeparator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblDateSeparator
        '
        Me.lblDateSeparator.AutoSize = True
        Me.lblDateSeparator.Location = New System.Drawing.Point(18, 261)
        Me.lblDateSeparator.Name = "lblDateSeparator"
        Me.lblDateSeparator.Size = New System.Drawing.Size(79, 13)
        Me.lblDateSeparator.TabIndex = 14
        Me.lblDateSeparator.Text = "Date Separator"
        '
        'txtDateSeparator
        '
        Me.txtDateSeparator.Location = New System.Drawing.Point(108, 258)
        Me.txtDateSeparator.Name = "txtDateSeparator"
        Me.txtDateSeparator.Size = New System.Drawing.Size(15, 20)
        Me.txtDateSeparator.TabIndex = 15
        Me.txtDateSeparator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(90, 341)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 30
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpValueFormat
        '
        Me.grpValueFormat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpValueFormat.Controls.Add(Me.lblFormat)
        Me.grpValueFormat.Controls.Add(Me.txtExpFormat)
        Me.grpValueFormat.Controls.Add(Me.lblExpFormat)
        Me.grpValueFormat.Controls.Add(Me.lblCantFit)
        Me.grpValueFormat.Controls.Add(Me.txtCantFit)
        Me.grpValueFormat.Controls.Add(Me.txtMaxWidth)
        Me.grpValueFormat.Controls.Add(Me.lblMaxWidth)
        Me.grpValueFormat.Controls.Add(Me.txtSignificantDigits)
        Me.grpValueFormat.Controls.Add(Me.txtFormat)
        Me.grpValueFormat.Controls.Add(Me.lblSigDig)
        Me.grpValueFormat.Location = New System.Drawing.Point(229, 11)
        Me.grpValueFormat.Margin = New System.Windows.Forms.Padding(2)
        Me.grpValueFormat.Name = "grpValueFormat"
        Me.grpValueFormat.Padding = New System.Windows.Forms.Padding(2)
        Me.grpValueFormat.Size = New System.Drawing.Size(214, 158)
        Me.grpValueFormat.TabIndex = 18
        Me.grpValueFormat.TabStop = False
        Me.grpValueFormat.Text = "Value Format"
        '
        'lblFormat
        '
        Me.lblFormat.AutoSize = True
        Me.lblFormat.Location = New System.Drawing.Point(4, 21)
        Me.lblFormat.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblFormat.Name = "lblFormat"
        Me.lblFormat.Size = New System.Drawing.Size(85, 13)
        Me.lblFormat.TabIndex = 19
        Me.lblFormat.Text = "Standard Format"
        Me.lblFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtExpFormat
        '
        Me.txtExpFormat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExpFormat.Location = New System.Drawing.Point(96, 44)
        Me.txtExpFormat.Name = "txtExpFormat"
        Me.txtExpFormat.Size = New System.Drawing.Size(113, 20)
        Me.txtExpFormat.TabIndex = 22
        '
        'lblExpFormat
        '
        Me.lblExpFormat.AutoSize = True
        Me.lblExpFormat.Location = New System.Drawing.Point(4, 47)
        Me.lblExpFormat.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblExpFormat.Name = "lblExpFormat"
        Me.lblExpFormat.Size = New System.Drawing.Size(87, 13)
        Me.lblExpFormat.TabIndex = 21
        Me.lblExpFormat.Text = "Exponent Format"
        Me.lblExpFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCantFit
        '
        Me.lblCantFit.AutoSize = True
        Me.lblCantFit.Location = New System.Drawing.Point(4, 125)
        Me.lblCantFit.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblCantFit.Name = "lblCantFit"
        Me.lblCantFit.Size = New System.Drawing.Size(94, 13)
        Me.lblCantFit.TabIndex = 27
        Me.lblCantFit.Text = "If Value Cannot Fit"
        Me.lblCantFit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCantFit
        '
        Me.txtCantFit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCantFit.Location = New System.Drawing.Point(150, 122)
        Me.txtCantFit.Name = "txtCantFit"
        Me.txtCantFit.Size = New System.Drawing.Size(59, 20)
        Me.txtCantFit.TabIndex = 28
        '
        'txtMaxWidth
        '
        Me.txtMaxWidth.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMaxWidth.Location = New System.Drawing.Point(150, 96)
        Me.txtMaxWidth.Name = "txtMaxWidth"
        Me.txtMaxWidth.Size = New System.Drawing.Size(59, 20)
        Me.txtMaxWidth.TabIndex = 26
        '
        'lblMaxWidth
        '
        Me.lblMaxWidth.AutoSize = True
        Me.lblMaxWidth.Location = New System.Drawing.Point(4, 99)
        Me.lblMaxWidth.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblMaxWidth.Name = "lblMaxWidth"
        Me.lblMaxWidth.Size = New System.Drawing.Size(87, 13)
        Me.lblMaxWidth.TabIndex = 25
        Me.lblMaxWidth.Text = "Maximum Length"
        Me.lblMaxWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSignificantDigits
        '
        Me.txtSignificantDigits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSignificantDigits.Location = New System.Drawing.Point(150, 70)
        Me.txtSignificantDigits.Name = "txtSignificantDigits"
        Me.txtSignificantDigits.Size = New System.Drawing.Size(59, 20)
        Me.txtSignificantDigits.TabIndex = 24
        '
        'txtFormat
        '
        Me.txtFormat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFormat.Location = New System.Drawing.Point(96, 18)
        Me.txtFormat.Name = "txtFormat"
        Me.txtFormat.Size = New System.Drawing.Size(113, 20)
        Me.txtFormat.TabIndex = 20
        '
        'lblSigDig
        '
        Me.lblSigDig.AutoSize = True
        Me.lblSigDig.Location = New System.Drawing.Point(4, 73)
        Me.lblSigDig.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSigDig.Name = "lblSigDig"
        Me.lblSigDig.Size = New System.Drawing.Size(85, 13)
        Me.lblSigDig.TabIndex = 23
        Me.lblSigDig.Text = "Significant Digits"
        Me.lblSigDig.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnApply.Location = New System.Drawing.Point(169, 341)
        Me.btnApply.Margin = New System.Windows.Forms.Padding(2)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 23)
        Me.btnApply.TabIndex = 31
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(248, 341)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(2)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 32
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnHelp.Location = New System.Drawing.Point(11, 341)
        Me.btnHelp.Margin = New System.Windows.Forms.Padding(2)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 23)
        Me.btnHelp.TabIndex = 29
        Me.btnHelp.Text = "Help"
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(230, 179)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(27, 13)
        Me.lblTitle.TabIndex = 33
        Me.lblTitle.Text = "Title"
        '
        'txtTitle
        '
        Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTitle.Location = New System.Drawing.Point(236, 195)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(204, 20)
        Me.txtTitle.TabIndex = 34
        '
        'frmOptions
        '
        Me.AcceptButton = Me.btnApply
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(452, 375)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.grpValueFormat)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grpDateFormat)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(227, 250)
        Me.Name = "frmOptions"
        Me.Text = "Date and Value Formats"
        Me.grpDateFormat.ResumeLayout(False)
        Me.grpDateFormat.PerformLayout()
        Me.grpValueFormat.ResumeLayout(False)
        Me.grpValueFormat.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpDateFormat As System.Windows.Forms.GroupBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpValueFormat As System.Windows.Forms.GroupBox
    Friend WithEvents lblSigDig As System.Windows.Forms.Label
    Friend WithEvents lblTimeSeparator As System.Windows.Forms.Label
    Friend WithEvents txtTimeSeparator As System.Windows.Forms.TextBox
    Friend WithEvents lblDateSeparator As System.Windows.Forms.Label
    Friend WithEvents txtDateSeparator As System.Windows.Forms.TextBox
    Friend WithEvents chkMonthNames As System.Windows.Forms.CheckBox
    Friend WithEvents chk2digitYears As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeconds As System.Windows.Forms.CheckBox
    Friend WithEvents chkMinutes As System.Windows.Forms.CheckBox
    Friend WithEvents chkHours As System.Windows.Forms.CheckBox
    Friend WithEvents chkDays As System.Windows.Forms.CheckBox
    Friend WithEvents chkMonths As System.Windows.Forms.CheckBox
    Friend WithEvents chkYears As System.Windows.Forms.CheckBox
    Friend WithEvents radioOrderYMD As System.Windows.Forms.RadioButton
    Friend WithEvents chkMidnight24 As System.Windows.Forms.CheckBox
    Friend WithEvents radioOrderJulian As System.Windows.Forms.RadioButton
    Friend WithEvents radioOrderDMY As System.Windows.Forms.RadioButton
    Friend WithEvents radioOrderMDY As System.Windows.Forms.RadioButton
    Friend WithEvents txtMaxWidth As System.Windows.Forms.TextBox
    Friend WithEvents lblMaxWidth As System.Windows.Forms.Label
    Friend WithEvents txtSignificantDigits As System.Windows.Forms.TextBox
    Friend WithEvents txtFormat As System.Windows.Forms.TextBox
    Friend WithEvents lblFormat As System.Windows.Forms.Label
    Friend WithEvents txtExpFormat As System.Windows.Forms.TextBox
    Friend WithEvents lblExpFormat As System.Windows.Forms.Label
    Friend WithEvents lblCantFit As System.Windows.Forms.Label
    Friend WithEvents txtCantFit As System.Windows.Forms.TextBox
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
End Class
