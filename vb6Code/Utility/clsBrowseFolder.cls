VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsBrowseFolder"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Private WithEvents objBD As BrowseDialog 'from common controls replacement project, see utilBrowseFolder for no events version

Friend Function BrowseFolderX(Owner As Form, Optional prompt As String = "Choose a folder from the list", Optional DefaultPath As String = "") As String
  Set objbd = New BrowseDialog
  objbd.AllowNewFolder = True
  objbd.Prompt1 = prompt
  objbd.SelectedFolder = DefaultPath
  objbd.Sizable = True
  objbd.BrowseMode = bdBrowseFSFoldersOnly
  ' assigning an hWnd to hwndOwner makes the dialog modal)
  objbd.hwndOwner = Owner.hwnd
  objbd.caption = Owner.caption & " Browse for Folder"
  If objbd.Browse Then
    BrowseFolder = objbd.SelectedFolder
  Else 'cancel
    BrowseFolder = ""
  End If
End Function

