VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfMsg"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pMsgFileName As String
Dim pBlockDefs As Collection 'of HspfBlockDef
Dim pErrorDescription As String
'Dim pHspfEngine As Object
'Dim pHspfEngineSet As Boolean
Dim pTSGroupDefs As Collection 'of HspfTSGroupDefs

Public Property Set Monitor(ByVal O As Object)
  Set IPC = O
  If IPC Is Nothing Then IPCset = False Else IPCset = True
End Property

Public Property Get Name() As String
  Name = pMsgFileName
End Property

Public Property Let Name(newName As String)
  Dim myDb As Database
  
  Dim myBlkRs As Recordset, lBlock As HspfBlockDef
  Dim lSections As New Collection
  
  Dim mySecRs As Recordset, lSection As HspfSectionDef
  Dim critSection$
  Dim lTables As New Collection
  Dim lBlkTables As New Collection
  
  Dim myTabRs As Recordset, ltable As HspfTableDef
  Dim critTable$
  Dim lParms As New Collection
  
  Dim myParmRs As Recordset, lParm As HSPFParmDef
  Dim critParm$, lTyp$
  
  Dim myTSGroupRs As Recordset, lTSGroup As HspfTSGroupDef
  Dim lTSMembers As Collection
  Dim myTSMemberRs As Recordset, lTSMember As HspfTSMemberDef
  
  Dim lNumeric As Boolean
  Dim h As String
  Dim s As String
  
  Dim lBlkCount&, lBlkNow&
  
  On Error GoTo err:
  If Len(newName) = 0 Then newName = "HSPFmsg.mdb"
  If Len(Dir(newName)) = 0 Then
    Dim ff As New ATCoFindFile
    ff.SetRegistryInfo "HSPF", "MessageMDB", "Path"
    ff.SetDialogProperties "Please locate 'HSPFmsg.mdb' in a writable directory", "HSPFmsg.mdb"
    newName = ff.GetName
    Set ff = Nothing
  End If
  Set myDb = OpenDatabase(newName, , True)
  pMsgFileName = newName
  Set pBlockDefs = Nothing
  Set pBlockDefs = New Collection
  Set myBlkRs = myDb.OpenRecordset("BlockDefns", dbOpenDynaset)
  myBlkRs.MoveLast
  lBlkCount = myBlkRs.RecordCount
  myBlkRs.MoveFirst
  lBlkNow = 0
  While Not (myBlkRs.EOF)
    'progress bar (dumb)
    s = "(Progress " & lBlkNow * 100 / lBlkCount & ")"
    'IPC.SendMonitorMessage s
    lBlkNow = lBlkNow + 1
    
    Set lBlock = New HspfBlockDef
    lBlock.Id = myBlkRs("ID")
    lBlock.Name = myBlkRs("Name")
    Set lSections = Nothing
    Set lSections = New Collection
    Set lBlkTables = Nothing
    Set lBlkTables = New Collection
    Set mySecRs = myDb.OpenRecordset("SectionDefns", dbOpenDynaset)
    critSection = "BlockID = " & CStr(lBlock.Id)
    mySecRs.FindFirst (critSection)
    While Not (mySecRs.NoMatch)
      Set lSection = New HspfSectionDef
      lSection.Name = mySecRs("Name")
      If IPCset Then
        If lSection.Name <> "<NONE>" Then
          s = "(MSG3 Reading about " & lBlock.Name & ":" & lSection.Name & ")"
        Else
          s = "(MSG3 Reading about " & lBlock.Name & ")"
        End If
        'IPC.SendMonitorMessage s
      End If
      lSection.Id = mySecRs("ID")
      Set lTables = Nothing
      Set lTables = New Collection
      
      Set myTabRs = myDb.OpenRecordset("TableDefns", dbOpenDynaset)
      critTable = "SectionID = " & CStr(lSection.Id)
      myTabRs.FindFirst (critTable)
      While Not (myTabRs.NoMatch)
        Set ltable = New HspfTableDef
        ltable.Id = myTabRs(0)
        Set ltable.Parent = lSection
        ltable.Name = myTabRs(2)
        ltable.SGRP = myTabRs(3)
        ltable.NumOccur = myTabRs(4)
        ltable.HeaderE = myTabRs(5)
        ltable.HeaderM = myTabRs(6)
        If IsNull(myTabRs(7)) Then
          ltable.Define = " "
        Else
          ltable.Define = myTabRs(7)
        End If
        If myTabRs.Fields.Count < 9 Then
          ltable.OccurGroup = 0
        Else
          ltable.OccurGroup = myTabRs(8)
        End If
        Set lParms = Nothing
        Set lParms = New Collection
        
        critParm = "TableID = " & CStr(ltable.Id)
        Set myParmRs = myDb.OpenRecordset("Select * from ParmDefns where " & critParm, dbOpenDynaset)
        'myParmRs.FindFirst (critParm)
        While Not (myParmRs.EOF)
          Set lParm = New HSPFParmDef
          lParm.Name = myParmRs(2) 'Name
          lTyp = myParmRs(3) 'Type
          Select Case lTyp
            Case "I": lParm.Typ = ATCoInt: lNumeric = True
            Case "R": lParm.Typ = ATCoSng: lNumeric = True
            Case "C": lParm.Typ = ATCoTxt: lNumeric = False
            Case Else: lParm.Typ = NONE: lNumeric = False
          End Select
          lParm.StartCol = myParmRs(4)
          lParm.Length = myParmRs(5)
          If lNumeric Then
            lParm.Min = myParmRs(6)
            lParm.Max = myParmRs(7)
            If myParmRs.Fields.Count > 10 Then
              lParm.MetricMin = myParmRs(10)
              lParm.MetricMax = myParmRs(11)
            Else
              lParm.MetricMin = myParmRs(6)
              lParm.MetricMax = myParmRs(7)
            End If
          End If
          If IsNull(myParmRs(8)) Then
            lParm.Default = " "
          Else
            lParm.Default = myParmRs(8) 'default
          End If
          If myParmRs.Fields.Count > 10 Then
            If IsNull(myParmRs(12)) Then
              lParm.MetricDefault = " "
            Else
              lParm.MetricDefault = myParmRs(12) 'default
            End If
          Else 'use english default
            If IsNull(myParmRs(8)) Then
              lParm.MetricDefault = " "
            Else
              lParm.MetricDefault = myParmRs(8)
            End If
          End If
          lParm.Other = myParmRs(4) & ":" & myParmRs(5)
          If IsNull(myParmRs(9)) Then
            lParm.Define = " "
          Else
            lParm.Define = myParmRs(9)
          End If
          lParms.Add lParm, lParm.Name
          myParmRs.MoveNext ' .FindNext (critParm)
        Wend
        myParmRs.Close
        Set ltable.ParmDefs = lParms
        updateParmsMultLines lBlock.Name, ltable
        lTables.Add ltable, ltable.Name
        lBlkTables.Add ltable, ltable.Name
        myTabRs.FindNext (critTable)
      Wend
      myTabRs.Close
      Set lSection.TableDefs = lTables
      lSections.Add lSection, lSection.Name
      mySecRs.FindNext (critSection)
    Wend
    mySecRs.Close
    Set lBlock.SectionDefs = lSections
    Set lBlock.TableDefs = lBlkTables
    pBlockDefs.Add lBlock, lBlock.Name
    myBlkRs.MoveNext
  Wend
  myBlkRs.Close
  
  'now read TS group and member info
  Set pTSGroupDefs = Nothing
  Set pTSGroupDefs = New Collection
  Set myTSGroupRs = myDb.OpenRecordset("TSGroupDefns", dbOpenDynaset)
  While Not (myTSGroupRs.EOF)
    Set lTSGroup = New HspfTSGroupDef
    lTSGroup.Id = myTSGroupRs("ID")
    lTSGroup.Name = myTSGroupRs("Name")
    If IPCset Then
      s = "(MSG3 Reading about Timeseries Groups and Members for " & lTSGroup.Name & ")"
      'IPC.SendMonitorMessage s
    End If
    lTSGroup.BlockId = myTSGroupRs("BlockID")
    Set lTSMembers = Nothing
    Set lTSMembers = New Collection
    Set myTSMemberRs = myDb.OpenRecordset("TSMemberDefns", dbOpenDynaset)
    critSection = "TSGroupID = " & CStr(lTSGroup.Id)
    myTSMemberRs.FindFirst (critSection)
    While Not (myTSMemberRs.NoMatch)
      Set lTSMember = New HspfTSMemberDef
      lTSMember.Id = myTSMemberRs("ID")
      lTSMember.Name = myTSMemberRs("Name")
      lTSMember.TSGroupId = myTSMemberRs("TSGroupID")
      Set lTSMember.Parent = lTSGroup
      lTSMember.SCLU = myTSMemberRs("SCLU")
      lTSMember.SGRP = myTSMemberRs("SGRP")
      lTSMember.mdim1 = FilterNull(myTSMemberRs("mdim1"))
      lTSMember.mdim2 = FilterNull(myTSMemberRs("mdim2"))
      lTSMember.maxsb1 = FilterNull(myTSMemberRs("maxsb1"))
      lTSMember.maxsb2 = FilterNull(myTSMemberRs("maxsb2"))
      lTSMember.mkind = FilterNull(myTSMemberRs("mkind"))
      lTSMember.sptrn = FilterNull(myTSMemberRs("sptrn"))
      lTSMember.msect = FilterNull(myTSMemberRs("msect"))
      lTSMember.mio = FilterNull(myTSMemberRs("mio"))
      lTSMember.osvbas = FilterNull(myTSMemberRs("osvbas"))
      lTSMember.osvoff = FilterNull(myTSMemberRs("osvoff"))
      lTSMember.eunits = FilterNull(myTSMemberRs("eunits"), " ")
      lTSMember.ltval1 = FilterNull(myTSMemberRs("ltval1"))
      lTSMember.ltval2 = FilterNull(myTSMemberRs("ltval2"))
      lTSMember.ltval3 = FilterNull(myTSMemberRs("ltval3"))
      lTSMember.ltval4 = FilterNull(myTSMemberRs("ltval4"))
      lTSMember.Defn = FilterNull(myTSMemberRs("defn"), " ")
      lTSMember.munits = FilterNull(myTSMemberRs("munits"), " ")
      lTSMember.ltval5 = FilterNull(myTSMemberRs("ltval5"))
      lTSMember.ltval6 = FilterNull(myTSMemberRs("ltval6"))
      lTSMember.ltval7 = FilterNull(myTSMemberRs("ltval7"))
      lTSMember.ltval8 = FilterNull(myTSMemberRs("ltval8"))
      lTSMembers.Add lTSMember, lTSMember.Name
      myTSMemberRs.FindNext (critSection)
    Wend
    myTSMemberRs.Close
    Set lTSGroup.MemberDefs = lTSMembers
    pTSGroupDefs.Add lTSGroup, CStr(lTSGroup.Id)
    myTSGroupRs.MoveNext
  Wend
  myTSGroupRs.Close
  
  myDb.Close
  'If IPCset Then IPC.SendMonitorMessage "(MSG3 )"
  Exit Property
err:
  pErrorDescription = "HspfMsg:Name:" & err.Description
End Property
Private Function FilterNull(v, Optional NullReturn = 0) As Variant
  If IsNull(v) Then
    FilterNull = NullReturn
  Else
    FilterNull = v
  End If
End Function

Private Sub updateParmsMultLines(blockname As String, ltable As HspfTableDef)
  Dim i&, lParm As HSPFParmDef, j&
  
  With ltable
    If blockname = "DURANL" And .Name = "LEVELS" Then
      For i = 1 To 6
        Set lParm = New HSPFParmDef
        lParm.Name = "LEVE" & CStr(15 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 76 + (i * 5)
        lParm.Length = 5
        lParm.Min = -999
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = "LEVEL(2thru21) contains the 20 possible user-specified levels for which the input time series will be analyzed."
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "DURANL" And .Name = "LCONC" Then
      For i = 1 To 3 'three fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "LCONC" & CStr(7 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 71 + (i * 10)
        lParm.Length = 10
        lParm.Min = -999
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "PERLND" And .Name = "IRRIG-SCHED" Then
      For i = 2 To 10 'up to 10 rows possible
        Set lParm = New HSPFParmDef
        lParm.Name = "IRYR" & CStr((2 * (i - 1)) + 1) 'year
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 12
        lParm.Length = 4
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRMO" & CStr((2 * (i - 1)) + 1) 'month
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 17
        lParm.Length = 2
        lParm.Min = 1
        lParm.Max = 12
        lParm.Default = 1
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRDY" & CStr((2 * (i - 1)) + 1) 'day
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 20
        lParm.Length = 2
        lParm.Min = 1
        lParm.Max = 31
        lParm.Default = 1
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRHR" & CStr((2 * (i - 1)) + 1) 'hour
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 23
        lParm.Length = 2
        lParm.Min = 0
        lParm.Max = 24
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRMI" & CStr((2 * (i - 1)) + 1) 'min
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 26
        lParm.Length = 2
        lParm.Min = 0
        lParm.Max = 60
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRDUR" & CStr((2 * (i - 1)) + 1) 'duration
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 28
        lParm.Length = 5
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRRAT" & CStr((2 * (i - 1)) + 1) 'rate
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 33
        lParm.Length = 10
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRYR" & CStr(2 * i) '2nd year
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 49
        lParm.Length = 4
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRMO" & CStr(2 * i) 'month
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 54
        lParm.Length = 2
        lParm.Min = 1
        lParm.Max = 12
        lParm.Default = 1
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRDY" & CStr(2 * i) 'day
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 57
        lParm.Length = 2
        lParm.Min = 1
        lParm.Max = 31
        lParm.Default = 1
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRHR" & CStr(2 * i) 'hour
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 60
        lParm.Length = 2
        lParm.Min = 0
        lParm.Max = 24
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRMI" & CStr(2 * i) 'min
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 63
        lParm.Length = 2
        lParm.Min = 0
        lParm.Max = 60
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRDUR" & CStr(2 * i) 'duration
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 65
        lParm.Length = 5
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "IRRAT" & CStr(2 * i) 'rate
        lParm.Typ = ATCoSng
        lParm.StartCol = (70 * (i - 1)) + 70
        lParm.Length = 10
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "RCHRES" And .Name = "HT-BED-DELH" Then
      For i = 2 To 14 '100 values needed
        For j = 1 To 7 '
          Set lParm = New HSPFParmDef
          lParm.Name = "DELH" & CStr((7 * (i - 1)) + j)
          lParm.Typ = ATCoSng
          lParm.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
          lParm.Length = 10
          lParm.Min = -999
          lParm.Max = -999
          lParm.Default = 0
          lParm.Other = lParm.StartCol & ":" & lParm.Length
          lParm.Define = ""
          .ParmDefs.Add lParm, lParm.Name
        Next j
      Next i
      For i = 1 To 2 'two more fields to tack on to make 100
        Set lParm = New HSPFParmDef
        lParm.Name = "DELH" & CStr(98 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 991 + (10 * (i - 1))
        lParm.Length = 10
        lParm.Min = -999
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "RCHRES" And .Name = "HT-BED-DELTT" Then
      For i = 2 To 14 '100 values needed
        For j = 1 To 7 '
          Set lParm = New HSPFParmDef
          lParm.Name = "DELTT" & CStr((7 * (i - 1)) + j)
          lParm.Typ = ATCoSng
          lParm.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
          lParm.Length = 10
          lParm.Min = -999
          lParm.Max = -999
          lParm.Default = 0
          lParm.Other = lParm.StartCol & ":" & lParm.Length
          lParm.Define = ""
          .ParmDefs.Add lParm, lParm.Name
        Next j
      Next i
      For i = 1 To 2 'two more fields to tack on to make 100
        Set lParm = New HSPFParmDef
        lParm.Name = "DELTT" & CStr(98 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 991 + (10 * (i - 1))
        lParm.Length = 10
        lParm.Min = -999
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "RCHRES" And .Name = "GQ-PHOTPM" Then
      For i = 1 To 7 'seven fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "PHOTPM" & CStr(7 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 71 + (i * 10)
        lParm.Length = 10
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
      For i = 1 To 6 'six more fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "PHOTPM" & CStr(14 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 141 + (i * 10)
        lParm.Length = 10
        If i < 5 Then
          lParm.Min = 0
          lParm.Max = -999
          lParm.Default = 0
        ElseIf i = 5 Then
          lParm.Min = 0.0001
          lParm.Max = 10
          lParm.Default = 1
        Else
          lParm.Min = 1
          lParm.Max = 2
          lParm.Default = 1
        End If
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "RCHRES" And .Name = "GQ-ALPHA" Then
      For i = 1 To 7 'seven fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "ALPH" & CStr(7 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 71 + (i * 10)
        lParm.Length = 10
        lParm.Min = 0.00001
        lParm.Max = -999
        lParm.Default = -999
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
      For i = 1 To 4 'four more fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "ALPH" & CStr(14 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 141 + (i * 10)
        lParm.Length = 10
        lParm.Min = 0.00001
        lParm.Max = -999
        lParm.Default = -999
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "RCHRES" And .Name = "GQ-GAMMA" Then
      For i = 1 To 7 'seven fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "GAMM" & CStr(7 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 71 + (i * 10)
        lParm.Length = 10
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
      For i = 1 To 4 'four more fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "GAMM" & CStr(14 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 141 + (i * 10)
        lParm.Length = 10
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "RCHRES" And .Name = "GQ-DELTA" Then
      For i = 1 To 7 'seven fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "DEL" & CStr(7 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 71 + (i * 10)
        lParm.Length = 10
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
      For i = 1 To 4 'four more fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "DEL" & CStr(14 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 141 + (i * 10)
        lParm.Length = 10
        lParm.Min = 0
        lParm.Max = -999
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "RCHRES" And .Name = "GQ-CLDFACT" Then
      For i = 1 To 7 'seven fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "KCLD" & CStr(7 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 71 + (i * 10)
        lParm.Length = 10
        lParm.Min = 0
        lParm.Max = 1
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
      For i = 1 To 4 'four more fields to tack on
        Set lParm = New HSPFParmDef
        lParm.Name = "KCLD" & CStr(14 + i) 'Name
        lParm.Typ = ATCoSng
        lParm.StartCol = 141 + (i * 10)
        lParm.Length = 10
        lParm.Min = 0
        lParm.Max = 1
        lParm.Default = 0
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "RCHRES" And .Name = "GQ-DAUGHTER" Then
      For i = 2 To 3 '3 rows needed
        For j = 1 To 3 'three values per row
          Set lParm = New HSPFParmDef
          lParm.Name = "ZERO" & CStr(i) & CStr(j)
          lParm.Typ = ATCoSng
          lParm.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
          lParm.Length = 10
          lParm.Min = 0
          lParm.Max = -999
          lParm.Default = 0
          lParm.Other = lParm.StartCol & ":" & lParm.Length
          lParm.Define = ""
          .ParmDefs.Add lParm, lParm.Name
        Next j
      Next i
    ElseIf blockname = "REPORT" And .Name = "REPORT-SRC" Then
      For i = 2 To 25 'up to 25 rows possible
        Set lParm = New HSPFParmDef
        lParm.Name = "SRCID" & CStr(i) 'Name
        lParm.Typ = ATCoTxt
        lParm.StartCol = (70 * (i - 1)) + 11
        lParm.Length = 20
        lParm.Default = ""
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    ElseIf blockname = "REPORT" And .Name = "REPORT-CON" Then
      For i = 2 To 20 'up to 20 rows possible
        Set lParm = New HSPFParmDef
        lParm.Name = "CONID" & CStr(i) 'Name
        lParm.Typ = ATCoTxt
        lParm.StartCol = ((i - 1) * 70) + 11
        lParm.Length = 20
        lParm.Default = ""
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "TRAN" & CStr(i) 'tran
        lParm.Typ = ATCoTxt
        lParm.StartCol = (70 * (i - 1)) + 32
        lParm.Length = 4
        lParm.Default = "SUM"
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "SIGD" & CStr(i) 'sig digits
        lParm.Typ = ATCoInt
        lParm.StartCol = (70 * (i - 1)) + 36
        lParm.Length = 5
        lParm.Min = 2
        lParm.Max = 5
        lParm.Default = 5
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
        Set lParm = New HSPFParmDef
        lParm.Name = "DECPLA" & CStr(i) 'dec places
        lParm.Typ = ATCoInt
        lParm.StartCol = (70 * (i - 1)) + 41
        lParm.Length = 5
        lParm.Min = 0
        lParm.Max = 3
        lParm.Default = 2
        lParm.Other = lParm.StartCol & ":" & lParm.Length
        lParm.Define = ""
        .ParmDefs.Add lParm, lParm.Name
      Next i
    End If
  End With
End Sub

Public Property Get BlockDefs() As Collection 'of HspfBlockDef
   Set BlockDefs = pBlockDefs
End Property

Public Property Get TSGroupDefs() As Collection 'of HspfTSGroupDef
   Set TSGroupDefs = pTSGroupDefs
End Property

Public Property Get ErrorDescription() As String
  ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property
