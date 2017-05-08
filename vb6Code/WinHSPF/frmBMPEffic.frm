VERSION 5.00
Begin VB.Form frmBMPEffic 
   Caption         =   "WinHSPF - Best Management Practices Efficiency Editor"
   ClientHeight    =   7440
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   7920
   HelpContextID   =   53
   LinkTopic       =   "Form1"
   ScaleHeight     =   7440
   ScaleWidth      =   7920
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox lblReference 
      Appearance      =   0  'Flat
      BackColor       =   &H80000016&
      BorderStyle     =   0  'None
      Height          =   975
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   6
      Text            =   "frmBMPEffic.frx":0000
      Top             =   600
      Width           =   7695
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
      Height          =   375
      Left            =   1440
      TabIndex        =   4
      Top             =   6960
      Width           =   1215
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
      Left            =   120
      TabIndex        =   3
      Top             =   6960
      Width           =   1215
   End
   Begin ATCoCtl.ATCoGrid grdBMPEfc 
      Height          =   4935
      Left            =   120
      TabIndex        =   2
      Top             =   1800
      Width           =   7695
      _ExtentX        =   13568
      _ExtentY        =   8700
      SelectionToggle =   0   'False
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
      Header          =   "Removal Fractions"
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
   Begin VB.ComboBox cmbBMPName 
      Height          =   315
      Left            =   1200
      TabIndex        =   0
      Text            =   "<unknown>"
      Top             =   120
      Width           =   3495
   End
   Begin VB.Label lblID 
      Caption         =   "BMP Operation # "
      Height          =   375
      Left            =   4800
      TabIndex        =   5
      Top             =   120
      Width           =   3015
   End
   Begin VB.Label lblBMPName 
      Caption         =   "BMP Name:"
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   975
   End
End
Attribute VB_Name = "frmBMPEffic"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private msgTitle$
Private myBmpDB As Database
Private myRec As Recordset
Private myRef As Recordset
Private ans&
Private OpnID&
Private Type HSPFParms
    Name As String
    DBName As String
    TableName As String
    TableNumber As Long
    StartColumn As Long
    Length As Long
    Key As Long
End Type
Dim BMPParms() As HSPFParms
Private loading As Boolean

Private Sub RefreshGrid()
  Dim i&
  
  grdBMPEfc.cols = 2
  grdBMPEfc.ColTitle(0) = "Constituent"
  grdBMPEfc.ColEditable(0) = False
  grdBMPEfc.ColTitle(1) = "Fraction"
  grdBMPEfc.ColEditable(1) = True
  grdBMPEfc.ColType(1) = ATCoSng
  grdBMPEfc.ColAlignment(1) = 1
  grdBMPEfc.ColMax(1) = 1#
  grdBMPEfc.ColMin(1) = 0#
  If cmbBMPName.ListIndex <= 0 Then
    cmbBMPName.ListIndex = 0
  Else
    grdBMPEfc.cols = 4
    grdBMPEfc.ColTitle(2) = "DB Range"
    grdBMPEfc.ColEditable(2) = False
    grdBMPEfc.ColTitle(3) = "Reference"
    grdBMPEfc.ColEditable(3) = False
    grdBMPEfc.colWidth(3) = 0
  End If
  
  grdBMPEfc.rows = UBound(BMPParms)
  For i = 1 To UBound(BMPParms)
    grdBMPEfc.TextMatrix(i, 0) = BMPParms(i).Name
  Next i
  
  Call RefreshCurrentFromUCI
  If cmbBMPName.ListIndex > 0 Then Call RefreshRangeFromDB
  
  grdBMPEfc.row = 1
  grdBMPEfc.col = 1
  
End Sub

Private Sub RefreshCurrentFromUCI()
  Dim lOpnBlk As HspfOpnBlk
  Dim lTable As HspfTable
  Dim loper As HspfOperation
  Dim i&, parmindex&
  Dim lParm As HSPFParm
  
  Set lOpnBlk = myUci.OpnBlks("BMPRAC")
  Set loper = lOpnBlk.operfromid(OpnID)
  
  For i = 1 To UBound(BMPParms)
    If Not loper Is Nothing Then
      Set lTable = loper.tables(BMPParms(i).TableName)
      If Not lTable Is Nothing Then
        If lTable.Name = "GQ-FRAC" Or lTable.Name = "CONS-FRAC" Then
          parmindex = Int(BMPParms(i).StartColumn / 10) - 1
        Else
          parmindex = Int(BMPParms(i).StartColumn / 10)
        End If
        Set lParm = lTable.Parms(parmindex)
        If Not lParm Is Nothing Then
          grdBMPEfc.TextMatrix(i, 1) = lParm.Value
        Else
          grdBMPEfc.TextMatrix(i, 1) = 0#
        End If
      Else
        grdBMPEfc.TextMatrix(i, 1) = 0#
      End If
    Else
      grdBMPEfc.TextMatrix(i, 1) = 0#
    End If
  Next i
End Sub
Private Sub RefreshRangeFromDB()
  Dim i&, s$, q$, r$
   
  Set myRec = myBmpDB.OpenRecordset("Ranges", dbOpenDynaset, dbReadOnly, dbReadOnly)

  For i = 1 To UBound(BMPParms)
    s = "<not available>"
    r = ""
    If Len(BMPParms(i).DBName) > 0 Then
      q = "ConstituentID = " & BMPParms(i).DBName & " AND PracticeID = " & cmbBMPName.ListIndex
      myRec.FindFirst (q)
      If Not (myRec.NoMatch) Then
        s = myRec("Range")
        r = myRec("ReferenceID")
      End If
    End If
    grdBMPEfc.TextMatrix(i, 2) = s
    grdBMPEfc.TextMatrix(i, 3) = r
  Next i
  
  myRec.Close
  
End Sub

Private Sub cmbBMPName_Click()
  If Not loading Then
    Call RefreshGrid
  End If
End Sub

Private Sub cmdClose_Click()
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

End Sub

Private Sub cmdUpU_Click()
  Dim lOpnBlk As HspfOpnBlk
  Dim lTable As HspfTable
  Dim loper As HspfOperation
  Dim i&, parmindex&
  Dim lParm As HSPFParm
  
  Set lOpnBlk = myUci.OpnBlks("BMPRAC")
  Set loper = lOpnBlk.operfromid(OpnID)
  
  For i = 1 To UBound(BMPParms)
    If Not loper Is Nothing Then
      Set lTable = loper.tables(BMPParms(i).TableName)
      If Not lTable Is Nothing Then
        If lTable.Name = "GQ-FRAC" Or lTable.Name = "CONS-FRAC" Then
          parmindex = Int(BMPParms(i).StartColumn / 10) - 1
        Else
          parmindex = Int(BMPParms(i).StartColumn / 10)
        End If
        Set lParm = lTable.Parms(parmindex)
        If Not lParm Is Nothing Then
          lParm.Value = grdBMPEfc.TextMatrix(i, 1)
        End If
      End If
    End If
  Next i

  cmdUpU.Enabled = False
End Sub

Private Sub Form_Load()
  Dim txtBMPDesc$, i&

  loading = True
  Me.Icon = frmBMP.Icon
  msgTitle = "Best Management Practices Efficiency Editor"

  Set myBmpDB = OpenDatabase(PathNameOnly(HSPFMain.W_EXEWINHSPF) & "\bmp.mdb", False, True)
  Set myRec = myBmpDB.OpenRecordset("Practice", dbOpenDynaset, dbReadOnly, dbReadOnly)
  Set myRef = myBmpDB.OpenRecordset("Reference", dbOpenDynaset, dbReadOnly, dbReadOnly)
  
  txtBMPDesc = UCase(frmBMP.txtBMPDesc)
  OpnID = frmBMP.atxBMPId.Value
  lblID = "BMP Operation # " & OpnID
  cmbBMPName.Clear
  cmbBMPName.AddItem ("<unknown>")
  Do Until myRec.EOF
    cmbBMPName.AddItem myRec("Name")
    If InStr(txtBMPDesc, UCase(cmbBMPName.List(cmbBMPName.listcount - 1))) Then
      cmbBMPName.ListIndex = cmbBMPName.listcount - 1
    End If
    myRec.MoveNext
  Loop
  myRec.Close
  
  Set myRec = myBmpDB.OpenRecordset("HSPFParms", dbOpenDynaset, dbReadOnly, dbReadOnly)
  myRec.MoveLast 'force recalc of record count
  i = myRec.RecordCount
  myRec.MoveFirst
  ReDim BMPParms(i)
  i = 1
  Do Until myRec.EOF
    BMPParms(i).Name = myRec("Name")
    If Not (IsNull(myRec("DBName"))) Then
      BMPParms(i).DBName = myRec("DBName")
    End If
    BMPParms(i).TableName = myRec("HSPFTable")
    BMPParms(i).TableNumber = myRec("TableNumber")
    BMPParms(i).StartColumn = myRec("StartColumn")
    BMPParms(i).Length = myRec("Length")
    i = i + 1
    myRec.MoveNext
  Loop
  loading = False
  
  Call RefreshGrid
End Sub

Private Sub grdBMPEfc_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  cmdUpU.Enabled = True
End Sub

Private Sub grdBMPEfc_RowColChange()
  Dim s$, v$
  
  v = grdBMPEfc.TextMatrix(grdBMPEfc.row, 3)
  If Len(v) > 0 Then
    myRef.FindFirst "ID = " & v
    s = "Reference: " & myRef("Detail")
  Else
    s = "Reference: <not applicable>"
  End If
  lblReference.Text = s
End Sub
