Attribute VB_Name = "ATCoGridDoc"
Option Explicit

Private Sub SampleUsage()
  Dim agd As ATCoGrid
  Dim rows As Long, cols As Long
  Dim NewDoc As Word.Document
  Set NewDoc = OpenGridAsDoc(agd)
  With NewDoc.Application.Selection
      cols = .Tables(1).Columns.Count
      .Tables(1).rows.Add
      rows = .Tables(1).rows.Count
      .Tables(1).Cell(rows, cols).Select
      .Text = "1024"
    With .Tables(1)
      cols = cols - 1
      While cols > 1
        .Cell(rows, cols).Merge .Cell(rows, cols - 1)
        cols = cols - 1
      Wend
    End With
    .Tables(1).Cell(rows, 1).Select
    .Text = "Grand Total"
  End With
  NewDoc.SaveAs "c:\test.doc"
  NewDoc.Close
End Sub

Public Function OpenGridAsDoc(agd As ATCoGrid) As Word.Document
  Dim WordApp As Word.Application
  Dim NewDoc As Word.Document
  Dim rows As Long, col As Long, cols As Long
  agd.copyAll
  rows = agd.rows + agd.FixedRows
  cols = 0
  For col = 0 To agd.cols
    If agd.colWidth(col) > 0 Then cols = cols + 1
  Next
  Set WordApp = New Word.Application
  With WordApp
    Set NewDoc = .Documents.Add
    NewDoc.PageSetup.Orientation = wdOrientLandscape
    
    .Selection.Text = Clipboard.GetText
    .Selection.find.ClearFormatting
    
    'Don't put header in table
    .Selection.find.Text = "^t"
    .Selection.find.Execute
    .Selection.HomeKey
    If .Selection.Start > 0 Then
      .Selection.Bookmarks.Add "tablestart"
      .Selection.Text = vbCr
      .Selection.Start = 0
      .Selection.ParagraphFormat.Alignment = wdAlignParagraphCenter
      .Selection.GoTo What:=wdGoToBookmark, Name:="tablestart"
      .Selection.Start = .Selection.Start + 1
    End If
    
    'Select everything after header
    .Selection.End = NewDoc.Characters.Count
    .Selection.Font.size = 8
    .Selection.ConvertToTable _
      Separator:=wdSeparateByTabs, _
      NumColumns:=cols, _
      NumRows:=rows, _
      Format:=wdTableFormatElegant, _
      ApplyBorders:=True, _
      ApplyShading:=True, _
      ApplyFont:=True, _
      ApplyColor:=True, _
      ApplyHeadingRows:=False, _
      ApplyLastRow:=False, _
      ApplyFirstColumn:=False, _
      ApplyLastColumn:=False, _
      AutoFit:=True, _
      AutoFitBehavior:=wdAutoFitContent
    .Selection.Tables(1).AutoFitBehavior wdAutoFitContent
    .Selection.Start = NewDoc.Characters.Count
    .Visible = True
  End With
  Set OpenGridAsDoc = NewDoc
End Function
