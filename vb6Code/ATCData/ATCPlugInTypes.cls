VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCPlugInTypes"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Type ATCPlugIn
  'Use CreateObject(PluginName & "." & ClassName) to instantiate
  PluginName As String 'Name of ocx/dll
  ClassName As String  'Name of this object
  Version As String    'String to identify version of PlugIn
  Label As String      'Label for use in interface, defaults to catalog label
  PluginType As String 'Name of interface implemented
  ID As Long           'counter of created plugins
  obj As Object        'instantiated object
End Type
