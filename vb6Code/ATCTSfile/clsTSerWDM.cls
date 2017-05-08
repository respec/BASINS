VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerWDM"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Implements ATCclsTserFile

Private pDcnt As Long
Private pFileExt As String
Private pErrorDescription As String
Private pMonitor As Object
Private pMonitorSet As Boolean
Private pData As Collection 'of ATCclsTserData
Private pDates As Collection 'of ATCclsTserDate
Private pFilename As String
Private pFileWholeName As String
Private pFileUnit As Long
Private pQuick As Boolean

Dim DsnDescs() As String, nDD As Long

Type BasinsInfo
  desc As String
  Nam As String
  Elev As Single
  sdat(3) As Long
  edat(3) As Long
  EvapCoef As Single
  dsn(7) As Long
End Type
Dim BasInf() As BasinsInfo
Dim BasInfAvail As Boolean

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Dim lRetval As Collection, vWDMAttr As Variant
  Dim CurWDMAttr As clsAttributeWDM
  Dim CurTSerAttr As ATCclsAttributeDefinition
  Set lRetval = New Collection
  For Each vWDMAttr In gMsg.Attributes
    Set CurTSerAttr = New ATCclsAttributeDefinition
    Set CurWDMAttr = vWDMAttr
    With CurTSerAttr
      .DataType = CurWDMAttr.DataType
      .Default = CurWDMAttr.Default
      .Description = CurWDMAttr.Description
      .Max = CurWDMAttr.Max
      .Min = CurWDMAttr.Min
      .Name = CurWDMAttr.Name
      .ValidValues = CurWDMAttr.ValidValues
    End With
    lRetval.Add CurTSerAttr
    Set CurTSerAttr = Nothing
  Next
  Set ATCclsTserFile_AvailableAttributes = lRetval
End Property

Private Sub ATCclsTserFile_clear()
  Dim lRet As Long
  Dim lWdmOpen As Long, lWDMUnit As Long
  
  If pFileUnit > 0 Then
    lWDMUnit = F90_INQNAM(pFileWholeName, Len(pFileWholeName))
    If lWDMUnit > 0 Then
      lRet = F90_WDMCLO(lWDMUnit)
    End If
    
    'force close
    lWdmOpen = F90_WDMOPN(pFileUnit, pFileWholeName, Len(pFileWholeName))
    If lWdmOpen > -1 Then 'the file is open, close it
      lRet = F90_WDFLCL(pFileUnit)
    End If
    If lRet <> 0 And lRet <> -87 Then
      MsgBox "Close WDM file on " & pFileUnit & " caused retcod " & lRet
    End If
  End If

  Set pData = Nothing
  Set pDates = Nothing
End Sub

Private Property Get ATCclsTserFile_Data(index As Long) As ATCclsTserData
  Set ATCclsTserFile_Data = pData(index)
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = pData.Count
End Property

Private Property Get ATCclsTserfile_DataCollection() As Collection
  Set ATCclsTserfile_DataCollection = pData
End Property

Private Property Get ATCclsTserFile_FileUnit() As Long
  ATCclsTserFile_FileUnit = pFileUnit
End Property

Private Property Let ATCclsTserFile_HelpFilename(ByVal newValue As String)
  App.HelpFile = newValue
End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "WDM"
End Property

Private Sub ATCclsTserFile_readData(t As ATCclsTserData)
  Dim v!(), n&, retc&, sdat&(6), edat&(6), i, f&(), lDateSummary As ATTimSerDateSummary
  Dim lWdmOpen&
  
  'Debug.Print "WDM read data " & t.Attrib("Location")
  lWdmOpen = F90_WDMOPN(pFileUnit, pFileWholeName, Len(pFileWholeName))
  
  lDateSummary = t.Dates.Summary
  
  If Not CBool(t.Attrib("HeaderComplete")) Then
    DsnReadGeneral t.Header.id, t.Header, lDateSummary
    t.Dates.Summary = lDateSummary
  End If
  
  If Not CBool(t.Attrib("HeaderOnly")) Then
    With lDateSummary
      Call J2Date(.SJDay, sdat)
      If .NVALS = 0 Then
        Call J2Date(.EJDay, edat)
        Call timdif(sdat, edat, .Tu, .ts, .NVALS)
      End If
      If .NVALS > 0 Then
        ReDim v(.NVALS)
        ReDim f(.NVALS)
        Call F90_WDTGET(pFileUnit, CLng(t.Header.id), _
                        .ts, sdat(0), .NVALS, CLng(0), CLng(31), .Tu, _
                        v(1), retc)
        'ReDim ATCclsTserFile_dataser(.NVALS)
        'For n = 1 To .NVALS
        '  If v(n) <> 0 Then Stop
        'Next
      Else
        ReDim v(0)
        ReDim f(0)
      End If
    End With
    t.flags = f
    t.Values = v
    t.calcSummary
    t.Dates.Summary = lDateSummary
    ' next 2 might be automatic
    ReDim v(0)
    ReDim f(0)
  End If
  
  If lWdmOpen <> 1 Then
    i = F90_WDMCLO(pFileUnit)
  End If
  
End Sub

'oldHeader must be given if newheader has a changed id
Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  Dim dsn&, saind&, salen&, retcod&, wdmfg&, s$, l$, c$, d$, i&
  Dim lHeader As ATTimSerDataHeader
  Dim lWdmOpen As Long
  Dim lMsg As String
  
  lMsg = "clsTSerWDM:WriteDataHeader:" & gMsgUnit & ":" & pFileUnit & ":" & pFileWholeName
  Call F90_MSG(lMsg, Len(lMsg))
  
  lWdmOpen = F90_WDMOPN(pFileUnit, pFileWholeName, Len(pFileWholeName))
  
  Set lHeader = dataObject.Header
  
  retcod = 0
  dsn = lHeader.id
  On Error GoTo Cntinu
    '  this IsMissing function didn't work on oldHeader for some reason
    '  If Not IsMissing(oldHeader) Then 'may need to update id
    If oldHeader.id <> dsn Then 'try to change dsn
      Call F90_WDDSRN(pFileUnit, oldHeader.id, dsn, retcod)
    End If
    '  End If
Cntinu:
  If retcod = 0 Then
    saind = 288
    salen = 8
    s = lHeader.Sen
    i = 1
    Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, s, Len(s))
    If retcod = 0 Then
      saind = 289
      c = lHeader.con
      i = 2
      Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, c, Len(c))
      If retcod = 0 Then
        saind = 290
        l = lHeader.loc
        i = 3
        Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, l, Len(l))
        If retcod = 0 Then
          saind = 45
          salen = 48
          d = lHeader.desc
          If Len(d) > salen Then
            MsgBox "Description: '" & d & vbCr & "truncated to: " & Left(d, salen), vbExclamation, "WDM Write Data Header"
          End If
          i = 4
          Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, d, Len(d))
        End If
      End If
    End If
  End If
  
  If retcod = 0 Then
    ATCclsTserFile_writeDataHeader = DsnWriteAttributes(dataObject)
  Else
    If Abs(retcod) = 73 Then
      pErrorDescription = "Unable to renumber Dataset " & oldHeader.id & " to " & dsn
    Else
      pErrorDescription = "Unable to Write a Data Header for Class WDM, Retcod:" & retcod & " from " & i
    End If
    ATCclsTserFile_writeDataHeader = False
  End If
  If lWdmOpen <> 1 Then
    i = F90_WDMCLO(pFileUnit)
  End If
  
End Function

Private Sub Class_Initialize()
  pFileExt = "WDM Files (*.wdm)|*.wdm"
  pErrorDescription = ""
  pMonitorSet = False
  pFilename = "<unknown>"
  pFileUnit = 0
  pQuick = False
  
  Set pData = Nothing
  Set pData = New Collection
  
  Set pDates = Nothing
  Set pDates = New Collection
  
  Dim lMsg As String
  Call F90_MSG("WRITE", 5)
  lMsg = "clsTSerWDM:Initialize:VB6"
  Call F90_MSG(lMsg, Len(lMsg))
End Sub

Private Property Set ATCclsTserFile_Monitor(ByVal o As Object)
  Set pMonitor = o
  pMonitorSet = True
End Property

Private Property Get ATCclsTserFile_ErrorDescription() As String
  ATCclsTserFile_ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

Private Property Get ATCclsTserFile_FileExtension() As String
  ATCclsTserFile_FileExtension = pFileExt
End Property

Public Property Get MsgUnit() As Long
  MsgUnit = gMsgUnit
End Property
Public Property Let MsgUnit(newValue As Long)
  SetMsgUnit newValue
End Property

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = pFilename
End Property
Private Property Let ATCclsTserFile_FileName(ByVal aFileName As String)
  Dim lRwFlg As Long, lAttr As Long, lFileUnit As Long, lRetcod As Long, lMsg As String
  lRwFlg = 0
    
  lMsg = "clsTSerWDM:FileName:Let" & aFileName
  Call F90_MSG(lMsg, Len(lMsg))
  
  If Len(Dir(aFileName)) = 0 Then
    pErrorDescription = "FileName '" & aFileName & "' not found"
  Else
    pFilename = AbsolutePath(aFileName, CurDir)
    
    lAttr = GetAttr(pFilename) 'if read only, change to not read only
    If (lAttr And vbReadOnly) <> 0 Then
      lAttr = lAttr - vbReadOnly
      SetAttr pFilename, lAttr
    End If
    
    lFileUnit = F90_INQNAM(pFilename, Len(pFilename))
    If lFileUnit > 0 Then
      pFileUnit = lFileUnit
      'i = F90_WDMCLO(lFileUnit)
    End If

    If pFileUnit = 0 Then
      lFileUnit = -1 'force no reuse of unit numbers
      F90_WDBOPNR lRwFlg, pFilename, lFileUnit, lRetcod, Len(pFilename)
      pFileUnit = lFileUnit
    End If
    If lRetcod = 159 Then
      'file is already open by another application
      'pFileUnit = 0
    End If
    If pFileUnit = 0 Then 'invalid WDM file
      pErrorDescription = "FileName '" & aFileName & "' is not a valid WDM file.  Retcod = " & str(lRetcod)
    Else
      pFileWholeName = AbsolutePath(pFilename, CurDir)
      BasInfAvail = ReadBasInf
      pQuick = True
      ATCclsTserFile_refresh
      pQuick = False
      If F90_WDMCLO(pFileUnit) <> 0 Then
        MsgBox "Could Not Close WDM on Unit " & pFileUnit, vbExclamation, "clsTSerWDM:FileName"
      End If
    End If
  End If
  If Len(pErrorDescription) = 0 Then
    lMsg = "clsTSerWDM:FileName:Let:Exit:" & pFileUnit & ":" & lRetcod
  Else
    lMsg = "clsTSerWDM:FileName:Let:Error" & pErrorDescription
  End If
  Call F90_MSG(lMsg, Len(lMsg))
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "WDM Time Series Data Type"
End Property

Public Sub ATCclsTserFile_refresh()
  Dim s$, l$, c$, dsn& ', cnt&
  Dim lDsn&
  Dim t#, i&
  Dim lWdmOpen As Long
  
  lWdmOpen = F90_WDMOPN(pFileUnit, pFileWholeName, Len(pFileWholeName))
  
  Set pData = Nothing
  Set pData = New Collection
  
  Set pDates = Nothing
  Set pDates = New Collection
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(OPEN WDM File)"
    pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
    pMonitor.SendMonitorMessage "(MSG1 " & pFilename & ")"
  End If
  
  t = Timer
  dsn = 1
  While dsn > 0
  'While dsn < 10000
    'If F90_WDCKDT(pFileUnit, dsn) = 1 Then
    lDsn = dsn
    F90_WDDSNX pFileUnit, dsn
    If dsn > 0 Then
      If F90_WDCKDT(pFileUnit, dsn) = 1 Then
        RefreshDsn dsn
      End If
    End If
    If pMonitorSet Then
      'If dsn Mod 500 = 1 Then
      If dsn - lDsn > 100 Then
        s = "(PROGRESS " & CStr((100 * dsn) / 32000) & ")"
        pMonitor.SendMonitorMessage s
      ElseIf dsn = -1 Then
        s = "(PROGRESS 100)"
        pMonitor.SendMonitorMessage s
      End If
    End If
    dsn = dsn + 1
  Wend
  t = Timer - t
  Debug.Print "clsTSerWDM:Refresh:" & pFilename & " took " & Format(t, "##0.0") & " sec"
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(CLOSE)"
    pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
  End If
  If lWdmOpen <> 1 Then i = F90_WDMCLO(pFileUnit)
  
End Sub

Public Function GetDataSetFromDsn(lDsn&) As ATCclsTserData
  Dim i&
  
  For i = 1 To pData.Count
    If lDsn = pData(i).Header.id Then
      Set GetDataSetFromDsn = pData(i)
      Exit Function
    End If
  Next i
  MsgBox "DSN " & lDsn & " does not exist.", vbOKOnly
End Function

Public Sub RefreshDsn(dsn As Long)
  Dim lData As ATCclsTserData
  Dim lDates As ATCclsTserDate
  Dim lDataHeader As ATTimSerDataHeader
  Dim lDateSum As ATTimSerDateSummary
  Dim lAttr As ATTimSerAttribute
  Dim sdat&(6), edat&(6), GRPSIZ&
  Dim hdrSen$, hdrLoc$, hdrCon$
  Dim s$, n$, Init&
  Dim saind&, saval&(256)
  Dim lWdmOpen As Long
  Dim i&

  lWdmOpen = F90_WDMOPN(pFileUnit, pFileWholeName, Len(pFileWholeName))
  Set lDataHeader = New ATTimSerDataHeader
  Call DsnReadGeneral(dsn, lDataHeader, lDateSum)
   
  Set lDates = Nothing
  Set lDates = New ATCclsTserDate
  lDates.Summary = lDateSum
  Set lDates.File = Me
  pDates.Add lDates
  
  Set lData = Nothing
  Set lData = New ATCclsTserData
  
  Init = 1
  Do
    F90_GETATT pFileUnit, dsn, Init, saind, saval(0)
    If saind > 0 Then
      If Not (AttrStored(saind)) Then
        n = gMsg.Attributes(saind).Name
        s = AttrVal2String(saind, saval)
        If n = "DCODE" Then
          lData.AttribSet "Units", GetUnitName(s), UnitsAttributeDefinition(True)
        Else
          lData.AttribSet n, s, gMsg.Attributes(saind)
        End If
        'Debug.Print "AttribSet:" & SAIND & ":" & n & ":" & s
      End If
    Else
      Exit Do
    End If
  Loop
  If Len(lData.Attrib("Units")) = 0 Then
    lData.AttribSet "Units", "Unknown", UnitsAttributeDefinition(True)
  End If
  If pQuick Then
    lData.AttribSet "HeaderComplete", 0 'false, must do later when used
  End If
  
  Set lData.Header = lDataHeader
  If lData.Header.loc = "<unk>" And BasInfAvail Then 'no location, try to check BASINS inf file
    Call HeaderFromBasinsInf(lData)
  End If
  
  Set lData.Dates = lDates
  Set lData.File = Me
  pData.Add lData
  If lWdmOpen <> 1 Then i = F90_WDMCLO(pFileUnit)
End Sub

Private Sub DsnReadGeneral(dsn&, lDataHeader As ATTimSerDataHeader, lDateSum As ATTimSerDateSummary)
  Dim salen&, saind&, retcod&
  Dim lts&, ltu&, lstr$, j&
  Dim sdt&(6), edt&(6), dsfrc&
  
  lDataHeader.id = dsn
  'get time step
  salen = 1
  saind = 33
  Call F90_WDBSGI(pFileUnit, dsn, saind, salen, lts, retcod)
  If (retcod <> 0) Then ' set time step to default of 1
    lts = 1
  End If
  lDateSum.ts = lts
  'get time units
  saind = 17
  Call F90_WDBSGI(pFileUnit, dsn, saind, salen, ltu, retcod)
  If (retcod <> 0) Then 'set to default of daily time units
    ltu = 4
  End If
  lDateSum.Tu = ltu
  If Not pQuick Then
    'get start and end dates for each data set
    j = 1
    Call F90_WTFNDT(pFileUnit, dsn, j, dsfrc, sdt(0), edt(0), retcod)
    If sdt(0) > 0 Then
      lDateSum.SJDay = Date2J(sdt)
      lDateSum.EJDay = Date2J(edt)
      Call timdif(sdt, edt, lDateSum.Tu, lDateSum.ts, lDateSum.NVALS)
    End If
  End If
  lDateSum.CIntvl = True
  If lDateSum.Tu = 4 Then 'day
    lDateSum.Intvl = lDateSum.ts
  ElseIf lDateSum.Tu = 3 Then 'hour
    lDateSum.Intvl = lDateSum.ts * 1 / CDbl(24)
  ElseIf lDateSum.Tu = 2 Then 'minute
    lDateSum.Intvl = lDateSum.ts * 1 / CDbl(1440)
  End If
  
  'get data-set scenario name
  saind = 288
  salen = 8
  Call F90_WDBSGC(pFileUnit, dsn, saind, salen, lstr)
  lstr = Trim(lstr)
  If Len(lstr) = 0 Then
    lstr = "<unk>"
  End If
  lDataHeader.Sen = lstr
  'get data-set location name
  saind = 290
  salen = 8
  Call F90_WDBSGC(pFileUnit, dsn, saind, salen, lstr)
  lstr = Trim(lstr)
  If Len(lstr) = 0 Then
    lstr = "<unk>"
  End If
  lDataHeader.loc = Trim(lstr)
  'get data-set constituent name
  saind = 289
  salen = 8
  Call F90_WDBSGC(pFileUnit, dsn, saind, salen, lstr)
  If Len(Trim(lstr)) = 0 Then 'try tstype for constituent
    saind = 1
    salen = 4
    Call F90_WDBSGC(pFileUnit, dsn, saind, salen, lstr)
  End If
  lDataHeader.con = Trim(lstr)
  'station name
  Call F90_WDBSGC(pFileUnit, dsn, CLng(45), CLng(48), lstr)
  lDataHeader.desc = Trim(lstr)
End Sub

Private Function AttrStored(saind&) As Boolean 'somewhere else
  Select Case saind
    Case 17: AttrStored = True 'tcode
    'Case 27: AttrStored = True 'tsbyr  'jlk commmented to fix winhspf problem
    Case 33: AttrStored = True 'tsstep
    Case 45: AttrStored = True 'staname
    Case 288: AttrStored = True 'idscen
    Case 289: AttrStored = True 'idlocn
    Case 290: AttrStored = True 'idcons
    Case Else: AttrStored = False
  End Select
End Function

Private Function AttrVal2String(saind&, saval&())
  Dim s$, i&
  Dim lAttr As clsAttributeWDM
  
  Set lAttr = gMsg.Attributes(saind)
  
  With lAttr
    If .DataType = ATCoTxt Then
      s = ""
      For i = 0 To (lAttr.ilen / 4) - 1
        s = s & Long2String(saval(i))
      Next i
    ElseIf .DataType = ATCoSng Then 'real
      s = CStr(Long2Single(saval(i)))
    Else
      s = CStr(saval(i))
    End If
  End With
  AttrVal2String = s
End Function

Private Function ATCclsTserFile_AddTimSer(t As ATCclsTserData, Optional ExistAction& = TsIdNoAction) As Boolean
  Dim i&, dsn&, lExAct&, retcod&, s$, TsInd&
  Dim BtnName() As Variant, nBtns&, AppendFg As Boolean, OvwrtFg As Boolean
  Dim myMsgBox As ATCoMessage, UsrExAct&
  Dim vData As Variant, lData As ATCclsTserData
  Dim bldOk As Boolean
  Dim lWdmOpen As Long
  Dim checklastval As Single
  
  Dim lMsg As String
  lMsg = "clsTSerWDM:AddTimSer:" & pFileUnit & ":" & pFileWholeName
  Call F90_MSG(lMsg, Len(lMsg))
  
  On Error Resume Next
  checklastval = t.Value(t.Dates.Summary.NVALS) 'make sure data has been read
  On Error GoTo ErrHandler
  
  lWdmOpen = F90_WDMOPN(pFileUnit, pFileWholeName, Len(pFileWholeName))
  
  ATCclsTserFile_AddTimSer = False 'assume we will fail
  lExAct = ExistAction 'use local copy of what to do if DSN exists
  AppendFg = False
  OvwrtFg = False
  
  dsn = t.Header.id
  i = 1
  For Each vData In pData
    Set lData = vData
    If dsn = lData.Header.id Then
      If lData.serial = t.serial Then   'exists already
        ATCclsTserFile_AddTimSer = DsnWriteData(t)
        Exit Function
      End If
      dsn = findNextDsn(dsn + 1) 'find next dsn
      If lExAct = TsIdNoAction Then  'just report the problem
        pErrorDescription = "WDM:AddTimSer:Id(DSN) " & t.Header.id & " Exists:Next Available is " & dsn
        Exit Function
      ElseIf lExAct > TsIdRenum Then 'ask user what to do
        nBtns = 0
        If lExAct And TsIdReplAsk Then 'overwrite is an option
          ReDim Preserve BtnName(nBtns)
          BtnName(nBtns) = "&Overwrite"
          nBtns = nBtns + 1
        End If
        If lExAct And TsIdAppendAsk Then 'append is an option
          ReDim Preserve BtnName(nBtns)
          BtnName(nBtns) = "&Append"
          nBtns = nBtns + 1
        End If
        If lExAct And TsIdRenumAsk Then 'renumber is an option
          ReDim Preserve BtnName(nBtns)
          BtnName(nBtns) = "&Renumber"
          nBtns = nBtns + 1
        End If
        'always have Cancel as last button (and default)
        ReDim Preserve BtnName(nBtns)
        BtnName(nBtns) = "+-&Cancel"
        Set myMsgBox = New ATCoCtl.ATCoMessage
        UsrExAct = myMsgBox.ShowArray("WDM data-set number " & t.Header.id & " already exists." & vbCrLf & "Next available data-set number is " & dsn & vbCrLf & "What should be done to it?", "WDM Data-set Number Problem", BtnName)
        If InStr(BtnName(UsrExAct - 1), "Overwrite") > 0 Then
          lExAct = TsIdRepl 'set to overwrite
        ElseIf InStr(BtnName(UsrExAct - 1), "Append") > 0 Then
          lExAct = TsIdAppend 'set to append
        ElseIf InStr(BtnName(UsrExAct - 1), "Renumber") > 0 Then
          lExAct = TsIdRenum 'set to renumber
        ElseIf InStr(BtnName(UsrExAct - 1), "Cancel") > 0 Then
          pErrorDescription = "WDM:AddTimSer:Id(DSN) " & t.Header.id & ".  User Cancelled on message box to resolve."
          Exit Function
        End If
      End If
      If lExAct = TsIdRepl Then 'overwrite the data set
        Call F90_WDDSDL(pFileUnit, t.Header.id, retcod)
        If retcod = 0 Then 'deleted ok
          'Set values in replaced time series
          lData.Dtran = t.Dtran
          lData.flags = t.flags
          lData.Max = t.Max
          lData.Min = t.Min
          lData.Values = t.Values
          Set lData.Dates = t.Dates
          Set lData.Header = t.Header
          Set lData.Attribs = t.Attribs
          
          'remove replaced timeseries from collections
          pDates.Remove (i)
          pData.Remove (i)
          OvwrtFg = True
        Else 'problem deleting
          If ExistAction > TsIdRenum Then 'report problem to user
            MsgBox "Could not overwrite data-set number " & t.Header.id & " on WDM file " & pFilename & ".", vbExclamation, "WDM Data Set Problem"
          End If
          pErrorDescription = "WDM:AddTimSer:Id(DSN) " & t.Header.id & " could not be deleted during data-set Overwrite."
          Exit Function
        End If
      ElseIf lExAct = TsIdAppend Then 'append to data set
        If t.Dates.Summary.SJDay >= lData.Dates.Summary.EJDay Then
          'start of new data follows end of existing, ok to append
          AppendFg = True
          TsInd = i
        Else 'can't append, new and existing data spans overlap
          If ExistAction > TsIdRenum Then 'report problem to user
            MsgBox "Time span of new data and existing data overlap." & vbCrLf & _
                   "Unable to append data to data-set number " & t.Header.id & " on WDM file " & pFilename & ".", vbExclamation, "WDM Data Set Problem"
          End If
          pErrorDescription = "WDM:AddTimSer:Id(DSN) " & t.Header.id & " could not have new data appended to it."
          Exit Function
        End If
      ElseIf lExAct = TsIdRenum Then 'renumber data set
        t.Header.id = dsn 'assign to next available data-set number
      End If
    End If
    i = i + 1
  Next vData

  If Not AppendFg Then
    pData.Add t 'add to internal collection
    pDates.Add t.Dates
    Set t.File = Me
  End If
  
  If pFileUnit > 0 Then 'save on wdm
    'i = F90_WDCKDT(pFileUnit, dsn)
    'If i = 0 Then 'dateset does not exist
    If Not AppendFg Then
      'create and add attributes
      bldOk = DsnBld(t)
    Else
      bldOk = True
    End If
    If bldOk Then
      'add data
      ATCclsTserFile_AddTimSer = DsnWriteData(t)
      If ATCclsTserFile_AddTimSer And AppendFg Then
        'update end date
        pDates.Remove (TsInd)
        pData.Remove (TsInd)
        RefreshDsn (t.Header.id)
      End If
    Else
      ATCclsTserFile_AddTimSer = bldOk
    End If
    
    If ExistAction > TsIdRenum Then 'report status
      If ATCclsTserFile_AddTimSer Then 'write succeeded
        If OvwrtFg Then
          s = "Successfully overwrote existing data-set number " & t.Header.id & " on WDM file " & pFilename & "."
        ElseIf AppendFg Then
          s = "Successfully appended data to existing data-set number " & t.Header.id & " on WDM file" & pFilename & "."
        Else 'new data set
          s = "New data-set number " & t.Header.id & " successfully stored on WDM file" & pFilename & "."
        End If
        MsgBox s, vbInformation + vbOKOnly, "WDM Data Set Add"
      Else 'problem
        If AppendFg Then
          s = "Problem appending data to data-set number " & t.Header.id & " on WDM file " & pFilename & "." & vbCr & ATCclsTserFile_ErrorDescription
        Else
          s = "Problem adding data-set number " & t.Header.id & " to WDM file " & pFilename & "." & vbCr & ATCclsTserFile_ErrorDescription
        End If
        MsgBox s, vbInformation + vbOKOnly, "WDM Data Set Problem"
      End If
    End If
  End If
  If lWdmOpen <> 1 Then i = F90_WDMCLO(pFileUnit)
  Exit Function
ErrHandler:
  MsgBox "Error adding timser" & vbCr & err.Description, vbCritical, ATCclsTserFile_Label
End Function

Private Function findNextDsn(dsn As Long) As Long
  Dim vData As Variant
  Dim retval As Long
  
  retval = dsn
  For Each vData In pData
    If retval = vData.Header.id Then
      retval = findNextDsn(dsn + 1)
      Exit For
    End If
  Next vData
  findNextDsn = retval
End Function

Private Function ATCclsTserFile_RemoveTimSer(t As ATCclsTserData) As Boolean
  Dim retc&
  Dim i&, searchSerial As Long, removeDate As Boolean
  Dim lWdmOpen As Long
  
  lWdmOpen = F90_WDMOPN(pFileUnit, pFileWholeName, Len(pFileWholeName))
  
  Call F90_WDDSDL(pFileUnit, (t.Header.id), retc)
  If retc = 0 Then
    ATCclsTserFile_RemoveTimSer = True
    searchSerial = t.serial
    For i = 1 To pData.Count
      If pData(i).serial = searchSerial Then pData.Remove i: Exit For
    Next
    
    removeDate = True
    searchSerial = t.Dates.serial
    For i = 1 To pData.Count
      If pData(i).Dates.serial = searchSerial Then removeDate = False: Exit For
    Next
  
    If removeDate Then
      For i = 1 To pDates.Count
        If pDates(i).serial = searchSerial Then pDates.Remove i: Exit For
      Next
    End If
  Else
    ATCclsTserFile_RemoveTimSer = False
    pErrorDescription = "WDM:RemoveTimSer:DSN" & t.Header.id & ":Retcod:" & retc
  End If
  
  If lWdmOpen <> 1 Then i = F90_WDMCLO(pFileUnit)
End Function

Private Function ATCclsTserFile_RewriteTimSer(t As ATCclsTserData) As Boolean
  ATCclsTserFile_RewriteTimSer = DsnWriteData(t)
End Function

Private Function ATCclsTserFile_SaveAs(aFileName As String) As Boolean
  Dim lRwFlg As Long, lRet As Long, lFileUnit As Long
  Dim lWdmOpen As Long
  Dim lMsg As String
  
  If Len(pFileWholeName) > 0 Then
    lWdmOpen = F90_WDMOPN(pFileUnit, pFileWholeName, Len(pFileWholeName))
  Else
    lWdmOpen = -1
  End If
  
  lMsg = "clsTSerWdm:SaveAs:" & lWdmOpen & ":" & aFileName
  Call F90_MSG(lMsg, Len(lMsg))
      
  lRwFlg = 2 'create new
  lFileUnit = F90_WDBOPN(lRwFlg, aFileName, Len(aFileName))
  
  lMsg = "clsTSerWdm:SaveAs:AfterCreate:" & lFileUnit & ":" & pFileUnit
  Call F90_MSG(lMsg, Len(lMsg))
  
  If lFileUnit > 0 Then 'create worked
    If pFileUnit > 0 Then 'copy existing data here
       'close current
       lRet = F90_WDFLCL(pFileUnit)
    End If
    'update
    pFilename = aFileName
    pFileWholeName = CurDir & "\" & FilenameNoPath(aFileName)
    pFileUnit = lFileUnit
    lMsg = "clsTSerWdm:SaveAs:RefreshStart"
    Call F90_MSG(lMsg, Len(lMsg))
    ATCclsTserFile_refresh
    lMsg = "clsTSerWdm:SaveAs:RefreshDone"
    Call F90_MSG(lMsg, Len(lMsg))
    If lWdmOpen <> 1 Then
      lRet = F90_WDMCLO(pFileUnit)
    End If
    lMsg = "clsTSerWdm:SaveAs:CloseDone:" & lRet
    Call F90_MSG(lMsg, Len(lMsg))
  End If
End Function

Private Function DsnBld(t As ATCclsTserData) As Boolean
'Private Sub DsnBld(dsn&, tsstep&, tcode&, Sen$, Loc$, Con$, Desc$, retcod&)
    'adapted from version in WDMUtil
    Dim ndn&, nup&, nsa&, nsasp&, ndp&, dsn&, i&
    Dim iVal&, psa&, ostr$, saind&, salen&, retcod&
    Dim CSDat&(6)

    Dim lMsg As String
    lMsg = "clsTSerWDM:DsnBld:" & pFileUnit
    Call F90_MSG(lMsg, Len(lMsg))
  
    dsn = t.Header.id
    'create label
    ndn = t.AttribNumeric("NDN", 10)
    nup = t.AttribNumeric("NUP", 10)
    nsa = t.AttribNumeric("NSA", 30)
    nsasp = t.AttribNumeric("NSASP", 100)
    ndp = t.AttribNumeric("NDP", 300)
    Call F90_WDLBAX(pFileUnit, dsn, 1, ndn, nup, nsa, nsasp, ndp, psa)
    'add attributes
    
    'add needed attributes
    saind = 1 'tstype
    salen = 4
    ostr = t.Attrib("TSTYPE", (Left(t.Header.con, 4)))
    Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, ostr, Len(ostr))
    salen = 1
    saind = 34 'tgroup
    iVal = t.AttribNumeric("TGROUP", 6)
    Call F90_WDBSAI(pFileUnit, dsn, gMsgUnit, saind, salen, iVal, retcod)
    saind = 83 'compfg
    iVal = t.AttribNumeric("COMPFG", 1)
    Call F90_WDBSAI(pFileUnit, dsn, gMsgUnit, saind, salen, iVal, retcod)
    saind = 84 'tsform
    iVal = t.AttribNumeric("TSFORM", 1)
    Call F90_WDBSAI(pFileUnit, dsn, gMsgUnit, saind, salen, iVal, retcod)
    saind = 17 'tcode
    iVal = t.Dates.Summary.Tu
    Call F90_WDBSAI(pFileUnit, dsn, gMsgUnit, saind, salen, iVal, retcod)
    saind = 33 'tsstep
    iVal = t.Dates.Summary.ts
    Call F90_WDBSAI(pFileUnit, dsn, gMsgUnit, saind, salen, iVal, retcod)
    saind = 85 'vbtime
    iVal = t.AttribNumeric("VBTIME", 1)
    If t.Dates.Summary.ts > 1 Then iVal = 2 'timestep > 1 vbtime must vary
    Call F90_WDBSAI(pFileUnit, dsn, gMsgUnit, saind, salen, iVal, retcod)
    Call J2Date(t.Dates.Summary.SJDay, CSDat)
    i = CSDat(0) Mod 10
    If i > 0 Then 'subtract back to start of this decade
      iVal = CSDat(0) - i
    Else 'back to start of previous decade
      iVal = CSDat(0) - 10
    End If
    saind = 27 'tsbyr
    Call F90_WDBSAI(pFileUnit, dsn, gMsgUnit, saind, salen, iVal, retcod)
    'salen = 48
    'saind = 45 'stanam  (how does this differ from Description below)
    'ostr = UCase(Left(t.Header.Desc, salen))
    'Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, ostr, Len(ostr))
    salen = 8
    saind = 288 'scenario
    ostr = UCase(Left(t.Header.Sen, salen))
    Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, ostr, Len(ostr))
    saind = 289 'constituent
    ostr = UCase(Left(t.Header.con, salen))
    Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, ostr, Len(ostr))
    saind = 290 'location
    ostr = UCase(Left(t.Header.loc, salen))
    Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, ostr, Len(ostr))
    salen = 48
    saind = 45 'description
    ostr = Left(t.Header.desc, 48)
    Call F90_WDBSAC(pFileUnit, dsn, gMsgUnit, saind, salen, retcod, ostr, Len(ostr))
    
    'others (from attrib)
    DsnBld = DsnWriteAttributes(t)
End Function

Private Function DsnWriteData(t As ATCclsTserData) As Boolean
  Dim v!(), retc&, sdat&(6), edat&(6), lDateSummary As ATTimSerDateSummary
  Dim i As Long, lWdmOpen As Long
  
  
    lWdmOpen = F90_WDMOPN(pFileUnit, pFileWholeName, Len(pFileWholeName))
    
    DsnWriteData = False 'assume the worst
    lDateSummary = t.Dates.Summary
    With lDateSummary
      If .CIntvl Then
        Call J2Date(.SJDay, sdat)
        If .NVALS = 0 Then
          'nothing to write
          pErrorDescription = "WDM:AddTimSer:Id(DSN) Problem - No data to write in " & t.Header.id
          '????? t.AttribSet "headercomplete", vbFalse
        Else
          ReDim v(.NVALS)
          v = t.Values
          Call F90_WDTPUT(pFileUnit, CLng(t.Header.id), _
                          .ts, sdat(0), .NVALS, CLng(1), CLng(0), .Tu, v(1), _
                          retc)
          If retc = 0 Then 'no problem
            DsnWriteData = True
          Else
            pErrorDescription = "WDM:AddTimSer:Id(DSN) Write in " & t.Header.id & " Problem " & retc
          End If
        End If
      Else
        pErrorDescription = "WDM:AddTimSer:Id(DSN) " & t.Header.id & " data is not constant interval"
      End If
    End With
    
    'does this need to be here?
    If DsnWriteData Then
      DsnWriteData = DsnWriteAttributes(t)
    End If
    
    If lWdmOpen <> 1 Then i = F90_WDMCLO(pFileUnit)

End Function

Private Function DsnWriteAttributes(t As ATCclsTserData) As Boolean

  Dim vAttr As Variant, lAttr As ATTimSerAttribute
  Dim lWAttr As clsAttributeWDM
  Dim retc&
  
  DsnWriteAttributes = True
  For Each vAttr In t.Attribs
    lAttr = vAttr
    If lAttr.Name = "Units" Then  'store Units ID as DCODE in WDM
      lAttr.Name = "DCODE"
      lAttr.Value = GetUnitID(lAttr.Value)
    End If
    Set lWAttr = gMsg.Attrib(lAttr.Name)
    If Not (lWAttr Is Nothing) Then
      With lWAttr
        Select Case .DataType
          Case ATCoInt 'integer
            If IsNumeric(lAttr.Value) Then
              F90_WDBSAI pFileUnit, CLng(t.Header.id), gMsgUnit, .Ind, .ilen, CLng(lAttr.Value), retc
            End If
          Case ATCoSng 'long
            If IsNumeric(lAttr.Value) Then
              F90_WDBSAR pFileUnit, CLng(t.Header.id), gMsgUnit, .Ind, .ilen, CSng(lAttr.Value), retc
            End If
          Case Else 'character
            F90_WDBSAC pFileUnit, CLng(t.Header.id), gMsgUnit, .Ind, .ilen, retc, lAttr.Value, Len(lAttr.Value)
        End Select
      End With
      If retc <> 0 Then
        If Abs(retc) = 104 Then 'cant update if data already present
          'Debug.Print "Skip:" & lAttr.Name & ", data present"
        Else
          If Len(pErrorDescription) = 0 Then
            pErrorDescription = "Unable to Write Data Attributes for Class WDM"
          End If
          pErrorDescription = pErrorDescription & vbCrLf & _
                              "  Attribute:" & lAttr.Name & _
                              ", Value:" & lAttr.Value & _
                              ", Retcod:" & retc
          DsnWriteAttributes = False
        End If
      End If
    End If
  Next vAttr
  If Len(pErrorDescription) > 0 Then
    Debug.Print pErrorDescription
  End If
End Function

Public Function ReadBasInf() As Boolean
  Dim bFun&, bNam$
  Dim i&, j&, ip&, istr$, NLoc&
  
  ReadBasInf = False
  
  bNam = PathNameOnly(pFilename) & "\" & FilenameOnly(pFilename) & ".inf"
  If Len(Dir(bNam)) > 0 Then
    'read available BASINS .inf file
    bFun = FreeFile(0)
    Open bNam For Input As #bFun
    'reads location records from BASINS .inf file
    On Error GoTo ErrHandler
    Do While InStr(istr, "number of stations") <= 0 And Not EOF(bFun)
      Line Input #bFun, istr
    Loop
    NLoc = CLng(Mid(istr, 1, 5))
    If NLoc > 0 Then 'valid number of locations read
      ReDim BasInf(NLoc - 1)
      Line Input #bFun, istr
      Line Input #bFun, istr
      For i = 0 To NLoc - 1
        Line Input #bFun, istr
        ip = InStr(2, istr, String(1, 34))
        If ip > 0 Then 'read description from between quotes
          BasInf(i).desc = Mid(istr, 2, ip - 2)
        End If
        'BasInf(i).Nam = Mid(istr, 30, 2) & Mid(istr, 36, 4)
        BasInf(i).Nam = Trim(Mid(istr, 30, 10))
        BasInf(i).Elev = CSng(Mid(istr, 42, 10))
        BasInf(i).sdat(0) = Mid(istr, 54, 4)
        ip = 59
        For j = 1 To 3
          BasInf(i).sdat(j) = Mid(istr, ip, 2)
          ip = ip + 3
        Next j
        BasInf(i).edat(0) = Mid(istr, 69, 4)
        ip = 74
        For j = 1 To 3
          BasInf(i).edat(j) = Mid(istr, ip, 2)
          ip = ip + 3
        Next j
        BasInf(i).EvapCoef = CSng(Mid(istr, 86, 8))
        istr = LTrim(Mid(istr, 94))
        ip = 1
        j = 0
        Do While j <= 7 And ip > 0
          ip = InStr(istr, " ")
          If ip > 0 Then 'process next data-set number
            BasInf(i).dsn(j) = CLng(Mid(istr, 1, ip - 1))
            istr = LTrim(Mid(istr, ip))
          Else 'must be at end of string
            BasInf(i).dsn(j) = CLng(Mid(istr, 1))
          End If
          j = j + 1
        Loop
      Next i
    End If
    'read dataset variable names and descriptions
    ReDim DsnDescs(0)
    nDD = 0
    Do While Not EOF(bFun)
      ReDim Preserve DsnDescs(nDD)
      Line Input #bFun, DsnDescs(nDD)
      nDD = nDD + 1
    Loop
    ReadBasInf = True
    Close bFun
  End If
  Exit Function

ErrHandler:
    MsgBox "Problem reading BASINS information file (.inf) associated with this WDM file." & vbCrLf & "Modifications made to this WDM file will not be reflected in the BASINS NPSM interface.", vbInformation, "WDMUtil File Open"

End Function

Private Sub HeaderFromBasinsInf(lData As ATCclsTserData)
  Dim ist&, lstr$, j&, i&
  
  On Error GoTo 0
  With lData.Header
    .Sen = "OBSERVED"
    .con = lData.Attrib("TSTYPE")
    i = .id - 10
    ist = ((i - (i Mod 20)) / 20)
    If ist >= LBound(BasInf) And ist <= UBound(BasInf) Then
      lstr = BasInf(ist).Nam
      If Len(lstr) > 8 Then 'compress name to fit in 8 characters
        'try to weed out 00s from station ID
        j = InStr(lstr, "00")
        Do While j > 0 And Len(lstr) > 8
          If j > 1 Then
            lstr = Left(lstr, j - 1) & Mid(lstr, j + 2)
          Else
            lstr = Mid(lstr, j + 2)
          End If
        Loop
        If Len(lstr) > 8 Then 'couldn't weed out 00s to compress name
          lstr = Left(lstr, 8)
        End If
      End If
      .loc = lstr
      .desc = BasInf(ist).desc
    End If
  End With
  
  'save revisions to header back to dsn
  ATCclsTserFile_writeDataHeader lData
  
End Sub
