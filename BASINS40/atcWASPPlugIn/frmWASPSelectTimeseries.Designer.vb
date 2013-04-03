<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWASPSelectTimeSeries
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWASPSelectTimeSeries))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.btnOK = New System.Windows.Forms.Button
        Me.lblRecordCount = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.btnApplyAll = New System.Windows.Forms.Button
        Me.TabMain = New System.Windows.Forms.TabControl
        Me.tpConstant = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtConstant = New System.Windows.Forms.TextBox
        Me.tpDatabase = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.lstConversion = New System.Windows.Forms.ListBox
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.cboProjects = New System.Windows.Forms.ComboBox
        Me.optWRDB = New System.Windows.Forms.RadioButton
        Me.optDatabase = New System.Windows.Forms.RadioButton
        Me.txtFilename = New System.Windows.Forms.TextBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lstTables = New System.Windows.Forms.ListBox
        Me.lstStations = New System.Windows.Forms.ListBox
        Me.lstPCodes = New System.Windows.Forms.ListBox
        Me.chkMapping = New System.Windows.Forms.CheckBox
        Me.btnView = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtScaleFactor = New System.Windows.Forms.TextBox
        Me.tpMappings = New System.Windows.Forms.TabPage
        Me.TabMapping = New System.Windows.Forms.TabControl
        Me.tpStations = New System.Windows.Forms.TabPage
        Me.dgStationMapping = New System.Windows.Forms.DataGridView
        Me.Segment = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Station = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.tpPCodes = New System.Windows.Forms.TabPage
        Me.dgPCodeMapping = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Conversion = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ScaleFactor = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.lblMult = New System.Windows.Forms.Label
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.lnkImportMappings = New System.Windows.Forms.LinkLabel
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TabMain.SuspendLayout()
        Me.tpConstant.SuspendLayout()
        Me.TableLayoutPanel7.SuspendLayout()
        Me.tpDatabase.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.tpMappings.SuspendLayout()
        Me.TabMapping.SuspendLayout()
        Me.tpStations.SuspendLayout()
        CType(Me.dgStationMapping, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpPCodes.SuspendLayout()
        CType(Me.dgPCodeMapping, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 6
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.Controls.Add(Me.btnOK, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblRecordCount, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnCancel, 5, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnClear, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnApplyAll, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 359)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(751, 29)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnOK.Location = New System.Drawing.Point(608, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(67, 23)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        '
        'lblRecordCount
        '
        Me.lblRecordCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRecordCount.Location = New System.Drawing.Point(3, 8)
        Me.lblRecordCount.Name = "lblRecordCount"
        Me.lblRecordCount.Size = New System.Drawing.Size(429, 13)
        Me.lblRecordCount.TabIndex = 0
        Me.lblRecordCount.Text = "lblRecordCount"
        Me.lblRecordCount.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(681, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(67, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        '
        'btnClear
        '
        Me.btnClear.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.HelpProvider1.SetHelpString(Me.btnClear, "Click this to close the form and clear any prior selection on the calling form (r" & _
                "everting it to ""Click to Select..."").")
        Me.btnClear.Location = New System.Drawing.Point(523, 3)
        Me.btnClear.Name = "btnClear"
        Me.HelpProvider1.SetShowHelp(Me.btnClear, True)
        Me.btnClear.Size = New System.Drawing.Size(79, 23)
        Me.btnClear.TabIndex = 2
        Me.btnClear.Text = "&Clear Prior"
        '
        'btnApplyAll
        '
        Me.btnApplyAll.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.HelpProvider1.SetHelpString(Me.btnApplyAll, "If ""Use...mappings"" checkbox is selected, this button becomes visible and you hav" & _
                "e the option of applying the selected table and mapping to all cells in the call" & _
                "ing form.")
        Me.btnApplyAll.Location = New System.Drawing.Point(438, 3)
        Me.btnApplyAll.Name = "btnApplyAll"
        Me.HelpProvider1.SetShowHelp(Me.btnApplyAll, True)
        Me.btnApplyAll.Size = New System.Drawing.Size(79, 23)
        Me.btnApplyAll.TabIndex = 1
        Me.btnApplyAll.Text = "&Apply to All"
        Me.btnApplyAll.Visible = False
        '
        'TabMain
        '
        Me.TabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabMain.Controls.Add(Me.tpConstant)
        Me.TabMain.Controls.Add(Me.tpDatabase)
        Me.TabMain.Controls.Add(Me.tpMappings)
        Me.TabMain.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabMain.Location = New System.Drawing.Point(12, 12)
        Me.TabMain.Name = "TabMain"
        Me.TabMain.SelectedIndex = 0
        Me.TabMain.Size = New System.Drawing.Size(751, 344)
        Me.TabMain.TabIndex = 0
        '
        'tpConstant
        '
        Me.tpConstant.Controls.Add(Me.TableLayoutPanel7)
        Me.tpConstant.Location = New System.Drawing.Point(4, 22)
        Me.tpConstant.Name = "tpConstant"
        Me.tpConstant.Padding = New System.Windows.Forms.Padding(3)
        Me.tpConstant.Size = New System.Drawing.Size(743, 318)
        Me.tpConstant.TabIndex = 0
        Me.tpConstant.Text = "Constant Value"
        Me.tpConstant.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel7.ColumnCount = 2
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.Controls.Add(Me.Label9, 0, 0)
        Me.TableLayoutPanel7.Controls.Add(Me.txtConstant, 1, 0)
        Me.TableLayoutPanel7.Location = New System.Drawing.Point(7, 7)
        Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
        Me.TableLayoutPanel7.RowCount = 1
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.Size = New System.Drawing.Size(730, 304)
        Me.TableLayoutPanel7.TabIndex = 0
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 145)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "&Constant Value:"
        '
        'txtConstant
        '
        Me.txtConstant.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.txtConstant, "When this is selected, a time series spanning the entire range of time and having" & _
                " a constant value will be created.")
        Me.txtConstant.Location = New System.Drawing.Point(93, 141)
        Me.txtConstant.Name = "txtConstant"
        Me.HelpProvider1.SetShowHelp(Me.txtConstant, True)
        Me.txtConstant.Size = New System.Drawing.Size(634, 21)
        Me.txtConstant.TabIndex = 1
        '
        'tpDatabase
        '
        Me.tpDatabase.Controls.Add(Me.TableLayoutPanel2)
        Me.tpDatabase.Location = New System.Drawing.Point(4, 22)
        Me.tpDatabase.Name = "tpDatabase"
        Me.tpDatabase.Padding = New System.Windows.Forms.Padding(3)
        Me.tpDatabase.Size = New System.Drawing.Size(743, 318)
        Me.tpDatabase.TabIndex = 1
        Me.tpDatabase.Text = "Database"
        Me.tpDatabase.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 4
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.99813!))
        Me.TableLayoutPanel2.Controls.Add(Me.lstConversion, 3, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel3, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label3, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label4, 2, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lstTables, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lstStations, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lstPCodes, 2, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.chkMapping, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.btnView, 3, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label11, 3, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label12, 3, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.txtScaleFactor, 3, 5)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 6
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(737, 312)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'lstConversion
        '
        Me.lstConversion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstConversion.Font = New System.Drawing.Font("Arial", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstConversion.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.lstConversion, "Select from this list of pre-defined conversion factors that will be applied upon" & _
                " creation of the time series.")
        Me.lstConversion.IntegralHeight = False
        Me.lstConversion.ItemHeight = 14
        Me.lstConversion.Items.AddRange(New Object() {"CFS→CMS", "°F→°C", "lb/day→Kg/day", "lang/hr→lang/day", "joules (wt/m²/sec)→", "in→m", "m/hr→m/sec"})
        Me.lstConversion.Location = New System.Drawing.Point(555, 120)
        Me.lstConversion.Name = "lstConversion"
        Me.HelpProvider1.SetShowHelp(Me.lstConversion, True)
        Me.lstConversion.Size = New System.Drawing.Size(179, 142)
        Me.lstConversion.TabIndex = 10
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.ColumnCount = 3
        Me.TableLayoutPanel2.SetColumnSpan(Me.TableLayoutPanel3, 4)
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel3.Controls.Add(Me.cboProjects, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.optWRDB, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.optDatabase, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.txtFilename, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.btnBrowse, 2, 1)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(731, 62)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'cboProjects
        '
        Me.cboProjects.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.SetColumnSpan(Me.cboProjects, 2)
        Me.cboProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProjects.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.cboProjects, "Select which previously created WRDB 5.x project you want to reference.")
        Me.cboProjects.Location = New System.Drawing.Point(105, 5)
        Me.cboProjects.MaxDropDownItems = 16
        Me.cboProjects.Name = "cboProjects"
        Me.HelpProvider1.SetShowHelp(Me.cboProjects, True)
        Me.cboProjects.Size = New System.Drawing.Size(623, 21)
        Me.cboProjects.Sorted = True
        Me.cboProjects.TabIndex = 1
        '
        'optWRDB
        '
        Me.optWRDB.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.optWRDB.AutoSize = True
        Me.optWRDB.Checked = True
        Me.optWRDB.Location = New System.Drawing.Point(3, 7)
        Me.optWRDB.Name = "optWRDB"
        Me.optWRDB.Size = New System.Drawing.Size(96, 17)
        Me.optWRDB.TabIndex = 0
        Me.optWRDB.TabStop = True
        Me.optWRDB.Text = "&WRDB Project:"
        Me.optWRDB.UseVisualStyleBackColor = True
        '
        'optDatabase
        '
        Me.optDatabase.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.optDatabase.AutoSize = True
        Me.optDatabase.Location = New System.Drawing.Point(3, 38)
        Me.optDatabase.Name = "optDatabase"
        Me.optDatabase.Size = New System.Drawing.Size(94, 17)
        Me.optDatabase.TabIndex = 2
        Me.optDatabase.Text = "&Database File:"
        Me.optDatabase.UseVisualStyleBackColor = True
        '
        'txtFilename
        '
        Me.txtFilename.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilename.Enabled = False
        Me.HelpProvider1.SetHelpString(Me.txtFilename, "This is the full path and name of the database file from which the time series wi" & _
                "ll be acquired.")
        Me.txtFilename.Location = New System.Drawing.Point(105, 36)
        Me.txtFilename.Name = "txtFilename"
        Me.HelpProvider1.SetShowHelp(Me.txtFilename, True)
        Me.txtFilename.Size = New System.Drawing.Size(542, 21)
        Me.txtFilename.TabIndex = 3
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnBrowse.Enabled = False
        Me.HelpProvider1.SetHelpString(Me.btnBrowse, "Click here to browse to select the database file.")
        Me.btnBrowse.Location = New System.Drawing.Point(653, 35)
        Me.btnBrowse.Name = "btnBrowse"
        Me.HelpProvider1.SetShowHelp(Me.btnBrowse, True)
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 4
        Me.btnBrowse.Text = "&Browse..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(71, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "&Tables:"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(251, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(50, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "&Stations:"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(436, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "&PCodes:"
        '
        'lstTables
        '
        Me.lstTables.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstTables.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.lstTables, "This is a list of all Working and Master tables found in the specified WRDB proje" & _
                "ct.")
        Me.lstTables.IntegralHeight = False
        Me.lstTables.Location = New System.Drawing.Point(3, 120)
        Me.lstTables.Name = "lstTables"
        Me.TableLayoutPanel2.SetRowSpan(Me.lstTables, 3)
        Me.HelpProvider1.SetShowHelp(Me.lstTables, True)
        Me.lstTables.Size = New System.Drawing.Size(178, 189)
        Me.lstTables.Sorted = True
        Me.lstTables.TabIndex = 7
        '
        'lstStations
        '
        Me.lstStations.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstStations.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.lstStations, "This is a list of available stations IDs found in the specified table.")
        Me.lstStations.IntegralHeight = False
        Me.lstStations.Location = New System.Drawing.Point(187, 120)
        Me.lstStations.Name = "lstStations"
        Me.TableLayoutPanel2.SetRowSpan(Me.lstStations, 3)
        Me.HelpProvider1.SetShowHelp(Me.lstStations, True)
        Me.lstStations.Size = New System.Drawing.Size(178, 189)
        Me.lstStations.Sorted = True
        Me.lstStations.TabIndex = 8
        '
        'lstPCodes
        '
        Me.lstPCodes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstPCodes.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.lstPCodes, "This is a list of available PCodes associated with the selected Station ID in the" & _
                " specified table.")
        Me.lstPCodes.IntegralHeight = False
        Me.lstPCodes.Location = New System.Drawing.Point(371, 120)
        Me.lstPCodes.Name = "lstPCodes"
        Me.TableLayoutPanel2.SetRowSpan(Me.lstPCodes, 3)
        Me.HelpProvider1.SetShowHelp(Me.lstPCodes, True)
        Me.lstPCodes.Size = New System.Drawing.Size(178, 189)
        Me.lstPCodes.Sorted = True
        Me.lstPCodes.TabIndex = 9
        '
        'chkMapping
        '
        Me.chkMapping.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.chkMapping.AutoSize = True
        Me.TableLayoutPanel2.SetColumnSpan(Me.chkMapping, 2)
        Me.HelpProvider1.SetHelpString(Me.chkMapping, resources.GetString("chkMapping.HelpString"))
        Me.chkMapping.Location = New System.Drawing.Point(3, 74)
        Me.chkMapping.Name = "chkMapping"
        Me.HelpProvider1.SetShowHelp(Me.chkMapping, True)
        Me.chkMapping.Size = New System.Drawing.Size(290, 17)
        Me.chkMapping.TabIndex = 1
        Me.chkMapping.Text = "&Use Segment/Station and Constituent/PCode mappings"
        Me.chkMapping.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnView.Location = New System.Drawing.Point(601, 71)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(133, 23)
        Me.btnView.TabIndex = 2
        Me.btnView.Text = "&View Selected Data..."
        Me.btnView.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(612, 100)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(65, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "&Conversion:"
        '
        'Label12
        '
        Me.Label12.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(609, 268)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(70, 13)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "Scale &Factor:"
        '
        'txtScaleFactor
        '
        Me.txtScaleFactor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtScaleFactor.Location = New System.Drawing.Point(555, 288)
        Me.txtScaleFactor.Name = "txtScaleFactor"
        Me.txtScaleFactor.Size = New System.Drawing.Size(179, 21)
        Me.txtScaleFactor.TabIndex = 12
        '
        'tpMappings
        '
        Me.tpMappings.Controls.Add(Me.TableLayoutPanel4)
        Me.tpMappings.Location = New System.Drawing.Point(4, 22)
        Me.tpMappings.Name = "tpMappings"
        Me.tpMappings.Padding = New System.Windows.Forms.Padding(3)
        Me.tpMappings.Size = New System.Drawing.Size(743, 318)
        Me.tpMappings.TabIndex = 2
        Me.tpMappings.Text = "Mappings"
        Me.tpMappings.UseVisualStyleBackColor = True
        '
        'TabMapping
        '
        Me.TabMapping.Controls.Add(Me.tpStations)
        Me.TabMapping.Controls.Add(Me.tpPCodes)
        Me.TabMapping.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabMapping.Location = New System.Drawing.Point(3, 3)
        Me.TabMapping.Name = "TabMapping"
        Me.TabMapping.SelectedIndex = 0
        Me.TabMapping.Size = New System.Drawing.Size(731, 287)
        Me.TabMapping.TabIndex = 0
        '
        'tpStations
        '
        Me.tpStations.Controls.Add(Me.dgStationMapping)
        Me.tpStations.Location = New System.Drawing.Point(4, 22)
        Me.tpStations.Name = "tpStations"
        Me.tpStations.Padding = New System.Windows.Forms.Padding(3)
        Me.tpStations.Size = New System.Drawing.Size(723, 261)
        Me.tpStations.TabIndex = 0
        Me.tpStations.Text = "Segments/Stations"
        Me.tpStations.UseVisualStyleBackColor = True
        '
        'dgStationMapping
        '
        Me.dgStationMapping.AllowUserToAddRows = False
        Me.dgStationMapping.AllowUserToDeleteRows = False
        Me.dgStationMapping.AllowUserToResizeColumns = False
        Me.dgStationMapping.AllowUserToResizeRows = False
        Me.dgStationMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgStationMapping.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Segment, Me.Station})
        Me.dgStationMapping.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HelpProvider1.SetHelpString(Me.dgStationMapping, resources.GetString("dgStationMapping.HelpString"))
        Me.dgStationMapping.Location = New System.Drawing.Point(3, 3)
        Me.dgStationMapping.Name = "dgStationMapping"
        Me.HelpProvider1.SetShowHelp(Me.dgStationMapping, True)
        Me.dgStationMapping.Size = New System.Drawing.Size(717, 255)
        Me.dgStationMapping.TabIndex = 0
        '
        'Segment
        '
        Me.Segment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.Segment.HeaderText = "Segment"
        Me.Segment.Name = "Segment"
        Me.Segment.ReadOnly = True
        Me.Segment.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Segment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Segment.Width = 55
        '
        'Station
        '
        Me.Station.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Station.HeaderText = "Station"
        Me.Station.Name = "Station"
        Me.Station.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Station.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'tpPCodes
        '
        Me.tpPCodes.Controls.Add(Me.dgPCodeMapping)
        Me.tpPCodes.Location = New System.Drawing.Point(4, 22)
        Me.tpPCodes.Name = "tpPCodes"
        Me.tpPCodes.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPCodes.Size = New System.Drawing.Size(729, 286)
        Me.tpPCodes.TabIndex = 1
        Me.tpPCodes.Text = "Constituents/PCodes"
        Me.tpPCodes.UseVisualStyleBackColor = True
        '
        'dgPCodeMapping
        '
        Me.dgPCodeMapping.AllowUserToAddRows = False
        Me.dgPCodeMapping.AllowUserToDeleteRows = False
        Me.dgPCodeMapping.AllowUserToOrderColumns = True
        Me.dgPCodeMapping.AllowUserToResizeColumns = False
        Me.dgPCodeMapping.AllowUserToResizeRows = False
        Me.dgPCodeMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgPCodeMapping.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.Conversion, Me.ScaleFactor})
        Me.dgPCodeMapping.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HelpProvider1.SetHelpString(Me.dgPCodeMapping, resources.GetString("dgPCodeMapping.HelpString"))
        Me.dgPCodeMapping.Location = New System.Drawing.Point(3, 3)
        Me.dgPCodeMapping.Name = "dgPCodeMapping"
        Me.HelpProvider1.SetShowHelp(Me.dgPCodeMapping, True)
        Me.dgPCodeMapping.Size = New System.Drawing.Size(723, 280)
        Me.dgPCodeMapping.TabIndex = 0
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn1.HeaderText = "Constituent/Function"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn1.Width = 114
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn2.HeaderText = "PCode"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Conversion
        '
        Me.Conversion.HeaderText = "Conversion"
        Me.Conversion.Name = "Conversion"
        '
        'ScaleFactor
        '
        Me.ScaleFactor.HeaderText = "Scale Factor"
        Me.ScaleFactor.Name = "ScaleFactor"
        '
        'lblMult
        '
        Me.lblMult.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblMult.AutoSize = True
        Me.lblMult.Location = New System.Drawing.Point(425, 146)
        Me.lblMult.Name = "lblMult"
        Me.lblMult.Size = New System.Drawing.Size(53, 13)
        Me.lblMult.TabIndex = 1
        Me.lblMult.Text = "&Multiplier:"
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.TabMapping, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.lnkImportMappings, 0, 1)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 2
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(737, 312)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'lnkImportMappings
        '
        Me.lnkImportMappings.AutoSize = True
        Me.HelpProvider1.SetHelpString(Me.lnkImportMappings, "When you click on this, you can browse to another WaspBuilder project file and im" & _
                "port all mappings")
        Me.lnkImportMappings.Location = New System.Drawing.Point(3, 296)
        Me.lnkImportMappings.Margin = New System.Windows.Forms.Padding(3)
        Me.lnkImportMappings.Name = "lnkImportMappings"
        Me.HelpProvider1.SetShowHelp(Me.lnkImportMappings, True)
        Me.lnkImportMappings.Size = New System.Drawing.Size(266, 13)
        Me.lnkImportMappings.TabIndex = 1
        Me.lnkImportMappings.TabStop = True
        Me.lnkImportMappings.Text = "Borrow Mappings from another WaspBuilder project..."
        '
        'frmWASPSelectTimeSeries
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(775, 400)
        Me.Controls.Add(Me.TabMain)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWASPSelectTimeSeries"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Time Series"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TabMain.ResumeLayout(False)
        Me.tpConstant.ResumeLayout(False)
        Me.TableLayoutPanel7.ResumeLayout(False)
        Me.TableLayoutPanel7.PerformLayout()
        Me.tpDatabase.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.tpMappings.ResumeLayout(False)
        Me.TabMapping.ResumeLayout(False)
        Me.tpStations.ResumeLayout(False)
        CType(Me.dgStationMapping, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpPCodes.ResumeLayout(False)
        CType(Me.dgPCodeMapping, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents TabMain As System.Windows.Forms.TabControl
    Friend WithEvents tpDatabase As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cboProjects As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lstTables As System.Windows.Forms.ListBox
    Friend WithEvents lstStations As System.Windows.Forms.ListBox
    Friend WithEvents lstPCodes As System.Windows.Forms.ListBox
    Friend WithEvents lblMult As System.Windows.Forms.Label
    Friend WithEvents lblRecordCount As System.Windows.Forms.Label
    Friend WithEvents tpConstant As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel7 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtConstant As System.Windows.Forms.TextBox
    Friend WithEvents tpMappings As System.Windows.Forms.TabPage
    Friend WithEvents chkMapping As System.Windows.Forms.CheckBox
    Friend WithEvents dgStationMapping As System.Windows.Forms.DataGridView
    Friend WithEvents Segment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Station As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgPCodeMapping As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TabMapping As System.Windows.Forms.TabControl
    Friend WithEvents tpStations As System.Windows.Forms.TabPage
    Friend WithEvents tpPCodes As System.Windows.Forms.TabPage
    Friend WithEvents btnApplyAll As System.Windows.Forms.Button
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents lstConversion As System.Windows.Forms.ListBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtScaleFactor As System.Windows.Forms.TextBox
    Friend WithEvents optWRDB As System.Windows.Forms.RadioButton
    Friend WithEvents optDatabase As System.Windows.Forms.RadioButton
    Friend WithEvents txtFilename As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Conversion As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ScaleFactor As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lnkImportMappings As System.Windows.Forms.LinkLabel

End Class
