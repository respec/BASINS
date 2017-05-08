VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerHSPFBinary"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

'Copyright 2002 by AQUA TERRA Consultants
Implements ATCclsTserFile

Private pBinaryFileName As String
Private pMonitor As Object
Private pMonitorSet As Boolean
Private pBinFile As clsHspfBinary
Private pHSPFNetwork As clsNetworkHSPFOutput
Private pFileExt As String
Private pErrorDescription As String
Private pTSer As Collection 'of ATCTSer (filtered)

Private Function ATCclsTserFile_AddTimSer(t As ATCData.ATCclsTserData, Optional ExistAction As Long = 0&) As Boolean
  pErrorDescription = "Unable to Add a Time Series for " & ATCclsTserFile_Description
  ATCclsTserFile_AddTimSer = False
End Function

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Dim retval As Collection
  'Dim vAttribute As Variant
  'Dim lCurTSerAttr As ATCclsAttributeDefinition
  
  'needed to edit attributes? that can't be done for this type!
  'for now - just return nothing
  
  Set retval = New Collection
  'Set lCurTSerAttr = New ATCclsAttributeDefinition
  
  'If pHSPFOutput.DataCollection.Count > 0 Then
  '  For Each vAttribute In pHSPFOutput.DataCollection(1).Attribs
  '    lCurTSerAttr.Name = vAttribute.Name
  '    retval.Add lCurTSerAttr
  '  Next
  'End If
   Set ATCclsTserFile_AvailableAttributes = retval
End Property

Private Sub ATCclsTserFile_clear()
  Set pTSer = Nothing
End Sub

Private Property Get ATCclsTserFile_Data(index As Long) As ATCData.ATCclsTserData
  Set ATCclsTserFile_Data = pTSer(index)
End Property

Private Property Get ATCclsTserfile_DataCollection() As Collection
  Set ATCclsTserfile_DataCollection = pTSer
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = pTSer.Count
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "HSPF Binary Time Series Data Type"
End Property

Private Property Get ATCclsTserFile_ErrorDescription() As String
  ATCclsTserFile_ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

Private Property Get ATCclsTserFile_FileExtension() As String
  ATCclsTserFile_FileExtension = pFileExt
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newValue As String)
  Dim f As String
  
'  Set pHSPFOutput = New clsATCoHSPFOutput
'  Set pHSPFOutput.TSerFile = Me
'  Set pHSPFNetwork = New clsNetworkHSPFOutput
'  Set pHSPFNetwork.TSerFile = Me
'
'  If LCase(Right(newvalue, 4)) = ".uci" Then 'need a hos file
'    f = FilenameSetExt(newvalue, ".hos")
'    SaveFileString f, "UCI " & newvalue
'  Else
'    f = newvalue
'  End If
'  pHSPFOutput.StatusFile = f
'  pHSPFNetwork.UCIFile = pHSPFOutput.UCIFileName
  
  pBinaryFileName = newValue
  BuildTSers
  
End Property

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = pBinaryFileName
End Property

Private Property Get ATCclsTserFile_FileUnit() As Long
  ATCclsTserFile_FileUnit = 0 'many files are accessed by this class
End Property

Private Property Let ATCclsTserFile_HelpFilename(ByVal newValue As String)
  App.HelpFile = newValue
End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "HSPFBinary"
End Property

Private Property Set ATCclsTserFile_Monitor(ByVal o As Object)
  Set pMonitor = o
  pMonitorSet = True
End Property

Private Sub BuildTSers()
  Dim lTSer As ATCclsTserData, lDates As ATCclsTserDate
  Dim lDateSumm As ATTimSerDateSummary
  Dim BinHeader As HSPFBinaryHeader
  Dim lData As HSPFBinaryData
  Dim i&, j&, SDate&(5), EDate&(5), TDate&(5), s$
  Dim lSJDate As Double, lEJDate As Double, lOutLev As Long
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(OPEN HSPF Binary Output File)"
    pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
    pMonitor.SendMonitorMessage "(MSG1 " & pBinaryFileName & ")"
  End If
  Set pTSer = New Collection
  Set pBinFile = New clsHspfBinary
  Set pBinFile.Monitor = pMonitor
  pBinFile.Filename = pBinaryFileName
  For i = 1 To pBinFile.Headers.Count
    BinHeader = pBinFile.Headers.ItemByIndex(i)
    With BinHeader
      lData = .Data(1)
      lSJDate = Date2J(lData.Date)
      lOutLev = .Data(1).OutLev
      If .Data.Count = 1 Then
        j = 2 'force daily
      Else
        j = 2
        While lOutLev <> .Data(j).OutLev And i < .Data.Count 'looking for same outlev
          j = j + 1
        Wend
      End If
      If j <= .Data.Count Then
        lData = .Data(j)
        lEJDate = Date2J(lData.Date)
      Else 'only one value dont know what interval is, assume day
        lEJDate = lSJDate + 1
      End If
      Set lDates = New ATCclsTserDate
      With lDateSumm
        .CIntvl = True
        If lEJDate - lSJDate >= 1 Then 'daily or longer interval
          .ts = 1
          .Tu = BinHeader.Data(1).OutLev + 1
          If .Tu = TUDay Then
            .Intvl = 1
          Else 'undefined for monthly or annual
          End If
        Else 'use minute
          .Tu = tuminute
          .ts = timdifJ(lSJDate, lEJDate, tuminute, 1)
          .Intvl = .ts / 1440
        End If
        s = CStr(BinHeader.Data.Key(1))
        J2Date Right(s, Len(s) - 2), TDate
        For j = 0 To 3
          SDate(j) = BinHeader.Data(1).Date(j)
        Next j
' !!!!!! maybe a better way to get the proper start date
        If BinHeader.Data(1).OutLev > 2 Then
          SDate(3) = TDate(3)
          If BinHeader.Data(1).OutLev > 3 Then
            SDate(2) = TDate(2)
            If BinHeader.Data(1).OutLev > 4 Then
              SDate(1) = TDate(1)
              SDate(0) = TDate(0) - 1
            End If
          End If
        End If
        .SJDay = Date2J(SDate)
        For j = 0 To 4
          EDate(j) = BinHeader.Data(BinHeader.Data.Count).Date(j)
        Next j
        .EJDay = Date2J(EDate)
        timdif SDate, EDate, .Tu, .ts, .NVALS
      End With
      lDates.Summary = lDateSumm
      For j = 1 To .VarNames.Count
        Set lTSer = New ATCclsTserData
        Set lTSer.File = Me
        lTSer.AttribSet "Operation", .id.OperationName
        lTSer.AttribSet "Section", .id.SectionName
        lTSer.AttribSet "IDSCEN", FilenameOnly(pBinaryFileName)
        lTSer.AttribSet "IDLOCN", Left(.id.OperationName, 1) & ":" & (.id.OperationNumber)
        lTSer.AttribSet "IDCONS", .VarNames.ItemByIndex(j)
        Set lTSer.dates = lDates
        pTSer.Add lTSer, lTSer.Attrib("IDSCEN") _
                 & "-" & lTSer.Attrib("IDLOCN") _
                 & "-" & lTSer.Attrib("IDCONS")
      Next j
    End With
    If pMonitorSet Then
      s = "(PROGRESS " & CStr(50 + ((100 * i) / (pBinFile.Headers.Count * 2))) & ")"
      pMonitor.SendMonitorMessage s
    End If
  Next i
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(CLOSE)"
    pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
  End If

End Sub

Private Sub ATCclsTserFile_readData(t As ATCData.ATCclsTserData)
  Dim v!(), i&, j&, f&()
  Dim vind As Long, lOutLev As Long
  Dim BinHeader As HSPFBinaryHeader
  Dim bd As HSPFBinaryData
  Dim lKey As String
  Dim curJday As Double

  lKey = t.Attrib("Operation") & ":" & Mid(t.Attrib("IDLOCN"), 3) & ":" & t.Attrib("Section")
  If pBinFile.Headers.IndexFromKey(lKey) > 0 Then
    BinHeader = pBinFile.Headers(lKey)
    With BinHeader
      vind = 0
      i = 0
      While vind = 0 And i < .VarNames.Count
        i = i + 1
        If .VarNames.ItemByIndex(i) = t.Attrib("IDCONS") Then
          vind = i
        End If
      Wend
      If vind > 0 Then
        ReDim v(t.dates.Summary.NVALS + 1)
        ReDim f(t.dates.Summary.NVALS + 1)
        lOutLev = .Data(1).OutLev
        j = 1
        For i = 1 To .Data.Count
          bd = .Data(i)
          curJday = Date2J(bd.Date)
          If curJday >= t.dates.Summary.SJDay Then
            If curJday > t.dates.Summary.EJDay Then Exit For
            If .Data(i).OutLev = lOutLev Then
              v(j) = .Data(i).value(vind)
              j = j + 1
            End If
          End If
        Next i
      Else
        pErrorDescription = "Could not retrieve HSPF Binary data values for variable: " & t.Attrib("IDCONS")
      End If
    End With
  Else
    pErrorDescription = "Could not retrieve data values for HSPF Binary TSER" & "Key = " & lKey
    ReDim v(0)
    ReDim f(0)
  End If
  t.flags = f
  t.Values = v
  t.calcSummary
  ' next 2 might be automatic
  ReDim v(0)
  ReDim f(0)

End Sub

Private Sub ATCclsTserFile_refresh()
  'should this do anything?
End Sub

Private Function ATCclsTserFile_RemoveTimSer(t As ATCData.ATCclsTserData) As Boolean
  pErrorDescription = "Unable to Remove a Time Series for " & ATCclsTserFile_Description
  ATCclsTserFile_RemoveTimSer = False
End Function

Private Function ATCclsTserFile_RewriteTimSer(t As ATCData.ATCclsTserData) As Boolean
  pErrorDescription = "Unable to Rewrite a Time Series for " & ATCclsTserFile_Description
  ATCclsTserFile_RewriteTimSer = False
End Function

Private Function ATCclsTserFile_SaveAs(Filename As String) As Boolean
  pErrorDescription = "Unable to SaveAS for " & ATCclsTserFile_Description
  ATCclsTserFile_SaveAs = False
End Function

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCData.ATCclsTserData, Optional oldHeader As ATCData.ATTimSerDataHeader) As Boolean
  pErrorDescription = "Unable to Write a Data Header for " & ATCclsTserFile_Description
  ATCclsTserFile_writeDataHeader = False
End Function

Public Sub ShowFilterEdit(icon As Object) 'should this be a ATCclsTserFile property or function?
'  pHSPFOutput.Filter.ShowFilterEdit icon
End Sub

Private Sub Class_Initialize()
  pMonitorSet = False
  pFileExt = "HSPF Binary Output (*.hbn)|*.hbn|All Files (*.*)|*.*"
End Sub

