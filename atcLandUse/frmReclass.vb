Imports atcControls
Imports atcUtility
Imports MapWinUtility
Imports atcMwGisUtility
Imports System.Drawing

Public Class frmReclass
    Inherits System.Windows.Forms.Form

    Dim pLanduseType As Long
    Dim pLULayer As String
    Dim pLanduseIDFieldName As String
    Dim pLanduseDescFieldName As String
    Dim pSubbasinsLayer As String
    Dim pSubbasinsIDFieldName As String
    Dim pSubbasinsNameFieldName As String
    Dim pLastClickedRow As Integer
    Dim cUnusedCode As Collection
    Dim cUnusedGroup As Collection
    Dim cUnusedPercentPervious As Collection
    Dim cUnusedMultiplier As Collection
    Dim cUnusedSubbasin As Collection

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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents cmdLoad As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents cmdDelete As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents sfdSave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ofdLoad As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents AtcGridLanduse As atcControls.atcGrid
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmReclass))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.cmdLoad = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.sfdSave = New System.Windows.Forms.SaveFileDialog
        Me.ofdLoad = New System.Windows.Forms.OpenFileDialog
        Me.lblProgress = New System.Windows.Forms.Label
        Me.AtcGridLanduse = New atcControls.atcGrid
        Me.lblHeader = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RadioButton2)
        Me.Panel1.Controls.Add(Me.RadioButton1)
        Me.Panel1.Location = New System.Drawing.Point(464, 8)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(208, 32)
        Me.Panel1.TabIndex = 4
        '
        'RadioButton2
        '
        Me.RadioButton2.Checked = True
        Me.RadioButton2.Location = New System.Drawing.Point(112, 8)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(96, 16)
        Me.RadioButton2.TabIndex = 5
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "Advanced"
        '
        'RadioButton1
        '
        Me.RadioButton1.Location = New System.Drawing.Point(12, 8)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(96, 16)
        Me.RadioButton1.TabIndex = 4
        Me.RadioButton1.Text = "Normal"
        '
        'cmdLoad
        '
        Me.cmdLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLoad.Location = New System.Drawing.Point(16, 464)
        Me.cmdLoad.Name = "cmdLoad"
        Me.cmdLoad.Size = New System.Drawing.Size(64, 24)
        Me.cmdLoad.TabIndex = 5
        Me.cmdLoad.Text = "&Load"
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Location = New System.Drawing.Point(96, 464)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(64, 24)
        Me.cmdSave.TabIndex = 6
        Me.cmdSave.Text = "&Save"
        '
        'cmdAdd
        '
        Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.Location = New System.Drawing.Point(584, 464)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(64, 24)
        Me.cmdAdd.TabIndex = 7
        Me.cmdAdd.Text = "&Add"
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.Location = New System.Drawing.Point(664, 464)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(64, 24)
        Me.cmdDelete.TabIndex = 8
        Me.cmdDelete.Text = "&Delete"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Location = New System.Drawing.Point(248, 464)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(88, 24)
        Me.cmdClose.TabIndex = 9
        Me.cmdClose.Text = "&Close"
        '
        'sfdSave
        '
        Me.sfdSave.DefaultExt = "dbf"
        Me.sfdSave.Filter = "DBF files (*.dbf)|*.dbf"
        Me.sfdSave.Title = "Save Reclassification File"
        '
        'ofdLoad
        '
        Me.ofdLoad.DefaultExt = "dbf"
        Me.ofdLoad.Filter = "DBF files (*.dbf)|*.dbf"
        Me.ofdLoad.Title = "Load Reclassification File"
        '
        'lblProgress
        '
        Me.lblProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgress.Location = New System.Drawing.Point(144, 32)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(480, 24)
        Me.lblProgress.TabIndex = 11
        Me.lblProgress.Text = "Processing Data ..."
        '
        'AtcGridLanduse
        '
        Me.AtcGridLanduse.AllowHorizontalScrolling = True
        Me.AtcGridLanduse.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridLanduse.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridLanduse.LineColor = System.Drawing.Color.Empty
        Me.AtcGridLanduse.LineWidth = 0.0!
        Me.AtcGridLanduse.Location = New System.Drawing.Point(24, 56)
        Me.AtcGridLanduse.Name = "AtcGridLanduse"
        Me.AtcGridLanduse.Size = New System.Drawing.Size(704, 400)
        Me.AtcGridLanduse.Source = Nothing
        Me.AtcGridLanduse.TabIndex = 12
        '
        'lblHeader
        '
        Me.lblHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.Location = New System.Drawing.Point(24, 32)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(704, 16)
        Me.lblHeader.TabIndex = 13
        Me.lblHeader.Text = "lblHeader"
        '
        'frmReclass
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(744, 496)
        Me.Controls.Add(Me.lblHeader)
        Me.Controls.Add(Me.AtcGridLanduse)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmdLoad)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmReclass"
        Me.Text = "BASINS LandUse Reclassification"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub initialize(ByVal LanduseType As Long, ByVal LULayer As String, ByVal LanduseIDFieldName As String, _
      ByVal LanduseDescFieldName As String, ByVal SubbasinsLayer As String, ByVal SubbasinsIDFieldName As String, ByVal SubbasinsNameFieldName As String)
        pLanduseType = LanduseType
        pLULayer = LULayer
        pLanduseIDFieldName = LanduseIDFieldName
        pLanduseDescFieldName = LanduseDescFieldName
        pSubbasinsLayer = SubbasinsLayer
        pSubbasinsIDFieldName = SubbasinsIDFieldName
        pSubbasinsNameFieldName = SubbasinsNameFieldName

        Panel1.Visible = False
        cmdAdd.Visible = False
        cmdDelete.Visible = False
        cmdLoad.Visible = False
        cmdSave.Visible = False
        With AtcGridLanduse
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .Visible = False
        End With
        With AtcGridLanduse.Source
            .Rows = 0
            .Columns = 7
            .CellValue(0, 0) = "Code"
            .CellValue(0, 1) = "Description"
            .CellValue(0, 2) = "Area Percent"
            .CellValue(0, 3) = "Group"
            .CellValue(0, 4) = "Impervious%"
            .CellValue(0, 5) = "Multiplier"
            .CellValue(0, 6) = "Subbasin"
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 3
            .Rows = 1
        End With
        lblHeader.Visible = False

        RadioButton1.Checked = True
    End Sub

    Public Sub FillTable()
        Dim i As Integer

        If pLanduseType = 0 Then
            SetShapefileTable("GIRAS")
        ElseIf pLanduseType = 1 Then
            SetGridTable("NLCD")
        ElseIf pLanduseType = 2 Then
            SetShapefileTable("User")
        ElseIf pLanduseType = 3 Then
            SetGridTable("User")
        End If

        AtcGridLanduse.SizeAllColumnsToContents()
        With AtcGridLanduse.Source
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .CellColor(0, 2) = SystemColors.ControlDark
            .CellColor(0, 3) = SystemColors.ControlDark
            .CellColor(0, 4) = SystemColors.ControlDark
            .CellColor(0, 5) = SystemColors.ControlDark
            .CellColor(0, 6) = SystemColors.ControlDark
            For i = 1 To .Rows - 1
                .CellColor(i, 0) = SystemColors.ControlDark
                .CellColor(i, 1) = SystemColors.ControlDark
                .CellColor(i, 2) = SystemColors.ControlDark
                .CellEditable(i, 3) = True
                .CellEditable(i, 4) = True
                .CellEditable(i, 5) = True
                .CellEditable(i, 6) = True
            Next i
        End With
        AtcGridLanduse.Visible = True
        If pSubbasinsLayer = "<none>" Then
            AtcGridLanduse.ColumnWidth(2) = 0
        End If
        AtcGridLanduse.ColumnWidth(5) = 0
        AtcGridLanduse.ColumnWidth(6) = 0
        AtcGridLanduse.Refresh()

        Panel1.Visible = True
        cmdLoad.Visible = True
        cmdSave.Visible = True
    End Sub

    Private Sub SetShapefileTable(ByVal lutype As String)
        Dim DriveLetter As String
        Dim PathName As String
        Dim NewFileName As String
        Dim ReclassifyFile As String
        Dim i As Long
        Dim j As Long
        Dim k As Long
        Dim LandUseFieldIndex As Long
        Dim DescriptionIndex As Long
        Dim inlist As Boolean
        Dim irow As Long
        Dim area As Single
        Dim totalpolygoncount As Long
        Dim polygoncount As Long
        Dim lastdisplayed As Long
        Dim tarea As Single
        Dim LanduseLayerIndex As Integer
        Dim SubbasinLayerIndex As Long
        Dim LandUseFieldName As String
        Dim DescriptionFieldName As String
        Dim lucode As String
        Dim desc As String
        Dim tmpDbf As IatcTable

        Me.Refresh()
        'set subbasin layer
        If pSubbasinsLayer <> "<none>" Then
            SubbasinLayerIndex = GisUtil.LayerIndex(pSubbasinsLayer)
        End If
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        Dim cluTiles As New Collection
        If lutype = "GIRAS" Then
            'set land use index layer
            LanduseLayerIndex = GisUtil.LayerIndex("Land Use Index")
            LandUseFieldIndex = GisUtil.FieldIndex(LanduseLayerIndex, "COVNAME")
            PathName = PathNameOnly(GisUtil.LayerFileName(LanduseLayerIndex))
            PathName = PathName & "\landuse"
            DriveLetter = Mid(PathName, 1, 1)

            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            ReclassifyFile = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
            If FileExists(ReclassifyFile) Then
                ReclassifyFile = ReclassifyFile & "giras.dbf"
            Else
                ReclassifyFile = lBasinsFolder & "\etc\giras.dbf"
            End If

            'figure out which land use tiles to list
            For i = 1 To GisUtil.NumFeatures(LanduseLayerIndex)
                'loop thru each shape of land use index shapefile
                NewFileName = GisUtil.FieldValue(LanduseLayerIndex, i - 1, LandUseFieldIndex)
                If pSubbasinsLayer <> "<none>" Then
                    'does this overlap any of our subbasins?
                    For j = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
                        If GisUtil.OverlappingPolygons(LanduseLayerIndex, i - 1, SubbasinLayerIndex, j - 1) Then
                            'yes, add it
                            cluTiles.Add(NewFileName)
                            Exit For
                        End If
                    Next j
                Else
                    cluTiles.Add(NewFileName)
                End If
            Next i

            'add tiles if not already on map
            'figure out how many polygons to overlay, for status message
            totalpolygoncount = 0
            For j = 1 To cluTiles.Count
                'loop thru each land use tile
                NewFileName = PathName & "\" & cluTiles(j) & ".shp"
                If Not GisUtil.AddLayer(NewFileName, cluTiles(j)) Then
                    Logger.Msg("The GIRAS Landuse Shapefile " & NewFileName & "does not exist." & _
                           vbCrLf & "Run the Download tool to bring this data into your project.", vbOKOnly, "Reclassify Problem")
                    Exit Sub
                End If
                totalpolygoncount = totalpolygoncount + GisUtil.NumFeatures(GisUtil.LayerIndex(cluTiles(j)))
            Next j

        Else
            'not giras, ie other shape type
            LanduseLayerIndex = GisUtil.LayerIndex(pLULayer)
            cluTiles.Add(pLULayer)
            PathName = PathNameOnly(GisUtil.LayerFileName(LanduseLayerIndex))
            DriveLetter = Mid(PathName, 1, 1)

            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            ReclassifyFile = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
            If FileExists(ReclassifyFile) Then
                ReclassifyFile = ReclassifyFile & "giras.dbf"
            Else
                ReclassifyFile = lBasinsFolder & "\etc\giras.dbf"
            End If
            totalpolygoncount = GisUtil.NumFeatures(LanduseLayerIndex)
        End If

        If pSubbasinsLayer <> "<none>" Then
            totalpolygoncount = totalpolygoncount * GisUtil.NumFeatures(SubbasinLayerIndex)
        End If

        polygoncount = 0
        lastdisplayed = 0
        For j = 1 To cluTiles.Count
            'loop thru each land use tile
            LanduseLayerIndex = GisUtil.LayerIndex(cluTiles(j))

            If lutype = "GIRAS" Then
                'find giras field index
                LandUseFieldName = "LUCODE"
                DescriptionFieldName = "LEVEL2"
            Else
                LandUseFieldName = pLanduseIDFieldName
                DescriptionFieldName = pLanduseDescFieldName
            End If
            LandUseFieldIndex = GisUtil.FieldIndex(LanduseLayerIndex, LandUseFieldName)
            DescriptionIndex = GisUtil.FieldIndex(LanduseLayerIndex, DescriptionFieldName)

            If cluTiles.Count > 1 Then
                lblProgress.Text = "Processing Data for Land Use Layer " & j & " of " & cluTiles.Count
            End If
            Me.Refresh()

            If pSubbasinsLayer <> "<none>" Then
                'do overlay

                System.Windows.Forms.Application.DoEvents()
                GisUtil.Overlay(cluTiles(j), LandUseFieldName, pSubbasinsLayer, pSubbasinsIDFieldName, _
                            PathName & "\overlay.shp", True)

                'now populate grid with results
                tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(PathName & "\overlay.dbf")

                For i = 1 To tmpDbf.NumRecords
                    tmpDbf.CurrentRecord = i
                    lucode = tmpDbf.Value(1)
                    area = tmpDbf.Value(3)
                    If Len(lucode) > 0 Then
                        'see if this type is already listed
                        inlist = False
                        For irow = 1 To AtcGridLanduse.Source.Rows
                            If AtcGridLanduse.Source.CellValue(irow, 0) = lucode Then
                                inlist = True
                                area = area + AtcGridLanduse.Source.CellValue(irow, 2)
                                AtcGridLanduse.Source.CellValue(irow, 2) = area
                                Exit For
                            End If
                        Next
                        If Not inlist Then
                            AtcGridLanduse.Source.Rows = AtcGridLanduse.Source.Rows + 1
                            'find corresponding description from land use layer
                            desc = ""
                            For k = 1 To GisUtil.NumFeatures(LanduseLayerIndex)
                                If lucode = GisUtil.FieldValue(LanduseLayerIndex, k - 1, LandUseFieldIndex) Then
                                    desc = GisUtil.FieldValue(LanduseLayerIndex, k - 1, DescriptionIndex)
                                    Exit For
                                End If
                            Next k
                            'now put in grid
                            AtcGridLanduse.Source.CellValue(AtcGridLanduse.Source.Rows - 1, 0) = lucode
                            AtcGridLanduse.Source.CellValue(AtcGridLanduse.Source.Rows - 1, 1) = desc
                            AtcGridLanduse.Source.CellValue(AtcGridLanduse.Source.Rows - 1, 2) = area
                        End If

                    End If
                Next i
            Else
                'no subbasin layer, include all land use codes
                For i = 1 To GisUtil.NumFeatures(LanduseLayerIndex)
                    lucode = GisUtil.FieldValue(LanduseLayerIndex, i - 1, LandUseFieldIndex)
                    desc = GisUtil.FieldValue(LanduseLayerIndex, i - 1, DescriptionIndex)
                    If Len(lucode) > 0 Then
                        'see if this type is already listed
                        inlist = False
                        For irow = 1 To AtcGridLanduse.Source.Rows
                            If AtcGridLanduse.Source.CellValue(irow, 0) = lucode Then
                                inlist = True
                                Exit For
                            End If
                        Next
                        If Not inlist Then
                            System.Windows.Forms.Application.DoEvents()
                            AtcGridLanduse.Source.Rows = AtcGridLanduse.Source.Rows + 1
                            AtcGridLanduse.Source.CellValue(AtcGridLanduse.Source.Rows - 1, 0) = lucode
                            AtcGridLanduse.Source.CellValue(AtcGridLanduse.Source.Rows - 1, 1) = desc
                        End If
                    End If
                Next i
            End If

        Next j

        If pSubbasinsLayer <> "<none>" Then
            'go thru area column and convert to percents
            tarea = 0
            For i = 2 To AtcGridLanduse.Source.Rows
                If Len(AtcGridLanduse.Source.CellValue(i - 1, 2)) = 0 Then
                    AtcGridLanduse.Source.CellValue(i - 1, 2) = 0
                End If
                tarea = tarea + AtcGridLanduse.Source.CellValue(i - 1, 2)
            Next
            For i = 2 To AtcGridLanduse.Source.Rows
                AtcGridLanduse.Source.CellValue(i - 1, 2) = (CInt(AtcGridLanduse.Source.CellValue(i - 1, 2) / tarea * 10000) / 100)
            Next
        End If

        AddGroupedClassifications(ReclassifyFile, lutype)

        AtcGridLanduse.Source = SortSourceByIntegerColumn(AtcGridLanduse.Source, 0)

        lblProgress.Visible = False
    End Sub

    Private Sub SetGridTable(ByVal lutype As String)
        Dim i As Long
        Dim k As Long
        Dim ReclassifyFile As String
        Dim aAreaLS(0, 0) As Double
        Dim area As Single
        Dim tarea As Single
        Dim numSubbasins As Long
        Dim SubbasinsLayerIndex As Long
        Dim LanduseGridLayerIndex As Integer

        Me.Refresh()
        'set subbasin shape file
        If pSubbasinsLayer <> "<none>" Then
            SubbasinsLayerIndex = GisUtil.LayerIndex(pSubbasinsLayer)
        End If

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        ReclassifyFile = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
        If FileExists(ReclassifyFile) Then
            ReclassifyFile = ReclassifyFile & "nlcd.dbf"
        Else
            ReclassifyFile = lBasinsFolder & "\etc\nlcd.dbf"
        End If

        AtcGridLanduse.Source.Rows = 1
        LanduseGridLayerIndex = GisUtil.LayerIndex(pLULayer)
        If GisUtil.LayerType(LanduseGridLayerIndex) = 4 Then
            'Grid

            If pSubbasinsLayer <> "<none>" Then
                'have subbasins specified
                numSubbasins = GisUtil.NumFeatures(SubbasinsLayerIndex)
                ReDim aAreaLS(Convert.ToInt32(GisUtil.GridLayerMaximum(LanduseGridLayerIndex)), numSubbasins)
                GisUtil.TabulateAreas(LanduseGridLayerIndex, SubbasinsLayerIndex, aAreaLS)
            Else
                numSubbasins = 1
            End If

            For i = Convert.ToInt32(GisUtil.GridLayerMinimum(LanduseGridLayerIndex)) To Convert.ToInt32(GisUtil.GridLayerMaximum(LanduseGridLayerIndex))
                area = 0
                If pSubbasinsLayer <> "<none>" Then
                    For k = 1 To GisUtil.NumFeatures(SubbasinsLayerIndex)
                        'loop thru subbasins
                        If aAreaLS(i, k - 1) > 0 Then
                            area = area + aAreaLS(i, k - 1)
                        End If
                    Next k
                    If area > 0 Then
                        With AtcGridLanduse.Source
                            .Rows = .Rows + 1
                            .CellValue(.Rows - 1, 0) = i
                            .CellValue(.Rows - 1, 2) = area
                        End With
                    End If
                Else
                    'no subbasin layer, add all categories
                    With AtcGridLanduse.Source
                        .Rows = .Rows + 1
                        .CellValue(.Rows - 1, 0) = i
                    End With
                End If
            Next i

            If pSubbasinsLayer <> "<none>" Then
                'go thru area column and convert to percents
                tarea = 0
                For i = 2 To AtcGridLanduse.Source.Rows
                    If Len(AtcGridLanduse.Source.CellValue(i - 1, 2)) = 0 Then
                        AtcGridLanduse.Source.CellValue(i - 1, 2) = 0
                    End If
                    tarea = tarea + AtcGridLanduse.Source.CellValue(i - 1, 2)
                Next
                For i = 2 To AtcGridLanduse.Source.Rows
                    AtcGridLanduse.Source.CellValue(i - 1, 2) = (CInt(AtcGridLanduse.Source.CellValue(i - 1, 2) / tarea * 10000) / 100)
                Next
            Else
                AtcGridLanduse.ColumnWidth(2) = 0
            End If

            If lutype = "NLCD" Then
                'add mrlc classifications

                lBasinsBinLoc = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
                ReclassifyFile = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
                If InStr(UCase(GisUtil.LayerName(LanduseGridLayerIndex)), "2001") > 0 Then
                    If FileExists(ReclassifyFile) Then
                        ReclassifyFile = ReclassifyFile & "mrlc2001.dbf"
                    Else
                        ReclassifyFile = lBasinsFolder & "\etc\mrlc2001.dbf"
                    End If
                Else
                    If FileExists(ReclassifyFile) Then
                        ReclassifyFile = ReclassifyFile & "mrlc.dbf"
                    Else
                        ReclassifyFile = lBasinsFolder & "\etc\mrlc.dbf"
                    End If
                End If

                Dim tmpDbf As IatcTable
                tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(ReclassifyFile)
                For k = 1 To AtcGridLanduse.Source.Rows
                    For i = 1 To tmpDbf.NumRecords
                        tmpDbf.CurrentRecord = i
                        If AtcGridLanduse.Source.CellValue(k, 0) = tmpDbf.Value(1) Then
                            AtcGridLanduse.Source.CellValue(k, 1) = tmpDbf.Value(2)
                            Exit For
                        End If
                    Next i
                Next k
            Else
                'dont know descriptions for other types
            End If

            'add grouped classifications
            lBasinsBinLoc = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            ReclassifyFile = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
            If FileExists(ReclassifyFile) Then
                ReclassifyFile = ReclassifyFile & "nlcd.dbf"
            Else
                ReclassifyFile = lBasinsFolder & "\etc\nlcd.dbf"
            End If
            AddGroupedClassifications(ReclassifyFile, lutype)
        End If

        lblProgress.Visible = False
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            AtcGridLanduse.SizeAllColumnsToContents()
            If pSubbasinsLayer = "<none>" Then
                AtcGridLanduse.ColumnWidth(2) = 0
            End If
            AtcGridLanduse.ColumnWidth(5) = 0
            AtcGridLanduse.ColumnWidth(6) = 0
            cmdAdd.Visible = False
            cmdDelete.Visible = False
        Else
            If pSubbasinsLayer = "<none>" Then
                AtcGridLanduse.ColumnWidth(2) = 0
            End If
            AtcGridLanduse.ColumnWidth(5) = 10
            AtcGridLanduse.ColumnWidth(6) = 10
            cmdAdd.Visible = True
            cmdDelete.Visible = True
            AtcGridLanduse.SizeAllColumnsToContents()
        End If
        AtcGridLanduse.Refresh()
    End Sub

    'Private Sub agdLanduse_RowColChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '  Dim i As Long, j As Long
    '  Dim cVals As Collection
    '  Dim inlist As Boolean

    'todo:  implement dropdown list for strings
    'With agdLanduse
    '  If .col = 3 Then
    '    .ClearValues()
    '    cVals = New Collection
    '    For i = 1 To .rows
    '      inlist = False
    '      For j = 1 To cVals.Count
    '        If cVals(j) = .get_TextMatrix(i, 3) Then
    '          inlist = True
    '          Exit For
    '        End If
    '      Next j
    '      If Not inlist Then
    '        cVals.Add(.get_TextMatrix(i, 3))
    '      End If
    '    Next i
    '    For i = 1 To cVals.Count
    '      .addValue(cVals(i))
    '    Next i
    '  Else
    '    .ClearValues()
    '  End If
    'End With
    'End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If pLastClickedRow > 0 Then
            AddRowToGrid(pLastClickedRow)
        End If
    End Sub

    Private Sub AddRowToGrid(ByVal aAddAfterRow As Integer)
        Dim i As Long, j As Long

        AtcGridLanduse.Source.Rows = AtcGridLanduse.Source.Rows + 1
        For i = AtcGridLanduse.Source.Rows To aAddAfterRow + 2 Step -1
            For j = 0 To AtcGridLanduse.Source.Columns - 1
                AtcGridLanduse.Source.CellValue(i - 1, j) = AtcGridLanduse.Source.CellValue(i - 2, j)
                AtcGridLanduse.Source.CellColor(i - 1, j) = AtcGridLanduse.Source.CellColor(i - 2, j)
                AtcGridLanduse.Source.CellEditable(i - 1, j) = AtcGridLanduse.Source.CellEditable(i - 2, j)
            Next j
        Next i
        AtcGridLanduse.Refresh()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        If pLastClickedRow > 0 Then
            DeleteRowFromGrid(pLastClickedRow)
        End If
    End Sub

    Private Sub DeleteRowFromGrid(ByVal aDeleteRow As Integer)
        Dim i As Long, j As Long

        For i = aDeleteRow + 1 To AtcGridLanduse.Source.Rows
            For j = 0 To AtcGridLanduse.Source.Columns - 1
                AtcGridLanduse.Source.CellValue(i - 1, j) = AtcGridLanduse.Source.CellValue(i, j)
                AtcGridLanduse.Source.CellColor(i - 1, j) = AtcGridLanduse.Source.CellColor(i, j)
                AtcGridLanduse.Source.CellEditable(i - 1, j) = AtcGridLanduse.Source.CellEditable(i, j)
            Next j
        Next i
        AtcGridLanduse.Source.Rows = AtcGridLanduse.Source.Rows - 1
        AtcGridLanduse.Refresh()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dbfname As String
        Dim tmpDbf As IatcTable
        Dim i As Long
        Dim baserow As Long

        If sfdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
            dbfname = sfdSave.FileName
            'does this dbf already exist?
            If FileExists(dbfname) Then
                'delete this file first
                TryDelete(dbfname)
            End If
            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(dbfname)
            tmpDbf.NumFields = 5
            tmpDbf.FieldName(1) = "Value"
            tmpDbf.FieldType(1) = "N"
            tmpDbf.FieldLength(1) = 10
            tmpDbf.FieldName(2) = "Landuse"
            tmpDbf.FieldType(2) = "C"
            tmpDbf.FieldLength(2) = 30
            tmpDbf.FieldName(3) = "Impervious%"
            tmpDbf.FieldType(3) = "C"
            tmpDbf.FieldLength(3) = 10
            tmpDbf.FieldName(4) = "Multiplier"
            tmpDbf.FieldType(4) = "C"
            tmpDbf.FieldLength(4) = 10
            tmpDbf.FieldName(5) = "Subbasin"
            tmpDbf.FieldType(5) = "C"
            tmpDbf.FieldLength(5) = 10

            baserow = 0
            'write out hidden portion for unused classifications
            For i = 1 To cUnusedCode.Count
                tmpDbf.CurrentRecord = i
                tmpDbf.Value(1) = cUnusedCode(i)
                tmpDbf.Value(2) = cUnusedGroup(i)
                tmpDbf.Value(3) = cUnusedPercentPervious(i)
                tmpDbf.Value(4) = cUnusedMultiplier(i)
                tmpDbf.Value(5) = cUnusedSubbasin(i)
                baserow = baserow + 1
            Next i

            'now write out main grid
            For i = 1 To AtcGridLanduse.Source.Rows - 1
                tmpDbf.CurrentRecord = i + baserow
                tmpDbf.Value(1) = AtcGridLanduse.Source.CellValue(i, 0)
                tmpDbf.Value(2) = AtcGridLanduse.Source.CellValue(i, 3)
                tmpDbf.Value(3) = AtcGridLanduse.Source.CellValue(i, 4)
                If Trim(AtcGridLanduse.Source.CellValue(i, 5)) = "1" Then
                    'write blank instead
                    tmpDbf.Value(4) = ""
                Else
                    tmpDbf.Value(4) = AtcGridLanduse.Source.CellValue(i, 5)
                End If
                If AtcGridLanduse.Source.CellValue(i, 6) = "<all>" Then
                    tmpDbf.Value(5) = ""
                Else
                    tmpDbf.Value(5) = AtcGridLanduse.Source.CellValue(i, 6)
                End If
            Next i

            tmpDbf.WriteFile(dbfname)

        End If
    End Sub

    Private Sub StoreUnusedClassifications(ByVal tmpDbf As IatcTable)
        Dim i As Long, j As Long
        Dim inlist As Boolean

        cUnusedCode = New Collection
        cUnusedGroup = New Collection
        cUnusedPercentPervious = New Collection
        cUnusedMultiplier = New Collection
        cUnusedSubbasin = New Collection

        For i = 1 To tmpDbf.NumRecords
            tmpDbf.CurrentRecord = i
            'look for codes that did not get used
            inlist = False
            For j = 1 To AtcGridLanduse.Source.Rows - 1
                If tmpDbf.Value(1) = AtcGridLanduse.Source.CellValue(j, 0) Then
                    'this one was used
                    inlist = True
                    Exit For
                End If
            Next j
            If Not inlist Then
                cUnusedCode.Add(tmpDbf.Value(1))
                cUnusedGroup.Add(tmpDbf.Value(2))
                cUnusedPercentPervious.Add(tmpDbf.Value(3))
                If tmpDbf.NumFields > 3 Then
                    cUnusedMultiplier.Add(tmpDbf.Value(4))
                Else
                    cUnusedMultiplier.Add("")
                End If
                If tmpDbf.NumFields > 4 Then
                    cUnusedSubbasin.Add(tmpDbf.Value(5))
                Else
                    cUnusedSubbasin.Add("")
                End If
            End If
        Next i
    End Sub

    Private Sub AddGroupedClassifications(ByVal ReclassifyFile As String, ByVal lutype As String)
        Dim i As Long, k As Long
        Dim tmpDbf As IatcTable
        Dim tmult As Single
        Dim tint As Long
        Dim foundvalue As Boolean

        tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(ReclassifyFile)

        k = 1
        Do While k <= AtcGridLanduse.Source.Rows
            'For k = 1 To agdLanduse.rows
            foundvalue = False
            For i = 1 To tmpDbf.NumRecords
                tmpDbf.CurrentRecord = i
                If AtcGridLanduse.Source.CellValue(k, 0) = tmpDbf.Value(1) Then

                    If foundvalue Then
                        'already found one of these, add a row to table
                        AddRowToGrid(k)
                        k = k + 1
                        AtcGridLanduse.Source.CellValue(k, 4) = ""
                        AtcGridLanduse.Source.CellValue(k, 5) = ""
                        AtcGridLanduse.Source.CellValue(k, 6) = ""
                    End If

                    AtcGridLanduse.Source.CellValue(k, 3) = tmpDbf.Value(2)

                    If tmpDbf.NumFields > 2 Then
                        'populate pervious field
                        AtcGridLanduse.Source.CellValue(k, 4) = tmpDbf.Value(3)
                    End If

                    If tmpDbf.NumFields > 3 Then
                        If IsNumeric(tmpDbf.Value(4)) Then
                            tmult = tmpDbf.Value(4)
                            'use this as multiplier
                            AtcGridLanduse.Source.CellValue(k, 5) = tmult
                        Else
                            AtcGridLanduse.Source.CellValue(k, 5) = 1
                        End If
                    End If

                    If tmpDbf.NumFields > 4 Then
                        If IsNumeric(tmpDbf.Value(5)) Then
                            tint = tmpDbf.Value(5)
                            'use this as subbasin field
                            AtcGridLanduse.Source.CellValue(k, 6) = tint
                        Else
                            AtcGridLanduse.Source.CellValue(k, 6) = "<all>"
                        End If
                    End If

                    foundvalue = True

                End If

            Next i

            If Len(AtcGridLanduse.Source.CellValue(k - 1, 5)) = 0 Then
                'populate multiplier
                AtcGridLanduse.Source.CellValue(k - 1, 5) = 1
            End If

            If Len(AtcGridLanduse.Source.CellValue(k - 1, 6)) = 0 Then
                'populate subbasin field
                AtcGridLanduse.Source.CellValue(k - 1, 6) = "<all>"
            End If

            k = k + 1
        Loop

        If lutype = "NLCD" And pSubbasinsLayer = "<none>" Then
            'display only those known to be valid NLCD types
            k = 1
            Do While k <= AtcGridLanduse.Source.Rows
                If Len(AtcGridLanduse.Source.CellValue(k - 1, 1)) = 0 Then
                    DeleteRowFromGrid(k - 1)
                Else
                    k = k + 1
                End If
            Loop
        End If

        'set appropriate header 
        If pSubbasinsLayer <> "<none>" Then
            lblHeader.Text = lutype & " classes within layer " & pSubbasinsLayer & " (grouped by " & FilenameNoPath(ReclassifyFile) & ")"
        Else
            If pLanduseType = 0 Or pLanduseType = 2 Then
                'giras or other shape
                lblHeader.Text = lutype & " classes in current project (grouped by " & FilenameNoPath(ReclassifyFile) & ")"
            ElseIf pLanduseType = 1 Or pLanduseType = 3 Then
                'nlcd grid or other grid
                lblHeader.Text = "All " & lutype & " classes (grouped by " & FilenameNoPath(ReclassifyFile) & ")"
            End If
        End If
        lblHeader.Visible = True
        StoreUnusedClassifications(tmpDbf)
    End Sub

    Private Sub cmdLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoad.Click
        Dim dbfname As String
        Dim lutype As String

        If pLanduseType = 0 Then
            lutype = "GIRAS"
        ElseIf pLanduseType = 1 Then
            lutype = "NLCD"
        Else
            lutype = "User"
        End If
        If ofdLoad.ShowDialog() = Windows.Forms.DialogResult.OK Then
            dbfname = ofdLoad.FileName
            AddGroupedClassifications(dbfname, lutype)
            AtcGridLanduse.Refresh()
        End If
    End Sub

    Private Function SortSourceByIntegerColumn(ByVal aSource As atcGridSource, ByVal aColumn As Integer) As atcGridSource
        Dim lSortedRows As New atcCollection
        Dim lOldRow As Integer
        Dim lNewRow As Integer
        Dim lColumns As Integer = aSource.Columns
        Dim lRows As Integer = aSource.Rows
        Dim lColorCells As Boolean = aSource.ColorCells

        For lOldRow = aSource.FixedRows To aSource.Rows - 1
            lSortedRows.Add(CInt(aSource.CellValue(lOldRow, aColumn)), lOldRow)
        Next
        lSortedRows.Sort()

        SortSourceByIntegerColumn = New atcGridSource
        With SortSourceByIntegerColumn
            .Rows = lRows
            .Columns = lColumns
            .FixedRows = aSource.FixedRows
            .FixedColumns = aSource.FixedColumns
            .ColorCells = lColorCells

            For lNewRow = 0 To lRows - 1
                If lNewRow < .FixedRows Then
                    lOldRow = lNewRow
                Else
                    lOldRow = lSortedRows.ItemByIndex(lNewRow - .FixedRows)
                End If
                For lColumn As Integer = 0 To lColumns - 1
                    .CellValue(lNewRow, lColumn) = aSource.CellValue(lOldRow, lColumn)
                    If lColorCells Then
                        .CellColor(lNewRow, lColumn) = aSource.CellColor(lOldRow, lColumn)
                    End If
                    .CellEditable(lNewRow, lColumn) = aSource.CellEditable(lOldRow, lColumn)
                    .Alignment(lNewRow, lColumn) = aSource.Alignment(lOldRow, lColumn)
                    .CellSelected(lNewRow, lColumn) = aSource.CellSelected(lOldRow, lColumn)
                Next
            Next
        End With
    End Function

    Private Sub AtcGridLanduse_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridLanduse.MouseDownCell
        Dim lUniqueValues As New ArrayList
        pLastClickedRow = aRow
        If aColumn = 3 Then
            With aGrid.Source
                For lRow As Integer = 1 To .Rows - 1
                    Dim lRowValue As String = .CellValue(lRow, 3)
                    If Not lRowValue Is Nothing AndAlso Not lUniqueValues.Contains(lRowValue) Then
                        lUniqueValues.Add(lRowValue)
                    End If
                Next
            End With
            aGrid.AllowNewValidValues = True
        Else
            aGrid.AllowNewValidValues = False
        End If
        aGrid.ValidValues = lUniqueValues
    End Sub

    Private Sub frmReclass_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Analysis\Reclassify Land Use.html")
        End If
    End Sub
End Class
