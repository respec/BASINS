VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.10#0"; "ATCoCtl.ocx"
Begin VB.Form frmTIGER 
   AutoRedraw      =   -1  'True
   Caption         =   "Import TIGER"
   ClientHeight    =   6480
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   7365
   LinkTopic       =   "Form1"
   ScaleHeight     =   6480
   ScaleWidth      =   7365
   StartUpPosition =   3  'Windows Default
   Begin VB.OptionButton optMode 
      Caption         =   "Add 2000 Housing"
      Height          =   255
      Index           =   8
      Left            =   240
      TabIndex        =   13
      Top             =   1560
      Width           =   2655
   End
   Begin VB.OptionButton optMode 
      Caption         =   "Add 303d fields"
      Height          =   255
      Index           =   7
      Left            =   3240
      TabIndex        =   12
      Top             =   1320
      Width           =   1575
   End
   Begin VB.OptionButton optMode 
      Caption         =   "Dump as CSV"
      Height          =   255
      Index           =   6
      Left            =   3240
      TabIndex        =   11
      Top             =   960
      Width           =   1575
   End
   Begin VB.OptionButton optMode 
      Caption         =   "1990 Pop, etc."
      Height          =   255
      Index           =   5
      Left            =   3240
      TabIndex        =   10
      Top             =   600
      Width           =   1575
   End
   Begin VB.OptionButton optMode 
      Caption         =   "Find Field Sizes"
      Height          =   255
      Index           =   4
      Left            =   3240
      TabIndex        =   8
      Top             =   240
      Width           =   1575
   End
   Begin VB.OptionButton optMode 
      Caption         =   "Get 2000 Population"
      Height          =   255
      Index           =   3
      Left            =   240
      TabIndex        =   7
      Top             =   1320
      Width           =   2655
   End
   Begin VB.OptionButton optMode 
      Caption         =   "Split Shapes by HUC"
      Height          =   255
      Index           =   2
      Left            =   240
      TabIndex        =   6
      Top             =   960
      Width           =   2655
   End
   Begin VB.OptionButton optMode 
      Caption         =   "Split TIGER shapes by CFCC"
      Height          =   255
      Index           =   1
      Left            =   240
      TabIndex        =   5
      Top             =   600
      Width           =   2655
   End
   Begin VB.OptionButton optMode 
      Caption         =   "TIGER lines to shape"
      Height          =   255
      Index           =   0
      Left            =   240
      TabIndex        =   4
      Top             =   240
      Value           =   -1  'True
      Width           =   2175
   End
   Begin ATCoCtl.ATCoSelectList aslSourceFiles 
      CausesValidation=   0   'False
      Height          =   3375
      Left            =   120
      TabIndex        =   3
      Top             =   2280
      Width           =   7215
      _ExtentX        =   12726
      _ExtentY        =   5953
      RightLabel      =   "Selected:"
      LeftLabel       =   "Available:"
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   3480
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.TextBox txtSourceFolder 
      Height          =   285
      Left            =   2040
      TabIndex        =   2
      Top             =   1920
      Width           =   5295
   End
   Begin VB.CommandButton cmdConvert 
      Caption         =   "Convert"
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   5880
      Width           =   1575
   End
   Begin VB.CommandButton cmdOpenFolder 
      Caption         =   "Set folder"
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   1920
      Width           =   1815
   End
   Begin VB.Label lblStatus 
      Height          =   375
      Left            =   1800
      TabIndex        =   9
      Top             =   6000
      Width           =   5535
   End
End
Attribute VB_Name = "frmTIGER"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Const UNZIPEXE As String = "C:\Program Files\WinZip\WZUNZIP.EXE"
Private Const ZIPEXE As String = "C:\Program Files\WinZip\WZZIP.EXE"
Private Const ALLCFCC As String = "ABCDEFHPX"

Private Const numNew90Fields As Long = 11
Private Const numNew00Fields As Long = 3

Private pMode As Long
Private pMaxLength() As Long
Private pNumFields As Long
Private pCurTable As clsATCTable

Private dbfHUC As clsATCTable
Private shpHUC As CShape_IO

Private dbfMisc As clsATCTable
Private shpMisc As CShape_IO

Private csvImpairments As clsATCTable
Private csvTMDLs As clsATCTable

Dim pLastMsg As String

Private Sub cmdConvert_Click()
  Dim SourceFilename As String
  Dim SourcePath As String
  Dim DestPath As String
  Dim iList As Long
  Dim starttime As Double
  Dim elapsed As Double
  Dim FIPSstr As String
  
  Me.MousePointer = vbHourglass
  starttime = Time
  
  If pMode = 2 Then
    Set dbfHUC = New clsATCTableDBF
    Set shpHUC = New CShape_IO
    dbfHUC.OpenFile "H:\Data\Census\huc250llb.dbf"
    shpHUC.ShapeFileOpen "H:\Data\Census\huc250llb.shp", 0
  End If
  Select Case pMode
    Case 4
      pNumFields = 0
    Case Else
      cdlg.Filter = "*.shp|*.shp|All Files (*.*)|*.*"
      cdlg.FilterIndex = 1
      cdlg.DefaultExt = "shp"
      cdlg.DialogTitle = "Save shape files in"
      cdlg.ShowSave
      DestPath = PathNameOnly(cdlg.Filename) & "\"
  End Select
  SourcePath = txtSourceFolder
  If Right(SourcePath, 1) <> "\" Then SourcePath = SourcePath & "\"
  
  For iList = 1 To aslSourceFiles.RightCount
    SourceFilename = SourcePath & aslSourceFiles.RightItem(iList - 1)
    lblStatus.Caption = SourceFilename
    
    elapsed = Time - starttime
    AppendFileString SourcePath & "\log.txt", _
      Time & " #" & iList & " of " & aslSourceFiles.RightCount & " " _
           & Format(iList / aslSourceFiles.RightCount * 100, "0.0") & "% in " _
           & Format(elapsed * 24, "0.00") & " (+" _
           & Format(24 * (elapsed * aslSourceFiles.RightCount / iList - elapsed), "0.00") _
           & ") hours" & vbCrLf
    
    DoEvents
    Select Case pMode
      Case 0: ConvertToShape SourceFilename, DestPath
      Case 1: SplitShapesCFCC SourceFilename, DestPath
      Case 2:
        FIPSstr = Right(FilenameOnly(SourceFilename), 5)
        If Not IsNumeric(FIPSstr) Then FIPSstr = Mid(FilenameOnly(SourceFilename), 3, 2)
        If Not IsNumeric(FIPSstr) Then
          SplitByHUC 0, SourceFilename, DestPath
          'MsgBox "Could not find FIPS in " & FilenameOnly(SourceFilename)
        Else
          SplitByHUC CLng(FIPSstr), SourceFilename, DestPath
        End If
      Case 3: Add2000population SourceFilename, DestPath
      Case 4: FindFieldSizes SourceFilename
      Case 5: Add1990popPlus SourceFilename, DestPath
      Case 6: DumpCSV SourceFilename, DestPath
      Case 7: Add303d SourceFilename, DestPath
      Case 8: Add2000housing SourceFilename, DestPath
    End Select
  Next
  
  If pMode = 4 Then
    Debug.Print pCurTable.CreationCode & vbCrLf
    For iList = 1 To pNumFields
      Debug.Print iList & ": " & pMaxLength(iList) & " / " & pCurTable.FieldLength(iList) & " " & pCurTable.FieldName(iList)
    Next
    pNumFields = 0
    Set pCurTable = Nothing
  End If
  
  If Not shpHUC Is Nothing Then
    shpHUC.FileShutDown
    Set shpHUC = Nothing
  End If
  
  If Not dbfHUC Is Nothing Then
    dbfHUC.Clear
    Set dbfHUC = Nothing
  End If
  
  'If Not csvImpairments Is Nothing Then
  '  csvImpairments.Clear
  '  Set csvImpairments = Nothing
  'End If

  'If Not csvTMDLs Is Nothing Then
  '  csvTMDLs.Clear
  '  Set csvTMDLs = Nothing
  'End If
  
  If Not shpMisc Is Nothing Then
    shpMisc.FileShutDown
    Set shpMisc = Nothing
  End If
  
  If Not dbfMisc Is Nothing Then
    dbfMisc.WriteFile DestPath & "misc.dbf"
    dbfMisc.Clear
    Set dbfMisc = Nothing
  End If
  
  lblStatus.Caption = ""
  Me.MousePointer = vbDefault
End Sub

Private Sub RewriteDBF(SourceFilename As String, DestPath As String)
  Dim iRecord As Long
  Dim newTable As clsATCTable
  Set newTable = New clsATCTableDBF
  
  With newTable
    Select Case Left(FilenameNoPath(SourceFilename), 2)
    Case "co"

      '1: 2 / 2 STATE
      '2: 3 / 3 COUNTY
      '3: 31 / 90 NAME
      '4: 2 / 2 LSAD
      '5: 7 / 9 Population

      '1: 20 / 20 AREA
      '2: 20 / 20 PERIMETER
      '3: 3 / 11 CO13_D00_
      '4: 3 / 11 CO13_D00_I
      '5: 2 / 2 STATE
      '6: 3 / 3 COUNTY
      '7: 31 / 90 NAME
      '8: 2 / 2 LSAD
      '9: 6 / 50 LSAD_TRANS

      .NumFields = 3
    
      .FieldName(1) = "FIPS"
      .FieldType(1) = "C"
      .FieldLength(1) = 5
      
      .FieldName(2) = "NAME"
      .FieldType(2) = "C"
      .FieldLength(2) = 31
    
      .FieldName(3) = "Population"
      .FieldType(3) = "N"
      .FieldLength(3) = 9
    
      .NumRecords = pCurTable.NumRecords
      
'      select cas pCurTable.FieldName(1) = "AREA" Then
      
      
      For iRecord = 1 To pCurTable.NumRecords
        .CurrentRecord = iRecord
        pCurTable.CurrentRecord = iRecord
        .Value(1) = pCurTable.Value(5) & pCurTable.Value(6)
        .Value(2) = pCurTable.Value(7)
        '.Value(3) = pCurTable.Value(4)
      Next
      .WriteFile pCurTable.Filename
    Case "bg"
      '1: 20 / 20 AREA
      '2: 20 / 20 PERIMETER
      '3: 5 / 11 BG78_D00_
      '4: 5 / 11 BG78_D00_I
      '5: 2 / 2 STATE
      '6: 3 / 3 COUNTY
      '7: 6 / 6 TRACT
      '8: 1 / 1 BLKGROUP
      '9: 1 / 90 NAME
      '10: 2 / 2 LSAD
      '11: 0 / 50 LSAD_TRANS
      Set newTable = New clsATCTableDBF
      With newTable
        .NumFields = 11
            
        .FieldName(5) = "STATE"
        .FieldType(5) = "C"
        .FieldLength(5) = 2
       '.FieldDecimalCount(5) = 0
      
        .FieldName(6) = "COUNTY"
        .FieldType(6) = "C"
        .FieldLength(6) = 3
       '.FieldDecimalCount(6) = 0
      
        .FieldName(7) = "TRACT"
        .FieldType(7) = "C"
        .FieldLength(7) = 6
       '.FieldDecimalCount(7) = 0
      
        .FieldName(8) = "BLKGROUP"
        .FieldType(8) = "C"
        .FieldLength(8) = 1
       '.FieldDecimalCount(8) = 0
      
        .FieldName(9) = "NAME"
        .FieldType(9) = "C"
        .FieldLength(9) = 90
       '.FieldDecimalCount(9) = 0
      
        .FieldName(10) = "LSAD"
        .FieldType(10) = "C"
        .FieldLength(10) = 2
       '.FieldDecimalCount(10) = 0
      
        .FieldName(11) = "LSAD_TRANS"
        .FieldType(11) = "C"
        .FieldLength(11) = 50
       '.FieldDecimalCount(11) = 0
      
        '.NumRecords = 213
        '.InitData
      End With
    Case "tr"
      '1: 20 / 20 AREA
      '2: 20 / 20 PERIMETER
      '3: 4 / 11 TR78_D00_
      '4: 4 / 11 TR78_D00_I
      '5: 2 / 2 STATE
      '6: 3 / 3 COUNTY
      '7: 6 / 6 TRACT
      '8: 7 / 90 NAME
      '9: 2 / 2 LSAD
      '10: 0 / 50 LSAD_TRANS
      Set newTable = New clsATCTableDBF
      With newTable
        .NumFields = 10
      
        .FieldName(5) = "STATE"
        .FieldType(5) = "C"
        .FieldLength(5) = 2
       '.FieldDecimalCount(5) = 0
      
        .FieldName(6) = "COUNTY"
        .FieldType(6) = "C"
        .FieldLength(6) = 3
       '.FieldDecimalCount(6) = 0
      
        .FieldName(7) = "TRACT"
        .FieldType(7) = "C"
        .FieldLength(7) = 6
       '.FieldDecimalCount(7) = 0
      
        .FieldName(8) = "NAME"
        .FieldType(8) = "C"
        .FieldLength(8) = 90
       '.FieldDecimalCount(8) = 0
      
        .FieldName(9) = "LSAD"
        .FieldType(9) = "C"
        .FieldLength(9) = 2
       '.FieldDecimalCount(9) = 0
      
        .FieldName(10) = "LSAD_TRANS"
        .FieldType(10) = "C"
        .FieldLength(10) = 50
       '.FieldDecimalCount(10) = 0
      
        '.NumRecords = 55
        '.InitData
      End With

    Case "pl"
      '1: 20 / 20 AREA
      '2: 20 / 20 PERIMETER
      '3: 4 / 11 PL78_D00_
      '4: 4 / 11 PL78_D00_I
      '5: 2 / 2 STATE
      '6: 4 / 4 PLC
      '7: 5 / 5 PLACEFP
      '8: 35 / 90 NAME
      '9: 2 / 2 LSAD
      '10: 16 / 50 LSAD_TRANS
      Set newTable = New clsATCTableDBF
      With newTable
        .NumFields = 10
      
        .FieldName(5) = "STATE"
        .FieldType(5) = "C"
        .FieldLength(5) = 2
       '.FieldDecimalCount(5) = 0
      
        .FieldName(6) = "PLC"
        .FieldType(6) = "C"
        .FieldLength(6) = 4
       '.FieldDecimalCount(6) = 0
      
        .FieldName(7) = "PLACEFP"
        .FieldType(7) = "C"
        .FieldLength(7) = 5
       '.FieldDecimalCount(7) = 0
      
        .FieldName(8) = "NAME"
        .FieldType(8) = "C"
        .FieldLength(8) = 90
       '.FieldDecimalCount(8) = 0
      
        .FieldName(9) = "LSAD"
        .FieldType(9) = "C"
        .FieldLength(9) = 2
       '.FieldDecimalCount(9) = 0
      
        .FieldName(10) = "LSAD_TRANS"
        .FieldType(10) = "C"
        .FieldLength(10) = 50
       '.FieldDecimalCount(10) = 0
      
        '.NumRecords = 9
        '.InitData
      End With
    Case "zt"
      '1: 20 / 20 AREA
      '2: 20 / 20 PERIMETER
      '3: 4 / 11 ZT72_D00_
      '4: 4 / 11 ZT72_D00_I
      '5: 5 / 5 ZCTA
      '6: 5 / 90 NAME
      '7: 2 / 2 LSAD
      '8: 12 / 50 LSAD_TRANS
      Set newTable = New clsATCTableDBF
      With newTable
        .NumFields = 8
            
        .FieldName(5) = "ZCTA"
        .FieldType(5) = "C"
        .FieldLength(5) = 5
       '.FieldDecimalCount(5) = 0
      
        .FieldName(6) = "NAME"
        .FieldType(6) = "C"
        .FieldLength(6) = 90
       '.FieldDecimalCount(6) = 0
      
        .FieldName(7) = "LSAD"
        .FieldType(7) = "C"
        .FieldLength(7) = 2
       '.FieldDecimalCount(7) = 0
      
        .FieldName(8) = "LSAD_TRANS"
        .FieldType(8) = "C"
        .FieldLength(8) = 50
       '.FieldDecimalCount(8) = 0
      
        '.NumRecords = 162
        '.InitData
      End With
    Case Else
      
    End Select
  End With

End Sub

Private Sub DumpCSV(SourceFilename As String, DestPath As String)
  Dim newCSV As clsATCTable
  Dim iRecord As Long
  Dim iValue As Long
  
  Set pCurTable = New clsATCTableDBF
  Set newCSV = New clsATCTableCSV
  
  pCurTable.OpenFile SourceFilename
  newCSV.CousinOf pCurTable
  newCSV.NumRecords = pCurTable.NumRecords
  For iRecord = 1 To pCurTable.NumRecords
    pCurTable.CurrentRecord = iRecord
    newCSV.CurrentRecord = iRecord
    For iValue = 1 To pCurTable.NumFields
      newCSV.Value(iValue) = pCurTable.Value(iValue)
    Next
  Next
  
  newCSV.WriteFile DestPath & FilenameSetExt(FilenameNoPath(SourceFilename), "csv")
  
End Sub

Private Sub FindFieldSizes(SourceFilename As String)
  Dim iField As Long
  Dim iRecord As Long
  Dim thisLength As Long
  
  Set pCurTable = New clsATCTableDBF
  pCurTable.OpenFile SourceFilename
  
  If pNumFields = 0 Then
    pNumFields = pCurTable.NumFields
    ReDim pMaxLength(pNumFields)
    Debug.Print pNumFields & " fields"
  ElseIf pCurTable.NumFields > pNumFields Then
    Debug.Print "More fields in " & SourceFilename & ": " & pCurTable.NumFields
    pNumFields = pCurTable.NumFields
    ReDim Preserve pMaxLength(pNumFields)
  End If
  
  For iRecord = 1 To pCurTable.NumRecords
    pCurTable.CurrentRecord = iRecord
    For iField = 1 To pCurTable.NumFields
      thisLength = Len(Trim(pCurTable.Value(iField)))
      If thisLength > pMaxLength(iField) Then pMaxLength(iField) = thisLength
    Next
  Next

End Sub

Private Sub ConvertToShape(SourceFilename As String, DestPath As String)
  Dim rt1 As clsATCTable
  Dim rt2 As clsATCTable
  Dim FIPS As String
  Dim ipc As ATCoIPC
  Dim cmdline As String
  Dim SourcePath As String
  Dim RT12Path As String
  Dim Unzipped As Boolean
  
  Set ipc = New ATCoIPC
  
  SourcePath = PathNameOnly(SourceFilename) & "\"
  FIPS = Right(FilenameOnly(SourceFilename), 5)
  
  If FileExt(SourceFilename) = "zip" Then
    cmdline = """" & UNZIPEXE & """ """ & SourceFilename & """ """ & DestPath & """ "
    cmdline = cmdline & "TGR" & FIPS & ".RT1 "
    cmdline = cmdline & "TGR" & FIPS & ".RT2 "
    Debug.Print cmdline
    ipc.StartProcess "Unzip", cmdline, 30, 1000
    RT12Path = DestPath
    Unzipped = True
  Else
    RT12Path = SourcePath
    Unzipped = False
  End If
  
  If Not FileExists(RT12Path & "TGR" & FIPS & ".RT1") Then
    MsgBox "Did not extract TGR" & FIPS & ".RT1"
  End If

  If Not FileExists(RT12Path & "TGR" & FIPS & ".RT2") Then
    MsgBox "Did not extract TGR" & FIPS & ".RT2"
  End If

  Set rt1 = New clsATCTableRT1
  Set rt2 = New clsATCTableRT2
  rt1.OpenFile RT12Path & "TGR" & FIPS & ".RT1"
  rt2.OpenFile RT12Path & "TGR" & FIPS & ".RT2"
    
  WriteShpFromTiger rt1, rt2, DestPath & "TGR" & FIPS & ".shp"
  
  Set rt1 = Nothing
  Set rt2 = Nothing
  If Unzipped Then
    Kill DestPath & "TGR" & FIPS & ".RT1"
    Kill DestPath & "TGR" & FIPS & ".RT2"
    cmdline = """" & ZIPEXE & """ -m """ & DestPath & "TGRSHP" & FIPS & ".zip"" "
    cmdline = cmdline & """" & DestPath & "TGR" & FIPS & ".shp"" "
    cmdline = cmdline & """" & DestPath & "TGR" & FIPS & ".shx"" "
    cmdline = cmdline & """" & DestPath & "TGR" & FIPS & ".dbf"" "
    Debug.Print cmdline
    ipc.StartProcess "Zip", cmdline, 30, 1000
  End If
  
End Sub

Private Function CFCCfilename(SourceFilename As String, iCFCC As Long, ext As String) As String
  CFCCfilename = FilenameNoExt(SourceFilename) & "_TGR_" & Mid(ALLCFCC, iCFCC, 1) & "." & ext
End Function

Private Sub SplitShapesCFCC(SourceFilename As String, DestPath As String)
  Dim dbfIn As clsATCTable
  Dim dbfOut() As clsATCTable
  
  Static starttime As Double
  Dim elapsed As Double
  
  Dim shpIn As CShape_IO
  Dim shpOut() As CShape_IO
  
  Static iList As Long
  Dim iCFCC As Long
  Dim iShape As Long
  Dim iField As Long
  Dim fieldCFCC As Long
  
  Dim baseFilename As String
  
  If starttime = 0 Then starttime = Time
  iList = iList + 1

  baseFilename = DestPath & FilenameNoPath(SourceFilename)
  
  'Debug.Print SourceFilename & " -> " & baseFilename

  Set dbfIn = New clsATCTableDBF
  dbfIn.OpenFile FilenameSetExt(SourceFilename, "dbf")
  fieldCFCC = dbfIn.FieldNumber("CFCC")
  If fieldCFCC < 1 Then
    If MsgBox("No CFCC field found in " & FilenameSetExt(SourceFilename, "dbf"), vbOKCancel, "Split by CFCC") = vbCancel Then Exit Sub
  Else
    Set shpIn = New CShape_IO
    shpIn.ShapeFileOpen FilenameSetExt(SourceFilename, "shp"), READWRITEFLAG.ReadOnly
    
    ReDim dbfOut(Len(ALLCFCC))
    ReDim shpOut(Len(ALLCFCC))
    For iCFCC = 1 To Len(ALLCFCC)
      Set dbfOut(iCFCC) = dbfIn.Cousin
      Set shpOut(iCFCC) = New CShape_IO
      shpOut(iCFCC).CreateNewShape CFCCfilename(baseFilename, iCFCC, "shp"), shpIn.getShapeHeader.ShapeType
    Next

    For iShape = 1 To shpIn.getRecordCount
      dbfIn.CurrentRecord = iShape
      iCFCC = InStr(ALLCFCC, Left(dbfIn.Value(fieldCFCC), 1))
      'If iCFCC = 8 Then iCFCC = 1 'Fold Provisional into Roads
      If iCFCC = 0 Then
        Debug.Print "Unknown CFCC " & dbfIn.Value(fieldCFCC)
      Else
        dbfOut(iCFCC).CurrentRecord = dbfOut(iCFCC).NumRecords + 1
        dbfOut(iCFCC).record = dbfIn.record
        'For iField = 1 To dbfIn.NumFields
        '  dbfOut(iCFCC).Value(iField) = dbfIn.Value(iField)
        'Next
        shpOut(iCFCC).putPoly 0, shpIn.getPoly(iShape)
      End If
      DoEvents
    Next

    For iCFCC = 1 To Len(ALLCFCC)
      shpOut(iCFCC).FileShutDown
      If dbfOut(iCFCC).NumRecords > 0 Then
        dbfOut(iCFCC).WriteFile CFCCfilename(baseFilename, iCFCC, "dbf")
      Else
        'Debug.Print "Did not find any CFCC for " & CFCCfilename(baseFilename, iCFCC, "shp")
        Kill CFCCfilename(baseFilename, iCFCC, "shp")
        Kill CFCCfilename(baseFilename, iCFCC, "shx")
      End If
      dbfOut(iCFCC).Clear
      Set dbfOut(iCFCC) = Nothing
      Set shpOut(iCFCC) = Nothing
    Next
    shpIn.FileShutDown
    Set shpIn = Nothing
  End If
  dbfIn.Clear
  Set dbfIn = Nothing
  elapsed = Time - starttime
'  AppendFileString DestPath & "log.txt", _
'    Time & " #" & iList & " of " & nFilesToDo & " " _
'         & Format(iList / nFilesToDo * 100, "0.0") & "% in " _
'         & Format(elapsed * 24, "0.00") & " (+" _
'         & Format(24 * (elapsed * nFilesToDo / iList - elapsed), "0.00") _
'         & ") hours - " & SourceFilename & vbCrLf
End Sub

Private Function outHUCfilename(HUC As String, SourceFilename As String) As String
  outHUCfilename = HUC & "_" & Left(FilenameOnly(SourceFilename), 2)
End Function

Private Sub SplitByHUC(FIPS As Long, SourceFilename As String, DestPath As String)
  Dim HUCshape() As T_shpPoly
  
  Dim dbfIn As clsATCTable
  Dim dbfOut() As clsATCTable
  
  Dim shpIn As CShape_IO
  Dim shpOut() As CShape_IO
    
  Dim iShape As Long
  Dim curShape As T_shpPoly
  Dim curPoint As T_shpXYPoint
  Dim ShpType As FILETYPEENUM
  
  Dim baseFilename As String
    
  Dim HUCSoverlapping As FastCollection
  Dim nHUCS As Long
  Dim iHUC As Long
  Dim sHUC As String
  
  Dim found As Boolean
  Dim overlaps As Boolean
      
  Set dbfIn = New clsATCTableDBF
  dbfIn.OpenFile FilenameSetExt(SourceFilename, "dbf")
  
  Set shpIn = New CShape_IO
  shpIn.ShapeFileOpen SourceFilename, READWRITEFLAG.ReadOnly
  
  ShpType = shpIn.getShapeHeader.ShapeType
  
  If dbfMisc Is Nothing Then
    baseFilename = DestPath & "misc"
    If FileExists(baseFilename & ".dbf") Then
      Set dbfMisc = New clsATCTableDBF
      dbfMisc.OpenFile baseFilename & ".dbf"
    Else
      Set dbfMisc = dbfIn.Cousin
    End If
  End If
  
  If shpMisc Is Nothing Then
    Set shpMisc = New CShape_IO
    If FileExists(baseFilename & ".shp") Then
      shpMisc.ShapeFileOpen baseFilename & ".shp", 1
    Else
      shpMisc.CreateNewShape baseFilename & ".shp", ShpType
    End If
  End If

  If FIPS = 0 Then
    Set HUCSoverlapping = GetAllHUCS(True)
  Else
    Set HUCSoverlapping = GetHUCS(FIPS, True)
  End If
  nHUCS = HUCSoverlapping.count
  ReDim dbfOut(nHUCS)
  ReDim shpOut(nHUCS)
  ReDim HUCshape(nHUCS)
  For iHUC = 1 To nHUCS
    sHUC = HUCSoverlapping.ItemByIndex(iHUC)
    baseFilename = DestPath & outHUCfilename(sHUC, SourceFilename)
    If FileExists(baseFilename & ".dbf") Then
      Set dbfOut(iHUC) = New clsATCTableDBF
      dbfOut(iHUC).OpenFile baseFilename & ".dbf"
    Else
      Set dbfOut(iHUC) = dbfIn.Cousin
    End If
    
    If nHUCS < 100 Then
      Set shpOut(iHUC) = New CShape_IO
      If FileExists(baseFilename & ".shp") Then
        shpOut(iHUC).ShapeFileOpen baseFilename & ".shp", 1
      Else
        shpOut(iHUC).CreateNewShape baseFilename & ".shp", ShpType
      End If
    End If
    
    If dbfHUC.FindFirst(3, sHUC) Then
      HUCshape(iHUC) = shpHUC.getPoly(dbfHUC.CurrentRecord)
    Else
      MsgBox "Failed to find HUC " & sHUC & " in reference layer"
    End If
  Next

  For iShape = 1 To shpIn.getRecordCount
    dbfIn.CurrentRecord = iShape
    If ShpType = typePoint Then
      curPoint = shpIn.getXYPoint(iShape)
    Else
      curShape = shpIn.getPoly(iShape)
    End If
    found = False
    For iHUC = 1 To nHUCS
      If ShpType = typePoint Then
        overlaps = PointInPolygon(curPoint.thePoint.x, curPoint.thePoint.y, HUCshape(iHUC))
      Else
        overlaps = PolyLineOverlapsPolygon(curShape, HUCshape(iHUC))
      End If
      If overlaps Then
        dbfOut(iHUC).CurrentRecord = dbfOut(iHUC).NumRecords + 1
        dbfOut(iHUC).record = dbfIn.record
        
        If nHUCS >= 100 Then
          sHUC = HUCSoverlapping.ItemByIndex(iHUC)
          baseFilename = DestPath & outHUCfilename(sHUC, SourceFilename)
          Set shpOut(iHUC) = New CShape_IO
          If FileExists(baseFilename & ".shp") Then
            shpOut(iHUC).ShapeFileOpen baseFilename & ".shp", 1
          Else
            shpOut(iHUC).CreateNewShape baseFilename & ".shp", ShpType
          End If
        End If
        
        If ShpType = typePoint Then
          shpOut(iHUC).putXYPoint 0, curPoint
        Else
          shpOut(iHUC).putPoly 0, curShape
        End If
        
        If nHUCS >= 100 Then shpOut(iHUC).FileShutDown
        
        found = True
      End If
    Next
    If Not found Then
      AppendFileString PathNameOnly(baseFilename) & "\log.txt", "Shape did not belong in any HUC: " & iShape & vbCrLf
      'dbfMisc.CurrentRecord = dbfMisc.NumRecords + 1
      'dbfMisc.record = dbfIn.record
      'If ShpType = typePoint Then
      '  shpMisc.putXYPoint 0, curPoint
      'Else
      '  shpMisc.putPoly 0, curShape
      'End If
    End If
  Next

  For iHUC = 1 To nHUCS
    If nHUCS < 100 Then shpOut(iHUC).FileShutDown
    sHUC = HUCSoverlapping.ItemByIndex(iHUC)
    baseFilename = DestPath & outHUCfilename(sHUC, SourceFilename)
    If dbfOut(iHUC).NumRecords > 0 Then
      dbfOut(iHUC).WriteFile baseFilename & ".dbf"
    Else
      AppendFileString PathNameOnly(baseFilename) & "\log.txt", "No shapes in HUC: " & sHUC & vbCrLf
'        Kill CFCCfilename(baseFilename & ".shp")
'        Kill CFCCfilename(baseFilename & ".shx")
    End If
    dbfOut(iHUC).Clear
    Set dbfOut(iHUC) = Nothing
    Set shpOut(iHUC) = Nothing
  Next
  shpIn.FileShutDown
  Set shpIn = Nothing
  dbfIn.Clear
  Set dbfIn = Nothing
  
End Sub

'Add 1990 population, housing units, source of water, sewage to shape DBFs
Private Sub Add1990popPlus(SourceFilename As String, DestPath As String)
  Dim iRecord As Long
  Dim iField As Long
  Dim summ_level As String
  Dim newValues(numNew90Fields) As String
  Dim oldTable(numNew90Fields) As clsATCTable
  Dim oldField(numNew90Fields) As Long
  Dim val As Long
  
  Dim state_fips As String
  Dim county_fips As String
  Dim place_fips As String
  Dim tract As String
  Dim block_group As String
  
  Dim TryToOpenShapeDBFs As Boolean
  Dim dbf301 As clsATCTable
  Dim dbf327 As clsATCTable
  Dim dbf329 As clsATCTable
  
  Dim DBFfilename As String
  Dim coDBF As clsATCTable
  Dim trDBF As clsATCTable
  Dim bgDBF As clsATCTable
  Dim plDBF As clsATCTable
  
  Set dbf301 = New clsATCTableDBF
  Set dbf327 = New clsATCTableDBF
  Set dbf329 = New clsATCTableDBF

  dbf301.OpenFile SourceFilename
  dbf327.OpenFile ReplaceString(SourceFilename, "301", "327")
  dbf329.OpenFile ReplaceString(SourceFilename, "301", "329")

  TryToOpenShapeDBFs = True
  For iRecord = 1 To dbf301.NumRecords
    dbf301.CurrentRecord = iRecord
    dbf327.CurrentRecord = iRecord
    dbf329.CurrentRecord = iRecord
    
    summ_level = dbf301.Value(1)
    state_fips = dbf301.Value(2)
    county_fips = dbf301.Value(3)
    place_fips = dbf301.Value(5)
    tract = dbf301.Value(6)
    block_group = dbf301.Value(7)
    
    If TryToOpenShapeDBFs Then
      GoSub CloseDBFs
      Dim NewFieldNames(numNew90Fields) As String
      Dim NewFieldTypes(numNew90Fields) As String
      Dim NewFieldLengths(numNew90Fields) As Long
      
      For iField = 1 To numNew90Fields
        NewFieldTypes(iField) = "N"
        NewFieldLengths(iField) = 9
      Next
      
      iField = 1
      Set oldTable(iField) = dbf301
      oldField(iField) = oldTable(iField).FieldNumber("P0010001")
      NewFieldNames(iField) = "Population"
      
'      iField = iField + 1
'      oldTable(iField) = dbf301
'      oldField(iField) = oldTable(iField).FieldNumber("P0060001")
'      NewFieldNames(iField) = "InsideUrban"
'
'      iField = iField + 1
'      oldTable(iField) = dbf301
'      oldField(iField) = oldTable(iField).FieldNumber("P0060002")
'      NewFieldNames(iField) = "OutsideUrban"
'
'      iField = iField + 1
'      oldTable(iField) = dbf301
'      oldField(iField) = oldTable(iField).FieldNumber("P0060003")
'      NewFieldNames(iField) = "RuralFarm"
'
'      iField = iField + 1
'      oldTable(iField) = dbf301
'      oldField(iField) = oldTable(iField).FieldNumber("P0060004")
'      NewFieldNames(iField) = "RuralNonfarm"
      
      'dbf327
      iField = iField + 1
      Set oldTable(iField) = dbf327
      oldField(iField) = oldTable(iField).FieldNumber("H0010001")
      NewFieldNames(iField) = "HouseUnits"
      NewFieldTypes(iField) = "N"
      
      iField = iField + 1
      Set oldTable(iField) = dbf327
      oldField(iField) = oldTable(iField).FieldNumber("H0040001")
      NewFieldNames(iField) = "Occupied"
      
      iField = iField + 1
      Set oldTable(iField) = dbf327
      oldField(iField) = oldTable(iField).FieldNumber("H0040002")
      NewFieldNames(iField) = "Vacant"
      
      'dbf329
      iField = iField + 1
      Set oldTable(iField) = dbf329
      oldField(iField) = oldTable(iField).FieldNumber("H0230001")
      NewFieldNames(iField) = "WatPublic"
      
      iField = iField + 1
      Set oldTable(iField) = dbf329
      oldField(iField) = oldTable(iField).FieldNumber("H0230002")
      NewFieldNames(iField) = "WatDrilWel"
      
      iField = iField + 1
      Set oldTable(iField) = dbf329
      oldField(iField) = oldTable(iField).FieldNumber("H0230003")
      NewFieldNames(iField) = "WatDugWell"
      
      iField = iField + 1
      Set oldTable(iField) = dbf329
      oldField(iField) = oldTable(iField).FieldNumber("H0230004")
      NewFieldNames(iField) = "WatOther"
      
      iField = iField + 1
      Set oldTable(iField) = dbf329
      oldField(iField) = oldTable(iField).FieldNumber("H0240001")
      NewFieldNames(iField) = "SewrPublic"
      
      iField = iField + 1
      Set oldTable(iField) = dbf329
      oldField(iField) = oldTable(iField).FieldNumber("H0240002")
      NewFieldNames(iField) = "SewrSeptic"
      
      iField = iField + 1
      Set oldTable(iField) = dbf329
      oldField(iField) = oldTable(iField).FieldNumber("H0240003")
      NewFieldNames(iField) = "SewrOther"
      
      DBFfilename = DestPath & "co" & state_fips & "_d90.dbf"
      Set coDBF = CreateNewShapefile(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
    
      DBFfilename = DestPath & "tr" & state_fips & "_d90.dbf"
      Set trDBF = CreateNewShapefile(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
    
      DBFfilename = DestPath & "bg" & state_fips & "_d90.dbf"
      Set bgDBF = CreateNewShapefile(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
      
      DBFfilename = DestPath & "pl" & state_fips & "_d90.dbf"
      Set plDBF = CreateNewShapefile(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
            
      TryToOpenShapeDBFs = False
    End If
    
    For iField = 1 To numNew90Fields
      If oldField(iField) > 0 Then
        val = oldTable(iField).Value(oldField(iField))
      Else
        val = 0
      End If
      newValues(iField) = val
    Next
    
    Select Case summ_level
      Case "050": If Not coDBF Is Nothing Then AddValue coDBF, newValues, state_fips, county_fips, place_fips, tract, block_group
      Case "140": If Not trDBF Is Nothing Then AddValue trDBF, newValues, state_fips, county_fips, place_fips, tract, block_group
      Case "150": If Not bgDBF Is Nothing Then AddValue bgDBF, newValues, state_fips, county_fips, place_fips, tract, block_group
      Case "160": If Not plDBF Is Nothing Then AddValue plDBF, newValues, state_fips, county_fips, place_fips, tract, block_group
    End Select

    
  Next
  
  GoSub CloseDBFs

  dbf301.Clear
  dbf327.Clear
  dbf329.Clear

Exit Sub

CloseDBFs:
  If Not coDBF Is Nothing Then
    coDBF.WriteFile coDBF.Filename
    Set coDBF = Nothing
  End If
  If Not trDBF Is Nothing Then
    trDBF.WriteFile trDBF.Filename
    Set trDBF = Nothing
  End If
  If Not bgDBF Is Nothing Then
    bgDBF.WriteFile bgDBF.Filename
    Set bgDBF = Nothing
  End If
  If Not plDBF Is Nothing Then
    plDBF.WriteFile plDBF.Filename
    Set plDBF = Nothing
  End If

  Return
End Sub

Private Sub Add303d(SourceFilename As String, DestPath As String)
  Dim SourceDBF As clsATCTable
  Dim DestDBF As clsATCTable
  Dim iFieldDBF As Long
  Dim iRecordDBF As Long
  Dim id_field_dbf As Long
  Dim list_id As String
  Dim id_field_Impairments As Long
  Dim id_field_TMDLs As Long
  Dim id As String
  Dim val As String
  Dim valpos As Long
  'Dim thisWidth As Long
  'Dim maxWidth() As Long
  
  If csvImpairments Is Nothing Then
    Set csvImpairments = New clsATCTableCSV
    csvImpairments.OpenFile "H:\Data\EPA303d\list_impairments.txt"
    Set csvTMDLs = New clsATCTableCSV
    csvTMDLs.OpenFile "H:\Data\EPA303d\tmdls.txt"
  End If
  
  id_field_Impairments = csvImpairments.FieldNumber("LIST_ID")
  id_field_TMDLs = csvTMDLs.FieldNumber("LIST_ID")
  
  'Debug.Print "Opening " & SourceFilename
  Set SourceDBF = New clsATCTableDBF
  SourceDBF.OpenFile SourceFilename
  
  Set DestDBF = New clsATCTableDBF
  DestDBF.NumFields = SourceDBF.NumFields + 6
  ReDim pMaxLength(DestDBF.NumFields)
  For iFieldDBF = 1 To SourceDBF.NumFields
    SetField DestDBF, iFieldDBF, SourceDBF.FieldName(iFieldDBF), _
                                 SourceDBF.FieldType(iFieldDBF), _
                                 SourceDBF.FieldLength(iFieldDBF)
  Next
  SetField DestDBF, iFieldDBF, "WBODY_NAME", "C", 85: iFieldDBF = iFieldDBF + 1
  SetField DestDBF, iFieldDBF, "WBODY_TYPE", "C", 19: iFieldDBF = iFieldDBF + 1
  SetField DestDBF, iFieldDBF, "ST_IMPAIR", "C", 255: iFieldDBF = iFieldDBF + 1
  SetField DestDBF, iFieldDBF, "EPA_IMPAIR", "C", 200: iFieldDBF = iFieldDBF + 1
  SetField DestDBF, iFieldDBF, "TMDL_ID", "N", 22: iFieldDBF = iFieldDBF + 1
  SetField DestDBF, iFieldDBF, "ST_POLUTNT", "C", 90: iFieldDBF = iFieldDBF + 1

  id_field_dbf = SourceDBF.FieldNumber("ENTITY_ID")

  For iRecordDBF = 1 To SourceDBF.NumRecords
    SourceDBF.CurrentRecord = iRecordDBF
    DestDBF.CurrentRecord = iRecordDBF
    For iFieldDBF = 1 To SourceDBF.NumFields
      DestDBF.Value(iFieldDBF) = SourceDBF.Value(iFieldDBF)
    Next
    id = DestDBF.Value(id_field_dbf)
    If csvImpairments.FindFirst(id_field_Impairments, id) Then
      SetValueAndMaxWidth DestDBF, iFieldDBF, csvImpairments.Value(5): iFieldDBF = iFieldDBF + 1
      SetValueAndMaxWidth DestDBF, iFieldDBF, csvImpairments.Value(6): iFieldDBF = iFieldDBF + 1
      SetValueAndMaxWidth DestDBF, iFieldDBF, csvImpairments.Value(7): iFieldDBF = iFieldDBF + 1
      SetValueAndMaxWidth DestDBF, iFieldDBF, csvImpairments.Value(8): iFieldDBF = iFieldDBF + 1
      While csvImpairments.FindNext(id_field_Impairments, id)
        AddValueToCSVfield DestDBF, iFieldDBF - 2, csvImpairments.Value(7)
        AddValueToCSVfield DestDBF, iFieldDBF - 1, csvImpairments.Value(8)
      Wend
    End If
    iFieldDBF = SourceDBF.NumFields + 5
    If csvTMDLs.FindFirst(id_field_TMDLs, id) Then
      DestDBF.Value(iFieldDBF) = csvTMDLs.Value(1): iFieldDBF = iFieldDBF + 1 'TMDL_ID
      DestDBF.Value(iFieldDBF) = csvTMDLs.Value(6): iFieldDBF = iFieldDBF + 1 'ST_POLUTNT
      While csvTMDLs.FindNext(id_field_TMDLs, id)
        AddValueToCSVfield DestDBF, iFieldDBF - 2, csvTMDLs.Value(1)
        AddValueToCSVfield DestDBF, iFieldDBF - 1, csvTMDLs.Value(6)
      Wend
    End If
  Next
  
  'Debug.Print "Writing " & DestPath & FilenameNoPath(SourceFilename)
  DestDBF.WriteFile DestPath & FilenameNoPath(SourceFilename)
  'For iFieldDBF = 1 To DestDBF.NumFields
  '  If pMaxLength(iFieldDBF) > DestDBF.FieldLength(iFieldDBF) Then
  '    Debug.Print "MaxLength(" & iFieldDBF & ") = " & pMaxLength(iFieldDBF) & " - " & DestDBF.FieldName(iFieldDBF)
  '  End If
  'Next
End Sub

Private Sub AddValueToCSVfield(aDest As clsATCTable, aDestField As Long, ByVal aNewValue As String)
  Dim OldValue As String
  Dim lNewValue As String
  Dim valpos As Long
  Dim endpos As Long
  Dim newvalpos As Long
  Dim newendpos As Long
  Dim found As Boolean
  Dim thisMsg As String
  
  Select Case aNewValue
    Case "PCB'S": aNewValue = "PCBS"
    Case "DISSOLVED OXYGEN": aNewValue = "DO"
  End Select
  
  OldValue = aDest.Value(aDestField)
  valpos = InStr(OldValue & ", ", aNewValue & ", ")
SelectValpos:
  Select Case valpos
    Case 0: found = False 'Was not found
    Case 1: found = True  'Value to add is first item in the list
    Case 2: found = False 'Was found too close to start for a comma and space
    Case Else: 'Was found, but make sure it was the whole item
      If Mid(OldValue, valpos - 2, 2) = ", " Then
        found = True
      Else
        'If aNewValue <> "PRIORITY ORGANICS" Then Debug.Print "New value '" & aNewValue & "' is a substring of existing value in '" & OldValue & "'"
        valpos = InStr(valpos + 1, OldValue, aNewValue & ", ")
        GoTo SelectValpos
      End If
  End Select
  If Not found Then
    lNewValue = OldValue & ", " & aNewValue
    
    newvalpos = InStr(aNewValue, "CONTAMINATED SEDIMENTS (")
    If newvalpos > 0 Then
      valpos = InStr(OldValue, "CONTAMINATED SEDIMENTS (")
      If valpos > 0 Then
        newendpos = InStr(aNewValue, ")")
        lNewValue = Mid(aNewValue, newvalpos + 24, newendpos - newvalpos - 24)
        If InStr(valpos + 24, OldValue, lNewValue) > 0 Then
          found = True
        Else
          endpos = InStr(valpos, OldValue, ")")
          lNewValue = Left(OldValue, endpos - 1) & ", " & lNewValue & Mid(OldValue, endpos)
        End If
      End If
    End If
    If Not found Then
      If Len(lNewValue) > aDest.FieldLength(aDestField) Then
        pMaxLength(aDestField) = Len(lNewValue)
        thisMsg = aDest.Filename & " MaxLength(" & aDestField & ") = " & pMaxLength(aDestField) & " - " & aDest.FieldName(aDestField) & " = " & lNewValue
        If thisMsg <> pLastMsg Then Debug.Print thisMsg: pLastMsg = thisMsg
        lNewValue = Left(lNewValue, aDest.FieldLength(aDestField) - 1) & "+"
      End If
      aDest.Value(aDestField) = lNewValue
    End If
  End If
End Sub

Private Sub SetValueAndMaxWidth(aDest As clsATCTable, aDestField As Long, ByVal aValue As String)
  Dim thisMsg As String
  Dim lenValue As Long
  
  Select Case aValue
    Case "ALL TRIBUTARIES ON BRANDYWINE CREEK  FROM THE HEADWATERS AT PA-DE LINE TO THE CONFLUENCE WITH THE CHRISTINA RIVER"
      aValue = "BRANDYWINE CREEK TRIBUTARIES FROM HEADWATERS AT PA-DE LINE TO CHRISTINA RIVER"
    Case "PCB'S": aValue = "PCBS"
    Case "YORK RIVER - NOTE THIS WATER IS NOT ONLY THREATENED BY NATURAL CONDITIONS BUT OTHER SOURCES AS WELL.  THIS WATER IS INCLUDED ON PART I AS IMPAIRED BY NUTRIENTS DUE TO POINT AND NON-POINT SOURCES", _
         "YORK RIVER - NOTE THIS WATER IS NOT ONLY THREATENED BY NATURAL CONDITIONS BUT OTHER SOURCES AS   THIS WATER IS INCLUDED ON PART I AS IMPAIRED BY NUTRIENTS DUE TO POINT AND NON-POINT SOURCES"
      aValue = "YORK RIVER"
    Case "GREEN BAY - SOUTH OF MARINETTE AND ITS TRIBS INCLUDING THE MENOMINEE, OCONTO, FOX & PESHTIGO RIVERS FROM THEIR MOUTHS TO THE FIRST DAM"
      aValue = "GREEN BAY - SOUTH OF MARINETTE, MENOMINEE, OCONTO, FOX & PESHTIGO"
    Case "RIVER ROUGE, (MAIN BR.); UPPER BR.; MIDDLE BR.; LOWER BR; BELL BR.; FRANKLIN BR.; EVANS DITCH"
      aValue = "RIVER ROUGE - MAIN, UPPER, MIDDLE, LOWER, BELL, FRANKLIN, EVANS DITCH"
  End Select
  lenValue = Len(aValue)
  If lenValue > pMaxLength(aDestField) Then pMaxLength(aDestField) = lenValue
  If lenValue > aDest.FieldLength(aDestField) Then
    thisMsg = "Field " & aDestField & " - " & aDest.FieldName(aDestField) & "Truncated from " & Len(aValue) & " to " & aDest.FieldLength(aDestField) & vbCrLf & aValue & vbCrLf & Left(aValue, aDest.FieldLength(aDestField) - 1 & "+")
    If thisMsg = pLastMsg Then
      Debug.Print ".";
    Else
      pLastMsg = thisMsg
      Debug.Print vbCrLf & thisMsg
    End If
    aDest.Value(aDestField) = Left(aValue, aDest.FieldLength(aDestField) - 1 & "+")
  Else
    aDest.Value(aDestField) = aValue
  End If
End Sub

'CSVlist must end with ", "
'Private Function CSVlistContains(CSVlist As String, lookFor As String) As Boolean
'  Dim valpos As Long
'  valpos = InStr(CSVlist, lookFor & ", ")
'SelectValpos:
'  Select Case valpos
'    Case 0: CSVlistContains = False 'Was not found
'    Case 1: CSVlistContains = True  'Value to add is first item in the list
'    Case 2: CSVlistContains = False 'Was found too close to start for a comma and space
'    Case Else: 'Was found, but make sure it was the whole item
'      If Mid(CSVlist, valpos - 2, 2) = ", " Then
'        CSVlistContains = True
'      Else
'        Debug.Print "New value '" & lookFor & "' is a substring of existing value in '" & CSVlist & "'"
'        valpos = InStr(valpos + 1, CSVlist, lookFor & ", ")
'        GoTo SelectValpos
'      End If
'  End Select
'End Function

Private Sub Add2000population(SourceFilename As String, DestPath As String)
  Dim iList As Long
  Dim iShape As Long
  Dim curShape As T_shpPoly
  
  Dim baseFilename As String
  Static starttime As Double
  Dim elapsed As Double
  
  Dim sf1File As Integer
  Dim sf1Record As String
  
  Dim DBFfilename As String
  Dim coDBF As clsATCTable
  Dim trDBF As clsATCTable
  Dim bgDBF As clsATCTable
  Dim plDBF As clsATCTable
  Dim ztDBF As clsATCTable
  
  Dim geoFile As Integer
  Dim geoRecord As String
  Dim summ_level As String
  Dim newValues(1) As String
  
  Dim state_fips As String
  Dim county_fips As String
  Dim place_fips As String
  Dim tract As String
  Dim block_group As String
  Dim zip5 As String
  
  Dim TryToOpenShapeDBFs As Boolean
  
  starttime = Time
    
  geoFile = FreeFile
  Open SourceFilename For Input As geoFile
  sf1File = FreeFile
  Open ReplaceString(SourceFilename, "geo", "00001") For Input As sf1File
  TryToOpenShapeDBFs = True
  While Not EOF(sf1File)
    Line Input #geoFile, geoRecord
    Line Input #sf1File, sf1Record
    summ_level = Mid(geoRecord, 9, 3)
    state_fips = Mid(geoRecord, 30, 2)
    
    If TryToOpenShapeDBFs Then
      GoSub CloseDBFs
      Dim NewFieldNames(1) As String
      Dim NewFieldTypes(1) As String
      Dim NewFieldLengths(1) As Long
      
      NewFieldNames(1) = "Population"
      NewFieldTypes(1) = "N"
      NewFieldLengths(1) = 9
      
      DBFfilename = DestPath & "co" & state_fips & "_d00.dbf"
      Set coDBF = CreateNewShapefile(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
    
      DBFfilename = DestPath & "tr" & state_fips & "_d00.dbf"
      Set trDBF = CreateNewShapefile(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
    
      DBFfilename = DestPath & "bg" & state_fips & "_d00.dbf"
      Set bgDBF = CreateNewShapefile(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
      
      DBFfilename = DestPath & "pl" & state_fips & "_d00.dbf"
      Set plDBF = CreateNewShapefile(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
      
      DBFfilename = DestPath & "zt" & state_fips & "_d00.dbf"
      Set ztDBF = CreateNewShapefile(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
      
      TryToOpenShapeDBFs = False
    End If
    
    newValues(1) = FieldFrom2000Record(NewFieldNames(1), sf1Record) 'population is the only new value for 2000
    
    state_fips = Mid(geoRecord, 30, 2)
    place_fips = Trim(Mid(geoRecord, 46, 5))
    county_fips = Trim(Mid(geoRecord, 32, 3))
    tract = Trim(Mid(geoRecord, 56, 6))
    If Right(tract, 2) = "00" Then tract = Left(tract, 4)

    block_group = Trim(Mid(geoRecord, 62, 1))
    zip5 = Trim(Mid(geoRecord, 161, 5))
    
    Select Case summ_level
      Case "050": If Not coDBF Is Nothing Then AddValue coDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
      Case "140": If Not trDBF Is Nothing Then AddValue trDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
      Case "150": If Not bgDBF Is Nothing Then AddValue bgDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
      Case "160": If Not plDBF Is Nothing Then AddValue plDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
      Case "871": If Not ztDBF Is Nothing Then AddValue ztDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
    End Select
  Wend
  Close geoFile
  GoSub CloseDBFs

Exit Sub

CloseDBFs:
  If Not coDBF Is Nothing Then
    coDBF.WriteFile coDBF.Filename
    Set coDBF = Nothing
  End If
  If Not trDBF Is Nothing Then
    trDBF.WriteFile trDBF.Filename
    Set trDBF = Nothing
  End If
  If Not bgDBF Is Nothing Then
    bgDBF.WriteFile bgDBF.Filename
    Set bgDBF = Nothing
  End If
  If Not plDBF Is Nothing Then
    plDBF.WriteFile plDBF.Filename
    Set plDBF = Nothing
  End If
  If Not ztDBF Is Nothing Then
    ztDBF.WriteFile ztDBF.Filename
    Set ztDBF = Nothing
  End If

  Return
  
End Sub

Private Function FieldFrom2000Record(aFieldName As String, aRecord As String) As String
  Dim valuePos As Long
  Dim valueEndPos As Long
  Dim valueFieldNum As Long
  Dim iField As Long
  
  Select Case aFieldName
    Case "Population": valueFieldNum = 6
    Case "HouseUnits": valueFieldNum = 6
    Case "Occupied":   valueFieldNum = 14
    Case "Vacant":     valueFieldNum = 15
  End Select
  
  'Find comma before the value
  For iField = 1 To valueFieldNum - 1
    valuePos = InStr(valuePos + 1, aRecord, ",")
  Next
  
  'Find comma after the value
  valueEndPos = InStr(valuePos + 1, aRecord, ",")
  If valueEndPos = 0 Then valueEndPos = Len(aRecord) + 1
  
  FieldFrom2000Record = Mid(aRecord, valuePos + 1, valueEndPos - valuePos - 1)
End Function

Private Function KeepField(FieldName As String, layer As String)
  Select Case FieldName
    Case "STATE", "TRACT", "BLKGROUP", "PLC", "NAME"
      KeepField = True
    Case "LSAD_TRANS"
      If layer = "pl" Then KeepField = True
    Case "AREA", "PERIMETER", "LSAD", "ZCTA"
      KeepField = False
    Case "COUNTY", "PLACEFP" 'These get folded into FIPS
      KeepField = False
    Case Else
      If InStr(FieldName, "_D00_") = 0 Then
        Debug.Print "Unknown field " & FieldName
      End If
  End Select
  
End Function

Private Sub MergePoly(shpFile As CShape_IO, iShape As Long, addShape As T_shpPoly)
  'Adds otherShape to shape at index iShape in shpfile
  Dim oldShape As T_shpPoly
  Dim combinedShape As T_shpPoly
  Dim i As Long
  
  oldShape = shpFile.getPoly(iShape)
  If addShape.ShapeType <> oldShape.ShapeType Then
    MsgBox "Cannot add different shape " & addShape.ShapeType & " to existing shape type " & oldShape.ShapeType, vbOKOnly, "MergePoly"
    Exit Sub
  End If
  With combinedShape
    .ShapeType = oldShape.ShapeType
    .Box = oldShape.Box
    If .Box.xMax < addShape.Box.xMax Then .Box.xMax = addShape.Box.xMax
    If .Box.yMax < addShape.Box.yMax Then .Box.yMax = addShape.Box.yMax
    If .Box.xMin > addShape.Box.xMin Then .Box.xMin = addShape.Box.xMin
    If .Box.yMin > addShape.Box.yMin Then .Box.yMin = addShape.Box.yMin
    .NumParts = oldShape.NumParts + addShape.NumParts
    .NumPoints = oldShape.NumPoints + addShape.NumPoints
    If .NumParts > 0 Then
      ReDim .Parts(0 To .NumParts - 1)
      If .ShapeType = FILETYPEENUM.typeMultiPatch Then ReDim .PartTypes(.NumParts)
      For i = 0 To oldShape.NumParts - 1
        .Parts(i) = oldShape.Parts(i)
        If .ShapeType = FILETYPEENUM.typeMultiPatch Then
          .PartTypes(i) = oldShape.PartTypes(i)
        End If
      Next
      For i = oldShape.NumParts To oldShape.NumParts + addShape.NumParts - 1
        .Parts(i) = addShape.Parts(i - oldShape.NumParts) + oldShape.NumPoints
        If .ShapeType = FILETYPEENUM.typeMultiPatch Then
          .PartTypes(i) = addShape.PartTypes(i - oldShape.NumParts)
        End If
      Next
    End If
    ReDim .thePoints(0 To oldShape.NumPoints + addShape.NumPoints - 1)
    For i = 0 To oldShape.NumPoints - 1
      .thePoints(i) = oldShape.thePoints(i)
    Next
    For i = oldShape.NumPoints To oldShape.NumPoints + addShape.NumPoints - 1
      .thePoints(i) = addShape.thePoints(i - oldShape.NumPoints)
    Next
    'TODO: M and Z
  End With
  shpFile.putPoly iShape, combinedShape
End Sub

Private Sub SetField(aDBF As clsATCTable, iField As Long, aFieldName As String, aFieldType As String, aFieldWidth As Long, Optional aFieldDecimals As Long = 0)
  With aDBF
    .FieldName(iField) = aFieldName
    .FieldType(iField) = "C"
    .FieldLength(iField) = aFieldWidth
    If aFieldDecimals <> 0 Then
      Dim ddd As clsATCTableDBF
      Set ddd = aDBF
      ddd.FieldDecimalCount(iField) = aFieldDecimals
    End If
  End With
End Sub

Private Function CreateNewDBF(DBFfilename As String, _
                              EnsureFieldName() As String, _
                              EnsureFieldType() As String, _
                              EnsureFieldLength() As Long) As clsATCTable
  Dim oldDBF As clsATCTable
  Dim oldDBFDBF As clsATCTableDBF
  If FileExists(DBFfilename) Then
    Set oldDBF = New clsATCTableDBF
    oldDBF.OpenFile DBFfilename
    If oldDBF.FieldNumber(EnsureFieldName(1)) = 0 Then
      Dim num_new_fields As Long
      Dim newDBF As clsATCTable
      Dim newDBFDBF As clsATCTableDBF
      Dim iNewField As Long
      Dim iRecord As Long
      Dim layer As String
      Dim Year As String
            
      num_new_fields = UBound(EnsureFieldName)
      layer = LCase(Left(FilenameNoPath(DBFfilename), 2))
      Year = Right(FilenameOnly(DBFfilename), 2) 'census year
            
      Set newDBF = New clsATCTableDBF
      Set newDBFDBF = newDBF
      Set oldDBFDBF = oldDBF
      
      With newDBF
        .NumFields = oldDBF.NumFields + num_new_fields
        For iNewField = 1 To oldDBF.NumFields
          SetField newDBF, iNewField, oldDBF.FieldName(iNewField), oldDBF.FieldType(iNewField), oldDBF.FieldLength(iNewField)
        Next
      
        For iNewField = 1 To num_new_fields
          SetField newDBF, .NumFields - (num_new_fields - iNewField), EnsureFieldName(iNewField), EnsureFieldType(iNewField), EnsureFieldLength(iNewField)
        Next
        .NumRecords = oldDBF.NumRecords
      
        For iRecord = 1 To oldDBF.NumRecords
          oldDBF.CurrentRecord = iRecord
          .CurrentRecord = iRecord
          For iNewField = 1 To oldDBF.NumFields
            .Value(iNewField) = oldDBF.Value(iNewField)
          Next
        Next
        '.WriteFile FilenameSetExt(oldDBF.Filename, "new.dbf")
        .WriteFile PathNameOnly(oldDBF.Filename) & "\new\" & FilenameNoPath(oldDBF.Filename)
        oldDBF.Clear
        Set oldDBF = Nothing
        Set CreateNewDBF = newDBF
      End With
    Else
      Set CreateNewDBF = oldDBF
    End If
  End If
End Function

Private Function CreateNewShapefile(DBFfilename As String, _
                                    EnsureFieldName() As String, _
                                    EnsureFieldType() As String, _
                                    EnsureFieldLength() As Long) As clsATCTable
  Dim oldDBF As clsATCTable
  Dim oldDBFDBF As clsATCTableDBF
  If FileExists(DBFfilename) Then
    Set oldDBF = New clsATCTableDBF
    oldDBF.OpenFile DBFfilename
CheckForField:
    If oldDBF.FieldNumber(EnsureFieldName(1)) = 0 Then
      Dim num_new_fields As Long
      Dim oldSHP As CShape_IO
      Dim newSHP As CShape_IO
      Dim newDBF As clsATCTable
      Dim newDBFDBF As clsATCTableDBF
      Dim iNewField As Long
      Dim iRecord As Long
      Dim layer As String
      Dim Year As String
      
      Dim state_field As Long
      Dim place_fips_field As Long
      Dim place_field As Long
      Dim county_field As Long
      Dim tract_field As Long
      Dim name_field As Long
      Dim zip5_field As Long
      Dim lsad_trans As Long
      
      num_new_fields = UBound(EnsureFieldName)
      layer = LCase(Left(FilenameNoPath(DBFfilename), 2))
      Year = Right(FilenameOnly(DBFfilename), 2) 'census year
      
      Set oldSHP = New CShape_IO
      oldSHP.ShapeFileOpen FilenameSetExt(DBFfilename, "shp"), READWRITEFLAG.ReadOnly
      
      Set newSHP = New CShape_IO
      newSHP.CreateNewShape PathNameOnly(DBFfilename) & "\new\" & FilenameOnly(DBFfilename) & ".shp", oldSHP.getShapeHeader.ShapeType
      
      Set newDBF = New clsATCTableDBF
      Set newDBFDBF = newDBF
      Set oldDBFDBF = oldDBF
      
      With newDBF
        Select Case layer
        Case "co"
          .NumFields = 2 + num_new_fields
          SetField newDBF, 1, "FIPS", "C", 5
          SetField newDBF, 2, "NAME", "C", 31
          state_field = 5
          county_field = 6
          name_field = 7
        Case "tr"
          .NumFields = 2 + num_new_fields
          SetField newDBF, 1, "FIPS", "C", 5
          SetField newDBF, 2, "NAME", "C", 6
          state_field = 5
          county_field = 6
          name_field = 7
          If Year = "90" Then tract_field = 8
        Case "bg"
          .NumFields = 3 + num_new_fields
          SetField newDBF, 1, "FIPS", "C", 5
          SetField newDBF, 2, "NAME", "C", 1
          SetField newDBF, 3, "TRACT", "C", 6
        
          Select Case Year
          Case "00": state_field = 5: county_field = 6: tract_field = 7: name_field = 9
          Case "90": state_field = 10: county_field = 11: tract_field = 12: name_field = 13
          End Select
        
        Case "pl"
          .NumFields = 3 + num_new_fields
          SetField newDBF, 1, "FIPS", "C", 7
          SetField newDBF, 2, "NAME", "C", 35
          SetField newDBF, 3, "PLC", "C", 4
          'SetField newDBF, 4, "LSAD", "C", 16
        
          Select Case Year
          Case "00": state_field = 5: place_field = 6: place_fips_field = 7: name_field = 8: lsad_trans = 10
          Case "90": state_field = 10: place_field = 11: place_fips_field = 13: name_field = 18
          End Select
        
        Case "zt"
          .NumFields = 1 + num_new_fields
          SetField newDBF, 1, "NAME", "C", 5
        Case Else
          MsgBox "Unknown type " & layer, vbOKOnly, "CreateNewShapefile"
          Stop
        End Select
      
        For iNewField = 1 To num_new_fields
          SetField newDBF, .NumFields - (num_new_fields - iNewField), EnsureFieldName(iNewField), EnsureFieldType(iNewField), EnsureFieldLength(iNewField)
        Next
        '.NumRecords = oldDBF.NumRecords
      
        For iRecord = 1 To oldDBF.NumRecords
          oldDBF.CurrentRecord = iRecord
          .CurrentRecord = .NumRecords + 1
          Select Case layer
          Case "co", "tr"
            .Value(1) = oldDBF.Value(state_field) & oldDBF.Value(county_field)
            .Value(2) = oldDBF.Value(name_field)
            If tract_field > 0 Then .Value(2) = .Value(2) & oldDBF.Value(tract_field)
          Case "bg"
            .Value(1) = oldDBF.Value(state_field) & oldDBF.Value(county_field)
            .Value(2) = oldDBF.Value(name_field)
            .Value(3) = oldDBF.Value(tract_field)
          Case "pl"
            .Value(1) = oldDBF.Value(state_field) & oldDBF.Value(place_fips_field)
            .Value(2) = oldDBF.Value(name_field)
            
            If Len(.Value(1)) + Len(.Value(2)) = 0 Then 'discard Nameless/FIPSless places
              '.Value(2) = iRecord 'Use index in place of name if no name
              .NumRecords = .NumRecords - 1
              GoTo SkipShape
            Else
              If Len(.Value(1)) = 0 Then .Value(1) = "0000000": Debug.Print "No FIPS for " & .Value(2)
              .Value(3) = oldDBF.Value(place_field)
              If lsad_trans > 0 Then 'Append LSAD to name to make 2000 match 1990
                If Len(oldDBF.Value(lsad_trans)) > 0 Then
                  If Len(oldDBF.Value(lsad_trans)) + Len(.Value(3)) >= .FieldLength(3) Then
                    Debug.Print "place name + LSAD too long:" & Len(oldDBF.Value(lsad_trans)) + Len(.Value(3)) + 1 & ": " & .Value(3)
                  End If
                  .Value(3) = .Value(3) & " " & oldDBF.Value(lsad_trans)
                End If
              End If
            End If
          Case "zt"
            .Value(1) = oldDBF.Value(6)
          End Select
          newDBFDBF.FindRecord newDBF.record
          If .CurrentRecord < .NumRecords Then 'Already have part of this shape in the file
            'Combine polygon from oldSHP/oldDBF.CurrentRecord into newSHP/newDBF.CurrentRecord
            Debug.Print "Merging record " & iRecord & " into " & .CurrentRecord & " from " & DBFfilename
            MergePoly newSHP, .CurrentRecord, oldSHP.getPoly(iRecord)
            .NumRecords = .NumRecords - 1
          Else
            'Append shape from oldSHP/oldDBF.CurrentRecord to newSHP
            newSHP.putPoly 0, oldSHP.getPoly(iRecord)
          End If
SkipShape:
          If .NumRecords <> newSHP.getRecordCount Then Stop
        Next
        '.WriteFile FilenameSetExt(oldDBF.Filename, "new.dbf")
        .WriteFile PathNameOnly(oldDBF.Filename) & "\new\" & FilenameNoPath(oldDBF.Filename)
        newSHP.FileShutDown
        
        oldSHP.FileShutDown
        oldDBF.Clear
        Set oldDBF = Nothing
        Set CreateNewShapefile = newDBF
      End With
    Else
      Set CreateNewShapefile = oldDBF
    End If
  End If
End Function

Private Sub AddValue(DestDBF As clsATCTable, _
                     newValues() As String, _
                     state_fips As String, _
                     county_fips As String, _
                     place_fips As String, _
                     tract As String, _
                     block_group As String, _
            Optional zip5 As String = "")
  Dim AtMatch As Boolean
  Dim StartedRecord As Long
  
  Dim fips_field As Long
  Dim tract_field As Long
  Dim name_field As Long
  Dim block_group_field As Long
  Dim zip5_field As Long
  
  Dim tract_value As String
  
  Dim iField As Long
  Dim num_new_fields As Long
  
  If Len(place_fips) > 0 Then
    place_fips = state_fips & place_fips
  Else
    place_fips = "X"
  End If
  If Len(county_fips) > 0 Then
    county_fips = state_fips & county_fips
  Else
    county_fips = "X"
  End If
  
  fips_field = DestDBF.FieldNumber("FIPS")
  name_field = DestDBF.FieldNumber("NAME")
  Select Case Left(FilenameNoPath(DestDBF.Filename), 2)
    Case "bg": block_group_field = name_field
               tract_field = DestDBF.FieldNumber("TRACT")
    Case "tr": tract_field = name_field
    Case "zt": zip5_field = name_field
  End Select
  
  AtMatch = False
  StartedRecord = DestDBF.CurrentRecord
CheckForMatch:
  If fips_field > 0 Then
    If DestDBF.Value(fips_field) = county_fips Or DestDBF.Value(fips_field) = place_fips Then
      If tract_field > 0 Then
        tract_value = DestDBF.Value(tract_field)
        If tract_value = tract Or tract_value = tract & "00" Then
          If block_group_field > 0 Then
            If DestDBF.Value(block_group_field) = block_group Then AtMatch = True
          Else
            If Len(block_group) = 0 Then AtMatch = True
          End If
        End If
      Else
        If Len(tract) = 0 Then AtMatch = True
      End If
    End If
  ElseIf zip5_field > 0 Then
    If DestDBF.Value(zip5_field) = zip5 Then AtMatch = True
  End If
  num_new_fields = UBound(newValues)
  If AtMatch Then
    For iField = 1 To num_new_fields
      DestDBF.Value(DestDBF.NumFields - (num_new_fields - iField)) = newValues(iField)
    Next
  Else
    If DestDBF.CurrentRecord = DestDBF.NumRecords Then
      DestDBF.CurrentRecord = 1
    Else
      DestDBF.CurrentRecord = DestDBF.CurrentRecord + 1
    End If
    If DestDBF.CurrentRecord = StartedRecord Then
      Debug.Print "Failed to find record in " & DestDBF.Filename
'      Debug.Print "county_fips = " & county_fips
'      Debug.Print "tract = " & tract
'      Debug.Print "block_group = " & block_group
    Else
      GoTo CheckForMatch
    End If
  End If
End Sub

Private Sub cmdOpenFolder_Click()
  Dim SourceFilename As String
  
  aslSourceFiles.ClearLeft
  aslSourceFiles.ClearRight
  
  Select Case pMode
    Case 0: cdlg.DialogTitle = "Locate a TIGER zip or RT1 file"
    Case 1: cdlg.DialogTitle = "Locate a TIGER shape file"
    Case 2: cdlg.DialogTitle = "Locate a shape file"
    Case 3, 8: cdlg.DialogTitle = "Locate a geo.uf1 file"
    Case 4: cdlg.DialogTitle = "Locate a dbf file"
    Case 5: cdlg.DialogTitle = "Locate a stf301xx.dbf file"
    Case 6: cdlg.DialogTitle = "Locate a dbf file"
    Case 7: cdlg.DialogTitle = "Locate a shape dbf"
  End Select
  
  cdlg.ShowOpen
  txtSourceFolder.Text = PathNameOnly(cdlg.Filename)
  
  Select Case pMode
    Case 3, 8: SourceFilename = Dir(txtSourceFolder.Text & "\*geo.uf1")
    Case Else: SourceFilename = Dir(txtSourceFolder.Text & "\*." & FileExt(cdlg.Filename))
  End Select
  
  While Len(SourceFilename) > 0
    aslSourceFiles.LeftItemFastAdd FilenameNoPath(SourceFilename)
    SourceFilename = Dir
  Wend
End Sub

Private Sub optMode_Click(Index As Integer)
  pMode = Index
End Sub

Private Sub Add2000housing(SourceFilename As String, DestPath As String)
  Dim iList As Long
  Dim iField As Long
  
  Dim baseFilename As String
  Dim elapsed As Double
  
  'Dim sf1File As Integer
  'Dim sf1Record As String
  
  Dim sf1File37 As Integer
  Dim sf1Record37 As String
  
  Dim DBFfilename As String
  Dim coDBF As clsATCTable
  Dim trDBF As clsATCTable
  Dim bgDBF As clsATCTable
  Dim plDBF As clsATCTable
  Dim ztDBF As clsATCTable
  
  Dim geoFile As Integer
  Dim geoRecord As String
  Dim summ_level As String
  
  Dim last_state_fips As String
  Dim state_fips As String
  Dim county_fips As String
  Dim place_fips As String
  Dim tract As String
  Dim block_group As String
  Dim zip5 As String
  
  Dim newValues(numNew00Fields) As String
  Dim NewFieldNames(numNew00Fields) As String
  Dim NewFieldTypes(numNew00Fields) As String
  Dim NewFieldLengths(numNew00Fields) As Long
  
  iField = 1
  'NewFieldNames(iField) = "Population": NewFieldLengths(iField) = 9: NewFieldTypes(iField) = "C": iField = iField + 1
  NewFieldNames(iField) = "HouseUnits": NewFieldLengths(iField) = 9: NewFieldTypes(iField) = "C": iField = iField + 1
  NewFieldNames(iField) = "Occupied":   NewFieldLengths(iField) = 9: NewFieldTypes(iField) = "C": iField = iField + 1
  NewFieldNames(iField) = "Vacant":     NewFieldLengths(iField) = 9: NewFieldTypes(iField) = "C"
    
  geoFile = FreeFile
  Open SourceFilename For Input As geoFile
  'sf1File = FreeFile
  'Open ReplaceString(SourceFilename, "geo", "00001") For Input As sf1File
  sf1File37 = FreeFile
  Open ReplaceString(SourceFilename, "geo", "00037") For Input As sf1File37
  
  While Not EOF(sf1File37)
    Line Input #geoFile, geoRecord
    'Line Input #sf1File, sf1Record
    Line Input #sf1File37, sf1Record37
    summ_level = Mid(geoRecord, 9, 3)
    state_fips = Mid(geoRecord, 30, 2)
    Select Case summ_level
      Case "050", "140", "150", "160", "871"
        If last_state_fips <> state_fips Then
          GoSub CloseDBFs
          GoSub OpenDBFs
          last_state_fips = state_fips
        End If
        
        'newValues(1) = FieldFrom2000Record(NewFieldNames(1), sf1Record)
        For iField = 1 To numNew00Fields
          newValues(iField) = FieldFrom2000Record(NewFieldNames(iField), sf1Record37)
        Next
        'Already got state_fips above
        'state_fips = Mid(geoRecord, 30, 2)
        place_fips = Trim(Mid(geoRecord, 46, 5))
        county_fips = Trim(Mid(geoRecord, 32, 3))
        tract = Trim(Mid(geoRecord, 56, 6))
        If Right(tract, 2) = "00" Then tract = Left(tract, 4)
    
        block_group = Trim(Mid(geoRecord, 62, 1))
        zip5 = Trim(Mid(geoRecord, 161, 5))
        
        Select Case summ_level
          Case "050": If Not coDBF Is Nothing Then AddValue coDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
          Case "140": If Not trDBF Is Nothing Then AddValue trDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
          Case "150": If Not bgDBF Is Nothing Then AddValue bgDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
          Case "160": If Not plDBF Is Nothing Then AddValue plDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
          Case "871": If Not ztDBF Is Nothing Then AddValue ztDBF, newValues, state_fips, county_fips, place_fips, tract, block_group, zip5
        End Select
      End Select
  Wend
  Close geoFile
  'Close sf1File
  Close sf1File37
  GoSub CloseDBFs

Exit Sub

OpenDBFs:
  Debug.Print "Opening DBFs for " & state_fips
  DBFfilename = DestPath & "co" & state_fips & "_d00.dbf"
  Set coDBF = CreateNewDBF(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)

  DBFfilename = DestPath & "tr" & state_fips & "_d00.dbf"
  Set trDBF = CreateNewDBF(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)

  DBFfilename = DestPath & "bg" & state_fips & "_d00.dbf"
  Set bgDBF = CreateNewDBF(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
  
  DBFfilename = DestPath & "pl" & state_fips & "_d00.dbf"
  Set plDBF = CreateNewDBF(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
  
  DBFfilename = DestPath & "zt" & state_fips & "_d00.dbf"
  Set ztDBF = CreateNewDBF(DBFfilename, NewFieldNames, NewFieldTypes, NewFieldLengths)
    
  Return

CloseDBFs:
  If Not coDBF Is Nothing Then coDBF.WriteFile coDBF.Filename: Set coDBF = Nothing
  If Not trDBF Is Nothing Then trDBF.WriteFile trDBF.Filename: Set trDBF = Nothing
  If Not bgDBF Is Nothing Then bgDBF.WriteFile bgDBF.Filename: Set bgDBF = Nothing
  If Not plDBF Is Nothing Then plDBF.WriteFile plDBF.Filename: Set plDBF = Nothing
  If Not ztDBF Is Nothing Then ztDBF.WriteFile ztDBF.Filename: Set ztDBF = Nothing

  Return
  
End Sub

