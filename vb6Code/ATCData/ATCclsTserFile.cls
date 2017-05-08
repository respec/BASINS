VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsTserFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_SUMMARY Abstract class used to implement&nbsp;timeseries.
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Property Set Monitor(ByVal o As Object)
End Property

Public Property Let HelpFilename(ByVal newValue$)
End Property

Public Property Get ErrorDescription() As String
End Property

Public Property Get FileExtension() As String
End Property

Public Property Get Label() As String
End Property

Public Property Get Filename() As String
End Property

Public Property Let Filename(ByVal newValue$)
End Property

Public Property Get FileUnit() As Long
End Property

Public Property Get Description() As String
End Property

Public Property Get AvailableAttributes() As Collection
End Property

Public Property Get Data(index&) As ATCclsTserData
End Property

Public Property Get DataCount() As Long
End Property

Public Property Get DataCollection() As Collection
End Property

Public Sub readData(dataObject As ATCclsTserData)
End Sub

Public Sub Clear()
End Sub

Public Sub refresh()
End Sub

'oldHeader should contain the old values, new values should have already been changed in dataObject
'oldheader is usually not needed except in WDM when id (dsn) has been changed
Public Function WriteDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
End Function

Public Function AddTimSer(t As ATCclsTserData, Optional ExistAction As Long = 0) As Boolean
End Function

Public Function RemoveTimSer(t As ATCclsTserData) As Boolean
End Function

Public Function RewriteTimSer(t As ATCclsTserData) As Boolean
End Function

Public Function SaveAs(Filename As String) As Boolean
End Function
