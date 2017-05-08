VERSION 5.00
Begin VB.Form frmDispFile 
   Caption         =   "Display File"
   ClientHeight    =   6885
   ClientLeft      =   990
   ClientTop       =   1665
   ClientWidth     =   11025
   BeginProperty Font 
      Name            =   "Courier New"
      Size            =   7.5
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   109
   Icon            =   "DispFile.frx":0000
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6885
   ScaleWidth      =   11025
   Begin VB.Frame fraOpt 
      Height          =   672
      Left            =   60
      TabIndex        =   12
      Top             =   6180
      Width           =   10935
      Begin VB.TextBox txtFind 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   8400
         TabIndex        =   15
         Top             =   240
         Width           =   2295
      End
      Begin VB.CommandButton cmdFind 
         Caption         =   "&Find"
         Default         =   -1  'True
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   7440
         TabIndex        =   14
         Top             =   240
         Width           =   735
      End
      Begin VB.CommandButton cmdSave 
         Caption         =   "&Save"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   6360
         TabIndex        =   9
         Top             =   240
         Width           =   855
      End
      Begin VB.ComboBox comboFormat 
         Height          =   315
         Left            =   2880
         Style           =   2  'Dropdown List
         TabIndex        =   6
         Top             =   240
         Width           =   1455
      End
      Begin ATCoCtl.ATCoText ATEOL 
         Height          =   255
         Left            =   6840
         TabIndex        =   11
         ToolTipText     =   "End Of Line"
         Top             =   240
         Width           =   375
         _ExtentX        =   1296
         _ExtentY        =   450
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   5
         Alignment       =   1
         DataType        =   0
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText ATbytesPerWord 
         Height          =   255
         Left            =   6480
         TabIndex        =   10
         ToolTipText     =   "Bytes Per Word"
         Top             =   240
         Width           =   375
         _ExtentX        =   1085
         _ExtentY        =   450
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   1
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   5
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   "2"
         Value           =   "2"
         Enabled         =   -1  'True
      End
      Begin VB.CommandButton cmdClose 
         Caption         =   "&Close"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   4440
         TabIndex        =   7
         Top             =   240
         Width           =   855
      End
      Begin VB.CommandButton cmdPrint 
         Caption         =   "&Print"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   5400
         TabIndex        =   8
         Top             =   240
         Width           =   855
      End
      Begin VB.CommandButton cmdPage 
         Caption         =   ">>"
         Height          =   348
         Index           =   3
         Left            =   1740
         TabIndex        =   5
         ToolTipText     =   "Last Page"
         Top             =   180
         Width           =   348
      End
      Begin VB.CommandButton cmdPage 
         Caption         =   "<<"
         Height          =   348
         Index           =   0
         Left            =   60
         TabIndex        =   1
         ToolTipText     =   "First Page"
         Top             =   180
         Width           =   348
      End
      Begin VB.CommandButton cmdPage 
         Caption         =   ">"
         Height          =   348
         Index           =   2
         Left            =   1380
         TabIndex        =   4
         ToolTipText     =   "Next Page"
         Top             =   180
         Width           =   348
      End
      Begin VB.CommandButton cmdPage 
         Caption         =   "<"
         Height          =   348
         Index           =   1
         Left            =   420
         TabIndex        =   2
         ToolTipText     =   "Previous Page"
         Top             =   180
         Width           =   348
      End
      Begin ATCoCtl.ATCoText txtPage 
         Height          =   255
         Left            =   840
         TabIndex        =   3
         Top             =   240
         Width           =   495
         _ExtentX        =   873
         _ExtentY        =   450
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   1
         HardMin         =   -999
         SoftMax         =   1
         SoftMin         =   -999
         MaxWidth        =   5
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   "1"
         Value           =   "1"
         Enabled         =   -1  'True
      End
      Begin MSComDlg.CommonDialog CommonDialog1 
         Left            =   2280
         Top             =   120
         _ExtentX        =   847
         _ExtentY        =   847
         _Version        =   393216
         CancelError     =   -1  'True
      End
      Begin VB.Label lblMaxPage 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Left            =   2160
         TabIndex        =   13
         Top             =   300
         Width           =   675
      End
   End
   Begin VB.TextBox txtBox 
      BackColor       =   &H00FFFFFF&
      Height          =   6132
      HideSelection   =   0   'False
      Left            =   60
      MultiLine       =   -1  'True
      ScrollBars      =   1  'Horizontal
      TabIndex        =   0
      Text            =   "DispFile.frx":0442
      Top             =   60
      Width           =   10935
   End
End
Attribute VB_Name = "frmDispFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private pFindString As String
Private pNeedCalcFilePos As Boolean
Private pFileOpen As Long
Private outFNum%, outFName$
Private linePerPage&
Private bytePerLine&, bytesInFile&, hexchar$(256) 'for binary mode
Private nShapes&, ShapeType&, lowX#, lowY#, uppX#, uppY# 'for shape files
Private pagePos(5000) As Long, pageCnt&
Private init As Boolean, Editable As Boolean
Enum DispModeType
  DispText = 0
  DispFortCC = 1
  DispFortUnSeq = 2
  DispBinary = 3
  DispShp = 4
  DispSWMM = 5
End Enum
Private DisplayMode As DispModeType

Public Property Get FindString() As String
  FindString = pFindString
End Property
Public Property Let FindString(newvalue As String)
  pFindString = newvalue
End Property

Public Sub DispFile(outfile$, cap$, ic As Object, allowEdits As Boolean, DispMode&)
  Dim txtbuff$, cbuff$, j&, w&, linecnt&, f$, pos&
  
  init = True
  pNeedCalcFilePos = True
  Me.MousePointer = vbHourglass
  outFName = outfile
    
  j = InStr(outfile, FilenameOnly(outfile)) - 1
  f = Right(outfile, Len(outfile) - j)

  If InStr(cap, " of ") = 0 Then Caption = cap & " of " & f

  DisplayMode = DispMode
  
  Editable = allowEdits
 
  icon = ic
  Show
  
  If pNeedCalcFilePos Then CalcFilePos
  
  If Editable And DisplayMode <> DispFortCC Then
    txtBox.BackColor = vbWindowBackground
    txtBox.ForeColor = vbWindowText
    txtBox.Locked = False
  Else
    txtBox.BackColor = vbButtonFace
    txtBox.ForeColor = vbButtonText
    txtBox.Locked = True
  End If
  cmdSave.Visible = Editable
  
  init = False
  cmdSave.Enabled = False
  Me.MousePointer = vbDefault
  
End Sub

Private Sub OpenFile()
  If pFileOpen = 0 Then
    outFNum = FreeFile(0)
    On Error GoTo openerror:
    
    Select Case DisplayMode
      Case DispText, DispFortCC
        Open outFName For Input As #outFNum
        ATbytesPerWord.Visible = False
        ATEOL.Visible = False
      Case DispFortUnSeq, DispBinary, DispShp
        Editable = False 'Can't edit in binary format yet
        Open outFName For Binary As #outFNum
        bytesInFile = LOF(outFNum)
        If DisplayMode = DispBinary Then
          ATbytesPerWord.Visible = True
          ATEOL.Visible = True
        ElseIf DisplayMode = DispShp Then
          Dim dummy&
          ReadShapeHeader outFNum, dummy, ShapeType, lowX, lowY, uppX, uppY
        End If
    End Select
    comboFormat.ListIndex = DisplayMode
  End If
  pFileOpen = pFileOpen + 1
  Exit Sub
openerror:
  Me.MousePointer = vbDefault
  MsgBox "Could not open file :" & outFName & " (#" & Err.Number & ") " & Err.Description & vbCrLf & outFName, vbOKOnly, "DispFile Error"
  pFileOpen = False
End Sub

Private Sub CloseFile()
  pFileOpen = pFileOpen - 1
  If pFileOpen < 0 Then
    MsgBox "Too Many Closes!"
  ElseIf pFileOpen = 0 Then
    Close outFNum
  End If
End Sub

Private Sub CalcFilePos()
  Dim i&, linePos&, linecnt&, cbuff$, w&
  
  OpenFile
  
  linePerPage = (txtBox.Height - 360) / (1# * TextHeight("ABCDEFGH"))
  If linePerPage < 1 Then linePerPage = 1
  i = 0
  Do While pagePos(i) > 0
    pagePos(i) = 0
    i = i + 1
  Loop
  i = 0
  pagePos(0) = 1
  pageCnt = 1
  linecnt = 0
  
  If DisplayMode = DispFortUnSeq Or DisplayMode = DispBinary Or DisplayMode = DispShp Then
    Dim bytePerPage&
    If bytesInFile < 1 Then Exit Sub
    bytePerLine = (txtBox.Width - 360) / (2# * TextWidth("X")) * 4 / 5
'    If bytePerLine > 64 Then bytePerLine = 64
'    If bytePerLine > 32 And bytePerLine < 64 Then bytePerLine = 32
'    If bytePerLine < 32 And bytePerLine > 16 Then bytePerLine = 16
'    If bytePerLine > 8 And bytePerLine < 16 Then bytePerLine = 8
    bytePerPage = linePerPage * bytePerLine
  End If
  
  Select Case DisplayMode
    Case DispBinary, DispFortUnSeq
      pageCnt = bytesInFile / bytePerPage
      If bytesInFile Mod bytePerPage <> 0 Then pageCnt = pageCnt + 1
      For i = 0 To pageCnt - 1
        pagePos(i) = i * bytePerPage + 1
      Next i
      pagePos(pageCnt) = bytesInFile
    Case DispShp
      If ShapeType = 1 Then
        nShapes = (bytesInFile - 100) / 28
        pageCnt = (nShapes + 5) / linePerPage
        If pageCnt < 1 Then pageCnt = 1
        For i = 1 To pageCnt - 1
          pagePos(i) = 101 + i * linePerPage * 28
        Next i
      Else
        Dim RecNumber&, RecLength&, ShpTyp&
        Seek outFNum, 101
        While Seek(outFNum) < bytesInFile
          i = i + 1
          pagePos(i) = Seek(outFNum)
          ReadShapeRecordHeader outFNum, RecNumber, RecLength, ShpTyp
          'Debug.Print RecNumber, pagePos(i), RecLength, ShpTyp
          Seek outFNum, Seek(outFNum) + RecLength * 2 - 4
        Wend
        nShapes = i
        pageCnt = i + 1
      End If
    Case DispText, DispFortCC
          
      Seek outFNum, 1
      While Not EOF(outFNum) And pageCnt < UBound(pagePos) ' Loop until end of file.
        linePos = Seek(outFNum)
        Line Input #outFNum, cbuff
        linecnt = linecnt + 1
        
        If init Then
          'adjust form to longest line
          On Error Resume Next
          w = TextWidth(Left(cbuff, Len(cbuff))) + 480
          On Error GoTo 0
          If w > Width Then
            If w < Screen.Width Then
              Width = w
            End If
          End If
        End If
        
        If (DisplayMode = DispFortCC And Left(cbuff, 1) = "1") Or linecnt = linePerPage Then
          If linePos > pagePos(pageCnt - 1) Then
            pagePos(pageCnt) = linePos
            pageCnt = pageCnt + 1
          End If
          If init And linecnt = linePerPage And pageCnt = 2 Then
            'get something displayed
            DispPage False, 1
            DoEvents
          End If
          linecnt = 0
        End If
      Wend
      pagePos(pageCnt) = Seek(outFNum)
  End Select
  txtPage.HardMax = pageCnt
  txtPage.SoftMax = pageCnt
  lblMaxPage.Caption = "of " & pageCnt
  txtPage.Value = 1
  DispPage False, 1
  pNeedCalcFilePos = False
  
  CloseFile
  
End Sub

Private Sub DispShpPage(p, tbuff$, blk$)
  Dim pos&, byt As Byte, endln&, digit&, numpts&, numparts&, parts&()
  Dim lowXs#, lowYs#, uppXs#, uppYs#
  Dim tbufflines&, RecNum&, ContentLen&, ThisShpTyp&, X#, Y#, xa#(), ya#()
  
  tbufflines = 0
  If p = 1 Then
    tbuff = blk & "Shape Type: " & ShapeType & " ("
    Select Case ShapeType
      Case 0: tbuff = tbuff & "null"
      Case 1: tbuff = tbuff & "Point"
      Case 3: tbuff = tbuff & "Arc"
      Case 5: tbuff = tbuff & "Polygon"
      Case 8: tbuff = tbuff & "Multipoint"
      Case Else: tbuff = tbuff & "unknown"
    End Select
    tbuff = tbuff & ")" & vbCrLf
    tbuff = tbuff & blk & "Bounding box: low  (" & lowX & ", " & lowY & ")" & vbCrLf
    tbuff = tbuff & blk & "Bounding box: high (" & uppX & ", " & uppY & ")" & vbCrLf
    If nShapes > 0 Then tbuff = tbuff & blk & "Number of shapes: " & nShapes & vbCrLf
    tbufflines = 4
  End If
  If p > 1 Or ShapeType = 1 Then
    While tbufflines < linePerPage And Seek(outFNum) < bytesInFile
      ReadShapeRecordHeader outFNum, RecNum, ContentLen, ThisShpTyp
      If ThisShpTyp <> ShapeType Then tbuff = tbuff & "# " & RecNum & " Warning: shape type (" & ThisShpTyp & ") not same as rest of file. " & vbCrLf
      tbuff = tbuff & blk & "# " & RecNum & ", Len: " & ContentLen
      Select Case ThisShpTyp
      Case 1
        ReadShapePoint outFNum, X, Y
        tbuff = tbuff & " Point (" & X & ", " & Y & ")"
      Case 3, 5
        ReadShapeArc outFNum, lowXs, lowYs, uppXs, uppYs, numparts, numpts, parts, xa, ya
        tbuff = tbuff & " Arc with " & numparts & " part"
        If numparts > 1 Then tbuff = tbuff & "s"
        tbuff = tbuff & ", " & numpts & " points." & vbCrLf
        tbuff = tbuff & blk & "Bounding box: low  (" & lowXs & ", " & lowYs & ")" & vbCrLf
        tbuff = tbuff & blk & "Bounding box: high (" & uppXs & ", " & uppYs & ")" & vbCrLf
        tbuff = tbuff & blk & "Part indexes: "
        For pos = 0 To numparts - 1
          tbuff = tbuff & parts(pos) & ", "
        Next pos
        tbuff = tbuff & vbCrLf
        tbufflines = tbufflines + 4
        pos = 0
        While pos < numpts 'and tbufflines < linePerPage
          tbuff = tbuff & pos + 1 & ": (" & xa(pos) & ", " & ya(pos) & ")" & vbCrLf
          pos = pos + 1
          tbufflines = tbufflines + 1
        Wend
        tbufflines = linePerPage 'Want just one complex shape per page
      Case Else
        pos = 0
        While pos < ContentLen * 2 - 4 And tbufflines < linePerPage And Seek(outFNum) < bytesInFile
          tbuff = tbuff & vbCrLf & "   "
          tbufflines = tbufflines + 1
          endln = pos + bytePerLine
          While pos < endln
            For digit = 1 To ATbytesPerWord.Value
              Get outFNum, , byt
              pos = pos + 1
              tbuff = tbuff & hexchar(byt)
              If pos = ContentLen * 2 - 3 Then GoTo nextshape
            Next digit
            tbuff = tbuff & " "
          Wend
        Wend
        'Seek outFNum, Seek(outFNum) + ContentLen * 2 - 4
      End Select
nextshape:
      tbuff = tbuff & vbCrLf
      tbufflines = tbufflines + 1
    Wend
  End If
End Sub

Private Sub DispFortUnSeqPage(p, tbuff$, blk$)
  Dim pos&, byt As Byte, endln&, digit&
  Dim tbufflines&, RecNum&, ContentLen&
  Dim prefix$, tmp$
  Dim first As Boolean
  first = True
  prefix = blk
  RecNum = p - 1
  tbufflines = 0
  Seek #outFNum, p
  If RecNum = 0 Then
    Get #outFNum, , byt
    If byt <> &HFD Then
      tbuff = "File: '" & outFName & "' is not a Fortran Unformatted Sequential File" & vbCrLf & "(does not begin with hex FD)"
      Exit Sub
    End If
  End If
  While tbufflines < linePerPage And Seek(outFNum) < bytesInFile
    ContentLen = FtnUnfSeqRecLen(outFNum, first)
    tmp = ContentLen & ": "
    If ContentLen > 0 Then
      tmp = tmp & String(6 - Len(tmp), " ")
    End If
    tbuff = tbuff & blk & tmp
    prefix = blk & String(Len(tmp), " ")
    pos = 1
    While pos < ContentLen And tbufflines < linePerPage
      If pos > 1 Then tbuff = tbuff & prefix
      endln = pos + bytePerLine
      If endln > ContentLen Then endln = ContentLen
      While pos < endln
        For digit = 1 To ATbytesPerWord.Value
          Get outFNum, , byt
          tbuff = tbuff & hexchar(byt)
          pos = pos + 1
          If Len(ATEOL.Value) > 0 Then
            If Right(tbuff, Len(ATEOL.Value)) = ATEOL.Value Then GoTo endofline
          End If
          If pos > endln Then digit = ATbytesPerWord.Value
        Next digit
        tbuff = tbuff & " "
      Wend
endofline:
      tbuff = tbuff & vbCrLf
      tbufflines = tbufflines + 1
    Wend
  Wend
End Sub

Private Function FtnUnfSeqRecLen(f%, first As Boolean) As Long
  Dim b As Byte, RecLen As Long, bytes As Integer, c As Long
  Static LastLen As Long
  
  If first Then
    LastLen = 0
    first = False
  Else
    c = 64
    Get #f, , b
    While LastLen > c
      c = c * 256
      Get #f, , b
    Wend
  End If
  Get #f, , b
  bytes = b And 3
  RecLen = b / 4
  c = 64
  Do While bytes > 0
    Get #f, , b
    bytes = bytes - 1
    RecLen = RecLen + b * c
    c = c * 256
  Loop
  LastLen = RecLen
  FtnUnfSeqRecLen = RecLen
End Function

Private Sub DispPage(Prt As Boolean, p&)
  Dim cbuff$, tbuff$, blk$
  Dim pos&, byt As Byte, endln&, digit&
  
  OpenFile
  
  If Prt Then ' left margin
    blk = "           "
  Else
    blk = ""
  End If
  
  tbuff = ""
  Seek outFNum, pagePos(p - 1)
  
  Select Case DisplayMode
    Case DispText:
      Do
        Line Input #outFNum, cbuff
        tbuff = tbuff & blk & cbuff & vbCrLf
        If Seek(outFNum) >= pagePos(p) Then Exit Do
      Loop
    Case DispFortCC:
      Do
        Line Input #outFNum, cbuff
        If cbuff <> "1" Then tbuff = tbuff & blk & cbuff & vbCrLf
        If Seek(outFNum) >= pagePos(p) Then Exit Do
      Loop
    Case DispShp:       DispShpPage p, tbuff, blk
    Case DispFortUnSeq: DispFortUnSeqPage p, tbuff, blk
    Case DispBinary:
      pos = pagePos(p - 1)
      While pos < pagePos(p)
        tbuff = tbuff & blk
        endln = pos + bytePerLine
        If endln > pagePos(p) Then endln = pagePos(p)
        While pos < endln
          For digit = 1 To ATbytesPerWord.Value
            Get outFNum, , byt
            tbuff = tbuff & hexchar(byt)
            pos = pos + 1
            If Len(ATEOL.Value) > 0 Then
              If Right(tbuff, Len(ATEOL.Value)) = ATEOL.Value Then GoTo endofline
            End If
          Next digit
          tbuff = tbuff & " "
        Wend
endofline:
        tbuff = tbuff & vbCrLf
      Wend
  End Select
  
  If Len(tbuff) > 0 Then
     If Prt Then
       Printer.Print tbuff
     Else
       txtBox.Text = tbuff
     End If
  ElseIf p < pageCnt Then
     p = p + 1
     DispPage Prt, p
     If Not (Prt) Then txtPage.Value = p
  End If
  cmdSave.Enabled = False
  
  CloseFile

End Sub

Private Sub PagePrint()
  Dim i&, j&, fp&, tp&
  
  On Error GoTo 10
  
  fp = 1
  tp = pageCnt
  Call ShowPrinterX(Me, fp, tp, 1, pageCnt, PD_NOSELECTION + PD_DISABLEPRINTTOFILE)
  Set Printer.Font = Font
  Printer.FontBold = False
  
  For i = fp To tp
    For j = 1 To 6
      Printer.Print
    Next j
    DispPage True, i
    If i < tp Then
      Printer.NewPage
    Else
      Printer.EndDoc
    End If
  Next i
  
10 'continue

End Sub

'Private Sub SetFortCC(c As Boolean)
'  Dim oldpos&, newpage&
'  oldpos = pagePos(txtPage.Value - 1)
'
'  FortCC = c
'  CalcFilePos
'
'  newpage = 0
'  If oldpos > 0 Then
'    While pagePos(newpage) < oldpos
'      newpage = newpage + 1
'    Wend
'    If pagePos(newpage) > oldpos Then newpage = newpage - 1
'  End If
'  txtPage.Value = newpage + 1
'
'  If Editable Then
'    If FortCC Then
'      txtBox.BackColor = &HC0C0C0
'      txtBox.Locked = True
'      cmdSave.Enabled = False
'    Else
'      txtBox.BackColor = vbWhite
'      txtBox.Locked = False
'    End If
'  End If
'End Sub

Private Sub cmdClose_Click()
  Unload Me
End Sub
Public Sub cmdFind_Click()
  Dim i&
  
  i = 0
  While i = 0
    i = InStr(txtBox.SelStart + 2, LCase(txtBox.Text), LCase(txtFind.Text))
    If i = 0 Then
      If txtPage.Value < txtPage.HardMax Then
        txtPage.Value = txtPage.Value + 1
      Else
        i = MsgBox("End of file reached.  Restart Find at beginning of file?", vbYesNo, "Find in " & Me.Caption)
        If i = vbYes Then
          txtPage.Value = 1
          i = 0 'start looking again
        End If
      End If
    Else
      txtBox.SelStart = i - 1
      txtBox.SelLength = Len(txtFind.Text)
    End If
  Wend
End Sub

Private Sub cmdPrint_Click()
  PagePrint
End Sub

Private Sub cmdSave_Click()
  cmdSave.Enabled = Not SaveChanges
End Sub

'Returns true if save was successful, false if there was an error or save was cancelled
Private Function SaveChanges() As Boolean
  SaveChanges = False
  
  Dim txtPageValue
  txtPageValue = txtPage.Value
  
  With CommonDialog1
    .Filename = outFName
    .flags = cdlOFNHideReadOnly + cdlOFNOverwritePrompt + cdlOFNNoReadOnlyReturn
    On Error GoTo cancelsave
    .ShowSave
    If Len(.Filename) > 0 Then
      Dim tmpNewFile&, thisPageStart&, buf$
      tmpNewFile = FreeFile(0)
      thisPageStart = pagePos(txtPage.Value - 1)
      Open .Filename & "TMP" For Output As tmpNewFile
      Seek outFNum, 1
      While Seek(outFNum) < thisPageStart
        Line Input #outFNum, buf 'buf = Input(100, outFNum)
        Print #tmpNewFile, buf
      Wend
      Print #tmpNewFile, txtBox.Text
      If txtPage.Value < pageCnt Then
        Seek outFNum, pagePos(txtPage.Value)
        While Not EOF(outFNum)
          Line Input #outFNum, buf 'buf = Input(100, outFNum)
          Print #tmpNewFile, buf
        Wend
      End If
      Close tmpNewFile
      On Error Resume Next
      Kill .Filename
      On Error GoTo 0
      FileCopy .Filename & "TMP", .Filename
      Kill .Filename & "TMP"
      DispFile .Filename, Me.Caption, Me.icon, Editable, DisplayMode
      txtPage.Value = txtPageValue
    End If
  End With
  
  SaveChanges = True
  
cancelsave:

End Function

Private Sub comboFormat_Change()
  If DisplayMode <> comboFormat.ItemData(comboFormat.ListIndex) Then
    DisplayMode = comboFormat.ItemData(comboFormat.ListIndex)
    DispFile outFName, Me.Caption, Me.icon, Editable, DisplayMode
  End If
End Sub

Private Sub comboFormat_Click()
  comboFormat_Change
End Sub

Private Sub Form_Load()
  Dim i&

  comboFormat.Clear
  comboFormat.AddItem "Plain Text":     comboFormat.ItemData(comboFormat.NewIndex) = DispText:           If DispText = DisplayMode Then comboFormat.ListIndex = comboFormat.NewIndex
  comboFormat.AddItem "FORTRAN Output": comboFormat.ItemData(comboFormat.NewIndex) = DispFortCC:       If DispFortCC = DisplayMode Then comboFormat.ListIndex = comboFormat.NewIndex
  comboFormat.AddItem "FORTRAN UnSeq":  comboFormat.ItemData(comboFormat.NewIndex) = DispFortUnSeq: If DispFortUnSeq = DisplayMode Then comboFormat.ListIndex = comboFormat.NewIndex
  comboFormat.AddItem "Binary":         comboFormat.ItemData(comboFormat.NewIndex) = DispBinary:       If DispBinary = DisplayMode Then comboFormat.ListIndex = comboFormat.NewIndex
  'should this next item be removed?
  comboFormat.AddItem "ESRI Shape":     comboFormat.ItemData(comboFormat.NewIndex) = DispShp:             If DispShp = DisplayMode Then comboFormat.ListIndex = comboFormat.NewIndex
  comboFormat.ListIndex = 0
  
  Dim ch$(16)
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

Private Sub Form_Resize()
  Dim i&
  Static lastHeight As Long
    
  If Height > 1500 Then
    If Height <> lastHeight Then
      txtBox.Height = Height - fraOpt.Height - 480
      CalcFilePos
      lastHeight = Height
    End If
    txtBox.Width = Width - 180
    fraOpt.Top = txtBox.Top + txtBox.Height
    fraOpt.Width = Width - 180
  End If
End Sub

Private Sub cmdPage_Click(index As Integer)
  If cmdSave.Enabled Then
    Select Case MsgBox("Save changes?", vbYesNoCancel, "File Edited")
      Case vbYes:     If Not SaveChanges Then Exit Sub 'Treat save error as cancel
      Case vbNo:      'just change the page as directed without saving
      Case vbCancel:  Exit Sub 'Cancel means don't save or change page
    End Select
  End If
  
  If index = 0 Then           'Go to first page
    txtPage.Value = 1
  ElseIf index = 1 Then
    If txtPage.Value > 1 Then 'Go to previous page
      txtPage.Value = txtPage.Value - 1
    Else
      Beep
    End If
  ElseIf index = 2 Then       'Go to next page
    If txtPage.Value < txtPage.HardMax Then
      txtPage.Value = txtPage.Value + 1
    Else
      Beep
    End If
  ElseIf index = 3 Then       'Go to end page
    txtPage.Value = txtPage.HardMax
  End If
End Sub

Private Sub txtBox_Change()
  cmdSave.Enabled = True
End Sub

Private Sub txtPage_Change()
  If txtPage.Value >= txtPage.HardMin And txtPage.Value <= txtPage.HardMax Then
    DispPage False, txtPage.Value
  End If
End Sub
