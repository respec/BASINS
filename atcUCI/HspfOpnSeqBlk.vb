Option Strict Off
Option Explicit On

Imports atcUtility

<System.Runtime.InteropServices.ProgId("HspfOpnSeqBlk_NET.HspfOpnSeqBlk")> Public Class HspfOpnSeqBlk
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pDelt As Integer
    Private pOpns As Collection 'of HspfOperation
    Private pUci As HspfUci
    Private pComment As String

    Public Property Uci() As HspfUci
        Get
            Uci = pUci
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
            Caption = "Opn Sequence Block"
        End Get
    End Property


    Public Property Comment() As String
        Get
            Comment = pComment
        End Get
        Set(ByVal Value As String)
            pComment = Value
        End Set
    End Property

    Public ReadOnly Property EditControlName() As String
        Get
            EditControlName = "ATCoHspf.ctlOpnSeqBlkEdit"
        End Get
    End Property

    Public ReadOnly Property Opns() As Collection
        Get
            Opns = pOpns
        End Get
    End Property

    Public ReadOnly Property Opn(ByVal Index As Integer) As HspfOperation
        Get
            If Index > 0 And Index <= pOpns.Count() Then
                Opn = pOpns.Item(Index)
            Else
                'UPGRADE_NOTE: Object Opn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                Opn = Nothing
            End If
        End Get
    End Property

    Public Property Delt() As Integer
        Get
            Delt = pDelt
        End Get
        Set(ByVal Value As Integer)
            pDelt = Value
        End Set
    End Property

    Public Sub Add(ByRef newOpn As HspfOperation) 'to end
        pOpns.Add(newOpn)
    End Sub

    Public Sub Delete(ByRef Index As Integer)
        pOpns.Remove(Index)
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

    Public Sub WriteUciFile(ByRef f As Short)
        Dim vOpn As Object
        Dim lOpn As HspfOperation
        Dim h, M As Integer

        If Len(pComment) > 0 Then
            PrintLine(f, pComment)
        End If
        PrintLine(f, " ")
        PrintLine(f, "OPN SEQUENCE")
        h = pDelt / 60
        M = pDelt - h * 60
        PrintLine(f, "    INGRP" & Space(14) & "INDELT " & Format(h, "00") & ":" & Format(M, "00"))
        For Each vOpn In pOpns
            lOpn = vOpn
            If Len(lOpn.Comment) > 0 Then
                PrintLine(f, lOpn.Comment)
            End If
            PrintLine(f, Space(6) & lOpn.Name & Space(10 - Len(lOpn.Name)) & myFormatI((lOpn.Id), 4))
        Next vOpn
        PrintLine(f, "    END INGRP")
        PrintLine(f, "END OPN SEQUENCE")
    End Sub
End Class