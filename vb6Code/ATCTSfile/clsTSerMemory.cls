VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerMemory"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Implements ATCclsTserFile

Private pReadingFileName As String
Private pFilename As String
Private pDefinitionFilename As String
Private pFileExt As String
Private pErrorDescription As String
Private pData As Collection
Private pDates As Collection

Private pMonitor As Object
Private pMonitorSet As Boolean

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pData = New Collection
  
  Set pDates = Nothing
  Set pDates = New Collection
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

Private Property Let ATCclsTserFile_HelpFilename(ByVal newValue As String)
  App.HelpFile = newValue
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCclsTserData)
  'pData.add dataObject
End Sub

Private Sub ATCclsTserFile_refresh()
  pErrorDescription = "Unable to refresh in-memory time series"
End Sub

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  'It seems harmless to simply do nothing for in memory data, rather than give an error
  'pErrorDescription = "Unable to Write a Data Header for in-memory time series"
  ATCclsTserFile_writeDataHeader = True
End Function

Private Sub Class_Initialize()
  pFilename = "<in memory>"
  pDefinitionFilename = ""
  pFileExt = "All Files (*.*)|*.*"
  pErrorDescription = ""
  pMonitorSet = False
  ATCclsTserFile_clear
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

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "In-Memory"
End Property

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = pFilename
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newValue$)
  Dim delimPos As Long
  
  delimPos = InStr(newValue, "|")
  If delimPos > 0 Then
    pReadingFileName = Left(newValue, delimPos - 1)
    pDefinitionFilename = Mid(newValue, delimPos + 1)
    RunSelectedScript
  Else
    pReadingFileName = newValue
    With frmSelectScript
      .Caption = "Script Selection for importing " & pReadingFileName
      .LoadGrid pReadingFileName
ShowSelect:
      .Show vbModal
      pDefinitionFilename = .agdScripts.TextMatrix(.agdScripts.row, 1)
      Select Case .ButtonPressed
        Case .cmdCancel.Caption: Exit Property
        Case .cmdRun.Caption
          RunSelectedScript
        Case .cmdTest.Caption
          DebuggingScript = True
          RunSelectedScript
          DebuggingScript = False
        Case .cmdWizard.Caption: GoTo OpenWizard
      End Select
    End With
  End If
Exit Property

OpenWizard:
  With frmInputWizard
    Set .TserFile = Me
    If pMonitorSet Then Set .Monitor = pMonitor
    .txtDataFile = pReadingFileName
    .txtScriptFile = pDefinitionFilename
    .ReadScript
    .Show vbModal
  End With
End Property

Private Sub RunSelectedScript()
  Dim Script As clsATCscriptExpression
  If pMonitorSet Then
     pMonitor.SendMonitorMessage "(OPEN Reading " & pReadingFileName & ")"
     pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)(BUTTOFF PAUSE)"
     ScriptSetMonitor pMonitor
  End If
  
  Set Script = ScriptFromString(WholeFileString(pDefinitionFilename))
  If Script Is Nothing Then
    MsgBox "Could not load script " & pDefinitionFilename & vbCr & err.Description, vbExclamation, "Run Script"
  Else
    ScriptRun Script, pReadingFileName, Me
  End If
  If pMonitorSet Then
     pMonitor.SendMonitorMessage "(CLOSE)"
     pMonitor.SendMonitorMessage "(BUTTON CANCEL)(BUTTON PAUSE)"
  End If
End Sub

Public Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "In-memory Time Series Data Type"
End Property

Private Function ATCclsTserFile_AddTimSer(t As ATCclsTserData, Optional ExistAction& = 0) As Boolean
  Dim i&, lExAct&, retcod&, s$, TsInd&
  Dim BtnName() As Variant, nBtns&, nextBtnName As String
  Dim AppendFg As Boolean, OvwrtFg As Boolean
  Dim myMsgBox As ATCoMessage, UsrExAct&
  
  Dim foundId As Boolean, sameId As ATCclsTserData, id As Long, newId As Long
  Dim foundSerial As Boolean, serial As Long
  Dim vData As Variant, vDate As Variant
  
  serial = t.serial: foundSerial = False
  id = t.Header.id:  foundId = False
  For Each vData In pData
    If vData.serial = serial Then foundSerial = True
    If vData.Header.id = id Then foundId = True: Set sameId = vData
  Next
  
  If foundSerial Then
    ATCclsTserFile_AddTimSer = False
    pErrorDescription = "AddTimSer: time series already added"
  ElseIf foundId Then
    lExAct = ExistAction 'use local copy of what to do if DSN exists
    If id = 1 Then lExAct = TsIdRenum
    AppendFg = False
    OvwrtFg = False
    newId = findNextId(id + 1) 'find next available Id
    If lExAct = TsIdNoAction Then lExAct = TsIdRenumAsk
    If lExAct > TsIdRenum Then 'ask user what to do
      nBtns = 0
      If lExAct And TsIdReplAsk Then nextBtnName = "&Overwrite": GoSub AddButtonName
'      If lExAct And TsIdAppendAsk Then nextBtnName = "&Append": GoSub AddButtonName
      If lExAct And TsIdRenumAsk Then nextBtnName = "&Renumber": GoSub AddButtonName
      'always have Cancel as last button (and default)
      nextBtnName = "+-&Cancel": GoSub AddButtonName
      Set myMsgBox = New ATCoCtl.ATCoMessage
      UsrExAct = myMsgBox.Showarray("ID number " & id & " already exists." & vbCrLf & "Next available ID is " & newId & vbCrLf & "What should be done to it?", "In-Memory Id Collision", BtnName)
      If InStr(BtnName(UsrExAct - 1), "Overwrite") > 0 Then
        lExAct = TsIdRepl 'set to overwrite
      ElseIf InStr(BtnName(UsrExAct - 1), "Append") > 0 Then
        lExAct = TsIdAppend 'set to append
      ElseIf InStr(BtnName(UsrExAct - 1), "Renumber") > 0 Then
        lExAct = TsIdRenum 'set to renumber
      ElseIf InStr(BtnName(UsrExAct - 1), "Cancel") > 0 Then
        pErrorDescription = "Memory:AddTimSer:Id " & id & ".  User Cancelled on message box to resolve."
        Exit Function
      End If
    End If
    Select Case lExAct
      Case TsIdRepl 'overwrite the conflicting data set
        If Not ATCclsTserFile_RemoveTimSer(sameId) Then Exit Function
      Case TsIdAppend 'append to data set
        pErrorDescription = "Memory:AddTimSer:Append not yet available."
        Exit Function
'        If t.Dates.Summary.SJDay >= lData.Dates.Summary.EJDay Then
'          'start of new data follows end of existing, ok to append
'        Else 'can't append, new and existing data spans overlap
'          If ExistAction > TsIdRenum Then 'report problem to user
'            MsgBox "Time span of new data and existing data overlap." & vbCrLf & _
'                   "Unable to append data to data-set number " & t.Header.Id & " in memory.", vbExclamation, "Memory Data Problem"
'          End If
'          pErrorDescription = "Memory:AddTimSer:Id " & Id & " could not have new data appended to it."
'          Exit Function
'        End If
      Case TsIdRenum 'renumber data set
        t.Header.id = newId 'assign to next available data-set number
    End Select
  End If
    
  ATCclsTserFile_AddTimSer = True
  pData.Add t
  Set t.File = Me
  
  'May need to add dates too, if another timeseries has not already added them
  foundSerial = False
  serial = t.Dates.serial
  For Each vDate In pDates
    If vDate.serial = t.serial Then foundSerial = True: Exit For
  Next
  If Not foundSerial Then pDates.Add t.Dates
    
  Exit Function

AddButtonName:
  ReDim Preserve BtnName(nBtns)
  BtnName(nBtns) = nextBtnName
  nBtns = nBtns + 1
  Return
End Function

Private Function findNextId(IdStart As Long) As Long
  Dim vData As Variant
  Dim retval As Long
  
  retval = IdStart
  For Each vData In pData
    If retval = vData.Header.id Then retval = findNextId(retval + 1): Exit For
  Next vData
  findNextId = retval
End Function

Private Function ATCclsTserFile_RemoveTimSer(t As ATCclsTserData) As Boolean
  Dim i&, searchSerial As Long
  Dim retval As Boolean
  Dim removeDate As Boolean
  searchSerial = t.serial
  retval = False
  For i = 1 To pData.Count
    If pData(i).serial = searchSerial Then
      pData.Remove i
      retval = True
      Exit For
    End If
  Next
  
  removeDate = True
  searchSerial = t.Dates.serial
  For i = 1 To pData.Count
    If pData(i).Dates.serial = searchSerial Then removeDate = False: Exit For
  Next

  If removeDate Then
    For i = 1 To pDates.Count
      If pDates(i).serial = searchSerial Then pDates.Remove i: Exit For
    Next
  End If
  If retval = False Then
    pErrorDescription = "RemoveTimSer: did not find " & ATCclsTserFile_Description
  End If
  ATCclsTserFile_RemoveTimSer = retval
End Function

Private Function ATCclsTserFile_RewriteTimSer(t As ATCclsTserData) As Boolean
  ATCclsTserFile_RewriteTimSer = False
  pErrorDescription = "RewriteTimSer not supported for " & ATCclsTserFile_Description
End Function

Private Function ATCclsTserFile_SaveAs(filename As String) As Boolean
  ATCclsTserFile_SaveAs = False
  pErrorDescription = "Save As not supported for " & ATCclsTserFile_Description
End Function
