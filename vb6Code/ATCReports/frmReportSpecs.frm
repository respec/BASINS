VERSION 5.00
Begin VB.Form frmReportSpecs 
   Caption         =   "Report Specifications"
   ClientHeight    =   6252
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   7260
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   LinkTopic       =   "Form1"
   ScaleHeight     =   6252
   ScaleWidth      =   7260
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog cdlReportFile 
      Left            =   5520
      Top             =   5760
      _ExtentX        =   677
      _ExtentY        =   677
      _Version        =   393216
      CancelError     =   -1  'True
      DialogTitle     =   "Save Report to File..."
   End
   Begin VB.Frame fraGeneral 
      Caption         =   "General Specfications"
      Height          =   1692
      Left            =   120
      TabIndex        =   24
      Top             =   120
      Width           =   6972
      Begin VB.OptionButton optFormat 
         Caption         =   "Table"
         Enabled         =   0   'False
         Height          =   252
         Index           =   0
         Left            =   1200
         TabIndex        =   27
         Top             =   360
         Width           =   972
      End
      Begin VB.OptionButton optFormat 
         Caption         =   "Database Records"
         Enabled         =   0   'False
         Height          =   252
         Index           =   1
         Left            =   2160
         TabIndex        =   26
         Top             =   360
         Width           =   2412
      End
      Begin VB.TextBox txtTitle 
         Height          =   288
         Left            =   840
         TabIndex        =   25
         Top             =   840
         Width           =   5892
      End
      Begin ATCoCtl.ATCoText atxSigDig 
         Height          =   252
         Left            =   1800
         TabIndex        =   30
         Top             =   1320
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   10
         HardMin         =   1
         SoftMax         =   8
         SoftMin         =   1
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   5
         Value           =   "5"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxDecPla 
         Height          =   252
         Left            =   4080
         TabIndex        =   32
         Top             =   1320
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   10
         HardMin         =   1
         SoftMax         =   8
         SoftMin         =   1
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   2
         Value           =   "2"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxColWid 
         Height          =   252
         Left            =   6120
         TabIndex        =   35
         Top             =   1320
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   20
         HardMin         =   1
         SoftMax         =   20
         SoftMin         =   1
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   10
         Value           =   "10"
         Enabled         =   -1  'True
      End
      Begin VB.Label lblColWid 
         Caption         =   "Column Width:"
         Height          =   252
         Left            =   4920
         TabIndex        =   34
         Top             =   1320
         Width           =   1332
      End
      Begin VB.Label lblFormat 
         Caption         =   "Format:"
         Enabled         =   0   'False
         Height          =   252
         Left            =   240
         TabIndex        =   33
         Top             =   360
         Width           =   852
      End
      Begin VB.Label lblDecPla 
         Caption         =   "Decimal Places:"
         Height          =   252
         Left            =   2640
         TabIndex        =   31
         Top             =   1320
         Width           =   1572
      End
      Begin VB.Label lblSigDig 
         Caption         =   "Significant Digits:"
         Height          =   252
         Left            =   240
         TabIndex        =   29
         Top             =   1320
         Width           =   1572
      End
      Begin VB.Label lblTitle 
         Caption         =   "Title:"
         Height          =   252
         Left            =   240
         TabIndex        =   28
         Top             =   840
         Width           =   492
      End
   End
   Begin VB.Frame fraLayout 
      Caption         =   "Layout Specifications"
      Height          =   3612
      Left            =   120
      TabIndex        =   2
      Top             =   1920
      Width           =   6972
      Begin VB.CheckBox chkRowSumm 
         Caption         =   "Count"
         Height          =   252
         Index           =   4
         Left            =   5040
         TabIndex        =   38
         Tag             =   "Cnt"
         Top             =   3120
         Width           =   1572
      End
      Begin VB.CheckBox chkColSumm 
         Caption         =   "Count"
         Height          =   252
         Index           =   4
         Left            =   2760
         TabIndex        =   37
         Tag             =   "Cnt"
         Top             =   3120
         Width           =   1572
      End
      Begin VB.CheckBox chkSectionSumm 
         Caption         =   "Count"
         Height          =   252
         Index           =   4
         Left            =   360
         TabIndex        =   36
         Tag             =   "Cnt"
         Top             =   3120
         Width           =   1572
      End
      Begin VB.CheckBox chkRowSumm 
         Caption         =   "Maximum"
         Height          =   252
         Index           =   3
         Left            =   5040
         TabIndex        =   23
         Top             =   2760
         Width           =   1572
      End
      Begin VB.ComboBox cboAtt 
         Height          =   288
         Index           =   0
         ItemData        =   "frmReportSpecs.frx":0000
         Left            =   360
         List            =   "frmReportSpecs.frx":0010
         Style           =   2  'Dropdown List
         TabIndex        =   16
         Top             =   720
         Width           =   1572
      End
      Begin VB.ComboBox cboAtt 
         Height          =   288
         Index           =   1
         ItemData        =   "frmReportSpecs.frx":003B
         Left            =   2760
         List            =   "frmReportSpecs.frx":004B
         Style           =   2  'Dropdown List
         TabIndex        =   15
         Top             =   720
         Width           =   1572
      End
      Begin VB.ComboBox cboAtt 
         Height          =   288
         Index           =   2
         ItemData        =   "frmReportSpecs.frx":0076
         Left            =   5040
         List            =   "frmReportSpecs.frx":0086
         Style           =   2  'Dropdown List
         TabIndex        =   14
         Top             =   720
         Width           =   1572
      End
      Begin VB.CheckBox chkSectionSumm 
         Caption         =   "Sum"
         Height          =   252
         Index           =   0
         Left            =   360
         TabIndex        =   13
         Tag             =   "Sum"
         Top             =   1680
         Width           =   1572
      End
      Begin VB.CheckBox chkSectionSumm 
         Caption         =   "Average"
         Height          =   252
         Index           =   1
         Left            =   360
         TabIndex        =   12
         Tag             =   "Ave"
         Top             =   2040
         Width           =   1572
      End
      Begin VB.CheckBox chkSectionSumm 
         Caption         =   "Minimum"
         Height          =   252
         Index           =   2
         Left            =   360
         TabIndex        =   11
         Top             =   2400
         Width           =   1572
      End
      Begin VB.CheckBox chkSectionSumm 
         Caption         =   "Maximum"
         Height          =   252
         Index           =   3
         Left            =   360
         TabIndex        =   10
         Top             =   2760
         Width           =   1572
      End
      Begin VB.CheckBox chkColSumm 
         Caption         =   "Sum"
         Height          =   252
         Index           =   0
         Left            =   2760
         TabIndex        =   9
         Tag             =   "Sum"
         Top             =   1680
         Width           =   1572
      End
      Begin VB.CheckBox chkColSumm 
         Caption         =   "Average"
         Height          =   252
         Index           =   1
         Left            =   2760
         TabIndex        =   8
         Tag             =   "Ave"
         Top             =   2040
         Width           =   1572
      End
      Begin VB.CheckBox chkColSumm 
         Caption         =   "Minimum"
         Height          =   252
         Index           =   2
         Left            =   2760
         TabIndex        =   7
         Top             =   2400
         Width           =   1572
      End
      Begin VB.CheckBox chkColSumm 
         Caption         =   "Maximum"
         Height          =   252
         Index           =   3
         Left            =   2760
         TabIndex        =   6
         Top             =   2760
         Width           =   1572
      End
      Begin VB.CheckBox chkRowSumm 
         Caption         =   "Sum"
         Height          =   252
         Index           =   0
         Left            =   5040
         TabIndex        =   5
         Tag             =   "Sum"
         Top             =   1680
         Width           =   1572
      End
      Begin VB.CheckBox chkRowSumm 
         Caption         =   "Average"
         Height          =   252
         Index           =   1
         Left            =   5040
         TabIndex        =   4
         Tag             =   "Ave"
         Top             =   2040
         Width           =   1572
      End
      Begin VB.CheckBox chkRowSumm 
         Caption         =   "Minimum"
         Height          =   252
         Index           =   2
         Left            =   5040
         TabIndex        =   3
         Top             =   2400
         Width           =   1572
      End
      Begin VB.Label lblAttCnt 
         Height          =   252
         Index           =   2
         Left            =   5040
         TabIndex        =   41
         Top             =   1080
         Width           =   1812
      End
      Begin VB.Label lblAttCnt 
         Height          =   252
         Index           =   1
         Left            =   2760
         TabIndex        =   40
         Top             =   1080
         Width           =   2172
      End
      Begin VB.Label lblAttCnt 
         Height          =   252
         Index           =   0
         Left            =   360
         TabIndex        =   39
         Top             =   1080
         Width           =   2292
      End
      Begin VB.Label lblAtt 
         Caption         =   "Section"
         Height          =   252
         Index           =   0
         Left            =   240
         TabIndex        =   22
         Top             =   360
         Width           =   1572
      End
      Begin VB.Label lblAtt 
         Caption         =   "Column"
         Height          =   252
         Index           =   1
         Left            =   2640
         TabIndex        =   21
         Top             =   360
         Width           =   1572
      End
      Begin VB.Label lblAtt 
         Caption         =   "Row"
         Height          =   252
         Index           =   2
         Left            =   4920
         TabIndex        =   20
         Top             =   360
         Width           =   1572
      End
      Begin VB.Label lblSectionSumm 
         Caption         =   "Summaries"
         Height          =   252
         Left            =   240
         TabIndex        =   19
         Top             =   1320
         Width           =   1572
      End
      Begin VB.Label lblColSumm 
         Caption         =   "Summaries"
         Height          =   252
         Left            =   2640
         TabIndex        =   18
         Top             =   1320
         Width           =   1572
      End
      Begin VB.Label lblRowSumm 
         Caption         =   "Summaries"
         Height          =   252
         Left            =   4920
         TabIndex        =   17
         Top             =   1320
         Width           =   1572
      End
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "Cancel"
      Height          =   372
      Index           =   1
      Left            =   3600
      TabIndex        =   1
      Top             =   5760
      Width           =   852
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "OK"
      Height          =   372
      Index           =   0
      Left            =   2400
      TabIndex        =   0
      Top             =   5760
      Width           =   852
   End
End
Attribute VB_Name = "frmReportSpecs"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private pReport As clsATCreport

Private pFName As String
Private pTSer As Collection 'of tserdata
Private Summs(4, 2) As Boolean
Private AttHdrs(2, 1) As Variant

Public Property Get Report() As clsATCreport
  Set Report = pReport
End Property
Public Property Set Report(newValue As clsATCreport)
  Set pReport = newValue
End Property

Public Function EditReportSpecs() As Boolean

  frmReportSpecs.Show vbModal
  While frmReportSpecs.Visible
    DoEvents
  Wend
  EditReportSpecs = frmReportSpecs.Tag
End Function

Private Sub cboAtt_Click(index As Integer)
  Dim Hdrs As CollString

  If pTSer.Count > 0 Then
    If cboAtt(index).Text = "Time" Then
      lblAttCnt(index).Caption = "(" & pTSer(1).dates.Summary.nVals & " occurences)"
    Else
      Set Hdrs = Nothing
      Set Hdrs = New CollString
      Set Hdrs = uniqueAttributeValues(cboAtt(index).Text, pTSer)
      lblAttCnt(index).Caption = "(" & Hdrs.Count & " occurences)"
    End If
  End If
End Sub

Private Sub chkColSumm_Click(index As Integer)
  Summs(index, 1) = chkColSumm(index).Value
End Sub

Private Sub chkSectionSumm_Click(index As Integer)
  Summs(index, 0) = chkSectionSumm(index).Value
End Sub

Private Sub chkRowSumm_Click(index As Integer)
  Summs(index, 2) = chkRowSumm(index).Value
End Sub

Private Sub cmdClose_Click(index As Integer)
  Dim i%, OKToClose As Long

  OKToClose = 1
  For i = 0 To cboAtt.Count - 1
    If cboAtt(i).Text = "Time" Then OKToClose = 0
  Next i
  If Len(cboAtt(0).Text) > 0 And Len(cboAtt(1).Text) > 0 And Len(cboAtt(2).Text) > 0 Then
    If cboAtt(0).Text = cboAtt(1).Text Or _
       cboAtt(0).Text = cboAtt(2).Text Or _
       cboAtt(1).Text = cboAtt(2).Text Then
      OKToClose = 2
    End If
  Else
    OKToClose = 2
  End If
  
  If OKToClose = 0 Then
    If index = 0 Then
      On Error GoTo FileCancel
      Me.Tag = vbTrue
      cdlReportFile.Filename = pFName
      cdlReportFile.ShowOpen
      pFName = cdlReportFile.Filename
    Else
      Me.Tag = vbFalse
    End If
    
    SetReportFromForm
    
    Me.Visible = False
  ElseIf OKToClose = 1 Then
    MsgBox "Time must be selected as the Section, Column, or Row variable for a valid report.", vbExclamation, "Report Specifications Problem"
  Else
    MsgBox "It is necessary to have unique Section, Column, and Report attributes to generate a report.", , "Report Specifications Problem"
  End If
  Exit Sub

FileCancel:
  Me.Tag = vbFalse
  Me.Visible = False

End Sub

Private Sub SetFormFromReport()

End Sub

Private Sub SetReportFromForm()
  Dim thisColumn As clsATCreportSection
  With pReport
    'FormFg = frmReportSpecs.FormFg
    .Title = txtTitle.Text
    If .Cols.Count = 0 Then
      Set thisColumn = New clsATCreportSection
      .Cols.Add thisColumn
    Else
      Set thisColumn = .Cols(1)
    End If
    With thisColumn
      .IsColumn = True
      .SectionAttribute = cboAtt(1).Text
      Set .Format = Nothing
      Set .Format = New ATCnumberFormat
      .Format.DecimalPlaces = dpla
      .Format.SignificantDigits = SDig
      .Format.Width = CWid
    End With
    
    CWid = frmReportSpecs.CWid
    SDig = frmReportSpecs.SDig
    dpla = frmReportSpecs.dpla
    'PageSumm = frmReportSpecs.Summ(0)
    'ColSumm = frmReportSpecs.Summ(1)
    'RowSumm = frmReportSpecs.Summ(2)
    FName = frmReportSpecs.FName
  End With
End Sub

Private Sub Form_Load()
  Dim i&

  Set pTSer = New Collection
  AttHdrs(0, 0) = "Section"
  AttHdrs(1, 0) = "Column"
  AttHdrs(2, 0) = "Row"
  AttHdrs(0, 1) = "Field 1"
  AttHdrs(1, 1) = "Field 2"
  AttHdrs(2, 1) = "Field 3"
  optFormat(0).Value = True
  For i = 0 To 2
    cboAtt(i).ListIndex = i
  Next i
  For i = 0 To 4
    If Summs(i, 0) Then chkSectionSumm(i).Value = vbChecked
    If Summs(i, 1) Then chkColSumm(i).Value = vbChecked
    If Summs(i, 2) Then chkRowSumm(i).Value = vbChecked
  Next i
  
End Sub

Private Sub Form_Unload(Cancel As Integer)
  Set pTSer = Nothing
End Sub

Private Sub optFormat_Click(index As Integer)
  Dim i&

  For i = 0 To 2
    lblAtt(i).Caption = AttHdrs(i, index)
  Next i
  If optFormat(0).Value = True Then 'need summary boxes
    fraLayout.Height = chkSectionSumm(4).Top + chkSectionSumm(4).Height + 200
  Else 'don't need summary boxes
    fraLayout.Height = cboAtt(0).Top + cboAtt(0).Height + 200
  End If
  cmdClose(0).Top = fraLayout.Top + fraLayout.Height + 200
  cmdClose(1).Top = cmdClose(0).Top
  Me.Height = cmdClose(0).Top + cmdClose(0).Height + 500

End Sub

Public Property Get CWid() As Long
  CWid = atxColWid.Value
End Property

Public Property Let CWid(ByVal vNewValue As Long)
  atxColWid.Value = vNewValue
End Property

Public Property Get SDig() As Long
  SDig = atxSigDig.Value
End Property

Public Property Let SDig(ByVal vNewValue As Long)
  atxSigDig.Value = vNewValue
End Property

Public Property Get dpla() As Long
  dpla = atxDecPla.Value
End Property

Public Property Let dpla(ByVal vNewValue As Long)
  atxDecPla.Value = vNewValue
End Property

Public Property Let Title(ByVal vNewValue As String)
  txtTitle.Text = vNewValue
End Property

'Public Property Get FormFg() As Long
'  If optFormat(0).Value = True Then
'    FormFg = 0
'  Else
'    FormFg = 1
'  End If
'End Property

Public Property Let FormFg(ByVal vNewValue As Long)
  'optFormat(FormFg).Value = True
End Property

Public Property Get Summ(index As Integer) As SectType
  Summ.Attribute = cboAtt(index).Text
  Summ.sSum = Summs(0, index)
  Summ.sAve = Summs(1, index)
  Summ.sMin = Summs(2, index)
  Summ.sMax = Summs(3, index)
  Summ.sCnt = Summs(4, index)
End Property

Public Property Let Summ(index As Integer, vNewValue As SectType)
  cboAtt(index).Text = vNewValue.Attribute
  Summs(0, index) = vNewValue.sSum
  Summs(1, index) = vNewValue.sAve
  Summs(2, index) = vNewValue.sMin
  Summs(3, index) = vNewValue.sMax
  Summs(4, index) = vNewValue.sCnt
End Property

Public Property Get FName() As String
  FName = pFName
End Property

Public Property Let FName(ByVal vNewValue As String)
  pFName = vNewValue
End Property

Public Property Set TSerColl(ByVal vNewValue As Collection)
  Dim i As Integer
  Set pTSer = vNewValue
  For i = 0 To 2
    cboAtt_Click (i)
  Next i
End Property
