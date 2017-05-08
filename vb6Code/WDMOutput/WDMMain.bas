Attribute VB_Name = "WDMMain"
Option Explicit
Global TserFiles As ATCPlugInManager
Global Const TserFileClassName = "ATCTSfile"
Global HspfMsgUnit&
Global ProjectWDMFile As ATCclsTserFile

Sub Main()
  Dim hdle&, s As String * 80, EXEName$, EXEPath$, i&
  
  'open unit 99 for messages
  Call F90_W99OPN
  'MsgBox App.EXEName
  hdle = GetModuleHandle("WDMOutput")
  i = GetModuleFileName(hdle, s, 80)
  If Len(s) = 0 Then
    MsgBox "Could not find EXEPath for WDMOutput"
    Exit Sub
  ElseIf InStr(UCase(s), "VB6.EXE") > 0 Then
    EXEName = "c:\vbexperimental\WDMOutput\WDMOutput.exe"
  Else
    EXEName = UCase(Left(s, InStr(s, Chr(0)) - 1))
  End If
  EXEPath = PathNameOnly(EXEName) & "\"
  'open the message wdm file
  OpenHSPFMsgWDM EXEPath & "hspfmsg.wdm"

  frmWDMOut.Show
End Sub

Private Sub OpenHSPFMsgWDM(Name$)
  HspfMsgUnit = F90_WDBOPN(1, Name, Len(Name))
End Sub

