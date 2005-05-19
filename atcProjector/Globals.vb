Module Globals
    'Declare this as global so that it can be accessed throughout the plug-in project.
    'The variable is initialized in the plugin_Initialize event.
    Public g_MapWin As MapWindow.Interfaces.IMapWin

    Public Function FileExists(ByVal PathName As String, Optional ByRef AcceptDirectories As Boolean = False, Optional ByRef AcceptFiles As Boolean = True) As Boolean
        ' ##SUMMARY Checks to see if specified file exists.
        ' ##PARAM PathName I Full path and filename.
        ' ##PARAM AcceptDirectories I If True will return True if PathName.
        ' ##PARAM PathName I Full path and filename.
        ' ##RETURNS True if file exists.

        On Error GoTo NoSuchFile

        If GetAttr(PathName) And FileAttribute.Directory Then
            FileExists = AcceptDirectories
        Else
            FileExists = AcceptFiles
        End If
NoSuchFile:
    End Function

    'Log a debugging trace message
    Public Sub LogDbg(ByRef msg As String)
        System.Diagnostics.Debug.WriteLine(msg)
        'If Not (gIPC Is Nothing) Then gIPC.dbg(msg)
        'If LCase(Left(msg, 6)) = "status" Then frmMain.DefInstance.Status = Trim(Mid(msg, 8))
    End Sub

    'Log the use of a message box (using pMsg As ATCoMessage)
    Public Function LogMsg(ByRef aMessage As String, ByRef aTitle As String, ByVal ParamArray aButtonName() As Object) As Integer
        'Dim lButtonName() As Object

        LogDbg("LogMsg:" & aTitle & ":" & aMessage & ":" & UBound(aButtonName) + 1)
        MsgBox(aMessage, MsgBoxStyle.OKOnly, aTitle)
        Return 1
        'If UBound(aButtonName) < 0 Then
        '    ReDim lButtonName(0)
    '    lButtonName(0) = "&OK"
        'Else
        '    lButtonName = VB6.CopyArray(aButtonName)
        'End If

        'LogMsg = pMsg.ShowArray(aMessage, aTitle, lButtonName)
        'If UBound(lButtonName) > 0 Then
        '    If (LogMsg > 0) Then
    '        LogDbg("LogMsg:UserSelectedButton:" & lButtonName(LogMsg - 1))
        '    Else
        '        LogDbg("LogMsg:UserEscaped")
        '    End If
        'End If
    End Function

    Public Sub MkDirPath(ByVal newPath As String)
        ' ##SUMMARY Makes the specified directory and any above it that are not yet there.
        ' ##SUMMARY   Example: MkDirPath("C:\foo\bar") creates the "C:\foo" and "C:\foo\bar" directories if they do not already exist.
        ' ##PARAM newPath I Path to specified directory
        ' ##LOCAL UpPath - parent directory of newPath
        Dim UpPath As String

        If Len(newPath) > 0 And Not FileExists(newPath, True) Then
            If Right(newPath, 1) = "\" Then newPath = Left(newPath, Len(newPath) - 1)
            UpPath = System.IO.Path.GetDirectoryName(newPath)
            If Len(UpPath) > 0 Then MkDirPath(UpPath)
            MkDir(newPath)
        End If
    End Sub

    Public Function ReplaceString(ByRef Source As String, ByRef Find As String, ByRef ReplaceWith As String) As String
        ' ##SUMMARY Replaces instances of Find in Source with ReplaceWith (case sensitive).
        ' ##SUMMARY   Example: ReplaceString("He left", "He", "She") = "She left"
        ' ##PARAM Source I Full string to be searched
        ' ##PARAM Find I Substring to be searched for and replaced
        ' ##PARAM ReplaceWith I Substring to replace Find
        ' ##RETURNS Returns new string like Source except that _
        'any occurences of Find (case sensitive) are replaced with Replace.
        Dim retval As String
        Dim findPos As Integer
        Dim lastFindEnd As Integer
        Dim findlen As Integer
        Dim replacelen As Integer
        ' ##LOCAL retval - string to be returned as ReplaceString
        ' ##LOCAL findpos - long position of Find in Source
        ' ##LOCAL lastFindEnd - long position of first character after last replaced string in Source
        ' ##LOCAL findlen - long length of Find
        ' ##LOCAL replacelen - long length of Replace

        findlen = Len(Find)
        If findlen > 0 Then
            replacelen = Len(ReplaceWith)
            findPos = InStr(Source, Find)
            lastFindEnd = 1
            While findPos > 0
                retval = retval & Mid(Source, lastFindEnd, findPos - lastFindEnd) & ReplaceWith
                lastFindEnd = findPos + findlen
                findPos = InStr(findPos + findlen, Source, Find)
            End While
            ReplaceString = retval & Mid(Source, lastFindEnd)
        Else
            ReplaceString = Source
        End If
    End Function

    Public Function WholeFileString(ByRef filename As String) As String
        ' ##SUMMARY Converts specified text file to a string.
        ' ##PARAM FileName I Name of text file
        ' ##RETURNS Returns contents of specified text file as string.
        Dim InFile As Short
        Dim FileLength As Integer
        ' ##LOCAL InFile - long filenumber of text file
        ' ##LOCAL FileLength - long length of text file contents

        On Error GoTo ErrorReading

        InFile = FreeFile()
        FileOpen(InFile, filename, OpenMode.Input)
        FileLength = LOF(InFile)
        WholeFileString = InputString(InFile, FileLength)
        FileClose(InFile)
        Exit Function

ErrorReading:
        MsgBox("Error reading '" & filename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OKOnly, "WholeFileString")
    End Function
End Module

