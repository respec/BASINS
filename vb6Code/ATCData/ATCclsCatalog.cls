VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsCatalog"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_SUMMARY Abstract class used to implement component plugins that are _
 found at runtime.
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

'##SUMMARY Collection of Type ATCPlugIn describing available components
Public Property Get Plugins() As Collection
End Property

