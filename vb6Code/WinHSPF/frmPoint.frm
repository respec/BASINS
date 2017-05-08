VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Begin VB.Form frmPoint 
   Caption         =   "WinHSPF - Point Sources"
   ClientHeight    =   6660
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   7776
   HelpContextID   =   39
   Icon            =   "frmPoint.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6660
   ScaleWidth      =   7776
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraSash 
      BackColor       =   &H8000000C&
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   132
      Left            =   120
      MousePointer    =   7  'Size N S
      TabIndex        =   10
      Top             =   3120
      Width           =   7572
   End
   Begin VB.CommandButton cmdScenario 
      Caption         =   "Create Scenario"
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
      Left            =   4080
      TabIndex        =   8
      Top             =   2640
      Width           =   1572
   End
   Begin VB.CheckBox chkDetails 
      Caption         =   "Show Details"
      Enabled         =   0   'False
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
      Left            =   120
      TabIndex        =   7
      Top             =   2640
      Width           =   1812
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "Add New"
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
      Index           =   0
      Left            =   2160
      TabIndex        =   6
      Top             =   2640
      Width           =   1572
   End
   Begin ATCoCtl.ATCoSelectList aslPoint 
      Height          =   2412
      Left            =   120
      TabIndex        =   5
      Top             =   120
      Width           =   8052
      _ExtentX        =   14203
      _ExtentY        =   4255
      RightLabel      =   "In Use:"
      LeftLabel       =   "Available:"
   End
   Begin VB.CommandButton cmdPoint 
      Cancel          =   -1  'True
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
      Left            =   4080
      TabIndex        =   3
      Top             =   6120
      Width           =   1335
   End
   Begin VB.CommandButton cmdPoint 
      Caption         =   "&OK"
      Default         =   -1  'True
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
      Left            =   2400
      TabIndex        =   2
      Top             =   6120
      Width           =   1335
   End
   Begin VB.Frame fraReach 
      Caption         =   "Details"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2772
      Left            =   120
      TabIndex        =   0
      Top             =   3240
      Width           =   7572
      Begin ATCoCtl.ATCoGrid agdMasterPoint 
         Height          =   1332
         Left            =   3720
         TabIndex        =   9
         Top             =   1320
         Visible         =   0   'False
         Width           =   3732
         _ExtentX        =   6583
         _ExtentY        =   2350
         SelectionToggle =   0   'False
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   0   'False
         Rows            =   2
         Cols            =   4
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
      Begin MSComctlLib.ImageList imgView 
         Left            =   6960
         Top             =   240
         _ExtentX        =   804
         _ExtentY        =   804
         BackColor       =   -2147483643
         ImageWidth      =   32
         ImageHeight     =   32
         MaskColor       =   12632256
         _Version        =   393216
         BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
            NumListImages   =   3
            BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmPoint.frx":08CA
               Key             =   "Graph"
               Object.Tag             =   "Graph"
            EndProperty
            BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmPoint.frx":0D1C
               Key             =   "List"
               Object.Tag             =   "List"
            EndProperty
            BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmPoint.frx":116E
               Key             =   "Delete"
               Object.Tag             =   "Delete"
            EndProperty
         EndProperty
      End
      Begin MSComctlLib.Toolbar tbrView 
         Height          =   516
         Left            =   6960
         TabIndex        =   4
         Top             =   240
         Width           =   492
         _ExtentX        =   868
         _ExtentY        =   910
         ButtonWidth     =   487
         ButtonHeight    =   826
         Appearance      =   1
         _Version        =   393216
      End
      Begin ATCoCtl.ATCoGrid agdPoint 
         Height          =   2292
         Left            =   120
         TabIndex        =   1
         Top             =   360
         Width           =   6492
         _ExtentX        =   11451
         _ExtentY        =   4043
         SelectionToggle =   -1  'True
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   0   'False
         Rows            =   2
         Cols            =   4
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
End
Attribute VB_Name = "frmPoint"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim DoneBuild As Boolean
Dim lts As Collection
Dim WithEvents tsl As ATCoTSlist
Attribute tsl.VB_VarHelpID = -1
Dim InUseFacs() As String, CountInUseFacs&
Dim AvailFacs() As String, CountAvailFacs&
Dim ConsLinks() As String, MemberLinks() As String, LinkCount&
Dim MSub1Links() As Long, MSub2Links() As Long

Private HsashDragging As Boolean
Private HsashDragStart As Single

Private Sub agdPoint_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits agdPoint
End Sub

Private Sub agdPoint_RowColChange()
  DoLimits agdPoint
End Sub

Private Sub agdPoint_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits agdPoint
End Sub

Private Sub FilterListOnScenFac(insen$, infac$)
  Dim sen$, loc$, con$, fac$, i&, dashpos&, j&
  
  If DoneBuild Then  'dont do while initializing
    
    With agdPoint
      loc = ""
      con = ""
    
      .rows = 0
      For i = 1 To agdMasterPoint.rows
        If agdMasterPoint.TextMatrix(i, 1) = insen And agdMasterPoint.TextMatrix(i, 3) = infac Then
          'add this to filtered grid
          .rows = .rows + 1
          For j = 0 To agdMasterPoint.cols - 1
            .TextMatrix(.rows, j) = agdMasterPoint.TextMatrix(i, j)
          Next j
        End If
      Next i
    End With
  End If
  
End Sub

Private Sub doAction(opt&)
  Dim tempts As Collection, i&, alist As Collection
  Dim g As Object, masterrow&, vtP As Variant, tP As HspfPoint
  Dim istart&, iend&, j&, lsen$, lfac$, k&, S$, iresp&
  
  Me.MousePointer = vbHourglass
  Set tempts = New Collection
  If agdPoint.SelStartRow > 0 Then
    For i = agdPoint.SelStartRow To agdPoint.SelEndRow
      tempts.Add lts(CInt(agdPoint.TextMatrix(i, 11)))
    Next i
  End If
  If opt = 1 Then
    If tempts.Count > 0 Then
      tsl.Showcoll tempts
    Else 'no rows selected
      MsgBox "No timeseries have been selected for listing.", vbOKOnly, _
             "Point Sources Listing Problem"
    End If
  ElseIf opt = 2 Then
    If tempts.Count > 0 Then
      'Call GLInit(1, g, tempts.Count, tempts.Count)
      MsgBox "Graph has not yet been implemented.", vbOKOnly, _
             "Point Sources Graphing Problem"
    Else 'no rows selected
      MsgBox "No timeseries have been selected for graphing.", vbOKOnly, _
             "Point Sources Graphing Problem"
    End If
  ElseIf opt = 3 Then
    If tempts.Count > 0 Then
      'delete the data sets
      j = 0
      istart = agdPoint.SelEndRow
      iend = agdPoint.SelStartRow
      iresp = MsgBox("Are you sure you want to delete these timeseries?", vbOKCancel, _
             "Point Sources Graphing Problem")
      If iresp = 1 Then
        For i = istart To iend Step -1
          myUci.DeleteWDMDataSet agdPoint.TextMatrix(i, 5), agdPoint.TextMatrix(i, 6)
          masterrow = agdPoint.TextMatrix(i, 14)
          lsen = agdPoint.TextMatrix(i, 1)
          lfac = agdPoint.TextMatrix(i, 3)
          'remove from point source structure
          k = 1
          For Each vtP In myUci.PointSources
            Set tP = vtP
            If tP.Target.volname = agdPoint.TextMatrix(i, 7) And _
               tP.Target.volid = agdPoint.TextMatrix(i, 8) And _
               tP.Source.volname = agdPoint.TextMatrix(i, 5) And _
               tP.Source.volid = agdPoint.TextMatrix(i, 6) Then
              myUci.PointSources.Remove k
            Else
              k = k + 1
            End If
          Next vtP
          agdPoint.DeleteRow i
          agdMasterPoint.DeleteRow masterrow
          j = j + 1
        Next i
        'keep grid in synch
        For i = istart To agdPoint.rows
          agdPoint.TextMatrix(i, 14) = agdPoint.TextMatrix(i, 14) - j
        Next i
        For i = 1 To agdMasterPoint.rows
          agdMasterPoint.TextMatrix(i, 14) = i
        Next i
        If agdPoint.rows = 0 Then
          'none left, remove from list
          Set alist = New Collection
          S = lfac & " (" & Mid(lsen, 4) & ")"
          If aslPoint.InLeftList(S) Then
            For j = 0 To aslPoint.LeftCount - 1
              If aslPoint.LeftItem(j) <> S Then
                alist.Add aslPoint.LeftItem(j)
              End If
            Next j
            aslPoint.ClearLeft
            For j = 1 To alist.Count
              aslPoint.LeftItemFastAdd alist(j)
            Next j
          ElseIf aslPoint.InRightList(S) Then
            For j = 0 To aslPoint.RightCount - 1
              If aslPoint.RightItem(j) <> S Then
                alist.Add aslPoint.RightItem(j)
              End If
            Next j
            aslPoint.ClearRight
            For j = 1 To alist.Count
              aslPoint.LeftItemFastAdd alist(j)
              aslPoint.MoveRight aslPoint.LeftCount - 1
            Next j
          End If
          If aslPoint.RightCount > 0 Then
            aslPoint_ItemSelected aslPoint.RightItem(0)
          ElseIf aslPoint.LeftCount > 0 Then
            aslPoint_ItemSelected aslPoint.LeftItem(0)
          End If
          chkDetails.Value = 0
        End If
      End If
    Else 'no rows selected
      MsgBox "No timeseries have been selected for deleting.", vbOKOnly, _
             "Point Sources Delete Problem"
    End If
  End If
  Me.MousePointer = vbNormal
End Sub

Private Sub aslPoint_Change()
  Dim i&, ifound As Boolean, k&, j&
  Dim sen$, fac$, ipos&, ilen&, itmp&
  
  'see if something new is in inuse facilities
  For i = 0 To aslPoint.RightCount - 1
    ifound = False
    For k = 1 To CountInUseFacs
      If aslPoint.RightItem(i) = InUseFacs(k - 1) Then
        ifound = True
        Exit For
      End If
    Next k
    If Not ifound Then
      'something added to inuse facilities
      ilen = Len(aslPoint.RightItem(i))
      If ilen > 0 Then
        ipos = InStr(1, aslPoint.RightItem(i), "(")
        If ipos > 0 Then
          'make sure this the last paren
          itmp = -1
          Do Until itmp = 0
            itmp = InStr(ipos + 1, aslPoint.RightItem(i), "(")
            If itmp > 0 Then
              ipos = itmp
            End If
          Loop
          sen = "PT-" & Mid(aslPoint.RightItem(i), ipos + 1, ilen - ipos - 1)
          fac = Mid(aslPoint.RightItem(i), 1, ipos - 2)
        End If
      End If
      For k = 1 To agdMasterPoint.rows
        If agdMasterPoint.TextMatrix(k, 1) = sen And agdMasterPoint.TextMatrix(k, 3) = fac Then
          'see if this constituent is in cons link list
          ifound = False
          For j = 1 To LinkCount
            If ConsLinks(j - 1) = UCase(agdMasterPoint.TextMatrix(k, 4)) Then
              ifound = True
              Exit For
            End If
          Next j
          If ifound Then
            'set indiv timsers to in use in master grid
            agdMasterPoint.TextMatrix(k, 0) = "Yes"
          End If
        End If
      Next k
      For k = 1 To agdPoint.rows
        If agdPoint.TextMatrix(k, 1) = sen And agdPoint.TextMatrix(k, 3) = fac Then
          'see if this constituent is in cons link list
          ifound = False
          For j = 1 To LinkCount
            If ConsLinks(j - 1) = UCase(agdPoint.TextMatrix(k, 4)) Then
              ifound = True
              Exit For
            End If
          Next j
          If ifound Then
            'set indiv timsers to in use in point grid
            agdPoint.TextMatrix(k, 0) = "Yes"
          End If
        End If
      Next k
    End If
  Next i
  'see if something new is in available facilities
  For i = 0 To aslPoint.LeftCount - 1
    ifound = False
    For k = 1 To CountAvailFacs
      If aslPoint.LeftItem(i) = AvailFacs(k - 1) Then
        ifound = True
        Exit For
      End If
    Next k
    If Not ifound Then
      'something added to available facilities
      ilen = Len(aslPoint.LeftItem(i))
      If ilen > 0 Then
        ipos = InStr(1, aslPoint.LeftItem(i), "(")
        If ipos > 0 Then
          'make sure this the last paren
          itmp = -1
          Do Until itmp = 0
            itmp = InStr(ipos + 1, aslPoint.LeftItem(i), "(")
            If itmp > 0 Then
              ipos = itmp
            End If
          Loop
          sen = "PT-" & Mid(aslPoint.LeftItem(i), ipos + 1, ilen - ipos - 1)
          fac = Mid(aslPoint.LeftItem(i), 1, ipos - 2)
        End If
      End If
      For k = 1 To agdMasterPoint.rows
        If agdMasterPoint.TextMatrix(k, 1) = sen And agdMasterPoint.TextMatrix(k, 3) = fac Then
          'set indiv timsers to not in use in master grid
          agdMasterPoint.TextMatrix(k, 0) = "No"
        End If
      Next k
      For k = 1 To agdPoint.rows
        If agdPoint.TextMatrix(k, 1) = sen And agdPoint.TextMatrix(k, 3) = fac Then
          'set indiv timsers to not in use in point grid
          agdPoint.TextMatrix(k, 0) = "No"
        End If
      Next k
    End If
  Next i
  'rebuild lists
  CountInUseFacs = aslPoint.RightCount
  ReDim InUseFacs(CountInUseFacs)
  For i = 0 To CountInUseFacs - 1
    InUseFacs(i) = aslPoint.RightItem(i)
  Next i
  CountAvailFacs = aslPoint.LeftCount
  ReDim AvailFacs(CountAvailFacs)
  For i = 0 To CountAvailFacs - 1
    AvailFacs(i) = aslPoint.LeftItem(i)
  Next i
End Sub

Private Sub aslPoint_ItemSelected(selectedtext As String)
  Dim sen$, fac$, ipos&, ilen&, itmp&
  ilen = Len(selectedtext)
  Grid2Master
  fraReach.Caption = "Details of " & selectedtext
  If ilen > 0 Then
    ipos = InStr(1, selectedtext, "(")
    If ipos > 0 Then
      'make sure this the last paren
      itmp = -1
      Do Until itmp = 0
        itmp = InStr(ipos + 1, selectedtext, "(")
        If itmp > 0 Then
          ipos = itmp
        End If
      Loop
      sen = "PT-" & Mid(selectedtext, ipos + 1, ilen - ipos - 1)
      fac = Mid(selectedtext, 1, ipos - 2)
    End If
    FilterListOnScenFac sen, fac
  End If
  chkDetails.Enabled = True
End Sub

Private Sub chkDetails_Click()
  If chkDetails.Value = 1 Then
    fraReach.Visible = True
    fraSash.Visible = True
  Else
    fraReach.Visible = False
    fraSash.Visible = False
  End If
  SetFormHeight
End Sub

Private Sub cmdNew_Click(Index As Integer)
  frmNewPoint.Init lts
  frmNewPoint.Show vbModal
  
  If agdPoint.rows > 0 Then
    FilterListOnScenFac agdPoint.TextMatrix(1, 1), agdPoint.TextMatrix(1, 3)
  End If
    
End Sub

Private Sub cmdPoint_Click(Index As Integer)
  Dim curcons$, curgroup$, curmember$, cursub1&, cursub2&
  Dim i&, j&, found As Boolean, ifound&, S$
  Dim targroup As String, tarmember As String
  Dim sen$, fac$, ipos&, ilen&, problem As Boolean

  problem = False
  If Index = 0 Then
    'user clicked ok
    Grid2Master
    'set any unselected facilities in aslpoint to not-in-use
    With aslPoint
      For i = 1 To .LeftCount
        S = .LeftItem(i - 1)
        ilen = Len(S)
        If ilen > 0 Then
          ipos = InStr(1, S, "(")
          If ipos > 0 Then
            sen = "PT-" & Mid(S, ipos + 1, ilen - ipos - 1)
            fac = Mid(S, 1, ipos - 2)
            For j = 1 To agdMasterPoint.rows
              If agdMasterPoint.TextMatrix(j, 1) = sen And _
                agdMasterPoint.TextMatrix(j, 3) = fac Then
                agdMasterPoint.TextMatrix(j, 0) = "No"
              End If
            Next j
          End If
        End If
      Next i
    End With
    'go through master list, putting point sources back
    With agdMasterPoint
      For i = 1 To .rows
        If .TextMatrix(i, 0) = "Yes" Then
          'this pt src is active
          'check to see if member and subs are filled in
          If Len(.TextMatrix(i, 10)) = 0 Then
            problem = True
          End If
          
          If Not problem Then
            'is it already in pt src structure
            ifound = 0
            For j = 1 To myUci.PointSources.Count
              If myUci.PointSources(j).Target.volname = .TextMatrix(i, 7) And _
                 myUci.PointSources(j).Target.volid = .TextMatrix(i, 8) And _
                 myUci.PointSources(j).Source.volname = .TextMatrix(i, 5) And _
                 myUci.PointSources(j).Source.volid = .TextMatrix(i, 6) Then
                ifound = j
              End If
            Next j
            If ifound > 0 Then
              myUci.PointSources(ifound).Target.group = .TextMatrix(i, 9)
              curmember = .TextMatrix(i, 10)
              j = InStr(1, curmember, "|")
              If j > 0 Then
                curmember = Mid(curmember, 1, j - 2)
              End If
              myUci.PointSources(ifound).Target.member = MemberFromLongVersion(curmember)
              myUci.PointSources(ifound).Target.memsub1 = MemSub1FromLongVersion(.TextMatrix(i, 10))
              myUci.PointSources(ifound).Target.memsub2 = MemSub2FromLongVersion(.TextMatrix(i, 10))
            End If
            
            If ifound = 0 Then
              'add to point source structure
              curcons = .TextMatrix(i, 4)
              curgroup = .TextMatrix(i, 9)
              curmember = .TextMatrix(i, 10)
              j = InStr(1, curmember, "|")
              If j > 0 Then
                curmember = Mid(curmember, 1, j - 2)
              End If
              curmember = MemberFromLongVersion(curmember)
              cursub1 = MemSub1FromLongVersion(.TextMatrix(i, 10))
              cursub2 = MemSub2FromLongVersion(.TextMatrix(i, 10))
              If Len(curgroup) > 0 And Len(curmember) > 0 Then
                myUci.AddPoint .TextMatrix(i, 5), .TextMatrix(i, 6), .TextMatrix(i, 8), .TextMatrix(i, 3), _
                               curgroup, curmember, cursub1, cursub2
              Else
                .TextMatrix(i, 0) = "No"
              End If
              myUci.Edited = True
            End If
          End If
        Else
          'this pt src is not active, but is it in pt src structure
          found = False
          For j = 1 To myUci.PointSources.Count
            If myUci.PointSources(j).Target.volname = .TextMatrix(i, 7) And _
               myUci.PointSources(j).Target.volid = .TextMatrix(i, 8) And _
               myUci.PointSources(j).Source.volname = .TextMatrix(i, 5) And _
               myUci.PointSources(j).Source.volid = .TextMatrix(i, 6) Then
              found = True
            End If
          Next j
          If found Then
            'remove from point source structure
            myUci.RemovePoint .TextMatrix(i, 5), .TextMatrix(i, 6), .TextMatrix(i, 8)
            myUci.Edited = True
          End If
        End If
      Next i
    End With
  End If
  'myUci.Source2MetSeg
  If problem Then
    myMsgBox.Show "At least one of the target members for " & vbCrLf & _
                  "a point source 'in use' has not been set." & vbCrLf & vbCrLf & _
                  "This must be set before continuing.", "Point Source Problem", "&OK"
  Else
    HSPFMain.ClearTree
    HSPFMain.BuildTree
    HSPFMain.UpdateLegend
    HSPFMain.UpdateDetails
    Unload Me
  End If
End Sub

Private Sub cmdScenario_Click()
  frmPointScenario.Show vbModal
End Sub

Private Sub Form_Load()
  Dim i&
  
  'always link flow to ivol
  LinkCount = 1
  ReDim ConsLinks(LinkCount)
  ReDim MemberLinks(LinkCount)
  ReDim MSub1Links(LinkCount)
  ReDim MSub2Links(LinkCount)
  ConsLinks(0) = "FLOW"
  MemberLinks(0) = "IVOL"
  MSub1Links(0) = 0
  MSub2Links(0) = 0
  
  DoneBuild = False
  SetGrid agdPoint
  SetGrid agdMasterPoint
  FillMasterGrid
  DoneBuild = True
  Set tsl = New ATCoTSlist
  tbrView.ImageList = imgView
  AddToolButton "Graph", "Graph Selected Point Sources"
  AddToolButton "List", "List/Edit Selected Point Sources"
  AddToolButton "Delete", "Delete Selected Point Sources"
  
  aslPoint.ButtonVisible("move up") = False
  aslPoint.ButtonVisible("move down") = False
  fraSash.BackColor = vbButtonFace
  
  fraReach.Visible = False
  fraSash.Visible = False
  SetFormHeight
  'myUci.MetSeg2Source
  CountInUseFacs = aslPoint.RightCount
  ReDim InUseFacs(CountInUseFacs)
  For i = 0 To CountInUseFacs - 1
    InUseFacs(i) = aslPoint.RightItem(i)
  Next i
  CountAvailFacs = aslPoint.LeftCount
  ReDim AvailFacs(CountAvailFacs)
  For i = 0 To CountAvailFacs - 1
    AvailFacs(i) = aslPoint.LeftItem(i)
  Next i
  
End Sub

Private Sub AddToolButton(buttName$, TipText$)
  tbrView.Buttons.Add tbrView.Buttons.Count + 1, buttName, , , buttName
  tbrView.Buttons(tbrView.Buttons.Count).tooltiptext = TipText
End Sub

Private Sub FillMasterGrid()
  Dim lTable As HspfTable
  Dim lOper As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim i&, lloc$, lcon$, lpol$, vpol As Variant, lsen$
  Dim j&, icnt&, k&, lfac$, S$, ifound As Boolean
  Dim dsncnt&, dsnptr&(), activeflag As Boolean

  aslPoint.ClearLeft
  aslPoint.ClearRight
  
  With agdMasterPoint
    .rows = 1
    
    Call myUci.FindTimSer("", "", "", lts)
    
    Set lOpnBlk = myUci.OpnBlks("RCHRES")
    icnt = 1
    For i = 1 To lts.Count
      lsen = lts(i).Header.sen
      If Mid(lsen, 1, 3) = "PT-" Then 'this is a pt src
        lloc = lts(i).Header.loc
        If IsNumeric(Mid(lloc, 4)) Then
          'get full reach name
          Set lOper = lOpnBlk.operfromid(CInt(Mid(lloc, 4)))
          If Not lOper Is Nothing Then
            'found a reach with this id
            lloc = "RCHRES " & lOper.Id & " - " & lOper.Description
            lfac = UCase(lts(i).Header.Desc)
            lcon = lts(i).Header.con
            S = lfac & " (" & Mid(lts(i).Header.sen, 4) & ")"
            If Not aslPoint.InLeftList(S) And Not aslPoint.InRightList(S) Then
              aslPoint.LeftItemFastAdd S
            End If
            
            'see how many times this dsn shows up in pt srcs
            dsncnt = 0
            activeflag = False
            If Not myUci.PointSources Is Nothing Then
              For j = 1 To myUci.PointSources.Count
                If myUci.PointSources(j).Target.volname = lOper.Name And _
                   myUci.PointSources(j).Target.volid = lOper.Id And _
                   Left(myUci.PointSources(j).Source.volname, 3) = lts(i).File.Label And _
                   myUci.PointSources(j).Source.volid = lts(i).Header.Id Then
                  'found this dsn in active point sources
                  dsncnt = dsncnt + 1
                  activeflag = True
                  ReDim Preserve dsnptr(dsncnt)
                  dsnptr(dsncnt) = j
                  If aslPoint.InLeftList(S) Then
                    For k = 1 To aslPoint.LeftCount
                      If aslPoint.LeftItem(k - 1) = S Then
                        aslPoint.MoveRight k - 1
                        Exit For
                      End If
                    Next k
                  End If
                End If
              Next j
            End If
            If activeflag = False Then
              'still add a line for this dsn
              dsncnt = 1
            End If
            
            For j = 1 To dsncnt
              If icnt > 1 Then .rows = .rows + 1
              If activeflag = False Then
                'not an active point source
                .TextMatrix(icnt, 0) = "No"
                .TextMatrix(icnt, 9) = "INFLOW"
                '.TextMatrix(icnt, 10) = "IVOL"
                '.TextMatrix(icnt, 12) = 0
                '.TextMatrix(icnt, 13) = 0
              Else
                'this is an active point source
                .TextMatrix(icnt, 0) = "Yes"
                .TextMatrix(icnt, 9) = myUci.PointSources(dsnptr(j)).Target.group
                .TextMatrix(icnt, 10) = MemberLongVersion(myUci.PointSources(dsnptr(j)).Target.member, myUci.PointSources(dsnptr(j)).Target.memsub1, myUci.PointSources(dsnptr(j)).Target.memsub2)
                '.TextMatrix(icnt, 12) = myUci.PointSources(dsnptr(j)).Target.memsub1
                '.TextMatrix(icnt, 13) = myUci.PointSources(dsnptr(j)).Target.memsub2
              End If

              .TextMatrix(icnt, 1) = lsen
              .TextMatrix(icnt, 2) = lloc
              .TextMatrix(icnt, 3) = UCase(lts(i).Header.Desc)
              .TextMatrix(icnt, 5) = myUci.GetWDMIdFromName(lts(i).File.Filename)  'save assoc src vol name
              .TextMatrix(icnt, 6) = lts(i).Header.Id     'save assoc src vol id
              .TextMatrix(icnt, 7) = lOper.Name     'save assoc tar vol name
              .TextMatrix(icnt, 8) = lOper.Id         'save assoc tar vol id
              .TextMatrix(icnt, 11) = i 'save index to lts
            
              'look for this con in pollutant list
              For Each vpol In PollutantList
                lpol = vpol
                If Mid(lcon, 1, 5) = Mid(lpol, 1, 5) Then
                  lcon = lpol
                  Exit For
                End If
              Next vpol
              .TextMatrix(icnt, 4) = lcon
              
              .TextMatrix(icnt, 14) = icnt 'save row number
              
              'default member based on constituent name if poss
              If activeflag Then
                'is active, see if we want to remember link
                ifound = False
                For k = 1 To LinkCount
                  If ConsLinks(k - 1) = UCase(Trim(lcon)) Then
                    ifound = True
                    Exit For
                  End If
                Next k
                If Not ifound Then
                  'add this to list
                  LinkCount = LinkCount + 1
                  ReDim Preserve ConsLinks(LinkCount)
                  ReDim Preserve MemberLinks(LinkCount)
                  ReDim Preserve MSub1Links(LinkCount)
                  ReDim Preserve MSub2Links(LinkCount)
                  ConsLinks(LinkCount - 1) = UCase(Trim(lcon))
                  MemberLinks(LinkCount - 1) = MemberFromLongVersion(.TextMatrix(icnt, 10))
                  MSub1Links(LinkCount - 1) = MemSub1FromLongVersion(.TextMatrix(icnt, 10))
                  MSub2Links(LinkCount - 1) = MemSub2FromLongVersion(.TextMatrix(icnt, 10))
                End If
              End If
              
              icnt = icnt + 1
            Next j
          End If
        End If
      End If
    Next i
    .ColsSizeByContents
    'set default members for all
    For i = 1 To .rows
      For k = 1 To LinkCount
        If ConsLinks(k - 1) = UCase(Trim(.TextMatrix(i, 4))) Then
          .TextMatrix(i, 10) = MemberLongVersion(MemberLinks(k - 1), MSub1Links(k - 1), MSub2Links(k - 1))
          '.TextMatrix(i, 12) = MSub1Links(K - 1)
          '.TextMatrix(i, 13) = MSub2Links(K - 1)
          Exit For
        End If
      Next k
    Next i
    If icnt = 1 Then .rows = 0
  End With

End Sub

Private Sub DoLimits(g As Object)
  g.ClearValues
  If g.col = 0 Then
    g.AddValue "No"
    g.AddValue "Yes"
  ElseIf g.col = 9 Then
    'Call SetGroupNames(g)
  ElseIf g.col = 10 Then
    Call SetMemberNames(g)
  ElseIf g.col = 12 Then
    'Call SetMemberSubs(g, 1)
  ElseIf g.col = 13 Then
    'Call SetMemberSubs(g, 2)
  End If
End Sub

Private Sub Form_Resize()
  Dim w&, h&
  Static LegendWidth&
  h = Me.ScaleHeight
  w = Me.ScaleWidth
  
  If Not (Me.WindowState = vbMinimized) Then
    If Width < 1500 Then Width = 1500
    'If height < 3500 Then height = 3500
    aslPoint.Width = Width + 200
    'fraNew.width = width - 300
    fraReach.Width = Width - 300
    agdPoint.Width = Width - 1400
    fraSash.Width = Width
    agdPoint.ColsSizeByContents
    tbrView.Left = Width - 900
    cmdPoint(0).Left = (Width / 2) - cmdPoint(0).Width - 200
    cmdPoint(1).Left = (Width / 2) + 200
    cmdNew(0).Left = (Width / 2) - cmdNew(0).Width - 200
    cmdScenario.Left = cmdPoint(1).Left
    'fraReach.height = height - 1200 - fraNew.height
    'agdPoint.height = height - 2600 - fraNew.height
    cmdPoint(0).Top = Height - 800
    cmdPoint(1).Top = cmdPoint(0).Top
    fraReach.Top = h - fraReach.Height - cmdPoint(0).Height - 300
    If fraReach.Height > 500 Then
      agdPoint.Height = fraReach.Height - 500
    End If
    If fraReach.Visible Then
      fraSash.Top = fraReach.Top - fraSash.Height
      If fraSash.Top > aslPoint.Top Then
        If fraSash.Top - aslPoint.Top - cmdNew(0).Height - 200 > 0 Then
          aslPoint.Height = fraSash.Top - aslPoint.Top - cmdNew(0).Height - 177
        End If
        cmdNew(0).Top = aslPoint.Top + aslPoint.Height + 120
        cmdScenario.Top = cmdNew(0).Top
        chkDetails.Top = cmdNew(0).Top
      End If
    Else
      If Height - aslPoint.Top - cmdNew(0).Height - 833 - cmdPoint(0).Height > 0 Then
        aslPoint.Height = Height - aslPoint.Top - cmdNew(0).Height - 833 - cmdPoint(0).Height
      End If
      cmdNew(0).Top = aslPoint.Top + aslPoint.Height + 120
      cmdScenario.Top = cmdNew(0).Top
      chkDetails.Top = cmdNew(0).Top
    End If
  End If
End Sub

Private Sub SetMemberNames(g As Object)
  Dim vMember As Variant, lMember As HspfTSMemberDef
  Dim sgroup$, skey$, lsub$, i&, sub1&, sub2&, j&, k&
  Dim vGroup As Variant, lGroup As HspfTSGroupDef, Desc$
  Dim lOper As HspfOperation, Id&, scoll As Collection
  
  Id = g.TextMatrix(g.row, 8)
  Set lOper = myUci.OpnBlks("RCHRES").operfromid(Id)
  GetMemberNames lOper, scoll
  For i = 1 To scoll.Count
    GetMemberSubs lOper, scoll(i), sub1, sub2
    If sub1 > 0 Then
      For j = 1 To sub1
        If sub2 > 0 Then
          For k = 1 To sub2
            g.AddValue MemberLongVersion(scoll(i), j, k)
          Next k
        Else
          g.AddValue MemberLongVersion(scoll(i), j, sub2)
        End If
      Next j
    Else
      g.AddValue MemberLongVersion(scoll(i), sub1, sub2)
    End If
  Next i

End Sub

'Private Sub SetMemberSubs(g As Object, isub&)
'  Dim i&, lmem$, sub1&, sub2&
'  Dim loper As HspfOperation, Id&
'
'  Id = g.TextMatrix(g.row, 8)
'  Set loper = myUci.OpnBlks("RCHRES").operfromid(Id)
'  lmem = g.TextMatrix(g.row, 10)
'  GetMemberSubs loper, lmem, sub1, sub2
'  If isub = 1 Then
'    g.ColMax(g.col) = sub1
'    g.ColMin(g.col) = 0
'  Else
'    g.ColMax(g.col) = sub2
'    g.ColMin(g.col) = 0
'  End If
'
'End Sub

Private Sub fraSash_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  HsashDragging = True
  HsashDragStart = y
  fraSash.BackColor = vb3DShadow
End Sub

Private Sub fraSash_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If HsashDragging Then
    Dim h&
    h = ScaleHeight - (fraSash.Top + (y - HsashDragStart) / Screen.TwipsPerPixelY + fraSash.Height)
    If h > cmdPoint(0).Height + 500 And h < Height - 2000 Then
      fraReach.Height = h - cmdPoint(0).Height - 300
    End If
    Form_Resize
  End If
End Sub

Private Sub fraSash_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  HsashDragging = False
  fraSash.BackColor = vbButtonFace
End Sub

Private Sub tbrView_ButtonClick(ByVal Button As MSComctlLib.Button)
  If Button.Key = "List" Then
    doAction 1
  ElseIf Button.Key = "Graph" Then
    doAction 2
  ElseIf Button.Key = "Delete" Then
    doAction 3
  End If
End Sub

Private Sub SetFormHeight()
  If fraReach.Visible Then
    Height = aslPoint.Height + 1800 + fraReach.Height
  Else
    Height = aslPoint.Height + 1700
  End If
End Sub

Private Sub SetGrid(g As Object)
  With g
    .cols = 14
    .ColType(0) = ATCoTxt
    .TextMatrix(0, 0) = "In Use"
    .TextMatrix(0, 1) = "HIDE" 'scenario
    .TextMatrix(0, 2) = "Reach"
    .TextMatrix(0, 3) = "HIDE" 'facility
    .TextMatrix(0, 4) = "Pollutant"
    .TextMatrix(0, 5) = "HIDE" 'src vol name
    .TextMatrix(0, 6) = "HIDE" 'src vol id
    .TextMatrix(0, 7) = "HIDE" 'tar vol name
    .TextMatrix(0, 8) = "HIDE" 'tar vol id
    .TextMatrix(0, 9) = "HIDE" 'tar group
    .TextMatrix(0, 10) = "Target Member"
    .TextMatrix(0, 11) = "HIDE" 'lts index
    '.TextMatrix(0, 12) = "MemSub1"
    '.TextMatrix(0, 13) = "MemSub2"
    .TextMatrix(0, 12) = "HIDE"
    .TextMatrix(0, 13) = "HIDE"
    .TextMatrix(0, 14) = "HIDE" 'row number
    .ColType(11) = ATCoInt
    .ColType(12) = ATCoInt
    .ColType(13) = ATCoInt
    .ColType(14) = ATCoInt
    .ColEditable(0) = True
    .ColEditable(1) = False
    .ColEditable(2) = False
    .ColEditable(3) = False
    .ColEditable(4) = False
    .ColEditable(5) = False
    .ColEditable(6) = False
    .ColEditable(7) = False
    .ColEditable(8) = False
    .ColEditable(9) = False
    .ColEditable(10) = True
    .ColEditable(11) = False
    .ColEditable(12) = True
    .ColEditable(13) = True
    .ColEditable(14) = False
  End With
End Sub

Private Sub Grid2Master()
  Dim sen$, loc$, con$, fac$, i&, dashpos&, j&, irow&
    
  If fraReach.Visible Then
    With agdPoint
      For i = 1 To .rows
        irow = .TextMatrix(i, 14)
        For j = 0 To agdMasterPoint.cols - 1
          agdMasterPoint.TextMatrix(irow, j) = .TextMatrix(i, j)
        Next j
      Next i
    End With
  End If
  
End Sub

Public Sub UpdateListsForNewPointSource(sen$, fac$, loc$, con$, WDMId$, dsn&, tarname$, tarid&, longloc$)
  Dim S$, icnt&, tempts As Collection, k&
  S = fac & " (" & Mid(sen, 4) & ")"
  If Not aslPoint.InLeftList(S) And Not aslPoint.InRightList(S) Then
    aslPoint.LeftItemFastAdd S
  End If
  With agdMasterPoint
    .rows = .rows + 1
    icnt = .rows
    .TextMatrix(icnt, 0) = "No"
    .TextMatrix(icnt, 1) = sen
    .TextMatrix(icnt, 2) = longloc
    .TextMatrix(icnt, 3) = UCase(fac)
    .TextMatrix(icnt, 4) = con
    .TextMatrix(icnt, 5) = WDMId
    .TextMatrix(icnt, 6) = dsn
    .TextMatrix(icnt, 7) = tarname
    .TextMatrix(icnt, 8) = tarid
    .TextMatrix(icnt, 9) = "INFLOW"
    myUci.FindTimSer sen, loc, con, tempts
    lts.Add tempts(tempts.Count)
    .TextMatrix(icnt, 11) = lts.Count 'save index to lts for list/graph
    .TextMatrix(icnt, 14) = icnt 'save row number
    '.TextMatrix(icnt, 10) = "IVOL"
    '.TextMatrix(icnt, 12) = 0
    '.TextMatrix(icnt, 13) = 0
    For k = 1 To LinkCount
      If ConsLinks(k - 1) = UCase(Trim(con)) Then
        .TextMatrix(icnt, 10) = MemberLongVersion(MemberLinks(k - 1), MSub1Links(k - 1), MSub2Links(k - 1))
        '.TextMatrix(icnt, 10) = MemberLinks(k - 1)
        '.TextMatrix(icnt, 12) = MSub1Links(k - 1)
        '.TextMatrix(icnt, 13) = MSub2Links(k - 1)
        If aslPoint.InRightList(S) Then
          .TextMatrix(icnt, 0) = "Yes"
        End If
        Exit For
      End If
    Next k

  End With
End Sub

Private Sub GetMemberNames(lOper As HspfOperation, scoll As Collection)
  Dim lTable As HspfTable
  
  Set lTable = lOper.tables("ACTIVITY")
  Set scoll = New Collection
  If lTable.Parms(1).Value = 1 Then
    'hydr on
    scoll.Add "IVOL"
    If Not lOper.Uci.categoryblock Is Nothing Then
      If lOper.Uci.categoryblock.Count > 0 Then
        scoll.Add "CIVOL"
      End If
    End If
  End If
  If lTable.Parms(3).Value = 1 Then
    'cons on
    scoll.Add "ICON"
  End If
  If lTable.Parms(4).Value = 1 Then
    'ht on
    scoll.Add "IHEAT"
  End If
  If lTable.Parms(5).Value = 1 Then
    'sed on
    scoll.Add "ISED"
  End If
  If lTable.Parms(6).Value = 1 Then
    'gqual on
    scoll.Add "IDQAL"
    scoll.Add "ISQAL"
  End If
  If lTable.Parms(7).Value = 1 Then
    'ox on
    scoll.Add "OXIF"
  End If
  If lTable.Parms(8).Value = 1 Then
    'nut on
    scoll.Add "NUIF1"
    scoll.Add "NUIF2"
  End If
  If lTable.Parms(9).Value = 1 Then
    'plank on
    scoll.Add "PKIF"
  End If
  If lTable.Parms(10).Value = 1 Then
    'ph on
    scoll.Add "PHIF"
  End If
End Sub

Private Sub GetMemberSubs(lOper As HspfOperation, lmem As String, sub1&, sub2&)
  Dim lTable As HspfTable, ncons&, ngqual&
  
  Set lTable = lOper.tables("ACTIVITY")
  
  If lmem = "IVOL" Then
    sub1 = 0
    sub2 = 0
  ElseIf lmem = "CIVOL" Then
    sub1 = lOper.Uci.categoryblock.Count
    sub2 = 0
  ElseIf lmem = "ICON" Then
    If lOper.TableExists("NCONS") Then
      ncons = lOper.tables("NCONS").Parms("NCONS")
    Else
      ncons = 1
    End If
    sub1 = ncons
    sub2 = 0
  ElseIf lmem = "IHEAT" Then
    sub1 = 0
    sub2 = 0
  ElseIf lmem = "ISED" Then
    sub1 = 3
    sub2 = 0
  ElseIf lmem = "IDQAL" Then
    If lOper.TableExists("GQ-GENDATA") Then
      ngqual = lOper.tables("GQ-GENDATA").Parms("NGQUAL")
    Else
      ngqual = 1
    End If
    sub1 = ngqual
    sub2 = 0
  ElseIf lmem = "ISQAL" Then
    If lOper.TableExists("GQ-GENDATA") Then
      ngqual = lOper.tables("GQ-GENDATA").Parms("NGQUAL")
    Else
      ngqual = 1
    End If
    sub1 = 3
    sub2 = ngqual
  ElseIf lmem = "OXIF" Then
    sub1 = 2
    sub2 = 0
  ElseIf lmem = "NUIF1" Then
    sub1 = 4
    sub2 = 0
  ElseIf lmem = "NUIF2" Then
    sub1 = 3
    sub2 = 2
  ElseIf lmem = "PKIF" Then
    sub1 = 5
    sub2 = 0
  ElseIf lmem = "PHIF" Then
    sub1 = 2
    sub2 = 0
  End If
End Sub

Private Function MemberLongVersion(mem$, sub1&, sub2&) As String
  Dim S$
  S = mem
  If sub1 > 0 Then
    S = S & "(" & sub1
    If sub2 > 0 Then
      S = S & "," & sub2 & ")"
    Else
      S = S & ")"
    End If
  End If
  If InStr(1, S, "|") = 0 Then
    MemberLongVersion = S & " | " & DescriptionFromMemberSubs(mem, sub1, sub2)
  Else
    MemberLongVersion = S
  End If
End Function

Private Function MemberFromLongVersion(S$) As String
  Dim i&
  i = InStr(1, S, "(")
  If i > 0 Then
    MemberFromLongVersion = Mid(S, 1, i - 1)
  Else
    MemberFromLongVersion = S
  End If
End Function

Private Function MemSub1FromLongVersion(S$) As Long
  Dim i&, j&
  i = InStr(1, S, "(")
  If i > 0 Then
    j = InStr(1, S, ",")
    If j = 0 Then
      j = InStr(1, S, ")")
    End If
    MemSub1FromLongVersion = CInt(Mid(S, i + 1, j - i - 1))
  Else
    MemSub1FromLongVersion = 0
  End If
End Function

Private Function MemSub2FromLongVersion(S$) As Long
  Dim i&, j&
  i = InStr(1, S, ",")
  If i > 0 Then
    j = InStr(1, S, ")")
    MemSub2FromLongVersion = CInt(Mid(S, i + 1, j - i - 1))
  Else
    MemSub2FromLongVersion = 0
  End If
End Function

Private Function DescriptionFromMemberSubs(lmem As String, sub1&, sub2&)
  Dim lOpnBlk As HspfOpnBlk
  Dim lOpn As HspfOperation
  Dim lTable As HspfTable
  
  If lmem = "IVOL" Then
    DescriptionFromMemberSubs = "water"
  ElseIf lmem = "CIVOL" Then
    DescriptionFromMemberSubs = "water for category " & CStr(sub1)
  ElseIf lmem = "ICON" Then
    DescriptionFromMemberSubs = "conservative"
  ElseIf lmem = "IHEAT" Then
    DescriptionFromMemberSubs = "heat"
  ElseIf lmem = "ISED" Then
    Select Case sub1
      Case 1: DescriptionFromMemberSubs = "sand"
      Case 2: DescriptionFromMemberSubs = "silt"
      Case 3: DescriptionFromMemberSubs = "clay"
    End Select
  ElseIf lmem = "IDQAL" Then
    Set lOpnBlk = myUci.OpnBlks("RCHRES")
    DescriptionFromMemberSubs = "dissolved gqual"
    If Not lOpnBlk Is Nothing Then
      Set lOpn = lOpnBlk.Ids(1)
      If Not lOpn Is Nothing Then
        If sub1 = 1 Or sub1 = 0 Then
          Set lTable = lOpn.tables("GQ-QALDATA")
        Else
          Set lTable = lOpn.tables("GQ-QALDATA:" & CStr(sub1))
        End If
        If Not lTable Is Nothing Then
          DescriptionFromMemberSubs = "dissolved " & Trim(lTable.Parms("GQID"))
        End If
      End If
    End If
  ElseIf lmem = "ISQAL" Then
    Select Case sub1
      Case 1: DescriptionFromMemberSubs = "sand associated "
      Case 2: DescriptionFromMemberSubs = "silt associated "
      Case 3: DescriptionFromMemberSubs = "clay associated "
    End Select
    Set lOpnBlk = myUci.OpnBlks("RCHRES")
    If Not lOpnBlk Is Nothing Then
      Set lOpn = lOpnBlk.Ids(1)
      If Not lOpn Is Nothing Then
        If sub2 = 1 Then
          Set lTable = lOpn.tables("GQ-QALDATA")
        Else
          Set lTable = lOpn.tables("GQ-QALDATA:" & CStr(sub2))
        End If
        If Not lTable Is Nothing Then
          DescriptionFromMemberSubs = DescriptionFromMemberSubs & Trim(lTable.Parms("GQID"))
        End If
      End If
    End If
  ElseIf lmem = "OXIF" Then
    Select Case sub1
      Case 1: DescriptionFromMemberSubs = "do"
      Case 2: DescriptionFromMemberSubs = "bod"
    End Select
  ElseIf lmem = "NUIF1" Then
    Select Case sub1
      Case 1: DescriptionFromMemberSubs = "no3"
      Case 2: DescriptionFromMemberSubs = "tam"
      Case 3: DescriptionFromMemberSubs = "no2"
      Case 4: DescriptionFromMemberSubs = "po4"
    End Select
  ElseIf lmem = "NUIF2" Then
    Select Case sub2
      Case 1: DescriptionFromMemberSubs = "particulate nh4 on "
      Case 2: DescriptionFromMemberSubs = "particulate po4 on "
    End Select
    Select Case sub1
      Case 1: DescriptionFromMemberSubs = DescriptionFromMemberSubs & "sand"
      Case 2: DescriptionFromMemberSubs = DescriptionFromMemberSubs & "silt"
      Case 3: DescriptionFromMemberSubs = DescriptionFromMemberSubs & "clay"
    End Select
  ElseIf lmem = "PKIF" Then
    Select Case sub1
      Case 1: DescriptionFromMemberSubs = "phyto"
      Case 2: DescriptionFromMemberSubs = "zoo"
      Case 3: DescriptionFromMemberSubs = "orn"
      Case 4: DescriptionFromMemberSubs = "orp"
      Case 5: DescriptionFromMemberSubs = "orc"
    End Select
  ElseIf lmem = "PHIF" Then
    Select Case sub1
      Case 1: DescriptionFromMemberSubs = "tic"
      Case 2: DescriptionFromMemberSubs = "co2"
    End Select
  End If
End Function
