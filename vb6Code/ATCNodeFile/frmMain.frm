VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmMain 
   Caption         =   "Create EFDC Maps"
   ClientHeight    =   4080
   ClientLeft      =   45
   ClientTop       =   270
   ClientWidth     =   5865
   LinkTopic       =   "Form1"
   ScaleHeight     =   4080
   ScaleWidth      =   5865
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdReadShape 
      Caption         =   "Read Shape"
      Height          =   372
      Left            =   3720
      TabIndex        =   4
      Top             =   120
      Visible         =   0   'False
      Width           =   1092
   End
   Begin VB.CommandButton cmdWriteShp 
      Caption         =   "Write Shape"
      Enabled         =   0   'False
      Height          =   372
      Left            =   2520
      TabIndex        =   3
      Top             =   120
      Width           =   1092
   End
   Begin VB.PictureBox pct 
      AutoRedraw      =   -1  'True
      Height          =   3372
      Left            =   120
      ScaleHeight     =   3315
      ScaleWidth      =   5595
      TabIndex        =   2
      Top             =   600
      Width           =   5652
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   4920
      Top             =   120
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      CancelError     =   -1  'True
   End
   Begin VB.CommandButton cmdDraw 
      Caption         =   "Redraw"
      Enabled         =   0   'False
      Height          =   372
      Left            =   1320
      TabIndex        =   1
      Top             =   120
      Width           =   1092
   End
   Begin VB.CommandButton cmdOpen 
      Caption         =   "Open File"
      Height          =   372
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   1092
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Dim fil As ATCclsNodeFile

Private Sub cmdDraw_Click()
  RedrawNodeNames
End Sub

Private Sub RedrawNodeNames()
  Dim vNode As Variant
  Dim nod As ATCclsNode
  Dim shap As ATCclsShape
  Dim X As Double
  Dim Y As Double
  Dim maxX As Double
  Dim minX As Double
  Dim maxY As Double
  Dim minY As Double
  
  minX = 1E+30
  minY = 1E+30
  maxX = -1E+30
  maxY = -1E+30
  pct.Cls
  
  If fil Is Nothing Then
    MsgBox "Must open a file first"
    Exit Sub
  End If
  
  If fil.Nodes.Count > 0 Then
    For Each vNode In fil.Nodes
      Set nod = vNode
      Set shap = nod.Shapes(1)
      X = shap.Vertices(0, 0)
      Y = shap.Vertices(0, 1)
      If X > maxX Then maxX = X
      If X < minX Then minX = X
      If Y > maxY Then maxY = Y
      If Y < minY Then minY = Y
    Next
    pct.ScaleLeft = minX - (maxX - minX) / 20
    If maxX > minX Then pct.ScaleWidth = (maxX - minX) * 1.1
    pct.ScaleTop = maxY + (maxY - minY) / 20
    If maxY > minY Then pct.ScaleHeight = (minY - maxY) * 1.1
      
    For Each vNode In fil.Nodes
      Set nod = vNode
      Set shap = nod.Shapes(1)
      'pct.Circle (shap.Vertices(0, 0), shap.Vertices(0, 1)), pct.ScaleWidth / 100, vbBlack
      pct.CurrentX = shap.Vertices(0, 0)
      pct.CurrentY = shap.Vertices(0, 1)
      pct.Print vNode.Attributes.Value("ID")

    Next
  End If
End Sub

Private Sub cmdOpen_Click()
  On Error GoTo NeverMind
  cdlg.DialogTitle = "Open EFDC Cellnet Input File"
  cdlg.Filter = "EFDC Cellnet Input Files (*.inp)|*.inp|All Files (*.*)|*.*"
  'If Len(cdlg.Filename) > 3 Then
  '  cdlg.Filename = Left(cdlg.Filename, Len(cdlg.Filename) - 3) & "out"
  'End If
  cdlg.ShowOpen
  If Len(cdlg.Filename) > 0 Then
    If Len(Dir(cdlg.Filename)) > 0 Then
      Set fil = Nothing
      If LCase(Right(cdlg.Filename, 3)) = "dbf" Then
        Set fil = New clsNodeDBF
      Else
        Set fil = New clsNodeEFDC
      End If
      Me.MousePointer = vbHourglass
      fil.Filename = cdlg.Filename
      cmdDraw.Enabled = True
      cmdWriteShp.Enabled = True
      RedrawNodeNames
      Me.MousePointer = vbDefault
    Else
      MsgBox "File not found: " & cdlg.Filename
    End If
  End If
NeverMind:
End Sub

Private Sub cmdReadShape_Click()
  'frmShpViewer.Show
End Sub

Private Sub cmdWriteShp_Click()
  
  If fil Is Nothing Then
    MsgBox "Must open a file first"
    Exit Sub
  End If
  On Error GoTo NeverMind
  cdlg.DialogTitle = "Name the shape files"
  cdlg.Filter = "Shape Files (*.shp)|*.shp|All Files (*.*)|*.*"
  If Len(cdlg.Filename) > 3 Then
    cdlg.Filename = Left(cdlg.Filename, Len(cdlg.Filename) - 3) & "shp"
  End If
  cdlg.ShowSave
  fil.WriteShapeFile Left(cdlg.Filename, Len(cdlg.Filename) - 4)
  If MsgBox("Add this layer to a map file or create new map file?", vbYesNo, "Write Shape") = vbYes Then
    cdlg.DialogTitle = "Select a map file (or create a new one)"
    cdlg.Filter = "Map Files (*.map)|*.map|All Files (*.*)|*.*"
    If Len(cdlg.Filename) > 3 Then
      cdlg.Filename = Left(cdlg.Filename, Len(cdlg.Filename) - 3) & "map"
    End If
    cdlg.ShowOpen
    If Len(cdlg.Filename) > 0 Then fil.WriteMapFile cdlg.Filename
  End If
NeverMind:
End Sub

Private Sub Form_Resize()
  If Me.ScaleWidth > 250 Then pct.Width = Me.ScaleWidth - 216
  If Me.ScaleHeight > 700 Then pct.Height = Me.ScaleHeight - 696
End Sub
