VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoTSgraph"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Public Sub Show(Data As Collection)
  MsgBox "This graph type is not currently supported", vbOKOnly, "ATCoTSgraph"
  'Dim frm As frmTSgraph
  'Set frm = New frmTSgraph
  'Set frm.Data = Data
End Sub
