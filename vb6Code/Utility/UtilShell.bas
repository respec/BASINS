Attribute VB_Name = "modShell"
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

' ##MODULE_NAME modShell
' ##MODULE_DATE March 26, 2002
' ##MODULE_AUTHOR Jack Kittle and Mark Gray of AQUA TERRA CONSULTANTS
' ##MODULE_SUMMARY Contains functions for opening external files.

Private Declare Function ShellExecute Lib "shell32.dll" Alias _
 "ShellExecuteA" _
  (ByVal hwnd As Long, _
   ByVal lpOperation As String, _
   ByVal lpFile As String, _
   ByVal lpParameters As String, _
   ByVal lpDirectory As String, _
   ByVal nShowCmd As Long) As Long
  '##SUMMARY Opens or prints a specified file. The file _
    can be an executable file or a document file.
  '##PARAM hwnd M Specifies a parent window. This window receives any message _
    boxes that an application produces. For example, an application may report _
    an error by producing a message box.
  '##PARAM lpOperation I Pointer to a null-terminated string that specifies the _
    operation to perform: "open", "print", or "explore".
  '##PARAM lpFile I Pointer to a null-terminated string that specifies the file _
    to open or print or the folder to open or explore. The function can open an _
    executable file or a document file. The function can print a document file.
  '##PARAM lpParameters I If lpFile specifies an executable file, lpParameters _
    is a pointer to a null-terminated string that specifies parameters to be _
    passed to the application. If lpFile specifies a document file, lpParameters _
    should be NULL.
  '##PARAM lpDirectory I Pointer to a null-terminated string that specifies the _
    default directory.
  '##PARAM nShowCmd I If lpFile specifies an executable file, nShowCmd specifies _
    how the application is to be shown when it is opened.
  '##RETURNS If the function succeeds, the return value is the instance handle of _
    the application that was run, or the handle of a dynamic data exchange (DDE) _
    server application. If the function fails, the return value is an error value _
    that is less than or equal to 32.

Public Function OpenFile(ByVal filename As String, Optional cdlg As CommonDialog, Optional operation As String = "open") As String
' ##SUMMARY Opens an external file using its files association. Displays message box on failure.
' ##PARAM FileName I Full path and filename.
' ##PARAM CDlg I Allows user to browse for file to be opened.
' ##RETURNS Blank string if file opened successfully, error message if not.
  Dim msg As String 'message returned to caller
  msg = OpenFileQuiet(filename, cdlg, operation)
  If Len(msg) > 0 Then
    If msg <> filename Then MsgBox msg & vbCr & filename, vbOKOnly, "Error opening file"
  End If
End Function

Public Function OpenFileQuiet(ByVal filename As String, Optional cdlg As CommonDialog, Optional operation As String = "open") As String
' ##SUMMARY Opens an external file using its files association. Does not display message box on failure.
' ##PARAM FileName I Full path and filename.
' ##PARAM CDlg I Allows user to browse for file to be opened.
' ##PARAM operation I If value is "print", file will be printed to default printer instead of opened
' ##RETURNS Name of file if file opened successfully, error message if not.
  Dim DirectoryNm As String 'directory path only, no filename
  Dim FileNm As String      'filename only, no directory path
  Dim errShell As Long      'code returned from call to ShellExecute, > 32 if successful
  Dim msg As String         'message returned to caller
  
  On Error GoTo errOpenFile
'
' Get file and pathname.
'
  filename = Trim(filename)
  If LCase(Left(filename, 7)) = "http://" Then
    FileNm = filename
    GoTo ShExec 'Skip checks to see if it is a local file
  End If
  If Not FileExists(filename, True, True) Then
    If Not IsMissing(cdlg) Then
      If Not cdlg Is Nothing Then
        cdlg.DialogTitle = "Open File"
        cdlg.filename = filename
        cdlg.CancelError = False
        cdlg.ShowOpen
        filename = Trim(cdlg.filename)
      End If
    End If
  End If

  If FileExists(filename, True, True) Then
    DirectoryNm = PathNameOnly(filename)
    If Len(DirectoryNm) = 0 Then
      FileNm = filename
    Else
      FileNm = Mid(filename, Len(DirectoryNm) + 2)
    End If
  '
  ' Open the file.
  '
ShExec:
    errShell = ShellExecute(0&, operation, FileNm, 0&, DirectoryNm, 1) '1=SW_SHOWNORMAL
    'File successfully opened if errShell > 32
    msg = ""
    If errShell <= 32 Then msg = errShell
    Select Case errShell
      Case 0:  msg = "The file could not be run due to insufficient system memory or a corrupt program file"
      Case 2:  msg = "File Not Found"
      Case 3:  msg = "Invalid Path"
      Case 5:  msg = "Sharing or protection error"
      Case 6:  msg = "Separate data segments are required for each task"
      Case 8:  msg = "Insufficient memory to run the program"
      Case 10: msg = "Incorrect Windows version"
      Case 11: msg = "Invalid Program File"
      Case 12: msg = "Program file requires a different operating System"
      Case 13: msg = "Program requires MS-DOS 4.0"
      Case 14: msg = "Unknown program file type"
      Case 15: msg = "Windows prgram does not support protected memory mode"
      Case 16: msg = "Invalid use of data segments when loading a second instance of a program"
      Case 19: msg = "Attempt to run a compressed program file"
      Case 20: msg = "Invalid dynamic link library"
      Case 21: msg = "Program requires Windows 32-bit extensions"
      Case 31: msg = "No application found for this file"
    End Select
  End If
  If msg = "" Then OpenFileQuiet = filename Else OpenFileQuiet = msg
  Exit Function
errOpenFile:
  OpenFileQuiet = "Could not open '" & FileName & "'" & vbCr & Err.Description
End Function

