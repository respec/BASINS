VERSION 5.00
Begin VB.Form frmUCIUpdate 
   Caption         =   "Form1"
   ClientHeight    =   2556
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   3744
   LinkTopic       =   "Form1"
   ScaleHeight     =   2556
   ScaleWidth      =   3744
   StartUpPosition =   3  'Windows Default
End
Attribute VB_Name = "frmUCIUpdate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public W_EXESTATUS As String
Public W_EXEWINHSPF As String
Public W_HSPFMSGMDB As String
Public W_POLLUTANTLIST As String
Public W_HSPFMSGWDM As String
Public W_STARTERPATH As String
Public BASINSPath As String
Public MessageUnit As Long 'needed?
Dim huc12ext$(266), uciid$(266), septic&(266), metmult#(266), metdsn&(266)
Dim reachid(100), ptdsn(100), ptdsn2(15, 100), BmpArea&(266, 40)
Dim fac$(100), ptval!(15, 100)

Private Sub Form_Load()

  Set reg = New ATCoRegistry
  reg.AppName = "WinHSPF"

  Set myMsgBox = New ATCoMessage
  'Set myMsgBox.Icon = Me.Icon
  
  'W_EXEWINHSPF = "c:\vbExperimental\winhspf\bin\winhspf.exe"
  W_HSPFMSGMDB = "hspfmsg.mdb"
  W_HSPFMSGWDM = "hspfmsg.wdm"
  W_STARTERPATH = ""
    
  'ChDrive Left(W_EXEWINHSPF, 2)
  'ChDir Mid(W_EXEWINHSPF, 3, Len(W_EXEWINHSPF) - Len("C:winhspf.exe"))
  
  ChDir "C:\mngwpd\Modelwq"
    
  Set IPC = New ATCoIPC
  With IPC
    .SendMonitorMessage "(OPEN WinHSPF)"
    .SendMonitorMessage "(Initializing WinHSPF)"
    
    F90_W99OPN
    F90_WDBFIN
    F90_PUTOLV 10
    F90_SPIPH .hPipeReadFromProcess(0), .hPipeWriteToProcess(0) ' .ComputeRead, .ComputeWrite '.ComputeReadFromParent, .ComputeWriteToParent
    MessageUnit = F90_WDBOPN(1, W_HSPFMSGWDM, Len(W_HSPFMSGWDM))
    .SendMonitorMessage "(BUTTOFF OUTPUT)"
    .SendMonitorMessage "(BUTTOFF PAUSE)"
    .SendMonitorMessage "(BUTTOFF CANCEL)"
    'MsgBox "myMsg.name = " & W_HSPFMSGMDB
    .SendMonitorMessage "(Initializing HspfMsg)"
    Set myMsg = New HspfMsg
    Set myMsg.Monitor = IPC
    myMsg.Name = W_HSPFMSGMDB
    .SendMonitorMessage "(Opening Default UCI 'starter.uci')"
  End With
  StartHSPFEngine
  OpenDefaultUCI
  'IPC.SendMonitorMessage "(Successfully Read Default UCI)"
  
  Set myUci = New HspfUci
  myUci.HelpFile = App.HelpFile
  myUci.StarterPath = W_STARTERPATH
  Set myUci.Monitor = IPC
  myUci.MessageUnit = MessageUnit

  IPC.SendMonitorMessage "(BUTTON OUTPUT)"
  IPC.SendMonitorMessage "(BUTTON PAUSE)"
  IPC.SendMonitorMessage "(BUTTON CANCEL)"

  ChangeUCIs
  
End Sub

Private Sub OpenDefaultUCI()
  Dim f$, s$, hin&, hout&, FilesOK As Boolean, echofile$

  'ChDrive Left(W_STARTERPATH, 1)
  'ChDir (W_STARTERPATH)
  f = "starter.uci"
  Me.MousePointer = vbHourglass
  Set defUci = Nothing
  Set defUci = New HspfUci
  Set defUci.Monitor = IPC
  defUci.HelpFile = App.HelpFile
  defUci.MessageUnit = MessageUnit
  'myUci.InitWDMArray
  defUci.MsgWDMName = W_HSPFMSGWDM
  defUci.ReadUci myMsg, f, -1, FilesOK, echofile
  Set defUci.Icon = Me.Icon
  Me.MousePointer = vbNormal
  s = defUci.ErrorDescription
10    ' continue here on cancel
  Err.Clear
  'clear out wdm array for myuci
  defUci.InitWDMArray
End Sub

Private Sub ChangeUCIs()
  Dim myPath$, cUCIs As Collection, myName$, vUCIs As Variant
  Dim FilesOK As Boolean, echofile$, lstr$, delim$, quote$
  Dim XLApp As Excel.Application, XLBook As Excel.Workbook, XLSheet As Excel.Worksheet
  
  myPath = ".."
  ChDir myPath
  Set cUCIs = New Collection
'  myName = Dir(myPath, vbDirectory)   ' Retrieve the first entry.
'  Do While myName <> ""   ' Start the loop.
'     ' Ignore the current directory and the encompassing directory.
'     If myName <> "." And myName <> ".." Then
'        ' Use bitwise comparison to make sure MyName is a directory.
'        If (GetAttr(myPath & myName) And vbDirectory) = vbDirectory Then
'           Debug.Print myName   ' Display entry only if it
'           cUCIs.Add myName
'        End If   ' it represents a directory.
'     End If
'     myName = Dir   ' Get next entry.
'  Loop
'

  commandline = Command
  clen = Len(commandline)
    
'    cUCIs.Add "ChaBigCk"
'    cUCIs.Add "ChaYelCk"
    cUCIs.Add "ChaUtoCk"
    cUCIs.Add "ChaSweCk"
    cUCIs.Add "ChaSuwCk"
    cUCIs.Add "ChaSopCk"
'    cUCIs.Add "ChaPeaCk"
'    cUCIs.Add "ChaMosCk"
'    cUCIs.Add "ChaLittR"
    cUCIs.Add "ChaLaniL"
'    cUCIs.Add "ChaChesR"
    cUCIs.Add "ChaCenCk"
'    cUCIs.Add "ChaNewR"
    cUCIs.Add "ChaDogR"
'
    cUCIs.Add "FliMornCk"
    cUCIs.Add "FliWOaCk"
    cUCIs.Add "FliLinCk"
'
    cUCIs.Add "OcoShoCk"
    cUCIs.Add "OcoNorUR"
'    cUCIs.Add "OcoMulbR"
'    cUCIs.Add "OcoMidUR"
'    cUCIs.Add "OcoLitUR"
'    cUCIs.Add "OcoAppUR"
    cUCIs.Add "OcoAppLR"
'
'    cUCIs.Add "OcmYelUR"
'    cUCIs.Add "OcmYelMR"
'    cUCIs.Add "OcmYelLR"
'    cUCIs.Add "OcmTussR"
'    cUCIs.Add "OcmTowUR"
'    cUCIs.Add "OcmSouUR"
'    cUCIs.Add "OcmSouLR"
'    cUCIs.Add "OcmAlcUR"
'    cUCIs.Add "OcmIndCk"
'
'    cUCIs.Add "EtoSprCk"
'    cUCIs.Add "EtoShoCk"
'    cUCIs.Add "EtoSetCk"
'    cUCIs.Add "EtoShMtC"
'    cUCIs.Add "EtoRacCk"
'    cUCIs.Add "EtoPumCk"
    cUCIs.Add "EtoPetCk"
'    cUCIs.Add "EtoLoSwC"
    cUCIs.Add "EtoLittR"
'    cUCIs.Add "EtoEuhCk"
    cUCIs.Add "EtoButCk"
    cUCIs.Add "EtoCanCk"
    cUCIs.Add "EtoAllaL"
    cUCIs.Add "Eto2RunC"
'
    cUCIs.Add "OosOooCk"
'    cUCIs.Add "OosLOosR"
'    cUCIs.Add "TalLittR"
'    cUCIs.Add "CooSalCk"
'    cUCIs.Add "CooPiLoC"
    
'  End If

  'read database
'  Set XLApp = New Excel.Application
'  Set XLBook = Workbooks.Open(CurDir & "\uciupdate\12d_data02.xls")
'  For Each XLSheet In Worksheets
'    If XLSheet.Name = "huc-12 data" Then
'      'this is the worksheet we want
'      XLSheet.Activate
'      With XLSheet
'        For i = 8 To 266
'          huc12ext(i) = .Cells(i, 3)
'          uciid(i) = .Cells(i, 5)
'          septic(i) = .Cells(i, 33)
'          metmult(i) = .Cells(i, 11)
'          metdsn(i) = .Cells(i, 12)
'        Next i
'      End With
'    End If
'  Next XLSheet

  'ReadNewFTables defUci
  
  'read existing point source database
'  Set XLApp = New Excel.Application
'  Set XLBook = Workbooks.Open(CurDir & "\uciupdate\pointsourceobs.xls")
'  For Each XLSheet In Worksheets
'    If XLSheet.Name = "DSN matrix" Then
'      'this is the worksheet we want
'      XLSheet.Activate
'      With XLSheet
'        For i = 1 To 100
'          reachid(i) = .Cells(i, 3)
'          ptdsn(i) = .Cells(i, 7)
'          ptdsn2(1, i) = .Cells(i, 7)
'          ptdsn2(2, i) = .Cells(i, 8)
'          ptdsn2(3, i) = .Cells(i, 9)
'          ptdsn2(4, i) = .Cells(i, 10)
'          ptdsn2(5, i) = .Cells(i, 11)
'          ptdsn2(6, i) = .Cells(i, 12)
'          ptdsn2(7, i) = .Cells(i, 13)
'          ptdsn2(8, i) = .Cells(i, 14)
'          ptdsn2(9, i) = .Cells(i, 15)
'          ptdsn2(10, i) = .Cells(i, 16)
'          ptdsn2(11, i) = .Cells(i, 17)
'          ptdsn2(12, i) = .Cells(i, 18)
'          ptdsn2(13, i) = .Cells(i, 19)
'          ptdsn2(14, i) = .Cells(i, 20)
'          ptdsn2(15, i) = .Cells(i, 21)
'        Next i
'      End With
'    End If
'  Next XLSheet

  'read future point source database
  Set XLApp = New Excel.Application
  Set XLBook = Workbooks.Open(CurDir & "\modelwq\projects\ps2.xls")
  For Each XLSheet In Worksheets
    If XLSheet.Name = "Sheet1" Then
      'this is the worksheet we want
      XLSheet.Activate
      With XLSheet
        For i = 1 To 100
          reachid(i) = .Cells(i, 2)
          ptdsn2(1, i) = .Cells(i, 15)
          ptdsn2(2, i) = .Cells(i, 16)
          ptdsn2(3, i) = .Cells(i, 17)
          ptdsn2(4, i) = .Cells(i, 18)
          ptdsn2(5, i) = .Cells(i, 19)
          ptdsn2(6, i) = .Cells(i, 20)
          ptdsn2(7, i) = .Cells(i, 21)
          ptdsn2(8, i) = .Cells(i, 22)
          ptdsn2(9, i) = .Cells(i, 23)
          ptdsn2(10, i) = .Cells(i, 24)
          If IsNumeric(.Cells(i, 5)) Then
            ptval(1, i) = -1# * CSng(.Cells(i, 5))
          End If
          If IsNumeric(.Cells(i, 6)) Then
            ptval(2, i) = .Cells(i, 6)
          End If
          If IsNumeric(.Cells(i, 7)) Then
            ptval(3, i) = .Cells(i, 7)
          End If
          If IsNumeric(.Cells(i, 8)) Then
            ptval(4, i) = .Cells(i, 8)
          End If
          If IsNumeric(.Cells(i, 9)) Then
            ptval(5, i) = -1# * CSng(.Cells(i, 9))
          End If
          If IsNumeric(.Cells(i, 10)) Then
            ptval(6, i) = .Cells(i, 10)
          End If
          If IsNumeric(.Cells(i, 11)) Then
            ptval(7, i) = .Cells(i, 11)
          End If
          If IsNumeric(.Cells(i, 12)) Then
            ptval(8, i) = .Cells(i, 12)
          End If
          If IsNumeric(.Cells(i, 13)) Then
            ptval(9, i) = -1# * CSng(.Cells(i, 13))
          End If
          If IsNumeric(.Cells(i, 14)) Then
            ptval(10, i) = .Cells(i, 14)
          End If
          fac(i) = .Cells(i, 4)
        Next i
      End With
    End If
  Next XLSheet

'BMP SECTION
  delim = ","
  quote = """"
  basepath = CurDir
  'read file of bmp areas
  i = FreeFile(0)
  tname = basepath & "\modelwq\projects\" & "altslandarea.csv"
  Open tname For Input As #i
  Line Input #i, lstr  'header line
  j = 0
  Do Until EOF(i)
    Line Input #i, lstr
    j = j + 1
    huc12ext(j) = StrSplit(lstr, delim, quote)
    X = StrSplit(lstr, delim, quote)
    For k = 1 To 40
      BmpArea(j, k) = StrSplit(lstr, delim, quote)
    Next k
  Loop
  Close #i
  
  basepath = CurDir
  For Each vUCIs In cUCIs
    Let myName = vUCIs
    'myName = "OcmIndCk"
    myPath = basepath
    ChDir myPath & "\modelwq\projects\" & myName
    myName = myName & ".uci"
    myUci.ReadUci myMsg, myName, -1, FilesOK, echofile
    'myUci.ReadUci myMsg, myName, -99, FilesOK, echofile
    
    'MakeMods_GlobalBinaryParms myUci
    'MakeMods_AddOutput myUci
    'MakeMods_AddSeptics myUci
    'MakeMods_RenumberReaches myUci
    'MakeMods_ReplaceFtables defUci, myUci
    'MakeMods_UpdateSeptics myUci
    'MakeMods_UpdateMet myUci
    'MakeMods_UpdateMetPI myUci
    'MakeMods_AddPointSources myUci
    'MakeMods_RenumberSeptics myUci
    'MakeMods_HPCP myUci
    'MakeMods_WaterIntakes myUci
    
'    MakeMods_AddWQ defUci, myUci
'    MakeMods_AddPointSourcesWQ myUci
'    MakeMods_AddExtTarLinkWQ myUci
'    MakeMods_AddExtSrcLinkWQ myUci
'    MakeMods_UpdatePrintFlags myUci
'    MakeMods_UpdateWQTables
'    MakeMods_UpdateDIV myUci
    'MakeMods_UpdatePhos

    'BuildNewPointDSNs

    'update point sources
    'MakeMods_RemovePointSources
    'MakeMods_RemoveWaterIntakes
    MakeMods_AddFuturePointSources
    'add bmps
    'MakeMods_AddBMPs myUci
    'MakeMods_AddBMPAreas myUci
    
    FileCopy myName, myName & ".sav"
    myUci.Save
    
    myUci.ClearWDM
    Set myUci = Nothing
    Set myUci = New HspfUci
    Set myUci.Monitor = IPC
    myUci.StarterPath = W_STARTERPATH
    myUci.MsgWDMName = W_HSPFMSGWDM
    myUci.MessageUnit = MessageUnit
    myUci.InitWDMArray
    'Exit Sub  'for testing
  Next vUCIs
  
End Sub

Private Sub MakeMods_GlobalBinaryParms(myUci As HspfUci)
  Dim s As String
  Dim lOper As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim ltable As HspfTable

  myUci.GlobalBlock.SDate(0) = 1992
  myUci.GlobalBlock.SDate(1) = 10
  myUci.GlobalBlock.SDate(2) = 1
  myUci.GlobalBlock.EDate(0) = 1999
  myUci.GlobalBlock.EDate(1) = 9
  myUci.GlobalBlock.EDate(2) = 30
  myUci.GlobalBlock.EDate(3) = 24
  myUci.GlobalBlock.EDate(4) = 0
  
  If myUci.FilesBlock.Count = 4 Then
    s = Mid(myUci.Name, 1, Len(myUci.Name) - 4) & ".hbn"
    myUci.FilesBlock.AddFromSpecsExt s, "BINO", 92
  End If
  
  Set lOpnBlk = myUci.OpnBlks("PERLND")
  For Each vOper In lOpnBlk.Ids
    Set lOper = vOper
    lOper.Tables("PRINT-INFO").Parms(3).Value = 5
    If Not lOper.TableExists("BINARY-INFO") Then
      lOpnBlk.AddTableForAll "BINARY-INFO", "PERLND"
    End If
    lOper.Tables("BINARY-INFO").Parms(1).Value = 4
    lOper.Tables("BINARY-INFO").Parms(2).Value = 4
    lOper.Tables("BINARY-INFO").Parms(3).Value = 5
    lOper.Tables("BINARY-INFO").Parms(4).Value = 4
    lOper.Tables("BINARY-INFO").Parms(5).Value = 4
    lOper.Tables("BINARY-INFO").Parms(6).Value = 4
    lOper.Tables("BINARY-INFO").Parms(7).Value = 4
    lOper.Tables("BINARY-INFO").Parms(8).Value = 4
    lOper.Tables("BINARY-INFO").Parms(9).Value = 4
    lOper.Tables("BINARY-INFO").Parms(10).Value = 4
    lOper.Tables("BINARY-INFO").Parms(11).Value = 4
    lOper.Tables("BINARY-INFO").Parms(12).Value = 4
    lOper.Tables("GEN-INFO").Parms(4).Value = 91
    lOper.Tables("GEN-INFO").Parms(6).Value = 92
    'update parms
    s = Mid(CStr(lOper.ID), 2)
    Set ltable = lOper.Tables("PWAT-PARM2")
    Select Case s
      Case "01":  ltable.Parms(4) = "250.": ltable.Parms(5) = 0.024
      Case "02":  ltable.Parms(4) = "250.": ltable.Parms(5) = 0.024
      Case "03":  ltable.Parms(4) = "250.": ltable.Parms(5) = 0.024
      Case "04":  ltable.Parms(4) = "250.": ltable.Parms(5) = 0.024: ltable.Parms(7) = 0.975
      Case "05":  ltable.Parms(4) = "250.": ltable.Parms(5) = 0.053
      Case "06":  ltable.Parms(4) = "250.": ltable.Parms(5) = 0.053
      Case "07":  ltable.Parms(4) = "250.": ltable.Parms(5) = 0.053
      Case "08":  ltable.Parms(4) = "300.": ltable.Parms(5) = 0.02
      Case "09":  ltable.Parms(4) = "250.": ltable.Parms(5) = 0.02
      Case "10":  ltable.Parms(4) = "270.": ltable.Parms(5) = 0.024
    End Select
  Next vOper
  
  Set lOpnBlk = myUci.OpnBlks("IMPLND")
  For Each vOper In lOpnBlk.Ids
    Set lOper = vOper
    lOper.Tables("PRINT-INFO").Parms(3).Value = 5
    If Not lOper.TableExists("BINARY-INFO") Then
      lOpnBlk.AddTableForAll "BINARY-INFO", "IMPLND"
    End If
    lOper.Tables("BINARY-INFO").Parms(1).Value = 4
    lOper.Tables("BINARY-INFO").Parms(2).Value = 4
    lOper.Tables("BINARY-INFO").Parms(3).Value = 5
    lOper.Tables("BINARY-INFO").Parms(4).Value = 4
    lOper.Tables("BINARY-INFO").Parms(5).Value = 4
    lOper.Tables("BINARY-INFO").Parms(6).Value = 4
    lOper.Tables("GEN-INFO").Parms(4).Value = 91
    lOper.Tables("GEN-INFO").Parms(6).Value = 92
    'update parms
    Set ltable = lOper.Tables("IWAT-PARM2")
    ltable.Parms(1) = "100."
    ltable.Parms(2) = 0.014
  Next vOper
  
  Set lOpnBlk = myUci.OpnBlks("RCHRES")
  For Each vOper In lOpnBlk.Ids
    Set lOper = vOper
    lOper.Tables("PRINT-INFO").Parms(1).Value = 5
    If Not lOper.TableExists("BINARY-INFO") Then
      lOpnBlk.AddTableForAll "BINARY-INFO", "RCHRES"
    End If
    lOper.Tables("BINARY-INFO").Parms(1).Value = 5
    lOper.Tables("BINARY-INFO").Parms(2).Value = 4
    lOper.Tables("BINARY-INFO").Parms(3).Value = 4
    lOper.Tables("BINARY-INFO").Parms(4).Value = 4
    lOper.Tables("BINARY-INFO").Parms(5).Value = 4
    lOper.Tables("BINARY-INFO").Parms(6).Value = 4
    lOper.Tables("BINARY-INFO").Parms(7).Value = 4
    lOper.Tables("BINARY-INFO").Parms(8).Value = 4
    lOper.Tables("BINARY-INFO").Parms(9).Value = 4
    lOper.Tables("BINARY-INFO").Parms(10).Value = 4
    lOper.Tables("GEN-INFO").Parms(5).Value = 91
    lOper.Tables("GEN-INFO").Parms(8).Value = 92
  Next vOper
    
End Sub

Private Sub MakeMods_AddOutput(myUci As HspfUci)
  Dim s As String
  Dim lOper As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim ltable As HspfTable
  Dim newdsn&, vOpn As Variant, ID&
  
  'figure out which reach is most downstream
  For Each vOpn In myUci.OpnSeqBlock.opns
    Set lOper = vOpn
    If lOper.Name = "RCHRES" Then
      ID = lOper.ID
    End If
  Next vOpn
  'is there output flow here yet?
  Set lOper = myUci.OpnBlks("RCHRES").operfromid(ID)
  found = False
  For i = 1 To lOper.targets.Count
    If Mid(lOper.targets(i).Target.volname, 1, 3) = "WDM" And lOper.targets(i).Target.member = "FLOW" Then
      found = True
    End If
  Next i
  If Not found Then
    'add it
    myUci.AddOutputWDMDataSet "RCH" & CStr(ID), "FLOW", 1000, 1, newdsn
    myUci.AddExtTarget "RCHRES", ID, "HYDR", "RO", 1, 1, 1#, "AVER", _
               "WDM1", newdsn, "FLOW", 1, "ENGL", "AGGR", "REPL"
  End If
  
End Sub

Private Sub MakeMods_AddSeptics(myUci As HspfUci)
  Dim s As String
  Dim lOper As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim ltable As HspfTable
  Dim rOpn As HspfOperation
  Dim copyid&, i&, dsn&(7), mem$(7), mem1&(7), mult(7) As Double
  
  'have we already done this?
  If myUci.OpnBlks("COPY").Ids.Count = 0 Then
    'add new copy operation
    copyid = 1
    myUci.AddOperation "COPY", copyid
    myUci.AddTable "COPY", copyid, "TIMESERIES"
    Set ltable = myUci.OpnBlks("COPY").operfromid(copyid).Tables("TIMESERIES")
    ltable.Parms("NMN") = 7
    'add to opn seq block after last perlnd or implnd
    i = 0
    For Each vOpn In myUci.OpnSeqBlock.opns
      Set lOper = vOpn
      If lOper.Name <> "PERLND" And lOper.Name <> "IMPLND" Then
        Exit For
      End If
      i = i + 1
    Next vOpn
    myUci.OpnSeqBlock.AddAfter myUci.OpnBlks("COPY").operfromid(copyid), i
    
  'WDM2  9101 FLOW     ENGLZERO    0.0825SAME COPY     7     INPUT  MEAN   1
  'WDM2  9113 TSSL     ENGLZERO   0.00025DIV  COPY     7     INPUT  MEAN   2
  'WDM2  9113 TSSL     ENGLZERO   0.00025DIV  COPY     7     INPUT  MEAN   3
  'WDM2  9102 BODL     ENGLZERO      2.47DIV  COPY     7     INPUT  MEAN   4
  'WDM2  9112 PHOSTL   ENGLZERO          DIV  COPY     7     INPUT  MEAN   5
  'WDM2  9117 FECOLL   ENGLZERO          DIV  COPY     7     INPUT  MEAN   6
  'WDM2  9121 NTOTL    ENGLZERO          DIV  COPY     7     INPUT  MEAN   7
  
    'create new ext srcs
    dsn(1) = 9501
    dsn(2) = 9513
    dsn(3) = 9513
    dsn(4) = 9502
    dsn(5) = 9512
    dsn(6) = 9517
    dsn(7) = 9521
    mem(1) = "FLOW"
    mem(2) = "TSSL"
    mem(3) = "TSSL"
    mem(4) = "BODL"
    mem(5) = "PHOSTL"
    mem(6) = "FECOLL"
    mem(7) = "NTOTL"
    mult(1) = 0.0825
    mult(2) = 0.00025
    mult(3) = 0.00025
    mult(4) = 2.47
    mult(5) = 1
    mult(6) = 1
    mult(7) = 1
    Set lOper = myUci.OpnBlks("COPY").operfromid(copyid)
    For i = 1 To 7
      Set lConn = New HspfConnection
      Set lConn.Uci = myUci
      lConn.typ = 1
      lConn.Source.volname = "WDM2"
      lConn.Source.VolId = dsn(i)
      lConn.Source.member = mem(i)
      lConn.Ssystem = "ENGL"
      lConn.Sgapstrg = "ZERO"
      lConn.mfact = mult(i)
      If i = 1 Then
        lConn.tran = "SAME"
      Else
        lConn.tran = "DIV"
      End If
      lConn.Target.volname = "COPY"
      lConn.Target.VolId = copyid
      lConn.Target.group = "INPUT"
      lConn.Target.member = "MEAN"
      lConn.Target.memsub1 = i
      If i = 1 Then
        lConn.Comment = "*** unit leaky septic"
      End If
      myUci.Connections.Add lConn
      Set lConn.Target.Opn = lOper
      lOper.Sources.Add lConn
    Next i
  
  'COPY     7                       665.3     RCHRES   1     99
    
    For Each vOpn In myUci.OpnBlks("RCHRES").Ids
      Set rOpn = vOpn
      Set lConn = New HspfConnection
      Set lConn.Uci = myUci
      lConn.typ = 3
      lConn.Source.volname = "COPY"
      lConn.Source.VolId = copyid
      lConn.mfact = 1
      lConn.Target.volname = RCHRES
      lConn.Target.VolId = rOpn.ID
      lConn.MassLink = 99
      lConn.Comment = ""
      myUci.Connections.Add lConn
      Set lConn.Target.Opn = rOpn
      Set lConn.Source.Opn = lOper
      lOper.Sources.Add lConn
      rOpn.targets.Add lConn
    Next vOpn
  
  '  MASS-LINK       99
  '<-Volume-> <-Grp> <-Member-><--Mult-->     <-Target vols> <-Grp> <-Member->  ***
  '<Name>            <Name> x x<-factor->     <Name>                <Name> x x  ***
  'COPY       OUTPUT MEAN   1                 RCHRES         INFLOW IVOL
  'COPY       OUTPUT MEAN   2                 RCHRES         INFLOW ISED   2
  'COPY       OUTPUT MEAN   3                 RCHRES         INFLOW ISED   3
  'COPY       OUTPUT MEAN   4                 RCHRES         INFLOW OXIF   2
  'COPY       OUTPUT MEAN   5                 RCHRES         INFLOW NUIF1  4
  'COPY       OUTPUT MEAN   6                 RCHRES         INFLOW IDQAL  1
  'COPY       OUTPUT MEAN   7                 RCHRES         INFLOW NUIF1  1
  '  END MASS-LINK   99
    
    mem(1) = "IVOL"
    mem(2) = "ISED"
    mem(3) = "ISED"
    mem(4) = "OXIF"
    mem(5) = "NUIF1"
    mem(6) = "IDQAL"
    mem(7) = "NUIF1"
    mem1(2) = 2
    mem1(3) = 3
    mem1(4) = 2
    mem1(5) = 4
    mem1(6) = 1
    mem1(7) = 1
    For i = 1 To 7
      Set lMassLink = New HspfMassLink
      Set lMassLink.Uci = myUci
      lMassLink.MassLinkID = 99
      lMassLink.Source.volname = "COPY"
      lMassLink.Source.group = "OUTPUT"
      lMassLink.Source.member = "MEAN"
      lMassLink.Source.memsub1 = i
      lMassLink.mfact = 1
      lMassLink.Target.volname = "RCHRES"
      lMassLink.Target.group = "INFLOW"
      lMassLink.Target.member = mem(i)
      If i > 1 Then
        lMassLink.Target.memsub1 = mem1(i)
      End If
      lMassLink.Comment = ""
      myUci.MassLinks.Add lMassLink
    Next i
  End If
  
End Sub

Private Sub MakeMods_RenumberReaches(myUci As HspfUci)
  Dim lConn As HspfConnection
  Dim lOpnBlk As HspfOpnBlk
  Dim lOper As HspfOperation
  Dim inid&
  
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set rOpn = vOpn
    
    inid = rOpn.ID
    Set lOpnBlk = myUci.OpnBlks("RCHRES")
    Set lOper = lOpnBlk.operfromid(inid)
    s = Trim(Mid(lOper.Tables("GEN-INFO").Parms(1).Value, 7))
    newid = ConvertReachId(s)
    
    rOpn.ID = newid
    For Each vConn In myUci.Connections
      Set lConn = vConn
      If lConn.Target.volname = "RCHRES" And lConn.Target.VolId = inid Then
        lConn.Target.VolId = rOpn.ID
      End If
      If lConn.Source.volname = "RCHRES" And lConn.Source.VolId = inid Then
        lConn.Source.VolId = rOpn.ID
      End If
    Next vConn
    
    lOper.Tables("HYDR-PARM2").Parms(2).Value = newid
    lOper.FTable.ID = newid
    
  Next vOpn
  
End Sub

Private Function ConvertReachId(ReachName) As Long
  For i = 1 To 266
    If ReachName = huc12ext(i) Then
      ConvertReachId = uciid(i)
      Exit For
    End If
  Next i
  If i > 266 Then
    MsgBox "unable to find new reach id for " & ReachName, vbOKOnly
  End If
End Function

Private Sub MakeMods_ReplaceFtables(defUci As HspfUci, myUci As HspfUci)
  Dim lOpn As HspfOperation
  
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    Set lOpn.FTable = defUci.OpnBlks("RCHRES").operfromid(lOpn.ID).FTable
    Set lOpn.FTable.operation = lOpn
    lOpn.FTable.ID = lOpn.ID
  Next vOpn
End Sub

Private Sub ReadNewFTables(defUci As HspfUci)
  Dim Init&, OmCode&, retkey&, cbuff$, retcod&
  Dim done As Boolean
  Dim i&, j&, ID&, lOpn As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim lOper As HspfOperation
  
  Filename = "uciupdate\reaches.out"
  Open Filename For Input As #1
  Set lOpnBlk = defUci.OpnBlks("RCHRES")
  Do While lOpnBlk.Count > 0
    lOpnBlk.Ids.Remove 1
  Loop
  
  done = False
  Do Until done
    Line Input #1, cbuff
    Init = 0
    If Mid(cbuff, 3, 6) = "FTABLE" Then  'this is a new one
      ID = CInt(Right(cbuff, 3))
      
      defUci.AddOperation "RCHRES", ID
      
      Set lOpn = defUci.OpnBlks("RCHRES").operfromid(ID)
      Set lFtable = New HspfFtable
      Line Input #1, cbuff
      Line Input #1, cbuff
      Set lOpn.FTable = lFtable
      With lOpn.FTable
        .Nrows = Left(cbuff, 5)
        .Ncols = Mid(cbuff, 6, 5)
        Line Input #1, cbuff
        Line Input #1, cbuff
        For i = 1 To .Nrows
          Line Input #1, cbuff
          .Depth(i) = Left(cbuff, 10)
          .Area(i) = Mid(cbuff, 11, 10)
          .Volume(i) = Mid(cbuff, 21, 10)
          j = .Ncols - 3
          If j > 0 Then
            .Outflow1(i) = Mid(cbuff, 31, 10)
          End If
          If j > 1 Then
            .Outflow2(i) = Mid(cbuff, 41, 10)
          End If
          If j > 2 Then
            .Outflow3(i) = Mid(cbuff, 51, 10)
          End If
          If j > 3 Then
            .Outflow4(i) = Mid(cbuff, 61, 10)
          End If
          If j > 4 Then
            .Outflow5(i) = Mid(cbuff, 71, 10)
          End If
        Next i
      End With
    ElseIf Trim(cbuff) = "END FTABLES" Then
      done = True
    End If
  Loop
  
End Sub

Private Sub MakeMods_UpdateSeptics(myUci As HspfUci)
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection
  
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    numhouse = 0
    For i = 1 To 266
      If IsNumeric(uciid(i)) Then
        If lOpn.ID = uciid(i) Then
          numhouse = septic(i)
          Exit For
        End If
      End If
    Next i
    mult = numhouse * 2.4 * 0.05
    For Each vConn In lOpn.Sources
      Set lConn = vConn
      If lConn.Source.volname = "COPY" Then
        'found copy to this reach, update mult
        lConn.mfact = mult
        lConn.Comment = "*** septic tank failure, #houses * 2.4 people/house * 5% failure"
      End If
    Next vConn
  Next vOpn
End Sub

Private Sub MakeMods_UpdateMet(myUci As HspfUci)
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    rchid = 0
    For i = 1 To 266
      If IsNumeric(uciid(i)) Then
        If lOpn.ID = uciid(i) Then
          rchid = i
          Exit For
        End If
      End If
    Next i
    
    Set lmetseg = New HspfMetSeg
    Set lmetseg.Uci = myUci
    For r = 1 To 8
      lmetseg.metsegrec(r).Source.volname = lOpn.metseg.metsegrec(r).Source.volname
      lmetseg.metsegrec(r).Source.member = lOpn.metseg.metsegrec(r).Source.member
      If lmetseg.metsegrec(r).Source.member = "PREC" Then
        lmetseg.metsegrec(r).mfactp = metmult(rchid)
        lmetseg.metsegrec(r).Mfactr = metmult(rchid)
        lmetseg.metsegrec(r).Source.VolId = metdsn(rchid)
      Else
        lmetseg.metsegrec(r).mfactp = lOpn.metseg.metsegrec(r).mfactp
        lmetseg.metsegrec(r).Mfactr = lOpn.metseg.metsegrec(r).Mfactr
        lmetseg.metsegrec(r).Source.VolId = lOpn.metseg.metsegrec(r).Source.VolId
      End If
      If lmetseg.metsegrec(r).Source.member = "EVAP" Then
        lmetseg.metsegrec(r).Source.member = "PEVT"
        lmetseg.metsegrec(r).Source.VolId = 1206
      End If
      lmetseg.metsegrec(r).Sgapstrg = ""
      lmetseg.metsegrec(r).Ssystem = "ENGL"
      lmetseg.metsegrec(r).tran = "SAME"
      lmetseg.metsegrec(r).typ = r
    Next r
         
    ifound = False
    For Each vmetseg In myUci.MetSegs
      'If vMetSeg.Compare(lmetseg, "PERLND") And vMetSeg.Compare(lmetseg, "RCHRES") Then
        'already exists
        'ifound = True
      'End If
    Next vmetseg
    If Not ifound Then
      lmetseg.ID = myUci.MetSegs.Count + 1
      myUci.MetSegs.Add lmetseg
    End If
    Set lOpn.metseg = lmetseg
  Next vOpn
  
  'set all perlnd/implnds to this new metseg
  For Each vOpn In myUci.OpnBlks("PERLND").Ids
    Set lOpn = vOpn
    Set lOpn.metseg = lmetseg
  Next vOpn
  For Each vOpn In myUci.OpnBlks("IMPLND").Ids
    Set lOpn = vOpn
    Set lOpn.metseg = lmetseg
  Next vOpn
  
End Sub

Private Sub MakeMods_UpdateMetPI(myUci As HspfUci)
  Dim lId&, lOpn As HspfOperation, moved As Boolean, desc$
  Dim vConn As Variant, i&
  Dim upLand As Collection 'of hspfoperations
  Dim upOper As HspfOperation, tOper As HspfOperation
  Dim vupOper As Variant, lupOper As HspfOperation
  Dim lGrouped As Boolean, SelectedMetSeg$, iresp&, iChange&
  Dim j&, tOperName As String, tOperId&, linc&, afterid&, k&
  Dim vTable As Variant, ltable As HspfTable, tabname$, ID&
    
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    'now add metseg to contrib area
    Set upLand = New Collection
    'check if we can apply it to the existing perlnd/implnds, or create new operations
    'find operations contributing to this rchres
    For Each vConn In lOpn.Sources
      Set lConn = vConn
      If lConn.Source.Opn.Name = "PERLND" Or lConn.Source.Opn.Name = "IMPLND" Then
        'found a contributing land area
        Set upOper = lConn.Source.Opn
        upLand.Add upOper
      End If
    Next vConn
    If upLand.Count = 0 Then
    Else
      ifound = False
      If upLand(1).metseg.metsegrec(1).Source.VolId = lOpn.metseg.metsegrec(1).Source.VolId And _
         upLand(1).metseg.metsegrec(1).mfactp = lOpn.metseg.metsegrec(1).mfactp Then
        ifound = True
      End If
      If Not ifound Then
        'create new operations
        'For Each vupOper In upLand
        Set lupOper = upLand(1)
        ibase = CInt(Mid(CStr(lupOper.ID), 1, 1)) * 100
        'create a new set
        For ipi = 1 To 17
          If ipi < 11 Then
            ID = ibase + ipi
            Set lupOper = myUci.OpnBlks("PERLND").operfromid(ID)
          Else
            ID = ibase + ipi - 10
            Set lupOper = myUci.OpnBlks("IMPLND").operfromid(ID)
          End If
          tOperName = lupOper.Name
          If lupOper.ID < 900 Then
            linc = 100
          Else
            linc = 1
          End If
          tOperId = lupOper.ID + linc
          'figure out which to put it after in the opn seq block
          afterid = 1
          For i = 1 To myUci.OpnSeqBlock.opns.Count
            If myUci.OpnSeqBlock.Opn(i).Name = lupOper.Name And _
               myUci.OpnSeqBlock.Opn(i).ID = lupOper.ID Then
              afterid = i
            End If
          Next i
          Do While Not myUci.OpnBlks(tOperName).operfromid(tOperId) Is Nothing
            tOperId = tOperId + linc
          Loop
        
          'add the operation to the uci object
          myUci.AddOperation tOperName, tOperId
          Set tOper = myUci.OpnBlks(tOperName).operfromid(tOperId)
          tOper.Description = lupOper.Description
          Set tOper.metseg = lOpn.metseg
          'add the operation to the opn seq block
          myUci.OpnSeqBlock.AddAfter tOper, afterid
          'copy tables from the existing operation
          For Each vTable In lupOper.Tables
            Set ltable = vTable
            If ltable.OccurCount > 1 And ltable.OccurNum > 1 Then
              tabname = ltable.Name & ":" & ltable.OccurNum
            Else
              tabname = ltable.Name
            End If
            myUci.AddTable tOperName, tOperId, tabname
            For i = 1 To ltable.Parms.Count
              tOper.Tables(tabname).Parms(i) = ltable.Parms(i)
            Next i
          Next vTable
          'move land area from one set to the other
          For Each vConn In lOpn.Sources
            Set lConn = vConn
            If lConn.Source.volname = lupOper.Name And _
               lConn.Source.VolId = lupOper.ID Then
              Set lConn.Source.Opn = tOper
              lConn.Source.VolId = tOper.ID
            End If
          Next vConn
          k = 1
          For Each vConn In lupOper.targets
            Set lConn = vConn
            If lConn.Target.volname = lOpn.Name And _
               lConn.Target.VolId = lOpn.ID Then
              'remove this target
              lupOper.targets.Remove k
              'set this as a target from the new opn
              tOper.targets.Add lConn
            Else
              k = k + 1
            End If
          Next vConn
        Next ipi
      End If
    End If
  Next vOpn
    
End Sub

Private Sub MakeMods_AddPointSources(myUci As HspfUci)
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  Dim vPt As Variant, lPt As HspfPoint
  
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    For i = 1 To 100
      If reachid(i) = lOpn.ID Then
        found = False
        For Each vPt In myUci.PointSources
          Set lPt = vPt
          If lPt.Source.VolId = ptdsn(i) Then
            found = True
            Exit For
          End If
        Next vPt
        If Not found Then
          'create new ext src
          Set lConn = New HspfConnection
          Set lConn.Uci = myUci
          lConn.typ = 1
          lConn.Source.volname = "WDM2"
          lConn.Source.VolId = ptdsn(i)
          lConn.Source.member = "FLOW"
          lConn.Ssystem = "ENGL"
          lConn.Sgapstrg = "ZERO"
          lConn.mfact = 0.0825
          lConn.tran = "SAME"
          lConn.Target.volname = lOpn.Name
          lConn.Target.VolId = lOpn.ID
          lConn.Target.group = "INFLOW"
          lConn.Target.member = "IVOL"
        
          myUci.Connections.Add lConn
          Set lConn.Target.Opn = lOpn
          lOpn.Sources.Add lConn
        End If
      End If
    Next i
  Next vOpn
End Sub

Private Sub MakeMods_RenumberSeptics(myUci As HspfUci)
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  Dim vPt As Variant, lPt As HspfPoint
  
  For Each vPt In myUci.PointSources
    Set lPt = vPt
    If lPt.Source.VolId = 9101 Then
      lPt.Source.VolId = 9501
    ElseIf lPt.Source.VolId = 9102 Then
      lPt.Source.VolId = 9502
    ElseIf lPt.Source.VolId = 9112 Then
      lPt.Source.VolId = 9512
    ElseIf lPt.Source.VolId = 9113 Then
      lPt.Source.VolId = 9513
    ElseIf lPt.Source.VolId = 9117 Then
      lPt.Source.VolId = 9517
    ElseIf lPt.Source.VolId = 9121 Then
      lPt.Source.VolId = 9521
    End If
  Next vPt
  
End Sub

Private Sub MakeMods_HPCP(myUci As HspfUci)
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  
  For Each vmetseg In myUci.MetSegs
    Set lmetseg = vmetseg
    lmetseg.metsegrec(1).Source.member = "HPCP"
  Next vmetseg
  
End Sub

Private Sub MakeMods_WaterIntakes(myUci As HspfUci)
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  Dim lpts As HspfPoint
  
  For Each vpts In myUci.PointSources
    Set lpts = vpts
    If lpts.Source.VolId > 9300 And lpts.Source.VolId < 9500 Then
      'this is a water intake
      lpts.mfact = 1#
      lpts.Target.group = "EXTNL"
      lpts.Target.member = "OUTDGT"
      lpts.Target.memsub1 = 2
      Set lOpn = lpts.Target.Opn
      lOpn.Tables("GEN-INFO").Parms(2).Value = 2
      lOpn.Tables("HYDR-PARM1").Parms(11).Value = 2
    End If
  Next vpts
  
End Sub

Private Sub MakeMods_AddWQ(defUci As HspfUci, myUci As HspfUci)
  ReadPollutants
  For j = 1 To defUci.Pollutants.Count
    Set lPoll = defUci.Pollutants(j)
    myUci.Pollutants.Add lPoll
  Next j
  myUci.PollutantsUnBuild
  Call CheckAndAddMissingTables("PERLND")
  Call CheckAndAddMissingTables("IMPLND")
  Call CheckAndAddMissingTables("RCHRES")
  Call UpdateFlagDependencies("RCHRES")
  Call SetMissingValuesToDefaults(myUci, defUci)
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    lOpn.Tables("GQ-GENDATA").Parms(2).Value = 1
  Next vOpn
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
  
  Do While defUci.Pollutants.Count > 0
    defUci.Pollutants.Remove 1
  Loop
  
  i = FreeFile(0)
  On Error GoTo ErrHandler
  tname = W_STARTERPATH & "..\..\uciupdate\" & "pollutants.txt"
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
            lPoll.ID = defUci.Pollutants.Count + 1
            lPoll.index = myUci.Pollutants.Count + 1
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
                lOpnBlk.Ids.Add lOper, coptyp & lOper.ID
              Next vOper
              For Each vOper In myUci.OpnBlks(coptyp).Ids
                Set lOper = vOper
                Set tOper = New HspfOperation
                tOper.Name = lOper.Name
                tOper.ID = lOper.ID
                tOper.Description = lOper.Description
                tOper.DefOpnId = DefaultOpnId(tOper, defUci)
                Set tOper.OpnBlk = lOpnBlk
                lPoll.Operations.Add tOper, coptyp & tOper.ID
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
                    lML.Source.memsub1 = 0
                  Else
                    lML.Source.memsub1 = CInt(istr)
                  End If
                  istr = Trim(Mid(lstr, 28, 1))
                  If Len(istr) = 0 Then
                    lML.Source.memsub2 = 0
                  Else
                    lML.Source.memsub2 = CInt(istr)
                  End If
                  istr = Trim(Mid(lstr, 30, 10))
                  If Len(istr) = 0 Then
                    lML.mfact = 1
                  Else
                    'lML.MFact = CSng(istr)
                    lML.mfact = istr
                  End If
                  lML.Target.volname = Trim(Mid(lstr, 44, 6))
                  lML.Target.group = Trim(Mid(lstr, 59, 6))
                  lML.Target.member = Trim(Mid(lstr, 66, 6))
                  istr = Trim(Mid(lstr, 73, 1))
                  If Len(istr) = 0 Then
                    lML.Target.memsub1 = 0
                  Else
                    lML.Target.memsub1 = CInt(istr)
                  End If
                  istr = Trim(Mid(lstr, 75, 1))
                  If Len(istr) = 0 Then
                    lML.Target.memsub2 = 0
                  Else
                    lML.Target.memsub2 = CInt(istr)
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

Private Sub MakeMods_AddPointSourcesWQ(myUci As HspfUci)
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  Dim vPt As Variant, lPt As HspfPoint
  Dim smember$(15), tmember$(15), mult(15) As Double, tran$(15), tsub1&(15)
  
  smember(1) = "FLOW" 'as temp
  smember(2) = "PHOSTL"
  smember(3) = "TSSL"
  smember(4) = "FECOLL"
  smember(5) = "BODL"
  smember(6) = "" '"TOTN" 'not used?
  smember(7) = "NH3L"
  smember(8) = "NO3L"
  smember(9) = "PBTOTL"
  smember(10) = "ZNTOTL"
  smember(11) = "ZNTOTL"
  smember(12) = "PBTOTL"
  smember(13) = "" '"PH" 'not used?
  smember(14) = "" '"TEMP" 'not used?
  smember(15) = "DO2L"
  
  mult(1) = 7410000
  mult(2) = 1
  mult(3) = 0.00025
  mult(4) = 1
  mult(5) = 2.47
  mult(6) = 1
  mult(7) = 1
  mult(8) = 1
  mult(9) = 1
  mult(10) = 1
  mult(11) = 1
  mult(12) = 1
  mult(13) = 1
  mult(14) = 1
  mult(15) = 1
  
  tran(1) = "SAME"
  tran(2) = "DIV"
  tran(3) = "DIV"
  tran(4) = "DIV"
  tran(5) = "DIV"
  tran(6) = "DIV"
  tran(7) = "DIV"
  tran(8) = "DIV"
  tran(9) = "DIV"
  tran(10) = "DIV"
  tran(11) = "DIV"
  tran(12) = "DIV"
  tran(13) = "DIV"
  tran(14) = "DIV"
  tran(15) = "DIV"
  
  tmember(1) = "IHEAT" 'as temp
  tmember(2) = "NUIF1"
  tmember(3) = "ISED"
  tmember(4) = "IDQAL"
  tmember(5) = "OXIF"
  tmember(6) = ""
  tmember(7) = "NUIF1"
  tmember(8) = "NUIF1"
  tmember(9) = "IDQAL"
  tmember(10) = "IDQAL"
  tmember(11) = "IDQAL"
  tmember(12) = "IDQAL"
  tmember(13) = ""
  tmember(14) = ""
  tmember(15) = "OXIF"
  
  tsub1(1) = 1
  tsub1(2) = 4
  tsub1(3) = 2
  tsub1(4) = 1
  tsub1(5) = 2
  tsub1(6) = 1
  tsub1(7) = 2
  tsub1(8) = 1
  tsub1(9) = 3
  tsub1(10) = 2
  tsub1(11) = 2
  tsub1(12) = 3
  tsub1(13) = 1
  tsub1(14) = 1
  tsub1(15) = 1
  
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    For j = 1 To 15
      For i = 1 To 100
        If reachid(i) = lOpn.ID Then
          found = False
          For Each vPt In myUci.PointSources
            Set lPt = vPt
            If lPt.Source.VolId = ptdsn2(j, i) Then
              found = True
              Exit For
            End If
          Next vPt
          If Not found Then
            If ptdsn2(j, i) <> 0 And Len(smember(j)) > 0 Then
              'create new ext src
              
              Set lConn = New HspfConnection
              Set lConn.Uci = myUci
              lConn.typ = 1
              lConn.Source.volname = "WDM2"
              lConn.Source.VolId = ptdsn2(j, i)
              lConn.Source.member = smember(j)
              lConn.Ssystem = "ENGL"
              lConn.Sgapstrg = "ZERO"
              lConn.mfact = mult(j)
              If tran(j) = "DIV" And ptdsn2(j, i) > 3999 And ptdsn2(j, i) < 5000 Then
                tran(j) = "SAME"
              Else
                lConn.tran = tran(j)
              End If
              lConn.Target.volname = lOpn.Name
              lConn.Target.VolId = lOpn.ID
              lConn.Target.group = "INFLOW"
              lConn.Target.member = tmember(j)
              lConn.Target.memsub1 = tsub1(j)
            
              myUci.Connections.Add lConn
              Set lConn.Target.Opn = lOpn
              lOpn.Sources.Add lConn
              
              If j = 3 Then
                'do twice for sed
                Set lConn = New HspfConnection
                Set lConn.Uci = myUci
                lConn.typ = 1
                lConn.Source.volname = "WDM2"
                lConn.Source.VolId = ptdsn2(j, i)
                lConn.Source.member = smember(j)
                lConn.Ssystem = "ENGL"
                lConn.Sgapstrg = "ZERO"
                lConn.mfact = mult(j)
                If tran(j) = "DIV" And ptdsn2(j, i) > 3999 And ptdsn2(j, i) < 5000 Then
                  tran(j) = "SAME"
                Else
                  lConn.tran = tran(j)
                End If
                lConn.Target.volname = lOpn.Name
                lConn.Target.VolId = lOpn.ID
                lConn.Target.group = "INFLOW"
                lConn.Target.member = tmember(j)
                lConn.Target.memsub1 = tsub1(j) + 1
              
                myUci.Connections.Add lConn
                Set lConn.Target.Opn = lOpn
                lOpn.Sources.Add lConn
              End If
              
            End If
          End If
        End If
      Next i
    Next j
  Next vOpn
End Sub

Private Sub MakeMods_AddExtTarLinkWQ(myUci As HspfUci)
  Dim s As String, desc$, group$, mfact!, tran$
  Dim lOper As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim ltable As HspfTable
  Dim newdsn&, vOpn As Variant, ID&
  Dim tstype$(39), member$(39), memsub1&(39), memsub2&(39)
  
  tstype(1) = "HEAT":    member(1) = "ROHEAT":  memsub1(1) = 1:  memsub2(1) = 1
  tstype(2) = "SED1":    member(2) = "ROSED":   memsub1(2) = 1:  memsub2(2) = 1
  tstype(3) = "SED2":    member(3) = "ROSED":   memsub1(3) = 2:  memsub2(3) = 1
  tstype(4) = "SED3":    member(4) = "ROSED":   memsub1(4) = 3:  memsub2(4) = 1
  tstype(5) = "DQAL1":   member(5) = "RODQAL":  memsub1(5) = 1:  memsub2(5) = 1
  tstype(6) = "DQAL2":   member(6) = "RODQAL":  memsub1(6) = 2:  memsub2(6) = 1
  tstype(7) = "DQAL3":   member(7) = "RODQAL":  memsub1(7) = 3:  memsub2(7) = 1
  tstype(8) = "SQAL12":  member(8) = "ROSQAL":  memsub1(8) = 1:  memsub2(8) = 2
  tstype(9) = "SQAL22":  member(9) = "ROSQAL":  memsub1(9) = 2:  memsub2(9) = 2
  tstype(10) = "SQAL32": member(10) = "ROSQAL": memsub1(10) = 3: memsub2(10) = 2
  tstype(11) = "SQAL13": member(11) = "ROSQAL": memsub1(11) = 1: memsub2(11) = 3
  tstype(12) = "SQAL23": member(12) = "ROSQAL": memsub1(12) = 2: memsub2(12) = 3
  tstype(13) = "SQAL33": member(13) = "ROSQAL": memsub1(13) = 3: memsub2(13) = 3
  tstype(14) = "OXF11":  member(14) = "OXCF1":  memsub1(14) = 1: memsub2(14) = 1
  tstype(15) = "OXF12":  member(15) = "OXCF1":  memsub1(15) = 2: memsub2(15) = 1
  tstype(16) = "NUF11":  member(16) = "NUCF1":  memsub1(16) = 1: memsub2(16) = 1
  tstype(17) = "NUF12":  member(17) = "NUCF1":  memsub1(17) = 2: memsub2(17) = 1
  tstype(18) = "NUF13":  member(18) = "NUCF1":  memsub1(18) = 3: memsub2(18) = 1
  tstype(19) = "NUF14":  member(19) = "NUCF1":  memsub1(19) = 4: memsub2(19) = 1
  tstype(20) = "NUF211": member(20) = "NUCF2":  memsub1(20) = 1: memsub2(20) = 1
  tstype(21) = "NUF221": member(21) = "NUCF2":  memsub1(21) = 2: memsub2(21) = 1
  tstype(22) = "NUF231": member(22) = "NUCF2":  memsub1(22) = 3: memsub2(22) = 1
  tstype(23) = "NUF212": member(23) = "NUCF2":  memsub1(23) = 1: memsub2(23) = 2
  tstype(24) = "NUF222": member(24) = "NUCF2":  memsub1(24) = 2: memsub2(24) = 2
  tstype(25) = "NUF232": member(25) = "NUCF2":  memsub1(25) = 3: memsub2(25) = 2
  tstype(26) = "PKF11":  member(26) = "PKCF1":  memsub1(26) = 1: memsub2(26) = 1
  tstype(27) = "PKF12":  member(27) = "PKCF1":  memsub1(27) = 2: memsub2(27) = 1
  tstype(28) = "PKF13":  member(28) = "PKCF1":  memsub1(28) = 3: memsub2(28) = 1
  tstype(29) = "PKF14":  member(29) = "PKCF1":  memsub1(29) = 4: memsub2(29) = 1
  'conc output
  tstype(30) = "DOX":    member(30) = "DOX":    memsub1(30) = 1: memsub2(30) = 1
  tstype(31) = "TW":     member(31) = "TW":     memsub1(31) = 1: memsub2(31) = 1
  tstype(32) = "FECOLC": member(32) = "DQAL":   memsub1(32) = 1: memsub2(32) = 1
  tstype(33) = "ZINCDC": member(33) = "DQAL":   memsub1(33) = 2: memsub2(33) = 1
  tstype(34) = "LEADDC": member(34) = "DQAL":   memsub1(34) = 3: memsub2(34) = 1
  tstype(35) = "BOD":    member(35) = "BOD":    memsub1(35) = 1: memsub2(35) = 1
  tstype(36) = "NO3C":   member(36) = "DNUST":  memsub1(36) = 1: memsub2(36) = 1
  tstype(37) = "SDTOTC": member(37) = "SSED":   memsub1(37) = 4: memsub2(37) = 1
  tstype(38) = "PO4DC":  member(38) = "DNUST":  memsub1(38) = 4: memsub2(38) = 1
  tstype(39) = "TAMDC":  member(39) = "DNUST":  memsub1(39) = 2: memsub2(39) = 1


  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOper = vOpn
    'is there output flow here?
    found = 0
    For i = 1 To lOper.targets.Count
      If Mid(lOper.targets(i).Target.volname, 1, 3) = "WDM" And lOper.targets(i).Target.member = "FLOW" Then
        found = i
        Exit For
      End If
    Next i
    If found > 0 Then
      'add whole set like this
      desc = Mid(lOper.Description, 8)
      For i = 1 To 39
        If i < 30 Then
          group = "ROFLOW"
          mfact = 1#
          tran = "SAME"
        ElseIf i = 30 Then
          group = "OXRX"
          mfact = 1#
          tran = "AVER"
        ElseIf i = 31 Then
          group = "HTRCH"
          mfact = 1#
          tran = "AVER"
        ElseIf i = 32 Then
          group = "GQUAL"
          mfact = 100000000#
          tran = "AVER"
        ElseIf i = 33 Then
          group = "GQUAL"
          mfact = 1#
          tran = "AVER"
        ElseIf i = 34 Then
          group = "GQUAL"
          mfact = 1#
          tran = "AVER"
        ElseIf i = 35 Then
          group = "OXRX"
          mfact = 1#
          tran = "AVER"
        ElseIf i = 36 Then
          group = "NUTRX"
          mfact = 1#
          tran = "AVER"
        ElseIf i = 37 Then
          group = "SEDTRN"
          mfact = 1#
          tran = "AVER"
        ElseIf i = 38 Then
          group = "NUTRX"
          mfact = 1#
          tran = "AVER"
        ElseIf i = 39 Then
          group = "NUTRX"
          mfact = 1#
          tran = "AVER"
        End If
        myUci.AddOutputWDMDataSetExt "R" & CStr(lOper.ID), tstype(i), _
          lOper.targets(found).Target.VolId, CInt(Mid(lOper.targets(found).Target.volname, 4, 1)), 3, desc, newdsn
        myUci.AddExtTarget "RCHRES", lOper.ID, group, member(i), memsub1(i), _
          memsub2(i), mfact, tran, lOper.targets(found).Target.volname, _
          newdsn, tstype(i), 1, "ENGL", "AGGR", "REPL"
      Next i
    End If
  Next vOpn
  
End Sub

Private Sub MakeMods_AddExtSrcLinkWQ(myUci As HspfUci)
  Dim s As String
  Dim lOper As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim ltable As HspfTable
  Dim newdsn&, vOpn As Variant, ID&
  Dim tstype$(29), member$(29), memsub1&(29), memsub2&(29)
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  Dim vPt As Variant, lPt As HspfPoint
  
  tstype(1) = "HEAT":    member(1) = "IHEAT":  memsub1(1) = 1:  memsub2(1) = 1
  tstype(2) = "SED1":    member(2) = "ISED":   memsub1(2) = 1:  memsub2(2) = 1
  tstype(3) = "SED2":    member(3) = "ISED":   memsub1(3) = 2:  memsub2(3) = 1
  tstype(4) = "SED3":    member(4) = "ISED":   memsub1(4) = 3:  memsub2(4) = 1
  tstype(5) = "DQAL1":   member(5) = "IDQAL":  memsub1(5) = 1:  memsub2(5) = 1
  tstype(6) = "DQAL2":   member(6) = "IDQAL":  memsub1(6) = 2:  memsub2(6) = 1
  tstype(7) = "DQAL3":   member(7) = "IDQAL":  memsub1(7) = 3:  memsub2(7) = 1
  tstype(8) = "SQAL12":  member(8) = "ISQAL":  memsub1(8) = 1:  memsub2(8) = 2
  tstype(9) = "SQAL22":  member(9) = "ISQAL":  memsub1(9) = 2:  memsub2(9) = 2
  tstype(10) = "SQAL32": member(10) = "ISQAL": memsub1(10) = 3: memsub2(10) = 2
  tstype(11) = "SQAL13": member(11) = "ISQAL": memsub1(11) = 1: memsub2(11) = 3
  tstype(12) = "SQAL23": member(12) = "ISQAL": memsub1(12) = 2: memsub2(12) = 3
  tstype(13) = "SQAL33": member(13) = "ISQAL": memsub1(13) = 3: memsub2(13) = 3
  tstype(14) = "OXF11":  member(14) = "OXIF":  memsub1(14) = 1: memsub2(14) = 1
  tstype(15) = "OXF12":  member(15) = "OXIF":  memsub1(15) = 2: memsub2(15) = 1
  tstype(16) = "NUF11":  member(16) = "NUIF1":  memsub1(16) = 1: memsub2(16) = 1
  tstype(17) = "NUF12":  member(17) = "NUIF1":  memsub1(17) = 2: memsub2(17) = 1
  tstype(18) = "NUF13":  member(18) = "NUIF1":  memsub1(18) = 3: memsub2(18) = 1
  tstype(19) = "NUF14":  member(19) = "NUIF1":  memsub1(19) = 4: memsub2(19) = 1
  tstype(20) = "NUF211": member(20) = "NUIF2":  memsub1(20) = 1: memsub2(20) = 1
  tstype(21) = "NUF221": member(21) = "NUIF2":  memsub1(21) = 2: memsub2(21) = 1
  tstype(22) = "NUF231": member(22) = "NUIF2":  memsub1(22) = 3: memsub2(22) = 1
  tstype(23) = "NUF212": member(23) = "NUIF2":  memsub1(23) = 1: memsub2(23) = 2
  tstype(24) = "NUF222": member(24) = "NUIF2":  memsub1(24) = 2: memsub2(24) = 2
  tstype(25) = "NUF232": member(25) = "NUIF2":  memsub1(25) = 3: memsub2(25) = 2
  tstype(26) = "PKF11":  member(26) = "PKIF":  memsub1(26) = 1: memsub2(26) = 1
  tstype(27) = "PKF12":  member(27) = "PKIF":  memsub1(27) = 2: memsub2(27) = 1
  tstype(28) = "PKF13":  member(28) = "PKIF":  memsub1(28) = 3: memsub2(28) = 1
  tstype(29) = "PKF14":  member(29) = "PKIF":  memsub1(29) = 4: memsub2(29) = 1

  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    For i = 1 To lOpn.PointSources.Count
      Set lPt = lOpn.PointSources(i)
      If (lPt.Source.volname = "WDM1" Or lPt.Source.volname = "WDM3") And lPt.Source.member = "FLOW" Then
        'this is an upstream input1
        'create new ext src set
        For j = 1 To 29
          Set lConn = New HspfConnection
          Set lConn.Uci = myUci
          lConn.typ = 1
          lConn.Source.volname = lPt.Source.volname
          lConn.Source.VolId = lPt.Source.VolId + j
          lConn.Source.member = tstype(j)
          lConn.Ssystem = "ENGL"
          lConn.Sgapstrg = ""
          lConn.mfact = 1
          lConn.tran = "SAME"
          lConn.Target.volname = lOpn.Name
          lConn.Target.VolId = lOpn.ID
          lConn.Target.group = "INFLOW"
          lConn.Target.member = member(j)
          lConn.Target.memsub1 = memsub1(j)
          lConn.Target.memsub2 = memsub2(j)
          
          myUci.Connections.Add lConn
          Set lConn.Target.Opn = lOpn
          lOpn.Sources.Add lConn
        Next j
      End If
    Next i
  Next vOpn
End Sub

Private Sub MakeMods_UpdatePrintFlags(myUci As HspfUci)

  Set lOpnBlk = myUci.OpnBlks("PERLND")
  For Each vOper In lOpnBlk.Ids
    Set lOper = vOper
    For i = 1 To 12
      lOper.Tables("PRINT-INFO").Parms(i).Value = 5
      lOper.Tables("BINARY-INFO").Parms(i).Value = 5
    Next i
  Next vOper
  
  Set lOpnBlk = myUci.OpnBlks("IMPLND")
  For Each vOper In lOpnBlk.Ids
    Set lOper = vOper
    For i = 1 To 6
      lOper.Tables("PRINT-INFO").Parms(i).Value = 5
      lOper.Tables("BINARY-INFO").Parms(i).Value = 5
    Next i
  Next vOper
  
  Set lOpnBlk = myUci.OpnBlks("RCHRES")
  For Each vOper In lOpnBlk.Ids
    Set lOper = vOper
    For i = 1 To 10
      lOper.Tables("PRINT-INFO").Parms(i).Value = 5
      lOper.Tables("BINARY-INFO").Parms(i).Value = 5
    Next i
  Next vOper
End Sub

Private Sub MakeMods_UpdateWQTables()
  Dim lOper As HspfOperation
  Dim ltable As HspfTable
  Dim dtable As HspfTable
  Dim dOper As HspfOperation
  Dim dOpnBlk As HspfOpnBlk
  
  Set lOpnBlk = myUci.OpnBlks("PERLND")
  Set dOpnBlk = defUci.OpnBlks("PERLND")
  For Each vOper In lOpnBlk.Ids
    Set lOper = vOper
    lOper.DefOpnId = DefaultOpnId(lOper, defUci)
    Set dOper = dOpnBlk.operfromid(lOper.DefOpnId)
    
    Set ltable = lOper.Tables("MON-POTFW")
    Set dtable = dOper.Tables("MON-POTFW")
    For i = 1 To ltable.Parms.Count
      ltable.Parms(i).Value = dtable.Parms(i).Value
    Next i
    
    Set ltable = lOper.Tables("MON-ACCUM:4")
    Set dtable = dOper.Tables("MON-ACCUM")
    For i = 1 To ltable.Parms.Count
      ltable.Parms(i).Value = dtable.Parms(i).Value
    Next i
    
    Set ltable = lOper.Tables("MON-SQOLIM:4")
    Set dtable = dOper.Tables("MON-SQOLIM")
    For i = 1 To ltable.Parms.Count
      ltable.Parms(i).Value = dtable.Parms(i).Value
    Next i
    
    Set ltable = lOper.Tables("SED-PARM3")
    Set dtable = dOper.Tables("SED-PARM3")
    For i = 1 To ltable.Parms.Count
      ltable.Parms(i).Value = dtable.Parms(i).Value
    Next i
    
  Next vOper
  
  Set lOpnBlk = myUci.OpnBlks("IMPLND")
  Set dOpnBlk = defUci.OpnBlks("IMPLND")
  For Each vOper In lOpnBlk.Ids
    Set lOper = vOper
    lOper.DefOpnId = DefaultOpnId(lOper, defUci)
    Set dOper = dOpnBlk.operfromid(lOper.DefOpnId)
    
    Set ltable = lOper.Tables("QUAL-INPUT:5")
    Set dtable = dOper.Tables("QUAL-INPUT")
    For i = 1 To ltable.Parms.Count
      ltable.Parms(i).Value = dtable.Parms(i).Value
    Next i
  
  Next vOper
  
End Sub

Private Sub MakeMods_UpdatePhos()
  Dim lOper As HspfOperation
  Dim ltable As HspfTable
  
  Set lOpnBlk = myUci.OpnBlks("PERLND")
  For Each vOper In lOpnBlk.Ids
    Set lOper = vOper
    If UCase(Mid(lOper.Description, 1, 6)) = "FOREST" Then
      'update mon-potfw
      Set ltable = lOper.Tables("MON-POTFW")
      ltable.Parms(1).Value = 0.02
      ltable.Parms(2).Value = 0.02
      ltable.Parms(3).Value = 0.02
      ltable.Parms(4).Value = 0.04
      ltable.Parms(5).Value = 0.04
      ltable.Parms(6).Value = 0.04
      ltable.Parms(7).Value = 0.04
      ltable.Parms(8).Value = 0.04
      ltable.Parms(9).Value = 0.04
      ltable.Parms(10).Value = 0.04
      ltable.Parms(11).Value = 0.02
      ltable.Parms(12).Value = 0.02
      'update mon-iflw-conc
      Set ltable = lOper.Tables("MON-IFLW-CONC:3")
      ltable.Parms(1).Value = 0.007
      ltable.Parms(2).Value = 0.007
      ltable.Parms(3).Value = 0.007
      ltable.Parms(4).Value = 0.007
      ltable.Parms(5).Value = 0.007
      ltable.Parms(6).Value = 0.007
      ltable.Parms(7).Value = 0.007
      ltable.Parms(8).Value = 0.007
      ltable.Parms(9).Value = 0.007
      ltable.Parms(10).Value = 0.007
      ltable.Parms(11).Value = 0.007
      ltable.Parms(12).Value = 0.007
      'update mon-grnd-conc
      Set ltable = lOper.Tables("MON-GRND-CONC:3")
      ltable.Parms(1).Value = 0.005
      ltable.Parms(2).Value = 0.005
      ltable.Parms(3).Value = 0.005
      ltable.Parms(4).Value = 0.005
      ltable.Parms(5).Value = 0.005
      ltable.Parms(6).Value = 0.005
      ltable.Parms(7).Value = 0.005
      ltable.Parms(8).Value = 0.005
      ltable.Parms(9).Value = 0.005
      ltable.Parms(10).Value = 0.005
      ltable.Parms(11).Value = 0.005
      ltable.Parms(12).Value = 0.005
    ElseIf UCase(Mid(lOper.Description, 1, 6)) = "AGRICU" Then
      'update mon-iflw-conc
      Set ltable = lOper.Tables("MON-IFLW-CONC:3")
      ltable.Parms(1).Value = 0.02
      ltable.Parms(2).Value = 0.04
      ltable.Parms(3).Value = 0.04
      ltable.Parms(4).Value = 0.04
      ltable.Parms(5).Value = 0.04
      ltable.Parms(6).Value = 0.04
      ltable.Parms(7).Value = 0.04
      ltable.Parms(8).Value = 0.04
      ltable.Parms(9).Value = 0.04
      ltable.Parms(10).Value = 0.02
      ltable.Parms(11).Value = 0.01
      ltable.Parms(12).Value = 0.01
      'update mon-grnd-conc
      Set ltable = lOper.Tables("MON-GRND-CONC:3")
      ltable.Parms(1).Value = 0.005
      ltable.Parms(2).Value = 0.005
      ltable.Parms(3).Value = 0.005
      ltable.Parms(4).Value = 0.005
      ltable.Parms(5).Value = 0.005
      ltable.Parms(6).Value = 0.005
      ltable.Parms(7).Value = 0.005
      ltable.Parms(8).Value = 0.005
      ltable.Parms(9).Value = 0.005
      ltable.Parms(10).Value = 0.005
      ltable.Parms(11).Value = 0.005
      ltable.Parms(12).Value = 0.005
    End If
  Next vOper
  
End Sub

Private Sub MakeMods_UpdateDIV(myUci As HspfUci)
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  Dim vPt As Variant, lPt As HspfPoint
  Dim smember$(15), tmember$(15), mult(15) As Double, tran$(15), tsub1&(15)

  For Each vPt In myUci.PointSources
    Set lPt = vPt
    If lPt.Source.volname = "WDM2" And lPt.Source.VolId < 9300 And lPt.Source.VolId > 4800 Then
      If lPt.Source.member <> "FLOW" Then
        lPt.tran = "DIV "
      End If
    End If
  Next vPt
          
End Sub

Private Sub MakeMods_AddBMPs(myUci As HspfUci)
  Dim s As String
  Dim lOper As HspfOperation, vOper As Variant
  Dim lOpnBlk As HspfOpnBlk
  Dim ltable As HspfTable
  Dim rOpn As HspfOperation, tmem$(23), tmem1&(23), tmem2&(23)
  Dim bmpid&, i&, mem$(23), grp$(23), mem1&(23), mult(23) As Double
  
  s = myUci.Name & " Future with BMPs"
  myUci.GlobalBlock.RunInf.Value = s
  
  If myUci.OpnBlks("BMPRAC").Ids.Count = 0 Then
    'add new bmp operations
    For Each vOper In myUci.OpnBlks("RCHRES").Ids
      Set lOper = vOper
      ctmp = Right(CStr(lOper.ID), 2)
      bmpid = 100 + Int(ctmp)
      myUci.AddOperation "BMPRAC", bmpid
      bmpid = 200 + Int(ctmp)
      myUci.AddOperation "BMPRAC", bmpid
    Next vOper
    'add to opn seq block just before each rchres
    i = 1
    Do While i <= myUci.OpnSeqBlock.opns.Count
      Set lOper = myUci.OpnSeqBlock.opns(i)
      If lOper.Name = "RCHRES" Then
        ctmp = Right(CStr(lOper.ID), 2)
        bmpid = 200 + Int(ctmp)
        myUci.OpnSeqBlock.AddBefore myUci.OpnBlks("BMPRAC").operfromid(bmpid), i
        bmpid = 100 + Int(ctmp)
        myUci.OpnSeqBlock.AddBefore myUci.OpnBlks("BMPRAC").operfromid(bmpid), i
        i = i + 2
      End If
      i = i + 1
    Loop
    'add tables for each bmprac
    For Each vOper In myUci.OpnBlks("BMPRAC").Ids
      Set lOper = vOper
      myUci.AddTable "BMPRAC", lOper.ID, "PRINT-INFO"
      myUci.AddTable "BMPRAC", lOper.ID, "GEN-INFO"
      myUci.AddTable "BMPRAC", lOper.ID, "SED-FRAC"
      myUci.AddTable "BMPRAC", lOper.ID, "DNUT-FRAC"
      myUci.AddTable "BMPRAC", lOper.ID, "ADSNUT-FRAC"
      lOper.Tables("PRINT-INFO").Parms(1) = 5
      lOper.Tables("PRINT-INFO").Parms(2) = 5
      lOper.Tables("PRINT-INFO").Parms(3) = 5
      lOper.Tables("PRINT-INFO").Parms(4) = 5
      lOper.Tables("PRINT-INFO").Parms(5) = 5
      lOper.Tables("PRINT-INFO").Parms(6) = 5
      lOper.Tables("PRINT-INFO").Parms(7) = 5
      lOper.Tables("PRINT-INFO").Parms(8) = 5
      lOper.Tables("PRINT-INFO").Parms(10) = 1
      lOper.Tables("PRINT-INFO").Parms(11) = 9
      lOper.Tables("GEN-INFO").Parms(4) = 3
      lOper.Tables("GEN-INFO").Parms(7) = 91
      If lOper.ID < 200 Then
        lOper.Tables("GEN-INFO").Parms(1) = "New Development"
        lOper.Tables("SED-FRAC").Parms(1) = 0.8
        lOper.Tables("SED-FRAC").Parms(2) = 0.8
        lOper.Tables("SED-FRAC").Parms(3) = 0.8
        lOper.Tables("DNUT-FRAC").Parms(4) = 0.5
        lOper.Tables("ADSNUT-FRAC").Parms(4) = 0.5
        lOper.Tables("ADSNUT-FRAC").Parms(5) = 0.5
        lOper.Tables("ADSNUT-FRAC").Parms(6) = 0.5
      Else
        lOper.Tables("GEN-INFO").Parms(1) = "Retrofit"
        lOper.Tables("SED-FRAC").Parms(1) = 0.5
        lOper.Tables("SED-FRAC").Parms(2) = 0.5
        lOper.Tables("SED-FRAC").Parms(3) = 0.5
        lOper.Tables("DNUT-FRAC").Parms(4) = 0.3
        lOper.Tables("ADSNUT-FRAC").Parms(4) = 0.3
        lOper.Tables("ADSNUT-FRAC").Parms(5) = 0.3
        lOper.Tables("ADSNUT-FRAC").Parms(6) = 0.3
      End If
    Next vOper
    
    'add masslinks
    i = 1
    grp(i) = "PWATER"
    mem(i) = "PERO"
    mult(i) = 0.0833333
    tmem(i) = "IVOL"
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "POQUAL"
    mem1(i) = 1
    mult(i) = 1
    tmem(i) = "IDNUT"
    tmem1(i) = 2
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "POQUAL"
    mem1(i) = 2
    mult(i) = 1
    tmem(i) = "IDNUT"
    tmem1(i) = 1
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "POQUAL"
    mem1(i) = 3
    mult(i) = 1
    tmem(i) = "IDNUT"
    tmem1(i) = 4
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "POQUAL"
    mem1(i) = 4
    mult(i) = 0.4
    tmem(i) = "IOX"
    tmem1(i) = 2
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "POQUAL"
    mem1(i) = 4
    mult(i) = 0.048
    tmem(i) = "IPLK"
    tmem1(i) = 3
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "POQUAL"
    mem1(i) = 4
    mult(i) = 0.0023
    tmem(i) = "IPLK"
    tmem1(i) = 4
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "POQUAL"
    mem1(i) = 5
    mult(i) = 1
    tmem(i) = "IDQAL"
    tmem1(i) = 1
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "SOQS"
    mem1(i) = 2
    mult(i) = 0.05
    tmem(i) = "ISQAL"
    tmem1(i) = 1
    tmem2(i) = 2
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "SOQS"
    mem1(i) = 2
    mult(i) = 0.55
    tmem(i) = "ISQAL"
    tmem1(i) = 2
    tmem2(i) = 2
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "SOQS"
    mem1(i) = 2
    mult(i) = 0.4
    tmem(i) = "ISQAL"
    tmem1(i) = 3
    tmem2(i) = 2
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "IOQUAL"
    mem1(i) = 6
    mult(i) = 1
    tmem(i) = "IDQAL"
    tmem1(i) = 2
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "AOQUAL"
    mem1(i) = 6
    mult(i) = 1
    tmem(i) = "IDQAL"
    tmem1(i) = 2
    
    i = i + 1
    grp(i) = "SEDMNT"
    mem(i) = "SOSED"
    mem1(i) = 1
    mult(i) = 0.05
    tmem(i) = "ISED"
    tmem1(i) = 1
    
    i = i + 1
    grp(i) = "SEDMNT"
    mem(i) = "SOSED"
    mem1(i) = 1
    mult(i) = 0.55
    tmem(i) = "ISED"
    tmem1(i) = 2
    
    i = i + 1
    grp(i) = "SEDMNT"
    mem(i) = "SOSED"
    mem1(i) = 1
    mult(i) = 0.4
    tmem(i) = "ISED"
    tmem1(i) = 3
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "SOQS"
    mem1(i) = 3
    mult(i) = 0.05
    tmem(i) = "ISQAL"
    tmem1(i) = 1
    tmem2(i) = 3
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "SOQS"
    mem1(i) = 3
    mult(i) = 0.55
    tmem(i) = "ISQAL"
    tmem1(i) = 2
    tmem2(i) = 3
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "SOQS"
    mem1(i) = 3
    mult(i) = 0.4
    tmem(i) = "ISQAL"
    tmem1(i) = 3
    tmem2(i) = 3
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "IOQUAL"
    mem1(i) = 7
    mult(i) = 1
    tmem(i) = "IDQAL"
    tmem1(i) = 3
    
    i = i + 1
    grp(i) = "PQUAL"
    mem(i) = "AOQUAL"
    mem1(i) = 7
    mult(i) = 1
    tmem(i) = "IDQAL"
    tmem1(i) = 3
    
    i = i + 1
    grp(i) = "PWTGAS"
    mem(i) = "PODOXM"
    mult(i) = 1
    tmem(i) = "IOX"
    tmem1(i) = 1
    
    i = i + 1
    grp(i) = "PWTGAS"
    mem(i) = "POHT"
    mult(i) = 1
    tmem(i) = "IHEAT"

    For i = 1 To 23
      Set lMassLink = New HspfMassLink
      Set lMassLink.Uci = myUci
      lMassLink.MassLinkID = 5
      lMassLink.Source.volname = "PERLND"
      lMassLink.Source.group = grp(i)
      lMassLink.Source.member = mem(i)
      lMassLink.Source.memsub1 = mem1(i)
      lMassLink.mfact = mult(i)
      lMassLink.Target.volname = "BMPRAC"
      lMassLink.Target.group = "INFLOW"
      lMassLink.Target.member = tmem(i)
      lMassLink.Target.memsub1 = tmem1(i)
      lMassLink.Target.memsub2 = tmem2(i)
      lMassLink.Comment = ""
      myUci.MassLinks.Add lMassLink
    Next i
  
    i = 1
    grp(i) = "IWATER"
    mem(i) = "SURO"
    mult(i) = 0.0833333
    tmem(i) = "IVOL"
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQUAL"
    mem1(i) = 1
    mult(i) = 1
    tmem(i) = "IDNUT"
    tmem1(i) = 2
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQUAL"
    mem1(i) = 2
    mult(i) = 1
    tmem(i) = "IDNUT"
    tmem1(i) = 1
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQUAL"
    mem1(i) = 3
    mult(i) = 1
    tmem(i) = "IDNUT"
    tmem1(i) = 4
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQUAL"
    mem1(i) = 4
    mult(i) = 0.4
    tmem(i) = "IOX"
    tmem1(i) = 2
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQUAL"
    mem1(i) = 4
    mult(i) = 0.048
    tmem(i) = "IPLK"
    tmem1(i) = 3
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQUAL"
    mem1(i) = 4
    mult(i) = 0.0023
    tmem(i) = "IPLK"
    tmem1(i) = 4
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQUAL"
    mem1(i) = 5
    mult(i) = 1
    tmem(i) = "IDQAL"
    tmem1(i) = 1
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQS"
    mem1(i) = 2
    mult(i) = 0.05
    tmem(i) = "ISQAL"
    tmem1(i) = 1
    tmem2(i) = 2
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQS"
    mem1(i) = 2
    mult(i) = 0.55
    tmem(i) = "ISQAL"
    tmem1(i) = 2
    tmem2(i) = 2
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQS"
    mem1(i) = 2
    mult(i) = 0.4
    tmem(i) = "ISQAL"
    tmem1(i) = 3
    tmem2(i) = 2
    
    i = i + 1
    grp(i) = "SOLIDS"
    mem(i) = "SOSLD"
    mem1(i) = 0
    mult(i) = 0.05
    tmem(i) = "ISED"
    tmem1(i) = 1
    
    i = i + 1
    grp(i) = "SOLIDS"
    mem(i) = "SOSLD"
    mem1(i) = 0
    mult(i) = 0.55
    tmem(i) = "ISED"
    tmem1(i) = 2
    
    i = i + 1
    grp(i) = "SOLIDS"
    mem(i) = "SOSLD"
    mem1(i) = 0
    mult(i) = 0.4
    tmem(i) = "ISED"
    tmem1(i) = 3
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQS"
    mem1(i) = 3
    mult(i) = 0.05
    tmem(i) = "ISQAL"
    tmem1(i) = 1
    tmem2(i) = 3
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQS"
    mem1(i) = 3
    mult(i) = 0.55
    tmem(i) = "ISQAL"
    tmem1(i) = 2
    tmem2(i) = 3
    
    i = i + 1
    grp(i) = "IQUAL"
    mem(i) = "SOQS"
    mem1(i) = 3
    mult(i) = 0.4
    tmem(i) = "ISQAL"
    tmem1(i) = 3
    tmem2(i) = 3
    
    i = i + 1
    grp(i) = "IWTGAS"
    mem(i) = "SODOXM"
    mem1(i) = 0
    mult(i) = 1
    tmem(i) = "IOX"
    tmem1(i) = 1
    tmem2(i) = 0
    
    i = i + 1
    grp(i) = "IWTGAS"
    mem(i) = "SOHT"
    mem1(i) = 0
    mult(i) = 1
    tmem(i) = "IHEAT"
    tmem1(i) = 0
    tmem2(i) = 0
    
    For i = 1 To 19
      Set lMassLink = New HspfMassLink
      Set lMassLink.Uci = myUci
      lMassLink.MassLinkID = 4
      lMassLink.Source.volname = "IMPLND"
      lMassLink.Source.group = grp(i)
      lMassLink.Source.member = mem(i)
      lMassLink.Source.memsub1 = mem1(i)
      lMassLink.mfact = mult(i)
      lMassLink.Target.volname = "BMPRAC"
      lMassLink.Target.group = "INFLOW"
      lMassLink.Target.member = tmem(i)
      lMassLink.Target.memsub1 = tmem1(i)
      lMassLink.Target.memsub2 = tmem2(i)
      lMassLink.Comment = ""
      myUci.MassLinks.Add lMassLink
    Next i
    
    Set lMassLink = New HspfMassLink
    Set lMassLink.Uci = myUci
    lMassLink.MassLinkID = 6
    lMassLink.Source.volname = "BMPRAC"
    lMassLink.Source.group = "ROFLOW"
    lMassLink.Target.volname = "RCHRES"
    lMassLink.Target.group = "INFLOW"
    lMassLink.Comment = ""
    myUci.MassLinks.Add lMassLink

  End If
End Sub


Private Sub MakeMods_AddBMPAreas(myUci As HspfUci)
  Dim s As String, ID&, baseid&
  Dim lOper As HspfOperation, vOper As Variant
  Dim lOpnBlk As HspfOpnBlk, lConn As HspfConnection
  Dim ltable As HspfTable
  Dim rOpn As HspfOperation
  Dim bmpid&, i&, mem$(23), grp$(23), mem1&(23), mult(23) As Double

  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set rOpn = vOpn
    'find area records for this reach
    desc = Mid(rOpn.Description, 8)
    Row = 0
    For i = 1 To 266
      If huc12ext(i) = desc Then
        Row = i
        Exit For
      End If
    Next i
    'figure out which p/i series goes to this reach
    For Each vConn In rOpn.Sources
      Set lConn = vConn
      If lConn.Source.volname = "PERLND" Then
        ID = lConn.Source.VolId
        If Len(Trim(str(ID))) = 3 Then
          baseid = (Int(ID / 100)) * 100
        Else
          baseid = (Int(ID / 10)) * 10
        End If
      End If
    Next vConn
    'remove old connections from p/i to this rchres
    i = 1
    Do While i <= myUci.Connections.Count
      Set lConn = myUci.Connections(i)
      If lConn.Target.volname = "RCHRES" And lConn.Target.VolId = rOpn.ID Then
        If lConn.Source.volname = "PERLND" Or lConn.Source.volname = "IMPLND" Then
          'remove this connection
          myUci.Connections.Remove i
        Else
          i = i + 1
        End If
      Else
        i = i + 1
      End If
    Loop
    i = 1
    Do While i <= rOpn.Sources.Count
      Set lConn = rOpn.Sources(i)
      If lConn.Source.volname = "PERLND" Or lConn.Source.volname = "IMPLND" Then
        'remove this connection
        rOpn.Sources.Remove i
      Else
        i = i + 1
      End If
    Loop
    ctmp = Right(CStr(rOpn.ID), 2)
    bmpid = Int(ctmp)
    
    'new schematic connections
    For i = 1 To 5
      For j = 1 To 6
        Set lConn = New HspfConnection
        Set lConn.Uci = myUci
        lConn.typ = 3
        lConn.mfact = BmpArea(Row, ((i - 1) * 6) + j)
        If i = 1 Then
          lConn.Source.VolId = baseid + 1
          lConn.Comment = "*** commercial"
        ElseIf i = 2 Then
          lConn.Source.VolId = baseid + 2
          lConn.Comment = "*** industrial"
        ElseIf i = 3 Then
          lConn.Source.VolId = baseid + 5
          lConn.Comment = "*** low res"
        ElseIf i = 4 Then
          lConn.Source.VolId = baseid + 6
          lConn.Comment = "*** med res"
        ElseIf i = 5 Then
          lConn.Source.VolId = baseid + 7
          lConn.Comment = "*** high res"
        End If
        If j = 1 Then
          lConn.Source.volname = "PERLND"
          lConn.Target.volname = "RCHRES"
          lConn.Target.VolId = rOpn.ID
          lConn.MassLink = 2
        ElseIf j = 2 Then
          lConn.Source.volname = "IMPLND"
          lConn.Target.volname = "RCHRES"
          lConn.Target.VolId = rOpn.ID
          lConn.MassLink = 1
          lConn.Comment = ""
        ElseIf j = 3 Then
          lConn.Source.volname = "PERLND"
          lConn.Target.volname = "BMPRAC"
          lConn.Target.VolId = 100 + bmpid
          lConn.MassLink = 5
          lConn.Comment = ""
        ElseIf j = 4 Then
          lConn.Source.volname = "IMPLND"
          lConn.Target.volname = "BMPRAC"
          lConn.Target.VolId = 100 + bmpid
          lConn.MassLink = 4
          lConn.Comment = ""
        ElseIf j = 5 Then
          lConn.Source.volname = "PERLND"
          lConn.Target.volname = "BMPRAC"
          lConn.Target.VolId = 200 + bmpid
          lConn.MassLink = 5
          lConn.Comment = ""
        ElseIf j = 6 Then
          lConn.Source.volname = "IMPLND"
          lConn.Target.volname = "BMPRAC"
          lConn.Target.VolId = 200 + bmpid
          lConn.MassLink = 4
          lConn.Comment = ""
        End If
        If lConn.mfact > 0 Then
          myUci.Connections.Add lConn
          Set lConn.Target.Opn = myUci.OpnBlks(lConn.Target.volname).operfromid(lConn.Target.VolId)
          Set lConn.Source.Opn = myUci.OpnBlks(lConn.Source.volname).operfromid(lConn.Source.VolId)
          myUci.OpnBlks(lConn.Source.volname).operfromid(lConn.Source.VolId).targets.Add lConn
          myUci.OpnBlks(lConn.Target.volname).operfromid(lConn.Target.VolId).Sources.Add lConn
        End If
      Next j
    Next i
    For i = 1 To 10
      Set lConn = New HspfConnection
      Set lConn.Uci = myUci
      lConn.typ = 3
      lConn.mfact = BmpArea(Row, 30 + i)
      If lConn.mfact > 0 Then
        lConn.Target.volname = "RCHRES"
        lConn.Target.VolId = rOpn.ID
        If i = 1 Then
          lConn.Source.volname = "PERLND"
          lConn.Source.VolId = baseid + 3
          lConn.Comment = "*** transportation"
          lConn.MassLink = 2
        ElseIf i = 2 Then
          lConn.Source.volname = "IMPLND"
          lConn.Source.VolId = baseid + 3
          lConn.MassLink = 1
          lConn.Comment = ""
        ElseIf i = 3 Then
          lConn.Source.volname = "PERLND"
          lConn.Source.VolId = baseid + 8
          lConn.Comment = "*** agric"
          lConn.MassLink = 2
        ElseIf i = 4 Then
          lConn.Source.volname = "IMPLND"
          lConn.Source.VolId = baseid + 8
          lConn.MassLink = 1
          lConn.Comment = ""
        ElseIf i = 5 Then
          lConn.Source.volname = "PERLND"
          lConn.Source.VolId = baseid + 4
          lConn.Comment = "*** extractive"
          lConn.MassLink = 2
        ElseIf i = 6 Then
          lConn.Source.volname = "IMPLND"
          lConn.Source.VolId = baseid + 4
          lConn.MassLink = 1
          lConn.Comment = ""
        ElseIf i = 7 Then
          lConn.Source.volname = "PERLND"
          lConn.Source.VolId = baseid + 9
          lConn.Comment = "*** forest"
          lConn.MassLink = 2
        ElseIf i = 8 Then
          lConn.Source.volname = "IMPLND"
          lConn.Source.VolId = baseid + 9
          lConn.MassLink = 1
          lConn.Comment = ""
        ElseIf i = 9 Then
          lConn.Source.volname = "PERLND"
          lConn.Source.VolId = baseid + 10
          lConn.Comment = "*** wetlands"
          lConn.MassLink = 2
        ElseIf i = 10 Then
          lConn.Source.volname = "IMPLND"
          lConn.Source.VolId = baseid + 10
          lConn.MassLink = 1
          lConn.Comment = ""
        End If
        myUci.Connections.Add lConn
        Set lConn.Target.Opn = rOpn
        Set lConn.Source.Opn = myUci.OpnBlks(lConn.Source.volname).operfromid(lConn.Source.VolId)
        myUci.OpnBlks(lConn.Source.volname).operfromid(lConn.Source.VolId).targets.Add lConn
        rOpn.Sources.Add lConn
      End If
    Next i
'    *** bmp output
    Set lConn = New HspfConnection
    Set lConn.Uci = myUci
    lConn.typ = 3
    lConn.mfact = 1
    lConn.Source.volname = "BMPRAC"
    lConn.Source.VolId = 100 + bmpid
    lConn.Comment = "*** bmp output"
    lConn.MassLink = 6
    lConn.Target.volname = "RCHRES"
    lConn.Target.VolId = rOpn.ID
    myUci.Connections.Add lConn
    Set lConn.Target.Opn = rOpn
    Set lConn.Source.Opn = myUci.OpnBlks("BMPRAC").operfromid(lConn.Source.VolId)
    myUci.OpnBlks("BMPRAC").operfromid(lConn.Source.VolId).targets.Add lConn
    rOpn.Sources.Add lConn
    Set lConn = New HspfConnection
    Set lConn.Uci = myUci
    lConn.typ = 3
    lConn.mfact = 1
    lConn.Source.volname = "BMPRAC"
    lConn.Source.VolId = 200 + bmpid
    lConn.MassLink = 6
    lConn.Target.volname = "RCHRES"
    lConn.Target.VolId = rOpn.ID
    myUci.Connections.Add lConn
    Set lConn.Target.Opn = rOpn
    Set lConn.Source.Opn = myUci.OpnBlks("BMPRAC").operfromid(lConn.Source.VolId)
    myUci.OpnBlks("BMPRAC").operfromid(lConn.Source.VolId).targets.Add lConn
    rOpn.Sources.Add lConn
  Next vOpn

End Sub

Private Sub MakeMods_RemovePointSources()
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  Dim vPt As Variant, lPt As HspfPoint
  
  i = 1
  Do While i <= myUci.PointSources.Count
    Set lPt = myUci.PointSources(i)
    If lPt.Target.volname = "RCHRES" And lPt.Source.volname = "WDM2" Then
      'remove it
      myUci.PointSources.Remove i
    Else
      i = i + 1
    End If
  Loop
End Sub

Private Sub MakeMods_RemoveWaterIntakes()
  Dim lOpn As HspfOperation, vConn As Variant
  Dim lConn As HspfConnection, r&, i&
  
  i = 1
  Do While i <= myUci.Connections.Count
    Set lConn = myUci.Connections(i)
    If lConn.Source.VolId > 9300 And lConn.Source.VolId < 9500 And lConn.Target.member = "OUTDGT" Then
      'remove it
      j = 1
      Do While j <= lConn.Target.Opn.Sources.Count
        If lConn.Target.Opn.Sources(j).Source.VolId = lConn.Source.VolId Then
          lConn.Target.Opn.Sources.Remove j
        Else
          j = j + 1
        End If
      Loop
      myUci.Connections.Remove i
    Else
      i = i + 1
    End If
  Loop
End Sub

Private Sub MakeMods_AddFuturePointSources()
  Dim lOpn As HspfOperation
  Dim lConn As HspfConnection, r&, i&
  Dim vPt As Variant, lPt As HspfPoint
  Dim smember$(15), tmember$(15), mult(15) As Double, tran$(15), tsub1&(15)
           
  smember(1) = "FLOW"
  smember(2) = "FLOW"
  smember(3) = "PHOSTL"
  smember(4) = "TSSL"
  smember(5) = "FECOLL"
  smember(6) = "BODL"
  smember(7) = "TN"
  smember(8) = "NH3L"
  smember(9) = "NO3L"
  smember(10) = "DO2L"

  mult(1) = 1
  mult(2) = 0.0825
  mult(3) = 1
  mult(4) = 0.00025
  mult(5) = 1
  mult(6) = 2.47
  mult(7) = 1
  mult(8) = 1
  mult(9) = 1
  mult(10) = 1
  
  tran(1) = "SAME"
  tran(2) = "SAME"
  tran(3) = "DIV"
  tran(4) = "DIV"
  tran(5) = "DIV"
  tran(6) = "DIV"
  tran(7) = "DIV"
  tran(8) = "DIV"
  tran(9) = "DIV"
  tran(10) = "DIV"
  
  tmember(1) = "OUTDGT"
  tmember(2) = "IVOL"
  tmember(3) = "NUIF1"
  tmember(4) = "ISED"
  tmember(5) = "IDQAL"
  tmember(6) = "OXIF"
  tmember(7) = ""
  tmember(8) = "NUIF1"
  tmember(9) = "NUIF1"
  tmember(10) = "OXIF"
  
  tsub1(1) = 2
  tsub1(2) = 1
  tsub1(3) = 4
  tsub1(4) = 2
  tsub1(5) = 1
  tsub1(6) = 2
  tsub1(7) = 1
  tsub1(8) = 2
  tsub1(9) = 1
  tsub1(10) = 1
  
  For Each vOpn In myUci.OpnBlks("RCHRES").Ids
    Set lOpn = vOpn
    lOpn.Tables("GEN-INFO").Parms(2).Value = 1
    lOpn.Tables("HYDR-PARM1").Parms(11).Value = 0
    For j = 1 To 10
      For i = 6 To 100
        If reachid(i) = Mid(lOpn.Description, 8) Then
          found = False
          For Each vPt In myUci.PointSources
            Set lPt = vPt
            If lPt.Source.VolId = ptdsn2(j, i) Then
              found = True
              Exit For
            End If
          Next vPt
          If Not found Then
            If Len(Trim(ptval(j, i))) <> 0 And ptval(j, i) <> 0 And Len(smember(j)) > 0 Then
              'create new ext src
              Set lConn = New HspfConnection
              Set lConn.Uci = myUci
              lConn.typ = 1
              lConn.Source.volname = "WDM2"
              lConn.Source.VolId = ptdsn2(j, i)
              lConn.Source.member = smember(j)
              lConn.Ssystem = "ENGL"
              lConn.Sgapstrg = "ZERO"
              lConn.mfact = mult(j)
              If tran(j) = "DIV" And ptdsn2(j, i) > 3999 And ptdsn2(j, i) < 5000 Then
                tran(j) = "SAME"
              Else
                lConn.tran = tran(j)
              End If
              lConn.Target.volname = lOpn.Name
              lConn.Target.VolId = lOpn.ID
              If j = 1 Then
                lConn.Target.group = "EXTNL"
                lOpn.Tables("GEN-INFO").Parms(2).Value = 2
                lOpn.Tables("HYDR-PARM1").Parms(11).Value = 2
              Else
                lConn.Target.group = "INFLOW"
              End If
              lConn.Target.member = tmember(j)
              lConn.Target.memsub1 = tsub1(j)
            
              myUci.Connections.Add lConn
              Set lConn.Target.Opn = lOpn
              lOpn.Sources.Add lConn
                           
              If j = 4 Then
                'do twice for sed
                Set lConn = New HspfConnection
                Set lConn.Uci = myUci
                lConn.typ = 1
                lConn.Source.volname = "WDM2"
                lConn.Source.VolId = ptdsn2(j, i)
                lConn.Source.member = smember(j)
                lConn.Ssystem = "ENGL"
                lConn.Sgapstrg = "ZERO"
                lConn.mfact = mult(j)
                If tran(j) = "DIV" And ptdsn2(j, i) > 3999 And ptdsn2(j, i) < 5000 Then
                  tran(j) = "SAME"
                Else
                  lConn.tran = tran(j)
                End If
                lConn.Target.volname = lOpn.Name
                lConn.Target.VolId = lOpn.ID
                lConn.Target.group = "INFLOW"
                lConn.Target.member = tmember(j)
                lConn.Target.memsub1 = tsub1(j) + 1
              
                myUci.Connections.Add lConn
                Set lConn.Target.Opn = lOpn
                lOpn.Sources.Add lConn
              End If
              
            End If
          End If
        End If
      Next i
    Next j
  Next vOpn
End Sub

Private Sub BuildNewPointDSNs()
  Dim j&, ndsn&, nfrec&, cscen$, wdmsfl&, retcod&, psa&, i&
  Dim GenTs As ATCclsTserData, addeddsn As Boolean, SDate&(6), EDate&(6)
  Dim wdmid&, ccons$, nsteps&, aval() As Single
  Dim TsDate As ATCclsTserDate, curdate!, iVal&
  Dim myDateSummary As ATTimSerDateSummary
  Dim lTs As Collection, conname$(10)
    
  conname(1) = "FLOW"
  conname(2) = "FLOW"
  conname(3) = "PHOSTL"
  conname(4) = "TSSL"
  conname(5) = "FECOLL"
  conname(6) = "BODL"
  conname(7) = "TN"
  conname(8) = "NH3L"
  conname(9) = "NO3L"
  conname(10) = "DO2L"
  
  For j = 6 To 100
    For k = 1 To 10
      If ptval(k, j) <> 0 Then
        'add this dsn
        ndsn = ptdsn2(k, j)
        newwdmid = "WDM2"
        Set GenTs = New ATCclsTserData
        With GenTs.Header
          .ID = ndsn
          .sen = "PT-OBS"
          .Con = conname(k)
          .Loc = CStr(ndsn)
          .desc = fac(j)
        End With
        Set TsDate = New ATCclsTserDate
        With myDateSummary
          .CIntvl = True
          .ts = 1
          .Tu = 5 'monthly
          .Intvl = 1
        End With
        'For i = 0 To 5
          SDate(0) = 1990
          SDate(1) = 1
          SDate(2) = 1
          SDate(3) = 0
          SDate(4) = 0
          SDate(5) = 0
          EDate(0) = 1999
          EDate(1) = 12
          EDate(2) = 31
          EDate(3) = 24
          EDate(4) = 0
          EDate(5) = 0
        'Next i
        myDateSummary.SJDay = Date2J(SDate)
        myDateSummary.EJDay = Date2J(EDate)
        
        nsteps = 120
        ReDim aval(nsteps)
        If k = 1 Or k = 2 Then
          'convert from mgd to cfs    1 cfs = 0.646 mgd
          For i = 0 To nsteps
            aval(i) = ptval(k, j) / 0.646
          Next i
        Else
          'load in pounds per month
          For i = 0 To nsteps  'use this value for all
            aval(i) = ptval(k, j)
          Next i
        End If
        myDateSummary.NVALS = nsteps
        Let TsDate.Summary = myDateSummary
        
        Set GenTs.Dates = TsDate
        GenTs.Values = aval
        GenTs.AttribSet "TSTYPE", conname(k)
        addeddsn = myUci.getwdmobj(2).AddTimSer(GenTs, 0)
      End If
    Next k
  Next j
    
End Sub
