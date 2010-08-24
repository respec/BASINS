Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Drawing
Imports atcData
Imports atcSegmentation

Public Class frmModelSetup
    Inherits System.Windows.Forms.Form

    Friend pModelName As String
    Friend WithEvents lstMet As System.Windows.Forms.ListBox
    Friend pMetStations As atcCollection
    Friend WithEvents AtcGridMet As atcControls.atcGrid
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend pMetBaseDsns As atcCollection
    Friend pMetWdmNames As atcCollection
    Friend pUniqueModelSegmentIds As atcCollection
    Friend pUniqueModelSegmentNames As atcCollection

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
    Friend WithEvents cboSub3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents ofdMetWDM As System.Windows.Forms.OpenFileDialog
    Friend WithEvents AtcGridPervious As atcControls.atcGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModelSetup))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
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
        Me.Label22 = New System.Windows.Forms.Label
        Me.AtcGridMet = New atcControls.atcGrid
        Me.lstMet = New System.Windows.Forms.ListBox
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
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage6.SuspendLayout()
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
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        'cboOutlets
        '
        Me.cboOutlets.AllowDrop = True
        Me.cboOutlets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboOutlets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOutlets.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOutlets.Location = New System.Drawing.Point(168, 231)
        Me.cboOutlets.Name = "cboOutlets"
        Me.cboOutlets.Size = New System.Drawing.Size(339, 25)
        Me.cboOutlets.TabIndex = 10
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
        Me.cboStreams.Size = New System.Drawing.Size(339, 25)
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
        Me.cboSubbasins.Size = New System.Drawing.Size(339, 25)
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
        Me.cboLanduse.Size = New System.Drawing.Size(339, 25)
        Me.cboLanduse.TabIndex = 7
        '
        'tbxName
        '
        Me.tbxName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbxName.Location = New System.Drawing.Point(168, 40)
        Me.tbxName.Name = "tbxName"
        Me.tbxName.Size = New System.Drawing.Size(173, 23)
        Me.tbxName.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(11, 234)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(140, 17)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Point Sources Layer:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(11, 187)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(104, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Streams Layer:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(11, 140)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Subbasins Layer:"
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
        Me.AtcGridPervious.Fixed3D = False
        Me.AtcGridPervious.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.cboLandUseLayer.Size = New System.Drawing.Size(339, 25)
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
        Me.cboStream9.Size = New System.Drawing.Size(168, 25)
        Me.cboStream9.TabIndex = 17
        '
        'cboStream8
        '
        Me.cboStream8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream8.Location = New System.Drawing.Point(248, 256)
        Me.cboStream8.Name = "cboStream8"
        Me.cboStream8.Size = New System.Drawing.Size(168, 25)
        Me.cboStream8.TabIndex = 16
        '
        'cboStream7
        '
        Me.cboStream7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream7.Location = New System.Drawing.Point(248, 224)
        Me.cboStream7.Name = "cboStream7"
        Me.cboStream7.Size = New System.Drawing.Size(168, 25)
        Me.cboStream7.TabIndex = 15
        '
        'cboStream6
        '
        Me.cboStream6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream6.Location = New System.Drawing.Point(248, 192)
        Me.cboStream6.Name = "cboStream6"
        Me.cboStream6.Size = New System.Drawing.Size(168, 25)
        Me.cboStream6.TabIndex = 14
        '
        'cboStream5
        '
        Me.cboStream5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream5.Location = New System.Drawing.Point(248, 160)
        Me.cboStream5.Name = "cboStream5"
        Me.cboStream5.Size = New System.Drawing.Size(168, 25)
        Me.cboStream5.TabIndex = 13
        '
        'cboStream4
        '
        Me.cboStream4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream4.Location = New System.Drawing.Point(248, 128)
        Me.cboStream4.Name = "cboStream4"
        Me.cboStream4.Size = New System.Drawing.Size(168, 25)
        Me.cboStream4.TabIndex = 12
        '
        'cboStream3
        '
        Me.cboStream3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream3.Location = New System.Drawing.Point(248, 96)
        Me.cboStream3.Name = "cboStream3"
        Me.cboStream3.Size = New System.Drawing.Size(168, 25)
        Me.cboStream3.TabIndex = 11
        '
        'cboStream2
        '
        Me.cboStream2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream2.Location = New System.Drawing.Point(248, 63)
        Me.cboStream2.Name = "cboStream2"
        Me.cboStream2.Size = New System.Drawing.Size(168, 25)
        Me.cboStream2.TabIndex = 10
        '
        'cboStream1
        '
        Me.cboStream1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStream1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStream1.Location = New System.Drawing.Point(248, 32)
        Me.cboStream1.Name = "cboStream1"
        Me.cboStream1.Size = New System.Drawing.Size(168, 25)
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
        Me.cboSub3.Size = New System.Drawing.Size(168, 25)
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
        Me.cboSub2.Size = New System.Drawing.Size(168, 25)
        Me.cboSub2.TabIndex = 4
        '
        'cboSub1
        '
        Me.cboSub1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub1.Location = New System.Drawing.Point(248, 32)
        Me.cboSub1.Name = "cboSub1"
        Me.cboSub1.Size = New System.Drawing.Size(168, 25)
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
        Me.cboPoint.Size = New System.Drawing.Size(167, 25)
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
        Me.cboYear.Size = New System.Drawing.Size(120, 25)
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
        Me.TabPage6.Controls.Add(Me.Label22)
        Me.TabPage6.Controls.Add(Me.AtcGridMet)
        Me.TabPage6.Controls.Add(Me.lstMet)
        Me.TabPage6.Location = New System.Drawing.Point(4, 25)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(519, 355)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Met Stations"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'Label22
        '
        Me.Label22.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(370, 328)
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
        Me.AtcGridMet.Fixed3D = False
        Me.AtcGridMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridMet.LineColor = System.Drawing.Color.Empty
        Me.AtcGridMet.LineWidth = 0.0!
        Me.AtcGridMet.Location = New System.Drawing.Point(21, 97)
        Me.AtcGridMet.Name = "AtcGridMet"
        Me.AtcGridMet.Size = New System.Drawing.Size(474, 221)
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
        Me.lstMet.Size = New System.Drawing.Size(474, 225)
        Me.lstMet.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblStatus)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(16, 472)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 32)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "OK"
        '
        'cmdExisting
        '
        Me.cmdExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdExisting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(224, 472)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(88, 32)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Cancel"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(368, 472)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(79, 32)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.TabPage6.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboSubbasins_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubbasins.SelectedIndexChanged

        cboSub1.Items.Clear()
        cboSub2.Items.Clear()
        cboSub3.Items.Clear()
        cboSub3.Items.Add("<none>")

        Dim lLayerIndex As Integer = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
        If lLayerIndex > -1 Then 'fill in fields 
            'need to know all before any actually change
            Dim lCboSub1SelectedIndex As Integer = -1
            Dim lCboSub2SelectedIndex As Integer = -1
            Dim lCboSub3SelectedIndex As Integer = 0
            For lFieldIndex As Integer = 0 To GisUtil.NumFields(lLayerIndex) - 1
                Dim lFieldName As String = GisUtil.FieldName(lFieldIndex, lLayerIndex)
                cboSub1.Items.Add(lFieldName)
                cboSub2.Items.Add(lFieldName)
                cboSub3.Items.Add(lFieldName)
                Dim lFieldNameUpper As String = lFieldName.ToUpper
                If lFieldNameUpper = "SUBBASIN" Or lFieldNameUpper = "STREAMLINK" Then
                    lCboSub1SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "SLO1" Or lFieldNameUpper = "SLOPE" Or lFieldNameUpper = "AVESLOPE" Then
                    lCboSub2SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "MODELSEG" Then
                    lCboSub3SelectedIndex = lFieldIndex + 1
                End If
            Next
            cboSub1.SelectedIndex = lCboSub1SelectedIndex
            cboSub2.SelectedIndex = lCboSub2SelectedIndex
            cboSub3.SelectedIndex = lCboSub3SelectedIndex
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
        cboStream1.Items.Clear()
        cboStream2.Items.Clear()
        cboStream3.Items.Clear()
        cboStream4.Items.Clear()
        cboStream5.Items.Clear()
        cboStream6.Items.Clear()
        cboStream7.Items.Clear()
        cboStream8.Items.Clear()
        cboStream9.Items.Clear()
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(cboStreams.Items(cboStreams.SelectedIndex))
        If lLayerIndex > -1 Then 'fill in fields 
            For lFieldIndex As Integer = 0 To GisUtil.NumFields(lLayerIndex) - 1
                Dim lFieldName As String = GisUtil.FieldName(lFieldIndex, lLayerIndex)
                cboStream1.Items.Add(lFieldName)
                cboStream2.Items.Add(lFieldName)
                cboStream3.Items.Add(lFieldName)
                cboStream4.Items.Add(lFieldName)
                cboStream5.Items.Add(lFieldName)
                cboStream6.Items.Add(lFieldName)
                cboStream7.Items.Add(lFieldName)
                cboStream8.Items.Add(lFieldName)
                cboStream9.Items.Add(lFieldName)
                Dim lFieldNameUpper As String = lFieldName.ToUpper
                If lFieldNameUpper = "SUBBASIN" Or lFieldNameUpper = "LINKNO" Then
                    cboStream1.SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "SUBBASINR" Or lFieldNameUpper = "DSLINKNO" Then
                    cboStream2.SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "LEN2" Or lFieldNameUpper = "LENGTH" Then
                    cboStream3.SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "SLO2" Or lFieldNameUpper = "SLOPE" Then
                    cboStream4.SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "WID2" Or lFieldNameUpper = "WIDTH" Or lFieldNameUpper = "MEANWIDTH" Then
                    cboStream5.SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "DEP2" Or lFieldNameUpper = "DEPTH" Or lFieldNameUpper = "MEANDEPTH" Then
                    cboStream6.SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "MINEL" Or lFieldNameUpper = "ELEVLOW" Then
                    cboStream7.SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "MAXEL" Or lFieldNameUpper = "ELEVHIGH" Then
                    cboStream8.SelectedIndex = lFieldIndex
                End If
                If lFieldNameUpper = "SNAME" Then
                    cboStream9.SelectedIndex = lFieldIndex
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
        cboPoint.Items.Clear()
        If cboOutlets.Items(cboOutlets.SelectedIndex) <> "<none>" Then
            Dim lLayerIndex As Integer = GisUtil.LayerIndex(cboOutlets.Items(cboOutlets.SelectedIndex))
            'fill in fields 
            For lFieldIndex As Integer = 0 To GisUtil.NumFields(lLayerIndex) - 1
                Dim lFieldName As String = GisUtil.FieldName(lFieldIndex, lLayerIndex)
                cboPoint.Items.Add(lFieldName)
                If lFieldName.ToUpper = "PCSID" Then
                    cboPoint.SelectedIndex = lFieldIndex
                End If
            Next
            If cboPoint.Items.Count > 0 And cboPoint.SelectedIndex < 0 Then
                cboPoint.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        Dim lLandUseType As String = cboLanduse.Items(cboLanduse.SelectedIndex)
        Dim lLayerNameLandUse As String = ""
        If cboLandUseLayer.SelectedIndex > -1 Then
            lLayerNameLandUse = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
        End If
        Dim lFieldNameLandUse As String = ""
        If cboDescription.SelectedIndex > -1 Then
            lFieldNameLandUse = cboDescription.Items(cboDescription.SelectedIndex)
        End If

        If lLandUseType = "USGS GIRAS Shapefile" Then
            cboLandUseLayer.Visible = False
            lblLandUseLayer.Visible = False
            cboDescription.Visible = False
            lblDescription.Visible = False
            lblClass.Text = "/BASINS/etc/giras.dbf"
            SetPerviousGrid(AtcGridPervious, lblClass.Text, cboLanduse.SelectedIndex, lLayerNameLandUse, lFieldNameLandUse)
        ElseIf lLandUseType = "Other Shapefile" Then
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
            SetPerviousGrid(AtcGridPervious, lblClass.Text, cboLanduse.SelectedIndex, lLayerNameLandUse, lFieldNameLandUse)
        ElseIf lLandUseType = "NLCD Grid" Then
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
            SetPerviousGrid(AtcGridPervious, lblClass.Text, cboLanduse.SelectedIndex, lLayerNameLandUse, lFieldNameLandUse)
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
            SetPerviousGrid(AtcGridPervious, lblClass.Text, cboLanduse.SelectedIndex, lLayerNameLandUse, lFieldNameLandUse)
        End If
    End Sub

    Private Sub cboLandUseLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLandUseLayer.SelectedIndexChanged
        cboDescription.Items.Clear()
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex))
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
        Dim lLandUseType As String = cboLanduse.Items(cboLanduse.SelectedIndex)
        Dim lLayerNameLandUse As String = ""
        If cboLandUseLayer.SelectedIndex > -1 Then
            lLayerNameLandUse = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
        End If
        Dim lFieldNameLandUse As String = ""
        If cboDescription.SelectedIndex > -1 Then
            lFieldNameLandUse = cboDescription.Items(cboDescription.SelectedIndex)
        End If

        SetPerviousGrid(AtcGridPervious, lblClass.Text, cboLanduse.SelectedIndex, lLayerNameLandUse, lFieldNameLandUse)
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("BASINS " & pModelName & " for MapWindow" & vbCrLf & vbCrLf & "Version 1.1", MsgBoxStyle.OkOnly, "BASINS " & pModelName)
    End Sub

    Private Sub cmdExisting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExisting.Click
        If pModelName = "AQUATOX" Then
            StartAQUATOX("")
        ElseIf ofdExisting.ShowDialog() = Windows.Forms.DialogResult.OK Then
            StartWinHSPF(ofdExisting.FileName)
        End If
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lOutputPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "modelout\"
        Dim lProjectName As String = tbxName.Text.Trim
        If FileExists(lOutputPath) Then
            lOutputPath &= lProjectName
        Else
            Dim lDriveLetter As String = CurDir().Substring(0, 1)
            lOutputPath = lDriveLetter & ":\BASINS\modelout\" & lProjectName
        End If
        Dim lBaseOutputName As String = lProjectName

        If pModelName = "AQUATOX" Then
            If SetupAQUATOX(lOutputPath, lBaseOutputName) Then
                StartAQUATOX(" UNKNOWN " & lOutputPath)
            End If
        Else
            'TODO: make all the variables below part of a class to pass into SetupHSPF
            Dim lSubbasinLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
            Dim lLandUseLayerName As String = ""
            If cboLandUseLayer.SelectedIndex > -1 Then
                lLandUseLayerName = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
            End If
            Dim lLanduseFieldName As String = ""
            If cboDescription.SelectedIndex > -1 Then
                lLanduseFieldName = cboDescription.Items(cboDescription.SelectedIndex)
            End If
            Dim lSubbasinFieldName As String = cboSub1.Items(cboSub1.SelectedIndex)
            Dim lSubbasinSlopeName As String = cboSub2.Items(cboSub2.SelectedIndex)
            Dim lSubbasinSegmentName As String = cboSub3.Items(cboSub3.SelectedIndex)
            Dim lLUType As Integer = cboLanduse.SelectedIndex
            Dim lLandUseClassFile As String = lblClass.Text
            Dim lOutletsLayerName As String = cboOutlets.Items(cboOutlets.SelectedIndex)
            Dim lPointFieldName As String = ""
            If cboPoint.SelectedIndex > -1 Then
                lPointFieldName = cboPoint.Items(cboPoint.SelectedIndex)
            End If
            Dim lPointYear As String = cboYear.Items(cboYear.SelectedIndex)
            Dim lPSRCustom As Boolean = chkCustom.Checked
            Dim lPSRCustomFile As String = lblCustom.Text
            Dim lPSRCalculate As Boolean = chkCalculate.Checked
            Dim lWQConstituents() As String = {}
            Dim lLUInclude() As Integer = {} ' = {61, 71, 81, 91}

            Dim lStreamLayerName As String = cboStreams.Items(cboStreams.SelectedIndex)
            Dim lStreamFields() As String = {cboStream1.Items(cboStream1.SelectedIndex), _
                                             cboStream2.Items(cboStream2.SelectedIndex), _
                                             cboStream3.Items(cboStream3.SelectedIndex), _
                                             cboStream4.Items(cboStream4.SelectedIndex), _
                                             cboStream5.Items(cboStream5.SelectedIndex), _
                                             cboStream6.Items(cboStream6.SelectedIndex), _
                                             cboStream7.Items(cboStream7.SelectedIndex), _
                                             cboStream8.Items(cboStream8.SelectedIndex), _
                                             cboStream9.Items(cboStream9.SelectedIndex)}

            EnableControls(False)
            If PreProcessChecking(lOutputPath, lBaseOutputName, pModelName, cboLanduse.SelectedIndex, _
                                  pMetStations.Count, lSubbasinLayerName, lLandUseLayerName) Then 'early checks OK
                lblStatus.Text = "Preparing HSPF Setup"
                Me.Refresh()


                'Dim lMetWDM As String = ""    'used to be txtMetWDMName.Text,
                '                              'enhanced to handle multiple met wdm files 
                Dim lUniqueMetWDMNames As New atcCollection
                Dim lMetWdmNames As New atcCollection    'contains a value for each model segment
                Dim lMetBaseDsns As New atcCollection    'contains a value for each model segment
                If pUniqueModelSegmentIds.Count = 0 Then
                    'use a single met station
                    pUniqueModelSegmentIds.Add(1)
                    lMetWdmNames.Add(lMetWdmNames.Count, pMetWdmNames(lstMet.SelectedIndex))
                    lMetBaseDsns.Add(lMetBaseDsns.Count, pMetBaseDsns(lstMet.SelectedIndex))
                    lUniqueMetWDMNames.Add(pMetWdmNames(lstMet.SelectedIndex))
                Else
                    'use the specified segmentation scheme
                    For lRow As Integer = 1 To AtcGridMet.Source.Rows - 1
                        Dim lMetIndex As Integer = pMetStations.IndexFromKey(AtcGridMet.Source.CellValue(lRow, 1))
                        lMetWdmNames.Add(lMetWdmNames.Count, pMetWdmNames(lMetIndex))
                        lMetBaseDsns.Add(lMetBaseDsns.Count, pMetBaseDsns(lMetIndex))
                        lUniqueMetWDMNames.Add(pMetWdmNames(lMetIndex))
                    Next
                End If
                'assign appropriate wdm ids, should have WDM2, WDM3, etc.
                Dim lMetWdmIds As New atcCollection      'contains a value for each model segment
                For Each lFileName As String In lMetWdmNames
                    lMetWdmIds.Add(lMetWdmIds.Count, "WDM" & (lUniqueMetWDMNames.IndexOf(lFileName) + 2).ToString)
                Next


                If SetupHSPF(AtcGridPervious, _
                             lMetBaseDsns, lMetWdmIds, _
                             pUniqueModelSegmentNames, pUniqueModelSegmentIds, _
                             lOutputPath, lBaseOutputName, _
                             lSubbasinLayerName, lSubbasinFieldName, lSubbasinSlopeName, _
                             lStreamLayerName, lStreamFields, _
                             lLUType, lLandUseLayerName, lLUInclude, _
                             lOutletsLayerName, lPointFieldName, lPointYear, _
                             lLanduseFieldName, lLandUseClassFile, _
                             lSubbasinSegmentName, _
                             lPSRCustom, lPSRCustomFile, lPSRCalculate) Then
                    Me.Dispose()
                    Me.Close()

                    If CreateUCI(lOutputPath & "\" & lBaseOutputName & ".uci", _
                                 lUniqueMetWDMNames, _
                                 lWQConstituents) Then
                        lblStatus.Text = "Completed HSPF Setup"
                        Me.Refresh()
                        StartWinHSPF(lOutputPath & "\" & lBaseOutputName & ".uci")
                    Else
                        lblStatus.Text = "HSPF Setup Failed in CreateUCI"
                        Me.Refresh()
                    End If
                Else
                    lblStatus.Text = "HSPF Setup Failed"
                    Me.Refresh()
                End If
            Else
                lblStatus.Text = "HSPF Setup Failed in PreProcess Checking"
                Me.Refresh()
            End If
            EnableControls(True)
            Me.Refresh()
        End If
    End Sub

    Private Function SetupAQUATOX(ByVal aOutputPath As String, ByVal aBaseOutputName As String) As Boolean
        Try
            lblStatus.Text = "Preparing to process"
            Me.Refresh()
            EnableControls(False)

            Dim lSubbasinThemeName As String = ""
            If cboSubbasins.SelectedIndex > -1 Then
                lSubbasinThemeName = cboSubbasins.Items(cboSubbasins.SelectedIndex)
            End If
            Dim lLandUseThemeName As String = ""
            If cboLandUseLayer.SelectedIndex > -1 Then
                lLandUseThemeName = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
            End If

            If Not PreProcessChecking(aOutputPath, aBaseOutputName, pModelName, cboLanduse.SelectedIndex, _
                                  pMetStations.Count, lSubbasinThemeName, lLandUseThemeName) Then 'failed early checks
                EnableControls(True)
                Exit Function
            End If
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'are any streams selected?
            Dim lStreamsThemeName As String = cboStreams.Items(cboStreams.SelectedIndex)
            Dim lStreamsFieldName As String = cboStream1.Items(cboStream1.SelectedIndex)
            Dim lStreamsLayerIndex As Integer = GisUtil.LayerIndex(lStreamsThemeName)
            Dim lStreamsFieldIndex As Integer = GisUtil.FieldIndex(lStreamsLayerIndex, lStreamsFieldName)
            Dim lSelectedStreams As New atcCollection
            For i As Integer = 1 To GisUtil.NumSelectedFeatures(lStreamsLayerIndex)
                lSelectedStreams.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, lStreamsLayerIndex))  'zero based index
            Next
            If lSelectedStreams.Count <> 1 Then
                Logger.Msg("BASINS AQUATOX requires one and only one selected stream segment.", MsgBoxStyle.Critical, "BASINS AQUATOX Problem")
                EnableControls(True)
                Return False
            End If

            'get the id of the selected stream
            Dim lUniqueStreamIds As New atcCollection
            lUniqueStreamIds.Add(GisUtil.FieldValue(lStreamsLayerIndex, lSelectedStreams(0), lStreamsFieldIndex))
            Dim lModelSegmentIds As New atcCollection
            lModelSegmentIds.Add(1)

            'Create Reach Segments
            Dim lStreamsLayerName As String = cboStreams.Items(cboStreams.SelectedIndex)
            Dim lStreamFields() As String = {cboStream1.Items(cboStream1.SelectedIndex), _
                                             cboStream2.Items(cboStream2.SelectedIndex), _
                                             cboStream3.Items(cboStream3.SelectedIndex), _
                                             cboStream4.Items(cboStream4.SelectedIndex), _
                                             cboStream5.Items(cboStream5.SelectedIndex), _
                                             cboStream6.Items(cboStream6.SelectedIndex), _
                                             cboStream7.Items(cboStream7.SelectedIndex), _
                                             cboStream8.Items(cboStream8.SelectedIndex), _
                                             cboStream9.Items(cboStream9.SelectedIndex)}
            Dim lReaches As Reaches = CreateReachSegments(lUniqueStreamIds, lModelSegmentIds, lStreamsLayerName, lStreamFields)

            'Create Stream Channels
            Dim lChannels As Channels = CreateStreamChannels(lReaches)

            'make output folder
            MkDirPath(aOutputPath)
            Dim lBaseFileName As String = aOutputPath & "\" & aBaseOutputName

            'write rch file 
            lblStatus.Text = "Writing RCH file"
            Me.Refresh()
            WriteRCHFile(lBaseFileName & ".rch", lReaches)

            'write ptf file
            lblStatus.Text = "Writing PTF file"
            Me.Refresh()
            WritePTFFile(lBaseFileName & ".ptf", lChannels)

            'write psr file
            lblStatus.Text = "Writing PSR file"
            Me.Refresh()
            Dim lOutSubs As New Collection
            Dim lOutletsLayerIndex As Integer = GisUtil.LayerIndex(cboOutlets.Items(cboOutlets.SelectedIndex))
            Dim lPointLayerIndex As Integer = GisUtil.FieldIndex(lOutletsLayerIndex, cboPoint.Items(cboPoint.SelectedIndex))
            Dim lYear As String = cboYear.Items(cboYear.SelectedIndex)
            WritePSRFile(lBaseFileName & ".psr", lUniqueStreamIds, lOutSubs, lOutletsLayerIndex, lPointLayerIndex, _
                         chkCustom.Checked, lblCustom.Text, chkCalculate.Checked, lYear)

            'start aquatox
            lblStatus.Text = "Starting AQUATOX"
            Me.Refresh()
            Me.Dispose()
            Me.Close()

            Return True
        Catch
            Logger.Msg("An error occurred: " & Err.Description, vbOKOnly, "BASINS " & pModelName & " Error")
            Me.Dispose()
            Me.Close()
            Return False
        End Try
    End Function

    Private Sub EnableControls(ByVal aEnabled As Boolean)
        cmdOK.Enabled = aEnabled
        cmdExisting.Enabled = aEnabled
        cmdHelp.Enabled = aEnabled
        cmdCancel.Enabled = aEnabled
        cmdAbout.Enabled = aEnabled
        TabControl1.Enabled = aEnabled
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

    Public Sub SetModelName(ByVal aModelName As String)
        pModelName = aModelName
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
            Label4.Top = 88
            cboStreams.Top = 88
        End If
    End Sub

    Public Sub InitializeUI()
        pMetStations = New atcCollection
        pMetBaseDsns = New atcCollection
        pMetWdmNames = New atcCollection

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

        With AtcGridMet
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With

        For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 3 Then 'PolygonShapefile 
                cboSubbasins.Items.Add(lLayerName)
                If lLayerName.ToUpper = "SUBBASINS" Or lLayerName.IndexOf("Watershed Shapefile") > -1 Then
                    cboSubbasins.SelectedIndex = cboSubbasins.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = 2 Then 'LineShapefile 
                cboStreams.Items.Add(lLayerName)
                If lLayerName.ToUpper = "STREAMS" Or lLayerName.IndexOf("Stream Reach Shapefile") > -1 Then
                    cboStreams.SelectedIndex = cboStreams.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = 1 Then 'PointShapefile
                cboOutlets.Items.Add(lLayerName)
                If lLayerName.ToUpper = "OUTLETS" Then
                    cboOutlets.SelectedIndex = cboOutlets.Items.Count - 1
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

        tbxName.Text = IO.Path.GetFileNameWithoutExtension(GisUtil.ProjectFileName)

        With AtcGridPervious
            .Source = New atcControls.atcGridSource
            '.Font = New Font(.Font, FontStyle.Bold)
            .AllowHorizontalScrolling = False
        End With

        cboLanduse.SelectedIndex = 1
        cboLanduse.SelectedIndex = 0
    End Sub

    Public Sub InitializeMetStationList()

        'this takes some time, show window and then do this
        lblStatus.Text = "Reading Meteorologic WDM File..."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        BuildListofMetStationNames(pMetWdmNames, pMetStations, pMetBaseDsns)
        CheckAndSetMetSegmentGrid()

        lstMet.Items.Clear()
        For Each lMetStation As String In pMetStations
            lstMet.Items.Add(lMetStation)
        Next
        If lstMet.Items.Count > 0 Then
            lstMet.SelectedIndex = 0
        End If

        lblStatus.Text = "Update specifications if desired, then click OK to proceed."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub cmdChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChange.Click
        If ofdClass.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim lLandUseType As String = cboLanduse.Items(cboLanduse.SelectedIndex)
            Dim lLayerNameLandUse As String = ""
            If cboLandUseLayer.SelectedIndex > -1 Then
                lLayerNameLandUse = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
            End If
            Dim lFieldNameLandUse As String = ""
            If cboDescription.SelectedIndex > -1 Then
                lFieldNameLandUse = cboDescription.Items(cboDescription.SelectedIndex)
            End If

            lblClass.Text = ofdClass.FileName
            SetPerviousGrid(AtcGridPervious, lblClass.Text, cboLanduse.SelectedIndex, lLayerNameLandUse, lFieldNameLandUse)
        End If
    End Sub

    Private Sub AtcGridPervious_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridPervious.CellEdited
        Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
        Dim lNewValueNumeric As Double = GetNaN()
        If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)

        Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
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

    Private Sub cboSub1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSub1.SelectedIndexChanged
        CheckAndSetMetSegmentGrid()
    End Sub

    Private Sub cboSub3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSub3.SelectedIndexChanged
        CheckAndSetMetSegmentGrid()
    End Sub

    Private Sub CheckAndSetMetSegmentGrid()
        If cboSubbasins.SelectedIndex > -1 And cboSub1.SelectedIndex > -1 Then
            Dim lSubbasinsLayerName As String = cboSubbasins.Items(cboSubbasins.SelectedIndex)
            Dim lSubbasinsFieldName As String = cboSub1.Items(cboSub1.SelectedIndex)
            Dim lModelSegmentFieldName As String = ""
            If cboSub3.SelectedIndex > 0 Then
                lModelSegmentFieldName = cboSub3.Items(cboSub3.SelectedIndex)
            End If
            SetMetSegmentGrid(AtcGridMet, pMetStations, pUniqueModelSegmentNames, pUniqueModelSegmentIds, _
                              lSubbasinsLayerName, lSubbasinsFieldName, lModelSegmentFieldName)
            If pUniqueModelSegmentIds.Count = 0 Then
                lstMet.Visible = True
                AtcGridMet.Visible = False
            Else
                lstMet.Visible = False
                AtcGridMet.Visible = True
            End If
        End If
    End Sub
End Class