Option Strict Off
Option Explicit On

Module modUCIRecords

    Private uciRec() As String
    Private uciRecCnt As Integer

    Public Sub ReadUCIRecords(ByRef aFileName As String)
        Dim lFileUnit As Integer
        Dim lCurrentRecord As String

        lFileUnit = FreeFile()
        FileOpen(lFileUnit, aFileName, OpenMode.Input)
        uciRecCnt = 0
        ReDim uciRec(500)
        Do Until EOF(lFileUnit)
            lCurrentRecord = LineInput(lFileUnit)
            uciRecCnt = uciRecCnt + 1
            If UBound(uciRec) < uciRecCnt Then
                ReDim Preserve uciRec(uciRecCnt * 2)
            End If
            uciRec(uciRecCnt) = lCurrentRecord
        Loop
        ReDim Preserve uciRec(uciRecCnt)
        FileClose(lFileUnit)
    End Sub

    Public Function GetUCIRecord(ByRef aIndex As Integer) As String
        If aIndex >= 0 And aIndex <= uciRec.GetUpperBound(0) Then
            Return uciRec(aIndex)
        Else
            Return Nothing
        End If
    End Function

    Public Sub GetNextRecordFromBlock(ByRef blockname As String, ByRef retkey As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
        Dim i, ilen As Integer

        If retkey = -1 Then
            For i = 1 To uciRecCnt
                ilen = Len(blockname)
                If Len(uciRec(i)) >= ilen Then
                    If Left(uciRec(i), ilen) = blockname Then
                        'found start
                        retkey = i
                        Exit For
                    End If
                End If
            Next i
            retcod = 10
        End If
        'start at retkey+1
        If retkey > -1 Then
            For i = retkey + 1 To uciRecCnt
                If Trim(uciRec(i)) = "END " & blockname Then
                    retkey = 0
                    retcod = 10
                    Exit For
                End If
                If InStr(1, uciRec(i), "***") = 0 And Len(Trim(uciRec(i))) > 0 Then
                    'found a real line of this block
                    retkey = i
                    cbuff = uciRec(i)
                    rectyp = 0
                    retcod = 2
                    Exit For
                ElseIf InStr(1, uciRec(i), "***") > 0 Then
                    'found comment
                    retkey = i
                    cbuff = uciRec(i)
                    rectyp = -1
                    retcod = 2
                    Exit For
                ElseIf Len(Trim(uciRec(i))) = 0 And blockname <> "FTABLES" Then
                    'found blank line
                    retkey = i
                    cbuff = ""
                    rectyp = -2
                    retcod = 2
                    Exit For
                End If
            Next i
        End If
        If retkey = uciRecCnt Then
            retcod = 0
        End If
    End Sub

    Public Sub StartingRecordofOperationTable(ByRef opname As String, ByRef kwd As String, ByRef srec As Integer, ByRef noccur As Integer)
        Dim ostart, i, ilen, oend As Integer

        srec = 0
        noccur = 0
        ostart = 0
        For i = 1 To uciRecCnt
            ilen = Len(opname)
            If Len(uciRec(i)) >= ilen Then
                If Left(uciRec(i), ilen) = opname Then
                    'found start of this operation type block
                    ostart = i
                    Exit For
                End If
            End If
        Next i

        oend = 0
        If ostart > 0 Then
            For i = ostart + 1 To uciRecCnt
                ilen = Len("END " & opname)
                If Len(uciRec(i)) >= ilen Then
                    If Left(uciRec(i), ilen) = "END " & opname Then
                        'found end of this operation type block
                        oend = i
                        Exit For
                    End If
                End If
            Next i
        End If

        If ostart > 0 And oend > 0 Then
            For i = ostart + 1 To oend
                ilen = Len("  " & kwd)
                If Len(uciRec(i)) >= ilen And InStr(1, uciRec(i), "***") = 0 Then
                    'If Left(uciRec(i), ilen) = "  " & kwd Then
                    'pbd -- distinguish between soil-data and soil-data2 for instance
                    If RTrim(Left(uciRec(i), ilen + 1)) = "  " & kwd Then
                        'found start of this table
                        If srec = 0 Then
                            srec = i
                        End If
                        noccur = noccur + 1
                    End If
                End If
            Next i
        End If
    End Sub

    Public Sub GetNextRecordFromTable(ByRef blockname As String, ByRef tablename As String, ByRef srec As Integer, ByRef initfg As Integer, ByRef noccur As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
        Dim i, ilen As Integer

        If noccur > 1 And initfg = 1 Then
            'first time in, need to find start of the next one of these tables
            For i = srec + 1 To uciRecCnt
                ilen = Len("  " & tablename)
                If Len(uciRec(i)) >= ilen And InStr(1, uciRec(i), "***") = 0 Then
                    If Left(uciRec(i), ilen) = "  " & tablename Then
                        'found start of this table
                        'pbd 9/04 always want next occur
                        srec = i
                        Exit For
                    End If
                End If
            Next i
        End If

        For i = srec + 1 To uciRecCnt
            If RTrim(uciRec(i)) = "  END " & tablename Then
                retcod = 10
                Exit For
            End If
            If InStr(1, uciRec(i), "***") = 0 And Len(Trim(uciRec(i))) > 0 Then
                'found a real line of this block
                srec = i
                cbuff = uciRec(i)
                rectyp = 0
                retcod = 2
                Exit For
            ElseIf InStr(1, uciRec(i), "***") > 0 Then
                'found comment
                srec = i
                cbuff = uciRec(i)
                rectyp = -1
                retcod = 3
                Exit For
            End If
        Next i

        If srec = uciRecCnt Then
            retcod = 0
        End If
    End Sub

    Public Function GetCommentBeforeBlock(ByRef aBlockName As String) As String
        Dim lStartRecordIndex As Integer = -1
        For lRecordIndex As Integer = 1 To uciRecCnt
            If uciRec(lRecordIndex).StartsWith(aBlockName) Then 'found start of block
                lStartRecordIndex = lRecordIndex
                Exit For
            End If
        Next lRecordIndex

        Dim lComment As String = ""
        If lStartRecordIndex > 1 Then
            For lRecordIndex As Integer = lStartRecordIndex - 1 To 1 Step -1
                If uciRec(lRecordIndex).Trim.Length = 0 Then 'found blank line
                    If lComment.Length = 0 Then
                        lComment = " "
                    Else
                        lComment = vbCrLf & lComment
                    End If
                ElseIf uciRec(lRecordIndex).IndexOf("***") > -1 Then 'found comment
                    If lComment.Length = 0 Then
                        lComment = uciRec(lRecordIndex)
                    Else
                        lComment = uciRec(lRecordIndex) & vbCrLf & lComment
                    End If
                Else 'something on this line and its not a comment
                    Exit For
                End If
            Next lRecordIndex
        End If
        Return lComment.TrimEnd
    End Function

    Public Function GetTableComment(ByRef aStartRecord As Integer, _
                                    ByRef aTableName As String, _
                                    ByRef aThisOccur As Integer) As String

        Dim lComment As String = ""
        Dim lRecordKey As Integer = aStartRecord
        Dim lOccurCount As Integer
        If aThisOccur > 1 Then
            lRecordKey = -1
            lOccurCount = 1
            For i As Integer = aStartRecord + 1 To uciRecCnt
                If aTableName.Trim = uciRec(i).Trim Then
                    'found another occurence
                    lOccurCount += 1
                    If lOccurCount = aThisOccur Then
                        lRecordKey = i
                    End If
                    Exit For
                End If
            Next i
        End If

        'start at retkey+1
        If lRecordKey > 0 Then
            For lRecordIndex As Integer = lRecordKey - 1 To 0 Step -1
                If uciRec(lRecordIndex).Trim.Length = 0 Then 'found blank line
                    'dont tack it on if the preceeding line is real
                    If lComment.Length = 0 Then
                        lComment = " "
                    Else
                        lComment = vbCrLf & lComment
                    End If
                ElseIf uciRec(lRecordIndex).IndexOf("***") = -1 Then
                    'something on this line and its not a comment
                    Exit For
                Else 'found comment
                    If lComment.Length = 0 Then
                        lComment = uciRec(lRecordIndex)
                    Else
                        lComment = uciRec(lRecordIndex) & vbCrLf & lComment
                    End If
                End If
            Next lRecordIndex
        End If
        Return lComment.TrimEnd
    End Function

    Public Sub DefaultBlockOrder(ByRef aOrder As ArrayList)
        Dim lOrder As New ArrayList(20)
        With lOrder
            .Add("GLOBAL")
            .Add("FILES")
            .Add("OPN SEQUENCE")
            .Add("MONTH DATA")
            .Add("CATEGORY")
            .Add("PERLND")
            .Add("IMPLND")
            .Add("RCHRES")
            .Add("FTABLES")
            .Add("COPY")
            .Add("PLTGEN")
            .Add("DISPLY")
            .Add("DURANL")
            .Add("GENER")
            .Add("MUTSIN")
            .Add("BMPRAC")
            .Add("REPORT")
            .Add("CONNECTIONS")
            .Add("MASSLINKS")
            .Add("SPECIAL ACTIONS")
        End With
        aOrder = lOrder
    End Sub

    Public Sub SaveBlockOrder(ByRef aOrder As ArrayList)
        Dim lOrder As New ArrayList(20)
        Dim lConnectionFound As Boolean
        For lRecordIndex As Integer = 1 To uciRecCnt
            Dim lBlock As String = uciRec(lRecordIndex).Trim
            Select Case lBlock
                Case "GLOBAL", "FILES", "OPN SEQUENCE", "PERLND", "IMPLND", _
                     "RCHRES", "FTABLES", "COPY", "PLTGEN", "DISPLY", _
                     "DURANL", "GENER", "MUTSIN", "BMPRAC", "REPORT", _
                     "CATEGORY"
                    lOrder.Add(lBlock)
                Case "MONTH-DATA"
                    lOrder.Add("MONTH DATA")
                Case "MASS-LINK"
                    lOrder.Add("MASSLINKS")
                Case "SPEC-ACTIONS"
                    lOrder.Add("SPECIAL ACTIONS")
                Case "EXT SOURCES", "EXT TARGETS", "SCHEMATIC", "NETWORK"
                    If Not lConnectionFound Then
                        lOrder.Add("CONNECTIONS")
                        lConnectionFound = True
                    End If
            End Select
        Next

        'add any blocks in pOrder that aren't in myOrder
        Dim iprev As Integer = -1
        For Each lBlock As String In aOrder
            Dim lFoundAt As Integer = lOrder.IndexOf(lBlock)
            If lFoundAt >= 0 Then
                iprev = lFoundAt
            Else 'add this one so that myOrder has a complete set
                iprev += 1
                lOrder.Insert(iprev, lBlock)
            End If
        Next
        aOrder.Clear()
        aOrder = lOrder
    End Sub
End Module