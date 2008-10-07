Option Strict Off
Option Explicit On

Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Collections.Specialized
Imports MapWinUtility

Public Module modFile

    Private ShapeExtensions() As String = {".shp", ".shx", ".dbf", ".prj", ".spx", ".sbn", ".sbx", ".xml", ".shp.xml", ".mwsr"}

    ''' <summary>
    ''' Try moving a file to a new location, log a failure rather than raising an exception
    ''' </summary>
    ''' <param name="aFromFilename">Path of file to be moved</param>
    ''' <param name="aToPath">Folder or full path to move to</param>
    ''' <returns>True if successful, False if unsuccessful</returns>
    ''' <remarks></remarks>
    Public Function TryMove(ByVal aFromFilename As String, ByVal aToPath As String) As Boolean
        TryMove = False
        Logger.Dbg("TryMove '" & aFromFilename & "' to '" & aToPath & "'")
        If FileExists(aFromFilename) Then
            Try
                Try
                    Dim lDirectory As String = aToPath
                    If IO.Directory.Exists(lDirectory) Then
                        aToPath = IO.Path.Combine(lDirectory, IO.Path.GetFileName(aFromFilename))
                    ElseIf aToPath.EndsWith(IO.Path.DirectorySeparatorChar) Then
                        IO.Directory.CreateDirectory(lDirectory)
                        aToPath = IO.Path.Combine(lDirectory, IO.Path.GetFileName(aFromFilename))
                    Else
                        lDirectory = IO.Path.GetDirectoryName(lDirectory)
                        IO.Directory.CreateDirectory(lDirectory)
                    End If
                    TryDelete(aToPath) 'Remove existing file at destination
                    IO.File.Move(aFromFilename, aToPath)
                    TryMove = True
                Catch exMove As Exception 'If moving didn't work, maybe copying will
                    Logger.Dbg("Exception '" & exMove.Message & "' while moving '" & aFromFilename & "' to '" & aToPath & "' attempting copy")
                    IO.File.Copy(aFromFilename, aToPath)
                    TryMove = True
                    TryDelete(aFromFilename)
                End Try
                Logger.Dbg("Moved file from '" & aFromFilename & "' to '" & aToPath & "'")
            Catch ex As Exception
                Logger.Dbg("Unable to move file from '" & aFromFilename & "' to '" & aToPath & "' - " & ex.Message)
            End Try
        Else
            Logger.Dbg("File to move does not exist: '" & aFromFilename & "'")
        End If
    End Function

    ''' <summary>
    ''' Delete a shape file (including all associated files: .shp, .shx, .dbf, .prj, .spx, .sbn, .xml, .shp.xml, .mwsr)
    ''' </summary>
    ''' <param name="aShapefilename">file name of shape file (must end with .shp)</param>
    ''' <returns>True if all existing files were deleted or already did not exist, 
    ''' False if any files were present which could not be deleted.</returns>
    ''' <remarks>Most likely cause of failure is shape file is open (currently on a map)</remarks>
    Public Function TryDeleteShapefile(ByVal aShapeFilename As String) As Boolean
        TryDeleteShapefile = True
        For Each lFilename As String In ShapeFilenames(aShapeFilename)
            If FileExists(lFilename) AndAlso Not TryDelete(lFilename) Then TryDeleteShapefile = False
        Next
    End Function

    ''' <summary>
    ''' Copy a shape file (including all associated files: .shp, .shx, .dbf, .prj, .spx, .sbn, .xml, .shp.xml, .mwsr)
    ''' Log any exceptions and return false rather than raising them
    ''' </summary>
    ''' <param name="aShapeFilename">full path and file name of shape file to copy (example: "C:\temp.shp")</param>
    ''' <param name="aDestinationPath">full path of destination folder or file name 
    ''' (example: "C:\NewLocation\" or "C:\NewLocation\NewFilename.shp")</param>
    ''' <returns>True if all existing files were copied, False if an existing file could not be copied.</returns>
    Public Function TryCopyShapefile(ByVal aShapeFilename As String, ByVal aDestinationPath As String) As Boolean
        TryCopyShapefile = True
        Dim lNewBaseName As String
        If IO.Path.GetExtension(aDestinationPath).Length > 0 Then
            lNewBaseName = IO.Path.GetFileNameWithoutExtension(aDestinationPath)
            aDestinationPath = IO.Path.GetDirectoryName(aDestinationPath)
        Else
            lNewBaseName = IO.Path.GetFileNameWithoutExtension(aShapeFilename)
        End If

        aShapeFilename = IO.Path.ChangeExtension(aShapeFilename, "").TrimEnd(".")
        aDestinationPath = IO.Path.Combine(aDestinationPath, lNewBaseName)

        For Each lExtension As String In ShapeExtensions
            Dim lFilename As String = aShapeFilename & lExtension
            If FileExists(lFilename) AndAlso Not TryCopy(lFilename, aDestinationPath & lExtension) Then
                TryCopyShapefile = False
            End If
        Next
    End Function

    ''' <summary>
    ''' Move a shape file (including all associated files: .shp, .shx, .dbf, .prj, .spx, .sbn, .xml, .shp.xml, .mwsr)
    ''' Log any exceptions and return false rather than raising them
    ''' </summary>
    ''' <param name="aShapeFilename">full path and file name of shape file to move (example: "C:\temp.shp")</param>
    ''' <param name="aDestinationPath">full path of destination folder or file name 
    ''' (example: "C:\NewLocation\" or "C:\NewLocation\NewFilename.shp")</param>
    ''' <returns>True if all existing files were moved, False if an existing file could not be moved.</returns>
    Public Function TryMoveShapefile(ByVal aShapeFilename As String, ByVal aDestinationPath As String) As Boolean
        TryMoveShapefile = True
        Dim lNewBaseName As String
        If IO.Path.GetExtension(aDestinationPath).Length > 0 Then
            lNewBaseName = IO.Path.GetFileNameWithoutExtension(aDestinationPath)
            aDestinationPath = IO.Path.GetDirectoryName(aDestinationPath)
        Else
            lNewBaseName = IO.Path.GetFileNameWithoutExtension(aShapeFilename)
        End If

        aShapeFilename = IO.Path.ChangeExtension(aShapeFilename, "").TrimEnd(".")
        aDestinationPath = IO.Path.Combine(aDestinationPath, lNewBaseName)

        For Each lExtension As String In ShapeExtensions
            Dim lFilename As String = aShapeFilename & lExtension
            If FileExists(lFilename) AndAlso Not TryMove(lFilename, aDestinationPath & lExtension) Then
                TryMoveShapefile = False
            End If
        Next
    End Function

    Private Function ShapeFilenames(ByVal aShapefilename As String) As ArrayList 
        ShapeFilenames = New ArrayList
        For Each lExtension As String In ShapeExtensions
            ShapeFilenames.Add(IO.Path.ChangeExtension(aShapefilename, lExtension))
        Next
    End Function

    ''' <summary>
    ''' Create a new empty folder in the user's temporary folder. 
    ''' </summary>
    ''' <param name="aBaseName">String to start temporary folder name with.</param>
    ''' <returns>
    ''' The return value will be aBaseName if it does not yet exist, or will have 
    ''' underscore and a number appended to avoid having the same name as an existing directory
    ''' </returns>
    ''' <remarks>It is the caller's responsibility to remove this folder and its contents later.</remarks>
    Public Function NewTempDir(ByVal aBaseName As String) As String
        Dim lCounter As Integer = 1
        Dim lExpirationDate As Double = Date.Now.ToOADate - 1 'Old temporary folders expire after one day
        NewTempDir = IO.Path.GetTempPath & aBaseName & IO.Path.DirectorySeparatorChar

        'If there is already a file or non-empty directory with this name, try another name
        While (IO.File.Exists(NewTempDir) OrElse _
               IO.Directory.Exists(NewTempDir) AndAlso _
               IO.Directory.GetFileSystemEntries(NewTempDir).Length > 0 AndAlso _
               IO.Directory.GetCreationTime(NewTempDir).ToOADate > lExpirationDate)
            lCounter += 1
            NewTempDir = IO.Path.GetTempPath & aBaseName & "_" & lCounter & IO.Path.DirectorySeparatorChar
        End While
        If IO.Directory.Exists(NewTempDir) Then
            Logger.Dbg("Reusing temporary directory '" & NewTempDir & "'")
            IO.Directory.Delete(NewTempDir, True)
        Else
            Logger.Dbg("Creating temporary directory '" & NewTempDir & "'")
        End If
        IO.Directory.CreateDirectory(NewTempDir)
    End Function

    'if aHelpTopic is a file, set the file to display instead of opening help
    Public Sub ShowHelp(ByVal aHelpTopic As String)
        Static lHelpFilename As String = ""
        Static lHelpProcess As Process = Nothing

        If aHelpTopic.ToLower.EndsWith(".chm") Then
            If IO.File.Exists(aHelpTopic) Then
                lHelpFilename = aHelpTopic
                Logger.Dbg("Set new help file '" & lHelpFilename & "'")
            Else
                Logger.Dbg("New help file not found at '" & lHelpFilename & "'")
            End If
        Else
            If lHelpProcess IsNot Nothing Then
                If Not lHelpProcess.HasExited Then
                    Try
                        Logger.Dbg("Killing old help process")
                        lHelpProcess.Kill()
                    Catch e As Exception
                        Logger.Dbg("Error killing old help process: " & e.Message)
                    End Try
                Else
                    Logger.Dbg("Old help process already exited")
                End If
                lHelpProcess.Close()
                lHelpProcess = Nothing
            End If

            If Not IO.File.Exists(lHelpFilename) Then
                lHelpFilename = FindFile("Please locate BASINS 4 help file", "Basins4.0.chm")
            End If

            If IO.File.Exists(lHelpFilename) Then
                If aHelpTopic.Length < 1 Then
                    Logger.Dbg("Showing help file '" & lHelpFilename & "'")
                    lHelpProcess = Process.Start("hh.exe", lHelpFilename)
                ElseIf Not aHelpTopic.Equals("CLOSE") Then
                    Logger.Dbg("Showing help file '" & lHelpFilename & "' topic '" & aHelpTopic & "'")
                    lHelpProcess = Process.Start("hh.exe", "mk:@MSITStore:" & lHelpFilename & "::/" & aHelpTopic)
                End If
            End If
        End If
    End Sub

    Public Function ChDriveDir(ByVal aPath As String) As Boolean
        ' ##SUMMARY Changes directory and, if necessary, drive. Returns True if successful.
        ' ##PARAM aPath I New pathname.
        ' ##RETURNS True if directory change is successful.
        Try
            If FileExists(aPath, True, False) Then
                If Mid(aPath, 2, 1) = ":" Then ChDrive(aPath)
                ChDir(aPath)
                Return True
            Else
                Return False
            End If
        Catch e As Exception
            Return False
        End Try
    End Function

    Public Function SafeFilename(ByVal aStr As String, _
                                 Optional ByVal aReplaceWith As String = "_") As String
        ' ##SUMMARY Converts, if necessary, non-printable characters in filename to printable alternative.
        ' ##PARAM aStr I Filename to be converted, if necessary.
        ' ##PARAM aReplaceWith I Character to replace non-printable characters in S (default="_").
        ' ##RETURNS Input parameter S with non-printable characters replaced with specific printable character (default="_").
        Dim lRetval As String = "" 'return string
        Dim lChr As String 'individual character in filename

        For i As Integer = 1 To aStr.Length
            lChr = Mid(aStr, i, 1)
            Select Case Asc(lChr)
                Case 0 : GoTo EndFound
                Case Is < 32, 34, 42, 47, 58, 60, 62, 63, 92, 124, Is > 126 : lRetval &= aReplaceWith
                Case Else : lRetval &= lChr
            End Select
        Next
EndFound:
        Return lRetval
    End Function

    Public Function FilenameNoPath(ByVal aStr As String) As String
        ' ##SUMMARY Converts full path, filename, and extension to only filename with extension.
        ' ##SUMMARY   Example: FilenameNoPath ("C:\foo\bar.txt") = "bar.txt"
        ' ##PARAM aStr I Filename with path and extension.
        ' ##RETURNS Filename and extension without directory path.
        Return IO.Path.GetFileName(aStr)
    End Function

    Public Function FileExt(ByVal aStr As String) As String
        ' ##SUMMARY Reduces full path, filename, and extension to only extension.
        ' ##SUMMARY   Example: FileExt ("C:\foo\bar.txt") = "txt"
        ' ##PARAM aStr I Filename with path and extension.
        ' ##RETURNS Extension without path or filename.
        Return Mid(IO.Path.GetExtension(aStr), 2)
    End Function

    Public Function FilenameSetExt(ByVal aStr As String, ByVal aExt As String) As String
        ' ##SUMMARY Converts extension of filename from existing to specified.
        ' ##SUMMARY   Example: FilenameSetExt ("C:\foo\bar.txt", "png") = "C:\foo\bar.png"
        ' ##SUMMARY   Example: FilenameSetExt ("C:\foo\bardtxt", "png") = "C:\foo\bardtxt.png"
        ' ##PARAM astr I Filename with path and extension.
        ' ##PARAM aExt I Extension to be added or to replace current extension.
        ' ##RETURNS Filename with new extension.
        Return IO.Path.ChangeExtension(aStr, aExt)
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
       Public Function GetShortPathName(ByVal lpszLongPath As String, _
                                        ByVal lpszShortPath As StringBuilder, _
                                        ByVal cchBuffer As Integer) As Integer
    End Function

    ''' <summary>
    ''' Convert a long path name to a short path name
    ''' </summary>
    ''' <param name="aLongPathName">Long path name to convert</param>
    ''' <returns>Converted short path name</returns>
    ''' <remarks>Use this routine when length of file name is limited (like wdm names)</remarks>
    Public Function ConvertLongPathToShort(ByVal aLongPathName As String) As String
        Dim lShortNameBuffer As New StringBuilder

        Dim lSize As Integer = GetShortPathName(aLongPathName, lShortNameBuffer, _
              lShortNameBuffer.Capacity)
        If (lSize >= lShortNameBuffer.Capacity) Then
            lShortNameBuffer.Capacity = lSize + 1
            GetShortPathName(aLongPathName, lShortNameBuffer, lShortNameBuffer.Capacity)
        End If

        Return lShortNameBuffer.ToString()

    End Function

    Public Function AbsolutePath(ByVal aFileName As String, ByVal aStartPath As String) As String
        ' ##SUMMARY Converts an relative pathname to an absolute path given the starting directory.
        ' ##SUMMARY   Example: AbsolutePath("..\Data\DataFile.wdm", "C:\BASINS\models") = "C:\BASINS\Data\DataFile.wdm"
        ' ##PARAM aFileName I Relative file path and name.
        ' ##PARAM aStartPath I Absolute starting directory from which relative path is traced.
        ' ##RETURNS Absolute path and filename.
        ' ##LOCAL slashposFilename - position of slash in filename.
        ' ##LOCAL slashposPath - position of slash in pathname.

        If Mid(aFileName, 2, 1) = ":" Then 'Already have a path that starts with a drive letter
            Return aFileName
        End If

        If aStartPath.EndsWith(IO.Path.DirectorySeparatorChar) Then
            aStartPath = Left(aStartPath, Len(aStartPath) - 1)
        End If

        Dim lSlashposFilename As Integer
        Dim lSlashposPath As Integer

        If UCase(Left(aFileName, 2)) <> UCase(Left(aStartPath, 2)) Then
            aFileName = aStartPath & IO.Path.DirectorySeparatorChar & aFileName
        End If

        lSlashposFilename = InStr(aFileName, "\..\")
        While lSlashposFilename > 0
            lSlashposPath = InStrRev(aFileName, IO.Path.DirectorySeparatorChar, lSlashposFilename - 1)
            If lSlashposPath = 0 Then
                lSlashposFilename = 0
            Else
                aFileName = Left(aFileName, lSlashposPath) & Mid(aFileName, lSlashposFilename + 4)
                lSlashposFilename = InStr(aFileName, "\..\")
            End If
        End While
        Return aFileName
    End Function

    Public Function RelativeFilename(ByVal filename As String, ByVal StartPath As String) As String
        ' ##SUMMARY Converts an absolute pathname to a relative path given the starting directory.
        ' ##SUMMARY If Filename is not on the same drive as StartPath, Filename is returned.
        ' ##SUMMARY   Example: RelativeFilename("c:\BASINS\Data\DataFile.wdm", "c:\BASINS") = "Data\DataFile.wdm"
        ' ##SUMMARY   Example: RelativeFilename("c:\BASINS\OtherData\DataFile.wdm", "c:\BASINS\Data") = "..\OtherData\DataFile.wdm"
        ' ##PARAM Filename I Absolute file path and name
        ' ##PARAM StartPath I Absolute starting directory from which relative path is traced
        ' ##RETURNS Relative path and filename.
        ' ##LOCAL slashposFilename - position of slash in filename
        ' ##LOCAL slashposPath - position of slash in pathname
        ' ##LOCAL sameUntil - position in pathname of divergence between Filename and StartPath
        Dim slashposFilename As Integer
        Dim slashposPath As Integer
        Dim sameUntil As Integer

        'Remove trailing slash if necessary
        If StartPath.EndsWith(IO.Path.DirectorySeparatorChar) Then
            StartPath = Left(StartPath, Len(StartPath) - 1)
        End If
        If Len(filename) > 2 Then
            If filename.StartsWith("..\") Then
                'Concatenate StartPath and Filename
                filename = StartPath & IO.Path.DirectorySeparatorChar & filename
            End If
        End If

        'Adjust path for Filename as necessary
        slashposFilename = InStr(filename, "\..\")
        While slashposFilename > 0
            slashposPath = InStrRev(filename, IO.Path.DirectorySeparatorChar, slashposFilename - 1)
            If slashposPath = 0 Then
                slashposFilename = 0
            Else
                filename = Left(filename, slashposPath) & Mid(filename, slashposFilename + 4)
                slashposFilename = InStr(filename, "\..\")
            End If
        End While

        If InStr(filename, IO.Path.DirectorySeparatorChar) = 0 Then
            'No path to check, so assume it is a file in StartPath
        ElseIf LCase(Left(filename, 2)) <> LCase(Left(StartPath, 2)) Then
            'filename is already relative or is on different drive
        Else
            'Reconcile StartPath and Filename
            slashposPath = Len(StartPath)
            If Mid(filename, slashposPath + 1, 1) = IO.Path.DirectorySeparatorChar Then 'Filename might include whole path
                If LCase(Left(filename, slashposPath)) = LCase(StartPath) Then
                    sameUntil = slashposPath + 1
                    GoTo FoundSameUntil
                End If
            End If
            slashposFilename = 0
            slashposPath = 0
            'Search for point of divergence between StartPath and Filename
            While slashposFilename = slashposPath
                If LCase(Left(filename, slashposPath)) = LCase(Left(StartPath, slashposPath)) Then
                    sameUntil = slashposPath
                    slashposFilename = InStr(slashposPath + 1, filename, IO.Path.DirectorySeparatorChar)
                    slashposPath = InStr(slashposPath + 1, StartPath, IO.Path.DirectorySeparatorChar)
                    If slashposPath = 0 Then slashposPath = -1 'If neither has another \, must end loop
                Else
                    slashposFilename = 0
                    slashposPath = -1
                End If
            End While
FoundSameUntil:
            'Set relative path from point of divergence between StartPath and Filename
            filename = Mid(filename, sameUntil + 1)
            If sameUntil < 1 Then sameUntil = 1
            slashposPath = InStr(sameUntil, StartPath, IO.Path.DirectorySeparatorChar)
            While slashposPath > 0
                filename = "..\" & filename
                slashposPath = InStr(slashposPath + 1, StartPath, IO.Path.DirectorySeparatorChar)
            End While
        End If
        Return filename
    End Function

    Public Sub AddFilesInDir(ByRef aFilenames As NameValueCollection, _
                             ByVal aDirName As String, _
                             ByVal aSubdirs As Boolean, _
                             Optional ByVal aFileFilter As String = "*", _
                             Optional ByVal aAttributes As Integer = 0)
        Dim dirsThisDir As NameValueCollection
        Dim fName As String
        Dim vName As Object
        Dim key As String
        Dim OrigDir As String
        Dim FileMatches As Boolean

        OrigDir = CurDir()
        If ChDriveDir(aDirName) Then

            If aFilenames Is Nothing Then aFilenames = New NameValueCollection

            'get all matching file names in this dir before changing into a subdirectory
            fName = Dir(aFileFilter, aAttributes)
            While Len(fName) > 0
                FileMatches = False
                If aAttributes > 0 Then
                    If GetAttr(fName) And aAttributes Then
                        FileMatches = True
                    End If
                ElseIf Not (GetAttr(fName) And FileAttribute.Directory) Then
                    FileMatches = True
                End If

                If FileMatches Then
                    fName = CurDir() & IO.Path.DirectorySeparatorChar & fName
                    key = fName.ToLower
                    'Try
                    vName = aFilenames.Get(key)
                    'If aFilenames.Get(key).Length > 0 Then
                    'Diagnostics.Debug.WriteLine("Already added " & fName)
                    'Else
                    'Catch e As Exception

                    'End Try
                    If vName Is Nothing Then aFilenames.Add(key, fName)
                    'End If
                End If
                fName = Dir()
            End While

            If aSubdirs Then 'find all the subdirectories in aDirName
                dirsThisDir = New NameValueCollection
                fName = Dir("*", FileAttribute.Directory)
                While Len(fName) > 0
                    If GetAttr(fName) And FileAttribute.Directory Then
                        If fName <> "." And fName <> ".." Then
                            dirsThisDir.Add(fName.ToLower, fName)
                        End If
                    End If
                    fName = Dir()
                End While

                For Each fName In dirsThisDir
                    AddFilesInDir(aFilenames, aDirName & IO.Path.DirectorySeparatorChar & fName, True, aFileFilter, aAttributes)
                Next fName

            End If
            ChDriveDir(OrigDir)
        End If
    End Sub

    Public Sub SaveFileString(ByVal filename As String, ByVal FileContents As String)
        ' ##SUMMARY Saves incoming string to a text file.
        ' ##PARAM FileName I Name of output text file
        ' ##PARAM FileContents I Incoming string to be saved to file
        Dim OutFile As Short
        ' ##LOCAL OutFile - integer filenumber of output text file

        Try
            MkDirPath(PathNameOnly(filename))
            OutFile = FreeFile()
            FileOpen(OutFile, filename, OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            Print(OutFile, FileContents)
            FileClose(OutFile)
        Catch ex As Exception
            Logger.Msg("Error writing '" & filename & "'" & vbCr & vbCr & ex.Message, "SaveFileString")
        End Try
    End Sub

    Public Sub SaveFileBytes(ByVal filename As String, ByVal FileContents() As Byte)
        ' ##SUMMARY Saves incoming Byte array to a text file.
        ' ##PARAM FileName I Name of output text file
        ' ##PARAM FileContents I Incoming Byte array to be saved to file
        Dim OutFile As Short
        ' ##LOCAL OutFile - integer filenumber of output text file

        On Error GoTo ErrorWriting

        MkDirPath(PathNameOnly(filename))

        OutFile = FreeFile()
        FileOpen(OutFile, filename, OpenMode.Binary, OpenAccess.Write, OpenShare.LockWrite)
        FilePut(OutFile, FileContents)
        FileClose(OutFile)
        Exit Sub

ErrorWriting:
        Logger.Msg("Error writing '" & filename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OkOnly, "SaveFileBytes")
    End Sub

    Public Function AppendFileString(ByVal filename As String, ByVal appendString As String) As Boolean
        ' ##SUMMARY Appends incoming string to existing text file.
        ' ##PARAM FileName I Name of existing text file
        ' ##PARAM appendString I Incoming string to be appended
        ' ##LOCAL OutFile - integer filenumber of existing text file

        Try
            MkDirPath(PathNameOnly(filename))

            Dim OutFile As Short
            OutFile = FreeFile()
            FileOpen(OutFile, filename, OpenMode.Append, OpenAccess.Write, OpenShare.LockWrite)
            Print(OutFile, appendString)
            FileClose(OutFile)
            Return True
        Catch ex As Exception
            'Logger.Msg("Error writing '" & filename & "'" & vbCr & vbCr & ex.Message, "AppendFileString")
            Return False
        End Try

    End Function

    Public Sub ReplaceStringToFile(ByVal Source As String, ByVal Find As String, ByVal ReplaceWith As String, ByVal filename As String)
        ' ##SUMMARY Saves new string like Source to Filename with _
        'occurences of Find in Source replaced with Replace.
        ' ##SUMMARY   Example: ReplaceString("He left", "He", "She") = "She left"
        ' ##PARAM Source I Full string to be searched
        ' ##PARAM Find I Substring to be searched for and replaced
        ' ##PARAM Replace I Substring to replace Find
        ' ##PARAM Filename I Name of output text file
        Dim findPos As Integer
        Dim findlen As Integer
        Dim lastFindEnd As Integer
        Dim replacelen As Integer
        Dim OutFile As Short
        ' ##LOCAL findpos - long position of Find in Source
        ' ##LOCAL findlen - long length of Find
        ' ##LOCAL lastFindEnd - long position of first character after last replaced string in Source
        ' ##LOCAL replacelen - long length of Replace
        ' ##LOCAL OutFile - integer filenumber of output text file

        findlen = Len(Find)
        If findlen > 0 Then
            replacelen = Len(ReplaceWith)
            findPos = InStr(Source, Find)
            If findPos > 0 Then
                lastFindEnd = 1
                On Error GoTo ErrorWriting
                OutFile = FreeFile()
                FileOpen(OutFile, filename, OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
                While findPos > 0
                    Print(OutFile, Mid(Source, lastFindEnd, findPos - lastFindEnd) & ReplaceWith)
                    lastFindEnd = findPos + findlen
                    findPos = InStr(findPos + findlen, Source, Find)
                End While
                Print(OutFile, Mid(Source, lastFindEnd))
                FileClose(OutFile)
            Else
                SaveFileString(filename, Source)
            End If
        Else
            SaveFileString(filename, Source)
        End If
        Exit Sub

ErrorWriting:
        Logger.Msg("Error writing '" & filename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OkOnly, "ReplaceStringToFile")
    End Sub

    'Open a file using the default method the system would have used if it was double-clicked
    Public Sub OpenFile(ByVal FileOrURL As String, Optional ByVal Wait As Boolean = False)
        Dim newProcess As New Process
        Try
            If FileOrURL <> "" Then
                'Use a .NET process() to launch the file or URL
                newProcess.StartInfo.FileName = FileOrURL
                newProcess.Start()
                If Wait Then
                    newProcess.WaitForExit()
                End If
            End If
        Catch ex As Exception
            Logger.Msg(ex.Message)
        End Try

    End Sub

    Public Function FindFile(ByVal aFileDialogTitle As String, _
                    Optional ByVal aDefaultFileName As String = " ", _
                    Optional ByVal aDefaultExt As String = "", _
                    Optional ByVal aFileFilter As String = "", _
                    Optional ByVal aUserVerifyFileName As Boolean = False, _
                    Optional ByVal aChangeIntoDir As Boolean = False, _
                    Optional ByRef aFilterIndex As Integer = 1) As String

        Dim lDir As String = CurDir()
        Dim lFileName As String = aDefaultFileName.Trim
        Dim lBaseFileName As String = IO.Path.GetFileName(lFileName).ToLower 'file name (not path) of file we are looking for
        Dim lExePath As String
        Dim lDLLpath As String

        If aDefaultExt.Length = 0 Then 'get extension from default name
            aDefaultExt = FileExt(aDefaultFileName)
        End If

        If aFileFilter.Length = 0 Then 'get filter from default ext
            If aDefaultExt.Length > 0 Then
                aFileFilter &= aDefaultExt & " Files (*." & aDefaultExt & ")|*." & aDefaultExt & "|"
            End If
            aFileFilter &= "All files (*.*)|*.*"
            aFileFilter = aFileFilter.Replace("..", ".")
        End If

        On Error Resume Next

        If lFileName.EndsWith(IO.Path.DirectorySeparatorChar) Then 'this is a folder
            Return ""
            'TODO: Implement FindFolder
            'Return FindFolder(aFileDialogTitle, _
            '            aDefaultFileName, _
            '            aDefaultExt, _
            '            aFileFilter, _
            '            aUserVerifyFileName, _
            '            aChDir2FileDir, _
            '            aFilterIndex)
        Else
            If lBaseFileName.Length > 0 AndAlso Not FileExists(lFileName, True) Then 'don't already know where it is, first look in registry
                'First look where this function would put a file location
                lFileName = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\VB and VBA Program Settings\FindFile\FoundFiles", lBaseFileName, "")
                'If lFileName.Length > 0 Then
                '    If Not FileExists(lFileName) Then 'delete bad name in registry
                '        My.Computer.Registry.CurrentUser.DeleteValue("FindFile\FoundFiles\" & lBaseFileName)
                '    End If
                'End If

                'Next, look in the part of the registry where installer puts file locations
                If Not FileExists(lFileName) Then
                    lFileName = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS\files", lBaseFileName, "")
                End If
            End If

            If Not FileExists(lFileName) AndAlso lBaseFileName.Length > 0 Then 'try some default locations if filename was specified, path was wrong or missing
                lFileName = aDefaultFileName
                If Not FileExists(lFileName) Then

                    Dim lDocumentsBasins As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Basins")
                    If IO.Directory.Exists(lDocumentsBasins) Then lFileName = FindRecursive(lBaseFileName, lDocumentsBasins)

                    If Not FileExists(lFileName) Then
                        lExePath = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location).ToLower & IO.Path.DirectorySeparatorChar
                        lDLLpath = PathNameOnly(Reflection.Assembly.GetExecutingAssembly.Location).ToLower & IO.Path.DirectorySeparatorChar

                        'First check in same folder or subfolder containing current .exe or .dll
                        lFileName = FindRecursive(lBaseFileName, lDLLpath, lExePath)

                        'If we are in bin directory, also look in some other BASINS folders
                        If lFileName.Length = 0 AndAlso lExePath.IndexOf("\bin\") > 0 Then
                            lFileName = FindRecursive(lBaseFileName, _
                                                      ReplaceString(lExePath, "\bin\", "\etc\"), _
                                                      ReplaceString(lExePath, "\bin\", "\models\"), _
                                                      ReplaceString(lExePath, "\bin\", "\docs\"), _
                                                      ReplaceString(lExePath, "\bin\", "\apr\"))
                        End If

                        'Finally, check in the Windows system folders
                        'If lFileName.Length = 0 AndAlso lExePath.IndexOf("\bin\") > 0 Then
                        '  lFileName = FindRecursive(lBaseFileName, "c:\winnt\", "c:\windows\")
                        'End If
                    End If

                    If FileExists(lFileName) Then
                        aDefaultFileName = lFileName
                    Else
                        lFileName = ""
                    End If
                End If
            End If

            If Not FileExists(lFileName) AndAlso (aFileDialogTitle Is Nothing OrElse aFileDialogTitle.Length = 0) Then
                lFileName = "" 'If we don't have a dialog title, return a blank filename instead of asking the user
            ElseIf aUserVerifyFileName OrElse Not FileExists(lFileName) Then 'ask the user
                Logger.Dbg("Asking user to find " & aDefaultFileName, aFileDialogTitle)
                Dim cdlg As New Windows.Forms.OpenFileDialog
                With cdlg
                    .Title = aFileDialogTitle
                    lFileName = AbsolutePath(lFileName, CurDir)
                    .FileName = aDefaultFileName
                    .Filter = aFileFilter
                    .FilterIndex = aFilterIndex
                    .DefaultExt = aDefaultExt
                    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                        lFileName = AbsolutePath(.FileName, CurDir)
                        aFilterIndex = .FilterIndex
                        Logger.Dbg("User specified file '" & lFileName & "'")
                    Else 'Return empty string if user clicked Cancel
                        lFileName = ""
                        Logger.Dbg("User Cancelled File Selection Dialog for " & aFileDialogTitle)
                        Logger.LastDbgText = "" 'forget about this - user was in control - no additional message box needed
                    End If
                End With

                If lBaseFileName.Length > 0 AndAlso FileExists(lFileName) Then
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\VB and VBA Program Settings\FindFile\FoundFiles", lBaseFileName, lFileName)
                End If
            End If

            If aChangeIntoDir Then
                lDir = IO.Path.GetDirectoryName(lFileName)
                If FileExists(lDir, True, False) Then ChDriveDir(lDir)
            ElseIf Not lDir.Equals(CurDir) Then 'Change back to dir where we started
                ChDriveDir(lDir)
            End If

            Return lFileName
        End If
    End Function

    'Private Function FindRecursiveArray(ByVal aFilename As String, ByVal aStartDirs() As String) As String
    '  Dim lFoundPath As String = ""
    '  For Each lStartDir As String In aStartDirs
    '    lFoundPath = FindRecursive(aFilename, lStartDir)
    '    If lFoundPath.Length > 0 Then Exit For
    '  Next
    '  Return lFoundPath
    'End Function

    'Returns full path of file found in any directory in aStartDirs or any subdirectory 
    'Returns empty string if not found
    Private Function FindRecursive(ByVal aFilename As String, ByVal ParamArray aStartDirs() As String) As String
        Dim lFoundPath As String = ""
        For Each lStartDir As String In aStartDirs
            If lStartDir.Length > 0 AndAlso Not lStartDir.EndsWith(IO.Path.DirectorySeparatorChar) Then lStartDir &= IO.Path.DirectorySeparatorChar
            If FileExists(lStartDir & aFilename) Then
                Return lStartDir & aFilename
            ElseIf FileExists(lStartDir, True, False) Then
                For Each lSubDir As String In IO.Directory.GetDirectories(lStartDir)
                    lFoundPath = FindRecursive(aFilename, lSubDir)
                    If lFoundPath.Length > 0 Then Exit For
                Next
            End If
        Next
        Return lFoundPath
    End Function

    'Given a set of file filters as used by common dialog, return the filter with the given index
    ' FindFileFilter("WDM Files (*.wdm)|*.wdm|All Files (*.*)|*.*", 1) = "WDM Files (*.wdm)|*.wdm"
    Public Function FindFileFilter(ByVal FileFilters As String, ByVal FileFilterIndex As Integer) As String
        Dim prevPipe As Integer = -1
        Dim pipePos As Integer

        Try
            'Find pipe symbol before desired filter, or start of string
            While FileFilterIndex > 1
                pipePos = FileFilters.IndexOf("|", prevPipe + 1)
                If pipePos > 0 Then
                    prevPipe = pipePos
                    pipePos = FileFilters.IndexOf("|", prevPipe + 1)
                    If pipePos > 0 Then
                        prevPipe = pipePos
                    End If
                End If
                FileFilterIndex -= 1
            End While

            'Find pipe symbol after desired filter, or end of string
            pipePos = FileFilters.IndexOf("|", prevPipe + 1)
            If pipePos > 0 Then pipePos = FileFilters.IndexOf("|", pipePos + 1)
            If pipePos < 0 Then pipePos = FileFilters.Length

            FindFileFilter = FileFilters.Substring(prevPipe + 1, pipePos - prevPipe - 1)
        Catch
            Return ""
        End Try
    End Function

    'Given a set of file filters as used by common dialog, return the index of the given filter
    ' FindFileFilterIndex("WDM Files (*.wdm)|*.wdm|All Files (*.*)|*.*", "All Files (*.*)|*.*") = 2
    'Returns 1 if not found
    Public Function FindFileFilterIndex(ByVal aAllFilters As String, ByVal aFindFilter As String) As String
        Try
            Dim filterPos As Integer = aAllFilters.IndexOf(aFindFilter)
            If filterPos = 0 Then
                Return 1
            Else
                Return CountString(Left(aAllFilters, filterPos - 1), "|") / 2 + 1
            End If
        Catch
            Return 1
        End Try
    End Function

    ''' <summary>
    ''' Return the contents of a file as a string
    ''' </summary>
    ''' <param name="aFilename">Name of text file to read</param>
    ''' <param name="aTimeoutMilliseconds">Length of time to keep trying in case of an error</param>
    ''' <returns>contents of specified file</returns>
    ''' <remarks>Timeout is desirable in cases where the file might not yet be closed properly but it will be soon</remarks>
    Public Function WholeFileString(ByVal aFilename As String, Optional ByVal aTimeoutMilliseconds As Integer = 1000) As String
        Dim lTryUntil As Date = Now.AddMilliseconds(aTimeoutMilliseconds)
        Dim lInFile As Integer
        Dim lFileLength As Integer

        WholeFileString = ""
        lInFile = FreeFile()
TryAgain:
        Try
            'IO.File.ReadAllText cannot read an open for writing file(like the logger)
            'WholeFileString = IO.File.ReadAllText(aFilename)
            FileOpen(lInFile, aFilename, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
            lFileLength = LOF(lInFile)
            WholeFileString = InputString(lInFile, lFileLength)
            FileClose(lInFile)
        Catch ex As Exception
            If Now > lTryUntil Then
                Logger.Msg("Error reading '" & aFilename & "'" & vbCr & vbCr & ex.Message, "WholeFileString - " & ex.GetType.Name)
                Return ""
            Else
                'Logger.Msg("WholeFileString error, trying again (" & ex.GetType.Name & ": " & ex.Message & ")")
                Threading.Thread.Sleep(50)
                GoTo TryAgain
            End If
        End Try
    End Function


    '    Public Function WholeFileBytes(ByVal aFilename As String) As Byte()
    '        ' ##SUMMARY Converts specified text file to Byte array
    '        ' ##PARAM FileName I Name of text file
    '        ' ##RETURNS Returns contents of specified text file in Byte array.
    '        Dim retval(0) As Byte
    '        ' ##LOCAL InFile - long filenumber of text file
    '        ' ##LOCAL retval() - byte array containing return values

    '        On Error GoTo ErrorReading

    '        WholeFileBytes = IO.File.ReadAllBytes(aFilename)

    'ErrorReading:
    '        Logger.Msg("Error reading '" & aFilename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OkOnly, "WholeFileBytes")
    '    End Function

    '''' <summary>
    '''' Compare two files and return True if contents are identical.
    '''' </summary>
    '''' <param name="aFilename1">Name of first file to compare</param>
    '''' <param name="aFilename2">Name of second file to compare</param>
    '''' <returns>true if files are identical, false if not</returns>
    '''' <remarks>An exception will occur if either file cannot be read.</remarks>
    'Public Function FilesMatch(ByVal aFilename1 As String, ByVal aFilename2 As String) As Boolean
    '    Dim LongFileLength As Long = FileLen(aFilename1)

    '    'If files are not the same size, they do not match
    '    If FileLen(aFilename2) <> LongFileLength Then Return False

    '    If LongFileLength > Integer.MaxValue Then
    '        Throw New ApplicationException("FilesMatch cannot compare files larger than 2 Gigabytes.")
    '    End If

    '    Dim FileLength As Integer = CInt(LongFileLength)
    '    Dim InFile1 As Short = FreeFile()
    '    Dim InFile2 As Short = FreeFile()
    '    Dim longBytes As Integer
    '    Dim testL1, testL2 As Integer
    '    Dim testB1, testB2 As Byte
    '    Dim i As Long

    '    ' ##LOCAL FileLength - length of first file in bytes
    '    ' ##LOCAL InFile1 - file handle of first file
    '    ' ##LOCAL InFile2 - file handle of second file
    '    ' ##LOCAL longBytes - number of bytes that can be tested in Integer-sized chunks
    '    ' ##LOCAL testL1, testL2 - Integers to read and compare more than one byte at a time
    '    ' ##LOCAL testB1, testB2 - Byte values to compare one byte at a time
    '    ' ##LOCAL i - byte index in files

    '    FileOpen(InFile1, aFilename1, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
    '    Try
    '        FileOpen(InFile2, aFilename2, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
    '    Catch ex As Exception
    '        FileClose(InFile1) 'clean up the first file which we opened before having trouble with second file
    '        Throw ex
    '    End Try

    '    Try
    '        'Compare most of the file in Integer-sized chunks
    '        longBytes = FileLength - FileLength Mod 4
    '        For i = 1 To longBytes Step 4
    '            FileGet(InFile1, testL1)
    '            FileGet(InFile2, testL2)
    '            If testL1 <> testL2 Then Return False
    '        Next

    '        'Compare any odd bytes at end of file one at a time
    '        Do While i <= FileLength
    '            FileGet(InFile1, testB1, i)
    '            FileGet(InFile2, testB2, i)
    '            If testB1 <> testB2 Then Return False
    '            i = i + 1
    '        Loop

    '        FileClose(InFile1)
    '        FileClose(InFile2)

    '        Return True 'Reached the end and found no mismatches

    '    Catch ex As Exception
    '        FileClose(InFile1)
    '        FileClose(InFile2)
    '        Throw ex
    '    End Try
    'End Function

    Public Function SwapBytes(ByVal n As Integer) As Integer
        ' ##SUMMARY Swaps between big and little endian 32-bit integers.
        ' ##SUMMARY   Example: SwapBytes(1) = 16777216
        ' ##PARAM N I Any long integer
        ' ##RETURNS Modified input parameter N.
        Dim OrigBytes As Byte()
        Dim NewBytes As Byte()
        ' ##LOCAL OrigBytes - stores original bytes
        ' ##LOCAL NewBytes - stores new bytes

        OrigBytes = BitConverter.GetBytes(n)
        ReDim NewBytes(3)
        NewBytes(0) = OrigBytes(3)
        NewBytes(1) = OrigBytes(2)
        NewBytes(2) = OrigBytes(1)
        NewBytes(3) = OrigBytes(0)
        Return BitConverter.ToInt32(NewBytes, 0)
    End Function

    Public Function ReadBigInt(ByVal InFile As Short) As Integer
        ' ##SUMMARY Reads big-endian integer from file number and converts to _
        'Intel little-endian value.
        ' ##SUMMARY   Example: ReadBigInt(1) = 1398893856
        ' ##PARAM InFile I Open file number
        ' ##RETURNS Input parameter InFile converted to Intel little-endian value.
        Dim n As Integer
        ' ##LOCAL n - variable into which data is read

        FileGet(InFile, n)
        Return SwapBytes(n)
    End Function

    Public Sub WriteBigInt(ByVal OutFile As Short, ByVal Value As Integer)
        ' ##SUMMARY Writes 32-bit integer as big endian to specified disk file.
        ' ##PARAM OutFile I File number
        ' ##PARAM Value I 32-bit integer
        FilePut(OutFile, SwapBytes(Value))
    End Sub

    Public Sub FileToBase64(ByVal InputFilePath As String, ByVal OutputFilePath As String)

        'initialize the reader to read binary
        Dim inStream As IO.Stream = IO.File.OpenRead(InputFilePath)
        Dim reader As New IO.BinaryReader(inStream)

        'read in each byte and convert it to a char
        Dim numbytes As Integer = reader.BaseStream.Length
        SaveFileString(OutputFilePath, Convert.ToBase64String(reader.ReadBytes(numbytes)))

        reader.Close()

    End Sub

    Public Function LinesInFile(ByVal aFileName As String) As IEnumerable
        Return New clsLinesInFile(aFileName)
    End Function

    Public Function LinesInFile(ByVal aFileReader As IO.BinaryReader) As IEnumerable
        Return New clsLinesInFile(aFileReader)
    End Function

    ''' <summary>
    ''' An enumerable set of lines read from a text file (or other BinaryReader)
    ''' lines in file end with carriage return and/or linefeed
    ''' end of line characters are stripped from enumerated lines returned
    ''' </summary>
    Private Class clsLinesInFile
        Implements IEnumerable, IEnumerator, IDisposable

        Private pStreamReader As IO.BinaryReader
        Private pCurrentLine As String
        Private pCanSeek As Boolean

        Public Sub New(ByVal aFileName As String)
            pStreamReader = New IO.BinaryReader(New IO.BufferedStream(New IO.FileStream(aFileName, IO.FileMode.Open, IO.FileAccess.Read), 16384))
            pCanSeek = pStreamReader.BaseStream.CanSeek
        End Sub

        Public Sub New(ByVal aStreamReader As IO.BinaryReader)
            pStreamReader = aStreamReader
            pCanSeek = pStreamReader.BaseStream.CanSeek
        End Sub

        ReadOnly Property Current() As Object Implements IEnumerator.Current
            Get
                Return pCurrentLine
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            If pStreamReader Is Nothing Then
                Return False
            Else
                Dim lChar As Char
                Dim lSb As New StringBuilder
                Dim lEndOfStream As Boolean = False
                Try
ReadCharacter:
                    If pCanSeek AndAlso pStreamReader.PeekChar = -1 Then
                        lEndOfStream = True
                    Else
                        lChar = pStreamReader.ReadChar
                        Select Case lChar
                            Case ControlChars.Cr 'Found carriage return, consume linefeed if it is next
                                If pStreamReader.PeekChar = 10 Then pStreamReader.ReadChar()
                            Case ControlChars.Lf 'Unix-style line ends without carriage return
                            Case Else 'Found a character that does not end the line
                                lSb.Append(lChar)
                                GoTo ReadCharacter
                        End Select
                        pCurrentLine = lSb.ToString
                        Logger.Progress(pStreamReader.BaseStream.Position, pStreamReader.BaseStream.Length)
                        Return True
                    End If
                Catch lEndOfStreamException As IO.EndOfStreamException
                    lEndOfStream = True
                End Try
                If lEndOfStream Then
                    Try
                        pStreamReader.Close()
                    Catch
                    End Try
                    pStreamReader = Nothing
                    pCurrentLine = lSb.ToString
                    Return (lSb.Length > 0)
                End If
            End If
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            Throw New NotSupportedException
        End Sub

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return Me
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            If Not pStreamReader Is Nothing Then
                Try
                    pStreamReader.Close()
                Catch
                End Try
                pStreamReader = Nothing
            End If
        End Sub

    End Class

    ''' <summary>
    ''' Read the next line from a text file whose lines end with carriage return and/or linefeed
    ''' </summary>
    ''' <param name="aReader"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NextLine(ByVal aReader As IO.BinaryReader) As String
        Dim ch As Char
        'TODO: test a StringBuilder in place of &= for each character
        NextLine = ""
        Try
ReadCharacter:
            ch = aReader.ReadChar
            Select Case ch
                Case ControlChars.Cr 'Found carriage return, consume linefeed if it is next
                    If aReader.PeekChar = 10 Then aReader.ReadChar()
                Case ControlChars.Lf 'Unix-style line ends without carriage return
                Case Else 'Found a character that does not end the line
                    NextLine &= ch
                    GoTo ReadCharacter
            End Select
        Catch endEx As IO.EndOfStreamException
            If NextLine.Length = 0 Then 'We had nothing to read, already finished file last time
                Throw endEx
            Else
                'Reaching the end of file is fine, we are returning the last line now
            End If
        End Try
    End Function

    ''' <summary>
    ''' Gets System.Double.NaN (not a number)
    ''' </summary>
    ''' <returns>Double.NaN</returns>
    ''' <remarks>workaround for mystery bug - program exit without message on first reference to NaN</remarks>
    Public Function GetNaN() As Double
        Return GetNaNInternal()
    End Function

    ''' <summary>
    ''' Magic - reference to System.Double.NaN in optional argument elimates problem
    ''' </summary>
    ''' <param name="aNaN"></param>
    ''' <returns>Double.NaN</returns>
    ''' <remarks>workaround for mystery bug - program exit without message on first reference to NaN</remarks>
    Private Function GetNaNInternal(Optional ByVal aNaN As Double = System.Double.NaN) As Double
        Return aNaN
    End Function

    ''' <summary>
    ''' Gets System.Double.MaxValue
    ''' </summary>
    ''' <returns>Double.MaxValue</returns>
    ''' <remarks>workaround for mystery bug - program exit without message on first reference to Double.*</remarks>
    Public Function GetMaxValue() As Double
        Return GetMaxValueInternal()
    End Function

    ''' <summary>
    ''' Magic - reference to System.Double.MaxValue in optional argument elimates problem
    ''' </summary>
    ''' <param name="aMaxValue"></param>
    ''' <returns>Double.MaxValue</returns>
    ''' <remarks>workaround for mystery bug - program exit without message on first reference to Double.*</remarks>
    Private Function GetMaxValueInternal(Optional ByVal aMaxValue As Double = System.Double.MaxValue) As Double
        Return aMaxValue
    End Function

    ''' <summary>
    ''' Gets System.Double.MinValue
    ''' </summary>
    ''' <returns>Double.MinValue</returns>
    ''' <remarks>workaround for mystery bug - program exit without message on first reference to Double.*</remarks>
    Public Function GetMinValue() As Double
        Return GetMinValueInternal()
    End Function

    ''' <summary>
    ''' Magic - reference to System.Double.MinValue in optional argument elimates problem
    ''' </summary>
    ''' <param name="aMinValue"></param>
    ''' <returns>Double.MaxValue</returns>
    ''' <remarks>workaround for mystery bug - program exit without message on first reference to Double.*</remarks>
    Private Function GetMinValueInternal(Optional ByVal aMinValue As Double = System.Double.MinValue) As Double
        Return aMinValue
    End Function
End Module
