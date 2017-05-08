Attribute VB_Name = "modUpdateWin32"
Option Explicit

Declare Sub CopyMemory Lib "Kernel32" Alias "RtlMoveMemory" (Dest As Any, Source As Any, ByVal nBytes As Long)
Declare Function GetCurrentProcessId Lib "Kernel32" () As Long
Declare Function GetExitCodeProcess Lib "Kernel32" (ByVal hProcess As Long, lpExitCode As Long) As Long
Declare Function OpenProcess Lib "Kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessId As Long) As Long
Declare Sub Sleep Lib "Kernel32" (ByVal dwMilliseconds As Long)

Private Declare Function GetFileVersionInfoSize Lib "version.dll" Alias "GetFileVersionInfoSizeA" (ByVal sFile As String, lpLen As Long) As Long
Private Declare Function GetFileVersionInfo Lib "version.dll" Alias "GetFileVersionInfoA" (ByVal sFile As String, ByVal lpIgnored As Long, ByVal lpSize As Long, ByVal lpBuf As Long) As Long
Private Declare Function GetSystemDirectory Lib "Kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
Private Declare Function GetTempFileNameA Lib "Kernel32" (ByVal lpszPath As String, ByVal lpPrefixString As String, ByVal wUnique As Long, ByVal lpTempFileName As String) As Long
Private Declare Function GetTempPath Lib "Kernel32" Alias "GetTempPathA" (ByVal nBufferLength As Long, ByVal lpBuffer As String) As Long
Private Declare Function GetWindowsDirectory Lib "Kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
Private Declare Function VerQueryValue Lib "version.dll" Alias "VerQueryValueA" (ByVal lpBuf As Long, ByVal szReceive As String, lpBufPtr As Long, lLen As Long) As Long


Private pWindowsSysDir As String
Private pWindowsDir As String
'Private pAppDir As String
Private pLenWinSys As Long
Private pLenWin As Long
'Private pLenApp As Long
Private Const gintMAX_SIZE% = 255 'Maximum buffer size

Private Type VERINFO                                            'Version FIXEDFILEINFO
    'There is data in the following two dwords, but it is for Windows internal
    '   use and we should ignore it
    Ignore(1 To 8) As Byte
    'Signature As Long
    'StrucVersion As Long
    FileVerPart2 As Integer
    FileVerPart1 As Integer
    FileVerPart4 As Integer
    FileVerPart3 As Integer
    ProductVerPart2 As Integer
    ProductVerPart1 As Integer
    ProductVerPart4 As Integer
    ProductVerPart3 As Integer
    FileFlagsMask As Long 'VersionFileFlags
    FileFlags As Long 'VersionFileFlags
    FileOS As Long 'VersionOperatingSystemTypes
    FileType As Long
    FileSubtype As Long 'VersionFileSubTypes
    'I've never seen any data in the following two dwords, so I'll ignore them
    Ignored(1 To 8) As Byte 'DateHighPart As Long, DateLowPart As Long
End Type

'Registry
Public Enum HKEY_TYPE
  HKEY_CLASSES_ROOT = &H80000000
  HKEY_CURRENT_CONFIG = &H80000005
  HKEY_CURRENT_USER = &H80000001
  HKEY_DYN_DATA = &H80000006
  HKEY_LOCAL_MACHINE = &H80000002
  HKEY_PERFORMANCE_DATA = &H80000004
  HKEY_USERS = &H80000003
End Enum

Private Const KEY_ALL_CLASSES = &HF0063
Private Const KEY_ALL_ACCESS = &H3F
Private Const STANDARD_RIGHTS_ALL = &H1F0000
Private Const READ_CONTROL = &H20000
Private Const KEY_QUERY_VALUE = &H1
Private Const KEY_ENUMERATE_SUB_KEYS = &H8
Private Const KEY_NOTIFY = &H10
Private Const SYNCHRONIZE = &H100000
Private Const KEY_READ = ((READ_CONTROL Or KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY) And (Not SYNCHRONIZE))
Private Const ERROR_SUCCESS = 0&

Private Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Long) As Long
Private Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" _
  (ByVal hKey As Long, ByVal lpSubKey As String, ByVal ulOptions As Long, ByVal samDesired As Long, phkResult As Long) As Long

Private Declare Function RegQueryValueEx Lib "advapi32.dll" Alias "RegQueryValueExA" _
  (ByVal hKey As Long, ByVal lpValueName As String, ByVal lpReserved As Long, lpType As Long, lpData As Any, lpcbData As Long) As Long

Private Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" _
  (ByVal hwnd As Long, _
   ByVal lpOperation As String, _
   ByVal lpFile As String, _
   ByVal lpParameters As String, _
   ByVal lpDirectory As String, _
   ByVal nShowCmd As Long) As Long

'-----------------------------------------------------------
' SUB: AddDirSep
' Add a trailing directory path separator (back slash) to the
' end of a pathname unless one already exists
'
' IN/OUT: [strPathName] - path to add separator to
'-----------------------------------------------------------
'
Public Sub AddDirSep(strPathName As String)
  strPathName = RTrim$(strPathName)
  If Right$(strPathName, 1) <> "/" Then
    If Right$(strPathName, 1) <> "\" Then strPathName = strPathName & "\"
  End If
End Sub

Public Sub OpenURL(URL As String)
  ShellExecute 0&, "open", URL, 0&, "", 1
End Sub

Public Function GetFileVerString(ByVal sFile As String) As String
  Const sEXE As String = "\"
  Dim lVerSize As Long
  Dim lTemp As Long
  Dim lRet As Long
  Dim bInfo() As Byte
  Dim lpBuffer As Long
  Dim sVer As VERINFO

  '
  'Get the size of the file version info, allocate a buffer for it, and get the
  'version info.  Next, we query the Fixed file info portion, where the internal
  'file version used by the Windows VerInstallFile API is kept.  We then copy
  'the fixed file info into a VERINFO structure.
  '
  lVerSize = GetFileVersionInfoSize(sFile, lTemp)
  ReDim bInfo(lVerSize)
  If lVerSize > 0 Then
    lRet = GetFileVersionInfo(sFile, lTemp, lVerSize, VarPtr(bInfo(0)))
    If lRet <> 0 Then
      lRet = VerQueryValue(VarPtr(bInfo(0)), sEXE, lpBuffer, lVerSize)
      If lRet <> 0 Then
        CopyMemory sVer, ByVal lpBuffer, lVerSize
        GetFileVerString = sVer.FileVerPart1 & "." _
                         & sVer.FileVerPart2 & "." _
                         & sVer.FileVerPart3 & "." _
                         & sVer.FileVerPart4
      End If
    End If
  End If
End Function

Public Function GetTmpFileName() As String
  Dim filename As String
  Dim path As String

  path = GetTmpPath
  filename = Space(Len(path) + 256)
  Call GetTempFileNameA(path, App.EXEName, &H0, filename)
  GetTmpFileName = Left(filename, InStr(filename, Chr$(0)) - 1)
End Function

Public Function GetTmpPath() As String
  Dim strFolder As String
  Dim lngResult As Long
  Dim MAX_PATH As Long
  MAX_PATH = 260
  strFolder = String(MAX_PATH, 0)
  lngResult = GetTempPath(MAX_PATH, strFolder)
  If lngResult <> 0 Then
      GetTmpPath = Left(strFolder, InStr(strFolder, Chr(0)) - 1)
  Else
      GetTmpPath = ""
  End If
End Function

Public Function StringFromBuffer(Buffer As String) As String
    Dim nPos As Long

    nPos = InStr(Buffer, vbNullChar)
    If nPos > 0 Then
        StringFromBuffer = Left$(Buffer, nPos - 1)
    Else
        StringFromBuffer = Buffer
    End If
End Function

Public Function GetWindowsDir() As String
  Dim strBuf As String

  strBuf = Space$(gintMAX_SIZE)
  '
  'Get the windows directory and then trim the buffer to the exact length
  'returned and add a dir sep (backslash) if the API didn't return one
  '
  If GetWindowsDirectory(strBuf, gintMAX_SIZE) Then
    GetWindowsDir = StringFromBuffer(strBuf)
    AddDirSep GetWindowsDir
  End If
End Function

Public Function GetWindowsSysDir() As String
    Dim strBuf As String

    strBuf = Space$(gintMAX_SIZE)
    '
    'Get the system directory and then trim the buffer to the exact length
    'returned and add a dir sep (backslash) if the API didn't return one
    '
    If GetSystemDirectory(strBuf, gintMAX_SIZE) Then
        GetWindowsSysDir = StringFromBuffer(strBuf)
        AddDirSep GetWindowsSysDir
    End If
End Function

Private Sub SetWinDirs()
  If pLenWinSys = 0 Then
    pWindowsSysDir = GetWindowsSysDir
    pWindowsDir = GetWindowsDir
    'remove trailing \
    If Right(pWindowsSysDir, 1) = "\" Then pWindowsSysDir = Left(pWindowsSysDir, Len(pWindowsSysDir) - 1)
    If Right(pWindowsDir, 1) = "\" Then pWindowsDir = Left(pWindowsDir, Len(pWindowsDir) - 1)
    pLenWinSys = Len(pWindowsSysDir)
    pLenWin = Len(pWindowsDir)
  End If
End Sub

Public Function ExpandWinSysNames(abbreviatedFilename As String, Optional appPath As String = "") As String
  Dim retval As String
  SetWinDirs
  retval = ReplaceString(abbreviatedFilename, "{sys}", pWindowsSysDir)
  retval = ReplaceString(retval, "{win}", pWindowsDir)
  If Len(appPath) > 0 Then
    If Right(appPath, 1) = "\" Then
      retval = ReplaceString(retval, "{app}", Left(appPath, Len(appPath) - 1))
    Else
      retval = ReplaceString(retval, "{app}", appPath)
    End If
  End If
  ExpandWinSysNames = retval
End Function

'Example:
'Username = Registry.RegGetString(HKEY_LOCAL_MACHINE, "\Network\Logon", "username")
'MachineName = Registry.RegGetString(HKEY_LOCAL_MACHINE, "System\CurrentControlSet\Control\ComputerName\ComputerName", "ComputerName")
Public Function RegGetString(keyhandle As HKEY_TYPE, ByVal section As String, ByVal key As String) As String
  Dim retval$, hSubKey As Long
  Dim dwType As Long
  Dim SZ As Long
  Dim r As Long, v As String
  Dim typeName As String
  
  'Debug.Print "ATCoRegistry:RegGetString(" & keyhandle & ", " & section & ", " & key & ")"
  
  retval = ""

  'MsgBox "RegGetString(" & section & ", " & key & ")"
  
  r = RegOpenKeyEx(keyhandle, section, 0, KEY_READ, hSubKey)
  If r <> ERROR_SUCCESS Then
    'MsgBox "RegOpenKeyEx(" & section & ") returned " & r, vbOKOnly, "ATCoRegistry RegGetString"
    GoTo Quit_Now
  End If
  SZ = 256
  v = String(SZ, 0)
  If Len(Trim(key)) = 0 Then key = vbNullString
  r = RegQueryValueEx(hSubKey, key, 0, dwType, ByVal v, SZ)
  
  'MsgBox "RegQueryValueEx(" & section & ", " & key & ") returned dwType=" & dwType & ", SZ=" & SZ & vbCr & "value=" & v
  
  If r <> ERROR_SUCCESS Then
    'MsgBox "RegGetString(" & section & ", " & key & ") failed with error " & r
    GoTo Quit_Now
  End If
'  Else
'    Select Case dwType
'      Case REG_BINARY:              typeName = "REG_BINARY"
'      Case REG_DWORD:               typeName = "REG_DWORD"
'      Case REG_DWORD_BIG_ENDIAN:    typeName = "REG_DWORD_BIG_ENDIAN"
'      Case REG_DWORD_LITTLE_ENDIAN: typeName = "REG_DWORD_LITTLE_ENDIAN"
'      Case REG_EXPAND_SZ:           typeName = "REG_EXPAND_SZ"
'      Case REG_LINK:                typeName = "REG_LINK"
'      Case REG_MULTI_SZ:            typeName = "REG_MULTI_SZ"
'      Case REG_NONE:                typeName = "REG_NONE"
'      Case REG_RESOURCE_LIST:       typeName = "REG_RESOURCE_LIST"
'      Case REG_SZ:                  typeName = "REG_SZ"
'    End Select
'    MsgBox "RegGetString(" & section & ", " & key & ") returned a value of type " & typeName & "(" & dwType & ")", vbOKOnly, "ATCoRegistry RegGetString"
  If dwType = 1 Then retval = Left(v, SZ - 1)
'  End If
  If keyhandle = 0 Then r = RegCloseKey(hSubKey)
Quit_Now:
  RegGetString = retval
End Function

