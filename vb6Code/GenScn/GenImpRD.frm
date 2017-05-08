VERSION 5.00
Begin VB.Form frmGenScnImportRDB 
   Caption         =   "GenScn Activate OBSERVED RDB"
   ClientHeight    =   7560
   ClientLeft      =   990
   ClientTop       =   1470
   ClientWidth     =   6525
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   53
   Icon            =   "GenImpRD.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   7560
   ScaleWidth      =   6525
   Begin VB.Frame fraFileHeader 
      Caption         =   "File Header"
      Height          =   1932
      Left            =   120
      TabIndex        =   44
      Top             =   720
      Width           =   6252
      Begin VB.ComboBox cboColumn 
         Enabled         =   0   'False
         Height          =   288
         Left            =   2280
         Style           =   2  'Dropdown List
         TabIndex        =   2
         Top             =   1560
         Width           =   1812
      End
      Begin VB.TextBox txtHead 
         BackColor       =   &H80000016&
         Height          =   1212
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   3  'Both
         TabIndex        =   1
         Top             =   240
         Width           =   6012
      End
      Begin VB.Label lblColumn 
         Caption         =   "Select Column to Import"
         Enabled         =   0   'False
         Height          =   252
         Left            =   120
         TabIndex        =   45
         Top             =   1560
         Width           =   2172
      End
   End
   Begin VB.CommandButton cmdImport 
      Caption         =   "Import"
      Enabled         =   0   'False
      Height          =   375
      Index           =   0
      Left            =   1920
      TabIndex        =   20
      Top             =   7080
      Width           =   972
   End
   Begin VB.Frame fraSpecs 
      Caption         =   "Data Set Specifications"
      Enabled         =   0   'False
      Height          =   4215
      Left            =   120
      TabIndex        =   24
      Top             =   2760
      Width           =   6252
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   11
         Left            =   4920
         TabIndex        =   16
         Text            =   "<none>"
         Top             =   3120
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   10
         Left            =   4920
         TabIndex        =   13
         Text            =   "<none>"
         Top             =   2040
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   9
         Left            =   4920
         TabIndex        =   12
         Text            =   "<none>"
         Top             =   1680
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   8
         Left            =   4920
         TabIndex        =   11
         Text            =   "<none>"
         Top             =   1320
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   7
         Left            =   4920
         TabIndex        =   15
         Text            =   "<none>"
         Top             =   2760
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   6
         Left            =   4920
         TabIndex        =   14
         Text            =   "<none>"
         Top             =   2400
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   5
         Left            =   1680
         TabIndex        =   10
         Text            =   "<none>"
         Top             =   3120
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   4
         Left            =   1680
         TabIndex        =   9
         Text            =   "<none>"
         Top             =   2760
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   3
         Left            =   1680
         TabIndex        =   8
         Text            =   "<none>"
         Top             =   2400
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   2
         Left            =   1680
         TabIndex        =   7
         Text            =   "<none>"
         Top             =   2040
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   1
         Left            =   1680
         TabIndex        =   6
         Text            =   "<none>"
         Top             =   1680
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   0
         Left            =   1680
         TabIndex        =   5
         Text            =   "<none>"
         Top             =   1320
         Width           =   972
      End
      Begin VB.TextBox txtName 
         Enabled         =   0   'False
         Height          =   288
         Left            =   1080
         TabIndex        =   4
         Text            =   "<none>"
         Top             =   720
         Width           =   4812
      End
      Begin VB.TextBox txtID 
         Enabled         =   0   'False
         Height          =   288
         Left            =   1080
         TabIndex        =   3
         Text            =   "<none>"
         Top             =   360
         Width           =   1452
      End
      Begin VB.TextBox txtCons 
         Enabled         =   0   'False
         Height          =   288
         Left            =   2400
         TabIndex        =   18
         Text            =   "FLOW"
         Top             =   3720
         Width           =   1452
      End
      Begin VB.TextBox txtLoc 
         Enabled         =   0   'False
         Height          =   288
         Left            =   4080
         TabIndex        =   19
         Text            =   "<none>"
         Top             =   3720
         Width           =   1452
      End
      Begin VB.TextBox txtScen 
         Enabled         =   0   'False
         Height          =   288
         Left            =   720
         TabIndex        =   17
         Text            =   "OBSERVED"
         Top             =   3720
         Width           =   1452
      End
      Begin VB.Label lblAtts 
         Caption         =   "Attributes:"
         Enabled         =   0   'False
         Height          =   255
         Left            =   2880
         TabIndex        =   43
         Top             =   1080
         Width           =   1335
      End
      Begin VB.Label lblCodes 
         Caption         =   "Codes:"
         Enabled         =   0   'False
         Height          =   255
         Left            =   120
         TabIndex        =   42
         Top             =   1080
         Width           =   615
      End
      Begin VB.Label lblAtt 
         Caption         =   "Timeseries Type"
         Enabled         =   0   'False
         Height          =   255
         Index           =   11
         Left            =   3120
         TabIndex        =   41
         Top             =   3120
         Width           =   1575
      End
      Begin VB.Label lblAtt 
         Caption         =   "Base Discharge"
         Enabled         =   0   'False
         Height          =   255
         Index           =   10
         Left            =   3120
         TabIndex        =   40
         Top             =   2040
         Width           =   1455
      End
      Begin VB.Label lblAtt 
         Caption         =   "Datum Elevation"
         Enabled         =   0   'False
         Height          =   255
         Index           =   9
         Left            =   3120
         TabIndex        =   39
         Top             =   1680
         Width           =   1455
      End
      Begin VB.Label lblAtt 
         Caption         =   "Total Drainage Area"
         Enabled         =   0   'False
         Height          =   255
         Index           =   8
         Left            =   3120
         TabIndex        =   38
         Top             =   1320
         Width           =   1815
      End
      Begin VB.Label lblAtt 
         Caption         =   "Longitude"
         Enabled         =   0   'False
         Height          =   255
         Index           =   7
         Left            =   3120
         TabIndex        =   37
         Top             =   2760
         Width           =   975
      End
      Begin VB.Label lblAtt 
         Caption         =   "Latitude"
         Enabled         =   0   'False
         Height          =   255
         Index           =   6
         Left            =   3120
         TabIndex        =   36
         Top             =   2400
         Width           =   735
      End
      Begin VB.Label lblAtt 
         Caption         =   "Hydrologic Unit"
         Enabled         =   0   'False
         Height          =   255
         Index           =   5
         Left            =   360
         TabIndex        =   35
         Top             =   3120
         Width           =   1335
      End
      Begin VB.Label lblAtt 
         Caption         =   "County"
         Enabled         =   0   'False
         Height          =   255
         Index           =   4
         Left            =   360
         TabIndex        =   34
         Top             =   2760
         Width           =   615
      End
      Begin VB.Label lblAtt 
         Caption         =   "District"
         Enabled         =   0   'False
         Height          =   255
         Index           =   3
         Left            =   360
         TabIndex        =   33
         Top             =   2400
         Width           =   615
      End
      Begin VB.Label lblAtt 
         Caption         =   "State Code"
         Enabled         =   0   'False
         Height          =   255
         Index           =   2
         Left            =   360
         TabIndex        =   32
         Top             =   2040
         Width           =   975
      End
      Begin VB.Label lblAtt 
         Caption         =   "Statistics"
         Enabled         =   0   'False
         Height          =   255
         Index           =   1
         Left            =   360
         TabIndex        =   31
         Top             =   1680
         Width           =   855
      End
      Begin VB.Label lblAtt 
         Caption         =   "Parameter"
         Enabled         =   0   'False
         Height          =   255
         Index           =   0
         Left            =   360
         TabIndex        =   30
         Top             =   1320
         Width           =   975
      End
      Begin VB.Label lblCons 
         Alignment       =   2  'Center
         Caption         =   "Constituent"
         Enabled         =   0   'False
         Height          =   255
         Left            =   2400
         TabIndex        =   29
         Top             =   3480
         Width           =   1455
      End
      Begin VB.Label lblLoc 
         Alignment       =   2  'Center
         Caption         =   "Location"
         Enabled         =   0   'False
         Height          =   255
         Left            =   4080
         TabIndex        =   28
         Top             =   3480
         Width           =   1455
      End
      Begin VB.Label lblScen 
         Alignment       =   2  'Center
         Caption         =   "Scenario"
         Enabled         =   0   'False
         Height          =   255
         Left            =   720
         TabIndex        =   27
         Top             =   3480
         Width           =   1455
      End
      Begin VB.Label lblStanam 
         Caption         =   "Name:"
         Enabled         =   0   'False
         Height          =   255
         Left            =   360
         TabIndex        =   26
         Top             =   720
         Width           =   615
      End
      Begin VB.Label lblStaid 
         Caption         =   "Station ID:"
         Enabled         =   0   'False
         Height          =   255
         Left            =   120
         TabIndex        =   25
         Top             =   360
         Width           =   975
      End
   End
   Begin VB.CommandButton cmdImport 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Index           =   1
      Left            =   3600
      TabIndex        =   21
      Top             =   7080
      Width           =   972
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Default         =   -1  'True
      Height          =   252
      Left            =   360
      TabIndex        =   0
      Top             =   360
      Width           =   852
   End
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   0
      Top             =   360
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   2.54052e-29
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Left            =   1320
      TabIndex        =   23
      Top             =   360
      Width           =   4212
   End
   Begin VB.Label lblFiles 
      Caption         =   "RDB File:"
      Height          =   252
      Left            =   120
      TabIndex        =   22
      Top             =   120
      Width           =   2412
   End
End
Attribute VB_Name = "frmGenScnImportRDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim rval!(4), ival&(5)
Dim NHdr&, ltu&
Dim TrailingTab As Boolean
Dim baseyear&
Private Sub cmdFile_Click()

    Dim i%, j%, ip%, ifl&, istr$, lstr$

    TrailingTab = False
    CDFile.Flags = &H1806&
    CDFile.filter = "RDB files (*.rdb)|*.rdb"
    CDFile.filename = "*.rdb"
    CDFile.DialogTitle = "GenScn Import RDB Data"
    On Error GoTo 10
    CDFile.CancelError = True
    CDFile.Action = 1
    lblFile.Caption = CDFile.filename
    cmdImport(0).Enabled = True
    lblColumn.Enabled = True
    cboColumn.Enabled = True
    fraSpecs.Enabled = True
    lblStaid.Enabled = True
    txtID.Enabled = True
    lblStanam.Enabled = True
    txtName.Enabled = True
    lblCodes.Enabled = True
    lblAtts.Enabled = True
    For i = 0 To 11
      lblAtt(i).Enabled = True
      txtAtt(i).Enabled = True
    Next i
    lblScen.Enabled = True
    lblCons.Enabled = True
    lblLoc.Enabled = True
    txtScen.Enabled = True
    txtCons.Enabled = True
    txtLoc.Enabled = True
    txtHead.Enabled = True
    txtHead.Text = ""
    ifl = FreeFile(0)
    Open CDFile.filename For Input As #ifl
    Line Input #ifl, istr
    NHdr = 0
    While InStr(istr, "#") > 0 Or Len(istr) = 0
      'put header lines in text box
      NHdr = NHdr + 1
      txtHead.Text = txtHead.Text & istr & vbCrLf
      If InStr(istr, "tation name") > 0 Then
        ip = InStr(istr, ":")
        If ip > 0 Then
          txtName.Text = LTrim(Mid(istr, ip + 1))
        Else
          ip = InStr(istr, "tation name")
          txtName.Text = LTrim(Mid(istr, ip + 11))
        End If
      ElseIf InStr(istr, "tation number") > 0 Then
        ip = InStr(istr, ":")
        If ip > 0 Then
          txtID.Text = LTrim(Mid(istr, ip + 1))
        Else
          ip = InStr(istr, "tation number")
          txtID.Text = LTrim(Mid(istr, ip + 13))
        End If
      ElseIf InStr(istr, "atitude") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(6).Text = LTrim(Mid(istr, ip + 2))
          If Len(txtAtt(6).Text) > 0 Then
            rval(0) = CSng(txtAtt(6).Text)
          End If
        End If
      ElseIf InStr(istr, "ongitude") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(7).Text = LTrim(Mid(istr, ip + 2))
          If Len(txtAtt(7).Text) > 0 Then
            rval(1) = CSng(txtAtt(7).Text)
          End If
        End If
      ElseIf InStr(istr, "tate code") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(2).Text = LTrim(Mid(istr, ip + 2))
          ival(2) = 0
          If Len(txtAtt(2).Text) > 0 Then
            If IsNumeric(txtAtt(2).Text) Then
              ival(2) = CLng(txtAtt(2).Text)
            End If
          End If
        End If
      ElseIf InStr(istr, "ounty code") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(4).Text = LTrim(Mid(istr, ip + 2))
          If Len(txtAtt(4).Text) > 0 Then
            ival(4) = CLng(txtAtt(4).Text)
          End If
        End If
      ElseIf InStr(istr, "logic unit code") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(5).Text = LTrim(Mid(istr, ip + 2))
          If Len(txtAtt(5).Text) > 0 Then
            ival(5) = CLng(txtAtt(5).Text)
          End If
        End If
      ElseIf InStr(istr, "buting drainage area") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(8).Text = LTrim(Mid(istr, ip + 2))
          If Len(txtAtt(8).Text) > 0 Then
            rval(2) = CSng(txtAtt(8).Text)
          End If
        End If
      ElseIf InStr(istr, "rainage area") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(8).Text = LTrim(Mid(istr, ip + 2))
          If Len(txtAtt(8).Text) > 0 Then
            rval(2) = CSng(txtAtt(8).Text)
          End If
        End If
      ElseIf InStr(istr, "age datum") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(9).Text = LTrim(Mid(istr, ip + 2))
          If Len(txtAtt(9).Text) > 0 Then
            rval(3) = CSng(txtAtt(9).Text)
          End If
        End If
      ElseIf InStr(istr, "STORE parameter code") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(0).Text = LTrim(Mid(istr, ip + 2))
          If Len(txtAtt(0).Text) > 0 Then
            ival(0) = CLng(txtAtt(0).Text)
          End If
        End If
      ElseIf InStr(istr, "STORE statistic code") > 0 Then
        ip = InStr(istr, ".. ")
        If ip > 0 Then
          txtAtt(1).Text = LTrim(Mid(istr, ip + 2))
          If Len(txtAtt(1).Text) > 0 Then
            ival(1) = CLng(txtAtt(1).Text)
          End If
        End If
      End If
      Line Input #ifl, istr
    Wend
    'add two lines for rdb column headers
    NHdr = NHdr + 2
    'first non-comment line is column headers
    txtHead.Text = txtHead.Text & istr & vbCrLf
    While Len(istr) > 0
      'look for tab separators
      istr = LTrim(istr)
      ip = InStr(istr, Chr(9))
      If ip = 0 Then 'no tab, add remaining string as data column
        ip = Len(istr) + 1
      End If
      cboColumn.AddItem Left(istr, ip - 1)
      istr = Mid(istr, ip + 1)
    Wend
    'add lines to display some data records
    For i = 1 To 5
      Line Input #ifl, istr
      txtHead.Text = txtHead.Text & istr & vbCrLf
      If i = 1 Then 'remove non-numeric columns
        j = 0
        While Len(istr) > 0
          istr = LTrim(istr)
          ip = InStr(istr, Chr(9))
          If ip = 0 Then 'no tab, add remaining string
            ip = Len(istr) + 1
          End If
          lstr = Left(istr, ip - 1)
          If InStr(lstr, "n") = 0 Then 'not a numeric field
            cboColumn.RemoveItem j
            If j = cboColumn.ListCount Then
              TrailingTab = True
            End If
          Else
            j = j + 1
          End If
          istr = Mid(istr, ip + 1)
        Wend
      ElseIf datefg = 0 Then
        'determine width of date field
        'for now, assume date is 1st field
        istr = LTrim(istr)
        ip = InStr(istr, Chr(9))
        If ip > 10 Then 'daily data
          ltu = 4
        ElseIf ip > 7 Then 'monthly data
          ltu = 3
        Else
          MsgBox "Unable to determine time units of data.", 48, "GenScn Import RDB Data Problem"
          err.Raise (vbObjectError + 1)
        End If
      End If
      If i = 2 Then 'determine base year
        istr = LTrim(istr)
        istr = Mid(istr, 1, 4)
        baseyear = CInt(istr)
        baseyear = Int(baseyear / 10) * 10
      End If
    Next i
    cboColumn.ListIndex = 0
    Close #ifl
    Exit Sub

10  'continue here on cancel
    txtHead.Enabled = False
    cmdImport(0).Enabled = False
    lblColumn.Enabled = True
    cboColumn.Enabled = True
    fraSpecs.Enabled = False
    lblStaid.Enabled = False
    txtID.Enabled = False
    lblStanam.Enabled = False
    txtName.Enabled = False
    lblCodes.Enabled = False
    lblAtts.Enabled = False
    For i = 0 To 11
      lblAtt(i).Enabled = False
      txtAtt(i).Enabled = False
    Next i
    lblScen.Enabled = False
    lblCons.Enabled = False
    lblLoc.Enabled = False
    txtScen.Enabled = False
    txtCons.Enabled = False
    txtLoc.Enabled = False

End Sub


Private Sub cmdImport_Click(Index As Integer)

    Dim i&, rcod&, filstr$, dsn&
    Dim lsdate&(5), ledate&(5)

    If Index = 0 Then
      'import data
      'Call WDMDir.TSESPC(txtScen.Text, txtLoc.Text, txtCons.Text, Len(txtScen.Text), Len(txtLoc.Text), Len(txtCons.Text))
      'dsn = 1
      dsn = 0
      'Call WDMDir.TSDSM(dsn)
      If dsn = 0 Then 'no datasets match this one
        MousePointer = vbHourglass
        'ok to create, find next available dsn
        Call F90_INFREE(p.WDMFiles(1).fileUnit, 1, 1, 1, dsn, rcod)
        Call F90_WDLBAD(p.WDMFiles(1).fileUnit, dsn, 1, i)
        'set base year attribute
        Call F90_WDBSAI(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, 27, 1, baseyear, rcod)
        'open rdb file
        ifl = 60 'FreeFile(0)
        filstr = CDFile.filename
        'determine format for reading values
        If ltu = 4 Then 'daily
          lfmt = "Y4,F.,M2,F.,D2,F9"
        Else 'monthly assumed
          lfmt = "Y4,F.,M2,F9"
        End If
        For i = 0 To cboColumn.ListIndex - 1
          'skip fields preceeding selected one
          lfmt = lfmt & ",F9"
        Next i
        lfmt = lfmt & ",V"
        If TrailingTab Then
          lfmt = lfmt & ",F9"
        End If
        Call F90_TSFLAT(p.WDMFiles(1).fileUnit, dsn, filstr, NHdr, lfmt, 0, 1, ltu, _
                        30, 0, -99999, lsdate(0), ledate(0), rcod, Len(filstr), Len(lfmt))
        If rcod = 0 Then
          'fill in attributes
          Call AddAtts(dsn)
        End If
        Dim myWdm As clsTSerWDM
        Set myWdm = p.WDMFiles(1)
        myWdm.RefreshDsn dsn
        MousePointer = vbDefault
      Else
        'data set with these attributes already exists
        MsgBox "A timeseries with these attributes already exists.", 48, "GenScn Import Watstore Data"
      End If
      Close #ifl
    ElseIf Index = 1 Then
      'cancel
      Unload frmGenScnImportRDB
      frmGenScn.SetFocus
      'Call WDMDir.TSDRRE(p.WDMFiles(1).fileUnit, 0&, 0&)
      'p.StatusFilePath = CurDir
      Call RefreshSLC
      Call frmGenScn.RefreshMain
    End If

End Sub


Private Sub txtAtt_GotFocus(Index As Integer)

    Dim oldstr$
    oldstr = txtAtt(Index).Text

End Sub

Private Sub txtAtt_LostFocus(Index As Integer)

    Dim newstr$, chgflg&, itemp&, rtemp!

    If Index >= 0 And Index < 5 Then
      'integer
      newstr = txtAtt(Index).Text
      Call ChkTxtI("Integer Value", -99999, 10000000, newstr, itemp, chgflg)
      If chgflg <> 1 Then
        txtAtt(Index).Text = oldstr
      Else
        ival(Index) = itemp
      End If
    ElseIf Index >= 6 And Index <= 10 Then
      'real
      newstr = txtAtt(Index).Text
      Call ChkTxtR("Real Value", -99999#, 10000000, newstr, rtemp, chgflg)
      If chgflg <> 1 Then
        txtAtt(Index).Text = oldstr
      Else
        rval(Index - 6) = rtemp
      End If
    End If

End Sub



Private Sub AddAtts(dsn&)

    Dim saind&, salen&, lstr$, retcod&

    saind = 288
    salen = 8
    lstr = txtScen.Text
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 289
    salen = 8
    lstr = txtCons.Text
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 290
    salen = 8
    lstr = txtLoc.Text
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 1
    salen = 4
    lstr = txtAtt(11).Text
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 2
    salen = 8
    lstr = txtID.Text
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 45
    salen = 48
    lstr = txtName.Text
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 8
    salen = 1
    Call F90_WDBSAR(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, rval(0), retcod)
    saind = 9
    Call F90_WDBSAR(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, rval(1), retcod)
    saind = 11
    Call F90_WDBSAR(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, rval(2), retcod)
    saind = 264
    Call F90_WDBSAR(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, rval(3), retcod)
    saind = 49
    Call F90_WDBSAR(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, rval(4), retcod)
    saind = 41
    Call F90_WDBSAI(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, ival(2), retcod)
    saind = 42
    Call F90_WDBSAI(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, ival(3), retcod)
    saind = 6
    Call F90_WDBSAI(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, ival(4), retcod)
    saind = 4
    Call F90_WDBSAI(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, ival(5), retcod)
    saind = 56
    Call F90_WDBSAI(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, ival(0), retcod)
    saind = 57
    Call F90_WDBSAI(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, ival(1), retcod)

End Sub

