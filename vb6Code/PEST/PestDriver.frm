VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "Comdlg32.ocx"
Begin VB.Form frmPestDriver 
   Caption         =   "BASINS-Pest Driver"
   ClientHeight    =   2430
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5340
   Icon            =   "PestDriver.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2430
   ScaleWidth      =   5340
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkAUI 
      Caption         =   "Automatic User Intervention"
      Height          =   255
      Left            =   120
      TabIndex        =   16
      Top             =   2040
      Width           =   2895
   End
   Begin VB.ComboBox cboSimDSNs 
      Height          =   315
      Left            =   4320
      Style           =   2  'Dropdown List
      TabIndex        =   15
      Top             =   1560
      Width           =   855
   End
   Begin VB.TextBox txtObsDSN 
      Height          =   285
      Left            =   1320
      TabIndex        =   13
      Top             =   1560
      Width           =   615
   End
   Begin VB.CommandButton cmdRunPest 
      Caption         =   "Run PEST"
      Height          =   375
      Left            =   3240
      TabIndex        =   11
      Top             =   1920
      Width           =   1335
   End
   Begin VB.CommandButton cmdUCIOpen 
      Caption         =   "Open UCI File"
      Height          =   375
      Left            =   120
      TabIndex        =   10
      Top             =   240
      Width           =   1335
   End
   Begin VB.CommandButton cmdPar2ParWrite 
      Caption         =   "Write PAR2PAR"
      Height          =   255
      Left            =   5520
      TabIndex        =   9
      Top             =   4440
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdPar2ParOpen 
      Caption         =   "Open PAR2PAR"
      Height          =   255
      Left            =   5520
      TabIndex        =   8
      Top             =   120
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdGroupWrite 
      Caption         =   "Write Group File"
      Height          =   255
      Left            =   1920
      TabIndex        =   7
      Top             =   4440
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdGroupOpen 
      Caption         =   "Open Group File"
      Height          =   255
      Left            =   2040
      TabIndex        =   6
      Top             =   120
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdTSPWrite 
      Caption         =   "Write TSPROC File"
      Height          =   255
      Left            =   3720
      TabIndex        =   5
      Top             =   4440
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdParmWrite 
      Caption         =   "Write Parm File"
      Height          =   255
      Left            =   120
      TabIndex        =   4
      Top             =   4440
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdTSPOpen 
      Caption         =   "Open TSPROC File"
      Height          =   255
      Left            =   3840
      TabIndex        =   3
      Top             =   120
      Visible         =   0   'False
      Width           =   1575
   End
   Begin MSComDlg.CommonDialog cdlFile 
      Left            =   0
      Top             =   240
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      DefaultExt      =   "*.dat"
      DialogTitle     =   "Parm Data File Name"
      Filter          =   "*.dat"
   End
   Begin VB.CommandButton cmdParmOpen 
      Caption         =   "Open Parm File"
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.Label Label4 
      Caption         =   "Available simulated DSNS:"
      Height          =   255
      Left            =   2400
      TabIndex        =   14
      Top             =   1560
      Width           =   2055
   End
   Begin VB.Label Label3 
      Caption         =   "Observed DSN:"
      Height          =   255
      Left            =   120
      TabIndex        =   12
      Top             =   1560
      Width           =   1335
   End
   Begin VB.Label Label2 
      Height          =   735
      Left            =   120
      TabIndex        =   2
      Top             =   960
      Width           =   6975
   End
   Begin VB.Label Label1 
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   720
      Width           =   6135
   End
End
Attribute VB_Name = "frmPestDriver"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Public W_HSPFMSGMDB As String
Public W_HSPFMSGWDM As String
Public W_STARTERPATH As String
Public myUci As HspfUci
Public myMsg As HspfMsg
Public MessageUnit As Long

Dim PData As clsPestParmFile
Dim GData As clsPestGroupFile
Dim TSPData As clsPestTSPROC
Dim P2PData As clsPestPAR2PAR
Dim Pest As clsPest

Private Sub cmdGroupOpen_Click()
  Dim lpd As Variant, str As String
  Dim lColl As FastCollection

  cdlFile.DialogTitle = "Open Group Data file"
  cdlFile.ShowOpen
  If Len(cdlFile.filename) > 0 Then
    Set GData = New clsPestGroupFile
    GData.filename = cdlFile.filename
    Label1.Caption = cdlFile.filename
    Set lColl = GData.GroupData(1).valid("Inctype")
    str = "Valid IncType options are: "
    For Each lpd In lColl
      str = str & lpd & ", "
    Next
    Label2.Caption = Left(str, Len(str) - 2) & vbCrLf
    Set lColl = GData.GroupData(1).valid("forcen")
    str = "Valid ForCen options are: "
    For Each lpd In lColl
      str = str & lpd & ", "
    Next
    Label2.Caption = Label2.Caption & Left(str, Len(str) - 2) & vbCrLf
    Set lColl = GData.GroupData(1).valid("dermeth")
    str = "Valid DerMeth options are: "
    For Each lpd In lColl
      str = str & lpd & ", "
    Next
    Label2.Caption = Label2.Caption & Left(str, Len(str) - 2) & vbCrLf
    For Each lpd In GData.GroupData
      str = lpd.Name & ", " & lpd.IncType & ", " & lpd.DerInc & ", " & _
            lpd.DILBnd & ", " & lpd.ForCen & ", " & _
            lpd.DIMult & ", " & lpd.DerMeth & vbCrLf
      Label2.Caption = Label2.Caption & str
    Next
  End If

End Sub

Private Sub cmdGroupWrite_Click()
  Dim lgd As Variant, cGD As clsPestGroupData
  Dim lGData As clsPestGroupFile
  Dim str As String, FUnit As Integer

  cdlFile.DialogTitle = "Save Group data to file"
  cdlFile.ShowOpen
  If Len(cdlFile.filename) > 0 Then
    'make copy to text property "Let"s
    Set lGData = New clsPestGroupFile
    For Each lgd In GData.GroupData
      Set cGD = New clsPestGroupData
      cGD.Name = lgd.Name
      cGD.IncType = lgd.IncType
      cGD.DerInc = lgd.DerInc
      cGD.DILBnd = lgd.DILBnd
      cGD.ForCen = lgd.ForCen
      cGD.DIMult = lgd.DIMult
      cGD.DerMeth = lgd.DerMeth
      lGData.GroupData.Add cGD
    Next
    str = lGData.WriteFileAsString
    FUnit = FreeFile(0)
    Open cdlFile.filename For Output As #FUnit
    Print #FUnit, str
    Close #FUnit
  End If

End Sub

Private Sub cmdPar2ParOpen_Click()
  
  cdlFile.DialogTitle = "Open PAR2PAR Data file"
  cdlFile.ShowOpen
  If Len(cdlFile.filename) > 0 Then
    Set P2PData = New clsPestPAR2PAR
    P2PData.filename = cdlFile.filename
    Label1.Caption = cdlFile.filename
    Label2.Caption = "PAR2PAR Data retrieved"
  End If

End Sub

Private Sub cmdPar2ParWrite_Click()
  Dim str As String, FUnit As Integer
  Dim cP2PD As clsPestPAR2PAR
  Dim v As Variant, lColl As FastCollection

  cdlFile.DialogTitle = "Save PAR2PAR data to file"
  cdlFile.ShowOpen
  If Len(cdlFile.filename) > 0 Then
    str = "Valid Precision options are: "
    Set lColl = P2PData.valid("precis")
    For Each v In lColl
      str = str & v & ", "
    Next
    Label2 = Left(str, Len(str) - 2) & vbCrLf
    str = "Valid Decimal Point options are: "
    Set lColl = P2PData.valid("dpoint")
    For Each v In lColl
      str = str & v & ", "
    Next
    Label2 = Label2 & Left(str, Len(str) - 2) & vbCrLf
    Set cP2PD = New clsPestPAR2PAR
    cP2PD.ParmData = P2PData.ParmData
    cP2PD.Templates = P2PData.Templates
    cP2PD.Precis = P2PData.Precis
    cP2PD.DPoint = P2PData.DPoint
    str = cP2PD.WriteFileAsString(True)
  End If
  FUnit = FreeFile(0)
  Open cdlFile.filename For Output As #FUnit
  Print #FUnit, str
  Close #FUnit

End Sub

Private Sub cmdParmOpen_Click()
  Dim lpd As Variant, str As String
  Dim lColl As FastCollection

  cdlFile.DialogTitle = "Open Parm Data file"
  cdlFile.ShowOpen
  If Len(cdlFile.filename) > 0 Then
    Set PData = New clsPestParmFile
    PData.filename = cdlFile.filename
    Label1.Caption = cdlFile.filename
    Set lColl = PData.ParmData(1).valid("Trans")
    str = "Valid Trans options are: "
    For Each lpd In lColl
      str = str & lpd & ", "
    Next
    Label2.Caption = Left(str, Len(str) - 2) & vbCrLf
    Set lColl = PData.ParmData(1).valid("Limtype")
    str = "Valid Limtype options are: "
    For Each lpd In lColl
      str = str & lpd & ", "
    Next
    Label2.Caption = Label2.Caption & Left(str, Len(str) - 2) & vbCrLf
    For Each lpd In PData.ParmData
      str = lpd.Name & ", " & lpd.Trans & ", " & lpd.LimType & ", " & _
            lpd.InitVal & ", " & lpd.LBnd & ", " & lpd.UBnd & ", " & _
            lpd.Group & ", " & lpd.Mult & ", " & lpd.Offset & vbCrLf
      Label2.Caption = Label2.Caption & str
    Next
  End If
End Sub

Sub Main()
  frmPestDriver.Show
End Sub

Private Sub cmdParmWrite_Click()
  Dim lpd As Variant, cPD As clsPestParmData
  Dim lPData As clsPestParmFile
  Dim str As String, FUnit As Integer

  cdlFile.DialogTitle = "Save Parm data to file"
  cdlFile.ShowOpen
  If Len(cdlFile.filename) > 0 Then
    'make copy to text property "Let"s
    Set lPData = New clsPestParmFile
    For Each lpd In PData.ParmData
      Set cPD = New clsPestParmData
      cPD.Name = lpd.Name
      cPD.Trans = lpd.Trans
      cPD.LimType = lpd.LimType
      cPD.InitVal = lpd.InitVal
      cPD.LBnd = lpd.LBnd
      cPD.UBnd = lpd.UBnd
      cPD.Group = lpd.Group
      cPD.Mult = lpd.Mult
      cPD.Offset = lpd.Offset
      lPData.ParmData.Add cPD
    Next
    str = lPData.WriteFileAsString
    FUnit = FreeFile(0)
    Open cdlFile.filename For Output As #FUnit
    Print #FUnit, str
    Close #FUnit
  End If
End Sub

Private Sub cmdRunPest_Click()
  If IsNumeric(txtObsDSN.Text) And IsNumeric(cboSimDSNs.List(cboSimDSNs.ListIndex)) Then
    Pest.IPC = IPC
    If chkAUI.Value = 1 Then Pest.AUI = True
    Pest.InitBasinsPest CLng(txtObsDSN.Text), CLng(cboSimDSNs.List(cboSimDSNs.ListIndex))
    Label2.Caption = Label2.Caption & vbCrLf & "Wrote UCI template file"
    Pest.Run
    Label2.Caption = "Completed PEST run"
  Else
    Label2.Caption = "NEED TO SPECIFY DSNS BEFORE RUNNING PEST!"
  End If
End Sub

Private Sub cmdTSPOpen_Click()

  cdlFile.DialogTitle = "Open TSPROC Data file"
  cdlFile.ShowOpen
  If Len(cdlFile.filename) > 0 Then
    Set TSPData = New clsPestTSPROC
    TSPData.filename = cdlFile.filename
    Label1.Caption = cdlFile.filename
    Label2.Caption = "TSPROC Data retrieved"
  End If
End Sub

Private Sub cmdTSPWrite_Click()
  Dim str As String, FUnit As Integer
  Dim cTSPD As clsPestTSPROC
  Dim v As Variant, lColl As FastCollection
  Dim lEE As clsPestTSPErase, lExT As clsPestTSPExceedence
  Dim lGP As clsPestTSPGetPLTGEN
  Dim lGS As clsPestTSPGetSSF
  Dim lGW As clsPestTSPGetWDM
  Dim lLO As clsPestTSPListOutput
  Dim lNT As clsPestTSPNewTimeBase
  Dim lRS As clsPestTSPReduceSpan
  Dim lSC As clsPestTSPSeriesClean
  Dim lsd As clsPestTSPSeriesDisplace
  Dim lSE As clsPestTSPSeriesEquation
  Dim lSS As clsPestTSPSeriesStats
  Dim lVC As clsPestTSPVolumeCalc

  cdlFile.DialogTitle = "Save TSPROC data to file"
  cdlFile.ShowOpen
  If Len(cdlFile.filename) > 0 Then
    Label2 = ""
    Set cTSPD = New clsPestTSPROC
    cTSPD.Settings.Comments = TSPData.Settings.Comments
    cTSPD.Settings.Context = TSPData.Settings.Context
    cTSPD.Settings.ContextCom = TSPData.Settings.ContextCom
    cTSPD.Settings.DateFormat = TSPData.Settings.DateFormat
    cTSPD.Settings.DateFormatCom = TSPData.Settings.DateFormatCom
    cTSPD.Settings.EndComments = TSPData.Settings.EndComments
    If TSPData.EraseEntity.Count > 0 Then
      For Each v In TSPData.EraseEntity
        Set lEE = New clsPestTSPErase
        lEE.Comments = v.Comments
        lEE.Context = v.Context
        lEE.ContextCom = v.ContextCom
        lEE.SeriesName = v.SeriesName
        lEE.SeriesNameCom = v.SeriesNameCom
        lEE.STableName = v.STableName
        lEE.STableNameCom = v.STableNameCom
        lEE.VTableName = v.VTableName
        lEE.VTableNameCom = v.VTableNameCom
        lEE.ETableName = v.ETableName
        lEE.ETableNameCom = v.ETableNameCom
        cTSPD.EraseEntity.Add lEE
      Next
    End If
    If TSPData.ExceedenceTime.Count > 0 Then
      str = "Valid Exceedence Time Units options are: "
      Set lColl = TSPData.ExceedenceTime(1).valid("exceedence_time_units")
      For Each v In lColl
        str = str & v & ", "
      Next
      Label2 = Label2 & Left(str, Len(str) - 2) & vbCrLf
      str = "Valid Exceedence Under/Over options are: "
      Set lColl = TSPData.ExceedenceTime(1).valid("under_over")
      For Each v In lColl
        str = str & v & ", "
      Next
      Label2 = Label2 & Left(str, Len(str) - 2) & vbCrLf
      For Each v In TSPData.ExceedenceTime
        Set lExT = New clsPestTSPExceedence
        lExT.Comments = v.Comments
        lExT.Context = v.Context
        lExT.ContextCom = v.ContextCom
        lExT.SeriesName = v.SeriesName
        lExT.SeriesNameCom = v.SeriesNameCom
        lExT.ETableName = v.ETableName
        lExT.ETableNameCom = v.ETableNameCom
        lExT.TimeUnits = v.TimeUnits
        lExT.TimeUnitsCom = v.TimeUnitsCom
        lExT.UnderOver = v.UnderOver
        lExT.UnderOverCom = v.UnderOverCom
        lExT.Flow = v.Flow
        lExT.FlowCom = v.FlowCom
        lExT.Delay = v.Delay
        lExT.DelayCom = v.DelayCom
        cTSPD.ExceedenceTime.Add lExT
      Next
    End If
    If TSPData.GetPLTGEN.Count > 0 Then
      For Each v In TSPData.GetPLTGEN
        Set lGP = New clsPestTSPGetPLTGEN
        lGP.Comments = v.Comments
        lGP.Context = v.Context
        lGP.ContextCom = v.ContextCom
        lGP.PLTGENFile = v.PLTGENFile
        lGP.PLTGENFileCom = v.PLTGENFileCom
        lGP.PLTGENLabel = v.PLTGENLabel
        lGP.PLTGENLabelCom = v.PLTGENLabelCom
        lGP.SeriesName = v.SeriesName
        lGP.SeriesNameCom = v.SeriesNameCom
        lGP.DATE1 = v.DATE1
        lGP.Date1Com = v.Date1Com
        lGP.Time1 = v.Time1
        lGP.Time1Com = v.Time1Com
        lGP.DATE2 = v.DATE2
        lGP.Date2Com = v.Date2Com
        lGP.Time2 = v.Time2
        lGP.Time2Com = v.Time2Com
        cTSPD.GetPLTGEN.Add lGP
      Next
    End If
    If TSPData.GetSSF.Count > 0 Then
      For Each v In TSPData.GetSSF
        Set lGS = New clsPestTSPGetSSF
        lGS.Comments = v.Comments
        lGS.Context = v.Context
        lGS.ContextCom = v.ContextCom
        lGS.SSFFile = v.SSFFile
        lGS.SSFFileCom = v.SSFFileCom
        lGS.SSFSite = v.SSFSite
        lGS.SSFSiteCom = v.SSFSiteCom
        lGS.SeriesName = v.SeriesName
        lGS.SeriesNameCom = v.SeriesNameCom
        lGS.DATE1 = v.DATE1
        lGS.Date1Com = v.Date1Com
        lGS.Time1 = v.Time1
        lGS.Time1Com = v.Time1Com
        lGS.DATE2 = v.DATE2
        lGS.Date2Com = v.Date2Com
        lGS.Time2 = v.Time2
        lGS.Time2Com = v.Time2Com
        cTSPD.GetSSF.Add lGS
      Next
    End If
    If TSPData.GetWDM.Count > 0 Then
      For Each v In TSPData.GetWDM
        Set lGW = New clsPestTSPGetWDM
        lGW.Comments = v.Comments
        lGW.Context = v.Context
        lGW.ContextCom = v.ContextCom
        lGW.WDMFile = v.WDMFile
        lGW.WDMFileCom = v.WDMFileCom
        lGW.dsn = v.dsn
        lGW.DSNCom = v.DSNCom
        lGW.SeriesName = v.SeriesName
        lGW.SeriesNameCom = v.SeriesNameCom
        lGW.DATE1 = v.DATE1
        lGW.Date1Com = v.Date1Com
        lGW.Time1 = v.Time1
        lGW.Time1Com = v.Time1Com
        lGW.DATE2 = v.DATE2
        lGW.Date2Com = v.Date2Com
        lGW.Time2 = v.Time2
        lGW.Time2Com = v.Time2Com
        lGW.DefTime = v.DefTime
        lGW.DefTimeCom = v.DefTimeCom
        lGW.Filter = v.Filter
        lGW.FilterCom = v.FilterCom
        cTSPD.GetWDM.Add lGW
      Next
    End If
    If TSPData.ListOutput.Count > 0 Then
      str = "Valid ListOutput SeriesFormat options are: "
      Set lColl = TSPData.ListOutput(1).valid("series_format")
      For Each v In lColl
        str = str & v & ", "
      Next
      Label2 = Label2 & Left(str, Len(str) - 2) & vbCrLf
      For Each v In TSPData.ListOutput
        Set lLO = New clsPestTSPListOutput
        lLO.Comments = v.Comments
        lLO.Context = v.Context
        lLO.ContextCom = v.ContextCom
        lLO.ListFile = v.ListFile
        lLO.ListFileCom = v.ListFileCom
        lLO.SeriesName = v.SeriesName
        lLO.SeriesNameCom = v.SeriesNameCom
        lLO.SeriesFormat = v.SeriesFormat
        lLO.SeriesFormatCom = v.SeriesFormatCom
        lLO.STableName = v.STableName
        lLO.STableNameCom = v.STableNameCom
        lLO.VTableName = v.VTableName
        lLO.VTableNameCom = v.VTableNameCom
        lLO.ETableName = v.ETableName
        lLO.ETableNameCom = v.ETableNameCom
        cTSPD.ListOutput.Add lLO
      Next
    End If
    If TSPData.NewTimeBase.Count > 0 Then
      For Each v In TSPData.NewTimeBase
        Set lNT = New clsPestTSPNewTimeBase
        lNT.Comments = v.Comments
        lNT.Context = v.Context
        lNT.ContextCom = v.ContextCom
        lNT.SeriesName = v.SeriesName
        lNT.SeriesNameCom = v.SeriesNameCom
        lNT.NewSeriesName = v.NewSeriesName
        lNT.NewSeriesNameCom = v.NewSeriesNameCom
        lNT.TBSeriesName = v.TBSeriesName
        lNT.TBSeriesNameCom = v.TBSeriesNameCom
        cTSPD.NewTimeBase.Add lNT
      Next
    End If
    If TSPData.GetWDM.Count > 0 Then
      For Each v In TSPData.ReduceSpan
        Set lRS = New clsPestTSPReduceSpan
        lRS.Comments = v.Comments
        lRS.Context = v.Context
        lRS.ContextCom = v.ContextCom
        lRS.SeriesName = v.SeriesName
        lRS.SeriesNameCom = v.SeriesNameCom
        lRS.NewSeriesName = v.NewSeriesName
        lRS.NewSeriesNameCom = v.NewSeriesNameCom
        lRS.DATE1 = v.DATE1
        lRS.Date1Com = v.Date1Com
        lRS.Time1 = v.Time1
        lRS.Time1Com = v.Time1Com
        lRS.DATE2 = v.DATE2
        lRS.Date2Com = v.Date2Com
        lRS.Time2 = v.Time2
        lRS.Time2Com = v.Time2Com
        cTSPD.ReduceSpan.Add lRS
      Next
    End If
    If TSPData.SeriesClean.Count > 0 Then
      For Each v In TSPData.SeriesClean
        Set lSC = New clsPestTSPSeriesClean
        lSC.Comments = v.Comments
        lSC.Context = v.Context
        lSC.ContextCom = v.ContextCom
        lSC.SeriesName = v.SeriesName
        lSC.SeriesNameCom = v.SeriesNameCom
        lSC.NewSeriesName = v.NewSeriesName
        lSC.NewSeriesNameCom = v.NewSeriesNameCom
        lSC.LEraseBnd = v.LEraseBnd
        lSC.LEraseBndCom = v.LEraseBndCom
        lSC.UEraseBnd = v.UEraseBnd
        lSC.UEraseBndCom = v.UEraseBndCom
        lSC.SubValue = v.SubValue
        lSC.SubValueCom = v.SubValueCom
        cTSPD.SeriesClean.Add lSC
      Next
    End If
    If TSPData.SeriesDisplace.Count > 0 Then
      For Each v In TSPData.SeriesDisplace
        Set lsd = New clsPestTSPSeriesDisplace
        lsd.Comments = v.Comments
        lsd.Context = v.Context
        lsd.ContextCom = v.ContextCom
        lsd.SeriesName = v.SeriesName
        lsd.SeriesNameCom = v.SeriesNameCom
        lsd.NewSeriesName = v.NewSeriesName
        lsd.NewSeriesNameCom = v.NewSeriesNameCom
        lsd.LagIncrement = v.LagIncrement
        lsd.LagIncrementCom = v.LagIncrementCom
        lsd.FillValue = v.FillValue
        lsd.FillValueCom = v.FillValueCom
        cTSPD.SeriesDisplace.Add lsd
      Next
    End If
    If TSPData.SeriesEquation.Count > 0 Then
      For Each v In TSPData.SeriesEquation
        Set lSE = New clsPestTSPSeriesEquation
        lSE.Comments = v.Comments
        lSE.Context = v.Context
        lSE.ContextCom = v.ContextCom
        lSE.NewSeriesName = v.NewSeriesName
        lSE.NewSeriesNameCom = v.NewSeriesNameCom
        lSE.Equation = v.Equation
        lSE.EquationCom = v.EquationCom
        cTSPD.SeriesEquation.Add lSE
      Next
    End If
    If TSPData.SeriesStats.Count > 0 Then
      For Each v In TSPData.SeriesStats
        Set lSS = New clsPestTSPSeriesStats
        lSS.Comments = v.Comments
        lSS.Context = v.Context
        lSS.ContextCom = v.ContextCom
        lSS.SeriesName = v.SeriesName
        lSS.SeriesNameCom = v.SeriesNameCom
        lSS.NewSTableName = v.NewSTableName
        lSS.NewSTableNameCom = v.NewSTableNameCom
        lSS.SumOpt = v.SumOpt
        lSS.SumOptCom = v.SumOptCom
        lSS.MeanOpt = v.MeanOpt
        lSS.MeanOptCom = v.MeanOptCom
        lSS.StdDevOpt = v.StdDevOpt
        lSS.StdDevOptCom = v.StdDevOptCom
        lSS.MaxOpt = v.MaxOpt
        lSS.MaxOptCom = v.MaxOptCom
        lSS.MinOpt = v.MinOpt
        lSS.MinOptCom = v.MinOptCom
        lSS.LogOpt = v.LogOpt
        lSS.LogOptCom = v.LogOptCom
        lSS.Power = v.Power
        lSS.PowerCom = v.PowerCom
        lSS.DATE1 = v.DATE1
        lSS.Date1Com = v.Date1Com
        lSS.Time1 = v.Time1
        lSS.Time1Com = v.Time1Com
        lSS.DATE2 = v.DATE2
        lSS.Date2Com = v.Date2Com
        lSS.Time2 = v.Time2
        lSS.Time2Com = v.Time2Com
        cTSPD.SeriesStats.Add lSS
      Next
    End If
    If TSPData.VolumeCalc.Count > 0 Then
      str = "Valid Volume Calculation Time Units options are: "
      Set lColl = TSPData.VolumeCalc(1).valid("flow_time_units")
      For Each v In lColl
        str = str & v & ", "
      Next
      Label2 = Label2 & Left(str, Len(str) - 2) & vbCrLf
      For Each v In TSPData.VolumeCalc
        Set lVC = New clsPestTSPVolumeCalc
        lVC.Comments = v.Comments
        lVC.Context = v.Context
        lVC.ContextCom = v.ContextCom
        lVC.SeriesName = v.SeriesName
        lVC.SeriesNameCom = v.SeriesNameCom
        lVC.NewVTableName = v.NewVTableName
        lVC.NewVTableNameCom = v.NewVTableNameCom
        lVC.DateFileName = v.DateFileName
        lVC.DateFileNameCom = v.DateFileNameCom
        lVC.TimeUnits = v.TimeUnits
        lVC.TimeUnitsCom = v.TimeUnitsCom
        lVC.Factor = v.Factor
        lVC.FactorCom = v.FactorCom
        cTSPD.VolumeCalc.Add lVC
      Next
    End If
    If TSPData.WritePest.Context.Count > 0 Then
      cTSPD.WritePest.Comments = TSPData.WritePest.Comments
      cTSPD.WritePest.Context = TSPData.WritePest.Context
      cTSPD.WritePest.ContextCom = TSPData.WritePest.ContextCom
      cTSPD.WritePest.TemplateFile = TSPData.WritePest.TemplateFile
      cTSPD.WritePest.TemplateFileCom = TSPData.WritePest.TemplateFileCom
      cTSPD.WritePest.ModelInputFile = TSPData.WritePest.ModelInputFile
      cTSPD.WritePest.ModelInputFileCom = TSPData.WritePest.ModelInputFileCom
      cTSPD.WritePest.ParmDataFile = TSPData.WritePest.ParmDataFile
      cTSPD.WritePest.ParmDataFileCom = TSPData.WritePest.ParmDataFileCom
      cTSPD.WritePest.ParmGroupFile = TSPData.WritePest.ParmGroupFile
      cTSPD.WritePest.ParmGroupFileCom = TSPData.WritePest.ParmGroupFileCom
      cTSPD.WritePest.Series = TSPData.WritePest.Series
      cTSPD.WritePest.ControlFile = TSPData.WritePest.ControlFile
      cTSPD.WritePest.ControlFileCom = TSPData.WritePest.ControlFileCom
      cTSPD.WritePest.InstructFile = TSPData.WritePest.InstructFile
      cTSPD.WritePest.InstructFileCom = TSPData.WritePest.InstructFileCom
      cTSPD.WritePest.ModelCommandLine = TSPData.WritePest.ModelCommandLine
      cTSPD.WritePest.ModelCommandLineCom = TSPData.WritePest.ModelCommandLineCom
    End If
    str = cTSPD.WriteFileAsString
  End If
  FUnit = FreeFile(0)
  Open cdlFile.filename For Output As #FUnit
  Print #FUnit, str
  Close #FUnit
End Sub

Private Sub cmdUCIOpen_Click()
  Dim ReadOK As Boolean, EchoFile As String
  Dim lOper As HspfOperation
  Dim lconn As HspfConnection
  Dim ff As ATCoFindFile
  Dim lParmFile As New clsPestParmFile
  Dim lGroupFile As New clsPestGroupFile
  Dim filename As String
  Dim i As Long, j As Long, k As Long

  ChDir "C:\models\BasPest"
  W_STARTERPATH = ""
  Set IPC = New ATCoIPC
  With IPC
    .SendMonitorMessage "(OPEN Basins PEST)"
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
  'OpenDefaultUCI
  'IPC.SendMonitorMessage "(Successfully Read Default UCI)"
  
  Set myUci = New HspfUci
  myUci.HelpFile = App.HelpFile
  myUci.StarterPath = W_STARTERPATH
  Set myUci.Monitor = IPC
  myUci.MessageUnit = MessageUnit

  IPC.SendMonitorMessage "(BUTTON OUTPUT)"
  IPC.SendMonitorMessage "(BUTTON PAUSE)"
  IPC.SendMonitorMessage "(BUTTON CANCEL)"
  IPC.SendMonitorMessage "(CLOSE)"

  cdlFile.DialogTitle = "Open UCI file"
  cdlFile.Filter = "UCI files|*.uci"
  cdlFile.ShowOpen
  If Len(cdlFile.filename) > 0 Then
    Label1.Caption = cdlFile.filename
    myUci.ReadUci myMsg, cdlFile.filename, 0, ReadOK, EchoFile
    If ReadOK Then Label2.Caption = "UCI file retrieved"
    Set Pest = New clsPest
    Pest.Uci = myUci
    Pest.IPC = IPC
    Pest.ModelName = "HSPF"
    Pest.Edit
'    Set ff = New ATCoFindFile
'    ff.SetDialogProperties "Please locate starter PEST parameter file", "parm_start.dat"
'    ff.SetRegistryInfo "PestDriver", "Defaults", "PestParmStart"
'    lParmFile.filename = ff.GetName
'    Pest.ParmFile = lParmFile
'    ff.SetDialogProperties "Please locate starter PEST group file", "group_start.dat"
'    ff.SetRegistryInfo "PestDriver", "Defaults", "PestPStart"
'    lGroupFile.filename = ff.GetName
'    Pest.GroupFile = lGroupFile
'    For i = 1 To myUci.OpnSeqBlock.Opns.Count
'      Set lOper = myUci.OpnSeqBlock.Opn(i)
''      If loper.Name = "RCHRES" Then
''        If IsFlowLocation(loper.Name, loper.ID) Then
'          'this is an output flow location
'          For j = 1 To lOper.Targets.Count
'            Set lconn = lOper.Targets(j)
'            If Left(lconn.Target.volname, 3) = "WDM" And Trim(lconn.Target.member) = "FLOW" Then
'              cboSimDSNs.AddItem CStr(lconn.Target.VolId)
'            End If
'          Next j
''        End If
''      End If
'    Next i
  End If

End Sub

Private Sub Form_Load()
  W_HSPFMSGMDB = "hspfmsg.mdb"
  W_HSPFMSGWDM = "hspfmsg.wdm"

End Sub

Private Sub Form_Unload(Cancel As Integer)

  On Error Resume Next
  IPC.SendMonitorMessage "(EXIT)"

End Sub
