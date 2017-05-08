VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfStatus"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Enum HspfStatusReqOptUnnEnum
  HspfStatusRequired = 1
  HspfStatusOptional = 2
  HspfStatusUnneeded = 4
End Enum
Public Enum HspfStatusPresentMissingEnum
  HspfStatusPresent = True
  HspfStatusMissing = False
  HspfStatusAny = 2
End Enum
Public Enum HspfStatusTypes
  HspfTable = 1
  HspfInputTimeseries = 2
  HspfOutputTimeseries = 3
End Enum

Dim pStatusType As HspfStatusTypes
Dim pOper As HspfOperation
Dim pTableStatus As Collection 'of HspfStatusType

Property Get StatusType() As HspfStatusTypes
  StatusType = pStatusType
End Property
Property Let StatusType(newStatusType As HspfStatusTypes)
  pStatusType = newStatusType
End Property

Property Get TotalPossible()
  If pTableStatus.Count = 0 Then Build
  TotalPossible = pTableStatus.Count
End Property

Public Sub Change(Name$, Occur&, Status&)
  Dim vTableStatus As Variant
  Dim lTableStatus As HspfStatusType
  Dim found As Boolean
  
  found = False
  For Each vTableStatus In pTableStatus
    Set lTableStatus = vTableStatus
    If lTableStatus.Name = Name And lTableStatus.Occur = Occur Then
      lTableStatus.ReqOptUnn = Status
      found = True
      Exit For
    End If
  Next vTableStatus
  If Not (found) Then MsgBox "Change Failed For " & Name & "(" & Occur & ")"
End Sub

Public Sub Change2(Name$, Occur1&, Occur2&, Status&)
  Dim vTableStatus As Variant
  Dim lTableStatus As HspfStatusType
  Dim found As Boolean
  Dim lOccur&
  
  lOccur = (Occur2 - 1) * 1000 + Occur1
  found = False
  For Each vTableStatus In pTableStatus
    Set lTableStatus = vTableStatus
    If lTableStatus.Name = Name And lTableStatus.Occur = lOccur Then
      lTableStatus.ReqOptUnn = Status
      found = True
      Exit For
    End If
  Next vTableStatus
  If Not (found) Then MsgBox "Change Failed For " & Name & "(" & Occur1 & "," & Occur2 & ")"
End Sub

Public Sub Update()
  Dim i&, j&, k&
  Dim vTable As Variant, ltable As HspfTable
  Dim vTableStatus As Variant, lTableStatus As HspfStatusType
  Dim vConnection As Variant, lConnection As HspfConnection
  Dim cOccur&, lOccur&
  Dim lSub1&, lSub2&, lMember$, lGroup$, lCnt&
  Dim lMemberStatus$, lGroupStatus$
  Dim lMassLink As HspfMassLink
  
  For Each vTableStatus In pTableStatus
    Set lTableStatus = vTableStatus
    lTableStatus.ReqOptUnn = HspfStatusUnneeded
    lTableStatus.Present = HspfStatusMissing
  Next vTableStatus
  
  If pStatusType = HspfTable Then
    For Each vTable In pOper.Tables 'should this be in another loop
      Set ltable = vTable
      For Each vTableStatus In pTableStatus
        Set lTableStatus = vTableStatus
        If ltable.OccurNum = lTableStatus.Occur And _
           ltable.Name = lTableStatus.Name Then
          lTableStatus.Present = HspfStatusPresent
          Exit For
        End If
      Next vTableStatus
    Next vTable
  ElseIf pStatusType = HspfInputTimeseries Then 'source
    For Each vConnection In pOper.Sources  'should this be in another loop
      Set lConnection = vConnection
      GetConnectionInfo True, lConnection, lGroup, lMember, lSub1, lSub2, True
      While Len(lGroup) > 0
        cOccur = (lSub2 - 1) * 1000 + lSub1
        For Each vTableStatus In pTableStatus
          Set lTableStatus = vTableStatus
          With lTableStatus
            j = InStr(.Name, ":")
            lGroupStatus = Left(.Name, j - 1)
            lMemberStatus = Right(.Name, Len(.Name) - j)
            lOccur = .Occur
          End With
          If cOccur = lOccur And _
            (lMember = lMemberStatus Or Len(lMember) = 0) And _
             lGroup = lGroupStatus Then
            lTableStatus.Present = HspfStatusPresent
            Exit For
          End If
        Next vTableStatus
        GetConnectionInfo True, lConnection, lGroup, lMember, lSub1, lSub2
      Wend
    Next vConnection
  Else 'target
    For Each vConnection In pOper.Targets
      Set lConnection = vConnection
      GetConnectionInfo False, lConnection, lGroup, lMember, lSub1, lSub2, True
      While Len(lGroup) > 0
        For Each vTableStatus In pTableStatus
          Set lTableStatus = vTableStatus
          With lTableStatus
            j = InStr(.Name, ":")
            lGroupStatus = Left(.Name, j - 1)
            lMemberStatus = Right(.Name, Len(.Name) - j)
            lOccur = .Occur
          End With
          cOccur = (lSub2 - 1) * 1000 + lSub1
          If cOccur = lOccur And _
            (lMember = lMemberStatus Or Len(lMember) = 0) And _
             lGroup = lGroupStatus Then
            lTableStatus.Present = HspfStatusPresent
            Exit For
          End If
        Next vTableStatus
        GetConnectionInfo False, lConnection, lGroup, lMember, lSub1, lSub2
      Wend
    Next vConnection
  End If

  If pStatusType = HspfTable Then
    Select Case pOper.Name
      Case "PERLND": UpdatePerlnd pOper, Me
      Case "IMPLND": UpdateImplnd pOper, Me
      Case "RCHRES": UpdateRchres pOper, Me
      Case "COPY": UpdateCopy pOper, Me
      Case "PLTGEN": UpdatePltgen pOper, Me
      Case "DISPLY": UpdateDisply pOper, Me
      Case "DURANL": UpdateDuranl pOper, Me
      Case "GENER": UpdateGener pOper, Me
      Case "MUTSIN": UpdateMutsin pOper, Me
      Case "BMPRAC": UpdateBmprac pOper, Me
      Case "REPORT": UpdateReport pOper, Me
    End Select
  ElseIf pStatusType = HspfInputTimeseries Then
    Select Case pOper.Name
      Case "PERLND": UpdateInputTimeseriesPerlnd pOper, Me
      Case "IMPLND": UpdateInputTimeseriesImplnd pOper, Me
      Case "RCHRES": UpdateInputTimeseriesRchres pOper, Me
      Case "COPY": UpdateInputTimeseriesCopy pOper, Me
      Case "PLTGEN": UpdateInputTimeseriesPltgen pOper, Me
      Case "DISPLY": UpdateInputTimeseriesDisply pOper, Me
      Case "DURANL": UpdateInputTimeseriesDuranl pOper, Me
      Case "GENER": UpdateInputTimeseriesGener pOper, Me
      Case "MUTSIN": UpdateInputTimeseriesMutsin pOper, Me
      Case "BMPRAC": UpdateInputTimeseriesBmprac pOper, Me
      Case "REPORT": UpdateInputTimeseriesReport pOper, Me
    End Select
  ElseIf pStatusType = HspfOutputTimeseries Then
    Select Case pOper.Name
      Case "PERLND": UpdateOutputTimeseriesPerlnd pOper, Me
      Case "IMPLND": UpdateOutputTimeseriesImplnd pOper, Me
      Case "RCHRES": UpdateOutputTimeseriesRchres pOper, Me
      Case "COPY": UpdateOutputTimeseriesCopy pOper, Me
      Case "PLTGEN": UpdateOutputTimeseriesPltgen pOper, Me
      Case "DISPLY": UpdateOutputTimeseriesDisply pOper, Me
      Case "DURANL": UpdateOutputTimeseriesDuranl pOper, Me
      Case "GENER": UpdateOutputTimeseriesGener pOper, Me
      Case "MUTSIN": UpdateOutputTimeseriesMutsin pOper, Me
      Case "BMPRAC": UpdateOutputTimeseriesBmprac pOper, Me
      Case "REPORT": UpdateOutputTimeseriesReport pOper, Me
    End Select
  End If
End Sub

Public Sub UpdateExtTargetsOutputs()
  Dim i&, j&, k&
  Dim vTable As Variant, ltable As HspfTable
  Dim vTableStatus As Variant, lTableStatus As HspfStatusType
  Dim vConnection As Variant, lConnection As HspfConnection
  Dim cOccur&, lOccur&
  Dim lSub1&, lSub2&, lMember$, lGroup$, lCnt&
  Dim lMemberStatus$, lGroupStatus$
  Dim lMassLink As HspfMassLink
  
  If pTableStatus.Count = 0 Then Build
  
  For Each vTableStatus In pTableStatus
    Set lTableStatus = vTableStatus
    lTableStatus.ReqOptUnn = HspfStatusUnneeded
    lTableStatus.Present = HspfStatusMissing
  Next vTableStatus
  
  For Each vConnection In pOper.Targets
    Set lConnection = vConnection
    If Left(lConnection.Target.VolName, 3) = "WDM" Then
      GetConnectionInfo False, lConnection, lGroup, lMember, lSub1, lSub2, True
      While Len(lGroup) > 0
        For Each vTableStatus In pTableStatus
          Set lTableStatus = vTableStatus
          With lTableStatus
            j = InStr(.Name, ":")
            lGroupStatus = Left(.Name, j - 1)
            lMemberStatus = Right(.Name, Len(.Name) - j)
            lOccur = .Occur
          End With
          cOccur = (lSub2 - 1) * 1000 + lSub1
          If cOccur = lOccur And _
            (lMember = lMemberStatus Or Len(lMember) = 0) And _
             lGroup = lGroupStatus Then
            lTableStatus.Present = HspfStatusPresent
            Exit For
          End If
        Next vTableStatus
        GetConnectionInfo False, lConnection, lGroup, lMember, lSub1, lSub2
      Wend
    End If
  Next vConnection
  
  Select Case pOper.Name
    Case "PERLND": UpdateOutputTimeseriesPerlnd pOper, Me
    Case "IMPLND": UpdateOutputTimeseriesImplnd pOper, Me
    Case "RCHRES": UpdateOutputTimeseriesRchres pOper, Me
    Case "COPY": UpdateOutputTimeseriesCopy pOper, Me
    Case "PLTGEN": UpdateOutputTimeseriesPltgen pOper, Me
    Case "DISPLY": UpdateOutputTimeseriesDisply pOper, Me
    Case "DURANL": UpdateOutputTimeseriesDuranl pOper, Me
    Case "GENER": UpdateOutputTimeseriesGener pOper, Me
    Case "MUTSIN": UpdateOutputTimeseriesMutsin pOper, Me
    Case "BMPRAC": UpdateOutputTimeseriesBmprac pOper, Me
    Case "REPORT": UpdateOutputTimeseriesReport pOper, Me
  End Select

End Sub

Private Sub GetConnectionInfo(Source As Boolean, _
                              Connection As HspfConnection, _
                              Group$, Member$, Sub1&, Sub2&, _
                              Optional init = False)
  Dim lMassLink As HspfMassLink
  Static massLinkPos
  
  If init Then massLinkPos = 1
  If Connection.MassLink = 0 Then
    If massLinkPos = 1 Then
      If Source Then
        Group = Connection.Target.Group
        Member = Connection.Target.Member
        Sub1 = Connection.Target.MemSub1
        Sub2 = Connection.Target.MemSub2
      Else
        Group = Connection.Source.Group
        Member = Connection.Source.Member
        Sub1 = Connection.Source.MemSub1
        Sub2 = Connection.Source.MemSub2
      End If
    Else 'only wanted one
      Group = ""
    End If
  Else
    Group = "?"
    While Group = "?"
      If massLinkPos <= pOper.Uci.MassLinks.Count Then
        Set lMassLink = pOper.Uci.MassLinks(massLinkPos)
        If lMassLink.MassLinkID = Connection.MassLink Then
          If Source Then
            Group = lMassLink.Target.Group
            Member = lMassLink.Target.Member
            Sub1 = lMassLink.Target.MemSub1
            Sub2 = lMassLink.Target.MemSub2
          Else
            Group = lMassLink.Source.Group
            Member = lMassLink.Source.Member
            Sub1 = lMassLink.Source.MemSub1
            Sub2 = lMassLink.Source.MemSub2
          End If
        Else
          massLinkPos = massLinkPos + 1
        End If
      Else 'done
        Group = ""
      End If
    Wend
  End If
  massLinkPos = massLinkPos + 1
  If Sub1 = 0 Then Sub1 = 1
  If Sub2 = 0 Then Sub2 = 1

End Sub
Private Sub Build()
  Dim vTableDef As Variant, lTableDef As HspfTableDef
  Dim i&, j&, ldim1&, ldim2&
  Dim lTableStatus As HspfStatusType
  Dim lOccur&, addMember As Boolean
  Dim vTSGroupDef As Variant, lTSGroupDef As HspfTSGroupDef
  Dim vTSMemberDef As Variant, lTSMemberDef As HspfTSMemberDef
  
  Set pTableStatus = Nothing
  Set pTableStatus = New Collection
  If pStatusType = HspfTable Then
    With pOper.Uci.Msg.BlockDefs(pOper.Name)
      For Each vTableDef In .TableDefs
        Set lTableDef = vTableDef
        For i = 1 To lTableDef.NumOccur
          Set lTableStatus = New HspfStatusType
          lTableStatus.Name = lTableDef.Name
          lTableStatus.Occur = i
          lTableStatus.Max = lTableDef.NumOccur
          pTableStatus.Add lTableStatus
        Next i
      Next vTableDef
    End With
  ElseIf pStatusType = HspfInputTimeseries Or pStatusType = HspfOutputTimeseries Then
    For Each vTSGroupDef In pOper.Uci.Msg.TSGroupDefs
      Set lTSGroupDef = vTSGroupDef
      If lTSGroupDef.BlockId = pOper.optyp + 120 Then
        For Each vTSMemberDef In lTSGroupDef.MemberDefs
          Set lTSMemberDef = vTSMemberDef
          With lTSMemberDef
            addMember = False
            If pStatusType = HspfInputTimeseries Then
              If .mio > 0 Then
                addMember = True
              End If
            ElseIf pStatusType = HspfOutputTimeseries Then
              If .mio < 2 Then
                addMember = True
              End If
            End If
            If addMember Then
              'next 2 line are a kludge for performance! (impact RCHRES:CAT only?)
              ldim1 = .mdim1: If ldim1 = 100 Then ldim1 = 10
              ldim2 = .mdim2: If ldim2 = 100 Then ldim2 = 10
              For i = 1 To ldim1
                For j = 1 To ldim2
                  Set lTableStatus = New HspfStatusType
                  lTableStatus.Name = .Parent.Name & ":" & .Name
                  lOccur = ((j - 1) * (1000)) + i
                  lTableStatus.Occur = lOccur
                  lTableStatus.Max = .mdim1 * .mdim2
                  lTableStatus.Tag = .msect
                  Set lTableStatus.Defn = lTSMemberDef
                  pTableStatus.Add lTableStatus
                Next j
              Next i
            End If
          End With
        Next vTSMemberDef
      End If
    Next vTSGroupDef
  End If
  Update 'current status
End Sub

Public Function GetInfo(filterRON&, Optional filterPresent& = HspfStatusAny) As Collection
  Dim vTableStatus As Variant, lTableStatus As HspfStatusType
  Dim cGetInfo As Collection
  
  If pTableStatus.Count = 0 Then
    Build
  Else
    Update
  End If
  
  Set cGetInfo = Nothing
  Set cGetInfo = New Collection
  For Each vTableStatus In pTableStatus
    Set lTableStatus = vTableStatus
    If (filterPresent = HspfStatusAny Or _
        (CBool(filterPresent) = lTableStatus.Present)) Then
      If (filterRON = lTableStatus.ReqOptUnn) Then  'pbd changed
        cGetInfo.Add vTableStatus
      ElseIf (filterRON = 2 And lTableStatus.ReqOptUnn = 4) Then
        cGetInfo.Add vTableStatus
      End If
    End If
  Next vTableStatus
  Set GetInfo = cGetInfo
End Function

Public Function GetOutputInfo(filterRON&, Optional filterPresent& = HspfStatusAny) As Collection
  Dim vTableStatus As Variant, lTableStatus As HspfStatusType
  Dim cGetInfo As Collection
  
  If pTableStatus.Count = 0 Then
    Build
  Else
    Update
  End If
  
  Set cGetInfo = Nothing
  Set cGetInfo = New Collection
  For Each vTableStatus In pTableStatus
    Set lTableStatus = vTableStatus
    If (filterPresent = HspfStatusAny Or _
        (CBool(filterPresent) = lTableStatus.Present)) Then
      If (filterRON = lTableStatus.ReqOptUnn) Then
        cGetInfo.Add vTableStatus
      End If
    End If
  Next vTableStatus
  Set GetOutputInfo = cGetInfo
End Function

Public Sub init(newOper As HspfOperation)
  Set pOper = newOper
End Sub

Private Sub Class_Initialize()
  Set pTableStatus = New Collection
  pStatusType = HspfTable
End Sub
