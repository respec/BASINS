VERSION 5.00
Begin VB.Form frmShapeAttributes 
   Caption         =   "Shape File Attributes"
   ClientHeight    =   4725
   ClientLeft      =   60
   ClientTop       =   285
   ClientWidth     =   4560
   LinkTopic       =   "Form1"
   ScaleHeight     =   4725
   ScaleWidth      =   4560
   StartUpPosition =   3  'Windows Default
   Begin VB.ListBox lstAttributes 
      Height          =   2790
      Left            =   120
      MultiSelect     =   1  'Simple
      TabIndex        =   7
      Top             =   1080
      Width           =   4332
   End
   Begin VB.ComboBox cboY 
      Height          =   288
      Left            =   1560
      TabIndex        =   2
      Top             =   444
      Width           =   2892
   End
   Begin VB.ComboBox cboX 
      Height          =   288
      Left            =   1560
      TabIndex        =   0
      Top             =   120
      Width           =   2892
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   1080
      TabIndex        =   4
      Top             =   4200
      Width           =   2532
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   1440
         TabIndex        =   6
         Top             =   0
         Width           =   1092
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "Ok"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   0
         TabIndex        =   5
         Top             =   0
         Width           =   1092
      End
   End
   Begin VB.Label lblAttributes 
      Caption         =   "Select data attributes to bring into shape file:"
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
      Left            =   120
      TabIndex        =   8
      Top             =   840
      Width           =   4332
   End
   Begin VB.Label lblY 
      Caption         =   "Y (Latitude)"
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
      Left            =   120
      TabIndex        =   3
      Top             =   480
      Width           =   1332
   End
   Begin VB.Label lblX 
      Caption         =   "X (Longitude)"
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
      Left            =   120
      TabIndex        =   1
      Top             =   156
      Width           =   1332
   End
End
Attribute VB_Name = "frmShapeAttributes"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private OkPressed As Boolean

Public Function Ok() As Boolean
  If OkPressed Then
    If Len(cboX.Text) > 0 And Len(cboY.Text) > 0 Then
      Ok = True
    End If
  End If
End Function

Private Sub cmdCancel_Click()
  OkPressed = False
  Me.Hide
End Sub

Private Sub cmdOk_Click()
  OkPressed = True
  Me.Hide
End Sub

'Private Sub agd_Click()
'  If agd.col = 0 Then
'    If Len(agd.TextMatrix(agd.row, 1)) > 0 Then
'      agd.TextMatrix(agd.row, 1) = ""
'    Else
'      agd.TextMatrix(agd.row, 1) = agd.TextMatrix(agd.row, 0)
'    End If
'  End If
'End Sub

Private Sub Form_Load()
  OkPressed = False
'  agd.ColTitle(0) = "Data File Attribute"
'  agd.ColTitle(1) = "Shape Field Name"
'  agd.ColEditable(0) = False
'  agd.ColEditable(1) = True
End Sub

Private Sub Form_Resize()
  Dim w&, h&
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  fraButtons.Left = (w - fraButtons.Width) / 2
  fraButtons.Top = h - 456
  If fraButtons.Top > 1500 Then lstAttributes.Height = fraButtons.Top - 1272
  If w > cboX.Left + 300 Then
    lstAttributes.Width = w - 228
    cboX.Width = w - cboX.Left - 108
    cboY.Width = cboX.Width
  End If
End Sub

'Public Sub SetDataFile(tsfile As ATCclsTserFile)
'  Dim AllAttributes As Collection
'  Dim Attrib As String
'  Dim i As Long
'  Dim lastX As String
'  Dim lastY As String
'  Dim lastSelected As New FastCollection
'
'  lastX = GetSetting(App.EXEName, "ShapeAttributes", "X", "")
'  lastY = GetSetting(App.EXEName, "ShapeAttributes", "Y", "")
'  i = 1
'  Attrib = GetSetting(App.EXEName, "ShapeAttributes", "last" & i, "")
'  While Len(Attrib) > 0
'    lastSelected.Add Attrib, Attrib
'    i = i + 1
'    Attrib = GetSetting(App.EXEName, "ShapeAttributes", "last" & i, "")
'  Wend
'
'  Set AllAttributes = tsfile.AvailableAttributes
'  cboX.Clear
'  cboY.Clear
'  lstAttributes.Clear
'  For i = 1 To AllAttributes.Count
'    Attrib = AllAttributes(i)
'    cboX.AddItem Attrib: If Attrib = lastX Then cboX.ListIndex = i
'    cboY.AddItem Attrib: If Attrib = lastY Then cboY.ListIndex = i
'    lstAttributes.AddItem Attrib
'    If lastSelected.KeyExists(Attrib) Then lstAttributes.Selected(i) = True
'  Next
'End Sub

Public Function SelectedAttributes() As String()
  Dim curLst As Long
  Dim curSel As Long
  Dim retval() As String
  With lstAttributes
    ReDim retval(.SelCount)
    For curLst = 1 To .ListCount
      If .Selected(curLst) Then
        curSel = curSel + 1
        retval(curSel) = .List(curLst)
      End If
    Next
  End With
End Function
