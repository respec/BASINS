VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsBasinsPrj"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

Private Const pLabel = "BASINS Project"
Private Const pClassName = "clsBasinsPrj"
Private pProvides As ChilkatXml

Private pManager As clsWebDataManager

Private Sub Class_Initialize()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("XML", "")
      nod2.AddAttribute "type", "ChilkatXml"
  
  Set nod1 = pProvides.newChild("criteria", "")
  Set nod2 = nod1.newChild("project_dir", "")
      nod2.AddAttribute "label", "Project Directory"
      nod2.AddAttribute "format", "openpath"
      
  Set nod1 = Nothing
  Set nod2 = Nothing

End Sub

Private Property Get clsWebData_HelpId() As Long
  clsWebData_HelpId = 30
End Property

Private Property Get clsWebData_Provides() As ChilkatXml
  Set clsWebData_Provides = pProvides
End Property

'Private Function clsWebData_ErrorDescription() As String
'  clsWebData_ErrorDescription = perrordescription
'  perrordescription = ""
'End Function

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, ByRef aResult As ChilkatXml) As Boolean
  Dim project_dir As String
  Dim xml As String
  Dim jChild As Long
  Dim lQueryChild As ChilkatXml
  Dim lCritChild As ChilkatXml
  
  pManager.LogDbg "  clsBasinsPrj GetData entry"
  clsWebData_GetData = True
  Set lQueryChild = aQuery.FirstChild
  While Not lQueryChild Is Nothing
    Select Case LCase(lQueryChild.Tag)
      Case "criteria"
        Set lCritChild = lQueryChild.FirstChild
        While Not lCritChild Is Nothing
          Select Case LCase(lCritChild.Tag)
            Case "project_dir":
              project_dir = lCritChild.Content
              While Len(project_dir) > 0 And Asc(project_dir) < 65
                project_dir = Mid(project_dir, 2)
              Wend
              If Right(project_dir, 1) <> "\" Then project_dir = project_dir & "\"
              On Error GoTo BadDir
              If Len(Dir(project_dir & "st.dbf")) = 0 Then
BadDir:               'not a basins project (need a better check)
                Err.Clear
                On Error GoTo 0
                clsWebData_GetData = False
                xml = "<" & pClassName & "><status complete='false' "
                xml = xml & "message='project_dir " & project_dir
                xml = xml & " does not point to a valid BASINS project directory'/>"
              Else
                SaveSetting "WebDataManager", "defaults", "project_dir", project_dir
              End If
          End Select
          If lCritChild.NextSibling2 = 0 Then Set lCritChild = Nothing
        Wend
      Case "output"
      Case "requested"
        If clsWebData_GetData Then
          xml = "<" & pClassName & "><status complete='true'/>"
          On Error GoTo BadXmdDbf
          Set lCritChild = lQueryChild.FirstChild
          While Not lCritChild Is Nothing
            xml = xml & xmlFromDbf(project_dir & lCritChild.Tag, lCritChild)
            If lCritChild.NextSibling2 = 0 Then Set lCritChild = Nothing
          Wend
        End If
    End Select
    If lQueryChild.NextSibling2 = 0 Then Set lQueryChild = Nothing
  Wend

  xml = xml & "</" & pClassName & ">"
  pManager.LogDbg "  clsBasinsPrj GetData xml '" & xml & "'"
  aResult.LoadXml xml
  If Len(aResult.ErrorLogText) > 0 Then
    pManager.LogMsg "Parse error getting data" & vbCr & aResult.ErrorLogText, "clsBasinsPrj Get Data"
  End If
    'DomStatus aResult, pManager.StatusFile
  
  pManager.LogDbg "  clsBasinsPrj GetData exit"

Exit Function

BadXmdDbf:
  pManager.LogMsg "Error getting XML for DBF", "clsBasinsPrj GetData"
End Function

Private Property Get clsWebData_Label() As String
  clsWebData_Label = pLabel
End Property

Private Property Get clsWebData_Name() As String
  clsWebData_Name = "clsBasinsPrj"
End Property

Private Property Set clsWebData_Manager(myManager As clsWebDataManager)
  Set pManager = Nothing
  Set pManager = myManager
  availableFromDBF
End Property

Private Property Get clsWebData_Manager() As clsWebDataManager
  Set clsWebData_Manager = pManager
End Property

Private Function xmlFromDbf(dbfName As String, lNode As ChilkatXml) As String
  Dim lFilename As String
  Dim myDBF As clsDBF
  Dim s As String
  'Dim fStart As String, fEnd As String
  Dim f As String
  Dim iRecord As Long
  Dim iChild As Long
  Dim fld As String
  Dim fldName() As String
  Dim fldNum() As Long
  
  pManager.LogDbg "  clsBasinsPrj xmlFromDbf (" & dbfName & ", " & lNode.GetXml & ")"
  
  f = FilenameOnly(dbfName)
  lFilename = PathNameOnly(dbfName) & "\" & Right(f, Len(f) - 4) & ".dbf"
  s = ""
  
  If Not (FileExists(lFilename)) Then
    pManager.LogMsg "Could not open " & lFilename, "BASINS Project (xmlFromDbf)", "Ok"
  Else
    Set myDBF = New clsDBF
    myDBF.OpenDBF lFilename
    ReDim fldNum(lNode.NumChildren)
    ReDim fldName(lNode.NumChildren)
    For iChild = 0 To lNode.NumChildren - 1
      fldName(iChild) = lNode.GetChild(iChild).Tag
      fldNum(iChild) = myDBF.FieldNumber(genericName(f, fldName(iChild)))
      pManager.LogDbg "  clsBasinsPrj xmlFromDbf fldName(" & iChild & ") = " & fldName(iChild)
      pManager.LogDbg "  clsBasinsPrj xmlFromDbf fldNum(" & iChild & ")  = " & fldNum(iChild)
    Next
    
    For iRecord = 1 To myDBF.NumRecords
      myDBF.CurrentRecord = iRecord
      'S = S & fStart
      For iChild = 0 To lNode.NumChildren - 1
        s = s & "<" & fldName(iChild) & ">" & myDBF.Value(fldNum(iChild)) & "</" & fldName(iChild) & ">"
      Next
      'S = S & fEnd
    Next
    myDBF.Clear
  End If
  xmlFromDbf = s
End Function

Private Function GetAvailDBF() As clsDBF
  Static openedDBF As Boolean
  Static sAvailDBF As clsDBF
  Static AvailFilename As String
  Dim ff As ATCoFindFile
  Dim step As String
  
  On Error GoTo erropen
  step = "If Not openedDB"
  If Not openedDBF Then
    step = "set ff"
    Set ff = New ATCoFindFile
    step = "ff.SetDialogProperties"
    AvailFilename = pManager.CurrentStatusGetString("basins_dir")
    If Len(AvailFilename) > 0 Then
      AvailFilename = AvailFilename & "etc\DataDownload"
    Else
      AvailFilename = CurDir
    End If
    ff.SetDialogProperties "Please locate database of available BASINS project attributes", AvailFilename & "\avail.dbf", "dbf"
    step = "ff.SetRegistryInfo"
    ff.SetRegistryInfo "WebData", "BasinsPrj", "AvailDBF"
    step = "ff.GetName"
    AvailFilename = ff.GetName
    step = "OpenDBF(" & AvailFilename & ")"
    Set sAvailDBF = New clsDBF
    sAvailDBF.OpenDBF AvailFilename
    step = "openedDB = True"
    openedDBF = True
  End If
  Set GetAvailDBF = sAvailDBF
  
  Exit Function

erropen:
  pManager.LogMsg "Error opening available database '" & AvailFilename & "'" & vbCr & "at step " & step & vbCr & Err.Description, "clsBasinsPrj GetAvailDBF"
  Set GetAvailDBF = Null
End Function

Private Sub availableFromDBF()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  Dim nod3 As ChilkatXml
  Dim myDBF As clsDBF
  Dim iRecord As Long
  Dim lFile As String

  'On Error GoTo erropen
  
  'this might come from a basins project, but how are we sure its complete?
  'Set myDB = OpenDatabase("c:\data\vbexperimental\basinsprojectdb\data", False, True, "DBASE IV")
  Set myDBF = GetAvailDBF
  
  Set nod1 = pProvides.newChild("available", "")
  If Not myDBF Is Nothing Then
    For iRecord = 1 To myDBF.NumRecords
      myDBF.CurrentRecord = iRecord
  
      If myDBF.Value(1) <> lFile Then
        'must start with character!
        Set nod2 = nod1.newChild("dbf_" & Left(myDBF.Value(1), Len(myDBF.Value(1)) - 4), "")
      End If
      Set nod3 = nod2.newChild(genericName(nod2.Tag, myDBF.Value(2)), "")
         'nod3.addAttribute "name", "state fips"
          nod3.AddAttribute "type", myDBF.Value(3) 'Todo: NameOfDBtype(myDBF(2).Value)
          nod3.AddAttribute "optional", "true"
          nod3.AddAttribute "selected", "false"
      lFile = myDBF.Value(1)
    Next
    myDBF.Clear
  End If
  
  Exit Sub

erropen:
  pManager.LogMsg "Error checking available database" & vbCr & Err.Description, "clsBasinsPrj availableFromDBF"
    
End Sub

Private Function genericName(file As String, field As String) As String
  Dim s$
  
  s = field
  Select Case file
    Case "dbf_st" 'must go either way!
      If LCase(field) = "st" Then s = "state_abbrev"
      If LCase(field) = "state_abbrev" Then s = "st"
    Case "dbf_cat"
      If LCase(field) = "cu" Then s = "huc_cd"
      If LCase(field) = "huc_cd" Then s = "cu"
  End Select
  genericName = s
End Function

'following code copied from atml2k:mapcol.bas
'Public Function NameOfDBtype$(ByVal typ&)
'  Select Case typ
'    Case dbBigInt:     NameOfDBtype = "Big Integer"
'    Case dbBinary:     NameOfDBtype = "Binary"
'    Case dbBoolean:    NameOfDBtype = "Boolean"
'    Case dbByte:       NameOfDBtype = "Byte"
'    Case dbChar:       NameOfDBtype = "Char"
'    Case dbCurrency:   NameOfDBtype = "Currency"
'    Case dbDate:       NameOfDBtype = "Date / Time"
'    Case dbDecimal:    NameOfDBtype = "Decimal"
'    Case dbDouble:     NameOfDBtype = "Double"
'    Case dbFloat:      NameOfDBtype = "Float"
'    Case dbGUID:       NameOfDBtype = "Guid"
'    Case dbInteger:    NameOfDBtype = "Integer"
'    Case dbLong:       NameOfDBtype = "Long"
'    Case dbLongBinary: NameOfDBtype = "Long Binary"
'    Case dbMemo:       NameOfDBtype = "Memo"
'    Case dbNumeric:    NameOfDBtype = "Numeric"
'    Case dbSingle:     NameOfDBtype = "Single"
'    Case dbText:       NameOfDBtype = "Text"
'    Case dbTime:       NameOfDBtype = "Time"
'    Case dbTimeStamp:  NameOfDBtype = "Time Stamp"
'    Case dbVarBinary:  NameOfDBtype = "VarBinary"
'    Case Else:         NameOfDBtype = "Undefined"
'  End Select
'End Function

Private Sub clsWebData_Specify(Optional parent As Object = Nothing, Optional ByVal Index As Long = 0)
  pManager.ShowCriteria Me, parent, Index
End Sub

Private Property Get clsWebData_Version() As String
  clsWebData_Version = App.Major & "." & App.Minor & "." & App.Revision
End Property

