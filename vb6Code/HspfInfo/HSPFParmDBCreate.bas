Attribute VB_Name = "HSPFParmDBCreate"
Attribute VB_Ext_KEY = "RVB_UniqueId" ,"3607BBE00167"
Option Explicit

Sub BuildHSPFParmDB() ' fill in tables on blank db
    
    Set myTabDef = myDb.CreateTableDef("OpnTypDefn")
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
      .Fields.Append .CreateField("Name", dbText, 8)
    End With
    myDb.TableDefs.Append myTabDef
    Set myRec = myDb.OpenRecordset("OpnTypDefn", dbOpenDynaset)
    With myRec
      .AddNew
      !Name = "PERLND"
      .Update
      .AddNew
      !Name = "IMPLND"
      .Update
      .AddNew
      !Name = "RCHRES"
      .Update
    End With
    
    Set myTabDef = myDb.CreateTableDef("ParmTypeDefn")
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
      .Fields.Append .CreateField("Name", dbText, 4)
    End With
    myDb.TableDefs.Append myTabDef
    Set myRec = myDb.OpenRecordset("ParmTypeDefn", dbOpenDynaset)
    With myRec
      .AddNew
      !Name = "Char"
      .Update
      .AddNew
      !Name = "Long"
      .Update
      .AddNew
      !Name = "Real"
      .Update
      .AddNew
      !Name = "Dble"
      .Update
      .Close
    End With
    
    Set myTabDef = myDb.CreateTableDef("ParmData")
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
      Set myIndx = .CreateIndex("iParmID")
      With myIndx
         .Name = "iParmID"
         .Unique = False
         .Primary = False
         .Fields = "ParmID"
      End With
      .Indexes.Append myIndx
      .Fields.Append .CreateField("ParmID", dbLong)
      .Fields(.Fields.Count - 1).Required = True
      .Fields.Append .CreateField("SegID", dbLong)
      .Fields(.Fields.Count - 1).Required = True
      .Fields.Append .CreateField("Occur", dbLong)
      .Fields.Append .CreateField("Value", dbText, 20)
    End With
    myDb.TableDefs.Append myTabDef
    
    Set myTabDef = myDb.CreateTableDef("ParmDefn")
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
      .Fields.Append .CreateField("Name", dbText, 16)
      .Fields.Append .CreateField("Assoc", dbText, 16)
      .Fields.Append .CreateField("AssocID", dbLong)
      .Fields.Append .CreateField("ParmTypeID", dbLong)
      .Fields(.Fields.Count - 1).Required = True
      .Fields.Append .CreateField("ParmTableID", dbLong)
      .Fields(.Fields.Count - 1).Required = True
      .Fields.Append .CreateField("Min", dbText, 20)
      .Fields.Append .CreateField("Max", dbText, 20)
      .Fields.Append .CreateField("Def", dbText, 20)
      .Fields.Append .CreateField("MetMin", dbText, 20)
      .Fields.Append .CreateField("MetMax", dbText, 20)
      .Fields.Append .CreateField("MetDef", dbText, 20)
      .Fields.Append .CreateField("StartCol", dbInteger)
      .Fields.Append .CreateField("Width", dbInteger)
      .Fields.Append .CreateField("Definition", dbText, 255)
    End With
    myDb.TableDefs.Append myTabDef
    
    Set myTabDef = myDb.CreateTableDef("ParmTableDefn")
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
      .Fields.Append .CreateField("Name", dbText, 16)
      .Fields.Append .CreateField("OpnTypID", dbLong)
      .Fields(.Fields.Count - 1).Required = True
      .Fields.Append .CreateField("Alias", dbBoolean)
      .Fields.Append .CreateField("TableNumber", dbLong)
      .Fields.Append .CreateField("Definition", dbText, 255)
    End With
    myDb.TableDefs.Append myTabDef
    
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
        
    BuildHSPFTable
    
    Call frmWat.BuildWatTable
    Call frmScn.BuildScnTable
    
    Set myTabDef = myDb.CreateTableDef("SegData")
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
      .Fields.Append .CreateField("Name", dbText, 24)
      .Fields.Append .CreateField("Description", dbText, 255)
      .Fields.Append .CreateField("OpnTypID", dbLong)
      .Fields(.Fields.Count - 1).Required = True
      .Fields.Append .CreateField("ScenarioID", dbLong)
      .Fields(.Fields.Count - 1).Required = True
    End With
    myDb.TableDefs.Append myTabDef
    
    Set myRel = myDb.CreateRelation("OperationName", _
      "OpnTypDefn", "SegData", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "OpnTypID"
    myDb.Relations.Append myRel

    Set myRel = myDb.CreateRelation("OpnTypDefnTableAliasDefn", _
      "OpnTypDefn", "TableAliasDefn", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "OpnTypID"
    myDb.Relations.Append myRel

    Set myRel = myDb.CreateRelation("ParmDefnTableAliasDefn", _
      "ParmDefn", "TableAliasDefn", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "IDVar"
    myDb.Relations.Append myRel
    
    'Set myRel = myDB.CreateRelation("TableAliasDefnParmTableDefn", _
    '  "TableAliasDefn", "ParmTableDefn", dbRelationUpdateCascade)
    'myRel.Fields.Append myRel.CreateField("Name")
    'myRel.Fields!id.ForeignName = "Name"
    'myDB.Relations.Append myRel
    
    Set myRel = myDb.CreateRelation("ScenarioName", _
      "ScenarioData", "SegData", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "ScenarioID"
    myDb.Relations.Append myRel
    
    Set myRel = myDb.CreateRelation("ProjectName", _
      "WatershedData", "ScenarioData", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "WatershedID"
    myDb.Relations.Append myRel
    
    Set myRel = myDb.CreateRelation("ParameterTypeName", _
      "ParmTypeDefn", "ParmDefn", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "ParmTypeID"
    myDb.Relations.Append myRel
    
    Set myRel = myDb.CreateRelation("ParameterTableName", _
      "ParmTableDefn", "ParmDefn", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "ParmTableID"
    myDb.Relations.Append myRel
    
    Set myRel = myDb.CreateRelation("ParmName", _
      "ParmDefn", "ParmData", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "ParmID"
    myDb.Relations.Append myRel
    
    Set myRel = myDb.CreateRelation("Name", _
      "SegData", "ParmData", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "SegID"
    myDb.Relations.Append myRel
    
    Set myRel = myDb.CreateRelation("ParmOperationName", _
       "OpnTypDefn", "ParmTableDefn", dbRelationUpdateCascade)
    myRel.Fields.Append myRel.CreateField("ID")
    myRel.Fields!id.ForeignName = "OpnTypID"
    myDb.Relations.Append myRel
    
    mySQL = "UPDATE DISTINCTROW TableAliasDefn INNER JOIN ParmTableDefn ON " & _
                              "(TableAliasDefn.OpnTypID = ParmTableDefn.OpnTypID) " & _
                              "AND (TableAliasDefn.Name = ParmTableDefn.Name) " & _
                              "SET ParmTableDefn.Alias = Yes;"
    Set myQuery = myDb.CreateQueryDef("ParmTableAliasAvailable", mySQL)
    myQuery.Execute

    mySQL = "SELECT DISTINCTROW SegData.ID AS SegID, " & _
                               "ParmTableDefn.Name, " & _
                               "ParmTableDefn.ID AS TabID,  " & _
                               "ParmTableDefn.OpnTypID  " & _
            "FROM (ScenarioData INNER JOIN SegData ON ScenarioData.ID = SegData.ScenarioID)  " & _
                               "INNER JOIN ((ParmTableDefn  " & _
                               "INNER JOIN ParmDefn ON ParmTableDefn.ID = ParmDefn.ParmTableID) " & _
                               "INNER JOIN ParmData ON ParmDefn.ID = ParmData.ParmID) ON SegData.ID = ParmData.SegID;"
    Set myQuery = myDb.CreateQueryDef("ScenTableList", mySQL)
    
    mySQL = "SELECT DISTINCTROW SegData.ID AS SegID, " & _
                               "ParmDefn.Name, " & _
                               "ParmData.Value, " & _
                               "ParmDefn.ID AS ParmID, " & _
                               "ParmDefn.AssocID AS AssocID, " & _
                               "ParmDefn.ParmTableID AS TabID, " & _
                               "ParmTableDefn.OpnTypID, " & _
                               "ParmTableDefn.Name AS [Table], " & _
                               "ParmData.Occur, " & _
                               "IIf([ParmTableDefn]![Alias]=0,' ','Alias') AS AliasInfo " & _
             "FROM (ScenarioData INNER JOIN SegData ON ScenarioData.ID = SegData.ScenarioID) " & _
                            "INNER JOIN ((ParmTableDefn " & _
                            "INNER JOIN ParmDefn ON ParmTableDefn.ID = ParmDefn.ParmTableID) " & _
                            "INNER JOIN ParmData ON ParmDefn.ID = ParmData.ParmID) ON SegData.ID = ParmData.SegID;"

    Set myQuery = myDb.CreateQueryDef("ParmTableData", mySQL)
    
    mySQL = "SELECT DISTINCTROW ParmDefn.Name, " & _
                               "ParmDefn.ID,  " & _
                               "ParmTableDefn.ID AS TabID, " & _
                               "ParmTableDefn.Name AS TabName, " & _
                               "ParmTableDefn.OpnTypID,  " & _
                               "ParmTypeDefn.Name AS ParmType,  " & _
                               "ParmDefn.Def, " & _
                               "ParmDefn.Min, " & _
                               "ParmDefn.Max, " & _
                               "ParmDefn.metDef, " & _
                               "ParmDefn.metMin, " & _
                               "ParmDefn.metMax, " & _
                               "ParmDefn.StartCol, " & _
                               "ParmDefn.Width " & _
            "FROM ParmTypeDefn INNER JOIN (ParmTableDefn  " & _
                              "INNER JOIN ParmDefn ON ParmTableDefn.ID = ParmDefn.ParmTableID) ON ParmTypeDefn.ID = ParmDefn.ParmTypeID  " & _
            "ORDER BY ParmDefn.ID, ParmTableDefn.ID;"
    Set myQuery = myDb.CreateQueryDef("ParmTableList", mySQL)

    mySQL = "SELECT DISTINCTROW OpnTypDefn.Name AS OpnType, " & _
                               "ParmDefn.Name AS ParmName, " & _
                               "ParmData.Value " & _
            "FROM (TableAliasDefn INNER JOIN (ParmDefn " & _
                                 "INNER JOIN ParmData ON ParmDefn.ID = ParmData.ParmID) ON TableAliasDefn.IDVar = ParmDefn.ID) INNER JOIN OpnTypDefn ON TableAliasDefn.OpnTypID = OpnTypDefn.ID " & _
            "GROUP BY TableAliasDefn.OpnTypID, OpnTypDefn.Name, ParmDefn.Name, ParmData.Value, TableAliasDefn.IDVar " & _
            "ORDER BY TableAliasDefn.OpnTypID, ParmDefn.Name, ParmData.Value;"
            
    Set myQuery = myDb.CreateQueryDef("UniqueName", mySQL)
    
    mySQL = "SELECT DISTINCTROW OpnTypDefn.Name AS OpnType, " & _
                               "ParmTableDefn.Name AS [Table], " & _
                               "ParmDefn.Name AS Parm, " & _
                               "WatershedData.WatershedName AS Watershed, " & _
                               "ScenarioData.Name AS Scenario, " & _
                               "SegData.Name AS Segment, " & _
                               "ParmData.Occur, " & _
                               "ParmData.Value " & _
            "FROM ((WatershedData INNER JOIN ScenarioData ON WatershedData.ID = ScenarioData.WatershedID) " & _
                                 "INNER JOIN SegData ON ScenarioData.ID = SegData.ScenarioID) " & _
                                 "INNER JOIN (OpnTypDefn " & _
                                 "INNER JOIN ((ParmTableDefn " & _
                                 "INNER JOIN ParmDefn ON ParmTableDefn.ID = ParmDefn.ParmTableID) " & _
                                 "INNER JOIN ParmData ON ParmDefn.ID = ParmData.ParmID) ON OpnTypDefn.ID = ParmTableDefn.OpnTypID) ON SegData.ID = ParmData.SegID " & _
            "ORDER BY ParmDefn.ParmTableID, ParmData.ParmID, ParmData.Occur, WatershedData.WatershedName, ScenarioData.Name, SegData.Name;"
     Set myQuery = myDb.CreateQueryDef("ParmListAll", mySQL)
     
     mySQL = "SELECT DISTINCTROW ScenarioData.ID, " & _
                                 "ScenarioData.Name, " & _
                                 "SegData.OpnTypID, " & _
                                 "Count(SegData.OpnTypID) AS CountOfOpnTypID " & _
              "FROM ScenarioData INNER JOIN SegData ON ScenarioData.ID = SegData.ScenarioID " & _
              "GROUP BY ScenarioData.ID, ScenarioData.Name, SegData.OpnTypID;"
     Set myQuery = myDb.CreateQueryDef("CountOpnTypes", mySQL)
      
End Sub


Sub BuildHSPFTable()

    Dim init&, id&, kwd$, kflg&, contfg&, retid&, opnid&, offset&
    Dim omcode&, tabno&, uunits&
    'Retrieved in a call to F90_XTINFO to get column info for grid
    Dim lnflds&, lscol&(30), lflen&(30), lftyp$, lapos&(30)
    Dim limin&(30), limax&(30), lidef&(30)
    Dim lrmin!(30), lrmax!(30), lrdef!(30)
    Dim limetmin&(30), limetmax&(30), limetdef&(30)
    Dim lrmetmin!(30), lrmetmax!(30), lrmetdef!(30)
    Dim lnmhdr&, hdrbuf$(10), lfdnam$(30)
    Dim ParmTableID&, ParmTableType&
    Dim snam$, i&, Assoc$, AssocID&, crit$
    
    Dim gnum&, initfg&, olen&, cont&, obuff$
    Dim Occur&, Name$, AppearName$, IDVarName$, SubsKeyName$, OName$

    Set myParmDefn = myDb.OpenRecordset("ParmDefn", dbOpenDynaset)
    Set myParmTableDefn = myDb.OpenRecordset("ParmTableDefn", dbOpenDynaset)

    ChDir "UciWdm"
    snam = "test04" 'dummy, need to save fwdm and fmsg in f90 module
    Call F90_ACTSCN(CLng(-1), fwdm, fmsg, retid, snam, Len(snam))

    omcode = 120
    uunits = 1
    Do While omcode < 123 'more operation types
      omcode = omcode + 1
      init = 1
      'get table info
      Do 'while more tables
        Call F90_GTNXKW(init, omcode, kwd, kflg, contfg, tabno)
        If init = 1 Then
          init = 0
        End If
        If tabno > 0 Then
          opnid = omcode - 120
          tabno = tabno - (opnid * 1000)
          With myParmTableDefn
            .AddNew
            !Name = kwd
            !OpnTypID = opnid
            !TableNumber = tabno
            ParmTableID = !id
            .Update
          End With
          'call first to get metric, then english
          Call F90_XTINFO(omcode, tabno, 2, _
            lnflds, lscol, lflen, lftyp, lapos, _
            limetmin, limetmax, limetdef, lrmetmin, lrmetmax, lrmetdef, _
            lnmhdr, hdrbuf, lfdnam, retid)
          Call F90_XTINFO(omcode, tabno, uunits, _
            lnflds, lscol, lflen, lftyp, lapos, _
            limin, limax, lidef, lrmin, lrmax, lrdef, _
            lnmhdr, hdrbuf, lfdnam, retid)
          Call F90_WMSGTW(CLng(1), Assoc)
          If Len(Assoc) > 0 Then
            With myParmDefn
              crit = "Name = '" & Assoc & "'"
              .FindFirst crit
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
              .AddNew
              !Name = lfdnam(i)
              !Assoc = Assoc
              !AssocID = AssocID
              ParmTableType = ParmType(Mid(lftyp, i + 1, 1))
              !ParmTypeID = ParmTableType
              !ParmTableID = ParmTableID
              !StartCol = lscol(i) + offset
              !Width = lflen(i)
              If ParmTableType = 2 Then ' integer
                !Min = ChkNone(CStr(limin(lapos(i) - 1)))
                !Max = ChkNone(CStr(limax(lapos(i) - 1)))
                !Def = ChkNone(CStr(lidef(lapos(i) - 1)))
                !metMin = ChkNone(CStr(limetmin(lapos(i) - 1)))
                !metMax = ChkNone(CStr(limetmax(lapos(i) - 1)))
                !metDef = ChkNone(CStr(limetdef(lapos(i) - 1)))
              ElseIf ParmTableType = 3 Then 'real
                !Min = ChkNone(CStr(lrmin(lapos(i) - 1)))
                !Max = ChkNone(CStr(lrmax(lapos(i) - 1)))
                !Def = ChkNone(CStr(lrdef(lapos(i) - 1)))
                !metMin = ChkNone(CStr(lrmetmin(lapos(i) - 1)))
                !metMax = ChkNone(CStr(lrmetmax(lapos(i) - 1)))
                !metDef = ChkNone(CStr(lrmetdef(lapos(i) - 1)))
              End If
              .Update
            End With
          Next i
        ElseIf contfg = 0 Then
          Exit Do
        End If
      Loop
    Loop

    'lblInfo.Caption = myParmDefn.RecordCount & " " & myParmTableDefn.RecordCount

    myParmDefn.Close
    myParmTableDefn.Close
    
    Set myRec = myDb.OpenRecordset("ParmDefn", dbOpenDynaset)
    Set myTableAliasDefn = myDb.OpenRecordset("TableAliasDefn", dbOpenDynaset)
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
          .AddNew
          !OpnTypID = opnid
          !Name = Left(obuff, 12)
          If OName <> !Name Then
            Occur = 1
            OName = !Name
          Else
            Occur = Occur + 1
          End If
          !Occur = Occur
          If Len(obuff) > 14 Then
            !AppearName = Mid(obuff, 15, 20)
          Else
            !AppearName = " "
          End If
          If Len(obuff) > 36 Then
            !IDVarName = Mid(obuff, 37, 6)
            crit = "Name = '" & !IDVarName & "'"
            myRec.FindFirst crit
            !IDVar = myRec!id
          Else
            !IDVarName = " "
          End If
          If Len(obuff) > 44 Then
            !SubsKeyName = Mid(obuff, 45, 6)
            crit = "Name = '" & !SubsKeyName & "'"
            myRec.FindFirst crit
            !IDSubs = myRec!id
          Else
            !SubsKeyName = " "
          End If
          .Update
        Loop
      Loop
    End With
    myTableAliasDefn.Close
    myRec.Close
    
    ChDir ".."

End Sub

Private Function ChkNone(s$) As String
    If s = "-999" Then
      ChkNone = "<none>"
    Else
      ChkNone = s
    End If
End Function

Private Function ParmType(s$) As Long

    Dim i&

    If s = "C" Then 'character
      i = 1
    ElseIf s = "I" Then 'long integer
      i = 2
    Else 'real
      i = 3
    End If
    ParmType = i

End Function
