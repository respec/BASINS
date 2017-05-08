Attribute VB_Name = "MapCol"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private RecSet As Recordset
Public AvailableFields() As MapFieldInfo
Public nAvailableFields As Long
Public MapColsOk As Boolean

Public Sub ClearFrmMapCols()
  If nAvailableFields > 0 Then
    ReDim AvailableFields(0)
    ReDim MapCols(0)
    nAvailableFields = 0
    On Error Resume Next
  End If
End Sub

'Public Sub AddColumn(TableName$, ColumnName$)
'  Dim t&, found As Boolean
'  t = 1
'  found = False
'  On Error GoTo RedimMapCols0
'  While t <= UBound(MapCols) And Not found
'    If MapCols(t).Name = TableName Then
'      ReDim Preserve MapCols(t).Fields(UBound(MapCols(t).Fields) + 1)
'      With MapCols(t).Fields(UBound(MapCols(t).Fields))
'        .Name = ColumnName
'        .Caption = .Name
'        .Column = 1
'      End With
'      found = True
'    Else
'      t = t + 1
'    End If
'  Wend
'IfNotFound:
'  If Not found Then
'    ReDim Preserve MapCols(UBound(MapCols) + 1)
'    With MapCols(UBound(MapCols))
'      .Name = TableName
'      ReDim .Fields(0)
'      .Fields(0).Name = ColumnName
'      .Fields(0).Caption = ColumnName
'      .Fields(0).Column = 1 ' .Visible = True
'    End With
'  End If
'  Exit Sub
'
'RedimMapCols0:
'  ReDim MapCols(0)  ' I hate VB
'  GoTo IfNotFound
'End Sub

Public Sub MapColAddField(ByVal fiName$, ByVal fiCaption$, ByVal fiColumn&, frm As Form) 'fi As MapFieldInfo)
  Dim r&, found As Boolean
  r = 0: found = False
  If nAvailableFields = 0 Then
    nAvailableFields = 1
    ReDim AvailableFields(0)
    GoTo SetValues
  End If
  
  While r < nAvailableFields And Not found
    If fiName = AvailableFields(r).Name Then found = True Else r = r + 1
  Wend
  If Not found Then
    r = nAvailableFields
    ReDim Preserve AvailableFields(r)
    nAvailableFields = nAvailableFields + 1
    'maybe we should also try to put this field in order according to AvailableFields(r).Column
    'by shifting values already present and changing r before SetValues below
  Else 'Already have this field at AvailableFields(r)
    If AvailableFields(r).Name = AvailableFields(r).Caption Then
      AvailableFields(r).Caption = fiCaption
    End If
    MapColSetFrameValues r, frm
    Exit Sub 'May want to do something else, but this works for now.
  End If

SetValues:
  AvailableFields(r).Name = fiName
  AvailableFields(r).Caption = fiCaption
  AvailableFields(r).Column = fiColumn
  MapColAddFrame r, frm
  MapColSetFrameValues r, frm
End Sub

Public Sub MapColSelectField(ByVal i&, sel As Boolean)
  If sel Then
    AvailableFields(i).Column = 99
  Else
    AvailableFields(i).Column = -1
  End If
End Sub

Private Sub MapColSetFrameValues(i&, fra As Object)
 With fra
  Dim af As MapFieldInfo
  af = AvailableFields(i)
  .lblColumn(i).Caption = af.Name
  .txtAlias(i).Text = af.Caption
  If af.Column >= 0 Then .chkTable(i).Value = 1 Else .chkTable(i).Value = 0
    
  On Error GoTo ExitSub
  .lblType(i).Caption = NameOfDBtype(RecSet.Fields(af.Name).Type)
  
  '.comboStyle(i).Clear
  'If l.symbol.SymbolType = moPointSymbol Then 'point
  '  comboStyle(i).AddItem "Circle", 0
  '  comboStyle(i).AddItem "Square", 1
  '  comboStyle(i).AddItem "Triangle", 2
  '  comboStyle(i).AddItem "Cross", 3
  '  lblType(i).Caption = "P"
  'End If
  'If l.symbol.Style < comboStyle(i).ListCount Then comboStyle(i).ListIndex = l.symbol.Style
 End With
ExitSub:
  On Error GoTo 0
End Sub

Private Sub MapColAddFrame(ByVal i&, frm As Object)
 With frm
  If i <= .fraColumn.ubound Then 'Already have this frame
    .fraColumn(i).Visible = True
  Else
    'another frame
    Load .fraColumn(i)
    .fraColumn(i).Top = .fraColumn(i - 1).Top + .fraColumn(i - 1).Height
    .fraColumn(i).BorderStyle = 0
    .fraColumn(i).Visible = True
    Load .lblType(i)
    Set .lblType(i).Container = .fraColumn(i)
    .lblType(i).Visible = True
    'another Column name
    Load .lblColumn(i)
    Set .lblColumn(i).Container = .fraColumn(i)
    .lblColumn(i).Visible = True
    'another label
    Load .txtAlias(i)
    Set .txtAlias(i).Container = .fraColumn(i)
    .txtAlias(i).Visible = True
    'another map show check box
    Load .chkMapCov(i)
    Set .chkMapCov(i).Container = .fraColumn(i)
'    chkMapCov(i).Visible = True
    'another table show
    Load .chkTable(i)
    Set .chkTable(i).Container = .fraColumn(i)
    '.chkTable(i).Value = 0
    .chkTable(i).Visible = True
    
    'another combobox
    Load .comboStyle(i)
    Set .comboStyle(i).Container = .fraColumn(i)
'    comboStyle(i).Visible = True
  End If
 End With
End Sub

Public Sub MapColSetRecordset(rs As Recordset, frm As Form)
  With frm
    Dim r&, i&
    On Error GoTo SetCode
    For r = 0 To RecSet.Fields.Count - 1
      .fraColumn(r).Visible = False
    Next r
SetCode:
    On Error GoTo AbortSub
    Set RecSet = rs
    i = RecSet.Fields.Count - 1
    If i > 30 Then
      'too many columns causes out of memory error  11/15/99 pbd
      'Changed from 20 to 30 Aug 24, 2000 mg - need to replace this with a grid
      i = 30
    End If
    For r = 0 To i

      With RecSet.Fields(r)
        MapColAddField .Name, .Name, -1, frm
      End With
    Next r
  End With
AbortSub:
End Sub

Public Sub MapColOption_Click(Index As Integer, frm As Object)
 With frm
  Dim tmpcol As MapFieldInfo, i&
    
  If Index = 2 Then 'Move Up
    If .CurColumn > 0 Then
      tmpcol = AvailableFields(.CurColumn)
      AvailableFields(.CurColumn) = AvailableFields(.CurColumn - 1)
      AvailableFields(.CurColumn - 1) = tmpcol
      MapColSetFrameValues .CurColumn, frm
      .SelectColumn .CurColumn - 1
      MapColSetFrameValues .CurColumn, frm
      If .VScroll.Visible And .CurColumn < .VScroll.Value Then
        .VScroll.Value = .CurColumn
      Else
        MapColsResize frm
      End If
    End If
  ElseIf Index = 3 Then 'Move Down
    If .CurColumn < UBound(AvailableFields) Then
      tmpcol = AvailableFields(.CurColumn)
      AvailableFields(.CurColumn) = AvailableFields(.CurColumn + 1)
      AvailableFields(.CurColumn + 1) = tmpcol
      MapColSetFrameValues .CurColumn, frm
      .SelectColumn .CurColumn + 1
      MapColSetFrameValues .CurColumn, frm
      If .VScroll.Visible And .fraColumn(.CurColumn).Left > .Width Then
        .VScroll.Value = .VScroll.Value + 1
      Else
        MapColsResize frm
      End If
    End If
  End If
 End With
End Sub

Public Function NameOfDBtype$(ByVal typ&)
  NameOfDBtype = "Undefined"
  Select Case typ
  Case dbBigInt
    NameOfDBtype = "Big Integer"
  Case dbBinary
    NameOfDBtype = "Binary"
  Case dbBoolean
    NameOfDBtype = "Boolean"
  Case dbByte
    NameOfDBtype = "Byte"
  Case dbChar
    NameOfDBtype = "Char"
  Case dbCurrency
    NameOfDBtype = "Currency"
  Case dbDate
    NameOfDBtype = "Date / Time"
  Case dbDecimal
    NameOfDBtype = "Decimal"
  Case dbDouble
    NameOfDBtype = "Double"
  Case dbFloat
    NameOfDBtype = "Float"
  Case dbGUID
    NameOfDBtype = "Guid"
  Case dbInteger
    NameOfDBtype = "Integer"
  Case dbLong
    NameOfDBtype = "Long"
  Case dbLongBinary
    NameOfDBtype = "Long Binary"
  Case dbMemo
    NameOfDBtype = "Memo"
  Case dbNumeric
    NameOfDBtype = "Numeric"
  Case dbSingle
    NameOfDBtype = "Single"
  Case dbText
    NameOfDBtype = "Text"
  Case dbTime
    NameOfDBtype = "Time"
  Case dbTimeStamp
    NameOfDBtype = "Time Stamp"
  Case dbVarBinary
    NameOfDBtype = "VarBinary"
  End Select
End Function

Public Sub MapColsResize(frm As Object)
  Dim l&, TopRow&, BotRow&
  If nAvailableFields = 0 Then Exit Sub
  With frm
    .fraButtons.Top = .Height - 2.5 * .fraButtons.Height
    
    Dim f&, pos&, dy&
    pos = .TopFrameTop
    dy = .fraColumn(0).Height
    
    If .VScroll.Visible Then
      For f = LBound(AvailableFields) To .VScroll.Value
        .fraColumn(f).Left = .Width + 100
      Next f
      f = .VScroll.Value
    Else
      f = 0
    End If
    TopRow = f
    BotRow = UBound(AvailableFields)
    While f <= UBound(AvailableFields)
      If .fraColumn(f).Visible Then
        .fraColumn(f).Top = pos
        pos = pos + dy
        If pos < .fraButtons.Top Then
          .fraColumn(f).Left = .TopFrameLeft
        Else
          .fraColumn(f).Left = .Width + 100
          If f <= BotRow Then BotRow = f - 1
        End If
      End If
      f = f + 1
    Wend
    If BotRow < UBound(AvailableFields) Or TopRow > 0 Then 'need scrollbar
      If .VScroll.Visible Then
        .VScroll.Visible = False
      Else
        .VScroll.Value = LBound(AvailableFields)
      End If
      .VScroll.Min = LBound(AvailableFields)
      .VScroll.Max = UBound(AvailableFields) - (BotRow - TopRow)
      If (BotRow - TopRow > 1) Then
        .VScroll.LargeChange = BotRow - TopRow
      Else
        .VScroll.LargeChange = 2
      End If
      .VScroll.Left = .Width - .VScroll.Width * 1.5
      If .fraButtons.Top - .VScroll.Top > 100 Then .VScroll.Height = .fraButtons.Top - .VScroll.Top
      .VScroll.Visible = True
    Else
      .VScroll.Visible = False
      If pos < .Height - 3 * .cmdClose(0).Height Then
        .Height = pos + dy + 2.5 * .cmdClose(0).Height
        .fraButtons.Top = .Height - 2.5 * .fraButtons.Height
      End If
    End If
  End With
End Sub

