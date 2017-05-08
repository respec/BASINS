Attribute VB_Name = "modDownloadMain"
Option Explicit

Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

Private Manager As clsWebDataManager

Sub Main()
  Dim curStep As String
  On Error GoTo ErrHand
  
  If App.PrevInstance Then
    MsgBox "Another copy of BASINS Data Download is already running." & vbCr _
         & "Only one copy can be running at a time." & vbCr _
         & "You may need to use Task Manager if you cannot see the other copy.", vbCritical, "BASINS Data Download"
  Else
  
    curStep = "Set Manager = New clsWebDataManager"
    Set Manager = New clsWebDataManager
    
    If Len(Command) > 0 Then
      On Error GoTo NoSuchFile
      If Not (GetAttr(Command) And vbDirectory) Then
        Manager.CurrentStatusFromFile Command
      End If
    End If
NoSuchFile:
    On Error GoTo ErrHand
    
    curStep = "Manager.SelectDataType"
    Manager.SelectDataType
    While Manager.State < 1000
      DoEvents
      Sleep 50
    Wend
  
  End If
  
  Exit Sub
  
ErrHand:
  MsgBox curStep & vbCr & Err.Description, vbOKOnly, "BASINS Data Download Main"
End Sub
