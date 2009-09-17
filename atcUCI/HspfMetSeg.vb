'Copyright 2006-7 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Public Class HspfMetSeg
    Private pMetSegRecs As New HspfMetSegRecords
    Public Id As Integer
    Public Name As String
    Public Uci As HspfUci
    Public Comment As String = ""
    Public AirType As Integer '1-GATMP, 2-AIRTMP

    Public ReadOnly Property MetSegRecs() As HspfMetSegRecords
        Get
            Return pMetSegRecs
        End Get
    End Property

    Public Function Add(ByRef aConnection As HspfConnection) As Boolean
        Dim lResult As Boolean = True
        If Str2Name(aConnection.Target.Member).Length > 0 Then
            If aConnection.Comment.Length > 0 Then
                If pMetSegRecs.Count = 0 Then
                    Comment = aConnection.Comment
                    aConnection.Comment = ""
                End If
            End If
            Dim lMetSegRec As New HspfMetSegRecord

            lMetSegRec.Source = aConnection.Source
            lMetSegRec.Tran = aConnection.Tran
            lMetSegRec.Sgapstrg = aConnection.Sgapstrg
            lMetSegRec.Ssystem = aConnection.Ssystem
            lMetSegRec.Name = Str2Name(aConnection.Target.Member)

            If aConnection.Target.VolName = "RCHRES" Then
                lMetSegRec.MFactR = aConnection.MFact
            ElseIf aConnection.Target.VolName = "PERLND" Or _
                   aConnection.Target.VolName = "IMPLND" Then
                lMetSegRec.MFactP = aConnection.MFact
            End If

            If aConnection.Target.VolName = "PERLND" Or _
               aConnection.Target.VolName = "IMPLND" Or _
               aConnection.Target.VolName = "RCHRES" Then
                If pMetSegRecs.Contains(lMetSegRec.Name) AndAlso _
                   pMetSegRecs(lMetSegRec.Name).Source.VolName.Length > 0 Then
                    'dont add if already have this type of record
                    lResult = False
                Else
                    pMetSegRecs.Add(lMetSegRec)
                    If aConnection.Target.Member = "GATMP" Then
                        AirType = 1
                    ElseIf aConnection.Target.Member = "AIRTMP" Then
                        AirType = 2
                    End If
                End If
            End If
        Else 'not a met timeseries
            lResult = False
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
            Case "PETINP" : lName = "PEVT"
            Case "POTEV" : lName = "PEVT"
        End Select
        Return lName
    End Function

    Public Function Compare(ByRef aMetSeg As HspfMetSeg, ByRef aOperName As String) As Boolean
        For Each lMetSegRec As HspfMetSegRecord In pMetSegRecs
            If aMetSeg.MetSegRecs.Contains(lMetSegRec.Name) Then
                Dim lMetSegRecInArg As HspfMetSegRecord = aMetSeg.MetSegRecs(lMetSegRec.Name)
                If Not (lMetSegRec.Compare(lMetSegRecInArg, aOperName)) Then
                    Return False
                End If
            Else
                Return False
            End If
        Next lMetSegRec
        Return True
    End Function

    Public Sub UpdateMetSeg(ByRef aMetSeg As HspfMetSeg)
        For Each lExistingMetSegRec As HspfMetSegRecord In pMetSegRecs
            With aMetSeg.MetSegRecs(lExistingMetSegRec.Name)
                If lExistingMetSegRec.MFactR = -999.0# And .MFactR <> -999.0# Then
                    lExistingMetSegRec.MFactR = .MFactR
                    lExistingMetSegRec.Sgapstrg = .Sgapstrg
                    lExistingMetSegRec.Source = .Source
                    lExistingMetSegRec.Ssystem = .Ssystem
                    lExistingMetSegRec.Tran = .Tran
                End If
            End With
        Next lExistingMetSegRec

        For Each lNewMetSegRec As HspfMetSegRecord In aMetSeg.MetSegRecs
            Dim lRecFound As Boolean = False
            For Each lExistingMetSegRec As HspfMetSegRecord In pMetSegRecs
                If lNewMetSegRec.Name = lExistingMetSegRec.Name Then
                    lRecFound = True
                End If
            Next lExistingMetSegRec
            If Not lRecFound Then
                'add this type of rec to the existing met seg
                pMetSegRecs.Add(lNewMetSegRec)
            End If
        Next lNewMetSegRec
    End Sub

    Public Sub New()
        MyBase.New()
        AirType = 0
    End Sub

    Public Sub ExpandMetSegName(ByRef aWdmId As String, ByRef aDsn As Integer)
        Dim lAddStr As String = ""
        Dim lCon As String = ""
        Me.Name = Me.Uci.GetWDMAttr(aWdmId, aDsn, "LOC")

        For Each lMetSegRec As HspfMetSegRecord In pMetSegRecs
            Select Case lMetSegRec.Name
                Case "PREC" : lCon = "PREC"
                Case "ATEM"
                    lCon = "GATMP"
                    If Me.AirType = 2 Then
                        lCon = "AIRTMP"
                    End If
                Case "DEWP" : lCon = "DTMPG"
                Case "WIND" : lCon = "WINMOV"
                Case "SOLR" : lCon = "SOLRAD"
                Case "CLOU" : lCon = "CLOUD"
                Case "PEVT" : lCon = "PETINP"
            End Select
            If lMetSegRec.MFactP <> 1 And _
               lMetSegRec.MFactP <> 0 And _
               lMetSegRec.MFactP <> -999 Then
                lAddStr &= ",PI:" & lCon & "=" & CStr(lMetSegRec.MFactP)
            End If
            If lMetSegRec.MFactR <> 1 And _
               lMetSegRec.MFactR <> 0 And _
               lMetSegRec.MFactR <> -999 Then
                lAddStr &= ",R:" & lCon & "=" & CStr(lMetSegRec.MFactR)
            End If
        Next lMetSegRec

        If lAddStr.Length > 0 Then
            Me.Name &= lAddStr
        End If
    End Sub

    Friend Function ToStringFromSpecs(ByRef aOperationType As String, _
                                      ByRef aCol() As Integer, _
                                      ByRef aLen() As Integer, _
                                      ByRef aHeaderPending As Boolean) As String
        Dim lSB As New System.Text.StringBuilder

        Dim lFirstId As Integer = 0
        Dim lLastId As Integer = 0
        For lOpnId As Integer = 1 To Uci.OpnBlks.Item(aOperationType).Ids.Count
            Dim lOpn As HspfOperation = Uci.OpnBlks.Item(aOperationType).NthOper(lOpnId)
            If Not lOpn.MetSeg Is Nothing AndAlso lOpn.MetSeg.Id = Me.Id Then
                If lFirstId = 0 Then
                    lFirstId = lOpn.Id
                Else
                    lLastId = lOpn.Id
                End If
            ElseIf lFirstId > 0 Then
                If lSB.Length > 0 AndAlso Not lSB.ToString.EndsWith(vbCrLf) Then
                    lSB.AppendLine("")
                End If
                lSB.Append(FormatRecords(aOperationType, lFirstId, lLastId, aCol, aLen, aHeaderPending))
                lFirstId = 0
                lLastId = 0
            End If
        Next lOpnId
        If lFirstId > 0 Then
            If lSB.Length > 0 AndAlso Not lSB.ToString.EndsWith(vbCrLf) Then
                lSB.AppendLine("")
            End If
            lSB.Append(FormatRecords(aOperationType, lFirstId, lLastId, aCol, aLen, aHeaderPending))
        End If
        Return lSB.ToString
    End Function

    Private Function FormatRecords(ByRef aOpTyp As String, _
                                  ByRef aFirstId As Integer, ByRef aLastId As Integer, _
                                  ByRef aCol() As Integer, ByRef aLen() As Integer, _
                                  ByRef aHeaderPending As Boolean) As String
        Dim lSB As New System.Text.StringBuilder

        Dim lInit As Boolean = True
        For Each lMetSegRec As HspfMetSegRecord In pMetSegRecs
            With lMetSegRec
                If (aOpTyp = "RCHRES" And .MFactR > 0.0#) Or _
                   (aOpTyp = "PERLND" And .MFactP > 0.0#) Or _
                   (aOpTyp = "IMPLND" And .MFactP > 0.0#) Then
                    'have this type of met seg record
                    Dim lStr As New System.Text.StringBuilder
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
                            Case Else
                                Debug.Print("Why!")
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
                            Case Else
                                Debug.Print("Why!")
                        End Select
                        If .Name = "ATEM" Then 'get right air temp member name
                            If AirType = 1 Then
                                lMember = "GATMP"
                            ElseIf AirType = 2 Then
                                lMember = "AIRTMP"
                            End If
                        End If
                    End If
                    lStr.Append(lMember)
                    lStr.Append(Space(aCol(13) - lStr.Length - 1))
                    If lInit Then
                        If aHeaderPending AndAlso Not Comment.Contains("<-Volume->") Then
                            lSB.AppendLine(HspfConnection.ExtSourceHeader)
                        End If
                        If Comment.Length = 0 Then
                            lSB.AppendLine("*** Met Seg " & Trim(Name))
                        Else
                            lSB.AppendLine(Comment)
                        End If
                        aHeaderPending = False
                        lInit = False
                    End If
                    lSB.AppendLine(lStr.ToString)
                End If
            End With
        Next lMetSegRec
        Dim lString As String = lSB.ToString
        If lString.EndsWith(vbCrLf) Then
            lString = lString.Remove(lString.Length - 2)
        End If
        Return lString
    End Function
End Class