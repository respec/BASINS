Attribute VB_Name = "modCount"
Option Explicit

Public Sub CountAll()
  Dim d$, f&, l&
  
  Screen.MousePointer = vbHourglass
  
  d = CurDir

  ChDir PathNameOnly(TopItem.Path)
  f = FreeFile(0)
  Open "c:\Count\" & FilenameOnly(TopItem.Name) & ".cnt" For Output As #f
  l = 0
  
  CountMe TopItem, f, l, FilenameOnly(TopItem.Name), ""
  
  Close #f
  
  ChDir d
  
  Screen.MousePointer = vbDefault
End Sub

Private Sub CountMe(item As clsVBitem, f&, l&, g$, p$)
  Dim i&, s$, lComment&, lInline&, lStatement&, lBlank&, lSubFun&, lTotal&
  
  CountDetails item.Body, lComment, lInline, lStatement, lBlank, lSubFun, lTotal
  If item.VBItype > 0 And item.VBItype <> vbi_Group Then
    Print #f, l & ", " & g & ", " & _
                    p & ", " & _
                    item.Name & ", " & _
                    item.nItems & ", " & _
                    item.VBItype & ", " & _
                    lComment & ", " & _
                    lInline & ", " & _
                    lStatement & ", " & _
                    lBlank & ", " & _
                    lTotal & ", " & _
                    lSubFun
  End If
  For i = 1 To item.nItems
    l = l + 1
    If l = 1 Then
      p = item.item(i).Name
    End If
    CountMe item.item(i), f, l, g, p
  Next i
  l = l - 1
End Sub

Private Sub CountDetails(lBody$, lComment&, lInline&, lStatement&, lBlank&, lSubFun&, lTotal&)
  Dim s$, l$
  
  s = lBody
  
  lBlank = 0
  lComment = 0
  lInline = 0
  lStatement = 0
  lTotal = 0
  lSubFun = 0
  
  While Len(s) > 0
    lTotal = lTotal + 1
    l = UCase(Trim(VBnextLine(s)))
    If Left(l, 1) = "'" Then
      lComment = lComment + 1
    ElseIf InStr(1, l, "'") Then
      lInline = lInline + 1
      QSubFun l, lSubFun
    ElseIf Len(l) <= 1 Then
      lBlank = lBlank + 1
    Else
      lStatement = lStatement + 1
      QSubFun l, lSubFun
    End If
  Wend
  
End Sub

Private Sub QSubFun(s$, lSubFun&)
  If InStr(1, s, "SUB ") Or InStr(1, s, "FUNCTION ") Or InStr(1, s, "PROPERTY GET ") Then
    lSubFun = lSubFun + 1
  End If
End Sub
