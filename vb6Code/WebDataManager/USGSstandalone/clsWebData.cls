VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsWebData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

'Label for type of data, used in interface
Public Property Get Label() As String
End Property

Public Property Get Name() As String
End Property

'XML describing what can be returned by GetData
'and what needs to be specified to retrieve it
Public Property Get Provides() As DOMDocument
End Property

'XML describing what needs to be known to find data
'Public Property Get Criteria() As DOMDocument
'End Property

'XML DataDescription based on Criteria must be populated
'Returns True if successful, False if unsuccessful
Public Function GetData(ByVal Query As DOMDocument, ByRef Result As DOMDocument) As Boolean
End Function

Public Property Get Manager() As clsWebDataManager
End Property
Public Property Set Manager(newManager As clsWebDataManager)
End Property

'Returns a description of what went wrong during failed GetData
'Resets ErrorDescription to empty string
'Public Function ErrorDescription() As String
'End Function

Public Property Get Version() As String
End Property

Public Property Get HelpId() As Long
End Property

Public Sub Specify(Optional parent As Object = Nothing, Optional ByVal Index As Long = 0)
End Sub

