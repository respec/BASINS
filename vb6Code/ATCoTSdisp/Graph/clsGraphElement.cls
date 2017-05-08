VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsGraphElement"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Public Sub Draw(graph As ATCoTSgraph)
End Sub

Public Property Get Xaxis() As clsGraphElement
End Property
Public Property Set Xaxis(newValue As clsGraphElement)
End Property

Public Property Get Yaxis() As clsGraphElement
End Property
Public Property Set Yaxis(newValue As clsGraphElement)
End Property

Public Property Get Zaxis() As clsGraphElement
End Property
Public Property Set Zaxis(newValue As clsGraphElement)
End Property

Public Property Get Label() As String
End Property

Public Property Get Xmin() As Double
End Property
Public Property Get Xmax() As Double
End Property
Public Property Get Ymin() As Double
End Property
Public Property Get Ymax() As Double
End Property
Public Property Get Zmin() As Double
End Property
Public Property Get Zmax() As Double
End Property

