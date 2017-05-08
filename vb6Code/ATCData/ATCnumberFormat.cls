VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCnumberFormat"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Private pWidth As Long
Private pSignificantDigits As Long
Private pDecimalPlaces As Long

Public Property Get Width() As Long
  Width = pWidth
End Property
Public Property Let Width(newValue As Long)
  pWidth = newValue
End Property

Public Property Get SignificantDigits() As Long
  SignificantDigits = pSignificantDigits
End Property
Public Property Let SignificantDigits(newValue As Long)
  pSignificantDigits = newValue
End Property

Public Property Get DecimalPlaces() As Long
  DecimalPlaces = pDecimalPlaces
End Property
Public Property Let DecimalPlaces(newValue As Long)
  pDecimalPlaces = newValue
End Property

