VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.10#0"; "ATCoCtl.ocx"
Begin VB.Form frmWDMOut 
   Caption         =   "WDM Output"
   ClientHeight    =   4830
   ClientLeft      =   75
   ClientTop       =   360
   ClientWidth     =   8025
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmWDMOut.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4830
   ScaleWidth      =   8025
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   372
      Left            =   120
      TabIndex        =   25
      Top             =   4320
      Width           =   7812
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   4080
         TabIndex        =   27
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "&OK"
         Default         =   -1  'True
         Height          =   375
         Left            =   2880
         TabIndex        =   26
         Top             =   0
         Width           =   852
      End
      Begin VB.Label lblStatus 
         Height          =   252
         Left            =   0
         TabIndex        =   28
         Top             =   120
         Width           =   7812
      End
   End
   Begin VB.Frame fraYear 
      Caption         =   "Output Dates"
      Height          =   1980
      Left            =   5160
      TabIndex        =   12
      Top             =   2160
      Visible         =   0   'False
      Width           =   2775
      Begin ATCoCtl.ATCoText atxEmonth 
         Height          =   255
         Left            =   1800
         TabIndex        =   16
         Top             =   1080
         Width           =   615
         _ExtentX        =   1085
         _ExtentY        =   450
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   12
         HardMin         =   1
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   "12"
         Value           =   "12"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxSmonth 
         Height          =   255
         Left            =   1800
         TabIndex        =   15
         Top             =   600
         Width           =   615
         _ExtentX        =   1085
         _ExtentY        =   450
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   12
         HardMin         =   1
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   "1"
         Value           =   "1"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxEyear 
         Height          =   255
         Left            =   840
         TabIndex        =   14
         Top             =   1080
         Width           =   855
         _ExtentX        =   1508
         _ExtentY        =   450
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   2100
         HardMin         =   1800
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   "2100"
         Value           =   "2100"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxSyear 
         Height          =   255
         Left            =   840
         TabIndex        =   13
         Top             =   600
         Width           =   855
         _ExtentX        =   1508
         _ExtentY        =   450
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   2100
         HardMin         =   1800
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   "1800"
         Value           =   "1800"
         Enabled         =   -1  'True
      End
      Begin VB.Label Label4 
         Caption         =   "End"
         Height          =   255
         Left            =   240
         TabIndex        =   20
         Top             =   1080
         Width           =   495
      End
      Begin VB.Label Label3 
         Caption         =   "Start"
         Height          =   255
         Left            =   240
         TabIndex        =   19
         Top             =   600
         Width           =   495
      End
      Begin VB.Label Label2 
         Alignment       =   2  'Center
         Caption         =   "Year"
         Height          =   255
         Left            =   840
         TabIndex        =   18
         Top             =   360
         Width           =   855
      End
      Begin VB.Label Label1 
         Alignment       =   2  'Center
         Caption         =   "Month"
         Height          =   255
         Left            =   1800
         TabIndex        =   17
         Top             =   360
         Width           =   615
      End
   End
   Begin VB.Frame fraMet 
      Caption         =   "Met Station"
      Height          =   1980
      Left            =   120
      TabIndex        =   5
      Top             =   2160
      Width           =   4932
      Begin VB.ListBox lstMet 
         Enabled         =   0   'False
         Height          =   1230
         Left            =   240
         MultiSelect     =   1  'Simple
         TabIndex        =   6
         Top             =   360
         Width           =   4452
      End
   End
   Begin VB.Frame fraModel 
      Caption         =   "Model"
      Height          =   632
      Left            =   120
      TabIndex        =   2
      Top             =   0
      Width           =   7812
      Begin VB.OptionButton opnSWAT 
         Caption         =   "SWAT"
         Height          =   255
         Left            =   240
         TabIndex        =   21
         Top             =   260
         Value           =   -1  'True
         Width           =   1212
      End
      Begin VB.OptionButton opnOldSWAT 
         Caption         =   "Old SWAT"
         Height          =   255
         Left            =   2640
         TabIndex        =   4
         Top             =   240
         Width           =   1212
      End
      Begin VB.OptionButton opnGWLF 
         Caption         =   "GWLF"
         Height          =   255
         Left            =   1560
         TabIndex        =   3
         Top             =   260
         Width           =   975
      End
   End
   Begin VB.Frame fraFiles 
      Caption         =   "Files"
      Height          =   1332
      Left            =   120
      TabIndex        =   0
      Top             =   720
      Width           =   7815
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   22
         Top             =   600
         Width           =   735
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   9
         Top             =   960
         Width           =   735
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   1
         Top             =   240
         Width           =   735
      End
      Begin VB.Label lblWDM 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   1  'Fixed Single
         Caption         =   "<none>"
         ForeColor       =   &H80000008&
         Height          =   252
         Index           =   2
         Left            =   2520
         TabIndex        =   24
         Top             =   600
         Width           =   5172
      End
      Begin VB.Label lblFile 
         Caption         =   "WDM DBF"
         Height          =   252
         Index           =   2
         Left            =   960
         TabIndex        =   23
         Top             =   600
         Width           =   1452
      End
      Begin VB.Label lblFile 
         Caption         =   "Output Location"
         Height          =   252
         Index           =   1
         Left            =   960
         TabIndex        =   11
         Top             =   960
         Width           =   1452
      End
      Begin VB.Label lblWDM 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   1  'Fixed Single
         Caption         =   "<none>"
         ForeColor       =   &H80000008&
         Height          =   252
         Index           =   1
         Left            =   2520
         TabIndex        =   10
         Top             =   960
         Width           =   5172
      End
      Begin VB.Label lblFile 
         Caption         =   "Met WDM File"
         Height          =   252
         Index           =   0
         Left            =   960
         TabIndex        =   8
         Top             =   240
         Width           =   1452
      End
      Begin VB.Label lblWDM 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   1  'Fixed Single
         Caption         =   "<none>"
         ForeColor       =   &H80000008&
         Height          =   252
         Index           =   0
         Left            =   2520
         TabIndex        =   7
         Top             =   240
         Width           =   5172
      End
   End
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   7440
      Top             =   3480
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
End
Attribute VB_Name = "frmWDMOut"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
'Dim WDMId$(4)
Dim MetDetails$()

Private Sub cmdFile_Click(index As Integer)
  Dim iwdm&, i&, s$, f$, fun&, numMetSeg&, arrayMetSegs$(), wid$
  Dim tmetseg, lMetDetails$()
  
  On Error GoTo NeverMind
  
  Select Case index
    Case 0 'met wdm file

'      If lblWDM(0).Caption <> "<none>" Then
'        Call MsgBox("Only one Met WDM file may be opened.", _
'                    vbOKOnly, "WDM Output Problem")
'      Else
        If Dir("\basins\data\met_data", vbDirectory) = "met_data" Then
          ChDir "\basins\data\met_data"
        End If
        CDFile.flags = &H1806&
        CDFile.Filter = "WDM files (*.wdm)|*.wdm"
        CDFile.Filename = "*.wdm"
        CDFile.DialogTitle = "Select Met WDM File"
        CDFile.CancelError = True
        CDFile.ShowOpen
        f = CDFile.Filename
        lblWDM(0).Caption = f
        OpenWDMFile f, fun
        If fun < 1 Then
          Call MsgBox("Problem opening the Met WDM file.", _
                      vbOKOnly, "WDM Output Problem")
        Else
          lstMet.Clear
          GetMetSegNames fun, numMetSeg, arrayMetSegs, lMetDetails
          If numMetSeg > 0 Then
            tmetseg = 0
                 
            ReDim Preserve MetDetails(tmetseg + numMetSeg)
            For i = 0 To numMetSeg - 1
              lstMet.AddItem arrayMetSegs(i)
              MetDetails(tmetseg + i) = lMetDetails(i) & "," & wid
            Next i
            lstMet.Enabled = True
            If lstMet.SelCount = 0 Then
              lstMet.Selected(0) = True
            End If
          End If
        End If
'      End If
    Case 1 'output file
      If opnSWAT.Value Then
        lblWDM(1).Caption = BrowseFolder(Me, "Choose a place to save SWAT files", CurDir, True)
        If Len(lblWDM(1).Caption) = 0 Then lblWDM(1).Caption = "<none>"
      Else
        If Dir("\basins\modelout", vbDirectory) = "modelout" Then
          ChDir "\basins\modelout"
        End If
        
        CDFile.flags = &H8806&
        CDFile.Filter = "Output File (*.*)"
        CDFile.Filename = "*.*"
        CDFile.DialogTitle = "Select Output Weather File"
        CDFile.CancelError = True
        CDFile.ShowSave
        lblWDM(1).Caption = CDFile.Filename
      End If
    Case 2
      CDFile.flags = &H1806&
      CDFile.Filter = "WDM files (*.dbf)|*.dbf"
      CDFile.Filename = "WDM.dbf"
      CDFile.DialogTitle = "Select WDM.dbf File"
      CDFile.CancelError = True
      CDFile.ShowOpen
      If Len(CDFile.Filename) > 0 Then
        If Len(Dir(CDFile.Filename)) > 0 Then lblWDM(2).Caption = CDFile.Filename
      End If
  End Select
NeverMind:
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub SetStatus(newStatus As String)
  Debug.Print Time & " Status: " & newStatus
  If Len(newStatus) > 0 Then
    lblStatus = newStatus & "..."
    cmdOk.Visible = False
    cmdCancel.Visible = False
  Else
    lblStatus = newStatus
    cmdOk.Visible = True
    cmdCancel.Visible = True
  End If
  DoEvents
End Sub

Private Sub cmdOk_Click()
  Dim i&, outwdm$, csdat&(5), cedat&(5)
  Dim PrecTs As ATCclsTserData
  Dim AvgTempTs As ATCclsTserData
  Dim MaxTempTs As ATCclsTserData
  Dim MinTempTs As ATCclsTserData
  Dim SolarTs As ATCclsTserData
  Dim WindTs As ATCclsTserData
  Dim HumidTs As ATCclsTserData
  Dim DewpointTs As ATCclsTserData
  Dim MetIndex As Long
  Dim MetName As String
  Dim MetLat As Single
  Dim MetLon As Single
  Dim MetEle As Single
  Dim WritingDBF As Boolean
  Dim dbf() As clsDBF
  Dim dbfindex As Integer
  Dim lastDBF As Integer
  Dim dataDBF As clsDBF
  Dim WDMDBF As New clsDBF
  Dim MathArgs As Collection
  Dim OutputDir As String
  
  WritingDBF = opnSWAT.Value
  
  If lblWDM(0) = "<none>" Then
    Call MsgBox("Met WDM File required but not specified.", vbOKOnly, "WDM Output Problem")
  ElseIf lblWDM(1) = "<none>" Then
    Call MsgBox("Output Location required but not specified.", vbOKOnly, "WDM Output Problem")
  ElseIf (lblWDM(2) = "<none>" And WritingDBF) Then
    Call MsgBox("WDM DBF required but not specified.", vbOKOnly, "WDM Output Problem")
  Else
    
    If lstMet.SelCount = 0 Then
      If lstMet.ListCount > 0 Then
        lstMet.Selected(0) = True
      Else
        MsgBox "No Met stations are available", vbOKOnly, "WDM Output Problem"
        Exit Sub
      End If
    ElseIf lstMet.SelCount > 1 Then
      If Not WritingDBF Then
        MsgBox "Only one met station may be selected for this type of output", vbOKOnly, "WDM Output Problem"
        Exit Sub
      End If
    End If
        
    'specifications okay, continue
    Me.MousePointer = vbHourglass
    csdat(0) = atxSyear.Value
    csdat(1) = atxSmonth.Value
    csdat(2) = 1
    csdat(3) = 0
    csdat(4) = 0
    csdat(5) = 0
    
    cedat(0) = atxEyear.Value
    cedat(1) = atxEmonth.Value
    cedat(2) = 31
    cedat(3) = 24
    cedat(4) = 0
    cedat(5) = 0
    
    If WritingDBF Then
      OutputDir = lblWDM(1).Caption
      SetStatus "Opening WDM DBF"
      WDMDBF.OpenDBF lblWDM(2)
      SetStatus "Initializing new location tables"
      lastDBF = 4
      ReDim dbf(lastDBF)
      For dbfindex = 0 To lastDBF
        Set dbf(dbfindex) = New clsDBF
        With dbf(dbfindex)
          .NumRecords = lstMet.SelCount
          .numFields = 5
          .FieldLength(1) = 4: .FieldType(1) = "N": .fieldName(1) = "ID"
          .FieldLength(2) = 8: .FieldType(2) = "C": .fieldName(2) = "NAME"
          .FieldLength(3) = 8: .FieldType(3) = "N": .fieldName(3) = "LAT":  .FieldDecimalCount(3) = 4
          .FieldLength(4) = 8: .FieldType(4) = "N": .fieldName(4) = "LONG": .FieldDecimalCount(4) = 4
          .FieldLength(5) = 8: .FieldType(5) = "N": .fieldName(5) = "ELEVATION"
          .InitData
          .CurrentRecord = 1
        End With
      Next
    End If
    For MetIndex = 0 To lstMet.ListCount - 1
      If lstMet.Selected(MetIndex) Then
        MetName = lstMet.List(MetIndex)
        
        If WritingDBF Then
          If WDMDBF.FindFirst(WDMDBF.FieldNumber("COOP_ID"), Right(MetName, 4)) Then
            MetLat = WDMDBF.Value(WDMDBF.FieldNumber("LAT_DD"))
            MetLon = WDMDBF.Value(WDMDBF.FieldNumber("LONG_DD"))
            MetEle = WDMDBF.Value(WDMDBF.FieldNumber("ELEV_FT")) * 0.3048 'Convert feet to meters
          Else
            Debug.Print Right(MetName, 4) & " not found in WDM.DBF so Lat, Long, Elev will be set to zero"
            MetLat = 0
            MetLon = 0
            MetEle = 0
          End If
        End If
        SetStatus "Processing " & MetName & " Lat: " & MetLat & " Lon: " & MetLon & " Elev: " & MetEle
        
        Set PrecTs = Nothing
        Set AvgTempTs = Nothing
        Set MaxTempTs = Nothing
        Set MinTempTs = Nothing
        If opnGWLF.Value Then 'gwlf -- get avg temp data
          Set PrecTs = FindAndTransformTimSer("OBSERVED", MetName, "PREC", csdat, cedat, 1, "MULT", 25.4, "PCP")
          If PrecTs Is Nothing Then
            MsgBox "Required time series PREC missing, skipping " & MetName, vbOKOnly, "WDM Output Problem"
          Else
            Set AvgTempTs = FindAndTransformTimSer("OBSERVED", MetName, "ATEM", csdat, cedat, 0, "F2C")
            If AvgTempTs Is Nothing Then
              MsgBox "Required time series ATEM missing - can't calculate average temp.", vbOKOnly, "WDM Output Problem"
            Else
              Call OutputGWLF(PrecTs, AvgTempTs)
            End If
          End If
        Else 'swat
          Set PrecTs = FindAndTransformTimSer("OBSERVED", MetName, "PREC", csdat, cedat, 1, "MULT", 25.4, "PCP")
          Set MaxTempTs = FindAndTransformTimSer("OBSERVED", MetName, "ATEM", csdat, cedat, 2, "F2C", , "Max")
          Set MinTempTs = FindAndTransformTimSer("OBSERVED", MetName, "ATEM", csdat, cedat, 3, "F2C", , "Min")
          If Not WritingDBF Then 'write old SWAT text format
            If PrecTs Is Nothing Or MaxTempTs Is Nothing Or MinTempTs Is Nothing Then
              MsgBox "Required time series missing, skipping " & MetName, vbOKOnly, "WDM Output Problem"
            Else
              Call OutputSWAT(PrecTs, MaxTempTs, MinTempTs)
            End If
          Else 'write new SWAT DBF
            Set AvgTempTs = FindAndTransformTimSer("OBSERVED", MetName, "ATEM", csdat, cedat, 0, "F2C")
            'langleys (cal/square cm) to MJ/square meter
            'DSOL is daily summed langleys, SOLR is hourly summed langleys
            Set SolarTs = FindAndTransformTimSer("OBSERVED", MetName, "DSOL", csdat, cedat, -1, "MULT", 0.04184, "SLR")
            'mph -> m/s: 1609.344 m/mi, 3600 sec/hr
            Set WindTs = FindAndTransformTimSer("OBSERVED", MetName, "WIND", csdat, cedat, 0, "MULT", (1609.344 / 3600), "WND")
            Set DewpointTs = FindAndTransformTimSer("OBSERVED", MetName, "DPTP", csdat, cedat, -1, "F2C", 0)
            If Not AvgTempTs Is Nothing And Not DewpointTs Is Nothing Then
              Set HumidTs = ComputeRelativeHumidity(AvgTempTs, DewpointTs)
            End If
            MetName = Left(FilenameOnly(lblWDM(0)), 2) & Right(MetName, 4)
            For dbfindex = 0 To lastDBF
              With dbf(dbfindex)
                .Value(1) = Right(MetName, 4)
                .Value(3) = MetLat
                .Value(4) = MetLon
                .Value(5) = MetEle
              End With
            Next
            If Not PrecTs Is Nothing Then
              SetStatus "Filling Precipitation in DBF for " & MetName
              dbf(0).Value(2) = MetName & "_P"
              Set dataDBF = FillDBF(PrecTs)
              SetStatus "Writing Max temperature DBF to disk for " & MetName
              dataDBF.WriteDBF OutputDir & dbf(0).Value(2)
              Set dataDBF = Nothing
            End If
            If Not MaxTempTs Is Nothing And Not MinTempTs Is Nothing Then
              SetStatus "Filling Max and Min temperature in DBF for " & MetName
              dbf(1).Value(2) = MetName & "_T"
              Set dataDBF = FillDBF(MaxTempTs, MinTempTs)
              SetStatus "Writing Min temperature DBF to disk for " & MetName
              dataDBF.WriteDBF OutputDir & dbf(1).Value(2)
              Set dataDBF = Nothing
            End If
            If Not SolarTs Is Nothing Then
              SetStatus "Filling Solar Radiation in DBF for " & MetName
              dbf(2).Value(2) = MetName & "_S"
              Set dataDBF = FillDBF(SolarTs)
              SetStatus "Writing Solar Radiation DBF to disk for " & MetName
              dataDBF.WriteDBF OutputDir & dbf(2).Value(2)
              Set dataDBF = Nothing
            End If
            If Not WindTs Is Nothing Then
              SetStatus "Filling Wind in DBF for " & MetName
              dbf(3).Value(2) = MetName & "_W"
              Set dataDBF = FillDBF(WindTs)
              SetStatus "Writing Wind DBF to disk for " & MetName
              dataDBF.WriteDBF OutputDir & dbf(3).Value(2)
              Set dataDBF = Nothing
            End If
            If Not HumidTs Is Nothing Then
              SetStatus "Filling Relative Humidity in DBF for " & MetName
              dbf(4).Value(2) = MetName & "_H"
              Set dataDBF = FillDBF(HumidTs)
              dataDBF.fieldName(2) = "HMD"
              SetStatus "Writing Relative Humidity DBF to disk for " & MetName
              dataDBF.WriteDBF OutputDir & dbf(4).Value(2)
              Set dataDBF = Nothing
            End If

            For dbfindex = 0 To lastDBF
              dbf(dbfindex).CurrentRecord = dbf(dbfindex).CurrentRecord + 1
            Next
          End If
        End If
      End If
    Next
    If WritingDBF Then
      'always have one too many records, remove one
      For dbfindex = 0 To lastDBF
        dbf(dbfindex).NumRecords = dbf(dbfindex).NumRecords - 1
      Next
      SetStatus "Writing location tables"
      MetName = OutputDir & FilenameOnly(lblWDM(0))
      dbf(0).WriteDBF MetName & "_PREC.dbf"
      dbf(1).WriteDBF MetName & "_ATEM.dbf"
      dbf(2).WriteDBF MetName & "_SOLR.dbf"
      dbf(3).WriteDBF MetName & "_WIND.dbf"
      dbf(4).WriteDBF MetName & "_HUMD.dbf"
    End If
    Me.MousePointer = vbNormal
    SetStatus ""
    Unload Me
  End If
End Sub

Private Sub Form_Load()
  Dim ff As ATCoFindFile
  
  Set ff = New ATCoFindFile
  ff.SetDialogProperties "Please locate WDMOutput.chm", "..\..\..\docs\WDMOutput.chm"
  ff.SetRegistryInfo "WDMOutput", "files", "WDMOutput.chm"
  App.HelpFile = ff.GetName
  lblWDM(0).Caption = "<none>"
End Sub

Private Sub OpenWDMFile(Name$, fun&)
  Dim TserIndex&, s$
  Dim regkey As Variant, iKey&
  
  fun = 0
  Set TserFiles = Nothing
  Set TserFiles = New ATCData.ATCPlugInManager
  TserFiles.Load (TserFileClassName)
  TserIndex = TserFiles.AvailIndexByName("clsTSerWDM")
  If TserIndex = 0 Then 'not a valid type
    MsgBox " OpenWDMFile Error: " & TserFiles.ErrorDescription, vbExclamation
  End If
  TserFiles.Create TserIndex
  
  Set ProjectWDMFile = TserFiles.CurrentActive.obj
  TserFiles.CurrentActive.obj.MsgUnit = HspfMsgUnit
  
  ProjectWDMFile.Filename = Name
  'check read errors
  s = ProjectWDMFile.ErrorDescription
  If Len(s) > 0 Then 'had a problem
    MsgBox s, vbExclamation, "AddWDMFile Read Error on " & Name
    TserFiles.Delete TserFiles.CurrentActiveIndex
    fun = 0
  Else
    fun = ProjectWDMFile.FileUnit
  End If
  
End Sub

Public Sub GetMetSegNames(fun&, numMetSeg&, arrayMetSegs$(), lMetDetails$())
  Dim dsn&, i&, loc$, j&, tempsj As Double, tempej As Double
  Dim sen$, Con$, sdat&(6), edat&(6)
  Dim lTs As Collection 'of atcotimser
  Dim ldate As ATCclsTserDate, sj As Double, ej As Double
  Dim llocts As Collection 'of atcotimser

  numMetSeg = 0
    
  'look for matching WDM datasets
  Call FindTimSer("OBSERVED", "", "PREC", lTs)
  'return the names of the data sets from this wdm file
  For i = 1 To lTs.Count
    loc = lTs(i).Header.loc
    If Len(loc) > 0 And lTs(i).File.Label = "WDM" And lTs(i).File.FileUnit = fun Then
      'first get the common dates from all timsers at this location
      Call FindTimSer("OBSERVED", loc, "", llocts)
      Set ldate = llocts(1).dates
      sj = ldate.Summary.SJDay
      ej = ldate.Summary.EJDay
      For j = 2 To lTs.Count
        Set ldate = llocts(j).dates
        tempsj = ldate.Summary.SJDay
        tempej = ldate.Summary.EJDay
        If tempsj > sj Then sj = tempsj
        If tempej < ej Then ej = tempej
      Next j
      'now save info about this met station
      numMetSeg = numMetSeg + 1
      ReDim Preserve arrayMetSegs(numMetSeg)
      arrayMetSegs(numMetSeg - 1) = loc
      ReDim Preserve lMetDetails(numMetSeg)
      dsn = lTs(i).Header.id
      Call J2Date(sj, sdat)
      Call J2Date(ej, edat)
      Call timcnv(edat)
      lMetDetails(numMetSeg - 1) = CStr(dsn) & "," & _
        CStr(sdat(0)) & "," & CStr(sdat(1)) & "," & CStr(sdat(2)) & "," & CStr(sdat(3)) & "," & CStr(sdat(4)) & "," & CStr(sdat(5)) & "," & _
        CStr(edat(0)) & "," & CStr(edat(1)) & "," & CStr(edat(2)) & "," & CStr(edat(3)) & "," & CStr(edat(4)) & "," & CStr(edat(5))
    End If
  Next i

End Sub

Private Function FindAndTransformTimSer(sen$, loc$, Con$, csdat&(), cedat&(), cdtran As Long, MathOperation As String, _
                           Optional MathArgument As Variant = 0, _
                           Optional NewDescription As String = "") As ATCclsTserData
  Dim lTs As Collection 'of atcotimser
  Dim ots As Collection 'of atcotimser
  Dim MathArgs As New Collection
  Dim StatusString As String
  
  StatusString = Con & " at " & loc
  
  SetStatus "Finding " & StatusString
  
  FindTimSer sen, loc, Con, lTs
  
  If lTs.Count > 0 Then
    If cdtran >= 0 Then
      Select Case cdtran
        Case 0: StatusString = "Average " & StatusString
        Case 1: StatusString = "Sum/Div " & StatusString
        Case 2: StatusString = "Max " & StatusString
        Case 3: StatusString = "Min " & StatusString
      End Select
      SetStatus "Filling " & StatusString & " to Daily"
      Set ots = FillTimSerDaily(lTs, csdat, cedat, cdtran)
      MathArgs.Add ots(1)
    Else
      MathArgs.Add lTs(1)
    End If
    If Len(MathOperation) > 0 Then
      StatusString = StatusString & " (" & MathOperation
      If MathArgument <> 0 Then
        MathArgs.Add MathArgument
        StatusString = StatusString & " " & MathArgument
      End If
      SetStatus "Converting units for " & StatusString & ")"
      Set FindAndTransformTimSer = MathColl(MathOperation, MathArgs)
    Else
      Set FindAndTransformTimSer = MathArgs(1)
    End If
    If Len(NewDescription) > 0 Then FindAndTransformTimSer.AttribSet "DESC", NewDescription
  Else
    Set FindAndTransformTimSer = Nothing
  End If
  
End Function

Private Sub FindTimSer(sen$, loc$, Con$, lTs As Collection)
  Dim dsn&, i&, j&, k&, l&, s$, GRPSIZ&, r!, imatch%
  Dim vTserFile As Variant, curClsTserFile As ATCclsTserFile, lds As ATTimSerDateSummary

  Set lTs = Nothing
  Set lTs = New Collection
  For Each vTserFile In TserFiles.Active
    Set curClsTserFile = vTserFile.obj
    For j = 1 To curClsTserFile.DataCount
      If (sen = curClsTserFile.Data(j).Header.sen Or Len(Trim(sen)) = 0) And _
         (loc = curClsTserFile.Data(j).Header.loc Or Len(Trim(loc)) = 0) And _
         (Con = curClsTserFile.Data(j).Header.Con Or Len(Trim(Con)) = 0) Then 'need this timser
        lTs.Add curClsTserFile.Data(j)
      End If
    Next j
  Next vTserFile
End Sub

Private Sub Form_Resize()
  Dim w As Long, h As Long
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > 3900 And h > 3700 Then
    fraMet.Height = h - 2856
    lstMet.Height = fraMet.Height - 470
    fraYear.Height = fraMet.Height
    fraButtons.Top = fraMet.Top + fraMet.Height + 120
    
    fraButtons.Left = (w - fraButtons.Width) / 2
    fraModel.Width = w - 220
    fraFiles.Width = fraModel.Width
    lblWDM(0).Width = fraFiles.Width - 2643
    lblWDM(1).Width = lblWDM(0).Width
    lblWDM(2).Width = lblWDM(0).Width
    fraMet.Width = w - 3096
    lstMet.Width = fraMet.Width - 480
    fraYear.Left = fraMet.Left + fraMet.Width + 108
  End If
End Sub

Private Sub lstMet_Click()
  Dim SDate&(5), EDate&(5)
  Dim basedsn&, MetDataDetails$, delim$, quote$, i& ', metwdmid$
  
  MetDataDetails = MetDetails(lstMet.ListIndex)
  'get details from the met data details
  delim = ","
  quote = """"
  basedsn = StrSplit(MetDataDetails, delim, quote)
  For i = 0 To 5
    SDate(i) = StrSplit(MetDataDetails, delim, quote)
  Next i
  For i = 0 To 5
    EDate(i) = StrSplit(MetDataDetails, delim, quote)
  Next i
'  metwdmid = MetDataDetails
  
  atxSyear.Value = SDate(0)
  atxSmonth.Value = SDate(1)
  atxEyear.Value = EDate(0)
  atxEmonth.Value = EDate(1)
  
  fraYear.Visible = True
End Sub

Public Function FillTimSerDaily(ts As Collection, csdat&(), cedat&(), cdtran&) As Collection
  Dim v As Variant
  Dim JSdt#, JEdt#, Sdt&(6), Edt&(6)
  Dim lTs As Collection
  Dim ds As ATCclsTserDate
  Dim subSet As ATCclsTserData
  Dim aggDat As ATCclsTserData

  JSdt = Date2J(csdat())
  JEdt = Date2J(cedat())
  
  Set lTs = Nothing
  Set lTs = New Collection
  
  Dim lDateSummary As ATTimSerDateSummary
  With lDateSummary
    .CIntvl = True
    .SJDay = JSdt
    .EJDay = JEdt
    .ts = 1
    .Tu = 4
    .Intvl = 1
    .NVALS = (JEdt - JSdt) / .Intvl '+ 1 jlk 8/2/00 (for listing)
  End With
  Set ds = New ATCclsTserDate
  ds.Summary = lDateSummary
  For Each v In ts
    Set subSet = v.SubSetByDate(JSdt, JEdt)
    Set aggDat = subSet.Aggregate(ds, cdtran)
    lTs.Add aggDat
    Set aggDat = Nothing
    Set subSet = Nothing
  Next
  
  'get the data values for selected data sets
  Set FillTimSerDaily = Nothing
  Set FillTimSerDaily = lTs
  'End If
End Function

Private Sub OutputGWLF(PrecTs As ATCclsTserData, AvgTempTs As ATCclsTserData)
  Dim i&, iday&, dates&(5), counter&, j&, r1!, r2!
  
  Open lblWDM(1).Caption For Output As #1
  If PrecTs.dates.Summary.NVALS = AvgTempTs.dates.Summary.NVALS And _
    PrecTs.dates.Value(1) = AvgTempTs.dates.Value(1) Then
    'start writing values
    counter = 1
    Do Until counter + PrecTs.dates.Summary.SJDay - 1 >= PrecTs.dates.Summary.EJDay
      Call J2Date(PrecTs.dates.Value(counter), dates())
      iday = daymon(dates(0), dates(1))
      Write #1, iday
      For j = 0 To iday - 1
        r1 = AvgTempTs.Value(counter + j) 'ConvertF2C(AvgTempTs.Value(counter + j))
        r2 = PrecTs.Value(counter + j) 'ConvertIn2MM(PrecTs.Value(counter + j)) / 10#
        Write #1, CSng(NumFmted(r1, 4, 1)), CSng(NumFmted(r2, 4, 1))
      Next j
      counter = counter + iday
    Loop
  Else
    Call MsgBox("Common time period required.", vbOKOnly, "WDM Output Problem")
  End If
  Close #1
End Sub

Private Sub OutputSWAT(PrecTs As ATCclsTserData, MaxTempTs As ATCclsTserData, MinTempTs As ATCclsTserData)
  Dim i&, iday&, dates&(5), counter&, j&, rjul!, iyear&, r1!, r2!, cjul$, cyear$
  Dim monstr$, daystr$, istr$
  
  Open lblWDM(1).Caption & ".pcp" For Output As #1
  'start writing values
  Call J2Date(PrecTs.dates.Summary.SJDay, dates)
  istr = dates(0) & ATCformat(dates(1), "00") & ATCformat(dates(2), "00")
  Print #1, istr
  For i = 1 To PrecTs.dates.Summary.NVALS
    r1 = PrecTs.Value(i) 'ConvertIn2MM(PrecTs.Value(i))
    Print #1, NumFmted(r1, 5, 1)
  Next i
  Close #1
  
  Open lblWDM(1).Caption & ".tmp" For Output As #1
  'start writing values
  Call J2Date(PrecTs.dates.Summary.SJDay, dates)
  istr = dates(0) & ATCformat(dates(1), "00") & ATCformat(dates(2), "00")
  Print #1, istr
  For i = 1 To PrecTs.dates.Summary.NVALS
    r1 = MaxTempTs.Value(i) 'ConvertF2C(MaxTempTs.Value(i))
    r2 = MinTempTs.Value(i) 'ConvertF2C(MinTempTs.Value(i))
    Print #1, NumFmted(r1, 5, 1); ","; NumFmted(r2, 5, 1)
  Next i
  Close #1
End Sub

'Private Sub OutputSWATDBF(PrecTs As ATCclsTserData, MaxTempTs As ATCclsTserData, MinTempTs As ATCclsTserData, MetName As String)
'  Dim dbf As clsDBF
'  Dim ts1 As ATCclsTserData
'  Dim ts2 As ATCclsTserData
'
'  Set dbf = FillDBF(MathColl("MULT", PrecTs, 25.4))
'  dbf.FieldName(2) = "PCP"
'  dbf.WriteDBF MetName & "_P"
'  Set dbf = Nothing
'
'  Set ts1 = MathColl("F2C", MaxTempTs): ts1.Attribset "Desc", "Max"
'  Set ts2 = MathColl("F2C", MinTempTs): ts2.Attribset "Desc", "Min"
'  Set dbf = FillDBF(ts1, ts2)
'  dbf.WriteDBF MetName & "_T"
'  Set dbf = Nothing
'
'End Sub

Private Function ComputeRelativeHumidity(AvgTempTs As ATCclsTserData, DewpointTs As ATCclsTserData) As ATCclsTserData
  Dim MathArgs As Collection
  Dim ActualVaporPressure As ATCclsTserData
  Dim SaturationVaporPressure As ATCclsTserData
  Dim temp As ATCclsTserData
  Dim denominator As ATCclsTserData

  'Vapor Pressure = 6.11 * 10 ^ (7.5 * CelsiusTemp / (237.7 + CelsiusTemp))
  
  'Find Actual Vapor Pressure
  Set MathArgs = New Collection: MathArgs.Add DewpointTs: MathArgs.Add 237.7
  Set denominator = MathColl("Add", MathArgs)
  
  Set MathArgs = New Collection: MathArgs.Add DewpointTs: MathArgs.Add 7.5
  Set temp = MathColl("Mult", MathArgs)

  Set MathArgs = New Collection: MathArgs.Add temp: MathArgs.Add denominator
  Set temp = MathColl("Div", MathArgs)

  Set MathArgs = New Collection: MathArgs.Add 10: MathArgs.Add temp
  Set temp = MathColl("Exp", MathArgs)

  Set MathArgs = New Collection: MathArgs.Add temp: MathArgs.Add 6.11
  Set ActualVaporPressure = MathColl("Mult", MathArgs)

  'Find Saturation Vapor Pressure
  Set MathArgs = New Collection: MathArgs.Add AvgTempTs: MathArgs.Add 237.7
  Set denominator = MathColl("Add", MathArgs)
  
  Set MathArgs = New Collection: MathArgs.Add AvgTempTs: MathArgs.Add 7.5
  Set temp = MathColl("Mult", MathArgs)

  Set MathArgs = New Collection: MathArgs.Add temp: MathArgs.Add denominator
  Set temp = MathColl("Div", MathArgs)

  Set MathArgs = New Collection: MathArgs.Add 10: MathArgs.Add temp
  Set temp = MathColl("Exp", MathArgs)

  Set MathArgs = New Collection: MathArgs.Add temp: MathArgs.Add 6.11
  Set SaturationVaporPressure = MathColl("Mult", MathArgs)

  'RelativeHumidity = ActualVaporPressure / SaturationVaporPressure
  Set MathArgs = New Collection: MathArgs.Add ActualVaporPressure: MathArgs.Add SaturationVaporPressure
  Set ComputeRelativeHumidity = MathColl("Div", MathArgs)

  'This still needs to be multiplied by 100 to get percent

End Function

'Pass in any number of ATCclsTserData, all with same dates and nVals
'Creates new DBF with first field as DATE (yyyyMMdd)
'Other fields are named by DESC attribute and populated with values of arguments
Private Function FillDBF(ParamArray datasets()) As clsDBF
  Dim lTs() As ATCclsTserData 'Indexed starting at 2 to align with corresponding DBF column
  Dim NVALS As Long
  Dim dates(5) As Long
  Dim ValueIndex As Long
  Dim DataSetIndex As Long
  Dim dbf As New clsDBF
  Dim param As Variant
  
  'First figure out nVals. We should also check for matching dates, but we don't yet
  DataSetIndex = 1
  For Each param In datasets
    DataSetIndex = DataSetIndex + 1
    ReDim Preserve lTs(DataSetIndex)
    Set lTs(DataSetIndex) = param
    If NVALS = 0 Then
      NVALS = lTs(DataSetIndex).dates.Summary.NVALS
    ElseIf NVALS <> lTs(DataSetIndex).dates.Summary.NVALS Then
      MsgBox "Different number of values: " & NVALS & " vs " & lTs(DataSetIndex).dates.Summary.NVALS, vbOKOnly, "WriteDBF"
      Exit Function
    End If
  Next
  dbf.numFields = DataSetIndex
  dbf.NumRecords = NVALS
  dbf.fieldName(1) = "DATE"
  dbf.FieldLength(1) = 8
  dbf.FieldType(1) = "D"
  For DataSetIndex = 2 To dbf.numFields
    dbf.fieldName(DataSetIndex) = lTs(DataSetIndex).Attrib("DESC")
    dbf.FieldLength(DataSetIndex) = 8
    dbf.FieldType(DataSetIndex) = "N"
    dbf.FieldDecimalCount(DataSetIndex) = 3
  Next
  dbf.InitData
  For ValueIndex = 1 To NVALS
    dbf.CurrentRecord = ValueIndex
    J2Date lTs(2).dates.Value(ValueIndex) - 1, dates
    dbf.Value(1) = dates(0) & Format(dates(1), "00") & Format(dates(2), "00")
    For DataSetIndex = 2 To dbf.numFields
      dbf.Value(DataSetIndex) = NumFmted(lTs(DataSetIndex).Value(ValueIndex), 8, 3)
    Next
  Next
  Set FillDBF = dbf
End Function

Private Sub opnGWLF_Click()
  cmdFile(2).Enabled = False
  lblFile(2).Enabled = False
  lblWDM(2).Enabled = False
End Sub

Private Sub opnOldSWAT_Click()
  cmdFile(2).Enabled = False
  lblFile(2).Enabled = False
  lblWDM(2).Enabled = False
End Sub

Private Sub opnSWAT_Click()
  cmdFile(2).Enabled = True
  lblFile(2).Enabled = True
  lblWDM(2).Enabled = True
End Sub

