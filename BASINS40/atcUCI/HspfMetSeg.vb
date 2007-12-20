'Copyright 2006-7 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Option Strict Off
Option Explicit On

Imports System.Text

Public Class HspfMetSeg
    Private pMetSegRecs As New HspfMetSegRecords
    Public Id As Integer
    Public Name As String
    Public Uci As HspfUci
    Public AirType As Integer '1-GATMP, 2-AIRTMP

    Public ReadOnly Property MetSegRecs() As HspfMetSegRecords
        Get
            Return pMetSegRecs
        End Get
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
        lMetSegRec.Name = Str2Name(aNewConnection.Target.Member)

        Dim lResult As Boolean = True

        If aNewConnection.Target.VolName = "PERLND" Or _
           aNewConnection.Target.VolName = "IMPLND" Or _
           aNewConnection.Target.VolName = "RCHRES" Then
            If pMetSegRecs(lMetSegRec.Name).Source.VolName.Length > 0 Then
                'dont add if already have this type of record
                lResult = False
            Else
                pMetSegRecs.Add(lMetSegRec)
                If aNewConnection.Target.Member = "GATMP" Then
                    AirType = 1
                ElseIf aNewConnection.Target.Member = "AIRTMP" Then
                    AirType = 2
                End If
            End If
        End If
        Return lResult
    End Function

    Private Function Str2Name(ByRef aStr As String) As String
        Dim lName As String = ""
        Select Case aStr
            Case "PREC" : lName = "PREC"
            Case "GATMP" : lName = "ATEM"
            Case "AIRTMP" : lName = "ATEM"
            Case "DTMPG" : lName = "DEWP"
            Case "DEWTMP" : lName = "DEWP"
            Case "WINMOV" : lName = "WIND"
            Case "WIND" : lName = "WIND"
            Case "SOLRAD" : lName = "SOLR"
            Case "CLOUD" : lName = "CLOU"
            Case "PETINP" : lName = "CLOU"
            Case "POTEV" : lName = "PEVT"
        End Select
        Return lName
    End Function

    Public Function Compare(ByRef newMetSeg As HspfMetSeg, ByRef opname As String) As Boolean
        For Each lMetSegRec As HspfMetSegRecord In pMetSegRecs
            If Not (lMetSegRec.Compare(newMetSeg.MetSegRecs(lMetSegRec.Name), opname)) Then
                Return False
            End If
        Next lMetSegRec
        Return True
    End Function

    Public Sub UpdateMetSeg(ByRef newMetSeg As HspfMetSeg)
        For Each lMetSegRec As HspfMetSegRecord In pMetSegRecs
            With newMetSeg.MetSegRecs(lMetSegRec.Name)
                If lMetSegRec.MFactR = -999.0# And .MFactR <> -999.0# Then
                    lMetSegRec.MFactR = .MFactR
                    lMetSegRec.Sgapstrg = .Sgapstrg
                    lMetSegRec.Source = .Source
                    lMetSegRec.Ssystem = .Ssystem
                    lMetSegRec.Tran = .Tran
                End If
            End With
        Next lMetSegRec
    End Sub

    Public Sub New()
        MyBase.New()
        AirType = 0
    End Sub

    Public Sub ExpandMetSegName(ByRef aWdmId As String, ByRef aDsn As Integer)
        Dim lType As Integer
        Dim lAddStr As String = ""
        Dim lCon As String = ""

        Me.Name = Me.Uci.GetWDMAttr(aWdmId, aDsn, "LOC")

        For Each lMetSegRec As HspfMetSegRecord In pMetSegRecs
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
        Next lMetSegRec

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
        For lOpnId As Integer = 1 To Uci.OpnBlks.Item(aOpTyp).Ids.Count
            Dim lOpn As HspfOperation = Uci.OpnBlks.Item(aOpTyp).NthOper(lOpnId)
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
        Dim lSB As New StringBuilder

        Dim lInit As Boolean = True
        For Each lMetSegRec As HspfMetSegRecord In pMetSegRecs
            With lMetSegRec
                If (aOpTyp = "RCHRES" And .MFactR > 0.0#) Or _
                       (aOpTyp = "PERLND" And .MFactP > 0.0#) Or _
                       (aOpTyp = "IMPLND" And .MFactP > 0.0#) Then
                    'have this type of met seg record
                    If lInit Then
                        lSB.AppendLine("*** Met Seg " & Name)
                        lInit = False
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
                    If aOpTyp <> "RCHRES" And AirType = 2 And .Name = "ATEM" Then
                        lStr.Append("ATEMP")
                    Else
                        lStr.Append("EXTNL")
                    End If
                    lStr.Append(Space(aCol(12) - lStr.Length - 1))

                    Dim lMember As String = ""
                    If aOpTyp = "RCHRES" Then
                        Select Case .Name
                            Case "PREC" : lMember = "PREC"
                            Case "ATEM" : lMember = "GATMP"
                            Case "DEWP" : lMember = "DEWTMP"
                            Case "WIND" : lMember = "WIND"
                            Case "SOLR" : lMember = "SOLRAD"
                            Case "CLOU" : lMember = "CLOUD"
                            Case "PEVT" : lMember = "POTEV"
                        End Select
                    Else
                        Select Case .Name
                            Case "PREC" : lMember = "PREC"
                            Case "ATEM" : lMember = "GATMP"
                            Case "DEWP" : lMember = "DTMPG"
                            Case "WIND" : lMember = "WINMOV"
                            Case "SOLR" : lMember = "SOLRAD"
                            Case "CLOU" : lMember = "CLOUD"
                            Case "PEVT" : lMember = "PETINP"
                        End Select
                        If .Name = "ATEM" Then
                            'get right air temp member name
                            If AirType = 1 Then
                                lMember = "GATMP"
                            ElseIf AirType = 2 Then
                                lMember = "AIRTMP"
                            End If
                        End If
                    End If
                    lStr.Append(lMember)
                    lStr.Append(Space(aCol(13) - lStr.Length - 1))
                    lSB.AppendLine(lStr.ToString)
                End If
            End With
        Next lMetSegRec
        Return lSB.ToString.Remove(lSB.Length - 2) 'remove CRLF
    End Function
End Class