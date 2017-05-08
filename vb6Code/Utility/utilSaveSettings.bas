Attribute VB_Name = "utilSaveSettings"
Option Explicit

Private Const SectionRecentFiles = "Recent Files"
Private Const MaxRecentFiles = 9

'Saves registry settings for the left, top, width, height of a window
'Return value is always false
Public Function SaveWindowSettings(frm As Form, AppName As String, Optional WindowName As String = "Main Window") As Boolean
  On Error Resume Next 'No errors are critical here, ignore them
  With frm
    .WindowState = vbNormal 'Un-minimize or un-maximize
    If .Width > 800 And .Height > 800 _
      And .Top > 0 And .Top < Screen.Height _
      And .Left > 0 And .Left < Screen.Width Then
      SaveWindowSettings = True
      SaveSetting AppName, WindowName, "Width", .Width
      SaveSetting AppName, WindowName, "Height", .Height
      SaveSetting AppName, WindowName, "Left", .Left
      SaveSetting AppName, WindowName, "Top", .Top
    End If
  End With
End Function

'Gets registry settings for the left, top, width, height of a window
'Return value is always false
Public Function RetrieveWindowSettings(frm As Form, AppName As String, Optional WindowName As String = "Main Window") As Boolean
  Dim setting As Variant, rf&
  Dim Left As Single
  Dim Top As Single
  Dim Width As Single
  Dim Height As Single
  
  On Error Resume Next 'No errors are critical here, ignore them
  With frm
    Left = .Left
    Top = .Top
    Width = .Width
    Height = .Height
  End With
  
  setting = GetSetting(AppName, WindowName, "Left")
  If IsNumeric(setting) Then Left = setting
  If Left <= 0 Or Left >= Screen.Width Then Left = 100
  
  setting = GetSetting(AppName, WindowName, "Top")
  If IsNumeric(setting) Then Top = setting
  If Top <= 0 Or Top > Screen.Height * 0.9 Then Top = 100
  
  setting = GetSetting(AppName, WindowName, "Width")
  If IsNumeric(setting) Then Width = setting
  If Width < 200 Or Width + Left >= Screen.Width Then Width = (Screen.Width - Left) * 0.9
  
  setting = GetSetting(AppName, WindowName, "Height")
  If IsNumeric(setting) Then Height = setting
  If Height < 200 Or Height + Top >= Screen.Height Then Height = (Screen.Height - Top) * 0.9
  
  frm.Move Left, Top, Width, Height
  
End Function

'mnuRecent must be a control array of menu items
Public Sub RetrieveRecentFiles(mnuRecent As Object, AppName As String)
  Dim setting As Variant
  Dim rf&
  On Error Resume Next 'No errors are critical here, ignore them
  For rf = MaxRecentFiles To 1 Step -1
    setting = GetSetting(AppName, SectionRecentFiles, CStr(rf))
    If FileExists(setting) Then AddRecentFile mnuRecent, CStr(setting)
  Next rf
End Sub

Public Sub SaveRecentFiles(mnuRecent As Object, AppName As String)
  Dim rf&
  On Error Resume Next 'No errors are critical here, ignore them
  'Delete old recent files list
  DeleteSetting AppName, SectionRecentFiles
  'Save new recent files
  For rf = mnuRecent.Count - 1 To 1 Step -1
    SaveSetting AppName, SectionRecentFiles, CStr(rf), mnuRecent(rf).Tag
  Next rf
End Sub

Public Sub AddRecentFile(mnuRecent As Object, FilePath As String)
  Dim rf&, rfMove&, newPath$, match As Boolean
  On Error Resume Next
  rf = 0
  While Not match And rf <= mnuRecent.Count - 2
    rf = rf + 1
    If UCase(mnuRecent(rf).Tag) = UCase(FilePath) Then match = True
  Wend
  If match Then 'move file to top of list
    For rfMove = rf To 2 Step -1
      mnuRecent(rfMove).Tag = mnuRecent(rfMove - 1).Tag
      mnuRecent(rfMove).Caption = "&" & rfMove & " " & FilenameOnly(mnuRecent(rfMove).Tag)
    Next rfMove
  Else 'Add file to list
    mnuRecent(0).Visible = True
    If mnuRecent.Count <= MaxRecentFiles Then Load mnuRecent(mnuRecent.Count)
    For rfMove = mnuRecent.Count - 1 To 2 Step -1
      mnuRecent(rfMove).Tag = mnuRecent(rfMove - 1).Tag
      mnuRecent(rfMove).Caption = "&" & rfMove & " " & FilenameOnly(mnuRecent(rfMove).Tag)
    Next rfMove
  End If
  mnuRecent(1).Visible = True
  mnuRecent(1).Tag = FilePath
  mnuRecent(1).Caption = "&1 " & FilenameOnly(mnuRecent(rfMove).Tag)
End Sub

