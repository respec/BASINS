VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTserGDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Implements ATCclsTserFile

'Private pGDBfile As Database
Private pFileExt As String
Private pErrorDescription As String
Private pData As Collection 'of ATCclsTserData
Private pDates As Collection 'of ATCclsTserDate
Private pFileName As String

Private Function ATCclsTserFile_AddTimSer(t As ATCData.ATCclsTserData, Optional ExistAction As Long = 0&) As Boolean
  Dim iAttribute As Long
  Dim iValue As Long
  
  For iAttribute = 1 To t.Attribs.Count
    Debug.Print t.Attribs(iAttribute).Name, t.Attribs(iAttribute).Value
  Next
  
  'For iValue = 1 To t.Dates.Summary.NVALS
  '  Debug.Print DumpDate(t.Dates.Value(iValue)) & "   " & t.Value(iValue)
  'Next

  
' loop to write records to database
' make sure no timeseries already in database
' stuff about whether to rewrite or renumber or whatever... (exist action)
' look at the .wdm one to see what else, and what existaction is...


End Function

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Dim retval As Collection
  Dim lCurTSerAttr As ATCclsAttributeDefinition
  
  Set retval = New Collection
  'Set lCurTSerAttr = New ATCclsAttributeDefinition
  
  '  For Each attribute available in GDB
  '    lCurTSerAttr.Name = attributeName
  '    retval.Add lCurTSerAttr
  '  Next
   
   Set ATCclsTserFile_AvailableAttributes = retval

End Property

Private Sub ATCclsTserFile_Clear()

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

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "Geodatabase Time Series Data Type"
End Property

Private Property Get ATCclsTserFile_ErrorDescription() As String
  ATCclsTserFile_ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

Private Property Get ATCclsTserFile_FileExtension() As String
  ATCclsTserFile_FileExtension = pFileExt
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newValue As String)
  pFileName = newValue
  MsgBox "Need to open GDB file here"
  
  ' get information about database
  
End Property

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = pFileName
End Property

Private Property Get ATCclsTserFile_FileUnit() As Long
  'leave blank, only used in clsTserWDM
End Property

Private Property Let ATCclsTserFile_HelpFilename(ByVal newValue As String)
  App.HelpFile = newValue
End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "Geodatabase"
End Property

Private Property Set ATCclsTserFile_Monitor(ByVal newValue As Object)
  'no need to do anything here unless you want to use a StatusMonitor to show progress
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCclsTserData)
  'Read all the values/dates from the Geodatabase into the dataObject
  'this dataObject was created during Let Filename or refresh of this class from this file
End Sub

Private Sub ATCclsTserFile_refresh()

End Sub

Private Function ATCclsTserFile_RemoveTimSer(t As ATCData.ATCclsTserData) As Boolean

End Function

Private Function ATCclsTserFile_RewriteTimSer(t As ATCData.ATCclsTserData) As Boolean

End Function

Private Function ATCclsTserFile_SaveAs(FileName As String) As Boolean

End Function

Private Function ATCclsTserFile_WriteDataHeader(dataObject As ATCData.ATCclsTserData, Optional oldHeader As ATCData.ATTimSerDataHeader) As Boolean

End Function

Private Sub Class_Initialize()
  pFileExt = "Geodatabase Files (*.mdb)|*.mdb"
  pErrorDescription = ""
  pFileName = "<unknown>"
  
  Set pData = Nothing
  Set pData = New Collection
  
  Set pDates = Nothing
  Set pDates = New Collection
End Sub
