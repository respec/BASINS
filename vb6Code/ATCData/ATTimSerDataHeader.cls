VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATTimSerDataHeader"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pDesc As String ' description
Private pId As Double    'identifier
Private pSen As String   'scnenario name
Private pLoc As String   'location name
Private pCon As String   'constituent name
  
'##SUMMARY Text description of timeseries
Public Property Get Desc() As String
  Desc = pDesc
End Property
Public Property Let Desc(newValue As String)
  pDesc = newValue
End Property

'##SUMMARY Identifier for timeseries (dsn if available).
Public Property Get ID() As Double
  ID = pId
End Property
Public Property Let ID(newValue As Double)
  pId = newValue
End Property

'##SUMMARY Scenario name
Public Property Get Sen() As String
  Sen = pSen
End Property
Public Property Let Sen(newValue As String)
  pSen = newValue
End Property

'##SUMMARY Location name
Public Property Get Loc() As String
  Loc = pLoc
End Property
Public Property Let Loc(newValue As String)
  pLoc = newValue
End Property

'##SUMMARY Constituent name
Public Property Get Con() As String
  Con = pCon
End Property
Public Property Let Con(newValue As String)
  pCon = newValue
End Property

'##SUMMARY Copy of this header
Public Function Copy() As ATTimSerDataHeader
  Dim retval As ATTimSerDataHeader
  
  Set retval = New ATTimSerDataHeader
  retval.Con = pCon
  retval.Desc = pDesc
  retval.ID = pId
  retval.Loc = Loc
  retval.Sen = Sen
  Set Copy = retval
End Function

'##SUMMARY Compare this header with another
Public Function Compare(nTsHeader As ATTimSerDataHeader) As Boolean
  Compare = False 'assume the worst
  If pCon <> nTsHeader.Con Then Exit Function
  If pSen <> nTsHeader.Sen Then Exit Function
  If pLoc <> nTsHeader.Loc Then Exit Function
  If pId <> nTsHeader.ID Then Exit Function
  Compare = True 'we passed!
End Function

Private Sub Class_Initialize()
  'Debug.Print "New ATTimSerDataHeader"
End Sub
