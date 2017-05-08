Attribute VB_Name = "GenScnEntry"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Const TserFileClassName = "ATCTSfile"
'Global Launch As ATCoCtl.AtCoLaunch
Global IPC As ATCoCtl.ATCoIPC
Global RunningVB As Boolean
Global Registry As ATCoRegistry
Global InMemFile As ATCclsTserFile
Global ExeName As String 'name of executable
Global ExePath As String
Global ScenFile As String
'Global CntDsn As Long
Global dsn As Long
Global CSDat(6) As Long, ComSDat(6) As Long
Global CEDat(6) As Long, ComEDat(6) As Long
Global ctunit&, CTStep&, CDTran&
Global Const MXTSCOL = 9
Global NtsCol&
Global TSColName$(MXTSCOL)
Global tscolwidth&(MXTSCOL)
Global TSColID&(MXTSCOL)
Global specSen$(), specLoc$(), specCon$()
Global scntSen%, scntLoc%, scntCon%
Global sSLC$(4)
'Global dbg As ATCoDebug
Global ATCoDispFile1 As ATCoDispFile
'Global g As ATCoGraph
'Global l As ATCoList
Global TserFiles As ATCPlugInManager
Global TserMemory As ATCclsTserFile
Global myMsgBox As ATCoMessage

Type WDMFile
    Name As String
    Unit As Long
End Type

Type Constituent
    Name As String
    desc As String
End Type

Type Location
    Name As String
    desc As String
    GeoCode As Boolean
End Type

Type Scenario
    Name As String
    Type As String
    File As String
    desc As String ' description
End Type

Type GenScnProject
    EditFlg As Boolean
    'ExtCount As Long 'count of external time series
    StatusFileName As String
    StatusFilePath As String
    ScenCount As Long
    Scen() As Scenario
    LocnCount As Long
    Locn() As Location
    ConsCount As Long
    Cons() As Constituent
    MapName As String
    HSPFMsg As WDMFile
    WDMFiles As Collection
    ExternalForms As Collection
End Type
Global p As GenScnProject

Type Timser
    Type As String 'WDM, EXT, RDB
    FilIndex As Long ' index of WDM, EXT, or RDB file
    Stanam As String ' station name
    ID As Double 'identifier
    ts As Long 'time step
    Tu As Long 'time unit
    sdat(6) As Long 'start date
    edat(6) As Long 'end date
    Sen As String 'scnenario name
    Loc As String 'location name
    Con As String 'constituent name
    spos As Long 'starting position in whole span of data for current date (FEQ only)
    NVal As Long 'number of values
    Vals() As Single 'array of values
    Min As Single 'minimum value
    Max As Single 'maximum value
    Dtran As Long 'data transformation code (0-ave/same, 1-sum/div, 2-max, 3-min)
    Dist As Single 'station (FEQ) or distance to next node (WDM)
    Invrt As Single 'invert elevation
    bran As String 'branch name
    Node As String 'node name
    DSNode As String 'downstream node name
    MVal As Single 'missing value indicator
    MAcc As Single 'missing distribution idicator
End Type
'Global TSer() As Timser
'Global Tser As Collection
'Global nts As Long
'Global STSer() As Timser
'Global STSer As Collection
'Global Snts As Long
Global ExTS() As Timser

Type RDBColInfo
    Header As String
    ColPos As Long 'starting column for this field
    FldLen As Long 'width of this field
    Scenario As String
    Location As String
    Constituent As String
End Type

Type RDBD
    NameRDB As String 'rdb file name
    ColCount As Long 'count of data columns (not including Date column)
    ColInfo() As RDBColInfo 'info for each data column
    DatePos As Long 'starting column for date field
    DateLen As Long 'width of date field
    TimCount As Long 'count of timeseries values
    jday() As Double 'julian day of values
End Type
Global RDBData() As RDBD

Type vardata
  Vals() As Single
  Trans As Long '0-arithmetic, 1-log
  Min As Single
  Max As Single
End Type

Type labeldata
  nLables As Long
  Labels() As String
  Position As Long   '0-Y axis, 1-Xaxis
  Orientation As Long
End Type

Type xyplotdata
  NVal As Long
  Var(1) As vardata '0-Y, 1-X
  DataLabels As labeldata
End Type

Type DsInfo
  dsn As Long
  sdat(5) As Long
  edat(5) As Long
End Type

Type BasinsInfo
  desc As String
  Nam As String
  Elev As Single
  sdat(3) As Long
  edat(3) As Long
  EvapCoef As Single
  ds(7) As DsInfo
End Type

Global MyRchAvail As Boolean
Global NoWDMSLC As Boolean

'Global MyForms() As Form
Global HelpCheck As Boolean

'nts = number of ts to fill in collection
'ConstInt will be false on return if it was false when called or if a non-constant interval ts was found
Public Function FillTimSerExt(ts As Collection, ConstInt As Boolean) As Collection
  Dim i%, j&, mxpts&, ldt&(5), t#, lTs As Collection
  
  If CDTran = 4 Then ConstInt = False 'if using native time step, assume they will not be the same
  
  'get the data values for selected data sets
  Set lTs = FillTimSer(ts)
  
  'look for any non-constant interval data
  If ConstInt Then
    i = 1
    While i <= lTs.Count And ConstInt
      If Not (lTs(i).Dates.Summary.CIntvl) Then ConstInt = False
      i = i + 1
    Wend
  End If
  
  Set FillTimSerExt = lTs
End Function

Public Sub DsnBld(dsn&, tsstep&, TCODE&, Sen$, Loc$, Con$, desc$, retcod&)

    Dim i&, iVal&, psa&, ostr$, saind&, salen&
    Const ndn& = 10
    Const nup& = 10
    Const nsa& = 20
    Const nsasp& = 50
    Const ndp& = 200

    Call F90_WDLBAX(p.WDMFiles(1).FileUnit, dsn, 1, ndn, nup, nsa, nsasp, ndp, psa)
    'add needed attributes
    saind = 1 'tstype
    salen = 4
    ostr = UCase(Left(Con, 4))
    Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, ostr, Len(ostr))
    salen = 1
    saind = 34 'tgroup
    iVal = 6
    Call F90_WDBSAI(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, iVal, retcod)
    iVal = 1
    saind = 83 'compfg
    Call F90_WDBSAI(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, iVal, retcod)
    saind = 84 'tsform
    Call F90_WDBSAI(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, iVal, retcod)
    saind = 85 'vbtime
    Call F90_WDBSAI(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, iVal, retcod)
    saind = 17 'tcode
    Call F90_WDBSAI(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, TCODE, retcod)
    saind = 33 'tsstep
    Call F90_WDBSAI(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, tsstep, retcod)
    i = CSDat(0) Mod 10
    If i > 0 Then 'subtract back to start of this decade
      iVal = CSDat(0) - i
    Else 'back to start of previous decade
      iVal = CSDat(0) - 10
    End If
    saind = 27 'tsbyr
    Call F90_WDBSAI(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, iVal, retcod)
    ostr = GetDescription("CON", Con)
    salen = 48
    saind = 45 'stanam
    Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, ostr, Len(ostr))
    salen = 8
    saind = 288 'scenario
    ostr = UCase(Left(Sen, 8))
    Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, ostr, Len(ostr))
    saind = 289 'constituent
    ostr = UCase(Left(Con, 8))
    Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, ostr, Len(ostr))
    saind = 290 'location
    ostr = UCase(Left(Loc, 8))
    Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, ostr, Len(ostr))
    salen = 48
    saind = 45 'description
    ostr = Left(desc, 48)
    Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, ostr, Len(ostr))

End Sub

Public Function FillTimSer(ts As Collection) As Collection

  Dim v As Variant
  Dim JSdt#, JEdt#, Sdt&(6), Edt&(6)
  Dim lTs As Collection
  Dim ds As ATCclsTserDate
  Dim subSet As ATCclsTserData
  Dim aggDat As ATCclsTserData

  JSdt = Date2J(CSDat())
  JEdt = Date2J(CEDat())
  
  Set lTs = Nothing
  Set lTs = New Collection
  
  If CDTran = 4 Then
    For Each v In ts ' was nts
      If JSdt > v.Dates.Summary.SJDay Or JEdt < v.Dates.Summary.EJDay Then
        lTs.Add v.SubSetByDate(JSdt, JEdt)
      Else
        lTs.Add v
      End If
    Next
  Else
    Dim lDateSummary As ATTimSerDateSummary
    With lDateSummary
      .CIntvl = True
      .SJDay = JSdt
      .EJDay = JEdt
      .ts = CTStep
      .Tu = ctunit
      Select Case ctunit
        Case TUSecond:  .Intvl = CTStep / 86400#
        Case TUMinute:  .Intvl = CTStep / 1440#
        Case TUHour:    .Intvl = CTStep / 24#
        Case TUDay:     .Intvl = CTStep
        Case TUMonth:   .Intvl = CTStep * 30
        Case TUYear:    .Intvl = CTStep * 365.25
        Case TUCentury: .Intvl = CTStep * 36525
        Case Else: .Tu = TUCentury: .Intvl = 36525 'should not happen
      End Select
      If .Intvl <= 1 Then
        .NVALS = (JEdt - JSdt) / .Intvl '+ 1
      Else 'special case for long intervals
        Call J2Date(JSdt, Sdt)
        Call J2Date(JEdt, Edt)
        Call F90_TIMDIF(Sdt(0), Edt(0), .Tu, .ts, .NVALS)
      End If
    End With
    Set ds = New ATCclsTserDate
    ds.Summary = lDateSummary
    For Each v In ts
      Set subSet = v.SubSetByDate(JSdt, JEdt)
      Set aggDat = subSet.Aggregate(ds, CDTran)
      lTs.Add aggDat
      Set aggDat = Nothing
      Set subSet = Nothing
    Next
  End If
  'get the data values for selected data sets
  Set FillTimSer = Nothing
  Set FillTimSer = lTs

End Function

Public Sub RefreshSLC()

  Dim i&, startTime As Variant
  Dim vTserFile As Variant
  Dim ATserF As ATCclsTserFile
  Dim Atser As ATCclsTserData
  Dim newColl As Collection 'build collection of all datasets
  Dim OpenFiles As Collection
  Dim tc As Collection

  ReDim p.Scen(0)
  p.ScenCount = 0
  ReDim p.Cons(0)
  p.ConsCount = 0
  ReDim p.Locn(0)
  p.LocnCount = 0
  Set OpenFiles = New Collection
  Set newColl = New Collection
  For Each vTserFile In TserFiles.Active
    Set ATserF = vTserFile.obj
    OpenFiles.Add ATserF
    For i = 1 To ATserF.DataCount
      Set Atser = ATserF.Data(i)
      newColl.Add Atser
      With Atser.Header
        AddUniqueSLC .Sen, "Scen"  'scenario
        AddUniqueSLC .Loc, "Locn"  'location
        AddUniqueSLC .Con, "Cons"  'constituent
      End With
    Next i
  Next vTserFile
  'startTime = Timer
  FillReachDetails newColl
  'Debug.Print "Gentry:RefreshSLC:FillReachDetails took " & Timer - startTime & " sec"
  Set frmGenScn.TimserGrid.WholeList = newColl
  Set frmGenScn.TimserGrid.OpenFiles = OpenFiles
  'CntDsn = newColl.Count ' CountAllTimser
  'Call frmGenScn.RefreshMain 'This is not commented out in GenScn, not sure why it is commented here
  Set OpenFiles = Nothing
  Set newColl = Nothing

End Sub

Sub Main()
        
  Dim hin&, hout&, hdle&, i&, binpos&
  Dim s As String * 80
  Dim ExCmd As String 'command line

  hdle = GetModuleHandle("WDMUtil")
  i = GetModuleFileName(hdle, s, 80)
  ExeName = UCase(Left(s, InStr(s, Chr(0)) - 1))
  If InStr(ExeName, "VB6.EXE") Then
    RunningVB = True
    ExeName = UCase(MACHINE_EXEWDMUTIL)
    ExCmd = UCase(MACHINE_CMDWDMUTIL)
  Else
    RunningVB = False
    ExCmd = Command$
  End If
  binpos = InStrRev(ExeName, "\")
  If binpos < 1 Then
    ExePath = CurDir
  Else
    ExePath = Left(ExeName, binpos)
  End If
  If Right(ExePath, 1) <> "\" Then ExePath = ExePath & "\"
    
'    Set dbg = New ATCoDebug
'    Set g = New ATCoGraph
'    Set l = New ATCoList
    Set ATCoDispFile1 = New ATCoDispFile
    Set TserMemory = New clsTSerMemory
    Call InitATCoTSer

    'Set MyForms(0) = New frmGenScn
    'Set MyForms(20) = New frmGenScnTSCol
    'Set MyForms(21) = New frmGenScnEdit
    HelpCheck = False
    
    Set Registry = New ATCoRegistry
    Registry.AppName = "WDMUtil"
    Set p.ExternalForms = New Collection
    
    Set IPC = New ATCoIPC
'    Set Launch = New AtCoLaunch
'    Launch.StartMonitor ExePath & "status.exe"
'    Launch.NconvertPath = Registry.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\NconvertPath", "") & "\Nconvert.exe"
'    If Len(Launch.NconvertPath) = 0 Then Launch.NconvertPath = ExePath & "Nconvert.exe"
'    If Len(Dir(Launch.NconvertPath)) = 0 Then Launch.NconvertPath = ExePath & "bin\Nconvert.exe"
'    If Len(Dir(Launch.NconvertPath)) = 0 Then Launch.NconvertPath = "Nconvert.exe"
'    If Len(Dir(Launch.NconvertPath)) = 0 Then Launch.NconvertPath = ""
'
'    hin = Launch.ComputeRead
'    hout = Launch.ComputeWrite
    
    Set InMemFile = New clsTSerMemory
    
    Call F90_W99OPN  'open error file
    Call F90_WDBFIN  'initialize WDM record buffer
    Call F90_PUTOLV(10)
    
    App.HelpFile = Registry.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\WDMUtil\DocPath", "") & "\WDMUtil.chm"
    If Len(Dir(App.HelpFile)) = 0 Then App.HelpFile = ExePath & "doc\WDMUtil.chm"
    If Len(Dir(App.HelpFile)) = 0 Then App.HelpFile = ExePath & "WDMUtil.chm"
    If Len(Dir(App.HelpFile)) = 0 Then App.HelpFile = "WDMUtil.chm"

    sSLC(0) = "Scenario"
    sSLC(1) = "Location"
    sSLC(2) = "Constituent"
    sSLC(3) = "Location3"
    sSLC(4) = "Location4"

    ReDim RDBData(0)
    Set p.WDMFiles = Nothing
    Set p.WDMFiles = New Collection
    
    'set to no scenarios
    ReDim specSen(0)
    scntSen = 0
    
    Load frmGenScn 'load main form
    frmGenScn.Visible = True
    
End Sub

Public Sub DbgMsg(Msg As String, Optional lvl As Long = 0, Optional CodeGroup As String = "", Optional typ As String = "")
  If Len(CodeGroup) > 0 Then
    IPC.dbg "WDMUtil:" & CodeGroup & ":" & Msg
  Else
    IPC.dbg "WDMUtil:" & Msg
  End If
End Sub

Public Function CountAllTimser() As Long
  Dim vTserFile As Variant, curClsTserFile As ATCclsTserFile, i&
   
  i = 0
  For Each vTserFile In TserFiles.Active
    Set curClsTserFile = vTserFile.obj
    i = i + curClsTserFile.DataCount
  Next vTserFile
  CountAllTimser = i
End Function

Public Sub FindTimSer(Sen$, Loc$, Con$, lTs As Collection)
    Dim dsn&, i&, j&, k&, l&, s$, GRPSIZ&, r!, imatch%
    Dim vTserFile As Variant, curClsTserFile As ATCclsTserFile, lds As ATTimSerDateSummary

'    Call F90_TSESPC(Sen, Loc, Con, Len(Trim(Sen)), Len(Trim(Loc)), Len(Trim(Con)))
'    'look for matching WDM datasets
'    lc = 0
'    dsn = 1
'    While dsn > 0
'      Call F90_TSDSM(dsn)
'      If dsn > 0 Then
'        ReDim Preserve lts(lc)
'        Call F90_TSDSPC(dsn, lts(lc).Sen, lts(lc).Loc, lts(lc).Con, lts(lc).Tu, lts(lc).ts, lts(lc).sdat, lts(lc).edat, GRPSIZ)
'        'convert from midnight of hr=24 (prev day) to hr=0 (curr day)
'        Call F90_TIMCVT(lts(lc).sdat(0))
'        Call F90_WDBSGC(p.WDMFiles(1).FileUnit, dsn, CLng(45), CLng(48), s)
'        lts(lc).Stanam = s
'        Call F90_TIMDIF(lts(lc).sdat(0), lts(lc).edat(0), lts(lc).Tu, lts(lc).ts, lts(lc).NVal)
'        lts(lc).Type = "WDM"
'        lts(lc).FilIndex = 1 ' only support 1 wdm file
'        lts(lc).id = dsn
'        lc = lc + 1
'        dsn = dsn + 1
'      End If
'    Wend

    'now check for atcotimser (rdb) data
    Set lTs = Nothing
    Set lTs = New Collection
    For Each vTserFile In TserFiles.Active
      Set curClsTserFile = vTserFile.obj
      For j = 1 To curClsTserFile.DataCount
        If (Sen = curClsTserFile.Data(j).Header.Sen Or Len(Trim(Sen)) = 0) And _
           (Loc = curClsTserFile.Data(j).Header.Loc Or Len(Trim(Loc)) = 0) And _
           (Con = curClsTserFile.Data(j).Header.Con Or Len(Trim(Con)) = 0) Then 'need this timser
          'ReDim Preserve lts(lc)
          'Call CopyTimserNew2Old(curClsTserFile.Data(j), _
          '                       lts(lc))
          lTs.Add curClsTserFile.Data(j)
          'lts(lc).Type = vTserFile.label
          'lts(lc).FilName = f
          'lc = lc + 1
        End If
      Next j
    Next vTserFile

    'now check for rdb data
'    For i = 0 To p.RDBCount - 1
'      For j = 0 To RDBData(i).ColCount - 1
'        If (Sen = RDBData(i).ColInfo(j).Scenario Or Len(Sen) = 0) And _
'           (Loc = RDBData(i).ColInfo(j).Location Or Len(Loc) = 0) And _
'           (Con = RDBData(i).ColInfo(j).Constituent Or Len(Con) = 0) Then
'          ReDim Preserve lts(lc)
'          lts(lc).Type = "RDB"
'          lts(lc).FilIndex = i
'          lts(lc).Stanam = RDBData(i).ColInfo(j).Location
'          lts(lc).id = (20000 * (i + 1)) + j
'          Call J2Date(RDBData(i).JDay(0), lts(lc).sdat())
'          Call J2Date(RDBData(i).JDay(RDBData(i).TimCount - 1), lts(lc).edat())
'          lts(lc).Tu = 1  ' dummy
'          lts(lc).ts = 1  ' dummy
'          lts(lc).Sen = RDBData(i).ColInfo(j).Scenario
'          lts(lc).Loc = RDBData(i).ColInfo(j).Location
'          lts(lc).Con = RDBData(i).ColInfo(j).Constituent
'          lts(lc).NVal = RDBData(i).TimCount
'
'          lc = lc + 1
'        End If
'      Next j
'    Next i

'    'now check for external data
'    For i = 0 To p.ExtCount - 1
'      If (Sen = ExTS(i).Sen Or Len(Sen) = 0) And _
'         (Loc = ExTS(i).Loc Or Len(Loc) = 0) And _
'         (Con = ExTS(i).Con Or Len(Con) = 0) Then
'        ReDim Preserve lts(lc)
'        lts(lc) = ExTS(i)
'        lc = lc + 1
'      End If
'    Next i

End Sub

Public Function GetDescription$(SLorC$, itemname$)
  Dim i&
  GetDescription = ""
  If SLorC = "SCN" Then
    For i = 0 To p.ScenCount - 1
      If p.Scen(i).Name = itemname Then
        GetDescription = p.Scen(i).desc
        Exit Function
      End If
    Next i
  ElseIf SLorC = "CON" Then
    For i = 0 To p.ConsCount - 1
      If p.Cons(i).Name = itemname Then
        GetDescription = p.Cons(i).desc
        Exit Function
      End If
    Next i
  ElseIf SLorC = "LOC" Then
    For i = 0 To p.LocnCount - 1
      If p.Locn(i).Name = itemname Then
        GetDescription = p.Locn(i).desc
        Exit Function
      End If
    Next i
  End If
End Function

Public Function ScenPath(s$) As String
    Dim i%
    
    i = -1
    Do
      i = i + 1
      If p.Scen(i).Name = s Then
        ScenPath = p.Scen(i).File
        Exit Do
      ElseIf i = p.ScenCount - 1 Then
        ScenPath = s
        Exit Do
      End If
    Loop
End Function

Public Sub InitATCoTSer()
  Dim regKey As Variant
  Dim iKey As Integer 'index of registry key being examined
  Dim TserIndex As Long
  
  Set TserFiles = Nothing
  On Error GoTo TSerPlugInsError
  Set TserFiles = New ATCData.ATCPlugInManager
  On Error GoTo 0
  regKey = GetAllSettings("ATCoPlugin", TserFileClassName)
  If IsEmpty(regKey) Then
    SaveSetting "ATCoPlugin", TserFileClassName, TserFileClassName, TserFileClassName & ".dll"
    regKey = GetAllSettings("ATCoPlugin", TserFileClassName)
  End If
  For iKey = LBound(regKey, 1) To UBound(regKey, 1)
    If Not TserFiles.Load(CStr(regKey(iKey, 0))) Then
      MsgBox "Could not load Timeseries " & regKey(iKey, 0) & " from " & regKey(iKey, 1) & vbCr & TserFiles.ErrorDescription
    End If
  Next iKey
  TserIndex = TserFiles.AvailIndexByName("clsTSerMemory")
  If TserIndex > 0 Then
    Call TserFiles.Create(TserIndex)
    Set InMemFile = Nothing
    Set InMemFile = TserFiles.CurrentActive.obj
  End If

  Exit Sub
  
TSerPlugInsError:
  MsgBox "Could not load Timeseries from " & TserFileClassName & vbCr & TserFiles.ErrorDescription
End Sub

Public Sub AddUniqueSLC(s$, slc$)
  Dim i%, fnd As Boolean
  
  fnd = False
  Select Case slc
  
  Case "Scen":
    For i = LBound(p.Scen) To UBound(p.Scen)
      If s = p.Scen(i).Name Then
        fnd = True
        Exit For
      End If
    Next i
    If Not (fnd) Then
      ReDim Preserve p.Scen(p.ScenCount)
      p.Scen(p.ScenCount).Name = s
      p.ScenCount = p.ScenCount + 1
    End If
  Case "Locn":
    For i = LBound(p.Locn) To UBound(p.Locn)
      If s = p.Locn(i).Name Then
        fnd = True
        Exit For
      End If
    Next i
    If Not (fnd) Then
      ReDim Preserve p.Locn(p.LocnCount)
      p.Locn(p.LocnCount).Name = s
      p.LocnCount = p.LocnCount + 1
    End If
  Case "Cons":
    For i = LBound(p.Cons) To UBound(p.Cons)
      If s = p.Cons(i).Name Then
        fnd = True
        Exit For
      End If
    Next i
    If Not (fnd) Then
      ReDim Preserve p.Cons(p.ConsCount)
      p.Cons(p.ConsCount).Name = s
      p.ConsCount = p.ConsCount + 1
    End If
  End Select
  
End Sub

Sub FillReachDetails(ts As Collection)
  'dummy to keep refreshSLC consistent
End Sub

Sub dbg(Msg As String, Optional lev As Long = 0, Optional modul As String = "", Optional typ As String = "")
  IPC.dbg modul & ":" & Msg
  'Debug.Print Msg, modul
End Sub
