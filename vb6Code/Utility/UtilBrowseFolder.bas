Attribute VB_Name = "UtilBrowseFolder"
Option Explicit

Public Function BrowseFolder(Owner As Form, Optional prompt As String = "Choose a folder from the list", Optional DefaultPath As String = "", Optional AllowNewFolder As Boolean = False) As String
  Dim objbd As BrowseDialog 'from common controls replacement project, see clsBrowseFolder if events needed
  
  Set objbd = New BrowseDialog
  objbd.AllowNewFolder = True
  objbd.Prompt1 = prompt
  objbd.SelectedFolder = DefaultPath
  objbd.Sizable = True
  objbd.BrowseMode = bdBrowseFSFoldersOnly
  ' assigning an hWnd to hwndOwner makes the dialog modal
  objbd.hwndOwner = Owner.hWnd
  objbd.caption = Owner.caption & " Browse for Folder"
  objbd.AllowNewFolder = AllowNewFolder
  If objbd.Browse Then
    BrowseFolder = objbd.SelectedFolder & "\"
  Else 'cancel
    BrowseFolder = ""
  End If
End Function

'Private pDefaultPath As String
'
'Private Declare Function SHBrowseForFolder Lib "shell32.dll" Alias _
'        "SHBrowseForFolderA" (lpBrowseInfo As BROWSEINFO) As Long
'
'Private Declare Function SHGetPathFromIDList Lib "shell32.dll" Alias _
'        "SHGetPathFromIDListA" (ByVal pidl As Long, _
'        ByVal pszPath As String) As Long
'
'Declare Function SHSimpleIDListFromPath Lib "shell32" Alias "#162" _
'                              (ByVal szPath As String) As Long
'
'Declare Sub CoTaskMemFree Lib "ole32.dll" (ByVal pv As Long)
'
'Private Const BIF_RETURNONLYFSDIRS = &H1      'Only file system directories
'Private Const BIF_DONTGOBELOWDOMAIN = &H2     'No network folders below domain level
'Private Const BIF_STATUSTEXT = &H4            'Includes status area in the dialog (for callback)
'Private Const BIF_RETURNFSANCESTORS = &H8     'Only returns file system ancestors
'Private Const BIF_EDITBOX = &H10              'Allows user to rename selection
'Private Const BIF_VALIDATE = &H20             'Insist on valid edit box result (or CANCEL)
'Private Const BIF_USENEWUI = &H40             'Version 5.0. Use the new user-interface.
'                                             'Setting this flag provides the user with
'                                             'a larger dialog box that can be resized.
'                                             'It has several new capabilities including:
'                                             'dialog box, reordering, context menus, new
'                                             'folders, drag and drop capability within
'                                             'the delete, and other context menu commands.
'                                             'To use you must call OleInitialize or
'                                             'CoInitialize before calling SHBrowseForFolder.
'Private Const BIF_BROWSEFORCOMPUTER = &H1000  'Only returns computers.
'Private Const BIF_BROWSEFORPRINTER = &H2000   'Only returns printers.
'Private Const BIF_BROWSEINCLUDEFILES = &H4000 'Browse for everything
'
'' From WinUser.h
'
'' NOTE: All Message Numbers below 0x0400 are RESERVED.
'
'Private Const WM_USER = &H400
'
'' From ShlObj.h
'
'' message from browser
'Private Const BFFM_INITIALIZED = 1
'Private Const BFFM_SELCHANGED = 2
'Private Const BFFM_VALIDATEFAILEDA = 3  ' lParam:szPath ret:1(cont),0(EndDialog)
'Private Const BFFM_VALIDATEFAILEDW = 4  ' lParam:wzPath ret:1(cont),0(EndDialog)
'Private Const BFFM_IUNKNOWN = 5         ' provides IUnknown to client. lParam: IUnknown*
'
'' messages to browser
'Private Const BFFM_SETSTATUSTEXTA = (WM_USER + 100)
'Private Const BFFM_ENABLEOK = (WM_USER + 101)
'Private Const BFFM_SETSELECTIONA = (WM_USER + 102)
'Private Const BFFM_SETSELECTIONW = (WM_USER + 103)
'Private Const BFFM_SETSTATUSTEXTW = (WM_USER + 104)
'Private Const BFFM_SETOKTEXT = (WM_USER + 105)          ' unicode only
'Private Const BFFM_SETEXPANDED = (WM_USER + 106)        ' Unicode only
'
'Private Type BROWSEINFO
'  hOwner As Long
'  pidlRoot As Long
'  pszDisplayName  As String
'  lpszTitle As String
'  ulFlags As Long
'  lpfn As Long
'  lParam As Long
'  iImage As Long
'End Type
'
'Private Type SHITEMID
'  cb As Long
'  abID As Byte
'End Type
'
'Private Type ITEMIDLIST
'  mkid As SHITEMID
'End Type
'
'Public Function BrowseFolderX(Owner As Form, Optional prompt As String = "Choose a folder from the list", Optional DefaultPath As String = "") As String
'  Dim bi As BROWSEINFO
'  Dim IDL As ITEMIDLIST
'  Dim r As Long
'  Dim pidl As Long
'  Dim tmppath As String
'  Dim Msg As String
'  Dim pos As Integer
'
'  pDefaultPath = DefaultPath
'
'  bi.hOwner = Owner.hwnd
'  bi.pidlRoot = 0&
'  bi.lpszTitle = prompt
'  bi.ulFlags = BIF_RETURNONLYFSDIRS Or BIF_USENEWUI
'  bi.lpfn = setAddress(AddressOf BrowseCallback)
'  'Msg = "Hi!" & Chr(0)
'  'bi.lParam = StrPtr(Msg)
'  pidl = SHBrowseForFolder(bi)
'
'  tmppath = Space$(512)
'  r = SHGetPathFromIDList(ByVal pidl, ByVal tmppath)
'  'free up memory for the pidl
'  CoTaskMemFree pidl
'
'  If r Then
'    pos = InStr(tmppath, Chr$(0))
'    tmppath = Left(tmppath, pos - 1)
'
'    If Right(tmppath, 1) <> "\" Then tmppath = tmppath & "\"
'    BrowseFolder = tmppath
'  Else
'    BrowseFolder = ""
'  End If
'End Function
'
'Private Function setAddress(v) As Long
'  setAddress = v
'End Function
'
'Public Function BrowseCallback(ByVal hwnd As Long, ByVal uint As Long, ByVal lParam As Long, ByVal lParamX As Long) As Integer
'  Dim j As Long
'  Dim tmppath As String
'
'  Select Case uint
'    Case BFFM_INITIALIZED:
'      If Len(pDefaultPath) = 0 Then
'        tmppath = CurDir & Chr(0)
'      Else
'        tmppath = pDefaultPath & Chr(0)
'      End If
'      j = SendMessage(hwnd, BFFM_SETSELECTIONA, 1&, ByVal tmppath)
'      'j = SendMessage(hwnd, BFFM_SETSELECTIONA, -1&, ByVal SHSimpleIDListFromPath(tmppath))
'      Debug.Print "Browse, status:" & j & " change to " & tmppath
'    Case BFFM_SELCHANGED:
'    Case Else: Debug.Print "Browse Unknown Message " & uint
'  End Select
'  BrowseCallback = 0
'End Function
