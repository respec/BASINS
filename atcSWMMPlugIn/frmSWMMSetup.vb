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
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cboStream8 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream7 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream6 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream5 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream4 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream3 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream2 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents cboSub3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cboSub2 As System.Windows.Forms.ComboBox
    Friend WithEvents cboSub1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents AtcGridMet As atcControls.atcGrid
    Friend WithEvents lstMet As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtMetWDMName As System.Windows.Forms.TextBox
    Friend WithEvents cmdSelectWDM As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cboOutlets As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents cboPoint As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
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
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.cboStream8 = New System.Windows.Forms.ComboBox
        Me.cboStream7 = New System.Windows.Forms.ComboBox
        Me.cboStream6 = New System.Windows.Forms.ComboBox
        Me.cboStream5 = New System.Windows.Forms.ComboBox
        Me.cboStream4 = New System.Windows.Forms.ComboBox
        Me.cboStream3 = New System.Windows.Forms.ComboBox
        Me.cboStream2 = New System.Windows.Forms.ComboBox
        Me.cboStream1 = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.cboSub3 = New System.Windows.Forms.ComboBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.cboSub2 = New System.Windows.Forms.ComboBox
        Me.cboSub1 = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.cboPoint = New System.Windows.Forms.ComboBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.Label22 = New System.Windows.Forms.Label
        Me.AtcGridMet = New atcControls.atcGrid
        Me.lstMet = New System.Windows.Forms.ListBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtMetWDMName = New System.Windows.Forms.TextBox
        Me.cmdSelectWDM = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(16, 465)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 32)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "OK"
        '
        'cmdExisting
        '
        Me.cmdExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdExisting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExisting.Location = New System.Drawing.Point(96, 465)
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
        Me.cmdCancel.Location = New System.Drawing.Point(224, 465)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(88, 32)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Cancel"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(370, 465)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(79, 32)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(458, 465)
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
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(60, 21)
        Me.TabControl1.Location = New System.Drawing.Point(18, 17)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(525, 369)
        Me.TabControl1.TabIndex = 8
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
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
        Me.TabPage1.Size = New System.Drawing.Size(517, 340)
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
        Me.cboOutlets.Size = New System.Drawing.Size(339, 25)
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
        Me.cboMet.Size = New System.Drawing.Size(337, 25)
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
        Me.cboStreams.Size = New System.Drawing.Size(337, 25)
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
        Me.cboSubbasins.Size = New System.Drawing.Size(337, 25)
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
        Me.cboLanduse.Size = New System.Drawing.Size(337, 25)
        Me.cboLanduse.TabIndex = 7
        '
        'tbxName
        '
        Me.tbxName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbxName.Location = New System.Drawing.Point(168, 40)
        Me.tbxName.Name = "tbxName"
        Me.tbxName.Size = New System.Drawing.Size(171, 23)
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
        Me.TabPage2.Size = New System.Drawing.Size(517, 340)
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
        Me.AtcGridPervious.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridPervious.LineColor = System.Drawing.Color.Empty
        Me.AtcGridPervious.LineWidth = 0.0!
        Me.AtcGridPervious.Location = New System.Drawing.Point(14, 148)
        Me.AtcGridPervious.Name = "AtcGridPervious"
        Me.AtcGridPervious.Size = New System.Drawing.Size(488, 172)
        Me.AtcGridPervious.Source = Nothing
        Me.AtcGridPervious.TabIndex = 18
        '
        'cmdChange
        '
        Me.cmdChange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChange.Location = New System.Drawing.Point(430, 117)
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
        Me.lblClass.Size = New System.Drawing.Size(258, 20)
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
        Me.cboLandUseLayer.Size = New System.Drawing.Size(337, 25)
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
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.Label17)
        Me.TabPage4.Controls.Add(Me.Label16)
        Me.TabPage4.Controls.Add(Me.Label15)
        Me.TabPage4.Controls.Add(Me.Label14)
        Me.TabPage4.Controls.Add(Me.Label13)
        Me.TabPage4.Controls.Add(Me.cboStream8)
        Me.TabPage4.Controls.Add(Me.cboStream7)
        Me.TabPage4.Controls.Add(Me.cboStream6)
        Me.TabPage4.Controls.Add(Me.cboStream5)
        Me.TabPage4.Controls.Add(Me.cboStream4)
        Me.TabPage4.Controls.Add(Me.cboStream3)
        Me.TabPage4.Controls.Add(Me.cboStream2)
        Me.TabPage4.Controls.Add(Me.cboStream1)
        Me.TabPage4.Controls.Add(Me.Label10)
        Me.TabPage4.Controls.Add(Me.Label11)
        Me.TabPage4.Controls.Add(Me.Label12)
        Me.TabPage4.Location = New System.Drawing.Point(4, 25)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(517, 340)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Conduits"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(71, 259)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(168, 23)
        Me.Label17.TabIndex = 22
        Me.Label17.Text = "Max Elev Field (meters):"
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(71, 227)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(168, 23)
        Me.Label16.TabIndex = 21
        Me.Label16.Text = "Min Elev Field (meters):"
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(71, 195)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(194, 23)
        Me.Label15.TabIndex = 20
        Me.Label15.Text = "Mean Depth Field (meters):"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(71, 162)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(178, 23)
        Me.Label14.TabIndex = 19
        Me.Label14.Text = "Mean Width Field (meters):"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(71, 131)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(152, 23)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "Outlet Node ID Field:"
        '
        'cboStream8
        '
        Me.cboStream8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream8.Location = New System.Drawing.Point(271, 256)
        Me.cboStream8.Name = "cboStream8"
        Me.cboStream8.Size = New System.Drawing.Size(166, 25)
        Me.cboStream8.TabIndex = 16
        '
        'cboStream7
        '
        Me.cboStream7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream7.Location = New System.Drawing.Point(271, 224)
        Me.cboStream7.Name = "cboStream7"
        Me.cboStream7.Size = New System.Drawing.Size(166, 25)
        Me.cboStream7.TabIndex = 15
        '
        'cboStream6
        '
        Me.cboStream6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream6.Location = New System.Drawing.Point(271, 192)
        Me.cboStream6.Name = "cboStream6"
        Me.cboStream6.Size = New System.Drawing.Size(166, 25)
        Me.cboStream6.TabIndex = 14
        '
        'cboStream5
        '
        Me.cboStream5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream5.Location = New System.Drawing.Point(271, 160)
        Me.cboStream5.Name = "cboStream5"
        Me.cboStream5.Size = New System.Drawing.Size(166, 25)
        Me.cboStream5.TabIndex = 13
        '
        'cboStream4
        '
        Me.cboStream4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream4.Location = New System.Drawing.Point(271, 128)
        Me.cboStream4.Name = "cboStream4"
        Me.cboStream4.Size = New System.Drawing.Size(166, 25)
        Me.cboStream4.TabIndex = 12
        '
        'cboStream3
        '
        Me.cboStream3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream3.Location = New System.Drawing.Point(271, 96)
        Me.cboStream3.Name = "cboStream3"
        Me.cboStream3.Size = New System.Drawing.Size(166, 25)
        Me.cboStream3.TabIndex = 11
        '
        'cboStream2
        '
        Me.cboStream2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream2.Location = New System.Drawing.Point(271, 63)
        Me.cboStream2.Name = "cboStream2"
        Me.cboStream2.Size = New System.Drawing.Size(166, 25)
        Me.cboStream2.TabIndex = 10
        '
        'cboStream1
        '
        Me.cboStream1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream1.Location = New System.Drawing.Point(271, 32)
        Me.cboStream1.Name = "cboStream1"
        Me.cboStream1.Size = New System.Drawing.Size(166, 25)
        Me.cboStream1.TabIndex = 9
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(71, 99)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(152, 23)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Inlet Node ID Field:"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(71, 67)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(194, 23)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "Downstream Conduit ID Field:"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(71, 35)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(152, 23)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "Name/ID Field:"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.cboSub3)
        Me.TabPage3.Controls.Add(Me.Label21)
        Me.TabPage3.Controls.Add(Me.cboSub2)
        Me.TabPage3.Controls.Add(Me.cboSub1)
        Me.TabPage3.Controls.Add(Me.Label8)
        Me.TabPage3.Controls.Add(Me.Label7)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(517, 340)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Catchments"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'cboSub3
        '
        Me.cboSub3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub3.Location = New System.Drawing.Point(248, 93)
        Me.cboSub3.Name = "cboSub3"
        Me.cboSub3.Size = New System.Drawing.Size(166, 25)
        Me.cboSub3.TabIndex = 6
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(72, 96)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(167, 23)
        Me.Label21.TabIndex = 5
        Me.Label21.Text = "Outlet Node ID Field:"
        '
        'cboSub2
        '
        Me.cboSub2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub2.Location = New System.Drawing.Point(248, 63)
        Me.cboSub2.Name = "cboSub2"
        Me.cboSub2.Size = New System.Drawing.Size(166, 25)
        Me.cboSub2.TabIndex = 4
        '
        'cboSub1
        '
        Me.cboSub1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub1.Location = New System.Drawing.Point(248, 32)
        Me.cboSub1.Name = "cboSub1"
        Me.cboSub1.Size = New System.Drawing.Size(166, 25)
        Me.cboSub1.TabIndex = 3
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(72, 67)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(152, 23)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Slope Field (percent):"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(72, 36)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(152, 23)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Name/ID Field:"
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.cboPoint)
        Me.TabPage5.Controls.Add(Me.Label19)
        Me.TabPage5.Location = New System.Drawing.Point(4, 25)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(517, 340)
        Me.TabPage5.TabIndex = 6
        Me.TabPage5.Text = "Nodes"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'cboPoint
        '
        Me.cboPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPoint.Location = New System.Drawing.Point(247, 35)
        Me.cboPoint.Name = "cboPoint"
        Me.cboPoint.Size = New System.Drawing.Size(167, 25)
        Me.cboPoint.TabIndex = 15
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(71, 39)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(100, 17)
        Me.Label19.TabIndex = 14
        Me.Label19.Text = "Name/ID Field:"
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.Label22)
        Me.TabPage6.Controls.Add(Me.AtcGridMet)
        Me.TabPage6.Controls.Add(Me.lstMet)
        Me.TabPage6.Controls.Add(Me.GroupBox2)
        Me.TabPage6.Location = New System.Drawing.Point(4, 25)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(517, 340)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Met Stations"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'Label22
        '
        Me.Label22.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(368, 313)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(125, 17)
        Me.Label22.TabIndex = 20
        Me.Label22.Text = "* Full Set Available"
        '
        'AtcGridMet
        '
        Me.AtcGridMet.AllowHorizontalScrolling = True
        Me.AtcGridMet.AllowNewValidValues = False
        Me.AtcGridMet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridMet.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridMet.LineColor = System.Drawing.Color.Empty
        Me.AtcGridMet.LineWidth = 0.0!
        Me.AtcGridMet.Location = New System.Drawing.Point(21, 97)
        Me.AtcGridMet.Name = "AtcGridMet"
        Me.AtcGridMet.Size = New System.Drawing.Size(472, 206)
        Me.AtcGridMet.Source = Nothing
        Me.AtcGridMet.TabIndex = 19
        '
        'lstMet
        '
        Me.lstMet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstMet.FormattingEnabled = True
        Me.lstMet.ItemHeight = 17
        Me.lstMet.Location = New System.Drawing.Point(21, 97)
        Me.lstMet.Name = "lstMet"
        Me.lstMet.Size = New System.Drawing.Size(472, 208)
        Me.lstMet.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.txtMetWDMName)
        Me.GroupBox2.Controls.Add(Me.cmdSelectWDM)
        Me.GroupBox2.Location = New System.Drawing.Point(21, 20)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(473, 59)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Met WDM File"
        '
        'txtMetWDMName
        '
        Me.txtMetWDMName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMetWDMName.Location = New System.Drawing.Point(21, 23)
        Me.txtMetWDMName.Name = "txtMetWDMName"
        Me.txtMetWDMName.ReadOnly = True
        Me.txtMetWDMName.Size = New System.Drawing.Size(345, 23)
        Me.txtMetWDMName.TabIndex = 2
        '
        'cmdSelectWDM
        '
        Me.cmdSelectWDM.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSelectWDM.Location = New System.Drawing.Point(382, 21)
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
        Me.GroupBox1.Location = New System.Drawing.Point(18, 393)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(527, 55)
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
        Me.lblStatus.Size = New System.Drawing.Size(496, 16)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Update specifications if desired, then click OK to proceed."
        '
        'frmSWMMSetup
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(562, 510)
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
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private pPlugIn As PlugIn
    Private pNodeFieldMap As New atcUtility.atcCollection
    Private pConduitFieldMap As New atcUtility.atcCollection
    Private pCatchmentFieldMap As New atcUtility.atcCollection

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            cboLandUseLayer.Visible = False
            lblLandUseLayer.Visible = False
            cboDescription.Visible = False
            lblDescription.Visible = False
            lblClass.Text = "/BASINS/etc/giras.dbf"
            'SetPerviousGrid()
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
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
            'SetPerviousGrid()
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
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
            lblClass.Text = "/BASINS/etc/nlcd.dbf"
            'SetPerviousGrid()
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
            'SetPerviousGrid()
        End If
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("BASINS SWMM for MapWindow" & vbCrLf & vbCrLf & "Version 1.0", MsgBoxStyle.OkOnly, "BASINS SWMM")
    End Sub

    Private Sub cmdExisting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExisting.Click
        If ofdExisting.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pPlugIn.SWMMProject.Run(ofdExisting.FileName)
        End If
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\SWMM.html")
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

        'hard code some things to test SWMM classes
        Dim lStartYear As Integer = 2006
        Dim lStartMonth As Integer = 10
        Dim lStartDay As Integer = 24
        Dim lEndYear As Integer = 2006
        Dim lEndMonth As Integer = 10
        Dim lEndDay As Integer = 31
        Dim lMetShapefileName As String = "C:\basins\data\02060006\met\met.shp"
        Dim lPrecGageName As String = "MD189070"  'could be multiple?
        Dim lMetWDMFileName As String = "C:\basins\data\02060006\met\met.wdm"
        Dim lMetGageName As String = "MD189070"
        Dim lLandUseGridName As String = "C:\BASINS\data\02060006\NLCD\NLCD_LandCover_2001.tif"

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

        'Dim lNodesShapefileName As String = "C:\basins\Predefined Delineations\West Branch\nodes.shp"
        'Dim lConduitShapefileName As String = "C:\basins\Predefined Delineations\West Branch\conduits.shp"
        'Dim lCatchmentShapefileName As String = "C:\basins\Predefined Delineations\West Branch\catchments.shp"

        lblStatus.Text = "Preparing to process"
        Me.Refresh()
        EnableControls(False)

        With pPlugIn.SWMMProject

            .Name = tbxName.Text
            Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
            'TODO: still use modelout?
            Dim lSWMMProjectFileName As String = lBasinsFolder & "\modelout\" & .Name & "\" & .Name & ".inp"

            If Not PreProcessChecking(lSWMMProjectFileName) Then 'failed early checks
                Exit Sub
            End If
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            .Title = "SWMM Project Written from BASINS"

            'add backdrop file
            .BackdropFile = FilenameSetExt(lSWMMProjectFileName, ".bmp")
            GisUtil.SaveMapAsImage(.BackdropFile)
            .BackdropX1 = GisUtil.MapExtentXmin
            .BackdropY1 = GisUtil.MapExtentYmin
            .BackdropX2 = GisUtil.MapExtentXmax
            .BackdropY2 = GisUtil.MapExtentYmax

            'set start and end dates
            .SJDate = MJD(lStartYear, lStartMonth, lStartDay)
            .EJDate = MJD(lEndYear, lEndMonth, lEndDay)

            'create rain gages from shapefile and selected station
            CreateRaingageFromShapefile(lMetShapefileName, lPrecGageName, .RainGages)

            'create met constituents from wdm file and selected station
            CreateMetConstituent(lMetWDMFileName, lMetGageName, "ATEM", .MetConstituents)
            CreateMetConstituent(lMetWDMFileName, lMetGageName, "PEVT", .MetConstituents)

            'populate the SWMM classes from the shapefiles
            .Nodes.Clear()
            .Conduits.Clear()
            .Catchments.Clear()
            Dim lTable As New atcUtility.atcTableDBF

            If lTable.OpenFile(FilenameSetExt(lNodesShapefileName, "dbf")) Then
                .Nodes.AddRange(lTable.PopulateObjects((New atcSWMM.Node).GetType, pNodeFieldMap))
            End If
            CompleteNodesFromShapefile(lNodesShapefileName, .Nodes)

            If lTable.OpenFile(FilenameSetExt(lConduitShapefileName, "dbf")) Then
                .Conduits.AddRange(NumberObjects(lTable.PopulateObjects((New atcSWMM.Conduit).GetType, pConduitFieldMap), "Name", "C", 1))
            End If
            CompleteConduitsFromShapefile(lConduitShapefileName, pPlugIn.SWMMProject, .Conduits)

            If lTable.OpenFile(FilenameSetExt(lCatchmentShapefileName, "dbf")) Then
                .Catchments.AddRange(lTable.PopulateObjects((New atcSWMM.Catchment).GetType, pCatchmentFieldMap))
            End If
            CompleteCatchmentsFromShapefile(lCatchmentShapefileName, pPlugIn.SWMMProject, .Catchments)

            'create landuses from nlcd landcover
            CreateLandusesFromGrid(lLandUseGridName, lCatchmentShapefileName, .Catchments, .Landuses)

            lblStatus.Text = "Writing SWMM INP file"
            Me.Refresh()

            'save project file and start SWMM
            .Save(lSWMMProjectFileName)
            .Run(lSWMMProjectFileName)
        End With

        lblStatus.Text = ""
        Me.Refresh()
        Me.Dispose()
        Me.Close()
    End Sub

    Private Sub EnableControls(ByVal aEnabled As Boolean)
        cmdOK.Enabled = aEnabled
        cmdExisting.Enabled = aEnabled
        cmdHelp.Enabled = aEnabled
        cmdCancel.Enabled = aEnabled
        cmdAbout.Enabled = aEnabled
    End Sub

    Private Function PreProcessChecking(ByVal aOutputFileName As String) As Boolean

        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            If GisUtil.LayerIndex("Land Use Index") = -1 Then
                'cant do giras without land use index layer
                Logger.Msg("When using GIRAS Landuse, the 'Land Use Index' layer must exist and be named as such.", vbOKOnly, "HSPF GIRAS Problem")
                EnableControls(True)
                Return False
            End If
        End If

        If cboLanduse.SelectedIndex <> 0 Then
            'not giras, make sure subbasins and land use layers aren't the same
            If cboSubbasins.Items(cboSubbasins.SelectedIndex) = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex) Then
                'same layer cannot be used for both
                Logger.Msg("The same layer cannot be used for the catchments layer and the landuse layer.", vbOKOnly, "BASINS HSPF Problem")
                EnableControls(True)
                Return False
            End If
        End If

        'If pMetStations.Count = 0 Then
        '    'cannot proceed if there are no met stations, need to specify a met wdm
        '    Logger.Msg("No met stations are available.  Use the 'Met Stations' tab to specify a WDM file with valid met stations.", vbOKOnly, "BASINS HSPF Problem")
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

        Return True
    End Function

    Public Sub InitializeUI(ByVal aPlugIn As PlugIn)
        pPlugIn = aPlugIn

        'pMetStations = New atcCollection
        'pMetBaseDsns = New atcCollection

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

        With AtcGridMet
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

    End Sub

    Private Sub frmSWMMSetup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\SWMM.html")
        End If
    End Sub

End Class