'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Text
Imports atcUtility

Public Class HspfOpnSeqBlk
    Private pDelt As Integer
    Private pOpns As Collection (Of HspfOperation)
    Private pUci As HspfUci
    Private pComment As String = ""

    Public Property Uci() As HspfUci
        Get
            Return pUci
        End Get
        Set(ByVal Value As HspfUci)
            pUci = Value
            For Each lOperation As HspfOperation In pOpns
                lOperation.Uci = pUci
            Next
        End Set
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Return "Opn Sequence Block"
        End Get
    End Property


    Public Property Comment() As String
        Get
            Return pComment
        End Get
        Set(ByVal Value As String)
            pComment = Value
        End Set
    End Property

    Public ReadOnly Property EditControlName() As String
        Get
            Return "ATCoHspf.ctlOpnSeqBlkEdit"
        End Get
    End Property

    Public ReadOnly Property Opns() As Collection(Of HspfOperation)
        Get
            Return pOpns
        End Get
    End Property

    Public ReadOnly Property Opn(ByVal aIndex As Integer) As HspfOperation
        Get
            'TODO: 0 or 1 based

            ''1-based code
            'If aIndex > 0 And aIndex <= pOpns.Count Then
            '    Opn = pOpns.Item(aIndex)
            'Else
            '    Opn = Nothing
            'End If

            '0-based code
            If aIndex > -1 And aIndex <= pOpns.Count - 1 Then
                Opn = pOpns.Item(aIndex)
            Else
                Opn = Nothing
            End If

        End Get
    End Property

    Public Property Delt() As Integer
        Get
            Return pDelt
        End Get
        Set(ByVal Value As Integer)
            pDelt = Value
        End Set
    End Property

    Public Sub Add(ByRef aNewOpn As HspfOperation) 'to end
        pOpns.Add(aNewOpn)
    End Sub

    Public Sub Delete(ByRef aIndex As Integer)
        pOpns.RemoveAt(aIndex)
    End Sub

    Public Sub AddAfter(ByRef aNewOpn As HspfOperation, ByRef aAfterId As Integer)
        'TODO: not used in atcUci, not tested
        pOpns.Insert(aAfterId + 1, aNewOpn)
    End Sub

    Public Sub AddBefore(ByRef aNewOpn As HspfOperation, ByRef aBeforeId As Integer)
        'TODO: not used in atcUci, not tested
        pOpns.Insert(aBeforeId, aNewOpn)
    End Sub

    Public Sub New()
        MyBase.New()
        pOpns = New Collection(Of HspfOperation)
    End Sub

    Public Sub ReadUciFile()
        Dim lReturnCode, lInit, lOmCode, lRetkey As Integer
        Dim lBuff As String = Nothing
        Dim lOperation As HspfOperation
        Dim lRecTyp As Integer
        Dim lComment As String

        lInit = 1
        lOmCode = HspfOmCode("OPN SEQUENCE")
        lReturnCode = 0
        ' first call gets delt
        lRetkey = -1
        pComment = GetCommentBeforeBlock("OPN SEQUENCE")
        GetNextRecordFromBlock("OPN SEQUENCE", lRetkey, lBuff, lRecTyp, lReturnCode)

        If lReturnCode >= 0 Then
            pDelt = CDbl(Mid(lBuff, 31, 2)) * 60
            If lBuff.Length > 33 Then
                pDelt = pDelt + CDbl(Mid(lBuff, 34, 2))
            End If
            lInit = 0
            lComment = ""
            While lReturnCode = 2
                GetNextRecordFromBlock("OPN SEQUENCE", lRetkey, lBuff, lRecTyp, lReturnCode)

                If InStr(lBuff, "INGRP") = 0 And lReturnCode = 2 And lRecTyp = 0 Then
                    lOperation = New HspfOperation
                    lOperation.Name = Trim(StrRetRem(lBuff))
                    If IsNumeric(lBuff) Then
                        lOperation.Id = CInt(lBuff)
                    Else
                        lOperation.Id = CInt(StrRetRem(lBuff))
                    End If
                    lOperation.Uci = pUci
                    lOperation.Comment = lComment
                    If lOperation.Name <> "UNKNOWN" Then
                        pOpns.Add(lOperation)
                    End If
                    lComment = ""
                ElseIf lReturnCode = 2 And lRecTyp = -1 Then
                    'save comment
                    If lComment.Length = 0 Then
                        lComment = lBuff
                    Else
                        lComment &= vbCrLf & lBuff
                    End If
                ElseIf lReturnCode = 2 And lRecTyp = -2 Then
                    'save blank line
                    If lComment.Length = 0 Then
                        lComment = " "
                    Else
                        lComment &= vbCrLf & " "
                    End If
                End If
            End While
        End If
    End Sub

    Public Overrides Function ToString() As String
        Dim lSb As New StringBuilder
        Dim lOpn As HspfOperation

        If pComment.Length > 0 Then
            lSb.AppendLine(pComment)
        End If
        lSb.AppendLine("OPN SEQUENCE")
        Dim h As Integer = pDelt / 60
        Dim M As Integer = pDelt - h * 60
        lSb.AppendLine("    INGRP" & Space(14) & "INDELT " & Format(h, "00") & ":" & Format(M, "00"))
        For Each vOpn As Object In pOpns
            lOpn = vOpn
            If lOpn.Comment.Length > 0 Then
                lSb.AppendLine(lOpn.Comment)
            End If
            lSb.AppendLine(Space(6) & lOpn.Name & Space(10 - Len(lOpn.Name)) & myFormatI((lOpn.Id), 4))
        Next vOpn
        lSb.AppendLine("    END INGRP")
        lSb.AppendLine("END OPN SEQUENCE")
        Return lSb.ToString
    End Function
End Class