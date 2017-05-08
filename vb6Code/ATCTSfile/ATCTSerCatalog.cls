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
'Copyright 2002 by AQUA TERRA Consultants
Implements ATCclsCatalog

Private Property Get ATCclsCatalog_Plugins() As Collection
  Dim pin As ATCPlugIn
  pin.version = "1.0"
  pin.PluginType = "ATCclsTserFile"
  Set ATCclsCatalog_Plugins = New Collection
  With ATCclsCatalog_Plugins
    pin.ClassName = "clsTSerWDM":       pin.Label = "WDM":       .Add pin
    pin.ClassName = "clsTSerBasObsWQ":  pin.Label = "BasObsWQ":  .Add pin
    pin.ClassName = "clsTSerEFDC":      pin.Label = "EFDC":      .Add pin
    pin.ClassName = "clsTSerFEO":       pin.Label = "FEO":       .Add pin
    pin.ClassName = "clsTSerPLTGEN":    pin.Label = "PLTGEN":    .Add pin
    pin.ClassName = "clsTSerRDB":       pin.Label = "RDB":       .Add pin
    pin.ClassName = "clsTSerSWATDBF":   pin.Label = "SWATDBF":   .Add pin
    pin.ClassName = "clsTSerSWMM":      pin.Label = "SWMM":      .Add pin
    pin.ClassName = "clsTSerMemory":    pin.Label = "Memory":    .Add pin
    'pin.ClassName = "clsTserWebGSdailyFlow":    pin.Label = "WebGSdailyFlow":    .Add pin
    'pin.ClassName = "clsTSerStock":     pin.Label = "Stock":     .Add pin
  End With
End Property
