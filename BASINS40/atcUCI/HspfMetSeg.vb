'Copyright 2006-7 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Option Strict Off
Option Explicit On

Imports System.Text

Public Class HspfMetSeg
    Private pMetSegRecs(7) As HspfMetSegRecord
    Private pId As Integer
    Private pName As String
    Private pUci As HspfUci
    Private pAirType As Integer '1-GATMP, 2-AIRTMP

    Public Property MetSegRecs() As HspfMetSegRecord()
        Get
            Return pMetSegRecs
        End Get
        Set(ByVal Value() As HspfMetSegRecord)
            For itype As Integer = 1 To pMetSegRecs.GetUpperBound(0)
                pMetSegRecs(itype) = Value(itype)
            Next
        End Set
    End Property

    Public ReadOnly Property MetSegRec(ByVal aMetSegRecordType As HspfMetSegRecord.MetSegRecordType) As HspfMetSegRecord
        Get
            Return pMetSegRecs(aMetSegRecordType)
        End Get
    End Property

    Public Property Id() As Integer
        Get
            Return pId
        End Get
        Set(ByVal Value As Integer)
            pId = Value
        End Set
    End Property

    Public Property AirType() As Integer
        Get
            Return pAirType
        End Get
        Set(ByVal Value As Integer)
            pAirType = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return pName
        End Get
        Set(ByVal Value As String)
            pName = Value
        End Set
    End Property

    Public Property Uci() As HspfUci
        Get
            Return pUci
        End Get
        Set(ByVal Value As HspfUci)
            pUci = Value
        End Set
    End Property

    Public Function Add(ByRef aNewConnection As HspfConnection) As Boolean
        Dim lMetSegRec As New HspfMetSegRecord
        If aNewConnection.Target.VolName = "RCHRES" Then
            lMetSegRec.MFactR = aNewConnection.MFact
        ElseIf aNewConnection.Target.VolName = "PERLND" Or _
               aNewConnection.Target.VolName = "IMPLND" Then
            lMetSegRec.MFactP = aNewConnection.MFact
        End If
        lMetSegRec.Source = aNewConnection.Source
        lMetSegRec.Tran = aNewConnection.Tran
        lMetSegRec.Sgapstrg = aNewConnection.Sgapstrg
        lMetSegRec.Ssystem = aNewConnection.Ssystem
        lMetSegRec.typ = Str2Type(aNewConnection.Target.Member)

        Dim lResult As Boolean = True

        If aNewConnection.Target.VolName = "PERLND" Or _
           aNewConnection.Target.VolName = "IMPLND" Or _
           aNewConnection.Target.VolName = "RCHRES" Then
            Dim lTyp As Integer = lMetSegRec.typ
            If lTyp <> HspfMetSegRecord.MetSegRecordType.msrUNK Then
                If Len(pMetSegRecs(lTyp).Source.VolName) > 0 Then
                    'dont add if already have this type of record
                    lResult = False
                Else
                    pMetSegRecs(lTyp) = Nothing
                    pMetSegRecs(lTyp) = lMetSegRec
                    If aNewConnection.Target.Member = "GATMP" Then
                        pAirType = 1
                    ElseIf aNewConnection.Target.Member = "AIRTMP" Then
                        pAirType = 2
                    End If
                End If
            Else ' not needed
                lResult = False
            End If
        Else
            lResult = False
        End If

        If Not lResult Then
            lMetSegRec = Nothing
        End If
        Return lResult
    End Function

    Private Function Str2Type(ByRef aStr As String) As HspfMetSegRecord.MetSegRecordType
        Select Case aStr
            Case "PREC" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrPREC
            Case "GATMP" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrGATMP
            Case "AIRTMP" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrGATMP
            Case "DTMPG" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrDTMPG
            Case "DEWTMP" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrDTMPG
            Case "WINMOV" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrWINMOV
            Case "WIND" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrWINMOV
            Case "SOLRAD" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrSOLRAD
            Case "CLOUD" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrCLOUD
            Case "PETINP" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrPETINP
            Case "POTEV" : Str2Type = HspfMetSegRecord.MetSegRecordType.msrPETINP
            Case Else : Str2Type = HspfMetSegRecord.MetSegRecordType.msrUNK
        End Select
    End Function

    Public Function Compare(ByRef newMetSeg As HspfMetSeg, ByRef opname As String) As Boolean
        For ltype As Integer = 1 To pMetSegRecs.GetUpperBound(0)
            If Not (pMetSegRecs(ltype).Compare(newMetSeg.MetSegRecs(ltype), opname)) Then
                Return False
            End If
        Next ltype
        Return True
    End Function

    Public Sub UpdateMetSeg(ByRef newMetSeg As HspfMetSeg)
        For lType As Integer = 1 To pMetSegRecs.GetUpperBound(0)
            With newMetSeg.MetSegRecs(lType)
                If pMetSegRecs(lType).MFactR = -999.0# And .MFactR <> -999.0# Then
                    pMetSegRecs(lType).MFactR = .MFactR
                    pMetSegRecs(lType).Sgapstrg = .Sgapstrg
                    pMetSegRecs(lType).Source = .Source
                    pMetSegRecs(lType).Ssystem = .Ssystem
                    pMetSegRecs(lType).Tran = .Tran
                    pMetSegRecs(lType).typ = .typ
                End If
            End With
        Next lType
    End Sub

    Public Sub New()
        MyBase.New()
        For lType As Integer = 1 To pMetSegRecs.GetUpperBound(0)
            pMetSegRecs(lType) = New HspfMetSegRecord
        Next
        pAirType = 0
    End Sub

    Public Sub ExpandMetSegName(ByRef aWdmId As String, ByRef aDsn As Integer)
        Dim lType As Integer
        Dim lAddStr As String = ""
        Dim lCon As String = ""

        Me.Name = Me.Uci.GetWDMAttr(aWdmId, aDsn, "LOC")

        For lType = 1 To pMetSegRecs.GetUpperBound(0)
            Select Case lType
                Case 1 : lCon = "PREC"
                Case 2 : lCon = "GATMP"
                Case 3 : lCon = "DTMPG"
                Case 4 : lCon = "WINMOV"
                Case 5 : lCon = "SOLRAD"
                Case 6 : lCon = "CLOUD"
                Case 7 : lCon = "PETINP"
                Case Else : lCon = "<unknown>"
            End Select
            If lType = 2 And Me.AirType = 2 Then
                lCon = "AIRTMP"
            End If
            If pMetSegRecs(lType).MFactP <> 1 And _
               pMetSegRecs(lType).MFactP <> 0 And _
               pMetSegRecs(lType).MFactP <> -999 Then
                lAddStr &= ",PI:" & lCon & "=" & CStr(pMetSegRecs(lType).MFactP)
            End If
            If pMetSegRecs(lType).MFactR <> 1 And _
               pMetSegRecs(lType).MFactR <> 0 And _
               pMetSegRecs(lType).MFactR <> -999 Then
                lAddStr &= ",R:" & lCon & "=" & CStr(pMetSegRecs(lType).MFactR)
            End If
        Next lType

        If lAddStr.Length > 0 Then
            Me.Name &= lAddStr
        End If
    End Sub

    Public Function ToStringFromSpecs(ByRef aOpTyp As String, _
                                      ByRef aCol() As Integer, _
                                      ByRef aLen() As Integer) As String
        Dim lSB As New StringBuilder

        Dim lFirstId As Integer = 0
        Dim lLastId As Integer = 0
        For lOpnId As Integer = 1 To pUci.OpnBlks.Item(aOpTyp).Ids.Count
            Dim lOpn As HspfOperation = pUci.OpnBlks.Item(aOpTyp).NthOper(lOpnId)
            If lOpn.MetSeg.Id = Me.Id Then
                If lFirstId = 0 Then
                    lFirstId = lOpn.Id
                Else
                    lLastId = lOpn.Id
                End If
            ElseIf lFirstId > 0 Then
                lSB.Append(FormatRecords(aOpTyp, lFirstId, lLastId, aCol, aLen))
                lFirstId = 0
                lLastId = 0
            End If
        Next lOpnId
        If lFirstId > 0 Then
            lSB.Append(FormatRecords(aOpTyp, lFirstId, lLastId, aCol, aLen))
        End If
        Return lSB.ToString
    End Function

    Public Function FormatRecords(ByRef aOpTyp As String, _
                                  ByRef aFirstId As Integer, ByRef aLastId As Integer, _
                                  ByRef aCol() As Integer, ByRef aLen() As Integer) As String
        Dim lSB As New System.Text.StringBuilder

        For lSegRec As Integer = 1 To 7
            With pMetSegRecs(lSegRec)
                If .typ <> 0 Then 'type exists
                    If (aOpTyp = "RCHRES" And .MFactR > 0.0#) Or _
                       (aOpTyp = "PERLND" And .MFactP > 0.0#) Or _
                       (aOpTyp = "IMPLND" And .MFactP > 0.0#) Then
                        'have this type of met seg record
                        If lSegRec = 1 Then
                            lSB.AppendLine("*** Met Seg " & pName)
                        End If
                        Dim lStr As New StringBuilder
                        lStr.Append(.Source.VolName.Trim.PadRight(aCol(1) - 1))
                        lStr.Append(CStr(.Source.VolId).PadLeft(aLen(1)))
                        lStr.Append(Space(aCol(2) - lStr.Length - 1))
                        lStr.Append(.Source.Member)
                        lStr.Append(Space(aCol(3) - lStr.Length - 1))
                        If .Source.MemSub1 <> 0 Then
                            lStr.Append(CStr(.Source.MemSub1).PadLeft(aLen(3)))
                        End If
                        lStr.Append(Space(aCol(4) - lStr.Length - 1))
                        lStr.Append(.Ssystem)
                        lStr.Append(Space(aCol(5) - lStr.Length - 1))
                        lStr.Append(.Sgapstrg)
                        lStr.Append(Space(aCol(6) - lStr.Length - 1))
                        If aOpTyp = "RCHRES" Then
                            If .MFactR <> 1 Then
                                lStr.Append(CStr(.MFactR).PadLeft(aLen(6)))
                            End If
                        Else
                            If .MFactP <> 1 Then
                                lStr.Append(CStr(.MFactP).PadLeft(aLen(6)))
                            End If
                        End If
                        lStr.Append(Space(aCol(7) - lStr.Length - 1))
                        lStr.Append(.Tran)
                        lStr.Append(Space(aCol(8) - lStr.Length - 1))
                        lStr.Append(aOpTyp)
                        lStr.Append(Space(aCol(9) - lStr.Length - 1))
                        lStr.Append(CStr(aFirstId).PadLeft(aLen(9)))
                        If aLastId > 0 Then
                            lStr.Append(Space(aCol(10) - lStr.Length - 1))
                            lStr.Append(CStr(aLastId).PadLeft(aLen(9)))
                        End If
                        lStr.Append(Space(aCol(11) - lStr.Length - 1))
                        If aOpTyp <> "RCHRES" And pAirType = 2 And .typ = 2 Then
                            lStr.Append("ATEMP")
                        Else
                            lStr.Append("EXTNL")
                        End If
                        lStr.Append(Space(aCol(12) - lStr.Length - 1))

                        Dim lMember As String = ""
                        If aOpTyp = "RCHRES" Then
                            Select Case .typ
                                Case 1 : lMember = "PREC"
                                Case 2 : lMember = "GATMP"
                                Case 3 : lMember = "DEWTMP"
                                Case 4 : lMember = "WIND"
                                Case 5 : lMember = "SOLRAD"
                                Case 6 : lMember = "CLOUD"
                                Case 7 : lMember = "POTEV"
                            End Select
                        Else
                            Select Case .typ
                                Case 1 : lMember = "PREC"
                                Case 2 : lMember = "GATMP"
                                Case 3 : lMember = "DTMPG"
                                Case 4 : lMember = "WINMOV"
                                Case 5 : lMember = "SOLRAD"
                                Case 6 : lMember = "CLOUD"
                                Case 7 : lMember = "PETINP"
                            End Select
                            If .typ = 2 Then
                                'get right air temp member name
                                If pAirType = 1 Then
                                    lMember = "GATMP"
                                ElseIf pAirType = 2 Then
                                    lMember = "AIRTMP"
                                End If
                            End If
                        End If
                        lStr.Append(lMember)
                        lStr.Append(Space(aCol(13) - lStr.Length - 1))
                        lSB.AppendLine(lStr.ToString)
                    End If
                End If
            End With
        Next lSegRec
        Return lSB.ToString.Remove(lSB.Length - 2) 'remove CRLF
    End Function
End Class