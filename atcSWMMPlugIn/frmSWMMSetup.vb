Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Drawing
Imports System

Public Class frmSWMMSetup
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
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
    Friend WithEvents cboLanduse As System.Windows.Forms.ComboBox
    Friend WithEvents tbxName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents AtcGridPervious As atcControls.atcGrid
    Friend WithEvents cmdChange As System.Windows.Forms.Button
    Friend WithEvents lblClass As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents cboDescription As System.Windows.Forms.ComboBox
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents cboLandUseLayer As System.Windows.Forms.ComboBox
    Friend WithEvents lblLandUseLayer As System.Windows.Forms.Label
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents AtcGridPrec As atcControls.atcGrid
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtMetWDMName As System.Windows.Forms.TextBox
    Friend WithEvents cmdSelectWDM As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cboOutlets As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents ofdMetWDM As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cboOtherMet As System.Windows.Forms.ComboBox
    Friend WithEvents lblOtherMet As System.Windows.Forms.Label
    Friend WithEvents lblPrecipStation As System.Windows.Forms.Label
    Friend WithEvents rbnMultiple As System.Windows.Forms.RadioButton
    Friend WithEvents rbnSingle As System.Windows.Forms.RadioButton
    Friend WithEvents cboPrecipStation As System.Windows.Forms.ComboBox
    Friend WithEvents AtcConnectFields As atcControls.atcConnectFields
    Friend WithEvents ofdClass As System.Windows.Forms.OpenFileDialog
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
    Friend WithEvents ofdExisting As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSWMMSetup))
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdExisting = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.ofdExisting = New System.Windows.Forms.OpenFileDialog
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.cboOutlets = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.cboMet = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cboStreams = New System.Windows.Forms.ComboBox
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.tbxName = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.AtcGridPervious = New atcControls.atcGrid
        Me.cmdChange = New System.Windows.Forms.Button
        Me.lblClass = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.cboDescription = New System.Windows.Forms.ComboBox
        Me.lblDescription = New System.Windows.Forms.Label
        Me.cboLandUseLayer = New System.Windows.Forms.ComboBox
        Me.lblLandUseLayer = New System.Windows.Forms.Label
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.AtcConnectFields = New atcControls.atcConnectFields
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.rbnMultiple = New System.Windows.Forms.RadioButton
        Me.rbnSingle = New System.Windows.Forms.RadioButton
        Me.cboPrecipStation = New System.Windows.Forms.ComboBox
        Me.lblPrecipStation = New System.Windows.Forms.Label
        Me.cboOtherMet = New System.Windows.Forms.ComboBox
        Me.lblOtherMet = New System.Windows.Forms.Label
        Me.AtcGridPrec = New atcControls.atcGrid
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtMetWDMName = New System.Windows.Forms.TextBox
        Me.cmdSelectWDM = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.ofdMetWDM = New System.Windows.Forms.OpenFileDialog
        Me.ofdClass = New System.Windows.Forms.OpenFileDialog
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
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(16, 551)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 32)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "OK"
        '
        'cmdExisting
        '
        Me.cmdExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdExisting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExisting.Location = New System.Drawing.Point(96, 551)
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
        Me.cmdCancel.Location = New System.Drawing.Point(224, 551)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(88, 32)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Cancel"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(409, 551)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(79, 32)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(497, 551)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(87, 32)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "About"
        '
        'ofdExisting
        '
        Me.ofdExisting.DefaultExt = "inp"
        Me.ofdExisting.Filter = "SWMM INP files (*.inp)|*.inp"
        Me.ofdExisting.InitialDirectory = "/BASINS/modelout/"
        Me.ofdExisting.Title = "Select SWMM inp file"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(60, 21)
        Me.TabControl1.Location = New System.Drawing.Point(18, 17)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(564, 455)
        Me.TabControl1.TabIndex = 8
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.cboOutlets)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.cboMet)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.cboStreams)
        Me.TabPage1.Controls.Add(Me.cboSubbasins)
        Me.TabPage1.Controls.Add(Me.cboLanduse)
        Me.TabPage1.Controls.Add(Me.tbxName)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(556, 426)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "General"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'cboOutlets
        '
        Me.cboOutlets.AllowDrop = True
        Me.cboOutlets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboOutlets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOutlets.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOutlets.Location = New System.Drawing.Point(168, 228)
        Me.cboOutlets.Name = "cboOutlets"
        Me.cboOutlets.Size = New System.Drawing.Size(378, 25)
        Me.cboOutlets.TabIndex = 14
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(11, 231)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(93, 17)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Nodes Layer:"
        '
        'cboMet
        '
        Me.cboMet.AllowDrop = True
        Me.cboMet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMet.Location = New System.Drawing.Point(168, 272)
        Me.cboMet.Name = "cboMet"
        Me.cboMet.Size = New System.Drawing.Size(376, 25)
        Me.cboMet.TabIndex = 12
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(11, 275)
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
        Me.cboStreams.Location = New System.Drawing.Point(168, 183)
        Me.cboStreams.Name = "cboStreams"
        Me.cboStreams.Size = New System.Drawing.Size(376, 25)
        Me.cboStreams.TabIndex = 9
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubbasins.Location = New System.Drawing.Point(168, 136)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(376, 25)
        Me.cboSubbasins.TabIndex = 8
        '
        'cboLanduse
        '
        Me.cboLanduse.AllowDrop = True
        Me.cboLanduse.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLanduse.Location = New System.Drawing.Point(168, 88)
        Me.cboLanduse.Name = "cboLanduse"
        Me.cboLanduse.Size = New System.Drawing.Size(376, 25)
        Me.cboLanduse.TabIndex = 7
        '
        'tbxName
        '
        Me.tbxName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbxName.Location = New System.Drawing.Point(168, 40)
        Me.tbxName.Name = "tbxName"
        Me.tbxName.Size = New System.Drawing.Size(210, 23)
        Me.tbxName.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(11, 187)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(107, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Conduits Layer:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(11, 140)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(126, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Catchments Layer:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(11, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Land Use Type:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(145, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SWMM Project Name:"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.AtcGridPervious)
        Me.TabPage2.Controls.Add(Me.cmdChange)
        Me.TabPage2.Controls.Add(Me.lblClass)
        Me.TabPage2.Controls.Add(Me.Label20)
        Me.TabPage2.Controls.Add(Me.cboDescription)
        Me.TabPage2.Controls.Add(Me.lblDescription)
        Me.TabPage2.Controls.Add(Me.cboLandUseLayer)
        Me.TabPage2.Controls.Add(Me.lblLandUseLayer)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(556, 426)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Land Use"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'AtcGridPervious
        '
        Me.AtcGridPervious.AllowHorizontalScrolling = True
        Me.AtcGridPervious.AllowNewValidValues = False
        Me.AtcGridPervious.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridPervious.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridPervious.Fixed3D = False
        Me.AtcGridPervious.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridPervious.LineColor = System.Drawing.Color.Empty
        Me.AtcGridPervious.LineWidth = 0.0!
        Me.AtcGridPervious.Location = New System.Drawing.Point(14, 148)
        Me.AtcGridPervious.Name = "AtcGridPervious"
        Me.AtcGridPervious.Size = New System.Drawing.Size(527, 263)
        Me.AtcGridPervious.Source = Nothing
        Me.AtcGridPervious.TabIndex = 18
        '
        'cmdChange
        '
        Me.cmdChange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChange.Location = New System.Drawing.Point(469, 117)
        Me.cmdChange.Name = "cmdChange"
        Me.cmdChange.Size = New System.Drawing.Size(72, 24)
        Me.cmdChange.TabIndex = 17
        Me.cmdChange.Text = "Change"
        '
        'lblClass
        '
        Me.lblClass.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblClass.Location = New System.Drawing.Point(164, 118)
        Me.lblClass.Name = "lblClass"
        Me.lblClass.Size = New System.Drawing.Size(364, 20)
        Me.lblClass.TabIndex = 16
        Me.lblClass.Text = "<none>"
        Me.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(11, 121)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(120, 17)
        Me.Label20.TabIndex = 15
        Me.Label20.Text = "Classification File:"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboDescription
        '
        Me.cboDescription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDescription.Location = New System.Drawing.Point(168, 80)
        Me.cboDescription.Name = "cboDescription"
        Me.cboDescription.Size = New System.Drawing.Size(168, 25)
        Me.cboDescription.TabIndex = 12
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(11, 83)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(128, 17)
        Me.lblDescription.TabIndex = 11
        Me.lblDescription.Text = "Classification Field:"
        '
        'cboLandUseLayer
        '
        Me.cboLandUseLayer.AllowDrop = True
        Me.cboLandUseLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLandUseLayer.Location = New System.Drawing.Point(168, 40)
        Me.cboLandUseLayer.Name = "cboLandUseLayer"
        Me.cboLandUseLayer.Size = New System.Drawing.Size(376, 25)
        Me.cboLandUseLayer.TabIndex = 10
        '
        'lblLandUseLayer
        '
        Me.lblLandUseLayer.AutoSize = True
        Me.lblLandUseLayer.Location = New System.Drawing.Point(11, 44)
        Me.lblLandUseLayer.Name = "lblLandUseLayer"
        Me.lblLandUseLayer.Size = New System.Drawing.Size(113, 17)
        Me.lblLandUseLayer.TabIndex = 9
        Me.lblLandUseLayer.Text = "Land Use Layer:"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.AtcConnectFields)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(556, 426)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Field Mapping"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'AtcConnectFields
        '
        Me.AtcConnectFields.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcConnectFields.Location = New System.Drawing.Point(2, 3)
        Me.AtcConnectFields.Name = "AtcConnectFields"
        Me.AtcConnectFields.Size = New System.Drawing.Size(551, 420)
        Me.AtcConnectFields.TabIndex = 2
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.rbnMultiple)
        Me.TabPage6.Controls.Add(Me.rbnSingle)
        Me.TabPage6.Controls.Add(Me.cboPrecipStation)
        Me.TabPage6.Controls.Add(Me.lblPrecipStation)
        Me.TabPage6.Controls.Add(Me.cboOtherMet)
        Me.TabPage6.Controls.Add(Me.lblOtherMet)
        Me.TabPage6.Controls.Add(Me.AtcGridPrec)
        Me.TabPage6.Controls.Add(Me.GroupBox2)
        Me.TabPage6.Location = New System.Drawing.Point(4, 25)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(556, 426)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Met Stations"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'rbnMultiple
        '
        Me.rbnMultiple.AutoSize = True
        Me.rbnMultiple.Location = New System.Drawing.Point(222, 96)
        Me.rbnMultiple.Name = "rbnMultiple"
        Me.rbnMultiple.Size = New System.Drawing.Size(173, 21)
        Me.rbnMultiple.TabIndex = 25
        Me.rbnMultiple.Text = "Multiple Precip Stations"
        Me.rbnMultiple.UseVisualStyleBackColor = True
        '
        'rbnSingle
        '
        Me.rbnSingle.AutoSize = True
        Me.rbnSingle.Checked = True
        Me.rbnSingle.Location = New System.Drawing.Point(27, 96)
        Me.rbnSingle.Name = "rbnSingle"
        Me.rbnSingle.Size = New System.Drawing.Size(157, 21)
        Me.rbnSingle.TabIndex = 24
        Me.rbnSingle.TabStop = True
        Me.rbnSingle.Text = "Single Precip Station"
        Me.rbnSingle.UseVisualStyleBackColor = True
        '
        'cboPrecipStation
        '
        Me.cboPrecipStation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPrecipStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrecipStation.FormattingEnabled = True
        Me.cboPrecipStation.Location = New System.Drawing.Point(137, 136)
        Me.cboPrecipStation.Name = "cboPrecipStation"
        Me.cboPrecipStation.Size = New System.Drawing.Size(395, 25)
        Me.cboPrecipStation.TabIndex = 23
        '
        'lblPrecipStation
        '
        Me.lblPrecipStation.AutoSize = True
        Me.lblPrecipStation.Location = New System.Drawing.Point(24, 139)
        Me.lblPrecipStation.Name = "lblPrecipStation"
        Me.lblPrecipStation.Size = New System.Drawing.Size(100, 17)
        Me.lblPrecipStation.TabIndex = 22
        Me.lblPrecipStation.Text = "Precip Station:"
        '
        'cboOtherMet
        '
        Me.cboOtherMet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboOtherMet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOtherMet.FormattingEnabled = True
        Me.cboOtherMet.Location = New System.Drawing.Point(136, 378)
        Me.cboOtherMet.Name = "cboOtherMet"
        Me.cboOtherMet.Size = New System.Drawing.Size(396, 25)
        Me.cboOtherMet.TabIndex = 21
        '
        'lblOtherMet
        '
        Me.lblOtherMet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblOtherMet.AutoSize = True
        Me.lblOtherMet.Location = New System.Drawing.Point(18, 381)
        Me.lblOtherMet.Name = "lblOtherMet"
        Me.lblOtherMet.Size = New System.Drawing.Size(109, 17)
        Me.lblOtherMet.TabIndex = 20
        Me.lblOtherMet.Text = "Other Met Data:"
        '
        'AtcGridPrec
        '
        Me.AtcGridPrec.AllowHorizontalScrolling = True
        Me.AtcGridPrec.AllowNewValidValues = False
        Me.AtcGridPrec.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridPrec.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridPrec.Fixed3D = False
        Me.AtcGridPrec.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridPrec.LineColor = System.Drawing.Color.Empty
        Me.AtcGridPrec.LineWidth = 0.0!
        Me.AtcGridPrec.Location = New System.Drawing.Point(21, 136)
        Me.AtcGridPrec.Name = "AtcGridPrec"
        Me.AtcGridPrec.Size = New System.Drawing.Size(511, 225)
        Me.AtcGridPrec.Source = Nothing
        Me.AtcGridPrec.TabIndex = 19
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.txtMetWDMName)
        Me.GroupBox2.Controls.Add(Me.cmdSelectWDM)
        Me.GroupBox2.Location = New System.Drawing.Point(21, 20)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(512, 59)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Met WDM File"
        '
        'txtMetWDMName
        '
        Me.txtMetWDMName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMetWDMName.Location = New System.Drawing.Point(21, 27)
        Me.txtMetWDMName.Name = "txtMetWDMName"
        Me.txtMetWDMName.ReadOnly = True
        Me.txtMetWDMName.Size = New System.Drawing.Size(384, 23)
        Me.txtMetWDMName.TabIndex = 2
        '
        'cmdSelectWDM
        '
        Me.cmdSelectWDM.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSelectWDM.Location = New System.Drawing.Point(421, 25)
        Me.cmdSelectWDM.Name = "cmdSelectWDM"
        Me.cmdSelectWDM.Size = New System.Drawing.Size(80, 27)
        Me.cmdSelectWDM.TabIndex = 1
        Me.cmdSelectWDM.Text = "Select"
        Me.cmdSelectWDM.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblStatus)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(18, 479)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(566, 55)
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
        Me.lblStatus.Size = New System.Drawing.Size(535, 16)
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
        'ofdClass
        '
        Me.ofdClass.DefaultExt = "dbf"
        Me.ofdClass.Filter = "DBF Files (*.dbf)|*.dbf"
        Me.ofdClass.Title = "Select Classification File"
        '
        'GroupBox3
        '
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
        Me.GroupBox3.Location = New System.Drawing.Point(15, 312)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(529, 104)
        Me.GroupBox3.TabIndex = 27
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
        Me.lblDay.Location = New System.Drawing.Point(291, 17)
        Me.lblDay.Name = "lblDay"
        Me.lblDay.Size = New System.Drawing.Size(33, 17)
        Me.lblDay.TabIndex = 35
        Me.lblDay.Text = "Day"
        '
        'lblMonth
        '
        Me.lblMonth.AutoSize = True
        Me.lblMonth.Location = New System.Drawing.Point(234, 17)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.Size = New System.Drawing.Size(47, 17)
        Me.lblMonth.TabIndex = 34
        Me.lblMonth.Text = "Month"
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Location = New System.Drawing.Point(150, 17)
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
        'frmSWMMSetup
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(601, 596)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdExisting)
        Me.Controls.Add(Me.cmdOK)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmSWMMSetup"
        Me.Text = "BASINS SWMM"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend pPlugIn As PlugIn
    Friend pNodeFieldMap As New atcUtility.atcCollection
    Friend pConduitFieldMap As New atcUtility.atcCollection
    Friend pCatchmentFieldMap As New atcUtility.atcCollection
    Friend pPrecStations As atcCollection
    Friend pMetStations As atcCollection
    Friend pBasinsFolder As String
    Private pInitializing As Boolean = True

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboSubbasins_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubbasins.SelectedIndexChanged
        SetFieldMappingControl()
        SetPrecipStationGrid()
    End Sub

    Private Sub cboStreams_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStreams.SelectedIndexChanged
        SetFieldMappingControl()
    End Sub

    Private Sub cboOutlets_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboOutlets.SelectedIndexChanged
        SetFieldMappingControl()
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        SetLanduseTab(cboLanduse.Items(cboLanduse.SelectedIndex))
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("BASINS SWMM for MapWindow" & vbCrLf & vbCrLf & "Version 1.0", MsgBoxStyle.OkOnly, "BASINS SWMM")
    End Sub

    Private Sub cmdExisting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExisting.Click
        If ofdExisting.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Logger.Dbg("Run SWMM with " & ofdExisting.FileName)
            pPlugIn.SWMMProject.Run(ofdExisting.FileName)
        End If
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\SWMM.html")
    End Sub

    Private Sub frmSWMMSetup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\SWMM.html")
        End If
    End Sub

    Private Sub cboLandUseLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLandUseLayer.SelectedIndexChanged
        cboDescription.Items.Clear()
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex))
        Logger.Dbg("LandUseLayerIndex " & lLayerIndex & "(" & cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex) & ")")
        If lLayerIndex > -1 Then
            If cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Grid" Then
                'make sure this is a grid layer
                If GisUtil.LayerType(lLayerIndex) = 4 Then
                    'todo: fill in description fields for selected grid layer if possible
                End If
            Else
                'make sure this is a shape layer
                If GisUtil.LayerType(lLayerIndex) = 3 Then  'PolygonShapefile
                    'this is the layer, fill in fields 
                    For lFieldIndex As Integer = 0 To GisUtil.NumFields(lLayerIndex) - 1
                        'MsgBox(sf.Field(i).Name)
                        cboDescription.Items.Add(GisUtil.FieldName(lFieldIndex, lLayerIndex))
                        If GisUtil.FieldType(lFieldIndex, lLayerIndex) = 0 Then 'string
                            cboDescription.SelectedIndex = lFieldIndex
                        End If
                    Next
                    If cboDescription.Items.Count > 0 And cboDescription.SelectedIndex < 0 Then
                        cboDescription.SelectedIndex = 0
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub cboDescription_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDescription.SelectedIndexChanged
        SetPerviousGrid()
    End Sub

    Friend Sub EnableControls(ByVal aEnabled As Boolean)
        cmdOK.Enabled = aEnabled
        cmdExisting.Enabled = aEnabled
        cmdCancel.Enabled = aEnabled
        If Not pInitializing Then
            cmdHelp.Enabled = aEnabled
            cmdAbout.Enabled = aEnabled
        End If
    End Sub

    Private Sub SetLanduseTab(ByVal aLanduseSelection As String)
        Logger.Dbg("SetLanduseTab " & aLanduseSelection)
        If aLanduseSelection = "USGS GIRAS Shapefile" Then
            cboLandUseLayer.Visible = False
            lblLandUseLayer.Visible = False
            cboDescription.Visible = False
            lblDescription.Visible = False
            lblClass.Text = pBasinsFolder & "\etc\giras.dbf"
            SetPerviousGrid()
        ElseIf aLanduseSelection = "Other Shapefile" Then
            cboLandUseLayer.Items.Clear()
            Dim lLayerDefaultIndex As Integer = 0
            For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerType(lLayerIndex) = 3 Then 'PolygonShapefile
                    cboLandUseLayer.Items.Add(GisUtil.LayerName(lLayerIndex))
                    If GisUtil.NumFeatures(lLayerIndex) < 1000 And lLayerDefaultIndex = 0 Then
                        lLayerDefaultIndex = cboLandUseLayer.Items.Count
                    End If
                End If
            Next
            If cboLandUseLayer.Items.Count > 0 And cboLandUseLayer.SelectedIndex < 0 Then
                'pick one without too many polygons for efficiency
                cboLandUseLayer.SelectedIndex = lLayerDefaultIndex - 1
            End If
            cboLandUseLayer.Visible = True
            lblLandUseLayer.Visible = True
            cboDescription.Visible = True
            lblDescription.Visible = True
            lblClass.Text = "<none>"
            SetPerviousGrid()
        ElseIf aLanduseSelection = "NLCD Grid" Then
            cboLandUseLayer.Items.Clear()
            For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerType(lLayerIndex) = 4 Then  'Grid 
                    If InStr(UCase(GisUtil.LayerFileName(lLayerIndex)), "\NLCD\") > 0 Then
                        cboLandUseLayer.Items.Add(GisUtil.LayerName(lLayerIndex))
                    End If
                End If
            Next
            If cboLandUseLayer.Items.Count > 0 And cboLandUseLayer.SelectedIndex < 0 Then
                cboLandUseLayer.SelectedIndex = 0
            End If
            cboLandUseLayer.Visible = True
            lblLandUseLayer.Visible = True
            cboDescription.Visible = False
            lblDescription.Visible = False
            lblClass.Text = pBasinsFolder & "\etc\nlcd.dbf"
            SetPerviousGrid()
        Else 'grid
            cboLandUseLayer.Items.Clear()
            For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerType(lLayerIndex) = 4 Then  'Grid
                    cboLandUseLayer.Items.Add(GisUtil.LayerName(lLayerIndex))
                End If
            Next
            If cboLandUseLayer.Items.Count > 0 And cboLandUseLayer.SelectedIndex < 0 Then
                cboLandUseLayer.SelectedIndex = 0
            End If
            cboLandUseLayer.Visible = True
            lblLandUseLayer.Visible = True
            cboDescription.Visible = False
            lblDescription.Visible = False
            lblClass.Text = "<none>"
            SetPerviousGrid()
        End If
    End Sub

    Private Sub SetPerviousGrid()
        If AtcGridPervious.Source Is Nothing Then
            Logger.Dbg("No atcGridPervious to Set")
        Else
            AtcGridPervious.Clear()
            With AtcGridPervious.Source
                .Rows = 1
                .Columns = 5
                .CellValue(0, 0) = "Code"
                .CellValue(0, 1) = "Group Description"
                .CellValue(0, 2) = "Impervious Percent"
                .CellValue(0, 3) = "Multiplier"
                .CellValue(0, 4) = "Subbasin"
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 2
            End With

            If lblClass.Text <> "<none>" And _
              (cboLandUseLayer.Visible = False Or _
              (cboLandUseLayer.Visible And cboLandUseLayer.SelectedIndex > -1)) Then
                'giras, nlcd, or other with reclass file set
                Dim lReclassTable As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(lblClass.Text)
                'do pre-scan to set up grid
                Dim lPrevCode As Integer = -1
                Dim lShowMults As Boolean = False
                Dim lShowCodes As Boolean = False
                Dim lGroupNames As New Collection
                Dim lGroupPercent As New Collection  'dont want to use atccollection because we may want to add mult times
                Dim lGroupIndex As Integer
                For lRecordIndex As Integer = 1 To lReclassTable.NumRecords
                    'scan to see if multiple records for the same code
                    lReclassTable.CurrentRecord = lRecordIndex
                    Dim lCode As Long = lReclassTable.Value(1)
                    If lCode = lPrevCode Then
                        lShowMults = True
                    End If
                    lPrevCode = lCode
                    'scan to see if perv percent varies within a group
                    Dim lInCollection As Boolean = False
                    For lGroupIndex = 1 To lGroupNames.Count
                        If lGroupNames(lGroupIndex) = lReclassTable.Value(2) Then
                            lInCollection = True
                            If lGroupPercent(lGroupIndex) <> lReclassTable.Value(3) Then
                                lShowCodes = True
                            End If
                            Exit For
                        End If
                    Next lGroupIndex

                    If Not lInCollection Then
                        lGroupNames.Add(lReclassTable.Value(2))
                        lGroupPercent.Add(lReclassTable.Value(3))
                    End If
                Next lRecordIndex

                If lShowMults Then
                    lShowCodes = True
                End If

                'sort list items
                Dim llReclassTableSorted As New atcCollection
                For lRecordIndex As Integer = 1 To lReclassTable.NumRecords
                    lReclassTable.CurrentRecord = lRecordIndex
                    llReclassTableSorted.Add(lRecordIndex, lReclassTable.Value(1))
                Next lRecordIndex
                llReclassTableSorted.SortByValue()

                'now populate grid
                With AtcGridPervious.Source
                    For Each lRow As Integer In llReclassTableSorted.Keys
                        lReclassTable.CurrentRecord = lRow
                        If Not lShowCodes Then
                            'just show group desc and percent perv
                            Dim lInCollection As Boolean = False
                            For lRowIndex As Integer = 1 To .Rows
                                If .CellValue(lRowIndex - 1, 1) = lReclassTable.Value(2) Then
                                    lInCollection = True
                                End If
                            Next
                            If Not lInCollection Then
                                .Rows += 1
                                .CellValue(.Rows - 1, 1) = lReclassTable.Value(2)
                                .CellValue(.Rows - 1, 2) = lReclassTable.Value(3)
                                .CellEditable(.Rows - 1, 2) = True
                                .CellColor(.Rows - 1, 1) = Me.BackColor
                            End If
                        Else 'need to show whole table
                            If lReclassTable.Value(1) > 0 Then
                                .Rows += 1
                                .CellValue(.Rows - 1, 0) = lReclassTable.Value(1)
                                .CellValue(.Rows - 1, 1) = lReclassTable.Value(2)
                                .CellValue(.Rows - 1, 2) = lReclassTable.Value(3)
                                .CellValue(.Rows - 1, 3) = lReclassTable.Value(4)
                                .CellValue(.Rows - 1, 4) = lReclassTable.Value(5)
                            End If
                        End If
                    Next
                End With

                AtcGridPervious.SizeAllColumnsToContents()
                If lShowMults Then
                    lShowCodes = True
                Else
                    AtcGridPervious.ColumnWidth(3) = 0
                    AtcGridPervious.ColumnWidth(4) = 0
                End If
                If Not lShowCodes Then
                    AtcGridPervious.ColumnWidth(0) = 0
                End If
            ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
                If cboLandUseLayer.SelectedIndex > -1 And cboDescription.SelectedIndex > -1 Then
                    Dim lLayerNameLandUse As String = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
                    Dim lFieldNameLandUse As String = cboDescription.Items(cboDescription.SelectedIndex)
                    'no reclass file, get unique landuse names
                    Dim lLayerIndex As Integer = GisUtil.LayerIndex(lLayerNameLandUse)
                    If lLayerIndex > -1 Then
                        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                        FillListUniqueLandUses(lLayerIndex, lFieldNameLandUse)
                        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                    End If
                End If
            Else 'other grid types with no reclass file set
                If cboLandUseLayer.SelectedIndex > -1 Then
                    Dim lLayerNameLandUse As String = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
                    'get unique landuse names
                    Dim lLayerIndex As Integer = GisUtil.LayerIndex(lLayerNameLandUse)
                    If GisUtil.LayerType(lLayerIndex) = 4 Then 'Grid
                        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                        For lGridX As Integer = Convert.ToInt32(GisUtil.GridLayerMinimum(lLayerIndex)) To Convert.ToInt32(GisUtil.GridLayerMaximum(lLayerIndex))
                            AtcGridPervious.Source.Rows += 1
                            AtcGridPervious.Source.CellValue(AtcGridPervious.Source.Rows - 1, 0) = lGridX
                            AtcGridPervious.Source.CellValue(AtcGridPervious.Source.Rows - 1, 1) = lGridX
                            AtcGridPervious.Source.CellValue(AtcGridPervious.Source.Rows - 1, 2) = 100
                        Next lGridX
                        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                    End If
                End If
                AtcGridPervious.SizeAllColumnsToContents()
                AtcGridPervious.ColumnWidth(0) = 0
                AtcGridPervious.ColumnWidth(3) = 0
                AtcGridPervious.ColumnWidth(4) = 0
            End If

            With AtcGridPervious.Source
                .CellColor(0, 0) = SystemColors.ControlDark
                .CellColor(0, 1) = SystemColors.ControlDark
                .CellColor(0, 2) = SystemColors.ControlDark
                .CellColor(0, 3) = SystemColors.ControlDark
                .CellColor(0, 4) = SystemColors.ControlDark
                For lRowIndex As Integer = 1 To .Rows - 1
                    .CellEditable(lRowIndex, 2) = True
                    .CellEditable(lRowIndex, 3) = True
                    .CellEditable(lRowIndex, 4) = True
                    .CellColor(lRowIndex, 0) = SystemColors.ControlDark
                    .CellColor(lRowIndex, 1) = SystemColors.ControlDark
                Next lRowIndex
            End With
            AtcGridPervious.Refresh()
        End If
    End Sub

    Private Sub SetFieldMappingControl()
        If pInitializing Then
            Logger.Dbg("SetFieldMappingControl Skipped while initializing")
        Else
            Logger.Dbg("SetFieldMappingControl Begin")

            'add source fields from dbf
            AtcConnectFields.lstSource.Items.Clear()
            If cboOutlets.SelectedIndex > 0 Then
                Dim lNodeLayerIndex As Integer = GisUtil.LayerIndex(cboOutlets.Items(cboOutlets.SelectedIndex))
                For lFieldIndex As Integer = 0 To GisUtil.NumFields(lNodeLayerIndex) - 1
                    AtcConnectFields.lstSource.Items.Add("Node:" & GisUtil.FieldName(lFieldIndex, lNodeLayerIndex))
                Next
            End If
            If cboStreams.SelectedIndex > -1 Then
                Dim lConduitLayerIndex As Integer = GisUtil.LayerIndex(cboStreams.Items(cboStreams.SelectedIndex))
                For lFieldIndex As Integer = 0 To GisUtil.NumFields(lConduitLayerIndex) - 1
                    AtcConnectFields.lstSource.Items.Add("Conduit:" & GisUtil.FieldName(lFieldIndex, lConduitLayerIndex))
                Next
            End If
            If cboSubbasins.SelectedIndex > -1 Then
                Dim lCatchmentLayerIndex As Integer = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
                For lFieldIndex As Integer = 0 To GisUtil.NumFields(lCatchmentLayerIndex) - 1
                    AtcConnectFields.lstSource.Items.Add("Catchment:" & GisUtil.FieldName(lFieldIndex, lCatchmentLayerIndex))
                Next
            End If

            'add target properties from introspection on the swmm classes
            AtcConnectFields.lstTarget.Items.Clear()
            Dim lNode As New atcSWMM.Node
            For Each lField As Reflection.FieldInfo In lNode.GetType.GetFields
                AtcConnectFields.lstTarget.Items.Add("Node:" & lField.Name)
            Next
            Dim lConduit As New atcSWMM.Conduit
            For Each lField As Reflection.FieldInfo In lConduit.GetType.GetFields
                AtcConnectFields.lstTarget.Items.Add("Conduit:" & lField.Name)
            Next
            Dim lCatchment As New atcSWMM.Catchment
            For Each lField As Reflection.FieldInfo In lCatchment.GetType.GetFields
                AtcConnectFields.lstTarget.Items.Add("Catchment:" & lField.Name)
            Next

            'add existing connections from default field maps
            AtcConnectFields.lstConnections.Items.Clear()
            Dim lConn As String
            Dim lType As String = "Node"
            For lIndex As Integer = 0 To pNodeFieldMap.Count - 1
                lConn = lType & ":" & pNodeFieldMap.Keys(lIndex) & " <-> " & lType & ":" & pNodeFieldMap(lIndex)
                AtcConnectFields.AddConnection(lConn, True)
            Next
            lType = "Conduit"
            For lIndex As Integer = 0 To pConduitFieldMap.Count - 1
                lConn = lType & ":" & pConduitFieldMap.Keys(lIndex) & " <-> " & lType & ":" & pConduitFieldMap(lIndex)
                AtcConnectFields.AddConnection(lConn, True)
            Next
            lType = "Catchment"
            For lIndex As Integer = 0 To pCatchmentFieldMap.Count - 1
                lConn = lType & ":" & pCatchmentFieldMap.Keys(lIndex) & " <-> " & lType & ":" & pCatchmentFieldMap(lIndex)
                AtcConnectFields.AddConnection(lConn, True)
            Next
        End If
    End Sub

    Private Sub FillListUniqueLandUses(ByVal aLayerIndex As Long, ByVal aFieldName As String)
        Dim lFieldIndex As Integer = GisUtil.FieldIndex(aLayerIndex, aFieldName)
        If lFieldIndex > -1 Then 'this is the field we want, get land use types
            Dim lUnique As New atcCollection
            For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(aLayerIndex) - 1
                Dim lFieldValue As String = GisUtil.FieldValue(aLayerIndex, lFeatureIndex, lFieldIndex)
                If lUnique.IndexFromKey(lFieldValue) = -1 Then 'new land use
                    With AtcGridPervious.Source
                        .Rows += 1
                        .CellValue(.Rows - 1, 1) = lFieldValue
                        .CellValue(.Rows - 1, 0) = lFieldValue
                        If lFieldValue.ToUpper.StartsWith("URBAN") Then
                            .CellValue(.Rows - 1, 2) = 50
                        Else
                            .CellValue(.Rows - 1, 2) = 0
                        End If
                        .CellEditable(.Rows - 1, 2) = True
                        lUnique.Add(lFieldValue)
                    End With
                End If
            Next lFeatureIndex

            With AtcGridPervious
                .SizeAllColumnsToContents()
                .ColumnWidth(0) = 0 'hide
                .ColumnWidth(3) = 0
                .ColumnWidth(4) = 0
            End With
        End If
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Logger.Dbg("Setup SWMM input files")
        'set field mapping as specified in field mapping tab
        pNodeFieldMap.Clear()
        pConduitFieldMap.Clear()
        pCatchmentFieldMap.Clear()
        For lIndex As Integer = 0 To AtcConnectFields.lstConnections.Items.Count - 1
            Dim lTxt As String = AtcConnectFields.lstConnections.Items(lIndex)
            Dim lBaseLen As Integer = 0
            Dim lBaseName As String = ""
            If Mid(lTxt, 1, 4) = "Node" Then
                lBaseLen = 4
                lBaseName = "Node"
            ElseIf Mid(lTxt, 1, 7) = "Conduit" Then
                lBaseLen = 7
                lBaseName = "Conduit"
            ElseIf Mid(lTxt, 1, 9) = "Catchment" Then
                lBaseLen = 9
                lBaseName = "Catchment"
            End If
            Dim lSpacePos As Integer = InStr(lTxt, " ")
            Dim lGTPos As Integer = InStr(lTxt, ">")
            Dim lSrc As String = Mid(lTxt, lBaseLen + 2, lSpacePos - lBaseLen - 2)

            If Mid(lTxt, lGTPos + 2, lBaseLen) = lBaseName Then
                Dim lTar As String = Mid(lTxt, lGTPos + lBaseLen + 3)
                If Mid(lTxt, 1, 4) = "Node" Then
                    pNodeFieldMap.Add(lSrc, lTar)
                ElseIf Mid(lTxt, 1, 7) = "Conduit" Then
                    pConduitFieldMap.Add(lSrc, lTar)
                ElseIf Mid(lTxt, 1, 9) = "Catchment" Then
                    pCatchmentFieldMap.Add(lSrc, lTar)
                End If
            Else
                'trying to add field mapping of different types, like a node to a catchment, wont work
                Logger.Dbg("Problem adding field map " & lTxt)
            End If
        Next

        'set file names for nodes, conduits, and catchments
        Dim lNodesShapefileName As String = ""
        If cboOutlets.SelectedIndex > 0 Then
            Dim lNodeLayerIndex As Integer = GisUtil.LayerIndex(cboOutlets.Items(cboOutlets.SelectedIndex))
            lNodesShapefileName = GisUtil.LayerFileName(lNodeLayerIndex)
        End If

        Dim lConduitLayerIndex As Integer = GisUtil.LayerIndex(cboStreams.Items(cboStreams.SelectedIndex))
        Dim lConduitShapefileName As String = GisUtil.LayerFileName(lConduitLayerIndex)

        Dim lCatchmentLayerIndex As Integer = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
        Dim lCatchmentShapefileName As String = GisUtil.LayerFileName(lCatchmentLayerIndex)

        'set file names for met stations
        Dim lMetShapefileName As String = ""
        If cboMet.SelectedIndex > 0 Then
            Dim lMetLayerIndex As Integer = GisUtil.LayerIndex(cboMet.Items(cboMet.SelectedIndex))
            lMetShapefileName = GisUtil.LayerFileName(lMetLayerIndex)
        End If
        Dim lMetWDMFileName As String = txtMetWDMName.Text

        'set reclassification records
        Dim lReclassificationRecords As New atcCollection
        Dim lCodesVisible As Boolean = True
        If AtcGridPervious.ColumnWidth(0) = 0 Then lCodesVisible = False
        GetLanduseReclassificationDetails(lblClass.Text, lCodesVisible, AtcGridPervious.Source, lReclassificationRecords)

        'Dim lNodesShapefileName As String = "C:\basins\Predefined Delineations\West Branch\nodes.shp"
        'Dim lConduitShapefileName As String = "C:\basins\Predefined Delineations\West Branch\conduits.shp"
        'Dim lCatchmentShapefileName As String = "C:\basins\Predefined Delineations\West Branch\catchments.shp"

        lblStatus.Text = "Preparing to process"
        Me.Refresh()
        EnableControls(False)

        Dim lName As String = tbxName.Text
        'TODO: still use modelout?
        Dim lSWMMProjectFileName As String = pBasinsFolder & "\modelout\" & lName & "\" & lName & ".inp"
        MkDirPath(PathNameOnly(lSWMMProjectFileName))

        If PreProcessChecking(lSWMMProjectFileName) Then
            With pPlugIn.SWMMProject
                .Name = lName
                .Title = "SWMM Project Written from BASINS"

                Dim lPrecGageNamesByCatchment As New Collection
                Dim lSelectedStation As StationDetails

                If rbnSingle.Checked Then
                    If cboPrecipStation.SelectedIndex > -1 Then
                        lSelectedStation = pPrecStations.ItemByKey(cboPrecipStation.Items(cboPrecipStation.SelectedIndex))
                        'use this precip gage for each catchment
                        lPrecGageNamesByCatchment.Add(lSelectedStation.Name)
                        'create rain gages from shapefile and selected station
                        CreateRaingageFromShapefile(lMetShapefileName, lSelectedStation.Name, .RainGages)
                    End If
                Else
                    For lrow As Integer = 1 To AtcGridPrec.Source.Rows - 1
                        lSelectedStation = pPrecStations.ItemByKey(AtcGridPrec.Source.CellValue(lrow, 1))
                        'remember which precip gage goes with each catchment
                        lPrecGageNamesByCatchment.Add(lSelectedStation.Name)
                        'create rain gages from shapefile and selected station
                        CreateRaingageFromShapefile(lMetShapefileName, lSelectedStation.Name, .RainGages)
                    Next
                End If

                Dim lMetGageName As String = ""
                If cboOtherMet.SelectedIndex > -1 Then
                    lSelectedStation = pMetStations.ItemByKey(cboOtherMet.Items(cboOtherMet.SelectedIndex))
                    lMetGageName = lSelectedStation.Name
                End If

                'create met constituents from wdm file and selected station
                CreateMetConstituent(lMetWDMFileName, lMetGageName, "ATEM", .MetConstituents)
                CreateMetConstituent(lMetWDMFileName, lMetGageName, "PEVT", .MetConstituents)


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

                If lSJDate < 1.0 Or lEJDate < 1 Or lSJDate > lEJDate Then 'failed date checks
                    Logger.Msg("The specified meteorologic stations do not have a common period of record.", vbOKOnly, "BASINS SWMM Problem")
                    EnableControls(True)
                    Exit Sub
                End If
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                'set start and end dates
                .SJDate = lSJDate
                .EJDate = lEJDate

                'add backdrop file
                .BackdropFile = FilenameSetExt(lSWMMProjectFileName, ".bmp")
                GisUtil.SaveMapAsImage(.BackdropFile)
                .BackdropX1 = GisUtil.MapExtentXmin
                .BackdropY1 = GisUtil.MapExtentYmin
                .BackdropX2 = GisUtil.MapExtentXmax
                .BackdropY2 = GisUtil.MapExtentYmax

                'populate the SWMM classes from the shapefiles
                .Nodes.Clear()
                .Conduits.Clear()
                .Catchments.Clear()
                Dim lTable As New atcUtility.atcTableDBF

                If lTable.OpenFile(FilenameSetExt(lNodesShapefileName, "dbf")) Then
                    Logger.Dbg("Add " & lTable.NumRecords & " NodesFrom " & lNodesShapefileName)
                    .Nodes.AddRange(lTable.PopulateObjects((New atcSWMM.Node).GetType, pNodeFieldMap))
                End If
                CompleteNodesFromShapefile(lNodesShapefileName, .Nodes)

                If lTable.OpenFile(FilenameSetExt(lConduitShapefileName, "dbf")) Then
                    Logger.Dbg("Add " & lTable.NumRecords & " ConduitsFrom " & lConduitShapefileName)
                    .Conduits.AddRange(NumberObjects(lTable.PopulateObjects((New atcSWMM.Conduit).GetType, pConduitFieldMap), "Name", "C", 1))
                End If
                CompleteConduitsFromShapefile(lConduitShapefileName, pPlugIn.SWMMProject, .Conduits)

                If lTable.OpenFile(FilenameSetExt(lCatchmentShapefileName, "dbf")) Then
                    Logger.Dbg("Add " & lTable.NumRecords & " CatchmentsFrom " & lCatchmentShapefileName)
                    .Catchments.AddRange(lTable.PopulateObjects((New atcSWMM.Catchment).GetType, pCatchmentFieldMap))
                End If
                CompleteCatchmentsFromShapefile(lCatchmentShapefileName, lPrecGageNamesByCatchment, pPlugIn.SWMMProject, .Catchments)

                lblStatus.Text = "Overlaying Landuses with Catchments"
                Me.Refresh()

                Dim lLanduseLayerName As String = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
                Dim lLanduseLayerIndex As Integer = GisUtil.LayerIndex(lLanduseLayerName)
                Dim lLandUseFileName As String = GisUtil.LayerFileName(lLanduseLayerIndex)
                Dim lSubbasinFieldIndex As Integer = GetFieldIndexFromMap(lCatchmentShapefileName, "Name", pCatchmentFieldMap)
                Dim lSubbasinFieldName As String = GisUtil.FieldName(lSubbasinFieldIndex, GisUtil.LayerIndex(lCatchmentShapefileName))
                If cboLanduse.SelectedIndex = 0 Then
                    'usgs giras is the selected land use type
                    CreateLandusesFromGIRAS(lLandUseFileName, lCatchmentShapefileName, lSubbasinFieldName, .Catchments, .Landuses)
                ElseIf cboLanduse.SelectedIndex = 1 Or cboLanduse.SelectedIndex = 3 Then
                    'create landuses from grid
                    CreateLandusesFromGrid(lLandUseFileName, lCatchmentShapefileName, .Catchments, .Landuses)
                ElseIf cboLanduse.SelectedIndex = 2 Then
                    'other shape
                    Dim lLanduseFieldName As String = ""
                    If cboDescription.SelectedIndex > -1 Then
                        lLanduseFieldName = cboDescription.Items(cboDescription.SelectedIndex)
                    End If
                    CreateLandusesFromShapefile(lLandUseFileName, lLanduseFieldName, lCatchmentShapefileName, lSubbasinFieldName, .Catchments, .Landuses)
                End If

                'now reclassify the landuses 
                Dim lReclassifyLanduses As atcSWMM.Landuses = ReclassifyLandUses(lReclassificationRecords, .Landuses)
                .Landuses = lReclassifyLanduses

                Logger.Dbg(lblStatus.Text)
                Me.Refresh()

                'save project file and start SWMM
                Logger.Dbg("Save SWMM input file" & lSWMMProjectFileName)
                .Save(lSWMMProjectFileName)
                Logger.Dbg("Run SWMM")
                .Run(lSWMMProjectFileName)
                Logger.Dbg("BackFromSWMM")
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

    Private Function PreProcessChecking(ByVal aOutputFileName As String) As Boolean
        Logger.Dbg("PreprocessChecking " & aOutputFileName)
        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            If GisUtil.LayerIndex("Land Use Index") = -1 Then
                'cant do giras without land use index layer
                Logger.Msg("When using GIRAS Landuse, the 'Land Use Index' layer must exist and be named as such.", vbOKOnly, "SWMM GIRAS Problem")
                EnableControls(True)
                Return False
            End If
        End If

        If cboLanduse.SelectedIndex <> 0 Then
            'not giras, make sure subbasins and land use layers aren't the same
            If cboSubbasins.Items(cboSubbasins.SelectedIndex) = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex) Then
                'same layer cannot be used for both
                Logger.Msg("The same layer cannot be used for the catchments layer and the landuse layer.", vbOKOnly, "BASINS SWMM Problem")
                EnableControls(True)
                Return False
            End If
        End If

        'If pMetStations.Count = 0 Then
        '    'cannot proceed if there are no met stations, need to specify a met wdm
        '    Logger.Msg("No met stations are available.  Use the 'Met Stations' tab to specify a WDM file with valid met stations.", vbOKOnly, "BASINS SWMM Problem")
        '    EnableControls(True)
        '    Return False
        'End If

        'see if this file already exists
        If FileExists(aOutputFileName) Then  'already exists
            If Logger.Msg("SWMM Project '" & FilenameNoPath(aOutputFileName) & "' already exists.  Do you want to overwrite it?", vbOKCancel, "Overwrite?") = MsgBoxResult.Cancel Then
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

        pPrecStations = New atcCollection
        pMetStations = New atcCollection

        'set field mapping for nodes, conduits, and catchments
        pNodeFieldMap.Clear()
        pNodeFieldMap.Add("ID", "Name")
        pConduitFieldMap.Clear()
        pConduitFieldMap.Add("InNodeId", "InletNodeName")
        pConduitFieldMap.Add("OutNodeID", "OutletNodeName")
        pConduitFieldMap.Add("SUBBASIN", "Name")
        pConduitFieldMap.Add("SUBBASINR", "DownConduitID")
        pConduitFieldMap.Add("MAXEL", "ElevationHigh")
        pConduitFieldMap.Add("MINEL", "ElevationLow")
        pConduitFieldMap.Add("WID2", "MeanWidth")
        pConduitFieldMap.Add("DEP2", "MeanDepth")
        pCatchmentFieldMap.Clear()
        pCatchmentFieldMap.Add("SUBBASIN", "Name")
        pCatchmentFieldMap.Add("SLO1", "Slope")
        pCatchmentFieldMap.Add("ID", "Name")
        pCatchmentFieldMap.Add("OutNode", "OutletNodeID")

        cboLanduse.Items.Add("USGS GIRAS Shapefile")
        cboLanduse.Items.Add("NLCD Grid")
        cboLanduse.Items.Add("Other Shapefile")
        cboLanduse.Items.Add("User Grid")
        cboLanduse.SelectedIndex = 0

        cboOutlets.Items.Add("<none>")
        cboMet.Items.Add("<none>")

        With AtcGridPrec
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With

        For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 3 Then 'PolygonShapefile 
                cboSubbasins.Items.Add(lLayerName)
                If lLayerName.IndexOf("Catchment") > -1 Or lLayerName.ToUpper = "SUBBASINS" Or lLayerName.IndexOf("Watershed Shapefile") > -1 Then
                    cboSubbasins.SelectedIndex = cboSubbasins.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = 2 Then 'LineShapefile 
                cboStreams.Items.Add(lLayerName)
                If lLayerName.IndexOf("Conduit") > -1 Or lLayerName.ToUpper = "STREAMS" Or lLayerName.IndexOf("Stream Reach Shapefile") > -1 Then
                    cboStreams.SelectedIndex = cboStreams.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = 1 Then 'PointShapefile
                cboOutlets.Items.Add(lLayerName)
                If lLayerName.IndexOf("Node") > -1 Then
                    cboOutlets.SelectedIndex = cboOutlets.Items.Count - 1
                End If
                cboMet.Items.Add(lLayerName)
                If lLayerName.IndexOf("Weather Station Sites 20") > -1 Then
                    'this takes some time, show window and then do this
                    'cboMet.SelectedIndex = cboMet.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = 4 Then 'Grid
                If lLayerName.IndexOf("NLCD") > -1 Then
                    cboLanduse.SelectedIndex = 1
                End If
            End If
        Next
        If cboSubbasins.Items.Count > 0 And cboSubbasins.SelectedIndex < 0 Then
            cboSubbasins.SelectedIndex = 0
        End If
        If cboStreams.Items.Count > 0 And cboStreams.SelectedIndex < 0 Then
            cboStreams.SelectedIndex = 0
        End If
        If cboOutlets.Items.Count > 0 And cboOutlets.SelectedIndex < 0 Then
            cboOutlets.SelectedIndex = 0
        End If
        If cboMet.Items.Count > 0 And cboMet.SelectedIndex < 0 Then
            cboMet.SelectedIndex = 0
        End If

        tbxName.Text = IO.Path.GetFileNameWithoutExtension(GisUtil.ProjectFileName)

        With AtcGridPervious
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With

        SetLanduseTab(cboLanduse.Items(cboLanduse.SelectedIndex))
        pInitializing = False
        SetFieldMappingControl()
        Logger.Dbg("InitializeUI Complete")
    End Sub

    Friend Sub InitializeMetStationList()
        For lLayerIndex As Integer = 0 To cboMet.Items.Count - 1
            Dim lLayerName As String = cboMet.Items(lLayerIndex)
            If lLayerName.IndexOf("Weather Station Sites 20") > -1 Then
                'this takes some time, show window and then do this
                Logger.Dbg("Initializing MetStationList")
                cboMet.SelectedIndex = lLayerIndex
            End If
        Next
    End Sub

    Private Sub cboMet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMet.SelectedIndexChanged
        'fill in met wdm file name as appropriate
        Dim lMetLayerName As String = cboMet.Items(cboMet.SelectedIndex)
        If lMetLayerName.IndexOf("Weather Station Sites 20") > -1 Then 'new basins met
            Dim lMetWDMName As String = GisUtil.LayerFileName(GisUtil.LayerIndex(lMetLayerName))
            lMetWDMName = FilenameSetExt(lMetWDMName, "wdm")
            txtMetWDMName.Text = lMetWDMName
        ElseIf lMetLayerName.IndexOf("WDM Weather") > -1 Then 'old basins met 
            If GisUtil.IsLayer("State Boundaries") Then
                Dim lStateIndex As Integer = GisUtil.LayerIndex("State Boundaries")
                Dim lDefaultState As String = GisUtil.FieldValue(lStateIndex, 0, GisUtil.FieldIndex(lStateIndex, "ST"))
                Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
                Dim lDataPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "data\"
                txtMetWDMName.Text = lDataPath & "met_data\" & lDefaultState & ".wdm"
            End If
        End If
    End Sub

    Private Sub cmdSelectWDM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectWDM.Click
        If ofdMetWDM.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtMetWDMName.Text = ofdMetWDM.FileName
        End If
    End Sub

    Private Sub txtMetWDMName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMetWDMName.TextChanged
        lblStatus.Text = "Reading Precipitation Data from WDM File..."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        EnableControls(False)

        BuildListofValidStationNames(txtMetWDMName.Text, "PREC", pPrecStations)
        SetPrecipStationGrid()
        Logger.Dbg("Processing " & pPrecStations.Count & " PREC stations")
        For Each lPrecStation As StationDetails In pPrecStations
            cboPrecipStation.Items.Add(lPrecStation.Description)
        Next
        If cboPrecipStation.Items.Count > 0 Then
            cboPrecipStation.SelectedIndex = 0
        End If

        lblStatus.Text = "Reading Other Meteorologic Data from WDM File..."
        Me.Refresh()

        BuildListofValidStationNames(txtMetWDMName.Text, "PEVT", pMetStations)
        Logger.Dbg("Processing " & pMetStations.Count & " MET stations")
        For Each lMetStation As StationDetails In pMetStations
            cboOtherMet.Items.Add(lMetStation.Description)
        Next
        If cboOtherMet.Items.Count > 0 Then
            cboOtherMet.SelectedIndex = 0
        End If

        lblStatus.Text = "Update specifications if desired, then click OK to proceed."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        EnableControls(True)
    End Sub

    Private Sub SetPrecipStationGrid()
        If AtcGridPrec.Source Is Nothing OrElse cboSubbasins.SelectedIndex = -1 Then
            Logger.Dbg("No atcGridPrec or Subbasin selected")
        Else
            Logger.Dbg("Begin")
            Dim lCatchmentLayerIndex As Integer = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
            Dim lCatchmentShapefileName As String = GisUtil.LayerFileName(lCatchmentLayerIndex)

            Dim lTempCatchments As New atcSWMM.Catchments
            Dim lTable As New atcUtility.atcTableDBF
            If lTable.OpenFile(FilenameSetExt(lCatchmentShapefileName, "dbf")) Then
                lTempCatchments.AddRange(lTable.PopulateObjects((New atcSWMM.Catchment).GetType, pCatchmentFieldMap))
            End If
            Logger.Dbg("CatchmentsCount " & lTempCatchments.Count)

            AtcGridPrec.Clear()
            With AtcGridPrec.Source
                .Columns = 2
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                .CellColor(0, 0) = SystemColors.ControlDark
                .CellColor(0, 1) = SystemColors.ControlDark
                .Rows = 1 + lTempCatchments.Count
                .CellValue(0, 0) = "Catchment"
                .CellValue(0, 1) = "Precip Station"
                For lIndex As Integer = 1 To lTempCatchments.Count
                    If IsNumeric(lTempCatchments(lIndex - 1).Name) Then
                        .CellValue(lIndex, 0) = CInt(lTempCatchments(lIndex - 1).Name)
                    Else
                        .CellValue(lIndex, 0) = lTempCatchments(lIndex - 1).Name
                    End If
                    .CellColor(lIndex, 0) = SystemColors.ControlDark
                    If pPrecStations.Count > 0 Then
                        .CellValue(lIndex, 1) = pPrecStations(0).Description
                        .CellEditable(lIndex, 1) = True
                    End If
                Next
            End With

            Logger.Dbg("SetValidValues")
            Dim lValidValues As New atcCollection
            For Each lPrecStation As StationDetails In pPrecStations
                lValidValues.Add(lPrecStation.Description)
            Next
            AtcGridPrec.ValidValues = lValidValues
            AtcGridPrec.SizeAllColumnsToContents()
            AtcGridPrec.Refresh()
            Logger.Dbg("PrecipStationGrid refreshed")
        End If
    End Sub

    Private Sub rbnSingle_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbnSingle.CheckedChanged
        If rbnSingle.Checked Then
            AtcGridPrec.Visible = False
            cboPrecipStation.Visible = True
            lblPrecipStation.Visible = True
        Else
            AtcGridPrec.Visible = True
            cboPrecipStation.Visible = False
            lblPrecipStation.Visible = False
        End If
    End Sub

    Private Function GetFieldIndexFromMap(ByVal aLayerName As String, ByVal aMapFieldName As String, ByVal aFieldMap As atcCollection) As Integer
        'given a layer ("subbasins") and a field map name ("Name"), return index of field mapped to that name
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aLayerName)
        GetFieldIndexFromMap = -1

        Dim lFieldName As String
        For lIndex As Integer = 0 To aFieldMap.Count - 1
            If aFieldMap(lIndex) = aMapFieldName Then
                lFieldName = aFieldMap.Keys(lIndex)
                If GisUtil.IsField(lLayerIndex, lFieldName) Then
                    GetFieldIndexFromMap = GisUtil.FieldIndex(lLayerIndex, lFieldName)
                    Exit For
                End If
            End If
        Next

    End Function

    Private Sub lblStatus_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStatus.TextChanged
        Logger.Dbg(lblStatus.Text)
    End Sub

    Private Sub cmdChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChange.Click
        If ofdClass.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblClass.Text = ofdClass.FileName
            SetPerviousGrid()
        End If
    End Sub

    Private Sub cboPrecipStation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPrecipStation.SelectedIndexChanged
        SetDates()
    End Sub

    Private Sub cboOtherMet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboOtherMet.SelectedIndexChanged
        SetDates()
    End Sub

    Private Sub SetDates()
        'set dates on the general tab to the last common year of the selected met data        

        Dim lSJDate As Double = 0.0
        Dim lEJDate As Double = 0.0
        Dim lPrecGageNamesByCatchment As New Collection
        Dim lSelectedStation As StationDetails

        'set dates from prec dsn
        If rbnSingle.Checked Then
            If cboPrecipStation.SelectedIndex > -1 Then
                lSelectedStation = pPrecStations.ItemByKey(cboPrecipStation.Items(cboPrecipStation.SelectedIndex))
                'set dates
                If lSelectedStation.StartJDate > lSJDate Then
                    lSJDate = lSelectedStation.StartJDate
                End If
                If lEJDate = 0.0 Or lSelectedStation.EndJDate < lEJDate Then
                    lEJDate = lSelectedStation.EndJDate
                End If
                'use this precip gage for each catchment
                lPrecGageNamesByCatchment.Add(lSelectedStation.Name)
            End If
        Else
            For lrow As Integer = 1 To AtcGridPrec.Source.Rows - 1
                lSelectedStation = pPrecStations.ItemByKey(AtcGridPrec.Source.CellValue(lrow, 1))
                'set dates
                If lSelectedStation.StartJDate > lSJDate Then
                    lSJDate = lSelectedStation.StartJDate
                End If
                If lEJDate = 0.0 Or lSelectedStation.EndJDate < lEJDate Then
                    lEJDate = lSelectedStation.EndJDate
                End If
                'remember which precip gage goes with each catchment
                lPrecGageNamesByCatchment.Add(lSelectedStation.Name)
            Next
        End If

        'change dates to reflect met dsns if different
        If cboOtherMet.SelectedIndex > -1 Then
            lSelectedStation = pMetStations.ItemByKey(cboOtherMet.Items(cboOtherMet.SelectedIndex))
            'set dates
            If lSelectedStation.StartJDate > lSJDate Then
                lSJDate = lSelectedStation.StartJDate
            End If
            If lEJDate = 0.0 Or lSelectedStation.EndJDate < lEJDate Then
                lEJDate = lSelectedStation.EndJDate
            End If
        End If

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

    Private Sub AtcGridPrec_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridPrec.CellEdited
        SetDates()
    End Sub
End Class