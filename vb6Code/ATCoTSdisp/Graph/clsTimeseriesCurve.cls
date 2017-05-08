VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTimeseriesCurve"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsGraphElement

Private pLabel As String
Private pXaxis As clsGraphElement
Private pYaxis As clsGraphElement
Private pZaxis As clsGraphElement

Private pXmax As Double
Private pXmin As Double
Private pYmax As Double
Private pYmin As Double
Private pZmax As Double
Private pZmin As Double

Private Sub clsGraphElement_Draw(graph As ATCoTSgraph)

End Sub

Private Property Get clsGraphElement_Label() As String
  clsGraphElement_Label = pLabel
End Property

Private Property Get clsGraphElement_Xaxis() As clsGraphElement
  Set clsGraphElement_Xaxis = pXaxis
End Property
Private Property Set clsGraphElement_Xaxis(newValue As clsGraphElement)
  Set pXaxis = newValue
End Property

Private Property Get clsGraphElement_Yaxis() As clsGraphElement
  Set clsGraphElement_Yaxis = pYaxis
End Property
Private Property Set clsGraphElement_Yaxis(newValue As clsGraphElement)
  Set pYaxis = newValue
End Property

Private Property Get clsGraphElement_Zaxis() As clsGraphElement
  Set clsGraphElement_Zaxis = pZaxis
End Property
Private Property Set clsGraphElement_Zaxis(newValue As clsGraphElement)
  Set pZaxis = newValue
End Property

Private Property Get clsGraphElement_Xmax() As Double
  clsGraphElement_Xmax = pXmax
End Property
Private Property Get clsGraphElement_Xmin() As Double
  clsGraphElement_Xmin = pXmin
End Property

Private Property Get clsGraphElement_Ymax() As Double
  clsGraphElement_Ymax = pYmax
End Property
Private Property Get clsGraphElement_Ymin() As Double
  clsGraphElement_Ymin = pYmin
End Property

Private Property Get clsGraphElement_Zmax() As Double
  clsGraphElement_Zmax = pZmax
End Property
Private Property Get clsGraphElement_Zmin() As Double
  clsGraphElement_Zmin = pZmin
End Property

