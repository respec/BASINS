VERSION 5.00
Begin VB.Form frmTime 
   Caption         =   "WinHSPF - Simulation Time and Meteorological Data"
   ClientHeight    =   5544
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   5676
   HelpContextID   =   36
   Icon            =   "frmTime.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5544
   ScaleWidth      =   5676
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraMet 
      Caption         =   "Met Segments"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3615
      Left            =   120
      TabIndex        =   19
      Top             =   1200
      Width           =   5415
      Begin VB.CommandButton cmdApply 
         Caption         =   "A&pply"
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
         Left            =   3240
         TabIndex        =   23
         ToolTipText     =   "Apply this Met Seg to Contributing Land Area"
         Top             =   240
         Width           =   1092
      End
      Begin VB.CommandButton cmdEdit 
         Caption         =   "&Edit"
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
         Left            =   2160
         TabIndex        =   22
         ToolTipText     =   "Edit Characteristics of Selected Met Segment"
         Top             =   240
         Width           =   1095
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
         Left            =   1080
         TabIndex        =   21
         ToolTipText     =   "Add a Met Segment to the Available List"
         Top             =   240
         Width           =   1095
      End
      Begin ATCoCtl.ATCoGrid agdMet 
         Height          =   2655
         Left            =   120
         TabIndex        =   20
         Top             =   600
         Width           =   5175
         _ExtentX        =   9123
         _ExtentY        =   4678
         SelectionToggle =   -1  'True
         AllowBigSelection=   -1  'True
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
         Header          =   "Connections"
         FixedRows       =   1
         FixedCols       =   0
         ScrollBars      =   3
         SelectionMode   =   1
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
   End
   Begin ATCoCtl.ATCoText atxEnd 
      Height          =   255
      Index           =   0
      Left            =   1080
      TabIndex        =   7
      Top             =   720
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText atxStart 
      Height          =   255
      Index           =   0
      Left            =   1080
      TabIndex        =   2
      Top             =   360
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin VB.CommandButton cmdTime 
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
      Left            =   3000
      TabIndex        =   1
      Top             =   5040
      Width           =   1335
   End
   Begin VB.CommandButton cmdTime 
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
      Left            =   1320
      TabIndex        =   0
      Top             =   5040
      Width           =   1335
   End
   Begin ATCoCtl.ATCoText atxStart 
      Height          =   255
      Index           =   1
      Left            =   1920
      TabIndex        =   3
      Top             =   360
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText atxStart 
      Height          =   255
      Index           =   2
      Left            =   2760
      TabIndex        =   4
      Top             =   360
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText atxStart 
      Height          =   255
      Index           =   3
      Left            =   3600
      TabIndex        =   5
      Top             =   360
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText atxStart 
      Height          =   255
      Index           =   4
      Left            =   4440
      TabIndex        =   6
      Top             =   360
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText atxEnd 
      Height          =   255
      Index           =   1
      Left            =   1920
      TabIndex        =   8
      Top             =   720
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText atxEnd 
      Height          =   255
      Index           =   2
      Left            =   2760
      TabIndex        =   9
      Top             =   720
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText atxEnd 
      Height          =   255
      Index           =   3
      Left            =   3600
      TabIndex        =   10
      Top             =   720
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText atxEnd 
      Height          =   255
      Index           =   4
      Left            =   4440
      TabIndex        =   11
      Top             =   720
      Width           =   735
      _ExtentX        =   1291
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin VB.Label lblDate 
      Alignment       =   2  'Center
      Caption         =   "Minute"
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
      Index           =   6
      Left            =   4440
      TabIndex        =   18
      Top             =   120
      Width           =   735
   End
   Begin VB.Label lblDate 
      Alignment       =   2  'Center
      Caption         =   "Hour"
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
      Index           =   5
      Left            =   3600
      TabIndex        =   17
      Top             =   120
      Width           =   735
   End
   Begin VB.Label lblDate 
      Alignment       =   2  'Center
      Caption         =   "Day"
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
      Index           =   4
      Left            =   2760
      TabIndex        =   16
      Top             =   120
      Width           =   735
   End
   Begin VB.Label lblDate 
      Alignment       =   2  'Center
      Caption         =   "Month"
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
      Left            =   1920
      TabIndex        =   15
      Top             =   120
      Width           =   735
   End
   Begin VB.Label lblDate 
      Alignment       =   2  'Center
      Caption         =   "Year"
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
      Left            =   1080
      TabIndex        =   14
      Top             =   120
      Width           =   735
   End
   Begin VB.Label lblDate 
      Alignment       =   1  'Right Justify
      Caption         =   "End"
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
      Left            =   120
      TabIndex        =   13
      Top             =   720
      Width           =   855
   End
   Begin VB.Label lblDate 
      Alignment       =   1  'Right Justify
      Caption         =   "Start"
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
      Left            =   120
      TabIndex        =   12
      Top             =   480
      Width           =   855
   End
End
Attribute VB_Name = "frmTime"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Dim lUci As HspfUci

Private Sub agdMet_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits agdMet
End Sub

Private Sub agdMet_DoubleClick(row As Long, col As Long)
  DoLimits agdMet
End Sub

Private Sub agdMet_RowColChange()
  DoLimits agdMet
End Sub

Private Sub agdMet_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits agdMet
End Sub

Private Sub cmdAdd_Click()
  frmAddMet.Init "", 0
  frmAddMet.Show vbModal
  RefreshGrid
  DoLimits agdMet
End Sub

Private Sub cmdApply_Click()
  Dim lId&, loper As HspfOperation, moved As Boolean, Desc$
  Dim vConn As Variant, lConn As HspfConnection
  Dim upLand As Collection 'of hspfoperations
  Dim upOper As HspfOperation, tOper As HspfOperation
  Dim vupOper As Variant, lupOper As HspfOperation
  Dim lGrouped As Boolean, SelectedMetSeg$, iresp&, iChange&
  Dim j&, tOperName As String, tOperId&, linc&, afterid&, i&, k&
  Dim vTable As Variant, lTable As HspfTable, tabname$
  
  Set upLand = New Collection
  
  If agdMet.TextMatrix(agdMet.SelStartRow, 0) = "<none>" Then
    iresp = myMsgBox.Show("Unable to apply Met Segment <none> to contributing land area.", "Apply Met Segment Problem", "+&OK")
  Else
    'check if we can apply it to the existing perlnd/implnds, or create new operations
    lId = CInt(Mid(agdMet.TextMatrix(agdMet.row, 1), 8))
    Set loper = myUci.OpnBlks("RCHRES").operfromid(lId)
    SelectedMetSeg = agdMet.TextMatrix(agdMet.SelStartRow, 0)
    'find operations contributing to this rchres
    For Each vConn In loper.Sources
      Set lConn = vConn
      If lConn.Source.Opn.Name = "PERLND" Or lConn.Source.Opn.Name = "IMPLND" Then
        'found a contributing land area
        Set upOper = lConn.Source.Opn
        upLand.Add upOper
      End If
    Next vConn
    If upLand.Count = 0 Then
      'no upstream land areas
      iresp = myMsgBox.Show("There is no contributing land area to which to apply this Met Segment.", "Apply Met Segment Problem", "+&OK")
    Else
      'do these land areas contribute to other reaches?
      lGrouped = False
      For Each vupOper In upLand
        Set lupOper = vupOper
        For Each vConn In lupOper.targets
          Set lConn = vConn
          If (lConn.Target.Opn.Name = "RCHRES" And lConn.Target.Opn.Id <> lId) _
            Or lConn.Target.Opn.Name = "BMPRAC" Then
            'this oper goes to another reach or a bmprac
            lGrouped = True
          End If
        Next vConn
      Next vupOper
      iChange = 0
      For Each vupOper In upLand
        Set lupOper = vupOper
        If lupOper.MetSeg.Name <> SelectedMetSeg Then
          'need to change
          iChange = iChange + 1
        End If
      Next vupOper
      If iChange = 0 Then
        'nothing to change
        iresp = myMsgBox.Show("The contributing land area is already associated with this Met Segment.", "Apply Met Segment", "+&OK")
      Else
        If Not lGrouped Then
          'no - apply to existing
          For Each vupOper In upLand
            Set lupOper = vupOper
            'find in list
            For j = 1 To agdMet.rows
              tOperName = Left(agdMet.TextMatrix(j, 1), 6)
              tOperId = CInt(Mid(agdMet.TextMatrix(j, 1), 8))
              If lupOper.Name = tOperName And lupOper.Id = tOperId Then
                agdMet.TextMatrix(j, 0) = agdMet.TextMatrix(agdMet.SelStartRow, 0)
              End If
            Next j
          Next vupOper
        Else
          'we are using grouped option
          'do we have an existing p/i set for this met seg?
          i = 0
          For Each vupOper In upLand
            moved = False
            Set lupOper = vupOper
            For j = 1 To agdMet.rows
              If agdMet.TextMatrix(j, 0) = agdMet.TextMatrix(agdMet.SelStartRow, 0) Then
                'this line has the met seg we're looking for
                tOperName = Left(agdMet.TextMatrix(j, 1), 6)
                tOperId = CInt(Mid(agdMet.TextMatrix(j, 1), 8))
                Desc = myUci.OpnBlks(tOperName).operfromid(tOperId).Description
                If lupOper.Name = tOperName And lupOper.Description = Desc Then
                  'this looks like the one we want
                  i = i + 1
                End If
              End If
            Next j
          Next vupOper
          'do we want to move to existing p/i set?
          iresp = 0
          If i > 0 Then
            iresp = myMsgBox.Show("Do you want to group the contributing area for this reach" & _
                                  vbCrLf & "with the corresponding existing met segment?", _
                                  "Group Met Segment", "+&Yes", "-&No")
          End If
          If iresp = 1 Then
            'do grouping to existing met seg
            i = 1
            For Each vupOper In upLand
              moved = False
              Set lupOper = vupOper
              For j = 1 To agdMet.rows
                If agdMet.TextMatrix(j, 0) = agdMet.TextMatrix(agdMet.SelStartRow, 0) Then
                  'this line has the met seg we're looking for
                  tOperName = Left(agdMet.TextMatrix(j, 1), 6)
                  tOperId = CInt(Mid(agdMet.TextMatrix(j, 1), 8))
                  Desc = myUci.OpnBlks(tOperName).operfromid(tOperId).Description
                  If lupOper.Name = tOperName And lupOper.Description = Desc Then
                    'this looks like the one we want
                    Set tOper = myUci.OpnBlks(tOperName).operfromid(tOperId)
                    'move land area from one set to the other
                    For Each vConn In loper.Sources
                      Set lConn = vConn
                      If lConn.Source.volname = lupOper.Name And _
                         lConn.Source.volid = lupOper.Id Then
                        Set lConn.Source.Opn = tOper
                        lConn.Source.volid = tOper.Id
                      End If
                    Next vConn
                    k = 1
                    For Each vConn In lupOper.targets
                      Set lConn = vConn
                      If lConn.Target.volname = loper.Name And _
                         lConn.Target.volid = loper.Id Then
                        'remove this target
                        lupOper.targets.Remove k
                        'set this as a target from the new opn
                        tOper.targets.Add lConn
                      Else
                        k = k + 1
                      End If
                    Next vConn
                    moved = True
                  End If
                End If
              Next j
              If moved Then
                upLand.Remove i
              Else
                i = i + 1
              End If
            Next vupOper
          End If
          If upLand.Count > 0 Then
            'create new
            iresp = myMsgBox.Show(upLand.Count & " new PERLNDs/IMPLNDs will be created." & vbCrLf & vbCrLf & _
                                  "Do you want to add these new operations?", _
                                  "Apply Met Segment", "+&Yes", "-&No")
            If iresp = 1 Then
              'create new operations
              For Each vupOper In upLand
                Set lupOper = vupOper
                tOperName = lupOper.Name
                If lupOper.Id < 900 Then
                  linc = 100
                Else
                  linc = 1
                End If
                tOperId = lupOper.Id + linc
                'figure out which to put it after in the opn seq block
                afterid = 1
                For i = 1 To myUci.OpnSeqBlock.Opns.Count
                  If myUci.OpnSeqBlock.Opn(i).Name = lupOper.Name And _
                     myUci.OpnSeqBlock.Opn(i).Id = lupOper.Id Then
                    afterid = i
                  End If
                Next i
                Do While Not myUci.OpnBlks(tOperName).operfromid(tOperId) Is Nothing
                  tOperId = tOperId + linc
                Loop
              
                'add the operation to the uci object
                myUci.AddOperation tOperName, tOperId
                Set tOper = myUci.OpnBlks(tOperName).operfromid(tOperId)
                tOper.Description = lupOper.Description
                Set tOper.MetSeg = lupOper.MetSeg
                'add the operation to the opn seq block
                myUci.OpnSeqBlock.AddAfter tOper, afterid
                'copy tables from the existing operation
                For Each vTable In lupOper.tables
                  Set lTable = vTable
                  If lTable.OccurCount > 1 And lTable.OccurNum > 1 Then
                    tabname = lTable.Name & ":" & lTable.OccurNum
                  Else
                    tabname = lTable.Name
                  End If
                  myUci.AddTable tOperName, tOperId, tabname
                  For i = 1 To lTable.Parms.Count
                    tOper.tables(tabname).Parms(i) = lTable.Parms(i)
                  Next i
                Next vTable
                'add to list
                For j = 1 To agdMet.rows
                  If lupOper.Name = Left(agdMet.TextMatrix(j, 1), 6) And _
                     lupOper.Id = CInt(Mid(agdMet.TextMatrix(j, 1), 8)) Then
                    agdMet.insertrow j
                    agdMet.TextMatrix(j + 1, 0) = SelectedMetSeg
                    agdMet.TextMatrix(j + 1, 1) = tOperName & " " & tOperId
                    Exit For
                  End If
                Next j
                'move land area from one set to the other
                For Each vConn In loper.Sources
                  Set lConn = vConn
                  If lConn.Source.volname = lupOper.Name And _
                     lConn.Source.volid = lupOper.Id Then
                    Set lConn.Source.Opn = tOper
                    lConn.Source.volid = tOper.Id
                  End If
                Next vConn
                k = 1
                For Each vConn In lupOper.targets
                  Set lConn = vConn
                  If lConn.Target.volname = loper.Name And _
                     lConn.Target.volid = loper.Id Then
                    'remove this target
                    lupOper.targets.Remove k
                    'set this as a target from the new opn
                    tOper.targets.Add lConn
                  Else
                    k = k + 1
                  End If
                Next vConn
              Next vupOper
            End If
          End If
        End If
      End If
    End If
  End If
End Sub

Private Sub cmdEdit_Click()
  If agdMet.SelCount > 0 Then
    If agdMet.TextMatrix(agdMet.SelStartRow, 0) = "<none>" Then
      MsgBox "Unable to Edit Met Segment <none>.", vbOKOnly, "Edit Met Segment Problem"
    Else
      frmAddMet.Init agdMet.TextMatrix(agdMet.SelStartRow, 0), 1
      frmAddMet.Show vbModal
      RefreshGrid
      DoLimits agdMet
    End If
  Else
    MsgBox "Select a Met Segment from the list below.", vbOKOnly, "Edit Met Segment Problem"
  End If
End Sub

'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Sub cmdTime_Click(Index As Integer)
  Dim i&, myglobal As HspfGlobalBlk
  Dim lOpnBlk As HspfOpnBlk
  Dim loper As HspfOperation
  Dim ctemp$, Id&, j&, inuse As Boolean
  
  If Index = 0 Then
    'okay
    Set myglobal = myUci.GlobalBlock
    For i = 0 To 4
      'put dates back
      myglobal.SDate(i) = atxStart(i).Value
      myglobal.EDate(i) = atxEnd(i).Value
    Next i
    For i = 1 To agdMet.rows
      'put met segs back
      ctemp = agdMet.TextMatrix(i, 1)
      Set lOpnBlk = myUci.OpnBlks(Mid(ctemp, 1, 6))
      Id = CInt(Mid(ctemp, 8))
      Set loper = lOpnBlk.operfromid(Id)
      If agdMet.TextMatrix(i, 0) = "<none>" Then
        Set loper.MetSeg = Nothing
      Else
        For j = 1 To myUci.MetSegs.Count
          If myUci.MetSegs(j).Name = agdMet.TextMatrix(i, 0) Then
            Set loper.MetSeg = myUci.MetSegs(j)
          End If
        Next j
      End If
    Next i
    'remove unused met segments
    j = 1
    Do While j <= myUci.MetSegs.Count
      inuse = False
      For i = 1 To agdMet.rows
        If agdMet.TextMatrix(i, 0) = myUci.MetSegs(j).Name Then
          inuse = True
        End If
      Next i
      If Not inuse Then
        myUci.MetSegs.Remove j
      Else
        myUci.MetSegs(j).Id = j
        j = j + 1
      End If
    Loop
  End If
  HSPFMain.ClearTree
  HSPFMain.BuildTree
  HSPFMain.UpdateLegend
  HSPFMain.UpdateDetails
  Unload Me
End Sub

Private Sub Form_Load()
  Dim i&
  
  'setting a copy -- not yet implemented
  '  would allow a full cancel
  'Set lUci = myUci.Copy
  
  atxStart(0).HardMin = 0
  atxStart(1).HardMin = 1
  atxStart(2).HardMin = 1
  atxStart(3).HardMin = 0
  atxStart(4).HardMin = 0
  atxStart(1).HardMax = 12
  atxStart(2).HardMax = 31
  atxStart(3).HardMax = 24
  atxStart(4).HardMax = 60
  atxEnd(0).HardMin = 0
  atxEnd(1).HardMin = 1
  atxEnd(2).HardMin = 1
  atxEnd(3).HardMin = 0
  atxEnd(4).HardMin = 0
  atxEnd(1).HardMax = 12
  atxEnd(2).HardMax = 31
  atxEnd(3).HardMax = 24
  atxEnd(4).HardMax = 60
  With myUci.GlobalBlock
    For i = 0 To 4
      atxStart(i).Value = .SDate(i)
      atxEnd(i).Value = .EDate(i)
    Next i
  End With
  
  RefreshGrid
  
End Sub

Private Sub Form_Resize()
  If Not (Me.WindowState = vbMinimized) Then
    If width < 1500 Then width = 1500
    If height < 4000 Then height = 4000
    fraMet.width = width - 400
    agdMet.width = fraMet.width - 200
    cmdTime(0).Left = (width / 2) - cmdTime(0).width - 200
    cmdTime(1).Left = (width / 2) + 200
    cmdAdd.Left = (fraMet.width / 2) - (1.5 * cmdAdd.width) - 100
    cmdEdit.Left = (fraMet.width / 2) - (0.5 * cmdEdit.width)
    cmdApply.Left = (fraMet.width / 2) + (0.5 * cmdApply.width) + 100
    cmdTime(0).Top = height - cmdTime(0).height - 500
    cmdTime(1).Top = height - cmdTime(1).height - 500
    fraMet.height = height - (6 * cmdTime(0).height)
    agdMet.height = fraMet.height - 700
  End If
End Sub

Private Sub DoLimits(g As Object)
  Dim i&
  g.ClearValues
  If g.col = 0 Then
    g.AddValue "<none>"
    For i = 1 To myUci.MetSegs.Count
      g.AddValue myUci.MetSegs(i).Name
    Next i
  End If
  If Left(g.TextMatrix(g.row, 1), 6) = "RCHRES" Then
    cmdApply.Enabled = True
  Else
    cmdApply.Enabled = False
  End If
End Sub

Private Sub RefreshGrid()
  Dim lTable As HspfTable
  Dim loper As HspfOperation
  Dim lConn As HspfConnection
  Dim lmetseg As HspfMetSeg
  Dim i&, s$, j&
  
  With agdMet
    .rows = 0
    .cols = 2
    .ColTitle(0) = "Met Seg ID"
    .ColTitle(1) = "Operation"
    For i = 1 To myUci.OpnSeqBlock.Opns.Count
      Set loper = myUci.OpnSeqBlock.Opns(i)
      If loper.Name = "PERLND" Or loper.Name = "IMPLND" Or loper.Name = "RCHRES" Then
        Set lmetseg = loper.MetSeg
        .rows = .rows + 1
        If lmetseg Is Nothing Then
          .TextMatrix(.rows, 0) = "<none>"
        Else
          .TextMatrix(.rows, 0) = lmetseg.Name
        End If
        .TextMatrix(.rows, 1) = loper.Name & " " & loper.Id
      End If
    Next i
    .ColEditable(0) = True
    .ColsSizeByContents
  End With
End Sub
