Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfMsg_NET.HspfMsg")> Public Class HspfMsg
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pMsgFileName As String
	Dim pBlockDefs As Collection 'of HspfBlockDef
	Dim pErrorDescription As String
	'Dim pHspfEngine As Object
	'Dim pHspfEngineSet As Boolean
	Dim pTSGroupDefs As Collection 'of HspfTSGroupDefs
	
	Public WriteOnly Property Monitor() As Object
		Set(ByVal Value As Object)
			IPC = Value
			If IPC Is Nothing Then IPCset = False Else IPCset = True
		End Set
	End Property
	
	
	Public Property Name() As String
		Get
			Name = pMsgFileName
		End Get
		Set(ByVal Value As String)
			Dim myDb As DAO.Database
			
            Dim myBlkRs As DAO.Recordset
			Dim lBlock As HspfBlockDef
			Dim lSections As New Collection
			
            Dim mySecRs As DAO.Recordset
			Dim lSection As HspfSectionDef
			Dim critSection As String
			Dim lTables As New Collection
			Dim lBlkTables As New Collection
			
            Dim myTabRs As DAO.Recordset
			Dim ltable As HspfTableDef
			Dim critTable As String
			Dim lParms As New Collection
			
            Dim myParmRs As DAO.Recordset
			Dim lParm As HSPFParmDef
			Dim critParm, lTyp As String
			
            Dim myTSGroupRs As DAO.Recordset
			Dim lTSGroup As HspfTSGroupDef
			Dim lTSMembers As Collection
            Dim myTSMemberRs As DAO.Recordset
			Dim lTSMember As HspfTSMemberDef
			
			Dim lNumeric As Boolean
			Dim h As String
			Dim s As String
			
			Dim lBlkCount, lBlkNow As Integer
			
			On Error GoTo err_Renamed
			If Len(Value) = 0 Then Value = "HSPFmsg.mdb"
            Dim ff As New ATCoCtl.ATCoFindFile
            If Not IO.File.Exists(Value) Then
                ff.SetRegistryInfo("HSPF", "MessageMDB", "Path")
                ff.SetDialogProperties("Please locate 'HSPFmsg.mdb' in a writable directory", "HSPFmsg.mdb")
                Value = ff.GetName
                ff = Nothing
            End If
			myDb = DAODBEngine_definst.OpenDatabase(Value,  , True)
			pMsgFileName = Value
            pBlockDefs = Nothing
			pBlockDefs = New Collection
			myBlkRs = myDb.OpenRecordset("BlockDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
			myBlkRs.MoveLast()
			lBlkCount = myBlkRs.RecordCount
			myBlkRs.MoveFirst()
			lBlkNow = 0
			While Not (myBlkRs.EOF)
				'progress bar (dumb)
				s = "(Progress " & lBlkNow * 100 / lBlkCount & ")"
				'IPC.SendMonitorMessage s
				lBlkNow = lBlkNow + 1
				
				lBlock = New HspfBlockDef
				lBlock.Id = myBlkRs.Fields("ID").Value
				lBlock.Name = myBlkRs.Fields("Name").Value
				'UPGRADE_NOTE: Object lSections may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
				lSections = Nothing
				lSections = New Collection
				'UPGRADE_NOTE: Object lBlkTables may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
				lBlkTables = Nothing
				lBlkTables = New Collection
				mySecRs = myDb.OpenRecordset("SectionDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
				critSection = "BlockID = " & CStr(lBlock.Id)
				mySecRs.FindFirst((critSection))
				While Not (mySecRs.NoMatch)
					lSection = New HspfSectionDef
					lSection.Name = mySecRs.Fields("Name").Value
					If IPCset Then
						If lSection.Name <> "<NONE>" Then
							s = "(MSG3 Reading about " & lBlock.Name & ":" & lSection.Name & ")"
						Else
							s = "(MSG3 Reading about " & lBlock.Name & ")"
						End If
						'IPC.SendMonitorMessage s
					End If
					lSection.Id = mySecRs.Fields("ID").Value
					'UPGRADE_NOTE: Object lTables may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
					lTables = Nothing
					lTables = New Collection
					
					myTabRs = myDb.OpenRecordset("TableDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
					critTable = "SectionID = " & CStr(lSection.Id)
					myTabRs.FindFirst((critTable))
					While Not (myTabRs.NoMatch)
						ltable = New HspfTableDef
						ltable.Id = myTabRs.Fields(0).Value
						ltable.Parent = lSection
						ltable.Name = myTabRs.Fields(2).Value
						ltable.SGRP = myTabRs.Fields(3).Value
						ltable.NumOccur = myTabRs.Fields(4).Value
						ltable.HeaderE = myTabRs.Fields(5).Value
						ltable.HeaderM = myTabRs.Fields(6).Value
						'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
						If IsDbNull(myTabRs.Fields(7).Value) Then
							ltable.Define = " "
						Else
							ltable.Define = myTabRs.Fields(7).Value
						End If
						If myTabRs.Fields.Count < 9 Then
							ltable.OccurGroup = 0
						Else
							ltable.OccurGroup = myTabRs.Fields(8).Value
						End If
						'UPGRADE_NOTE: Object lParms may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
						lParms = Nothing
						lParms = New Collection
						
						critParm = "TableID = " & CStr(ltable.Id)
						myParmRs = myDb.OpenRecordset("Select * from ParmDefns where " & critParm, DAO.RecordsetTypeEnum.dbOpenDynaset)
						'myParmRs.FindFirst (critParm)
						While Not (myParmRs.EOF)
							lParm = New HSPFParmDef
							lParm.Name = myParmRs.Fields(2).Value 'Name
							lTyp = myParmRs.Fields(3).Value 'Type
							Select Case lTyp
								Case "I" : lParm.Typ = ATCoCtl.ATCoDataType.ATCoInt : lNumeric = True
								Case "R" : lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng : lNumeric = True
								Case "C" : lParm.Typ = ATCoCtl.ATCoDataType.ATCoTxt : lNumeric = False
								Case Else : lParm.Typ = ATCData.ATCOperatorType.NONE : lNumeric = False
							End Select
							lParm.StartCol = myParmRs.Fields(4).Value
							lParm.Length = myParmRs.Fields(5).Value
							If lNumeric Then
								lParm.Min = myParmRs.Fields(6).Value
								lParm.Max = myParmRs.Fields(7).Value
								If myParmRs.Fields.Count > 10 Then
									lParm.MetricMin = myParmRs.Fields(10).Value
									lParm.MetricMax = myParmRs.Fields(11).Value
								Else
									lParm.MetricMin = myParmRs.Fields(6).Value
									lParm.MetricMax = myParmRs.Fields(7).Value
								End If
							End If
							'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
							If IsDbNull(myParmRs.Fields(8).Value) Then
								lParm.Default_Renamed = " "
							Else
								lParm.Default_Renamed = myParmRs.Fields(8).Value 'default
							End If
							If myParmRs.Fields.Count > 10 Then
								'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
								If IsDbNull(myParmRs.Fields(12).Value) Then
									lParm.MetricDefault = " "
								Else
									lParm.MetricDefault = myParmRs.Fields(12).Value 'default
								End If
							Else 'use english default
								'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
								If IsDbNull(myParmRs.Fields(8).Value) Then
									lParm.MetricDefault = " "
								Else
									lParm.MetricDefault = myParmRs.Fields(8).Value
								End If
							End If
							lParm.Other = myParmRs.Fields(4).Value & ":" & myParmRs.Fields(5).Value
							'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
							If IsDbNull(myParmRs.Fields(9).Value) Then
								lParm.Define = " "
							Else
								lParm.Define = myParmRs.Fields(9).Value
							End If
							lParms.Add(lParm, lParm.Name)
							myParmRs.MoveNext() ' .FindNext (critParm)
						End While
						myParmRs.Close()
						ltable.ParmDefs = lParms
						updateParmsMultLines((lBlock.Name), ltable)
						lTables.Add(ltable, ltable.Name)
						lBlkTables.Add(ltable, ltable.Name)
						myTabRs.FindNext((critTable))
					End While
					myTabRs.Close()
					lSection.TableDefs = lTables
					lSections.Add(lSection, lSection.Name)
					mySecRs.FindNext((critSection))
				End While
				mySecRs.Close()
				lBlock.SectionDefs = lSections
				lBlock.TableDefs = lBlkTables
				pBlockDefs.Add(lBlock, lBlock.Name)
				myBlkRs.MoveNext()
			End While
			myBlkRs.Close()
			
			'now read TS group and member info
			'UPGRADE_NOTE: Object pTSGroupDefs may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
			pTSGroupDefs = Nothing
			pTSGroupDefs = New Collection
			myTSGroupRs = myDb.OpenRecordset("TSGroupDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
			While Not (myTSGroupRs.EOF)
				lTSGroup = New HspfTSGroupDef
				lTSGroup.Id = myTSGroupRs.Fields("ID").Value
				lTSGroup.Name = myTSGroupRs.Fields("Name").Value
				If IPCset Then
					s = "(MSG3 Reading about Timeseries Groups and Members for " & lTSGroup.Name & ")"
					'IPC.SendMonitorMessage s
				End If
				lTSGroup.BlockId = myTSGroupRs.Fields("BlockID").Value
				'UPGRADE_NOTE: Object lTSMembers may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
				lTSMembers = Nothing
				lTSMembers = New Collection
				myTSMemberRs = myDb.OpenRecordset("TSMemberDefns", DAO.RecordsetTypeEnum.dbOpenDynaset)
				critSection = "TSGroupID = " & CStr(lTSGroup.Id)
				myTSMemberRs.FindFirst((critSection))
				While Not (myTSMemberRs.NoMatch)
					lTSMember = New HspfTSMemberDef
					lTSMember.Id = myTSMemberRs.Fields("ID").Value
					lTSMember.Name = myTSMemberRs.Fields("Name").Value
					lTSMember.TSGroupId = myTSMemberRs.Fields("TSGroupID").Value
					lTSMember.Parent = lTSGroup
					lTSMember.SCLU = myTSMemberRs.Fields("SCLU").Value
					lTSMember.SGRP = myTSMemberRs.Fields("SGRP").Value
                    lTSMember.mdim1 = FilterNull(myTSMemberRs.Fields("mdim1"))
                    lTSMember.mdim2 = FilterNull(myTSMemberRs.Fields("mdim2"))
                    lTSMember.maxsb1 = FilterNull(myTSMemberRs.Fields("maxsb1"))
                    lTSMember.maxsb2 = FilterNull(myTSMemberRs.Fields("maxsb2"))
                    lTSMember.mkind = FilterNull(myTSMemberRs.Fields("mkind"))
                    lTSMember.sptrn = FilterNull(myTSMemberRs.Fields("sptrn"))
                    lTSMember.msect = FilterNull(myTSMemberRs.Fields("msect"))
                    lTSMember.mio = FilterNull(myTSMemberRs.Fields("mio"))
                    lTSMember.osvbas = FilterNull(myTSMemberRs.Fields("osvbas"))
                    lTSMember.osvoff = FilterNull(myTSMemberRs.Fields("osvoff"))
                    lTSMember.eunits = FilterNull(myTSMemberRs.Fields("eunits"), " ")
                    lTSMember.ltval1 = FilterNull(myTSMemberRs.Fields("ltval1"))
                    lTSMember.ltval2 = FilterNull(myTSMemberRs.Fields("ltval2"))
                    lTSMember.ltval3 = FilterNull(myTSMemberRs.Fields("ltval3"))
                    lTSMember.ltval4 = FilterNull(myTSMemberRs.Fields("ltval4"))
                    lTSMember.defn = FilterNull(myTSMemberRs.Fields("defn"), " ")
                    lTSMember.munits = FilterNull(myTSMemberRs.Fields("munits"), " ")
                    lTSMember.ltval5 = FilterNull(myTSMemberRs.Fields("ltval5"))
                    lTSMember.ltval6 = FilterNull(myTSMemberRs.Fields("ltval6"))
                    lTSMember.ltval7 = FilterNull(myTSMemberRs.Fields("ltval7"))
                    lTSMember.ltval8 = FilterNull(myTSMemberRs.Fields("ltval8"))
					lTSMembers.Add(lTSMember, lTSMember.Name)
					myTSMemberRs.FindNext((critSection))
				End While
				myTSMemberRs.Close()
				lTSGroup.MemberDefs = lTSMembers
				pTSGroupDefs.Add(lTSGroup, CStr(lTSGroup.Id))
				myTSGroupRs.MoveNext()
			End While
			myTSGroupRs.Close()
			
			myDb.Close()
			'If IPCset Then IPC.SendMonitorMessage "(MSG3 )"
			Exit Property
err_Renamed: 
			pErrorDescription = "HspfMsg:Name:" & Err.Description
		End Set
	End Property
	
	Public ReadOnly Property BlockDefs() As Collection
		Get 'of HspfBlockDef
			BlockDefs = pBlockDefs
		End Get
	End Property
	
	Public ReadOnly Property TSGroupDefs() As Collection
		Get 'of HspfTSGroupDef
			TSGroupDefs = pTSGroupDefs
		End Get
	End Property
	
	Public ReadOnly Property ErrorDescription() As String
		Get
			ErrorDescription = pErrorDescription
			pErrorDescription = ""
		End Get
    End Property

	Private Function FilterNull(ByRef v As Object, Optional ByRef NullReturn As Object = 0) As Object
		'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If IsDBNull(v.value) Then
            Return NullReturn
        Else
            Return v.value
        End If
	End Function
	
	Private Sub updateParmsMultLines(ByRef blockname As String, ByRef ltable As HspfTableDef)
		Dim i, j As Integer
		Dim lParm As HSPFParmDef
		
		With ltable
			If blockname = "DURANL" And .Name = "LEVELS" Then
				For i = 1 To 6
					lParm = New HSPFParmDef
					lParm.Name = "LEVE" & CStr(15 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 76 + (i * 5)
					lParm.Length = 5
					lParm.Min = -999
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = "LEVEL(2thru21) contains the 20 possible user-specified levels for which the input time series will be analyzed."
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "DURANL" And .Name = "LCONC" Then 
				For i = 1 To 3 'three fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "LCONC" & CStr(7 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 71 + (i * 10)
					lParm.Length = 10
					lParm.Min = -999
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "PERLND" And .Name = "IRRIG-SCHED" Then 
				For i = 2 To 10 'up to 10 rows possible
					lParm = New HSPFParmDef
					lParm.Name = "IRYR" & CStr((2 * (i - 1)) + 1) 'year
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 12
					lParm.Length = 4
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRMO" & CStr((2 * (i - 1)) + 1) 'month
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 17
					lParm.Length = 2
					lParm.Min = 1
					lParm.Max = 12
					lParm.Default_Renamed = 1
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRDY" & CStr((2 * (i - 1)) + 1) 'day
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 20
					lParm.Length = 2
					lParm.Min = 1
					lParm.Max = 31
					lParm.Default_Renamed = 1
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRHR" & CStr((2 * (i - 1)) + 1) 'hour
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 23
					lParm.Length = 2
					lParm.Min = 0
					lParm.Max = 24
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRMI" & CStr((2 * (i - 1)) + 1) 'min
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 26
					lParm.Length = 2
					lParm.Min = 0
					lParm.Max = 60
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRDUR" & CStr((2 * (i - 1)) + 1) 'duration
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 28
					lParm.Length = 5
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRRAT" & CStr((2 * (i - 1)) + 1) 'rate
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 33
					lParm.Length = 10
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRYR" & CStr(2 * i) '2nd year
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 49
					lParm.Length = 4
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRMO" & CStr(2 * i) 'month
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 54
					lParm.Length = 2
					lParm.Min = 1
					lParm.Max = 12
					lParm.Default_Renamed = 1
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRDY" & CStr(2 * i) 'day
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 57
					lParm.Length = 2
					lParm.Min = 1
					lParm.Max = 31
					lParm.Default_Renamed = 1
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRHR" & CStr(2 * i) 'hour
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 60
					lParm.Length = 2
					lParm.Min = 0
					lParm.Max = 24
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRMI" & CStr(2 * i) 'min
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 63
					lParm.Length = 2
					lParm.Min = 0
					lParm.Max = 60
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRDUR" & CStr(2 * i) 'duration
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 65
					lParm.Length = 5
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "IRRAT" & CStr(2 * i) 'rate
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = (70 * (i - 1)) + 70
					lParm.Length = 10
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "RCHRES" And .Name = "HT-BED-DELH" Then 
				For i = 2 To 14 '100 values needed
					For j = 1 To 7 '
						lParm = New HSPFParmDef
						lParm.Name = "DELH" & CStr((7 * (i - 1)) + j)
						lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
						lParm.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
						lParm.Length = 10
						lParm.Min = -999
						lParm.Max = -999
						lParm.Default_Renamed = 0
						lParm.Other = lParm.StartCol & ":" & lParm.Length
						lParm.Define = ""
						.ParmDefs.Add(lParm, lParm.Name)
					Next j
				Next i
				For i = 1 To 2 'two more fields to tack on to make 100
					lParm = New HSPFParmDef
					lParm.Name = "DELH" & CStr(98 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 991 + (10 * (i - 1))
					lParm.Length = 10
					lParm.Min = -999
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "RCHRES" And .Name = "HT-BED-DELTT" Then 
				For i = 2 To 14 '100 values needed
					For j = 1 To 7 '
						lParm = New HSPFParmDef
						lParm.Name = "DELTT" & CStr((7 * (i - 1)) + j)
						lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
						lParm.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
						lParm.Length = 10
						lParm.Min = -999
						lParm.Max = -999
						lParm.Default_Renamed = 0
						lParm.Other = lParm.StartCol & ":" & lParm.Length
						lParm.Define = ""
						.ParmDefs.Add(lParm, lParm.Name)
					Next j
				Next i
				For i = 1 To 2 'two more fields to tack on to make 100
					lParm = New HSPFParmDef
					lParm.Name = "DELTT" & CStr(98 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 991 + (10 * (i - 1))
					lParm.Length = 10
					lParm.Min = -999
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "RCHRES" And .Name = "GQ-PHOTPM" Then 
				For i = 1 To 7 'seven fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "PHOTPM" & CStr(7 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 71 + (i * 10)
					lParm.Length = 10
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
				For i = 1 To 6 'six more fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "PHOTPM" & CStr(14 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 141 + (i * 10)
					lParm.Length = 10
					If i < 5 Then
						lParm.Min = 0
						lParm.Max = -999
						lParm.Default_Renamed = 0
					ElseIf i = 5 Then 
						lParm.Min = 0.0001
						lParm.Max = 10
						lParm.Default_Renamed = 1
					Else
						lParm.Min = 1
						lParm.Max = 2
						lParm.Default_Renamed = 1
					End If
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "RCHRES" And .Name = "GQ-ALPHA" Then 
				For i = 1 To 7 'seven fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "ALPH" & CStr(7 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 71 + (i * 10)
					lParm.Length = 10
					lParm.Min = 0.00001
					lParm.Max = -999
					lParm.Default_Renamed = -999
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
				For i = 1 To 4 'four more fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "ALPH" & CStr(14 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 141 + (i * 10)
					lParm.Length = 10
					lParm.Min = 0.00001
					lParm.Max = -999
					lParm.Default_Renamed = -999
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "RCHRES" And .Name = "GQ-GAMMA" Then 
				For i = 1 To 7 'seven fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "GAMM" & CStr(7 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 71 + (i * 10)
					lParm.Length = 10
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
				For i = 1 To 4 'four more fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "GAMM" & CStr(14 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 141 + (i * 10)
					lParm.Length = 10
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "RCHRES" And .Name = "GQ-DELTA" Then 
				For i = 1 To 7 'seven fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "DEL" & CStr(7 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 71 + (i * 10)
					lParm.Length = 10
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
				For i = 1 To 4 'four more fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "DEL" & CStr(14 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 141 + (i * 10)
					lParm.Length = 10
					lParm.Min = 0
					lParm.Max = -999
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "RCHRES" And .Name = "GQ-CLDFACT" Then 
				For i = 1 To 7 'seven fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "KCLD" & CStr(7 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 71 + (i * 10)
					lParm.Length = 10
					lParm.Min = 0
					lParm.Max = 1
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
				For i = 1 To 4 'four more fields to tack on
					lParm = New HSPFParmDef
					lParm.Name = "KCLD" & CStr(14 + i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
					lParm.StartCol = 141 + (i * 10)
					lParm.Length = 10
					lParm.Min = 0
					lParm.Max = 1
					lParm.Default_Renamed = 0
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "RCHRES" And .Name = "GQ-DAUGHTER" Then 
				For i = 2 To 3 '3 rows needed
					For j = 1 To 3 'three values per row
						lParm = New HSPFParmDef
						lParm.Name = "ZERO" & CStr(i) & CStr(j)
						lParm.Typ = ATCoCtl.ATCoDataType.ATCoSng
						lParm.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
						lParm.Length = 10
						lParm.Min = 0
						lParm.Max = -999
						lParm.Default_Renamed = 0
						lParm.Other = lParm.StartCol & ":" & lParm.Length
						lParm.Define = ""
						.ParmDefs.Add(lParm, lParm.Name)
					Next j
				Next i
			ElseIf blockname = "REPORT" And .Name = "REPORT-SRC" Then 
				For i = 2 To 25 'up to 25 rows possible
					lParm = New HSPFParmDef
					lParm.Name = "SRCID" & CStr(i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoTxt
					lParm.StartCol = (70 * (i - 1)) + 11
					lParm.Length = 20
					lParm.Default_Renamed = ""
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			ElseIf blockname = "REPORT" And .Name = "REPORT-CON" Then 
				For i = 2 To 20 'up to 20 rows possible
					lParm = New HSPFParmDef
					lParm.Name = "CONID" & CStr(i) 'Name
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoTxt
					lParm.StartCol = ((i - 1) * 70) + 11
					lParm.Length = 20
					lParm.Default_Renamed = ""
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "TRAN" & CStr(i) 'tran
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoTxt
					lParm.StartCol = (70 * (i - 1)) + 32
					lParm.Length = 4
					lParm.Default_Renamed = "SUM"
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "SIGD" & CStr(i) 'sig digits
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoInt
					lParm.StartCol = (70 * (i - 1)) + 36
					lParm.Length = 5
					lParm.Min = 2
					lParm.Max = 5
					lParm.Default_Renamed = 5
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
					lParm = New HSPFParmDef
					lParm.Name = "DECPLA" & CStr(i) 'dec places
					lParm.Typ = ATCoCtl.ATCoDataType.ATCoInt
					lParm.StartCol = (70 * (i - 1)) + 41
					lParm.Length = 5
					lParm.Min = 0
					lParm.Max = 3
					lParm.Default_Renamed = 2
					lParm.Other = lParm.StartCol & ":" & lParm.Length
					lParm.Define = ""
					.ParmDefs.Add(lParm, lParm.Name)
				Next i
			End If
		End With
	End Sub
End Class