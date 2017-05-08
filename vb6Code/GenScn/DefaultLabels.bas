Attribute VB_Name = "modDefaultLabels"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
'Formerly sglabl

Sub DefaultLabels _
          (CDTran&, ConstInt As Boolean, _
           nts&, CScenm$(), clocnm$(), cconnm$(), Tu&, Dtran&, _
           which&(), typind&(), _
           cntcon&, cntsen&, cntloc&, _
           alab$, yrlab$, YLLab$, Titl$, clab$(), ctran$, ctunit$)

'creates labels for GenScn plots
' (from lib3.0\newaqt\sglabl

  Dim i&, slen&, llen&, clen&, j&, precfg&
  Dim uniSen$(), uniCon$(), uniLoc$()
  Dim ctmp$
  
  yrlab = ""
  YLLab = ""
  precfg = 0

  If ConstInt And CDTran <> 4 Then
    Select Case Dtran
      Case 0: ctran = "MEAN"
      Case 1: ctran = "SUMMED"
      Case 2: ctran = "MAXIMUM"
      Case 3: ctran = "MINIMUM"
    End Select
    Select Case Tu
      Case 1: ctunit = "SECONDLY"
      Case 2: ctunit = "MINUTELY"
      Case 3: ctunit = "HOURLY"
      Case 4: ctunit = "DAILY"
      Case 5: ctunit = "MONTHLY"
      Case 6: ctunit = "YEARLY"
    End Select
  Else
    ctran = ""
    ctunit = ""
  End If
  
  If nts = 1 Then
    'only one curve, put all info in title
    alab = " "
    yrlab = " "
    YLLab = cconnm(0)
    Titl = ctunit & " " & ctran & " " _
         & CScenm(0) & " " _
         & cconnm(0) & " at " _
         & clocnm(0)
    clab(0) = CScenm(0) & " " & clocnm(0)
    which(0) = 1
    typind(0) = 1
  Else
    'count occurances of locations
    Call strUni(clocnm, cntloc, uniLoc)
    'count occurances of constit
    Call strUni(cconnm, cntcon, uniCon)
    'count occurances of scenarios
    Call strUni(CScenm, cntsen, uniSen)
    For i = 0 To nts - 1
      typind(i) = i
    Next i
    For i = 0 To cntcon - 1
      ctmp = uniCon(i)
      If Left(ctmp, 2) = "PR" And cntcon > 1 Then
        'precip on aux axis
        which(i) = 3
        alab = ctmp
        'If precfg = 0 Then
          'reduce count of constit for moved prec
        '  cntcon = cntcon - 1
        'End If
        precfg = 1
      Else 'If YLLab = " " Or YLLab = CConnm(i) Then
        'we are plotting another constituent on left
        If Len(YLLab) > 0 Then
          YLLab = YLLab & ", " & ctmp
        Else
          YLLab = ctmp
        End If
        which(i) = 1 'this is ignored later
      'Else
      '  'put on right for lack of a better place
      '  YRLab = ctmp
      '  Which(i) = 2
      End If
    Next i
    If cntcon = 1 And cntloc = 1 And precfg = 0 Then
      'only scen varies
      Titl = ctunit & " " & ctran & " " & cconnm(1) & " at " & clocnm(1)
      For i = 0 To nts - 1
        clab(i) = CScenm(i)
      Next i
    ElseIf cntsen = 1 And cntloc = 1 Then
      'only const varies
      Titl = ctunit & " " & ctran & " " & CScenm(1) & " at " & clocnm(1)
      For i = 0 To nts - 1
        clab(i) = cconnm(i)
      Next i
    ElseIf cntsen = 1 And cntcon = 1 Then
      'only locn varies
      Titl = ctunit & " " & ctran & " " & CScenm(1) & " " & cconnm(1)
      For i = 0 To nts - 1
        clab(i) = clocnm(i)
      Next i
    ElseIf cntloc = 1 Then
      'scen and con vary
      Titl = "Analysis Plot for " & ctunit & " " & ctran & " at " & clocnm(1)
      For i = 0 To nts - 1
        clab(i) = CScenm(i) & " " & cconnm(i)
      Next i
    ElseIf cntcon = 1 Then
      'scen and loc vary
      Titl = "Analysis Plot for " & ctunit & " " & ctran & " " & cconnm(1)
      For i = 0 To nts - 1
        clab(i) = CScenm(i) & " " & clocnm(i)
      Next i
    ElseIf cntsen = 1 Then
      'locn and con vary
      Titl = "Analysis Plot for " & ctunit & " " & ctran & " " & CScenm(1)
      For i = 0 To nts - 1
        clab(i) = clocnm(i) & " " & cconnm(i)
      Next i
    Else
      'everything varies
      Titl = "Analysis Plot for " & ctunit & " " & ctran & " Values"
      'initialize curve labels scenario name, reach name
      For i = 0 To nts - 1
        'build headers for legend
        clab(i) = CScenm(i) & " " & clocnm(i) & " " & cconnm(i)
      Next i
    End If
  End If
  
  'trim extra blanks out of default title
  StrTrim Titl
  For i = 0 To nts - 1 'curve labels too
    StrTrim clab(i)
  Next i
  
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
