VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "clsBinFileView"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit
Implements clsFileView

Private frmPar As Form       'parent form (frmFileView)

Private hexchar$(256)        '2-character hexadecimal to print for each byte value

Private Sub Class_Initialize()
  Dim ch$(16), i&
  For i = 0 To 9
    ch(i) = Chr(48 + i)        'Debug.Print i, ch(i)
  Next i
  For i = 97 To 102 '65 to 70 for upper case
    ch(i - 87) = Chr(i)        'Debug.Print i - 87, ch(i - 87)
  Next i
  For i = 0 To 255
    hexchar(i) = ch((i And 240) / 16) & ch(i And 15)        'Debug.Print i, hexchar(i)
  Next i
End Sub

Public Function clsFileView_Desc() As String
  clsFileView_Desc = "Binary"
End Function

Public Sub clsFileView_SaveChanges()
  MsgBox "BinFileView:SaveChanges"
End Sub
Public Sub clsFileView_OpenFile(frmParent As Form)
  Set frmPar = frmParent
  Open frmPar.Filename For Binary As #frmPar.FileHandle
  
  frmPar.AddOption "Word Width", ""
  frmPar.AddOption "Line Break", ""
  frmPar.AddOption "Page Break", ""
  frmPar.AddOption "Line Numbers", True
  
  If frmPar.TextWidth("1") <> frmPar.TextWidth("W") Then 'proportional font in use
    frmPar.FontName = "Courier New"
    frmPar.SetFont frmPar
  End If
End Sub

Public Function clsFileView_ScrollPos(ByVal Direction&, ByVal Units&, ByVal Distance&) As Long
  Dim retval& 'return value
  Dim line&, page&, unit&           'counter to count up/down to Distance
  Dim bytesPerWord&, maxBytesPerLine&, linesperpage&, LineNumbering As Boolean

  Dim cbuff$, nospacebuff$          'character buffers for reading lines of file
  Dim FirstByteRead&, NextByte& 'first and last byte just read from file
  Dim EOLstring$, LenEOL&, CheckEOL As Boolean
  Dim EOPstring$, LenEOP&, CheckEOP As Boolean, EOPfound As Boolean
  
  SetPageParams bytesPerWord, maxBytesPerLine, linesperpage, LineNumbering, EOLstring, LenEOL, CheckEOL, EOPstring, LenEOP, CheckEOP
  
  If Direction * Units * Distance = 0 Then ' no scrolling necessary, return frmPar.FirstDispByte
    retval = frmPar.FirstDispByte
  Else    'Figure out how far to move display pointer
    Seek frmPar.FileHandle, frmPar.FirstDispByte
    Select Case Units
      Case 1: 'byte
        retval = frmPar.FirstDispByte + Direction * Distance
      Case 2: 'line
        If Direction = 1 Then 'line forward
          line = 1
          While line <= Distance And Not EOF(frmPar.FileHandle)
            ReadLine cbuff, nospacebuff, EOPfound, bytesPerWord, maxBytesPerLine, _
              EOLstring, LenEOL, CheckEOL, EOPstring, LenEOP, CheckEOP
            line = line + 1
          Wend
          retval = Seek(frmPar.FileHandle)
        Else 'line back toward file start
                  
        End If
      Case 3, 4: 'page
        If Direction = 1 And frmPar.NextDispByte < frmPar.BytesInFile Then 'page forward
          retval = frmPar.NextDispByte
          Seek frmPar.FileHandle, retval
          Distance = Distance - 1
          While Distance > 0 And Not EOF(frmPar.FileHandle)
            For line = 1 To linesperpage
              ReadLine cbuff, nospacebuff, EOPfound, bytesPerWord, maxBytesPerLine, _
                EOLstring, LenEOL, CheckEOL, EOPstring, LenEOP, CheckEOP
              If EOPfound Then GoTo NextPage
            Next line
NextPage:
            Distance = Distance - 1
            retval = Seek(frmPar.FileHandle)
          Wend
        ElseIf Direction = -1 And frmPar.FirstDispByte > 1 Then
          'page back toward file start
        End If
    End Select
  End If

  'Make sure return value is sane
  If retval > frmPar.BytesInFile Then retval = frmPar.BytesInFile
  If retval < 1 Then retval = 1

  clsFileView_ScrollPos = retval

End Function

Private Sub SetPageParams(ByRef bytesPerWord, ByRef maxBytesPerLine&, ByRef linesperpage&, _
  ByRef LineNumbering As Boolean, _
  ByRef EOLstring$, ByRef LenEOL&, ByRef CheckEOL As Boolean, _
  ByRef EOPstring$, ByRef LenEOP&, ByRef CheckEOP As Boolean)
  
  Dim opt As clsOption, sampleWord$, maxWordsPerLine&
  
  frmPar.ParseDelimiterOption "Line Break", EOLstring, LenEOL, CheckEOL
  frmPar.ParseDelimiterOption "Page Break", EOPstring, LenEOP, CheckEOP
  
  LineNumbering = False
  Set opt = frmPar.Options("Line Numbers")
  If opt.Valid Then
    If VarType(opt.Value) = vbBoolean Then LineNumbering = opt.Value
  End If
  
  linesperpage = (frmPar.txtMain.Height - 600) / (1# * frmPar.TextHeight("W"))
  Set opt = frmPar.Options("Page Length")
  If opt.Valid Then
    If IsNumeric(opt.Value) Then linesperpage = opt.Value
  End If
  If linesperpage < 1 Then linesperpage = 1

  bytesPerWord = 2
  Set opt = frmPar.Options("Word Width")
  If opt.Valid Then
    If IsNumeric(opt.Value) Then bytesPerWord = opt.Value
  End If
  sampleWord = String(bytesPerWord * 2, "0") & " "
  maxWordsPerLine = (frmPar.txtMain.Width - 360) / (frmPar.TextWidth(sampleWord))
  If LineNumbering Then maxWordsPerLine = maxWordsPerLine - 1
  maxBytesPerLine = bytesPerWord * maxWordsPerLine
  
End Sub

Private Sub ReadLine(ByRef linebuff$, ByRef nospacebuff$, ByRef EOPfound As Boolean, _
  bytesPerWord&, maxBytesPerLine&, _
  EOLstring$, LenEOL&, CheckEOL As Boolean, _
  EOPstring$, LenEOP&, CheckEOP As Boolean)
  
  Dim byt As Byte, digit&
  Dim pos&
  pos = 0
  linebuff = ""
  While pos < maxBytesPerLine And Not EOF(frmPar.FileHandle) And Not EOPfound
    For digit = 1 To bytesPerWord
      Get frmPar.FileHandle, , byt
      linebuff = linebuff & hexchar(byt)
      nospacebuff = nospacebuff & hexchar(byt)
      pos = pos + 1
      If CheckEOL Then
        If Right(nospacebuff, LenEOL) = EOLstring Then GoTo EndSub
      End If
      If CheckEOP Then
        If Right(nospacebuff, LenEOP) = EOPstring Then
          EOPfound = True
          GoTo EndSub
        End If
      End If
    Next digit
    linebuff = linebuff & " "
  Wend
EndSub: 'branch here if EOP or EOL found
End Sub

Public Sub clsFileView_DispPage(ByVal linePrefix$)
  Dim nospacebuff$, tbuff$, linebuff$, ByteNum$
  Dim EOLstring$, LenEOL&, CheckEOL As Boolean
  Dim EOPstring$, LenEOP&, CheckEOP As Boolean, EOPfound As Boolean
  Dim linesperpage&, linecnt&, maxBytesPerLine&, bytesPerWord&
  Dim LineNumbering As Boolean
  
  SetPageParams bytesPerWord, maxBytesPerLine, linesperpage, LineNumbering, EOLstring, LenEOL, CheckEOL, EOPstring, LenEOP, CheckEOP
  
  If frmPar.FirstDispByte > frmPar.BytesInFile Then frmPar.FirstDispByte = frmPar.BytesInFile
  If frmPar.FirstDispByte < 1 Then frmPar.FirstDispByte = 1
  Seek frmPar.FileHandle, frmPar.FirstDispByte
  tbuff = ""
  nospacebuff = ""
  EOPfound = False
  linecnt = 0
  While Not EOF(frmPar.FileHandle) And linecnt <= linesperpage And Not EOPfound
    tbuff = tbuff & linePrefix
    If LineNumbering Then
      ByteNum = CStr(Seek(frmPar.FileHandle))
      If Len(ByteNum) < 4 Then ByteNum = ByteNum & String(4 - Len(ByteNum), " ")
      tbuff = tbuff & ByteNum & ": "
    End If
    ReadLine linebuff, nospacebuff, EOPfound, bytesPerWord, maxBytesPerLine, EOLstring, LenEOL, CheckEOL, EOPstring, LenEOP, CheckEOP
    tbuff = tbuff & linebuff & vbCrLf
    If EOPfound Then GoTo endofPage
    linecnt = linecnt + 1
  Wend
endofPage:
  frmPar.txtMain.Text = Left(tbuff, Len(tbuff) - 2) 'strip final vbCrLf
  frmPar.SetNextDispByte Seek(frmPar.FileHandle)

End Sub

Public Function clsFileView_CanOpen(Filename As String) As Boolean
  clsFileView_CanOpen = True
End Function

Public Sub clsFileView_Find(SearchFor As String, Direction As Long)

End Sub

