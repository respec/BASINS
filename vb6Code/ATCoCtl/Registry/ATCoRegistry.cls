VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoRegistry"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

' Paths saved in the registry during installation are saved as the default value of a key such as
' HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\WinHSPF\DocPath
' The value is a directory name such as
' C:\Program Files\WinHSPF\doc
' with no trailing "\" and no file name.
' This sort of value can be retrieved by a call like
' App.HelpFile = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\WinHSPF\DocPath", "") & "\WinHSPF.chm"

Private Type FILETIME
  dwLowDateTime As Long
  dwHighDateTime As Long
End Type

Private Declare Function RegEnumKeyEx Lib "advapi32.dll" Alias "RegEnumKeyExA" _
  (ByVal hKey As Long, ByVal dwIndex As Long, ByVal lpName As String, lpcbName As Long, ByVal lpReserved As Long, ByVal lpClass As String, lpcbClass As Long, lpftLastWriteTime As FILETIME) As Long

Private Declare Function RegDeleteKey Lib "advapi32.dll" Alias "RegDeleteKeyA" _
  (ByVal hKey As Long, ByVal lpSubKey As String) As Long
Private Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" _
  (ByVal hKey As Long, ByVal lpValueName As String) As Long

Private Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Long) As Long
Private Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" _
  (ByVal hKey As Long, ByVal lpSubKey As String, ByVal ulOptions As Long, ByVal samDesired As Long, phkResult As Long) As Long

Private Declare Function RegQueryValueEx Lib "advapi32.dll" Alias "RegQueryValueExA" _
  (ByVal hKey As Long, ByVal lpValueName As String, ByVal lpReserved As Long, lpType As Long, lpData As Any, lpcbData As Long) As Long

Private Declare Function RegCreateKeyEx Lib "advapi32.dll" Alias "RegCreateKeyExA" _
  (ByVal hKey As Long, ByVal lpSubKey As String, ByVal Reserved As Long, ByVal lpClass As String, ByVal dwOptions As Long, ByVal samDesired As Long, lpSecurityAttributes As Long, phkResult As Long, lpdwDisposition As Long) As Long
Private Declare Function RegCreateKey Lib "advapi32.dll" Alias "RegCreateKeyA" _
  (ByVal hKey As Long, ByVal lpSubKey As String, keyhandle As Long) As Long

Private Declare Function RegSetValue Lib "advapi32.dll" Alias "RegSetValueA" _
  (ByVal hKey As Long, ByVal lpszSubKey As String, ByVal fdwType As Long, ByVal lpszValue As String, ByVal dwLength As Long) As Long
Private Declare Function RegSetValueEx Lib "advapi32.dll" Alias "RegSetValueExA" _
  (ByVal hKey As Long, ByVal lpValueName As String, ByVal Reserved As Long, ByVal dwType As Long, lpData As Byte, ByVal cbData As Long) As Long

Private Declare Function RegSetValueExString Lib "advapi32.dll" Alias "RegSetValueExA" _
  (ByVal hKey As Long, ByVal lpValueName As String, ByVal Reserved As Long, ByVal dwType As Long, ByVal lpValue As String, ByVal cbData As Long) As Long
Private Declare Function RegSetValueExLong Lib "advapi32.dll" Alias "RegSetValueExA" _
  (ByVal hKey As Long, ByVal lpValueName As String, ByVal Reserved As Long, ByVal dwType As Long, lpValue As Long, ByVal cbData As Long) As Long

'Private Const HKEY_CLASSES_ROOT = &H80000000
'Private Const HKEY_CURRENT_CONFIG = &H80000005
'Private Const HKEY_CURRENT_USER = &H80000001
'Private Const HKEY_DYN_DATA = &H80000006
'Private Const HKEY_LOCAL_MACHINE = &H80000002
'Private Const HKEY_PERFORMANCE_DATA = &H80000004
'Private Const HKEY_USERS = &H80000003

Private Enum HKEY_DATATYPE
  REG_BINARY = 3
  REG_DWORD = 4
  REG_DWORD_BIG_ENDIAN = 5
  REG_DWORD_LITTLE_ENDIAN = 4
  REG_EXPAND_SZ = 2
  REG_LINK = 6
  REG_MULTI_SZ = 7
  REG_NONE = 0
  REG_RESOURCE_LIST = 8
  REG_SZ = 1
End Enum

Public Enum HKEY_TYPE
  HKEY_CLASSES_ROOT = &H80000000
  HKEY_CURRENT_CONFIG = &H80000005
  HKEY_CURRENT_USER = &H80000001
  HKEY_DYN_DATA = &H80000006
  HKEY_LOCAL_MACHINE = &H80000002
  HKEY_PERFORMANCE_DATA = &H80000004
  HKEY_USERS = &H80000003
End Enum

Private Const REG_OPTION_NON_VOLATILE = &O0

Private Const KEY_ALL_CLASSES = &HF0063
Private Const KEY_ALL_ACCESS = &H3F
Private Const STANDARD_RIGHTS_ALL = &H1F0000
Private Const READ_CONTROL = &H20000
Private Const KEY_QUERY_VALUE = &H1
Private Const KEY_ENUMERATE_SUB_KEYS = &H8
Private Const KEY_NOTIFY = &H10
Private Const SYNCHRONIZE = &H100000
Private Const KEY_READ = ((READ_CONTROL Or KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY) And (Not SYNCHRONIZE))

'Private Const REG_SZ As Long = 1
Private Const MAX_PATH = 256&

' Return codes from Registration functions.
Private Const ERROR_SUCCESS = 0&
Private Const ERROR_BADDB = 1&
Private Const ERROR_BADKEY = 2&
Private Const ERROR_CANTOPEN = 3&
Private Const ERROR_CANTREAD = 4&
Private Const ERROR_CANTWRITE = 5&
Private Const ERROR_OUTOFMEMORY = 6&
Private Const ERROR_INVALID_PARAMETER = 7&
Private Const ERROR_ACCESS_DENIED = 8&

'Names of return codes
Private Reg_Error() As String
Private pGlobalPrefix As String
Private pAppName As String
'Defaults to App.EXEName
'for GlobalValue, next level within GlobalPrefix, above section
'for UserValue, key within HKEY_CURRENT_USER\Software\VB and VBA Applications
Public Property Get AppName() As String
  AppName = pAppName
End Property

Public Property Let AppName(NewValue As String)
  pAppName = NewValue
End Property

'Location in HKEY_LOCAL_MACHINE to put global values, default="Software\AQUA TERRA Consultants\"
Public Property Get GlobalPrefix() As String
  GlobalPrefix = pGlobalPrefix
End Property

Public Property Let GlobalPrefix(NewValue As String)
  pGlobalPrefix = NewValue
End Property

'Per-user settings such as recent files, window sizes
Public Property Get UserValue(section As String, key As String) As Variant
  UserValue = GetSetting(pAppName, section, key, "")
End Property

Public Property Let UserValue(section As String, key As String, NewValue As Variant)
  SaveSetting pAppName, section, key, NewValue
End Property

'System-wide values such as registered plugins, exe paths
Public Property Get GlobalValue(section As String, key As String) As String
  GlobalValue = RegGetString(HKEY_LOCAL_MACHINE, GlobalPrefix & pAppName & "\" & section, key)
End Property

Public Property Let GlobalValue(section As String, key As String, NewValue As String)
  Dim hNewKey As Long
  Dim lRetVal As Long, disposition As Long
  Dim KeyPath$
  
  KeyPath = GlobalPrefix & pAppName
  If section <> "" Then KeyPath = KeyPath & "\" & section
  lRetVal = RegCreateKeyEx(HKEY_LOCAL_MACHINE, KeyPath, 0&, vbNullString, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, 0&, hNewKey, disposition)
  'add avlue
  If lRetVal = ERROR_SUCCESS Then
    lRetVal = RegSetValue(hNewKey, key, REG_SZ, NewValue, Len(NewValue))
    RegCloseKey (hNewKey)
  End If
  If lRetVal <> ERROR_SUCCESS Then
    If lRetVal >= 0 And lRetVal <= UBound(Reg_Error) Then
      MsgBox Reg_Error(lRetVal) & "Could not set registry value" & vbCr & "section = 'HKEY_LOCAL_MACHINE\" & GlobalPrefix & section & "'" & vbCr & "key = '" & key & "'" & vbCr & "value = '" & NewValue & "'", vbOKOnly, "Registry"
    Else
      MsgBox "Could not set registry value" & vbCr & "section = 'HKEY_LOCAL_MACHINE\" & GlobalPrefix & section & "'" & vbCr & "key = '" & key & "'" & vbCr & "value = '" & NewValue & "'" & vbCr & "Error = " & lRetVal, vbOKOnly, "Registry"
    End If
  End If
End Property

Public Sub RegSetString(keyhandle As HKEY_TYPE, ByVal section As String, ByVal key As String, ByVal newValue As String)
  Dim hNewKey As Long
  Dim lRetVal As Long, disposition As Long

  lRetVal = RegCreateKeyEx(keyhandle, section, 0&, vbNullString, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, 0&, hNewKey, disposition)
  'add avlue
  If lRetVal = ERROR_SUCCESS Then
    lRetVal = RegSetValue(hNewKey, key, REG_SZ, newValue, Len(newValue))
    RegCloseKey (hNewKey)
  End If
  If lRetVal <> ERROR_SUCCESS Then
    If lRetVal >= 0 And lRetVal <= UBound(Reg_Error) Then
      MsgBox Reg_Error(lRetVal) & "Could not set create registry value" & vbCr & "section = '" & section & "'" & vbCr & "key = '" & key & "'" & vbCr & "value = '" & newValue & "'", vbOKOnly, "Registry"
    Else
      MsgBox "Could not set registry value" & vbCr & "section = '" & section & "'" & vbCr & "key = '" & key & "'" & vbCr & "value = '" & newValue & "'" & vbCr & "Error = " & lRetVal, vbOKOnly, "Registry"
    End If
  End If
End Sub

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
  SZ = MAX_PATH
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
  If dwType = REG_SZ Then retval = Left(v, SZ - 1)
'  End If
  If keyhandle = 0 Then r = RegCloseKey(hSubKey)
Quit_Now:
  RegGetString = retval
End Function

'Public Function ClassIDfilename(ByVal ClassID As String, ByVal version As String) As String
'  If Left(ClassID, 1) <> "{" Then ClassID = "{" & ClassID & "}"
'  ClassIDfilename = RegGetString(HKEY_CLASSES_ROOT, "TypeLib\" & ClassID & "\" & version & "\0\win32" & vbNullString, "@" & vbNullString)
'End Function

Public Function ClassID(Filename As String, Optional CLSIDguess As String) As String
  Dim r As Long, rSubkey As Long
  Dim CLSIDindex As Long, VersionIndex As Long
  Dim lenCLSID As Long, dumLong As Long, lastModified As FILETIME
  Dim version As String, lenVersion As Long
  Dim thisFilename As String, lenFilename As Long
  Dim hKeyTypeLib As Long, hKeyVersion As Long
  If Not IsMissing(CLSIDguess) Then
    r = RegOpenKeyEx(HKEY_CLASSES_ROOT, "TypeLib\" & CLSIDguess & vbNullString, 0&, KEY_ALL_CLASSES, hKeyTypeLib)
    If r = ERROR_SUCCESS Then
      r = RegCloseKey(hKeyTypeLib)
      ClassID = CLSIDguess
      Exit Function
    End If
  End If
  CLSIDindex = 0
  r = RegOpenKeyEx(HKEY_CLASSES_ROOT, "TypeLib", 0&, KEY_ALL_CLASSES, hKeyTypeLib)
  While r = ERROR_SUCCESS
    CLSIDguess = Space(255)
    r = RegEnumKeyEx(hKeyTypeLib, CLSIDindex, CLSIDguess, lenCLSID, 0&, vbNullString, dumLong, lastModified)
    If r = ERROR_SUCCESS Then
      VersionIndex = 0
      rSubkey = RegOpenKeyEx(hKeyTypeLib, CLSIDguess, 0&, KEY_ALL_CLASSES, hKeyVersion)
      While rSubkey = ERROR_SUCCESS
        CLSIDguess = Left(CLSIDguess, lenCLSID)
        rSubkey = RegEnumKeyEx(hKeyVersion, VersionIndex, version, lenVersion, 0&, vbNullString, dumLong, lastModified)
        If rSubkey = ERROR_SUCCESS Then
          version = Left(version, lenVersion)
          thisFilename = RegGetString(HKEY_CLASSES_ROOT, "TypeLib\" & CLSIDguess & "\" & version & "\0\win32" & vbNullString, "@" & vbNullString)
          If thisFilename = Filename Then
            ClassID = CLSIDguess
            Exit Function
          End If
          VersionIndex = VersionIndex + 1
        End If
      Wend
    End If
    CLSIDindex = CLSIDindex + 1
  Wend
  ClassID = ""
End Function

'appName     = "winword"           name of executable without .exe
'appLabel    = "Word 95"           Human readable name of app
'extension   = ".doc"              If * is first char, it is removed, If . is not first, it will be added
'commandLine = "winword.exe -f %1" %1 will be replaced with filename when run
'Returns "" on success, error message on failure
Public Function AddAssociation(AppName As String, appLabel As String, extension As String, commandLine As String) As String
  Dim ret&                 'Holds error status if any from API calls.
  Dim keyhandle&           'Holds created key handle from RegCreateKey.
  Dim localExtension As String
  Dim sKeyName As String
  
  localExtension = extension
  If Left(localExtension, 1) = "*" Then localExtension = Mid(localExtension, 2)
  If Left(localExtension, 1) <> "." Then localExtension = "." & localExtension
  AddAssociation = ""
  
  'This creates a Root entry for appName and a label
  ret = RegCreateKey&(HKEY_CLASSES_ROOT, AppName, keyhandle)
  If ret > 0 Then GoTo ErrorMessage
  ret = RegSetValue&(keyhandle, "", REG_SZ, appLabel, 0)
  If ret > 0 Then GoTo ErrorMessage

  'This creates a Root entry for the extenstion associated with appName
  ret& = RegCreateKey&(HKEY_CLASSES_ROOT, localExtension, keyhandle)
  If ret > 0 Then GoTo ErrorMessage
  ret& = RegSetValue&(keyhandle, "", REG_SZ, AppName, 0)
  If ret > 0 Then GoTo ErrorMessage
  RegCloseKey (keyhandle)
  
  'This sets the command line for "MyApp".
  sKeyName = "MyApp" '*
  ret& = RegCreateKey&(HKEY_CLASSES_ROOT, AppName, keyhandle)
  If ret > 0 Then GoTo ErrorMessage
  ret& = RegSetValue&(keyhandle, "shell\open\command", REG_SZ, commandLine, MAX_PATH)
  If ret > 0 Then GoTo ErrorMessage
  RegCloseKey (keyhandle)
  Exit Function
ErrorMessage:
  AddAssociation = Reg_Error(ret) & vbCr & "Could not create association:" & vbCr & "appName='" & AppName & "'" & vbCr & "appLabel='" & appLabel & "'" & vbCr & "extension='" & localExtension & "'" & vbCr & "commandLine='" & commandLine & "'" & vbCr
End Function

'appName     = "winword"           name of executable without .exe
'appLabel    = "Word 95"           Human readable name of app
'extension   = ".doc"              If * is first char, it is removed, If . is not first, it will be added
'commandLine = "winword.exe -f %1" %1 will be replaced with filename when run
'Returns "" on success, error message on failure
Public Function RemoveAssociation(AppName As String, extension As String) As String
  Dim ret&                 'Holds error status if any from API calls.
  Dim val As String
  Dim localExtension As String
  localExtension = extension
  If Left(localExtension, 1) = "*" Then localExtension = Mid(localExtension, 2)
  If Left(localExtension, 1) <> "." Then localExtension = "." & localExtension
  RemoveAssociation = ""
  
  val = RegGetString(HKEY_CLASSES_ROOT, "", localExtension)
  If UCase(val) = UCase(AppName) Then
    ret = RegDeleteKey(HKEY_CLASSES_ROOT, localExtension)
  End If
  If ret > 0 Then GoTo ErrorMessage
  'RegCloseKey (keyhandle)
  Exit Function
ErrorMessage:
  RemoveAssociation = Reg_Error(ret) & vbCr & _
                      "Could not remove association:" & vbCr & _
                      "appName='" & AppName & "'" & vbCr & _
                      "extension='" & localExtension & "'" & vbCr
                      
End Function

Private Sub Class_Initialize()
  ReDim Reg_Error(ERROR_ACCESS_DENIED)
  
  Reg_Error(ERROR_SUCCESS) = "Success"
  Reg_Error(ERROR_BADDB) = "Error: Bad DB"
  Reg_Error(ERROR_BADKEY) = "Error: Bad Key"
  Reg_Error(ERROR_CANTOPEN) = "Error: Can't Open"
  Reg_Error(ERROR_CANTREAD) = "Error: Can't Read"
  Reg_Error(ERROR_CANTWRITE) = "Error: Can't Write"
  Reg_Error(ERROR_OUTOFMEMORY) = "Error: Out of Memory"
  Reg_Error(ERROR_INVALID_PARAMETER) = "Error: Invalid Parameter"
  Reg_Error(ERROR_ACCESS_DENIED) = "Error: Access Denied"
  
  pAppName = App.EXEName
  GlobalPrefix = "Software\AQUA TERRA Consultants\"
End Sub
