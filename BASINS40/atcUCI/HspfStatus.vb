Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfStatus_NET.HspfStatus")> _
Public Class HspfStatus
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

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

    Private pStatusType As HspfStatusTypes
    Private pOper As HspfOperation
    Private pTableStatus As Collection 'of HspfStatusType

    Property StatusType() As HspfStatusTypes
        Get
            StatusType = pStatusType
        End Get
        Set(ByVal Value As HspfStatusTypes)
            pStatusType = Value
        End Set
    End Property

    ReadOnly Property TotalPossible() As Integer
        Get
            If pTableStatus.Count() = 0 Then Build()
            TotalPossible = pTableStatus.Count()
        End Get
    End Property

    Public Sub Change(ByRef Name As String, ByRef Occur As Integer, ByRef Status As Integer)
        Dim vTableStatus As Object
        Dim lTableStatus As HspfStatusType
        Dim found As Boolean

        found = False
        For Each vTableStatus In pTableStatus
            lTableStatus = vTableStatus
            If lTableStatus.Name = Name And lTableStatus.Occur = Occur Then
                lTableStatus.ReqOptUnn = Status
                found = True
                Exit For
            End If
        Next vTableStatus
        If Not (found) Then MsgBox("Change Failed For " & Name & "(" & Occur & ")")
    End Sub

    Public Sub Change2(ByRef Name As String, ByRef Occur1 As Integer, ByRef Occur2 As Integer, ByRef Status As Integer)
        Dim vTableStatus As Object
        Dim lTableStatus As HspfStatusType
        Dim found As Boolean
        Dim lOccur As Integer

        lOccur = (Occur2 - 1) * 1000 + Occur1
        found = False
        For Each vTableStatus In pTableStatus
            lTableStatus = vTableStatus
            If lTableStatus.Name = Name And lTableStatus.Occur = lOccur Then
                lTableStatus.ReqOptUnn = Status
                found = True
                Exit For
            End If
        Next vTableStatus
        If Not (found) Then MsgBox("Change Failed For " & Name & "(" & Occur1 & "," & Occur2 & ")")
    End Sub

    Public Sub Update()
        Dim j As Integer
        Dim vTable As Object
        Dim ltable As HspfTable
        Dim vTableStatus As Object
        Dim lTableStatus As HspfStatusType
        Dim vConnection As Object
        Dim lConnection As HspfConnection
        Dim cOccur, lOccur As Integer
        Dim lSub2, lSub1 As Integer
        Dim lMember As String = Nothing
        Dim lGroup As String = Nothing
        Dim lMemberStatus, lGroupStatus As String

        For Each vTableStatus In pTableStatus
            lTableStatus = vTableStatus
            lTableStatus.ReqOptUnn = HspfStatusReqOptUnnEnum.HspfStatusUnneeded
            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusMissing
        Next vTableStatus

        If pStatusType = HspfStatusTypes.HspfTable Then
            For Each vTable In pOper.Tables 'should this be in another loop
                ltable = vTable
                For Each vTableStatus In pTableStatus
                    lTableStatus = vTableStatus
                    If ltable.OccurNum = lTableStatus.Occur And ltable.Name = lTableStatus.Name Then
                        lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusPresent
                        Exit For
                    End If
                Next vTableStatus
            Next vTable
        ElseIf pStatusType = HspfStatusTypes.HspfInputTimeseries Then  'source
            For Each vConnection In pOper.Sources 'should this be in another loop
                lConnection = vConnection
                GetConnectionInfo(True, lConnection, lGroup, lMember, lSub1, lSub2, True)
                While Len(lGroup) > 0
                    cOccur = (lSub2 - 1) * 1000 + lSub1
                    For Each vTableStatus In pTableStatus
                        lTableStatus = vTableStatus
                        With lTableStatus
                            j = InStr(.Name, ":")
                            lGroupStatus = Left(.Name, j - 1)
                            lMemberStatus = Right(.Name, Len(.Name) - j)
                            lOccur = .Occur
                        End With
                        If cOccur = lOccur And (lMember = lMemberStatus Or Len(lMember) = 0) And lGroup = lGroupStatus Then
                            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusPresent
                            Exit For
                        End If
                    Next vTableStatus
                    GetConnectionInfo(True, lConnection, lGroup, lMember, lSub1, lSub2)
                End While
            Next vConnection
        Else 'target
            For Each vConnection In pOper.Targets
                lConnection = vConnection
                GetConnectionInfo(False, lConnection, lGroup, lMember, lSub1, lSub2, True)
                While Len(lGroup) > 0
                    For Each vTableStatus In pTableStatus
                        lTableStatus = vTableStatus
                        With lTableStatus
                            j = InStr(.Name, ":")
                            lGroupStatus = Left(.Name, j - 1)
                            lMemberStatus = Right(.Name, Len(.Name) - j)
                            lOccur = .Occur
                        End With
                        cOccur = (lSub2 - 1) * 1000 + lSub1
                        If cOccur = lOccur And (lMember = lMemberStatus Or Len(lMember) = 0) And lGroup = lGroupStatus Then
                            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusPresent
                            Exit For
                        End If
                    Next vTableStatus
                    GetConnectionInfo(False, lConnection, lGroup, lMember, lSub1, lSub2)
                End While
            Next vConnection
        End If

        If pStatusType = HspfStatusTypes.HspfTable Then
            Select Case pOper.Name
                Case "PERLND" : UpdatePerlnd(pOper, Me)
                Case "IMPLND" : UpdateImplnd(pOper, Me)
                Case "RCHRES" : UpdateRchres(pOper, Me)
                Case "COPY" : UpdateCopy(pOper, Me)
                Case "PLTGEN" : UpdatePltgen(pOper, Me)
                Case "DISPLY" : UpdateDisply(pOper, Me)
                Case "DURANL" : UpdateDuranl(pOper, Me)
                Case "GENER" : UpdateGener(pOper, Me)
                Case "MUTSIN" : UpdateMutsin(pOper, Me)
                Case "BMPRAC" : UpdateBmprac(pOper, Me)
                Case "REPORT" : UpdateReport(pOper, Me)
            End Select
        ElseIf pStatusType = HspfStatusTypes.HspfInputTimeseries Then
            Select Case pOper.Name
                Case "PERLND" : UpdateInputTimeseriesPerlnd(pOper, Me)
                Case "IMPLND" : UpdateInputTimeseriesImplnd(pOper, Me)
                Case "RCHRES" : UpdateInputTimeseriesRchres(pOper, Me)
                Case "COPY" : UpdateInputTimeseriesCopy(pOper, Me)
                Case "PLTGEN" : UpdateInputTimeseriesPltgen(pOper, Me)
                Case "DISPLY" : UpdateInputTimeseriesDisply(pOper, Me)
                Case "DURANL" : UpdateInputTimeseriesDuranl(pOper, Me)
                Case "GENER" : UpdateInputTimeseriesGener(pOper, Me)
                Case "MUTSIN" : UpdateInputTimeseriesMutsin(pOper, Me)
                Case "BMPRAC" : UpdateInputTimeseriesBmprac(pOper, Me)
                Case "REPORT" : UpdateInputTimeseriesReport(pOper, Me)
            End Select
        ElseIf pStatusType = HspfStatusTypes.HspfOutputTimeseries Then
            Select Case pOper.Name
                Case "PERLND" : UpdateOutputTimeseriesPerlnd(pOper, Me)
                Case "IMPLND" : UpdateOutputTimeseriesImplnd(pOper, Me)
                Case "RCHRES" : UpdateOutputTimeseriesRchres(pOper, Me)
                Case "COPY" : UpdateOutputTimeseriesCopy(pOper, Me)
                Case "PLTGEN" : UpdateOutputTimeseriesPltgen(pOper, Me)
                Case "DISPLY" : UpdateOutputTimeseriesDisply(pOper, Me)
                Case "DURANL" : UpdateOutputTimeseriesDuranl(pOper, Me)
                Case "GENER" : UpdateOutputTimeseriesGener(pOper, Me)
                Case "MUTSIN" : UpdateOutputTimeseriesMutsin(pOper, Me)
                Case "BMPRAC" : UpdateOutputTimeseriesBmprac(pOper, Me)
                Case "REPORT" : UpdateOutputTimeseriesReport(pOper, Me)
            End Select
        End If
    End Sub

    Public Sub UpdateExtTargetsOutputs()
        Dim j As Integer
        Dim vTableStatus As Object
        Dim lTableStatus As HspfStatusType
        Dim vConnection As Object
        Dim lConnection As HspfConnection
        Dim cOccur, lOccur As Integer
        Dim lSub2, lSub1 As Integer
        Dim lMember As String = ""
        Dim lGroup As String = ""
        Dim lMemberStatus, lGroupStatus As String

        If pTableStatus.Count() = 0 Then Build()

        For Each vTableStatus In pTableStatus
            lTableStatus = vTableStatus
            lTableStatus.ReqOptUnn = HspfStatusReqOptUnnEnum.HspfStatusUnneeded
            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusMissing
        Next vTableStatus

        For Each vConnection In pOper.Targets
            lConnection = vConnection
            If Left(lConnection.Target.VolName, 3) = "WDM" Then
                GetConnectionInfo(False, lConnection, lGroup, lMember, lSub1, lSub2, True)
                While Len(lGroup) > 0
                    For Each vTableStatus In pTableStatus
                        lTableStatus = vTableStatus
                        With lTableStatus
                            j = InStr(.Name, ":")
                            lGroupStatus = Left(.Name, j - 1)
                            lMemberStatus = Right(.Name, Len(.Name) - j)
                            lOccur = .Occur
                        End With
                        cOccur = (lSub2 - 1) * 1000 + lSub1
                        If cOccur = lOccur And (lMember = lMemberStatus Or Len(lMember) = 0) And lGroup = lGroupStatus Then
                            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusPresent
                            Exit For
                        End If
                    Next vTableStatus
                    GetConnectionInfo(False, lConnection, lGroup, lMember, lSub1, lSub2)
                End While
            End If
        Next vConnection

        Select Case pOper.Name
            Case "PERLND" : UpdateOutputTimeseriesPerlnd(pOper, Me)
            Case "IMPLND" : UpdateOutputTimeseriesImplnd(pOper, Me)
            Case "RCHRES" : UpdateOutputTimeseriesRchres(pOper, Me)
            Case "COPY" : UpdateOutputTimeseriesCopy(pOper, Me)
            Case "PLTGEN" : UpdateOutputTimeseriesPltgen(pOper, Me)
            Case "DISPLY" : UpdateOutputTimeseriesDisply(pOper, Me)
            Case "DURANL" : UpdateOutputTimeseriesDuranl(pOper, Me)
            Case "GENER" : UpdateOutputTimeseriesGener(pOper, Me)
            Case "MUTSIN" : UpdateOutputTimeseriesMutsin(pOper, Me)
            Case "BMPRAC" : UpdateOutputTimeseriesBmprac(pOper, Me)
            Case "REPORT" : UpdateOutputTimeseriesReport(pOper, Me)
        End Select

    End Sub

    Private Sub GetConnectionInfo(ByRef Source As Boolean, ByRef Connection As HspfConnection, ByRef Group As String, ByRef Member As String, ByRef Sub1 As Integer, ByRef Sub2 As Integer, Optional ByRef init As Boolean = False)
        Dim lMassLink As HspfMassLink
        Static massLinkPos As Integer

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
                    lMassLink = pOper.Uci.MassLinks(massLinkPos)
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
                        massLinkPos += 1
                    End If
                Else 'done
                    Group = ""
                End If
            End While
        End If
        massLinkPos += 1
        If Sub1 = 0 Then Sub1 = 1
        If Sub2 = 0 Then Sub2 = 1

    End Sub

    Private Sub Build()
        Dim vTableDef As Object
        Dim lTableDef As HspfTableDef
        Dim ldim1, i, j, ldim2 As Integer
        Dim lTableStatus As HspfStatusType
        Dim lOccur As Integer
        Dim addMember As Boolean
        Dim lTSGroupDef As HspfTSGroupDef
        Dim lTSMemberDef As HspfTSMemberDef

        'UPGRADE_NOTE: Object pTableStatus may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        pTableStatus = Nothing
        pTableStatus = New Collection
        If pStatusType = HspfStatusTypes.HspfTable Then
            With pOper.Uci.Msg.BlockDefs(pOper.Name) 'as HspfBlockDef
                For Each vTableDef In .TableDefs
                    lTableDef = vTableDef
                    For i = 1 To lTableDef.NumOccur
                        lTableStatus = New HspfStatusType
                        lTableStatus.Name = lTableDef.Name
                        lTableStatus.Occur = i
                        lTableStatus.Max = lTableDef.NumOccur
                        pTableStatus.Add(lTableStatus)
                    Next i
                Next vTableDef
            End With
        ElseIf pStatusType = HspfStatusTypes.HspfInputTimeseries Or pStatusType = HspfStatusTypes.HspfOutputTimeseries Then
            For Each lTSGroupDef In pOper.Uci.Msg.TSGroupDefs
                If lTSGroupDef.BlockId = pOper.optyp + 120 Then
                    For Each lTSMemberDef In lTSGroupDef.MemberDefs
                        With lTSMemberDef
                            addMember = False
                            If pStatusType = HspfStatusTypes.HspfInputTimeseries Then
                                If .mio > 0 Then
                                    addMember = True
                                End If
                            ElseIf pStatusType = HspfStatusTypes.HspfOutputTimeseries Then
                                If .mio < 2 Then
                                    addMember = True
                                End If
                            End If
                            If addMember Then
                                'next 2 line are a kludge for performance! (impact RCHRES:CAT only?)
                                ldim1 = .mdim1 : If ldim1 = 100 Then ldim1 = 10
                                ldim2 = .mdim2 : If ldim2 = 100 Then ldim2 = 10
                                For i = 1 To ldim1
                                    For j = 1 To ldim2
                                        lTableStatus = New HspfStatusType
                                        lTableStatus.Name = .Parent.Name & ":" & .Name
                                        lOccur = ((j - 1) * (1000)) + i
                                        lTableStatus.Occur = lOccur
                                        lTableStatus.Max = .mdim1 * .mdim2
                                        lTableStatus.Tag = CStr(.msect)
                                        lTableStatus.Defn = lTSMemberDef
                                        pTableStatus.Add(lTableStatus)
                                    Next j
                                Next i
                            End If
                        End With
                    Next
                End If
            Next
        End If
        Update() 'current status
    End Sub

    Public Function GetInfo(ByRef filterRON As Integer, Optional ByRef filterPresent As Integer = HspfStatusPresentMissingEnum.HspfStatusAny) As Collection
        Dim vTableStatus As Object
        Dim lTableStatus As HspfStatusType
        Dim cGetInfo As Collection

        If pTableStatus.Count() = 0 Then
            Build()
        Else
            Update()
        End If

        'UPGRADE_NOTE: Object cGetInfo may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cGetInfo = Nothing
        cGetInfo = New Collection
        For Each vTableStatus In pTableStatus
            lTableStatus = vTableStatus
            If (filterPresent = HspfStatusPresentMissingEnum.HspfStatusAny Or (CBool(filterPresent) = lTableStatus.Present)) Then
                If (filterRON = lTableStatus.ReqOptUnn) Then 'pbd changed
                    cGetInfo.Add(vTableStatus)
                ElseIf (filterRON = 2 And lTableStatus.ReqOptUnn = 4) Then
                    cGetInfo.Add(vTableStatus)
                End If
            End If
        Next vTableStatus
        GetInfo = cGetInfo
    End Function

    Public Function GetOutputInfo(ByRef filterRON As Integer, Optional ByRef filterPresent As Integer = HspfStatusPresentMissingEnum.HspfStatusAny) As Collection
        Dim vTableStatus As Object
        Dim lTableStatus As HspfStatusType
        Dim cGetInfo As Collection

        If pTableStatus.Count() = 0 Then
            Build()
        Else
            Update()
        End If

        'UPGRADE_NOTE: Object cGetInfo may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cGetInfo = Nothing
        cGetInfo = New Collection
        For Each vTableStatus In pTableStatus
            lTableStatus = vTableStatus
            If (filterPresent = HspfStatusPresentMissingEnum.HspfStatusAny Or (CBool(filterPresent) = lTableStatus.Present)) Then
                If (filterRON = lTableStatus.ReqOptUnn) Then
                    cGetInfo.Add(vTableStatus)
                End If
            End If
        Next vTableStatus
        GetOutputInfo = cGetInfo
    End Function

    Public Sub init(ByRef newOper As HspfOperation)
        pOper = newOper
    End Sub

    Public Sub New()
        MyBase.New()
        pTableStatus = New Collection
        pStatusType = HspfStatusTypes.HspfTable
    End Sub
End Class