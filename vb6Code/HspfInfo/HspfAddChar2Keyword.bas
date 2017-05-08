Attribute VB_Name = "HspfAddChar2Keyword"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Global lastOperationSerial As Long
Global IPC As ATCoIPC
Global IPCset As Boolean

Function AddChar2Keyword(k As String) As String
  Dim kwd$
              
  kwd = k
  Select Case kwd
    Case "MON-IFLW-CON": kwd = kwd & "C"
    Case "MON-GRND-CON": kwd = kwd & "C"
    Case "PEST-AD-FLAG": kwd = kwd & "S"
    Case "PHOS-AD-FLAG": kwd = kwd & "S"
    Case "TRAC-AD-FLAG": kwd = kwd & "S"
    Case "PLNK-AD-FLAG": kwd = kwd & "S"
    Case "HYDR-CATEGOR": kwd = kwd & "Y"
    Case Else
  End Select
  AddChar2Keyword = kwd
End Function
