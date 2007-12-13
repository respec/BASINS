'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text

Public Class HspfMassLink
    Public Source As HspfSrcTar
    Public Target As HspfSrcTar
    Public Tran As String
    Public MFact As Double
    Public Uci As HspfUci
    Public MassLinkId As Integer
    Public Comment As String = ""
    Public ReadOnly Property EditControlName() As String
        Get
            EditControlName = "ATCoHspf.ctlMassLinkEdit"
        End Get
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Caption = "Mass-Link Block"
        End Get
    End Property

    Public Sub readMassLinks(ByRef aUci As HspfUci)
        Dim lMassLinkIds(-1) As Integer
        Dim lMassLinkKeys(-1) As Integer
        Dim lOmCode As Integer = HspfOmCode("MASS-LINKS")
        Dim lMassLinkCount As Integer

        If aUci.FastFlag Then
            lMassLinkCount = 1
            ReDim Preserve lMassLinkKeys(lMassLinkCount)
        Else
            Dim lId As Integer = -101
            Dim lInit As Integer = 1
            Dim lContinueFlag As Integer
            Dim lKeyFlag As Integer
            Dim lReturnId As Integer
            lMassLinkCount = 0
            Do
                Dim lKeyWord As String = ""
                Call REM_GTNXKW(aUci, lInit, lId, lKeyWord, lKeyFlag, lContinueFlag, lReturnId)
                If lReturnId <> 0 Then
                    lMassLinkCount += 1
                    ReDim Preserve lMassLinkIds(lMassLinkCount)
                    ReDim Preserve lMassLinkKeys(lMassLinkCount)
                    lMassLinkIds(lMassLinkCount - 1) = CInt(lKeyWord)
                    lMassLinkKeys(lMassLinkCount - 1) = lReturnId
                End If
                lInit = 0
            Loop While lContinueFlag = 1
        End If

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
                If aUci.FastFlag Then
                    GetNextRecordFromBlock("MASS-LINK", lReturnKey, lString, lRecordType, lReturnCode)
                    If lString.StartsWith("  MASS-LINK") Then
                        'start of a new mass link
                        lMassLinkId = CShort(Mid(lString, 16, 5))
                        lPastHeader = False
                        GetNextRecordFromBlock("MASS-LINK", lReturnKey, lString, lRecordType, lReturnCode)
                    ElseIf lString.StartsWith("  END MASS-LINK") Then
                        'end of a mass link
                        lRecordType = -2
                    End If
                Else
                    lReturnKey = -1
                    Call REM_XBLOCKEX(aUci, lOmCode, lInit, lReturnKey, lString, lRecordType, lReturnCode)
                    lMassLinkId = lMassLinkIds(lMassLinkIndex)
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
                    lStr = Trim(Mid(lString, 29, 10))
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

    Public Sub New()
        MyBase.New()
        Source = New HspfSrcTar
        Target = New HspfSrcTar
        MFact = 1
    End Sub

    Public Sub Edit()
        editInit(Me, Me.Uci.icon, True)
    End Sub

    Public Overrides Function ToString() As String
        Dim s As String
        Dim lSB As New StringBuilder
        Dim lBlockDef As HspfBlockDef
        Dim lTableDef As HspfTableDef
        Dim j, i, k As Integer
        Dim typeexists(4) As Boolean
        Dim icol(15) As Integer
        Dim ilen(15) As Integer
        Dim lParmDef As HSPFParmDef
        Dim t As String
        Dim mlno(-1) As Integer
        Dim mlcnt As Integer = 0
        Dim found As Boolean

        For Each lML As HspfMassLink In Uci.MassLinks
            found = False
            For k = 0 To mlcnt - 1
                If lML.MassLinkID = mlno(k) Then
                    found = True
                End If
            Next k
            If found = False Then
                mlcnt = mlcnt + 1
                ReDim Preserve mlno(mlcnt)
                mlno(mlcnt - 1) = lML.MassLinkID
            End If
        Next lML

        s = "MASS-LINK"
        lBlockDef = Uci.Msg.BlockDefs.Item(s)
        lTableDef = lBlockDef.TableDefs.Item(0)
        'get lengths and starting positions
        j = 0
        For Each lParmDef In lTableDef.ParmDefs
            icol(j) = lParmDef.StartCol
            ilen(j) = lParmDef.Length
            j = j + 1
        Next lParmDef
        lSB.AppendLine(s)

        For i = 1 To mlcnt
            lSB.AppendLine(" ")
            lSB.AppendLine("  MASS-LINK    " & CStr(mlno(i - 1)).PadLeft(5))
            'now start building the records
            lSB.AppendLine("<-Volume-> <-Grp> <-Member-><--Mult-->     <-Target vols> <-Grp> <-Member->  ***")
            lSB.AppendLine("<Name>            <Name> x x<-factor->     <Name>                <Name> x x  ***")
            For Each lML As HspfMassLink In Uci.MassLinks
                If lML.MassLinkID = mlno(i - 1) Then
                    If lML.Comment.Length > 0 Then
                        lSB.AppendLine(lML.Comment)
                    End If
                    Dim lStr As New StringBuilder
                    lStr.Append(lML.Source.VolName.Trim)
                    lStr.Append(Space(icol(1) - lStr.Length - 1)) 'pad prev field
                    lStr.Append(lML.Source.Group)
                    lStr.Append(Space(icol(2) - lStr.Length - 1))
                    lStr.Append(lML.Source.Member)
                    lStr.Append(Space(icol(3) - lStr.Length - 1))
                    If lML.Source.MemSub1 <> 0 Then
                        t = CStr(lML.Source.MemSub1).PadLeft(ilen(3))
                        If lML.Source.VolName = "RCHRES" Then
                            t = Uci.IntAsCat(lML.Source.Member, 1, t)
                        End If
                        lStr.Append(t)
                    End If
                    lStr.Append(Space(icol(4) - lStr.Length - 1))
                    If lML.Source.MemSub2 <> 0 Then
                        t = CStr(lML.Source.MemSub2).PadLeft(ilen(4))
                        If lML.Source.VolName = "RCHRES" Then
                            t = Uci.IntAsCat(lML.Source.Member, 2, t)
                        End If
                        lStr.Append(t)
                    End If
                    lStr.Append(Space(icol(5) - lStr.Length - 1))
                    If lML.MFact <> 1 Then
                        lStr.Append(CStr(lML.MFact).PadLeft(ilen(5)))
                    End If
                    lStr.Append(Space(icol(6) - lStr.Length - 1))
                    'str = str & lML.Tran
                    'str = str & Space(icol(7) - Len(str) - 1)
                    lStr.Append(lML.Target.VolName)
                    lStr.Append(Space(icol(7) - lStr.Length - 1))
                    lStr.Append(lML.Target.Group)
                    lStr.Append(Space(icol(8) - lStr.Length - 1))
                    lStr.Append(lML.Target.Member)
                    lStr.Append(Space(icol(9) - lStr.Length - 1))
                    If lML.Target.MemSub1 <> 0 Then
                        t = CStr(lML.Target.MemSub1).PadLeft(ilen(9))
                        If lML.Target.VolName = "RCHRES" Then
                            t = Uci.IntAsCat(lML.Target.Member, 1, t)
                        End If
                        lStr.Append(t)
                    End If
                    lStr.Append(Space(icol(10) - lStr.Length - 1))
                    If lML.Target.MemSub2 <> 0 Then
                        t = Space(ilen(10))
                        t = RSet(CStr(lML.Target.MemSub2), Len(t))
                        If lML.Target.VolName = "RCHRES" Then
                            t = Uci.IntAsCat(lML.Target.Member, 2, t)
                        End If
                        lStr.Append(t)
                    End If
                    lSB.AppendLine(lStr.ToString)
                End If
            Next lML
            lSB.AppendLine("  END MASS-LINK" & CStr(mlno(i - 1)).PadLeft(5))
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