VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPest"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pModelName As String
Private pUCI As HspfUci
Private pControlFile As String
Private pP2PFile As String
Private pBatchFile As String
Private pTSPROC As clsPestTSPROC
Private pPar2Par As clsPestPAR2PAR
Private pInitParmFile As clsPestParmFile   'initial parameter file (parm_start.dat)
Private pInitGroupFile As clsPestGroupFile 'initial group file (group_start.dat)
Private pCurParmFile As clsPestParmFile    'actual parameter file used in PEST run
Private pCurGroupFile As clsPestGroupFile  'actual group file used in PEST run
Private pIPC As ATCoIPC
Private pAUI As Boolean

Private pPestPath As String
Private pOriginalUCIPath As String

Public Property Get ModelName() As String
  ModelName = pModelName
End Property
Public Property Let ModelName(newvalue As String)
  pModelName = newvalue
End Property

Public Property Get UCI() As HspfUci
  Set UCI = pUCI
End Property
Public Property Let UCI(newvalue As HspfUci)
  Set pUCI = newvalue
  pOriginalUCIPath = PathNameOnly(pUCI.Name)
End Property

Public Property Get IPC() As ATCoIPC
  Set IPC = pIPC
End Property
Public Property Let IPC(newvalue As ATCoIPC)
  Set pIPC = newvalue
End Property

Public Property Get AUI() As Boolean
  AUI = pAUI
End Property
Public Property Let AUI(newvalue As Boolean)
  pAUI = newvalue
End Property

Public Property Get InitParmFile() As clsPestParmFile
  Set InitParmFile = pInitParmFile
End Property
Public Property Let InitParmFile(newvalue As clsPestParmFile)
  Set pInitParmFile = newvalue
End Property

Public Property Get CurParmFile() As clsPestParmFile
  Set CurParmFile = pCurParmFile
End Property
Public Property Let CurParmFile(newvalue As clsPestParmFile)
  Set pCurParmFile = newvalue
End Property

Public Property Get InitGroupFile() As clsPestGroupFile
  Set InitGroupFile = pInitGroupFile
End Property
Public Property Let InitGroupFile(newvalue As clsPestGroupFile)
  Set pInitGroupFile = newvalue
End Property

Public Property Get CurGroupFile() As clsPestGroupFile
  Set CurGroupFile = pCurGroupFile
End Property
Public Property Let CurGroupFile(newvalue As clsPestGroupFile)
  Set pCurGroupFile = newvalue
End Property

Public Function InitBasinsPest(ObsDSN As Long, SimDSN As Long, Optional pth As String = "") As String
  Dim FUnit As Long, i As Long
  Dim str As String, lstr As String, ErrStr As String
  Dim lContextOn As FastCollection
  Dim lv As Variant
  Dim WinHSPFltEXE As String
  Dim reg As New ATCoRegistry
  Dim StartTime As Single

  On Error GoTo SetupFailed
  StartTime = Timer
  WinHSPFltEXE = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\GenScn\ExePath", "") & "\WinHspfLt.exe"

  pIPC.SendMonitorMessage "(OPEN BASINS PEST)"
  pIPC.SendMonitorMessage "(Msg1 Running TSPROC to set up PEST)"
  pIPC.SendMonitorMessage "(Msg2 )"
  If Len(pth) > 0 Then
    ChDir pth
  Else
    ChDir pOriginalUCIPath
    If Right(CurDir, 5) <> "\PEST" Then 'make sure we don't keep generating new \PEST directories
      If Len(Dir(CurDir & "\PEST", vbDirectory)) = 0 Then
        MkDir "PEST"
      End If
      ChDir "PEST"
    End If
  End If
  InitParmFiles 'UCI template, Par2Par template, parm/group files
  Set pTSPROC = New clsPestTSPROC
  pTSPROC.IPC = pIPC
  ErrStr = pTSPROC.BasinsHSPFInit(pUCI, ObsDSN, SimDSN) 'tsproc prep/model run file
  If Len(ErrStr) > 0 Then
    Err.Description = ErrStr
    Err.Raise 1
  End If
  pControlFile = pTSPROC.PestControlFile
  'write out updated UCI file in PEST directory
  pUCI.Name = CurDir & "\" & FilenameNoPath(pUCI.Name)
  pUCI.Save
  pTSPROC.WritePest.AUI = pAUI
  str = pTSPROC.WriteFileAsString
  FUnit = FreeFile(0)
  Open pTSPROC.filename For Output As #FUnit
  Print #FUnit, str
  Close #FUnit
  'write out model run TSPROC response file
  Open "tsproc.in" For Output As #FUnit
  str = pTSPROC.filename & vbCrLf & FilenameNoExt(pTSPROC.filename) & ".rec"
  Print #FUnit, str
  Print #FUnit, "n" & vbCrLf & "n"
  Close #FUnit
  'write out model set up TSPROC response file
  Open "tspprep.in" For Output As #FUnit
  Print #FUnit, str
  Print #FUnit, "y" & vbCrLf & "y"
  Close #FUnit
  'write batch file for composite model
  Open pTSPROC.BatchFile For Output As #FUnit
  Print #FUnit, "@echo off"
  'Print #FUnit, "del " & FilenameNoPath(FilenameNoExt(pUCI.Name)) & ".sup > nul"
  Print #FUnit, pPestPath & "\par2par " & pP2PFile '& " > nul"
  'gets rid of WinHSPFlt status monitor, because buffer full or bit bucket???
'  Print #FUnit, WinHSPFltEXE & " " & pIPC.hPipeWriteToProcess(0) & " " & pIPC.hPipeReadFromProcess(0) & " " & pUCI.Name
  'try removing pipes
  Print #FUnit, WinHSPFltEXE & " -1 -1 " & pUCI.Name
  Print #FUnit, pPestPath & "\tsproc < tsproc.in" ' > nul"
  Close #FUnit
  'run TSPROC in model prep context
  pIPC.StartProcess "Basins Pest Setup", pPestPath & "\TSPROC" ', 20
  Do 'process messages from parent
    str = pIPC.GetProcessMessage("Basins Pest Setup")
    If Len(str) > 3 Then
      pIPC.SendMonitorMessage "(Msg2 " & str & ")"
      Select Case LCase(Left(str, 8))
        Case " program"
          pIPC.SendProcessMessage "Basins Pest Setup", "tsproc.dat" & vbCrLf
          str = ""
        Case " enter n"
          pIPC.SendProcessMessage "Basins Pest Setup", "tsproc.rec" & vbCrLf
          str = ""
        Case Else
          If Right(str, 6) = "y/n]: " Then
            pIPC.SendProcessMessage "Basins Pest Setup", "y" & vbCrLf
          ElseIf InStr(str, "Error:") > 0 Then 'problems
            i = InStr(str, "Error:")
            Err.Description = "Problem running TSPROC for PEST set up." & vbCrLf & Mid(str, i)
            Err.Raise 1
          End If
          If Right(str, 6) <> "ocess." Then 'not end of run
            If str <> lstr Then 'TSPROC continuing normally
              lstr = str
              str = ""
            Else
              'same message as last time, must be repeating exit message caused by error
              Err.Description = "Problem running TSPROC for PEST set up." & vbCrLf & "Run TSPROC outside BASINS to determine error."
              Err.Raise 2
            End If
          End If
      End Select
    End If
  Loop While Len(str) = 0
  pIPC.SendMonitorMessage "(Msg2 Completed PEST set up)"
  'update TSPROC file for model run context
  Set lContextOn = New FastCollection
  With pTSPROC.Settings
    For i = 1 To pTSPROC.Settings.ContextOn.Count
      If .ContextOn.Key(i) = "pest_prep" Then
        lv = False
      ElseIf .ContextOn.Key(i) = "model_run" Then
        lv = True
      Else
        lv = .ContextOn.ItemByIndex(i)
      End If
      lContextOn.Add lv, .ContextOn.Key(i)
    Next
    Set .ContextOn = lContextOn
  End With
  str = pTSPROC.WriteFileAsString
  StartTime = Timer
  FUnit = FreeFile(0)
  Open pTSPROC.filename For Output As #FUnit
  Print #FUnit, str
  Close #FUnit
  InitBasinsPest = ""
  Exit Function

SetupFailed:
  If Err.Number = 70 Then
    DoEvents
    Sleep 100
    DoEvents
    If (Timer - StartTime) < 10 Then Resume 'Retry for up to 10 seconds
    Err.Description = "Timed out waiting for permission denial to clear"
  End If
  InitBasinsPest = Err.Description

End Function

Public Sub Edit()

  frmBasinsPest.Pest = Me
  frmBasinsPest.Show vbModal

End Sub

Public Sub Run()
  Dim str As String
  Dim RunPest As Boolean
  Dim AllDone As Boolean
  Dim NormalEnd As Boolean

  'run PEST
  If Len(pControlFile) > 0 Then
    NormalEnd = False
    pIPC.SendMonitorMessage "(OPEN BASINS PEST)"
    'check pest control file, assume failure
    RunPest = False
    pIPC.SendMonitorMessage "(Msg1 Checking PEST Control file)"
    pIPC.StartProcess "Basins Pest", pPestPath & "\PESTCHEK " & pControlFile '
    Do 'process messages from parent
      str = pIPC.GetProcessMessage("Basins Pest")
      pIPC.SendMonitorMessage "(Msg2 " & str & ")"
      If InStr(str, "No errors") > 0 And InStr(str, "No warnings") > 0 Then
        RunPest = True
      ElseIf Len(str) > 0 Then
        str = ""
      End If
    Loop While Len(str) = 0
    Sleep 1000 'be sure everything is cleared out
    If RunPest Then 'check UCI template file, assume failure
      RunPest = False
      pIPC.SendMonitorMessage "(Msg2 PESTCHEK completed: OK)"
      pIPC.SendMonitorMessage "(Msg1 Checking HSPF template file)"
      pIPC.StartProcess "Basins Pest", pPestPath & "\TEMPCHEK " & FilenameNoExt(FilenameNoPath(pUCI.Name)) & ".tpl"
      Do 'process messages from parent
        str = pIPC.GetProcessMessage("Basins Pest")
        pIPC.SendMonitorMessage "(Msg2 " & str & ")"
        If InStr(str, "No errors") > 0 Then
          RunPest = True
        ElseIf Len(str) > 0 Then
          str = ""
        End If
      Loop While Len(str) = 0
    Else
      pIPC.SendMonitorMessage "(Msg2 PESTCHEK completed: FAILED)"
    End If
    Sleep 1000 'be sure everything is cleared out
    If RunPest Then
      pIPC.SendMonitorMessage "(Msg2 TEMPCHEK completed: OK)"
      pIPC.SendMonitorMessage "(Msg1 Running PEST)"
      pIPC.SendMonitorMessage "(IgnoreWindowCommands)"
      pIPC.StartProcess "Basins Pest", pPestPath & "\PEST " & pControlFile ',  20
      Do 'process messages from parent
        str = pIPC.GetProcessMessage("Basins Pest")
        pIPC.SendMonitorMessage "(Msg2 " & str & ")"
        If Right(str, 10) = "residuals." Then 'normal end of PEST run
          AllDone = True
          NormalEnd = True
        End If
          
        Select Case pIPC.GetProcessMessage(0, 0)
          Case "": 'continue
          Case "C": AllDone = True
          pIPC.ExitProcess ("Basins Pest")
        End Select
      Loop While Not AllDone
      pIPC.SendMonitorMessage "(Msg1 Completed PEST Run)"
      pIPC.SendMonitorMessage "(FollowWindowCommands)"
      pIPC.SendMonitorMessage "(HIDE)"
    Else
      pIPC.SendMonitorMessage "(Msg2 TEMPCHEK completed: FAILED)"
    End If
    If NormalEnd Then
      MsgBox "PEST run fully completed.  View PEST output files for details." & vbCrLf & _
             "Refinement of the PEST run is best performed in batch (command) mode.", vbInformation, "BASINS Pest"
    End If
  End If
End Sub

Private Sub InitParmFiles()
  Dim TemplateTables As FastCollection, optypes As FastCollection
  Dim OpName As String
  Dim VaryingParms As String
  Dim Opns As Collection
  Dim opn As Variant
  Dim RecCnt As Long, i As Long, j As Long, k As Long
  Dim Pind As Long, ipos As Long, MonVarRec As Long, MonParInd As Long
  Dim ltable As HspfTable
  Dim lparm As HSPFParm
  Dim lstr As String, outstr As String
  Dim vstr As String, v As String
  Dim FirstRec As Boolean, Transformed As Boolean
  Dim P2PData As New FastCollection, P2PTemplates As New FastCollection
  Dim MonVarData As New FastCollection, MonUCITpl As New FastCollection
  Dim pName As String
  Dim FName As String, FUnit As Long
  Dim GroupNames As String
  Dim GrpRec As clsPestGroupData
  Dim PrmRec As clsPestParmData
  Dim FirstOpID As String, lModelSpecs As String
  Dim vParm As Variant

  outstr = "ptf $" & vbCrLf
  'get parameters being varied along with their table and operation types
  VaryingParms = GetVaryingParms(optypes, TemplateTables)
  'current parm data collection is built from initial parm file and UCI file
  Set pCurParmFile = New clsPestParmFile
  pCurParmFile.ParmData = New FastCollection
  For Each GrpRec In pInitGroupFile.GroupData
    If InStr(GroupNames, GrpRec.Name) = 0 Then 'add to list of groups
      GroupNames = GroupNames & GrpRec.Name & ","
    End If
  Next
  GroupNames = Left(GroupNames, Len(GroupNames) - 1)
  'init current group data collection from initial group file
  Set pCurGroupFile = New clsPestGroupFile
  Set pCurGroupFile = pInitGroupFile
  For j = 1 To optypes.Count 'loop through operations containing parameters being varied
    OpName = optypes.ItemByIndex(j)
    If pUCI.OpnBlks(OpName).IDs.Count > 0 Then
      Set Opns = pUCI.OpnBlks(OpName).IDs
      For i = 1 To TemplateTables.Count 'loop through tables containing parameters being varied
        FirstRec = True
        For Each opn In Opns
          If opn.TableExists(TemplateTables(i)) Then
            RecCnt = RecCnt + 1
            lstr = NumFmtI(RecCnt, 3) & NumFmtI(opn.Tables(TemplateTables(i)).Parms.Count, 3)
            vstr = ""
            MonParInd = 0
            Set ltable = opn.Tables(TemplateTables(i))
            ltable.SuppID = RecCnt
            For Each lparm In ltable.Parms
              With lparm
                If FirstRec Then
                  lstr = lstr & Space(.Def.StartCol - Len(lstr) - 1) 'pad prev field
                  lstr = lstr & .Def.Name
                  FirstOpID = opn.id
                End If
                If InStr(VaryingParms, UCase(.Def.Name)) > 0 Then
                  'this parameter is to be varied by PEST
                  Transformed = IsTransformed(VaryingParms, .Def.Name)
                  If Transformed Then 'varying parmater is transformed
                    pName = .Def.Name & "TRN" & opn.id
                  Else 'normal varying
                    pName = .Def.Name & opn.id
                  End If
                  v = "$" & pName & Space(13 - Len(pName)) & "$"
                  'add this parm to the PAR2PAR data collection
                  P2PData.Add "= " & v, pName
                  If Transformed Then 'include record to transform the parameter
                    ipos = InStr(pName, "TRN")
                    P2PData.Add "= " & pName & "/(1+" & pName & ")", Left(pName, ipos - 1) & opn.id
                    'remove "TRN" from UCI template record
                    ipos = InStr(v, "TRN")
                    v = Left(v, ipos - 1) & opn.id & "     $"
                  End If
                  If IsMonthlyVarying(lparm, opn) Then 'store monthly PAR2PAR Parms
                    MonParInd = MonParInd + 1
                    MonVarRec = RecCnt + Opns.Count * MonParInd
                    SaveMonVarData lparm, opn, MonVarData, MonVarRec, MonUCITpl
                  End If
                  If FirstRec And InStr(GroupNames, .Def.Name) = 0 Then 'new entry for group data
                    Set GrpRec = New clsPestGroupData
                    GrpRec.Name = .Def.Name
                    GrpRec.IncType = "relative"
                    GrpRec.DerInc = 0.01
                    GrpRec.DILBnd = 0.001
                    GrpRec.ForCen = "switch"
                    GrpRec.DIMult = 2#
                    GrpRec.DerMeth = "parabolic"
                    pCurGroupFile.GroupData.Add GrpRec
                  End If
                  'find index of this parm in initial parm file
                  Pind = 0
                  For Each vParm In pInitParmFile.ParmData
                    If InStr(vParm.Name, .Def.Name) > 0 Then
                      Pind = 1 'indicate we found parm in initial parm file
                      Exit For
                    End If
                  Next
                  If Pind = 0 Then Set vParm = Nothing
                  'now add parm data entry
                  Set PrmRec = New clsPestParmData
                  If vParm Is Nothing Then 'use defaults from HSPF message file
                    If FirstRec Then 'default to log
                      PrmRec.Trans = "log"
                    Else
                      PrmRec.Trans = "tied_" & .Def.Name & FirstOpID
                    End If
                    PrmRec.LimType = "factor"
                    If Abs(.Value) < 0.000001 Then 'use real small number
                      PrmRec.InitVal = 0.0001
                    Else 'use UCI initial value
                      PrmRec.InitVal = .Value
                    End If
                    If Abs(.Def.Min) < 0.000001 Then 'make it a real small number
                      PrmRec.LBnd = 0.0001
                    Else 'use the parameter definition min
                      PrmRec.LBnd = .Def.Min
                    End If
                    PrmRec.UBnd = .Def.Max
                    PrmRec.Group = .Def.Name
                    PrmRec.Mult = 1#
                    PrmRec.Offset = 0#
                  Else 'use values from initial parm file
                    If FirstRec Then
                      PrmRec.Trans = vParm.Trans
                    Else
                      PrmRec.Trans = "tied_" & vParm.Name & FirstOpID '.Def.Name & FirstOpID
                    End If
                    PrmRec.LimType = vParm.LimType
                    PrmRec.InitVal = vParm.InitVal
                    PrmRec.LBnd = vParm.LBnd
                    PrmRec.UBnd = vParm.UBnd
                    PrmRec.Group = vParm.Group
                    PrmRec.Mult = vParm.Mult
                    PrmRec.Offset = vParm.Offset
                  End If
                  PrmRec.Name = pName
                  PrmRec.ModelSpecs = OpName & "," & TemplateTables(i) & "," & opn.id
                  pCurParmFile.ParmData.Add PrmRec
                Else
                  v = .Value
                End If
                If Len(vstr) = 0 Then
                  vstr = Space(.Def.StartCol) & v
                Else
                  vstr = vstr & " " & v
                End If
              End With
            Next
            outstr = outstr & lstr & vbCrLf & vstr & vbCrLf
            FirstRec = False
          End If
        Next
        If MonUCITpl.Count > 0 Then 'add monthly varying records to UCI template file
          For k = 1 To MonUCITpl.Count
            outstr = outstr & MonUCITpl.ItemByIndex(k)
          Next k
          RecCnt = RecCnt + MonUCITpl.Count
          Set MonUCITpl = New FastCollection
        End If
      Next i
    End If
  Next
  'write out UCI template file
  FName = LCase(FilenameNoExt(FilenameNoPath(pUCI.Name)) & ".tpl")
  FUnit = FreeFile(0)
  Open FName For Output As #FUnit
  Print #FUnit, outstr
  Close #FUnit
  'be sure supplemental file is in UCI
  For i = 1 To pUCI.FilesBlock.Count
    If pUCI.FilesBlock.Value(i).Typ = "PESTSU" Then 'already have supplemental file
      If LCase(pUCI.FilesBlock.Value(i).Name) <> LCase(FilenameNoExt(FName)) & ".sup" Then
        'update name to match this UCI
        pUCI.FilesBlock.Value(i).Name = FilenameNoExt(FName) & ".sup"
        Exit For
      End If
    ElseIf i = pUCI.FilesBlock.Count Then 'no supplemental file found, add one
      pUCI.FilesBlock.AddFromSpecs FilenameNoExt(FName) & ".sup", "PESTSU"
      Exit For
    End If
  Next i
  'use similar name for pPar2Par data file
  pP2PFile = "p2p_" & FilenameNoExt(FName) & ".dat"
  'put template name in P2P template collection
  P2PTemplates.Add FName, LCase(FilenameNoExt(FName) & ".sup")
  'write out hspf response file
  FName = "hspf.in"
  Open FName For Output As #FUnit
  outstr = LCase(FilenameNoPath(pUCI.Name)) & vbCrLf & LCase(FilenameNoExt(FilenameNoPath(pUCI.Name))) & ".sup"
  Print #FUnit, outstr
  Close #FUnit
  'write out PAR2PAR template file
  Open FilenameNoExt(pP2PFile) & ".tpl" For Output As #FUnit
  Set pPar2Par = New clsPestPAR2PAR
  pPar2Par.ParmData = P2PData
  For i = 1 To MonVarData.Count
    pPar2Par.ParmData.Add MonVarData.ItemByIndex(i), MonVarData.Key(i)
  Next i
  pPar2Par.Templates = P2PTemplates
  pPar2Par.Precis = "single"
  pPar2Par.DPoint = "point"
  outstr = pPar2Par.WriteFileAsString(2)
  Print #FUnit, outstr
  Close #FUnit
  'write out group and parm files
'  Set GrpFile = New clsPestGroupFile
'  GrpFile.GroupData = GrpData
  outstr = pCurGroupFile.WriteFileAsString
  Open "group.dat" For Output As #FUnit
  Print #FUnit, outstr
  Close #FUnit
'  Set PrmFile = New clsPestParmFile
'  PrmFile.ParmData = PrmData
  outstr = pCurParmFile.WriteFileAsString
  Open "parm.dat" For Output As #FUnit
  Print #FUnit, outstr
  Close #FUnit
  
End Sub

Private Sub Class_Initialize()

  Dim ff As New ATCoFindFile
  Dim PestExe As String

  ff.SetRegistryInfo "Basins", "Pest", "Path"
  ff.SetDialogProperties "Please locate PEST executable code", "Pest.exe", "exe"
  PestExe = ff.GetName

  If Len(PestExe) > 0 Then
    If Len(Dir(PestExe)) > 0 Then
      pPestPath = PathNameOnly(PestExe)
    End If
  End If

  Set pTSPROC = New clsPestTSPROC
  pAUI = False
End Sub

Public Sub UpdateUCIFromSup()
  Dim i As Long, j As Long, FUnit As Long, lID As String
  Dim SupName As String, SupFile As String, OpName As String, lstr As String
  Dim vOpnBlk As Variant, lOpnBlk As HspfOpnBlk
  Dim Opns As Collection
  Dim opn As Variant
  Dim ltable As HspfTable
  Dim lparm As HSPFParm
  Dim IDs As FastCollection, Values As FastCollection
  Dim TemplateTables As FastCollection, optypes As FastCollection

  SupName = ""
  For i = 1 To pUCI.FilesBlock.Count
    If pUCI.FilesBlock.Value(i).Typ = "PESTSU" Then 'already have supplemental file
      SupName = pUCI.FilesBlock.Value(i).Name
      SupFile = WholeFileString(SupName)
      If Len(SupFile) > 0 Then pUCI.FilesBlock.Remove i 'remove supplemental file
      Exit For
    End If
  Next i
  If Len(SupFile) > 0 Then
    Set IDs = New FastCollection
    Set Values = New FastCollection
    While Len(SupFile) > 0 'read supp file records
      lstr = StrSplit(SupFile, vbLf, "")
      If Len(lstr) > 0 Then 'first record has ID and number of values
        lID = StrRetRem(lstr)
        IDs.Add lID, lID
        lstr = StrSplit(SupFile, vbCrLf, "")
        If Len(lstr) > 0 Then 'second record has parameter values
          Values.Add lstr, lID 'store with key = same ID as previous record
        End If
      End If
    Wend
    lstr = GetVaryingParms(optypes, TemplateTables)
    For j = 1 To optypes.Count
      OpName = optypes.ItemByIndex(j)
      If pUCI.OpnBlks(OpName).IDs.Count > 0 Then
        Set Opns = pUCI.OpnBlks(OpName).IDs
        For i = 1 To TemplateTables.Count
          For Each opn In Opns
            If opn.TableExists(TemplateTables(i)) Then
              Set ltable = opn.Tables(TemplateTables(i))
              If ltable.SuppID > 0 Then
                If IDs.KeyExists(ltable.SuppID) Then
                  lstr = Values.ItemByKey(ltable.SuppID)
                  For Each lparm In ltable.Parms
                    lparm.Value = StrRetRem(lstr)
                  Next
                End If
                ltable.SuppID = 0 'no longer reference supplemental file
              End If
            End If
          Next
        Next i
      End If
    Next j
  End If
  pUCI.Save
  
End Sub

Private Function GetVaryingParms(optypes As FastCollection, TemplateTables As FastCollection) As String
  'look through initial PEST parameter file for parameters to be varied
  'also build collections of operations (optypes) and tables (TemplateTables)
  'that contain parameters being varied
  Dim VaryingParms As String
  Dim lModelSpecs As String, OpName As String, TableName As String
  Dim PrmRec As clsPestParmData

  'look through current parm file for parameters being varied
  VaryingParms = ""
  Set TemplateTables = New FastCollection
  Set optypes = New FastCollection
  For Each PrmRec In pInitParmFile.ParmData
    If InStr(VaryingParms, PrmRec.Group) = 0 Then 'add to list of parms to be varied
      VaryingParms = VaryingParms & PrmRec.Group & ","
      lModelSpecs = PrmRec.ModelSpecs
      OpName = StrRetRem(lModelSpecs)
      If Not optypes.KeyExists(OpName) Then
        'add to collection of operations containing parameters to be varied
        optypes.Add OpName, OpName
      End If
      TableName = StrRetRem(lModelSpecs)
      If Not TemplateTables.KeyExists(TableName) Then
        'add to collection of tables containing parameters to be varied
        TemplateTables.Add TableName, TableName
      End If
    End If
  Next
  If Len(VaryingParms) > 0 Then VaryingParms = Left(VaryingParms, Len(VaryingParms) - 1)
  GetVaryingParms = VaryingParms

End Function

Private Function IsMonthlyVarying(lparm As HSPFParm, opn As Variant) As Boolean

  Dim MonthlyVarying As Boolean
  Dim i As Long, j As Long
  Dim vpos As Long, tPos As Long
  Dim ltable As HspfTable
  Dim vTables As Variant
  Dim vParms As Variant
  Dim VarsInTable As Variant
  Dim CouldVary As Boolean

  vTables = Array("PWAT-PARM1", "SED-PARM1", "PSTEMP-PARM1", "PWT-PARM1", "IWAT-PARM1")
  VarsInTable = Array(6, 2, 6, 4, 4)
  vParms = Array("CEPSC", "UZSN", "NSUR", "INTFW", "IRC", "LZETP", _
                 "COVER", "NVSI", _
                 "ASLT", "BSLT", "ULTP1", "ULTP2", "LGTP1", "LGTP2", _
                 "IDOXP", "ICO2P", "ADOXP", "ACO2P", _
                 "", "", "RETSC", "NSUR")

  MonthlyVarying = False 'assume not varying monthly
  CouldVary = False
  tPos = 0
  vpos = 0
  i = 0
  'look through possible monthly varying parameters
  While i <= UBound(vTables) And Not CouldVary
    j = 1
    While j <= VarsInTable(i)
      If lparm.Name = vParms(vpos) Then 'found matching variable among possible monthly varying parms
        tPos = i
        vpos = j 'save variable position in this table
        j = VarsInTable(i) + 1
        CouldVary = True
      Else
        j = j + 1
        vpos = vpos + 1
      End If
    Wend
    i = i + 1
  Wend
  If CouldVary Then 'this parameter may vary monthly
    Set ltable = opn.Tables(vTables(tPos))
    If tPos = 0 Then 'offset PWAT-PARM1 varying flags by 2
      vpos = vpos + 3
    ElseIf tPos = 2 Then 'PSTEMP has 2 tables per flag
      vpos = Int((vpos + 1) / 2)
    End If
    If ltable.Parms(vpos) = 1 Then 'this parm is monthly varying
      MonthlyVarying = True
    End If
  End If

  IsMonthlyVarying = MonthlyVarying
End Function

Private Sub SaveMonVarData(lparm As HSPFParm, opn As Variant, MonVarData As FastCollection, _
                           MonVarRec As Long, MonUCITpl As FastCollection)
  Dim i As Long, vpos As Long, ipos As Long
  Dim TableName As String
  Dim Mons As Variant
  Dim Mult As Single
  Dim vMon As String, vJan As String
  Dim tplstr As String
  Dim Vars As Variant, VarTables As Variant
  Dim ltable As HspfTable

  Mons = Array("", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC")
  Vars = Array("CEPSC", "UZSN", "NSUR", "INTFW", "IRC", "LZETP", _
               "COVER", "NVSI", "ASLT", "BSLT", "ULTP1", "ULTP2", "LGTP1", "LGTP2", _
               "IDOXP", "ICO2P", "ADOXP", "ACO2P", "RETSC", "NSUR")
  VarTables = Array("INTERCEP", "UZSN", "MANNING", "INTERFLW", "IRC", "LZETPARM", _
                    "COVER", "NVSI", "ASLT", "BSLT", "ULTP1", "ULTP2", "LGTP1", "LGTP2", _
                    "IFWDOX", "IFWCO2", "GRNDDOX", "GRNDCO2", "RETN", "MANNING")

  vpos = -1
  For i = 0 To UBound(Vars)
    If lparm.Name = Vars(i) Then
      vpos = i
      Exit For
    End If
  Next i
  If vpos >= 0 Then
    TableName = "MON-" & VarTables(vpos)
    tplstr = MonVarRec & "  12  " & TableName & vbCrLf
    Set ltable = opn.Tables(TableName)
    ltable.SuppID = MonVarRec
    vJan = lparm.Name & Mons(1) & opn.id
    tplstr = tplstr & "$" & vJan & Space(13 - Len(vJan)) & "$ "
    'set JAN value to PAR2PAR parameter value
    MonVarData.Add " = " & lparm.Name & opn.id, vJan
    For i = 2 To 12
      Mult = ltable.Parms(i) / ltable.Parms(1)
      vMon = lparm.Name & Mons(i) & opn.id
      tplstr = tplstr & "$" & vMon & Space(13 - Len(vMon)) & "$ "
      MonVarData.Add " = " & Mult & " * " & vJan, vMon
    Next i
    tplstr = tplstr & vbCrLf
    ipos = 0
    i = 1
    While i <= MonUCITpl.Count
      If CLng(MonUCITpl.Key(i)) < MonVarRec Then 'try next one in collection
        i = i + 1
      Else 'insert new record before this one
        ipos = i
        i = MonUCITpl.Count + 1
      End If
    Wend
    If ipos = 0 Then 'just add to end
      MonUCITpl.Add tplstr, CStr(MonVarRec)
    Else
      MonUCITpl.Add tplstr, CStr(MonVarRec), ipos
    End If
  End If
End Sub

Private Function IsTransformed(VaryingParms As String, Parm As String) As Boolean
  Dim Transformed As Boolean
  Dim i As Long, lstr As String

  Transformed = False
  i = InStr(VaryingParms, Parm)
  If i > 0 Then
    lstr = Mid(VaryingParms, i + Len(Parm))
    If UCase(Left(lstr, 5)) = "TRANS" Then
      Transformed = True
    End If
  End If
  IsTransformed = Transformed

End Function
