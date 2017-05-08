VERSION 5.00
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.10#0"; "ATCoCtl.ocx"
Begin VB.Form frmAddExpert 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "WinHSPF - Add Output"
   ClientHeight    =   4704
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   5520
   HelpContextID   =   41
   Icon            =   "frmAddExpert.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4704
   ScaleWidth      =   5520
   StartUpPosition =   2  'CenterScreen
   Begin VB.OptionButton optHD 
      Caption         =   "Daily"
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
      Left            =   4320
      TabIndex        =   12
      Top             =   3720
      Value           =   -1  'True
      Visible         =   0   'False
      Width           =   972
   End
   Begin VB.OptionButton optHD 
      Caption         =   "Hourly"
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
      Left            =   4320
      TabIndex        =   11
      Top             =   3480
      Visible         =   0   'False
      Width           =   972
   End
   Begin VB.TextBox txtDesc 
      BackColor       =   &H80000004&
      Height          =   525
      Left            =   240
      MultiLine       =   -1  'True
      TabIndex        =   10
      Top             =   2760
      Width           =   5055
   End
   Begin VB.ListBox lstMember 
      Height          =   1776
      Left            =   2880
      TabIndex        =   8
      Top             =   600
      Width           =   2415
   End
   Begin ATCoCtl.ATCoText atxBase 
      Height          =   255
      Left            =   2640
      TabIndex        =   7
      Top             =   3840
      Width           =   1455
      _ExtentX        =   2561
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   32000
      HardMin         =   1
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   -999
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   "1000"
      Value           =   "1000"
      Enabled         =   -1  'True
   End
   Begin VB.TextBox txtLoc 
      Height          =   285
      Left            =   2640
      TabIndex        =   6
      Top             =   3480
      Width           =   1455
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "&Cancel"
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
      Left            =   2880
      TabIndex        =   3
      Top             =   4200
      Width           =   1095
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "&OK"
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
      Index           =   0
      Left            =   1560
      TabIndex        =   2
      Top             =   4200
      Width           =   1095
   End
   Begin VB.ListBox lstLocations 
      Height          =   1776
      Left            =   240
      TabIndex        =   1
      Top             =   600
      Width           =   2415
   End
   Begin VB.Label lblMember 
      Caption         =   "Group/Member:"
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
      Top             =   240
      Width           =   2415
   End
   Begin VB.Label lblLoc 
      Alignment       =   1  'Right Justify
      Caption         =   "Base WDM DSN:"
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
      Left            =   960
      TabIndex        =   5
      Top             =   3840
      Width           =   1575
   End
   Begin VB.Label lblLoc 
      Alignment       =   1  'Right Justify
      Caption         =   "WDM Location ID:"
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
      Left            =   840
      TabIndex        =   4
      Top             =   3480
      Width           =   1695
   End
   Begin VB.Label lblLocation 
      Caption         =   "Operation:"
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
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   2415
   End
End
Attribute VB_Name = "frmAddExpert"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim UserOption

Public Sub Init(opt&)
  UserOption = opt
  '1 - calib locations
  '2 - flow locations
  '3 - other
  '4 - copy all
  '5 - aquatox link
  If UserOption = 4 Then
    lblMember = "To Operation"
    lblLocation = "From Operation"
    frmAddExpert.Caption = "WinHSPF - Copy Output"
  Else
    lblLocation = "Operation:"
    lblMember = "Group/Member:"
    frmAddExpert.Caption = "WinHSPF - Add Output"
  End If
End Sub

Private Sub cmdOpt_Click(Index As Integer)
  Dim adsn&(28), Id&, ostr$(28), copyid&, ContribArea!
  Dim lTable As HspfTable, WDMId&, newdsn&, spacepos&, parenpos&
  Dim opname$, tmem$, group$, mem$, colonpos&, crem$, commapos&, sub1&, sub2&
  Dim ctemp$, ctxt$, found As Boolean, gqualfg&(3), tempmem$
  Dim fromOper As HspfOperation
  Dim toOper As HspfOperation
  Dim lOper As HspfOperation
  Dim vConn As Variant, vToConn As Variant
  Dim lConn As HspfConnection, lToConn As HspfConnection
  Dim fromcalib As Boolean, outtu As Long
  Dim member$(28), msub1&(28), tgroup$(28), lTrans As String
                          
  If Index = 0 Then
    'ok
    If optHD(0).Value = True Then
      outtu = 3  'hourly
    Else
      outtu = 4  'daily
    End If
    If UserOption = 1 Then 'add calib location
      If lstLocations.ListIndex > -1 Then
        Id = lstLocations.ItemData(lstLocations.ListIndex)
        If Not frmOutput.IsCalibLocation("RCHRES", (Id)) Then
          Me.MousePointer = vbHourglass
          'add data sets
          myUci.AddExpertDsns Id, txtLoc.Text, atxBase.Value, adsn, ostr
          'add to copy block
          copyid = 1
          myUci.AddOperation "COPY", copyid
          myUci.AddTable "COPY", copyid, "TIMESERIES"
          Set lTable = myUci.OpnBlks("COPY").operfromid(copyid).tables("TIMESERIES")
          lTable.Parms("NMN") = 7
          'add to opn seq block
          myUci.OpnSeqBlock.Add myUci.OpnBlks("COPY").operfromid(copyid)
          'add to ext targets block
          ContribArea = myUci.UpstreamArea(Id)
          myUci.AddExpertExtTargets Id, copyid, ContribArea, adsn, ostr
          'add mass-link and schematic copy records
          myUci.AddExpertSchematic Id, copyid
          myUci.Edited = True
          Me.MousePointer = vbNormal
        End If
      End If
    ElseIf UserOption = 2 Then 'add flow location
      If lstLocations.ListIndex > -1 Then
        Id = lstLocations.ItemData(lstLocations.ListIndex)
        myUci.AddOutputWDMDataSetExt txtLoc.Text, "FLOW", atxBase.Value, WDMId, outtu, "", newdsn
        myUci.AddExtTarget "RCHRES", Id, "HYDR", "RO", 1, 1, 1#, "AVER", _
               "WDM" & CStr(WDMId), newdsn, "FLOW", 1, "ENGL", "AGGR", "REPL"
        myUci.Edited = True
      End If
    ElseIf UserOption = 3 Then 'add this other output
      If lstLocations.ListIndex > -1 And lstMember.ListIndex > -1 Then
        spacepos = InStr(1, lstLocations.List(lstLocations.ListIndex), " ")
        parenpos = InStr(1, lstLocations.List(lstLocations.ListIndex), "(")
        If spacepos > 0 And parenpos > 0 Then
          'parse operation name and id
          Id = CInt(Mid(lstLocations.List(lstLocations.ListIndex), spacepos, parenpos - spacepos))
          opname = Mid(lstLocations.List(lstLocations.ListIndex), 1, spacepos - 1)
          If Id > 0 And Len(opname) > 0 Then
            'parse member name
            tmem = lstMember.List(lstMember.ListIndex)
            parenpos = InStr(1, tmem, "(")
            colonpos = InStr(1, tmem, ":")
            If parenpos > 0 Then 'has subscripts
              crem = Mid(tmem, parenpos)
              commapos = InStr(1, crem, ",")
              If commapos > 0 Then
                sub1 = CInt(Mid(crem, 2, commapos - 2))
                sub2 = CInt(Mid(crem, commapos + 1, Len(crem) - commapos - 1))
              Else
                sub1 = CInt(Mid(crem, 2, Len(crem) - 1))
                sub2 = 1
              End If
              mem = Mid(tmem, colonpos + 1, parenpos - colonpos - 1)
              group = Mid(tmem, 1, colonpos - 1)
              tempmem = mem & CStr(sub1)
              If TSMaxSubscript(2, group, mem) > 1 Then
                tempmem = tempmem & CStr(sub2)
              End If
            Else
              sub1 = 1
              sub2 = 1
              mem = Mid(tmem, colonpos + 1)
              tempmem = mem
            End If
            group = Mid(tmem, 1, colonpos - 1)
            'now add the data set
            If outtu = 3 Then
              'hourly, use blank transform
              lTrans = "    "
            Else
              lTrans = "AVER"
            End If
            myUci.AddOutputWDMDataSetExt txtLoc.Text, tempmem, atxBase.Value, WDMId, outtu, "", newdsn
            myUci.AddExtTarget opname, Id, group, mem, sub1, sub2, 1#, lTrans, _
                 "WDM" & CStr(WDMId), newdsn, tempmem, 1, "ENGL", "AGGR", "REPL"
            myUci.Edited = True
          End If
        End If
      Else
        Call MsgBox("An operation and group/member must be selected.", vbOKOnly, "Add Output Problem")
      End If
    ElseIf UserOption = 4 Then 'copy
      If lstMember.listcount > 0 And lstMember.ListIndex > -1 Then
        'something selected in both lists
        ctemp = lstLocations.List(lstLocations.ListIndex)
        spacepos = InStr(1, ctemp, " ")
        opname = Mid(ctemp, 1, spacepos - 1)
        Id = CInt(Mid(ctemp, spacepos + 1))
        Set fromOper = myUci.OpnBlks(opname).operfromid(Id)
        ctemp = lstMember.List(lstMember.ListIndex)
        spacepos = InStr(1, ctemp, " ")
        opname = Mid(ctemp, 1, spacepos - 1)
        Id = CInt(Mid(ctemp, spacepos + 1))
        Set toOper = myUci.OpnBlks(opname).operfromid(Id)
        If fromOper.Name = "RCHRES" Then
          fromcalib = frmOutput.IsCalibLocation(fromOper.Name, fromOper.Id)
        Else
          fromcalib = False
        End If
        For Each vConn In fromOper.targets
          Set lConn = vConn
          If Mid(lConn.Target.volname, 1, 3) = "WDM" Then
            If lConn.Source.volname = "COPY" Then
              'assume this is a calibration location, skip it
            Else
              'this is an output from this operation, copy it
              If fromcalib And lConn.Source.group = "ROFLOW" And lConn.Source.member = "ROVOL" Then
              Else
                'make sure we do not already have this output
                found = False
                For Each vToConn In toOper.targets
                  Set lToConn = vToConn
                  If lToConn.Source.group = lConn.Source.group And _
                     lToConn.Source.member = lConn.Source.member Then
                    found = True
                  End If
                Next vToConn
                If found = False Then
                  'now add it
                  Call myUci.AddOutputWDMDataSet(txtLoc.Text, lConn.Target.member, atxBase.Value, WDMId, newdsn)
                  myUci.AddExtTarget toOper.Name, toOper.Id, lConn.Source.group, lConn.Source.member, _
                     lConn.Source.memsub1, lConn.Source.memsub2, lConn.MFact, lConn.Tran, _
                     "WDM" & CStr(WDMId), newdsn, lConn.Target.member, lConn.Target.memsub1, _
                     lConn.Ssystem, lConn.Sgapstrg, lConn.Amdstrg
                  myUci.Edited = True
                End If
              End If
            End If
          End If
        Next vConn
      Else
        MsgBox "An operation must be selected in the 'To Operation' list.", vbOKOnly, "Output Manager Problem"
      End If
    ElseIf UserOption = 5 Then 'add aquatox linkage
      If lstLocations.ListIndex > -1 Then
        Id = lstLocations.ItemData(lstLocations.ListIndex)
        If Not frmOutput.IsAQUATOXLocation("RCHRES", (Id)) Then
          'make sure required sections are on
          Set lOper = myUci.OpnBlks("RCHRES").operfromid(Id)
          Set lTable = lOper.tables("ACTIVITY")
          If lTable.Parms(1).Value = 1 And lTable.Parms(4).Value = 1 And _
             lTable.Parms(5).Value = 1 And lTable.Parms(7).Value = 1 And _
             lTable.Parms(8).Value = 1 Then
            'all required rchres sections are on
            '(hydr, htrch, sedtrn, oxrx, nutrx)
            CheckGQUALs Id, gqualfg
            Me.MousePointer = vbHourglass
            'add data sets
            myUci.AddAQUATOXDsnsExt Id, txtLoc.Text, atxBase.Value, _
                                    lTable.Parms(9).Value, gqualfg, _
                                    WDMId, member, msub1, tgroup, adsn, ostr, outtu
            'add to ext targets block
            myUci.AddAQUATOXExtTargetsExt Id, WDMId, member, msub1, tgroup, adsn, ostr, outtu
            myUci.Edited = True
            Me.MousePointer = vbNormal
          Else
            ctemp = ""
            If lTable.Parms(1).Value = 0 Then
              ctemp = ctemp & "HYDR "
            End If
            If lTable.Parms(4).Value = 0 Then
              ctemp = ctemp & "HTRCH "
            End If
            If lTable.Parms(5).Value = 0 Then
              ctemp = ctemp & "SEDTRN "
            End If
            If lTable.Parms(7).Value = 0 Then
              ctemp = ctemp & "OXRX "
            End If
            If lTable.Parms(8).Value = 0 Then
              ctemp = ctemp & "NUTRX "
            End If
            myMsgBox.Show "The following required sections are not on: " & ctemp, "AQUATOX Linkage Problem", "OK"
          End If
        End If
      End If
    End If
    frmOutput.RefreshAll
  Else
    'cancel
  End If
  Unload Me
End Sub

Private Sub Form_Load()
  Dim lOper As HspfOperation
  Dim i&, S$
  
  If UserOption = 3 Then 'other types
    lstLocations.Width = 2415
    lstMember.Visible = False
    lblMember.Visible = False
    txtDesc.Visible = True
    For i = 1 To myUci.OpnSeqBlock.Opns.Count
      Set lOper = myUci.OpnSeqBlock.Opn(i)
      If lOper.Name = "RCHRES" Or lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Or lOper.Name = "BMPRAC" Then
        S = lOper.Name & " " & lOper.Id & " (" & _
          lOper.Description & ")"
        lstLocations.AddItem S
        lstLocations.ItemData(lstLocations.listcount - 1) = lOper.Id
      End If
    Next i
  ElseIf UserOption = 1 Or UserOption = 2 Or UserOption = 5 Then 'calib or flow or aquatox
    lstLocations.Width = 5055
    lstLocations.Height = lstLocations.Height + 600
    lstMember.Visible = False
    lblMember.Visible = False
    txtDesc.Visible = False
    If UserOption = 5 Then
      'aquatox type
      txtLoc.Locked = True
    End If
    For i = 1 To myUci.OpnSeqBlock.Opns.Count
      Set lOper = myUci.OpnSeqBlock.Opn(i)
      If lOper.Name = "RCHRES" Then
        S = lOper.Name & " " & lOper.Id & " (" & _
          lOper.Description & ")"
        lstLocations.AddItem S
        lstLocations.ItemData(lstLocations.listcount - 1) = lOper.Id
      End If
    Next i
  ElseIf UserOption = 4 Then 'copy
    lstLocations.Clear
    lstLocations.Width = 2415
    lstMember.Visible = False
    lblMember.Visible = False
    txtDesc.Visible = False
    For i = 1 To frmOutput.agdOutput.rows
      If Not InList(frmOutput.agdOutput.TextMatrix(i, 0), lstLocations) Then
        lstLocations.AddItem frmOutput.agdOutput.TextMatrix(i, 0)
      End If
    Next i
  End If
  If UserOption = 2 Or UserOption = 3 Or UserOption = 5 Then
    'give option of output timeunits if hourly run, otherwise always daily
    If myUci.OpnSeqBlock.Delt <= 60 Then
      optHD(0).Visible = True
      optHD(1).Visible = True
    End If
  End If
  
  txtLoc.Text = "<none>"
End Sub

Private Sub lstLocations_Click()
  Dim Id&, spacepos&, parenpos&, opname$
  Dim lOper As HspfOperation, lsub$
  Dim cpresent As Collection 'of HspfStatusType
  Dim ctimser As Collection 'of HspfStatusType
  Dim vTimser As Variant, lTimser As HspfStatusType
  Dim ctemp$, i&, exttarget As Boolean
  Dim vOper As Variant
  Dim lOpnBlk As HspfOpnBlk
  Dim vConn As Variant, lConn As HspfConnection
  
  If UserOption = 1 Or UserOption = 2 Or UserOption = 5 Then
    Id = lstLocations.ItemData(lstLocations.ListIndex)
    txtLoc.Text = "RCH" & CStr(Id)
  ElseIf UserOption = 3 Then
    txtDesc.Text = ""
    spacepos = InStr(1, lstLocations.List(lstLocations.ListIndex), " ")
    parenpos = InStr(1, lstLocations.List(lstLocations.ListIndex), "(")
    If spacepos > 0 And parenpos > 0 Then
      Id = CInt(Mid(lstLocations.List(lstLocations.ListIndex), spacepos, parenpos - spacepos))
      opname = Mid(lstLocations.List(lstLocations.ListIndex), 1, spacepos - 1)
      If Id > 0 And Len(opname) > 0 Then
        Set lOper = myUci.OpnBlks(opname).operfromid(Id)
        If Not lOper Is Nothing Then
          If opname = "PERLND" Then
            txtLoc.Text = "P" & CStr(Id)
          ElseIf opname = "IMPLND" Then
            txtLoc.Text = "I" & CStr(Id)
          ElseIf opname = "RCHRES" Then
            txtLoc.Text = "RCH" & CStr(Id)
          ElseIf opname = "BMPRAC" Then
            txtLoc.Text = "BMP" & CStr(Id)
          End If
          'get possible timsers from edit operation method
          lOper.OutputTimeseriesStatus.UpdateExtTargetsOutputs
          Set ctimser = lOper.OutputTimeseriesStatus.GetOutputInfo(2)
          lstMember.Clear
          For Each vTimser In ctimser
            Set lTimser = vTimser
            exttarget = False
            If lTimser.Present Then
              'see if used as external target
              For Each vConn In lOper.targets
                Set lConn = vConn
                If UCase(Mid(lConn.Target.volname, 1, 3)) = "WDM" Then
                  'used as ext target, don't add
                  i = InStr(1, lTimser.Name, ":")
                  If i > 0 Then
                    If lConn.Source.group = Mid(lTimser.Name, 1, i - 1) Then
                      If lConn.Source.member = Mid(lTimser.Name, i + 1) Then
                        If lTimser.Max > 1 Then
                          If lConn.Source.memsub1 = lTimser.Occur Mod 1000 Then
                            If lConn.Source.memsub2 = CLng((1 + (lTimser.Occur) / 1000)) Then
                              exttarget = True
                            End If
                          End If
                        Else
                          exttarget = True
                        End If
                      End If
                    End If
                  End If
                End If
              Next vConn
            End If
            If Not exttarget Then
              If lTimser.Max = 1 Then
                lstMember.AddItem lTimser.Name
              Else
                lsub = "(" & lTimser.Occur Mod 1000 & "," & CLng(1 + (lTimser.Occur) / 1000) & ")"
                lstMember.AddItem lTimser.Name & lsub
              End If
            End If
          Next vTimser
          lstMember.Visible = True
          lblMember.Visible = True
        End If
      End If
    End If
  ElseIf UserOption = 4 Then 'copy
    ctemp = lstLocations.List(lstLocations.ListIndex)
    spacepos = InStr(1, ctemp, " ")
    opname = Mid(ctemp, 1, spacepos - 1)
    Id = CInt(Mid(ctemp, spacepos + 1))
    lstMember.Clear
    Set lOpnBlk = myUci.OpnBlks(opname)
    For i = 1 To lOpnBlk.Count
      Set lOper = lOpnBlk.NthOper(i)
      If lOper.Id <> Id Then
        lstMember.AddItem lOper.Name & " " & lOper.Id
      End If
    Next i
    If lstMember.listcount > 0 Then
      lstMember.Visible = True
      lblMember.Visible = True
    Else
      lstMember.Visible = False
    End If
  End If
End Sub

Private Sub lstMember_Click()
  Dim spacepos&, parenpos&, Id&, opname$, colonpos&
  Dim lOper As HspfOperation, group$, mem$, i&
  Dim lGroupDef As HspfTSGroupDef
  
  spacepos = InStr(1, lstMember.List(lstMember.ListIndex), " ")
  parenpos = InStr(1, lstMember.List(lstMember.ListIndex), "(")
  If parenpos = 0 Then parenpos = Len(lstMember.List(lstMember.ListIndex)) + 1
  If spacepos > 0 And parenpos > 0 Then
    Id = CInt(Mid(lstMember.List(lstMember.ListIndex), spacepos, parenpos - spacepos))
    opname = Mid(lstMember.List(lstMember.ListIndex), 1, spacepos - 1)
    If Id > 0 And Len(opname) > 0 Then
      If opname = "PERLND" Then
        txtLoc.Text = "P" & CStr(Id)
      ElseIf opname = "IMPLND" Then
        txtLoc.Text = "I" & CStr(Id)
      ElseIf opname = "RCHRES" Then
        txtLoc.Text = "RCH" & CStr(Id)
      ElseIf opname = "BMPRAC" Then
        txtLoc.Text = "BMP" & CStr(Id)
      End If
    End If
  End If
  'set desc text
  spacepos = InStr(1, lstLocations.List(lstLocations.ListIndex), " ")
  parenpos = InStr(1, lstLocations.List(lstLocations.ListIndex), "(")
  If spacepos > 0 And parenpos > 0 Then
    Id = CInt(Mid(lstLocations.List(lstLocations.ListIndex), spacepos, parenpos - spacepos))
    opname = Mid(lstLocations.List(lstLocations.ListIndex), 1, spacepos - 1)
    If Id > 0 And Len(opname) > 0 Then
      Set lOper = myUci.OpnBlks(opname).operfromid(Id)
      colonpos = InStr(1, lstMember.List(lstMember.ListIndex), ":")
      If colonpos > 0 Then
        group = Mid(lstMember.List(lstMember.ListIndex), 1, colonpos - 1)
        parenpos = InStr(1, lstMember.List(lstMember.ListIndex), "(")
        If parenpos > 0 Then
          mem = Mid(lstMember.List(lstMember.ListIndex), colonpos + 1, parenpos - colonpos - 1)
        Else
          mem = Mid(lstMember.List(lstMember.ListIndex), colonpos + 1)
        End If
        For i = 1 To myUci.Msg.TSGroupDefs.Count
          If myUci.Msg.TSGroupDefs(i).Name = group Then
            Set lGroupDef = myUci.Msg.TSGroupDefs(i)
            'now set the text
            txtDesc = group & ":" & mem & " - " & lGroupDef.memberdefs(mem).Defn
            If myUci.GlobalBlock.emfg = 1 Then
              txtDesc = txtDesc & " (" & lGroupDef.memberdefs(mem).eunits & ")"
            Else
              txtDesc = txtDesc & " (" & lGroupDef.memberdefs(mem).munits & ")"
            End If
            Exit For
          End If
        Next i
      End If
    End If
  End If
End Sub

Private Sub CheckGQUALs(Id&, gqualfg&())
  Dim lTable As HspfTable
  Dim lOper As HspfOperation
  Dim ngqual&, itemp&, i&, tname$, iresp&
  Dim vOpn As Variant, lOpn As HspfOperation
  
  Set lOper = myUci.OpnBlks("RCHRES").operfromid(Id)
  Set lTable = lOper.tables("ACTIVITY")
  If lTable.Parms(6).Value = 1 Then
    'gqual on
    'figure out how many gquals
    ngqual = 0
    For Each vOpn In myUci.OpnBlks("RCHRES").Ids
      Set lOpn = vOpn
      If lOpn.TableExists("GQ-QALDATA") Then
        If lOpn.TableExists("GQ-GENDATA") Then
          itemp = lOpn.tables("GQ-GENDATA").Parms("NGQUAL")
        Else
          itemp = 1
        End If
        If itemp > ngqual Then
          ngqual = itemp
        End If
      End If
    Next vOpn
    ReDim qname(ngqual)
    For i = 1 To ngqual
      If i = 1 Then
        tname = "GQ-QALDATA"
      Else
        tname = "GQ-QALDATA:" & i
      End If
      qname(i) = ""
      For Each vOpn In myUci.OpnBlks("RCHRES").Ids
        Set lOpn = vOpn
        If lOpn.TableExists(tname) And Len(qname(i)) = 0 Then
          qname(i) = Trim(lOpn.tables(tname).Parms("GQID").Value)
        End If
      Next vOpn
      iresp = myMsgBox.Show("Do you want to include the total inflow of GQUAL " & i & _
        " (" & qname(i) & ") in the AQUATOX Linkage?", "AQUATOX Query", "+&Yes", "&No")
      If iresp = 1 Then
        gqualfg(i) = 1
      Else
        gqualfg(i) = 0
      End If
    Next i
  Else
    gqualfg(1) = 0
    gqualfg(2) = 0
    gqualfg(3) = 0
  End If
End Sub
