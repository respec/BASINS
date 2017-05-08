Attribute VB_Name = "buildDB"
Option Explicit
Public myDb As Database
Public myTabDef As TableDef
Public myIndx As Index
Public myRec As Recordset
Public mySQL As String
Public myQuery As QueryDef

Sub BuildHspMsgDB() ' fill in tables on blank db
  Kill "HspfMsg.mdb"
  Set myDb = CreateDatabase("HspfMsg.mdb", dbLangGeneral, 0)
  
  Set myTabDef = myDb.CreateTableDef("BlockDefns")
  With myTabDef
    Set myIndx = .CreateIndex("iID")
    With myIndx
      .Name = "iID"
      .Unique = True
      .Primary = True
      .Fields = "ID"
    End With
    .Indexes.Append myIndx
    .Fields.Append .CreateField("ID", dbLong)
    .Fields.Append .CreateField("Name", dbText, 16)
  End With
  myDb.TableDefs.Append myTabDef
    
  Set myTabDef = myDb.CreateTableDef("TableDefns")
  With myTabDef
    Set myIndx = .CreateIndex("iID")
    With myIndx
      .Name = "iID"
      .Primary = True
      .Fields = "ID"
    End With
    .Indexes.Append myIndx
    .Fields.Append .CreateField("ID", dbLong)
    .Fields.Append .CreateField("SectionID", dbLong)
    .Fields.Append .CreateField("Name", dbText, 16)
    .Fields.Append .CreateField("SGRP", dbLong)
    .Fields.Append .CreateField("NumOccur", dbLong)
    .Fields.Append .CreateField("HeaderE", dbMemo)
    .Fields.Append .CreateField("HeaderM", dbMemo)
    .Fields.Append .CreateField("Help", dbMemo)
    .Fields.Append .CreateField("OccurGroupID", dbLong)
  End With
  myDb.TableDefs.Append myTabDef
  
  Set myTabDef = myDb.CreateTableDef("ParmDefns")
  With myTabDef
    Set myIndx = .CreateIndex("iID")
    With myIndx
      .Name = "iID"
      .Primary = True
      .Fields = "ID"
    End With
    .Indexes.Append myIndx
    .Fields.Append .CreateField("ID", dbLong)
    Set myIndx = .CreateIndex("iTableID")
    With myIndx
      .Name = "iTableID"
      .Unique = False
      .Primary = False
      .Fields = "TableID"
    End With
    .Indexes.Append myIndx
    .Fields.Append .CreateField("TableID", dbLong)
    .Fields.Append .CreateField("Name", dbText, 16)
    .Fields.Append .CreateField("Type", dbText, 1)
    .Fields.Append .CreateField("StartCol", dbLong)
    .Fields.Append .CreateField("Length", dbLong)
    .Fields.Append .CreateField("Minimum", dbSingle)
    .Fields.Append .CreateField("Maximum", dbSingle)
    .Fields.Append .CreateField("Default", dbSingle)
    .Fields.Append .CreateField("Help", dbMemo)
    .Fields.Append .CreateField("MetricMinimum", dbSingle)
    .Fields.Append .CreateField("MetricMaximum", dbSingle)
    .Fields.Append .CreateField("MetricDefault", dbSingle)
  End With
  myDb.TableDefs.Append myTabDef
  
  Set myTabDef = myDb.CreateTableDef("SectionDefns")
  With myTabDef
    Set myIndx = .CreateIndex("iID")
    With myIndx
      .Name = "iID"
      .Unique = True
      .Primary = True
      .Fields = "ID"
    End With
    .Indexes.Append myIndx
    .Fields.Append .CreateField("ID", dbLong)
    .Fields.Append .CreateField("BlockID", dbLong)
    .Fields.Append .CreateField("Name", dbText, 16)
  End With
  myDb.TableDefs.Append myTabDef
  
 
  'mySQL = "SELECT [BlockDefns].[Name] AS BlockName, " & _
  '               "[SectionDefns].[Name], " & _
  '               "[TableDefns].[Name] AS TableName, " & _
   '              "[ParmDefns].[Name] AS ParmName " & _
  '         "FROM ((ParmDefns INNER JOIN TableDefns ON " & _
  '               "[ParmDefns].[TableID] = [TableDefns].[ID]) " & _
  '               "INNER JOIN SectionDefns ON " & _
  '               "([TableDefns].[BlockID] = [SectionDefns].[BlockID]) " & _
  '               "AND ([TableDefns].[SectionID] = [SectionDefns].[ID])) " & _
  '               "INNER JOIN BlockDefns ON " & _
  '               "[SectionDefns].[BlockID] = [BlockDefns].[ID] " & _
  '      "ORDER BY [ParmDefns].[ID];"
        
        
  mySQL = "SELECT [BlockDefns].[Name], " & _
                 "[SectionDefns].[Name], " & _
                 "[TableDefns].[Name], " & _
                 "[ParmDefns].[Name] " & _
           "FROM ((BlockDefns INNER JOIN SectionDefns ON " & _
                 "[BlockDefns].[ID] = [SectionDefns].[BlockID]) " & _
                 "INNER JOIN TableDefns ON " & _
                 "[SectionDefns].[ID] = [TableDefns].[SectionID]) " & _
                 "INNER JOIN ParmDefns ON " & _
                 "[TableDefns].[ID] = [ParmDefns].[TableID] " & _
        "ORDER BY [ParmDefns].[ID];"
        
  Set myQuery = myDb.CreateQueryDef("BlockSectionTableParm", mySQL)
  
  mySQL = "SELECT TableDefns.ID, BlockDefns.ID " & _
           "FROM (TableDefns INNER JOIN SectionDefns ON " & _
                 "TableDefns.SectionID = SectionDefns.ID) " & _
                 "INNER JOIN BlockDefns ON SectionDefns.BlockID = BlockDefns.ID;"
  Set myQuery = myDb.CreateQueryDef("BlockIDFromTableID", mySQL)
                 

  
  Set myTabDef = myDb.CreateTableDef("TSGroupDefns")
  With myTabDef
    Set myIndx = .CreateIndex("iID")
    With myIndx
      .Name = "iID"
      .Unique = True
      .Primary = True
      .Fields = "ID"
    End With
    .Indexes.Append myIndx
    .Fields.Append .CreateField("ID", dbLong)
    .Fields.Append .CreateField("BlockID", dbLong)
    .Fields.Append .CreateField("Name", dbText, 16)
  End With
  myDb.TableDefs.Append myTabDef
  
  Set myTabDef = myDb.CreateTableDef("TSMemberDefns")
  With myTabDef
    Set myIndx = .CreateIndex("iID")
    With myIndx
      .Name = "iID"
      .Unique = True
      .Primary = True
      .Fields = "ID"
    End With
    .Indexes.Append myIndx
    .Fields.Append .CreateField("ID", dbLong)
    .Fields.Append .CreateField("TSGroupID", dbLong)
    .Fields.Append .CreateField("Name", dbText, 16)
    .Fields.Append .CreateField("SCLU", dbLong)
    .Fields.Append .CreateField("SGRP", dbLong)
    .Fields.Append .CreateField("mdim1", dbLong)
    .Fields.Append .CreateField("mdim2", dbLong)
    .Fields.Append .CreateField("maxsb1", dbLong)
    .Fields.Append .CreateField("maxsb2", dbLong)
    .Fields.Append .CreateField("mkind", dbLong)
    .Fields.Append .CreateField("sptrn", dbLong)
    .Fields.Append .CreateField("msect", dbLong)
    .Fields.Append .CreateField("mio", dbLong)
    .Fields.Append .CreateField("osvbas", dbLong)
    .Fields.Append .CreateField("osvoff", dbLong)
    .Fields.Append .CreateField("eunits", dbText, 8)
    .Fields.Append .CreateField("ltval1", dbSingle)
    .Fields.Append .CreateField("ltval2", dbSingle)
    .Fields.Append .CreateField("ltval3", dbSingle)
    .Fields.Append .CreateField("ltval4", dbSingle)
    .Fields.Append .CreateField("defn", dbText, 43)
    .Fields.Append .CreateField("munits", dbText, 8)
    .Fields.Append .CreateField("ltval5", dbSingle)
    .Fields.Append .CreateField("ltval6", dbSingle)
    .Fields.Append .CreateField("ltval7", dbSingle)
    .Fields.Append .CreateField("ltval8", dbSingle)
  End With
  myDb.TableDefs.Append myTabDef

  mySQL = "SELECT [BlockDefns].[Name], " & _
                 "[TSGroupDefns].[Name], " & _
                 "[TSMemberDefns].[Name] " & _
            "FROM (TSMemberDefns INNER JOIN TSGroupDefns ON " & _
                 "[TSMemberDefns].[TSGroupID] = [TSGroupDefns].[ID]) " & _
                 "INNER JOIN BlockDefns ON " & _
                 "[TSGroupDefns].[BlockID] = [BlockDefns].[ID] " & _
        "ORDER BY [TSMemberDefns].[ID];"

  Set myQuery = myDb.CreateQueryDef("BlockTSGroupTSMember", mySQL)
  
  Set myTabDef = myDb.CreateTableDef("TableAliasDefn")
  With myTabDef
    Set myIndx = .CreateIndex("iID")
    With myIndx
      .Name = "iID"
      .Unique = True
      .Primary = True
      .Fields = "ID"
    End With
    .Indexes.Append myIndx
    .Fields.Append .CreateField("ID", dbLong)
    .Fields(.Fields.Count - 1).Attributes = dbAutoIncrField
    .Fields.Append .CreateField("OpnTypID", dbLong)
    .Fields.Append .CreateField("Name", dbText, 16)
    .Fields.Append .CreateField("Occur", dbLong)
    .Fields.Append .CreateField("AppearName", dbText, 20)
    .Fields.Append .CreateField("IDVarName", dbText, 8)
    .Fields.Append .CreateField("IDVar", dbLong)
    .Fields.Append .CreateField("SubsKeyName", dbText, 8)
    .Fields.Append .CreateField("IDSubs", dbLong)
  End With
  myDb.TableDefs.Append myTabDef
  
  Set myTabDef = myDb.CreateTableDef("WDMAttribDefns")
  With myTabDef
    Set myIndx = .CreateIndex("iID")
    With myIndx
      .Name = "iID"
      .Unique = True
      .Primary = True
      .Fields = "ID"
    End With
    .Indexes.Append myIndx
    .Fields.Append .CreateField("ID", dbLong)
    .Fields.Append .CreateField("Name", dbText, 16)
    .Fields.Append .CreateField("Length", dbLong)
    .Fields.Append .CreateField("Type", dbLong)
    .Fields.Append .CreateField("Desc", dbText, 47)
    .Fields.Append .CreateField("Min", dbSingle)
    .Fields.Append .CreateField("Max", dbSingle)
    .Fields.Append .CreateField("Def", dbSingle)
    .Fields.Append .CreateField("Valid", dbText, 240)
  End With
  myDb.TableDefs.Append myTabDef
  
End Sub

