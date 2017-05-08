VERSION 5.00
Begin VB.Form frmAgPrac 
   Caption         =   "Add Pre-defined Agricultural Practice"
   ClientHeight    =   2832
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   6972
   LinkTopic       =   "Form1"
   ScaleHeight     =   2832
   ScaleWidth      =   6972
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraLayers 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   1092
      Left            =   120
      TabIndex        =   23
      Top             =   2760
      Width           =   2172
      Begin ATCoCtl.ATCoText atxUpper 
         Height          =   252
         Left            =   960
         TabIndex        =   24
         Top             =   600
         Width           =   972
         _ExtentX        =   1715
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   1
         HardMin         =   0
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxSurface 
         Height          =   252
         Left            =   960
         TabIndex        =   25
         Top             =   240
         Width           =   972
         _ExtentX        =   1715
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   1
         HardMin         =   0
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin VB.Label lblUpper 
         Caption         =   "Upper"
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
         TabIndex        =   28
         Top             =   600
         Width           =   1092
      End
      Begin VB.Label lblSurface 
         Caption         =   "Surface"
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
         TabIndex        =   27
         Top             =   360
         Width           =   972
      End
      Begin VB.Label lblLayer 
         Caption         =   "Layer Split:"
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
         TabIndex        =   26
         Top             =   0
         Width           =   1812
      End
   End
   Begin VB.CommandButton cmdOk 
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
      Height          =   495
      Left            =   2520
      TabIndex        =   22
      Top             =   2160
      Width           =   852
   End
   Begin VB.CommandButton cmdCancel 
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
      Height          =   495
      Left            =   3600
      TabIndex        =   21
      Top             =   2160
      Width           =   852
   End
   Begin VB.Frame fraProps 
      BorderStyle     =   0  'None
      Height          =   972
      Left            =   120
      TabIndex        =   4
      Top             =   2160
      Width           =   6732
      Begin VB.ComboBox cboRepeat 
         Height          =   288
         Left            =   3360
         Style           =   2  'Dropdown List
         TabIndex        =   7
         Top             =   240
         Width           =   1332
      End
      Begin VB.CheckBox chkDelay 
         Caption         =   "Defer if Raining"
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
         Left            =   2520
         TabIndex        =   5
         Top             =   720
         Width           =   1812
      End
      Begin ATCoCtl.ATCoText atxValue 
         Height          =   252
         Index           =   0
         Left            =   5760
         TabIndex        =   6
         Top             =   240
         Width           =   972
         _ExtentX        =   1715
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   2
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxStart 
         Height          =   252
         Index           =   1
         Left            =   720
         TabIndex        =   8
         Top             =   240
         Width           =   492
         _ExtentX        =   868
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   12
         HardMin         =   1
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxStart 
         Height          =   252
         Index           =   0
         Left            =   0
         TabIndex        =   9
         Top             =   240
         Width           =   612
         _ExtentX        =   1080
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxStart 
         Height          =   252
         Index           =   2
         Left            =   1320
         TabIndex        =   10
         Top             =   240
         Width           =   492
         _ExtentX        =   868
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   31
         HardMin         =   1
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxStart 
         Height          =   252
         Index           =   3
         Left            =   1920
         TabIndex        =   11
         Top             =   240
         Width           =   492
         _ExtentX        =   868
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   24
         HardMin         =   0
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText atxStart 
         Height          =   252
         Index           =   4
         Left            =   2520
         TabIndex        =   12
         Top             =   240
         Width           =   492
         _ExtentX        =   868
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   60
         HardMin         =   0
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   -999
         Value           =   "-999"
         Enabled         =   -1  'True
      End
      Begin VB.Label Label13 
         Alignment       =   2  'Center
         Caption         =   "Min"
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
         Left            =   2520
         TabIndex        =   20
         Top             =   0
         Width           =   492
      End
      Begin VB.Label Label12 
         Alignment       =   2  'Center
         Caption         =   "Hr"
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
         Left            =   1920
         TabIndex        =   19
         Top             =   0
         Width           =   492
      End
      Begin VB.Label Label11 
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
         Height          =   252
         Left            =   1320
         TabIndex        =   18
         Top             =   0
         Width           =   492
      End
      Begin VB.Label Label10 
         Alignment       =   2  'Center
         Caption         =   "Mo"
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
         Left            =   720
         TabIndex        =   17
         Top             =   0
         Width           =   492
      End
      Begin VB.Label Label9 
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
         Height          =   252
         Left            =   0
         TabIndex        =   16
         Top             =   0
         Width           =   612
      End
      Begin VB.Label lblRepeat 
         Caption         =   "Repeat Interval"
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
         Left            =   3360
         TabIndex        =   15
         Top             =   0
         Width           =   1692
      End
      Begin VB.Label lblValue 
         Caption         =   "Parameter Value"
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
         Left            =   5040
         TabIndex        =   14
         Top             =   0
         Width           =   1692
      End
      Begin VB.Label lblName 
         Caption         =   "Name"
         Height          =   252
         Index           =   0
         Left            =   5040
         TabIndex        =   13
         Top             =   240
         Width           =   732
      End
   End
   Begin VB.ListBox lstPrac 
      Height          =   1392
      Left            =   120
      TabIndex        =   1
      Top             =   360
      Width           =   3252
   End
   Begin VB.ListBox lstSeg 
      Height          =   1392
      Left            =   3600
      TabIndex        =   0
      Top             =   360
      Width           =   3252
   End
   Begin VB.Label lblPrac 
      Caption         =   "Agricultural Practices"
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
      TabIndex        =   3
      Top             =   120
      Width           =   2172
   End
   Begin VB.Label lblDown 
      Caption         =   "Land Segments"
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
      Left            =   3600
      TabIndex        =   2
      Top             =   120
      Width           =   2172
   End
End
Attribute VB_Name = "frmAgPrac"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pUci As HspfUci
Dim pCtl As ctlSpecialActionEdit

Dim cPracIDs As New Collection
Dim cPracRecs As New Collection

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOK_Click()
  Dim Id&, i&, perlndid&, cbuff$, ctmp$, itype&, uvq$
  Dim rDate&(5), SDate&(5), EDate&(5), nrepeat&, RecordCount&
  Dim LayerFlag As Boolean, j&, Sub1$, Sub2$, DistribID&
  Dim FirstAction As Boolean, WroteAction As Boolean
  Dim strid As String
  
  'if one practice and one land use selected, apply
  If lstPrac.SelCount = 1 And lstSeg.SelCount = 1 Then
    
    ctmp = Mid(lstSeg.List(lstSeg.ListIndex), 8)
    perlndid = StrRetRem(ctmp)
    
    If chkDelay.Value = 1 Then
      'deferring, add uvquan and conditional
      uvq = pCtl.UVQuanInUse("PERLND", perlndid)
      If Len(uvq) = 0 Then
        'find an unused uvquan name
        uvq = pCtl.NextUVQuanName("prec")
        If Len(uvq) = 5 Then
          uvq = uvq & " "
        End If
        'write this one to spec act records
        strid = "   "
        RSet strid = CStr(perlndid)
        cbuff = "  UVQUAN " & uvq & " PERLND " & strid & " PREC             3                 DY  1 SUM"
        pCtl.AddToBeginning cbuff, hUserDefineQuan
      End If
    End If
  
    LayerFlag = False
    FirstAction = True
    WroteAction = False
    DistribID = 0
    Id = lstPrac.ListIndex + 1
    RecordCount = 0
    For i = 1 To cPracIDs.Count
      If cPracIDs(i) = Id Then
        'identify the type of record
        cbuff = cPracRecs(i)
        If Len(cbuff) = 0 Or InStr(cbuff, "***") > 0 Then
          itype = hComment
        ElseIf Left(Trim(cbuff), 3) = "IF " Or _
               Left(Trim(cbuff), 4) = "ELSE" Or _
               Left(Trim(cbuff), 6) = "END IF" Then
          itype = hCondition
        ElseIf Mid(cbuff, 3, 6) = "DISTRB" Then
          itype = hDistribute
          DistribID = pCtl.NextDistribNumber
        ElseIf Mid(cbuff, 3, 6) = "UVNAME" Then
          itype = hUserDefineName
        ElseIf Mid(cbuff, 3, 6) = "UVQUAN" Then
          itype = hUserDefineQuan
        Else
          itype = hAction
        End If
        'modify this record as appropriate
        If itype = hUserDefineQuan Then
          'set operation number
          strid = "   "
          RSet strid = CStr(perlndid)
          cbuff = Mid(cbuff, 1, 23) & strid & Mid(cbuff, 27)
        ElseIf itype = hUserDefineName Then
          'create var name from upper/surface split
          LayerFlag = True
          strid = CStr(atxSurface.Value)
          j = InStr(1, strid, ".")
          Sub1 = " "
          If j > 0 And Len(strid) >= j + 1 Then
            Sub1 = Mid(strid, j + 1, 1)
          End If
          strid = CStr(atxUpper.Value)
          j = InStr(1, strid, ".")
          Sub2 = " "
          If j > 0 And Len(strid) >= j + 1 Then
            Sub2 = Mid(strid, j + 1, 1)
          End If
          cbuff = Mid(cbuff, 1, 13) & Sub1 & Sub2 & Mid(cbuff, 16)
          strid = "     "
          RSet strid = CStr(atxSurface.Value)
          cbuff = Mid(cbuff, 1, 36) & strid & Mid(cbuff, 42)
          strid = "     "
          RSet strid = CStr(atxUpper.Value)
          cbuff = Mid(cbuff, 1, 66) & strid & Mid(cbuff, 72)
        ElseIf itype = hDistribute Then
          'change distrib id
          strid = "   "
          RSet strid = CStr(DistribID)
          cbuff = Mid(cbuff, 1, 8) & strid & Mid(cbuff, 12)
        ElseIf itype = hAction Then
          RecordCount = RecordCount + 1
          
          If FirstAction And chkDelay.Value = 1 Then
            'need to open conditional
            pCtl.AddToEnd "IF (" & Trim(uvq) & " < 0.11) THEN", hCondition
            FirstAction = False
          End If
          
          'set operation number
          strid = "   "
          RSet strid = CStr(perlndid)
          cbuff = Mid(cbuff, 1, 8) & strid & Mid(cbuff, 12)
          'check if defering
          If chkDelay.Value = 1 Then
            cbuff = Mid(cbuff, 1, 15) & "DY  1" & Mid(cbuff, 21)
          Else
            cbuff = Mid(cbuff, 1, 15) & "     " & Mid(cbuff, 21)
          End If
          'start year
          strid = "    "
          RSet strid = CStr(atxStart(0).Value)
          cbuff = Mid(cbuff, 1, 20) & strid & Mid(cbuff, 25)
          'start mon
          strid = "   "
          RSet strid = CStr(atxStart(1).Value)
          If atxStart(1).Value <> 0 Then
            cbuff = Mid(cbuff, 1, 24) & strid & Mid(cbuff, 28)
          Else
            cbuff = Mid(cbuff, 1, 24) & "   " & Mid(cbuff, 28)
          End If
          'start day
          strid = "   "
          RSet strid = CStr(atxStart(2).Value)
          If atxStart(2).Value <> 0 Then
            cbuff = Mid(cbuff, 1, 27) & strid & Mid(cbuff, 31)
          Else
            cbuff = Mid(cbuff, 1, 27) & "   " & Mid(cbuff, 31)
          End If
          'start hr
          strid = "   "
          RSet strid = CStr(atxStart(3).Value)
          If atxStart(3).Value <> 0 Then
            cbuff = Mid(cbuff, 1, 30) & strid & Mid(cbuff, 34)
          Else
            cbuff = Mid(cbuff, 1, 30) & "   " & Mid(cbuff, 34)
          End If
          'start min
          strid = "   "
          RSet strid = CStr(atxStart(4).Value)
          If atxStart(4).Value <> 0 Then
            cbuff = Mid(cbuff, 1, 33) & strid & Mid(cbuff, 37)
          Else
            cbuff = Mid(cbuff, 1, 33) & "   " & Mid(cbuff, 37)
          End If
          'distrib id
          If IsNumeric(Mid(cbuff, 39, 3)) And DistribID > 0 Then
            strid = "   "
            RSet strid = CStr(DistribID)
            cbuff = Mid(cbuff, 1, 35) & strid & Mid(cbuff, 39)
          End If
          If LayerFlag Then
            'update uvname
            cbuff = Mid(cbuff, 1, 45) & Sub1 & Sub2 & Mid(cbuff, 48)
          End If
          'value
          strid = "          "
          RSet strid = CStr(atxValue(RecordCount - 1).Value)
          cbuff = Mid(cbuff, 1, 60) & strid & Mid(cbuff, 71)
          'check if repeating
          If cboRepeat.List(cboRepeat.ListIndex) <> "None" Then
            'repeating
            If Len(Mid(cbuff, 75, 3)) = 0 Then
              cbuff = Mid(cbuff, 1, 71) & cboRepeat.List(cboRepeat.ListIndex) & "   1" & Mid(cbuff, 78)
            Else
              cbuff = Mid(cbuff, 1, 71) & cboRepeat.List(cboRepeat.ListIndex) & Mid(cbuff, 74)
            End If
          Else
            cbuff = Mid(cbuff, 1, 71) & "      " & Mid(cbuff, 77)
          End If
          If Mid(cbuff, 72, 2) = "YR" Then
            'yearly repeating
            'check number of repeats
            EDate(0) = pUci.GlobalBlock.EDate(0)
            EDate(1) = pUci.GlobalBlock.EDate(1)
            EDate(2) = pUci.GlobalBlock.EDate(2)
            EDate(3) = pUci.GlobalBlock.EDate(3)
            EDate(4) = pUci.GlobalBlock.EDate(4)
            SDate(0) = atxStart(0).Value
            SDate(1) = atxStart(1).Value
            SDate(2) = atxStart(2).Value
            SDate(3) = atxStart(3).Value
            SDate(4) = atxStart(4).Value
            nrepeat = Int((Date2J(EDate) - Date2J(SDate)) / 365)
            strid = "   "
            RSet strid = CStr(nrepeat)
            cbuff = Mid(cbuff, 1, 77) & strid
          End If
          WroteAction = True
        End If
        'add to spec act list
        If itype = hUserDefineName Then
          'check to make sure this name hasn't already been used
          ctmp = Mid(cbuff, 11, 5)
          If Not pCtl.UVNameInUse(ctmp) Then
            pCtl.AddToEnd cbuff, itype
          End If
        Else
          pCtl.AddToEnd cbuff, itype
        End If
      End If
    Next i
    If WroteAction And chkDelay.Value = 1 Then
      'need to close conditional
      pCtl.AddToEnd "END IF", hCondition
    End If
    Unload Me
  Else
    myMsgBox.Show "One Practice and One Land Segment must be selected.", "Add Ag Practice Problem", "OK"
  End If
End Sub

Private Sub Form_Load()
  Dim vOper As Variant, lOper As HspfOperation
  Dim i&, tname$, lstr$, ilen&, tstr$
  Dim ctmp$, pcount&, cend As Boolean

  fraLayers.Visible = False
  fraProps.Visible = False
  Height = 3300
  cboRepeat.Clear
  cboRepeat.AddItem "None"
  cboRepeat.AddItem "YR"
  cboRepeat.AddItem "MO"
  cboRepeat.AddItem "DY"
  cboRepeat.AddItem "HR"
  cboRepeat.AddItem "MI"
  
  'read database
  i = FreeFile(0)
  On Error GoTo ErrHandler
  tname = pUci.StarterPath & "\" & "agpractice.txt"
  Open tname For Input As #i
  pcount = 0
  
  Do Until EOF(i)
    Line Input #i, lstr
    ilen = Len(lstr)
    If ilen > 6 Then
      If Left(lstr, 7) = "PRACTIC" Then
        'found start of a practice
        ctmp = StrRetRem(lstr)
        lstPrac.AddItem lstr
        pcount = pcount + 1
        cend = False
        Do While Not cend
          Line Input #i, lstr
          tstr = Trim(lstr)
          ilen = Len(tstr)
          If Left(tstr, ilen) = "END PRACTICE" Then
            'found end of practice
            cend = True
          Else
            cPracIDs.Add pcount
            cPracRecs.Add lstr
          End If
        Loop
      End If
    End If
  Loop
  Close #i
  GoTo FillLists
ErrHandler:
  If err.Number = 53 Then
    MsgBox "File " & tname & " not found.", vbOKOnly, "Read Ag Practices Problem"
  Else
    MsgBox err.Description & vbCrLf & vbCrLf & lstr, _
      vbOKOnly, "Read Ag Practices Problem"
  End If
        
FillLists:
  'fill list of land segments
  For Each vOper In pUci.OpnSeqBlock.Opns
    Set lOper = vOper
    If lOper.Name = "PERLND" Then
      lstSeg.AddItem lOper.Name & " " & lOper.Id & " (" & lOper.Description & ")"
    End If
  Next vOper
  
  atxStart(0).HardMax = pUci.GlobalBlock.EDate(0)
  atxStart(0).HardMin = pUci.GlobalBlock.SDate(0)
End Sub

Public Sub init(u As HspfUci, ctl As ctlSpecialActionEdit)
  Set pUci = u
  Set pCtl = ctl
  
  Me.icon = ctl.frm.icon
End Sub

Private Sub Form_Resize()
  If Not (Me.WindowState = vbMinimized) Then
    If Width < 4200 Then Width = 4200
    If Height < 1500 Then Height = 1500
    cmdOk.Top = Height - cmdOk.Height - 440
    cmdCancel.Top = Height - cmdCancel.Height - 440
    cmdOk.Left = Width / 2 - cmdOk.Width - 100
    cmdCancel.Left = Width / 2 + 100
    lstSeg.Left = Width / 2 + 100
    lblDown.Left = Width / 2 + 100
    lstSeg.Width = Width / 2 - 400
    lstPrac.Width = Width / 2 - 200
    If lstSeg.SelCount < 1 Or lstPrac.SelCount < 1 Then
      lstSeg.Height = Height - 2 * cmdOk.Height - 400
      lstPrac.Height = Height - 2 * cmdOk.Height - 400
    Else
      lstSeg.Height = Height - 2 * cmdOk.Height - 1400
      lstPrac.Height = Height - 2 * cmdOk.Height - 1400
    End If
    fraProps.Top = lstSeg.Height + 440
    fraLayers.Top = lstSeg.Height + 1040
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  Do Until cPracIDs.Count = 0
    cPracIDs.Remove 1
  Loop
  Do Until cPracRecs.Count = 0
    cPracRecs.Remove 1
  Loop
End Sub

Private Sub lstPrac_Click()
  If lstPrac.SelCount = 1 And lstSeg.SelCount = 1 Then
    ShowDetails
  End If
End Sub

Private Sub lstSeg_Click()
  If lstPrac.SelCount = 1 And lstSeg.SelCount = 1 Then
    ShowDetails
  End If
End Sub

Private Sub ShowDetails()
  Dim i&, cbuff$, itype&, ctmp$
  Dim rDate&(5), SDate&(5), ActionCount&
  Dim LayerFlag As Boolean
  Dim Id As Long
  
  Id = lstPrac.ListIndex + 1
  
  LayerFlag = False
  For i = 1 To cPracIDs.Count
    If cPracIDs(i) = Id Then
      If Mid(cPracRecs(i), 3, 6) = "UVNAME" Then
        LayerFlag = True
      End If
    End If
  Next i
  
  If fraProps.Visible = False Then
    fraProps.Visible = True
    Height = Height + 1000
    'cmdOk.Top = cmdOk.Top + 1000
    'cmdCancel.Top = cmdCancel.Top + 1000
  End If
  If LayerFlag Then
    fraLayers.Visible = True
  Else
    fraLayers.Visible = False
  End If
  
  ActionCount = 0
  'unload unneeded members of control array
  If lblName.UBound > 0 Then
    For i = 1 To lblName.UBound
      Unload lblName(i)
    Next i
  End If
  If atxValue.UBound > 0 Then
    For i = 1 To atxValue.UBound
      Unload atxValue(i)
    Next i
  End If
  
  For i = 1 To cPracIDs.Count
    If cPracIDs(i) = Id Then
      'identify the type of record
      cbuff = cPracRecs(i)
      If Len(cbuff) = 0 Or InStr(cbuff, "***") > 0 Then
        itype = hComment
      ElseIf Left(Trim(cbuff), 3) = "IF " Or _
             Left(Trim(cbuff), 4) = "ELSE" Or _
             Left(Trim(cbuff), 6) = "END IF" Then
        itype = hCondition
      ElseIf Mid(cbuff, 3, 6) = "DISTRB" Then
        itype = hDistribute
      ElseIf Mid(cbuff, 3, 6) = "UVNAME" Then
        itype = hUserDefineName
      ElseIf Mid(cbuff, 3, 6) = "UVQUAN" Then
        itype = hUserDefineQuan
      Else
        itype = hAction
      End If
      'extract info as needed
      If itype = hUserDefineName Then
        'assume that the uvname will have surface/upper distrib
        atxSurface.Value = Mid(cbuff, 37, 5)
        atxUpper.Value = Mid(cbuff, 67, 5)
      ElseIf itype = hAction Then
        ActionCount = ActionCount + 1
        'check if repeating
        ctmp = Mid(cbuff, 72, 2)
        If ctmp = "YR" Then
          cboRepeat.ListIndex = 1
        ElseIf ctmp = "MO" Then
          cboRepeat.ListIndex = 2
        ElseIf ctmp = "DY" Then
          cboRepeat.ListIndex = 3
        ElseIf ctmp = "HR" Then
          cboRepeat.ListIndex = 4
        ElseIf ctmp = "MI" Then
          cboRepeat.ListIndex = 5
        Else
          cboRepeat.ListIndex = 0
        End If
        'check for dated action
        If Len(Trim(Mid(cbuff, 21, 4))) > 0 Then
          rDate(0) = CInt(Mid(cbuff, 21, 4))
          If Mid(cbuff, 25, 3) = "   " Then
            rDate(1) = 0
          Else
            rDate(1) = CInt(Mid(cbuff, 25, 3))
          End If
          If Mid(cbuff, 28, 3) = "   " Then
            rDate(2) = 0
          Else
            rDate(2) = CInt(Mid(cbuff, 28, 3))
          End If
          If Mid(cbuff, 31, 3) = "   " Then
            rDate(3) = 0
          Else
            rDate(3) = CInt(Mid(cbuff, 31, 3))
          End If
          If Mid(cbuff, 34, 3) = "   " Then
            rDate(4) = 0
          Else
            rDate(4) = CInt(Mid(cbuff, 34, 3))
          End If
          SDate(0) = pUci.GlobalBlock.SDate(0)
          SDate(1) = pUci.GlobalBlock.SDate(1)
          SDate(2) = pUci.GlobalBlock.SDate(2)
          SDate(3) = pUci.GlobalBlock.SDate(3)
          SDate(4) = pUci.GlobalBlock.SDate(4)
          If Mid(cbuff, 72, 2) = "DY" Then
            'set date to start of run
            atxStart(0).Value = SDate(0)
            atxStart(1).Value = SDate(1)
            atxStart(2).Value = SDate(2)
            atxStart(3).Value = SDate(3)
            atxStart(4).Value = SDate(4)
          Else
            'yearly repeating or other
            rDate(0) = SDate(0)
            If Date2J(rDate) < Date2J(SDate) Then
              'change to following year
              rDate(0) = rDate(0) + 1
            End If
            atxStart(0).Value = rDate(0)
            atxStart(1).Value = rDate(1)
            atxStart(2).Value = rDate(2)
            atxStart(3).Value = rDate(3)
            atxStart(4).Value = rDate(4)
          End If
        End If
        'fill names and values
        If ActionCount > lblName.UBound + 1 Then
          Load lblName(ActionCount - 1)
          lblName(ActionCount - 1).Top = lblName(ActionCount - 2).Top + 300
          lblName(ActionCount - 1).Visible = True
        End If
        If ActionCount > atxValue.UBound + 1 Then
          Load atxValue(ActionCount - 1)
          atxValue(ActionCount - 1).Top = atxValue(ActionCount - 2).Top + 300
          atxValue(ActionCount - 1).Visible = True
        End If
        If LayerFlag Then
          lblName(ActionCount - 1) = Mid(cbuff, 43, 3)
        Else
          lblName(ActionCount - 1) = Mid(cbuff, 43, 6)
        End If
        atxValue(ActionCount - 1).Value = Mid(cbuff, 61, 10)
        If Len(Mid(cbuff, 16, 2)) > 0 Then
          'defer by default
          chkDelay.Value = 1
        End If
      End If
    End If
  Next i
End Sub
