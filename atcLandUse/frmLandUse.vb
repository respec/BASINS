Imports atcMwGisUtility
Imports atcUtility
Imports MapWinUtility

Public Class frmLandUse
    Inherits System.Windows.Forms.Form

    Dim DefaultClassFile As String
    Dim FullClassFile As String
    Friend pBasinsFolder As String

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
    Friend WithEvents cboLanduse As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblLandUseLayer As System.Windows.Forms.Label
    Friend WithEvents cboLandUseLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cboDescription As System.Windows.Forms.ComboBox
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents cboSub1 As System.Windows.Forms.ComboBox
    Friend WithEvents cboLUID As System.Windows.Forms.ComboBox
    Friend WithEvents cboSub2 As System.Windows.Forms.ComboBox
    Friend WithEvents cmdNext As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblLUID As System.Windows.Forms.Label
    Friend WithEvents lblSubid As System.Windows.Forms.Label
    Friend WithEvents lblSubname As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmLandUse))
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblLandUseLayer = New System.Windows.Forms.Label
        Me.cboLandUseLayer = New System.Windows.Forms.ComboBox
        Me.cboDescription = New System.Windows.Forms.ComboBox
        Me.lblDescription = New System.Windows.Forms.Label
        Me.cboSub1 = New System.Windows.Forms.ComboBox
        Me.lblSubid = New System.Windows.Forms.Label
        Me.lblLUID = New System.Windows.Forms.Label
        Me.cboLUID = New System.Windows.Forms.ComboBox
        Me.lblSubname = New System.Windows.Forms.Label
        Me.cboSub2 = New System.Windows.Forms.ComboBox
        Me.cmdNext = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubbasins.Location = New System.Drawing.Point(192, 154)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(200, 25)
        Me.cboSubbasins.TabIndex = 12
        '
        'cboLanduse
        '
        Me.cboLanduse.AllowDrop = True
        Me.cboLanduse.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLanduse.Location = New System.Drawing.Point(192, 17)
        Me.cboLanduse.Name = "cboLanduse"
        Me.cboLanduse.Size = New System.Drawing.Size(200, 25)
        Me.cboLanduse.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 154)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(168, 25)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Summarize within Layer:"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(152, 26)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Land Use Type:"
        '
        'lblLandUseLayer
        '
        Me.lblLandUseLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLandUseLayer.Location = New System.Drawing.Point(16, 51)
        Me.lblLandUseLayer.Name = "lblLandUseLayer"
        Me.lblLandUseLayer.Size = New System.Drawing.Size(152, 26)
        Me.lblLandUseLayer.TabIndex = 13
        Me.lblLandUseLayer.Text = "Land Use Layer:"
        '
        'cboLandUseLayer
        '
        Me.cboLandUseLayer.AllowDrop = True
        Me.cboLandUseLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLandUseLayer.Location = New System.Drawing.Point(192, 51)
        Me.cboLandUseLayer.Name = "cboLandUseLayer"
        Me.cboLandUseLayer.Size = New System.Drawing.Size(200, 25)
        Me.cboLandUseLayer.TabIndex = 14
        '
        'cboDescription
        '
        Me.cboDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboDescription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDescription.Location = New System.Drawing.Point(208, 119)
        Me.cboDescription.Name = "cboDescription"
        Me.cboDescription.Size = New System.Drawing.Size(184, 25)
        Me.cboDescription.TabIndex = 16
        '
        'lblDescription
        '
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.Location = New System.Drawing.Point(40, 119)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(152, 26)
        Me.lblDescription.TabIndex = 15
        Me.lblDescription.Text = "Description Field:"
        '
        'cboSub1
        '
        Me.cboSub1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSub1.Location = New System.Drawing.Point(208, 188)
        Me.cboSub1.Name = "cboSub1"
        Me.cboSub1.Size = New System.Drawing.Size(184, 25)
        Me.cboSub1.TabIndex = 18
        '
        'lblSubid
        '
        Me.lblSubid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubid.Location = New System.Drawing.Point(40, 188)
        Me.lblSubid.Name = "lblSubid"
        Me.lblSubid.Size = New System.Drawing.Size(152, 25)
        Me.lblSubid.TabIndex = 17
        Me.lblSubid.Text = "ID Field:"
        '
        'lblLUID
        '
        Me.lblLUID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLUID.Location = New System.Drawing.Point(40, 85)
        Me.lblLUID.Name = "lblLUID"
        Me.lblLUID.Size = New System.Drawing.Size(152, 26)
        Me.lblLUID.TabIndex = 19
        Me.lblLUID.Text = "ID Field:"
        '
        'cboLUID
        '
        Me.cboLUID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLUID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLUID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLUID.Location = New System.Drawing.Point(208, 85)
        Me.cboLUID.Name = "cboLUID"
        Me.cboLUID.Size = New System.Drawing.Size(184, 25)
        Me.cboLUID.TabIndex = 20
        '
        'lblSubname
        '
        Me.lblSubname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubname.Location = New System.Drawing.Point(40, 222)
        Me.lblSubname.Name = "lblSubname"
        Me.lblSubname.Size = New System.Drawing.Size(152, 25)
        Me.lblSubname.TabIndex = 21
        Me.lblSubname.Text = "Name Field:"
        '
        'cboSub2
        '
        Me.cboSub2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSub2.Location = New System.Drawing.Point(208, 222)
        Me.cboSub2.Name = "cboSub2"
        Me.cboSub2.Size = New System.Drawing.Size(184, 25)
        Me.cboSub2.TabIndex = 22
        '
        'cmdNext
        '
        Me.cmdNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNext.Location = New System.Drawing.Point(232, 265)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(72, 34)
        Me.cmdNext.TabIndex = 23
        Me.cmdNext.Text = "&Next"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(136, 265)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 34)
        Me.cmdCancel.TabIndex = 24
        Me.cmdCancel.Text = "&Cancel"
        '
        'frmLandUse
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 16)
        Me.ClientSize = New System.Drawing.Size(440, 315)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdNext)
        Me.Controls.Add(Me.cboSub2)
        Me.Controls.Add(Me.lblSubname)
        Me.Controls.Add(Me.cboLUID)
        Me.Controls.Add(Me.lblLUID)
        Me.Controls.Add(Me.cboSub1)
        Me.Controls.Add(Me.lblSubid)
        Me.Controls.Add(Me.cboDescription)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.cboLandUseLayer)
        Me.Controls.Add(Me.lblLandUseLayer)
        Me.Controls.Add(Me.cboSubbasins)
        Me.Controls.Add(Me.cboLanduse)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmLandUse"
        Me.Text = "BASINS LandUse Reclassification"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        Dim lyr As Integer

        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            cboLandUseLayer.Visible = False
            lblLandUseLayer.Visible = False
            cboDescription.Visible = False
            lblDescription.Visible = False
            cboLUID.Visible = False
            lblLUID.Visible = False
            DefaultClassFile = pBasinsFolder & "\etc\giras.dbf"
            FullClassFile = "<none>"
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
            cboLandUseLayer.Items.Clear()
            For lyr = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerType(lyr) = 3 Then
                    'PolygonShapefile
                    cboLandUseLayer.Items.Add(GisUtil.LayerName(lyr))
                End If
            Next
            If cboLandUseLayer.Items.Count > 0 And cboLandUseLayer.SelectedIndex < 0 Then
                cboLandUseLayer.SelectedIndex = 0
            End If
            cboLandUseLayer.Visible = True
            lblLandUseLayer.Visible = True
            cboDescription.Visible = True
            lblDescription.Visible = True
            cboLUID.Visible = True
            lblLUID.Visible = True
            DefaultClassFile = "<none>"
            FullClassFile = "<none>"
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
            cboLandUseLayer.Items.Clear()
            For lyr = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerType(lyr) = 4 Then
                    'Grid 
                    If InStr(UCase(GisUtil.LayerFileName(lyr)), "\NLCD\") > 0 Then
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
            cboLUID.Visible = False
            lblLUID.Visible = False
            DefaultClassFile = pBasinsFolder & "\etc\nlcd.dbf"
            FullClassFile = pBasinsFolder & "\etc\mlrc.dbf"
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
            cboLUID.Visible = False
            lblLUID.Visible = False
            DefaultClassFile = "<none>"
            FullClassFile = "<none>"
        End If
    End Sub

    Private Sub cboSubbasins_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubbasins.SelectedIndexChanged
        Dim lyr As Long
        Dim i As Long
        Dim ctemp As String

        cboSub1.Items.Clear()
        cboSub2.Items.Clear()
        If cboSubbasins.Items(cboSubbasins.SelectedIndex) = "<none>" Then
            cboSub1.Visible = False
            cboSub2.Visible = False
            lblSubid.Visible = False
            lblSubname.Visible = False
        Else
            cboSub1.Visible = True
            cboSub2.Visible = True
            lblSubid.Visible = True
            lblSubname.Visible = True
            lyr = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
            If lyr > -1 Then
                For i = 0 To GisUtil.NumFields(lyr) - 1
                    ctemp = GisUtil.FieldName(i, lyr)
                    cboSub1.Items.Add(ctemp)
                    cboSub2.Items.Add(ctemp)
                    If UCase(ctemp) = "SUBBASIN" Then
                        cboSub1.SelectedIndex = i
                    End If
                    If UCase(ctemp) = "NAME" Or UCase(ctemp) = "BNAME" Then
                        cboSub2.SelectedIndex = i
                    End If
                Next
            End If
            If cboSub1.Items.Count > 0 And cboSub1.SelectedIndex < 0 Then
                cboSub1.SelectedIndex = 0
            End If
            If cboSub2.Items.Count > 0 And cboSub2.SelectedIndex < 0 Then
                cboSub2.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cboLandUseLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLandUseLayer.SelectedIndexChanged
        Dim i As Long, lyr As Integer
        Dim ctemp As String

        cboDescription.Items.Clear()
        cboLUID.Items.Clear()
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
                        ctemp = GisUtil.FieldName(i, lyr)
                        cboDescription.Items.Add(ctemp)
                        If GisUtil.FieldType(i, lyr) = 0 Then
                            'string
                            cboDescription.SelectedIndex = i
                        End If
                        cboLUID.Items.Add(ctemp)
                        If GisUtil.FieldType(i, lyr) = 1 Then
                            'integer
                            cboLUID.SelectedIndex = i
                        End If
                    Next
                    If cboDescription.Items.Count > 0 And cboDescription.SelectedIndex < 0 Then
                        cboDescription.SelectedIndex = 0
                    End If
                    If cboLUID.Items.Count > 0 And cboLUID.SelectedIndex < 0 Then
                        cboLUID.SelectedIndex = 0
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Dim newFrm As New frmReclass
        Dim LanduseLayerName As String
        Dim SubbasinLayerName As String
        Dim LanduseIDFieldName As String
        Dim LanduseDescFieldName As String
        Dim SubbasinsIDFieldName As String
        Dim SubbasinsNameFieldName As String
        Dim i As Long

        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            i = GisUtil.LayerIndex("Land Use Index")
            If i = -1 Then  'cant do giras without land use index layer
                logger.msg("When using GIRAS Landuse, the 'Land Use Index' layer must exist and be named as such.", vbOKOnly, "Reclass GIRAS Problem")
                Exit Sub
            End If
        End If

        If cboLandUseLayer.SelectedIndex > -1 Then
            LanduseLayerName = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
        Else
            LanduseLayerName = "<none>"
        End If
        If cboSubbasins.SelectedIndex > -1 Then
            SubbasinLayerName = cboSubbasins.Items(cboSubbasins.SelectedIndex)
        Else
            SubbasinLayerName = "<none>"
        End If

        If cboLUID.SelectedIndex > -1 Then
            LanduseIDFieldName = cboLUID.Items(cboLUID.SelectedIndex)
        Else
            LanduseIDFieldName = ""
        End If
        If cboDescription.SelectedIndex > -1 Then
            LanduseDescFieldName = cboDescription.Items(cboDescription.SelectedIndex)
        Else
            LanduseDescFieldName = ""
        End If
        If cboSub1.SelectedIndex > -1 Then
            SubbasinsIDFieldName = cboSub1.Items(cboSub1.SelectedIndex)
        Else
            SubbasinsIDFieldName = ""
        End If
        If cboSub2.SelectedIndex > -1 Then
            SubbasinsNameFieldName = cboSub2.Items(cboSub2.SelectedIndex) '
        Else
            SubbasinsNameFieldName = ""
        End If

        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        newFrm.initialize(cboLanduse.SelectedIndex, LanduseLayerName, _
          LanduseIDFieldName, LanduseDescFieldName, SubbasinLayerName, _
          SubbasinsIDFieldName, SubbasinsNameFieldName)

        newFrm.Show()
        newFrm.FillTable()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Close()
    End Sub

    Public Sub InitializeUI()
        pBasinsFolder = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\BASINS")

        Dim ctemp As String

        cboLanduse.Items.Add("USGS GIRAS Shapefile")
        cboLanduse.Items.Add("NLCD Grid")
        cboLanduse.Items.Add("Other Shapefile")
        cboLanduse.Items.Add("User Grid")
        cboLanduse.SelectedIndex = 0

        cboSubbasins.Items.Add("<none>")

        Dim lyr As Integer

        For lyr = 0 To GisUtil.NumLayers() - 1
            ctemp = GisUtil.LayerName(lyr)
            If GisUtil.LayerType(lyr) = 3 Then
                'PolygonShapefile 
                cboSubbasins.Items.Add(ctemp)
                If UCase(ctemp) = "SUBBASINS" Then
                    cboSubbasins.SelectedIndex = cboSubbasins.Items.Count - 1
                End If
            End If
        Next
        If cboSubbasins.Items.Count > 0 And cboSubbasins.SelectedIndex < 0 Then
            cboSubbasins.SelectedIndex = 0
        End If
    End Sub

    Private Sub frmLandUse_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Analysis\Reclassify Land Use.html")
        End If
    End Sub
End Class
