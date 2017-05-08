VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
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
      Rows            =   54
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

Dim pColl As FastCollection
'Dim plst As ChilkatXml
'Dim pAbort As Boolean

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

'Private Sub agd_Error(code As Long, Description As String)
'  If MsgBox(Description, vbAbortRetryIgnore, "Error displaying grid") = vbAbort Then
'    pAbort = True
'  End If
'End Sub

Private Sub agd_GotFocus()
'  Stop
End Sub

Private Sub cmdCancel_Click()
  Set pColl = Nothing
  Me.Hide
End Sub

Private Sub cmdOk_Click(Index As Integer)
  Dim row As Long, col As Long
  Dim tagVal As String
  Dim tagName As String
  Dim attVal As String
  Dim xmlStr As String
  Dim rowStr As String
  Dim tmpxml As ChilkatXml
  
  Me.MousePointer = vbHourglass
  
  'xmlStr = "<selected>" & vbCr
  pColl.Clear
  tagName = agd.ColTitle(0)
  For row = 1 To agd.rows
    If agd.Selected(row, 0) Or Index = 1 Then
      tagVal = Trim(agd.TextMatrix(row, 0))
      If Len(tagVal) > 0 Then
        rowStr = "<" & tagName
        For col = 1 To agd.cols - 1
          attVal = Trim(agd.TextMatrix(row, col))
          If Len(attVal) > 0 Then rowStr = rowStr & " " & agd.ColTitle(col) & "=""" & attVal & """"
        Next
        xmlStr = xmlStr & rowStr & ">" & tagVal & "</" & tagName & ">" & vbCr
      End If
      
      Set tmpxml = New ChilkatXml
      tmpxml.LoadXml ReplaceString(xmlStr, "&", "&amp;")
      pColl.Add tmpxml
      Set tmpxml = Nothing
    End If
  Next
  'xmlStr = xmlStr & "</selected>"
  'xmlStr = ReplaceString(xmlStr, "&", "&amp;")

  'Set plst = New ChilkatXml
  'plst.LoadXml xmlStr
  
  Me.MousePointer = vbDefault
  Me.Hide
End Sub

Private Sub Form_Load()
  With agd
    .ColEditable(0) = True
    .ColSelectable(0) = True
    .ColType(0) = ATCoTxt
    '.SetFocus
  End With
  Set pColl = New FastCollection
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

Public Property Get coll() As FastCollection
  Set coll = pColl
End Property
Public Property Set coll(newValue As FastCollection)
  Dim row As Long
  Dim col As Long
  Dim vItem As Variant
  Dim lNode As ChilkatXml
  Dim iAttr As Long
  Dim AttrName As String
  Dim AttrValue As String
  Dim rowsToShow As Long
  Dim colTitles As New FastCollection
  
  Set pColl = newValue
  
  rowsToShow = pColl.Count + 1 'All in collection plus a row to enter user-specified value
  If rowsToShow > 1001 Then rowsToShow = 1001
  
  agd.FixedRows = 1
  agd.rows = 0
  agd.rows = rowsToShow
  agd.SelectionMode = ASdisjointByRow
  agd.ColTitle(0) = Me.Tag
  colTitles.Add 0, LCase(Me.Tag)
  
  If rowsToShow > 1 Then
    If VarType(pColl.ItemByIndex(1)) = vbObject Then
      For row = 1 To rowsToShow - 1
        Set lNode = pColl.ItemByIndex(row)
        agd.TextMatrix(row, 0) = lNode.content
              
        For iAttr = 0 To lNode.NumAttributes - 1
          AttrName = lNode.GetAttributeName(iAttr)
          AttrValue = ReplaceString(lNode.GetAttributeValue(iAttr), "&amp;", "&")
          col = colTitles.IndexFromKey(LCase(AttrName))
          If col = -1 Then
            col = agd.cols
            agd.cols = agd.cols + 1
            If agd.cols > col Then
              agd.ColTitle(col) = AttrName
              colTitles.Add col, LCase(AttrName)
            Else
              col = -1
            End If
          Else
            col = colTitles.ItemByIndex(col)
          End If
          If col >= 0 Then
            agd.TextMatrix(row, col) = AttrValue
          End If
        Next
      Next
    Else
      For row = 1 To rowsToShow - 1
        agd.TextMatrix(row, 0) = pColl.ItemByIndex(row)
      Next
    End If
    If rowsToShow <= pColl.Count Then
      agd.TextMatrix(rowsToShow, 0) = "(" & pColl.Count - rowsToShow + 1 & " rows could not be displayed)"
    End If
  End If
End Property

'Public Property Get lst() As ChilkatXml
'  Set lst = plst
'End Property
'Public Property Set lst(newValue As ChilkatXml)
'  Dim lNode As ChilkatXml
'  Dim row As Long
'  Dim col As Long
'  Dim iAttr As Long
'
'  Dim AttrName As String
'  Dim AttrValue As String
'
'  agd.FixedRows = 1
'  agd.rows = 0
'  agd.rows = 1
'  agd.SelectionMode = ASdisjointByRow
'  agd.ColTitle(0) = Me.Tag
'  Set plst = newValue
'  row = 0
'  If Not plst Is Nothing Then
'    Set lNode = plst.FirstChild
'    While Not lNode Is Nothing
'      row = row + 1
'      agd.TextMatrix(row, 0) = lNode.Content
'      For iAttr = 0 To lNode.NumAttributes - 1
'        AttrName = lNode.GetAttributeName(iAttr)
'        AttrValue = ReplaceString(lNode.GetAttributeValue(iAttr), "&amp;", "&")
'        For col = 1 To agd.cols - 1
'          If LCase(agd.ColTitle(col)) = LCase(AttrName) Then
'            agd.TextMatrix(row, col) = AttrValue
'            col = agd.cols + 2
'          End If
'        Next
'        If col = agd.cols Then 'did not find matching column
'          agd.ColTitle(col) = AttrName
'          agd.TextMatrix(row, col) = AttrValue
'        End If
'      Next
'      If lNode.NextSibling2 = 0 Then Set lNode = Nothing
'    Wend
'  End If
'  agd.rows = agd.rows + 1
'  agd.ColsSizeByContents
'End Property

