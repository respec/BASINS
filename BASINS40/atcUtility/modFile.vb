Option Strict Off
Option Explicit On

Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Collections.Specialized
Imports MapWinUtility

Public Module modFile
    <CLSCompliant(False)> _
    Declare Function _controlfp Lib "msvcrt.dll" (ByVal newControl As UInteger, ByVal mask As UInteger) As UInteger
    <CLSCompliant(False)> _
    Declare Sub _fpreset Lib "msvcrt.dll" ()
    <CLSCompliant(False)> _
    Declare Function _statusfp Lib "msvcrt.dll" () As UInteger

    Public g_PathChar As String = IO.Path.DirectorySeparatorChar
    Public ShapeExtensions() As String = {".shp", ".shx", ".dbf", ".prj", ".spx", ".sbn", ".sbx", ".xml", ".shp.xml", ".mwsr"}
    Public TifExtensions() As String = {".tif", ".prj", ".tfw", ".xml", ".tif.xml", ".mwsr", ".mwleg"}

    Public Function TryMove(ByVal aFromFilename As String, ByVal aToPath As String) As Boolean
        Return TryMove(aFromFilename, aToPath, False)
    End Function

    ''' <summary>
    ''' Try moving a file to a new location, log a failure rather than raising an exception
    ''' </summary>
    ''' <param name="aFromFilename">Path of file to be moved</param>
    ''' <param name="aToPath">Folder or full path to move to</param>
    ''' <param name="aVerbose">True to log what happens with Logger.Dbg</param>
    ''' <returns>True if successful, False if unsuccessful</returns>
    ''' <remarks></remarks>
    Public Function TryMove(ByVal aFromFilename As String, ByVal aToPath As String, ByVal aVerbose As Boolean) As Boolean
        Dim lTryMove As Boolean = False
        If aVerbose Then Logger.Dbg("TryMove '" & aFromFilename & "' to '" & aToPath & "'")
        If FileExists(aFromFilename) Then
            Try
                Try
                    Dim lDirectory As String = aToPath
                    If IO.Directory.Exists(lDirectory) Then
                        aToPath = IO.Path.Combine(lDirectory, IO.Path.GetFileName(aFromFilename))
                    ElseIf aToPath.EndsWith(g_PathChar) Then
                        IO.Directory.CreateDirectory(lDirectory)
                        aToPath = IO.Path.Combine(lDirectory, IO.Path.GetFileName(aFromFilename))
                    Else
                        lDirectory = IO.Path.GetDirectoryName(lDirectory)
                        IO.Directory.CreateDirectory(lDirectory)
                    End If
                    TryDelete(aToPath, aVerbose) 'Remove existing file at destination
                    IO.File.Move(aFromFilename, aToPath)
                    lTryMove = True
                Catch exMove As Exception 'If moving didn't work, maybe copying will
                    If aVerbose Then Logger.Dbg("Exception '" & exMove.Message & "' while moving '" & aFromFilename & "' to '" & aToPath & "' attempting copy")
                    IO.File.Copy(aFromFilename, aToPath)
                    lTryMove = True
                    TryDelete(aFromFilename, aVerbose)
                End Try
                If aVerbose Then Logger.Dbg("Moved file from '" & aFromFilename & "' to '" & aToPath & "'")
            Catch ex As Exception
                If aVerbose Then Logger.Dbg("Unable to move file from '" & aFromFilename & "' to '" & aToPath & "' - " & ex.Message)
            End Try
        Else
            If aVerbose Then Logger.Dbg("File to move does not exist: '" & aFromFilename & "'")
        End If
        Return lTryMove
    End Function

    ''' <summary>
    ''' Delete a shape file (including all associated files: .shp, .shx, .dbf, .prj, .spx, .sbn, .xml, .shp.xml, .mwsr)
    ''' </summary>
    ''' <param name="aShapefilename">file name of shape file (must end with .shp)</param>
    ''' <returns>True if all existing files were deleted or already did not exist, 
    ''' False if any files were present which could not be deleted.</returns>
    ''' <remarks>Most likely cause of failure is shape file is open (currently on a map)</remarks>
    Public Function TryDeleteShapefile(ByVal aShapeFilename As String) As Boolean
        Return TryDeleteGroup(aShapeFilename, ShapeExtensions)
    End Function

    ''' <summary>
    ''' Delete a group of files with the same base name and different extensions
    ''' </summary>
    ''' <param name="aBaseFilename">full path and file name of one file to delete (example: "C:\temp.shp")</param>
    ''' <param name="aExtensions">all possible extensions to copy, examples are ShapeExtensions and TifExtensions above</param>
    ''' <param name="aVerbose">True to log what happens with Logger.Dbg</param>
    ''' <returns>True if all existing files were deleted or already did not exist, 
    ''' False if any files were present which could not be deleted.</returns>
    ''' <remarks>Most likely cause of failure is a file is open (for example is currently on a map)</remarks>
    Public Function TryDeleteGroup(ByVal aBaseFilename As String, ByVal aExtensions() As String, Optional ByVal aVerbose As Boolean = False) As Boolean
        Dim lSuccess As Boolean = True
        Dim lFilename As String
        For Each lExtension As String In aExtensions
            lFilename = IO.Path.ChangeExtension(aBaseFilename, lExtension)
            If IO.File.Exists(lFilename) AndAlso Not TryDelete(lFilename, aVerbose) Then
                lSuccess = False
                Exit For
            End If
        Next
        Return lSuccess
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
        Return TryCopyGroup(aShapeFilename, aDestinationPath, ShapeExtensions)
    End Function

    ''' <summary>
    ''' Copy a group of files with the same base name and different extensions
    ''' Log any exceptions and return false rather than raising them
    ''' </summary>
    ''' <param name="aBaseFilename">full path and file name of one file to copy (example: "C:\temp.shp")</param>
    ''' <param name="aDestinationPath">full path of destination folder or file name 
    ''' (example: "C:\NewLocation\" or "C:\NewLocation\NewFilename.shp")</param>
    ''' <param name="aExtensions">all possible extensions to copy, examples are ShapeExtensions and TifExtensions above</param>
    ''' <param name="aVerbose">True to log what happens with Logger.Dbg</param>
    ''' <returns>True if all existing files were copied, False if an existing file could not be copied.</returns>
    Public Function TryCopyGroup(ByVal aBaseFilename As String, ByVal aDestinationPath As String, ByVal aExtensions() As String, Optional ByVal aVerbose As Boolean = False) As Boolean
        TryCopyGroup = True
        Dim lNewBaseName As String
        If IO.Path.GetExtension(aDestinationPath).Length > 0 Then
            lNewBaseName = IO.Path.GetFileNameWithoutExtension(aDestinationPath)
            aDestinationPath = IO.Path.GetDirectoryName(aDestinationPath)
        Else
            lNewBaseName = IO.Path.GetFileNameWithoutExtension(aBaseFilename)
        End If

        aBaseFilename = IO.Path.ChangeExtension(aBaseFilename, "").TrimEnd(".")
        aDestinationPath = IO.Path.Combine(aDestinationPath, lNewBaseName)

        For Each lExtension As String In aExtensions
            Dim lFilename As String = aBaseFilename & lExtension
            If IO.File.Exists(lFilename) AndAlso Not TryCopy(lFilename, aDestinationPath & lExtension, aVerbose) Then
                TryCopyGroup = False
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
        Return TryMoveGroup(aShapeFilename, aDestinationPath, ShapeExtensions)
    End Function

    ''' <summary>
    ''' Move a group of files with the same base name and different extensions
    ''' Log any exceptions and return false rather than raising them
    ''' </summary>
    ''' <param name="aBaseFilename">full path and file name of shape file to move (example: "C:\temp.shp")</param>
    ''' <param name="aDestinationPath">full path of destination folder or file name 
    ''' (example: "C:\NewLocation\" or "C:\NewLocation\NewFilename.shp")</param>
    ''' <param name="aExtensions">all possible extensions to copy, examples are ShapeExtensions and TifExtensions above</param>
    ''' <param name="aVerbose">True to log what happens with Logger.Dbg</param>
    ''' <returns>True if all existing files were moved, False if an existing file could not be moved.</returns>
    Public Function TryMoveGroup(ByVal aBaseFilename As String, ByVal aDestinationPath As String, ByVal aExtensions() As String, Optional ByVal aVerbose As Boolean = False) As Boolean
        Dim lTryMoveGroup As Boolean = True
        Dim lNewBaseName As String
        If IO.Path.GetExtension(aDestinationPath).Length > 0 Then
            lNewBaseName = IO.Path.GetFileNameWithoutExtension(aDestinationPath)
            aDestinationPath = IO.Path.GetDirectoryName(aDestinationPath)
        Else
            lNewBaseName = IO.Path.GetFileNameWithoutExtension(aBaseFilename)
        End If

        aBaseFilename = IO.Path.ChangeExtension(aBaseFilename, "").TrimEnd(".")
        aDestinationPath = IO.Path.Combine(aDestinationPath, lNewBaseName)

        For Each lExtension As String In aExtensions
            Dim lFilename As String = aBaseFilename & lExtension
            If IO.File.Exists(lFilename) AndAlso Not TryMove(lFilename, aDestinationPath & lExtension, aVerbose) Then
                lTryMoveGroup = False
            End If
        Next
        Return lTryMoveGroup
    End Function

    ''' <summary>
    ''' Return a new file name in the IO.Path.GetTempPath folder with a given base file name and extension.
    ''' An integer is inserted between the base file name and extension if needed to avoid conflict with an existing file.
    ''' If aBaseName.aExtension already exists (as a file or folder), 
    '''    aBaseName_1.aExtension is tried, then 
    '''    aBaseName_2.aExtension, ..., until a file name is found for which the file does not yet exist
    ''' Temporary files/folders found older than one day will be deleted and the name will be reused
    ''' </summary>
    ''' <remarks>It is the caller's responsibility to both create and remove this file. Two identical calls will get the same result if the file is not created before the second call.</remarks>
    Public Function GetTemporaryFileName(ByVal aBaseName As String, ByVal aExtension As String) As String
        Dim lCounter As Integer = 1
        Dim lExpirationDate As Double = Date.Now.ToOADate - 1 'Old temporary files/folders expire after one day
        If aBaseName Is Nothing OrElse aBaseName.Length = 0 Then aBaseName = "temp"
        Dim lBaseName As String
        If IO.Path.IsPathRooted(aBaseName) Then
            lBaseName = aBaseName
        Else
            lBaseName = IO.Path.Combine(IO.Path.GetTempPath, aBaseName)
        End If
        Dim lName As String = lBaseName
        If aExtension IsNot Nothing AndAlso aExtension.Length > 0 Then lName = IO.Path.ChangeExtension(lName, aExtension)
        While FileExists(lName, True)
            'First, see if existing temp file/folder is expired and try deleting it if so.
            'If not expired, or expired but cannot delete, then try next numbered name
            If IO.File.GetCreationTime(lName).ToOADate > lExpirationDate OrElse Not TryDelete(lName) Then
                lCounter += 1
                lName = lBaseName & "_" & lCounter
                If aExtension IsNot Nothing AndAlso aExtension.Length > 0 Then lName = IO.Path.ChangeExtension(lName, aExtension)
            End If
        End While
        Return lName
    End Function

    ''' <summary>
    ''' Return a file name matching the given pattern which does not yet exist.
    ''' If aBaseName.aExtension already exists (as a file or folder), 
    '''    aBaseName_1.aExtension is tried, then 
    '''    aBaseName_2.aExtension, ..., until a file name is found for which the file does not yet exist
    ''' </summary>
    ''' <remarks>It is the caller's responsibility to both create and remove this file. Two identical calls will get the same result if the file is not created before the second call.</remarks>
    Public Function GetNewFileName(ByVal aBaseName As String, ByVal aExtension As String) As String
        Dim lCounter As Integer = 1
        If aBaseName Is Nothing OrElse aBaseName.Length = 0 Then
            aBaseName = "temp"
        Else
            aBaseName = IO.Path.Combine(IO.Path.GetDirectoryName(aBaseName), IO.Path.GetFileNameWithoutExtension(aBaseName))
        End If
        Dim lBaseName As String
        If IO.Path.IsPathRooted(aBaseName) Then
            lBaseName = aBaseName
        Else
            lBaseName = IO.Path.Combine(CurDir, aBaseName)
        End If
        Dim lName As String = lBaseName
        If aExtension IsNot Nothing AndAlso aExtension.Length > 0 Then lName = IO.Path.ChangeExtension(lName, aExtension)
        While IO.File.Exists(lName) OrElse IO.Directory.Exists(lName)
            lCounter += 1
            lName = lBaseName & "_" & lCounter
            If aExtension IsNot Nothing AndAlso aExtension.Length > 0 Then lName = IO.Path.ChangeExtension(lName, aExtension)
        End While
        Return lName
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
        Dim lName As String = GetTemporaryFileName(aBaseName, Nothing) & g_PathChar
        Logger.Dbg("Creating temporary directory '" & lName & "'")
        IO.Directory.CreateDirectory(lName)
        Return lName
    End Function

    ''' <summary>
    ''' Show Help for specified topic
    ''' </summary>
    ''' <param name="aHelpTopic">Topic to display</param>
    ''' <remarks>if aHelpTopic is a file, set the file to display instead of opening help</remarks>
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

            If Not aHelpTopic.Equals("CLOSE") Then
                If Not IO.File.Exists(lHelpFilename) Then
                    lHelpFilename = FindFile("Please locate BASINS 4 help file", "Basins4.0.chm")
                End If

                If IO.File.Exists(lHelpFilename) Then
                    If aHelpTopic.Length < 1 Then
                        Logger.Dbg("Showing help file '" & lHelpFilename & "'")
                        lHelpProcess = Process.Start("hh.exe", lHelpFilename)
                    Else
                        Logger.Dbg("Showing help file '" & lHelpFilename & "' topic '" & aHelpTopic & "'")
                        lHelpProcess = Process.Start("hh.exe", "mk:@MSITStore:" & lHelpFilename & "::/" & aHelpTopic)
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Changes directory and, if necessary, drive. Returns True if successful.
    ''' </summary>
    ''' <param name="aPath">New pathname</param>
    ''' <returns>True if directory change is successful</returns>
    ''' <remarks></remarks>
    Public Function ChDriveDir(ByVal aPath As String) As Boolean
        Try
            If IO.Directory.Exists(aPath) Then
                IO.Directory.SetCurrentDirectory(aPath)
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

        If aStartPath.EndsWith(g_PathChar) Then
            aStartPath = Left(aStartPath, Len(aStartPath) - 1)
        End If

        Dim lSlashposFilename As Integer
        Dim lSlashposPath As Integer

        If UCase(Left(aFileName, 2)) <> UCase(Left(aStartPath, 2)) Then
            aFileName = aStartPath & g_PathChar & aFileName
        End If

        lSlashposFilename = InStr(aFileName, g_PathChar & ".." & g_PathChar)
        While lSlashposFilename > 0
            lSlashposPath = InStrRev(aFileName, g_PathChar, lSlashposFilename - 1)
            If lSlashposPath = 0 Then
                lSlashposFilename = 0
            Else
                aFileName = Left(aFileName, lSlashposPath) & Mid(aFileName, lSlashposFilename + 4)
                lSlashposFilename = InStr(aFileName, g_PathChar & ".." & g_PathChar)
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
        If StartPath.EndsWith(g_PathChar) Then
            StartPath = Left(StartPath, Len(StartPath) - 1)
        End If
        If Len(filename) > 2 Then
            If filename.StartsWith(".." & g_PathChar) Then
                'Concatenate StartPath and Filename
                filename = StartPath & g_PathChar & filename
            End If
        End If

        'Adjust path for Filename as necessary
        slashposFilename = InStr(filename, g_PathChar & ".." & g_PathChar)
        While slashposFilename > 0
            slashposPath = InStrRev(filename, g_PathChar, slashposFilename - 1)
            If slashposPath = 0 Then
                slashposFilename = 0
            Else
                filename = Left(filename, slashposPath) & Mid(filename, slashposFilename + 4)
                slashposFilename = InStr(filename, g_PathChar & ".." & g_PathChar)
            End If
        End While

        If InStr(filename, g_PathChar) = 0 Then
            'No path to check, so assume it is a file in StartPath
        ElseIf LCase(Left(filename, 2)) <> LCase(Left(StartPath, 2)) Then
            'filename is already relative or is on different drive
        Else
            'Reconcile StartPath and Filename
            slashposPath = Len(StartPath)
            If Mid(filename, slashposPath + 1, 1) = g_PathChar Then 'Filename might include whole path
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
                    slashposFilename = InStr(slashposPath + 1, filename, g_PathChar)
                    slashposPath = InStr(slashposPath + 1, StartPath, g_PathChar)
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
            slashposPath = InStr(sameUntil, StartPath, g_PathChar)
            While slashposPath > 0
                filename = ".." & g_PathChar & filename
                slashposPath = InStr(slashposPath + 1, StartPath, g_PathChar)
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
            Windows.Forms.Application.DoEvents()
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
                    fName = CurDir() & g_PathChar & fName
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
                    AddFilesInDir(aFilenames, aDirName & g_PathChar & fName, True, aFileFilter, aAttributes)
                Next fName

            End If
            ChDriveDir(OrigDir)
        End If
    End Sub

    Public Function ReportFilesInDir(ByVal aDirName As String, _
                                     ByVal aSubdirs As Boolean, _
                            Optional ByVal aFileFilter As String = "*", _
                            Optional ByVal aAttributes As Integer = 0) As String
        'Dim lLastDoEvents As Date = Now
        Dim lReport As New System.Text.StringBuilder
        Dim lSkipFilename As Integer = aDirName.Length
        lReport.AppendLine("Files in " & aDirName)

        Dim lallFiles As New Collections.Specialized.NameValueCollection
        AddFilesInDir(lallFiles, aDirName, aSubdirs, aFileFilter, aAttributes)
        For Each lFilename As String In lallFiles
            lReport.AppendLine(FileDateTime(lFilename).ToString("yyyy-MM-dd HH:mm:ss") & vbTab & StrPad(Format(FileLen(lFilename), "#,###"), 10) & vbTab & lFilename.Substring(lSkipFilename))
            'If Now.Subtract(lLastDoEvents).TotalSeconds > 0.01 Then
            Windows.Forms.Application.DoEvents()
            'lLastDoEvents = Now
            'End If
        Next
        Return lReport.ToString
    End Function

    Public Sub SaveFileString(ByVal filename As String, ByVal FileContents As String)
        ' ##SUMMARY Saves incoming string to a text file.
        ' ##PARAM FileName I Name of output text file
        ' ##PARAM FileContents I Incoming string to be saved to file
        Dim OutFile As Short = FreeFile()
        ' ##LOCAL OutFile - integer filenumber of output text file

Retry:
        Try
            MkDirPath(PathNameOnly(filename))
            FileOpen(OutFile, filename, OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            Print(OutFile, FileContents)
            FileClose(OutFile)
        Catch ex As Exception
            If Logger.Msg("Error writing '" & filename & "'" & vbCr & vbCr & ex.Message, Microsoft.VisualBasic.MsgBoxStyle.RetryCancel, "SaveFileString") = MsgBoxResult.Retry Then
                GoTo Retry
            End If
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

        If aDefaultExt Is Nothing OrElse aDefaultExt.Length = 0 Then 'get extension from default name
            aDefaultExt = FileExt(aDefaultFileName)
        End If

        If aFileFilter Is Nothing OrElse aFileFilter.Length = 0 Then 'get filter from default ext
            aFileFilter = ""
            If aDefaultExt.Length > 0 Then
                aFileFilter &= aDefaultExt & " Files (*." & aDefaultExt & ")|*." & aDefaultExt & "|"
            End If
            aFileFilter &= "All files (*.*)|*.*"
            aFileFilter = aFileFilter.Replace("..", ".")
        End If

        On Error Resume Next

        If lFileName.EndsWith(g_PathChar) Then 'this is a folder
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
                '        My.Computer.Registry.CurrentUser.DeleteValue("FindFile\FoundFiles" & g_PathChar & lBaseFileName)
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
                        lExePath = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location).ToLower & g_PathChar
                        lDLLpath = PathNameOnly(Reflection.Assembly.GetExecutingAssembly.Location).ToLower & g_PathChar

                        'First check in same folder or subfolder containing current .exe or .dll
                        lFileName = FindRecursive(lBaseFileName, lDLLpath, lExePath)

                        'If we are in bin directory, also look in some other BASINS folders
                        If lFileName.Length = 0 AndAlso lExePath.IndexOf(g_PathChar & "bin" & g_PathChar) > 0 Then
                            lFileName = FindRecursive(lBaseFileName, _
                                                      ReplaceString(lExePath, g_PathChar & "bin" & g_PathChar, g_PathChar & "etc" & g_PathChar), _
                                                      ReplaceString(lExePath, g_PathChar & "bin" & g_PathChar, g_PathChar & "models" & g_PathChar), _
                                                      ReplaceString(lExePath, g_PathChar & "bin" & g_PathChar, g_PathChar & "docs" & g_PathChar), _
                                                      ReplaceString(lExePath, g_PathChar & "bin" & g_PathChar, g_PathChar & "apr" & g_PathChar))
                        End If

                        'Finally, check in the Windows system folders
                        'If lFileName.Length = 0 AndAlso lExePath.IndexOf("\bin" & g_PathChar) > 0 Then
                        '  lFileName = FindRecursive(lBaseFileName, "c:\winnt" & g_PathChar, "c:\windows\")
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
            If lStartDir.Length > 0 AndAlso Not lStartDir.EndsWith(g_PathChar) Then lStartDir &= g_PathChar
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

    Public Function LinesInFileReadLine(ByVal aFileReader As IO.StreamReader) As IEnumerable
        Return New clsLinesInFileReadLine(aFileReader)
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
        Private pNextChar As Char = Nothing
        Private pHaveNextChar As Boolean = False
        Private pLength As Long = 0

        Public Sub New(ByVal aFileName As String)
            'NOTE: default buffer size (4096) or larger degrades performance - jlk 10/2008
            pStreamReader = New IO.BinaryReader(New IO.BufferedStream(New IO.FileStream(aFileName, IO.FileMode.Open, IO.FileAccess.Read), 1024))
            Clear()
        End Sub

        Public Sub New(ByVal aStreamReader As IO.BinaryReader)
            pStreamReader = aStreamReader
            Clear()
        End Sub

        Private Sub Clear()
            If pStreamReader.BaseStream.CanSeek Then
                pLength = pStreamReader.BaseStream.Length
            End If
            pHaveNextChar = False
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
                    If pHaveNextChar Then 'Use already read next char
                        lChar = pNextChar
                        pHaveNextChar = False
                    Else                  'Read a new character
                        If pLength > 0 AndAlso pStreamReader.BaseStream.Position = pLength Then
                            GoTo AtEndOfStream
                        End If
                        lChar = pStreamReader.ReadChar
                    End If
                    Select Case lChar
                        Case ControlChars.Cr 'Found carriage return, consume linefeed if it is next
                            pNextChar = pStreamReader.ReadChar
                            If pNextChar <> vbLf Then pHaveNextChar = True 'Save next char if it was not LF
                        Case ControlChars.Lf 'Unix-style line ends without carriage return
                        Case Else 'Found a character that does not end the line
                            lSb.Append(lChar)
                            GoTo ReadCharacter
                    End Select
                    pCurrentLine = lSb.ToString
                    Logger.Progress(Math.Floor(pStreamReader.BaseStream.Position / pStreamReader.BaseStream.Length * 1000), 1000)
                    Return True
                Catch lEndOfStreamException As IO.EndOfStreamException
                    lEndOfStream = True
                End Try
                If lEndOfStream Then
AtEndOfStream:
                    Try
                        Logger.Dbg("Closing Stream")
                        pStreamReader.Close()
                    Catch
                    End Try
                    pStreamReader = Nothing
                    pCurrentLine = lSb.ToString
                End If
                Return (lSb.Length > 0)
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
    ''' An enumerable set of lines read from a text file (or other BinaryReader)
    ''' lines in file end with carriage return and/or linefeed
    ''' end of line characters are stripped from enumerated lines returned
    ''' </summary>
    Private Class clsLinesInFileReadLine
        Implements IEnumerable, IEnumerator, IDisposable

        Private pStreamReader As IO.StreamReader
        Private pCurrentLine As String
        Private pNextChar As Char = Nothing
        Private pHaveNextChar As Boolean = False
        Private pLength As Long = 0

        Public Sub New(ByVal aFileName As String)
            'NOTE: default buffer size (4096) or larger degrades performance - jlk 10/2008
            pStreamReader = New IO.StreamReader(New IO.BufferedStream(New IO.FileStream(aFileName, IO.FileMode.Open, IO.FileAccess.Read), 1024))
            Clear()
        End Sub

        Public Sub New(ByVal aStreamReader As IO.StreamReader)
            pStreamReader = aStreamReader
            Clear()
        End Sub

        Private Sub Clear()
            If pStreamReader.BaseStream.CanSeek Then
                pLength = pStreamReader.BaseStream.Length
            End If
            pHaveNextChar = False
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
                Dim lEndOfStream As Boolean = False
                Try
                    If pStreamReader.EndOfStream Then
                        Return False
                    Else
                        pCurrentLine = pStreamReader.ReadLine
                        If pCurrentLine Is Nothing Then
                            pCurrentLine = ""
                            Return False
                        End If
                        If pLength > 0 Then
                            Logger.Progress(pStreamReader.BaseStream.Position / pLength * 1000, 1000)
                        End If
                        Return True
                    End If
                Catch lEndOfStreamException As IO.EndOfStreamException
                    lEndOfStream = True
                End Try
                If lEndOfStream Then
                    Try
                        Logger.Dbg("Closing Stream")
                        pStreamReader.Close()
                    Catch
                    End Try
                    pStreamReader = Nothing
                    pCurrentLine = ""
                End If
                Return False
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
        Dim lChar As Char
        'TODO: test a StringBuilder in place of &= for each character
        Dim lNextLine As New StringBuilder
        Try
ReadCharacter:
            lChar = aReader.ReadChar
            Select Case lChar
                Case ControlChars.Cr 'Found carriage return, consume linefeed if it is next
                    If aReader.PeekChar = 10 Then aReader.ReadChar()
                Case ControlChars.Lf 'Unix-style line ends without carriage return
                Case Else 'Found a character that does not end the line
                    lNextLine.Append(lChar)
                    GoTo ReadCharacter
            End Select
        Catch lExEndofStream As IO.EndOfStreamException
            If lNextLine.Length = 0 Then 'We had nothing to read, already finished file last time
                Throw lExEndofStream
            Else
                'Reaching the end of file is fine, we are returning the last line now
            End If
        End Try
        Return lNextLine.ToString
    End Function

    ''' <summary>
    ''' Gets System.Double.NaN (not a number)
    ''' </summary>
    ''' <returns>Double.NaN</returns>
    ''' <remarks>workaround for mystery bug - program exit without message on first reference to NaN</remarks>
    Public Function GetNaN() As Double
        Static lFirst As Boolean = True
        Static lCodeWithCount As New atcCollection
        If lFirst Then Logger.Dbg("GetNaN")
        Dim lStatus As UInteger = _statusfp
        If lFirst Then
            Logger.Dbg("StatusB4 " & lStatus)
            _fpreset()
            Logger.Dbg("StatusAf " & _statusfp)
            lFirst = False
        ElseIf lStatus > 1 Then
            lCodeWithCount.Increment(lStatus, 1)
            Dim lCount As Integer = lCodeWithCount.ItemByKey(lStatus)
            If lCount < 10 OrElse lCount Mod 500 = 0 Then
                Logger.Dbg("StatusB4 " & lStatus & " Count " & lCount)
            End If
            _fpreset()
            End If
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
