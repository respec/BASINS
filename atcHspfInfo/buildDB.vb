Option Strict Off
Option Explicit On
Module buildDB
	Public myDb As DAO.Database
	Public myTabDef As DAO.TableDef
	Public myIndx As DAO.Index
    Public myRec As DAO.Recordset
	Public mySQL As String
	Public myQuery As DAO.QueryDef
	
	Sub BuildHspMsgDB() ' fill in tables on blank db
		Kill("HspfMsg.mdb")
		myDb = DAODBEngine_definst.CreateDatabase("HspfMsg.mdb", DAO.LanguageConstants.dbLangGeneral, 0)
		
		myTabDef = myDb.CreateTableDef("BlockDefns")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.Name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		myTabDef = myDb.CreateTableDef("TableDefns")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.Name = "iID"
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("SectionID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
			.Fields.Append(.CreateField("SGRP", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("NumOccur", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("HeaderE", DAO.DataTypeEnum.dbMemo))
			.Fields.Append(.CreateField("HeaderM", DAO.DataTypeEnum.dbMemo))
			.Fields.Append(.CreateField("Help", DAO.DataTypeEnum.dbMemo))
			.Fields.Append(.CreateField("OccurGroupID", DAO.DataTypeEnum.dbLong))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		myTabDef = myDb.CreateTableDef("ParmDefns")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.Name = "iID"
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			myIndx = .CreateIndex("iTableID")
			With myIndx
				.Name = "iTableID"
				.Unique = False
				.Primary = False
				.Fields = "TableID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("TableID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
			.Fields.Append(.CreateField("Type", DAO.DataTypeEnum.dbText, 1))
			.Fields.Append(.CreateField("StartCol", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Length", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Minimum", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("Maximum", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("Default", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("Help", DAO.DataTypeEnum.dbMemo))
			.Fields.Append(.CreateField("MetricMinimum", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("MetricMaximum", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("MetricDefault", DAO.DataTypeEnum.dbSingle))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		myTabDef = myDb.CreateTableDef("SectionDefns")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.Name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("BlockID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		
		'mySQL = "SELECT [BlockDefns].[Name] AS BlockName, " & _
		''               "[SectionDefns].[Name], " & _
		''               "[TableDefns].[Name] AS TableName, " & _
		''              "[ParmDefns].[Name] AS ParmName " & _
		''         "FROM ((ParmDefns INNER JOIN TableDefns ON " & _
		''               "[ParmDefns].[TableID] = [TableDefns].[ID]) " & _
		''               "INNER JOIN SectionDefns ON " & _
		''               "([TableDefns].[BlockID] = [SectionDefns].[BlockID]) " & _
		''               "AND ([TableDefns].[SectionID] = [SectionDefns].[ID])) " & _
		''               "INNER JOIN BlockDefns ON " & _
		''               "[SectionDefns].[BlockID] = [BlockDefns].[ID] " & _
		''      "ORDER BY [ParmDefns].[ID];"
		
		
		mySQL = "SELECT [BlockDefns].[Name], " & "[SectionDefns].[Name], " & "[TableDefns].[Name], " & "[ParmDefns].[Name] " & "FROM ((BlockDefns INNER JOIN SectionDefns ON " & "[BlockDefns].[ID] = [SectionDefns].[BlockID]) " & "INNER JOIN TableDefns ON " & "[SectionDefns].[ID] = [TableDefns].[SectionID]) " & "INNER JOIN ParmDefns ON " & "[TableDefns].[ID] = [ParmDefns].[TableID] " & "ORDER BY [ParmDefns].[ID];"
		
		myQuery = myDb.CreateQueryDef("BlockSectionTableParm", mySQL)
		
		mySQL = "SELECT TableDefns.ID, BlockDefns.ID " & "FROM (TableDefns INNER JOIN SectionDefns ON " & "TableDefns.SectionID = SectionDefns.ID) " & "INNER JOIN BlockDefns ON SectionDefns.BlockID = BlockDefns.ID;"
		myQuery = myDb.CreateQueryDef("BlockIDFromTableID", mySQL)
		
		
		
		myTabDef = myDb.CreateTableDef("TSGroupDefns")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.Name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("BlockID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		myTabDef = myDb.CreateTableDef("TSMemberDefns")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.Name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("TSGroupID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
			.Fields.Append(.CreateField("SCLU", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("SGRP", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("mdim1", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("mdim2", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("maxsb1", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("maxsb2", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("mkind", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("sptrn", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("msect", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("mio", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("osvbas", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("osvoff", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("eunits", DAO.DataTypeEnum.dbText, 8))
			.Fields.Append(.CreateField("ltval1", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("ltval2", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("ltval3", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("ltval4", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("defn", DAO.DataTypeEnum.dbText, 43))
			.Fields.Append(.CreateField("munits", DAO.DataTypeEnum.dbText, 8))
			.Fields.Append(.CreateField("ltval5", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("ltval6", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("ltval7", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("ltval8", DAO.DataTypeEnum.dbSingle))
		End With
		myDb.TableDefs.Append(myTabDef)
		
		mySQL = "SELECT [BlockDefns].[Name], " & "[TSGroupDefns].[Name], " & "[TSMemberDefns].[Name] " & "FROM (TSMemberDefns INNER JOIN TSGroupDefns ON " & "[TSMemberDefns].[TSGroupID] = [TSGroupDefns].[ID]) " & "INNER JOIN BlockDefns ON " & "[TSGroupDefns].[BlockID] = [BlockDefns].[ID] " & "ORDER BY [TSMemberDefns].[ID];"
		
		myQuery = myDb.CreateQueryDef("BlockTSGroupTSMember", mySQL)
		
		myTabDef = myDb.CreateTableDef("TableAliasDefn")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.Name = "iID"
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
		
		myTabDef = myDb.CreateTableDef("WDMAttribDefns")
		With myTabDef
			myIndx = .CreateIndex("iID")
			With myIndx
				.Name = "iID"
				.Unique = True
				.Primary = True
				.Fields = "ID"
			End With
			.Indexes.Append(myIndx)
			.Fields.Append(.CreateField("ID", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Name", DAO.DataTypeEnum.dbText, 16))
			.Fields.Append(.CreateField("Length", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Type", DAO.DataTypeEnum.dbLong))
			.Fields.Append(.CreateField("Desc", DAO.DataTypeEnum.dbText, 47))
			.Fields.Append(.CreateField("Min", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("Max", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("Def", DAO.DataTypeEnum.dbSingle))
			.Fields.Append(.CreateField("Valid", DAO.DataTypeEnum.dbText, 240))
		End With
		myDb.TableDefs.Append(myTabDef)
		
	End Sub
End Module