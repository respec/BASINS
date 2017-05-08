VERSION 5.00
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.9#0"; "ATCoCtl.ocx"
Begin VB.Form frmMultiple 
   Caption         =   "frmMultiple"
   ClientHeight    =   2850
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   10710
   Icon            =   "frmMultiple.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2850
   ScaleWidth      =   10710
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoGrid agd 
      Height          =   2292
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   10692
      _ExtentX        =   18865
      _ExtentY        =   4048
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   37
      Cols            =   1
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "Select and/or type values to use"
      FixedRows       =   0
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   4
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483632
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   120
      TabIndex        =   1
      Top             =   2400
      Width           =   3372
      Begin VB.CommandButton cmdOk 
         Caption         =   "Use All"
         Height          =   372
         Index           =   1
         Left            =   1440
         TabIndex        =   3
         Top             =   0
         Width           =   972
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   372
         Left            =   2520
         TabIndex        =   4
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "Use Selected"
         Height          =   372
         Index           =   0
         Left            =   0
         TabIndex        =   2
         Top             =   0
         Width           =   1332
      End
   End
End
Attribute VB_Name = "frmMultiple"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

'Dim pColl As Collection
Dim plst As IXMLDOMNodeList

Private Sub agd_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  'Dim row As Long, col As Long
  If agd.MaxOccupiedRow >= agd.rows Then agd.rows = agd.MaxOccupiedRow + 1
  'row = agd.row
  'col = agd.col
  agd.DeselectAll
  'agd.Selected(row, col) = True
  'agd.row = row
  'agd.col = col
  cmdOk(0).SetFocus
  agd.SetFocus
End Sub

Private Sub agd_GotFocus()
'  Stop
End Sub

Private Sub cmdCancel_Click()
  Set plst = Nothing
  Me.Hide
End Sub

Private Sub cmdOk_Click(Index As Integer)
  Dim row As Long, col As Long
  Dim tagVal As String
  Dim tagName As String
  Dim attVal As String
  Dim xmlStr As String
  Dim tmpDoc As DOMDocument
  
  xmlStr = "<selected>" & vbCr
  tagName = agd.ColTitle(0)
  For row = 1 To agd.rows
    If agd.Selected(row, 0) Or Index = 1 Then
      tagVal = Trim(agd.TextMatrix(row, 0))
      If Len(tagVal) > 0 Then
        xmlStr = xmlStr & "<" & tagName
        For col = 1 To agd.cols - 1
          attVal = Trim(agd.TextMatrix(row, col))
          If Len(attVal) > 0 Then xmlStr = xmlStr & " " & agd.ColTitle(col) & "=""" & attVal & """"
        Next
        xmlStr = xmlStr & ">" & tagVal & "</" & tagName & ">" & vbCr
      End If
    End If
  Next
  xmlStr = xmlStr & "</selected>"
  
  Set tmpDoc = New DOMDocument
  tmpDoc.loadXML xmlStr
  
  'plst.Reset
  Set plst = Nothing
  Set plst = tmpDoc.getElementsByTagName(tagName)
  Set tmpDoc = Nothing
  Me.Hide
End Sub


Private Sub Form_Load()
  With agd
    .ColEditable(0) = True
    .ColSelectable(0) = True
    .ColType(0) = ATCoTxt
    '.SetFocus
  End With
End Sub

Private Sub Form_Resize()
  Dim h As Single
  Dim w As Single
  h = Me.ScaleHeight
  w = Me.ScaleWidth
  
  If w > 12 Then agd.Width = w - 12
  If h > 520 Then agd.Height = h - 520
  fraButtons.Top = agd.Top + agd.Height + 120
  fraButtons.Left = (w - fraButtons.Width) / 2
End Sub

'Public Property Get coll() As Collection
'  Set coll = pColl
'End Property
'Public Property Set coll(newValue As Collection)
'  Dim row As Long
'  Dim vItem As Variant
'  agd.rows = 1
'  agd.TextMatrix(1, 0) = ""
'  Set pColl = newValue
'  row = 0
'  For Each vItem In pColl
'    row = row + 1
'    agd.TextMatrix(row, 0) = vItem
'  Next
'  agd.rows = agd.rows + 1
'End Property

Public Property Get lst() As IXMLDOMNodeList
  Set lst = plst
End Property
Public Property Set lst(newValue As IXMLDOMNodeList)
  Dim row As Long, col As Long
  Dim vItem As Variant, vAttribute As Variant
  agd.FixedRows = 1
  agd.rows = 0
  agd.rows = 1
  agd.SelectionMode = ASdisjointByRow
  agd.ColTitle(0) = Me.Tag
  Set plst = newValue
  row = 0
  If Not plst Is Nothing Then
    For Each vItem In plst
      row = row + 1
      agd.TextMatrix(row, 0) = vItem.Text
      For Each vAttribute In vItem.Attributes
        For col = 1 To agd.cols - 1
          If LCase(agd.ColTitle(col)) = LCase(vAttribute.nodeName) Then
            agd.TextMatrix(row, col) = vAttribute.Text
            col = agd.cols + 2
          End If
        Next
        If col = agd.cols Then 'did not find matching column
          agd.ColTitle(col) = vAttribute.nodeName
          agd.TextMatrix(row, col) = vAttribute.Text
        End If
      Next
    Next
  End If
  agd.rows = agd.rows + 1
  agd.ColsSizeByContents
End Property

