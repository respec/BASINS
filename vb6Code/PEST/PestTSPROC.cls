VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPROC"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pFileName As String       'name of TSPROC file
Private pSettings As clsPestTSPSettings
Private pEraseEntity As FastCollection 'of clsPestTSPErase
Private pExceedenceTime As FastCollection 'of clsPestTSPExceedence
Private pGetPLTGEN As FastCollection 'of clsPestTSPGetPLTGEN
Private pGetSSF As FastCollection 'of clsPestTSPGetSSF
Private pGetWDM As FastCollection 'of clsPestTSPGetWDM
Private pListOutput As FastCollection 'of clsPestTSPListOutput
Private pNewTimeBase As FastCollection 'of clsPestTSPNewTimeBase
Private pReduceSpan As FastCollection 'of clsPestTSPReduceSpan
Private pSeriesClean As FastCollection 'of clsPestTSPSeriesClean
Private pSeriesDisplace As FastCollection 'of clsPestTSPSeriesDisplace
Private pSeriesEquation As FastCollection 'of clsPestTSPSeriesEquation
Private pSeriesStats As FastCollection 'of clsPestTSPSeriesStats
Private pVolumeCalc As FastCollection 'of clsPestTSPVolumeCalc
Private pWritePest As clsPestTSPWritePest
Private pPestControlFile As String
Private pBatchFile As String
Private pWDMFileName As String
Private pIPC As ATCoIPC

Public Property Get Filename() As String
  Filename = pFileName
End Property
Public Property Let Filename(ByVal newvalue As String)
  pFileName = newvalue
  PopulateTSPROC
End Property

Public Property Get Settings() As clsPestTSPSettings
  Set Settings = pSettings
End Property
Public Property Let Settings(ByVal newvalue As clsPestTSPSettings)
  pSettings = newvalue
End Property

Public Property Get EraseEntity() As FastCollection
  Set EraseEntity = pEraseEntity
End Property
Public Property Let EraseEntity(ByVal newvalue As FastCollection)
  Set pEraseEntity = newvalue
End Property

Public Property Get ExceedenceTime() As FastCollection
  Set ExceedenceTime = pExceedenceTime
End Property
Public Property Let ExceedenceTime(ByVal newvalue As FastCollection)
  Set pExceedenceTime = newvalue
End Property

Public Property Get GetPLTGEN() As FastCollection
  Set GetPLTGEN = pGetPLTGEN
End Property
Public Property Let GetPLTGEN(ByVal newvalue As FastCollection)
  Set pGetPLTGEN = newvalue
End Property

Public Property Get GetSSF() As FastCollection
  Set GetSSF = pGetSSF
End Property
Public Property Let GetSSF(ByVal newvalue As FastCollection)
  Set pGetSSF = newvalue
End Property

Public Property Get GetWDM() As FastCollection
  Set GetWDM = pGetWDM
End Property
Public Property Let GetWDM(ByVal newvalue As FastCollection)
  Set pGetWDM = newvalue
End Property

Public Property Get ListOutput() As FastCollection
  Set ListOutput = pListOutput
End Property
Public Property Let ListOutput(ByVal newvalue As FastCollection)
  Set pListOutput = newvalue
End Property

Public Property Get NewTimeBase() As FastCollection
  Set NewTimeBase = pNewTimeBase
End Property
Public Property Let NewTimeBase(ByVal newvalue As FastCollection)
  Set pNewTimeBase = newvalue
End Property

Public Property Get ReduceSpan() As FastCollection
  Set ReduceSpan = pReduceSpan
End Property
Public Property Let ReduceSpan(ByVal newvalue As FastCollection)
  Set pReduceSpan = newvalue
End Property

Public Property Get SeriesClean() As FastCollection
  Set SeriesClean = pSeriesClean
End Property
Public Property Let SeriesClean(ByVal newvalue As FastCollection)
  Set pSeriesClean = newvalue
End Property

Public Property Get SeriesDisplace() As FastCollection
  Set SeriesDisplace = pSeriesDisplace
End Property
Public Property Let SeriesDisplace(ByVal newvalue As FastCollection)
  Set pSeriesDisplace = newvalue
End Property

Public Property Get SeriesEquation() As FastCollection
  Set SeriesEquation = pSeriesEquation
End Property
Public Property Let SeriesEquation(ByVal newvalue As FastCollection)
  Set pSeriesEquation = newvalue
End Property

Public Property Get SeriesStats() As FastCollection
  Set SeriesStats = pSeriesStats
End Property
Public Property Let SeriesStats(ByVal newvalue As FastCollection)
  Set pSeriesStats = newvalue
End Property

Public Property Get VolumeCalc() As FastCollection
  Set VolumeCalc = pVolumeCalc
End Property
Public Property Let VolumeCalc(ByVal newvalue As FastCollection)
  Set pVolumeCalc = newvalue
End Property

Public Property Get WritePest() As clsPestTSPWritePest
  Set WritePest = pWritePest
End Property
Public Property Let WritePest(ByVal newvalue As clsPestTSPWritePest)
  pWritePest = newvalue
End Property

Public Property Get PestControlFile() As String
  PestControlFile = pPestControlFile
End Property
Public Property Let PestControlFile(ByVal newvalue As String)
  pPestControlFile = newvalue
End Property

Public Property Get BatchFile() As String
  BatchFile = pBatchFile
End Property
Public Property Let BatchFile(ByVal newvalue As String)
  pBatchFile = newvalue
End Property

Public Property Get WDMFileName() As String
  WDMFileName = pWDMFileName
End Property
Public Property Let WDMFileName(ByVal newvalue As String)
  pWDMFileName = newvalue
End Property

Public Property Get IPC() As ATCoIPC
  Set IPC = pIPC
End Property
Public Property Let IPC(newvalue As ATCoIPC)
  Set pIPC = newvalue
End Property

Private Sub PopulateTSPROC()
  Dim FUnit As Integer
  Dim str As String, comstr As String, blkstr As String, kwd As String
  Dim lErEnt As clsPestTSPErase
  Dim lExcTime As clsPestTSPExceedence
  Dim lGetPltGen As clsPestTSPGetPLTGEN
  Dim lGetSSF As clsPestTSPGetSSF
  Dim lGetWDM As clsPestTSPGetWDM
  Dim lListOutput As clsPestTSPListOutput
  Dim lNewTimeBase As clsPestTSPNewTimeBase
  Dim lReduceSpan As clsPestTSPReduceSpan
  Dim lSeriesClean As clsPestTSPSeriesClean
  Dim lSeriesDisplace As clsPestTSPSeriesDisplace
  Dim lSeriesEquation As clsPestTSPSeriesEquation
  Dim lSeriesStats As clsPestTSPSeriesStats
  Dim lVolumeCalc As clsPestTSPVolumeCalc

  FUnit = FreeFile(0)
  Open pFileName For Input As #FUnit
  comstr = ""
  While Not EOF(FUnit)
    Line Input #FUnit, str
    str = Trim(str)
    If Left(str, 1) = "#" Then 'comment line
      If Len(comstr) > 0 Then
        comstr = comstr & vbCrLf & str
      Else
        comstr = str
      End If
    ElseIf Left(str, 6) = "START " Then
      kwd = Trim(Mid(str, 7))
      blkstr = ""
      Line Input #FUnit, str 'read first line of block
      While Left(LTrim(str), 4) <> "END "
        If Len(blkstr) > 0 Then
          blkstr = blkstr & vbCrLf & str
        Else
          blkstr = str
        End If
        Line Input #FUnit, str
      Wend
      Select Case kwd
        Case "SETTINGS"
          pSettings.Comments = comstr
          pSettings.Populate (blkstr)
        Case "ERASE_ENTITY"
          Set lErEnt = New clsPestTSPErase
          lErEnt.Comments = comstr
          lErEnt.Populate (blkstr)
          pEraseEntity.Add lErEnt
        Case "EXCEEDENCE_TIME"
          Set lExcTime = New clsPestTSPExceedence
          lExcTime.Comments = comstr
          lExcTime.Populate (blkstr)
          pExceedenceTime.Add lExcTime
        Case "GET_SERIES_PLOTGEN"
          Set lGetPltGen = New clsPestTSPGetPLTGEN
          lGetPltGen.Comments = comstr
          lGetPltGen.Populate (blkstr)
          pGetPLTGEN.Add lGetPltGen
        Case "GET_SERIES_SSF"
          Set lGetSSF = New clsPestTSPGetSSF
          lGetSSF.Comments = comstr
          lGetSSF.Populate (blkstr)
          pGetSSF.Add lGetSSF
        Case "GET_SERIES_WDM"
          Set lGetWDM = New clsPestTSPGetWDM
          lGetWDM.Comments = comstr
          lGetWDM.Populate (blkstr)
          pGetWDM.Add lGetWDM
        Case "LIST_OUTPUT"
          Set lListOutput = New clsPestTSPListOutput
          lListOutput.Comments = comstr
          lListOutput.Populate (blkstr)
          pListOutput.Add lListOutput
        Case "NEW_TIME_BASE"
          Set lNewTimeBase = New clsPestTSPNewTimeBase
          lNewTimeBase.Comments = comstr
          lNewTimeBase.Populate (blkstr)
          pNewTimeBase.Add lNewTimeBase
        Case "REDUCE_TIME_SPAN"
          Set lReduceSpan = New clsPestTSPReduceSpan
          lReduceSpan.Comments = comstr
          lReduceSpan.Populate (blkstr)
          pReduceSpan.Add lReduceSpan
        Case "SERIES_CLEAN"
          Set lSeriesClean = New clsPestTSPSeriesClean
          lSeriesClean.Comments = comstr
          lSeriesClean.Populate (blkstr)
          pSeriesClean.Add lSeriesClean
        Case "SERIES_DISPLACE"
          Set lSeriesDisplace = New clsPestTSPSeriesDisplace
          lSeriesDisplace.Comments = comstr
          lSeriesDisplace.Populate (blkstr)
          pSeriesDisplace.Add lSeriesDisplace
        Case "SERIES_EQUATION"
          Set lSeriesEquation = New clsPestTSPSeriesEquation
          lSeriesEquation.Comments = comstr
          lSeriesEquation.Populate (blkstr)
          pSeriesEquation.Add lSeriesEquation
        Case "SERIES_STATISTICS"
          Set lSeriesStats = New clsPestTSPSeriesStats
          lSeriesStats.Comments = comstr
          lSeriesStats.Populate (blkstr)
          pSeriesStats.Add lSeriesStats
        Case "VOLUME_CALCULATION"
          Set lVolumeCalc = New clsPestTSPVolumeCalc
          lVolumeCalc.Comments = comstr
          lVolumeCalc.Populate (blkstr)
          pVolumeCalc.Add lVolumeCalc
        Case "WRITE_PEST_FILES"
          pWritePest.Comments = comstr
          pWritePest.Populate (blkstr)
      End Select
      comstr = ""
    End If
  Wend

  Close #FUnit
End Sub

Public Function WriteFileAsString() As String
  Dim str As String
  Dim lv As Variant

  'start with Settings block (required)
  str = pSettings.WriteBlock & vbCrLf
  If pEraseEntity.Count > 0 Then
    For Each lv In pEraseEntity
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pGetPLTGEN.Count > 0 Then
    For Each lv In pGetPLTGEN
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pGetSSF.Count > 0 Then
    For Each lv In pGetSSF
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pGetWDM.Count > 0 Then
    For Each lv In pGetWDM
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pNewTimeBase.Count > 0 Then
    For Each lv In pNewTimeBase
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pReduceSpan.Count > 0 Then
    For Each lv In pReduceSpan
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pSeriesClean.Count > 0 Then
    For Each lv In pSeriesClean
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pSeriesDisplace.Count > 0 Then
    For Each lv In pSeriesDisplace
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pSeriesEquation.Count > 0 Then
    For Each lv In pSeriesEquation
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pSeriesStats.Count > 0 Then
    For Each lv In pSeriesStats
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pVolumeCalc.Count > 0 Then
    For Each lv In pVolumeCalc
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pExceedenceTime.Count > 0 Then
    For Each lv In pExceedenceTime
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pListOutput.Count > 0 Then
    For Each lv In pListOutput
      str = str & vbCrLf & lv.WriteBlock & vbCrLf
    Next
  End If
  If pWritePest.Context.Count > 0 Then
    str = str & vbCrLf & pWritePest.WriteBlock & vbCrLf
  End If
  WriteFileAsString = str
End Function

Private Sub Class_Initialize()
  pFileName = ""
  Set pSettings = New clsPestTSPSettings
  Set pEraseEntity = New FastCollection
  Set pExceedenceTime = New FastCollection
  Set pGetPLTGEN = New FastCollection
  Set pGetSSF = New FastCollection
  Set pGetWDM = New FastCollection
  Set pListOutput = New FastCollection
  Set pNewTimeBase = New FastCollection
  Set pReduceSpan = New FastCollection
  Set pSeriesClean = New FastCollection
  Set pSeriesDisplace = New FastCollection
  Set pSeriesEquation = New FastCollection
  Set pSeriesStats = New FastCollection
  Set pVolumeCalc = New FastCollection
  Set pWritePest = New clsPestTSPWritePest
  pPestControlFile = ""
  pBatchFile = ""
  pWDMFileName = ""
End Sub

Public Function BasinsHSPFInit(myUCI As HspfUci, ObsDSN As Long, SimDSN As Long) As String
  Dim GetSeriesWDM As clsPestTSPGetWDM
  Dim NewTBase As clsPestTSPNewTimeBase
  Dim VolCalc As clsPestTSPVolumeCalc
  Dim Exceed As clsPestTSPExceedence
  Dim ListOut As clsPestTSPListOutput
  Dim Ser As clsPestSeries
  Dim wdm As ATCclsTserFile
  Dim FName As String, outstr As String
  Dim sDateStr As String, sTimeStr As String, eDateStr As String, eTimeStr As String
  Dim FUnit As Long
  Dim jsd As Double, jed As Double, ejd As Double
  Dim ts As ATCclsTserData, lts As ATCclsTserData, mts As ATCclsTserData
  Dim clas() As Single, c As Double, cr As Single, clog As Single, bound(1) As Single
  Dim iexp As Long, i As Long, j As Long, l As Long
  Dim lsd(5) As Long, led(5) As Long
  Dim ltyp As String, ftyp As String
  Dim lf As HspfFile
  Const NExcInts As Long = 20

  On Error GoTo OuttaHere

  pFileName = "tsproc.dat"
  'use full path to preserve WDM file reference
  For i = 1 To myUCI.FilesBlock.Count
    If Left(myUCI.FilesBlock.Value(i).Typ, 3) = "WDM" Then
      If myUCI.FilesBlock.Value(i).Typ = "WDM" Then
        ftyp = "WDM1"
      Else
        ftyp = myUCI.FilesBlock.Value(i).Typ
      End If
      For j = 1 To myUCI.WDMCount
        myUCI.GetWDMIDFromUnit myUCI.GetWDMObj(j).FileUnit, ltyp
        If ftyp = ltyp Then
          lf.Name = myUCI.GetWDMObj(j).Filename
          lf.Typ = myUCI.FilesBlock.Value(i).Typ
          lf.Unit = myUCI.FilesBlock.Value(i).Unit
          lf.Comment = myUCI.FilesBlock.Value(i).Comment
          myUCI.FilesBlock.Value(i) = lf
        End If
      Next j
    End If
  Next i
  Set ts = Nothing
  i = 1
  While ts Is Nothing And i <= myUCI.WDMCount
    Set ts = myUCI.GetDataSetFromDsn(i, ObsDSN)
    If ts Is Nothing Then i = i + 1 'try next WDM file in UCI
  Wend
  If ts Is Nothing Then 'can't find observed DSN
    Err.Description = "Could not find Observed Data set (" & ObsDSN & ") on WDM file."
    Err.Raise 1
  Else
    Set wdm = New ATCclsTserFile
    Set wdm = myUCI.GetWDMObj(i) 'set to WDM on which Obs DSN was found
    pWDMFileName = wdm.Filename
  End If
  For i = 0 To 5
    lsd(i) = myUCI.GlobalBlock.SDate(i)
    led(i) = myUCI.GlobalBlock.EDate(i)
  Next i
  jsd = Date2J(lsd)
  jed = Date2J(led)
  Set lts = ts.SubSetByDate(jsd, jed)
  ReDim clas(NExcInts)
'  iexp = Fix(Log10(lts.Max))
'  bound(1) = 10# ^ (iexp + 1)
  'just round max value to 3 digits for max exceedence interval
  bound(1) = Signif(lts.Max, False)
  If lts.Min < 0 Then
    iexp = Int(Log10(lts.Max / 1000))
  Else
    iexp = Int(Log10(lts.Min))
  End If
  bound(0) = 10# ^ (iexp)
  'set up class intervals
  cr = (bound(0) / bound(1)) ^ (1# / (NExcInts + 1))
  clas(0) = 0#
  clas(1) = bound(0)
  clas(NExcInts) = bound(1)
  For j = 1 To NExcInts - 2
    i = NExcInts - j
    clas(i) = clas(i + 1) * cr
  Next j
  'round off class intervals
  For i = 1 To NExcInts
    c = clas(i)
    clog = Log10(c) + 0.001
    If clog < 0# Then clog = clog - 1
    l = Fix(clog)
    l = l - 1
    c = (c / (10# ^ l)) + 0.5
    clas(i) = (Fix(c)) * (10# ^ l)
  Next i
  'build dates file
  FName = "dates.dat"
  FUnit = FreeFile(0)
  Open FName For Output As #FUnit
  While ejd < jed
    TIMADD lsd, 5, 1, 1, led
    ejd = Date2J(led)
    If ejd >= jed Then 'only go as far as end date of simulation
      ejd = jed
      J2Date ejd, led
      timcnv led
      outstr = NumFmtI(lsd(1), 2) & "/01/" & lsd(0) & "  24:00:00    " & _
               NumFmtI(led(1), 2) & "/" & led(2) & "/" & led(0) & "  24:00:00"
    Else
      outstr = NumFmtI(lsd(1), 2) & "/01/" & lsd(0) & "  24:00:00    " & _
               NumFmtI(led(1), 2) & "/01/" & led(0) & "  24:00:00"
    End If
    Print #FUnit, outstr
    For i = 0 To 5
      lsd(i) = led(i)
    Next i
  Wend
  Close #FUnit
  sDateStr = myUCI.GlobalBlock.SDate(1) & "/" & myUCI.GlobalBlock.SDate(2) & "/" & myUCI.GlobalBlock.SDate(0)
  sTimeStr = NumFmtI(myUCI.GlobalBlock.SDate(3), 2) & ":" & NumFmtI(myUCI.GlobalBlock.SDate(4), 2) & ":00"
  If myUCI.GlobalBlock.SDate(3) < 10 Then sTimeStr = "0" & Trim(sTimeStr)
  If myUCI.GlobalBlock.SDate(4) < 10 Then sTimeStr = Left(sTimeStr, 3) & "0" & Right(sTimeStr, 4)
  eDateStr = myUCI.GlobalBlock.EDate(1) & "/" & myUCI.GlobalBlock.EDate(2) & "/" & myUCI.GlobalBlock.EDate(0)
  eTimeStr = NumFmtI(myUCI.GlobalBlock.EDate(3), 2) & ":" & NumFmtI(myUCI.GlobalBlock.EDate(4), 2) & ":00"
  If myUCI.GlobalBlock.EDate(3) < 10 Then eTimeStr = "0" & Trim(eTimeStr)
  If myUCI.GlobalBlock.EDate(4) < 10 Then eTimeStr = Left(eTimeStr, 3) & "0" & Right(eTimeStr, 4)
  pSettings.DateFormat = "mm/dd/yyyy"
  pSettings.Context.Add "pest_prep"
  pSettings.ContextOn.Add True, "pest_prep" 'initial run is for model set up
  pSettings.Context.Add "model_run"
  pSettings.ContextOn.Add False, "model_run"
  'look for Simulated DSN on WDM file(s)
  Set ts = Nothing
  i = 1
  While ts Is Nothing And i <= myUCI.WDMCount
    Set ts = myUCI.GetDataSetFromDsn(i, SimDSN)
    If ts Is Nothing Then i = i + 1 'try next WDM file in UCI
  Wend
  If ts Is Nothing Then 'can't find observed DSN
    Err.Description = "Could not find Modeled Data set (" & SimDSN & ") on WDM file."
    Err.Raise 1
  Else
    'build get modeled flow block
    Set GetSeriesWDM = New clsPestTSPGetWDM
    With GetSeriesWDM
      .Context.Add "all"
      .WDMFile = myUCI.GetWDMObj(i).Filename
      .DSN = SimDSN
      .DATE1 = sDateStr
      .Time1 = sTimeStr
      .DATE2 = eDateStr
      .Time2 = eTimeStr
      .SeriesName = "nflow"
    End With
    pGetWDM.Add GetSeriesWDM
  End If
  'build get observed flow block
  Set GetSeriesWDM = New clsPestTSPGetWDM
  With GetSeriesWDM
    .Context.Add "all"
    .WDMFile = pWDMFileName
    .DSN = ObsDSN
    .DATE1 = sDateStr
    .Time1 = sTimeStr
    .DATE2 = eDateStr
    .Time2 = eTimeStr
    .SeriesName = "oflow"
  End With
  pGetWDM.Add GetSeriesWDM
  'build new time base block
  Set NewTBase = New clsPestTSPNewTimeBase
  With NewTBase
    .Context.Add "all"
    .SeriesName = "nflow"
    .TBSeriesName = "oflow"
    .NewSeriesName = "mflow"
  End With
  pNewTimeBase.Add NewTBase
  'build modeled volume calculation block
  Set VolCalc = New clsPestTSPVolumeCalc
  With VolCalc
    .Context.Add "all"
    .SeriesName = "mflow"
    .DateFileName = "dates.dat"
    .NewVTableName = "mvol"
    .TimeUnits = "seconds"
  End With
  pVolumeCalc.Add VolCalc
  'build observed volume calculation block
  Set VolCalc = New clsPestTSPVolumeCalc
  With VolCalc
    .Context.Add "pest_prep"
    .SeriesName = "oflow"
    .DateFileName = "dates.dat"
    .NewVTableName = "ovol"
    .TimeUnits = "seconds"
  End With
  pVolumeCalc.Add VolCalc
  'build modeled exceedence block
  Set Exceed = New clsPestTSPExceedence
  With Exceed
    .Context.Add "all"
    .SeriesName = "mflow"
    .TimeUnits = "days"
    .ETableName = "mtime"
    For i = 1 To NExcInts
      .Flow.Add clas(i)
    Next i
  End With
  pExceedenceTime.Add Exceed
  'build observed exceedence block
  Set Exceed = New clsPestTSPExceedence
  With Exceed
    .Context.Add "pest_prep"
    .SeriesName = "oflow"
    .TimeUnits = "days"
    .ETableName = "otime"
    For i = 1 To NExcInts
      .Flow.Add clas(i)
    Next i
  End With
  pExceedenceTime.Add Exceed
  'build list output block
  Set ListOut = New clsPestTSPListOutput
  With ListOut
    .Context.Add "all"
    .ListFile = "model.out"
    .SeriesName.Add "mflow"
    .VTableName.Add "mvol"
    .ETableName.Add "mtime"
    .SeriesFormat = "short"
  End With
  pListOutput.Add ListOut
  'build write pest files block
  With pWritePest
    .ContextCom.Add "# General information", "pest_prep"
    .Context.Add "pest_prep"
    pPestControlFile = "control.pst"
    .ControlFile = pPestControlFile
    FName = LCase(FilenameNoExt(FilenameNoPath(myUCI.Name)))
    .TemplateFile.Add "p2p_" & FName & ".tpl"
    .ModelInputFile.Add "p2p_" & FName & ".dat", "p2p_" & FName & ".tpl"
    .InstructFile = "model.ins"
    .ParmDataFile = "parm.dat"
    .ParmGroupFile = "group.dat"
    pBatchFile = "model.bat"
    .ModelCommandLine = "'batch " & pIPC.hPipeWriteToProcess(0) & " " & pIPC.hPipeReadFromProcess(0) & " " & pBatchFile & "'"
  End With
  Set Ser = New clsPestSeries
  With Ser
    .SeriesType = "t"
    .ObsSeries = "oflow"
    .ModelSeries = "mflow"
    .WeightEquation = "1.0e-3/(@_abs_value+0.001)" '"1.0/(@_abs_value+0.001)"
    .WeightMin = 0.0000001
    .WeightMax = 1000#
  End With
  pWritePest.Series.Add Ser
  Set Ser = New clsPestSeries
  With Ser
    .SeriesType = "v"
    .ObsSeries = "ovol"
    .ModelSeries = "mvol"
    .WeightEquation = "1.0e-3/sqrt(@_abs_value)" '"1.0e-2/sqrt(@_abs_value)"
  End With
  pWritePest.Series.Add Ser
  Set Ser = New clsPestSeries
  With Ser
    .SeriesType = "e"
    .ObsSeries = "otime"
    .ModelSeries = "mtime"
    .WeightEquation = "1000.0" '"300.0"
  End With
  pWritePest.Series.Add Ser
  Exit Function
  
OuttaHere:
  BasinsHSPFInit = "BasinsHSPFInit: " & Err.Description
End Function


