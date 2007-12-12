' Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text

''' <summary>
''' HSPF Point Source
''' </summary>
Public Class HspfPointSource
    Public Id As Integer
    Public Name As String
    Public Con As String
    Public MFact As Double
    Public RFact As Double
    Public Tran As String
    Public Sgapstrg As String
    Public Ssystem As String
    Public Source As HspfSrcTar
    Public Target As HspfSrcTar
    Public AssocOperationId As Integer

    Public Sub New()
        MyBase.New()
        With Me
            .Source = New HspfSrcTar
            .Target = New HspfSrcTar
            .MFact = 1.0#
            .RFact = 1.0#
            .Sgapstrg = ""
            .Ssystem = ""
        End With
    End Sub

    Public Function ToStringFromSpecs(ByVal aCol() As Integer, _
                                      ByVal aLen() As Integer) As String
        Dim lStr As New StringBuilder
        lStr.Append(Me.Source.VolName.Trim)
        lStr.Append(Space(aCol(1) - lStr.Length - 1)) 'pad prev field
        lStr.Append(CStr(Me.Source.VolId).PadLeft(aLen(1)))
        lStr.Append(Space(aCol(2) - lStr.Length - 1))
        lStr.Append(Source.Member)
        lStr.Append(Space(aCol(3) - lStr.Length - 1))
        If Me.Source.MemSub1 <> 0 Then
            lStr.Append(CStr(Me.Source.MemSub1).PadLeft(aLen(3)))
        End If
        lStr.Append(Space(aCol(4) - lStr.Length - 1))
        lStr.Append(Me.Ssystem)
        lStr.Append(Space(aCol(5) - lStr.Length - 1))
        lStr.Append(Me.Sgapstrg)
        lStr.Append(Space(aCol(6) - lStr.Length - 1))
        If Me.MFact <> 1 Then
            lStr.Append(CStr(Me.MFact).PadLeft(aLen(6)))
        End If
        lStr.Append(Space(aCol(7) - lStr.Length - 1))
        lStr.Append(Me.Tran)
        lStr.Append(Space(aCol(8) - lStr.Length - 1))
        lStr.Append(Me.Target.VolName)
        lStr.Append(Space(aCol(9) - lStr.Length - 1))
        If Me.Target.VolId > 0 And Me.Target.VolIdL > 0 Then
            'have a range of operations, just write the one for the assoc oper
            lStr.Append(CStr(AssocOperationId).PadLeft(aLen(9)))
        Else
            lStr.Append(CStr(Me.Target.VolId).PadLeft(aLen(9)))
        End If
        lStr.Append(Space(aCol(11) - lStr.Length - 1))
        lStr.Append(Me.Target.Group)
        lStr.Append(Space(aCol(12) - lStr.Length - 1))
        lStr.Append(Me.Target.Member)
        lStr.Append(Space(aCol(13) - lStr.Length - 1))
        If Me.Target.MemSub1 > 0 Then
            Dim lString As String = CStr(Me.Target.MemSub1).PadLeft(aLen(13))
            If Me.Target.VolName = "RCHRES" Then
                lString = Me.Target.Opn.Uci.IntAsCat(Me.Target.Member, 1, lString)
            End If
            lStr.Append(lString)
            lStr.Append(Space(aCol(14) - lStr.Length - 1))
        End If
        If Me.Target.MemSub2 > 0 Then
            Dim lString As String = CStr(Me.Target.MemSub2).PadLeft(aLen(14))
            If Me.Target.VolName = "RCHRES" Then
                lString = Me.Target.Opn.Uci.IntAsCat(Me.Target.Member, 2, lString)
            End If
            lStr.Append(lString)
        End If
        Return lStr.ToString
    End Function
End Class