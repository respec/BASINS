Option Strict Off
Option Explicit On

Imports atcUtility

<System.Runtime.InteropServices.ProgId("HspfUci_NET.HspfUci")> Public Class HspfUci
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Declare Function GetCurrentProcessId Lib "kernel32" () As Integer

	Dim pMsgWDMName As String
	Dim pMsgUnit As Integer
	Dim pMsg As HspfMsg
	Dim pWdmUnit(4) As Integer
	Dim pWDMObj(4) As ATCData.ATCclsTserFile
	Dim pWdmCount As Integer
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
	
	Private pIcon As System.Drawing.Image
	
	Public Property Monitor() As Object
		Get
			Monitor = IPC
		End Get
		Set(ByVal Value As Object)
			IPC = Value
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
		End Set
	End Property
	
    'Public WriteOnly Property HelpFile() As String
    '	Set(ByVal Value As String)
    '		'UPGRADE_ISSUE: App property App.HelpFile was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
    '		App.HelpFile = Value
    '	End Set
    'End Property
	
	
	Public Property Edited() As Boolean
		Get
			Edited = pEdited
		End Get
		Set(ByVal Value As Boolean)
			pEdited = Value
		End Set
	End Property
	
	
	Public Property FastFlag() As Boolean
		Get
			FastFlag = pFastFlag
		End Get
		Set(ByVal Value As Boolean)
			pFastFlag = Value
		End Set
	End Property
	
	
	Public Property AcidphFlag() As Boolean
		Get
			AcidphFlag = pAcidphFlag
		End Get
		Set(ByVal Value As Boolean)
			pAcidphFlag = Value
		End Set
	End Property
	
	Public Property Initialized() As Boolean
		Get
			Initialized = pInitialized
			If Not (pInitialized) Then
				pErrorDescription = "UCI File not Initialized"
			End If
		End Get
		Set(ByVal Value As Boolean)
			pInitialized = Value
		End Set
	End Property
	
	Public Property Msg() As HspfMsg
		Get
			Msg = pMsg
		End Get
		Set(ByVal Value As HspfMsg)
			pMsg = Value
		End Set
	End Property
	
	Public WriteOnly Property StatusIn() As Integer
		Set(ByVal Value As Integer)
			'pStatusIn = newStatusIn
		End Set
	End Property
	
	Public WriteOnly Property StatusOut() As Integer
		Set(ByVal Value As Integer)
			'pStatusOut = newStatusOut
		End Set
	End Property
	
	Public Property icon() As System.Drawing.Image
		Get
			icon = pIcon
		End Get
		Set(ByVal Value As System.Drawing.Image)
			pIcon = Value
            'TODO: myMsgBox.icon = Value
		End Set
	End Property
	
	
	Public Property ErrorDescription() As String
		Get
			ErrorDescription = pErrorDescription
			pErrorDescription = ""
		End Get
		Set(ByVal Value As String)
			pErrorDescription = Value
		End Set
	End Property
	
	Public Property GlobalBlock() As HspfGlobalBlk
		Get
            If Initialized Then GlobalBlock = pGlobalBlk
		End Get
		Set(ByVal Value As HspfGlobalBlk)
			pGlobalBlk = Value
		End Set
	End Property
	
	Public Property FilesBlock() As HspfFilesBlk
		Get
			If Initialized Then FilesBlock = pFilesBlk
		End Get
		Set(ByVal Value As HspfFilesBlk)
			pFilesBlk = Value
		End Set
	End Property
	
	Public Property CategoryBlock() As HspfCategoryBlk
		Get
			If Initialized Then CategoryBlock = pCategoryBlk
		End Get
		Set(ByVal Value As HspfCategoryBlk)
			pCategoryBlk = Value
		End Set
	End Property
	
	Public ReadOnly Property MonthData() As HspfMonthData
		Get
			MonthData = pMonthData
		End Get
	End Property
	
	Public Property OpnSeqBlock() As HspfOpnSeqBlk
		Get
			If Initialized Then OpnSeqBlock = pOpnSeqBlk
		End Get
		Set(ByVal Value As HspfOpnSeqBlk)
			pOpnSeqBlk = Value
		End Set
	End Property
	
	Public Property Connections() As Collection
		Get 'of HspfConnection
			Connections = pConnections
		End Get
		Set(ByVal Value As Collection) 'of HspfConnection
			Dim lConnection As HspfConnection
			Dim vConn As Object
			For	Each vConn In Value
				lConnection = vConn
				pConnections.Add(lConnection)
			Next vConn
		End Set
	End Property
	
	Public ReadOnly Property MetSegs() As Collection
		Get 'of HSPFMetSeg
			MetSegs = pMetSegs
		End Get
	End Property
	
	Public ReadOnly Property PointSources() As Collection
		Get 'of HSPFPoint
			PointSources = pPointSources
		End Get
	End Property
	
	Public ReadOnly Property Pollutants() As Collection
		Get 'of HSPFPollutant
			Pollutants = pPollutants
		End Get
	End Property
	
	Public Property MassLinks() As Collection
		Get 'of HspfMassLinks
			MassLinks = pMassLinks
		End Get
		Set(ByVal Value As Collection) 'of HspfMassLinks
			Dim lMassLink As HspfMassLink
			Dim vMassLink As Object
			For	Each vMassLink In Value
				lMassLink = vMassLink
				pMassLinks.Add(lMassLink)
			Next vMassLink
		End Set
	End Property
	
    Public Property OpnBlks() As Collection 'of HspfOpnBlk
        Get
            OpnBlks = pOpnBlks
        End Get
        Set(ByVal Value As Collection) 'of HspfOpnBlk
            For Each lopnblk As HspfOpnBlk In Value
                pOpnBlks.Add(lopnblk, lopnblk.Name)
            Next
        End Set
    End Property
	
	Public Property Name() As String
		Get
			Name = pName
		End Get
		Set(ByVal Value As String)
			pName = Value
		End Set
	End Property
	
	Public WriteOnly Property MsgWDMName() As String
		Set(ByVal Value As String)
			pMsgWDMName = Value
		End Set
	End Property
	
	Public ReadOnly Property SpecialActionBlk() As HspfSpecialActionBlk
		Get
			SpecialActionBlk = pSpecialActionBlk
		End Get
	End Property
	
	Public ReadOnly Property WDMCount() As Integer
		Get
			WDMCount = pWdmCount
		End Get
	End Property
	
	Public Property MessageUnit() As Integer
		Get
			MessageUnit = pMsgUnit
		End Get
		Set(ByVal Value As Integer)
			If Value = 0 Then
				F90_MSGUNIT(pMsgUnit)
			Else 'could check to be sure?
				pMsgUnit = Value
			End If
		End Set
	End Property
	
	
	Public Property MaxAreaByLand2Stream() As Double
		Get
			If pMaxAreaByLand2Stream = 0 Then
				CalcMaxAreaByLand2Stream()
			End If
			MaxAreaByLand2Stream = pMaxAreaByLand2Stream
		End Get
		Set(ByVal Value As Double)
			pMaxAreaByLand2Stream = Value
		End Set
	End Property
	
	Public Property StarterPath() As String
		Get
            If Initialized Then StarterPath = pStarterPath
		End Get
		Set(ByVal Value As String)
			pStarterPath = Value
		End Set
	End Property
	
	Public Sub Save()
		Dim f As Short
        Dim lopnblk As HspfOpnBlk
		Dim i As Integer
		
		f = FreeFile()
		FileOpen(f, pName, OpenMode.Output)
		
		PrintLine(f, "RUN")
		
		For i = 1 To cOrder.Count()			
            If cOrder.Item(i) = "GLOBAL" Then
                pGlobalBlk.WriteUciFile((f))
            ElseIf cOrder.Item(i) = "FILES" Then
                pFilesBlk.WriteUciFile((f))
            ElseIf cOrder.Item(i) = "CATEGORY" Then
                If Not pCategoryBlk Is Nothing Then
                    If pCategoryBlk.Count > 0 Then
                        pCategoryBlk.WriteUciFile((f))
                    End If
                End If
            ElseIf cOrder.Item(i) = "OPN SEQUENCE" Then
                pOpnSeqBlk.WriteUciFile((f))
            ElseIf cOrder.Item(i) = "MONTH DATA" Then
                If Not pMonthData Is Nothing Then
                    pMonthData.WriteUciFile((f))
                End If
            ElseIf cOrder.Item(i) = "PERLND" Then
                lopnblk = OpnBlks.Item("PERLND")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "IMPLND" Then
                lopnblk = OpnBlks.Item("IMPLND")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "RCHRES" Then
                lopnblk = OpnBlks.Item("RCHRES")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "FTABLES" Then
                lopnblk = OpnBlks.Item("RCHRES")
                If lopnblk.Count > 0 Then
                    lopnblk.Ids.Item(1).FTable.WriteUciFile(f)
                End If
            ElseIf cOrder.Item(i) = "COPY" Then
                lopnblk = OpnBlks.Item("COPY")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "PLTGEN" Then
                lopnblk = OpnBlks.Item("PLTGEN")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "DISPLY" Then
                lopnblk = OpnBlks.Item("DISPLY")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "DURANL" Then
                lopnblk = OpnBlks.Item("DURANL")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "GENER" Then
                lopnblk = OpnBlks.Item("GENER")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "MUTSIN" Then
                lopnblk = OpnBlks.Item("MUTSIN")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "BMPRAC" Then
                lopnblk = OpnBlks.Item("BMPRAC")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "REPORT" Then
                lopnblk = OpnBlks.Item("REPORT")
                If lopnblk.Count > 0 Then
                    Call lopnblk.WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "CONNECTIONS" Then
                If pConnections.Count() > 0 Then
                    Call pConnections.Item(1).WriteUciFile(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "MASSLINKS" Then
                If pMassLinks.Count() > 0 Then
                    Call pMassLinks.Item(1).writeMassLinks(f, pMsg)
                End If
            ElseIf cOrder.Item(i) = "SPECIAL ACTIONS" Then
                If Not pSpecialActionBlk Is Nothing Then
                    pSpecialActionBlk.WriteUciFile((f))
                End If
            End If
        Next i
		
		PrintLine(f, " ")
		PrintLine(f, "END RUN")
		FileClose(f)
		pEdited = False
	End Sub
	
	Public Sub SaveAs(ByRef oldname As String, ByRef newName As String, ByRef basedsn As Integer, ByRef relabs As Integer)
		If oldname <> newName Then
			Call pFilesBlk.newName(oldname, newName)
			Call newOutputDsns(oldname, newName, basedsn, relabs)
		End If
		Call Save()
	End Sub
	
    Public Sub New()
        MyBase.New()
        pName = ""
        pErrorDescription = ""
        pEdited = False
        pFastFlag = False
        pInitialized = False
        pMaxAreaByLand2Stream = 0
        'myMsgBox = New ATCoCtl.ATCoMessage

        'init others as appropriate

        pMsg = Nothing
        pConnections = New Collection
        pOpnBlks = New Collection
        pMetSegs = New Collection
        pPointSources = New Collection
        pMassLinks = New Collection
        pPollutants = New Collection

        cOrder = New Collection
        cOrder.Add("GLOBAL")
        cOrder.Add("FILES")
        cOrder.Add("OPN SEQUENCE")
        cOrder.Add("MONTH DATA")
        cOrder.Add("CATEGORY")
        cOrder.Add("PERLND")
        cOrder.Add("IMPLND")
        cOrder.Add("RCHRES")
        cOrder.Add("FTABLES")
        cOrder.Add("COPY")
        cOrder.Add("PLTGEN")
        cOrder.Add("DISPLY")
        cOrder.Add("DURANL")
        cOrder.Add("GENER")
        cOrder.Add("MUTSIN")
        cOrder.Add("BMPRAC")
        cOrder.Add("REPORT")
        cOrder.Add("CONNECTIONS")
        cOrder.Add("MASSLINKS")
        cOrder.Add("SPECIAL ACTIONS")

        InitAtCoTser()
    End Sub
	
	Public Sub CreateUci(ByRef M As HspfMsg, ByRef newName As String, ByRef outputwdm As String, ByRef metwdms() As String, ByRef wdmids() As String, ByRef MetDataDetails As String, ByRef oneseg As Boolean, ByRef PollutantList As Collection)
		
		'Call F90_SPIPH(pStatusIn, pStatusOut)
		Call CreateUciFromBASINS(Me, M, newName, outputwdm, metwdms, wdmids, MetDataDetails, oneseg, PollutantList)
	End Sub
	
	Public Sub FastReadUciForStarter(ByRef Msg As HspfMsg, ByRef newName As String)
		Dim FilesOK As Boolean
		Dim FullFg As Integer
		Dim EchoFile As String
		
		pFastFlag = True
		FullFg = -1
		ReadUci(Msg, newName, FullFg, FilesOK, EchoFile)
		pFastFlag = False
	End Sub
	
	Public Sub FastReadUci(ByRef Msg As HspfMsg, ByRef newName As String)
		'called by scripthspf, processes wdm files
		Dim FilesOK As Boolean
		Dim FullFg As Integer
		Dim EchoFile As String
		
		pFastFlag = True
		FullFg = -3
		ReadUci(Msg, newName, FullFg, FilesOK, EchoFile)
		pFastFlag = False
	End Sub

    ''' <summary>
    ''' Read UCI file into this class
    ''' </summary>
    ''' <param name="Msg">HspfMsg file object</param>
    ''' <param name="newName">File to read</param>
    ''' <param name="FullFg">-3 = , -1 = starter</param>
    ''' <param name="FilesOK">gets set to True if files are ok, false if not</param>
    ''' <param name="EchoFile"></param>
    ''' <remarks></remarks>
    Public Sub ReadUci(ByRef Msg As HspfMsg, _
                       ByRef newName As String, _
                       ByRef FullFg As Integer, _
                       ByRef FilesOK As Boolean, _
                       ByRef EchoFile As String)
        Dim j, i, r, iwdm As Integer
        Dim M, n, s, m2 As String
        Dim lOpn As HspfOperation
        Dim lopnblk As HspfOpnBlk
        Dim lOpnName As String
        Dim ret As Integer
        Dim lConnection As HspfConnection
        Dim lMassLink As HspfMassLink

        On Error Resume Next

        pMsg = Msg
        FilesOK = True
        If Not IO.File.Exists(newName) Then
            pErrorDescription = "UciFileName '" & newName & "' not found"
        Else
            pName = newName
            If FullFg <> -1 Then 'not doing starter, process wdm files
                PreScanFilesBlock(pName, FilesOK, EchoFile)
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
                    ReadUCIRecords(pName)
                Else
                    'do normal activate of uci, including running the run interpreter
                    IPC.SendProcessMessage("HSPFUCI", "CURDIR " & CurDir())
                    IPC.SendProcessMessage("HSPFUCI", "ACTIVATE " & s & " " & i)
                    'IPC.SendProcessMessage "HSPFUCI", "ACTIVATE " & s & " " & -1   'should not run interpret
                    'Call F90_ACTSCN(i, pWdmUnit(1), pMsgUnit, r, s, Len(s))
                    M = WaitForChildMessage()
                    If Left(M, 6) = "CURDIR" Then M = WaitForChildMessage()
                    If CDbl(Right(M, 1)) <> 0 Or Left(M, 24) = "HSPFUCI exited with code" Then
                        'would be helpful to include m here
                        pErrorDescription = "Error interpreting UCI File '" & s & "'." & vbCrLf & vbCrLf & "See the file '" & Trim(EchoFile) & "' for more details." '& vbCrLf & vbCrLf & M
                        IPC.SendMonitorMessage(pErrorDescription)
                    End If
                    'also do fast read -- this would be a very significant performance improvement
                    ReadUCIRecords(pName)
                    pFastFlag = True
                End If

                pInitialized = True

                IPC.SendMonitorMessage("(Show)") 'where was the hide?
                If Not pFastFlag Then
                    IPC.SendMonitorMessage("(Msg1 Building Collections)")
                End If

                If pFastFlag Then
                    SaveBlockOrder(cOrder)
                End If

                pGlobalBlk = New HspfGlobalBlk
                pGlobalBlk.Uci = Me
                pGlobalBlk.ReadUciFile()

                pFilesBlk = New HspfFilesBlk
                pFilesBlk.Uci = Me
                pFilesBlk.ReadUciFile()

                pCategoryBlk = New HspfCategoryBlk
                pCategoryBlk.Uci = Me
                pCategoryBlk.ReadUciFile()

                pMonthData = New HspfMonthData
                pMonthData.Uci = Me
                pMonthData.ReadUciFile()

                pOpnSeqBlk = New HspfOpnSeqBlk
                pOpnSeqBlk.Uci = Me
                pOpnSeqBlk.ReadUciFile()

                pOpnBlks = Nothing
                pOpnBlks = New Collection
                i = 1
                lOpnName = HspfOperName(i)
                While lOpnName <> "UNKNOWN"
                    lopnblk = New HspfOpnBlk
                    lopnblk.Name = lOpnName
                    lopnblk.Uci = Me
                    pOpnBlks.Add(lopnblk, lOpnName)
                    i = i + 1
                    lOpnName = HspfOperName(i)
                End While
                For Each lOpn In pOpnSeqBlk.Opns
                    lopnblk = pOpnBlks.Item(lOpn.Name)
                    lopnblk.Ids.Add(lOpn, "K" & lOpn.Id)
                    lOpn.OpnBlk = lopnblk
                Next

                For Each lopnblk In pOpnBlks 'perlnd, implnd, etc
                    If lopnblk.Count > 0 Then
                        If Not pFastFlag Then
                            IPC.SendMonitorMessage("(MSG2 Setting table values for " & lopnblk.Name & ")")
                        End If
                        lopnblk.setTableValues(Msg.BlockDefs.Item(lopnblk.Name))
                    End If
                Next

                pSpecialActionBlk = New HspfSpecialActionBlk
                pSpecialActionBlk.Uci = Me
                pSpecialActionBlk.ReadUciFile()

                If Not pFastFlag Then
                    IPC.SendMonitorMessage("(MSG2 Processing Ftables)")
                End If
                ProcessFTables()

                If Not pFastFlag Then
                    IPC.SendMonitorMessage("(MSG2 Processing Connections)")
                End If
                pConnections = Nothing
                pConnections = New Collection
                lConnection = New HspfConnection 'dummy to get entry point
                lConnection.readTimSer(Me)
                lConnection = Nothing
                For Each lOpn In pOpnSeqBlk.Opns
                    lOpn.setTimSerConnections()
                Next

                pMassLinks = Nothing
                pMassLinks = New Collection
                lMassLink = New HspfMassLink
                lMassLink.readMassLinks(Me)

                'look for met segments
                If Not pFastFlag Then
                    IPC.SendMonitorMessage("(MSG2 Processing Met Segments)")
                End If
                Source2MetSeg()
                'look for point loads
                If Not pFastFlag Then
                    IPC.SendMonitorMessage("(MSG2 Processing Point Sources)")
                End If
                Source2Point()
                IPC.SendMonitorMessage("(Hide)")

            End If
        End If
        pEdited = False 'all the reads set edited
        Exit Sub
    End Sub
	
	Public Sub CalcMaxAreaByLand2Stream()
		Dim vOpTyps, vOpTyp As Object
        Dim lOpn As HspfOperation
		Dim iConn, jConn As HspfConnection
		Dim M, x As Double
		Dim i, j As Integer
		
		M = 0
		If pInitialized Then
            vOpTyps = New Object() {"RCHRES", "BMPRAC"} 'operations with contrib landuse area
			For	Each vOpTyp In vOpTyps
                For Each lOpn In pOpnBlks.Item(vOpTyp).Ids 'each operation
                    For i = 1 To lOpn.Sources.Count()
                        iConn = lOpn.Sources.Item(i)
                        x = 0
                        If iConn.Source.VolName = "PERLND" Or iConn.Source.VolName = "IMPLND" Then
                            x = iConn.MFact
                            For j = i + 1 To lOpn.Sources.Count()
                                jConn = lOpn.Sources.Item(j)
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
                Next
			Next vOpTyp
		End If
		pMaxAreaByLand2Stream = M
	End Sub
	
	Public Sub Source2MetSeg()
        Dim lOpn As HspfOperation
        Dim vOpTyp As String
		Dim lConn As HspfConnection
		Dim j As Integer
		Dim Comment As String
		Dim tMetSeg As HspfMetSeg
        Dim vMetSeg As HspfMetSeg
		Dim newSeg As Boolean
		Dim wdmid As String
        Dim idsn As Integer
        Dim isdat(6) As Integer
        Dim iedat(6) As Integer
        Static lOpTypes() As String = {"PERLND", "IMPLND", "RCHRES"}
		
        For Each vOpTyp In lOpTypes
            For Each lOpn In pOpnBlks.Item(vOpTyp).Ids
                tMetSeg = New HspfMetSeg 'init moved here
                tMetSeg.Uci = Me
                Comment = ""
                j = 1
                Do While j <= lOpn.Sources.Count()
                    lConn = lOpn.Sources.Item(j)
                    If lConn.Typ = 1 Then
                        If tMetSeg.Add(lConn) Then
                            lOpn.Sources.Remove(j)
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
                If pMetSegs.Count() > 0 Then
                    For Each vMetSeg In pMetSegs
                        If vMetSeg.Compare(tMetSeg, lOpn.Name) Then
                            newSeg = False
                            If lOpn.Name = "RCHRES" Then
                                'may need to update met seg
                                vMetSeg.UpdateMetSeg(tMetSeg)
                            End If
                            lOpn.MetSeg = vMetSeg
                            Exit For
                        End If
                    Next vMetSeg
                End If

                If newSeg Then
                    tMetSeg.Id = pMetSegs.Count() + 1
                    'get met seg name from precip data set
                    idsn = tMetSeg.MetSegRec(1).Source.VolId
                    If idsn > 0 Then
                        wdmid = tMetSeg.MetSegRec(1).Source.VolName
                        If pWdmCount > 0 Then
                            tMetSeg.ExpandMetSegName(wdmid, idsn)
                        Else
                            If Len(Comment) > 13 Then
                                tMetSeg.Name = Mid(Comment, 13)
                            Else
                                tMetSeg.Name = Comment
                            End If
                        End If
                        pMetSegs.Add(tMetSeg)
                        lOpn.MetSeg = tMetSeg
                    Else 'need in case there is no prec in the met seg
                        tMetSeg.Name = ""
                        pMetSegs.Add(tMetSeg)
                        lOpn.MetSeg = tMetSeg
                    End If
                    tMetSeg = New HspfMetSeg
                    tMetSeg.Uci = Me
                End If
            Next
        Next vOpTyp
        tMetSeg = Nothing 'get rid of last one
		
		'set any undefined mfacts to 0
		If pMetSegs.Count() > 0 Then
			For	Each vMetSeg In pMetSegs
				For j = 1 To 8
                    If vMetSeg.MetSegRec(j).MFactP = -999.0# Then
                        vMetSeg.MetSegRec(j).MFactP = 0
                    End If
                    If vMetSeg.MetSegRec(j).MFactR = -999.0# Then
                        vMetSeg.MetSegRec(j).MFactR = 0
                    End If
				Next j
			Next vMetSeg
		End If
		
	End Sub
	
	Public Sub Source2Point()
        Dim lOpn As HspfOperation
		Dim lConn As HspfConnection
		Dim j As Integer
		Dim tPoint As HspfPoint
		Dim idPoint As HspfPoint
        Dim idsn As Integer
        Dim isdat(6) As Integer
		Dim iedat(6) As Integer
        Dim lastid As Integer
		Dim wdmid As String
        Dim vOpTyp As String
		Dim newpoint As Boolean
		Dim i As Integer
        Dim RFact As Single
        Static lOpTypes() As String = {"RCHRES", "COPY"} 'operations with assoc pt srcs
		
        For Each vOpTyp In lOpTypes
            For Each lOpn In pOpnBlks.Item(vOpTyp).Ids
                j = 1
                Do While j <= lOpn.Sources.Count()
                    lConn = lOpn.Sources.Item(j)

                    If (lConn.Target.VolName = vOpTyp And lConn.Target.Group <> "EXTNL") And (Left(lConn.Source.VolName, 3) = "WDM") Then
                        'if wdm data set to rchres add to collection,
                        'or if wdm data set to copy and copy goes to rchres
                        newpoint = False
                        If lConn.Target.VolName = "COPY" Then
                            RFact = 0
                            For i = 1 To lConn.Target.Opn.Targets.Count()
                                If lConn.Target.Opn.Targets.Item(i).Target.VolName = "RCHRES" Then
                                    newpoint = True
                                    'sum up the mfacts (really for septic modeling)
                                    RFact = RFact + lConn.Target.Opn.Targets.Item(i).MFact
                                End If
                            Next i
                        ElseIf lConn.Target.VolName = "RCHRES" Then
                            newpoint = True
                        End If
                        If newpoint Then
                            If Trim(lConn.Source.VolName) = "WDM" Then
                                lConn.Source.VolName = "WDM1"
                            End If
                            tPoint = New HspfPoint
                            tPoint.MFact = lConn.MFact
                            If lConn.Target.VolName = "COPY" Then
                                'save rfact for septics
                                tPoint.RFact = RFact
                            End If
                            tPoint.Source = lConn.Source
                            tPoint.Tran = lConn.Tran
                            tPoint.Sgapstrg = lConn.Sgapstrg
                            tPoint.Ssystem = lConn.Ssystem
                            tPoint.Target = lConn.Target
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
                            Next idPoint
                            If tPoint.Id = 0 Then
                                lastid = lastid + 1
                                tPoint.Id = lastid
                            End If
                            pPointSources.Add(tPoint)
                            lOpn.PointSources.Add(tPoint)
                            lOpn.Sources.Remove(j)
                        Else
                            j = j + 1
                        End If
                    Else
                        j = j + 1
                    End If
                Loop
            Next
        Next vOpTyp
		
	End Sub
	
	Public Sub Point2Source()
        Dim lOpn As HspfOperation
		Dim lConn As HspfConnection
        Dim i As Integer
		Dim tPoint As HspfPoint
        Dim isdat(6) As Integer
		Dim iedat(6) As Integer
        Dim vOpTyp As String
        Static lOpTypes() As String = {"RCHRES", "COPY"} 'operations with assoc pt srcs
		
        For Each vOpTyp In lOpTypes
            For Each lOpn In pOpnBlks.Item(vOpTyp).Ids
                For Each tPoint In lOpn.PointSources
                    lConn = New HspfConnection
                    lConn.Uci = Me
                    If tPoint.Source.VolName = "MUTSIN" Then
                        lConn.Typ = 2
                    Else
                        lConn.Typ = 1
                    End If
                    lConn.Source = tPoint.Source
                    lConn.Ssystem = tPoint.Ssystem
                    lConn.Sgapstrg = tPoint.Sgapstrg
                    lConn.MFact = tPoint.MFact
                    lConn.Tran = tPoint.Tran
                    lConn.Target = tPoint.Target
                    'Me.Connections.Add lConn
                    lOpn.Sources.Add(lConn)
                    'now remove all point sources
                    lOpn.PointSources.Remove(1)
                Next tPoint

            Next
        Next vOpTyp
        'now remove all point sources
        Do Until pPointSources.Count() = 0
            i = pPointSources.Count()
            pPointSources.Remove((i))
        Loop

        'need to synch collection of connections with opn connections
        RemoveConnectionsFromCollection(1) 'remove all type ext src
        For Each lOpn In Me.OpnSeqBlock.Opns
            For i = 1 To lOpn.Sources.Count()
                lConn = lOpn.Sources.Item(i)
                If lConn.Typ = 1 Then
                    Me.Connections.Add(lConn)
                End If
            Next i
        Next
		
	End Sub
	
	Public Sub MetSeg2Source()
        Dim lOpn As HspfOperation
        Dim vOpTyp As String
		Dim lConn As HspfConnection
        Dim i As Integer
        Dim segRec As Integer
        Static lOpTypes() As String = {"PERLND", "IMPLND", "RCHRES"} 'operations with assoc met segs
		
        For Each vOpTyp In lOpTypes
            For Each lOpn In pOpnBlks.Item(vOpTyp).Ids
                If Not lOpn.MetSeg Is Nothing Then
                    For segRec = 1 To 8
                        With lOpn.MetSeg.MetSegRec(segRec)
                            If .typ <> 0 Then 'type exists
                                If (lOpn.Name = "RCHRES" And .MFactR > 0.0#) Or (lOpn.Name = "PERLND" And .MFactP > 0.0#) Or (lOpn.Name = "IMPLND" And .MFactP > 0.0#) Then
                                    lConn = New HspfConnection
                                    lConn.Uci = Me
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
                                        Select Case .typ
                                            Case 1 : lConn.Target.Member = "PREC"
                                            Case 2 : lConn.Target.Member = "GATMP"
                                            Case 3 : lConn.Target.Member = "DEWTMP"
                                            Case 4 : lConn.Target.Member = "WIND"
                                            Case 5 : lConn.Target.Member = "SOLRAD"
                                            Case 6 : lConn.Target.Member = "CLOUD"
                                            Case 7 : lConn.Target.Member = "PETINP"
                                            Case 8 : lConn.Target.Member = "POTEV"
                                        End Select
                                    Else
                                        lConn.MFact = .MFactP
                                        Select Case .typ
                                            Case 1 : lConn.Target.Member = "PREC"
                                            Case 2 : lConn.Target.Member = "GATMP"
                                            Case 3 : lConn.Target.Member = "DTMPG"
                                            Case 4 : lConn.Target.Member = "WINMOV"
                                            Case 5 : lConn.Target.Member = "SOLRAD"
                                            Case 6 : lConn.Target.Member = "CLOUD"
                                            Case 7 : lConn.Target.Member = "PETINP"
                                            Case 8 : lConn.Target.Member = "POTEV"
                                        End Select
                                        If .typ = 2 Then
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
                                    lOpn.Sources.Add(lConn)
                                End If
                            End If
                        End With
                    Next segRec
                End If
            Next
        Next vOpTyp
		
		'now remove all metsegs
		Do Until pMetSegs.Count() = 0
			i = pMetSegs.Count()
			pMetSegs.Remove((i))
		Loop 
		
		'need to synch collection of connections with opn connections
		RemoveConnectionsFromCollection(1) 'remove all type ext src
        For Each lOpn In Me.OpnSeqBlock.Opns
            For i = 1 To lOpn.Sources.Count()
                lConn = lOpn.Sources.Item(i)
                If lConn.Typ = 1 Then
                    Me.Connections.Add(lConn)
                End If
            Next i
        Next
	End Sub
	
	Public Sub RunUci(ByRef retcod As Integer)
        Dim p, s, M As String
        Dim ret, i As Integer
		
		'Call F90_SCNDBG(10)
		s = FilenameOnly(pName)
        p = IO.Path.GetDirectoryName(pName)
		If Len(p) > 0 Then ChDir((p))
		
		ReportMissingTimsers(ret)
		If ret = 0 Then 'user chose do anyway after timser warning
			
			i = -1 'dont interp in actscn (itll be done in simscn)
			'Call F90_ACTSCN(i, pWdmUnit(1), pMsgUnit, r, s, Len(s))
			'Call F90_SIMSCN(retcod)
			
			If Len(p) > 0 Then IPC.SendProcessMessage("HSPFUCI", "CURDIR " & p)
			IPC.SendProcessMessage("HSPFUCI", "ACTIVATE " & s & " " & i)
			M = WaitForChildMessage
			IPC.SendProcessMessage("HSPFUCI", "SIMULATE") 'calls F90_SIMSCN
			M = WaitForChildMessage
			M = WaitForChildMessage 'Activate Complete
			While CDbl(UCase(CStr(InStr(M, "PROGRESS")))) > 0
				M = WaitForChildMessage
			End While
			'Stop 'What should we be doing here exactly? Can't do GetExitCodeProcess any more.
			'ret = GetExitCodeProcess(Me.Monitor.launch.hComputeProcess, i)
			'If i <> &H103 Then
			'need to restart hspfengine
			retcod = CInt(Right(M, 2))
			'End If
			
			RestartHSPFEngine()
			'have to reset wdms, may have changed pointers during simulate
			ClearWDM()
			InitWDMArray()
			SetWDMFiles()
			
			If IsNumeric(Right(M, 1)) Then
				retcod = CInt(Right(M, 1))
			End If
			'next line fixed 10/28/03 to handle new ipc return message
			If CDbl(Right(M, 1)) <> 0 Or Left(M, 24) = "HSPFUCI exited with code" Then
				pErrorDescription = "Fatal HSPF error while running UCI file '" & s & "'." & vbCrLf & vbCrLf & "See the file '" & EchoFileName & "' for more details."
				IPC.SendMonitorMessage(pErrorDescription)
			End If
		End If
		
	End Sub
	
	Public Sub DeleteOperation(ByRef delname As String, ByRef delid As Integer)
		
		Dim j, i, nth As Integer
		Dim isource() As Integer
		Dim itarget, iscnt As Integer
        Dim lOpn As HspfOperation
        Dim lConnection As HspfConnection
		Dim lMassLink As Integer
		Dim lopnblk As HspfOpnBlk
		
		'figure out where this opn is in opn seq block and delete it
		nth = 1
        For Each lOpn In pOpnSeqBlk.Opns
            If lOpn.Name = delname And lOpn.Id = delid Then
                pOpnSeqBlk.Delete(nth)
            End If
            nth = nth + 1
        Next
		
		'need to remove from all operation type blocks
		lopnblk = pOpnBlks.Item(delname)
		If Not lopnblk.OperFromID(delid) Is Nothing Then
			lopnblk.Ids.Remove("K" & delid)
		End If
		
		'remove connections
		'need to remove connections between this and anything else
		i = 1
		iscnt = 0
		itarget = 0
		For	Each lConnection In Me.Connections
			If (lConnection.Source.VolName = delname And lConnection.Source.VolId = delid) Or (lConnection.Target.VolName = delname And lConnection.Target.VolId = delid) Then
				lMassLink = lConnection.MassLink
				If lConnection.Target.VolId = delid And lConnection.Target.VolName = delname And lConnection.Source.VolName = delname Then
					'remember the source
					iscnt = iscnt + 1
					ReDim Preserve isource(iscnt)
					isource(iscnt) = lConnection.Source.VolId
				ElseIf lConnection.Source.VolId = delid And lConnection.Source.VolName = delname And lConnection.Target.VolName = delname Then 
					'remember the target
					itarget = lConnection.Target.VolId
				End If
				Me.Connections.Remove(i)
			Else
				i = i + 1
			End If
		Next lConnection
		
		If iscnt > 0 And itarget > 0 Then
			'need to join sources and targets of this deleted opn
			For i = 1 To iscnt
				lConnection = New HspfConnection
				lConnection.Uci = Me
				lConnection.Typ = 3
				lConnection.Source.VolName = delname
				lConnection.Source.VolId = isource(i)
                lConnection.Source.Opn = pOpnBlks.Item(delname).OperFromID(isource(i))
				lConnection.MFact = 1#
				lConnection.Target.VolName = delname
				lConnection.Target.VolId = itarget
                lConnection.Target.Opn = pOpnBlks.Item(delname).OperFromID(itarget)
				If lMassLink > 0 Then
					lConnection.MassLink = lMassLink
				Else
					lConnection.MassLink = 3
				End If
				Me.Connections.Add(lConnection)
				lConnection.Source.Opn.Targets.Add(lConnection)
				lConnection.Target.Opn.Sources.Add(lConnection)
			Next i
		End If
		
		'remove this oper from source and target collections for other operations
        For Each lOpn In pOpnSeqBlk.Opns
            j = 1
            Do While j <= lOpn.Targets.Count()
                If lOpn.Targets.Item(j).Target.VolId = delid And lOpn.Targets.Item(j).Target.VolName = delname Then
                    lOpn.Targets.Remove(j)
                Else
                    j = j + 1
                End If
            Loop
            j = 1
            Do While j <= lOpn.Sources.Count()
                If lOpn.Sources.Item(j).Source.VolId = delid And lOpn.Sources.Item(j).Source.VolName = delname Then
                    lOpn.Sources.Remove(j)
                Else
                    j = j + 1
                End If
            Loop
        Next
	End Sub
	
	Public Sub OpenWDM(ByRef OpenOrCreate As Integer, ByRef fname As String, ByRef fun As Integer, ByRef wid As String)
        Dim lFile As ATCData.ATCclsTserFile
		Dim iret, Ind As Integer
		Dim FileOk As Boolean
		
		If OpenOrCreate = 2 Then
			'need to create
			fun = F90_WDBOPN(OpenOrCreate, fname, Len(fname))
			iret = F90_WDFLCL(fun)
		End If
		
		fun = 0
		AddWDMFile(fname, FileOk)
		If FileOk Then
			pWdmCount = pWdmCount + 1
			lFile = TserFiles.CurrentActive.obj
			Ind = WDMInd(wid)
			pWdmUnit(Ind) = lFile.FileUnit
			pWDMObj(Ind) = lFile
			fun = pWdmUnit(Ind)
		End If
		
	End Sub
	
	Public Sub ClearWDM()
        Dim i As Integer
		Dim M As String
		
		M = "before close in ClearWDM"
		Call F90_FILSTA(M, Len(M))
		For i = 0 To 4
			If pWdmUnit(i) <> 0 Then
				'lwdmopen = F90_WDMOPN(pWdmUnit(i), pWDMObj(i).Filename, Len(pWDMObj(i).Filename))
				'j = F90_WDFLCL(pWdmUnit(i))  'now happens in clstserwdm
				pWdmUnit(i) = 0
				'UPGRADE_NOTE: Object pWDMObj() may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
				pWDMObj(i) = Nothing
				TserFiles.Delete(TserFiles.Active.Count)
			End If
		Next i
		
		M = "after close in ClearWDM"
		Call F90_FILSTA(M, Len(M))
		'Call F90_W99OPN
	End Sub
	
	Public Sub InitWDMArray()
		Dim i As Integer
		Dim M As String
		
		pWdmCount = 0
		For i = 0 To 4
			pWdmUnit(i) = 0
		Next i
		If pMsgUnit = 0 Then 'not yet open
			Call F90_WDIINI()
			Call F90_WDBFIN()
			'IPC.SendProcessMessage "HSPFUCI", "WDIINI"
			'IPC.SendProcessMessage "HSPFUCI", "WDBFIN"
			i = 1
			pMsgUnit = F90_WDBOPN(i, pMsgWDMName, Len(pMsgWDMName))
			IPC.SendProcessMessage("HSPFUCI", "WDBOPN " & pMsgWDMName & " " & i)
			M = WaitForChildMessage
			'could be better
			pMsgUnit = CInt(Right(M, 3))
		End If
	End Sub
	
	Public Sub GetMetSegNames(ByRef fun As Integer, ByRef numMetSeg As Integer, ByRef arrayMetSegs() As String, ByRef lMetDetails() As String, ByRef lMetDescs() As String)
        Dim i, dsn, j As Integer
        Dim lLocation As String
		Dim tempsj, tempej As Double
        Dim sdat(6) As Integer
		Dim edat(6) As Integer
        Dim lts As Collection 'of atcotimser
        Dim lTser As ATCData.ATCclsTserData
        Dim ldate As ATCData.ATCclsTserDate
		Dim sj, ej As Double
		Dim llocts As Collection 'of atcotimser
		
		numMetSeg = 0
		
		'look for matching WDM datasets
		Call findtimser("OBSERVED", "", "PREC", lts)
		'return the names of the data sets from this wdm file
        For i = 1 To lts.Count()
            lTser = lts.Item(i)
            With lTser
                lLocation = .Header.Loc
                If Len(lLocation) > 0 And .File.Label = "WDM" And .File.FileUnit = fun Then
                    'first get the common dates from all timsers at this location
                    Call findtimser("OBSERVED", lLocation, "", llocts)
                    ldate = llocts.Item(1).Dates
                    sj = ldate.Summary.SJDay
                    ej = ldate.Summary.EJDay
                    For j = 2 To llocts.Count()
                        ldate = llocts.Item(j).Dates
                        tempsj = ldate.Summary.SJDay
                        tempej = ldate.Summary.EJDay
                        If tempsj > sj Then sj = tempsj
                        If tempej < ej Then ej = tempej
                    Next j
                    'now save info about this met station
                    numMetSeg = numMetSeg + 1
                    ReDim Preserve arrayMetSegs(numMetSeg)
                    arrayMetSegs(numMetSeg - 1) = lLocation
                    ReDim Preserve lMetDetails(numMetSeg)
                    dsn = .Header.id
                    Call J2Date(sj, sdat)
                    Call J2Date(ej, edat)
                    Call timcnv(edat)
                    lMetDetails(numMetSeg - 1) = CStr(dsn) & "," & CStr(sdat(0)) & "," & CStr(sdat(1)) & "," & CStr(sdat(2)) & "," & CStr(sdat(3)) & "," & CStr(sdat(4)) & "," & CStr(sdat(5)) & "," & CStr(edat(0)) & "," & CStr(edat(1)) & "," & CStr(edat(2)) & "," & CStr(edat(3)) & "," & CStr(edat(4)) & "," & CStr(edat(5))
                    ReDim Preserve lMetDescs(numMetSeg)
                    lMetDescs(numMetSeg - 1) = .Header.Desc
                End If
            End With
        Next i
		
	End Sub
	
	Public Sub AddExpertDsns(ByRef Id As Integer, ByRef clocn As String, ByRef basedsn As Integer, ByRef adsn() As Integer, ByRef ostr() As String)
        Dim wdmsfl, ndsn, j, nfrec, i As Integer
		Dim cscen As String
        Dim GenTs As ATCData.ATCclsTserData
		Dim addeddsn As Boolean
        Dim wdmid As Integer
        Dim TsDate As ATCData.ATCclsTserDate
		Dim myDateSummary As ATCData.ATTimSerDateSummary
		
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
					GenTs = GetDataSetFromDsn(wdmid, ndsn)
					If GenTs Is Nothing Then
						'found unused data set number
						nfrec = 0
						GenTs = New ATCData.ATCclsTserData
					End If
				Loop 
				
				With GenTs.Header
					.Id = ndsn
					.sen = UCase(cscen)
					.Con = UCase(ostr(j))
					.loc = UCase(clocn)
				End With
				TsDate = New ATCData.ATCclsTserDate
				With myDateSummary
					.CIntvl = True
					.ts = 1
					.Tu = 4
					.Intvl = 1
				End With
				TsDate.Summary = myDateSummary
				GenTs.Dates = TsDate
				
				GenTs.AttribSet("TSTYPE", GenTs.Header.Con)
				addeddsn = pWDMObj(wdmid).AddTimSer(GenTs, 0)
				adsn(j) = ndsn
			Next j
		Else
			'no wdm files in this uci
			Call MsgBox("No WDM Files are available with this UCI, so no calibration locations may be added", MsgBoxStyle.OKOnly, "Add Problem")
		End If
	End Sub
	
	Public Sub AddAQUATOXDsns(ByRef Id As Integer, ByRef clocn As String, ByRef basedsn As Integer, ByRef plank As Integer, ByRef gqualfg() As Integer, ByRef wdmid As Integer, ByRef Member() As String, ByRef Sub1() As Integer, ByRef Group() As String, ByRef adsn() As Integer, ByRef ostr() As String)
		
		AddAQUATOXDsnsExt(Id, clocn, basedsn, plank, gqualfg, wdmid, Member, Sub1, Group, adsn, ostr, 4)
	End Sub
	
	Public Sub AddAQUATOXDsnsExt(ByRef Id As Integer, ByRef clocn As String, ByRef basedsn As Integer, ByRef plank As Integer, ByRef gqualfg() As Integer, ByRef wdmid As Integer, ByRef Member() As String, ByRef Sub1() As Integer, ByRef Group() As String, ByRef adsn() As Integer, ByRef ostr() As String, ByRef outtu As Integer)
        Dim wdmsfl, ndsn, j, nfrec, i As Integer
		Dim cscen As String
        Dim GenTs As ATCData.ATCclsTserData
        Dim addeddsn As Boolean
		Dim lts As Collection 'of atcotimser
        Dim TsDate As ATCData.ATCclsTserDate
		Dim myDateSummary As ATCData.ATTimSerDateSummary
		Dim lOper As HspfOperation
		Dim ltable As HspfTable
		Dim wid As String
		Dim deleteddsn As Integer
		Dim vConn As Object
		Dim lConn As HspfConnection
		Dim ctmp As String
		Dim referenced As Boolean
		
		Member(1) = "VOL" : Sub1(1) = 1 : Group(1) = "HYDR" : ostr(1) = "VOL     " 'volume (ac.ft) AVER
		Member(2) = "IVOL" : Sub1(2) = 1 : Group(2) = "HYDR" : ostr(2) = "IVOL    " 'inflow (ac.ft) SUM
		Member(3) = "RO" : Sub1(3) = 1 : Group(3) = "HYDR" : ostr(3) = "RO      " 'discharge in cfs AVER
		Member(4) = "SAREA" : Sub1(4) = 1 : Group(4) = "HYDR" : ostr(4) = "SARA     " 'surface area in acres AVER
		Member(5) = "AVDEP" : Sub1(5) = 1 : Group(5) = "HYDR" : ostr(5) = "AVDP    " 'mean depth in feet AVER
		Member(6) = "PRSUPY" : Sub1(6) = 1 : Group(6) = "HYDR" : ostr(6) = "PSUP    " 'volume in from precip (ac.ft) SUM
		Member(7) = "VOLEV" : Sub1(7) = 1 : Group(7) = "HYDR" : ostr(7) = "VEVP    " 'volume out to evap (ac.ft) SUM
		Member(8) = "TW" : Sub1(8) = 1 : Group(8) = "HTRCH" : ostr(8) = "TW      " 'water temp in degrees AVER
		Member(9) = "NUIF1" : Sub1(9) = 1 : Group(9) = "NUTRX" : ostr(9) = "NO3     " 'inflow of no3 in lbs SUM
		Member(10) = "NUIF1" : Sub1(10) = 2 : Group(10) = "NUTRX" : ostr(10) = "NH3     " 'inflow of nh2 in lbs SUM
		Member(11) = "NUIF1" : Sub1(11) = 3 : Group(11) = "NUTRX" : ostr(11) = "NO2     " 'inflow of no2 in lbs SUM
		Member(12) = "NUIF1" : Sub1(12) = 4 : Group(12) = "NUTRX" : ostr(12) = "PO4     " 'inflow of po4 in lbs SUM
		Member(13) = "OXIF" : Sub1(13) = 1 : Group(13) = "OXRX" : ostr(13) = "DO      " 'inflow of do in lbs SUM
		Member(14) = "OXIF" : Sub1(14) = 2 : Group(14) = "OXRX" : ostr(14) = "BOD     " 'inflow of bod in lbs SUM
		Member(15) = "PKIF" : Sub1(15) = 5 : Group(15) = "PLANK" : ostr(15) = "ORC     " 'inflow of organic c in lbs SUM
		Member(16) = "PKIF" : Sub1(16) = 1 : Group(16) = "PLANK" : ostr(16) = "PHYT    " 'inflow of phyto in lbs SUM
		Member(17) = "ISED" : Sub1(17) = 1 : Group(17) = "SEDTRN" : ostr(17) = "ISD1    " 'inflow of sediment in tons SUM
		Member(18) = "ISED" : Sub1(18) = 2 : Group(18) = "SEDTRN" : ostr(18) = "ISD2    " 'inflow of sediment in tons SUM
		Member(19) = "ISED" : Sub1(19) = 3 : Group(19) = "SEDTRN" : ostr(19) = "ISD3    " 'inflow of sediment in tons SUM
		Member(20) = "SSED" : Sub1(20) = 1 : Group(20) = "SEDTRN" : ostr(20) = "SSD1    " 'sediment conc mg/l AVER
		Member(21) = "SSED" : Sub1(21) = 2 : Group(21) = "SEDTRN" : ostr(21) = "SSD2    " 'sediment conc mg/l AVER
		Member(22) = "SSED" : Sub1(22) = 3 : Group(22) = "SEDTRN" : ostr(22) = "SSD3    " 'sediment conc mg/l AVER
		Member(23) = "TIQAL" : Sub1(23) = 1 : Group(23) = "GQUAL" : ostr(23) = "TIQ1    " 'total inflow of qual SUM
		Member(24) = "TIQAL" : Sub1(24) = 2 : Group(24) = "GQUAL" : ostr(24) = "TIQ2    " 'total inflow of qual SUM
		Member(25) = "TIQAL" : Sub1(25) = 3 : Group(25) = "GQUAL" : ostr(25) = "TIQ3    " 'total inflow of qual SUM
		Member(26) = "NUIF2" : Sub1(26) = 4 : Group(26) = "NUTRX" : ostr(26) = "PPO4    " 'inflow of particulate po4 in lbs SUM
		Member(27) = "TPKIF" : Sub1(27) = 2 : Group(27) = "PLANK" : ostr(27) = "TORP    " 'inflow of total organic p in lbs SUM
		Member(28) = "TPKIF" : Sub1(28) = 5 : Group(28) = "PLANK" : ostr(28) = "TTP     " 'inflow of total p in lbs SUM
		
		
		If plank <> 1 Then
			ostr(15) = ""
			ostr(16) = ""
			ostr(27) = ""
			ostr(28) = ""
		End If
		
		If gqualfg(1) <> 1 Then 'if any organic chemicals
			ostr(23) = ""
		End If
		If gqualfg(2) <> 1 Then
			ostr(24) = ""
		End If
		If gqualfg(3) <> 1 Then
			ostr(25) = ""
		End If
		
		'check to see that all timsers have inputs
        lOper = pOpnBlks.Item("RCHRES").OperFromID(Id)
		If lOper.TableExists("NUT-FLAGS") Then
			ltable = lOper.Tables.Item("NUT-FLAGS")
			If ltable.Parms.Item("NH3FG").Value = 0 Then
                ostr(10) = ""
            End If
            If ltable.Parms.Item("NO2FG").Value = 0 Then
                ostr(11) = ""
            End If
            If ltable.Parms.Item("PO4FG").Value = 0 Then
                ostr(12) = ""
            End If
        Else
            ostr(10) = ""
            ostr(11) = ""
            ostr(12) = ""
            ostr(26) = ""
		End If
		If lOper.TableExists("PLNK-FLAGS") Then
			ltable = lOper.Tables.Item("PLNK-FLAGS")
            If ltable.Parms.Item("PHYFG").Value = 0 Then
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
					findtimser(UCase(Trim(cscen)), Trim(clocn), Trim(ostr(j)), lts)
					For i = 1 To lts.Count()
						GenTs = lts.Item(i)
						If GenTs.File.Label = "WDM" Then
							GetWDMIDFromUnit(GenTs.File.FileUnit, wid)
							If CShort(Right(wid, 1)) = wdmid Then
								'this is on our output wdm
								'make sure it is not referenced in this UCI already
								referenced = False
								For	Each vConn In Me.Connections
									lConn = vConn
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
									ClearWDMDataSet(wid, deleteddsn)
									DeleteWDMDataSet(wid, deleteddsn)
								End If
							End If
						End If
					Next i
					
					If deleteddsn > 0 Then
                        GenTs = Nothing
						GenTs = New ATCData.ATCclsTserData
						ndsn = deleteddsn
					Else
						'find a free dsn
						nfrec = 1
						Do While nfrec > 0
							ndsn = ndsn + 1
							'look in wdmx for this data set
							GenTs = GetDataSetFromDsn(wdmid, ndsn)
							If GenTs Is Nothing Then
								'found unused data set number
								nfrec = 0
								GenTs = New ATCData.ATCclsTserData
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
					TsDate = New ATCData.ATCclsTserDate
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
					GenTs.Dates = TsDate
					
					GenTs.AttribSet("TSTYPE", GenTs.Header.Con)
					addeddsn = pWDMObj(wdmid).AddTimSer(GenTs, 0)
					adsn(j) = ndsn
				End If
			Next j
		Else
			'no wdm files in this uci
			Call MsgBox("No WDM Files are available with this UCI, so no AQUATOX locations may be added", MsgBoxStyle.OKOnly, "Add Problem")
		End If
	End Sub
	
	Public Sub AddExpertExtTargets(ByRef reachid As Integer, ByRef copyid As Integer, ByRef ContribArea As Single, ByRef adsn() As Integer, ByRef ostr() As String)
        Dim i As Integer
		Dim MFact As Single
		Dim Tran, gap As String
		
		MFact = 12# / ContribArea
		'mfact = Format(mfact, "0.#######")
		AddExtTarget("RCHRES", reachid, "ROFLOW", "ROVOL", 1, 1, MFact, "    ", "WDM", adsn(1), ostr(1), 1, "ENGL", "AGGR", "REPL")
		
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
				
				AddExtTarget("COPY", copyid, "OUTPUT", "MEAN", i - 1, 1, MFact, Tran, "WDM", adsn(i), ostr(i), 1, "ENGL", gap, "REPL")
			Next i
		End If
		
	End Sub
	
	Public Sub AddAQUATOXExtTargets(ByRef reachid As Integer, ByRef wdmid As Integer, ByRef Member() As String, ByRef Sub1() As Integer, ByRef Group() As String, ByRef adsn() As Integer, ByRef ostr() As String)
		
		AddAQUATOXExtTargetsExt(reachid, wdmid, Member, Sub1, Group, adsn, ostr, 4)
		
	End Sub
	
	Public Sub AddAQUATOXExtTargetsExt(ByRef reachid As Integer, ByRef wdmid As Integer, ByRef Member() As String, ByRef Sub1() As Integer, ByRef Group() As String, ByRef adsn() As Integer, ByRef ostr() As String, ByRef outtu As Integer)
        Dim i, Sub2 As Integer
		Dim MFact As Single
		Dim Tran, gap As String
		
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
				AddExtTarget("RCHRES", reachid, Group(i), Member(i), Sub1(i), Sub2, MFact, Tran, "WDM" & CStr(wdmid), adsn(i), ostr(i), 1, "METR", gap, "REPL")
			End If
		Next i
		
	End Sub
	
	Public Sub AddExpertSchematic(ByRef reachid As Integer, ByRef copyid As Integer)
		'add schematic block records for expert system copy data sets
		Dim lOpn As HspfOperation
		Dim cOpns As Collection 'of hspfOperations
        Dim i As Integer
		Dim found As Boolean
		Dim pml, iml As Integer
		Dim ostr(10) As String
		Dim lConn As HspfConnection
		Dim lMassLink As HspfMassLink
		
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
		For	Each lConn In pConnections
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
				For	Each lMassLink In pMassLinks
					If lMassLink.MassLinkID = pml Then
						pml = pml + 1
						found = True
						Exit For
					End If
				Next lMassLink
			Loop 
			'now add perlnd masslink
			For i = 1 To 7
				lMassLink = New HspfMassLink
				lMassLink.Uci = Me
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
				pMassLinks.Add(lMassLink)
			Next i
		End If
		If iml = 0 Then
			'need to add implnd masslink
			iml = 91
			found = True
			Do Until found = False
				found = False
				For	Each lMassLink In pMassLinks
					If lMassLink.MassLinkID = iml Then
						iml = iml + 1
						found = True
						Exit For
					End If
				Next lMassLink
			Loop 
			'now add implnd masslink
			For i = 8 To 10
				lMassLink = New HspfMassLink
				lMassLink.Uci = Me
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
				pMassLinks.Add(lMassLink)
			Next i
		End If
		
		'add schematic records
        lOpn = pOpnBlks.Item("RCHRES").OperFromID(reachid)
		cOpns = New Collection
		Call AddCopyToSchematic(lOpn, copyid, pml, iml)
		Call FindUpstreamOpns(lOpn, cOpns)
		
		Do While cOpns.Count() > 0
			lOpn = cOpns.Item(1)
			cOpns.Remove(1)
			Call AddCopyToSchematic(lOpn, copyid, pml, iml)
			Call FindUpstreamOpns(lOpn, cOpns)
		Loop 
		
	End Sub
	
    Public Sub AddExtTarget(ByRef sname As String, ByRef sid As Integer, ByRef sgroup As String, ByRef Smember As String, ByRef Smem1 As Integer, ByRef Smem2 As Integer, ByRef MFact As Single, ByRef Tran As String, ByRef tname As String, ByRef Tid As Integer, ByRef tmember As String, ByRef Tsub1 As Integer, ByRef aSystem As String, ByRef gap As String, ByRef amd As String)

        Dim lOpn As HspfOperation
        Dim lConn As HspfConnection

        lOpn = pOpnBlks.Item(sname).OperFromID(sid)
        lConn = New HspfConnection
        lConn.Uci = Me
        lConn.Typ = 4
        lConn.Source.VolName = lOpn.Name
        lConn.Source.VolId = lOpn.Id
        lConn.Source.Group = sgroup
        lConn.Source.Member = Smember
        lConn.Source.MemSub1 = Smem1
        lConn.Source.MemSub2 = Smem2
        lConn.Source.Opn = lOpn
        lConn.MFact = MFact
        lConn.Tran = Tran
        lConn.Target.VolName = tname
        lConn.Target.VolId = Tid
        lConn.Target.Member = tmember
        lConn.Target.MemSub1 = Tsub1
        lConn.Ssystem = aSystem
        lConn.Sgapstrg = gap
        lConn.Amdstrg = amd
        pConnections.Add(lConn)
        lOpn.Targets.Add(lConn)

    End Sub
	
	Public Sub AddOutputWDMDataSet(ByRef clocn As String, ByRef ccons As String, ByRef basedsn As Integer, ByRef wdmid As Integer, ByRef adsn As Integer)
		Dim i As Integer
		
		i = 0
		AddOutputWDMDataSetExt(clocn, ccons, basedsn, i, 4, "", adsn)
		wdmid = i
	End Sub
	
	Public Sub AddOutputWDMDataSetExt(ByRef clocn As String, ByRef ccons As String, ByRef basedsn As Integer, ByRef wdmid As Integer, ByRef tunit As Integer, ByRef Desc As String, ByRef adsn As Integer)
        Dim wdmsfl, ndsn, nfrec, i As Integer
		Dim cscen As String
        Dim GenTs As ATCData.ATCclsTserData
		Dim addeddsn As Boolean
        Dim TsDate As ATCData.ATCclsTserDate
		Dim myDateSummary As ATCData.ATTimSerDateSummary
		
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
				GenTs = GetDataSetFromDsn(wdmid, ndsn)
				If GenTs Is Nothing Then
					'found unused data set number
					nfrec = 0
					GenTs = New ATCData.ATCclsTserData
				End If
			Loop 
			
			With GenTs.Header
				.Id = ndsn
				.sen = UCase(cscen)
				.Con = UCase(ccons)
				.loc = UCase(clocn)
				.Desc = Desc
			End With
			TsDate = New ATCData.ATCclsTserDate
			With myDateSummary
				.CIntvl = True
				.ts = 1
				.Tu = tunit
				.Intvl = 1
			End With
			TsDate.Summary = myDateSummary
			GenTs.Dates = TsDate
			
			GenTs.AttribSet("TSTYPE", GenTs.Header.Con)
			addeddsn = pWDMObj(wdmid).AddTimSer(GenTs, 0)
			adsn = ndsn
		End If
	End Sub
	
	Public Sub ClearWDMDataSet(ByRef wdmid As String, ByRef adsn As Integer)
		Dim wdmsfl, Id As Integer
        Dim GenTs As ATCData.ATCclsTserData
		Dim deletedsn, adddsn As Boolean
        Dim TsDate As ATCData.ATCclsTserDate
		Dim myDateSummary As ATCData.ATTimSerDateSummary
		
		If Len(wdmid) < 4 Then
			Id = 1
		Else
			Id = CShort(Mid(wdmid, 4, 1))
		End If
		wdmsfl = pWdmUnit(Id)
		Dim NewGenTs As New ATCData.ATCclsTserData
		If wdmsfl > 0 Then
			GenTs = GetDataSetFromDsn(Id, adsn)
			'save attributes
			NewGenTs.Attribs = GenTs.Attribs
			
			TsDate = New ATCData.ATCclsTserDate
			With myDateSummary
				.CIntvl = GenTs.Dates.Summary.CIntvl
				.ts = GenTs.Dates.Summary.ts
				.Tu = GenTs.Dates.Summary.Tu
				.Intvl = GenTs.Dates.Summary.Intvl
				.SJDay = GenTs.Dates.Summary.SJDay
				.EJDay = GenTs.Dates.Summary.EJDay
			End With
			TsDate.Summary = myDateSummary
			NewGenTs.Dates = TsDate
			
			NewGenTs.Header = GenTs.Header
			'delete dsn
			deletedsn = pWDMObj(Id).RemoveTimSer(GenTs)
			'add dsn
			adddsn = pWDMObj(Id).AddTimSer(NewGenTs, 0)
		End If
	End Sub
	
	Public Sub DeleteWDMDataSet(ByRef wdmid As String, ByRef adsn As Integer)
		Dim wdmsfl, Id As Integer
        Dim GenTs As ATCData.ATCclsTserData
		Dim deletedsn As Boolean
		
		If Len(wdmid) < 4 Then
			Id = 1
		Else
			Id = CShort(Mid(wdmid, 4, 1))
		End If
		wdmsfl = pWdmUnit(Id)
		If wdmsfl > 0 Then
			GenTs = GetDataSetFromDsn(Id, adsn)
			deletedsn = pWDMObj(Id).RemoveTimSer(GenTs)
		End If
	End Sub
	
	Public Sub ClearAllOutputDsns()
		Dim vConn As Object
		Dim lConn As HspfConnection
		
		For	Each vConn In pConnections
			lConn = vConn
			If lConn.Typ = 4 Then
				If Mid(lConn.Target.VolName, 1, 3) = "WDM" Then
					'clear this dsn
					ClearWDMDataSet(lConn.Target.VolName, lConn.Target.VolId)
				End If
			End If
		Next vConn
	End Sub
	
	Public Sub AddWDMFile(ByRef Name As String, ByRef FileOk As Boolean)
		Dim attr, TserIndex, i As Integer
		Dim newClsTserFile As ATCData.ATCclsTserFile
		Dim s As String
		
		FileOk = True
		TserIndex = TserFiles.AvailIndexByName("clsTSerWDM")
		If TserIndex = 0 Then 'not a valid type
			MsgBox(" AddWDMFile Error: " & TserFiles.ErrorDescription, MsgBoxStyle.Exclamation)
			FileOk = False
		Else 'a valid type, create an instance
			TserFiles.Create(TserIndex)
			'make monitor available
			newClsTserFile = TserFiles.CurrentActive.obj
			If IPCset Then newClsTserFile.Monitor = IPC
			'UPGRADE_WARNING: Couldn't resolve default property of object TserFiles.CurrentActive.obj.MsgUnit. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			TserFiles.CurrentActive.obj.MsgUnit = pMsgUnit
			
			FileOk = True
			attr = GetAttr(Name)
			If (attr And FileAttribute.ReadOnly) <> 0 Then
				'i = MsgBox("The WDM file " & Name & " is Read Only and may not be opened in that state." & vbCrLf & "Change file to not Read Only so that it may be opened?", vbExclamation + vbYesNo + vbDefaultButton1, "File Open Problem")
				'If i = vbYes Then 'change to not readonly
				attr = attr - FileAttribute.ReadOnly
				SetAttr(Name, attr)
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
					System.Windows.Forms.Application.DoEvents()
				Loop 
				If Len(s) > 0 Then 'had a problem
					MsgBox(s, MsgBoxStyle.Exclamation, "AddWDMFile Read Error on " & Name)
					TserFiles.Delete((TserFiles.CurrentActiveIndex))
					FileOk = False
				End If
			End If
		End If
	End Sub
	
	Public Sub PreScanFilesBlock(ByRef pName As String, ByRef FilesOK As Boolean, ByRef EchoFile As String)
		Dim i, Ind As Integer
		Dim tname, s, w, tpath As String
		Dim lFile As ATCData.ATCclsTserFile
		
		On Error GoTo x
		
		's = "PreScanFilesBlock entry"
		'F90_FILSTA s, Len(s)
		
		FilesOK = True
		pWdmCount = 0
		EchoFile = ""
		i = FreeFile()
		FileOpen(i, pName, OpenMode.Input)
		Do 
			s = LineInput(i)
			If Left(s, Len("FILES")) = "FILES" Then 'at files block
				While Left(s, Len("END FILES")) <> "END FILES"
					s = LineInput(i)
					If InStr(1, s, "***") = 0 Then
						If Left(s, 3) = "WDM" Then
							AddWDMFile(Mid(s, 17, Len(s) - 16), FilesOK)
							If FilesOK Then
								pWdmCount = pWdmCount + 1
								lFile = TserFiles.CurrentActive.obj
								Ind = WDMInd(Left(s, 4))
								pWdmUnit(Ind) = lFile.FileUnit
								pWDMObj(Ind) = lFile
							End If
						ElseIf Left(s, Len("END FILES")) <> "END FILES" Then  'make sure the other files are ok
							If Len(s) > 16 Then
								tname = Mid(s, 17, Len(s) - 16)
                                tpath = IO.Path.GetDirectoryName(tname)
                                If tpath.Length > 0 AndAlso Not IO.Directory.Exists(tpath) Then
                                    MsgBox("Error in Files Block:  Folder " & tpath & " does not exist.", MsgBoxStyle.OkOnly, "Open UCI Problem")
                                    FilesOK = False
                                ElseIf UCase(Right(tname, 4)) = ".MUT" Then  'does this file exist
                                    If Not IO.File.Exists(tname) Then
                                        MsgBox("Error in Files Block:  Input File " & tname & " does not exist.", MsgBoxStyle.OkOnly, "Open UCI Problem")
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
				End While
				Exit Do
			End If
		Loop 
		FileClose(i)
		's = "PreScanFilesBlock exit"
		'F90_FILSTA s, Len(s)
		System.Windows.Forms.Application.DoEvents()
		Exit Sub
x: 
        'TODO: myMsgBox.Show("Cannot open '" & Mid(s, 17, Len(s) - 16) & "' in PreScanFilesBlock." & vbCrLf & vbCrLf & "Error: " & Err.Description, "HSPF Files Error", "+-&OK")
		FilesOK = False
		FileClose(i)
	End Sub
	
	Public Sub SetWDMFiles()
		Dim Ind, i, iret As Integer
		Dim tname, s, w, tpath As String
		Dim lFile As ATCData.ATCclsTserFile
		Dim lHFile As HspfData.HspfFile
        Dim FilesOK As Boolean
		Dim ifound As Boolean
		Dim j As Integer
		Dim M As String
		'used after editing files block to open wdm files
		On Error GoTo x
		
		FilesOK = True
		
		M = "at start of SetWDMFiles"
		Call F90_FILSTA(M, Len(M))
		
		pWdmCount = 0
		For i = 1 To pFilesBlk.Count
            lHFile = pFilesBlk.Value(i)
			If Len(lHFile.Typ) > 2 Then
				If Left(lHFile.Typ, 3) = "WDM" Then
					'see if this wdm is already in project
					ifound = False
					If ifound = False And pWdmCount < 4 Then 'add it to project
						M = "just before AddWDMFile"
						Call F90_FILSTA(M, Len(M))
						AddWDMFile(lHFile.Name, FilesOK)
						M = "at end of AddWDMFile"
						Call F90_FILSTA(M, Len(M))
						If FilesOK Then
							lFile = TserFiles.CurrentActive.obj
							s = lHFile.Typ
							Ind = WDMInd(Left(s, 4))
							pWdmUnit(Ind) = lFile.FileUnit
							pWDMObj(Ind) = lFile
							pWdmCount = pWdmCount + 1
						Else
							MsgBox("Error in SetWDMFiles")
						End If
					End If
				End If
			End If
		Next i
		Exit Sub
x: 
		MsgBox("Error " & Err.Description & " in SetWDMFiles")
		FilesOK = False
	End Sub
	
    'TODO: use new code for WDM
    Public Function GetWDMAttr(ByRef wdmid As String, ByRef idsn As Integer, ByRef attr As String) As String
        Dim s As String
        Dim dsnObj As ATCData.ATCclsTserData

        dsnObj = GetDataSetFromDsn(WDMInd(wdmid), idsn)
        If Not (dsnObj Is Nothing) And attr = "LOC" Then
            s = dsnObj.Header.Loc
        ElseIf Not (dsnObj Is Nothing) And attr = "CON" Then
            s = dsnObj.Header.Con
        ElseIf Not (dsnObj Is Nothing) And attr = "DESC" Then
            s = dsnObj.Header.Desc
        Else
            s = ""
        End If
        GetWDMAttr = s
    End Function
	
    'TODO: use new code for WDM
    Public Function GetDataSetFromDsn(ByRef lWdmInd As Integer, ByRef lDsn As Integer) As ATCData.ATCclsTserData
        Dim i As Integer
        Dim lWdmObj As ATCData.ATCclsTserFile

        lWdmObj = pWDMObj(lWdmInd)
        With lWdmObj
            For i = 1 To .DataCount
                If lDsn = .Data(i).Header.id Then
                    GetDataSetFromDsn = .Data(i)
                    Exit Function
                End If
            Next i
            GetDataSetFromDsn = Nothing
            'MsgBox "DSN " & lDsn & " does not exist.", vbOKOnly
        End With
    End Function
	
	Public Function GetWDMObj(ByRef Index As Integer) As ATCData.ATCclsTserFile
		If Not pWDMObj(Index) Is Nothing Then
			GetWDMObj = pWDMObj(Index)
		Else
			'UPGRADE_NOTE: Object GetWDMObj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
			GetWDMObj = Nothing
		End If
	End Function
	
	Public Function GetWDMIdFromName(ByRef Name As String) As String
        GetWDMIdFromName = "WDM"
        For i As Integer = 1 To 4
            If Not pWDMObj(i) Is Nothing Then
                If pWDMObj(i).FileName = Name Then
                    GetWDMIdFromName = "WDM" & i
                End If
            End If
        Next
	End Function
	
    Public Sub findtimser(ByRef sen As String, ByRef aLocation As String, ByRef Con As String, ByRef lts As Collection)
        Dim j As Integer
        lts = Nothing
        lts = New Collection
        For Each lTserFile As ATCData.ATCclsTserFile In TserFiles.Active
            For j = 1 To lTserFile.DataCount
                If (sen = lTserFile.Data(j).Header.Sen Or Len(Trim(sen)) = 0) And (aLocation = lTserFile.Data(j).Header.Loc Or Len(Trim(aLocation)) = 0) And (Con = lTserFile.Data(j).Header.Con Or Len(Trim(Con)) = 0) Then 'need this timser
                    lts.Add(lTserFile.Data(j))
                End If
            Next j
        Next
    End Sub
	
	Public Sub EditActivityAll()
		editActivityAllInit(Me, (Me.icon))
	End Sub
	
	Public Function UpstreamArea(ByRef RCHId As Integer) As Single
        Dim lOpn As HspfOperation
        Dim cOpns As Collection 'of hspfOperations
        Dim totalarea As Single

        totalarea = 0
        lOpn = pOpnBlks.Item("RCHRES").OperFromID(RCHId)
        cOpns = New Collection
        totalarea = totalarea + LocalUpstreamArea(lOpn)
        Call FindUpstreamOpns(lOpn, cOpns)

        Do While cOpns.Count() > 0
            lOpn = cOpns.Item(1)
            cOpns.Remove(1)
            totalarea = totalarea + LocalUpstreamArea(lOpn)
            Call FindUpstreamOpns(lOpn, cOpns)
        Loop
        UpstreamArea = totalarea

    End Function
	
	Private Function LocalUpstreamArea(ByRef lOpn As HspfOperation) As Single
		
		Dim iConn As HspfConnection
        Dim i As Integer
		Dim uparea As Single
		
		uparea = 0
		For i = 1 To lOpn.Sources.Count()
			iConn = lOpn.Sources.Item(i)
			If iConn.Source.VolName = "PERLND" Or iConn.Source.VolName = "IMPLND" Then
				uparea = uparea + iConn.MFact
			End If
		Next i
		LocalUpstreamArea = uparea
		
	End Function
	
	Private Sub FindUpstreamOpns(ByRef lOpn As HspfOperation, ByRef cOpns As Collection)
		
		Dim iConn As HspfConnection
        Dim i As Integer
		Dim tOpn As HspfOperation
		
		For i = 1 To lOpn.Sources.Count()
			iConn = lOpn.Sources.Item(i)
			If iConn.Source.VolName = "RCHRES" Or iConn.Source.VolName = "BMPRAC" Then
				'add the source operation to the collection
				tOpn = iConn.Source.Opn
				cOpns.Add(tOpn)
			End If
		Next i
		
	End Sub
	
	Private Sub AddCopyToSchematic(ByRef lOpn As HspfOperation, ByRef copyid As Integer, ByRef pml As Integer, ByRef iml As Integer)
		'adds the copy record to the schematic block for each local land segment
		'contributing to this operation
		
		Dim iConn, jConn As HspfConnection
		Dim lConn As HspfConnection
		Dim j, i, found As Integer
		Dim copyOpn As HspfOperation
		
		For i = 1 To lOpn.Sources.Count()
			iConn = lOpn.Sources.Item(i)
			If iConn.Source.VolName = "PERLND" Or iConn.Source.VolName = "IMPLND" Then
				'copy this record
				'does this oper to copy already exist?
                copyOpn = pOpnBlks.Item("COPY").OperFromID(copyid)
				found = 0
				For j = 1 To copyOpn.Sources.Count()
					jConn = copyOpn.Sources.Item(j)
					If jConn.Source.VolName = iConn.Source.VolName And jConn.Source.VolId = iConn.Source.VolId Then
						found = j
					End If
				Next j
				If found > 0 Then
					jConn = copyOpn.Sources.Item(found)
					jConn.MFact = jConn.MFact + iConn.MFact
				Else 'does not already exist
					lConn = New HspfConnection
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
					pConnections.Add(lConn)
					iConn.Source.Opn.Targets.Add(lConn)
                    copyOpn = pOpnBlks.Item("COPY").OperFromID(copyid)
					copyOpn.Sources.Add(lConn)
				End If
			End If
		Next i
		
	End Sub
	Public Sub AddOperation(ByRef opname As String, ByRef opid As Integer)
		'add an operation/oper id (ie copy 100) to the uci object
        Dim lOpn As HspfOperation
        Dim lopnblk As HspfOpnBlk
        Dim inuse, freeid As Integer
		
		lopnblk = pOpnBlks.Item(opname)
		
		If lopnblk.Count > 0 Then
			'already have some of this operation, make sure this id is not in use
			freeid = 0
			Do Until freeid > 0
				inuse = 0
				For	Each lOpn In lopnblk.Ids
					If lOpn.Id = opid Then
						'in use
						inuse = opid
						opid = opid + 1
					End If
				Next lOpn
				If inuse = 0 Then freeid = opid
			Loop 
		End If
		
		lOpn = New HspfOperation
		lOpn.Name = opname
		lOpn.Id = opid
		lOpn.Uci = Me
		
		lopnblk.Ids.Add(lOpn, "K" & lOpn.Id)
		lOpn.OpnBlk = lopnblk
		
	End Sub
	
	Public Sub AddTable(ByRef opname As String, ByRef opid As Integer, ByRef tabname As String)
		'create a new table, or add this operation id to the current table
		Dim lopnblk As HspfOpnBlk
		
		lopnblk = pOpnBlks.Item(opname)
		If lopnblk.Count > 0 Then
			'this operation block exists, okay to add table
            Call lopnblk.AddTable(opid, tabname, pMsg.BlockDefs.Item(opname))
		End If
		
	End Sub
	
	Public Sub RemoveTable(ByRef opname As String, ByRef opid As Integer, ByRef tabname As String)
		'remove this operation id from the current table, remove whole table
		'if this is the only operation in the table
		Dim lopnblk As HspfOpnBlk
		
		lopnblk = pOpnBlks.Item(opname)
		If lopnblk.Count > 0 Then
			'this operation block exists, okay to remove table
			Call lopnblk.RemoveTable(opid, tabname)
		End If
		
	End Sub
	
	Private Sub newOutputDsns(ByRef oldn As String, ByRef newn As String, ByRef basedsn As Integer, ByRef relabs As Integer)
		'build new output dsns on saveas
        Dim lts As Collection 'of atcotimser
        Dim lTimser As ATCData.ATCclsTserData
		Dim addeddsn, Update As Boolean
		Dim nfrec, wdmid, wdmsfl, i, ndsn As Integer
		Dim cwdm, tstype As String
        Dim GenTs As ATCData.ATCclsTserData
        Dim TsDate As ATCData.ATCclsTserDate
		Dim lConn As HspfConnection
		Dim vConn As Object
		Dim myDateSummary As ATCData.ATTimSerDateSummary
		
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
			For i = 1 To lts.Count()
                lTimser = lts.Item(i)
				'find a free dsn
				nfrec = 1
				If relabs = 1 Then
                    ndsn = lTimser.Header.id + basedsn - 1
				Else
					If ndsn = 0 Then
						ndsn = basedsn - 1
					End If
				End If
				Do While nfrec > 0
					ndsn = ndsn + 1
					'look in wdmx for this data set
					GenTs = GetDataSetFromDsn(wdmid, ndsn)
					If GenTs Is Nothing Then
						'found unused data set number
						nfrec = 0
						GenTs = New ATCData.ATCclsTserData
					End If
				Loop 
				
				'set attribs to the old version
				With GenTs.Header
					.Id = ndsn
					.sen = newn
                    .Con = lTimser.Header.Con
                    .Loc = lTimser.Header.Loc
                    .Desc = lTimser.Header.Desc
				End With
				TsDate = New ATCData.ATCclsTserDate
				With myDateSummary
                    .CIntvl = lTimser.Dates.Summary.CIntvl
                    .ts = lTimser.Dates.Summary.ts
                    .Tu = lTimser.Dates.Summary.Tu
                    .Intvl = lTimser.Dates.Summary.Intvl
				End With
				TsDate.Summary = myDateSummary
				GenTs.Dates = TsDate
				
				'now add the timser
                addeddsn = AddWDMDataSet(wdmid, ndsn, newn, lTimser.Header.Loc, lTimser.Header.Con, lTimser.Dates.Summary.Tu, lTimser.Dates.Summary.ts, lTimser.Header.Desc)
				
				'update tstype attribute
				GenTs = Me.GetDataSetFromDsn(wdmid, ndsn)
				If Not GenTs Is Nothing Then
                    tstype = lTimser.Attrib("TSTYPE")
					GenTs.AttribSet("TSTYPE", tstype)
					Update = GenTs.File.WriteDataHeader(GenTs)
				End If
				
				'change the appropriate ext targets record
				cwdm = "WDM" & CStr(wdmid)
				For	Each vConn In pConnections
					lConn = vConn
					If lConn.Typ = 4 Then
                        If (Trim(lConn.Target.VolName) = cwdm Or (Trim(lConn.Target.VolName) = "WDM" And wdmid = 1)) And lConn.Target.VolId = lTimser.Header.id Then
                            'found the old dsn in the ext targets, change it
                            lConn.Target.VolId = ndsn
                        End If
					End If
				Next vConn
			Next i
			'Me.GetWDMObj(wdmid).Refresh    'Not necessary
		End If
	End Sub
	
	Public Function AddWDMDataSet(ByRef wdmid As Integer, ByRef dsn As Integer, ByRef scen As String, ByRef locn As String, ByRef cons As String, ByRef Tu As Integer, ByRef ts As Integer, Optional ByRef Desc As String = "") As Boolean
        Dim TsDate As ATCData.ATCclsTserDate
		Dim GenTs As New ATCData.ATCclsTserData
		Dim myDateSummary As ATCData.ATTimSerDateSummary
		
		With GenTs.Header
			.Id = dsn
			.sen = UCase(scen)
			.Con = UCase(cons)
			.loc = UCase(locn)
			If Len(Desc) > 0 Then
				.Desc = UCase(Desc)
			End If
		End With
		TsDate = New ATCData.ATCclsTserDate
		With myDateSummary
			.CIntvl = True
			.ts = ts
			.Tu = Tu
			.Intvl = 1
		End With
		TsDate.Summary = myDateSummary
		GenTs.Dates = TsDate
		GenTs.AttribSet("TSTYPE", GenTs.Header.Con)
		AddWDMDataSet = pWDMObj(wdmid).AddTimSer(GenTs, 0)
		
	End Function
	
    Public Sub AddPointSourceDataSet(ByRef sen As String, ByRef aLocation As String, ByRef Con As String, ByRef stanam As String, ByRef tstype As String, ByRef ndates As Integer, ByRef jdates() As Single, ByRef Load() As Single, ByRef newwdmid As String, ByRef newdsn As Integer)

        Dim wdmsfl, ndsn, nfrec, i As Integer
        Dim GenTs As ATCData.ATCclsTserData
        Dim addeddsn As Boolean
        Dim SDate(6) As Integer
        Dim EDate(6) As Integer
        Dim wdmid, nsteps As Integer
        Dim aval() As Single
        Dim TsDate As ATCData.ATCclsTserDate
        Dim curdate As Single
        Dim ival As Integer
        Dim myDateSummary As ATCData.ATTimSerDateSummary

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
                GenTs = GetDataSetFromDsn(wdmid, ndsn)
                If GenTs Is Nothing Then
                    'found unused data set number
                    nfrec = 0
                    newdsn = ndsn
                    newwdmid = "WDM" & CStr(wdmid)
                    GenTs = New ATCData.ATCclsTserData
                End If
            Loop

            With GenTs.Header
                .id = ndsn
                .Sen = UCase(sen)
                .Con = UCase(Con)
                .Loc = UCase(aLocation)
                .Desc = stanam
            End With
            TsDate = New ATCData.ATCclsTserDate
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
                If ndates = 0 Or ndates = 1 Then 'use this value for all
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
                    For i = 0 To nsteps 'use this value for all
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
            TsDate.Summary = myDateSummary

            GenTs.Dates = TsDate
            GenTs.Values = VB6.CopyArray(aval)
            GenTs.AttribSet("TSTYPE", tstype)

            addeddsn = pWDMObj(wdmid).AddTimSer(GenTs, 0)
        End If
    End Sub
	
	Public Sub AddPoint(ByRef wdmid As String, ByRef wdmdsn As Integer, ByRef tarid As Integer, ByRef srcname As String, ByRef targroup As String, ByRef tarmember As String, ByRef Sub1 As Integer, ByRef Sub2 As Integer)
		Dim lOpn As HspfOperation
		Dim tPoint As HspfPoint
		Dim idPoint As HspfPoint
		Dim Tu, lastid, runts As Integer
        Dim dsnObj As ATCData.ATCclsTserData
		
        lOpn = pOpnBlks.Item("RCHRES").OperFromID(tarid)
		dsnObj = Me.GetDataSetFromDsn(WDMInd(wdmid), wdmdsn)
		
		tPoint = New HspfPoint
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
		If tPoint.Source.Member = "Flow" Or tPoint.Source.Member = "FLOW" Or tPoint.Source.Member = "flow" Then
			'mfactor needs to convert cfs to ac-ft/interval
			tPoint.MFact = 0.0826
			tPoint.Tran = "SAME"
		Else
			'not flow, so assume pounds per day
			runts = 3
			If Me.OpnSeqBlock.Delt = 1440 Then
				runts = 4
			End If
			If Tu > runts Then 'daily pt src in hourly run, for example
				tPoint.Tran = "DIV"
			ElseIf Tu = runts Then  'hourly in hourly run, for example
				tPoint.Tran = "SAME"
			ElseIf Tu < runts Then  'hourly pt src in daily run, for example
				tPoint.Tran = "SUM"
			End If
		End If
		tPoint.Sgapstrg = ""
		tPoint.Ssystem = "ENGL"
		tPoint.Target.Opn = lOpn
		tPoint.Target.VolName = "RCHRES"
		tPoint.Target.VolId = tarid
		tPoint.Target.Group = targroup
		tPoint.Target.Member = tarmember
		tPoint.Target.MemSub1 = Sub1
		tPoint.Target.MemSub2 = Sub2
		tPoint.Name = srcname
		
		For	Each idPoint In pPointSources
			If idPoint.Name = tPoint.Name And idPoint.Target.VolId = tarid Then
				'use same id as an existing one
				tPoint.Id = idPoint.Id
				Exit For
			End If
		Next idPoint
		If tPoint.Id = 0 Then
			lastid = 1
			For	Each idPoint In pPointSources
				If idPoint.Id >= lastid Then
					lastid = idPoint.Id + 1
				End If
			Next idPoint
			'this is the id for the new one
			tPoint.Id = lastid
		End If
		pPointSources.Add(tPoint)
		lOpn.PointSources.Add(tPoint)
	End Sub
	
	Public Sub RemovePoint(ByRef wdmid As String, ByRef wdmdsn As Integer, ByRef tarid As Integer)
		Dim lOpn As HspfOperation
		Dim idPoint As HspfPoint
		Dim irem As Integer
		
        lOpn = pOpnBlks.Item("RCHRES").OperFromID(tarid)
		
		irem = 0
		For	Each idPoint In pPointSources
			irem = irem + 1
			If idPoint.Source.VolName = wdmid And idPoint.Source.VolId = wdmdsn And idPoint.Target.VolId = tarid Then
				'remove this one
				Exit For
			End If
		Next idPoint
		pPointSources.Remove(irem)
		
		irem = 0
		For	Each idPoint In lOpn.PointSources
			irem = irem + 1
			If idPoint.Source.VolName = wdmid And idPoint.Source.VolId = wdmdsn And idPoint.Target.VolId = tarid Then
				'remove this one
				Exit For
			End If
		Next idPoint
		lOpn.PointSources.Remove(irem)
		
	End Sub
	
	Public Sub GetWDMUnits(ByRef nwdm As Integer, ByRef aunits() As Integer)
		Dim i As Integer
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
	
	Public Sub GetWDMIDFromUnit(ByRef nunit As Integer, ByRef Id As String)
		Dim i As Integer
		
		Id = ""
		For i = 1 To 4
			If pWdmUnit(i) > 0 Then
				If pWdmUnit(i) = nunit Then
					Id = "WDM" & CStr(i)
				End If
			End If
		Next i
	End Sub
	
	Public Sub RemoveConnectionsFromCollection(ByRef itype As Integer)
		Dim i As Integer
		Dim lConn As HspfConnection
		
		i = 1
		Do While i <= Me.Connections.Count()
			'remove this type of connections from pconnections collection
			lConn = Me.Connections.Item(i)
			If lConn.Typ = itype Then
				Me.Connections.Remove(i)
			Else
				i = i + 1
			End If
		Loop 
	End Sub
	
	Public Function Copy() As HspfUci
		Dim lUCI As HspfUci
		lUCI = New HspfUci
		
		lUCI.Name = Me.Name
		
		Copy = lUCI
	End Function
	
	Public Function WaitForChildMessage() As String
		'Dim pipeBuffer$, res&, ExitCode&
		Dim s As String
		
		Do  'process messages from parent
			'res = GetExitCodeProcess(pHspfEngine.launch.hComputeProcess, ExitCode)
			'If ExitCode = &H103 Then 'back end STILL_ACTIVE
			s = IPC.GetProcessMessage("HSPFUCI") 'pHspfEngine.ReadTokenFromPipe(IPC.ParentRead, pipeBuffer, False)
			'Else
			'  s = "Child has exited"
			'End If
			If Len(s) > 3 Then
				Select Case (LCase(Left(s, 3)))
					Case "dbg", "msg" ', "com", "act"
						IPC.SendMonitorMessage(s)
						s = ""
				End Select
			End If
		Loop While Len(s) = 0
		WaitForChildMessage = s
		
	End Function
	
	Public Function EchoFileName() As String
		Dim i As Integer
		
		EchoFileName = ""
		For i = 1 To pFilesBlk.Count
			If pFilesBlk.Value(i).Typ = "MESSU" Then
				EchoFileName = pFilesBlk.Value(i).Name
			End If
		Next i
	End Function
	
	Private Sub ReportMissingTimsers(ByRef retcod As Integer)
		Dim lOpn As HspfOperation
		Dim coll As Collection
        Dim i, iresp As Integer
		Dim ctxt As String
		
		If Me.MetSegs.Count() > 0 Then
			MetSeg2Source()
		End If
		Point2Source()
		
		ctxt = ""
		'UPGRADE_NOTE: Object coll may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		coll = Nothing
        For Each lOpn In pOpnSeqBlk.Opns
            coll = Nothing
            'lOpn.InputTimeseriesStatus.Update
            coll = lOpn.InputTimeseriesStatus.GetInfo(HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired, HspfStatus.HspfStatusPresentMissingEnum.HspfStatusMissing)
            If coll.Count() > 0 Then
                For i = 1 To coll.Count()
                    ctxt = ctxt & vbCrLf & lOpn.Name & " " & lOpn.Id & " " & coll.Item(i).Name
                Next i
            End If
        Next
		Source2MetSeg()
		Source2Point()
		retcod = 0
		If Len(ctxt) > 0 Then
			'some missing timsers
            'TODO: iresp = myMsgBox.Show("WinHSPF has detected missing input time series" & vbCrLf & "required for the selected simulation options:" & vbCrLf & ctxt & vbCrLf & vbCrLf & "Do you want to try running HSPF anyway?", "WinHSPF Simulate Problem", "&OK", "+-&Cancel")
			retcod = iresp - 1
		End If
	End Sub
	
	Public Sub PollutantsBuild()
		modPollutantsBuild(Me, pMsg)
	End Sub
	
	Public Sub PollutantsUnBuild()
		modPollutantsUnBuild(Me, pMsg)
	End Sub
	
	Private Sub ProcessFTables()
		Dim retcod, OmCode, init, retkey, rectyp As Integer
		Dim cbuff As String
		Dim done As Boolean
		Dim j, i, Id As Integer
		Dim lOpn, tOpn As HspfOperation

		OmCode = HspfOmCode("FTABLES")
		init = 1
		done = False
		retkey = -1
		Do Until done
			If Me.FastFlag Then
				GetNextRecordFromBlock("FTABLES", retkey, cbuff, rectyp, retcod)
			Else
				Call REM_XBLOCK(Me, OmCode, init, retkey, cbuff, retcod)
			End If
			init = 0
			If Mid(cbuff, 3, 6) = "FTABLE" Then 'this is a new one
				Id = CShort(Right(Trim(cbuff), 3))
				'find which oper this ftable is associated with
                For Each tOpn In Me.OpnBlks.Item("RCHRES").Ids
                    If tOpn.Tables.Item("HYDR-PARM2").Parms("FTBUCI") = Id Then
                        lOpn = tOpn
                        Exit For
                    End If
                Next
				If Not lOpn Is Nothing Then
					If Me.FastFlag Then
						rectyp = -999
						Do Until rectyp = 0
							GetNextRecordFromBlock("FTABLES", retkey, cbuff, rectyp, retcod)
						Loop 
					Else
						Call REM_XBLOCK(Me, OmCode, init, retkey, cbuff, retcod)
					End If
					With lOpn.FTable
						.Nrows = CInt(Left(cbuff, 5))
						.Ncols = CInt(Mid(cbuff, 6, 5))
						i = 1
						Do While i <= .Nrows
							If Me.FastFlag Then
								GetNextRecordFromBlock("FTABLES", retkey, cbuff, rectyp, retcod)
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
								.Depth(i) = CDbl(Left(cbuff, 10))
								.DepthAsRead(i) = Left(cbuff, 10)
								.Area(i) = CDbl(Mid(cbuff, 11, 10))
								.AreaAsRead(i) = Mid(cbuff, 11, 10)
								.Volume(i) = CDbl(Mid(cbuff, 21, 10))
								.VolumeAsRead(i) = Mid(cbuff, 21, 10)
								j = .Ncols - 3
								If j > 0 Then
									.Outflow1(i) = CDbl(Mid(cbuff, 31, 10))
									.Outflow1AsRead(i) = Mid(cbuff, 31, 10)
								End If
								If j > 1 Then
									.Outflow2(i) = CDbl(Mid(cbuff, 41, 10))
									.Outflow2AsRead(i) = Mid(cbuff, 41, 10)
								End If
								If j > 2 Then
									.Outflow3(i) = CDbl(Mid(cbuff, 51, 10))
									.Outflow3AsRead(i) = Mid(cbuff, 51, 10)
								End If
								If j > 3 Then
									.Outflow4(i) = CDbl(Mid(cbuff, 61, 10))
									.Outflow4AsRead(i) = Mid(cbuff, 61, 10)
								End If
								If j > 4 Then
									.Outflow5(i) = CDbl(Mid(cbuff, 71, 10))
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
			End If
		Loop 
		
	End Sub
	
	Private Sub RestartHSPFEngine()
		Dim s, HSPFEngineExe As String
		Dim ff As New ATCoCtl.ATCoFindFile
		
		ff.SetDialogProperties("Please locate HSPFEngine.exe", "HSPFEngine.exe")
		ff.SetRegistryInfo("HSPFEngine", "files", "HSPFEngine.exe")
		HSPFEngineExe = ff.GetName
		
		IPC.ExitProcess("HSPFUCI")
        IPC.StartProcess("HSPFUCI", HSPFEngineExe & " " & GetCurrentProcessId)
		IPC.SendProcessMessage("HSPFUCI", "W99OPN")
		IPC.SendProcessMessage("HSPFUCI", "WDBFIN")
		IPC.SendProcessMessage("HSPFUCI", "PUTOLV 10")
		IPC.SendProcessMessage("HSPFUCI", "SPIPH " & CStr(IPC.hPipeReadFromParent("HSPFUCI")) & " " & CStr(IPC.hPipeWriteToParent("HSPFUCI")) & " ")
		IPC.SendProcessMessage("HSPFUCI", "WDBOPN " & pMsgWDMName & " 1")
		s = WaitForChildMessage
	End Sub
	
	Public Function CatAsInt(ByRef s As String) As Integer
		'turn a two character category tag into its integer equivalent
		Dim i As Integer
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
	
	Public Function IntAsCat(ByRef Member As String, ByRef sub1or2 As Integer, ByRef sint As String) As String
		'given a timeseries member name and a subscript, see if there is a
		'category equivalent.  if so, turn the integer category tag into its
		'two character equivalent
        Dim i As Integer
        IntAsCat = ""
		If Not Me.CategoryBlock Is Nothing Then
			IntAsCat = sint
			If IsNumeric(sint) Then
				i = CShort(sint)
				If Me.CategoryBlock.Count > 0 And Me.CategoryBlock.Count >= i Then
					'have category block
					'check to see if this one is valid to convert into a category tag
					If Member = "COTDGT" And sub1or2 = 2 Or Member = "CIVOL" And sub1or2 = 1 Or Member = "CVOL" And sub1or2 = 1 Or Member = "CRO" And sub1or2 = 1 Or Member = "CO" And sub1or2 = 2 Or Member = "CDFVOL" And sub1or2 = 2 Or Member = "CROVOL" And sub1or2 = 1 Or Member = "COVOL" And sub1or2 = 2 Then
						IntAsCat = Me.CategoryBlock.Value(i).Tag
					End If
				End If
			End If
		End If
	End Function
End Class