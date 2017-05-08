Attribute VB_Name = "modProcesses"
Option Explicit

Private Declare Function GetWindow Lib "user32" (ByVal hwnd As Long, ByVal wCmd As Long) As Long
Private Declare Function GetWindowTextLength Lib "user32" Alias "GetWindowTextLengthA" (ByVal hwnd As Long) As Long
Private Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hwnd As Long, ByVal lpString As String, ByVal cch As Long) As Long
Private Declare Function GetWindowThreadProcessId Lib "user32" (ByVal hwnd As Long, lpdwProcessId As Long) As Long

Private Const GW_CHILD = 5
Private Const GW_HWNDFIRST = 0
Private Const GW_HWNDLAST = 1
Private Const GW_HWNDNEXT = 2
Private Const GW_HWNDPREV = 3
Private Const GW_MAX = 5
Private Const GW_OWNER = 4

Public Sub GetRunningApplications()
  Dim lLgthChild As Long
  Dim sNameChild As String
  Dim lLgthOwner As Long
  Dim sNameOwner As String
  Dim lHwnd As Long
  Dim lHwnd2 As Long
  Dim lProssId As Long

  Const vbTextCompare = 1
  
  lHwnd = GetWindow(fMain.hwnd, GW_HWNDFIRST)
  While lHwnd <> 0
    lHwnd2 = GetWindow(lHwnd, GW_OWNER)
    lLgthOwner = GetWindowTextLength(lHwnd2)
    sNameOwner = String$(lLgthOwner + 1, Chr$(0))
    lLgthOwner = GetWindowText(lHwnd2, sNameOwner, lLgthOwner + 1)
    If lLgthOwner <> 0 Then
      sNameOwner = Left$(sNameOwner, InStr(1, sNameOwner, Chr$(0), vbTextCompare) - 1)
      Call GetWindowThreadProcessId(lHwnd2, lProssId)
      Debug.Print sNameOwner, lProssId
    End If
    
    lLgthChild = GetWindowTextLength(lHwnd)
    sNameChild = String$(lLgthChild + 1, Chr$(0))
    lLgthChild = GetWindowText(lHwnd, sNameChild, lLgthChild + 1)
    If lLgthChild <> 0 Then
       sNameChild = Left$(sNameChild, InStr(1, sNameChild, Chr$(0), vbTextCompare) - 1)
       
       Call GetWindowThreadProcessId(lHwnd, lProssId)
       Debug.Print sNameChild, lProssId
      
    End If
    lHwnd = GetWindow(lHwnd, GW_HWNDNEXT)
    DoEvents
  Wend

End Sub

