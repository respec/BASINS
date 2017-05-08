VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfUci"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pMsgWDMName As String
Dim pMsgUnit As Long
Dim pMsg As HspfMsg
Dim pWdmUnit(4) As Long
Dim pWDMObj(4) As ATCclsTserFile
Dim pWdmCount As Long
Dim pName As String
Dim pGlobalBlk As HspfGlobalBlk
Dim pFilesBlk As HspfFilesBlk
Dim pOpnSeqBlk As HspfOpnSeqBlk
Dim pOpnBlks As Collection 'of HspfOpnBlk
Dim pConnections As Collection 'of HspfConnection
Dim pMassLinks As Collection 'of HspfMasslink
Dim pMetSegs As Collection 'of HspfMetSeg
Dim pPointSources As Collection 'of HspfPoint
Dim pPollutants As Collection 'of HspfPollutant
Dim pMonthData As HspfMonthData
Dim pErrorDescription As String
Dim pEdited As Boolean
Dim pInitialized As Boolean
Dim pSpecialActionBlk As HspfSpecialActionBlk
Dim pCategoryBlk As HspfCategoryBlk
'Dim pStatusIn As Long
'Dim pStatusOut As Long
Dim pMaxAreaByLand2Stream As Double
'Dim pHspfEngine As Object
'Dim pHspfEngineSet As Boolean
Dim pHelpFileName As String
Dim pStarterPath As String
Dim pFastFlag As Boolean
Dim pAcidphFlag As Boolean

Dim cOrder As Collection 'for saving order of blocks

Private pIcon As StdPicture

Public Property Get Monitor() As Object
  Set Monitor = IPC
End Property
Public Property Set Monitor(ByVal O As Object)
  Set IPC = O
  If IPC Is Nothing Then IPCset = False Else IPCset = True
  
  'With pHspfEngine.launch
    '.SendComputeMessage "SPIPH " & .ComputeReadFromParent & " " & .ComputeWriteToParent
    '.SendComputeMessage "W99OPN"
    'Call F90_W99OPN  'open error file for fortan problems
    'Call F90_WDBFIN  'initialize WDM record buffer
    '.SendComputeMessage "PUTOLV 10"
    'Call F90_PUTOLV(10)
    'Call F90_SCNDBG(-1)
  'End With
End Property

Public Property Let HelpFile(newName As String)
  App.HelpFile = newName
End Property

Public Property Get Edited() As Boolean
  Edited = pEdited
End Property

Public Property Let Edited(newEdited As Boolean)
  pEdited = newEdited
End Property

Public Property Get FastFlag() As Boolean
  FastFlag = pFastFlag
End Property

Public Property Let FastFlag(newFastFlag As Boolean)
  pFastFlag = newFastFlag
End Property

Public Property Get AcidphFlag() As Boolean
  AcidphFlag = pAcidphFlag
End Property

Public Property Let AcidphFlag(newAcidphFlag As Boolean)
  pAcidphFlag = newAcidphFlag
End Property

Private Property Get Initialized() As Boolean
  Initialized = pInitialized
  If Not (pInitialized) Then
    pErrorDescription = "UCI File not Initialized"
  End If
End Property
Public Property Let Initialized(newInitialized As Boolean)
  pInitialized = newInitialized
End Property

Public Property Get Msg() As HspfMsg
  Set Msg = pMsg
End Property
Public Property Set Msg(newMsg As HspfMsg)
  Set pMsg = newMsg
End Property

Public Property Let StatusIn(newStatusIn As Long)
  'pStatusIn = newStatusIn
End Property

Public Property Let StatusOut(newStatusOut As Long)
  'pStatusOut = newStatusOut
End Property

Public Property Get icon() As StdPicture
  Set icon = pIcon
End Property
Public Property Set icon(newIcon As StdPicture)
  Set pIcon = newIcon
  Set myMsgBox.icon = newIcon
End Property

Public Property Get ErrorDescription() As String
  ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

Public Property Let ErrorDescription(newED As String)
  pErrorDescription = newED
End Property

Public Property Get GlobalBlock() As HspfGlobalBlk
  If pInitialized Then Set GlobalBlock = pGlobalBlk
End Property
Public Property Let GlobalBlock(newGlobalBlock As HspfGlobalBlk)
    Set pGlobalBlk = newGlobalBlock
End Property

Public Property Get FilesBlock() As HspfFilesBlk
  If Initialized Then Set FilesBlock = pFilesBlk
End Property
Public Property Let FilesBlock(newFilesBlock As HspfFilesBlk)
  Set pFilesBlk = newFilesBlock
End Property

Public Property Get CategoryBlock() As HspfCategoryBlk
  If Initialized Then Set CategoryBlock = pCategoryBlk
End Property
Public Property Let CategoryBlock(newCategoryBlock As HspfCategoryBlk)
  Set pCategoryBlk = newCategoryBlock
End Property

Public Property Get MonthData() As HspfMonthData
  Set MonthData = pMonthData
End Property

Public Property Get OpnSeqBlock() As HspfOpnSeqBlk
  If Initialized Then Set OpnSeqBlock = pOpnSeqBlk
End Property
Public Property Let OpnSeqBlock(newOpnSeqBlock As HspfOpnSeqBlk)
  Set pOpnSeqBlk = newOpnSeqBlock
End Property

Public Property Get Connections() As Collection 'of HspfConnection
  Set Connections = pConnections
End Property
Public Property Let Connections(newConnections As Collection) 'of HspfConnection
  Dim lConnection As HspfConnection, vConn As Variant
  For Each vConn In newConnections
    Set lConnection = vConn
    pConnections.Add lConnection
  Next vConn
End Property

Public Property Get MetSegs() As Collection 'of HSPFMetSeg
  Set MetSegs = pMetSegs
End Property

Public Property Get PointSources() As Collection 'of HSPFPoint
  Set PointSources = pPointSources
End Property

Public Property Get Pollutants() As Collection 'of HSPFPollutant
  Set Pollutants = pPollutants
End Property

Public Property Get MassLinks() As Collection 'of HspfMassLinks
  Set MassLinks = pMassLinks
End Property
Public Property Let MassLinks(newMassLinks As Collection) 'of HspfMassLinks
  Dim lMassLink As HspfMassLink, vMassLink As Variant
  For Each vMassLink In newMassLinks
    Set lMassLink = vMassLink
    pMassLinks.Add lMassLink
  Next vMassLink
End Property

Public Property Get OpnBlks() As Collection 'of HspfOpnBlk
  Set OpnBlks = pOpnBlks
End Property
Public Property Let OpnBlks(newOpnBlks As Collection) 'of HspfOpnBlk
  Dim lopnblk As HspfOpnBlk, vOpnBlk As Variant
  For Each vOpnBlk In newOpnBlks
    Set lopnblk = vOpnBlk
    pOpnBlks.Add lopnblk, lopnblk.Name
  Next vOpnBlk
End Property

Public Property Get Name() As String
Attribute Name.VB_MemberFlags = "200"
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property

Public Property Let MsgWDMName(newName As String)
  pMsgWDMName = newName
End Property

Public Property Get SpecialActionBlk() As HspfSpecialActionBlk
  Set SpecialActionBlk = pSpecialActionBlk
End Property

Public Property Get WDMCount() As Long
  WDMCount = pWdmCount
End Property

Public Sub Save()
  Dim f%, vOpnBlk As Variant, lopnblk As HspfOpnBlk
  Dim i&
  
  f = FreeFile(0)
  Open pName For Output As #f
  
  Print #f, "RUN"
  
  For i = 1 To cOrder.Count
    If cOrder(i) = "GLOBAL" Then
      pGlobalBlk.WriteUciFile (f)
    ElseIf cOrder(i) = "FILES" Then
      pFilesBlk.WriteUciFile (f)
    ElseIf cOrder(i) = "CATEGORY" Then
      If Not pCategoryBlk Is Nothing Then
        If pCategoryBlk.Count > 0 Then
          pCategoryBlk.WriteUciFile (f)
        End If
      End If
    ElseIf cOrder(i) = "OPN SEQUENCE" Then
      pOpnSeqBlk.WriteUciFile (f)
    ElseIf cOrder(i) = "MONTH DATA" Then
      If Not pMonthData Is Nothing Then
        pMonthData.WriteUciFile (f)
      End If
    ElseIf cOrder(i) = "PERLND" Then
      Set lopnblk = OpnBlks("PERLND")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "IMPLND" Then
      Set lopnblk = OpnBlks("IMPLND")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "RCHRES" Then
      Set lopnblk = OpnBlks("RCHRES")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "FTABLES" Then
      Set lopnblk = OpnBlks("RCHRES")
      If lopnblk.Count > 0 Then
        lopnblk.Ids(1).FTable.WriteUciFile (f)
      End If
    ElseIf cOrder(i) = "COPY" Then
      Set lopnblk = OpnBlks("COPY")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "PLTGEN" Then
      Set lopnblk = OpnBlks("PLTGEN")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "DISPLY" Then
      Set lopnblk = OpnBlks("DISPLY")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "DURANL" Then
      Set lopnblk = OpnBlks("DURANL")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "GENER" Then
      Set lopnblk = OpnBlks("GENER")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "MUTSIN" Then
      Set lopnblk = OpnBlks("MUTSIN")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "BMPRAC" Then
      Set lopnblk = OpnBlks("BMPRAC")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "REPORT" Then
      Set lopnblk = OpnBlks("REPORT")
      If lopnblk.Count > 0 Then
        Call lopnblk.WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "CONNECTIONS" Then
      If pConnections.Count > 0 Then
        Call pConnections(1).WriteUciFile(f, pMsg)
      End If
    ElseIf cOrder(i) = "MASSLINKS" Then
      If pMassLinks.Count > 0 Then
        Call pMassLinks(1).writeMassLinks(f, pMsg)
      End If
    ElseIf cOrder(i) = "SPECIAL ACTIONS" Then
      If Not pSpecialActionBlk Is Nothing Then
        pSpecialActionBlk.WriteUciFile (f)
      End If
    End If
  Next i
  
  Print #f, " "
  Print #f, "END RUN"
  Close #f
  pEdited = False
End Sub

Public Sub SaveAs(oldname$, newName$, basedsn&, relabs&)
  If oldname <> newName Then
    Call pFilesBlk.newName(oldname, newName)
    Call newOutputDsns(oldname, newName, basedsn, relabs)
  End If
  Call Save
End Sub

Private Sub Class_Initialize()
  Dim i&
  
  pName = ""
  pErrorDescription = ""
  pEdited = False
  pFastFlag = False
  pInitialized = False
  pMaxAreaByLand2Stream = 0
  Set myMsgBox = New ATCoMessage
  
  'init others as appropriate
    
  Set pMsg = Nothing
  Set pConnections = New Collection
  Set pOpnBlks = New Collection
  Set pMetSegs = New Collection
  Set pPointSources = New Collection
  Set pMassLinks = New Collection
  Set pPollutants = New Collection
  
  Set cOrder = New Collection
  cOrder.Add "GLOBAL"
  cOrder.Add "FILES"
  cOrder.Add "OPN SEQUENCE"
  cOrder.Add "MONTH DATA"
  cOrder.Add "CATEGORY"
  cOrder.Add "PERLND"
  cOrder.Add "IMPLND"
  cOrder.Add "RCHRES"
  cOrder.Add "FTABLES"
  cOrder.Add "COPY"
  cOrder.Add "PLTGEN"
  cOrder.Add "DISPLY"
  cOrder.Add "DURANL"
  cOrder.Add "GENER"
  cOrder.Add "MUTSIN"
  cOrder.Add "BMPRAC"
  cOrder.Add "REPORT"
  cOrder.Add "CONNECTIONS"
  cOrder.Add "MASSLINKS"
  cOrder.Add "SPECIAL ACTIONS"
    
  InitAtCoTser
End Sub

Public Property Let MessageUnit(newUnit As Long)
  If newUnit = 0 Then
    F90_MSGUNIT pMsgUnit
  Else 'could check to be sure?
    pMsgUnit = newUnit
  End If
End Property
Public Property Get MessageUnit() As Long
  MessageUnit = pMsgUnit
End Property

Public Sub CreateUci(M As HspfMsg, newName As String, outputwdm$, _
                     metwdms$(), wdmids$(), MetDataDetails$, _
                     oneseg As Boolean, PollutantList As Collection)
                     
  'Call F90_SPIPH(pStatusIn, pStatusOut)
  Call CreateUciFromBASINS(Me, M, newName, outputwdm, metwdms, wdmids, _
                           MetDataDetails, oneseg, PollutantList)
End Sub

Public Sub FastReadUciForStarter(Msg As HspfMsg, newName As String)
  Dim FilesOK As Boolean
  Dim FullFg As Long, EchoFile As String
  
  pFastFlag = True
  FullFg = -1
  ReadUci Msg, newName, FullFg, FilesOK, EchoFile
  pFastFlag = False
End Sub

Public Sub FastReadUci(Msg As HspfMsg, newName As String)
  'called by scripthspf, processes wdm files
  Dim FilesOK As Boolean
  Dim FullFg As Long, EchoFile As String
  
  pFastFlag = True
  FullFg = -3
  ReadUci Msg, newName, FullFg, FilesOK, EchoFile
  pFastFlag = False
End Sub

Public Sub ReadUci(Msg As HspfMsg, newName As String, FullFg&, FilesOK As Boolean, EchoFile$)
  Dim i&, n$, r&, s$, j&, iwdm&, M$, m2$
  Dim lOpn As HspfOperation, vOpn As Variant
  Dim lopnblk As HspfOpnBlk, vOpnBlk As Variant
  Dim lOpnName$, ret&
  Dim lConnection As HspfConnection
  Dim lMassLink As HspfMassLink
  
  On Error Resume Next
  
  Set pMsg = Msg
  FilesOK = True
  If Len(Dir(newName)) = 0 Then
    pErrorDescription = "UciFileName '" & newName & "' not found"
  Else
    pName = newName
    If FullFg <> -1 Then 'not doing starter, process wdm files
      PreScanFilesBlock pName, FilesOK, EchoFile
      EchoFile = Trim(EchoFile)
    End If
    If FilesOK Then
      s = FilenameOnly(pName)
      's = Left(s, Len(s) - 4)
      'Call F90_SPIPH(pStatusIn, pStatusOut)
      'set debug level
      'Call F90_SCNDBG(0)
      If FullFg = -3 Then
        i = FullFg
      Else
        i = -2 'flag as coming from hspf class for status title
      End If
      
      If pFastFlag Then
        'do fast read of uci, no run interpreter
        ReadUCIRecords pName
      Else
        'do normal activate of uci, including running the run interpreter
        IPC.SendProcessMessage "HSPFUCI", "CURDIR " & CurDir
        IPC.SendProcessMessage "HSPFUCI", "ACTIVATE " & s & " " & i
        'IPC.SendProcessMessage "HSPFUCI", "ACTIVATE " & s & " " & -1   'should not run interpret
        'Call F90_ACTSCN(i, pWdmUnit(1), pMsgUnit, r, s, Len(s))
        M = WaitForChildMessage
        If Left(M, 6) = "CURDIR" Then M = WaitForChildMessage
        If Right(M, 1) <> 0 Or Left(M, 24) = "HSPFUCI exited with code" Then
          'would be helpful to include m here
          pErrorDescription = "Error interpreting UCI File '" & s & "'." & vbCrLf & vbCrLf & _
              "See the file '" & Trim(EchoFile) & "' for more details." '& vbCrLf & vbCrLf & M
          IPC.SendMonitorMessage pErrorDescription
        End If
        'also do fast read -- this would be a very significant performance improvement
        ReadUCIRecords pName
        pFastFlag = True
      End If
      
      pInitialized = True
    
      IPC.SendMonitorMessage "(Show)" 'where was the hide?
      If Not pFastFlag Then
        IPC.SendMonitorMessage "(Msg1 Building Collections)"
      End If
      
      If pFastFlag Then
        SaveBlockOrder cOrder
      End If
      
      Set pGlobalBlk = New HspfGlobalBlk
      Set pGlobalBlk.Uci = Me
      pGlobalBlk.ReadUciFile

      Set pFilesBlk = New HspfFilesBlk
      Set pFilesBlk.Uci = Me
      pFilesBlk.ReadUciFile
      
      Set pCategoryBlk = New HspfCategoryBlk
      Set pCategoryBlk.Uci = Me
      pCategoryBlk.ReadUciFile
    
      Set pMonthData = New HspfMonthData
      Set pMonthData.Uci = Me
      pMonthData.ReadUciFile
    
      Set pOpnSeqBlk = New HspfOpnSeqBlk
      Set pOpnSeqBlk.Uci = Me
      pOpnSeqBlk.ReadUciFile
    
      Set pOpnBlks = Nothing
      Set pOpnBlks = New Collection
      i = 1
      lOpnName = HspfOperName(i)
      While lOpnName <> "UNKNOWN"
        Set lopnblk = New HspfOpnBlk
        lopnblk.Name = lOpnName
        Set lopnblk.Uci = Me
        pOpnBlks.Add lopnblk, lOpnName
        i = i + 1
        lOpnName = HspfOperName(i)
      Wend
      For Each vOpn In pOpnSeqBlk.Opns
        Set lOpn = vOpn
        Set lopnblk = pOpnBlks(lOpn.Name)
        lopnblk.Ids.Add lOpn, "K" & lOpn.Id
        Set lOpn.OpnBlk = lopnblk
      Next vOpn

      For Each vOpnBlk In pOpnBlks 'perlnd, implnd, etc
        Set lopnblk = vOpnBlk
        If lopnblk.Count > 0 Then
          If Not pFastFlag Then
            IPC.SendMonitorMessage "(MSG2 Setting table values for " & lopnblk.Name & ")"
          End If
          lopnblk.setTableValues Msg.BlockDefs(lopnblk.Name)
        End If
      Next vOpnBlk
      
      Set pSpecialActionBlk = New HspfSpecialActionBlk
      Set pSpecialActionBlk.Uci = Me
      pSpecialActionBlk.ReadUciFile
      
      If Not pFastFlag Then
        IPC.SendMonitorMessage "(MSG2 Processing Ftables)"
      End If
      ProcessFTables

      If Not pFastFlag Then
        IPC.SendMonitorMessage "(MSG2 Processing Connections)"
      End If
      Set pConnections = Nothing
      Set pConnections = New Collection
      Set lConnection = New HspfConnection 'dummy to get entry point
      lConnection.readTimSer Me
      Set lConnection = Nothing
      For Each vOpn In pOpnSeqBlk.Opns
        Set lOpn = vOpn
        lOpn.setTimSerConnections
      Next vOpn
      
      Set pMassLinks = Nothing
      Set pMassLinks = New Collection
      Set lMassLink = New HspfMassLink
      lMassLink.readMassLinks Me
      
      'look for met segments
      If Not pFastFlag Then
        IPC.SendMonitorMessage "(MSG2 Processing Met Segments)"
      End If
      Source2MetSeg
      'look for point loads
      If Not pFastFlag Then
        IPC.SendMonitorMessage "(MSG2 Processing Point Sources)"
      End If
      Source2Point
      IPC.SendMonitorMessage "(Hide)"
      
    End If
  End If
  pEdited = False 'all the reads set edited
  Exit Sub
End Sub

Public Property Get MaxAreaByLand2Stream() As Double
  If pMaxAreaByLand2Stream = 0 Then
    CalcMaxAreaByLand2Stream
  End If
  MaxAreaByLand2Stream = pMaxAreaByLand2Stream
End Property

Public Property Let MaxAreaByLand2Stream(newval As Double)
  pMaxAreaByLand2Stream = newval
End Property

Public Sub CalcMaxAreaByLand2Stream()
  Dim vOpTyps As Variant, vOpTyp As Variant
  Dim vOpn As Variant, lOpn As HspfOperation
  Dim iConn As HspfConnection, jConn As HspfConnection
  Dim M#, x#, i&, j&
  
  M = 0
  If pInitialized Then
    vOpTyps = Array("RCHRES", "BMPRAC") 'operations with contrib landuse area
    For Each vOpTyp In vOpTyps
      For Each vOpn In pOpnBlks(vOpTyp).Ids 'each operation
        Set lOpn = vOpn
        For i = 1 To lOpn.Sources.Count
          Set iConn = lOpn.Sources(i)
          x = 0
          If iConn.Source.VolName = "PERLND" Or iConn.Source.VolName = "IMPLND" Then
            x = iConn.MFact
            For j = i + 1 To lOpn.Sources.Count
              Set jConn = lOpn.Sources(j)
              If jConn.Source.VolName = "PERLND" Or jConn.Source.VolName = "IMPLND" Or jConn.Source.VolName = "BMPRAC" Then
                If Not jConn.Source.Opn Is Nothing And Not iConn.Source.Opn Is Nothing Then
                  If jConn.Source.Opn.Description = iConn.Source.Opn.Description Then 'more
                    x = x + jConn.MFact
                  End If
                End If
              End If
            Next j
          End If
          If x > M Then M = x
        Next i
      Next vOpn
    Next vOpTyp
  End If
  pMaxAreaByLand2Stream = M
End Sub

Public Sub Source2MetSeg()
  Dim vOpn As Variant, lOpn As HspfOperation
  Dim vOpTyps As Variant, vOpTyp As Variant
  Dim lConn As HspfConnection, j&, Comment$
  Dim tMetSeg As HspfMetSeg, vMetSeg As Variant
  Dim newSeg As Boolean, wdmid$, idsnid&
  Dim idsn&, scen$, loc$, cons$, itu&, its&, isdat&(6), iedat&(6), igsiz&
 
  vOpTyps = Array("PERLND", "IMPLND", "RCHRES") 'operations with assoc met segs
  
  For Each vOpTyp In vOpTyps
    For Each vOpn In pOpnBlks(vOpTyp).Ids
      Set tMetSeg = New HspfMetSeg  'init moved here
      Set tMetSeg.Uci = Me
      Set lOpn = vOpn
      Comment = ""
      j = 1
      Do While j <= lOpn.Sources.Count
        Set lConn = lOpn.Sources(j)
        If lConn.Typ = 1 Then
          If tMetSeg.Add(lConn) Then
            lOpn.Sources.Remove j
            If Len(Comment) = 0 Then
              Comment = lConn.Comment
            End If
          Else
            j = j + 1
          End If
        Else
          j = j + 1
        End If
      Loop
      
      'check to see if we already have this met segment
      newSeg = True
      If pMetSegs.Count > 0 Then
        For Each vMetSeg In pMetSegs
          If vMetSeg.Compare(tMetSeg, lOpn.Name) Then
            newSeg = False
            If lOpn.Name = "RCHRES" Then
              'may need to update met seg
              vMetSeg.UpdateMetSeg tMetSeg
            End If
            Set lOpn.MetSeg = vMetSeg
            Exit For
          End If
        Next vMetSeg
      End If
      
      If newSeg Then
        tMetSeg.Id = pMetSegs.Count + 1
        'get met seg name from precip data set
        idsn = tMetSeg.MetSegRec(1).Source.VolId
        If idsn > 0 Then
          wdmid = tMetSeg.MetSegRec(1).Source.VolName
          If pWdmCount > 0 Then
            tMetSeg.ExpandMetSegName wdmid, idsn
          Else
            If Len(Comment) > 13 Then
              tMetSeg.Name = Mid(Comment, 13)
            Else
              tMetSeg.Name = Comment
            End If
          End If
          pMetSegs.Add tMetSeg
          Set lOpn.MetSeg = tMetSeg
        Else 'need in case there is no prec in the met seg
          tMetSeg.Name = ""
          pMetSegs.Add tMetSeg
          Set lOpn.MetSeg = tMetSeg
        End If
        Set tMetSeg = New HspfMetSeg
        Set tMetSeg.Uci = Me
      End If
    Next vOpn
  Next vOpTyp
  Set tMetSeg = Nothing 'get rid of last one
  
  'set any undefined mfacts to 0
  If pMetSegs.Count > 0 Then
    For Each vMetSeg In pMetSegs
      For j = 1 To 7
        If vMetSeg.MetSegRec(j).MFactP = -999# Then
          vMetSeg.MetSegRec(j).MFactP = 0
        End If
        If vMetSeg.MetSegRec(j).MFactR = -999# Then
          vMetSeg.MetSegRec(j).MFactR = 0
        End If
      Next j
    Next vMetSeg
  End If
  
End Sub

Public Sub Source2Point()
  Dim vOpn As Variant, lOpn As HspfOperation
  Dim lConn As HspfConnection, j&
  Dim tPoint As HspfPoint
  Dim idPoint As HspfPoint
  Dim idsn&, scen$, loc$, cons$, itu&, its&, isdat&(6), iedat&(6), igsiz&
  Dim lastid&, idsnid&, wdmid$
  Dim vOpTyps As Variant, vOpTyp As Variant
  Dim newpoint As Boolean, i&, RFact!
 
  vOpTyps = Array("RCHRES", "COPY") 'operations with assoc pt srcs
  For Each vOpTyp In vOpTyps
 
    For Each vOpn In pOpnBlks(vOpTyp).Ids
      Set lOpn = vOpn
      j = 1
      Do While j <= lOpn.Sources.Count
        Set lConn = lOpn.Sources(j)
        
        If (lConn.Target.VolName = vOpTyp And _
           lConn.Target.Group <> "EXTNL") And _
          (Left(lConn.Source.VolName, 3) = "WDM") Then
          'if wdm data set to rchres add to collection,
          'or if wdm data set to copy and copy goes to rchres
          newpoint = False
          If lConn.Target.VolName = "COPY" Then
            RFact = 0
            For i = 1 To lConn.Target.Opn.Targets.Count
              If lConn.Target.Opn.Targets(i).Target.VolName = "RCHRES" Then
                newpoint = True
                'sum up the mfacts (really for septic modeling)
                RFact = RFact + lConn.Target.Opn.Targets(i).MFact
              End If
            Next i
          ElseIf lConn.Target.VolName = "RCHRES" Then
            newpoint = True
          End If
          If newpoint Then
            If Trim(lConn.Source.VolName) = "WDM" Then
              lConn.Source.VolName = "WDM1"
            End If
            Set tPoint = New HspfPoint
            tPoint.MFact = lConn.MFact
            If lConn.Target.VolName = "COPY" Then
              'save rfact for septics
              tPoint.RFact = RFact
            End If
            Set tPoint.Source = lConn.Source
            tPoint.Tran = lConn.Tran
            tPoint.Sgapstrg = lConn.Sgapstrg
            tPoint.Ssystem = lConn.Ssystem
            Set tPoint.Target = lConn.Target
            'pbd -- store associated operation id for use when writing
            tPoint.AssocOper = lOpn.Id
            'get point source name from any data set
            If Left(tPoint.Source.VolName, 3) = "WDM" Then
              idsn = tPoint.Source.VolId
              If idsn > 0 Then
                wdmid = tPoint.Source.VolName
                If pWdmCount > 0 Then
                  tPoint.Name = GetWDMAttr(wdmid, idsn, "DESC")
                  tPoint.Con = GetWDMAttr(wdmid, idsn, "CON")
                End If
              End If
            Else
              tPoint.Name = tPoint.Source.VolName & " " & tPoint.Source.VolId
              tPoint.Con = ""
            End If
            For Each idPoint In pPointSources
              If idPoint.Name = tPoint.Name Then
                tPoint.Id = idPoint.Id
                Exit For
              End If
            Next
            If tPoint.Id = 0 Then
              lastid = lastid + 1
              tPoint.Id = lastid
            End If
            pPointSources.Add tPoint
            lOpn.PointSources.Add tPoint
            lOpn.Sources.Remove j
          Else
            j = j + 1
          End If
        Else
          j = j + 1
        End If
      Loop
    Next vOpn
  Next vOpTyp

End Sub

Public Sub Point2Source()
  Dim vOpn As Variant, lOpn As HspfOperation
  Dim lConn As HspfConnection, j&, i&
  Dim tPoint As HspfPoint
  Dim idsn&, scen$, loc$, cons$, itu&, its&, isdat&(6), iedat&(6), igsiz&
  Dim vOpTyps As Variant, vOpTyp As Variant
 
  vOpTyps = Array("RCHRES", "COPY") 'operations with assoc pt srcs
  For Each vOpTyp In vOpTyps
 
    For Each vOpn In pOpnBlks(vOpTyp).Ids
      Set lOpn = vOpn
       
      For Each tPoint In lOpn.PointSources
        Set lConn = New HspfConnection
        Set lConn.Uci = Me
        If tPoint.Source.VolName = "MUTSIN" Then
          lConn.Typ = 2
        Else
          lConn.Typ = 1
        End If
        Set lConn.Source = tPoint.Source
        lConn.Ssystem = tPoint.Ssystem
        lConn.Sgapstrg = tPoint.Sgapstrg
        lConn.MFact = tPoint.MFact
        lConn.Tran = tPoint.Tran
        Set lConn.Target = tPoint.Target
        'Me.Connections.Add lConn
        lOpn.Sources.Add lConn
        'now remove all point sources
        lOpn.PointSources.Remove 1
      Next tPoint
   
    Next vOpn
  Next vOpTyp
  'now remove all point sources
  Do Until pPointSources.Count = 0
    i = pPointSources.Count
    pPointSources.Remove (i)
  Loop
  
  'need to synch collection of connections with opn connections
  RemoveConnectionsFromCollection 1  'remove all type ext src
  For Each vOpn In Me.OpnSeqBlock.Opns
    Set lOpn = vOpn
    For i = 1 To lOpn.Sources.Count
      Set lConn = lOpn.Sources(i)
      If lConn.Typ = 1 Then
        Me.Connections.Add lConn
      End If
    Next i
  Next vOpn
  
End Sub

Public Sub MetSeg2Source()
  Dim vOpn As Variant, lOpn As HspfOperation
  Dim vOpTyps As Variant, vOpTyp As Variant
  Dim lConn As HspfConnection, j&, i&
  Dim tMetSeg As HspfMetSeg, vMetSeg As Variant
  Dim newSeg As Boolean
  Dim segRec As Long
 
  vOpTyps = Array("PERLND", "IMPLND", "RCHRES") 'operations with assoc met segs
  For Each vOpTyp In vOpTyps
    For Each vOpn In pOpnBlks(vOpTyp).Ids
      Set lOpn = vOpn
      If Not lOpn.MetSeg Is Nothing Then
        For segRec = 1 To 7
          With lOpn.MetSeg.MetSegRec(segRec)
            If .Typ <> 0 Then 'type exists
              If (lOpn.Name = "RCHRES" And .MFactR > 0#) Or _
                 (lOpn.Name = "PERLND" And .MFactP > 0#) Or _
                 (lOpn.Name = "IMPLND" And .MFactP > 0#) Then
                Set lConn = New HspfConnection
                Set lConn.Uci = Me
                lConn.Typ = 1
                'set source components
                lConn.Source.Group = .Source.Group
                lConn.Source.Member = .Source.Member
                lConn.Source.MemSub1 = .Source.MemSub1
                lConn.Source.MemSub2 = .Source.MemSub2
                lConn.Source.VolId = .Source.VolId
                lConn.Source.VolIdL = .Source.VolIdL
                lConn.Source.VolName = .Source.VolName
                lConn.Ssystem = .Ssystem
                lConn.Sgapstrg = .Sgapstrg
                lConn.Target.Group = "EXTNL"
                If lOpn.Name = "RCHRES" Then
                  lConn.MFact = .MFactR
                  Select Case .Typ
                    Case 1:   lConn.Target.Member = "PREC"
                    Case 2:   lConn.Target.Member = "GATMP"
                    Case 3:   lConn.Target.Member = "DEWTMP"
                    Case 4:   lConn.Target.Member = "WIND"
                    Case 5:   lConn.Target.Member = "SOLRAD"
                    Case 6:   lConn.Target.Member = "CLOUD"
                    Case 7:   lConn.Target.Member = "POTEV"
                  End Select
                Else
                  lConn.MFact = .MFactP
                  Select Case .Typ
                    Case 1:   lConn.Target.Member = "PREC"
                    Case 2:   lConn.Target.Member = "GATMP"
                    Case 3:   lConn.Target.Member = "DTMPG"
                    Case 4:   lConn.Target.Member = "WINMOV"
                    Case 5:   lConn.Target.Member = "SOLRAD"
                    Case 6:   lConn.Target.Member = "CLOUD"
                    Case 7:   lConn.Target.Member = "PETINP"
                  End Select
                  If .Typ = 2 Then
                    'get right air temp member name
                    If lOpn.MetSeg.AirType = 1 Then
                      lConn.Target.Member = "GATMP"
                    ElseIf lOpn.MetSeg.AirType = 2 Then
                      lConn.Target.Member = "AIRTMP"
                      lConn.Target.Group = "ATEMP"
                    End If
                  End If
                End If
                lConn.Tran = .Tran
                lConn.Target.VolName = lOpn.Name
                lConn.Target.VolId = lOpn.Id
                'Me.Connections.Add lConn
                lOpn.Sources.Add lConn
              End If
            End If
          End With
        Next segRec
      End If
    Next vOpn
  Next vOpTyp
  
  'now remove all metsegs
  Do Until pMetSegs.Count = 0
    i = pMetSegs.Count
    pMetSegs.Remove (i)
  Loop
  
  'need to synch collection of connections with opn connections
  RemoveConnectionsFromCollection 1  'remove all type ext src
  For Each vOpn In Me.OpnSeqBlock.Opns
    Set lOpn = vOpn
    For i = 1 To lOpn.Sources.Count
      Set lConn = lOpn.Sources(i)
      If lConn.Typ = 1 Then
        Me.Connections.Add lConn
      End If
    Next i
  Next vOpn
End Sub

Public Sub RunUci(retcod&)
  Dim s$, r&, i&, n$, p$, M$, ret&, start&
  
  'Call F90_SCNDBG(10)
  s = FilenameOnly(pName)
  p = PathNameOnly(pName)
  If Len(p) > 0 Then ChDir (p)
    
  ReportMissingTimsers ret
  If ret = 0 Then  'user chose do anyway after timser warning
  
    i = -1 'dont interp in actscn (itll be done in simscn)
    'Call F90_ACTSCN(i, pWdmUnit(1), pMsgUnit, r, s, Len(s))
    'Call F90_SIMSCN(retcod)
    
    If Len(p) > 0 Then IPC.SendProcessMessage "HSPFUCI", "CURDIR " & p
    IPC.SendProcessMessage "HSPFUCI", "ACTIVATE " & s & " " & i
    M = WaitForChildMessage
    IPC.SendProcessMessage "HSPFUCI", "SIMULATE"  'calls F90_SIMSCN
    M = WaitForChildMessage
    M = WaitForChildMessage 'Activate Complete
    While UCase(InStr(M, "PROGRESS")) > 0
      M = WaitForChildMessage
    Wend
    'Stop 'What should we be doing here exactly? Can't do GetExitCodeProcess any more.
    'ret = GetExitCodeProcess(Me.Monitor.launch.hComputeProcess, i)
    'If i <> &H103 Then
      'need to restart hspfengine
      retcod = Right(M, 2)
    'End If
    
    RestartHSPFEngine
    'have to reset wdms, may have changed pointers during simulate
    ClearWDM
    InitWDMArray
    SetWDMFiles
    
    If IsNumeric(Right(M, 1)) Then
      retcod = Right(M, 1)
    End If
    'next line fixed 10/28/03 to handle new ipc return message
    If Right(M, 1) <> 0 Or Left(M, 24) = "HSPFUCI exited with code" Then
      pErrorDescription = "Fatal HSPF error while running UCI file '" & s & "'." & vbCrLf & vbCrLf & _
              "See the file '" & EchoFileName & "' for more details."
      IPC.SendMonitorMessage pErrorDescription
    End If
  End If
      
End Sub

Public Sub DeleteOperation(delname$, delid&)
  
  Dim i&, j&, nth&
  Dim isource&(), itarget&, iscnt&, vOpn As Variant, lOpn As HspfOperation
  Dim lConn As HspfConnection, lMassLink&, lConnection As HspfConnection
  Dim lopnblk As HspfOpnBlk
  
  'figure out where this opn is in opn seq block and delete it
  nth = 1
  For Each vOpn In pOpnSeqBlk.Opns
    Set lOpn = vOpn
    If lOpn.Name = delname And lOpn.Id = delid Then
      pOpnSeqBlk.Delete nth
    End If
    nth = nth + 1
  Next vOpn
  
  'need to remove from all operation type blocks
  Set lopnblk = pOpnBlks(delname)
  If Not lopnblk.OperFromID(delid) Is Nothing Then
    lopnblk.Ids.Remove "K" & delid
  End If
  
  'remove connections
  'need to remove connections between this and anything else
  i = 1
  iscnt = 0
  itarget = 0
  For Each lConnection In Me.Connections
    If (lConnection.Source.VolName = delname And _
       lConnection.Source.VolId = delid) Or _
       (lConnection.Target.VolName = delname And _
       lConnection.Target.VolId = delid) Then
      lMassLink = lConnection.MassLink
      If lConnection.Target.VolId = delid And _
        lConnection.Target.VolName = delname And _
        lConnection.Source.VolName = delname Then
        'remember the source
        iscnt = iscnt + 1
        ReDim Preserve isource(iscnt)
        isource(iscnt) = lConnection.Source.VolId
      ElseIf lConnection.Source.VolId = delid And _
        lConnection.Source.VolName = delname And _
        lConnection.Target.VolName = delname Then
        'remember the target
        itarget = lConnection.Target.VolId
      End If
      Me.Connections.Remove i
    Else
      i = i + 1
    End If
  Next lConnection
  
  If iscnt > 0 And itarget > 0 Then
    'need to join sources and targets of this deleted opn
    For i = 1 To iscnt
      Set lConnection = New HspfConnection
      Set lConnection.Uci = Me
      lConnection.Typ = 3
      lConnection.Source.VolName = delname
      lConnection.Source.VolId = isource(i)
      Set lConnection.Source.Opn = pOpnBlks(delname).OperFromID(isource(i))
      lConnection.MFact = 1#
      lConnection.Target.VolName = delname
      lConnection.Target.VolId = itarget
      Set lConnection.Target.Opn = pOpnBlks(delname).OperFromID(itarget)
      If lMassLink > 0 Then
        lConnection.MassLink = lMassLink
      Else
        lConnection.MassLink = 3
      End If
      Me.Connections.Add lConnection
      lConnection.Source.Opn.Targets.Add lConnection
      lConnection.Target.Opn.Sources.Add lConnection
    Next i
  End If
  
  'remove this oper from source and target collections for other operations
  For Each vOpn In pOpnSeqBlk.Opns
    Set lOpn = vOpn
    j = 1
    Do While j <= lOpn.Targets.Count
      If lOpn.Targets(j).Target.VolId = delid And lOpn.Targets(j).Target.VolName = delname Then
        lOpn.Targets.Remove j
      Else
        j = j + 1
      End If
    Loop
    j = 1
    Do While j <= lOpn.Sources.Count
      If lOpn.Sources(j).Source.VolId = delid And lOpn.Sources(j).Source.VolName = delname Then
        lOpn.Sources.Remove j
      Else
        j = j + 1
      End If
    Loop
  Next vOpn
  
End Sub

Public Sub OpenWDM(OpenOrCreate&, fname$, fun&, wid$)
  Dim Typ$, lFile As ATCclsTserFile, iret&, Ind&, FileOk As Boolean
  
  If OpenOrCreate = 2 Then
    'need to create
    fun = F90_WDBOPN(OpenOrCreate, fname, Len(fname))
    iret = F90_WDFLCL(fun)
  End If
        
  fun = 0
  AddWDMFile fname, FileOk
  If FileOk Then
    pWdmCount = pWdmCount + 1
    Set lFile = TserFiles.CurrentActive.obj
    Ind = WDMInd(wid)
    pWdmUnit(Ind) = lFile.FileUnit
    Set pWDMObj(Ind) = lFile
    fun = pWdmUnit(Ind)
  End If
  
End Sub

Public Sub ClearWDM()
  Dim i&, M$, lwdmopen&, j&
  
  M = "before close in ClearWDM"
  Call F90_FILSTA(M, Len(M))
  For i = 0 To 4
    If pWdmUnit(i) <> 0 Then
      'lwdmopen = F90_WDMOPN(pWdmUnit(i), pWDMObj(i).Filename, Len(pWDMObj(i).Filename))
      'j = F90_WDFLCL(pWdmUnit(i))  'now happens in clstserwdm
      pWdmUnit(i) = 0
      Set pWDMObj(i) = Nothing
      TserFiles.Delete TserFiles.Active.Count
    End If
  Next i
  
  M = "after close in ClearWDM"
  Call F90_FILSTA(M, Len(M))
  'Call F90_W99OPN
End Sub

Public Sub InitWDMArray()
  Dim i&, M$
  
  pWdmCount = 0
  For i = 0 To 4
    pWdmUnit(i) = 0
  Next i
  If pMsgUnit = 0 Then 'not yet open
    Call F90_WDIINI
    Call F90_WDBFIN
    'IPC.SendProcessMessage "HSPFUCI", "WDIINI"
    'IPC.SendProcessMessage "HSPFUCI", "WDBFIN"
    i = 1
    pMsgUnit = F90_WDBOPN(i, pMsgWDMName, Len(pMsgWDMName))
    IPC.SendProcessMessage "HSPFUCI", "WDBOPN " & pMsgWDMName & " " & i
    M = WaitForChildMessage
    'could be better
    pMsgUnit = Right(M, 3)
  End If
End Sub

Public Sub GetMetSegNames(fun&, numMetSeg&, arrayMetSegs$(), lMetDetails$(), lMetDescs$())
  Dim dsn&, i&, loc$, j&, tempsj As Double, tempej As Double
  Dim sen$, Con$, sdat&(6), edat&(6)
  Dim lts As Collection 'of atcotimser
  Dim ldate As ATCclsTserDate, sj As Double, ej As Double
  Dim llocts As Collection 'of atcotimser

  numMetSeg = 0
    
  'look for matching WDM datasets
  Call findtimser("", "", "PREC", lts)
  'return the names of the data sets from this wdm file
  For i = 1 To lts.Count
    loc = lts(i).Header.loc
    sen = lts(i).Header.sen
    If sen = "COMPUTED" Then
      'see if there is also observed at this location, skip this if there is
      Call findtimser("OBSERVED", loc, "PREC", llocts)
      If llocts.Count > 0 Then
        sen = "SKIP"
      End If
    End If
    If sen = "OBSERVED" Or sen = "COMPUTED" Then
      If Len(loc) > 0 And lts(i).File.Label = "WDM" And lts(i).File.FileUnit = fun Then
        'first get the common dates from prec and pevt at this location
        Call findtimser(sen, loc, "PREC", llocts)
        Set ldate = llocts(1).Dates
        sj = ldate.Summary.SJDay
        ej = ldate.Summary.EJDay
        Call findtimser("", loc, "PEVT", llocts)
        For j = 1 To llocts.Count
          Set ldate = llocts(j).Dates
          tempsj = ldate.Summary.SJDay
          tempej = ldate.Summary.EJDay
          If tempsj > sj Then sj = tempsj
          If tempej < ej Then ej = tempej
        Next j
        'now save info about this met station
        numMetSeg = numMetSeg + 1
        ReDim Preserve arrayMetSegs(numMetSeg)
        arrayMetSegs(numMetSeg - 1) = loc
        ReDim Preserve lMetDetails(numMetSeg)
        dsn = lts(i).Header.Id
        Call J2Date(sj, sdat)
        Call J2Date(ej, edat)
        Call timcnv(edat)
        lMetDetails(numMetSeg - 1) = CStr(dsn) & "," & _
          CStr(sdat(0)) & "," & CStr(sdat(1)) & "," & CStr(sdat(2)) & "," & CStr(sdat(3)) & "," & CStr(sdat(4)) & "," & CStr(sdat(5)) & "," & _
          CStr(edat(0)) & "," & CStr(edat(1)) & "," & CStr(edat(2)) & "," & CStr(edat(3)) & "," & CStr(edat(4)) & "," & CStr(edat(5))
        ReDim Preserve lMetDescs(numMetSeg)
        lMetDescs(numMetSeg - 1) = lts(i).Header.Desc
      End If
    End If
  Next i

End Sub

Public Sub AddExpertDsns(Id As Long, clocn$, basedsn&, adsn&(), ostr$())
  Dim j&, ndsn&, nfrec&, cscen$, wdmsfl&, retcod&, psa&, i&
  Dim GenTs As ATCclsTserData, addeddsn As Boolean
  'Dim lts As Collection 'of atcotimser
  'Dim owdm&, odsn&, tsstep&, tcode&, Desc$, tgroup&, outsit&()
  Dim wdmid&, ccons$
  Dim TsDate As ATCclsTserDate
  Dim myDateSummary As ATTimSerDateSummary

  ostr(1) = "SIMQ    "
  ostr(2) = "SURO    "
  ostr(3) = "IFWO    "
  ostr(4) = "AGWO    "
  ostr(5) = "PETX    "
  ostr(6) = "SAET    "
  ostr(7) = "UZSX    "
  ostr(8) = "LZSX    "
  
  For i = 4 To 1 Step -1
    If pWdmUnit(i) > 0 Then
      'use this as the output wdm
      wdmsfl = pWdmUnit(i)
      wdmid = i
    End If
  Next i
  
  If wdmsfl > 0 Then
    'okay to continue
    ndsn = basedsn
    cscen = FilenameOnly(pName)
    
    For j = 1 To 8
      'create each of the 8 expert system dsns
      
      'find a free dsn
      nfrec = 1
      Do While nfrec > 0
        ndsn = ndsn + 1
        'look in wdmx for this data set
        Set GenTs = GetDataSetFromDsn(wdmid, ndsn)
        If GenTs Is Nothing Then
          'found unused data set number
          nfrec = 0
          Set GenTs = New ATCclsTserData
        End If
      Loop
      
      With GenTs.Header
        .Id = ndsn
        .sen = UCase(cscen)
        .Con = UCase(ostr(j))
        .loc = UCase(clocn)
      End With
      Set TsDate = New ATCclsTserDate
      With myDateSummary
        .CIntvl = True
        .ts = 1
        .Tu = 4
        .Intvl = 1
      End With
      TsDate.Summary = myDateSummary
      Set GenTs.Dates = TsDate
      
      GenTs.AttribSet "TSTYPE", GenTs.Header.Con
      addeddsn = pWDMObj(wdmid).AddTimSer(GenTs, 0)
      adsn(j) = ndsn
    Next j
  Else
    'no wdm files in this uci
    Call MsgBox("No WDM Files are available with this UCI, so no calibration locations may be added", _
                vbOKOnly, "Add Problem")
  End If
End Sub

Public Sub AddAQUATOXDsns(Id As Long, clocn$, basedsn&, plank&, _
                          gqualfg&(), wdmid&, Member$(), _
                          Sub1&(), Group$(), adsn&(), ostr$())
                             
  AddAQUATOXDsnsExt Id, clocn, basedsn, plank, _
                    gqualfg, wdmid, Member, _
                    Sub1, Group, adsn, ostr, 4
End Sub

Public Sub AddAQUATOXDsnsExt(Id As Long, clocn$, basedsn&, plank&, _
                             gqualfg&(), wdmid&, Member$(), _
                             Sub1&(), Group$(), adsn&(), ostr$(), outtu&)
  Dim j&, ndsn&, nfrec&, cscen$, wdmsfl&, retcod&, psa&, i&
  Dim GenTs As ATCclsTserData, addeddsn As Boolean, found As Boolean
  Dim lts As Collection 'of atcotimser
  'Dim owdm&, odsn&, tsstep&, tcode&, Desc$, tgroup&, outsit&()
  Dim ccons$, lMassLink As HspfMassLink
  Dim TsDate As ATCclsTserDate
  Dim myDateSummary As ATTimSerDateSummary
  Dim lOper As HspfOperation
  Dim ltable As HspfTable
  Dim wid As String, deleteddsn As Long
  Dim vConn As Variant, lConn As HspfConnection
  Dim ctmp As String, referenced As Boolean

  Member(1) = "VOL":    Sub1(1) = 1:   Group(1) = "HYDR":    ostr(1) = "VOL     " 'volume (ac.ft) AVER
  Member(2) = "IVOL":   Sub1(2) = 1:   Group(2) = "HYDR":    ostr(2) = "IVOL    " 'inflow (ac.ft) SUM
  Member(3) = "RO":     Sub1(3) = 1:   Group(3) = "HYDR":    ostr(3) = "RO      " 'discharge in cfs AVER
  Member(4) = "SAREA":  Sub1(4) = 1:   Group(4) = "HYDR":    ostr(4) = "SARA     " 'surface area in acres AVER
  Member(5) = "AVDEP":  Sub1(5) = 1:   Group(5) = "HYDR":    ostr(5) = "AVDP    " 'mean depth in feet AVER
  Member(6) = "PRSUPY": Sub1(6) = 1:   Group(6) = "HYDR":    ostr(6) = "PSUP    " 'volume in from precip (ac.ft) SUM
  Member(7) = "VOLEV":  Sub1(7) = 1:   Group(7) = "HYDR":    ostr(7) = "VEVP    " 'volume out to evap (ac.ft) SUM
  Member(8) = "TW":     Sub1(8) = 1:   Group(8) = "HTRCH":   ostr(8) = "TW      " 'water temp in degrees AVER
  Member(9) = "NUIF1":  Sub1(9) = 1:   Group(9) = "NUTRX":   ostr(9) = "NO3     " 'inflow of no3 in lbs SUM
  Member(10) = "NUIF1": Sub1(10) = 2:  Group(10) = "NUTRX":  ostr(10) = "NH3     " 'inflow of nh2 in lbs SUM
  Member(11) = "NUIF1": Sub1(11) = 3:  Group(11) = "NUTRX":  ostr(11) = "NO2     " 'inflow of no2 in lbs SUM
  Member(12) = "NUIF1": Sub1(12) = 4:  Group(12) = "NUTRX":  ostr(12) = "PO4     " 'inflow of po4 in lbs SUM
  Member(13) = "OXIF":  Sub1(13) = 1:  Group(13) = "OXRX":   ostr(13) = "DO      " 'inflow of do in lbs SUM
  Member(14) = "OXIF":  Sub1(14) = 2:  Group(14) = "OXRX":   ostr(14) = "BOD     " 'inflow of bod in lbs SUM
  Member(15) = "PKIF":  Sub1(15) = 5:  Group(15) = "PLANK":  ostr(15) = "ORC     " 'inflow of organic c in lbs SUM
  Member(16) = "PKIF":  Sub1(16) = 1:  Group(16) = "PLANK":  ostr(16) = "PHYT    " 'inflow of phyto in lbs SUM
  Member(17) = "ISED":  Sub1(17) = 1:  Group(17) = "SEDTRN": ostr(17) = "ISD1    " 'inflow of sediment in tons SUM
  Member(18) = "ISED":  Sub1(18) = 2:  Group(18) = "SEDTRN": ostr(18) = "ISD2    " 'inflow of sediment in tons SUM
  Member(19) = "ISED":  Sub1(19) = 3:  Group(19) = "SEDTRN": ostr(19) = "ISD3    " 'inflow of sediment in tons SUM
  Member(20) = "SSED":  Sub1(20) = 1:  Group(20) = "SEDTRN": ostr(20) = "SSD1    " 'sediment conc mg/l AVER
  Member(21) = "SSED":  Sub1(21) = 2:  Group(21) = "SEDTRN": ostr(21) = "SSD2    " 'sediment conc mg/l AVER
  Member(22) = "SSED":  Sub1(22) = 3:  Group(22) = "SEDTRN": ostr(22) = "SSD3    " 'sediment conc mg/l AVER
  Member(23) = "TIQAL": Sub1(23) = 1:  Group(23) = "GQUAL":  ostr(23) = "TIQ1    " 'total inflow of qual SUM
  Member(24) = "TIQAL": Sub1(24) = 2:  Group(24) = "GQUAL":  ostr(24) = "TIQ2    " 'total inflow of qual SUM
  Member(25) = "TIQAL": Sub1(25) = 3:  Group(25) = "GQUAL":  ostr(25) = "TIQ3    " 'total inflow of qual SUM
  Member(26) = "NUIF2": Sub1(26) = 4:  Group(26) = "NUTRX":  ostr(26) = "PPO4    " 'inflow of particulate po4 in lbs SUM
  Member(27) = "TPKIF": Sub1(27) = 2:  Group(27) = "PLANK":  ostr(27) = "TORP    " 'inflow of total organic p in lbs SUM
  Member(28) = "TPKIF": Sub1(28) = 5:  Group(28) = "PLANK":  ostr(28) = "TTP     " 'inflow of total p in lbs SUM
  
  
  If plank <> 1 Then
    ostr(15) = ""
    ostr(16) = ""
    ostr(27) = ""
    ostr(28) = ""
  End If
  
  If gqualfg(1) <> 1 Then  'if any organic chemicals
    ostr(23) = ""
  End If
  If gqualfg(2) <> 1 Then
    ostr(24) = ""
  End If
  If gqualfg(3) <> 1 Then
    ostr(25) = ""
  End If
  
  'check to see that all timsers have inputs
  Set lOper = pOpnBlks("RCHRES").OperFromID(Id)
  If lOper.TableExists("NUT-FLAGS") Then
    Set ltable = lOper.Tables("NUT-FLAGS")
    If ltable.Parms("NH3FG").Value = 0 Then
      ostr(10) = ""
    End If
    If ltable.Parms("NO2FG").Value = 0 Then
      ostr(11) = ""
    End If
    If ltable.Parms("PO4FG").Value = 0 Then
      ostr(12) = ""
    End If
'    If ltable.Parms("ADPOFG").Value = 0 Then
'      ostr(26) = ""
'    End If
  Else
    ostr(10) = ""
    ostr(11) = ""
    ostr(12) = ""
    ostr(26) = ""
  End If
  If lOper.TableExists("PLNK-FLAGS") Then
    Set ltable = lOper.Tables("PLNK-FLAGS")
    If ltable.Parms("PHYFG").Value = 0 Then
      ostr(16) = ""
    End If
  Else
    ostr(16) = ""
  End If
  
  For i = 4 To 1 Step -1
    If pWdmUnit(i) > 0 Then
      'use this as the output wdm
      wdmsfl = pWdmUnit(i)
      wdmid = i
    End If
  Next i
  
  If wdmsfl > 0 Then
    'okay to continue
    ndsn = basedsn
    cscen = FilenameOnly(pName)
    
    For j = 1 To 28
      'create each of the 28 aquatox dsns
      
      If Len(ostr(j)) > 0 Then
      
        'if there is already a dsn with this scen/loc/cons,
        'and it is unused in this uci, delete it to avoid confusion
        deleteddsn = 0
        findtimser UCase(Trim(cscen)), Trim(clocn), Trim(ostr(j)), lts
        For i = 1 To lts.Count
          Set GenTs = lts(i)
          If GenTs.File.Label = "WDM" Then
            GetWDMIDFromUnit GenTs.File.FileUnit, wid
            If CInt(Right(wid, 1)) = wdmid Then
              'this is on our output wdm
              'make sure it is not referenced in this UCI already
              referenced = False
              For Each vConn In Me.Connections
                Set lConn = vConn
                ctmp = lConn.Target.VolName
                If ctmp = "WDM" Then ctmp = "WDM1"
                If ctmp = wid And lConn.Target.VolId = GenTs.Header.Id Then
                  'this dataset is referenced in the uci, don't delete
                  referenced = True
                End If
              Next vConn
              If Not referenced Then
                'delete it to avoid confusion
                deleteddsn = GenTs.Header.Id
                ClearWDMDataSet wid, deleteddsn
                DeleteWDMDataSet wid, deleteddsn
              End If
            End If
          End If
        Next i
      
        If deleteddsn > 0 Then
          Set GenTs = Nothing
          Set GenTs = New ATCclsTserData
          ndsn = deleteddsn
        Else
          'find a free dsn
          nfrec = 1
          Do While nfrec > 0
            ndsn = ndsn + 1
            'look in wdmx for this data set
            Set GenTs = GetDataSetFromDsn(wdmid, ndsn)
            If GenTs Is Nothing Then
              'found unused data set number
              nfrec = 0
              Set GenTs = New ATCclsTserData
            End If
          Loop
        End If
        
        With GenTs.Header
          .Id = ndsn
          .sen = UCase(cscen)
          .Con = UCase(ostr(j))
          .loc = UCase(clocn)
          .Desc = "AQUATOX Linkage Timeseries for " & ostr(j)
        End With
        Set TsDate = New ATCclsTserDate
        With myDateSummary
          .CIntvl = True
          .ts = 1
          If j = 1 Then
            .Tu = 3 'output vol as hourly
          Else
            '.Tu = 4 'the rest as daily by default, or hourly if requested
            .Tu = outtu
          End If
          .Intvl = 1
        End With
        TsDate.Summary = myDateSummary
        Set GenTs.Dates = TsDate
        
        GenTs.AttribSet "TSTYPE", GenTs.Header.Con
        addeddsn = pWDMObj(wdmid).AddTimSer(GenTs, 0)
        adsn(j) = ndsn
      End If
    Next j
  Else
    'no wdm files in this uci
    Call MsgBox("No WDM Files are available with this UCI, so no AQUATOX locations may be added", _
                vbOKOnly, "Add Problem")
  End If
End Sub

Public Sub AddExpertExtTargets(reachid As Long, copyid As Long, ContribArea As Single, adsn&(), ostr$())
  Dim lConn As HspfConnection
  Dim lOpn As HspfOperation
  Dim i&, MFact!, Tran$, gap$
  
  MFact = 12# / ContribArea
  'mfact = Format(mfact, "0.#######")
  AddExtTarget "RCHRES", reachid, "ROFLOW", "ROVOL", 1, 1, MFact, "    ", _
               "WDM", adsn(1), ostr(1), 1, "ENGL", "AGGR", "REPL"
  
  If copyid > 0 Then
    MFact = 1# / ContribArea
    'mfact = Format(mfact, "0.#######")
    For i = 2 To 8
      If i < 7 Then
        Tran = "    "
      Else
        Tran = "AVER"
      End If
      'If i < 5 Then
      '  gap = "    "
      'Else
      gap = "AGGR"
      'End If
      
      AddExtTarget "COPY", copyid, "OUTPUT", "MEAN", i - 1, 1, MFact, Tran, _
                   "WDM", adsn(i), ostr(i), 1, "ENGL", gap, "REPL"
    Next i
  End If
  
End Sub

Public Sub AddAQUATOXExtTargets(reachid As Long, wdmid&, Member$(), _
                                Sub1&(), Group$(), adsn&(), ostr$())
                                
  AddAQUATOXExtTargetsExt reachid, wdmid, Member, _
                          Sub1, Group, adsn, ostr, 4
  
End Sub

Public Sub AddAQUATOXExtTargetsExt(reachid As Long, wdmid&, Member$(), _
                                Sub1&(), Group$(), adsn&(), ostr$(), outtu&)
  Dim lConn As HspfConnection
  Dim lOpn As HspfOperation
  Dim i&, MFact!, Tran$, gap$, Sub2&
  
  For i = 1 To 28
    If Len(ostr(i)) > 0 Then
      If i = 1 Or i = 3 Or i = 4 Or i = 5 Or i = 8 Or i = 20 Or i = 21 Or i = 22 Then
        Tran = "AVER"
      Else
        If Me.OpnSeqBlock.Delt = 1440 And outtu = 4 Then
          'daily run and daily output requested
          Tran = ""
        ElseIf Me.OpnSeqBlock.Delt = 60 And outtu = 3 Then
          'hourly run and hourly output requested
          Tran = ""
        Else
          Tran = "SUM"
        End If
      End If
      gap = "AGGR"
      MFact = 1#
      Sub2 = 1
      If i = 26 Then Sub2 = 2
      AddExtTarget "RCHRES", reachid, Group(i), Member(i), Sub1(i), Sub2, MFact, Tran, _
                   "WDM" & CStr(wdmid), adsn(i), ostr(i), 1, "METR", gap, "REPL"
    End If
  Next i
  
End Sub

Public Sub AddExpertSchematic(reachid As Long, copyid As Long)
  'add schematic block records for expert system copy data sets
  Dim lOpn As HspfOperation
  Dim cOpns As Collection 'of hspfOperations
  Dim i&, j&, found As Boolean
  Dim pml&, iml&, ostr$(10)
  Dim lConn As HspfConnection, lMassLink As HspfMassLink
  
  ostr(1) = "SURO"
  ostr(2) = "IFWO"
  ostr(3) = "AGWO"
  ostr(4) = "PET"
  ostr(5) = "TAET"
  ostr(6) = "UZS"
  ostr(7) = "LZS"
  ostr(8) = "SURO"
  ostr(9) = "PET"
  ostr(10) = "IMPEV"
  
  'determine mass link numbers
  pml = 0
  iml = 0
  For Each lConn In pConnections
    If lConn.Source.VolName = "PERLND" And lConn.Target.VolName = "COPY" Then
      pml = lConn.MassLink
    ElseIf lConn.Source.VolName = "IMPLND" And lConn.Target.VolName = "COPY" Then
      iml = lConn.MassLink
    End If
  Next lConn
  If pml = 0 Then
    'need to add perlnd masslink
    pml = 90
    found = True
    Do Until found = False
      found = False
      For Each lMassLink In pMassLinks
        If lMassLink.MassLinkID = pml Then
          pml = pml + 1
          found = True
          Exit For
        End If
      Next lMassLink
    Loop
    'now add perlnd masslink
    For i = 1 To 7
      Set lMassLink = New HspfMassLink
      Set lMassLink.Uci = Me
      lMassLink.MassLinkID = pml
      lMassLink.Source.VolName = "PERLND"
      lMassLink.Source.VolId = 0
      lMassLink.Source.Group = "PWATER"
      lMassLink.Source.Member = ostr(i)
      lMassLink.MFact = 1#
      lMassLink.Tran = ""
      lMassLink.Target.VolName = "COPY"
      lMassLink.Target.VolId = 0
      lMassLink.Target.Group = "INPUT"
      lMassLink.Target.Member = "MEAN"
      lMassLink.Target.MemSub1 = i
      pMassLinks.Add lMassLink
    Next i
  End If
  If iml = 0 Then
    'need to add implnd masslink
    iml = 91
    found = True
    Do Until found = False
      found = False
      For Each lMassLink In pMassLinks
        If lMassLink.MassLinkID = iml Then
          iml = iml + 1
          found = True
          Exit For
        End If
      Next lMassLink
    Loop
    'now add implnd masslink
    For i = 8 To 10
      Set lMassLink = New HspfMassLink
      Set lMassLink.Uci = Me
      lMassLink.MassLinkID = iml
      lMassLink.Source.VolName = "IMPLND"
      lMassLink.Source.VolId = 0
      lMassLink.Source.Group = "IWATER"
      lMassLink.Source.Member = ostr(i)
      lMassLink.MFact = 1#
      lMassLink.Tran = ""
      lMassLink.Target.VolName = "COPY"
      lMassLink.Target.VolId = 0
      lMassLink.Target.Group = "INPUT"
      lMassLink.Target.Member = "MEAN"
      If i = 8 Then
        lMassLink.Target.MemSub1 = 1
      ElseIf i = 9 Then
        lMassLink.Target.MemSub1 = 4
      ElseIf i = 10 Then
        lMassLink.Target.MemSub1 = 5
      End If
      pMassLinks.Add lMassLink
    Next i
  End If
  
  'add schematic records
  Set lOpn = pOpnBlks("RCHRES").OperFromID(reachid)
  Set cOpns = New Collection
  Call AddCopyToSchematic(lOpn, copyid, pml, iml)
  Call FindUpstreamOpns(lOpn, cOpns)
  
  Do While cOpns.Count > 0
    Set lOpn = cOpns(1)
    cOpns.Remove 1
    Call AddCopyToSchematic(lOpn, copyid, pml, iml)
    Call FindUpstreamOpns(lOpn, cOpns)
  Loop
  
End Sub

Public Sub AddExtTarget(sname$, sid&, sgroup$, Smember$, Smem1&, Smem2&, MFact!, Tran$, _
                        tname$, Tid&, tmember$, Tsub1&, System$, gap$, amd$)
                        
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection
              
  Set lOpn = pOpnBlks(sname).OperFromID(sid)
  Set lConn = New HspfConnection
  Set lConn.Uci = Me
  lConn.Typ = 4
  lConn.Source.VolName = lOpn.Name
  lConn.Source.VolId = lOpn.Id
  lConn.Source.Group = sgroup
  lConn.Source.Member = Smember
  lConn.Source.MemSub1 = Smem1
  lConn.Source.MemSub2 = Smem2
  Set lConn.Source.Opn = lOpn
  lConn.MFact = MFact
  lConn.Tran = Tran
  lConn.Target.VolName = tname
  lConn.Target.VolId = Tid
  lConn.Target.Member = tmember
  lConn.Target.MemSub1 = Tsub1
  lConn.Ssystem = System
  lConn.Sgapstrg = gap
  lConn.Amdstrg = amd
  pConnections.Add lConn
  lOpn.Targets.Add lConn
  
End Sub

Public Sub AddOutputWDMDataSet(clocn$, ccons$, basedsn&, wdmid&, adsn&)
  Dim i&
  
  i = 0
  AddOutputWDMDataSetExt clocn, ccons, basedsn, i, 4, "", adsn
  wdmid = i
End Sub

Public Sub AddOutputWDMDataSetExt(clocn$, ccons$, basedsn&, wdmid&, tunit&, Desc$, adsn&)
  Dim ndsn&, nfrec&, cscen$, wdmsfl&, retcod&, psa&, i&
  Dim GenTs As ATCclsTserData, addeddsn As Boolean
  Dim TsDate As ATCclsTserDate
  Dim myDateSummary As ATTimSerDateSummary
  
  If wdmid = 0 Then
    For i = 4 To 1 Step -1
      If pWdmUnit(i) > 0 Then
        'use this as the output wdm
        wdmsfl = pWdmUnit(i)
        wdmid = i
      End If
    Next i
  Else
    wdmsfl = pWdmUnit(wdmid)
  End If
  
  If wdmsfl > 0 Then
    'okay to continue
    ndsn = basedsn
    cscen = FilenameOnly(pName)
    
    'find a free dsn
    nfrec = 1
    Do While nfrec > 0
      ndsn = ndsn + 1
      'look in wdmx for this data set
      Set GenTs = GetDataSetFromDsn(wdmid, ndsn)
      If GenTs Is Nothing Then
        'found unused data set number
        nfrec = 0
        Set GenTs = New ATCclsTserData
      End If
    Loop
      
    With GenTs.Header
      .Id = ndsn
      .sen = UCase(cscen)
      .Con = UCase(ccons)
      .loc = UCase(clocn)
      .Desc = Desc
    End With
    Set TsDate = New ATCclsTserDate
    With myDateSummary
      .CIntvl = True
      .ts = 1
      .Tu = tunit
      .Intvl = 1
    End With
    TsDate.Summary = myDateSummary
    Set GenTs.Dates = TsDate
      
    GenTs.AttribSet "TSTYPE", GenTs.Header.Con
    addeddsn = pWDMObj(wdmid).AddTimSer(GenTs, 0)
    adsn = ndsn
  End If
End Sub

Public Sub ClearWDMDataSet(wdmid$, adsn&)
  Dim wdmsfl&, Id&
  Dim GenTs As ATCclsTserData, deletedsn As Boolean, adddsn As Boolean
  Dim TsDate As ATCclsTserDate
  Dim myDateSummary As ATTimSerDateSummary

  If Len(wdmid) < 4 Then
    Id = 1
  Else
    Id = CInt(Mid(wdmid, 4, 1))
  End If
  wdmsfl = pWdmUnit(Id)
  If wdmsfl > 0 Then
    Set GenTs = GetDataSetFromDsn(Id, adsn)
    'save attributes
    Dim NewGenTs As New ATCclsTserData
    Set NewGenTs.Attribs = GenTs.Attribs
    
    Set TsDate = New ATCclsTserDate
    With myDateSummary
      .CIntvl = GenTs.Dates.Summary.CIntvl
      .ts = GenTs.Dates.Summary.ts
      .Tu = GenTs.Dates.Summary.Tu
      .Intvl = GenTs.Dates.Summary.Intvl
      .SJDay = GenTs.Dates.Summary.SJDay
      .EJDay = GenTs.Dates.Summary.EJDay
    End With
    TsDate.Summary = myDateSummary
    Set NewGenTs.Dates = TsDate
    
    Set NewGenTs.Header = GenTs.Header
    'delete dsn
    deletedsn = pWDMObj(Id).RemoveTimSer(GenTs)
    'add dsn
    adddsn = pWDMObj(Id).AddTimSer(NewGenTs, 0)
  End If
End Sub

Public Sub DeleteWDMDataSet(wdmid$, adsn&)
  Dim wdmsfl&, Id&
  Dim GenTs As ATCclsTserData, deletedsn As Boolean

  If Len(wdmid) < 4 Then
    Id = 1
  Else
    Id = CInt(Mid(wdmid, 4, 1))
  End If
  wdmsfl = pWdmUnit(Id)
  If wdmsfl > 0 Then
    Set GenTs = GetDataSetFromDsn(Id, adsn)
    '11/06 dont know why but it helps to do summary
    GenTs.Dates.calcSummary
    deletedsn = pWDMObj(Id).RemoveTimSer(GenTs)
  End If
End Sub

Public Sub ClearAllOutputDsns()
  Dim vConn As Variant
  Dim lConn As HspfConnection
  
  For Each vConn In pConnections
    Set lConn = vConn
    If lConn.Typ = 4 Then
      If Mid(lConn.Target.VolName, 1, 3) = "WDM" Then
        'clear this dsn
        ClearWDMDataSet lConn.Target.VolName, lConn.Target.VolId
      End If
    End If
  Next vConn
End Sub

Public Sub AddWDMFile(Name$, FileOk As Boolean)
  Dim TserIndex&, newClsTserFile As ATCclsTserFile, s$, attr&, i&
  
  FileOk = True
  TserIndex = TserFiles.AvailIndexByName("clsTSerWDM")
  If TserIndex = 0 Then 'not a valid type
    MsgBox " AddWDMFile Error: " & TserFiles.ErrorDescription, vbExclamation
    FileOk = False
  Else 'a valid type, create an instance
    TserFiles.Create TserIndex
    'make monitor available
    Set newClsTserFile = TserFiles.CurrentActive.obj
    If IPCset Then Set newClsTserFile.Monitor = IPC
    TserFiles.CurrentActive.obj.MsgUnit = pMsgUnit
    
    FileOk = True
    attr = GetAttr(Name)
    If (attr And vbReadOnly) <> 0 Then
      'i = MsgBox("The WDM file " & Name & " is Read Only and may not be opened in that state." & vbCrLf & "Change file to not Read Only so that it may be opened?", vbExclamation + vbYesNo + vbDefaultButton1, "File Open Problem")
      'If i = vbYes Then 'change to not readonly
        attr = attr - vbReadOnly
        SetAttr Name, attr
      'Else
      '  FileOk = False
      'End If
    End If
    
    If FileOk = True Then
      'read the file
      s = " "
      i = 0
      Do While Len(s) > 0 And i < 1000
        newClsTserFile.Filename = Name
        'check read errors
        i = i + 1
        s = newClsTserFile.ErrorDescription
        DoEvents
      Loop
      If Len(s) > 0 Then 'had a problem
        MsgBox s, vbExclamation, "AddWDMFile Read Error on " & Name
        TserFiles.Delete TserFiles.CurrentActiveIndex
        FileOk = False
      End If
    End If
  End If
End Sub

Public Sub PreScanFilesBlock(pName As String, FilesOK As Boolean, EchoFile$)
  Dim i&, s$, w$, Ind&, lFile As ATCclsTserFile, tname$, tpath$
  
  On Error GoTo x:
  
  's = "PreScanFilesBlock entry"
  'F90_FILSTA s, Len(s)
  
  FilesOK = True
  pWdmCount = 0
  EchoFile = ""
  i = FreeFile(0)
  Open pName For Input As #i
  Do
    Line Input #i, s
    If Left(s, Len("FILES")) = "FILES" Then 'at files block
      While Left(s, Len("END FILES")) <> "END FILES"
        Line Input #i, s
        If InStr(1, s, "***") = 0 Then
          If Left(s, 3) = "WDM" Then
            AddWDMFile Mid(s, 17, Len(s) - 16), FilesOK
            If FilesOK Then
              pWdmCount = pWdmCount + 1
              Set lFile = TserFiles.CurrentActive.obj
              Ind = WDMInd(Left(s, 4))
              pWdmUnit(Ind) = lFile.FileUnit
              Set pWDMObj(Ind) = lFile
            End If
          ElseIf Left(s, Len("END FILES")) <> "END FILES" Then  'make sure the other files are ok
            If Len(s) > 16 Then
              tname = Mid(s, 17, Len(s) - 16)
              tpath = PathNameOnly(tname)
              If Len(Dir(tpath, vbDirectory)) = 0 Then 'folder does not exist
                MsgBox "Error in Files Block:  Folder " & tpath & " does not exist.", vbOKOnly, "Open UCI Problem"
                FilesOK = False
              ElseIf UCase(Right(tname, 4)) = ".MUT" Then  'does this file exist
                If Len(Dir(tname, vbNormal)) = 0 Then
                  MsgBox "Error in Files Block:  Input File " & tname & " does not exist.", vbOKOnly, "Open UCI Problem"
                  FilesOK = False
                End If
              End If
              If Left(s, 5) = "MESSU" Then
                'save echo file name
                EchoFile = tname
              End If
            End If
          End If
        End If
      Wend
      Exit Do
    End If
  Loop
  Close #i
  's = "PreScanFilesBlock exit"
  'F90_FILSTA s, Len(s)
  DoEvents
  Exit Sub
x:
  myMsgBox.Show "Cannot open '" & Mid(s, 17, Len(s) - 16) & "' in PreScanFilesBlock." & _
      vbCrLf & vbCrLf & "Error: " & err.Description, "HSPF Files Error", "+-&OK"
  FilesOK = False
  Close #i
End Sub

Public Sub SetWDMFiles()
  Dim i&, s$, w$, Ind&, lFile As ATCclsTserFile, tname$, tpath$, iret&
  Dim lHFile As HspfFile, vHFile As Variant, FilesOK As Boolean
  Dim ifound As Boolean, j&, M$
  'used after editing files block to open wdm files
  On Error GoTo x:
  
  FilesOK = True
  
  M = "at start of SetWDMFiles"
  Call F90_FILSTA(M, Len(M))
  
  pWdmCount = 0
  For i = 1 To pFilesBlk.Count
    Let lHFile = pFilesBlk.Value(i)
    If Len(lHFile.Typ) > 2 Then
      If Left(lHFile.Typ, 3) = "WDM" Then
        'see if this wdm is already in project
        ifound = False
        If ifound = False And pWdmCount < 4 Then 'add it to project
          M = "just before AddWDMFile"
          Call F90_FILSTA(M, Len(M))
          AddWDMFile lHFile.Name, FilesOK
          M = "at end of AddWDMFile"
          Call F90_FILSTA(M, Len(M))
          If FilesOK Then
            Set lFile = TserFiles.CurrentActive.obj
            s = lHFile.Typ
            Ind = WDMInd(Left(s, 4))
            pWdmUnit(Ind) = lFile.FileUnit
            Set pWDMObj(Ind) = lFile
            pWdmCount = pWdmCount + 1
          Else
            MsgBox "Error in SetWDMFiles"
          End If
        End If
      End If
    End If
  Next i
  Exit Sub
x:
  MsgBox "Error " & err.Description & " in SetWDMFiles"
  FilesOK = False
End Sub

Public Function GetWDMAttr(wdmid$, idsn&, attr$) As String
  Dim Ind&, s$, dsnObj As ATCclsTserData
  
  Set dsnObj = GetDataSetFromDsn(WDMInd(wdmid), idsn)
  If Not (dsnObj Is Nothing) And attr = "LOC" Then
    s = dsnObj.Header.loc
  ElseIf Not (dsnObj Is Nothing) And attr = "CON" Then
    s = dsnObj.Header.Con
  ElseIf Not (dsnObj Is Nothing) And attr = "DESC" Then
    s = dsnObj.Header.Desc
  Else
    s = ""
  End If
  GetWDMAttr = s
End Function

Public Function GetDataSetFromDsn(lWdmInd&, lDsn&) As ATCclsTserData
  Dim i&, lWdmObj As ATCclsTserFile
  
  Set lWdmObj = pWDMObj(lWdmInd)
  With lWdmObj
    For i = 1 To .DataCount
      If lDsn = .Data(i).Header.Id Then
        Set GetDataSetFromDsn = .Data(i)
        Exit Function
      End If
    Next i
    Set GetDataSetFromDsn = Nothing
    'MsgBox "DSN " & lDsn & " does not exist.", vbOKOnly
  End With
End Function

Public Function GetWDMObj(Index&) As ATCclsTserFile
  If Not pWDMObj(Index) Is Nothing Then
    Set GetWDMObj = pWDMObj(Index)
  Else
    Set GetWDMObj = Nothing
  End If
End Function

Public Function GetWDMIdFromName(Name$) As String
  Dim i
  GetWDMIdFromName = "WDM"
  For i = 1 To 4
    If Not pWDMObj(i) Is Nothing Then
      If pWDMObj(i).Filename = Name Then
        GetWDMIdFromName = "WDM" & i
      End If
    End If
  Next i
End Function

Public Sub findtimser(sen$, loc$, Con$, lts As Collection)
  Dim dsn&, i&, j&, k&, l&, s$, GRPSIZ&, r!, imatch%
  Dim vTserFile As Variant, curClsTserFile As ATCclsTserFile, lds As ATTimSerDateSummary
  'Dim t As ATCclsTserData
  'dim newTs As ATTimSer

  Set lts = Nothing
  Set lts = New Collection
  For Each vTserFile In TserFiles.Active
    Set curClsTserFile = vTserFile.obj
    For j = 1 To curClsTserFile.DataCount
      If (sen = curClsTserFile.Data(j).Header.sen Or Len(Trim(sen)) = 0) And _
         (loc = curClsTserFile.Data(j).Header.loc Or Len(Trim(loc)) = 0) And _
         (Con = curClsTserFile.Data(j).Header.Con Or Len(Trim(Con)) = 0) Then 'need this timser
        lts.Add curClsTserFile.Data(j)
      End If
    Next j
  Next vTserFile
    
End Sub

Public Sub EditActivityAll()
  editActivityAllInit Me, Me.icon
End Sub

Public Function UpstreamArea(RCHId&) As Single
  
  Dim lOpn As HspfOperation, vOpn As Variant
  Dim cOpns As Collection 'of hspfOperations
  Dim i&, j&, totalarea!
  
  totalarea = 0
  Set lOpn = pOpnBlks("RCHRES").OperFromID(RCHId)
  Set cOpns = New Collection
  totalarea = totalarea + LocalUpstreamArea(lOpn)
  Call FindUpstreamOpns(lOpn, cOpns)
  
  Do While cOpns.Count > 0
    Set lOpn = cOpns(1)
    cOpns.Remove 1
    totalarea = totalarea + LocalUpstreamArea(lOpn)
    Call FindUpstreamOpns(lOpn, cOpns)
  Loop
  UpstreamArea = totalarea
  
End Function

Private Function LocalUpstreamArea(lOpn As HspfOperation) As Single
  
  Dim iConn As HspfConnection
  Dim i&, j&
  Dim uparea!
  
  uparea = 0
  For i = 1 To lOpn.Sources.Count
    Set iConn = lOpn.Sources(i)
    If iConn.Source.VolName = "PERLND" Or iConn.Source.VolName = "IMPLND" Then
      uparea = uparea + iConn.MFact
    End If
  Next i
  LocalUpstreamArea = uparea
  
End Function

Private Sub FindUpstreamOpns(lOpn As HspfOperation, cOpns As Collection)
  
  Dim iConn As HspfConnection
  Dim i&, j&, tOpn As HspfOperation
  
  For i = 1 To lOpn.Sources.Count
    Set iConn = lOpn.Sources(i)
    If iConn.Source.VolName = "RCHRES" Or iConn.Source.VolName = "BMPRAC" Then
      'add the source operation to the collection
      Set tOpn = iConn.Source.Opn
      cOpns.Add tOpn
    End If
  Next i
  
End Sub

Private Sub AddCopyToSchematic(lOpn As HspfOperation, copyid&, pml&, iml&)
  'adds the copy record to the schematic block for each local land segment
  'contributing to this operation
  
  Dim iConn As HspfConnection, jConn As HspfConnection
  Dim lConn As HspfConnection
  Dim i&, j&, found&
  Dim copyOpn As HspfOperation
  
  For i = 1 To lOpn.Sources.Count
    Set iConn = lOpn.Sources(i)
    If iConn.Source.VolName = "PERLND" Or iConn.Source.VolName = "IMPLND" Then
      'copy this record
      'does this oper to copy already exist?
      Set copyOpn = pOpnBlks("COPY").OperFromID(copyid)
      found = 0
      For j = 1 To copyOpn.Sources.Count
        Set jConn = copyOpn.Sources(j)
        If jConn.Source.VolName = iConn.Source.VolName And _
           jConn.Source.VolId = iConn.Source.VolId Then
          found = j
        End If
      Next j
      If found > 0 Then
        Set jConn = copyOpn.Sources(found)
        jConn.MFact = jConn.MFact + iConn.MFact
      Else 'does not already exist
        Set lConn = New HspfConnection
        lConn.Source.VolName = iConn.Source.VolName
        lConn.Source.VolId = iConn.Source.VolId
        lConn.Typ = iConn.Typ
        lConn.MFact = iConn.MFact
        lConn.Target.VolName = "COPY"
        lConn.Target.VolId = copyid
        If lConn.Source.VolName = "PERLND" Then
          lConn.MassLink = pml
        Else
          lConn.MassLink = iml
        End If
        pConnections.Add lConn
        iConn.Source.Opn.Targets.Add lConn
        Set copyOpn = pOpnBlks("COPY").OperFromID(copyid)
        copyOpn.Sources.Add lConn
      End If
    End If
  Next i
  
End Sub
Public Sub AddOperation(opname$, opid&)
  'add an operation/oper id (ie copy 100) to the uci object
  Dim i&, n$, r&, s$, j&, iwdm&
  Dim lOpn As HspfOperation, vOpn As Variant
  Dim lopnblk As HspfOpnBlk, vOpnBlk As Variant
  Dim lOpnName$, inuse&, freeid&
  
  Set lopnblk = pOpnBlks(opname)
  
  If lopnblk.Count > 0 Then
    'already have some of this operation, make sure this id is not in use
    freeid = 0
    Do Until freeid > 0
      inuse = 0
      For Each lOpn In lopnblk.Ids
        If lOpn.Id = opid Then
          'in use
          inuse = opid
          opid = opid + 1
        End If
      Next lOpn
      If inuse = 0 Then freeid = opid
    Loop
  End If
  
  Set lOpn = New HspfOperation
  lOpn.Name = opname
  lOpn.Id = opid
  Set lOpn.Uci = Me
        
  lopnblk.Ids.Add lOpn, "K" & lOpn.Id
  Set lOpn.OpnBlk = lopnblk
  
End Sub

Public Sub AddTable(opname$, opid&, tabname$)
  'create a new table, or add this operation id to the current table
  Dim lopnblk As HspfOpnBlk
  
  Set lopnblk = pOpnBlks(opname)
  If lopnblk.Count > 0 Then
    'this operation block exists, okay to add table
    Call lopnblk.AddTable(opid, tabname, pMsg.BlockDefs(opname))
  End If
  
End Sub

Public Sub RemoveTable(opname$, opid&, tabname$)
  'remove this operation id from the current table, remove whole table
  'if this is the only operation in the table
  Dim lopnblk As HspfOpnBlk
  
  Set lopnblk = pOpnBlks(opname)
  If lopnblk.Count > 0 Then
    'this operation block exists, okay to remove table
    Call lopnblk.RemoveTable(opid, tabname)
  End If
  
End Sub

Private Sub newOutputDsns(oldn$, newn$, basedsn&, relabs&)
  'build new output dsns on saveas
  Dim lts As Collection 'of atcotimser
  Dim addeddsn As Boolean, Update As Boolean
  Dim wdmsfl&, wdmid&, i&, nfrec&, ndsn&, cwdm$, tstype$
  Dim GenTs As ATCclsTserData
  Dim TsDate As ATCclsTserDate
  Dim lConn As HspfConnection
  Dim vConn As Variant
  Dim myDateSummary As ATTimSerDateSummary
  
  'look for output wdm
  For i = 4 To 1 Step -1
    If pWdmUnit(i) > 0 Then
      'use this as the output wdm
      wdmsfl = pWdmUnit(i)
      wdmid = i
    End If
  Next i
  
  If wdmsfl > 0 Then
    'okay to continue
    'look for matching WDM datasets
    Call findtimser(UCase(oldn), "", "", lts)
    'return the names of the data sets from this wdm file
    ndsn = 0
    For i = 1 To lts.Count
    
      'find a free dsn
      nfrec = 1
      If relabs = 1 Then
        ndsn = lts(i).Header.Id + basedsn - 1
      Else
        If ndsn = 0 Then
          ndsn = basedsn - 1
        End If
      End If
      Do While nfrec > 0
        ndsn = ndsn + 1
        'look in wdmx for this data set
        Set GenTs = GetDataSetFromDsn(wdmid, ndsn)
        If GenTs Is Nothing Then
          'found unused data set number
          nfrec = 0
          Set GenTs = New ATCclsTserData
        End If
      Loop
      
      'set attribs to the old version
      With GenTs.Header
        .Id = ndsn
        .sen = newn
        .Con = lts(i).Header.Con
        .loc = lts(i).Header.loc
        .Desc = lts(i).Header.Desc
      End With
      Set TsDate = New ATCclsTserDate
      With myDateSummary
        .CIntvl = lts(i).Dates.Summary.CIntvl
        .ts = lts(i).Dates.Summary.ts
        .Tu = lts(i).Dates.Summary.Tu
        .Intvl = lts(i).Dates.Summary.Intvl
      End With
      TsDate.Summary = myDateSummary
      Set GenTs.Dates = TsDate
      
      'now add the timser
      addeddsn = AddWDMDataSet(wdmid, ndsn, newn, lts(i).Header.loc, _
                    lts(i).Header.Con, lts(i).Dates.Summary.Tu, _
                    lts(i).Dates.Summary.ts, lts(i).Header.Desc)
      
      'update tstype attribute
      Set GenTs = Me.GetDataSetFromDsn(wdmid, ndsn)
      If Not GenTs Is Nothing Then
        tstype = lts(i).Attrib("TSTYPE")
        GenTs.AttribSet "TSTYPE", tstype
        Update = GenTs.File.WriteDataHeader(GenTs)
      End If
      
      'change the appropriate ext targets record
      cwdm = "WDM" & CStr(wdmid)
      For Each vConn In pConnections
        Set lConn = vConn
        If lConn.Typ = 4 Then
          If (Trim(lConn.Target.VolName) = cwdm Or _
            (Trim(lConn.Target.VolName) = "WDM" And wdmid = 1)) And _
            lConn.Target.VolId = lts(i).Header.Id Then
            'found the old dsn in the ext targets, change it
            lConn.Target.VolId = ndsn
          End If
        End If
      Next vConn
    Next i
    'Me.GetWDMObj(wdmid).Refresh    'Not necessary
  End If
End Sub

Public Function AddWDMDataSet(wdmid&, dsn&, scen$, locn$, cons$, Tu&, ts&, Optional Desc$ = "") As Boolean
  Dim TsDate As ATCclsTserDate
  Dim GenTs As New ATCclsTserData
  Dim myDateSummary As ATTimSerDateSummary
  
  With GenTs.Header
    .Id = dsn
    .sen = UCase(scen)
    .Con = UCase(cons)
    .loc = UCase(locn)
    If Len(Desc) > 0 Then
      .Desc = UCase(Desc)
    End If
  End With
  Set TsDate = New ATCclsTserDate
  With myDateSummary
    .CIntvl = True
    .ts = ts
    .Tu = Tu
    .Intvl = 1
  End With
  TsDate.Summary = myDateSummary
  Set GenTs.Dates = TsDate
  GenTs.AttribSet "TSTYPE", GenTs.Header.Con
  AddWDMDataSet = pWDMObj(wdmid).AddTimSer(GenTs, 0)
  
End Function

Public Sub AddPointSourceDataSet(sen$, loc$, Con$, stanam$, tstype$, _
  ndates&, jdates!(), Load!(), newwdmid$, newdsn&)
  
  Dim j&, ndsn&, nfrec&, cscen$, wdmsfl&, retcod&, psa&, i&
  Dim GenTs As ATCclsTserData, addeddsn As Boolean, SDate&(6), EDate&(6)
  Dim wdmid&, ccons$, nsteps&, aval() As Single
  Dim TsDate As ATCclsTserDate, curdate!, ival&
  Dim myDateSummary As ATTimSerDateSummary
  Dim lts As Collection

    For i = 4 To 1 Step -1
      If pWdmUnit(i) > 0 Then
        'use this as the output wdm
        wdmsfl = pWdmUnit(i)
        wdmid = i
      End If
    Next i
    
    If wdmsfl > 0 Then
      'okay to continue
      ndsn = 7000
      'find a free dsn
      nfrec = 1
      Do While nfrec > 0
        ndsn = ndsn + 1
        'look in wdmx for this data set
        Set GenTs = GetDataSetFromDsn(wdmid, ndsn)
        If GenTs Is Nothing Then
          'found unused data set number
          nfrec = 0
          newdsn = ndsn
          newwdmid = "WDM" & CStr(wdmid)
          Set GenTs = New ATCclsTserData
        End If
      Loop
        
      With GenTs.Header
        .Id = ndsn
        .sen = UCase(sen)
        .Con = UCase(Con)
        .loc = UCase(loc)
        .Desc = stanam
      End With
      Set TsDate = New ATCclsTserDate
      With myDateSummary
        .CIntvl = True
        .ts = 1
        'assume daily
        .Tu = 4
        .Intvl = 1
      End With
      
      If ndates = 0 Then 'get dates from global block
        For i = 0 To 5
          SDate(i) = Me.GlobalBlock.SDate(i)
          EDate(i) = Me.GlobalBlock.EDate(i)
        Next i
        myDateSummary.SJDay = Date2J(SDate)
        myDateSummary.EJDay = Date2J(EDate)
      Else
        myDateSummary.SJDay = jdates(1)
        myDateSummary.EJDay = jdates(ndates)
      End If
      
      nsteps = (myDateSummary.EJDay - myDateSummary.SJDay)
      ReDim aval(nsteps)
      If Con = "Flow" Or Con = "FLOW" Or Con = "flow" Then
        'keep load in cfs
        If ndates = 0 Or ndates = 1 Then  'use this value for all
          For i = 0 To nsteps
            aval(i) = Load(1)
          Next i
        Else
          curdate = jdates(1)
          i = 0
          ival = 1
          Do While curdate < jdates(ndates) 'loop through each day
            aval(i) = Load(ival)
            i = i + 1
            curdate = curdate + 1
            If ival < ndates Then
              If curdate = jdates(ival + 1) Then 'increment value
                ival = ival + 1
              End If
            End If
          Loop
        End If
      Else
        'change load from pounds per hour to pounds per day
        If ndates = 0 Or ndates = 1 Then
          For i = 0 To nsteps  'use this value for all
            aval(i) = Load(1) * 24
          Next i
        Else
          curdate = jdates(1)
          i = 0
          ival = 1
          Do While curdate < jdates(ndates) 'loop through each day
            aval(i) = Load(ival) * 24
            i = i + 1
            curdate = curdate + 1
            If ival < ndates Then
              If curdate = jdates(ival + 1) Then 'increment value
                ival = ival + 1
              End If
            End If
          Loop
        End If
      End If
      myDateSummary.NVALS = nsteps
      Let TsDate.Summary = myDateSummary
      
      Set GenTs.Dates = TsDate
      GenTs.Values = aval
      GenTs.AttribSet "TSTYPE", tstype
        
      addeddsn = pWDMObj(wdmid).AddTimSer(GenTs, 0)
    End If
End Sub

Public Sub AddPoint(wdmid$, wdmdsn&, tarid&, srcname$, targroup$, tarmember$, Sub1&, Sub2&)
  Dim lOpn As HspfOperation
  Dim tPoint As HspfPoint
  Dim idPoint As HspfPoint
  Dim lastid&, Tu&, runts&
  Dim dsnObj As ATCclsTserData
 
  Set lOpn = pOpnBlks("RCHRES").OperFromID(tarid)
  Set dsnObj = Me.GetDataSetFromDsn(WDMInd(wdmid), wdmdsn)
  
  Set tPoint = New HspfPoint
  tPoint.MFact = 1
  tPoint.Source.VolId = wdmdsn
  tPoint.Source.VolName = wdmid
  If Not dsnObj Is Nothing Then
    tPoint.Con = dsnObj.Header.Con
    tPoint.Source.Member = dsnObj.Attrib("TSTYPE")
    Tu = dsnObj.Dates.Summary.Tu
  Else
    Tu = 4
  End If
  If tPoint.Source.Member = "Flow" Or tPoint.Source.Member = "FLOW" Or _
    tPoint.Source.Member = "flow" Then
    'mfactor needs to convert cfs to ac-ft/interval
    tPoint.MFact = 0.0826
    tPoint.Tran = "SAME"
  Else
    'not flow, so assume pounds per day
    runts = 3
    If Me.OpnSeqBlock.Delt = 1440 Then
      runts = 4
    End If
    If Tu > runts Then  'daily pt src in hourly run, for example
      tPoint.Tran = "DIV"
    ElseIf Tu = runts Then 'hourly in hourly run, for example
      tPoint.Tran = "SAME"
    ElseIf Tu < runts Then  'hourly pt src in daily run, for example
      tPoint.Tran = "SUM"
    End If
  End If
  tPoint.Sgapstrg = ""
  tPoint.Ssystem = "ENGL"
  Set tPoint.Target.Opn = lOpn
  tPoint.Target.VolName = "RCHRES"
  tPoint.Target.VolId = tarid
  tPoint.Target.Group = targroup
  tPoint.Target.Member = tarmember
  tPoint.Target.MemSub1 = Sub1
  tPoint.Target.MemSub2 = Sub2
  tPoint.Name = srcname
  
  For Each idPoint In pPointSources
    If idPoint.Name = tPoint.Name And _
       idPoint.Target.VolId = tarid Then
      'use same id as an existing one
      tPoint.Id = idPoint.Id
      Exit For
    End If
  Next
  If tPoint.Id = 0 Then
    lastid = 1
    For Each idPoint In pPointSources
      If idPoint.Id >= lastid Then
        lastid = idPoint.Id + 1
      End If
    Next
    'this is the id for the new one
    tPoint.Id = lastid
  End If
  pPointSources.Add tPoint
  lOpn.PointSources.Add tPoint
End Sub

Public Sub RemovePoint(wdmid$, wdmdsn&, tarid&)
  Dim lOpn As HspfOperation
  Dim idPoint As HspfPoint
  Dim irem&
 
  Set lOpn = pOpnBlks("RCHRES").OperFromID(tarid)
  
  irem = 0
  For Each idPoint In pPointSources
    irem = irem + 1
    If idPoint.Source.VolName = wdmid And _
       idPoint.Source.VolId = wdmdsn And _
       idPoint.Target.VolId = tarid Then
      'remove this one
      Exit For
    End If
  Next
  pPointSources.Remove irem
  
  irem = 0
  For Each idPoint In lOpn.PointSources
    irem = irem + 1
    If idPoint.Source.VolName = wdmid And _
       idPoint.Source.VolId = wdmdsn And _
       idPoint.Target.VolId = tarid Then
      'remove this one
      Exit For
    End If
  Next
  lOpn.PointSources.Remove irem
  
End Sub

Public Sub GetWDMUnits(nwdm&, aunits&())
  Dim i&
  Dim wdmid&, ccons$

  nwdm = 0
  For i = 1 To 4
    If pWdmUnit(i) > 0 Then
      'add
      nwdm = nwdm + 1
      ReDim Preserve aunits(nwdm)
      aunits(nwdm) = pWdmUnit(i)
    End If
  Next i
End Sub

Public Sub GetWDMIDFromUnit(nunit&, Id$)
  Dim i&
  
  Id = ""
  For i = 1 To 4
    If pWdmUnit(i) > 0 Then
      If pWdmUnit(i) = nunit Then
        Id = "WDM" & CStr(i)
      End If
    End If
  Next i
End Sub

Public Sub RemoveConnectionsFromCollection(itype&)
  Dim i&, lConn As HspfConnection
  
  i = 1
  Do While i <= Me.Connections.Count
    'remove this type of connections from pconnections collection
    Set lConn = Me.Connections(i)
    If lConn.Typ = itype Then
      Me.Connections.Remove i
    Else
      i = i + 1
    End If
  Loop
End Sub

Public Function Copy() As HspfUci
  Dim lUCI As HspfUci
  Set lUCI = New HspfUci
  
  lUCI.Name = Me.Name
  
  Set Copy = lUCI
End Function

Public Function WaitForChildMessage() As String
  'Dim pipeBuffer$, res&, ExitCode&
  Dim s As String

  Do 'process messages from parent
    'res = GetExitCodeProcess(pHspfEngine.launch.hComputeProcess, ExitCode)
    'If ExitCode = &H103 Then 'back end STILL_ACTIVE
      s = IPC.GetProcessMessage("HSPFUCI") 'pHspfEngine.ReadTokenFromPipe(IPC.ParentRead, pipeBuffer, False)
    'Else
    '  s = "Child has exited"
    'End If
    If Len(s) > 3 Then
      Select Case (LCase(Left(s, 3)))
      Case "dbg", "msg" ', "com", "act"
        IPC.SendMonitorMessage s
        s = ""
      End Select
    End If
  Loop While Len(s) = 0
  WaitForChildMessage = s
  
End Function

Public Function EchoFileName() As String
  Dim i&
  
  EchoFileName = ""
  For i = 1 To pFilesBlk.Count
    If pFilesBlk.Value(i).Typ = "MESSU" Then
      EchoFileName = pFilesBlk.Value(i).Name
    End If
  Next i
End Function

Private Sub ReportMissingTimsers(retcod&)
  Dim lOpn As HspfOperation, coll As Collection
  Dim vOpn As Variant, i&, ctxt$, iresp&
  
  If Me.MetSegs.Count > 0 Then
    MetSeg2Source
  End If
  Point2Source

  ctxt = ""
  Set coll = Nothing
  For Each vOpn In pOpnSeqBlk.Opns
    Set lOpn = vOpn
    Set coll = Nothing
    'lOpn.InputTimeseriesStatus.Update
    Set coll = lOpn.InputTimeseriesStatus.GetInfo(HspfStatusRequired, HspfStatusMissing)
    If coll.Count > 0 Then
      For i = 1 To coll.Count
        ctxt = ctxt & vbCrLf & lOpn.Name & " " & lOpn.Id & " " & coll(i).Name
      Next i
    End If
  Next vOpn
  Source2MetSeg
  Source2Point
  retcod = 0
  If Len(ctxt) > 0 Then
    'some missing timsers
    iresp = myMsgBox.Show("WinHSPF has detected missing input time series" & vbCrLf & "required for the selected simulation options:" & _
      vbCrLf & ctxt & vbCrLf & vbCrLf & "Do you want to try running HSPF anyway?", "WinHSPF Simulate Problem", "&OK", "+-&Cancel")
    retcod = iresp - 1
  End If
End Sub

Public Sub PollutantsBuild()
  modPollutantsBuild Me, pMsg
End Sub

Public Sub PollutantsUnBuild()
  modPollutantsUnBuild Me, pMsg
End Sub

Public Property Get StarterPath() As String
  If pInitialized Then Let StarterPath = pStarterPath
End Property
Public Property Let StarterPath(newStarterPath As String)
  pStarterPath = newStarterPath
End Property

Private Sub ProcessFTables()
  Dim init&, OmCode&, retkey&, cbuff$, retcod&, rectyp&
  Dim done As Boolean
  Dim i&, j&, Id&, lOpn As HspfOperation, vOpn As Variant, tOpn As HspfOperation
  
  OmCode = HspfOmCode("FTABLES")
  init = 1
  done = False
  retkey = -1
  Do Until done
    If Me.FastFlag Then
      GetNextRecordFromBlock "FTABLES", retkey, cbuff, rectyp, retcod
    Else
      Call REM_XBLOCK(Me, OmCode, init, retkey, cbuff, retcod)
    End If
    init = 0
    If Mid(cbuff, 3, 6) = "FTABLE" Then  'this is a new one
      Id = CInt(Right(Trim(cbuff), 3))
      'find which oper this ftable is associated with
      For Each vOpn In Me.OpnBlks("RCHRES").Ids
        Set tOpn = vOpn
        If tOpn.Tables("HYDR-PARM2").Parms("FTBUCI") = Id Then
          Set lOpn = tOpn
          Exit For
        End If
      Next vOpn
      If Not lOpn Is Nothing Then
        If Me.FastFlag Then
          rectyp = -999
          Do Until rectyp = 0
            GetNextRecordFromBlock "FTABLES", retkey, cbuff, rectyp, retcod
          Loop
        Else
          Call REM_XBLOCK(Me, OmCode, init, retkey, cbuff, retcod)
        End If
        With lOpn.FTable
          .Nrows = Left(cbuff, 5)
          .Ncols = Mid(cbuff, 6, 5)
          i = 1
          Do While i <= .Nrows
            If Me.FastFlag Then
              GetNextRecordFromBlock "FTABLES", retkey, cbuff, rectyp, retcod
            Else
              rectyp = 0
              Call REM_XBLOCK(Me, OmCode, init, retkey, cbuff, retcod)
            End If
            If rectyp = -1 Then
              'this is a comment
              If Len(.Comment) = 0 Then
                .Comment = cbuff
              Else
                .Comment = .Comment & vbCrLf & cbuff
              End If
            Else
              'this is a regular record
              .Depth(i) = Left(cbuff, 10)
              .DepthAsRead(i) = Left(cbuff, 10)
              .Area(i) = Mid(cbuff, 11, 10)
              .AreaAsRead(i) = Mid(cbuff, 11, 10)
              .Volume(i) = Mid(cbuff, 21, 10)
              .VolumeAsRead(i) = Mid(cbuff, 21, 10)
              j = .Ncols - 3
              If j > 0 Then
                .Outflow1(i) = Mid(cbuff, 31, 10)
                .Outflow1AsRead(i) = Mid(cbuff, 31, 10)
              End If
              If j > 1 Then
                .Outflow2(i) = Mid(cbuff, 41, 10)
                .Outflow2AsRead(i) = Mid(cbuff, 41, 10)
              End If
              If j > 2 Then
                .Outflow3(i) = Mid(cbuff, 51, 10)
                .Outflow3AsRead(i) = Mid(cbuff, 51, 10)
              End If
              If j > 3 Then
                .Outflow4(i) = Mid(cbuff, 61, 10)
                .Outflow4AsRead(i) = Mid(cbuff, 61, 10)
              End If
              If j > 4 Then
                .Outflow5(i) = Mid(cbuff, 71, 10)
                .Outflow5AsRead(i) = Mid(cbuff, 71, 10)
              End If
              i = i + 1
            End If
          Loop
        End With
      End If
    ElseIf Trim(cbuff) = "END FTABLES" Then
      done = True
    ElseIf retkey = 0 Then
      done = True
    ElseIf retcod = 10 Then
      done = True
    End If
  Loop
  
End Sub

Private Sub RestartHSPFEngine()
  Dim s$, HSPFEngineExe$
  Dim ff As New ATCoFindFile
  
  ff.SetDialogProperties "Please locate HSPFEngine.exe", "HSPFEngine.exe"
  ff.SetRegistryInfo "HSPFEngine", "files", "HSPFEngine.exe"
  HSPFEngineExe = ff.GetName
  
  IPC.ExitProcess "HSPFUCI"
  IPC.StartProcess "HSPFUCI", HSPFEngineExe & " " & GetCurrentProcessId
  IPC.SendProcessMessage "HSPFUCI", "W99OPN"
  IPC.SendProcessMessage "HSPFUCI", "WDBFIN"
  IPC.SendProcessMessage "HSPFUCI", "PUTOLV 10"
  IPC.SendProcessMessage "HSPFUCI", "SPIPH " & _
                      CStr(IPC.hPipeReadFromParent("HSPFUCI")) & " " & _
                      CStr(IPC.hPipeWriteToParent("HSPFUCI")) & " "
  IPC.SendProcessMessage "HSPFUCI", "WDBOPN " & pMsgWDMName & " 1"
  s = WaitForChildMessage
End Sub

Public Function CatAsInt(s As String) As Long
  'turn a two character category tag into its integer equivalent
  Dim i As Long
  If Len(s) > 0 Then
    If Not Me.CategoryBlock Is Nothing Then
      'have category block
      For i = 1 To Me.CategoryBlock.Count
        If Me.CategoryBlock.Value(i).Tag = s Then
          CatAsInt = i
        End If
      Next i
    End If
  End If
End Function

Public Function IntAsCat(Member As String, sub1or2 As Long, sint As String) As String
  'given a timeseries member name and a subscript, see if there is a
  'category equivalent.  if so, turn the integer category tag into its
  'two character equivalent
  Dim i As Long
  IntAsCat = sint
  If Not Me.CategoryBlock Is Nothing Then
    If IsNumeric(sint) Then
      i = CInt(sint)
      If Me.CategoryBlock.Count > 0 And Me.CategoryBlock.Count >= i Then
        'have category block
        'check to see if this one is valid to convert into a category tag
        If Member = "COTDGT" And sub1or2 = 2 Or _
           Member = "CIVOL" And sub1or2 = 1 Or _
           Member = "CVOL" And sub1or2 = 1 Or _
           Member = "CRO" And sub1or2 = 1 Or _
           Member = "CO" And sub1or2 = 2 Or _
           Member = "CDFVOL" And sub1or2 = 2 Or _
           Member = "CROVOL" And sub1or2 = 1 Or _
           Member = "COVOL" And sub1or2 = 2 Then
          IntAsCat = Me.CategoryBlock.Value(i).Tag
        End If
      End If
    End If
  End If
End Function

