Attribute VB_Name = "Sglabel"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Sub sglabl _
          (ndsn&, cscenm$(), clocnm$(), cconnm$(), Tu&, Dtran&, _
           which&(), typind&(), _
           cntcon&, cntsen&, cntloc&, _
           alab$, yrlab$, yllab$, titl$, clab$(), ctran$, ctunit$)

'creates labels for GenScn plots
' (from lib3.0\newaqt\sglabl

'     INTEGER       NDSN,CNTCON,WHICH(NDSN),TYPIND(NDSN),
'                   CNTSEN , CNTLOC, TU, DTRAN
'     CHARACTER*8   CSCENM(NDSN),CLOCNM(NDSN),CCONNM(NDSN),
'                   CTRAN , CTUNIT
'     CHARACTER*20  CLAB(NDSN)
'     CHARACTER*80  YRLAB,YLLAB,ALAB
'      CHARACTER*240 TITL

'     + + + ARGUMENT DEFINITIONS + + +
'     NDSN   - number of data sets for which to set labels
'     CNTCON - count of constituents
'     CNTSEN - count of scenarios
'     CNTLOC - count of locations
'     WHICH  - which axis flag
'     TYPIND -
'     CSCENM - scenario names
'     CLOCNM - location names
'     CCONNM - constituent names
'     CLAB   - plot label
'     YRLAB  - right axis label
'     YLLAB  - left axis label
'     ALAB   - aux axis label
'     TITL   - title of plot
'     TU     - time units for data
'     DTRAN  - transformation function for data
'     CTRAN  -
'     CTUNIT -
  Dim i&, slen&, llen&, clen&, j&, precfg&
  Dim uniSen$(), uniCon$(), uniLoc$()
  Dim ctmp$
  
  yrlab = " "
  yllab = " "
  precfg = 0

  Select Case Dtran
    Case 0: ctran = "MEAN   "
    Case 1: ctran = "SUMMED "
    Case 2: ctran = "MAXIMUM"
    Case 3: ctran = "MINIMUM"
  End Select
  
  Select Case Tu
    Case 1: ctunit = "SECONDLY"
    Case 2: ctunit = "MINUTELY"
    Case 3: ctunit = "HOURLY  "
    Case 4: ctunit = "DAILY   "
    Case 5: ctunit = "MONTHLY "
    Case 6: ctunit = "YEARLY  "
  End Select

  If ndsn = 1 Then
    'only one curve, put all info in title
    alab = " "
    yrlab = " "
    yllab = cconnm(0)
    titl = ctunit & " " & ctran & " " _
         & cscenm(0) & " " _
         & cconnm(0) & " at " _
         & clocnm(0)
    clab(0) = cscenm(0) & " " & clocnm(0)
    which(0) = 1
    typind(0) = 1
  Else
    'count occurances of locations
    Call strUni(clocnm, cntloc, uniLoc)
    'count occurances of constit
    Call strUni(cconnm, cntcon, uniCon)
    'count occurances of scenarios
    Call strUni(cscenm, cntsen, uniSen)
    For i = 0 To ndsn - 1
      ctmp = cconnm(i)
      If Left(ctmp, 2) = "PR" And cntcon > 1 Then
        'precip on aux axis
        which(i) = 3
        alab = ctmp
        If precfg = 0 Then
          'reduce count of constit for moved prec
          cntcon = cntcon - 1
        End If
        precfg = 1
      ElseIf yllab = " " Or yllab = cconnm(i) Then
        'we are plotting another constituent on left
        yllab = cconnm(i)
        which(i) = 1
      Else
        'put on right for lack of a better place
        yrlab = cconnm(i)
        which(i) = 2
      End If
      typind(i) = i
    Next i

    If cntcon = 1 And cntloc = 1 And precfg = 0 Then
      'only scen varies
      titl = ctunit & " " & ctran & " " & cconnm(1) & " at " & clocnm(1)
      For i = 0 To ndsn - 1
        clab(i) = cscenm(i)
      Next i
    ElseIf cntsen = 1 And cntloc = 1 Then
      'only const varies
      titl = ctunit & " " & ctran & " " & cscenm(1) & " at " & clocnm(1)
      For i = 0 To ndsn - 1
        clab(i) = cconnm(i)
      Next i
    ElseIf cntsen = 1 And cntcon = 1 Then
      'only locn varies
      titl = ctunit & " " & ctran & " " & cscenm(1) & " " & cconnm(1)
      For i = 0 To ndsn - 1
        clab(i) = clocnm(i)
      Next i
    ElseIf cntloc = 1 Then
      'scen and con vary
      titl = "Analysis Plot for " & ctunit & " " & ctran & " at " & clocnm(1)
      For i = 0 To ndsn - 1
        clab(i) = cscenm(i) & " " & cconnm(i)
      Next i
    ElseIf cntcon = 1 Then
      'scen and loc vary
      titl = "Analysis Plot for " & ctunit & " " & ctran & " " & cconnm(1)
      For i = 0 To ndsn - 1
        clab(i) = cscenm(i) & " " & clocnm(i)
      Next i
    ElseIf cntsen = 1 Then
      'locn and con vary
      titl = "Analysis Plot for " & ctunit & " " & ctran & " " & cscenm(1)
      For i = 0 To ndsn - 1
        clab(i) = clocnm(i) & " " & cconnm(i)
      Next i
    Else
      'everything varies
      titl = "Analysis Plot for " & ctunit & " " & ctran & " Values"
      'initialize curve labels scenario name, reach name
      For i = 0 To ndsn - 1
        'build headers for legend
        clab(i) = cscenm(i) & " " & clocnm(i) & " " & cconnm(i)
      Next i
    End If
  End If
End Sub

Sub strUni(str$(), cnt&, ustr$())
' determine unique strings in str
'   STR    - strings to search
'   CNT    - count of unique strings
'   USTR   - unique strings
  Dim i&, j&, match As Boolean
  
  'first string is unique
  cnt = 1
  ReDim ustr(cnt)
  ustr(cnt - 1) = str(0)
  For i = 1 To UBound(str)
    'look thru other strings
    match = False
    For j = 1 To cnt
      'look thru existing uniques
      If (str(i) = ustr(j - 1)) Then
        'Not Unique
        match = True
        Exit For
      End If
    Next j
    If Not (match) Then 'first occurance
      cnt = cnt + 1
      ReDim Preserve ustr(cnt)
      ustr(cnt - 1) = str(i)
    End If
  Next i
End Sub
