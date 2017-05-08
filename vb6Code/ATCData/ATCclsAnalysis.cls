VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsAnalysis"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_SUMMARY Abstract class used for timeseries analysis tools _
 found at runtime.
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

'##SUMMARY Sets status monitor to an existing instance of a status monitor.
Public Property Set Monitor(ByVal o As Object)
End Property

'##SUMMARY Text description of most recent error that occurred within _
 class.
Public Property Get ErrorDescription() As String
End Property

'##SUMMARY Returns text description of Analysis tool.
Public Property Get Description() As String
End Property

'##SUMMARY Collection of timeseries data to be analyzed.
Public Property Set DataCollection(coll As Collection) 'of ATCclsTserData
End Property

'##SUMMARY Use the tool to do the analysis.
Public Sub Go()
End Sub

'##RETURNS Returns False if user cancelled
'##SUMMARY Method to edit specifications for the analysis.
Public Function EditSpecification() As Boolean
End Function

'##SUMMARY String containing specifications used by the tool.
Public Property Get Specification() As String
End Property
Public Property Let Specification(newValue As String)
End Property

