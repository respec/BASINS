VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestSeries"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pSeriesType As String '"t" (timeseries), "s" (s_table), "v" (v_table), "e" (e_table)
Private pObsSeries As String
Private pObsSeriesCom As String
Private pModelSeries As String
Private pModelSeriesCom As String
Private pWeightEquation As String
Private pWeightEquationCom As String
Private pWeightMin As Single
Private pWeightMax As Single
Private pWeightMinMaxCom As String

Public Property Get SeriesType() As String
  SeriesType = pSeriesType
End Property
Public Property Let SeriesType(newvalue As String)
  pSeriesType = newvalue
End Property

Public Property Get ObsSeries() As String
  ObsSeries = pObsSeries
End Property
Public Property Let ObsSeries(newvalue As String)
  pObsSeries = newvalue
End Property

Public Property Get ObsSeriesCom() As String
  ObsSeriesCom = pObsSeriesCom
End Property
Public Property Let ObsSeriesCom(newvalue As String)
  pObsSeriesCom = newvalue
End Property

Public Property Get ModelSeries() As String
  ModelSeries = pModelSeries
End Property
Public Property Let ModelSeries(newvalue As String)
  pModelSeries = newvalue
End Property

Public Property Get ModelSeriesCom() As String
  ModelSeriesCom = pModelSeriesCom
End Property
Public Property Let ModelSeriesCom(newvalue As String)
  pModelSeriesCom = newvalue
End Property

Public Property Get WeightEquation() As String
  WeightEquation = pWeightEquation
End Property
Public Property Let WeightEquation(newvalue As String)
  pWeightEquation = newvalue
End Property

Public Property Get WeightEquationCom() As String
  WeightEquationCom = pWeightEquationCom
End Property
Public Property Let WeightEquationCom(newvalue As String)
  pWeightEquationCom = newvalue
End Property

Public Property Get WeightMin() As Single
  WeightMin = pWeightMin
End Property
Public Property Let WeightMin(newvalue As Single)
  pWeightMin = newvalue
End Property

Public Property Get WeightMax() As Single
  WeightMax = pWeightMax
End Property
Public Property Let WeightMax(newvalue As Single)
  pWeightMax = newvalue
End Property

Public Property Get WeightMinMaxCom() As String
  WeightMinMaxCom = pWeightMinMaxCom
End Property
Public Property Let WeightMinMaxCom(newvalue As String)
  pWeightMinMaxCom = newvalue
End Property

Private Sub Class_Initialize()

  pSeriesType = ""
  pObsSeries = ""
  pObsSeriesCom = ""
  pModelSeries = ""
  pModelSeriesCom = ""
  pWeightEquation = ""
  pWeightEquationCom = ""
  pWeightMin = -1
  pWeightMax = -1
  pWeightMinMaxCom = ""

End Sub
