VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfTable"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pDef As HspfTableDef
Dim pOccurCount As Long  'total number of occurances
Dim pOccurNum As Long    'nth occurance
Dim pOccurIndex As Long  'occurance with which this table is associated
Dim pComment As String
Dim pTableComment As String
Dim pParms As Collection 'of HSPFParm
Dim pOpn As HspfOperation
Dim pEditAllSimilar As Boolean
Dim pEdited As Boolean
Dim pSuppID As Long      '>0 indicates parms on this record are in supplemental file
Dim pCombineOK As Boolean

Public Property Let OccurCount(newOccurCount As Long)
  pOccurCount = newOccurCount
End Property
Public Property Get OccurCount() As Long
  OccurCount = pOccurCount
End Property

Public Property Let OccurNum(newOccurNum As Long)
  pOccurNum = newOccurNum
End Property
Public Property Get OccurNum() As Long
  OccurNum = pOccurNum
End Property

Public Property Let OccurIndex(newOccurIndex As Long)
  pOccurIndex = newOccurIndex
End Property
Public Property Get OccurIndex() As Long
  OccurIndex = pOccurIndex
End Property

Public Property Get Def() As HspfTableDef
  Set Def = pDef
End Property
Public Property Set Def(newDef As HspfTableDef)
  Set pDef = newDef
End Property

Public Property Get Comment() As String
  Comment = pComment
End Property
Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Public Property Get TableComment() As String
  TableComment = pTableComment
End Property
Public Property Let TableComment(newTableComment As String)
  pTableComment = newTableComment
End Property

Public Property Get Name() As String
  Name = pDef.Name
End Property

Public Property Get Parms() As Collection 'of HSPFParm
  Set Parms = pParms
End Property

Public Property Get Opn() As HspfOperation
  Set Opn = pOpn
End Property
Public Property Set Opn(newOpn As HspfOperation)
  Set pOpn = newOpn
End Property

Public Property Get Edited() As Boolean
  Edited = pEdited
End Property
Public Property Let Edited(newEdited As Boolean)
  pEdited = newEdited
  If newEdited Then pOpn.Edited = True
End Property

Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlTableEdit"
End Property

Public Property Get EditAllSimilar() As Boolean
  EditAllSimilar = pEditAllSimilar
End Property
Public Function EditAllSimilarChange(newEditAllSimilar As Boolean)
  EditAllSimilarChange = pEditAllSimilar
  pEditAllSimilar = newEditAllSimilar
End Function

Public Property Get Caption()
  Caption = pOpn.Name & ":" & Name
End Property

Public Property Let SuppID(newValue As Long)
  pSuppID = newValue
End Property
Public Property Get SuppID() As Long
  SuppID = pSuppID
End Property

Public Property Get CombineOK() As Boolean
  CombineOK = pCombineOK
End Property
Public Property Let CombineOK(newCombineOK As Boolean)
  pCombineOK = newCombineOK
End Property

Public Sub initTable(s As String)
  Dim lParm As HSPFParm
  Dim vParmDef As Variant
  Dim lParmDef As HSPFParmDef
  Dim unitfg As Long
  
  For Each vParmDef In pDef.ParmDefs
    Set lParmDef = vParmDef
    Set lParm = New HSPFParm
    Set lParm.Parent = Me
    Set lParm.Def = lParmDef
    lParm.Value = Trim(Mid(s, lParmDef.StartCol, lParmDef.Length))
    lParm.ValueAsRead = Mid(s, lParmDef.StartCol, lParmDef.Length)
    If lParm.ValueAsRead = "" And Len(s) > 0 Then lParm.ValueAsRead = " "
    If Len(lParm.Value) = 0 Then  'try default
      unitfg = pOpn.OpnBlk.Uci.GlobalBlock.emfg
      If unitfg = 1 Then
        lParm.Value = lParm.Def.Default
      Else
        lParm.Value = lParm.Def.MetricDefault
      End If
    End If
    If Len(lParm.Value) > 0 Then
      If lParm.Def.Typ = 1 Then
        If IsNumeric(lParm.Value) Then
          lParm.Value = CLng(lParm.Value)
        End If
      ElseIf lParm.Def.Typ = 2 Then
        If IsNumeric(lParm.Value) Then
          If lParm.Value > 10 ^ lParmDef.Length Then
            lParm.Value = NumFmtRE(lParm.Value, lParmDef.Length)
          ElseIf lParmDef.Length <= 5 Then
            If lParm.Value < 1 And lParm.Value > 0 Then
              'lParm.Value = Format(CSng(lParm.Value), ".###")
              lParm.Value = NumFmtRE(lParm.Value, lParmDef.Length)
            Else
              lParm.Value = Format(CSng(lParm.Value), "0.###")
            End If
          ElseIf Abs(lParm.Value) > 0 And Abs(lParm.Value) <= 10 ^ -4 Then   'pbd bug fix for small e formats
            lParm.Value = NumFmtRE(lParm.Value, lParmDef.Length)
          Else
            'lParm.Value = Format(CSng(lParm.Value), "0.####")   'pbd no need to limit to 4 decimal places
          End If
          'If lParmDef.Length <= 5 Then
          '  Debug.Print Trim(Mid(s, lParmDef.StartCol, lParmDef.Length)), lParm.Value
          'End If
        End If
      End If
    End If
    pParms.Add lParm, lParm.Name
  Next vParmDef
End Sub

Public Sub Edit()
  Dim iresp&
  iresp = 1
  If Me.Name = "PWAT-PARM1" Or Me.Name = "IWAT-PARM1" Or Me.Name = "HYDR-PARM1" Then
    'choose regular or deluxe version to edit
    iresp = myMsgBox.Show("Choose an option for editing this table.", Me.Name & " Edit Option", "+-&Basic", "&Enhanced")
  End If
  If iresp <> 2 Then
    editInit Me, Me.Opn.OpnBlk.Uci.icon
  ElseIf Me.Name = "PWAT-PARM1" Then
    frmPwatEdit.init Me, Me.Opn.Uci.icon
    frmPwatEdit.Show vbModal
  ElseIf Me.Name = "IWAT-PARM1" Then
    frmIwatEdit.init Me, Me.Opn.Uci.icon
    frmIwatEdit.Show vbModal
  ElseIf Me.Name = "HYDR-PARM1" Then
    frmHydrEdit.init Me, Me.Opn.Uci.icon
    frmHydrEdit.Show vbModal
  End If
End Sub

Public Sub WriteUciFile(f As Integer, Optional instance As Long = 0)
  Dim lOpn As HspfOperation, ltable As HspfTable, lParm As HSPFParm
  Dim s$, t$, ilen&, icon&, SuppStr$, tempValue$
  Dim i&, tname$, j&, r!, firstopn As Boolean
  Dim pFlg As Boolean, p$, units&
  Dim lFirstOccur&, lLastOccur&
  
  units = Me.Opn.OpnBlk.Uci.GlobalBlock.emfg
  
  If instance = 0 Then
    lFirstOccur = 1
    lLastOccur = pOccurCount
  Else
    lFirstOccur = instance
    lLastOccur = instance
  End If
  
  For i = lFirstOccur To lLastOccur
    If i = 1 Then
      tname = pDef.Name
    Else
      tname = pDef.Name & ":" & i
    End If
    Print #f, "  " & pDef.Name
    
    pFlg = False
    On Error GoTo noTableForThisOper:
    firstopn = True
    For j = 1 To pOpn.OpnBlk.Ids.Count
      Set lOpn = pOpn.OpnBlk.NthOper(j)
      'write values here
      If err Then Resume
      If Not (lOpn.TableExists(tname)) Then
        'no Table for this Operation
      Else
        Set ltable = lOpn.Tables(tname)
        s = myFormatI(lOpn.Id, 5) & Space(5)
        For Each lParm In ltable.Parms
          With lParm
            tempValue = lParm.Value
            s = s & Space(.Def.StartCol - Len(s) - 1) 'pad prev field
            If .Def.Typ = ATCoTxt Then 'left justify strings
              If .Def.Length < Len(tempValue) Then
                tempValue = Left(tempValue, .Def.Length)
              End If
              t = LTrim(tempValue)
            Else
              'not a string
              'compare format of this value with the format as read
              If NumericallyTheSame(.ValueAsRead, tempValue, .Def.Default) Then
                'use the value as read
                t = .ValueAsRead
              Else
                'right justify everything else
                t = Space(.Def.Length)
                If Len(tempValue) > .Def.Length Then
                  r = tempValue
                  If (Len(CStr(r)) = .Def.Length + 1) And tempValue < 1# Then
                    'just leave off leading zero
                    RSet t = Mid(CStr(r), 2)
                  Else
                    RSet t = NumFmtRE(tempValue, .Def.Length)
                  End If
                Else
                  RSet t = CStr(tempValue)
                End If
              End If
            End If
          End With
          s = s & t
        Next lParm
        If ltable.SuppID > 0 Then 'include supplemental file ID for this record
          SuppStr = "~" & ltable.SuppID & "~"
          s = Left(s, 10) & SuppStr & Mid(s, 11 + Len(SuppStr))
        End If
        If pFlg Then
          If compareTableString(1, 10, p, s) And ltable.CombineOK Then
            s = Left(p, 5) & Left(s, 5) & Right(s, Len(s) - 10)
          Else
            ilen = Len(p)
            If ilen > 80 Then
              'this is a multi line table
              If tname = "REPORT-CON" Then 'special case for this table
                icon = Me.Opn.Tables("REPORT-FLAGS").Parms("NCON")
                p = Mid(p, 1, 10 + (icon * 70))
              End If
              Call PrintMultiLine(f, p)
            Else
              Print #f, p
              If Len(ltable.Comment) > 0 Then 'an comment associated with this operation
                Print #f, ltable.Comment
              End If
            End If
          End If
        End If
        pFlg = True
        If firstopn Then
          If Len(lOpn.Tables(tname).Comment) > 0 Then  'an associated comment
            Print #f, lOpn.Tables(tname).Comment   'pbd
          Else
            If units = 1 Then
              Print #f, pDef.HeaderE
            Else
              Print #f, pDef.HeaderM
            End If
          End If
          firstopn = False
        End If
        p = s
        GoTo notMissingTableForThisOper:
      End If
noTableForThisOper:
      If pFlg Then 'record pending
        ilen = Len(p)
        If ilen > 80 Then
          'this is a multi line table
          If tname = "REPORT-CON" Then 'special case for this table
            icon = Me.Opn.Tables("REPORT-FLAGS").Parms("NCON")
            p = Mid(p, 1, 10 + (icon * 70))
          End If
          Call PrintMultiLine(f, p)
        Else
          Print #f, p
        End If
        pFlg = False
      End If
notMissingTableForThisOper:
    Next j
    If pFlg Then 'record pending
      ilen = Len(p)
      If ilen > 80 Then
        'this is a multi line table
        If tname = "REPORT-CON" Then 'special case for this table
          icon = Me.Opn.Tables("REPORT-FLAGS").Parms("NCON")
          p = Mid(p, 1, 10 + (icon * 70))
        End If
        Call PrintMultiLine(f, p)
      Else
        Print #f, p
      End If
    End If
    Print #f, "  END " & pDef.Name & vbCrLf
  Next i
End Sub

Private Sub Class_Initialize()
  Set pDef = New HspfTableDef
  Set pParms = New Collection
  pOccurCount = 0
  pEditAllSimilar = True
  pCombineOK = True
End Sub

Private Sub PrintMultiLine(f As Integer, p As String)
  Dim ilen&, nlines&, n&, nchar&, t$
  
  ilen = Len(p)
  Print #f, Mid(p, 1, 80)
  nlines = (ilen - 10) / 70
  If nlines > 3 Then 'make sure something in remaining lines
    p = RTrim(p)
    ilen = Len(p)
    nlines = (ilen - 10) / 70
  End If
  
  For n = 1 To nlines
    If n = nlines Then
      nchar = ilen - (n * 70) - 10
    Else
      nchar = 70
    End If
    If nchar > 0 Then
      t = Mid(p, 1, 10) & Mid(p, (n * 70) + 11, nchar)
      Print #f, t
    End If
  Next n
End Sub

Public Sub SetQualIndex(noccur&, Nqual&)
  Dim tabcnt&, i&, t$, p$, tname$
  
  t = ""
  Select Case pDef.Name
    Case "GQ-GENDECAY": t = "GQ-QALFG": p = "QALFG6"  'rchres
    Case "GQ-HYDPM": t = "GQ-QALFG": p = "QALFG1"
    Case "GQ-ROXPM": t = "GQ-QALFG": p = "QALFG2"
    Case "GQ-PHOTPM": t = "GQ-QALFG": p = "QALFG3"
    Case "GQ-CFGAS": t = "GQ-QALFG": p = "QALFG4"
    Case "GQ-BIOPM": t = "GQ-QALFG": p = "QALFG5"
    Case "MON-BIO": t = "GQ-QALFG": p = "QALFG5"
    Case "GQ-SEDDECAY": t = "GQ-QALFG": p = "QALFG7"
    Case "GQ-KD": t = "GQ-QALFG": p = "QALFG7"
    Case "GQ-ADRATE": t = "GQ-QALFG": p = "QALFG7"
    Case "GQ-ADTHETA": t = "GQ-QALFG": p = "QALFG7"
    Case "GQ-SEDCONC": t = "GQ-QALFG": p = "QALFG7"
    'Case "MON-WATEMP": t = "GQ-QALFG": p = QALFG1
    Case "MON-PHVAL": t = "GQ-QALFG": p = "QALFG1"
    Case "MON-ROXYGEN": t = "GQ-QALFG": p = "QALFG2"
    Case "GQ-ALPHA": t = "GQ-QALFG": p = "QALFG3"
    Case "GQ-GAMMA": t = "GQ-QALFG": p = "QALFG3"
    Case "GQ-DELTA": t = "GQ-QALFG": p = "QALFG3"
    Case "GQ-CLDFACT": t = "GQ-QALFG": p = "QALFG3"
    Case "MON-CLOUD": t = "GQ-QALFG": p = "QALFG3"
    Case "MON-SEDCONC": t = "GQ-QALFG": p = "QALFG3"
    Case "MON-PHYTO": t = "GQ-QALFG": p = "QALFG3"
    'Case "GQ-DAUGHTER": t = "GQ-QALFG": p = QALFG1
    Case "MON-SQOLIM": t = "QUAL-PROPS": p = "VQOFG" 'perlnd
    Case "MON-POTFW": t = "QUAL-PROPS": p = "VPFWFG"
    Case "MON-POTFS": t = "QUAL-PROPS": p = "VPFSFG"
    Case "MON-ACCUM": t = "QUAL-PROPS": p = "VQOFG"
    Case "MON-IFLW-CONC": t = "QUAL-PROPS": p = "VIQCFG"
    Case "MON-GRND-CONC": t = "QUAL-PROPS": p = "VAQCFG"
    Case "MON-SQOLIM": t = "QUAL-PROPS": p = "VQOFG" 'implnd
    Case "MON-POTFW": t = "QUAL-PROPS": p = "VPFWFG"
    Case "MON-ACCUM": t = "QUAL-PROPS": p = "VQOFG"
  End Select
      
  If Len(t) > 0 Then
    tabcnt = 0
    tname = t
    For i = 1 To Nqual
      If i > 1 Then
        tname = t & ":" & i
      End If
      If pOpn.TableExists(tname) Then
        If pOpn.Tables(tname).Parms(p) > 0 Then
          tabcnt = tabcnt + 1
          If tabcnt = noccur Then
            'this is the one this table belongs to
            pOccurIndex = i
          End If
        End If
      End If
    Next i
  End If
End Sub

Public Function TableNeededForAllQuals() As Boolean
  
  If pDef.Name = "QUAL-INPUT" Or pDef.Name = "GQ-QALFG" Or _
    pDef.Name = "GQ-FLG2" Or pDef.Name = "GQ-VALUES" Or _
    pDef.Name = "QUAL-PROPS" Or pDef.Name = "GQ-QALDATA" Then
    TableNeededForAllQuals = True
  Else
    TableNeededForAllQuals = False
  End If
  
End Function

Private Function NumericallyTheSame(ValueAsRead$, ValueStored$, ValueDefault$) As Boolean
  'see if the current table value is the same as the value as read from the uci
  '4. is the same as 4.0
  '"  " is the same as 1 if 1 is the default
  Dim rtemp1!, rtemp2!
  
  NumericallyTheSame = False
  If IsNumeric(ValueStored) Then
    If IsNumeric(ValueAsRead) Then
      'simple case
      rtemp1 = ValueAsRead
      rtemp2 = ValueStored
      If rtemp1 = rtemp2 Then
        NumericallyTheSame = True
      End If
    ElseIf Len(ValueAsRead) > 0 And Len(Trim(ValueAsRead)) = 0 Then
      'one or more blank characters
      'see if the value stored is the same as the default
      rtemp1 = ValueStored
      rtemp2 = ValueDefault
      If rtemp1 = rtemp2 Then
        'we can use the blanks
        NumericallyTheSame = True
      End If
    End If
  End If
End Function
