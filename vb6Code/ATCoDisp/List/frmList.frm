VERSION 5.00
Begin VB.Form frmL 
   Appearance      =   0  'Flat
   BackColor       =   &H80000016&
   Caption         =   "List"
   ClientHeight    =   4236
   ClientLeft      =   2520
   ClientTop       =   3192
   ClientWidth     =   7236
   BeginProperty Font 
      Name            =   "Courier"
      Size            =   9.6
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   ForeColor       =   &H80000008&
   HelpContextID   =   10100
   Icon            =   "frmList.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   4236
   ScaleWidth      =   7236
   Tag             =   "-1"
   Visible         =   0   'False
   Begin VB.Timer Timer2 
      Enabled         =   0   'False
      Interval        =   100
      Left            =   6480
      Top             =   120
   End
   Begin VB.Timer Timer1 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   6000
      Top             =   120
   End
   Begin ATCoCtl.ATCoGrid agd 
      Height          =   4215
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   6975
      _ExtentX        =   12298
      _ExtentY        =   7430
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
      Header          =   "lblHeader"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
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
   Begin MSComDlg.CommonDialog cdlPrintSave 
      Left            =   5040
      Top             =   120
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      CancelError     =   -1  'True
      DialogTitle     =   "Save List File"
      Filter          =   "Text Files (*.txt)|*.txt|RDB Files (*.rdb)|*.rdb|All Files (*.*)|*.*"
      Flags           =   6
      FontSize        =   2.54052e-29
      InitDir         =   "CurDir"
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuSav 
         Caption         =   "&Save"
      End
      Begin VB.Menu mnuSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuPrint 
         Caption         =   "&Print"
      End
      Begin VB.Menu mnuSep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuClose 
         Caption         =   "&Close"
      End
   End
   Begin VB.Menu mnuEdit 
      Caption         =   "&Edit"
      Begin VB.Menu mnuEdList 
         Caption         =   "&General"
         Index           =   0
      End
      Begin VB.Menu mnuEdList 
         Caption         =   "&Fields"
         Index           =   1
      End
      Begin VB.Menu mnuEdList 
         Caption         =   "&Summaries"
         Index           =   2
      End
      Begin VB.Menu mnuEdList 
         Caption         =   "&Dates"
         Index           =   3
      End
      Begin VB.Menu mnuEdSep 
         Caption         =   "-"
      End
      Begin VB.Menu mnuEdCopy 
         Caption         =   "Copy"
         Shortcut        =   ^C
      End
      Begin VB.Menu mnuEdPaste 
         Caption         =   "Paste"
         Enabled         =   0   'False
         Shortcut        =   ^V
      End
   End
End
Attribute VB_Name = "frmL"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Public parentlst As ATCoList

'listing data buffer
Const POSMAX& = 18
Dim bufpos(2, 2 * POSMAX) As Long
Dim bufEditable(0 To POSMAX) As Boolean
Dim bufEdited(0 To POSMAX) As Boolean
Dim editingGrid As Boolean
Dim bufInCol(0 To POSMAX) As Long
Dim wchvr(2, POSMAX) As Integer
Dim yx() As Double
'listing stuff
Public Nvar As Long
Public NList As Long
Dim LType As Long '0-xy, 1-constant time, 2-non-constant time
Dim DateFmt As Long '0-yyyy MON dd hh:mm:ss, 1-yyyy.mm.dd.hh.mm.ss, 2-mm/dd/yy hh:mm:ss

Dim FWid(2 * POSMAX) As Long
Dim SDig(2 * POSMAX) As Long
Dim dpla(2 * POSMAX) As Long
Dim fmin(2 * POSMAX) As Single
Dim fmax(2 * POSMAX) As Single
Dim FInRan(2 * POSMAX) As Boolean

'Dim firstrow&, lastrow& 'Range currently displayed in ATCoGrid
Dim firstVisibleJDate#
Dim maxrows& '(for setting sb1.max) Maximum number of rows in any data column. We try to make good guesses in SetData and correct them at the end of DispList
Dim MaxDatLev& 'Max date level needed for summaries 1=sec, 6=year (see DatPrt) set in DispList, used to limit rows above top of list we have to render
Dim scrolling As Boolean 'simple lock on scrollbar
Dim DispListTimeout& 'minimum seconds to allow DispList to work before returning

Dim Title As String
Dim lbv(2 * POSMAX) As String
'list view variables
'Dim colitm As ListItem
Dim XRangeInAll As Boolean, XRangeOutAll As Boolean, XRangeInOut As Boolean
'time variables
Dim SDate&(6), EDate&(6), CurDate&(6)
Dim EndMon&, TCODE&, TSTEP&
'accumulator variables
Dim SumDat(7, POSMAX) As Single
Dim CntDat(7, POSMAX) As Single
Dim MinDat(7, POSMAX) As Single
Dim MaxDat(7, POSMAX) As Single
Dim SumChk(7) As Long
Dim AveChk(7) As Long
Dim CntChk(7) As Long
Dim MinChk(7) As Long
Dim MaxChk(7) As Long
Dim MaxSumCnt As Long
Dim Native As Boolean

Private Sub DatPrt(irec&, col&, lastflg As Boolean)

  Call DatPrtLine(irec, col, 1) 'always try seconds
  If Not (Native) Then
    If CurDate(5) = 0 Or lastflg Then  'minute boundary
      Call DatPrtLine(irec, col, 2)
      If CurDate(4) = 0 Or lastflg Then 'hour boundary
        Call DatPrtLine(irec, col, 3)
        If CurDate(3) = 24 Or lastflg Then 'day boundary
          Call DatPrtLine(irec, col, 4)
          If CurDate(2) = daymon(CurDate(0), CurDate(1)) Or lastflg Then ' month boundary
            Call DatPrtLine(irec, col, 5)
            If CurDate(1) = EndMon Or lastflg Then 'year boundary
              Call DatPrtLine(irec, col, 6)
              If lastflg Then 'span boundary
                Call DatPrtLine(irec, col, 7)
              End If
            End If
          End If
        End If
      End If
    End If
  End If

End Sub

Private Sub DatPrtLine(irec&, col&, lev&)

  Dim d$, i&
  
  If lev >= TCODE Then
    'If irec + 1 >= firstrow And irec <= lastrow Then
      If lev = 7 Then
        d = "ALL"
      ElseIf DateFmt = 0 Then 'default date format
        Call F90_DATLST(CurDate, d)
        If lev = 6 Then 'year
          d = Left(d, 4)
        ElseIf lev = 5 Then 'month
          d = Left(d, 8)
        ElseIf lev = 4 Then 'day
          d = Left(d, 11)
        ElseIf lev = 3 Then 'hour
          d = Left(d, 14)
        ElseIf lev = 2 Then 'minute
          d = Left(d, 17)
        End If
      ElseIf DateFmt = 1 Then 'yyyy.mm.dd hh:mm:ss
        d = CStr(CurDate(0))
        For i = 1 To 6 - lev
          d = d & "." & Format(CurDate(i), "00")
        Next i
      ElseIf DateFmt = 2 Then 'mm/dd/yy hh:mm:ss
        d = Right(CStr(CurDate(0)), 2)
        If lev = 5 Then 'month
          d = Format(CurDate(1), "00") & "/" & d
        Else 'day or less
          d = Format(CurDate(1), "00") & "/" & Format(CurDate(2), "00") & "/" & d
          If lev <= 3 Then 'hour or less
            d = d & " "
            For i = 3 To 6 - lev
              d = d & CStr(CurDate(i)) & ":"
            Next i
            d = Left(d, Len(d) - 1)
          End If
        End If
      End If
    'End If
    If Right(d, 3) = ":00" Then d = Left(d, Len(d) - 3)
    If Native Then
      Call DatPrtDetail(irec, col, "", lev, d)
      CntDat(lev, col) = 0
    Else
      If SumChk(lev) = 1 Then Call DatPrtDetail(irec, col, "Sum", lev, d)
      If CntChk(lev) = 1 Then Call DatPrtDetail(irec, col, "Cnt", lev, d)
      If AveChk(lev) = 1 Then Call DatPrtDetail(irec, col, "Ave", lev, d)
      If MaxChk(lev) = 1 Then Call DatPrtDetail(irec, col, "Max", lev, d)
      If MinChk(lev) = 1 Then Call DatPrtDetail(irec, col, "Min", lev, d)
      If lev > TCODE And lev < 7 Then 'accum to next level
        SumDat(lev + 1, col) = SumDat(lev + 1, col) + SumDat(lev, col)
        CntDat(lev + 1, col) = CntDat(lev + 1, col) + CntDat(lev, col)
        CntDat(lev, col) = 0
        SumDat(lev, col) = 0
        If MaxDat(lev + 1, col) < MaxDat(lev, col) Then
          MaxDat(lev + 1, col) = MaxDat(lev, col)
        End If
        MaxDat(lev, col) = -1E+30
        If MinDat(lev + 1, col) > MinDat(lev, col) Then
          MinDat(lev + 1, col) = MinDat(lev, col)
        End If
        MinDat(lev, col) = 1E+30
      ElseIf lev = TCODE Then
        CntDat(lev, col) = 0
      End If
    End If
  End If

End Sub

Sub DatPrtDetail(irec&, col&, o$, lev&, d$)

  Dim lstr$, i&, r!, s$, val$, jdkey$, tagstr$, TransType$
  Dim cjdt#, ljdt#
  'Dim DateFnd As ListItem
  Dim KeyRow&, agdrec&
  On Error GoTo errhandlerD
  'increment record
  irec = irec + 1
  
  'determine which transformation to show
  val = "*"
  TransType = Left(o, 3)
  Select Case TransType
  Case "":
    If CntDat(lev, col) > 0 Then Call F90_DECCHX(SumDat(lev, col), FWid(col), SDig(col), dpla(col), val)
    tagstr = "Z" & CStr(lev)
  Case "Sum":
    If CntDat(lev, col) > 0 Then Call F90_DECCHX(SumDat(lev, col), FWid(col), SDig(col), dpla(col), val)
    tagstr = "S" & CStr(lev)
  Case "Min":
    If CntDat(lev, col) > 0 Then Call F90_DECCHX(MinDat(lev, col), FWid(col), SDig(col), dpla(col), val)
    tagstr = "N" & CStr(lev)
  Case "Max":
    If CntDat(lev, col) > 0 Then Call F90_DECCHX(MaxDat(lev, col), FWid(col), SDig(col), dpla(col), val)
    tagstr = "X" & CStr(lev)
  Case "Cnt":
    val = NumFmtI(CLng(CntDat(lev, col)), (FWid(col)))
    tagstr = "C" & CStr(lev)
  Case Else:
    If CntDat(lev, col) > 0 Then 'average
      r = SumDat(lev, col) / CntDat(lev, col)
      Call F90_DECCHX(r, FWid(col), SDig(col), dpla(col), val)
    End If
    tagstr = "A" & CStr(lev)
  End Select
  For i = Len(val) - 1 To 0 Step -1 'remove trailing 0s
    If Right(val, 1) = "0" Then
      val = Left(val, Len(val) - 1)
    Else
      Exit For
    End If
  Next i
  'If d = "1954 APR  1 22:00:00" Then Stop
  If val <> "*" Then
    agdrec = irec
    If agdrec > agd.MaxOccupiedRow + 1 Then agdrec = agd.MaxOccupiedRow + 1
    If agdrec <= agd.MaxOccupiedRow Then 'see if this date is already in list
      cjdt = Date2J(CurDate())
      jdkey = tagstr & CStr(cjdt)
      KeyRow = agd.RowContaining(jdkey, -1)
    Else 'add new record to end
      KeyRow = -1
    End If
    If KeyRow < 0 Then 'new record, find where it should go
      'store date to be added as julian for comparison
      cjdt = Date2J(CurDate())
      jdkey = tagstr & CStr(cjdt)
      If agdrec <= agd.MaxOccupiedRow Then
        s = agd.ItemData(agdrec)
        If Len(s) > 2 Then ljdt = CDbl(Mid(s, 3)) Else ljdt = cjdt
        While cjdt > ljdt And agdrec <= agd.rows
          irec = irec + 1
          agdrec = agdrec + 1
          s = agd.ItemData(agdrec)
          If Len(s) > 2 Then ljdt = CDbl(Mid(s, 3)) Else ljdt = cjdt
        Wend
      ElseIf agd.rows < agdrec Then
        agd.rows = agd.rows * 2
      End If
      'put date and accumulator on it
      
      If agdrec <= agd.MaxOccupiedRow Then
        If agdrec = 1 Then agd.InsertRow agdrec Else agd.InsertRow agdrec - 1
        Dim clrcol&
        For clrcol = 0 To agd.cols - 1
          agd.TextMatrix(agdrec, clrcol) = ""
        Next clrcol
      End If
      agd.TextMatrix(agdrec, 0) = d
      agd.TextMatrix(agdrec, 1) = o
      agd.ItemData(agdrec) = jdkey
    Else 'adding values to existing record
      agdrec = KeyRow
      irec = KeyRow '+ firstrow - 1
    End If
    agd.TextMatrix(agdrec, col + 2) = val
  End If
  Exit Sub
errhandlerD:   ' Error handler line label.
  Call DispError("DatPrtDetail", Err)
  Resume Next ' Resume procedure.
End Sub

Public Sub DispXYlist()
  Dim i&, ypos&, minypos&, maxypos&, nMissing&, s&, c&, ip&, xp&, kx&, ky&
  Dim irec&, agdrec&, xstr$, ystr$

  'firstrow = 1
  For i = 0 To Nvar - 1
    ky = wchvr(1, i)
    kx = wchvr(2, i)
    'irec = 0
    agdrec = 0
    'now list the values
    minypos = bufpos(1, ky)
    maxypos = bufpos(2, ky)
    For ypos = minypos To maxypos
      'irec = irec + 1
      xp = bufpos(1, kx) + agdrec 'ypos - minypos
      agdrec = agdrec + 1 'irec - firstrow + 1
      Call F90_DECCHX(CSng(yx(xp)), FWid(kx), SDig(kx), dpla(kx), xstr)
      Call F90_DECCHX(CSng(yx(ypos)), FWid(ky), SDig(ky), dpla(ky), ystr)
      If i = 0 Then 'add new record to list
        If ky = 0 Then 'put y value in 1st column
          agd.TextMatrix(agdrec, 0) = ystr
          agd.TextMatrix(agdrec, 1) = xstr
        Else 'put x value in 1st column
          agd.TextMatrix(agdrec, 0) = xstr
          agd.TextMatrix(agdrec, 1) = ystr
        End If
      Else 'adding values to existing record
        If kx > 0 Then agd.TextMatrix(agdrec, kx - 1) = xstr
        If ky > 0 Then agd.TextMatrix(agdrec, ky - 1) = ystr
      End If
    Next ypos
  Next i
End Sub

'Returns true if list was displayed completely, false if time ran out
Public Function DispList(LimitTime As Boolean) As Boolean
  'generate listing of data values
  Static NextCol&, NextRow&
  Dim col&, ypos&, datpos&, nMissing&, s&, c&, ip&, xp&, kx&, ky&
  Dim irec& ', ncol&
  Dim minxpos&, minypos&, maxypos&
  Dim ltitl$, xstr$, ystr$
  Dim lastflg As Boolean, X As Boolean
  Dim listPos&(POSMAX), listJdate&(POSMAX)
  Dim StartTime&
  Dim scratch As Boolean
  frmL.MousePointer = vbHourglass
  DispList = False
  editingGrid = True

  If LType > 0 Then Nvar = NList  'same number of variables as data listings
  If NextCol >= Nvar Then NextCol = 0
  If NextCol = 0 And NextRow = 0 Then
    agd.Visible = False
    agd.Clear
    agd.cols = 1
    agd.rows = 1
    scratch = agd.HadError 'clear error flag
    Call BldHeader    'build header for listing
    'On Error GoTo allocationErrhandler
    ''If maxrows > 100000 Then
    ''  If MsgBox("Warning: A very large list has been requested (" & maxrows & " rows)." & vbCr _
    ''             & "This may cause an error that will cause this program to exit immediately." & vbCr _
    ''             & "Attempt to display the list anyway?", vbYesNo, "Warning") = vbNo Then
    ''    frmL.MousePointer = vbDefault
    ''    Exit Function
    ''  End If
    ''End If
    If maxrows > 1 Then agd.rows = maxrows Else agd.rows = 10
    If agd.HadError Then 'Error occurred because rows x cols too large
      maxrows = 1
      frmL.MousePointer = vbDefault
      Exit Function
    End If
    ltitl = Title
    ip = InStr(ltitl, "&")
    While ip > 0
      ltitl = Mid(ltitl, 1, ip - 1) & vbCrLf & Mid(ltitl, ip + 1)
      ip = InStr(ltitl, "&")
    Wend
    agd.Header = ltitl
  End If
  If LType < 1 Then
    DispXYlist
  Else
    If LimitTime Then StartTime = Timer

    MaxDatLev = 0
    
    On Error GoTo errhandler
    
    'include date/tran fields for time listing    ncol = NList + 2
    If NextCol = 0 And NextRow = 0 Then 'init accumulators
      For col = 0 To NList - 1
        listPos(col) = bufpos(1, wchvr(1, col))
        listJdate(col) = 0
        For datpos = 1 To 7       'time spans
          SumDat(datpos, col) = 0
          CntDat(datpos, col) = 0
          MinDat(datpos, col) = 1E+30
          MaxDat(datpos, col) = -1E+30
        Next datpos
      Next col
    End If
    If Not agd.Visible Then Timer2.Enabled = True 'Show agd soon
    For col = NextCol To Nvar - 1
      lastflg = False
      ky = wchvr(1, col)
      kx = wchvr(2, col)
      irec = 0
      'now list the values
      minypos = bufpos(1, ky)
      maxypos = bufpos(2, ky)
      minxpos = bufpos(1, kx)
      For ypos = minypos To maxypos
        'If LType > 0 Then 'time listing, date needed
          ip = ypos - minypos + 1
          If LType = 1 And Not (Native) Then 'constant interval
            Call TIMADD(SDate, TCODE, TSTEP, ip, CurDate)
          Else 'non-constant interval
            Call J2Date(yx(minxpos + ip - 1), CurDate())
          End If
          'convert to hr=24 for day boundary
          Call timcnv(CurDate)
          Call DatAccum(col, CSng(yx(ypos)))
          If ypos = maxypos Then lastflg = True
          Call DatPrt(irec, col, lastflg)
        'Else 'xy data listing
        'End If
        DoEvents
      Next ypos
      NextCol = col + 1
      If Timer - StartTime > DispListTimeout Then col = Nvar
    Next col
    If NextCol >= Nvar Then
      NextCol = 0
      DispList = True
      Timer1.Enabled = False
    Else
      DispList = False
      Timer1.Enabled = frmL.Visible 'if user has gotten tired and closed window, don't continue populating list
    End If
  End If
  agd.rows = agd.MaxOccupiedRow
  agd.Visible = True
  If Not Timer1.Enabled Then
    agd.ColsSizeByContents
    If curL.WindowState = vbNormal Then curL.Width = agd.gridWidth + 200
    frmL.MousePointer = vbDefault
    editingGrid = False
  End If
  Exit Function

errhandler:
  If Err.Number <> 401 Then
  'ignore error of showing list form while list edit form is still open
    
    'Call DispError("DispList", Err)
    Resume Next ' Resume procedure.
  End If
  Timer1.Enabled = False
  frmL.MousePointer = vbDefault
  DispList = False
  Exit Function

End Function


Public Function GetVarLabel$(cfld&)

   GetVarLabel = lbv(cfld)

End Function

Public Sub GetTitles(tit, capt)

   tit = Title
   capt = Caption

End Sub

'u = 0 for printer or file handle for file
Sub PrintSave(u%)

  Dim i&, k&, ip&, ncol&, f$, rdb As Boolean
  Dim lstr$, colstr$, ltitl$, rdbfmt$, tstr$
  Dim tFont As New StdFont

  rdb = False
  On Error GoTo errhandler
  If u = 0 Then 'print to printer
    'cdlPrintSave.ShowPrinter
    Dim fp&, tp&
    fp = 1
    tp = 1
    Call ShowPrinterX(Me, fp, tp, 1, 1, PD_NOSELECTION + PD_NOPAGENUMS + PD_DISABLEPRINTTOFILE)
    
    If tp < 0 Then Exit Sub 'Cancel selected in print dialog
    
    tFont = Font ' lsvList.Font

    Printer.CurrentY = 1
    Set Printer.Font = tFont
    Printer.FontBold = False
  Else 'print to file, get name from user
    cdlPrintSave.ShowSave
    f = cdlPrintSave.Filename
    If LCase(Right(f, 4)) = ".rdb" Then rdb = True 'output to RDB file
    If Len(Dir(f)) > 0 Then
      'get rid of existing file
      Kill f
    End If
    'open output file
    Open f For Output As #u
  End If

  ltitl = Title
  ip = InStr(ltitl, "&")
  While ip > 0
    ltitl = Mid(ltitl, 1, ip - 1) & vbCrLf & Mid(ltitl, ip + 1)
    ip = InStr(ltitl, "&")
  Wend
  If u = 0 Then
    Printer.Print
    Printer.Print
    Printer.Print
    Printer.Print "        " & ltitl
    Printer.Print
  ElseIf rdb Then 'RDB file
    Print #u, "# " & ltitl
    Print #u, "#"
  Else 'text file
    Print #u, ltitl
    Print #u,
  End If
  lstr = ""
  rdbfmt = ""
  ncol = agd.cols 'ncol = lsvList.ColumnHeaders.Count
  For i = 1 To ncol
    colstr = agd.ColTitle(i - 1) 'colstr = lsvList.ColumnHeaders(i)
    If rdb Then 'need tabs and column formats
      'build column format string
      If Left(colstr, 4) = "Time" Then 'string column
        tstr = CStr(Len(agd.TextMatrix(1, 0))) & "d"
      ElseIf Left(colstr, 4) = "Tran" Then 'string column
        tstr = "3s"
      Else 'numeric column
        If LType = 0 Then 'x,y listing (no time columns)
          tstr = CStr(FWid(i - 1)) & "n"
        ElseIf i > 2 Then 'time listing, 1st 2 fields have no width specs
          tstr = CStr(FWid(i - 3)) & "n"
        Else 'shouldn't get here
          tstr = "10n"
        End If
      End If
      rdbfmt = rdbfmt & tstr
      If i < ncol Then 'add tabs between columns
        colstr = colstr & Chr(9)
        rdbfmt = rdbfmt & Chr(9)
      End If
    Else 'add spaces to align columns
      While TextWidth(colstr) < agd.colWidth(i - 1) ' lsvList.ColumnHeaders(i).Width
        colstr = colstr & " "
      Wend
      'always add at least one space between columns
      colstr = colstr & SPACE(1)
    End If
    lstr = lstr & colstr
  Next i
  If u = 0 Then
    Printer.Print "        " & lstr
  Else 'rdb or text file
    Print #u, lstr
  End If
  If rdb Then 'include column formats for RDB file
    Print #u, rdbfmt
  End If
  For i = 1 To agd.rows ' lsvList.ListItems.Count
    lstr = agd.TextMatrix(i, 0) ' lsvList.ListItems(i)
    If Not rdb Then 'add spaces to align columns
      While TextWidth(lstr) < agd.colWidth(0) ' lsvList.ColumnHeaders(1).Width
        lstr = lstr & " "
      Wend
    End If
    For k = 1 To ncol - 1
      colstr = agd.TextMatrix(i, k) ' lsvList.ListItems(i).SubItems(k)
      If rdb Then 'include tabs in RDB output
        lstr = lstr & Chr(9) & colstr
      Else 'add spaces to align columns
        While TextWidth(colstr) < agd.colWidth(k) ' lsvList.ColumnHeaders(k + 1).Width
          colstr = colstr & " "
        Wend
        'always add at least one space between columns
        lstr = lstr & " " & colstr
      End If
    Next k
    If u = 0 Then
      Printer.Print "        " & lstr
    Else
      Print #u, lstr
    End If
  Next i
  'end printer output
  If u = 0 Then
    Printer.EndDoc
  Else
    Close #u
  End If
  Exit Sub
errhandler:
  On Error GoTo 0

End Sub

Public Sub SetCurDrvDir(V$, d$)
    Dim e&
    
    On Error GoTo eh
    
    If Len(V) > 0 Then
      e = 1
      ChDrive V
    End If
    If Len(d) > 0 Then
      e = 2
      ChDir d
    End If
    Exit Sub
    
eh:
    On Error GoTo 0
    If e = 1 Then
      MsgBox "SetCurDrvDir:bad drive: " & V, 48, "HassGraph Problem"
    ElseIf e = 2 Then
      MsgBox "SetCurDrvDir:bad directory: " & d, 48, "HassGraph Problem"
    End If
End Sub

Public Sub GetDateFmt(ifmt&)

    ifmt = DateFmt

End Sub

Public Sub SetDateFmt(ifmt&)

    DateFmt = ifmt

End Sub


Public Sub SetListType(ltyp&)

    LType = ltyp

End Sub

Public Sub SetXRange(linall As Boolean, linout As Boolean, loutall As Boolean)

    XRangeInAll = linall
    XRangeOutAll = loutall
    XRangeInOut = linout
    
End Sub
Public Sub GetXRange(linall As Boolean, linout As Boolean, loutall As Boolean)

    linall = XRangeInAll
    loutall = XRangeOutAll
    linout = XRangeInOut
    
End Sub

Private Sub agd_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim row&, col&, inum&, bufstart&, bufoffset&
  Dim EditingYX&, EditingBuf& 'We tried having these global and setting them only in the RowColChange, but that didn't work when the user moved in the grid while editing
  
  If Not editingGrid Then
    editingGrid = True
    For col = ChangeFromCol To ChangeToCol
      EditingBuf = bufInCol(col)
    'We don't allow changing more than one value at a time yet (pasting, for example), that would be a little harder
      If EditingBuf >= 0 Then ' And ChangeFromRow = ChangeToRow And ChangeFromCol = ChangeToCol Then
        For row = ChangeFromRow To ChangeToRow
          EditingYX = yxIndex(row, col)
          If EditingYX >= 0 Then
            If IsNumeric(agd.TextMatrix(row, col)) Then
              Dim temp#
              temp = agd.TextMatrix(row, col)
              If temp <> yx(EditingYX) Then
                'Debug.Print "Value " & yx(EditingYX) & " at (" & row & ", " & col & ") changed to " & temp
                yx(EditingYX) = temp
                bufEdited(EditingBuf) = bufEditable(EditingBuf)
              End If
            End If
          End If
        Next row
      End If
    Next col
  '    For col = ChangeFromCol To ChangeToCol
  '      inum = bufInCol(ChangeFromCol)
  '      If inum >= 0 Then
  '        bufstart = bufpos(1, inum)
    '    For row = 1 To ChangeFromRow - 1
    '      If (ThisVariableHasAValueInThisRow) Then bufoffset = bufoffset + 1
    '    Next row
  '      For row = ChangeFromRow To ChangeToRow
  '        If LType > 0 Then 'time listing
  '
  '        Else 'xy data listing
    '        yx(bufstart + row - firstrow) = agd.TextMatrix(row, col)
  '        End If
  '      Next row
  '    Next col
    editingGrid = False
  End If
End Sub

Private Sub agd_Error(code As Long, Description As String)
  Select Case code
  Case 1  'allocation error
    MsgBox "Data set too large to be displayed." & vbCr & Description, vbOKOnly, "List Error"
  Case Else
    MsgBox Err.Description, vbOKOnly, "List Error"
  End Select
End Sub

Private Sub agd_RowColChange()
  Dim EditingBuf&, NowEditable As Boolean
  EditingBuf = bufInCol(agd.col)
  NowEditable = False
  If EditingBuf >= 0 Then
    If bufEditable(EditingBuf) Then
      If yxIndex(agd.row, agd.col) >= 0 Then
        NowEditable = True
      End If
    End If
  End If
  mnuEdPaste.Enabled = NowEditable
  agd.ColEditable(agd.col) = NowEditable
End Sub

'returns -1 on failure
Private Function yxIndex&(agdRow&, agdCol&)
  Dim inum&, minypos&, maxypos&, minxpos&, retval&, kx&, ky&
  retval = -1
  inum = bufInCol(agdCol)
  ky = wchvr(1, inum)
  kx = wchvr(2, inum)
  minypos = bufpos(1, ky)
  maxypos = bufpos(2, ky)
  minxpos = bufpos(1, kx)
  If LType < 1 Then 'xy data
    retval = minypos + agdRow - 1 'NOT CORRECT if we can have datasets that don't all start at the same X value
  Else  'time in left column
    Dim itmdta$, jdat#, jdat2#, found As Boolean
    itmdta = agd.ItemData(agdRow) 'Could be faster if we took advantage of the fact that dates are already sorted
    If Len(itmdta) > 2 Then
      jdat = CDbl(Mid(itmdta, 3))
      found = False
      retval = minypos
      While Not found And retval <= maxypos
        If LType = 1 Then 'constant interval
          Call TIMADD(SDate, TCODE, TSTEP, retval - minypos + 1, CurDate)
          Call timcnv(CurDate)
          jdat2 = Date2J(CurDate())
        Else 'non-constant interval
          jdat2 = yx(minxpos + retval - minypos)
        End If
        If Abs(jdat - jdat2) < 0.000001 Then found = True Else retval = retval + 1
      Wend
    End If
  End If
  If Not found Or retval < minypos Or retval > maxypos Then yxIndex = -1 Else yxIndex = retval
End Function

Private Sub Form_Activate()

    If frmL.Tag > -1 Then
      Set curL = l(frmL.Tag)
    End If
'    If Tag >= 0 Then
'      Set frmL = l(Tag)
'    End If

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  Dim i&, HaveChanges As Boolean
  HaveChanges = False
  For i = 0 To Nvar - 1
    If bufEdited(i) Then HaveChanges = True
  Next i
  If HaveChanges Then
    Dim answer As VbMsgBoxResult
    answer = MsgBox("Save edits?", vbYesNoCancel, "Closing List")
    If answer = vbCancel Then Cancel = 1
    If answer = vbYes Then
      For i = 0 To Nvar - 1
        If bufEdited(i) Then parentlst.RaiseDataChanged i
      Next i
    End If
  End If
End Sub

Private Sub Form_Resize()
  If Height > 100 And Width > 100 Then    'not minimized
  
    If Me.ScaleWidth > 200 Then
      agd.Width = Me.ScaleWidth - 30 '- sb1.Width
      'sb1.Left = Me.ScaleWidth - 15 - sb1.Width
    End If
    If Me.ScaleHeight - agd.Top > 50 Then 'lsvList.Top > 50 Then
      agd.Height = Me.ScaleHeight - agd.Top - 50 'lsvList.Height = Me.ScaleHeight - lsvList.Top - 50
      'If agd.Height > agd.gridTop Then
      '  sb1.Height = agd.Height - agd.gridTop
      '  sb1.Top = agd.Top + agd.gridTop
      'End If
      'Dim r&
      'r = agd.TopRow
      'While agd.RowIsVisible(r) And r < agd.Rows
      '  r = r + 1
      'Wend
      'If r < agd.Rows Then
      '  agd.Rows = r
      'Else
      '  While agd.RowIsVisible(agd.Rows)
      '    agd.Rows = agd.Rows + 1
      '  Wend
      '  If agd.Rows > 1 Then agd.Rows = agd.Rows - 1
      'End If
      'If agd.Rows > 3 Then sb1.LargeChange = agd.Rows - 3
    End If
  
    'Call DispList      ' not in the displist resize

  End If
End Sub

Private Sub mnuClose_Click()

    Unload Me
    Me.Tag = -1

End Sub

Private Sub mnuEdCopy_Click()
  agd.Copy
End Sub

Private Sub mnuEdList_Click(index As Integer)

    Dim i&

    Set frmL = Me
    frmLEdit.sstEd.Tab = index
    frmLEdit.icon = Me.icon
    If LType <> 1 Or Native Then 'not constant interval timeseries
      'disable summations
      For i = 1 To 7
        frmLEdit.lblIntrvl(i - 1).Enabled = False
        frmLEdit.chkAve(i).Enabled = False
        frmLEdit.chkSum(i).Enabled = False
        frmLEdit.chkMin(i).Enabled = False
        frmLEdit.chkMax(i).Enabled = False
        frmLEdit.chkCnt(i).Enabled = False
      Next i
      frmLEdit.lstEndMon.Enabled = False
      'For i = 0 To 2
      '  frmLEdit.optDateFmt(i).Enabled = False
      'Next i
    End If
    frmLEdit.Show 1

End Sub

Private Sub mnuEdPaste_Click()
  agd.Paste
End Sub

Private Sub mnuPrint_Click()

    'output to printer
    Call PrintSave(0)
    
End Sub

Public Sub SetEditable(inum&, Editable As Boolean)
  bufEditable(inum) = Editable
End Sub

Public Sub SetData(inum&, ipos&, nv&, arra() As Double, retcod&)

  'put data to be listed in list buffer
  Dim i&, k&

  If inum <= 2 * POSMAX Then 'room for more data
    bufpos(1, inum) = ipos
    bufpos(2, inum) = ipos + nv - 1
    'resize data buffer
    ReDim Preserve yx(bufpos(2, inum))
    'put values in buffer
    For i = 0 To nv - 1
      k = ipos + i
      yx(k) = arra(LBound(arra) + i)
    Next i
    If nv > maxrows Then maxrows = nv ': sb1.Max = maxrows
    retcod = 0
  Else
    retcod = 1
  End If

End Sub

Public Sub GetData(inum&, arra() As Double)
  'retrieve edited data from list buffer
  Dim i&, ipos&, nv&

  ipos = bufpos(1, inum)
  nv = bufpos(2, inum) - ipos + 1
  'put values in buffer
  For i = 0 To nv - 1
    arra(LBound(arra) + i) = yx(ipos + i)
  Next i

End Sub

Public Sub GetNumVars(inlis&, invar&)

  'set number of listings and variables
  inlis = NList
  invar = Nvar

End Sub

Public Sub SetNumVars(inlis&, invar&)

  'set number of listings and variables
  If inlis <= POSMAX And invar <= 2 * POSMAX Then
    NList = inlis
    Nvar = invar
  Else
    MsgBox "SetNumVars: Too many listings (" & inlis & ") or variables (" & invar & ")." & vbCrLf & "Max for listings is " & POSMAX & "." & vbCrLf & "Max for variables is " & 2 * POSMAX & ".", 48, "HassGraph Problem"
  End If

End Sub

Private Sub BldHeader()

  'build header for data listing

  'Dim colhdr As ColumnHeader
  Dim i&, colwid&, swid&, hdrstr$, nhdrs&
  nhdrs = 0
  'lsvList.ColumnHeaders.Clear
  For i = 0 To POSMAX
    bufInCol(i) = NONE
    bufEdited(i) = False
  Next i
  
  For i = 0 To Nvar - 1
    If i = 0 And TCODE > 0 And LType > 0 Then
      'add time column label
      If TCODE = 6 Then 'year
        colwid = TextWidth("Time")
        swid = 4
      ElseIf TCODE = 5 Then 'month
        colwid = TextWidth("0000 WWW")
        swid = 7
      ElseIf TCODE = 4 Then 'day
        colwid = TextWidth("0000 WWW 00")
        swid = 10
      ElseIf TCODE = 3 Then 'hour
        colwid = TextWidth("0000 WWW 00 00")
        swid = 13
      ElseIf TCODE = 2 Then 'minute
        colwid = TextWidth("0000 WWW 00 00:00")
        swid = 16
      ElseIf TCODE = 1 Then 'second
        colwid = TextWidth("0000 WWW 00 00:00:00")
        swid = 19
      End If
      hdrstr = "Time"
      'add column header to listview
      'Set colhdr = lsvList.ColumnHeaders.add(, , hdrstr, colwid)
      agd.ColTitle(nhdrs) = hdrstr
      agd.colWidth(nhdrs) = colwid
      agd.ColType(nhdrs) = ATCoTxt
      agd.ColEditable(nhdrs) = False
      nhdrs = nhdrs + 1
      'add transformation column
      'Set colhdr = lsvList.ColumnHeaders.add(, , "Tran", TextWidth("Tran"), 1)
      agd.ColTitle(nhdrs) = "Tran"
      agd.colWidth(nhdrs) = TextWidth("Tran")
      agd.ColType(nhdrs) = ATCoTxt
      agd.ColEditable(nhdrs) = False
      nhdrs = nhdrs + 1
    End If
    hdrstr = Trim(lbv(i))
    colwid = TextWidth(String(FWid(i), "0"))
    If TextWidth(hdrstr) > colwid Then
      colwid = TextWidth(hdrstr)
    End If
    'add column header to list view
    agd.ColTitle(nhdrs) = hdrstr
    agd.colWidth(nhdrs) = colwid
    agd.ColType(nhdrs) = ATCoTxt 'ATCoSng
    agd.ColEditable(nhdrs) = bufEditable(i)
    bufInCol(nhdrs) = i
    nhdrs = nhdrs + 1
    'Set colhdr = lsvList.ColumnHeaders.add(, , hdrstr, colwid)
    
    'If i > 0 Or LType > 0 Then
    '  'align column to right
    '  colhdr.Alignment = 1
    'End If
  Next i

End Sub

Public Sub SetTitles(titl$, capt$)

    'set listing title and form caption
    Title = titl
    Caption = capt

End Sub

Public Sub SetVars(curve&, yvar&, xvar&)

    'store which variables to use for each listing
    If curve <= POSMAX Then 'room for more listings
      wchvr(1, curve) = yvar
      wchvr(2, curve) = xvar
    End If

End Sub

Public Sub SetVarLabel(vind&, vlab$)

    'set label for a variable being listed
    If vind <= POSMAX Then
      lbv(vind) = vlab
    End If

End Sub


Public Sub init()

    'initialization routine for generating a listing
    Dim i&, lflg&, id&

    'look for an available list form
    lflg = 0
    i = 0
    While i < NumList
      If l(i).Tag = -1 Then
        'this form is available, use it for this listing
        l(i).Tag = i 'setting tag triggers load
        id = i
        Set curL = l(i)
        Set frmL = curL
        i = NumList
        lflg = 1
      Else
        i = i + 1
      End If
    Wend
    If lflg = 0 Then
      'no list forms available, need a new one
      ReDim Preserve l(NumList)
      l(NumList).Tag = NumList
      id = NumList
      Set curL = l(NumList)
      Set frmL = curL
      NumList = NumList + 1
    End If
    l(id).Top = l(0).Top + id * 300
    Native = False

End Sub

Public Sub ShowIt(Optional modal As Boolean = False)
  Set curL = Me
  If modal Then
    curL.Show 1
  Else
    curL.Show
  End If
  DispListTimeout = 2
  DispList True
End Sub

Public Sub GetFldPrms(cfld&, wid&, sdg&, dpl&)

    If cfld <= POSMAX Then
      wid = FWid(cfld)
      sdg = SDig(cfld)
      dpl = dpla(cfld)
    Else
      wid = 1
      sdg = 1
      dpl = 0
    End If

End Sub
Public Sub GetFldRange(cfld&, lmin!, lmax!, linran As Boolean)

   If cfld <= POSMAX Then
     lmin = fmin(cfld)
     lmax = fmax(cfld)
     linran = FInRan(cfld)
   Else
     lmin = -1E+30
     lmax = 1E+30
     linran = True
   End If
  
End Sub

Public Sub SetFldPrms(cfld&, wid&, sdg&, dpl&)

    If cfld <= POSMAX Then
      FWid(cfld) = wid
      SDig(cfld) = sdg
      dpla(cfld) = dpl
    End If

End Sub
Public Sub SetFldRange(cfld&, lmin!, lmax!, linran As Boolean)

   If cfld <= POSMAX Then
     fmin(cfld) = lmin
     fmax(cfld) = lmax
     FInRan(cfld) = linran
   End If
   
End Sub

Public Sub SetIcon(ic As Object)

    'set icon for plot form
    icon = ic

End Sub

Public Sub DatAccum(col&, r!)

    Dim doit As Boolean
    
    If r < fmin(col) Or r > fmax(col) Then
      doit = False
    Else
      doit = True
    End If
    If Not (FInRan(col)) Then
      doit = Not (doit)
    End If
      
    'save data in current interval
    If doit Then
      SumDat(TCODE, col) = r
      MinDat(TCODE, col) = r
      MaxDat(TCODE, col) = r
      CntDat(TCODE, col) = 1
      'accumulate data to the next time interval
      SumDat(TCODE + 1, col) = SumDat(TCODE + 1, col) + r
      CntDat(TCODE + 1, col) = CntDat(TCODE + 1, col) + 1
      If r > MaxDat(TCODE + 1, col) Then
        MaxDat(TCODE + 1, col) = r
      End If
      If r < MinDat(TCODE + 1, col) Then
        MinDat(TCODE + 1, col) = r
      End If
    End If

End Sub

Public Sub GetSums(nsums&, sumtype&(), sumint&(), yrend&, mxsumcnt&)

    'get selected summaries/aggregations for data listing
    Dim i&

    nsums = 0
    For i = 1 To 7
      If SumChk(i) = 1 Then
        sumtype(nsums) = 1
        sumint(nsums) = i
        nsums = nsums + 1
      End If
      If AveChk(i) = 1 Then
        sumtype(nsums) = 0
        sumint(nsums) = i
        nsums = nsums + 1
      End If
      If MinChk(i) = 1 Then
        sumtype(nsums) = 3
        sumint(nsums) = i
        nsums = nsums + 1
      End If
      If MaxChk(i) = 1 Then
        sumtype(nsums) = 2
        sumint(nsums) = i
        nsums = nsums + 1
      End If
      If CntChk(i) = 1 Then
        sumtype(nsums) = 4
        sumint(nsums) = i
        nsums = nsums + 1
      End If
    Next i
    yrend = EndMon
    mxsumcnt = MaxSumCnt

End Sub
Public Sub SetSums(nsums&, sumtype&(), sumint&(), yrend&, mxsumcnt&)

    'set desired summaries/aggregations for data listing
    Dim i&, ip&, ltc&, npts&

    'init summary flags
    For i = 1 To 7
      SumChk(i) = 0
      AveChk(i) = 0
      MinChk(i) = 0
      MaxChk(i) = 0
      CntChk(i) = 0
    Next i
    MaxSumCnt = mxsumcnt
    If MaxSumCnt > 0 Then
      'find summation interval to yield less
      'than maxsumcnt records
      ltc = TCODE - 1
      Do
        ltc = ltc + 1
        Call timdif(SDate, EDate, ltc, TSTEP, npts)
      Loop While npts > MaxSumCnt And ltc < 7
      'increase summation intervals as needed
      For i = 0 To nsums - 1
        If sumint(LBound(sumint) + i) < ltc Then
          sumint(LBound(sumint) + i) = ltc
        End If
      Next i
    End If
    For i = 0 To nsums - 1
      ip = LBound(sumtype) + i
      If sumtype(ip) = 1 Then
        SumChk(sumint(ip)) = 1
      ElseIf sumtype(ip) = 0 Then
        AveChk(sumint(ip)) = 1
      ElseIf sumtype(ip) = 3 Then
        MinChk(sumint(ip)) = 1
      ElseIf sumtype(ip) = 2 Then
        MaxChk(sumint(ip)) = 1
      ElseIf sumtype(ip) = 4 Then
        CntChk(sumint(ip)) = 1
      End If
    Next i
    EndMon = yrend

End Sub

Public Sub GetTime(ts&(), tc&(), sdt&(), edt&(), dtr&())

    Dim i&

    For i = 0 To 5
      sdt(i) = SDate(i)
      edt(i) = EDate(i)
    Next i
    tc(0) = TCODE
    ts(0) = TSTEP

End Sub
Public Sub SetTime(ts&(), tc&(), sdt&(), edt&(), dtype&())

    Dim i&

    For i = 0 To 5
      SDate(i) = sdt(i)
      EDate(i) = edt(i)
    Next i
    TCODE = tc(LBound(tc))
    TSTEP = ts(LBound(ts))
    For i = LBound(tc) + 1 To UBound(tc) - 1
      If tc(i) > 0 Then ' skip useless values from frmG because UBound(tc) is always 18
        If tc(i) <> TCODE Or ts(i) <> TSTEP Then Native = True
        If tc(i) < TCODE Then TCODE = tc(i)
      End If
    Next
    LType = 0
    For i = LBound(dtype) To UBound(dtype) - 1
      If dtype(i) > LType Then LType = dtype(i)
    Next i
    If LType = 2 Then
      Native = True
    End If
    If Native Then TCODE = 1   'only outputs at seconds
End Sub

Private Sub mnuSav_Click()
  
    Dim u%

    u = FreeFile(0)
    Call PrintSave(u)

End Sub

'Private Sub sb1_Change()
'  Static LastValue&
'  Dim row&
'  row = 1
'  If Not scrolling Then
'    scrolling = True
'    If LType > 0 Then 'time listing
'      Select Case sb1.value
'      Case LastValue + sb1.SmallChange: 'scroll down one row
'        While CDbl(Mid(agd.ItemData(row), 3)) <= firstVisibleJDate
'          row = row + 1
'        Wend
'        firstVisibleJDate = CDbl(Mid(agd.ItemData(row), 3))
'      Case LastValue - sb1.SmallChange:
'
'      Case LastValue + sb1.LargeChange:
'
'      Case LastValue - sb1.LargeChange:
'
'      End Select
'    Else
'
'    End If
'    DispList False
'    scrolling = False
'  End If
'  LastValue = sb1.value
'End Sub

Private Sub Timer1_Timer()
  Timer1.Enabled = False
  DispList True
End Sub

Private Sub Timer2_Timer()
  Timer2.Enabled = False
  agd.Visible = True
End Sub
