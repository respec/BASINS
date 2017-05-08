VERSION 5.00
Begin VB.Form frmWDMURead 
   Caption         =   "WDMUtil Data Initialization"
   ClientHeight    =   7536
   ClientLeft      =   2004
   ClientTop       =   1080
   ClientWidth     =   5604
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   31
   Icon            =   "WDMURead.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   7536
   ScaleWidth      =   5604
   Begin VB.Frame fraFileHeader 
      Caption         =   "File View"
      Height          =   2772
      Left            =   120
      TabIndex        =   23
      Top             =   480
      Width           =   5292
      Begin VB.TextBox txtFormat 
         Height          =   288
         Left            =   120
         TabIndex        =   2
         Top             =   2400
         Width           =   5052
      End
      Begin VB.TextBox txtHead 
         BackColor       =   &H00C0C0C0&
         Height          =   1092
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   3  'Both
         TabIndex        =   0
         Top             =   240
         Width           =   5052
      End
      Begin ATCoCtl.ATCoText atxHdr 
         Height          =   252
         Left            =   3000
         TabIndex        =   1
         Top             =   1560
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         DataType        =   0
         DefaultValue    =   0
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin VB.Label lblHdr 
         Caption         =   "Enter number of header lines (i.e. lines to skip before data starts):"
         Height          =   492
         Left            =   120
         TabIndex        =   29
         Top             =   1440
         Width           =   2892
      End
      Begin VB.Label lblFormat 
         Caption         =   "Enter format for reading dates and desired data column (See Help for information about specifying data formats)"
         Height          =   372
         Index           =   0
         Left            =   120
         TabIndex        =   28
         Top             =   1920
         Width           =   4932
      End
   End
   Begin VB.CommandButton cmdImport 
      Caption         =   "OK"
      Height          =   375
      Left            =   1440
      TabIndex        =   14
      Top             =   7080
      Width           =   972
   End
   Begin VB.Frame fraSpecs 
      Caption         =   "Data Set Specifications"
      Height          =   2172
      Left            =   120
      TabIndex        =   18
      Top             =   3360
      Width           =   5292
      Begin VB.ComboBox cboCons 
         Height          =   288
         Left            =   1920
         Style           =   2  'Dropdown List
         TabIndex        =   4
         Top             =   480
         Width           =   1452
      End
      Begin VB.ComboBox cboTU 
         Height          =   288
         ItemData        =   "WDMURead.frx":0442
         Left            =   1320
         List            =   "WDMURead.frx":0458
         Style           =   2  'Dropdown List
         TabIndex        =   8
         Top             =   1200
         Width           =   1812
      End
      Begin VB.TextBox txtDesc 
         Height          =   288
         Left            =   1320
         TabIndex        =   7
         Top             =   840
         Width           =   3732
      End
      Begin VB.TextBox txtCons 
         Height          =   288
         Left            =   1920
         TabIndex        =   5
         Top             =   480
         Width           =   1452
      End
      Begin VB.TextBox txtLoc 
         Height          =   288
         Left            =   3600
         TabIndex        =   6
         Top             =   480
         Width           =   1452
      End
      Begin VB.TextBox txtScen 
         Height          =   288
         Left            =   240
         TabIndex        =   3
         Text            =   "OBSERVED"
         Top             =   480
         Width           =   1452
      End
      Begin ATCoCtl.ATCoText atxMiss 
         Height          =   252
         Left            =   960
         TabIndex        =   10
         Top             =   1800
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         DataType        =   0
         DefaultValue    =   0
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxAcc 
         Height          =   252
         Left            =   3120
         TabIndex        =   11
         Top             =   1800
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         DataType        =   0
         DefaultValue    =   0
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxFill 
         Height          =   252
         Left            =   4440
         TabIndex        =   12
         Top             =   1800
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         DataType        =   0
         DefaultValue    =   0
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxTS 
         Height          =   252
         Left            =   4440
         TabIndex        =   9
         Top             =   1200
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         DataType        =   0
         DefaultValue    =   0
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin VB.Label lblTS 
         Caption         =   "Time Step:"
         Height          =   252
         Left            =   3480
         TabIndex        =   31
         Top             =   1200
         Width           =   972
      End
      Begin VB.Label lblTU 
         Caption         =   "Time Units:"
         Height          =   252
         Left            =   240
         TabIndex        =   30
         Top             =   1200
         Width           =   1092
      End
      Begin VB.Label lblFill 
         Caption         =   "Fill:"
         Height          =   252
         Left            =   4080
         TabIndex        =   27
         Top             =   1800
         Width           =   372
      End
      Begin VB.Label lblMiss 
         Caption         =   "Missing:"
         Height          =   252
         Left            =   240
         TabIndex        =   26
         Top             =   1800
         Width           =   732
      End
      Begin VB.Label lblAcc 
         Caption         =   "Accumulated:"
         Height          =   252
         Left            =   1920
         TabIndex        =   25
         Top             =   1800
         Width           =   1212
      End
      Begin VB.Label lblData 
         Caption         =   "Data Value Indicators:"
         Height          =   252
         Left            =   240
         TabIndex        =   24
         Top             =   1560
         Width           =   3132
      End
      Begin VB.Label lblCons 
         Alignment       =   2  'Center
         Caption         =   "Constituent"
         Height          =   252
         Left            =   1920
         TabIndex        =   22
         Top             =   240
         Width           =   1452
      End
      Begin VB.Label lblLoc 
         Alignment       =   2  'Center
         Caption         =   "Location"
         Height          =   252
         Left            =   3600
         TabIndex        =   21
         Top             =   240
         Width           =   1452
      End
      Begin VB.Label lblScen 
         Alignment       =   2  'Center
         Caption         =   "Scenario"
         Height          =   252
         Left            =   240
         TabIndex        =   20
         Top             =   240
         Width           =   1452
      End
      Begin VB.Label lblStanam 
         Caption         =   "Description:"
         Height          =   252
         Left            =   240
         TabIndex        =   19
         Top             =   840
         Width           =   1092
      End
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   3120
      TabIndex        =   15
      Top             =   7080
      Width           =   972
   End
   Begin ATCoCtl.ATCoDate ctlDates 
      Height          =   1356
      Left            =   120
      TabIndex        =   13
      Top             =   5640
      Width           =   5364
      _ExtentX        =   11578
      _ExtentY        =   2392
      TUnit           =   4
      TAggr           =   1
      TStep           =   1
      CurrE           =   35064
      CurrS           =   33970
      LimtE           =   36525
      LimtS           =   33239
      DispL           =   4
      LabelCurrentRange=   "Current"
      LabelMaxRange   =   "Available"
      TstepVisible    =   0   'False
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   372
      Left            =   1200
      TabIndex        =   17
      Top             =   120
      Width           =   4212
   End
   Begin VB.Label lblFiles 
      Caption         =   "NOAA File:"
      Height          =   252
      Left            =   120
      TabIndex        =   16
      Top             =   120
      Width           =   972
   End
End
Attribute VB_Name = "frmWDMURead"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim ts As Timser
Dim Precip As Boolean, NOAA As Boolean
Dim FName$, lsdat&(5), ledat&(5), dtype$, ElType$(), ElCnt&

Private Sub cmdCancel_Click()

  'cancel
  Unload frmWDMURead
  frmGenScn.SetFocus

End Sub

Private Sub cmdImport_Click()

  Dim i&, j&, ifl&, retcod&, fmt$, elstr&, elend&, tsfill!
  Dim fail As Boolean

  On Error GoTo errhandler

  fail = False
  If dtype = "SOD" Then txtCons.Text = "ALL" 'put dummy in cons field to pass check
  If Len(txtScen.Text) = 0 Or Len(txtCons.Text) = 0 Or Len(txtLoc.Text) = 0 Then
    'need S, L, C specs
    MsgBox "Please enter Scenario, Location, and Constituent names in the fields provided." & vbCrLf & "These names will be used to identify the data you are reading in.", vbExclamation, "WDMUtil Data Initialization Problem"
    fail = True
  ElseIf cboTU.ListIndex = 0 Then 'no time units specified
    MsgBox "Please select the time units of the data being read from the list provided.", vbExclamation, "WDMUtil Data Initialization Problem"
    fail = True
  End If

  If Not fail Then
    'import data
    MousePointer = ccHourglass
    ts.Type = "EXT"
    ts.Sen = txtScen.Text
    ts.Loc = txtLoc.Text
    If dtype = "SOD" And cboCons.Text = "ALL" Then 'read multiple cons
      elstr = 0
      elend = ElCnt - 1
    Else 'only reading one constituent
      If ElCnt > 1 Then 'more than one to pick from, find its index
        elstr = cboCons.ListIndex - 1
      Else
        elstr = 0
      End If
      elend = elstr
    End If
    ts.Stanam = txtDesc.Text
    ts.tu = cboTU.ItemData(cboTU.ListIndex)
    ts.ts = atxTS.Value
    ts.MVal = atxMiss.Value
    ts.MAcc = atxAcc.Value
    tsfill = atxFill.Value
    Call CopyI(6, lsdat(), ts.sdat())
    Call CopyI(6, ledat(), ts.EDat())
    Call F90_TIMDIF(lsdat(0), ledat(0), ts.tu, ts.ts, ts.NVal)
    ReDim ts.Vals(ts.NVal)

    For i = elstr To elend
      For j = 0 To ts.NVal - 1
        ts.Vals(j) = tsfill
      Next j
      If dtype = "SOD" Then 'get current cons from array
        ts.Con = ElType(i)
      Else 'only one cons, get from text box
        ts.Con = txtCons.Text
      End If
      ifl = FreeFile(0)
      Open FName For Input As #ifl
      If dtype <> "DLY" And dtype <> "SOD" And lsdat(3) = 0 Then
        'set start hour to 1 for hourly data value positioning
        lsdat(3) = 1
      End If
      If NOAA Then 'read noaa formatted data
        Call ReadNCDC(ifl, ts, dtype, ElType(i), lsdat(), retcod)
      Else 'read flat file
        fmt = UCase(txtFormat.Text)
        Call ReadTSFlat(ifl, CLng(atxHdr.Value), fmt, ts.ts, ts.tu, lsdat(), ledat(), tsfill, retcod)
      End If
      Close #ifl
      MousePointer = ccDefault
  
      If retcod >= 0 Then 'data read
        If retcod = 0 Then
          MsgBox "Time-series data values successfully read for the following data set:" & vbCrLf & "Constituent: " & ts.Con & "  Location: " & ts.Loc & "  Scenario: " & ts.Sen & vbCrLf & "Time series will be added to the list of available time series on the main form.", vbInformation, "WDMUtil Data Initialization"
        Else
          MsgBox "Some problems encountered during Time-series read.  Check data values closely for the following data set:" & vbCrLf & "Constituent: " & ts.Con & "  Location: " & ts.Loc & "  Scenario: " & ts.Sen & vbCrLf & "Time series will be added to the list of available time series on the main form.", vbInformation, "WDMUtil Data Initialization"
        End If
        ts.FilIndex = p.ExtCount + 1
        ts.id = 10000 + p.ExtCount + 1
        ReDim Preserve ExTS(p.ExtCount)
        ExTS(p.ExtCount) = ts
        p.ExtCount = p.ExtCount + 1
        Call RefreshSLC
        Call frmGenScn.RefreshMain
        Call frmGenScn.SelectAll
        frmGenScn.agdDSN.TopRow = frmGenScn.agdDSN.Rows
      Else 'read cancelled
        MsgBox "Time-series read cancelled." & vbCrLf & "Time series will NOT be added to the list of available time series on the main form.", vbInformation, "WDMUtil Data Initialization"
      End If
    Next i
    Unload frmWDMURead
  End If
  Exit Sub

errhandler:
  MousePointer = ccDefault
  MsgBox "Problem reading data into WDMUtil.", vbExclamation, "WDMUtil Data Initialization Problem"

End Sub

Private Sub ctlDates_LostFocus()
    
  lsdat(0) = Year(ctlDates.CurrS)
  lsdat(1) = Month(ctlDates.CurrS)
  lsdat(2) = Day(ctlDates.CurrS)
  'lsdat(3) = Hour(ctlDates.CurrS)
  'lsdat(4) = Minute(ctlDates.CurrS)
  ledat(0) = Year(ctlDates.CurrE)
  ledat(1) = Month(ctlDates.CurrE)
  ledat(2) = Day(ctlDates.CurrE)
  'ledat(3) = Hour(ctlDates.CurrE)
  'ledat(4) = Minute(ctlDates.CurrE)

End Sub

Private Sub Form_Load()

  Dim i&, ifl&, ofl&, istr$, lstr$, flen&, daypos&, errcod%, hppos&
  Dim tmpdat&(5), lnv&, ElExist As Boolean, tmpName$

  On Error GoTo initerrhnd
  errcod = 1
  Precip = False

  For i = 0 To 5
    lsdat(i) = 0
    ledat(i) = 0
  Next i
  FName = frmGenScn.cdlFile.FileName
  lblFile.Caption = FName
  flen = FileLen(FName)
  ifl = FreeFile(0)
  Open FName For Input As #ifl
  'read 1st line from file and put in file view box
  Line Input #ifl, istr
  If Len(istr) > 0.95 * flen Then 'whole file was read into string,
    'likely no Carriage Returns, write temp file with CRs
    ofl = FreeFile(0)
    tmpName = "WDMUxxx" & UCase(Right(FName, 4))
    Open tmpName For Output As #ofl
    Do
      Do While Len(istr) > 0
        i = InStr(istr, Chr(10))
        lstr = left(istr, i - 1) & vbCr
        'Print #ofl, left(istr, i - 1) & Chr(13) 'vbCrLf
        Print #ofl, lstr
        istr = Mid(istr, i + 1)
      Loop
      If Not EOF(ifl) Then 'some portion of input file still unread
        Line Input #ifl, istr
      End If
    Loop Until EOF(ifl)
    Close #ofl
    Close #ifl
    FName = tmpName
    Open FName For Input As #ifl
    'read 1st line from file and put in file view box
    Line Input #ifl, istr
  End If
  txtHead.Text = istr
  errcod = 2
  lstr = UCase(Right(FName, 4))
  If lstr = ".NCD" Or lstr = ".CRD" Then
'  If frmGenScn.cdlFile.FilterIndex = 1 Then
    'known noaa formats, user won't enter format
    NOAA = True
    fraFileHeader.Height = 1452
    'get some default info from 1st data record
    dtype = left(istr, 3)
    If IsNumeric(dtype) Then 'likely online SOD format
      If IsNumeric(Mid(istr, 32, 4)) Then 'check validity of year
        lsdat(0) = Mid(istr, 32, 4)
        If lsdat(0) > 1800 And lsdat(0) < 9999 Then
          dtype = "SOD"
          ts.ts = 1
          ts.tu = 4
          lsdat(1) = Mid(istr, 36, 2)
          lsdat(2) = 1 'assume start on 1st of month
          ElCnt = 1
          ReDim ElType(0)
          ElType(0) = Mid(istr, 55, 4)
          txtCons.Visible = False
          cboCons.Visible = True 'use list to display multiple cons
          cboCons.AddItem ElType(0)
          Do 'read more records to find additional constituents
            ElExist = False
            Line Input #ifl, istr
            If InList(Mid(istr, 55, 4), cboCons) Then
'            For i = 0 To ElCnt - 1 'look through current cons
'              If InStr(ElType(i), Mid(istr, 55, 4)) Then
                'constituent already in list, stop looking
                ElExist = True
'              End If
'            Next i
'            If Not ElExist Then 'add another cons
            Else 'add another cons
              ReDim Preserve ElType(ElCnt)
              ElType(ElCnt) = Mid(istr, 55, 4)
              cboCons.AddItem ElType(ElCnt)
              ElCnt = ElCnt + 1
            End If
          Loop Until ElExist
          'include ALL item if multiple cons
          If ElCnt > 1 Then cboCons.AddItem "ALL", 0
          cboCons.ListIndex = 0
        End If
      End If
      If dtype <> "SOD" Then 'problem reading format
        Err.Raise (1)
      End If
    Else 'should be a standard archive format
      If dtype = "DLY" Then
        ts.ts = 1
        ts.tu = 4
      ElseIf dtype = "HPD" Or dtype = "HLY" Then
        ts.ts = 1
        ts.tu = 3
      ElseIf dtype = "15M" Then
        ts.ts = 15
        ts.tu = 2
      ElseIf dtype = "COO" Then 'special coop data
        'assume hourly precip for now (may be other types later)
        ts.ts = 1
        ts.tu = 3
      Else 'not a recognized format
        Err.Raise (1)
      End If
      ReDim ElType(0)
      ElCnt = 1
      If dtype = "COO" Then 'skip down 2 records to get to data
        Line Input #ifl, istr
        txtHead.Text = txtHead.Text & vbCrLf & istr
        Line Input #ifl, istr
        txtHead.Text = txtHead.Text & vbCrLf & istr
        txtLoc.Text = left(istr, 6)
        ElType(0) = "HPCP"
        If Mid(istr, 11, 4) = "HPCP" Then
          hppos = 19
        ElseIf Mid(istr, 42, 4) = "HPCP" Then
          hppos = 50
          dtype = "COOX" 'indicate extra characters to read (field name)
          txtDesc = Trim(Mid(istr, 8, 30)) 'default description field
        Else 'not recognized as hourly precip coop data
          Err.Raise (1)
        End If
        lsdat(0) = Mid(istr, hppos, 4)
        lsdat(1) = Mid(istr, hppos + 5, 2)
        lsdat(2) = Mid(istr, hppos + 8, 2)
      Else 'other standard NCDC formats
        txtLoc.Text = Mid(istr, 4, 8)
        ElType(0) = Mid(istr, 12, 4)
        lsdat(0) = Mid(istr, 18, 4)
        lsdat(1) = Mid(istr, 22, 2)
        If dtype = "DLY" Then 'summary of the day NCDC format
          daypos = 31
        Else 'hourly (or less) data
          daypos = 26
        End If
        lsdat(2) = Mid(istr, daypos, 2)
      End If
      cboCons.Visible = False
      txtCons.Visible = True 'use text box for single cons
      txtCons.Text = ElType(0)
    End If
    cboTU.ListIndex = ts.tu - 1
    cboTU.Locked = True
    cboTU.Enabled = False
    atxTS.Value = ts.ts
    atxTS.Enabled = False
    For i = 1 To 3
      Line Input #ifl, istr
      txtHead.Text = txtHead.Text & vbCrLf & istr
    Next i
    'go to last record of file to find end date
    'back up a few records from end
    If dtype = "SOD" Then 'back up extra to find additional constituents
      Seek #ifl, flen - 6000
      Line Input #ifl, istr 'read 1 partial record before checking for constituents below
    Else
      Seek #ifl, flen - 1200
    End If
    'read until last record
    Do While Not EOF(ifl)
      Line Input #ifl, istr
      If Len(istr) > 0 Then
        lstr = istr 'save record, could be last
        If dtype = "SOD" Then 'check for more constituents
          If Not InList(Mid(istr, 55, 4), cboCons) Then
            ReDim Preserve ElType(ElCnt)
            ElType(ElCnt) = Mid(istr, 55, 4)
            cboCons.AddItem ElType(ElCnt)
            ElCnt = ElCnt + 1
          End If
        End If
      End If
    Loop
    If dtype = "SOD" Then
      tmpdat(0) = Mid(lstr, 32, 4)
      tmpdat(1) = Mid(lstr, 36, 2)
    ElseIf dtype = "COO" Or dtype = "COOX" Then 'hourly precip coop data
      tmpdat(0) = Mid(lstr, hppos, 4)
      tmpdat(1) = Mid(lstr, hppos + 5, 2)
      tmpdat(2) = Mid(lstr, hppos + 8, 2)
    Else
      tmpdat(0) = Mid(lstr, 18, 4)
      tmpdat(1) = Mid(lstr, 22, 2)
      tmpdat(2) = Mid(lstr, daypos, 2)
    End If
    If dtype = "HLY" Then 'last day of hourly values is on last record, leave end day alone
      Call CopyI(6, tmpdat(), ledat())
    Else 'precip event data or daily data (month of values/record)
      'assume data ends at end of month
      lnv = F90_DAYMON(tmpdat(0), tmpdat(1))
      tmpdat(2) = 0
      Call F90_TIMADD(tmpdat(0), 4, 1, lnv, ledat(0))
    End If
  Else 'unkown format, user will need format entry
    NOAA = False
    fraFileHeader.Height = 2772
    cboTU.ListIndex = 0
    cboTU.Enabled = True
    atxTS.Enabled = True
    cboCons.Visible = False
    txtCons.Visible = True 'use text box for single cons
    dtype = "USER"
    ElCnt = 1
    'put 1st 100 lines in file viewer
    i = 1
    Do While i < 100 And Not EOF(ifl)
      Line Input #ifl, istr
      txtHead.Text = txtHead.Text & vbCrLf & istr
      i = i + 1
    Loop
    lsdat(0) = 1900
    lsdat(1) = 1
    lsdat(2) = 1
    ledat(0) = 2020
    ledat(1) = 12
    ledat(2) = 31
  End If
  ledat(3) = 24
  fraSpecs.Top = fraFileHeader.Top + fraFileHeader.Height + 108
  ctlDates.Top = fraSpecs.Top + fraSpecs.Height + 108
  cmdImport.Top = ctlDates.Top + ctlDates.Height + 84
  cmdCancel.Top = cmdImport.Top
  frmWDMURead.Height = cmdImport.Top + cmdImport.Height + 477
  If InStr(txtCons.Text, "TMIN") > 0 Or _
     InStr(txtCons.Text, "TMAX") > 0 Then 'temp data
    atxMiss.Value = -99999#
    atxAcc.Value = -99998#
    atxFill.Value = -99999#
  Else 'precip or other non-negative data
    atxMiss.Value = -9.99
    atxAcc.Value = -9.98
    atxFill.Value = 0#
    If InStr(txtCons.Text, "CP") Then 'precip data
      Precip = True
    End If
  End If
  ctlDates.CurrS = DateSerial(lsdat(0), lsdat(1), lsdat(2)) '+ TimeSerial(lsdat(3), lsdat(4), lsdat(5))
  ctlDates.CurrE = DateSerial(ledat(0), ledat(1), ledat(2)) '+ TimeSerial(ledat(3), ledat(4), ledat(5))
  ctlDates.LimtS = DateSerial(lsdat(0), lsdat(1), lsdat(2)) '+ TimeSerial(lsdat(3), lsdat(4), lsdat(5))
  ctlDates.LimtE = DateSerial(ledat(0), ledat(1), ledat(2)) '+ TimeSerial(ledat(3), ledat(4), ledat(5))
  Close #ifl
  Exit Sub

initerrhnd:
  If Err.Number = 7 Then 'out of memory, buffer filled, continue
    Resume Next
  Else
    If errcod = 1 Then 'can't read file
      MsgBox "Unable to read specified file.  " & vbCrLf & "File may be missing carriage return/line feeds." & vbCrLf & "If file was downloaded from the Internet, it must be saved as" & vbCrLf & "a text file to assure carriage return/line feeds are included." & vbCrLf & "See the Data Access section in the WDMUtil documentation", vbExclamation, "WDMUtil File Open Problem"
      Call cmdCancel_Click
    Else
      MsgBox "Problem reading default information from specified file." & vbCrLf & "May have problems reading data." & vbCrLf & "Are you sure this data is in one of the NCDC formats?" & vbCrLf & "See the Data Access section in the WDMUtil documentation", vbExclamation, "WDMUtil File Open Problem"
      Resume Next
    End If
  End If

End Sub

Private Sub ReadNCDC(ifl&, ts As Timser, RecTyp$, EType$, sdat&(), retcod&)

  Dim i&, j&, istr$, ipos&, dpos&, mpos&, datpos&
  Dim yrpos&, mopos&, daypos&, HrPos&
  Dim nv&, CDat&(5), MAcc!, dvsr!
  Dim units$, flg1$, mflg$, mfrst As Boolean
  Dim readfg As Boolean, FirstRead As Boolean
  Dim ConMatch As Boolean, coopfg As Boolean

  On Error GoTo NCDC_erhnd
  retcod = 0

  mpos = 0 'init missing data position to start in case reading starts in middle of missing period
  mfrst = True
  mflg = ""
  ConMatch = True 'assume only one constituent, don't need to check for match
  coopfg = False 'assume not coop precip data
  If RecTyp = "SOD" Then 'set year, month position
    yrpos = 32
    mopos = 36
  ElseIf InStr(RecTyp, "COO") > 0 Then 'hourly precip coop data
    coopfg = True
    'skip 2 header records
    Line Input #ifl, istr
    Line Input #ifl, istr
    If RecTyp = "COOX" Then 'station name included, dates further out on record
      yrpos = 50
    Else
      yrpos = 19
    End If
    mopos = yrpos + 5
    daypos = yrpos + 8
    HrPos = yrpos + 11
  Else 'year, month position same for all other NCDC formats
    yrpos = 18
    mopos = 22
    If RecTyp = "DLY" Then 'summary of the day NCDC format
      daypos = 31
      HrPos = 33
    Else 'hourly precip or surface airways NCDC format
      daypos = 26
      HrPos = 31
    End If
  End If
  readfg = False
  FirstRead = True
cont: 'come back here if problem reading a record and want to continue
  Do While (readfg Or FirstRead) And Not EOF(ifl)
    If RecTyp = "SOD" Then ConMatch = False
    Do
      Line Input #ifl, istr
      If Not ConMatch Then 'check for matching constituent
        If Mid(istr, 55, 4) = EType Then
          ConMatch = True 'process this record
        End If
      End If
    Loop While Not ConMatch
    If Not readfg Then 'check date for start of data to read
      CDat(0) = CLng(Mid(istr, yrpos, 4))
      If CDat(0) >= sdat(0) Then 'same year or later
        CDat(1) = CLng(Mid(istr, mopos, 2))
        If CDat(1) >= sdat(1) Then 'same month or later
          If RecTyp = "DLY" Or RecTyp = "SOD" Then
            'daily data, start reading values
            readfg = True
          Else 'need to check day for hourly or 15 minute data
            CDat(2) = CLng(Mid(istr, daypos, 2))
            If CDat(2) >= sdat(2) Then 'same day or later
              readfg = True
            End If
          End If
        End If
      End If
    End If
    If readfg Then
      If FirstRead Then 'find first date to process
        ipos = 0
        Do
          If RecTyp = "SOD" Then '1st day of month implied
            CDat(2) = 1
          ElseIf RecTyp = "DLY" Then 'read next day value
            CDat(2) = CLng(Mid(istr, daypos + ipos, 2))
          Else 'day value constant, read next hour/minute values
            CDat(2) = CLng(Mid(istr, daypos, 2))
            CDat(3) = CLng(Mid(istr, HrPos + ipos, 2))
            If RecTyp = "HLY" Then 'surface airways format has hours=00-23
              CDat(3) = CDat(3) + 1
            ElseIf RecTyp = "15M" Then '15 minute data, need minutes too
              CDat(4) = CLng(Mid(istr, HrPos + ipos + 2, 2))
            End If
          End If
          ipos = ipos + 12
        Loop Until F90_TIMCHK(sdat(0), CDat(0)) >= 0
        FirstRead = False
        units = Mid(istr, 16, 2)
        If RecTyp = "SOD" Then
          If EType = "AWND" Or EType = "DPTP" Or EType = "SNOW" Then
            'measurements in tenths
            dvsr = 10
          ElseIf EType = "PRCP" Then 'precip in hundredths
            dvsr = 100
          ElseIf EType = "PRES" Then 'pressure in thousandths
            dvsr = 1000
          Else 'don't need to adjust values for units
            dvsr = 1#
          End If
        ElseIf coopfg Then 'precip data in hundredths
          dvsr = 100#
        ElseIf units = "HI" Then 'values in hundredths of inches
          dvsr = 100#
        ElseIf units = "TI" Then 'values in tenths of inches
          dvsr = 10#
        Else 'don't need to adjust values for units
          dvsr = 1#
        End If
      Else 'process start date from this record
        CDat(0) = CLng(Mid(istr, yrpos, 4))
        CDat(1) = CLng(Mid(istr, mopos, 2))
        If RecTyp <> "SOD" Then CDat(2) = CLng(Mid(istr, daypos, 2))
        If RecTyp <> "DLY" And RecTyp <> "SOD" Then 'get hour too
          CDat(3) = CLng(Mid(istr, HrPos, 2))
        End If
      End If
      'Call F90_TIMDIF(sdat(0), CDat(0), ts.tu, ts.ts, ioff)
      If RecTyp = "DLY" Or RecTyp = "SOD" Or coopfg Then
        '# values is days in month
        nv = F90_DAYMON(CDat(0), CDat(1))
      Else '# values is on current record
        nv = CLng(Mid(istr, 28, 3))
      End If
      ipos = 0
      For i = 0 To nv - 1
        If ipos + 30 <= Len(istr) Then 'more to read
          If RecTyp = "SOD" Then 'day is same as value index
            CDat(2) = i + 1
          ElseIf RecTyp = "DLY" Then 'read next day value
            CDat(2) = CLng(Mid(istr, daypos + ipos, 2))
          Else 'read next hour (and minute, if needed) value
            CDat(3) = CLng(Mid(istr, HrPos + ipos, 2))
            If RecTyp = "HLY" Then 'surface airways format has hours=00-23
              CDat(3) = CDat(3) + 1
            ElseIf RecTyp = "15M" Then '15 minute data, need minutes too
              CDat(4) = CLng(Mid(istr, HrPos + ipos + 2, 2))
            End If
          End If
        Else 'beyond end of input record
          Exit For
        End If
        If CDat(3) < 25 Then 'don't process daily sum of hourly values (hr=25)
          'determine offset based on value's date
          Call F90_TIMDIF(sdat(0), CDat(0), ts.tu, ts.ts, dpos)
          If dpos < ts.NVal Then
            If RecTyp = "SOD" Then
              flg1 = ""
            ElseIf coopfg Then
              flg1 = Mid(istr, daypos + ipos + 15, 1)
            Else
              flg1 = Mid(istr, ipos + 41, 1)
            End If
            If Len(Trim(flg1)) > 0 Then
              flg1 = UCase(flg1)
              If flg1 = "," Then 'continuation of accumulated period
                flg1 = "A"
              ElseIf flg1 = "{" Or flg1 = "}" Then 'deleted
                flg1 = "D"
              ElseIf flg1 = "[" Or flg1 = "]" Then 'missing
                flg1 = "M"
              End If
              If flg1 = "D" And _
                (RecTyp <> "HPD" And RecTyp <> "QPD" And Not coopfg) Then
                'D means deleted only for hourly or 15m precip
                flg1 = " "
              End If
            End If
            If coopfg Then
              datpos = daypos + ipos + 8
            Else
              datpos = ipos + 35
            End If
            If RecTyp = "HLY" And Mid(istr, datpos, 6) = "-99999" Then
              'missing indicator for surface airways hourly is -99999
              flg1 = "M"
            End If
            If flg1 = "M" Or flg1 = "D" Or flg1 = "A" Then
              'missing, deleted, or accumulated value
              If RecTyp = "DLY" Or RecTyp = "HLY" Then
                'value missing in place (no start/end)
                mflg = flg1
                If mflg = "A" Then 'find any missing values preceeding this one
                  mpos = dpos - 1
                  Do While Abs(ts.Vals(mpos) - ts.MVal) < 0.0001
                    mpos = mpos - 1
                  Loop
                  mpos = mpos + 1
                Else 'missing current value
                  mpos = dpos
                End If
              End If
              If mflg = "" Then 'set start of missing period
                mpos = dpos
                mflg = flg1
                If mfrst And mflg = "A" Then
                  'may need accumulated value if starting read in midst of missing distribution
                  MAcc = CSng(Mid(istr, datpos, 6)) / dvsr
                End If
              ElseIf flg1 = mflg Then 'reached end of missing period
                If flg1 <> "A" Or Mid(istr, datpos + 1, 5) <> "99999" Then
                  'not a continuation of an accumulated period
                  For j = mpos To dpos 'fill with missing value indicator
                    If mflg = "A" Then
                      ts.Vals(j) = ts.MAcc
                    Else
                      ts.Vals(j) = ts.MVal
                    End If
                  Next j
                  If mflg = "A" Then 'read accumulated value
                    ts.Vals(dpos) = CSng(Mid(istr, datpos, 6)) / dvsr
                  End If
                  mflg = ""
                  mfrst = False
                End If
              Else 'missing flag doesn't match current missing type
                MsgBox "Problem with missing/accumulated value indicators found at " & _
                        CDat(0) & "/" & CDat(1) & "/" & CDat(2) & " " & CDat(3) & ":" & CDat(4) & ":" & CDat(5), vbExclamation, "WDMUtil Data Initialization Problem"
                'use most recent indicator as start of new missing period
                mpos = dpos
                mflg = flg1
              End If
            Else 'valid value
              If RecTyp = "SOD" Then
                If Mid(istr, ipos + 62, 5) = "99999" Then 'missing value
                  ts.Vals(dpos) = ts.MVal
                Else 'valid value
                  If EType = "PKGS" Then 'only read 3 digits for peak gust
                    ts.Vals(dpos) = CSng(Mid(istr, ipos + 64, 3))
                  Else
                    ts.Vals(dpos) = CSng(Mid(istr, ipos + 61, 6)) / dvsr
                  End If
                End If
              ElseIf EType = "TSKC" Then 'only read 2 digits for sky (cloud) cover
                ts.Vals(dpos) = CSng(Mid(istr, datpos + 2, 2))
              ElseIf EType = "WIND" Then 'only read 3 digits for wind data
                ts.Vals(dpos) = CSng(Mid(istr, datpos + 3, 3))
              Else
                ts.Vals(dpos) = CSng(Mid(istr, datpos, 6)) / dvsr
              End If
              If RecTyp <> "DLY" And mflg <> "" And Not coopfg And _
                 Mid(istr, datpos + 1, 5) <> "99999" Then
                'shouldn't have valid values during missing period
                If mfrst Then 'first missing period,
                  'must have started reading in middle of it
                  For j = 0 To mpos 'fill from start of data to first missing position w/missing value indicator
                    If mflg = "A" Then
                      ts.Vals(j) = ts.MAcc
                    Else
                      ts.Vals(j) = ts.MVal
                    End If
                  Next j
                  If mflg = "A" Then 'read accumulated value
                    ts.Vals(mpos) = MAcc
                  End If
                  mflg = ""
                  mfrst = False
                Else 'problem
                  MsgBox "Found valid value in midst of missing period at " & _
                          CDat(0) & "/" & CDat(1) & "/" & CDat(2) & " " & CDat(3) & ":" & CDat(4) & ":" & CDat(5), vbExclamation, "WDMUtil Data Initialization Problem"
                End If
              End If
            End If
            If RecTyp = "SOD" Then
              ipos = ipos + 8
            ElseIf coopfg Then
              ipos = ipos + 16
            Else
              ipos = ipos + 12
            End If
          Else 'beyond end of data to be read
            readfg = False
            Exit For
          End If
        End If
      Next i
    End If
  Loop
  If RecTyp <> "DLY" And mflg <> "" Then 'missing period at end of data, fill it
    For j = mpos To ts.NVal - 1
      If mflg = "A" Then
        ts.Vals(j) = ts.MAcc
      Else
        ts.Vals(j) = ts.MVal
      End If
    Next j
  End If
  Exit Sub

NCDC_erhnd:
  If Len(istr) > 0 Then
    i = MsgBox("Could not process the following record:" & vbCrLf & istr, vbExclamation + vbOKCancel)
  Else 'blank line, ignore it
    i = vbOK
  End If
  If i = vbOK Then
    Resume cont
    retcod = 1
  Else 'cancel read
    retcod = -1
  End If

End Sub

Private Sub ReadTSFlat(SFile&, IHead&, DaFmt$, lts&, ltu&, BSDate&(), BEDate&(), tsfill!, retcod&)

  'Process time-series data values from flat file.

  'SFILE  - source flat file containing time-series data
  'IHEAD  - number of header lines in source file
  'DAFMT  - format of data on source file
  'LTS    - time step of the data
  'LTU    - time units of the data
  'BSDATE - start date of data being stored
  'BEDATE - end date of data being stored
  'RETCOD - return code

  Dim i&, j&, CDat&(6), LHead&, datpos&, DtSPos&
  Dim DtEPos&, Doff&, lval&, NVal&, NRVal&
  Dim DonFg&, VType&(31)
  Dim rval!(31), Buff$
  Const DatStr = "YMDHNS"

  On Error GoTo TSFlat_erhnd
  retcod = 0
  
  'add data values
  DtSPos = 6
  DtEPos = -1
  For i = 0 To 5
    'find start/end of date specs in format
    datpos = InStr(DaFmt, Mid(DatStr, i + 1, 1))
    If datpos > 0 Then 'this date position specified in format
      If i < DtSPos Then 'new date start position
        DtSPos = i
      End If
      If i > DtEPos Then 'new date end position
        DtEPos = i
      End If
    End If
  Next i
  If BSDate(0) > 0 And BEDate(0) > 0 Then
    'start/end date specified, determine total number of values
    Call F90_TIMDIF(BSDate(0), BEDate(0), ltu, lts, ts.NVal)
    ts.NVal = ts.NVal + 1
  Else 'init to a default buffer size
    ts.NVal = 100
  End If
  ReDim ts.Vals(ts.NVal)
  For i = 0 To ts.NVal - 1
    ts.Vals(i) = tsfill
  Next i

  For i = 0 To UBound(VType)
    VType(i) = 1
  Next i
  'NVal = 0
  NVal = -1
  lval = 0
  DonFg = 0
  LHead = IHead
cont: 'come back here if problem reading a record and want to continue
  Do While DonFg = 0 'get next record
    Do
      'skip any header lines
      Line Input #SFile, Buff
      LHead = LHead - 1
    Loop Until LHead < 0 Or EOF(SFile)
'    If EOF(SFile) Then 'reached end of file
'      DonFg = 1
'    End If
    If DonFg = 0 Then 'process data value
      Call GFMVAL(DaFmt, Buff, VType(), CDat(), NRVal, rval())
      If DtSPos > 0 And DtSPos < 6 Then
        'fill current date with base date up to start position
        For i = 0 To DtSPos - 1
          CDat(i) = BSDate(i)
        Next i
      End If
      If DtEPos > -1 And DtEPos < 5 Then
        'fill current date with base date up to start position
        For i = DtEPos + 1 To 5
          CDat(i) = BSDate(i)
        Next i
      End If
      If BSDate(0) = 0 Then
        'base date not yet defined, use initial date value read from file
        Call CopyI(6, CDat(), BSDate())
        If BEDate(0) > 0 Then
          'end date defined also, determine total number of values
          Call F90_TIMDIF(BSDate(0), BEDate(0), ltu, lts, ts.NVal)
          ts.NVal = ts.NVal + 1
          ReDim ts.Vals(ts.NVal)
          For i = 0 To ts.NVal - 1
            ts.Vals(i) = tsfill
          Next i
        End If
      End If
      If DtSPos < 6 Then
        'date specified on file, use to position in data buffer
        If F90_TIMCHK(BSDate(0), CDat(0)) >= 0 Then
          'current date is same as or after start date
          Call F90_TIMDIF(BSDate(0), CDat(0), ltu, lts, Doff)
          NVal = Doff '+ 1
        Else 'current date preceeds start date, don't process
          NRVal = 0
        End If
      Else 'dates not in use, just add to next buffer position
        NVal = NVal + 1
      End If
      If NVal + NRVal > ts.NVal Then 'too many values
        If DtSPos < 6 Then 'dates specified, only go to end date
          NRVal = ts.NVal - NVal
          DonFg = 1 'don't process any more values
        Else 'not using dates, add more to buffer
          ReDim Preserve ts.Vals(UBound(ts.Vals) + 100)
        End If
      End If
      If NRVal > 0 Then 'ok to include value(s)
        For i = 0 To NRVal - 1
          ts.Vals(NVal + i) = rval(i)
        Next i
        'save current number of values
        NVal = NVal + NRVal - 1
        lval = NVal
      Else 'reset to previous number of values
        NVal = lval
      End If
    End If
    'check for end of file
    If EOF(SFile) Then DonFg = 1
cont2: 'back here on problem reading record and continuing, but at end of file
    If DonFg <> 0 Then 'done processing values
      If NVal >= 0 Then 'processed values
        Call F90_TIMADD(BSDate(0), ltu, lts, NVal, ts.EDat(0))
        If ltu >= 3 Then 'hourly or greater time units
          'assume data goes through end of day
          ts.EDat(3) = 24
          If ltu >= 5 Then 'monthly or greater time units
            'assume data goes through end of month
            ts.EDat(2) = F90_DAYMON(ts.EDat(0), ts.EDat(1))
          End If
        End If
        ts.NVal = NVal + 1
      Else 'didn't process any values
        retcod = -1
        ts.NVal = 0
      End If
    End If
  Loop
  Exit Sub

TSFlat_erhnd:
  i = MsgBox("Could not process the following record:" & vbCrLf & Buff, vbExclamation + vbOKCancel)
  If i = vbOK Then
    retcod = 1
    If EOF(SFile) Then 'wrap up processing of time series
      DonFg = 1
      Resume cont2
    Else 'continue reading time series
      Resume cont
    End If
  Else 'cancel read
    retcod = -1
  End If
End Sub

Private Sub GFMVAL(CFmt$, Buff$, ValTyp&(), CDt&(), nrv&, rval!())

  'Use format statement for data input to generate date
  'and data values for the current data record.

  'CFMT - character string containing format for dates/data
  'BUFF - current data record being processed
  'ValTyp - array of data value types (0-int, 1-real, 2-char)
  'CDt  - date for data value(s) OR name for attribute value
  'NRV  - number of data values read from this record
  'RVAL - array of data value(s) read

  Dim i&, ipos&, xpos&, dpos&, vpos&, fpos&, rvpos&
  Dim DonFg&, nskip&, nx&, ilen&, jlen&, dp&, nrpt&
  Dim ParFlg&, ParPos&, ParRpt&, ParLen&
  Dim chfsep$, lfmt$, tfmt$, CurFmt$, ParFmt$
  Const DatStr = "YMDHNS"

  nrv = 0
  DonFg = 0
  rvpos = 0
  tfmt = CFmt
  Call LTrim(tfmt)
  If left(tfmt, 1) = "(" Then 'strip leading left paren
    tfmt = Mid(tfmt, 2)
  End If
  ipos = LenStr(tfmt)
  If Mid(tfmt, ipos, 1) = ")" Then 'strip trailing right paren
    tmft = left(tfmt, ipos - 1)
  End If
  CurFmt = tfmt

  ParFlg = 0
  nskip = 1
  Do
    'parse next part of format
    If ParFlg = 0 Then 'look for parentheses in format statement
      Call PARFND(CurFmt, ParPos, ParFmt, ParRpt, ParLen)
      i = InStr(tfmt, ",")
      If ParPos > 0 And (ParPos < i Or i = 0) Then
        'time to process parentheses, store remainder of format for later
        lfmt = Mid(CurFmt, ParPos + ParLen)
        'look for next part of format after parentheses
        ipos = InStr(lfmt, ",")
        If ipos > 0 Then
          tfmt = Mid(lfmt, ipos + 1)
        Else
          tfmt = lfmt
        End If
        'make current format the contents of the parentheses
        CurFmt = ParFmt
        ParFlg = 1
      End If
    End If
    Call LTrim(CurFmt)
    i = InStr(CurFmt, ",")
    If i > 0 Then 'find rightmost position of this segment
      ipos = LenStr(left(CurFmt, i - 1))
    Else 'no more commas, must be at end
      ipos = LenStr(CurFmt)
      DonFg = 1
    End If
    lfmt = left(CurFmt, ipos)
    xpos = InStr(lfmt, "X")
    dpos = InStr(DatStr, left(lfmt, 1))
    vpos = InStr(lfmt, "V")
    fpos = InStr(lfmt, "F")
    If xpos > 0 Then 'process space skip
      If ipos = 1 Then 'just an X, skip one space
        nx = 1
      ElseIf xpos = 1 Then 'X at beginning, process trailing numbers
        nx = CLng(Mid(lfmt, 2, ipos - 1))
      Else 'process numbers preceeding X
        nx = CLng(left(lfmt, ipos - 1))
      End If
      nskip = nskip + nx
    ElseIf dpos > 0 Then 'date specification
      dp = InStr(Mid(lfmt, 2, ipos - 2 + 1), ".")
      If dp > 0 Then
        'period in date format, just get field width for read
        ilen = CLng(Mid(lfmt, 2, dp - 1))
      Else 'no period, width of field is all thats left
        ilen = CLng(Mid(lfmt, 2, ipos - 1))
      End If
      CDt(dpos - 1) = CLng(Mid(Buff, nskip, ilen))
      nskip = nskip + ilen
    ElseIf vpos > 0 Then 'data value
      If vpos > 1 Then 'look for repeat number
        ilen = vpos - 1
        nrpt = CLng(left(lfmt, ilen))
        If nrpt = 0 Then 'not a valid repeat number
          nrpt = 1
        End If
      Else 'just get one value
        nrpt = 1
      End If
      dp = InStr(Mid(lfmt, vpos + 1, ipos - vpos), ".")
      If dp > 0 Then 'period in value format, just get field width for read
        ilen = CLng(Mid(lfmt, vpos + 1, dp - 1))
      ElseIf ipos > vpos Then 'no period, width of field is all thats left
        ilen = CLng(Mid(lfmt, vpos + 1, ipos - vpos))
      Else 'no period and no field width
        'look for 1st non-blank character
        Do While Mid(Buff, nskip, 1) = " " And nskip < Len(Buff)
          nskip = nskip + 1
        Loop
        'back up from end of record until a valid number is read
        ilen = Len(Buff) - nskip + 1
        Do While Not IsNumeric(Mid(Buff, nskip, ilen)) And ilen > 1
          ilen = ilen - 1
        Loop
      End If
      For i = 1 To nrpt
        If LenStr(Mid(Buff, nskip)) > 0 Then
          'more info on record to process
          jlen = LenStr(Mid(Buff, nskip, ilen))
          nrv = nrv + 1
          If ValTyp(nrv) < 2 Then 'integer or real
            rval(rvpos) = CSng(Mid(Buff, nskip, jlen))
          Else 'character, store position and length of string in buffer
            rval(rvpos) = 100 * nskip + jlen
          End If
          rvpos = rvpos + 1
          nskip = nskip + ilen
        Else 'at end of record
          Err.Raise 0
          DonFg = 1
          ParRpt = 1
        End If
      Next i
    ElseIf fpos > 0 Then 'field delimeter specifier
      If IsNumeric(Mid(lfmt, 2, ipos - 1)) Then 'ASCII code for field separator
        i = CLng(Mid(lfmt, 2, ipos - 1))
        chfsep = Chr(i)
      Else 'actual character specified
        chfsep = Mid(lfmt, 2, 1)
      End If
      'move to position just after field separator
      i = InStr(Mid(Buff, nskip), chfsep)
      nskip = nskip + i
    End If

    If DonFg = 0 Then 'move to start of next segment of format statement
      ipos = InStr(CurFmt, ",")
      lfmt = Mid(CurFmt, ipos + 1)
      CurFmt = lfmt
    ElseIf ParFlg = 1 Then 'parentheses in use
      If ParRpt > 1 Then 'repeating parentheses
        CurFmt = ParFmt
        ParRpt = ParRpt - 1
        DonFg = 0
      Else 'all done with parentheses
        If LenStr(tfmt) > 0 Then 'more to process in original format
          CurFmt = tfmt
          ParFlg = 0
          DonFg = 0
        End If
      End If
    End If
  Loop Until DonFg <> 0

End Sub

Private Sub PARFND(tfmt$, ParPos&, ParFmt$, ParRpt&, ParLen&)

  'Find any embedded parentheses in the format statement.

  'TFMT   - text containing current format statement
  'PARPOS - position in format of the start of the parentheses
  'PARFMT - text within the parentheses in the format
  'PARRPT - number of times to repeat format info within the parentheses
  'PARLEN - number of characters from start to end of parentheses

  Dim ipos&, ilen&, lbuff$

  ParFmt = " "
  ParRpt = 0
  ParPos = InStr(tfmt, "(")
  If ParPos > 0 Then 'parenthesis embedded in format
    If ParPos > 1 Then 'look for repeat indicator
      ipos = ParPos
      Do
        ipos = ipos - 1
      Loop Until Mid(tfmt, ipos, 1) = "," Or ipos = 1
      If ipos > 1 Then 'move up to position after comma
        ipos = ipos + 1
      End If
      ilen = ParPos - ipos
      If ilen > 0 Then 'read preceeding characters
        ParRpt = CLng(Mid(tfmt, ipos, ilen))
      End If
    End If
    If ParRpt = 0 Then 'assume indefinite repeating
      ParRpt = 1000
    End If
    ipos = InStr(tfmt, ")")
    If ipos > 0 Then 'right paren found, store contents between parentheses
      ParFmt = Mid(tfmt, ParPos + 1, ipos - ParPos - 1)
    Else 'did not find right paren, use remainder of format
      ipos = LenStr(tfmt)
      ParFmt = Mid(tfmt, ParPos + 1, ipos - ParPos)
    End If
    ParLen = ipos - ParPos + 1
  End If

End Sub
