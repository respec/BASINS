VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTserStock"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Implements ATCclsTserFile

Private pFileName As String
Private pFileExt As String
Private p3CharExt As String
Private pErrorDescription As String
Private pData As Collection
Private pDates As Collection

Private pMonitor As Object
Private pMonitorSet As Boolean

Private Type arraSng
  a() As Single
End Type

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pDates = Nothing
End Sub

Private Property Get ATCclsTserFile_Data(index As Long) As ATCclsTserData
  Set ATCclsTserFile_Data = pData(index)
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = pData.Count
End Property

Private Property Get ATCclsTserfile_DataCollection() As Collection
  Set ATCclsTserfile_DataCollection = pData
End Property

Private Property Get ATCclsTserFile_FileUnit() As Long
  ATCclsTserFile_FileUnit = 1
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCclsTserData)
  pData.Add dataObject
End Sub

Private Sub ATCclsTserFile_refresh()
  pErrorDescription = "Unable to refresh"
End Sub

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  pErrorDescription = "Unable to Write a Data Header"
  ATCclsTserFile_writeDataHeader = False
End Function

Private Sub Class_Initialize()
  pFileName = ""
  pFileExt = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
  pErrorDescription = ""
  pMonitorSet = False
  
  Set pData = Nothing
  Set pData = New Collection
  
  Set pDates = Nothing
  Set pDates = New Collection
End Sub

Private Property Set ATCclsTserFile_Monitor(ByVal o As Object)
  Set pMonitor = o
  pMonitorSet = True
End Property

Private Property Get ATCclsTserFile_ErrorDescription() As String
  ATCclsTserFile_ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

Private Property Get ATCclsTserFile_FileExtension() As String
  ATCclsTserFile_FileExtension = pFileExt
End Property

'Private Property Get ATCclsTserFile_ThreeCharExtension() As String
'  ATCclsTserFile_ThreeCharExtension = p3CharExt
'End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "Stock"
End Property

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = pFileName
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newValue$)
  'frmStock.Hide
  On Error GoTo 0
  If frmStock.txtURL <> newValue Then
    frmStock.Show
    Set frmStock.TserFileToAddTo = Me
    frmStock.URL = newValue
    If Left(newValue, 7) = "http://" Then frmStock.Retrieve
    While pFileName <> newValue
      DoEvents
    Wend
    frmStock.Hide
    Unload frmStock
  Else
    Dim buf As String, linebuf As String, valbuf As String
    Dim tmpDate As Date
    Dim cnt As Long, nData As Long
    Dim dateIndex As Long, dataIndex As Long
    Dim lData() As ATCData.ATCclsTserData
    Dim lDates As ATCData.ATCclsTserDate
    'Dim lDataHeader As ATCData.ATTimSerDataHeader
    Dim lDataValues() As arraSng
    Dim lDataFlags() As Long
    Dim lDateValues() As Double
    Dim lDateFlags() As Long
    If pMonitorSet Then
       pMonitor.SendMonitorMessage "(OPEN " & newValue & ")"
       pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
       pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
    End If
    
    buf = frmStock.txtResults
    linebuf = StrSplit(buf, vbLf, "")
    nData = -1
    While linebuf <> ""
      nData = nData + 1
      valbuf = StrSplit(linebuf, ",", "")
    Wend
    If nData > 0 Then
      cnt = 0
      While buf <> ""
        cnt = cnt + 1
        linebuf = StrSplit(buf, vbLf, "")
      Wend
      If cnt > 0 Then
        'cnt = cnt - 1           'use zero index for first date/value
        ReDim lData(nData)
        ReDim lDataValues(nData)
        ReDim lDateValues(cnt)
        ReDim lDateFlags(cnt)
        Set lDates = Nothing
        Set lDates = New ATCData.ATCclsTserDate
        buf = frmStock.txtResults
        linebuf = StrSplit(buf, vbLf, "")
        valbuf = StrSplit(linebuf, ",", "") 'discard Date header
        For dataIndex = 1 To nData
          ReDim lDataValues(dataIndex).a(cnt)
          valbuf = StrSplit(linebuf, ",", "")
          Set lData(dataIndex) = Nothing
          Set lData(dataIndex) = New ATCData.ATCclsTserData
          With lData(dataIndex).Header
            .id = dataIndex
            .desc = frmStock.txtSymbol & " " & valbuf
            .Loc = valbuf
            If UCase(valbuf) = "VOLUME" Then .Con = "Volume" Else .Con = "Price"
            .Sen = frmStock.txtSymbol
          End With
          'lData(dataIndex).Header = lDataHeader
        Next
        For dateIndex = cnt To 1 Step -1
          linebuf = StrSplit(buf, vbLf, "")
          tmpDate = CDate(StrSplit(linebuf, ",", ""))
          lDateValues(dateIndex) = MJD(Year(tmpDate), Month(tmpDate), Day(tmpDate))
          lDateFlags(dateIndex) = JDateIntrvl(lDateValues(dateIndex))
          For dataIndex = 1 To nData
            valbuf = StrSplit(linebuf, ",", "")
            lDataValues(dataIndex).a(dateIndex) = valbuf
          Next
        Next
        With lDates
          .Values = lDateValues
          .flags = lDateFlags
          .calcSummary
          Set .File = Me
          pDates.Add lDates
        End With
        For dataIndex = 1 To nData
          With lData(dataIndex)
            Set .Dates = lDates
            .Values = lDataValues(dataIndex).a
            .calcSummary
            Set .File = Me
            pData.Add lData(dataIndex)
          End With
        Next
      End If
    End If
    pFileName = newValue
    If pMonitorSet Then
       pMonitor.SendMonitorMessage "(CLOSE)"
       pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
       pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
    End If
  End If
End Property

Public Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "In-memory Time Series Data Type"
End Property

Private Function ATCclsTserFile_AddTimSer(t As ATCclsTserData, Optional ExistAction& = 0) As Boolean
  ATCclsTserFile_AddTimSer = True
  pData.Add t
End Function

Private Function ATCclsTserFile_RemoveTimSer(t As ATCclsTserData) As Boolean
  ATCclsTserFile_RemoveTimSer = False
  pErrorDescription = "RemoveTimSer not supported for " & ATCclsTserFile_Description
End Function

Private Function ATCclsTserFile_RewriteTimSer(t As ATCclsTserData) As Boolean
  ATCclsTserFile_RewriteTimSer = False
  pErrorDescription = "RewriteTimSer not supported for " & ATCclsTserFile_Description
End Function

Private Function ATCclsTserFile_SaveAs(Filename As String) As Boolean
  ATCclsTserFile_SaveAs = False
  pErrorDescription = "SaveAs not supported for " & ATCclsTserFile_Description
End Function

