'********************************************************************************************************
'File Name: clsStrings.vb
'Description: Useful utility functions dealing with or relating to strings.
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source Utility Library. 
'
'The Initial Developer of this version of the Original Code is Christopher Michaelis, done
'by reshifting and moving about the various utility functions from MapWindow's modPublic.vb
'(which no longer exists) and some useful utility functions from Aqua Terra Consulting.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'
'********************************************************************************************************

Public Class Strings
    Public Shared Function StrSplit(ByRef Source As String, ByRef delim As String, ByRef quote As String) As String
        ' ##SUMMARY Divides string into 2 portions at position of 1st occurence of specified _
        'delimeter. Quote specifies a particular string that is exempt from the delimeter search.
        ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "") = "Julie", and "Todd, Jane, and Ray" is returned as Source.
        ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "Julie, Todd") = "Julie, Todd", and "Jane, and Ray" is returned as Source.
        ' ##PARAM Source M String to be analyzed
        ' ##PARAM delim I Single-character string delimeter
        ' ##PARAM quote I Multi-character string exempted from search.
        ' ##RETURNS  Returns leading portion of incoming string up to first occurence of delimeter. _
        'Returns input parameter without that portion. If no delimiter in string, _
        'returns whole string, and input parameter reduced to null string.
        Dim retval As String
        Dim i As Integer
        Dim quoted As Boolean
        Dim trimlen As Integer
        Dim quotlen As Integer
        ' ##LOCAL retval - string to return as StrSplit
        ' ##LOCAL i - long character position of search through Source
        ' ##LOCAL quoted - Boolean whether quote was encountered in Source
        ' ##LOCAL trimlen - long length of delimeter, or quote if encountered first
        ' ##LOCAL quotlen - long length of quote

        Source = LTrim(Source) 'remove leading blanks
        quotlen = Len(quote)
        If quotlen > 0 Then
            i = InStr(Source, quote)
            If i = 1 Then 'string beginning
                trimlen = quotlen
                Source = Mid(Source, trimlen + 1)
                i = InStr(Source, quote) 'string end
                quoted = True
            Else
                i = InStr(Source, delim)
                trimlen = Len(delim)
            End If
        Else
            i = InStr(Source, delim)
            trimlen = Len(delim)
        End If

        If i > 0 Then 'found delimeter
            retval = Microsoft.VisualBasic.Left(Source, i - 1) 'string to return
            If Microsoft.VisualBasic.Right(retval, 1) = " " Then retval = RTrim(retval)
            Source = LTrim(Mid(Source, i + trimlen)) 'string remaining
            If quoted And Len(Source) > 0 Then
                If Microsoft.VisualBasic.Left(Source, Len(delim)) = delim Then Source = LTrim(Mid(Source, Len(delim) + 1))
            End If
        Else 'take it all
            retval = Source
            Source = "" 'nothing left
        End If

        StrSplit = retval

    End Function

    Public Shared Function WholeFileString(ByRef aFilename As String, Optional ByVal aTimeoutMilliseconds As Integer = 1000) As String
        ' ##SUMMARY Converts specified text file to a string.
        ' ##PARAM FileName I Name of text file
        ' ##RETURNS Returns contents of specified text file as string.
        Dim InFile As Integer
        Dim FileLength As Long
        Dim TryUntil As Date = Now.AddMilliseconds(aTimeoutMilliseconds)
        ' ##LOCAL InFile - filenumber of text file
        ' ##LOCAL FileLength - length of text file contents

        InFile = FreeFile()
TryAgain:
        Try
            FileOpen(InFile, aFilename, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
            FileLength = FileSystem.LOF(InFile)
            WholeFileString = InputString(InFile, CType(FileLength, Integer))
            FileClose(InFile)
        Catch ex As Exception
            If Now > TryUntil Then
                Return ""
            Else
                'MsgBox("WholeFileString error, trying again (" & ex.GetType.Name & ": " & ex.Message & ")")
                System.Threading.Thread.Sleep(50)
                GoTo TryAgain
            End If
        End Try
    End Function

    Public Shared Function IsEmpty(ByVal str As String) As Boolean
        If str Is Nothing Then Return True
        If str.Length < 1 Then Return True
        Return False
    End Function

    Public Shared Sub SaveFileString(ByRef filename As String, ByRef FileContents As String)
        ' ##SUMMARY Saves incoming string to a text file.
        ' ##PARAM FileName I Name of output text file
        ' ##PARAM FileContents I Incoming string to be saved to file
        Dim OutFile As Integer
        ' ##LOCAL OutFile - integer filenumber of output text file

        Try
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filename))
            OutFile = FileSystem.FreeFile()
            FileOpen(OutFile, filename, OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            Print(OutFile, FileContents)
            FileClose(OutFile)
        Catch ex As Exception
        End Try
    End Sub
End Class
