VERSION 5.00
Begin VB.UserControl ATCTimserGrid 
   ClientHeight    =   2190
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   4935
   ScaleHeight     =   2190
   ScaleWidth      =   4935
   Begin VB.Frame fraAllNone 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   252
      Left            =   3600
      TabIndex        =   2
      Top             =   20
      Width           =   1332
      Begin VB.CommandButton cmdAllNone 
         Caption         =   "All"
         Enabled         =   0   'False
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
         Left            =   0
         TabIndex        =   3
         ToolTipText     =   "Select all items in list"
         Top             =   0
         Width           =   612
      End
      Begin VB.CommandButton cmdAllNone 
         Caption         =   "None"
         Enabled         =   0   'False
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
         Left            =   720
         TabIndex        =   4
         ToolTipText     =   "Deselect all items in list"
         Top             =   0
         Width           =   612
      End
   End
   Begin ATCoCtl.ATCoGrid grdDsn 
      Height          =   1812
      Left            =   0
      TabIndex        =   0
      Top             =   360
      Visible         =   0   'False
      Width           =   4452
      _ExtentX        =   7858
      _ExtentY        =   3201
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
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
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483648
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin MSComctlLib.Toolbar tbrTimser 
      Height          =   312
      Left            =   36
      TabIndex        =   1
      Top             =   0
      Width           =   4416
      _ExtentX        =   7779
      _ExtentY        =   556
      ButtonWidth     =   609
      ButtonHeight    =   582
      ImageList       =   "imgListTools"
      _Version        =   393216
      BeginProperty Buttons {66833FE8-8583-11D1-B16A-00C0F0283628} 
         NumButtons      =   18
         BeginProperty Button1 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Add"
            Object.ToolTipText     =   "Add to Time-Series List"
         EndProperty
         BeginProperty Button2 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Remove"
            Object.ToolTipText     =   "Remove from Time-Series List"
         EndProperty
         BeginProperty Button3 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Clear"
            Object.ToolTipText     =   "Clear Time-Series List"
         EndProperty
         BeginProperty Button4 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Separator1"
            Style           =   3
            MixedState      =   -1  'True
         EndProperty
         BeginProperty Button5 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Top"
            Object.ToolTipText     =   "Move Time-Series to Top of List"
         EndProperty
         BeginProperty Button6 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Up"
            Object.ToolTipText     =   "Move Time-Series Item Up in List"
         EndProperty
         BeginProperty Button7 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Down"
            Object.ToolTipText     =   "Move Time-Series Down in List"
         EndProperty
         BeginProperty Button8 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Separator2"
            Style           =   3
            MixedState      =   -1  'True
         EndProperty
         BeginProperty Button9 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Headers"
            Object.ToolTipText     =   "Modify Column Headers"
         EndProperty
         BeginProperty Button10 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Separator3"
            Style           =   3
            MixedState      =   -1  'True
         EndProperty
         BeginProperty Button11 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Save"
            Object.ToolTipText     =   "Save Time-Series List to File"
         EndProperty
         BeginProperty Button12 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Get"
            Object.ToolTipText     =   "Get Time-Series List from File"
         EndProperty
         BeginProperty Button13 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Separator4"
            Style           =   3
            MixedState      =   -1  'True
         EndProperty
         BeginProperty Button14 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Properties"
            Object.ToolTipText     =   "Edit Properties of Selected Time Series "
         EndProperty
         BeginProperty Button15 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Style           =   3
            MixedState      =   -1  'True
         EndProperty
         BeginProperty Button16 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Key             =   "Trash"
            Object.ToolTipText     =   "Delete Time Series from disk"
         EndProperty
         BeginProperty Button17 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Object.Visible         =   0   'False
            Key             =   "SaveTS"
            Object.ToolTipText     =   "Save Time Series"
         EndProperty
         BeginProperty Button18 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Enabled         =   0   'False
            Object.Visible         =   0   'False
            Key             =   "New"
            Object.ToolTipText     =   "Create New Time Series"
         EndProperty
      EndProperty
   End
   Begin MSComDlg.CommonDialog cdlTSFile 
      Left            =   3360
      Top             =   120
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   4.873e-37
   End
   Begin MSComctlLib.ImageList imgListTools 
      Left            =   0
      Top             =   0
      _ExtentX        =   794
      _ExtentY        =   794
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   8421376
      _Version        =   393216
      BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
         NumListImages   =   25
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":0000
            Key             =   "Zoom"
         EndProperty
         BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":0552
            Key             =   "ZoomIn"
         EndProperty
         BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":0AA4
            Key             =   "ZoomOut"
         EndProperty
         BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":0FF6
            Key             =   "Pan"
         EndProperty
         BeginProperty ListImage5 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":1548
            Key             =   "FullExtent"
         EndProperty
         BeginProperty ListImage6 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":1A9A
            Key             =   "Identify"
         EndProperty
         BeginProperty ListImage7 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":1FEC
            Key             =   "Left"
         EndProperty
         BeginProperty ListImage8 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":2306
            Key             =   "Right"
         EndProperty
         BeginProperty ListImage9 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":2620
            Key             =   "Select"
         EndProperty
         BeginProperty ListImage10 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":293A
            Key             =   "Unselect"
         EndProperty
         BeginProperty ListImage11 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":2C54
            Key             =   "Get"
         EndProperty
         BeginProperty ListImage12 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":2F6E
            Key             =   "Save"
         EndProperty
         BeginProperty ListImage13 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":3288
            Key             =   "Print"
         EndProperty
         BeginProperty ListImage14 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":35A2
            Key             =   "Add"
         EndProperty
         BeginProperty ListImage15 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":38BC
            Key             =   "Remove"
         EndProperty
         BeginProperty ListImage16 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":3BD6
            Key             =   "Properties"
         EndProperty
         BeginProperty ListImage17 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":3EF0
            Key             =   "Clear"
         EndProperty
         BeginProperty ListImage18 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":420A
            Key             =   "Up"
         EndProperty
         BeginProperty ListImage19 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":4524
            Key             =   "Down"
         EndProperty
         BeginProperty ListImage20 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":483E
            Key             =   "Headers"
         EndProperty
         BeginProperty ListImage21 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":4B58
            Key             =   "Move"
         EndProperty
         BeginProperty ListImage22 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":4E72
            Key             =   "All"
         EndProperty
         BeginProperty ListImage23 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":518C
            Key             =   "None"
         EndProperty
         BeginProperty ListImage24 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":54A6
            Key             =   "Top"
         EndProperty
         BeginProperty ListImage25 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "ATCTimserGrid.ctx":57C0
            Key             =   "Trash"
         EndProperty
      EndProperty
   End
End
Attribute VB_Name = "ATCTimserGrid"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private pAvailAttributes As Collection
Private pVisibleAttributes As Collection
Private pVisibleList As Collection
Private pWholeList As Collection
Private pOpenFiles As Collection
Private pCriteria As Collection
Private WithEvents TSnew As frmTSnew
Attribute TSnew.VB_VarHelpID = -1

Public Event Add()       'User pressed + button
Public Event Change()    'Visible list changes
Public Event SelChange() 'SelectedList changes
Public Event Edit()      'Deleted, created, or edited data or attributes

Public Sub RaiseChange()
  RaiseEvent Change
End Sub

Public Sub RaiseEdit()
  PopulateGrid
  RaiseEvent Edit
End Sub

Public Property Get OpenFiles() As Collection
  Set OpenFiles = pOpenFiles
End Property
Public Property Set OpenFiles(newValue As Collection)
  Set pOpenFiles = Nothing
  Set pOpenFiles = newValue
End Property

'Public Property Get NtsCol() As Long
'  NtsCol = grdDsn.cols - 1
'End Property
'Public Property Let NtsCol(newValue As Long)
'  grdDsn.cols = newValue + 1
'  'ReDim Preserve pTSColID(newvalue)
'  grdDsn.ColTitle(newValue) = "HIDE" 'last col for collection sorting index
'End Property

'Public Property Get TSColName(Index&) As String
'  TSColName = grdDsn.ColTitle(Index)
'End Property
'Public Property Let TSColName(Index&, newValue As String)
'  grdDsn.ColTitle(Index) = newValue
'  If Index >= NtsCol Then NtsCol = Index
'End Property

Public Property Get Enabled() As Boolean
  Enabled = tbrTimser.Buttons(1).Enabled
End Property
Public Property Let Enabled(ByVal newValue As Boolean)
  Dim i&
  For i = 1 To tbrTimser.Buttons.Count
    tbrTimser.Buttons(i).Enabled = newValue
  Next i
  cmdAllNone(0).Enabled = newValue
  cmdAllNone(1).Enabled = newValue
  grdDsn.Visible = newValue
End Property

'buttonID can be the name of a button "Pan" or the 1-based index
Public Property Get ButtonVisible(ByVal buttonID As Variant) As Boolean
  On Error GoTo NoSuchButton
  ButtonVisible = tbrTimser.Buttons(buttonID).Visible
  Exit Property
NoSuchButton:
  ButtonVisible = False
End Property

Public Property Let ButtonVisible(ByVal buttonID As Variant, ByVal state As Boolean)
  'DbgMsg "Let ButtonVisible " & buttonID & " = " & state
  On Error GoTo NoSuchButton
  tbrTimser.Buttons(buttonID).Visible = state
  With tbrTimser.Buttons(tbrTimser.Buttons.Count)
    If .Left + .Width > tbrTimser.Width Then tbrTimser.Width = .Left + .Width
  End With
  Exit Property
NoSuchButton:
End Property

Public Property Get GridVisible() As Boolean
  GridVisible = grdDsn.Visible
End Property
Public Property Let GridVisible(ByVal newValue As Boolean)
  grdDsn.Visible = newValue
End Property

Public Function FirstSelected() As ATCclsTserData
  Dim row&
  For row = grdDsn.SelStartRow To grdDsn.SelEndRow
    If grdDsn.Selected(row, 0) Then
      Set FirstSelected = pVisibleList(row)
      Exit Function '**********************
    End If
  Next
End Function

Public Function IsSelected(ts As ATCclsTserData) As Boolean
  Dim row&, startRow&, endRow&
  Dim newColl As Collection
  Set newColl = New Collection
  startRow = grdDsn.SelStartRow
  endRow = grdDsn.SelEndRow
  For row = startRow To endRow
    If grdDsn.Selected(row, 0) Then
      If pVisibleList(row).Serial = ts.Serial Then
        IsSelected = True
        Exit Function
      End If
    End If
  Next
  IsSelected = False
End Function

Public Property Get SelectedList() As Collection
  Dim row&, startRow&, endRow&
  Dim newColl As Collection
  Set newColl = New Collection
  startRow = grdDsn.SelStartRow
  endRow = grdDsn.SelEndRow
  If endRow > pVisibleList.Count Then endRow = pVisibleList.Count
  For row = startRow To endRow
    If grdDsn.Selected(row, 0) Then newColl.Add pVisibleList(row)
  Next
  Set SelectedList = newColl
End Property

Public Property Let SelectedList(SelectedTSer As Collection)
  Dim row&, tsIndex&, SetSelected As Boolean
  For row = 1 To grdDsn.Rows
    SetSelected = False
    For tsIndex = 1 To SelectedTSer.Count
      If SelectedTSer(tsIndex).Serial = pVisibleList(row).Serial Then
        SetSelected = True
        Exit For
      End If
    Next tsIndex
    grdDsn.Selected(row, 0) = SetSelected
  Next row
End Property

Public Sub SelectRow(row&, SetSelected As Boolean)
  grdDsn.Selected(row, 0) = SetSelected
End Sub

Public Function IsVisible(ts As ATCclsTserData) As Boolean
  Dim vTs As Variant
  For Each vTs In pVisibleList
    If vTs.Serial = ts.Serial Then
      IsVisible = True
      Exit Function
    End If
  Next
  IsVisible = False
End Function

Public Property Get AvailAttributes() As Collection
  Set AvailAttributes = pAvailAttributes
End Property
Public Property Set AvailAttributes(newValue As Collection)
  Set pAvailAttributes = Nothing
  Set pAvailAttributes = newValue
End Property

Public Property Get VisibleAttributes() As Collection
  Set VisibleAttributes = pVisibleAttributes
End Property
Public Property Set VisibleAttributes(newValue As Collection)
  Set pVisibleAttributes = Nothing
  Set pVisibleAttributes = newValue
End Property

Public Property Get VisibleList() As Collection
  Set VisibleList = pVisibleList
End Property

Public Property Get WholeList() As Collection
  Set WholeList = pWholeList
End Property
Public Property Set WholeList(newValue As Collection)
  Set pWholeList = Nothing
  Set pWholeList = newValue
  'ClearVisible
  
  'Remove visible items that are no longer in WholeList
  Dim v As Long, vTs As Variant, wTs As Variant, NeedToPopulateGrid As Boolean
  NeedToPopulateGrid = False
  v = 1
  While v <= pVisibleList.Count
    Set vTs = pVisibleList(v)
    For Each wTs In pWholeList
      If vTs.Serial = wTs.Serial Then v = v + 1: GoTo NextTS
    Next
    pVisibleList.Remove v
    NeedToPopulateGrid = True
NextTS:
  Wend
  If NeedToPopulateGrid Then PopulateGrid
End Property

Public Sub WholeListAdd(newValue As ATCclsTserData)
  pWholeList.Add newValue
End Sub
Public Sub WholeListAddFile(newValue As ATCclsTserFile)
  Dim i&
  For i = 1 To newValue.DataCount
    pWholeList.Add newValue.Data(i)
  Next
End Sub

Public Sub AddRule(newCrit As ATCclsCriterion)
  pCriteria.Add newCrit
End Sub
Public Sub ClearRules()
  Set pCriteria = Nothing
  Set pCriteria = New Collection
End Sub

Public Sub AddTimeseriesMatchingCurrentRules()
  Dim vTs As Variant, vmTs As Variant
  Dim vCrit As Variant
  Dim ts As ATCclsTserData, mTs As ATCclsTserData
  
  'Set pVisibleList = Nothing
  'Set pVisibleList = New Collection
  
  For Each vTs In pWholeList
    Set ts = vTs
    'If Not IsVisible(ts) Then
      For Each vCrit In pCriteria
        If Not vCrit.MatchField(ts.Header) Then GoTo NextTS
      Next
      'is this matching timeseries already in list?
      For Each vmTs In pVisibleList
        Set mTs = vmTs
        If mTs.Serial = ts.Serial Then GoTo NextTS 'have it in buffer already
'        If mTs.Compare(ts) Then GoTo NextTS 'have it in buffer already
      Next vmTs
      pVisibleList.Add vTs
    'End If
NextTS:
  Next
  
  PopulateGrid
  
End Sub

Private Sub cmdAllNone_Click(index As Integer)
  Dim sel As Boolean, row As Long
  If index = 0 Then sel = True Else sel = False
  With grdDsn
    For row = 1 To .Rows
      .Selected(row, 0) = sel
    Next
  End With
End Sub

Private Sub grdDsn_DoubleClick(row&, col&)
  ManageTimeSeriesList 9, "Properties"
End Sub

Private Sub grdDsn_SelChange(row As Long, col As Long)
  If col = 0 Then RaiseEvent SelChange
End Sub

Private Sub grdDsn_Sorted(col As Long, Ascending As Boolean)
  ResyncTSerToList
End Sub

Private Sub tbrTimser_ButtonClick(ByVal Butn As MSComctlLib.Button)
  Dim i%

  DbgMsg "tbrTimser:Click:" & Butn.Key, 3, "TimserGrid", "m"
  Select Case Butn.Key
    Case "Add":        i = 0
    Case "Remove":     i = 1
    Case "Clear":      i = 2
    Case "Top":        i = 3
    Case "Up":         i = 4
    Case "Down":       i = 5
    Case "Headers":    i = 6
    Case "Save":       i = 7
    Case "Get":        i = 8
    Case "Properties": i = 9
    Case "Trash":      i = 11
    Case "SaveTS":     i = 12
    Case "New":        i = 13
  End Select
  Call ManageTimeSeriesList(i, Butn.Key)

End Sub

Private Sub UserControl_InitProperties()
  grdDsn.Rows = 0
End Sub

Private Sub UserControl_Resize()
  grdDsn.Width = Width
  tbrTimser.Width = Width
  If Height > grdDsn.Top Then grdDsn.Height = Height - grdDsn.Top
  fraAllNone.Left = Width - fraAllNone.Width
End Sub

Private Sub UserControl_Initialize()
  Dim butt&, col&
  Set pAvailAttributes = New Collection
  Set pVisibleAttributes = New Collection
  Set pVisibleList = New Collection
  Set pWholeList = New Collection
  Set pCriteria = New Collection
  ClearVisible
  On Error Resume Next 'Separator buttons don't have images
  For butt = 1 To tbrTimser.Buttons.Count
    With tbrTimser.Buttons.item(butt)
      If .Image = 0 And .Style <> tbrSeparator Then .Image = .Key
    End With
  Next
  ReDim pTSColID(10)
  'init available column names and widths
  'default thru headers file name, collection index and key
End Sub

Private Sub ClearVisible()
  Set pVisibleList = Nothing
  Set pVisibleList = New Collection
  grdDsn.Rows = 0
End Sub

Private Sub PopulateColTitles()
  Dim col&, maxCol&
  If pVisibleAttributes.Count < 1 Then
    If pAvailAttributes.Count < 1 Then
      Set pAvailAttributes = uniqueAttributeNames(pVisibleList)
    End If
    maxCol = 10
    If maxCol > AvailAttributes.Count Then maxCol = pAvailAttributes.Count
    For col = 1 To maxCol
      pVisibleAttributes.Add pAvailAttributes(col)
    Next
  End If
  For col = 0 To pVisibleAttributes.Count - 1
    grdDsn.ColTitle(col) = pVisibleAttributes(col + 1)
  Next
  grdDsn.ColTitle(col) = "HIDE" 'last col for collection sorting index
  grdDsn.cols = col + 1
End Sub

Private Property Get IndexText(row&) As String
  IndexText = grdDsn.TextMatrix(row, grdDsn.cols - 1)
End Property
Private Property Let IndexText(row&, newValue$)
  grdDsn.TextMatrix(row, grdDsn.cols - 1) = newValue
End Property

'Call after changes in grdDsn such as removing rows or sorting
Private Sub ResyncTSerToList()
  Dim row&, ts As ATCclsTserData
  Dim newList As Collection
  
  Set newList = New Collection
  
  MousePointer = vbHourglass
  
  For row = 1 To grdDsn.Rows
    Set ts = pVisibleList(CInt(IndexText(row)))
    newList.Add ts
    IndexText(row) = row
  Next row
  
  Set pVisibleList = Nothing
  Set pVisibleList = newList

  MousePointer = vbDefault

End Sub

Public Sub NewVisibleAttributes()
  PopulateGrid
End Sub

Public Sub PopulateGrid()
  Dim i&, col&, lstr$, Key$, srt%, d&(6)
  Dim r&, lAtr$
  Dim SaveSelected As Collection
  
  Set SaveSelected = Me.SelectedList
  
  grdDsn.Visible = False 'lstdsn.visible = false
  PopulateColTitles
  grdDsn.Rows = 0
  For i = 1 To pVisibleList.Count
    With pVisibleList(i)
      For col = 0 To grdDsn.cols - 2
        lAtr = VisibleAttributes(col + 1)
        lstr = .Attrib(lAtr)
        Select Case lAtr
          Case "DATCRE", "DATMOD"
            If Len(lstr) > 12 Then 'format date
              lstr = Left(lstr, 4) & "/" & Mid(lstr, 5, 2) & "/" & Mid(lstr, 7, 2) & " " & _
                     Mid(lstr, 9, 2) & ":" & Mid(lstr, 11, 2)
            End If
          Case "Start", "End"
            If Left(lstr, 7) = "1858/11" Then lstr = "-"
        End Select
        grdDsn.TextMatrix(i, col) = lstr
      Next col
    End With
    grdDsn.TextMatrix(i, grdDsn.cols - 1) = i ' key
  Next i
  grdDsn.ColsSizeByContents
  grdDsn.Visible = True 'LstDsn.Visible = True

  Me.SelectedList = SaveSelected
End Sub

'Index =
'0 add
'1 remove
'2 clear
'3 top
'4 up
'5 down
'6 edit columns
'7 save
'8 get
'9 edit time series
'10 interpolate time series
'11 delete time series from file
'12 save time series to file
'13 new time series
Public Sub ManageTimeSeriesList(index%, s$)
  Dim Msg$
  Dim i&, iMax&, istr$, j&, jmax&, jStr$, k&, kmax&, kstr$
  Dim l&, ll&, lid&, fu&, lstr$, r&
  Dim lts As Collection, lnts&
  Dim TSBFile$, tempsen$, temploc$, tempcon$
  Dim vTs As Variant, ts As ATCclsTserData
  Dim oldPos&
  Dim oldSelected As Collection
  Set oldSelected = New Collection
  Dim vItem As Variant
  Dim nInMemory As Long
  Set lts = New Collection
  
  DbgMsg "TSLstMan:" & s, 3, "ATCData", "i"
  Select Case index
  Case 0 'add
    RaiseEvent Add
  Case 1 'remove
    On Error GoTo errx
    grdDsn.DeleteRows
    ResyncTSerToList
    If k > -1 Then 'select at pos of first removed
      If k > pVisibleList.Count Then
        k = k - 1 'del last line, back up one
      End If
      If k > 0 Then grdDsn.Selected(k, 0) = True
      RaiseEvent Change 'Call GetCommonDates
    Else
      Msg = "No time series are selected to remove."
      MsgBox Msg, vbExclamation, "Timeseries List Remove Problem"
      DbgMsg "TSLstMan:RemoveProblem:" & Msg, 4, "TimserGrid", "e"
    End If
  Case 2 'clear
    If grdDsn.Rows > 0 Then ' LstDsn.ListItems.Count > 0 Then
      i = ListFindFirstSel(grdDsn)  'lstdsn)
      If i > 0 Then 'some
        DbgMsg "TSLstMan:ClearFrom: " & i & " " & pVisibleList(i).Header.id, 6, "TimserGrid", "t"
        grdDsn.Rows = i - 1
      Else 'all
        DbgMsg "TSLstMan:ClearAll", 6, "TimserGrid", "t"
        grdDsn.Rows = 0
      End If
      ResyncTSerToList
      RaiseEvent Change 'Call GetCommonDates
    End If
  Case 3 'top
    If grdDsn.Rows > 1 Then
      k = 0 'number moved
      Set lts = Nothing
      Set lts = New Collection
      For i = 1 To grdDsn.Rows
        If grdDsn.Selected(i, 0) Then
          k = k + 1
          lts.Add pVisibleList(i)
        End If
      Next i
      For i = 1 To grdDsn.Rows
        If grdDsn.Selected(i, 0) Then
          grdDsn.Selected(i, 0) = False
        Else
          lts.Add pVisibleList(i)
        End If
      Next i
      If k > 0 Then
        Set pVisibleList = Nothing
        Set pVisibleList = lts
        PopulateGrid
        RaiseEvent Change
        grdDsn.Selected(k + 1, 0) = True
      Else
        Msg = "Select a time series to Move to Top."
        MsgBox Msg, vbExclamation, "Time Series MoveTop Problem"
      End If
    End If
  Case 4 'up
    If grdDsn.Rows > 1 Then
      If SelectedList.Count < 1 And grdDsn.row > 0 Then grdDsn.Selected(grdDsn.row, 0) = True
      i = ListFindFirstSel(grdDsn)
      If i > 0 Then
        DbgMsg "TSLstMan:MoveUp: " & i & " " & pVisibleList(i).Header.id, 6, "TimserGrid", "t"
        If i > 1 Then
          Set lts = Nothing
          Set lts = New Collection
          For j = 1 To i - 2
            lts.Add pVisibleList(j)
          Next j
          lts.Add pVisibleList(i)
          lts.Add pVisibleList(i - 1)
          For j = i + 1 To pVisibleList.Count
            lts.Add pVisibleList(j)
            grdDsn.Selected(j, 0) = False
          Next j
          Set pVisibleList = Nothing
          Set pVisibleList = lts
          PopulateGrid
          RaiseEvent Change
          grdDsn.Selected(i, 0) = False
          grdDsn.Selected(i - 1, 0) = True
        End If
      Else
        Msg = "Select a time series to Move Up."
        MsgBox Msg, vbExclamation, "Time Series MoveUp Problem"
      End If
    End If
  Case 5 'down
    If grdDsn.Rows > 1 Then
      If SelectedList.Count < 1 And grdDsn.row > 0 Then grdDsn.Selected(grdDsn.row, 0) = True
      i = ListFindFirstSel(grdDsn)
      If i > 0 Then
        DbgMsg "TSLstMan:MoveDown: " & i & " " & pVisibleList(i).Header.id, 6, "TimserGrid", "t"
        If i > 0 And i < pVisibleList.Count Then
          Set lts = Nothing
          Set lts = New Collection
          For j = 1 To i - 1
            lts.Add pVisibleList(j)
          Next j
          lts.Add pVisibleList(i + 1)
          lts.Add pVisibleList(i)
          For j = i + 2 To pVisibleList.Count
            lts.Add pVisibleList(j)
            grdDsn.Selected(j, 0) = False
          Next j
          Set pVisibleList = Nothing
          Set pVisibleList = lts
          ResyncTSerToList
          PopulateGrid
          RaiseEvent Change 'Call GetCommonDates
          grdDsn.Selected(i, 0) = False
          grdDsn.Selected(i + 1, 0) = True
        End If
      Else
        Msg = "Select a time series to Move Down."
        MsgBox Msg, vbExclamation, "Time Series MoveDown Problem"
      End If
    End If
  Case 6 'columns
    DbgMsg "TSLstMan:ColumnsEdit:", 6, "TimserGrid", "t"
    If pVisibleList.Count < 1 Then
      MsgBox "Must have at least one time series to edit columns.", vbOKOnly, "Time Series Columns"
    Else
      Set pAvailAttributes = Nothing
      Set pAvailAttributes = uniqueAttributeNames(pVisibleList) 'make attributes available as col headers
      Set TSCol.Grid = Me
      TSCol.Show
    End If
  Case 7, 8   'save,get
    On Error GoTo ErrHandTS
    cdlTSFile.CancelError = True
    cdlTSFile.filter = "Time Series List files (*.tsb)|*.tsb"
    cdlTSFile.Filename = "*.tsb"
    fu = FreeFile(0)
    If index = 7 Then 'save
      cdlTSFile.DialogTitle = "Time Series List Save"
      cdlTSFile.flags = &H3804& 'create & not read only
      cdlTSFile.ShowSave
      DbgMsg "TSLstMan:SaveTo:" & cdlTSFile.Filename, 6, "TimserGrid", "t"
      TSBFile = cdlTSFile.Filename
      Open TSBFile For Output As #fu
      For i = 1 To pVisibleList.Count
        With pVisibleList(i).Header
          Write #fu, .sen, .loc, .con
        End With
      Next i
      Close #fu
    Else 'get
      cdlTSFile.DialogTitle = "Time Series List Get"
      cdlTSFile.flags = &H1000& 'file must exist
      cdlTSFile.ShowOpen
      DbgMsg "TSLstMan:GetFrom:" & cdlTSFile.Filename, 3, "TimserGrid", "t"
      TSBFile = cdlTSFile.Filename
      Open TSBFile For Input As #fu
      'If we didn't have an error opening the file, go ahead and clear the visible list
      Set pVisibleList = Nothing
      Set pVisibleList = New Collection
      While Not EOF(fu)
        Input #fu, tempsen, temploc, tempcon
        For Each vTs In pWholeList
          Set ts = vTs
          With ts.Header
            If .sen = tempsen Then
              If .loc = temploc Then
                If .con = tempcon Then
                  pVisibleList.Add ts
                  Exit For 'Or should we allow for more than one time series with same S,L,C?
                End If
              End If
            End If
          End With
        Next
      Wend
      Close #fu
      PopulateGrid
      RaiseEvent Change
    End If
  
ErrHandTS:
    Close #fu

  Case 9 'Edit attributes
    If SelectedList.Count < 1 And grdDsn.row > 0 Then grdDsn.Selected(grdDsn.row, 0) = True
    If SelectedList.Count > 0 Then
      Set TSEdit.TimeseriesToEdit = SelectedList
      Set TSEdit.Notify = Me
      TSEdit.Show
    Else
      MsgBox "No time series are selected to edit.", vbExclamation, "Time Series Edit Problem"
    End If
  Case 10: InterpolateTS
  Case 11  'Delete
    If SelectedList.Count < 1 And grdDsn.row > 0 Then grdDsn.Selected(grdDsn.row, 0) = True
    Set lts = SelectedList
    If lts.Count > 0 Then

      'Silently delete in-memory time series, but confirm deleting time series on disk
      nInMemory = 0
      For i = 1 To lts.Count
        If lts(i).File.label = "In-Memory" Then nInMemory = nInMemory + 1
      Next
      If nInMemory < lts.Count Then
        Msg = "Permanently delete " & lts.Count - nInMemory & " time series from disk?"
        If MsgBox(Msg, vbYesNo, "Delete Data") = vbNo Then GoTo DontDelete
      End If

      For i = 1 To lts.Count
RemoveTs:
        If Not lts(i).File.RemoveTimSer(lts(i)) Then
          Select Case MsgBox(lts(i).File.ErrorDescription, vbAbortRetryIgnore, "Delete Timeseries")
            Case vbAbort: Exit For
            Case vbRetry: GoTo RemoveTs
            Case vbIgnore:
          End Select
        End If
      Next i
      'SelectedList = New Collection
      RaiseEvent Edit
    Else
      MsgBox "Must select at least one time series to delete.", vbExclamation, "Delete Time Series"
    End If
DontDelete:
  Case 12 'SaveTS
    If SelectedList.Count < 1 Then
      MsgBox "No time series were selected to Save", vbOKOnly, "Save Time Series"
    Else
      SaveSelectedTS
    End If
  Case 13 'New
    CreateNewTS
  End Select
  'lstDsn_GotFocus
  If pVisibleList.Count > 0 Then
    DbgMsg "TSLstMan:TS list size now " & pVisibleList.Count, 4, "TimserGrid", "t"
  End If
  
  i = ListFindFirstSel(grdDsn)
  If i > 0 Then
    If Not (grdDsn.RowIsVisible(i)) Then 'make it visible
      grdDsn.TopRow = i
    End If
  End If
  
  'UpdateLblDsn
  
  Exit Sub

errx:
  If err.Number = 91 Then
    Msg = "No time series are available to remove."
    MsgBox Msg, vbExclamation, "Time Series Remove Problem"
    DbgMsg "TSLstMan:RemoveProblem:" & Msg, 4, "TimserGrid", "e"
  Else
    Msg = "Error Number " & err.Number & ":" & err.Description
    MsgBox Msg, vbExclamation, "Time Series List Problem"
    DbgMsg "TSLstMan:Problem:" & Msg, 4, "TimserGrid", "e"
  End If
  
End Sub

Private Sub CreateNewTS()
  'Dim OpenFiles As New Collection
  'Dim v As Variant, tsf As ATCclsTserFile
  Dim row&
  
  Set TSnew = New frmTSnew
      
  'On Error Resume Next 'Adding same file many times will cause many errors
  'For Each v In pWholeList
  '  Set tsf = v.File
  '  OpenFiles.Add tsf, tsf.filename
  'Next
    
  Set TSnew.OpenFiles = OpenFiles
  Set TSnew.AllTSer = Me.VisibleList
  
  For row = grdDsn.SelStartRow To grdDsn.SelEndRow
    If grdDsn.Selected(row, 0) Then
      TSnew.SetDefaultTS row
      Exit For
    End If
  Next
  
  TSnew.Show
  
End Sub

Private Sub InterpolateTS()
  Dim i&
  MousePointer = vbHourglass
  DbgMsg "TSLstMan:Interpolate", 6, "TimserGrid", "t"
  If SelectedList.Count < 1 And grdDsn.row > 0 Then grdDsn.Selected(grdDsn.row, 0) = True
  Dim useDates As Collection
  Dim commonDates As ATCclsTserDate
  Dim lts As Collection
  Dim lTser As ATCclsTserData
  Set useDates = New Collection
  
  Set lts = SelectedList
  For i = 1 To lts.Count
    useDates.Add lts(i).dates
  Next i
  
  If useDates.Count > 0 Then
    DbgMsg "TSLstMan:Interpolate using" & useDates.Count & " sets of dates", 7, "TimserGrid", "t"
    If useDates.Count = 1 Then
      Set commonDates = useDates(1)
    Else
      Set commonDates = useDates(1).GetCommonDates(useDates)
    End If
    For i = 1 To pVisibleList.Count
      If pVisibleList(i).dates.Serial <> commonDates.Serial Then
        Set lTser = pVisibleList(i).Interpolate(commonDates)
        Set lTser.File = Nothing
        TSnew_CreatedTser lTser
      End If
    Next i
  Else
    MsgBox "Must select at least one time series to provide times for interpolation", vbExclamation, "Interpolation Problem"
  End If
  MousePointer = vbDefault
End Sub

Private Sub SaveSelectedTS()
  'Dim OpenFiles As New Collection, tsf As ATCclsTserFile
  'Dim v As Variant
  Dim tSave As New frmTSsave
  
  'On Error Resume Next
  'For Each v In pWholeList
  '  Set tsf = v.File
  '  OpenFiles.Add tsf, tsf.filename
  'Next
  
  Set tSave.OpenFiles = OpenFiles
  Set tSave.Tsers = Me.SelectedList
  tSave.Show vbModal
  'For Each v In Me.SelectedList
  '  Set tSave.Tser = v
  '  tSave.Show vbModal
  'Next
  Unload tSave
  RaiseEvent Edit
End Sub

Private Sub TSnew_CreatedTser(newTS As ATCData.ATCclsTserData)
  If newTS.File Is Nothing Then
    Set newTS.File = InMemFile
    InMemFile.addtimser newTS, TsIdRenum
  End If
  pVisibleList.Add newTS
  PopulateGrid
  RaiseEvent Edit
End Sub

Public Property Let TopRow(ByVal newValue&)
  grdDsn.TopRow = newValue
End Property
Public Property Get TopRow() As Long
  TopRow = grdDsn.TopRow
End Property

Public Property Get RowBackColor(row&) As Long
  Dim oldRow&
  oldRow = grdDsn.row
  grdDsn.row = row
  RowBackColor = grdDsn.CellBackColor
  grdDsn.row = oldRow
End Property
Public Property Let RowBackColor(row&, BackColor As Long)
  Dim oldRow&, col&
  If Not grdDsn.Selected(row, 0) Then
    oldRow = grdDsn.row
    grdDsn.row = row
    For col = 0 To grdDsn.cols - 1
      grdDsn.col = col
      grdDsn.CellBackColor = BackColor
      grdDsn.CellForeColor = vbBlack
    Next col
    grdDsn.row = oldRow
  End If
End Property
