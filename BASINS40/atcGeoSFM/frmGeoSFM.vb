Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcData
Imports System.Drawing
Imports System
Imports System.Windows.Forms

Public Class frmGeoSFM
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
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdAbout As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents comboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents comboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ComboBox5 As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ComboBox4 As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents ComboBox13 As System.Windows.Forms.ComboBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents ComboBox12 As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents ComboBox11 As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents ComboBox10 As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ComboBox9 As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ComboBox8 As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ComboBox7 As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ComboBox6 As System.Windows.Forms.ComboBox
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents ComboBox14 As System.Windows.Forms.ComboBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents ComboBox15 As System.Windows.Forms.ComboBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents ComboBox16 As System.Windows.Forms.ComboBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents ComboBox17 As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents ComboBox18 As System.Windows.Forms.ComboBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents ComboBox19 As System.Windows.Forms.ComboBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents ComboBox20 As System.Windows.Forms.ComboBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents ComboBox21 As System.Windows.Forms.ComboBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents ComboBox22 As System.Windows.Forms.ComboBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents ComboBox23 As System.Windows.Forms.ComboBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents ComboBox24 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox25 As System.Windows.Forms.ComboBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents ComboBox26 As System.Windows.Forms.ComboBox
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents ComboBox43 As System.Windows.Forms.ComboBox
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents ComboBox42 As System.Windows.Forms.ComboBox
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents ComboBox41 As System.Windows.Forms.ComboBox
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents ComboBox40 As System.Windows.Forms.ComboBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents ComboBox27 As System.Windows.Forms.ComboBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents ComboBox28 As System.Windows.Forms.ComboBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents ComboBox29 As System.Windows.Forms.ComboBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents ComboBox30 As System.Windows.Forms.ComboBox
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents ComboBox31 As System.Windows.Forms.ComboBox
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents ComboBox32 As System.Windows.Forms.ComboBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents ComboBox33 As System.Windows.Forms.ComboBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents ComboBox34 As System.Windows.Forms.ComboBox
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents ComboBox35 As System.Windows.Forms.ComboBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents ComboBox36 As System.Windows.Forms.ComboBox
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents ComboBox37 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox38 As System.Windows.Forms.ComboBox
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents ComboBox39 As System.Windows.Forms.ComboBox
    Friend WithEvents AtcGridMannings As atcControls.atcGrid
    Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents AtcText1 As atcControls.atcText
    Friend WithEvents ComboBox46 As System.Windows.Forms.ComboBox
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents ComboBox44 As System.Windows.Forms.ComboBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents ComboBox45 As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents AtcText2 As atcControls.atcText
    Friend WithEvents AtcText3 As atcControls.atcText
    Friend WithEvents AtcText4 As atcControls.atcText
    Friend WithEvents AtcText5 As atcControls.atcText
    Friend WithEvents AtcText6 As atcControls.atcText
    Friend WithEvents AtcText7 As atcControls.atcText
    Friend WithEvents cboPrecipStation As System.Windows.Forms.ComboBox
    Friend WithEvents lblPrecipStation As System.Windows.Forms.Label
    Friend WithEvents cboOtherMet As System.Windows.Forms.ComboBox
    Friend WithEvents lblOtherMet As System.Windows.Forms.Label
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents AtcGrid1 As atcControls.atcGrid
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents AtcGrid2 As atcControls.atcGrid
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage11 As System.Windows.Forms.TabPage
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents TabPage10 As System.Windows.Forms.TabPage
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGeoSFM))
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage9 = New System.Windows.Forms.TabPage
        Me.Label53 = New System.Windows.Forms.Label
        Me.AtcText1 = New atcControls.atcText
        Me.ComboBox46 = New System.Windows.Forms.ComboBox
        Me.Label52 = New System.Windows.Forms.Label
        Me.ComboBox44 = New System.Windows.Forms.ComboBox
        Me.Label50 = New System.Windows.Forms.Label
        Me.Label51 = New System.Windows.Forms.Label
        Me.ComboBox45 = New System.Windows.Forms.ComboBox
        Me.TabPage7 = New System.Windows.Forms.TabPage
        Me.Label19 = New System.Windows.Forms.Label
        Me.ComboBox13 = New System.Windows.Forms.ComboBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.ComboBox12 = New System.Windows.Forms.ComboBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.ComboBox11 = New System.Windows.Forms.ComboBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.ComboBox10 = New System.Windows.Forms.ComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.ComboBox9 = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.ComboBox8 = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.ComboBox7 = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.ComboBox6 = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.ComboBox5 = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.ComboBox4 = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboBox3 = New System.Windows.Forms.ComboBox
        Me.comboBox1 = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.comboBox2 = New System.Windows.Forms.ComboBox
        Me.TabPage8 = New System.Windows.Forms.TabPage
        Me.AtcGridMannings = New atcControls.atcGrid
        Me.Label49 = New System.Windows.Forms.Label
        Me.ComboBox43 = New System.Windows.Forms.ComboBox
        Me.Label48 = New System.Windows.Forms.Label
        Me.ComboBox42 = New System.Windows.Forms.ComboBox
        Me.Label47 = New System.Windows.Forms.Label
        Me.ComboBox41 = New System.Windows.Forms.ComboBox
        Me.Label46 = New System.Windows.Forms.Label
        Me.ComboBox40 = New System.Windows.Forms.ComboBox
        Me.RadioButton3 = New System.Windows.Forms.RadioButton
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.Label5 = New System.Windows.Forms.Label
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label54 = New System.Windows.Forms.Label
        Me.Label55 = New System.Windows.Forms.Label
        Me.Label56 = New System.Windows.Forms.Label
        Me.Label57 = New System.Windows.Forms.Label
        Me.Label58 = New System.Windows.Forms.Label
        Me.AtcText2 = New atcControls.atcText
        Me.AtcText3 = New atcControls.atcText
        Me.AtcText4 = New atcControls.atcText
        Me.AtcText5 = New atcControls.atcText
        Me.AtcText6 = New atcControls.atcText
        Me.AtcText7 = New atcControls.atcText
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.ComboBox14 = New System.Windows.Forms.ComboBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.ComboBox15 = New System.Windows.Forms.ComboBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.ComboBox16 = New System.Windows.Forms.ComboBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.ComboBox17 = New System.Windows.Forms.ComboBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.ComboBox18 = New System.Windows.Forms.ComboBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.ComboBox19 = New System.Windows.Forms.ComboBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.ComboBox20 = New System.Windows.Forms.ComboBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.ComboBox21 = New System.Windows.Forms.ComboBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.ComboBox22 = New System.Windows.Forms.ComboBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.ComboBox23 = New System.Windows.Forms.ComboBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.ComboBox24 = New System.Windows.Forms.ComboBox
        Me.ComboBox25 = New System.Windows.Forms.ComboBox
        Me.Label31 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.ComboBox26 = New System.Windows.Forms.ComboBox
        Me.Label33 = New System.Windows.Forms.Label
        Me.ComboBox27 = New System.Windows.Forms.ComboBox
        Me.Label34 = New System.Windows.Forms.Label
        Me.ComboBox28 = New System.Windows.Forms.ComboBox
        Me.Label35 = New System.Windows.Forms.Label
        Me.ComboBox29 = New System.Windows.Forms.ComboBox
        Me.Label36 = New System.Windows.Forms.Label
        Me.ComboBox30 = New System.Windows.Forms.ComboBox
        Me.Label37 = New System.Windows.Forms.Label
        Me.ComboBox31 = New System.Windows.Forms.ComboBox
        Me.Label38 = New System.Windows.Forms.Label
        Me.ComboBox32 = New System.Windows.Forms.ComboBox
        Me.Label39 = New System.Windows.Forms.Label
        Me.ComboBox33 = New System.Windows.Forms.ComboBox
        Me.Label40 = New System.Windows.Forms.Label
        Me.ComboBox34 = New System.Windows.Forms.ComboBox
        Me.Label41 = New System.Windows.Forms.Label
        Me.ComboBox35 = New System.Windows.Forms.ComboBox
        Me.Label42 = New System.Windows.Forms.Label
        Me.ComboBox36 = New System.Windows.Forms.ComboBox
        Me.Label43 = New System.Windows.Forms.Label
        Me.ComboBox37 = New System.Windows.Forms.ComboBox
        Me.ComboBox38 = New System.Windows.Forms.ComboBox
        Me.Label44 = New System.Windows.Forms.Label
        Me.Label45 = New System.Windows.Forms.Label
        Me.ComboBox39 = New System.Windows.Forms.ComboBox
        Me.cboPrecipStation = New System.Windows.Forms.ComboBox
        Me.lblPrecipStation = New System.Windows.Forms.Label
        Me.cboOtherMet = New System.Windows.Forms.ComboBox
        Me.lblOtherMet = New System.Windows.Forms.Label
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.AtcGrid1 = New atcControls.atcGrid
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.AtcGrid2 = New atcControls.atcGrid
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.TabPage10 = New System.Windows.Forms.TabPage
        Me.TabPage11 = New System.Windows.Forms.TabPage
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.Button6 = New System.Windows.Forms.Button
        Me.Button7 = New System.Windows.Forms.Button
        Me.Button8 = New System.Windows.Forms.Button
        Me.Button9 = New System.Windows.Forms.Button
        Me.Button10 = New System.Windows.Forms.Button
        Me.Button11 = New System.Windows.Forms.Button
        Me.Button12 = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage9.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.TabPage10.SuspendLayout()
        Me.TabPage11.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(449, 529)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(73, 28)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Close"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(528, 529)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(65, 28)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(599, 529)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(72, 28)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "About"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage9)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Controls.Add(Me.TabPage8)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage10)
        Me.TabControl1.Controls.Add(Me.TabPage11)
        Me.TabControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(60, 21)
        Me.TabControl1.Location = New System.Drawing.Point(15, 15)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(655, 446)
        Me.TabControl1.TabIndex = 8
        '
        'TabPage9
        '
        Me.TabPage9.Controls.Add(Me.Button1)
        Me.TabPage9.Controls.Add(Me.Label53)
        Me.TabPage9.Controls.Add(Me.AtcText1)
        Me.TabPage9.Controls.Add(Me.ComboBox46)
        Me.TabPage9.Controls.Add(Me.Label52)
        Me.TabPage9.Controls.Add(Me.ComboBox44)
        Me.TabPage9.Controls.Add(Me.Label50)
        Me.TabPage9.Controls.Add(Me.Label51)
        Me.TabPage9.Controls.Add(Me.ComboBox45)
        Me.TabPage9.Location = New System.Drawing.Point(4, 46)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Size = New System.Drawing.Size(647, 396)
        Me.TabPage9.TabIndex = 10
        Me.TabPage9.Text = "Terrain Analysis"
        Me.TabPage9.UseVisualStyleBackColor = True
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Location = New System.Drawing.Point(33, 135)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(149, 13)
        Me.Label53.TabIndex = 38
        Me.Label53.Text = "Stream Delineation Threshold:"
        '
        'AtcText1
        '
        Me.AtcText1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText1.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText1.DefaultValue = ""
        Me.AtcText1.HardMax = 9999
        Me.AtcText1.HardMin = 0
        Me.AtcText1.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText1.Location = New System.Drawing.Point(189, 132)
        Me.AtcText1.MaxWidth = 20
        Me.AtcText1.Name = "AtcText1"
        Me.AtcText1.NumericFormat = "0"
        Me.AtcText1.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText1.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText1.SelLength = 0
        Me.AtcText1.SelStart = 0
        Me.AtcText1.Size = New System.Drawing.Size(64, 21)
        Me.AtcText1.SoftMax = -999
        Me.AtcText1.SoftMin = -999
        Me.AtcText1.TabIndex = 37
        Me.AtcText1.ValueDouble = 1000
        Me.AtcText1.ValueInteger = 1000
        '
        'ComboBox46
        '
        Me.ComboBox46.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox46.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox46.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox46.Location = New System.Drawing.Point(121, 54)
        Me.ComboBox46.Name = "ComboBox46"
        Me.ComboBox46.Size = New System.Drawing.Size(403, 21)
        Me.ComboBox46.TabIndex = 31
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label52.Location = New System.Drawing.Point(33, 57)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(82, 13)
        Me.Label52.TabIndex = 30
        Me.Label52.Text = "River Shapefile:"
        Me.Label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox44
        '
        Me.ComboBox44.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox44.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox44.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox44.Location = New System.Drawing.Point(121, 27)
        Me.ComboBox44.Name = "ComboBox44"
        Me.ComboBox44.Size = New System.Drawing.Size(403, 21)
        Me.ComboBox44.TabIndex = 29
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.Location = New System.Drawing.Point(33, 30)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(83, 13)
        Me.Label50.TabIndex = 28
        Me.Label50.Text = "Basin Shapefile:"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label51.Location = New System.Drawing.Point(33, 84)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(56, 13)
        Me.Label51.TabIndex = 26
        Me.Label51.Text = "DEM Grid:"
        Me.Label51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox45
        '
        Me.ComboBox45.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox45.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox45.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox45.Location = New System.Drawing.Point(121, 81)
        Me.ComboBox45.Name = "ComboBox45"
        Me.ComboBox45.Size = New System.Drawing.Size(403, 21)
        Me.ComboBox45.TabIndex = 27
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.Button2)
        Me.TabPage7.Controls.Add(Me.Label19)
        Me.TabPage7.Controls.Add(Me.ComboBox13)
        Me.TabPage7.Controls.Add(Me.Label18)
        Me.TabPage7.Controls.Add(Me.ComboBox12)
        Me.TabPage7.Controls.Add(Me.Label17)
        Me.TabPage7.Controls.Add(Me.ComboBox11)
        Me.TabPage7.Controls.Add(Me.Label16)
        Me.TabPage7.Controls.Add(Me.ComboBox10)
        Me.TabPage7.Controls.Add(Me.Label15)
        Me.TabPage7.Controls.Add(Me.ComboBox9)
        Me.TabPage7.Controls.Add(Me.Label14)
        Me.TabPage7.Controls.Add(Me.ComboBox8)
        Me.TabPage7.Controls.Add(Me.Label13)
        Me.TabPage7.Controls.Add(Me.ComboBox7)
        Me.TabPage7.Controls.Add(Me.Label12)
        Me.TabPage7.Controls.Add(Me.ComboBox6)
        Me.TabPage7.Controls.Add(Me.Label11)
        Me.TabPage7.Controls.Add(Me.ComboBox5)
        Me.TabPage7.Controls.Add(Me.Label10)
        Me.TabPage7.Controls.Add(Me.ComboBox4)
        Me.TabPage7.Controls.Add(Me.Label8)
        Me.TabPage7.Controls.Add(Me.ComboBox3)
        Me.TabPage7.Controls.Add(Me.comboBox1)
        Me.TabPage7.Controls.Add(Me.Label7)
        Me.TabPage7.Controls.Add(Me.Label3)
        Me.TabPage7.Controls.Add(Me.comboBox2)
        Me.TabPage7.Location = New System.Drawing.Point(4, 46)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(647, 396)
        Me.TabPage7.TabIndex = 8
        Me.TabPage7.Text = "Basin Characteristics"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(27, 362)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(137, 13)
        Me.Label19.TabIndex = 46
        Me.Label19.Text = "Max Impervious Cover Grid:"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox13
        '
        Me.ComboBox13.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox13.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox13.Location = New System.Drawing.Point(183, 359)
        Me.ComboBox13.Name = "ComboBox13"
        Me.ComboBox13.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox13.TabIndex = 47
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(27, 335)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(134, 13)
        Me.Label18.TabIndex = 44
        Me.Label18.Text = "Downstream Basin ID Grid:"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox12
        '
        Me.ComboBox12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox12.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox12.Location = New System.Drawing.Point(183, 333)
        Me.ComboBox12.Name = "ComboBox12"
        Me.ComboBox12.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox12.TabIndex = 45
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(27, 309)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(88, 13)
        Me.Label17.TabIndex = 42
        Me.Label17.Text = "Stream Link Grid:"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox11
        '
        Me.ComboBox11.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox11.Location = New System.Drawing.Point(183, 306)
        Me.ComboBox11.Name = "ComboBox11"
        Me.ComboBox11.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox11.TabIndex = 43
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(27, 282)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(152, 13)
        Me.Label16.TabIndex = 40
        Me.Label16.Text = "Downstream Flow Length Grid:"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox10
        '
        Me.ComboBox10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox10.Location = New System.Drawing.Point(183, 279)
        Me.ComboBox10.Name = "ComboBox10"
        Me.ComboBox10.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox10.TabIndex = 41
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(27, 255)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(137, 13)
        Me.Label15.TabIndex = 38
        Me.Label15.Text = "Hydraulic Conductivity Grid:"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox9
        '
        Me.ComboBox9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox9.Location = New System.Drawing.Point(183, 252)
        Me.ComboBox9.Name = "ComboBox9"
        Me.ComboBox9.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox9.TabIndex = 39
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(27, 228)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(88, 13)
        Me.Label14.TabIndex = 36
        Me.Label14.Text = "Soil Texture Grid:"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox8
        '
        Me.ComboBox8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox8.Location = New System.Drawing.Point(183, 225)
        Me.ComboBox8.Name = "ComboBox8"
        Me.ComboBox8.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox8.TabIndex = 37
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(27, 201)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(81, 13)
        Me.Label13.TabIndex = 34
        Me.Label13.Text = "Soil Depth Grid:"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox7
        '
        Me.ComboBox7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox7.Location = New System.Drawing.Point(183, 198)
        Me.ComboBox7.Name = "ComboBox7"
        Me.ComboBox7.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox7.TabIndex = 35
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(27, 174)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(144, 13)
        Me.Label12.TabIndex = 32
        Me.Label12.Text = "Water Holding Capacity Grid:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox6
        '
        Me.ComboBox6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox6.Location = New System.Drawing.Point(183, 171)
        Me.ComboBox6.Name = "ComboBox6"
        Me.ComboBox6.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox6.TabIndex = 33
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(27, 147)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(135, 13)
        Me.Label11.TabIndex = 30
        Me.Label11.Text = "Runoff Curve Number Grid:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox5
        '
        Me.ComboBox5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox5.Location = New System.Drawing.Point(183, 144)
        Me.ComboBox5.Name = "ComboBox5"
        Me.ComboBox5.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox5.TabIndex = 31
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(27, 120)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(82, 13)
        Me.Label10.TabIndex = 28
        Me.Label10.Text = "Hill Length Grid:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox4
        '
        Me.ComboBox4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox4.Location = New System.Drawing.Point(183, 117)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox4.TabIndex = 29
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(27, 92)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(121, 13)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "Flow Accumulation Grid:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox3
        '
        Me.ComboBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox3.Location = New System.Drawing.Point(183, 90)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(351, 21)
        Me.ComboBox3.TabIndex = 27
        '
        'comboBox1
        '
        Me.comboBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comboBox1.Location = New System.Drawing.Point(183, 36)
        Me.comboBox1.Name = "comboBox1"
        Me.comboBox1.Size = New System.Drawing.Size(351, 21)
        Me.comboBox1.TabIndex = 25
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(27, 38)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(58, 13)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "Basin Grid:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(27, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Processed DEM:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'comboBox2
        '
        Me.comboBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comboBox2.Location = New System.Drawing.Point(183, 63)
        Me.comboBox2.Name = "comboBox2"
        Me.comboBox2.Size = New System.Drawing.Size(351, 21)
        Me.comboBox2.TabIndex = 23
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.Button3)
        Me.TabPage8.Controls.Add(Me.AtcGridMannings)
        Me.TabPage8.Controls.Add(Me.Label49)
        Me.TabPage8.Controls.Add(Me.ComboBox43)
        Me.TabPage8.Controls.Add(Me.Label48)
        Me.TabPage8.Controls.Add(Me.ComboBox42)
        Me.TabPage8.Controls.Add(Me.Label47)
        Me.TabPage8.Controls.Add(Me.ComboBox41)
        Me.TabPage8.Controls.Add(Me.Label46)
        Me.TabPage8.Controls.Add(Me.ComboBox40)
        Me.TabPage8.Controls.Add(Me.RadioButton3)
        Me.TabPage8.Controls.Add(Me.RadioButton2)
        Me.TabPage8.Controls.Add(Me.RadioButton1)
        Me.TabPage8.Controls.Add(Me.Label5)
        Me.TabPage8.Location = New System.Drawing.Point(4, 46)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(647, 396)
        Me.TabPage8.TabIndex = 9
        Me.TabPage8.Text = "Basin Response"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'AtcGridMannings
        '
        Me.AtcGridMannings.AllowHorizontalScrolling = True
        Me.AtcGridMannings.AllowNewValidValues = False
        Me.AtcGridMannings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridMannings.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridMannings.Fixed3D = False
        Me.AtcGridMannings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridMannings.LineColor = System.Drawing.Color.Empty
        Me.AtcGridMannings.LineWidth = 0.0!
        Me.AtcGridMannings.Location = New System.Drawing.Point(31, 259)
        Me.AtcGridMannings.Name = "AtcGridMannings"
        Me.AtcGridMannings.Size = New System.Drawing.Size(175, 107)
        Me.AtcGridMannings.Source = Nothing
        Me.AtcGridMannings.TabIndex = 50
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(28, 226)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(120, 13)
        Me.Label49.TabIndex = 48
        Me.Label49.Text = "USGS Land Cover Grid:"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox43
        '
        Me.ComboBox43.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox43.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox43.Location = New System.Drawing.Point(152, 223)
        Me.ComboBox43.Name = "ComboBox43"
        Me.ComboBox43.Size = New System.Drawing.Size(368, 21)
        Me.ComboBox43.TabIndex = 49
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label48.Location = New System.Drawing.Point(28, 199)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(96, 13)
        Me.Label48.TabIndex = 46
        Me.Label48.Text = "Stream Outlet Grid:"
        Me.Label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox42
        '
        Me.ComboBox42.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox42.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox42.Location = New System.Drawing.Point(152, 196)
        Me.ComboBox42.Name = "ComboBox42"
        Me.ComboBox42.Size = New System.Drawing.Size(368, 21)
        Me.ComboBox42.TabIndex = 47
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(28, 172)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(90, 13)
        Me.Label47.TabIndex = 44
        Me.Label47.Text = "Flow Length Grid:"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox41
        '
        Me.ComboBox41.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox41.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox41.Location = New System.Drawing.Point(152, 169)
        Me.ComboBox41.Name = "ComboBox41"
        Me.ComboBox41.Size = New System.Drawing.Size(368, 21)
        Me.ComboBox41.TabIndex = 45
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.Location = New System.Drawing.Point(28, 145)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(99, 13)
        Me.Label46.TabIndex = 42
        Me.Label46.Text = "Flow Direction Grid:"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox40
        '
        Me.ComboBox40.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox40.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox40.Location = New System.Drawing.Point(152, 142)
        Me.ComboBox40.Name = "ComboBox40"
        Me.ComboBox40.Size = New System.Drawing.Size(368, 21)
        Me.ComboBox40.TabIndex = 43
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(48, 103)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(223, 17)
        Me.RadioButton3.TabIndex = 3
        Me.RadioButton3.Text = "Uniform from User Supplied Velocity Value"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(48, 80)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(238, 17)
        Me.RadioButton2.TabIndex = 2
        Me.RadioButton2.Text = "Non-Uniform from User Supplied Velocity Grid"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(48, 57)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(220, 17)
        Me.RadioButton1.TabIndex = 1
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Non-Uniform from USGS Land Cover Grid"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(28, 30)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(231, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Method of Overland Flow Velocity Computation:"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Button4)
        Me.TabPage3.Controls.Add(Me.cboPrecipStation)
        Me.TabPage3.Controls.Add(Me.lblPrecipStation)
        Me.TabPage3.Controls.Add(Me.cboOtherMet)
        Me.TabPage3.Controls.Add(Me.lblOtherMet)
        Me.TabPage3.Controls.Add(Me.GroupBox2)
        Me.TabPage3.Location = New System.Drawing.Point(4, 46)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(647, 396)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Rain/Evap Data"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.Label54)
        Me.GroupBox2.Controls.Add(Me.Label55)
        Me.GroupBox2.Controls.Add(Me.Label56)
        Me.GroupBox2.Controls.Add(Me.Label57)
        Me.GroupBox2.Controls.Add(Me.Label58)
        Me.GroupBox2.Controls.Add(Me.AtcText2)
        Me.GroupBox2.Controls.Add(Me.AtcText3)
        Me.GroupBox2.Controls.Add(Me.AtcText4)
        Me.GroupBox2.Controls.Add(Me.AtcText5)
        Me.GroupBox2.Controls.Add(Me.AtcText6)
        Me.GroupBox2.Controls.Add(Me.AtcText7)
        Me.GroupBox2.Location = New System.Drawing.Point(19, 27)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(516, 96)
        Me.GroupBox2.TabIndex = 29
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Simulation Dates"
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Location = New System.Drawing.Point(87, 58)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(26, 13)
        Me.Label54.TabIndex = 37
        Me.Label54.Text = "End"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(83, 32)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(29, 13)
        Me.Label55.TabIndex = 36
        Me.Label55.Text = "Start"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(242, 14)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(26, 13)
        Me.Label56.TabIndex = 35
        Me.Label56.Text = "Day"
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(193, 14)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(37, 13)
        Me.Label57.TabIndex = 34
        Me.Label57.Text = "Month"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(125, 14)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(29, 13)
        Me.Label58.TabIndex = 33
        Me.Label58.Text = "Year"
        '
        'AtcText2
        '
        Me.AtcText2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText2.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText2.DefaultValue = ""
        Me.AtcText2.HardMax = 31
        Me.AtcText2.HardMin = 1
        Me.AtcText2.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText2.Location = New System.Drawing.Point(245, 58)
        Me.AtcText2.MaxWidth = 20
        Me.AtcText2.Name = "AtcText2"
        Me.AtcText2.NumericFormat = "0"
        Me.AtcText2.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText2.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText2.SelLength = 0
        Me.AtcText2.SelStart = 0
        Me.AtcText2.Size = New System.Drawing.Size(44, 21)
        Me.AtcText2.SoftMax = -999
        Me.AtcText2.SoftMin = -999
        Me.AtcText2.TabIndex = 32
        Me.AtcText2.ValueDouble = 31
        Me.AtcText2.ValueInteger = 31
        '
        'AtcText3
        '
        Me.AtcText3.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText3.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText3.DefaultValue = ""
        Me.AtcText3.HardMax = 31
        Me.AtcText3.HardMin = 1
        Me.AtcText3.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText3.Location = New System.Drawing.Point(245, 32)
        Me.AtcText3.MaxWidth = 20
        Me.AtcText3.Name = "AtcText3"
        Me.AtcText3.NumericFormat = "0"
        Me.AtcText3.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText3.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText3.SelLength = 0
        Me.AtcText3.SelStart = 0
        Me.AtcText3.Size = New System.Drawing.Size(44, 21)
        Me.AtcText3.SoftMax = -999
        Me.AtcText3.SoftMin = -999
        Me.AtcText3.TabIndex = 31
        Me.AtcText3.ValueDouble = 1
        Me.AtcText3.ValueInteger = 1
        '
        'AtcText4
        '
        Me.AtcText4.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText4.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText4.DefaultValue = ""
        Me.AtcText4.HardMax = 9999
        Me.AtcText4.HardMin = 0
        Me.AtcText4.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText4.Location = New System.Drawing.Point(127, 32)
        Me.AtcText4.MaxWidth = 20
        Me.AtcText4.Name = "AtcText4"
        Me.AtcText4.NumericFormat = "0"
        Me.AtcText4.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText4.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText4.SelLength = 0
        Me.AtcText4.SelStart = 0
        Me.AtcText4.Size = New System.Drawing.Size(64, 21)
        Me.AtcText4.SoftMax = -999
        Me.AtcText4.SoftMin = -999
        Me.AtcText4.TabIndex = 30
        Me.AtcText4.ValueDouble = 2000
        Me.AtcText4.ValueInteger = 2000
        '
        'AtcText5
        '
        Me.AtcText5.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText5.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText5.DefaultValue = ""
        Me.AtcText5.HardMax = 12
        Me.AtcText5.HardMin = 1
        Me.AtcText5.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText5.Location = New System.Drawing.Point(196, 58)
        Me.AtcText5.MaxWidth = 20
        Me.AtcText5.Name = "AtcText5"
        Me.AtcText5.NumericFormat = "0"
        Me.AtcText5.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText5.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText5.SelLength = 0
        Me.AtcText5.SelStart = 0
        Me.AtcText5.Size = New System.Drawing.Size(44, 21)
        Me.AtcText5.SoftMax = -999
        Me.AtcText5.SoftMin = -999
        Me.AtcText5.TabIndex = 29
        Me.AtcText5.ValueDouble = 12
        Me.AtcText5.ValueInteger = 12
        '
        'AtcText6
        '
        Me.AtcText6.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText6.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText6.DefaultValue = ""
        Me.AtcText6.HardMax = 12
        Me.AtcText6.HardMin = 1
        Me.AtcText6.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText6.Location = New System.Drawing.Point(196, 32)
        Me.AtcText6.MaxWidth = 20
        Me.AtcText6.Name = "AtcText6"
        Me.AtcText6.NumericFormat = "0"
        Me.AtcText6.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText6.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText6.SelLength = 0
        Me.AtcText6.SelStart = 0
        Me.AtcText6.Size = New System.Drawing.Size(44, 21)
        Me.AtcText6.SoftMax = -999
        Me.AtcText6.SoftMin = -999
        Me.AtcText6.TabIndex = 28
        Me.AtcText6.ValueDouble = 1
        Me.AtcText6.ValueInteger = 1
        '
        'AtcText7
        '
        Me.AtcText7.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText7.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText7.DefaultValue = ""
        Me.AtcText7.HardMax = 9999
        Me.AtcText7.HardMin = 0
        Me.AtcText7.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText7.Location = New System.Drawing.Point(127, 58)
        Me.AtcText7.MaxWidth = 20
        Me.AtcText7.Name = "AtcText7"
        Me.AtcText7.NumericFormat = "0"
        Me.AtcText7.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText7.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText7.SelLength = 0
        Me.AtcText7.SelStart = 0
        Me.AtcText7.Size = New System.Drawing.Size(64, 21)
        Me.AtcText7.SoftMax = -999
        Me.AtcText7.SoftMin = -999
        Me.AtcText7.TabIndex = 27
        Me.AtcText7.ValueDouble = 2000
        Me.AtcText7.ValueInteger = 2000
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblStatus)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(15, 467)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(656, 48)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Status"
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(13, 21)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(631, 14)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Update specifications if desired, then click OK to proceed."
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(27, 362)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(137, 13)
        Me.Label20.TabIndex = 46
        Me.Label20.Text = "Max Impervious Cover Grid:"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox14
        '
        Me.ComboBox14.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox14.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox14.Location = New System.Drawing.Point(183, 359)
        Me.ComboBox14.Name = "ComboBox14"
        Me.ComboBox14.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox14.TabIndex = 47
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(27, 335)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(134, 13)
        Me.Label21.TabIndex = 44
        Me.Label21.Text = "Downstream Basin ID Grid:"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox15
        '
        Me.ComboBox15.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox15.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox15.Location = New System.Drawing.Point(183, 333)
        Me.ComboBox15.Name = "ComboBox15"
        Me.ComboBox15.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox15.TabIndex = 45
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(27, 309)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(88, 13)
        Me.Label22.TabIndex = 42
        Me.Label22.Text = "Stream Link Grid:"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox16
        '
        Me.ComboBox16.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox16.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox16.Location = New System.Drawing.Point(183, 306)
        Me.ComboBox16.Name = "ComboBox16"
        Me.ComboBox16.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox16.TabIndex = 43
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(27, 282)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(152, 13)
        Me.Label23.TabIndex = 40
        Me.Label23.Text = "Downstream Flow Length Grid:"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox17
        '
        Me.ComboBox17.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox17.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox17.Location = New System.Drawing.Point(183, 279)
        Me.ComboBox17.Name = "ComboBox17"
        Me.ComboBox17.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox17.TabIndex = 41
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(27, 255)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(137, 13)
        Me.Label24.TabIndex = 38
        Me.Label24.Text = "Hydraulic Conductivity Grid:"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox18
        '
        Me.ComboBox18.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox18.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox18.Location = New System.Drawing.Point(183, 252)
        Me.ComboBox18.Name = "ComboBox18"
        Me.ComboBox18.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox18.TabIndex = 39
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(27, 228)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(88, 13)
        Me.Label25.TabIndex = 36
        Me.Label25.Text = "Soil Texture Grid:"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox19
        '
        Me.ComboBox19.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox19.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox19.Location = New System.Drawing.Point(183, 225)
        Me.ComboBox19.Name = "ComboBox19"
        Me.ComboBox19.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox19.TabIndex = 37
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(27, 201)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(81, 13)
        Me.Label26.TabIndex = 34
        Me.Label26.Text = "Soil Depth Grid:"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox20
        '
        Me.ComboBox20.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox20.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox20.Location = New System.Drawing.Point(183, 198)
        Me.ComboBox20.Name = "ComboBox20"
        Me.ComboBox20.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox20.TabIndex = 35
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(27, 174)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(144, 13)
        Me.Label27.TabIndex = 32
        Me.Label27.Text = "Water Holding Capacity Grid:"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox21
        '
        Me.ComboBox21.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox21.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox21.Location = New System.Drawing.Point(183, 171)
        Me.ComboBox21.Name = "ComboBox21"
        Me.ComboBox21.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox21.TabIndex = 33
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(27, 147)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(135, 13)
        Me.Label28.TabIndex = 30
        Me.Label28.Text = "Runoff Curve Number Grid:"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox22
        '
        Me.ComboBox22.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox22.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox22.Location = New System.Drawing.Point(183, 144)
        Me.ComboBox22.Name = "ComboBox22"
        Me.ComboBox22.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox22.TabIndex = 31
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(27, 120)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(82, 13)
        Me.Label29.TabIndex = 28
        Me.Label29.Text = "Hill Length Grid:"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox23
        '
        Me.ComboBox23.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox23.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox23.Location = New System.Drawing.Point(183, 117)
        Me.ComboBox23.Name = "ComboBox23"
        Me.ComboBox23.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox23.TabIndex = 29
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(27, 92)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(121, 13)
        Me.Label30.TabIndex = 26
        Me.Label30.Text = "Flow Accumulation Grid:"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox24
        '
        Me.ComboBox24.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox24.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox24.Location = New System.Drawing.Point(183, 90)
        Me.ComboBox24.Name = "ComboBox24"
        Me.ComboBox24.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox24.TabIndex = 27
        '
        'ComboBox25
        '
        Me.ComboBox25.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox25.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox25.Location = New System.Drawing.Point(183, 36)
        Me.ComboBox25.Name = "ComboBox25"
        Me.ComboBox25.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox25.TabIndex = 25
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(27, 38)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(58, 13)
        Me.Label31.TabIndex = 24
        Me.Label31.Text = "Basin Grid:"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(27, 65)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(87, 13)
        Me.Label32.TabIndex = 20
        Me.Label32.Text = "Processed DEM:"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox26
        '
        Me.ComboBox26.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox26.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox26.Location = New System.Drawing.Point(183, 63)
        Me.ComboBox26.Name = "ComboBox26"
        Me.ComboBox26.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox26.TabIndex = 23
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(27, 362)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(137, 13)
        Me.Label33.TabIndex = 46
        Me.Label33.Text = "Max Impervious Cover Grid:"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox27
        '
        Me.ComboBox27.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox27.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox27.Location = New System.Drawing.Point(183, 359)
        Me.ComboBox27.Name = "ComboBox27"
        Me.ComboBox27.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox27.TabIndex = 47
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(27, 335)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(134, 13)
        Me.Label34.TabIndex = 44
        Me.Label34.Text = "Downstream Basin ID Grid:"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox28
        '
        Me.ComboBox28.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox28.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox28.Location = New System.Drawing.Point(183, 333)
        Me.ComboBox28.Name = "ComboBox28"
        Me.ComboBox28.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox28.TabIndex = 45
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(27, 309)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(88, 13)
        Me.Label35.TabIndex = 42
        Me.Label35.Text = "Stream Link Grid:"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox29
        '
        Me.ComboBox29.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox29.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox29.Location = New System.Drawing.Point(183, 306)
        Me.ComboBox29.Name = "ComboBox29"
        Me.ComboBox29.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox29.TabIndex = 43
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(27, 282)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(152, 13)
        Me.Label36.TabIndex = 40
        Me.Label36.Text = "Downstream Flow Length Grid:"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox30
        '
        Me.ComboBox30.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox30.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox30.Location = New System.Drawing.Point(183, 279)
        Me.ComboBox30.Name = "ComboBox30"
        Me.ComboBox30.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox30.TabIndex = 41
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(27, 255)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(137, 13)
        Me.Label37.TabIndex = 38
        Me.Label37.Text = "Hydraulic Conductivity Grid:"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox31
        '
        Me.ComboBox31.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox31.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox31.Location = New System.Drawing.Point(183, 252)
        Me.ComboBox31.Name = "ComboBox31"
        Me.ComboBox31.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox31.TabIndex = 39
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(27, 228)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(88, 13)
        Me.Label38.TabIndex = 36
        Me.Label38.Text = "Soil Texture Grid:"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox32
        '
        Me.ComboBox32.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox32.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox32.Location = New System.Drawing.Point(183, 225)
        Me.ComboBox32.Name = "ComboBox32"
        Me.ComboBox32.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox32.TabIndex = 37
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(27, 201)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(81, 13)
        Me.Label39.TabIndex = 34
        Me.Label39.Text = "Soil Depth Grid:"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox33
        '
        Me.ComboBox33.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox33.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox33.Location = New System.Drawing.Point(183, 198)
        Me.ComboBox33.Name = "ComboBox33"
        Me.ComboBox33.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox33.TabIndex = 35
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(27, 174)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(144, 13)
        Me.Label40.TabIndex = 32
        Me.Label40.Text = "Water Holding Capacity Grid:"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox34
        '
        Me.ComboBox34.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox34.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox34.Location = New System.Drawing.Point(183, 171)
        Me.ComboBox34.Name = "ComboBox34"
        Me.ComboBox34.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox34.TabIndex = 33
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.Location = New System.Drawing.Point(27, 147)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(135, 13)
        Me.Label41.TabIndex = 30
        Me.Label41.Text = "Runoff Curve Number Grid:"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox35
        '
        Me.ComboBox35.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox35.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox35.Location = New System.Drawing.Point(183, 144)
        Me.ComboBox35.Name = "ComboBox35"
        Me.ComboBox35.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox35.TabIndex = 31
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(27, 120)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(82, 13)
        Me.Label42.TabIndex = 28
        Me.Label42.Text = "Hill Length Grid:"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox36
        '
        Me.ComboBox36.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox36.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox36.Location = New System.Drawing.Point(183, 117)
        Me.ComboBox36.Name = "ComboBox36"
        Me.ComboBox36.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox36.TabIndex = 29
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(27, 92)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(121, 13)
        Me.Label43.TabIndex = 26
        Me.Label43.Text = "Flow Accumulation Grid:"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox37
        '
        Me.ComboBox37.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox37.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox37.Location = New System.Drawing.Point(183, 90)
        Me.ComboBox37.Name = "ComboBox37"
        Me.ComboBox37.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox37.TabIndex = 27
        '
        'ComboBox38
        '
        Me.ComboBox38.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox38.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox38.Location = New System.Drawing.Point(183, 36)
        Me.ComboBox38.Name = "ComboBox38"
        Me.ComboBox38.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox38.TabIndex = 25
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(27, 38)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(58, 13)
        Me.Label44.TabIndex = 24
        Me.Label44.Text = "Basin Grid:"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(27, 65)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(87, 13)
        Me.Label45.TabIndex = 20
        Me.Label45.Text = "Processed DEM:"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox39
        '
        Me.ComboBox39.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox39.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox39.Location = New System.Drawing.Point(183, 63)
        Me.ComboBox39.Name = "ComboBox39"
        Me.ComboBox39.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox39.TabIndex = 23
        '
        'cboPrecipStation
        '
        Me.cboPrecipStation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPrecipStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrecipStation.FormattingEnabled = True
        Me.cboPrecipStation.Location = New System.Drawing.Point(119, 129)
        Me.cboPrecipStation.Name = "cboPrecipStation"
        Me.cboPrecipStation.Size = New System.Drawing.Size(416, 21)
        Me.cboPrecipStation.TabIndex = 33
        '
        'lblPrecipStation
        '
        Me.lblPrecipStation.AutoSize = True
        Me.lblPrecipStation.Location = New System.Drawing.Point(16, 132)
        Me.lblPrecipStation.Name = "lblPrecipStation"
        Me.lblPrecipStation.Size = New System.Drawing.Size(76, 13)
        Me.lblPrecipStation.TabIndex = 32
        Me.lblPrecipStation.Text = "Precip Station:"
        '
        'cboOtherMet
        '
        Me.cboOtherMet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboOtherMet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOtherMet.FormattingEnabled = True
        Me.cboOtherMet.Location = New System.Drawing.Point(119, 156)
        Me.cboOtherMet.Name = "cboOtherMet"
        Me.cboOtherMet.Size = New System.Drawing.Size(416, 21)
        Me.cboOtherMet.TabIndex = 31
        '
        'lblOtherMet
        '
        Me.lblOtherMet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblOtherMet.AutoSize = True
        Me.lblOtherMet.Location = New System.Drawing.Point(16, 159)
        Me.lblOtherMet.Name = "lblOtherMet"
        Me.lblOtherMet.Size = New System.Drawing.Size(83, 13)
        Me.lblOtherMet.TabIndex = 30
        Me.lblOtherMet.Text = "Other Met Data:"
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button5)
        Me.TabPage1.Controls.Add(Me.AtcGrid1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 46)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(647, 396)
        Me.TabPage1.TabIndex = 11
        Me.TabPage1.Text = "Compute Soil Water Balance"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'AtcGrid1
        '
        Me.AtcGrid1.AllowHorizontalScrolling = True
        Me.AtcGrid1.AllowNewValidValues = False
        Me.AtcGrid1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGrid1.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGrid1.Fixed3D = False
        Me.AtcGrid1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGrid1.LineColor = System.Drawing.Color.Empty
        Me.AtcGrid1.LineWidth = 0.0!
        Me.AtcGrid1.Location = New System.Drawing.Point(21, 19)
        Me.AtcGrid1.Name = "AtcGrid1"
        Me.AtcGrid1.Size = New System.Drawing.Size(521, 308)
        Me.AtcGrid1.Source = Nothing
        Me.AtcGrid1.TabIndex = 51
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Button6)
        Me.TabPage2.Controls.Add(Me.AtcGrid2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 46)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(647, 396)
        Me.TabPage2.TabIndex = 12
        Me.TabPage2.Text = "Compute Stream Flow"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'AtcGrid2
        '
        Me.AtcGrid2.AllowHorizontalScrolling = True
        Me.AtcGrid2.AllowNewValidValues = False
        Me.AtcGrid2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGrid2.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGrid2.Fixed3D = False
        Me.AtcGrid2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGrid2.LineColor = System.Drawing.Color.Empty
        Me.AtcGrid2.LineWidth = 0.0!
        Me.AtcGrid2.Location = New System.Drawing.Point(25, 23)
        Me.AtcGrid2.Name = "AtcGrid2"
        Me.AtcGrid2.Size = New System.Drawing.Size(518, 308)
        Me.AtcGrid2.Source = Nothing
        Me.AtcGrid2.TabIndex = 52
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.Button7)
        Me.TabPage4.Location = New System.Drawing.Point(4, 46)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(647, 396)
        Me.TabPage4.TabIndex = 13
        Me.TabPage4.Text = "Sensitivity Analysis"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.Button8)
        Me.TabPage5.Location = New System.Drawing.Point(4, 46)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(647, 396)
        Me.TabPage5.TabIndex = 14
        Me.TabPage5.Text = "Model Calibration"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.Button9)
        Me.TabPage6.Location = New System.Drawing.Point(4, 46)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(647, 396)
        Me.TabPage6.TabIndex = 15
        Me.TabPage6.Text = "Bankfull and Flow Statistics"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'TabPage10
        '
        Me.TabPage10.Controls.Add(Me.Button12)
        Me.TabPage10.Controls.Add(Me.Button10)
        Me.TabPage10.Location = New System.Drawing.Point(4, 46)
        Me.TabPage10.Name = "TabPage10"
        Me.TabPage10.Size = New System.Drawing.Size(647, 396)
        Me.TabPage10.TabIndex = 16
        Me.TabPage10.Text = "Flow Percentile Map"
        Me.TabPage10.UseVisualStyleBackColor = True
        '
        'TabPage11
        '
        Me.TabPage11.Controls.Add(Me.Button11)
        Me.TabPage11.Location = New System.Drawing.Point(4, 46)
        Me.TabPage11.Name = "TabPage11"
        Me.TabPage11.Size = New System.Drawing.Size(647, 396)
        Me.TabPage11.TabIndex = 17
        Me.TabPage11.Text = "Flow Hydrographs"
        Me.TabPage11.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(555, 350)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(73, 28)
        Me.Button1.TabIndex = 39
        Me.Button1.Text = "Next >"
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(558, 354)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(73, 28)
        Me.Button2.TabIndex = 48
        Me.Button2.Text = "Next >"
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button3.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Location = New System.Drawing.Point(558, 355)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(73, 28)
        Me.Button3.TabIndex = 51
        Me.Button3.Text = "Next >"
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button4.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(556, 352)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(73, 28)
        Me.Button4.TabIndex = 40
        Me.Button4.Text = "Next >"
        '
        'Button5
        '
        Me.Button5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button5.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.Location = New System.Drawing.Point(554, 353)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(73, 28)
        Me.Button5.TabIndex = 52
        Me.Button5.Text = "Next >"
        '
        'Button6
        '
        Me.Button6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button6.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button6.Location = New System.Drawing.Point(555, 349)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(73, 28)
        Me.Button6.TabIndex = 53
        Me.Button6.Text = "Next >"
        '
        'Button7
        '
        Me.Button7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button7.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button7.Location = New System.Drawing.Point(558, 353)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(73, 28)
        Me.Button7.TabIndex = 40
        Me.Button7.Text = "Next >"
        '
        'Button8
        '
        Me.Button8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button8.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button8.Location = New System.Drawing.Point(554, 350)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(73, 28)
        Me.Button8.TabIndex = 40
        Me.Button8.Text = "Next >"
        '
        'Button9
        '
        Me.Button9.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button9.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button9.Location = New System.Drawing.Point(558, 350)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(73, 28)
        Me.Button9.TabIndex = 40
        Me.Button9.Text = "Next >"
        '
        'Button10
        '
        Me.Button10.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button10.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button10.Location = New System.Drawing.Point(554, 349)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(73, 28)
        Me.Button10.TabIndex = 40
        Me.Button10.Text = "Next >"
        '
        'Button11
        '
        Me.Button11.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button11.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button11.Location = New System.Drawing.Point(557, 350)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(73, 28)
        Me.Button11.TabIndex = 40
        Me.Button11.Text = "Generate"
        '
        'Button12
        '
        Me.Button12.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button12.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button12.Location = New System.Drawing.Point(475, 349)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(73, 28)
        Me.Button12.TabIndex = 41
        Me.Button12.Text = "Generate"
        '
        'frmGeoSFM
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(686, 568)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmGeoSFM"
        Me.Text = "Geospatial Stream Flow Model (GeoSFM) for BASINS"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage9.ResumeLayout(False)
        Me.TabPage9.PerformLayout()
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage7.PerformLayout()
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage8.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage10.ResumeLayout(False)
        Me.TabPage11.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("GeoSFM for BASINS/MapWindow" & vbCrLf & vbCrLf & "Version 1.01", MsgBoxStyle.OkOnly, "BASINS GeoSFM")
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\GeoSFM.html")
    End Sub

    Private Sub frmWASPSetup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\GeoSFM.html")
        End If
    End Sub

    Friend Sub EnableControls(ByVal aEnabled As Boolean)
        cmdCancel.Enabled = aEnabled
        cmdHelp.Enabled = aEnabled
        cmdAbout.Enabled = aEnabled
    End Sub

    Public Sub InitializeUI(ByVal aPlugIn As PlugIn)
        
    End Sub

    Private Sub lblStatus_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStatus.TextChanged
        Logger.Dbg(lblStatus.Text)
    End Sub

End Class