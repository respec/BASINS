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
        Me.grpboxDateFormat = New System.Windows.Forms.GroupBox
        Me.radioOrderJulian = New System.Windows.Forms.RadioButton
        Me.radioOrderDMY = New System.Windows.Forms.RadioButton
        Me.radioOrderMDY = New System.Windows.Forms.RadioButton
        Me.radioOrderYMD = New System.Windows.Forms.RadioButton
        Me.chkMidnight24 = New System.Windows.Forms.CheckBox
        Me.chkMonthNames = New System.Windows.Forms.CheckBox
        Me.chk2digitYears = New System.Windows.Forms.CheckBox
        Me.chkSeconds = New System.Windows.Forms.CheckBox
        Me.chkMinutes = New System.Windows.Forms.CheckBox
        Me.chkHours = New System.Windows.Forms.CheckBox
        Me.chkDays = New System.Windows.Forms.CheckBox
        Me.chkMonths = New System.Windows.Forms.CheckBox
        Me.chkYears = New System.Windows.Forms.CheckBox
        Me.lblTimeSeparator = New System.Windows.Forms.Label
        Me.txtTimeSeparator = New System.Windows.Forms.TextBox
        Me.lblDateSeparator = New System.Windows.Forms.Label
        Me.txtDateSeparator = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grpboxNumberFormat = New System.Windows.Forms.GroupBox
        Me.lblFormat = New System.Windows.Forms.Label
        Me.txtExpFormat = New System.Windows.Forms.TextBox
        Me.lblExpFormat = New System.Windows.Forms.Label
        Me.lblCantFit = New System.Windows.Forms.Label
        Me.txtCantFit = New System.Windows.Forms.TextBox
        Me.txtMaxWidth = New System.Windows.Forms.TextBox
        Me.lblMaxWidth = New System.Windows.Forms.Label
        Me.txtSignificantDigits = New System.Windows.Forms.TextBox
        Me.txtFormat = New System.Windows.Forms.TextBox
        Me.lblSigDig = New System.Windows.Forms.Label
        Me.btnApply = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.grpboxDateFormat.SuspendLayout()
        Me.grpboxNumberFormat.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpboxDateFormat
        '
        Me.grpboxDateFormat.Controls.Add(Me.radioOrderJulian)
        Me.grpboxDateFormat.Controls.Add(Me.radioOrderDMY)
        Me.grpboxDateFormat.Controls.Add(Me.radioOrderMDY)
        Me.grpboxDateFormat.Controls.Add(Me.radioOrderYMD)
        Me.grpboxDateFormat.Controls.Add(Me.chkMidnight24)
        Me.grpboxDateFormat.Controls.Add(Me.chkMonthNames)
        Me.grpboxDateFormat.Controls.Add(Me.chk2digitYears)
        Me.grpboxDateFormat.Controls.Add(Me.chkSeconds)
        Me.grpboxDateFormat.Controls.Add(Me.chkMinutes)
        Me.grpboxDateFormat.Controls.Add(Me.chkHours)
        Me.grpboxDateFormat.Controls.Add(Me.chkDays)
        Me.grpboxDateFormat.Controls.Add(Me.chkMonths)
        Me.grpboxDateFormat.Controls.Add(Me.chkYears)
        Me.grpboxDateFormat.Controls.Add(Me.lblTimeSeparator)
        Me.grpboxDateFormat.Controls.Add(Me.txtTimeSeparator)
        Me.grpboxDateFormat.Controls.Add(Me.lblDateSeparator)
        Me.grpboxDateFormat.Controls.Add(Me.txtDateSeparator)
        Me.grpboxDateFormat.Location = New System.Drawing.Point(9, 11)
        Me.grpboxDateFormat.Margin = New System.Windows.Forms.Padding(2)
        Me.grpboxDateFormat.Name = "grpboxDateFormat"
        Me.grpboxDateFormat.Padding = New System.Windows.Forms.Padding(2)
        Me.grpboxDateFormat.Size = New System.Drawing.Size(216, 331)
        Me.grpboxDateFormat.TabIndex = 1
        Me.grpboxDateFormat.TabStop = False
        Me.grpboxDateFormat.Text = "Date Format"
        '
        'radioOrderJulian
        '
        Me.radioOrderJulian.AutoSize = True
        Me.radioOrderJulian.Location = New System.Drawing.Point(21, 87)
        Me.radioOrderJulian.Name = "radioOrderJulian"
        Me.radioOrderJulian.Size = New System.Drawing.Size(113, 17)
        Me.radioOrderJulian.TabIndex = 16
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
        Me.radioOrderDMY.TabIndex = 15
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
        Me.radioOrderMDY.TabIndex = 14
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
        Me.radioOrderYMD.TabIndex = 13
        Me.radioOrderYMD.TabStop = True
        Me.radioOrderYMD.Text = "Year Month Day"
        Me.radioOrderYMD.UseVisualStyleBackColor = True
        '
        'chkMidnight24
        '
        Me.chkMidnight24.AutoSize = True
        Me.chkMidnight24.Location = New System.Drawing.Point(93, 188)
        Me.chkMidnight24.Name = "chkMidnight24"
        Me.chkMidnight24.Size = New System.Drawing.Size(110, 17)
        Me.chkMidnight24.TabIndex = 12
        Me.chkMidnight24.Text = "Midnight as 24:00"
        Me.chkMidnight24.UseVisualStyleBackColor = True
        '
        'chkMonthNames
        '
        Me.chkMonthNames.AutoSize = True
        Me.chkMonthNames.Location = New System.Drawing.Point(93, 142)
        Me.chkMonthNames.Name = "chkMonthNames"
        Me.chkMonthNames.Size = New System.Drawing.Size(92, 17)
        Me.chkMonthNames.TabIndex = 11
        Me.chkMonthNames.Text = "Month Names"
        Me.chkMonthNames.UseVisualStyleBackColor = True
        '
        'chk2digitYears
        '
        Me.chk2digitYears.AutoSize = True
        Me.chk2digitYears.Location = New System.Drawing.Point(93, 119)
        Me.chk2digitYears.Name = "chk2digitYears"
        Me.chk2digitYears.Size = New System.Drawing.Size(101, 17)
        Me.chk2digitYears.TabIndex = 10
        Me.chk2digitYears.Text = "Two Digit Years"
        Me.chk2digitYears.UseVisualStyleBackColor = True
        '
        'chkSeconds
        '
        Me.chkSeconds.AutoSize = True
        Me.chkSeconds.Location = New System.Drawing.Point(21, 234)
        Me.chkSeconds.Name = "chkSeconds"
        Me.chkSeconds.Size = New System.Drawing.Size(68, 17)
        Me.chkSeconds.TabIndex = 9
        Me.chkSeconds.Text = "Seconds"
        Me.chkSeconds.UseVisualStyleBackColor = True
        '
        'chkMinutes
        '
        Me.chkMinutes.AutoSize = True
        Me.chkMinutes.Location = New System.Drawing.Point(21, 211)
        Me.chkMinutes.Name = "chkMinutes"
        Me.chkMinutes.Size = New System.Drawing.Size(63, 17)
        Me.chkMinutes.TabIndex = 8
        Me.chkMinutes.Text = "Minutes"
        Me.chkMinutes.UseVisualStyleBackColor = True
        '
        'chkHours
        '
        Me.chkHours.AutoSize = True
        Me.chkHours.Location = New System.Drawing.Point(21, 188)
        Me.chkHours.Name = "chkHours"
        Me.chkHours.Size = New System.Drawing.Size(54, 17)
        Me.chkHours.TabIndex = 7
        Me.chkHours.Text = "Hours"
        Me.chkHours.UseVisualStyleBackColor = True
        '
        'chkDays
        '
        Me.chkDays.AutoSize = True
        Me.chkDays.Location = New System.Drawing.Point(21, 165)
        Me.chkDays.Name = "chkDays"
        Me.chkDays.Size = New System.Drawing.Size(50, 17)
        Me.chkDays.TabIndex = 6
        Me.chkDays.Text = "Days"
        Me.chkDays.UseVisualStyleBackColor = True
        '
        'chkMonths
        '
        Me.chkMonths.AutoSize = True
        Me.chkMonths.Location = New System.Drawing.Point(21, 142)
        Me.chkMonths.Name = "chkMonths"
        Me.chkMonths.Size = New System.Drawing.Size(61, 17)
        Me.chkMonths.TabIndex = 5
        Me.chkMonths.Text = "Months"
        Me.chkMonths.UseVisualStyleBackColor = True
        '
        'chkYears
        '
        Me.chkYears.AutoSize = True
        Me.chkYears.Location = New System.Drawing.Point(21, 119)
        Me.chkYears.Name = "chkYears"
        Me.chkYears.Size = New System.Drawing.Size(53, 17)
        Me.chkYears.TabIndex = 4
        Me.chkYears.Text = "Years"
        Me.chkYears.UseVisualStyleBackColor = True
        '
        'lblTimeSeparator
        '
        Me.lblTimeSeparator.AutoSize = True
        Me.lblTimeSeparator.Location = New System.Drawing.Point(18, 296)
        Me.lblTimeSeparator.Name = "lblTimeSeparator"
        Me.lblTimeSeparator.Size = New System.Drawing.Size(79, 13)
        Me.lblTimeSeparator.TabIndex = 3
        Me.lblTimeSeparator.Text = "Time Separator"
        '
        'txtTimeSeparator
        '
        Me.txtTimeSeparator.Location = New System.Drawing.Point(108, 293)
        Me.txtTimeSeparator.Name = "txtTimeSeparator"
        Me.txtTimeSeparator.Size = New System.Drawing.Size(15, 20)
        Me.txtTimeSeparator.TabIndex = 2
        Me.txtTimeSeparator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblDateSeparator
        '
        Me.lblDateSeparator.AutoSize = True
        Me.lblDateSeparator.Location = New System.Drawing.Point(18, 270)
        Me.lblDateSeparator.Name = "lblDateSeparator"
        Me.lblDateSeparator.Size = New System.Drawing.Size(79, 13)
        Me.lblDateSeparator.TabIndex = 1
        Me.lblDateSeparator.Text = "Date Separator"
        '
        'txtDateSeparator
        '
        Me.txtDateSeparator.Location = New System.Drawing.Point(108, 267)
        Me.txtDateSeparator.Name = "txtDateSeparator"
        Me.txtDateSeparator.Size = New System.Drawing.Size(15, 20)
        Me.txtDateSeparator.TabIndex = 0
        Me.txtDateSeparator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(229, 355)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpboxNumberFormat
        '
        Me.grpboxNumberFormat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpboxNumberFormat.Controls.Add(Me.lblFormat)
        Me.grpboxNumberFormat.Controls.Add(Me.txtExpFormat)
        Me.grpboxNumberFormat.Controls.Add(Me.lblExpFormat)
        Me.grpboxNumberFormat.Controls.Add(Me.lblCantFit)
        Me.grpboxNumberFormat.Controls.Add(Me.txtCantFit)
        Me.grpboxNumberFormat.Controls.Add(Me.txtMaxWidth)
        Me.grpboxNumberFormat.Controls.Add(Me.lblMaxWidth)
        Me.grpboxNumberFormat.Controls.Add(Me.txtSignificantDigits)
        Me.grpboxNumberFormat.Controls.Add(Me.txtFormat)
        Me.grpboxNumberFormat.Controls.Add(Me.lblSigDig)
        Me.grpboxNumberFormat.Location = New System.Drawing.Point(229, 11)
        Me.grpboxNumberFormat.Margin = New System.Windows.Forms.Padding(2)
        Me.grpboxNumberFormat.Name = "grpboxNumberFormat"
        Me.grpboxNumberFormat.Padding = New System.Windows.Forms.Padding(2)
        Me.grpboxNumberFormat.Size = New System.Drawing.Size(214, 158)
        Me.grpboxNumberFormat.TabIndex = 4
        Me.grpboxNumberFormat.TabStop = False
        Me.grpboxNumberFormat.Text = "Number Format"
        '
        'lblFormat
        '
        Me.lblFormat.AutoSize = True
        Me.lblFormat.Location = New System.Drawing.Point(4, 21)
        Me.lblFormat.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblFormat.Name = "lblFormat"
        Me.lblFormat.Size = New System.Drawing.Size(85, 13)
        Me.lblFormat.TabIndex = 11
        Me.lblFormat.Text = "Standard Format"
        Me.lblFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtExpFormat
        '
        Me.txtExpFormat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExpFormat.Location = New System.Drawing.Point(150, 44)
        Me.txtExpFormat.Name = "txtExpFormat"
        Me.txtExpFormat.Size = New System.Drawing.Size(59, 20)
        Me.txtExpFormat.TabIndex = 10
        '
        'lblExpFormat
        '
        Me.lblExpFormat.AutoSize = True
        Me.lblExpFormat.Location = New System.Drawing.Point(4, 47)
        Me.lblExpFormat.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblExpFormat.Name = "lblExpFormat"
        Me.lblExpFormat.Size = New System.Drawing.Size(87, 13)
        Me.lblExpFormat.TabIndex = 9
        Me.lblExpFormat.Text = "Exponent Format"
        Me.lblExpFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCantFit
        '
        Me.lblCantFit.AutoSize = True
        Me.lblCantFit.Location = New System.Drawing.Point(4, 125)
        Me.lblCantFit.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblCantFit.Name = "lblCantFit"
        Me.lblCantFit.Size = New System.Drawing.Size(104, 13)
        Me.lblCantFit.TabIndex = 8
        Me.lblCantFit.Text = "If Number Cannot Fit"
        Me.lblCantFit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCantFit
        '
        Me.txtCantFit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCantFit.Location = New System.Drawing.Point(150, 122)
        Me.txtCantFit.Name = "txtCantFit"
        Me.txtCantFit.Size = New System.Drawing.Size(59, 20)
        Me.txtCantFit.TabIndex = 7
        '
        'txtMaxWidth
        '
        Me.txtMaxWidth.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMaxWidth.Location = New System.Drawing.Point(150, 96)
        Me.txtMaxWidth.Name = "txtMaxWidth"
        Me.txtMaxWidth.Size = New System.Drawing.Size(59, 20)
        Me.txtMaxWidth.TabIndex = 6
        '
        'lblMaxWidth
        '
        Me.lblMaxWidth.AutoSize = True
        Me.lblMaxWidth.Location = New System.Drawing.Point(4, 99)
        Me.lblMaxWidth.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblMaxWidth.Name = "lblMaxWidth"
        Me.lblMaxWidth.Size = New System.Drawing.Size(101, 13)
        Me.lblMaxWidth.TabIndex = 5
        Me.lblMaxWidth.Text = "Maximim Characters"
        Me.lblMaxWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSignificantDigits
        '
        Me.txtSignificantDigits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSignificantDigits.Location = New System.Drawing.Point(150, 70)
        Me.txtSignificantDigits.Name = "txtSignificantDigits"
        Me.txtSignificantDigits.Size = New System.Drawing.Size(59, 20)
        Me.txtSignificantDigits.TabIndex = 4
        '
        'txtFormat
        '
        Me.txtFormat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFormat.Location = New System.Drawing.Point(104, 18)
        Me.txtFormat.Name = "txtFormat"
        Me.txtFormat.Size = New System.Drawing.Size(105, 20)
        Me.txtFormat.TabIndex = 3
        '
        'lblSigDig
        '
        Me.lblSigDig.AutoSize = True
        Me.lblSigDig.Location = New System.Drawing.Point(4, 73)
        Me.lblSigDig.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSigDig.Name = "lblSigDig"
        Me.lblSigDig.Size = New System.Drawing.Size(85, 13)
        Me.lblSigDig.TabIndex = 2
        Me.lblSigDig.Text = "Significant Digits"
        Me.lblSigDig.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnApply.Location = New System.Drawing.Point(308, 355)
        Me.btnApply.Margin = New System.Windows.Forms.Padding(2)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 23)
        Me.btnApply.TabIndex = 5
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(150, 355)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(2)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'frmOptions
        '
        Me.AcceptButton = Me.btnApply
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(452, 389)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.grpboxNumberFormat)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grpboxDateFormat)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(227, 250)
        Me.Name = "frmOptions"
        Me.Text = "Formats"
        Me.grpboxDateFormat.ResumeLayout(False)
        Me.grpboxDateFormat.PerformLayout()
        Me.grpboxNumberFormat.ResumeLayout(False)
        Me.grpboxNumberFormat.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpboxDateFormat As System.Windows.Forms.GroupBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpboxNumberFormat As System.Windows.Forms.GroupBox
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
End Class
