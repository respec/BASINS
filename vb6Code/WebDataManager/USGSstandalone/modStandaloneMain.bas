Attribute VB_Name = "modStandaloneMain"
Option Explicit


Sub Main()
  Dim i As Long
  Dim pManager As clsWebDataManager
  Set pManager = New clsWebDataManager
  pManager.DataTypes("USGSdaily").Specify
  
  On Error GoTo ErrHand
StillOpen:
  Sleep 100
  DoEvents
  For i = 1 To pManager.collFrmCriteria.Count
    If pManager.collFrmCriteria.Item(i).Visible Then GoTo StillOpen
CheckNext:
  Next
  Set pManager = Nothing
  End

ErrHand:
  Resume CheckNext

End Sub
