VERSION 5.00
Begin VB.Form frmGenScnEdit 
   Caption         =   "GenScn Edit TimeSeries List"
   ClientHeight    =   2568
   ClientLeft      =   1080
   ClientTop       =   1380
   ClientWidth     =   5772
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   42
   Icon            =   "GenTSEd.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   2568
   ScaleWidth      =   5772
   Begin VB.TextBox txtLocation 
      Height          =   288
      Left            =   4080
      TabIndex        =   2
      Top             =   840
      Width           =   1452
   End
   Begin VB.TextBox txtConstituent 
      Height          =   288
      Left            =   2160
      TabIndex        =   1
      Top             =   840
      Width           =   1452
   End
   Begin VB.TextBox txtScenario 
      Height          =   288
      Left            =   240
      TabIndex        =   0
      Top             =   840
      Width           =   1452
   End
   Begin VB.TextBox txtStanam 
      Height          =   288
      Left            =   240
      TabIndex        =   3
      Top             =   1560
      Width           =   5292
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   372
      Left            =   3000
      TabIndex        =   5
      Top             =   2040
      Width           =   1092
   End
   Begin VB.CommandButton cmdOkay 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   372
      Left            =   1680
      TabIndex        =   4
      Top             =   2040
      Width           =   1092
   End
   Begin VB.Label lblAtt 
      Caption         =   "Attributes of DSN _____:"
      Height          =   252
      Left            =   240
      TabIndex        =   10
      Top             =   240
      Width           =   3252
   End
   Begin VB.Label lblLocation 
      Caption         =   "Location:"
      Height          =   252
      Left            =   4080
      TabIndex        =   9
      Top             =   600
      Width           =   1452
   End
   Begin VB.Label lblConstituent 
      Caption         =   "Constituent:"
      Height          =   252
      Left            =   2160
      TabIndex        =   8
      Top             =   600
      Width           =   1452
   End
   Begin VB.Label lblScenario 
      Caption         =   "Scenario:"
      Height          =   252
      Left            =   240
      TabIndex        =   7
      Top             =   600
      Width           =   1452
   End
   Begin VB.Label lblStanam 
      Caption         =   "Description:"
      Height          =   252
      Left            =   240
      TabIndex        =   6
      Top             =   1320
      Width           =   1332
   End
End
Attribute VB_Name = "frmGenScnEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
    Dim lats&(), lnts
Private Sub cmdCancel_Click()
    Unload frmGenScnEdit
End Sub


Private Sub cmdOkay_Click()
    Dim i&, dsn&, saind&, salen&, retcod&, wdmfg&, saval$
    txtScenario = UCase(txtScenario)
    txtLocation = UCase(txtLocation)
    txtConstituent = UCase(txtConstituent)
    If lnts = 1 Then
      'put sen, loc, con, stanam
      rsp = vbYes
      If (Len(txtScenario.Text) = 0 Or Len(txtConstituent.Text) = 0 Or Len(txtLocation.Text) = 0) And TSer(lats(1) - 1).Type = "WDM" Then
        'changing wdm attributes to blank, are you sure?
        rsp = MsgBox("Are you sure you want to change this time series attribute to blank?" & Chr(13) & Chr(10) & _
                     "This time series will no longer be accessible in WDMUtil.", _
                     4, "WDMUtil Edit Time Series List Query")
      End If
      If rsp = vbYes Then
        dsn = TSer(lats(1) - 1).id
        If TSer(lats(1) - 1).Type = "WDM" Then
          saind = 288
          salen = 8
          saval = UCase(txtScenario)
          Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, saval, Len(saval))
          saind = 289
          saval = UCase(txtConstituent)
          Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, saval, Len(saval))
          saind = 290
          saval = UCase(txtLocation)
          Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, saval, Len(saval))
          saind = 45
          salen = 48
          Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, txtStanam, Len(txtStanam))
        ElseIf TSer(lats(1) - 1).Type = "EXT" Then
          i = 0
          Do While i <= UBound(ExTS)
            If dsn = ExTS(i).id Then 'alter this external time series
              ExTS(i).Sen = UCase(txtScenario)
              ExTS(i).Loc = UCase(txtLocation)
              ExTS(i).Con = UCase(txtConstituent)
              ExTS(i).Stanam = txtStanam
              i = UBound(ExTS)
            End If
            i = i + 1
          Loop
        End If
        Call F90_TSAPUT(dsn, txtScenario, txtLocation, txtConstituent, Len(txtScenario.Text), Len(txtLocation.Text), _
                        Len(txtConstituent.Text))
        TSer(lats(1) - 1).Sen = UCase(txtScenario)
        TSer(lats(1) - 1).Con = UCase(txtConstituent)
        TSer(lats(1) - 1).Loc = UCase(txtLocation)
        TSer(lats(1) - 1).Stanam = txtStanam
      End If
    Else
      rsp = vbYes
      If (Len(txtScenario.Text) = 0 Or Len(txtConstituent.Text) = 0 Or Len(txtLocation.Text) = 0) Then
        wdmfg = 0
        For i = 1 To lnts
          If TSer(lats(i) - 1).Type = "WDM" Then
            wdmfg = 1
          End If
        Next i
        If wdmfg = 1 Then
          'changing wdm attributes to blank, are you sure?
          rsp = MsgBox("Are you sure you want to change these time series attributes to blank?" & Chr(13) & Chr(10) & _
                 "These time series will no longer be accessible from WDMUtil.", _
                 4, "WDMUtil Edit Time Series List Query")
        End If
      End If
      If rsp = vbYes Then
        If txtScenario <> "<MULTIPLE>" Then
          'put scen
          For i = 1 To lnts
            dsn = TSer(lats(i) - 1).id
            If TSer(lats(i) - 1).Type = "WDM" Then
              saind = 288
              salen = 8
              saval = UCase(txtScenario)
              Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, saval, Len(saval))
            ElseIf TSer(lats(1) - 1).Type = "EXT" Then
              i = 0
              Do While i <= UBound(ExTS)
                If dsn = ExTS(i).id Then 'alter this external time series
                  ExTS(i).Sen = UCase(txtScenario)
                  i = UBound(ExTS)
                End If
                i = i + 1
              Loop
            End If
            TSer(lats(i) - 1).Sen = UCase(txtScenario)
          Next i
        End If
        If txtLocation <> "<MULTIPLE>" Then
          'put loc
          For i = 1 To lnts
            dsn = TSer(lats(i) - 1).id
            If TSer(lats(i) - 1).Type = "WDM" Then
              saind = 290
              salen = 8
              saval = UCase(txtLocation)
              Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, saval, Len(saval))
            ElseIf TSer(lats(1) - 1).Type = "EXT" Then
              i = 0
              Do While i <= UBound(ExTS)
                If dsn = ExTS(i).id Then 'alter this external time series
                  ExTS(i).Loc = UCase(txtLocation)
                  i = UBound(ExTS)
                End If
                i = i + 1
              Loop
            End If
            TSer(lats(i) - 1).Loc = UCase(txtLocation)
          Next i
        End If
        If txtConstituent <> "<MULTIPLE>" Then
          'put cons
          For i = 1 To lnts
            dsn = TSer(lats(i) - 1).id
            If TSer(lats(i) - 1).Type = "WDM" Then
              saind = 289
              salen = 8
              saval = UCase(txtConstituent)
              Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, saval, Len(saval))
            ElseIf TSer(lats(1) - 1).Type = "EXT" Then
              i = 0
              Do While i <= UBound(ExTS)
                If dsn = ExTS(i).id Then 'alter this external time series
                  ExTS(i).Con = UCase(txtConstituent)
                  i = UBound(ExTS)
                End If
                i = i + 1
              Loop
            End If
            TSer(lats(i) - 1).Con = UCase(txtConstituent)
          Next i
        End If
        If txtStanam <> "<multiple>" Then
          'put stanam
          For i = 1 To lnts
            dsn = TSer(lats(i) - 1).id
            If TSer(lats(i) - 1).Type = "WDM" Then
              saind = 45
              salen = 48
              Call F90_WDBSAC(p.WDMFiles(1).FileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, txtStanam, Len(txtStanam))
            ElseIf TSer(lats(1) - 1).Type = "EXT" Then
              i = 0
              Do While i <= UBound(ExTS)
                If dsn = ExTS(i).id Then 'alter this external time series
                  ExTS(i).Stanam = txtStanam
                  i = UBound(ExTS)
                End If
                i = i + 1
              Loop
            End If
            TSer(lats(i) - 1).Stanam = txtStanam
          Next i
        End If
        For i = 1 To lnts
          dsn = TSer(lats(i) - 1).id
          Call F90_TSAPUT(dsn, TSer(lats(i) - 1).Sen, TSer(lats(i) - 1).Loc, TSer(lats(i) - 1).Con, Len(TSer(lats(i) - 1).Sen), Len(TSer(lats(i) - 1).Loc), Len(TSer(lats(i) - 1).Con))
        Next i
      End If
    End If

    p.ScenCount = 0
    p.LocnCount = 0
    p.ConsCount = 0
    Call RefreshSLC
    frmGenScn.lstSLC(0).Clear
    Dim ObservedFlag As Boolean
    ObservedFlag = False
    For i = 0 To p.ScenCount - 1
      frmGenScn.lstSLC(0).AddItem p.Scen(i).Name
      If p.Scen(i).Name = "OBSERVED" Then
        ObservedFlag = True
      End If
    Next i
    If ObservedFlag = False Then
      frmGenScn.lstSLC(0).AddItem "OBSERVED"
    End If
    frmGenScn.lstSLC_GotFocus (0)
    frmGenScn.lstSLC(1).Clear
    For i = 0 To p.LocnCount - 1
      frmGenScn.lstSLC(1).AddItem p.Locn(i).Name
    Next i
    frmGenScn.lstSLC_GotFocus (1)
    frmGenScn.lstSLC(2).Clear
    For i = 0 To p.ConsCount - 1
      frmGenScn.lstSLC(2).AddItem p.Cons(i).Name
    Next i
    frmGenScn.lstSLC_GotFocus (2)
    If ((Len(txtScenario.Text) = 0 Or Len(txtConstituent.Text) = 0 Or Len(txtLocation.Text) = 0) And rsp = vbYes) Then
      i = 1
      While i <= nts
        If frmGenScn.agdDSN.Selected(i, 0) Then
          frmGenScn.agdDSN.DeleteRow i
          For j = i To nts - 1
            TSer(j - 1) = TSer(j)
          Next j
          nts = nts - 1
          ReDim Preserve TSer(nts)
        
          'If frmGenScn.LstDsn.ListItems(i).Selected Then
          '  'items blanked, need to remove
          '  frmGenScn.LstDsn.ListItems.Remove i
          'End If
          
        Else
          i = i + 1
        End If
      Wend
      'Scen = frmGenScn.LstDsn.ListItems.Count & " of " & CntDsn
      Scen = frmGenScn.agdDSN.MaxOccupiedRow & " of " & CntDsn
      frmGenScn.lblDsn.Caption = Scen
    End If
    Call frmGenScn.RefreshTSList
    
    Unload frmGenScnEdit
End Sub


Private Sub Form_Load()
    lnts = 0
    For i = 1 To nts
      If frmGenScn.agdDSN.Selected(i, 0) Then ' frmGenScn.LstDsn.ListItems(i).Selected Then
        'items selected to edit
        lnts = lnts + 1
        ReDim Preserve lats(lnts)
        lats(lnts) = i
      End If
    Next i
    If lnts = 1 Then
      lblAtt.Caption = "Attributes of DSN " & TSer(lats(lnts) - 1).id & ":"
      txtScenario = TSer(lats(1) - 1).Sen
      txtConstituent = TSer(lats(1) - 1).Con
      txtLocation = TSer(lats(1) - 1).Loc
      txtStanam = TSer(lats(1) - 1).Stanam
    Else
      lblAtt.Caption = "Attributes of Selected DSNs:"
      txtScenario = TSer(lats(1) - 1).Sen
      txtLocation = TSer(lats(1) - 1).Loc
      txtConstituent = TSer(lats(1) - 1).Con
      txtStanam = TSer(lats(1) - 1).Stanam
      For i = 2 To lnts
        If TSer(lats(i) - 1).Sen <> txtScenario Then
          txtScenario = "<multiple>"
          txtScenario.Enabled = False
          txtScenario.BackColor = Me.BackColor
        End If
        If TSer(lats(i) - 1).Con <> txtConstituent Then
          txtConstituent = "<multiple>"
          txtConstituent.Enabled = False
          txtConstituent.BackColor = Me.BackColor
        End If
        If TSer(lats(i) - 1).Loc <> txtLocation Then
          txtLocation = "<multiple>"
          txtLocation.Enabled = False
          txtLocation.BackColor = Me.BackColor
        End If
        If TSer(lats(i) - 1).Stanam <> txtStanam Then
          txtStanam = "<multiple>"
          txtStanam.Enabled = False
          txtStanam.BackColor = Me.BackColor
        End If
      Next i
    End If
End Sub



