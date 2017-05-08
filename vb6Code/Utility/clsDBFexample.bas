Attribute VB_Name = "clsDBFexample"
Option Explicit

Sub main()
  Dim fldNum As Long
  Dim i As Long
  Dim tmpDBF As clsDBF
  Set tmpDBF = New clsDBF
  tmpDBF.OpenDBF "c:\foo.dbf"
  fldNum = tmpDBF.FieldNumber("NAME")
  For i = 1 To tmpDBF.NumRecords
    tmpDBF.CurrentRecord = i
    Debug.Print tmpDBF.Value(fldNum)
  Next
End Sub
 
