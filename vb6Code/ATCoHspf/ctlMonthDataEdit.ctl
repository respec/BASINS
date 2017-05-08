VERSION 5.00
Begin VB.UserControl ctlMonthDataEdit 
   ClientHeight    =   1590
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   7590
   ScaleHeight     =   1590
   ScaleWidth      =   7590
   Begin ATCoCtl.ATCoGrid grdMonthValues 
      Height          =   732
      Left            =   120
      TabIndex        =   2
      Top             =   600
      Width           =   7332
      _ExtentX        =   12938
      _ExtentY        =   1296
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
      Cols            =   12
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "Monthly Values"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483637
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.ComboBox cboID 
      Height          =   288
      Left            =   2640
      Style           =   2  'Dropdown List
      TabIndex        =   0
      Top             =   120
      Width           =   975
   End
   Begin VB.Label lblRefCount 
      Caption         =   "is referenced ? times."
      Height          =   252
      Left            =   3720
      TabIndex        =   3
      Top             =   120
      Width           =   3492
   End
   Begin VB.Label lblId 
      Alignment       =   1  'Right Justify
      Caption         =   "Month Data Table Number :"
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
      Top             =   120
      Width           =   2412
   End
End
Attribute VB_Name = "ctlMonthDataEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pMonthData As HspfMonthData, curId As Long
Dim Edited As Boolean, InRefresh As Boolean
Dim myMsg As ATCoMessage, myMsgTitle As String
Event Change()
Private pFrm As Form

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Property Set Owner(newOwner As HspfMonthData)
  Dim i&, months As Variant
   
  Set pMonthData = newOwner
  
  Set myMsg = New ATCoMessage
  Set myMsg.icon = pMonthData.Uci.icon
  myMsgTitle = "Month Data Edit"
  
  months = Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec")
  With grdMonthValues
    For i = 0 To 11
      .ColEditable(i) = True
      .ColType(i) = ATCoSng
      .ColTitle(i) = months(i)
    Next i
  End With
  
  Refresh
  
End Property

Private Function CurrentMonthDataTableIndex() As Long
  Dim j&
  
  CurrentMonthDataTableIndex = 1
  With pMonthData
    For j = 1 To .MonthDataTables.Count
      If .MonthDataTables(j).Id = cboID Then
        CurrentMonthDataTableIndex = j
        Exit For
      End If
    Next j
  End With
End Function
Private Function CurrentMonthDataTable() As HspfMonthDataTable
  If pMonthData.MonthDataTables.Count > 0 Then
    Set CurrentMonthDataTable = pMonthData.MonthDataTables(CurrentMonthDataTableIndex)
  End If
End Function

Private Sub Refresh()
  Dim i&
  
  InRefresh = True
  
  If cboID.ListCount > 0 Then
    curId = CLng(cboID)
  Else
    curId = 1
  End If
  cboID.Clear
  For i = 1 To pMonthData.MonthDataTables.Count
    cboID.AddItem pMonthData.MonthDataTables(i).Id
    If curId = pMonthData.MonthDataTables(i).Id Then
      cboID.ListIndex = i - 1
    End If
  Next i
  
  If pMonthData.MonthDataTables.Count > 0 Then
    With CurrentMonthDataTable
      lblRefCount = "is referenced " & .ReferencedBy.Count & " times"
      For i = 0 To 11
        grdMonthValues.TextMatrix(1, i) = .MonthValue(i + 1)
      Next i
    End With
  End If
  
  Edited = False
  InRefresh = False
  
End Sub

Private Sub cboID_Click()
  Dim discard&
  
  If Not (InRefresh) Then
    If CLng(cboID) <> curId Then 'need to change table
      If Edited Then
        discard = MsgBox("Changes to current Month Data Table have not been saved. Discard them?", vbYesNo)
      Else
        discard = vbYes 'no changes
      End If
      If discard = vbYes Then
        Refresh
      Else
        cboID = curId
      End If
    End If
  End If
End Sub

Private Sub grdMonthValues_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  If Not (InRefresh) Then
    Edited = True
    RaiseEvent Change
  End If
End Sub

Public Sub Save()
  Dim i&
  
  With CurrentMonthDataTable
    For i = 1 To 12
      .MonthValue(i) = grdMonthValues.TextMatrix(1, i - 1)
    Next i
  End With
End Sub

Public Sub Remove()
  Dim j&
  
  If pMonthData.MonthDataTables.Count > 0 Then
    j = CurrentMonthDataTableIndex
    With pMonthData.MonthDataTables(j)
      If .ReferencedBy.Count > 0 Then
        MsgBox "Can not Remove table referenced by an Operation"
      Else
        pMonthData.MonthDataTables.Remove (j)
        curId = 1
        Refresh
      End If
    End With
  Else
    MsgBox "No Month-Data tables are present to remove", vbOKOnly, "Remove Problem"
  End If
End Sub

Public Sub Help()
  'just do contents for now
  HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Public Sub Add()
  MsgBox "Add ADD here", vbOKOnly, myMsgTitle
End Sub
