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
    Friend WithEvents tbxName As System.Windows.Forms.TextBox
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
    Friend WithEvents atxTravelTimeMax As atcControls.atcText
    Friend WithEvents cmdFieldMapping As System.Windows.Forms.Button
    Friend WithEvents cmdCreateShapefile As System.Windows.Forms.Button
    Friend WithEvents cbxWind As System.Windows.Forms.ComboBox
    Friend WithEvents cbxSolar As System.Windows.Forms.ComboBox
    Friend WithEvents cbxAir As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblAir As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents atxTravelTimeMin As atcControls.atcText
    Friend WithEvents cboModel As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents AtcGridBound As atcControls.atcGrid
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
        Me.cboModel = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
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
        Me.tbxName = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.atxTravelTimeMin = New atcControls.atcText
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmdCreateShapefile = New System.Windows.Forms.Button
        Me.cmdFieldMapping = New System.Windows.Forms.Button
        Me.atxTravelTimeMax = New atcControls.atcText
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdGenerate = New System.Windows.Forms.Button
        Me.AtcGridSegmentation = New atcControls.atcGrid
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.AtcGridFlow = New atcControls.atcGrid
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.AtcGridLoad = New atcControls.atcGrid
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.cbxWind = New System.Windows.Forms.ComboBox
        Me.cbxSolar = New System.Windows.Forms.ComboBox
        Me.cbxAir = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblAir = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.ofdMetWDM = New System.Windows.Forms.OpenFileDialog
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.AtcGridBound = New atcControls.atcGrid
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage5.SuspendLayout()
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
        Me.TabControl1.Controls.Add(Me.TabPage5)
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
        Me.TabPage1.Controls.Add(Me.cboModel)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.cboMet)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.tbxName)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(899, 398)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "General"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'cboModel
        '
        Me.cboModel.AllowDrop = True
        Me.cboModel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboModel.Location = New System.Drawing.Point(167, 79)
        Me.cboModel.Name = "cboModel"
        Me.cboModel.Size = New System.Drawing.Size(341, 25)
        Me.cboModel.TabIndex = 30
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(11, 82)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(94, 17)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "WASP Model:"
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
        Me.cboMet.Location = New System.Drawing.Point(168, 151)
        Me.cboMet.Name = "cboMet"
        Me.cboMet.Size = New System.Drawing.Size(719, 25)
        Me.cboMet.TabIndex = 12
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(11, 154)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(130, 17)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "Met Stations Layer:"
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
        Me.TabPage2.Controls.Add(Me.atxTravelTimeMin)
        Me.TabPage2.Controls.Add(Me.Label6)
        Me.TabPage2.Controls.Add(Me.cmdCreateShapefile)
        Me.TabPage2.Controls.Add(Me.cmdFieldMapping)
        Me.TabPage2.Controls.Add(Me.atxTravelTimeMax)
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
        'atxTravelTimeMin
        '
        Me.atxTravelTimeMin.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxTravelTimeMin.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxTravelTimeMin.DefaultValue = ""
        Me.atxTravelTimeMin.HardMax = -999
        Me.atxTravelTimeMin.HardMin = 0
        Me.atxTravelTimeMin.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxTravelTimeMin.Location = New System.Drawing.Point(215, 44)
        Me.atxTravelTimeMin.MaxWidth = 20
        Me.atxTravelTimeMin.Name = "atxTravelTimeMin"
        Me.atxTravelTimeMin.NumericFormat = "0.#####"
        Me.atxTravelTimeMin.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxTravelTimeMin.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxTravelTimeMin.SelLength = 1
        Me.atxTravelTimeMin.SelStart = 0
        Me.atxTravelTimeMin.Size = New System.Drawing.Size(48, 24)
        Me.atxTravelTimeMin.SoftMax = -999
        Me.atxTravelTimeMin.SoftMin = -999
        Me.atxTravelTimeMin.TabIndex = 27
        Me.atxTravelTimeMin.ValueDouble = 0
        Me.atxTravelTimeMin.ValueInteger = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(20, 47)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(186, 17)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "Minimum Travel Time (days)"
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
        'atxTravelTimeMax
        '
        Me.atxTravelTimeMax.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxTravelTimeMax.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxTravelTimeMax.DefaultValue = ""
        Me.atxTravelTimeMax.HardMax = -999
        Me.atxTravelTimeMax.HardMin = 0
        Me.atxTravelTimeMax.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxTravelTimeMax.Location = New System.Drawing.Point(215, 16)
        Me.atxTravelTimeMax.MaxWidth = 20
        Me.atxTravelTimeMax.Name = "atxTravelTimeMax"
        Me.atxTravelTimeMax.NumericFormat = "0.#####"
        Me.atxTravelTimeMax.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxTravelTimeMax.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxTravelTimeMax.SelLength = 1
        Me.atxTravelTimeMax.SelStart = 0
        Me.atxTravelTimeMax.Size = New System.Drawing.Size(48, 24)
        Me.atxTravelTimeMax.SoftMax = -999
        Me.atxTravelTimeMax.SoftMin = -999
        Me.atxTravelTimeMax.TabIndex = 6
        Me.atxTravelTimeMax.ValueDouble = 0
        Me.atxTravelTimeMax.ValueInteger = 0
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
        Me.cmdGenerate.Location = New System.Drawing.Point(269, 20)
        Me.cmdGenerate.Name = "cmdGenerate"
        Me.cmdGenerate.Size = New System.Drawing.Size(126, 44)
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
        Me.TabPage3.Controls.Add(Me.AtcGridFlow)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(899, 398)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Flows"
        Me.TabPage3.UseVisualStyleBackColor = True
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
        Me.TabPage4.Controls.Add(Me.AtcGridLoad)
        Me.TabPage4.Location = New System.Drawing.Point(4, 25)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(899, 398)
        Me.TabPage4.TabIndex = 6
        Me.TabPage4.Text = "Loads"
        Me.TabPage4.UseVisualStyleBackColor = True
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
        'cbxWind
        '
        Me.cbxWind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxWind.FormattingEnabled = True
        Me.cbxWind.Location = New System.Drawing.Point(164, 92)
        Me.cbxWind.Name = "cbxWind"
        Me.cbxWind.Size = New System.Drawing.Size(707, 25)
        Me.cbxWind.TabIndex = 28
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
        'cbxAir
        '
        Me.cbxAir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxAir.FormattingEnabled = True
        Me.cbxAir.Location = New System.Drawing.Point(164, 30)
        Me.cbxAir.Name = "cbxAir"
        Me.cbxAir.Size = New System.Drawing.Size(707, 25)
        Me.cbxAir.TabIndex = 26
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
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(32, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(105, 17)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Solar Radiation"
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
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.AtcGridBound)
        Me.TabPage5.Location = New System.Drawing.Point(4, 25)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(899, 398)
        Me.TabPage5.TabIndex = 7
        Me.TabPage5.Text = "Boundaries"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'AtcGridBound
        '
        Me.AtcGridBound.AllowHorizontalScrolling = True
        Me.AtcGridBound.AllowNewValidValues = False
        Me.AtcGridBound.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridBound.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridBound.Fixed3D = False
        Me.AtcGridBound.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridBound.LineColor = System.Drawing.Color.Empty
        Me.AtcGridBound.LineWidth = 0.0!
        Me.AtcGridBound.Location = New System.Drawing.Point(23, 74)
        Me.AtcGridBound.Name = "AtcGridBound"
        Me.AtcGridBound.Size = New System.Drawing.Size(854, 307)
        Me.AtcGridBound.Source = Nothing
        Me.AtcGridBound.TabIndex = 22
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
        Me.TabPage5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend pPlugIn As PlugIn
    Friend pSelectedIndexes As atcCollection
    Friend pSegmentLayerIndex As Integer
    Friend pBasinsFolder As String
    Friend pfrmWASPFieldMapping As frmWASPFieldMapping
    Friend pWASPModelsDB As atcCollection
    Friend pWASPSystemIdsDB As atcCollection
    Friend pWASPSystemNamesDB As atcCollection
    Friend pWASPTimeFunctionIdsDB As atcCollection
    Friend pWASPTimeFunctionNamesDB As atcCollection

    Private pSelectedRow As Integer
    Private pSelectedColumn As Integer

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'WriteWASPConstituentNamesToFile(pBasinsFolder & "\etc\WASPConstituents.txt")
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

        pPlugIn.WASPProject.RebuildTimeseriesCollections(cbxAir.SelectedItem, cbxSolar.SelectedItem, cbxWind.SelectedItem, AtcGridFlow.Source, AtcGridLoad.Source, AtcGridBound.Source)

        'check that specified dates are valid
        Dim lSJDate As Double = 0.0
        Dim lEJDate As Double = 0.0
        Dim lSDate(5) As Integer
        Dim lEDate(5) As Integer
        lSDate(0) = atxSYear.Text
        lSDate(1) = atxSMonth.Text
        lSDate(2) = atxSDay.Text
        lEDate(0) = atxEYear.Text
        lEDate(1) = atxEMonth.Text
        lEDate(2) = atxEDay.Text
        lSJDate = Date2J(lSDate)
        lEJDate = Date2J(lEDate)
        If lSJDate < 1.0 Or lEJDate < 1 Then 'failed date check
            Logger.Msg("The specified start/end dates are invalid.", vbOKOnly, "BASINS WASP Problem")
            EnableControls(True)
            Exit Sub
        End If
        If lSJDate > lEJDate Then 'failed date check
            Logger.Msg("The specified starting date is after the ending date.", vbOKOnly, "BASINS WASP Problem")
            EnableControls(True)
            Exit Sub
        End If

        'check that time series dates have a common period of record, and that the specified start/end dates 
        'are within that period
        Dim lSJDateTS As Double = 0.0
        Dim lEJDateTS As Double = 0.0
        For Each lTimeseries As atcWASPTimeseries In pPlugIn.WASPProject.InputTimeseriesCollection
            If lTimeseries.SDate > lSJDateTS Then
                lSJDateTS = lTimeseries.SDate
            End If
            If lEJDateTS = 0.0 Or lTimeseries.EDate < lEJDateTS Then
                lEJDateTS = lTimeseries.EDate
            End If
        Next
        If lSJDateTS > lEJDateTS Then 'failed date check
            Logger.Msg("The specified time series do not have a common period of record.", vbOKOnly, "BASINS WASP Problem")
            EnableControls(True)
            Exit Sub
        End If
        'compare dates from met data with specified start and end dates, make sure they are valid
        If lSJDate < lSJDateTS Or lEJDateTS < lEJDate Then 'failed date check
            Logger.Msg("The specified start/end dates are not within the dates of the specified time series.", vbOKOnly, "BASINS WASP Problem")
            EnableControls(True)
            Exit Sub
        End If

        Dim lName As String = tbxName.Text
        'TODO: still use modelout?
        Dim lWASPProjectFileName As String = pBasinsFolder & "\modelout\" & lName & "\" & lName & ".wnf"
        Dim lWASPInpFileName As String = pBasinsFolder & "\modelout\" & lName & "\" & lName & ".inp"
        MkDirPath(PathNameOnly(lWASPProjectFileName))

        If PreProcessChecking(lWASPInpFileName) Then
            With pPlugIn.WASPProject
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                'set start and end dates
                .SJDate = lSJDate
                .EJDate = lEJDate

                .Name = lName

                'save project file and start WASP
                Logger.Dbg("Save WASP network import file" & lWASPProjectFileName)
                .Save(lWASPProjectFileName)
                .WriteINP(lWASPInpFileName)
                Logger.Dbg("Run WASP")
                .Run(lWASPInpFileName)
                Logger.Dbg("BackFromWASP")
            End With

            'WriteWASPConstituentNamesToFile(pBasinsFolder & "\etc\WASPConstituents.txt")

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

    Public Sub InitializeUI(ByVal aPlugIn As PlugIn, ByVal aSelectedIndexes As atcCollection, ByVal aSegmentLayerIndex As Integer)
        Logger.Dbg("InitializeUI")
        EnableControls(False)
        pPlugIn = aPlugIn
        pSelectedIndexes = aSelectedIndexes
        pSegmentLayerIndex = aSegmentLayerIndex

        pBasinsFolder = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        'ReadWASPConstituentNamesFromFile(pBasinsFolder & "\etc\WASPConstituents.txt")
        ReadWASPdb(pBasinsFolder & "\etc\")

        For Each lModel As String In pWASPModelsDB
            cboModel.Items.Add(lModel)
        Next
        cboModel.SelectedIndex = 0

        cboMet.Items.Add("<none>")

        With AtcGridSegmentation
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With

        With AtcGridFlow
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With

        With AtcGridBound
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = True
        End With

        With AtcGridLoad
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = True
        End With

        For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 3 Then 'PolygonShapefile 

            ElseIf GisUtil.LayerType(lLayerIndex) = 2 Then 'LineShapefile 

            ElseIf GisUtil.LayerType(lLayerIndex) = 1 Then 'PointShapefile
                cboMet.Items.Add(lLayerName)
                If lLayerName.ToUpper.IndexOf("WEATHER STATION SITES 20") > -1 Then
                    'this takes some time, show window and then do this
                    'cboMet.SelectedIndex = cboMet.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = 4 Then 'Grid

            End If
        Next

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
            .Columns = 4
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 3
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .CellColor(0, 2) = SystemColors.ControlDark
            .Rows = 1 + pPlugIn.WASPProject.Segments.Count
            .CellValue(0, 0) = "Segment"
            .CellValue(0, 1) = "Cum. Drainage Area (km^2)"
            .CellValue(0, 2) = "Mean Annual Flow (cms)"
            .CellValue(0, 3) = "Input Flow Timeseries"
        End With

        AtcGridBound.Clear()
        With AtcGridBound.Source
            .Columns = 2
            .Rows = 1 + pPlugIn.WASPProject.Segments.Count
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellValue(0, 0) = "Boundary Segment"
        End With

        AtcGridLoad.Clear()
        With AtcGridLoad.Source
            .Columns = 2
            .Rows = 1 + pPlugIn.WASPProject.Segments.Count
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellValue(0, 0) = "Segment"
        End With

        Logger.Dbg("InitializeUI Complete")
    End Sub

    Friend Sub InitializeStationLists()
        'this takes some time, show window and then do this
        Logger.Dbg("Initializing StationLists")
        lblStatus.Text = "Reading Timeseries Data..."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        EnableControls(False)

        'TODO: move to atcWASP???
        With pPlugIn.WASPProject
            .BuildListofValidStationNames("FLOW", .FlowStationCandidates)
            .BuildListofValidStationNames("ATMP", .AirTempStationCandidates)
            .BuildListofValidStationNames("ATEM", .AirTempStationCandidates)
            .BuildListofValidStationNames("SOLRAD", .SolRadStationCandidates)
            .BuildListofValidStationNames("SOLR", .SolRadStationCandidates)
            .BuildListofValidStationNames("WIND", .WindStationCandidates)
            .BuildListofValidStationNames("", .WQStationCandidates)

            'set layer index for met stations
            SetStationCoordinates()

            'set valid values
            SetFlowStationGrid()
            SetLoadStationGrid()
            SetBoundaryGrid()
            SetMetStationValidValues()
        End With

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

    Friend Sub GenerateSegments()
        'this takes some time, show window and then do this
        Logger.Dbg("Generating Segments")
        lblStatus.Text = "Reading Segment Data..."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        EnableControls(False)

        pPlugIn.WASPProject.GenerateSegments(pSegmentLayerIndex, pSelectedIndexes, CDbl(atxTravelTimeMax.Text), CDbl(atxTravelTimeMin.Text))

        SetSegmentationGrid()
        SetFlowStationGrid()
        SetLoadStationGrid()
        SetBoundaryGrid()

        lblStatus.Text = "Update specifications if desired, then click OK to proceed."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        EnableControls(True)
    End Sub

    Private Sub SetSegmentationGrid()
        If AtcGridSegmentation.Source Is Nothing Then
            Logger.Dbg("No atcGridSegmentation")
        Else
            Logger.Dbg("Begin")

            Dim lMaxTravelTime As Double = 0
            Dim lMinTravelTime As Double = 1.0E+28
            With AtcGridSegmentation.Source
                .Rows = 1 + pPlugIn.WASPProject.Segments.Count
                For lIndex As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                    .CellValue(lIndex, 0) = pPlugIn.WASPProject.Segments(lIndex - 1).ID & ":" & pPlugIn.WASPProject.Segments(lIndex - 1).Name
                    .CellColor(lIndex, 0) = SystemColors.ControlDark
                    .CellValue(lIndex, 1) = pPlugIn.WASPProject.Segments(lIndex - 1).WASPID
                    .CellColor(lIndex, 1) = SystemColors.ControlDark
                    .CellValue(lIndex, 2) = SignificantDigits(pPlugIn.WASPProject.Segments(lIndex - 1).Length, 3)
                    .CellEditable(lIndex, 2) = True
                    .CellValue(lIndex, 3) = SignificantDigits(pPlugIn.WASPProject.Segments(lIndex - 1).Width, 3)
                    .CellEditable(lIndex, 3) = True
                    .CellValue(lIndex, 4) = SignificantDigits(pPlugIn.WASPProject.Segments(lIndex - 1).Depth, 3)
                    .CellEditable(lIndex, 4) = True
                    .CellValue(lIndex, 5) = SignificantDigits(pPlugIn.WASPProject.Segments(lIndex - 1).Slope, 3)
                    .CellEditable(lIndex, 5) = True
                    .CellValue(lIndex, 6) = pPlugIn.WASPProject.Segments(lIndex - 1).Roughness
                    .CellEditable(lIndex, 6) = True
                    .CellValue(lIndex, 7) = pPlugIn.WASPProject.Segments(lIndex - 1).DownID
                    .CellValue(lIndex, 8) = SignificantDigits(pPlugIn.WASPProject.Segments(lIndex - 1).Velocity, 3)
                    .CellEditable(lIndex, 8) = True
                    Dim lTravelTime As Double = 0.0
                    With pPlugIn.WASPProject
                        Dim lSegment As atcWASPSegment = .Segments(lIndex - 1)
                        lTravelTime = SignificantDigits(.TravelTime(lSegment.Length, lSegment.Velocity), 3)
                    End With
                    .CellValue(lIndex, 9) = lTravelTime
                    If lTravelTime > lMaxTravelTime Then
                        lMaxTravelTime = lTravelTime
                    End If
                    If lTravelTime < lMinTravelTime Then
                        lMinTravelTime = lTravelTime
                    End If
                Next
            End With

            AtcGridSegmentation.SizeAllColumnsToContents()
            AtcGridSegmentation.ColumnWidth(0) = 140
            AtcGridSegmentation.Refresh()

            'If pInitializing Then
            atxTravelTimeMax.Text = lMaxTravelTime
            atxTravelTimeMin.Text = lMinTravelTime
            'End If

            Logger.Dbg("SegmentationGrid refreshed")
        End If
    End Sub

    Private Sub SetFlowStationGrid()
        If AtcGridFlow.Source Is Nothing Then
            Logger.Dbg("No atcGridFlow")
        Else
            Logger.Dbg("Begin")

            With AtcGridFlow.Source
                .Rows = 1 + pPlugIn.WASPProject.Segments.Count
                For lIndex As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                    .CellValue(lIndex, 0) = pPlugIn.WASPProject.Segments(lIndex - 1).ID & ":" & pPlugIn.WASPProject.Segments(lIndex - 1).Name
                    .CellColor(lIndex, 0) = SystemColors.ControlDark
                    .CellValue(lIndex, 1) = pPlugIn.WASPProject.Segments(lIndex - 1).CumulativeDrainageArea
                    .CellValue(lIndex, 2) = pPlugIn.WASPProject.Segments(lIndex - 1).MeanAnnualFlow
                    .CellValue(lIndex, 3) = "<none>"
                    .CellEditable(lIndex, 3) = True
                Next
            End With

            Logger.Dbg("SetValidValues")
            Dim lValidValues As New atcCollection
            lValidValues.Add("<none>")
            lValidValues.Add("<mean annual flow>")
            For Each lFlowStation As atcWASPTimeseries In pPlugIn.WASPProject.FlowStationCandidates
                lValidValues.Add(lFlowStation.Description)
            Next
            AtcGridFlow.ValidValues = lValidValues
            AtcGridFlow.SizeAllColumnsToContents()
            AtcGridFlow.Refresh()

            Logger.Dbg("FlowStationGrid refreshed")
        End If
    End Sub

    Friend Sub SetLoadStationGrid()
        If AtcGridLoad.Source Is Nothing Then
            Logger.Dbg("No atcGridLoad")
        Else
            Logger.Dbg("Begin")

            With AtcGridLoad.Source
                .Columns = 1 + pPlugIn.WASPProject.WASPConstituents.Count
                For lColumn As Integer = 1 To pPlugIn.WASPProject.WASPConstituents.Count
                    .CellColor(0, lColumn) = SystemColors.ControlDark
                    .CellValue(0, lColumn) = pPlugIn.WASPProject.WASPConstituents(lColumn - 1)
                Next
            End With

            With AtcGridLoad.Source
                .Rows = 1 + pPlugIn.WASPProject.Segments.Count
                For lIndex As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                    .CellValue(lIndex, 0) = pPlugIn.WASPProject.Segments(lIndex - 1).ID & ":" & pPlugIn.WASPProject.Segments(lIndex - 1).Name
                    .CellColor(lIndex, 0) = SystemColors.ControlDark
                    For lColumn As Integer = 1 To pPlugIn.WASPProject.WASPConstituents.Count
                        .CellValue(lIndex, lColumn) = "<none>"
                        .CellEditable(lIndex, lColumn) = True
                    Next
                Next
            End With

            AtcGridLoad.SizeAllColumnsToContents()
            AtcGridLoad.Refresh()

            Logger.Dbg("LoadStationGrid refreshed")
        End If
    End Sub

    Private Sub SetLoadStationValidValues()
        Logger.Dbg("SetValidValues")
        Dim lValidValues As New atcCollection
        lValidValues.Add("<none>")
        For Each lWQStation As atcWASPTimeseries In pPlugIn.WASPProject.WQStationCandidates
            lValidValues.Add(lWQStation.Description)
        Next

        AtcGridLoad.ValidValues = lValidValues
    End Sub

    Friend Sub SetBoundaryGrid()
        If AtcGridBound.Source Is Nothing Then
            Logger.Dbg("No atcGridBound")
        Else
            Logger.Dbg("Begin")

            With AtcGridBound.Source
                .Columns = 1 + pPlugIn.WASPProject.WASPConstituents.Count
                For lColumn As Integer = 1 To pPlugIn.WASPProject.WASPConstituents.Count
                    .CellColor(0, lColumn) = SystemColors.ControlDark
                    .CellValue(0, lColumn) = pPlugIn.WASPProject.WASPConstituents(lColumn - 1)
                Next
            End With

            'only load segments that are boundary segments
            With AtcGridBound.Source
                Dim lRow As Integer = 0
                For Each lSegment As atcWASPSegment In pPlugIn.WASPProject.Segments
                    If pPlugIn.WASPProject.IsBoundary(lSegment) Then
                        lRow = lRow + 1
                        .Rows = lRow
                        .CellValue(lRow, 0) = lSegment.ID & ":" & lSegment.Name
                        .CellColor(lRow, 0) = SystemColors.ControlDark
                        For lColumn As Integer = 1 To pPlugIn.WASPProject.WASPConstituents.Count
                            .CellValue(lRow, lColumn) = "<none>"
                            .CellEditable(lRow, lColumn) = True
                        Next
                    End If
                Next
            End With

            AtcGridBound.SizeAllColumnsToContents()
            AtcGridBound.Refresh()

            Logger.Dbg("BoundaryGrid refreshed")
        End If
    End Sub

    Private Sub SetBoundaryValidValues()
        Logger.Dbg("SetValidValues")
        Dim lValidValues As New atcCollection
        lValidValues.Add("<none>")
        For Each lWQStation As atcWASPTimeseries In pPlugIn.WASPProject.WQStationCandidates
            lValidValues.Add(lWQStation.Description)
        Next

        AtcGridBound.ValidValues = lValidValues
    End Sub

    Private Sub SetMetStationValidValues()
        cbxAir.Items.Clear()
        cbxAir.Items.Add("<none>")

        cbxSolar.Items.Clear()
        cbxSolar.Items.Add("<none>")

        cbxWind.Items.Clear()
        cbxWind.Items.Add("<none>")

        For Each lStationCandidate As atcWASPTimeseries In pPlugIn.WASPProject.AirTempStationCandidates
            cbxAir.Items.Add(lStationCandidate.Description)
        Next

        For Each lStationCandidate As atcWASPTimeseries In pPlugIn.WASPProject.SolRadStationCandidates
            cbxSolar.Items.Add(lStationCandidate.Description)
        Next

        For Each lStationCandidate As atcWASPTimeseries In pPlugIn.WASPProject.WindStationCandidates
            cbxWind.Items.Add(lStationCandidate.Description)
        Next

        pPlugIn.WASPProject.DefaultClosestMetStation(cbxAir.SelectedIndex, cbxSolar.SelectedIndex, cbxWind.SelectedIndex)

    End Sub

    Private Sub cmdFieldMapping_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFieldMapping.Click
        If IsNothing(pfrmWASPFieldMapping) Then
            pfrmWASPFieldMapping = New frmWASPFieldMapping
            pfrmWASPFieldMapping.Init(pSegmentLayerIndex, pPlugIn.WASPProject.SegmentFieldMap, Me)
            pfrmWASPFieldMapping.Show()
        Else
            If pfrmWASPFieldMapping.IsDisposed Then
                pfrmWASPFieldMapping = New frmWASPFieldMapping
                pfrmWASPFieldMapping.Init(pSegmentLayerIndex, pPlugIn.WASPProject.SegmentFieldMap, Me)
                pfrmWASPFieldMapping.Show()
            Else
                pfrmWASPFieldMapping.WindowState = FormWindowState.Normal
                pfrmWASPFieldMapping.BringToFront()
            End If
        End If
    End Sub

    Private Sub cmdCreateShapefile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreateShapefile.Click
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim lWASPSegmentShapefile As String = ""
        pPlugIn.WASPProject.CreateSegmentShapeFile(pSegmentLayerIndex, lWASPSegmentShapefile)
        pPlugIn.WASPProject.CreateBufferedSegmentShapeFile(lWASPSegmentShapefile)
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Logger.Msg("Create Shapefile complete.", MsgBoxStyle.OkOnly, "Create Shapefile")
    End Sub

    Private Sub SetDates()
        'set dates on the general tab to the last common year of the selected timeseries       

        Dim lSJDate As Double = 0.0
        Dim lEJDate As Double = 0.0

        For Each lTimeseries As atcWASPTimeseries In pPlugIn.WASPProject.InputTimeseriesCollection
            If lTimeseries.SDate > lSJDate Then
                lSJDate = lTimeseries.SDate
            End If
            If lEJDate = 0.0 Or lTimeseries.EDate < lEJDate Then
                lEJDate = lTimeseries.EDate
            End If
        Next

        If lEJDate > lSJDate Then
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
        End If
    End Sub

    'Private Sub cmdGenerateTimeseries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Logger.Msg("Feature not yet implemented.", MsgBoxStyle.OkOnly, "Generate New Timeseries")
    'End Sub

    'Private Sub cmdVolumes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Logger.Msg("Feature not yet implemented.", MsgBoxStyle.OkOnly, "Volumes/Depths")
    'End Sub

    Private Sub cbxAir_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxAir.SelectedIndexChanged
        pPlugIn.WASPProject.RebuildTimeseriesCollections(cbxAir.SelectedItem, cbxSolar.SelectedItem, cbxWind.SelectedItem, AtcGridFlow.Source, AtcGridLoad.Source, AtcGridBound.Source)
        SetDates()
    End Sub

    Private Sub cbxSolar_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSolar.SelectedIndexChanged
        pPlugIn.WASPProject.RebuildTimeseriesCollections(cbxAir.SelectedItem, cbxSolar.SelectedItem, cbxWind.SelectedItem, AtcGridFlow.Source, AtcGridLoad.Source, AtcGridBound.Source)
        SetDates()
    End Sub

    Private Sub cbxWind_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxWind.SelectedIndexChanged
        pPlugIn.WASPProject.RebuildTimeseriesCollections(cbxAir.SelectedItem, cbxSolar.SelectedItem, cbxWind.SelectedItem, AtcGridFlow.Source, AtcGridLoad.Source, AtcGridBound.Source)
        SetDates()
    End Sub

    Private Sub AtcGridLoad_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridLoad.MouseDownCell
        pSelectedColumn = aColumn
        pSelectedRow = aRow
        SetLoadStationValidValues()
    End Sub

    Private Sub AtcGridBound_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridBound.MouseDownCell
        pSelectedColumn = aColumn
        pSelectedRow = aRow
        SetBoundaryValidValues()
    End Sub

    Private Sub cboMet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMet.SelectedIndexChanged
        SetStationCoordinates()
    End Sub

    'Public Sub ReadWASPConstituentNamesFromFile(ByVal aFileName As String)
    '    If FileExists(aFileName) Then
    '        For Each lString As String In LinesInFile(aFileName)
    '            If Not lString.StartsWith(";") Then
    '                pPlugIn.WASPProject.WASPConstituents.Add(lString.Trim)
    '            End If
    '        Next
    '    End If
    'End Sub

    'Public Sub WriteWASPConstituentNamesToFile(ByVal aFileName As String)
    '    Dim lStr As String = ""
    '    For Each lItem As String In pPlugIn.WASPProject.WASPConstituents
    '        lStr &= lItem.ToString & vbCrLf
    '    Next
    '    SaveFileString(aFileName, lStr)
    'End Sub

    Private Sub ReadWASPdb(ByVal aPathName As String)
        Dim lTable As New atcUtility.atcTableDelimited

        'read wasp models database
        pWASPModelsDB = New atcCollection
        If lTable.OpenFile(aPathName & "WASPModels.csv") Then
            For lRow As Integer = 1 To lTable.NumRecords
                lTable.CurrentRecord = lRow
                pWASPModelsDB.Add(lTable.Value(1), ReplaceString(lTable.Value(2), """", ""))
            Next
        End If

        'read wasp systems database
        pWASPSystemIdsDB = New atcCollection
        pWASPSystemNamesDB = New atcCollection
        If lTable.OpenFile(aPathName & "WASPSystems.csv") Then
            For lRow As Integer = 1 To lTable.NumRecords
                lTable.CurrentRecord = lRow
                pWASPSystemIdsDB.Add(lRow, lTable.Value(3))
                pWASPSystemNamesDB.Add(lRow, ReplaceString(lTable.Value(4), """", ""))
            Next
        End If

        'read wasp time functions database
        pWASPTimeFunctionIdsDB = New atcCollection
        pWASPTimeFunctionNamesDB = New atcCollection
        If lTable.OpenFile(aPathName & "WASPTimeFunctions.csv") Then
            For lRow As Integer = 1 To lTable.NumRecords
                lTable.CurrentRecord = lRow
                pWASPTimeFunctionIdsDB.Add(lRow, lTable.Value(2))
                pWASPTimeFunctionNamesDB.Add(lRow, ReplaceString(lTable.Value(4), """", ""))
            Next
        End If
    End Sub

    Private Sub SetStationCoordinates()
        With pPlugIn.WASPProject
            If cboMet.SelectedIndex > 0 Then
                Dim lMetLayerIndex As Integer = GisUtil.LayerIndex(cboMet.Items(cboMet.SelectedIndex))
                .GetMetStationCoordinates(lMetLayerIndex, .AirTempStationCandidates)
                .GetMetStationCoordinates(lMetLayerIndex, .AirTempStationCandidates)
                .GetMetStationCoordinates(lMetLayerIndex, .SolRadStationCandidates)
                .GetMetStationCoordinates(lMetLayerIndex, .SolRadStationCandidates)
                .GetMetStationCoordinates(lMetLayerIndex, .WindStationCandidates)
            End If
        End With
    End Sub

    Private Sub cboModel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboModel.SelectedIndexChanged
        Dim lModelId As String = pWASPModelsDB.Keys(cboModel.SelectedIndex)
        pPlugIn.WASPProject.ModelType = lModelId

        'set the load constituents to the wasp system names
        pPlugIn.WASPProject.WASPConstituents.Clear()
        For lIndex As Integer = 1 To pWASPSystemIdsDB.Count
            If pWASPSystemIdsDB(lIndex - 1) = lModelId Then
                pPlugIn.WASPProject.WASPConstituents.Add(pWASPSystemNamesDB(lIndex - 1))
            End If
        Next
        SetLoadStationGrid()
        SetBoundaryGrid()

        'set the time functions to the wasp time function names
        pPlugIn.WASPProject.WASPTimeFunctions.Clear()
        For lIndex As Integer = 1 To pWASPTimeFunctionIdsDB.Count
            If pWASPTimeFunctionIdsDB(lIndex - 1) = lModelId Then
                pPlugIn.WASPProject.WASPTimeFunctions.Add(pWASPTimeFunctionNamesDB(lIndex - 1))
            End If
        Next

    End Sub

End Class