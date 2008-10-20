'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text
Imports MapWinUtility

Public Class HspfConnection
    Public MFact As Double
    Public MFactAsRead As String
    Public Uci As HspfUci
    Public Source As HspfSrcTar
    Public Target As HspfSrcTar
    Public Tran As String
    Public Sgapstrg As String
    Public Amdstrg As String
    Public Ssystem As String
    Public MassLink As Integer
    Public Comment As String = ""
    Public Typ As Integer '1-ExtSource,2-Network,3-Schematic,4-ExtTarget
    Public ReadOnly EditControlName As String = "ATCoHspf.ctlConnectionEdit"
    Private pDesiredType As String

    Public ReadOnly Property DesiredRecordType() As String
        Get
            Return pDesiredType
        End Get
    End Property
    Public ReadOnly Property Caption() As String
        Get
            Return pDesiredType & " Block"
        End Get
    End Property

    Public Sub ReadTimSer(ByRef aUci As HspfUci)
        Dim lConnection As HspfConnection
        Dim lRecTyp As Integer
        Dim lRetCod As Integer
        Dim lBuff As String = Nothing
        Dim lStr As String

        Uci = aUci

        Dim lOmCode As Integer = HspfOmCode("EXT SOURCES")
        Dim lInit As Integer = 1
        Dim lComment As String = ""
        Dim lPastHeader As Boolean = False
        Dim lRetKey As Integer = -1
        Do
            If aUci.FastFlag Then
                GetNextRecordFromBlock("EXT SOURCES", lRetKey, lBuff, lRecTyp, lRetCod)
            Else
                lRetKey = -1
                REM_XBLOCKEX(aUci, lOmCode, lInit, lRetKey, lBuff, lRecTyp, lRetCod)
            End If
            If lRetCod <> 2 Then Exit Do
            lInit = 0
            If lRecTyp = 0 Then
                lConnection = New HspfConnection
                lConnection.Uci = aUci
                lConnection.Typ = 1
                lConnection.Source.VolName = Trim(Left(lBuff, 6))
                lConnection.Source.VolId = CInt(Mid(lBuff, 7, 4))
                lConnection.Source.Member = Trim(Mid(lBuff, 12, 6))
                lStr = lBuff.Substring(17, 2).Trim
                If lStr.Length > 0 Then
                    lConnection.Source.MemSub1 = CInt(lStr)
                End If
                lConnection.Ssystem = lBuff.Substring(20, 4)
                lConnection.Sgapstrg = lBuff.Substring(24, 4)

                lConnection.MFactAsRead = lBuff.Substring(28, 10)
                lStr = lBuff.Substring(28, 10).Trim
                If lStr.Length > 0 Then
                    lConnection.MFact = CDbl(lStr)
                End If
                lStr = lBuff.Substring(38, 4)
                If lStr.Length > 0 Then
                    lConnection.Tran = lStr
                End If
                lConnection.Target.VolName = Trim(Mid(lBuff, 44, 6))
                lConnection.Target.VolId = CInt(Mid(lBuff, 51, 3))
                lStr = lBuff.Substring(54, 3).Trim
                If lStr.Length > 0 Then lConnection.Target.VolIdL = CInt(lStr)
                lConnection.Target.Group = lBuff.Substring(58, 6).Trim
                lConnection.Target.Member = lBuff.Substring(65, 6).Trim
                lStr = lBuff.Substring(71, 2).Trim
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Target.MemSub1 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then lConnection.Target.MemSub1 = aUci.CatAsInt(lStr)
                End If
                lStr = lBuff.Substring(73, 2).Trim
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Target.MemSub2 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then lConnection.Target.MemSub2 = aUci.CatAsInt(lStr)
                End If
                lConnection.Comment = lComment
                aUci.Connections.Add(lConnection)
                lComment = ""
            ElseIf lRecTyp = -1 And lRetCod <> 1 Then
                'save comment
                If lBuff.StartsWith("<Name>") Then 'a cheap rule to identify the last header line
                    lPastHeader = True
                ElseIf lPastHeader Then
                    If lComment.Length = 0 Then
                        lComment = lBuff
                    Else
                        lComment &= vbCrLf & lBuff
                    End If
                End If
            End If
        Loop

        lOmCode = HspfOmCode("NETWORK")
        lInit = 1
        lComment = ""
        lPastHeader = False
        lRetKey = -1
        Do
            If aUci.FastFlag Then
                GetNextRecordFromBlock("NETWORK", lRetKey, lBuff, lRecTyp, lRetCod)
            Else
                lRetKey = -1
                Call REM_XBLOCKEX(aUci, lOmCode, lInit, lRetKey, lBuff, lRecTyp, lRetCod)
            End If
            If lRetCod <> 2 Then Exit Do
            lInit = 0
            If lRecTyp = 0 Then
                lConnection = New HspfConnection
                lConnection.Uci = aUci
                lConnection.Typ = 2
                lConnection.Source.VolName = lBuff.Substring(0, 6).Trim
                lConnection.Source.VolId = CInt(lBuff.Substring(6, 4))
                lConnection.Source.Group = lBuff.Substring(11, 6).Trim
                lConnection.Source.Member = lBuff.Substring(18, 6).Trim
                lStr = lBuff.Substring(24, 2).Trim
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Source.MemSub1 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then lConnection.Source.MemSub1 = aUci.CatAsInt(lStr)
                End If
                lStr = lBuff.Substring(26, 2).Trim
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Source.MemSub2 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then lConnection.Source.MemSub2 = aUci.CatAsInt(lStr)
                End If
                lConnection.MFactAsRead = lBuff.Substring(28, 10)
                lStr = lBuff.Substring(28, 10).Trim
                If lStr.Length > 0 Then lConnection.MFact = CDbl(lStr)
                lConnection.Tran = lBuff.Substring(38, 4).Trim
                lConnection.Target.VolName = lBuff.Substring(43, 6).Trim
                lConnection.Target.VolId = CInt(lBuff.Substring(50, 3))
                lStr = Trim(Mid(lBuff, 55, 3))
                If lStr.Length > 0 Then lConnection.Target.VolIdL = CInt(lStr)
                lConnection.Target.Group = Trim(Mid(lBuff, 59, 6))
                lConnection.Target.Member = Trim(Mid(lBuff, 66, 6))
                lStr = Trim(Mid(lBuff, 72, 2))
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Target.MemSub1 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then lConnection.Target.MemSub1 = aUci.CatAsInt(lStr)
                End If
                lStr = Trim(Mid(lBuff, 74, 2))
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Target.MemSub2 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then lConnection.Target.MemSub2 = aUci.CatAsInt(lStr)
                End If
                lConnection.Comment = lComment
                aUci.Connections.Add(lConnection)
                lComment = ""
            ElseIf lRecTyp = -1 And lRetCod <> 1 Then
                'save comment
                If lBuff.StartsWith("<Name>") Then 'a cheap rule to identify the last header line
                    lPastHeader = True
                ElseIf lPastHeader Then
                    If lComment.Length = 0 Then
                        lComment = lBuff
                    Else
                        lComment &= vbCrLf & lBuff
                    End If
                End If
            End If
        Loop

        lOmCode = HspfOmCode("SCHEMATIC")
        lInit = 1
        lComment = ""
        lPastHeader = False
        lRetKey = -1
        Do
            If aUci.FastFlag Then
                GetNextRecordFromBlock("SCHEMATIC", lRetKey, lBuff, lRecTyp, lRetCod)
            Else
                lRetKey = -1
                Call REM_XBLOCKEX(aUci, lOmCode, lInit, lRetKey, lBuff, lRecTyp, lRetCod)
            End If
            If lRetCod <> 2 Then
                Exit Do
            End If
            lInit = 0
            If lRecTyp = 0 Then
                lConnection = New HspfConnection
                lConnection.Uci = aUci
                lConnection.Typ = 3
                lConnection.Source.VolName = Trim(Left(lBuff, 6))
                lConnection.Source.VolId = CInt(Mid(lBuff, 7, 4))
                lStr = Trim(Mid(lBuff, 29, 10))
                lConnection.MFactAsRead = Mid(lBuff, 29, 10)
                If lStr.Length > 0 Then lConnection.MFact = CDbl(lStr)
                lConnection.Target.VolName = Trim(Mid(lBuff, 44, 6))
                lConnection.Target.VolId = CInt(Mid(lBuff, 50, 4))
                lConnection.MassLink = CInt(Mid(lBuff, 57, 4))
                lStr = Trim(Mid(lBuff, 72, 2))
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Target.MemSub1 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then lConnection.Target.MemSub1 = aUci.CatAsInt(lStr)
                End If
                lStr = Trim(Mid(lBuff, 74, 2))
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Target.MemSub2 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then lConnection.Target.MemSub2 = aUci.CatAsInt(lStr)
                End If
                lConnection.Comment = lComment
                aUci.Connections.Add(lConnection)
                lComment = ""
            ElseIf lRecTyp = -1 And lRetCod <> 1 Then
                'save comment
                If lBuff.StartsWith("<Name>") Then 'a cheap rule to identify the last header line
                    lPastHeader = True
                ElseIf lPastHeader Then
                    If lComment.Length = 0 Then
                        lComment = lBuff
                    Else
                        lComment &= vbCrLf & lBuff
                    End If
                End If
            End If
        Loop

        lOmCode = HspfOmCode("EXT TARGETS")
        lInit = 1
        lComment = ""
        lPastHeader = False
        lRetKey = -1
        Do
            If aUci.FastFlag Then
                GetNextRecordFromBlock("EXT TARGETS", lRetKey, lBuff, lRecTyp, lRetCod)
            Else
                lRetKey = -1
                Call REM_XBLOCKEX(aUci, lOmCode, lInit, lRetKey, lBuff, lRecTyp, lRetCod)
            End If
            If lRetCod <> 2 Then Exit Do
            lInit = 0
            If lRecTyp = 0 Then
                lConnection = New HspfConnection
                lConnection.Uci = aUci
                lConnection.Typ = 4
                lConnection.Source.VolName = Trim(Left(lBuff, 6))
                lConnection.Source.VolId = CInt(Mid(lBuff, 7, 4))
                lConnection.Source.Group = Trim(Mid(lBuff, 12, 6))
                lConnection.Source.Member = Trim(Mid(lBuff, 19, 6))
                lStr = Trim(Mid(lBuff, 25, 2))
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Source.MemSub1 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then
                        lConnection.Source.MemSub1 = aUci.CatAsInt(lStr)
                    End If
                End If
                lStr = Trim(Mid(lBuff, 27, 2))
                If lStr.Length > 0 And IsNumeric(lStr) Then
                    lConnection.Source.MemSub2 = CInt(lStr)
                Else
                    If lStr.Length > 0 Then
                        lConnection.Source.MemSub1 = aUci.CatAsInt(lStr)
                    End If
                End If
                lStr = Trim(Mid(lBuff, 29, 10))
                lConnection.MFactAsRead = Mid(lBuff, 29, 10)
                If lStr.Length > 0 Then lConnection.MFact = CDbl(lStr)
                lStr = Trim(Mid(lBuff, 39, 4))
                If lStr.Length > 0 Then lConnection.Tran = lStr
                lConnection.Target.VolName = Trim(Mid(lBuff, 44, 6))
                lConnection.Target.VolId = CInt(Mid(lBuff, 50, 4))
                lConnection.Target.Member = Trim(Mid(lBuff, 55, 6))
                lStr = Trim(Mid(lBuff, 61, 2))
                If lStr.Length > 0 Then lConnection.Target.MemSub1 = CInt(lStr)
                lConnection.Ssystem = Trim(Mid(lBuff, 64, 4))
                lConnection.Sgapstrg = Trim(Mid(lBuff, 69, 4))
                lConnection.Amdstrg = Trim(Mid(lBuff, 74, 4))
                lConnection.Comment = lComment
                aUci.Connections.Add(lConnection)
                lComment = ""
            ElseIf lRecTyp = -1 And lRetCod <> 1 Then
                'save comment
                If lBuff.StartsWith("<Name>") Then 'a cheap rule to identify the last header line
                    lPastHeader = True
                ElseIf lPastHeader Then
                    If lComment.Length = 0 Then
                        lComment = lBuff
                    Else
                        lComment &= vbCrLf & lBuff
                    End If
                End If
            End If
        Loop
    End Sub

    Public Sub New()
        MyBase.New()
        Source = New HspfSrcTar
        Target = New HspfSrcTar
        Typ = 0
        MFact = 1.0#
    End Sub
    Public Sub EditExtSrc()
        pDesiredType = "EXT SOURCES"
        editInit(Me, Me.Uci.Icon, True) 'add remove ok
    End Sub
    Public Sub EditExtTar()
        pDesiredType = "EXT TARGETS"
        editInit(Me, Me.Uci.Icon, True) 'add remove ok
    End Sub
    Public Sub EditNetwork()
        pDesiredType = "NETWORK"
        editInit(Me, Me.Uci.Icon, True) 'add remove ok
    End Sub
    Public Sub EditSchematic()
        pDesiredType = "SCHEMATIC"
        editInit(Me, Me.Uci.Icon, True) 'add remove ok
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        'ext sources, network, schematic, ext targets
        Static lTypeExists() As Boolean = {False, False, False, False}

        If Uci.MetSegs.Count > 0 Then
            lTypeExists(0) = True
        End If
        If Uci.PointSources.Count > 0 Then
            lTypeExists(0) = True
        End If

        Dim lOpnSeqBlock As HspfOpnSeqBlk = Uci.OpnSeqBlock
        For Each lOperation As HspfOperation In lOpnSeqBlock.Opns
            For Each lConnection As HspfConnection In lOperation.Targets
                lTypeExists(lConnection.Typ - 1) = True
            Next lConnection
            For Each lConnection As HspfConnection In lOperation.Sources
                lTypeExists(lConnection.Typ - 1) = True
            Next lConnection
        Next lOperation

        For lTypeIndex As Integer = 1 To 4
            If lTypeExists(lTypeIndex - 1) Then
                Dim lBlockName As String = ""
                Select Case lTypeIndex
                    Case 1 : lBlockName = "EXT SOURCES"
                    Case 2 : lBlockName = "NETWORK"
                    Case 3 : lBlockName = "SCHEMATIC"
                    Case 4 : lBlockName = "EXT TARGETS"
                End Select
                Dim lBlockDef As HspfBlockDef = Uci.Msg.BlockDefs.Item(lBlockName)
                Dim lTableDef As HspfTableDef = lBlockDef.TableDefs.Item(0)
                'get lengths and starting positions
                Dim lParmDefIndex As Integer = 0
                Dim iCol(15) As Integer
                Dim iLen(15) As Integer
                For Each lParmDef As HSPFParmDef In lTableDef.ParmDefs
                    iCol(lParmDefIndex) = lParmDef.StartCol
                    iLen(lParmDefIndex) = lParmDef.Length
                    lParmDefIndex += 1
                Next lParmDef
                If lTypeIndex > 1 Then
                    'don't need another blank line before ext sources
                    lSB.AppendLine(" ")
                End If
                lSB.AppendLine(lBlockName)
                'now start building the records
                Select Case lTypeIndex
                    Case 1 'ext srcs
                        lSB.AppendLine("<-Volume-> <Member> SsysSgap<--Mult-->Tran <-Target vols> <-Grp> <-Member-> ***")
                        lSB.AppendLine("<Name>   x <Name> x tem strg<-factor->strg <Name>   x   x        <Name> x x ***")
                        'do met segs - operations with assoc met segs
                        Static lOperationTypes() As String = {"PERLND", "IMPLND", "RCHRES"}
                        For Each lOperationType As String In lOperationTypes
                            For Each lMetSeg As HspfMetSeg In Uci.MetSegs
                                lSB.AppendLine(lMetSeg.ToStringFromSpecs(lOperationType, iCol, iLen))
                            Next
                        Next
                        'If pUci.PointSources.Count > 0 And pUci.MetSegs.Count > 0 Then
                        '    lSB.AppendLine("") 'write a blank line between met segs and pt srcs
                        'End If
                        'do point sources
                        For Each lPtSrc As HspfPointSource In Uci.PointSources
                            lSB.AppendLine(lPtSrc.ToStringFromSpecs(iCol, iLen))
                        Next
                        'now do everything else
                        For Each lOperation As HspfOperation In lOpnSeqBlock.Opns
                            For Each lConnection As HspfConnection In lOperation.Sources
                                If lConnection.Typ = lTypeIndex Then
                                    If lConnection.Comment.Length > 0 Then
                                        lSB.Append(lConnection.Comment & vbCrLf)
                                    End If
                                    Dim lStr As New StringBuilder
                                    lStr.Append(lConnection.Source.VolName.Trim)
                                    lStr.Append(Space(iCol(1) - lStr.Length - 1)) 'pad prev field
                                    Dim t As String = Space(iLen(1)) 'right justify numbers
                                    t = RSet(CStr(lConnection.Source.VolId), Len(t))
                                    lStr.Append(t)
                                    lStr.Append(Space(iCol(2) - lStr.Length - 1))
                                    lStr.Append(lConnection.Source.Member)
                                    lStr.Append(Space(iCol(3) - lStr.Length - 1))
                                    If lConnection.Source.MemSub1 <> 0 Then
                                        t = Space(iLen(3))
                                        t = RSet(CStr(lConnection.Source.MemSub1), Len(t))
                                        If lConnection.Source.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Source.Member, 1, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(iCol(4) - lStr.Length - 1))
                                    lStr.Append(lConnection.Ssystem)
                                    lStr.Append(Space(iCol(5) - lStr.Length - 1))
                                    lStr.Append(lConnection.Sgapstrg)
                                    lStr.Append(Space(iCol(6) - lStr.Length - 1))
                                    If NumericallyTheSame((lConnection.MFactAsRead), (lConnection.MFact)) Then
                                        lStr.Append(lConnection.MFactAsRead)
                                    ElseIf lConnection.MFact <> 1 Then
                                        lStr.Append(CStr(lConnection.MFact).PadLeft(iLen(6)))
                                    End If
                                    lStr.Append(Space(iCol(7) - lStr.Length - 1))
                                    lStr.Append(lConnection.Tran)
                                    lStr.Append(Space(iCol(8) - lStr.Length - 1))
                                    lStr.Append(lOperation.Name)
                                    lStr.Append(Space(iCol(9) - lStr.Length - 1))
                                    lStr.Append(CStr(lOperation.Id).PadLeft(iLen(9)))
                                    lStr.Append(Space(iCol(11) - lStr.Length - 1))
                                    lStr.Append(lConnection.Target.Group)
                                    lStr.Append(Space(iCol(12) - lStr.Length - 1))
                                    lStr.Append(lConnection.Target.Member)
                                    lStr.Append(Space(iCol(13) - lStr.Length - 1))
                                    If lConnection.Target.MemSub1 <> 0 Then
                                        t = CStr(lConnection.Target.MemSub1).PadLeft(iLen(13))
                                        If lConnection.Target.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Target.Member, 1, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(iCol(14) - lStr.Length - 1))
                                    If lConnection.Target.MemSub2 <> 0 Then
                                        t = CStr(lConnection.Target.MemSub2).PadLeft(iLen(14))
                                        If lConnection.Target.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Target.Member, 2, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lSB.AppendLine(lStr.ToString)
                                End If
                            Next lConnection
                        Next lOperation
                    Case 2 'network
                        lSB.AppendLine("<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Target vols> <-Grp> <-Member->  ***")
                        lSB.AppendLine("<Name>   x        <Name> x x<-factor->strg <Name>   x   x        <Name> x x  ***")
                        For Each lOperation As HspfOperation In lOpnSeqBlock.Opns
                            For Each lConnection As HspfConnection In lOperation.Sources 'used to go thru targets, misses range
                                If lConnection.Typ = lTypeIndex Then
                                    If lConnection.Comment.Length > 0 Then
                                        lSB.AppendLine(lConnection.Comment)
                                    End If
                                    Dim lStr As New StringBuilder
                                    lStr.Append(lConnection.Source.Opn.Name.Trim)
                                    lStr.Append(Space(iCol(1) - lStr.Length - 1)) 'pad prev field
                                    lStr.Append(CStr(lConnection.Source.Opn.Id).PadLeft(iLen(1)))
                                    lStr.Append(Space(iCol(2) - lStr.Length - 1))
                                    lStr.Append(lConnection.Source.Group)
                                    lStr.Append(Space(iCol(3) - lStr.Length - 1))
                                    lStr.Append(lConnection.Source.Member)
                                    lStr.Append(Space(iCol(4) - lStr.Length - 1))
                                    If lConnection.Source.MemSub1 <> 0 Then
                                        Dim t As String = CStr(lConnection.Source.MemSub1).PadLeft(iLen(4))
                                        If lConnection.Source.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Source.Member, 1, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(iCol(5) - lStr.Length - 1))
                                    If lConnection.Source.MemSub2 <> 0 Then
                                        Dim t As String = CStr(lConnection.Source.MemSub2).PadLeft(iLen(5))
                                        If lConnection.Source.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Source.Member, 2, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(iCol(6) - lStr.Length - 1))
                                    If NumericallyTheSame((lConnection.MFactAsRead), _
                                                          (lConnection.MFact)) Then
                                        lStr.Append(lConnection.MFactAsRead)
                                    ElseIf lConnection.MFact <> 1 Then
                                        lStr.Append(CStr(lConnection.MFact).PadLeft(iLen(6)))
                                    End If
                                    lStr.Append(Space(iCol(7) - lStr.Length - 1))
                                    lStr.Append(lConnection.Tran)
                                    lStr.Append(Space(iCol(8) - lStr.Length - 1))
                                    lStr.Append(lOperation.Name)
                                    lStr.Append(Space(iCol(9) - lStr.Length - 1))
                                    lStr.Append(CStr(lOperation.Id).PadLeft(iLen(9)))
                                    lStr.Append(Space(iCol(11) - lStr.Length - 1))
                                    lStr.Append(lConnection.Target.Group)
                                    lStr.Append(Space(iCol(12) - lStr.Length - 1))
                                    lStr.Append(lConnection.Target.Member)
                                    lStr.Append(Space(iCol(13) - lStr.Length - 1))
                                    If lConnection.Target.MemSub1 <> 0 Then
                                        Dim t As String = CStr(lConnection.Target.MemSub1).PadLeft(iLen(13))
                                        If lConnection.Target.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Target.Member, 1, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(iCol(14) - lStr.Length - 1))
                                    If lConnection.Target.MemSub2 <> 0 Then
                                        Dim t As String = CStr(lConnection.Target.MemSub2).PadLeft(iLen(14))
                                        If lConnection.Target.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Target.Member, 2, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lSB.AppendLine(lStr.ToString)
                                End If
                            Next lConnection
                        Next lOperation
                    Case 3 'schematic
                        lSB.AppendLine("<-Volume->                  <--Area-->     <-Volume->  <ML#> ***       <sb>")
                        lSB.AppendLine("<Name>   x                  <-factor->     <Name>   x        ***        x x")
                        For Each lOperation As HspfOperation In lOpnSeqBlock.Opns
                            For Each lConnection As HspfConnection In lOperation.Sources
                                If lConnection.Typ = lTypeIndex Then
                                    If lConnection.Comment.Length > 0 Then
                                        lSB.AppendLine(lConnection.Comment)
                                    End If
                                    If lConnection.Source.Opn Is Nothing OrElse lConnection.Target.Opn Is Nothing Then
                                        Logger.Dbg("SchematicProblem" & _
                                                   lConnection.Source.VolName & ":" & lConnection.Source.VolId & " to " & _
                                                   lConnection.Target.VolName & ":" & lConnection.Target.VolId)
                                    Else
                                        Dim lStr As New StringBuilder
                                        lStr.Append(lConnection.Source.Opn.Name.Trim)
                                        lStr.Append(Space(iCol(1) - lStr.Length - 1)) 'pad prev field
                                        lStr.Append(CStr(lConnection.Source.Opn.Id).PadLeft(iLen(1)))
                                        lStr.Append(Space(iCol(2) - lStr.Length - 1))
                                        If NumericallyTheSame((lConnection.MFactAsRead), (lConnection.MFact)) Then
                                            lStr.Append(lConnection.MFactAsRead)
                                        ElseIf lConnection.MFact <> 1 Then
                                            lConnection.MFact = System.Math.Round(lConnection.MFact, 2)
                                            lStr.Append(CStr(lConnection.MFact).PadLeft(iLen(2)))
                                        End If
                                        lStr.Append(Space(iCol(3) - lStr.Length - 1))
                                        lStr.Append(lConnection.Target.Opn.Name)
                                        lStr.Append(Space(iCol(4) - lStr.Length - 1))
                                        lStr.Append(CStr(lConnection.Target.Opn.Id).PadLeft(iLen(5)))
                                        lStr.Append(Space(iCol(5) - lStr.Length - 1))
                                        lStr.Append(CStr(lConnection.MassLink).PadLeft(iLen(5)))
                                        If lConnection.Target.MemSub1 > 0 Then
                                            lStr.Append(Space(iCol(6) - lStr.Length - 1))
                                            Dim t As String = CStr(lConnection.Target.MemSub1).PadLeft(iLen(6))
                                            If lConnection.Target.VolName = "RCHRES" Then
                                                t = Uci.IntAsCat(lConnection.Target.Member, 1, t)
                                            End If
                                            lStr.Append(t)
                                        End If
                                        If lConnection.Target.MemSub2 > 0 Then
                                            lStr.Append(Space(iCol(7) - lStr.Length - 1))
                                            Dim t As String = CStr(lConnection.Target.MemSub2).PadLeft(iLen(7))
                                            If lConnection.Target.VolName = "RCHRES" Then
                                                t = Uci.IntAsCat(lConnection.Target.Member, 2, t)
                                            End If
                                            lStr.Append(t)
                                        End If
                                        lSB.AppendLine(lStr.ToString)
                                    End If
                                End If
                            Next lConnection
                        Next lOperation
                    Case 4 'ext targ
                        lSB.AppendLine("<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Volume-> <Member> Tsys Aggr Amd ***")
                        lSB.AppendLine("<Name>   x        <Name> x x<-factor->strg <Name>   x <Name>qf  tem strg strg***")
                        For Each lOperation As HspfOperation In lOpnSeqBlock.Opns
                            For Each lConnection As HspfConnection In lOperation.Targets
                                If lConnection.Typ = lTypeIndex Then
                                    If lConnection.Comment.Length > 0 Then
                                        lSB.AppendLine(lConnection.Comment)
                                    End If
                                    Dim lStr As New StringBuilder
                                    lStr.Append(lConnection.Source.Opn.Name.Trim)
                                    lStr.Append(Space(iCol(1) - lStr.Length - 1)) 'pad prev field
                                    lStr.Append(CStr(lConnection.Source.Opn.Id).PadLeft(iLen(1)))
                                    lStr.Append(Space(iCol(2) - lStr.Length - 1))
                                    lStr.Append(lConnection.Source.Group)
                                    lStr.Append(Space(iCol(3) - lStr.Length - 1))
                                    lStr.Append(lConnection.Source.Member)
                                    lStr.Append(Space(iCol(4) - lStr.Length - 1))
                                    If lConnection.Source.MemSub1 <> 0 Then
                                        Dim t As String = CStr(lConnection.Source.MemSub1).PadLeft(iLen(4))
                                        If lConnection.Source.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Source.Member, 1, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(iCol(5) - lStr.Length - 1))
                                    If lConnection.Source.MemSub2 <> 0 Then
                                        Dim t As String = CStr(lConnection.Source.MemSub2).PadLeft(iLen(5))
                                        If lConnection.Source.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Source.Member, 2, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(iCol(6) - lStr.Length - 1))
                                    If NumericallyTheSame((lConnection.MFactAsRead), (lConnection.MFact)) Then
                                        lStr.Append(lConnection.MFactAsRead)
                                    ElseIf lConnection.MFact <> 1 Then
                                        'lConn.MFact = Format(lConn.MFact, "0.#######")
                                        'RSet t = CStr(lConn.MFact)
                                        Dim t As String = RSet(HspfTable.NumFmtRE(lConnection.MFact, iLen(6) - 1), iLen(6) - 1)
                                        lConnection.MFact = CDbl(t)
                                        lStr.Append(" " & t)
                                    End If
                                    lStr.Append(Space(iCol(7) - lStr.Length - 1))
                                    lStr.Append(lConnection.Tran)
                                    lStr.Append(Space(iCol(8) - lStr.Length - 1))
                                    lStr.Append(lConnection.Target.VolName)
                                    lStr.Append(Space(iCol(9) - lStr.Length - 1))
                                    lStr.Append(CStr(lConnection.Target.VolId).PadLeft(iLen(9)))
                                    lStr.Append(Space(iCol(10) - lStr.Length - 1))
                                    If Len(lConnection.Target.Member) > iLen(10) Then 'dont write more chars than there is room for
                                        lConnection.Target.Member = Mid(lConnection.Target.Member, 1, iLen(10))
                                    End If
                                    lStr.Append(lConnection.Target.Member.TrimEnd)
                                    If (iCol(11) - lStr.Length - 1 > 0) Then 'check to make sure not spacing zero or fewer characters
                                        lStr.Append(Space(iCol(11) - lStr.Length - 1))
                                    End If
                                    If lConnection.Target.MemSub1 <> 0 Then
                                        Dim t As String = CStr(lConnection.Target.MemSub1).PadLeft(iLen(11))
                                        If lConnection.Target.VolName = "RCHRES" Then
                                            t = Uci.IntAsCat(lConnection.Target.Member, 1, t)
                                        End If
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(iCol(12) - lStr.Length - 1))
                                    lStr.Append(lConnection.Ssystem)
                                    lStr.Append(Space(iCol(13) - lStr.Length - 1))
                                    lStr.Append(lConnection.Sgapstrg)
                                    lStr.Append(Space(iCol(14) - lStr.Length - 1))
                                    lStr.Append(lConnection.Amdstrg)
                                    lSB.AppendLine(lStr.ToString)
                                End If
                            Next lConnection
                        Next lOperation
                End Select
                lSB.AppendLine("END " & lBlockName)
            End If
        Next lTypeIndex
        Return lSB.ToString
    End Function

    Private Function NumericallyTheSame(ByRef aValueAsRead As String, ByRef aValueStored As Single) As Boolean
        'see if the current mfact value is the same as the value as read from the uci
        '4. is the same as 4.0
        If IsNumeric(aValueStored) Then
            If IsNumeric(aValueAsRead) Then
                'simple case
                If CSng(aValueAsRead) = aValueStored Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function
End Class