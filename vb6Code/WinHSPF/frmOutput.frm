VERSION 5.00
Begin VB.Form frmOutput 
   Caption         =   "WinHSPF - Output Manager"
   ClientHeight    =   5076
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   5784
   HelpContextID   =   41
   Icon            =   "frmOutput.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5076
   ScaleWidth      =   5784
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdOutput 
      Cancel          =   -1  'True
      Caption         =   "&Close"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   1
      Left            =   2400
      TabIndex        =   6
      Top             =   4560
      Width           =   1215
   End
   Begin VB.Frame fraOutput 
      Caption         =   "Output Type"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2052
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   5535
      Begin VB.OptionButton opnOutput 
         Caption         =   "AQUATOX Linkage"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   3
         Left            =   3240
         TabIndex        =   11
         Top             =   240
         Width           =   2172
      End
      Begin VB.TextBox txtDesc 
         BackColor       =   &H80000004&
         Height          =   975
         Left            =   240
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         TabIndex        =   10
         Text            =   "frmOutput.frx":08CA
         Top             =   840
         Width           =   5055
      End
      Begin VB.OptionButton opnOutput 
         Caption         =   "Other"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   2
         Left            =   3240
         TabIndex        =   5
         Top             =   480
         Width           =   1095
      End
      Begin VB.OptionButton opnOutput 
         Caption         =   "Flow"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   1
         Left            =   480
         TabIndex        =   4
         Top             =   480
         Width           =   1095
      End
      Begin VB.OptionButton opnOutput 
         Caption         =   "Hydrology Calibration"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   0
         Left            =   480
         TabIndex        =   3
         Top             =   240
         Value           =   -1  'True
         Width           =   2535
      End
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   255
      Left            =   960
      TabIndex        =   0
      Top             =   4080
      Width           =   4095
      Begin VB.CommandButton cmdCopy 
         Caption         =   "Co&py"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   2880
         TabIndex        =   9
         Top             =   0
         Width           =   1215
      End
      Begin VB.CommandButton cmdAdd 
         Caption         =   "&Add"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   0
         TabIndex        =   8
         Top             =   0
         Width           =   1215
      End
      Begin VB.CommandButton cmdRemove 
         Caption         =   "&Remove"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   1440
         TabIndex        =   7
         Top             =   0
         Width           =   1215
      End
   End
   Begin ATCoCtl.ATCoGrid agdOutput 
      Height          =   1572
      Left            =   120
      TabIndex        =   1
      Top             =   2280
      Width           =   5532
      _ExtentX        =   9758
      _ExtentY        =   2773
      SelectionToggle =   0   'False
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   2
      Cols            =   2
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "Output Locations:"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   1
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483637
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
End
Attribute VB_Name = "frmOutput"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Sub cmdCopy_Click()
  If agdOutput.rows > 0 Then
    frmAddExpert.Init 4
    frmAddExpert.Show 1
  Else
    'have nothing to copy
    Call MsgBox("No 'Other' output specifications exist to copy.", vbOKOnly, "Copy Problem")
  End If
End Sub

Private Sub cmdOutput_Click(Index As Integer)
  Unload Me
End Sub

Public Sub RefreshAll()
  
  Dim loper As HspfOperation
  Dim i&, s$, j&
  Dim vConn As Variant, lConn As HspfConnection
  Dim dsnObj As ATCclsTserData
  Dim WDMId$, idsn&, ctemp$

  If opnOutput(0) = True Then
    txtDesc.Text = "Output will be generated at each 'Hydrology Calibration' output location for " & _
      "total runoff, surface runoff, interflow, base flow, potential evapotranspiration, actual evapotranspiration, " & _
      "upper zone storage, and lower zone storage."
    With agdOutput
      .rows = 0
      .cols = 2
      .ColTitle(0) = "Name"
      .ColTitle(1) = "Description"
      .ColType(0) = ATCoTxt
      .ColEditable(0) = False
      .ColEditable(1) = False
      For i = 1 To myUci.OpnSeqBlock.Opns.Count
        Set loper = myUci.OpnSeqBlock.Opn(i)
        If loper.Name = "RCHRES" Then
          If IsCalibLocation(loper.Name, loper.Id) Then
            'this is an expert system output location
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = loper.Name & " " & loper.Id
            .TextMatrix(.rows, 1) = loper.Description
          End If
        End If
      Next i
      .ColsSizeByContents
    End With
    cmdCopy.Visible = False
  ElseIf opnOutput(1) = True Then
    txtDesc.Text = "Streamflow output will be generated at each 'Flow' output location."
    With agdOutput
      .rows = 0
      .cols = 2
      .ColTitle(0) = "Name"
      .ColTitle(1) = "Description"
      .ColType(0) = ATCoTxt
      .ColEditable(0) = False
      .ColEditable(1) = False
      For i = 1 To myUci.OpnSeqBlock.Opns.Count
        Set loper = myUci.OpnSeqBlock.Opn(i)
        If loper.Name = "RCHRES" Then
          If IsFlowLocation(loper.Name, loper.Id) Then
            'this is an output flow location
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = loper.Name & " " & loper.Id
            .TextMatrix(.rows, 1) = loper.Description
          End If
        End If
      Next i
      .ColsSizeByContents
    End With
    cmdCopy.Visible = False
  ElseIf opnOutput(2) = True Then
    txtDesc.Text = "Output will be generated at each 'Other' output location " & _
      "for the specified constituents."
    With agdOutput
      .rows = 0
      .cols = 3
      .ColTitle(0) = "Name"
      .ColTitle(1) = "Description"
      .ColTitle(2) = "Group/Member"
      .ColType(0) = ATCoTxt
      .ColEditable(0) = False
      .ColEditable(1) = False
      .ColEditable(2) = False
      For i = 1 To myUci.OpnSeqBlock.Opns.Count
        Set loper = myUci.OpnSeqBlock.Opn(i)
        'look for any output from here in ext targets
        For Each vConn In loper.targets
          Set lConn = vConn
          If Mid(lConn.Target.volname, 1, 3) = "WDM" Then
            If lConn.Source.volname = "COPY" Then
              'assume this is a calibration location, skip it
            ElseIf lConn.Source.group = "ROFLOW" And lConn.Source.member = "ROVOL" Then
              'this is part of the calibration location
            ElseIf lConn.Source.group = "HYDR" And lConn.Source.member = "RO" And IsFlowLocation(loper.Name, loper.Id) Then
              'this is an output flow location
            Else
              idsn = lConn.Target.volid
              WDMId = lConn.Target.volname
              Set dsnObj = myUci.GetDataSetFromDsn(WDMInd(WDMId), idsn)
              If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
                'this an an aquatox output location
              Else
                'this is an other output location
                .rows = .rows + 1
                .TextMatrix(.rows, 0) = loper.Name & " " & loper.Id
                .TextMatrix(.rows, 1) = loper.Description
                ctemp = lConn.Source.group & ":" & lConn.Source.member
                If TSMaxSubscript(1, lConn.Source.group, lConn.Source.member) > 1 Then
                  ctemp = ctemp & "(" & lConn.Source.MemSub1
                  If TSMaxSubscript(2, lConn.Source.group, lConn.Source.member) > 1 Then
                    ctemp = ctemp & "," & lConn.Source.MemSub2
                  End If
                  ctemp = ctemp & ")"
                End If
                .TextMatrix(.rows, 2) = ctemp
              End If
            End If
          End If
        Next vConn
      Next i
      .ColsSizeByContents
    End With
    cmdCopy.Visible = True
  ElseIf opnOutput(3) = True Then
    txtDesc.Text = "Output will be generated at each 'AQUATOX Linkage' output location for " & _
      "inflow, discharge, surface area, mean depth, water temperature, suspended sediment, " & _
      "organic chemicals (if available), and inflows of nutrients, " & _
      "DO, BOD, refractory organic carbon, and sediment."
    With agdOutput
      .rows = 0
      .cols = 2
      .ColTitle(0) = "Name"
      .ColTitle(1) = "Description"
      .ColType(0) = ATCoTxt
      .ColEditable(0) = False
      .ColEditable(1) = False
      For i = 1 To myUci.OpnSeqBlock.Opns.Count
        Set loper = myUci.OpnSeqBlock.Opn(i)
        If loper.Name = "RCHRES" Then
          If IsAQUATOXLocation(loper.Name, loper.Id) Then
            'this is an expert system output location
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = loper.Name & " " & loper.Id
            .TextMatrix(.rows, 1) = loper.Description
          End If
        End If
      Next i
      .ColsSizeByContents
    End With
  End If
End Sub

Private Sub Form_Load()
  RefreshAll
  'lstTo.Visible = False
  opnOutput(0) = True
End Sub

Private Sub Form_Resize()
  If width > 700 And height > 2400 Then
    fraOutput.width = width - 300
    txtDesc.width = fraOutput.width - 500
    agdOutput.width = width - 300
    If cmdCopy.Visible Then
      fraButtons.width = (3 * cmdAdd.width) + 450
    Else
      fraButtons.width = (2 * cmdAdd.width) + 225
    End If
    fraButtons.Left = width / 2 - fraButtons.width / 2
    fraButtons.Top = height - fraButtons.height - 1000
    cmdOutput(1).Left = (width / 2) - (cmdOutput(1).width / 2)
    cmdOutput(1).Top = height - cmdOutput(1).height - 500
    If height > 3500 Then
      agdOutput.height = height - fraOutput.height - fraButtons.height - 1400
    End If
  End If
End Sub

Private Sub cmdAdd_Click()
  If opnOutput(0) = True Then 'calibration locations
    frmAddExpert.Init 1
    frmAddExpert.Show 1
  ElseIf opnOutput(1) = True Then 'flow locations
    frmAddExpert.Init 2
    frmAddExpert.Show 1
  ElseIf opnOutput(2) = True Then 'other outputs
    frmAddExpert.Init 3
    frmAddExpert.Show 1
  ElseIf opnOutput(3) = True Then 'aquatox link
    frmAddExpert.Init 5
    frmAddExpert.Show 1
  End If
End Sub
    
Private Sub cmdRemove_Click()
  Dim i&, copyid&, irch&, nsel&, crch$, j&
  Dim vConn As Variant, lConn As HspfConnection
  Dim vtarget As Variant, ltarget As HspfConnection
  Dim lOpn As HspfOperation, k&
  Dim colonpos&, Id&, WDMId$, idsn&, parpos&, commapos&, lparpos&
  Dim spacepos&, ctemp$, opname$, group$, member$, sub1&, sub2&
  Dim iresp As Long
  Dim dsnObj As ATCclsTserData
  
  If opnOutput(0) = True Then 'calibration locations
    nsel = 0
    For i = 1 To agdOutput.rows
      If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
        'something is selected
        crch = agdOutput.TextMatrix(i, 0)
        'find copy operation associated with crch
        irch = CLng(Mid(crch, 7))
        copyid = Reach2Copy(irch)
        
        iresp = myMsgBox.Show("Do you want to permanently delete the WDM timeseries associated with this calibration location?", _
                              "Delete Query", "+&Yes", "-&No")
        If iresp = 1 Then
          'remove the ext targets datasets for this copy
          For Each vConn In myUci.Connections
            Set lConn = vConn
            If lConn.typ = 4 And Trim(lConn.Source.volname) = "COPY" And _
               lConn.Source.volid = copyid Then 'this is one
              'delete this dsn
              myUci.DeleteWDMDataSet lConn.Target.volname, lConn.Target.volid
            End If
          Next vConn
        End If
        
        'now delete this copy operation
        If copyid > 0 Then
          Call myUci.DeleteOperation("COPY", copyid)
        End If
        'remove the ext targets entry for simq here
        j = 1
        For Each vConn In myUci.Connections
          Set lConn = vConn
          If lConn.typ = 4 And lConn.Source.volname = "RCHRES" And _
             lConn.Source.volid = irch And Trim(lConn.Target.member) = "SIMQ" Then 'this is the one
            If iresp = 1 Then
              'delete this dsn
              myUci.DeleteWDMDataSet lConn.Target.volname, lConn.Target.volid
            End If
            'remove the connection
            myUci.Connections.Remove j
            'also remove connection from operation
            Set lOpn = myUci.OpnBlks("RCHRES").operfromid(irch)
            k = 1
            For Each vtarget In lOpn.targets
              Set ltarget = vtarget
              If Mid(ltarget.Target.volname, 1, 3) = "WDM" And _
                 Trim(ltarget.Target.member) = "SIMQ" Then
                lOpn.targets.Remove k
              Else
                k = k + 1
              End If
            Next vtarget
          Else
            j = j + 1
          End If
        Next vConn
        nsel = nsel + 1
      End If
    Next i
    If nsel > 0 Then
      RefreshAll
    Else
      Call MsgBox("No location is selected to remove.", vbOKOnly, "Remove Problem")
    End If
  ElseIf opnOutput(1) = True Then 'flow locations
    'remove
    nsel = 0
    For i = 1 To agdOutput.rows
      If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
        'something is selected
        crch = agdOutput.TextMatrix(i, 0)
        irch = CLng(Mid(crch, 7))
        
        iresp = myMsgBox.Show("Do you want to permanently delete the output WDM timeseries?", _
                              "Delete Query", "+&Yes", "-&No")

        'remove the ext targets entry here
        j = 1
        For Each vConn In myUci.Connections
          Set lConn = vConn
          If lConn.typ = 4 And lConn.Source.volname = "RCHRES" And _
             lConn.Source.volid = irch And lConn.Target.member = "FLOW" Then 'this is the one
            If iresp = 1 Then
              'delete this dsn
              myUci.DeleteWDMDataSet lConn.Target.volname, lConn.Target.volid
            End If
            'remove the connection
            myUci.Connections.Remove j
            'also remove connection from operation
            Set lOpn = myUci.OpnBlks("RCHRES").operfromid(irch)
            k = 1
            For Each vtarget In lOpn.targets
              Set ltarget = vtarget
              If Mid(ltarget.Target.volname, 1, 3) = "WDM" And _
                 ltarget.Target.member = "FLOW" Then
                lOpn.targets.Remove k
              Else
                k = k + 1
              End If
            Next vtarget
          Else
            j = j + 1
          End If
        Next vConn
        nsel = nsel + 1
      End If
    Next i
    If nsel > 0 Then
      RefreshAll
    Else
      Call MsgBox("No location is selected to remove.", vbOKOnly, "Remove Problem")
    End If
  ElseIf opnOutput(2) = True Then 'remove other
    nsel = 0
    For i = 1 To agdOutput.rows
      If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
        'something is selected
        ctemp = agdOutput.TextMatrix(i, 0)
        spacepos = InStr(1, ctemp, " ")
        opname = Mid(ctemp, 1, spacepos - 1)
        Id = CInt(Mid(ctemp, spacepos + 1))
        ctemp = agdOutput.TextMatrix(i, 2)
        colonpos = InStr(1, ctemp, ":")
        group = Mid(ctemp, 1, colonpos - 1)
        member = Mid(ctemp, colonpos + 1)
        parpos = InStr(1, member, "(")
        sub1 = 1
        sub2 = 1
        If parpos > 0 Then
          commapos = InStr(1, member, ",")
          If commapos > 0 Then
            sub1 = CInt(Mid(member, parpos + 1, commapos - parpos - 1))
            lparpos = InStr(1, member, ")")
            sub2 = CInt(Mid(member, commapos + 1, lparpos - commapos - 1))
            member = Mid(member, 1, parpos - 1)
          Else
            lparpos = InStr(1, member, ")")
            sub1 = CInt(Mid(member, parpos + 1, lparpos - parpos - 1))
            sub2 = 1
            member = Mid(member, 1, parpos - 1)
          End If
        End If
        
        'remove the ext targets entry here
        
        iresp = myMsgBox.Show("Do you want to permanently delete the output WDM timeseries?", _
                              "Delete Query", "+&Yes", "-&No")
                              
        j = 1
        For Each vConn In myUci.Connections
          Set lConn = vConn
          If lConn.typ = 4 And lConn.Source.volname = opname And _
             lConn.Source.volid = Id And lConn.Source.group = group And _
             lConn.Source.member = member And _
             lConn.Source.MemSub1 = sub1 And lConn.Source.MemSub2 = sub2 Then 'this is the one
            If iresp = 1 Then
              'delete this dsn
              myUci.DeleteWDMDataSet lConn.Target.volname, lConn.Target.volid
            End If
            'remove the connection
            myUci.Connections.Remove j
            'also remove connection from operation
            Set lOpn = myUci.OpnBlks(opname).operfromid(Id)
            k = 1
            For Each vtarget In lOpn.targets
              Set ltarget = vtarget
              If Mid(ltarget.Target.volname, 1, 3) = "WDM" And _
                 ltarget.Source.group = group And _
                 ltarget.Source.member = member And _
                 ltarget.Source.MemSub1 = sub1 And ltarget.Source.MemSub2 = sub2 Then
                lOpn.targets.Remove k
              Else
                k = k + 1
              End If
            Next vtarget
          Else
            j = j + 1
          End If
        Next vConn
        nsel = nsel + 1
      End If
    Next i
    If nsel > 0 Then
      RefreshAll
    Else
      Call MsgBox("No output is selected to remove.", vbOKOnly, "Remove Problem")
    End If
  ElseIf opnOutput(3) = True Then 'remove aquatox location
    nsel = 0
    For i = 1 To agdOutput.rows
      If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
        'something is selected
        crch = agdOutput.TextMatrix(i, 0)
        irch = CLng(Mid(crch, 7))
        
        j = 0
        For Each vConn In myUci.Connections
          Set lConn = vConn
          If lConn.typ = 4 And lConn.Source.volname = "RCHRES" And _
             lConn.Source.volid = irch Then 'this is the one
            WDMId = lConn.Target.volname
            idsn = lConn.Target.volid
            Set dsnObj = myUci.GetDataSetFromDsn(WDMInd(WDMId), idsn)
            If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
              j = j + 1
            End If
          End If
        Next vConn
        
        iresp = myMsgBox.Show("Do you want to permanently delete the " & j & " WDM timeseries " & vbCrLf & _
                              "associated with this AQUATOX Linkage location?", _
                              "Delete Query", "+&Yes", "-&No")
        
        'remove the ext targets and dsns
        j = 1
        For Each vConn In myUci.Connections
          Set lConn = vConn
          If lConn.typ = 4 And lConn.Source.volname = "RCHRES" And _
             lConn.Source.volid = irch Then 'this is the one
            WDMId = lConn.Target.volname
            idsn = lConn.Target.volid
            Set dsnObj = myUci.GetDataSetFromDsn(WDMInd(WDMId), idsn)
            If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
              'found aquatox dsn
              If iresp = 1 Then
                'delete this dsn
                myUci.DeleteWDMDataSet WDMId, idsn
              End If
              'remove the connection
              myUci.Connections.Remove j
              'also remove connection from operation
              Set lOpn = myUci.OpnBlks("RCHRES").operfromid(irch)
              k = 1
              For Each vtarget In lOpn.targets
                Set ltarget = vtarget
                If ltarget.Target.volname = WDMId And _
                  ltarget.Target.volid = idsn Then
                  lOpn.targets.Remove k
                Else
                  k = k + 1
                End If
              Next vtarget
            Else
              j = j + 1
            End If
          Else
            j = j + 1
          End If
        Next vConn
        nsel = nsel + 1
      End If
    Next i
    If nsel > 0 Then
      RefreshAll
    Else
      Call MsgBox("No location is selected to remove.", vbOKOnly, "Remove Problem")
    End If
  End If
End Sub

Public Function IsCalibLocation(Name$, Id&) As Boolean
  'call it a calib loc if there are copy ops to wdm and
  '  this reach is associated with a copy ifwo dataset,
  '  there may be a better way
  
  Dim lTable As HspfTable
  Dim loper As HspfOperation
  Dim lConn As HspfConnection
  Dim i&, s$, j&, expertflag As Boolean, copyid&

  IsCalibLocation = False
  expertflag = False
  For i = 1 To myUci.OpnSeqBlock.Opns.Count
    Set loper = myUci.OpnSeqBlock.Opn(i)
    If loper.Name = "COPY" Then
      For j = 1 To loper.targets.Count
        Set lConn = loper.targets(j)
        If Left(lConn.Target.volname, 3) = "WDM" And _
          Trim(lConn.Target.member) = "IFWO" Then
          'looks like we have some expert system output locations
          expertflag = True
        End If
      Next j
    End If
  Next i
  copyid = Reach2Copy(Id)
  If copyid > 0 Then
    Set loper = myUci.OpnBlks("COPY").operfromid(copyid)
    For j = 1 To loper.targets.Count
      Set lConn = loper.targets(j)
      If Left(lConn.Target.volname, 3) = "WDM" And _
        Trim(lConn.Target.member) = "IFWO" And expertflag Then
        'this is an expert system output location
        IsCalibLocation = True
      End If
    Next j
  End If
  
End Function

Public Function IsAQUATOXLocation(Name$, Id&) As Boolean
  'call it an aquatox loc if required sections are on and
  '  this reach has required output
  Dim dsnObj As ATCclsTserData
  Dim lTable As HspfTable
  Dim loper As HspfOperation
  Dim lConn As HspfConnection
  Dim i&, s$, j&, expertflag As Boolean, copyid&
  Dim ifound(7) As Boolean, idsn&, WDMId$

  IsAQUATOXLocation = False
  Set loper = myUci.OpnBlks(Name).operfromid(Id)
  Set lTable = loper.tables("ACTIVITY")
  If lTable.Parms(1).Value = 1 And lTable.Parms(4).Value = 1 And _
     lTable.Parms(5).Value = 1 And lTable.Parms(7).Value = 1 And _
     lTable.Parms(8).Value = 1 Then
    'all required rchres sections are on
    '(hydr, htrch, sedtrn, oxrx, nutrx)
    For j = 1 To 7
      ifound(j) = False
    Next j
    For j = 1 To loper.targets.Count
      Set lConn = loper.targets(j)
      If Left(lConn.Target.volname, 3) = "WDM" Then
        idsn = lConn.Target.volid
        WDMId = lConn.Target.volname
        Set dsnObj = myUci.GetDataSetFromDsn(WDMInd(WDMId), idsn)
        If Trim(lConn.Source.member) = "AVDEP" Then
          If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
            ifound(1) = True
          End If
        ElseIf Trim(lConn.Source.member) = "SAREA" Then
          If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
            ifound(2) = True
          End If
        ElseIf Trim(lConn.Source.member) = "IVOL" Then
          If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
            ifound(3) = True
          End If
        ElseIf Trim(lConn.Source.member) = "RO" Then
          If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
            ifound(4) = True
          End If
        ElseIf Trim(lConn.Source.member) = "TW" Then
          If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
            ifound(5) = True
          End If
        ElseIf Trim(lConn.Source.member) = "NUIF1" Then
          If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
            ifound(6) = True
          End If
        ElseIf Trim(lConn.Source.member) = "OXIF" Then
          If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
            ifound(7) = True
          End If
        End If
      End If
    Next j
    If ifound(1) And ifound(2) And ifound(3) And ifound(4) And _
       ifound(5) And ifound(6) And ifound(7) Then
      'this is an aquatox output location
      IsAQUATOXLocation = True
    End If
  End If
  
End Function

Public Function IsFlowLocation(Name$, Id&) As Boolean
  
  Dim lTable As HspfTable
  Dim loper As HspfOperation
  Dim lConn As HspfConnection
  Dim i&, s$, j&, expertflag As Boolean, copyid&

  IsFlowLocation = False
  
  If Id > 0 Then
    Set loper = myUci.OpnBlks(Name).operfromid(Id)
    For j = 1 To loper.targets.Count
      Set lConn = loper.targets(j)
      If Left(lConn.Target.volname, 3) = "WDM" And _
        Trim(lConn.Target.member) = "FLOW" Then
        'this is an output flow location
        IsFlowLocation = True
      End If
    Next j
  End If
  
End Function

Private Function Reach2Copy(rchid&) As Long
  'given a reach id, find its associated copy for expert system datasets
  
  Dim loper As HspfOperation
  Dim lConn As HspfConnection
  Dim i&, s$, j&, larea!, ldsn!, copyid&

  copyid = 0
  Set loper = myUci.OpnBlks("RCHRES").operfromid(rchid)
  For j = 1 To loper.targets.Count
    Set lConn = loper.targets(j)
    If Left(lConn.Target.volname, 3) = "WDM" And _
      Trim(lConn.Target.member) = "SIMQ" Then
      'this is an expert system output locn, save area and dsn
      ldsn = lConn.Target.volid
      larea = lConn.MFact
    End If
  Next j
  
  For i = 1 To myUci.OpnSeqBlock.Opns.Count
    Set loper = myUci.OpnSeqBlock.Opn(i)
    If loper.Name = "COPY" Then
      For j = 1 To loper.targets.Count
        Set lConn = loper.targets(j)
        If Left(lConn.Target.volname, 3) = "WDM" And _
          Trim(lConn.Target.member) = "IFWO" Then
          If Abs(larea - (lConn.MFact * 12)) < 0.000001 Then
            'this appears to be the associated copy
            copyid = loper.Id
          End If
        End If
      Next j
    End If
  Next i
  Reach2Copy = copyid
End Function

Private Sub opnOutput_Click(Index As Integer)
  RefreshAll
  Call Form_Resize
End Sub

Private Function WDMInd(WDMId$) As Long
  Dim w$
  
  If Len(WDMId) > 3 Then
    w = Mid(WDMId, 4, 1)
    If w = " " Then w = "1"
  Else
    w = "1"
  End If
  WDMInd = w
End Function
