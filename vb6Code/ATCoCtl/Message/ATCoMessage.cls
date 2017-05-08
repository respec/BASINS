VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoMessage"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

'Returns message and title that were displayed, button number (first = 0) and button label
Public Event ButtonPress(Message As String, title As String, Button As Long, buttonLabel As String)

Private WithEvents frm As frmMsg
Attribute frm.VB_VarHelpID = -1
Private pIcon As StdPicture
Private pButtonOnLostFocus As Long
Private pButtonPressed As Long
Private pRegApp$
Private pRegSection$
Private pRegKey$

'The assigned icon will be used on subsequent message boxes
'newIcon is a picture or icon property of a control or a value returned by LoadPicture
Public Property Set Icon(newIcon As StdPicture)
  Set pIcon = newIcon
End Property
Public Property Get Icon() As StdPicture
  Set Icon = pIcon
End Property

'ButtonOnLostFocus is the index of the button to press when the form loses focus
' 1=first button, 0 = do not press a button when form loses focus
Public Property Let ButtonOnLostFocus(NewValue As Long)
  pButtonOnLostFocus = NewValue
End Property
Public Property Get ButtonOnLostFocus() As Long
  ButtonOnLostFocus = pButtonOnLostFocus
End Property

'Opens the message window
'
'Message is the label in the main part of the window
'Title is the caption in the title bar
'ButtonName is any number of names for buttons to be displayed along the bottom
'
'Returns the index (1..number of ButtonNames) of the button that was pressed
Public Function Show(Message$, title$, ParamArray ButtonName()) As Long
  Dim regValue&
  Dim lButtonName()
  
  lButtonName = ButtonName
  Show = ShowArray(Message, title, lButtonName)
End Function

'Same as Show except button names are passed in a user-supplied array instead of a variable number of args
Public Function ShowArray(Message$, title$, ButtonName()) As Long
  Dim regValue&
  
  If pRegApp = "" Or pRegSection = "" Or pRegKey = "" Then
    regValue = 0
  Else
    regValue = GetSetting(pRegApp, pRegSection, pRegKey, 0)
  End If
  If regValue > 0 Then
    ShowArray = regValue
  Else
    On Error Resume Next
    Unload frm
    Set frm = Nothing
    Set frm = New frmMsg
    Set frm.Icon = pIcon
    On Error GoTo 0
    pButtonPressed = -1
    frm.ShowMessage Message, title, pButtonOnLostFocus, pRegApp, pRegSection, pRegKey, ButtonName
    While pButtonPressed < 0
      DoEvents
    Wend
    ShowArray = pButtonPressed
  End If

End Function

'Opens the message window and does not wait for user to press a button
'
'Message is the label in the main part of the window
'Title is the caption in the title bar
'ButtonName is any number of names for buttons to be displayed along the bottom
'
'ButtonPress event can alert app when button is pressed
Public Sub ShowNoWait(Message$, title$, ParamArray ButtonName())
  Dim lButtonName(), regValue&
  
  If pRegApp = "" Or pRegSection = "" Or pRegKey = "" Then
    regValue = 0
  Else
    regValue = GetSetting(pRegApp, pRegSection, pRegKey, 0)
  End If
  If regValue > 0 Then
    RaiseEvent ButtonPress(Message, title, regValue, CStr(ButtonName(regValue)))
  Else
    On Error Resume Next
    Unload frm
    Set frm = Nothing
    Set frm = New frmMsg
    Set frm.Icon = pIcon
    On Error GoTo 0
    lButtonName = ButtonName
    frm.ShowMessage Message, title, pButtonOnLostFocus, pRegApp, pRegSection, pRegKey, lButtonName
  End If

End Sub

' a checkbox allowing the user to avoid seeing this message again
'
'regApp, regSection, regKey form the registry key to check and/or set
' The registry entry used is
' HKEY_CURRENT_USER\Software\VB and VBA Program Settings\regApp\regSection\regKey
'
' If the key exists and its value is greater than zero, the button whose index
'    matches the value will be pressed automatically and the message will not be shown
' If the key exists and its value is <= zero or if the key does not exist,
'    a checkbox will be displayed allowing the user to change the value to one
' It is the calling program's responsibility to give users the ability to change
' the registry key's value back to nonzero and/or remove it from the registry.
'
Public Sub CheckboxRegKey(regApp$, regSection$, regKey$)
  pRegApp = regApp
  pRegSection = regSection
  pRegKey = regKey
End Sub

Public Sub Hide()
  On Error Resume Next
  Unload frm
End Sub

'Call ShowMessage before this
'Waits for a button to be pressed, then
'Returns the number of the button that was pressed
'Public Function WaitButtonPressNumber() As Long
'  On Error GoTo Err
'  WaitButtonPressNumber = frm.WaitButtonPressNumber
'  Exit Function
'Err:
'  WaitButtonPressNumber = 0
'End Function

'Call ShowMessage before this
'Waits for a button to be pressed, then
'Returns the label of the button that was pressed
'Public Function WaitButtonPressLabel() As String
'  On Error GoTo Err
'  WaitButtonPressLabel = frm.WaitButtonPressLabel
'  Exit Function
'Err:
'  WaitButtonPressLabel = Err.Description
'End Function

Private Sub Class_Initialize()
  Set pIcon = LoadPicture
  pButtonOnLostFocus = -1
  pRegApp = ""
  pRegSection = ""
  pRegKey = ""
End Sub

Private Sub frm_ButtonPress(Message As String, title As String, Button As Long, buttonLabel As String)
  RaiseEvent ButtonPress(Message, title, Button, buttonLabel)
  pButtonPressed = Button
  'pButtonPressedLabel = buttonLabel
End Sub
