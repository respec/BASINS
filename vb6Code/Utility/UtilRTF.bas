Attribute VB_Name = "UtilRTF"
Option Explicit

'Return a string that includes inserted \ to escape special RTF characters \ { }
Public Function RTFescape(source$) As String
  Dim retval$, ch$, pos&
  For pos = 1 To Len(source)
    ch = Mid(source, pos, 1)
    Select Case ch
    Case "\", "{", "}": retval = retval & "\" & ch
    Case Else: retval = retval & ch
    End Select
  Next pos
  RTFescape = retval
End Function

Public Sub WriteRDBgrid(agd As ATCoGrid, f%, ByVal extraColTitle$, ByVal extraColText$, doColTitles As Boolean)
  Dim r&, C&, writingExtraCol As Boolean
  If Len(extraColTitle) > 0 And Len(extraColText) > 0 Then writingExtraCol = True
  
  With agd
  
    'Put column titles and definitions
    If doColTitles Then
      If writingExtraCol Then Put #f, , extraColTitle & vbTab
      For C = 0 To .cols - 2
        Put #f, , .ColTitle(C) & vbTab
      Next C
      Put #f, , .ColTitle(C) & vbCrLf
      
      'RDB column definitions: 12s = 12-character string, 3n = 3-digit number
      If writingExtraCol Then Put #f, , Len(extraColText) & "s" & vbTab
      For C = 0 To .cols - 1
        Select Case .colType(C)
          Case ATCoInt, ATCoSng: Put #f, , "n"
          Case Else:             Put #f, , "s"
        End Select
        If C < .cols - 1 Then Put #f, , vbTab
      Next C
      Put #f, , vbCrLf
    End If
    For r = 1 To .rows
      If writingExtraCol Then Put #f, , extraColText & vbTab
      For C = 0 To .cols - 2
        Put #f, , .TextMatrix(r, C) & vbTab
      Next C
      Put #f, , .TextMatrix(r, C) & vbCrLf
    Next r
  End With
End Sub

Public Sub WriteRTFgrid(agd As ATCoGrid, f%)
  Dim agdWidth&, colWidth&, x&, r&, C&, rowHeader$, borderSpec$
  With agd
    'total width of columns may be <> width of grid, so we calculate it
    agdWidth = 0
    For C = 0 To .cols - 1
      agdWidth = agdWidth + .colWidth(C)
    Next C
    x = 108
    rowHeader = Chr(10) & "\trowd \trgaph108\trleft-108\trbrdrh\brdrs\brdrw15 \trbrdrv\brdrs\brdrw15 \clbrdrb\brdrs\brdrw15 \clbrdrr\brdrs\brdrw15 "
    'rowHeader = Chr(10) & "\trowd \trgaph108\trleft-108 "
    'colWidth = 8855 / .Cols
    
    'make columns in RTF proportional to columns in grid
    For C = 0 To .cols - 1
      colWidth = 8855 * .colWidth(C) / agdWidth
      x = x + colWidth
      rowHeader = rowHeader & "\cellx" & x & "\clbrdrl\brdrs\brdrw15 \clbrdrb\brdrs\brdrw15 \clbrdrr\brdrs\brdrw15 "
    Next C
    rowHeader = rowHeader & " \pard\plain \intbl "
    For r = 0 To .rows
      Put #f, , rowHeader
      For C = 0 To .cols - 1
        Put #f, , .TextMatrix(r, C) & "\cell "
      Next C
      Put #f, , " \pard \intbl \row "
    Next r
    Put #f, , "\pard \par "
  End With
End Sub

