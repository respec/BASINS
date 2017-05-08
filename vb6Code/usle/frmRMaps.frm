VERSION 5.00
Object = "*\A..\..\VBEXPE~1\ATCoCtl\ATCoCtl.vbp"
Object = "{64925800-D0EE-11CF-989B-00805F7CED21}#1.0#0"; "MOLT10.OCX"
Begin VB.Form frmRMaps 
   Caption         =   "Form1"
   ClientHeight    =   7596
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   5868
   HelpContextID   =   20
   Icon            =   "frmRMaps.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   7596
   ScaleWidth      =   5868
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdHelp 
      Height          =   444
      Left            =   3120
      Picture         =   "frmRMaps.frx":0CFA
      Style           =   1  'Graphical
      TabIndex        =   5
      Tag             =   "Help"
      ToolTipText     =   "Access Help on R Factor"
      Top             =   288
      Width           =   492
   End
   Begin VB.CommandButton cmdZoom 
      Height          =   444
      Index           =   2
      Left            =   1320
      Picture         =   "frmRMaps.frx":10DD
      Style           =   1  'Graphical
      TabIndex        =   2
      Tag             =   "Zoom"
      ToolTipText     =   "Zoom Out"
      Top             =   288
      Width           =   492
   End
   Begin VB.CommandButton cmdZoom 
      Height          =   444
      Index           =   1
      Left            =   720
      Picture         =   "frmRMaps.frx":14C5
      Style           =   1  'Graphical
      TabIndex        =   1
      Tag             =   "Zoom"
      ToolTipText     =   "Zoom In"
      Top             =   288
      Width           =   492
   End
   Begin VB.CommandButton cmdFull 
      Height          =   444
      Left            =   2520
      Picture         =   "frmRMaps.frx":18AF
      Style           =   1  'Graphical
      TabIndex        =   4
      Tag             =   "Full"
      ToolTipText     =   "Full Extent"
      Top             =   288
      Width           =   492
   End
   Begin VB.CommandButton cmdZoom 
      Height          =   444
      Index           =   0
      Left            =   120
      Picture         =   "frmRMaps.frx":1CF1
      Style           =   1  'Graphical
      TabIndex        =   0
      Tag             =   "Zoom"
      ToolTipText     =   "Drag-select an area to zoom in on"
      Top             =   288
      Width           =   492
   End
   Begin VB.CommandButton cmdPan 
      Height          =   444
      Left            =   1920
      Picture         =   "frmRMaps.frx":20D6
      Style           =   1  'Graphical
      TabIndex        =   3
      Tag             =   "Pan"
      ToolTipText     =   "Pan"
      Top             =   288
      Width           =   492
   End
   Begin MapObjectsLT.Map RMaps 
      Height          =   4692
      Left            =   120
      TabIndex        =   6
      Top             =   756
      Width           =   5652
      _Version        =   65536
      _ExtentX        =   9970
      _ExtentY        =   8276
      _StockProps     =   161
      BackColor       =   16777215
      BorderStyle     =   1
      Contents        =   "frmRMaps.frx":24C3
   End
   Begin VB.Frame fraSelectMap 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   2052
      Left            =   120
      TabIndex        =   11
      Top             =   5520
      Width           =   5772
      Begin VB.CommandButton cmdRMap 
         Caption         =   "OK"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Index           =   0
         Left            =   3000
         TabIndex        =   16
         ToolTipText     =   "Insert R Factor Value in Main Table"
         Top             =   1680
         Width           =   612
      End
      Begin VB.CommandButton cmdRMap 
         Caption         =   "Cancel"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Index           =   1
         Left            =   3720
         TabIndex        =   15
         ToolTipText     =   "Return to Main Table without inserting R Factor value"
         Top             =   1680
         Width           =   732
      End
      Begin VB.CommandButton cmdRMap 
         Caption         =   "More on R"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Index           =   2
         Left            =   4560
         TabIndex        =   14
         ToolTipText     =   "Access Help on R Factor"
         Top             =   1680
         Width           =   1092
      End
      Begin VB.CommandButton cmdRMaps 
         Caption         =   "Eastern US"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1332
         Index           =   0
         Left            =   4320
         Picture         =   "frmRMaps.frx":24DD
         Style           =   1  'Graphical
         TabIndex        =   10
         ToolTipText     =   "Display R Factor Map for Eastern US"
         Top             =   240
         Width           =   1332
      End
      Begin VB.CommandButton cmdRMaps 
         Caption         =   "Western US"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1332
         Index           =   1
         Left            =   2880
         Picture         =   "frmRMaps.frx":28B6
         Style           =   1  'Graphical
         TabIndex        =   9
         ToolTipText     =   "Display R Factor Map for Western US"
         Top             =   240
         Width           =   1332
      End
      Begin VB.CommandButton cmdRMaps 
         Caption         =   "California"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1332
         Index           =   2
         Left            =   1440
         Picture         =   "frmRMaps.frx":2A68
         Style           =   1  'Graphical
         TabIndex        =   8
         ToolTipText     =   "Display R Factor Map for California"
         Top             =   240
         Width           =   1332
      End
      Begin VB.CommandButton cmdRMaps 
         Caption         =   "Wash./Oregon"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1332
         Index           =   3
         Left            =   0
         Picture         =   "frmRMaps.frx":2C10
         Style           =   1  'Graphical
         TabIndex        =   7
         ToolTipText     =   "Display R Factor Map for Northwest"
         Top             =   240
         Width           =   1332
      End
      Begin ATCoCtl.ATCoText atxRVal 
         Height          =   252
         Left            =   2160
         TabIndex        =   17
         Top             =   1680
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   1
         SoftMax         =   1000
         SoftMin         =   1
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   0
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin VB.Label lblRVal 
         Caption         =   "Enter Estimated R Value:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   0
         TabIndex        =   18
         Top             =   1680
         Width           =   2172
      End
      Begin VB.Label Label2 
         Caption         =   "Use the buttons below to change the area displayed on the map."
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   0
         TabIndex        =   13
         Top             =   0
         Width           =   5772
      End
   End
   Begin VB.Label lblMap 
      Alignment       =   1  'Right Justify
      Caption         =   "Use Zoom and Pan tools to display relevant isoerodent lines and values."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   492
      Left            =   120
      TabIndex        =   12
      Top             =   60
      Width           =   5652
   End
End
Attribute VB_Name = "frmRMaps"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public TableRow As Long

Private newLayer As ImageLayer
Private HUClayer As New MapLayer
Private HUChighlighter As Symbol
Private Panning As Boolean
Private Zooming As Boolean

Private Sub cmdFull_Click()
  RMaps.Extent = RMaps.FullExtent
End Sub

Private Sub cmdHelp_Click()
  SendKeys "{F1}"
End Sub

Private Sub cmdPan_Click()
  Zooming = False
  Panning = True
End Sub

Private Sub cmdRMap_Click(index As Integer)
  If index = 2 Then
    SendKeys "{F1}"
  Else 'close form
    Me.Hide
    If index = 0 Then 'save R-Factor value in table
      frmTable.agdTable.TextMatrix(TableRow, RCOL) = atxRVal.Value
      frmTable.txtTotal = DoSedCalcs(frmTable.agdTable, TableRow)
    End If
  End If
End Sub

Private Sub cmdRMaps_Click(index As Integer)
  Dim lyr As Long, lyrName As String
  Me.MousePointer = vbHourglass
  lyrName = MapDir & "R_Fact_Map_" & CStr(index + 1) & ".bmp"
  SaveSetting AppRegName, "Defaults", "ImageLayer", lyrName
  For lyr = 0 To RMaps.Layers.Count - 1
    If RMaps.Layers(lyr).File <> MapDir & "huc2mna.shp" Then
      RMaps.Layers.Remove lyr
    End If
  Next
  Set newLayer = New ImageLayer
  newLayer.File = lyrName
  newLayer.Name = newLayer.File
  newLayer.Visible = True
  RMaps.Layers.Add newLayer
  RMaps.Layers.MoveToBottom 0 'Move HUC Layer back to top
  RMaps.Extent = newLayer.Extent
  
  Set newLayer = Nothing
  Debug.Print "Layers = " & RMaps.Layers.Count
  Me.MousePointer = vbDefault
End Sub


Private Sub cmdZoom_Click(index As Integer)
  Dim r As Rectangle
  Set r = RMaps.Extent
  Select Case index
    Case 0: Zooming = True
            Panning = False
    Case 1: r.ScaleRectangle (0.5)
            RMaps.Extent = r
    Case 2: r.ScaleRectangle (2#)
            RMaps.Extent = r
  End Select
End Sub

Private Sub Form_Load()
  HUClayer.File = MapDir & "huc2mna.shp"
  HUClayer.Symbol.Style = moTransparentFill
  HUClayer.Symbol.OutlineColor = moTeal
  Set newLayer = New ImageLayer
  
  newLayer.File = GetSetting(AppRegName, "Defaults", "ImageLayer", MapDir & "R_Fact_Map_1.bmp")
  newLayer.Name = newLayer.File
  newLayer.Visible = True
  RMaps.Layers.Add newLayer
  RMaps.Layers.Add HUClayer
  Set newLayer = Nothing
  If MapExtentSet Then Set RMaps.Extent = MapExtent
  Set HUChighlighter = New Symbol
  With HUChighlighter
    .SymbolType = RMaps.Layers(0).Symbol.SymbolType
    .Style = moSolidFill
    .OutlineColor = RMaps.Layers(0).Symbol.OutlineColor
    .Color = moRed
  End With
End Sub

Private Sub Form_Resize()
  If Width > 320 Then RMaps.Width = Width - 312
  If Height > 3300 Then RMaps.Height = Height - 3268
  fraSelectMap.Top = Height - 2480
'  lblRVal.Top = fraSelectMap.Top + fraSelectMap.Height + 50
'  atxRVal.Top = lblRVal.Top
'  cmdRMap(0).Top = lblRVal.Top
'  cmdRMap(1).Top = lblRVal.Top
'  cmdRMap(2).Top = lblRVal.Top
End Sub

Private Sub Form_Unload(Cancel As Integer)
  Set HUChighlighter = Nothing
  Set newLayer = Nothing
  Set HUClayer = Nothing
End Sub

Private Sub RMaps_AfterLayerDraw(ByVal index As Integer, ByVal hDC As Stdole.OLE_HANDLE)
  Dim CurrentMapShape As MapObjectsLT.Shape
  Dim mydb As Database, myHUCrs As Recordset
  Dim row As Long, HUCstr As Variant, SQL As String
  If index = 0 And CurHUCs.Count > 0 Then
    For Each HUCstr In CurHUCs
      Set CurrentMapShape = RMaps.Layers(0).find("HUC=" & HUCstr)
      'On Error Resume Next
      RMaps.DrawShape CurrentMapShape, HUChighlighter
      'On Error GoTo 0
      Set CurrentMapShape = Nothing
    Next
  End If
End Sub

Private Sub RMaps_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  If Zooming Then
    RMaps.Extent = RMaps.TrackRectangle
  ElseIf Panning Then
    RMaps.Pan
  End If
End Sub

