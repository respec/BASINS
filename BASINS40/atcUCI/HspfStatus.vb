'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On
Imports System.Collections.ObjectModel
Imports MapWinUtility

Public Class HspfStatus
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

    Private pOper As HspfOperation
    Private pTableStatus As Collection(Of HspfStatusType)

    Public StatusType As HspfStatusTypes

    ReadOnly Property TotalPossible() As Integer
        Get
            If pTableStatus.Count = 0 Then
                Build()
            End If
            Return pTableStatus.Count
        End Get
    End Property

    Public Sub Change(ByRef aName As String, ByRef aOccur As Integer, ByRef aStatus As Integer)
        Dim lFound As Boolean = False
        For Each lTableStatus As HspfStatusType In pTableStatus
            If lTableStatus.Name = aName And lTableStatus.Occur = aOccur Then
                lTableStatus.ReqOptUnn = aStatus
                lFound = True
                Exit For
            End If
        Next lTableStatus
        If Not (lFound) Then
            Logger.Msg("Change Failed For " & aName & "(" & aOccur & ")")
        End If
    End Sub

    Public Sub Change2(ByRef aName As String, _
                       ByRef aOccur1 As Integer, ByRef aOccur2 As Integer, _
                       ByRef aStatus As Integer)
        Dim lFound As Boolean = False
        Dim lOccur As Integer = (aOccur2 - 1) * 1000 + aOccur1
        For Each lTableStatus As HspfStatusType In pTableStatus
            If lTableStatus.Name = aName And lTableStatus.Occur = lOccur Then
                lTableStatus.ReqOptUnn = aStatus
                lFound = True
                Exit For
            End If
        Next lTableStatus
        If Not (lFound) Then
            Logger.Msg("Change Failed For " & aName & "(" & aOccur1 & "," & aOccur2 & ")")
        End If
    End Sub

    Public Sub Update()
        For Each lTableStatus As HspfStatusType In pTableStatus
            lTableStatus.ReqOptUnn = HspfStatusReqOptUnnEnum.HspfStatusUnneeded
            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusMissing
        Next lTableStatus

        If StatusType = HspfStatusTypes.HspfTable Then
            For Each lTable As HspfTable In pOper.Tables 'should this be in another loop
                For Each lTableStatus As HspfStatusType In pTableStatus
                    If lTable.OccurNum = lTableStatus.Occur And lTable.Name = lTableStatus.Name Then
                        lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusPresent
                        Exit For
                    End If
                Next lTableStatus
            Next lTable
        ElseIf StatusType = HspfStatusTypes.HspfInputTimeseries Then  'source
            For Each lConnection As HspfConnection In pOper.Sources 'should this be in another loop
                Dim lSub2, lSub1 As Integer
                Dim lMember As String = Nothing
                Dim lGroup As String = Nothing
                Dim lMemberStatus, lGroupStatus As String
                GetConnectionInfo(True, lConnection, lGroup, lMember, lSub1, lSub2, True)
                While lGroup.Length > 0
                    Dim cOccur As Integer = (lSub2 - 1) * 1000 + lSub1
                    Dim lOccur As Integer
                    For Each lTableStatus As HspfStatusType In pTableStatus
                        With lTableStatus
                            Dim j As Integer = InStr(.Name, ":")
                            lGroupStatus = Left(.Name, j - 1)
                            lMemberStatus = Right(.Name, Len(.Name) - j)
                            lOccur = .Occur
                        End With
                        If cOccur = lOccur And (lMember = lMemberStatus Or Len(lMember) = 0) And lGroup = lGroupStatus Then
                            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusPresent
                            Exit For
                        End If
                    Next lTableStatus
                    GetConnectionInfo(True, lConnection, lGroup, lMember, lSub1, lSub2)
                End While
            Next lConnection
        Else 'target
            For Each lConnection As HspfConnection In pOper.Targets
                Dim lSub2, lSub1 As Integer
                Dim lMember As String = Nothing
                Dim lGroup As String = Nothing
                Dim lMemberStatus, lGroupStatus As String
                GetConnectionInfo(False, lConnection, lGroup, lMember, lSub1, lSub2, True)
                While Len(lGroup) > 0
                    For Each lTableStatus As HspfStatusType In pTableStatus
                        Dim lOccur As Integer
                        With lTableStatus
                            Dim j As Integer = InStr(.Name, ":")
                            lGroupStatus = Left(.Name, j - 1)
                            lMemberStatus = Right(.Name, Len(.Name) - j)
                            lOccur = .Occur
                        End With
                        Dim cOccur As Integer = (lSub2 - 1) * 1000 + lSub1
                        If cOccur = lOccur And (lMember = lMemberStatus Or Len(lMember) = 0) And lGroup = lGroupStatus Then
                            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusPresent
                            Exit For
                        End If
                    Next lTableStatus
                    GetConnectionInfo(False, lConnection, lGroup, lMember, lSub1, lSub2)
                End While
            Next lConnection
        End If

        If StatusType = HspfStatusTypes.HspfTable Then
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
        ElseIf StatusType = HspfStatusTypes.HspfInputTimeseries Then
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
        ElseIf StatusType = HspfStatusTypes.HspfOutputTimeseries Then
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
        If pTableStatus.Count = 0 Then
            Build()
        End If

        For Each lTableStatus As HspfStatusType In pTableStatus
            lTableStatus.ReqOptUnn = HspfStatusReqOptUnnEnum.HspfStatusUnneeded
            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusMissing
        Next lTableStatus

        For Each lConnection As HspfConnection In pOper.Targets
            If lConnection.Target.VolName.StartsWith("WDM") Then
                Dim lGroup As String = ""
                Dim lMember As String = ""
                Dim lSub2, lSub1 As Integer
                Dim lMemberStatus, lGroupStatus As String
                GetConnectionInfo(False, lConnection, lGroup, lMember, lSub1, lSub2, True)
                While lGroup.Length > 0
                    For Each lTableStatus As HspfStatusType In pTableStatus
                        Dim lOccur As Integer
                        With lTableStatus
                            Dim lColonIndex As Integer = InStr(.Name, ":")
                            lGroupStatus = Left(.Name, lColonIndex - 1)
                            lMemberStatus = Right(.Name, Len(.Name) - lColonIndex)
                            lOccur = .Occur
                        End With
                        Dim cOccur As Integer = (lSub2 - 1) * 1000 + lSub1
                        If cOccur = lOccur And (lMember = lMemberStatus Or Len(lMember) = 0) And lGroup = lGroupStatus Then
                            lTableStatus.Present = HspfStatusPresentMissingEnum.HspfStatusPresent
                            Exit For
                        End If
                    Next lTableStatus
                    GetConnectionInfo(False, lConnection, lGroup, lMember, lSub1, lSub2)
                End While
            End If
        Next lConnection

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

    Private Sub GetConnectionInfo(ByRef aSource As Boolean, _
                                  ByRef aConnection As HspfConnection, _
                                  ByRef aGroup As String, _
                                  ByRef aMember As String, _
                                  ByRef aSub1 As Integer, _
                                  ByRef aSub2 As Integer, _
                                  Optional ByRef aInit As Boolean = False)
        Static lMassLinkPos As Integer 'save between calls

        If aInit Then
            lMassLinkPos = 1
        End If

        If aConnection.MassLink = 0 Then
            If lMassLinkPos = 1 Then
                If aSource Then
                    aGroup = aConnection.Target.Group
                    aMember = aConnection.Target.Member
                    aSub1 = aConnection.Target.MemSub1
                    aSub2 = aConnection.Target.MemSub2
                Else
                    aGroup = aConnection.Source.Group
                    aMember = aConnection.Source.Member
                    aSub1 = aConnection.Source.MemSub1
                    aSub2 = aConnection.Source.MemSub2
                End If
            Else 'only wanted one
                aGroup = ""
            End If
        Else
            aGroup = "?"
            While aGroup = "?"
                If lMassLinkPos <= pOper.Uci.MassLinks.Count Then
                    Dim lMassLink As HspfMassLink = pOper.Uci.MassLinks(lMassLinkPos)
                    If lMassLink.MassLinkID = aConnection.MassLink Then
                        If aSource Then
                            aGroup = lMassLink.Target.Group
                            aMember = lMassLink.Target.Member
                            aSub1 = lMassLink.Target.MemSub1
                            aSub2 = lMassLink.Target.MemSub2
                        Else
                            aGroup = lMassLink.Source.Group
                            aMember = lMassLink.Source.Member
                            aSub1 = lMassLink.Source.MemSub1
                            aSub2 = lMassLink.Source.MemSub2
                        End If
                    Else
                        lMassLinkPos += 1
                    End If
                Else 'done
                    aGroup = ""
                End If
            End While
        End If
        lMassLinkPos += 1
        If aSub1 = 0 Then aSub1 = 1
        If aSub2 = 0 Then aSub2 = 1

    End Sub

    Private Sub Build()
        pTableStatus = New Collection(Of HspfStatusType)

        If StatusType = HspfStatusTypes.HspfTable Then
            With pOper.Uci.Msg.BlockDefs(pOper.Name) 'as HspfBlockDef
                For Each lTableDef As HspfTableDef In .TableDefs
                    For lOccurIndex As Integer = 1 To lTableDef.NumOccur
                        Dim lTableStatus As HspfStatusType = New HspfStatusType
                        lTableStatus.Name = lTableDef.Name
                        lTableStatus.Occur = lOccurIndex
                        lTableStatus.Max = lTableDef.NumOccur
                        pTableStatus.Add(lTableStatus)
                    Next lOccurIndex
                Next lTableDef
            End With
        ElseIf StatusType = HspfStatusTypes.HspfInputTimeseries Or _
               StatusType = HspfStatusTypes.HspfOutputTimeseries Then
            For Each lTSGroupDef As HspfTSGroupDef In pOper.Uci.Msg.TSGroupDefs
                If lTSGroupDef.BlockID = pOper.OpTyp + 120 Then
                    For Each lTSMemberDef As HspfTSMemberDef In lTSGroupDef.MemberDefs
                        With lTSMemberDef
                            Dim lAddMember As Boolean = False
                            If StatusType = HspfStatusTypes.HspfInputTimeseries Then
                                If .Mio > 0 Then
                                    lAddMember = True
                                End If
                            ElseIf StatusType = HspfStatusTypes.HspfOutputTimeseries Then
                                If .Mio < 2 Then
                                    lAddMember = True
                                End If
                            End If
                            If lAddMember Then
                                'next 2 lines are a kludge for performance! (impact RCHRES:CAT only?)
                                Dim lDim1 As Integer = .MDim1 : If lDim1 = 100 Then lDim1 = 10
                                Dim lDim2 As Integer = .MDim2 : If lDim2 = 100 Then lDim2 = 10
                                For i As Integer = 1 To lDim1
                                    For j As Integer = 1 To lDim2
                                        Dim lTableStatus As HspfStatusType = New HspfStatusType
                                        lTableStatus.Name = .Parent.Name & ":" & .Name
                                        Dim lOccur As Integer = ((j - 1) * (1000)) + i
                                        lTableStatus.Occur = lOccur
                                        lTableStatus.Max = .MDim1 * .MDim2
                                        lTableStatus.Tag = CStr(.Msect)
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

    Public Function GetInfo(ByRef aFilterRON As Integer, _
                   Optional ByRef aFilterPresent As Integer = HspfStatusPresentMissingEnum.HspfStatusAny) _
                   As Collection(Of HspfStatusType)
        If pTableStatus.Count = 0 Then
            Build()
        Else
            Update()
        End If

        Dim lGetInfo As New Collection(Of HspfStatusType)
        For Each lTableStatus As HspfStatusType In pTableStatus
            If (aFilterPresent = HspfStatusPresentMissingEnum.HspfStatusAny Or _
              (CBool(aFilterPresent) = lTableStatus.Present)) Then
                If (aFilterRON = lTableStatus.ReqOptUnn) Then 'pbd changed
                    lGetInfo.Add(lTableStatus)
                ElseIf (aFilterRON = 2 And lTableStatus.ReqOptUnn = 4) Then
                    lGetInfo.Add(lTableStatus)
                End If
            End If
        Next lTableStatus
        Return lGetInfo
    End Function

    Public Function GetOutputInfo(ByRef aFilterRON As Integer, _
                         Optional ByRef aFilterPresent As Integer = HspfStatusPresentMissingEnum.HspfStatusAny) _
                               As Collection(Of HspfStatusType)
        If pTableStatus.Count = 0 Then
            Build()
        Else
            Update()
        End If

        Dim lGetInfo As New Collection(Of HspfStatusType)
        For Each lTableStatus As HspfStatusType In pTableStatus
            If (aFilterPresent = HspfStatusPresentMissingEnum.HspfStatusAny Or _
               (CBool(aFilterPresent) = lTableStatus.Present)) Then
                If (aFilterRON = lTableStatus.ReqOptUnn) Then
                    lGetInfo.Add(lTableStatus)
                End If
            End If
        Next lTableStatus
        Return lGetInfo
    End Function

    Public Sub Init(ByRef aNewOper As HspfOperation)
        pOper = aNewOper
    End Sub

    Public Sub New()
        MyBase.New()
        pTableStatus = New Collection(Of HspfStatusType)
        StatusType = HspfStatusTypes.HspfTable
    End Sub
End Class