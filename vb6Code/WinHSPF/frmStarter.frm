VERSION 5.00
Begin VB.Form frmStarter 
   Caption         =   "WinHSPF - Starting Values Manager"
   ClientHeight    =   4716
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   6588
   Icon            =   "frmStarter.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4716
   ScaleWidth      =   6588
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdStarter 
      Caption         =   "&Set Starter"
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
      Left            =   3480
      TabIndex        =   4
      ToolTipText     =   "Set the file containing the Starting Values"
      Top             =   3720
      Width           =   1452
   End
   Begin VB.CommandButton cmdApply 
      Caption         =   "&Apply to UCI"
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
      Left            =   1800
      TabIndex        =   3
      ToolTipText     =   "Apply the Starter Values to the Current UCI"
      Top             =   3720
      Width           =   1452
   End
   Begin VB.CommandButton cmdCancel 
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
      Height          =   372
      Left            =   3480
      TabIndex        =   2
      Top             =   4200
      Width           =   972
   End
   Begin VB.CommandButton cmdOK 
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
      Height          =   372
      Left            =   2280
      TabIndex        =   1
      Top             =   4200
      Width           =   972
   End
   Begin ATCoCtl.ATCoGrid agdStarter 
      Height          =   3252
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   6132
      _ExtentX        =   10816
      _ExtentY        =   5736
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
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   4800
      Top             =   3840
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
End
Attribute VB_Name = "frmStarter"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub agdStarter_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits
End Sub

Private Sub agdStarter_RowColChange()
  DoLimits
End Sub

Private Sub agdStarter_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits
End Sub

Private Sub cmdApply_Click()
  Dim iresp&
  
  iresp = myMsgBox.Show("All parameters will be set to the values specified in 'starter.uci'." & vbCrLf & vbCrLf & _
                        "Are you sure you want to continue?", _
                        "Apply Starter Values", "+&Yes", "-&No")
  If iresp = 1 Then
    Call SaveSpecs
    setDefault myUci, defUci
  End If
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOk_Click()
  Call SaveSpecs
  Unload Me
End Sub

Private Sub cmdStarter_Click()
  Dim filepath$
  Dim vOpTyp As Variant, OpTyps As Variant, loptyp As HspfOpnBlk
  Dim vOpn As Variant, lOpn As HspfOperation, dOpn As HspfOperation
  
  CDFile.flags = &H8806&
  CDFile.Filter = "HSPF User Control Input Files (*.uci)"
  CDFile.Filename = "*.uci"
  CDFile.DialogTitle = "Select Starter UCI File"
  On Error GoTo 50
  CDFile.CancelError = True
  CDFile.Action = 1
  
  filepath = PathNameOnly(CDFile.Filename)
  HSPFMain.OpenDefaultUCI filepath, CDFile.Filename
  'unset the default oper ids
  OpTyps = Array("PERLND", "IMPLND", "RCHRES")
  For Each vOpTyp In OpTyps
    If myUci.OpnBlks(vOpTyp).Count > 0 Then
      Set loptyp = myUci.OpnBlks(vOpTyp)
      For Each vOpn In loptyp.Ids
        Set lOpn = vOpn
        lOpn.DefOpnId = 0
      Next vOpn
    End If
  Next vOpTyp
  RefreshGrid
  
50    'continue here on cancel
End Sub

Private Sub Form_Load()
  RefreshGrid
End Sub
  
Private Sub RefreshGrid()
  Dim vOpTyp As Variant, OpTyps As Variant, loptyp As HspfOpnBlk
  Dim vOpn As Variant, lOpn As HspfOperation, dOpn As HspfOperation
  Dim Id&, lrow&, Desc$, dcount&, darray$(), found As Boolean, i&
  
  With agdStarter
    .rows = 1
    .cols = 3
    .ColTitle(0) = "Project Operations"
    .ColTitle(1) = "Mapped From Starter Operation"
    .ColType(1) = ATCoTxt
    .ColEditable(1) = True
    .ColTitle(2) = "HIDE"
    .ColTitle(3) = "HIDE"
    OpTyps = Array("PERLND", "IMPLND", "RCHRES")
    lrow = 0
    For Each vOpTyp In OpTyps
      dcount = 0
      If myUci.OpnBlks(vOpTyp).Count > 0 Then
        Set loptyp = myUci.OpnBlks(vOpTyp)
        For Each vOpn In loptyp.Ids
          Set lOpn = vOpn
          If Len(Trim(lOpn.Description)) > 0 Then
            Desc = lOpn.Description
          Else
            Desc = "Unspecified"
          End If
          'check to see we havent already loaded this desc
          If dcount > 0 Then
            found = False
            For i = 1 To dcount
              If darray(i) = Desc Then
                found = True
              End If
            Next i
            If found = False Then
              dcount = dcount + 1
              ReDim Preserve darray(dcount)
              darray(dcount) = Desc
            End If
          Else
            dcount = dcount + 1
            ReDim Preserve darray(dcount)
            darray(dcount) = Desc
            found = False
          End If
          If found = False Then
            'add a row for this one
            lrow = lrow + 1
            .rows = lrow
            .TextMatrix(.rows, 0) = Desc & " (" & lOpn.Name & ")"
            If lOpn.DefOpnId <> 0 Then
              Set dOpn = defUci.OpnBlks(lOpn.Name).operfromid(lOpn.DefOpnId)
              Desc = dOpn.Description
              .TextMatrix(.rows, 1) = Desc & " (" & dOpn.Name & " " & dOpn.Id & ")"
              .TextMatrix(.rows, 2) = lOpn.DefOpnId
            Else
              Id = DefaultOpnId(lOpn, defUci)
              Set dOpn = defUci.OpnBlks(lOpn.Name).operfromid(Id)
              If Not dOpn Is Nothing Then
                Desc = dOpn.Description
                .TextMatrix(.rows, 1) = Desc & " (" & dOpn.Name & " " & dOpn.Id & ")"
              Else
                .TextMatrix(.rows, 1) = "None available"
              End If
              .TextMatrix(.rows, 2) = Id
            End If
            .TextMatrix(.rows, 3) = lOpn.Name
          End If
        Next vOpn
      End If
    Next vOpTyp
    .ColsSizeByContents
  End With
End Sub

Private Sub DoLimits()
  Dim lopname$
  Dim loptyp As HspfOpnBlk
  Dim vOpn As Variant
  Dim lOpn As HspfOperation
  Dim Desc As String
  
  With agdStarter
    .ClearValues
    If .col = 1 Then
      lopname = agdStarter.TextMatrix(.row, 3)
      If defUci.OpnBlks(lopname).Count > 0 Then
        Set loptyp = defUci.OpnBlks(lopname)
        For Each vOpn In loptyp.Ids
          Set lOpn = vOpn
          Desc = lOpn.Description
          agdStarter.AddValue Desc & " (" & lOpn.Name & " " & lOpn.Id & ")"
        Next vOpn
      End If
    End If
  End With
  UpdateDef
End Sub

Private Sub SaveSpecs()
  Dim vOpTyp As Variant, OpTyps As Variant, loptyp As HspfOpnBlk
  Dim vOpn As Variant, lOpn As HspfOperation, dOpn As HspfOperation
  Dim Id&, lrow&, Desc$
  Dim i As Long
  
  With agdStarter
    OpTyps = Array("PERLND", "IMPLND", "RCHRES")
    
    For Each vOpTyp In OpTyps
      If myUci.OpnBlks(vOpTyp).Count > 0 Then
        Set loptyp = myUci.OpnBlks(vOpTyp)
        For Each vOpn In loptyp.Ids
          Set lOpn = vOpn
          If Len(Trim(lOpn.Description)) > 0 Then
            Desc = lOpn.Description & " (" & lOpn.Name & ")"
          Else
            Desc = "Unspecified" & " (" & lOpn.Name & ")"
          End If
          For i = 1 To .rows
            If .TextMatrix(i, 0) = Desc Then
              lOpn.DefOpnId = .TextMatrix(i, 2)
            End If
          Next i
        Next vOpn
      End If
    Next vOpTyp
  End With
End Sub

Private Sub UpdateDef()
  Dim parpos As Long
  Dim spacepos As Long
  Dim ilen As Long
  Dim temp As String
  Dim ctemp As String
  Dim OpId As Long
  
  With agdStarter
    If .col = 1 Then
      temp = .TextMatrix(.row, .col)
      parpos = InStr(1, temp, "(")
      If parpos > 0 Then
        ilen = Len(temp)
        spacepos = InStr(parpos, temp, " ")
        ctemp = Mid(temp, spacepos, ilen - spacepos)
        If IsNumeric(ctemp) Then
          OpId = CInt(ctemp)
          .TextMatrix(.row, .col + 1) = OpId
        End If
      End If
    End If
  End With
End Sub

Private Sub Form_Resize()
  If width > 4000 Then
    agdStarter.width = width - 600
    cmdOK.Left = width / 2 - cmdOK.width - 100
    cmdCancel.Left = width / 2 + 100
    cmdApply.Left = width / 2 - cmdApply.width - 100
    cmdStarter.Left = width / 2 + 100
    agdStarter.ColsSizeToWidth
  End If
  If height > 2000 Then
    agdStarter.height = height - 1900
    cmdOK.Top = height - 900
    cmdCancel.Top = height - 900
    cmdApply.Top = height - 1400
    cmdStarter.Top = height - 1400
  End If
End Sub
