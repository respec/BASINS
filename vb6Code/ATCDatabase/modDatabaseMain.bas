Attribute VB_Name = "modDatabaseMain"
Option Explicit

Global g_NHD_DBF As Boolean 'True if creating a BASINS NHD shape file DBF

Public CopyData As Boolean
Public frmMainVisible As Boolean
Private Const pBinaryData As Boolean = False
'Private pMonitor As Object
'Private pMonitorSet As Boolean

'Private Const indent1 = "    "
'Private Const indent2 = "        "
'Private Const indent3 = "             "

Private Const indent1 = vbTab
Private Const indent2 = vbTab & vbTab
Private Const indent3 = vbTab & vbTab & vbTab

Private ShapeType As Long

Sub main()
  Dim mdbPos As Long
  Dim xmlPos As Long
  Dim DatabaseFilename As String
  Dim XMLFilename As String
  If Len(Command) > 0 Then
    mdbPos = InStr(LCase(Command), ".mdb")
    xmlPos = InStr(LCase(Command), ".xml")
  End If
  If mdbPos > 0 And xmlPos > 0 Then
    CopyData = True
    If mdbPos > xmlPos Then
      XMLFilename = Trim(Left(Command, xmlPos + 4))
      DatabaseFilename = Trim(Mid(Command, xmlPos + 5))
      If FileExists(XMLFilename) Then
        XMLtoDatabase XMLFilename, DatabaseFilename
      Else
        MsgBox "XML input file " & XMLFilename & " not found", vbOKOnly, "Database"
      End If
    Else
      DatabaseFilename = Trim(Left(Command, mdbPos + 4))
      XMLFilename = Trim(Mid(Command, mdbPos + 5))
      If FileExists(DatabaseFilename) Then
        DatabaseToXML DatabaseFilename, XMLFilename
      Else
        MsgBox "Database input file " & DatabaseFilename & " not found", vbOKOnly, "Database"
      End If
    End If
  Else
    frmMainVisible = True
    frmMain.Show
  End If
End Sub

Private Sub SetStatus(status As String)
  If frmMainVisible Then
    frmMain.lblStatus.Caption = status
    frmMain.lblStatus.Refresh
  End If
End Sub

Sub XMLtoDatabase(XMLFilename As String, DatabaseFilename As String)
  Dim xmlString As String
  Dim xml As clsXMLelement
  Dim xmlSection As clsXMLelement
  Dim vSection As Variant
  
  Dim DB As Database
  Dim dbTable As DAO.TableDef
  Dim dbQuery As DAO.QueryDef
  Dim dbRelation As DAO.Relation
  
  Dim findTag As Long
  Dim allNames As String
  Dim name As String
  Dim attributes As String
  
  On Error GoTo 0 'ErrHand
  
  SetStatus "Reading " & FilenameNoPath(XMLFilename)
  xmlString = WholeFileString(XMLFilename)
  findTag = InStr(xmlString, "<ATCDatabase ")
  If findTag = 0 Then
    MsgBox "ATCDatabase not found", vbOKOnly, "Could not read " & XMLFilename
  Else
    
    SetStatus "Checking for existing database"
    If Len(Dir(DatabaseFilename)) > 0 Then
      If MsgBox("Overwrite existing database '" & DatabaseFilename & "'?", vbOKCancel, "Target Database Exists") = vbCancel Then Exit Sub
      Kill DatabaseFilename
    End If
    
    xmlString = Mid(xmlString, findTag)
    Set xml = New clsXMLelement
    SetStatus "Parsing " & FilenameNoPath(XMLFilename)
    xml.SetString xmlString
    xmlString = ""
    SetStatus "Creating database " & DatabaseFilename
    Set DB = CreateDatabase(DatabaseFilename, dbLangGeneral, dbVersion30)
    For Each vSection In xml.SubElements
      Set xmlSection = vSection
      With xmlSection
        name = .AttributeValue("Name")
        Select Case LCase(.tag)
          Case "table"
            SetStatus "Converting table " & name
            attributes = .AttributeValue("Attributes", "Default")
            If IsNumeric(attributes) Then
              Set dbTable = DB.CreateTableDef(name, CLng(attributes))
            Else
              Set dbTable = DB.CreateTableDef(name)
            End If
            AddTableToDatabase xmlSection, DB, dbTable
          Case "relation"
            SetStatus "Converting relation " & name
            attributes = .AttributeValue("Attributes", "Default")
            If Not IsNumeric(attributes) Then attributes = "0"
            Set dbRelation = DB.CreateRelation(name, .AttributeValue("Table"), _
                                                     .AttributeValue("ForeignTable"), _
                                                     CLng(attributes))
            allNames = .AttributeValue("Fields")
            While Len(allNames) > 0
              name = StrSplit(allNames, ",", "")
              dbRelation.Fields.Append dbRelation.CreateField(name)
              If Len(allNames) > 0 Then
                dbRelation.Fields(name).ForeignName = StrSplit(allNames, ",", "")
              Else
                dbRelation.Fields(name).ForeignName = name
              End If
            Wend
            DB.Relations.Append dbRelation
          Case "query"
            SetStatus "Converting query " & name
            Set dbQuery = DB.CreateQueryDef(name, XML2str(.AttributeValue("SQL")))
            'db.QueryDefs.Append dbQuery
          Case Else
            MsgBox .GetString, vbOKOnly, "Unrecognized child of ATCDatabase skipped"
        End Select
      End With
    Next 'Table
  End If
  If Not DB Is Nothing Then DB.Close
  Exit Sub
ErrHand:
  MsgBox Err.Description, vbOKOnly, "Error in XMLtoDatabase"
  If Not DB Is Nothing Then DB.Close
End Sub

Private Sub AddTableToDatabase(xmlTable As clsXMLelement, DB As Database, dbTable As TableDef)
  Dim xmlTablePart As clsXMLelement
  Dim vTablePart As Variant
  Dim vField As Variant
  Dim dbField As DAO.Field
  Dim dbIndex As DAO.index
  Dim rs As Recordset
  Dim vAttributeName As Variant
  Dim AttributeValue As String
    
  For Each vTablePart In xmlTable.SubElements
    Set xmlTablePart = vTablePart
    With xmlTablePart
      Select Case LCase(.tag)
        Case "index"
          Set dbIndex = dbTable.CreateIndex(.AttributeValue("Name"))
          dbIndex.Fields = .AttributeValue("Fields")
          If LCase(.AttributeValue("Unique", "True")) = "false" Then dbIndex.Unique = False Else dbIndex.Unique = True
          If LCase(.AttributeValue("Primary", "True")) = "false" Then dbIndex.Primary = False Else dbIndex.Primary = True
          dbTable.Indexes.Append dbIndex
        
        Case "field"
          Set dbField = dbTable.CreateField(.AttributeValue("Name"), _
                                       CLng(.AttributeValue("Type")), _
                                       CLng(.AttributeValue("Size")))
          For Each vAttributeName In .AttributeNames
            AttributeValue = .AttributeValue(vAttributeName)
            Select Case LCase(vAttributeName)
              Case "required": If LCase(AttributeValue) = "true" Then dbField.Required = True
              Case "allowzerolength": If LCase(AttributeValue) = "false" Then dbField.AllowZeroLength = False
              Case "attributes": dbField.attributes = CLng(AttributeValue)
              Case "defaultvalue": dbField.DefaultValue = AttributeValue
              Case "validationrule": dbField.ValidationRule = AttributeValue
              Case "validationtext": dbField.ValidationText = AttributeValue
            End Select
          Next
          dbTable.Fields.Append dbField
        
        Case "row"
          If Not CopyData Then Exit For 'Skip reading data if we are only copying the structure
          If rs Is Nothing Then
            'All fields must appear before all rows in a table
            DB.TableDefs.Append dbTable
            Set rs = dbTable.OpenRecordset(dbOpenTable, dbDenyWrite)
          End If
          rs.AddNew
          For Each vField In .AttributeNames
            If rs.Fields(vField).Type = 11 Then
              rs.Fields(vField).Value = Hex2Binary(.AttributeValue(vField))
            Else
              rs.Fields(vField).Value = XML2str(.AttributeValue(vField))
            End If
          Next
          rs.Update
        Case Else
          MsgBox .GetString, vbOKOnly, "Unrecognized child of Table"
      End Select
    End With
  Next
  If rs Is Nothing Then 'Table must be empty, but add it to db anyway if it has fields
    If dbTable.Fields.Count > 0 Then DB.TableDefs.Append dbTable
  Else
    rs.Close
    Set rs = Nothing
  End If
End Sub

Sub DatabaseToXML(DatabaseFilename As String, XMLFilename As String)
  Dim OutFile As Integer
  Dim BinOutFile As Integer
  Dim DB As Database
  Dim rs As Recordset
  Dim fld As DAO.Field
  Dim indx As DAO.index
  Dim i As Long
  Dim IndexNum As Long
  Dim fieldNum As Long
  Dim Value As String
    
  'On Error GoTo ErrHandler
  OutFile = FreeFile
  Open XMLFilename For Output As OutFile
  If pBinaryData Then
    BinOutFile = FreeFile
    Open XMLFilename & "_data.bin" For Binary As BinOutFile
  End If
'  If pMonitorSet Then
'     pMonitor.SendMonitorMessage "(OPEN Exporting Database " & DatabaseFilename & " to " & XMLFilename & ")"
'     pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
'     pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
'     pMonitor.SendMonitorMessage "(MSG1 " & DatabaseFilename & ")"
'  End If
    
  Set DB = OpenDatabase(DatabaseFilename, False, vbReadOnly)
  Print #OutFile, "<?xml version=""1.0""?>"
  'Print #OutFile, "<!DOCTYPE ATCDatabase SYSTEM ""http://hspf.com/pub/download/ATCDatabase.dtd"">"
  Print #OutFile, "<ATCDatabase Name=""" & FilenameNoPath(DatabaseFilename) & """ Path=""" & PathNameOnly(DatabaseFilename) & """>"
  
  For i = 0 To DB.TableDefs.Count - 1
    With DB.TableDefs(i)
      If Left(.name, 4) <> "MSys" Then
        SetStatus "Converting table " & .name
        Debug.Print "Converting table " & .name
        Print #OutFile, indent1 & "<Table Name=""" & .name & """ Attributes=""" & .attributes & """>"
        
        For IndexNum = 0 To .Indexes.Count - 1
          Set indx = .Indexes(IndexNum)
          If Not indx.Foreign Then
            Print #OutFile, indent2 & "<Index Name=""" & indx.name & """ Fields=""" & indx.Fields & """";
            If Not indx.Unique Then Print #OutFile, " Unique=""False""";
            If Not indx.Primary Then Print #OutFile, " Primary=""False""";
            Print #OutFile, "/>"
          End If
        Next
        
        For fieldNum = 0 To .Fields.Count - 1
          Set fld = .Fields(fieldNum)
          Print #OutFile, indent2 & "<Field Name=""" & fld.name _
                                    & """ Type=""" & fld.Type _
                                    & """ Size=""" & fld.size & """";
          If fld.Required Then Print #OutFile, " Required=""True""";
          Select Case fld.Type
            Case dbText, dbMemo: If Not fld.AllowZeroLength Then Print #OutFile, " AllowZeroLength=""False""";
          End Select
          If fld.attributes <> 0 Then Print #OutFile, " Attributes=""" & fld.attributes & """";
          If Len(fld.DefaultValue) > 0 Then Print #OutFile, " DefaultValue=""" & fld.DefaultValue & """";
          If Len(fld.ValidationRule) > 0 Then Print #OutFile, " ValidationRule=""" & fld.ValidationRule & """";
          If Len(fld.ValidationText) > 0 Then Print #OutFile, " ValidationText=""" & fld.ValidationText & """";
          Print #OutFile, "/>"
        Next
        
        If CopyData Then
          Set rs = DB.OpenRecordset(.name, dbOpenTable)
          While Not rs.EOF
            If pBinaryData Then
'              For FieldNum = 0 To rs.Fields.Count - 1
'                Put #BinOutFile, , rs.Fields(FieldNum).value
'              Next
            'Binary-like data written to text file
              For fieldNum = 0 To rs.Fields.Count - 1
                With rs.Fields(fieldNum)
                  If IsNull(.Value) Then
                    Print #OutFile, "0000"
                  Else
                    Value = str2XML(.Value)
                    Print #OutFile, Format(Len(Value), "0000") & Value;
                  End If
                End With
              Next
            
            Else
              Print #OutFile, indent2 & "<Row"
              For fieldNum = 0 To rs.Fields.Count - 1
                With rs.Fields(fieldNum)
                  If Not IsNull(.Value) Then
                    If .Type = 11 Then
                      Print #OutFile, indent3 & .name & "=""" & Binary2Hex(.Value) & """"
                    Else
                      Print #OutFile, indent3 & .name & "=""" & str2XML(.Value) & """"
                    End If
                  End If
                End With
              Next
              Print #OutFile, indent2 & "/>"
            End If
            rs.MoveNext
          Wend
        End If
        Print #OutFile, indent1 & "</Table>"
      End If
    End With
  Next
  
  'Save Relations
  For i = 0 To DB.Relations.Count - 1
    With DB.Relations(i)
      Print #OutFile, indent1 & "<Relation Name=""" & .name & """";
      Print #OutFile, " Table=""" & .Table & """";
      Print #OutFile, " ForeignTable=""" & .ForeignTable & """";
      If .attributes <> 0 Then Print #OutFile, " Attributes=""" & .attributes & """";
      
      Print #OutFile, " Fields=""";
      For fieldNum = 0 To .Fields.Count - 1
        Print #OutFile, .Fields(fieldNum).name & "," & .Fields(fieldNum).ForeignName;
        If fieldNum < .Fields.Count - 1 Then Print #OutFile, ",";
      Next
      Print #OutFile, """/>"
    End With
  Next
  
  'Save Queries
  For i = 0 To DB.QueryDefs.Count - 1
    With DB.QueryDefs(i)
      If Left(.name, 1) = "~" Then
        'Print #OutFile, indent1 & "<OddQuery Name=""" & .name & """ Type=""" & .Type & """ SQL=""" & str2XML(.SQL) & """/>"
      Else
        Print #OutFile, indent1 & "<Query Name=""" & .name & """ Type=""" & .Type & """ SQL=""" & str2XML(.SQL) & """/>"
      End If
    End With
  Next 'Query
  
  Print #OutFile, "</ATCDatabase>"
  Close #OutFile
  
  If Not DB Is Nothing Then DB.Close
  
'  If pMonitorSet Then
'     pMonitor.SendMonitorMessage "(CLOSE)"
'     pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
'     pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
'  End If
  
  Exit Sub
ErrHandler:
  MsgBox Err.Description, vbOKOnly, "Error in DatabaseToXML"
  If Not DB Is Nothing Then DB.Close
End Sub

Sub GeoDatabaseToShape(aDatabaseFilename As String)
  Dim gdb As New clsGeoDatabase
  Dim LayersToSave As FastCollection
  Dim iLayer As Long
  Dim LayerName As String
  Dim ShapeFilename As String
  
  On Error GoTo ErrHandler
  
  If Not gdb.OpenGeoDatabase(aDatabaseFilename) Then
    MsgBox "Could not open '" & aDatabaseFilename & "'" & vbCrLf & vbCrLf _
          & gdb.LastError, vbCritical, "Unable to open database"
  Else
    Set LayersToSave = frmSelectLayers.SelectLayers(gdb)
    Unload frmSelectLayers
    For iLayer = 1 To LayersToSave.Count
      LayerName = LayersToSave.ItemByIndex(iLayer)
      SetStatus "Converting table " & LayerName
      frmMain.cdlg.Filename = LayerName & ".shp"
      With frmMain.cdlg
        .DialogTitle = "Save Shape file as"
        .Filter = "Shape files *.shp|*.shp|All Files|*.*"
        .FilterIndex = 1
        .DefaultExt = "shp"
        .ShowSave
        ShapeFilename = .Filename
      End With
      
      
      If LCase(LayerName) = "nhdflowline" Then
        If MsgBox("Convert NHD flow line shape table for use by BASINS?", vbYesNo, "NHD Flow Line") = vbYes Then
          gdb.SaveShape LayerName, ShapeFilename, "DSRCHID,ComID,NHDFlow,FromComID,ToComID"
          ReformatNHDflowlineDBF FilenameNoExt(ShapeFilename) & ".dbf"
        Else
          gdb.SaveShape LayerName, ShapeFilename
        End If
      Else
          gdb.SaveShape LayerName, ShapeFilename
      End If
      
NextTable:
    Next
  End If
      
  
  Exit Sub

NoRecords:
  Resume NextTable

ErrHandler:
  If Err.Number = 32755 Then Resume NextTable
  MsgBox Err.Description, vbOKOnly, "Error in GeoDatabaseToShape"
  Exit Sub

End Sub

Private Sub ReformatNHDflowlineDBF(aShapeDBFfilename As String)
  Dim oldDBF As clsDBF
  Dim newDBF As clsDBF
  Dim oldField As Long
  Dim newField As Long
  Dim DateField As Long
  Dim MetersField As Long
  Dim ComIDField As Long
  Dim NewRchIDField As Long
  Dim iRecord As Long

  Set newDBF = NewNHDtable
  Set oldDBF = New clsDBF
  oldDBF.OpenDBF aShapeDBFfilename
  newDBF.NumRecords = oldDBF.NumRecords
  newDBF.InitData
  
  ReDim DBFmatchingField(oldDBF.NumFields)
  For oldField = 1 To oldDBF.NumFields
    Select Case LCase(oldDBF.FieldName(oldField))
      Case "comid":      newField = newDBF.FieldNumber("COM_ID"):  ComIDField = newField
      Case "fdate":      newField = newDBF.FieldNumber("RCH_DATE"): DateField = newField
      Case "resolution": newField = newDBF.FieldNumber("LEVEL")
      Case "gnis_id":    newField = newDBF.FieldNumber("GNIS_ID")
      Case "gnis_name":  newField = newDBF.FieldNumber("NAME")
      Case "lengthkm":   newField = newDBF.FieldNumber("METERS"): MetersField = newField
      Case "reachcode":  newField = newDBF.FieldNumber("RCH_CODE")
      'Maybe there a a field by the same name, if not, zero
      Case Else: newField = newDBF.FieldNumber(oldDBF.FieldName(oldField))
    End Select
    DBFmatchingField(oldField) = newField
  Next
  NewRchIDField = newDBF.FieldNumber("RCHID")
  
  For iRecord = 1 To oldDBF.NumRecords
    oldDBF.CurrentRecord = iRecord
    newDBF.CurrentRecord = iRecord
    For oldField = 1 To oldDBF.NumFields
      newField = DBFmatchingField(oldField)
      If newField > 0 Then
        Select Case newField
          Case MetersField
            If IsNumeric(oldDBF.Value(oldField)) Then
              newDBF.Value(newField) = oldDBF.Value(oldField) * 1000
            Else
              newDBF.Value(newField) = ""
            End If
        Case DateField
          newDBF.Value(newField) = ReformatDate(oldDBF.Value(oldField))
        Case ComIDField
          newDBF.Value(newField) = oldDBF.Value(oldField)
          If NewRchIDField > 0 Then 'Copy same ID to redundant field
            newDBF.Value(NewRchIDField) = oldDBF.Value(oldField)
          End If
        Case Else
          newDBF.Value(newField) = oldDBF.Value(oldField)
        End Select
      End If
    Next
  Next
  newDBF.WriteDBF aShapeDBFfilename
  newDBF.Clear
  oldDBF.Clear
End Sub

'Re-format month/day/year to yyyymmdd
Private Function ReformatDate(mmddyyyy As String) As String
  Dim firstslash As Long
  Dim secondslash As Long
  ReformatDate = mmddyyyy 'default to original value if we can't parse
  firstslash = InStr(mmddyyyy, "/")
  If firstslash > 0 Then
    secondslash = InStr(firstslash + 1, mmddyyyy, "/")
    If secondslash > 0 Then
      ReformatDate = Mid(mmddyyyy, secondslash + 1) & _
        Format(Left(mmddyyyy, firstslash - 1), "00") & _
        Format(Mid(mmddyyyy, firstslash + 1, secondslash - firstslash - 1), "00")
    End If
  End If
End Function

Private Function str2XML(str As String) As String
  Dim xml As String
  xml = ReplaceString(str, "<", "&lt;")
  xml = ReplaceString(xml, ">", "&gt;")
  xml = ReplaceString(xml, "&", "&amp;")
  xml = ReplaceString(xml, """", "&quot;")
  str2XML = xml
End Function

Private Function XML2str(xml As String) As String
  Dim str As String
  str = ReplaceString(xml, "&lt;", "<")
  str = ReplaceString(str, "&gt;", ">")
  str = ReplaceString(str, "&amp;", "&")
  str = ReplaceString(str, "&quot;", """")
  XML2str = str
End Function

Private Function Binary2Hex(bytes() As Byte) As String
  Dim i As Long
  For i = LBound(bytes) To UBound(bytes)
    If bytes(i) < 16 Then Binary2Hex = Binary2Hex & "0"
    Binary2Hex = Binary2Hex & hex(bytes(i))
  Next
  
'  If Left(Binary2Hex, 2) = "05" Then Stop
'
'  Dim iOffset As Long
'  Dim d As Double
'  Dim allReasonable As Boolean
'  For iOffset = 0 To 7
'    allReasonable = True
'    For i = LBound(bytes) + iOffset To UBound(bytes) - 8 Step 8
'      CopyMemory ByVal VarPtr(d), bytes(i), 8
'      On Error GoTo CompareErr
'      'If allReasonable Then
'        If d > 180 Or d < -180 Then
'          allReasonable = False
'        ElseIf Abs(d) < 0.1 Then 'And Abs(d) > 0 Then
'          allReasonable = False
'        Else
'          Debug.Print "Byte " & i & "-" & i + 7 & ": " & d
'        End If
'      'End If
'    Next

'    If allReasonable Then
'      Debug.Print "Doubles at offset " & iOffset
'      For i = LBound(bytes) + iOffset To UBound(bytes) - 8 Step 8
'        CopyMemory ByVal VarPtr(d), bytes(i), 8
'        Debug.Print d
'      Next
'      Stop
'    End If
  
'  Next
'
'  Exit Function
'
'CompareErr:
'  allReasonable = False
'  Resume Next
End Function

Private Function Hex2Binary(hex As String) As Byte()
  Dim nBytes As Long
  Dim i As Long, iHex As Long
  Dim Hex2Bin() As Byte
  nBytes = Len(hex) / 2
  ReDim Hex2Bin(1 To nBytes)
  iHex = 1
  For i = 1 To nBytes
    Hex2Bin(i) = TwoHexits2Byte(Mid(hex, iHex, 2))
    iHex = iHex + 2
  Next
  Hex2Binary = Hex2Bin
End Function

'Public Function DoubleToHex(dbl As Double) As String
'  Dim hex As String
'  Dim b(0 To 7) As Byte
'  Dim i As Long
'
'  CopyMemory b(0), VarPtr(hex), 8
'
'  For i = 0 To 7
'    Debug.Print b(i)
'  Next
'
'  DoubleToHex = Binary2Hex(b)
'End Function
'
'Public Function HexToDouble(ByVal hex As String) As Double
'  Dim d As Double
'  Dim b(0 To 7) As Byte
'  Dim i As Long
'  hex = ReplaceString(hex, " ", "")
'  For i = 0 To 7
'    'Debug.Print Mid(hex, i * 2 + 1, 2)
'    b(7 - i) = TwoHexits2Byte(Mid(hex, i * 2 + 1, 2))
'    Debug.Print b(7 - i)
'  Next
'  Debug.Print "'" & Binary2Hex(b) & "'"
'  CopyMemory ByVal VarPtr(d), b(0), 8
'  HexToDouble = d
'End Function
'
'Public Function HexToIntLittle(hex As String) As Long
'  Dim i As Long
'  For i = Len(hex) - 1 To 1 Step -2
'    'Debug.Print TwoHexits2Byte(Mid(hex, i, 2))
'    HexToIntLittle = HexToIntLittle * 256 + TwoHexits2Byte(Mid(hex, i, 2))
'  Next
'End Function
'
'Public Function HexToIntBig(hex As String) As Long
'  Dim i As Long
'  For i = 1 To Len(hex) - 1 Step 2
'    HexToIntBig = HexToIntBig * 16 + Hexit2Byte(Mid(hex, i, 2))
'  Next
'End Function

Private Function TwoHexits2Byte(str As String) As Byte
  TwoHexits2Byte = Hexit2Byte(Left(str, 1)) * 16 + Hexit2Byte(Right(str, 1))
End Function

Private Function Hexit2Byte(ch As String) As Byte
  Select Case Asc(ch)
    Case 48 To 57: Hexit2Byte = Asc(ch) - 48  '0-9
    Case 65 To 70: Hexit2Byte = Asc(ch) - 55  'A-F
    Case 97 To 102: Hexit2Byte = Asc(ch) - 87 'a-f
    Case Else: Debug.Print "Hexit2Byte: Invalid hex character ascii " & Asc(ch)
  End Select
End Function

Private Function NewNHDtable() As clsDBF
  Set NewNHDtable = New clsDBF
  With NewNHDtable
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CByte(Format(Now, "mm"))
    .Day = CByte(Format(Now, "dd"))
    .NumFields = 11
  
'    .FieldName(1) = "RCH_"
'    .FieldType(1) = "N"
'    .FieldLength(1) = 11
'    .FieldDecimalCount(1) = 0
'
'    .FieldName(2) = "RCH_ID"
'    .FieldType(2) = "N"
'    .FieldLength(2) = 11
'    .FieldDecimalCount(2) = 0
  
    .FieldName(1) = "COM_ID"
    .FieldType(1) = "N"
    .FieldLength(1) = 11
    .FieldDecimalCount(1) = 0
  
    .FieldName(2) = "RCH_CODE"
    .FieldType(2) = "C"
    .FieldLength(2) = 14
    .FieldDecimalCount(2) = 0
  
    .FieldName(3) = "RCH_DATE"
    .FieldType(3) = "C"
    .FieldLength(3) = 8
    .FieldDecimalCount(3) = 0
  
    .FieldName(4) = "LEVEL"
    .FieldType(4) = "N"
    .FieldLength(4) = 5
    .FieldDecimalCount(4) = 0
  
    .FieldName(5) = "METERS"
    .FieldType(5) = "N"
    .FieldLength(5) = 12
    .FieldDecimalCount(5) = 0
  
    .FieldName(6) = "GNIS_ID"
    .FieldType(6) = "C"
    .FieldLength(6) = 8
    .FieldDecimalCount(6) = 0
  
    .FieldName(7) = "NAME"
    .FieldType(7) = "C"
    .FieldLength(7) = 99
    .FieldDecimalCount(7) = 0
  
    .FieldName(8) = "RCHID"
    .FieldType(8) = "C"
    .FieldLength(8) = 10
    .FieldDecimalCount(8) = 0
  
    .FieldName(9) = "DSRCHID"
    .FieldType(9) = "C"
    .FieldLength(9) = 10
    .FieldDecimalCount(9) = 0
  
    .FieldName(10) = "FType"
    .FieldType(10) = "C"
    .FieldLength(10) = 10
    .FieldDecimalCount(10) = 0
  
    .FieldName(11) = "FCode"
    .FieldType(11) = "C"
    .FieldLength(11) = 10
    .FieldDecimalCount(11) = 0
  End With
End Function
