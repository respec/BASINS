VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Begin VB.Form frmWDMUWrite 
   Caption         =   "Write to WDM"
   ClientHeight    =   2970
   ClientLeft      =   2685
   ClientTop       =   4785
   ClientWidth     =   9105
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   87
   LinkTopic       =   "Form1"
   ScaleHeight     =   2970
   ScaleWidth      =   9105
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   2760
      TabIndex        =   4
      Top             =   2520
      Width           =   2772
      Begin VB.CommandButton cmdWrite 
         Caption         =   "&Write"
         Height          =   372
         Left            =   0
         TabIndex        =   6
         Top             =   0
         Width           =   1212
      End
      Begin VB.CommandButton cmdClose 
         Cancel          =   -1  'True
         Caption         =   "&Cancel"
         Height          =   372
         Left            =   1560
         TabIndex        =   5
         Top             =   0
         Width           =   1212
      End
   End
   Begin VB.OptionButton optDate 
      Caption         =   "Use full period for each data set"
      Height          =   252
      Index           =   1
      Left            =   120
      TabIndex        =   3
      Top             =   1320
      Width           =   3612
   End
   Begin VB.OptionButton optDate 
      Caption         =   "Use common period for all data sets, as defined on main form"
      Height          =   252
      Index           =   0
      Left            =   120
      TabIndex        =   2
      Top             =   1080
      Value           =   -1  'True
      Width           =   5772
   End
   Begin ATCoCtl.ATCoGrid agdWrite 
      Height          =   735
      Left            =   120
      TabIndex        =   1
      Top             =   1680
      Width           =   7935
      _ExtentX        =   13996
      _ExtentY        =   1296
      SelectionToggle =   0   'False
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
      Rows            =   1
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
      ScrollBars      =   2
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483632
      BackColorSel    =   -2147483634
      ForeColorSel    =   16777215
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
   Begin VB.Label lblWrite 
      Caption         =   $"WDMUWrit.frx":0000
      Height          =   855
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   7815
   End
End
Attribute VB_Name = "frmWDMUWrite"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim TGs As Variant

Private Sub agdWrite_RowColChange()

  Dim i&

  With agdWrite
    .ClearValues
    If .col = 2 Then 'add scenarios
      For i = 0 To p.ScenCount - 1
        .addValue p.Scen(i).Name
      Next i
    ElseIf .col = 3 Then 'add locations
      For i = 0 To p.LocnCount - 1
        .addValue p.Locn(i).Name
      Next i
    ElseIf .col = 4 Then 'add constituents
      For i = 0 To p.ConsCount - 1
        .addValue p.Cons(i).Name
      Next i
    ElseIf .col = 8 Then 'add time group options
      For i = 0 To UBound(TGs)
        .addValue CStr(TGs(i))
      Next i
    End If
  End With

End Sub

Private Sub cmdClose_Click()

  Unload frmWDMUWrite

End Sub

Private Sub cmdWrite_Click()

  Dim i&, j&, k&, outdsn&, retcod&, ExistAction&
  Dim failfg As Boolean, S$, lsen$, lcon$, lloc$
  Dim NoScen As Boolean, NoCons As Boolean, NoLocn As Boolean
  Dim nsa As Long, nsasp As Long, ndp As Long, TGrId As Long
  Dim TGroup As String, TsBYr As String
  Dim lTsData As ATCclsTserData
  Dim lTsFile As ATCclsTserFile
  Dim SJDt#, EJDt#
  Dim STSer As Collection
  Const QualFg& = 0

  On Error Resume Next

  Set STSer = frmGenScn.TSerSelected

  ExistAction = TsIdReplAsk + TsIdAppendAsk + TsIdRenumAsk
  failfg = False
  NoScen = False
  NoCons = False
  NoLocn = False
  For i = 1 To STSer.Count
    If Not IsNumeric(agdWrite.TextMatrix(i, 1)) Then
      failfg = True
      MsgBox "Valid data-set numbers have not been specified for all time series." & vbCrLf & "Enter a data-set number (1-9999) for each time series.", 48, "WDMUtil Write Problem"
    ElseIf Len(agdWrite.TextMatrix(i, 2)) = 0 Then 'scenario not defined
      NoScen = True
    ElseIf Len(agdWrite.TextMatrix(i, 3)) = 0 Then 'location not defined
      NoLocn = True
    ElseIf Len(agdWrite.TextMatrix(i, 4)) = 0 Then 'constituent not defined
      NoCons = True
    End If
    If Not failfg Then 'now check attrib/data space values
      nsa = CLng(agdWrite.TextMatrix(i, 5))
      nsasp = CLng(agdWrite.TextMatrix(i, 6))
      ndp = CLng(agdWrite.TextMatrix(i, 7))
      TGroup = agdWrite.TextMatrix(i, 8)
      TsBYr = CLng(agdWrite.TextMatrix(i, 9))
      j = 0
      TGrId = 0
      While j <= UBound(TGs)
        If TGroup = TGs(j) Then
          TGrId = j + 3
          j = UBound(TGs)
        End If
        j = j + 1
      Wend
      If nsa < 10 Or nsasp < 10 Or ndp < 10 Then
        MsgBox "For new Data-set number " & agdWrite.TextMatrix(i, 1) & "," & vbCrLf & _
               "invalid values entered for Number of Search Attributes, Search Space, or Number of Data Pointers", vbInformation, "WDMUtil Write to WDM"
        failfg = True
      ElseIf 2 * nsa + nsasp + ndp > 473 Then
        MsgBox "For new Data-set number " & agdWrite.TextMatrix(i, 1) & "," & vbCrLf & _
               "the values for Number of Search Attributes, Search Attribute Space, and Number of Data Pointers are too large." & vbCrLf & _
               "The sum total of 2*NSA + NSASP + NDP must not exceed 473.", vbInformation, "WDMUtil Write to WDM"
        failfg = True
      ElseIf TGrId = 0 Then
        MsgBox "For new Data-set number " & agdWrite.TextMatrix(i, 1) & "," & vbCrLf & _
               "invalid value entered for Time Group." & vbCrLf & _
               "Select a valid Time Group value from the pull-down list.", vbInformation, "WDMUtil Write to WDM"
        failfg = True
      ElseIf TsBYr < 1 Or TsBYr > CSDat(0) Then
        MsgBox "For new Data-set number " & agdWrite.TextMatrix(i, 1) & "," & vbCrLf & _
               "invalid value entered for Base Year." & vbCrLf & _
               "Base year should be greater than 1 and less that the starting year of the data being written.", vbInformation, "WDMUtil Write to WDM"
        failfg = True
      End If
    End If
  Next i
  If NoScen Or NoLocn Or NoCons Then
    j = MsgBox("A Scenario, Location, and Constituent have not all been specified for the data sets being saved." & _
               "Scenario, Location, and Constituent names are very useful for selecting desired data sets in WDMUtil." & _
               "Are you sure you want to leave these unspecified?", _
               vbApplicationModal + vbYesNo + vbInformation, "WDMUtil Write Problem")
    If j <> vbYes Then failfg = True
  End If
  
  If Not failfg Then 'ok to write to WDM
    S = ""
    Set lTsFile = TserFiles.Active(2).obj 'WDM assumed to be in 2nd position
    For i = 1 To STSer.Count
      If optDate(0).Value = True Then
        'use dates from main form as period to write
        SJDt = Date2J(CSDat)
        EJDt = Date2J(CEDat)
        Set lTsData = STSer(i).SubSetByDate(SJDt, EJDt)
      Else 'use full period of data set as period to write
        Set lTsData = STSer(i).Copy
      End If
      lTsData.Header.id = CLng(agdWrite.TextMatrix(i, 1))
      lTsData.Header.Sen = agdWrite.TextMatrix(i, 2)
      lTsData.Header.Loc = agdWrite.TextMatrix(i, 3)
      lTsData.Header.Con = agdWrite.TextMatrix(i, 4)
      nsa = CLng(agdWrite.TextMatrix(i, 5))
      nsasp = CLng(agdWrite.TextMatrix(i, 6))
      ndp = CLng(agdWrite.TextMatrix(i, 7))
      TGroup = agdWrite.TextMatrix(i, 8)
      TsBYr = CLng(agdWrite.TextMatrix(i, 9))
      j = 0
      While j <= UBound(TGs)
        If TGroup = TGs(j) Then
          TGrId = j + 3
          j = UBound(TGs)
        End If
        j = j + 1
      Wend
      lTsData.AttribSet "NDN", "1"
      lTsData.AttribSet "NUP", "1"
      lTsData.AttribSet "NSA", CStr(nsa)
      lTsData.AttribSet "NSASP", CStr(nsasp)
      lTsData.AttribSet "NDP", CStr(ndp)
      lTsData.AttribSet "TGROUP", CStr(TGrId)
      lTsData.AttribSet "TSBYR", CStr(TsBYr)
      'create data set
      If lTsFile.addtimser(lTsData, ExistAction) Then 'dsn built fine
        For j = 1 To InMemFile.DataCount 'remove in memory version
          If InMemFile.Data(j).Serial = STSer(i).Serial Then
            If Not STSer(i).File.RemoveTimSer(STSer(i)) Then
              MsgBox "Could not delete external Time Series (File: " & STSer(i).File.Filename & ").", vbExclamation, "WDMUtil Delete Time Series Problem"
            End If
            Exit For
          End If
        Next j
      Else 'problem writing dsn
      End If
    Next i
    Call RefreshSLC
    Call frmGenScn.RefreshMain
    Call frmGenScn.SelectAll
    Unload frmWDMUWrite
  End If

End Sub

Private Sub Form_Load()
  Dim i&, j&, CSDat&(6), iVal As Long
  Dim STSer As Collection
  
  TGs = Array("Hours", "Days", "Months", "Years", "Centuries")

  Set STSer = frmGenScn.TSerSelected

  lblWrite.Caption = "Specify Output Data-set Number(s);" & vbCrLf & "Select/Enter Scenario, Location, Constituent as needed;" & vbCrLf & "Data-set attributes may be updated if needed" & vbCrLf & "Click Write button to store data on WDM file."
  With agdWrite
    .ComboCheckValidValues = False
    .ColTitle(0) = "DSN/ID"
    .ColTitle(1) = "Output DSN"
    .ColEditable(1) = True
    .ColType(1) = ATCoInt
    .ColMin(1) = 1
    .ColMax(1) = 9999
    .ColTitle(2) = "Scenario"
    .ColEditable(2) = True
    .ColTitle(3) = "Location"
    .ColEditable(3) = True
    .ColTitle(4) = "Constituent"
    .ColEditable(4) = True
    .ColTitle(5) = "# Attributes"
    .ColEditable(5) = True
    .ColMin(5) = 10
    .ColMax(5) = 70
    .ColTitle(6) = "Attr. Space"
    .ColEditable(6) = True
    .ColMin(5) = 10
    .ColMax(5) = 200
    .ColTitle(7) = "# Data Pointers"
    .ColEditable(7) = True
    .ColMin(7) = 10
    .ColMax(7) = 400
    .ColTitle(8) = "Time Group"
    .ColEditable(8) = True
    .ColTitle(9) = "Base Year"
    .ColEditable(9) = True
    .Rows = STSer.Count
    For i = 1 To STSer.Count
      .TextMatrix(i, 0) = STSer(i).Header.id
      .TextMatrix(i, 2) = STSer(i).Header.Sen
      .TextMatrix(i, 3) = STSer(i).Header.Loc
      .TextMatrix(i, 4) = STSer(i).Header.Con
      .TextMatrix(i, 5) = "30"
      .TextMatrix(i, 6) = "100"
      .TextMatrix(i, 7) = "300"
      .TextMatrix(i, 8) = "Years"
      Call J2Date(STSer(i).dates.Summary.SJDay, CSDat)
      j = CSDat(0) Mod 10
      If j > 0 Then 'subtract back to start of this decade
        iVal = CSDat(0) - j
      Else 'back to start of previous decade
        iVal = CSDat(0) - 10
      End If
      .TextMatrix(i, 9) = CStr(iVal)
    Next i
    If STSer.Count = 1 Then 'set height for 1 row
      .Height = 492
    Else 'set height for multiple rows
      .Height = 492 + (STSer.Count - 1) * 240
    End If
    .ColsSizeByContents
  End With
  fraButtons.Top = agdWrite.Top + agdWrite.Height + 228
  frmWDMUWrite.Height = fraButtons.Top + fraButtons.Height + 500
End Sub

Private Sub Form_Resize()
  Dim w As Single, h As Single
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > 2000 And h > 2350 Then
    agdWrite.Width = w - 336
    agdWrite.Height = h - 2238
    fraButtons.Top = h - 468
    fraButtons.Left = (w - fraButtons.Width) / 2
  End If
End Sub
