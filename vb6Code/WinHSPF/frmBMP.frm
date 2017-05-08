VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Begin VB.Form frmBMP 
   Caption         =   "WinHSPF - Best Management Practices Editor"
   ClientHeight    =   5292
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   9948
   HelpContextID   =   53
   LinkTopic       =   "Form1"
   ScaleHeight     =   5292
   ScaleWidth      =   9948
   StartUpPosition =   2  'CenterScreen
   Begin VB.OptionButton rdoOpt 
      Caption         =   "ID"
      Height          =   255
      Index           =   1
      Left            =   8040
      TabIndex        =   13
      Top             =   120
      Visible         =   0   'False
      Width           =   735
   End
   Begin VB.OptionButton rdoOpt 
      Caption         =   "Area"
      Height          =   255
      Index           =   0
      Left            =   7200
      TabIndex        =   12
      Top             =   120
      Value           =   -1  'True
      Visible         =   0   'False
      Width           =   735
   End
   Begin VB.Frame fraBMPDet 
      Caption         =   "Current BMP Details"
      Height          =   1215
      Left            =   1680
      TabIndex        =   7
      Top             =   480
      Width           =   8055
      Begin VB.CommandButton cmdEdit 
         Caption         =   "&Edit Removal Efficiency"
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
         Left            =   2040
         TabIndex        =   14
         Top             =   240
         Width           =   2415
      End
      Begin VB.TextBox txtBMPDesc 
         Height          =   285
         Left            =   1080
         TabIndex        =   8
         Text            =   "BMPDescription"
         Top             =   720
         Width           =   5295
      End
      Begin ATCoCtl.ATCoText atxBMPId 
         Height          =   255
         Left            =   1080
         TabIndex        =   11
         Top             =   360
         Width           =   735
         _ExtentX        =   1291
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   999
         HardMin         =   1
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   5
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin VB.Label Label2 
         Alignment       =   1  'Right Justify
         Caption         =   "ID:"
         Height          =   255
         Left            =   360
         TabIndex        =   10
         Top             =   360
         Width           =   735
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         Caption         =   "Description:"
         Height          =   255
         Left            =   240
         TabIndex        =   9
         Top             =   720
         Width           =   855
      End
   End
   Begin VB.CommandButton cmdUpU 
      Caption         =   "&Update UCI"
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
      Height          =   375
      Left            =   360
      TabIndex        =   6
      Top             =   4800
      Width           =   1215
   End
   Begin VB.CommandButton cmdBMP 
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
      Index           =   2
      Left            =   1680
      TabIndex        =   5
      Top             =   4800
      Width           =   1215
   End
   Begin VB.CommandButton cmdBMP 
      Caption         =   "&Delete BMP"
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
      Left            =   240
      TabIndex        =   4
      Top             =   1200
      Width           =   1215
   End
   Begin VB.CommandButton cmdBMP 
      Caption         =   "&Add BMP"
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
      Left            =   240
      TabIndex        =   3
      Top             =   720
      Width           =   1215
   End
   Begin ATCoCtl.ATCoGrid grdSrc 
      Height          =   2775
      Left            =   0
      TabIndex        =   2
      Top             =   1920
      Width           =   9735
      _ExtentX        =   17166
      _ExtentY        =   4890
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   2
      Cols            =   3
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "Contributing Sources to Selected Reach:"
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
   Begin VB.ComboBox cmbReach 
      Height          =   315
      Left            =   3480
      Style           =   2  'Dropdown List
      TabIndex        =   0
      Top             =   120
      Width           =   3495
   End
   Begin VB.Label lblSelect 
      Caption         =   "Select Summary or Reach below BMP:"
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
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   3975
   End
End
Attribute VB_Name = "frmBMP"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Type BmpInfo
  Id As Long
  Desc As String
  InUseNow As Boolean 'associated with current reach
  DeletePending As Boolean 'get rid of at next update uci
  col As Long
End Type
Private myBmp() As BmpInfo

Private Type BmpPtrInfo
  Ind As Long 'into type bmpinfo
  Area As Single
End Type

Private Type TribInfo
  Id As Long
  OpNam As String
  Desc As String
  Area As Single
  BMPCnt As Long
  BmpPtr() As BmpPtrInfo
End Type
Private myTrib() As TribInfo

Private Type RchInfo
  Id As Long
  Desc As String
  BMPCnt As Long
End Type
Private MyRch() As RchInfo
Private msgTitle$
Private curBMPId&
Public BMPDesc$

Private Sub GetUciInfo(cur&)
  
  Dim Init&, Id&, rorb&
  Dim ctxt$, cmor$, pnam$, rcnt&, tcnt&
  Dim i&, j&, k&, h&, bcnt&, found As Boolean
  Dim rarea!, barea!
  Dim lOper As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim vConn As Variant
  Dim lConn As HspfConnection
  
  Id = MyRch(cur).Id
  'assume bmps not in use for this reach
  For i = 1 To UBound(myBmp)
    myBmp(i).InUseNow = False
  Next i
  
  ReDim myTrib(0)
  tcnt = 0
  bcnt = UBound(myBmp)
  Set lOpnBlk = myUci.OpnBlks("RCHRES")  'reach
  Set lOper = lOpnBlk.operfromid(Id)
  
  If Not lOper Is Nothing Then
    'loop through all connections looking for this oper as target
    For Each vConn In myUci.Connections
      Set lConn = vConn
      If lConn.Typ = 3 Or lConn.Typ = 2 Then 'network or schematic
        If Not lConn.Target.Opn Is Nothing Then
          If lConn.Target.Opn.Id = lOper.Id And _
             lConn.Target.Opn.Name = lOper.Name Then  'found a source
            If lConn.Source.volname = "BMPRAC" Then
              For i = 1 To bcnt
                If myBmp(i).Id = lConn.Source.volid Then
                  myBmp(i).InUseNow = True
                  MyRch(cur).BMPCnt = MyRch(cur).BMPCnt + 1
                  Exit For
                End If
              Next i
            Else 'non-bmp source
              tcnt = tcnt + 1
              ReDim Preserve myTrib(tcnt)
              myTrib(tcnt).Id = lConn.Source.volid
              myTrib(tcnt).OpNam = lConn.Source.volname
              myTrib(tcnt).Area = lConn.MFact
              myTrib(tcnt).Desc = lConn.Source.Opn.Description
              myTrib(tcnt).BMPCnt = 0
              ReDim myTrib(tcnt).BmpPtr(0)
            End If
          End If
        End If
      End If
    Next vConn
    
    For i = 1 To bcnt 'loop thru bmps to find their tribs
      If myBmp(i).InUseNow Then
        Id = myBmp(i).Id
        Set lOpnBlk = myUci.OpnBlks("BMPRAC")  'bmp
        Set lOper = lOpnBlk.operfromid(Id)
    
        For Each vConn In myUci.Connections
          Set lConn = vConn
          If lConn.Typ = 3 Or lConn.Typ = 2 Then 'network or schematic
            If lConn.Target.Opn.Id = lOper.Id And _
               lConn.Target.Opn.Name = lOper.Name Then 'found a source
              If lConn.Source.volname = "BMPRAC" Then
                MsgBox "PROBLEM - BMPs cant go to BMPs in the WinHSPF BMP Editor", vbOKOnly, msgTitle
                cmdBMP_Click (2) 'close up bmp editing
              End If
              found = False
              For j = 1 To tcnt
                If myTrib(j).OpNam = lConn.Source.volname Then
                  If myTrib(j).Id = lConn.Source.Opn.Id Then
                    'know about this trib
                    found = True
                    Exit For
                  End If
                End If
              Next j
              If Not (found) Then 'add to trib list
                tcnt = tcnt + 1
                j = tcnt
                ReDim Preserve myTrib(j)
                myTrib(j).Desc = lConn.Source.Opn.Description
                myTrib(j).Id = lConn.Source.Opn.Id
                myTrib(j).OpNam = lConn.Source.volname
              End If
            
              With myTrib(j)
                .BMPCnt = .BMPCnt + 1
                ReDim Preserve .BmpPtr(.BMPCnt)
                .BmpPtr(.BMPCnt).Ind = i
                .BmpPtr(.BMPCnt).Area = lConn.MFact
                .Area = .Area + lConn.MFact
              End With
            End If
          End If
        Next vConn
      End If
    Next i 'bmp
    
    For i = 1 To bcnt
      If myBmp(i).InUseNow Then
        For j = 1 To UBound(myTrib)
          found = False
          For k = 1 To myTrib(j).BMPCnt
            If myTrib(j).BmpPtr(k).Ind = i Then
              found = True
              Exit For
            ElseIf myTrib(j).BmpPtr(k).Ind > i Then
              With myTrib(j)
                .BMPCnt = .BMPCnt + 1
                ReDim Preserve .BmpPtr(.BMPCnt)
                For h = .BMPCnt To k + 1 Step -1
                  .BmpPtr(h) = .BmpPtr(h - 1)
                Next h
                .BmpPtr(k).Ind = i
                .BmpPtr(k).Area = 0
              End With
              found = True 'we added it
              Exit For
            End If
          Next k
          If Not found Then 'add to end
            With myTrib(j)
              .BMPCnt = .BMPCnt + 1
              ReDim Preserve .BmpPtr(.BMPCnt)
              .BmpPtr(.BMPCnt).Ind = i
              .BmpPtr(.BMPCnt).Area = 0
            End With
          End If
        Next j
      End If
    Next i
  End If
End Sub

Private Sub BmpDescUpdate(i&)
  Dim j&
  
  If i > 0 Then
    curBMPId = Mid(grdSrc.ColTitle(grdSrc.col), 6, Len(grdSrc.ColTitle(grdSrc.col)) - 4)
    For j = 1 To UBound(myBmp)
      If curBMPId = myBmp(j).Id Then
        atxBMPId.Value = myBmp(j).Id
        txtBMPDesc.Text = myBmp(j).Desc
        Exit For
      End If
    Next j
    fraBMPDet.Visible = True
  Else
    curBMPId = 0
    fraBMPDet.Visible = False
  End If
  
End Sub

Private Sub PopulateGrid()
  Dim bcnt&, tcnt&, rcnt&, ctxt$, barea!, rarea!, cur&, i&, j&, k&, buse&
  Dim exist As Boolean, temptitle$
  
  bcnt = UBound(myBmp)
  tcnt = UBound(myTrib)
  cur = cmbReach.ListIndex
  
  grdSrc.Clear
  grdSrc.rows = 1
  rcnt = 0
  grdSrc.FixedCols = 1
  
  If cur = 0 Then
    'summary style grid
    grdSrc.Header = "Summary of Areas by LandUse and Reach"
    grdSrc.cols = 1
    For i = 1 To cmbReach.listcount - 1
      rcnt = rcnt + 1
      grdSrc.TextMatrix(rcnt, 0) = "R:" & cmbReach.List(i)
      Call GetUciInfo(i)
      For j = 1 To UBound(myTrib)
        If myTrib(j).OpNam <> "RCHRES" Then
          exist = False
          If InStr(1, myTrib(j).Desc, ":") Then
            temptitle = myTrib(j).Desc
          Else
            temptitle = Mid(myTrib(j).OpNam, 1, 1) & ":" & myTrib(j).Desc
          End If
          For k = 1 To grdSrc.cols
            If grdSrc.ColTitle(k) = temptitle Then
              exist = True
              Exit For
            End If
          Next k
          If Not exist Then
            k = grdSrc.cols
            grdSrc.ColTitle(grdSrc.cols) = temptitle
            grdSrc.ColEditable(k) = False
            grdSrc.ColType(k) = ATCoSng
          End If
          'If rdoOpt(0) Then
          grdSrc.TextMatrix(rcnt, k) = myTrib(j).Area
          'Else
          '  grdSrc.TextMatrix(rcnt, k) = myTrib(j).id
          'End If
        End If
      Next j
    Next i
    fraBMPDet.Visible = False
    cmdBMP(0).Visible = False
    cmdBMP(1).Visible = False
  Else
    'build reach/bmp grid
    grdSrc.Header = "Contributing Sources to Reach " & MyRch(cur).Id & " (" & MyRch(cur).Desc & ")"
  
    grdSrc.ColTitle(0) = "Source"
    grdSrc.ColEditable(0) = False
    grdSrc.ColTitle(1) = "Area"
    grdSrc.ColEditable(1) = False
    grdSrc.ColTitle(2) = "% No BMP"
    grdSrc.ColEditable(2) = True
    grdSrc.ColMax(2) = 100
    grdSrc.ColType(2) = ATCoSng
    grdSrc.cols = 3
  
    buse = 0
    For i = 1 To bcnt
      If myBmp(i).InUseNow And Not (myBmp(i).DeletePending) Then
        myBmp(i).col = grdSrc.cols
        grdSrc.cols = grdSrc.cols + 1
        buse = buse + 1
        grdSrc.ColTitle(2 + buse) = "% BMP " & myBmp(i).Id
        grdSrc.ColEditable(2 + buse) = True
        grdSrc.ColMin(2 + buse) = 0
        grdSrc.ColMax(2 + buse) = 100
        grdSrc.ColType(2 + buse) = ATCoSng
      Else
        myBmp(i).col = 0
      End If
    Next i
  
    If buse = 0 Then ' no bmps, % must be 100
      grdSrc.ColMin(2) = 100
      fraBMPDet.Visible = False
      grdSrc.ColEditable(2) = False
    Else
      grdSrc.ColMin(2) = 0
      fraBMPDet.Visible = True
      grdSrc.ColEditable(2) = True
    End If
  
    grdSrc.ColWidth(0) = grdSrc.Width * 0.4
    grdSrc.ColWidth(1) = (grdSrc.Width - grdSrc.ColWidth(0) - 40) / CSng(2 + buse)
    For i = 2 To 2 + buse
      grdSrc.ColWidth(i) = grdSrc.ColWidth(1)
    Next i
    
    For j = 1 To tcnt 'trib source areas and %
      If myTrib(j).OpNam <> "BMPRAC" Then
        rcnt = rcnt + 1
        grdSrc.rows = rcnt
        ctxt = myTrib(j).OpNam & " : " & myTrib(j).Id & " (" & myTrib(j).Desc & ")"
        grdSrc.TextMatrix(rcnt, 0) = ctxt
        barea = 0
        If Left(ctxt, 3) = "RCH" Then
          grdSrc.TextMatrix(j, 1) = "NA"
        Else
          grdSrc.TextMatrix(j, 1) = myTrib(j).Area
        End If
        buse = 0
        For i = 1 To bcnt
          If myBmp(i).InUseNow And Not (myBmp(i).DeletePending) Then
            buse = buse + 1
            If buse <= myTrib(j).BMPCnt Then
              rarea = myTrib(j).BmpPtr(buse).Area
            Else
              rarea = 0
            End If
            grdSrc.TextMatrix(j, 2 + buse) = 100 * (rarea / myTrib(j).Area)
            barea = barea + rarea
          End If
        Next i
        grdSrc.TextMatrix(j, 2) = 100 - 100 * (barea / myTrib(j).Area)
      End If
    Next j
    ' default pos to first row, bmp col (if avail)
    grdSrc.row = 1
    grdSrc.col = 2 + buse
    BmpDescUpdate (buse)
    If buse > 0 Then
      fraBMPDet.Visible = True
    End If
    cmdBMP(0).Visible = True
    cmdBMP(1).Visible = True
  End If
  
  grdSrc.ColsSizeByContents
  
End Sub

Private Sub atxBMPId_LostFocus()
  Dim i&, bcnt&
  
  If atxBMPId.Enabled = True Then
    bcnt = UBound(myBmp)
    For i = 1 To bcnt
      If myBmp(i).Id = atxBMPId.Value Then
        MsgBox "BMP id " & myBmp(i).Id & " is already in use" & vbCrLf & _
                "Try another ID number", vbOKOnly, msgTitle
        atxBMPId.Value = curBMPId
        Exit Sub
      End If
    Next i
  
    curBMPId = atxBMPId.Value
  
    grdSrc.ColTitle(grdSrc.col) = "% BMP " & curBMPId
    myBmp(grdSrc.col - 2).Id = curBMPId
  
    cmdUpU.Enabled = True
  End If
  
End Sub

Private Sub cmbReach_Click()
  If cmbReach.ListIndex = 0 Then
    rdoOpt(0).Visible = False 'True
    rdoOpt(1).Visible = False 'True
  Else
    rdoOpt(0).Visible = False
    rdoOpt(1).Visible = False
  End If
  Call GetUciInfo(cmbReach.ListIndex)
  Call PopulateGrid
  cmdUpU.Enabled = False
  atxBMPId.Enabled = False
End Sub

Private Sub cmdBMP_Click(Index As Integer)
  Dim ans&, i&, j&, k&, cur&, nth&, ncnt&, curbmp&
  Dim done As Boolean
  
  cur = cmbReach.ListIndex

  If Index = 0 Then 'add bmp
    i = UBound(myBmp) + 1
    ReDim Preserve myBmp(i)
    ' assign an id not in use
    j = 0
    done = False
    Do Until done
      done = True ' assume the best
      For k = 1 To i - 1
        If myBmp(k).Id = MyRch(cur).Id + j Then 'in use
          j = j + 1
          done = False
          Exit For
        End If
      Next k
    Loop
    
    myBmp(i).Id = MyRch(cur).Id + j
    myBmp(i).Desc = "New BMP"
    myBmp(i).InUseNow = True
    For j = 1 To UBound(myTrib)
      myTrib(j).BMPCnt = myTrib(j).BMPCnt + 1
      k = myTrib(j).BMPCnt
      ReDim Preserve myTrib(j).BmpPtr(k)
      myTrib(j).BmpPtr(k).Area = 0
      myTrib(j).BmpPtr(k).Ind = i
    Next j
    MyRch(cur).BMPCnt = MyRch(cur).BMPCnt + 1
    Call PopulateGrid
    cmdUpU.Enabled = True
    atxBMPId.Enabled = True
  ElseIf Index = 1 Then 'delete bmp
    If MyRch(cur).BMPCnt = 0 Then
      MsgBox "No BMPs are associated with Reach " & MyRch(cur).Id, _
             vbExclamation, msgTitle
    ElseIf grdSrc.col > 2 Then 'are in a bmp
      'delete nth bmp
      nth = grdSrc.col - 2
      ncnt = 0
      For j = 1 To UBound(myBmp)
        If myBmp(j).InUseNow = True Then
          ncnt = ncnt + 1
          If nth = ncnt Then
            'this is the one to delete
            curbmp = myBmp(j).Id
            nth = j
          End If
        End If
      Next j
      curbmp = atxBMPId.Value
      ans = MsgBox("Are you sure you want to Delete BMP " & curbmp & "?", _
             vbYesNo, msgTitle)
      If ans = vbYes Then
        myBmp(nth).DeletePending = True
        myBmp(nth).InUseNow = False
        MyRch(cur).BMPCnt = MyRch(cur).BMPCnt - 1
        For j = 1 To UBound(myTrib)
          'adjust here
        Next j
        grdSrc.cols = grdSrc.cols - 1
        grdSrc.col = 2
        Call PopulateGrid
        cmdUpU.Enabled = True
     End If
    End If
  ElseIf Index = 2 Then 'close
    If cmdUpU.Enabled = True Then
      ans = MsgBox("You have changes to your UCI made which have not been saved. " & vbCrLf & _
                   "OK trashes them, Cancel allows you a chance to update your UCI.", _
                   vbOKCancel, msgTitle)
    Else 'no changes pending
      ans = vbOK
    End If
    
    If ans = vbOK Then
      Unload Me
    End If
  End If
End Sub

Private Sub cmdEdit_Click()
    frmBMPEffic.Show 1
End Sub

Private Sub cmdUpU_Click()
  Dim i&, j&, k&, ret&, rchid&, c&, bmpId&, itmnam$
  Dim lOpn As HspfOperation, addit As Boolean, a#
  Dim lOpnBlk As HspfOpnBlk, addbefore&
  Dim vConn As Variant
  Dim lConn As HspfConnection
  Dim lTable As HspfTable
  
  rchid = MyRch(cmbReach.ListIndex).Id
  
  atxBMPId.Enabled = False
  For i = 1 To UBound(myBmp) 'process deletes first
    If myBmp(i).DeletePending Then 'get rid of it
      myUci.DeleteOperation "BMPRAC", myBmp(i).Id
      myBmp(i).InUseNow = False
    End If
  Next i
  
  'put areas back for the bmps that are in use
  For i = 1 To UBound(myBmp)
    If myBmp(i).InUseNow Then
    
      'see if we need to add this bmp to opn seq block
      Set lOpnBlk = myUci.OpnBlks("BMPRAC")
      addit = True
      If lOpnBlk.Count > 0 Then
        Set lOpn = lOpnBlk.operfromid(myBmp(i).Id)
        If Not lOpn Is Nothing Then addit = False
      End If
      If addit Then
        'add new bmprac operation
        myUci.AddOperation "BMPRAC", myBmp(i).Id
        'figure out where to put it in opn seq block
        addbefore = myUci.OpnSeqBlock.Opns.Count
        For j = 1 To myUci.OpnSeqBlock.Opns.Count
          If myUci.OpnSeqBlock.Opn(j).Name = "RCHRES" And _
             myUci.OpnSeqBlock.Opn(j).Id = rchid Then
            addbefore = j
          End If
        Next j
        'now add to opn seq block
        Set lOpn = lOpnBlk.operfromid(myBmp(i).Id)
        lOpn.Description = myBmp(i).Desc
        Set lOpn.Uci = myUci
        Set lOpn.OpnBlk = lOpnBlk
        myUci.OpnSeqBlock.addbefore lOpn, addbefore
        If lOpnBlk.Count > 1 Then
          'already have some of this operation
          For Each lTable In lOpnBlk.Ids(1).tables
            'add this opn id to this table
            myUci.AddTable "BMPRAC", myBmp(i).Id, lTable.Name
          Next lTable
        Else
          'added the first bmprac, need to add associated tables
          Dim lBlock As HspfBlockDef
          Dim vSection As Variant, lSection As HspfSectionDef
          Dim vTable As Variant, lTabledef As HspfTableDef
          Set lBlock = myMsg.BlockDefs("BMPRAC")
          For Each vSection In lBlock.SectionDefs
            Set lSection = vSection
            For Each vTable In lSection.TableDefs
              Set lTabledef = vTable
              myUci.AddTable "BMPRAC", myBmp(i).Id, lTabledef.Name
            Next vTable
          Next vSection
        End If
        Set lTable = lOpnBlk.tables("GEN-INFO")
        lTable.Parms("BMPID").Value = myBmp(i).Desc
        lTable.Parms("NGQUAL").Value = 0   'assume no gquals
        Set lTable = lOpnBlk.tables("GQ-FRAC")
        lTable.Parms("GQID").Value = "unknown"
      End If
      
      'look for bmp to rchres connection, add it if not existing
      Call PutSchematicRecord("BMPRAC", myBmp(i).Id, "RCHRES", rchid, 1#)
    End If
  Next i
  
  For j = 1 To UBound(myTrib)
    'put area going directly to rch
    a = myTrib(j).Area * CSng((grdSrc.TextMatrix(j, 2) / 100))
    Call PutSchematicRecord(myTrib(j).OpNam, myTrib(j).Id, "RCHRES", rchid, a)
    'put area going to bmps
    For k = 1 To myTrib(j).BMPCnt
      If myBmp(myTrib(j).BmpPtr(k).Ind).InUseNow Then
        bmpId = myBmp(myTrib(j).BmpPtr(k).Ind).Id
        c = myBmp(myTrib(j).BmpPtr(k).Ind).col
        a = myTrib(j).Area * CSng((grdSrc.TextMatrix(j, c) / 100))
        Call PutSchematicRecord(myTrib(j).OpNam, myTrib(j).Id, "BMPRAC", bmpId, a)
      End If
    Next k
  Next j
   
  Call GetUciInfo(cmbReach.ListIndex) 'refresh with new data
  Call PopulateGrid
  cmdUpU.Enabled = False
    
End Sub

Private Sub Form_Load()
  Dim iexist As Boolean, itmnam$, OpId&, cnt&, i&
  Dim lOpnBlk As HspfOpnBlk
  Dim lTable As HspfTable
  Dim lOper As HspfOperation
  
  Me.Icon = HSPFMain.Icon
  msgTitle = "Best Management Practices Editor"
  'are any reaches available?
  Set lOpnBlk = myUci.OpnBlks("RCHRES")
  ReDim MyRch(0)
  If lOpnBlk.Count > 0 Then
    'what reaches are available?
    cmbReach.Clear
    cmbReach.AddItem "Summary"
    ReDim MyRch(lOpnBlk.Count)
    'get ids
    For i = 1 To lOpnBlk.Count
      Set lOper = lOpnBlk.Ids(i)
      MyRch(i).Id = lOper.Id
      'get reach names
      Set lTable = lOper.tables("GEN-INFO")
      MyRch(i).Desc = lTable.Parms("RCHID")
      If InStr(MyRch(i).Desc, ":") Then 'assume id already present by convention
        cmbReach.AddItem MyRch(i).Desc
      Else
        cmbReach.AddItem MyRch(i).Id & ":" & MyRch(i).Desc
      End If
    Next i
    
    'get all bmp info
    Set lOpnBlk = myUci.OpnBlks("BMPRAC")
    ReDim myBmp(0)
    If lOpnBlk.Count > 0 Then
      ReDim myBmp(lOpnBlk.Count)
      For i = 1 To lOpnBlk.Count
        Set lOper = lOpnBlk.Ids(i)
        myBmp(i).Id = lOper.Id
        Set lTable = lOper.tables("GEN-INFO")
        myBmp(i).Desc = lTable.Parms("BMPID")
        myBmp(i).DeletePending = False
      Next i
    End If
    
    cmbReach.ListIndex = 0
    Call PopulateGrid
    
  Else
    MsgBox "The BMP option requires a scenario containing a RCHRES block." & vbCrLf & _
          "The current scenario does not have a RCHRES block.", _
          vbExclamation, msgTitle
    cmdBMP_Click (2) 'close
  End If
    
End Sub

Private Sub Form_Resize()
  Dim h%
  
  h = grdSrc.Top + cmdBMP(0).Height + 600
  If Me.Width > 500 And h < Me.Height Then
    grdSrc.Width = Me.Width - 100
    grdSrc.Height = Me.Height - h
    cmdUpU.Top = grdSrc.Top + grdSrc.Height + 100
    cmdBMP(2).Top = cmdUpU.Top
  End If
End Sub

Private Sub grdSrc_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim t!, i&, r&, c&, a&, cNow&
  
  c = ChangeToCol
  
  For r = ChangeFromRow To ChangeToRow
    t = grdSrc.TextMatrix(r, c) 'highest priority is just set value
    
    cNow = 2
    For i = 1 To UBound(myBmp) ' each bmp
      If myBmp(i).InUseNow And Not (myBmp(i).DeletePending) Then
        cNow = cNow + 1
        If cNow <> c Then 'dont double count current
          t = t + grdSrc.TextMatrix(r, cNow)
          If t > 100 Then
            grdSrc.TextMatrix(r, cNow) = grdSrc.TextMatrix(r, cNow) - t + 100
            t = 100
          End If
        End If
      End If
    Next i

    If t < 100 Then 'need some more
      If c = 2 Then ' changing no bmp col, adjust first bmp
        grdSrc.TextMatrix(r, 3) = 100 - (t + grdSrc.TextMatrix(r, 3))
      Else 'changing a bmp, adjust no bmp
        grdSrc.TextMatrix(r, 2) = 100 - t
      End If
    ElseIf c > 2 Then
      grdSrc.TextMatrix(r, 2) = 100 - t
    End If
  Next r
  
  cmdUpU.Enabled = True

End Sub

Private Sub grdSrc_RowColChange()
  If grdSrc.col >= 2 And Left(grdSrc.Header, 7) <> "Summary" Then
    BmpDescUpdate (grdSrc.col - 2)
  End If
End Sub

Private Sub rdoOpt_Click(Index As Integer)
  Call PopulateGrid
End Sub

Private Sub txtBMPDesc_Change()
  Dim i&
  For i = 1 To UBound(myBmp)
    If myBmp(i).InUseNow Then
       myBmp(i).Desc = txtBMPDesc.Text
    End If
  Next i
  cmdUpU.Enabled = True
End Sub

Private Sub PutSchematicRecord(sname$, sid&, tname$, tid&, multfact#)
  Dim addit As Boolean, mlid&, deleteit As Boolean
  Dim vConn As Variant, deleteindex&
  Dim lConn As HspfConnection
  Dim lOpnBlk As HspfOpnBlk, i&
  Dim lOpn As HspfOperation, tempOpn As HspfOperation

  If sname = "RCHRES" And tname = "BMPRAC" Then 'dont do rchres to bmp connections
  Else
    If sname = "RCHRES" And tname = "RCHRES" Then
      multfact = 1#
    End If
    addit = True
    deleteit = False
    For i = 1 To myUci.Connections.Count
      Set lConn = myUci.Connections(i)
      If lConn.Typ = 3 Then 'schematic
        If lConn.Target.Opn.Id = tid And _
           lConn.Target.Opn.Name = tname And _
           lConn.Source.Opn.Id = sid And _
           lConn.Source.Opn.Name = sname Then
          addit = False
          lConn.MFact = multfact
          If Abs(multfact) < 0.00000001 Then
            deleteit = True
            deleteindex = i
          End If
        End If
      End If
    Next i
    If addit And Abs(multfact) > 0.00000001 Then 'need to add the connection
      Set lConn = New HspfConnection
      Set lOpnBlk = myUci.OpnBlks(sname)
      Set lOpn = lOpnBlk.operfromid(sid)
      Set lConn.Source.Opn = lOpn
      lConn.Source.volname = lOpn.Name
      lConn.Source.volid = lOpn.Id
      lConn.Typ = 3
      lConn.MFact = multfact
      Set lOpnBlk = myUci.OpnBlks(tname)
      Set lOpn = lOpnBlk.operfromid(tid)
      Set lConn.Target.Opn = lOpn
      lConn.Target.volname = lOpn.Name
      lConn.Target.volid = lOpn.Id
      Call GetMassLinkID(sname, tname, mlid)
      If mlid = 0 Then
        Call AddMassLink(sname, tname, mlid)
      End If
      lConn.MassLink = mlid
      Set lConn.Uci = myUci
      'add targets to source opn
      Set lOpnBlk = myUci.OpnBlks(sname)
      Set lOpn = lOpnBlk.operfromid(sid)
      lOpn.targets.Add lConn
      Set lConn.Source.Opn = lOpn
      'add sources to target opn
      Set lOpnBlk = myUci.OpnBlks(tname)
      Set lOpn = lOpnBlk.operfromid(tid)
      lOpn.Sources.Add lConn
      Set lConn.Target.Opn = lOpn
      'add to collection of connections
      myUci.Connections.Add lConn
    ElseIf deleteit Then 'need to delete the connection
      myUci.Connections.Remove deleteindex
      'remove target from source opn
      Set lOpnBlk = myUci.OpnBlks(sname)
      Set lOpn = lOpnBlk.operfromid(sid)
      i = 1
      For Each vConn In lOpn.targets
        Set lConn = vConn
        If lConn.Target.volname = tname And _
           lConn.Target.volid = tid Then
          lOpn.targets.Remove i
        Else
          i = i + 1
        End If
      Next vConn
      'remove source from target opn
      Set lOpnBlk = myUci.OpnBlks(tname)
      Set lOpn = lOpnBlk.operfromid(tid)
      i = 1
      For Each vConn In lOpn.Sources
        Set lConn = vConn
        If lConn.Source.volname = sname And _
           lConn.Source.volid = sid Then
          lOpn.Sources.Remove i
        Else
          i = i + 1
        End If
      Next vConn
    End If
  End If
End Sub

Private Sub GetMassLinkID(sname$, tname$, mlid&)
  Dim lConn As HspfConnection
  
  'determine mass link number
  mlid = 0
  For Each lConn In myUci.Connections
    If lConn.Typ = 3 Then
      If lConn.Source.volname = sname And lConn.Target.volname = tname Then
        mlid = lConn.MassLink
      End If
    End If
  Next lConn
End Sub

Private Sub AddMassLink(sname$, tname$, mlid&)
  Dim lOpn As HspfOperation
  Dim cOpns As Collection 'of hspfOperations
  Dim i&, j&, found As Boolean
  Dim pml&, iml&, ostr$(10)
  Dim lConn As HspfConnection, lMassLink As HspfMassLink
  Dim cMassLink As HspfMassLink, copyid&

  'need to add masslink, find an unused number
  found = True
  mlid = 1
  Do Until found = False
    found = False
    For Each lMassLink In myUci.MassLinks
      If lMassLink.MassLinkID = mlid Then
        mlid = mlid + 1
        found = True
        Exit For
      End If
    Next lMassLink
  Loop
  'find id of masslink to copy
  copyid = 0
  If sname = "BMPRAC" And tname = "RCHRES" Then
    'copy from perlnd to rchres masslink
    Call GetMassLinkID("PERLND", tname, copyid)
  ElseIf sname = "PERLND" And tname = "BMPRAC" Then
    'copy from perlnd to rchres masslink
    Call GetMassLinkID(sname, "RCHRES", copyid)
  ElseIf sname = "IMPLND" And tname = "BMPRAC" Then
    'copy from implnd to rchres masslink
    Call GetMassLinkID(sname, "RCHRES", copyid)
  End If
  If mlid > 0 And copyid > 0 Then
    'now copy masslink
    For Each cMassLink In myUci.MassLinks
      If cMassLink.MassLinkID = copyid Then
        'copy this record
        Set lMassLink = New HspfMassLink
        Set lMassLink.Uci = myUci
        lMassLink.MassLinkID = mlid
        lMassLink.Source.volname = sname
        lMassLink.Source.volid = 0
        lMassLink.Source.group = cMassLink.Source.group
        lMassLink.Source.member = cMassLink.Source.member
        lMassLink.Source.memsub1 = cMassLink.Source.memsub1
        lMassLink.Source.memsub2 = cMassLink.Source.memsub2
        lMassLink.MFact = cMassLink.MFact
        lMassLink.Tran = cMassLink.Tran
        lMassLink.Target.volname = tname
        lMassLink.Target.volid = 0
        lMassLink.Target.group = cMassLink.Target.group
        lMassLink.Target.member = cMassLink.Target.member
        lMassLink.Target.memsub1 = cMassLink.Target.memsub1
        lMassLink.Target.memsub2 = cMassLink.Target.memsub2
        
        If (sname = "PERLND" Or sname = "IMPLND") And _
          tname = "BMPRAC" Then  'special cases
          If cMassLink.Target.member = "OXIF" Then
            lMassLink.Target.member = "IOX"
          ElseIf cMassLink.Target.member = "NUIF1" Then
            lMassLink.Target.member = "IDNUT"
          ElseIf cMassLink.Target.member = "NUIF2" Then
            lMassLink.Target.member = "ISNUT"
          ElseIf cMassLink.Target.member = "PKIF" Then
            lMassLink.Target.member = "IPLK"
          End If
        End If
        
        If sname = "BMPRAC" And tname = "RCHRES" Then
          'special cases
          lMassLink.Source.group = "ROFLOW"
          lMassLink.MFact = 1#
          If cMassLink.Target.member = "IVOL" Then
            lMassLink.Source.member = "ROVOL"
          ElseIf cMassLink.Target.member = "CIVOL" Then
            lMassLink.Source.member = "CROVOL"
          ElseIf cMassLink.Target.member = "ICON" Then
            lMassLink.Source.member = "ROCON"
          ElseIf cMassLink.Target.member = "IHEAT" Then
            lMassLink.Source.member = "ROHEAT"
          ElseIf cMassLink.Target.member = "ISED" Then
            lMassLink.Source.member = "ROSED"
          ElseIf cMassLink.Target.member = "IDQAL" Then
            lMassLink.Source.member = "RODQAL"
          ElseIf cMassLink.Target.member = "ISQAL" Then
            lMassLink.Source.member = "ROSQAL"
          ElseIf cMassLink.Target.member = "OXIF" Then
            lMassLink.Source.member = "ROOX"
          ElseIf cMassLink.Target.member = "NUIF1" Then
            lMassLink.Source.member = "RODNUT"
          ElseIf cMassLink.Target.member = "NUIF2" Then
            lMassLink.Source.member = "ROSNUT"
          ElseIf cMassLink.Target.member = "PKIF" Then
            lMassLink.Source.member = "ROPLK"
          ElseIf cMassLink.Target.member = "PHIF" Then
            lMassLink.Source.member = "ROPH"
          End If
          lMassLink.Source.memsub1 = cMassLink.Target.memsub1
          lMassLink.Source.memsub2 = cMassLink.Target.memsub2
        End If

        myUci.MassLinks.Add lMassLink
      End If
    Next cMassLink
  End If
   
End Sub

