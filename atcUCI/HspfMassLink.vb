'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text

Public Class HspfMassLink
    Implements ICloneable

    Public Source As HspfSrcTar
    Public Target As HspfSrcTar
    Public Tran As String
    Public MFact As Double
    Public MFactAsRead As String
    Public Uci As HspfUci
    Public MassLinkId As Integer
    Public Comment As String = ""
    Public Const EditControlName As String = "ATCoHspf.ctlMassLinkEdit"
    Public Const Caption As String = "Mass-Link Block"

    Public Sub New()
        MyBase.New()
        Source = New HspfSrcTar
        Target = New HspfSrcTar
        MFact = 1
    End Sub

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim lNewMassLink As New HspfMassLink
        With lNewMassLink
            .Source = Me.Source.Clone
            .Target = Me.Target.Clone
            .Tran = Me.Tran
            .MFact = Me.MFact
            .MFactAsRead = Me.MFactAsRead
            .Uci = Me.Uci
            .MassLinkId = Me.MassLinkId
            .Comment = Me.Comment
        End With
        Return lNewMassLink
    End Function

    Public Sub ReadMassLinks(ByRef aUci As HspfUci)
        Dim lMassLinkIds(-1) As Integer
        Dim lMassLinkKeys(-1) As Integer
        Dim lOmCode As Integer = HspfOmCode("MASS-LINK")
        Dim lMassLinkCount As Integer

        lMassLinkCount = 1
        ReDim Preserve lMassLinkKeys(lMassLinkCount)

        Dim lRecordType As Integer
        For lMassLinkIndex As Integer = 0 To lMassLinkCount - 1
            'loop through each mass link
            Dim lInit As Integer = lMassLinkKeys(lMassLinkIndex)
            Dim lStringComment As String = ""
            Dim lPastHeader As Boolean = False
            Dim lString As String = Nothing
            Dim lReturnKey As Integer = -1
            Dim lReturnCode As Integer
            Dim lMassLinkId As Integer
            Do
                GetNextRecordFromBlock("MASS-LINK", lReturnKey, lString, lRecordType, lReturnCode)
                If lRecordType = -1 Then 'this is a comment
                Else
                    If lString Is Nothing Then
                        Exit Do
                    ElseIf lString.StartsWith("  MASS-LINK") Then
                        'start of a new mass link
                        lMassLinkId = CShort(Mid(lString, 16, 5))
                        lPastHeader = False
                        GetNextRecordFromBlock("MASS-LINK", lReturnKey, lString, lRecordType, lReturnCode)
                    ElseIf lString.StartsWith("  END MASS-LINK") Then
                        'end of a mass link
                        lRecordType = -2
                    End If
                End If
                lInit = 0
                If lRecordType = 0 Then
                    Dim lMassLink As New HspfMassLink
                    lMassLink.Uci = aUci
                    lMassLink.MassLinkId = lMassLinkId
                    lMassLink.Source.VolName = Trim(Left(lString, 6))
                    lMassLink.Source.VolId = 0
                    lMassLink.Source.Group = Trim(Mid(lString, 12, 6))
                    lMassLink.Source.Member = Trim(Mid(lString, 19, 6))
                    Dim lStr As String
                    lStr = Trim(Mid(lString, 25, 2))
                    If lStr.Length > 0 Then
                        If IsNumeric(lStr) Then
                            lMassLink.Source.MemSub1 = CInt(lStr)
                        Else
                            lMassLink.Source.MemSub1 = aUci.CatAsInt(lStr)
                        End If
                    End If
                    lStr = Trim(Mid(lString, 27, 2))
                    If lStr.Length > 0 Then
                        If IsNumeric(lStr) Then
                            lMassLink.Source.MemSub2 = CInt(lStr)
                        Else
                            lMassLink.Source.MemSub2 = aUci.CatAsInt(lStr)
                        End If
                    End If
                    lStr = Mid(lString, 29, 10).Trim
                    lMassLink.MFactAsRead = Mid(lString, 29, 10)
                    If lStr.Length > 0 Then
                        lMassLink.MFact = CDbl(lStr)
                    End If
                    lMassLink.Tran = Trim(Mid(lString, 39, 4))
                    lMassLink.Target.VolName = Trim(Mid(lString, 44, 6))
                    lMassLink.Target.VolId = 0
                    lStr = Trim(Mid(lString, 55, 3))
                    If Len(lStr) > 0 Then lMassLink.Target.VolIdL = CInt(lStr)
                    lMassLink.Target.Group = Trim(Mid(lString, 59, 6))
                    lMassLink.Target.Member = Trim(Mid(lString, 66, 6))
                    lStr = Trim(Mid(lString, 72, 2))
                    If Len(lStr) > 0 And IsNumeric(lStr) Then
                        lMassLink.Target.MemSub1 = CInt(lStr)
                    Else
                        If Len(lStr) > 0 Then lMassLink.Target.MemSub1 = aUci.CatAsInt(lStr)
                    End If
                    lStr = Trim(Mid(lString, 74, 2))
                    If Len(lStr) > 0 And IsNumeric(lStr) Then
                        lMassLink.Target.MemSub2 = CInt(lStr)
                    Else
                        If Len(lStr) > 0 Then lMassLink.Target.MemSub2 = aUci.CatAsInt(lStr)
                    End If
                    lMassLink.Comment = lStringComment
                    aUci.MassLinks.Add(lMassLink)
                    lStringComment = ""
                ElseIf lRecordType = -1 And lReturnCode <> 1 Then
                    'save comment
                    If lString.StartsWith("<Name>") Then 'a cheap rule to identify the last header line
                        lPastHeader = True
                    ElseIf lPastHeader Then
                        If lStringComment.Length = 0 Then
                            lStringComment = lString
                        Else
                            lStringComment &= vbCrLf & lString
                        End If
                    End If
                End If
            Loop While lReturnCode = 2
        Next lMassLinkIndex
    End Sub

    Public Overrides Function ToString() As String
        Dim lMassLinkIds(-1) As Integer
        Dim lMassLinkCount As Integer = 0

        For Each lMassLink As HspfMassLink In Uci.MassLinks
            Dim lFound As Boolean = False
            For lMassLinkIndex As Integer = 0 To lMassLinkCount - 1
                If lMassLink.MassLinkId = lMassLinkIds(lMassLinkIndex) Then
                    lFound = True
                End If
            Next lMassLinkIndex
            If lFound = False Then
                lMassLinkCount += 1
                ReDim Preserve lMassLinkIds(lMassLinkCount)
                lMassLinkIds(lMassLinkCount - 1) = lMassLink.MassLinkId
            End If
        Next lMassLink

        Dim s As String = "MASS-LINK"
        Dim lBlockDef As HspfBlockDef = Uci.Msg.BlockDefs.Item(s)
        Dim lTableDef As HspfTableDef = lBlockDef.TableDefs.Item(0)
        'get lengths and starting positions
        Dim j As Integer = 0
        Dim lStartCol(15) As Integer
        Dim lLength(15) As Integer
        For Each lParmDef As HSPFParmDef In lTableDef.ParmDefs
            lStartCol(j) = lParmDef.StartCol
            lLength(j) = lParmDef.Length
            j += 1
        Next lParmDef
        Dim lSB As New StringBuilder
        lSB.AppendLine(s)

        For i As Integer = 1 To lMassLinkCount
            lSB.AppendLine(" ")
            lSB.AppendLine("  MASS-LINK    " & CStr(lMassLinkIds(i - 1)).PadLeft(5))
            'now start building the records
            lSB.AppendLine("<-Volume-> <-Grp> <-Member-><--Mult-->     <-Target vols> <-Grp> <-Member->  ***")
            lSB.AppendLine("<Name>            <Name> x x<-factor->     <Name>                <Name> x x  ***")
            For Each lML As HspfMassLink In Uci.MassLinks
                If lML.MassLinkId = lMassLinkIds(i - 1) Then
                    If lML.Comment.Length > 0 Then
                        lSB.AppendLine(lML.Comment)
                    End If
                    Dim lStr As New StringBuilder
                    lStr.Append(lML.Source.VolName.Trim)
                    lStr.Append(Space(lStartCol(1) - lStr.Length - 1)) 'pad prev field
                    lStr.Append(lML.Source.Group)
                    lStr.Append(Space(lStartCol(2) - lStr.Length - 1))
                    lStr.Append(lML.Source.Member)
                    lStr.Append(Space(lStartCol(3) - lStr.Length - 1))
                    Dim t As String
                    If lML.Source.MemSub1 <> 0 Then
                        t = CStr(lML.Source.MemSub1).PadLeft(lLength(3))
                        If lML.Source.VolName = "RCHRES" Then
                            t = Uci.IntAsCat(lML.Source.Member, 1, t)
                        End If
                        lStr.Append(t)
                    End If
                    lStr.Append(Space(lStartCol(4) - lStr.Length - 1))
                    If lML.Source.MemSub2 <> 0 Then
                        t = CStr(lML.Source.MemSub2).PadLeft(lLength(4))
                        If lML.Source.VolName = "RCHRES" Then
                            t = Uci.IntAsCat(lML.Source.Member, 2, t)
                        End If
                        lStr.Append(t)
                    End If
                    lStr.Append(Space(lStartCol(5) - lStr.Length - 1))
                    If NumericallyTheSame((lML.MFactAsRead), (lML.MFact)) Then
                        lStr.Append(lML.MFactAsRead)
                    ElseIf lML.MFact <> 1 Then
                        lStr.Append(CStr(lML.MFact).PadLeft(lLength(5)))
                    End If
                    lStr.Append(Space(lStartCol(6) - lStr.Length - 1))
                    'str = str & lML.Tran
                    'str = str & Space(icol(7) - Len(str) - 1)
                    lStr.Append(lML.Target.VolName)
                    lStr.Append(Space(lStartCol(7) - lStr.Length - 1))
                    lStr.Append(lML.Target.Group)
                    lStr.Append(Space(lStartCol(8) - lStr.Length - 1))
                    lStr.Append(lML.Target.Member)
                    lStr.Append(Space(lStartCol(9) - lStr.Length - 1))
                    If lML.Target.MemSub1 <> 0 Then
                        t = CStr(lML.Target.MemSub1).PadLeft(lLength(9))
                        If lML.Target.VolName = "RCHRES" Then
                            t = Uci.IntAsCat(lML.Target.Member, 1, t)
                        End If
                        lStr.Append(t)
                    End If
                    lStr.Append(Space(lStartCol(10) - lStr.Length - 1))
                    If lML.Target.MemSub2 <> 0 Then
                        t = CStr(lML.Target.MemSub2).PadLeft(lLength(10))
                        If lML.Target.VolName = "RCHRES" Then
                            t = Uci.IntAsCat(lML.Target.Member, 2, t)
                        End If
                        lStr.Append(t)
                    End If
                    lSB.AppendLine(lStr.ToString)
                End If
            Next lML
            lSB.AppendLine("  END MASS-LINK" & CStr(lMassLinkIds(i - 1)).PadLeft(5))
        Next i
        lSB.AppendLine("END " & s)
        Return lSB.ToString
    End Function

    Public Function FindMassLinkID(ByVal aSourceVolName As String, _
                                   ByVal aTargetVolName As String) As Integer
        For Each lMassLink As HspfMassLink In Uci.MassLinks
            If lMassLink.Source.VolName = aSourceVolName And _
               lMassLink.Target.VolName = aTargetVolName Then
                Return lMassLink.MassLinkId
            End If
        Next lMassLink
        Return -1
    End Function
End Class