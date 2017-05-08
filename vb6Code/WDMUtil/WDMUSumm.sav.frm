VERSION 5.00
Begin VB.Form frmWDMUSumm 
   Caption         =   "Summarize Data"
   ClientHeight    =   6804
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   6828
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   45
   Icon            =   "WDMUSumm.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6804
   ScaleWidth      =   6828
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog cdlSummary 
      Left            =   120
      Top             =   6360
      _ExtentX        =   677
      _ExtentY        =   677
      _Version        =   393216
      CancelError     =   -1  'True
      DialogTitle     =   "Save Summary As"
      Filter          =   "All files (*.*)|*.*"
   End
   Begin VB.CommandButton cmdSaveSumm 
      Caption         =   "Save Summary"
      Height          =   372
      Left            =   2880
      TabIndex        =   9
      Top             =   6360
      Width           =   1572
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "Close"
      Height          =   372
      Left            =   4800
      TabIndex        =   8
      Top             =   6360
      Width           =   1212
   End
   Begin VB.CommandButton cmdSumm 
      Caption         =   "Perform Summary"
      Height          =   372
      Left            =   840
      TabIndex        =   7
      Top             =   6360
      Width           =   1692
   End
   Begin VB.Frame fraSpec 
      Caption         =   "Specifications"
      Height          =   1692
      Left            =   120
      TabIndex        =   5
      Top             =   120
      Width           =   6612
      Begin ATCoCtl.ATCoGrid agdSpec 
         Height          =   1332
         Left            =   120
         TabIndex        =   6
         Top             =   240
         Width           =   6372
         _ExtentX        =   11240
         _ExtentY        =   2350
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   -1  'True
         Rows            =   3
         Cols            =   2
         ColWidthMinimum =   300
         gridFontBold    =   0   'False
         gridFontItalic  =   0   'False
         gridFontName    =   "MS Sans Serif"
         gridFontSize    =   8
         gridFontUnderline=   0   'False
         gridFontWeight  =   400
         gridFontWidth   =   0
         Header          =   "Specify Missing Value and Distribution Indicators; Faulty Value Min/Max"
         FixedRows       =   1
         FixedCols       =   0
         ScrollBars      =   3
         SelectionMode   =   0
         BackColor       =   -2147483643
         ForeColor       =   -2147483640
         BackColorBkg    =   -2147483632
         BackColorSel    =   -2147483635
         ForeColorSel    =   16777215
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   -2147483630
         InsideLimitsBackground=   -2147483643
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         ComboCheckValidValues=   -1  'True
      End
   End
   Begin VB.Frame fraSummary 
      Caption         =   "Summary"
      Enabled         =   0   'False
      Height          =   2052
      Left            =   120
      TabIndex        =   1
      Top             =   4200
      Width           =   6612
      Begin ATCoCtl.ATCoGrid agdSumm 
         Height          =   732
         Left            =   120
         TabIndex        =   3
         Top             =   360
         Visible         =   0   'False
         Width           =   6372
         _ExtentX        =   11240
         _ExtentY        =   1291
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
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
      Begin VB.Label lblSumm 
         Caption         =   "<-Missing Values->Missing Distributions<-Faulty Values->"
         Height          =   252
         Left            =   1680
         TabIndex        =   4
         Top             =   120
         Visible         =   0   'False
         Width           =   4812
      End
   End
   Begin VB.Frame fraDetails 
      Caption         =   "Details"
      Enabled         =   0   'False
      Height          =   2172
      Left            =   120
      TabIndex        =   0
      Top             =   1920
      Width           =   6612
      Begin VB.TextBox txtDetails 
         BackColor       =   &H8000000A&
         Enabled         =   0   'False
         Height          =   1812
         Left            =   120
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   2
         Top             =   240
         Width           =   6372
      End
   End
End
Attribute VB_Name = "frmWDMUSumm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
      
Private Type MissingDataInfo
  MisId As Long 'time series id for missing period
  MisCod As Long 'type of missing data (1-values, 2-distribution, 3-faulty)
  MSDate(5) As Long 'start date for missing period
  MEDate(5) As Long 'end date for missing period
  NMisVl As Long 'count of missing time intervals
  MisVal As Single 'cummulative values for missing distribution (-1.0E30 for missing or faulty data)
End Type
Dim MisInf() As MissingDataInfo
Dim nmp As Long 'total number of missing periods
'missing value, missing distrib, faulty min/max indicators
Dim VMis!(), VDis!(), FMin!(), FMax!()


Private Sub cmdClose_Click()

  Unload frmWDMUSumm

End Sub

Private Sub cmdSaveSumm_Click()

  Dim i&, j&, fun%

  On Error GoTo 10
  cdlSummary.flags = &H3804& 'create & not read only
  cdlSummary.ShowSave
  fun = FreeFile(0)
  Open cdlSummary.Filename For Output As #fun
  Print #fun, txtDetails.Text
  Print #fun,
  Print #fun, "DSN/ID", "Increments", "Periods", "Total", "Periods", "Total", "Periods", "Total"
  For i = 1 To agdSumm.Rows
    For j = 0 To agdSumm.cols - 1
      Print #fun, agdSumm.TextMatrix(i, j),
    Next j
    Print #fun,
  Next i
  Close #fun
10 'get here on Cancel

End Sub

Private Sub cmdSumm_Click()

  Dim i&, j&, failfg As Boolean

  failfg = False
  MousePointer = ccHourglass
  fraDetails.Enabled = True
  txtDetails.Enabled = True
  fraSummary.Enabled = True
  lblSumm.Visible = True
  agdSumm.Visible = True
  For i = 1 To agdSpec.Rows
    For j = 3 To 6
      If Not IsNumeric(agdSpec.TextMatrix(i, j)) Then
        'no value entered for field
        failfg = True
      End If
    Next j
    If Not failfg Then
      VMis(i) = CSng(agdSpec.TextMatrix(i, 3))
      VDis(i) = CSng(agdSpec.TextMatrix(i, 4))
      FMin(i) = CSng(agdSpec.TextMatrix(i, 5))
      FMax(i) = CSng(agdSpec.TextMatrix(i, 6))
    End If
  Next i
  If Not failfg Then 'do summary, then allow it to be saved
    Call MissDataSumm
    cmdSaveSumm.Enabled = True
  Else
    MsgBox "All values for indicating Missing Values, Missing Distributions, and Faulty Data have not been specified." & "These values must be properly defined before summary may be performed.", vbExclamation, "WDMUtil Summary Problem"
  End If
  MousePointer = ccDefault

End Sub

Private Sub Form_Load()
  Dim i&
  Dim STSer As Collection
  Set STSer = frmGenScn.TSerSelected
  
  ReDim VMis(STSer.Count)
  ReDim VDis(STSer.Count)
  ReDim FMin(STSer.Count)
  ReDim FMax(STSer.Count)
  For i = 1 To STSer.Count
    VMis(i) = STSer(i).attrib("MVal", -999)
    VDis(i) = STSer(i).attrib("MAcc", -998)
    If VMis(i) < 0 Then 'set faulty min to just below missing indicator
      FMin(i) = Fix(VMis(i) - 1)
    Else 'set faulty min to big negative value
      FMin(i) = -1000
    End If
    FMax(i) = 10000
  Next i
  With agdSpec
    .Rows = STSer.Count
    .ColTitle(0) = "DSN/ID"
    .ColEditable(0) = False
    .ColTitle(1) = "Location"
    .ColEditable(1) = False
    .ColTitle(2) = "Constituent"
    .ColEditable(2) = False
    .ColTitle(3) = "Miss. Val."
    .ColEditable(3) = True
    .ColType(3) = ATCoTxt
    .ColTitle(4) = "Miss. Dist."
    .ColEditable(4) = True
    .ColType(4) = ATCoTxt
    .ColTitle(5) = "Faulty Min"
    .ColEditable(5) = True
    .ColType(5) = ATCoTxt
    .ColTitle(6) = "Faulty Max"
    .ColEditable(6) = True
    .ColType(6) = ATCoTxt
    For i = 1 To STSer.Count
      .TextMatrix(i, 0) = STSer(i).Header.id
      .TextMatrix(i, 1) = STSer(i).Header.Loc
      .TextMatrix(i, 2) = STSer(i).Header.Con
      If VMis(i) <> 0 Then .TextMatrix(i, 3) = VMis(i)
      If VDis(i) <> 0 Then .TextMatrix(i, 4) = VDis(i)
      .TextMatrix(i, 5) = FMin(i)
      .TextMatrix(i, 6) = FMax(i)
    Next i
    If STSer.Count = 1 Then 'set height for 1 row
      .Height = 972
    ElseIf STSer.Count < 7 Then 'set height for multiple rows
      .Height = 972 + (STSer.Count - 1) * 240
    Else 'scroll after 10 rows
      .Height = 2172
    End If
    fraSpec.Height = .Height + 360
  End With
  fraDetails.Top = fraSpec.Top + fraSpec.Height + 108
  fraSummary.Top = fraDetails.Top + fraDetails.Height + 108
  With agdSumm
    .Rows = STSer.Count
    .ColTitle(0) = "DSN/ID"
    .ColTitle(1) = "Increments"
    .ColTitle(2) = "Periods"
    .ColTitle(3) = "Total"
    .ColTitle(4) = "Periods"
    .ColTitle(5) = "Total"
    .ColTitle(6) = "Periods"
    .ColTitle(7) = "Total"
    If STSer.Count = 1 Then 'set height for 1 row
      .Height = 492
    ElseIf STSer.Count < 7 Then 'set height for multiple rows
      .Height = 492 + (STSer.Count - 1) * 240
    Else 'scroll after 7 rows
      .Height = 1932
    End If
    fraSummary.Height = .Height + 480
  End With
  cmdSumm.Top = fraSummary.Top + fraSummary.Height + 108
  cmdSaveSumm.Top = cmdSumm.Top
  cmdSaveSumm.Enabled = False
  cmdClose.Top = cmdSumm.Top
  frmWDMUSumm.Height = cmdClose.Top + cmdClose.Height + 456

End Sub
Private Sub MissDataSumm()

  Dim i&, j&, ldate&(5), iMisPd&, tnvals&
  Dim nv&, mpos&, MCod&, NMVals&, lpos&
  Dim NMVPd&, NMDPd&, NBVPd&, TotMV&, TotMD&, TotBV&
  Dim lbuff!(), MVal!, Sdt#, Edt#
  Dim DateStr$, s$, TSStr$, TUStr$(6)
  Dim vSTSer As Variant, lSTSer As ATCclsTserData
  Dim lTser As ATCclsTserData
  Dim STSer As Collection
  Set STSer = frmGenScn.TSerSelected
  
  'TUStr = Array("seconds", "minutes", "hours", "days", "months", "years")
  TUStr(1) = "seconds"
  TUStr(2) = "minutes"
  TUStr(3) = "hours"
  TUStr(4) = "days"
  TUStr(5) = "months"
  TUStr(6) = "years"

  'build Julian start/end dates
  Sdt = Date2J(CSDat())
  Edt = Date2J(CEDat())
  'init missing period counter and output text box
  nmp = 0
  txtDetails.Text = ""
  i = 1
  For Each vSTSer In STSer
    Set lSTSer = vSTSer
    Set lTser = lSTSer.SubSetByDate(Sdt, Edt)
    s = "For Data-set number " & lTser.Header.id & " (" & lTser.Header.Sen & " " & lTser.Header.Loc & " " & lTser.Header.Con & ")" & vbCrLf
    'build time step/units string for display
    TSStr = TUStr(lTser.dates.Summary.Tu) 'start with time units
    If lTser.dates.Summary.ts > 1 Then 'need time step too
      TSStr = lTser.dates.Summary.ts & " " & TSStr
      'remove plural from time units, add 'increments'
      TSStr = Left(TSStr, Len(TSStr) - 1) & " increments"
    End If
    iMisPd = 0
    MCod = 1
    mpos = 1
    Do While MCod > 0
      nv = lTser.dates.Summary.NVALS - mpos + 1
      If nv > 0 Then 'values in buffer to check
        ReDim lbuff(nv)
        For j = mpos To lTser.dates.Summary.NVALS
          lbuff(j - mpos + 1) = lTser.Value(j)
        Next j
        Call NxtMis(nv, lbuff(), VMis(i), VDis(i), FMin(i), FMax(i), MCod, lpos, NMVals, MVal)
        If MCod > 0 Then 'somethings missing
          mpos = mpos + lpos - 1
          If mpos > 0 Then 'determine date
            If lTser.dates.Summary.Tu > TUHour Then
              Call F90_TIMADD(CSDat(0), lTser.dates.Summary.Tu, lTser.dates.Summary.ts, mpos - 1, ldate(0))
            Else
              Call F90_TIMADD(CSDat(0), lTser.dates.Summary.Tu, lTser.dates.Summary.ts, mpos, ldate(0))
            End If
          Else 'must be at the start
            For j = 0 To 5
              ldate(j) = CSDat(j)
            Next j
          End If
          If lTser.dates.Summary.Tu < 4 Then 'display h,m,s
            DateStr = ldate(0) & "/" & ldate(1) & "/" & ldate(2) & " " & ldate(3) & ":" & ldate(4) & ":" & ldate(5)
          Else 'just display day
            DateStr = ldate(0) & "/" & ldate(1) & "/" & ldate(2)
          End If
          If MCod = 1 Then 'missing values
            s = s & "  " & NMVals & " " & TSStr & " of missing values starting " & DateStr & vbCrLf
          ElseIf MCod = 2 Then 'accumulated values
            s = s & "  " & NMVals & " " & TSStr & " of missing time distribution starting " & DateStr & vbCrLf
          ElseIf MCod = 3 Then 'screwball values
            s = s & "  " & NMVals & " " & TSStr & " of faulty values starting " & DateStr & vbCrLf
          End If
          'update missing data info structure
          ReDim Preserve MisInf(nmp)
          MisInf(nmp).MisId = lTser.Header.id
          MisInf(nmp).MisCod = MCod
          MisInf(nmp).NMisVl = NMVals
          MisInf(nmp).MisVal = MVal
          For j = 0 To 5
            MisInf(nmp).MSDate(j) = ldate(j)
          Next j
          Call F90_TIMADD(ldate(0), lTser.dates.Summary.Tu, lTser.dates.Summary.ts, NMVals, MisInf(nmp).MEDate(0))
          nmp = nmp + 1
          iMisPd = iMisPd + 1
          mpos = mpos + NMVals
        End If
      Else 'no values left to scan
        MCod = 0
      End If
    Loop
    If mpos > 0 Then 'missing data found for this time series
      s = s & iMisPd & " period(s) of missing or bad data." & vbCrLf
    Else 'nothing missing
      s = s & "  No missing data for this time series" & vbCrLf
    End If
    txtDetails.Text = txtDetails.Text & s & vbCrLf
    i = i + 1
  Next
  For i = 1 To STSer.Count
    agdSumm.TextMatrix(i, 0) = STSer(i).Header.id
    Call F90_TIMDIF(CSDat(0), CEDat(0), STSer(i).dates.Summary.Tu, STSer(i).dates.Summary.ts, tnvals)
    agdSumm.TextMatrix(i, 1) = tnvals
    NMVPd = 0
    TotMV = 0
    NMDPd = 0
    TotMD = 0
    NBVPd = 0
    TotBV = 0
    For j = 0 To nmp - 1
      'look through missing data for this data set
      If MisInf(j).MisId = STSer(i).Header.id Then
        'match for this data set, add to summary
        If MisInf(j).MisCod = 1 Then
          'missing values
          NMVPd = NMVPd + 1
          TotMV = TotMV + MisInf(j).NMisVl
        ElseIf MisInf(j).MisCod = 2 Then
          'missing distribution
          NMDPd = NMDPd + 1
          TotMD = TotMD + MisInf(j).NMisVl
        ElseIf MisInf(j).MisCod = 3 Then
          'faulty data
          NBVPd = NBVPd + 1
          TotBV = TotBV + MisInf(j).NMisVl
        End If
      End If
    Next j
    With agdSumm
      .TextMatrix(i, 2) = NMVPd
      .TextMatrix(i, 3) = TotMV
      .TextMatrix(i, 4) = NMDPd
      .TextMatrix(i, 5) = TotMD
      .TextMatrix(i, 6) = NBVPd
      .TextMatrix(i, 7) = TotBV
    End With
  Next i

End Sub

Public Sub NxtMis(BufSiz&, DBuff!(), ValMis!, ValAcc!, FaultMin!, FaultMax!, MisCod&, MisPos&, NVALS&, MVal!)

  'Find the next missing value or distribution or screwball value
  'in the data buffer (DBUFF).  If none found, return MISCOD = 0.

  'BUFSIZ - size of data buffer array
  'DBUFF  - array of data values in which to look for missing data
  'VALMIS - value indicating missing data values
  'VALACC - value indicating accumulated values (missing distribution)
  'FaultMin - value indicating value is too low to be valid
  'FaultMax - value indicating value is too high to be valid
  'MISCOD - type of missing data
  '         1 - missing value
  '         2 - missing distribution
  '         3 - garbage value
  'MISPOS - position in array where missing data starts
  'NVALS  - number of data values missing
  'MVAL   - accumulated value (for accumulated data) or dummy
  '         for other types of missing data
    
  Dim i&, MisFlg&, AccFlg&, BadFlg&, DonFlg&
    
  MisFlg = 0
  AccFlg = 0
  BadFlg = 0
  DonFlg = 0

  i = 0
  Do While DonFlg = 0 And i < BufSiz
    'check next data value
    i = i + 1
    If Abs(DBuff(i) - ValMis) < 0.00001 Then
      'we have a missing value
      If AccFlg > 0 Then
        'we were in the midst of an accum value array,
        'change it all to missing because dont know total
        MisFlg = AccFlg
        AccFlg = 0
      End If
      MisFlg = MisFlg + 1
      If MisFlg = 1 Then
        'first missing value, save start position
        MisPos = i
      End If
    ElseIf Abs(DBuff(i) - ValAcc) < 0.00001 Then
      'we have an accumulated value
      If MisFlg > 0 Then
        'report only missing values now, will get accum next time
        DonFlg = 1
      Else
        'accumulated value w/no distribution
        AccFlg = AccFlg + 1
        If AccFlg = 1 Then
          'first accumulated value, save start position
          MisPos = i
        End If
      End If
    ElseIf DBuff(i) < FaultMin Or DBuff(i) > FaultMax Then
      'we have a screwball value
      BadFlg = BadFlg + 1
      If BadFlg = 1 Then
        'first bad value, save start position
        MisPos = i
      End If
    Else
      'no problem with this value
      If MisFlg > 0 Or AccFlg > 0 Or BadFlg > 0 Then
        'end of missing period
        DonFlg = 1
        If AccFlg > 0 Then
          'include first good value in accumulated count
          AccFlg = AccFlg + 1
        End If
      End If
    End If
  Loop

  If MisFlg > 0 Then
    'missing values
    MisCod = 1
    NVALS = MisFlg
    MVal = -1E+30
  ElseIf AccFlg > 0 Then
    'missing distribution
    MisCod = 2
    NVALS = AccFlg
    'save accumulated value for possible distribution later
    MVal = DBuff(i)
  ElseIf BadFlg > 0 Then
    'screwball values
    MisCod = 3
    NVALS = BadFlg
    MVal = -1E+30
  Else
    'no missing data in buffer
    MisCod = 0
    MisPos = 0
    NVALS = 0
    MVal = 0#
  End If

End Sub
