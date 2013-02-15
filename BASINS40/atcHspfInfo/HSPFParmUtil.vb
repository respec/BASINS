Option Strict Off
Option Explicit On
Module HSPFParmUtil
	
	Public fmsg As Integer
	Public fwdm As Integer
	
	Structure OperDetails
		Dim name As String
		Dim Count As Short
		Dim Ind As Short
		Dim SegID() As Integer
		Dim SegNum() As Integer
	End Structure
	Dim OpDet(3) As OperDetails
	
	Structure ParmDetail
		Dim name As String
		Dim StartCol As Integer
		Dim Width As Integer
		Dim Def As String
		Dim id As Integer
	End Structure
	
	'UPGRADE_NOTE: filter was upgraded to filter_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Structure filter_Renamed
		Dim txt As String
		Dim num As Integer
		Dim id() As Integer
	End Structure
	
	'Global myDB As Database
	'Global myParmDefn As Recordset
	'Global myParmTableDefn As Recordset
	'Global myTableAliasDefn As Recordset
	'Global myWat As Recordset
	'Global myScen As Recordset
	'Global mySeg As Recordset
	'Global myTab As Recordset
	'Global myParmData As Recordset
	'Global myTabDef As TableDef
	'Global myRec As Recordset
	'Global mySQL As String
	'Global myQuery As QueryDef
	'Global myIndx As Index
	'Global myRel As Relation
	
	Public FiltInd As Integer
	Public Filt(3) As filter_Renamed
	
	Public ExName As String
	Public ExCmd As String
	Public ExPath As String
	
	Sub GetScenInfo(ByRef scnID As Integer)
		Dim Ind, contfg, id, Init, kflg, retid, i As Integer
		Dim kwd As String
		
		'look for perlnd, implnd, rchres operation types
		Init = 1
		id = 0
		Do 
			Call F90_GTNXKW(Init, id, kwd, kflg, contfg, retid)
			If kflg > 0 And (retid > 120 And retid <= 123) Then ' just P,I,R
				'this operation type exists
				Ind = retid - 120
				OpDet(Ind).name = kwd
				OpDet(Ind).Count = 0
				OpDet(Ind).Ind = Ind
				Call GetOperInfo(OpDet(Ind), scnID)
			End If
			Init = 0
		Loop While contfg = 1
		
		For i = 1 To 3
			Call GetTableDetails(i)
		Next i
	End Sub
	
	Sub GetOperInfo(ByRef OpLoc As OperDetails, ByRef scnID As Integer) 'from frmgenscnact (cousin)
		'find information about hspf operation type
		Dim cbuff, crit As String
		Dim retcod, omcode, Init, retkey As Integer
		Dim i As Short
		'UPGRADE_WARNING: Arrays in structure mySeg may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim mySeg As DAO.Recordset
		
		mySeg = myDb.OpenRecordset("SegData", DAO.RecordsetTypeEnum.dbOpenDynaset)
		
		omcode = 3
		Init = 1
		Call F90_XBLOCK(omcode, Init, retkey, cbuff, retcod)
		Init = 0
		Do 
			Call F90_XBLOCK(omcode, Init, retkey, cbuff, retcod)
			If Mid(cbuff, 7, 6) = OpLoc.name Then
				OpLoc.Count = OpLoc.Count + 1
				ReDim Preserve OpLoc.SegID(OpLoc.Count)
				ReDim Preserve OpLoc.SegNum(OpLoc.Count)
				
				i = CShort(Mid(cbuff, 18, 3))
				OpLoc.SegNum(OpLoc.Count) = i
				With mySeg
					.AddNew()
					.Fields("name").Value = Mid(cbuff, 7, 14)
					.Fields("Description").Value = "????"
					.Fields("ScenarioID").Value = scnID
					.Fields("OpnTypID").Value = OpLoc.Ind
					OpLoc.SegID(OpLoc.Count) = .Fields("id").Value
					.Update()
				End With
			End If
		Loop While retcod = 1 Or retcod = 2
		
		mySeg.Close()
		
	End Sub
	
	Sub GetTableDetails(ByRef i As Integer)
		Dim retid, kflg, Init, contfg, Ind As Integer
		Dim kwd As String
		Dim retcod, initb, tabno, uunits, retkey, Occur As Integer
		Dim cbuff As String
		Dim k, opL, opF, j, s As Integer
		Dim tmp As String
		Dim crit As String
		Dim pd() As ParmDetail
		Dim pc As Integer
		'UPGRADE_WARNING: Arrays in structure myParmData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim myParmData As DAO.Recordset
		'UPGRADE_WARNING: Arrays in structure myParmDefn may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim myParmDefn As DAO.Recordset
		'UPGRADE_WARNING: Arrays in structure myParmTableDefn may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim myParmTableDefn As DAO.Recordset
		'UPGRADE_WARNING: Arrays in structure mySeg may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim mySeg As DAO.Recordset
		
		myParmData = myDb.OpenRecordset("ParmData", DAO.RecordsetTypeEnum.dbOpenDynaset)
		myParmDefn = myDb.OpenRecordset("ParmDefn", DAO.RecordsetTypeEnum.dbOpenDynaset)
		myParmTableDefn = myDb.OpenRecordset("ParmTableDefn", DAO.RecordsetTypeEnum.dbOpenDynaset)
		
		Init = 1
		Ind = i + 120
		Do 
			Call F90_GTNXKW(Init, Ind, kwd, kflg, contfg, retid)
			If kflg <> 0 Then
				'frmMain.lblInfo.Caption = kwd
				System.Windows.Forms.Application.DoEvents()
				crit = "Name = '" & kwd & "' AND OpnTypID = " & i
				myParmTableDefn.FindFirst(crit)
				
				With myParmDefn
					crit = "ParmTableID = " & myParmTableDefn.Fields("id").Value
					.FindFirst(crit)
					pc = 0
					Do 
						pc = pc + 1
						ReDim Preserve pd(pc)
						pd(pc).name = .Fields("name").Value
						pd(pc).StartCol = .Fields("StartCol").Value
						pd(pc).Width = .Fields("Width").Value
						'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
						If IsDbNull(.Fields("Def").Value) Then
							pd(pc).Def = ""
						Else
							pd(pc).Def = .Fields("Def").Value
						End If
						pd(pc).id = .Fields("id").Value
						.FindNext(crit)
					Loop While Not (.NoMatch)
				End With
				initb = 1
				tabno = retid - (i * 1000)
				uunits = 1
				Occur = 1
				Do 
					Call F90_XTABLE(Ind, tabno, uunits, initb, CInt(0), Occur, retkey, cbuff, retcod)
					initb = 0
					If retcod = 2 Then 'process me
						'Debug.Print cbuff
						opF = CInt(Left(cbuff, 5))
						tmp = Trim(Mid(cbuff, 6, 5))
						If Len(tmp) = 0 Then
							opL = opF
						Else
							opL = CInt(tmp)
						End If
						For j = 1 To OpDet(i).Count
							s = OpDet(i).SegNum(j)
							If (opF <= s) And (opL >= s) Then
								For k = 1 To pc
									With myParmData
										.AddNew()
										.Fields("ParmID").Value = pd(k).id
										.Fields("SegID").Value = OpDet(i).SegID(j)
										tmp = Trim(Mid(cbuff, pd(k).StartCol, pd(k).Width))
										If Len(tmp) = 0 Then
											tmp = pd(k).Def
											If Len(tmp) = 0 Then
												tmp = " "
											End If
										End If
										.Fields("Occur").Value = Occur
										.Fields("Value").Value = tmp
										.Update()
									End With
									If pd(k).name = "LSID" Or pd(k).name = "RCHID" Then
										crit = "ID = " & OpDet(i).SegID(j)
										'MsgBox pd(k).Name & " is " & tmp & " find " & crit
										mySeg = myDb.OpenRecordset("SegData", DAO.RecordsetTypeEnum.dbOpenDynaset)
										With mySeg
											.FindFirst(crit)
											.Edit()
											.Fields("Description").Value = tmp
											.Update()
											.Close()
										End With
									End If
								Next k
							End If
						Next j
					End If
					If retcod = 10 Then 'look for more occurances of this table
						retcod = 1
						Occur = Occur + 1
						initb = 1
					End If
				Loop While retcod = 1 Or retcod = 2
			End If
			Init = 0
		Loop While contfg = 1
		
		myParmData.Close()
		myParmDefn.Close()
		myParmTableDefn.Close()
		
	End Sub
	
	Sub UpdateNumberSegs()
		'UPGRADE_WARNING: Arrays in structure myOpnCnt may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim myOpnCnt As DAO.Recordset
		Dim crit As String
		Dim ityp, cnt As Integer
		'UPGRADE_WARNING: Arrays in structure mySeg may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim mySeg As DAO.Recordset
		
		'update number of segs
		myOpnCnt = myDb.OpenRecordset("CountOpnTypes", DAO.RecordsetTypeEnum.dbOpenDynaset)
		mySeg = myDb.OpenRecordset("ScenarioData", DAO.RecordsetTypeEnum.dbOpenDynaset)
		
		While Not (mySeg.EOF)
			crit = "ID = " & mySeg.Fields(0).Value
			myOpnCnt.FindFirst(crit)
			mySeg.Edit()
			While Not (myOpnCnt.NoMatch)
				ityp = myOpnCnt.Fields("OpnTypID").Value
				cnt = myOpnCnt.Fields("CountOfOpnTypID").Value
				If ityp = 3 Then 'rchres(3)
					mySeg.Fields("NumReaches").Value = cnt
				Else 'perlnd(1) or implnd(2)
					'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
					If IsDbNull(mySeg.Fields("NumSegments").Value) Then
						mySeg.Fields("NumSegments").Value = cnt
					Else
						mySeg.Fields("NumSegments").Value = cnt + mySeg.Fields("NumSegments").Value
					End If
				End If
				myOpnCnt.FindNext(crit)
			End While
			mySeg.Update()
			mySeg.MoveNext()
		End While
		mySeg.Close()
	End Sub
End Module