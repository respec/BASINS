VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HSPFParmDef"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_SUMMARY Class containing definition of a model parameter.
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pName As String
Dim pTyp As Long 'atcoDataType is in control
Dim pMin As Double
Dim pMax As Double
Dim pDefault As String
Dim pMetricMin As Double
Dim pMetricMax As Double
Dim pMetricDefault As String
Dim pSoftMin As Double
Dim pSoftMax As Double
Dim pDefine As String
Dim pOther As String
Dim pStartCol As Long
Dim pLength As Long
Dim pParent As Object

'##SUMMARY Name of parameter.
Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property

'##SUMMARY Type of parameter.
Public Property Get Typ() As Long
  Typ = pTyp
End Property
Public Property Let Typ(newTyp As Long)
  pTyp = newTyp
End Property

'##SUMMARY Minimum value for parameter.
Public Property Get Min() As Double
  Min = pMin
End Property
Public Property Let Min(newMin As Double)
  pMin = newMin
End Property
Public Property Get MetricMin() As Double
  MetricMin = pMetricMin
End Property
Public Property Let MetricMin(newMetricMin As Double)
  pMetricMin = newMetricMin
End Property

'##SUMMARY Maximum value for parameter
Public Property Get Max() As Double
  Max = pMax
End Property
Public Property Let Max(newMax As Double)
  pMax = newMax
End Property
Public Property Get MetricMax() As Double
  MetricMax = pMetricMax
End Property
Public Property Let MetricMax(newMetricMax As Double)
  pMetricMax = newMetricMax
End Property

'##SUMMARY Devault value for parameter.
Public Property Get Default() As String
  Default = pDefault
End Property
Public Property Let Default(newDefault As String)
  pDefault = newDefault
End Property
Public Property Get MetricDefault() As String
  MetricDefault = pMetricDefault
End Property
Public Property Let MetricDefault(newMetricDefault As String)
  pMetricDefault = newMetricDefault
End Property

'##SUMMARY Recommended minimum value for parameter.
Public Property Get SoftMin() As Double
  SoftMin = pSoftMin
End Property
Public Property Let SoftMin(newSoftMin As Double)
  pSoftMin = newSoftMin
End Property

'##SUMMARY Recommended maximum value for parameter.
Public Property Get SoftMax() As Double
  SoftMax = pSoftMax
End Property
Public Property Let SoftMax(newSoftMax As Double)
  pSoftMax = newSoftMax
End Property

'##SUMMARY Text definition of parameter.
Public Property Get Define() As String
  Define = pDefine
End Property
Public Property Let Define(newDefine As String)
  pDefine = newDefine
End Property

'##SUMMARY Parent object of parameter definition.
Public Property Get Parent() As Object
  Set Parent = pParent
End Property
Public Property Set Parent(newParent As Object)
  Set pParent = newParent
End Property

'##SUMMARY Additional information about parameter.
Public Property Get Other() As String
  Other = pOther
End Property
Public Property Let Other(newOther As String)
  pOther = Other
End Property

'##SUMMARY Starting column for parameter within a text string.
Public Property Get StartCol() As Long
  StartCol = pStartCol
End Property
Public Property Let StartCol(newStartCol As Long)
  pStartCol = newStartCol
End Property

'##SUMMARY Length of parameter value when stored as a string.
Public Property Get Length() As Long
  Length = pLength
End Property
Public Property Let Length(NewLength As Long)
  pLength = NewLength
End Property

Private Sub Class_Initialize()
  'Debug.Print "init HSPFParmDef"
End Sub
