Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Drawing
Imports atcData
Imports atcSegmentation
Imports System.Collections.ObjectModel

Public Class frmModelSetup
    Inherits System.Windows.Forms.Form

    Public pModelName As String
    Friend WithEvents lstMet As System.Windows.Forms.ListBox
    Public pMetStations As atcCollection
    Public pMetBaseDsns As atcCollection

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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbxName As System.Windows.Forms.TextBox
    Friend WithEvents cboLanduse As System.Windows.Forms.ComboBox
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
    Friend WithEvents cboStreams As System.Windows.Forms.ComboBox
    Friend WithEvents cboOutlets As System.Windows.Forms.ComboBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cboYear As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cboSub1 As System.Windows.Forms.ComboBox
    Friend WithEvents cboSub2 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream3 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream2 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cboStream4 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream5 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream6 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream7 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream8 As System.Windows.Forms.ComboBox
    Friend WithEvents cboStream9 As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents cboPoint As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents chkCalculate As System.Windows.Forms.CheckBox
    Friend WithEvents chkCustom As System.Windows.Forms.CheckBox
    Friend WithEvents cboLandUseLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cboDescription As System.Windows.Forms.ComboBox
    Friend WithEvents lblLandUseLayer As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdExisting As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdAbout As System.Windows.Forms.Button
    Friend WithEvents ofdExisting As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ofdCustom As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblCustom As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents lblClass As System.Windows.Forms.Label
    Friend WithEvents cmdChange As System.Windows.Forms.Button
    Friend WithEvents ofdClass As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cboMet As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cboSub3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdSelectWDM As System.Windows.Forms.Button
    Friend WithEvents txtMetWDMName As System.Windows.Forms.TextBox
    Friend WithEvents ofdMetWDM As System.Windows.Forms.OpenFileDialog
    Friend WithEvents AtcGridPervious As atcControls.atcGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModelSetup))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.cboMet = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cboOutlets = New System.Windows.Forms.ComboBox
        Me.cboStreams = New System.Windows.Forms.ComboBox
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.tbxName = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
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
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.cboStream9 = New System.Windows.Forms.ComboBox
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
        Me.lblCustom = New System.Windows.Forms.Label
        Me.chkCustom = New System.Windows.Forms.CheckBox
        Me.chkCalculate = New System.Windows.Forms.CheckBox
        Me.cboPoint = New System.Windows.Forms.ComboBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.cboYear = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtMetWDMName = New System.Windows.Forms.TextBox
        Me.cmdSelectWDM = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdExisting = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.ofdExisting = New System.Windows.Forms.OpenFileDialog
        Me.ofdCustom = New System.Windows.Forms.OpenFileDialog
        Me.ofdClass = New System.Windows.Forms.OpenFileDialog
        Me.ofdMetWDM = New System.Windows.Forms.OpenFileDialog
        Me.lstMet = New System.Windows.Forms.ListBox
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
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(60, 21)
        Me.TabControl1.Location = New System.Drawing.Point(16, 16)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(527, 384)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.cboMet)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.cboOutlets)
        Me.TabPage1.Controls.Add(Me.cboStreams)
        Me.TabPage1.Controls.Add(Me.cboSubbasins)
        Me.TabPage1.Controls.Add(Me.cboLanduse)
        Me.TabPage1.Controls.Add(Me.tbxName)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(519, 355)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "General"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'cboMet
        '
        Me.cboMet.AllowDrop = True
        Me.cboMet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMet.Location = New System.Drawing.Point(168, 277)
        Me.cboMet.Name = "cboMet"
        Me.cboMet.Size = New System.Drawing.Size(339, 24)
        Me.cboMet.TabIndex = 12
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(11, 280)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(130, 17)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "Met Stations Layer:"
        '
        'cboOutlets
        '
        Me.cboOutlets.AllowDrop = True
        Me.cboOutlets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboOutlets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOutlets.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOutlets.Location = New System.Drawing.Point(168, 231)
        Me.cboOutlets.Name = "cboOutlets"
        Me.cboOutlets.Size = New System.Drawing.Size(339, 24)
        Me.cboOutlets.TabIndex = 10
        '
        'cboStreams
        '
        Me.cboStreams.AllowDrop = True
        Me.cboStreams.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStreams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStreams.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStreams.Location = New System.Drawing.Point(168, 183)
        Me.cboStreams.Name = "cboStreams"
        Me.cboStreams.Size = New System.Drawing.Size(339, 24)
        Me.cboStreams.TabIndex = 9
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubbasins.Location = New System.Drawing.Point(168, 136)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(339, 24)
        Me.cboSubbasins.TabIndex = 8
        '
        'cboLanduse
        '
        Me.cboLanduse.AllowDrop = True
        Me.cboLanduse.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLanduse.Location = New System.Drawing.Point(168, 88)
        Me.cboLanduse.Name = "cboLanduse"
        Me.cboLanduse.Size = New System.Drawing.Size(339, 24)
        Me.cboLanduse.TabIndex = 7
        '
        'tbxName
        '
        Me.tbxName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbxName.Location = New System.Drawing.Point(168, 40)
        Me.tbxName.Name = "tbxName"
        Me.tbxName.Size = New System.Drawing.Size(173, 22)
        Me.tbxName.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(11, 234)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(140, 17)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Point Sources Layer:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(11, 187)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(104, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Streams Layer:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(11, 140)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Subbasins Layer:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(11, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Land Use Type:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(137, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "HSPF Project Name:"
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
        Me.TabPage2.Size = New System.Drawing.Size(519, 355)
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
        Me.AtcGridPervious.LineColor = System.Drawing.Color.Empty
        Me.AtcGridPervious.LineWidth = 0.0!
        Me.AtcGridPervious.Location = New System.Drawing.Point(14, 148)
        Me.AtcGridPervious.Name = "AtcGridPervious"
        Me.AtcGridPervious.Size = New System.Drawing.Size(490, 187)
        Me.AtcGridPervious.Source = Nothing
        Me.AtcGridPervious.TabIndex = 18
        '
        'cmdChange
        '
        Me.cmdChange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChange.Location = New System.Drawing.Point(432, 117)
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
        Me.lblClass.Size = New System.Drawing.Size(260, 20)
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
        Me.cboDescription.Size = New System.Drawing.Size(168, 24)
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
        Me.cboLandUseLayer.Size = New System.Drawing.Size(339, 24)
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
        Me.TabPage4.Controls.Add(Me.Label18)
        Me.TabPage4.Controls.Add(Me.Label17)
        Me.TabPage4.Controls.Add(Me.Label16)
        Me.TabPage4.Controls.Add(Me.Label15)
        Me.TabPage4.Controls.Add(Me.Label14)
        Me.TabPage4.Controls.Add(Me.Label13)
        Me.TabPage4.Controls.Add(Me.cboStream9)
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
        Me.TabPage4.Size = New System.Drawing.Size(519, 355)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Streams"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(72, 292)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(152, 23)
        Me.Label18.TabIndex = 23
        Me.Label18.Text = "Stream Name Field:"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(72, 260)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(168, 23)
        Me.Label17.TabIndex = 22
        Me.Label17.Text = "Max Elev Field (meters):"
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(72, 227)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(168, 23)
        Me.Label16.TabIndex = 21
        Me.Label16.Text = "Min Elev Field (meters):"
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(72, 195)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(152, 23)
        Me.Label15.TabIndex = 20
        Me.Label15.Text = "Depth Field (meters):"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(72, 164)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(152, 23)
        Me.Label14.TabIndex = 19
        Me.Label14.Text = "Width Field (meters):"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(72, 132)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(152, 23)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "Slope Field (percent):"
        '
        'cboStream9
        '
        Me.cboStream9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream9.Location = New System.Drawing.Point(248, 288)
        Me.cboStream9.Name = "cboStream9"
        Me.cboStream9.Size = New System.Drawing.Size(168, 24)
        Me.cboStream9.TabIndex = 17
        '
        'cboStream8
        '
        Me.cboStream8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream8.Location = New System.Drawing.Point(248, 256)
        Me.cboStream8.Name = "cboStream8"
        Me.cboStream8.Size = New System.Drawing.Size(168, 24)
        Me.cboStream8.TabIndex = 16
        '
        'cboStream7
        '
        Me.cboStream7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream7.Location = New System.Drawing.Point(248, 224)
        Me.cboStream7.Name = "cboStream7"
        Me.cboStream7.Size = New System.Drawing.Size(168, 24)
        Me.cboStream7.TabIndex = 15
        '
        'cboStream6
        '
        Me.cboStream6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream6.Location = New System.Drawing.Point(248, 192)
        Me.cboStream6.Name = "cboStream6"
        Me.cboStream6.Size = New System.Drawing.Size(168, 24)
        Me.cboStream6.TabIndex = 14
        '
        'cboStream5
        '
        Me.cboStream5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream5.Location = New System.Drawing.Point(248, 160)
        Me.cboStream5.Name = "cboStream5"
        Me.cboStream5.Size = New System.Drawing.Size(168, 24)
        Me.cboStream5.TabIndex = 13
        '
        'cboStream4
        '
        Me.cboStream4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream4.Location = New System.Drawing.Point(248, 128)
        Me.cboStream4.Name = "cboStream4"
        Me.cboStream4.Size = New System.Drawing.Size(168, 24)
        Me.cboStream4.TabIndex = 12
        '
        'cboStream3
        '
        Me.cboStream3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream3.Location = New System.Drawing.Point(248, 96)
        Me.cboStream3.Name = "cboStream3"
        Me.cboStream3.Size = New System.Drawing.Size(168, 24)
        Me.cboStream3.TabIndex = 11
        '
        'cboStream2
        '
        Me.cboStream2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream2.Location = New System.Drawing.Point(248, 63)
        Me.cboStream2.Name = "cboStream2"
        Me.cboStream2.Size = New System.Drawing.Size(168, 24)
        Me.cboStream2.TabIndex = 10
        '
        'cboStream1
        '
        Me.cboStream1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream1.Location = New System.Drawing.Point(248, 32)
        Me.cboStream1.Name = "cboStream1"
        Me.cboStream1.Size = New System.Drawing.Size(168, 24)
        Me.cboStream1.TabIndex = 9
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(72, 99)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(152, 23)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Length Field (meters):"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(72, 67)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(152, 23)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "Downstream ID Field:"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(72, 36)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(152, 23)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "Subbasin ID Field:"
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
        Me.TabPage3.Size = New System.Drawing.Size(519, 355)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Subbasins"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'cboSub3
        '
        Me.cboSub3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub3.Location = New System.Drawing.Point(248, 93)
        Me.cboSub3.Name = "cboSub3"
        Me.cboSub3.Size = New System.Drawing.Size(168, 24)
        Me.cboSub3.TabIndex = 6
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(72, 96)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(167, 23)
        Me.Label21.TabIndex = 5
        Me.Label21.Text = "Model Segment ID Field:"
        '
        'cboSub2
        '
        Me.cboSub2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub2.Location = New System.Drawing.Point(248, 63)
        Me.cboSub2.Name = "cboSub2"
        Me.cboSub2.Size = New System.Drawing.Size(168, 24)
        Me.cboSub2.TabIndex = 4
        '
        'cboSub1
        '
        Me.cboSub1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub1.Location = New System.Drawing.Point(248, 32)
        Me.cboSub1.Name = "cboSub1"
        Me.cboSub1.Size = New System.Drawing.Size(168, 24)
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
        Me.Label7.Text = "Subbasin ID Field:"
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.lblCustom)
        Me.TabPage5.Controls.Add(Me.chkCustom)
        Me.TabPage5.Controls.Add(Me.chkCalculate)
        Me.TabPage5.Controls.Add(Me.cboPoint)
        Me.TabPage5.Controls.Add(Me.Label19)
        Me.TabPage5.Controls.Add(Me.cboYear)
        Me.TabPage5.Controls.Add(Me.Label6)
        Me.TabPage5.Location = New System.Drawing.Point(4, 25)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(519, 355)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Point Sources"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'lblCustom
        '
        Me.lblCustom.Location = New System.Drawing.Point(95, 208)
        Me.lblCustom.Name = "lblCustom"
        Me.lblCustom.Size = New System.Drawing.Size(320, 24)
        Me.lblCustom.TabIndex = 18
        Me.lblCustom.Text = "<none>"
        Me.lblCustom.Visible = False
        '
        'chkCustom
        '
        Me.chkCustom.Location = New System.Drawing.Point(76, 153)
        Me.chkCustom.Name = "chkCustom"
        Me.chkCustom.Size = New System.Drawing.Size(351, 41)
        Me.chkCustom.TabIndex = 17
        Me.chkCustom.Text = "Use custom loading table"
        '
        'chkCalculate
        '
        Me.chkCalculate.Location = New System.Drawing.Point(76, 107)
        Me.chkCalculate.Name = "chkCalculate"
        Me.chkCalculate.Size = New System.Drawing.Size(351, 40)
        Me.chkCalculate.TabIndex = 16
        Me.chkCalculate.Text = "Calculate river mile for each point source"
        Me.chkCalculate.Visible = False
        '
        'cboPoint
        '
        Me.cboPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPoint.Location = New System.Drawing.Point(248, 32)
        Me.cboPoint.Name = "cboPoint"
        Me.cboPoint.Size = New System.Drawing.Size(167, 24)
        Me.cboPoint.TabIndex = 13
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(72, 36)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(144, 17)
        Me.Label19.TabIndex = 12
        Me.Label19.Text = "Point Source ID Field:"
        '
        'cboYear
        '
        Me.cboYear.AllowDrop = True
        Me.cboYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboYear.Location = New System.Drawing.Point(248, 63)
        Me.cboYear.Name = "cboYear"
        Me.cboYear.Size = New System.Drawing.Size(120, 24)
        Me.cboYear.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(72, 67)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(107, 24)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "PCS Year:"
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.lstMet)
        Me.TabPage6.Controls.Add(Me.GroupBox2)
        Me.TabPage6.Location = New System.Drawing.Point(4, 25)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(519, 355)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Met Segments"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.txtMetWDMName)
        Me.GroupBox2.Controls.Add(Me.cmdSelectWDM)
        Me.GroupBox2.Location = New System.Drawing.Point(21, 20)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(475, 59)
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
        Me.txtMetWDMName.Size = New System.Drawing.Size(347, 22)
        Me.txtMetWDMName.TabIndex = 2
        '
        'cmdSelectWDM
        '
        Me.cmdSelectWDM.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSelectWDM.Location = New System.Drawing.Point(384, 21)
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
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(16, 408)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(527, 55)
        Me.GroupBox1.TabIndex = 1
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
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(16, 472)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 32)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "OK"
        '
        'cmdExisting
        '
        Me.cmdExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdExisting.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExisting.Location = New System.Drawing.Point(96, 472)
        Me.cmdExisting.Name = "cmdExisting"
        Me.cmdExisting.Size = New System.Drawing.Size(120, 32)
        Me.cmdExisting.TabIndex = 4
        Me.cmdExisting.Text = "Open Existing"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(224, 472)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(88, 32)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Cancel"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(368, 472)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(79, 32)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(456, 472)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(87, 32)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "About"
        '
        'ofdExisting
        '
        Me.ofdExisting.DefaultExt = "uci"
        Me.ofdExisting.Filter = "UCI files (*.uci)|*.uci"
        Me.ofdExisting.InitialDirectory = "/BASINS/modelout/"
        Me.ofdExisting.Title = "Select UCI"
        '
        'ofdCustom
        '
        Me.ofdCustom.DefaultExt = "dbf"
        Me.ofdCustom.Filter = "DBF Files (*.dbf)|*.dbf"
        Me.ofdCustom.Title = "Select Custom Loading Table"
        '
        'ofdClass
        '
        Me.ofdClass.DefaultExt = "dbf"
        Me.ofdClass.Filter = "DBF Files (*.dbf)|*.dbf"
        Me.ofdClass.Title = "Select Classification File"
        '
        'ofdMetWDM
        '
        Me.ofdMetWDM.DefaultExt = "wdm"
        Me.ofdMetWDM.Filter = "Met WDM files (*.wdm)|*.wdm"
        Me.ofdMetWDM.InitialDirectory = "/BASINS/data/"
        Me.ofdMetWDM.Title = "Select Met WDM File"
        '
        'lstMet
        '
        Me.lstMet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstMet.FormattingEnabled = True
        Me.lstMet.ItemHeight = 16
        Me.lstMet.Location = New System.Drawing.Point(21, 97)
        Me.lstMet.Name = "lstMet"
        Me.lstMet.Size = New System.Drawing.Size(474, 228)
        Me.lstMet.TabIndex = 1
        '
        'frmModelSetup
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(560, 517)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdExisting)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmModelSetup"
        Me.Text = "BASINS"
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
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboSubbasins_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubbasins.SelectedIndexChanged
        Dim lyr As Long
        Dim i As Long
        Dim ctemp As String

        cboSub1.Items.Clear()
        cboSub2.Items.Clear()
        cboSub3.Items.Clear()
        cboSub3.Items.Add("<none>")
        lyr = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
        If lyr > -1 Then
            'fill in fields 
            For i = 0 To GisUtil.NumFields(lyr) - 1
                ctemp = GisUtil.FieldName(i, lyr)
                cboSub1.Items.Add(ctemp)
                cboSub2.Items.Add(ctemp)
                cboSub3.Items.Add(ctemp)
                If UCase(ctemp) = "SUBBASIN" Or UCase(ctemp) = "STREAMLINK" Then
                    cboSub1.SelectedIndex = i
                End If
                If UCase(ctemp) = "SLO1" Or UCase(ctemp) = "SLOPE" Or UCase(ctemp) = "AVESLOPE" Then
                    cboSub2.SelectedIndex = i
                End If
                If UCase(ctemp) = "MODELSEG" Then
                    cboSub3.SelectedIndex = i + 1
                End If
            Next
        End If
        If cboSub1.Items.Count > 0 And cboSub1.SelectedIndex < 0 Then
            cboSub1.SelectedIndex = 0
        End If
        If cboSub2.Items.Count > 0 And cboSub2.SelectedIndex < 0 Then
            cboSub2.SelectedIndex = 0
        End If
        If cboSub3.Items.Count > 0 And cboSub3.SelectedIndex < 0 Then
            cboSub3.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboStreams_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStreams.SelectedIndexChanged
        Dim lyr As Long
        Dim i As Long
        Dim ctemp As String

        cboStream1.Items.Clear()
        cboStream2.Items.Clear()
        cboStream3.Items.Clear()
        cboStream4.Items.Clear()
        cboStream5.Items.Clear()
        cboStream6.Items.Clear()
        cboStream7.Items.Clear()
        cboStream8.Items.Clear()
        cboStream9.Items.Clear()
        lyr = GisUtil.LayerIndex(cboStreams.Items(cboStreams.SelectedIndex))
        If lyr > -1 Then
            'fill in fields 
            For i = 0 To GisUtil.NumFields(lyr) - 1
                ctemp = GisUtil.FieldName(i, lyr)
                cboStream1.Items.Add(ctemp)
                cboStream2.Items.Add(ctemp)
                cboStream3.Items.Add(ctemp)
                cboStream4.Items.Add(ctemp)
                cboStream5.Items.Add(ctemp)
                cboStream6.Items.Add(ctemp)
                cboStream7.Items.Add(ctemp)
                cboStream8.Items.Add(ctemp)
                cboStream9.Items.Add(ctemp)
                If UCase(ctemp) = "SUBBASIN" Or UCase(ctemp) = "LINKNO" Then
                    cboStream1.SelectedIndex = i
                End If
                If UCase(ctemp) = "SUBBASINR" Or UCase(ctemp) = "DSLINKNO" Then
                    cboStream2.SelectedIndex = i
                End If
                If UCase(ctemp) = "LEN2" Or UCase(ctemp) = "LENGTH" Then
                    cboStream3.SelectedIndex = i
                End If
                If UCase(ctemp) = "SLO2" Or UCase(ctemp) = "SLOPE" Then
                    cboStream4.SelectedIndex = i
                End If
                If UCase(ctemp) = "WID2" Or UCase(ctemp) = "WIDTH" Or UCase(ctemp) = "MEANWIDTH" Then
                    cboStream5.SelectedIndex = i
                End If
                If UCase(ctemp) = "DEP2" Or UCase(ctemp) = "DEPTH" Or UCase(ctemp) = "MEANDEPTH" Then
                    cboStream6.SelectedIndex = i
                End If
                If UCase(ctemp) = "MINEL" Or UCase(ctemp) = "ELEVLOW" Then
                    cboStream7.SelectedIndex = i
                End If
                If UCase(ctemp) = "MAXEL" Or UCase(ctemp) = "ELEVHIGH" Then
                    cboStream8.SelectedIndex = i
                End If
                If UCase(ctemp) = "SNAME" Then
                    cboStream9.SelectedIndex = i
                End If
            Next
        End If
        If cboStream1.Items.Count > 0 And cboStream1.SelectedIndex < 0 Then
            cboStream1.SelectedIndex = 0
        End If
        If cboStream2.Items.Count > 0 And cboStream2.SelectedIndex < 0 Then
            cboStream2.SelectedIndex = 0
        End If
        If cboStream3.Items.Count > 0 And cboStream3.SelectedIndex < 0 Then
            cboStream3.SelectedIndex = 0
        End If
        If cboStream4.Items.Count > 0 And cboStream4.SelectedIndex < 0 Then
            cboStream4.SelectedIndex = 0
        End If
        If cboStream5.Items.Count > 0 And cboStream5.SelectedIndex < 0 Then
            cboStream5.SelectedIndex = 0
        End If
        If cboStream6.Items.Count > 0 And cboStream6.SelectedIndex < 0 Then
            cboStream6.SelectedIndex = 0
        End If
        If cboStream7.Items.Count > 0 And cboStream7.SelectedIndex < 0 Then
            cboStream7.SelectedIndex = 0
        End If
        If cboStream8.Items.Count > 0 And cboStream8.SelectedIndex < 0 Then
            cboStream8.SelectedIndex = 0
        End If
        If cboStream9.Items.Count > 0 And cboStream9.SelectedIndex < 0 Then
            cboStream9.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboOutlets_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboOutlets.SelectedIndexChanged
        Dim lyr As Long
        Dim i As Long
        Dim ctemp As String

        cboPoint.Items.Clear()
        If cboOutlets.Items(cboOutlets.SelectedIndex) <> "<none>" Then
            lyr = GisUtil.LayerIndex(cboOutlets.Items(cboOutlets.SelectedIndex))
            'fill in fields 
            For i = 0 To GisUtil.NumFields(lyr) - 1
                ctemp = GisUtil.FieldName(i, lyr)
                cboPoint.Items.Add(ctemp)
                If UCase(ctemp) = "PCSID" Then
                    cboPoint.SelectedIndex = i
                End If
            Next
            If cboPoint.Items.Count > 0 And cboPoint.SelectedIndex < 0 Then
                cboPoint.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        Dim lyr As Long
        Dim ldef As Integer

        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            cboLandUseLayer.Visible = False
            lblLandUseLayer.Visible = False
            cboDescription.Visible = False
            lblDescription.Visible = False
            lblClass.Text = "/BASINS/etc/giras.dbf"
            SetPerviousGrid()
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
            cboLandUseLayer.Items.Clear()
            ldef = 0
            For lyr = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerType(lyr) = 3 Then
                    'PolygonShapefile
                    cboLandUseLayer.Items.Add(GisUtil.LayerName(lyr))
                    If GisUtil.NumFeatures(lyr) < 1000 And ldef = 0 Then
                        ldef = cboLandUseLayer.Items.Count
                    End If
                End If
            Next
            If cboLandUseLayer.Items.Count > 0 And cboLandUseLayer.SelectedIndex < 0 Then
                'pick one without too many polygons for efficiency
                cboLandUseLayer.SelectedIndex = ldef - 1
            End If
            cboLandUseLayer.Visible = True
            lblLandUseLayer.Visible = True
            cboDescription.Visible = True
            lblDescription.Visible = True
            lblClass.Text = "<none>"
            SetPerviousGrid()
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
            cboLandUseLayer.Items.Clear()
            For lyr = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerType(lyr) = 4 Then
                    'Grid 
                    If InStr(GisUtil.LayerFileName(lyr), "\nlcd\") > 0 Then
                        cboLandUseLayer.Items.Add(GisUtil.LayerName(lyr))
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
            SetPerviousGrid()
        Else 'grid
            cboLandUseLayer.Items.Clear()
            For lyr = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerType(lyr) = 4 Then
                    'Grid
                    cboLandUseLayer.Items.Add(GisUtil.LayerName(lyr))
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

    Private Sub cboLandUseLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLandUseLayer.SelectedIndexChanged
        Dim i As Long, lyr As Long

        cboDescription.Items.Clear()
        lyr = GisUtil.LayerIndex(cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex))
        If lyr > -1 Then
            If cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Grid" Then
                'make sure this is a grid layer
                If GisUtil.LayerType(lyr) = 4 Then
                    'todo: fill in description fields for selected grid layer if possible
                End If
            Else
                'make sure this is a shape layer
                If GisUtil.LayerType(lyr) = 3 Then
                    'PolygonShapefile
                    'this is the layer, fill in fields 
                    For i = 0 To GisUtil.NumFields(lyr) - 1
                        'MsgBox(sf.Field(i).Name)
                        cboDescription.Items.Add(GisUtil.FieldName(i, lyr))
                        If GisUtil.FieldType(i, lyr) = 0 Then
                            'string
                            cboDescription.SelectedIndex = i
                        End If
                    Next
                    If cboDescription.Items.Count > 0 And cboDescription.SelectedIndex < 0 Then
                        cboDescription.SelectedIndex = 0
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub SetPerviousGrid()
        Dim LandUseLayerName As String, LandUseFieldName As String
        Dim i As Long, k As Long, lyr As Long
        Dim alreadyinlist As Boolean
        Dim tmpDbf As IatcTable
        Dim tcode As Long, prevcode As Long
        Dim showmults As Boolean, showcodes As Boolean
        Dim lSorted As New atcCollection

        If AtcGridPervious.Source Is Nothing Then Exit Sub

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

            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lblClass.Text)
            'do pre-scan to set up grid
            prevcode = -1
            showmults = False
            showcodes = False
            Dim cGroupNames As New Collection
            Dim cGroupPercent As New Collection
            For i = 1 To tmpDbf.NumRecords
                'scan to see if multiple records for the same code
                tmpDbf.CurrentRecord = i
                tcode = tmpDbf.Value(1)
                If tcode = prevcode Then
                    showmults = True
                End If
                prevcode = tcode
                'scan to see if perv percent varies within a group
                alreadyinlist = False
                'If cGroupNames.Count > 0 Then
                For k = 1 To cGroupNames.Count
                    If cGroupNames(k) = tmpDbf.Value(2) Then
                        alreadyinlist = True
                        If cGroupPercent(k) <> tmpDbf.Value(3) Then
                            showcodes = True
                        End If
                        Exit For
                    End If
                Next k
                'End If
                If Not alreadyinlist Then
                    cGroupNames.Add(tmpDbf.Value(2))
                    cGroupPercent.Add(tmpDbf.Value(3))
                End If
            Next i

            If showmults Then
                showcodes = True
            End If

            'sort list items
            For i = 1 To tmpDbf.NumRecords
                tmpDbf.CurrentRecord = i
                lSorted.Add(tmpDbf.Value(1), i)
            Next i
            lSorted.Sort()

            'now populate grid
            With AtcGridPervious.Source
                For Each i In lSorted
                    tmpDbf.CurrentRecord = i
                    If Not showcodes Then
                        'just show group desc and percent perv
                        alreadyinlist = False
                        For k = 1 To .Rows
                            If .CellValue(k - 1, 1) = tmpDbf.Value(2) Then
                                alreadyinlist = True
                            End If
                        Next
                        If Not alreadyinlist Then
                            .Rows = .Rows + 1
                            .CellValue(.Rows - 1, 1) = tmpDbf.Value(2)
                            .CellValue(.Rows - 1, 2) = tmpDbf.Value(3)
                            .CellEditable(.Rows - 1, 2) = True
                            .CellColor(.Rows - 1, 1) = Me.BackColor
                        End If
                    Else
                        'need to show whole table
                        .Rows = .Rows + 1
                        .CellValue(.Rows - 1, 0) = tmpDbf.Value(1)
                        .CellValue(.Rows - 1, 1) = tmpDbf.Value(2)
                        .CellValue(.Rows - 1, 2) = tmpDbf.Value(3)
                        .CellValue(.Rows - 1, 3) = tmpDbf.Value(4)
                        .CellValue(.Rows - 1, 4) = tmpDbf.Value(5)
                    End If
                Next
            End With

            AtcGridPervious.SizeAllColumnsToContents()
            If showmults Then
                showcodes = True
            Else
                AtcGridPervious.ColumnWidth(3) = 0
                AtcGridPervious.ColumnWidth(4) = 0
            End If
            If Not showcodes Then
                AtcGridPervious.ColumnWidth(0) = 0
            End If

        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
            If cboLandUseLayer.SelectedIndex > -1 And cboDescription.SelectedIndex > -1 Then
                LandUseLayerName = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
                LandUseFieldName = cboDescription.Items(cboDescription.SelectedIndex)
                'no reclass file, get unique landuse names
                lyr = GisUtil.LayerIndex(LandUseLayerName)
                If lyr > -1 Then
                    Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                    FillListUniqueLandUses(lyr, LandUseFieldName)
                    Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                End If
            End If

        Else
            'other grid types with no reclass file set
            If cboLandUseLayer.SelectedIndex > -1 Then
                LandUseLayerName = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
                'get unique landuse names
                lyr = GisUtil.LayerIndex(LandUseLayerName)
                If GisUtil.LayerType(lyr) = 4 Then
                    'Grid
                    Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                    For i = Convert.ToInt32(GisUtil.GridLayerMinimum(lyr)) To Convert.ToInt32(GisUtil.GridLayerMaximum(lyr))
                        AtcGridPervious.Source.Rows = AtcGridPervious.Source.Rows + 1
                        AtcGridPervious.Source.CellValue(AtcGridPervious.Source.Rows - 1, 0) = i
                        AtcGridPervious.Source.CellValue(AtcGridPervious.Source.Rows - 1, 1) = i
                        AtcGridPervious.Source.CellValue(AtcGridPervious.Source.Rows - 1, 2) = 100
                    Next i
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
            For i = 1 To .Rows - 1
                .CellEditable(i, 2) = True
                .CellEditable(i, 3) = True
                .CellEditable(i, 4) = True
                .CellColor(i, 0) = SystemColors.ControlDark
                .CellColor(i, 1) = SystemColors.ControlDark
            Next i
        End With
        AtcGridPervious.Refresh()

    End Sub

    Private Sub cboDescription_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDescription.SelectedIndexChanged
        SetPerviousGrid()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        MsgBox("BASINS " & pModelName & " for MapWindow" & vbCrLf & vbCrLf & "Version 1.1", , "BASINS " & pModelName)
    End Sub

    Private Sub cmdExisting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExisting.Click
        Dim uciname As String

        If pModelName = "AQUATOX" Then
            StartAQUATOX("")
        Else
            If ofdExisting.ShowDialog() = Windows.Forms.DialogResult.OK Then
                uciname = ofdExisting.FileName
                StartWinHSPF(uciname)
            End If
        End If
    End Sub

    Private Sub StartWinHSPF(ByVal ucommand As String)
        Dim WinHSPFexe As String

        'todo:  get this from the registry
        WinHSPFexe = "c:\basins\models\hspf\bin\winhspf.exe"
        If Not FileExists(WinHSPFexe) Then
            WinHSPFexe = "d:\basins\models\hspf\bin\winhspf.exe"
        End If
        If Not FileExists(WinHSPFexe) Then
            WinHSPFexe = "e:\basins\models\hspf\bin\winhspf.exe"
        End If
        If Not FileExists(WinHSPFexe) Then
            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            WinHSPFexe = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "models\hspf\bin\winhspf.exe"
        End If
        If Not FileExists(WinHSPFexe) Then
            WinHSPFexe = FindFile("Please locate WinHSPF.exe", "WinHSPF.exe")
        End If
        If FileExists(WinHSPFexe) Then
            Process.Start(WinHSPFexe, ucommand)
        Else
            MsgBox("Cannot find WinHSPF.exe", MsgBoxStyle.Critical, "BASINS HSPF Problem")
        End If
    End Sub

    Public Sub StartAQUATOX(ByVal ucommand$)
        Dim AQUATOXexe$
        'Dim reg As New ATCoRegistry

        'todo:  get this from the registry
        'AQUATOXexe = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\Eco Modeling\AQUATOX\ExePath", "") & "\AQUATOX.exe"
        AQUATOXexe = "\basins\models\AQUATOX\AQUATOX.exe"

        If FileExists(AQUATOXexe) Then
            Process.Start(AQUATOXexe, ucommand)
        Else
            MsgBox("Cannot find AQUATOX.exe", MsgBoxStyle.Critical, "BASINS AQUATOX Problem")
        End If
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        'MsgBox("Help is not yet implemented.", MsgBoxStyle.Critical, "BASINS " & pModelName & " Problem")
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
    End Sub

    Private Sub FillListUniqueLandUses(ByVal layerindex As Long, ByVal fieldname As String)
        Dim i As Long, j As Long, k As Long
        Dim alreadyinlist As Boolean
        Dim lstr As String

        i = GisUtil.FieldIndex(layerindex, fieldname)
        If i > -1 Then
            'this is the field we want, get land use types
            Dim cUnique As New Collection
            For j = 0 To GisUtil.NumFeatures(layerindex) - 1
                alreadyinlist = False
                lstr = GisUtil.FieldValue(layerindex, j, i)
                For k = 1 To cUnique.Count
                    If cUnique(k) = lstr Then
                        alreadyinlist = True
                    End If
                Next
                If Not alreadyinlist Then
                    With AtcGridPervious.Source
                        .Rows = AtcGridPervious.Source.Rows + 1
                        .CellValue(.Rows - 1, 1) = GisUtil.FieldValue(layerindex, j, i)
                        .CellValue(.Rows - 1, 0) = GisUtil.FieldValue(layerindex, j, i)
                        If UCase(Microsoft.VisualBasic.Left(GisUtil.FieldValue(layerindex, j, i), 5)) = "URBAN" Then
                            .CellValue(.Rows - 1, 2) = 50
                        Else
                            .CellValue(.Rows - 1, 2) = 0
                        End If
                        .CellEditable(.Rows - 1, 2) = True
                        cUnique.Add(GisUtil.FieldValue(layerindex, j, i))
                    End With
                End If
            Next j
            With AtcGridPervious
                .SizeAllColumnsToContents()
                .ColumnWidth(0) = 0
                .ColumnWidth(3) = 0
                .ColumnWidth(4) = 0
            End With
        End If
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim OutputPath As String
        Dim BaseOutputName As String
        Dim DriveLetter As String

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        OutputPath = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "modelout\"
        If FileExists(OutputPath) Then
            OutputPath = OutputPath & tbxName.Text
        Else
            DriveLetter = Mid(CurDir(), 1, 1)
            OutputPath = DriveLetter & ":\BASINS\modelout\" & tbxName.Text
        End If
        BaseOutputName = tbxName.Text

        If pModelName = "AQUATOX" Then
            If SetupAQUATOX(OutputPath, BaseOutputName) Then
                StartAQUATOX(" UNKNOWN " & OutputPath)
            End If
        Else
            If SetupHSPF(OutputPath, BaseOutputName) Then
                If CreateUCI(OutputPath & "\" & BaseOutputName & ".uci", txtMetWDMName.Text) Then
                    StartWinHSPF(OutputPath & "\" & BaseOutputName & ".uci")
                Else
                    'below is the old way of creating a uci, in WinHSPF
                    StartWinHSPF(OutputPath & "\" & BaseOutputName & ".wsd")
                End If
            End If
        End If
    End Sub

    Public Function SetupHSPF(ByVal OutputPath As String, ByVal BaseOutputName As String) As Boolean
        Dim luDriveLetter As String
        Dim BaseFileName As String
        Dim SubbasinThemeName As String
        Dim SubbasinLayerIndex As Long
        Dim SubbasinFieldName As String
        Dim SubbasinFieldIndex As Long
        Dim SubbasinSlopeIndex As Long
        Dim LanduseFieldName As String = ""
        Dim LandUseThemeName As String = ""
        Dim OutletsThemeName As String
        Dim LanduseLayerIndex As Long
        Dim luPathName As String
        Dim NewFileName As String
        Dim ReclassifyFile As String = ""
        Dim i As Long
        Dim j As Long
        Dim k As Long
        Dim LandUseFieldIndex As Long
        Dim incollection As Boolean
        Dim shapeindex As Long
        Dim s As String
        Dim area As Double
        Dim subslope As Double
        Dim lucode As String, subid As String
        Dim totalpolygoncount As Long, polygoncount As Long
        Dim lastdisplayed As Long
        Dim aAreaLS(,) As Double
        Dim bfirst As Boolean
        Dim tmpDbf As IatcTable

        On Error GoTo ErrHand

        lblStatus.Text = "Preparing to process"
        Me.Refresh()
        EnableControls(False)

        If Not PreProcessChecking(OutputPath, BaseOutputName) Then
            'failed on one of the early checks, exit sub
            Exit Function
        End If
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'set subbasin indexes
        SubbasinThemeName = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        SubbasinFieldName = cboSub1.Items(cboSub1.SelectedIndex)
        SubbasinLayerIndex = GisUtil.LayerIndex(SubbasinThemeName)
        SubbasinFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, SubbasinFieldName)
        SubbasinSlopeIndex = GisUtil.FieldIndex(SubbasinLayerIndex, cboSub2.Items(cboSub2.SelectedIndex))
        'are any subbasins selected?
        Dim cSelectedSubbasins As New Collection
        For i = 1 To GisUtil.NumSelectedFeatures(SubbasinLayerIndex)
            cSelectedSubbasins.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, SubbasinLayerIndex))
        Next
        If cSelectedSubbasins.Count = 0 Then
            'no subbasins selected, act as if all are selected
            For i = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
                cSelectedSubbasins.Add(i - 1)
            Next
        End If

        'set landuse layer
        If cboLandUseLayer.SelectedIndex > -1 Then
            LandUseThemeName = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
        End If
        If cboDescription.SelectedIndex > -1 Then
            LanduseFieldName = cboDescription.Items(cboDescription.SelectedIndex)
        End If

        Dim cLucode As New Collection
        Dim cSubid As New Collection
        Dim cArea As New Collection
        Dim cSubSlope As New Collection

        If cboLanduse.SelectedIndex = 0 Then
            'usgs giras is the selected land use type
            LandUseThemeName = "Land Use Index"
            LanduseFieldName = "COVNAME"

            lblStatus.Text = "Selecting land use tiles for overlay"
            Me.Refresh()

            'set land use index layer
            LanduseLayerIndex = GisUtil.LayerIndex(LandUseThemeName)
            LandUseFieldIndex = GisUtil.FieldIndex(LanduseLayerIndex, LanduseFieldName)
            luPathName = PathNameOnly(GisUtil.LayerFileName(LanduseLayerIndex))
            luPathName &= "\landuse"
            luDriveLetter = Mid(luPathName, 1, 1)
            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            ReclassifyFile = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
            If FileExists(ReclassifyFile) Then
                ReclassifyFile = ReclassifyFile & "giras.dbf"
            Else
                ReclassifyFile = luDriveLetter & ":\basins\etc\giras.dbf"
            End If

            'figure out which land use tiles to overlay
            Dim cluTiles As New Collection
            For i = 1 To GisUtil.NumFeatures(LanduseLayerIndex)
                'loop thru each shape of land use index shapefile
                For j = 1 To cSelectedSubbasins.Count
                    'loop thru each selected subbasin (or all if none selected)
                    shapeindex = cSelectedSubbasins(j)
                    If GisUtil.OverlappingPolygons(LanduseLayerIndex, i - 1, SubbasinLayerIndex, shapeindex) Then
                        'add this to collection of tiles we'll need
                        incollection = False
                        For Each s In cluTiles
                            If s = GisUtil.FieldValue(LanduseLayerIndex, i - 1, LandUseFieldIndex) Then
                                incollection = True
                            End If
                        Next
                        If Not incollection Then
                            cluTiles.Add(GisUtil.FieldValue(LanduseLayerIndex, i - 1, LandUseFieldIndex))
                        End If
                    End If
                Next j
            Next i

            'add tiles if not already on map
            'figure out how many polygons to overlay, for status message
            totalpolygoncount = 0
            For j = 1 To cluTiles.Count
                'loop thru each land use tile
                NewFileName = luPathName & "\" & cluTiles(j) & ".shp"
                If Not GisUtil.AddLayer(NewFileName, cluTiles(j)) Then
                    MsgBox("The GIRAS Landuse Shapefile " & NewFileName & "does not exist." & _
                           vbCrLf & "Run the Download tool to bring this data into your project.", vbOKOnly, "HSPF Problem")
                    EnableControls(True)
                    Exit Function
                End If
                totalpolygoncount = totalpolygoncount + GisUtil.NumFeatures(GisUtil.LayerIndex(cluTiles(j)))
            Next j
            totalpolygoncount = totalpolygoncount * cSelectedSubbasins.Count

            'reset selected features since they may have become unselected
            If cSelectedSubbasins.Count < GisUtil.NumFeatures(SubbasinLayerIndex) Then
                For i = 1 To cSelectedSubbasins.Count
                    GisUtil.SetSelectedFeature(SubbasinLayerIndex, cSelectedSubbasins(i))
                Next i
            End If

            polygoncount = 0
            lastdisplayed = 0
            LanduseFieldName = "LUCODE"
            For j = 1 To cluTiles.Count
                'loop thru each land use tile
                If j = 1 Then
                    bfirst = True
                Else
                    bfirst = False
                End If
                'lblStatus.Text = "Overlaying Land Use and Subbasins (" & Int(polygoncount / totalpolygoncount * 100) & "%)"
                lblStatus.Text = "Overlaying Land Use and Subbasins (Tile " & j & " of " & cluTiles.Count & ")"
                Me.Refresh()

                'do overlay
                GisUtil.Overlay(cluTiles(j), LanduseFieldName, SubbasinThemeName, SubbasinFieldName, _
                                luPathName & "\overlay.shp", bfirst)
            Next j

            'compile areas, slopes and lengths
            lblStatus.Text = "Compiling Overlay Results"
            Me.Refresh()

            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(luPathName & "\overlay.dbf")
            For i = 1 To tmpDbf.NumRecords
                tmpDbf.CurrentRecord = i
                lucode = tmpDbf.Value(1)
                subid = tmpDbf.Value(2)
                area = tmpDbf.Value(3)
                cLucode.Add(lucode)
                cSubid.Add(subid)
                cArea.Add(area)
                For j = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
                    k = GisUtil.FieldValue(SubbasinLayerIndex, j - 1, SubbasinFieldIndex)
                    If k = subid Then
                        subslope = GisUtil.FieldValue(SubbasinLayerIndex, j - 1, SubbasinSlopeIndex)
                        cSubSlope.Add(subslope)
                        Exit For
                    End If
                Next j
            Next i

        ElseIf cboLanduse.SelectedIndex = 1 Or cboLanduse.SelectedIndex = 3 Then
            'nlcd grid or other grid
            If cboLanduse.SelectedIndex = 1 Then
                'nlcd grid
                Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
                ReclassifyFile = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
                If FileExists(ReclassifyFile) Then
                    ReclassifyFile = ReclassifyFile & "nlcd.dbf"
                Else
                    ReclassifyFile = "\BASINS\etc\nlcd.dbf"
                End If
            Else
                If lblClass.Text <> "<none>" Then
                    ReclassifyFile = lblClass.Text
                End If
            End If

            lblStatus.Text = "Overlaying Land Use and Subbasins"
            Me.Refresh()

            LanduseLayerIndex = GisUtil.LayerIndex(LandUseThemeName)
            If GisUtil.LayerType(LanduseLayerIndex) = 4 Then
                'the landuse layer is a grid

                k = Convert.ToInt32(GisUtil.GridLayerMaximum(LanduseLayerIndex))
                ReDim aAreaLS(k, GisUtil.NumFeatures(SubbasinLayerIndex))
                GisUtil.TabulateAreas(LanduseLayerIndex, SubbasinLayerIndex, aAreaLS)

                For k = 1 To cSelectedSubbasins.Count
                    'loop thru each selected subbasin (or all if none selected)
                    shapeindex = cSelectedSubbasins(k)
                    subid = GisUtil.FieldValue(SubbasinLayerIndex, shapeindex, SubbasinFieldIndex)
                    For i = 1 To Convert.ToInt32(GisUtil.GridLayerMaximum(LanduseLayerIndex))
                        If aAreaLS(i, shapeindex) > 0 Then
                            subslope = GisUtil.FieldValue(SubbasinLayerIndex, shapeindex, SubbasinSlopeIndex)
                            cLucode.Add(i)
                            cArea.Add(aAreaLS(i, shapeindex))
                            cSubid.Add(subid)
                            cSubSlope.Add(subslope)
                        End If
                    Next i
                Next k
            End If

        ElseIf cboLanduse.SelectedIndex = 2 Then
            'other shape
            lblStatus.Text = "Overlaying Land Use and Subbasins"
            Me.Refresh()

            If lblClass.Text <> "<none>" Then
                ReclassifyFile = lblClass.Text
            End If

            LanduseLayerIndex = GisUtil.LayerIndex(LandUseThemeName)
            luPathName = PathNameOnly(GisUtil.LayerFileName(LanduseLayerIndex))

            'do overlay
            GisUtil.Overlay(LandUseThemeName, LanduseFieldName, SubbasinThemeName, SubbasinFieldName, _
                            luPathName & "\overlay.shp", True)

            'compile areas and slopes
            lblStatus.Text = "Compiling Overlay Results"
            Me.Refresh()

            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(luPathName & "\overlay.dbf")
            For i = 1 To tmpDbf.NumRecords
                tmpDbf.CurrentRecord = i
                lucode = tmpDbf.Value(1)
                subid = tmpDbf.Value(2)
                area = tmpDbf.Value(3)
                cLucode.Add(lucode)
                cSubid.Add(subid)
                cArea.Add(area)
                For j = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
                    k = GisUtil.FieldValue(SubbasinLayerIndex, j - 1, SubbasinFieldIndex)
                    If k = subid Then
                        subslope = GisUtil.FieldValue(SubbasinLayerIndex, j - 1, SubbasinSlopeIndex)
                        cSubSlope.Add(subslope)
                        Exit For
                    End If
                Next j
            Next i
        End If

        lblStatus.Text = "Completed overlay of subbasins and land use layers"
        Me.Refresh()

        'figure out which outlets are in which subbasins
        OutletsThemeName = cboOutlets.Items(cboOutlets.SelectedIndex)
        Dim cOutSubs As New Collection
        If OutletsThemeName <> "<none>" Then
            lblStatus.Text = "Joining point sources to subbasins"
            Me.Refresh()
            i = GisUtil.LayerIndex(OutletsThemeName)
            For j = 1 To GisUtil.NumFeatures(i)
                k = GisUtil.PointInPolygon(i, j, SubbasinLayerIndex)
                If k > -1 Then
                    cOutSubs.Add(GisUtil.FieldValue(SubbasinLayerIndex, k, SubbasinFieldIndex))
                Else
                    cOutSubs.Add(-1)
                End If
            Next j
        End If

        'build collection of unique subbasin ids
        Dim cUniqueSubids As New Collection
        For i = 1 To cSubid.Count
            incollection = False
            For j = 1 To cUniqueSubids.Count
                If cUniqueSubids(j) = cSubid(i) Then
                    incollection = True
                    Exit For
                End If
            Next j
            If Not incollection Then
                cUniqueSubids.Add(cSubid(i))
            End If
        Next i


        'make output folder
        MkDirPath(OutputPath)
        BaseFileName = OutputPath & "\" & BaseOutputName

        'write wsd file
        lblStatus.Text = "Writing WSD file"
        Me.Refresh()
        WriteWSDFile(BaseFileName & ".wsd", cArea, cLucode, cSubid, cSubSlope, ReclassifyFile)

        'write rch file (and ptf)
        lblStatus.Text = "Writing RCH and PTF files"
        Me.Refresh()
        WriteRCHFile(BaseFileName & ".rch", cUniqueSubids)

        'write psr file
        lblStatus.Text = "Writing PSR file"
        Me.Refresh()
        WritePSRFile(BaseFileName & ".psr", cUniqueSubids, cOutSubs)

        'write seg file
        lblStatus.Text = "Writing SEG file"
        Me.Refresh()
        WriteSEGFile(BaseFileName & ".seg")

        'write map file
        lblStatus.Text = "Writing MAP file"
        Me.Refresh()
        WriteMAPFile(BaseFileName & ".map")

        'start winhspf
        lblStatus.Text = "Starting WinHSPF"
        Me.Refresh()
        Me.Dispose()
        Me.Close()

        Return True

ErrHand:
        MsgBox("An error occurred: " & Err.Description, vbOKOnly, "BASINS " & pModelName & " Error")
        Me.Dispose()
        Me.Close()
        Return False

    End Function

    Private Function SetupAQUATOX(ByVal OutputPath As String, ByVal BaseOutputName As String) As Boolean
        Dim BaseFileName As String
        Dim i As Long
        Dim StreamsThemeName As String
        Dim StreamsFieldName As String
        Dim StreamsLayerIndex As Long
        Dim StreamsFieldIndex As Long

        On Error GoTo ErrHand

        lblStatus.Text = "Preparing to process"
        Me.Refresh()
        EnableControls(False)

        If Not PreProcessChecking(OutputPath, BaseOutputName) Then
            'failed on one of the early checks, exit sub
            Return False
        End If
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'are any streams selected?
        StreamsThemeName = cboStreams.Items(cboStreams.SelectedIndex)
        StreamsFieldName = cboStream1.Items(cboStream1.SelectedIndex)
        StreamsLayerIndex = GisUtil.LayerIndex(StreamsThemeName)
        StreamsFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, StreamsFieldName)
        Dim cSelectedStreams As New Collection
        For i = 1 To GisUtil.NumSelectedFeatures(StreamsLayerIndex)
            cSelectedStreams.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, StreamsLayerIndex))
        Next
        If cSelectedStreams.Count <> 1 Then
            MsgBox("BASINS AQUATOX requires one and only one selected stream segment.", MsgBoxStyle.Critical, "BASINS AQUATOX Problem")
            EnableControls(True)
            Return False
        End If

        'get the id of the selected stream
        Dim cUniqueStreamIds As New Collection
        cUniqueStreamIds.Add(GisUtil.FieldValue(StreamsLayerIndex, cSelectedStreams(1), StreamsFieldIndex))

        'make output folder
        MkDirPath(OutputPath)
        BaseFileName = OutputPath & "\" & BaseOutputName

        'write rch file (and ptf)
        lblStatus.Text = "Writing RCH and PTF files"
        Me.Refresh()
        WriteRCHFile(BaseFileName & ".rch", cUniqueStreamIds)

        'write psr file
        lblStatus.Text = "Writing PSR file"
        Me.Refresh()
        Dim cOutSubs As New Collection
        WritePSRFile(BaseFileName & ".psr", cUniqueStreamIds, cOutSubs)

        'start aquatox
        lblStatus.Text = "Starting AQUATOX"
        Me.Refresh()
        Me.Dispose()
        Me.Close()

        Return True

ErrHand:
        MsgBox("An error occurred: " & Err.Description, vbOKOnly, "BASINS " & pModelName & " Error")
        Me.Dispose()
        Me.Close()
        Return False

    End Function

    Private Sub EnableControls(ByVal b As Boolean)
        cmdOK.Enabled = b
        cmdExisting.Enabled = b
        cmdHelp.Enabled = b
        cmdCancel.Enabled = b
        cmdAbout.Enabled = b
        TabControl1.Enabled = b
    End Sub

    Private Function PreProcessChecking(ByVal OutputPath As String, ByVal BaseOutputName As String) As Boolean
        Dim i As Long
        Dim wsdFileName As String
        Dim rchFileName As String

        PreProcessChecking = True
        If pModelName <> "AQUATOX" Then
            If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
                i = GisUtil.LayerIndex("Land Use Index")
                If i = -1 Then
                    'cant do giras without land use index layer
                    MsgBox("When using GIRAS Landuse, the 'Land Use Index' layer must exist and be named as such.", vbOKOnly, "HSPF GIRAS Problem")
                    EnableControls(True)
                    PreProcessChecking = False
                    Exit Function
                End If
            End If

            If cboLanduse.SelectedIndex <> 0 Then
                'not giras, make sure subbasins and land use layers aren't the same
                If cboSubbasins.Items(cboSubbasins.SelectedIndex) = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex) Then
                    'same layer cannot be used for both
                    MsgBox("The same layer cannot be used for the subbasins layer and the landuse layer.", vbOKOnly, "BASINS HSPF Problem")
                    EnableControls(True)
                    PreProcessChecking = False
                    Exit Function
                End If
            End If

            'see if these files already exist
            wsdFileName = OutputPath & "\" & BaseOutputName & ".wsd"
            If Len(Dir(wsdFileName)) > 0 Then
                'already exists
                i = MsgBox("HSPF Project '" & BaseOutputName & "' already exists.  Do you want to overwrite it?", vbOKCancel, "Overwrite?")
                If i = 2 Then
                    EnableControls(True)
                    PreProcessChecking = False
                    Exit Function
                End If
            End If
        Else
            'in AQUATOX, see if these files already exist
            rchFileName = OutputPath & "\" & BaseOutputName & ".rch"
            If Len(Dir(rchFileName)) > 0 Then
                'already exists
                i = MsgBox("AQUATOX Project '" & BaseOutputName & "' already exists.  Do you want to overwrite it?", vbOKCancel, "Overwrite?")
                If i = 2 Then
                    EnableControls(True)
                    PreProcessChecking = False
                    Exit Function
                End If
            End If
        End If
    End Function

    Private Sub WriteWSDFile(ByVal WsdFileName As String, ByVal cArea As Collection, ByVal cLucode As Collection, ByVal cSubid As Collection, _
                             ByVal cSubslope As Collection, ByVal ReclassifyFile As String)
        Dim OutFile As Integer
        Dim i As Integer, j As Integer, k As Integer
        Dim incollection As Boolean
        Dim percentimperv As Double
        Dim tarea As Double, stype As String
        Dim tmpDbf As IatcTable
        Dim PerArea(,) As Single
        Dim ImpArea(,) As Single
        Dim length() As Single
        Dim slope() As Single
        Dim UseSimpleGrid As Boolean
        Dim spos As Long
        Dim lpos As Long
        Dim luname As String = ""
        Dim multiplier As Single
        Dim subbasin As String
        Dim useit As Boolean

        'if simple reclassifyfile exists, read it in
        Dim cRcode As New Collection
        Dim cRname As New Collection
        UseSimpleGrid = False
        If Len(ReclassifyFile) > 0 And AtcGridPervious.ColumnWidth(0) = 0 Then
            'have the simple percent pervious grid, need to know which 
            'lucodes correspond to which lugroups
            UseSimpleGrid = True
            'open dbf file
            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(ReclassifyFile)
            For i = 1 To tmpDbf.NumRecords
                tmpDbf.CurrentRecord = i
                cRcode.Add(tmpDbf.Value(1))
                cRname.Add(tmpDbf.Value(2))
            Next i
        End If

        'create summary array 
        '  area of each land use group in each subbasin

        'build collection of unique subbasin ids
        Dim cUniqueSubids As New Collection
        For i = 1 To cSubid.Count
            incollection = False
            For j = 1 To cUniqueSubids.Count
                If cUniqueSubids(j) = cSubid(i) Then
                    incollection = True
                    Exit For
                End If
            Next j
            If Not incollection Then
                cUniqueSubids.Add(cSubid(i))
            End If
        Next i

        'build collection of unique landuse groups
        Dim cUniqueLugroups As New Collection
        For i = 1 To AtcGridPervious.Source.Rows
            incollection = False
            For j = 1 To cUniqueLugroups.Count
                If cUniqueLugroups(j) = AtcGridPervious.Source.CellValue(i, 1) Then
                    incollection = True
                    Exit For
                End If
            Next j
            If Not incollection Then
                cUniqueLugroups.Add(AtcGridPervious.Source.CellValue(i, 1))
            End If
        Next i

        ReDim PerArea(cUniqueSubids.Count, cUniqueLugroups.Count)
        ReDim ImpArea(cUniqueSubids.Count, cUniqueLugroups.Count)
        ReDim length(cUniqueSubids.Count)
        ReDim slope(cUniqueSubids.Count)

        'loop through each polygon (or grid subid/lucode combination)
        'and populate array with area values
        If UseSimpleGrid Then
            For i = 1 To cSubid.Count

                'find subbasin position in the area array
                For j = 1 To cUniqueSubids.Count
                    If cSubid(i) = cUniqueSubids(j) Then
                        spos = j
                        Exit For
                    End If
                Next j
                'find lugroup that corresponds to this lucode
                For j = 1 To cRcode.Count
                    If cLucode(i) = cRcode(j) Then
                        luname = cRname(j)
                        Exit For
                    End If
                Next j
                'find percent perv that corresponds to this lugroup
                For j = 1 To AtcGridPervious.Source.Rows
                    If luname = AtcGridPervious.Source.CellValue(j, 1) Then
                        percentimperv = AtcGridPervious.Source.CellValue(j, 2)
                        Exit For
                    End If
                Next j
                'find lugroup position in the area array
                For j = 1 To cUniqueLugroups.Count
                    If luname = cUniqueLugroups(j) Then
                        lpos = j
                        Exit For
                    End If
                Next j

                PerArea(spos, lpos) = PerArea(spos, lpos) + (cArea(i) * (100 - percentimperv) / 100)
                ImpArea(spos, lpos) = ImpArea(spos, lpos) + (cArea(i) * percentimperv / 100)
                length(spos) = 0.0
                slope(spos) = cSubslope(i) / 100.0

            Next i

        Else
            'using custom table for landuse classification
            For i = 1 To cSubid.Count
                'loop through each polygon (or grid subid/lucode combination)

                'find subbasin position in the area array
                For j = 1 To cUniqueSubids.Count
                    If cSubid(i) = cUniqueSubids(j) Then
                        spos = j
                        Exit For
                    End If
                Next j

                'find lugroup that corresponds to this lucode, could be multiple matches
                For j = 1 To AtcGridPervious.Source.Rows
                    luname = ""
                    lpos = -1
                    If AtcGridPervious.Source.CellValue(j, 0) <> "" Then
                        If cLucode(i) = AtcGridPervious.Source.CellValue(j, 0) Then
                            'see if any of these are subbasin-specific
                            percentimperv = AtcGridPervious.Source.CellValue(j, 2)
                            If IsNumeric(AtcGridPervious.Source.CellValue(j, 3)) Then
                                multiplier = CSng(AtcGridPervious.Source.CellValue(j, 3))
                            Else
                                multiplier = 1.0
                            End If
                            subbasin = AtcGridPervious.Source.CellValue(j, 4)
                            If Len(subbasin) > 0 And subbasin <> "Invalid Field Number" Then
                                'this row is subbasin-specific
                                If subbasin = cSubid(i) Then
                                    'we want this one now
                                    luname = AtcGridPervious.Source.CellValue(j, 1)
                                End If
                            Else
                                'make sure that no other rows of this lucode are 
                                'subbasin-specific for this subbasin and that we 
                                'should therefore not use this row
                                useit = True
                                For k = 1 To AtcGridPervious.Source.Rows
                                    If k <> j Then
                                        If AtcGridPervious.Source.CellValue(k, 0) = AtcGridPervious.Source.CellValue(j, 0) Then
                                            'this other row has same lucode
                                            If AtcGridPervious.Source.CellValue(k, 1) = AtcGridPervious.Source.CellValue(j, 1) Then
                                                'and the same group name
                                                subbasin = AtcGridPervious.Source.CellValue(k, 4)
                                                If Len(subbasin) > 0 Then
                                                    'and its subbasin-specific
                                                    If subbasin = cSubid(i) Then
                                                        'and its specific to this subbasin
                                                        useit = False
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Next k
                                If useit Then
                                    'we want this one now
                                    luname = AtcGridPervious.Source.CellValue(j, 1)
                                End If
                            End If

                            If Len(luname) > 0 Then
                                'find lugroup position in the area array
                                For k = 1 To cUniqueLugroups.Count
                                    If luname = cUniqueLugroups(k) Then
                                        lpos = k
                                        Exit For
                                    End If
                                Next k
                            End If

                            If lpos > 0 Then
                                PerArea(spos, lpos) = PerArea(spos, lpos) + (cArea(i) * multiplier * (100 - percentimperv) / 100)
                                ImpArea(spos, lpos) = ImpArea(spos, lpos) + (cArea(i) * multiplier * percentimperv / 100)
                                length(spos) = 0.0 'were not computing lsur since winhspf does that
                                slope(spos) = cSubslope(i) / 100.0
                            End If

                        End If
                    End If
                Next j

            Next i
        End If

        OutFile = FreeFile()
        FileOpen(OutFile, WsdFileName, OpenMode.Output)
        WriteLine(OutFile, "LU Name", "Type (1=Impervious, 2=Pervious)", "Watershd-ID", "Area", "Slope", "Distance")

        'now write
        For i = 1 To cUniqueSubids.Count
            For j = 1 To cUniqueLugroups.Count
                stype = "2"
                tarea = PerArea(i, j) / 4046.8564
                If CInt(tarea) > 0 Then
                    PrintLine(OutFile, Chr(34) & cUniqueLugroups(j) & Chr(34), " " & stype & " ", cUniqueSubids(i), Format(tarea, "0."), Format(slope(i), "0.000000"), Format(length(i), "0.0000"))
                End If
                stype = "1"
                tarea = ImpArea(i, j) / 4046.8564
                If CInt(tarea) > 0 Then
                    PrintLine(OutFile, Chr(34) & cUniqueLugroups(j) & Chr(34), " " & stype & " ", cUniqueSubids(i), Format(tarea, "0."), Format(slope(i), "0.000000"), Format(length(i), "0.0000"))
                End If
            Next j
        Next i

        FileClose(OutFile)
    End Sub

    Private Sub WriteRCHFile(ByVal RchFileName$, ByVal cUniqueSubids As Collection)
        Dim OutFile As Integer, OutFile2 As Integer
        Dim PtfFileName As String
        Dim sname As String
        Dim cDOWN$, cLENGTH#, cDEPTH#, cWIDTH#, cSLOPE#, cMINEL#, cMAXEL#, cELEV#
        Dim LayerIndex As Long
        Dim StreamsIndex As Long
        Dim StreamsRIndex As Long
        Dim Len2Index As Long
        Dim Slo2Index As Long
        Dim Wid2Index As Long
        Dim Dep2Index As Long
        Dim MinelIndex As Long
        Dim MaxelIndex As Long
        Dim SnameIndex As Long
        Dim i As Long, j As Long

        OutFile = FreeFile()
        FileOpen(OutFile, RchFileName, OpenMode.Output)
        WriteLine(OutFile, "Rivrch", "Pname", "Watershed-ID", "HeadwaterFlag", _
                  "Exits", "Milept", "Stream/Resevoir Type", "Segl", _
                  "Delth", "Elev", "Ulcsm", "Urcsm", "Dscsm", "Ccsm", _
                  "Mnflow", "Mnvelo", "Svtnflow", "Svtnvelo", "Pslope", _
                  "Pdepth", "Pwidth", "Pmile", "Ptemp", "Pph", "Pk1", _
                  "Pk2", "Pk3", "Pmann", "Psod", "Pbgdo", _
                  "Pbgnh3", "Pbgbod5", "Pbgbod", "Level")

        OutFile2 = FreeFile()
        PtfFileName = Mid(RchFileName, 1, Len(RchFileName) - 3) & "ptf"
        FileOpen(OutFile2, PtfFileName, OpenMode.Output)
        WriteLine(OutFile2, "Reach Number", "Length(ft)", _
            "Mean Depth(ft)", "Mean Width (ft)", _
            "Mannings Roughness Coeff.", "Long. Slope", _
            "Type of x-section", "Side slope of upper FP left", _
            "Side slope of lower FP left", "Zero slope FP width left(ft)", _
            "Side slope of channel left", "Side slope of channel right", _
            "Zero slope FP width right(ft)", "Side slope lower FP right", _
            "Side slope upper FP right", "Channel Depth(ft)", _
            "Flood side slope change at depth", "Max. depth", _
            "No. of exits", "Fraction of flow through exit 1", _
            "Fraction of flow through exit 2", "Fraction of flow through exit 3", _
            "Fraction of flow through exit 4", "Fraction of flow through exit 5")

        'set field indexes
        LayerIndex = GisUtil.LayerIndex(cboStreams.Items(cboStreams.SelectedIndex))
        StreamsIndex = GisUtil.FieldIndex(LayerIndex, cboStream1.Items(cboStream1.SelectedIndex))
        StreamsRIndex = GisUtil.FieldIndex(LayerIndex, cboStream2.Items(cboStream2.SelectedIndex))
        Len2Index = GisUtil.FieldIndex(LayerIndex, cboStream3.Items(cboStream3.SelectedIndex))
        Slo2Index = GisUtil.FieldIndex(LayerIndex, cboStream4.Items(cboStream4.SelectedIndex))
        Wid2Index = GisUtil.FieldIndex(LayerIndex, cboStream5.Items(cboStream5.SelectedIndex))
        Dep2Index = GisUtil.FieldIndex(LayerIndex, cboStream6.Items(cboStream6.SelectedIndex))
        MinelIndex = GisUtil.FieldIndex(LayerIndex, cboStream7.Items(cboStream7.SelectedIndex))
        MaxelIndex = GisUtil.FieldIndex(LayerIndex, cboStream8.Items(cboStream8.SelectedIndex))
        SnameIndex = GisUtil.FieldIndex(LayerIndex, cboStream9.Items(cboStream9.SelectedIndex))

        For i = 1 To GisUtil.NumFeatures(LayerIndex)
            'is this subbasin in the list?
            For j = 1 To cUniqueSubids.Count
                GisUtil.FieldValue(LayerIndex, i - 1, StreamsIndex)
                If cUniqueSubids(j) = GisUtil.FieldValue(LayerIndex, i - 1, StreamsIndex) Then
                    'in list, output it
                    sname = GisUtil.FieldValue(LayerIndex, i - 1, SnameIndex)
                    If Len(Trim(sname)) = 0 Then
                        sname = "STREAM " + cUniqueSubids(j)
                    End If
                    cDOWN = GisUtil.FieldValue(LayerIndex, i - 1, StreamsRIndex)
                    If IsNumeric(GisUtil.FieldValue(LayerIndex, i - 1, Len2Index)) Then
                        cLENGTH = (CSng(GisUtil.FieldValue(LayerIndex, i - 1, Len2Index)) * 3.28) / 5280
                    Else
                        cLENGTH = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(LayerIndex, i - 1, Slo2Index)) Then
                        cSLOPE = CSng(GisUtil.FieldValue(LayerIndex, i - 1, Slo2Index)) / 100
                    Else
                        cSLOPE = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(LayerIndex, i - 1, Wid2Index)) Then
                        cWIDTH = CSng(GisUtil.FieldValue(LayerIndex, i - 1, Wid2Index)) * 3.28
                    Else
                        cWIDTH = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(LayerIndex, i - 1, Dep2Index)) Then
                        cDEPTH = CSng(GisUtil.FieldValue(LayerIndex, i - 1, Dep2Index)) * 3.28
                    Else
                        cDEPTH = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(LayerIndex, i - 1, MinelIndex)) Then
                        cMINEL = CSng(GisUtil.FieldValue(LayerIndex, i - 1, MinelIndex)) * 3.28
                    Else
                        cMINEL = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(LayerIndex, i - 1, MaxelIndex)) Then
                        cMAXEL = CSng(GisUtil.FieldValue(LayerIndex, i - 1, MaxelIndex)) * 3.28
                    Else
                        cMAXEL = 0.0#
                    End If
                    cELEV = ((cMAXEL + cMINEL) / 2)
                    PrintLine(OutFile, cUniqueSubids(j) & " " & Chr(34) & sname & Chr(34) & " " & cUniqueSubids(j) & " " & _
                           " 0 1 0 S " & Format(cLENGTH, "0.00") & " " & Format(Math.Abs(cMAXEL - cMINEL), "0.00") & " " & _
                           Format(cELEV, "0.") & " 0 0 " & cDOWN & " 0 0 0 0 0 " & _
                           Format(cSLOPE, "0.000000") & " " & Format(cDEPTH, "0.0000") & " " & Format(cWIDTH, "0.000") & _
                           " 0 0 0 0 0 0 0 0 0 0 0 0 0")
                    PrintLine(OutFile2, cUniqueSubids(j) & " " & Format(cLENGTH * 5280.0#, "0.") & " " & _
                           Format(cDEPTH, "0.00000") & " " & Format(cWIDTH, "0.00000") & " 0.05 " & _
                           Format(cSLOPE, "0.00000") & " " & "Trapezoidal" & " " & _
                           "0.5 0.5 " & Format(cWIDTH, "0.000") & " 1 1 " & Format(cWIDTH, "0.000") & _
                           " 0.5 0.5 " & Format(cDEPTH * 1.25, "0.0000") & " " & Format(cDEPTH * 1.875, "0.0000") & " " & _
                           Format(cDEPTH * 62.5, "0.000") & " 1 1 0 0 0 0")
                    If (2 * cDEPTH) > cWIDTH Then
                        'problem
                        MsgBox("The depth and width values specified for Reach " & cUniqueSubids(j) & ", coupled with the trapezoidal" & vbCrLf & _
                               "cross section assumptions of WinHSPF, indicate a physical imposibility." & vbCrLf & _
                               "(Given 1:1 side slopes, the depth of the channel cannot be more than half the width.)" & vbCrLf & vbCrLf & _
                               "This problem can be corrected in WinHSPF by revising the FTABLE or by " & vbCrLf & _
                               "importing the ptf with modifications to the width and depth values." & vbCrLf & _
                               "See the WinHSPF manual for more information.", vbOKOnly, "Channel Problem")
                    End If
                End If
            Next j
        Next i
        FileClose(OutFile)
        FileClose(OutFile2)
    End Sub

    Private Sub WritePSRFile(ByVal PsrFileName$, ByVal cUniqueSubids As Collection, ByVal cOutSubs As Collection)
        Dim OutFile As Integer
        Dim i As Integer, j As Long, k As Long
        Dim PointIndex As Long, LayerIndex As Long, pcsLayerIndex As Long
        Dim npdesIndex As Long, flowIndex As Long, cuIndex As Long, facIndex As Long
        Dim flow As Single
        Dim facname As String
        Dim huc As String
        Dim mipt As Single
        Dim dbffilename As String
        Dim dbname As String = ""
        Dim lnpdes As Object
        Dim ctemp As String
        Dim tmpDbf As IatcTable
        Dim ParmCode(0) As String, ParmName(0) As String
        Dim RowCount As Long = 0
        Dim prevdbf As String
        Dim YearField As Long, ParmField As Long
        Dim LoadField As Long, NPDESField As Long
        Dim dbrcount As Long
        Dim TableYear(0) As String
        Dim TableParm(0) As String
        Dim TableLoad(0) As String
        Dim TableNPDES(0) As String
        Dim tPoll As String
        Dim tValue As String
        Dim iFound As Boolean

        OutFile = FreeFile()
        FileOpen(OutFile, PsrFileName, OpenMode.Output)

        Dim cNPDES As New Collection
        Dim cSubbasin As New Collection
        Dim cFlow As New Collection
        Dim cMipt As New Collection
        Dim cFacName As New Collection
        Dim cHuc As New Collection

        If cOutSubs.Count > 0 Then

            'set field indexes
            LayerIndex = GisUtil.LayerIndex(cboOutlets.Items(cboOutlets.SelectedIndex))
            PointIndex = GisUtil.FieldIndex(LayerIndex, cboPoint.Items(cboPoint.SelectedIndex))

            'build collection of npdes sites to output
            For i = 1 To cOutSubs.Count
                For j = 1 To cUniqueSubids.Count
                    If cOutSubs(i) = cUniqueSubids(j) Then
                        'found this subbasin in selected list
                        If Len(GisUtil.FieldValue(LayerIndex, i - 1, PointIndex)) > 0 Then
                            cNPDES.Add(GisUtil.FieldValue(LayerIndex, i - 1, PointIndex))
                            cSubbasin.Add(cOutSubs(i))
                        End If
                    End If
                Next j
            Next i

            'If cNPDES.Count = 0 Then
            '  MsgBox("No point sources have been added to the outlets layer." & vbCrLf & _
            '  "To add point sources, update the outlets layer using the" & vbCrLf & _
            '  "BASINS watershed delineator or update it manually.", vbOKOnly, "BASINS HSPF Information")
            'End If

            If Not chkCustom.Checked Then
                'use pcs data
                If GisUtil.IsLayer("Permit Compliance System") Then
                    'set pcs shape file
                    pcsLayerIndex = GisUtil.LayerIndex("Permit Compliance System")
                    npdesIndex = GisUtil.FieldIndex(pcsLayerIndex, "NPDES")
                    flowIndex = GisUtil.FieldIndex(pcsLayerIndex, "FLOW_RATE")
                    facIndex = GisUtil.FieldIndex(pcsLayerIndex, "FAC_NAME")
                    cuIndex = GisUtil.FieldIndex(pcsLayerIndex, "BCU")
                    If npdesIndex > -1 Then
                        For i = 1 To cNPDES.Count
                            flow = 0.0#
                            facname = ""
                            huc = ""
                            mipt = 0.0#
                            If Len(Trim(cNPDES(i))) > 0 Then
                                For j = 1 To GisUtil.NumFeatures(pcsLayerIndex)
                                    If GisUtil.FieldValue(pcsLayerIndex, j - 1, npdesIndex) = cNPDES(i) Then
                                        'this is the one
                                        If IsNumeric(GisUtil.FieldValue(pcsLayerIndex, j - 1, flowIndex)) Then
                                            flow = GisUtil.FieldValue(pcsLayerIndex, j - 1, flowIndex) * 1.55
                                        Else
                                            flow = 0.0
                                        End If
                                        facname = GisUtil.FieldValue(pcsLayerIndex, j - 1, facIndex)
                                        If chkCalculate.Checked Then
                                            'calculate mile point on stream
                                            'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), FilenameOnly(OutletsJoinThemeName), PCSIdField, pNPDES(j))
                                            'mipt = dist / 1609.3
                                        Else
                                            mipt = 0.0#
                                        End If
                                        huc = GisUtil.FieldValue(pcsLayerIndex, j - 1, cuIndex)
                                        Exit For
                                    End If
                                Next j
                            End If
                            cFlow.Add(flow)
                            cMipt.Add(mipt)
                            cFacName.Add(facname)
                            cHuc.Add(huc)
                        Next i
                    End If
                    'check for dbf associated with each npdes point
                    i = 1
                    dbname = PathNameOnly(GisUtil.LayerFileName(pcsLayerIndex)) & "\pcs\"
                    For Each lnpdes In cNPDES
                        dbffilename = Trim(cHuc(i)) & ".dbf"
                        If Len(Dir(dbname & dbffilename)) > 0 And Len(Trim(lnpdes)) > 0 Then
                            'yes, it exists
                            i = i + 1
                        Else
                            'remove from collection
                            cNPDES.Remove(i)
                            cSubbasin.Remove(i)
                            cFlow.Remove(i)
                            cMipt.Remove(i)
                            cFacName.Remove(i)
                            cHuc.Remove(i)
                        End If
                    Next lnpdes
                Else
                    'no pcs layer, clear out
                    Do While cNPDES.Count > 0
                        cNPDES.Remove(1)
                    Loop
                End If

            Else
                'using custom table
                'must have these fields in this order:
                'pcsid (same as outlets layer)
                'facname
                'load (flow or other value) lbs/yr or cfs
                'parm (flow or other name)
                tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lblCustom.Text)

                i = 1
                Do While i <= cNPDES.Count
                    If chkCalculate.Checked Then
                        'calculate mile point on stream
                        'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), FilenameOnly(OutletsJoinThemeName), PCSIdField, pNPDES(j))
                        'mipt = dist / 1609.3
                    Else
                        mipt = 0.0#
                    End If
                    cMipt.Add(mipt)
                    iFound = False
                    For j = 1 To tmpDbf.NumRecords
                        tmpDbf.CurrentRecord = j
                        If cNPDES(i) = tmpDbf.Value(1) Then
                            cFacName.Add(tmpDbf.Value(2))
                            iFound = True
                            Exit For
                        End If
                    Next j
                    If Not iFound Then
                        cNPDES.Remove(i)
                        cSubbasin.Remove(i)
                        cMipt.Remove(i)
                    Else
                        i = i + 1
                    End If
                Loop
            End If
        End If

        'write first part of point source file
        PrintLine(OutFile, " " & CStr(cNPDES.Count))
        PrintLine(OutFile, " ")
        WriteLine(OutFile, "Facility Name", "Npdes", "Cuseg", "Mi")
        For i = 1 To cNPDES.Count
            ctemp = Chr(34) & cFacName(i) & Chr(34) & " " & cNPDES(i) & " " & cSubbasin(i) & " " & Format(cMipt(i), "0.000000")
            PrintLine(OutFile, ctemp)
        Next i

        If Not chkCustom.Checked Then
            'read in Permitted Discharges Parameter Table
            If cNPDES.Count > 0 Then
                'open dbf file
                tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(PathNameOnly(GisUtil.LayerFileName(pcsLayerIndex)) & "\pcs3_prm.dbf")
                RowCount = tmpDbf.NumRecords
                ReDim ParmCode(RowCount)
                ReDim ParmName(RowCount)
                For i = 1 To RowCount
                    tmpDbf.CurrentRecord = i
                    ParmCode(i) = tmpDbf.Value(1)
                    ParmName(i) = tmpDbf.Value(2)
                Next i
            End If
        End If

        PrintLine(OutFile, " ")
        WriteLine(OutFile, "Ordinal Number", "Pollutant", "Load (lbs/hr)")
        If Not chkCustom.Checked Then
            'using pcs data
            prevdbf = ""
            For j = 1 To cNPDES.Count
                'open dbf file
                dbffilename = dbname & Trim(cHuc(j)) & ".dbf"
                If Len(Dir(dbffilename)) > 0 Then
                    If dbffilename <> prevdbf Then
                        tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(dbffilename)
                        prevdbf = dbffilename
                        For k = 1 To tmpDbf.NumFields
                            If UCase(tmpDbf.FieldName(k)) = "YEAR" Then
                                YearField = k
                            End If
                            If UCase(tmpDbf.FieldName(k)) = "PARM" Then
                                ParmField = k
                            End If
                            If UCase(tmpDbf.FieldName(k)) = "LOAD" Then
                                LoadField = k
                            End If
                            If UCase(tmpDbf.FieldName(k)) = "NPDES" Then
                                NPDESField = k
                            End If
                        Next k
                        dbrcount = tmpDbf.NumRecords
                        ReDim TableYear(dbrcount)
                        ReDim TableParm(dbrcount)
                        ReDim TableLoad(dbrcount)
                        ReDim TableNPDES(dbrcount)
                        For k = 1 To dbrcount
                            tmpDbf.CurrentRecord = k
                            TableYear(k) = tmpDbf.Value(YearField)
                            TableParm(k) = tmpDbf.Value(ParmField)
                            TableLoad(k) = tmpDbf.Value(LoadField)
                            TableNPDES(k) = tmpDbf.Value(NPDESField)
                        Next k
                    End If
                    For k = 1 To dbrcount
                        If TableNPDES(k) = cNPDES(j) And TableYear(k) = cboYear.Items(cboYear.SelectedIndex) Then
                            'found one, output it
                            tPoll = ""
                            For i = 0 To RowCount - 1
                                If TableParm(k) = ParmCode(i) Then
                                    tPoll = ParmName(i)
                                    Exit For
                                End If
                            Next i
                            tValue = TableLoad(k) / 8760 'lbs/hr
                            ctemp = CStr(j - 1) & " " & Chr(34) & Trim(tPoll) & Chr(34) & " " & Format(CSng(tValue), "0.000000")
                            PrintLine(OutFile, ctemp)
                        End If
                    Next k
                End If
            Next j
            'now output flows
            For j = 1 To cNPDES.Count
                ctemp = CStr(j - 1) & " Flow " & Format(cFlow(j), "0.000000")
                PrintLine(OutFile, ctemp)
            Next j
        Else
            'using custom data
            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lblCustom.Text)
            For i = 1 To cNPDES.Count
                For j = 1 To tmpDbf.NumRecords
                    tmpDbf.CurrentRecord = j
                    If cNPDES(i) = tmpDbf.Value(1) Then
                        If UCase(tmpDbf.Value(4)) = "FLOW" Then
                            ctemp = CStr(i - 1) & " Flow " & Format(CStr(tmpDbf.Value(3)), "0.000000")
                        Else
                            tValue = CSng(tmpDbf.Value(3)) / 8760 'lbs/hr
                            ctemp = CStr(i - 1) & " " & Chr(34) & Trim(tmpDbf.Value(4)) & Chr(34) & " " & Format(CStr(tValue), "0.000000")
                        End If
                        PrintLine(OutFile, ctemp)
                    End If
                Next j
            Next i
        End If

        FileClose(OutFile)
    End Sub

    Private Sub WriteSEGFile(ByVal aSegFileName As String)

        Dim lOutFile As Integer = FreeFile()
        FileOpen(lOutFile, aSegFileName, OpenMode.Output)

        WriteLine(lOutFile, "SegID", "PrecWdmId", "PrecDsn", "PrecTstype", "PrecMFactPI", "PrecMFactR", _
                                     "AtemWdmId", "AtemDsn", "AtemTstype", "AtemMFactPI", "AtemMFactR", _
                                     "DewpWdmId", "DewpDsn", "DewpTstype", "DewpMFactPI", "DewpMFactR", _
                                     "WindWdmId", "WindDsn", "WindTstype", "WindMFactPI", "WindMFactR", _
                                     "SolrWdmId", "SolrDsn", "SolrTstype", "SolrMFactPI", "SolrMFactR", _
                                     "ClouWdmId", "ClouDsn", "ClouTstype", "ClouMFactPI", "ClouMFactR", _
                                     "PevtWdmId", "PevtDsn", "PevtTstype", "PevtMFactPI", "PevtMFactR")

        For Each lIndex As Integer In lstMet.SelectedIndices
            Dim lBaseDsn As Integer = pMetBaseDsns(lIndex)
            PrintLine(lOutFile, CStr(lIndex + 1) & " WDM2 " & CStr(lBaseDsn) & " PREC 1 1" & _
                                                   " WDM2 " & CStr(lBaseDsn + 2) & " ATEM 1 1" & _
                                                   " WDM2 " & CStr(lBaseDsn + 6) & " DEWP 1 1" & _
                                                   " WDM2 " & CStr(lBaseDsn + 3) & " WIND 1 1" & _
                                                   " WDM2 " & CStr(lBaseDsn + 4) & " SOLR 1 1" & _
                                                   " WDM2 " & CStr(lBaseDsn + 7) & " CLOU 0 1" & _
                                                   " WDM2 " & CStr(lBaseDsn + 5) & " PEVT 1 1")
        Next

        FileClose(lOutFile)

    End Sub

    Private Sub WriteMAPFile(ByVal aMapFileName As String)
        Dim lOutFile As Integer
        Dim i As Integer
        Dim lTemp As String

        lOutFile = FreeFile()
        FileOpen(lOutFile, aMapFileName, OpenMode.Output)

        lTemp = "EXT " & GisUtil.MapExtentXmin & " " & GisUtil.MapExtentYmax & " " & GisUtil.MapExtentXmax & " " & GisUtil.MapExtentYmin
        PrintLine(lOutFile, lTemp)

        For i = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(i) = 1 Or GisUtil.LayerType(i) = 2 Or GisUtil.LayerType(i) = 3 Then
                'shapefile
                lTemp = "LYR '" + GisUtil.LayerFileName(i) & "', " & GisUtil.LayerColor(i)
                If GisUtil.LayerType(i) = 3 Then
                    'polygon 
                    If Not GisUtil.LayerTransparent(i) Then
                        lTemp = lTemp & ",Style Transparent "
                    End If
                    lTemp = lTemp & ",Outline " & GisUtil.LayerOutlineColor(i)
                End If
                'hide the layers not turned on
                If Not GisUtil.LayerVisible(i) Then
                    lTemp = lTemp & ",Hide"
                End If
                'add theme name as caption
                lTemp = lTemp & ",Name '" & GisUtil.LayerName(i) & "'"
                PrintLine(lOutFile, lTemp)
            End If
        Next i
        FileClose(lOutFile)

    End Sub

    Private Sub chkCustom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustom.CheckedChanged
        If chkCustom.Checked Then
            If ofdCustom.ShowDialog() = Windows.Forms.DialogResult.OK Then
                lblCustom.Text = ofdCustom.FileName
                lblCustom.Visible = True
            Else
                lblCustom.Visible = False
            End If
        Else
            lblCustom.Visible = False
        End If
    End Sub

    Public Sub SetModelName(ByVal s As String)
        pModelName = s
        Me.Text = "BASINS " & pModelName
        If pModelName = "AQUATOX" Then
            TabControl1.TabPages.Remove(TabPage6)
            TabControl1.TabPages.Remove(TabPage5)
            TabControl1.TabPages.Remove(TabPage3)
            TabControl1.TabPages.Remove(TabPage2)
            Label1.Text = "AQUATOX Project Name:"
            Label2.Visible = False
            Label3.Visible = False
            Label5.Visible = False
            cboLanduse.Visible = False
            cboSubbasins.Visible = False
            cboOutlets.Visible = False
            cboMet.Visible = False
            Label4.Top = 88
            cboStreams.Top = 88
        End If
    End Sub

    Public Sub InitializeUI()
        Dim ctemp As String

        cboLanduse.Items.Add("USGS GIRAS Shapefile")
        cboLanduse.Items.Add("NLCD Grid")
        cboLanduse.Items.Add("Other Shapefile")
        cboLanduse.Items.Add("User Grid")
        cboLanduse.SelectedIndex = 0

        cboYear.Items.Add("1999")
        cboYear.Items.Add("1998")
        cboYear.Items.Add("1997")
        cboYear.Items.Add("1996")
        cboYear.Items.Add("1995")
        cboYear.Items.Add("1994")
        cboYear.Items.Add("1993")
        cboYear.Items.Add("1992")
        cboYear.Items.Add("1991")
        cboYear.SelectedIndex = 0

        cboOutlets.Items.Add("<none>")
        cboMet.Items.Add("<none>")

        Dim lyr As Long
        For lyr = 0 To GisUtil.NumLayers() - 1
            ctemp = GisUtil.LayerName(lyr)
            If GisUtil.LayerType(lyr) = 3 Then
                'PolygonShapefile 
                cboSubbasins.Items.Add(ctemp)
                If UCase(ctemp) = "SUBBASINS" Or InStr(ctemp, "Watershed Shapefile") > 0 Then
                    cboSubbasins.SelectedIndex = cboSubbasins.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lyr) = 2 Then
                'LineShapefile 
                cboStreams.Items.Add(ctemp)
                If UCase(ctemp) = "STREAMS" Or InStr(ctemp, "Stream Reach Shapefile") > 0 Then
                    cboStreams.SelectedIndex = cboStreams.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lyr) = 1 Then
                'PointShapefile
                cboOutlets.Items.Add(ctemp)
                If UCase(ctemp) = "OUTLETS" Then
                    cboOutlets.SelectedIndex = cboOutlets.Items.Count - 1
                End If
                cboMet.Items.Add(ctemp)
                If InStr(ctemp, "Weather Station Sites 20") > 0 Then
                    cboMet.SelectedIndex = cboMet.Items.Count - 1
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

        tbxName.Text = FilenameOnly(GisUtil.ProjectFileName)

        With AtcGridPervious
            .Source = New atcControls.atcGridSource
            .Font = New Font(.Font, FontStyle.Bold)
            .AllowHorizontalScrolling = False
        End With

        cboLanduse.SelectedIndex = 1
        cboLanduse.SelectedIndex = 0
    End Sub

    Private Sub cmdChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChange.Click
        If ofdClass.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblClass.Text = ofdClass.FileName
            SetPerviousGrid()
        End If
    End Sub

    Private Sub AtcGridPervious_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridPervious.CellEdited
        Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
        Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
        Dim lNewValueNumeric As Double = Double.NaN

        If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)

        Select Case aColumn
            Case 2 'Percent should be between 0 and 100
                If lNewValueNumeric >= 0 AndAlso lNewValueNumeric <= 100 Then
                    lNewColor = aGrid.CellBackColor
                Else
                    lNewColor = Color.Pink
                End If
        End Select

        If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
            aGrid.Source.CellColor(aRow, aColumn) = lNewColor
        End If
    End Sub

    Private Sub frmModelSetup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            If pModelName = "HSPF" Then
                ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
            ElseIf pModelName = "AQUATOX" Then
                ShowHelp("BASINS Details\Watershed and Instream Model Setup\AQUATOX.html")
            End If
        End If
    End Sub

    Private Function CreateUCI(ByVal aUciName As String, ByVal aMetWDMName As String) As Boolean
        CreateUCI = False

        ChDriveDir(PathNameOnly(aUciName))
        'get message file ready
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        'get starter uci ready
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lStarterUciName As String = "starter.uci"
        Dim lStarterPath As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "models\hspf\bin\starter\" & lStarterUciName
        If Not FileExists(lStarterPath) Then
            lStarterPath = "\basins\models\hspf\bin\starter\" & lStarterUciName
            If Not FileExists(lStarterPath) Then
                lStarterPath = FindFile("Please locate " & lStarterUciName, lStarterUciName)
            End If
        End If
        lStarterUciName = lStarterPath

        'location master pollutant list 
        Dim lPollutantListFileName As String = "poltnt_2.prn"
        Dim lPollutantListPath As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "models\hspf\bin\" & lPollutantListFileName
        If Not FileExists(lPollutantListPath) Then
            lPollutantListPath = "\basins\models\hspf\bin\" & lPollutantListFileName
            If Not FileExists(lPollutantListPath) Then
                lPollutantListPath = FindFile("Please locate " & lPollutantListFileName, lPollutantListFileName)
            End If
        End If
        lPollutantListFileName = lPollutantListPath

        'open project wdm
        Dim lDataSources As New Collection(Of atcData.atcDataSource)
        Dim lDataSource As New atcWDM.atcDataSourceWDM
        Dim lProjectWDMName As String = FilenameOnly(aUciName) & ".wdm"
        Dim lFound As Boolean = False
        For Each lBASINSDataSource As atcDataSource In atcDataManager.DataSources
            If lBASINSDataSource.Specification.ToUpper = lProjectWDMName.ToUpper Then
                'found it in the BASINS data sources
                lDataSource = lBASINSDataSource
                lFound = True
                Exit For
            End If
        Next
        If Not lFound Then
            If lDataSource.Open(lProjectWDMName) Then
                'need to open it here
                lFound = True
            End If
        End If
        lDataSources.Add(lDataSource)

        'open met wdm
        lDataSource = New atcWDM.atcDataSourceWDM
        lFound = False
        For Each lBASINSDataSource As atcDataSource In atcDataManager.DataSources
            If lBASINSDataSource.Specification.ToUpper = aMetWDMName.ToUpper Then
                'found it in the BASINS data sources
                lDataSource = lBASINSDataSource
                lFound = True
                Exit For
            End If
        Next
        If Not lFound Then
            If lDataSource.Open(aMetWDMName) Then
                'need to open it here
                lFound = True
            End If
        End If
        lDataSources.Add(lDataSource)

        Dim lWatershedName As String = FilenameOnly(aUciName)
        Dim lWatershed As New Watershed
        If lWatershed.Open(lWatershedName) = 0 Then  'everything read okay, continue
            Dim lHspfUci As New atcUCI.HspfUci
            lHspfUci.Msg = lMsg
            lHspfUci.CreateUciFromBASINS(lWatershed, _
                                         lDataSources, _
                                         lStarterUciName, lPollutantListFileName)
            lHspfUci.Save()
            Return True
        End If

    End Function

    Private Sub cboMet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMet.SelectedIndexChanged

        'fill in met wdm file name as appropriate
        Dim lMetLayerName As String = cboMet.Items(cboMet.SelectedIndex)
        If InStr(lMetLayerName, "Weather Station Sites 20") > 0 Then
            'new basins met
            Dim lMetWDMName As String = GisUtil.LayerFileName(GisUtil.LayerIndex(lMetLayerName))
            lMetWDMName = FilenameSetExt(lMetWDMName, "wdm")
            txtMetWDMName.Text = lMetWDMName
        ElseIf InStr(lMetLayerName, "WDM Weather") > 0 Then
            'old basins met 
            If GisUtil.IsLayer("State Boundaries") Then
                Dim lStateIndex As Integer = GisUtil.LayerIndex("State Boundaries")
                Dim lDefaultState As String = GisUtil.FieldValue(lStateIndex, 0, GisUtil.FieldIndex(lStateIndex, "ST"))
                Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
                Dim lDataPath As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "data\"
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
        BuildListofMetStationNames()
        lstMet.Items.Clear()
        For Each lMetStation As String In pMetStations
            lstMet.Items.Add(lMetStation)
        Next
        If lstMet.Items.Count > 0 Then
            lstMet.SelectedIndex = 0
        End If
    End Sub

    Private Function BuildListofMetStationNames() As Boolean
        pMetStations = New atcCollection
        pMetStations.Clear()
        pMetBaseDsns = New atcCollection
        pMetBaseDsns.Clear()
        Dim lDataSource As New atcWDM.atcDataSourceWDM
        If FileExists(txtMetWDMName.Text) Then
            Dim lFound As Boolean = False
            For Each lBASINSDataSource As atcDataSource In atcDataManager.DataSources
                If lBASINSDataSource.Specification.ToUpper = txtMetWDMName.Text.ToUpper Then
                    'found it in the BASINS data sources
                    lDataSource = lBASINSDataSource
                    lFound = True
                    Exit For
                End If
            Next
            If Not lFound Then
                If lDataSource.Open(txtMetWDMName.Text) Then
                    'need to open it here
                    lFound = True
                End If
            End If
            If lFound Then
                For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                    If lDataSet.Attributes.GetValue("Scenario") = "OBSERVED" And lDataSet.Attributes.GetValue("Constituent") = "PREC" Then
                        Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                        Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                        Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                        'get the common dates from prec and pevt at this location
                        Dim lSJDay As Double = lDataSet.Dates.Value(0)
                        Dim lEJDay As Double = lDataSet.Dates.Value(lDataSet.Dates.numValues)
                        'find pevt dataset at the same location
                        Dim lPevtFound As Boolean = False
                        For Each lDataSet2 As atcData.atcTimeseries In lDataSource.DataSets
                            If lDataSet2.Attributes.GetValue("Constituent") = "PEVT" And _
                               lDataSet2.Attributes.GetValue("Location") = lLoc Then
                                Dim lSJDay2 As Double = lDataSet2.Dates.Value(0)
                                Dim lEJDay2 As Double = lDataSet2.Dates.Value(lDataSet2.Dates.numValues)
                                If lSJDay2 > lSJDay Then lSJDay = lSJDay2
                                If lEJDay2 < lEJDay Then lEJDay = lEJDay2
                                lPevtFound = True
                                Exit For
                            End If
                        Next
                        If lPevtFound Then
                            Dim lLeadingChar As String = ""
                            If IsBASINSMetWDM(lDataSource.DataSets, lDsn, lLoc) Then
                                'full set available here
                                lLeadingChar = "*"
                            End If
                            Dim lSdate(6) As Integer
                            Dim lEdate(6) As Integer
                            J2Date(lSJDay, lSdate)
                            J2Date(lEJDay, lEdate)
                            Dim lDateString As String = "(" & lSdate(0) & "/" & lSdate(1) & "/" & lSdate(2) & "-" & lEdate(0) & "/" & lEdate(1) & "/" & lEdate(2) & ")"
                            pMetStations.Add(lLeadingChar & lLoc & ":" & lStanam & " " & lDateString)
                            pMetBaseDsns.Add(lDsn)
                        End If
                    End If
                Next
            End If
        End If
        Return True
    End Function

    Private Function IsBASINSMetWDM(ByVal aDataSets As atcData.atcDataGroup, ByVal aBaseDsn As Integer, ByVal aLoc As String) As Boolean
        Dim lCheckCount As Integer = 0
        For Each lDataSet As atcData.atcTimeseries In aDataSets
            If lDataSet.Attributes.GetValue("Location") = aLoc Then
                If lDataSet.Attributes.GetValue("ID") = aBaseDsn Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "PREC" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 2 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "ATEM" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 3 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "WIND" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 4 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "SOLR" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 5 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "PEVT" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 6 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "DEWP" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 7 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "CLOU" Then
                        lCheckCount += 1
                    End If
                End If
            End If
        Next

        If lCheckCount = 7 Then
            Return True
        Else
            Return False
        End If

    End Function
End Class