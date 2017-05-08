VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCoHSPFOutput"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Dim pStatusFileName As String
Dim pErrorDescription As String
Dim pTSerFile As ATCclsTserFile
Dim pHSPFOutputTserFile As clsTSerHSPFOutput
Dim pFilter As clsTSerFilter
Dim pAllTSer As Collection  ' of ATCTSer
Dim pTSer As Collection 'of ATCTSer (filtered)

Public Type HSPFFiles
  FUnit As Long
  FName As String
  FType As String
  UnitType As Long '1-English, 2-Metric
End Type
Dim pOutputFiles As FastCollection 'of HSPFFiles
Public Type Section
  rec As String
  numeric As Boolean
  multHeader As Boolean
End Type
Public Type PageInstance
  Offset As Long
  SnowPackFlag As Boolean
  SnowFluxFlag As Boolean
End Type
Public Type PageType
  len As Long
  PageInstances As FastCollection 'of PageInstance
  dates As ATCclsTserDate
  tsers As FastCollection 'of ATCTSer
  SnowFlag As Boolean
End Type
Dim pPages As FastCollection 'of PageType
Dim pSnowStateOffset As Long
Dim pSnowFluxOffset As Long

Public Property Get Tser() As Collection
  Set Tser = pTSer
End Property
Public Property Set Tser(NewTser As Collection)
  Set pTSer = NewTser
End Property

Public Property Get AllTser() As Collection
  Set AllTser = pAllTSer
End Property

Public Property Get DataCollection() As Collection
  Set DataCollection = pTSer
End Property

Friend Property Get Filter() As clsTSerFilter
  Set Filter = pFilter
End Property

Public Property Get PageCollection() As FastCollection
  Set PageCollection = pPages
End Property

Public Property Get OutputFiles() As FastCollection
  Set OutputFiles = pOutputFiles
End Property

Property Get StatusFile() As String
  StatusFile = pStatusFileName
End Property
Property Let StatusFile(newName As String)
  Dim s As String, rec As String, kword As String, d As String
  
  If Len(Dir(newName)) = 0 Then
    pErrorDescription = "Status File Name '" & newName & "' not found"
  Else
    Screen.MousePointer = vbHourglass
    pStatusFileName = newName
    d = PathNameOnly(pStatusFileName)
    ChDriveDir d
    s = WholeFileString(FilenameNoPath(pStatusFileName))
    While Len(s) > 0
      rec = StrSplit(s, vbCrLf, "")
      kword = UCase(StrRetRem(rec))
      Select Case kword
        Case "UCI":
          GetUCIOutputInfo (rec)
        Case "TSER":
          
        Case Else:
          pErrorDescription = "Unable to process the following keyword in HSPF Output Status File: " & kword
      End Select
    Wend
    Screen.MousePointer = vbDefault
  End If

End Property

Public Property Get ErrorDescription() As String
  ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

Private Sub GetUCIOutputInfo(FName As String)
  Dim UCIFile$, FileTable$, rec$, pth$
  Dim FilesAvail As FastCollection 'of HSPFFiles
  Dim lFile As HSPFFiles

  If Len(Dir(FName)) = 0 Then
    pErrorDescription = "UCI File '" & FName & "' not found"
  Else
    pth = PathNameOnly(FName)
    If Len(pth) > 0 Then pth = pth & "\"
    Set FilesAvail = Nothing
    Set FilesAvail = New FastCollection
    Set pOutputFiles = Nothing
    Set pOutputFiles = New FastCollection
    UCIFile = WholeFileString(FName)
    FileTable = GetUCITable(UCIFile, "FILES")
    While Len(FileTable) > 0
      rec = StrSplitSave(FileTable, vbCrLf, "", True, False)
      If Len(rec) > 16 Then
        If IsNumeric(Mid(rec, 10, 5)) Then 'valid unit number
          lFile.FUnit = Mid(rec, 10, 5)
          lFile.FName = pth & Mid(rec, 17)
          FilesAvail.Add lFile
        End If
      End If
    Wend
    GetUCIOutputFiles UCIFile, FName, FilesAvail
  End If

End Sub

Private Sub GetUCIOutputFiles(UCIFile As String, UCIFname As String, FilesAvail As FastCollection)
  Dim i&, j&, k&, upos&, OUnit&, GetUnits As Boolean
  Dim rec$, OpName$(3), OpTable$
  Dim lFile As HSPFFiles

  Set pPages = New FastCollection
  Set pAllTSer = New Collection
  Set pTSer = New Collection
  OpName(1) = "PERLND"
  OpName(2) = "IMPLND"
  OpName(3) = "RCHRES"
  For i = 1 To 3
    OpTable = GetUCITable(UCIFile, OpName(i))
    GetUnits = False
    If i = 2 Then 'implnd, slightly different format
      upos = 46
    Else
      upos = 51
    End If
    While Len(OpTable) > 0
      rec = StrSplitSave(OpTable, vbCrLf, "", True, False)
      If InStr(rec, "END GEN-INFO") > 0 Then 'done with this op
        OpTable = ""
      ElseIf GetUnits And Len(rec) > upos - 1 Then
        For j = 1 To 2
          If IsNumeric(Mid(rec, upos + (j - 1) * 5, 5)) Then 'valid english unit number
            OUnit = Mid(rec, upos + (j - 1) * 5, 5)
            If OUnit > 0 Then 'see if unit number already saved
              For k = 1 To pOutputFiles.Count
                If OUnit = pOutputFiles(k).FUnit Then 'already got it
                  OUnit = 0
                End If
              Next k
            End If
            If OUnit > 0 Then
              For k = 1 To FilesAvail.Count
                If OUnit = FilesAvail(k).FUnit Then 'save unit number and name
                  lFile.FUnit = OUnit
                  lFile.FName = FilesAvail(k).FName
                  lFile.FType = OpName(i)
                  lFile.UnitType = j
                  pOutputFiles.Add lFile
                  BuildTSers lFile.FName, FilenameOnly(UCIFname)
                  Debug.Print pAllTSer.Count & " tsers found in " & lFile.FName
                  Exit For
                End If
              Next k
            End If
          End If
        Next j
      ElseIf InStr(rec, "GEN-INFO") > 0 Then 'start processing unit numbers
        GetUnits = True
      End If
    Wend
  Next i
  Set pTSer = pFilter.Filter(pAllTSer)
End Sub

Private Sub BuildTSers(FName As String, Scen As String)
  Dim outfile As String, rec As String, CurPage As String
  Dim TCODE&, CurID&, CurLoc As String, OpTyp As String
  Dim CurDate&(5), FileOffset&, LenOutfile&

  Dim GetPage As Boolean, PageTypeInd&, HdrEnd&, epos&
  Dim lPage As PageType, lDateSumm As ATTimSerDateSummary
  Dim sdat&(5), edat&(5)
  Dim lPageInstance As PageInstance
  
  If Len(Dir(FName)) = 0 Then
    pErrorDescription = "HSPF Outut File '" & FName & "' not found"
  Else
    epos = -1 'cryptic offset to skip "1" and blank lines at start of 1st page
    outfile = WholeFileString(FName)
    LenOutfile = Len(outfile)
    While epos < LenOutfile
      GoSub GetNextPageNeeded
      If Len(CurPage) > 0 Then
        ProcessPage FName, CurPage, Scen, OpTyp, CurLoc, PageTypeInd, outfile
      End If
    Wend
  End If
Exit Sub

GetNextPageNeeded:
  'assumes outfile is at the start of the next page
  GetPage = False
  CurPage = ""
  While Not GetPage And epos < LenOutfile
    'skips CR/LF at end of last page, plus skips "1" and 2 blank lines at start of this page
    FileOffset = epos + 7
    HdrEnd = InStr(FileOffset, outfile, vbCrLf)
    epos = InStr(HdrEnd, outfile, vbCrLf & "1" & vbCrLf)
    If epos = 0 Then epos = LenOutfile 'last page in the file
    rec = Mid(outfile, FileOffset, HdrEnd - FileOffset)
    GetSummaryDate rec, OpTyp, CurID, CurLoc, CurDate, TCODE
'    PageTypeInd = pPages.IndexFromKey(OpTyp & "-" & CurID & "-" & TCODE)
    PageTypeInd = pPages.IndexFromKey(OpTyp & "-" & CurID)
    If PageTypeInd > 0 Then 'already have this Operation and ID
      GetPage = False
      If pPages.ItemByIndex(PageTypeInd).dates.Summary.Tu = TCODE Then
        'another page of this print interval, keep track of it
        lPageInstance.Offset = FileOffset
        SetSnowFlags lPageInstance, Mid(outfile, lPageInstance.Offset, 400), pPages.ItemByIndex(PageTypeInd).SnowFlag
        pPages.ItemByIndex(PageTypeInd).PageInstances.Add lPageInstance
        lDateSumm = pPages.ItemByIndex(PageTypeInd).dates.Summary
        With lDateSumm
          .NVALS = .NVALS + 1
          J2Date .SJDay, sdat
          TIMADD sdat, .Tu, .ts, .NVALS, edat
          .EJDay = Date2J(edat)
          .Intvl = (.EJDay - .SJDay) / .NVALS
        End With
        pPages.ItemByIndex(PageTypeInd).dates.Summary = lDateSumm
      End If
    Else 'need this Operation and ID
      GetPage = True
      Set lPage.tsers = New FastCollection
      Set lPage.dates = New ATCclsTserDate
      With lDateSumm
        .CIntvl = True
        .ts = 1
        .Tu = TCODE
        .NVALS = 1
        If CurDate(2) = 0 Then CurDate(2) = 1 'set to first day of month
        .SJDay = Date2J(CurDate)
        TIMADD CurDate, .Tu, .ts, .NVALS, edat
        .EJDay = Date2J(edat)
      End With
      lPage.dates.Summary = lDateSumm
      lPage.len = epos - HdrEnd - 2
      If InStr(Mid(outfile, FileOffset, lPage.len), "SNOW") > 0 Then
        lPage.SnowFlag = True
      Else
        lPage.SnowFlag = False
      End If
      Set lPage.PageInstances = New FastCollection
      lPageInstance.Offset = FileOffset
      SetSnowFlags lPageInstance, Mid(outfile, lPageInstance.Offset, 400), lPage.SnowFlag
      lPage.PageInstances.Add lPageInstance
      'pPages.Add lPage, OpTyp & "-" & CurID & "-" & TCODE
      pPages.Add lPage, OpTyp & "-" & CurID
      PageTypeInd = pPages.Count
      CurPage = Mid(outfile, HdrEnd + 4, lPage.len)
    End If
  Wend
Return

End Sub

Private Sub SetSnowFlags(tPageInstance As PageInstance, s As String, tSnowFlag As Boolean)
  With tPageInstance
    If tSnowFlag Then
      If InStr(s, "NO PACK") Then
        .SnowPackFlag = False
      Else
        .SnowPackFlag = True
      End If
      If InStr(s, "FLUXES ALL ZERO") Then
        .SnowFluxFlag = False
      Else
        .SnowFluxFlag = True
      End If
    Else
      .SnowPackFlag = False
      .SnowFluxFlag = False
    End If
  End With
End Sub

Private Sub ProcessPage(FName As String, CurPage As String, Scen As String, OpTyp As String, CurLoc As String, PageTypeInd As Long, outfile As String)
  
  Dim Sect As FastCollection 'of Section
  Dim Headers As FastCollection, newHeaders As FastCollection, savHeaders As FastCollection
  Dim CurSect As String, SubSect As String
  Dim i&, j&, reclen&, HdrInd&, HdrPos&, TserOffset&, TserOffsetSave&
  Dim lstr As String, rec As String, Cons As String, lCons As String
  Dim InNumbers As Boolean, InField As Boolean, DecPt As Boolean
  Dim lTSer As ATCclsTserData
  Dim crop As Long, flen As Long
  Dim lTserOffset As Long
  Dim lSnowFlux As Boolean, lSnowState As Boolean

  TserOffset = 135
  lSnowFlux = False
  lSnowState = False
  While Len(CurPage) > 0
    Set Sect = GetNextSubSect(CurPage, CurSect, SubSect, TserOffset)
    If CurSect = "SNOW" Then
      If SubSect = "STATE VARIABLES" Then
        lSnowState = True
      ElseIf SubSect = "FLUXES" Then
        lSnowFlux = True
      ElseIf SubSect = "NO PACK" Then
      ElseIf SubSect = "FLUXES ALL ZERO" Then
      End If
    End If
    If Left(CurSect, 4) = "OXRX" Or Left(CurSect, 5) = "NUTRX" Then
      'special cases needed to handle multiple consituents in these sections
      If CurSect = "OXRX" Or CurSect = "NUTRX" Then
        'no constituent name in section yet, must be the start of a new constituent
        CurSect = CurSect & "-" & SubSect
        If SubSect = "DISSOLVED OXYGEN" Then
          lCons = "DO"
        ElseIf SubSect = "BIOCHEMICAL OXYGEN DEMAND" Then
          lCons = "BOD"
        Else 'extract chemical symbols from NUTRX names
          i = InStr(SubSect, "(")
          If i > 0 Then
            lCons = Mid(SubSect, i + 1, Len(SubSect) - i - 1)
          End If
        End If
        SubSect = "" 'reset SubSect and get it from next record
        While SubSect = "" And Sect.Count > 0
          TserOffset = TserOffset + Len(Sect(1).rec) + 2
          Sect.Remove 1
          If Len(Sect(1).rec) > 0 Then
            j = InStr(LTrim(Sect(1).rec), "  ")
            If j > 0 Then 'found 2 blanks, just take up to there
              SubSect = Trim(Left(LTrim(Sect(1).rec), j))
            Else 'take whole record
              SubSect = Trim(Sect(1).rec)
            End If
          End If
        Wend
      End If
      If SubSect = "BALANCE" Then 'last subsection for this constituent
        j = InStr(CurSect, "-")
        CurSect = Left(CurSect, j - 1) 'reset section name to OXRX or NUTRX
      End If
    End If
    If SubSect = "GENERALIZED QUALITY" Then 'GQUAL, save its number
      CurSect = "GQ" & CLng(Right(Sect(2).rec, 2)) & "-" & Trim(Mid(Sect(1).rec, 42))
      For i = 1 To Sect.Count
        TserOffset = TserOffset + Len(Sect(i).rec) + 2
      Next i
    ElseIf SubSect <> "BALANCE" Then 'skip mass balances
      Set Headers = Nothing
      Set Headers = New FastCollection
      InNumbers = False
      For i = 1 To Sect.Count
        If Sect(i).numeric Then 'numbers on this record
          Set savHeaders = Nothing
          Set savHeaders = Headers 'may need these for ensuing numeric records
          HdrInd = 0
          rec = Sect(i).rec
          TserOffset = TserOffset + Len(rec)
          TserOffsetSave = TserOffset
          While IsRecNumeric(rec) 'parse numbers out of record
            reclen = Len(rec)
            If HdrInd > 0 Then 'only look at header lines that contain constituent names
              HdrPos = HdrInd
            Else 'look at all header lines
              HdrPos = 1
            End If
            For j = HdrPos To Headers.Count
              If Len(Headers(j)) > reclen Then 'last number must not be right justified
                reclen = Len(Headers(j))
              End If
            Next j
            If reclen > Len(rec) Then 'number is not right justified
              TserOffset = TserOffset + reclen - Len(rec)
              rec = rec & Space(reclen - Len(rec))
            End If
            If IsNumeric(Right(rec, 10)) Then 'valid value
              Select Case Left(CurSect, 4)
                Case "SNOW", "PWAT", "SEDM", "PQUA", "HYDR", "IWAT", "IWTG", "IQUA", "SOLI", "PLAN"
                  If (CurSect = "PQUAL" Or CurSect = "IQUAL" Or CurSect = "HYDR" Or _
                      CurSect = "PLANK") And InStr(SubSect, "FLUXES") > 0 Then
                    'no units, var name on last header record
                    HdrInd = Headers.Count
                  Else 'units above value, skip up one to get var name
                    HdrInd = Headers.Count - 1
                  End If
                Case "OXRX", "NUTR"
                  HdrInd = -Headers.Count
                Case Else 'look for variable name on last header record
                  If Headers.Count > 0 Then
                    If Len(Headers(Headers.Count)) > Len(rec) - 10 Then
                      'there is a label above the number
                      HdrInd = Headers.Count
                    Else
                      HdrInd = 0
                    End If
                  Else 'no label above number, look for label on same record as number
                    HdrInd = 0
                  End If
              End Select
              Cons = ""
              If HdrInd > 0 Then 'variable name should be in header
                If Len(Headers(HdrInd)) > 10 Then
                  flen = Len(Headers(HdrInd)) - InStrRev(Headers(HdrInd), " ", Len(Headers(HdrInd)) - 9)
                  Cons = Trim(Right(Headers(HdrInd), flen))
                End If
              ElseIf HdrInd < 0 Then
                For j = 1 To Abs(HdrInd)
                  If Len(Headers(j)) > Len(rec) - 10 Then
                    If Len(Cons) > 0 Then
                      If Right(Cons, 1) = "-" Then 'remove hyphen
                        Cons = Left(Cons, Len(Cons) - 1)
                      Else 'insert blank between words
                        Cons = Cons & " "
                      End If
                    End If
                    flen = Len(Headers(j)) - InStrRev(Headers(j), " ", Len(Headers(j)) - 9)
                    Cons = Cons & Trim(Right(Headers(j), flen))
                  End If
                Next j
              End If
              If Len(Cons) = 0 Then 'look on same record as value
                Cons = GetConsName(rec)
                flen = 10
              End If
              If CurSect = "PQUAL" Or CurSect = "IQUAL" Then
                'add constituent name from beginning of record
                Cons = Cons & "-" & Trim(Left(rec, 15))
              ElseIf CurSect = "SEDTRN" Then
                If HdrInd <> 0 Then
                  Cons = "SED-" & GetConsName(rec) & "-" & Cons
                Else
                  Cons = "SED-" & Cons
                End If
              ElseIf Left(CurSect, 2) = "GQ" Then
                lstr = Mid(CurSect, 5)
                If Left(SubSect, 5) = "STATE" Then
                  If HdrInd <> 0 Then
                    If InStr(Headers(1), "STORAGE") > 0 Then
                      Cons = lstr & "-" & Mid(rec, 8, 3) & " STORAGE-" & Cons
                    ElseIf InStr(Headers(1), "CONC") > 0 Then
                      Cons = lstr & "-SED. CONC.-" & Cons
                    End If
                  Else
                    Cons = lstr & "-" & Cons
                  End If
                ElseIf Left(SubSect, 6) = "FLUXES" And HdrInd = 1 Then
                  Cons = lstr & "-" & Trim(Left(Headers(1), 40)) & "-" & Cons
                Else
                  Cons = lstr & "-" & Cons
                End If
              ElseIf Left(CurSect, 4) = "OXRX" Then
                Cons = lCons & "-" & Cons
              ElseIf Left(CurSect, 5) = "NUTRX" Then
                If lCons = "TAM-N" And Left(SubSect, 5) = "STATE" Then
                  'lots of exceptions for TAM state variables
                  If InStr(rec, "ED CONC") > 0 Then 'dissolved conc
                    Cons = lCons & "-" & GetConsName(rec) & "-" & Cons
                  ElseIf InStr(rec, "WITH SED") > 0 Then 'NH4 sed conc
                    Cons = lCons & "-NH4 SED. CONC.-" & Cons
                  ElseIf InStr(rec, "ED STOR") > 0 Then 'dissolved storage
                    If Len(rec) > 65 Then 'NH3
                      Cons = lCons & "-DISSOLVED STORAGE-NH3"
                    ElseIf Len(rec) > 53 Then 'NH4
                      Cons = lCons & "-DISSOLVED STORAGE-NH4"
                    Else
                      Cons = lCons & "-DISSOLVED STORAGE-TAM"
                    End If
                  ElseIf InStr(rec, "SEDIMENT") > 0 Then 'susp sed storage
                    Cons = lCons & "-SUSP. SED. STORAGE-" & Cons
                  Else
                    Cons = lCons & "-" & Cons
                  End If
                ElseIf HdrInd <> 0 Then
                  Cons = lCons & "-" & GetConsName(Headers(Abs(HdrInd))) & "-" & Cons
                Else
                  Cons = lCons & "-" & Cons
                End If
              ElseIf Left(CurSect, 5) = "PLANK" Then
                If HdrInd > 0 Then
                  Cons = CurSect & "-" & GetConsName(Headers(HdrInd)) & "-" & Cons
                Else
                  Cons = CurSect & "-" & Cons
                End If
                
              End If
              TserOffset = TserOffset - 10
              If Not IsNumeric(Mid(outfile, TserOffset + 6, 10)) Then
                Debug.Print "Parse Error:" & OpTyp & ":" & CurSect & ":" & SubSect & ":" & Scen & _
                             ":" & CurLoc & ":" & Cons & ":" & TserOffset & ":" & _
                             Mid(outfile, TserOffset + 6, 10) '6 is offset of first page (should be variable)
              Else
                'Debug.Print "AddTSer:" & OpTyp & ":" & CurSect & ":" & SubSect & ":" & Scen & _
                '             ":" & CurLoc & ":" & Cons & ":" & TserOffset & ":" & _
                '             Mid(outfile, TserOffset + 6, 10) '6 is offset of first page (should be variable)
                Set lTSer = New ATCclsTserData
                Set lTSer.File = pTSerFile
                lTSer.AttribSet "Operation", OpTyp
                lTSer.AttribSet "Section", CurSect
                lTSer.AttribSet "SubSection", SubSect
                lTSer.AttribSet "IDSCEN", Scen
                lTSer.AttribSet "IDLOCN", CurLoc
                lTSer.AttribSet "IDCONS", Cons
                lTSer.AttribSet "OutFile", FName
                lTserOffset = TserOffset
                If lSnowState And Not (CurSect = "SNOW" And SubSect = "STATE VARIABLES") Then
                  lTserOffset = lTserOffset - pSnowStateOffset
                End If
                If lSnowFlux And Not (CurSect = "SNOW" And SubSect = "FLUXES") Then
                  lTserOffset = lTserOffset - pSnowFluxOffset
                End If
                lTSer.AttribSet "OffsetInPage", CStr(lTserOffset)
                Set lTSer.dates = pPages.ItemByIndex(PageTypeInd).dates
                pAllTSer.Add lTSer
                pPages.ItemByIndex(PageTypeInd).tsers.Add lTSer
              End If
            Else 'skip this field
              TserOffset = TserOffset - 10
              flen = 10
            End If
            If flen < 10 Then flen = 10
            'remove field from end of records
            Set newHeaders = Nothing
            Set newHeaders = New FastCollection
            For j = 1 To Headers.Count
              If Len(Headers(j)) > Len(rec) - flen Then
                lstr = RTrim(Left(Headers(j), Len(Headers(j)) - flen))
              Else
                lstr = Headers(j)
              End If
              newHeaders.Add lstr
            Next j
            Set Headers = Nothing
            Set Headers = newHeaders
            crop = Len(rec) - 10
            rec = RTrim(Left(rec, Len(rec) - 10))
            TserOffset = TserOffset - crop + Len(rec)
            'If crop <> Len(rec) Then
              'Debug.Print "XXX"
            'End If
          Wend
          TserOffset = TserOffsetSave + 2 'reset offset position and include CR/LF
          InNumbers = True
          Set Headers = Nothing
          Set Headers = savHeaders 'may need these for ensuing numeric records
        Else 'label or blank line
          If InNumbers Then 'done with numbers,
            'this must be the start of another set of numbers
            Set Headers = Nothing
            Set Headers = New FastCollection
          End If
          If Len(Sect(i).rec) > 0 Then 'header for next set of numbers
            Headers.Add Sect(i).rec
          End If
          TserOffset = TserOffset + Len(Sect(i).rec) + 2
          InNumbers = False
        End If
      Next i
    Else 'balance (skip)
      For i = 1 To Sect.Count
        TserOffset = TserOffset + Len(Sect(i).rec) + 2
      Next i
    End If
  Wend
End Sub

Private Function GetConsName(rec As String) As String
  Dim HPos&, lstr$

  lstr = ""
  HPos = InStr(LTrim(rec), "  ")
  If HPos > 1 Then
    lstr = Trim(Left(LTrim(rec), HPos - 1))
    HPos = InStr(lstr, "(")
    If HPos > 0 Then
      lstr = Left(lstr, HPos - 1)
    End If
  End If
  GetConsName = lstr

End Function

Private Function IsRecNumeric(ByVal rec As String) As Boolean
  Dim lstr As String, retval As Boolean

  retval = False 'assume not numeric record
  If InStr(rec, "    LOSS   OUTFLOW      ") = 0 Then
    While Len(rec) > 0
      lstr = StrRetRem(rec)
      If IsNumeric(Left(lstr, 2)) Then 'numbers on this record
        retval = True
        rec = ""
      End If
    Wend
  End If
  IsRecNumeric = retval

End Function

Private Function GetNextSubSect(CurPage As String, CurSect As String, SubSect As String, TserOffset As Long) As FastCollection
  Dim rec As String, lstr As String
  Dim RetSect As FastCollection, lSect As Section
  Dim epos&, NumbersFound As Boolean, KeepReading As Boolean
  Dim lMultHeader As Boolean

  lMultHeader = False
  Set RetSect = New FastCollection
  While Len(CurPage) > 0 And RetSect.Count = 0
    While Len(rec) = 0 'skip blank lines
      rec = StrSplitSave(CurPage, vbCrLf, "", True, False)
      If Len(rec) = 0 Then TserOffset = TserOffset + 2
    Wend
    If InStr(rec, "***") > 0 Then 'new section name
      epos = InStr(6, rec, " ") 'find blank at end of section name
      CurSect = Mid(rec, 6, epos - 6)
      TserOffset = TserOffset + Len(rec) + 4
      rec = StrSplitSave(CurPage, vbCrLf, "", True, False) 'skip blank line
      rec = StrSplitSave(CurPage, vbCrLf, "", True, False) 'get first line of next sub-section
    End If
    'first record is start of subsection
    lstr = LTrim(rec)
    epos = InStr(lstr, "  ") 'look for multiple blanks to parse subsection name
    If epos > 0 Then
      SubSect = Left(lstr, epos - 1)
    Else 'just take rest of string
      SubSect = lstr
    End If
    KeepReading = True
    NumbersFound = False
    While KeepReading
      lSect.rec = rec
      lSect.numeric = IsRecNumeric(rec)
      lSect.multHeader = lMultHeader
      If Not NumbersFound Then 'see if this record contains numbers
        If lSect.numeric Then NumbersFound = True
      End If
      If Len(rec) > 0 Then
        If InStr(rec, "BY CATEGORY:") > 0 Then
          lMultHeader = True
          TserOffset = TserOffset + Len(rec) + 2
        ElseIf Left(rec, 4) <> "    " And NumbersFound And Not lSect.numeric Then
          'non-numeric record after numeric records with indenting at subsection level (or higher)
          KeepReading = False
          CurPage = rec & vbCrLf & CurPage 'put record back in Page for start of next section
        End If
      End If
      If KeepReading Then
        If InStr(rec, "BY CATEGORY:") = 0 Then RetSect.Add lSect
        If Len(CurPage) > 0 Then
          rec = StrSplitSave(CurPage, vbCrLf, "", True, False)
        Else 'end of page
          KeepReading = False
        End If
      End If
    Wend
  Wend

  Set GetNextSubSect = RetSect

End Function

Private Sub GetSummaryDate(rec As String, OpTyp As String, CurID As Long, CurLoc As String, CurDate() As Long, TCODE As Long)
  Dim ipos As Long, s As String

  If Left(LTrim(rec), 8) = "PERVIOUS" Then
    OpTyp = "P"
  ElseIf Left(LTrim(rec), 10) = "IMPERVIOUS" Then
    OpTyp = "I"
  ElseIf Left(LTrim(rec), 9) = "REACH/MIX" Then
    OpTyp = "R"
  Else
    pErrorDescription = "Unknown HSPF Output File Type, record is:  " & rec
  End If
  If OpTyp = "I" Then
    CurID = CLng(Mid(rec, 29, 4))
  Else
    CurID = CLng(Mid(rec, 27, 4))
  End If
  s = Trim(Mid(rec, 57, 20))
  If Left(s, 2) <> OpTyp & ":" Then s = OpTyp & ":" & s
  CurLoc = s & ":" & CurID
  'CurLoc = OpTyp & ":" & Trim(Mid(rec, 57, 20)) & ":" & CurID
  If InStr(80, rec, " YEAR ") > 0 Then
    TCODE = 6
    ipos = 125
  ElseIf InStr(80, rec, " MONTH ") > 0 Then
    TCODE = 5
    ipos = 125
  ElseIf InStr(80, rec, " DAY ") > 0 Then
    TCODE = 4
    ipos = 122
  ElseIf InStr(80, rec, " INTERVALS ") > 0 Then
    TCODE = 3
    ipos = 116
  End If
  CurDate(0) = CLng(Mid(rec, ipos, 4))
  CurDate(1) = CLng(Mid(rec, ipos + 5, 2))
  If TCODE <= 4 Then
    CurDate(2) = CLng(Mid(rec, ipos + 8, 2))
    If TCODE <= 3 Then
      CurDate(3) = CLng(Mid(rec, ipos + 11, 2))
      CurDate(4) = CLng(Mid(rec, ipos + 14, 2))
    End If
  End If

End Sub

Public Property Set TSerFile(newValue As ATCclsTserFile)
  Set pTSerFile = newValue
End Property

Public Property Get SnowStateOffset() As Long
  SnowStateOffset = pSnowStateOffset
End Property
Public Property Get SnowFluxOffset() As Long
  SnowFluxOffset = pSnowFluxOffset
End Property

Private Sub Class_Initialize()
  pSnowStateOffset = 668
  pSnowFluxOffset = 367
  
  Set pFilter = New clsTSerFilter
  Set pFilter.Parent = Me
  Set pFilter.FilterTypes = Array("Operation", "Section", "SubSection", "Constituent", "Location")
End Sub
