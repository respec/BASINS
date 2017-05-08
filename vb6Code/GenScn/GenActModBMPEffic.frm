VERSION 5.00
Begin VB.Form frmGenActModBMPEffic 
   Caption         =   "GenScn Activate Modify BMP Efficiency"
   ClientHeight    =   7440
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   7920
   HelpContextID   =   56
   LinkTopic       =   "Form1"
   ScaleHeight     =   7440
   ScaleWidth      =   7920
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox lblReference 
      Appearance      =   0  'Flat
      BackColor       =   &H80000016&
      BorderStyle     =   0  'None
      Height          =   975
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   6
      Text            =   "GenActModBMPEffic.frx":0000
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
Attribute VB_Name = "frmGenActModBMPEffic"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
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

Private Sub RefreshGrid()
  Dim i&
  
  grdBMPEfc.Cols = 2
  grdBMPEfc.ColTitle(0) = "Constituent"
  grdBMPEfc.ColEditable(0) = False
  grdBMPEfc.ColTitle(1) = "Fraction"
  grdBMPEfc.ColEditable(1) = True
  If cmbBMPName.ListIndex <= 0 Then
    cmbBMPName.ListIndex = 0
  Else
    grdBMPEfc.Cols = 4
    grdBMPEfc.ColTitle(2) = "DB Range"
    grdBMPEfc.ColEditable(2) = False
    grdBMPEfc.ColTitle(3) = "Reference"
    grdBMPEfc.ColEditable(3) = False
    grdBMPEfc.colWidth(3) = 0
  End If
  
  grdBMPEfc.Rows = UBound(BMPParms)
  For i = 1 To UBound(BMPParms)
    grdBMPEfc.TextMatrix(i, 0) = BMPParms(i).Name
  Next i
  
  Call RefreshCurrentFromUCI
  If cmbBMPName.ListIndex > 0 Then Call RefreshRangeFromDB
  
  grdBMPEfc.row = 1
  grdBMPEfc.Col = 1
  
End Sub

Private Sub RefreshCurrentFromUCI()
  Dim omcode&, init&, uunits&, addfg&, occur&
  Dim cbuff$, retkey&, retcod&, lstkey&
  Dim i&, r#, l&
  
  omcode = 130
  uunits = 1
  addfg = 1
  occur = 1
  For i = 1 To UBound(BMPParms)
    init = 1
    Do
      Call F90_XTABLE(omcode, BMPParms(i).TableNumber, uunits, init, addfg, occur, _
                      retkey, cbuff, retcod)
      init = 0
      If retcod = 2 Then
        lstkey = retkey
        If Left(cbuff, 5) = OpnID Then
          If Len(cbuff) > BMPParms(i).StartColumn + BMPParms(i).Length Then
            l = BMPParms(i).Length
          ElseIf Len(cbuff) >= BMPParms(i).StartColumn Then
            l = Len(cbuff) - BMPParms(i).StartColumn + 1
          Else
            l = 0
          End If
          
          If l > 0 Then
            r = Mid(cbuff, BMPParms(i).StartColumn, l)
          Else
            r = 0
          End If
          Exit Do
        End If
      ElseIf retcod = 10 Then 'nothing more to return
        r = 0
        Exit Do
      End If
    Loop
    grdBMPEfc.TextMatrix(i, 1) = r
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
  Call RefreshGrid
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
  Dim omcode&, init&, uunits&, addfg&, occur&, j&
  Dim cbuff$, retkey&, retcod&, lstkey&, cr$, ctemp$, cr2$
  Dim i&, r#, l&
  
  omcode = 130
  uunits = 1
  addfg = 1
  occur = 1
  For i = 1 To UBound(BMPParms)
    init = 1
    Do
      Call F90_XTABLE(omcode, BMPParms(i).TableNumber, uunits, init, addfg, occur, _
                      retkey, cbuff, retcod)
      init = 0
      If retcod = 2 Then
        lstkey = retkey
        If Left(cbuff, 5) = OpnID Then
          If Len(cbuff) > BMPParms(i).StartColumn + BMPParms(i).Length Then
            l = BMPParms(i).Length
          ElseIf Len(cbuff) >= BMPParms(i).StartColumn Then
            l = Len(cbuff) - BMPParms(i).StartColumn + 1
          Else
            l = 0
          End If
          
          If l > 0 Then
            'r = Mid(cbuff, BMPParms(i).StartColumn, l)
            r = grdBMPEfc.TextMatrix(i, 1)
            cr = CStr(r)
            If Len(cr) > l Then
              cr = Mid(cr, 1, l)
            ElseIf Len(cr) < l Then
              'add spaces to right justify in string
              cr2 = ""
              For j = 1 To l - Len(cr)
                cr2 = cr2 & " "
              Next j
              cr = cr2 & cr
            End If
            ctemp = Left(cbuff, BMPParms(i).StartColumn - 1) & cr & Mid(cbuff, BMPParms(i).StartColumn + l)
            'replace this record in uci file
            Call F90_REPUCI(retkey, ctemp, Len(ctemp))
          End If
          Exit Do
        End If
      ElseIf retcod = 10 Then 'nothing more to return
        Exit Do
      End If
    Loop
  Next i
  
  cmdUpU.Enabled = False
End Sub

Private Sub Form_Load()
  Dim txtBMPDesc$, i&

  Me.Icon = frmGenScnActivate.Icon
  msgTitle = "GenScn Activate Modify BMP Efficiency"

  Set myBmpDB = OpenDatabase(ExePath & "bin\bmp.mdb", False, True)
  Set myRec = myBmpDB.OpenRecordset("Practice", dbOpenDynaset, dbReadOnly, dbReadOnly)
  Set myRef = myBmpDB.OpenRecordset("Reference", dbOpenDynaset, dbReadOnly, dbReadOnly)
  
  txtBMPDesc = UCase(frmGenActModBMP.txtBMPDesc)
  OpnID = frmGenActModBMP.atxBMPId.value
  lblID = "BMP Operation # " & OpnID
  cmbBMPName.Clear
  cmbBMPName.AddItem ("<unknown>")
  Do Until myRec.EOF
    cmbBMPName.AddItem myRec("Name")
    If InStr(txtBMPDesc, UCase(cmbBMPName.List(cmbBMPName.ListCount - 1))) Then
      cmbBMPName.ListIndex = cmbBMPName.ListCount - 1
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
