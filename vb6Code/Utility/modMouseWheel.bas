Attribute VB_Name = "modMouseWheel"
Option Explicit

Private Type POINTAPI
    X As Long
    Y As Long
End Type

Private Declare Function CallWindowProc Lib "user32.dll" Alias "CallWindowProcA" (ByVal lpPrevWndFunc As Long, ByVal hwnd As Long, ByVal Msg As Long, ByVal Wparam As Long, ByVal Lparam As Long) As Long
Private Declare Function SetWindowLong Lib "user32.dll" Alias "SetWindowLongA" (ByVal hwnd As Long, ByVal nIndex As Long, ByVal dwNewLong As Long) As Long
Private Declare Function ScreenToClient Lib "user32" (ByVal hwnd As Long, lpPoint As POINTAPI) As Long

Public Const MK_LBUTTON = &H1
Public Const MK_RBUTTON = &H2
Public Const MK_MBUTTON = &H10
Public Const MK_CONTROL = &H8
Public Const MK_SHIFT = &H4

Private Const GWL_WNDPROC = -4
Private Const WM_MOUSEWHEEL = &H20A

Private pHwnd As Long
Private pSaveWndProc As Long
Private pForm As Form

Private Function WindowProc(ByVal Lwnd As Long, ByVal Lmsg As Long, ByVal Wparam As Long, ByVal Lparam As Long) As Long
  Dim MouseKeys As Long
  Dim Rotation As Long
  Dim Xpos As Long
  Dim Ypos As Long
  Dim pt As POINTAPI
  If Lmsg = WM_MOUSEWHEEL Then
    MouseKeys = Wparam And 65535
    Rotation = Wparam / 65536
    pt.X = Lparam And 65535
    pt.Y = Lparam / 65536
    ScreenToClient pForm.hwnd, pt
    pForm.MouseWheel MouseKeys, Rotation, pt.X, pt.Y
  End If
  WindowProc = CallWindowProc(pSaveWndProc, Lwnd, Lmsg, Wparam, Lparam)
End Function

Public Sub WheelHook(aForm As Form)
  On Error Resume Next
  Set pForm = aForm
  pHwnd = pForm.hwnd
  pSaveWndProc = SetWindowLong(pHwnd, GWL_WNDPROC, AddressOf WindowProc)
End Sub

Public Sub WheelUnHook()
  Dim WorkFlag As Long
  On Error Resume Next
  WorkFlag = SetWindowLong(pHwnd, GWL_WNDPROC, pSaveWndProc)
  Set pForm = Nothing
End Sub

