<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRecess
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRecess))
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabConfig = New System.Windows.Forms.TabPage
        Me.chkSaveInterimToFile = New System.Windows.Forms.CheckBox
        Me.gbRnages = New System.Windows.Forms.GroupBox
        Me.btnBrowseRangeFile = New System.Windows.Forms.Button
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtOutputDir = New System.Windows.Forms.TextBox
        Me.lblOutpuDir = New System.Windows.Forms.Label
        Me.txtMinRecessionDays = New System.Windows.Forms.TextBox
        Me.lblRecessionDays = New System.Windows.Forms.Label
        Me.gbMonthSeason = New System.Windows.Forms.GroupBox
        Me.rdoNoSeason = New System.Windows.Forms.RadioButton
        Me.rdoWinter = New System.Windows.Forms.RadioButton
        Me.rdoFall = New System.Windows.Forms.RadioButton
        Me.rdoSummer = New System.Windows.Forms.RadioButton
        Me.rdoSpring = New System.Windows.Forms.RadioButton
        Me.lstMonths = New System.Windows.Forms.ListBox
        Me.gbDates = New System.Windows.Forms.GroupBox
        Me.txtEndDateUser = New System.Windows.Forms.TextBox
        Me.txtStartDateUser = New System.Windows.Forms.TextBox
        Me.btnExamineData = New System.Windows.Forms.Button
        Me.txtDataEnd = New System.Windows.Forms.TextBox
        Me.txtDataStart = New System.Windows.Forms.TextBox
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.tabAnalysis = New System.Windows.Forms.TabPage
        Me.lstRecessSegments = New System.Windows.Forms.CheckedListBox
        Me.scDisplay = New System.Windows.Forms.SplitContainer
        Me.txtAnalysisResults = New System.Windows.Forms.TextBox
        Me.lstTable = New System.Windows.Forms.ListBox
        Me.gbToolBar = New System.Windows.Forms.GroupBox
        Me.btnSummary = New System.Windows.Forms.Button
        Me.btnAnalyse = New System.Windows.Forms.Button
        Me.btnGetAllSegments = New System.Windows.Forms.Button
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain.SuspendLayout()
        Me.tabConfig.SuspendLayout()
        Me.gbRnages.SuspendLayout()
        Me.gbMonthSeason.SuspendLayout()
        Me.gbDates.SuspendLayout()
        Me.tabAnalysis.SuspendLayout()
        Me.scDisplay.Panel1.SuspendLayout()
        Me.scDisplay.SuspendLayout()
        Me.gbToolBar.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMain.Controls.Add(Me.tabConfig)
        Me.tabMain.Controls.Add(Me.tabAnalysis)
        Me.tabMain.Location = New System.Drawing.Point(0, 27)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(658, 404)
        Me.tabMain.TabIndex = 0
        '
        'tabConfig
        '
        Me.tabConfig.Controls.Add(Me.chkSaveInterimToFile)
        Me.tabConfig.Controls.Add(Me.gbRnages)
        Me.tabConfig.Controls.Add(Me.txtOutputDir)
        Me.tabConfig.Controls.Add(Me.lblOutpuDir)
        Me.tabConfig.Controls.Add(Me.txtMinRecessionDays)
        Me.tabConfig.Controls.Add(Me.lblRecessionDays)
        Me.tabConfig.Controls.Add(Me.gbMonthSeason)
        Me.tabConfig.Controls.Add(Me.gbDates)
        Me.tabConfig.Location = New System.Drawing.Point(4, 22)
        Me.tabConfig.Name = "tabConfig"
        Me.tabConfig.Padding = New System.Windows.Forms.Padding(3)
        Me.tabConfig.Size = New System.Drawing.Size(650, 378)
        Me.tabConfig.TabIndex = 0
        Me.tabConfig.Text = "Configuration"
        Me.tabConfig.UseVisualStyleBackColor = True
        '
        'chkSaveInterimToFile
        '
        Me.chkSaveInterimToFile.AutoSize = True
        Me.chkSaveInterimToFile.Location = New System.Drawing.Point(8, 213)
        Me.chkSaveInterimToFile.Name = "chkSaveInterimToFile"
        Me.chkSaveInterimToFile.Size = New System.Drawing.Size(150, 17)
        Me.chkSaveInterimToFile.TabIndex = 47
        Me.chkSaveInterimToFile.Text = "Save Intermediate Results"
        Me.chkSaveInterimToFile.UseVisualStyleBackColor = True
        '
        'gbRnages
        '
        Me.gbRnages.Controls.Add(Me.btnBrowseRangeFile)
        Me.gbRnages.Controls.Add(Me.TextBox4)
        Me.gbRnages.Controls.Add(Me.TextBox3)
        Me.gbRnages.Controls.Add(Me.Label3)
        Me.gbRnages.Controls.Add(Me.Label2)
        Me.gbRnages.Controls.Add(Me.TextBox2)
        Me.gbRnages.Controls.Add(Me.Label1)
        Me.gbRnages.Location = New System.Drawing.Point(6, 236)
        Me.gbRnages.Name = "gbRnages"
        Me.gbRnages.Size = New System.Drawing.Size(582, 58)
        Me.gbRnages.TabIndex = 46
        Me.gbRnages.TabStop = False
        Me.gbRnages.Text = "Recession Data Display Parameters"
        Me.gbRnages.Visible = False
        '
        'btnBrowseRangeFile
        '
        Me.btnBrowseRangeFile.Location = New System.Drawing.Point(453, 29)
        Me.btnBrowseRangeFile.Name = "btnBrowseRangeFile"
        Me.btnBrowseRangeFile.Size = New System.Drawing.Size(117, 23)
        Me.btnBrowseRangeFile.TabIndex = 6
        Me.btnBrowseRangeFile.Text = "Browse for range file"
        Me.btnBrowseRangeFile.UseVisualStyleBackColor = True
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(300, 32)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(100, 20)
        Me.TextBox4.TabIndex = 5
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(156, 33)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(100, 20)
        Me.TextBox3.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(297, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Maximum Log(Flow)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(153, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Minimum Log(Flow)"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(9, 32)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(100, 20)
        Me.TextBox2.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Last Day"
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Location = New System.Drawing.Point(6, 187)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(581, 20)
        Me.txtOutputDir.TabIndex = 41
        '
        'lblOutpuDir
        '
        Me.lblOutpuDir.AutoSize = True
        Me.lblOutpuDir.Location = New System.Drawing.Point(6, 171)
        Me.lblOutpuDir.Name = "lblOutpuDir"
        Me.lblOutpuDir.Size = New System.Drawing.Size(118, 13)
        Me.lblOutpuDir.TabIndex = 45
        Me.lblOutpuDir.Text = "Specify output directory"
        '
        'txtMinRecessionDays
        '
        Me.txtMinRecessionDays.Location = New System.Drawing.Point(235, 141)
        Me.txtMinRecessionDays.Name = "txtMinRecessionDays"
        Me.txtMinRecessionDays.Size = New System.Drawing.Size(72, 20)
        Me.txtMinRecessionDays.TabIndex = 40
        '
        'lblRecessionDays
        '
        Me.lblRecessionDays.AutoSize = True
        Me.lblRecessionDays.Location = New System.Drawing.Point(6, 144)
        Me.lblRecessionDays.Name = "lblRecessionDays"
        Me.lblRecessionDays.Size = New System.Drawing.Size(223, 13)
        Me.lblRecessionDays.TabIndex = 44
        Me.lblRecessionDays.Text = "Specify minimum flow recession length in days"
        '
        'gbMonthSeason
        '
        Me.gbMonthSeason.Controls.Add(Me.rdoNoSeason)
        Me.gbMonthSeason.Controls.Add(Me.rdoWinter)
        Me.gbMonthSeason.Controls.Add(Me.rdoFall)
        Me.gbMonthSeason.Controls.Add(Me.rdoSummer)
        Me.gbMonthSeason.Controls.Add(Me.rdoSpring)
        Me.gbMonthSeason.Controls.Add(Me.lstMonths)
        Me.gbMonthSeason.Location = New System.Drawing.Point(313, 6)
        Me.gbMonthSeason.Name = "gbMonthSeason"
        Me.gbMonthSeason.Size = New System.Drawing.Size(275, 155)
        Me.gbMonthSeason.TabIndex = 43
        Me.gbMonthSeason.TabStop = False
        Me.gbMonthSeason.Text = "Months and Season"
        '
        'rdoNoSeason
        '
        Me.rdoNoSeason.AutoSize = True
        Me.rdoNoSeason.Location = New System.Drawing.Point(6, 131)
        Me.rdoNoSeason.Name = "rdoNoSeason"
        Me.rdoNoSeason.Size = New System.Drawing.Size(122, 17)
        Me.rdoNoSeason.TabIndex = 8
        Me.rdoNoSeason.TabStop = True
        Me.rdoNoSeason.Text = "No particular season"
        Me.rdoNoSeason.UseVisualStyleBackColor = True
        '
        'rdoWinter
        '
        Me.rdoWinter.AutoSize = True
        Me.rdoWinter.Location = New System.Drawing.Point(183, 108)
        Me.rdoWinter.Name = "rdoWinter"
        Me.rdoWinter.Size = New System.Drawing.Size(56, 17)
        Me.rdoWinter.TabIndex = 7
        Me.rdoWinter.TabStop = True
        Me.rdoWinter.Text = "Winter"
        Me.rdoWinter.UseVisualStyleBackColor = True
        '
        'rdoFall
        '
        Me.rdoFall.AutoSize = True
        Me.rdoFall.Location = New System.Drawing.Point(136, 108)
        Me.rdoFall.Name = "rdoFall"
        Me.rdoFall.Size = New System.Drawing.Size(41, 17)
        Me.rdoFall.TabIndex = 6
        Me.rdoFall.TabStop = True
        Me.rdoFall.Text = "Fall"
        Me.rdoFall.UseVisualStyleBackColor = True
        '
        'rdoSummer
        '
        Me.rdoSummer.AutoSize = True
        Me.rdoSummer.Location = New System.Drawing.Point(67, 108)
        Me.rdoSummer.Name = "rdoSummer"
        Me.rdoSummer.Size = New System.Drawing.Size(63, 17)
        Me.rdoSummer.TabIndex = 5
        Me.rdoSummer.TabStop = True
        Me.rdoSummer.Text = "Summer"
        Me.rdoSummer.UseVisualStyleBackColor = True
        '
        'rdoSpring
        '
        Me.rdoSpring.AutoSize = True
        Me.rdoSpring.Location = New System.Drawing.Point(6, 108)
        Me.rdoSpring.Name = "rdoSpring"
        Me.rdoSpring.Size = New System.Drawing.Size(55, 17)
        Me.rdoSpring.TabIndex = 4
        Me.rdoSpring.TabStop = True
        Me.rdoSpring.Text = "Spring"
        Me.rdoSpring.UseVisualStyleBackColor = True
        '
        'lstMonths
        '
        Me.lstMonths.FormattingEnabled = True
        Me.lstMonths.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.lstMonths.Location = New System.Drawing.Point(7, 20)
        Me.lstMonths.MultiColumn = True
        Me.lstMonths.Name = "lstMonths"
        Me.lstMonths.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstMonths.Size = New System.Drawing.Size(256, 82)
        Me.lstMonths.TabIndex = 3
        '
        'gbDates
        '
        Me.gbDates.Controls.Add(Me.txtEndDateUser)
        Me.gbDates.Controls.Add(Me.txtStartDateUser)
        Me.gbDates.Controls.Add(Me.btnExamineData)
        Me.gbDates.Controls.Add(Me.txtDataEnd)
        Me.gbDates.Controls.Add(Me.txtDataStart)
        Me.gbDates.Controls.Add(Me.lblDataEnd)
        Me.gbDates.Controls.Add(Me.lblDataStart)
        Me.gbDates.Location = New System.Drawing.Point(6, 6)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Size = New System.Drawing.Size(301, 109)
        Me.gbDates.TabIndex = 42
        Me.gbDates.TabStop = False
        Me.gbDates.Text = "Streamflow Analysis Dates"
        '
        'txtEndDateUser
        '
        Me.txtEndDateUser.Location = New System.Drawing.Point(162, 52)
        Me.txtEndDateUser.Name = "txtEndDateUser"
        Me.txtEndDateUser.Size = New System.Drawing.Size(88, 20)
        Me.txtEndDateUser.TabIndex = 1
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Location = New System.Drawing.Point(162, 26)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(88, 20)
        Me.txtStartDateUser.TabIndex = 0
        '
        'btnExamineData
        '
        Me.btnExamineData.Location = New System.Drawing.Point(156, 78)
        Me.btnExamineData.Name = "btnExamineData"
        Me.btnExamineData.Size = New System.Drawing.Size(94, 23)
        Me.btnExamineData.TabIndex = 2
        Me.btnExamineData.Text = "Examine Data"
        Me.btnExamineData.UseVisualStyleBackColor = True
        '
        'txtDataEnd
        '
        Me.txtDataEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataEnd.Location = New System.Drawing.Point(68, 53)
        Me.txtDataEnd.Name = "txtDataEnd"
        Me.txtDataEnd.ReadOnly = True
        Me.txtDataEnd.Size = New System.Drawing.Size(88, 20)
        Me.txtDataEnd.TabIndex = 3
        '
        'txtDataStart
        '
        Me.txtDataStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataStart.Location = New System.Drawing.Point(68, 26)
        Me.txtDataStart.Name = "txtDataStart"
        Me.txtDataStart.ReadOnly = True
        Me.txtDataStart.Size = New System.Drawing.Size(88, 20)
        Me.txtDataStart.TabIndex = 2
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(6, 52)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(52, 13)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Text = "Data End"
        '
        'lblDataStart
        '
        Me.lblDataStart.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(6, 26)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(55, 13)
        Me.lblDataStart.TabIndex = 0
        Me.lblDataStart.Text = "Data Start"
        '
        'tabAnalysis
        '
        Me.tabAnalysis.Controls.Add(Me.lstRecessSegments)
        Me.tabAnalysis.Controls.Add(Me.scDisplay)
        Me.tabAnalysis.Controls.Add(Me.gbToolBar)
        Me.tabAnalysis.Location = New System.Drawing.Point(4, 22)
        Me.tabAnalysis.Name = "tabAnalysis"
        Me.tabAnalysis.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAnalysis.Size = New System.Drawing.Size(650, 378)
        Me.tabAnalysis.TabIndex = 1
        Me.tabAnalysis.Text = "Recess Analysis"
        Me.tabAnalysis.UseVisualStyleBackColor = True
        '
        'lstRecessSegments
        '
        Me.lstRecessSegments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstRecessSegments.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstRecessSegments.FormattingEnabled = True
        Me.lstRecessSegments.Location = New System.Drawing.Point(7, 65)
        Me.lstRecessSegments.Name = "lstRecessSegments"
        Me.lstRecessSegments.Size = New System.Drawing.Size(100, 304)
        Me.lstRecessSegments.TabIndex = 40
        '
        'scDisplay
        '
        Me.scDisplay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scDisplay.Location = New System.Drawing.Point(113, 64)
        Me.scDisplay.Name = "scDisplay"
        '
        'scDisplay.Panel1
        '
        Me.scDisplay.Panel1.Controls.Add(Me.txtAnalysisResults)
        Me.scDisplay.Panel1.Controls.Add(Me.lstTable)
        Me.scDisplay.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'scDisplay.Panel2
        '
        Me.scDisplay.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.scDisplay.Size = New System.Drawing.Size(534, 303)
        Me.scDisplay.SplitterDistance = 250
        Me.scDisplay.TabIndex = 39
        '
        'txtAnalysisResults
        '
        Me.txtAnalysisResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAnalysisResults.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtAnalysisResults.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAnalysisResults.Location = New System.Drawing.Point(0, 0)
        Me.txtAnalysisResults.Multiline = True
        Me.txtAnalysisResults.Name = "txtAnalysisResults"
        Me.txtAnalysisResults.ReadOnly = True
        Me.txtAnalysisResults.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtAnalysisResults.Size = New System.Drawing.Size(250, 303)
        Me.txtAnalysisResults.TabIndex = 1
        Me.txtAnalysisResults.Visible = False
        Me.txtAnalysisResults.WordWrap = False
        '
        'lstTable
        '
        Me.lstTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstTable.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstTable.FormattingEnabled = True
        Me.lstTable.Location = New System.Drawing.Point(0, 0)
        Me.lstTable.Name = "lstTable"
        Me.lstTable.Size = New System.Drawing.Size(250, 303)
        Me.lstTable.TabIndex = 0
        '
        'gbToolBar
        '
        Me.gbToolBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbToolBar.Controls.Add(Me.btnSummary)
        Me.gbToolBar.Controls.Add(Me.btnAnalyse)
        Me.gbToolBar.Controls.Add(Me.btnGetAllSegments)
        Me.gbToolBar.Location = New System.Drawing.Point(6, 6)
        Me.gbToolBar.Name = "gbToolBar"
        Me.gbToolBar.Size = New System.Drawing.Size(648, 52)
        Me.gbToolBar.TabIndex = 37
        Me.gbToolBar.TabStop = False
        Me.gbToolBar.Text = "Recess Tool Bar"
        '
        'btnSummary
        '
        Me.btnSummary.Location = New System.Drawing.Point(507, 19)
        Me.btnSummary.Name = "btnSummary"
        Me.btnSummary.Size = New System.Drawing.Size(75, 23)
        Me.btnSummary.TabIndex = 17
        Me.btnSummary.Text = "Summary"
        Me.btnSummary.UseVisualStyleBackColor = True
        '
        'btnAnalyse
        '
        Me.btnAnalyse.Location = New System.Drawing.Point(426, 19)
        Me.btnAnalyse.Name = "btnAnalyse"
        Me.btnAnalyse.Size = New System.Drawing.Size(75, 23)
        Me.btnAnalyse.TabIndex = 16
        Me.btnAnalyse.Text = "Analyse"
        Me.btnAnalyse.UseVisualStyleBackColor = True
        '
        'btnGetAllSegments
        '
        Me.btnGetAllSegments.Location = New System.Drawing.Point(6, 19)
        Me.btnGetAllSegments.Name = "btnGetAllSegments"
        Me.btnGetAllSegments.Size = New System.Drawing.Size(72, 23)
        Me.btnGetAllSegments.TabIndex = 12
        Me.btnGetAllSegments.Text = "Find Peaks"
        Me.btnGetAllSegments.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.Window
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(658, 24)
        Me.MenuStrip1.TabIndex = 30
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileSelectData})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Name = "mnuFileSelectData"
        Me.mnuFileSelectData.Size = New System.Drawing.Size(132, 22)
        Me.mnuFileSelectData.Text = "Select Data"
        '
        'mnuHelp
        '
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "Help"
        '
        'frmRecess
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(658, 431)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.tabMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmRecess"
        Me.Text = "USGS RECESS"
        Me.tabMain.ResumeLayout(False)
        Me.tabConfig.ResumeLayout(False)
        Me.tabConfig.PerformLayout()
        Me.gbRnages.ResumeLayout(False)
        Me.gbRnages.PerformLayout()
        Me.gbMonthSeason.ResumeLayout(False)
        Me.gbMonthSeason.PerformLayout()
        Me.gbDates.ResumeLayout(False)
        Me.gbDates.PerformLayout()
        Me.tabAnalysis.ResumeLayout(False)
        Me.scDisplay.Panel1.ResumeLayout(False)
        Me.scDisplay.Panel1.PerformLayout()
        Me.scDisplay.ResumeLayout(False)
        Me.gbToolBar.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabConfig As System.Windows.Forms.TabPage
    Friend WithEvents tabAnalysis As System.Windows.Forms.TabPage
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gbRnages As System.Windows.Forms.GroupBox
    Friend WithEvents btnBrowseRangeFile As System.Windows.Forms.Button
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents lblOutpuDir As System.Windows.Forms.Label
    Friend WithEvents txtMinRecessionDays As System.Windows.Forms.TextBox
    Friend WithEvents lblRecessionDays As System.Windows.Forms.Label
    Friend WithEvents gbMonthSeason As System.Windows.Forms.GroupBox
    Friend WithEvents rdoNoSeason As System.Windows.Forms.RadioButton
    Friend WithEvents rdoWinter As System.Windows.Forms.RadioButton
    Friend WithEvents rdoFall As System.Windows.Forms.RadioButton
    Friend WithEvents rdoSummer As System.Windows.Forms.RadioButton
    Friend WithEvents rdoSpring As System.Windows.Forms.RadioButton
    Friend WithEvents lstMonths As System.Windows.Forms.ListBox
    Friend WithEvents gbDates As System.Windows.Forms.GroupBox
    Friend WithEvents txtEndDateUser As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDateUser As System.Windows.Forms.TextBox
    Friend WithEvents btnExamineData As System.Windows.Forms.Button
    Friend WithEvents txtDataEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtDataStart As System.Windows.Forms.TextBox
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents gbToolBar As System.Windows.Forms.GroupBox
    Friend WithEvents btnSummary As System.Windows.Forms.Button
    Friend WithEvents btnAnalyse As System.Windows.Forms.Button
    Friend WithEvents btnGetAllSegments As System.Windows.Forms.Button
    Friend WithEvents scDisplay As System.Windows.Forms.SplitContainer
    Friend WithEvents lstTable As System.Windows.Forms.ListBox
    Friend WithEvents lstRecessSegments As System.Windows.Forms.CheckedListBox
    Friend WithEvents txtAnalysisResults As System.Windows.Forms.TextBox
    Friend WithEvents chkSaveInterimToFile As System.Windows.Forms.CheckBox
End Class
