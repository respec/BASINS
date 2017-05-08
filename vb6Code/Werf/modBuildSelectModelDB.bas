Attribute VB_Name = "modBuildSelectModelDB"
Option Explicit
Dim myDb As Database
Dim myRs As Recordset
Dim myTabDef As TableDef
Dim myIndx As Index
Dim myCriteria As Recordset
Dim mygroups As Recordset
Dim myvalues As Recordset
Dim myModels As Recordset
Dim myDetails As Recordset
Dim XLApp As Excel.Application
Dim XLBook As Excel.Workbook
Dim XLsheet As Excel.Worksheet

Sub Main()
  Dim i&, n$
  Dim hdle&, EXEName$, BasePath$
  Dim s As String * 80
  Dim RunningVB As Boolean
  Dim xlsFilename As String
  Dim mdbFilename As String
  
'  hdle = GetModuleHandle("BuildSelectModelDB")
'  i = GetModuleFileName(hdle, s, 80)
'  EXEName = UCase(Left(s, InStr(s, Chr(0)) - 1))
'  If InStr(EXEName, "VB6.EXE") Then
'    RunningVB = True
'    EXEName = "c:\vbexperimental\werf\BuildSelectSelectModelDB.exe"
'  Else
'    RunningVB = False
'  End If
'  ChDrive (Left(EXEName, 2))
'  BasePath = PathNameOnly(EXEName)
'  If UCase(Right(BasePath, 4)) = "\BIN" Then BasePath = Left(BasePath, Len(BasePath) - 4)
'  ChDir (BasePath & "\data")
  
  With frm.cdlg
    .DialogTitle = "Open Model Descriptions"
    .Filter = "Excel Spreadsheet (*.xls)|*.xls"
    .FilterIndex = 1
    .CancelError = True
    On Error GoTo Nevermind
    .ShowOpen
    xlsFilename = .Filename
        
    .Filename = "modelSelection.mdb"
    .DialogTitle = "Save Database As..."
    .Filter = "Access Database (*.mdb)|*.mdb"
    .FilterIndex = 1
    .ShowSave
    mdbFilename = .Filename
  End With
  
  On Error GoTo ErrHandler
  
  frm.Show
  'MsgBox "Wait for Build Complete Message"
  'use copy of existing for lookup tables of valid values
  BuildDB mdbFilename
  
  Set myDb = OpenDatabase(mdbFilename)
  Set myRs = myDb.OpenRecordset("Types", dbOpenDynaset)
  Set myCriteria = myDb.OpenRecordset("Criteria", dbOpenDynaset)
  Set mygroups = myDb.OpenRecordset("Groups", dbOpenDynaset)
  Set myvalues = myDb.OpenRecordset("Values", dbOpenDynaset)
  Set myModels = myDb.OpenRecordset("Models", dbOpenDynaset)
  Set myDetails = myDb.OpenRecordset("Details", dbOpenDynaset)
  
  On Error GoTo ErrExcel
  Set XLApp = New Excel.Application
  Set XLBook = Workbooks.Open(xlsFilename)
  With ActiveWorkbook
    For i = 1 To .Worksheets.Count
      n = .Worksheets(i).Name
      If Right(n, 4) <> "_Rpt" Then
        myRs.AddNew
        myRs!Name = n
        myRs.Update
        Set XLsheet = .Worksheets(i)
        Call ReadSheet(n)
      End If
    Next i
  End With
 
  myDb.Close
  XLApp.Quit
  Unload frm

Nevermind:
  End
  
ErrHandler:
  MsgBox Err.Description, vbCritical, "Build Select Model Database"
  End

ErrExcel:
  MsgBox "Error starting Excel: " & vbCr & Err.Description, vbCritical, "Build Select Model Database"
  End
End Sub

Sub ReadSheet(t$)
  Dim i&, j&, g$, c$, v$, cx$, M$, clast&
  
  For i = 2 To 1000
    g = XLsheet.Cells(2, i)
    cx = XLsheet.Cells(3, i)
    v = XLsheet.Cells(4, i)
    If Len(g) = 0 And Len(cx) = 0 And Len(v) = 0 Then
      clast = i - 1
      Exit For
    End If
    
    If Len(g) > 0 Then
      mygroups.AddNew
      mygroups!Name = g
      mygroups!Type = t
      mygroups.Update
    Else
      g = " "
    End If
    
    If Len(cx) > 0 Then
      c = cx
      myCriteria.AddNew
      myCriteria!Group = g
      myCriteria!Type = t
      myCriteria!Name = c
      myCriteria.Update
    End If
    
    myvalues.AddNew
    myvalues!Criteria = c
    myvalues!Type = t
    If Len(v) > 0 Then myvalues!Name = v Else myvalues!Name = "?"
    myvalues.Update
  Next i
  
  For j = 5 To 1000
    M = XLsheet.Cells(j, 1)
    If Len(M) = 0 Then
      If j > 5 Then 'not blank at start
        Exit For
      End If
    ElseIf Len(M) > 0 Then
      myModels.AddNew
      myModels!Name = M
      myModels!Type = t
      myModels.Update
      For i = 2 To clast
        cx = XLsheet.Cells(3, i)
        If Len(cx) > 0 Then
          c = cx
        End If
        If Len(XLsheet.Cells(j, i)) > 0 Then 'save this detail
          myDetails.AddNew
          myDetails!Type = t
          myDetails!Model = M
          myDetails!Criteria = c
          myDetails!Value = XLsheet.Cells(4, i)
          myDetails.Update
        End If
      Next i
    End If
  Next j
End Sub

Sub BuildDB(mdbFilename As String) ' fill in tables on blank db
  Dim vTypes, vType
  Dim myField As Field
  
  On Error Resume Next
  Kill mdbFilename
  On Error GoTo ErrHand
  Set myDb = CreateDatabase(mdbFilename, dbLangGeneral, 0)
  
  Set myTabDef = myDb.CreateTableDef("Types")
  With myTabDef
    .Fields.Append .CreateField("Name", dbText, 32)
  End With
  myDb.TableDefs.Append myTabDef
  
  Set myTabDef = myDb.CreateTableDef("Groups")
  With myTabDef
    .Fields.Append .CreateField("Name", dbText, 32)
    Set myField = .CreateField("Type", dbText, 32)
    'myTabDef.SourceTableName "Types"
    'myField.SourceField = "Name"
    .Fields.Append myField
  End With
  myDb.TableDefs.Append myTabDef
  Set myTabDef = myDb.CreateTableDef("Criteria")
  With myTabDef
    .Fields.Append .CreateField("Name", dbText, 48)
    .Fields.Append .CreateField("Group", dbText, 32)
    .Fields.Append .CreateField("Type", dbText, 32)
  End With
  myDb.TableDefs.Append myTabDef

  Set myTabDef = myDb.CreateTableDef("Details")
  With myTabDef
    .Fields.Append .CreateField("Type", dbText, 32)
    .Fields.Append .CreateField("Model", dbText, 48)
    .Fields.Append .CreateField("Criteria", dbText, 48)
    .Fields.Append .CreateField("Value", dbText, 32)
  End With
  myDb.TableDefs.Append myTabDef
   
  Set myTabDef = myDb.CreateTableDef("Models")
  With myTabDef
    .Fields.Append .CreateField("Name", dbText, 48)
    .Fields.Append .CreateField("Type", dbText, 32)
  End With
  myDb.TableDefs.Append myTabDef
  
  Set myTabDef = myDb.CreateTableDef("Values")
  With myTabDef
    .Fields.Append .CreateField("Name", dbText, 48)
    .Fields.Append .CreateField("Criteria", dbText, 32)
    .Fields.Append .CreateField("Type", dbText, 32)
  End With
  myDb.TableDefs.Append myTabDef
 
  Exit Sub
  
ErrHand:
  MsgBox "Error building database " & mdbFilename & vbCr & Err.Description, vbCritical, "Build Select Model Database"
  End
  
End Sub
