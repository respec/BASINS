VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsAnalysisReportUnit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Implements ATCclsAnalysis

Private Const pLabel = "Unit Loads"
Private Const NoValue As Single = -3E+38

Private pColl As Collection 'of ATCclsTserData
Private pSpecification As String
Private pErrorDesc As String
Private pMonitor As Object
Private pMonitorSet As Boolean

Private FormFg As Long
Private SDig As Long
Private dpla As Long
Private CWid As Long
Private PWid As Long
Private PLin As Long
Private PCode As Long
Private PYrEnd As Long
Private Title As String
Private FName As String

Public Type SectType
  Attribute As String
  sSum As Boolean
  sAve As Boolean
  sMin As Boolean
  sMax As Boolean
  sCnt As Boolean
End Type
Private PageSumm As SectType
Private ColSumm As SectType
Private RowSumm As SectType

Public Type ConsType
  Tran As String 'SUM, AVER, MAX, MIN, LAST
  SigDig As Long 'significant digits
  DecPla As Long 'decimal places
End Type

'Private Cons As FastCollection 'of ConsType
'Private Sources As FastCollection 'of land segs, pt. srcs., atmos. dep., etc...

Private Property Set ATCclsAnalysis_DataCollection(newColl As Collection)
'  Dim i&, Ind&, lstr$, lCons As ConsType

  Set pColl = Nothing
  Set pColl = newColl

'  Set Sources = Nothing
'  Set Cons = Nothing
'  Set Sources = New FastCollection
'  Set Cons = New FastCollection
'  For i = 1 To pColl.Count
'    lstr = pColl(i).attrib("IDLOCN")
'    Ind = Sources.IndexFromKey(lstr)
'    If Ind < 1 Then 'don't have this source yet
'      Sources.Add lstr, lstr
'    End If
'    lstr = pColl(i).attrib("IDCONS")
'    Ind = Cons.IndexFromKey(lstr)
'    If Ind < 1 Then 'don't have this constituent yet
'      lCons.DecPla = 2
'      lCons.SigDig = 5
'      lCons.Tran = "AVE"
'      Cons.Add lCons, lstr
'    End If
'  Next i
End Property

Private Property Get ATCclsAnalysis_Description() As String
  ATCclsAnalysis_Description = pLabel
End Property

Private Function ATCclsAnalysis_EditSpecification() As Boolean
  SendMonitorMessage "(MSG1 Editing Specification for " & pLabel & ")"

  Set frmReportSpecs.Report = Me
  
  'frmReportSpecs.FormFg = FormFg
  frmReportSpecs.Title = Title
  frmReportSpecs.CWid = CWid
  frmReportSpecs.SDig = SDig
  frmReportSpecs.dpla = dpla
  Set frmReportSpecs.TSerColl = pColl
  If frmReportSpecs.EditReportSpecs Then
    'FormFg = frmReportSpecs.FormFg
    'Title = frmReportSpecs.Title
    CWid = frmReportSpecs.CWid
    SDig = frmReportSpecs.SDig
    dpla = frmReportSpecs.dpla
    PageSumm = frmReportSpecs.Summ(0)
    ColSumm = frmReportSpecs.Summ(1)
    RowSumm = frmReportSpecs.Summ(2)
    FName = frmReportSpecs.FName
    ATCclsAnalysis_EditSpecification = True
  Else
    ATCclsAnalysis_EditSpecification = False
  End If
  Unload frmReportSpecs

End Function

Private Property Get ATCclsAnalysis_ErrorDescription() As String
  ATCclsAnalysis_ErrorDescription = pErrorDesc
  pErrorDesc = ""
End Property

Private Sub ATCclsAnalysis_Go()
  Dim i&, j&, of&, DateInt&
  Dim TSerInd() As Long
  Dim vTSer As Variant
  Dim v As Single, d As String
  Dim timefg(2) As Boolean
  Dim PageCnt&, RowCnt&, ColCnt&
  Dim Pages As CollString, Rows As CollString, Cols As CollString

  SendMonitorMessage "(OPEN Generating Report: " & pLabel & ")"

  'MsgBox "Generating report: " & pLabel
  of = FreeFile(0)
  Open FName For Output As of
  Print #of, Title
  SetHdrs PageSumm.Attribute, Pages, timefg(0)
  SetHdrs RowSumm.Attribute, Rows, timefg(1)
  SetHdrs ColSumm.Attribute, Cols, timefg(2)
  If timefg(0) Then
    PageCnt = 1
  Else
    PageCnt = Pages.Count
  End If
  If timefg(1) Then
    RowCnt = 1
  Else
    RowCnt = Rows.Count
  End If
  If timefg(2) Then
    ColCnt = 1
  Else
    ColCnt = Cols.Count
  End If
  ReDim TSerInd(PageCnt, RowCnt, ColCnt)
  SetTSerIndices PageCnt, RowCnt, ColCnt, Pages, Rows, Cols, timefg, TSerInd
  
  PrintPages of, Pages, Rows, Cols, timefg, TSerInd
  

'initial code for doing a database report
'  For i = 1 To Cons.Count
'    For Each vTSer In pColl
'      If vTSer.attrib("IDCONS") = Cons.Key(i) Then 'output this tser
'        J2Date vTSer.dates.Value(1), lDate
'        DateInt = DateIntrvl(lDate)
'        For j = 1 To vTSer.dates.summary.nvals
'          d = BuildDate(vTSer.dates.Value(j), DateInt)
'          v = NumFmted(vTSer.Value(j), CWid, Cons.ItemByIndex(i).DecPla)
'          If FormFg = 0 Then 'database records
'            Print #of, vTSer.attrib("IDCONS"), vTSer.attrib("IDLOCN"), d, v
'          ElseIf FormFg = 1 Then 'table
'          End If
'        Next j
'      End If
'    Next
'  Next i
  Close #of

  SendMonitorMessage "(CLOSE)"
End Sub

Private Sub SetHdrs(AttName As String, Hdrs As CollString, timefg As Boolean)
  Dim i&, DateStr As String, lDate&(5), DateInt&

  Set Hdrs = Nothing
  Set Hdrs = New CollString
  If AttName = "Time" Then
    J2Date pColl(1).dates.Value(1), lDate
    DateInt = DateIntrvl(lDate)
    For i = 1 To pColl(1).dates.Summary.nVals
      DateStr = BuildDate(pColl(1).dates.Value(i), DateInt)
      Hdrs.Add DateStr, i
    Next i
    timefg = True
  Else
    Set Hdrs = uniqueAttributeValues(AttName, pColl)
    timefg = False
  End If

End Sub

Private Sub SetTSerIndices(PCnt As Long, RCnt As Long, CCnt As Long, Pages As CollString, Rows As CollString, Cols As CollString, timefg() As Boolean, TSerInd() As Long)
  Dim i&, PInd&, RInd&, CInd&

  For i = 1 To pColl.Count
    For PInd = 1 To PCnt
      If timefg(0) Or pColl(i).attrib(PageSumm.Attribute) = Pages.coll(PInd) Then
        For RInd = 1 To RCnt
          If timefg(1) Or pColl(i).attrib(RowSumm.Attribute) = Rows.coll(RInd) Then
            For CInd = 1 To CCnt
              If timefg(2) Or pColl(i).attrib(ColSumm.Attribute) = Cols.coll(CInd) Then
                'its a match (finally!)
                GoTo OuttaThisMess
              End If
            Next CInd
          End If
        Next RInd
      End If
    Next PInd
    'getting here means no match was found
    MsgBox "no match found - big problem!", vbExclamation
OuttaThisMess:
    If TSerInd(PInd, RInd, CInd) = 0 Then
      TSerInd(PInd, RInd, CInd) = i
    Else 'multiple tsers found for this page/row/col combination
      MsgBox "Multiple TSers exist for this combination of page, row, and column attributes" & vbCrLf & _
             "Page: " & Pages.coll(PInd) & vbCrLf & _
             "Row:  " & Rows.coll(RInd) & vbCrLf & _
             "Col:  " & Cols.coll(CInd), vbExclamation
    End If
  Next i
End Sub

Private Sub PrintPages(of As Long, Pages As CollString, Rows As CollString, Cols As CollString, timefg() As Boolean, TSInd() As Long)
  Dim i&, j&, k&, PInd&, RInd&, CInd&, VInd&, r&, c&
  Dim Page As Variant, Row As Variant, Col As Variant
  Dim RowStr As String
  Dim CurVal As Single, PageVals() As Single, PageSummVals() As Single

  ReDim PageVals(Rows.Count, Cols.Count)
  ReDim PageSummVals(5, Rows.Count, Cols.Count)
  For i = 0 To 4
    For j = 1 To Rows.Count
      For k = 1 To Cols.Count
        If i = 2 Then
          PageSummVals(i, j, k) = 1000000#
        ElseIf i = 3 Then
          PageSummVals(i, j, k) = -1000000#
        Else
          PageSummVals(i, j, k) = 0
        End If
      Next k
    Next j
  Next i
  If timefg(0) Then VInd = 1
  PInd = 1
  For Each Page In Pages
    Print #of,
    Print #of, Page 'print page header
    PrintColumnHeaders of, Cols, True
    If timefg(1) Then VInd = 1
    RInd = 1
    r = 1
    For Each Row In Rows
      If Len(Row) <= CWid Then
        RowStr = Row & Space(CWid - Len(Row))
      Else
        RowStr = Left(Row, CWid)
      End If
      If timefg(2) Then VInd = 1
      CInd = 1
      c = 1
      For Each Col In Cols
        If TSInd(PInd, RInd, CInd) > 0 Then
          CurVal = pColl(TSInd(PInd, RInd, CInd)).Value(VInd)
          RowStr = RowStr & " " & NumFmted(CurVal, CWid, dpla)
          PageVals(r, c) = CurVal
        Else 'no tser, thus no value
          RowStr = RowStr & Space(CWid + 1)
          PageVals(r, c) = NoValue
        End If
        If timefg(2) Then
          VInd = VInd + 1
        Else
          CInd = CInd + 1
        End If
        c = c + 1
      Next
      SummRow PageVals, r, RowStr
      Print #of, RowStr
      If timefg(1) Then
        VInd = VInd + 1
      Else
        RInd = RInd + 1
      End If
      r = r + 1
    Next
    SummCols of, PageVals, PageSummVals
    If timefg(0) Then
      VInd = VInd + 1
    Else
      PInd = PInd + 1
    End If
    Print #of,
  Next
  If PageSumm.sSum Then PrintPageSumms of, PageSummVals, 0, PageSumm.Attribute & " Sums", Cols, Rows
  If PageSumm.sAve Then PrintPageSumms of, PageSummVals, 1, PageSumm.Attribute & " Averages", Cols, Rows
  If PageSumm.sMin Then PrintPageSumms of, PageSummVals, 2, PageSumm.Attribute & " Minimums", Cols, Rows
  If PageSumm.sMax Then PrintPageSumms of, PageSummVals, 3, PageSumm.Attribute & " Maximums", Cols, Rows
  If PageSumm.sCnt Then PrintPageSumms of, PageSummVals, 4, PageSumm.Attribute & " Counts", Cols, Rows

End Sub

Private Sub PrintColumnHeaders(of As Long, Cols As CollString, IncludeSumms As Boolean)
  Dim Col As Variant, HdrStr As String
  
  'build column headers
  HdrStr = Space(CWid)
  For Each Col In Cols
    If Len(Col) <= CWid Then
      Col = Space(CWid - Len(Col)) & Col
    Else
      Col = Left(Col, CWid)
    End If
    HdrStr = HdrStr & " " & Col
  Next
  If IncludeSumms Then
    If RowSumm.sSum Then HdrStr = HdrStr & " " & Space(CWid - 3) & "Sum"
    If RowSumm.sAve Then HdrStr = HdrStr & " " & Space(CWid - 3) & "Ave"
    If RowSumm.sMin Then HdrStr = HdrStr & " " & Space(CWid - 3) & "Min"
    If RowSumm.sMax Then HdrStr = HdrStr & " " & Space(CWid - 3) & "Max"
    If RowSumm.sCnt Then HdrStr = HdrStr & " " & Space(CWid - 3) & "Cnt"
    Print #of, HdrStr 'print column headers
  End If
End Sub

Private Sub SummRow(PageVals() As Single, irow As Long, RowStr As String)
  Dim i&, Tot As Single, lMin As Single, lMax As Single, lCnt As Long
  If RowSumm.sAve Or RowSumm.sSum Or RowSumm.sMax Or RowSumm.sMin Or RowSumm.sCnt Then
    Tot = 0
    lCnt = 0
    lMin = 1000000#
    lMax = -1000000#
    For i = 1 To UBound(PageVals, 2)
      If PageVals(irow, i) > NoValue Then
        lCnt = lCnt + 1
        If RowSumm.sAve Or RowSumm.sSum Then
          Tot = Tot + PageVals(irow, i)
        End If
        If PageVals(irow, i) < lMin Then
          lMin = PageVals(irow, i)
        End If
        If PageVals(irow, i) > lMax Then
          lMax = PageVals(irow, i)
        End If
      End If
    Next i
    If lCnt > 0 Then
      If RowSumm.sSum Then RowStr = RowStr & " " & NumFmted(Tot, CWid, dpla)
      If RowSumm.sAve Then RowStr = RowStr & " " & NumFmted(Tot / lCnt, CWid, dpla)
      If RowSumm.sMin Then RowStr = RowStr & " " & NumFmted(lMin, CWid, dpla)
      If RowSumm.sMax Then RowStr = RowStr & " " & NumFmted(lMax, CWid, dpla)
      If RowSumm.sCnt Then RowStr = RowStr & " " & NumFmtI(lCnt, CWid)
    End If
  End If
  
End Sub

Private Sub SummCols(of As Long, PageVals() As Single, PageSummVals() As Single)
  Dim i&, j&, Tot As Single, lMin As Single, lMax As Single, lCnt As Long
  Dim SumStr(4) As String

  If ColSumm.sAve Or ColSumm.sSum Or ColSumm.sMax Or ColSumm.sMin Then Print #of,
  If ColSumm.sSum Then SumStr(0) = "Sum" & Space(CWid - 3)
  If ColSumm.sAve Then SumStr(1) = "Ave" & Space(CWid - 3)
  If ColSumm.sMin Then SumStr(2) = "Min" & Space(CWid - 3)
  If ColSumm.sMax Then SumStr(3) = "Max" & Space(CWid - 3)
  If ColSumm.sCnt Then SumStr(4) = "Cnt" & Space(CWid - 3)
  For i = 1 To UBound(PageVals, 2)
    lCnt = 0
    Tot = 0
    lMin = 1000000#
    lMax = -1000000#
    For j = 1 To UBound(PageVals)
      If PageVals(j, i) > NoValue Then
        lCnt = lCnt + 1
        If ColSumm.sAve Or ColSumm.sSum Then
          Tot = Tot + PageVals(j, i)
        End If
        If PageVals(j, i) < lMin Then
          lMin = PageVals(j, i)
        End If
        If PageVals(j, i) > lMax Then
          lMax = PageVals(j, i)
        End If
        'now do page summaries
        If PageSumm.sSum Or PageSumm.sAve Then PageSummVals(0, j, i) = PageSummVals(0, j, i) + PageVals(j, i)
        If PageVals(j, i) < PageSummVals(2, j, i) Then
          PageSummVals(2, j, i) = PageVals(j, i)
        End If
        If PageVals(j, i) > PageSummVals(3, j, i) Then
          PageSummVals(3, j, i) = PageVals(j, i)
        End If
        PageSummVals(4, j, i) = PageSummVals(4, j, i) + 1
      End If
    Next j
    If ColSumm.sSum Then SumStr(0) = SumStr(0) & " " & NumFmted(Tot, CWid, dpla)
    If ColSumm.sAve Then SumStr(1) = SumStr(1) & " " & NumFmted(Tot / lCnt, CWid, dpla)
    If ColSumm.sMin Then SumStr(2) = SumStr(2) & " " & NumFmted(lMin, CWid, dpla)
    If ColSumm.sMax Then SumStr(3) = SumStr(3) & " " & NumFmted(lMax, CWid, dpla)
    If ColSumm.sCnt Then SumStr(4) = SumStr(4) & " " & NumFmtI(lCnt, CWid)
  Next i
  If ColSumm.sSum Then Print #of, SumStr(0)
  If ColSumm.sAve Then Print #of, SumStr(1)
  If ColSumm.sMin Then Print #of, SumStr(2)
  If ColSumm.sMax Then Print #of, SumStr(3)
  If ColSumm.sCnt Then Print #of, SumStr(4)
End Sub

Private Sub PrintPageSumms(of As Long, PageSummVals() As Single, SummInd As Long, Title As String, Cols As CollString, Rows As CollString)
  Dim i&, j&, RowStr As String, Row As Variant

  Print #of,
  Print #of, Title
  PrintColumnHeaders of, Cols, False
  i = 1
  For Each Row In Rows
    If Len(Row) <= CWid Then
      RowStr = Row & Space(CWid - Len(Row))
    Else
      RowStr = Left(Row, CWid)
    End If
    For j = 1 To Cols.Count
      If PageSummVals(4, i, j) > 0 Then
        If SummInd = 1 Then
          RowStr = RowStr & " " & NumFmted(PageSummVals(0, i, j) / PageSummVals(4, i, j), CWid, dpla)
        ElseIf SummInd = 4 Then
          RowStr = RowStr & " " & NumFmtI(CInt(PageSummVals(SummInd, i, j)), CWid)
        Else
          RowStr = RowStr & " " & NumFmted(PageSummVals(SummInd, i, j), CWid, dpla)
        End If
      Else
        RowStr = RowStr & Space(CWid - 2) & "N/A"
      End If
    Next j
    Print #of, RowStr
    i = i + 1
  Next
  Print #of,
End Sub

Private Property Set ATCclsAnalysis_Monitor(ByVal newValue As Object)
  Set pMonitor = newValue
  pMonitorSet = True
End Property

Private Property Let ATCclsAnalysis_Specification(newValue As String)
  pSpecification = newValue
End Property
Private Property Get ATCclsAnalysis_Specification() As String
  ATCclsAnalysis_Specification = pSpecification
End Property

Private Sub Class_Initialize()
  Set pColl = New Collection
  'Set Sources = New FastCollection
  'Set Cons = New FastCollection
  FormFg = 1
  SDig = 5
  dpla = 2
  CWid = 10
  PCode = 5 'annual
  Title = "Land Surface Loads"
End Sub

Private Sub SendMonitorMessage(msg As String)
  If pMonitorSet Then
    pMonitor.SendMonitorMessage msg
  End If
End Sub
          
Private Function BuildDate(jDate As Double, DateInt As Long) As String
  Dim d As String, sep As String
  Dim lDate&(5), k&

  d = ""
  If DateInt > 0 Then
    J2Date jDate, lDate
    For k = 0 To DateInt - 1
      If k < 3 Then
        sep = "/"
      ElseIf k = 3 Then
        sep = " "
      Else
        sep = ":"
      End If
      d = d & lDate(k) & sep
    Next k
    d = Left(d, Len(d) - 1) 'trim rightmost separator
  End If
  BuildDate = d

End Function

