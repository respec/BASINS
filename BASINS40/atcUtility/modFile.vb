Option Strict Off
Option Explicit On 

Imports System.Collections.Specialized

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

  Public Function FilenameOnly(ByRef istr As String) As String
    ' ##SUMMARY Converts full path, filename, and extension to filename only.
    ' ##SUMMARY   Example: FilenameOnly("C:\foo\bar.txt") = "bar"
    ' ##PARAM istr I Filename with path and extension.
    ' ##RETURNS Filename without directory path or extension.
    Return System.IO.Path.GetFileNameWithoutExtension(istr)
  End Function

  Public Function FilenameNoPath(ByRef istr As String) As String
    ' ##SUMMARY Converts full path, filename, and extension to only filename with extension.
    ' ##SUMMARY   Example: FilenameNoPath ("C:\foo\bar.txt") = "bar.txt"
    ' ##PARAM istr I Filename with path and extension.
    ' ##RETURNS Filename and extension without directory path.
    Return System.IO.Path.GetFileName(istr)
  End Function

  Public Function FilenameNoExt(ByRef istr As String) As String
    ' ##SUMMARY Converts full path, filename, and extension to only path and filename without extension.
    ' ##SUMMARY   Example: FilenameNoExt ("C:\foo\bar.txt") = "C:\foo\bar"
    ' ##PARAM istr I Filename with path and extension.
    ' ##RETURNS Path and filename without extension.
    Dim dirname As String
    dirname = System.IO.Path.GetDirectoryName(istr)
    If dirname.Length > 0 Then dirname = dirname & System.IO.Path.DirectorySeparatorChar
    Return dirname & System.IO.Path.GetFileNameWithoutExtension(istr)
  End Function

  Public Function FileExt(ByRef istr As String) As String
    ' ##SUMMARY Reduces full path, filename, and extension to only extension.
    ' ##SUMMARY   Example: FileExt ("C:\foo\bar.txt") = "txt"
    ' ##PARAM istr I Filename with path and extension.
    ' ##RETURNS Extension without path or filename.
    Return Mid(System.IO.Path.GetExtension(istr), 2)
  End Function

  Public Function PathNameOnly(ByRef istr As String) As String
    ' ##SUMMARY Reduces full path, filename, and extension to only path.
    ' ##SUMMARY   Example: PathNameOnly ("C:\foo\bar.txt") = "C:\foo"
    ' ##PARAM istr I Filename with path and extension.
    ' ##RETURNS Directory path without filename or extension.
    Try
      Return System.IO.Path.GetDirectoryName(istr)
    Catch e As Exception
      Return ""
    End Try
  End Function

  Public Function FilenameSetExt(ByRef istr As String, ByRef newExt As String) As String
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

  Public Sub MkDirPath(ByVal newPath As String)
    ' ##SUMMARY Makes the specified directory and any above it that are not yet there.
    ' ##SUMMARY   Example: MkDirPath("C:\foo\bar") creates the "C:\foo" and "C:\foo\bar" directories if they do not already exist.
    ' ##PARAM newPath I Path to specified directory
    If Len(newPath) > 0 Then
      System.IO.Directory.CreateDirectory(newPath)
    End If
  End Sub

  Public Sub AddFilesInDir(ByRef aFilenames As NameValueCollection, ByRef aDirName As String, ByRef aSubdirs As Boolean, Optional ByRef aFileFilter As String = "*", Optional ByRef aAttributes As Integer = 0)
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
  Public Sub RemoveFilesInDir(ByRef aDirName As String)
    System.IO.Directory.Delete(aDirName, True)
  End Sub

  Public Sub SaveFileString(ByRef filename As String, ByRef FileContents As String)
    ' ##SUMMARY Saves incoming string to a text file.
    ' ##PARAM FileName I Name of output text file
    ' ##PARAM FileContents I Incoming string to be saved to file
    Dim OutFile As Short
    ' ##LOCAL OutFile - integer filenumber of output text file

    On Error GoTo ErrorWriting

    MkDirPath(PathNameOnly(filename))

    OutFile = FreeFile()
    FileOpen(OutFile, filename, OpenMode.Output)
    Print(OutFile, FileContents)
    FileClose(OutFile)
    Exit Sub

ErrorWriting:
    MsgBox("Error writing '" & filename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OKOnly, "SaveFileString")
  End Sub

  Public Sub SaveFileBytes(ByRef filename As String, ByRef FileContents() As Byte)
    ' ##SUMMARY Saves incoming Byte array to a text file.
    ' ##PARAM FileName I Name of output text file
    ' ##PARAM FileContents I Incoming Byte array to be saved to file
    Dim OutFile As Short
    ' ##LOCAL OutFile - integer filenumber of output text file

    On Error GoTo ErrorWriting

    MkDirPath(PathNameOnly(filename))

    OutFile = FreeFile()
    FileOpen(OutFile, filename, OpenMode.Binary)
    FilePut(OutFile, FileContents)
    FileClose(OutFile)
    Exit Sub

ErrorWriting:
    MsgBox("Error writing '" & filename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OKOnly, "SaveFileBytes")
  End Sub

  Public Sub AppendFileString(ByRef filename As String, ByRef appendString As String)
    ' ##SUMMARY Appends incoming string to existing text file.
    ' ##PARAM FileName I Name of existing text file
    ' ##PARAM appendString I Incoming string to be appended
    Dim OutFile As Short
    ' ##LOCAL OutFile - integer filenumber of existing text file

    On Error GoTo ErrorWriting

    MkDirPath(PathNameOnly(filename))

    OutFile = FreeFile()
    FileOpen(OutFile, filename, OpenMode.Append)
    Print(OutFile, appendString)
    FileClose(OutFile)
    Exit Sub

ErrorWriting:
    MsgBox("Error writing '" & filename & "'" & vbCr & vbCr & Err.Description, MsgBoxStyle.OKOnly, "AppendFileString")
  End Sub
  Public Sub ReplaceStringToFile(ByRef Source As String, ByRef Find As String, ByRef ReplaceWith As String, ByRef filename As String)
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
        FileOpen(OutFile, filename, OpenMode.Output)
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
  Public Function FileExists(ByVal PathName As String, Optional ByRef AcceptDirectories As Boolean = False, Optional ByRef AcceptFiles As Boolean = True) As Boolean
    ' ##SUMMARY Checks to see if specified file exists.
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
                  Optional ByVal aChDir2FileDir As Boolean = False, _
                  Optional ByRef aFilterIndex As Integer = 1) As String
    Dim lDir As String
    Dim baseFileName As String 'file name (with no path) of file we are looking for
    Dim lFileName As String
    Dim lRegistryFileName As String
    Dim lFileNameFoundInRegistry As Boolean
    Dim LookingForDir As Boolean

    If (Len(aDefaultExt)) = 0 Then 'try to get from default name
      aDefaultExt = FileExt(aDefaultFileName)
    End If

    If (Len(aFileFilter)) = 0 Then 'try to get from default ext
      If Len(aDefaultExt) > 0 Then
        aFileFilter &= aDefaultExt & " Files (*." & aDefaultExt & ")|*." & aDefaultExt & "|"
      End If
      aFileFilter &= "All files (*.*)|*.*"
    End If

    lDir = CurDir()
    lFileName = Trim(aDefaultFileName)

    On Error Resume Next

    If Right(lFileName, 1) = "\" Then
      LookingForDir = True
      If Len(lFileName) = 1 Then lFileName = ""
    End If

    If Not FileExists(lFileName, True) Then 'don't already know where it is, first look in registry
      'If Len(pRegistrySection) > 0 Then
      'lRegistryFileName = GetSetting(pAppName, pRegistrySection, pRegistryKey, "")
      'If Not FileExists(lRegistryFileName, True) Then
      'lRegistryFileName = pRegistry.RegGetString(HKEY_LOCAL_MACHINE, pLocalMachinePrefix & pAppName & "\" & pRegistrySection, pRegistryKey)
      'End If
      'End If
      If Len(lRegistryFileName) > 0 Then
        If FileExists(lRegistryFileName, True) Then 'got from registry
          lFileName = lRegistryFileName
          lFileNameFoundInRegistry = True
          If aChDir2FileDir Then
            ChDriveDir(System.IO.Path.GetFileName(lRegistryFileName))
          End If
        Else 'bad name in registry, message to user needed?
        End If
      End If
    End If

    If Not FileExists(lFileName, True) Then 'try some default locations if filename was specified, but not path
      If LookingForDir Then
        baseFileName = System.IO.Path.GetFileName(Left(lFileName, Len(lFileName) - 1))
      Else
        baseFileName = System.IO.Path.GetFileName(lFileName)
      End If
      If Len(baseFileName) > 0 Then
        lFileName = aDefaultFileName
        If Not FileExists(lFileName, True) Then 'Look in directory containing current .exe
          lFileName = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location) & "\" & baseFileName
        End If
        If Not FileExists(lFileName, True) Then 'Look in directory containing current plugin
          lFileName = PathNameOnly(System.Reflection.Assembly.GetExecutingAssembly.Location) & "\" & baseFileName
        End If
        If Not FileExists(lFileName, True) Then lFileName = "c:\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "c:\winnt\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "c:\winnt\system\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "c:\winnt\system32\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "c:\windows\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "c:\windows\system\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "c:\windows\system32\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "c:\BASINS\models\HSPF\bin\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "c:\BASINS\models\HSPF\WDMUtil\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "c:\BASINS\etc\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "d:\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "d:\BASINS\models\HSPF\bin\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "d:\BASINS\models\HSPF\WDMUtil\" & baseFileName
        If Not FileExists(lFileName, True) Then lFileName = "d:\BASINS\etc\" & baseFileName
        If FileExists(lFileName, True) Then
          aDefaultFileName = lFileName
        Else
          lFileName = ""
        End If
      End If
    End If

    If Not FileExists(lFileName, True) Or aUserVerifyFileName Then 'ask the user
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
          'If lFileName <> lRegistryFileName Then 'try to force registry update
          '    lFileNameFoundInRegistry = False
          'End If
        Else 'Return empty string if user clicked Cancel
          lFileName = ""
        End If
      End With

      If LookingForDir Then
        lFileName = PathNameOnly(lFileName)
      End If

      'If Not lFileNameFoundInRegistry Then 'try to add the key to the registry
      '    If Len(pRegistrySection) > 0 Then
      '        SaveSetting(pAppName, pRegistrySection, pRegistryKey, lFileName)
      '    End If
      'End If
    End If

    If Not aChDir2FileDir Or Len(lFileName) = 0 Then
      ChDriveDir(lDir)
    End If

    Return lFileName

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
End Module
