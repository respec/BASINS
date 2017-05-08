VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoDebug"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Public Sub Show(Optional c$)
  
  If App.NonModalAllowed Then
    DbgMsg "Show:NonModal:Caption:" & Not IsMissing(c), 7, "ATCoDebug", "t"
    frmDebug.Show
  Else
    DbgMsg "Show:Modal:Caption:" & Not IsMissing(c), 7, "ATCoDebug", "t"
    frmDebug.Show vbModal
  End If
  If Not (IsMissing(c)) Then
    frmDebug.Caption = c & " Debug"
    DbgMsg "Show:Caption:" & c, 7, "ATCoDebug", "t"
  End If
  frmDebug.txtLev = lev
  frmDebug.txtFlsh = flsh
  ReDo True
  
End Sub

'str = message
'level = 1 (very important) ... 7 (not very important) ... maxint (totally unimportant)
'modul = section of program (AtCoText, GenScn, ...)
'typ = type of event (M = Mouse, K = Keyboard, P = property change, C = computation
Public Sub add(str$, ByVal level&, Optional modul$, Optional typ$)

  If IsMissing(typ) Then
    If IsMissing(modul) Then
      DbgMsg str, level
    Else
      DbgMsg str, level, modul
    End If
  Else
    DbgMsg str, level, modul, typ
  End If
    
End Sub

Private Sub UserControl_Initialize()
    
  flsh = 8
  lev = 3
  DbgMsg "Initialize", 7, "ATCoDebug", "i"
    
End Sub

Public Property Get level&()
  level = lev
End Property

'Public Property Let Level(ByVal vNewValue&)
'  lev = Level
'End Property
