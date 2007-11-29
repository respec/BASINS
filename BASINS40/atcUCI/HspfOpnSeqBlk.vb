'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Text
Imports atcUtility

Public Class HspfOpnSeqBlk
    Private pDelt As Integer
    Private pOpns As Collection '(Of HspfOperation)
    Private pUci As HspfUci
    Private pComment As String

    Public Property Uci() As HspfUci
        Get
            Return pUci
        End Get
        Set(ByVal Value As HspfUci)
            pUci = Value
            For Each lOpn As HspfOperation In pOpns
                lOpn.Uci = pUci
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

    Public ReadOnly Property Opns() As Collection
        Get
            Return pOpns
        End Get
    End Property

    Public ReadOnly Property Opn(ByVal Index As Integer) As HspfOperation
        Get
            If Index > 0 And Index <= pOpns.Count() Then
                Opn = pOpns.Item(Index)
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

    Public Sub Add(ByRef newOpn As HspfOperation) 'to end
        pOpns.Add(newOpn)
    End Sub

    Public Sub Delete(ByRef aIndex As Integer)
        pOpns.Remove(aIndex)
    End Sub

    Public Sub AddAfter(ByRef newOpn As HspfOperation, ByRef afterid As Integer)
        pOpns.Add(newOpn, , , afterid)
    End Sub

    Public Sub AddBefore(ByRef newOpn As HspfOperation, ByRef beforeid As Integer)
        pOpns.Add(newOpn, , beforeid)
    End Sub

    Public Sub Edit()
        editInit(Me, Me.Uci.icon, True, True)
    End Sub

    Public Sub New()
        MyBase.New()
        pOpns = New Collection
    End Sub

    Public Sub ReadUciFile()
        Dim retcod, init, OmCode, retkey As Integer
        Dim cbuff As String = Nothing
        Dim lOpn As HspfOperation
        Dim rectyp As Integer
        Dim c As String

        init = 1
        OmCode = HspfOmCode("OPN SEQUENCE")
        retcod = 0
        ' first call gets delt
        If pUci.FastFlag Then
            retkey = -1
            GetCommentBeforeBlock("OPN SEQUENCE", pComment)
            GetNextRecordFromBlock("OPN SEQUENCE", retkey, cbuff, rectyp, retcod)
        Else
            Call REM_XBLOCK((Me.Uci), OmCode, init, retkey, cbuff, retcod)
        End If
        If retcod >= 0 Then
            pDelt = CDbl(Mid(cbuff, 31, 2)) * 60
            If Len(cbuff) > 33 Then
                pDelt = pDelt + CDbl(Mid(cbuff, 34, 2))
            End If
            init = 0
            c = ""
            While retcod = 2
                If pUci.FastFlag Then
                    GetNextRecordFromBlock("OPN SEQUENCE", retkey, cbuff, rectyp, retcod)
                Else
                    retkey = -1
                    Call REM_XBLOCKEX((Me.Uci), OmCode, init, retkey, cbuff, rectyp, retcod)
                End If
                If InStr(cbuff, "INGRP") = 0 And retcod = 2 And rectyp = 0 Then
                    lOpn = New HspfOperation
                    lOpn.Name = Trim(StrRetRem(cbuff))
                    If IsNumeric(cbuff) Then
                        lOpn.Id = CInt(cbuff)
                    Else
                        lOpn.Id = CInt(StrRetRem(cbuff))
                    End If
                    lOpn.Uci = pUci
                    lOpn.Comment = c
                    If lOpn.Name <> "UNKNOWN" Then
                        pOpns.Add(lOpn)
                    End If
                    c = ""
                ElseIf retcod = 2 And rectyp = -1 Then
                    'save comment
                    If Len(c) = 0 Then
                        c = cbuff
                    Else
                        c = c & vbCrLf & cbuff
                    End If
                ElseIf retcod = 2 And rectyp = -2 Then
                    'save blank line
                    If Len(c) = 0 Then
                        c = " "
                    Else
                        c = c & vbCrLf & " "
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