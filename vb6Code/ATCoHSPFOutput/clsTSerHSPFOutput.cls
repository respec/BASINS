VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerHSPFOutput"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

'Copyright 2000 by AQUA TERRA Consultants
Implements ATCclsTserFile

Private pMonitor As Object
Private pMonitorSet As Boolean
Private pHSPFOutput As clsATCoHSPFOutput
Private pHSPFNetwork As clsNetworkHSPFOutput
Private pFileExt As String
Private pErrorDescription As String

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
  Set pHSPFOutput = Nothing
End Sub

Private Property Get ATCclsTserFile_Data(index As Long) As ATCData.ATCclsTserData
  Set ATCclsTserFile_Data = pHSPFOutput.DataCollection(index)
End Property

Private Property Get ATCclsTserfile_DataCollection() As Collection
  Set ATCclsTserfile_DataCollection = pHSPFOutput.DataCollection
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = pHSPFOutput.DataCollection.Count
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "HSPF Output Time Series Data Type"
End Property

Private Property Get ATCclsTserFile_ErrorDescription() As String
  Dim s As String
  
  s = pHSPFOutput.ErrorDescription
  If Len(s) = 0 Then
    s = pErrorDescription
  ElseIf Len(pErrorDescription) > 0 Then
    s = s & vbCrLf & pErrorDescription
  End If
  ATCclsTserFile_ErrorDescription = s
  pErrorDescription = ""
End Property

Private Property Get ATCclsTserFile_FileExtension() As String
  ATCclsTserFile_FileExtension = pFileExt
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newValue As String)
  Dim f As String
  
  Set pHSPFOutput = New clsATCoHSPFOutput
  Set pHSPFOutput.TSerFile = Me
  Set pHSPFNetwork = New clsNetworkHSPFOutput
  Set pHSPFNetwork.TSerFile = Me
  
  If LCase(Right(newValue, 4)) = ".uci" Then 'need a hos file
    f = FilenameSetExt(newValue, ".hos")
    SaveFileString f, "UCI " & newValue
  Else
    f = newValue
  End If
  pHSPFOutput.StatusFile = f
  'pHSPFNetwork.UCIFile = pHSPFOutput.UCIFileName
  
End Property

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = pHSPFOutput.StatusFile
End Property

Private Property Get ATCclsTserFile_FileUnit() As Long
  ATCclsTserFile_FileUnit = 0 'many files are accessed by this class
End Property

Private Property Let ATCclsTserFile_HelpFilename(ByVal newValue As String)
  App.HelpFile = newValue
End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "HSPFOutput"
End Property

Private Property Set ATCclsTserFile_Monitor(ByVal o As Object)
  Set pMonitor = o
  pMonitorSet = True
End Property

Private Sub ATCclsTserFile_readData(t As ATCData.ATCclsTserData)
  Dim v!(), i, f&()
  Dim lPages As FastCollection, PageKey As String, lLoc As String, PageInd&
  Dim outfile As String, TserPos&
  Dim SnowAdjustFlag As Boolean, ValSetFlag As Boolean
  Dim lSection As String, lSubSection As String

  Set lPages = New FastCollection
  Set lPages = pHSPFOutput.PageCollection
  lLoc = t.Attrib("IDLOCN")
  i = InStrRev(lLoc, ":")
  If i > 2 Then
'    PageKey = t.Attrib("Operation") & "-" & Mid(lLoc, 2, i - 2) & "-" & t.dates.Summary.Tu
    PageKey = t.Attrib("Operation") & "-" & Right(lLoc, Len(lLoc) - i)
  End If
  PageInd = lPages.IndexFromKey(PageKey)
  If PageInd > 0 Then 'found page for this tser
    lSection = t.Attrib("Section")
    lSubSection = t.Attrib("SubSection")
    If lPages(PageKey).SnowFlag Then
      If lSection = "ATEMP" Then
        SnowAdjustFlag = False
      Else
        SnowAdjustFlag = True
      End If
    Else
      SnowAdjustFlag = False
    End If
    ReDim v(t.dates.Summary.NVALS)
    ReDim f(t.dates.Summary.NVALS)
    outfile = WholeFileString(t.Attrib("OutFile"))
    For i = 1 To t.dates.Summary.NVALS
      ValSetFlag = False
      With lPages(PageKey).PageInstances.ItemByIndex(i)
        TserPos = .Offset + t.Attrib("OffsetInPage")
        If SnowAdjustFlag Then
          If lSection = "SNOW" Then
            If lSubSection = "STATE VARIABLES" Then
              If Not (.SnowPackFlag) Then
                v(i) = 0#  'or better?
                ValSetFlag = True
              End If
            ElseIf lSubSection = "FLUXES" Then
              If Not (.SnowFluxFlag) Then
                v(i) = 0# 'or better
                ValSetFlag = True
              ElseIf .SnowPackFlag Then
                TserPos = TserPos + pHSPFOutput.SnowStateOffset
              End If
            End If
          Else
            If .SnowPackFlag Then
              TserPos = TserPos + pHSPFOutput.SnowStateOffset
            End If
            If .SnowFluxFlag Then
              TserPos = TserPos + pHSPFOutput.SnowFluxOffset
            End If
          End If
        End If
      End With
      If Not (ValSetFlag) Then v(i) = Mid(outfile, TserPos, 10)
    Next i
  Else
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
  pHSPFOutput.Filter.ShowFilterEdit icon
End Sub

Private Sub Class_Initialize()
  pMonitorSet = False
  pFileExt = "HSPF Output Specification (*.hos)|*.hos|HSPF User's Control Input (*.uci)|*.uci"
End Sub
