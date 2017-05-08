Attribute VB_Name = "UtilFolder"
Option Explicit

Public Sub AddFilesInDir(aFilenames As FastCollection, aDirName As String, aSubdirs As Boolean, _
                         Optional aFileFilter As String = "*", Optional aAttributes As Long = 0)
  Dim dirsThisDir As FastCollection
  Dim fName As String
  Dim vName As Variant
  Dim key As String
  Dim OrigDir As String
  Dim FileMatches As Boolean
  
  OrigDir = CurDir
  ChDriveDir aDirName
  
  If aFilenames Is Nothing Then Set aFilenames = New FastCollection
  
  'get all matching file names in this dir before changing into a subdirectory
  fName = Dir(aFileFilter, aAttributes)
  While Len(fName) > 0
    FileMatches = False
    If aAttributes > 0 Then
      If GetAttr(fName) And aAttributes Then
        FileMatches = True
      End If
    ElseIf Not (GetAttr(fName) And vbDirectory) Then
      FileMatches = True
    End If
    
    If FileMatches Then
      fName = CurDir & "\" & fName
      key = LCase(fName)
      If aFilenames.KeyExists(key) Then
        Debug.Print "Already added " & fName
      Else
        aFilenames.Add fName, key
      End If
    End If
    fName = Dir
  Wend
  
  If aSubdirs Then   'find all the subdirectories in aDirName
    Set dirsThisDir = New FastCollection
    fName = Dir("*", vbDirectory)
    While Len(fName) > 0
      If GetAttr(fName) And vbDirectory Then
        If fName <> "." And fName <> ".." Then
          dirsThisDir.Add fName
        End If
      End If
      fName = Dir
    Wend
    
    For Each vName In dirsThisDir
      AddFilesInDir aFilenames, aDirName & "\" & vName, True, aFileFilter, aAttributes
    Next
    
  End If
    
  ChDriveDir OrigDir
End Sub

'Recursively remove all files and directories in aDirName
Public Sub RemoveFilesInDir(aDirName As String)
  Dim lOrigDir As String
  Dim lDirName As String
  
  If FileExists(aDirName, True, False) Then
    lOrigDir = CurDir
    ChDriveDir aDirName
      
    'get rid of files
    On Error Resume Next
    Kill "*"
    
    'get rid of directories
    lDirName = Dir("*", vbDirectory)
    While Len(lDirName) > 0
      If GetAttr(lDirName) And vbDirectory Then
        If lDirName <> "." And lDirName <> ".." Then
          RemoveFilesInDir lDirName
          RmDir lDirName
          lDirName = Dir("*", vbDirectory)
        Else
          lDirName = Dir
        End If
      End If
    Wend
    ChDriveDir lOrigDir
  End If
End Sub
