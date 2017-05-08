VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoTSlist"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

'Private pStartDate As Double
'Private pEndDate As Double
Private pOpenFiles As Collection
Private pAttributes As Collection

Private frm As frmTSlist
Attribute frm.VB_VarHelpID = -1

Public Event CreatedTser(newTS As ATCclsTserData)
Public Event Edit()

Public Sub RaiseCreatedTser(newTS As ATCclsTserData)
  RaiseEvent CreatedTser(newTS)
End Sub

Public Sub RaiseEdit()
  RaiseEvent Edit
End Sub

'Public Property Get StartDate() As Double
'  StartDate = pStartDate
'End Property
'Public Property Let StartDate(ByVal newvalue As Double)
'  pStartDate = newvalue
'End Property
'
'Public Property Get EndDate() As Double
'  EndDate = pEndDate
'End Property
'Public Property Let EndDate(ByVal newvalue As Double)
'  pEndDate = newvalue
'End Property


Public Property Get Attributes() As Collection 'of ATCclsTserFile
  Set Attributes = pAttributes
End Property
Public Property Set Attributes(newvalue As Collection) 'of ATCclsTserFile
  Set pAttributes = Nothing
  Set pAttributes = newvalue
End Property

Public Property Let HelpFileName(Filename As String)
  App.HelpFile = Filename
End Property

'If a timeseries is edited or a new one is created,
'OpenFiles are the default places where they can be saved
Public Property Get OpenFiles() As Collection 'of ATCclsTserFile
  Set OpenFiles = pOpenFiles
End Property
Public Property Set OpenFiles(newvalue As Collection) 'of ATCclsTserFile
  Set pOpenFiles = Nothing
  Set pOpenFiles = newvalue
End Property

Public Sub Show(Tser As ATCclsTserData)
  Set frm = New frmTSlist
  Set frm.ListClass = Me
  Set frm.OpenFiles = pOpenFiles
  frm.Add Tser
  frm.Show
  frm.PopulateGrid
End Sub

Public Sub ShowColl(tsColl As Collection) 'of ATCclsTserData
  Set frm = New frmTSlist
  Set frm.ListClass = Me
  Set frm.VisibleAttributes = pAttributes
  Set frm.OpenFiles = pOpenFiles
  Set frm.AllTSer = tsColl
  frm.Show
  frm.PopulateGrid
End Sub
