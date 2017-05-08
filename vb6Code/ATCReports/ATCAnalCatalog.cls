VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATComponentCatalog"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Implements ATCclsCatalog

Private Property Get ATCclsCatalog_Plugins() As Collection
  Dim pin As ATCPlugIn
  pin.version = "1.0"
  pin.PluginType = "ATCclsAnalysis"
  Set ATCclsCatalog_Plugins = New Collection
  With ATCclsCatalog_Plugins
    pin.ClassName = "clsAnalysisReportUnit":       pin.Label = "Unit Load Report":       .Add pin
  End With
End Property
