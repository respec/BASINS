Attribute VB_Name = "modConvert"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

'BaseName (~) is the name of program being documented.
'File ~.txt contains list of source files (ProjectFileName)
'~.hlp will be created if converting to help (also optionally ~.cnt, ~.hpj)
'~.doc will be created if converting to printable
'~.hhp, ~.hhc, ~.ID -> ~.chm

Public OutputFormat As outputType

Public Enum outputType
  tASCII = 0
  tHTML = 1
  tPRINT = 2
  tHELP = 3
  tHTMLHELP = 4
  NONE = -999
End Enum

Private word As WordBasic

Private Const maxLevels = 9 ' Do you really want sections nested deeper than this?

Private ProjectFile As Integer
Private SourceWin$, ContentsWin$, TargetWin$
Private SourceText$, TargetText$
Private SourceFilename$, IconFilename$
Private SourceBaseDirectory$, SaveDirectory$
Private HelpSourceRTFName As String
Private Directory$
Private ProjectFileEntry() As String, NextProjectFileEntry As Long, MaxProjectFileEntry As Long
Private HeadingWord$(8), HeadingText$(maxLevels), HeadingFile$(maxLevels), ContentsEntries%(maxLevels)
Private HeaderStyle$(maxLevels), FooterStyle$(maxLevels), BodyStyle$(maxLevels), WordStyle(maxLevels) As Collection
Private BodyTag As String
Private StyleFile$(maxLevels)
Private PromptForFiles As Boolean, FirstHeaderInFile As Boolean
Private NotFirstPrintFooter As Boolean, NotFirstPrintHeader As Boolean
Private TablePrintFormat As Long, TablePrintApply As Long

Private InsertParagraphsAroundImages As Boolean
Private BuildContents As Boolean
Private BuildProject As Boolean
Private FooterTimestamps As Boolean
Private UpNext As Boolean
Private BuildID As Boolean
Private IDfile As Integer
Private IDnum As Long
Private AliasSection As String
Private HTMLContentsfile%, HTMLHelpProjectfile%, HTMLIndexfile%
Private SaveFilename As String
Private InPre As Boolean
Private AlreadyInitialized As Boolean
Private LastHeadingLevel%, HeadingLevel%, BookLevel%, StyleLevel% ', IconLevel%
Private SectionLevelName(99) As String

Private Const CuteButtons = False
Private Const MoveHeadings = 0
Private Const MakeBoxyHeaders = False
Private LinkToImageFiles% '0=store data in document, 1=link+store in doc 2=soft links, -1=do not process images (assigned in Init())

Public Const Asterisks80 = "********************************************************************************"
Private Const SixSplats = "******"
Private Const SevenSplats = "*******"
Private Const TensPlace = "         1         2         3         4         5         6         7         8"
Private Const OnesPlace = "12345678901234567890123456789012345678901234567890123456789012345678901234567890"
Private Const MaxRowLength = 80
Private Const MaxSectionNameLen = 53
Private Const TableType = "Table-type "
Private Const lenTableType = 11
Private WholeCardHeader As String
Private lenWholeCardHeader As Long

Private TotalTruncated As Long
Private TotalRepeated As Long

Declare Function ShellExecute Lib _
    "shell32.dll" Alias "ShellExecuteA" _
    (ByVal hwnd As Long, _
    ByVal lpOperation As String, _
    ByVal lpFile As String, _
    ByVal lpParameters As String, _
    ByVal lpDirectory As String, _
    ByVal nShowCmd As Long) As Long

'Returns position of first character from chars in str
'Returns len(str) + 1 if none were found (0 if none found and reverse=true)
Private Function FirstCharPos(start&, str$, chars$, Optional reverse As Boolean = False) As Long
  Dim retval&, curval&, CharPos&, LenChars&
  If reverse Then retval = 0 Else retval = Len(str) + 1
  LenChars = Len(chars)
  For CharPos = 1 To LenChars
    If reverse Then
      curval = InStrRev(str, Mid(chars, CharPos, 1), start)
      If curval > retval Then retval = curval
    Else
      curval = InStr(start, str, Mid(chars, CharPos, 1))
      If curval > 0 And curval < retval Then retval = curval
    End If
  Next CharPos
  FirstCharPos = retval
End Function

Public Sub CreateHelpProject(IDfileExists As Boolean)
  Dim outf%
  outf = FreeFile
  Open SaveDirectory & BaseName & ".hpj" For Output As outf
  Print #outf, "[OPTIONS]" & vbCrLf
  Print #outf, "LCID=0x409 0x0 0x0 ; English (United States)" & vbCrLf
  Print #outf, "REPORT=Yes" & vbCrLf
  Print #outf, "CNT=" & BaseName & ".cnt" & vbCrLf & vbCrLf
  Print #outf, "HLP=" & BaseName & ".hlp" & vbCrLf & vbCrLf
  
  Print #outf, "[FILES]" & vbCrLf
  Print #outf, HelpSourceRTFName & vbCrLf & vbCrLf
  
  If IDfileExists Then
    Print #outf, "[MAP]" & vbCrLf
    Print #outf, "#include <" & BaseName & ".ID>" & vbCrLf & vbCrLf
  End If
  
  Print #outf, "[WINDOWS]" & vbCrLf
  Print #outf, "Main=" & Chr$(34) & BaseName & " Manual" & Chr$(34) & ", , 60672, (r14876671), (r12632256), f2; " & vbCrLf & vbCrLf; ""
  
  Print #outf, "[CONFIG]" & vbCrLf
  Print #outf, "BrowseButtons()" & vbCrLf
  Close outf
End Sub

Public Function HTMLRelativeFilename(WinFilename$, WinStartPath$) As String
  HTMLRelativeFilename = ReplaceString(RelativeFilename(WinFilename, WinStartPath), "\", "/")
End Function

Private Sub OpenHTMLHelpProjectfile()
  'If OutputFormat = tHTMLHELP Then
  HTMLHelpProjectfile = FreeFile
  Open SaveDirectory & BaseName & ".hhp" For Output As HTMLHelpProjectfile
  Print #HTMLHelpProjectfile, "[OPTIONS]" & vbLf;
  Print #HTMLHelpProjectfile, "Auto Index=Yes" & vbLf;
  Print #HTMLHelpProjectfile, "Compatibility=1.1 Or later" & vbLf;
  Print #HTMLHelpProjectfile, "Compiled file=" & BaseName & ".chm" & vbLf;
  Print #HTMLHelpProjectfile, "Contents file=" & BaseName & ".hhc" & vbLf;
  'Print #HTMLHelpProjectfile, "Default topic=Introduction.html"
  Print #HTMLHelpProjectfile, "Display compile progress=Yes" & vbLf;
  Print #HTMLHelpProjectfile, "Enhanced decompilation=Yes" & vbLf;
  Print #HTMLHelpProjectfile, "Full-text search=Yes" & vbLf;
  Print #HTMLHelpProjectfile, "Index file = " & BaseName & ".hhk" & vbLf;
  Print #HTMLHelpProjectfile, "Language=0x409 English (United States)" & vbLf;
  Print #HTMLHelpProjectfile, "Title=" & BaseName & " Manual" & vbLf & vbLf;
  'Print #HTMLHelpProjectfile, ""
  Print #HTMLHelpProjectfile, "[Files]" & vbLf;
  AliasSection = vbLf & "[ALIAS]"
End Sub

Private Sub CheckStyle()
  Dim startTag As Long, closeTag As Long
  startTag = InStr(LCase(TargetText), "<style")
  If startTag > 0 Then
    closeTag = InStr(startTag, TargetText, ">")
    If closeTag < startTag Then
      MsgBox "Style tag not terminated in " & SourceFilename
    Else
      ReadStyleFile Mid(TargetText, startTag + 7, closeTag - startTag - 7), HeadingLevel
    End If
  ElseIf HeadingLevel <= StyleLevel Then
    StyleLevel = StyleLevel - 1
    While Len(StyleFile(StyleLevel)) = 0
      StyleLevel = StyleLevel - 1
    Wend
    ReadStyleFile "", StyleLevel
  End If
End Sub

Private Sub ReadStyleFile(StyleFilename As String, HeadingLevel As Integer)
  Dim InFile As Integer
  Dim buf As String, CurrSection As String, FirstChar As String, level As Integer
  
  For level = 1 To maxLevels
    Set WordStyle(level) = Nothing
    Set WordStyle(level) = New Collection
  Next
  
  If Len(StyleFilename) = 0 Then
    StyleFilename = StyleFile(HeadingLevel)
  Else
    If Len(Dir(StyleFilename)) = 0 Then
      If Len(Dir(StyleFilename & ".sty")) > 0 Then
        StyleFilename = StyleFilename & ".sty"
      End If
    End If
    StyleFilename = CurDir & "\" & StyleFilename
  End If

  If Len(Dir(StyleFilename)) > 0 Then
    InFile = FreeFile(0)
    Open StyleFilename For Input As InFile
    StyleFile(HeadingLevel) = StyleFilename
    StyleLevel = HeadingLevel
    While Not EOF(InFile)
      Line Input #InFile, buf
      buf = Trim(buf)
      FirstChar = Left(buf, 1)
      Select Case FirstChar
        Case "#", ""
          'skip comments and blank lines
        Case "["
          CurrSection = LCase(Mid(buf, 2, Len(buf) - 2))
          level = 0
        Case Else
          If IsNumeric(FirstChar) Then
            level = CInt(FirstChar)
            buf = Mid(buf, 2)
            While IsNumeric(Left(buf, 1))
              level = level * 10 + CInt(Left(buf, 1))
              buf = Mid(buf, 2)
            Wend
            While Left(buf, 1) = " " Or Left(buf, 1) = "="
              buf = Mid(buf, 2)
            Wend
            GoSub SetVal
          ElseIf CurrSection = "printstart" Then
            If OutputFormat = tPRINT Then WordCommand buf, 0
          Else
            For level = 0 To maxLevels
              GoSub SetVal
            Next level
          End If
      End Select
    Wend
  End If
  Exit Sub

SetVal:
  Select Case CurrSection
    
    Case "printsection":  WordStyle(level).Add buf
    Case "top":           HeaderStyle(level) = buf
    Case "bottom":        FooterStyle(level) = buf
    Case "body":
      If Len(buf) > 0 Then
        BodyStyle(level) = "<body " & buf & ">"
      Else
        BodyStyle(level) = "<body>"
      End If
  End Select
  Return
  
End Sub

Private Sub WordCommand(ByVal cmdline As String, ByVal localHeadingLevel As Long)
  Dim cmd As String, arg As String, val As String, isnum As Boolean
  Dim consuming As String, intval As Integer
  
  DoEvents
  On Error GoTo WordCommandErr
  consuming = cmdline
  cmd = StrSplit(consuming, " ", """")
  With word
    Select Case LCase(cmd)
'      Case "applystyle":
'        If IsNumeric(consuming) Then localHeadingLevel = CLng(consuming)
'        .EditBookmark "Hstart" & localHeadingLevel
'        .EditGoTo "Hend" & localHeadingLevel
'        .Insert vbCr & vbCr
'        .EditGoTo "Hstart" & localHeadingLevel
'        .ExtendSelection
'        .EditGoTo "Hend" & localHeadingLevel
'        .Style "ADheading" & localHeadingLevel
'        .Cancel
'        .CharRight
      Case "borderbottom":   If IsNumeric(consuming) Then .BorderBottom CInt(consuming)
      Case "borderinside":   If IsNumeric(consuming) Then .BorderInside CInt(consuming)
      Case "borderleft":     If IsNumeric(consuming) Then .BorderLeft CInt(consuming)
      Case "borderlinestyle" '0=none, 1 to 6 increasing thickness, 7,8,9 double, 10 gray, 11 dashed
        Select Case LCase(Trim(consuming))
          Case "0", "1", "2", "3", "4", "5", "6"
            .BorderLineStyle CInt(consuming)
          Case "none":        .BorderLineStyle 0
          Case "thin":        .BorderLineStyle 1
          Case "thick":       .BorderLineStyle 6
          Case "double":      .BorderLineStyle 7
          Case "doublethick": .BorderLineStyle 9
          Case "dashed":      .BorderLineStyle 11
          Case Else: MsgBox "Unknown BorderLineStyle: " & consuming, vbOKOnly, "AuthorDoc:WordCommand"
        End Select
      Case "bordernone":     If IsNumeric(consuming) Then .BorderNone CInt(consuming)
      Case "borderoutside":  If IsNumeric(consuming) Then .BorderOutside CInt(consuming)
      Case "borderright":    If IsNumeric(consuming) Then .BorderRight CInt(consuming)
      Case "bordertop":      If IsNumeric(consuming) Then .BorderTop CInt(consuming)
      Case "charleft":       .CharLeft
      Case "charright":      .CharRight
      Case "centerpara":     .CenterPara
      Case "editclear"
        If IsNumeric(consuming) Then
          .EditClear CLng(consuming)
        Else
          .EditClear
        End If
      Case "editselectall":  .EditSelectAll
      Case "filepagesetup"
        While Len(consuming) > 0
          val = StrSplit(consuming, ",", """")
          arg = StrSplit(val, ":=", """")
          If IsNumeric(val) Then intval = CInt(val) Else intval = 0
          Select Case LCase(arg)
            Case "topmargin":       word.FilePageSetup TopMargin:=val
            Case "bottommargin":    word.FilePageSetup BottomMargin:=val
            Case "leftmargin":      word.FilePageSetup LeftMargin:=val
            Case "rightmargin":     word.FilePageSetup RightMargin:=val
            Case "headerdistance":  word.FilePageSetup HeaderDistance:=val
            Case "facingpages":     word.FilePageSetup FacingPages:=intval
            Case "oddandevenpages": word.FilePageSetup OddAndEvenPages:=intval
          End Select
        Wend
'      Case "formatdefinestyleborders"
'        While Len(consuming) > 0
'          val = StrSplit(consuming, ",", """")
'          arg = StrSplit(val, ":=", """")
'          If IsNumeric(val) Then
'            intval = CInt(val)
'            Select Case LCase(arg)
'              Case "topborder":      .FormatDefineStyleBorders TopBorder:=intval
'              Case "leftborder":     .FormatDefineStyleBorders LeftBorder:=intval
'              Case "bottomborder":   .FormatDefineStyleBorders BottomBorder:=intval
'              Case "rightborder":    .FormatDefineStyleBorders RightBorder:=intval
'              Case "horizborder":    .FormatDefineStyleBorders HorizBorder:=intval
'              Case "vertborder":     .FormatDefineStyleBorders VertBorder:=intval
'              Case "topcolor":       .FormatDefineStyleBorders TopColor:=intval
'              Case "leftcolor":      .FormatDefineStyleBorders LeftColor:=intval
'              Case "bottomcolor":    .FormatDefineStyleBorders BottomColor:=intval
'              Case "rightcolor":     .FormatDefineStyleBorders RightColor:=intval
'              Case "horizcolor":     .FormatDefineStyleBorders HorizColor:=intval
'              Case "vertcolor":      .FormatDefineStyleBorders VertColor:=intval
'              Case "foreground":     .FormatDefineStyleBorders Foreground:=intval
'              Case "background":     .FormatDefineStyleBorders Background:=intval
'              Case "shading":        .FormatDefineStyleBorders Shading:=intval
'              Case "fineshading":    .FormatDefineStyleBorders FineShading:=intval
'            End Select
'          Else
'            MsgBox "non-numeric value for " & arg & " in " & cmd, vbOKOnly, "AuthorDoc:WordCommand"
'          End If
'        Wend
      Case "formatdefinestylefont"
        While Len(consuming) > 0
          val = StrSplit(consuming, ",", """")
          arg = StrSplit(val, ":=", """")
          If LCase(arg) = "font" Then
            .FormatDefineStyleFont Font:=val
          ElseIf IsNumeric(val) Then
            intval = CInt(val)
            Select Case LCase(arg)
              Case "points":     .FormatDefineStyleFont Points:=intval
              Case "underline":  .FormatDefineStyleFont Underline:=intval
              Case "allcaps":    .FormatDefineStyleFont AllCaps:=intval
              Case "kerning":    .FormatDefineStyleFont Kerning:=intval
              Case "kerningmin": .FormatDefineStyleFont KerningMin:=intval
              Case "bold":       .FormatDefineStyleFont Bold:=intval
              Case "italic":     .FormatDefineStyleFont Italic:=intval
              Case "outline":    .FormatDefineStyleFont Outline:=intval
              Case "shadow":     .FormatDefineStyleFont Shadow:=intval
              Case "font":
            End Select
          End If
        Wend
'      Case "formatdefinestylepara"
'        While Len(consuming) > 0
'          val = StrSplit(consuming, ",", """")
'          arg = StrSplit(val, ":=", """")
'          isnum = IsNumeric(val)
'          If isnum Then
'            intval = CInt(val)
'            Select Case LCase(arg)
'              Case "before":       .FormatDefineStylePara Before:=intval
'              Case "after":        .FormatDefineStylePara After:=intval
'              Case "keepwithnext": .FormatDefineStylePara KeepWithNext:=intval
'              Case "alignment":    .FormatDefineStylePara Alignment:=intval
'            End Select
'          Else
'            MsgBox "non-numeric value for " & arg & " in " & cmd, vbOKOnly, "AuthorDoc:WordCommand"
'          End If
'        Wend
      Case "formatfont"
        While Len(consuming) > 0
          val = StrSplit(consuming, ",", """")
          arg = StrSplit(val, ":=", """")
          If LCase(arg) = "font" Then
            .FormatFont Font:=val
          ElseIf Len(val) = 0 Then
            If IsNumeric(arg) Then .FormatFont Points:=arg
          ElseIf IsNumeric(val) Then
            intval = CInt(val)
            Select Case LCase(arg)
              Case "points":     .FormatFont Points:=intval
              Case "underline":  .FormatFont Underline:=intval
              Case "allcaps":    .FormatFont AllCaps:=intval
              Case "kerning":    .FormatFont Kerning:=intval
              Case "kerningmin": .FormatFont KerningMin:=intval
              Case "bold":       .FormatFont Bold:=intval
              Case "italic":     .FormatFont Italic:=intval
              Case "outline":    .FormatFont Outline:=intval
              Case "shadow":     .FormatFont Shadow:=intval
            End Select
          Else
            MsgBox "non-numeric value for " & arg & " in " & cmd, vbOKOnly, "AuthorDoc:WordCommand"
          End If
        Wend
      Case "formatheaderfooterlink": .FormatHeaderFooterLink
      Case "formatpagenumber"
        While Len(consuming) > 0
          val = StrSplit(consuming, ",", """")
          arg = StrSplit(val, ":=", """")
          If LCase(arg) = "font" Then
            .FormatFont Font:=val
          ElseIf Len(val) = 0 Then
            If IsNumeric(arg) Then .FormatFont Points:=arg
          ElseIf IsNumeric(val) Then
            intval = CInt(val)
            Select Case LCase(arg)
              Case "chapternumber": .FormatPageNumber ChapterNumber:=intval
              Case "numrestart":    .FormatPageNumber NumRestart:=intval
              Case "numformat":     .FormatPageNumber NumFormat:=intval
              Case "startingnum":   .FormatPageNumber StartingNum:=intval
              Case "level":         .FormatPageNumber level:=intval
              Case "separator":     .FormatPageNumber Separator:=intval
            End Select
          Else
            MsgBox "non-numeric value for " & arg & " in " & cmd, vbOKOnly, "AuthorDoc:WordCommand"
          End If
        Wend
      Case "formatpara", "formatparagraph"
        While Len(consuming) > 0
          val = StrSplit(consuming, ",", """")
          arg = StrSplit(val, ":=", """")
          isnum = IsNumeric(val)
          If isnum Then
            intval = CInt(val)
            Select Case LCase(arg)
              Case "before":       .FormatParagraph Before:=intval
              Case "after":        .FormatParagraph After:=intval
              Case "keepwithnext": .FormatParagraph KeepWithNext:=intval
              Case "alignment":    .FormatParagraph Alignment:=intval
            End Select
          Else
            MsgBox "non-numeric value for " & arg & " in " & cmd, vbOKOnly, "AuthorDoc:WordCommand"
          End If
        Wend
'      Case "formatstyle"
'        arg = StrSplit(consuming, ",", """")
'        If arg = "Normal" Then 'Provide some good defaults in case they aren't explicit in the style file
'          .FormatStyle arg, AddToTemplate:=1, Define:=1
'          .FormatDefineStyleFont 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 1, 1, "Times New Roman", 0, 0, 0, 0
'          .FormatDefineStylePara Chr$(34), Chr$(34), 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, Chr$(34)
'          .FormatDefineStyleLang "English (US)", 1
'          .FormatDefineStyleBorders 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1
'        Else
'          .FormatStyle arg, BasedOn:="Normal", AddToTemplate:=0, Define:=1
'          .FormatStyle arg, Delete:=1
'          .FormatStyle arg, BasedOn:="Normal", AddToTemplate:=0, Define:=1
'        End If
      Case "formattabs"
        arg = StrSplit(consuming, ",", """")
        val = StrSplit(consuming, ",", """") '1=left, 2=right
        If Not IsNumeric(arg) Then
          MsgBox "non-numeric value for tab position in " & cmd, vbOKOnly, "AuthorDoc:WordCommand"
        ElseIf Not IsNumeric(val) Then
          MsgBox "non-numeric value for alignment in " & cmd, vbOKOnly, "AuthorDoc:WordCommand"
        Else
          intval = val
          .FormatTabs arg & """", Align:=intval, Set:=1
        End If
      Case "formattabsclear":  .FormatTabs ClearAll:=1
      Case "gotoheaderfooter": .GoToHeaderFooter
      Case "insert": .Insert ReplaceStyleString(consuming, localHeadingLevel)
      Case "insertbreak":
        Select Case LCase(Trim(consuming))
          '0 (zero) Page break, 1 Column break, 2 Next Page section break, 3 Continuous section break, 4 Even Page section break, 5 Odd Page section break, 6 Line break (newline character)
          Case "0", "1", "2", "3", "4", "5", "6"
            .InsertBreak CInt(consuming)
          Case "page":            .InsertBreak 0
          Case "column":          .InsertBreak 1
          Case "pagesection":     .InsertBreak 2
          Case "contsection":     .InsertBreak 3
          Case "evenpagesection": .InsertBreak 4
          Case "oddpagesection":  .InsertBreak 5
          Case "line":            .InsertBreak 6
          Case Else: MsgBox "Unknown argument to InsertBreak: " & consuming
        End Select
      Case "insertdatetime":
        If Len(Trim(consuming)) > 0 Then
          .InsertDateTime consuming, 0
        Else
          .InsertDateTime "   hh:mm MMMM d, yyyy", 0
        End If
      Case "insertfield": .InsertField consuming
      Case "insertpagenumbers"
        Dim typeVal As Integer, posVal As Integer, firstVal As Integer
        typeVal = 1
        posVal = 1
        firstVal = 0
        While Len(consuming) > 0
          val = StrSplit(consuming, ",", """")
          arg = StrSplit(val, ":=", """")
          isnum = IsNumeric(val)
          If isnum Then
            intval = CInt(val)
            Select Case LCase(arg)
              Case "type":      typeVal = intval
              Case "position":  posVal = intval
              Case "firstpage": firstVal = intval
            End Select
          Else
            MsgBox "non-numeric value for " & arg & " in " & cmd, vbOKOnly, "AuthorDoc:WordCommand"
          End If
        Wend
        .InsertPageNumbers Type:=typeVal, Position:=posVal, FirstPage:=firstVal
      Case "InsertParagraphsAroundImages":
        Select Case LCase(consuming)
          Case "0", "false": InsertParagraphsAroundImages = False
          Case "1", "true":  InsertParagraphsAroundImages = True
        End Select
      Case "shownextheaderfooter":   .ShowNextHeaderFooter
      Case "startofdocument":        .StartOfDocument
      Case "tableprintapply":  If IsNumeric(consuming) Then TablePrintApply = CLng(consuming)
      Case "tableprintformat": If IsNumeric(consuming) Then TablePrintFormat = CLng(consuming)
      Case "toggleheaderfooterlink": .ToggleHeaderFooterLink
      Case "viewfooter": .ViewFooter
      Case "viewheader": .ViewHeader
      Case "viewfooterandset"
        ViewFooterAndSet ReplaceStyleString(consuming, localHeadingLevel)
      Case "viewheaderandset"
        ViewHeaderAndSet ReplaceStyleString(consuming, localHeadingLevel)
      Case "viewnormal": .ViewNormal
      Case "viewpage": .ViewPage
      Case Else: MsgBox "WordCommand not recognized: " & cmd
    End Select
  End With
  Exit Sub
WordCommandErr:
  'MsgBox "Error with Word command '" & cmdline & "'" & vbCr & Err.Description
  Debug.Print "Error with Word command '" & cmdline & "'" & vbCr & Err.Description
End Sub

Private Function ReplaceStyleString(str As String, localHeadingLevel As Long) As String
  Dim retval As String, level As Long, wordpos As Long, endwordpos As Long, wordstr As String, wordnum As Long
  retval = str
  retval = ReplaceString(retval, "<sectionname>", HeadingText(localHeadingLevel))
  For level = 1 To localHeadingLevel
    retval = ReplaceString(retval, "<sectionname " & level & ">", HeadingText(level))
  Next
  retval = ReplaceString(retval, "vbTab", vbTab)
  retval = ReplaceString(retval, "vbCr", vbCr)
  retval = ReplaceString(retval, "vbLf", vbLf)
  retval = ReplaceString(retval, "vbCrLf", vbCrLf)
  wordpos = InStr(retval, "<sectionword")
  While wordpos > 0
    endwordpos = InStr(wordpos + 12, retval, ">")
    If endwordpos = 0 Then
      wordpos = 0
    Else
      wordstr = Trim(Mid(retval, wordpos + 12, endwordpos - wordpos - 12))
      If IsNumeric(wordstr) Then
        wordnum = CInt(wordstr)
        wordstr = HeadingText(localHeadingLevel)
        While wordnum > 1
          StrSplit wordstr, " ", ""
        Wend
        wordstr = StrSplit(wordstr, " ", "")
        retval = Left(retval, wordpos - 1) & wordstr & Mid(retval, endwordpos + 1)
      Else
        retval = Left(retval, wordpos - 1) & HeadingText(localHeadingLevel) & Mid(retval, endwordpos + 1)
      End If
      wordpos = InStr(wordpos + 1, retval, "<sectionword")
    End If
  Wend
  ReplaceStyleString = retval
End Function

Public Sub Convert(outputAs%, makeContents As Boolean, timestamps As Boolean, makeUpNext As Boolean, makeID As Boolean, makeProject As Boolean)
  Dim lastSourceFilename$
  Dim i&, buf$
  Dim keyword As Variant
  Dim replaceSelectionOption As Long
  
  Set Keywords = New Collection
  Init
  OutputFormat = outputAs
  BuildContents = makeContents
  BuildProject = makeProject
  FooterTimestamps = timestamps
  UpNext = makeUpNext
  BuildID = makeID
  frmConvert.CmDialog1.DefaultExt = "txt"
  If Len(ProjectFileName) > 0 And Len(Dir(ProjectFileName)) > 0 Then
    PromptForFiles = False
    
    ProjectFile = FreeFile(0)
    Open ProjectFileName For Input As ProjectFile
    ReDim ProjectFileEntry(100)
    MaxProjectFileEntry = 0
    
    While Not EOF(ProjectFile)
      Line Input #ProjectFile, buf
      If Len(Trim(buf)) > 0 Then
        MaxProjectFileEntry = MaxProjectFileEntry + 1
        If MaxProjectFileEntry > UBound(ProjectFileEntry) Then
          ReDim Preserve ProjectFileEntry(MaxProjectFileEntry * 2)
        End If
        ProjectFileEntry(MaxProjectFileEntry) = buf
      End If
    Wend
    ReDim Preserve ProjectFileEntry(MaxProjectFileEntry)
    NextProjectFileEntry = 1
    Close ProjectFile
    
  Else
    MsgBox "Could not open project file"
    Exit Sub
  End If
  'If Not OpenFile("Open list of source files:", ProjectFileName) Then Exit Sub
  SourceBaseDirectory = PathNameOnly(ProjectFileName) & "\"
  ChDrive SourceBaseDirectory
  ChDir SourceBaseDirectory
  'SaveDirectory = SourceBaseDirectory & BaseName & "ConversionOutput\"
  SaveDirectory = SourceBaseDirectory & "Out\"
  If Len(Dir(SaveDirectory, vbDirectory)) = 0 Then MkDir SaveDirectory
  If BuildProject Then
    If OutputFormat = tHELP Then
      CreateHelpProject True
    ElseIf OutputFormat = tHTMLHELP Then
      OpenHTMLHelpProjectfile
    ElseIf OutputFormat = tASCII Then
      HTMLHelpProjectfile = FreeFile
      Open SaveDirectory & BaseName & ".txt" For Output As HTMLHelpProjectfile
    End If
  End If
  
  If BuildID Then
    IDfile = FreeFile
    Open SaveDirectory & BaseName & ".ID" For Output As IDfile
    IDnum = 2
  End If
  
  InitContents
  PromptForFiles = False
  lastSourceFilename = ""
  SourceFilename = NextSourceFilename
  If OutputFormat = tPRINT Or OutputFormat = tHELP Then
    Set word = CreateObject("Word.Basic")
    With word
      .AppShow
      '.ToolsOptionsView PicturePlaceHolders:=1
      .ChDir SaveDirectory
      If OutputFormat = tPRINT Then
        .FileNewDefault
        DefinePrintStyles
        .FileSaveAs SaveDirectory & BaseName & ".doc", 0
        TargetWin = .WindowName
      ElseIf OutputFormat = tHELP Then
        .FileNewDefault
        .FilePageSetup PageWidth:="12 in"
        .FileSaveAs SaveDirectory & HelpSourceRTFName, 6
        TargetWin = .WindowName
      End If
      .ChDir SourceBaseDirectory
    End With
  End If
  ReadStyleFile BaseName & ".sty", 0
  LastHeadingLevel = 0
  While Len(SourceFilename) > 0 And SourceFilename <> lastSourceFilename
OpeningFile:
    Status "Opening " & SourceFilename
    lastSourceFilename = SourceFilename
    FirstHeaderInFile = True
    DoEvents
    If OutputFormat = tPRINT Or OutputFormat = tHELP Then
      With word
        .Activate TargetWin
        .ScreenUpdating 0  'comment out to debug (show lots of updates)
        .EditBookmark "CurrentFileStart"
        On Error GoTo FileNotFound
        .Insert WholeFileString(Directory & SourceFilename)
        On Error GoTo 0
        NumberHeaderTagsWithWord
        If LinkToImageFiles >= 0 Then
          .EditGoTo "CurrentFileStart"
          TranslateIMGtags Directory & SourceFilename
        End If
        .EndOfDocument
        .ScreenUpdating 1
      End With
    ElseIf OutputFormat = tASCII Then
      i = FreeFile(0)
      On Error GoTo FileNotFound
      Open Directory & SourceFilename For Input As #i  'SourceBaseDirectory &
      On Error GoTo 0
      While Not EOF(i)  ' Loop until end of file.
        ParseHSPFsourceLine i
      Wend
      If BuildProject And Keywords.Count > 0 Then
        Print #HTMLHelpProjectfile, vbCrLf & "[Keywords]" & vbCrLf;
        For Each keyword In Keywords
          Print #HTMLHelpProjectfile, keyword & vbCrLf;
        Next
      End If
      Close i
    ElseIf OutputFormat = tHTML Or OutputFormat = tHTMLHELP Then
      Dim dotpos&
      'OpenFile SourceFilename, SourceBaseDirectory & SourceFilename
      SourceText = WholeFileString(SourceBaseDirectory & SourceFilename)
      TargetText = Trim(SourceText)
TrimTargetText:
      Select Case Left(TargetText, 1)
        Case vbCr, vbLf, vbTab, " ":
          TargetText = Mid(TargetText, 2)
          GoTo TrimTargetText
      End Select
      If Len(TargetText) = 0 Then TargetText = "<toc>"
      dotpos = InStrRev(SourceFilename, ".")
      If dotpos > 1 Then SaveFilename = Left(SourceFilename, dotpos - 1) Else SaveFilename = SourceFilename
      SaveFilename = SaveFilename & ".html"
      If OutputFormat = tHTMLHELP Then
        FormatTag "b", OutputFormat
        FormatKeywordsHTMLHelp
        If BuildProject Then Print #HTMLHelpProjectfile, SaveFilename & vbLf;
      End If
      NumberHeaderTags
      CheckStyle
      FormatHeadings tHTML, SaveFilename
      TranslateButtons OutputFormat
      MakeLocalTOCs
      HREFsInsureExtension
      AbsoluteToRelative
      CopyImages
      FormatCardGraphic
      SaveInNewDir SaveDirectory & SaveFilename
    End If
    Status "Closing " & SourceFilename
OpenNextFile:
    SourceFilename = NextSourceFilename
  Wend
  If OutputFormat = tHTMLHELP And makeProject Then
    Print #HTMLHelpProjectfile, AliasSection & vbLf;
    Print #HTMLHelpProjectfile, "[MAP]" & vbLf & "#include " & BaseName & ".ID" & vbLf;
    Close HTMLHelpProjectfile
  ElseIf OutputFormat = tASCII Then
    Close IDfile
    Close HTMLHelpProjectfile
  End If
  If (OutputFormat = tPRINT Or OutputFormat = tHELP) Then
    With word
      .ToolsOptionsEdit (replaceSelectionOption) 'save current value of this option
      .ToolsOptionsEdit (1) 'be sure option is on
      .ScreenUpdating 0 'comment out to debug (show lots of updates)
      .Activate TargetWin
      ConvertTablesToWord
      ConvertTagsToWord
      If makeContents Then
        If OutputFormat = tHTMLHELP Or OutputFormat = tHTML Then
          FinishHTMLHelpContents
        'ElseIf OutputFormat = tHTML Then
        '  .Activate ContentsWin
        '  .FileSaveAs Directory & "Contents.html", 2
        '  .FileClose 2
        ElseIf OutputFormat = tPRINT Then
          .Activate TargetWin
          .StartOfDocument
          .Insert "Contents" & vbCr & vbCr
          .InsertTableOfContents 0, 0, AddedStyles:="ADheading1,1,ADheading2,2,ADheading3,3,ADheading4,4,ADheading5,5,ADheading6,6", RightAlignPageNumbers:=1
        ElseIf Len(ContentsWin) > 0 Then
          .Activate ContentsWin
          .FileSave
          .FileClose 2
        End If
      End If
      .ToolsOptionsEdit (replaceSelectionOption)
      If Len(TargetWin) > 0 Then
        .Activate TargetWin
        Status "Saving file: " & TargetWin
        .FileSave
        .FileClose 2
      End If
      .ScreenUpdating 1
      .AppClose
    End With
  ElseIf OutputFormat = tHTMLHELP Or OutputFormat = tHTML Then
    FinishHTMLHelpContents
  End If
  Set word = Nothing
  If IDfile > -1 Then Close IDfile
  If TotalTruncated > 0 Or TotalRepeated > 0 Then
    MsgBox "Total Truncated = " & TotalTruncated & vbCr & "Total Repeated = " & TotalRepeated
  End If
  Status "Conversion Finished"
  If OutputFormat = tHELP Then
    ShellExecute frmConvert.hwnd, "Open", SaveDirectory & BaseName & ".hpj", vbNullString, vbNullString, 1 'SW_SHOWNORMAL"
  ElseIf OutputFormat = tHTMLHELP Then
    ShellExecute frmConvert.hwnd, "Open", SaveDirectory & BaseName & ".hhp", vbNullString, vbNullString, 1 'SW_SHOWNORMAL"
  End If
  Exit Sub
FileNotFound:
  If MsgBox("Error opening " & Directory & SourceFilename & " (" & Err.Description & ")", vbRetryCancel, "Help Convert") = vbRetry Then
    GoTo OpeningFile
  Else
    GoTo OpenNextFile
  End If
End Sub

Private Sub ParseHSPFsourceLine(InFile As Long)
  Dim buf As String, buf2 As String
  Dim SectionNum As String, SectionDirName As String, SectionDir As String, SectionName As String
  Dim parsePos As Long
  Dim Lenbuf As Long
  Dim keyword As String, a As Long, a2 As Long, AllCapsStart As Long
  Static DirectoryLevels&
  Dim InHeader As Boolean
  Dim v As Variant
  Static CurrentOutputDirectory As String
  Static CurrentOutputFilename As String, ImageFilename As String
  Dim dummy As String
  Dim FileRepeat&
  
  DirectoryLevels = 0
  Line Input #InFile, buf
  buf = ReplaceString(buf, "<", "&lt;")
  buf = ReplaceString(buf, ">", "&gt;")
  buf = ReplaceString(buf, "&lt;pre&gt;", "<pre>")
  buf = ReplaceString(buf, "&lt;/pre&gt;", "</pre>")
  buf = ReplaceString(buf, "&lt;ol&gt;", "<ol>")
  buf = ReplaceString(buf, "&lt;/ol&gt;", "</ol>")
  buf = ReplaceString(buf, "&lt;li&gt;", "<li>")
  If IsNumeric(Left(buf, 1)) And Left(buf, 10) <> "1234567890" Then 'And Mid(buf, 3, 1) <> vbTab Then
    If IsNumeric(Trim(buf)) Then 'This indicates an image rather than a section header
      If Len(buf) > 3 Then GoTo NormalLine
      keyword = CStr(CLng(Left(buf, 2)) * 2)
      keyword = String(3 - Len(keyword), "0") & keyword
      keyword = SourceFilename & "_files\image" & keyword & ".png"
      For parsePos = 1 To DirectoryLevels
        keyword = "../" & keyword
      Next
      buf = "<p>" & "<img src=""" & keyword & """>"
    Else
      If Mid(buf, 2, 1) <> "." Then GoTo NormalLine
      If Not IsNumeric(Mid(buf, 3, 1)) Then GoTo NormalLine
      InHeader = True
      If IDfile > 0 Then
        If InPre Then Print #IDfile, vbCrLf & "</pre>" & vbCrLf;
        If FileKeywords.Count > 0 Then
          For Each v In FileKeywords
            Print #IDfile, "<keyword=" & v & ">" & vbCrLf
          Next
        End If
        Close IDfile
      End If
      InPre = False
      Set FileKeywords = Nothing
      Set FileKeywords = New Collection
      parsePos = InStr(buf, " ")
      SectionNum = Left(buf, parsePos - 1)
      SectionName = Trim(Mid(buf, parsePos + 1))
      
      Line Input #InFile, buf
      SectionName = SectionName & " " & Trim(buf)
      buf = ""
      
      parsePos = InStr(SectionName, " -- ")
      If parsePos > 0 Then
        buf = Trim(Mid(SectionName, parsePos + 4))
        SectionName = Trim(Left(SectionName, parsePos - 1))
        If InStr(UCase(SectionName), "BLOCK") > 0 Then
          SectionName = buf
          buf = ""
        End If
      Else
        parsePos = InStr(SectionName, "(")
        If parsePos > 0 Then
          buf = Trim(Mid(SectionName, parsePos))
          SectionName = Trim(Left(SectionName, parsePos - 1))
        End If
      End If
      If UCase(Left(SectionName, 7)) = "SECTION" Then SectionName = Mid(SectionName, 9)
      If UCase(Left(SectionName, lenTableType)) = UCase(TableType) Then SectionName = Mid(SectionName, lenTableType + 1)
      buf = "<SecNum " & SectionNum & "> " & "<h>" & SectionName & "</h>" & vbCrLf & buf & vbCrLf
      
      
      If Right(SectionNum, 2) = ".0" Then SectionNum = Left(SectionNum, Len(SectionNum) - 2)
      SectionDir = ""
      SectionDirName = ""
      parsePos = InStr(SectionNum, ".")
      If parsePos > 0 Then
        DirectoryLevels = 1
        parsePos = InStrRev(SectionNum, ".")
        SectionDir = Left(SectionNum, parsePos - 1)
        SectionDirName = SectionLevelName(DirectoryLevels)
        parsePos = InStr(SectionDir, ".")
        While parsePos > 0
          DirectoryLevels = DirectoryLevels + 1
          SectionDir = Left(SectionDir, parsePos - 1) & "\" & Mid(SectionDir, parsePos + 1)
          SectionDirName = SectionDirName & "\" & SectionLevelName(DirectoryLevels)
          parsePos = InStr(parsePos + 1, SectionDir, ".")
        Wend
        SectionDir = SectionDir & "\"
        SectionDirName = SectionDirName & "\"
      End If
      IDfile = FreeFile
      'If Len(Dir(SaveDirectory & SectionDir, vbDirectory)) = 0 Then MkDir SaveDirectory & SectionDir
      If Len(Dir(SaveDirectory & SectionDirName, vbDirectory)) = 0 Then MkDir SaveDirectory & SectionDirName
      'Debug.Print
      'Debug.Print SectionDir & SectionNum & ":" & CurrentOutputDirectory & CurrentOutputFilename
      CurrentOutputDirectory = SaveDirectory & SectionDirName 'SectionDir
      dummy = MakeValidFilename(SectionName)
      If Len(dummy) <= MaxSectionNameLen Then
        SectionLevelName(DirectoryLevels + 1) = dummy
      Else
        TotalTruncated = TotalTruncated + 1
        SectionLevelName(DirectoryLevels + 1) = Trim(Left(dummy, 34) & Right(dummy, 1)) 'MakeValidFilename(buf)
        Debug.Print "Truncated "; dummy & vbLf & "Shorter = " & SectionLevelName(DirectoryLevels + 1)
      End If
      FileRepeat = 1
SetFilenameHere:
      CurrentOutputFilename = SectionLevelName(DirectoryLevels + 1) & ".txt" 'Mid(SectionNum, Len(SectionDir) + 1) & ".txt"
      If Len(CurrentOutputDirectory & CurrentOutputFilename) > 255 Then
        MsgBox "Path longer than 255 characters detected:" & vbCr & CurrentOutputDirectory & vbCr & CurrentOutputFilename
      End If
      If Len(Dir(CurrentOutputDirectory & CurrentOutputFilename)) > 0 Then
        FileRepeat = FileRepeat + 1
        SectionLevelName(DirectoryLevels + 1) = SectionLevelName(DirectoryLevels + 1) & FileRepeat
        GoTo SetFilenameHere
      End If
      'Debug.Print Space(2 * DirectoryLevels) & "<li><a href=""Functional Description" & Mid(CurrentOutputDirectory, 21) & SectionLevelName(DirectoryLevels + 1) & """>" & dummy & "</a>"
      If FileRepeat > 1 Then TotalRepeated = TotalRepeated + 1
      Open CurrentOutputDirectory & CurrentOutputFilename For Output As IDfile
      If BuildProject Then Print #HTMLHelpProjectfile, Space(2 * DirectoryLevels) & SectionLevelName(DirectoryLevels + 1) 'Trim(Mid(buf, Len(SectionNum) + 1))  'Mid(SectionNum, Len(SectionDir) + 1)
    End If
  Else
NormalLine:
    If Trim(buf) <> "" Then
      If Left(buf, 3) = "{{{" Then
        ImageFilename = Mid(buf, 4, InStr(buf, "}}}") - 4) & ".png"
        buf = "<p><img src=""" & ImageFilename & """>"
RetryImage:
        On Error GoTo MissingImage
        FileCopy SourceBaseDirectory & "png\" & ImageFilename, CurrentOutputDirectory & ImageFilename
        On Error GoTo 0
      End If
      buf = ReplaceString(buf, "[[[", "<br><figure>")
      buf = ReplaceString(buf, "]]]", "</figure>")
      If InPre Then
        If InStr(buf, "Explanation") > 0 Then
          InPre = False
          buf = "</pre>" & vbCrLf & buf
        End If
      Else
        If InStr(buf, "****************************************") > 0 Or InStr(buf, "----------------------------------------") > 0 Then
          InPre = True
          buf = "<pre>" & vbCrLf & buf
        End If
      End If
      If Not InPre Then
        If Left(buf, 4) <> "<li>" Then buf = "<p>" & buf
      End If
    End If
  End If
  AllCapsStart = 0
  Lenbuf = Len(buf)
  For parsePos = 1 To Lenbuf
    a = Asc(Mid(buf, parsePos, 1))
    If a > 64 And a < 91 Then 'If capital letter, set AllCapsStart
      If AllCapsStart = 0 Then AllCapsStart = parsePos
    Else
      If AllCapsStart > 0 Then
        Select Case a
          Case 48 To 57     'Allow numbers as in PWAT-PARM1 but not F10
            If AllCapsStart < parsePos - 1 Then GoTo NextChar
          Case 32, 45, 95   'Allow spaces, dashes, underscores in keywords
            If parsePos + 2 <= Lenbuf Then
              a2 = Asc(Mid(buf, parsePos + 1, 1))
              If a2 > 64 And a2 < 91 Then           'If the next char after is capital
                a2 = Asc(Mid(buf, parsePos + 2, 1)) 'And the next one is not lowercase
                If Not (a2 > 96 And a2 < 123) Then GoTo NextChar
              End If
            End If
        End Select
        
        If parsePos - AllCapsStart > 2 Then
          keyword = Mid(buf, AllCapsStart, parsePos - AllCapsStart)
          On Error GoTo NewFileKeyword
          dummy = FileKeywords(keyword) 'Debug.Print "[" & FileKeywords(keyword) & "]";
          Err.Clear
AddedLocal:
'          On Error GoTo 0
'          On Error GoTo NewKeyword
'          Debug.Print "(" & Keywords(keyword) & ")";
NotNew:
          'If InHeader Then
            buf2 = buf2 & keyword & Chr(a)
          'Else
          '  buf2 = buf2 & vbcrlf _
          '      & "<Object id=hhctrl type=""application/x-oleobject""" & vbcrlf _
          '      & "classid = ""clsid:adb880a6-d8ff-11cf-9377-00aa003b7a11""" & vbcrlf _
          '      & "codebase = ""hhctrl.ocx#Version=4,74,8702,0"" Width = 100 Height = 100>" & vbcrlf _
          '      & "<param name=""Command"" value=""KLink"">" & vbcrlf _
          '      & "<param name=""Button"" value=""Text:" & keyword & """>" & vbcrlf _
          '      & "<param name=""Item1"" value="""">" & vbcrlf _
          '      & "<param name=""Item2"" value=""" & keyword & """>" & vbcrlf & "</OBJECT>" & vbcrlf & Chr(a)
          'End If
        Else
          buf2 = buf2 & Mid(buf, AllCapsStart, parsePos - AllCapsStart + 1)
        End If
        AllCapsStart = 0
      Else
        buf2 = buf2 & Chr(a)
      End If
    End If
NextChar:
  Next
  If AllCapsStart > 0 Then buf2 = buf2 & Mid(buf, AllCapsStart)
  Print #IDfile, buf2 & vbCrLf;
  Exit Sub
  
NewFileKeyword:
  FileKeywords.Add keyword, keyword
  'Debug.Print keyword & ";"
  Err.Clear
  Resume AddedLocal
NewKeyword:
  Keywords.Add keyword, keyword
  'Debug.Print "+" & keyword & ";";
  Err.Clear
  Resume NotNew
MissingImage:
  Select Case MsgBox("Missing Image: " & vbCr & ImageFilename, vbAbortRetryIgnore, "Missing")
    Case vbRetry:  GoTo RetryImage
    Case vbIgnore: Resume Next
    Case vbAbort:  Exit Sub
  End Select
End Sub

Private Sub ConvertTablesToWord()
  Dim TableText As String
  Dim lTableText As String
  Dim TableCols As Long
  Dim ColPos As Long
  Dim RowEnd As Long
  Dim HeaderCell() As Boolean
  Dim MergeCells As Long, MergeCount As Long
  With word
    .StartOfDocument
    .EditFindClearFormatting
    .EditFind "<table", "", 0
    While .EditFindFound
      .ExtendSelection
      .EditFind ">"
      If Not .EditFindFound Then Exit Sub
      .EditClear                  'delete <table...>
      .Cancel
      .EditBookmark "TableStart"
      .EditFind "</table>"
      If Not .EditFindFound Then Exit Sub
      .EditClear
      .EditBookmark "TableEnd"
      .ExtendSelection
      .EditGoTo "TableStart"
      While Asc(.Selection) < 33 'skip leading blanks and newlines
        .CharRight
      Wend
      .Cancel
      .EditBookmark "TableAll"
      
      'Count columns = # <th> + # <td> in first row
      TableText = LCase(.Selection)
      TableCols = 0
      ColPos = InStr(TableText, "tr>")
      RowEnd = InStr(ColPos + 1, TableText, "tr>")
      If RowEnd = 0 Then RowEnd = Len(TableText)
      While ColPos > 0 And ColPos < RowEnd
        TableCols = TableCols + 1
        ColPos = InStr(ColPos + 1, TableText, "<th")
        If Mid(TableText, ColPos + 3, 8) = " colspan" Then
          TableCols = TableCols + CInt(Mid(TableText, ColPos + 12, 1)) - 1
        End If
      Wend
      ColPos = InStr(TableText, "tr>")
      ColPos = InStr(ColPos + 1, TableText, "<td>")
      While ColPos > 0 And ColPos < RowEnd
        TableCols = TableCols + 1
        ColPos = InStr(ColPos + 1, TableText, "<td>")
      Wend
      TableCols = TableCols - 1
      ReDim HeaderCell(500, TableCols)
      'Make <th> bold
'      .EditFind "<tr><th>", Wrap:=0
'      While .EditFindFound
'        .EditClear
'        .ExtendSelection
'        .EditFind "<td>"
'        If Not .EditFindFound Then .EditGoTo "TableEnd"
'        .FormatFont Bold:=1
'        .Cancel
'        .EditGoTo "TableAll"
'        .EditFind "<tr><th>", Wrap:=0
'      Wend
      
      .EditGoTo "TableAll"
      .EditReplace "^p^w", "^p", ReplaceAll:=True
      .EditReplace "^w^p", "^p", ReplaceAll:=True
      .EditReplace "^p", " ", ReplaceAll:=True
      .FormatTabs "2""", Align:=0, Set:=1
      .EditReplace "<tr><th>", "^p", 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "<tr><td>", "^p", 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "<tr>^w<th>", "^p", 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "<tr>^w<td>", "^p", 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "<tr>", "^p", 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "<td>", vbTab, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "<th>", vbTab, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "<td ", vbTab, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "<th ", vbTab, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "<p>", "", 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .EditReplace "</tr>", "", 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0
      .CharRight
      .ExtendSelection
      .EditGoTo "TableStart"
      While Asc(.Selection) < 33 'skip leading blanks and newlines
        .CharRight
      Wend
      .TableInsertTable ConvertFrom:=1, NumColumns:=TableCols, Format:=TablePrintFormat, Apply:=TablePrintApply
      .EditBookmark "TableAll"
      .TableColumnWidth AutoFit:=1
      .EditFind "colspan="
      While .EditFindFound
        .EditClear
        .ExtendSelection
        .CharRight 'Want to merge more than 9 columns? probably not.
        MergeCells = .Selection
        .CharRight '>
        .EditClear
        .NextCell
        .CharLeft
        .ExtendSelection
        'For MergeCount = 2 To MergeCells
          .CharRight MergeCells
        'Next
        .TableMergeCells
        .EditGoTo "TableAll"
        .EditFind "colspan="
      Wend
      .CharRight
      .EditFind "<table", "", 0
      
    Wend
  End With
End Sub

Private Sub ConvertTagsToWord()
  With word
    Status "Removing HTML Headers"
    RemoveStuffOutsideBody
    
    Status "Translating Paragraph Marks"
    InsertParagraphsInPRE OutputFormat
    .StartOfDocument
    Status "Removing Whitespace After Paragraph Marks"
    .EditReplace "^p^w", "^p", ReplaceAll:=True
    Status "Removing Whitespace Before Paragraph Marks"
    .EditReplace "^w^p", "^p", ReplaceAll:=True
    Status "Removing Non-HTML Paragraphs"
    .EditReplace "^p", " ", ReplaceAll:=True
    
    TranslateLists "ul", 1
    TranslateLists "ol", 7

    Status "Replacing HTML Paragraphs"
    .EditReplace "<p>", "^p", ReplaceAll:=True
    Status "Replacing HTML Line Breaks"
    .EditReplace "<br>", "^l", ReplaceAll:=True
    Status "Removing Whitespace After Paragraph Marks"
    .EditReplace "^p^w", "^p", ReplaceAll:=True
    Status "Removing Whitespace Before Paragraph Marks"
    .EditReplace "^w^p", "^p", ReplaceAll:=True
    Status "Replacing HTML Page Breaks"
    .EditReplace "<page>", "^m", ReplaceAll:=True
    
    .EditSelectAll
    .FormatParagraph After:=10, LineSpacingRule:=3, LineSpacing:=32
    If OutputFormat = tHELP Then .FormatFont 12
    .Cancel
    Status "Translating Buttons"
    TranslateButtons OutputFormat
    Status "Formatting Headings"
    FormatHeadings OutputFormat, TargetWin
    FormatTag "div", OutputFormat
    FormatTag "pre", OutputFormat
    FormatTag "figure", OutputFormat
    FormatTag "u", OutputFormat
    FormatTag "b", OutputFormat
    FormatTag "i", OutputFormat
    FormatTag "sub", OutputFormat
    FormatTag "sup", OutputFormat
    
    If OutputFormat = tHELP Then HREFsToHelpHyperlinks
    Status "Removing Remaining HTML Tags"
    HTMLQuotedCharsToPrint
    
    'Once more, replace the few places where unwanted whitespace has crept back in
    .EditReplace "^p^w", "^p", ReplaceAll:=True
    .EditReplace "^w^p", "^p", ReplaceAll:=True
    .EditReplace "^p^p^p", "^p^p", ReplaceAll:=True
    
    .EditReplace "^pXyZ", "^p", ReplaceAll:=True 'Finally, unhide whitespace at start of lines in <pre>
    
    'Replace stupid quotes with smart quotes
    .ToolsOptionsAutoFormat 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1
    .FormatAutoFormat
  End With
End Sub

Private Sub SaveInNewDir(newFilePath$)
  Dim fname$, path$
  path = PathNameOnly(newFilePath)
  fname = Mid(newFilePath, Len(path) + 2)
  If Len(Dir(path, vbDirectory)) = 0 Then MkDir path
  If OutputFormat = tHTML Or OutputFormat = tHTMLHELP Then
    Dim OutFile As Integer
    OutFile = FreeFile
    Open newFilePath For Output As OutFile
    Print #OutFile, TargetText
    Close OutFile
  Else
    Dim oldpath$
    With word
      oldpath = .DefaultDir(14)
      .ChDir path
      .FileSaveAs fname, 2, AddToMRU:=False
      .ChDir oldpath
    End With
  End If
End Sub

Private Function NextSourceFilename() As String
  Dim lvl&, ch$, pos&, ach&, fn$ 'FileName, will be return value
  Dim buf$
  
Beginning:
  
  If NextProjectFileEntry > MaxProjectFileEntry Then
    NextSourceFilename = ""
  Else
    buf = ProjectFileEntry(NextProjectFileEntry)
    NextProjectFileEntry = NextProjectFileEntry + 1
    'insert levels of hierarchy for subsections indented two spaces
    fn = ""
    lvl = 1
    While Left(buf, 2) = "  "
      buf = Mid(buf, 3)
      fn = fn & HeadingWord(lvl) & "\"
      lvl = lvl + 1
    Wend
    buf = Trim(buf)
    HeadingWord(lvl) = fn
    pos = 1
    ch = Mid(buf, pos, 1)
    ach = Asc(ch)
    While ach > 31 And ach < 127 ' > 47 And ach < 58 Or ach > 64 And ach < 91 Or ach > 96 And ach < 123 Or ach = 92 'alphanumeric or \
      If ach = 34 Or ach = 42 Or ach = 47 Or ach = 58 Or ach = 60 Or ach = 62 Or ach = 63 Or ach = 124 Then 'illegal for file names
        fn = fn & "_"
      Else
        fn = fn & ch
      End If
      pos = pos + 1
      If pos <= Len(buf) Then
        ch = Mid(buf, pos, 1)
        ach = Asc(ch)
      Else
        ach = 0
      End If
    Wend
    If Len(fn) > Len(HeadingWord(lvl)) Then
      HeadingWord(lvl) = Mid(fn, 1 + Len(HeadingWord(lvl)))
      HeadingLevel = lvl
      NextSourceFilename = fn & SourceExtension
    Else
      NextSourceFilename = ""
    End If
  End If
End Function

'Private Function oldNextSourceFilename() As String
'  Dim lvl&, ch$, pos&, ach&, fn$ 'FileName, will be return value
'  Dim ListEntry$
'Beginning:
'  If EOF(ProjectFile) Then
'    NextSourceFilename = ""
'  Else
'    Line Input #ProjectFile, ListEntry
'    If Len(Trim(ListEntry)) = 0 Then GoTo Beginning 'skip blank lines
'    'insert levels of hierarchy for subsections indented two spaces
'    fn = ""
'    lvl = 1
'    While Left(ListEntry, 2) = "  "
'      ListEntry = Mid(ListEntry, 3)
'      fn = fn & HeadingWord(lvl) & "\"
'      lvl = lvl + 1
'    Wend
'    ListEntry = Trim(ListEntry)
'    HeadingWord(lvl) = fn
'    pos = 1
'    ch = Mid(ListEntry, pos, 1)
'    ach = Asc(ch)
'    While ach > 31 And ach < 127 ' > 47 And ach < 58 Or ach > 64 And ach < 91 Or ach > 96 And ach < 123 Or ach = 92 'alphanumeric or \
'      If ach = 34 Or ach = 42 Or ach = 47 Or ach = 58 Or ach = 60 Or ach = 62 Or ach = 63 Or ach = 124 Then 'illegal for file names
'        fn = fn & "_"
'      Else
'        fn = fn & ch
'      End If
'      pos = pos + 1
'      If pos <= Len(ListEntry) Then
'        ch = Mid(ListEntry, pos, 1)
'        ach = Asc(ch)
'      Else
'        ach = 0
'      End If
'    Wend
'    If Len(fn) > Len(HeadingWord(lvl)) Then
'      HeadingWord(lvl) = Mid(fn, 1 + Len(HeadingWord(lvl)))
'      HeadingLevel = lvl
'      NextSourceFilename = fn & SourceExtension
'    Else
'      NextSourceFilename = ""
'    End If
'  End If
'End Function

Private Sub InitContents()
  If BuildContents Then
    If OutputFormat = tHTML Then
      HTMLContentsfile = FreeFile
      Open SaveDirectory & "Contents.html" For Output As HTMLContentsfile
      Print #HTMLContentsfile, "<html><head><title>" & BaseName & " Help Contents</title></head>"
      Print #HTMLContentsfile, "<body>"
      Print #HTMLContentsfile, "<h1>Contents</h1>"
    ElseIf OutputFormat = tHTMLHELP Then
      HTMLContentsfile = FreeFile
      Open SaveDirectory & BaseName & ".hhc" For Output As HTMLContentsfile
      Print #HTMLContentsfile, "<html><head><!-- Sitemap 1.0 --></head>"
      Print #HTMLContentsfile, "<body>"
      Print #HTMLContentsfile, "<OBJECT type=""text/site properties"">"
      Print #HTMLContentsfile, "<param name=""ImageType"" value=""Folder"">"
      Print #HTMLContentsfile, "</OBJECT>"
     
      HTMLIndexfile = FreeFile
      Open SaveDirectory & BaseName & ".hhk" For Output As HTMLIndexfile
      Print #HTMLIndexfile, "<html><head></head>"
      Print #HTMLIndexfile, "<body>"
      Print #HTMLIndexfile, "<ul>"
     
     ElseIf OutputFormat = tHELP Then
      With word
        'Header of contents file
        .FileNewDefault
        .Insert ":Title " & BaseName & " Help" & vbCr
        .Insert ":Base " & BaseName & ".hlp" & vbCr
        .ChDir SaveDirectory
        .FileSaveAs BaseName & ".cnt", 2
        .ChDir SourceBaseDirectory
        ContentsWin = .WindowName()
      End With
    End If
  End If
End Sub

Sub Init()
  Dim HeaderLevel As Long
  
  TotalTruncated = 0
  TotalRepeated = 0
  BodyTag = "<body>"
  PromptForFiles = True
  NotFirstPrintHeader = False
  NotFirstPrintFooter = False
  InsertParagraphsAroundImages = False
  HelpSourceRTFName = BaseName & ".rtf"
  TablePrintFormat = 0
  TablePrintApply = 511
  IDfile = -1
  HTMLContentsfile = -1
  HTMLIndexfile = -1
  If AlreadyInitialized Then Exit Sub
  AlreadyInitialized = True
  PromptForFiles = True
  LinkToImageFiles = 0 '2 ' make soft links with word95 and large document
  frmConvert.CmDialog1.DefaultExt = "doc"
  Screen.MousePointer = vbHourglass
  frmConvert.Show
  Screen.MousePointer = vbDefault
  'IconLevel = 999

  WholeCardHeader = Asterisks80 & vbCrLf & TensPlace & vbCrLf & OnesPlace
  lenWholeCardHeader = Len(WholeCardHeader)

  'set default HTML styles
  For HeaderLevel = 0 To maxLevels
    HeaderStyle(HeaderLevel) = "<hr size=7><h2><sectionname></h2><hr size=7>"
    FooterStyle(HeaderLevel) = ""
    BodyStyle(HeaderLevel) = "<body>"
    Set WordStyle(HeaderLevel) = New Collection
  Next
End Sub

Sub SetUnInitialized()
    AlreadyInitialized = False
End Sub

'Function OpenFile(title, Optional filename) As Boolean
'  Dim InputFile%, buf$, lbuf$
'  Dim pos&
'  OpenFile = False
'  If IsMissing(filename) Then filename = ""
'
'  If Not PromptForFiles And filename <> "" And Len(Dir(Directory & filename)) > 0 Then
'    On Error GoTo nofile
'    Word.FileNew
'    InputFile = FreeFile
'    Open filename For Input As InputFile
'    lbuf = Input(LOF(InputFile), InputFile)
'    lbuf = ReplaceString(lbuf, "<html>", "      ")
'    lbuf = ReplaceString(lbuf, "<head>", "      ")
'    lbuf = ReplaceString(lbuf, "<body>", "      ")
'    lbuf = ReplaceString(lbuf, "<form>", "      ")
'    lbuf = ReplaceString(lbuf, "</html>", "       ")
'    lbuf = ReplaceString(lbuf, "</head>", "       ")
'    lbuf = ReplaceString(lbuf, "</body>", "       ")
'    lbuf = ReplaceString(lbuf, "</form>", "       ")
'    Word.Insert lbuf
''    While Not EOF(InputFile)  ' Loop until end of file.
''      Line Input #InputFile, buf
''      lbuf = LCase(buf)
''
''      'blank out confusing tags
''      pos = InStr(lbuf, "<html>")
''      If pos > 0 Then buf = Left(buf, pos - 1) & "      " & Mid(buf, pos + 6)
''      pos = InStr(lbuf, "<head>")
''      If pos > 0 Then buf = Left(buf, pos - 1) & "      " & Mid(buf, pos + 6)
''      pos = InStr(lbuf, "<body>")
''      If pos > 0 Then buf = Left(buf, pos - 1) & "      " & Mid(buf, pos + 6)
''      pos = InStr(lbuf, "<form>")
''      If pos > 0 Then buf = Left(buf, pos - 1) & "      " & Mid(buf, pos + 6)
''
''      pos = InStr(lbuf, "</html>")
''      If pos > 0 Then buf = Left(buf, pos - 1) & "       " & Mid(buf, pos + 7)
''      pos = InStr(lbuf, "</head>")
''      If pos > 0 Then buf = Left(buf, pos - 1) & "       " & Mid(buf, pos + 7)
''      pos = InStr(lbuf, "</body>")
''      If pos > 0 Then buf = Left(buf, pos - 1) & "       " & Mid(buf, pos + 7)
''      pos = InStr(lbuf, "</form>")
''      If pos > 0 Then buf = Left(buf, pos - 1) & "       " & Mid(buf, pos + 7)
''
''      Word.Insert buf & vbLf
''    Wend
'    Close InputFile
'    'Word.FileOpen Directory & filename, 0
'    OpenFile = True
'    Word.ViewNormal
'    Exit Function
'  End If
'  With frmConvert.CmDialog1
'    If Len(filename) > 0 Then .filename = filename
'
'    On Error GoTo nofile
'    .CancelError = True
'    .DialogTitle = title
'    .ShowOpen
'
'    If IsNull(Dir(.filename)) Then GoTo nofile
'
'    Directory = Left(.filename, Len(.filename) - Len(.FileTitle))
'    Word.FileOpen .filename
'  End With
'  Screen.MousePointer = vbHourglass
'  With Word
'    .ViewNormal
'    .ToolsOptionsView Hidden:=1
'    .ToolsOptionsEdit AutoWordSelection:=0, SmartCutPaste:=0
'  End With
'
'  Screen.MousePointer = vbDefault
'  OpenFile = True
'
'  Exit Function
'nofile:
'  frmConvert.CmDialog1.filename = ""
'End Function

Private Sub InsertParagraphsInPRE(OutputFormat As outputType)
 With word
  If OutputFormat = tPRINT Or OutputFormat = tHELP Then
    .StartOfDocument
    .EditFindClearFormatting
    .EditFind "<pre>", "", 0
    While .EditFindFound
      .CharRight
      .EditBookmark "Hstart"
      .EditFind "</pre>"
      If Not .EditFindFound Then Exit Sub
      .EditBookmark "Hend"
      .ExtendSelection
      .EditGoTo "Hstart"
      .Cancel
      .EditReplace "^p ", "<P>XyZ ", ReplaceAll:=True, Wrap:=0 'Hide spaces at start of line in <pre>
      .EditReplace "^p", "<P>", ReplaceAll:=True, Wrap:=0
      .EditGoTo "Hend"
      .CharRight
      .EditFind "<pre>", "", 0
    Wend
  End If
 End With
End Sub

Private Sub ApplyWordFormat(f$, OutputFormat As outputType, divArgs$)
  Dim caption$
  With word
    Select Case LCase(f)
      Case "sub": .Subscript
      Case "sup": .Superscript
      Case "b":   .FormatFont Bold:=1
      Case "i":   .FormatFont Italic:=1
      Case "u":
        If OutputFormat = tHELP Then .FormatFont Bold:=1 Else .FormatFont Underline:=1
      Case "figure"
        caption = .Selection
        .EditClear
        .InsertCaption "Figure", "", ": " & caption, Position:=1
        .Insert vbCr
      Case "pre"
        .FormatParagraph After:=0
        .FormatFont Font:="Courier New", Points:=9.5
      Case "div"
        If InStr(divArgs, "left") > 0 Then .FormatParagraph Alignment:=0
        If InStr(divArgs, "center") > 0 Then .FormatParagraph Alignment:=1
        If InStr(divArgs, "right") > 0 Then .FormatParagraph Alignment:=2
        If InStr(divArgs, "justify") > 0 Then .FormatParagraph Alignment:=3
    End Select
 End With
End Sub


Private Sub FormatTag(tag$, OutputFormat As outputType)
  Dim begintag As String, endtag As String, taggedText As String, insertText As String
  Dim startTag As Long, closeTag As Long, lenBeginTag As Long
  Dim divArgs As String
  
  begintag = "<" & tag & ">"
  endtag = "</" & tag & ">"
  If tag = "div" Then begintag = "<" & tag & " "
  lenBeginTag = Len(begintag)
  
  Status "Formatting HTML " & begintag
  
  Select Case OutputFormat
    Case tPRINT, tHELP
      With word
        .StartOfDocument
        .EditFindClearFormatting
        .EditFind begintag, "", 0
        While .EditFindFound
          .EditClear                           'delete beginTag
          .EditBookmark "Hstart"
          If tag = "div" Then
            .EditFind ">"
            If Not .EditFindFound Then Exit Sub
            .EditClear
            .ExtendSelection
            .EditGoTo "Hstart"
            divArgs = LCase(Trim(.Selection))
            .EditClear
            .Insert vbCr
            .EditBookmark "Hstart"
          Else
            divArgs = ""
          End If
          .EditFind endtag
          If Not .EditFindFound Then Exit Sub
          .EditClear                           'delete endTag
          .EditBookmark "Hend"
          .ExtendSelection
          .EditGoTo "Hstart"
          .Cancel
          ApplyWordFormat tag, OutputFormat, divArgs
          .CharRight
          .EditFind begintag, "", 0
        Wend
      End With
    Case tHTML, tHTMLHELP
      startTag = InStr(LCase(TargetText), begintag)
      While startTag > 0
        closeTag = InStr(startTag + 2, LCase(TargetText), endtag)
        If closeTag > 0 Then
          taggedText = Mid(TargetText, startTag + lenBeginTag, closeTag - (startTag + lenBeginTag))
          Select Case LCase(tag)
            Case "b":
              If InStr(taggedText, "<") > 0 Then
                insertText = ""
              ElseIf InStr(taggedText, ">") > 0 Then
                insertText = ""
              Else
                insertText = "<a name=""" & taggedText & """>" 'Insert link target for bold text
                If OutputFormat = tHTMLHELP Then 'Insert bold text in index
                  insertText = insertText & "<indexword=""" & taggedText & """>"
                End If
                TargetText = Left(TargetText, startTag - 1) & insertText & Mid(TargetText, startTag)
              End If
          End Select
        End If
        startTag = InStr(startTag + Len(insertText) + 2, LCase(TargetText), begintag)
      Wend
  End Select
End Sub

Private Sub FormatHeadings(OutputFormat As outputType, targetFilename$)
  If OutputFormat = tPRINT Or OutputFormat = tHELP Then
    FormatHeadingsWithWord OutputFormat, targetFilename
  Else
    Dim localHeadingLevel%, direction&, Selection As String
    Dim startTag As Long, startNumber As Long, endtag As Long, closeTag As Long, CloseTagEnd As Long
    Dim IconStart As Long, IconFilenameStart As Long, IconFilenameEnd As Long, IconEnd As Long
    
    BodyTag = BodyStyle(HeadingLevel)
    startTag = InStr(LCase(TargetText), "<body")
    If startTag > 0 Then
      endtag = InStr(startTag, TargetText, ">")
      If endtag > startTag Then
        BodyTag = Mid(TargetText, startTag, endtag - startTag + 1)
        TargetText = Left(TargetText, startTag - 1) & Mid(TargetText, endtag + 1)
      End If
    End If
    
    startTag = InStr(LCase(TargetText), "<h")
    While startTag > 0
      If localHeadingLevel = 0 Then localHeadingLevel = HeadingLevel
      direction = 1
      Select Case Mid(TargetText, startTag + 2, 1)
        Case ">": endtag = startTag + 2: GoTo FindingHend
'        Case "+": startNumber = startTag + 3
'        Case "-": startNumber = startTag + 3: direction = -1
        Case "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
          startNumber = startTag + 2
          'endTag = InStr(startTag, TargetText, ">")
          localHeadingLevel = 0
        Case Else: GoTo NextHeader
      End Select
      endtag = InStr(startTag, TargetText, ">")
      If endtag = 0 Then Exit Sub
      
      'now we have found the header number (startNumber..endtag-1)
      Selection = Mid(TargetText, startNumber, endtag - startNumber)
      If Len(Selection) > 1 Then
        If MsgBox("Warning: suspicious header tag '<h" & Selection & ">' " & vbCr _
                & "Found in '" & SourceFilename & "'" & vbCr _
                & "Continue processing this section?", vbYesNo) = vbNo Then Exit Sub
      End If
      If IsNumeric(Selection) Then
        localHeadingLevel = localHeadingLevel + direction * CInt(Selection)
      Else
        localHeadingLevel = localHeadingLevel + direction
      End If
FindingHend:
      closeTag = InStr(startTag, LCase(TargetText), "</h")
      If closeTag = 0 Then Exit Sub
      CloseTagEnd = InStr(closeTag, TargetText, ">")
      
      HeadingText(localHeadingLevel) = Trim(Mid(TargetText, endtag + 1, closeTag - endtag - 1))
      'If HeadingText(localHeadingLevel) = "Duration" Then Stop
      HeadingFile(localHeadingLevel) = SaveFilename
      
      TargetText = Left(TargetText, startTag - 1) & Mid(TargetText, CloseTagEnd + 1)
      '& localHeadingLevel & ">" & HeadingText(localHeadingLevel) & "</h" & localHeadingLevel & ">"
      'look for icon
'      If IconLevel >= localHeadingLevel Then IconLevel = 999
'      If OutputFormat = tHTML Or OutputFormat = tHTMLHELP Then
'        IconStart = InStr(LCase(Mid(TargetText, startTag, 10)), "<img src=")
'        If IconStart > 0 Then
'          IconStart = IconStart + startTag - 1
'          IconEnd = InStr(IconStart, TargetText, ">")
'          IconFilenameStart = InStr(IconStart, TargetText, """") + 1
'          If IconFilenameStart > IconStart And IconFilenameStart < IconEnd Then
'            IconFilenameEnd = InStr(IconFilenameStart + 1, TargetText, """") - 1
'            If IconFilenameEnd > IconFilenameStart And IconFilenameEnd < IconEnd Then
'              IconFilename = Mid(TargetText, IconFilenameStart, IconFilenameEnd - IconFilenameStart + 1)
'              IconLevel = localHeadingLevel
'              Debug.Print "Icon " & IconFilename & " - level " & IconLevel
'              TargetText = Left(TargetText, IconStart - 1) & Mid(TargetText, IconEnd + 1)
'            End If
'          End If
'        End If
'      End If
      startTag = startTag - 1
      FormatHeadingHTML localHeadingLevel, targetFilename, startTag 'Insert header and adjust startTag to end of header
      LastHeadingLevel = localHeadingLevel
NextHeader:
      startTag = InStr(startTag + 2, LCase(TargetText), "<h")
    Wend
  End If
End Sub

Private Sub FormatHeadingsWithWord(OutputFormat As outputType, targetFilename$)
  With word
    .StartOfDocument
    .EditFindClearFormatting
    Dim localHeadingLevel As Long, endtag$, direction&
    
    .EditFind "<h", "", 0
    While .EditFindFound
      .EditClear
      .EditBookmark "Hstart"
      .ExtendSelection
      .CharRight
      If localHeadingLevel = 0 Then localHeadingLevel = HeadingLevel
      direction = 1
      Select Case .Selection
        Case ">":
          .EditClear
          .EditBookmark "Hstart" & localHeadingLevel
          .EditFind "</h>"
          GoTo FindingHend
        Case "+": .EditClear: .EditBookmark "Hstart"
        Case "-": .EditClear: .EditBookmark "Hstart": direction = -1
        Case "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
                   .Cancel: .CharLeft: localHeadingLevel = 0
        Case Else: .Cancel: .CharLeft: .Insert "<h": GoTo NextHeader
      End Select
      .EditFind ">", "", 0
      If Not .EditFindFound Then .Insert "<h": Exit Sub
      .EditClear
      .ExtendSelection
      .EditGoTo "Hstart"
      
      'now we have selected the header number and deleted the <h> from around it
      If Len(.Selection) > 1 Then
        If MsgBox("Warning: suspicious header tag '<h" & .Selection & ">' " & vbCr _
                & "Found in '" & .WindowName & "'." & vbCr _
                & "Continue processing this section?", vbYesNo) = vbNo Then
          .Insert "</h" & .Selection & ">"
          Exit Sub
        End If
      End If
      If IsNumeric(.Selection) Then
        localHeadingLevel = localHeadingLevel + direction * CInt(.Selection)
      Else
        localHeadingLevel = localHeadingLevel + direction
      End If
      .EditClear
      .EditBookmark "Hstart" & localHeadingLevel
      .EditFind "</h" & localHeadingLevel & ">" 'find end tag of this header
FindingHend:
      If Not .EditFindFound Then Exit Sub
      .EditClear                           'delete </hx> tag
      .EditBookmark "Hend" & localHeadingLevel
      .ExtendSelection
      .EditGoTo "Hstart" & localHeadingLevel
      .Cancel
      HeadingText(localHeadingLevel) = Trim(.Selection)
      'If HeadingText(localHeadingLevel) = "Duration" Then Stop
      HeadingFile(localHeadingLevel) = SaveFilename
      .CharRight
      'look for icon
'      If IconLevel >= localHeadingLevel Then IconLevel = 999
'      .CharRight 4, 1
'      .EditFind "^g"
'      If .EditFindFound Then 'we may want to move icon before heading
'        Dim sizex$, sizey$
'        .FormatPicture
'        sizex = Word.CurValues.FormatPicture.sizex
'        sizey = Word.CurValues.FormatPicture.sizey
'        'if image is less than 1" square, then move it.
'        If val(Left(sizey, Len(sizey) - 1)) < 1 And _
'           val(Left(sizex, Len(sizex) - 1)) < 1 Then
'          .EditCut
'          IconLevel = localHeadingLevel
'
'          'look for a second icon
'          .CharRight 4, 1
'          .EditFind "^g"
'          If .EditFindFound Then 'we may want to move this one, too
'            .FormatPicture
'            sizex = Word.CurValues.FormatPicture.sizex
'            sizey = Word.CurValues.FormatPicture.sizey
'            'if image is less than 1" square, then move it.
'            If val(Left(sizey, Len(sizey) - 1)) < 1 And _
'               val(Left(sizex, Len(sizex) - 1)) < 1 Then
'               .CharLeft  'paste first icon found earlier
'               .EditPaste
'               .CharLeft
'               .CharRight 2, 1 'select both icons and cut them together
'               .EditCut
'            End If
'          Else
'            .CharLeft
'          End If
'        End If
'      Else
'        .CharLeft
'      End If
      
      .EditGoTo "Hend" & localHeadingLevel
      .ExtendSelection
      .EditGoTo "Hstart" & localHeadingLevel
      .Cancel
    
      If OutputFormat = tPRINT Then
        FormatHeadingPrint localHeadingLevel
      ElseIf OutputFormat = tHELP Then
        FormatHeadingHelp localHeadingLevel
      Else
        .Cancel
      End If
      LastHeadingLevel = localHeadingLevel
NextHeader:
      .CharRight
      .EditFind "<h", "", 0
    Wend
  End With
End Sub

Sub RemoveStuffOutsideBody()
 With word
  .StartOfDocument
  .EditReplace "<html>", "", ReplaceAll:=True
  .StartOfDocument
  .EditReplace "</html>", "", ReplaceAll:=True
  .StartOfDocument
  .EditFind "<head>", "", 0
  While .EditFindFound
    .ExtendSelection
    .EditFind "</head>"
    If .EditFindFound Then .EditClear
    .EditFind "<head>"
  Wend
  .StartOfDocument
  .EditReplace "<body>", "", ReplaceAll:=True
  .StartOfDocument
  .EditReplace "</body>", "", ReplaceAll:=True
 End With
End Sub

Sub TranslateButtons(OutputFormat As outputType)
  
  If Not CuteButtons Then Exit Sub
  
  Dim label$
  If OutputFormat = tHELP Or OutputFormat = tHTML Or OutputFormat = tHTMLHELP Then
    With word
      .StartOfDocument
      .EditFind "' button"
      While .EditFindFound
        .CharLeft
        .EditBookmark "LabelEnd"
        .EditFind "'", direction:=1
        .CharRight
        .ExtendSelection
        .EditGoTo "LabelEnd"
        label = .Selection
        If Len(label) < 20 Then
          .EditClear 2
          .CharLeft
          .EditClear
          If OutputFormat = tHELP Then
            .Insert "{button " & label & ",}"
          ElseIf OutputFormat = tHTML Or OutputFormat = tHTMLHELP Then
            .Insert "<input type=submit value=""" & label & """>"
          Else 'should not get here
            .Insert "'" & label & "'"
          End If
        Else
          Status "false alarm, not a button"
        End If
        .EditFind "' button", direction:=0
      Wend
      .EditFind PatternMatch:=0
    End With
  End If
End Sub

Sub TranslateLists(tag$, MarkerType%)
  Dim begintag As String, endtag As String, bulletNumber As Integer
  
  begintag = "<" & tag & ">"
  endtag = "</" & tag & ">"
  With word
    .StartOfDocument
    Status "Translating HTML <" & tag & ">"
    .EditFind endtag, direction:=0
    While .EditFindFound
      .EditClear
      .Insert vbCr
      .CharLeft
      .EditBookmark "ListEnd"
      .EditFind begintag, direction:=1
      If .EditFindFound Then
        '.Insert vbCr
        .EditClear
        WordRemoveTrailingWhitespace
        .EditBookmark "ListStart"
        .ExtendSelection
        .EditGoTo "ListEnd"
        .EditBookmark "WholeList"
        .Cancel
        bulletNumber = 1
'        .FormatBulletsAndNumbering Hang:=1, preset:=MarkerType
'        If tag = "ol" Then .FormatNumber StartAt:=bulletNumber: bulletNumber = bulletNumber + 1
'        .FormatParagraph LeftIndent:="0.5"""
        
        .EditFind "<li>", direction:=0
'        If .EditFindFound Then
'          .EditClear
'          WordRemoveTrailingWhitespace
'          .EditFind "<li>", direction:=0
'        End If
        While .EditFindFound
          .Insert vbCr
          WordRemoveTrailingWhitespace
          .FormatBulletsAndNumbering Hang:=1, preset:=MarkerType
          If tag = "ol" Then
            .FormatNumber StartAt:=bulletNumber
            bulletNumber = bulletNumber + 1
            .FormatParagraph LeftIndent:="0.5"""
          End If
          .EditGoTo "WholeList"
          .EditFind "<li>"
        Wend
'.ScreenUpdating 1
'        .EditGoTo "WholeList"
        .EditReplace "<p>", "<br><br>", ReplaceAll:=True
'        .EditFind "<p>"
'        While .EditFindFound
'          .CharLeft
'          .Insert vbCr
'          .SkipNumbering
'          .EditGoTo "WholeList"
'          .EditFind "<p>"
'        Wend
        
        
        .EditGoTo "ListEnd"
      End If
      .EditFind endtag
    Wend
  End With
End Sub

Private Sub WordRemoveTrailingWhitespace()
  Dim asci As Long
  With word
    .ExtendSelection
    .CharRight
    If Len(.Selection) > 0 Then asci = Asc(.Selection) Else asci = 33
    While asci < 33 'skip leading blanks and newlines
      .EditClear
      .CharRight
      If Len(.Selection) > 0 Then asci = Asc(.Selection) Else asci = 33
    Wend
    .CharLeft
  End With
End Sub

'Private Sub InsertHTMLskeleton(headText$)
'  '<form> allows buttons to be displayed as buttons in browser
'  With Word
'    .StartOfDocument
'    .Insert "<html>" & vbLf & "<head>" & headText & "</head>" & vbLf
'    .Insert "<body>" & vbLf
'    If CuteButtons Then .Insert "<form>" & vbLf
'    .EndOfDocument 'in case there is already some text in the file
'    If CuteButtons Then .Insert vbLf & "</form>"
'    .Insert vbLf & "</body></html>"
'    .StartOfDocument
'    If CuteButtons Then .LineDown 3 Else .LineDown 2
'  End With
'End Sub

Private Function AlinkAnchor(keyword As String) As String
  AlinkAnchor = "<Object type=""application/x-oleobject"" classid=""clsid:1e2a7bd0-dab9-11d0-b93a-00c04fc99f9e"">" & vbCrLf _
              & "    <param name=""ALink Name"" value=""" & keyword & """>" & vbCrLf _
              & "</Object>" & vbCrLf
End Function

Private Function KeywordAnchor(keyword As String) As String
  KeywordAnchor = "<Object type=""application/x-oleobject"" classid=""clsid:1e2a7bd0-dab9-11d0-b93a-00c04fc99f9e"">" & vbCrLf _
                & "    <param name=""Keyword"" value=""" & keyword & """>" & vbCrLf _
                & "</Object>" & vbCrLf
End Function

Private Function KeywordButton(keyword As String) As String
  KeywordButton = "<Object id=hhctrl type=""application/x-oleobject""" & vbCrLf _
                   & "classid=""clsid:adb880a6-d8ff-11cf-9377-00aa003b7a11""" & vbCrLf _
                   & "codebase=""hhctrl.ocx#Version=4,74,8702,0"" Width = 100 Height = 100>" & vbCrLf _
                   & "<param name=""Command"" value=""KLink"">" & vbCrLf _
                   & "<param name=""Button"" value=""Text:" & keyword & """>" & vbCrLf _
                   & "<param name=""Item1"" value="""">" & vbCrLf _
                   & "<param name=""Item2"" value=""" & keyword & """>" & vbCrLf & "</OBJECT>" & vbCrLf

End Function

Private Sub FormatKeywordsHTMLHelp()
  Dim startPos As Long, endPos As Long, keyword As String, KeywordPreamble As String
  startPos = InStr(LCase(TargetText), "<keyword=")
  If startPos > 0 Then
    KeywordPreamble = "<p>" & vbCrLf & "Keywords:"
    TargetText = Left(TargetText, startPos - 1) & KeywordPreamble & Mid(TargetText, startPos)
    startPos = startPos + Len(KeywordPreamble)
  End If
  While startPos > 0
    endPos = InStr(startPos, TargetText, ">")
    If endPos > 0 Then
      keyword = TrimQuotes(Trim(Mid(TargetText, startPos + 9, endPos - startPos - 9)))
      TargetText = Left(TargetText, startPos - 1) & KeywordAnchor(keyword) & "&nbsp;" & KeywordButton(keyword) & Mid(TargetText, endPos + 1)
    End If
    startPos = InStr(endPos, LCase(TargetText), "<keyword=")
  Wend

  startPos = InStr(LCase(TargetText), "<indexword=")
  While startPos > 0
    endPos = InStr(startPos, TargetText, ">")
    If endPos > 0 Then
      keyword = TrimQuotes(Trim(Mid(TargetText, startPos + 11, endPos - startPos - 11)))
      TargetText = Left(TargetText, startPos - 1) & KeywordAnchor(keyword) & Mid(TargetText, endPos + 1)
    End If
    startPos = InStr(endPos, LCase(TargetText), "<indexword=")
  Wend

End Sub

'Private Sub FormatKeywordsHTMLHelpUsingWord()
'  Dim keyword$
'  With Word
'    .StartOfDocument
'    .EditFind "<Keyword=", "", 0
'    While .EditFindFound
'      .EditClear
'      .EditBookmark "LinkStart"
'      .EditFind ">"
'      If Not .EditFindFound Then Exit Sub
'      .EditClear
'      .ExtendSelection
'      .EditGoTo "LinkStart"
'      keyword = Trim(.Selection)
'      .EditClear
'      .Cancel
'      .Insert "<Object type=""application/x-oleobject"" classid=""clsid:1e2a7bd0-dab9-11d0-b93a-00c04fc99f9e"">" & vbCrLf
'      .Insert "    <param name=""Keyword"" value=""" & keyword & """>" & vbCrLf
'      .Insert "</Object>"
'      'Print #IDfile, "<p>"
'      .Insert vbCrLf _
'            & "<Object id=hhctrl type=""application/x-oleobject""" & vbCrLf _
'            & "classid=""clsid:adb880a6-d8ff-11cf-9377-00aa003b7a11""" & vbCrLf _
'            & "codebase=""hhctrl.ocx#Version=4,74,8702,0"" Width = 100 Height = 100>" & vbCrLf _
'            & "<param name=""Command"" value=""KLink"">" & vbCrLf _
'            & "<param name=""Button"" value=""Text:" & keyword & """>" & vbCrLf _
'            & "<param name=""Item1"" value="""">" & vbCrLf _
'            & "<param name=""Item2"" value=""" & keyword & """>" & vbCrLf & "</OBJECT>" & vbCrLf
'
'      .Insert "<p>"
'      .EditFind "<Keyword="
'    Wend
'  End With
'End Sub

Private Sub NumberHeaderTags()
  If OutputFormat = tPRINT Or OutputFormat = tHELP Then
    NumberHeaderTagsWithWord
  Else
    Dim curTag$
    Dim selStr$
    Dim localHeadingLevel&, direction&, distance&
    Dim startPos As Long, endPos As Long, hedr As String
    Dim IgnoreUnknown As Boolean
    
    startPos = InStr(LCase(TargetText), "<h")
    If startPos = 0 Then 'need to insert section header
      TargetText = "<h" & HeadingLevel & ">" & HeadingWord(HeadingLevel) & "</h" & HeadingLevel & "> " & vbLf & TargetText
    End If
    curTag = "<h"
    startPos = InStr(LCase(TargetText), curTag)
    localHeadingLevel = HeadingLevel
    While startPos > 0
      endPos = InStr(startPos, TargetText, ">")
      If endPos > 0 Then
        selStr = Mid(TargetText, startPos + Len(curTag), endPos - startPos - Len(curTag))
        direction = 0
        If selStr = "" Then
          TargetText = Left(TargetText, startPos - 1) & curTag & localHeadingLevel & Mid(TargetText, endPos)
        Else
          Select Case Left(selStr, 1)
            Case "+", "-":
              If Left(selStr, 1) = "+" Then direction = 1 Else direction = -1
              selStr = Mid(selStr, 2)
              If IsNumeric(selStr) Then
                localHeadingLevel = HeadingLevel + direction * CInt(selStr)
              Else
                localHeadingLevel = HeadingLevel + direction
              End If
              TargetText = Left(TargetText, startPos + 1) & localHeadingLevel & Mid(TargetText, endPos)
            Case Else:
              If IsNumeric(selStr) Then
                localHeadingLevel = CInt(selStr)
              ElseIf UCase(selStr) = "EAD" Or UCase(selStr) = "TML" Then
                'ignore <html> and <head> even though these should not be in source files
              ElseIf UCase(Left(selStr, 1)) = "R" Then
                'ignore <hr> <hr size=7> etc.
              Else
                If Not IgnoreUnknown Then
                  If MsgBox("Unknown heading tag '<h" & selStr & ">'" & vbCr _
                          & "In file " & SourceFilename & vbCr _
                          & "Warn about future unknown headers?", _
                          vbYesNo, "Number Header Tags") = vbNo Then
                    IgnoreUnknown = True
                  End If
                End If
              End If
          End Select
        End If
        If curTag = "<h" Then curTag = "</h" Else curTag = "<h"
        startPos = InStr(endPos, LCase(TargetText), curTag)
      End If
    Wend
  End If
End Sub

Private Sub NumberHeaderTagsWithWord()
  Dim curTag$
  Dim selStr$
  Dim localHeadingLevel&, direction&, distance&
  With word
    .EditGoTo "CurrentFileStart"
    .EditFind "<h", "", 0
    If Not .EditFindFound Then 'need to insert section header
      .EditGoTo "CurrentFileStart"
      .Insert "<h" & HeadingLevel & ">" & HeadingWord(HeadingLevel) & "</h" & HeadingLevel & "> " & vbLf
    End If
    curTag = "<h"
    .EditGoTo "CurrentFileStart"
    .EditFind curTag, "", 0
    While .EditFindFound
      .CharRight
      .ExtendSelection
      .EditFind ">", "", 0
      localHeadingLevel = HeadingLevel
      If .EditFindFound Then
        selStr = Left(.Selection, Len(.Selection) - 1)
        .CharLeft
        direction = 0
        If selStr = "" Then
          .Cancel
          .Insert localHeadingLevel
        Else
          Select Case Left(selStr, 1)
            Case "+", "-":
              .EditClear
              If Left(selStr, 1) = "+" Then direction = 1 Else direction = -1
              selStr = Mid(selStr, 2)
              If IsNumeric(selStr) Then
                localHeadingLevel = HeadingLevel + direction * CInt(selStr)
              Else
                localHeadingLevel = HeadingLevel + direction
              End If
              .Insert localHeadingLevel
            Case "r", "R": curTag = "</h" 'ignore <hr>
            Case Else:
              If IsNumeric(selStr) Then
                localHeadingLevel = CInt(selStr)
              Else
                .StartOfLine
                .ExtendSelection
                .EndOfLine
                MsgBox "Suspicious heading " & selStr
                Stop
              End If
          End Select
          .Cancel
        End If
        If curTag = "<h" Then curTag = "</h" Else curTag = "<h"
        .EditFind curTag, "", 0
      End If
    Wend
  End With
End Sub

Private Sub TranslateIMGtags(sourcefile$)
  Dim LinkFilename$, path$
  path = sourcefile
  While path <> "" And Right(path, 1) <> "\"
    path = Left(path, Len(path) - 1)
  Wend
  With word
    .EditFind "<IMG SRC=""", "", 0
    While .EditFindFound
      Dim InsertParagraphs As Boolean, curpath$, curfilename$, LinkToThisImageFile%
      InsertParagraphs = InsertParagraphsAroundImages
      LinkToThisImageFile = LinkToImageFiles
      .EditClear
      .EditBookmark "LinkStart"
      .EditFind """"
      If Not .EditFindFound Then Exit Sub
      .EditClear
      .ExtendSelection
      .EditGoTo "LinkStart"
      'curpath = path
      curfilename = .Selection
      .EditClear
      LinkFilename = PathNameOnly(ProjectFileName) & "\" & path & curfilename
      'While Left(curfilename, 2) = ".."
      '  curfilename = Right(curfilename, Len(curfilename) - 3)
      '  curpath = Left(curpath, Len(curpath) - 1)
      '  While Len(curpath) > 0 And Right(curpath, 1) <> "\"
      '    curpath = Left(curpath, Len(curpath) - 1)
      '  Wend
      'Wend
      'LinkFilename = curpath & curfilename
      .ExtendSelection
      .EditFind ">"
      If InStr(1, LinkFilename, "icon", 1) > 0 Then
        InsertParagraphs = False
        'LinkToThisImageFile = 0
      End If
      If Len(.Selection) > 1 Then InsertParagraphs = False 'probably said ALIGN=LEFT in img tag
      .EditClear
      .Cancel
      If InsertParagraphs Then .Insert "<p>"
      .InsertPicture LinkFilename, LinkToThisImageFile
      
      '.Insert "{ INCLUDEPICTURE """ & LinkFilename & """ \* MERGEFORMAT \D }"
      '.CharLeft 1, 1
      
      'Dim sizexS$, sizeyS$, sizex!, sizey!
      '.FormatPicture
      '.FormatPicture ScaleX:="100%", ScaleY:="100%"
      'sizexS = Word.CurValues.FormatPicture.sizex
      'sizex = Val(Left(sizexS, Len(sizexS) - 1))
      'sizeyS = Word.CurValues.FormatPicture.sizey
      'sizey = Val(Left(sizeyS, Len(sizeyS) - 1))
      
      'If sizex > 6.5 Then
      '  Dim percent$
      '  percent = (100# * 6.5 / sizex) & "%"
      '  .FormatPicture ScaleX:=percent, ScaleY:=percent
      'End If
      '.CharRight
      
      If InStr(1, LinkFilename, "icon", 1) > 0 Then
        .CharLeft 1, 1
        .FormatFont Position:=-12 'half-point units
        .CharRight
      End If
      
      If InsertParagraphs Then .Insert "<p>"
      .EditFind "<IMG SRC="""
    Wend
  End With
End Sub

Public Sub FormatHeadingHTML(thisHeadingLevel%, targetFilename$, ByRef thisHeadingStart As Long)
  Static LinkToFirstHeader$
  Dim tmp As String
  If MoveHeadings <> 0 Then
    Dim hn$
    hn = "h" & (thisHeadingLevel + MoveHeadings) & ">"
    TargetText = Left(TargetText, thisHeadingStart) & "<" & hn & HeadingText(thisHeadingLevel) & "</" & hn & Mid(TargetText, thisHeadingStart + 1)
  Else
    'Dim IconPath$
    Dim ht As String 'HeadingText
    Dim TextToInsert As String, TextToPrepend As String, TextToAppend As String
    ht = HeadingText(thisHeadingLevel)
    TextToInsert = ""
    TextToPrepend = ""
    TextToAppend = ReplaceString(FooterStyle(thisHeadingLevel), "<sectionname>", ht)
'    IconPath = ""
'    For i = IconLevel + 1 To thisHeadingLevel
'      IconPath = "../" & IconPath
'    Next i

    'Insert name anchor around heading
    TextToInsert = TextToInsert & "<a name=""" & ht & """>" _
                 & ReplaceString(HeaderStyle(thisHeadingLevel), "<sectionname>", ht) & "</a>" & vbLf
'    If IconLevel <= thisHeadingLevel Then
'      TextToInsert = TextToInsert & "<img src=""" & IconPath & IconFilename & """ align=right>"
'    End If
    
    If FirstHeaderInFile Then  'Insert navigation to parents in hierarchy
      FirstHeaderInFile = False
      If UpNext Then
        LinkToFirstHeader = "Up to: <a href=""#" & ht & """>" & ht & "</a>" & vbLf
        Dim h%, dirstep%, ParentHT$
        If thisHeadingLevel > 1 Or (OutputFormat = tHTML And BuildContents) Then
          TextToInsert = TextToInsert & "Up to: "
          For h = thisHeadingLevel - 1 To 1 Step -1
            ParentHT = HeadingText(h)
            TextToInsert = TextToInsert & "<a href=""\" & HeadingFile(h) & "#" & ParentHT & """>" & ParentHT & "</a>, "
          Next h
          If OutputFormat = tHTML And BuildContents Then
            TextToInsert = TextToInsert & "<a href=""\Contents.html#" & ht & """>Contents</a>"
          Else 'remove last ", "
            TextToInsert = Left(TextToInsert, Len(TextToInsert) - 2)
          End If
          TextToInsert = TextToInsert & "<p>" & vbLf
        End If
      End If
      TextToPrepend = "<html><head><title>" & ht & "</title></head>" & vbCrLf & BodyTag & vbCrLf '& "<form>" & vbCrLf
      TextToAppend = vbCrLf & "</body>" & vbCrLf & "</html>" & vbCrLf
      
      If OutputFormat = tHTMLHELP Then
        If InStr(TargetText, "<param name=""Keyword"" value=""" & ht & """>") = 0 Then
          TextToAppend = KeywordAnchor(ht) & TextToAppend
        End If
        If thisHeadingLevel = 5 Then
          If Right(HeadingText(3), 5) = "Block" Then
            TextToAppend = AlinkAnchor(ht & Left(HeadingText(3), Len(HeadingText(3)) - 6)) & TextToAppend
          End If
        End If
      End If
    End If
    TargetText = TextToPrepend & Left(TargetText, thisHeadingStart) & TextToInsert & Mid(TargetText, thisHeadingStart + 1) & TextToAppend
    thisHeadingStart = thisHeadingStart + Len(TextToPrepend) + Len(TextToInsert)
    
    If BuildContents Then HTMLContentsEntry thisHeadingLevel, targetFilename, ht
  
  End If
End Sub

Private Sub HTMLContentsEntry(thisHeadingLevel%, targetFilename$, headerText$)
  Dim lvl&, id$, SafeFilename As String
  '.Activate ContentsWin
  For lvl = LastHeadingLevel + 1 To thisHeadingLevel
    Print #HTMLContentsfile, Space((lvl - 1) * 4) & "<ul>"
  Next lvl
  For lvl = thisHeadingLevel + 1 To LastHeadingLevel
    Print #HTMLContentsfile, Space((lvl - 1) * 4) & "</ul>"
  Next lvl
  Print #HTMLContentsfile, "<li>"
  
  If OutputFormat = tHTML Then
    SafeFilename = ReplaceString(targetFilename, "\", "/")
    SafeFilename = ReplaceString(SafeFilename, " ", "%20")
    Print #HTMLContentsfile, Space((thisHeadingLevel - 1) * 4) & "<a name=""" & headerText & """>"
    Print #HTMLContentsfile, Space((thisHeadingLevel - 1) * 4) & "<a href=""" & SafeFilename & "#" & headerText & """>" & headerText & "</a></a>"
  ElseIf OutputFormat = tHTMLHELP Then
    Dim objdef As String
    objdef = Space((thisHeadingLevel - 1) * 4) & "<li><OBJECT type=""text/sitemap"">" & vbCr
    objdef = objdef & Space((thisHeadingLevel) * 4) & "<param name=""Name"" value=""" & headerText & """>" & vbCr
    objdef = objdef & Space((thisHeadingLevel) * 4) & "<param name=""Local"" value=""" & targetFilename & """>" & vbCr
    objdef = objdef & Space((thisHeadingLevel) * 4) & "</OBJECT>" & vbCr
    Print #HTMLContentsfile, objdef
    Print #HTMLIndexfile, objdef
    
    id = MakeValidHelpID(headerText)
    
    If BuildID Then
      Print #IDfile, "#define " & id & vbTab & IDnum
      IDnum = IDnum + 1
    End If
    If BuildProject Then
      AliasSection = AliasSection & vbLf & id & " = " & targetFilename
    End If
  End If
  'If IconLevel = thisHeadingLevel Then
  '  Dim slashpos%, IconPath$, i&
  '  IconPath = ""
  '  slashpos = 0
  '  For i = 1 To IconLevel - 1
  '    slashpos = InStr(slashpos + 1, targetFilename, "\")
  '  Next i
  '  If slashpos > 0 Then IconPath = Mid(targetFilename, 1, slashpos)
  '
  '  Print #HTMLContentsfile, Space((thisHeadingLevel) * 4) & "<img src=""" & IconPath & IconFilename & """>"
  'End If
  '.Activate SourceWin
End Sub

Public Sub FormatHeadingPrint(thisHeadingLevel As Long)
'  With Word
'  Dim styleName$, ht$
  Dim cmd As Variant
'  styleName = "ADheading" & thisHeadingLevel
'  ht = HeadingText(thisHeadingLevel)
  With word
    .EditBookmark "Hall" & thisHeadingLevel
    .CharRight
    .EditBookmark "Hend" & thisHeadingLevel
    .Insert vbCr & vbCr
    .EditGoTo "Hall" & thisHeadingLevel
    .CharLeft
    .Insert vbCr
    .ExtendSelection
    .EditGoTo "Hend" & thisHeadingLevel
    .Style "ADheading" & thisHeadingLevel
    .Cancel
  End With
  For Each cmd In WordStyle(thisHeadingLevel)
    WordCommand cmd, thisHeadingLevel
    'If Left(LCase(cmd), 11) = "insertbreak" Then GoSub ApplyStyle
  Next
  
'  If thisHeadingLevel = 4 Then
'    .InsertBreak 2
'    GoSub ApplyStyle
'    If MakeBoxyHeaders Then
'      ViewHeaderAndSet ht & vbTab & vbTab & ht
'      .FormatTabs ClearAll:=1
'      .FormatTabs "3.25""", Align:=1, Set:=1 'Center text in center of pg
'      .FormatTabs "6.5""", Align:=2, Set:=1 'flush right at right margin
'      .BorderLineStyle 8    'make double-line border around header
'      .BorderOutside 1
'      'If IconLevel <= thisHeadingLevel Then
'      '  .StartOfLine
'      '  .EditPaste
'      '  .Insert "   "
'      'End If
'      .ViewNormal
'    End If
'  ElseIf thisHeadingLevel = 5 Then
'    .InsertBreak 2
'    GoSub ApplyStyle
'    If MakeBoxyHeaders Then
'      ViewHeaderAndSet HeadingText(4) & vbTab & ht & vbTab & HeadingText(4)
'      'If IconLevel <= thisHeadingLevel Then
'      '  .StartOfLine
'      '  .EditPaste
'      '  .Insert "   "
'      'End If
'      .BorderLineStyle 8    'make double-line border around header
'      .BorderOutside 1
'      .ViewNormal
'    End If
'  ElseIf thisHeadingLevel > 5 Then
'    .InsertBreak 3
'    GoSub ApplyStyle
'    'If IconLevel = thisHeadingLevel Then
'    '  .EditGoTo "Hstart" & thisHeadingLevel
'    '  .EditPaste
'    '  .Insert "   "
'    'End If
'  Else 'If thisHeadingLevel < 4 Then
'    .InsertBreak 2
'    GoSub ApplyStyle
'    'If IconLevel <= thisHeadingLevel Then
'    '  .EditGoTo "Hstart" & thisHeadingLevel
'    '  .EditPaste
'    '  .Insert "   "
'    'End If
'    ViewHeaderAndSet ""
'    .BorderNone 1
'    .GoToHeaderFooter
'    GoSub PageNumber
'  End If
' End With
 Exit Sub

'ApplyStyle:
'  With Word
'    .EditBookmark "Hstart" & thisHeadingLevel
'    .EditGoTo "Hend" & thisHeadingLevel
'    .Insert vbCr & vbCr
'    .EditGoTo "Hstart" & thisHeadingLevel
'    .ExtendSelection
'    .EditGoTo "Hend" & thisHeadingLevel
'    .Style StyleName
'    .Cancel
'    .CharRight
'    Return
'  End With

'PageNumber:
' With Word
'  On Error Resume Next
'  .ToggleHeaderFooterLink
'  On Error GoTo 0
'  .EditSelectAll
'  .CenterPara
'  .Insert ht
'  If FooterTimestamps Then .InsertDateTime "   hh:mm MMMM d, yyyy", InsertAsField:=0
'  .ViewNormal
'  .InsertPageNumbers 1, 4, 1
'  .ViewFooter
'  .EditSelectAll
'  .FormatFont 9, Bold:=1
'  .ViewNormal
'  Return
' End With

End Sub

Private Sub ViewHeaderAndSet(s$)
  With word
    If Len(s) > 0 Then
      .FilePageSetup TopMargin:="1.5""", HeaderDistance:="1""", ApplyPropsTo:=0
    Else
      .FilePageSetup TopMargin:="1""", HeaderDistance:="0.5""", ApplyPropsTo:=0
    End If
    .ViewHeader
    If NotFirstPrintHeader Then
      On Error GoTo toggle
      .FormatHeaderFooterLink
      On Error GoTo 0
    Else
      NotFirstPrintHeader = True
    End If
    .EditSelectAll
    .EditClear
    .Insert s
  End With
  Exit Sub
toggle:
  On Error GoTo foo
  word.ToggleHeaderFooterLink
  Resume Next
foo:
  Resume Next
End Sub

Private Sub ViewFooterAndSet(s$)
  With word
    .ViewFooter
    If NotFirstPrintFooter Then
      On Error GoTo toggle
      .FormatHeaderFooterLink
      On Error GoTo 0
    Else
      NotFirstPrintFooter = True
    End If
    .EditSelectAll
    .EditClear
    .Insert s
  End With
  Exit Sub
toggle:
  On Error GoTo foo
  word.ToggleHeaderFooterLink
  Resume Next
foo:
  Resume Next
End Sub

Sub DefinePrintStyles()
  With word
    Dim level As Long
    For level = 1 To maxLevels
      .FormatStyle "ADheading" & level
    Next
    .FormatStyle "Normal"
'    .ViewHeader
'    .Insert " "
'    .ViewNormal
'    'Stop
'    .ToolsOptionsGeneral Units:=0
'    .FilePageSetup TopMargin:="1""", BottomMargin:="1""", LeftMargin:="1""", RightMargin:="1""", HeaderDistance:="0.5""", ApplyPropsTo:=4
'
'    .FormatStyle "Normal", AddToTemplate:=1, Define:=1
'    .FormatDefineStyleFont 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 1, 1, "Times New Roman", 0, 0, 0, 0
'    .FormatDefineStylePara Chr$(34), Chr$(34), 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, Chr$(34)
'    .FormatDefineStyleLang "English (US)", 1
'    .FormatDefineStyleBorders 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1
'
'    .FormatStyle "heading1", BasedOn:="Normal", AddToTemplate:=1, Define:=1
'    .FormatDefineStyleFont 18, Kerning:=1, KerningMin:=14, Bold:=1
'    .FormatDefineStylePara Before:=12, After:=9, KeepWithNext:=1
'    .FormatDefineStyleBorders TopBorder:=2, BottomBorder:=2, HorizColor:=1
'
'    .FormatStyle "heading2", BasedOn:="Normal", AddToTemplate:=1, Define:=1
'    .FormatDefineStyleFont 16, Bold:=1
'    .FormatDefineStylePara Before:=12, After:=3, KeepWithNext:=1
'
'    .FormatStyle "heading3", BasedOn:="Normal", AddToTemplate:=1, Define:=1
'    .FormatDefineStyleFont 14, Underline:=1, Bold:=1
'    .FormatDefineStylePara Before:=12, After:=3, KeepWithNext:=1
'
'    .FormatStyle "heading4", BasedOn:="Normal", AddToTemplate:=1, Define:=1
'    .FormatDefineStyleFont 12
'    .FormatDefineStylePara Before:=12, After:=3, KeepWithNext:=1
'
'    .FormatStyle "heading5", BasedOn:="Normal", AddToTemplate:=1, Define:=1
'    .FormatDefineStyleFont 12
'    .FormatDefineStylePara Before:=12, After:=3, KeepWithNext:=1
'
'    .FormatStyle "heading6", BasedOn:="Normal", AddToTemplate:=1, Define:=1
'    .FormatDefineStyleFont 12
'    .FormatDefineStylePara Before:=12, After:=3, KeepWithNext:=1
'
'    .FormatStyle "heading7", BasedOn:="Normal", AddToTemplate:=1, Define:=1
'    .FormatDefineStyleFont 12
'    .FormatDefineStylePara Before:=12, After:=3, KeepWithNext:=1
  End With
End Sub

Private Function TrimQuotes(str$) As String
  Dim retval As String
  retval = Trim(str)
  If Left(retval, 1) = """" Then retval = Mid(retval, 2)
  If Right(retval, 1) = """" Then retval = Left(retval, Len(retval) - 1)
  TrimQuotes = retval
End Function

Sub HTMLQuotedCharsToPrint()
  With word
   .EditReplace "<p>", "^p", ReplaceAll:=True
   .StartOfDocument
   .EditFind "<", "", 0
   While .EditFindFound
     .EditClear
     .EditBookmark "TagStart"
     .EditFind ">"
     If Not .EditFindFound Then Exit Sub
     .EditClear
     .ExtendSelection
     .EditGoTo "TagStart"
     .EditClear
     .Cancel
     .EditFind "<"
   Wend
   .StartOfDocument
   .EditReplace "&lt;", "<", ReplaceAll:=True
   .StartOfDocument
   .EditReplace "&gt;", ">", ReplaceAll:=True
   .StartOfDocument
   .EditReplace "&amp;", "&", ReplaceAll:=True
   .StartOfDocument
   .EditReplace "&quot;", """", ReplaceAll:=True
  End With
End Sub

Sub HelpFootnotes(topic$, id$, Keywords$)
  With word
    .InsertFootnote "#", 1
    .Insert id
    .OtherPane
    .InsertFootnote "$", 1
    .Insert topic
    .OtherPane
    .InsertFootnote "K", 1
    .Insert Keywords
    .OtherPane
    .InsertFootnote "+", 1
    .Insert "auto"
    .ClosePane
  End With
End Sub

Public Sub FormatHeadingHelp(thisHeadingLevel As Long)
 Dim topic$
 Dim id$, h%
 topic = HeadingText(thisHeadingLevel)
' If topic = "Graph" Then Stop
 id = MakeValidHelpID(topic)
 With word
  .Bold
  .FontSize .FontSize + 4
  .Cancel
  .CharLeft
  .Insert vbCr
  .InsertBreak 0
  HelpFootnotes topic, id, topic
  .EditBookmark "Hstart" & thisHeadingLevel
  'If IconLevel <= thisHeadingLevel Then
  '  .EditPaste
  '  .Insert "   "
  'End If
  .EditGoTo "Hend" & thisHeadingLevel
  .Insert vbCr & vbCr
  .CharLeft 2
  MakeNonscrollingHereBackToPageBreak
  .CharRight
  .EditClear
  .CharRight
  If BuildContents Then HelpContentsEntry topic, id, thisHeadingLevel
  If BuildID Then
    Print #IDfile, "#define " & id & vbTab & IDnum
    IDnum = IDnum + 1
  End If
  On Error GoTo NoPrevSection
  .EditBookmark "temp"
  If UpNext Then
    If LastHeadingLevel >= thisHeadingLevel Then
      .EditGoTo "UpFrom" & LastHeadingLevel
      .Insert vbTab & "Next: "
      HelpHyperlink topic, id
    End If
    For h = thisHeadingLevel To LastHeadingLevel - 1
      .EditGoTo "UpFrom" & h
      .Insert vbTab & "Next: "
      HelpHyperlink topic, id
    Next h
  End If
NoPrevSection:
  .EditGoTo "temp"
  On Error GoTo 0
  
  If thisHeadingLevel > 1 Then  'Insert navigation to/from parents in hierarchy
    If UpNext Then
      Dim parentTopic$
      .Insert "Up to: "
      For h = thisHeadingLevel - 1 To 1 Step -1
        parentTopic = HeadingText(h)
        HelpHyperlink parentTopic, MakeValidHelpID(parentTopic)
        If h > 1 Then .Insert ", "
      Next h
      .Insert vbCr
      .EditBookmark "SectionContents" & thisHeadingLevel
      .CharLeft
      .EditBookmark "UpFrom" & thisHeadingLevel
      
      'insert entry in section contents of parent topic
      If HeadingText(thisHeadingLevel - 1) <> "Tutorial" Then
        ContentsEntries(thisHeadingLevel - 1) = ContentsEntries(thisHeadingLevel - 1) + 1
        .EditGoTo "SectionContents" & thisHeadingLevel - 1
        If ContentsEntries(thisHeadingLevel - 1) = 1 Then
          .Insert "In This Section:" & vbCr
          .CharLeft
        End If
        .Insert Chr$(11) & vbTab
        HelpHyperlink topic, id
        .EditBookmark "SectionContents" & thisHeadingLevel - 1
        .EditGoTo "SectionContents" & thisHeadingLevel
      End If
    End If
  Else
    .EditBookmark "UpFrom" & thisHeadingLevel
    .Insert vbCr
    .EditBookmark "SectionContents" & thisHeadingLevel
  End If
  ContentsEntries(thisHeadingLevel) = 0
 End With
End Sub

Sub FinishHTMLHelpContents()
  Dim lvl&
  For lvl = HeadingLevel To 1 Step -1
    Print #HTMLContentsfile, Space((lvl - 1) * 4) & "</ul>"
  Next lvl
  Print #HTMLContentsfile, "</body>"
  Print #HTMLContentsfile, "</html>"
  Close HTMLContentsfile
  
  If HTMLIndexfile >= 0 Then
    Print #HTMLIndexfile, "</ul>"
    Print #HTMLIndexfile, "</body>"
    Print #HTMLIndexfile, "</html>"
    Close HTMLIndexfile
  End If
End Sub

Sub MakeNonscrollingHereBackToPageBreak()
  'Make non-scrolling region at top of help topic
  With word
'        .EditBookmark "NonscrollEnd"
    .ExtendSelection
    .EditFind "^m", "", 1
    .LineDown
    .StartOfLine
    .ParaKeepWithNext
    .Cancel
    .CharRight
  End With
End Sub

Sub HelpHyperlink(label, target)
  With word
  'DebugMsg "label: " & label & ", target: " & target
    .Insert "   "
    .CharLeft 2
    .EditBookmark "link1"
    .DoubleUnderline 1
    .Insert label
    .CharRight
    .EditBookmark "link2"
    .DoubleUnderline 0
    .Hidden 1
    .Insert target
    .CharRight
    .Hidden 0
    .EditBookmark "endlink"
    .EditGoTo "link1"
    .EditClear -1
    .EditGoTo "link2"
    .EditClear -1
    .EditGoTo "endlink"
  End With
End Sub

Public Function MakeValidHelpID(id$) As String
  Dim a&, retval$, i&, ch$, lastReplaced As Boolean
  retval = ""
  lastReplaced = True
  For i = 1 To Len(id)
    ch = Mid(id, i, 1)
    a = Asc(ch)
    Select Case a
      Case 65 To 90, 97 To 122, 47 'in range A-Z a-z or /
        retval = retval & ch
        lastReplaced = False
      Case Else
        If lastReplaced = False Then
          retval = retval & "_"
          lastReplaced = True
        End If
      End Select
  Next i
  MakeValidHelpID = retval
End Function

Public Function MakeValidFilename(ByVal id$) As String
  Dim a&, retval$, i&, ch$, lastReplaced As Boolean
  retval = ""
  id = Trim(id)
  lastReplaced = True ' Don't replace multiple illegal chars in a row with underscore
  For i = 1 To Len(id)
    ch = Mid(id, i, 1)
    a = Asc(ch)
    Select Case a
      Case 32, 33, 35 To 41, 43 To 46, 48 To 57, 65 To 90, 94, 95, 97 To 122
        retval = retval & ch
        lastReplaced = False
      Case 0 - 31
        'Omit control characters and don't insert underscores for them
      Case Else
        If lastReplaced = False Then
          retval = retval & "_"
          lastReplaced = True
        End If
    End Select
  Next i
  MakeValidFilename = retval
End Function

Sub HelpContentsEntry(topic$, id$, thisHeadingLevel As Long)
  With word
    .Activate ContentsWin
    If LastHeadingLevel = 0 Then LastHeadingLevel = 1
    .Insert thisHeadingLevel & " " & topic & "=" & id & vbCr
    If thisHeadingLevel < LastHeadingLevel Then BookLevel = thisHeadingLevel
    If thisHeadingLevel > LastHeadingLevel And BookLevel < LastHeadingLevel Or BookLevel = thisHeadingLevel Then
      Dim numlines%, tmpstr$
      If thisHeadingLevel > LastHeadingLevel Then
        numlines = 2
        BookLevel = LastHeadingLevel
      Else
        numlines = 1
      End If
      .LineUp numlines
      .EditFind "=", "", 0
      .CharLeft
      .StartOfLine 1
      .CharRight 1, 1
      
      tmpstr = .Selection
      .Cancel
      .CharLeft
      .Insert tmpstr
      'Before copy buffer was used for icons, the last 4 lines were:
      '.EditCopy
      '.Cancel
      '.CharLeft
      '.EditPaste
      
      .Insert vbCr & thisHeadingLevel
      .LineDown numlines
    End If
    'Dim lvl%
    'For lvl = LastHeadingLevel + 1 To thisHeadingLevel - 1
    '  .Insert lvl & " missing level" & vbCr
    'Next lvl
    .Activate TargetWin
  End With
End Sub

Private Sub HREFsToHelpHyperlinks()
  Dim LinkRef$, LinkLabel$, topic$, id$, pos
  With word
    .StartOfDocument
    Status "Translating HTML links to Help Hyperlinks"
    .EditFind "<A HREF=""", "", 0
    While .EditFindFound
      'set LinkRef$
      .EditClear
      .EditBookmark "LinkStart"
      .EditFind """>"
      If Not .EditFindFound Then Exit Sub
      .EditClear
      .ExtendSelection
      .EditGoTo "LinkStart"
      LinkRef = .Selection
      .EditClear
      .Cancel
      
      'set LinkLabel$
      .EditBookmark "LinkStart"
      .EditFind "</A>"
      If Not .EditFindFound Then Exit Sub
      .EditClear
      .ExtendSelection
      .EditGoTo "LinkStart"
      LinkLabel = .Selection
      .EditClear
      .Cancel
      
      pos = InStr(1, LinkRef, "#")
      If IsNull(pos) Then
        Status "Warning: Link '" & LinkLabel & " -> " & LinkRef & "' does not contain valid help topic."
      ElseIf pos = 0 Then
        Status "Warning: Link '" & LinkLabel & " -> " & LinkRef & "' does not contain valid help topic."
      Else
        topic = Mid(LinkRef, pos + 1)
        id = MakeValidHelpID(topic)
        HelpHyperlink LinkLabel, id
      End If
      
      .EditFind "<A HREF="""
    Wend
  End With
End Sub

Private Sub AbsoluteToRelative()
  Dim startTag As Long, endtag As Long, LevelCount As Long, FilePath As String
  startTag = InStr(TargetText, "=""/")
  While startTag > 0
    FilePath = ""
    For LevelCount = 1 To HeadingLevel - 1
      FilePath = FilePath & "../"
    Next
    TargetText = Left(TargetText, startTag + 1) & FilePath & Mid(TargetText, startTag + 3)
    startTag = InStr(TargetText, "=""/")
  Wend
End Sub

Private Sub MakeLocalTOCs()
  Dim startTag As Long
  startTag = InStr(LCase(TargetText), "<toc>")
  If startTag > 0 Then
    TargetText = Left(TargetText, startTag - 1) & SectionContents & Mid(TargetText, startTag + 5)
    If InStr(LCase(TargetText), "<toc>") Then MsgBox "More than one <toc> in '" & SourceFilename & "'"
  End If
End Sub

Private Function SectionContents() As String
  Dim retval As String
  Dim localNextEntry As Long
  Dim lvl As Long, nextLevel As Long, prevLevel As Long
  Dim nextName As String, nextHref As String
  Dim localHeadingWord(10) As String
  
  localNextEntry = NextProjectFileEntry
  localHeadingWord(HeadingLevel) = HeadingWord(HeadingLevel)
  prevLevel = HeadingLevel
  GoSub GetNextEntryLevel
  If nextLevel > HeadingLevel Then
    While nextLevel > HeadingLevel
      
      If nextLevel > prevLevel Then
        For lvl = prevLevel To (nextLevel - 1)
          retval = retval & "<ul>" & vbCr
        Next
      ElseIf nextLevel < prevLevel Then
        For lvl = nextLevel To (prevLevel - 1)
          retval = retval & "</ul>" & vbCr
        Next
      End If
      
      retval = retval & "<li><a href=""" & nextHref & """>" & nextName & "</a>" & vbCr
      prevLevel = nextLevel
      GoSub GetNextEntryLevel
    Wend
    retval = retval & "</ul>" & vbCr
  End If
  SectionContents = retval
  Exit Function
  
GetNextEntryLevel:
  If localNextEntry > MaxProjectFileEntry Then
    nextLevel = 0
  Else
    nextName = LTrim(ProjectFileEntry(localNextEntry))
    nextLevel = (Len(ProjectFileEntry(localNextEntry)) - Len(nextName)) / 2 + 1
    nextName = RTrim(nextName)
    localHeadingWord(nextLevel) = nextName
    nextHref = ""
    For lvl = HeadingLevel To nextLevel - 1
      nextHref = nextHref & localHeadingWord(lvl) & "\"
    Next
    nextHref = nextHref & nextName & ".html"
    localNextEntry = localNextEntry + 1
  End If
  Return
  
End Function

Private Sub CopyImages()
  Dim startPos As Long, endPos As Long, chrPos As Long, lcaseText As String
  Dim ImageFilename As String, SrcPath As String, DstPath As String
  Dim HTMLsafeFilename As String
  Dim IgnoreAll As Boolean
  
  lcaseText = LCase(TargetText)
  If OutputFormat = tHELP Or OutputFormat = tPRINT Then Exit Sub
  Status "Copying Images"
  SrcPath = PathNameOnly(SourceBaseDirectory & SourceFilename) & "\"
  DstPath = PathNameOnly(SaveDirectory & SaveFilename) & "\"
  startPos = InStr(lcaseText, "<img src=""")
  While startPos > 0
    endPos = InStr(startPos + 10, lcaseText, """")
    If endPos = 0 Then Exit Sub
    ImageFilename = Mid(TargetText, startPos + 10, endPos - startPos - 10)
CheckForImage:
    If Len(Dir(SrcPath & ImageFilename)) > 0 Then
      Dim s$
      s = PathNameOnly(AbsolutePath(ReplaceString(ImageFilename, "/", "\"), DstPath))
      If Len(Dir(s, vbDirectory)) = 0 Then MkDirPath s
      FileCopy SrcPath & ImageFilename, DstPath & ImageFilename
    ElseIf Not IgnoreAll Then
      Select Case MsgBox("Missing image: " & vbCr & SrcPath & ImageFilename, vbAbortRetryIgnore, "AuthorDoc") = vbAbort
        Case vbAbort: Exit Sub
        Case vbRetry: GoTo CheckForImage
        Case vbIgnore
          If MsgBox("Ignore all missing images?", vbYesNo, "AuthorDoc") = vbYes Then IgnoreAll = True
      End Select
    End If
    
    If OutputFormat = tHTML Then
      HTMLsafeFilename = ReplaceString(ImageFilename, "\", "/")
      HTMLsafeFilename = ReplaceString(HTMLsafeFilename, " ", "%20")
      If HTMLsafeFilename <> ImageFilename Then
        TargetText = Left(TargetText, startPos + 9) & HTMLsafeFilename & Mid(TargetText, endPos)
        lcaseText = LCase(TargetText)
      End If
    End If
    
    startPos = InStr(endPos, lcaseText, "<img src=""")
  Wend
End Sub

Private Sub HREFsInsureExtension()
  Dim LinkRef$, LinkFile$, LinkTopic$
  Dim startPos As Long, endPos As Long, pos As Long
    
  If OutputFormat = tHELP Then
    HREFsInsureExtensionWithWord
  ElseIf OutputFormat = tPRINT Then
    'We don't preserve links in printable, so skip this step
  Else
    Status "HREFsInsureExtension"
    startPos = InStr(LCase(TargetText), "<a href=""")
    While startPos > 0
      endPos = InStr(startPos + 9, TargetText, """")
      If endPos = 0 Then Exit Sub
      LinkRef = Mid(TargetText, startPos + 9, endPos - startPos - 9)
      pos = InStr(LinkRef, "#")
      If pos = 0 Then
        LinkFile = LinkRef
        LinkTopic = ""
      Else
        LinkFile = Left(LinkRef, pos - 1)
        LinkTopic = Mid(LinkRef, pos)
      End If
      If InStr(LCase(LinkFile), ".html") < 1 And InStr(LinkFile, ":") < 1 Then
        If LCase(Right(LinkFile, 4)) = ".txt" Then
          LinkFile = Left(LinkFile, Len(LinkFile) - 3) & "html"
        Else
          LinkFile = LinkFile & ".html"
        End If
      End If
      If OutputFormat = tHTML Then
        LinkFile = ReplaceString(LinkFile, "\", "/")
        LinkFile = ReplaceString(LinkFile, " ", "%20")
      End If
      If LinkFile & LinkTopic <> LinkRef Then
        TargetText = Left(TargetText, startPos + 8) & LinkFile & LinkTopic & Mid(TargetText, endPos)
      End If
      startPos = InStr(endPos, LCase(TargetText), "<a href=""")
    Wend
  End If
End Sub
Private Sub HREFsInsureExtensionWithWord()
  Dim LinkRef$, LinkLabel$, topic$, id$, pos As Long
  With word
    .StartOfDocument
    .Cancel
    Status "HREFsInsureExtension"
    .EditFind "<A HREF=""", "", 0
    While .EditFindFound
      'set LinkRef$
      .CharRight '.EditClear
      .EditBookmark "LinkStart"
      .EditFind """>"
      If Not .EditFindFound Then Exit Sub
      .CharLeft '.EditClear
      .ExtendSelection
      .EditGoTo "LinkStart"
      LinkRef = .Selection
      .Cancel
      .CharRight
      If InStr(LCase(LinkRef), ".html") < 1 Then
        pos = InStr(1, LinkRef, "#")
        If pos > 0 Then .CharLeft (Len(LinkRef) - pos + 1)
        .ExtendSelection
        .CharLeft 4
        If LCase(.Selection) = ".txt" Then
          .EditClear
        Else
          .Cancel
          .CharRight
        End If
        .Insert ".html"
      End If
      
      .EditFind "<A HREF="""
    Wend
  End With
End Sub

Private Sub DebugMsg(s$)
  Debug.Print s
End Sub

Private Sub Status(s$)
  frmConvert.Text1.Text = s
  DoEvents
  'DebugMsg "Status: " & s
End Sub

Public Sub FormatCardGraphic()
  Dim startPos As Long, endPos As Long, TableText As String, ImageFilename As String, ImageDirectory As String
  Dim ImageMap As String
  startPos = InStr(TargetText, WholeCardHeader)
  If startPos > 0 Then
    endPos = InStrRev(TargetText, Asterisks80)
    If endPos > 0 Then
      ImageDirectory = PathNameOnly(SaveDirectory & SaveFilename)
      ImageFilename = FilenameOnly(SaveFilename) & ".bmp"
      TableText = Mid(TargetText, startPos + lenWholeCardHeader, endPos - startPos - lenWholeCardHeader)
      ImageMap = CardImage(TableText)
      If Len(ImageMap) > 0 Then
        TargetText = "<map name=""CardImageMap"">" & vbCrLf & ImageMap & "</map>" & vbCrLf _
                   & Left(TargetText, startPos - 1) & "<p>" & vbCrLf & "<img src=""" & ImageFilename & """ usemap=""#CardImageMap"" border=0>" & vbCrLf & "<p>" & Mid(TargetText, endPos + 81)
      Else
        TargetText = Left(TargetText, startPos - 1) & "<p>" & vbCrLf & "<img src=""" & ImageFilename & """>" & vbCrLf & "<p>" & Mid(TargetText, endPos + 81)
      End If
      If Len(Dir(ImageDirectory, vbDirectory)) = 0 Then MkDir ImageDirectory
      SavePicture frmSample.img.Image, ImageDirectory & "\" & ImageFilename
    End If
  End If
End Sub

'Creates image on frmSample.img and returns HTML map for links
Public Function CardImage(TableText As String) As String
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
  Dim SubsectionName As String
  Dim retval As String
  Dim parsePos As Long, StartLinkCol As Long, StopLinkCol As Long
  Dim SaveFileNameOnly As String
  Dim DoLinks As Boolean
  
  DoLinks = True
  
  SaveFileNameOnly = FilenameOnly(SaveFilename)
  retval = ""
  GrayLevel = 170
  txt = ReplaceString(TableText, "&gt;", ">")
  txt = ReplaceString(txt, "&lt;", "<")
  GrayColor = RGB(GrayLevel, GrayLevel, GrayLevel)
  
  lentxt = Len(txt)
  thisCR = 0
  Rows = 0
  RowStopChecking = 256
  GoSub FindCR
  While lastCR <= lentxt
    Rows = Rows + 1
    TextRow(Rows) = Mid(txt, lastCR + 1, thisCR - lastCR - 1)
    If Right(TextRow(Rows), 1) = vbCr Then TextRow(Rows) = Left(TextRow(Rows), Len(TextRow(Rows)) - 1)
    Select Case TextRow(Rows)
      Case SixSplats, SevenSplats, Asterisks80, TensPlace, OnesPlace 'Skip some rows
        Rows = Rows - 1
      Case "Example"
        If RowStopChecking > 0 Then RowStopChecking = Rows
      Case "<otyp>"
        DoLinks = False
      Case "SPEC-ACTIONS"
        DoLinks = False
        RowStopChecking = 0
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
      
      StartLinkCol = 0
      StopLinkCol = 0
      
      If LCase(Mid(TextRow(Row), 3, lenTableType)) = LCase(TableType) Then
        Mid(TextRow(Row), 3, lenTableType) = TableType
        StartLinkCol = lenTableType + 3
        GoSub StartArea
      End If
      
      If LCase(Mid(TextRow(Row), 3, 13)) = "general input" Then
        Mid(TextRow(Row), 3, 7) = "General input"
        StartLinkCol = 3
        GoSub StartArea
      End If
      
      If LCase(Mid(TextRow(Row), 3, 7)) = "section" Then
        Mid(TextRow(Row), 3, 7) = "Section"
        StartLinkCol = 11
        GoSub StartArea
      End If
      
      If Not DoLinks Then StartLinkCol = 0
      
      For col = 1 To Len(TextRow(Row))
        curChar = Mid(TextRow(Row), col, 1)
        .CurrentX = XMargin + (col - 1) * CharWidth + (CharWidth - .TextWidth(curChar)) / 2
        .CurrentY = RowY(Row)
                
        If col = StartLinkCol Then .ForeColor = vbBlue
        If col = StopLinkCol Then GoSub EndTableLink
        
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
      If .ForeColor <> vbBlack Then GoSub EndTableLink
      frmSample.img.Print
      'frmSample.img.Line (col * CharWidth, 0)-((col + 1) * CharWidth, .Height), RGB(222, 222, 222), BF
    Next
    'Clipboard.SetData .Image
    If frmSample.WindowState <> vbNormal Then frmSample.WindowState = vbNormal
    frmSample.Move frmSample.Left, frmSample.Top, (.Width * Screen.TwipsPerPixelX) + 108, (.Height * Screen.TwipsPerPixelY) + 372
    CardImage = retval
  
    Exit Function
  
StartArea:
    If DoLinks Then
      retval = retval & "<area coords=""" & .CurrentX & "," & .CurrentY
      StopLinkCol = InStr(StartLinkCol, TextRow(Row), "]")
      If StopLinkCol > 0 Then 'Ignore tables containing the string "Tables in brackets [] are ..."
        If Mid(TextRow(Row), StopLinkCol - 1, 1) = "[" Then StopLinkCol = 0
      End If
      If StopLinkCol = 0 Then
        StopLinkCol = InStr(StartLinkCol, TextRow(Row), "  ")
        If StopLinkCol = 0 Then
          StopLinkCol = 999
        End If
      End If
    End If
    Return
    
EndTableLink:
    If DoLinks Then
      .ForeColor = vbBlack
      If curChar = "]" Or curChar = " " Then
        SubsectionName = Trim(Mid(TextRow(Row), 3, col - 3))
      Else
        SubsectionName = Trim(Mid(TextRow(Row), 3))
      End If
      If Left(SubsectionName, 7) = "Section" Then SubsectionName = Mid(SubsectionName, 9)
      If Left(SubsectionName, lenTableType) = TableType Then SubsectionName = Mid(SubsectionName, lenTableType + 1)
      retval = retval & "," & .CurrentX & "," & .CurrentY + CharHeight & """ href="""
      
      'Several special cases for sections that are not where they are expected
      Select Case Left(SubsectionName, 9)
        Case "SOIL-DATA", "CROP-DATE"
          retval = retval & "PWATER Input"
        Case "OXRX inpu", "NUTRX inp", "PLANK inp", "PHCARB in"
          retval = retval & SaveFileNameOnly
          If SaveFileNameOnly <> "Input for RQUAL sections" Then
            retval = retval & "/Input for RQUAL sections"
          End If
        Case "SURF-EXPO"
          If SaveFileNameOnly = "GQUAL input" Then
            retval = retval & "Input for RQUAL sections/PLANK input"
          Else
            retval = retval & SaveFileNameOnly
          End If
        Case Else
          If SaveFileNameOnly <> "OXRX input" And (SubsectionName = "ELEV" Or Left(SubsectionName, 3) = "OX-") Then
            retval = retval & "Input for RQUAL sections/OXRX input"
          Else 'Most links do not need tweaking and fall through to here
            retval = retval & SaveFileNameOnly
          End If
      End Select
      retval = retval & "/" & SubsectionName & ".html"">" & vbCrLf
    End If
    Return
  
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
  
  thisCR = InStr(thisCR + 1, txt, vbLf)
  If thisCR = 0 Then thisCR = lentxt + 1
  Return
    
End Function

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

