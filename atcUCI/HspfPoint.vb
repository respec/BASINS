' Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

''' <summary>
''' HSPF Point Source
''' </summary>
Public Class HspfPointSource
    Public Id As Integer
    Public Name As String
    Public Con As String
    Public MFact As Double = 1.0#
    Public RFact As Double = 1.0#
    Public Tran As String
    Public Sgapstrg As String = ""
    Public Ssystem As String = ""
    Public Source As New HspfSrcTar
    Public Target As New HspfSrcTar
    Public AssocOperationId As Integer
    Public Comment As String = ""

    Public Function ToStringFromSpecs(ByVal aColumn() As Integer, _
                                      ByVal aLength() As Integer) As String
        Dim lStr As New System.Text.StringBuilder
        lStr.Append(Me.Source.VolName.Trim)
        lStr.Append(Space(aColumn(1) - lStr.Length - 1)) 'pad prev field
        lStr.Append(CStr(Me.Source.VolId).PadLeft(aLength(1)))
        lStr.Append(Space(aColumn(2) - lStr.Length - 1))
        lStr.Append(Source.Member)
        lStr.Append(Space(aColumn(3) - lStr.Length - 1))
        If Me.Source.MemSub1 <> 0 Then
            lStr.Append(CStr(Me.Source.MemSub1).PadLeft(aLength(3)))
        End If
        lStr.Append(Space(aColumn(4) - lStr.Length - 1))
        lStr.Append(Me.Ssystem)
        lStr.Append(Space(aColumn(5) - lStr.Length - 1))
        lStr.Append(Me.Sgapstrg)
        lStr.Append(Space(aColumn(6) - lStr.Length - 1))
        If Me.MFact <> 1 Then
            lStr.Append(CStr(Me.MFact).PadLeft(aLength(6)))
        End If
        lStr.Append(Space(aColumn(7) - lStr.Length - 1))
        lStr.Append(Me.Tran)
        lStr.Append(Space(aColumn(8) - lStr.Length - 1))
        lStr.Append(Me.Target.VolName)
        lStr.Append(Space(aColumn(9) - lStr.Length - 1))
        If Me.Target.VolId > 0 And Me.Target.VolIdL > 0 Then
            'have a range of operations, just write the one for the assoc oper
            lStr.Append(CStr(AssocOperationId).PadLeft(aLength(9)))
        Else
            lStr.Append(CStr(Me.Target.VolId).PadLeft(aLength(9)))
        End If
        lStr.Append(Space(aColumn(11) - lStr.Length - 1))
        lStr.Append(Me.Target.Group)
        lStr.Append(Space(aColumn(12) - lStr.Length - 1))
        lStr.Append(Me.Target.Member)
        lStr.Append(Space(aColumn(13) - lStr.Length - 1))
        If Me.Target.MemSub1 > 0 Then
            Dim lString As String = CStr(Me.Target.MemSub1).PadLeft(aLength(13))
            If Me.Target.VolName = "RCHRES" Then
                lString = Me.Target.Opn.Uci.IntAsCat(Me.Target.Member, 1, lString)
            End If
            lStr.Append(lString)
            lStr.Append(Space(aColumn(14) - lStr.Length - 1))
        End If
        If Me.Target.MemSub2 > 0 Then
            Dim lString As String = CStr(Me.Target.MemSub2).PadLeft(aLength(14))
            If Me.Target.VolName = "RCHRES" Then
                lString = Me.Target.Opn.Uci.IntAsCat(Me.Target.Member, 2, lString)
            End If
            lStr.Append(lString)
        End If
        Dim lSB As New System.Text.StringBuilder
        If Comment.Length > 0 Then
            lSB.AppendLine(Comment)
        End If
        lSB.Append(lStr.ToString)
        Return lSB.ToString
    End Function
End Class