'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports atcUtility

Public Class HspfFile
    Public Typ As String 'valid are MESSU, WDM(x), DSSx or blank
    Public Unit As Integer 'use 21-99
    Public Name As String 'complete path
    Public Comment As String 'preceeding comment
End Class

Public Class HspfFilesBlk
    Private pFiles As Collection(Of HspfFile)
    Private pUci As HspfUci
    Private pComment As String = ""

    Public ReadOnly Caption As String = "Files Block"
    Public Comment As String
    Friend Uci As HspfUci

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

    Public Property Value(ByVal aIndex As Integer) As HspfFile
        Get
            If aIndex > 0 And aIndex <= pFiles.Count Then
                Value = pFiles.Item(aIndex - 1)
            Else
                Value = New HspfFile
                Value.Name = ""
                Value.Typ = ""
                Value.Unit = 0
            End If
        End Get
        Set(ByVal aValue As HspfFile) '?
            If aIndex >= 0 And aIndex < pFiles.Count Then
                pFiles.RemoveAt(aIndex)
                pFiles.Insert(aIndex, aValue)
            ElseIf aIndex = pFiles.Count Then
                pFiles.Add(aValue)
            Else
                Throw New ApplicationException("Bad Files Block Index " & aIndex & " Range Must be (0-" & pFiles.Count & ")")
            End If
        End Set
    End Property

    Public Sub Clear()
        pFiles.Clear()
    End Sub

    Public Sub Add(ByRef aValue As HspfFile) 'to end, how about in between
        pFiles.Add(aValue)
    End Sub

    Public Sub AddFromSpecs(ByRef aName As String, ByRef aType As String)
        Dim lUnit As Integer = 25
        AddFromSpecs(aName, aType, lUnit)
    End Sub

    Public Sub AddFromSpecs(ByVal aName As String, _
                            ByVal aType As String, _
                            ByVal aUnit As Integer)
        Dim lNewFile As New HspfFile
        lNewFile.Name = aName
        lNewFile.Typ = aType
        'find available unit
        Dim lUnit As Integer = aUnit - 1
        Dim lFound As Boolean = True
        Do Until Not lFound
            lUnit += 1
            lFound = False
            For Each lFile As HspfFile In pFiles
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
        Dim lFile As New HspfFile
        If aIndex > 0 And aIndex <= pFiles.Count Then
            Dim lExistingFile As HspfFile = pFiles.Item(aIndex)
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
        pFiles = New Collection(Of HspfFile)
    End Sub

    Private Sub Update()
        pUci.Edited = True
    End Sub

    'Public Function Check() As String
    '	'verify values are correct in relation to each other and other tables
    'End Function

    Friend Sub ReadUciFile()
        Dim lBuff As String = Nothing
        Try
            pComment = GetCommentBeforeBlock("FILES")

            Dim lRectyp As Integer
            Dim lRetcod As Integer = 0
            Dim lInit As Integer = 1
            Dim lOmCode As Integer = HspfOmCode("FILES")
            Dim lComment As String = ""
            Dim lRetkey As Integer = -1
            Do
                GetNextRecordFromBlock("FILES", lRetkey, lBuff, lRectyp, lRetcod)

                If lRetcod = 10 Then
                    Exit Do
                ElseIf lRectyp = 0 Then
                    Dim lFile As New HspfFile
                    If Left(lBuff, 6).Trim.Length > 0 Then
                        lFile.Typ = StrRetRem(lBuff)
                    Else
                        lFile.Typ = ""
                    End If
                    lFile.Unit = CInt(StrRetRem(lBuff))
                    lFile.Name = lBuff
                    lFile.Comment = lComment
                    pFiles.Add(lFile)
                    lComment = ""
                ElseIf lRectyp = -1 Then 'save comment
                    If lComment.Length = 0 Then
                        lComment = lBuff
                    Else
                        lComment &= vbCrLf & lBuff
                    End If
                ElseIf lRetcod = 2 And lRectyp = -2 Then 'save blank line
                    If lComment.Length = 0 Then
                        lComment = " "
                    Else
                        lComment &= vbCrLf & " "
                    End If
                End If
            Loop
        Catch lEx As Exception
            MapWinUtility.Logger.Msg(lEx.ToString & vbCr & vbCr & lBuff, MsgBoxStyle.Critical, "Error in HspfFilesBlock:ReadUciFile")
        End Try
    End Sub

    Public Overrides Function ToString() As String
        Dim lSb As New System.Text.StringBuilder

        If pComment.Length > 0 Then
            lSb.AppendLine(pComment)
        End If
        lSb.AppendLine("FILES")
        If pFiles.Count > 0 Then
            Dim lFile As HspfFile = pFiles.Item(0)
            If Not (lFile.Comment Is Nothing) AndAlso _
               lFile.Comment.Length = 0 Then 'need to add default header
                lSb.AppendLine("<FILE>  <UN#>***<----FILE NAME------------------------------------------------->")
            End If
        End If

        For Each lFile As HspfFile In pFiles
            Dim lName As String = Trim(lFile.Name)
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
        Dim lFiles As New Collection(Of HspfFile)

        For Each lHspfFile As HspfFile In pFiles
            If lHspfFile.Typ.Trim = "MESSU" Or _
               lHspfFile.Typ.Trim = "" Or _
               lHspfFile.Typ.Trim = "BINO" Then
                'Close lFile.Unit
                'replace file name
                Dim lTempName As String = lHspfFile.Name.Trim
                Dim lOldLength As Integer = aOldName.Length
                Dim lTemp As Integer = InStr(1, lTempName.ToUpper, aOldName.ToUpper)
                Dim lSlashPos As Integer = lTempName.IndexOf(g_PathChar) + 1
                If ((lTemp > 0 And lSlashPos > 0 And lTemp > lSlashPos) Or _
                    (lTemp > 0 And lSlashPos = 0)) Then
                    'found the old name in this string, replace it
                    lHspfFile.Name = Mid(lTempName, 1, lTemp - 1) & aNewName & Mid(lTempName, lTemp + lOldLength)
                Else  'just add the new scen name
                    If lSlashPos = 0 Then 'no path
                        lHspfFile.Name = aNewName & "." & lTempName
                    Else 'have a path name, insert after slash
                        lHspfFile.Name = Mid(lTempName, 1, lSlashPos) & aNewName & "." & Mid(lTempName, lSlashPos + 1, lTempName.Length)
                    End If
                End If
            End If
            lFiles.Add(lHspfFile)
        Next
        pFiles.Clear()
        pFiles = lFiles
    End Sub

    Public Sub newNameAll(ByRef aOldName As String, ByRef aNewName As String)
        Dim lFiles As New Collection(Of HspfFile)

        For Each lHspfFile As HspfFile In pFiles
            If lHspfFile.Typ.Trim = "MESSU" Or _
               lHspfFile.Typ.Trim = "" Or _
               lHspfFile.Typ.Trim = "BINO" Or _
               lHspfFile.Typ.Trim.StartsWith("WDM") Then
                'Close lFile.Unit
                'replace file name
                Dim lTempName As String = lHspfFile.Name
                Dim lOldLength As Integer = aOldName.Length
                Dim lTemp As Integer = InStr(1, UCase(lTempName), UCase(aOldName))
                Dim lSlashPos As Integer = lTempName.IndexOf(g_PathChar)
                If ((lTemp > 0 And lSlashPos > 0 And lTemp > lSlashPos) Or _
                    (lTemp > 0 And lSlashPos = 0)) Then
                    'found the old name in this string, replace it
                    lHspfFile.Name = Mid(lTempName, 1, lTemp - 1) & aNewName & Mid(lTempName, lTemp + lOldLength)
                Else  'just add the new scen name
                    If lSlashPos = 0 Then 'no path
                        lHspfFile.Name = aNewName & "." & lTempName
                    Else 'have a path name, insert after slash
                        lHspfFile.Name = Mid(lTempName, 1, lSlashPos) & aNewName & "." & Mid(lTempName, lSlashPos + 1, lTempName.Length)
                    End If
                End If
            End If
            lFiles.Add(lHspfFile)
        Next
        pFiles.Clear()
        pFiles = lFiles
    End Sub
End Class