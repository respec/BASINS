VERSION 5.00
Begin VB.Form frmTSlist 
   Caption         =   "Timeseries Data"
   ClientHeight    =   4305
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   7050
   HelpContextID   =   910
   Icon            =   "frmTSlist.frx":0000
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   ScaleHeight     =   4305
   ScaleWidth      =   7050
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   1200
      Top             =   1080
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      CancelError     =   -1  'True
   End
   Begin VB.Frame fraDateFormat 
      Caption         =   "Date Format"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3375
      Left            =   2400
      TabIndex        =   6
      Top             =   0
      Visible         =   0   'False
      Width           =   3135
      Begin VB.CheckBox chkMid24 
         Caption         =   "Midnight 24:00"
         Height          =   255
         Left            =   120
         TabIndex        =   26
         Top             =   1800
         Width           =   1695
      End
      Begin VB.CheckBox chkEndInterval 
         Caption         =   "End of interval"
         Height          =   252
         Left            =   120
         TabIndex        =   24
         Top             =   1560
         Width           =   1452
      End
      Begin VB.CommandButton cmdDateCancel 
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
         Left            =   1680
         TabIndex        =   21
         Top             =   2880
         Width           =   972
      End
      Begin VB.CommandButton cmdDateOk 
         Caption         =   "&OK"
         Default         =   -1  'True
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
         Left            =   360
         TabIndex        =   20
         Top             =   2880
         Width           =   972
      End
      Begin VB.CheckBox chkYY 
         Caption         =   "2-digit years"
         Height          =   252
         Left            =   120
         TabIndex        =   11
         Top             =   1320
         Width           =   1452
      End
      Begin VB.CheckBox chkMonthNames 
         Caption         =   "Month Names"
         Height          =   252
         Left            =   120
         TabIndex        =   10
         Top             =   1080
         Width           =   1452
      End
      Begin VB.CheckBox chkYears 
         Caption         =   "Years"
         Height          =   252
         Left            =   1920
         TabIndex        =   14
         Top             =   240
         Value           =   1  'Checked
         Width           =   972
      End
      Begin VB.CheckBox chkMonths 
         Caption         =   "Months"
         Height          =   252
         Left            =   1920
         TabIndex        =   15
         Top             =   480
         Value           =   1  'Checked
         Width           =   972
      End
      Begin VB.CheckBox chkDays 
         Caption         =   "Days"
         Height          =   252
         Left            =   1920
         TabIndex        =   16
         Top             =   720
         Value           =   1  'Checked
         Width           =   972
      End
      Begin VB.TextBox txtTimeSep 
         Height          =   288
         Left            =   1440
         TabIndex        =   13
         Text            =   ":"
         Top             =   2400
         Width           =   252
      End
      Begin VB.CheckBox chkSeconds 
         Caption         =   "Seconds"
         Height          =   252
         Left            =   1920
         TabIndex        =   19
         Top             =   1440
         Width           =   972
      End
      Begin VB.CheckBox chkMinutes 
         Caption         =   "Minutes"
         Height          =   252
         Left            =   1920
         TabIndex        =   18
         Top             =   1200
         Value           =   1  'Checked
         Width           =   972
      End
      Begin VB.TextBox txtDateSep 
         Height          =   288
         Left            =   1440
         TabIndex        =   12
         Text            =   "/"
         Top             =   2130
         Width           =   252
      End
      Begin VB.CheckBox chkHours 
         Caption         =   "Hours"
         Height          =   252
         Left            =   1920
         TabIndex        =   17
         Top             =   960
         Value           =   1  'Checked
         Width           =   972
      End
      Begin VB.OptionButton optDateFmt 
         Caption         =   "day month year"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   9
         Top             =   720
         Width           =   1620
      End
      Begin VB.OptionButton optDateFmt 
         Caption         =   "month day year"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   8
         Top             =   480
         Width           =   1620
      End
      Begin VB.OptionButton optDateFmt 
         Caption         =   "year month day"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   7
         Top             =   240
         Value           =   -1  'True
         Width           =   1656
      End
      Begin VB.OptionButton optDateFmt 
         Caption         =   "Modified Julian"
         Height          =   252
         Index           =   3
         Left            =   120
         TabIndex        =   25
         Top             =   960
         Visible         =   0   'False
         Width           =   1656
      End
      Begin VB.Label lblTimeSep 
         Caption         =   "Time Separator"
         Height          =   255
         Left            =   120
         TabIndex        =   23
         Top             =   2475
         Width           =   1455
      End
      Begin VB.Label lblDateSep 
         Caption         =   "Date Separator"
         Height          =   255
         Left            =   120
         TabIndex        =   22
         Top             =   2160
         Width           =   1455
      End
   End
   Begin ATCoCtl.ATCoScrollbar vScroll 
      Height          =   4212
      Left            =   6720
      TabIndex        =   5
      Top             =   0
      Width           =   252
      _ExtentX        =   450
      _ExtentY        =   7435
      Enabled         =   -1  'True
      Min             =   1
      Max             =   100
      SmallChange     =   1
      LargeChange     =   10
      DragEvents      =   -1  'True
      Value           =   1
   End
   Begin VB.Frame fraMergeDates 
      Caption         =   "Merging Dates"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   612
      Left            =   1320
      TabIndex        =   2
      Top             =   3600
      Visible         =   0   'False
      Width           =   4572
      Begin VB.CommandButton cmdCancel 
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
         Height          =   252
         Left            =   3600
         TabIndex        =   4
         Top             =   240
         Width           =   852
      End
      Begin MSComctlLib.ProgressBar progressMerge 
         Height          =   252
         Left            =   120
         TabIndex        =   3
         Top             =   240
         Width           =   3372
         _ExtentX        =   5953
         _ExtentY        =   450
         _Version        =   393216
         Appearance      =   1
      End
   End
   Begin VB.VScrollBar VScrollold 
      Height          =   3972
      Left            =   6720
      Min             =   1
      TabIndex        =   1
      Top             =   0
      Value           =   1
      Visible         =   0   'False
      Width           =   252
   End
   Begin ATCoCtl.ATCoGrid agd 
      Height          =   4215
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   6975
      _ExtentX        =   12303
      _ExtentY        =   7435
      SelectionToggle =   -1  'True
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
      Rows            =   2
      Cols            =   2
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "Courier New"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   ""
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   1
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuFileNew 
         Caption         =   "&New Timeseries"
      End
      Begin VB.Menu mnuFileSaveChanged 
         Caption         =   "Save Changed"
      End
      Begin VB.Menu mnuFileSaveSelected 
         Caption         =   "Save Selected"
      End
      Begin VB.Menu mnuFileSaveText 
         Caption         =   "Save to Text File"
      End
      Begin VB.Menu mnuFileSaveHeaders 
         Caption         =   "Save Headers"
      End
      Begin VB.Menu mnuFilePrint 
         Caption         =   "&Print"
      End
      Begin VB.Menu mnusep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuFileClose 
         Caption         =   "&Close"
      End
   End
   Begin VB.Menu mnuEdit 
      Caption         =   "&Edit"
      Begin VB.Menu mnuEditCopy 
         Caption         =   "&Copy"
      End
      Begin VB.Menu mnuEditCopyAll 
         Caption         =   "Copy &All"
      End
      Begin VB.Menu mnuEditPaste 
         Caption         =   "&Paste"
      End
      Begin VB.Menu mnuEditSep 
         Caption         =   "-"
      End
      Begin VB.Menu mnuEditDateFormat 
         Caption         =   "&Date Format"
      End
      Begin VB.Menu mnuEditNumberFormat 
         Caption         =   "&Number Format"
      End
      Begin VB.Menu mnuEditSelectAttributes 
         Caption         =   "&Select Attributes"
      End
      Begin VB.Menu mnuEditEditAttributes 
         Caption         =   "&Edit Attributes"
      End
   End
End
Attribute VB_Name = "frmTSlist"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private pTser As Collection
Private nts As Long
Private rowJday() As Double
Private nJdays As Long
Private topRow As Long
Private RowsBeforeData As Long
Private edited() As Boolean
Private WithEvents TSnew As frmTSnew
Attribute TSnew.VB_VarHelpID = -1
Private pTserEdited() As ATCclsTserData
Private pAvailAttributes As Collection
Private pVisibleAttributes As Collection
Private pOpenFiles As Collection
Private pCancelPressed As Boolean
Private pFinishedMergeDates As Boolean

Private FWid() As Long
Private SDig() As Long
Private dpla() As Long

Private pDateSettingFormat As Boolean 'True while setting up Date Format frame

Private pDateOrder   As Long
Private pDateSep     As String
Private pTimeSep     As String
Private pDateYears   As Boolean
Private pDateMonths  As Boolean
Private pDateDays    As Boolean
Private pDateHours   As Boolean
Private pDateMinutes As Boolean
Private pDateSeconds As Boolean
Private pDateYY      As Boolean
Private pDateMonthNames  As Boolean
Private pDateEndInterval As Boolean
Private pMidnight24      As Boolean

'Previous values to re-use if Cancel is pressed in Date Format frame
Private pDateOrderPrev   As Long
Private pDateSepPrev     As String
Private pTimeSepPrev     As String
Private pDateYearsPrev   As Boolean
Private pDateMonthsPrev  As Boolean
Private pDateDaysPrev    As Boolean
Private pDateHoursPrev   As Boolean
Private pDateMinutesPrev As Boolean
Private pDateSecondsPrev As Boolean
Private pDateYYPrev      As Boolean
Private pDateMonthNamesPrev As Boolean
Private pDateEndIntervalPrev As Boolean
Private pMidnight24Prev      As Boolean

Private pDragX As Single
Private pDragY As Single

Private Const bigJday = 1E+30
'Private Const SampleJday = 40374.567

Private pListClass As ATCoTSlist

Public Sub RaiseEdit()
  PopulateGrid
  If Not pListClass Is Nothing Then pListClass.RaiseEdit
End Sub

Public Sub RaiseCreatedTser(newTS As ATCclsTserData)
  If Not pListClass Is Nothing Then pListClass.RaiseCreatedTser newTS
End Sub

Public Property Get ListClass() As ATCoTSlist
  Set ListClass = pListClass
End Property
Public Property Set ListClass(newvalue As ATCoTSlist)
  Set pListClass = newvalue
End Property

Public Property Get FormatWidth(col As Long) As Long
  FormatWidth = FWid(col)
End Property
Public Property Let FormatWidth(col As Long, newvalue As Long)
  FWid(col) = newvalue
End Property

Public Property Get FormatSignifDigits(col As Long) As Long
  FormatSignifDigits = SDig(col)
End Property
Public Property Let FormatSignifDigits(col As Long, newvalue As Long)
  SDig(col) = newvalue
End Property

Public Property Get FormatDecimalPlaces(col As Long) As Long
  FormatDecimalPlaces = dpla(col)
End Property
Public Property Let FormatDecimalPlaces(col As Long, newvalue As Long)
  dpla(col) = newvalue
End Property

Public Property Get OpenFiles() As Collection
  Dim curTs As Long
  If pOpenFiles Is Nothing Then
    Set pOpenFiles = New Collection
    On Error Resume Next
    For curTs = 1 To nts
      pOpenFiles.Add pTserEdited(curTs).File, pTserEdited(curTs).File.FileName
    Next
  End If
  Set OpenFiles = pOpenFiles
End Property
Public Property Set OpenFiles(newvalue As Collection)
  Set pOpenFiles = newvalue
End Property

Public Property Get AvailAttributes() As Collection
  Set AvailAttributes = uniqueAttributeNames(pTser)
End Property

Public Property Get VisibleAttributes() As Collection
  Dim curTs As Long
  Dim MySettings As Variant, intSettings As Integer

  If pVisibleAttributes Is Nothing Then GoSub GetDefaultAttributes
  If pVisibleAttributes.Count < 1 Then GoSub GetDefaultAttributes
  Set VisibleAttributes = pVisibleAttributes
  Exit Property

GetDefaultAttributes:
  Set pVisibleAttributes = Nothing
  Set pVisibleAttributes = New Collection
  MySettings = GetAllSettings(App.Title, "TSListAttributes")
  If Not IsEmpty(MySettings) Then
    For intSettings = LBound(MySettings, 1) To UBound(MySettings, 1)
      pVisibleAttributes.Add MySettings(intSettings, 1)
    Next intSettings
  End If
  If pVisibleAttributes.Count < 1 Then
    pVisibleAttributes.Add "Scenario"
    pVisibleAttributes.Add "Location"
    pVisibleAttributes.Add "Constituent"
  End If
  Return
End Property
Public Property Set VisibleAttributes(newvalue As Collection)
  Set pVisibleAttributes = newvalue
End Property

Private Sub agd_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim dateIndex As Long
  Dim row As Long, col As Long
  Dim CurJday As Double
  Dim ts As ATCclsTserData
  Dim newtext As String
  Dim newvalue As Long
  Dim changed As Boolean
  Dim JdayIndex As Long
  
  changed = False
  
  If RowsBeforeData > 0 And ChangeFromRow < 4 Then 'Editing number format
    If ChangeToRow > 3 Then
      MsgBox "Changes were made in number format and values at the same time." & vbCr & _
             "Since this is likely to be a mistake, these changes will be undone.", vbOKOnly, "List Edit"
      PopulateGrid
    Else
      For row = ChangeFromRow To ChangeToRow
        For col = ChangeFromCol To ChangeToCol
          If IsNumeric(agd.TextMatrix(row, col)) Then
            newvalue = CLng(agd.TextMatrix(row, col))
            If newvalue >= 0 Then
              Select Case row
                Case 1
                      If FormatWidth(col) <> newvalue Then
                        FormatWidth(col) = newvalue
                        changed = True
                      End If
                Case 2
                      If FormatSignifDigits(col) <> newvalue Then
                        FormatSignifDigits(col) = newvalue
                        changed = True
                      End If
                Case 3
                      If FormatDecimalPlaces(col) <> newvalue Then
                        FormatDecimalPlaces(col) = newvalue
                        changed = True
                      End If
              End Select
            End If
          End If
        Next
      Next
    End If
    If changed Then PopulateGrid
    
  Else 'Editing values
    For col = ChangeFromCol To ChangeToCol
      Set ts = pTserEdited(col)
      With ts
        row = ChangeFromRow
        JdayIndex = row + topRow - 1 - RowsBeforeData
        If JdayIndex <= nJdays Then
          CurJday = rowJday(JdayIndex)
          dateIndex = .dates.IndexAtOrAfter(CurJday)
        End If
        Do
          newtext = agd.TextMatrix(row, col)
          If IsNumeric(newtext) Then
            With ts.dates.Summary
              If .CIntvl And Not pDateEndInterval Then CurJday = TimAddJ(CurJday, .Tu, .ts, 1)
            End With
            While (CurJday - .dates.Value(dateIndex)) > JulianSecond And dateIndex <= .dates.Summary.NVALS
              dateIndex = dateIndex + 1
            Wend
            If Abs(CurJday - .dates.Value(dateIndex)) < JulianSecond Then
              If Not edited(col) Then
                edited(col) = True
                Set pTserEdited(col) = ts.Copy
              End If
              pTserEdited(col).Value(dateIndex) = newtext
            End If
          End If
          row = row + 1
          JdayIndex = row + topRow - 1 - RowsBeforeData
          If JdayIndex <= nJdays Then
            CurJday = rowJday(JdayIndex)
          Else
            row = ChangeToRow + 1
          End If
        Loop While row <= ChangeToRow
      End With
    Next col
    PopulateGrid
  End If
End Sub

'Private Sub agd_KeyDown(KeyCode As Integer, Shift As Integer)
'  Dim ScrollChange As Long
'  Select Case KeyCode
'    Case vbKeyPageUp:   ScrollChange = -VScroll.LargeChange
'    Case vbKeyPageDown: ScrollChange = VScroll.LargeChange
'    Case vbKeyUp:   If agd.row = 1 Then ScrollChange = -1
'    Case vbKeyDown: If agd.row = agd.Rows Then ScrollChange = 1
'  End Select
'  If ScrollChange <> 0 Then
'    If VScroll.Value + ScrollChange > VScroll.Max Then
'      VScroll.Value = VScroll.Max
'    ElseIf VScroll.Value + ScrollChange < VScroll.Min Then
'      VScroll.Value = VScroll.Min
'    Else
'      VScroll.Value = VScroll.Value + ScrollChange
'    End If
'    KeyCode = 0
'  End If
'End Sub

Private Sub agd_RowColChange()
  If Len(agd.Text) > 0 Then
    agd.ColEditable(agd.col) = True
  Else
    agd.ColEditable(agd.col) = False
  End If
End Sub

Private Sub DateFormatFromFrame()
  If Not pDateSettingFormat Then
    If chkDays.Value = vbChecked Then pDateDays = True Else pDateDays = False
    If chkHours.Value = vbChecked Then pDateHours = True Else pDateHours = False
    If chkMinutes.Value = vbChecked Then pDateMinutes = True Else pDateMinutes = False
    If chkMonthNames.Value = vbChecked Then pDateMonthNames = True Else pDateMonthNames = False
    If chkMonths.Value = vbChecked Then pDateMonths = True Else pDateMonths = False
    If chkSeconds.Value = vbChecked Then pDateSeconds = True Else pDateSeconds = False
    If chkYears.Value = vbChecked Then pDateYears = True Else pDateYears = False
    If chkYY.Value = vbChecked Then pDateYY = True Else pDateYY = False
    pDateSep = txtDateSep.Text
    pTimeSep = txtTimeSep.Text
    If optDateFmt(0).Value Then pDateOrder = 0
    If optDateFmt(1).Value Then pDateOrder = 1
    If optDateFmt(2).Value Then pDateOrder = 2
    If optDateFmt(3).Value Then pDateOrder = 3
    If chkMid24.Value = vbChecked Then pMidnight24 = True Else pMidnight24 = False
    If chkEndInterval.Value = vbChecked Then
      If Not pDateEndInterval Then 'changed from false to true by user checking box
        pDateEndInterval = True
        pFinishedMergeDates = False
        nJdays = 0
        MergeDates
      End If
    Else
      If pDateEndInterval Then 'changed from true to false by user un-checking box
        pDateEndInterval = False
        pFinishedMergeDates = False
        nJdays = 0
        MergeDates
      End If
    End If
    If fraDateFormat.Visible Then PopulateGrid
  End If
End Sub

Private Sub DateFormatToFrame()
  pDateSettingFormat = True
  If pDateDays Then chkDays.Value = vbChecked Else chkDays.Value = vbUnchecked
  If pDateHours Then chkHours.Value = vbChecked Else chkHours.Value = vbUnchecked
  If pDateMinutes Then chkMinutes.Value = vbChecked Else chkMinutes.Value = vbUnchecked
  If pDateMonthNames Then chkMonthNames.Value = vbChecked Else chkMonthNames.Value = vbUnchecked
  If pDateMonths Then chkMonths.Value = vbChecked Else chkMonths.Value = vbUnchecked
  If pDateSeconds Then chkSeconds.Value = vbChecked Else chkSeconds.Value = vbUnchecked
  If pDateYears Then chkYears.Value = vbChecked Else chkYears.Value = vbUnchecked
  If pDateYY Then chkYY.Value = vbChecked Else chkYY.Value = vbUnchecked
  txtDateSep.Text = pDateSep
  txtTimeSep.Text = pTimeSep
  Select Case pDateOrder
    Case 0: optDateFmt(0).Value = True
    Case 1: optDateFmt(1).Value = True
    Case 2: optDateFmt(2).Value = True
    Case 3: optDateFmt(3).Value = True
  End Select
  If pMidnight24 Then chkMid24.Value = vbChecked Else chkMid24.Value = vbUnchecked
  If pDateEndInterval Then chkEndInterval.Value = vbChecked Else chkEndInterval.Value = vbUnchecked
  pDateSettingFormat = False
End Sub

Private Sub DateFormatFromRegistry()
  pDateOrder = GetSetting(App.Title, "TSListFormat", "DateOrder", pDateOrder)
  pDateSep = GetSetting(App.Title, "TSListFormat", "DateSep", pDateSep)
  pTimeSep = GetSetting(App.Title, "TSListFormat", "TimeSep", pTimeSep)
  pDateYears = GetSetting(App.Title, "TSListFormat", "DateYears", pDateYears)
  'Display of months, days, hours and minutes is set according to data interval
  'pDateMonths = GetSetting(App.Title, "TSListFormat", "DateMonths", pDateMonths)
  'pDateDays = GetSetting(App.Title, "TSListFormat", "DateDays", pDateDays)
  'pDateHours = GetSetting(App.Title, "TSListFormat", "DateHours", pDateHours)
  'pDateMinutes = GetSetting(App.Title, "TSListFormat", "DateMinutes", pDateMinutes)
  pDateSeconds = GetSetting(App.Title, "TSListFormat", "DateSeconds", pDateSeconds)
  pDateMonthNames = GetSetting(App.Title, "TSListFormat", "DateMonthNames", pDateMonthNames)
  pDateYY = GetSetting(App.Title, "TSListFormat", "DateYY", pDateYY)
  pMidnight24 = GetSetting(App.Title, "TSListFormat", "Midnight24", pMidnight24)
  pDateEndInterval = GetSetting(App.Title, "TSListFormat", "DateEndInterval", pDateEndInterval)
End Sub
Private Sub DateFormatToRegistry()
  SaveSetting App.Title, "TSListFormat", "DateOrder", pDateOrder
  SaveSetting App.Title, "TSListFormat", "DateSep", pDateSep
  SaveSetting App.Title, "TSListFormat", "TimeSep", pTimeSep
  SaveSetting App.Title, "TSListFormat", "DateYears", pDateYears
  'Display of months, days, hours and minutes is set according to data interval
  'SaveSetting App.Title, "TSListFormat", "DateMonths", pDateMonths
  'SaveSetting App.Title, "TSListFormat", "DateDays", pDateDays
  'SaveSetting App.Title, "TSListFormat", "DateHours", pDateHours
  'SaveSetting App.Title, "TSListFormat", "DateMinutes", pDateMinutes
  SaveSetting App.Title, "TSListFormat", "DateSeconds", pDateSeconds
  SaveSetting App.Title, "TSListFormat", "DateMonthNames", pDateMonthNames
  SaveSetting App.Title, "TSListFormat", "DateYY", pDateYY
  SaveSetting App.Title, "TSListFormat", "Midnight24", pMidnight24
  SaveSetting App.Title, "TSListFormat", "DateEndInterval", pDateEndInterval
End Sub

Private Sub chkDays_Click()
  DateFormatFromFrame
End Sub

Private Sub chkEndInterval_Click()
  DateFormatFromFrame
End Sub

Private Sub chkHours_Click()
  DateFormatFromFrame
End Sub

Private Sub chkMid24_Click()
  DateFormatFromFrame
End Sub

Private Sub chkMinutes_Click()
  DateFormatFromFrame
End Sub

Private Sub chkMonthNames_Click()
  DateFormatFromFrame
End Sub
Private Sub chkMonths_Click()
  DateFormatFromFrame
End Sub
Private Sub chkSeconds_Click()
  DateFormatFromFrame
End Sub
Private Sub chkYears_Click()
  DateFormatFromFrame
End Sub
Private Sub chkYY_Click()
  DateFormatFromFrame
End Sub

Private Sub fraMergeDates_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  If Button = 1 Then
    pDragX = x
    pDragY = y
  End If
End Sub
Private Sub fraMergeDates_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If Button = 1 Then
    fraMergeDates.Top = fraMergeDates.Top + y - pDragY
    fraMergeDates.Left = fraMergeDates.Left + x - pDragX
  End If
End Sub

Private Sub fraDateFormat_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  Select Case Button
    Case vbLeftButton
      pDragX = x
      pDragY = y
    Case vbRightButton
      optDateFmt(3).Visible = Not optDateFmt(3).Visible
  End Select
End Sub
Private Sub fraDateFormat_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If Button = 1 Then
    fraDateFormat.Top = fraDateFormat.Top + y - pDragY
    fraDateFormat.Left = fraDateFormat.Left + x - pDragX
  End If
End Sub

Private Sub mnuEditEditAttributes_Click()
  Dim pTsEdTemp As New Collection, i As Long
  For i = 1 To nts
    pTsEdTemp.Add pTserEdited(i)
  Next
  Set TSEdit.TimeseriesToEdit = pTsEdTemp
  Set TSEdit.Notify = Me
  TSEdit.Show
End Sub

Private Sub mnuFileSaveHeaders_Click()
  Dim AllHeaders As String
  Dim r As Long
  Dim c As Long
  On Error GoTo NeverMind
  For r = 1 - agd.FixedRows To 0
    For c = 0 To agd.cols - 2
      AllHeaders = AllHeaders & agd.TextMatrix(r, c) & vbTab
    Next
    AllHeaders = AllHeaders & agd.TextMatrix(r, c) & vbCrLf
  Next
  cdlg.CancelError = True
  cdlg.DialogTitle = "Save Headers As:"
  cdlg.DefaultExt = ".txt"
  cdlg.Filter = "Text files (*.txt)|*.txt|All Files|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowSave
  If Len(cdlg.FileName) > 0 Then SaveFileString cdlg.FileName, AllHeaders
NeverMind:
End Sub

Private Sub optDateFmt_Click(index As Integer)
  DateFormatFromFrame
End Sub
Private Sub txtDateSep_Change()
  DateFormatFromFrame
End Sub

Private Sub txtTimeSep_Change()
  DateFormatFromFrame
End Sub

Private Sub cmdCancel_Click()
  pCancelPressed = True
End Sub

Private Sub cmdDateCancel_Click()
  pDateOrder = pDateOrderPrev
  pDateSep = pDateSepPrev
  pTimeSep = pTimeSepPrev
  pDateYears = pDateYearsPrev
  pDateMonths = pDateMonthsPrev
  pDateDays = pDateDaysPrev
  pDateHours = pDateHoursPrev
  pDateMinutes = pDateMinutesPrev
  pDateSeconds = pDateSecondsPrev
  pDateMonthNames = pDateMonthNamesPrev
  pDateYY = pDateYYPrev
  pMidnight24 = pMidnight24Prev
  If pDateEndInterval <> pDateEndIntervalPrev Then
    pDateEndInterval = pDateEndIntervalPrev
    MergeDates
  End If
  fraDateFormat.Visible = False
  mnuEditDateFormat.Checked = False
  PopulateGrid
End Sub

Private Sub cmdDateOk_Click()
  fraDateFormat.Visible = False
  mnuEditDateFormat.Checked = False
  'PopulateGrid
End Sub

Private Sub Form_Load()
  Dim Left As Long, Top As Long
  With agd
    .ColTitle(0) = "Date"
    .ColTitle(1) = "Value"
    .Rows = 0
  End With
  
  'get width and height from last time, but keep new left and top to avoid overlapping existing windows
  Left = Me.Left
  Top = Me.Top
  RetrieveWindowSettings Me, App.Title, "TSListWindow"
  Me.Left = Left
  Me.Top = Top
  
  'App.HelpFile = gHelpFileName
  pDateSettingFormat = False
  DateFormatFromFrame
  DateFormatFromRegistry
  DateFormatToFrame
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  Dim f&, Msg As String, nEdited As Long
  For f = 1 To nts
    If edited(f) Then
      If nEdited > 0 Then Msg = Msg & ", ":
      Msg = Msg & f
      nEdited = nEdited + 1
    End If
  Next f
  If nEdited > 0 Then
    If nEdited = 1 Then
      Msg = "Column " & Msg & " has been edited." & vbCr & "Discard new values?"
    Else
      Msg = "Columns " & Msg & " have been edited." & vbCr & "Discard new values?"
    End If
    If MsgBox(Msg, vbYesNo, "Close") = vbNo Then Cancel = True
  End If
  DateFormatToRegistry
End Sub

Private Sub Form_Resize()
  Static LastHeight As Long, LastWidth As Long
  
  If LastWidth <> Width Then
    If Me.ScaleWidth > 200 Then
      agd.Width = Me.ScaleWidth - 30 - vScroll.Width
      vScroll.Left = agd.Left + agd.Width
    End If
    LastWidth = Width
  End If
  
  If LastHeight <> Height Then
    If Me.ScaleHeight - agd.Top > 50 Then
      agd.Height = Me.ScaleHeight - agd.Top - 50
      vScroll.Height = agd.Height    'If agd.Height > 270 Then VScroll.Height = agd.Height - 270
    End If
    ResizeGrid
    fraMergeDates.Top = Height - fraMergeDates.Height * 2
    LastHeight = Height
  End If

  SaveWindowSettings Me, App.Title, "TSListWindow"
End Sub

Private Sub ResizeGrid()
  Dim origRows As Long
  With agd
    origRows = .Rows + .FixedRows
    .FixedRows = VisibleAttributes.Count
    If .Rows < 1 Then .Rows = 1
    While .RowIsVisible(.Rows)
      .Rows = .Rows + 1
    Wend
    While .Rows > 1 And Not .RowIsVisible(.Rows)
      .Rows = .Rows - 1
    Wend
    If .Rows + .FixedRows > origRows Then
      PopulateGrid
      .ColsSizeByContents
    End If
  
    If .Rows + .FixedRows <> origRows Then
      If .Rows < 2 Then
        vScroll.LargeChange = 1
      Else 'If nJdays < 32000 Then 'had to limit LargeChange with VB's limited scrollbar
        vScroll.LargeChange = .Rows - 1
      'Else
      '  vScroll.LargeChange = Fix((.Rows - 1) * 32000 / nJdays)
      End If
      'Debug.Print VScroll.LargeChange
    End If
  End With
End Sub

Public Property Get AllTSer() As Collection
  Set AllTSer = pTser
End Property
Public Property Set AllTSer(newvalue As Collection)
  Dim tsIndex As Long
  Dim MinInterval As Double
  
  Set pTser = Nothing
  Set pTser = newvalue
  If pTser Is Nothing Then Set pTser = New Collection
  nts = pTser.Count
  ReDim edited(nts)
  ReDim pTserEdited(nts)
  ReDim FWid(nts)
  ReDim SDig(nts)
  ReDim dpla(nts)
  
  MinInterval = JulianYear
  For tsIndex = 1 To nts
    Set pTserEdited(tsIndex) = pTser(tsIndex)
    FWid(tsIndex) = 10
    SDig(tsIndex) = 3
    With pTserEdited(tsIndex)
      If .dates.Summary.CIntvl Then
        If .dates.Summary.Intvl < MinInterval Then MinInterval = .dates.Summary.Intvl
      Else
        MinInterval = 0
      End If
      If .Max < 0.01 Then
        dpla(tsIndex) = 6
      ElseIf .Max < 0.1 Then
        dpla(tsIndex) = 5
      ElseIf .Max < 1 Then
        dpla(tsIndex) = 4
      ElseIf .Max < 10 Then
        dpla(tsIndex) = 3
      Else
        dpla(tsIndex) = 1
      End If
    End With
  Next
  
  'Set the display of months, days, hours, minutes, seconds to make sense with current data
  pDateSettingFormat = True
  'For yearly data, don't show months, otherwise do show months
  If MinInterval > 300 Then Me.chkMonths.Value = vbUnchecked Else Me.chkMonths.Value = vbChecked
  'For monthly or greater interval data, don't show days, otherwise do show days
  If MinInterval > 20 Then Me.chkDays.Value = vbUnchecked Else Me.chkDays.Value = vbChecked
  'For daily or greater interval data, don't show hours, minutes, seconds, otherwise show at least hours and minutes
  If MinInterval >= 1 Then Me.chkHours.Value = vbUnchecked Else Me.chkHours.Value = vbChecked
  Me.chkMinutes.Value = Me.chkHours.Value
  If Not Me.chkMinutes.Value Then Me.chkSeconds.Value = vbUnchecked
  pDateSettingFormat = False
  DateFormatFromFrame
  
  pFinishedMergeDates = False
  nJdays = 0
End Property

Public Sub Add(newTS As ATCclsTserData)
  If pTser Is Nothing Then
    Set pTser = New Collection
  End If
  pTser.Add newTS
  nts = pTser.Count
  ReDim Preserve edited(nts)
  ReDim Preserve pTserEdited(nts)
  ReDim Preserve FWid(nts)
  ReDim Preserve SDig(nts)
  ReDim Preserve dpla(nts)
  Set pTserEdited(nts) = newTS
  FWid(nts) = 10
  SDig(nts) = 3
  dpla(nts) = 1
  pFinishedMergeDates = False
  nJdays = 0
End Sub

Private Sub MergeDates()
  Dim lastIndex() As Long
  Dim dayIndex() As Long
  Dim nextJday() As Double
  Dim LastEnd As Double
  Dim testJday As Double
  Dim DimJdays As Long
  Dim LastUpdate As Single
  Dim allDone As Boolean
  Static Populated As Boolean
  Static inMergeDates As Boolean
  Dim tsIndex As Long
  
  Populated = False
  
  If inMergeDates Then Exit Sub
  inMergeDates = True
  
  ReDim nextJday(nts)
  ReDim dayIndex(nts)
  ReDim lastIndex(nts)
  
  If nts = 1 Then
    If nJdays = 0 Then
      DimJdays = pTser(1).dates.Summary.NVALS
      'rowJday = pTser(1).dates.Values 'Thought we could be clever and cheap, but we want more processing
      If DimJdays < 0 Then DimJdays = 0
      ReDim rowJday(DimJdays)
    End If
  Else
    If nJdays = 0 Then
      DimJdays = 1000
      ReDim rowJday(DimJdays)
    Else
      DimJdays = nJdays + 1000
      ReDim Preserve rowJday(DimJdays)
    End If
  End If 'Moved this End If up from just before pFinishedMergeDates = True
    
  LastUpdate = Timer
  
  For tsIndex = 1 To nts
    With pTser(tsIndex).dates
      dayIndex(tsIndex) = 1
      If .Summary.CIntvl And pDateEndInterval Then
        nextJday(tsIndex) = .Value(1)
      Else
        nextJday(tsIndex) = .Summary.SJDay
      End If
      lastIndex(tsIndex) = .Summary.NVALS
      If .Summary.EJDay > LastEnd Then LastEnd = .Summary.EJDay
    End With
  Next

NextRow:
  If Not Populated Then
    If nJdays >= topRow + agd.Rows Then PopulateGridBackend: Populated = True
  End If
  
  nJdays = nJdays + 1
  testJday = bigJday
  For tsIndex = 1 To nts
    If nextJday(tsIndex) < testJday Then testJday = nextJday(tsIndex)
  Next
    
  If testJday < bigJday Then
    
    'Provide feedback if this is taking a while
    If Timer - LastUpdate > 0.5 Then
      If fraMergeDates.Visible Then
        SetVscrollMax
        DoEvents
        If pCancelPressed Then
          Me.MousePointer = vbDefault
          fraMergeDates.Visible = False
          Exit Sub
        End If
      Else
        fraMergeDates.Visible = True
        DoEvents
      End If
      progressMerge.Value = 100 * (testJday - rowJday(1)) / (LastEnd - rowJday(1))
      LastUpdate = Timer
    End If
    
    If nJdays > DimJdays Then
      DimJdays = nJdays + DimJdays
      ReDim Preserve rowJday(DimJdays)
    End If
    rowJday(nJdays) = testJday
    
    For tsIndex = 1 To nts
      If nextJday(tsIndex) - JulianSecond < testJday Then
        dayIndex(tsIndex) = dayIndex(tsIndex) + 1
        If dayIndex(tsIndex) > lastIndex(tsIndex) Then
          nextJday(tsIndex) = bigJday
        Else
          With pTser(tsIndex).dates
            If .Summary.CIntvl And Not pDateEndInterval Then
              nextJday(tsIndex) = .Value(dayIndex(tsIndex) - 1) 'TimAddJ(nextJday(tsIndex), .Summary.Tu, .Summary.ts, 1)
            Else
              nextJday(tsIndex) = .Value(dayIndex(tsIndex))
            End If
          End With
        End If
      End If
    Next
    
    GoTo NextRow
  
  End If
  
  nJdays = nJdays - 1
  If nJdays <> DimJdays Then ReDim Preserve rowJday(nJdays)
' End If
  pFinishedMergeDates = True
  fraMergeDates.Visible = False
  If Not Populated Then PopulateGridBackend
  SetVscrollMax
  inMergeDates = False
End Sub

Private Sub SetVscrollMax()
  'If nJdays < 32000 Then
    vScroll.Max = nJdays
  'Else
  '  vScroll.Max = 32000
  'End If
End Sub

Public Sub NewVisibleAttributes()
  Dim vAttr As Variant
  On Error Resume Next
  DeleteSetting App.Title, "TSListAttributes"
  For Each vAttr In pVisibleAttributes
    SaveSetting App.Title, "TSListAttributes", vAttr, vAttr
  Next
  On Error GoTo 0
  ResizeGrid
  PopulateGrid
End Sub

Public Sub PopulateGrid()
  Dim tsIndex As Long, row As Long, col As Long
  Dim attrval As Variant, AttrName As String
'  Dim oldColWidth() As Long
  If pTser Is Nothing Then Exit Sub
  If nts < 1 Then Exit Sub
  With agd
    If .Rows < 2 Then ResizeGrid
    .clear
    If topRow < 1 Then topRow = 1
    For row = 1 - VisibleAttributes.Count To 0 'rows <= are the non-scrolling column headers
      .TextMatrix(row, 0) = pVisibleAttributes(.FixedRows + row)
    Next row
    For tsIndex = 1 To nts
      .colDecimalPos(tsIndex) = Fix(Log10(Fix(pTserEdited(tsIndex).Max))) + 2
      'space for -
      If pTserEdited(tsIndex).Min < 0 Then .colDecimalPos(tsIndex) = .colDecimalPos(tsIndex) + 1
      For row = 1 - VisibleAttributes.Count To 0 'rows <= are the non-scrolling column headers
        AttrName = pVisibleAttributes(.FixedRows + row)
        attrval = pTserEdited(tsIndex).Attrib(AttrName)
        If IsNumeric(attrval) Then               'Format numeric attributes like we format values
          Select Case AttrName
            Case "Max", "Min", "Sum", "Mean", "Geometric Mean", "StdDeviation", "Variance"
              .TextMatrix(row, tsIndex) = FormatColValue(tsIndex, CSng(attrval))
            Case Else '"DSN", "Nval", "TUnit", "TStep" 'Don't format some numeric attributes as values
              .TextMatrix(row, tsIndex) = attrval
          End Select
        Else
          .TextMatrix(row, tsIndex) = attrval
        End If
      Next row
    Next
    
    Me.MousePointer = vbHourglass
    
    If Not pFinishedMergeDates And nJdays < topRow + .Rows Then
      pCancelPressed = False
      MergeDates
      If pCancelPressed Then Debug.Print "Cancel pressed" 'Unload Me
    Else
      PopulateGridBackend
    End If
    Me.MousePointer = vbDefault
  End With
End Sub

Private Function DateString(Jday As Double) As String
  Dim retval As String
  Dim curDate(5) As Long
  Dim altDate(5) As Long
  
  J2Date Jday, curDate
  If curDate(3) = 24 Then
    J2Date Jday + JulianHour, curDate
    curDate(3) = 0
  End If
  
  If pMidnight24 Then timcnv curDate 'convert to 24th hr prev day
  
  Select Case pDateOrder
    Case 0:
            If pDateYears Then
              If pDateYY Then
                retval = retval & Right(curDate(0), 2)
              Else
                retval = curDate(0)
              End If
            End If
            If pDateMonths Then
              If pDateYears Then retval = retval & pDateSep
              If pDateMonthNames Then
                retval = retval & MonthName3(curDate(1))
              Else
                retval = retval & Format(curDate(1), "00")
              End If
            End If
            If pDateDays Then
              If pDateYears Or pDateMonths Then retval = retval & pDateSep
              retval = retval & Format(curDate(2), "00")
            End If
    Case 1:
            If pDateMonths Then
              If pDateMonthNames Then
                retval = retval & MonthName3(curDate(1))
              Else
                retval = retval & Format(curDate(1), "00")
              End If
            End If
            If pDateDays Then
              If pDateMonths Then retval = retval & pDateSep
              retval = retval & Format(curDate(2), "00")
            End If
            If pDateYears Then
              If pDateDays Or pDateMonths Then retval = retval & pDateSep
              If pDateYY Then
                retval = retval & Right(curDate(0), 2)
              Else
                retval = retval & curDate(0)
              End If
            End If
    Case 2:
            If pDateDays Then
              retval = retval & Format(curDate(2), "00")
            End If
            If pDateMonths Then
              If pDateDays Then retval = retval & pDateSep
              If pDateMonthNames Then
                retval = retval & MonthName3(curDate(1))
              Else
                retval = retval & Format(curDate(1), "00")
              End If
            End If
            If pDateYears Then
              If pDateDays Or pDateMonths Then retval = retval & pDateSep
              If pDateYY Then
                retval = retval & Right(curDate(0), 2)
              Else
                retval = retval & curDate(0)
              End If
            End If
    Case 3: retval = ATCformat(Jday, "00000.000")
  End Select
  If pDateHours Or pDateMinutes Or pDateSeconds Then retval = retval & " "
  If pDateHours Then retval = retval & Format(CStr(curDate(3)), "00")
  If pDateMinutes Then retval = retval & pTimeSep & Format(CStr(curDate(4)), "00")
  If pDateSeconds Then retval = retval & pTimeSep & Format(CStr(curDate(5)), "00")
  DateString = retval
End Function

Private Sub PopulateGridBackend()
  Dim index As Long, NumVals As Long
  Dim CurJday As Double
  Dim ptsJday As Double
  Dim CompareJday As Double
  Dim row As Long, col As Long
  Dim JdayIndex As Long
  Dim tsIndex As Long
  Dim dayIndex() As Long, LastDay() As Long, CIntvl() As Boolean
  
  If nJdays < topRow Then Exit Sub

  ReDim dayIndex(nts)
  ReDim LastDay(nts)
  ReDim CIntvl(nts)
  
  CurJday = rowJday(topRow)
  For tsIndex = 1 To nts
    CIntvl(tsIndex) = pTser(tsIndex).dates.Summary.CIntvl
    LastDay(tsIndex) = pTser(tsIndex).dates.Summary.NVALS
    If CIntvl(tsIndex) And Not pDateEndInterval Then
      dayIndex(tsIndex) = pTser(tsIndex).dates.IndexAtOrAfter(CurJday)
    Else
      dayIndex(tsIndex) = pTser(tsIndex).dates.IndexAtOrAfter(CurJday)
    End If
  Next
  With agd
    If RowsBeforeData > 0 Then
      .TextMatrix(1, 0) = "Number Width"
      .TextMatrix(2, 0) = "Significant Digits"
      .TextMatrix(3, 0) = "Decimal Places"
      For col = 1 To .cols - 1
        agd.TextMatrix(1, col) = FWid(col)
        agd.TextMatrix(2, col) = SDig(col)
        agd.TextMatrix(3, col) = dpla(col)
      Next
    End If
    For row = 1 + RowsBeforeData To .Rows
      JdayIndex = row + topRow - 1 - RowsBeforeData
      If JdayIndex > nJdays Then Exit For
      CurJday = rowJday(JdayIndex)
      .TextMatrix(row, 0) = DateString(CurJday)
      
      For tsIndex = 1 To nts
DoDay:
        If dayIndex(tsIndex) <= LastDay(tsIndex) Then
          ptsJday = pTser(tsIndex).dates.Value(dayIndex(tsIndex))
          If CIntvl(tsIndex) And Not pDateEndInterval Then
            With pTser(tsIndex).dates.Summary
              CompareJday = TimAddJ(CurJday, .Tu, .ts, 1)
            End With
          Else
            CompareJday = CurJday
          End If
          
          If Abs(CompareJday - ptsJday) < JulianSecond Then
            .TextMatrix(row, tsIndex) = FormatColValue(tsIndex, pTserEdited(tsIndex).Value(dayIndex(tsIndex)))
            dayIndex(tsIndex) = dayIndex(tsIndex) + 1
          ElseIf CompareJday > ptsJday Then
            dayIndex(tsIndex) = dayIndex(tsIndex) + 1
            GoTo DoDay
          End If
        End If
      Next
    Next
    If row < .Rows And pCancelPressed Then .TextMatrix(row, 0) = "Cancelled Merge"
  End With
End Sub

Private Sub mnuEditCopy_Click()
  agd.Copy
End Sub

Private Sub mnuEditDateFormat_Click()
  If fraDateFormat.Visible Then
    fraDateFormat.Visible = False
  Else
    pDateOrderPrev = pDateOrder
    pDateSepPrev = pDateSep
    pTimeSepPrev = pTimeSep
    pDateYearsPrev = pDateYears
    pDateMonthsPrev = pDateMonths
    pDateDaysPrev = pDateDays
    pDateHoursPrev = pDateHours
    pDateMinutesPrev = pDateMinutes
    pDateSecondsPrev = pDateSeconds
    pDateMonthNamesPrev = pDateMonthNames
    pDateYYPrev = pDateYY
    pMidnight24Prev = pMidnight24
    pDateEndIntervalPrev = pDateEndInterval
    If fraDateFormat.Left < 0 Or fraDateFormat.Left > (Me.ScaleWidth - fraDateFormat.Width) Then fraDateFormat.Left = Me.ScaleWidth / 2
    If fraDateFormat.Top < 0 Or fraDateFormat.Top > (Me.ScaleHeight - fraDateFormat.Height) Then fraDateFormat.Top = 1
    DateFormatToFrame
    fraDateFormat.Visible = True
  End If
  mnuEditDateFormat.Checked = fraDateFormat.Visible
End Sub

Private Sub mnuEditNumberFormat_Click()
'  Dim numFmt As New frmTSNumFormat
'  numFmt.SetHeaders agd
'  numFmt.SetTSlist Me
'  numFmt.Show
  If mnuEditNumberFormat.Checked Then
    mnuEditNumberFormat.Checked = False
    RowsBeforeData = 0
  Else
    mnuEditNumberFormat.Checked = True
    RowsBeforeData = 3
  End If
  PopulateGrid
End Sub

Private Sub mnuEditPaste_Click()
  agd.Paste
End Sub

Private Sub mnuEditCopyAll_Click()
  Me.MousePointer = vbHourglass
  Clipboard.clear
  Clipboard.SetText WholeGridString
  Me.MousePointer = vbDefault
End Sub

Private Sub mnuEditSelectAttributes_Click()
  Set TSCol.Grid = Me
  'TSCol.Caption = "Time Series Attributes"
  'TSCol.asl.LeftLabel = "Available Attributes"
  'TSCol.asl.RightLabel = "Show These Attributes"
  TSCol.lblShow.Caption = "" '"Attributes appear at top of column"
  TSCol.Show
End Sub

Private Sub mnuFileClose_Click()
  Unload Me
End Sub

Private Sub mnuFileNew_Click()
  Set TSnew = New frmTSnew
  Set TSnew.OpenFiles = Me.OpenFiles
  Set TSnew.AllTSer = pTser
  TSnew.Show
End Sub

Private Sub mnuFilePrint_Click()
  frmTSPrintSave.Show
  frmTSPrintSave.PrintGrid Me
End Sub

Private Sub mnuFileSaveSelected_Click()
  Dim f As Long, lTsFile As ATCclsTserFile
  Dim tSave As New frmTSsave
  Dim saved As Boolean
  Set tSave.OpenFiles = Me.OpenFiles
  f = agd.SelstartCol
  If f < 1 Then f = 1
  While f <= agd.SelEndCol
    pTserEdited(f).calcSummary
    Set tSave.Tser = pTserEdited(f)
    tSave.Show vbModal
    f = f + 1
    saved = True
  Wend
  Unload tSave
  If saved Then RaiseEdit
End Sub

' Old save
'      With pTserEdited(f)
'        'Would be nice to have a choice of which file to save to.
'        Select Case MsgBox("Save column " & f & vbCr & _
'          "Scenario '" & .Header.Sen & "'" & vbCr & _
'          "Location '" & .Header.Loc & "'" & vbCr & _
'          "Constituent '" & .Header.Con & "'" & vbCr & _
'          "To file " & .file.filename & " ?", vbYesNoCancel, "Save Timeseries")
'          Case vbYes
'            If .file.addtimser(pTserEdited(f), TsIdReplAsk + TsIdAppendAsk + TsIdRenumAsk) Then 'Add successful
'              'MsgBox "Saved." confimation messagebox is opened in addtimser, at least in WDM it is.
'            Else
'              MsgBox "Failed to save timeseries." & vbCr & lTsFile.ErrorDescription, vbExclamation, "Save Timeseries"
'            End If
'          Case vbCancel
'            Exit Sub
'        End Select
'      End With

Private Sub mnuFileSaveChanged_Click()
  Dim f As Long, lTsFile As ATCclsTserFile
  Dim tSave As New frmTSsave
  Dim saved As Boolean
  Set tSave.OpenFiles = Me.OpenFiles
  For f = 1 To nts
    If edited(f) Then
      pTserEdited(f).calcSummary
      Set tSave.Tser = pTserEdited(f)
      tSave.Show vbModal
      edited(f) = False
      saved = True
    End If
  Next f
  Unload tSave
  If saved Then RaiseEdit
End Sub

Private Function WholeGridString(Optional delim As String = vbTab, Optional EndOfLine As String = vbCrLf) As String
  Dim retval As String
  Dim r&, c&
  Dim ColValue As String
  Dim index As Long, NumVals As Long
  Dim CurJday As Double
  Dim ptsJday As Double
  Dim CompareJday As Double
  Dim tsIndex As Long
  Dim dayIndex() As Long, LastDay() As Long, CIntvl() As Boolean
  
  For r = 1 To agd.FixedRows
    For c = 0 To agd.cols - 1
      retval = retval & agd.TextMatrix(r - agd.FixedRows, c)
      If (c + 1) < agd.cols Then retval = retval & delim
    Next
    retval = retval & EndOfLine
  Next

  ReDim dayIndex(nts)
  ReDim LastDay(nts)
  ReDim CIntvl(nts)
  
  For tsIndex = 1 To nts
    CIntvl(tsIndex) = pTser(tsIndex).dates.Summary.CIntvl
    LastDay(tsIndex) = pTser(tsIndex).dates.Summary.NVALS
    If CIntvl(tsIndex) And Not pDateEndInterval Then
      dayIndex(tsIndex) = pTser(tsIndex).dates.IndexAtOrAfter(CurJday)
    Else
      dayIndex(tsIndex) = pTser(tsIndex).dates.IndexAtOrAfter(CurJday)
    End If
  Next
  
  For r = 1 To nJdays
    CurJday = rowJday(r)
    retval = retval & DateString(CurJday) & delim
    
    For tsIndex = 1 To nts
DoDay:
      If dayIndex(tsIndex) <= LastDay(tsIndex) Then
        ptsJday = pTser(tsIndex).dates.Value(dayIndex(tsIndex))
        If CIntvl(tsIndex) And Not pDateEndInterval Then
          With pTser(tsIndex).dates.Summary
            CompareJday = TimAddJ(CurJday, .Tu, .ts, 1)
          End With
        Else
          CompareJday = CurJday
        End If
        
        If Abs(CompareJday - ptsJday) < JulianSecond Then
          ColValue = FormatColValue(tsIndex, pTserEdited(tsIndex).Value(dayIndex(tsIndex)))
          If delim <> "" Then ColValue = Trim(ColValue)
          retval = retval & ColValue
          dayIndex(tsIndex) = dayIndex(tsIndex) + 1
        ElseIf CompareJday > ptsJday Then
          dayIndex(tsIndex) = dayIndex(tsIndex) + 1
          GoTo DoDay
        End If
      End If
      If tsIndex < nts Then retval = retval & delim Else retval = retval & EndOfLine
    Next
  Next
  
  WholeGridString = retval
  
  Exit Function
SaveError:
  MsgBox "Error copying " & vbCr & Err.Description, vbExclamation, "Copying Time Series List"

End Function

'Non-interactively save or print
'if outFile is zero, print
'if outFile is a valid file handle as returned by FreeFile(0), save
'   filename is prompted for if not specified
'includeColTitles - if true, column titles are written, default is true
'
'delimiter is character to separate columns. delimiter = "" to space-pad columns instead
'quote is added before and after each value written
Public Sub SavePrint(OutFile%, _
                    Optional includeColTitles As Boolean = True, _
                    Optional FileName$ = "", _
                    Optional delimiter$ = "", _
                    Optional emptyCell$ = "")
  Dim outstr As String, cellStr As String, cellWidth As Long
  Dim r&, c&
  Dim ColValue As String
  Dim index As Long, NumVals As Long
  Dim CurJday As Double
  Dim ptsJday As Double
  Dim CompareJday As Double
  Dim tsIndex As Long
  Dim dayIndex() As Long, LastDay() As Long, CIntvl() As Boolean
  Const PrintMargin = "        "
  
  outstr = ""
  
  On Error GoTo SaveError
  If OutFile > 0 Then
    Open FileName For Output As OutFile
  Else
    Dim fp&, tp&
    fp = 1
    tp = 1
    Call ShowPrinterX(Me, fp, tp, 1, 1, PD_NOSELECTION + PD_NOPAGENUMS + PD_DISABLEPRINTTOFILE)
    If tp < 0 Then Exit Sub 'Cancel selected in print dialog
  End If
  If includeColTitles Then
    For r = 1 To agd.FixedRows
      For c = 0 To agd.cols - 1
        cellStr = agd.TextMatrix(r - agd.FixedRows, c)
        
        If cellStr = "" Then cellStr = emptyCell
        If delimiter = "" Then
          If c = 0 Then
            cellWidth = Len(DateString(CurJday))
          Else
            cellWidth = Len(FormatColValue(c, pTserEdited(c).Value(1)))
          End If
          cellWidth = cellWidth - Len(cellStr)
          If cellWidth > 0 Then cellStr = Space(cellWidth) & cellStr
          cellStr = cellStr & " "
        End If
        outstr = outstr & cellStr
        If (c + 1) < agd.cols Then outstr = outstr & delimiter
      Next
      GoSub PrintOut
    Next
  End If

  ReDim dayIndex(nts)
  ReDim LastDay(nts)
  ReDim CIntvl(nts)
  
  For tsIndex = 1 To nts
    CIntvl(tsIndex) = pTser(tsIndex).dates.Summary.CIntvl
    LastDay(tsIndex) = pTser(tsIndex).dates.Summary.NVALS
    If CIntvl(tsIndex) And Not pDateEndInterval Then
      dayIndex(tsIndex) = pTser(tsIndex).dates.IndexAtOrAfter(CurJday)
    Else
      dayIndex(tsIndex) = pTser(tsIndex).dates.IndexAtOrAfter(CurJday)
    End If
  Next
  
  For r = 1 To nJdays
    CurJday = rowJday(r)
    outstr = DateString(CurJday) & delimiter
    
    For tsIndex = 1 To nts
DoDay:
      If dayIndex(tsIndex) <= LastDay(tsIndex) Then
        ptsJday = pTser(tsIndex).dates.Value(dayIndex(tsIndex))
        If CIntvl(tsIndex) And Not pDateEndInterval Then
          With pTser(tsIndex).dates.Summary
            CompareJday = TimAddJ(CurJday, .Tu, .ts, 1)
          End With
        Else
          CompareJday = CurJday
        End If
        
        If Abs(CompareJday - ptsJday) < JulianSecond Then
          ColValue = FormatColValue(tsIndex, pTserEdited(tsIndex).Value(dayIndex(tsIndex)))
          If delimiter <> "" Then ColValue = Trim(ColValue) Else ColValue = " " & ColValue
          outstr = outstr & ColValue
          dayIndex(tsIndex) = dayIndex(tsIndex) + 1
        Else
          outstr = outstr & emptyCell
          If CompareJday > ptsJday Then
            dayIndex(tsIndex) = dayIndex(tsIndex) + 1
            GoTo DoDay
          End If
        End If
      End If
      If tsIndex < nts Then outstr = outstr & delimiter
    Next
    GoSub PrintOut
  Next
  If OutFile = 0 Then
    Printer.EndDoc
  Else
    Close #OutFile
  End If
  
  Exit Sub
  
PrintOut:
  If OutFile = 0 Then
    Printer.Print PrintMargin & outstr
  Else
    Print #OutFile, outstr
  End If
  outstr = ""
  Return
  
SaveError:
  MsgBox "Error saving " & FileName & vbCr & Err.Description, vbExclamation, "Save to Text File"
NeverMind:
End Sub

Private Function FormatColValue(col As Long, Value As Single)
  Dim retval As String
  'If tsCol > 0 And tsCol <= nts Then
    F90_DECCHX Value, FWid(col), SDig(col), dpla(col), retval
  'End If
  FormatColValue = retval
End Function

Private Sub mnuFileSaveText_Click()
  frmTSPrintSave.Show
  frmTSPrintSave.SaveGrid Me
End Sub
  
'  Dim Filename As String
'  Dim outf As Integer
'  Dim r&, c&
'
'  Dim index As Long, NumVals As Long
'  Dim CurJday As Double
'  Dim ptsJday As Double
'  Dim CompareJday As Double
'  Dim tsIndex As Long
'  Dim dayIndex() As Long, LastDay() As Long, CIntvl() As Boolean
'
'  cdlg.DialogTitle = "Save to tab-delimited text file"
'  On Error GoTo NeverMind
'  cdlg.ShowSave
'  Filename = cdlg.Filename
'  On Error GoTo SaveError
'  outf = FreeFile
'  Open Filename For Output As #outf
'  For r = 1 To agd.FixedRows
'    For c = 0 To agd.cols
'      Print #outf, agd.TextMatrix(r - agd.FixedRows, c);
'      If c < agd.cols Then Print #outf, vbTab;
'    Next
'    Print #outf, vbCrLf;
'  Next
'
'  ReDim dayIndex(nts)
'  ReDim LastDay(nts)
'  ReDim CIntvl(nts)
'
'  For tsIndex = 1 To nts
'    CIntvl(tsIndex) = pTser(tsIndex).dates.Summary.CIntvl
'    LastDay(tsIndex) = pTser(tsIndex).dates.Summary.NVALS
'    If CIntvl(tsIndex) and not pDateEndInterval Then
'      dayIndex(tsIndex) = pTser(tsIndex).dates.IndexAtOrAfter(CurJday)
'    Else
'      dayIndex(tsIndex) = pTser(tsIndex).dates.IndexAtOrAfter(CurJday)
'    End If
'  Next
'
'  For r = 1 To nJdays
'    CurJday = rowJday(r)
'    Print #outf, DateString(CurJday) & vbTab;
'
'    For tsIndex = 1 To nts
'DoDay:
'      If dayIndex(tsIndex) <= LastDay(tsIndex) Then
'        ptsJday = pTser(tsIndex).dates.Value(dayIndex(tsIndex))
'        If CIntvl(tsIndex) and not pDateEndInterval Then
'          With pTser(tsIndex).dates.Summary
'            CompareJday = TimAddJ(CurJday, .Tu, .ts, 1)
'          End With
'        Else
'          CompareJday = CurJday
'        End If
'
'        If Abs(CompareJday - ptsJday) < JulianSecond Then
'          Print #outf, pTserEdited(tsIndex).Value(dayIndex(tsIndex));
'          dayIndex(tsIndex) = dayIndex(tsIndex) + 1
'        ElseIf CompareJday > ptsJday Then
'          dayIndex(tsIndex) = dayIndex(tsIndex) + 1
'          GoTo DoDay
'        End If
'      End If
'      If tsIndex < nts Then Print #outf, vbTab; Else Print #outf, vbCrLf;
'    Next
'  Next
'  Close #outf
'  Exit Sub
'SaveError:
'  MsgBox "Error saving " & Filename & vbCr & Err.Description, vbExclamation, "Save to Text File"
'NeverMind:

Private Sub TSnew_CreatedTser(newTS As ATCData.ATCclsTserData)
  Me.Add newTS
  edited(nts) = False
  Set pTserEdited(nts) = newTS
  MergeDates
  If pCancelPressed Then
    Unload Me
  Else
    PopulateGrid
    agd.ColsSizeByContents
    agd.ColsSizeToWidth
  End If
  RaiseCreatedTser newTS
End Sub

'Public Sub PopulateGrid()
'  Dim index As Long, NumVals As Long
'  Dim CurDate(5) As Long, dateStr As String
'  Dim ts As ATCclsTserData
'  'Dim row As Long, y As Long
'
'  If pTser Is Nothing Then Exit Sub
'
'  With agd
'
'    .rows = 0
'    .rows = NumVals
'    Set ts = pTser(1)
'    NumVals = ts.Dates.Summary.NVALS
'    .Header = ts.Header.Desc
'    For index = 1 To NumVals
'      J2Date ts.Dates.Value(index), CurDate
'      'dateStr = Format(CurDate(1), "00") & "/" & Format(CurDate(2), "00") & "/" & Right(CStr(CurDate(0)), 2)
'      dateStr = CurDate(0) & "/" & Format(CurDate(1), "00") & "/" & Format(CurDate(2), "00")
'      dateStr = dateStr & " " & Format(CStr(CurDate(3)), "00") & ":" & Format(CStr(CurDate(4)), "00")
'
'      .TextMatrix(index, 0) = dateStr
'      .TextMatrix(index, 1) = ts.Value(index)
'    Next
'  End With
'  Me.Visible = True
'End Sub

Private Sub VScroll_Change()
  'If nJdays < 32000 Then
    topRow = vScroll.Value
  'Else
  '  topRow = vScroll.Value / 32000 * nJdays
  'End If
  PopulateGrid
End Sub
