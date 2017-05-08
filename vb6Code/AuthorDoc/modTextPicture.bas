Attribute VB_Name = "modTextPicture"
Option Explicit

Public Const Asterisks80 = "********************************************************************************"
Private Const SixSplats = "******"
Private Const SevenSplats = "*******"
Private Const TensPlace = "         1         2         3         4         5         6         7         8"
Private Const OnesPlace = "12345678901234567890123456789012345678901234567890123456789012345678901234567890"
Private Const MaxRowLength = 80
Private WholeCardHeader As String
Private lenWholeCardHeader As Long

Public Sub FormatCardGraphic()
  Dim startpos As Long, endpos As Long, TableText As String, ImageFilename As String
  If WholeCardHeader = "" Then
    WholeCardHeader = Asterisks80 & TensPlace & OnesPlace
    lenWholeCardHeader = Len(WholeCardHeader)
  End If
  startpos = InStr(TargetText, WholeCardHeader)
  If startpos > 0 Then
    endpos = InStrRev(TargetText, Asterisks80)
    If endpos > 0 Then
      ImageFilename = FileNameOnly(SaveFilename) & ".bmp"
      TableText = Mid(TargetText, startpos + lenWholeCardHeader, endpos - startpos - lenWholeCardHeader)
      TargetText = Left(TargetText, startpos - 1) & "<p>" & vbCrLf & "<img src=""" & ImageFilename & """>" & vbCrLf & "<p>" & Mid(TargetText, endpos + 81)
      CardImage TableText
      SavePicture frmSample.img, PathNameOnly(SaveDirectory & SaveFilename) & ImageFilename
    End If
  End If

End Sub

Public Sub CardImage(TableText As String)
  Dim TextRow(255) As String
  Dim RowY(255) As Long
  Dim TensY As Long
  Dim OnesY As Long
  Dim Row As Long, Rows As Long
  Dim col As Long, lentxt As Long
  Dim lastCR As Long, thisCR As Long
  Dim GrayColor As OLE_COLOR
  Dim curChar As String
  Dim CharWidth As Long
  Dim XMargin As Long
  Dim CharHeight As Long
  Dim txt As String
  Dim GrayLevel As Long
  Dim RangeExists As Boolean
  Dim RowStopChecking As Long
  
  GrayLevel = 170
  txt = ReplaceString(TableText, "&gt;", ">")
  txt = ReplaceString(txt, "&lt;", "<")
  GrayColor = RGB(GrayLevel, GrayLevel, GrayLevel)
  
  lentxt = Len(txt)
  thisCR = 0
  Rows = 0
  RowStopChecking = 256
  GoSub FindCR
  While lastCR < lentxt
    Rows = Rows + 1
    TextRow(Rows) = Mid(txt, lastCR + 1, thisCR - lastCR - 1)
    Select Case TextRow(Rows)
      Case SixSplats, SevenSplats, Asterisks80, TensPlace, OnesPlace 'Skip some rows
        Rows = Rows - 1
      Case "Example"
        RowStopChecking = Rows
      Case Else                                                      'Split long rows
        While Len(TextRow(Rows)) > MaxRowLength
          Rows = Rows + 1
          TextRow(Rows) = ""
          col = 1
          While Mid(TextRow(Rows - 1), col, 1) = " "
            TextRow(Rows) = TextRow(Rows) & " "
            col = col + 1
          Wend
          TextRow(Rows) = TextRow(Rows) & Mid(TextRow(Rows - 1), MaxRowLength + 1)
          TextRow(Rows - 1) = Left(TextRow(Rows - 1), MaxRowLength)
        Wend
    End Select
    GoSub FindCR
  Wend
  
  frmSample.Visible = True
  With frmSample.img
    CharWidth = .TextWidth("X")
    XMargin = CharWidth / 2
    CharHeight = .TextHeight("X")
    .Width = CharWidth * MaxRowLength + XMargin * 2
    .Height = CharHeight * 10 'Start with enough height for header, adjust again after header
    .Cls
    'frmSample.img.Line (0, 0)-(.Width, 0), vbBlack
    
    'Print tens places in gray
    .ForeColor = GrayColor
    .CurrentY = CharHeight / 2
    TensY = .CurrentY
    For col = 1 To 8
      curChar = col
      .CurrentX = XMargin + (col * 10 - 1) * CharWidth + (CharWidth - .TextWidth(curChar)) / 2
      frmSample.img.Print curChar;
    Next
    frmSample.img.Print
    'Print Ones Place in gray
    OnesY = .CurrentY
    For col = 1 To 80
      curChar = col Mod 10
      .CurrentX = XMargin + (col - 1) * CharWidth + (CharWidth - .TextWidth(curChar)) / 2
      frmSample.img.Print curChar;
    Next
    frmSample.img.Print
    RowY(0) = .CurrentY
    .Height = RowY(0) + (OnesY - TensY) * Rows + TensY
    .ForeColor = vbBlack
    If InStr(txt, "<-range>") > 0 Then
      RangeExists = True
      col = 5
      frmSample.img.Line (XMargin + col * CharWidth, 0)-Step(0, .Height), GrayColor
      GoSub NumberCol
      col = 10
      frmSample.img.Line (XMargin + col * CharWidth, 0)-Step(0, .Height), GrayColor
      GoSub NumberCol
    Else
      RangeExists = False
    End If
    For Row = 1 To Rows
      RowY(Row) = RowY(Row - 1) + OnesY - TensY
      For col = 1 To Len(TextRow(Row))
        curChar = Mid(TextRow(Row), col, 1)
        .CurrentX = XMargin + (col - 1) * CharWidth + (CharWidth - .TextWidth(curChar)) / 2
        .CurrentY = RowY(Row)
        frmSample.img.Print curChar;
        If (Not RangeExists Or col > 10) And Row < RowStopChecking Then
          Select Case curChar
            Case "<"
              frmSample.img.Line (XMargin + (col - 1) * CharWidth, 0)-Step(0, .Height), GrayColor
              'GoSub NumberCol
            Case ">"
              frmSample.img.Line (XMargin + col * CharWidth, 0)-Step(0, .Height), GrayColor
              GoSub NumberCol
          End Select
        End If
      Next
      frmSample.img.Print
      'frmSample.img.Line (col * CharWidth, 0)-((col + 1) * CharWidth, .Height), RGB(222, 222, 222), BF
    Next
    Clipboard.SetData .Image
    frmSample.Move frmSample.Left, frmSample.Top, .Width + 108, .Height + 372
  Exit Sub
  
NumberCol:
    If col > 9 Then 'And curChar <> "<" Then
      curChar = Int(col / 10)
      .CurrentX = XMargin + (col - 1) * CharWidth + (CharWidth - .TextWidth(curChar)) / 2
      .CurrentY = TensY
      frmSample.img.Print curChar;
    End If
    curChar = col Mod 10
    .CurrentX = XMargin + (col - 1) * CharWidth + (CharWidth - .TextWidth(curChar)) / 2
    .CurrentY = OnesY
    frmSample.img.Print curChar;
    Return
  End With

FindCR:
  lastCR = thisCR
  If lastCR < lentxt Then
    If Mid(txt, lastCR + 1, 1) = vbLf Then lastCR = lastCR + 1
  End If
  
  thisCR = InStr(thisCR + 1, txt, vbCr)
  If thisCR = 0 Then thisCR = lentxt + 1
  Return
  
End Sub

'Public Sub PictureString(buf As String)
'  Dim col As Long, maxcol As Long, curChar As String
'  maxcol = Len(buf)
'  If maxcol > Cols Then maxcol = Cols
'  With frmSample.img
'    .CurrentY = Row * CharHeight
'    For col = 1 To maxcol
'      curChar = Mid(buf, col, 1)
'      .CurrentX = (col - 1) * CharWidth + (CharWidth - .TextWidth(curChar)) / 2
'      frmSample.img.Print curChar;
'    Next
'  End With
'  Row = Row + 1
'  If Len(buf) > CharWidth Then PictureString Mid(buf, CharWidth + 1)
'End Sub
