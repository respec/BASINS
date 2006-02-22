Option Strict Off
Option Explicit On 

Imports System.Collections.Specialized
Imports MapWinUtility

Public Module modFile
  Public Function ChDriveDir(ByVal newPath As String) As Boolean
    ' ##SUMMARY Changes directory and, if necessary, drive. Returns True if successful.
    ' ##PARAM newPath I New pathname.
    ' ##RETURNS True if directory change is successful.
    Try
      If FileExists(newPath, True, False) Then
        If Mid(newPath, 2, 1) = ":" Then ChDrive(newPath)
        ChDir(newPath)
        Return True
      Else
        Return False
      End If
    Catch e As Exception
      Return False
    End Try
  End Function

  Public Function SafeFilename(ByVal S As String, Optional ByVal ReplaceWith As String = "_") As String
    ' ##SUMMARY Converts, if necessary, non-printable characters in filename to printable alternative.
    ' ##PARAM S I Filename to be converted, if necessary.
    ' ##PARAM ReplaceWith I Character to replace non-printable characters in S (default="_").
    ' ##RETURNS Input parameter S with non-printable characters replaced with specific printable character (default="_").
    Dim retval As String 'return string
    Dim i As Short 'loop counter
    Dim strLen As Short 'length of string
    Dim ch As String 'individual character in filename

    strLen = Len(S)
    For i = 1 To strLen
      ch = Mid(S, i, 1)
      Select Case Asc(ch)
        Case 0 : GoTo EndFound
        Case Is < 32, 34, 42, 47, 58, 60, 62, 63, 92, 124, Is > 126 : retval &= ReplaceWith
        Case Else : retval &= ch
      End Select
    Next
EndFound:
    Return retval
  End Function

  Public Function FilenameOnly(ByVal istr As String) As String
    ' ##SUMMARY Converts full path, filename, and extension to filename only.
    ' ##SUMMARY   Example: FilenameOnly("C:\foo\bar.txt") = "bar"
    ' ##PARAM istr I Filename with path and extension.
    ' ##RETURNS Filename without directory path or extension.
    Return System.IO.Path.GetFileNameWithoutExtension(istr)
  End Function

  Public Function FilenameNoPath(ByVal istr As String) As String
    ' ##SUMMARY Converts full path, filename, and extension to only filename with extension.
    ' ##SUMMARY   Example: FilenameNoPath ("C:\foo\bar.txt") = "bar.txt"
    ' ##PARAM istr I Filename with path and extension.
    ' ##RETURNS Filename and extension without directory path.
    Return System.IO.Path.GetFileName(istr)
  End Function

  Public Function FileExt(ByVal istr As String) As String
    ' ##SUMMARY Reduces full path, filename, and extension to only extension.
    ' ##SUMMARY   Example: FileExt ("C:\foo\bar.txt") = "txt"
    ' ##PARAM istr I Filename with path and extension.
    ' ##RETURNS Extension without path or filename.
    Return Mid(System.IO.Path.GetExtension(istr), 2)
  End Function

  Public Function FilenameSetExt(ByVal istr As String, ByVal newExt As String) As String
    ' ##SUMMARY Converts extension of filename from existing to specified.
    ' ##SUMMARY   Example: FilenameSetExt ("C:\foo\bar.txt", "png") = "C:\foo\bar.png"
    ' ##SUMMARY   Example: FilenameSetExt ("C:\foo\bardtxt", "png") = "C:\foo\bardtxt.png"
    ' ##PARAM istr I Filename with path and extension.
    ' ##PARAM newExt I Extension to be added or to replace current extension.
    ' ##RETURNS Filename with new extension.
    Return System.IO.Path.ChangeExtension(istr, newExt)
  End Function

  Public Function AbsolutePath(ByVal filename As String, ByVal StartPath As String) As String
    ' ##SUMMARY Converts an relative pathname to an absolute path given the starting directory.
    ' ##SUMMARY   Example: AbsolutePath("..\Data\DataFile.wdm", "C:\BASINS\models") = "C:\BASINS\Data\DataFile.wdm"
    ' ##PARAM Filename I Relative file path and name.
    ' ##PARAM StartPath I Absolute starting directory from which relative path is traced.
    ' ##RETURNS Absolute path and filename.
    ' ##LOCAL slashposFilename - position of slash in filename.
    ' ##LOCAL slashposPath - position of slash in pathname.
    Dim slashposFilename As Integer
    Dim slashposPath As Integer

    If Mid(filename, 2, 1) = ":" Then 'Already have a path that starts with a drive letter
      Return filename
    End If

    If Right(StartPath, 1) = "\" Then StartPath = Left(StartPath, Len(StartPath) - 1)

    If UCase(Left(filename, 2)) <> UCase(Left(StartPath, 2)) Then filename = StartPath & "\" & filename

    slashposFilename = InStr(filename, "\..\")
    While slashposFilename > 0
      slashposPath = InStrRev(filename, "\", slashposFilename - 1)
      If slashposPath = 0 Then
        slashposFilename = 0
      Else
        filename = Left(filename, slashposPath) & Mid(filename, slashposFilename + 4)
        slashposFilename = InStr(filename, "\..\")
      End If
    End While
    Return filename
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
    If Right(StartPath, 1) = "\" Then StartPath = Left(StartPath, Len(StartPath) - 1)

    If Len(filename) > 2 Then
      If Left(filename, 3) = "..\" Then
        'Concatenate StartPath and Filename
        filename = StartPath & "\" & filename
      End If
    End If

    'Adjust path for Filename as necessary
    slashposFilename = InStr(filename, "\..\")
    While slashposFilename > 0
      slashposPath = InStrRev(filename, "\", slashposFilename - 1)
      If slashposPath = 0 Then
        slashposFilename = 0
      Else
        filename = Left(filename, slashposPath) & Mid(filename, slashposFilename + 4)
        slashposFilename = InStr(filename, "\..\")
      End If
    End While

    If InStr(filename, "\") = 0 Then
      'No path to check, so assume it is a file in StartPath
    ElseIf LCase(Left(filename, 2)) <> LCase(Left(StartPath, 2)) Then
      'filename is already relative or is on different drive
    Else
      'Reconcile StartPath and Filename
      slashposPath = Len(StartPath)
      If Mid(filename, slashposPath + 1, 1) = "\" Then 'Filename might include whole path
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
          slashposFilename = InStr(slashposPath + 1, filename, "\")
          slashposPath = InStr(slashposPath + 1, StartPath, "\")
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
      slashposPath = InStr(sameUntil, StartPath, "\")
      While slashposPath > 0
        filename = "..\" & filename
        slashposPath = InStr(slashposPath + 1, StartPath, "\")
      End While
    End If
    Return filename
  End Function

  Public Sub AddFilesInDir(ByRef aFilenames As NameValueCollection, ByVal aDirName As String, ByVal aSubdirs As Boolean, Optional ByVal aFileFilter As String = "*", Optional ByVal aAttributes As Integer = 0)
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
          fName = CurDir() & "\" & fName
          key = fName.ToLower
          'Try
          vName = aFilenames.Get(key)
          'If aFilenames.Get(key).Length > 0 Then
          'System.Diagnostics.Debug.WriteLine("Already added " & fName)
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
          AddFilesInDir(aFilenames, aDirName & "\" & fName, True, aFileFilter, aAttributes)
        Next fName

      End If
      ChDriveDir(OrigDir)
    End If
  End Sub

  'Recursively remove all files and directories in aDirName
  Public Sub RemoveFilesInDir(ByVal aDirName As String)
    System.IO.Directory.Delete(aDirName, True)
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
    MsgBox("Error writing '" & filename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OKOnly, "SaveFileBytes")
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
    MsgBox("Error writing '" & filename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OKOnly, "ReplaceStringToFile")
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
          'TODO: wait for newProcess to finish
        End If
      End If
    Catch ex As System.Exception
      MsgBox(ex.Message)
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
    Dim lFileName As String = Trim(aDefaultFileName)
    Dim lBaseFileName As String = System.IO.Path.GetFileName(lFileName) 'file name (with no path) of file we are looking for
    Dim lExePath As String
    Dim lDLLpath As String

    If (Len(aDefaultExt)) = 0 Then 'get extension from default name
      aDefaultExt = FileExt(aDefaultFileName)
    End If

    If (Len(aFileFilter)) = 0 Then 'get filter from default ext
      If Len(aDefaultExt) > 0 Then
        aFileFilter &= aDefaultExt & " Files (*." & aDefaultExt & ")|*." & aDefaultExt & "|"
      End If
      aFileFilter &= "All files (*.*)|*.*"
    End If

    On Error Resume Next

    If Right(lFileName, 1) = "\" Then
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
      If Not FileExists(lFileName, True) Then 'don't already know where it is, first look in registry
        lFileName = GetSetting("FindFile", "FoundFiles", lBaseFileName.ToLower, "")
        If lFileName.Length > 0 Then
          If Not FileExists(lFileName) Then 'delete bad name in registry
            DeleteSetting("FindFile", "FoundFiles", lBaseFileName.ToLower)
          End If
        End If
        'Look in another part of registry
        'If Not FileExists(lFileName) Then
        '  lRegistryFileName = pRegistry.RegGetString(HKEY_LOCAL_MACHINE, pLocalMachinePrefix & pAppName & "\" & pRegistrySection, pRegistryKey)
        'End If
      End If

      If Not FileExists(lFileName) AndAlso lBaseFileName.Length > 0 Then 'try some default locations if filename was specified, path was wrong or missing
        lFileName = aDefaultFileName
        If Not FileExists(lFileName) Then
          lExePath = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location).ToLower & "\"
          lDLLpath = PathNameOnly(System.Reflection.Assembly.GetExecutingAssembly.Location).ToLower & "\"

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
          If lFileName.Length = 0 AndAlso lExePath.IndexOf("\bin\") > 0 Then
            lFileName = FindRecursive(lBaseFileName, "c:\winnt\", "c:\windows\")
          End If

          If FileExists(lFileName) Then
            aDefaultFileName = lFileName
          Else
            lFileName = ""
          End If
        End If
      End If
      If aUserVerifyFileName OrElse Not FileExists(lFileName) Then 'ask the user
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
          Else 'Return empty string if user clicked Cancel
            lFileName = ""
          End If
        End With

        If FileExists(lFileName) Then
          SaveSetting("FindFile", "FoundFiles", lBaseFileName.ToLower, lFileName)
        End If
      End If

      If aChangeIntoDir Then
        lDir = System.IO.Path.GetDirectoryName(lFileName)
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
      If lStartDir.Length > 0 AndAlso Not lStartDir.EndsWith("\") Then lStartDir &= "\"
      If FileExists(lStartDir & aFilename) Then
        Return lStartDir & aFilename
      ElseIf FileExists(lStartDir, True, False) Then
        For Each lSubDir As String In System.IO.Directory.GetDirectories(lStartDir)
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

  Public Function WholeFileString(ByVal aFilename As String, Optional ByVal aTimeoutMilliseconds As Integer = 1000) As String
    ' ##SUMMARY Converts specified text file to a string.
    ' ##PARAM FileName I Name of text file
    ' ##RETURNS Returns contents of specified text file as string.
    Dim InFile As Integer
    Dim FileLength As Integer
    Dim TryUntil As Date = Now.AddMilliseconds(aTimeoutMilliseconds)
    ' ##LOCAL InFile - filenumber of text file
    ' ##LOCAL FileLength - length of text file contents

    InFile = FreeFile()
TryAgain:
    Try
      FileOpen(InFile, aFilename, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
      FileLength = LOF(InFile)
      WholeFileString = InputString(InFile, FileLength)
      FileClose(InFile)
    Catch ex As Exception
      If Now > TryUntil Then
        Logger.Msg("Error reading '" & aFilename & "'" & vbCr & vbCr & ex.Message, "WholeFileString - " & ex.GetType.Name)
        Return ""
      Else
        'MsgBox("WholeFileString error, trying again (" & ex.GetType.Name & ": " & ex.Message & ")")
        System.Threading.Thread.Sleep(50)
        GoTo TryAgain
      End If
    End Try
  End Function


  Public Function WholeFileBytes(ByVal filename As String) As Byte()
    ' ##SUMMARY Converts specified text file to Byte array
    ' ##PARAM FileName I Name of text file
    ' ##RETURNS Returns contents of specified text file in Byte array.
    Dim InFile As Short
    Dim FileLength As Integer
    Dim retval() As Byte
    ' ##LOCAL InFile - long filenumber of text file
    ' ##LOCAL retval() - byte array containing return values

    On Error GoTo ErrorReading

    InFile = FreeFile()
    FileOpen(InFile, filename, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
    FileLength = LOF(InFile)
    ReDim retval(FileLength - 1)
    FileGet(InFile, retval)
    FileClose(InFile)
    Return retval

ErrorReading:
    MsgBox("Error reading '" & filename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OKOnly, "WholeFileBytes")
  End Function

  Public Function FirstMismatch(ByVal filename1 As String, ByVal filename2 As String) As Integer
    ' ##SUMMARY Compares 2 files and locates first sequential byte that is different between files.
    ' ##PARAM filename1 I Name of first file
    ' ##PARAM filename2 I Name of second file
    ' ##RETURNS Returns byte position of first non-matching byte between two files: _
    'zero if they match, -1 if there was an error.
    Dim InFile1 As Short
    Dim FileLength1 As Integer
    Dim InFile2 As Short
    Dim FileLength2 As Integer
    Dim minLength As Integer
    Dim longBytes As Integer
    Dim testL1, testL2 As Integer
    Dim testB1, testB2 As Byte
    Dim i As Integer
    ' ##LOCAL InFile1 - file handle of first file
    ' ##LOCAL InFile2 - file handle of second file
    ' ##LOCAL FileLength1 - length of first file in bytes
    ' ##LOCAL FileLength2 - length of first file in bytes
    ' ##LOCAL i - byte index in files

    On Error GoTo ErrorReading

    If Not FileExists(filename1) Or Not FileExists(filename2) Then
      FirstMismatch = -1
    Else
      InFile1 = FreeFile()
      FileOpen(InFile1, filename1, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
      FileLength1 = LOF(InFile1)

      InFile2 = FreeFile()
      FileOpen(InFile2, filename2, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
      FileLength2 = LOF(InFile2)

      If FileLength1 < FileLength2 Then
        minLength = FileLength1
      Else
        minLength = FileLength2
      End If

      longBytes = minLength - minLength Mod 4

      For i = 1 To longBytes Step 4
        FileGet(InFile1, testL1)
        FileGet(InFile2, testL2)
        If testL1 <> testL2 Then Exit For
      Next

      Do While i <= minLength
        FileGet(InFile1, testB1, i)
        FileGet(InFile2, testB2, i)
        If testB1 <> testB2 Then Exit Do
        i = i + 1
      Loop

      If i <= minLength Then 'Found a mismatch before the shorter file ended
        FirstMismatch = i
      ElseIf FileLength1 <> FileLength2 Then  'Longer file matched shorter one while it lasted
        FirstMismatch = i
      Else
        FirstMismatch = 0
      End If

      FileClose(InFile1)
      FileClose(InFile2)
    End If
    Exit Function

ErrorReading:
    MsgBox("Error reading '" & filename1 & "'" & vbCr & "or '" & filename2 & "'" & vbCr & Err.Description, MsgBoxStyle.OKOnly, "WholeFileBytes")
    On Error Resume Next
    FileClose(InFile1)
    FileClose(InFile2)
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

    OrigBytes = System.BitConverter.GetBytes(n)
    ReDim NewBytes(3)
    NewBytes(0) = OrigBytes(3)
    NewBytes(1) = OrigBytes(2)
    NewBytes(2) = OrigBytes(1)
    NewBytes(3) = OrigBytes(0)
    Return System.BitConverter.ToInt32(NewBytes, 0)
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
    Dim reader As New System.IO.BinaryReader(inStream)

    'read in each byte and convert it to a char
    Dim numbytes As Integer = reader.BaseStream.Length
    SaveFileString(OutputFilePath, System.Convert.ToBase64String(reader.ReadBytes(numbytes)))

    reader.Close()

  End Sub

End Module
