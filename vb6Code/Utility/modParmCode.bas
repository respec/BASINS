Attribute VB_Name = "modParmCode"
Option Explicit
Private pIdIndex As FastCollection

Public Function ParmCodeTable() As clsATCTable
  Static AlreadyReportedErrOpen As Boolean
  Static openedDB As Boolean
  Static lParmCodeDatabase As clsATCTable
  Dim i As Long
  
  If Not openedDB Then
    Dim DBpath As String
    Dim ff As New ATCoFindFile
    On Error GoTo erropen
    
    ff.SetRegistryInfo "ATCoParmCode", "Path", "ATCoParmCode"
    ff.SetDialogProperties "Please locate 'ATCoParmCode.dbf'", "ATCoParmCode.dbf"
    DBpath = ff.GetName
    
    If LCase(FileExt(DBpath)) = "mdb" Then
      DBpath = FilenameNoExt(DBpath) & ".dbf"
    End If
    
    If FileExists(DBpath) Then
      Set lParmCodeDatabase = New clsATCTableDBF
      lParmCodeDatabase.OpenFile DBpath
      openedDB = True
      Set pIdIndex = New FastCollection
      lParmCodeDatabase.MoveFirst
      For i = 1 To lParmCodeDatabase.NumRecords
        pIdIndex.Add i, lParmCodeDatabase.Value(1)
        lParmCodeDatabase.MoveNext
      Next
      lParmCodeDatabase.MoveFirst
    End If
  End If
  Set ParmCodeTable = lParmCodeDatabase
  Exit Function
erropen:
  If Not AlreadyReportedErrOpen Then
    MsgBox "Error opening color database '" & DBpath & "'" & vbCr & err.Description
    AlreadyReportedErrOpen = True
  End If
  Set ParmCodeTable = Null
End Function

Public Function FindParmCode(aId As Long, aParmCodeTable As clsATCTable) As Boolean
  Dim lind As Long
  
  If Not pIdIndex Is Nothing Then
    lind = pIdIndex.IndexFromKey(aId)
  End If
  
  If lind > 0 Then
    aParmCodeTable.CurrentRecord = lind
    FindParmCode = True
  Else
    FindParmCode = False
  End If
End Function
