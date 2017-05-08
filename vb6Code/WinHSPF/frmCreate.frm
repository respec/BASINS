VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmCreate 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "WinHSPF - Create Project"
   ClientHeight    =   4260
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   8040
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   26
   Icon            =   "frmCreate.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4260
   ScaleWidth      =   8040
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraMet 
      Caption         =   "Initial Met Station"
      Height          =   1695
      Left            =   120
      TabIndex        =   15
      Top             =   1680
      Width           =   5532
      Begin VB.ListBox lstMet 
         BackColor       =   &H80000004&
         Enabled         =   0   'False
         Height          =   840
         Left            =   240
         TabIndex        =   16
         Top             =   360
         Width           =   5052
      End
      Begin VB.Label lblStar 
         Height          =   252
         Left            =   3480
         TabIndex        =   17
         Top             =   1420
         Width           =   1812
      End
   End
   Begin VB.CommandButton cmdOkayCancel 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Index           =   0
      Left            =   3000
      TabIndex        =   14
      Top             =   3600
      Width           =   852
   End
   Begin VB.CommandButton cmdOkayCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Index           =   1
      Left            =   4200
      TabIndex        =   13
      Top             =   3600
      Width           =   852
   End
   Begin VB.Frame fraScheme 
      Caption         =   "Model Segmentation"
      Height          =   1695
      Left            =   5760
      TabIndex        =   10
      Top             =   1680
      Width           =   2172
      Begin VB.OptionButton opnScheme 
         Caption         =   "Grouped"
         Height          =   255
         Index           =   0
         Left            =   360
         TabIndex        =   12
         ToolTipText     =   "Each Perlnd/Implnd connects to multiple Rchres"
         Top             =   600
         Value           =   -1  'True
         Width           =   1572
      End
      Begin VB.OptionButton opnScheme 
         Caption         =   "Individual"
         Height          =   255
         Index           =   1
         Left            =   360
         TabIndex        =   11
         ToolTipText     =   "Each Perlnd/Implnd connects to only one Rchres"
         Top             =   960
         Width           =   1572
      End
   End
   Begin VB.Frame fraFiles 
      Caption         =   "Files"
      Height          =   1455
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   7815
      Begin VB.ListBox lstWDM 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         Height          =   225
         Left            =   3480
         TabIndex        =   4
         Top             =   600
         Width           =   4212
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   3
         Top             =   600
         Width           =   735
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   2
         Top             =   240
         Width           =   732
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   1
         Top             =   960
         Width           =   732
      End
      Begin VB.Label lblName 
         Caption         =   "Met WDM Files"
         Height          =   255
         Index           =   4
         Left            =   1200
         TabIndex        =   9
         Top             =   600
         Width           =   1935
      End
      Begin VB.Label lblName 
         Caption         =   "Project WDM File"
         Height          =   255
         Index           =   2
         Left            =   1200
         TabIndex        =   8
         Top             =   960
         Width           =   1935
      End
      Begin VB.Label lblName 
         Caption         =   "BASINS Watershed File"
         Height          =   255
         Index           =   0
         Left            =   1200
         TabIndex        =   7
         Top             =   240
         Width           =   2175
      End
      Begin VB.Label lblFile 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   1  'Fixed Single
         Caption         =   "<none>"
         ForeColor       =   &H80000008&
         Height          =   255
         Index           =   2
         Left            =   3480
         TabIndex        =   6
         Top             =   240
         Width           =   4215
      End
      Begin VB.Label lblFile 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   1  'Fixed Single
         Caption         =   "<none>"
         ForeColor       =   &H80000008&
         Height          =   255
         Index           =   0
         Left            =   3480
         TabIndex        =   5
         Top             =   960
         Width           =   4215
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
Attribute VB_Name = "frmCreate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim WDMId$(4), MetDetails$()

Private Sub cmdFile_Click(Index As Integer)
  Dim iwdm&, i&, s$, f$, fun&, numMetSeg&, arrayMetSegs$(), wid$
  Dim tmetseg, lMetDetails$(), lMetDescs$()
  
  If Index = 0 And lblFile(0).Caption <> "<none>" Then
    Call MsgBox("Only one project WDM file may be included.", _
                vbOKOnly, "Create Project Problem")
  ElseIf Index = 1 And lstWDM.listcount > 2 Then
    Call MsgBox("No more than three Met WDM files may be included in a project.", _
                vbOKOnly, "Create Project Problem")
  Else
    If Index = 0 Then
      'project wdm file
      If FileExists(BASINSPath & "\modelout", True, False) Then
        ChDriveDir BASINSPath & "\modelout"
      End If
      CDFile.flags = &H8806&
    ElseIf Index = 1 Then
      'met wdm file
      If FileExists(BASINSPath & "\data\met_data", True, False) Then
        ChDriveDir BASINSPath & "\data\met_data"
      End If
      CDFile.flags = &H1806&
    End If
    If Index = 0 Or Index = 1 Then
      CDFile.Filter = "WDM files (*.wdm)|*.wdm"
      CDFile.Filename = "*.wdm"
      If Index = 0 Then
        CDFile.DialogTitle = "Select Project WDM File"
      Else
        CDFile.DialogTitle = "Select Met WDM File"
      End If
      On Error GoTo 40
      CDFile.CancelError = True
      CDFile.Action = 1
      If Index = 1 Then
        'met wdms
        f = CDFile.Filename
        If InList(f, lstWDM) Then
          GoTo 25
        Else
          If lstWDM.List(0) = "<none>" Then lstWDM.RemoveItem 0
          lstWDM.AddItem f
          iwdm = 0
          wid = "WDM" & lstWDM.listcount + 1
          myUci.OpenWDM iwdm, f, fun, wid
          WDMId(lstWDM.listcount) = wid
          If fun < 1 Then
            Call MsgBox("Problem opening the Met WDM file.", _
                        vbOKOnly, "Create Project Problem")
          Else
            myUci.GetMetSegNames fun, numMetSeg, arrayMetSegs, lMetDetails, lMetDescs
            If numMetSeg > 0 Then
              'add the met segs from this wdm to the list
              If lstWDM.listcount = 1 Then
                tmetseg = 0
              Else
                tmetseg = UBound(MetDetails)
              End If
              ReDim Preserve MetDetails(tmetseg + numMetSeg)
              Dim lLeadingChar As String
              For i = 0 To numMetSeg - 1
                If IsBASINSMetWDM(lMetDetails(i) & "," & wid) Then
                  'full set available here
                  lLeadingChar = "*"
                  lblStar.Caption = "* Full set available"
                Else
                  lLeadingChar = ""
                End If
                lstMet.AddItem lLeadingChar & arrayMetSegs(i) & ":" & lMetDescs(i) & " " & DateStringFromMetDetails(lMetDetails(i))
                MetDetails(tmetseg + i) = lMetDetails(i) & "," & wid
              Next i
              lstMet.Enabled = True
              lstMet.BackColor = &H80000005
              If lstMet.SelCount = 0 Then
                lstMet.Selected(0) = True
              End If
            'Else
            '  Call MsgBox("This WDM file has no location attributes.  These attributes " & vbCrLf & _
            '    "would have been updated had the associated .inf file been present.", vbOKOnly, "Met WDM Problem")
            End If
          End If
        End If
25      ' continue here on cancel
      Else
        'project wdm
        lblFile(Index).Caption = CDFile.Filename
        'does wdm exist?
        On Error GoTo 20
        Open lblFile(0).Caption For Input As #1
        'yes, it exists
        Close #1
        iwdm = 0
        GoTo 30
20      'no, it does not exist, create it
        iwdm = 2
30      'open wdm file
        wid = "WDM1"
        myUci.OpenWDM iwdm, lblFile(0).Caption, fun, wid
        If iwdm = 2 And fun < 1 Then
          Call MsgBox("Problem creating the project WDM file.", _
                        vbOKOnly, "Create Project Problem")
          lblFile(Index).Caption = "<none>"
        ElseIf iwdm = 0 And fun < 1 Then
          Call MsgBox("Problem opening the project WDM file.", _
                        vbOKOnly, "Create Project Problem")
          lblFile(Index).Caption = "<none>"
        Else
          WDMId(0) = wid
        End If
      End If
40        'continue here on cancel
    ElseIf Index = 2 Then
      If FileExists(BASINSPath & "\modelout", True, False) Then
        ChDriveDir BASINSPath & "\modelout"
      End If
      CDFile.flags = &H8806&
      CDFile.Filter = "BASINS Watershed Files (*.wsd)"
      CDFile.Filename = "*.wsd"
      CDFile.DialogTitle = "Select BASINS Watershed File"
      On Error GoTo 50
      CDFile.CancelError = True
      CDFile.Action = 1
      lblFile(Index).Caption = CDFile.Filename
50        'continue here on cancel
    End If
  End If
End Sub

Private Sub cmdOkayCancel_Click(Index As Integer)
    Dim i&, s$, wdmname$(3), outwdm$, tmpuci$, iresp&, lmetdetail$, continuefg As Boolean
    Dim lPrecDsn&, lPrecWdmid$, lId&, sjday#, ejday#, sdat&(6), edat&(6)
    Dim tmetseg As HspfMetSeg
    Dim lPetDsn&, lPetWdmid$, sjday2#, ejday2#
    
    If Index = 0 And lblFile(0) = "<none>" Then
      'no project file specified, don't allow to okay
      Call MsgBox("A project WDM file must be specified.", vbOKOnly, "WinHSPF Create Problem")
    Else
      'specifications okay, continue
      If lblFile(0) <> "<none>" Then
        outwdm = lblFile(0)
      End If
      For i = 1 To 3
        wdmname(i) = ""
      Next i
      For i = 1 To lstWDM.listcount
        If lstWDM.List(i - 1) <> "<none>" Then
          wdmname(i) = lstWDM.List(i - 1)
        End If
      Next i
      If Index = 0 Then
        'okay to create new
        If lblFile(2) <> "<none>" Then
          tmpuci = Mid(lblFile(2), 1, Len(lblFile(2)) - 3) & "uci"
          iresp = 6
          If FileExists(tmpuci) Then
            'this file already exists, warn user
            iresp = MsgBox("A uci file by this name already exists." & vbCrLf & vbCrLf & _
                    "Do you want to overwrite it?", vbExclamation + vbYesNo + vbDefaultButton1, "Create Problem")
          End If
          If iresp = 6 Then
            If lstMet.ListIndex < 0 Then
              lmetdetail = ""
            Else
              lmetdetail = MetDetails(lstMet.ListIndex)
            End If
            'check to see if using BASINS wdm
            
            continuefg = True
            If Not IsBASINSMetWDM(lmetdetail) Then
              'do window to specify met data details
              frmAddMet.Init lstMet.List(lstMet.ListIndex), 2
              frmAddMet.Show vbModal
              If myUci.MetSegs.Count = 0 Then
                'user clicked cancel
                continuefg = False
              Else
                'specified met segment
                Set tmetseg = myUci.MetSegs(1)
                lPrecDsn = tmetseg.MetSegRec(1).Source.volid
                lPrecWdmid = tmetseg.MetSegRec(1).Source.volname
                If Len(lPrecWdmid) > 0 Then
                  lId = CInt(Mid(lPrecWdmid, 4, 1))
                End If
                If lId > 0 And lPrecDsn > 0 Then
                  sjday = myUci.GetDataSetFromDsn(lId, lPrecDsn).Dates.Summary.sjday
                  ejday = myUci.GetDataSetFromDsn(lId, lPrecDsn).Dates.Summary.ejday
                End If
                lPetDsn = tmetseg.MetSegRec(7).Source.volid
                lPetWdmid = tmetseg.MetSegRec(7).Source.volname
                If Len(lPetWdmid) > 0 Then
                  lId = CInt(Mid(lPetWdmid, 4, 1))
                End If
                If lId > 0 And lPetDsn > 0 Then
                  sjday2 = myUci.GetDataSetFromDsn(lId, lPetDsn).Dates.Summary.sjday
                  ejday2 = myUci.GetDataSetFromDsn(lId, lPetDsn).Dates.Summary.ejday
                End If
                'use the common period of the prec and pet data sets
                If sjday2 > sjday Then sjday = sjday2
                If ejday2 < ejday Then ejday = ejday2
                Call J2Date(sjday, sdat)
                Call J2Date(ejday, edat)
                Call timcnv(edat)
                lmetdetail = CStr(-1 * lPrecDsn) & "," & _
                   CStr(sdat(0)) & "," & CStr(sdat(1)) & "," & CStr(sdat(2)) & "," & CStr(sdat(3)) & "," & CStr(sdat(4)) & "," & CStr(sdat(5)) & "," & _
                   CStr(edat(0)) & "," & CStr(edat(1)) & "," & CStr(edat(2)) & "," & CStr(edat(3)) & "," & CStr(edat(4)) & "," & CStr(edat(5)) & "," & _
                   lPrecWdmid
              End If
            Else
              'default first met seg from basins data
              DefaultBASINSMetseg lmetdetail
            End If
            
            If continuefg Then
              Me.MousePointer = vbHourglass
              Call HSPFMain.DoCreate(lblFile(2), outwdm, wdmname, WDMId, lmetdetail, opnScheme(0).Value)
            
              setDefault myUci, defUci
              setDefaultML myUci, defUci
              myUci.Save
              Me.MousePointer = vbNormal
              Unload Me
            End If
          End If
        Else
          Call MsgBox("User must specify a BASINS Watershed File.", _
                        vbOKOnly, "Create Project Problem")
        End If
        'add files to files block
      ElseIf Index = 1 Then 'cancel
        Unload Me
      End If
    End If
End Sub

Private Sub Form_Load()
    lstWDM.AddItem "<none>"
    'Set myUci = Nothing
    'Set myUci = New HspfUci
    myUci.HelpFile = App.HelpFile
    myUci.MsgWDMName = HSPFMain.W_HSPFMSGWDM
    myUci.MessageUnit = HSPFMain.MessageUnit
    'myUci.InitWDMArray
    Set myUci.Icon = HSPFMain.Icon
End Sub

Public Sub Init(wsdfile$)
  lblFile(2) = wsdfile
End Sub

Private Function IsBASINSMetWDM(MetDetails$)
  Dim dsn&, i&, loc$, j&, tempsj As Double, tempej As Double
  Dim sen$, con$, SDate&(6), EDate&(6), checkcount&
  Dim lts As Collection 'of atcotimser
  Dim ldate As ATCclsTserDate, sj As Double, ej As Double
  Dim llocts As Collection 'of atcotimser
  Dim delim$, quote$, basedsn&, metwdmid$, lMetDetails$, lunit&, WDMId$

  IsBASINSMetWDM = False
  lMetDetails = MetDetails
  If Len(lMetDetails) > 0 Then
    'get details from the met data details
    delim = ","
    quote = """"
    basedsn = StrSplit(lMetDetails, delim, quote)
    For i = 0 To 5
      SDate(i) = StrSplit(lMetDetails, delim, quote)
    Next i
    For i = 0 To 5
      EDate(i) = StrSplit(lMetDetails, delim, quote)
    Next i
    metwdmid = lMetDetails
    
    'look for matching WDM datasets
    Call myUci.findtimser("", "", "", lts)
    lunit = 0
    For i = 1 To lts.Count
      If lts(i).Header.Id = basedsn Then
        myUci.GetWDMIDFromUnit lts(i).File.FileUnit, WDMId
        If WDMId = metwdmid Then
          lunit = lts(i).File.FileUnit
          Exit For
        End If
      End If
    Next i
    checkcount = 0
    For i = 1 To lts.Count
      If lts(i).Header.Id = basedsn And lts(i).File.FileUnit = lunit Then
        If lts(i).Attrib("TSTYPE") = "PREC" Then
          checkcount = checkcount + 1
        End If
      ElseIf lts(i).Header.Id = basedsn + 2 And lts(i).File.FileUnit = lunit Then
        If lts(i).Attrib("TSTYPE") = "ATEM" Then
          checkcount = checkcount + 1
        End If
      ElseIf lts(i).Header.Id = basedsn + 3 And lts(i).File.FileUnit = lunit Then
        If lts(i).Attrib("TSTYPE") = "WIND" Then
          checkcount = checkcount + 1
        End If
      ElseIf lts(i).Header.Id = basedsn + 4 And lts(i).File.FileUnit = lunit Then
        If lts(i).Attrib("TSTYPE") = "SOLR" Then
          checkcount = checkcount + 1
        End If
      ElseIf lts(i).Header.Id = basedsn + 5 And lts(i).File.FileUnit = lunit Then
        If lts(i).Attrib("TSTYPE") = "PEVT" Then
          checkcount = checkcount + 1
        End If
      ElseIf lts(i).Header.Id = basedsn + 6 And lts(i).File.FileUnit = lunit Then
        If lts(i).Attrib("TSTYPE") = "DEWP" Then
          checkcount = checkcount + 1
        End If
      ElseIf lts(i).Header.Id = basedsn + 7 And lts(i).File.FileUnit = lunit Then
        If lts(i).Attrib("TSTYPE") = "CLOU" Then
          checkcount = checkcount + 1
        End If
      End If
    Next i
    If checkcount = 7 Then
      IsBASINSMetWDM = True
    End If
  End If

End Function

Private Sub DefaultBASINSMetseg(MetDetails As String)
  Dim r&, i&
  Dim SDate&(6), EDate&(6)
  Dim delim$, quote$, basedsn&, metwdmid$, lMetDetails$
  Dim lmetseg As HspfMetSeg
  
  lMetDetails = MetDetails
  If Len(lMetDetails) > 0 Then
    'get details from the met data details
    delim = ","
    quote = """"
    basedsn = StrSplit(lMetDetails, delim, quote)
    For i = 0 To 5
      SDate(i) = StrSplit(lMetDetails, delim, quote)
    Next i
    For i = 0 To 5
      EDate(i) = StrSplit(lMetDetails, delim, quote)
    Next i
    metwdmid = lMetDetails
    
    Set lmetseg = New HspfMetSeg
    Set lmetseg.Uci = myUci
    For r = 1 To 7
      lmetseg.MetSegRec(r).Source.volname = metwdmid
      lmetseg.MetSegRec(r).Sgapstrg = ""
      lmetseg.MetSegRec(r).Ssystem = "ENGL"
      lmetseg.MetSegRec(r).Tran = "SAME"
      lmetseg.MetSegRec(r).Typ = r
      Select Case r
        Case 1:
          lmetseg.MetSegRec(r).Source.volid = basedsn
          lmetseg.MetSegRec(r).Source.member = "PREC"
          lmetseg.MetSegRec(r).MFactP = 1
          lmetseg.MetSegRec(r).MFactR = 1
          lmetseg.MetSegRec(r).Sgapstrg = "ZERO"
        Case 2:
          lmetseg.MetSegRec(r).Source.volid = basedsn + 2
          lmetseg.MetSegRec(r).Source.member = "ATEM"
          lmetseg.MetSegRec(r).MFactP = 1
          lmetseg.MetSegRec(r).MFactR = 1
        Case 3:
          lmetseg.MetSegRec(r).Source.volid = basedsn + 6
          lmetseg.MetSegRec(r).Source.member = "DEWP"
          lmetseg.MetSegRec(r).MFactP = 1
          lmetseg.MetSegRec(r).MFactR = 1
        Case 4:
          lmetseg.MetSegRec(r).Source.volid = basedsn + 3
          lmetseg.MetSegRec(r).Source.member = "WIND"
          lmetseg.MetSegRec(r).MFactP = 1
          lmetseg.MetSegRec(r).MFactR = 1
        Case 5:
          lmetseg.MetSegRec(r).Source.volid = basedsn + 4
          lmetseg.MetSegRec(r).Source.member = "SOLR"
          lmetseg.MetSegRec(r).MFactP = 1
          lmetseg.MetSegRec(r).MFactR = 1
        Case 6:
          lmetseg.MetSegRec(r).Source.volid = basedsn + 7
          lmetseg.MetSegRec(r).Source.member = "CLOU"
          lmetseg.MetSegRec(r).MFactP = 0
          lmetseg.MetSegRec(r).MFactR = 1
        Case 7:
          lmetseg.MetSegRec(r).Source.volid = basedsn + 5
          lmetseg.MetSegRec(r).Source.member = "PEVT"
          lmetseg.MetSegRec(r).MFactP = 1
          lmetseg.MetSegRec(r).MFactR = 1
      End Select
    Next r
    lmetseg.ExpandMetSegName metwdmid, basedsn
    lmetseg.Id = myUci.MetSegs.Count + 1
    myUci.MetSegs.Add lmetseg
  End If
End Sub

Private Function DateStringFromMetDetails(aMetDetails As String)
    Dim lDelim As String, lQuote As String
    Dim lSdate(6) As Integer
    Dim lEdate(6) As Integer
    Dim lBaseDsn As String
    Dim i As Integer
    Dim lMetDetails As String

    lDelim = ","
    lQuote = """"
    lMetDetails = aMetDetails
    lBaseDsn = StrSplit(lMetDetails, lDelim, lQuote)
    For i = 0 To 5
      lSdate(i) = StrSplit(lMetDetails, lDelim, lQuote)
    Next i
    For i = 0 To 5
      lEdate(i) = StrSplit(lMetDetails, lDelim, lQuote)
    Next i
    DateStringFromMetDetails = "(" & lSdate(0) & "/" & lSdate(1) & "/" & lSdate(2) & "-" & lEdate(0) & "/" & lEdate(1) & "/" & lEdate(2) & ")"
End Function
