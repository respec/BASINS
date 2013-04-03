<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWASPSetup
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnBuild As System.Windows.Forms.Button
    Friend WithEvents TabMain As System.Windows.Forms.TabControl
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
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents atxTravelTimeMax As atcControls.atcText
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents atxTravelTimeMin As atcControls.atcText
    Friend WithEvents cboModel As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents dgFlow As System.Windows.Forms.DataGridView
    Friend WithEvents dgBound As System.Windows.Forms.DataGridView
    Friend WithEvents dgLoad As System.Windows.Forms.DataGridView
    Friend WithEvents dgTime As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lnkCreateShapefile As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkFieldMapping As System.Windows.Forms.LinkLabel
    Friend WithEvents btnReselect As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWASPSetup))
        Me.btnClose = New System.Windows.Forms.Button()
        Me.TabMain = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.tbxName = New System.Windows.Forms.TextBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtOutputDir = New System.Windows.Forms.TextBox()
        Me.cboModel = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cboMet = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblEnd = New System.Windows.Forms.Label()
        Me.lblStart = New System.Windows.Forms.Label()
        Me.lblDay = New System.Windows.Forms.Label()
        Me.lblMonth = New System.Windows.Forms.Label()
        Me.lblYear = New System.Windows.Forms.Label()
        Me.atxEDay = New atcControls.atcText()
        Me.atxSDay = New atcControls.atcText()
        Me.atxSYear = New atcControls.atcText()
        Me.atxEMonth = New atcControls.atcText()
        Me.atxSMonth = New atcControls.atcText()
        Me.atxEYear = New atcControls.atcText()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.dgSegmentation = New System.Windows.Forms.DataGridView()
        Me.lnkCreateShapefile = New System.Windows.Forms.LinkLabel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnReselect = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.atxTravelTimeMin = New atcControls.atcText()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.atxTravelTimeMax = New atcControls.atcText()
        Me.lnkFieldMapping = New System.Windows.Forms.LinkLabel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnDeleteSegments = New System.Windows.Forms.Button()
        Me.btnCombineSegments = New System.Windows.Forms.Button()
        Me.btnDivideSegments = New System.Windows.Forms.Button()
        Me.lblNumSelected = New System.Windows.Forms.Label()
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboLabelLayer = New System.Windows.Forms.ComboBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.dgFlow = New System.Windows.Forms.DataGridView()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.dgBound = New System.Windows.Forms.DataGridView()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.dgLoad = New System.Windows.Forms.DataGridView()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.dgTime = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.btnBuild = New System.Windows.Forms.Button()
        Me.btnNew = New InstantUpdate.Controls.SplitButton()
        Me.mnuNew = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuNewSelect = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuNewProj = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnOpen = New InstantUpdate.Controls.SplitButton()
        Me.mnuOpen = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuOpenLast = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOpenProj = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnSave = New InstantUpdate.Controls.SplitButton()
        Me.mnuSave = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSaveProj = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnHelp = New InstantUpdate.Controls.SplitButton()
        Me.mnuHelp = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuHelpManual = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabMain.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.dgSegmentation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        CType(Me.dgFlow, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage5.SuspendLayout()
        CType(Me.dgBound, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage4.SuspendLayout()
        CType(Me.dgLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage6.SuspendLayout()
        CType(Me.dgTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.mnuNew.SuspendLayout()
        Me.mnuOpen.SuspendLayout()
        Me.mnuSave.SuspendLayout()
        Me.mnuHelp.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(530, 388)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(73, 28)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        '
        'TabMain
        '
        Me.TabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabMain.Controls.Add(Me.TabPage1)
        Me.TabMain.Controls.Add(Me.TabPage2)
        Me.TabMain.Controls.Add(Me.TabPage3)
        Me.TabMain.Controls.Add(Me.TabPage5)
        Me.TabMain.Controls.Add(Me.TabPage4)
        Me.TabMain.Controls.Add(Me.TabPage6)
        Me.TabMain.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabMain.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabMain.HotTrack = True
        Me.TabMain.ItemSize = New System.Drawing.Size(60, 21)
        Me.TabMain.Location = New System.Drawing.Point(12, 12)
        Me.TabMain.Name = "TabMain"
        Me.TabMain.SelectedIndex = 0
        Me.TabMain.Size = New System.Drawing.Size(591, 308)
        Me.TabMain.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.Transparent
        Me.TabPage1.Controls.Add(Me.TableLayoutPanel4)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(583, 279)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "General"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 3
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel4.Controls.Add(Me.tbxName, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.btnBrowse, 2, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.txtOutputDir, 1, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.cboModel, 1, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.Label3, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.cboMet, 1, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.Label4, 0, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.Label9, 0, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.GroupBox3, 0, 5)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.Padding = New System.Windows.Forms.Padding(3)
        Me.TableLayoutPanel4.RowCount = 6
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(583, 279)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'tbxName
        '
        Me.tbxName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel4.SetColumnSpan(Me.tbxName, 2)
        Me.tbxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.tbxName, "WASP project name (which defaults to the name of the BASINS project)")
        Me.tbxName.Location = New System.Drawing.Point(121, 6)
        Me.tbxName.Name = "tbxName"
        Me.HelpProvider1.SetShowHelp(Me.tbxName, True)
        Me.tbxName.Size = New System.Drawing.Size(456, 20)
        Me.tbxName.TabIndex = 1
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Location = New System.Drawing.Point(502, 32)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 4
        Me.btnBrowse.Text = "Bro&wse..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        Me.btnBrowse.Visible = False
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "WASP &Project Name:"
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.txtOutputDir, "Name of directory where generated .wnf and .inp files will be stored.")
        Me.txtOutputDir.Location = New System.Drawing.Point(121, 33)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.HelpProvider1.SetShowHelp(Me.txtOutputDir, True)
        Me.txtOutputDir.Size = New System.Drawing.Size(375, 20)
        Me.txtOutputDir.TabIndex = 3
        Me.txtOutputDir.Visible = False
        '
        'cboModel
        '
        Me.cboModel.AllowDrop = True
        Me.cboModel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel4.SetColumnSpan(Me.cboModel, 2)
        Me.cboModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.cboModel, "Type of WASP model to create.")
        Me.cboModel.Location = New System.Drawing.Point(121, 61)
        Me.cboModel.Name = "cboModel"
        Me.HelpProvider1.SetShowHelp(Me.cboModel, True)
        Me.cboModel.Size = New System.Drawing.Size(456, 21)
        Me.cboModel.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Output &Directory:"
        Me.Label3.Visible = False
        '
        'cboMet
        '
        Me.cboMet.AllowDrop = True
        Me.cboMet.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel4.SetColumnSpan(Me.cboMet, 2)
        Me.cboMet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.cboMet, "BASINS met stations layer (usually called Weather Station Sites 2006 in BASINS). " & _
        "The Met Stations Layer field will default to the Weather Station Sites if they h" & _
        "ave been downloaded for this project. ")
        Me.cboMet.Location = New System.Drawing.Point(121, 88)
        Me.cboMet.Name = "cboMet"
        Me.HelpProvider1.SetShowHelp(Me.cboMet, True)
        Me.cboMet.Size = New System.Drawing.Size(456, 21)
        Me.cboMet.TabIndex = 8
        Me.cboMet.Visible = False
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "WASP Model &Type:"
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(6, 92)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(98, 13)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "&Met Stations Layer:"
        Me.Label9.Visible = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TableLayoutPanel4.SetColumnSpan(Me.GroupBox3, 3)
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
        Me.GroupBox3.Location = New System.Drawing.Point(115, 185)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(353, 88)
        Me.GroupBox3.TabIndex = 9
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Simulation Dates"
        '
        'lblEnd
        '
        Me.lblEnd.AutoSize = True
        Me.lblEnd.Location = New System.Drawing.Point(92, 62)
        Me.lblEnd.Name = "lblEnd"
        Me.lblEnd.Size = New System.Drawing.Size(29, 13)
        Me.lblEnd.TabIndex = 7
        Me.lblEnd.Text = "&End:"
        '
        'lblStart
        '
        Me.lblStart.AutoSize = True
        Me.lblStart.Location = New System.Drawing.Point(89, 36)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(35, 13)
        Me.lblStart.TabIndex = 3
        Me.lblStart.Text = "Sta&rt:"
        '
        'lblDay
        '
        Me.lblDay.AutoSize = True
        Me.lblDay.Location = New System.Drawing.Point(242, 14)
        Me.lblDay.Name = "lblDay"
        Me.lblDay.Size = New System.Drawing.Size(26, 13)
        Me.lblDay.TabIndex = 2
        Me.lblDay.Text = "Day"
        '
        'lblMonth
        '
        Me.lblMonth.AutoSize = True
        Me.lblMonth.Location = New System.Drawing.Point(193, 14)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.Size = New System.Drawing.Size(37, 13)
        Me.lblMonth.TabIndex = 1
        Me.lblMonth.Text = "Month"
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Location = New System.Drawing.Point(125, 14)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(29, 13)
        Me.lblYear.TabIndex = 0
        Me.lblYear.Text = "Year"
        '
        'atxEDay
        '
        Me.atxEDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxEDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxEDay.DefaultValue = ""
        Me.atxEDay.HardMax = 31.0R
        Me.atxEDay.HardMin = 1.0R
        Me.HelpProvider1.SetHelpString(Me.atxEDay, "Date associated with simulation date (1-31)")
        Me.atxEDay.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxEDay.Location = New System.Drawing.Point(245, 58)
        Me.atxEDay.MaxWidth = 20
        Me.atxEDay.Name = "atxEDay"
        Me.atxEDay.NumericFormat = "0"
        Me.atxEDay.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxEDay.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxEDay.SelLength = 0
        Me.atxEDay.SelStart = 0
        Me.HelpProvider1.SetShowHelp(Me.atxEDay, True)
        Me.atxEDay.Size = New System.Drawing.Size(44, 21)
        Me.atxEDay.SoftMax = -999.0R
        Me.atxEDay.SoftMin = -999.0R
        Me.atxEDay.TabIndex = 10
        Me.atxEDay.ValueDouble = 31.0R
        Me.atxEDay.ValueInteger = 31
        '
        'atxSDay
        '
        Me.atxSDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSDay.DefaultValue = ""
        Me.atxSDay.HardMax = 31.0R
        Me.atxSDay.HardMin = 1.0R
        Me.HelpProvider1.SetHelpString(Me.atxSDay, "Date associated with simulation date (1-31)")
        Me.atxSDay.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSDay.Location = New System.Drawing.Point(245, 32)
        Me.atxSDay.MaxWidth = 20
        Me.atxSDay.Name = "atxSDay"
        Me.atxSDay.NumericFormat = "0"
        Me.atxSDay.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSDay.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSDay.SelLength = 0
        Me.atxSDay.SelStart = 0
        Me.HelpProvider1.SetShowHelp(Me.atxSDay, True)
        Me.atxSDay.Size = New System.Drawing.Size(44, 21)
        Me.atxSDay.SoftMax = -999.0R
        Me.atxSDay.SoftMin = -999.0R
        Me.atxSDay.TabIndex = 6
        Me.atxSDay.ValueDouble = 1.0R
        Me.atxSDay.ValueInteger = 1
        '
        'atxSYear
        '
        Me.atxSYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSYear.DefaultValue = ""
        Me.atxSYear.HardMax = 9999.0R
        Me.atxSYear.HardMin = 0.0R
        Me.HelpProvider1.SetHelpString(Me.atxSYear, "Four-digit year associated with simulation date")
        Me.atxSYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSYear.Location = New System.Drawing.Point(127, 32)
        Me.atxSYear.MaxWidth = 20
        Me.atxSYear.Name = "atxSYear"
        Me.atxSYear.NumericFormat = "0"
        Me.atxSYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSYear.SelLength = 0
        Me.atxSYear.SelStart = 0
        Me.HelpProvider1.SetShowHelp(Me.atxSYear, True)
        Me.atxSYear.Size = New System.Drawing.Size(64, 21)
        Me.atxSYear.SoftMax = -999.0R
        Me.atxSYear.SoftMin = -999.0R
        Me.atxSYear.TabIndex = 4
        Me.atxSYear.ValueDouble = 2000.0R
        Me.atxSYear.ValueInteger = 2000
        '
        'atxEMonth
        '
        Me.atxEMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxEMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxEMonth.DefaultValue = ""
        Me.atxEMonth.HardMax = 12.0R
        Me.atxEMonth.HardMin = 1.0R
        Me.HelpProvider1.SetHelpString(Me.atxEMonth, "Month associated with simulation date (1-12)")
        Me.atxEMonth.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxEMonth.Location = New System.Drawing.Point(196, 58)
        Me.atxEMonth.MaxWidth = 20
        Me.atxEMonth.Name = "atxEMonth"
        Me.atxEMonth.NumericFormat = "0"
        Me.atxEMonth.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxEMonth.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxEMonth.SelLength = 0
        Me.atxEMonth.SelStart = 0
        Me.HelpProvider1.SetShowHelp(Me.atxEMonth, True)
        Me.atxEMonth.Size = New System.Drawing.Size(44, 21)
        Me.atxEMonth.SoftMax = -999.0R
        Me.atxEMonth.SoftMin = -999.0R
        Me.atxEMonth.TabIndex = 9
        Me.atxEMonth.ValueDouble = 12.0R
        Me.atxEMonth.ValueInteger = 12
        '
        'atxSMonth
        '
        Me.atxSMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSMonth.DefaultValue = ""
        Me.atxSMonth.HardMax = 12.0R
        Me.atxSMonth.HardMin = 1.0R
        Me.HelpProvider1.SetHelpString(Me.atxSMonth, "Month associated with simulation date (1-12)")
        Me.atxSMonth.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSMonth.Location = New System.Drawing.Point(196, 32)
        Me.atxSMonth.MaxWidth = 20
        Me.atxSMonth.Name = "atxSMonth"
        Me.atxSMonth.NumericFormat = "0"
        Me.atxSMonth.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSMonth.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSMonth.SelLength = 0
        Me.atxSMonth.SelStart = 0
        Me.HelpProvider1.SetShowHelp(Me.atxSMonth, True)
        Me.atxSMonth.Size = New System.Drawing.Size(44, 21)
        Me.atxSMonth.SoftMax = -999.0R
        Me.atxSMonth.SoftMin = -999.0R
        Me.atxSMonth.TabIndex = 5
        Me.atxSMonth.ValueDouble = 1.0R
        Me.atxSMonth.ValueInteger = 1
        '
        'atxEYear
        '
        Me.atxEYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxEYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxEYear.DefaultValue = ""
        Me.atxEYear.HardMax = 9999.0R
        Me.atxEYear.HardMin = 0.0R
        Me.HelpProvider1.SetHelpString(Me.atxEYear, "Four-digit year associated with simulation date")
        Me.atxEYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxEYear.Location = New System.Drawing.Point(127, 58)
        Me.atxEYear.MaxWidth = 20
        Me.atxEYear.Name = "atxEYear"
        Me.atxEYear.NumericFormat = "0"
        Me.atxEYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxEYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxEYear.SelLength = 0
        Me.atxEYear.SelStart = 0
        Me.HelpProvider1.SetShowHelp(Me.atxEYear, True)
        Me.atxEYear.Size = New System.Drawing.Size(64, 21)
        Me.atxEYear.SoftMax = -999.0R
        Me.atxEYear.SoftMin = -999.0R
        Me.atxEYear.TabIndex = 8
        Me.atxEYear.ValueDouble = 2000.0R
        Me.atxEYear.ValueInteger = 2000
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(583, 279)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Segmentation"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.dgSegmentation, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lnkCreateShapefile, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lnkFieldMapping, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel5, 0, 3)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(583, 279)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'dgSegmentation
        '
        Me.dgSegmentation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgSegmentation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgSegmentation.Location = New System.Drawing.Point(3, 93)
        Me.dgSegmentation.Name = "dgSegmentation"
        Me.dgSegmentation.Size = New System.Drawing.Size(577, 74)
        Me.dgSegmentation.TabIndex = 1
        '
        'lnkCreateShapefile
        '
        Me.lnkCreateShapefile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lnkCreateShapefile.AutoSize = True
        Me.HelpProvider1.SetHelpString(Me.lnkCreateShapefile, "Click this link to have a new layer created comprised of a ""thick"" lines that fac" & _
        "ilitate WASP visualization.")
        Me.lnkCreateShapefile.Location = New System.Drawing.Point(3, 260)
        Me.lnkCreateShapefile.Name = "lnkCreateShapefile"
        Me.lnkCreateShapefile.Padding = New System.Windows.Forms.Padding(3)
        Me.HelpProvider1.SetShowHelp(Me.lnkCreateShapefile, True)
        Me.lnkCreateShapefile.Size = New System.Drawing.Size(474, 19)
        Me.lnkCreateShapefile.TabIndex = 5
        Me.lnkCreateShapefile.TabStop = True
        Me.lnkCreateShapefile.Text = "Create shapefile from selected stream segments using buffered polygons for WASP v" & _
    "isualization"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.btnReselect, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.GroupBox2, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(577, 84)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'btnReselect
        '
        Me.btnReselect.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.HelpProvider1.SetHelpString(Me.btnReselect, "This will redisplay the WASP Initialization form where you can reselect the segme" & _
        "nts. Note that this will replace all information on this form.")
        Me.btnReselect.Image = Global.atcWASPPlugIn.My.Resources.Resources.PropertiesHS
        Me.btnReselect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReselect.Location = New System.Drawing.Point(3, 23)
        Me.btnReselect.Name = "btnReselect"
        Me.HelpProvider1.SetShowHelp(Me.btnReselect, True)
        Me.btnReselect.Size = New System.Drawing.Size(166, 38)
        Me.btnReselect.TabIndex = 0
        Me.btnReselect.Text = "Reselect Segments on &Map"
        Me.btnReselect.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReselect.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.GroupBox2.Controls.Add(Me.btnGenerate)
        Me.GroupBox2.Controls.Add(Me.atxTravelTimeMin)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.atxTravelTimeMax)
        Me.GroupBox2.Location = New System.Drawing.Point(249, 5)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(325, 73)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Regenerate Segmentation Based on Travel Time"
        '
        'btnGenerate
        '
        Me.HelpProvider1.SetHelpString(Me.btnGenerate, resources.GetString("btnGenerate.HelpString"))
        Me.btnGenerate.Image = Global.atcWASPPlugIn.My.Resources.Resources.RefreshDocViewHS
        Me.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerate.Location = New System.Drawing.Point(223, 23)
        Me.btnGenerate.Name = "btnGenerate"
        Me.HelpProvider1.SetShowHelp(Me.btnGenerate, True)
        Me.btnGenerate.Size = New System.Drawing.Size(90, 38)
        Me.btnGenerate.TabIndex = 3
        Me.btnGenerate.Text = "&Regenerate"
        Me.btnGenerate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'atxTravelTimeMin
        '
        Me.atxTravelTimeMin.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxTravelTimeMin.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxTravelTimeMin.DefaultValue = ""
        Me.atxTravelTimeMin.HardMax = -999.0R
        Me.atxTravelTimeMin.HardMin = 0.0R
        Me.HelpProvider1.SetHelpString(Me.atxTravelTimeMin, "This is the minimum amount of travel time (in days) that you want to have associa" & _
        "ted with segments; segments with travel times less than this will be combined wh" & _
        "en you click ""Regenerate"".")
        Me.atxTravelTimeMin.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxTravelTimeMin.Location = New System.Drawing.Point(168, 44)
        Me.atxTravelTimeMin.MaxWidth = 20
        Me.atxTravelTimeMin.Name = "atxTravelTimeMin"
        Me.atxTravelTimeMin.NumericFormat = "0.#####"
        Me.atxTravelTimeMin.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxTravelTimeMin.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxTravelTimeMin.SelLength = 0
        Me.atxTravelTimeMin.SelStart = 0
        Me.HelpProvider1.SetShowHelp(Me.atxTravelTimeMin, True)
        Me.atxTravelTimeMin.Size = New System.Drawing.Size(40, 21)
        Me.atxTravelTimeMin.SoftMax = -999.0R
        Me.atxTravelTimeMin.SoftMin = -999.0R
        Me.atxTravelTimeMin.TabIndex = 4
        Me.atxTravelTimeMin.ValueDouble = 0.0R
        Me.atxTravelTimeMin.ValueInteger = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(147, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Ma&ximum Travel Time (days):"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 47)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(143, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "M&inimum Travel Time (days):"
        '
        'atxTravelTimeMax
        '
        Me.atxTravelTimeMax.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxTravelTimeMax.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxTravelTimeMax.DefaultValue = "0"
        Me.atxTravelTimeMax.HardMax = -999.0R
        Me.atxTravelTimeMax.HardMin = 0.0R
        Me.HelpProvider1.SetHelpString(Me.atxTravelTimeMax, "This is the maximum amount of travel time (in days) that you want to have associa" & _
        "ted with segments; segments with travel times longer than this will be subdivide" & _
        "d when you click ""Regenerate"".")
        Me.atxTravelTimeMax.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxTravelTimeMax.Location = New System.Drawing.Point(168, 20)
        Me.atxTravelTimeMax.MaxWidth = 20
        Me.atxTravelTimeMax.Name = "atxTravelTimeMax"
        Me.atxTravelTimeMax.NumericFormat = "0.#####"
        Me.atxTravelTimeMax.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxTravelTimeMax.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxTravelTimeMax.SelLength = 1
        Me.atxTravelTimeMax.SelStart = 0
        Me.HelpProvider1.SetShowHelp(Me.atxTravelTimeMax, True)
        Me.atxTravelTimeMax.Size = New System.Drawing.Size(40, 21)
        Me.atxTravelTimeMax.SoftMax = -999.0R
        Me.atxTravelTimeMax.SoftMin = -999.0R
        Me.atxTravelTimeMax.TabIndex = 1
        Me.atxTravelTimeMax.ValueDouble = 0.0R
        Me.atxTravelTimeMax.ValueInteger = 0
        '
        'lnkFieldMapping
        '
        Me.lnkFieldMapping.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lnkFieldMapping.AutoSize = True
        Me.HelpProvider1.SetHelpString(Me.lnkFieldMapping, "Click this link if you are using a non-standard layer and need to manually specif" & _
        "y field mapping.")
        Me.lnkFieldMapping.Location = New System.Drawing.Point(3, 241)
        Me.lnkFieldMapping.Name = "lnkFieldMapping"
        Me.lnkFieldMapping.Padding = New System.Windows.Forms.Padding(3)
        Me.HelpProvider1.SetShowHelp(Me.lnkFieldMapping, True)
        Me.lnkFieldMapping.Size = New System.Drawing.Size(393, 19)
        Me.lnkFieldMapping.TabIndex = 4
        Me.lnkFieldMapping.TabStop = True
        Me.lnkFieldMapping.Text = "Define field mapping between specified streams layer and WASP data structure"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.ColumnCount = 4
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.Controls.Add(Me.btnDeleteSegments, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.btnCombineSegments, 3, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.btnDivideSegments, 2, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.lblNumSelected, 0, 0)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 173)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(577, 33)
        Me.TableLayoutPanel3.TabIndex = 2
        '
        'btnDeleteSegments
        '
        Me.btnDeleteSegments.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteSegments.AutoSize = True
        Me.btnDeleteSegments.CausesValidation = False
        Me.btnDeleteSegments.Image = Global.atcWASPPlugIn.My.Resources.Resources.deletehs
        Me.btnDeleteSegments.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDeleteSegments.Location = New System.Drawing.Point(187, 5)
        Me.btnDeleteSegments.Name = "btnDeleteSegments"
        Me.btnDeleteSegments.Size = New System.Drawing.Size(125, 23)
        Me.btnDeleteSegments.TabIndex = 1
        Me.btnDeleteSegments.Text = "&Delete Segments"
        Me.btnDeleteSegments.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDeleteSegments.UseVisualStyleBackColor = True
        '
        'btnCombineSegments
        '
        Me.btnCombineSegments.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCombineSegments.AutoSize = True
        Me.btnCombineSegments.CausesValidation = False
        Me.btnCombineSegments.Image = Global.atcWASPPlugIn.My.Resources.Resources.Combine
        Me.btnCombineSegments.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCombineSegments.Location = New System.Drawing.Point(449, 5)
        Me.btnCombineSegments.Name = "btnCombineSegments"
        Me.btnCombineSegments.Size = New System.Drawing.Size(125, 23)
        Me.btnCombineSegments.TabIndex = 3
        Me.btnCombineSegments.Text = "&Combine Segments"
        Me.btnCombineSegments.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCombineSegments.UseVisualStyleBackColor = True
        '
        'btnDivideSegments
        '
        Me.btnDivideSegments.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDivideSegments.AutoSize = True
        Me.btnDivideSegments.CausesValidation = False
        Me.btnDivideSegments.Image = Global.atcWASPPlugIn.My.Resources.Resources.Split
        Me.btnDivideSegments.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDivideSegments.Location = New System.Drawing.Point(318, 5)
        Me.btnDivideSegments.Name = "btnDivideSegments"
        Me.btnDivideSegments.Size = New System.Drawing.Size(125, 23)
        Me.btnDivideSegments.TabIndex = 2
        Me.btnDivideSegments.Text = "Di&vide Segments"
        Me.btnDivideSegments.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDivideSegments.UseVisualStyleBackColor = True
        '
        'lblNumSelected
        '
        Me.lblNumSelected.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblNumSelected.AutoSize = True
        Me.lblNumSelected.Location = New System.Drawing.Point(3, 10)
        Me.lblNumSelected.Name = "lblNumSelected"
        Me.lblNumSelected.Size = New System.Drawing.Size(107, 13)
        Me.lblNumSelected.TabIndex = 0
        Me.lblNumSelected.Text = "0 Segments Selected"
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel5.ColumnCount = 2
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.Label5, 0, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.cboLabelLayer, 1, 0)
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(3, 212)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 1
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(577, 26)
        Me.TableLayoutPanel5.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 6)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(180, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Auto-&label segments using this field:"
        '
        'cboLabelLayer
        '
        Me.cboLabelLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cboLabelLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLabelLayer.FormattingEnabled = True
        Me.cboLabelLayer.Location = New System.Drawing.Point(189, 3)
        Me.cboLabelLayer.Name = "cboLabelLayer"
        Me.cboLabelLayer.Size = New System.Drawing.Size(133, 21)
        Me.cboLabelLayer.TabIndex = 1
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.dgFlow)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(5)
        Me.TabPage3.Size = New System.Drawing.Size(583, 279)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Flows"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'dgFlow
        '
        Me.dgFlow.AllowUserToAddRows = False
        Me.dgFlow.AllowUserToDeleteRows = False
        Me.dgFlow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgFlow.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HelpProvider1.SetHelpString(Me.dgFlow, resources.GetString("dgFlow.HelpString"))
        Me.dgFlow.Location = New System.Drawing.Point(5, 5)
        Me.dgFlow.Name = "dgFlow"
        Me.HelpProvider1.SetShowHelp(Me.dgFlow, True)
        Me.dgFlow.Size = New System.Drawing.Size(573, 269)
        Me.dgFlow.TabIndex = 0
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.dgBound)
        Me.TabPage5.Location = New System.Drawing.Point(4, 25)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(5)
        Me.TabPage5.Size = New System.Drawing.Size(583, 279)
        Me.TabPage5.TabIndex = 3
        Me.TabPage5.Text = "Boundaries"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'dgBound
        '
        Me.dgBound.AllowUserToAddRows = False
        Me.dgBound.AllowUserToDeleteRows = False
        Me.dgBound.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgBound.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HelpProvider1.SetHelpString(Me.dgBound, resources.GetString("dgBound.HelpString"))
        Me.dgBound.Location = New System.Drawing.Point(5, 5)
        Me.dgBound.Name = "dgBound"
        Me.HelpProvider1.SetShowHelp(Me.dgBound, True)
        Me.dgBound.Size = New System.Drawing.Size(573, 269)
        Me.dgBound.TabIndex = 0
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.dgLoad)
        Me.TabPage4.Location = New System.Drawing.Point(4, 25)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(5)
        Me.TabPage4.Size = New System.Drawing.Size(583, 279)
        Me.TabPage4.TabIndex = 4
        Me.TabPage4.Text = "Loads"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'dgLoad
        '
        Me.dgLoad.AllowUserToAddRows = False
        Me.dgLoad.AllowUserToDeleteRows = False
        Me.dgLoad.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgLoad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HelpProvider1.SetHelpString(Me.dgLoad, resources.GetString("dgLoad.HelpString"))
        Me.dgLoad.Location = New System.Drawing.Point(5, 5)
        Me.dgLoad.Name = "dgLoad"
        Me.HelpProvider1.SetShowHelp(Me.dgLoad, True)
        Me.dgLoad.Size = New System.Drawing.Size(573, 269)
        Me.dgLoad.TabIndex = 0
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.dgTime)
        Me.TabPage6.Location = New System.Drawing.Point(4, 25)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(5)
        Me.TabPage6.Size = New System.Drawing.Size(583, 279)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Time Functions"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'dgTime
        '
        Me.dgTime.AllowUserToAddRows = False
        Me.dgTime.AllowUserToDeleteRows = False
        Me.dgTime.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgTime.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HelpProvider1.SetHelpString(Me.dgTime, resources.GetString("dgTime.HelpString"))
        Me.dgTime.Location = New System.Drawing.Point(5, 5)
        Me.dgTime.Name = "dgTime"
        Me.HelpProvider1.SetShowHelp(Me.dgTime, True)
        Me.dgTime.Size = New System.Drawing.Size(573, 269)
        Me.dgTime.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblStatus)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(15, 326)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(588, 48)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Status"
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(13, 16)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(563, 29)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Update specifications if desired, then click 'Build WASP File' to proceed."
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnBuild
        '
        Me.btnBuild.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBuild.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.btnBuild, "After you specify all input parameters, this will build the WASP input file, star" & _
        "t the WASP program, and load the data.")
        Me.btnBuild.Image = Global.atcWASPPlugIn.My.Resources.Resources.PlayHS
        Me.btnBuild.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBuild.Location = New System.Drawing.Point(415, 388)
        Me.btnBuild.Name = "btnBuild"
        Me.HelpProvider1.SetShowHelp(Me.btnBuild, True)
        Me.btnBuild.Size = New System.Drawing.Size(109, 28)
        Me.btnBuild.TabIndex = 6
        Me.btnBuild.Text = "&Build WASP File"
        Me.btnBuild.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnNew
        '
        Me.btnNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNew.AutoSize = True
        Me.btnNew.ContextMenuStrip = Me.mnuNew
        Me.HelpProvider1.SetHelpString(Me.btnNew, "Create a new project; for other options, click the down arrow to the right")
        Me.btnNew.Image = Global.atcWASPPlugIn.My.Resources.Resources.NewCardHS
        Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNew.Location = New System.Drawing.Point(12, 388)
        Me.btnNew.Name = "btnNew"
        Me.HelpProvider1.SetShowHelp(Me.btnNew, True)
        Me.btnNew.Size = New System.Drawing.Size(75, 28)
        Me.btnNew.SplitMenuStrip = Me.mnuNew
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "&New"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'mnuNew
        '
        Me.mnuNew.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNewSelect, Me.mnuNewProj})
        Me.mnuNew.Name = "mnuNew"
        Me.mnuNew.Size = New System.Drawing.Size(236, 48)
        '
        'mnuNewSelect
        '
        Me.mnuNewSelect.Image = Global.atcWASPPlugIn.My.Resources.Resources.NewCardHS
        Me.mnuNewSelect.Name = "mnuNewSelect"
        Me.mnuNewSelect.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuNewSelect.Size = New System.Drawing.Size(235, 22)
        Me.mnuNewSelect.Text = "New Project - Reselect"
        '
        'mnuNewProj
        '
        Me.mnuNewProj.Image = Global.atcWASPPlugIn.My.Resources.Resources.NewDocumentHS
        Me.mnuNewProj.Name = "mnuNewProj"
        Me.mnuNewProj.Size = New System.Drawing.Size(235, 22)
        Me.mnuNewProj.Text = "New Project"
        '
        'btnOpen
        '
        Me.btnOpen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOpen.AutoSize = True
        Me.btnOpen.ContextMenuStrip = Me.mnuOpen
        Me.HelpProvider1.SetHelpString(Me.btnOpen, "Open your last project; for other options, click the down arrow to the right")
        Me.btnOpen.Image = Global.atcWASPPlugIn.My.Resources.Resources.OpenSelectedItemHS
        Me.btnOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOpen.Location = New System.Drawing.Point(93, 388)
        Me.btnOpen.Name = "btnOpen"
        Me.HelpProvider1.SetShowHelp(Me.btnOpen, True)
        Me.btnOpen.Size = New System.Drawing.Size(75, 28)
        Me.btnOpen.SplitMenuStrip = Me.mnuOpen
        Me.btnOpen.TabIndex = 3
        Me.btnOpen.Text = "&Open"
        Me.btnOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'mnuOpen
        '
        Me.mnuOpen.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOpenLast, Me.mnuOpenProj})
        Me.mnuOpen.Name = "mnuOpen"
        Me.mnuOpen.Size = New System.Drawing.Size(208, 48)
        '
        'mnuOpenLast
        '
        Me.mnuOpenLast.Image = Global.atcWASPPlugIn.My.Resources.Resources.OpenSelectedItemHS
        Me.mnuOpenLast.Name = "mnuOpenLast"
        Me.mnuOpenLast.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.mnuOpenLast.Size = New System.Drawing.Size(207, 22)
        Me.mnuOpenLast.Text = "Open Last Project"
        '
        'mnuOpenProj
        '
        Me.mnuOpenProj.Image = Global.atcWASPPlugIn.My.Resources.Resources.openHS
        Me.mnuOpenProj.Name = "mnuOpenProj"
        Me.mnuOpenProj.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuOpenProj.Size = New System.Drawing.Size(207, 22)
        Me.mnuOpenProj.Text = "Open Project..."
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSave.AutoSize = True
        Me.btnSave.ContextMenuStrip = Me.mnuSave
        Me.HelpProvider1.SetHelpString(Me.btnSave, "Save your project; for other options, click the down arrow to the right")
        Me.btnSave.Image = Global.atcWASPPlugIn.My.Resources.Resources.saveHS
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(174, 388)
        Me.btnSave.Name = "btnSave"
        Me.HelpProvider1.SetShowHelp(Me.btnSave, True)
        Me.btnSave.Size = New System.Drawing.Size(75, 27)
        Me.btnSave.SplitMenuStrip = Me.mnuSave
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'mnuSave
        '
        Me.mnuSave.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSaveProj, Me.mnuSaveAs})
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.Size = New System.Drawing.Size(206, 48)
        '
        'mnuSaveProj
        '
        Me.mnuSaveProj.Image = Global.atcWASPPlugIn.My.Resources.Resources.saveHS
        Me.mnuSaveProj.Name = "mnuSaveProj"
        Me.mnuSaveProj.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuSaveProj.Size = New System.Drawing.Size(205, 22)
        Me.mnuSaveProj.Text = "Save Project"
        '
        'mnuSaveAs
        '
        Me.mnuSaveAs.Image = Global.atcWASPPlugIn.My.Resources.Resources.SaveAllHS
        Me.mnuSaveAs.Name = "mnuSaveAs"
        Me.mnuSaveAs.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.mnuSaveAs.Size = New System.Drawing.Size(205, 22)
        Me.mnuSaveAs.Text = "Save Project As..."
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnHelp.AutoSize = True
        Me.btnHelp.ContextMenuStrip = Me.mnuHelp
        Me.HelpProvider1.SetHelpString(Me.btnHelp, "Display the Help Manual; for other options, click the down arrow to the right")
        Me.btnHelp.Image = Global.atcWASPPlugIn.My.Resources.Resources.Help
        Me.btnHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnHelp.Location = New System.Drawing.Point(294, 388)
        Me.btnHelp.Name = "btnHelp"
        Me.HelpProvider1.SetShowHelp(Me.btnHelp, True)
        Me.btnHelp.Size = New System.Drawing.Size(75, 28)
        Me.btnHelp.SplitMenuStrip = Me.mnuHelp
        Me.btnHelp.TabIndex = 5
        Me.btnHelp.Text = "&Help"
        Me.btnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'mnuHelp
        '
        Me.mnuHelp.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpManual, Me.mnuHelpAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(220, 48)
        '
        'mnuHelpManual
        '
        Me.mnuHelpManual.Image = Global.atcWASPPlugIn.My.Resources.Resources.Help
        Me.mnuHelpManual.Name = "mnuHelpManual"
        Me.mnuHelpManual.Size = New System.Drawing.Size(219, 22)
        Me.mnuHelpManual.Text = "Help Manual"
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Image = Global.atcWASPPlugIn.My.Resources.Resources.TextboxHS
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(219, 22)
        Me.mnuHelpAbout.Text = "About WASP Model Builder"
        '
        'frmWASPSetup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(615, 431)
        Me.Controls.Add(Me.TabMain)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnBuild)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.GroupBox1)
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(631, 469)
        Me.Name = "frmWASPSetup"
        Me.Text = "BASINS WASP Model Builder"
        Me.TabMain.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.dgSegmentation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TableLayoutPanel5.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        CType(Me.dgFlow, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage5.ResumeLayout(False)
        CType(Me.dgBound, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage4.ResumeLayout(False)
        CType(Me.dgLoad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage6.ResumeLayout(False)
        CType(Me.dgTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.mnuNew.ResumeLayout(False)
        Me.mnuOpen.ResumeLayout(False)
        Me.mnuSave.ResumeLayout(False)
        Me.mnuHelp.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnNew As InstantUpdate.Controls.SplitButton
    Friend WithEvents mnuNew As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuNewProj As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewSelect As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnOpen As InstantUpdate.Controls.SplitButton
    Friend WithEvents mnuOpen As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuOpenLast As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOpenProj As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnSave As InstantUpdate.Controls.SplitButton
    Friend WithEvents mnuSave As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSaveProj As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSaveAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnHelp As InstantUpdate.Controls.SplitButton
    Friend WithEvents mnuHelp As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuHelpManual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dgSegmentation As System.Windows.Forms.DataGridView
    Friend WithEvents btnCombineSegments As System.Windows.Forms.Button
    Friend WithEvents btnDivideSegments As System.Windows.Forms.Button
    Friend WithEvents btnDeleteSegments As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblNumSelected As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cboLabelLayer As System.Windows.Forms.ComboBox

End Class
