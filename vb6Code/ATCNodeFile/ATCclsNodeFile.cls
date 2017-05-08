VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsNodeFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Public Property Set Monitor(ByVal o As Object)
End Property

Public Property Let HelpFilename(ByVal newvalue$)
End Property

Public Property Get ErrorDescription() As String
End Property

Public Property Get FileExtension() As String
End Property

Public Property Get Label() As String
End Property

Public Property Get filename() As String
End Property

Public Property Let filename(ByVal newvalue$)
End Property

Public Property Get Description() As String
End Property

Public Property Get Nodes() As Collection
End Property

Public Sub Clear()
End Sub

Public Sub refresh()
End Sub

Public Sub WriteMapFile(filename As String)
End Sub

Public Sub WriteShapeFile(basename As String)
End Sub

