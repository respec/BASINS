VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCoFeedback"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Private pLogger As clsATCoLogger
Private pUninstallFile As clsUninstallFile
Private pUninstallRegistryKeysAsText As String
Private pFiles As FastCollection
Private pText As String
Private pFilesNotFound As String
Private pUnInstallFilesFound As String
Private pEXEName As String
Private pAppDir As String
Private pWindowsSysDir As String
Private pWindowsDir As String
Private pLenWinSys As Long
Private pLenWin As Long
Private pLenApp As Long
Private pIncludeReference As Boolean
Private pReference As String
Private pWait As Boolean

Public Sub AddText(newText As String)
  pText = pText & newText & vbCrLf
End Sub

Public Function Text() As String
  Text = pText
End Function

Public Property Set Logger(aLogger As clsATCoLogger)
  Set pLogger = aLogger
End Property

Public Sub AddFile(newValue As String)
  Dim fName As String      'name of file being added
  Dim Index As String      'numeric portion of uninstall file name, "000" of unin000.dat
  Dim indexNext As String  'Next index as we look for additional files like  unin001.dat
  Dim fileCount As Long    'count of files in an uninstall file
  Dim vString As Variant   'variant of current string from uninstall file
  Dim lString As String    'current string from uninstall file
  Dim lType As String      'current type from uninstall file
  Dim lReg As String       'temp registry string
  Dim lkey As HKEYS
  
  On Error GoTo SkipFile:
  
  fName = newValue
  If Len(fName) > 4 Then
    If LCase(Right(fName, 4)) = ".pif" Then 'this is not found, use .lnk alias extension
      fName = Left(fName, Len(fName) - 4) & ".lnk"
    End If
  End If
  
  If FileExists(fName, True) Or _
    (FileExt(fName) = "dat" And Left(FilenameOnly(fName), 4) = "unin") Then 'may be missing first uns, try all jlk 7/2/3
  
    If Len(pAppDir) = 0 Then
      If FileExists(fName, True, False) Then
        pAppDir = fName
      Else
        pAppDir = PathNameOnly(fName)
      End If
    End If
    
    If FileExt(fName) = "dat" And Left(FilenameOnly(fName), 4) = "unin" Then
      Index = Mid(FilenameOnly(fName), 6, 3) ' probably "000"
      fileCount = 0
      While FileExists(fName) 'looking for all possible uninstall files
        pUnInstallFilesFound = pUnInstallFilesFound & vbCrLf & "  " & fName
        Try2AddFile fName
        pIncludeReference = True
        pReference = FilenameOnly(fName)
        pFilesNotFound = pFilesNotFound & " from " & fName & vbCrLf
        pUninstallRegistryKeysAsText = pUninstallRegistryKeysAsText & " from " & fName & vbCrLf
        
        Set pUninstallFile = New clsUninstallFile
        pUninstallFile.Name = fName
        For Each vString In pUninstallFile.Strings
          lString = vString
          lType = StrSplit(lString, vbTab, "'")
          If lType = "82" Or lType = "81" Then 'file (82) or directory (81)
            fileCount = fileCount + 1
            AddFile lString
          ElseIf lType > "82" Then 'registry keys
            If InStr(pUninstallRegistryKeysAsText, lString) = 0 Then 'only dump once
              If InStr(lString, "VB and VBA") Then
                lkey = HKEY_CURRENT_USER
              Else
                lkey = HKEY_LOCAL_MACHINE
              End If
              Dim cRegistry As New RegistryHandler
              pUninstallRegistryKeysAsText = pUninstallRegistryKeysAsText & _
                                               "  UninsType:" & lType & " Key:" & lString & _
                                               DumpRegistryTree(lkey, lString, cRegistry, 2) & vbCrLf
              Set cRegistry = Nothing
            End If
          End If
        Next
        Set pUninstallFile = Nothing
        
        If fileCount = 0 And Len(pFilesNotFound) > 0 Then 'nothing missing from this uninstall file
          pFilesNotFound = pFilesNotFound & "  <nothing missing>" & vbCrLf
        End If
        fileCount = 0
        ' more uninstall files?
        indexNext = Format(CInt(Index) + 1, "000")
        fName = ReplaceString(fName, Index, indexNext)
        Index = indexNext
        pIncludeReference = False
      Wend
    Else 'Just adding a normal file, not an uninsXXX.dat
      Try2AddFile fName
    End If
  Else
    pFilesNotFound = pFilesNotFound & "  " & fName & vbCrLf
  End If
  Exit Sub
SkipFile:
  pFilesNotFound = pFilesNotFound & "  " & fName & " skipped due to " & Err.Description & vbCrLf
End Sub

Private Sub Try2AddFile(f As String)
  Dim keyindex As Long
  Dim key As String
  
  key = LCase(FilenameNoPath(f))
  If pFiles.KeyExists(key) Then
    'Debug.Print "Duplicate key " & s
    If pIncludeReference Then
      If InStr(pFiles.item(key), pReference) = 0 Then 'only need ref once
        pFiles.item(key) = pFiles.item(key) & "," & pReference
      End If
    End If
  Else
    keyindex = 1
    While keyindex <= pFiles.Count
      If pFiles.key(keyindex) > key Then
        GoTo NextFile
      Else
        keyindex = keyindex + 1
      End If
    Wend
NextFile:
    If keyindex <= pFiles.Count Then 'add file in appropriate alphabetical location
      pFiles.Add f, key, keyindex
    Else 'add new file at end of collection
      pFiles.Add f, key
    End If
    If pIncludeReference Then
      pFiles.item(key) = pFiles.item(key) & " ref by " & pReference
    End If
  End If
End Sub

Public Sub Show(AppObject As Object, icon As Object)
  Dim vFile
  Dim i As Long
  
  With frmFeedback
    .Show
    .icon = icon
    .MousePointer = vbHourglass
    .fraButtons.Visible = False
    DoEvents 'Get the form up
    .lblStatus = "Collecting System Information"
    DoEvents
    pText = SystemInfo(AppObject) & pText
    
    If Len(pAppDir) > 0 Then
      pText = pText & FileInfo(pAppDir, True)
    End If
    
    .lblStatus = "Processing Uninstall Files"
    DoEvents
    
    If Len(pUnInstallFilesFound) > 0 Then
      pText = pText & "Uninstall Files Found:" & pUnInstallFilesFound & vbCrLf
    Else
      pText = pText & "No Uninstall Files Found!" & vbCrLf
    End If
    
    If Len(pFilesNotFound) > 0 Then
      pText = pText & vbCrLf & "Files Not Found:" _
                    & vbCrLf & pFilesNotFound & vbCrLf
    End If
    
    .lblStatus = "Processing Uninstall Registry Keys"
    DoEvents
    
    If Len(pUninstallRegistryKeysAsText) > 0 Then
      pText = pText & "Registry Keys Referenced In Uninstall Files:" _
                    & vbCrLf & pUninstallRegistryKeysAsText & vbCrLf
    End If
    
    If pFiles.Count > 0 Then
      i = 0
      For Each vFile In pFiles
        i = i + 1
        If (i - 1) Mod 50 = 0 Then
          .lblStatus = "Processing FileInfo for file " & i & " of " & pFiles.Count & " files"
          DoEvents
        End If
        pText = pText & FileInfo(CStr(vFile))
      Next
    End If
    
    If Len(pEXEName) > 0 Then
      .lblpleaseType.Caption = "Type a message to the " & pEXEName & " developers"
      .Caption = pEXEName & " Feedback"
    End If
    
    If Not pLogger Is Nothing Then
      pText = pText & vbCrLf & vbCrLf & "Event Log:" & vbCrLf & vbCrLf & pLogger.CurrentLog
    End If
    
    .fraButtons.Visible = True
    .lblStatus.Visible = False
    .setFeedback Me
    
    .MousePointer = vbDefault
  End With
      
  'If we have been asked to wait, then wait until frmFeedback clears Wait flag
  If Wait Then
    Do
      DoEvents
      Sleep 100
    Loop While pWait
    Wait = True 'Re-set the Wait flag in case this clsATCoFeedback will be reused
  End If
      
End Sub

'True if feedback should block when Show is called, waiting for the user before returning
Public Property Get Wait() As Boolean
  Wait = pWait
End Property
Public Property Let Wait(aWait As Boolean)
  pWait = aWait
End Property

Private Function SystemInfo(AppObject As Object) As String
  Dim retval As String
  Dim i As Long
  Dim lDrives As New Collection, vDrive As Variant
  Dim lDrive As String, lDriveStatus As Boolean, r As Long
  Dim lSectPerCluster As Long, lBytePerSector As Long, lNumFreeCluster As Long, lTotalCluster As Long
    
  On Error Resume Next
  
  pWindowsSysDir = GetWindowsSysDir
  pWindowsDir = GetWindowsDir
  'remove trailing \
  If Right(pWindowsSysDir, 1) = "\" Then pWindowsSysDir = Left(pWindowsSysDir, Len(pWindowsSysDir) - 1)
  If Right(pWindowsDir, 1) = "\" Then pWindowsDir = Left(pWindowsDir, Len(pWindowsDir) - 1)
  pLenWinSys = Len(pWindowsSysDir)
  pLenWin = Len(pWindowsDir)
  
  retval = ""
  retval = retval & "System Information:"
  retval = retval & vbCrLf & "  " & GetWinPlatform
  retval = retval & vbCrLf & "  Computer Name: " & APIComputerName
  retval = retval & vbCrLf & "  Current User: " & APIUserName
  retval = retval & vbCrLf & "  Current Directory: " & CurDir
  retval = retval & vbCrLf & "  Windows Directory {win}: " & pWindowsDir
  retval = retval & vbCrLf & "  Windows System {sys}: " & pWindowsSysDir
  retval = retval & vbCrLf & "  Temp Directory: " & GetTmpPath
  retval = retval & vbCrLf
  
  If IsNull(AppObject) Then
    retval = retval & vbCrLf & "App Object not set"
  Else
    If Len(AppObject.title) > 0 Then
      pEXEName = AppObject.title
    Else
      pEXEName = AppObject.exeName
    End If
    retval = retval & vbCrLf & "App.EXEName: " & AppObject.exeName
    retval = retval & vbCrLf & "  Path: " & AppObject.path
    retval = retval & vbCrLf & "  Title: " & AppObject.title
    retval = retval & vbCrLf & "  ProductName: " & AppObject.ProductName
    retval = retval & vbCrLf & "  Version: " & AppObject.Major & "." & AppObject.Minor & "." & AppObject.Revision
    retval = retval & vbCrLf
  End If
    
  retval = retval & vbCrLf & "Memory(mb):"
  retval = retval & vbCrLf & "  Total: " & Format(TotalMemory, "#,#00.00")
  retval = retval & vbCrLf & "  Available: " & Format(AvailableMemory, "#,#00.00")
  retval = retval & vbCrLf & "  TotalPhysical: " & Format(TotalPhysicalMemory, "#,#00.00")
  retval = retval & vbCrLf & "  AvailablePhysical: " & Format(AvailablePhysicalMemory, "#,#00.00")
  retval = retval & vbCrLf
  
  Dim cDiskSpace As clsDiskSpace
  retval = retval & vbCrLf & "Disk Type and Space(gb):"
  Set lDrives = GetLogicalDriveStringsAsCollection
  For Each vDrive In lDrives
    lDrive = vDrive
    retval = retval & vbCrLf & "  " & lDrive
    Set cDiskSpace = New clsDiskSpace
    With cDiskSpace
      lDriveStatus = .GetFreeSpace(lDrive)
      retval = retval & " Type:" & .DriveType
      If .DriveType = "FIXED" Then
        retval = retval & " Total:" & Format(.TotalSpace("gb"), "0.000") & ":" & _
                                  " Free:" & Format(.TotalFreeSpace("gb"), "0.000")
      End If
    End With
    Set cDiskSpace = Nothing
  Next
  retval = retval & vbCrLf
  
  retval = retval & vbCrLf & "RegistryKeys:"
  Dim cRegistry As New RegistryHandler
  Dim lRoot As String
  lRoot = "Software\AQUA TERRA Consultants"
  retval = retval & vbCrLf & Space(2) & lRoot
  retval = retval & DumpRegistryTree(HKEY_LOCAL_MACHINE, lRoot, cRegistry, 2)
  retval = retval & vbCrLf
  lRoot = "Software\VB and VBA Program Settings"
  retval = retval & vbCrLf & Space(2) & lRoot
  retval = retval & DumpRegistryTree(HKEY_CURRENT_USER, lRoot, cRegistry, 2)
  retval = retval & vbCrLf
  
  SystemInfo = retval & vbCrLf & "Application Status:" & vbCrLf & vbCrLf
    
End Function

Private Function FileInfo(FileName As String, Optional isAppDir As Boolean = False) As String
  Dim thisFileInfo As String
  Dim tmpstr As String
  Dim lFileName As String, lRef As Long
  
  On Error GoTo SomeError
  
  If isAppDir Then
    pAppDir = FileName
    If Right(pAppDir, 1) = "\" Then pAppDir = Left(pAppDir, Len(pAppDir) - 1)
    pLenApp = Len(pAppDir)
    thisFileInfo = "Application directory {app}: " & pAppDir & vbCrLf
  Else
    lFileName = FileName
    lRef = InStr(lFileName, "ref by")
    If lRef > 0 Then
      lFileName = Left(lFileName, lRef - 1)
    End If
    tmpstr = PathNameOnly(lFileName)
    If pLenWinSys > 0 And LCase(Left(tmpstr, pLenWinSys)) = LCase(pWindowsSysDir) Then
      tmpstr = "{sys}" & Mid(tmpstr, pLenWinSys + 1)
    ElseIf pLenWin > 0 And LCase(Left(tmpstr, pLenWin)) = LCase(pWindowsDir) Then
      tmpstr = "{win}" & Mid(tmpstr, pLenWin + 1)
    ElseIf pLenApp > 0 And LCase(Left(tmpstr, pLenApp)) = LCase(pAppDir) Then
      tmpstr = "{app}" & Mid(tmpstr, pLenApp + 1)
    End If
    thisFileInfo = FilenameNoPath(FileName) & " in " & tmpstr & vbCrLf
    tmpstr = GetFileVerString(lFileName)
    If Len(tmpstr) > 0 Then thisFileInfo = thisFileInfo & "  Version: " & tmpstr & vbCrLf
    tmpstr = Format(FileDateTime(lFileName), "MM/DD/YYYY hh:mm:ss am/pm")
    If Len(tmpstr) > 0 Then thisFileInfo = thisFileInfo & "  Date: " & tmpstr & vbCrLf
    tmpstr = FileLen(lFileName)
    thisFileInfo = thisFileInfo & "  Size: " & FileLen(lFileName) & vbCrLf
  End If
  FileInfo = thisFileInfo & vbCrLf
  Exit Function
SomeError:
  tmpstr = "(" & Err.Description & ")"
  Err.Clear
  Resume Next
End Function

Private Function DumpRegistryTree(aRootKey As HKEYS, _
                                  aRoot As String, _
                                  aRegistry As RegistryHandler, _
                                  aLevel As Long) As String
  Dim vKeys As Variant, vKey As Variant, vValues As Variant, vValue As Variant, sValue As String
  Dim retval As String
  With aRegistry
    
    vValues = .EnumValues(aRootKey, aRoot)
    If UBound(vValues) > 0 Then
      For Each vValue In vValues
        sValue = vValue
        If Len(sValue) < 2000 Then
          If Len(sValue) = 0 Then
            retval = retval & ":'" & .GetRegValue(aRootKey, aRoot, sValue) & "'"
          Else
            retval = retval & vbCrLf & Space(2 * aLevel)
            retval = retval & sValue & ":'" & .GetRegValue(aRootKey, aRoot, sValue) & "'"
          End If
        End If
      Next vValue
    End If
    
    vKeys = .EnumKeys(aRootKey, aRoot)
    If IsArray(vKeys) Then
      If UBound(vKeys) > 0 Then
        For Each vKey In vKeys
          If Len(vKey) < 2000 Then
            retval = retval & vbCrLf & Space(2 * aLevel) & vKey
            retval = retval & DumpRegistryTree(aRootKey, aRoot & "\" & vKey, aRegistry, aLevel + 1)
          End If
        Next vKey
      End If
    Else
      retval = retval & vbCrLf & Space(2 * aLevel) & vKey & " key NOT found"
    End If
  
  End With
  DumpRegistryTree = retval
End Function

Private Sub Class_Initialize()
  Set pFiles = New FastCollection
  pFiles.AllowUpdates = True
  pIncludeReference = False
  pUninstallRegistryKeysAsText = ""
End Sub
