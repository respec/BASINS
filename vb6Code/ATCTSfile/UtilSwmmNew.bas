Attribute VB_Name = "UtilSwmmNew"
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

'these want to be ATCoTimSers
Type SwmmTimser
  v() As Single 'flow and pollutants loads
End Type

Type SwmmPoll
  Pname As String 'pollutant names
  Punit As String 'pollutant units, eg mg/l, MPN/l, JTU, etc.
  Ndim As Long 'type of concentration units, 0-mg/l, 1=other
End Type

Type SwmmLocation
  Name As String 'location id (handles either NLOC (numeric) or KLOC (alphanumeric) SWMM convention)
  q As SwmmTimser 'flow array
  Poll() As SwmmTimser 'dim by Npoll
End Type

Type SwmmTime
  JDay As Double 'Julian start date/time for each time step
  Delta As Single 'step size in seconds for next time step
End Type

Type SwmmData
  fname As String 'name of SWMM interface file
  Title1 As String 'first line of title from 1st block
  Title2 As String 'second line of title from 1st block
  Idatez As Long 'starting date
  Tzero As Single 'starting time
  Title3 As String 'first line of title from prior block
  Title4 As String 'second line of title from prior block
  source As String 'name of immediately prior block
  Locats As Long 'number of locations
  Npoll As Long 'number of pollutants
  Triba As Single 'tributary service area, acres
  Jce As Long 'flag for type of location numbers, 0-numeric, 1-alphanumeric
  p() As SwmmPoll 'array of pollutant info
  Qconv As Single 'conversion factor to get flow in cfs
  Tcount As Long 'count of time steps
  t() As SwmmTime 'array of time step info
  l() As SwmmLocation ' size Locats
End Type

Sub SwmmRead(sd As SwmmData)

  'read SWMM binary interface file
  Dim f%, l&, s$, i%, j%, lts&
  Dim NLoc&, Julday&, Timday!

  'open and check validity of unformatted sequential file
  f = FtnUnfSeqInitRd(sd.fname)
  If f <= 0 Then 'file not opened, not unformatted sequential
    MsgBox "File: '" & sd.fname & "' is not a SWMM Interface File"
  Else 'file opened ok, read it
    l = FtnUnfSeqRecLen(f)
    sd.Title1 = Input(l, #f)
    l = FtnUnfSeqRecLen(f)
    sd.Title2 = Input(l, #f)
    l = FtnUnfSeqRecLen(f)
    Get #f, , sd.Idatez
    Get #f, , sd.Tzero
    l = FtnUnfSeqRecLen(f)
    sd.Title3 = Input(l, #f)
    l = FtnUnfSeqRecLen(f)
    sd.Title4 = Input(l, #f)
    l = FtnUnfSeqRecLen(f)
    sd.source = Input(20, #f)
    Get #f, , sd.Locats
    ReDim sd.l(sd.Locats)
    Get #f, , sd.Npoll
    ReDim sd.p(sd.Npoll)
    For i = 0 To sd.Locats - 1
      ReDim sd.l(i).Poll(sd.Npoll)
    Next i
    Get #f, , sd.Triba
    Get #f, , sd.Jce
    l = FtnUnfSeqRecLen(f)
    For i = 0 To sd.Locats - 1
      If sd.Jce = 0 Then 'read numeric location name
        Get #f, , NLoc
        sd.l(i).Name = CStr(NLoc)
      ElseIf sd.Jce = 1 Then 'read alphanumeric location name
        sd.l(i).Name = Input(10, #f)
      End If
    Next i
    If sd.Npoll > 0 Then
      l = FtnUnfSeqRecLen(f)
      For i = 0 To sd.Npoll - 1
        sd.p(i).Pname = Input(8, #f)
      Next i
      l = FtnUnfSeqRecLen(f)
      For i = 0 To sd.Npoll - 1
        sd.p(i).Punit = Input(8, #f)
      Next i
      l = FtnUnfSeqRecLen(f)
      For i = 0 To sd.Npoll - 1
        Get #f, , sd.p(i).Ndim
      Next i
    End If
    l = FtnUnfSeqRecLen(f)
    Get #f, , sd.Qconv

    sd.Tcount = 0
    lts = FtnUnfSeqRecLen(f)
    Do
      ReDim Preserve sd.t(sd.Tcount)
      Get #f, , Julday
      Get #f, , Timday
      sd.t(sd.Tcount).JDay = SWMMDate2Jul(Julday, Timday)
      Get #f, , sd.t(sd.Tcount).Delta
      For i = 0 To sd.Locats - 1
        ReDim Preserve sd.l(i).q.v(sd.Tcount)
        Get #f, , sd.l(i).q.v(sd.Tcount)
        If sd.Npoll > 0 Then
          For j = 0 To sd.Npoll - 1
            ReDim Preserve sd.l(i).Poll(j).v(sd.Tcount)
            Get #f, , sd.l(i).Poll(j).v(sd.Tcount)
          Next j
        End If
      Next i
      sd.Tcount = sd.Tcount + 1
      l = FtnUnfSeqRecLen(f)
      If l < lts Then
        Exit Do
      End If
    Loop
  End If

  Close #f

End Sub

Sub SwmmWrite(sd As SwmmData)

  'write SWMM data out to SWMM binary interface file
  Dim f%, l&, i%, j%, k&, b As Byte
  Dim NLoc&, Julday&, Timday!

  f = FtnUnfSeqInitWr(sd.fname)
  If f > 0 Then
    l = 80
    Call FtnUnfSeqWrHead(f, l)
    Call FtnUnfSeqWrStr(f, l, Left(sd.Title1, l))
    Call FtnUnfSeqWrHead(f, l)
    Call FtnUnfSeqWrStr(f, l, Left(sd.Title2, l))
    l = 8
    Call FtnUnfSeqWrHead(f, l)
    Put #f, , sd.Idatez
    Put #f, , sd.Tzero
    l = 80
    Call FtnUnfSeqWrHead(f, l)
    Call FtnUnfSeqWrStr(f, l, Left(sd.Title3, l))
    Call FtnUnfSeqWrHead(f, l)
    Call FtnUnfSeqWrStr(f, l, Left(sd.Title4, l))
    l = 36
    Call FtnUnfSeqWrHead(f, l)
    l = 20
    Call FtnUnfSeqWrStr(f, l, Left(sd.source, l))
    Put #f, , sd.Locats
    Put #f, , sd.Npoll
    Put #f, , sd.Triba
    Put #f, , sd.Jce

    If sd.Jce = 0 Then
      l = sd.Locats * 4 '+ 1
    Else
      l = sd.Locats * 10 '+ 1
    End If
    Call FtnUnfSeqWrHead(f, l)
    For i = 0 To sd.Locats - 1
      If sd.Jce = 0 Then
        NLoc = sd.l(i).Name
        Put #f, , NLoc
      ElseIf sd.Jce = 1 Then
        Call FtnUnfSeqWrStr(f, 10, Left(sd.l(i).Name, 10))
      End If
    Next i
    b = 0

    If sd.Npoll > 0 Then
      l = sd.Npoll * 8
      Call FtnUnfSeqWrHead(f, l)
      For i = 0 To sd.Npoll - 1
         Call FtnUnfSeqWrStr(f, 8, Left(sd.p(i).Pname, 8))
      Next i
      Call FtnUnfSeqWrHead(f, l)
      For i = 0 To sd.Npoll - 1
         Call FtnUnfSeqWrStr(f, 8, Left(sd.p(i).Punit, 8))
      Next i
      l = l / 2
      Call FtnUnfSeqWrHead(f, l)
      For i = 0 To sd.Npoll - 1
         Put #f, , sd.p(i).Ndim
      Next i
    End If
    l = 4
    Call FtnUnfSeqWrHead(f, l)
    Put #f, , sd.Qconv

    l = 4 * (3 + sd.Locats * (sd.Npoll + 1))
    For k = 0 To sd.Tcount - 1
      Call FtnUnfSeqWrHead(f, l)
      'convert julian date/time to SWMM convention
      Call SWMMJul2Date(sd.t(k).JDay, Julday, Timday)
      Put #f, , Julday
      Put #f, , Timday
      Put #f, , sd.t(k).Delta
      For i = 0 To sd.Locats - 1
        Put #f, , sd.l(i).q.v(k)
        If sd.Npoll > 0 Then
          For j = 0 To sd.Npoll - 1
            Put #f, , sd.l(i).Poll(j).v(k)
          Next j
        End If
      Next i
    Next k
  End If

  Call FtnUnfSeqWrHead(f, CLng(0)) 'clear out trailer

  Close #f

End Sub

Private Function SWMMDate2Jul(Julday&, Timday!) As Double

  'convert SWMM convention date values
  'to Julian date/time value
  'Julday - 5 digit number, 2 digit year, 3 digit julian day
  'Timday - time of day in seconds

  Dim ldt&(5), jd&

  ldt(0) = Int(Julday / 1000) 'year
  If ldt(0) < 30 Then 'assume > year 2000
    ldt(0) = ldt(0) + 2000
  Else 'assume 1900 date
    ldt(0) = ldt(0) + 1900
  End If
  jd = Julday Mod 1000 'julian day
  'get month and day from year and julian day
  MsgBox "Need to implement get month and day from year and julian day"
  'Call F90_JDMODY(ldt(0), jd, ldt(1), ldt(2))
  SWMMDate2Jul = MJD(ldt(0), ldt(1), ldt(2)) + Timday / 86400

End Function
Private Sub SWMMJul2Date(JDatim#, Julday&, Timday!)

  'convert Julian date/time value to
  'SWMM convention date values
  'JDatim - Julian date/time value
  'Julday - 5 digit number, 2 digit year, 3 digit julian day
  'Timday - time of day in seconds

  Dim ldt&(5), jd&, i&

  jd = Fix(JDatim)
  Call INVMJD(jd, ldt(0), ldt(1), ldt(2))
  Julday = (ldt(0) Mod 100) * 1000 'put 2 digit year in first 2 of 5 digits
  'calculate julian day
  For i = 1 To ldt(1) - 1 'sum days of months preceeding current month
    Julday = Julday + daymon(ldt(0), i)
  Next i
  Julday = Julday + ldt(2) 'include days of current month
  Timday = (JDatim - jd) * 86400#

End Sub

Public Sub Date2SWMMDatTim(lsdat&(), Idatez&, Tzero!)
  'convert date/time array to SWMM date format
  'Idatez - 5 digit number, 2 digit year, 3 digit julian day
  'Tzero - time of day in seconds

  Dim i&, Julday&

  Idatez = (lsdat(0) Mod 100) * 1000
  'calculate julian day
  For i = 1 To lsdat(1) - 1 'sum days of months preceeding current month
    Julday = Julday + daymon(lsdat(0), i)
  Next i
  Julday = Julday + lsdat(2) 'include days of current month
  Idatez = Idatez + Julday
  'save start time in seconds (as needed on binary interface file)
  Tzero = lsdat(3) * 3600 + lsdat(4) * 60 + lsdat(5)

End Sub

Sub SwTransInRead(f$, s As SwmmData)

  Dim i&, ifl&, istr$, lstr$, LLoc$, LType&, TrType$, Jin&
  Dim err As Boolean

  err = False
  ifl = FreeFile(0)
  Open f For Input As #ifl
  s.Locats = 0
  s.Npoll = 0
  Do While Not EOF(ifl) And Not err
    Line Input #ifl, istr
    If Left(istr, 7) = "$RUNOFF" Then 'can't have runoff input in same file
      MsgBox "Transport input file contains RUNOFF input." & vbCrLf & "Please remove RUNOFF input from file and try again.", vbExclamation, "GenScn SWMM Problem"
      err = True
    ElseIf Left(istr, 2) = "SW" Then 'I/O unit numbers
      istr = Mid(istr, 3)
      lstr = StrRetRem(istr) 'skip number of blocks
      lstr = StrRetRem(istr) 'get unit # for input interface file
      Jin = CLng(lstr) 'save unit number
    ElseIf Left(istr, 1) = "@" Then 'interface file name
      istr = Mid(istr, 2)
      lstr = StrRetRem(istr) 'get unit number
      'see if unit number for this interface file
      'matches unit number for input interface file
      If Jin = CLng(lstr) Then 'unit numbers match
        s.fname = StrRetRem(istr) 'get file name
      End If
    ElseIf Left(istr, 6) = "$TRANS" Then 'standard transport
      TrType = "T"
    ElseIf Left(istr, 7) = "$EXTRAN" Then 'extran transport
      TrType = "E"
    ElseIf Left(istr, 2) = "A1" Then 'process titles
      istr = Mid(istr, 3)
      If Len(s.Title1) = 0 Then '1st line
        s.Title1 = StrRetRem(istr)
      Else '2nd line
        s.Title2 = StrRetRem(istr)
      End If
    ElseIf Left(istr, 2) = "B1" Then 'process time parameters
      istr = Mid(istr, 3)
      lstr = StrRetRem(istr)
      s.Tcount = CLng(lstr) 'number of time steps
      ReDim s.t(s.Tcount)
      If TrType = "T" Then 'skip to start date value
        For i = 1 To 8
          lstr = StrRetRem(istr)
        Next i
        s.Idatez = CLng(lstr) 'start date
      ElseIf TrType = "E" Then 'read other EXTRAN time values
        lstr = StrRetRem(istr)
        s.t(0).Delta = CSng(lstr) 'time step
        lstr = StrRetRem(istr)
        s.Tzero = CSng(lstr) 'start time
        For i = 1 To 4
          lstr = StrRetRem(istr)
        Next i
        If Len(istr) > 0 Then 'start date is on record
          s.Idatez = CLng(lstr) 'start date
        End If
      End If
    ElseIf Left(istr, 2) = "B2" And TrType = "T" Then 'process time parameters
      istr = Mid(istr, 3)
      lstr = StrRetRem(istr)
      s.t(0).Delta = CSng(lstr) 'time step
      lstr = StrRetRem(istr) 'skip to start time
      lstr = StrRetRem(istr) 'skip to start time
      lstr = StrRetRem(istr)
      s.Tzero = CSng(lstr) 'start time
      lstr = StrRetRem(istr) 'skip to trib area
      lstr = StrRetRem(istr)
      s.Triba = CSng(lstr) 'tributary area
    ElseIf Left(istr, 2) = "E1" And TrType = "T" Then
      'process TRANSPORT location record
      istr = Mid(istr, 3)
      LLoc = StrRetRem(istr) 'get location name
      For i = 1 To 4 'skip three values, get location type
        lstr = StrRetRem(istr)
      Next i
      LType = CLng(lstr)
      If LType = 19 Then 'manhole, include this location
        ReDim Preserve s.l(s.Locats)
        s.l(s.Locats).Name = LLoc
        s.Locats = s.Locats + 1
      End If
    ElseIf Left(istr, 2) = "F1" And TrType = "T" Then
      'process TRANSPORT pollution record
      ReDim Preserve s.p(s.Npoll)
      istr = Mid(istr, 3)
      lstr = StrRetRem(istr) 'skip 1 value
      s.p(s.Npoll).Pname = StrRetRem(istr) 'get name
      s.p(s.Npoll).Punit = StrRetRem(istr) 'get units
      s.p(s.Npoll).Ndim = StrRetRem(istr) 'get type of concentration units
      s.Npoll = s.Npoll + 1
    ElseIf Left(istr, 2) = "C1" And TrType = "E" Then
      'process EXTRAN elements
      istr = Mid(istr, 3)
      ReDim Preserve s.l(s.Locats)
      s.l(s.Locats).Name = StrRetRem(istr)
      s.Locats = s.Locats + 1
    End If
  Loop
  Close ifl

End Sub
