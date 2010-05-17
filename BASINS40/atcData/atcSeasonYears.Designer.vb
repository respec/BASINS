<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class atcSeasonYears
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.grpBoundaries = New System.Windows.Forms.GroupBox
        Me.btnWaterYear = New System.Windows.Forms.Button
        Me.btnCalendarYear = New System.Windows.Forms.Button
        Me.cboStartMonth = New System.Windows.Forms.ComboBox
        Me.lblYearStart = New System.Windows.Forms.Label
        Me.txtEndDay = New System.Windows.Forms.TextBox
        Me.txtStartDay = New System.Windows.Forms.TextBox
        Me.cboEndMonth = New System.Windows.Forms.ComboBox
        Me.lblYearEnd = New System.Windows.Forms.Label
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.grpYears = New System.Windows.Forms.GroupBox
        Me.btnCommon = New System.Windows.Forms.Button
        Me.btnAll = New System.Windows.Forms.Button
        Me.lblCommonStart = New System.Windows.Forms.Label
        Me.lblCommonEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblOmitBefore = New System.Windows.Forms.Label
        Me.lblOmitAfter = New System.Windows.Forms.Label
        Me.txtOmitAfterYear = New System.Windows.Forms.TextBox
        Me.txtOmitBeforeYear = New System.Windows.Forms.TextBox
        Me.grpBoundaries.SuspendLayout()
        Me.grpYears.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpBoundaries
        '
        Me.grpBoundaries.Controls.Add(Me.btnWaterYear)
        Me.grpBoundaries.Controls.Add(Me.btnCalendarYear)
        Me.grpBoundaries.Controls.Add(Me.cboStartMonth)
        Me.grpBoundaries.Controls.Add(Me.lblYearStart)
        Me.grpBoundaries.Controls.Add(Me.txtEndDay)
        Me.grpBoundaries.Controls.Add(Me.txtStartDay)
        Me.grpBoundaries.Controls.Add(Me.cboEndMonth)
        Me.grpBoundaries.Controls.Add(Me.lblYearEnd)
        Me.grpBoundaries.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpBoundaries.Location = New System.Drawing.Point(0, 0)
        Me.grpBoundaries.Name = "grpBoundaries"
        Me.grpBoundaries.Size = New System.Drawing.Size(176, 107)
        Me.grpBoundaries.TabIndex = 67
        Me.grpBoundaries.TabStop = False
        Me.grpBoundaries.Text = "Year or Season Boundaries"
        '
        'btnWaterYear
        '
        Me.btnWaterYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWaterYear.AutoSize = True
        Me.btnWaterYear.Location = New System.Drawing.Point(89, 19)
        Me.btnWaterYear.Name = "btnWaterYear"
        Me.btnWaterYear.Size = New System.Drawing.Size(70, 23)
        Me.btnWaterYear.TabIndex = 1
        Me.btnWaterYear.Text = "Water"
        Me.btnWaterYear.UseVisualStyleBackColor = True
        '
        'btnCalendarYear
        '
        Me.btnCalendarYear.AutoSize = True
        Me.btnCalendarYear.Location = New System.Drawing.Point(9, 19)
        Me.btnCalendarYear.Name = "btnCalendarYear"
        Me.btnCalendarYear.Size = New System.Drawing.Size(74, 23)
        Me.btnCalendarYear.TabIndex = 0
        Me.btnCalendarYear.Text = "Calendar"
        Me.btnCalendarYear.UseVisualStyleBackColor = True
        '
        'cboStartMonth
        '
        Me.cboStartMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboStartMonth.Location = New System.Drawing.Point(41, 48)
        Me.cboStartMonth.MaxDropDownItems = 12
        Me.cboStartMonth.Name = "cboStartMonth"
        Me.cboStartMonth.Size = New System.Drawing.Size(88, 21)
        Me.cboStartMonth.TabIndex = 2
        Me.cboStartMonth.Text = "January"
        '
        'lblYearStart
        '
        Me.lblYearStart.AutoSize = True
        Me.lblYearStart.Location = New System.Drawing.Point(6, 51)
        Me.lblYearStart.Name = "lblYearStart"
        Me.lblYearStart.Size = New System.Drawing.Size(29, 13)
        Me.lblYearStart.TabIndex = 23
        Me.lblYearStart.Text = "Start"
        '
        'txtEndDay
        '
        Me.txtEndDay.Location = New System.Drawing.Point(135, 75)
        Me.txtEndDay.Name = "txtEndDay"
        Me.txtEndDay.Size = New System.Drawing.Size(24, 20)
        Me.txtEndDay.TabIndex = 5
        Me.txtEndDay.Text = "31"
        Me.txtEndDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtStartDay
        '
        Me.txtStartDay.Location = New System.Drawing.Point(135, 48)
        Me.txtStartDay.Name = "txtStartDay"
        Me.txtStartDay.Size = New System.Drawing.Size(24, 20)
        Me.txtStartDay.TabIndex = 3
        Me.txtStartDay.Text = "1"
        Me.txtStartDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cboEndMonth
        '
        Me.cboEndMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboEndMonth.Location = New System.Drawing.Point(41, 75)
        Me.cboEndMonth.MaxDropDownItems = 12
        Me.cboEndMonth.Name = "cboEndMonth"
        Me.cboEndMonth.Size = New System.Drawing.Size(88, 21)
        Me.cboEndMonth.TabIndex = 4
        Me.cboEndMonth.Text = "December"
        '
        'lblYearEnd
        '
        Me.lblYearEnd.AutoSize = True
        Me.lblYearEnd.Location = New System.Drawing.Point(6, 78)
        Me.lblYearEnd.Name = "lblYearEnd"
        Me.lblYearEnd.Size = New System.Drawing.Size(26, 13)
        Me.lblYearEnd.TabIndex = 61
        Me.lblYearEnd.Text = "End"
        '
        'Splitter1
        '
        Me.Splitter1.BackColor = System.Drawing.SystemColors.Control
        Me.Splitter1.Location = New System.Drawing.Point(176, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(10, 107)
        Me.Splitter1.TabIndex = 6
        Me.Splitter1.TabStop = False
        '
        'grpYears
        '
        Me.grpYears.Controls.Add(Me.btnCommon)
        Me.grpYears.Controls.Add(Me.btnAll)
        Me.grpYears.Controls.Add(Me.lblCommonStart)
        Me.grpYears.Controls.Add(Me.lblCommonEnd)
        Me.grpYears.Controls.Add(Me.lblDataStart)
        Me.grpYears.Controls.Add(Me.lblDataEnd)
        Me.grpYears.Controls.Add(Me.lblOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitAfter)
        Me.grpYears.Controls.Add(Me.txtOmitAfterYear)
        Me.grpYears.Controls.Add(Me.txtOmitBeforeYear)
        Me.grpYears.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpYears.Location = New System.Drawing.Point(186, 0)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Size = New System.Drawing.Size(212, 107)
        Me.grpYears.TabIndex = 69
        Me.grpYears.TabStop = False
        Me.grpYears.Text = "Years to Include"
        '
        'btnCommon
        '
        Me.btnCommon.Location = New System.Drawing.Point(103, 19)
        Me.btnCommon.Name = "btnCommon"
        Me.btnCommon.Size = New System.Drawing.Size(64, 20)
        Me.btnCommon.TabIndex = 56
        Me.btnCommon.Text = "Common"
        Me.btnCommon.UseVisualStyleBackColor = True
        '
        'btnAll
        '
        Me.btnAll.Location = New System.Drawing.Point(44, 19)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(53, 20)
        Me.btnAll.TabIndex = 55
        Me.btnAll.Text = "All"
        Me.btnAll.UseVisualStyleBackColor = True
        '
        'lblCommonStart
        '
        Me.lblCommonStart.AutoSize = True
        Me.lblCommonStart.Location = New System.Drawing.Point(119, 42)
        Me.lblCommonStart.Name = "lblCommonStart"
        Me.lblCommonStart.Size = New System.Drawing.Size(0, 13)
        Me.lblCommonStart.TabIndex = 54
        Me.lblCommonStart.Tag = ""
        '
        'lblCommonEnd
        '
        Me.lblCommonEnd.AutoSize = True
        Me.lblCommonEnd.Location = New System.Drawing.Point(119, 65)
        Me.lblCommonEnd.Name = "lblCommonEnd"
        Me.lblCommonEnd.Size = New System.Drawing.Size(0, 13)
        Me.lblCommonEnd.TabIndex = 53
        Me.lblCommonEnd.Tag = ""
        '
        'lblDataStart
        '
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(41, 42)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(0, 13)
        Me.lblDataStart.TabIndex = 52
        Me.lblDataStart.Tag = "Data Starts"
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(41, 65)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(0, 13)
        Me.lblDataEnd.TabIndex = 51
        Me.lblDataEnd.Tag = ""
        '
        'lblOmitBefore
        '
        Me.lblOmitBefore.AutoSize = True
        Me.lblOmitBefore.Location = New System.Drawing.Point(6, 42)
        Me.lblOmitBefore.Name = "lblOmitBefore"
        Me.lblOmitBefore.Size = New System.Drawing.Size(29, 13)
        Me.lblOmitBefore.TabIndex = 40
        Me.lblOmitBefore.Text = "Start"
        Me.lblOmitBefore.Visible = False
        '
        'lblOmitAfter
        '
        Me.lblOmitAfter.AutoSize = True
        Me.lblOmitAfter.Location = New System.Drawing.Point(6, 65)
        Me.lblOmitAfter.Name = "lblOmitAfter"
        Me.lblOmitAfter.Size = New System.Drawing.Size(26, 13)
        Me.lblOmitAfter.TabIndex = 43
        Me.lblOmitAfter.Text = "End"
        Me.lblOmitAfter.Visible = False
        '
        'txtOmitAfterYear
        '
        Me.txtOmitAfterYear.Location = New System.Drawing.Point(169, 74)
        Me.txtOmitAfterYear.Name = "txtOmitAfterYear"
        Me.txtOmitAfterYear.Size = New System.Drawing.Size(37, 20)
        Me.txtOmitAfterYear.TabIndex = 10
        Me.txtOmitAfterYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtOmitBeforeYear
        '
        Me.txtOmitBeforeYear.Location = New System.Drawing.Point(169, 48)
        Me.txtOmitBeforeYear.Name = "txtOmitBeforeYear"
        Me.txtOmitBeforeYear.Size = New System.Drawing.Size(37, 20)
        Me.txtOmitBeforeYear.TabIndex = 9
        Me.txtOmitBeforeYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'atcSeasonYears
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpYears)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.grpBoundaries)
        Me.Name = "atcSeasonYears"
        Me.Size = New System.Drawing.Size(401, 107)
        Me.grpBoundaries.ResumeLayout(False)
        Me.grpBoundaries.PerformLayout()
        Me.grpYears.ResumeLayout(False)
        Me.grpYears.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpBoundaries As System.Windows.Forms.GroupBox
    Friend WithEvents cboStartMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearStart As System.Windows.Forms.Label
    Friend WithEvents txtEndDay As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDay As System.Windows.Forms.TextBox
    Friend WithEvents cboEndMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearEnd As System.Windows.Forms.Label
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents grpYears As System.Windows.Forms.GroupBox
    Friend WithEvents lblOmitBefore As System.Windows.Forms.Label
    Friend WithEvents lblOmitAfter As System.Windows.Forms.Label
    Friend WithEvents txtOmitAfterYear As System.Windows.Forms.TextBox
    Friend WithEvents txtOmitBeforeYear As System.Windows.Forms.TextBox
    Friend WithEvents btnWaterYear As System.Windows.Forms.Button
    Friend WithEvents btnCalendarYear As System.Windows.Forms.Button
    Friend WithEvents btnCommon As System.Windows.Forms.Button
    Friend WithEvents btnAll As System.Windows.Forms.Button
    Friend WithEvents lblCommonStart As System.Windows.Forms.Label
    Friend WithEvents lblCommonEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label

End Class
