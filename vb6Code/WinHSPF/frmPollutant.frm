VERSION 5.00
Begin VB.Form frmPollutant 
   Caption         =   "WinHSPF - Pollutant Selection"
   ClientHeight    =   2928
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   7680
   Icon            =   "frmPollutant.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2928
   ScaleWidth      =   7680
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame2"
      Height          =   372
      Left            =   2520
      TabIndex        =   0
      Top             =   2400
      Width           =   2652
      Begin VB.CommandButton cmdPollutant 
         Caption         =   "&OK"
         Default         =   -1  'True
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Index           =   0
         Left            =   0
         TabIndex        =   2
         Top             =   0
         Width           =   1215
      End
      Begin VB.CommandButton cmdPollutant 
         Cancel          =   -1  'True
         Caption         =   "&Cancel"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Index           =   1
         Left            =   1440
         TabIndex        =   1
         Top             =   0
         Width           =   1215
      End
   End
   Begin ATCoCtl.ATCoSelectList aslGQual 
      Height          =   2172
      Left            =   120
      TabIndex        =   3
      Top             =   0
      Width           =   7452
      _ExtentX        =   13145
      _ExtentY        =   3831
      RightLabel      =   "Selected:"
      LeftLabel       =   "Available:"
   End
End
Attribute VB_Name = "frmPollutant"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdPollutant_Click(Index As Integer)
  Dim i, vPoll As Variant, lPoll As HspfPollutant, found As Boolean
  Dim j&, pcount&, icount&, rcount&
  
  'make sure not too many
  If Index = 0 Then
    pcount = 0
    icount = 0
    rcount = 0
    For i = 1 To aslGQual.RightCount
      For Each vPoll In myUci.Pollutants
        Set lPoll = vPoll
        If lPoll.Name = aslGQual.RightItem(i - 1) Then
          If lPoll.modeltype = "PIG" Then
            pcount = pcount + 1
            icount = icount + 1
            rcount = rcount + 1
          ElseIf lPoll.modeltype = "PIOnly" Then
            pcount = pcount + 1
            icount = icount + 1
          ElseIf lPoll.modeltype = "GOnly" Then
            rcount = rcount + 1
          End If
        End If
      Next vPoll
      For Each vPoll In defUci.Pollutants
        Set lPoll = vPoll
        If lPoll.Name = aslGQual.RightItem(i - 1) Then
          If lPoll.modeltype = "PIG" Then
            pcount = pcount + 1
            icount = icount + 1
            rcount = rcount + 1
          ElseIf lPoll.modeltype = "PIOnly" Then
            pcount = pcount + 1
            icount = icount + 1
          ElseIf lPoll.modeltype = "GOnly" Then
            rcount = rcount + 1
          End If
        End If
      Next vPoll
    Next i
    If pcount > 10 Or icount > 10 Then
      'too many p/i quals
      Index = -1
      myMsgBox.Show "The number of PERLND or IMPLND quality constituents exceeds the maximum." & vbCrLf & _
        "Remove some pollutants from the 'Selected' list.", "Pollutant Problem", "OK"
    End If
    If rcount > 3 Then
      'too many gquals
      Index = -1
      myMsgBox.Show "The number of General quality constituents exceeds the maximum." & vbCrLf & _
        "Remove some pollutants from the 'Selected' list.", "Pollutant Problem", "OK"
    End If
  End If
  
  If Index = 0 Then
    'okay
    For i = 1 To aslGQual.RightCount
      found = False
      For Each vPoll In myUci.Pollutants
        Set lPoll = vPoll
        If lPoll.Name = aslGQual.RightItem(i - 1) Then
          found = True
        End If
      Next vPoll
      For Each vPoll In defUci.Pollutants
        Set lPoll = vPoll
        If lPoll.Name = aslGQual.RightItem(i - 1) And _
          lPoll.modeltype = "DataIn" Then
          found = True
        End If
      Next vPoll
      If Not found Then
        'need to add
        myUci.Edited = True
        j = 1
        Do While j <= defUci.Pollutants.Count
          Set lPoll = defUci.Pollutants(j)
          If lPoll.Name = aslGQual.RightItem(i - 1) Then
            'add this one
            myUci.Pollutants.Add lPoll
            If Mid(lPoll.modeltype, 1, 4) = "Data" Then
              'instead of removing it, flag as in use
              defUci.Pollutants(j).modeltype = "DataIn"
            Else
              'remove the other types
              defUci.Pollutants.Remove j
            End If
            Exit Do
          Else
            j = j + 1
          End If
        Loop
      End If
    Next i
    
    j = 1
    Do While j <= myUci.Pollutants.Count
      Set lPoll = myUci.Pollutants(j)
      found = False
      For i = 1 To aslGQual.RightCount
        If lPoll.Name = aslGQual.RightItem(i - 1) Then
          found = True
        End If
      Next i
      If Not found Then
        'need to remove
        myUci.Edited = True
        'are there any associated ext targets?
        FindAndRemoveExtTargets j
        defUci.Pollutants.Add lPoll
        myUci.Pollutants.Remove j
      Else
        j = j + 1
      End If
    Loop
    
    For i = 1 To aslGQual.LeftCount
      For Each vPoll In defUci.Pollutants
        Set lPoll = vPoll
        If lPoll.Name = aslGQual.LeftItem(i - 1) Then
          'found in def list
          If lPoll.modeltype = "DataIn" Then
            lPoll.modeltype = "Data"
          End If
        End If
      Next vPoll
    Next i
    
  Else
    'cancel
  End If
  If Index = 0 Or Index = 1 Then
    Unload Me
  End If
End Sub

Private Sub Form_Load()
  Dim i&, j&
  
  aslGQual.ButtonVisible("add all") = False
  aslGQual.ButtonVisible("remove all") = False
  aslGQual.ButtonVisible("move up") = False
  aslGQual.ButtonVisible("move down") = False
  For i = 1 To myUci.Pollutants.Count
    aslGQual.LeftItemFastAdd myUci.Pollutants(i).Name
  Next i
  aslGQual.MoveAllRight
  
  If defUci.Pollutants.Count = 0 Then
    ReadPollutants
  End If
  
  j = 0
  For i = 1 To defUci.Pollutants.Count
    aslGQual.LeftItemFastAdd defUci.Pollutants(i).Name
    If defUci.Pollutants(i).modeltype = "DataIn" Then
      aslGQual.MoveRight j
    Else
      j = j + 1
    End If
  Next i
  
End Sub

Private Sub ReadPollutants()
  Dim delim$, quote$, lstr$, i&, tname$, amax&, tstr$, tcnt&
  Dim cend As Boolean, opend As Boolean, tabend As Boolean
  Dim ilen&, ctmp$, coptyp$, ctab$, opf&, opl&, sopl$, j&
  Dim vOper As Variant, lOper As HspfOperation, tOper As HspfOperation
  Dim ltable As HspfTable, lOpnBlk As HspfOpnBlk, ccons$
  Dim ptype&, itype&, rtype&, massend As Boolean
  Dim lML As HspfMassLink, istr$, found As Boolean
  Dim tempTable As HspfTable, thistable&, tempname$
  
  i = FreeFile(0)
  On Error GoTo ErrHandler
  tname = HSPFMain.W_STARTERPATH & "\" & "pollutants.txt"
  Open tname For Input As #i
  Do Until EOF(i)
    Line Input #i, lstr
    ilen = Len(lstr)
    
    If ilen > 6 Then
      If Left(lstr, 7) = "CONSTIT" Then
        'found start of a constituent
        
        Dim lPoll As New HspfPollutant
        ctmp = StrRetRem(lstr)
        ccons = lstr
        lPoll.Name = lstr
        ptype = 0
        itype = 0
        rtype = 0
        cend = False
        Do While Not cend
          Line Input #i, lstr
          lstr = Trim(lstr)
          ilen = Len(lstr)
          
          If Left(lstr, ilen) = "END CONSTIT" Then
            'found end of constituent
            cend = True
            lPoll.Id = defUci.Pollutants.Count + 1
            lPoll.Index = myUci.Pollutants.Count + 1
            If ptype = 1 And rtype = 1 Then
              lPoll.modeltype = "PIG"
            ElseIf ptype = 1 Then
              lPoll.modeltype = "PIOnly"
            ElseIf rtype = 1 Then
              lPoll.modeltype = "GOnly"
            Else
              lPoll.modeltype = "Data"
            End If
            found = False
            For j = 1 To myUci.Pollutants.Count
              If myUci.Pollutants(j).Name = lPoll.Name Then
                found = True
              End If
            Next j
            For j = 1 To defUci.Pollutants.Count
              If defUci.Pollutants(j).Name = lPoll.Name Then
                found = True
              End If
            Next j
            If found = False Then
              defUci.Pollutants.Add lPoll
            End If
            Set lPoll = Nothing
          Else
            coptyp = Left(lstr, ilen)
            If coptyp = "PERLND" Or coptyp = "IMPLND" Or coptyp = "RCHRES" Then
              'found start of an operation
              Set lOpnBlk = New HspfOpnBlk
              lOpnBlk.Name = coptyp
              Set lOpnBlk.Uci = defUci
              For Each vOper In myUci.OpnBlks(coptyp).Ids
                Set lOper = vOper
                lOpnBlk.Ids.Add lOper, coptyp & lOper.Id
              Next vOper
              For Each vOper In myUci.OpnBlks(coptyp).Ids
                Set lOper = vOper
                Set tOper = New HspfOperation
                tOper.Name = lOper.Name
                tOper.Id = lOper.Id
                tOper.Description = lOper.Description
                tOper.DefOpnId = DefaultOpnId(tOper, defUci)
                Set tOper.OpnBlk = lOpnBlk
                lPoll.Operations.Add tOper, coptyp & tOper.Id
              Next vOper
              
              opend = False
              Do While Not opend
                Line Input #i, lstr
                ilen = Len(RTrim(lstr))
                If Left(lstr, ilen) = "END " & coptyp Then
                  'found end of operation
                  opend = True
                ElseIf ilen > 0 Then
                  'found start of table
                  ctab = RTrim(Mid(lstr, 3))
                  
                  tabend = False
                  Do While Not tabend
                    Line Input #i, lstr
                    ilen = Len(lstr)
                    lstr = RTrim(lstr)
                    If ilen > 0 Then
                      If Left(lstr, ilen) = "  END " & ctab Then
                        'found end of table
                        tabend = True
                      Else
                        If InStr(1, lstr, "***") Then
                          'comment, ignore
                        Else
                          'found line of table
                          opf = Left(lstr, 5)
                          sopl = Trim(Mid(lstr, 6, 5))
                          If Len(sopl) > 0 Then
                            opl = sopl
                          Else
                            opl = opf
                          End If
                          For Each vOper In lPoll.Operations
                            Set lOper = vOper
                            If lOper.Name = coptyp Then
                              If opf = lOper.DefOpnId Or (opf <= lOper.DefOpnId And lOper.DefOpnId <= opl) Then
                                Set ltable = New HspfTable
                                Set ltable.Def = myMsg.BlockDefs(coptyp).TableDefs(ctab)
                                Set ltable.Opn = lOper
                                ltable.initTable lstr
                                If ltable.Name = "GQ-QALDATA" Then
                                  rtype = 1
                                ElseIf ltable.Name = "QUAL-PROPS" Then
                                  ptype = 1
                                  itype = 1
                                End If
                                'Set lTable.Opn = lOper
                                ltable.OccurCount = 1
                                ltable.OccurNum = 1
                                ltable.OccurIndex = 0
                                If Not lOper.TableExists(ltable.Name) Then
                                  lOper.Tables.Add ltable, ltable.Name
                                  If Not lPoll.TableExists(ltable.Name) Then
                                    lPoll.Tables.Add ltable, ltable.Name
                                  End If
                                Else
                                  'handle multiple occurs of this table
                                  Set tempTable = lOper.Tables(ltable.Name)
                                  thistable = tempTable.OccurCount + 1
                                  tempTable.OccurCount = thistable
                                  For j = 2 To thistable - 1
                                    tempname = ltable.Name & ":" & CStr(j)
                                    Set tempTable = lOper.Tables(tempname)
                                    tempTable.OccurCount = thistable
                                  Next j
                                  ltable.OccurCount = thistable
                                  ltable.OccurNum = thistable
                                  tempname = ltable.Name & ":" & CStr(thistable)
                                  lOper.Tables.Add ltable, tempname
                                  If Not lPoll.TableExists(tempname) Then
                                    lPoll.Tables.Add ltable, tempname
                                  End If
                                End If
                              End If
                            End If
                          Next vOper
                        End If
                      End If
                    End If
                  Loop
                  
                End If
              Loop
            ElseIf coptyp = "MASS-LINKS" Then
              massend = False
              Do While Not massend
                Line Input #i, lstr
                ilen = Len(lstr)
                If Left(lstr, ilen) = "END " & coptyp Then
                  'found end of masslinks
                  massend = True
                ElseIf ilen > 0 Then
                  'found a masslink
                  Set lML = New HspfMassLink
                  Set lML.Uci = defUci
                  lML.Source.volname = Trim(Mid(lstr, 1, 6))
                  lML.Source.group = Trim(Mid(lstr, 12, 6))
                  lML.Source.member = Trim(Mid(lstr, 19, 6))
                  istr = Trim(Mid(lstr, 26, 1))
                  If Len(istr) = 0 Then
                    lML.Source.MemSub1 = 0
                  Else
                    lML.Source.MemSub1 = CInt(istr)
                  End If
                  istr = Trim(Mid(lstr, 28, 1))
                  If Len(istr) = 0 Then
                    lML.Source.MemSub2 = 0
                  Else
                    lML.Source.MemSub2 = CInt(istr)
                  End If
                  istr = Trim(Mid(lstr, 30, 10))
                  If Len(istr) = 0 Then
                    lML.MFact = 1
                  Else
                    'lML.MFact = CSng(istr)
                    lML.MFact = istr
                  End If
                  lML.Target.volname = Trim(Mid(lstr, 44, 6))
                  lML.Target.group = Trim(Mid(lstr, 59, 6))
                  lML.Target.member = Trim(Mid(lstr, 66, 6))
                  istr = Trim(Mid(lstr, 73, 1))
                  If Len(istr) = 0 Then
                    lML.Target.MemSub1 = 0
                  Else
                    lML.Target.MemSub1 = CInt(istr)
                  End If
                  istr = Trim(Mid(lstr, 75, 1))
                  If Len(istr) = 0 Then
                    lML.Target.MemSub2 = 0
                  Else
                    lML.Target.MemSub2 = CInt(istr)
                  End If
                  lML.MassLinkID = myUci.MassLinks(1).FindMassLinkID(lML.Source.volname, lML.Target.volname)
                  lPoll.MassLinks.Add lML
                End If
              Loop
            End If
          End If
        Loop
      End If
    End If
  Loop
  Close #i
  Exit Sub
ErrHandler:
  If Err.Number = 53 Then
    MsgBox "File " & tname & " not found.", vbOKOnly, "Read Pollutant Problem"
  Else
    MsgBox Err.Description & vbCrLf & vbCrLf & ccons & " " & coptyp & " " & ctab, _
      vbOKOnly, "Read Pollutant Problem"
  End If
End Sub

Private Sub Form_Resize()
  If width > 200 And height > 1100 Then
    aslGQual.width = width - 200
    fraButtons.Left = width / 2 - fraButtons.width / 2
    fraButtons.Top = height - fraButtons.height - 450
    aslGQual.height = fraButtons.Top - 200
  End If
End Sub

Private Sub FindAndRemoveExtTargets(j&)
  Dim lConn As HspfConnection, tConn As HspfConnection
  Dim lPoll As HspfPollutant, tPoll As HspfPollutant
  Dim ltable As HspfTable, lOper As HspfOperation
  Dim thisindex&, pscount&, pcount&, iscount&, icount&
  Dim rcount&, i&, remflag As Boolean, k&
  Dim psflag As Boolean, pflag As Boolean, isflag As Boolean
  Dim iflag As Boolean, rflag As Boolean
    
  'figure out which gqual, pqual, iqual to look for
  Set lPoll = myUci.Pollutants(j)
  thisindex = lPoll.Index
  pscount = 0  'perlnd qual sed assoc count
  pcount = 0   'perlnd qual count
  iscount = 0  'implnd qual sed assoc count
  icount = 0   'implnd qual count
  rcount = 0   'gqual count
  For i = 1 To myUci.Pollutants.Count
    Set tPoll = myUci.Pollutants(i)
    If tPoll.Index <= thisindex Then
      For k = 1 To tPoll.Operations.Count
        If tPoll.Operations(k).Name = "PERLND" Then
          If tPoll.Operations(k).TableExists("QUAL-PROPS") Then
            Set ltable = tPoll.Operations(k).Tables("QUAL-PROPS")
            If ltable.Parms("QSDFG") = 1 Then
              pscount = pscount + 1
            End If
            pcount = pcount + 1
          End If
          Exit For
        End If
      Next k
      For k = 1 To tPoll.Operations.Count
        If tPoll.Operations(k).Name = "IMPLND" Then
          If tPoll.Operations(k).TableExists("QUAL-PROPS") Then
            Set ltable = tPoll.Operations(k).Tables("QUAL-PROPS")
            If ltable.Parms("QSDFG") = 1 Then
              iscount = iscount + 1
            End If
            icount = icount + 1
          End If
          Exit For
        End If
      Next k
      For k = 1 To tPoll.Operations.Count
        If tPoll.Operations(k).Name = "RCHRES" Then
          If tPoll.Operations(k).TableExists("GQ-QALDATA") Then
            rcount = rcount + 1
          End If
          Exit For
        End If
      Next k
    End If
  Next i

  'figure out if we are removing a pqual, iqual, rqual
  psflag = False   'removing a pqual sed assoc
  pflag = False    'removing a pqual
  isflag = False   'removing a iqual sed assoc
  iflag = False    'removing a iqual
  rflag = False    'removing a gqual
  For k = 1 To lPoll.Operations.Count
    If lPoll.Operations(k).Name = "PERLND" Then
      If lPoll.Operations(k).TableExists("QUAL-PROPS") Then
        Set ltable = lPoll.Operations(k).Tables("QUAL-PROPS")
        If ltable.Parms("QSDFG") = 1 Then
          psflag = True
        End If
        pflag = True
      End If
      Exit For
    End If
  Next k
  For k = 1 To lPoll.Operations.Count
    If lPoll.Operations(k).Name = "IMPLND" Then
      If lPoll.Operations(k).TableExists("QUAL-PROPS") Then
        Set ltable = lPoll.Operations(k).Tables("QUAL-PROPS")
        If ltable.Parms("QSDFG") = 1 Then
          isflag = True
        End If
        iflag = True
      End If
      Exit For
    End If
  Next k
  For k = 1 To lPoll.Operations.Count
    If lPoll.Operations(k).Name = "RCHRES" Then
      If lPoll.Operations(k).TableExists("GQ-QALDATA") Then
        rflag = True
      End If
      Exit For
    End If
  Next k
  
  'look through all ext targets for ones to remove
  k = 1
  Do While k <= myUci.Connections.Count
    Set lConn = myUci.Connections(k)
    remflag = False
    If lConn.typ = 4 Then
      If lConn.Source.volname = "RCHRES" And lConn.Source.group = "GQUAL" Then
        If lConn.Source.member = "TIQAL" Or lConn.Source.member = "DQAL" Or _
           lConn.Source.member = "RDQAL" Or lConn.Source.member = "RRQAL" Or _
           lConn.Source.member = "IDQAL" Or lConn.Source.member = "PDQAL" Or _
           lConn.Source.member = "GQADDR" Or lConn.Source.member = "GQADWT" Or _
           lConn.Source.member = "GQADEP" Or lConn.Source.member = "RODQAL" Or _
           lConn.Source.member = "TROQAL" Then
          If rflag And lConn.Source.MemSub1 = rcount Then
            remflag = True
          End If
        Else
          If rflag And lConn.Source.MemSub2 = rcount Then
            remflag = True
          End If
        End If
      ElseIf lConn.Source.volname = "PERLND" And lConn.Source.group = "PQUAL" Then
        If lConn.Source.member = "SOQSP" Or lConn.Source.member = "WASHQS" Or _
           lConn.Source.member = "SCRQS" Or lConn.Source.member = "SOQS" Then
          If psflag And lConn.Source.MemSub1 = pscount Then
            remflag = True
          End If
        Else
          If pflag And lConn.Source.MemSub1 = pcount Then
            remflag = True
          End If
        End If
      ElseIf lConn.Source.volname = "IMPLND" And lConn.Source.group = "IQUAL" Then
        If lConn.Source.member = "SOQSP" Or lConn.Source.member = "SOQS" Then
          If isflag And lConn.Source.MemSub1 = iscount Then
            remflag = True
          End If
        Else
          If iflag And lConn.Source.MemSub1 = icount Then
            remflag = True
          End If
        End If
      End If
    End If
    If remflag Then
      'remove the ext target from the uci and the operation
      myUci.Connections.Remove k
      Set lOper = myUci.OpnBlks(lConn.Source.volname).operfromid(lConn.Source.volid)
      i = 1
      Do While i <= lOper.Targets.Count
        Set tConn = lOper.Targets(i)
        If tConn.Target.volname = lConn.Target.volname And _
           tConn.Target.volid = lConn.Target.volid Then
          lOper.Targets.Remove i
        Else
          i = i + 1
        End If
      Loop
    Else
      k = k + 1
    End If
  Loop
  
  'look through all ext targets for ones to decrement
  k = 1
  Do While k <= myUci.Connections.Count
    Set lConn = myUci.Connections(k)
    If lConn.typ = 4 Then
      If lConn.Source.volname = "RCHRES" And lConn.Source.group = "GQUAL" Then
        If lConn.Source.member = "TIQAL" Or lConn.Source.member = "DQAL" Or _
           lConn.Source.member = "RDQAL" Or lConn.Source.member = "RRQAL" Or _
           lConn.Source.member = "IDQAL" Or lConn.Source.member = "PDQAL" Or _
           lConn.Source.member = "GQADDR" Or lConn.Source.member = "GQADWT" Or _
           lConn.Source.member = "GQADEP" Or lConn.Source.member = "RODQAL" Or _
           lConn.Source.member = "TROQAL" Then
          If rflag And lConn.Source.MemSub1 > rcount Then
            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
          End If
        Else
          If rflag And lConn.Source.MemSub2 > rcount Then
            lConn.Source.MemSub2 = lConn.Source.MemSub2 - 1
          End If
        End If
      ElseIf lConn.Source.volname = "PERLND" And lConn.Source.group = "PQUAL" Then
        If lConn.Source.member = "SOQSP" Or lConn.Source.member = "WASHQS" Or _
           lConn.Source.member = "SCRQS" Or lConn.Source.member = "SOQS" Then
          If psflag And lConn.Source.MemSub1 > pscount Then
            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
          End If
        Else
          If pflag And lConn.Source.MemSub1 > pcount Then
            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
          End If
        End If
      ElseIf lConn.Source.volname = "IMPLND" And lConn.Source.group = "IQUAL" Then
        If lConn.Source.member = "SOQSP" Or lConn.Source.member = "SOQS" Then
          If isflag And lConn.Source.MemSub1 = iscount Then
            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
          End If
        Else
          If iflag And lConn.Source.MemSub1 = icount Then
            lConn.Source.MemSub1 = lConn.Source.MemSub1 - 1
          End If
        End If
      End If
    End If
    If Not remflag Then
      k = k + 1
    End If
  Loop
  
End Sub
