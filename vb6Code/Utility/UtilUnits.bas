Attribute VB_Name = "UtilUnits"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Const UnitsTableName = "Units"
Private Const CategoryTableName = "Category"

'Debug.Print GetParameterUnits("LAI", "SwatDbfParameter", "sbs")
Public Function GetParameterUnits(ByVal ParameterName As String, _
                                  ByVal TableName As String, _
                                  Optional ByVal FileType As String = "") As String
  Static AlreadyReportedError As Boolean
  Dim rs As Recordset
  Dim unitsID As Variant
    
  On Error GoTo ErrHand
  
  GetParameterUnits = "Unknown"
  
  If Len(FileType) = 0 Then
    unitsID = GetField(ParameterName, "UnitsID", TableName)
  Else
    Set rs = GetRecordSet("Select " & TableName & ".UnitsID from " & TableName, _
                          " where " & TableName & ".Name='" & ParameterName & "'" _
                          & " and " & TableName & ".FileType='" & FileType & "'", "")
    If Not rs Is Nothing Then
      If Not rs.EOF Then unitsID = rs.Fields(0).Value
      rs.Close
    End If
  End If
  If IsNumeric(unitsID) Then
    GetParameterUnits = GetField(unitsID, "Name", UnitsTableName)
  End If
  Exit Function

ErrHand:
  If Not AlreadyReportedError Then
    MsgBox "Error in GetParameterUnits" & vbCr & err.Description
    AlreadyReportedError = True
  End If
End Function

Public Function GetConversionFactor(ByVal fromUnits As String, ByVal toUnits As String) As Double
  Static AlreadyReportedError As Boolean
  Dim vConversionFrom As Variant
  Dim vConversionTo As Variant
  
  On Error GoTo ErrHand
  
  vConversionFrom = GetField(fromUnits, "PerDefaultUnit", UnitsTableName)
  vConversionTo = GetField(toUnits, "PerDefaultUnit", UnitsTableName)
  If IsNumeric(vConversionFrom) And IsNumeric(vConversionTo) And vConversionFrom <> 0 Then
    GetConversionFactor = CDbl(vConversionTo) / CDbl(vConversionFrom)
  End If
  
  Exit Function
  
ErrHand:
  If Not AlreadyReportedError Then
    MsgBox "Error in GetConversionFactor from: " & fromUnits & " to: " & toUnits & vbCr & err.Description
    AlreadyReportedError = True
  End If

End Function

Public Function GetUnitDescription(ByVal unitsName As String) As String
  GetUnitDescription = GetField(unitsName, "Description", UnitsTableName)
End Function

Public Function GetUnitID(ByVal unitsName As String) As Long
  Dim retval As Variant
  retval = GetField(unitsName, "ID", UnitsTableName)
  If Not IsNumeric(retval) Then retval = 0
  GetUnitID = CLng(retval)
End Function

Public Function GetUnitName(ByVal unitsID As Long) As String
  Dim retval As Variant
  retval = GetField(unitsID, "Name", UnitsTableName)
  GetUnitName = CStr(retval)
End Function

Public Function GetUnitCategory(ByVal unitsName As String) As String
  Static AlreadyReportedError As Boolean
  Dim CategoryID As Variant
    
  On Error GoTo ErrHand
  
  GetUnitCategory = "Unknown"
  CategoryID = GetField(unitsName, "CategoryID", UnitsTableName)
  If IsNumeric(CategoryID) Then
    GetUnitCategory = GetField(CategoryID, "Name", CategoryTableName)
  End If
  Exit Function

ErrHand:
  If Not AlreadyReportedError Then
    MsgBox "Error in GetUnitCategory" & vbCr & err.Description
    AlreadyReportedError = True
  End If
End Function

'If Category = "all" then all units in all categories are returned
Public Function GetAllUnitsInCategory(ByVal Category As String) As FastCollection
  Static AlreadyReportedError As Boolean
  Dim CategoryID As Long
  Dim rs As Recordset
  Dim retval As FastCollection
  
  On Error GoTo ErrHand
  
  Set retval = New FastCollection
  Set GetAllUnitsInCategory = retval
  
  If LCase(Category) = "all" Then
    Set rs = GetRecordSet("Select Units.Name from Units", " where Units.Name<>'Unknown'", "")
  Else
    CategoryID = GetField(Category, "ID", CategoryTableName)
    Set rs = GetRecordSet("Select Units.Name from Units", " where Units.CategoryID=" & CategoryID, "")
  End If
  
  If Not rs Is Nothing Then
    While Not rs.EOF
      retval.Add rs.Fields(0).Value
      rs.MoveNext
    Wend
    rs.Close
  End If
  
  Exit Function

ErrHand:
  If Not AlreadyReportedError Then
    MsgBox "Error in GetAllUnitsInCategory for: " & Category & vbCr & err.Description
    AlreadyReportedError = True
  End If
End Function

Public Function GetAllUnitCategories() As FastCollection
  Static AlreadyReportedError As Boolean
  Dim db As Database
  Dim rs As Recordset
  Dim retval As FastCollection
  
  On Error GoTo ErrHand
  
  Set retval = New FastCollection
   
  Set rs = GetRecordSet("Select Category.Name from Category ", " where Category.Name<>'Unknown'", "")
  If Not rs Is Nothing Then
    While Not rs.EOF
      retval.Add rs.Fields(0).Value
      rs.MoveNext
    Wend
    rs.Close
  End If
  
  Set GetAllUnitCategories = retval
    
  Exit Function

ErrHand:
  Set GetAllUnitCategories = retval
  If Not AlreadyReportedError Then
    MsgBox "Error in GetAllUnitCategories" & vbCr & err.Description
    AlreadyReportedError = True
  End If
End Function

Private Function GetField(ByVal IDorName As String, ByVal FieldName As String, ByVal table As String) As Variant
  Static AlreadyReportedError As Boolean
  Dim rs As Recordset
  
  On Error GoTo ErrHand
  
  GetField = "<none>"
  Set rs = GetRecordSet("Select " & table & "." & FieldName & " from " & table, IDorName, table)
  If Not rs Is Nothing Then
    If Not rs.EOF Then GetField = rs.Fields(0).Value
    rs.Close
  End If
    
  Exit Function

ErrHand:
  If Not AlreadyReportedError Then
    MsgBox "Error in GetField " & FieldName & " for: " & IDorName & " in " & table & vbCr & err.Description
    AlreadyReportedError = True
  End If
End Function

Private Function GetRecordSet(sql As String, IDorName As String, table As String) As Recordset
  Dim rs As Recordset
  Dim db As Database
  Set db = unitsDB
  If Not db Is Nothing Then
    If IsNumeric(IDorName) Then
      Set GetRecordSet = db.OpenRecordset(sql & " where " & table & ".ID=" & IDorName, dbOpenDynaset, dbReadOnly, dbReadOnly)
    ElseIf Left(IDorName, 6) = " where" Then
      Set GetRecordSet = db.OpenRecordset(sql & IDorName, dbOpenDynaset, dbReadOnly, dbReadOnly)
    Else
      Set rs = db.OpenRecordset(sql & " where " & table & ".Name='" & IDorName & "'", dbOpenDynaset, dbReadOnly, dbReadOnly)
      If (rs.NoMatch Or rs.EOF) And table = UnitsTableName Then 'Try long name (Description) if short name wasn't found
        Set rs = db.OpenRecordset(sql & " where Units.Description='" & IDorName & "'", dbOpenDynaset, dbReadOnly, dbReadOnly)
      End If
      Set GetRecordSet = rs
    End If
  End If
End Function

Private Function unitsDB() As Database
  Static SaveUnitsDatabase As Database
  Static AlreadyReportedErrOpen As Boolean
  
  If SaveUnitsDatabase Is Nothing Then
    Dim DBpath As String
    Dim ff As New ATCoFindFile
    On Error GoTo erropen
    
    ff.SetRegistryInfo "ATCoCtl", "ATCoUnits", "Path"
    ff.SetDialogProperties "Please locate 'ATCoUnits.mdb' in a writable folder", "ATCoUnits.mdb"
    DBpath = ff.GetName
    
    If Len(DBpath) > 0 Then
      If Len(Dir(DBpath)) > 0 Then
        Set SaveUnitsDatabase = OpenDatabase(DBpath, False, True)
      End If
    End If
  End If
  Set unitsDB = SaveUnitsDatabase
  Exit Function
erropen:
  If Not AlreadyReportedErrOpen Then
    MsgBox "Error opening units database '" & DBpath & "'" & vbCr & err.Description
    AlreadyReportedErrOpen = True
  End If
  Set unitsDB = Null
End Function

