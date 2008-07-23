'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Text
Imports MapWinUtility
Imports atcUtility

Public Class HspfFilesBlk
    Private pFiles As Collection(Of HspfData.HspfFile)
    Private pUci As HspfUci
    Private pComment As String = ""

    Friend Property Uci() As HspfUci
        Get
            Return pUci
        End Get
        Set(ByVal aUci As HspfUci)
            pUci = aUci
        End Set
    End Property

    'Public ReadOnly Property Caption() As String
    '    Get
    '        Caption = "Files Block"
    '    End Get
    'End Property

    Public Property Comment() As String
        Get
            Return pComment
        End Get
        Set(ByVal aComment As String)
            pComment = aComment
        End Set
    End Property

    'Public ReadOnly Property EditControlName() As String
    '    Get
    '        EditControlName = "ATCoHspf.ctlFilesBlkEdit"
    '    End Get
    'End Property

    Public ReadOnly Property Count() As Integer
        Get
            Return pFiles.Count
        End Get
    End Property

    Public Property Value(ByVal aIndex As Integer) As HspfData.HspfFile
        Get
            If aIndex > 0 And aIndex <= pFiles.Count Then
                Value = pFiles.Item(aIndex - 1)
            Else
                Value = New HspfData.HspfFile
                Value.Name = ""
                Value.Typ = ""
                Value.Unit = 0
            End If
        End Get
        Set(ByVal aValue As HspfData.HspfFile) '?
            If aIndex > 0 And aIndex <= pFiles.Count Then
                pFiles.RemoveAt(aIndex)
                pFiles.Insert(aIndex, aValue)
            ElseIf aIndex = pFiles.Count + 1 Then
                pFiles.Add(aValue)
            Else 'error?
            End If
        End Set
    End Property

    Public Sub Clear()
        pFiles.Clear()
    End Sub

    Public Sub Add(ByRef aValue As HspfData.HspfFile) 'to end, how about in between
        pFiles.Add(aValue)
    End Sub

    Public Sub AddFromSpecs(ByRef aName As String, ByRef aType As String)
        Dim lUnit As Integer = 25
        AddFromSpecs(aName, aType, lUnit)
    End Sub

    Public Sub AddFromSpecs(ByRef aName As String, ByRef aType As String, ByRef aUnit As Integer)
        Dim lUnit As Integer
        Dim lFound As Boolean
        Dim lNewFile As New HspfData.HspfFile

        lNewFile.Name = aName
        lNewFile.Typ = aType
        'find available unit
        lUnit = aUnit - 1
        lFound = True
        Do Until Not lFound
            lUnit += 1
            lFound = False
            For Each lFile As HspfData.HspfFile In pFiles
                If lUnit = lFile.Unit Then
                    lFound = True
                    Exit For
                End If
            Next
        Loop
        lNewFile.Unit = lUnit
        pFiles.Add(lNewFile)
    End Sub

    Public Sub Remove(ByRef aIndex As Integer)
        If aIndex > 0 And aIndex <= pFiles.Count Then
            pFiles.RemoveAt(aIndex)
        End If
    End Sub

    Public Sub SetTyp(ByRef aIndex As Integer, ByRef aType As String)
        Dim lFile As New HspfData.HspfFile
        If aIndex > 0 And aIndex <= pFiles.Count Then
            Dim lExistingFile As HspfData.HspfFile = pFiles.Item(aIndex)
            lFile.Typ = aType
            lFile.Comment = lExistingFile.Comment
            lFile.Name = lExistingFile.Name
            lFile.Unit = lExistingFile.Unit
            pFiles.RemoveAt(aIndex)
            pFiles.Add(lFile)
        End If
    End Sub

    'Public Sub Edit()
    '    editInit(Me, Me.Uci.icon, True)
    'End Sub

    Public Sub New()
        MyBase.New()
        pFiles = New Collection(Of HspfData.HspfFile)
    End Sub

    Private Sub Update()
        pUci.Edited = True
    End Sub

    'Public Function Check() As String
    '	'verify values are correct in relation to each other and other tables
    'End Function

    Friend Sub ReadUciFile()
        Dim lFile As HspfData.HspfFile
        Dim lComment As String
        Dim lRetkey, lInit, lRetcod, lOmCode, lRectyp As Integer
        Dim lBuff As String = Nothing

        Try
            If pUci.FastFlag Then
                pComment = GetCommentBeforeBlock("FILES")
            End If

            lRetcod = 0
            lInit = 1
            lOmCode = HspfOmCode("FILES")
            lComment = ""
            lRetkey = -1
            Do
                If pUci.FastFlag Then
                    GetNextRecordFromBlock("FILES", lRetkey, lBuff, lRectyp, lRetcod)
                Else
                    lRetkey = -1
                    Call REM_XBLOCKEX((Me.Uci), lOmCode, lInit, lRetkey, lBuff, lRectyp, lRetcod)
                    lInit = 0
                End If
                If lRetcod = 10 Then Exit Do
                If lRectyp = 0 Then
                    If Len(Trim(Left(lBuff, 6))) > 0 Then
                        lFile.Typ = StrRetRem(lBuff)
                    Else
                        lFile.Typ = ""
                    End If
                    lFile.Unit = CInt(StrRetRem(lBuff))
                    lFile.Name = lBuff
                    lFile.Comment = lComment
                    pFiles.Add(lFile)
                    lComment = ""
                ElseIf lRectyp = -1 Then
                    'save comment
                    If lComment.Length = 0 Then
                        lComment = lBuff
                    Else
                        lComment &= vbCrLf & lBuff
                    End If
                ElseIf lRetcod = 2 And lRectyp = -2 Then
                    'save blank line
                    If lComment.Length = 0 Then
                        lComment = " "
                    Else
                        lComment &= vbCrLf & " "
                    End If
                End If
            Loop
        Catch lEx As Exception
            Logger.Msg(lEx.ToString & vbCr & vbCr & lBuff, MsgBoxStyle.Critical, "Error in HspfFilesBlock:ReadUciFile")
        End Try
    End Sub

    Public Overrides Function ToString() As String
        Dim lSb As New StringBuilder

        If pComment.Length > 0 Then
            lSb.AppendLine(pComment)
        End If
        lSb.AppendLine("FILES")
        If pFiles.Count > 0 Then
            Dim lFile As HspfData.HspfFile = pFiles.Item(0)
            If Not (lFile.Comment Is Nothing) AndAlso lFile.Comment.Length = 0 Then
                'need to add default header
                lSb.AppendLine("<FILE>  <UN#>***<----FILE NAME------------------------------------------------->")
            End If
        End If

        For Each lFile As HspfData.HspfFile In pFiles
            Dim lName As String = lFile.Name
            If InStr(1, lName, ":") Then
                'this is the absolute path name, make relative
                'tpath = CurDir
                Dim lpath As String = IO.Path.GetDirectoryName(Me.Uci.Name)
                lName = RelativeFilename(lName, lpath)
                lFile.Name = lName
            End If
            If Not lFile.Comment Is Nothing AndAlso lFile.Comment.Length > 0 Then
                lSb.AppendLine(lFile.Comment)
            End If
            lSb.AppendLine(lFile.Typ.PadRight(10) & myFormatI(lFile.Unit, 3) & Space(3) & lName)
        Next
        lSb.AppendLine("END FILES")
        Return lSb.ToString
    End Function

    Public Sub newName(ByRef aOldName As String, ByRef aNewName As String)
        Dim lOldLength, i, j, lSlashPos, lTemp As Integer
        Dim lTempName As String
        Dim lHspfFile As HspfData.HspfFile
        Dim lFiles As New Collection(Of HspfData.HspfFile)

        For Each lHspfFile In pFiles
            If Trim(lHspfFile.Typ) = "MESSU" Or Trim(lHspfFile.Typ) = "" Or Trim(lHspfFile.Typ) = "BINO" Then
                'Close lFile.Unit
                'replace file name
                lTempName = lHspfFile.Name
                lOldLength = aOldName.Length
                lTemp = InStr(1, UCase(lTempName), UCase(aOldName))
                j = lTempName.Length
                lSlashPos = 0
                For i = 1 To j
                    'check for a path in the name
                    If (Mid(lTempName, i, 1) = "\") Then
                        lSlashPos = i
                    End If
                Next i
                If ((lTemp > 0 And lSlashPos > 0 And lTemp > lSlashPos) Or (lTemp > 0 And lSlashPos = 0)) Then
                    'found the old name in this string, replace it
                    j = aNewName.Length
                    lHspfFile.Name = Mid(lTempName, 1, lTemp - 1) & aNewName & Mid(lTempName, lTemp + lOldLength)
                Else  'just add the new scen name
                    If lSlashPos = 0 Then 'no path
                        lHspfFile.Name = aNewName & "." & lTempName
                    Else 'have a path name, insert after slash
                        lHspfFile.Name = Mid(lTempName, 1, lSlashPos) & aNewName & "." & Mid(lTempName, lSlashPos + 1, j)
                    End If
                End If
            End If
            lFiles.Add(lHspfFile)
        Next
        pFiles.Clear()
        pFiles = lFiles
    End Sub
End Class