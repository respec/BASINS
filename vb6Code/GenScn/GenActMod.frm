VERSION 5.00
Begin VB.Form frmGenActMod 
   Caption         =   "GenScn Activate Modify"
   ClientHeight    =   2772
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   3876
   HelpContextID   =   55
   LinkTopic       =   "Form1"
   ScaleHeight     =   2772
   ScaleWidth      =   3876
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdSchem 
      Caption         =   "&Network --> Schematic / Mass-Link"
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
      Height          =   735
      Left            =   240
      TabIndex        =   2
      Top             =   1080
      Width           =   1575
   End
   Begin VB.CommandButton cmdReport 
      Caption         =   "&Reports"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   735
      Left            =   2040
      TabIndex        =   1
      Top             =   120
      Width           =   1575
   End
   Begin VB.CommandButton cmdBMP 
      Caption         =   "&BMP"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   735
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Width           =   1575
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
      Height          =   615
      Left            =   1440
      TabIndex        =   4
      Top             =   2040
      Width           =   975
   End
   Begin VB.CommandButton cmdSpecAct 
      Caption         =   "&Special Actions"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   735
      Left            =   2040
      TabIndex        =   3
      Top             =   1080
      Width           =   1575
   End
End
Attribute VB_Name = "frmGenActMod"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Sub cmdBMP_Click()
    frmGenActModBMP.Show 1
End Sub

Private Sub cmdClose_Click()
   Unload Me
End Sub

Private Sub cmdReport_Click()
    frmGenActModReport.Show 1
End Sub

Private Sub cmdSchem_Click()
    Dim Init&, OmCode&, scount&, ml1count&, retkey&, retcod&
    Dim cbuff$, SrcName$, TarName$, SrcOp$, SrcMem$, Mfact$
    Dim Rfact!, Tstrg$, TarOp$, TarMem$, ifound&
    Dim SchRec$(), ML1Rec$(), ML2Rec$(), ML3Rec$()
    Dim ml2count&, ml3count&, i&, Key&
    Dim delcnt&, delrec&(), iresp
    
    iresp = MsgBox("This function will make changes to your UCI file, save it to disk, and reactivate this scenario." & vbCrLf & vbCrLf & _
      "Are you sure you want to continue?", 1, "Network --> Schematic/Mass-Link")
    If iresp = 1 Then
      Init = 1
      OmCode = 7
      scount = 0
      ml1count = 0
      ml2count = 0
      ml3count = 0
      delcnt = 0
      Do
        'look through each record of network block
        Call F90_XBLOCK(OmCode, Init, retkey, cbuff, retcod)
        'parse the records into pieces
        SrcName = Mid(cbuff, 1, 6)
        TarName = Mid(cbuff, 44, 6)
        SrcOp = Mid(cbuff, 1, 10)
        SrcMem = Mid(cbuff, 12, 17)
        Mfact = Mid(cbuff, 29, 10)
        If SrcMem = "PWATER PERO      " Or SrcMem = "IWATER SURO      " Then
          'need to mult area by 12
          Rfact = CSng(Mfact) * 12#
          Mfact = CStr(Rfact)
        End If
        Tstrg = Mid(cbuff, 39, 4)
        TarOp = Mid(cbuff, 44, 10)
        TarMem = Mid(cbuff, 59, 17)
        If (SrcName = "PERLND" Or SrcName = "IMPLND" Or SrcName = "RCHRES") And TarName = "RCHRES" Then
          'only worry about these at this point
          ifound = 0
          For i = 1 To scount
            'see if this schem record already is in the list
            If Mid(SchRec(i - 1), 1, 10) = SrcOp And Mid(SchRec(i - 1), 44, 10) = TarOp Then
              ifound = 1
              'Mid(SchRec(i - 1), 29, 10) = Mfact  isnt going to work for sosed
            End If
          Next i
          If ifound = 0 Then
            'add this record to schematic list
            scount = scount + 1
            ReDim Preserve SchRec(scount)
            While Len(Mfact) < 10
              Mfact = " " & Mfact
            Wend
            'build the new record
            If SrcName = "PERLND" Then
              SchRec(scount - 1) = SrcOp & "                  " & Mfact & "     " & TarOp & "      1"
            ElseIf SrcName = "IMPLND" Then
              SchRec(scount - 1) = SrcOp & "                  " & Mfact & "     " & TarOp & "      2"
            ElseIf SrcName = "RCHRES" Then
              SchRec(scount - 1) = SrcOp & "                  " & Mfact & "     " & TarOp & "      3"
            End If
          End If
          If SrcName = "PERLND" Then
            'add this record to mass link 1 list
            ifound = 0
            For i = 1 To ml1count
              If Mid(ML1Rec(i - 1), 1, 6) = SrcName And Mid(ML1Rec(i - 1), 12, 17) = SrcMem And Mid(ML1Rec(i - 1), 44, 6) = TarName And Mid(ML1Rec(i - 1), 59, 17) = TarMem Then
                ifound = 1
              End If
            Next i
            If ifound = 0 Then
              'not already in list
              ml1count = ml1count + 1
              ReDim Preserve ML1Rec(ml1count)
              If SrcMem = "PWATER PERO      " Then
                ML1Rec(ml1count - 1) = SrcName & "     " & SrcMem & " 0.0833333" & Tstrg & " " & TarName & "         " & TarMem
              Else
                ML1Rec(ml1count - 1) = SrcName & "     " & SrcMem & "          " & Tstrg & " " & TarName & "         " & TarMem
              End If
            End If
          ElseIf SrcName = "IMPLND" Then
            'add this record to mass link 2 list
            ifound = 0
            For i = 1 To ml2count
              If Mid(ML2Rec(i - 1), 1, 6) = SrcName And Mid(ML2Rec(i - 1), 12, 17) = SrcMem And Mid(ML2Rec(i - 1), 44, 6) = TarName And Mid(ML2Rec(i - 1), 59, 17) = TarMem Then
                ifound = 1
              End If
            Next i
            If ifound = 0 Then
              'not already in list
              ml2count = ml2count + 1
              ReDim Preserve ML2Rec(ml2count)
              If SrcMem = "IWATER SURO      " Then
                ML2Rec(ml2count - 1) = SrcName & "     " & SrcMem & " 0.0833333" & Tstrg & " " & TarName & "         " & TarMem
              Else
                ML2Rec(ml2count - 1) = SrcName & "     " & SrcMem & "          " & Tstrg & " " & TarName & "         " & TarMem
              End If
            End If
          ElseIf SrcName = "RCHRES" Then
            'add this record to mass link 3 list
            ifound = 0
            For i = 1 To ml3count
              If Mid(ML3Rec(i - 1), 1, 6) = SrcName And Mid(ML3Rec(i - 1), 12, 17) = SrcMem And Mid(ML3Rec(i - 1), 44, 6) = TarName And Mid(ML3Rec(i - 1), 59, 17) = TarMem Then
                ifound = 1
              End If
            Next i
            If ifound = 0 Then
              'not already in list
              ml3count = ml3count + 1
              ReDim Preserve ML3Rec(ml3count)
              ML3Rec(ml3count - 1) = SrcName & "     " & SrcMem & "          " & Tstrg & " " & TarName & "         " & TarMem
            End If
          End If
          'store list of records to delete
          delcnt = delcnt + 1
          ReDim Preserve delrec(delcnt)
          delrec(delcnt - 1) = retkey
        End If
      
        Init = 0
      Loop While retcod = 1 Or retcod = 2
    
      'now put the new records to the uci file
      Key = retkey
      If ml1count > 0 Or ml2count > 0 Or ml3count > 0 Then
        'end the mass link block
        cbuff = "END MASS-LINK"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
      End If
      If ml3count > 0 Then
        cbuff = "  END MASS-LINK    3"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        For i = 1 To ml3count
          Call F90_PUTUCI(Key, 0, ML3Rec(ml3count - i), Len(ML3Rec(ml3count - i)))
        Next i
        cbuff = "<Name>            <Name> x x<-factor->strg <Name>                <Name> x x  ***"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        cbuff = "<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Target vols> <-Grp> <-Member->  ***"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        cbuff = "  MASS-LINK        3"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
      End If
      If ml2count > 0 Then
        cbuff = "  END MASS-LINK    2"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        For i = 1 To ml2count
          Call F90_PUTUCI(Key, 0, ML2Rec(ml2count - i), Len(ML2Rec(ml2count - i)))
        Next i
        cbuff = "<Name>            <Name> x x<-factor->strg <Name>                <Name> x x  ***"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        cbuff = "<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Target vols> <-Grp> <-Member->  ***"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        cbuff = "  MASS-LINK        2"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
      End If
      If ml1count > 0 Then
        cbuff = "  END MASS-LINK    1"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        For i = 1 To ml1count
          Call F90_PUTUCI(Key, 0, ML1Rec(ml1count - i), Len(ML1Rec(ml1count - i)))
        Next i
        cbuff = "<Name>            <Name> x x<-factor->strg <Name>                <Name> x x  ***"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        cbuff = "<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Target vols> <-Grp> <-Member->  ***"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        cbuff = "  MASS-LINK        1"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
      End If
      If ml1count > 0 Or ml2count > 0 Or ml3count > 0 Then
        'start the mass link block
        cbuff = "MASS-LINK"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
      End If
      If scount > 0 Then
        'put schematic block
        cbuff = "END SCHEMATIC"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        For i = 1 To scount
          Call F90_PUTUCI(Key, 0, SchRec(scount - i), Len(SchRec(scount - i)))
        Next i
        cbuff = "<Name>   x                  <-factor->     <Name>   x        ***        # #"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        cbuff = "<-Volume->                  <--Area-->     <-Volume->  <ML#> ***       <sb>"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
        cbuff = "SCHEMATIC"
        Call F90_PUTUCI(Key, 0, cbuff, Len(cbuff))
      End If
      'now delete the records that we no longer need
      For i = 1 To delcnt
        Call F90_DELUCI(delrec(i - 1))
      Next i
      cmdSchem.Enabled = False
    
      'the rest of this subroutine is needed to refresh
      'the activate window without adding new code to hass_ent.
    
      'save the uci file
      If scount > 0 Then
        Call frmGenScnActivate.SaveUci
        Unload frmGenScnActivate
        Load frmGenScnActivate
        frmGenActMod.Show 1
      End If
    End If
End Sub

Private Sub cmdSpecAct_Click()
Dim ifound&, iexist As Boolean, i&

   iexist = frmGenScnActivate.OperationExists("SPEC-ACTIONS")

   If iexist Then
     ChDriveDir p.StatusFilePath
     MousePointer = vbHourglass
     'does input file exist?
     On Error GoTo 20
     Open "uvquan.inp" For Input As #1
     'yes, it exists
     Close #1
     GoTo 30
20     'no, it does not exist
     iexist = False
30   If iexist Then
       frmGenActTrans.Show 1
     Else
       MsgBox "The system file 'uvquan.inp' does not exist.", _
          vbExclamation, "GenScn Activate Modify SpecialActions"
     End If
   Else
     MsgBox "The Special Actions option requires a scenario containing a Special Actions block." & vbCrLf & _
          "The current scenario does not have a Special Actions block.", _
          vbExclamation, "GenScn Activate Modify SpecialActions"
   End If
   MousePointer = vbDefault
End Sub

Private Sub Form_Load()
  Dim nexist As Boolean, sexist As Boolean
  Me.Icon = frmGenScnActivate.Icon

  nexist = frmGenScnActivate.OperationExists("NETWORK")
  sexist = frmGenScnActivate.OperationExists("SCHEMATIC")
  If nexist And Not sexist Then
    cmdSchem.Enabled = True
  End If
End Sub
