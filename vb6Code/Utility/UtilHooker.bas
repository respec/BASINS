Attribute VB_Name = "Hooker"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private hKeyboardHook As Long
Private KeystrokesRecorded As String
Private ShiftDown As Boolean
Private CtrlDown As Boolean
Private AltDown As Boolean

Public Sub HookKeyboard()
  KeystrokesRecorded = ""
  hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD, _
                           AddressOf KeyboardProc, _
                           0&, _
                           App.ThreadID)
End Sub

Public Sub UnhookKeyboard()
  If hKeyboardHook <> 0 Then Call UnhookWindowsHookEx(hKeyboardHook)
  hKeyboardHook = 0
End Sub

Public Function GetRecordedKeystrokes() As String
  GetRecordedKeystrokes = KeystrokesRecorded
End Function

Public Function KeyboardProc(ByVal nCode As Long, _
                             ByVal wParam As Long, _
                             ByVal lParam As Long) As Long
  'static variables for keeping track of last key so we can compare it to the current one
  'Note that LastKeyLen and LastKeyName are not used quite as might be expected
  Static LastKeyName$, LastKeyLen&, RepeatCount&, KeystrokesLen&
  Dim keyName64 As String * 64, keyName$, UkeyName$, modifiers$
  Dim nameLen&
  Dim Press As Boolean
  If nCode = 0 Then
    GetKeyNameText lParam, keyName64, 64
    nameLen = 0
    While nameLen < 64 And Asc(Mid(keyName64, nameLen + 1, 1)) >= 32
      nameLen = nameLen + 1
    Wend
    keyName = Left(keyName64, nameLen)
    UkeyName = UCase(keyName)
    If (lParam And KB_RELEASE_MASK) = 0 Then Press = True Else Press = False
    If Not Press Then
      Select Case keyName
        Case "Alt":         AltDown = False
        Case "Ctrl":        CtrlDown = False
        Case "Shift":       ShiftDown = False
        Case "Right Shift": ShiftDown = False
        Case "Left Shift":  ShiftDown = False
      End Select
    Else
      If Len(keyName) > 1 Then
        Select Case keyName
          Case "Shift", "Right Shift", "Left Shift"
                              ShiftDown = True: UkeyName = ""
          Case "Alt":         AltDown = True:   UkeyName = ""
          Case "Ctrl":        CtrlDown = True:  UkeyName = ""
          
          Case "Num Lock":    UkeyName = "{NUMLOCK}"
          Case "Caps Lock":   UkeyName = "{CAPSLOCK}"
          Case "Scroll Lock": UkeyName = "{SCROLLLOCK}"
          
          Case "Page Up":     UkeyName = "{PGUP}"
          Case "Page Down":   UkeyName = "{PGDN}"
          Case "Space":       UkeyName = "{ }"
      
          Case Else: UkeyName = "{" & UkeyName & "}"
          
        End Select
      Else
        Select Case UkeyName
          Case "'", "~", "+", "(", ")", "[", "]", "{", "}" 'characters that need to be quoted for SendKeys
            UkeyName = "{" & UkeyName & "}"
        End Select
      End If
      If Len(UkeyName) > 0 Then
        modifiers = ""
        If AltDown Then modifiers = vbCrLf & "%"
        If ShiftDown Then modifiers = modifiers & "+"
        If CtrlDown Then UkeyName = modifiers & "^"
        
        'Turn a series of the same keystroke into one e.g. {UP}{UP}{UP} -> {UP 3}
        If UkeyName <> "{ }" And modifiers & UkeyName = LastKeyName Then
          RepeatCount = RepeatCount + 1
          If Left(UkeyName, 1) = "{" Then
            UkeyName = modifiers & Left(UkeyName, Len(UkeyName) - 1) & " " & RepeatCount & "}"
          Else
            UkeyName = modifiers & UkeyName & "{" & UkeyName & " " & RepeatCount & "}"
          End If
          KeystrokesRecorded = Left(KeystrokesRecorded, KeystrokesLen - LastKeyLen) & UkeyName
          KeystrokesLen = KeystrokesLen - LastKeyLen
          LastKeyLen = Len(UkeyName)
          KeystrokesLen = KeystrokesLen + LastKeyLen
        Else
          LastKeyName = modifiers & UkeyName
          LastKeyLen = Len(LastKeyName)
          KeystrokesRecorded = KeystrokesRecorded & LastKeyName
          KeystrokesLen = KeystrokesLen + LastKeyLen
          RepeatCount = 1
        End If
        
      End If
    End If
  'Else
  '  GetKeyNameText lParam, keyName64, 16
  '  Debug.Print "keyName=" & keyName64 & " wParam=" & wParam & " lParam=" & lParam
  End If
  KeyboardProc = CallNextHookEx(hKeyboardHook, nCode, wParam, lParam)
End Function

