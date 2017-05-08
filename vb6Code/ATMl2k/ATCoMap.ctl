VERSION 5.00
Begin VB.UserControl ATCoMap 
   ClientHeight    =   7875
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   6930
   ScaleHeight     =   7875
   ScaleWidth      =   6930
   ToolboxBitmap   =   "ATCoMap.ctx":0000
   Begin VB.Frame sash 
      Appearance      =   0  'Flat
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      ForeColor       =   &H80000008&
      Height          =   60
      Left            =   0
      MousePointer    =   7  'Size N S
      TabIndex        =   9
      Top             =   5160
      Width           =   6975
   End
   Begin MSComDlg.CommonDialog comMap 
      Left            =   6480
      Top             =   360
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Frame fraMap1 
      BorderStyle     =   0  'None
      Caption         =   "map1Frame"
      Height          =   4695
      Left            =   0
      TabIndex        =   4
      Top             =   480
      Width           =   6855
      Begin MapObjectsLT.Map Map1 
         Height          =   4455
         Left            =   0
         TabIndex        =   3
         Top             =   0
         Width           =   6855
         _Version        =   65536
         _ExtentX        =   12091
         _ExtentY        =   7858
         _StockProps     =   161
         BackColor       =   16777215
         BorderStyle     =   1
         Contents        =   "ATCoMap.ctx":0312
      End
   End
   Begin VB.CommandButton cmdSelLocations 
      Caption         =   "All"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   0
      Left            =   5520
      TabIndex        =   1
      ToolTipText     =   "Select ALL Locations matching Scen&Cons"
      Top             =   60
      Width           =   612
   End
   Begin VB.CommandButton cmdSelLocations 
      Caption         =   "None"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   1
      Left            =   6240
      TabIndex        =   2
      ToolTipText     =   "Select NO Locations"
      Top             =   60
      Width           =   612
   End
   Begin TabDlg.SSTab sstLegend 
      Height          =   2412
      Left            =   0
      TabIndex        =   5
      Top             =   5400
      Visible         =   0   'False
      Width           =   6852
      _ExtentX        =   12091
      _ExtentY        =   4260
      _Version        =   393216
      Tabs            =   2
      Tab             =   1
      TabsPerRow      =   2
      TabHeight       =   420
      BackColor       =   12632256
      ForeColor       =   -2147483630
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      TabCaption(0)   =   "Legend"
      TabPicture(0)   =   "ATCoMap.ctx":032C
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "fraLegend"
      Tab(0).ControlCount=   1
      TabCaption(1)   =   "Tab 1"
      TabPicture(1)   =   "ATCoMap.ctx":0348
      Tab(1).ControlEnabled=   -1  'True
      Tab(1).Control(0)=   "agdMapTable(1)"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).ControlCount=   1
      Begin ATCoCtl.ATCoGrid agdMapTable 
         Height          =   1935
         Index           =   1
         Left            =   120
         TabIndex        =   10
         Top             =   360
         Width           =   6615
         _ExtentX        =   11668
         _ExtentY        =   3413
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   0   'False
         Rows            =   2
         Cols            =   2
         ColWidthMinimum =   300
         gridFontBold    =   0   'False
         gridFontItalic  =   0   'False
         gridFontName    =   "MS Sans Serif"
         gridFontSize    =   8
         gridFontUnderline=   0   'False
         gridFontWeight  =   400
         gridFontWidth   =   0
         Header          =   ""
         FixedRows       =   1
         FixedCols       =   0
         ScrollBars      =   3
         SelectionMode   =   4
         BackColor       =   -2147483643
         ForeColor       =   0
         BackColorBkg    =   -2147483633
         BackColorSel    =   -2147483635
         ForeColorSel    =   16777215
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   0
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         ComboCheckValidValues=   -1  'True
      End
      Begin VB.Frame fraLegend 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   1935
         HelpContextID   =   513
         Left            =   -74880
         TabIndex        =   7
         Top             =   360
         Width           =   6615
         Begin VB.VScrollBar LegendScroll 
            Height          =   1935
            LargeChange     =   5
            Left            =   6360
            TabIndex        =   11
            Top             =   0
            Visible         =   0   'False
            Width           =   255
         End
         Begin VB.PictureBox pctLegend 
            Appearance      =   0  'Flat
            AutoRedraw      =   -1  'True
            BackColor       =   &H00C0C0C0&
            BorderStyle     =   0  'None
            FillColor       =   &H00C0C0C0&
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H80000000&
            Height          =   255
            Index           =   0
            Left            =   240
            ScaleHeight     =   255
            ScaleWidth      =   255
            TabIndex        =   12
            TabStop         =   0   'False
            Top             =   240
            Width           =   255
         End
         Begin VB.Label lblLegend 
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   0
            Left            =   600
            TabIndex        =   8
            Top             =   240
            Width           =   2355
         End
      End
   End
   Begin MSComctlLib.ImageList imgListTools 
      Left            =   4320
      Top             =   0
      _ExtentX        =   794
      _ExtentY        =   794
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   8421376
      _Version        =   393216
      BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
         NumListImages   =   23
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":0364
            Key             =   "Zoom"
         EndProperty
         BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":08B6
            Key             =   "Pointer"
         EndProperty
         BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":096E
            Key             =   "ZoomIn"
         EndProperty
         BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":0EC0
            Key             =   "ZoomOut"
         EndProperty
         BeginProperty ListImage5 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":1412
            Key             =   "Print"
         EndProperty
         BeginProperty ListImage6 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":172C
            Key             =   "Pan"
         EndProperty
         BeginProperty ListImage7 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":1C7E
            Key             =   "FullExtent"
         EndProperty
         BeginProperty ListImage8 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":21D0
            Key             =   "Identify"
         EndProperty
         BeginProperty ListImage9 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":2722
            Key             =   "Left"
         EndProperty
         BeginProperty ListImage10 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":2A3C
            Key             =   "Right"
         EndProperty
         BeginProperty ListImage11 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":2D56
            Key             =   "Select"
         EndProperty
         BeginProperty ListImage12 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":3070
            Key             =   "Unselect"
         EndProperty
         BeginProperty ListImage13 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":338A
            Key             =   "Get"
         EndProperty
         BeginProperty ListImage14 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":36A4
            Key             =   "Save"
         EndProperty
         BeginProperty ListImage15 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":39BE
            Key             =   "PrintFolder"
         EndProperty
         BeginProperty ListImage16 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":3CD8
            Key             =   "Add"
         EndProperty
         BeginProperty ListImage17 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":3FF2
            Key             =   "Remove"
         EndProperty
         BeginProperty ListImage18 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":430C
            Key             =   "Properties"
         EndProperty
         BeginProperty ListImage19 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":4626
            Key             =   "Clear"
         EndProperty
         BeginProperty ListImage20 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":4940
            Key             =   "Up"
         EndProperty
         BeginProperty ListImage21 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":4C5A
            Key             =   "Down"
         EndProperty
         BeginProperty ListImage22 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":4F74
            Key             =   "Edit"
         EndProperty
         BeginProperty ListImage23 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCoMap.ctx":528E
            Key             =   "Move"
         EndProperty
      EndProperty
   End
   Begin MSComctlLib.Toolbar tbrMap1 
      Height          =   312
      Left            =   0
      TabIndex        =   0
      Top             =   36
      Width           =   5004
      _ExtentX        =   8837
      _ExtentY        =   556
      ButtonWidth     =   487
      ButtonHeight    =   466
      Wrappable       =   0   'False
      ImageList       =   "imgListTools"
      _Version        =   393216
      BeginProperty Buttons {66833FE8-8583-11D1-B16A-00C0F0283628} 
         NumButtons      =   15
         BeginProperty Button1 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Zoom"
            Object.ToolTipText     =   "Zoom"
            Object.Tag             =   "0"
         EndProperty
         BeginProperty Button2 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "ZoomIn"
            Object.ToolTipText     =   "Zoom In"
            Object.Tag             =   "10"
         EndProperty
         BeginProperty Button3 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "ZoomOut"
            Object.ToolTipText     =   "Zoom Out"
            Object.Tag             =   "20"
         EndProperty
         BeginProperty Button4 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Pan"
            Object.ToolTipText     =   "Pan"
            Object.Tag             =   "30"
         EndProperty
         BeginProperty Button5 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "FullExtent"
            Object.ToolTipText     =   "Full Extent"
            Object.Tag             =   "40"
         EndProperty
         BeginProperty Button6 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Separator1"
            Style           =   3
            MixedState      =   -1  'True
         EndProperty
         BeginProperty Button7 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Identify"
            Object.ToolTipText     =   "Identify"
            Object.Tag             =   "60"
         EndProperty
         BeginProperty Button8 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Edit"
            Object.ToolTipText     =   "Edit Layers"
            Object.Tag             =   "300"
         EndProperty
         BeginProperty Button9 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Add"
            Object.ToolTipText     =   "Add Location"
            Object.Tag             =   "200"
         EndProperty
         BeginProperty Button10 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Move"
            Object.ToolTipText     =   "Move Location"
            Object.Tag             =   "220"
         EndProperty
         BeginProperty Button11 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Separator2"
            Style           =   3
            MixedState      =   -1  'True
         EndProperty
         BeginProperty Button12 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Save"
            Object.ToolTipText     =   "Save Map"
            Object.Tag             =   "80"
         EndProperty
         BeginProperty Button13 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Get"
            Object.ToolTipText     =   "Get Map Specs"
            Object.Tag             =   "90"
         EndProperty
         BeginProperty Button14 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Separator3"
            Style           =   3
            MixedState      =   -1  'True
         EndProperty
         BeginProperty Button15 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Key             =   "Print"
            Object.ToolTipText     =   "Print Map"
            Object.Tag             =   "110"
         EndProperty
      EndProperty
   End
   Begin VB.Label lblNselected 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   192
      Left            =   5400
      TabIndex        =   6
      Top             =   120
      Width           =   60
   End
   Begin VB.Image imgZoom 
      Height          =   480
      Left            =   4320
      Picture         =   "ATCoMap.ctx":55A8
      Top             =   0
      Visible         =   0   'False
      Width           =   480
   End
   Begin VB.Image imgPan 
      Height          =   480
      Left            =   5520
      Picture         =   "ATCoMap.ctx":58B2
      Top             =   240
      Visible         =   0   'False
      Width           =   480
   End
   Begin VB.Image imgMovePoint 
      Height          =   480
      Left            =   6000
      Picture         =   "ATCoMap.ctx":617C
      Top             =   120
      Visible         =   0   'False
      Width           =   480
   End
End
Attribute VB_Name = "ATCoMap"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Public Enum ATCoRendType
  ATCoNoRend = 0
  ATCoValueRend = 1
  ATCoClassBreakRend = 2
End Enum

'Private pDebug As ATCoDebug
Private pIPC As ATCoIPC ' AtCoLaunch
Private LayerInfo() As MapLayerInfo

Private pMapFileName$ 'file specifying layers and properties, from last SetMapData call
Private pMapFilePath$

Private curLay&
Private nLayers&
Private TrackingNearestShape As Boolean 'If true, closest shape to mouse in curLay is highlighted yellow
Private SashDragging As Boolean

'if true, selection does not happen until program calls SetSelectedByKey, probably in response to a SelectionChange event
Private ConfirmSelect As Boolean
Private DelaySelectionChange As Boolean
Private pStreamSelectMode As Long

Private MapMovingPoint& 'index of shape in curLay now being moved

Private RefreshLayer As Boolean 'True if we only refresh current layer when highlighting
Private RecentlyRefreshedLayer As Boolean 'True if we did a RefreshLayer more recently than Refresh
                     'Neccessary because of a bug in MoLT where only the most recent layer is drawn

Private CurrentMapShape As MapObjectsLT.Shape
Private CurrentMapShapeName$
Private CurrentMapSymbol As New symbol
Private CurShapeFldVals As MapObjectsLT.Strings

Private SelectedMapShapeId$()
Private SelectedMapShape() As MapObjectsLT.Shape
Private SelectedMapSymbol() As symbol
Private maxSelectedShapes&

Private Map1_MouseDownX&, Map1_MouseDownY&
Private MapHighlightColor&
Private UserFullExtent As Rectangle
Private LegendIsVisible As Boolean
Private ToolbarIsVisible As Boolean
Private pUnsavedChanges As Boolean
Private pDeletingPoint As Boolean

'Private MapPtLay& 'temporary, need to remove
'Private MapRS As Recordset

Private Type MapSaveInfo
  sym As symbol
  vRend As ValueMapRenderer
  cRend As ClassBreaksRenderer
End Type

Public Event SelectionChange(FeatureID$, Layer&, state As Boolean)
Public Event AddedShape(ShapeType&, dbfKeyVal$, ByVal x#, ByVal y#)
Public Event ChangedLayers() 'Whenever layers are added, removed, edited

Private Sub DbgMsg(Msg$)
  If (pIPC Is Nothing) Then
    Debug.Print "ATCoMap:" & Msg
    'MsgBox Msg
  Else
    pIPC.dbg "ATCoMap:" & Msg
  End If
End Sub

Public Property Set DebugControl(newDebug As ATCoCtl.ATCoDebug)
  'Set pDebug = newDebug
End Property

Public Property Set Launch(newLaunch As ATCoCtl.ATCoLaunch)
  'Set pLaunch = newLaunch
End Property

Public Property Set IPC(newIPC As ATCoCtl.ATCoIPC)
  Set pIPC = newIPC
End Property

Public Property Get ConfirmSelections() As Boolean
  ConfirmSelections = ConfirmSelect
End Property

Public Property Let ConfirmSelections(ByVal newValue As Boolean)
  ConfirmSelect = newValue
End Property

Public Property Get MapFileName$()
  MapFileName = pMapFileName
End Property

Public Property Get MapFilePath$()
  MapFilePath = pMapFilePath
End Property

'0=No stream-following selection, clicking toggles one location's selection
'1=Toggle entire branch
'2=Toggle clicked node/segment and all downstream
Public Property Get StreamSelectMode() As Long
  StreamSelectMode = pStreamSelectMode
End Property
Public Property Let StreamSelectMode(ByVal newValue As Long)
  pStreamSelectMode = newValue
End Property

Public Property Get LayerName$(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerName = LayerInfo(layerindex).Name
End Property
Public Property Let LayerName(ByVal layerindex&, ByVal newValue$)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).Name <> newValue Then
    LayerInfo(layerindex).Name = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerPath$(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerPath = LayerInfo(layerindex).path
End Property

Public Property Let LayerPath(ByVal layerindex&, ByVal newValue$)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).path <> newValue Then
    LayerInfo(layerindex).path = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerFilename$(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerFilename = LayerInfo(layerindex).baseFilename
End Property

Public Property Let LayerFilename(ByVal layerindex&, ByVal newValue$)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).baseFilename <> newValue Then
    LayerInfo(layerindex).baseFilename = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerRendType(ByVal layerindex&) As ATCoRendType
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerRendType = LayerInfo(layerindex).RendType
End Property

Public Property Let LayerRendType(ByVal layerindex&, ByVal newValue As ATCoRendType)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).RendType <> newValue Then
    LayerInfo(layerindex).RendType = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerKeyField$(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerKeyField = LayerInfo(layerindex).keyField
End Property

Public Property Let LayerKeyField(ByVal layerindex&, ByVal newValue$)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).keyField <> newValue Then
    LayerInfo(layerindex).keyField = newValue
    UnsavedChanges = True
  End If
End Property

'Returns column in grid containing the key values for this layer or -1 on failure
Public Property Get LayerKeyCol&(ByVal layerindex&)
  Dim c&, f&
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  f = 0
  c = -1
  While f < LayerInfo(layerindex).nFields
    If LayerInfo(layerindex).Fields(f).Name = LayerInfo(layerindex).keyField Then
      c = LayerInfo(layerindex).Fields(f).Column
      f = LayerInfo(layerindex).nFields
    Else
      f = f + 1
    End If
  Wend
  LayerKeyCol = c
End Property

Public Property Get LayerLabelField$(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerLabelField = LayerInfo(layerindex).LabelField
End Property

Public Property Let LayerLabelField(ByVal layerindex&, ByVal newValue$)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).LabelField <> newValue Then
    LayerInfo(layerindex).LabelField = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerBranchField$(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerBranchField = LayerInfo(layerindex).BranchField
End Property

Public Property Let LayerBranchField(ByVal layerindex&, ByVal newValue$)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).BranchField <> newValue Then
    LayerInfo(layerindex).BranchField = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerDownIDField$(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerDownIDField = LayerInfo(layerindex).DownIDField
End Property

Public Property Let LayerDownIDField(ByVal layerindex&, ByVal newValue$)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).DownIDField <> newValue Then
    LayerInfo(layerindex).DownIDField = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerLengthField$(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerLengthField = LayerInfo(layerindex).LengthField
End Property

Public Property Let LayerLengthField(ByVal layerindex&, ByVal newValue$)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).LengthField <> newValue Then
    LayerInfo(layerindex).LengthField = newValue
    UnsavedChanges = True
  End If
End Property
Public Property Get LayerTableIndex&(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerTableIndex = LayerInfo(layerindex).TableIndex
End Property

Public Property Let LayerTableIndex(ByVal layerindex&, ByVal newValue&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).TableIndex <> newValue Then
    LayerInfo(layerindex).TableIndex = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerSelCount&(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerSelCount = LayerInfo(layerindex).nSelected
End Property

Public Property Get ShapeSelected(ByVal Layer&, ByVal item&) As Boolean
  If Layer >= nLayers Or Layer < 0 Then Exit Property
  If item < 0 Or item > UBound(LayerInfo(Layer).Selected) Then Exit Property
  ShapeSelected = LayerInfo(Layer).Selected(item)
End Property

Public Property Let ShapeSelected(ByVal Layer&, ByVal item&, ByVal state As Boolean)
  If Layer >= nLayers Or Layer < 0 Then Exit Property
  DbgMsg "Let ShapeSelected(" & Layer & ", " & item & ") = " & state
  With LayerInfo(Layer)
    If item < 0 Then Exit Property
    If item > UBound(.Selected) Then ReDim Preserve .Selected(item): DbgMsg "ReDim Preserve .Selected(" & item & ")"
    If state <> .Selected(item) Then
      If Not ConfirmSelect Then ReallySetSelected Layer, item, state
      If Not DelaySelectionChange Then
        Dim c&, FeatureID$
        c = LayerKeyCol(Layer)
        If c < 0 Then FeatureID = item Else FeatureID = agdMapTable(.TableIndex).TextMatrix(item, c)
        RaiseEvent SelectionChange(FeatureID, Layer, state)
      End If
    End If
  End With
End Property

Public Property Get LayerAnimate(ByVal layerindex&) As Boolean
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerAnimate = LayerInfo(layerindex).Animate
End Property

Public Property Let LayerAnimate(ByVal layerindex&, ByVal newValue As Boolean)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If LayerInfo(layerindex).Animate <> newValue Then
    LayerInfo(layerindex).Animate = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerFieldCount&(ByVal layerindex&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  LayerFieldCount = LayerInfo(layerindex).nFields
End Property

Public Property Let LayerFieldCount(ByVal layerindex&, ByVal newValue&)
  If layerindex >= nLayers Or layerindex < 0 Then Exit Property
  If newValue < 0 Then Exit Property
  If newValue <> LayerInfo(layerindex).nFields Then
    If newValue > 0 Then
      ReDim Preserve LayerInfo(layerindex).Fields(0 To newValue - 1)
      LayerInfo(layerindex).nFields = newValue
    Else
      ReDim LayerInfo(layerindex).Fields(0)
      LayerInfo(layerindex).nFields = 0
    End If
  End If
End Property

Public Property Get LayerFieldName$(ByVal Layer&, ByVal field&)
  If Layer >= nLayers Or Layer < 0 Then Exit Property
  If field >= LayerInfo(Layer).nFields Or field < 0 Then Exit Property
  LayerFieldName = LayerInfo(Layer).Fields(field).Name
End Property

Public Property Let LayerFieldName(ByVal Layer&, ByVal field&, ByVal newValue$)
  If Layer >= nLayers Or Layer < 0 Then Exit Property
  If field >= LayerInfo(Layer).nFields Or field < 0 Then Exit Property
  If LayerInfo(Layer).Fields(field).Name <> newValue Then
    LayerInfo(Layer).Fields(field).Name = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerFieldCaption$(ByVal Layer&, ByVal field&)
  If Layer >= nLayers Or Layer < 0 Then Exit Property
  If field >= LayerInfo(Layer).nFields Or field < 0 Then Exit Property
  LayerFieldCaption = LayerInfo(Layer).Fields(field).Caption
End Property

Public Property Let LayerFieldCaption(ByVal Layer&, ByVal field&, ByVal newValue$)
  If Layer >= nLayers Or Layer < 0 Then Exit Property
  If field >= LayerInfo(Layer).nFields Or field < 0 Then Exit Property
  If LayerInfo(Layer).Fields(field).Caption <> newValue Then
    LayerInfo(Layer).Fields(field).Caption = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerFieldColumn&(ByVal Layer&, ByVal field&)
  If Layer >= nLayers Or Layer < 0 Then Exit Property
  If field >= LayerInfo(Layer).nFields Or field < 0 Then Exit Property
  LayerFieldColumn = LayerInfo(Layer).Fields(field).Column
End Property

Public Property Let LayerFieldColumn(ByVal Layer&, ByVal field&, ByVal newValue&)
  If Layer >= nLayers Or Layer < 0 Then Exit Property
  If field >= LayerInfo(Layer).nFields Or field < 0 Then Exit Property
  If LayerInfo(Layer).Fields(field).Column <> newValue Then
    LayerInfo(Layer).Fields(field).Column = newValue
    UnsavedChanges = True
  End If
End Property

Public Property Get LayerCount&()
  LayerCount = nLayers
End Property

Public Property Let LayerCount(ByVal newValue&)
  Dim layr As Long
  If newValue <> nLayers Then
    If newValue > 0 Then
      ReDim Preserve LayerInfo(0 To newValue - 1)
      For layr = nLayers To newValue - 1
        ReDim LayerInfo(layr).Selected(0)
        ReDim LayerInfo(layr).Fields(0)
      Next
      UnsavedChanges = True
    Else
      ReDim LayerInfo(0)
      UnsavedChanges = False
    End If
    nLayers = newValue
  End If
End Property

Public Property Get CurLayer&()
  CurLayer = curLay
End Property

Public Property Let CurLayer(ByVal newValue&)
  Dim LayerName$, l& ', oldtab%
  DbgMsg "Let CurLayer = " & newValue
  'oldtab = sstLegend.Tab
  If newValue < 0 Or newValue >= nLayers Then 'switch to legend tab if they ask for a layer < 0
    sstLegend.Tab = 0 'sstLegend_Click oldtab
  Else
    curLay = newValue
    LayerName = LayerInfo(curLay).Name
    For l = 1 To sstLegend.Tabs - 1 'skip checking tab 0 (Legend)
      If LayerName = sstLegend.TabCaption(l) Then sstLegend.Tab = l
    Next l
  End If
End Property

Public Property Get Map() As Object 'Map
  Set Map = Map1
End Property

Public Property Get RefreshMapLayer() As Boolean
  RefreshMapLayer = RefreshLayer
End Property

Public Property Let RefreshMapLayer(ByVal newValue As Boolean)
  RefreshLayer = newValue
End Property

'buttonID can be the name of a button "Pan" or the 1-based index
Public Property Get ButtonVisible(ByVal buttonID As Variant) As Boolean
  On Error GoTo NoSuchButton
  ButtonVisible = tbrMap1.Buttons(buttonID).Visible
  Exit Property
NoSuchButton:
  ButtonVisible = False
End Property

Public Property Let ButtonVisible(ByVal buttonID As Variant, ByVal state As Boolean)
  DbgMsg "Let ButtonVisible " & buttonID & " = " & state
  On Error GoTo NoSuchButton
  tbrMap1.Buttons(buttonID).Visible = state
  With tbrMap1.Buttons(tbrMap1.Buttons.Count)
    tbrMap1.Width = .Left + .Width
  End With
  Exit Property
NoSuchButton:
End Property

Public Property Get LegendVisible() As Boolean
  LegendVisible = LegendIsVisible
End Property

Public Property Let LegendVisible(ByVal state As Boolean)
  LegendIsVisible = state
  cmdSelLocations(0).Visible = state
  cmdSelLocations(1).Visible = state
  sash.Visible = state
  If Map1.Visible Then
    If LegendIsVisible Then
      If Enabled Then sstLegend.Visible = True
      cmdSelLocations(0).Visible = True
      cmdSelLocations(1).Visible = True
    End If
  End If
  UserControl_Resize
End Property

Public Property Get ToolbarVisible() As Boolean
  ToolbarVisible = ToolbarIsVisible
End Property

Public Property Let ToolbarVisible(ByVal state As Boolean)
  ToolbarIsVisible = state
  'If Map1.Visible Then
  tbrMap1.Visible = state
End Property

Public Property Let UnsavedChanges(ByVal newValue As Boolean)
  pUnsavedChanges = newValue
End Property

'True if changes have been made that would affect map file
Public Property Get UnsavedChanges() As Boolean
  UnsavedChanges = pUnsavedChanges
End Property

Public Property Get TrackNearestShape() As Boolean
  TrackNearestShape = TrackingNearestShape
End Property

Public Property Let TrackNearestShape(ByVal newValue As Boolean)
  TrackingNearestShape = newValue
End Property

Public Property Get Highlight$()
  Highlight = CurrentMapShapeName
End Property

Public Property Let Highlight(ByVal keyValue$)
  Dim c$
  DbgMsg "Highlight " & keyValue
  If curLay >= nLayers Then Exit Property 'only happens on LostFocus after close project
  DbgMsg "Highlight: " & keyValue
  If Len(keyValue) < 1 Then
    If Len(CurrentMapShapeName) > 0 Then
      CurrentMapShapeName = ""
      Set CurrentMapShape = Nothing
      If RefreshLayer Then Map1.RefreshLayer curLay Else Map1.Refresh
      RecentlyRefreshedLayer = RefreshLayer
    End If
  ElseIf keyValue <> CurrentMapShapeName Then
    If InStr(1, keyValue, "=") > 0 Then
      Map1.ToolTipText = "" 'Don't display text when we are just looking by FeatureID
    Else
      Map1.ToolTipText = keyValue
      If Len(LayerInfo(CurLayer).keyField) > 1 Then
        keyValue = LayerInfo(CurLayer).keyField & " = '" & keyValue & "'"
      Else
        'What should we search for? Should this be an error?
      End If
    End If
    'If Len(CurrentMapShapeName) > 0 Then Map1.DrawShape CurrentMapShape, CurrentMapSymbol
    Map1.SetFocus
    Set CurrentMapShape = Nothing
    Set CurrentMapShape = Map1.Layers(curLay).Find(keyValue)
    CurrentMapShapeName = keyValue
    CurrentMapSymbol.SymbolType = Map1.Layers(curLay).symbol.SymbolType
    CurrentMapSymbol.Color = Map1.Layers(curLay).symbol.Color
    CurrentMapSymbol.Style = Map1.Layers(curLay).symbol.Style
    CurrentMapSymbol.size = Map1.Layers(curLay).symbol.size
    If RefreshLayer Then Map1.RefreshLayer curLay Else Map1.Refresh
    RecentlyRefreshedLayer = RefreshLayer
  End If
  On Error GoTo ExitSub
  If CurShapeFldVals.Count > 0 And Len(LayerInfo(curLay).LabelField) > 0 Then
    Dim nameIndex&, found As Boolean
    nameIndex = 0: found = False
    While nameIndex < CurShapeFldVals.Count And Not found
      If Map1.Layers(curLay).Fields(nameIndex) = LayerInfo(curLay).LabelField Then found = True Else nameIndex = nameIndex + 1
    Wend
    If found Then
      If Len(CurShapeFldVals(nameIndex)) > 0 Then
        Map1.ToolTipText = CurShapeFldVals(nameIndex)
      Else
        nameIndex = 0: found = False
        While nameIndex < CurShapeFldVals.Count And Not found
          If Map1.Layers(curLay).Fields(nameIndex) = LayerInfo(curLay).keyField Then found = True Else nameIndex = nameIndex + 1
        Wend
        If found Then
          If Len(CurShapeFldVals(nameIndex)) > 0 Then Map1.ToolTipText = CurShapeFldVals(nameIndex)
        End If
      End If
    End If
  End If
ExitSub:
End Property

Private Sub HighlightCurrentShape(ByVal SizeMultiplier As Single)
  Dim sym As New symbol
  Set sym = CurrentMapSymbol
  sym.Color = moYellow  'MapHighlightColor
  sym.size = Map1.Layers(curLay).symbol.size * SizeMultiplier
  On Error Resume Next
  Map1.DrawShape CurrentMapShape, sym
  On Error GoTo 0
  Set sym = Nothing
End Sub

Private Sub HighlightSelectedShapes()
  Dim selShpNum&, sym As New symbol
  
  For selShpNum = 1 To LayerInfo(curLay).nSelected
    Set sym = SelectedMapSymbol(selShpNum)
    sym.Color = moRed
    On Error Resume Next
    Map1.DrawShape SelectedMapShape(selShpNum), sym
    On Error GoTo 0
  Next selShpNum
  Set sym = Nothing
End Sub

Public Property Get Enabled() As Boolean
  Enabled = cmdSelLocations(0).Enabled
End Property

Public Property Let Enabled(ByVal ena As Boolean)
  Dim i&
  For i = 1 To tbrMap1.Buttons.Count
    'If ToolbarIsVisible Then
      tbrMap1.Buttons(i).Enabled = ena
    'Else
    '  tbrMap1.Buttons(i).Visible = False
    'End If
  Next i
  If ena Then
    Map1.BackColor = moWhite
  Else
    lblNselected.Caption = ""
    Map1.BackColor = vb3DLight
  End If
  
  cmdSelLocations(0).Enabled = ena
  cmdSelLocations(1).Enabled = ena
  If LegendIsVisible Then
    sstLegend.Visible = ena
    cmdSelLocations(0).Visible = True
    cmdSelLocations(1).Visible = True
  Else
    sstLegend.Visible = False
    cmdSelLocations(0).Visible = False
    cmdSelLocations(1).Visible = False
  End If
End Property

Public Property Get SelCount&()
  Dim l&, cnt&
  cnt = 0
  For l = 0 To nLayers - 1
    cnt = cnt + LayerInfo(l).nSelected
  Next l
  SelCount = cnt
  DbgMsg "SelCount = " & cnt
End Property

'if control < 0, set focus to map
'if control = 0, set focus to legend
'if control > 0, set focus to controlth grid
Public Sub SetControlFocus(ByVal control&)
  DbgMsg "SetControlFocus"
  If control < 0 Then
    Map1.SetFocus
  Else
    If control < sstLegend.Tabs Then sstLegend.Tab = control
    If sstLegend.Tab > 0 Then
      agdMapTable(sstLegend.Tab).SetFocus
    Else
      sstLegend.SetFocus
    End If
  End If
End Sub

Public Sub GetSelectedKeys(ByRef keys$())
  Dim l&, r&, k&, c&
  DbgMsg "GetSelectedKeys"
  k = 0
  ReDim keys(SelCount)
  For l = 0 To nLayers - 1
    If LayerInfo(l).nSelected > 0 Then
      c = LayerKeyCol(l)
      If c >= 0 Then
        With agdMapTable(LayerInfo(l).TableIndex)
          For r = 1 To .rows
            If LayerInfo(l).Selected(r) Then
              keys(k) = .TextMatrix(r, c)
              k = k + 1
            End If
          Next r
        End With
      End If
    End If
  Next l
End Sub

Public Sub Clear()
  DbgMsg "Clear"
  If Map1.Layers.Count > 0 Then
    Dim t&
    Map1.Layers.Clear
    Map1.BackColor = &HC0C0C0
    sstLegend.Tab = 0
    For t = 1 To sstLegend.Tabs - 1
      sstLegend.TabCaption(t) = ""
      sstLegend.TabEnabled(t) = False
    Next t
    UnloadLegend
    UnsavedChanges = True
  End If
  LayerCount = 0
  'pMapFileName = ""
  'pMapFilePath = ""
  ReDim LayerInfo(0)
  'Enabled = False
End Sub

'Do not use this from outside ATCoMap, it will disappear in the next version
Public Sub AddShapeLayer(ByVal path$, ByVal baseFilename$, ByVal label$, _
                          ByVal sym As Object, ByVal Visibl As Boolean, _
                          ByVal Animate As Boolean, ByVal dbKey$, ByVal dbname$)
  NewAddShapeLayer path, baseFilename, label, sym, Visibl, Animate, dbKey, dbname, "", "", ""
End Sub

'AddShapeLayer example:
'AddShapeLayer("C:\maps\shena", "gage", "Shena Gages")
'will load the shapefile specified by gage.shp, gage.shx, and gage.dbf in C:\maps\shena
Private Sub NewAddShapeLayer(ByVal path$, ByVal baseFilename$, ByVal label$, _
                          ByVal sym As Object, ByVal Visibl As Boolean, _
                          ByVal Animate As Boolean, ByVal dbKey$, ByVal dbname$, _
                          ByVal dbBranchFld$, ByVal dbDownIDFld$, ByVal dbLengthFld$)
  DbgMsg "NewAddShapeLayer " & path & ", " & baseFilename & ", " & label & _
         ", " & Visibl & ", " & Animate & ", " & dbKey & ", " & dbname & _
         ", " & dbBranchFld & ", " & dbDownIDFld & ", " & dbLengthFld
  If Len(path) = 0 Then
    path = PathNameOnly(baseFilename)
    baseFilename = FilenameOnly(baseFilename)
    If Len(path) = 0 Then path = CurDir
  End If
  If Len(path) > 0 And Right(path, 1) <> "\" Then path = path & "\"
  If InStr(1, baseFilename, ":") Then path = ""
  If Len(baseFilename) > 3 Then
    If Mid(baseFilename, Len(baseFilename) - 3, 1) = "." Then baseFilename = Left(baseFilename, Len(baseFilename) - 4)
  End If
  ReDim Preserve LayerInfo(0 To nLayers)
  nLayers = nLayers + 1
  Dim l&
  For l = nLayers - 1 To 1 Step -1
    LayerInfo(l) = LayerInfo(l - 1)
  Next l
    
'#If LT Then
'sstLegend.TabCaption(0) = "LT"
'#Else
'sstLegend.TabCaption(0) = "not LT"
'#End If
  
  With LayerInfo(0)
    .Name = label
    .path = path
    .baseFilename = baseFilename
    .keyField = dbKey
    If Len(dbname) > 0 Then .LabelField = dbname Else .LabelField = dbKey
    .RendType = 0
    .TableIndex = 0
    .nFields = 0
    .nSelected = 0
    ReDim .Selected(0)
    .Animate = Animate
    .BranchField = dbBranchFld
    .DownIDField = dbDownIDFld
    .LengthField = dbLengthFld
    If Visibl Then .TableIndex = 0 Else .TableIndex = -1
  End With
  
  Dim newLayer As New MapLayer
  newLayer.File = path & baseFilename & ".shp"
  newLayer.Name = label
  newLayer.symbol.Style = sym.Style
  newLayer.symbol.Color = sym.Color
  If newLayer.symbol.SymbolType <> moLineSymbol Then newLayer.symbol.OutlineColor = sym.OutlineColor
  'If Len(dbname) > 0 Then newLayer.LabelField = dbname
  newLayer.Visible = Visibl
  Map1.Layers.Add newLayer
  'Map1.Layers.MoveTo 0, nLayers - 1
  'If Map1.Layers(0).symbol.SymbolType = moPointSymbol Then curLay = nLayers
  Set newLayer = Nothing
  UnsavedChanges = True
End Sub

Private Sub SelectHighlightedPoint(ByVal opt&)
  If sstLegend.Tab > 0 Then
    Dim makeSelected As Boolean
    Select Case opt
      Case -1: DbgMsg "SelectHighlightedPoint (toggle)"
      Case 1:  DbgMsg "SelectHighlightedPoint (select)"
      Case 0:  DbgMsg "SelectHighlightedPoint (unselect)"
    End Select
    
    Dim r&, oldSelected As Boolean
    r = agdMapTable(LayerInfo(curLay).TableIndex).row
    oldSelected = LayerInfo(curLay).Selected(r)
    Select Case opt
      Case -1: makeSelected = (Not oldSelected) 'toggle
      Case 0:  makeSelected = False 'select
      Case 1:  makeSelected = True  'unselect
    End Select
    If makeSelected <> oldSelected Then 'change needed
      Dim MapRS As Recordset
      Dim fld As String
      Dim keyfld As String
      Dim Val As String
      Dim rec As Long
      ShapeSelected(curLay, r) = makeSelected
      Select Case StreamSelectMode
        Case 1 'toggle branch
          DelaySelectionChange = True
          MousePointer = vbHourglass
          fld = LayerInfo(curLay).BranchField
          If Len(fld) = 0 Then
            If MsgBox("Can't select branch for this layer because a branch field" & vbCr _
                    & "has not been specified. Specify a branch field now?", vbYesNo, "Select Branch") = vbYes Then
              MapAction 300
              MsgBox "Select the layer of interest, then click 'Fields' to specify a branch field.", vbOKOnly, "Select Branch"
            End If
          Else
            Set MapRS = OpenDBF(LayerInfo(curLay).path & LayerInfo(curLay).baseFilename & ".dbf", True)
            On Error GoTo closedb1
            If Not IsNull(MapRS) Then
              MapRS.MoveFirst
              MapRS.Move r
              Val = MapRS(fld)
              MapRS.MoveFirst
              rec = 1
              While Not MapRS.EOF
                If MapRS(fld) = Val Then
                  oldSelected = LayerInfo(curLay).Selected(rec)
                  If oldSelected <> makeSelected Then
                    ShapeSelected(curLay, rec) = makeSelected
                  End If
                End If
                MapRS.MoveNext
                rec = rec + 1
              Wend
closedb1:
              On Error Resume Next
              MapRS.Close
            End If
          End If
          DelaySelectionChange = False
          RaiseEvent SelectionChange("branch", CurLayer, makeSelected)
          Map1.Refresh
          MousePointer = vbDefault
        Case 2 'toggle downstream
          DelaySelectionChange = True
          MousePointer = vbHourglass
          fld = LayerInfo(curLay).DownIDField
          keyfld = LayerInfo(curLay).keyField
          If Len(fld) = 0 Then
            If MsgBox("Can't select downstream for this layer because a downstream field" & vbCr _
                    & "has not been specified. Specify a downstream field now?", vbYesNo, "Select Downstream") = vbYes Then
              MapAction 300
              MsgBox "Select the layer of interest, then click 'Fields' to specify a downstream field.", vbOKOnly, "Select Branch"
            End If
          ElseIf Len(keyfld) = 0 Then
            If MsgBox("Can't select downstream for this layer because a key field" & vbCr _
                    & "has not been specified. Specify a key field now?", vbYesNo, "Select Downstream") = vbYes Then
              MapAction 300
              MsgBox "Select the layer of interest, then click 'Fields' to specify a key field." _
                   & "Key values should correspond to values in the Downstream ID field.", vbOKOnly, "Select Key"
            End If
          Else
            Set MapRS = OpenDBF(LayerInfo(curLay).path & LayerInfo(curLay).baseFilename & ".dbf", True)
            On Error GoTo closedb2
            If Not IsNull(MapRS) Then
              MapRS.MoveFirst
              MapRS.Move r
              Val = MapRS(fld)
              
              While Val <> "0" And Len(Val) > 0
                MapRS.MoveFirst
                rec = 1
                While Not MapRS.EOF
                  If MapRS(keyfld) = Val Then
                    oldSelected = LayerInfo(curLay).Selected(rec)
                    If oldSelected <> makeSelected Then
                      ShapeSelected(curLay, rec) = makeSelected
                    End If
                    GoTo NextVal
                  End If
                  MapRS.MoveNext
                  rec = rec + 1
                Wend
NextVal:
                Val = ""
                If Not MapRS.EOF Then
                  If Not IsNull(MapRS(fld)) Then Val = MapRS(fld)
                End If
              Wend
closedb2:
              On Error Resume Next
              MapRS.Close
            End If
          End If
          DelaySelectionChange = False
          RaiseEvent SelectionChange("downstream", CurLayer, makeSelected)
          Map1.Refresh
          MousePointer = vbDefault
      End Select
    End If
  End If
  
  Exit Sub
  
ErrHand:
  MsgBox "Error in selection" & vbCr & Err.Description, vbOKOnly, "Map Select"
End Sub

Public Sub SelectAll(ByVal layerindex&, ByVal state As Boolean)
  DbgMsg "SelectAll " & layerindex & ", " & state
  With LayerInfo(layerindex)
    If .TableIndex > 0 And .TableIndex < 99 Then
    
      'if none or all are already selected, we don't have to do anything
      If Not state And .nSelected = 0 Then Exit Sub
      If state And .nSelected = UBound(.Selected) Then Exit Sub
      
      MousePointer = ccHourglass
      DelaySelectionChange = True
      Dim r& ', saveRefreshLayer As Boolean
      Dim startTime As Single
      startTime = Timer
      'saveRefreshLayer = RefreshLayer
      'RefreshLayer = True 'Optimization to speed up display and reduce flashing
      For r = 1 To agdMapTable(.TableIndex).rows
        ShapeSelected(layerindex, r) = state
        If Timer - startTime > 5 Then
          If MsgBox("This selection operation is taking a long time." & vbCrLf & "Currently " & r & "/" & agdMapTable(.TableIndex).rows & " complete. Continue?", vbYesNo) = vbNo Then
            r = agdMapTable(.TableIndex).rows
          Else
            startTime = Timer
          End If
        End If
      Next r
      'RefreshLayer = saveRefreshLayer
      DelaySelectionChange = False
      RaiseEvent SelectionChange("all", CurLayer, state)
      Map1.Refresh
      MousePointer = ccDefault
    End If
  End With
End Sub

Public Sub SetSelectedByKey(ByVal Layer&, ByVal keyValue$, ByVal state As Boolean)
  Dim ssrow&, sscol&
  DbgMsg "SetSelectedByKey " & Layer & ", " & keyValue & " = " & state
  If Layer < 0 Then Layer = curLay
  sscol = LayerKeyCol(Layer)
  If sscol < 0 Then
    If IsNumeric(keyValue) Then ssrow = CLng(keyValue) Else ssrow = 0
  Else
    ssrow = agdMapTable(LayerInfo(Layer).TableIndex).RowContaining(keyValue, sscol)
  End If
  If ssrow > 0 Then
    ReallySetSelected Layer, ssrow, state
  Else
    'Annoying error message removed 30 Aug 2001
    'MsgBox "Item '" & keyValue & "' not found in column " & sscol & " (" & LayerInfo(Layer).KeyField & ").", vbOKOnly, "Map SetSelectedByKey"
  End If
End Sub

Private Sub ReallySetSelected(ByVal Layer&, ByVal ssrow&, ByVal state As Boolean)
  DbgMsg "ReallySetSelected: layer=" & Layer & ", row=" & ssrow & ", state=" & state
  With LayerInfo(Layer)
    If .Selected(ssrow) <> state Then
      Dim keyValue$, srch$, shp&, found As Boolean
      keyValue = Layer & "," & ssrow
      If state Then
        .nSelected = .nSelected + 1
        If maxSelectedShapes < .nSelected Then
          maxSelectedShapes = .nSelected * 2
          ReDim Preserve SelectedMapShapeId(maxSelectedShapes)
          ReDim Preserve SelectedMapShape(maxSelectedShapes)
          ReDim Preserve SelectedMapSymbol(maxSelectedShapes)
        End If
        SelectedMapShapeId(.nSelected) = keyValue
        srch = "FeatureId=" & ssrow 'LayerInfo(layer).KeyField & " = '" & keyValue & "'"
        Set SelectedMapShape(.nSelected) = Map1.Layers(Layer).Find(srch)
        Set SelectedMapSymbol(.nSelected) = New symbol
        With SelectedMapSymbol(.nSelected)
          .SymbolType = Map1.Layers(Layer).symbol.SymbolType
          .Color = Map1.Layers(Layer).symbol.Color
          .Style = Map1.Layers(Layer).symbol.Style
          .size = Map1.Layers(Layer).symbol.size
        End With
      Else
        shp = 1: found = False
        While shp <= .nSelected And Not found
          If SelectedMapShapeId(shp) = keyValue Then found = True Else shp = shp + 1
        Wend
        If found Then
          While shp < .nSelected
            SelectedMapShapeId(shp) = SelectedMapShapeId(shp + 1)
            Set SelectedMapShape(shp) = SelectedMapShape(shp + 1)
            Set SelectedMapSymbol(shp) = SelectedMapSymbol(shp + 1)
            shp = shp + 1
          Wend
          .nSelected = .nSelected - 1
        End If
      End If
      If sstLegend.Tab = .TableIndex And .TableIndex > 0 Then
        On Error GoTo ErrLabel
        lblNselected.Caption = .nSelected & " of " & agdMapTable(.TableIndex).rows
        lblNselected.Left = cmdSelLocations(0).Left - lblNselected.Width - 100
      End If
      DbgMsg ".Selected(ssrow) = state"
      .Selected(ssrow) = state
      DbgMsg "agdMapTable(LayerInfo(Layer).TableIndex).Selected(ssrow, 0) = state"
      agdMapTable(LayerInfo(Layer).TableIndex).Selected(ssrow, 0) = state
      
      'Stop highlighting this shape since its appearance just changed
      CurrentMapShapeName = ""
      Set CurrentMapShape = Nothing
      If Not DelaySelectionChange Then
        DbgMsg "RefreshLayer"
        If RefreshLayer Then
          Map1.RefreshLayer Layer
          RecentlyRefreshedLayer = True
        Else
          Map1.Refresh
        End If
      End If
    End If
  End With
  
  Exit Sub

ErrLabel:
  DbgMsg "Error setting label" & vbCr & Err.Description
  Resume Next
End Sub

'Called when a toolbar button or menu item is clicked
'cmd is the index of the command (index property of menu = tag of button)
Public Sub MapAction(ByVal cmd&)
  Static LastCommand As Long
  Dim j&, r As Rectangle, mouseptr%, b As Buttons
  DbgMsg "MapAction " & cmd
  Set b = tbrMap1.Buttons
  
  'Un-press a mode button if we were already in that mode
  If cmd = LastCommand Then
    Select Case cmd
      Case 0, 30, 60, 200, 210, 220: cmd = -1 ' Zoom,Pan,Identify,Add,Remove,"Move
    End Select
  End If

  'Set all buttons to unpressed state
  For j = 1 To b.Count
    b.item(j).Value = tbrUnpressed
  Next j
  
  mouseptr = vbDefault
  TrackNearestShape = False
  pDeletingPoint = False
  Highlight = ""
  MapMovingPoint = 0
  If RecentlyRefreshedLayer Then
    Map1.Refresh
    Map1.Refresh
    RecentlyRefreshedLayer = False
  End If
  Select Case cmd
  Case -1 'Nothing, just clear buttons and tracking state
  Case 0: DbgMsg "Map Rectangle Zoom Start"
    b("Zoom").Value = tbrPressed
    mouseptr = 99
  Case 10: DbgMsg "Map Zoom In"
    Set r = Map1.Extent
    r.ScaleRectangle (0.5)
    Map1.Extent = r
    UnsavedChanges = True
  Case 20: DbgMsg "Map Zoom Out"
    Set r = Map1.Extent
    r.ScaleRectangle (2#)
    Map1.Extent = r
    UnsavedChanges = True
  Case 30: DbgMsg "Map Pan Start"
    b("Pan").Value = tbrPressed
    mouseptr = 99
  Case 40: DbgMsg "Map Full Zoom"
    Set Map1.Extent = UserFullExtent
    UnsavedChanges = True
  Case 60: DbgMsg "Map Identify mode"
    If curLay < 0 Then
      MsgBox "No Feature Specified to Identify." & vbCr & "Choose a table below."
    'ElseIf Map1.Layers(curLay).symbol.SymbolType = 2 Then
    '  MsgBox "Identify mode is supported for point and line layers only, not areas."
    Else
      b("Identify").Value = tbrPressed
      mouseptr = vbArrowQuestion
      TrackNearestShape = True
      'Dim l&
      'l = 1
      'curLay = -1
      'While l < nLayers And curLay < 0
      '  If LayerInfo(l).TableIndex = sstLegend.Tab Then curLay = l Else l = l + 1
      'Wend
    End If
  Case 80 'Save .map file
    Call MapSave
  Case 90 'Read .map file
    Call MapGet
  Case 110: DbgMsg "Map Print"
    'molt always writes to default printer!
    Dim lDefB4 As String, lDef As String, lRet As Long, lLand As Boolean
  
    lDefB4 = String$(128, 0)
    lRet = GetProfileString("WINDOWS", "DEVICE", "", lDefB4, 127) 'default printer
  
    On Error GoTo printcancel:
    comMap.HelpContext = 511
    comMap.PrinterDefault = True
    comMap.CancelError = True
    comMap.ShowPrinter
    If comMap.Orientation = cdlLandscape Then
      lLand = True
    Else
      lLand = False
    End If
    Map1.PrintMap "Map", "", lLand
printcancel:
    On Error GoTo 0
    lDef = String$(128, 0)
    lRet = GetProfileString("WINDOWS", "DEVICE", "", lDef, 127)
    If lDef <> lDefB4 Then 'changed default printer
      lRet = WriteProfileString("WINDOWS", "DEVICE", lDefB4)
      lDef = String$(128, 0)
      lRet = GetProfileString("WINDOWS", "DEVICE", "", lDef, 127) 'this seems to force an update
      If lDef <> lDefB4 Then 'reset to orig failed
        MsgBox "Default printer is now:" & vbCr & lDef
      End If
    End If
  Case 200: DbgMsg "Map Add Point"
    If Map1.Layers(curLay).symbol.SymbolType = moPointSymbol Then
      b("Add").Value = tbrPressed
      mouseptr = vbCrosshair
    Else
      MsgBox "Adding shapes is only supported for point layers."
    End If
  Case 210: DbgMsg "Map Delete Point"
  '  b("Remove").Value = tbrPressed
    If Map1.Layers(curLay).symbol.SymbolType = moPointSymbol Then
      'If LayerInfo(curLay).TableIndex = sstLegend.Tab Then
        TrackNearestShape = True
        pDeletingPoint = True
        mouseptr = vbArrow
      'Else
      '  MsgBox "Table for current layer must be visible to delete points."
      'End If
    Else
      MsgBox "Deleting is only supported for point layers."
    End If
  Case 220: DbgMsg "Map Move Point Start"
    If Map1.Layers(curLay).symbol.SymbolType = moPointSymbol Then
      Dim result As VbMsgBoxResult
      result = MsgBox("Do you really want to permanently move points on the map?", vbYesNo, "Confirmation")
      If result = vbYes Then
        b("Move").Value = tbrPressed
        mouseptr = 99
        TrackNearestShape = True
      End If
    Else
      MsgBox "Moving shapes is only supported for point layers."
    End If
  Case 230 'Up
  Case 240 'Down
  Case 250 'Select All
    SelectAll curLay, True
  Case 260 'Select None
    SelectAll curLay, False
  Case 300: DbgMsg "Map Edit"
    If frmMapCov.Visible Then
      frmMapCov.ZOrder
    Else
      If LegendIsVisible Then
        'Selections can get messed up in frmMapCov as layers are rearranged. This should help
        For j = 0 To nLayers - 1
          SelectAll j, False
        Next j
      End If
      frmMapCov.Hide 'make sure form is loaded but not visible
      Set frmMapCov.m = Map1
      Set frmMapCov.am = Me
      On Error Resume Next
      frmMapCov.Icon = UserControl.Parent.Icon
      On Error GoTo 0
Debug.Print "ATML2k:Show frmMapCov"
      frmMapCov.Show 1
    End If
  Case 400 'Branch Select Mode
    If Me.StreamSelectMode = 1 Then
      Me.StreamSelectMode = 0
    Else
      Me.StreamSelectMode = 1
    End If
  Case 410 'Downstream Select Mode
    If Me.StreamSelectMode = 2 Then
      Me.StreamSelectMode = 0
    Else
      Me.StreamSelectMode = 2
    End If
  Case Else
    MsgBox "Unknown MapAction: " & cmd
  End Select
  fraMap1.MousePointer = mouseptr
  If mouseptr = 99 Then
    If cmd = 0 Then fraMap1.MouseIcon = imgZoom.Picture
    If cmd = 30 Then fraMap1.MouseIcon = imgPan.Picture
    If cmd = 220 Then fraMap1.MouseIcon = imgMovePoint.Picture
  End If
  tbrMap1.Refresh
  LastCommand = cmd
End Sub

Public Sub SetMapData(FilePath$, MapName$, AnimTab$)

  Dim x#, i&, a&, fu%, S$, RECT As Rectangle
  Dim istr$, rtyp$, Utyp$, MapScale#
  
  DbgMsg "SetMapData:" & FilePath & ":" & MapName
  AnimTab = ""
  MapScale = 1#
  Set RECT = Map1.Extent 'need to set it to any valid rectangle, not necessarily map1.Extent
  RECT.Bottom = 0
  RECT.Top = 0
  RECT.Left = 0
  RECT.Right = 0

  Clear 'clear any existing map info

  If Len(FilePath) > 0 Then
    If Right(FilePath, 1) <> "\" Then FilePath = FilePath & "\"
    DbgMsg "SetMapData:ChangePath:" & FilePath
    ChDriveDir FilePath
  End If
  On Error GoTo ReadErr
  fu = FreeFile(0)
  DbgMsg "SetMapData:OpenMapFile:" & MapName
  Open MapName For Input As #fu
  i = 0
  
  Do While Not EOF(fu)
    Line Input #fu, istr
    rtyp = UCase(StrRetRem(istr))
    DbgMsg "SetMapData:ReadRecord:" & rtyp & ":" & istr
    
    If rtyp = "EXT" Then
      RECT.Left = StrRetRem(istr)
      RECT.Top = StrRetRem(istr)
      RECT.Right = StrRetRem(istr)
      RECT.Bottom = StrRetRem(istr)
      DbgMsg "SetMapData:Extent:" & CStr(RECT.Left) & ":" & _
                                    CStr(RECT.Top) & ":" & _
                                    CStr(RECT.Right) & ":" & _
                                    CStr(RECT.Bottom)
    ElseIf rtyp = "DBF" Then 'line contains info about a column of DBF to put in table for the last LYR
      With LayerInfo(0)
        Dim fldnam$, fld&
        fldnam = StrRetRem(istr)
        fld = .nFields
        .nFields = .nFields + 1
        DbgMsg "SetMapData:DatabaseField:" & fld & ":" & fldnam
        ReDim Preserve .Fields(.nFields)
        .Fields(fld).Column = fld
        .Fields(fld).Name = fldnam
        If Len(istr) > 0 Then
          .Fields(fld).Caption = StrRetRem(istr)
          'If this is an old-style map file, skip the field number
          If IsNumeric(.Fields(fld).Caption) Then
            If Len(istr) > 1 Then
              .Fields(fld).Caption = StrRetRem(istr)
            Else
              .Fields(fld).Caption = fldnam
            End If
          End If
        Else
          .Fields(fld).Caption = fldnam
        End If
        .TableIndex = 99 'any value > 0 for now, will be set properly in RepopulateGrids
      End With
    ElseIf rtyp = "LYR" Then
      Dim Filename$, Caption$, sym As New symbol, dbKey$, dbname$
      Dim dbBranchFld$, dbDownIDFld$, dbLengthFld$
      Dim Visibl As Boolean, Animate As Boolean
      Filename = SwitchSlash(StrRetRem(istr))
      Caption = FilenameOnly(Filename)
      sym.Color = TextOrNumericColor(StrRetRem(istr))
      DbgMsg "SetMapData:Layer:" & Filename & ":" & sym.Color
      Visibl = True
      Animate = False
      dbKey = "": dbname = ""
      dbBranchFld = "BRANCH"
      dbDownIDFld = "DOWNID"
      dbLengthFld = "LENGTH"
      While Len(istr) > 0
        rtyp = StrRetRem(istr)
        Select Case UCase(Left(rtyp, 3))
        Case "EXT": If Len(istr) > 0 Then MapScale = StrRetRem(istr)
        Case "KEY": dbKey = StrRetRem(istr)
        Case "LAB": dbname = StrRetRem(istr)
        Case "NAM": Caption = StrRetRem(istr)
        Case "BRN": dbBranchFld = StrRetRem(istr)
        Case "DWN": dbDownIDFld = StrRetRem(istr)
        Case "LEN": dbLengthFld = StrRetRem(istr)
        Case "ANI"
          If Len(AnimTab) > 0 Then AnimTab = AnimTab & ","
          AnimTab = AnimTab & Filename
          Animate = True
        Case "TRA": 'transparent fill
          sym.Style = moTransparentFill
          sym.OutlineColor = sym.Color
        Case "HID": Visibl = False  'hide this layer
        Case "SYM", "STY": sym.Style = TextOrNumericStyle(StrRetRem(istr)) 'symbol(marker,linetype,fill)
        Case "OUT": sym.OutlineColor = TextOrNumericColor(StrRetRem(istr))
        Case "ROT": sym.Rotation = StrRetRem(istr) 'degrees counterclockwise
          
'        case "SIZ":  'symbol size in points
'          Dim siz&
'          siz = StrRetRem(istr)
'          sym.Size = siz
        End Select
      Wend
      DbgMsg "SetMapData:AddLayer:" & Caption & ":" & Visibl

      NewAddShapeLayer "", Filename, Caption, sym, Visibl, Animate, dbKey, _
                    dbname, dbBranchFld, dbDownIDFld, dbLengthFld
      Set sym = Nothing
    ElseIf rtyp = "LAB" Then
      With Map1.Layers(0)
        .LabelField = StrRetRem(istr)
        .LabelFont.Bold = False
        While Len(istr) > 0
          rtyp = StrRetRem(istr)
          Select Case Left(UCase(rtyp), 3)
            Case "FON": .LabelFont.Name = StrRetRem(istr)
            Case "SIZ": .LabelFont.size = StrRetRem(istr)
            Case "COL": .LabelColor = TextOrNumericColor(StrRetRem(istr))
            Case "BOL": .LabelFont.Bold = True
            Case "ITA": .LabelFont.Italic = True
            Case "UND": .LabelFont.Underline = True
            Case "STR": .LabelFont.Strikethrough = True
            Case "HOR": .LabelHorzAlignment = TextOrNumericAlignment(StrRetRem(istr))
            Case "VER": .LabelVertAlignment = TextOrNumericAlignment(StrRetRem(istr))
          End Select
        Wend
      End With
    ElseIf rtyp = "REN" Then
      Dim rendtyp$, rendfield$, rendcnt$, j&
'      MapRendField = StrRetRem(istr)
'      MapRendType = StrRetRem(istr)
      rendtyp = StrRetRem(istr)
      rendfield = StrRetRem(istr)
      rendcnt = StrRetRem(istr)
      If UCase(Left(rendtyp, 3)) = "VAL" Then
        LayerInfo(0).RendType = ATCoValueRend
        Dim lvRend As New ValueMapRenderer
        lvRend.field = rendfield
        lvRend.ValueCount = rendcnt
        lvRend.SymbolType = Map1.Layers(0).symbol.SymbolType
        For j = 1 To rendcnt
          Line Input #fu, istr
          lvRend.Value(j - 1) = StrRetRem(istr)
          lvRend.symbol(j - 1).Color = TextOrNumericColor(StrRetRem(istr))
          While Len(istr) > 0
            Select Case UCase(Left(StrRetRem(istr), 3))
              Case "STY": lvRend.symbol(j - 1).Style = TextOrNumericStyle(StrRetRem(istr))
              Case "OUT": lvRend.symbol(j - 1).OutlineColor = TextOrNumericColor(StrRetRem(istr))
              Case Else:  Debug.Print "Ignoring token before " & istr & " in map file"
            End Select
          Wend
        Next j
        Set Map1.Layers(0).Renderer = lvRend
        Set lvRend = Nothing
      ElseIf UCase(Left(rendtyp, 3)) = "CLA" Then
        LayerInfo(0).RendType = ATCoClassBreakRend
        Dim lcRend As New ClassBreaksRenderer
        lcRend.field = rendfield
        lcRend.BreakCount = rendcnt
        lcRend.SymbolType = Map1.Layers(0).symbol.SymbolType
        For j = 1 To rendcnt
          Line Input #fu, istr
          lcRend.Break(j - 1) = StrRetRem(istr)
          lcRend.symbol(j - 1).Color = TextOrNumericColor(StrRetRem(istr))
          While Len(istr) > 0
            Select Case UCase(Left(StrRetRem(istr), 3))
              Case "STY": lcRend.symbol(j - 1).Style = TextOrNumericStyle(StrRetRem(istr))
              Case "OUT": lcRend.symbol(j - 1).OutlineColor = TextOrNumericColor(StrRetRem(istr))
              Case Else:  Debug.Print "Ignoring token before " & istr & " in map file"
            End Select
          Wend
        Next j
        Set Map1.Layers(0).Renderer = lcRend
        Set lcRend = Nothing
      End If
    End If
  Loop
  DbgMsg "SetMapData:EndofMapFile"
  
  pMapFilePath = FilePath
  pMapFileName = MapName

  Close #fu
'  MapPtLay = Map1.Layers.Count - MapPtLay
'  MapAnLay = Map1.Layers.Count - 1 - MapAnLay
  
'  If MapPtLay >= 0 Then
'    Call DrawMapPts(M)
'  Else
    cmdSelLocations(0).Enabled = False
    cmdSelLocations(1).Enabled = False
'  End If

  If RECT.Bottom = RECT.Top Then
    Set RECT = Map1.Extent
    RECT.ScaleRectangle (MapScale)
    DbgMsg "SetMapData:UsingOldMapExtent"
  End If
  Map1.Extent = RECT
  RECT.ScaleRectangle (1#)
  Set UserFullExtent = RECT
  DbgMsg "SetMapData:About2SetLegend"
  SetLegend
  DbgMsg "SetMapData:About2RepopulateGrids"
  RepopulateGrids
  Enabled = True
  UnsavedChanges = False
  Exit Sub 'completed ok
ReadErr:
    MsgBox "A problem occurred reading the map file " & MapName & vbCrLf & _
           Err.Description, 48, "ATCoMap Problem"
    DbgMsg "SetMapData:Error:" & Err.Number & ":" & Err.Description & ":" & istr

End Sub

Private Function SwitchSlash(S$) As String
  Dim i&, t$, c$
  
  t = ""
  For i = 0 To Len(S) - 1
    c = Mid(S, i + 1, 1)
    If c = "/" Then
      t = t & "\"
    Else
      t = t & c
    End If
  Next i
  SwitchSlash = t
End Function

Public Sub MapSave() '(Index%) ' 0 = save, 1 = get
  Static LastSaveFilename As String
  Static LastSaveFilterIndex As Long
  Dim FilterChosen As String
  Dim MapFile As String
  Dim FileType As String
  Dim dotpos As Long
  
  If Len(LastSaveFilename) = 0 Then LastSaveFilename = MapFilePath & FilenameOnly(MapFileName)
  If Len(LastSaveFilename) = 0 Then LastSaveFilename = "untitled"
  If LastSaveFilterIndex < 1 Then LastSaveFilterIndex = 1
  On Error GoTo ErrHand
  
  If (pIPC Is Nothing) Then
    comMap.HelpContext = 509
    comMap.CancelError = True
    comMap.Filter = "Map files (*.map)|*.map|Bitmap files (*.bmp)|*.bmp|Windows Metafiles (*.wmf)|*.wmf|Clipboard bitmap|*.clb|Clipboard Metafile|*.clm"
    comMap.FilterIndex = LastSaveFilterIndex
    comMap.Filename = LastSaveFilename
    comMap.DialogTitle = "Save Map As"
    comMap.flags = cdlOFNHideReadOnly Or cdlOFNNoReadOnlyReturn Or cdlOFNCreatePrompt '&H3804& 'create & not read only
    comMap.ShowSave
    MapFile = comMap.Filename
    DbgMsg "MapSave noIPC " & MapFile
    Select Case comMap.FilterIndex
      Case 4: Map1.ExportMap moExportClipboardBMP, True, 1
      Case 5: Map1.ExportMap moExportClipboardEMF, True, 1
      Case Else:
        Select Case UCase(Right(MapFile, 3))
          Case "MAP": SaveMapFile MapFile
          Case "BMP": Map1.ExportMap moExportBMP, MapFile, 1
          Case "WMF": Map1.ExportMap moExportEMF, MapFile, 1
          Case Else: GoTo ErrHand
        End Select
    End Select
    LastSaveFilterIndex = comMap.FilterIndex
  Else
    MapFile = pIPC.SavePictureDialog(FilenameNoExt(LastSaveFilename), "=Save Map As", "+Clipboard Metafile|*.clm", "+Windows Metafile (*.wmf)|*.wmf", "+Map files (*.map)|*.map")
    DbgMsg "MapSave " & MapFile

    FileType = FileExt(MapFile)
    If FileType = "" Then FileType = MapFile
    Select Case UCase(FileType)
      Case "CLB": Map1.ExportMap moExportClipboardBMP, True, 1
      Case "CLM": Map1.ExportMap moExportClipboardEMF, True, 1
      Case "MAP": SaveMapFile MapFile
      Case "BMP": Map1.ExportMap moExportBMP, MapFile, 1
      Case "WMF": Map1.ExportMap moExportEMF, MapFile, 1
      Case Else:  Map1.ExportMap moExportBMP, MapFile & ".bmp", 1
                  pIPC.SavePictureAs Nothing, MapFile
    End Select
  End If
  
  LastSaveFilename = MapFile
  Exit Sub

ErrHand:
  MsgBox "File not saved " & MapFile, vbOKOnly, "Map Save"
End Sub

Public Sub SaveMapFile(MapFile$)
  Dim b#, l#, t#, r#, fu%, i&, j&, S$
  Dim tSym As symbol, RECT As Rectangle
  Dim mp$
  DbgMsg "SaveMapFile " & MapFile
  On Error GoTo ErrHand

  fu = FreeFile(0)
  Open MapFile For Output As #fu
  mp = PathNameOnly(MapFile)
  If Len(mp) < 1 Then mp = CurDir
  Set RECT = Map1.Extent
  Print #fu, "EXT " & RECT.Left & " " & RECT.Top & " " & RECT.Right & " " & RECT.Bottom
  For i = nLayers - 1 To 0 Step -1 'Map1.Layers.Count - 1 To 0 Step -1
    With Map1.Layers(i)
      Print #fu, "LYR '" & RelativeFilename(.File, mp) & "'," & colorName(.symbol.Color) & _
                ",Style " & StyleName(.symbol.SymbolType, .symbol.Style);
      If .symbol.SymbolType <> moLineSymbol Then
        Print #fu, ",Outline " & colorName(.symbol.OutlineColor);
      End If
      If .symbol.SymbolType = moPointSymbol Then
        Print #fu, ",Size " & .symbol.size;
      End If
      If Not .Visible Then
        Print #fu, ",Hide";
      End If
      If LayerInfo(i).Animate Then Print #fu, ",Animate";
      If LayerInfo(i).BranchField <> "BRANCH" Then Print #fu, ",BRN '" & LayerInfo(i).BranchField & "'";
      If LayerInfo(i).DownIDField <> "DOWNID" Then Print #fu, ",DWN '" & LayerInfo(i).DownIDField & "'";
      If LayerInfo(i).LengthField <> "LENGTH" Then Print #fu, ",LEN '" & LayerInfo(i).LengthField & "'";
      If .Name <> FilenameOnly(.File) Then
        Print #fu, ",Name '" & .Name & "'";
      End If
      If Len(LayerInfo(i).keyField) > 0 Then Print #fu, ",Keys '" & LayerInfo(i).keyField & "'";
      If Len(LayerInfo(i).LabelField) > 0 Then
        If LayerInfo(i).LabelField <> LayerInfo(i).keyField Then
          Print #fu, ",Labels '" & LayerInfo(i).LabelField & "'";
        End If
      End If
      Print #fu,
      
      If Len(.LabelField) > 0 Then
        Print #fu, "LAB '" & .LabelField & "'";
        Print #fu, ",Font '" & .LabelFont.Name & "'";
        Print #fu, ",Size " & .LabelFont.size;
        Print #fu, ",Color " & colorName(.LabelColor);
        If .LabelFont.Bold Then Print #fu, ",Bold";
        If .LabelFont.Italic Then Print #fu, ",Italic";
        If .LabelFont.Strikethrough Then Print #fu, ",Strikethrough";
        If .LabelFont.Underline Then Print #fu, ",Underline";

        Print #fu, ",HorzAlign " & Choose(.LabelHorzAlignment, "Top", "Bottom", "Left", "Right", "Center", "Baseline");
        Print #fu, ",VertAlign " & Choose(.LabelVertAlignment, "Top", "Bottom", "Left", "Right", "Center", "Baseline");
        Print #fu,
      End If
      
      If LayerInfo(i).RendType <> ATCoNoRend Then
        If LayerInfo(i).RendType = ATCoValueRend Then
          Print #fu, "REN Value " & .Renderer.field & " " & .Renderer.ValueCount
          For j = 0 To .Renderer.ValueCount - 1
            Set tSym = .Renderer.symbol(j)
            Print #fu, "   '" & .Renderer.Value(j) & "'," & colorName(tSym.Color) & _
                ",Style " & StyleName(tSym.SymbolType, tSym.Style);
            If tSym.SymbolType <> moLineSymbol Then
              Print #fu, ",Outline " & colorName(tSym.OutlineColor);
            End If
            Print #fu,
          Next j
        ElseIf LayerInfo(i).RendType = ATCoClassBreakRend Then
          Print #fu, "REN Class " & .Renderer.field & " " & .Renderer.BreakCount
          For j = 0 To .Renderer.BreakCount - 1
            Set tSym = .Renderer.symbol(j)
            Print #fu, "   " & .Renderer.Break(j) & "," & colorName(tSym.Color) & _
                ",STY " & StyleName(tSym.SymbolType, tSym.Style);
            If tSym.SymbolType <> moLineSymbol Then
              Print #fu, ",OUT " & colorName(tSym.OutlineColor);
            End If
            Print #fu,
          Next j
        End If
      End If
      
    End With
    For j = 0 To LayerInfo(i).nFields - 1
      With LayerInfo(i).Fields(j)
        Print #fu, "DBF '" & .Name & "'";
        If .Caption <> .Name Then Print #fu, ",'" & .Caption & "'" Else Print #fu,
      End With
    Next j
  Next i
  Close #fu
  UnsavedChanges = False
  Exit Sub

ErrHand:
  MsgBox "Error writing .map file" & vbCr & Err.Description, vbExclamation, "Save Map File"
End Sub

Public Sub MapGet()
  Dim MapFile$, S$
  DbgMsg "MapGet"
  On Error GoTo errhandmap
  comMap.HelpContext = 510
  comMap.CancelError = True
  comMap.Filter = "Map files (*.map)|*.map"
  comMap.Filename = "*.map"
  comMap.DialogTitle = "Map Get"
  comMap.flags = &H1000& 'file must exist
  comMap.ShowOpen
  MapFile = comMap.Filename
  
  If Len(MapFile) > 0 Then
    S = ""
    SetMapData "", MapFile, S
  End If
  
  Exit Sub

errhandmap:
  If Err.Description <> "Cancel was selected." Then
    MsgBox "Error reading map file '" & MapFile & "'." & vbCrLf & vbCrLf & _
            Err.Description, vbExclamation, "Map Get"
  End If
End Sub

Private Function RequireKey(lay&) As Boolean
  If Len(LayerInfo(lay).keyField) > 0 Then
    RequireKey = True
  Else
    Dim msgtxt$
    msgtxt = "Layer '" & LayerInfo(curLay).Name & "' does not have a key field specified."
    msgtxt = msgtxt & vbCrLf & "A key must be specified before this operation can be completed."
    msgtxt = msgtxt & vbCrLf & "For example, Key 'IDLOCN' could appear on the LYR line in the .map file."
    MsgBox msgtxt, vbOKOnly, "Map"
    RequireKey = False
  End If
End Function

Public Function AddPointToShapeFile(ByVal baseFilename$, ByVal dbfKeyField$, dbfKeyVal$, ByVal x#, ByVal y#) As Boolean
  Dim f$, shpfile%, shxfile%, RecordNumber& ', c&
  Dim FileLength&, ShapeType&, lowX#, lowY#, uppX#, uppY#
  Dim MapRS As Recordset
  DbgMsg "AddPointToShapeFile " & baseFilename & ", " & dbfKeyField & " = " & dbfKeyVal & " [" & x & ", " & y & "]"
  On Error GoTo exitfunction
  'Add to .dbf file
  f = baseFilename & ".dbf"
  Set MapRS = OpenDBF(f, False)
  
  'If IsNull(MapRS) Then Exit Function 'This should work, but it doesn't

  MapRS.AddNew
  MapRS(dbfKeyField) = dbfKeyVal
  MapRS.Update
  MapRS.Close
  
  'Shape Main file (.shp)
  f = baseFilename & ".shp"
  shpfile = FreeFile(0)
  Open f For Binary Access Read Write As shpfile
  Call ReadShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  FileLength = FileLength + 14
  If x > uppX Then uppX = x Else If x < lowX Then lowX = x
  If y > uppY Then uppY = y Else If y < lowY Then lowY = y
  Call WriteShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY, 0, 0, 0, 0)
  RecordNumber = (FileLength - 50) / 14
  Seek #shpfile, 101 + 28 * (RecordNumber - 1)
  Call WriteShapePointAll(shpfile, RecordNumber, x, y)
  Close shpfile
  
  'Shape Index file (.shx)
  f = baseFilename & ".shx"
  shxfile = FreeFile(0)
  Open f For Binary Access Read Write As shxfile
  'Call ReadShapeHeader(outfile2, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  FileLength = 50 + RecordNumber * 4
  Call WriteShapeHeader(shxfile, FileLength, ShapeType, lowX, lowY, uppX, uppY, 0, 0, 0, 0)
  Seek #shxfile, 101 + 8 * (RecordNumber - 1)
  Call WriteShapePointIndex(shxfile, RecordNumber)
  Close shxfile
  AddPointToShapeFile = True
  RaiseEvent AddedShape(moPointSymbol, dbfKeyVal, x, y)
  Exit Function
exitfunction:
  MsgBox "Error adding point to shape file" & vbCr & Err.Description
End Function

Public Function AddPointToShapeFileOnMap(dbfKeyVal$, ByVal x#, ByVal y#) As Boolean
  Dim f$
  AddPointToShapeFileOnMap = False
  DbgMsg "AddPointToShapeFileOnMap " & dbfKeyVal & " [" & x & ", " & y & "]"
  
  If RequireKey(curLay) Then
    Dim MSI As MapSaveInfo
    RemoveMapLayerTemporarily curLay, MSI
    
    f = LayerInfo(curLay).path & LayerInfo(curLay).baseFilename
    AddPointToShapeFileOnMap = AddPointToShapeFile(f, LayerInfo(curLay).keyField, dbfKeyVal, x, y)

    PutLayerBackOnMap curLay, MSI
    PopulateGrid agdMapTable(LayerInfo(curLay).TableIndex), LayerInfo(curLay)
  End If
  MapAction -1
End Function

'Edits a field of a record in the current map layer. Set CurLayer first.
Public Sub EditShapeDBFOnMap(record&, dbfField$, newValue As Variant)
  Dim f$
  Dim MSI As MapSaveInfo, MapRS As Recordset
  DbgMsg "EditShapeDBFOnMap " & record & ", " & dbfField & " = " & newValue
  RemoveMapLayerTemporarily curLay, MSI
  On Error GoTo exitfunction
  f = LayerInfo(curLay).path & LayerInfo(curLay).baseFilename & ".dbf"
  Set MapRS = OpenDBF(f, False)
  On Error GoTo closedbf
  If Not IsNull(MapRS) Then
    MapRS.MoveFirst
    MapRS.Move record
    MapRS.Edit
    MapRS(dbfField) = newValue
    MapRS.Update
closedbf:
    On Error GoTo exitfunction
    MapRS.Close
  End If
exitfunction:
  On Error Resume Next
  PutLayerBackOnMap curLay, MSI
End Sub

'Removes a point in the current map layer. Set CurLayer first.
Public Function DeletePointOnMap(record&) As Boolean
  DeletePointOnMap = False
  DbgMsg "DeletePointOnMap " & record
  On Error GoTo exitfunction

  If RequireKey(curLay) Then
    Dim f$, shfile%
    Dim FileLength&, FilePos&, ShapeType&, lowX#, lowY#, uppX#, uppY#
    Dim MSI As MapSaveInfo, MapRS As Recordset
    Dim FileBytes() As Byte, FileBytesAfter() As Byte, curByte As Long, nBytes As Long
    RemoveMapLayerTemporarily curLay, MSI
    f = LayerInfo(curLay).path & LayerInfo(curLay).baseFilename & ".dbf"
    Set MapRS = OpenDBF(f, False)
    On Error GoTo closedbf
    If Not IsNull(MapRS) Then
      MapRS.MoveFirst
      MapRS.Move record
      MapRS.Delete
      MapRS.Update
closedbf:
      On Error GoTo exitfunction
      MapRS.Close
      
      'Shape Index file (.shx)
      f = LayerInfo(curLay).path & LayerInfo(curLay).baseFilename & ".shx"
      shfile = FreeFile(0)
      Open f For Binary Access Read Write As shfile
      Call ReadShapeHeader(shfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
      Seek #shfile, 1
      FileBytes = InputB(LOF(shfile) - 8, shfile)
      Close shfile
      
      'don't need to shift contents of shx around since entries are all the same
      
      If Len(Dir(LayerInfo(curLay).path & "befordel.shx")) > 0 Then Kill LayerInfo(curLay).path & "befordel.shx"
      Name f As LayerInfo(curLay).path & "befordel.shx"
      
      shfile = FreeFile(0)
      Open f For Binary Access Read Write As shfile
      Put #shfile, , FileBytes
      Seek #shfile, 1
      Call WriteShapeHeader(shfile, FileLength - 4, ShapeType, lowX, lowY, uppX, uppY, 0, 0, 0, 0)
      Close shfile

      'Shape Main file (.shp)
      f = LayerInfo(curLay).path & LayerInfo(curLay).baseFilename & ".shp"
      shfile = FreeFile(0)
      Open f For Binary Access Read Write As shfile
      nBytes = LOF(shfile)
      Call ReadShapeHeader(shfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
      FilePos = 101 + 28 * record
      Seek #shfile, 1
      FileBytes = InputB(nBytes, shfile)
      Close shfile
      
      For curByte = LBound(FileBytes) + 100 + 28 * record To LBound(FileBytes) + nBytes - 1
        FileBytes(curByte - 28) = FileBytes(curByte)
      Next
      ReDim Preserve FileBytes(nBytes - 28)
      
      If Len(Dir(LayerInfo(curLay).path & "befordel.shp")) > 0 Then Kill LayerInfo(curLay).path & "befordel.shp"
      Name f As LayerInfo(curLay).path & "befordel.shp"
      
      Open f For Binary Access Read Write As shfile
      Put #shfile, , FileBytes
      Seek #shfile, 1
      Call WriteShapeHeader(shfile, FileLength - 14, ShapeType, lowX, lowY, uppX, uppY, 0, 0, 0, 0)
      Close shfile
      
      DeletePointOnMap = True
    End If
  End If
exitfunction:
  On Error Resume Next
  PutLayerBackOnMap curLay, MSI
  PopulateGrid agdMapTable(LayerInfo(curLay).TableIndex), LayerInfo(curLay)
  MapAction -1
End Function


Public Sub RewritePointInShapeFile(ByVal RecNum&, ByVal pt As Point)
  Call RewriteXYPointInShapeFile(RecNum, pt.x, pt.y)
End Sub

Public Sub RewriteXYPointInShapeFile(ByVal RecNum&, ByVal x#, ByVal y#)
  Dim OutFile%, Offset&, MSI As MapSaveInfo
  DbgMsg "RewriteXYPointInShapeFile: " & RecNum & ": [" & x & ", " & y & "]"
  
  RemoveMapLayerTemporarily curLay, MSI

  OutFile = FreeFile(0)
  Open LayerInfo(curLay).path & LayerInfo(curLay).baseFilename & ".shp" For Binary Access Read Write As OutFile
  Offset = 113 + 28 * (RecNum - 1)
  Put #OutFile, Offset, x
  Put #OutFile, , y
  Close OutFile

  PutLayerBackOnMap curLay, MSI
End Sub

Public Sub RepopulateGrids()
  Dim lnum&, lname$, mlnum&, tnum&, found As Boolean, gridTop&, gridLeft&
  Dim sstVisible As Boolean, sstEnabled As Boolean
  DbgMsg "RepopulateGrids"
  sstVisible = sstLegend.Visible
  sstEnabled = sstLegend.Enabled
  sstLegend.Visible = True
  sstLegend.Enabled = True
  If sstLegend.Tabs > 1 Then
    sstLegend.Tab = 1
    gridTop = agdMapTable(1).Top
    gridLeft = agdMapTable(1).Left
  Else
    gridTop = 360
    gridLeft = 120
  End If
  tnum = 1
  For lnum = 0 To Map1.Layers.Count - 1 'To 0 Step -1
    If Map1.Layers(lnum).Visible Then
      lname = Map1.Layers(lnum).Name
      mlnum = 0
      found = False
      While mlnum < nLayers And Not found
        If LayerInfo(mlnum).Name = lname Then found = True Else mlnum = mlnum + 1
      Wend
      If found Then
        If LayerInfo(mlnum).TableIndex > 0 Then
          Dim agd As ATCoGrid
          If agdMapTable.Count < tnum Then
            If sstLegend.Tabs <= tnum Then
              sstLegend.TabsPerRow = tnum + 1
              sstLegend.Tabs = tnum + 1
            End If
            sstLegend.TabEnabled(tnum) = True
            sstLegend.Tab = tnum
            Load agdMapTable(tnum)
            'Set agdMapTable(tnum).Container = sstLegend ' .Tabs(tnum)
            'Call SetParent(agdMapTable(tnum).hwnd, sstLegend.hwnd)
            'sstLegend.Tab( s(tnum).Control(0) = "agdMapTable(3)"
            agdMapTable(tnum).Visible = True
          Else
            sstLegend.TabEnabled(tnum) = True
            sstLegend.Tab = tnum
          End If
          agdMapTable(tnum).Top = gridTop
          agdMapTable(tnum).Left = gridLeft
          sstLegend.TabCaption(tnum) = LayerInfo(mlnum).Name
          Set agd = agdMapTable(tnum)
          PopulateGrid agd, LayerInfo(mlnum)
          LayerInfo(mlnum).TableIndex = tnum
          tnum = tnum + 1
        End If
      End If
    End If
  Next lnum
  sstLegend.Tab = 0
  If tnum < 2 Then tnum = 2
  If tnum < sstLegend.Tabs Then
    For mlnum = tnum + 1 To agdMapTable.Count
      agdMapTable(mlnum - 1).Clear
    '  Unload agdMapTable(mlnum - 1)
    Next mlnum
    For mlnum = tnum + 1 To sstLegend.Tabs
      sstLegend.TabCaption(mlnum - 1) = ""
      sstLegend.TabEnabled(mlnum - 1) = False
    Next mlnum
    'sstLegend.Tabs = tnum
    'sstLegend.TabsPerRow = tnum
  End If
  sstLegend.Enabled = sstEnabled
  sstLegend.Visible = sstVisible
  RaiseEvent ChangedLayers
End Sub

Private Sub RemoveMapLayerTemporarily(layerindex&, MSI As MapSaveInfo)
  DbgMsg "RemoveMapLayerTemporarily"
  With Map1.Layers(layerindex)
    Set MSI.sym = .symbol
    Select Case LayerInfo(layerindex).RendType
    Case ATCoValueRend
      Set MSI.vRend = .Renderer
    Case ATCoClassBreakRend
      Set MSI.cRend = .Renderer
    End Select
  End With
  Map1.Layers.Remove layerindex
End Sub

Private Sub PutLayerBackOnMap(layerindex&, MSI As MapSaveInfo)
  Dim MapLay As MapLayer
  DbgMsg "PutLayerBackOnMap"
  Set MapLay = New MapLayer
  MapLay.File = LayerInfo(layerindex).path & LayerInfo(layerindex).baseFilename & ".shp"
  MapLay.Name = LayerInfo(layerindex).Name
  MapLay.symbol.SymbolType = MSI.sym.SymbolType 'This never works, SymbolType seems to be read-only
  MapLay.symbol.Style = MSI.sym.Style
  MapLay.symbol.Color = MSI.sym.Color
  MapLay.symbol.size = MSI.sym.size
  If MSI.sym.SymbolType <> moLineSymbol Then MapLay.symbol.OutlineColor = MSI.sym.OutlineColor
  If MSI.sym.SymbolType = moPointSymbol Then
    MapLay.symbol.Font = MSI.sym.Font
    'MapLay.symbol.Rotation = MSI.sym.Rotation 'This sometimes caused errors after adding a point
  End If
  Select Case LayerInfo(layerindex).RendType
    Case ATCoValueRend:       Set MapLay.Renderer = MSI.vRend
    Case ATCoClassBreakRend:  Set MapLay.Renderer = MSI.cRend
  End Select
  
  Map1.Layers.Add MapLay
  Map1.Layers.MoveTo 0, layerindex
  Set MapLay = Nothing 'dont leave a legacy in memory
End Sub

Private Sub UpdateShapeDBF(ByVal layerindex&, ByVal shapeIndex&, FieldName$, newValue As Variant)
  Dim i&, c&, f$, fieldSize As Long, oldvalue As String
  Dim MSI As MapSaveInfo, MapRS As Recordset
  DbgMsg "UpdateShapeDBF " & FieldName & " = " & newValue
  On Error GoTo updatePointError
    
  RemoveMapLayerTemporarily layerindex, MSI
  
  f = LayerInfo(layerindex).path & LayerInfo(layerindex).baseFilename & ".dbf"

  Set MapRS = OpenDBF(f, False)
  If IsNull(MapRS) Then
    PutLayerBackOnMap layerindex, MSI
    Exit Sub
  End If
  If Not MapRS.Updatable Then
    PutLayerBackOnMap layerindex, MSI
    Exit Sub
  End If
  MapRS.MoveFirst
  MapRS.Move shapeIndex - 1
  If Not MapRS.EOF Then
    If MapRS(FieldName).Type = 10 Then
      fieldSize = MapRS(FieldName).size
      If fieldSize < Len(newValue) Then
        oldvalue = newValue
        newValue = Left(newValue, fieldSize)
        MsgBox "Value '" & oldvalue & "' was too long for the field." & vbCr _
             & "Maximum width is " & fieldSize & "." & vbCr _
             & "Shortened to '" & newValue & "'", vbOKOnly, "Value too long"
      End If
    End If
    MapRS.Edit
    MapRS(FieldName) = newValue
    MapRS.Update
  Else
    Err.Description = "Shape index " & shapeIndex & " not in file"
    GoTo updatePointError
  End If
Closeup:
  On Error Resume Next
  MapRS.Close 'close table before putting layer back on map
  PutLayerBackOnMap layerindex, MSI
  Exit Sub
  
updatePointError:
  MsgBox "Error updating '" & FieldName & "' to value '" & newValue & "' in file " & f & vbCr & Err.Description
  GoTo Closeup
End Sub

Private Function IdPoint&(x!, y!)
  Dim tolr&, keyIndex&, keyval As Variant, found As Boolean
  DbgMsg "IdPoint at [" & x & ", " & y & "]"
  If curLay >= 0 Then
   'If Map1.Layers(curLay).symbol.SymbolType < 2 Then
    With Map1.Layers(curLay)
      tolr = 10
      Set CurShapeFldVals = .Identify(Map1.ToMapPoint(x, y), Map1.ToMapDistance(tolr))
      While CurShapeFldVals.Count = 0 And tolr < 100000000
        tolr = tolr * 1.5
        Set CurShapeFldVals = .Identify(Map1.ToMapPoint(x, y), Map1.ToMapDistance(tolr))
      Wend
      DbgMsg "IdPoint: tolr=" & tolr
      If CurShapeFldVals.Count = 0 Then
        IdPoint = 0
      Else
        IdPoint = CLng(CurShapeFldVals(0))
      End If
    End With
   End If
  'End If
End Function

Private Sub agdMapTable_Click(Index As Integer)
'  With agdMapTable(Index)
'    DbgMsg "agdMapTable(" & Index & "):Click: (" & .row & ", " & .col & ")", 3, "ATCoMap", "m"
'
'    If .ColSelectable(.col) Then
'      Dim state As Boolean
'
'      state = .Selected(.row, .col)
'
'      'Selection change may not be confirmed. Grid already reflects change, so undo it for now
'      '.Selected(.Row, .col) = False
'
'      ShapeSelected(curLay, .row) = state
'      .Selected(.row, .col) = ShapeSelected(curLay, .row)
'    End If
'  End With
End Sub

Private Sub agdMapTable_CommitChange(Index As Integer, ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim r&, c&
  DbgMsg "agdMapTable[" & Index & "]:CommitChange: [" & ChangeFromRow & ", " & ChangeFromCol & "] = " & agdMapTable(Index).TextMatrix(ChangeFromRow, ChangeFromCol)
  For r = ChangeFromRow To ChangeToRow
    For c = ChangeFromCol To ChangeToCol
      UpdateShapeDBF CurLayer, r, LayerInfo(CurLayer).Fields(c).Name, agdMapTable(Index).TextMatrix(r, c)
    Next c
  Next r
End Sub

Private Sub agdMapTable_SelChange(Index As Integer, row As Long, col As Long)
  With agdMapTable(Index)
    DbgMsg "agdMapTable[" & Index & "]:SelChange: [" & row & ", " & col & "]"
    
    If col = .cols - 1 Then 'only react when last column is selected so they can all be unselected if necessary
      Dim state As Boolean, newState As Boolean
      state = .Selected(.row, .col)
      ShapeSelected(curLay, .row) = state
      newState = ShapeSelected(curLay, .row)
      If newState <> state Then .Selected(.row, .col) = newState
    End If
  End With
End Sub

Private Sub agdMapTable_TextChange(Index As Integer, ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DbgMsg "agdMapTable(" & Index & "):TextChange = " & agdMapTable(Index).Text
End Sub

'Call with full path and extension of .dbf file
'Closes previously opened DBF, if any, without saving updates
Public Function OpenShapeDBF(f$, ReadOnly As Boolean) As Recordset
  DbgMsg "OpenShapeDBF " & f & ", ReadOnly=" & ReadOnly
  Set OpenShapeDBF = OpenDBF(f, ReadOnly)
End Function

Public Sub OpenMapCols()
  DbgMsg "OpenMapCols"
  frmMapCols.Hide
  Dim r&
  For r = 0 To LayerInfo(curLay).nFields - 1
    With LayerInfo(curLay).Fields(r)
      frmMapCols.AddField .Name, .Caption, .Column
    End With
  Next r
'  frmMapCols.SetRecordset MapRS
  frmMapCols.Show
End Sub

Private Sub cmdSelLocations_Click(Index As Integer)
  If Index = 0 Then
    MapAction 250  'Select All
  Else
    MapAction 260  'Select None
  End If
End Sub

Private Sub fraLegend_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  MapAction 300 'Edit Legend
End Sub

Private Sub lblLegend_MouseUp(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
  MapAction 300 'Edit Legend
End Sub

Private Sub lblNselected_DblClick()
  RefreshMapLayer = Not RefreshMapLayer 'For debugging of annoying MOLt behavior
End Sub

Private Sub LegendScroll_Change()
  If LegendScroll.Visible Then SetLegendPositions False
End Sub

Private Sub LegendScroll_GotFocus()
  pctLegend(0).SetFocus
End Sub

'MapObjects: Private Sub Map1_AfterLayerDraw(ByVal Index As Integer, ByVal canceled As Boolean, ByVal hDC As stdole.OLE_HANDLE)
Private Sub Map1_AfterLayerDraw(ByVal Index As Integer, ByVal hdc As stdole.OLE_HANDLE)
  If Index = curLay Then
    HighlightSelectedShapes
    If RefreshLayer And Len(CurrentMapShapeName) > 0 Then HighlightCurrentShape 1
  End If
End Sub

Private Sub Map1_BeforeLayerDraw(ByVal Index As Integer, ByVal hdc As stdole.OLE_HANDLE)
  If Not RefreshLayer And Index = curLay And Len(CurrentMapShapeName) > 0 Then
    Select Case CurrentMapSymbol.SymbolType
      Case moPointSymbol: HighlightCurrentShape 1.75
      Case moLineSymbol:  HighlightCurrentShape 4
      Case moFillSymbol:  'Can't highlight areas yet
    End Select
  End If
End Sub

Private Sub Map1_LostFocus()
  If Len(CurrentMapShapeName) > 0 Then Highlight = ""
End Sub

Private Sub Map1_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  DbgMsg "Map1_MouseDown: (" & x & ", " & y & ") Button=" & Button
  If curLay < 0 Then
    'Debug.Print "MouseDown but no curLay"
  ElseIf Button = vbKeyRButton Then
    Map1_MouseDownX = x
    Map1_MouseDownY = y
    MapAction -1 'Deselect any mode (identify, zoom, add, move)
    ' do we want to respond to right mouse events?
  ElseIf tbrMap1.Buttons("Zoom").Value = tbrPressed Then
    Map1.Extent = Map1.TrackRectangle
    Map1_MouseDownX = x
    Map1_MouseDownY = y
    UnsavedChanges = True
  ElseIf tbrMap1.Buttons("Pan").Value = tbrPressed Then
    Map1.Pan
    UnsavedChanges = True
  ElseIf tbrMap1.Buttons("Add").Value = tbrPressed Then
    frmMapAddPt.Show vbModal, Me
    If frmMapAddPt.ok Then
      Dim pt As Point
      Set pt = Map1.ToMapPoint(x, y)
      AddPointToShapeFileOnMap frmMapAddPt.ptname, pt.x, pt.y
    End If
    tbrMap1.Buttons("Add").Value = tbrUnpressed
    fraMap1.MousePointer = vbDefault
  'ElseIf tbrMap1.Buttons("Remove").Value = tbrPressed Then

  ElseIf tbrMap1.Buttons("Move").Value = tbrPressed Then
    TrackNearestShape = False
    MapMovingPoint = IdPoint(x, y)
    Highlight = ""
  ElseIf pDeletingPoint Then
    If CurShapeFldVals.Count < 1 Then IdPoint x, y
    If CurShapeFldVals.Count < 1 Then
      MsgBox "Did not find a point to delete", vbOKOnly, "ATCoMap"
    ElseIf DeletePointOnMap(CurShapeFldVals(0)) Then
      MsgBox "Deleted a point", vbOKOnly, "ATCoMap"
    Else
      MsgBox "Did not delete a point", vbOKOnly, "ATCoMap"
    End If
    TrackNearestShape = False
    pDeletingPoint = False
    fraMap1.MousePointer = vbDefault
  ElseIf Button = vbKeyLButton Then
    HighlightShapeNear x, y    'IdPoint X, Y
    SelectHighlightedPoint -1   'toggle select state
  End If
End Sub

Private Sub Map1_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  'Static variables address a strange behavior: sometimes we see continuous MouseMove events in same place
  Static lastX As Single, lastY As Single
  If x = lastX And y = lastY Then Exit Sub
  lastX = x
  lastY = y
  'DbgMsg "Map1_MouseMove: (" & x & ", " & y & ") Button=" & Button
  If tbrMap1.Buttons("Move").Value = tbrPressed And MapMovingPoint > 0 Then
    RewritePointInShapeFile MapMovingPoint, Map1.ToMapPoint(x, y)
  ElseIf TrackNearestShape Then
    HighlightShapeNear x, y
  Else
    Map1.ToolTipText = ""
  End If
End Sub

Private Sub HighlightShapeNear(x As Single, y As Single)
  Dim HighlightString$, found As Boolean, i&
  
  IdPoint x, y
  If CurShapeFldVals.Count > 0 Then
    'This code looks for the key field, then sets the highlight string to the key
    'found = False
    'If Len(LayerInfo(curLay).KeyField) > 1 Then
    '  Dim keyIndex&
    '  keyIndex = 0
    '  While keyIndex < CurShapeFldVals.Count And Not found
    '    If Map1.Layers(curLay).Fields(keyIndex) = LayerInfo(curLay).KeyField Then found = True Else keyIndex = keyIndex + 1
    '  Wend
    '  If found Then
    '    If IsNumeric(CurShapeFldVals(keyIndex)) Then
    '      HighlightString = LayerInfo(curLay).KeyField & "=" & CurShapeFldVals(keyIndex) & ""
    '    Else
    '      HighlightString = LayerInfo(curLay).KeyField & "='" & CurShapeFldVals(keyIndex) & "'"
    '    End If
    '  End If
    'End If
    'If Not found Then
    HighlightString = "FeatureId=" & CurShapeFldVals(0)
    
    i = LayerInfo(curLay).TableIndex
    If i > 0 And sstLegend.Tab = i Then
      agdMapTable(i).row = CurShapeFldVals(0)
      If Not (agdMapTable(i).RowIsVisible(1) And agdMapTable(i).RowIsVisible(agdMapTable(i).rows)) Then
        'not all rows on one screen
        agdMapTable(LayerInfo(curLay).TableIndex).TopRow = CurShapeFldVals(0)
      End If
    End If
  Else
    HighlightString = ""
  End If
  Highlight = HighlightString
  CurShapeFldVals.Clear
End Sub


Private Sub Map1_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  DbgMsg "Map1_MouseUp: [" & x & ", " & y & "] Button=" & Button
  If tbrMap1.Buttons("Move").Value = tbrPressed Then
    MapMovingPoint = 0
    TrackNearestShape = True
  ElseIf tbrMap1.Buttons("Zoom").Value = tbrPressed Then
    If Map1_MouseDownX = x And Map1_MouseDownY = y Then
      Dim r As Rectangle
      Set r = Map1.Extent
      r.Offset (x / Map1.Width - 0.5) * r.Width, ((Map1.Height - y) / Map1.Height - 0.5) * r.Height
      If Button = vbLeftButton Then
        r.ScaleRectangle (0.5)
      ElseIf Button = vbRightButton Then
        r.ScaleRectangle (2#)
      End If
      Map1.Extent = r
      UnsavedChanges = True
    End If
  End If
  Map1.Refresh
End Sub

Private Sub pctLegend_MouseUp(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
  MapAction 300 'Edit Legend
End Sub

Private Sub sash_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  SashDragging = True
End Sub

Private Sub sash_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  SashDragging = False
  Dim t&, newHeight&
  newHeight = sstLegend.Height - 477
  If newHeight > 100 Then
    fraLegend.Height = newHeight
    LegendScroll.Height = newHeight
    SetLegendPositions True
    For t = 1 To agdMapTable.Count
      agdMapTable(t).Height = newHeight
    Next t
  End If
End Sub

Private Sub sash_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  Dim newHeight&
  If SashDragging And (sash.Top + y) > 0 And (sash.Top + y < Height) Then
    sash.Top = sash.Top + y
    newHeight = Height - (sash.Top + sash.Height)
    If newHeight > 0 Then sstLegend.Height = newHeight
    UserControl_Resize
  End If
End Sub

Private Sub sstLegend_Click(PreviousTab As Integer)
  DbgMsg "sstLegend_Click from tab " & PreviousTab & " to " & sstLegend.Tab
  Dim l&, LayerName$
  If sstLegend.Tab > 0 Then
    LayerName = sstLegend.TabCaption(sstLegend.Tab)
    For l = 0 To nLayers - 1
      If LayerName = LayerInfo(l).Name And curLay <> l Then CurLayer = l
    Next l
  End If
  If sstLegend.Tab > 0 And agdMapTable.Count >= sstLegend.Tab Then
    lblNselected.Caption = LayerInfo(curLay).nSelected & " of " & agdMapTable(sstLegend.Tab).rows
    lblNselected.Left = cmdSelLocations(0).Left - lblNselected.Width - 100
  Else
    lblNselected.Caption = ""
  End If
  If TrackingNearestShape And Map1.Layers.Count >= curLay Then
    If Map1.Layers(curLay).symbol.SymbolType = moFillSymbol Then MapAction -1
  End If
End Sub

Private Sub tbrMap1_ButtonClick(ByVal Button As MSComctlLib.Button)
  DbgMsg "tbrMap1:Click:" & Button.Key
  If IsNumeric(Button.Tag) Then
    MapAction CInt(Button.Tag)
  'ElseIf Button.Tag = "All" Then
  '  mnuLoc_Click 0
  'ElseIf Button.Tag = "None" Then
  '  mnuLoc_Click 1
  Else
    MsgBox "Non-numeric toolbar button tag in tbrMap1_ButtonClick: " & Button.Key & " = " & Button.Tag
  End If
End Sub

Private Sub UserControl_Initialize()
  Dim i&
  curLay = 0
  On Error Resume Next
  With tbrMap1.Buttons
    For i = 1 To .Count
      If .item(i).Image = 0 And Left(.item(i).Key, 9) <> "Separator" Then .item(i).Image = .item(i).Key
    Next i
  End With
  With tbrMap1.Buttons(tbrMap1.Buttons.Count)
    tbrMap1.Width = .Left + .Width
  End With
  On Error GoTo 0
  With agdMapTable(1)
    .Clear
    .AllowBigSelection = False
    .rows = 1
  End With
End Sub

Private Sub UserControl_InitProperties()
  MapHighlightColor = moYellow
End Sub

Private Sub UserControl_ReadProperties(PropBag As PropertyBag)
  Let RefreshMapLayer = PropBag.ReadProperty("RefreshMapLayer", True)
  Let ConfirmSelections = PropBag.ReadProperty("ConfirmSelections", False)
  Let Enabled = PropBag.ReadProperty("Enabled", 2)
  Let LegendVisible = PropBag.ReadProperty("LegendVisible", True)
  Let ToolbarVisible = PropBag.ReadProperty("ToolbarVisible", True)
End Sub

Private Sub UserControl_Show()
  If Enabled Then Let LegendVisible = LegendIsVisible
  Let ToolbarVisible = ToolbarIsVisible
End Sub

Private Sub UserControl_WriteProperties(PropBag As PropertyBag)
  PropBag.WriteProperty "RefreshMapLayer", RefreshMapLayer
  PropBag.WriteProperty "ConfirmSelections", ConfirmSelections
  PropBag.WriteProperty "Enabled", Enabled
  PropBag.WriteProperty "LegendVisible", LegendVisible
  PropBag.WriteProperty "ToolbarVisible", ToolbarVisible
End Sub

Private Sub UserControl_Resize()
  Dim h&, w&, i&, LegendHeight&
  h = Height
  w = Width
  If LegendVisible Then LegendHeight = sstLegend.Height + sash.Height Else LegendHeight = 0
  If h > (fraMap1.Top + LegendHeight) Then
    fraMap1.Height = h - (fraMap1.Top + LegendHeight)
    Map1.Height = fraMap1.Height
    If LegendVisible Then
      sash.Top = fraMap1.Top + fraMap1.Height
      sstLegend.Top = sash.Top + sash.Height
    End If
  End If
  If w > 250 Then
    fraMap1.Width = w '- 240
    Map1.Width = fraMap1.Width
    If LegendVisible Then
      cmdSelLocations(1).Left = Map1.Width - cmdSelLocations(1).Width
      cmdSelLocations(0).Left = cmdSelLocations(1).Left - cmdSelLocations(0).Width - 100
      lblNselected.Left = cmdSelLocations(0).Left - lblNselected.Width - 100
      sstLegend.Width = fraMap1.Width
      sash.Width = sstLegend.Width
      If sstLegend.Width > 300 Then
        fraLegend.Width = sstLegend.Width - 237
        LegendScroll.Left = fraLegend.Width - 255
        SetLegendPositions True
        For i = 1 To agdMapTable.Count
          agdMapTable(i).Width = sstLegend.Width - 282
        Next i
      End If
    End If
  End If

End Sub

Public Sub SetLegend()
  Dim MapLegendCnt As Integer 'number of items in legend (rendered layers have more than 1)
  Dim i&, j&, boxsize&
  Dim layr As MapLayer, layrIndex&, RendType As ATCoRendType
  
  DbgMsg "SetLegend"
  fraLegend.Enabled = True
  pctLegend(0).Enabled = True
  pctLegend(0).Top = lblLegend(0).Top ' start at right spot
  'pctLegend(0).Cls
  lblLegend(0).Enabled = True
  
  'fraLegend.Visible = False
  MapLegendCnt = lblLegend.Count
  For i = 1 To MapLegendCnt - 1 'clear off old legend
    Unload lblLegend(i)
    Unload pctLegend(i)
  Next i

  MapLegendCnt = 0
  LegendScroll.Value = 0
  
  ' loop thru layers on map
  i = -1
  For layrIndex = 0 To nLayers - 1
    RendType = LayerInfo(layrIndex).RendType
    Set layr = Map1.Layers(layrIndex) 'Each layr In Map1.Layers
    If layr.Visible Then
      'create marker symbols
      If RendType = ATCoNoRend Then
        MapLegendCnt = MapLegendCnt + 1
        SetLegendItem i, layr.Name
        pctLegend(i).BackColor = fraLegend.BackColor
        Select Case layr.symbol.SymbolType
          Case moPointSymbol: SetLegendPoint pctLegend(i), layr.symbol
          Case moLineSymbol:  pctLegend(i).Line (0, pctLegend(i).Height / 2)-(pctLegend(i).Width, pctLegend(i).Height / 2), layr.symbol.Color
          Case moFillSymbol
            pctLegend(i).ForeColor = layr.symbol.OutlineColor
            boxsize = pctLegend(i).Height - 10
            If layr.symbol.Style = moTransparentFill Then ' draw a box
              pctLegend(i).BackColor = fraLegend.BackColor
            Else ' fill
              pctLegend(i).BackColor = layr.symbol.Color
            End If
            pctLegend(i).Line (0, 0)-(boxsize, 0)
            pctLegend(i).Line -(boxsize, boxsize)
            pctLegend(i).Line -(0, boxsize)
            pctLegend(i).Line -(0, 0)
        End Select
      Else
        Dim rendsym&, maxrendsym&
        If RendType = ATCoValueRend Then
          maxrendsym = layr.Renderer.ValueCount - 1
        Else
          maxrendsym = layr.Renderer.BreakCount - 1
        End If
        For rendsym = 0 To maxrendsym
          MapLegendCnt = MapLegendCnt + 1
          If RendType = ATCoValueRend Then
            SetLegendItem i, layr.Name & ":" & layr.Renderer.Value(rendsym)
          Else
            SetLegendItem i, layr.Name & ":" & layr.Renderer.Break(rendsym)
          End If
          With layr.Renderer
            Select Case .symbol(rendsym).SymbolType
              Case moPointSymbol: SetLegendPoint pctLegend(i), .symbol(rendsym) ', 0
              Case moLineSymbol:  pctLegend(i).Line (0, pctLegend(i).Height / 2)-(pctLegend(i).Width, pctLegend(i).Height / 2), layr.Renderer.symbol(rendsym).Color
              Case moFillSymbol:
                pctLegend(i).ForeColor = layr.Renderer.symbol(rendsym).OutlineColor
                boxsize = pctLegend(i).Height - 10
                If .symbol(rendsym).Style = moTransparentFill Then ' draw a box
                  pctLegend(i).BackColor = fraLegend.BackColor
                Else ' fill
                  pctLegend(i).BackColor = layr.Renderer.symbol(rendsym).Color
                End If
                pctLegend(i).Line (0, 0)-(boxsize, 0)
                pctLegend(i).Line -(boxsize, boxsize)
                pctLegend(i).Line -(0, boxsize)
                pctLegend(i).Line -(0, 0)
            End Select
          End With
        Next rendsym
      End If
    End If
  Next 'layr
  Set layr = Nothing
  SetLegendPositions False
  fraLegend.Visible = True
End Sub

Private Sub SetLegendPoint(p As PictureBox, symbol As symbol)
  Dim S$, f#, h#, u&
  DbgMsg "SetLegendPoint " & p.Index
  With p
    .FillStyle = vbTransparent
    If symbol.Style = moTrueTypeMarker Then
      .ForeColor = symbol.Color
      .AutoSize = True
      .AutoRedraw = True
      .FontTransparent = False
      .Font = symbol.Font
      S = Chr(symbol.CharacterIndex)
      .Width = .TextWidth(S)
      .Height = .TextHeight(S)
      p.Print S
    Else
      .FillColor = symbol.Color
      .ForeColor = symbol.OutlineColor
      If symbol.Style = moCircleMarker Then
        p.Circle (75, 75), 60
      ElseIf symbol.Style = moCrossMarker Then
        p.Line (75, 25)-(75, 125), .FillColor
        p.Line (25, 75)-(125, 75), .FillColor
      ElseIf symbol.Style = moTriangleMarker Then
        p.Line (75, 25)-(125, 125), .ForeColor
        p.Line -(25, 125), .ForeColor
        p.Line -(75, 25), .ForeColor
        FloodFill .hdc, .ScaleX(75, p.ScaleMode, vbPixels), _
                        .ScaleY(100, p.ScaleMode, vbPixels), .ForeColor
      Else ' all others If layr.Renderer.symbol(0).Style = moSquareMarker Then
        p.Line (25, 25)-(125, 125), p.ForeColor, B
      End If
    End If
  End With
End Sub

Private Sub SetLegendPositions(ByVal resizeLegend As Boolean)
  Static FirstTop&, FirstPctLeft&, FirstLblLeft&, ItemHeight&
  Static lastFraHeight&, lastFraWidth&
  Static displayRows&, rows&, cols&, colWidth() As Long 'colWidth = max width of lblLegend in each col
  DbgMsg "SetLegendPositions"
  If FirstLblLeft = 0 Then
    Set pctLegend(0).Font = lblLegend(0).Font
    FirstTop = lblLegend(0).Top
    FirstPctLeft = pctLegend(0).Left
    FirstLblLeft = lblLegend(0).Left
    ItemHeight = lblLegend(0).Height + 50
    displayRows = (fraLegend.Height - FirstTop * 2) / ItemHeight
    rows = displayRows
    cols = 1
    ReDim colWidth(1)
    colWidth(1) = fraLegend.Width
  End If
  
  Dim nItems&, item&, topItem&, itemsInView&, r&, c&
  Dim vPos&, hPosLbl&, hPosPct, maxWidth&
  
  nItems = lblLegend.Count
  
  If resizeLegend And lastFraHeight = fraLegend.Height And lastFraWidth = fraLegend.Width Then
    'discard useless resize events
  Else
    For item = 0 To nItems - 1 'move all legend items out of view
      lblLegend(item).Top = -500
      pctLegend(item).Top = -500
    Next item
    
    If resizeLegend Then
      LegendScroll.Visible = False
      displayRows = (fraLegend.Height - FirstTop * 2) / ItemHeight
      rows = displayRows
      cols = 1
      ReDim colWidth(1)
      colWidth(1) = fraLegend.Width
      While rows < nItems And widthUsingRows(rows, cols, colWidth, nItems) > (fraLegend.Width - FirstLblLeft * cols)
        rows = rows + 1
      Wend
      LegendScroll.Value = 0
    End If
    
    If LegendScroll.Visible Then
      topItem = LegendScroll.Value
      LegendScroll.Visible = False
    Else
      topItem = 0
    End If
    
    hPosLbl = FirstLblLeft
    hPosPct = FirstPctLeft
    itemsInView = 0
    For c = 1 To cols
      vPos = FirstTop
      item = topItem + (c - 1) * rows
      r = 1
      While r <= displayRows And item < nItems
        lblLegend(item).Top = vPos
        pctLegend(item).Top = vPos
        lblLegend(item).Left = hPosLbl
        pctLegend(item).Left = hPosPct
        vPos = vPos + ItemHeight
        item = item + 1
        itemsInView = itemsInView + 1
        r = r + 1
      Wend
      hPosPct = hPosPct + colWidth(c) + FirstLblLeft
      hPosLbl = hPosPct + FirstLblLeft - FirstPctLeft
    Next c
    If itemsInView < nItems Then 'need scrollbar
      LegendScroll.Min = 0
      LegendScroll.Max = rows - displayRows
      If LegendScroll.Max > LegendScroll.Min Then
        LegendScroll.Value = topItem
        LegendScroll.Visible = True
        'LegendScroll.SetFocus
      End If
    Else
      LegendScroll.Visible = False
    End If
  End If
  lastFraHeight = fraLegend.Height
  lastFraWidth = fraLegend.Width
End Sub

Private Function widthUsingRows(rows&, cols&, colWidth() As Long, nItems&) As Long
  Dim c&, item&, colTopItem&, totalWidth&
  cols = 1
  ReDim colWidth(99)
  colWidth(cols) = lblLegend(0).Width
  colTopItem = 0
  For item = 1 To nItems - 1
    If item - colTopItem >= rows Then
      colTopItem = item
      cols = cols + 1
      colWidth(cols) = lblLegend(item).Width
    Else
      If lblLegend(item).Width > colWidth(cols) Then colWidth(cols) = lblLegend(item).Width
    End If
  Next item
  totalWidth = 0
  For c = 1 To cols
    totalWidth = totalWidth + colWidth(c)
  Next c
  widthUsingRows = totalWidth
  DbgMsg "widthUsingRows = " & totalWidth
End Function

Private Sub SetLegendItem(ByRef i&, lbl$)
  DbgMsg "SetLegendItem " & i & " = " & lbl
  i = i + 1
  If i > 0 Then
    Load lblLegend(i)
    Load pctLegend(i)
  End If
  pctLegend(i).Height = 150
  pctLegend(i).Width = 150
  pctLegend(i).Font = lblLegend(i).Font
  
  lblLegend(i).Caption = lbl
  lblLegend(i).Width = pctLegend(i).TextWidth(lbl)
  lblLegend(i).Visible = True
  pctLegend(i).Visible = True
End Sub

Private Sub UnloadLegend()
  Dim i&, Count&
  DbgMsg "UnloadLegend"
  Count = lblLegend.Count
  lblLegend(0).Visible = False
  pctLegend(0).Visible = False
  fraLegend.Enabled = False
  For i = 1 To Count - 1 'clear off old legend
    Unload lblLegend(i)
    Unload pctLegend(i)
  Next i
End Sub

Private Sub PopulateGrid(g As ATCoGrid, inf As MapLayerInfo)
  Dim pt&, prop As Variant, o&, i&, MapRS As Recordset, Val
  DbgMsg "PopulateGrid"
  pt = 1
  Set MapRS = OpenDBF(inf.path & inf.baseFilename & ".shp", True)
  On Error GoTo AbortSub
  MapRS.MoveFirst
  'Debug.Print MapRS.Name
  g.ClearData
  
  'g.rows could be set to zero here, but this saves us adding rows later. RecordCount need not be exact, but should be <= actual number
  g.rows = MapRS.RecordCount
  g.cols = inf.nFields 'MapRS.Fields.Count
  For i = 0 To g.cols - 1 '0 To MapRS.Fields.Count - 1
    g.ColTitle(i) = inf.Fields(i).Caption
    If inf.Fields(i).Name = inf.keyField Then
      g.ColEditable(i) = False 'g.ColSelectable(i) = True
    Else
      g.ColEditable(i) = True  'g.ColSelectable(i) = False
    End If
    g.ColSelectable(i) = True
  Next i
  While Not MapRS.EOF
    If g.rows < pt Then
      g.rows = pt + 100 'over-allocate so we don't have to for each element.
      ReDim Preserve inf.Selected(pt + 101)
    End If
    For i = 0 To g.cols - 1
      Val = MapRS.Fields(inf.Fields(i).Name).Value
      If Not IsNull(Val) Then
        g.TextMatrix(pt, i) = Val
      Else
        g.TextMatrix(pt, i) = ""
      End If
    Next i
    pt = pt + 1
    MapRS.MoveNext
  Wend
  MapRS.Close
  g.rows = pt - 1
  If pt < 1000 Then g.ColsSizeByContents
  ReDim Preserve inf.Selected(pt)
  Exit Sub
AbortSub:
  MsgBox "Error populating map grid" & vbCr & Err.Description
End Sub

