<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSpecifyYearsSeasons
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecifyYearsSeasons))
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.txtStartDay = New System.Windows.Forms.TextBox
        Me.lblYearStart = New System.Windows.Forms.Label
        Me.cboStartMonth = New System.Windows.Forms.ComboBox
        Me.txtEndDay = New System.Windows.Forms.TextBox
        Me.cboEndMonth = New System.Windows.Forms.ComboBox
        Me.lblYearEnd = New System.Windows.Forms.Label
        Me.txtOmitBeforeYear = New System.Windows.Forms.TextBox
        Me.txtOmitAfterYear = New System.Windows.Forms.TextBox
        Me.lblOmitAfter = New System.Windows.Forms.Label
        Me.lblOmitBefore = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.grpYears = New System.Windows.Forms.GroupBox
        Me.grpDates = New System.Windows.Forms.GroupBox
        Me.txtNDays = New System.Windows.Forms.TextBox
        Me.lblNDays = New System.Windows.Forms.Label
        Me.grpYears.SuspendLayout()
        Me.grpDates.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblDataStart
        '
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(109, 22)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(60, 13)
        Me.lblDataStart.TabIndex = 0
        Me.lblDataStart.Tag = "Data Starts"
        Me.lblDataStart.Text = "Data Starts"
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(109, 48)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(57, 13)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Tag = "Data Ends"
        Me.lblDataEnd.Text = "Data Ends"
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
        'lblYearStart
        '
        Me.lblYearStart.AutoSize = True
        Me.lblYearStart.Location = New System.Drawing.Point(6, 22)
        Me.lblYearStart.Name = "lblYearStart"
        Me.lblYearStart.Size = New System.Drawing.Size(29, 13)
        Me.lblYearStart.TabIndex = 23
        Me.lblYearStart.Text = "Start"
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
        'txtEndDay
        '
        Me.txtEndDay.Location = New System.Drawing.Point(135, 46)
        Me.txtEndDay.Name = "txtEndDay"
        Me.txtEndDay.Size = New System.Drawing.Size(24, 20)
        Me.txtEndDay.TabIndex = 63
        Me.txtEndDay.Text = "31"
        Me.txtEndDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
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
        'txtOmitBeforeYear
        '
        Me.txtOmitBeforeYear.Location = New System.Drawing.Point(66, 19)
        Me.txtOmitBeforeYear.Name = "txtOmitBeforeYear"
        Me.txtOmitBeforeYear.Size = New System.Drawing.Size(37, 20)
        Me.txtOmitBeforeYear.TabIndex = 5
        Me.txtOmitBeforeYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtOmitAfterYear
        '
        Me.txtOmitAfterYear.Location = New System.Drawing.Point(66, 45)
        Me.txtOmitAfterYear.Name = "txtOmitAfterYear"
        Me.txtOmitAfterYear.Size = New System.Drawing.Size(37, 20)
        Me.txtOmitAfterYear.TabIndex = 6
        Me.txtOmitAfterYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblOmitAfter
        '
        Me.lblOmitAfter.AutoSize = True
        Me.lblOmitAfter.Location = New System.Drawing.Point(6, 48)
        Me.lblOmitAfter.Name = "lblOmitAfter"
        Me.lblOmitAfter.Size = New System.Drawing.Size(51, 13)
        Me.lblOmitAfter.TabIndex = 43
        Me.lblOmitAfter.Text = "End Year"
        '
        'lblOmitBefore
        '
        Me.lblOmitBefore.AutoSize = True
        Me.lblOmitBefore.Location = New System.Drawing.Point(6, 22)
        Me.lblOmitBefore.Name = "lblOmitBefore"
        Me.lblOmitBefore.Size = New System.Drawing.Size(54, 13)
        Me.lblOmitBefore.TabIndex = 40
        Me.lblOmitBefore.Text = "Start Year"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(285, 190)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 24)
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(207, 190)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 24)
        Me.btnOk.TabIndex = 16
        Me.btnOk.Text = "Ok"
        '
        'grpYears
        '
        Me.grpYears.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpYears.Controls.Add(Me.lblDataStart)
        Me.grpYears.Controls.Add(Me.lblDataEnd)
        Me.grpYears.Controls.Add(Me.lblOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitAfter)
        Me.grpYears.Controls.Add(Me.txtOmitAfterYear)
        Me.grpYears.Controls.Add(Me.txtOmitBeforeYear)
        Me.grpYears.Location = New System.Drawing.Point(12, 98)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Size = New System.Drawing.Size(345, 78)
        Me.grpYears.TabIndex = 44
        Me.grpYears.TabStop = False
        Me.grpYears.Text = "Years to Include in Analysis"
        '
        'grpDates
        '
        Me.grpDates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDates.Controls.Add(Me.cboStartMonth)
        Me.grpDates.Controls.Add(Me.lblYearStart)
        Me.grpDates.Controls.Add(Me.txtEndDay)
        Me.grpDates.Controls.Add(Me.txtStartDay)
        Me.grpDates.Controls.Add(Me.cboEndMonth)
        Me.grpDates.Controls.Add(Me.lblYearEnd)
        Me.grpDates.Location = New System.Drawing.Point(12, 12)
        Me.grpDates.Name = "grpDates"
        Me.grpDates.Size = New System.Drawing.Size(345, 73)
        Me.grpDates.TabIndex = 64
        Me.grpDates.TabStop = False
        Me.grpDates.Text = "Year or Season Boundaries"
        '
        'txtNDays
        '
        Me.txtNDays.Location = New System.Drawing.Point(101, 193)
        Me.txtNDays.Name = "txtNDays"
        Me.txtNDays.Size = New System.Drawing.Size(40, 20)
        Me.txtNDays.TabIndex = 66
        Me.txtNDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtNDays.Visible = False
        '
        'lblNDays
        '
        Me.lblNDays.AutoSize = True
        Me.lblNDays.Location = New System.Drawing.Point(12, 196)
        Me.lblNDays.Name = "lblNDays"
        Me.lblNDays.Size = New System.Drawing.Size(83, 13)
        Me.lblNDays.TabIndex = 65
        Me.lblNDays.Text = "Number of Days"
        Me.lblNDays.Visible = False
        '
        'frmSpecifyYearsSeasons
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(369, 226)
        Me.Controls.Add(Me.txtNDays)
        Me.Controls.Add(Me.lblNDays)
        Me.Controls.Add(Me.grpDates)
        Me.Controls.Add(Me.grpYears)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSpecifyYearsSeasons"
        Me.Text = "Specify Years and Seasons"
        Me.grpYears.ResumeLayout(False)
        Me.grpYears.PerformLayout()
        Me.grpDates.ResumeLayout(False)
        Me.grpDates.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents txtStartDay As System.Windows.Forms.TextBox
    Friend WithEvents lblYearStart As System.Windows.Forms.Label
    Friend WithEvents cboStartMonth As System.Windows.Forms.ComboBox
    Friend WithEvents txtOmitBeforeYear As System.Windows.Forms.TextBox
    Friend WithEvents txtOmitAfterYear As System.Windows.Forms.TextBox
    Friend WithEvents lblOmitAfter As System.Windows.Forms.Label
    Friend WithEvents lblOmitBefore As System.Windows.Forms.Label
    Friend WithEvents txtEndDay As System.Windows.Forms.TextBox
    Friend WithEvents cboEndMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearEnd As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grpYears As System.Windows.Forms.GroupBox
    Friend WithEvents grpDates As System.Windows.Forms.GroupBox
    Friend WithEvents txtNDays As System.Windows.Forms.TextBox
    Friend WithEvents lblNDays As System.Windows.Forms.Label
End Class
