<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDFLOWArgs
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDFLOWArgs))
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.gbBio = New System.Windows.Forms.GroupBox
        Me.tbBio3 = New System.Windows.Forms.TextBox
        Me.tbBio4 = New System.Windows.Forms.TextBox
        Me.tbBio2 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbBio1 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.rbBio3 = New System.Windows.Forms.RadioButton
        Me.rbBio2 = New System.Windows.Forms.RadioButton
        Me.rbBio1 = New System.Windows.Forms.RadioButton
        Me.ckbBio = New System.Windows.Forms.CheckBox
        Me.Button3 = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.tbNonBio3 = New System.Windows.Forms.TextBox
        Me.tbNonBio4 = New System.Windows.Forms.TextBox
        Me.tbNonBio2 = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.tbNonBio1 = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.rbNonBio3 = New System.Windows.Forms.RadioButton
        Me.rbNonBio2 = New System.Windows.Forms.RadioButton
        Me.rbNonBio1 = New System.Windows.Forms.RadioButton
        Me.clbDataSets = New System.Windows.Forms.CheckedListBox
        Me.grpDates = New System.Windows.Forms.GroupBox
        Me.cboStartMonth = New System.Windows.Forms.ComboBox
        Me.lblYearStart = New System.Windows.Forms.Label
        Me.tbEndDay = New System.Windows.Forms.TextBox
        Me.tbStartDay = New System.Windows.Forms.TextBox
        Me.cboEndMonth = New System.Windows.Forms.ComboBox
        Me.lblYearEnd = New System.Windows.Forms.Label
        Me.grpYears = New System.Windows.Forms.GroupBox
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblOmitBefore = New System.Windows.Forms.Label
        Me.lblOmitAfter = New System.Windows.Forms.Label
        Me.tbOmitAfterYear = New System.Windows.Forms.TextBox
        Me.tbOmitBeforeYear = New System.Windows.Forms.TextBox
        Me.gbTextOutput = New System.Windows.Forms.GroupBox
        Me.txtOutputDir = New System.Windows.Forms.TextBox
        Me.lblOutputDir = New System.Windows.Forms.Label
        Me.txtOutputRootName = New System.Windows.Forms.TextBox
        Me.lblBaseFilename = New System.Windows.Forms.Label
        Me.gbBio.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.grpDates.SuspendLayout()
        Me.grpYears.SuspendLayout()
        Me.gbTextOutput.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(684, 535)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 28)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Location = New System.Drawing.Point(792, 535)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(100, 28)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'gbBio
        '
        Me.gbBio.Controls.Add(Me.tbBio3)
        Me.gbBio.Controls.Add(Me.tbBio4)
        Me.gbBio.Controls.Add(Me.tbBio2)
        Me.gbBio.Controls.Add(Me.Label4)
        Me.gbBio.Controls.Add(Me.Label3)
        Me.gbBio.Controls.Add(Me.Label2)
        Me.gbBio.Controls.Add(Me.tbBio1)
        Me.gbBio.Controls.Add(Me.Label1)
        Me.gbBio.Controls.Add(Me.rbBio3)
        Me.gbBio.Controls.Add(Me.rbBio2)
        Me.gbBio.Controls.Add(Me.rbBio1)
        Me.gbBio.Controls.Add(Me.ckbBio)
        Me.gbBio.Location = New System.Drawing.Point(447, 15)
        Me.gbBio.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbBio.Name = "gbBio"
        Me.gbBio.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbBio.Size = New System.Drawing.Size(443, 282)
        Me.gbBio.TabIndex = 2
        Me.gbBio.TabStop = False
        Me.gbBio.Text = "Biological Design Flow Parameters"
        '
        'tbBio3
        '
        Me.tbBio3.Enabled = False
        Me.tbBio3.Location = New System.Drawing.Point(351, 209)
        Me.tbBio3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbBio3.Name = "tbBio3"
        Me.tbBio3.Size = New System.Drawing.Size(60, 22)
        Me.tbBio3.TabIndex = 11
        Me.tbBio3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbBio4
        '
        Me.tbBio4.Enabled = False
        Me.tbBio4.Location = New System.Drawing.Point(351, 240)
        Me.tbBio4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbBio4.Name = "tbBio4"
        Me.tbBio4.Size = New System.Drawing.Size(60, 22)
        Me.tbBio4.TabIndex = 10
        Me.tbBio4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbBio2
        '
        Me.tbBio2.Enabled = False
        Me.tbBio2.Location = New System.Drawing.Point(351, 178)
        Me.tbBio2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbBio2.Name = "tbBio2"
        Me.tbBio2.Size = New System.Drawing.Size(60, 22)
        Me.tbBio2.TabIndex = 9
        Me.tbBio2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Enabled = False
        Me.Label4.Location = New System.Drawing.Point(4, 244)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(330, 17)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Average number of excursions counted per cluster:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Enabled = False
        Me.Label3.Location = New System.Drawing.Point(4, 213)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(289, 17)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Length of excursion clustering period (days):"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Enabled = False
        Me.Label2.Location = New System.Drawing.Point(4, 182)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(300, 17)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Average number of years between excursions:"
        '
        'tbBio1
        '
        Me.tbBio1.Enabled = False
        Me.tbBio1.Location = New System.Drawing.Point(351, 148)
        Me.tbBio1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbBio1.Name = "tbBio1"
        Me.tbBio1.Size = New System.Drawing.Size(60, 22)
        Me.tbBio1.TabIndex = 5
        Me.tbBio1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Enabled = False
        Me.Label1.Location = New System.Drawing.Point(4, 151)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(195, 17)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Flow averaging period (days):"
        '
        'rbBio3
        '
        Me.rbBio3.AutoSize = True
        Me.rbBio3.Location = New System.Drawing.Point(29, 108)
        Me.rbBio3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rbBio3.Name = "rbBio3"
        Me.rbBio3.Size = New System.Drawing.Size(87, 21)
        Me.rbBio3.TabIndex = 3
        Me.rbBio3.Text = "Ammonia"
        Me.rbBio3.UseVisualStyleBackColor = True
        '
        'rbBio2
        '
        Me.rbBio2.AutoSize = True
        Me.rbBio2.Location = New System.Drawing.Point(29, 80)
        Me.rbBio2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rbBio2.Name = "rbBio2"
        Me.rbBio2.Size = New System.Drawing.Size(305, 21)
        Me.rbBio2.TabIndex = 2
        Me.rbBio2.Text = "Criterion continuous concentration (chronic)"
        Me.rbBio2.UseVisualStyleBackColor = True
        '
        'rbBio1
        '
        Me.rbBio1.AutoSize = True
        Me.rbBio1.Checked = True
        Me.rbBio1.Location = New System.Drawing.Point(29, 52)
        Me.rbBio1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rbBio1.Name = "rbBio1"
        Me.rbBio1.Size = New System.Drawing.Size(283, 21)
        Me.rbBio1.TabIndex = 1
        Me.rbBio1.TabStop = True
        Me.rbBio1.Text = "Criterion maximum concentration (acute)"
        Me.rbBio1.UseVisualStyleBackColor = True
        '
        'ckbBio
        '
        Me.ckbBio.AutoSize = True
        Me.ckbBio.Checked = True
        Me.ckbBio.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ckbBio.Location = New System.Drawing.Point(8, 23)
        Me.ckbBio.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ckbBio.Name = "ckbBio"
        Me.ckbBio.Size = New System.Drawing.Size(109, 21)
        Me.ckbBio.TabIndex = 0
        Me.ckbBio.Text = "Use defaults"
        Me.ckbBio.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Location = New System.Drawing.Point(576, 535)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(100, 28)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Help"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.tbNonBio3)
        Me.GroupBox2.Controls.Add(Me.tbNonBio4)
        Me.GroupBox2.Controls.Add(Me.tbNonBio2)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.tbNonBio1)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.rbNonBio3)
        Me.GroupBox2.Controls.Add(Me.rbNonBio2)
        Me.GroupBox2.Controls.Add(Me.rbNonBio1)
        Me.GroupBox2.Location = New System.Drawing.Point(447, 305)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.Size = New System.Drawing.Size(443, 175)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Non-Biological Design Flow Parameters"
        '
        'tbNonBio3
        '
        Me.tbNonBio3.Enabled = False
        Me.tbNonBio3.Location = New System.Drawing.Point(351, 108)
        Me.tbNonBio3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbNonBio3.Name = "tbNonBio3"
        Me.tbNonBio3.Size = New System.Drawing.Size(60, 22)
        Me.tbNonBio3.TabIndex = 11
        Me.tbNonBio3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbNonBio4
        '
        Me.tbNonBio4.Enabled = False
        Me.tbNonBio4.Location = New System.Drawing.Point(351, 135)
        Me.tbNonBio4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbNonBio4.Name = "tbNonBio4"
        Me.tbNonBio4.Size = New System.Drawing.Size(60, 22)
        Me.tbNonBio4.TabIndex = 10
        Me.tbNonBio4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbNonBio2
        '
        Me.tbNonBio2.Enabled = False
        Me.tbNonBio2.Location = New System.Drawing.Point(351, 71)
        Me.tbNonBio2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbNonBio2.Name = "tbNonBio2"
        Me.tbNonBio2.Size = New System.Drawing.Size(60, 22)
        Me.tbNonBio2.TabIndex = 9
        Me.tbNonBio2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Enabled = False
        Me.Label5.Location = New System.Drawing.Point(411, 113)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(26, 17)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "cfs"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Enabled = False
        Me.Label6.Location = New System.Drawing.Point(411, 139)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(20, 17)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "%"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Enabled = False
        Me.Label7.Location = New System.Drawing.Point(25, 75)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(306, 17)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Return period on years with excursions (years):"
        '
        'tbNonBio1
        '
        Me.tbNonBio1.Enabled = False
        Me.tbNonBio1.Location = New System.Drawing.Point(351, 44)
        Me.tbNonBio1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbNonBio1.Name = "tbNonBio1"
        Me.tbNonBio1.Size = New System.Drawing.Size(60, 22)
        Me.tbNonBio1.TabIndex = 5
        Me.tbNonBio1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Enabled = False
        Me.Label8.Location = New System.Drawing.Point(25, 48)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(195, 17)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Flow averaging period (days):"
        '
        'rbNonBio3
        '
        Me.rbNonBio3.AutoSize = True
        Me.rbNonBio3.Location = New System.Drawing.Point(8, 137)
        Me.rbNonBio3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rbNonBio3.Name = "rbNonBio3"
        Me.rbNonBio3.Size = New System.Drawing.Size(127, 21)
        Me.rbNonBio3.TabIndex = 3
        Me.rbNonBio3.Text = "Flow percentile:"
        Me.rbNonBio3.UseVisualStyleBackColor = True
        '
        'rbNonBio2
        '
        Me.rbNonBio2.AutoSize = True
        Me.rbNonBio2.Location = New System.Drawing.Point(8, 108)
        Me.rbNonBio2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rbNonBio2.Name = "rbNonBio2"
        Me.rbNonBio2.Size = New System.Drawing.Size(142, 21)
        Me.rbNonBio2.TabIndex = 2
        Me.rbNonBio2.Text = "Explicit flow value:"
        Me.rbNonBio2.UseVisualStyleBackColor = True
        '
        'rbNonBio1
        '
        Me.rbNonBio1.AutoSize = True
        Me.rbNonBio1.Checked = True
        Me.rbNonBio1.Location = New System.Drawing.Point(8, 23)
        Me.rbNonBio1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rbNonBio1.Name = "rbNonBio1"
        Me.rbNonBio1.Size = New System.Drawing.Size(107, 21)
        Me.rbNonBio1.TabIndex = 1
        Me.rbNonBio1.TabStop = True
        Me.rbNonBio1.Text = "Hydrological"
        Me.rbNonBio1.UseVisualStyleBackColor = True
        '
        'clbDataSets
        '
        Me.clbDataSets.CheckOnClick = True
        Me.clbDataSets.FormattingEnabled = True
        Me.clbDataSets.Location = New System.Drawing.Point(16, 15)
        Me.clbDataSets.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.clbDataSets.Name = "clbDataSets"
        Me.clbDataSets.Size = New System.Drawing.Size(401, 276)
        Me.clbDataSets.TabIndex = 5
        '
        'grpDates
        '
        Me.grpDates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDates.Controls.Add(Me.cboStartMonth)
        Me.grpDates.Controls.Add(Me.lblYearStart)
        Me.grpDates.Controls.Add(Me.tbEndDay)
        Me.grpDates.Controls.Add(Me.tbStartDay)
        Me.grpDates.Controls.Add(Me.cboEndMonth)
        Me.grpDates.Controls.Add(Me.lblYearEnd)
        Me.grpDates.Location = New System.Drawing.Point(16, 299)
        Me.grpDates.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpDates.Name = "grpDates"
        Me.grpDates.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpDates.Size = New System.Drawing.Size(403, 90)
        Me.grpDates.TabIndex = 66
        Me.grpDates.TabStop = False
        Me.grpDates.Text = "Year or Season Boundaries"
        '
        'cboStartMonth
        '
        Me.cboStartMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboStartMonth.Location = New System.Drawing.Point(55, 23)
        Me.cboStartMonth.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboStartMonth.MaxDropDownItems = 12
        Me.cboStartMonth.Name = "cboStartMonth"
        Me.cboStartMonth.Size = New System.Drawing.Size(116, 24)
        Me.cboStartMonth.TabIndex = 3
        Me.cboStartMonth.Text = "April"
        '
        'lblYearStart
        '
        Me.lblYearStart.AutoSize = True
        Me.lblYearStart.Location = New System.Drawing.Point(8, 27)
        Me.lblYearStart.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblYearStart.Name = "lblYearStart"
        Me.lblYearStart.Size = New System.Drawing.Size(38, 17)
        Me.lblYearStart.TabIndex = 23
        Me.lblYearStart.Text = "Start"
        '
        'tbEndDay
        '
        Me.tbEndDay.Location = New System.Drawing.Point(180, 57)
        Me.tbEndDay.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbEndDay.Name = "tbEndDay"
        Me.tbEndDay.Size = New System.Drawing.Size(31, 22)
        Me.tbEndDay.TabIndex = 63
        Me.tbEndDay.Text = "31"
        Me.tbEndDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbStartDay
        '
        Me.tbStartDay.Location = New System.Drawing.Point(180, 23)
        Me.tbStartDay.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbStartDay.Name = "tbStartDay"
        Me.tbStartDay.Size = New System.Drawing.Size(31, 22)
        Me.tbStartDay.TabIndex = 4
        Me.tbStartDay.Text = "1"
        Me.tbStartDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboEndMonth
        '
        Me.cboEndMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboEndMonth.Location = New System.Drawing.Point(55, 57)
        Me.cboEndMonth.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboEndMonth.MaxDropDownItems = 12
        Me.cboEndMonth.Name = "cboEndMonth"
        Me.cboEndMonth.Size = New System.Drawing.Size(116, 24)
        Me.cboEndMonth.TabIndex = 11
        Me.cboEndMonth.Text = "March"
        '
        'lblYearEnd
        '
        Me.lblYearEnd.AutoSize = True
        Me.lblYearEnd.Location = New System.Drawing.Point(8, 60)
        Me.lblYearEnd.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblYearEnd.Name = "lblYearEnd"
        Me.lblYearEnd.Size = New System.Drawing.Size(33, 17)
        Me.lblYearEnd.TabIndex = 61
        Me.lblYearEnd.Text = "End"
        '
        'grpYears
        '
        Me.grpYears.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpYears.Controls.Add(Me.lblDataStart)
        Me.grpYears.Controls.Add(Me.lblDataEnd)
        Me.grpYears.Controls.Add(Me.lblOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitAfter)
        Me.grpYears.Controls.Add(Me.tbOmitAfterYear)
        Me.grpYears.Controls.Add(Me.tbOmitBeforeYear)
        Me.grpYears.Location = New System.Drawing.Point(16, 397)
        Me.grpYears.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpYears.Size = New System.Drawing.Size(403, 83)
        Me.grpYears.TabIndex = 65
        Me.grpYears.TabStop = False
        Me.grpYears.Text = "Years to Include in Analysis"
        '
        'lblDataStart
        '
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(145, 27)
        Me.lblDataStart.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(79, 17)
        Me.lblDataStart.TabIndex = 0
        Me.lblDataStart.Tag = "Data Starts"
        Me.lblDataStart.Text = "Data Starts"
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(145, 59)
        Me.lblDataEnd.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(74, 17)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Tag = "Data Ends"
        Me.lblDataEnd.Text = "Data Ends"
        '
        'lblOmitBefore
        '
        Me.lblOmitBefore.AutoSize = True
        Me.lblOmitBefore.Location = New System.Drawing.Point(8, 27)
        Me.lblOmitBefore.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOmitBefore.Name = "lblOmitBefore"
        Me.lblOmitBefore.Size = New System.Drawing.Size(72, 17)
        Me.lblOmitBefore.TabIndex = 40
        Me.lblOmitBefore.Text = "Start Year"
        '
        'lblOmitAfter
        '
        Me.lblOmitAfter.AutoSize = True
        Me.lblOmitAfter.Location = New System.Drawing.Point(8, 59)
        Me.lblOmitAfter.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOmitAfter.Name = "lblOmitAfter"
        Me.lblOmitAfter.Size = New System.Drawing.Size(67, 17)
        Me.lblOmitAfter.TabIndex = 43
        Me.lblOmitAfter.Text = "End Year"
        '
        'tbOmitAfterYear
        '
        Me.tbOmitAfterYear.Location = New System.Drawing.Point(88, 55)
        Me.tbOmitAfterYear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbOmitAfterYear.Name = "tbOmitAfterYear"
        Me.tbOmitAfterYear.Size = New System.Drawing.Size(48, 22)
        Me.tbOmitAfterYear.TabIndex = 6
        Me.tbOmitAfterYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbOmitBeforeYear
        '
        Me.tbOmitBeforeYear.Location = New System.Drawing.Point(88, 23)
        Me.tbOmitBeforeYear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tbOmitBeforeYear.Name = "tbOmitBeforeYear"
        Me.tbOmitBeforeYear.Size = New System.Drawing.Size(48, 22)
        Me.tbOmitBeforeYear.TabIndex = 5
        Me.tbOmitBeforeYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gbTextOutput
        '
        Me.gbTextOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbTextOutput.Controls.Add(Me.txtOutputDir)
        Me.gbTextOutput.Controls.Add(Me.lblOutputDir)
        Me.gbTextOutput.Controls.Add(Me.txtOutputRootName)
        Me.gbTextOutput.Controls.Add(Me.lblBaseFilename)
        Me.gbTextOutput.Location = New System.Drawing.Point(16, 487)
        Me.gbTextOutput.Name = "gbTextOutput"
        Me.gbTextOutput.Size = New System.Drawing.Size(546, 78)
        Me.gbTextOutput.TabIndex = 74
        Me.gbTextOutput.TabStop = False
        Me.gbTextOutput.Text = "Output"
        Me.gbTextOutput.Visible = False
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputDir.Location = New System.Drawing.Point(81, 15)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(459, 22)
        Me.txtOutputDir.TabIndex = 12
        '
        'lblOutputDir
        '
        Me.lblOutputDir.AutoSize = True
        Me.lblOutputDir.Location = New System.Drawing.Point(38, 18)
        Me.lblOutputDir.Name = "lblOutputDir"
        Me.lblOutputDir.Size = New System.Drawing.Size(37, 17)
        Me.lblOutputDir.TabIndex = 31
        Me.lblOutputDir.Text = "Path"
        '
        'txtOutputRootName
        '
        Me.txtOutputRootName.Location = New System.Drawing.Point(81, 46)
        Me.txtOutputRootName.Name = "txtOutputRootName"
        Me.txtOutputRootName.Size = New System.Drawing.Size(241, 22)
        Me.txtOutputRootName.TabIndex = 13
        '
        'lblBaseFilename
        '
        Me.lblBaseFilename.AutoSize = True
        Me.lblBaseFilename.Location = New System.Drawing.Point(6, 49)
        Me.lblBaseFilename.Name = "lblBaseFilename"
        Me.lblBaseFilename.Size = New System.Drawing.Size(69, 17)
        Me.lblBaseFilename.TabIndex = 30
        Me.lblBaseFilename.Text = "File Prefix"
        '
        'frmDFLOWArgs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(905, 576)
        Me.Controls.Add(Me.gbTextOutput)
        Me.Controls.Add(Me.grpDates)
        Me.Controls.Add(Me.grpYears)
        Me.Controls.Add(Me.clbDataSets)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.gbBio)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmDFLOWArgs"
        Me.Text = "DFLOW Inputs"
        Me.gbBio.ResumeLayout(False)
        Me.gbBio.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.grpDates.ResumeLayout(False)
        Me.grpDates.PerformLayout()
        Me.grpYears.ResumeLayout(False)
        Me.grpYears.PerformLayout()
        Me.gbTextOutput.ResumeLayout(False)
        Me.gbTextOutput.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents gbBio As System.Windows.Forms.GroupBox
    Friend WithEvents rbBio3 As System.Windows.Forms.RadioButton
    Friend WithEvents rbBio2 As System.Windows.Forms.RadioButton
    Friend WithEvents rbBio1 As System.Windows.Forms.RadioButton
    Friend WithEvents ckbBio As System.Windows.Forms.CheckBox
    Friend WithEvents tbBio1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbBio3 As System.Windows.Forms.TextBox
    Friend WithEvents tbBio4 As System.Windows.Forms.TextBox
    Friend WithEvents tbBio2 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents tbNonBio3 As System.Windows.Forms.TextBox
    Friend WithEvents tbNonBio4 As System.Windows.Forms.TextBox
    Friend WithEvents tbNonBio2 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbNonBio1 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents rbNonBio3 As System.Windows.Forms.RadioButton
    Friend WithEvents rbNonBio2 As System.Windows.Forms.RadioButton
    Friend WithEvents rbNonBio1 As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents clbDataSets As System.Windows.Forms.CheckedListBox
    Friend WithEvents grpDates As System.Windows.Forms.GroupBox
    Friend WithEvents cboStartMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearStart As System.Windows.Forms.Label
    Friend WithEvents tbEndDay As System.Windows.Forms.TextBox
    Friend WithEvents tbStartDay As System.Windows.Forms.TextBox
    Friend WithEvents cboEndMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearEnd As System.Windows.Forms.Label
    Friend WithEvents grpYears As System.Windows.Forms.GroupBox
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblOmitBefore As System.Windows.Forms.Label
    Friend WithEvents lblOmitAfter As System.Windows.Forms.Label
    Friend WithEvents tbOmitAfterYear As System.Windows.Forms.TextBox
    Friend WithEvents tbOmitBeforeYear As System.Windows.Forms.TextBox
    Friend WithEvents gbTextOutput As System.Windows.Forms.GroupBox
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputDir As System.Windows.Forms.Label
    Friend WithEvents txtOutputRootName As System.Windows.Forms.TextBox
    Friend WithEvents lblBaseFilename As System.Windows.Forms.Label
End Class
