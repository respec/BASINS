Attribute VB_Name = "UtilShortcut"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Declare Function SHAddToRecentDocs Lib "shell32.dll" _
    (ByVal dwFlags As Long, ByVal dwData As String) As Long

Private Declare Function SHFileOperation _
    Lib "shell32.dll" Alias "SHFileOperationA" _
    (lpFileOp As SHFILEOPSTRUCT) As Long

'LinkPath is the directory where the link will be placed. May want to use GetSpecialFolder.
'LinkLabel will be the text under the link icon
'ExePath is the path of the executable to run
'cmdLine is the filename of the executable to run and any parameters
Sub CreateLink(LinkPath As String, LinkLabel$, ExePath$, cmdLine$)

  Dim r As Long
  Dim i As Integer
  Dim FolderPath As String
  Dim fname As String
  Dim fNameOld As String
  Dim fNameNew As String
  
  FolderPath = GetSpecialFolder(CSIDL_RECENT) 'Recent Files folder
  
  If FolderPath = "" Or LinkPath = "" Then
    MsgBox "We canne' do it Captain: Error retrieving folder paths."
    Exit Sub
  End If
  fname = ExePath & cmdLine
  'Add the file to the Recent Files list.
  r = SHAddToRecentDocs(SHARD_PATH, fname)
  
  'At this point, if you check the Start\Documents menu,
  'the file will be listed.
  
  '--------------------------------------------------------------------
  'Since we now know the shortcuts exist in the documents
  'folder, and the path to both that folder and the users
  'start menu, we can move the shortcuts from the recent
  'folder into our new application folder using the
  'SHFileOperation API.  Conveniently, SHFileOperation
  'will even create out new folder for us if it doesn't
  'exist!  And by specifying the flags FOF_SILENT and
  'FOF_NOCONFIRMATION, messages and dialogs indicating
  'this is happening will be suppressed.
  '
  'Because the shortcuts now reside in the Recent folder,
  'we need to modify the file array to include the Recent
  'folder path. In addition, the extension now has .lnk
  'appended to the original filename.
  fname = FolderPath & cmdLine & ".lnk"
  
  'call SHFileOperation to move the shortcuts
  ShellMoveFile fname, LinkPath
  
  'Now, if desired, we can rename the links to
  'something more traditional for a start menu item.
  'The FO_RENAME flag in ShellRenameFile requires
  'that we do this one file at a time.
  fNameOld = LinkPath & cmdLine & ".lnk"
  fNameNew = LinkPath & LinkLabel
  ShellRenameFile fNameOld, fNameNew
  
End Sub

Private Sub ShellMoveFile(sFile As String, sDestination As String)
  
  'set some working variables
  Dim r As Long
  Dim i As Integer
  Dim sFiles As String
  Dim SHFileOp As SHFILEOPSTRUCT
  
  sFiles = sFile & Chr$(0)
  'set up the options
  With SHFileOp
    .wFunc = FO_MOVE
    .pFrom = sFiles
    .pTo = sDestination
    .fFlags = FOF_SILENT Or FOF_NOCONFIRMATION Or FOF_NOCONFIRMMKDIR
  End With
  
  'and perform the move.  Because the folder specified
  'doesn't exist, SHFileOperation will create it. FOF_SILENT
  'above instructs the API to suppress displaying the "flying
  'folders" dialog during the move.  FOF_NOCONFIRMATION suppresses
  'prompting to move the files ... the "Are you sure you want to
  'move etc..." dialog. FOF_NOCONFIRMMKDIR instructs it to
  'create the folder without prompting if its OK.
  r = SHFileOperation(SHFileOp)

End Sub

'Moved to Win32Api.bas (modWin32Api)
'Public Function GetSpecialFolder(CSIDL As SpecialFolderType) As String
'
'  'a few local variables needed
'   Dim r As Long
'   Dim sPath As String
'   Dim pidl As Long
'
'   Const NOERROR = 0
'   Const MAX_LENGTH = 260
'
'  'fill pidl with the specified folder item
'   r = SHGetSpecialFolderLocation(frmGenScn.hwnd, CSIDL, pidl)
'
'   If r = NOERROR Then
'
'     'Of the structure is filled, initialize and
'     'retrieve the path from the id list, and return
'     'the folder with a trailing slash appended.
'      sPath = Space$(MAX_LENGTH)
'      r = SHGetPathFromIDList(ByVal pidl, ByVal sPath)
'
'      If r Then
'         GetSpecialFolder = Left$(sPath, _
'                            InStr(sPath, Chr$(0)) - 1) & "\"
'      End If
'
'   End If
'
'End Function

Private Sub ShellRenameFile(sOldName As String, sNewName As String)
  
  'set some working variables
   Dim SHFileOp As SHFILEOPSTRUCT
   Dim r As Long
  
  'add a pair of terminating nulls to each string
   sOldName = sOldName & Chr$(0) & Chr$(0)
   sNewName = sNewName & Chr$(0) & Chr$(0)
    
  'set up the options
   With SHFileOp
      .wFunc = FO_RENAME
      .pFrom = sOldName
      .pTo = sNewName
      .fFlags = FOF_SILENT Or FOF_NOCONFIRMATION
   End With
  
  'and rename the file
   r = SHFileOperation(SHFileOp)

End Sub


'Declare Function GetWindowsDirectory Lib "Kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
'Declare Function GetSystemDirectory Lib "Kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long

'Declare Function DLLSelfRegister Lib "vb6stkit.dll" (ByVal lpDllName As String) As Integer
'How to register and un-register ActiveX Controls
'
'This tip describes how ActiveX controls can be registered and unregistered directly from Visual Basic.  Every ActiveX control
'contains two functions that can be called that will instruct the OCX to either register or un-register itself with the system.
'These functions are DLLRegisterServer and DLLUnregisterServer.  The following tip demonstrates how to register and
'un-register the Microsoft Common Controls OCX, ComCtl32.OCX.
'
'Declarations
'
'Copy the following code into the declarations section of your projects.
'
'Declare Function RegComCtl32 Lib "ComCtl32.OCX" _
'Alias "DllRegisterServer" () As Long
'
'Declare Function UnRegComCtl32 Lib "ComCtl32.OCX" _
'Alias "DllUnregisterServer" () As Long
'
'Const ERROR_SUCCESS = &H0
'
'Code
'
'To register the Microsoft Common Controls, use this code:
'
'If RegComCtl32 = ERROR_SUCCESS Then
'    MsgBox "Registration Successful"
'Else
'    MsgBox "Registration Unsuccessful"
'End If
'
'If UnRegComCtl32 = ERROR_SUCCESS Then
'    MsgBox "UnRegistration Successful"
'Else
'    MsgBox "UnRegistration Unsuccessful"
'End If '
'
'Note: Each DLL function call may take up to 5 seconds.

'Declare Function OSfCreateShellLink Lib "vb6stkit.dll" Alias "fCreateShellLink" (ByVal lpstrFolderName As String, ByVal lpstrLinkName As String, ByVal lpstrLinkPath As String, ByVal lpstrLinkArguments As String, ByVal fPrivate As Long, ByVal sParent As String) As Long
'Declare Function OSfRemoveShellLink Lib "vb6stkit.dll" Alias "fRemoveShellLink" (ByVal lpstrFolderName As String, ByVal lpstrLinkName As String) As Long

'Declare Function fCreateShellLink Lib "vb6stkit.dll" (ByVal _
'        lpstrFolderName As String, ByVal lpstrLinkName As String, ByVal _
'        lpstrLinkPath As String, ByVal lpstrLinkArguments As String, _
'        ByVal fPrivate As Long, ByVal sParent As String) As Long

'Declare Function fCreateShellLink Lib "stkit432.dll" (ByVal _
'        lpstrFolderName As String, ByVal lpstrLinkName As String, ByVal _
'        lpstrLinkPath As String, ByVal lpstrLinkArgs As String) As Long

