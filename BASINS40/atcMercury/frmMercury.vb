Imports System.Drawing
Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcControls
Imports System.Windows.Forms
Imports System.Data.OleDb
Imports HTMLBuilder

Friend Class frmMercury
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Friend WithEvents btnAbout As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSaveAs As System.Windows.Forms.Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Friend WithEvents tabResults As System.Windows.Forms.TabPage
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents btnPreview As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents wbResults As System.Windows.Forms.WebBrowser
    Friend WithEvents tabChem As System.Windows.Forms.TabPage
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tabHydrology As System.Windows.Forms.TabPage
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tabLanduse As System.Windows.Forms.TabPage
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cboLandUseIDField As System.Windows.Forms.ComboBox
    Friend WithEvents lblLandUseIDField As System.Windows.Forms.Label
    Friend WithEvents cboLandUseLayer As System.Windows.Forms.ComboBox
    Friend WithEvents lblLandUseLayer As System.Windows.Forms.Label
    Friend WithEvents cboLanduse As System.Windows.Forms.ComboBox
    Friend WithEvents lblLanduseType As System.Windows.Forms.Label
    Friend WithEvents tabSoils As System.Windows.Forms.TabPage
    Friend WithEvents lblSoilIDField As System.Windows.Forms.Label
    Friend WithEvents cboSoilIDField As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboSoilLayer As System.Windows.Forms.ComboBox
    Friend WithEvents lblSoilsLayer As System.Windows.Forms.Label
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents txtOutputName As System.Windows.Forms.TextBox
    Friend WithEvents txtGridSize As atcControls.atcText
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblGridSize As System.Windows.Forms.Label
    Friend WithEvents cboSediment As System.Windows.Forms.ComboBox
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblOutput As System.Windows.Forms.Label
    Friend WithEvents lblSubbasinsLayer As System.Windows.Forms.Label
    Friend WithEvents tabMercury As System.Windows.Forms.TabControl
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents dgSoil As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents dgLandUse As System.Windows.Forms.DataGridView
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Density As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AWC As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pH As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Depth As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Organic As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Clay As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewComboBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tabClimate As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents dgClimate As System.Windows.Forms.DataGridView
    Friend WithEvents cboClimate As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents cboStartYear As System.Windows.Forms.ComboBox
    Friend WithEvents cboEndYear As System.Windows.Forms.ComboBox
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox12 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents lblGenerate As System.Windows.Forms.Label
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider

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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMercury))
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnHelp = New System.Windows.Forms.Button
        Me.btnAbout = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
        Me.btnSaveAs = New System.Windows.Forms.Button
        Me.tabResults = New System.Windows.Forms.TabPage
        Me.lblGenerate = New System.Windows.Forms.Label
        Me.btnCopy = New System.Windows.Forms.Button
        Me.btnPreview = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.wbResults = New System.Windows.Forms.WebBrowser
        Me.tabChem = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.TextBox7 = New System.Windows.Forms.TextBox
        Me.TextBox8 = New System.Windows.Forms.TextBox
        Me.TextBox9 = New System.Windows.Forms.TextBox
        Me.TextBox10 = New System.Windows.Forms.TextBox
        Me.TextBox11 = New System.Windows.Forms.TextBox
        Me.TextBox12 = New System.Windows.Forms.TextBox
        Me.TextBox13 = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label27 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.tabHydrology = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tabLanduse = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.dgLandUse = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewComboBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cboLandUseIDField = New System.Windows.Forms.ComboBox
        Me.lblLandUseIDField = New System.Windows.Forms.Label
        Me.cboLandUseLayer = New System.Windows.Forms.ComboBox
        Me.lblLandUseLayer = New System.Windows.Forms.Label
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.lblLanduseType = New System.Windows.Forms.Label
        Me.tabSoils = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.dgSoil = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Density = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.AWC = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.pH = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Depth = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Organic = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Clay = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblSoilIDField = New System.Windows.Forms.Label
        Me.cboSoilIDField = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cboSoilLayer = New System.Windows.Forms.ComboBox
        Me.lblSoilsLayer = New System.Windows.Forms.Label
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.txtOutputName = New System.Windows.Forms.TextBox
        Me.txtGridSize = New atcControls.atcText
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblGridSize = New System.Windows.Forms.Label
        Me.cboSediment = New System.Windows.Forms.ComboBox
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.lblOutput = New System.Windows.Forms.Label
        Me.lblSubbasinsLayer = New System.Windows.Forms.Label
        Me.tabMercury = New System.Windows.Forms.TabControl
        Me.tabClimate = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel
        Me.dgClimate = New System.Windows.Forms.DataGridView
        Me.Label12 = New System.Windows.Forms.Label
        Me.cboClimate = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.cboStartYear = New System.Windows.Forms.ComboBox
        Me.cboEndYear = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.tabResults.SuspendLayout()
        Me.tabChem.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tabHydrology.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.tabLanduse.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        CType(Me.dgLandUse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabSoils.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.dgSoil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabGeneral.SuspendLayout()
        Me.tabMercury.SuspendLayout()
        Me.tabClimate.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        CType(Me.dgClimate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(551, 348)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(85, 26)
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "&Generate"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(642, 348)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 26)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Close"
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHelp.Location = New System.Drawing.Point(287, 347)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 26)
        Me.btnHelp.TabIndex = 3
        Me.btnHelp.Text = "&Help"
        '
        'btnAbout
        '
        Me.btnAbout.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnAbout.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAbout.Location = New System.Drawing.Point(368, 347)
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(75, 26)
        Me.btnAbout.TabIndex = 4
        Me.btnAbout.Text = "&About"
        '
        'btnOpen
        '
        Me.btnOpen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.btnOpen, "Open an existing mercury scenario file which contains all settings specified on t" & _
                "hese tabs. If you do not explicitly open a data file, the default file will auto" & _
                "matically be opened and used.")
        Me.btnOpen.Location = New System.Drawing.Point(12, 347)
        Me.btnOpen.Name = "btnOpen"
        Me.HelpProvider1.SetShowHelp(Me.btnOpen, True)
        Me.btnOpen.Size = New System.Drawing.Size(75, 26)
        Me.btnOpen.TabIndex = 1
        Me.btnOpen.Text = "&Open..."
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSaveAs
        '
        Me.btnSaveAs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSaveAs.Location = New System.Drawing.Point(93, 347)
        Me.btnSaveAs.Name = "btnSaveAs"
        Me.btnSaveAs.Size = New System.Drawing.Size(75, 26)
        Me.btnSaveAs.TabIndex = 2
        Me.btnSaveAs.Text = "&Save As..."
        Me.btnSaveAs.UseVisualStyleBackColor = True
        '
        'tabResults
        '
        Me.tabResults.Controls.Add(Me.lblGenerate)
        Me.tabResults.Controls.Add(Me.btnCopy)
        Me.tabResults.Controls.Add(Me.btnPreview)
        Me.tabResults.Controls.Add(Me.btnPrint)
        Me.tabResults.Controls.Add(Me.wbResults)
        Me.tabResults.Location = New System.Drawing.Point(4, 25)
        Me.tabResults.Name = "tabResults"
        Me.tabResults.Size = New System.Drawing.Size(509, 287)
        Me.tabResults.TabIndex = 7
        Me.tabResults.Text = "Results"
        Me.tabResults.UseVisualStyleBackColor = True
        '
        'lblGenerate
        '
        Me.lblGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblGenerate.AutoSize = True
        Me.lblGenerate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGenerate.Location = New System.Drawing.Point(85, 266)
        Me.lblGenerate.Name = "lblGenerate"
        Me.lblGenerate.Size = New System.Drawing.Size(190, 13)
        Me.lblGenerate.TabIndex = 5
        Me.lblGenerate.Text = "Click Generate button to refresh results"
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.Location = New System.Drawing.Point(4, 261)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(75, 23)
        Me.btnCopy.TabIndex = 1
        Me.btnCopy.Text = "&Copy"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'btnPreview
        '
        Me.btnPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPreview.Location = New System.Drawing.Point(350, 261)
        Me.btnPreview.Name = "btnPreview"
        Me.btnPreview.Size = New System.Drawing.Size(75, 23)
        Me.btnPreview.TabIndex = 2
        Me.btnPreview.Text = "Pre&view..."
        Me.btnPreview.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.Location = New System.Drawing.Point(431, 261)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 23)
        Me.btnPrint.TabIndex = 3
        Me.btnPrint.Text = "&Print..."
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'wbResults
        '
        Me.wbResults.AllowWebBrowserDrop = False
        Me.wbResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.wbResults.Location = New System.Drawing.Point(4, 3)
        Me.wbResults.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbResults.Name = "wbResults"
        Me.wbResults.Size = New System.Drawing.Size(502, 252)
        Me.wbResults.TabIndex = 0
        '
        'tabChem
        '
        Me.tabChem.Controls.Add(Me.TableLayoutPanel1)
        Me.tabChem.Controls.Add(Me.Label7)
        Me.tabChem.Location = New System.Drawing.Point(4, 25)
        Me.tabChem.Name = "tabChem"
        Me.tabChem.Size = New System.Drawing.Size(509, 287)
        Me.tabChem.TabIndex = 4
        Me.tabChem.Text = "Chemistry"
        Me.tabChem.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox4, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox5, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox6, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox7, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox8, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox9, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox10, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox11, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox12, 3, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox13, 3, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Label18, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label19, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label20, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label21, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label22, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Label24, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label25, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label26, 2, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label27, 2, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Label23, 2, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 94)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 5
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(503, 190)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'TextBox4
        '
        Me.TextBox4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox4.Location = New System.Drawing.Point(183, 9)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(65, 20)
        Me.TextBox4.TabIndex = 0
        '
        'TextBox5
        '
        Me.TextBox5.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox5.Location = New System.Drawing.Point(183, 47)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(65, 20)
        Me.TextBox5.TabIndex = 0
        '
        'TextBox6
        '
        Me.TextBox6.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox6.Location = New System.Drawing.Point(183, 85)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(65, 20)
        Me.TextBox6.TabIndex = 0
        '
        'TextBox7
        '
        Me.TextBox7.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox7.Location = New System.Drawing.Point(183, 123)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(65, 20)
        Me.TextBox7.TabIndex = 0
        '
        'TextBox8
        '
        Me.TextBox8.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox8.Location = New System.Drawing.Point(183, 161)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New System.Drawing.Size(65, 20)
        Me.TextBox8.TabIndex = 0
        '
        'TextBox9
        '
        Me.TextBox9.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox9.Location = New System.Drawing.Point(434, 9)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.Size = New System.Drawing.Size(65, 20)
        Me.TextBox9.TabIndex = 0
        '
        'TextBox10
        '
        Me.TextBox10.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox10.Location = New System.Drawing.Point(434, 47)
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New System.Drawing.Size(65, 20)
        Me.TextBox10.TabIndex = 0
        '
        'TextBox11
        '
        Me.TextBox11.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox11.Location = New System.Drawing.Point(434, 85)
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.Size = New System.Drawing.Size(65, 20)
        Me.TextBox11.TabIndex = 0
        '
        'TextBox12
        '
        Me.TextBox12.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox12.Location = New System.Drawing.Point(434, 123)
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.Size = New System.Drawing.Size(65, 20)
        Me.TextBox12.TabIndex = 0
        '
        'TextBox13
        '
        Me.TextBox13.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox13.Location = New System.Drawing.Point(434, 161)
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.Size = New System.Drawing.Size(65, 20)
        Me.TextBox13.TabIndex = 0
        '
        'Label18
        '
        Me.Label18.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(28, 12)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(149, 13)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "&Dry deposition rate (g/m^2-yr):"
        '
        'Label19
        '
        Me.Label19.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(68, 50)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(109, 13)
        Me.Label19.TabIndex = 1
        Me.Label19.Text = "Deposition &period (yr):"
        '
        'Label20
        '
        Me.Label20.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(28, 88)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(149, 13)
        Me.Label20.TabIndex = 1
        Me.Label20.Text = "Soil &loss degrading const (/yr):"
        '
        'Label21
        '
        Me.Label21.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(24, 126)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(153, 13)
        Me.Label21.TabIndex = 1
        Me.Label21.Text = "Soil base reduction &depth (cm):"
        '
        'Label22
        '
        Me.Label22.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(41, 164)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(136, 13)
        Me.Label22.TabIndex = 1
        Me.Label22.Text = "Pollutant &enrichment factor:"
        '
        'Label24
        '
        Me.Label24.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(274, 50)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(154, 13)
        Me.Label24.TabIndex = 1
        Me.Label24.Text = "Soil-water partition coef (mL/g):"
        '
        'Label25
        '
        Me.Label25.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(276, 88)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(152, 13)
        Me.Label25.TabIndex = 1
        Me.Label25.Text = "Soil base &reduction rate (/day):"
        '
        'Label26
        '
        Me.Label26.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(272, 120)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(156, 26)
        Me.Label26.TabIndex = 1
        Me.Label26.Text = "Watershed &incorporation depth (cm):"
        '
        'Label27
        '
        Me.Label27.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(277, 164)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(151, 13)
        Me.Label27.TabIndex = 1
        Me.Label27.Text = "Base soil &mercury conc (ng/h):"
        '
        'Label23
        '
        Me.Label23.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(275, 12)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(153, 13)
        Me.Label23.TabIndex = 1
        Me.Label23.Text = "&Wet deposition rate (g/m^2-yr):"
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.BackColor = System.Drawing.SystemColors.Info
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label7.Location = New System.Drawing.Point(3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(503, 91)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "This page is used to determine the mercury concentration in the watershed's surfa" & _
            "ce soil."
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabHydrology
        '
        Me.tabHydrology.Controls.Add(Me.TableLayoutPanel4)
        Me.tabHydrology.Controls.Add(Me.Label2)
        Me.tabHydrology.Location = New System.Drawing.Point(4, 25)
        Me.tabHydrology.Name = "tabHydrology"
        Me.tabHydrology.Size = New System.Drawing.Size(509, 287)
        Me.tabHydrology.TabIndex = 3
        Me.tabHydrology.Text = "Hydrology"
        Me.tabHydrology.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.45527!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.54473!))
        Me.TableLayoutPanel4.Controls.Add(Me.Label9, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.Label8, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.Label15, 0, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.Label16, 0, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.Label17, 0, 4)
        Me.TableLayoutPanel4.Controls.Add(Me.ComboBox1, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.ComboBox2, 1, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.TextBox1, 1, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.TextBox2, 1, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.TextBox3, 1, 4)
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 119)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 5
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(503, 165)
        Me.TableLayoutPanel4.TabIndex = 1
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(140, 10)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(146, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Growing Season &Start Month:"
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(143, 43)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(143, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Growing Season &End Month:"
        '
        'Label15
        '
        Me.Label15.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(105, 76)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(181, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "&Growing Season Avg Rain Days/mo:"
        '
        'Label16
        '
        Me.Label16.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(82, 109)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(204, 13)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "&Non-Growing Season Avg Rain Days/mo:"
        '
        'Label17
        '
        Me.Label17.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(38, 142)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(248, 13)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Annual Irrigation for Cropland (cm/growing season):"
        '
        'ComboBox1
        '
        Me.ComboBox1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"})
        Me.ComboBox1.Location = New System.Drawing.Point(292, 6)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(94, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'ComboBox2
        '
        Me.ComboBox2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Items.AddRange(New Object() {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"})
        Me.ComboBox2.Location = New System.Drawing.Point(292, 39)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(94, 21)
        Me.ComboBox2.TabIndex = 1
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBox1.Location = New System.Drawing.Point(292, 72)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(94, 20)
        Me.TextBox1.TabIndex = 2
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBox2.Location = New System.Drawing.Point(292, 105)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(94, 20)
        Me.TextBox2.TabIndex = 2
        '
        'TextBox3
        '
        Me.TextBox3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBox3.Location = New System.Drawing.Point(292, 138)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(94, 20)
        Me.TextBox3.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.SystemColors.Info
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(503, 115)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "The growing season information is used when computing mercury uptake by various p" & _
            "rocesses. The growing season start and end months are inclusive and assumed to i" & _
            "nclude the entire month."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabLanduse
        '
        Me.tabLanduse.Controls.Add(Me.TableLayoutPanel3)
        Me.tabLanduse.Controls.Add(Me.Label4)
        Me.tabLanduse.Controls.Add(Me.cboLandUseIDField)
        Me.tabLanduse.Controls.Add(Me.lblLandUseIDField)
        Me.tabLanduse.Controls.Add(Me.cboLandUseLayer)
        Me.tabLanduse.Controls.Add(Me.lblLandUseLayer)
        Me.tabLanduse.Controls.Add(Me.cboLanduse)
        Me.tabLanduse.Controls.Add(Me.lblLanduseType)
        Me.tabLanduse.Location = New System.Drawing.Point(4, 25)
        Me.tabLanduse.Name = "tabLanduse"
        Me.tabLanduse.Size = New System.Drawing.Size(694, 287)
        Me.tabLanduse.TabIndex = 2
        Me.tabLanduse.Text = "Land Use"
        Me.tabLanduse.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.dgLandUse, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.Label6, 0, 0)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 166)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(688, 121)
        Me.TableLayoutPanel3.TabIndex = 7
        '
        'dgLandUse
        '
        Me.dgLandUse.AllowUserToResizeColumns = False
        Me.dgLandUse.AllowUserToResizeRows = False
        Me.dgLandUse.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgLandUse.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgLandUse.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgLandUse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgLandUse.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.DataGridViewComboBoxColumn1, Me.DataGridViewTextBoxColumn6, Me.DataGridViewTextBoxColumn7, Me.DataGridViewTextBoxColumn8, Me.DataGridViewTextBoxColumn9, Me.DataGridViewTextBoxColumn10, Me.DataGridViewTextBoxColumn11, Me.Column1})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgLandUse.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgLandUse.Location = New System.Drawing.Point(3, 16)
        Me.dgLandUse.Name = "dgLandUse"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgLandUse.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgLandUse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgLandUse.Size = New System.Drawing.Size(682, 102)
        Me.dgLandUse.TabIndex = 2
        '
        'DataGridViewTextBoxColumn4
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn4.HeaderText = "Landuse ID"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn5
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewTextBoxColumn5.FillWeight = 200.0!
        Me.DataGridViewTextBoxColumn5.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewComboBoxColumn1
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        Me.DataGridViewComboBoxColumn1.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewComboBoxColumn1.HeaderText = "Grow Cover"
        Me.DataGridViewComboBoxColumn1.Name = "DataGridViewComboBoxColumn1"
        Me.DataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewComboBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Nongrow Cover"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Pct Imperv."
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "Is Water?"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "CN A"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "CN B"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.HeaderText = "CN C"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column1
        '
        Me.Column1.HeaderText = "CN D"
        Me.Column1.Name = "Column1"
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(280, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(128, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Land Use Characteristics:"
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.SystemColors.Info
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label4.Location = New System.Drawing.Point(3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(688, 82)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = resources.GetString("Label4.Text")
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboLandUseIDField
        '
        Me.cboLandUseIDField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseIDField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLandUseIDField.FormattingEnabled = True
        Me.cboLandUseIDField.Location = New System.Drawing.Point(186, 139)
        Me.cboLandUseIDField.Name = "cboLandUseIDField"
        Me.cboLandUseIDField.Size = New System.Drawing.Size(463, 21)
        Me.cboLandUseIDField.Sorted = True
        Me.cboLandUseIDField.TabIndex = 6
        '
        'lblLandUseIDField
        '
        Me.lblLandUseIDField.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblLandUseIDField.AutoSize = True
        Me.lblLandUseIDField.Location = New System.Drawing.Point(71, 142)
        Me.lblLandUseIDField.Name = "lblLandUseIDField"
        Me.lblLandUseIDField.Size = New System.Drawing.Size(95, 13)
        Me.lblLandUseIDField.TabIndex = 5
        Me.lblLandUseIDField.Text = "Land Use ID &Field:"
        '
        'cboLandUseLayer
        '
        Me.cboLandUseLayer.AllowDrop = True
        Me.cboLandUseLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLandUseLayer.Location = New System.Drawing.Point(186, 112)
        Me.cboLandUseLayer.Name = "cboLandUseLayer"
        Me.cboLandUseLayer.Size = New System.Drawing.Size(463, 21)
        Me.cboLandUseLayer.Sorted = True
        Me.cboLandUseLayer.TabIndex = 4
        '
        'lblLandUseLayer
        '
        Me.lblLandUseLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblLandUseLayer.AutoSize = True
        Me.lblLandUseLayer.Location = New System.Drawing.Point(71, 115)
        Me.lblLandUseLayer.Name = "lblLandUseLayer"
        Me.lblLandUseLayer.Size = New System.Drawing.Size(85, 13)
        Me.lblLandUseLayer.TabIndex = 3
        Me.lblLandUseLayer.Text = "Land Use &Layer:"
        '
        'cboLanduse
        '
        Me.cboLanduse.AllowDrop = True
        Me.cboLanduse.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse.Location = New System.Drawing.Point(165, 85)
        Me.cboLanduse.Name = "cboLanduse"
        Me.cboLanduse.Size = New System.Drawing.Size(507, 21)
        Me.cboLanduse.TabIndex = 2
        '
        'lblLanduseType
        '
        Me.lblLanduseType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblLanduseType.AutoSize = True
        Me.lblLanduseType.Location = New System.Drawing.Point(19, 88)
        Me.lblLanduseType.Name = "lblLanduseType"
        Me.lblLanduseType.Size = New System.Drawing.Size(112, 13)
        Me.lblLanduseType.TabIndex = 1
        Me.lblLanduseType.Text = "Land Use Layer &Type:"
        '
        'tabSoils
        '
        Me.tabSoils.Controls.Add(Me.TableLayoutPanel2)
        Me.tabSoils.Controls.Add(Me.lblSoilIDField)
        Me.tabSoils.Controls.Add(Me.cboSoilIDField)
        Me.tabSoils.Controls.Add(Me.Label3)
        Me.tabSoils.Controls.Add(Me.cboSoilLayer)
        Me.tabSoils.Controls.Add(Me.lblSoilsLayer)
        Me.tabSoils.Location = New System.Drawing.Point(4, 25)
        Me.tabSoils.Name = "tabSoils"
        Me.tabSoils.Size = New System.Drawing.Size(694, 287)
        Me.tabSoils.TabIndex = 1
        Me.tabSoils.Text = "Soils"
        Me.tabSoils.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.dgSoil, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label5, 0, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 139)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(688, 145)
        Me.TableLayoutPanel2.TabIndex = 5
        '
        'dgSoil
        '
        Me.dgSoil.AllowUserToResizeColumns = False
        Me.dgSoil.AllowUserToResizeRows = False
        Me.dgSoil.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgSoil.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgSoil.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.dgSoil.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgSoil.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.Density, Me.AWC, Me.pH, Me.Depth, Me.Organic, Me.Clay})
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgSoil.DefaultCellStyle = DataGridViewCellStyle11
        Me.dgSoil.Location = New System.Drawing.Point(3, 16)
        Me.dgSoil.Name = "dgSoil"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgSoil.RowHeadersDefaultCellStyle = DataGridViewCellStyle12
        Me.dgSoil.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgSoil.Size = New System.Drawing.Size(682, 126)
        Me.dgSoil.TabIndex = 2
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn1.HeaderText = "Soil ID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn2.FillWeight = 200.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn3
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle10
        Me.DataGridViewTextBoxColumn3.HeaderText = "Hyd. Group"
        Me.DataGridViewTextBoxColumn3.Items.AddRange(New Object() {"A", "B", "C", "D"})
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'Density
        '
        Me.Density.HeaderText = "Bulk Density"
        Me.Density.Name = "Density"
        Me.Density.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'AWC
        '
        Me.AWC.HeaderText = "AWC"
        Me.AWC.Name = "AWC"
        Me.AWC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'pH
        '
        Me.pH.HeaderText = "pH"
        Me.pH.Name = "pH"
        Me.pH.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Depth
        '
        Me.Depth.HeaderText = "Depth"
        Me.Depth.Name = "Depth"
        Me.Depth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Organic
        '
        Me.Organic.HeaderText = "Organic Content"
        Me.Organic.Name = "Organic"
        Me.Organic.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Clay
        '
        Me.Clay.HeaderText = "Clay Content"
        Me.Clay.Name = "Clay"
        Me.Clay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(259, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(170, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Soil Type/&Physical Characteristics:"
        '
        'lblSoilIDField
        '
        Me.lblSoilIDField.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblSoilIDField.AutoSize = True
        Me.lblSoilIDField.Location = New System.Drawing.Point(26, 115)
        Me.lblSoilIDField.Name = "lblSoilIDField"
        Me.lblSoilIDField.Size = New System.Drawing.Size(93, 13)
        Me.lblSoilIDField.TabIndex = 3
        Me.lblSoilIDField.Text = "Soil Type ID &Field:"
        '
        'cboSoilIDField
        '
        Me.cboSoilIDField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSoilIDField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSoilIDField.FormattingEnabled = True
        Me.cboSoilIDField.Location = New System.Drawing.Point(172, 112)
        Me.cboSoilIDField.Name = "cboSoilIDField"
        Me.cboSoilIDField.Size = New System.Drawing.Size(507, 21)
        Me.cboSoilIDField.Sorted = True
        Me.cboSoilIDField.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.BackColor = System.Drawing.SystemColors.Info
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Location = New System.Drawing.Point(3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(688, 82)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = resources.GetString("Label3.Text")
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboSoilLayer
        '
        Me.cboSoilLayer.AllowDrop = True
        Me.cboSoilLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSoilLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSoilLayer.Location = New System.Drawing.Point(172, 85)
        Me.cboSoilLayer.Name = "cboSoilLayer"
        Me.cboSoilLayer.Size = New System.Drawing.Size(507, 21)
        Me.cboSoilLayer.Sorted = True
        Me.cboSoilLayer.TabIndex = 2
        '
        'lblSoilsLayer
        '
        Me.lblSoilsLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblSoilsLayer.AutoSize = True
        Me.lblSoilsLayer.Location = New System.Drawing.Point(26, 88)
        Me.lblSoilsLayer.Name = "lblSoilsLayer"
        Me.lblSoilsLayer.Size = New System.Drawing.Size(83, 13)
        Me.lblSoilsLayer.TabIndex = 1
        Me.lblSoilsLayer.Text = "Soil Type &Layer:"
        '
        'tabGeneral
        '
        Me.tabGeneral.BackColor = System.Drawing.Color.Transparent
        Me.tabGeneral.Controls.Add(Me.txtOutputName)
        Me.tabGeneral.Controls.Add(Me.txtGridSize)
        Me.tabGeneral.Controls.Add(Me.Label1)
        Me.tabGeneral.Controls.Add(Me.lblGridSize)
        Me.tabGeneral.Controls.Add(Me.cboSediment)
        Me.tabGeneral.Controls.Add(Me.cboSubbasins)
        Me.tabGeneral.Controls.Add(Me.Label10)
        Me.tabGeneral.Controls.Add(Me.lblOutput)
        Me.tabGeneral.Controls.Add(Me.lblSubbasinsLayer)
        Me.tabGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Size = New System.Drawing.Size(509, 287)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'txtOutputName
        '
        Me.txtOutputName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputName.Location = New System.Drawing.Point(167, 145)
        Me.txtOutputName.Name = "txtOutputName"
        Me.txtOutputName.Size = New System.Drawing.Size(322, 20)
        Me.txtOutputName.TabIndex = 2
        '
        'txtGridSize
        '
        Me.txtGridSize.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtGridSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGridSize.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtGridSize.DefaultValue = 30
        Me.txtGridSize.HardMax = 1000
        Me.txtGridSize.HardMin = 0
        Me.txtGridSize.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtGridSize.Location = New System.Drawing.Point(167, 171)
        Me.txtGridSize.MaxDecimal = 0
        Me.txtGridSize.maxWidth = 20
        Me.txtGridSize.Name = "txtGridSize"
        Me.txtGridSize.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtGridSize.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtGridSize.SelLength = 0
        Me.txtGridSize.SelStart = 2
        Me.txtGridSize.Size = New System.Drawing.Size(322, 20)
        Me.txtGridSize.SoftMax = 100
        Me.txtGridSize.SoftMin = 5
        Me.txtGridSize.TabIndex = 4
        Me.txtGridSize.Value = CType(30, Long)
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.BackColor = System.Drawing.SystemColors.Info
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(503, 133)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = resources.GetString("Label1.Text")
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblGridSize
        '
        Me.lblGridSize.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblGridSize.AutoSize = True
        Me.lblGridSize.Location = New System.Drawing.Point(21, 175)
        Me.lblGridSize.Name = "lblGridSize"
        Me.lblGridSize.Size = New System.Drawing.Size(89, 13)
        Me.lblGridSize.TabIndex = 3
        Me.lblGridSize.Text = "Grid &Cell Size (m):"
        '
        'cboSediment
        '
        Me.cboSediment.AllowDrop = True
        Me.cboSediment.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSediment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSediment.Location = New System.Drawing.Point(167, 224)
        Me.cboSediment.Name = "cboSediment"
        Me.cboSediment.Size = New System.Drawing.Size(322, 21)
        Me.cboSediment.Sorted = True
        Me.cboSediment.TabIndex = 8
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Location = New System.Drawing.Point(167, 197)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(322, 21)
        Me.cboSubbasins.Sorted = True
        Me.cboSubbasins.TabIndex = 8
        '
        'Label10
        '
        Me.Label10.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(21, 227)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(83, 13)
        Me.Label10.TabIndex = 7
        Me.Label10.Text = "&Sediment Layer:"
        '
        'lblOutput
        '
        Me.lblOutput.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblOutput.AutoSize = True
        Me.lblOutput.Location = New System.Drawing.Point(21, 148)
        Me.lblOutput.Name = "lblOutput"
        Me.lblOutput.Size = New System.Drawing.Size(105, 13)
        Me.lblOutput.TabIndex = 1
        Me.lblOutput.Text = "Output Folder &Name:"
        '
        'lblSubbasinsLayer
        '
        Me.lblSubbasinsLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblSubbasinsLayer.AutoSize = True
        Me.lblSubbasinsLayer.Location = New System.Drawing.Point(21, 200)
        Me.lblSubbasinsLayer.Name = "lblSubbasinsLayer"
        Me.lblSubbasinsLayer.Size = New System.Drawing.Size(88, 13)
        Me.lblSubbasinsLayer.TabIndex = 7
        Me.lblSubbasinsLayer.Text = "Subbasins &Layer:"
        '
        'tabMercury
        '
        Me.tabMercury.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMercury.Controls.Add(Me.tabGeneral)
        Me.tabMercury.Controls.Add(Me.tabSoils)
        Me.tabMercury.Controls.Add(Me.tabLanduse)
        Me.tabMercury.Controls.Add(Me.tabHydrology)
        Me.tabMercury.Controls.Add(Me.tabClimate)
        Me.tabMercury.Controls.Add(Me.tabChem)
        Me.tabMercury.Controls.Add(Me.tabResults)
        Me.tabMercury.Cursor = System.Windows.Forms.Cursors.Default
        Me.tabMercury.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMercury.ItemSize = New System.Drawing.Size(60, 21)
        Me.tabMercury.Location = New System.Drawing.Point(14, 14)
        Me.tabMercury.Name = "tabMercury"
        Me.tabMercury.SelectedIndex = 0
        Me.tabMercury.Size = New System.Drawing.Size(702, 316)
        Me.tabMercury.TabIndex = 0
        '
        'tabClimate
        '
        Me.tabClimate.Controls.Add(Me.TableLayoutPanel5)
        Me.tabClimate.Controls.Add(Me.Label11)
        Me.tabClimate.Location = New System.Drawing.Point(4, 25)
        Me.tabClimate.Name = "tabClimate"
        Me.tabClimate.Padding = New System.Windows.Forms.Padding(3)
        Me.tabClimate.Size = New System.Drawing.Size(509, 287)
        Me.tabClimate.TabIndex = 8
        Me.tabClimate.Text = "Climate Data"
        Me.tabClimate.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel5.ColumnCount = 4
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.dgClimate, 0, 2)
        Me.TableLayoutPanel5.Controls.Add(Me.Label12, 0, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.cboClimate, 2, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.Label13, 0, 1)
        Me.TableLayoutPanel5.Controls.Add(Me.Label14, 2, 1)
        Me.TableLayoutPanel5.Controls.Add(Me.cboStartYear, 1, 1)
        Me.TableLayoutPanel5.Controls.Add(Me.cboEndYear, 3, 1)
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(6, 107)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 3
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 116.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(497, 177)
        Me.TableLayoutPanel5.TabIndex = 2
        '
        'dgClimate
        '
        Me.dgClimate.AllowUserToResizeColumns = False
        Me.dgClimate.AllowUserToResizeRows = False
        Me.dgClimate.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgClimate.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgClimate.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle13
        Me.dgClimate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel5.SetColumnSpan(Me.dgClimate, 4)
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgClimate.DefaultCellStyle = DataGridViewCellStyle14
        Me.dgClimate.Location = New System.Drawing.Point(3, 57)
        Me.dgClimate.Name = "dgClimate"
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgClimate.RowHeadersDefaultCellStyle = DataGridViewCellStyle15
        Me.dgClimate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgClimate.Size = New System.Drawing.Size(491, 117)
        Me.dgClimate.TabIndex = 3
        '
        'Label12
        '
        Me.Label12.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label12.AutoSize = True
        Me.TableLayoutPanel5.SetColumnSpan(Me.Label12, 2)
        Me.Label12.Location = New System.Drawing.Point(90, 7)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(155, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Representative &Climate Station:"
        '
        'cboClimate
        '
        Me.cboClimate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel5.SetColumnSpan(Me.cboClimate, 2)
        Me.cboClimate.FormattingEnabled = True
        Me.cboClimate.Location = New System.Drawing.Point(251, 3)
        Me.cboClimate.Name = "cboClimate"
        Me.cboClimate.Size = New System.Drawing.Size(243, 21)
        Me.cboClimate.TabIndex = 1
        '
        'Label13
        '
        Me.Label13.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(64, 34)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(57, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "&Start Year:"
        '
        'Label14
        '
        Me.Label14.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(315, 34)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(54, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "&End Year:"
        '
        'cboStartYear
        '
        Me.cboStartYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStartYear.FormattingEnabled = True
        Me.cboStartYear.Location = New System.Drawing.Point(127, 30)
        Me.cboStartYear.Name = "cboStartYear"
        Me.cboStartYear.Size = New System.Drawing.Size(118, 21)
        Me.cboStartYear.TabIndex = 2
        '
        'cboEndYear
        '
        Me.cboEndYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboEndYear.FormattingEnabled = True
        Me.cboEndYear.Location = New System.Drawing.Point(375, 30)
        Me.cboEndYear.Name = "cboEndYear"
        Me.cboEndYear.Size = New System.Drawing.Size(119, 21)
        Me.cboEndYear.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.BackColor = System.Drawing.SystemColors.Info
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Location = New System.Drawing.Point(3, 3)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(503, 101)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = resources.GetString("Label11.Text")
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmMercury
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(729, 386)
        Me.Controls.Add(Me.btnSaveAs)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.btnAbout)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tabMercury)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(552, 420)
        Me.Name = "frmMercury"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "BASINS Mercury Estimator"
        Me.tabResults.ResumeLayout(False)
        Me.tabResults.PerformLayout()
        Me.tabChem.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.tabHydrology.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.tabLanduse.ResumeLayout(False)
        Me.tabLanduse.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        CType(Me.dgLandUse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSoils.ResumeLayout(False)
        Me.tabSoils.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.dgSoil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabMercury.ResumeLayout(False)
        Me.tabClimate.ResumeLayout(False)
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TableLayoutPanel5.PerformLayout()
        CType(Me.dgClimate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Dim gInitializing As Boolean
    Dim gLanduseType As Integer
    Dim gMethod As Integer

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Logger.Dbg("UserCanceled")
        Me.Close()
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim Header As String = String.Format("Version:1.0{0}StartHTML:0000000105{0}EndHTML:{1,10:0}{0}StartFragment:0000000105{0}EndFragment:{1,10:0}{0}", vbCrLf, wbResults.DocumentText.Length)
        Clipboard.SetDataObject(New DataObject(DataFormats.Html, Header & wbResults.DocumentText))
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\Mercury.html")
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click

        tabMercury.SelectedIndex = tabMercury.TabCount - 1
        wbResults.DocumentText = ""

        EnableControls(False)

        'save current settings as defaults
        SaveForm()

        Dim dictSoil As New Generic.Dictionary(Of String, Double)
        ProgressForm = New frmProgress

        With ProgressForm
            .Show(Me)

            If GenerateLoads() Then
                .Status = "Refreshing Results report..."
                wbResults.Navigate(SummaryReport)
            End If

            .Close()
            .Dispose()
            ProgressForm = Nothing
        End With

        EnableControls(True)
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        With New OpenFileDialog
            .AddExtension = True
            .CheckFileExists = True
            .CheckPathExists = True
            .DefaultExt = ".Mercury"
            .Filter = "Mercury files (*.mercury)|*.mercury"
            .FilterIndex = 0
            .InitialDirectory = Project.MercuryFolder
            If Not My.Computer.FileSystem.DirectoryExists(.InitialDirectory) Then My.Computer.FileSystem.CreateDirectory(.InitialDirectory)
            .Title = "Open Mercury File"
            Dim merfiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Project.MercuryFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.mercury")
            If merfiles.Count > 0 Then .FileName = merfiles(0)
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                LoadData(.FileName)
                LoadForm()
                wbResults.DocumentText = ""
            End If
            .Dispose()
        End With
    End Sub

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        wbResults.ShowPrintPreviewDialog()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        wbResults.ShowPrintDialog()
    End Sub

    Private Sub btnSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAs.Click
        SaveForm()
        With New SaveFileDialog
            .AddExtension = True
            .CheckFileExists = False
            .CheckPathExists = True
            .DefaultExt = ".Mercury"
            .Filter = "Mercury files (*.mercury)|*.mercury"
            .FilterIndex = 0
            .InitialDirectory = Project.MercuryFolder
            If Not My.Computer.FileSystem.DirectoryExists(.InitialDirectory) Then My.Computer.FileSystem.CreateDirectory(.InitialDirectory)
            .Title = "Save Mercury File"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then Project.FileName = .FileName : SaveData()
            Text = "BASINS Mercury Estimator"
            If .FileName <> "" Then Text &= " - " & IO.Path.GetFileName(.FileName)
            .Dispose()
        End With
    End Sub

    Private Sub cboBMPIDField_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectionChangeCommitted, cboLandUseIDField.SelectionChangeCommitted, cboLandUseLayer.SelectionChangeCommitted, cboSoilIDField.SelectionChangeCommitted, cboSoilLayer.SelectionChangeCommitted, cboSubbasins.SelectionChangeCommitted, cboSediment.SelectionChangeCommitted
        Project.Modified = True
        wbResults.DocumentText = ""
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        cboLandUseLayer.Enabled = True
        lblLandUseLayer.Enabled = True
        cboLandUseIDField.Enabled = False
        lblLandUseIDField.Enabled = False
        cboLandUseLayer.Items.Clear()
        For lLyr As Integer = 0 To GisUtil.NumLayers() - 1
            Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(lLyr)
            If cboLanduse.Text.StartsWith("USGS GIRAS") Then
                If GisUtil.LayerFileName(lLyr).ToUpper.Contains("GIRAS") And lt = MapWindow.Interfaces.eLayerType.PolygonShapefile Then cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
                cboLandUseLayer.Enabled = False
                lblLandUseLayer.Enabled = False
            ElseIf cboLanduse.Text = "User Shapefile" Then
                If lt = MapWindow.Interfaces.eLayerType.PolygonShapefile Then cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
                cboLandUseIDField.Enabled = True
                lblLandUseIDField.Enabled = True
            ElseIf cboLanduse.Text.StartsWith("NLCD") Then
                If GisUtil.LayerFileName(lLyr).ToUpper.Contains("NLCD") And lt = MapWindow.Interfaces.eLayerType.Grid Then cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
            Else
                If lt = MapWindow.Interfaces.eLayerType.Grid Then cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
            End If
        Next
        If cboLandUseLayer.Items.Count > 0 Then cboLandUseLayer.SelectedIndex = 0
        Project.LanduseType = cboLanduse.SelectedIndex
        LoadForm()
    End Sub

    Private Sub cboLandUseLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLandUseLayer.SelectedIndexChanged
        If Not GisUtil.IsLayer(cboLandUseLayer.Text) Then Exit Sub
        Dim lyr As Integer = GisUtil.LayerIndex(cboLandUseLayer.Text)
        Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(lyr)
        With cboLandUseIDField.Items
            .Clear()
            If lt = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                For i As Integer = 0 To GisUtil.NumFields(lyr) - 1
                    .Add(GisUtil.FieldName(i, lyr))
                Next
            End If
        End With
    End Sub

    Private Sub cboSoilLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSoilLayer.SelectedIndexChanged
        If cboSoilLayer.SelectedIndex = -1 Then Exit Sub
        Dim lyr As Integer = GisUtil.LayerIndex(cboSoilLayer.Text)
        With cboSoilIDField.Items
            .Clear()
            For i As Integer = 0 To GisUtil.NumFields(lyr) - 1
                .Add(GisUtil.FieldName(i, lyr))
            Next
        End With
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        Logger.Msg("BASINS Mercury Estimator" & vbCrLf & vbCrLf & _
                   "Version " & System.Reflection.Assembly.GetEntryAssembly.GetName.Version.ToString, _
                   "BASINS - MERCURY")
    End Sub

    Private Sub EnableControls(ByVal b As Boolean)
        btnOK.Enabled = b
        btnHelp.Enabled = b
        btnCancel.Enabled = b
        btnAbout.Enabled = b
        tabMercury.Enabled = b
        btnOpen.Enabled = b
        btnSaveAs.Enabled = b
    End Sub

    Private Sub frmMercury_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        With Project
            If .Modified Then
                If .FileName = "" Then
                    btnSaveAs.PerformClick()
                    'e.Cancel = .FileName = ""
                Else
                    Select Case MessageBox.Show(String.Format("Save changes to {0}?", .FileName), "File Has Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                        Case Windows.Forms.DialogResult.Yes : SaveForm() : e.Cancel = Not SaveData()
                        Case Windows.Forms.DialogResult.No : e.Cancel = False
                        Case Windows.Forms.DialogResult.Cancel : e.Cancel = True
                    End Select
                End If
            End If
        End With
    End Sub

    Private Sub frmMercury_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Project.Initialize()

        With cboLanduse
            .Items.Clear()
            .Items.AddRange(New String() {"USGS GIRAS Shapefiles", "NLCD 1992 Grid", "NLCD 2001 Grid", "User Shapefile", "User Grid"})
            .SelectedIndex = 0
        End With

        cboSubbasins.Items.Clear()
        cboSoilLayer.Items.Clear()
        cboLandUseLayer.Items.Clear()
        cboSediment.Items.Clear()

        Try
            For i As Integer = 0 To GisUtil.NumLayers - 1
                Dim ln As String = GisUtil.LayerName(i)
                Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(i)

                If lt = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                    cboSubbasins.Items.Add(ln)
                    cboSoilLayer.Items.Add(ln)
                ElseIf lt = MapWindow.Interfaces.eLayerType.Grid Then
                    cboSediment.Items.Add(ln)
                End If
            Next

            'set reasonable defaults, in case Default.Mercury not found

            With cboSubbasins
                For Each item As String In .Items
                    If item.ToUpper.Contains("SUBBASIN") Then .Text = item : Exit For
                Next
            End With
            With cboSoilLayer
                For Each item As String In .Items
                    If item.ToUpper.Contains("SOIL") Then .Text = item : Exit For
                Next
            End With
            With cboSediment
                For Each item As String In .Items
                    If item.ToUpper.Contains("SEDIMENT") Then .Text = item : Exit For
                Next
            End With
            With cboLandUseLayer
                For Each item As String In .Items
                    If item.ToUpper.Contains("LANDUSE") Then .Text = item : Exit For
                Next
            End With

        Catch ex As Exception
            Logger.Message("Unable to connect to MapWindow", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Try

        LoadData()
        LoadForm()

        Dim merfiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Project.MercuryFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.mercury")
        Select Case merfiles.Count
            Case 0
            Case 1
                LoadData(merfiles(0))
                LoadForm()
                wbResults.DocumentText = ""
            Case Else
                btnOpen.PerformClick()
        End Select

    End Sub

    ''' <summary>
    ''' Move data from Project structure into form fields
    ''' </summary>
    Private Sub LoadForm()
        Try
            With Project
                Text = "BASINS Mercury Estimator"
                If .FileName <> "" Then Text &= " - " & IO.Path.GetFileName(.FileName)
                txtOutputName.Text = .OutputFolder
                txtGridSize.Text = .GridSize
                cboSubbasins.Text = .SubbasinLayer

                cboSoilLayer.Text = .SoilLayer
                cboSoilIDField.Text = .SoilField

                cboLanduse.SelectedIndex = .LanduseType
                cboLandUseLayer.Text = .LandUseLayer
                cboLandUseIDField.Text = .LandUseField

                For i As Integer = 1 To 2
                    With CType(Choose(i, dgSoil, dgLandUse), DataGridView)
                        .AllowUserToAddRows = False
                        .Rows.Clear()
                        If i = 1 Then
                            For Each key As String In Project.dictSoil.Keys
                                Dim soil As clsSoilLookup = Project.dictSoil.Item(key)
                                .Rows.Add(key, soil.Description, soil.HydGroup, soil.HydGroup, soil.HydGroup, soil.HydGroup, soil.HydGroup, soil.HydGroup, soil.HydGroup)
                            Next
                        Else
                            For Each key As String In Project.dictLandUse(Project.LanduseType).Keys
                                Dim land As clsLandUseLookup = Project.dictLandUse(Project.LanduseType).Item(key)
                                .Rows.Add(key, land.Description, land.GETCover, land.NGETCover, land.PctImperv, land.IsWater, land.CN_A, land.CN_B, land.CN_C, land.CN_D)
                            Next
                        End If
                        .Sort(.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
                        For c As Integer = 0 To .ColumnCount - 1
                            .Columns(c).SortMode = DataGridViewColumnSortMode.NotSortable
                            If c = 2 Then
                                .Columns(c).DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                            Else
                                .Columns(c).DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft
                            End If
                        Next
                        .Sort(.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
                        .AllowUserToAddRows = True
                        For c As Integer = 0 To .ColumnCount - 1
                            .Columns(c).SortMode = DataGridViewColumnSortMode.NotSortable
                        Next
                    End With
                Next i

                Dim csb As New OleDbConnectionStringBuilder
                'csb.d()
                'Dim dr As OleDb.OleDbDataReader=


            End With
        Catch ex As Exception
            Logger.Message("Error while trying to load form:" & vbCr & vbCr & ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Try
    End Sub

    ''' <summary>
    ''' Move data from form fields into Project structure
    ''' </summary>
    Friend Sub SaveForm()
        Try
            With Project
                Text = "BASINS Mercury Estimator"
                If .FileName <> "" Then Text &= " - " & IO.Path.GetFileName(.FileName)
                .OutputFolder = txtOutputName.Text
                .GridSize = txtGridSize.Text
                .SubbasinLayer = cboSubbasins.Text

                .SoilLayer = cboSoilLayer.Text
                .SoilField = cboSoilIDField.Text

                .LanduseType = cboLanduse.SelectedIndex
                .LandUseLayer = cboLandUseLayer.Text
                .LandUseField = cboLandUseIDField.Text

                .dictSoil.Clear()
                With dgSoil
                    .EndEdit()
                    For r As Integer = 0 To .RowCount - 2
                        With .Rows(r)
                            If Not (IsDBNull(.Cells(0).Value) Or IsDBNull(.Cells(2).Value)) And Not Project.dictSoil.ContainsKey(.Cells(0).Value) Then
                                Project.dictSoil.Add(.Cells(0).Value, New clsSoilLookup(.Cells(1).Value, .Cells(2).Value, .Cells(3).Value, .Cells(4).Value, .Cells(5).Value, .Cells(6).Value, .Cells(7).Value, .Cells(8).Value))
                            End If
                        End With
                    Next
                End With

                Dim dict As Generic.Dictionary(Of String, clsLandUseLookup) = .dictLandUse(cboLanduse.SelectedIndex)
                dict.Clear()
                With dgLandUse
                    .EndEdit()
                    For r As Integer = 0 To .RowCount - 2
                        With .Rows(r)
                            If Not (IsDBNull(.Cells(0).Value) Or IsDBNull(.Cells(2).Value)) And Not dict.ContainsKey(.Cells(0).Value) Then
                                dict.Add(.Cells(0).Value, New clsLandUseLookup(.Cells(1).Value, .Cells(2).Value, .Cells(3).Value, .Cells(4).Value, .Cells(5).Value, .Cells(6).Value, .Cells(7).Value, .Cells(8).Value, .Cells(9).Value))
                            End If
                        End With
                    Next
                End With
            End With
        Catch ex As Exception
            Logger.Message("Error while trying to save form:" & vbCr & vbCr & ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Try
    End Sub

    Private Sub txtGridSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGridSize.KeyPress, txtOutputName.KeyPress
        Project.Modified = True
        wbResults.DocumentText = ""
    End Sub

    Private Sub txtGridSize_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGridSize.Validated
        Project.Modified = True
        wbResults.DocumentText = ""
    End Sub
End Class

