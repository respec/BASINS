VERSION 5.00
Begin VB.Form frmGenPLTInit 
   Caption         =   "PLTGEN Data Initialization"
   ClientHeight    =   4284
   ClientLeft      =   3060
   ClientTop       =   2376
   ClientWidth     =   6636
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "GenPLTIn.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   4284
   ScaleWidth      =   6636
   Tag             =   "0"
   Begin ATCoCtl.ATCoGrid acgPltgen 
      Height          =   2895
      Left            =   240
      TabIndex        =   0
      Top             =   600
      Width           =   6135
      _ExtentX        =   10816
      _ExtentY        =   5101
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
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
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   372
      Index           =   2
      Left            =   4200
      TabIndex        =   4
      Top             =   3720
      Width           =   1095
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "&Update"
      Height          =   372
      Index           =   1
      Left            =   2760
      TabIndex        =   3
      Top             =   3720
      Width           =   1095
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "&OK"
      Height          =   372
      Index           =   0
      Left            =   1320
      TabIndex        =   1
      Top             =   3720
      Width           =   1095
   End
   Begin VB.Label lblColHdr 
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   6372
   End
End
Attribute VB_Name = "frmGenPLTInit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Dim fname$
    

Private Sub acgPltgen_RowColChange()
  With acgPltgen
    If .col = 0 Then
      .clearvalues
      .addValue ("True")
      .addValue ("False")
    Else
      .clearvalues
      .ComboCheckValidValues = False
    End If
  End With
End Sub

Private Sub cmdClose_Click(index As Integer)

    If index = 0 Then 'OK
      frmGenPLTInit.Tag = 1
      frmGenPLTInit.Hide
    ElseIf index = 1 Then 'update
      frmGenPLTInit.Tag = 0
      MousePointer = vbHourglass
      Call UpdatePLTFile
      MousePointer = vbDefault
    ElseIf index = 2 Then 'cancel
      frmGenPLTInit.Tag = -1
      frmGenPLTInit.Hide
    End If

End Sub
Public Sub UpdatePLTFile()
    Dim ifl&, istr$, i&
    Dim txtbuf$(), txtcnt&, keybuf&
    
    ifl = FreeFile(0)
    Open fname For Input As #ifl
    
    Line Input #ifl, istr
    txtcnt = 0
    ReDim Preserve txtbuf(txtcnt + 1)
    txtbuf(txtcnt) = istr
    For i = 1 To 10
      Line Input #ifl, istr
      txtcnt = txtcnt + 1
      ReDim Preserve txtbuf(txtcnt + 1)
      txtbuf(txtcnt) = istr
    Next i
    
    For i = 1 To acgPltgen.rows
      Line Input #ifl, istr
      txtcnt = txtcnt + 1
      ReDim Preserve txtbuf(txtcnt + 1)
      txtbuf(txtcnt) = istr
      
      'now add scenario and location names
      txtbuf(txtcnt) = Left(txtbuf(txtcnt), 80) & "                         "
      Mid(txtbuf(txtcnt), 81, 8) = acgPltgen.TextMatrix(i, 1)
      Mid(txtbuf(txtcnt), 91, 8) = acgPltgen.TextMatrix(i, 2)
      Mid(txtbuf(txtcnt), 6, 8) = "        "
      Mid(txtbuf(txtcnt), 6, 8) = acgPltgen.TextMatrix(i, 3)
    Next i
    
    While Not EOF(ifl)
      Line Input #ifl, istr
      txtcnt = txtcnt + 1
      ReDim Preserve txtbuf(txtcnt + 1)
      txtbuf(txtcnt) = istr
    Wend
    Close ifl
    
    'now write out revised file
    Open fname For Output As ifl
    For i = 1 To UBound(txtbuf)
      Print #ifl, txtbuf(i - 1)
    Next i
    Close ifl
End Sub
Public Sub SetPltgenName(tmpname$)
    fname = tmpname
End Sub

Private Sub Form_Resize()
  acgPltgen.Height = frmGenPLTInit.Height - 2000
  cmdClose(0).Top = frmGenPLTInit.Height - 1000
  cmdClose(1).Top = frmGenPLTInit.Height - 1000
  cmdClose(2).Top = frmGenPLTInit.Height - 1000
  acgPltgen.Width = frmGenPLTInit.Width - 500
  cmdClose(1).Left = (frmGenPLTInit.Width / 2) - (cmdClose(1).Width / 2)
  cmdClose(0).Left = cmdClose(1).Left - cmdClose(0).Width - 500
  cmdClose(2).Left = cmdClose(1).Left + cmdClose(2).Width + 500
  lblColHdr.Width = acgPltgen.Width
End Sub
