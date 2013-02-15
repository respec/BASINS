Option Strict Off
Option Explicit On
Imports MapWinUtility
Imports atcUtility

Module HSPFmsg

    'Public ToStatus As StatusMonitor
    Friend g_Pause As Boolean = False 'True when user has pressed Pause button on Status Monitor, False again when user has pressed Run
    Friend g_Cancel As Boolean = False 'True when user has pressed Cancel button on Status Monitor

	'UPGRADE_WARNING: Application will terminate when Sub Main() finishes. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="E08DDC71-66BA-424F-A612-80AF11498FF8"'
	Public Sub Main()
		Dim o As Object
		Dim myTableAliasDefn As Object
		Dim igroup As Object
		Dim k As Object
		Dim istart As Object
		Dim tempbuff As Object
		Dim j As Object
		Dim myparmrec As Object
		Dim mytableRec As Object
		
		Dim hscn, hmsg, hwdm, dbname As String
		Dim i, hdle As Integer
		Dim s As New VB6.FixedLengthString(80)
		Dim opnid, contfg, id, Init, kflg, retid, offset As Integer
		Dim kwd As String
		Dim tabno, uunits As Integer
		Dim lnflds As Integer
		Dim lscol(30) As Integer
		Dim lflen(30) As Integer
		Dim lftyp As String
		Dim lapos(30) As Integer
		Dim limin(30) As Integer
		Dim limax(30) As Integer
		Dim lidef(30) As Integer
		Dim lrmin(30) As Single
		Dim lrmax(30) As Single
		Dim lrdef(30) As Single
		Dim limetmin(30) As Integer
		Dim limetmax(30) As Integer
		Dim limetdef(30) As Integer
		Dim lrmetmin(30) As Single
		Dim lrmetmax(30) As Single
		Dim lrmetdef(30) As Single
		Dim lnmhdr As Integer
		Dim hdrbuf(10) As String
		Dim lfdnam(30) As String
		Dim ParmTableID, ParmTableType As Integer
		Dim Assoc, snam, crit As String
		Dim AssocID As Integer
		Dim olen, gnum, initfg, cont As Integer
		Dim obuff As String
		Dim Occur As Integer
		Dim SubsKeyName, AppearName, Name, IDVarName, OName As String
		Dim opcnt, SCLU, SGRP, gptr As Integer
		Dim fptr(64) As Integer
		Dim lheader As String
		Dim tabnam() As String
		Dim omcode() As Integer
		Dim tabcnt As Integer
		Dim tabret As Integer
		Dim adjLen As Integer
		Dim isect As Integer
		Dim irept As Integer
		Dim groupbase() As Integer
		Dim ngroup As Object
		Dim ilen, itype As Integer
		Dim desc As String
		Dim rmax, rmin, rdef As Single
		Dim hpos, hlen, hrec, vlen As Integer
		Dim svalid As String
		'UPGRADE_WARNING: Arrays in structure myQRS may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim myQRS As DAO.Recordset
		Dim BlockID As Integer
		Dim lnmhdrM As Integer
		Dim hdrbufM(10) As String
        Dim hspfinfofolder As String = "c:\vb6\hspfinfo"
        Dim pStatusMonitor As MonitorProgressStatus

        'Start status monitor
        'ToStatus = New StatusMonitor
        'AddHandler ToStatus.MonitorButtonPressed, AddressOf MonitorButtonPressed
        'Logger.ProgressStatus = ToStatus
        logger.Status("Hide")
        Logger.Status("PROGRESS TIME ON")

        Logger.StartToFile(hspfinfofolder & g_PathChar _
                 & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-hspfinfo.log")
        If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            pStatusMonitor = New MonitorProgressStatus
            If pStatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"), _
                                            hspfinfofolder & g_PathChar, _
                                            System.Diagnostics.Process.GetCurrentProcess.Id) Then
                'put our status monitor (StatusMonitor.exe) between the Logger and the default MW status monitor
                'pStatusMonitor.InnerProgressStatus = Logger.ProgressStatus
                Logger.ProgressStatus = pStatusMonitor
                Logger.Status("LABEL TITLE HSPF Info Status")
                Logger.Status("PROGRESS TIME ON") 'Enable time-to-completion estimation
                Logger.Status("")
            Else
                pStatusMonitor.StopMonitor()
                pStatusMonitor = Nothing
            End If
        End If

		Call F90_W99OPN() 'open error file
		Call F90_WDBFIN() 'initialize WDM record buffer
        Call F90_PUTOLV(10)

        ChDir(hspfinfofolder)
		
        hmsg = "c:\lib3.0\hspfmsg.wdm"
		i = 1
		fmsg = F90_WDBOPN(i, hmsg, Len(hmsg))
		Call F90_MSGUNIT(fmsg)
		
		'build the access database
		Call BuildHspMsgDB()
		myRec = myDb.OpenRecordset("BlockDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
		
		'pull info from hspfmsg.wdm and populate access database
		initfg = 1
		cont = 1
		SCLU = 201
		SGRP = 22
		opcnt = 0
		tabcnt = 0
		Do While cont <> 0
			'get each block and its id number
			retid = 0
			olen = 80
			Call F90_WMSGTT(fmsg, SCLU, SGRP, initfg, olen, cont, obuff)
			tabcnt = tabcnt + 1
			ReDim Preserve tabnam(tabcnt)
			ReDim Preserve omcode(tabcnt)
			tabnam(tabcnt - 1) = Mid(obuff, 1, 12)
			omcode(tabcnt - 1) = CInt(Mid(obuff, 18, 3))
			If omcode(tabcnt - 1) = 100 Then
				opcnt = opcnt + 1
				omcode(tabcnt - 1) = 120 + opcnt
			End If
			With myRec
				'add each block name to the block definition table
				.AddNew()
				.Fields("Name").Value = Trim(tabnam(tabcnt - 1))
				.Fields("id").Value = omcode(tabcnt - 1)
				.Update()
			End With
			initfg = 0
		Loop 
		
		'fill table of perlnd, implnd, rchres sections
		myRec = myDb.OpenRecordset("SectionDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
		For i = 1 To 3
			initfg = 1
			cont = 1
			SCLU = 120 + i
			SGRP = 2
			Do While cont <> 0
				'get each section and its id number
				retid = 0
				olen = 80
				Call F90_WMSGTT(fmsg, SCLU, SGRP, initfg, olen, cont, obuff)
				With myRec
					'add each block name to the block definition table
					'If Trim(Mid(obuff, 1, 6)) <> "ACIDPH" Then
					.AddNew()
					.Fields("Name").Value = Trim(Mid(obuff, 1, 8))
					.Fields("BlockID").Value = SCLU
					.Fields("id").Value = (100 * i) + CInt(Mid(obuff, 10, 3))
					.Update()
					'End If
				End With
				initfg = 0
			Loop 
		Next i
		For i = 1 To tabcnt
			'add dummy sections for blocks without sections
			If omcode(i - 1) < 121 Or omcode(i - 1) > 123 Then
				With myRec
					'add each block name to the block definition table
					.AddNew()
					.Fields("Name").Value = "<NONE>"
					.Fields("BlockID").Value = omcode(i - 1)
					.Fields("id").Value = omcode(i - 1)
					.Update()
				End With
			End If
		Next i
		
		mytableRec = myDb.OpenRecordset("TableDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
		myparmrec = myDb.OpenRecordset("ParmDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
		uunits = 1
		For i = 1 To tabcnt
			'loop through each block
			retid = 0
			tabno = 1
			If omcode(i - 1) > 2 Then
				'cant do for global block, get details about all others
				If omcode(i - 1) < 100 Then
					'uci block (files,ext targs, etc)
					Call F90_XTINFO(omcode(i - 1), tabno, 2, 0, lnflds, lscol, lflen, lftyp, lapos, limetmin, limetmax, limetdef, lrmetmin, lrmetmax, lrmetdef, lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
					Call F90_XTINFO(omcode(i - 1), tabno, uunits, 0, lnflds, lscol, lflen, lftyp, lapos, limin, limax, lidef, lrmin, lrmax, lrdef, lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
					Call F90_WMSGTW(CInt(1), Assoc)
					Call F90_WMSGTH(gptr, fptr(0))
					If retid = 0 Then
						'got some info about this block
						With mytableRec
							'write to table definition table
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.AddNew. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							.AddNew()
							'!Name = Trim(tabnam(i - 1))
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!Name. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							!Name = "<NONE>"
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.RecordCount. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!id. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							!id = .RecordCount + 1
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!sectionid. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							!sectionid = omcode(i - 1)
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!numoccur. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							!numoccur = irept
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!SGRP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							!SGRP = 1
							lheader = addComment(RTrim(hdrbuf(0)), 0)
							For j = 2 To lnmhdr
								'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								lheader = lheader & vbCrLf & addComment(RTrim(hdrbuf(j - 1)), 0)
							Next j
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!headerE. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							!headerE = lheader
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!headerM. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							!headerM = lheader
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!occurgroupid. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							!occurgroupid = 0
							'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.Update. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							.Update()
						End With
						With myparmrec
							'write to parameter definition table
							For j = 1 To lnflds
								'loop through parameter fields
								'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								If Len(Trim(lfdnam(j - 1))) > 0 Then
									'dont add fields without field names
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.AddNew. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									.AddNew()
									'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Name. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!Name = Trim(lfdnam(j - 1))
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.RecordCount. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!id. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!id = .RecordCount + 1
									'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.RecordCount. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!TableId. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!TableId = mytableRec.RecordCount
									'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Type. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!Type = Mid(lftyp, j, 1)
									'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!StartCol. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!StartCol = lscol(j - 1) - 3
									'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!length. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!length = lflen(j - 1)
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Type. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									If !Type = "I" Then
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!minimum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!minimum = limin(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!maximum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!maximum = limax(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Default. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!Default = lidef(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricminimum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!metricminimum = limetmin(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricmaximum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!metricmaximum = limetmax(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricDefault. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!metricDefault = limetdef(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Type. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									ElseIf !Type = "R" Then 
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!minimum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!minimum = lrmin(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!maximum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!maximum = lrmax(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Default. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!Default = lrdef(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricminimum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!metricminimum = lrmetmin(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricmaximum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!metricmaximum = lrmetmax(lapos(j - 1) - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricDefault. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!metricDefault = lrmetdef(lapos(j - 1) - 1)
									End If
									'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.Update. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									.Update()
								End If
							Next j
						End With
					End If
				Else
					'this is an operation type table
					Init = 1
					Do While retid > -1
						'loop through each operation table (perlnd, mutsin, etc)
						Call F90_GTNXKW(Init, omcode(i - 1), kwd, kflg, contfg, tabret)
						Init = 0
						'do for metric table, then english
						Call F90_XTINFO(omcode(i - 1), tabno, 2, 0, lnflds, lscol, lflen, lftyp, lapos, limetmin, limetmax, limetdef, lrmetmin, lrmetmax, lrmetdef, lnmhdrM, hdrbufM, lfdnam, isect, irept, retid)
						Call F90_XTINFO(omcode(i - 1), tabno, uunits, 0, lnflds, lscol, lflen, lftyp, lapos, limin, limax, lidef, lrmin, lrmax, lrdef, lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
						Call F90_WMSGTW(CInt(1), Assoc)
						If retid = 0 Then
							'got a table
							If lflen(0) = 8 Then
								'need to add 2 characters to starting pos
								adjLen = 2
							Else
								adjLen = 0
							End If
							
							'If Mid(kwd, 1, 5) <> "ACID-" Then
							'need to update some table names that were truncated
							kwd = AddChar2Keyword(kwd)
							
							With mytableRec
								'add info about this table to table definitions table
								'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.AddNew. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								.AddNew()
								'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!Name. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								!Name = Trim(kwd)
								'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.RecordCount. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!id. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								!id = .RecordCount + 1
								If isect = 0 Then
									'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!sectionid. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!sectionid = omcode(i - 1)
									'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!occurgroupid. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!occurgroupid = 0
								ElseIf isect > 20 Then 
									'a group of repeating tables is denoted by this
									'strip off the last digit of isect
									'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!sectionid. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!sectionid = (100 * (omcode(i - 1) - 120)) + Int(isect / 10)
									'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!occurgroupid. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!occurgroupid = isect - (Int(isect / 10) * 10)
								Else
									'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!sectionid. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!sectionid = (100 * (omcode(i - 1) - 120)) + isect
									'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!occurgroupid. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									!occurgroupid = 0
								End If
								'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!numoccur. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								!numoccur = irept
								'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!SGRP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								!SGRP = tabno
								lheader = addComment(RTrim(hdrbuf(0)), adjLen)
								For j = 2 To lnmhdr
									'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									lheader = lheader & vbCrLf & addComment(RTrim(hdrbuf(j - 1)), adjLen)
								Next j
								'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!headerE. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								!headerE = lheader
								lheader = addComment(RTrim(hdrbufM(0)), adjLen)
								For j = 2 To lnmhdrM
									'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									lheader = lheader & vbCrLf & addComment(RTrim(hdrbufM(j - 1)), adjLen)
								Next j
								'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!headerM. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								!headerM = lheader
								'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.Update. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								.Update()
							End With
							With myparmrec
								'add info about the fields of this table to the
								'parameter definition table
								For j = 2 To lnflds
									'loop through fields
									'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									If Len(Trim(lfdnam(j - 1))) > 0 Then
										'dont add fields without field names
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.AddNew. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										.AddNew()
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Name. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!Name = Trim(lfdnam(j - 1))
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.RecordCount. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!id. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!id = .RecordCount + 1
										'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.RecordCount. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!TableId. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!TableId = mytableRec.RecordCount
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Type. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!Type = Mid(lftyp, j, 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!StartCol. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!StartCol = lscol(j - 1) + adjLen
										'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!length. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										!length = lflen(j - 1)
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Type. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										If !Type = "I" Then
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!minimum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!minimum = limin(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!maximum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!maximum = limax(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Default. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!Default = lidef(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricminimum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!metricminimum = limetmin(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricmaximum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!metricmaximum = limetmax(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricDefault. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!metricDefault = limetdef(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Type. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										ElseIf !Type = "R" Then 
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!minimum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!minimum = lrmin(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!maximum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!maximum = lrmax(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Default. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!Default = lrdef(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricminimum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!metricminimum = lrmetmin(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricmaximum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!metricmaximum = lrmetmax(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!metricDefault. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											!metricDefault = lrmetdef(lapos(j - 1) - 1)
											'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!Type. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										ElseIf !Type = "C" Then 
											'special case for some 4character category parms
											'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
											If lflen(j - 1) = 4 Then
												'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
												If (Left(Trim(lfdnam(j - 1)), 4) = "CTAG" Or Left(Trim(lfdnam(j - 1)), 5) = "CFVOL" Or Trim(lfdnam(j - 1)) = "CEVAP" Or Trim(lfdnam(j - 1)) = "CPREC" Or Trim(lfdnam(j - 1)) = "ICAT") Then
													'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!length. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
													!length = 2
													'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
													'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!StartCol. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
													!StartCol = lscol(j - 1) + adjLen + 2
												End If
											End If
										End If
										'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.Update. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
										.Update()
									End If
								Next j
							End With
							'End If
						End If
						tabno = tabno + 1
					Loop 
				End If
			End If
		Next i
		
		'get help information for each table
		mytableRec = myDb.OpenRecordset("TableDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
		myQRS = myDb.OpenRecordset("BlockIDFromTableID", DAO.RecordsetTypeEnum.dbOpenDynaset)
		myparmrec = myDb.OpenRecordset("ParmDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
		With mytableRec
			'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.MoveLast. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			.MoveLast()
			'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.RecordCount. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			j = .RecordCount
			'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.MoveFirst. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			.MoveFirst()
			On Error Resume Next
			'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.MoveFirst. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			myparmrec.MoveFirst()
			'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			For i = 1 To j
				myQRS.MoveFirst()
				myQRS.FindFirst(("TableDefns.ID = " & i))
				BlockID = myQRS.Fields("BlockDefns.ID").Value
				'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!SGRP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Call F90_XTINFO(BlockID, !SGRP, uunits, 0, lnflds, lscol, lflen, lftyp, lapos, limin, limax, lidef, lrmin, lrmax, lrdef, lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
				initfg = 1
				olen = 80
				cont = 1
				obuff = ""
				'UPGRADE_WARNING: Couldn't resolve default property of object tempbuff. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				tempbuff = ""
				SCLU = -1
				Call F90_WMSGTH(gptr, fptr(0))
				Do While cont = 1
					If gptr > 0 Then
						olen = 80
						Call F90_WMSGTT(fmsg, SCLU, gptr, initfg, olen, cont, obuff)
						If Len(tempbuff) = 0 Then
							'UPGRADE_WARNING: Couldn't resolve default property of object tempbuff. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							tempbuff = Trim(obuff)
						Else
							'UPGRADE_WARNING: Couldn't resolve default property of object tempbuff. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							tempbuff = tempbuff & " " & Trim(obuff)
						End If
						SCLU = 0
					Else
						cont = 0
					End If
				Loop 
				'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.Edit. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				.Edit()
				'UPGRADE_WARNING: Couldn't resolve default property of object tempbuff. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec!help. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				!help = Trim(tempbuff)
				'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.Update. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				.Update()
				'UPGRADE_WARNING: Couldn't resolve default property of object mytableRec.MoveNext. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				.MoveNext()
				'now fill parameter help
				'UPGRADE_WARNING: Couldn't resolve default property of object istart. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				istart = 2
				If BlockID = 4 Then
					'special case for ftables
					'UPGRADE_WARNING: Couldn't resolve default property of object istart. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					istart = 1
				End If
				'UPGRADE_WARNING: Couldn't resolve default property of object istart. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				For k = istart To lnflds
					'UPGRADE_WARNING: Couldn't resolve default property of object k. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					If Len(Trim(lfdnam(k - 1))) > 0 Then
						'dont add help for fields without field names
						'UPGRADE_WARNING: Couldn't resolve default property of object k. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						If fptr(k - 1) > 0 Then
							initfg = 1
							olen = 80
							cont = 1
							obuff = ""
							'UPGRADE_WARNING: Couldn't resolve default property of object tempbuff. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							tempbuff = ""
							SCLU = -1
							Do While cont = 1
								olen = 80
								'UPGRADE_WARNING: Couldn't resolve default property of object k. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								Call F90_WMSGTT(fmsg, SCLU, fptr(k - 1), initfg, olen, cont, obuff)
								If Len(tempbuff) = 0 Then
									'UPGRADE_WARNING: Couldn't resolve default property of object tempbuff. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									tempbuff = Trim(obuff)
								Else
									'UPGRADE_WARNING: Couldn't resolve default property of object tempbuff. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									tempbuff = tempbuff & " " & Trim(obuff)
								End If
								SCLU = 0
							Loop 
							'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.Edit. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							myparmrec.Edit()
							'UPGRADE_WARNING: Couldn't resolve default property of object tempbuff. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec!help. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							myparmrec!help = Trim(tempbuff)
							'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.Update. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							myparmrec.Update()
						End If
						'UPGRADE_WARNING: Couldn't resolve default property of object myparmrec.MoveNext. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						myparmrec.MoveNext()
					End If
				Next k
			Next i
		End With
		
		'fill table of timeseries groups/member names for each operation
		For i = 1 To tabcnt
			myRec = myDb.OpenRecordset("TSGroupDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
			If omcode(i - 1) > 100 Then
				initfg = 1
				cont = 1
				SGRP = 1
				'UPGRADE_WARNING: Couldn't resolve default property of object igroup. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				igroup = 0
				Do While cont <> 0
					'get each group name
					retid = 0
					olen = 10
					Call F90_WMSGTT(fmsg, omcode(i - 1) + 20, SGRP, initfg, olen, cont, obuff)
					With myRec
						'add each group name to the group definition table
						.AddNew()
						.Fields("Name").Value = Trim(Mid(obuff, 1, 6))
						.Fields("BlockID").Value = omcode(i - 1)
						'UPGRADE_WARNING: Couldn't resolve default property of object igroup. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						igroup = igroup + 1
						'UPGRADE_WARNING: Couldn't resolve default property of object igroup. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						.Fields("id").Value = ((omcode(i - 1) - 120) * 100) + igroup
						.Update()
					End With
					'save base number for associated sgrp
					ReDim Preserve groupbase(igroup)
					'UPGRADE_WARNING: Couldn't resolve default property of object igroup. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					groupbase(igroup - 1) = CShort(Mid(obuff, 8, 3))
					initfg = 0
				Loop 
				'now populate member names
				myRec = myDb.OpenRecordset("TSMemberDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
				'UPGRADE_WARNING: Couldn't resolve default property of object igroup. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				For j = 1 To igroup
					initfg = 1
					cont = 1
					'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					SGRP = 1 + j
					'UPGRADE_WARNING: Couldn't resolve default property of object ngroup. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					ngroup = 1
					Do While cont <> 0
						'get each member name
						retid = 0
						olen = 8
						Call F90_WMSGTT(fmsg, omcode(i - 1) + 20, SGRP, initfg, olen, cont, obuff)
						With myRec
							'add each member name to the member definition table
							.AddNew()
							.Fields("Name").Value = Trim(Mid(obuff, 1, 6))
							'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							.Fields("TSGroupID").Value = ((omcode(i - 1) - 120) * 100) + j
							.Fields("id").Value = .RecordCount + 1
							.Fields("SCLU").Value = omcode(i - 1) + 20
							'UPGRADE_WARNING: Couldn't resolve default property of object ngroup. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							.Fields("SGRP").Value = groupbase(j - 1) + ngroup
							.Update()
						End With
						'UPGRADE_WARNING: Couldn't resolve default property of object ngroup. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						ngroup = ngroup + 1
						initfg = 0
					Loop 
				Next j
			End If
		Next i
		'now populate member details
		myRec = myDb.OpenRecordset("TSMemberDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
		With myRec
			.MoveLast()
			'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			j = .RecordCount
			.MoveFirst()
			On Error Resume Next
			'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			For i = 1 To j
				'get first line of details
				initfg = 1
				olen = 80
				cont = 1
				obuff = ""
				Call F90_WMSGTT(fmsg, .Fields("SCLU").Value, .Fields("SGRP").Value, initfg, olen, cont, obuff)
				.Edit()
				.Fields("mdim1").Value = CShort(Mid(obuff, 7, 3))
				.Fields("mdim2").Value = CShort(Mid(obuff, 10, 3))
				.Fields("maxsb1").Value = CShort(Mid(obuff, 13, 6))
				.Fields("maxsb2").Value = CShort(Mid(obuff, 19, 6))
				.Fields("mkind").Value = CShort(Mid(obuff, 25, 2))
				.Fields("sptrn").Value = CShort(Mid(obuff, 27, 2))
				.Fields("msect").Value = CShort(Mid(obuff, 29, 2))
				.Fields("mio").Value = CShort(Mid(obuff, 31, 2))
				.Fields("osvbas").Value = CShort(Mid(obuff, 33, 5))
				.Fields("osvoff").Value = CShort(Mid(obuff, 38, 6))
				If Len(Trim(Mid(obuff, 49, 8))) > 0 Then
					.Fields("eunits").Value = Trim(Mid(obuff, 49, 8))
				Else
					.Fields("eunits").Value = " "
				End If
				.Fields("ltval1").Value = CSng(Mid(obuff, 57, 4))
				.Fields("ltval2").Value = CSng(Mid(obuff, 61, 8))
				.Fields("ltval3").Value = CSng(Mid(obuff, 69, 4))
				.Fields("ltval4").Value = CSng(Mid(obuff, 73, 8))
				'now get second line of details
				initfg = 0
				olen = 80
				Call F90_WMSGTT(fmsg, .Fields("SCLU").Value, .Fields("SGRP").Value, initfg, olen, cont, obuff)
				.Fields("defn").Value = Trim(Mid(obuff, 1, 43))
				If Len(Trim(Mid(obuff, 49, 8))) > 0 Then
					.Fields("munits").Value = Trim(Mid(obuff, 49, 8))
				Else
					.Fields("munits").Value = " "
				End If
				.Fields("ltval5").Value = CSng(Mid(obuff, 57, 4))
				.Fields("ltval6").Value = CSng(Mid(obuff, 61, 8))
				.Fields("ltval7").Value = CSng(Mid(obuff, 69, 4))
				.Fields("ltval8").Value = CSng(Mid(obuff, 73, 8))
				.Update()
				.MoveNext()
			Next i
		End With
		
		'build table of table aliases
		myTableAliasDefn = myDb.OpenRecordset("TableAliasDefn", DAO.RecordsetTypeEnum.dbOpenDynaset)
		With myTableAliasDefn
			gnum = 10
			SCLU = 120
			Do While SCLU < 123 'more operation types
				SCLU = SCLU + 1
				initfg = 1
				cont = 1
				opnid = SCLU - 120
				OName = ""
				Do While cont <> 0
					olen = 80
					Call F90_WMSGTT(fmsg, SCLU, gnum, initfg, olen, cont, obuff)
					initfg = 0
					'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn.AddNew. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					.AddNew()
					'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!OpnTypID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					!OpnTypID = opnid
					'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!Name. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					!Name = Left(obuff, 12)
					'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!Name. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					If OName <> !Name Then
						Occur = 1
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!Name. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						OName = !Name
					Else
						Occur = Occur + 1
					End If
					'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!Occur. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					!Occur = Occur
					If Len(obuff) > 14 Then
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!AppearName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						!AppearName = Mid(obuff, 15, 20)
					Else
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!AppearName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						!AppearName = " "
					End If
					If Len(obuff) > 36 Then
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!IDVarName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						!IDVarName = Mid(obuff, 37, 6)
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!IDVarName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						crit = "Name = '" & !IDVarName & "'"
						myRec.FindFirst(crit)
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!IDVar. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						!IDVar = myRec.Fields("id").Value
					Else
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!IDVarName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						!IDVarName = " "
					End If
					If Len(obuff) > 44 Then
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!SubsKeyName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						!SubsKeyName = Mid(obuff, 45, 6)
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!SubsKeyName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						crit = "Name = '" & !SubsKeyName & "'"
						myRec.FindFirst(crit)
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!IDSubs. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						!IDSubs = myRec.Fields("id").Value
					Else
						'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn!SubsKeyName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						!SubsKeyName = " "
					End If
					'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn.Update. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					.Update()
				Loop 
			Loop 
		End With
		'UPGRADE_WARNING: Couldn't resolve default property of object myTableAliasDefn.Close. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		myTableAliasDefn.Close()
		
		'build table of wdm attributes
		On Error GoTo 0
		myRec = myDb.OpenRecordset("WDMAttribDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
		id = 1
		Do While id < 500
			Call F90_WDSAGY(fmsg, id, ilen, itype, rmin, rmax, rdef, hlen, hrec, hpos, vlen, Name, desc, svalid)
			'add info about this attrib to attrib definitions table
			With myRec
				'UPGRADE_WARNING: Couldn't resolve default property of object o. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If Len(Name) > o Then
					.AddNew()
					.Fields("Name").Value = Trim(Name)
					.Fields("id").Value = id
					.Fields("length").Value = ilen
					.Fields("Type").Value = itype
					.Fields("desc").Value = Trim(desc)
					.Fields("Min").Value = rmin
					.Fields("Max").Value = rmax
					.Fields("Def").Value = rdef
					If vlen > 0 Then
						.Fields("valid").Value = svalid
					End If
					.Update()
				End If
			End With
			id = id + 1
		Loop 

        'If ToStatus IsNot Nothing Then
        '    ToStatus.Progress(100, 100)
        '    ToStatus.Status("(EXIT)")
        'End If
        If pStatusMonitor IsNot Nothing Then
            Logger.ProgressStatus = pStatusMonitor.InnerProgressStatus
            pStatusMonitor.StopMonitor()
        End If
    End Sub

    Private Sub MonitorButtonPressed(ByVal aButton As String)
        Select Case aButton
            Case "C" : g_Cancel = True
            Case "P" : g_Pause = True
            Case "R" : g_Pause = False
        End Select
    End Sub

	Private Function addComment(ByRef s As String, ByRef adjLen As Integer) As String
		Dim i As Integer
		Dim t As String
		If adjLen > 0 Then 'adjust length for old aide 78 char problem - add 2 blanks at 1 and 6
			s = " " & Left(s, 4) & " " & Right(s, Len(s) - 4)
		End If
		If Not (InStr(s, "***")) Then 'not a comment
			i = InStr(s, "   ")
			If i > 0 Then 'replace first three blanks
				t = Left(s, i - 1) & "***" & Right(s, Len(s) - i - 2)
				s = t
			ElseIf Len(s) < 78 Then  'at end
				s = s & "***"
			Else
				s = Left(s, 77) & "***"
			End If
		End If
		addComment = s
    End Function
End Module

''' <summary>
''' Sends messages to VB6 Status Monitor. 
''' Passes messages received from Status Monitor to file handle pPipeReadFromStatus
''' </summary>
''' <remarks></remarks>
'Friend Class StatusMonitor
'    Implements MapWinUtility.IProgressStatus

'    Dim pInit As Boolean = False
'    Dim pMonitorProcess As Process

'    Public Event MonitorButtonPressed(ByVal aButton As String)

'    Public Sub Progress(ByVal aCurrentPosition As Integer, ByVal aLastPosition As Integer) Implements MapWinUtility.IProgressStatus.Progress
'        WriteStatus("PROGRESS " & aCurrentPosition & " of " & aLastPosition)
'    End Sub

'    Public Sub Status(ByVal aStatusMessage As String) Implements MapWinUtility.IProgressStatus.Status
'        If Not pInit Then
'            Try
'                Dim lProcessId As Integer = Process.GetCurrentProcess.Id
'                pMonitorProcess = New Process
'                With pMonitorProcess.StartInfo
'                    .FileName = FindFile("Status Monitor", "StatusMonitor.exe")
'                    .Arguments = lProcessId
'                    .CreateNoWindow = True
'                    .UseShellExecute = False
'                    .RedirectStandardInput = True
'                    .RedirectStandardOutput = True
'                    AddHandler pMonitorProcess.OutputDataReceived, AddressOf MonitorButtonHandler
'                    .RedirectStandardError = True
'                    AddHandler pMonitorProcess.ErrorDataReceived, AddressOf MonitorButtonHandler
'                End With
'                pMonitorProcess.Start()
'                pMonitorProcess.BeginErrorReadLine()
'                pMonitorProcess.BeginOutputReadLine()
'                Logger.Dbg("MonitorLaunched")
'                pInit = True
'            Catch ex As Exception
'                Logger.Msg("StatusProcessStartError:" & ex.Message)
'            End Try
'        End If

'        WriteStatus(aStatusMessage)

'        If aStatusMessage.ToLower = "exit" Then
'            If Not pMonitorProcess.HasExited Then
'                pMonitorProcess.Kill()
'            End If
'        End If
'    End Sub

'    Private Function WriteStatus(ByVal aMsg As String) As Boolean
'        If Not IsNothing(pMonitorProcess) Then
'            If pMonitorProcess.HasExited Then
'                If pMonitorProcess.ExitCode <> &H103S Then 'TODO: check to be sure codes have not changed
'                    Return False  'Process at other end of pipe is dead, stop talking to it
'                End If
'            Else
'                If Left(aMsg, 1) = "(" And Right(aMsg, 1) = ")" Then
'                    aMsg = Mid(aMsg, 2, Len(aMsg) - 2)
'                End If

'                If aMsg.Length > 0 Then
'                    Dim OpenParenEscape As String = Chr(6)
'                    aMsg = ReplaceString(aMsg, "(", OpenParenEscape)
'                    Dim CloseParenEscape As String = Chr(7)
'                    aMsg = ReplaceString(aMsg, ")", CloseParenEscape)
'                    If Asc(Right(aMsg, 1)) > 31 Then
'                        aMsg = "(" & aMsg & ")"
'                    End If
'                    Logger.Dbg(aMsg)
'                    pMonitorProcess.StandardInput.WriteLine(aMsg)
'                End If
'                Return True
'            End If
'        End If
'        Return False
'    End Function

'    Private Sub MonitorButtonHandler(ByVal aSendingProcess As Object, _
'                                     ByVal aOutLine As DataReceivedEventArgs)
'        If Not String.IsNullOrEmpty(aOutLine.Data) Then
'            RaiseEvent MonitorButtonPressed(aOutLine.Data)
'        End If
'    End Sub
'End Class