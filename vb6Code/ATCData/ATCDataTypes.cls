VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCDataTypes"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Type ATTimSerDateSummary
  NVALS As Long      'count of timeseries values
  CIntvl As Boolean  'constant interval flag
  Intvl As Double    'interval of data
  ts As Long         'time step
  Tu As ATCTimeUnit  'time unit
  SJDay As Double    'SDat(6) As Long 'start date
  EJDay As Double    'EDat(6) As Long 'end date
End Type

Public Type ATTimSerAttribute
  Name As String
  Value As String
  Definition As ATCclsAttributeDefinition
End Type

Type ATCdetailEXEtype
  Label As String
  Value As String
  filter As String
  isFile As Boolean
  isOutput As Boolean
  isOnCommandline As Boolean
End Type

Type ATCexternalEXEtype
  Label As String
  path As String
  Details As Collection
End Type

Public Enum ATCTimeUnit
  TUSecond = 1
  TUMinute = 2
  TUHour = 3
  TUDay = 4
  TUMonth = 5
  TUYear = 6
  TUCentury = 7
End Enum

Public Enum ATCTran
  TranAverSame = 0
  TranSumDiv = 1
  TranMax = 2
  TranMin = 3
  TranNative = 4
End Enum

Public Enum ATCCompare
  atEQ
  atLT
  atGT
  atLE
  atGE
  atNE
End Enum

Public Enum ATCTsIdExistAction
  TsIdNoAction = 0
  TsIdRepl = 1
  TsIdAppend = 2
  TsIdRenum = 4
  TsIdReplAsk = 8
  TsIdAppendAsk = 16
  TsIdRenumAsk = 32
End Enum

'======-------- Backward compatibility stuff not used in new projects ----===
Public Type ATTimSerAttributes
  Name As String
  Value As String
End Type

Public Type ATTimSerJDateVals
  JDate As Double
  IntvlFg As Integer 'data interval (1:yr,2:mo,3:dy,4:hr,5:mn,6:sc)
End Type

Public Type ATTimSerJDate
  NVALS As Long 'count of timeseries values
  CIntvl As Boolean 'constant interval flag
  Intvl As Double 'interval of data
  j() As ATTimSerJDateVals
End Type

Public Type ATTimSerDataValues
  Vals As Single
  ValFg As Integer '0-original,1-agg,2-disagg,3-interp ???
End Type

Public Type ATTimSerDataSummary
  spos As Long 'starting position in whole span of data for current date (FEQ only)
  NVal As Long 'number of values
  CIntvl As Boolean 'constant interval flag
  JDIndex As Integer 'index in jd array of ATTimser
  ts As Long 'time step
  Tu As Long 'time unit
  Dtran As Long 'data transformation code (0-ave/same, 1-sum/div, 2-max, 3-min)
  SJDay As Double 'SDat(6) As Long 'start date
  EJDay As Double 'EDat(6) As Long 'end date
  Min As Single 'minimum value
  Max As Single 'maximum value
End Type

Public Type ATTimSerDetail
  Type As String 'WDM, FEQ, RDB
  FilIndex As Long ' index of WDM, FEO, or RDB file
  FileName As String ' may replace filindex??
  Stanam As String ' station name
  ID As Double 'identifier
  Sen As String 'scnenario name
  Loc As String 'location name
  Con As String 'constituent name
  s As ATTimSerDataSummary
  V() As ATTimSerDataValues 'array of values and assoc info
  Attrib() As ATTimSerAttributes
End Type

Public Type ATTimSer
  t() As ATTimSerDetail
  jd() As ATTimSerJDate 'array of julian days and interval info
End Type

'-------========= End backward compatibility section ======---------

Private Sub Class_Initialize()
  Debug.Print "Init ATCDataTypes"
End Sub


