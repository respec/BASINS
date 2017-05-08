VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCreportSection"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Private pAttribute As String
Private pIsColumn As Boolean
Private pChildren As Collection
Private pFormat As ATCnumberFormat

Public Property Get Title() As String
End Property
Public Property Let Title(ByVal newValue As String)
End Property

Public Property Get SectionAttribute() As String
  SectionAttribute = pAttribute
End Property
Public Property Let SectionAttribute(newValue As String)
  pAttribute = SectionAttribute
End Property

Public Property Get Children() As Collection 'of clsATCreportSection
  Set Children = pChildren
End Property
Public Property Set Children(newValue As Collection) 'of clsATCreportSection
  Set pChildren = newValue
End Property

Public Property Get IsColumn() As Boolean
  IsColumn = pIsColumn
End Property
Public Property Let IsColumn(newValue As Boolean)
  pIsColumn = newValue
End Property

Public Property Get asString() As String
  Dim retval As String
  retval = Title
  If Len(retval) > 0 Then retval = retval & vbCrLf
  
End Property

Public Property Get Format() As ATCnumberFormat
  Set Format = pFormat
End Property
Public Property Set Format(newValue As ATCnumberFormat)
  Set pFormat = newValue
End Property

