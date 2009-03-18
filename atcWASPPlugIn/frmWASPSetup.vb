Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcData
Imports System.Drawing
Imports System
Imports System.Windows.Forms
Imports atcWASP

Public Class frmWASPSetup
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
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdExisting As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdAbout As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents cboMet As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cboStreams As System.Windows.Forms.ComboBox
    Friend WithEvents tbxName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents ofdMetWDM As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lblEnd As System.Windows.Forms.Label
    Friend WithEvents lblStart As System.Windows.Forms.Label
    Friend WithEvents lblDay As System.Windows.Forms.Label
    Friend WithEvents lblMonth As System.Windows.Forms.Label
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents atxEDay As atcControls.atcText
    Friend WithEvents atxSDay As atcControls.atcText
    Friend WithEvents atxSYear As atcControls.atcText
    Friend WithEvents atxEMonth As atcControls.atcText
    Friend WithEvents atxSMonth As atcControls.atcText
    Friend WithEvents atxEYear As atcControls.atcText
    Friend WithEvents AtcGridFlow As atcControls.atcGrid
    Friend WithEvents cmdGenerate As System.Windows.Forms.Button
    Friend WithEvents AtcGridSegmentation As atcControls.atcGrid
    Friend WithEvents AtcGridLoad As atcControls.atcGrid
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents atxTravelTime As atcControls.atcText
    Friend WithEvents cmdFieldMapping As System.Windows.Forms.Button
    Friend WithEvents cmdCreateShapefile As System.Windows.Forms.Button
    Friend WithEvents cmdSelectConstituents As System.Windows.Forms.Button
    Friend WithEvents cmdGenerateTimeseries As System.Windows.Forms.Button
    Friend WithEvents cmdVolumes As System.Windows.Forms.Button
    Friend WithEvents cbxWind As System.Windows.Forms.ComboBox
    Friend WithEvents cbxSolar As System.Windows.Forms.ComboBox
    Friend WithEvents cbxAir As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblAir As System.Windows.Forms.Label
    Friend WithEvents ofdExisting As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWASPSetup))
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdExisting = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.ofdExisting = New System.Windows.Forms.OpenFileDialog
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.lblEnd = New System.Windows.Forms.Label
        Me.lblStart = New System.Windows.Forms.Label
        Me.lblDay = New System.Windows.Forms.Label
        Me.lblMonth = New System.Windows.Forms.Label
        Me.lblYear = New System.Windows.Forms.Label
        Me.atxEDay = New atcControls.atcText
        Me.atxSDay = New atcControls.atcText
        Me.atxSYear = New atcControls.atcText
        Me.atxEMonth = New atcControls.atcText
        Me.atxSMonth = New atcControls.atcText
        Me.atxEYear = New atcControls.atcText
        Me.cboMet = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cboStreams = New System.Windows.Forms.ComboBox
        Me.tbxName = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.cmdVolumes = New System.Windows.Forms.Button
        Me.cmdCreateShapefile = New System.Windows.Forms.Button
        Me.cmdFieldMapping = New System.Windows.Forms.Button
        Me.atxTravelTime = New atcControls.atcText
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdGenerate = New System.Windows.Forms.Button
        Me.AtcGridSegmentation = New atcControls.atcGrid
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.cmdGenerateTimeseries = New System.Windows.Forms.Button
        Me.AtcGridFlow = New atcControls.atcGrid
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.cmdSelectConstituents = New System.Windows.Forms.Button
        Me.AtcGridLoad = New atcControls.atcGrid
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.ofdMetWDM = New System.Windows.Forms.OpenFileDialog
        Me.lblAir = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.cbxAir = New System.Windows.Forms.ComboBox
        Me.cbxSolar = New System.Windows.Forms.ComboBox
        Me.cbxWind = New System.Windows.Forms.ComboBox
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(16, 523)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 32)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "OK"
        '
        'cmdExisting
        '
        Me.cmdExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdExisting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExisting.Location = New System.Drawing.Point(96, 523)
        Me.cmdExisting.Name = "cmdExisting"
        Me.cmdExisting.Size = New System.Drawing.Size(120, 32)
        Me.cmdExisting.TabIndex = 4
        Me.cmdExisting.Text = "Open Existing"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(224, 523)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(88, 32)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Cancel"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(752, 523)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(79, 32)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(840, 523)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(87, 32)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "About"
        '
        'ofdExisting
        '
        Me.ofdExisting.DefaultExt = "inp"
        Me.ofdExisting.Filter = "WASP INP files (*.inp)|*.inp"
        Me.ofdExisting.InitialDirectory = "/BASINS/modelout/"
        Me.ofdExisting.Title = "Select WASP inp file"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(60, 21)
        Me.TabControl1.Location = New System.Drawing.Point(18, 17)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(907, 427)
        Me.TabControl1.TabIndex = 8
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.cboMet)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.cboStreams)
        Me.TabPage1.Controls.Add(Me.tbxName)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(899, 398)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "General"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.lblEnd)
        Me.GroupBox3.Controls.Add(Me.lblStart)
        Me.GroupBox3.Controls.Add(Me.lblDay)
        Me.GroupBox3.Controls.Add(Me.lblMonth)
        Me.GroupBox3.Controls.Add(Me.lblYear)
        Me.GroupBox3.Controls.Add(Me.atxEDay)
        Me.GroupBox3.Controls.Add(Me.atxSDay)
        Me.GroupBox3.Controls.Add(Me.atxSYear)
        Me.GroupBox3.Controls.Add(Me.atxEMonth)
        Me.GroupBox3.Controls.Add(Me.atxSMonth)
        Me.GroupBox3.Controls.Add(Me.atxEYear)
        Me.GroupBox3.Location = New System.Drawing.Point(14, 270)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(872, 111)
        Me.GroupBox3.TabIndex = 28
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Simulation Dates"
        '
        'lblEnd
        '
        Me.lblEnd.AutoSize = True
        Me.lblEnd.Location = New System.Drawing.Point(105, 67)
        Me.lblEnd.Name = "lblEnd"
        Me.lblEnd.Size = New System.Drawing.Size(33, 17)
        Me.lblEnd.TabIndex = 37
        Me.lblEnd.Text = "End"
        '
        'lblStart
        '
        Me.lblStart.AutoSize = True
        Me.lblStart.Location = New System.Drawing.Point(100, 37)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(38, 17)
        Me.lblStart.TabIndex = 36
        Me.lblStart.Text = "Start"
        '
        'lblDay
        '
        Me.lblDay.AutoSize = True
        Me.lblDay.Location = New System.Drawing.Point(291, 16)
        Me.lblDay.Name = "lblDay"
        Me.lblDay.Size = New System.Drawing.Size(33, 17)
        Me.lblDay.TabIndex = 35
        Me.lblDay.Text = "Day"
        '
        'lblMonth
        '
        Me.lblMonth.AutoSize = True
        Me.lblMonth.Location = New System.Drawing.Point(232, 16)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.Size = New System.Drawing.Size(47, 17)
        Me.lblMonth.TabIndex = 34
        Me.lblMonth.Text = "Month"
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Location = New System.Drawing.Point(150, 16)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(38, 17)
        Me.lblYear.TabIndex = 33
        Me.lblYear.Text = "Year"
        '
        'atxEDay
        '
        Me.atxEDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxEDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxEDay.DefaultValue = ""
        Me.atxEDay.HardMax = 31
        Me.atxEDay.HardMin = 1
        Me.atxEDay.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxEDay.Location = New System.Drawing.Point(294, 67)
        Me.atxEDay.MaxWidth = 20
        Me.atxEDay.Name = "atxEDay"
        Me.atxEDay.NumericFormat = "0"
        Me.atxEDay.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxEDay.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxEDay.SelLength = 2
        Me.atxEDay.SelStart = 0
        Me.atxEDay.Size = New System.Drawing.Size(53, 24)
        Me.atxEDay.SoftMax = -999
        Me.atxEDay.SoftMin = -999
        Me.atxEDay.TabIndex = 32
        Me.atxEDay.ValueDouble = 31
        Me.atxEDay.ValueInteger = 31
        '
        'atxSDay
        '
        Me.atxSDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSDay.DefaultValue = ""
        Me.atxSDay.HardMax = 31
        Me.atxSDay.HardMin = 1
        Me.atxSDay.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSDay.Location = New System.Drawing.Point(294, 37)
        Me.atxSDay.MaxWidth = 20
        Me.atxSDay.Name = "atxSDay"
        Me.atxSDay.NumericFormat = "0"
        Me.atxSDay.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSDay.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSDay.SelLength = 1
        Me.atxSDay.SelStart = 0
        Me.atxSDay.Size = New System.Drawing.Size(53, 24)
        Me.atxSDay.SoftMax = -999
        Me.atxSDay.SoftMin = -999
        Me.atxSDay.TabIndex = 31
        Me.atxSDay.ValueDouble = 1
        Me.atxSDay.ValueInteger = 1
        '
        'atxSYear
        '
        Me.atxSYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSYear.DefaultValue = ""
        Me.atxSYear.HardMax = 9999
        Me.atxSYear.HardMin = 0
        Me.atxSYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSYear.Location = New System.Drawing.Point(153, 37)
        Me.atxSYear.MaxWidth = 20
        Me.atxSYear.Name = "atxSYear"
        Me.atxSYear.NumericFormat = "0"
        Me.atxSYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSYear.SelLength = 4
        Me.atxSYear.SelStart = 0
        Me.atxSYear.Size = New System.Drawing.Size(76, 24)
        Me.atxSYear.SoftMax = -999
        Me.atxSYear.SoftMin = -999
        Me.atxSYear.TabIndex = 30
        Me.atxSYear.ValueDouble = 2000
        Me.atxSYear.ValueInteger = 2000
        '
        'atxEMonth
        '
        Me.atxEMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxEMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxEMonth.DefaultValue = ""
        Me.atxEMonth.HardMax = 12
        Me.atxEMonth.HardMin = 1
        Me.atxEMonth.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxEMonth.Location = New System.Drawing.Point(235, 67)
        Me.atxEMonth.MaxWidth = 20
        Me.atxEMonth.Name = "atxEMonth"
        Me.atxEMonth.NumericFormat = "0"
        Me.atxEMonth.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxEMonth.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxEMonth.SelLength = 2
        Me.atxEMonth.SelStart = 0
        Me.atxEMonth.Size = New System.Drawing.Size(53, 24)
        Me.atxEMonth.SoftMax = -999
        Me.atxEMonth.SoftMin = -999
        Me.atxEMonth.TabIndex = 29
        Me.atxEMonth.ValueDouble = 12
        Me.atxEMonth.ValueInteger = 12
        '
        'atxSMonth
        '
        Me.atxSMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSMonth.DefaultValue = ""
        Me.atxSMonth.HardMax = 12
        Me.atxSMonth.HardMin = 1
        Me.atxSMonth.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSMonth.Location = New System.Drawing.Point(235, 37)
        Me.atxSMonth.MaxWidth = 20
        Me.atxSMonth.Name = "atxSMonth"
        Me.atxSMonth.NumericFormat = "0"
        Me.atxSMonth.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSMonth.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSMonth.SelLength = 1
        Me.atxSMonth.SelStart = 0
        Me.atxSMonth.Size = New System.Drawing.Size(53, 24)
        Me.atxSMonth.SoftMax = -999
        Me.atxSMonth.SoftMin = -999
        Me.atxSMonth.TabIndex = 28
        Me.atxSMonth.ValueDouble = 1
        Me.atxSMonth.ValueInteger = 1
        '
        'atxEYear
        '
        Me.atxEYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxEYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxEYear.DefaultValue = ""
        Me.atxEYear.HardMax = 9999
        Me.atxEYear.HardMin = 0
        Me.atxEYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxEYear.Location = New System.Drawing.Point(153, 67)
        Me.atxEYear.MaxWidth = 20
        Me.atxEYear.Name = "atxEYear"
        Me.atxEYear.NumericFormat = "0"
        Me.atxEYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxEYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxEYear.SelLength = 4
        Me.atxEYear.SelStart = 0
        Me.atxEYear.Size = New System.Drawing.Size(76, 24)
        Me.atxEYear.SoftMax = -999
        Me.atxEYear.SoftMin = -999
        Me.atxEYear.TabIndex = 27
        Me.atxEYear.ValueDouble = 2000
        Me.atxEYear.ValueInteger = 2000
        '
        'cboMet
        '
        Me.cboMet.AllowDrop = True
        Me.cboMet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMet.Location = New System.Drawing.Point(168, 127)
        Me.cboMet.Name = "cboMet"
        Me.cboMet.Size = New System.Drawing.Size(719, 25)
        Me.cboMet.TabIndex = 12
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(11, 130)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(130, 17)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "Met Stations Layer:"
        '
        'cboStreams
        '
        Me.cboStreams.AllowDrop = True
        Me.cboStreams.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStreams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStreams.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStreams.Location = New System.Drawing.Point(168, 87)
        Me.cboStreams.Name = "cboStreams"
        Me.cboStreams.Size = New System.Drawing.Size(719, 25)
        Me.cboStreams.TabIndex = 9
        '
        'tbxName
        '
        Me.tbxName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbxName.Location = New System.Drawing.Point(168, 40)
        Me.tbxName.Name = "tbxName"
        Me.tbxName.Size = New System.Drawing.Size(553, 23)
        Me.tbxName.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(11, 91)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(104, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Streams Layer:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(141, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "WASP Project Name:"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.cmdVolumes)
        Me.TabPage2.Controls.Add(Me.cmdCreateShapefile)
        Me.TabPage2.Controls.Add(Me.cmdFieldMapping)
        Me.TabPage2.Controls.Add(Me.atxTravelTime)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.cmdGenerate)
        Me.TabPage2.Controls.Add(Me.AtcGridSegmentation)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(899, 398)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Segmentation"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'cmdVolumes
        '
        Me.cmdVolumes.Location = New System.Drawing.Point(618, 16)
        Me.cmdVolumes.Name = "cmdVolumes"
        Me.cmdVolumes.Size = New System.Drawing.Size(137, 25)
        Me.cmdVolumes.TabIndex = 25
        Me.cmdVolumes.Text = "Volumes/Depths"
        Me.cmdVolumes.UseVisualStyleBackColor = True
        '
        'cmdCreateShapefile
        '
        Me.cmdCreateShapefile.Location = New System.Drawing.Point(466, 47)
        Me.cmdCreateShapefile.Name = "cmdCreateShapefile"
        Me.cmdCreateShapefile.Size = New System.Drawing.Size(137, 25)
        Me.cmdCreateShapefile.TabIndex = 8
        Me.cmdCreateShapefile.Text = "Create Shapefile"
        Me.cmdCreateShapefile.UseVisualStyleBackColor = True
        '
        'cmdFieldMapping
        '
        Me.cmdFieldMapping.Location = New System.Drawing.Point(466, 16)
        Me.cmdFieldMapping.Name = "cmdFieldMapping"
        Me.cmdFieldMapping.Size = New System.Drawing.Size(137, 25)
        Me.cmdFieldMapping.TabIndex = 7
        Me.cmdFieldMapping.Text = "Field Mapping"
        Me.cmdFieldMapping.UseVisualStyleBackColor = True
        '
        'atxTravelTime
        '
        Me.atxTravelTime.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxTravelTime.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxTravelTime.DefaultValue = ""
        Me.atxTravelTime.HardMax = -999
        Me.atxTravelTime.HardMin = 0
        Me.atxTravelTime.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxTravelTime.Location = New System.Drawing.Point(215, 16)
        Me.atxTravelTime.MaxWidth = 20
        Me.atxTravelTime.Name = "atxTravelTime"
        Me.atxTravelTime.NumericFormat = "0.#####"
        Me.atxTravelTime.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxTravelTime.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxTravelTime.SelLength = 1
        Me.atxTravelTime.SelStart = 0
        Me.atxTravelTime.Size = New System.Drawing.Size(48, 24)
        Me.atxTravelTime.SoftMax = -999
        Me.atxTravelTime.SoftMin = -999
        Me.atxTravelTime.TabIndex = 6
        Me.atxTravelTime.ValueDouble = 0
        Me.atxTravelTime.ValueInteger = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(189, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Maximum Travel Time (days)"
        '
        'cmdGenerate
        '
        Me.cmdGenerate.Location = New System.Drawing.Point(271, 16)
        Me.cmdGenerate.Name = "cmdGenerate"
        Me.cmdGenerate.Size = New System.Drawing.Size(126, 25)
        Me.cmdGenerate.TabIndex = 1
        Me.cmdGenerate.Text = "Regenerate"
        Me.cmdGenerate.UseVisualStyleBackColor = True
        '
        'AtcGridSegmentation
        '
        Me.AtcGridSegmentation.AllowHorizontalScrolling = True
        Me.AtcGridSegmentation.AllowNewValidValues = False
        Me.AtcGridSegmentation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridSegmentation.CellBackColor = System.Drawing.SystemColors.Window
        Me.AtcGridSegmentation.Fixed3D = False
        Me.AtcGridSegmentation.LineColor = System.Drawing.SystemColors.Control
        Me.AtcGridSegmentation.LineWidth = 1.0!
        Me.AtcGridSegmentation.Location = New System.Drawing.Point(23, 74)
        Me.AtcGridSegmentation.Name = "AtcGridSegmentation"
        Me.AtcGridSegmentation.Size = New System.Drawing.Size(854, 307)
        Me.AtcGridSegmentation.Source = Nothing
        Me.AtcGridSegmentation.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.cmdGenerateTimeseries)
        Me.TabPage3.Controls.Add(Me.AtcGridFlow)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(899, 398)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Flows"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'cmdGenerateTimeseries
        '
        Me.cmdGenerateTimeseries.Location = New System.Drawing.Point(23, 22)
        Me.cmdGenerateTimeseries.Name = "cmdGenerateTimeseries"
        Me.cmdGenerateTimeseries.Size = New System.Drawing.Size(155, 46)
        Me.cmdGenerateTimeseries.TabIndex = 23
        Me.cmdGenerateTimeseries.Text = "Generate New Timeseries"
        Me.cmdGenerateTimeseries.UseVisualStyleBackColor = True
        '
        'AtcGridFlow
        '
        Me.AtcGridFlow.AllowHorizontalScrolling = True
        Me.AtcGridFlow.AllowNewValidValues = False
        Me.AtcGridFlow.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridFlow.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridFlow.Fixed3D = False
        Me.AtcGridFlow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridFlow.LineColor = System.Drawing.Color.Empty
        Me.AtcGridFlow.LineWidth = 0.0!
        Me.AtcGridFlow.Location = New System.Drawing.Point(23, 74)
        Me.AtcGridFlow.Name = "AtcGridFlow"
        Me.AtcGridFlow.Size = New System.Drawing.Size(854, 307)
        Me.AtcGridFlow.Source = Nothing
        Me.AtcGridFlow.TabIndex = 20
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.cmdSelectConstituents)
        Me.TabPage4.Controls.Add(Me.AtcGridLoad)
        Me.TabPage4.Location = New System.Drawing.Point(4, 25)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(899, 398)
        Me.TabPage4.TabIndex = 6
        Me.TabPage4.Text = "Boundaries/Loads"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'cmdSelectConstituents
        '
        Me.cmdSelectConstituents.Location = New System.Drawing.Point(23, 21)
        Me.cmdSelectConstituents.Name = "cmdSelectConstituents"
        Me.cmdSelectConstituents.Size = New System.Drawing.Size(155, 47)
        Me.cmdSelectConstituents.TabIndex = 22
        Me.cmdSelectConstituents.Text = "Select Constituents"
        Me.cmdSelectConstituents.UseVisualStyleBackColor = True
        '
        'AtcGridLoad
        '
        Me.AtcGridLoad.AllowHorizontalScrolling = True
        Me.AtcGridLoad.AllowNewValidValues = False
        Me.AtcGridLoad.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridLoad.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridLoad.Fixed3D = False
        Me.AtcGridLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridLoad.LineColor = System.Drawing.Color.Empty
        Me.AtcGridLoad.LineWidth = 0.0!
        Me.AtcGridLoad.Location = New System.Drawing.Point(23, 74)
        Me.AtcGridLoad.Name = "AtcGridLoad"
        Me.AtcGridLoad.Size = New System.Drawing.Size(854, 307)
        Me.AtcGridLoad.Source = Nothing
        Me.AtcGridLoad.TabIndex = 21
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.cbxWind)
        Me.TabPage6.Controls.Add(Me.cbxSolar)
        Me.TabPage6.Controls.Add(Me.cbxAir)
        Me.TabPage6.Controls.Add(Me.Label5)
        Me.TabPage6.Controls.Add(Me.Label3)
        Me.TabPage6.Controls.Add(Me.lblAir)
        Me.TabPage6.Location = New System.Drawing.Point(4, 25)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(899, 398)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Meteorologic Time Series"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblStatus)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(18, 451)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(909, 55)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Status"
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(16, 24)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(878, 16)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Update specifications if desired, then click OK to proceed."
        '
        'ofdMetWDM
        '
        Me.ofdMetWDM.DefaultExt = "wdm"
        Me.ofdMetWDM.Filter = "Met WDM files (*.wdm)|*.wdm"
        Me.ofdMetWDM.InitialDirectory = "/BASINS/data/"
        Me.ofdMetWDM.Title = "Select Met WDM File"
        '
        'lblAir
        '
        Me.lblAir.AutoSize = True
        Me.lblAir.Location = New System.Drawing.Point(32, 33)
        Me.lblAir.Name = "lblAir"
        Me.lblAir.Size = New System.Drawing.Size(111, 17)
        Me.lblAir.TabIndex = 23
        Me.lblAir.Text = "Air Temperature"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(32, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(105, 17)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Solar Radiation"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(32, 95)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 17)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Wind Speed"
        '
        'cbxAir
        '
        Me.cbxAir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxAir.FormattingEnabled = True
        Me.cbxAir.Location = New System.Drawing.Point(164, 30)
        Me.cbxAir.Name = "cbxAir"
        Me.cbxAir.Size = New System.Drawing.Size(707, 25)
        Me.cbxAir.TabIndex = 26
        '
        'cbxSolar
        '
        Me.cbxSolar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxSolar.FormattingEnabled = True
        Me.cbxSolar.Location = New System.Drawing.Point(164, 61)
        Me.cbxSolar.Name = "cbxSolar"
        Me.cbxSolar.Size = New System.Drawing.Size(707, 25)
        Me.cbxSolar.TabIndex = 27
        '
        'cbxWind
        '
        Me.cbxWind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxWind.FormattingEnabled = True
        Me.cbxWind.Location = New System.Drawing.Point(164, 92)
        Me.cbxWind.Name = "cbxWind"
        Me.cbxWind.Size = New System.Drawing.Size(707, 25)
        Me.cbxWind.TabIndex = 28
        '
        'frmWASPSetup
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(944, 568)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdExisting)
        Me.Controls.Add(Me.cmdOK)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmWASPSetup"
        Me.Text = "BASINS WASP"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend pSegmentFieldMap As New atcCollection
    Friend pPlugIn As PlugIn
    Friend pBasinsFolder As String
    Friend pfrmWASPFieldMapping As frmWASPFieldMapping

    Friend pFlowStationCandidates As WASPTimeseriesCollection
    Friend pAirTempStationCandidates As WASPTimeseriesCollection
    Friend pSolRadStationCandidates As WASPTimeseriesCollection
    Friend pWindStationCandidates As WASPTimeseriesCollection
    Friend pWaterTempStationCandidates As WASPTimeseriesCollection

    Private pInitializing As Boolean = True
    Private pSelectedRow As Integer
    Private pSelectedColumn As Integer

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("BASINS WASP for MapWindow" & vbCrLf & vbCrLf & "Version 1.0", MsgBoxStyle.OkOnly, "BASINS WASP")
    End Sub

    Private Sub cmdExisting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExisting.Click
        If ofdExisting.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Logger.Dbg("Run WASP with " & ofdExisting.FileName)
            pPlugIn.WASPProject.Run(ofdExisting.FileName)
        End If
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\WASP.html")
    End Sub

    Private Sub frmWASPSetup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\WASP.html")
        End If
    End Sub

    Friend Sub EnableControls(ByVal aEnabled As Boolean)
        cmdOK.Enabled = aEnabled
        cmdExisting.Enabled = aEnabled
        cmdCancel.Enabled = aEnabled
        'If Not pInitializing Then
        cmdHelp.Enabled = aEnabled
        cmdAbout.Enabled = aEnabled
        'End If
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Logger.Dbg("Setup WASP input files")

        lblStatus.Text = "Preparing to process"
        Me.Refresh()
        EnableControls(False)

        'put contents of segment class back into structure
        With AtcGridSegmentation.Source
            For lIndex As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                pPlugIn.WASPProject.Segments(lIndex - 1).Length = .CellValue(lIndex, 2)
                pPlugIn.WASPProject.Segments(lIndex - 1).Width = .CellValue(lIndex, 3)
                pPlugIn.WASPProject.Segments(lIndex - 1).Depth = .CellValue(lIndex, 4)
                pPlugIn.WASPProject.Segments(lIndex - 1).Slope = .CellValue(lIndex, 5)
                pPlugIn.WASPProject.Segments(lIndex - 1).Roughness = .CellValue(lIndex, 6)
                pPlugIn.WASPProject.Segments(lIndex - 1).DownID = .CellValue(lIndex, 7)
            Next
        End With

        RebuildTimeseriesCollections()

        Dim lName As String = tbxName.Text
        'TODO: still use modelout?
        Dim lWASPProjectFileName As String = pBasinsFolder & "\modelout\" & lName & "\" & lName & ".wnf"
        MkDirPath(PathNameOnly(lWASPProjectFileName))

        If PreProcessChecking(lWASPProjectFileName) Then
            With pPlugIn.WASPProject
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                .Name = lName

                'save project file and start WASP
                Logger.Dbg("Save WASP network import file" & lWASPProjectFileName)
                .Save(lWASPProjectFileName)
                Logger.Dbg("Run WASP")
                .Run(lWASPProjectFileName)
                Logger.Dbg("BackFromWASP")
            End With
            lblStatus.Text = ""
            Me.Refresh()
            Me.Dispose()
            Me.Close()
            Logger.Dbg("Done")
        Else
            Logger.Dbg("Failed PreProcess Check")
        End If
        Logger.Flush()
    End Sub

    Friend Sub RebuildTimeseriesCollections()
        'clear out collections of timeseries prior to rebuilding
        pPlugIn.WASPProject.InputTimeseriesCollection.Clear()
        For lIndex As Integer = 1 To pPlugIn.WASPProject.Segments.Count
            pPlugIn.WASPProject.Segments(lIndex - 1).InputTimeseriesCollection.Clear()
        Next

        'build collections of timeseries 
        Dim lKeyString As String = ""
        For lIndex As Integer = 1 To pPlugIn.WASPProject.Segments.Count
            'input flows 
            lKeyString = "FLOW:" & AtcGridFlow.Source.CellValue(lIndex, 1)
            If AtcGridFlow.Source.CellValue(lIndex, 1) <> "<none>" Then
                AddSelectedTimeseriesToWASPSegment(lKeyString, pFlowStationCandidates, pPlugIn.WASPProject, pPlugIn.WASPProject.Segments(lIndex - 1))
            End If
            'need to add other wq loads
            lKeyString = "WTMP:" & AtcGridLoad.Source.CellValue(lIndex, 1)
            If AtcGridLoad.Source.CellValue(lIndex, 1) <> "<none>" Then
                AddSelectedTimeseriesToWASPSegment(lKeyString, pWaterTempStationCandidates, pPlugIn.WASPProject, pPlugIn.WASPProject.Segments(lIndex - 1))
            End If
        Next
        'met timeseries are not segment-specific
        'air temp
        If cbxAir.SelectedItem <> "<none>" Then
            lKeyString = "ATMP:" & cbxAir.SelectedItem
            AddSelectedTimeseriesToWASPProject(lKeyString, pAirTempStationCandidates, pPlugIn.WASPProject)
            lKeyString = "ATEM:" & cbxAir.SelectedItem
            AddSelectedTimeseriesToWASPProject(lKeyString, pAirTempStationCandidates, pPlugIn.WASPProject)
        End If
        'sol rad
        If cbxSolar.SelectedItem <> "<none>" Then
            lKeyString = "SOLR:" & cbxSolar.SelectedItem
            AddSelectedTimeseriesToWASPProject(lKeyString, pSolRadStationCandidates, pPlugIn.WASPProject)
            lKeyString = "SOLRAD:" & cbxSolar.SelectedItem
            AddSelectedTimeseriesToWASPProject(lKeyString, pSolRadStationCandidates, pPlugIn.WASPProject)
        End If
        'wind 
        If cbxWind.SelectedItem <> "<none>" Then
            lKeyString = "WIND:" & cbxWind.SelectedItem
            AddSelectedTimeseriesToWASPProject(lKeyString, pWindStationCandidates, pPlugIn.WASPProject)
        End If
    End Sub

    Private Function PreProcessChecking(ByVal aOutputFileName As String) As Boolean
        Logger.Dbg("PreprocessChecking " & aOutputFileName)

        'see if this file already exists
        If FileExists(aOutputFileName) Then  'already exists
            If Logger.Msg("WASP Project '" & FilenameNoPath(aOutputFileName) & "' already exists.  Do you want to overwrite it?", vbOKCancel, "Overwrite?") = MsgBoxResult.Cancel Then
                EnableControls(True)
                Return False
            End If
        End If

        Logger.Dbg("PreprocessChecking OK")
        Return True
    End Function

    Public Sub InitializeUI(ByVal aPlugIn As PlugIn)
        Logger.Dbg("InitializeUI")
        EnableControls(False)
        pPlugIn = aPlugIn
        pBasinsFolder = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        pFlowStationCandidates = New WASPTimeseriesCollection
        pAirTempStationCandidates = New WASPTimeseriesCollection
        pSolRadStationCandidates = New WASPTimeseriesCollection
        pWindStationCandidates = New WASPTimeseriesCollection
        pWaterTempStationCandidates = New WASPTimeseriesCollection

        'set field mapping for segments
        pSegmentFieldMap.Clear()
        pSegmentFieldMap.Add("GNIS_NAME", "Name")
        pSegmentFieldMap.Add("COMID", "ID")
        pSegmentFieldMap.Add("LINKNO", "ID")
        pSegmentFieldMap.Add("LENGTHKM", "Length")
        pSegmentFieldMap.Add("MeanWidth", "Width")
        pSegmentFieldMap.Add("DSCOMID", "DownID")
        pSegmentFieldMap.Add("DSLINKNO", "DownID")
        pSegmentFieldMap.Add("TOCOMID", "DownID")
        pSegmentFieldMap.Add("MAVELU", "Velocity")
        pSegmentFieldMap.Add("MAFLOWU", "MeanAnnualFlow")
        pSegmentFieldMap.Add("SLOPE", "Slope")
        pSegmentFieldMap.Add("CUMDRAINAG", "CumulativeDrainageArea")

        cboMet.Items.Add("<none>")

        With AtcGridSegmentation
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With

        With AtcGridFlow
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With

        With AtcGridLoad
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With

        For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 3 Then 'PolygonShapefile 

            ElseIf GisUtil.LayerType(lLayerIndex) = 2 Then 'LineShapefile 
                cboStreams.Items.Add(lLayerName)
                'see if there are any selected features in this layer, if so assume this is the stream segment layer
                If GisUtil.NumSelectedFeatures(lLayerIndex) > 0 Then
                    cboStreams.SelectedIndex = cboStreams.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = 1 Then 'PointShapefile
                cboMet.Items.Add(lLayerName)
                If lLayerName.ToUpper.IndexOf("WEATHER STATION SITES 20") > -1 Then
                    'this takes some time, show window and then do this
                    'cboMet.SelectedIndex = cboMet.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = 4 Then 'Grid

            End If
        Next

        'if no stream layer selected and there is an nhd layer on the map, make it selected
        If cboStreams.SelectedIndex < 0 Then
            For lIndex As Integer = 1 To cboStreams.Items.Count
                Dim lLayerName As String = cboStreams.Items(lIndex - 1).ToString
                If lLayerName.ToUpper.IndexOf("FLOWLINE") > -1 Then
                    cboStreams.SelectedIndex = lIndex - 1
                End If
            Next
        End If
        If cboStreams.SelectedIndex < 0 Then
            For lIndex As Integer = 1 To cboStreams.Items.Count
                Dim lLayerName As String = cboStreams.Items(lIndex - 1).ToString
                If lLayerName.ToUpper.IndexOf("NHD") > -1 Then
                    cboStreams.SelectedIndex = lIndex - 1
                End If
            Next
        End If

        If cboStreams.Items.Count > 0 And cboStreams.SelectedIndex < 0 Then
            cboStreams.SelectedIndex = 0
        End If
        If cboMet.Items.Count > 0 And cboMet.SelectedIndex < 0 Then
            cboMet.SelectedIndex = 0
        End If

        For lLayerIndex As Integer = 0 To cboMet.Items.Count - 1
            Dim lLayerName As String = cboMet.Items(lLayerIndex)
            If lLayerName.IndexOf("Weather Station Sites 20") > -1 Then
                cboMet.SelectedIndex = lLayerIndex
            End If
        Next

        tbxName.Text = IO.Path.GetFileNameWithoutExtension(GisUtil.ProjectFileName)

        AtcGridSegmentation.Clear()
        With AtcGridSegmentation.Source
            .Columns = 9
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .CellColor(0, 2) = SystemColors.ControlDark
            .CellColor(0, 3) = SystemColors.ControlDark
            .CellColor(0, 4) = SystemColors.ControlDark
            .CellColor(0, 5) = SystemColors.ControlDark
            .CellColor(0, 6) = SystemColors.ControlDark
            .CellColor(0, 7) = SystemColors.ControlDark
            .CellColor(0, 8) = SystemColors.ControlDark
            .CellColor(0, 9) = SystemColors.ControlDark
            .Rows = 1 + pPlugIn.WASPProject.Segments.Count
            .CellValue(0, 0) = "Segment"
            .CellValue(0, 1) = "WASP ID"
            .CellValue(0, 2) = "Length (km)"
            .CellValue(0, 3) = "Width (m)"
            .CellValue(0, 4) = "Depth (m)"
            .CellValue(0, 5) = "Slope"
            .CellValue(0, 6) = "Roughness"
            .CellValue(0, 7) = "DownStream ID"
            .CellValue(0, 8) = "Velocity (m/s)"
            .CellValue(0, 9) = "Travel Time (days)"
        End With

        AtcGridFlow.Clear()
        With AtcGridFlow.Source
            .Columns = 3
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 2
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .Rows = 1 + pPlugIn.WASPProject.Segments.Count
            .CellValue(0, 0) = "Segment"
            .CellValue(0, 1) = "Cum. Drainage Area (km^2)"
            .CellValue(0, 2) = "Input Flow Timeseries"
        End With

        AtcGridLoad.Clear()
        With AtcGridLoad.Source
            .Columns = 2
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .Rows = 1 + pPlugIn.WASPProject.Segments.Count
            .CellValue(0, 0) = "Segment"
            .CellValue(0, 1) = "Water Temp Timeseries"
        End With

        pInitializing = True
        GenerateSegments()
        pInitializing = False
        Logger.Dbg("InitializeUI Complete")
    End Sub

    Friend Sub InitializeStationLists()
        'this takes some time, show window and then do this
        Logger.Dbg("Initializing StationLists")
        lblStatus.Text = "Reading Timeseries Data..."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        EnableControls(False)

        BuildListofValidStationNames("FLOW", pFlowStationCandidates)
        BuildListofValidStationNames("WTMP", pWaterTempStationCandidates)
        BuildListofValidStationNames("ATMP", pAirTempStationCandidates)
        BuildListofValidStationNames("ATEM", pAirTempStationCandidates)
        BuildListofValidStationNames("SOLRAD", pSolRadStationCandidates)
        BuildListofValidStationNames("SOLR", pSolRadStationCandidates)
        BuildListofValidStationNames("WIND", pWindStationCandidates)

        'set layer index for met stations
        Dim lMetLayerIndex As Integer = GisUtil.LayerIndex(cboMet.Items(cboMet.SelectedIndex))
        GetMetStationCoordinates(lMetLayerIndex, pAirTempStationCandidates)
        GetMetStationCoordinates(lMetLayerIndex, pAirTempStationCandidates)
        GetMetStationCoordinates(lMetLayerIndex, pSolRadStationCandidates)
        GetMetStationCoordinates(lMetLayerIndex, pSolRadStationCandidates)
        GetMetStationCoordinates(lMetLayerIndex, pWindStationCandidates)

        'redo to set valid values
        SetFlowStationGrid()
        SetLoadStationGrid()
        SetMetStationValidValues()

        lblStatus.Text = "Update specifications if desired, then click OK to proceed."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        EnableControls(True)
    End Sub

    Private Sub lblStatus_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStatus.TextChanged
        Logger.Dbg(lblStatus.Text)
    End Sub

    Private Sub cmdGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGenerate.Click
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        GenerateSegments()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub GenerateSegments()
        'set file names for segments
        Dim lSegmentLayerIndex As Integer = GisUtil.LayerIndex(cboStreams.Items(cboStreams.SelectedIndex))
        Dim lSegmentShapefileName As String = GisUtil.LayerFileName(lSegmentLayerIndex)

        'populate the SWMM classes from the shapefiles
        With pPlugIn.WASPProject
            .Segments.Clear()
            Dim lTable As New atcUtility.atcTableDBF

            'add only selected segments
            Dim lTempSegments As New atcWASP.Segments
            If lTable.OpenFile(FilenameSetExt(lSegmentShapefileName, "dbf")) Then
                Logger.Dbg("Add " & lTable.NumRecords & " SegmentsFrom " & lSegmentShapefileName)
                lTempSegments.AddRange(NumberObjects(lTable.PopulateObjects((New atcWASP.Segment).GetType, pSegmentFieldMap), "Name"))
            End If
            Logger.Dbg("SegmentsCount " & lTempSegments.Count)

            Dim lShapeIndex As Integer = -1
            For Each lSegment As atcWASP.Segment In lTempSegments
                Dim lTimeseriesCollection As New atcWASP.WASPTimeseriesCollection
                lSegment.InputTimeseriesCollection = lTimeseriesCollection
                lSegment.BaseID = lSegment.ID   'store segment id before breaking up
                lShapeIndex += 1
                GisUtil.LineCentroid(lSegmentLayerIndex, lShapeIndex, lSegment.CentroidX, lSegment.CentroidY) 'store centroid 
            Next

            'after reading the attribute table, see if any are selected
            If GisUtil.NumSelectedFeatures(lSegmentLayerIndex) > 0 Then
                'put only selected segments in .segments 
                For lIndex As Integer = 0 To GisUtil.NumSelectedFeatures(lSegmentLayerIndex) - 1
                    .Segments.Add(lTempSegments(GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex, lSegmentLayerIndex)))
                Next
            Else
                'add all 
                .Segments = lTempSegments
            End If

            'calculate depth and width from mean annual flow and mean annual velocity
            'Depth (ft)= a*DA^b (english):  a= 1.5; b=0.284
            For Each lSegment As Segment In .Segments
                lSegment.Depth = 1.5 * (lSegment.CumulativeDrainageArea ^ 0.284)   'gives depth in ft
                lSegment.Width = (lSegment.MeanAnnualFlow / lSegment.Velocity) / lSegment.Depth  'gives width in ft
            Next

            'do unit conversions from NHDPlus units to WASP assumed units
            For Each lSegment As Segment In .Segments
                lSegment.Velocity = SignificantDigits(lSegment.Velocity / 3.281, 3)  'convert ft/s to m/s
                lSegment.MeanAnnualFlow = SignificantDigits(lSegment.MeanAnnualFlow / (3.281 ^ 3), 3) 'convert cfs to cms
                'lSegment.DrainageArea = lSegment.DrainageArea  'already in sq km
                lSegment.Depth = SignificantDigits(lSegment.Depth / 3.281, 3)  'convert ft to m
                lSegment.Width = SignificantDigits(lSegment.Width / 3.281, 3)  'convert ft to m
            Next

            'if a maximum travel time has been set, divide the segments as needed
            Dim lMaxTravelTime As Double = atxTravelTime.Text
            If lMaxTravelTime > 0 Then
                Dim lNewSegments As New Segments
                Dim lNewSegmentPositions As New atcCollection
                For lIndex As Integer = 1 To .Segments.Count
                    Dim lSegment As Segment = .Segments(lIndex - 1)
                    If TravelTime(lSegment.Length, lSegment.Velocity) > lMaxTravelTime Then
                        'need to break this segment into multiple
                        Dim lBreakNumber As Integer = Int(TravelTime(lSegment.Length, lSegment.Velocity) / lMaxTravelTime) + 1
                        'create the new pieces
                        For lBreakIndex As Integer = 2 To lBreakNumber
                            Dim lNewSegment As New Segment
                            lNewSegment = lSegment.Clone
                            lNewSegment.ID = lSegment.ID & IntegerToAlphabet(lBreakIndex - 1)
                            If lBreakIndex < lBreakNumber Then
                                lNewSegment.DownID = lSegment.ID & IntegerToAlphabet(lBreakIndex)
                            Else
                                lNewSegment.DownID = lSegment.DownID
                            End If
                            lNewSegment.Length = lSegment.Length / lBreakNumber
                            lNewSegments.Add(lNewSegment)
                            lNewSegmentPositions.Add(lNewSegment.ID, lIndex)
                        Next
                        'reset length and id for the original segment 
                        Dim lOldID As String = lSegment.ID
                        lSegment.ID = lOldID & "A"
                        'if this segment id shows up as a downid anywhere else, change it
                        For Each lTempSeg As Segment In .Segments
                            If lTempSeg.DownID = lOldID Then
                                lTempSeg.DownID = lSegment.ID
                            End If
                        Next
                        lSegment.DownID = lOldID & "B"
                        lSegment.Length = lSegment.Length / lBreakNumber
                    End If
                Next
                'if any new segments, add them now to the segments collection
                For lIndex As Integer = lNewSegments.Count To 1 Step -1
                    .Segments.Insert(lNewSegmentPositions(lIndex - 1), lNewSegments(lIndex - 1))
                Next
            End If

            Dim lProblem As String = .Segments.AssignWaspIds()
        End With


        SetSegmentationGrid()
        SetFlowStationGrid()
        SetLoadStationGrid()

    End Sub

    Private Sub SetSegmentationGrid()
        If AtcGridSegmentation.Source Is Nothing Then
            Logger.Dbg("No atcGridSegmentation")
        Else
            Logger.Dbg("Begin")

            Dim lMaxTravelTime As Double = 0
            With AtcGridSegmentation.Source
                .Rows = 1 + pPlugIn.WASPProject.Segments.Count
                For lIndex As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                    .CellValue(lIndex, 0) = pPlugIn.WASPProject.Segments(lIndex - 1).ID & ":" & pPlugIn.WASPProject.Segments(lIndex - 1).Name
                    .CellColor(lIndex, 0) = SystemColors.ControlDark
                    .CellValue(lIndex, 1) = pPlugIn.WASPProject.Segments(lIndex - 1).WASPID
                    .CellColor(lIndex, 1) = SystemColors.ControlDark
                    .CellValue(lIndex, 2) = SignificantDigits(pPlugIn.WASPProject.Segments(lIndex - 1).Length, 3)
                    .CellEditable(lIndex, 2) = True
                    .CellValue(lIndex, 3) = pPlugIn.WASPProject.Segments(lIndex - 1).Width
                    .CellEditable(lIndex, 3) = True
                    .CellValue(lIndex, 4) = pPlugIn.WASPProject.Segments(lIndex - 1).Depth
                    .CellEditable(lIndex, 4) = True
                    .CellValue(lIndex, 5) = pPlugIn.WASPProject.Segments(lIndex - 1).Slope
                    .CellEditable(lIndex, 5) = True
                    .CellValue(lIndex, 6) = pPlugIn.WASPProject.Segments(lIndex - 1).Roughness
                    .CellEditable(lIndex, 6) = True
                    .CellValue(lIndex, 7) = pPlugIn.WASPProject.Segments(lIndex - 1).DownID
                    .CellValue(lIndex, 8) = pPlugIn.WASPProject.Segments(lIndex - 1).Velocity
                    .CellEditable(lIndex, 8) = True
                    Dim lTravelTime As Double = 0.0
                    If pPlugIn.WASPProject.Segments(lIndex - 1).Velocity > 0 Then
                        lTravelTime = TravelTime(pPlugIn.WASPProject.Segments(lIndex - 1).Length, pPlugIn.WASPProject.Segments(lIndex - 1).Velocity)
                        lTravelTime = SignificantDigits(lTravelTime, 3)
                    End If
                    .CellValue(lIndex, 9) = lTravelTime
                    If lTravelTime > lMaxTravelTime Then
                        lMaxTravelTime = lTravelTime
                    End If
                Next
            End With

            AtcGridSegmentation.SizeAllColumnsToContents()
            AtcGridSegmentation.ColumnWidth(0) = 140
            AtcGridSegmentation.Refresh()

            If pInitializing Then
                atxTravelTime.Text = lMaxTravelTime
            End If

            Logger.Dbg("SegmentationGrid refreshed")
        End If
    End Sub

    Private Sub SetFlowStationGrid()
        If AtcGridFlow.Source Is Nothing OrElse cboStreams.SelectedIndex = -1 Then
            Logger.Dbg("No atcGridFlow or Streams layer selected")
        Else
            Logger.Dbg("Begin")

            With AtcGridFlow.Source
                .Rows = 1 + pPlugIn.WASPProject.Segments.Count
                For lIndex As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                    .CellValue(lIndex, 0) = pPlugIn.WASPProject.Segments(lIndex - 1).ID & ":" & pPlugIn.WASPProject.Segments(lIndex - 1).Name
                    .CellColor(lIndex, 0) = SystemColors.ControlDark
                    .CellValue(lIndex, 1) = pPlugIn.WASPProject.Segments(lIndex - 1).CumulativeDrainageArea
                    .CellValue(lIndex, 2) = "<none>"
                    If pFlowStationCandidates.Count > 0 Then
                        .CellEditable(lIndex, 2) = True
                    Else
                        .CellEditable(lIndex, 2) = False
                    End If
                Next
            End With

            Logger.Dbg("SetValidValues")
            Dim lValidValues As New atcCollection
            lValidValues.Add("<none>")
            For Each lFlowStation As WASPTimeseries In pFlowStationCandidates
                lValidValues.Add(lFlowStation.Description)
            Next
            AtcGridFlow.ValidValues = lValidValues
            AtcGridFlow.SizeAllColumnsToContents()
            AtcGridFlow.Refresh()

            Logger.Dbg("FlowStationGrid refreshed")
        End If
    End Sub

    Private Sub SetLoadStationGrid()
        If AtcGridLoad.Source Is Nothing Then
            Logger.Dbg("No atcGridLoad")
        Else
            Logger.Dbg("Begin")

            With AtcGridLoad.Source
                .Rows = 1 + pPlugIn.WASPProject.Segments.Count
                For lIndex As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                    .CellValue(lIndex, 0) = pPlugIn.WASPProject.Segments(lIndex - 1).ID & ":" & pPlugIn.WASPProject.Segments(lIndex - 1).Name
                    .CellColor(lIndex, 0) = SystemColors.ControlDark
                    .CellValue(lIndex, 1) = "<none>"
                    If pWaterTempStationCandidates.Count > 0 Then
                        .CellValue(lIndex, 1) = "<none>"
                        .CellEditable(lIndex, 1) = True
                    Else
                        .CellEditable(lIndex, 1) = False
                    End If
                Next
            End With

            Logger.Dbg("SetValidValues")
            Dim lValidValues As New atcCollection
            lValidValues.Add("<none>")
            For Each lWaterTempStation As WASPTimeseries In pWaterTempStationCandidates
                lValidValues.Add(lWaterTempStation.Description)
            Next
            AtcGridLoad.ValidValues = lValidValues
            AtcGridLoad.SizeAllColumnsToContents()
            AtcGridLoad.Refresh()

            Logger.Dbg("LoadStationGrid refreshed")
        End If
    End Sub

    Private Sub SetMetStationValidValues()

        cbxAir.Items.Clear()
        cbxAir.Items.Add("<none>")

        cbxSolar.Items.Clear()
        cbxSolar.Items.Add("<none>")

        cbxWind.Items.Clear()
        cbxWind.Items.Add("<none>")

        For Each lStationCandidate As WASPTimeseries In pAirTempStationCandidates
            cbxAir.Items.Add(lStationCandidate.Description)
        Next

        For Each lStationCandidate As WASPTimeseries In pSolRadStationCandidates
            cbxSolar.Items.Add(lStationCandidate.Description)
        Next

        For Each lStationCandidate As WASPTimeseries In pWindStationCandidates
            cbxWind.Items.Add(lStationCandidate.Description)
        Next

        'default met stations based on distance
        Dim lXSum As Double = 0
        Dim lYSum As Double = 0
        For Each lSegment As Segment In pPlugIn.WASPProject.Segments
            'find average segment centroid 
            lXSum = lXSum + lSegment.CentroidX
            lYSum = lYSum + lSegment.CentroidY
        Next
        Dim lXAvg As Double = 0
        Dim lYAvg As Double = 0
        If pPlugIn.WASPProject.Segments.Count > 0 Then
            lXAvg = lXSum / pPlugIn.WASPProject.Segments.Count
            lYAvg = lYSum / pPlugIn.WASPProject.Segments.Count
        Else
            cbxAir.SelectedIndex = 0
            cbxSolar.SelectedIndex = 0
            cbxWind.SelectedIndex = 0
        End If

        If pPlugIn.WASPProject.Segments.Count > 0 Then
            'for each valid value, find distance
            Dim lShortestDistance As Double = 1.0E+28
            Dim lDistance As Double = 0.0
            Dim lClosestIndex As Integer = 0
            Dim lStationIndex As Integer = 0
            For Each lStationCandidate As WASPTimeseries In pAirTempStationCandidates
                lStationIndex += 1
                lDistance = CalculateDistance(lXAvg, lYAvg, lStationCandidate.LocationX, lStationCandidate.LocationY)
                If lDistance < lShortestDistance Then
                    lShortestDistance = lDistance
                    lClosestIndex = lStationIndex
                End If
            Next
            cbxAir.SelectedIndex = lClosestIndex

            lShortestDistance = 1.0E+28
            lDistance = 0.0
            lClosestIndex = 0
            lStationIndex = 0
            For Each lStationCandidate As WASPTimeseries In pSolRadStationCandidates
                lStationIndex += 1
                lDistance = CalculateDistance(lXAvg, lYAvg, lStationCandidate.LocationX, lStationCandidate.LocationY)
                If lDistance < lShortestDistance Then
                    lShortestDistance = lDistance
                    lClosestIndex = lStationIndex
                End If
            Next
            cbxSolar.SelectedIndex = lClosestIndex

            lShortestDistance = 1.0E+28
            lDistance = 0.0
            lClosestIndex = 0
            lStationIndex = 0
            For Each lStationCandidate As WASPTimeseries In pWindStationCandidates
                lStationIndex += 1
                lDistance = CalculateDistance(lXAvg, lYAvg, lStationCandidate.LocationX, lStationCandidate.LocationY)
                If lDistance < lShortestDistance Then
                    lShortestDistance = lDistance
                    lClosestIndex = lStationIndex
                End If
            Next
            cbxWind.SelectedIndex = lClosestIndex
        End If

    End Sub

    Friend Function GetMetFile(ByRef aMetWDMName As String) As atcWDM.atcDataSourceWDM
        Logger.Dbg("MetWDMName " & aMetWDMName)

        Dim lDataSource As atcWDM.atcDataSourceWDM = Nothing
        If FileExists(aMetWDMName) Then
            Dim lFound As Boolean = False
            For Each lBASINSDataSource As atcTimeseriesSource In atcDataManager.DataSources
                If lBASINSDataSource.Specification.ToUpper = aMetWDMName.ToUpper Then
                    'found it in the BASINS data sources
                    lDataSource = lBASINSDataSource
                    lFound = True
                    Exit For
                End If
            Next

            If Not lFound Then 'need to open it here
                lDataSource = New atcWDM.atcDataSourceWDM
                If lDataSource.Open(aMetWDMName) Then
                    lFound = True
                End If
            End If
        End If
        Return lDataSource
    End Function

    Private Sub cmdFieldMapping_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFieldMapping.Click
        Dim lStreamsLayerIndex As Integer = GisUtil.LayerIndex(cboStreams.Items(cboStreams.SelectedIndex))
        If IsNothing(pfrmWASPFieldMapping) Then
            pfrmWASPFieldMapping = New frmWASPFieldMapping
            pfrmWASPFieldMapping.Init(lStreamsLayerIndex, pSegmentFieldMap, Me)
            pfrmWASPFieldMapping.Show()
        Else
            If pfrmWASPFieldMapping.IsDisposed Then
                pfrmWASPFieldMapping = New frmWASPFieldMapping
                pfrmWASPFieldMapping.Init(lStreamsLayerIndex, pSegmentFieldMap, Me)
                pfrmWASPFieldMapping.Show()
            Else
                pfrmWASPFieldMapping.WindowState = FormWindowState.Normal
                pfrmWASPFieldMapping.BringToFront()
            End If
        End If
    End Sub

    Private Sub cmdCreateShapefile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreateShapefile.Click
        Dim lSegmentLayerIndex As Integer = GisUtil.LayerIndex(cboStreams.Items(cboStreams.SelectedIndex))
        Dim lSegmentShapefileName As String = GisUtil.LayerFileName(lSegmentLayerIndex)

        Me.Cursor = Cursors.WaitCursor
        'come up with name of new shapefile
        Dim lOutputPath As String = PathNameOnly(lSegmentShapefileName)
        Dim lIndex As Integer = 1
        Dim lWASPShapefileName As String = lOutputPath & "\WASPSegments" & lIndex & ".shp"
        Do While FileExists(lWASPShapefileName)
            lIndex += 1
            lWASPShapefileName = lOutputPath & "\WASPSegments" & lIndex & ".shp"
        Loop

        'figure out which shapes we want from old shapefile
        Dim lShapeIds As New atcCollection
        For Each lSegment As Segment In pPlugIn.WASPProject.Segments
            If Not lShapeIds.Contains(lSegment.BaseID) Then
                lShapeIds.Add(lSegment.BaseID)
            End If
        Next

        'which field is mapped to the id?
        Dim lIDFieldIndex As Integer
        Dim lIDFieldName As String = "ID"
        For lIndex = 0 To pSegmentFieldMap.Count - 1
            If pSegmentFieldMap.ItemByIndex(lIndex) = "ID" Then
                Dim lKey As String = pSegmentFieldMap.Keys(lIndex)
                If GisUtil.IsField(lSegmentLayerIndex, lKey) Then
                    lIDFieldIndex = GisUtil.FieldIndex(lSegmentLayerIndex, lKey)
                    lIDFieldName = lKey
                End If
            End If
        Next

        'create the new empty shapefile
        GisUtil.CreateEmptyShapefile(lWASPShapefileName, "", "line")
        GisUtil.AddLayer(lWASPShapefileName, "WASP Segments")
        Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(lWASPShapefileName)
        GisUtil.LayerVisible(lNewLayerIndex) = True
        'add an id field to the new shapefile
        Dim lNewIDFieldIndex = GisUtil.AddField(lNewLayerIndex, lIDFieldName, 0, 20)

        'find each desired shape
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lSegmentLayerIndex) - 1
            For Each lShapeId As String In lShapeIds
                If GisUtil.FieldValue(lSegmentLayerIndex, lFeatureIndex, lIDFieldIndex) = lShapeId Then
                    'this is one of the shapes we want

                    'how many shapes do we want to break this one into?
                    Dim lCount As Integer = 0
                    Dim lPieceIDs As New atcCollection
                    For Each lSegment As Segment In pPlugIn.WASPProject.Segments
                        If lSegment.BaseID = lShapeId Then
                            lCount = lCount + 1
                            lPieceIDs.Add(lSegment.ID)
                        End If
                    Next

                    Dim lX() As Double = Nothing
                    Dim lY() As Double = Nothing
                    GisUtil.PointsOfLine(lSegmentLayerIndex, lFeatureIndex, lX, lY)

                    If lCount = 1 Then
                        'create line from these points in the new shapefile
                        GisUtil.AddLine(lNewLayerIndex, lX, lY)
                        GisUtil.SetFeatureValue(lNewLayerIndex, lNewIDFieldIndex, GisUtil.NumFeatures(lNewLayerIndex) - 1, lShapeId)
                    Else
                        'break this line into lcount pieces
                        Dim lX2() As Double = Nothing
                        Dim lY2() As Double = Nothing
                        Dim lLineEndIndexes(lCount) As Integer
                        BreakLine(lX, lY, lCount, lX2, lY2, lLineEndIndexes)
                        For lLineIndex As Integer = 1 To lCount
                            Dim lXTemp(lLineEndIndexes(lLineIndex) - lLineEndIndexes(lLineIndex - 1)) As Double
                            Dim lYTemp(lLineEndIndexes(lLineIndex) - lLineEndIndexes(lLineIndex - 1)) As Double
                            Dim lPointCounter As Integer = -1
                            For lPoints As Integer = lLineEndIndexes(lLineIndex - 1) To lLineEndIndexes(lLineIndex)
                                lPointCounter += 1
                                lXTemp(lPointCounter) = lX2(lPoints)
                                lYTemp(lPointCounter) = lY2(lPoints)
                            Next
                            GisUtil.AddLine(lNewLayerIndex, lXTemp, lYTemp)
                            GisUtil.SetFeatureValue(lNewLayerIndex, lNewIDFieldIndex, GisUtil.NumFeatures(lNewLayerIndex) - 1, lPieceIDs(lLineIndex - 1))
                        Next
                    End If

                End If
            Next
        Next

        Me.Cursor = Cursors.Default
        Logger.Msg("Create Shapefile complete.", MsgBoxStyle.OkOnly, "Create Shapefile")
    End Sub

    Private Sub SetDates()
        'set dates on the general tab to the last common year of the selected timeseries       

        Dim lSJDate As Double = 0.0
        Dim lEJDate As Double = 0.0

        For Each lTimeseries As WASPTimeseries In pPlugIn.WASPProject.InputTimeseriesCollection
            If lTimeseries.SDate > lSJDate Then
                lSJDate = lTimeseries.SDate
            End If
            If lEJDate = 0.0 Or lTimeseries.EDate < lEJDate Then
                lEJDate = lTimeseries.EDate
            End If
        Next

        Dim lEDate(5) As Integer, lSDate(5) As Integer
        J2Date(lEJDate, lEDate)
        J2Date(lSJDate, lSDate)

        'set limits
        atxSYear.HardMax = lEDate(0)
        atxSYear.HardMin = lSDate(0)
        atxEYear.HardMax = lEDate(0)
        atxEYear.HardMin = lSDate(0)

        'default to last calendar year of data
        lSDate(0) = lEDate(0) - 1
        lSDate(1) = 1
        lSDate(2) = 1
        lEDate(0) = lSDate(0)
        lEDate(1) = 12
        lEDate(2) = 31
        atxSYear.Text = lSDate(0)
        atxSMonth.Text = lSDate(1)
        atxSDay.Text = lSDate(2)
        atxEYear.Text = lEDate(0)
        atxEMonth.Text = lEDate(1)
        atxEDay.Text = lEDate(2)
    End Sub

    Private Sub cmdSelectConstituents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectConstituents.Click
        Logger.Msg("Feature not yet implemented.", MsgBoxStyle.OkOnly, "Select Constituents")
    End Sub

    Private Sub cmdGenerateTimeseries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGenerateTimeseries.Click
        Logger.Msg("Feature not yet implemented.", MsgBoxStyle.OkOnly, "Generate New Timeseries")
    End Sub

    Private Sub cmdVolumes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdVolumes.Click
        Logger.Msg("Feature not yet implemented.", MsgBoxStyle.OkOnly, "Volumes/Depths")
    End Sub

    Private Sub cbxAir_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxAir.SelectedIndexChanged
        RebuildTimeseriesCollections()
        SetDates()
    End Sub

    Private Sub cbxSolar_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSolar.SelectedIndexChanged
        RebuildTimeseriesCollections()
        SetDates()
    End Sub

    Private Sub cbxWind_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxWind.SelectedIndexChanged
        RebuildTimeseriesCollections()
        SetDates()
    End Sub
End Class