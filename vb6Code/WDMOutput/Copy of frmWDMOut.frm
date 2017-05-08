VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.7#0"; "ATCoCtl.ocx"
Begin VB.Form frmWDMOut 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "WDM Output"
   ClientHeight    =   4836
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   8028
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmWDMOut.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4836
   ScaleWidth      =   8028
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   372
      Left            =   3000
      TabIndex        =   25
      Top             =   4320
      Width           =   2052
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   1200
         TabIndex        =   27
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "&OK"
         Default         =   -1  'True
         Height          =   375
         Left            =   0
         TabIndex        =   26
         Top             =   0
         Width           =   852
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
         _ExtentX        =   1080
         _ExtentY        =   445
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
         _ExtentX        =   1080
         _ExtentY        =   445
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
         _ExtentX        =   1503
         _ExtentY        =   445
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
         _ExtentX        =   1503
         _ExtentY        =   445
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
         Height          =   1392
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
      _ExtentX        =   699
      _ExtentY        =   699
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

Private Sub cmdFile_Click(Index As Integer)
  Dim iwdm&, i&, s$, f$, fun&, numMetSeg&, arrayMetSegs$(), wid$
  Dim tmetseg, lMetDetails$()
  
  Select Case Index
    Case 0 'met wdm file

      If lblWDM(0).Caption <> "<none>" Then
        Call MsgBox("Only one Met WDM file may be opened.", _
                    vbOKOnly, "WDM Output Problem")
      Else
        If Dir("\basins\data\met_data", vbDirectory) = "met_data" Then
          ChDir "\basins\data\met_data"
        End If
        CDFile.flags = &H1806&
        CDFile.Filter = "WDM files (*.wdm)|*.wdm"
        CDFile.filename = "*.wdm"
        CDFile.DialogTitle = "Select Met WDM File"
        On Error GoTo 40
        CDFile.CancelError = True
        CDFile.ShowOpen
        f = CDFile.filename
        lblWDM(0).Caption = f
        OpenWDMFile f, fun
        If fun < 1 Then
          Call MsgBox("Problem opening the Met WDM file.", _
                      vbOKOnly, "WDM Output Problem")
        Else
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
      End If
40        'continue here on cancel
    Case 1 'output file
      If Dir("\basins\modelout", vbDirectory) = "modelout" Then
        ChDir "\basins\modelout"
      End If
      CDFile.flags = &H8806&
      CDFile.Filter = "Output File (*.*)"
      CDFile.filename = "*.*"
      CDFile.DialogTitle = "Select Output Weather File"
      On Error GoTo 50
      CDFile.CancelError = True
      CDFile.ShowSave
      lblWDM(1).Caption = CDFile.filename
50        'continue here on cancel
    Case 2
      CDFile.flags = &H1806&
      CDFile.Filter = "WDM files (*.dbf)|*.dbf"
      CDFile.filename = "WDM.dbf"
      CDFile.DialogTitle = "Select WDM.dbf File"
      On Error GoTo 60
      CDFile.CancelError = True
      CDFile.ShowOpen
      If Len(CDFile.filename) > 0 Then
        If Len(Dir(CDFile.filename)) > 0 Then lblWDM(2).Caption = CDFile.filename
      End If
60
  End Select
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOk_Click()
  Dim i&, outwdm$, csdat&(5), cedat&(5)
  Dim lTs As Collection 'of atcotimser
  Dim ots As Collection 'of atcotimser
  Dim PrecTs As ATCclsTserData
  Dim AvgTempTs As ATCclsTserData
  Dim MaxTempTs As ATCclsTserData
  Dim MinTempTs As ATCclsTserData
  Dim SolarTs As ATCclsTserData
  Dim WindTs As ATCclsTserData
  Dim HumidTs
  Dim MetIndex As Long
  Dim MetName As String
  Dim MetLat As Single
  Dim MetLon As Single
  Dim MetEle As Single
  Dim WritingDBF As Boolean
  Dim dbf(2) As clsDBF
  Dim dbfindex As Integer
  Dim dataDBF As clsDBF
  Dim WDMDBF As clsDBF
  Dim MathArgs As Collection
  
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
        lstMet.Selected(1) = True
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
      WDMDBF.OpenDBF lblWDM(2)
      For dbfindex = 0 To 3
        Set dbf(dbfindex) = New clsDBF
        With dbf(dbfindex)
          .NumRecords = lstMet.SelCount
          .NumFields = 5
          .FieldLength(1) = 4: .FieldType(1) = "N": .FieldName(1) = "ID"
          .FieldLength(2) = 8: .FieldType(2) = "C": .FieldName(2) = "NAME"
          .FieldLength(3) = 8: .FieldType(3) = "N": .FieldName(3) = "LAT"
          .FieldLength(4) = 8: .FieldType(4) = "N": .FieldName(4) = "LONG"
          .FieldLength(5) = 8: .FieldType(5) = "N": .FieldName(5) = "ELEVATION"
          .InitData
          .CurrentRecord = 1
        End With
      Next
    End If
    For MetIndex = 1 To lstMet.ListCount
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
        
        Set PrecTs = Nothing
        Set AvgTempTs = Nothing
        Set MaxTempTs = Nothing
        Set MinTempTs = Nothing
        'get precip data
        Call FindTimSer("OBSERVED", MetName, "PREC", lTs)
        If lTs.Count > 0 Then
          Set ots = FillTimSerDaily(lTs, csdat, cedat, 1)
          Set PrecTs = ots(1)
        End If
        If PrecTs Is Nothing Then
          MsgBox "Required time series precipitation missing.", vbOKOnly, "WDM Output Problem"
        Else
          Call FindTimSer("OBSERVED", MetName, "ATEM", lTs)
          If lTs.Count = 0 Then
            MsgBox "Required time series ATEM missing.", vbOKOnly, "WDM Output Problem"
          ElseIf opnGWLF.Value Then 'gwlf -- get avg temp data
            Set ots = FillTimSerDaily(lTs, csdat, cedat, 0)
            Set AvgTempTs = ots(1)
            Call OutputGWLF(PrecTs, AvgTempTs)
          Else 'swat -- get min/max temp data
            Set MathArgs = New Collection: MathArgs.Add PrecTs: MathArgs.Add 25.4
            Set PrecTs = MathColl("MULT", MathArgs)
            PrecTs.Attribset "DESC", "PCP"
            
            Set ots = FillTimSerDaily(lTs, csdat, cedat, 2)
            Set MathArgs = New Collection: MathArgs.Add ots(1)
            Set MaxTempTs = MathColl("F2C", MathArgs)
            MaxTempTs.Attribset "DESC", "Max"
            
            Set ots = FillTimSerDaily(lTs, csdat, cedat, 3)
            Set MathArgs = New Collection: MathArgs.Add ots(1)
            Set MinTempTs = MathColl("F2C", MathArgs)
            MinTempTs.Attribset "DESC", "Min"
              
            If WritingDBF Then
              MetName = Left(FilenameOnly(lblWDM(0)), 2) & Right(MetName, 4)
              For dbfindex = 0 To 2
                With dbf(dbfindex)
                  .Value(1) = Right(PrecTs.attrib("Location"), 4)
                  .Value(3) = MetLat
                  .Value(4) = MetLon
                  .Value(5) = MetEle
                  .CurrentRecord = .CurrentRecord + 1
                End With
              Next
              
              dbf(0).Value(2) = MetName & "_P"
              Set dataDBF = FillDBF(PrecTs)
              dataDBF.WriteDBF dbf(0).Value(2)
              Set dataDBF = Nothing
              
              dbf(1).Value(2) = MetName & "_T"
              Set dataDBF = FillDBF(MaxTempTs, MinTempTs)
              dataDBF.WriteDBF dbf(1).Value(2)
              Set dataDBF = Nothing
                            
'              dbf(2).Value(2) = MetName & "_S"
'              Set dataDBF = FillDBF(SolarTs, WindTs, HumidTs)
'              dataDBF.WriteDBF dbf(2).Value(2)
'              Set dataDBF = Nothing
                            
            Else
              Call OutputSWAT(PrecTs, MaxTempTs, MinTempTs)
            End If
          End If
        End If
      End If
    Next
    If WritingDBF Then
      MetName = FilenameNoExt(lblWDM(0))
      dbf(0).WriteDBF MetName & "_PREC.dbf"
      dbf(1).WriteDBF MetName & "_ATEM.dbf"
      dbf(2).WriteDBF MetName & "_SLR_WND_HMD.dbf"
    End If
    Me.MousePointer = vbNormal
    Unload Me
  End If
End Sub

Private Sub Form_Load()
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
  
  ProjectWDMFile.filename = Name
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
    loc = lTs(i).header.loc
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
      dsn = lTs(i).header.ID
      Call J2Date(sj, sdat)
      Call J2Date(ej, edat)
      Call timcnv(edat)
      lMetDetails(numMetSeg - 1) = CStr(dsn) & "," & _
        CStr(sdat(0)) & "," & CStr(sdat(1)) & "," & CStr(sdat(2)) & "," & CStr(sdat(3)) & "," & CStr(sdat(4)) & "," & CStr(sdat(5)) & "," & _
        CStr(edat(0)) & "," & CStr(edat(1)) & "," & CStr(edat(2)) & "," & CStr(edat(3)) & "," & CStr(edat(4)) & "," & CStr(edat(5))
    End If
  Next i

End Sub

Public Sub FindTimSer(sen$, loc$, Con$, lTs As Collection)
  Dim dsn&, i&, j&, k&, l&, s$, GRPSIZ&, r!, imatch%
  Dim vTserFile As Variant, curClsTserFile As ATCclsTserFile, lds As ATTimSerDateSummary
  'Dim t As ATCclsTserData
  'dim newTs As ATTimSer

  Set lTs = Nothing
  Set lTs = New Collection
  For Each vTserFile In TserFiles.Active
    Set curClsTserFile = vTserFile.obj
    For j = 1 To curClsTserFile.DataCount
      If (sen = curClsTserFile.Data(j).header.sen Or Len(Trim(sen)) = 0) And _
         (loc = curClsTserFile.Data(j).header.loc Or Len(Trim(loc)) = 0) And _
         (Con = curClsTserFile.Data(j).header.Con Or Len(Trim(Con)) = 0) Then 'need this timser
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
    fraYear.Left = lstMet.Left + lstMet.Width = 108
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
        r1 = ConvertF2C(AvgTempTs.Value(counter + j))
        r2 = ConvertIn2MM(PrecTs.Value(counter + j)) / 10#
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
  dbf.NumFields = DataSetIndex
  dbf.NumRecords = NVALS
  dbf.FieldName(1) = "DATE"
  dbf.FieldLength(1) = 8
  For DataSetIndex = 2 To dbf.NumFields
    dbf.FieldLength(DataSetIndex) = 8
    dbf.FieldName(DataSetIndex) = lTs(DataSetIndex).attrib("DESC")
  Next
  dbf.InitData
  For ValueIndex = 1 To NVALS
    dbf.CurrentRecord = ValueIndex
    J2Date lTs(2).dates.Value(ValueIndex), dates
    dbf.Value(1) = dates(0) & Format(dates(1), "00") & Format(dates(2), "00")
    For DataSetIndex = 2 To dbf.NumFields
      dbf.Value(ValueIndex) = NumFmted(lTs(DataSetIndex).Value(ValueIndex), 5, 1)
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
