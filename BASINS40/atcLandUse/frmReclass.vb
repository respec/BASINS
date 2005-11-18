Imports atcUtility
Imports atcMwGisUtility.GISUtils

Public Class frmReclass
  Inherits System.Windows.Forms.Form

  Dim pLanduseType As Long
  Dim pLULayer As String
  Dim pLanduseIDFieldName As String
  Dim pLanduseDescFieldName As String
  Dim pSubbasinsLayer As String
  Dim pSubbasinsIDFieldName As String
  Dim pSubbasinsNameFieldName As String

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
  Friend WithEvents agdLanduse As AxATCoCtl.AxATCoGrid
  Friend WithEvents Panel1 As System.Windows.Forms.Panel
  Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
  Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
  Friend WithEvents cmdLoad As System.Windows.Forms.Button
  Friend WithEvents cmdSave As System.Windows.Forms.Button
  Friend WithEvents cmdAdd As System.Windows.Forms.Button
  Friend WithEvents cmdDelete As System.Windows.Forms.Button
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents sfdSave As System.Windows.Forms.SaveFileDialog
  Friend WithEvents agdHidden As AxATCoCtl.AxATCoGrid
  Friend WithEvents ofdLoad As System.Windows.Forms.OpenFileDialog
  Friend WithEvents lblProgress As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmReclass))
    Me.agdLanduse = New AxATCoCtl.AxATCoGrid
    Me.Panel1 = New System.Windows.Forms.Panel
    Me.RadioButton2 = New System.Windows.Forms.RadioButton
    Me.RadioButton1 = New System.Windows.Forms.RadioButton
    Me.cmdLoad = New System.Windows.Forms.Button
    Me.cmdSave = New System.Windows.Forms.Button
    Me.cmdAdd = New System.Windows.Forms.Button
    Me.cmdDelete = New System.Windows.Forms.Button
    Me.cmdClose = New System.Windows.Forms.Button
    Me.sfdSave = New System.Windows.Forms.SaveFileDialog
    Me.agdHidden = New AxATCoCtl.AxATCoGrid
    Me.ofdLoad = New System.Windows.Forms.OpenFileDialog
    Me.lblProgress = New System.Windows.Forms.Label
    CType(Me.agdLanduse, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.Panel1.SuspendLayout()
    CType(Me.agdHidden, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'agdLanduse
    '
    Me.agdLanduse.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdLanduse.Enabled = True
    Me.agdLanduse.Location = New System.Drawing.Point(24, 24)
    Me.agdLanduse.Name = "agdLanduse"
    Me.agdLanduse.OcxState = CType(resources.GetObject("agdLanduse.OcxState"), System.Windows.Forms.AxHost.State)
    Me.agdLanduse.Size = New System.Drawing.Size(704, 432)
    Me.agdLanduse.TabIndex = 0
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
    Me.cmdLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdLoad.Location = New System.Drawing.Point(16, 464)
    Me.cmdLoad.Name = "cmdLoad"
    Me.cmdLoad.Size = New System.Drawing.Size(64, 24)
    Me.cmdLoad.TabIndex = 5
    Me.cmdLoad.Text = "&Load"
    '
    'cmdSave
    '
    Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdSave.Location = New System.Drawing.Point(96, 464)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Size = New System.Drawing.Size(64, 24)
    Me.cmdSave.TabIndex = 6
    Me.cmdSave.Text = "&Save"
    '
    'cmdAdd
    '
    Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdAdd.Location = New System.Drawing.Point(584, 464)
    Me.cmdAdd.Name = "cmdAdd"
    Me.cmdAdd.Size = New System.Drawing.Size(64, 24)
    Me.cmdAdd.TabIndex = 7
    Me.cmdAdd.Text = "&Add"
    '
    'cmdDelete
    '
    Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdDelete.Location = New System.Drawing.Point(664, 464)
    Me.cmdDelete.Name = "cmdDelete"
    Me.cmdDelete.Size = New System.Drawing.Size(64, 24)
    Me.cmdDelete.TabIndex = 8
    Me.cmdDelete.Text = "&Delete"
    '
    'cmdClose
    '
    Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
    'agdHidden
    '
    Me.agdHidden.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdHidden.Enabled = True
    Me.agdHidden.Location = New System.Drawing.Point(136, 136)
    Me.agdHidden.Name = "agdHidden"
    Me.agdHidden.OcxState = CType(resources.GetObject("agdHidden.OcxState"), System.Windows.Forms.AxHost.State)
    Me.agdHidden.Size = New System.Drawing.Size(512, 248)
    Me.agdHidden.TabIndex = 10
    Me.agdHidden.Visible = False
    '
    'ofdLoad
    '
    Me.ofdLoad.DefaultExt = "dbf"
    Me.ofdLoad.Filter = "DBF files (*.dbf)|*.dbf"
    Me.ofdLoad.Title = "Load Reclassification File"
    '
    'lblProgress
    '
    Me.lblProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblProgress.Location = New System.Drawing.Point(144, 32)
    Me.lblProgress.Name = "lblProgress"
    Me.lblProgress.Size = New System.Drawing.Size(480, 24)
    Me.lblProgress.TabIndex = 11
    Me.lblProgress.Text = "Processing Data ..."
    '
    'frmReclass
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(744, 496)
    Me.Controls.Add(Me.lblProgress)
    Me.Controls.Add(Me.agdHidden)
    Me.Controls.Add(Me.cmdClose)
    Me.Controls.Add(Me.cmdDelete)
    Me.Controls.Add(Me.cmdAdd)
    Me.Controls.Add(Me.cmdSave)
    Me.Controls.Add(Me.cmdLoad)
    Me.Controls.Add(Me.Panel1)
    Me.Controls.Add(Me.agdLanduse)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmReclass"
    Me.Text = "BASINS LandUse Reclassification"
    CType(Me.agdLanduse, System.ComponentModel.ISupportInitialize).EndInit()
    Me.Panel1.ResumeLayout(False)
    CType(Me.agdHidden, System.ComponentModel.ISupportInitialize).EndInit()
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

    agdLanduse.Visible = False
    Panel1.Visible = False
    cmdAdd.Visible = False
    cmdDelete.Visible = False
    cmdLoad.Visible = False
    cmdSave.Visible = False
    With agdLanduse
      .set_ColTitle(0, "Code")
      .set_ColTitle(1, "Description")
      .set_ColTitle(2, "Area Percent")
      .set_ColTitle(3, "Group")
      .set_ColTitle(4, "% Pervious")
      .set_ColTitle(5, "Multiplier")
      .set_ColTitle(6, "Subbasin")
      .FixedCols = 3
      .set_ColEditable(3, True)
      .set_ColType(4, 2)
      .set_ColEditable(4, True)
      .set_ColMax(4, 100)
      .set_ColMin(4, 0)
      .set_ColType(5, 2)
      .set_ColEditable(5, True)
      .set_ColEditable(6, True)
    End With
    With agdHidden
      .set_ColTitle(0, "Code")
      .set_ColTitle(1, "Group")
      .set_ColTitle(2, "% Pervious")
      .set_ColTitle(3, "Multiplier")
      .set_ColTitle(4, "Subbasin")
    End With
    RadioButton1.Checked = True
  End Sub

  Public Sub FillTable()
    If pLanduseType = 0 Then
      SetShapefileTable("GIRAS")
    ElseIf pLanduseType = 1 Then
      SetGridTable("NLCD")
    ElseIf pLanduseType = 2 Then
      SetShapefileTable("User")
    ElseIf pLanduseType = 3 Then
      SetGridTable("User")
    End If
    agdLanduse.Visible = True
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
    Dim LanduseLayerIndex As Long
    Dim SubbasinLayerIndex As Long
    Dim LandUseFieldName As String
    Dim DescriptionFieldName As String
    Dim lucode As String
    Dim desc As String
    Dim tmpDbf As IATCTable

    Me.Refresh()
    'set subbasin layer
    If pSubbasinsLayer <> "<none>" Then
      SubbasinLayerIndex = GisUtil_FindLayerIndexByName(pSubbasinsLayer)
    End If

    Dim cluTiles As New Collection
    If lutype = "GIRAS" Then
      'set land use index layer
      LanduseLayerIndex = GisUtil_FindLayerIndexByName("Land Use Index")
      LandUseFieldIndex = GisUtil_FindFieldIndexByName(LanduseLayerIndex, "COVNAME")
      PathName = PathNameOnly(GisUtil_LayerFileName(LanduseLayerIndex))
      PathName = PathName & "\landuse"
      DriveLetter = Mid(PathName, 1, 1)
      ReclassifyFile = DriveLetter & ":\basins\etc\giras.dbf"

      'figure out which land use tiles to list
      For i = 1 To GisUtil_NumFeaturesInLayer(LanduseLayerIndex)
        'loop thru each shape of land use index shapefile
        NewFileName = GisUtil_CellValueNthFeatureInLayer(LanduseLayerIndex, LandUseFieldIndex, i - 1)
        If pSubbasinsLayer <> "<none>" Then
          'does this overlap any of our subbasins?
          For j = 1 To GisUtil_NumFeaturesInLayer(SubbasinLayerIndex)
            If GisUtil_OverlappingPolygons(LanduseLayerIndex, i - 1, SubbasinLayerIndex, j - 1) Then
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
        If Not GisUtil_AddLayerToMap(NewFileName, cluTiles(j)) Then
          MsgBox("The GIRAS Landuse Shapefile " & NewFileName & "does not exist." & _
                 vbCrLf & "Run the Download tool to bring this data into your project.", vbOKOnly, "Reclassify Problem")
          Exit Sub
        End If
        totalpolygoncount = totalpolygoncount + GisUtil_NumFeaturesInLayer(GisUtil_FindLayerIndexByName(cluTiles(j)))
      Next j

    Else
      'not giras, ie other shape type
      LanduseLayerIndex = GisUtil_FindLayerIndexByName(pLULayer)
      cluTiles.Add(pLULayer)
      PathName = PathNameOnly(GisUtil_LayerFileName(LanduseLayerIndex))
      DriveLetter = Mid(PathName, 1, 1)
      ReclassifyFile = DriveLetter & ":\basins\etc\giras.dbf"
      totalpolygoncount = GisUtil_NumFeaturesInLayer(LanduseLayerIndex)
    End If

    If pSubbasinsLayer <> "<none>" Then
      totalpolygoncount = totalpolygoncount * GisUtil_NumFeaturesInLayer(SubbasinLayerIndex)
    End If

    agdLanduse.rows = 0
    polygoncount = 0
    lastdisplayed = 0
    For j = 1 To cluTiles.Count
      'loop thru each land use tile
      LanduseLayerIndex = GisUtil_FindLayerIndexByName(cluTiles(j))

      If lutype = "GIRAS" Then
        'find giras field index
        LandUseFieldName = "LUCODE"
        DescriptionFieldName = "LEVEL2"
      Else
        LandUseFieldName = pLanduseIDFieldName
        DescriptionFieldName = pLanduseDescFieldName
      End If
      LandUseFieldIndex = GisUtil_FindFieldIndexByName(LanduseLayerIndex, LandUseFieldName)
      DescriptionIndex = GisUtil_FindFieldIndexByName(LanduseLayerIndex, DescriptionFieldName)

      If cluTiles.Count > 1 Then
        lblProgress.Text = "Processing Data for Land Use Layer " & j & " of " & cluTiles.Count
      End If
      Me.Refresh()

      With agdLanduse
        If pSubbasinsLayer <> "<none>" Then
          'do overlay

          GisUtil_Overlay(cluTiles(j), LandUseFieldName, pSubbasinsLayer, pSubbasinsIDFieldName, _
                      PathName & "\overlay.shp", True)

          'now populate grid with results
          tmpDbf = atcUtility.TableOpener.OpenAnyTable(PathName & "\overlay.dbf")

          For i = 1 To tmpDbf.NumRecords
            tmpDbf.CurrentRecord = i
            lucode = tmpDbf.Value(1)
            area = tmpDbf.Value(3)
            If Len(lucode) > 0 Then
              'see if this type is already listed
              inlist = False
              For irow = 1 To agdLanduse.rows
                If .get_TextMatrix(irow, 0) = lucode Then
                  inlist = True
                  area = area + .get_TextMatrix(irow, 2)
                  .set_TextMatrix(irow, 2, area)
                  Exit For
                End If
              Next
              If Not inlist Then
                .rows = .rows + 1
                'find corresponding description from land use layer
                desc = ""
                For k = 1 To GisUtil_NumFeaturesInLayer(LanduseLayerIndex)
                  If lucode = GisUtil_CellValueNthFeatureInLayer(LanduseLayerIndex, LandUseFieldIndex, k - 1) Then
                    desc = GisUtil_CellValueNthFeatureInLayer(LanduseLayerIndex, DescriptionIndex, k - 1)
                    Exit For
                  End If
                Next k
                'now put in grid
                .set_TextMatrix(.rows, 0, lucode)
                .set_TextMatrix(.rows, 1, desc)
                .set_TextMatrix(.rows, 2, area)
              End If
            End If
          Next i
        Else
          'no subbasin layer, include all land use codes
          .set_ColTitle(2, "HIDE")
          For i = 1 To GisUtil_NumFeaturesInLayer(LanduseLayerIndex)
            lucode = GisUtil_CellValueNthFeatureInLayer(LanduseLayerIndex, LandUseFieldIndex, i - 1)
            desc = GisUtil_CellValueNthFeatureInLayer(LanduseLayerIndex, DescriptionIndex, i - 1)
            If Len(lucode) > 0 Then
              'see if this type is already listed
              inlist = False
              For irow = 1 To agdLanduse.rows
                If .get_TextMatrix(irow, 0) = lucode Then
                  inlist = True
                  Exit For
                End If
              Next
              If Not inlist Then
                .rows = .rows + 1
                .set_TextMatrix(.rows, 0, lucode)
                .set_TextMatrix(.rows, 1, desc)
              End If
            End If
          Next i
        End If

      End With
    Next j

    If pSubbasinsLayer <> "<none>" Then
      'go thru area column and convert to percents
      tarea = 0
      For i = 1 To agdLanduse.rows
        tarea = tarea + agdLanduse.get_TextMatrix(i, 2)
      Next
      For i = 1 To agdLanduse.rows
        agdLanduse.set_TextMatrix(i, 2, (CInt(agdLanduse.get_TextMatrix(i, 2) / tarea * 10000) / 100))
      Next
    End If

    AddGroupedClassifications(ReclassifyFile, lutype)

    agdLanduse.Sort(0, True)
    agdLanduse.ColsSizeByContents()
    lblProgress.Visible = False

  End Sub

  Private Sub SetGridTable(ByVal lutype As String)
    Dim i As Long
    Dim j As Long
    Dim k As Long
    Dim ReclassifyFile As String
    Dim aAreaLS(,) As Double
    Dim subid As Long
    Dim area As Single
    Dim tarea As Single
    Dim numSubbasins As Long
    Dim SubbasinsLayerIndex As Long
    Dim LanduseGridLayerIndex As Long

    Me.Refresh()
    'set subbasin shape file
    If pSubbasinsLayer <> "<none>" Then
      SubbasinsLayerIndex = GisUtil_FindLayerIndexByName(pSubbasinsLayer)
    End If

    ReclassifyFile = "\BASINS\etc\nlcd.dbf"

    agdLanduse.rows = 0
    LanduseGridLayerIndex = GisUtil_FindLayerIndexByName(pLULayer)
    If GisUtil_LayerType(LanduseGridLayerIndex) = 4 Then
      'Grid

      If pSubbasinsLayer <> "<none>" Then
        'have subbasins specified
        numSubbasins = GisUtil_NumFeaturesInLayer(SubbasinsLayerIndex)
        ReDim aAreaLS(GisUtil_GridLayerMaximum(LanduseGridLayerIndex), numSubbasins)
        GisUtil_TabulateAreas(LanduseGridLayerIndex, SubbasinsLayerIndex, aAreaLS)
      Else
        numSubbasins = 1
      End If

      For i = GisUtil_GridLayerMinimum(LanduseGridLayerIndex) To GisUtil_GridLayerMaximum(LanduseGridLayerIndex)
        area = 0
        If pSubbasinsLayer <> "<none>" Then
          For k = 1 To GisUtil_NumFeaturesInLayer(SubbasinsLayerIndex)
            'loop thru subbasins
            If aAreaLS(i, k - 1) > 0 Then
              area = area + aAreaLS(i, k - 1)
            End If
          Next k
          If area > 0 Then
            With agdLanduse
              .rows = .rows + 1
              .set_TextMatrix(.rows, 0, i)
              .set_TextMatrix(.rows, 2, area)
            End With
          End If
        Else
          'no subbasin layer, add all categories
          With agdLanduse
            .rows = .rows + 1
            .set_TextMatrix(.rows, 0, i)
          End With
        End If
      Next i

      If pSubbasinsLayer <> "<none>" Then
        'go thru area column and convert to percents
        tarea = 0
        For i = 1 To agdLanduse.rows
          tarea = tarea + agdLanduse.get_TextMatrix(i, 2)
        Next
        For i = 1 To agdLanduse.rows
          agdLanduse.set_TextMatrix(i, 2, (CInt(agdLanduse.get_TextMatrix(i, 2) / tarea * 10000) / 100))
        Next
      Else
        agdLanduse.set_ColTitle(2, "HIDE")
      End If

      If lutype = "NLCD" Then
        'add mrlc classifications
        ReclassifyFile = "\BASINS\etc\mrlc.dbf"
        Dim tmpDbf As IATCTable
        tmpDbf = atcUtility.TableOpener.OpenAnyTable(ReclassifyFile)
        For k = 1 To agdLanduse.rows
          For i = 1 To tmpDbf.NumRecords
            tmpDbf.CurrentRecord = i
            If agdLanduse.get_TextMatrix(k, 0) = tmpDbf.Value(1) Then
              agdLanduse.set_TextMatrix(k, 1, tmpDbf.Value(2))
              Exit For
            End If
          Next i
        Next k
      Else
        'dont know descriptions for other types
      End If

      'add grouped classifications
      ReclassifyFile = "\BASINS\etc\nlcd.dbf"
      AddGroupedClassifications(ReclassifyFile, lutype)
    End If

    agdLanduse.ColsSizeByContents()
    lblProgress.Visible = False
  End Sub

  Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
    If RadioButton1.Checked Then
      agdLanduse.set_ColTitle(5, "HIDE")
      agdLanduse.set_ColTitle(6, "HIDE")
      agdLanduse.ColsSizeByContents()
      cmdAdd.Visible = False
      cmdDelete.Visible = False
    Else
      agdLanduse.set_ColTitle(5, "Multiplier")
      agdLanduse.set_ColTitle(6, "Subbasin")
      agdLanduse.ColsSizeByContents()
      cmdAdd.Visible = True
      cmdDelete.Visible = True
    End If
  End Sub

  Private Sub agdLanduse_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles agdLanduse.RowColChange
    Dim i As Long, j As Long
    Dim cVals As Collection
    Dim inlist As Boolean

    With agdLanduse
      If .col = 3 Then
        .ClearValues()
        cVals = New Collection
        For i = 1 To .rows
          inlist = False
          For j = 1 To cVals.Count
            If cVals(j) = .get_TextMatrix(i, 3) Then
              inlist = True
              Exit For
            End If
          Next j
          If Not inlist Then
            cVals.Add(.get_TextMatrix(i, 3))
          End If
        Next i
        For i = 1 To cVals.Count
          .addValue(cVals(i))
        Next i
      Else
        .ClearValues()
      End If
    End With
  End Sub

  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub

  Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
    Dim i As Long, j As Long

    agdLanduse.rows = agdLanduse.rows + 1
    For i = agdLanduse.rows To agdLanduse.SelStartRow() + 1 Step -1
      For j = 0 To agdLanduse.cols - 1
        agdLanduse.set_TextMatrix(i, j, agdLanduse.get_TextMatrix(i - 1, j))
      Next j
    Next i
  End Sub

  Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
    agdLanduse.DeleteRow(agdLanduse.SelStartRow())
  End Sub

  'Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
  '  Dim dbfname As String
  '  Dim tmpDbf As MapWinGIS.Table
  '  Dim tmpField As MapWinGIS.Field
  '  Dim tmpField2 As MapWinGIS.Field
  '  Dim tmpField3 As MapWinGIS.Field
  '  Dim tmpField4 As MapWinGIS.Field
  '  Dim tmpField5 As MapWinGIS.Field
  '  Dim i As Long, j As Long
  '  Dim baserow As Long

  '  If sfdSave.ShowDialog() = DialogResult.OK Then
  '    dbfname = sfdSave.FileName
  '    tmpDbf = New MapWinGIS.Table
  '    'does this dbf already exist?
  '    If FileExists(dbfname) Then
  '      'delete this file first
  '      System.IO.File.Delete(dbfname)
  '    End If
  '    tmpDbf.CreateNew(dbfname)
  '    tmpDbf.StartEditingTable()
  '    tmpField = New MapWinGIS.Field
  '    tmpField.Name = "Value"
  '    tmpField.Type = MapWinGIS.FieldType.INTEGER_FIELD
  '    tmpField.Width = 10
  '    tmpDbf.EditInsertField(tmpField, 1)
  '    tmpField2 = New MapWinGIS.Field
  '    tmpField2.Name = "Landuse"
  '    tmpField2.Type = MapWinGIS.FieldType.STRING_FIELD
  '    tmpField2.Width = 30
  '    tmpDbf.EditInsertField(tmpField2, 2)
  '    tmpField3 = New MapWinGIS.Field
  '    tmpField3.Name = "Pervious"
  '    tmpField3.Type = MapWinGIS.FieldType.STRING_FIELD
  '    tmpField3.Width = 10
  '    tmpDbf.EditInsertField(tmpField3, 3)
  '    tmpField4 = New MapWinGIS.Field
  '    tmpField4.Name = "Multiplier"
  '    tmpField4.Type = MapWinGIS.FieldType.STRING_FIELD
  '    tmpField4.Width = 10
  '    tmpDbf.EditInsertField(tmpField4, 4)
  '    tmpField5 = New MapWinGIS.Field
  '    tmpField5.Name = "Subbasin"
  '    tmpField5.Type = MapWinGIS.FieldType.STRING_FIELD
  '    tmpField5.Width = 10
  '    tmpDbf.EditInsertField(tmpField5, 5)
  '    tmpDbf.EditInsertRow(0)
  '    baserow = -1

  '    'write out hidden portion for unused classifications
  '    For i = 1 To agdHidden.rows
  '      tmpDbf.EditCellValue(0, i - 1, agdHidden.get_TextMatrix(i, 0))
  '      tmpDbf.EditCellValue(1, i - 1, agdHidden.get_TextMatrix(i, 1))
  '      tmpDbf.EditCellValue(2, i - 1, agdHidden.get_TextMatrix(i, 2))
  '      tmpDbf.EditCellValue(3, i - 1, agdHidden.get_TextMatrix(i, 3))
  '      tmpDbf.EditCellValue(4, i - 1, agdHidden.get_TextMatrix(i, 4))
  '      tmpDbf.EditInsertRow(i)
  '      baserow = baserow + 1
  '    Next i

  '    'now write out main grid
  '    For i = 1 To agdLanduse.rows
  '      tmpDbf.EditCellValue(0, i + baserow, agdLanduse.get_TextMatrix(i, 0))
  '      tmpDbf.EditCellValue(1, i + baserow, agdLanduse.get_TextMatrix(i, 3))
  '      tmpDbf.EditCellValue(2, i + baserow, agdLanduse.get_TextMatrix(i, 4))
  '      If agdLanduse.get_TextMatrix(i, 5) = 1 Then
  '        'write blank instead
  '        tmpDbf.EditCellValue(3, i + baserow, "")
  '      Else
  '        tmpDbf.EditCellValue(3, i + baserow, agdLanduse.get_TextMatrix(i, 5))
  '      End If
  '      If agdLanduse.get_TextMatrix(i, 6) = "<all>" Then
  '        tmpDbf.EditCellValue(4, i + baserow, "")
  '      Else
  '        tmpDbf.EditCellValue(4, i + baserow, agdLanduse.get_TextMatrix(i, 6))
  '      End If
  '      If i < agdLanduse.rows Then
  '        tmpDbf.EditInsertRow(i + baserow + 1)
  '      End If
  '    Next i
  '    tmpDbf.StopEditingTable(True)
  '    tmpDbf.Close()
  '  End If
  'End Sub

  Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
    Dim dbfname As String
    Dim tmpDbf As IATCTable
    Dim i As Long, j As Long
    Dim baserow As Long

    If sfdSave.ShowDialog() = DialogResult.OK Then
      dbfname = sfdSave.FileName
      'does this dbf already exist?
      If FileExists(dbfname) Then
        'delete this file first
        System.IO.File.Delete(dbfname)
      End If
      tmpDbf = atcUtility.TableOpener.OpenAnyTable(dbfname)
      tmpDbf.NumFields = 5
      tmpDbf.FieldName(1) = "Value"
      tmpDbf.FieldType(1) = "N"
      tmpDbf.FieldLength(1) = 10
      tmpDbf.FieldName(2) = "Landuse"
      tmpDbf.FieldType(2) = "C"
      tmpDbf.FieldLength(2) = 30
      tmpDbf.FieldName(3) = "Pervious"
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
      For i = 1 To agdHidden.rows
        tmpDbf.CurrentRecord = i
        tmpDbf.Value(1) = agdHidden.get_TextMatrix(i, 0)
        tmpDbf.Value(2) = agdHidden.get_TextMatrix(i, 1)
        tmpDbf.Value(3) = agdHidden.get_TextMatrix(i, 2)
        tmpDbf.Value(4) = agdHidden.get_TextMatrix(i, 3)
        tmpDbf.Value(5) = agdHidden.get_TextMatrix(i, 4)
        baserow = baserow + 1
      Next i

      'now write out main grid
      For i = 1 To agdLanduse.rows
        tmpDbf.CurrentRecord = i + baserow
        tmpDbf.Value(1) = agdLanduse.get_TextMatrix(i, 0)
        tmpDbf.Value(2) = agdLanduse.get_TextMatrix(i, 3)
        tmpDbf.Value(3) = agdLanduse.get_TextMatrix(i, 4)
        If agdLanduse.get_TextMatrix(i, 5) = 1 Then
          'write blank instead
          tmpDbf.Value(4) = ""
        Else
          tmpDbf.Value(4) = agdLanduse.get_TextMatrix(i, 5)
        End If
        If agdLanduse.get_TextMatrix(i, 6) = "<all>" Then
          tmpDbf.Value(5) = ""
        Else
          tmpDbf.Value(5) = agdLanduse.get_TextMatrix(i, 6)
        End If
      Next i
      tmpDbf.WriteFile(dbfname)

    End If
  End Sub

  Private Sub StoreUnusedClassifications(ByVal tmpDbf As IATCTable)
    Dim i As Long, j As Long
    Dim inlist As Boolean

    agdHidden.ClearValues()
    agdHidden.rows = 0
    For i = 1 To tmpDbf.NumRecords
      tmpDbf.CurrentRecord = i
      'look for codes that did not get used
      inlist = False
      For j = 1 To agdLanduse.rows
        If tmpDbf.Value(1) = agdLanduse.get_TextMatrix(j, 0) Then
          'this one was used
          inlist = True
          Exit For
        End If
      Next j
      If Not inlist Then
        With agdHidden
          .rows = .rows + 1
          .set_TextMatrix(.rows, 0, tmpDbf.Value(1))
          .set_TextMatrix(.rows, 1, tmpDbf.Value(2))
          .set_TextMatrix(.rows, 2, tmpDbf.Value(3))
          If tmpDbf.NumFields > 3 Then
            .set_TextMatrix(.rows, 3, tmpDbf.Value(4))
          Else
            .set_TextMatrix(.rows, 3, "")
          End If
          If tmpDbf.NumFields > 4 Then
            .set_TextMatrix(.rows, 4, tmpDbf.Value(5))
          Else
            .set_TextMatrix(.rows, 4, "")
          End If
        End With
      End If
    Next i
  End Sub

  Private Sub AddGroupedClassifications(ByVal ReclassifyFile As String, ByVal lutype As String)
    Dim i As Long, k As Long
    Dim tmpDbf As IATCTable
    Dim ctmp As String
    Dim tmult As Single
    Dim tint As Long
    Dim foundvalue As Boolean
    Dim irow As Long
    Dim j As Long

    tmpDbf = atcUtility.TableOpener.OpenAnyTable(ReclassifyFile)
    k = 1
    Do While k <= agdLanduse.rows
      'For k = 1 To agdLanduse.rows
      foundvalue = False
      For i = 1 To tmpDbf.NumRecords
        tmpDbf.CurrentRecord = i
        If agdLanduse.get_TextMatrix(k, 0) = tmpDbf.Value(1) Then

          If foundvalue Then
            'already found one of these, add a row to table
            agdLanduse.rows = agdLanduse.rows + 1
            For irow = agdLanduse.rows To k + 1 Step -1
              For j = 0 To agdLanduse.cols - 1
                agdLanduse.set_TextMatrix(irow, j, agdLanduse.get_TextMatrix(irow - 1, j))
              Next j
            Next irow
            k = k + 1
            agdLanduse.set_TextMatrix(k, 4, "")
            agdLanduse.set_TextMatrix(k, 5, "")
            agdLanduse.set_TextMatrix(k, 6, "")
          End If

          agdLanduse.set_TextMatrix(k, 3, tmpDbf.Value(2))

          If tmpDbf.NumFields > 2 Then
            'populate pervious field
            agdLanduse.set_TextMatrix(k, 4, tmpDbf.Value(3))
          End If

          If tmpDbf.NumFields > 3 Then
            If IsNumeric(tmpDbf.Value(4)) Then
              tmult = tmpDbf.Value(4)
              'use this as multiplier
              agdLanduse.set_TextMatrix(k, 5, tmult)
            End If
          End If

          If tmpDbf.NumFields > 4 Then
            If IsNumeric(tmpDbf.Value(5)) Then
              tint = tmpDbf.Value(5)
              'use this as subbasin field
              agdLanduse.set_TextMatrix(k, 6, tint)
            End If
          End If

          foundvalue = True

        End If

      Next i

      If Len(agdLanduse.get_TextMatrix(k, 5)) = 0 Then
        'populate multiplier
        agdLanduse.set_TextMatrix(k, 5, 1)
      End If

      If Len(agdLanduse.get_TextMatrix(k, 6)) = 0 Then
        'populate subbasin field
        agdLanduse.set_TextMatrix(k, 6, "<all>")
      End If

      k = k + 1
    Loop
    'Next k

    If lutype = "NLCD" And pSubbasinsLayer = "<none>" Then
      'display only those known to be valid NLCD types
      k = 1
      Do While k <= agdLanduse.rows
        If Len(agdLanduse.get_TextMatrix(k, 1)) = 0 Then
          agdLanduse.DeleteRow(k)
        Else
          k = k + 1
        End If
      Loop
    End If

    'set appropriate header 
    If pSubbasinsLayer <> "<none>" Then
      agdLanduse.set_header(lutype & " classes within layer " & pSubbasinsLayer & " (grouped by " & FilenameNoPath(ReclassifyFile) & ")")
    Else
      If pLanduseType = 0 Or pLanduseType = 2 Then
        'giras or other shape
        agdLanduse.set_header(lutype & " classes in current project (grouped by " & FilenameNoPath(ReclassifyFile) & ")")
      ElseIf pLanduseType = 1 Or pLanduseType = 3 Then
        'nlcd grid or other grid
        agdLanduse.set_header("All " & lutype & " classes (grouped by " & FilenameNoPath(ReclassifyFile) & ")")
      End If
    End If
    'agdLanduse.set_header(lutype & " Classifications (" & FilenameNoPath(ReclassifyFile) & ")")
    StoreUnusedClassifications(tmpDbf)

    'tmpDbf.Close()
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
    If ofdLoad.ShowDialog() = DialogResult.OK Then
      dbfname = ofdLoad.FileName
      AddGroupedClassifications(dbfname, lutype)
    End If
  End Sub
End Class
