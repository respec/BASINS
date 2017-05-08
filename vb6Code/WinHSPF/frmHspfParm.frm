VERSION 5.00
Begin VB.Form frmHspfParm 
   Caption         =   "WinHSPF - HSPFParm Linkage"
   ClientHeight    =   5076
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7332
   Icon            =   "frmHspfParm.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5076
   ScaleWidth      =   7332
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   6840
      Top             =   4560
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
   End
   Begin VB.CommandButton cmdClose 
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
      Height          =   372
      Left            =   3120
      TabIndex        =   6
      Top             =   4560
      Width           =   972
   End
   Begin VB.Frame fraResults 
      Caption         =   "Assign Values From HSPFParm Report File To Starter"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3612
      Left            =   120
      TabIndex        =   1
      Top             =   840
      Width           =   7092
      Begin VB.OptionButton OptAssign 
         Caption         =   "Assign By Land Use"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Index           =   1
         Left            =   3000
         TabIndex        =   9
         Top             =   720
         Width           =   2532
      End
      Begin VB.OptionButton OptAssign 
         Caption         =   "Assign By Parameter"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Index           =   0
         Left            =   240
         TabIndex        =   8
         Top             =   720
         Value           =   -1  'True
         Width           =   2772
      End
      Begin VB.Frame fraParameter 
         BorderStyle     =   0  'None
         Caption         =   "Frame1"
         Height          =   1932
         Left            =   240
         TabIndex        =   7
         Top             =   1080
         Width           =   6612
         Begin VB.ListBox lstStarter 
            Height          =   1200
            Left            =   3480
            MultiSelect     =   1  'Simple
            TabIndex        =   11
            Top             =   360
            Width           =   2892
         End
         Begin VB.ListBox lstParms 
            Height          =   1200
            Left            =   240
            MultiSelect     =   1  'Simple
            TabIndex        =   10
            Top             =   360
            Width           =   2892
         End
         Begin VB.Label lblStarter 
            Caption         =   "To Starter Land Uses"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   7.8
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   252
            Left            =   3480
            TabIndex        =   13
            Top             =   120
            Width           =   2892
         End
         Begin VB.Label lblParm 
            Caption         =   "Parameters from HSPFParm"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   7.8
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   252
            Left            =   240
            TabIndex        =   12
            Top             =   120
            Width           =   2412
         End
      End
      Begin VB.CommandButton cmdApply 
         Caption         =   "&Apply to Starter"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   240
         TabIndex        =   5
         Top             =   3120
         Width           =   1572
      End
      Begin ATCoCtl.ATCoGrid agdStarter 
         Height          =   1932
         Left            =   240
         TabIndex        =   4
         Top             =   1080
         Width           =   6612
         _ExtentX        =   11663
         _ExtentY        =   3408
         SelectionToggle =   0   'False
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   0   'False
         Rows            =   1
         Cols            =   2
         ColWidthMinimum =   300
         gridFontBold    =   0   'False
         gridFontItalic  =   0   'False
         gridFontName    =   "MS Sans Serif"
         gridFontSize    =   8
         gridFontUnderline=   0   'False
         gridFontWeight  =   400
         gridFontWidth   =   0
         Header          =   ""
         FixedRows       =   1
         FixedCols       =   0
         ScrollBars      =   3
         SelectionMode   =   0
         BackColor       =   -2147483643
         ForeColor       =   -2147483640
         BackColorBkg    =   -2147483633
         BackColorSel    =   -2147483635
         ForeColorSel    =   -2147483634
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   -2147483630
         InsideLimitsBackground=   -2147483643
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         ComboCheckValidValues=   0   'False
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "&Open File"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   240
         TabIndex        =   2
         Top             =   360
         Width           =   1092
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   1560
         TabIndex        =   3
         Top             =   360
         Width           =   5292
      End
   End
   Begin VB.CommandButton cmdStart 
      Caption         =   "Start &HSPFParm"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Left            =   2400
      TabIndex        =   0
      Top             =   240
      Width           =   2412
   End
End
Attribute VB_Name = "frmHspfParm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim HSPFParms As Collection

Private Sub agdStarter_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim i As Long
  DoLimits
  For i = 1 To agdStarter.rows
    If agdStarter.TextMatrix(i, 1) <> "<none>" Then
      cmdApply.Enabled = True
    End If
  Next i
End Sub

Private Sub agdStarter_RowColChange()
  DoLimits
End Sub

Private Sub agdStarter_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits
End Sub

Private Sub cmdApply_Click()
  Dim defOpn As HspfOperation
  Dim ctemp As String
  Dim OpId As Long
  Dim OpType As String
  Dim Desc As String
  Dim lParm As clsHSPFParm
  Dim i As Long
  Dim j As Long
  Dim ilen As Long
  Dim ipos As Long
  
  If OptAssign(0) Then
    'by parameter
    For i = 0 To lstParms.listcount - 1
      If lstParms.Selected(i) Then
        Set lParm = HSPFParms(lstParms.ItemData(i))
        For j = 0 To lstStarter.listcount - 1
          If lstStarter.Selected(j) Then
            'set this parm in starter
            ctemp = lstStarter.List(j)
            OpType = StrRetRem(ctemp)
            OpId = CInt(StrRetRem(ctemp))
            Set defOpn = defUci.OpnBlks(OpType).operfromid(OpId)
            If defOpn.TableExists(lParm.Table) Then
              defOpn.tables(lParm.Table).Parms(lParm.Parm) = lParm.Value
            Else
              myMsgBox.Show "Table " & lParm.Table & " does not exist in the Starter UCI.", "HSPFParm Linkage Problem", "OK"
            End If
          End If
        Next j
      End If
    Next i
  Else
    'by land use
    With agdStarter
      For i = 1 To .rows
        If .TextMatrix(i, 1) <> "<none>" Then
          ctemp = .TextMatrix(i, 1)
          OpType = StrRetRem(ctemp)
          OpId = CInt(StrRetRem(ctemp))
          ilen = Len(ctemp)
          ipos = InStr(1, ctemp, "(")
          Desc = Mid(ctemp, ipos + 1, ilen - ipos - 1)
          For j = 1 To HSPFParms.Count
            Set lParm = HSPFParms(j)
            If lParm.OpType = OpType And lParm.Desc = Desc Then
              'update this parameter
              ctemp = .TextMatrix(i, 0)
              OpType = StrRetRem(ctemp)
              OpId = CInt(StrRetRem(ctemp))
              Set defOpn = defUci.OpnBlks(OpType).operfromid(OpId)
              If defOpn.TableExists(lParm.Table) Then
                defOpn.tables(lParm.Table).Parms(lParm.Parm) = lParm.Value
              Else
                myMsgBox.Show "Table " & lParm.Table & " does not exist in the Starter UCI.", "HSPFParm Linkage Problem", "OK"
              End If
            End If
          Next j
        End If
      Next i
    End With
  End If
  'now save starter uci file
  ChDriveDir HSPFMain.W_STARTERPATH
  defUci.Save
End Sub

Private Sub cmdClose_Click()
  Unload Me
End Sub

Private Sub cmdFile_Click()
  Dim ret&, ArrayIds$(), cnt&, i&
  
  If FileExists(BASINSPath & "\modelout", True, False) Then
    ChDriveDir BASINSPath & "\modelout"
  End If
  CDFile.flags = &H8806&
  CDFile.Filter = "HSPFParm Report Files (*.*)"
  CDFile.Filename = "*.*"
  CDFile.DialogTitle = "Select HSPFParm Report File"
  On Error GoTo 50
  CDFile.CancelError = True
  CDFile.Action = 1
  'read file here
  ReadReportFile CDFile.Filename, ret
  If ret = 0 Then
    lblFile.Caption = CDFile.Filename
    'fill in grid or lists
    Me.MousePointer = vbHourglass
    RefreshParms
    DefaultGrid
    Me.MousePointer = vbNormal
  End If
50        'continue here on cancel
End Sub

Private Sub cmdStart_Click()
  StartHSPFParm
End Sub

Private Sub StartHSPFParm()
  Dim HSPFParmEXE$
  Dim ff As New ATCoFindFile
'    Dim reg As New ATCoRegistry
'    HSPFParmEXE = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\HSPFParm\ExePath", "") & "\hspfparm.exe"
'    If Len(Dir(HSPFParmEXE)) = 0 Then HSPFParmEXE = PathNameOnly(App.EXEName) & "\hspfparm.exe"
'    If Len(Dir(HSPFParmEXE)) = 0 Then HSPFParmEXE = CurDir & "\hspfparm.exe"
'    If Len(Dir(HSPFParmEXE)) = 0 Then HSPFParmEXE = "c:\program files\basins\bin\hspfparm.exe"
'    If Len(Dir(HSPFParmEXE)) = 0 Then HSPFParmEXE = "c:\basins\models\HSPF\bin\hspfparm.exe"
'
'    If Len(Dir(HSPFParmEXE)) = 0 Then
'      'hspfparm not in registry
'      On Error GoTo NeverMind
'      CDFile.CancelError = True
'      CDFile.DialogTitle = "Please locate HSPFParm.exe so HSPFParm can be started."
'      CDFile.Filename = "HSPFParm.exe"
'      CDFile.ShowOpen
'      HSPFParmEXE = CDFile.Filename
'    End If
  ff.SetDialogProperties "Please locate HSPFParm.exe so HSPFParm can be started", "HSPFParm.exe"
  ff.SetRegistryInfo "HSPFParm", "files", "ExePath"
  HSPFParmEXE = ff.GetName
  If Not FileExists(HSPFParmEXE) Then
    DisableAll True
    myMsgBox.Show "WinHSPF could not find HSPFParm.exe", "Start HSPFParm Problem", "+-&Close"
    DisableAll False
  Else
    'Me.Hide
    IPC.dbg "Starting HSPFParm... " & HSPFParmEXE
    IPC.StartProcess "HSPFParm", HSPFParmEXE, 0, 864000
    IPC.dbg "Finished Running HSPFParm"
    'Me.Show
  End If
NeverMind:
End Sub

Private Sub ReadReportFile(Filename$, ret&)
  Dim lstr$, i&, ilen&, havedesc As Boolean, j&
  Dim lParm$, loptyp$, lOpId$, lDesc$, lValue$
  Dim dOpn As HspfOperation, lTable$
  Dim lparmname$(), lTabledef As HspfTableDef
  Dim tHSPFParm As clsHSPFParm
  Dim istart As Long
  Dim nparms As Long
  
  Set HSPFParms = New Collection 'of type hspfparm
  
  ret = 0
  i = FreeFile(0)
  On Error GoTo ErrHandler
  Open Filename For Input As #i
  Do Until EOF(i)
    Line Input #i, lstr
    lstr = Trim(lstr)
    ilen = Len(lstr)
    If ilen > 8 Then
      If Mid(lstr, 1, 9) = "Parameter" Then
        'process these parameter records
        Line Input #i, lstr 'blank line
        Line Input #i, lstr 'header line
        lstr = Trim(lstr)
        ilen = Len(lstr)
        If ilen > 50 Then
          'have operation description
          havedesc = True
        Else
          havedesc = False
        End If
        Line Input #i, lstr 'parameter line
        Do Until Len(Trim(lstr)) = 0
          Set tHSPFParm = New clsHSPFParm
          tHSPFParm.Parm = StrRetRem(lstr)
          tHSPFParm.Value = StrRetRem(lstr)
          tHSPFParm.OpType = StrRetRem(lstr)
          tHSPFParm.OpId = StrRetRem(lstr)
          tHSPFParm.Table = FindTableName(tHSPFParm.OpType, tHSPFParm.Parm)
          If havedesc Then
            tHSPFParm.Desc = Trim(Mid(lstr, 21))
          Else
            tHSPFParm.Desc = ""
          End If
          tHSPFParm.Id = HSPFParms.Count + 1
          HSPFParms.Add tHSPFParm
          Line Input #i, lstr 'parameter line
        Loop
      End If
    End If
    If ilen > 5 Then
      If Mid(lstr, 1, 5) = "Table" Then
        'process these table records
        lTable = Trim(Mid(lstr, 7))
        Line Input #i, lstr
        lstr = Trim(lstr)
        loptyp = Trim(Mid(lstr, 9))
        Line Input #i, lstr 'blank
        Line Input #i, lstr 'header
        If Mid(lstr, 21, 4) = "Desc" Then
          'have operation description
          havedesc = True
          istart = 40
        Else
          havedesc = False
          istart = 20
        End If
        If Mid(lstr, 21, 5) = "Occur" Or Mid(lstr, 21, 6) = "QUALID" Or Mid(lstr, 21, 4) = "GQID" Then
          'multiple occurance table, don't do for now
        Else
          'ok to continue
          ilen = Len(Trim(lstr)) - istart
          
          'nparms = Int(ilen / 10) + 1   'why assume 10 chars
          Set lTabledef = myMsg.BlockDefs(loptyp).TableDefs(lTable)
          nparms = lTabledef.ParmDefs.Count
          
          ReDim lparmname(nparms)
          lstr = Mid(lstr, istart + 1)
          For j = 1 To nparms
            'lparmname(j) = StrRetRem(lstr)
            lparmname(j) = lTabledef.ParmDefs(j).Name
          Next j
          Line Input #i, lstr 'data line
          Do Until Len(Trim(lstr)) = 0
            lOpId = Mid(lstr, 1, 5)
            If havedesc Then
              lDesc = Mid(lstr, 21, 20)
            Else
              lDesc = ""
            End If
            lstr = Mid(lstr, istart + 1)
            For j = 1 To nparms
              Set tHSPFParm = New clsHSPFParm
              tHSPFParm.Parm = lparmname(j)
              tHSPFParm.Table = lTable
              'tHSPFParm.Value = StrRetRem(lstr)
              If lTabledef.ParmDefs(j).typ <> 0 And Len(lstr) >= lTabledef.ParmDefs(j).Length Then
                tHSPFParm.Value = Mid(lstr, 1, lTabledef.ParmDefs(j).Length)
              End If
              lstr = Mid(lstr, lTabledef.ParmDefs(j).Length + 1)
              tHSPFParm.OpType = loptyp
              tHSPFParm.OpId = lOpId
              tHSPFParm.Desc = Trim(lDesc)
              tHSPFParm.Id = HSPFParms.Count + 1
              HSPFParms.Add tHSPFParm
            Next j
            Line Input #i, lstr 'data line
          Loop
        End If
      End If
    End If
  Loop
  Close #i
  Exit Sub
ErrHandler:
  Call MsgBox("Problem reading file " & Filename, , "HSPFParm Report File Problem")
  ret = 3
End Sub

Private Sub Form_Load()
  Dim i&
  Dim vOpn As Variant
  Dim lOpn As HspfOperation
  Dim lOpType As HspfOpnBlk
  Dim Desc As String
  
  With agdStarter
    .rows = 0
    .cols = 2
    .ColTitle(0) = "Starter Operation"
    .ColTitle(1) = "Mapped from HSPFParm Operation"
  End With
  If OptAssign(0) Then
    fraParameter.Visible = True
    agdStarter.Visible = False
  Else
    agdStarter.Visible = True
    fraParameter.Visible = False
  End If
  cmdApply.Enabled = False
  For i = 1 To defUci.OpnBlks.Count
    Set lOpType = defUci.OpnBlks(i)
    With agdStarter
    For Each vOpn In lOpType.Ids
      Set lOpn = vOpn
      .rows = .rows + 1
      Desc = lOpn.Description
      .TextMatrix(.rows, 0) = lOpn.Name & " " & lOpn.Id & " (" & Desc & ")"
      .TextMatrix(.rows, 1) = "<none>"
    Next vOpn
    End With
  Next i
End Sub

Private Sub Form_Resize()
  If width > 2500 Then
    fraResults.width = width - 400
    agdStarter.width = fraResults.width - 600
    cmdStart.Left = width / 2 - (cmdStart.width / 2)
    cmdClose.Left = width / 2 - (cmdClose.width / 2)
    fraParameter.width = fraResults.width - 480
    lstParms.width = (fraResults.width / 2) - (240 * 3)
    lstStarter.width = lstParms.width
    lstStarter.Left = fraParameter.width - 240 - lstStarter.width
    lblStarter.Left = lstStarter.Left
    lblFile.width = fraParameter.width - cmdFile.width - 500
  End If
  If height > 4000 Then
    fraResults.height = height - 1788
    cmdClose.Top = height - 840
    cmdApply.Top = fraResults.height - 500
    fraParameter.height = fraResults.height - 1680
    lstParms.height = fraParameter.height - 540
    lstStarter.height = fraParameter.height - 540
    agdStarter.height = fraParameter.height
  End If
End Sub

Private Sub lstParms_Click()
  Dim lopname$
  Dim lOpType As HspfOpnBlk
  Dim vOpn As Variant
  Dim lOpn As HspfOperation
  Dim tempOpname$
  Dim Desc As String
  Dim i As Long
  
  If lstParms.SelCount > 0 Then
    If lstParms.SelCount > 1 Then
      tempOpname = ""
      For i = 1 To lstParms.listcount
        If lstParms.Selected(i - 1) Then
          If tempOpname = "" Then
            tempOpname = HSPFParms(lstParms.ItemData(i - 1)).OpType
          ElseIf tempOpname <> HSPFParms(lstParms.ItemData(i - 1)).OpType Then
            'unselect operations if different oper names
            lstParms.Selected(i - 1) = False
          End If
        End If
      Next i
    End If
    
    lstStarter.Clear
    For i = 1 To lstParms.listcount
      If lstParms.Selected(i - 1) Then
        lopname = HSPFParms(lstParms.ItemData(i - 1)).OpType
        If defUci.OpnBlks(lopname).Count > 0 Then
          Set lOpType = defUci.OpnBlks(lopname)
          For Each vOpn In lOpType.Ids
            Set lOpn = vOpn
            Desc = lOpn.Description
            lstStarter.AddItem lOpn.Name & " " & lOpn.Id & " - " & lOpn.Description
          Next vOpn
          Exit For
        End If
      End If
    Next i
  Else
    lstStarter.Clear
  End If
End Sub

Private Sub lstStarter_Click()
  cmdApply.Enabled = True
End Sub

Private Sub OptAssign_Click(Index As Integer)
  Dim i As Long
  cmdApply.Enabled = False
  If OptAssign(0) Then
    fraParameter.Visible = True
    agdStarter.Visible = False
    If lstStarter.SelCount > 0 Then
      cmdApply.Enabled = True
    End If
  Else
    agdStarter.Visible = True
    fraParameter.Visible = False
    For i = 1 To agdStarter.rows
      If agdStarter.TextMatrix(i, 1) <> "<none>" Then
        cmdApply.Enabled = True
      End If
    Next i
  End If
End Sub

Private Sub RefreshParms()
  Dim vParm As Variant, lParm As clsHSPFParm
  Dim ctemp As String
  
  lstParms.Clear
  For Each vParm In HSPFParms
    Set lParm = vParm
    ctemp = lParm.Parm & " = " & lParm.Value & " (" & lParm.OpType & " " & lParm.OpId
    If Len(lParm.Desc) > 0 Then
      ctemp = ctemp & " - " & lParm.Desc & ")"
    Else
      ctemp = ctemp & ")"
    End If
    If Not InList(ctemp, lstParms) Then
      lstParms.AddItem ctemp
      lstParms.ItemData(lstParms.listcount - 1) = lParm.Id
    End If
  Next vParm
End Sub

Private Sub DefaultGrid()
  Dim lOpType As HspfOpnBlk
  Dim vOpn As Variant
  Dim lOpn As HspfOperation
  Dim vParm As Variant, lParm As clsHSPFParm
  Dim ctemp As String, i&, row&
    
  row = 0
  For i = 1 To defUci.OpnBlks.Count
    Set lOpType = defUci.OpnBlks(i)
    For Each vOpn In lOpType.Ids
      Set lOpn = vOpn
      row = row + 1
      For Each vParm In HSPFParms
        Set lParm = vParm
        If lParm.OpType = lOpn.Name And UCase(lParm.Desc) = UCase(lOpn.Description) Then
          'matching land use name
          ctemp = vParm.OpType & " " & vParm.OpId & " (" & vParm.Desc & ")"
          agdStarter.TextMatrix(row, 1) = ctemp
        End If
      Next vParm
    Next vOpn
  Next i

End Sub

Private Function FindTableName(lOpType$, lparmname$) As String
  Dim i&, vTable As Variant, lTable As HspfTableDef
  Dim vParm As Variant
  
  FindTableName = ""
  For Each vTable In myMsg.BlockDefs(lOpType).TableDefs
    Set lTable = vTable
    For Each vParm In lTable.ParmDefs
      If vParm.Name = lparmname Then
        FindTableName = lTable.Name
        Exit For
      End If
    Next vParm
    If Len(FindTableName) > 0 Then Exit For
  Next vTable
End Function

Private Sub DoLimits()
  Dim vHSPFParm As Variant, tHSPFParm As clsHSPFParm
  Dim ctemp$, listcount&, alist$(), ifound As Boolean
  Dim i As Long
  
  With agdStarter
    .ClearValues
    If .col = 1 Then
      If Not HSPFParms Is Nothing Then
        .ColEditable(1) = True
        .AddValue "<none>"
        listcount = 0
        For Each vHSPFParm In HSPFParms
          Set tHSPFParm = vHSPFParm
          If tHSPFParm.OpType = Mid(.TextMatrix(.row, 0), 1, 6) Then
            ctemp = tHSPFParm.OpType & " " & tHSPFParm.OpId & " (" & tHSPFParm.Desc & ")"
            If listcount = 0 Then
              listcount = listcount + 1
              ReDim Preserve alist(listcount)
              alist(listcount) = ctemp
            Else
              For i = 1 To listcount
                ifound = False
                If alist(i) = ctemp Then
                  ifound = True
                End If
              Next i
              If ifound = False Then
                listcount = listcount + 1
                ReDim Preserve alist(listcount)
                alist(listcount) = ctemp
              End If
            End If
          End If
        Next vHSPFParm
        For i = 1 To listcount
          .AddValue alist(i)
        Next i
      End If
    End If
  End With
End Sub
