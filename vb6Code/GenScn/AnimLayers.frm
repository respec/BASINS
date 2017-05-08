VERSION 5.00
Begin VB.Form frmAnimLayers 
   Caption         =   "Edit Map Display Attributes"
   ClientHeight    =   5928
   ClientLeft      =   3924
   ClientTop       =   3720
   ClientWidth     =   11760
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   49
   Icon            =   "AnimLayers.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5928
   ScaleWidth      =   11760
   StartUpPosition =   3  'Windows Default
   Visible         =   0   'False
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   240
      TabIndex        =   24
      Top             =   1200
      Width           =   8172
      Begin VB.CommandButton cmdClose 
         Caption         =   "&OK"
         Default         =   -1  'True
         Height          =   372
         Index           =   0
         Left            =   0
         TabIndex        =   32
         Top             =   0
         Width           =   855
      End
      Begin VB.CommandButton cmdClose 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   372
         Index           =   1
         Left            =   960
         TabIndex        =   31
         TabStop         =   0   'False
         Top             =   0
         Width           =   855
      End
      Begin VB.CommandButton cmdOption 
         Caption         =   "&Add"
         Height          =   372
         Index           =   0
         Left            =   2040
         TabIndex        =   30
         ToolTipText     =   "Place new shape file on map"
         Top             =   0
         Width           =   855
      End
      Begin VB.CommandButton cmdOption 
         Caption         =   "&Delete"
         Height          =   372
         Index           =   1
         Left            =   3000
         TabIndex        =   29
         ToolTipText     =   "Remove layer from map"
         Top             =   0
         Width           =   855
      End
      Begin VB.CommandButton cmdOption 
         Caption         =   "&Up"
         Height          =   372
         Index           =   2
         Left            =   4080
         TabIndex        =   28
         ToolTipText     =   "Move layer up (top layer is drawn last)"
         Top             =   0
         Width           =   855
      End
      Begin VB.CommandButton cmdOption 
         Caption         =   "Dow&n"
         Height          =   372
         Index           =   3
         Left            =   5040
         TabIndex        =   27
         ToolTipText     =   "Move layer down (bottom layer is drawn first)"
         Top             =   0
         Width           =   855
      End
      Begin VB.CommandButton cmdOption 
         Caption         =   "&Renderer"
         Height          =   372
         Index           =   5
         Left            =   7200
         TabIndex        =   26
         ToolTipText     =   "Edit renderer for layer"
         Top             =   0
         Width           =   972
      End
      Begin VB.CommandButton cmdOption 
         Caption         =   "&Table"
         Height          =   372
         Index           =   4
         Left            =   6120
         TabIndex        =   25
         ToolTipText     =   "Edit columns of table for layer"
         Top             =   0
         Width           =   972
      End
   End
   Begin VB.VScrollBar VScroll 
      Height          =   900
      Left            =   11520
      TabIndex        =   23
      Top             =   0
      Visible         =   0   'False
      Width           =   255
   End
   Begin VB.Frame fraLayer 
      BorderStyle     =   0  'None
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   475
      Index           =   0
      Left            =   240
      TabIndex        =   0
      Top             =   360
      Width           =   11055
      Begin VB.CheckBox chkAnimate 
         Height          =   255
         Index           =   0
         Left            =   5280
         TabIndex        =   7
         ToolTipText     =   "Check to allow animation of this layer. A layer must have IDLOCN to be animated."
         Top             =   150
         Width           =   195
      End
      Begin VB.CheckBox chkLabels 
         Height          =   255
         Index           =   0
         Left            =   4740
         TabIndex        =   6
         ToolTipText     =   "Check to label items in this layer"
         Top             =   150
         Width           =   192
      End
      Begin VB.CheckBox chkAnimLayers 
         Height          =   255
         Index           =   0
         Left            =   3660
         TabIndex        =   4
         ToolTipText     =   "Check to put layer on map"
         Top             =   150
         Width           =   192
      End
      Begin VB.ComboBox comboStyle 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   0
         Left            =   7260
         Style           =   2  'Dropdown List
         TabIndex        =   10
         Top             =   120
         Width           =   1815
      End
      Begin VB.CheckBox chkTable 
         Height          =   255
         Index           =   0
         Left            =   4200
         TabIndex        =   5
         ToolTipText     =   "Check to have a table below the map for this layer"
         Top             =   150
         Width           =   192
      End
      Begin VB.TextBox txtAlias 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   312
         Index           =   0
         Left            =   2220
         TabIndex        =   3
         Text            =   "?"
         ToolTipText     =   "Displayed on tab for layer table"
         Top             =   120
         Width           =   1272
      End
      Begin VB.CommandButton cmdColor 
         Appearance      =   0  'Flat
         BackColor       =   &H00FFFFFF&
         Height          =   255
         Index           =   0
         Left            =   6045
         MaskColor       =   &H00000002&
         Style           =   1  'Graphical
         TabIndex        =   8
         Top             =   150
         Width           =   375
      End
      Begin VB.CommandButton cmdOutColor 
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Height          =   255
         Index           =   0
         Left            =   6660
         MaskColor       =   &H00000002&
         Style           =   1  'Graphical
         TabIndex        =   9
         Top             =   150
         Width           =   375
      End
      Begin VB.Label lblRend 
         Caption         =   "?"
         Height          =   240
         Index           =   0
         Left            =   9180
         TabIndex        =   11
         Top             =   180
         Width           =   1395
      End
      Begin VB.Label lblType 
         Caption         =   "?"
         Height          =   255
         Index           =   0
         Left            =   1800
         TabIndex        =   2
         Top             =   160
         Width           =   195
      End
      Begin VB.Label lblLayer 
         Caption         =   "?"
         Height          =   255
         Index           =   0
         Left            =   120
         TabIndex        =   1
         Top             =   165
         Width           =   1575
      End
   End
   Begin MSComDlg.CommonDialog cdlAnimLayers 
      Left            =   120
      Top             =   840
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   2.54052e-29
   End
   Begin VB.Label lblCol 
      Caption         =   "Labels"
      Height          =   255
      Index           =   5
      Left            =   4845
      TabIndex        =   21
      Top             =   120
      Width           =   615
   End
   Begin VB.Label lblCol 
      Caption         =   "Renderer"
      Height          =   255
      Index           =   9
      Left            =   9420
      TabIndex        =   20
      Top             =   120
      Width           =   915
   End
   Begin VB.Label lblCol 
      Caption         =   "Table"
      Height          =   255
      Index           =   4
      Left            =   4250
      TabIndex        =   19
      Top             =   120
      Width           =   495
   End
   Begin VB.Label lblCol 
      Caption         =   "Map"
      Height          =   255
      Index           =   3
      Left            =   3780
      TabIndex        =   18
      Top             =   120
      Width           =   435
   End
   Begin VB.Label lblCol 
      Caption         =   "Caption"
      Height          =   252
      Index           =   2
      Left            =   2460
      TabIndex        =   17
      Top             =   120
      Width           =   1152
   End
   Begin VB.Label lblCol 
      Caption         =   "Type"
      Height          =   255
      Index           =   1
      Left            =   1920
      TabIndex        =   16
      Top             =   120
      Width           =   495
   End
   Begin VB.Label lblCol 
      Caption         =   "Style"
      Height          =   255
      Index           =   8
      Left            =   7500
      TabIndex        =   15
      Top             =   120
      Width           =   735
   End
   Begin VB.Label lblCol 
      Caption         =   "Outline"
      Height          =   255
      Index           =   7
      Left            =   6780
      TabIndex        =   14
      Top             =   120
      Width           =   735
   End
   Begin VB.Label lblCol 
      Alignment       =   2  'Center
      Caption         =   "Fill"
      Height          =   255
      Index           =   6
      Left            =   6280
      TabIndex        =   13
      Top             =   120
      Width           =   375
   End
   Begin VB.Label lblCol 
      Caption         =   "Layer"
      Height          =   255
      Index           =   0
      Left            =   360
      TabIndex        =   12
      Top             =   120
      Width           =   1155
   End
   Begin VB.Label lblCol 
      Caption         =   "Animate"
      Height          =   255
      Index           =   10
      Left            =   5520
      TabIndex        =   22
      Top             =   120
      Width           =   915
   End
End
Attribute VB_Name = "frmAnimLayers"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Public M As Map
Public am As ATCoMap
Dim MLay() As MapLayer
Dim MExtent As Rectangle
Dim CurLayer&
Dim TopFrameTop&, TopFrameLeft&
Dim FormActivated As Boolean
Dim LayerInfo() As MapLayerInfo, nLayers&
Private ProgramSettingCheckbox As Boolean

Public Sub UpdateRenderer(i&, l As MapLayer, rt&)
  Set MLay(i) = l
  LayerInfo(i).RendType = rt
  SetFrameValues i
End Sub

Private Sub chkAnimate_Click(Index As Integer)
  If Not ProgramSettingCheckbox Then
    SelectLayer Index
    If chkAnimate(Index).Value = 0 Then
      LayerInfo(Index).Animate = False
    Else
      LayerInfo(Index).Animate = True
    End If
  End If
End Sub

Private Sub chkLabels_Click(Index As Integer)
  If Not ProgramSettingCheckbox Then
    SelectLayer Index
    If chkLabels(Index).Value = 1 Then
      EditLabelProperties Index
      M(Index).LabelField = LayerInfo(Index).LabelField
    Else
      M(Index).LabelField = ""
    End If
    'If chkLabels(Index).Value = 1 Then m(Index).LabelField = LayerInfo(Index).LabelField Else m(Index).LabelField = ""
  End If
End Sub

Private Sub chkAnimLayers_Click(Index As Integer)
  If Not ProgramSettingCheckbox Then SelectLayer Index
  If chkAnimLayers(Index).Value = 0 Then
    MLay(Index).Visible = False
  Else
    MLay(Index).Visible = True
  End If
End Sub

Private Sub chkTable_Click(Index As Integer)
  If Not ProgramSettingCheckbox Then
    SelectLayer Index
    If chkTable(Index).Value = 1 Then EditTableCols Index Else LayerInfo(Index).TableIndex = 0
  End If
End Sub

Private Sub EditLabelProperties(Index As Integer)
  frmMapLabel.Hide
  frmMapLabel.Caption = "Labels for '" & LayerInfo(Index).Name & "' layer"
  frmMapLabel.Icon = Me.Icon
  SetLabelDialogValuesFromLayer M.Layers(Index), LayerInfo(Index)
  frmMapLabel.Show 1
  If frmMapLabel.MapLabelOk Then SetLayerFromLabelDialogValues M.Layers(Index), LayerInfo(Index)
  Unload frmMapLabel
End Sub

Private Sub SetLabelDialogValuesFromLayer(lyr As MapLayer, inf As MapLayerInfo)
  Dim MapRS As Recordset, r&, fieldname$
  
  On Error GoTo AbortSub
  
  Set MapRS = OpenDBF(inf.Path & inf.baseFilename & ".shp", True)
  
  With frmMapLabel
    .comboLabelField.Clear
    .comboKeyField.Clear
    .comboLabelField.AddItem "None": .comboLabelField.ListIndex = .comboLabelField.NewIndex
    .comboKeyField.AddItem "None":   .comboKeyField.ListIndex = .comboKeyField.NewIndex
    
    For r = 0 To MapRS.Fields.Count - 1
      fieldname = MapRS.Fields(r).Name
      .comboLabelField.AddItem fieldname
      .comboKeyField.AddItem fieldname
      If fieldname = inf.LabelField Then .comboLabelField.ListIndex = .comboLabelField.NewIndex
      If fieldname = inf.KeyField Then .comboKeyField.ListIndex = .comboKeyField.NewIndex
    Next r
  End With
  MapRS.Close
  
  With frmMapLabel.comboVertAlign
    .Clear
    .AddItem "Top"
    .ItemData(.NewIndex) = 1
    If lyr.LabelVertAlignment = 1 Then .ListIndex = .NewIndex
    .AddItem "Bottom"
    .ItemData(.NewIndex) = 2
    If lyr.LabelVertAlignment = 2 Then .ListIndex = .NewIndex
    .AddItem "Center"
    .ItemData(.NewIndex) = 5
    If lyr.LabelVertAlignment = 5 Then .ListIndex = .NewIndex
    .AddItem "Baseline"
    .ItemData(.NewIndex) = 6
    If lyr.LabelVertAlignment = 6 Then .ListIndex = .NewIndex
  End With
  With frmMapLabel.comboHorizAlign
    .Clear
    .AddItem "Left"
    .ItemData(.NewIndex) = 3
    If lyr.LabelHorzAlignment = 3 Then .ListIndex = .NewIndex
    .AddItem "Right"
    .ItemData(.NewIndex) = 4
    If lyr.LabelHorzAlignment = 4 Then .ListIndex = .NewIndex
    .AddItem "Center"
    .ItemData(.NewIndex) = 5
    If lyr.LabelHorzAlignment = 5 Then .ListIndex = .NewIndex
  End With
  
  With frmMapLabel.cdlFont
    .flags = cdlCFScreenFonts + cdlCFEffects
    .FontName = lyr.LabelFont.Name
    .FontBold = lyr.LabelFont.Bold
    .FontItalic = lyr.LabelFont.Italic
    .FontSize = lyr.LabelFont.size
    .FontStrikethru = lyr.LabelFont.Strikethrough
    .FontUnderline = lyr.LabelFont.Underline
    .Color = lyr.LabelColor
  End With
  Exit Sub
AbortSub:
  If Err.Number <> 91 Then '91 is Object variable not set
    MsgBox "Error setting label values from layer" & vbCr & Err.Description
  Else
    MsgBox "Error setting label values from layer"
  End If
End Sub

Private Sub SetLayerFromLabelDialogValues(lyr As MapLayer, inf As MapLayerInfo)
  With frmMapLabel
    If .comboKeyField.Text = "None" Then inf.KeyField = "" Else inf.KeyField = .comboKeyField.Text
    If .comboLabelField.Text = "None" Then inf.LabelField = "" Else inf.LabelField = .comboLabelField.Text
  
    lyr.LabelVertAlignment = .comboVertAlign.ItemData(.comboVertAlign.ListIndex)
    lyr.LabelHorzAlignment = .comboHorizAlign.ItemData(.comboHorizAlign.ListIndex)
  End With
  With frmMapLabel.cdlFont
    If Len(.FontName) > 0 Then
      lyr.LabelFont.Name = .FontName
      lyr.LabelFont.Bold = .FontBold
      lyr.LabelFont.Italic = .FontItalic
      lyr.LabelFont.size = .FontSize
      lyr.LabelFont.Strikethrough = .FontStrikethru
      lyr.LabelFont.Underline = .FontUnderline
      lyr.LabelColor = .Color
    End If
  End With
End Sub

Private Sub EditTableCols(ByVal Index&)
  Dim MapRS As Recordset, KeyAdded As Boolean, LabelAdded As Boolean
  If Len(LayerInfo(Index).KeyField) > 0 Then KeyAdded = False Else KeyAdded = True
  If Len(LayerInfo(Index).LabelField) > 0 Then LabelAdded = False Else LabelAdded = True
  Set MapRS = OpenDBF(LayerInfo(Index).Path & LayerInfo(Index).baseFilename & ".shp", True)
Debug.Print "frmAnimLayers:EditTableCols"
  frmMapCols.Hide
  frmMapCols.Caption = "Table Columns for '" & LayerInfo(Index).Name & "' layer"
  frmMapCols.Icon = Me.Icon
  Dim r&
  For r = 0 To LayerInfo(Index).nFields - 1
    With LayerInfo(Index).Fields(r)
      frmMapCols.AddField .Name, .Caption, .Column
      If .Name = LayerInfo(Index).KeyField Then KeyAdded = True
      If .Name = LayerInfo(Index).LabelField Then LabelAdded = True
    End With
  Next r
  
  With LayerInfo(Index)
    If Not KeyAdded Then frmMapCols.AddField .KeyField, .KeyField, 99
    If Not LabelAdded Then frmMapCols.AddField .LabelField, .LabelField, 99
  End With
  
  frmMapCols.SetRecordset MapRS
  On Error Resume Next
  MapRS.Close
  frmMapCols.Show 1
  If MapColsOk Then SetFieldsFromMapCols Else chkTable(Index).Value = 0
End Sub

Private Sub SetFieldsFromMapCols()
  Dim availcol&, col&, nColsSelected&
  nColsSelected = 0
  For availcol = 0 To nAvailableFields - 1
    If AvailableFields(availcol).Column >= 0 Then nColsSelected = nColsSelected + 1
  Next availcol
  With LayerInfo(CurLayer)
    If nColsSelected <> .nFields Then
      ReDim .Fields(0 To nColsSelected - 1)
      .nFields = nColsSelected
    End If
    col = 0
    For availcol = 0 To nAvailableFields - 1
      If AvailableFields(availcol).Column >= 0 Then
        .Fields(col).Caption = AvailableFields(availcol).Caption
        .Fields(col).Name = AvailableFields(availcol).Name
        .Fields(col).Column = col
        col = col + 1
      End If
    Next availcol
    If col > 0 Then .TableIndex = 99 Else .TableIndex = 0
  End With
  
  ClearFrmMapCols
  Unload frmMapCols
End Sub

Private Sub cmdClose_Click(Index As Integer)
  Dim layr As MapLayer
  If Index = 0 Then 'ok update map
    Dim l& 'layer number
    Hide
    
    M.Layers.Clear
    am.Clear
    am.LayerCount = nLayers
    For l = nLayers - 1 To 0 Step -1
      M.Layers.Add MLay(l)
      SetAmLayerInfo l
    Next l
    Set M.Extent = MExtent
    CurLayer = -1
    am.SetLegend
    am.RepopulateGrids
  End If
  For l = nLayers - 1 To 1 Step -1
    Set MLay(l) = Nothing
    Unload chkAnimLayers(l)
    Unload chkTable(l)
    Unload chkLabels(l)
    Unload chkAnimate(l)
    Unload cmdColor(l)
    Unload cmdOutColor(l)
    'Unload optCov(l)
    Unload lblType(l)
    Unload lblLayer(l)
    Unload txtAlias(l)
    Unload comboStyle(l)
    Unload lblRend(l)
    Unload fraLayer(l)
  Next l
  Set MLay(0) = Nothing
  Set M = Nothing
  Set am = Nothing
  Unload Me
End Sub

Private Sub cmdOption_Click(Index As Integer)
  Dim tmpLay As MapLayer, tmpLayerInfo As MapLayerInfo, i&
  
  If CurLayer < 0 Then SelectLayer 0
  
  If Index = 0 Then ' add
    Dim f$, directory$
    directory = CurDir
    cdlAnimLayers.DialogTitle = "Add Map Layer"
    cdlAnimLayers.Filter = "Shape files (*.shp)|*.shp"
    cdlAnimLayers.flags = &H1804&
    cdlAnimLayers.CancelError = False
    cdlAnimLayers.Action = 1
    ChDir directory
    f = cdlAnimLayers.Filename
    If Len(f) > 0 Then
      Dim newLay&
      newLay = nLayers
      ReDim Preserve MLay(0 To nLayers)
      ReDim Preserve LayerInfo(0 To nLayers)
      nLayers = nLayers + 1
      Set MLay(newLay) = New MapLayer
      With LayerInfo(newLay)
        .Animate = False
        .baseFilename = FilenameOnly(f)
        If Len(.baseFilename) > 3 Then
          If Mid(.baseFilename, Len(.baseFilename) - 3, 1) = "." Then .baseFilename = Left(.baseFilename, Len(.baseFilename) - 4)
        End If
        .Path = PathNameOnly(f) & "\"
        .KeyField = ""
        .LabelField = ""
        .Name = .baseFilename
        .nFields = 0
        .nSelected = 0
        .RendType = 0
        .TableIndex = 0
      End With
      With MLay(newLay)
        .File = LayerInfo(newLay).Path & LayerInfo(newLay).baseFilename & ".shp"
        .Name = LayerInfo(newLay).Name
        .symbol.Color = Rnd * vbWhite
        If .symbol.SymbolType = moPointSymbol Then
          .symbol.Style = moSquareMarker
          .symbol.OutlineColor = moBlack
        ElseIf .symbol.SymbolType = moLineSymbol Then
          .symbol.Style = moSolidLine
          .symbol.Color = moBlack
        ElseIf .symbol.SymbolType = moLineSymbol Then
          .symbol.Style = moTransparentFill
          .symbol.OutlineColor = moBlack
        End If
      End With
      AddLayerFrame newLay
      SetFrameValues newLay
      Form_Resize
      If MLay(newLay).Valid Then
        'MsgBox "New Layer is valid."
      Else
        MsgBox "Warning: new layer is not valid"
      End If
    End If
  ElseIf Index = 1 Then ' delete
    If nLayers < 2 Then
      MsgBox "Can't delete only layer in map."
    Else
      fraLayer(CurLayer).BorderStyle = 0
      nLayers = nLayers - 1
      For i = CurLayer To nLayers - 1
        Set MLay(i) = MLay(i + 1)
        LayerInfo(i) = LayerInfo(i + 1)
        SetFrameValues i
      Next i
      ReDim Preserve MLay(0 To nLayers - 1)
      ReDim Preserve LayerInfo(0 To nLayers - 1)
      If CurLayer >= nLayers Then SelectLayer nLayers - 1
      fraLayer(CurLayer).BorderStyle = 1
      fraLayer(nLayers).Visible = False
      Form_Resize
    End If
  ElseIf Index = 2 Then 'Move Up
    If CurLayer > 0 Then
      Set tmpLay = MLay(CurLayer)
      Set MLay(CurLayer) = MLay(CurLayer - 1)
      Set MLay(CurLayer - 1) = tmpLay
      tmpLayerInfo = LayerInfo(CurLayer)
      LayerInfo(CurLayer) = LayerInfo(CurLayer - 1)
      LayerInfo(CurLayer - 1) = tmpLayerInfo
      SetFrameValues CurLayer
      SelectLayer CurLayer - 1
      SetFrameValues CurLayer
      Form_Resize
    End If
  ElseIf Index = 3 Then 'Move Down
    If CurLayer < nLayers - 1 Then
      Set tmpLay = MLay(CurLayer)
      Set MLay(CurLayer) = MLay(CurLayer + 1)
      Set MLay(CurLayer + 1) = tmpLay
      tmpLayerInfo = LayerInfo(CurLayer)
      LayerInfo(CurLayer) = LayerInfo(CurLayer + 1)
      LayerInfo(CurLayer + 1) = tmpLayerInfo
      SetFrameValues CurLayer
      SelectLayer CurLayer + 1
      SetFrameValues CurLayer
      Form_Resize
    End If
  ElseIf Index = 4 Then 'Adjust Table Header
    If chkTable(CurLayer).Value = 0 Then
      chkTable(CurLayer).Value = 1 'this will also trigger EditTableCols
    Else
      EditTableCols CurLayer
    End If
  ElseIf Index = 5 Then 'Renderer
    If frmMapRend.SetRendererInfo(CurLayer, MLay(CurLayer), lblLayer(CurLayer), lblRend(CurLayer)) Then
      frmMapRend.Icon = Icon
      frmMapRend.Show 1
    End If
  End If
End Sub

Private Sub comboStyle_Change(Index As Integer)
  SelectLayer Index
  comboStyle_Click Index
End Sub

Private Sub comboStyle_Click(Index As Integer)
  If MLay(Index).symbol.SymbolType = moFillSymbol And comboStyle(Index).ListIndex = moTransparentFill Then
    cmdColor(Index).Visible = False
  Else
    cmdColor(Index).Visible = True
  End If
  MLay(Index).symbol.Style = comboStyle(Index).ListIndex
  If LayerInfo(Index).RendType = 1 Then copySymbol MLay(Index).symbol, MLay(Index).Renderer.DefaultSymbol
  
End Sub

Private Sub SetAmLayerInfo(i&)
  Dim j&
  With LayerInfo(i)
    am.LayerFilename(i) = .baseFilename
    am.LayerKeyField(i) = .KeyField
    am.LayerLabelField(i) = .LabelField
    am.LayerName(i) = .Name
    am.LayerFieldCount(i) = .nFields
    am.LayerPath(i) = .Path
    am.LayerRendType(i) = .RendType
    am.LayerAnimate(i) = .Animate
    am.LayerTableIndex(i) = .TableIndex
    For j = 0 To .nFields - 1
      am.LayerFieldName(i, j) = .Fields(j).Name
      am.LayerFieldCaption(i, j) = .Fields(j).Caption
      am.LayerFieldColumn(i, j) = .Fields(j).Column
    Next j
  End With
End Sub

Private Sub Form_Activate()
  If FormActivated Then
    If WindowState = vbNormal Then Height = 6000
  Else
    FormActivated = True
    If TopFrameLeft = 0 Then
      TopFrameTop = fraLayer(0).Top
      TopFrameLeft = fraLayer(0).Left
    End If
    Visible = False
    
    Dim i&, j&, layr As MapLayer
    nLayers = am.LayerCount
    If nLayers < 1 Then
      ReDim LayerInfo(0 To 0)
    Else
      ReDim LayerInfo(0 To nLayers - 1)
      For i = 0 To nLayers - 1
        With LayerInfo(i)
          .baseFilename = am.LayerFilename(i)
          .KeyField = am.LayerKeyField(i)
          .LabelField = am.LayerLabelField(i)
          .Name = am.LayerName(i)
          .nFields = am.LayerFieldCount(i)
          .nSelected = 0
          .Path = am.LayerPath(i)
          .RendType = am.LayerRendType(i)
          .Animate = am.LayerAnimate(i)
          .TableIndex = am.LayerTableIndex(i)
          ReDim .Fields(.nFields)
          For j = 0 To .nFields - 1
            .Fields(j).Name = am.LayerFieldName(i, j)
            .Fields(j).Caption = am.LayerFieldCaption(i, j)
            .Fields(j).Column = am.LayerFieldColumn(i, j)
          Next j
        End With
      Next i
    End If
    If Not am.LegendVisible Then
      Dim shftLeft&
      lblCol(2).Visible = False
      txtAlias(0).Visible = False
      lblCol(4).Visible = False
      chkTable(0).Visible = False
      cmdOption(4).Visible = False
      shftLeft = lblCol(3).Left - lblCol(2).Left
      For i = 3 To 10
        If i = 4 Then shftLeft = shftLeft + lblCol(5).Left - lblCol(4).Left
        lblCol(i).Left = lblCol(i).Left - shftLeft
      Next i
      chkAnimLayers(0).Left = lblCol(3).Left - 120
      chkLabels(0).Left = lblCol(5).Left - 105
      chkAnimate(0).Left = lblCol(10).Left - 240
      cmdColor(0).Left = lblCol(6).Left - 235
      cmdOutColor(0).Left = lblCol(7).Left - 120
      comboStyle(0).Left = lblCol(8).Left - 240
      lblRend(0).Left = lblCol(9).Left - 240
      fraLayer(0).Width = lblRend(0).Left + 1515
      Width = fraLayer(0).Width + 540
    End If
    ReDim MLay(0 To M.Layers.Count - 1)
    Set MExtent = M.Extent
    i = -1
    For Each layr In M.Layers
      i = i + 1
      Set MLay(i) = layr
      If i > 0 Then AddLayerFrame i
      SetFrameValues i
    Next 'layr
    Set layr = Nothing
    ' move down option buttons
    'cmdClose(0).Top = fraLayer(i).Top + fraLayer(i).Height * 2.5
    'cmdClose(1).Top = cmdClose(0).Top
    'For i = cmdOption.LBound To cmdOption.UBound
    '  cmdOption(i).Top = cmdClose(0).Top
    'Next i
    
    'optCov(0).Value = True
    SelectLayer 0
    fraLayer(CurLayer).BorderStyle = 1
    Me.Visible = True
    Height = 6000
    Form_Resize
  End If
End Sub

Private Sub Form_Load()
  If WindowState = vbNormal Then Height = 6000
End Sub

Private Sub Form_Resize()
  AnimLayersResize Me
End Sub

Public Sub AnimLayersResize(frm As Form)
  Dim l&, TopRow&, BotRow&
  If nLayers = 0 Then Exit Sub
  With frm
    .fraButtons.Top = .Height - 2.5 * .fraButtons.Height

    Dim f&, pos&, dy&
    pos = TopFrameTop
    dy = .fraLayer(0).Height
    
    If .VScroll.Visible Then
      For f = 0 To .VScroll.Value
        .fraLayer(f).Left = .Width + 100
      Next f
      f = .VScroll.Value
    Else
      f = 0
    End If
    TopRow = f
    BotRow = fraLayer.Count - 1
    While f < fraLayer.Count
      If .fraLayer(f).Visible Then
        .fraLayer(f).Top = pos
        pos = pos + dy
        If pos < .fraButtons.Top Then
          .fraLayer(f).Left = TopFrameLeft
        Else
          .fraLayer(f).Left = .Width + 100
          If f <= BotRow Then BotRow = f - 1
        End If
      End If
      f = f + 1
    Wend
    If BotRow < fraLayer.Count - 1 Or TopRow > 0 Then 'need scrollbar
      If .VScroll.Visible Then
        .VScroll.Visible = False
      Else
        .VScroll.Value = 0
      End If
      .VScroll.Min = 0
      .VScroll.Max = fraLayer.Count - 1 - (BotRow - TopRow)
      If (BotRow - TopRow > 1) Then
        .VScroll.LargeChange = BotRow - TopRow
      Else
        .VScroll.LargeChange = 1
      End If
      .VScroll.Left = .Width - .VScroll.Width * 1.5
      If .fraButtons.Top - .VScroll.Top > 100 Then .VScroll.Height = .fraButtons.Top - .VScroll.Top
      .VScroll.Visible = True
    Else
      .VScroll.Visible = False
      If pos < .Height - 3 * .cmdClose(0).Height And .WindowState = vbNormal Then
        .Height = pos + dy + 2.5 * .cmdClose(0).Height
        .fraButtons.Top = .Height - 2.5 * .fraButtons.Height
      End If
    End If
  End With
End Sub

Private Sub Form_Unload(Cancel As Integer)
  FormActivated = False
End Sub

Private Sub fraLayer_Click(Index As Integer)
  SelectLayer Index
End Sub

Private Sub lblLayer_Click(Index As Integer)
  SelectLayer Index
End Sub

Private Sub lblRend_Click(Index As Integer)
  SelectLayer Index
  cmdOption_Click 5 'Edit Renderer
End Sub

Private Sub lblType_Click(Index As Integer)
  SelectLayer Index
End Sub

Private Sub SelectLayer(Index As Integer)
  If (CurLayer >= 0) Then
    fraLayer(CurLayer).BorderStyle = 0
  End If
  CurLayer = Index
  fraLayer(CurLayer).BorderStyle = 1
End Sub

Private Sub cmdColor_Click(Index As Integer)
  SelectLayer Index
  cdlAnimLayers.DialogTitle = "Fill Color for '" & LayerInfo(Index).Name & "' layer"
  cdlAnimLayers.Color = cmdColor(Index).BackColor
  cdlAnimLayers.flags = &H1&
  cdlAnimLayers.ShowColor
  cmdColor(Index).BackColor = cdlAnimLayers.Color
  With MLay(Index)
    If .symbol.SymbolType <> moFillSymbol Or .symbol.Style <> moTransparentFill Then
      .symbol.Color = cmdColor(Index).BackColor
      If LayerInfo(Index).RendType = 1 Then .Renderer.DefaultSymbol.Color = cmdColor(Index).BackColor
    End If
  End With
End Sub

Private Sub cmdOutColor_Click(Index As Integer)
  SelectLayer Index
  cdlAnimLayers.DialogTitle = "Outline Color for '" & LayerInfo(Index).Name & "' layer"
  cdlAnimLayers.Color = cmdOutColor(Index).BackColor
  cdlAnimLayers.flags = &H1&
  cdlAnimLayers.ShowColor
  cmdOutColor(Index).BackColor = cdlAnimLayers.Color
  With MLay(Index)
    If .symbol.SymbolType <> moLineSymbol Then
      .symbol.OutlineColor = cmdOutColor(Index).BackColor
      If LayerInfo(Index).RendType = 1 Then .Renderer.DefaultSymbol.OutlineColor = cmdOutColor(Index).BackColor
    End If
  End With
End Sub

Private Sub AddLayerFrame(ByVal i&)
  ProgramSettingCheckbox = True
  If i <= fraLayer.UBound Then 'Already have this frame
    fraLayer(i).Top = fraLayer(i - 1).Top + fraLayer(i - 1).Height
    fraLayer(i).BorderStyle = 0
    fraLayer(i).Visible = True
  Else
    'another frame
    Load fraLayer(i)
    fraLayer(i).Top = fraLayer(i - 1).Top + fraLayer(i - 1).Height
    fraLayer(i).BorderStyle = 0
    fraLayer(i).Visible = True
    
    'another layer name
    Load lblLayer(i)
    Set lblLayer(i).Container = fraLayer(i)
    lblLayer(i).Visible = lblCol(0).Visible
    
    'another type label
    Load lblType(i)
    Set lblType(i).Container = fraLayer(i)
    lblType(i).Visible = lblCol(1).Visible
    
    'another layer alias
    Load txtAlias(i)
    Set txtAlias(i).Container = fraLayer(i)
    txtAlias(i).Visible = lblCol(2).Visible
    
    'another map show check box
    Load chkAnimLayers(i)
    Set chkAnimLayers(i).Container = fraLayer(i)
    chkAnimLayers(i).Visible = lblCol(3).Visible
    
    'another table show
    Load chkTable(i)
    Set chkTable(i).Container = fraLayer(i)
    chkTable(i).Value = 0
    chkTable(i).Visible = lblCol(4).Visible
    
    'another labels show
    Load chkLabels(i)
    Set chkLabels(i).Container = fraLayer(i)
    chkLabels(i).Value = 0
    chkLabels(i).Visible = lblCol(5).Visible
    
    'another Animate check box
    Load chkAnimate(i)
    Set chkAnimate(i).Container = fraLayer(i)
    chkAnimate(i).Value = 0
    chkAnimate(i).Visible = lblCol(10).Visible
    
    'another fill color panel
    Load cmdColor(i)
    Set cmdColor(i).Container = fraLayer(i)
    cmdColor(i).Visible = lblCol(6).Visible
    
    'another outline color panel
    Load cmdOutColor(i)
    Set cmdOutColor(i).Container = fraLayer(i)
    cmdOutColor(i).Visible = lblCol(7).Visible
    
    'another combobox
    Load comboStyle(i)
    Set comboStyle(i).Container = fraLayer(i)
    comboStyle(i).Visible = lblCol(8).Visible
    
    'another render type
    Load lblRend(i)
    Set lblRend(i).Container = fraLayer(i)
    lblRend(i).Visible = lblCol(9).Visible
  End If
  ProgramSettingCheckbox = False
End Sub

Private Sub SetFrameValues(i&)
  Dim l As MapLayer
  Set l = MLay(i)
  lblLayer(i).Caption = FilenameOnly(l.File)
  txtAlias(i).Text = l.Name
  ProgramSettingCheckbox = True
    
  If LayerInfo(i).TableIndex > 0 Then
      chkTable(i).Value = 1
  Else
      chkTable(i).Value = 0
  End If
  
  If l.Visible Then
    chkAnimLayers(i).Value = 1
  Else
    chkAnimLayers(i).Value = 0
  End If
  
  If Len(l.LabelField) > 0 Then
    chkLabels(i).Value = 1
  Else
    chkLabels(i).Value = 0
  End If
  
  If LayerInfo(i).Animate Then
    chkAnimate(i).Value = 1
  Else
    chkAnimate(i).Value = 0
  End If
  
  
  cmdColor(i).BackColor = l.symbol.Color
  If l.symbol.SymbolType = moFillSymbol And l.symbol.Style = moTransparentFill Then
    cmdColor(i).Visible = False
  Else
    cmdColor(i).Visible = True
  End If
  
  If l.symbol.SymbolType = moLineSymbol Then
    cmdOutColor(i).Visible = False
  Else
    cmdOutColor(i).BackColor = l.symbol.OutlineColor
    cmdOutColor(i).Visible = True
  End If
  
  comboStyle(i).Clear
  If l.symbol.SymbolType = moPointSymbol Then 'point
    comboStyle(i).AddItem "Circle", 0
    comboStyle(i).AddItem "Square", 1
    comboStyle(i).AddItem "Triangle", 2
    comboStyle(i).AddItem "Cross", 3
    lblType(i).Caption = "P"
  ElseIf l.symbol.SymbolType = moLineSymbol Then 'line
    comboStyle(i).AddItem "Solid Line", 0
    comboStyle(i).AddItem "Dash Line", 1
    comboStyle(i).AddItem "Dot Line", 2
    comboStyle(i).AddItem "Dash-Dot Line", 3
    comboStyle(i).AddItem "Dash-Dot-Dot Line", 4
    lblType(i).Caption = "L"
  ElseIf l.symbol.SymbolType = moFillSymbol Then 'polygon
    comboStyle(i).AddItem "Solid Fill", 0
    comboStyle(i).AddItem "Transparent", 1
    comboStyle(i).AddItem "Horizontal", 2
    comboStyle(i).AddItem "Vertical", 3
    comboStyle(i).AddItem "Upward Diagonal", 4
    comboStyle(i).AddItem "Downward Diagonal", 5
    comboStyle(i).AddItem "Cross Fill", 6
    comboStyle(i).AddItem "Diagonal Cross", 7
    comboStyle(i).AddItem "Light Gray", 8
    comboStyle(i).AddItem "Gray", 9
    comboStyle(i).AddItem "Dark Gray", 10
    lblType(i).Caption = "A"
  End If
  If l.symbol.Style < comboStyle(i).ListCount Then comboStyle(i).ListIndex = l.symbol.Style
  comboStyle(i).Visible = True
  
  Select Case LayerInfo(i).RendType
    Case 0: lblRend(i) = "None"
    Case 1: lblRend(i) = "Value " & l.Renderer.field
    Case 2: lblRend(i) = "Break " & l.Renderer.field
            comboStyle(i).Visible = False
            cmdColor(i).Visible = False
            cmdOutColor(i).Visible = False
  End Select
  
  If TextWidth(lblRend(i).Caption) > lblRend(i).Width Then
    lblRend(i).ToolTipText = lblRend(i).Caption
  Else
    lblRend(i).ToolTipText = ""
  End If
  
  While TextWidth(lblRend(i).Caption) > lblRend(i).Width
    lblRend(i) = Left(lblRend(i), Len(lblRend(i)) - 1)
  Wend
  
  Set l = Nothing
  ProgramSettingCheckbox = False
End Sub

Private Sub txtAlias_Change(Index As Integer)
  MLay(Index).Name = txtAlias(Index).Text
  LayerInfo(Index).Name = txtAlias(Index).Text
End Sub

Private Sub txtAlias_Click(Index As Integer)
  SelectLayer Index
End Sub

Private Sub VScroll_Change()
  VScroll_Scroll
End Sub

Private Sub VScroll_Scroll()
  If VScroll.Visible Then
    AnimLayersResize Me
  End If
End Sub

