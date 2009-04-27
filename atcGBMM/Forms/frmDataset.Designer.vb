<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmDataset
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdSave As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cboSoil_Map As System.Windows.Forms.ComboBox
    Public WithEvents cboDEM As System.Windows.Forms.ComboBox
    Public WithEvents cboPoint_Sources As System.Windows.Forms.ComboBox
    Public WithEvents btnDEM As System.Windows.Forms.Button
    Public WithEvents btnLanduse As System.Windows.Forms.Button
    Public WithEvents btnSoil_Map As System.Windows.Forms.Button
    Public WithEvents btnPoint_Sources As System.Windows.Forms.Button
    Public WithEvents cboLanduse As System.Windows.Forms.ComboBox
    Public WithEvents cboClimate_Station As System.Windows.Forms.ComboBox
    Public WithEvents btnClimate_Station As System.Windows.Forms.Button
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label17 As System.Windows.Forms.Label
    Public WithEvents Label18 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents Layers As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents btnClimate_Data As System.Windows.Forms.Button
    Public WithEvents cboClimate_Data As System.Windows.Forms.ComboBox
    Public WithEvents btnLanduse_Lookup_Table As System.Windows.Forms.Button
    Public WithEvents cboLanduse_Lookup_Table As System.Windows.Forms.ComboBox
    Public WithEvents btnLanduse_CN_Lookup_Table As System.Windows.Forms.Button
    Public WithEvents cboLanduse_CN_Lookup_Table As System.Windows.Forms.ComboBox
    Public WithEvents btnPoint_Source_Table As System.Windows.Forms.Button
    Public WithEvents cboPoint_Source_Table As System.Windows.Forms.ComboBox
    Public WithEvents cboSoil_Property As System.Windows.Forms.ComboBox
    Public WithEvents btnSoil_Property As System.Windows.Forms.Button
    Public WithEvents Label19 As System.Windows.Forms.Label
    Public WithEvents Label21 As System.Windows.Forms.Label
    Public WithEvents Label22 As System.Windows.Forms.Label
    Public WithEvents Label23 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Tables As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents btnNHD_Flow_Relation As System.Windows.Forms.Button
    Public WithEvents cboNHD_Flow_Relation As System.Windows.Forms.ComboBox
    Public WithEvents btnNHD_Streams As System.Windows.Forms.Button
    Public WithEvents cboNHD_Streams As System.Windows.Forms.ComboBox
    Public WithEvents btnNHD_Lakes As System.Windows.Forms.Button
    Public WithEvents cboNHD_Lakes As System.Windows.Forms.ComboBox
    Public WithEvents btnNHD_Drains As System.Windows.Forms.Button
    Public WithEvents cboNHD_Drains As System.Windows.Forms.ComboBox
    Public WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents lblNHD_Lakes As System.Windows.Forms.Label
    Public WithEvents lblNHD_Drains As System.Windows.Forms.Label
    Public WithEvents FrameNHD As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDataset))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSave = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.Layers = New System.Windows.Forms.GroupBox
        Me.btnSoil_Map = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cboSoil_Map = New System.Windows.Forms.ComboBox
        Me.cboDEM = New System.Windows.Forms.ComboBox
        Me.cboPoint_Sources = New System.Windows.Forms.ComboBox
        Me.btnDEM = New System.Windows.Forms.Button
        Me.btnLanduse = New System.Windows.Forms.Button
        Me.btnPoint_Sources = New System.Windows.Forms.Button
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.cboClimate_Station = New System.Windows.Forms.ComboBox
        Me.btnClimate_Station = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me.Tables = New System.Windows.Forms.GroupBox
        Me.btnClimate_Data = New System.Windows.Forms.Button
        Me.cboClimate_Data = New System.Windows.Forms.ComboBox
        Me.btnLanduse_Lookup_Table = New System.Windows.Forms.Button
        Me.cboLanduse_Lookup_Table = New System.Windows.Forms.ComboBox
        Me.btnLanduse_CN_Lookup_Table = New System.Windows.Forms.Button
        Me.cboLanduse_CN_Lookup_Table = New System.Windows.Forms.ComboBox
        Me.btnPoint_Source_Table = New System.Windows.Forms.Button
        Me.cboPoint_Source_Table = New System.Windows.Forms.ComboBox
        Me.cboSoil_Property = New System.Windows.Forms.ComboBox
        Me.btnSoil_Property = New System.Windows.Forms.Button
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage
        Me.FrameNHD = New System.Windows.Forms.GroupBox
        Me.btnNHD_Flow_Relation = New System.Windows.Forms.Button
        Me.cboNHD_Flow_Relation = New System.Windows.Forms.ComboBox
        Me.btnNHD_Streams = New System.Windows.Forms.Button
        Me.cboNHD_Streams = New System.Windows.Forms.ComboBox
        Me.btnNHD_Lakes = New System.Windows.Forms.Button
        Me.cboNHD_Lakes = New System.Windows.Forms.ComboBox
        Me.btnNHD_Drains = New System.Windows.Forms.Button
        Me.cboNHD_Drains = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblNHD_Lakes = New System.Windows.Forms.Label
        Me.lblNHD_Drains = New System.Windows.Forms.Label
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.Layers.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.Tables.SuspendLayout()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me.FrameNHD.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSave.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSave.Location = New System.Drawing.Point(308, 254)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSave.Size = New System.Drawing.Size(54, 24)
        Me.cmdSave.TabIndex = 1
        Me.cmdSave.Text = "Save"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(368, 254)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(54, 24)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(42, 19)
        Me.SSTab1.Location = New System.Drawing.Point(12, 12)
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(411, 230)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.Layers)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(403, 203)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Map Layers"
        '
        'Layers
        '
        Me.Layers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Layers.BackColor = System.Drawing.SystemColors.Control
        Me.Layers.Controls.Add(Me.btnSoil_Map)
        Me.Layers.Controls.Add(Me.cboSoil_Map)
        Me.Layers.Controls.Add(Me.cboDEM)
        Me.Layers.Controls.Add(Me.cboPoint_Sources)
        Me.Layers.Controls.Add(Me.btnDEM)
        Me.Layers.Controls.Add(Me.btnLanduse)
        Me.Layers.Controls.Add(Me.btnPoint_Sources)
        Me.Layers.Controls.Add(Me.cboLanduse)
        Me.Layers.Controls.Add(Me.cboClimate_Station)
        Me.Layers.Controls.Add(Me.btnClimate_Station)
        Me.Layers.Controls.Add(Me.Label2)
        Me.Layers.Controls.Add(Me.Label17)
        Me.Layers.Controls.Add(Me.Label18)
        Me.Layers.Controls.Add(Me.Label1)
        Me.Layers.Controls.Add(Me.Label9)
        Me.Layers.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Layers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Layers.Location = New System.Drawing.Point(3, 3)
        Me.Layers.Name = "Layers"
        Me.Layers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Layers.Size = New System.Drawing.Size(397, 197)
        Me.Layers.TabIndex = 0
        Me.Layers.TabStop = False
        Me.Layers.Text = "Layers"
        '
        'btnSoil_Map
        '
        Me.btnSoil_Map.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnSoil_Map.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnSoil_Map.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSoil_Map.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSoil_Map.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSoil_Map.ImageIndex = 0
        Me.btnSoil_Map.ImageList = Me.ImageList1
        Me.btnSoil_Map.Location = New System.Drawing.Point(367, 24)
        Me.btnSoil_Map.Name = "btnSoil_Map"
        Me.btnSoil_Map.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnSoil_Map.Size = New System.Drawing.Size(24, 22)
        Me.btnSoil_Map.TabIndex = 2
        Me.btnSoil_Map.Tag = "Feature"
        Me.btnSoil_Map.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSoil_Map.UseVisualStyleBackColor = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "openfolderhs.png")
        '
        'cboSoil_Map
        '
        Me.cboSoil_Map.BackColor = System.Drawing.SystemColors.Window
        Me.cboSoil_Map.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSoil_Map.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSoil_Map.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSoil_Map.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSoil_Map.Location = New System.Drawing.Point(154, 24)
        Me.cboSoil_Map.Name = "cboSoil_Map"
        Me.cboSoil_Map.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSoil_Map.Size = New System.Drawing.Size(207, 22)
        Me.cboSoil_Map.TabIndex = 1
        Me.cboSoil_Map.Tag = "Feature"
        '
        'cboDEM
        '
        Me.cboDEM.BackColor = System.Drawing.SystemColors.Window
        Me.cboDEM.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDEM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDEM.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDEM.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDEM.Location = New System.Drawing.Point(154, 88)
        Me.cboDEM.Name = "cboDEM"
        Me.cboDEM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDEM.Size = New System.Drawing.Size(207, 22)
        Me.cboDEM.TabIndex = 7
        Me.cboDEM.Tag = "Raster"
        '
        'cboPoint_Sources
        '
        Me.cboPoint_Sources.BackColor = System.Drawing.SystemColors.Window
        Me.cboPoint_Sources.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPoint_Sources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPoint_Sources.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPoint_Sources.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPoint_Sources.Location = New System.Drawing.Point(154, 152)
        Me.cboPoint_Sources.Name = "cboPoint_Sources"
        Me.cboPoint_Sources.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPoint_Sources.Size = New System.Drawing.Size(207, 22)
        Me.cboPoint_Sources.TabIndex = 13
        Me.cboPoint_Sources.Tag = "Feature"
        '
        'btnDEM
        '
        Me.btnDEM.AutoSize = True
        Me.btnDEM.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnDEM.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDEM.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDEM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDEM.ImageIndex = 0
        Me.btnDEM.ImageList = Me.ImageList1
        Me.btnDEM.Location = New System.Drawing.Point(367, 88)
        Me.btnDEM.Name = "btnDEM"
        Me.btnDEM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDEM.Size = New System.Drawing.Size(24, 22)
        Me.btnDEM.TabIndex = 8
        Me.btnDEM.Tag = "Raster"
        Me.btnDEM.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnDEM.UseVisualStyleBackColor = False
        '
        'btnLanduse
        '
        Me.btnLanduse.AutoSize = True
        Me.btnLanduse.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnLanduse.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnLanduse.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLanduse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnLanduse.ImageIndex = 0
        Me.btnLanduse.ImageList = Me.ImageList1
        Me.btnLanduse.Location = New System.Drawing.Point(367, 56)
        Me.btnLanduse.Name = "btnLanduse"
        Me.btnLanduse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnLanduse.Size = New System.Drawing.Size(24, 22)
        Me.btnLanduse.TabIndex = 5
        Me.btnLanduse.Tag = "Raster"
        Me.btnLanduse.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnLanduse.UseVisualStyleBackColor = False
        '
        'btnPoint_Sources
        '
        Me.btnPoint_Sources.AutoSize = True
        Me.btnPoint_Sources.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnPoint_Sources.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnPoint_Sources.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPoint_Sources.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPoint_Sources.ImageIndex = 0
        Me.btnPoint_Sources.ImageList = Me.ImageList1
        Me.btnPoint_Sources.Location = New System.Drawing.Point(367, 152)
        Me.btnPoint_Sources.Name = "btnPoint_Sources"
        Me.btnPoint_Sources.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnPoint_Sources.Size = New System.Drawing.Size(24, 22)
        Me.btnPoint_Sources.TabIndex = 14
        Me.btnPoint_Sources.Tag = "Feature"
        Me.btnPoint_Sources.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnPoint_Sources.UseVisualStyleBackColor = False
        '
        'cboLanduse
        '
        Me.cboLanduse.BackColor = System.Drawing.SystemColors.Window
        Me.cboLanduse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLanduse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLanduse.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLanduse.Location = New System.Drawing.Point(154, 56)
        Me.cboLanduse.Name = "cboLanduse"
        Me.cboLanduse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLanduse.Size = New System.Drawing.Size(207, 22)
        Me.cboLanduse.TabIndex = 4
        Me.cboLanduse.Tag = "Raster"
        '
        'cboClimate_Station
        '
        Me.cboClimate_Station.BackColor = System.Drawing.SystemColors.Window
        Me.cboClimate_Station.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClimate_Station.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboClimate_Station.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClimate_Station.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboClimate_Station.Location = New System.Drawing.Point(154, 120)
        Me.cboClimate_Station.Name = "cboClimate_Station"
        Me.cboClimate_Station.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClimate_Station.Size = New System.Drawing.Size(207, 22)
        Me.cboClimate_Station.TabIndex = 10
        Me.cboClimate_Station.Tag = "Feature"
        '
        'btnClimate_Station
        '
        Me.btnClimate_Station.AutoSize = True
        Me.btnClimate_Station.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnClimate_Station.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnClimate_Station.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClimate_Station.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnClimate_Station.ImageIndex = 0
        Me.btnClimate_Station.ImageList = Me.ImageList1
        Me.btnClimate_Station.Location = New System.Drawing.Point(367, 120)
        Me.btnClimate_Station.Name = "btnClimate_Station"
        Me.btnClimate_Station.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnClimate_Station.Size = New System.Drawing.Size(24, 22)
        Me.btnClimate_Station.TabIndex = 11
        Me.btnClimate_Station.Tag = "Feature"
        Me.btnClimate_Station.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnClimate_Station.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(6, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(31, 14)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "DEM:"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.SystemColors.Control
        Me.Label17.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label17.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label17.Location = New System.Drawing.Point(6, 28)
        Me.Label17.Name = "Label17"
        Me.Label17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label17.Size = New System.Drawing.Size(50, 14)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Soil Map:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.SystemColors.Control
        Me.Label18.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label18.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label18.Location = New System.Drawing.Point(6, 156)
        Me.Label18.Name = "Label18"
        Me.Label18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label18.Size = New System.Drawing.Size(127, 14)
        Me.Label18.TabIndex = 12
        Me.Label18.Text = "Point Sources (Optional):"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(6, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(52, 14)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Landuse:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(6, 123)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(86, 14)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Climate Stations:"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.Tables)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(403, 203)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "Tables"
        '
        'Tables
        '
        Me.Tables.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tables.BackColor = System.Drawing.SystemColors.Control
        Me.Tables.Controls.Add(Me.btnClimate_Data)
        Me.Tables.Controls.Add(Me.cboClimate_Data)
        Me.Tables.Controls.Add(Me.btnLanduse_Lookup_Table)
        Me.Tables.Controls.Add(Me.cboLanduse_Lookup_Table)
        Me.Tables.Controls.Add(Me.btnLanduse_CN_Lookup_Table)
        Me.Tables.Controls.Add(Me.cboLanduse_CN_Lookup_Table)
        Me.Tables.Controls.Add(Me.btnPoint_Source_Table)
        Me.Tables.Controls.Add(Me.cboPoint_Source_Table)
        Me.Tables.Controls.Add(Me.cboSoil_Property)
        Me.Tables.Controls.Add(Me.btnSoil_Property)
        Me.Tables.Controls.Add(Me.Label19)
        Me.Tables.Controls.Add(Me.Label21)
        Me.Tables.Controls.Add(Me.Label22)
        Me.Tables.Controls.Add(Me.Label23)
        Me.Tables.Controls.Add(Me.Label3)
        Me.Tables.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tables.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Tables.Location = New System.Drawing.Point(3, 3)
        Me.Tables.Name = "Tables"
        Me.Tables.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Tables.Size = New System.Drawing.Size(397, 198)
        Me.Tables.TabIndex = 0
        Me.Tables.TabStop = False
        Me.Tables.Text = "Tables"
        '
        'btnClimate_Data
        '
        Me.btnClimate_Data.AutoSize = True
        Me.btnClimate_Data.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnClimate_Data.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnClimate_Data.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClimate_Data.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnClimate_Data.ImageIndex = 0
        Me.btnClimate_Data.ImageList = Me.ImageList1
        Me.btnClimate_Data.Location = New System.Drawing.Point(367, 87)
        Me.btnClimate_Data.Name = "btnClimate_Data"
        Me.btnClimate_Data.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnClimate_Data.Size = New System.Drawing.Size(24, 22)
        Me.btnClimate_Data.TabIndex = 7
        Me.btnClimate_Data.Tag = "Table"
        Me.btnClimate_Data.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnClimate_Data.UseVisualStyleBackColor = False
        '
        'cboClimate_Data
        '
        Me.cboClimate_Data.BackColor = System.Drawing.SystemColors.Window
        Me.cboClimate_Data.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClimate_Data.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboClimate_Data.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClimate_Data.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboClimate_Data.Location = New System.Drawing.Point(154, 89)
        Me.cboClimate_Data.Name = "cboClimate_Data"
        Me.cboClimate_Data.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClimate_Data.Size = New System.Drawing.Size(207, 22)
        Me.cboClimate_Data.TabIndex = 8
        Me.cboClimate_Data.Tag = "Table"
        '
        'btnLanduse_Lookup_Table
        '
        Me.btnLanduse_Lookup_Table.AutoSize = True
        Me.btnLanduse_Lookup_Table.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnLanduse_Lookup_Table.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnLanduse_Lookup_Table.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLanduse_Lookup_Table.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnLanduse_Lookup_Table.ImageIndex = 0
        Me.btnLanduse_Lookup_Table.ImageList = Me.ImageList1
        Me.btnLanduse_Lookup_Table.Location = New System.Drawing.Point(367, 24)
        Me.btnLanduse_Lookup_Table.Name = "btnLanduse_Lookup_Table"
        Me.btnLanduse_Lookup_Table.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnLanduse_Lookup_Table.Size = New System.Drawing.Size(24, 22)
        Me.btnLanduse_Lookup_Table.TabIndex = 1
        Me.btnLanduse_Lookup_Table.Tag = "Table"
        Me.btnLanduse_Lookup_Table.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnLanduse_Lookup_Table.UseVisualStyleBackColor = False
        '
        'cboLanduse_Lookup_Table
        '
        Me.cboLanduse_Lookup_Table.BackColor = System.Drawing.SystemColors.Window
        Me.cboLanduse_Lookup_Table.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLanduse_Lookup_Table.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse_Lookup_Table.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLanduse_Lookup_Table.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLanduse_Lookup_Table.Location = New System.Drawing.Point(154, 25)
        Me.cboLanduse_Lookup_Table.Name = "cboLanduse_Lookup_Table"
        Me.cboLanduse_Lookup_Table.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLanduse_Lookup_Table.Size = New System.Drawing.Size(207, 22)
        Me.cboLanduse_Lookup_Table.TabIndex = 2
        Me.cboLanduse_Lookup_Table.Tag = "Table"
        '
        'btnLanduse_CN_Lookup_Table
        '
        Me.btnLanduse_CN_Lookup_Table.AutoSize = True
        Me.btnLanduse_CN_Lookup_Table.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnLanduse_CN_Lookup_Table.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnLanduse_CN_Lookup_Table.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLanduse_CN_Lookup_Table.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnLanduse_CN_Lookup_Table.ImageIndex = 0
        Me.btnLanduse_CN_Lookup_Table.ImageList = Me.ImageList1
        Me.btnLanduse_CN_Lookup_Table.Location = New System.Drawing.Point(367, 56)
        Me.btnLanduse_CN_Lookup_Table.Name = "btnLanduse_CN_Lookup_Table"
        Me.btnLanduse_CN_Lookup_Table.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnLanduse_CN_Lookup_Table.Size = New System.Drawing.Size(24, 22)
        Me.btnLanduse_CN_Lookup_Table.TabIndex = 4
        Me.btnLanduse_CN_Lookup_Table.Tag = "Table"
        Me.btnLanduse_CN_Lookup_Table.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnLanduse_CN_Lookup_Table.UseVisualStyleBackColor = False
        '
        'cboLanduse_CN_Lookup_Table
        '
        Me.cboLanduse_CN_Lookup_Table.BackColor = System.Drawing.SystemColors.Window
        Me.cboLanduse_CN_Lookup_Table.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLanduse_CN_Lookup_Table.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse_CN_Lookup_Table.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLanduse_CN_Lookup_Table.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLanduse_CN_Lookup_Table.Location = New System.Drawing.Point(154, 57)
        Me.cboLanduse_CN_Lookup_Table.Name = "cboLanduse_CN_Lookup_Table"
        Me.cboLanduse_CN_Lookup_Table.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLanduse_CN_Lookup_Table.Size = New System.Drawing.Size(207, 22)
        Me.cboLanduse_CN_Lookup_Table.TabIndex = 5
        Me.cboLanduse_CN_Lookup_Table.Tag = "Table"
        '
        'btnPoint_Source_Table
        '
        Me.btnPoint_Source_Table.AutoSize = True
        Me.btnPoint_Source_Table.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnPoint_Source_Table.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnPoint_Source_Table.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPoint_Source_Table.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPoint_Source_Table.ImageIndex = 0
        Me.btnPoint_Source_Table.ImageList = Me.ImageList1
        Me.btnPoint_Source_Table.Location = New System.Drawing.Point(367, 152)
        Me.btnPoint_Source_Table.Name = "btnPoint_Source_Table"
        Me.btnPoint_Source_Table.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnPoint_Source_Table.Size = New System.Drawing.Size(24, 22)
        Me.btnPoint_Source_Table.TabIndex = 13
        Me.btnPoint_Source_Table.Tag = "Table"
        Me.btnPoint_Source_Table.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnPoint_Source_Table.UseVisualStyleBackColor = False
        '
        'cboPoint_Source_Table
        '
        Me.cboPoint_Source_Table.BackColor = System.Drawing.SystemColors.Window
        Me.cboPoint_Source_Table.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPoint_Source_Table.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPoint_Source_Table.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPoint_Source_Table.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPoint_Source_Table.Location = New System.Drawing.Point(154, 153)
        Me.cboPoint_Source_Table.Name = "cboPoint_Source_Table"
        Me.cboPoint_Source_Table.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPoint_Source_Table.Size = New System.Drawing.Size(207, 22)
        Me.cboPoint_Source_Table.TabIndex = 14
        Me.cboPoint_Source_Table.Tag = "Table"
        '
        'cboSoil_Property
        '
        Me.cboSoil_Property.BackColor = System.Drawing.SystemColors.Window
        Me.cboSoil_Property.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSoil_Property.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSoil_Property.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSoil_Property.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSoil_Property.Location = New System.Drawing.Point(154, 121)
        Me.cboSoil_Property.Name = "cboSoil_Property"
        Me.cboSoil_Property.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSoil_Property.Size = New System.Drawing.Size(207, 22)
        Me.cboSoil_Property.TabIndex = 11
        Me.cboSoil_Property.Tag = "Table"
        '
        'btnSoil_Property
        '
        Me.btnSoil_Property.AutoSize = True
        Me.btnSoil_Property.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnSoil_Property.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSoil_Property.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSoil_Property.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSoil_Property.ImageIndex = 0
        Me.btnSoil_Property.ImageList = Me.ImageList1
        Me.btnSoil_Property.Location = New System.Drawing.Point(367, 120)
        Me.btnSoil_Property.Name = "btnSoil_Property"
        Me.btnSoil_Property.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnSoil_Property.Size = New System.Drawing.Size(24, 22)
        Me.btnSoil_Property.TabIndex = 10
        Me.btnSoil_Property.Tag = "Table"
        Me.btnSoil_Property.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSoil_Property.UseVisualStyleBackColor = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.SystemColors.Control
        Me.Label19.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label19.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label19.Location = New System.Drawing.Point(6, 92)
        Me.Label19.Name = "Label19"
        Me.Label19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label19.Size = New System.Drawing.Size(69, 14)
        Me.Label19.TabIndex = 6
        Me.Label19.Text = "Climate Data:"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.BackColor = System.Drawing.SystemColors.Control
        Me.Label21.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label21.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label21.Location = New System.Drawing.Point(6, 28)
        Me.Label21.Name = "Label21"
        Me.Label21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label21.Size = New System.Drawing.Size(145, 14)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Landuse: Parameter Lookup:"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.BackColor = System.Drawing.SystemColors.Control
        Me.Label22.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label22.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label22.Location = New System.Drawing.Point(6, 60)
        Me.Label22.Name = "Label22"
        Me.Label22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label22.Size = New System.Drawing.Size(110, 14)
        Me.Label22.TabIndex = 3
        Me.Label22.Text = "Landuse: CN Lookup:"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.BackColor = System.Drawing.SystemColors.Control
        Me.Label23.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label23.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label23.Location = New System.Drawing.Point(6, 156)
        Me.Label23.Name = "Label23"
        Me.Label23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label23.Size = New System.Drawing.Size(121, 14)
        Me.Label23.TabIndex = 12
        Me.Label23.Text = "Point Source (Optional):"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(6, 124)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(71, 14)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Soil Property:"
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me.FrameNHD)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(403, 203)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "NHD"
        '
        'FrameNHD
        '
        Me.FrameNHD.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FrameNHD.BackColor = System.Drawing.SystemColors.Control
        Me.FrameNHD.Controls.Add(Me.btnNHD_Flow_Relation)
        Me.FrameNHD.Controls.Add(Me.cboNHD_Flow_Relation)
        Me.FrameNHD.Controls.Add(Me.btnNHD_Streams)
        Me.FrameNHD.Controls.Add(Me.cboNHD_Streams)
        Me.FrameNHD.Controls.Add(Me.btnNHD_Lakes)
        Me.FrameNHD.Controls.Add(Me.cboNHD_Lakes)
        Me.FrameNHD.Controls.Add(Me.btnNHD_Drains)
        Me.FrameNHD.Controls.Add(Me.cboNHD_Drains)
        Me.FrameNHD.Controls.Add(Me.Label14)
        Me.FrameNHD.Controls.Add(Me.Label4)
        Me.FrameNHD.Controls.Add(Me.lblNHD_Lakes)
        Me.FrameNHD.Controls.Add(Me.lblNHD_Drains)
        Me.FrameNHD.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FrameNHD.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameNHD.Location = New System.Drawing.Point(3, 3)
        Me.FrameNHD.Name = "FrameNHD"
        Me.FrameNHD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameNHD.Size = New System.Drawing.Size(397, 198)
        Me.FrameNHD.TabIndex = 0
        Me.FrameNHD.TabStop = False
        Me.FrameNHD.Text = "NHD (National Hydrography Dataset)"
        '
        'btnNHD_Flow_Relation
        '
        Me.btnNHD_Flow_Relation.AutoSize = True
        Me.btnNHD_Flow_Relation.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnNHD_Flow_Relation.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNHD_Flow_Relation.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNHD_Flow_Relation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnNHD_Flow_Relation.ImageIndex = 0
        Me.btnNHD_Flow_Relation.ImageList = Me.ImageList1
        Me.btnNHD_Flow_Relation.Location = New System.Drawing.Point(367, 72)
        Me.btnNHD_Flow_Relation.Name = "btnNHD_Flow_Relation"
        Me.btnNHD_Flow_Relation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNHD_Flow_Relation.Size = New System.Drawing.Size(24, 22)
        Me.btnNHD_Flow_Relation.TabIndex = 5
        Me.btnNHD_Flow_Relation.Tag = "Table"
        Me.btnNHD_Flow_Relation.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnNHD_Flow_Relation.UseVisualStyleBackColor = False
        '
        'cboNHD_Flow_Relation
        '
        Me.cboNHD_Flow_Relation.BackColor = System.Drawing.SystemColors.Window
        Me.cboNHD_Flow_Relation.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboNHD_Flow_Relation.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboNHD_Flow_Relation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboNHD_Flow_Relation.Location = New System.Drawing.Point(156, 71)
        Me.cboNHD_Flow_Relation.Name = "cboNHD_Flow_Relation"
        Me.cboNHD_Flow_Relation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboNHD_Flow_Relation.Size = New System.Drawing.Size(205, 22)
        Me.cboNHD_Flow_Relation.TabIndex = 4
        Me.cboNHD_Flow_Relation.Tag = "Table"
        '
        'btnNHD_Streams
        '
        Me.btnNHD_Streams.AutoSize = True
        Me.btnNHD_Streams.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnNHD_Streams.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNHD_Streams.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNHD_Streams.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnNHD_Streams.ImageIndex = 0
        Me.btnNHD_Streams.ImageList = Me.ImageList1
        Me.btnNHD_Streams.Location = New System.Drawing.Point(367, 39)
        Me.btnNHD_Streams.Name = "btnNHD_Streams"
        Me.btnNHD_Streams.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNHD_Streams.Size = New System.Drawing.Size(24, 22)
        Me.btnNHD_Streams.TabIndex = 2
        Me.btnNHD_Streams.Tag = "Feature"
        Me.btnNHD_Streams.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnNHD_Streams.UseVisualStyleBackColor = False
        '
        'cboNHD_Streams
        '
        Me.cboNHD_Streams.BackColor = System.Drawing.SystemColors.Window
        Me.cboNHD_Streams.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboNHD_Streams.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboNHD_Streams.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboNHD_Streams.Location = New System.Drawing.Point(156, 39)
        Me.cboNHD_Streams.Name = "cboNHD_Streams"
        Me.cboNHD_Streams.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboNHD_Streams.Size = New System.Drawing.Size(205, 22)
        Me.cboNHD_Streams.TabIndex = 1
        Me.cboNHD_Streams.Tag = "Feature"
        '
        'btnNHD_Lakes
        '
        Me.btnNHD_Lakes.AutoSize = True
        Me.btnNHD_Lakes.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnNHD_Lakes.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNHD_Lakes.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNHD_Lakes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnNHD_Lakes.ImageIndex = 0
        Me.btnNHD_Lakes.ImageList = Me.ImageList1
        Me.btnNHD_Lakes.Location = New System.Drawing.Point(367, 104)
        Me.btnNHD_Lakes.Name = "btnNHD_Lakes"
        Me.btnNHD_Lakes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNHD_Lakes.Size = New System.Drawing.Size(24, 22)
        Me.btnNHD_Lakes.TabIndex = 8
        Me.btnNHD_Lakes.Tag = "Feature"
        Me.btnNHD_Lakes.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnNHD_Lakes.UseVisualStyleBackColor = False
        '
        'cboNHD_Lakes
        '
        Me.cboNHD_Lakes.BackColor = System.Drawing.SystemColors.Window
        Me.cboNHD_Lakes.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboNHD_Lakes.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboNHD_Lakes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboNHD_Lakes.Location = New System.Drawing.Point(156, 103)
        Me.cboNHD_Lakes.Name = "cboNHD_Lakes"
        Me.cboNHD_Lakes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboNHD_Lakes.Size = New System.Drawing.Size(205, 22)
        Me.cboNHD_Lakes.TabIndex = 7
        Me.cboNHD_Lakes.Tag = "Feature"
        '
        'btnNHD_Drains
        '
        Me.btnNHD_Drains.AutoSize = True
        Me.btnNHD_Drains.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnNHD_Drains.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNHD_Drains.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNHD_Drains.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnNHD_Drains.ImageIndex = 0
        Me.btnNHD_Drains.ImageList = Me.ImageList1
        Me.btnNHD_Drains.Location = New System.Drawing.Point(367, 136)
        Me.btnNHD_Drains.Name = "btnNHD_Drains"
        Me.btnNHD_Drains.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNHD_Drains.Size = New System.Drawing.Size(24, 22)
        Me.btnNHD_Drains.TabIndex = 11
        Me.btnNHD_Drains.Tag = "Feature"
        Me.btnNHD_Drains.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnNHD_Drains.UseVisualStyleBackColor = False
        '
        'cboNHD_Drains
        '
        Me.cboNHD_Drains.BackColor = System.Drawing.SystemColors.Window
        Me.cboNHD_Drains.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboNHD_Drains.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboNHD_Drains.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboNHD_Drains.Location = New System.Drawing.Point(156, 135)
        Me.cboNHD_Drains.Name = "cboNHD_Drains"
        Me.cboNHD_Drains.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboNHD_Drains.Size = New System.Drawing.Size(205, 22)
        Me.cboNHD_Drains.TabIndex = 10
        Me.cboNHD_Drains.Tag = "Feature"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.SystemColors.Control
        Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label14.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label14.Location = New System.Drawing.Point(6, 76)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(141, 14)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "NHD RFlow (Flow Relation):"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(6, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(74, 14)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "NHD Streams:"
        '
        'lblNHD_Lakes
        '
        Me.lblNHD_Lakes.AutoSize = True
        Me.lblNHD_Lakes.BackColor = System.Drawing.SystemColors.Control
        Me.lblNHD_Lakes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNHD_Lakes.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNHD_Lakes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNHD_Lakes.Location = New System.Drawing.Point(6, 106)
        Me.lblNHD_Lakes.Name = "lblNHD_Lakes"
        Me.lblNHD_Lakes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNHD_Lakes.Size = New System.Drawing.Size(113, 14)
        Me.lblNHD_Lakes.TabIndex = 6
        Me.lblNHD_Lakes.Text = "NHD Lakes (Optional):"
        '
        'lblNHD_Drains
        '
        Me.lblNHD_Drains.AutoSize = True
        Me.lblNHD_Drains.BackColor = System.Drawing.SystemColors.Control
        Me.lblNHD_Drains.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNHD_Drains.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNHD_Drains.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNHD_Drains.Location = New System.Drawing.Point(6, 138)
        Me.lblNHD_Drains.Name = "lblNHD_Drains"
        Me.lblNHD_Drains.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNHD_Drains.Size = New System.Drawing.Size(147, 14)
        Me.lblNHD_Drains.TabIndex = 9
        Me.lblNHD_Drains.Text = "NHD Drains (WASP Linkage):"
        '
        'frmDataset
        '
        Me.AcceptButton = Me.cmdSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(434, 290)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(2, 21)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDataset"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Data Management"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.Layers.ResumeLayout(False)
        Me.Layers.PerformLayout()
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.Tables.ResumeLayout(False)
        Me.Tables.PerformLayout()
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me.FrameNHD.ResumeLayout(False)
        Me.FrameNHD.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
#End Region
End Class