VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCreport"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Implements ATCclsAnalysis

Private Const pLabel = "Basic Report"

Private pSpecification As String
Private pErrorDesc As String
Private pMonitor As Object
Private pMonitorSet As Boolean

Private pTitle As String
Private pData As Collection     'of ATCoTserData
Private pRows As FastCollection 'of clsATCreportSection
Private pCols As FastCollection 'of clsATCreportSection
Private pSummariesAvailable As FastCollection 'of String
Private pSummaryEnabled() As Boolean

Public Property Get Title() As String
  Title = pTitle
End Property
Public Property Let Title(ByVal newValue As String)
  pTitle = newValue
End Property

Private Property Get ATCclsAnalysis_DataCollection() As Collection
  Set ATCclsAnalysis_DataCollection = pData
End Property
Private Property Set ATCclsAnalysis_DataCollection(newColl As Collection)
  Set pData = newColl
End Property

Private Property Get ATCclsAnalysis_Description() As String
  ATCclsAnalysis_Description = pLabel
End Property

Private Function ATCclsAnalysis_EditSpecification() As Boolean
  SendMonitorMessage "(MSG1 Editing Specification for " & pLabel & ")"

  'frmReportSpecs.FormFg = FormFg
  frmReportSpecs.Title = Title
  'frmReportSpecs.CWid = CWid
  'frmReportSpecs.SDig = SDig
  'frmReportSpecs.dpla = dpla
  'Set frmReportSpecs.TSerColl = pColl
  If frmReportSpecs.EditReportSpecs Then
    'FormFg = frmReportSpecs.FormFg
    'Title = frmReportSpecs.Title
    'CWid = frmReportSpecs.CWid
    'SDig = frmReportSpecs.SDig
    'dpla = frmReportSpecs.dpla
    'PageSumm = frmReportSpecs.Summ(0)
    'ColSumm = frmReportSpecs.Summ(1)
    'RowSumm = frmReportSpecs.Summ(2)
    'FName = frmReportSpecs.FName
    ATCclsAnalysis_EditSpecification = True
  Else
    ATCclsAnalysis_EditSpecification = False
  End If
  Unload frmReportSpecs
End Function

Private Property Get ATCclsAnalysis_ErrorDescription() As String
  ATCclsAnalysis_ErrorDescription = pErrorDesc
  pErrorDesc = ""
End Property

Private Sub ATCclsAnalysis_Go()
  
End Sub

Public Property Get SummariesAvailable() As FastCollection 'of String
  Set SummariesAvailable = pSummariesAvailable
End Property
Public Property Set SummariesAvailable(newValue As FastCollection) 'of String
  Set pSummariesAvailable = newValue
End Property

'SummaryName is from SummariesAvailable
Public Property Get ReportSummaryEnabled(ByVal SummaryName As String) As Boolean
  Dim index As Long
  index = pSummariesAvailable.IndexFromKey(SummaryName)
  If index > 0 Then
    ReportSummaryEnabled = pSummaryEnabled(index)
  Else
    MsgBox "Unknown Summary Name: " & SummaryName, vbOKOnly, "Get Report Summary Enabled"
    ReportSummaryEnabled = False
  End If
End Property
Public Property Let ReportSummaryEnabled(ByVal SummaryName As String, ByVal makeEnabled As Boolean)
  Dim index As Long
  index = pSummariesAvailable.IndexFromKey(SummaryName)
  If index > 0 Then
    pSummaryEnabled(index) = makeEnabled
  Else
    MsgBox "Unknown Summary Name: " & SummaryName, vbOKOnly, "Get Report Summary Enabled"
  End If
End Property

Public Property Get Rows() As FastCollection 'of clsATCreportSection
  Set Rows = pRows
End Property

Public Property Get Cols() As FastCollection 'of clsATCreportSection
  Set Cols = pCols
End Property

Public Property Get asString() As String
  Dim retval As String
  Dim vRow As Variant
  retval = pTitle
  For Each vRow In pRows
    retval = retval & vRow.asString & vbCrLf
  Next
End Property

Private Property Set ATCclsAnalysis_Monitor(ByVal newValue As Object)
  Set pMonitor = newValue
  pMonitorSet = True
End Property

Private Sub SendMonitorMessage(msg As String)
  If pMonitorSet Then pMonitor.SendMonitorMessage msg
End Sub

Private Property Let ATCclsAnalysis_Specification(newValue As String)
  pSpecification = newValue
End Property
Private Property Get ATCclsAnalysis_Specification() As String
  ATCclsAnalysis_Specification = pSpecification
End Property

Private Sub Class_Initialize()
  Dim i As Long

  Set pSummariesAvailable = New FastCollection
  With pSummariesAvailable
    .Add "Ave"
    .Add "Sum"
    .Add "Min"
    .Add "Max"
    .Add "Cnt"
  End With
  
  Set pRows = New FastCollection
  Set pCols = New FastCollection
  
  ReDim pSummaryEnabled(pSummariesAvailable.Count)
  
  'By default we could enable all summaries
  'For i = 1 To pSummariesAvailable.Count
  '  pSummaryEnabled(i) = True
  'Next
End Sub
