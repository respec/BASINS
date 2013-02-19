Option Strict Off
Option Explicit On
Module HSPFParmDBCreate
	
	Sub BuildHSPFParmDB() ' fill in tables on blank db
		
		myTabDef = myDb.CreateTableDef("OpnTypDefn")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Attributes = DAO.FieldAttributeEnum.dbAutoIncrField
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 8))
		End With
		myDb.TableDefs.Append(myTabDef)
		myRec = myDb.OpenRecordset("OpnTypDefn", DAO.RecordsetTypeEnum.dbOpenDynaset)
		With myRec
			.AddNew()
			.Fields("name").Value = "PERLND"
			.Update()
			.AddNew()
			.Fields("name").Value = "IMPLND"
			.Update()
			.AddNew()
			.Fields("name").Value = "RCHRES"
			.Update()
		End With
		
		myTabDef = myDb.CreateTableDef("ParmTypeDefn")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Attributes = DAO.FieldAttributeEnum.dbAutoIncrField
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 4))
		End With
		myDb.TableDefs.Append(myTabDef)
		myRec = myDb.OpenRecordset("ParmTypeDefn", DAO.RecordsetTypeEnum.dbOpenDynaset)
		With myRec
			.AddNew()
			.Fields("name").Value = "Char"
			.Update()
			.AddNew()
			.Fields("name").Value = "Long"
			.Update()
			.AddNew()
			.Fields("name").Value = "Real"
			.Update()
			.AddNew()
			.Fields("name").Value = "Dble"
			.Update()
			.Close()
		End With
		
		myTabDef = myDb.CreateTableDef("ParmData")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Attributes = DAO.FieldAttributeEnum.dbAutoIncrField
			myIndx = .CreateIndex("iParmID")
			With myIndx
				.name = "iParmID"
				.Unique = False
				.Primary = False
				.Fields = "ParmID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ParmID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Required = True
			.Fields.Append(.CreateField("SegID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Required = True
			.Fields.Append(.CreateField("Occur", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Value", DAO.DataTypeEnum.dbText, 20))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		myTabDef = myDb.CreateTableDef("ParmDefn")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Attributes = DAO.FieldAttributeEnum.dbAutoIncrField
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
			.Fields.Append(.CreateField("Assoc", DAO.DataTypeEnum.dbText, 16))
			.Fields.Append(.CreateField("AssocID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("ParmTypeID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Required = True
			.Fields.Append(.CreateField("ParmTableID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Required = True
			.Fields.Append(.CreateField("Min", DAO.DataTypeEnum.dbText, 20))
			.Fields.Append(.CreateField("Max", DAO.DataTypeEnum.dbText, 20))
			.Fields.Append(.CreateField("Def", DAO.DataTypeEnum.dbText, 20))
			.Fields.Append(.CreateField("MetMin", DAO.DataTypeEnum.dbText, 20))
			.Fields.Append(.CreateField("MetMax", DAO.DataTypeEnum.dbText, 20))
			.Fields.Append(.CreateField("MetDef", DAO.DataTypeEnum.dbText, 20))
			.Fields.Append(.CreateField("StartCol", DAO.DataTypeEnum.dbInteger))
			.Fields.Append(.CreateField("Width", DAO.DataTypeEnum.dbInteger))
			.Fields.Append(.CreateField("Definition", DAO.DataTypeEnum.dbText, 255))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		myTabDef = myDb.CreateTableDef("ParmTableDefn")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Attributes = DAO.FieldAttributeEnum.dbAutoIncrField
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
			.Fields.Append(.CreateField("OpnTypID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Required = True
			.Fields.Append(.CreateField("Alias", DAO.DataTypeEnum.dbBoolean))
			.Fields.Append(.CreateField("TableNumber", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Definition", DAO.DataTypeEnum.dbText, 255))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		myTabDef = myDb.CreateTableDef("TableAliasDefn")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Attributes = DAO.FieldAttributeEnum.dbAutoIncrField
			.Fields.Append(.CreateField("OpnTypID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
			.Fields.Append(.CreateField("Occur", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("AppearName", DAO.DataTypeEnum.dbText, 20))
			.Fields.Append(.CreateField("IDVarName", DAO.DataTypeEnum.dbText, 8))
			.Fields.Append(.CreateField("IDVar", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("SubsKeyName", DAO.DataTypeEnum.dbText, 8))
			.Fields.Append(.CreateField("IDSubs", DAO.DataTypeEnum.dbLong))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		BuildHSPFTable()
		
		'Call frmWat.BuildWatTable
		'Call frmScn.BuildScnTable
		
		myTabDef = myDb.CreateTableDef("SegData")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Attributes = DAO.FieldAttributeEnum.dbAutoIncrField
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 24))
			.Fields.Append(.CreateField("Description", DAO.DataTypeEnum.dbText, 255))
			.Fields.Append(.CreateField("OpnTypID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Required = True
			.Fields.Append(.CreateField("ScenarioID", DAO.DataTypeEnum.dbLong))
			.Fields(.Fields.Count - 1).Required = True
		End With
		myDb.TableDefs.Append(myTabDef)
		
		Dim myRel As DAO.Relation
		myRel = myDb.CreateRelation("OperationName", "OpnTypDefn", "SegData", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "OpnTypID"
		myDb.Relations.Append(myRel)
		
		myRel = myDb.CreateRelation("OpnTypDefnTableAliasDefn", "OpnTypDefn", "TableAliasDefn", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "OpnTypID"
		myDb.Relations.Append(myRel)
		
		myRel = myDb.CreateRelation("ParmDefnTableAliasDefn", "ParmDefn", "TableAliasDefn", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "IDVar"
		myDb.Relations.Append(myRel)
		
		'Set myRel = myDB.CreateRelation("TableAliasDefnParmTableDefn", _
		''  "TableAliasDefn", "ParmTableDefn", dbRelationUpdateCascade)
		'myRel.Fields.Append myRel.CreateField("Name")
		'myRel.Fields!id.ForeignName = "Name"
		'myDB.Relations.Append myRel
		
		myRel = myDb.CreateRelation("ScenarioName", "ScenarioData", "SegData", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "ScenarioID"
		myDb.Relations.Append(myRel)
		
		myRel = myDb.CreateRelation("ProjectName", "WatershedData", "ScenarioData", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "WatershedID"
		myDb.Relations.Append(myRel)
		
		myRel = myDb.CreateRelation("ParameterTypeName", "ParmTypeDefn", "ParmDefn", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "ParmTypeID"
		myDb.Relations.Append(myRel)
		
		myRel = myDb.CreateRelation("ParameterTableName", "ParmTableDefn", "ParmDefn", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "ParmTableID"
		myDb.Relations.Append(myRel)
		
		myRel = myDb.CreateRelation("ParmName", "ParmDefn", "ParmData", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "ParmID"
		myDb.Relations.Append(myRel)
		
		myRel = myDb.CreateRelation("Name", "SegData", "ParmData", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "SegID"
		myDb.Relations.Append(myRel)
		
		myRel = myDb.CreateRelation("ParmOperationName", "OpnTypDefn", "ParmTableDefn", DAO.RelationAttributeEnum.dbRelationUpdateCascade)
		myRel.Fields.Append(myRel.CreateField("ID"))
		myRel.Fields.Item("id").ForeignName = "OpnTypID"
		myDb.Relations.Append(myRel)
		
		mySQL = "UPDATE DISTINCTROW TableAliasDefn INNER JOIN ParmTableDefn ON " & "(TableAliasDefn.OpnTypID = ParmTableDefn.OpnTypID) " & "AND (TableAliasDefn.Name = ParmTableDefn.Name) " & "SET ParmTableDefn.Alias = Yes;"
		myQuery = myDb.CreateQueryDef("ParmTableAliasAvailable", mySQL)
		myQuery.Execute()
		
		mySQL = "SELECT DISTINCTROW SegData.ID AS SegID, " & "ParmTableDefn.Name, " & "ParmTableDefn.ID AS TabID,  " & "ParmTableDefn.OpnTypID  " & "FROM (ScenarioData INNER JOIN SegData ON ScenarioData.ID = SegData.ScenarioID)  " & "INNER JOIN ((ParmTableDefn  " & "INNER JOIN ParmDefn ON ParmTableDefn.ID = ParmDefn.ParmTableID) " & "INNER JOIN ParmData ON ParmDefn.ID = ParmData.ParmID) ON SegData.ID = ParmData.SegID;"
		myQuery = myDb.CreateQueryDef("ScenTableList", mySQL)
		
		mySQL = "SELECT DISTINCTROW SegData.ID AS SegID, " & "ParmDefn.Name, " & "ParmData.Value, " & "ParmDefn.ID AS ParmID, " & "ParmDefn.AssocID AS AssocID, " & "ParmDefn.ParmTableID AS TabID, " & "ParmTableDefn.OpnTypID, " & "ParmTableDefn.Name AS [Table], " & "ParmData.Occur, " & "IIf([ParmTableDefn]![Alias]=0,' ','Alias') AS AliasInfo " & "FROM (ScenarioData INNER JOIN SegData ON ScenarioData.ID = SegData.ScenarioID) " & "INNER JOIN ((ParmTableDefn " & "INNER JOIN ParmDefn ON ParmTableDefn.ID = ParmDefn.ParmTableID) " & "INNER JOIN ParmData ON ParmDefn.ID = ParmData.ParmID) ON SegData.ID = ParmData.SegID;"
		
		myQuery = myDb.CreateQueryDef("ParmTableData", mySQL)
		
		mySQL = "SELECT DISTINCTROW ParmDefn.Name, " & "ParmDefn.ID,  " & "ParmTableDefn.ID AS TabID, " & "ParmTableDefn.Name AS TabName, " & "ParmTableDefn.OpnTypID,  " & "ParmTypeDefn.Name AS ParmType,  " & "ParmDefn.Def, " & "ParmDefn.Min, " & "ParmDefn.Max, " & "ParmDefn.metDef, " & "ParmDefn.metMin, " & "ParmDefn.metMax, " & "ParmDefn.StartCol, " & "ParmDefn.Width " & "FROM ParmTypeDefn INNER JOIN (ParmTableDefn  " & "INNER JOIN ParmDefn ON ParmTableDefn.ID = ParmDefn.ParmTableID) ON ParmTypeDefn.ID = ParmDefn.ParmTypeID  " & "ORDER BY ParmDefn.ID, ParmTableDefn.ID;"
		myQuery = myDb.CreateQueryDef("ParmTableList", mySQL)
		
		mySQL = "SELECT DISTINCTROW OpnTypDefn.Name AS OpnType, " & "ParmDefn.Name AS ParmName, " & "ParmData.Value " & "FROM (TableAliasDefn INNER JOIN (ParmDefn " & "INNER JOIN ParmData ON ParmDefn.ID = ParmData.ParmID) ON TableAliasDefn.IDVar = ParmDefn.ID) INNER JOIN OpnTypDefn ON TableAliasDefn.OpnTypID = OpnTypDefn.ID " & "GROUP BY TableAliasDefn.OpnTypID, OpnTypDefn.Name, ParmDefn.Name, ParmData.Value, TableAliasDefn.IDVar " & "ORDER BY TableAliasDefn.OpnTypID, ParmDefn.Name, ParmData.Value;"
		
		myQuery = myDb.CreateQueryDef("UniqueName", mySQL)
		
		mySQL = "SELECT DISTINCTROW OpnTypDefn.Name AS OpnType, " & "ParmTableDefn.Name AS [Table], " & "ParmDefn.Name AS Parm, " & "WatershedData.WatershedName AS Watershed, " & "ScenarioData.Name AS Scenario, " & "SegData.Name AS Segment, " & "ParmData.Occur, " & "ParmData.Value " & "FROM ((WatershedData INNER JOIN ScenarioData ON WatershedData.ID = ScenarioData.WatershedID) " & "INNER JOIN SegData ON ScenarioData.ID = SegData.ScenarioID) " & "INNER JOIN (OpnTypDefn " & "INNER JOIN ((ParmTableDefn " & "INNER JOIN ParmDefn ON ParmTableDefn.ID = ParmDefn.ParmTableID) " & "INNER JOIN ParmData ON ParmDefn.ID = ParmData.ParmID) ON OpnTypDefn.ID = ParmTableDefn.OpnTypID) ON SegData.ID = ParmData.SegID " & "ORDER BY ParmDefn.ParmTableID, ParmData.ParmID, ParmData.Occur, WatershedData.WatershedName, ScenarioData.Name, SegData.Name;"
		myQuery = myDb.CreateQueryDef("ParmListAll", mySQL)
		
		mySQL = "SELECT DISTINCTROW ScenarioData.ID, " & "ScenarioData.Name, " & "SegData.OpnTypID, " & "Count(SegData.OpnTypID) AS CountOfOpnTypID " & "FROM ScenarioData INNER JOIN SegData ON ScenarioData.ID = SegData.ScenarioID " & "GROUP BY ScenarioData.ID, ScenarioData.Name, SegData.OpnTypID;"
		myQuery = myDb.CreateQueryDef("CountOpnTypes", mySQL)
		
	End Sub
	
	
	Sub BuildHSPFTable()
		
        Dim isect, offset, retid, kflg, Init, contfg, opnid, estflg, irept As Integer
        Dim kwd As String = ""
		Dim tabno, omcode, uunits As Integer
		'Retrieved in a call to F90_XTINFO to get column info for grid
		Dim lnflds As Integer
		Dim lscol(30) As Integer
		Dim lflen(30) As Integer
        Dim lftyp As String = ""
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
        Dim Assoc As String = ""
        Dim snam, crit As String
		Dim i, AssocID As Integer
		
		Dim olen, gnum, initfg, cont As Integer
        Dim obuff As String = ""
		Dim Occur As Integer
        Dim OName As String
		
        Dim myParmDefn As DAO.Recordset
        Dim myParmTableDefn As DAO.Recordset
		myParmDefn = myDb.OpenRecordset("ParmDefn", DAO.RecordsetTypeEnum.dbOpenDynaset)
		myParmTableDefn = myDb.OpenRecordset("ParmTableDefn", DAO.RecordsetTypeEnum.dbOpenDynaset)
		
		ChDir("UciWdm")
		snam = "test04" 'dummy, need to save fwdm and fmsg in f90 module
		Call F90_ACTSCN(CInt(-1), fwdm, fmsg, retid, snam, Len(snam))
		
		omcode = 120
		uunits = 1
		Do While omcode < 123 'more operation types
			omcode = omcode + 1
			Init = 1
			'get table info
			Do  'while more tables
				Call F90_GTNXKW(Init, omcode, kwd, kflg, contfg, tabno)
				If Init = 1 Then
					Init = 0
				End If
				If tabno > 0 Then
					opnid = omcode - 120
					tabno = tabno - (opnid * 1000)
					With myParmTableDefn
						.AddNew()
						.Fields("name").Value = kwd
						.Fields("OpnTypID").Value = opnid
						.Fields("TableNumber").Value = tabno
						ParmTableID = .Fields("id").Value
						.Update()
					End With
					'call first to get metric, then english
					Call F90_XTINFO(omcode, tabno, 2, estflg, lnflds, lscol, lflen, lftyp, lapos, limetmin, limetmax, limetdef, lrmetmin, lrmetmax, lrmetdef, lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
					Call F90_XTINFO(omcode, tabno, uunits, estflg, lnflds, lscol, lflen, lftyp, lapos, limin, limax, lidef, lrmin, lrmax, lrdef, lnmhdr, hdrbuf, lfdnam, isect, irept, retid)
					Call F90_WMSGTW(CInt(1), Assoc)
					If Len(Assoc) > 0 Then
						With myParmDefn
							crit = "Name = '" & Assoc & "'"
							.FindFirst(crit)
							AssocID = .Fields(0).Value
						End With
					Else
						Assoc = " "
						AssocID = 0
					End If
					If lscol(1) = 9 Then
						offset = 2
					Else
						offset = 0
					End If
					For i = 1 To lnflds - 1
						With myParmDefn
							.AddNew()
							.Fields("name").Value = lfdnam(i)
							.Fields("Assoc").Value = Assoc
							.Fields("AssocID").Value = AssocID
							ParmTableType = ParmType(Mid(lftyp, i + 1, 1))
							.Fields("ParmTypeID").Value = ParmTableType
							.Fields("ParmTableID").Value = ParmTableID
							.Fields("StartCol").Value = lscol(i) + offset
							.Fields("Width").Value = lflen(i)
							If ParmTableType = 2 Then ' integer
								.Fields("Min").Value = ChkNone(CStr(limin(lapos(i) - 1)))
								.Fields("Max").Value = ChkNone(CStr(limax(lapos(i) - 1)))
								.Fields("Def").Value = ChkNone(CStr(lidef(lapos(i) - 1)))
								.Fields("metMin").Value = ChkNone(CStr(limetmin(lapos(i) - 1)))
								.Fields("metMax").Value = ChkNone(CStr(limetmax(lapos(i) - 1)))
								.Fields("metDef").Value = ChkNone(CStr(limetdef(lapos(i) - 1)))
							ElseIf ParmTableType = 3 Then  'real
								.Fields("Min").Value = ChkNone(CStr(lrmin(lapos(i) - 1)))
								.Fields("Max").Value = ChkNone(CStr(lrmax(lapos(i) - 1)))
								.Fields("Def").Value = ChkNone(CStr(lrdef(lapos(i) - 1)))
								.Fields("metMin").Value = ChkNone(CStr(lrmetmin(lapos(i) - 1)))
								.Fields("metMax").Value = ChkNone(CStr(lrmetmax(lapos(i) - 1)))
								.Fields("metDef").Value = ChkNone(CStr(lrmetdef(lapos(i) - 1)))
							End If
							.Update()
						End With
					Next i
				ElseIf contfg = 0 Then 
					Exit Do
				End If
			Loop 
		Loop 
		
		'lblInfo.Caption = myParmDefn.RecordCount & " " & myParmTableDefn.RecordCount
		
		myParmDefn.Close()
		myParmTableDefn.Close()
		
		myRec = myDb.OpenRecordset("ParmDefn", DAO.RecordsetTypeEnum.dbOpenDynaset)
        Dim myTableAliasDefn As DAO.Recordset
		myTableAliasDefn = myDb.OpenRecordset("TableAliasDefn", DAO.RecordsetTypeEnum.dbOpenDynaset)
		With myTableAliasDefn
			gnum = 10
			omcode = 120
			Do While omcode < 123 'more operation types
				omcode = omcode + 1
				initfg = 1
				cont = 1
				opnid = omcode - 120
				OName = ""
				Do While cont <> 0
					olen = 80
					Call F90_WMSGTT(fmsg, omcode, gnum, initfg, olen, cont, obuff)
					initfg = 0
					.AddNew()
					.Fields("OpnTypID").Value = opnid
					.Fields("name").Value = Left(obuff, 12)
					If OName <> .Fields("name").Value Then
						Occur = 1
						OName = .Fields("name").Value
					Else
						Occur = Occur + 1
					End If
					.Fields("Occur").Value = Occur
					If Len(obuff) > 14 Then
						.Fields("AppearName").Value = Mid(obuff, 15, 20)
					Else
						.Fields("AppearName").Value = " "
					End If
					If Len(obuff) > 36 Then
						.Fields("IDVarName").Value = Mid(obuff, 37, 6)
						crit = "Name = '" & .Fields("IDVarName").Value & "'"
						myRec.FindFirst(crit)
						.Fields("IDVar").Value = myRec.Fields("id").Value
					Else
						.Fields("IDVarName").Value = " "
					End If
					If Len(obuff) > 44 Then
						.Fields("SubsKeyName").Value = Mid(obuff, 45, 6)
						crit = "Name = '" & .Fields("SubsKeyName").Value & "'"
						myRec.FindFirst(crit)
						.Fields("IDSubs").Value = myRec.Fields("id").Value
					Else
						.Fields("SubsKeyName").Value = " "
					End If
					.Update()
				Loop 
			Loop 
		End With
		myTableAliasDefn.Close()
		myRec.Close()
		
		ChDir("..")
		
	End Sub
	
	Private Function ChkNone(ByRef s As String) As String
		If s = "-999" Then
			ChkNone = "<none>"
		Else
			ChkNone = s
		End If
	End Function
	
	Private Function ParmType(ByRef s As String) As Integer
		
		Dim i As Integer
		
		If s = "C" Then 'character
			i = 1
		ElseIf s = "I" Then  'long integer
			i = 2
		Else 'real
			i = 3
		End If
		ParmType = i
		
	End Function
End Module