Attribute VB_Name = "mGeneral"
Option Explicit
'-------------------------------------------------------------------------------------------'
'   This Registry handler is developed by Ronald Kas (r.kas@kaycys.com)                     '
'   from Kaycys (http://www.kaycys.com).                                                    '
'                                                                                           '
'   You may use this Registry Handler for all purposes except from making profit with it.   '
'   Check our site regulary for updates.                                                    '
'-------------------------------------------------------------------------------------------'


' Declare Windows API functions...
Public Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Long) As Long
Public Declare Function RegCreateKeyEx Lib "advapi32.dll" Alias "RegCreateKeyExA" (ByVal hKey As Long, _
                        ByVal lpSubKey As String, ByVal Reserved As Long, ByVal lpClass As String, _
                        ByVal dwOptions As Long, ByVal samDesired As Long, _
                        ByRef lpSecurityAttributes As SECURITY_ATTRIBUTES, ByRef phkResult As Long, _
                        ByRef lpdwDisposition As Long) As Long
Public Declare Function RegDeleteKey Lib "advapi32.dll" Alias "RegDeleteKeyA" (ByVal hKey As Long, _
                        ByVal lpSubKey As String) As Long
Public Declare Function RegEnumKeyEx Lib "advapi32.dll" Alias "RegEnumKeyExA" (ByVal hKey As Long, _
                        ByVal dwIndex As Long, ByVal lpName As String, ByRef lpcbName As Long, _
                        ByVal lpReserved As Long, ByVal lpClass As String, ByRef lpcbClass As Long, _
                        lpftLastWriteTime As FILE_TIME) As Long
Public Declare Function RegEnumValue Lib "advapi32.dll" Alias "RegEnumValueA" (ByVal hKey As Long, _
                        ByVal dwIndex As Long, ByVal lpValueName As String, ByRef lpcbValueName As Long, _
                        ByVal lpReserved As Long, ByRef lpType As Long, ByRef lpData As Any, _
                        ByRef lpcbData As Long) As Long
Public Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (ByVal hKey As Long, _
                        ByVal lpSubKey As String, ByVal ulOptions As Long, ByVal samDesired As Long, _
                        ByRef phkResult As Long) As Long
Public Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Long, _
                        ByVal lpValueName As String, ByVal lpReserved As Long, ByRef lpType As Long, _
                        ByVal lpData As String, ByRef lpcbData As Long) As Long
Public Declare Function RegQueryInfoKey Lib "advapi32.dll" Alias "RegQueryInfoKeyA" (ByVal hKey As Long, _
                        ByVal lpClass As String, ByRef lpcbClass As Long, ByVal lpReserved As Long, _
                        ByRef lpcSubKeys As Long, ByRef lpcbMaxSubKeyLen As Long, ByRef lpcbMaxClassLen As Long, _
                        ByRef lpcValues As Long, ByRef lpcbMaxValueNameLen As Long, ByRef lpcbMaxValueLen As Long, _
                        ByRef lpcbSecurityDescriptor As Long, ByRef lpftLastWriteTime As FILE_TIME) As Long
Public Declare Function RegSetValueExString Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Long, _
                        ByVal lpValueName As String, ByVal Reserved As Long, ByVal dwType As Long, _
                        ByVal lpValue As String, ByVal cbData As Long) As Long
Public Declare Function RegSetValueExBoolean Lib "advapi32" Alias "RegSetValueExA" (ByVal hKey As Long, _
                        ByVal lpValueName As String, ByVal Reserved As Long, ByVal dwType As Long, _
                        ByRef lpData As Boolean, ByVal cbData As Long) As Long
Public Declare Function RegSetValueExLong Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Long, _
                        ByVal lpValueName As String, ByVal Reserved As Long, ByVal dwType As Long, _
                        ByRef lpValue As Long, ByVal cbData As Long) As Long
Public Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" (ByVal hKey As Long, ByVal lpValueName As String) As Long

                        
' Declare Windows API constants...
Public Const lngHKEY_CLASSES_ROOT = &H80000000
Public Const lngHKEY_CURRENT_USER = &H80000001
Public Const lngHKEY_LOCAL_MACHINE = &H80000002
Public Const lngHKEY_USERS = &H80000003

Public Const lngERROR_SUCCESS = 0&
Public Const lngERROR_INVALID_HANDLE = 6&
Public Const lngERROR_FAILURE = 13&
Public Const lngUNREADABLE_NODE = 234&
Public Const lngNO_MORE_NODES = 259&
Public Const lngERROR_MORE_DATA = 234&

Public Const lngREG_OPTION_NON_VOLATILE = 0
Public Const lngSYNCHRONIZE = &H100000
Public Const lngSTANDARD_RIGHTS_READ = &H20000
Public Const lngKEY_QUERY_VALUE = &H1
Public Const lngKEY_ENUMERATE_SUB_KEYS = &H8
Public Const lngKEY_NOTIFY = &H10
Public Const lngKEY_SET_VALUE = &H2
Public Const lngKEY_CREATE_SUB_KEY = &H4
Public Const lngKEY_CREATE_LINK = &H20
Public Const lngSTANDARD_RIGHTS_ALL = &H1F0000
Public Const lngKEY_READ = ((lngSTANDARD_RIGHTS_READ Or lngKEY_QUERY_VALUE Or lngKEY_ENUMERATE_SUB_KEYS Or _
                             lngKEY_NOTIFY) And (Not lngSYNCHRONIZE))
Public Const lngKEY_ALL_ACCESS = ((lngSTANDARD_RIGHTS_ALL Or lngKEY_QUERY_VALUE Or lngKEY_SET_VALUE Or _
                                   lngKEY_CREATE_SUB_KEY Or lngKEY_ENUMERATE_SUB_KEYS Or lngKEY_NOTIFY Or _
                                   lngKEY_CREATE_LINK) And (Not lngSYNCHRONIZE))
Public Const lngREG_SZ = 1
Public Const lngREG_BINARY = 3
Public Const lngREG_DWORD = 4

' Declare Windows API types...
Public Type FILE_TIME
  dwLowDateTime As Long
  dwHighDateTime As Long
End Type
Type SECURITY_ATTRIBUTES
  nLength As Long
  lpSecurityDescriptor As Long
  bInheritHandle As Boolean
End Type

