VERSION 5.00
Begin VB.Form frmGenActModBMP 
   Caption         =   "GenScn Activate Modify BMP"
   ClientHeight    =   5292
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   9948
   HelpContextID   =   56
   LinkTopic       =   "Form1"
   ScaleHeight     =   5292
   ScaleWidth      =   9948
   StartUpPosition =   3  'Windows Default
   Begin VB.OptionButton rdoOpt 
      Caption         =   "ID"
      Height          =   255
      Index           =   1
      Left            =   8040
      TabIndex        =   2
      Top             =   120
      Width           =   735
   End
   Begin VB.OptionButton rdoOpt 
      Caption         =   "Area"
      Height          =   255
      Index           =   0
      Left            =   7200
      TabIndex        =   1
      Top             =   120
      Value           =   -1  'True
      Width           =   735
   End
   Begin VB.Frame fraBMPDet 
      Caption         =   "Current BMP Details"
      Height          =   1215
      Left            =   1680
      TabIndex        =   12
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
         TabIndex        =   6
         Top             =   240
         Width           =   2415
      End
      Begin VB.TextBox txtBMPDesc 
         Height          =   285
         Left            =   1200
         TabIndex        =   7
         Text            =   "BMPDescription"
         Top             =   720
         Width           =   5295
      End
      Begin ATCoCtl.ATCoText atxBMPId 
         Height          =   252
         Left            =   1200
         TabIndex        =   5
         Top             =   360
         Width           =   732
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
         TabIndex        =   14
         Top             =   360
         Width           =   735
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         Caption         =   "Description:"
         Height          =   255
         Left            =   240
         TabIndex        =   13
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
      TabIndex        =   10
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
      TabIndex        =   11
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
      TabIndex        =   9
      Top             =   1920
      Width           =   9735
      _ExtentX        =   17166
      _ExtentY        =   4890
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
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
      TabIndex        =   8
      Top             =   120
      Width           =   3975
   End
End
Attribute VB_Name = "frmGenActModBMP"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Type BmpInfo
  id As Long
  desc As String
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
  id As Long
  OpNam As String
  desc As String
  Area As Single
  BMPCnt As Long
  BmpPtr() As BmpPtrInfo
End Type
Private myTrib() As TribInfo

Private Type RchInfo
  id As Long
  desc As String
  BMPCnt As Long
End Type
Private MyRch() As RchInfo
Private msgTitle$
Private curBMPId&
Public BMPDesc$

Private Sub GetUciInfo(cur&)
  
  Dim Init&, id&, rorb&
  Dim ctxt$, cmor$, pnam$, rcnt&, tcnt&
  Dim i&, j&, k&, h&, bcnt&, found As Boolean
  Dim rarea!, barea!
  
  id = MyRch(cur).id
  'assume bmps not in use for this reach
  For i = 1 To UBound(myBmp)
    myBmp(i).InUseNow = False
  Next i
  
  rorb = 1 'reach
  ReDim myTrib(0)
  tcnt = 0
  bcnt = UBound(myBmp)
  Init = 1
  Do
    Call F90_GTINS(Init, id, rorb, ctxt, rarea)
    Init = 0
    If IsNumeric(ctxt) Then
      Exit Do
    ElseIf Left(ctxt, 3) = "BMP" Then
      j = CLng(Right(ctxt, 4))
      For i = 1 To bcnt
         If myBmp(i).id = j Then
           myBmp(i).InUseNow = True
           MyRch(cur).BMPCnt = MyRch(cur).BMPCnt + 1
           Exit For
         End If
      Next i
    Else
      pnam = Left(ctxt, 3) & "DESC "
      Call F90_HGETC(pnam, CLng(Right(ctxt, 4)), cmor, Len(pnam))
      tcnt = tcnt + 1
      ReDim Preserve myTrib(tcnt)
      myTrib(tcnt).id = CLng(Right(ctxt, 4))
      myTrib(tcnt).OpNam = Left(ctxt, 6)
      myTrib(tcnt).Area = rarea
      myTrib(tcnt).desc = cmor
      myTrib(tcnt).BMPCnt = 0
      ReDim myTrib(tcnt).BmpPtr(0)
    End If
  Loop
  
  For i = 1 To bcnt 'loop thru bmps to find their tribs
    If myBmp(i).InUseNow Then
      Init = 1
      id = myBmp(i).id
      rorb = 2 'bmp
      Do
        Call F90_GTINS(Init, id, rorb, ctxt, rarea)
        Init = 0
        If IsNumeric(ctxt) Then
          Exit Do
        Else
          pnam = Left(ctxt, 3) & "DESC "
          If Left(ctxt, 3) = "BMP" Then
            MsgBox "PROBLEM - BMPs cant go to BMPs in GenScn", vbOKOnly, msgTitle
            cmdBMP_Click (2) 'close up bmp editing
          End If
          Call F90_HGETC(pnam, CLng(Right(ctxt, 4)), cmor, Len(pnam))
          found = False
          For j = 1 To tcnt
            If myTrib(j).OpNam = Left(ctxt, 6) Then
              If myTrib(j).id = CLng(Right(ctxt, 4)) Then
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
            myTrib(j).desc = cmor
            myTrib(j).id = CLng(Right(ctxt, 4))
            myTrib(j).OpNam = Left(ctxt, 6)
          End If
        
          With myTrib(j)
            .BMPCnt = .BMPCnt + 1
            ReDim Preserve .BmpPtr(.BMPCnt)
            .BmpPtr(.BMPCnt).Ind = i
            .BmpPtr(.BMPCnt).Area = rarea
            .Area = .Area + rarea
          End With
        End If
      Loop
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
End Sub

Private Sub BmpDescUpdate(i&)
  Dim j&
  
  If i > 0 Then
    curBMPId = Mid(grdSrc.ColTitle(grdSrc.col), 6, Len(grdSrc.ColTitle(grdSrc.col)) - 4)
    For j = 1 To UBound(myBmp)
      If curBMPId = myBmp(j).id Then
        atxBMPId.value = myBmp(j).id
        txtBMPDesc.text = myBmp(j).desc
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
  Dim exist As Boolean
  
  bcnt = UBound(myBmp)
  tcnt = UBound(myTrib)
  cur = cmbReach.ListIndex
  
  grdSrc.clear
  grdSrc.Rows = 1
  rcnt = 0
  grdSrc.FixedCols = 1
  
  If cur = 0 Then
    'summary style grid
    grdSrc.Header = "Summary of Areas by LandUse and Reach"
    grdSrc.cols = 1
    For i = 1 To cmbReach.ListCount - 1
      rcnt = rcnt + 1
      grdSrc.TextMatrix(rcnt, 0) = "R:" & cmbReach.List(i)
      Call GetUciInfo(i)
      For j = 1 To UBound(myTrib)
        If myTrib(j).OpNam <> "RCHRES" Then
          exist = False
          For k = 1 To grdSrc.cols
            If grdSrc.ColTitle(k) = myTrib(j).desc Then
              exist = True
              Exit For
            End If
          Next k
          If Not exist Then
            k = grdSrc.cols
            grdSrc.ColTitle(grdSrc.cols) = myTrib(j).desc
            grdSrc.ColEditable(k) = False
            grdSrc.ColType(k) = ATCoSng
          End If
          If rdoOpt(0) Then
            grdSrc.TextMatrix(rcnt, k) = myTrib(j).Area
          Else
            grdSrc.TextMatrix(rcnt, k) = myTrib(j).id
          End If
        End If
      Next j
    Next i
    fraBMPDet.Visible = False
    cmdBMP(0).Visible = False
    cmdBMP(1).Visible = False
  Else
    'build reach/bmp grid
    grdSrc.Header = "Contributing Sources to Reach " & MyRch(cur).id & " (" & MyRch(cur).desc & ")"
  
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
        grdSrc.ColTitle(2 + buse) = "% BMP " & myBmp(i).id
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
  
    grdSrc.colWidth(0) = grdSrc.Width * 0.4
    grdSrc.colWidth(1) = (grdSrc.Width - grdSrc.colWidth(0) - 40) / CSng(2 + buse)
    For i = 2 To 2 + buse
      grdSrc.colWidth(i) = grdSrc.colWidth(1)
    Next i
    
    For j = 1 To tcnt 'trib source areas and %
      If myTrib(j).OpNam <> "BMPRAC" Then
        rcnt = rcnt + 1
        grdSrc.Rows = rcnt
        ctxt = myTrib(j).OpNam & " : " & myTrib(j).id & " (" & myTrib(j).desc & ")"
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
  
  'grdSrc.ColsSizeByContents
  
End Sub

Private Sub atxBMPId_LostFocus()
  Dim i&, bcnt&
  
  If atxBMPId.Enabled = True Then
    bcnt = UBound(myBmp)
    For i = 1 To bcnt
      If myBmp(i).id = atxBMPId.value Then
        MsgBox "BMP id " & myBmp(i).id & " is already in use" & vbCrLf & _
                "Try another ID number", vbOKOnly, msgTitle
        atxBMPId.value = curBMPId
        Exit Sub
      End If
    Next i
  
    curBMPId = atxBMPId.value
  
    grdSrc.ColTitle(grdSrc.col) = "% BMP " & curBMPId
    myBmp(grdSrc.col - 2).id = curBMPId
  
    cmdUpU.Enabled = True
  End If
  
End Sub

Private Sub cmbReach_Click()
  If cmbReach.ListIndex = 0 Then
    rdoOpt(0).Visible = True
    rdoOpt(1).Visible = True
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
        If myBmp(k).id = MyRch(cur).id + j Then 'in use
          j = j + 1
          done = False
          Exit For
        End If
      Next k
    Loop
    
    myBmp(i).id = MyRch(cur).id + j
    myBmp(i).desc = "New BMP"
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
      MsgBox "No BMPs are associated with Reach " & MyRch(cur).id, _
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
            curbmp = myBmp(j).id
            nth = j
          End If
        End If
      Next j
      ans = MsgBox("Are you sure you want to Delete BMP " & curbmp & "?", _
             vbYesNo, msgTitle)
      If ans = vbYes Then
        myBmp(nth).DeletePending = True
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
    frmGenActModBMPEffic.Show 1
End Sub

Private Sub cmdUpU_Click()
  Dim i&, j&, k&, ret&, a!, rchId&, c&, bmpId&, itmnam$
  
  rchId = MyRch(cmbReach.ListIndex).id
  
  atxBMPId.Enabled = False
  For i = 1 To UBound(myBmp) 'process deletes first
    If myBmp(i).DeletePending Then 'get rid of it
      For j = 1 To UBound(myTrib) 'first connections in schematic
        a = 0
        Call F90_PBMPAR(myTrib(j).OpNam, myTrib(j).id, a, "BMPRAC", myBmp(i).id, ret, 6, 6)
        If ret <> 0 Then
          MsgBox "Problem 1 Updating UCI " & ret, vbOKOnly, msgTitle
          Exit Sub
        End If
      Next j
      Call F90_DELBMP(myBmp(i).id)
      myBmp(i).InUseNow = False
    End If
  Next i
  
  For i = 1 To UBound(myBmp)
    If myBmp(i).InUseNow Then
      a = 1#
      Call F90_PBMPAR("BMPRAC", myBmp(i).id, a, "RCHRES", rchId, ret, 6, 6)
      If ret > 0 Then
        MsgBox "Problem 2 Updating UCI " & ret, vbOKOnly, msgTitle
        Exit Sub
      ElseIf ret = -1 Then
        'added a bmprac, need to add associated tables
        Call F90_ADDBMP(p.HSPFMsg.Unit, myBmp(i).id)
      End If
      'put description to uci
      itmnam = "BMPDESC "
      Call F90_HPUTC(itmnam, myBmp(i).id, myBmp(i).desc, Len(itmnam), Len(myBmp(i).desc))
    End If
  Next i
  
  For j = 1 To UBound(myTrib)
    'going directly to rch
    a = myTrib(j).Area * CSng((grdSrc.TextMatrix(j, 2) / 100))
    Call F90_PBMPAR(myTrib(j).OpNam, myTrib(j).id, a, "RCHRES", rchId, ret, 6, 6)
    For k = 1 To myTrib(j).BMPCnt
      If myBmp(myTrib(j).BmpPtr(k).Ind).InUseNow Then
        bmpId = myBmp(myTrib(j).BmpPtr(k).Ind).id
        c = myBmp(myTrib(j).BmpPtr(k).Ind).col
        a = myTrib(j).Area * CSng((grdSrc.TextMatrix(j, c) / 100))
        Call F90_PBMPAR(myTrib(j).OpNam, myTrib(j).id, a, "BMPRAC", bmpId, ret, 6, 6)
        If ret <> 0 Then
          MsgBox "Problem 3 Updating UCI " & ret, vbOKOnly, msgTitle
          Exit Sub
        End If
      End If
    Next k
  Next j
   
  Call GetUciInfo(cmbReach.ListIndex) 'refresh with new data
  Call PopulateGrid
  cmdUpU.Enabled = False
    
End Sub

Private Sub Form_Load()
  Dim iexist As Boolean, itmnam$, opid&, cnt&, i&
  
  Me.Icon = frmGenScnActivate.Icon
  msgTitle = "GenScn Activate Modify BMP"
  'are any reaches available?
  iexist = frmGenScnActivate.OperationExists("RCHRES")
  If iexist Then
    'what reaches are available?
    cmbReach.clear
    cmbReach.AddItem "Summary"
    ReDim MyRch(0)
    cnt = 0
    'get ids
    itmnam = "RCHID   "
    Do
      Call F90_HGETI(itmnam, CLng(0), opid, Len(itmnam))
      If opid = -99 Then
        Exit Do
      Else
        cnt = cnt + 1
        ReDim Preserve MyRch(cnt)
        MyRch(cnt).id = opid
      End If
    Loop
    'get reach names
    itmnam = "RCHDESC "
    For i = 1 To cnt
      Call F90_HGETC(itmnam, MyRch(i).id, MyRch(i).desc, Len(itmnam))
      If InStr(MyRch(i).desc, ":") Then 'assume id already present by convention
        cmbReach.AddItem MyRch(i).desc
      Else
        cmbReach.AddItem MyRch(i).id & ":" & MyRch(i).desc
      End If
    Next i
    
    'get all bmp info
    ReDim myBmp(0)
    'get bmp ids
    cnt = 0
    itmnam = "BMPID   "
    Do
      Call F90_HGETI(itmnam, CLng(0), opid, Len(itmnam))
      If opid = -99 Then
        Exit Do
      Else
        cnt = cnt + 1
        ReDim Preserve myBmp(cnt)
        myBmp(cnt).id = opid
      End If
    Loop
    'get names
    itmnam = "BMPDESC "
    For i = 1 To cnt
      Call F90_HGETC(itmnam, myBmp(i).id, myBmp(i).desc, Len(itmnam))
      myBmp(i).DeletePending = False
    Next i
    
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
  Dim t!, i&, r&, c&, a&, cnow&
  
  c = ChangeToCol
  
  For r = ChangeFromRow To ChangeToRow
    t = grdSrc.TextMatrix(r, c) 'highest priority is just set value
    
    cnow = 2
    For i = 1 To UBound(myBmp) ' each bmp
      If myBmp(i).InUseNow And Not (myBmp(i).DeletePending) Then
        cnow = cnow + 1
        If cnow <> c Then 'dont double count current
          t = t + grdSrc.TextMatrix(r, cnow)
          If t > 100 Then
            grdSrc.TextMatrix(r, cnow) = grdSrc.TextMatrix(r, cnow) - t + 100
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
       myBmp(i).desc = txtBMPDesc.text
    End If
  Next i
  cmdUpU.Enabled = True
End Sub
