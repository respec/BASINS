Attribute VB_Name = "modTSCol"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
'Dim pAvailTSCol As Collection
'
'Public Function AvailTSColNameCount()
'  AvailTSColNameCount = pAvailTSCol.Count
'End Function
'
'Public Function AvailTSColName(Index&) As String
'
'  If pAvailTSCol Is Nothing Then AvailTSColNameInit
'
'  If Index < 0 Or Index > pAvailTSCol.Count - 1 Then
'    AvailTSColName = ""
'  Else
'    AvailTSColName = pAvailTSCol(Index + 1)
'  End If
'
'End Function
'
'Public Sub AvailTSColNameAdd(newName As String)
'  If pAvailTSCol Is Nothing Then AvailTSColNameInit
'  On Error Resume Next
'  pAvailTSCol.Add newName, newName
'End Sub
'
'Public Sub AvailTSColNameInit(Optional ts As Collection)
'  Dim i As Long, lAt As Variant
'
'  Set pAvailTSCol = Nothing
'
'  Set pAvailTSCol = New Collection
'
'  If IsMissing(ts) Then
'  ElseIf ts Is Nothing Then
'  ElseIf ts.Count < 1 Then
'  Else
'    For Each lAt In ts(1).AttribNames
'      pAvailTSCol.Add lAt, lAt
'    Next
'    On Error Resume Next
'    For i = 2 To ts.Count
'      For Each lAt In ts(i).Attribs
'        pAvailTSCol.Add lAt.Name, lAt.Name
'      Next
'    Next
'  End If
'End Sub


