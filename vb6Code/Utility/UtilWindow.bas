Attribute VB_Name = "UtilWindow"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

'Returns a handle to the window containing TitlePart as part of its window title
'Returns 0 if no window's title contains TitlePart (non-case-sensitive match)
'Window with hWnd = notHwnd is skipped in search
Function FindWindowPartial(ByVal TitlePart$, ByVal notHwnd&) As Long
  'Borrowed from a VBPJ article...
  
  Dim hWndTmp As Long
  Dim nRet As Long
  Dim TitleTmp As String
  Const GW_HWNDNEXT = 2
  
  FindWindowPartial = 0 'default return value if no matching window is found
  
  'We alter the title to compare it case-insensitively.
  TitlePart = UCase$(TitlePart)
  
  'loop through open windows and find the right one.
  hWndTmp = FindWindow(0&, 0&)
  
  Do Until hWndTmp = 0
    TitleTmp = VBA.Space$(256)
    nRet = GetWindowText(hWndTmp, TitleTmp, Len(TitleTmp))
    If nRet <> 0 And hWndTmp <> notHwnd Then
      TitleTmp = UCase(VBA.Left$(TitleTmp, nRet))
      If InStr(TitleTmp, TitlePart) Then
        FindWindowPartial = hWndTmp
        Exit Do
      End If
    End If
    hWndTmp = GetWindow(hWndTmp, GW_HWNDNEXT)
  Loop
End Function

'WinCaption is a substring to search all window captions for
'WinCommand is one of the constants SW_* defined above
'skipHwnd   is a window that should be skipped in the search even if its caption matches
Sub ShowWin(WinCaption$, WinCommand As ShowWindowCommand, skipHwnd&)
  Dim CurhWnd&, i&
  CurhWnd = FindWindowPartial(WinCaption, skipHwnd)
  If CurhWnd <> 0 Then i = ShowWindow(CurhWnd, WinCommand)
End Sub

Sub CloseWin(WinCaption$)
  Dim CurhWnd&, i&
  CurhWnd = FindWindowPartial(WinCaption, 0)
  If CurhWnd <> 0 Then i = SendMessage(CurhWnd, WM_CLOSE, &O0, &O0)
End Sub

