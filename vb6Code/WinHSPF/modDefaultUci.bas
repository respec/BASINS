Attribute VB_Name = "modDefaultUci"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Sub setDefault(myUci As HspfUci, defUci As HspfUci)
  Dim vOpTyp As Variant, OpTyps As Variant, loptyp As HspfOpnBlk
  Dim vOpn As Variant, lOpn As HspfOperation, dOpn As HspfOperation
  Dim vTab As Variant, lTab As HspfTable, dTab As HspfTable
  Dim vPar As Variant, lPar As HSPFParm, dPar As HSPFParm
  Dim Id&
  
  OpTyps = Array("PERLND", "IMPLND", "RCHRES")
  For Each vOpTyp In OpTyps
    If myUci.OpnBlks(vOpTyp).Count > 0 Then
      Set loptyp = myUci.OpnBlks(vOpTyp)
      'Debug.Print lOpTyp.Name
      For Each vOpn In loptyp.Ids
        Set lOpn = vOpn
        'Debug.Print lOpn.Description
        Id = DefaultOpnId(lOpn, defUci)
        If Id > 0 Then
          Set dOpn = defUci.OpnBlks(lOpn.Name).operfromid(Id)
          If Not dOpn Is Nothing Then
            For Each vTab In lOpn.tables
              Set lTab = vTab
              If DefaultThisTable(loptyp.Name, lTab.Name) Then
                If dOpn.TableExists(lTab.Name) Then
                  Set dTab = dOpn.tables(lTab.Name)
                  'Debug.Print lTab.Name
                  For Each vPar In lTab.Parms
                    'Set lPar = vPar   'used to have to do this
                    If DefaultThisParameter(loptyp.Name, lTab.Name, vPar.Name) Then
                      'Set dPar = dTab.parms(vPar.name)
                      'Debug.Print lPar.Name
                      If vPar.Value <> vPar.Name Then
                        'Debug.Print "update " & lPar.Name
                        vPar.Value = dTab.Parms(vPar.Name).Value
                      End If
                    End If
                  Next vPar
                End If
              End If
            Next vTab
          End If
        End If
      Next vOpn
    End If
  Next vOpTyp
End Sub

Public Sub setDefaultsForTable(myUci As HspfUci, defUci As HspfUci, opname$, TableName$)
  Dim vOpTyp As Variant, OpTyps As Variant, loptyp As HspfOpnBlk
  Dim vOpn As Variant, lOpn As HspfOperation, dOpn As HspfOperation
  Dim vTab As Variant, lTab As HspfTable, dTab As HspfTable
  Dim vPar As Variant, lPar As HSPFParm, dPar As HSPFParm
  Dim Id&
  
    If myUci.OpnBlks(opname).Count > 0 Then
      Set loptyp = myUci.OpnBlks(opname)
      For Each vOpn In loptyp.Ids
        Set lOpn = vOpn
        Id = DefaultOpnId(lOpn, defUci)
        If Id > 0 Then
          Set dOpn = defUci.OpnBlks(lOpn.Name).operfromid(Id)
          If Not dOpn Is Nothing Then
            If lOpn.TableExists(TableName) Then
              Set lTab = lOpn.tables(TableName)
              If DefaultThisTable(loptyp.Name, lTab.Name) Then
                If dOpn.TableExists(lTab.Name) Then
                  Set dTab = dOpn.tables(lTab.Name)
                  For Each vPar In lTab.Parms
                    If DefaultThisParameter(loptyp.Name, lTab.Name, vPar.Name) Then
                      If vPar.Value <> vPar.Name Then
                        vPar.Value = dTab.Parms(vPar.Name).Value
                      End If
                    End If
                  Next vPar
                End If
              End If
            End If
          End If
        End If
      Next vOpn
    End If
  
End Sub

Public Sub setDefaultML(myUci As HspfUci, defUci As HspfUci)
  Dim vML As Variant, dML As HspfMassLink
  Dim lMassLink As HspfMassLink
  
  For Each vML In defUci.MassLinks
    Set dML = vML
    If dML.Source.volname = "PERLND" And dML.Source.member = "PERO" Then
    ElseIf dML.Source.volname = "IMPLND" And dML.Source.member = "SURO" Then
    ElseIf dML.Source.volname = "RCHRES" And dML.Source.group = "ROFLOW" Then
    Else
      'add the other ones
      Set lMassLink = New HspfMassLink
      Set lMassLink = dML
      myUci.MassLinks.Add lMassLink
    End If
  Next vML
      
End Sub

Public Sub SetMissingValuesToDefaults(myUci As HspfUci, defUci As HspfUci)
  Dim vOpTyp As Variant, OpTyps As Variant, loptyp As HspfOpnBlk
  Dim vOpn As Variant, lOpn As HspfOperation, dOpn As HspfOperation
  Dim vTab As Variant, lTab As HspfTable, dTab As HspfTable
  Dim vPar As Variant, lPar As HSPFParm, dPar As HSPFParm
  Dim Id&
  
  OpTyps = Array("PERLND", "IMPLND", "RCHRES")
  For Each vOpTyp In OpTyps
    If myUci.OpnBlks(vOpTyp).Count > 0 Then
      Set loptyp = myUci.OpnBlks(vOpTyp)
      For Each vOpn In loptyp.Ids
        Set lOpn = vOpn
        Id = DefaultOpnId(lOpn, defUci)
        If Id > 0 Then
          Set dOpn = defUci.OpnBlks(lOpn.Name).operfromid(Id)
          If Not dOpn Is Nothing Then
            For Each vTab In lOpn.tables
              Set lTab = vTab
              If dOpn.TableExists(lTab.Name) Then
                Set dTab = dOpn.tables(lTab.Name)
                For Each vPar In lTab.Parms
                  If vPar.Value = -999# Then
                    vPar.Value = dTab.Parms(vPar.Name).Value
                  End If
                Next vPar
              End If
            Next vTab
          End If
        End If
      Next vOpn
    End If
  Next vOpTyp
End Sub

Public Function matchOperWithDefault(OpTypName$, OpnDesc$, defUci As HspfUci) As HspfOperation
  Dim vOpn As Variant, lOpn As HspfOperation, ctemp$
  
  For Each vOpn In defUci.OpnBlks(OpTypName).Ids
    Set lOpn = vOpn
    If lOpn.Description = OpnDesc Then
      Set matchOperWithDefault = lOpn
      Exit Function
    End If
  Next vOpn
  'a complete match not found, look for partial
  For Each vOpn In defUci.OpnBlks(OpTypName).Ids
    Set lOpn = vOpn
    If Len(lOpn.Description) > Len(OpnDesc) Then
      ctemp = Left(lOpn.Description, Len(OpnDesc))
      If ctemp = OpnDesc Then
        Set matchOperWithDefault = lOpn
        Exit Function
      End If
    ElseIf Len(lOpn.Description) < Len(OpnDesc) Then
      ctemp = Left(OpnDesc, Len(lOpn.Description))
      If lOpn.Description = ctemp Then
        Set matchOperWithDefault = lOpn
        Exit Function
      End If
    End If
    If Len(OpnDesc) > 4 And Len(lOpn.Description) > 4 Then
      ctemp = Left(OpnDesc, 4)
      If Left(lOpn.Description, 4) = ctemp Then
        Set matchOperWithDefault = lOpn
        Exit Function
      End If
    End If
  Next vOpn
  'not found, use first one
  If defUci.OpnBlks(OpTypName).Count > 0 Then
    Set matchOperWithDefault = defUci.OpnBlks(OpTypName).Ids(1)
  Else
    Set matchOperWithDefault = Nothing
  End If
End Function

Public Function DefaultOpnId(lOpn As HspfOperation, defUci As HspfUci) As Long
  Dim dOpn As HspfOperation
  If lOpn.DefOpnId <> 0 Then
    DefaultOpnId = lOpn.DefOpnId
  Else
    Set dOpn = matchOperWithDefault(lOpn.Name, lOpn.Description, defUci)
    If dOpn Is Nothing Then
      DefaultOpnId = 0
    Else
      DefaultOpnId = dOpn.Id
    End If
  End If
End Function

Private Function DefaultThisTable(OperName$, TableName$) As Boolean
  If OperName = "PERLND" Or OperName = "IMPLND" Then
    If TableName = "ACTIVITY" Or _
       TableName = "PRINT-INFO" Or _
       TableName = "GEN-INFO" Or _
       TableName = "PWAT-PARM5" Then
      DefaultThisTable = False
    ElseIf Left(TableName, 4) = "QUAL" Then
      DefaultThisTable = False
    Else
      DefaultThisTable = True
    End If
  ElseIf OperName = "RCHRES" Then
    If TableName = "ACTIVITY" Or _
       TableName = "PRINT-INFO" Or _
       TableName = "GEN-INFO" Or _
       TableName = "HYDR-PARM1" Then
      DefaultThisTable = False
    ElseIf Left(TableName, 3) = "GQ-" Then
      DefaultThisTable = False
    Else
      DefaultThisTable = True
    End If
  Else
    DefaultThisTable = False
  End If
End Function

Private Function DefaultThisParameter(OperName$, TableName$, ParmName$) As Boolean
    DefaultThisParameter = True
    If OperName = "PERLND" Then
      If TableName = "PWAT-PARM2" Then
        If ParmName = "SLSUR" Or ParmName = "LSUR" Then
          DefaultThisParameter = False
        End If
      ElseIf TableName = "NQUALS" Then
        If ParmName = "NQUAL" Then
          DefaultThisParameter = False
        End If
      End If
    ElseIf OperName = "IMPLND" Then
      If TableName = "IWAT-PARM2" Then
        If ParmName = "SLSUR" Or ParmName = "LSUR" Then
          DefaultThisParameter = False
        End If
      ElseIf TableName = "NQUALS" Then
        If ParmName = "NQUAL" Then
          DefaultThisParameter = False
        End If
      End If
    ElseIf OperName = "RCHRES" Then
      If TableName = "HYDR-PARM2" Then
        If ParmName = "LEN" Or _
           ParmName = "DELTH" Or _
           ParmName = "FTBUCI" Then
          DefaultThisParameter = False
        End If
      ElseIf TableName = "GQ-GENDATA" Then
        If ParmName = "NGQUAL" Then
          DefaultThisParameter = False
        End If
      End If
    End If
End Function
