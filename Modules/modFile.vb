Public Module modFile

    ''' <summary>
    ''' Tests for existence of a specified file or directory
    ''' </summary>
    ''' <param name="aPathName">file or directory name to check for</param>
    ''' <param name="aAcceptDirectories">True to look for a directory matching PathName</param>
    ''' <param name="aAcceptFiles">True to look for a file matching PathName</param>
    ''' <returns>True if the specified file or directory exists, False if it does not</returns>
    ''' <remarks>AcceptDirectories and AcceptFiles can both be true to check for a file or directory at the same time.</remarks>
    Public Function FileExists(ByVal aPathName As String, _
                      Optional ByVal aAcceptDirectories As Boolean = False, _
                      Optional ByVal aAcceptFiles As Boolean = True) As Boolean
        Try
            If aPathName IsNot Nothing Then
                If aAcceptFiles AndAlso IO.File.Exists(aPathName) Then Return True
                If aAcceptDirectories AndAlso IO.Directory.Exists(aPathName) Then Return True
            End If
        Catch ex As Exception 'If there was an exception, assume that means the file does not exist
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Converts full path, filename, and extension to only path and filename without extension
    ''' </summary>
    ''' <param name="aFilename">Complete file name</param>
    ''' <returns>path and file name without extension</returns>
    ''' <remarks>Example: FilenameNoExt ("C:\foo\bar.txt") = "C:\foo\bar"</remarks>
    Public Function FilenameNoExt(ByVal aFilename As String) As String
        Return IO.Path.Combine(IO.Path.GetDirectoryName(aFilename), _
                               IO.Path.GetFileNameWithoutExtension(aFilename))
    End Function

    ''' <summary>
    ''' Compare two files and return True if contents are identical.
    ''' </summary>
    ''' <param name="aFilename1">Name of first file to compare</param>
    ''' <param name="aFilename2">Name of second file to compare</param>
    ''' <returns>true if files are identical, false if not</returns>
    ''' <remarks>An exception will occur if either file cannot be read.</remarks>
    Public Function FilesMatch(ByVal aFilename1 As String, ByVal aFilename2 As String) As Boolean
        Dim lFileLength As Long = New IO.FileInfo(aFilename1).Length

        'If files are not the same size, they do not match
        If New IO.FileInfo(aFilename2).Length <> lFileLength Then Return False

        Dim lBinaryReader1 As New IO.BinaryReader(New IO.FileStream(aFilename1, IO.FileMode.Open, IO.FileAccess.Read))
        Dim lBinaryReader2 As New IO.BinaryReader(New IO.FileStream(aFilename2, IO.FileMode.Open, IO.FileAccess.Read))
        For lIndex As Long = 1 To lFileLength
            If lBinaryReader1.ReadByte <> lBinaryReader2.ReadByte Then
                Return False
            End If
        Next
        lBinaryReader1.Close()
        lBinaryReader2.Close()
        Return True 'Reached the end and found no mismatches
    End Function

    ''' <summary>
    ''' Create the specified directory and any above it that are not yet there.
    ''' </summary>
    ''' <param name="aNewDirectory">new directory to create</param>
    ''' <remarks>Example: MkDirPath("C:\foo\bar") creates the "C:\foo" and "C:\foo\bar" directories if they do not already exist.</remarks>
    Public Sub MkDirPath(ByVal aNewDirectory As String)
        If Not aNewDirectory Is Nothing AndAlso aNewDirectory.Length > 0 AndAlso Not FileExists(aNewDirectory, True) Then
            System.IO.Directory.CreateDirectory(aNewDirectory)
        End If
    End Sub

    ''' <summary>
    ''' Reduces full path, filename, and extension to only path
    ''' </summary>
    ''' <param name="aFilename">path and file name</param>
    ''' <returns>path portion of aFilename</returns>
    ''' <remarks>Example: PathNameOnly ("C:\foo\bar.txt") = "C:\foo"</remarks>
    Public Function PathNameOnly(ByRef aFilename As String) As String
        ' ##SUMMARY Reduces full path, filename, and extension to only path.
        ' ##SUMMARY   Example: PathNameOnly ("C:\foo\bar.txt") = "C:\foo"
        ' ##PARAM istr I Filename with path and extension.
        ' ##RETURNS Directory path without filename or extension.
        Try
            Return System.IO.Path.GetDirectoryName(aFilename)
        Catch e As Exception
            Return ""
        End Try
    End Function

    Public Function TryDelete(ByVal aPath As String) As Boolean
        Return TryDelete(aPath, False)
    End Function

    ''' <summary>
    ''' Try to delete a file or directory. 
    ''' If it cannot not be deleted, log a message instead of throwing an exception
    ''' </summary>
    ''' <param name="aPath">Full path of file or directory to be deleted</param>
    ''' <returns>
    ''' True if aPath was deleted without error or did not exist
    ''' False if there was an exception while trying to delete
    ''' </returns>
    ''' <remarks>
    ''' If aPath is a directory, all the contents are deleted too.
    ''' Helpful for non-critical cleanup of temporary files that may be locked
    ''' </remarks>
    Public Function TryDelete(ByVal aPath As String, ByVal aVerbose As Boolean) As Boolean
        Dim lTryDelete As Boolean = False
        Try
            System.GC.WaitForPendingFinalizers() 'In case something was about to give up a file handle
            If FileExists(aPath) Then
                IO.File.Delete(aPath)
                If aVerbose Then Logger.Dbg("Deleted file '" & aPath & "'")
            ElseIf FileExists(aPath, True, False) Then
                IO.Directory.Delete(aPath, True)
                If aVerbose Then Logger.Dbg("Deleted directory '" & aPath & "'")
            Else
                If aVerbose Then Logger.Dbg("Path not found, no need to delete '" & aPath & "' (CurDir '" & CurDir() & "')")
            End If
            lTryDelete = True
        Catch ex As Exception
            If aVerbose Then Logger.Dbg("Exception deleting '" & aPath & "': " & ex.Message)
        End Try
        Return lTryDelete
    End Function

    Public Function TryCopy(ByVal aFromPath As String, ByVal aToPath As String) As Boolean
        Return TryCopy(aFromPath, aToPath, False)
    End Function

    ''' <summary>
    ''' Try copying file from aFromPath to aToPath
    ''' If it cannot not be copied, log a message instead of throwing an exception
    ''' </summary>
    ''' <param name="aFromPath">Path of existing file</param>
    ''' <param name="aToPath">Location for new copy of file</param>
    ''' <returns>True if file was copied, False if file did not exist or could not be copied</returns>
    ''' <remarks></remarks>
    Public Function TryCopy(ByVal aFromPath As String, ByVal aToPath As String, ByVal aVerbose As Boolean) As Boolean
        Try
            If IO.File.Exists(aFromPath) Then
                If IO.File.Exists(aToPath) Then TryDelete(aToPath)
                Dim lDirectory As String = IO.Path.GetDirectoryName(aToPath)
                If lDirectory.Length > 3 AndAlso Not IO.Directory.Exists(lDirectory) Then
                    IO.Directory.CreateDirectory(lDirectory)
                End If
                IO.File.Copy(aFromPath, aToPath)
                Return True
            End If
        Catch ex As Exception
            If aVerbose Then Logger.Dbg("Exception copying '" & aFromPath & "' to '" & aToPath & "': " & ex.Message)
        End Try
        Return False
    End Function
End Module
