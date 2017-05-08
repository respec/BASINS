VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsLaunchWrap"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim WithEvents pLaunch As AtCoLaunch
Attribute pLaunch.VB_VarHelpID = -1
Private pMonitorName$

Public Property Get Launch() As AtCoLaunch
  Set Launch = pLaunch
End Property
Public Property Set Launch(newValue As AtCoLaunch)
  Set pLaunch = newValue
End Property

Private Sub pLaunch_Terminated(ExitCode As Long)
  If ExitCode = -99 Then MsgBox pLaunch.ComputeStatus, vbExclamation, "Launch Problem"
End Sub

Private Sub Class_Initialize()
  Set pLaunch = New AtCoLaunch
End Sub
