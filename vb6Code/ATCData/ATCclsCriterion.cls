VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsCriterion"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pField As String
Private pOperation As ATCCompare
Private pValues As Collection

'##SUMMARY For Operation atEQ, testValue must be equal to a value in _
 Values collection.  For all other operations, the expression testValue _
 [Operation] value must be true for each value _
 in Values.  For atGT, atLT, atGE, atLE there will probably be only _
 one value in Values.
Public Function Match(testValue As Variant) As Boolean
  Dim Item As Variant
  Match = False
  Select Case pOperation
    Case atEQ
              For Each Item In pValues
                If testValue = Item Then Match = True: Exit Function
              Next
    Case atLT
              For Each Item In pValues
                If testValue >= Item Then Exit Function
              Next
    Case atGT
              For Each Item In pValues
                If testValue <= Item Then Exit Function
              Next
    Case atLE
              For Each Item In pValues
                If testValue > Item Then Exit Function
              Next
    Case atGE
              For Each Item In pValues
                If testValue < Item Then Exit Function
              Next
    Case atNE
              For Each Item In pValues
                If testValue = Item Then Exit Function
              Next
  End Select
  If pOperation <> atEQ Then Match = True
End Function

Public Function MatchField(obj As Variant) As Boolean
  MatchField = Match(CallByName(obj, pField, VbGet))
End Function

Public Property Get Field() As String
  Field = pField
End Property
Public Property Let Field(newValue As String)
  pField = newValue
End Property

Public Property Get Operation() As ATCCompare
Attribute Operation.VB_Description = "=, <, >, <=, >=, <>, !="
  Operation = pOperation
End Property
Public Property Let Operation(newValue As ATCCompare)
  pOperation = newValue
End Property

Public Property Get Values() As Collection
  Set Values = pValues
End Property
Public Property Let Values(newValues As Collection)
  Set pValues = newValues
End Property

Private Sub Class_Initialize()
  Operation = atEQ
  Set pValues = New Collection
End Sub
