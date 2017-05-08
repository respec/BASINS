VERSION 5.00
Begin VB.Form frmControl 
   Caption         =   "WinHSPF - Control Cards"
   ClientHeight    =   4932
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   7908
   HelpContextID   =   38
   Icon            =   "frmControl.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4932
   ScaleWidth      =   7908
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraControl 
      BorderStyle     =   0  'None
      Height          =   4812
      Left            =   0
      TabIndex        =   0
      Top             =   120
      Width           =   7932
      Begin VB.CommandButton cmdControl 
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
         Left            =   4080
         TabIndex        =   3
         Top             =   4320
         Width           =   1335
      End
      Begin VB.CommandButton cmdControl 
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
         Left            =   2520
         TabIndex        =   2
         Top             =   4320
         Width           =   1335
      End
      Begin TabDlg.SSTab SSTabPIR 
         Height          =   3852
         Left            =   240
         TabIndex        =   1
         Top             =   240
         Width           =   7452
         _ExtentX        =   13145
         _ExtentY        =   6795
         _Version        =   393216
         Tab             =   2
         TabHeight       =   520
         TabCaption(0)   =   "&Pervious Land"
         TabPicture(0)   =   "frmControl.frx":08CA
         Tab(0).ControlEnabled=   0   'False
         Tab(0).Control(0)=   "fraPerlnd"
         Tab(0).ControlCount=   1
         TabCaption(1)   =   "&Impervious Land"
         TabPicture(1)   =   "frmControl.frx":08E6
         Tab(1).ControlEnabled=   0   'False
         Tab(1).Control(0)=   "fraImplnd"
         Tab(1).ControlCount=   1
         TabCaption(2)   =   "&Reaches/Reservoirs"
         TabPicture(2)   =   "frmControl.frx":0902
         Tab(2).ControlEnabled=   -1  'True
         Tab(2).Control(0)=   "fraRchres"
         Tab(2).Control(0).Enabled=   0   'False
         Tab(2).ControlCount=   1
         Begin VB.Frame fraPerlnd 
            BorderStyle     =   0  'None
            Height          =   3372
            Left            =   -74880
            TabIndex        =   8
            Top             =   360
            Width           =   7212
            Begin VB.CheckBox chkPer 
               Caption         =   "Check1"
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
               TabIndex        =   9
               Top             =   120
               Width           =   7092
            End
         End
         Begin VB.Frame fraImplnd 
            BorderStyle     =   0  'None
            Height          =   3372
            Left            =   -74880
            TabIndex        =   6
            Top             =   360
            Width           =   7212
            Begin VB.CheckBox chkImp 
               Caption         =   "Check1"
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
               TabIndex        =   7
               Top             =   360
               Width           =   7092
            End
         End
         Begin VB.Frame fraRchres 
            BorderStyle     =   0  'None
            Height          =   3372
            Left            =   120
            TabIndex        =   4
            Top             =   360
            Width           =   7212
            Begin VB.CheckBox chkRch 
               Caption         =   "Check1"
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
               TabIndex        =   5
               Top             =   240
               Width           =   7092
            End
         End
      End
   End
End
Attribute VB_Name = "frmControl"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim iChange As Boolean
Dim rChange As Boolean
Dim pChange As Boolean
Dim iChangedUponLoading As Boolean
Dim rChangedUponLoading As Boolean
Dim pChangedUponLoading As Boolean
Private WithEvents ctrl As VBControlExtender
Attribute ctrl.VB_VarHelpID = -1

Private Sub chkImp_Click(Index As Integer)
  iChange = True
  If chkImp(Index) = 1 Then
    Call CheckIChange(Index)
  End If
End Sub
Private Sub chkPer_Click(Index As Integer)
  pChange = True
  If chkPer(Index) = 1 Then
    Call CheckPChange(Index)
  End If
End Sub
Private Sub chkRch_Click(Index As Integer)
  rChange = True
  If chkRch(Index) = 1 Then
    Call CheckRChange(Index)
  End If
End Sub

Private Sub cmdControl_Click(Index As Integer)
  Dim lTable As HspfTable
  Dim lOpnBlk As HspfOperation, vOpnBlk As Variant
  Dim i&
  
  If Index = 0 And (iChange Or rChange Or pChange) Then
    'okay and something has changed
    For Each vOpnBlk In myUci.OpnBlks("PERLND").Ids
      Set lOpnBlk = vOpnBlk
      Set lTable = lOpnBlk.tables("ACTIVITY")
      For i = 1 To lTable.Parms.Count
        lTable.Parms(i).Value = chkPer(i - 1).Value
      Next i
    Next vOpnBlk
    
    For Each vOpnBlk In myUci.OpnBlks("IMPLND").Ids
      Set lOpnBlk = vOpnBlk
      Set lTable = lOpnBlk.tables("ACTIVITY")
      For i = 1 To lTable.Parms.Count
        lTable.Parms(i).Value = chkImp(i - 1).Value
      Next i
    Next vOpnBlk
    
    For Each vOpnBlk In myUci.OpnBlks("RCHRES").Ids
      Set lOpnBlk = vOpnBlk
      Set lTable = lOpnBlk.tables("ACTIVITY")
      For i = 1 To lTable.Parms.Count
        lTable.Parms(i).Value = chkRch(i - 1).Value
      Next i
    Next vOpnBlk
    
    'query for updating tables
    If pChange Then
      If AnyMissingTables("PERLND") Then
        i = myMsgBox.Show("Changes have been made to the PERLND control cards, " & _
                        "and additional tables are required." & vbCrLf & _
                        "Do you want to add them automatically?", _
                        "WinHSPF - Control Card Query", "+&OK", "Cancel")
        If i = 1 Then
          Call CheckAndAddMissingTables("PERLND")
        End If
      End If
    End If
    If iChange Then
      If AnyMissingTables("IMPLND") Then
        i = myMsgBox.Show("Changes have been made to the IMPLND control cards, " & _
                          "and additional tables are required." & vbCrLf & _
                          "Do you want to add them automatically?", _
                          "WinHSPF - Control Card Query", "+&OK", "Cancel")
        If i = 1 Then
          Call CheckAndAddMissingTables("IMPLND")
        End If
      End If
    End If
    If rChange Then
      If AnyMissingTables("RCHRES") Then
        i = myMsgBox.Show("Changes have been made to the RCHRES control cards, " & _
                          "and additional tables are required." & vbCrLf & _
                          "Do you want to add them automatically?", _
                          "WinHSPF - Control Card Query", "+&OK", "Cancel")
        If i = 1 Then
          Call CheckAndAddMissingTables("RCHRES")
          Call UpdateFlagDependencies("RCHRES")
        End If
      End If
    End If
    If iChange Or rChange Or pChange Then
      Call SetMissingValuesToDefaults(myUci, defUci)
    End If
    
    myUci.Edited = True
  ElseIf iChange Or rChange Or pChange Then
    i = myMsgBox.Show("Changes have been made to Control Cards." & vbCrLf & _
                      "Discard them?", "WinHSPF Control Discard", "&Yes", "-+&No")
    If i = 2 Then 'no discard
      Exit Sub
    End If
  End If
  Unload Me
End Sub

Private Sub Form_Load()
  Dim i&, s$, h&, ctemp$, bpos&

  SSTabPIR.Tab = 0
  pChangedUponLoading = False
  iChangedUponLoading = False
  rChangedUponLoading = False
  
  If myUci.OpnBlks("PERLND").Count > 0 Then
    SSTabPIR.TabEnabled(0) = True
    With myUci.OpnBlks("PERLND").tables("ACTIVITY")
      'h = 1 + (.Parms.Count / 2)
      h = .Parms.Count + 1
      For i = 1 To .Parms.Count
        If i > 1 Then
          load chkPer(i - 1)
          chkPer(i - 1).Top = chkPer(i - 2).Top + chkPer(i - 2).height
          If i >= h Then
            If i = h Then chkPer(i - 1).Top = chkPer(0).Top
            chkPer(i - 1).Left = chkPer(0).Left + chkPer(0).width
          End If
          chkPer(i - 1).Visible = True
        End If
        ctemp = Mid(.Parms(i).Def.define, 31, 6)
        bpos = InStr(1, ctemp, " ")
        If bpos > 0 Then
          ctemp = Mid(ctemp, 1, bpos)
        End If
        ctemp = Trim(ctemp)
        
        Select Case ctemp
          Case "ATEMP":
            ctemp = ctemp & " - Air Temperature Elevation Difference"
          Case "SNOW":
            ctemp = ctemp & " - Accumulation and Melting of Snow and Ice"
          Case "PWATER":
            ctemp = ctemp & " - Water Budget Pervious"
          Case "SEDMNT":
            ctemp = ctemp & " - Production and Removal of Sediment"
          Case "PSTEMP":
            ctemp = ctemp & " - Soil Temperatures"
          Case "PWTGAS":
            ctemp = ctemp & " - Water Temperature and Dissolved Gas Concentrations"
          Case "PQUAL":
            ctemp = ctemp & " - Quality Constituents Using Simple Relationships"
          Case "MSTLAY":
            ctemp = ctemp & " - Moisture Content of Soil Layers"
          Case "PEST":
            ctemp = ctemp & " - Pesticide Behavior"
          Case "NITR":
            ctemp = ctemp & " - Nitrogen Behavior"
          Case "PHOS":
            ctemp = ctemp & " - Phosphorous Behavior"
          Case "TRACER":
            ctemp = ctemp & " - Movement of a Tracer"
        End Select
        
        chkPer(i - 1).Caption = ctemp
        s = .Parms(i).Def.define
        chkPer(i - 1).tooltiptext = "Check" & Right(s, Len(s) - Len("Value of 1 "))
        chkPer(i - 1).Value = .Parms(i).Value
      Next i
    End With
  Else 'no perlnd
    SSTabPIR.TabEnabled(0) = False
  End If
  
  If myUci.OpnBlks("IMPLND").Count > 0 Then
    SSTabPIR.TabEnabled(1) = True
    With myUci.OpnBlks("IMPLND").tables("ACTIVITY")
      'h = 1 + (.parms.Count / 2)
      h = .Parms.Count + 1
      For i = 1 To .Parms.Count
        If i > 1 Then
          load chkImp(i - 1)
          chkImp(i - 1).Top = chkImp(i - 2).Top + chkImp(i - 2).height
          If i >= h Then
            If i = h Then chkImp(i - 1).Top = chkImp(0).Top
            chkImp(i - 1).Left = chkImp(0).Left + chkImp(0).width
          End If
          chkImp(i - 1).Visible = True
        End If
        ctemp = Mid(.Parms(i).Def.define, 31, 6)
        bpos = InStr(1, ctemp, " ")
        If bpos > 0 Then
          ctemp = Mid(ctemp, 1, bpos)
        End If
        ctemp = Trim(ctemp)
        
        Select Case ctemp
          Case "ATEMP":
            ctemp = ctemp & " - Air Temperature Elevation Difference"
          Case "SNOW":
            ctemp = ctemp & " - Accumulation and Melting of Snow and Ice"
          Case "IWATER":
            ctemp = ctemp & " - Water Budget Impervious"
          Case "SOLIDS":
            ctemp = ctemp & " - Accumulation and Removal of Solids"
          Case "IWTGAS":
            ctemp = ctemp & " - Water Temperature and Dissolved Gas Concentrations"
          Case "IQUAL":
            ctemp = ctemp & " - Quality Constituents Using Simple Relationships"
        End Select
        
        chkImp(i - 1).Caption = ctemp
        s = .Parms(i).Def.define
        chkImp(i - 1).tooltiptext = "Check" & Right(s, Len(s) - Len("Value of 1 "))
        chkImp(i - 1).Value = .Parms(i).Value
      Next i
    End With
  Else ' no implnd
    SSTabPIR.TabEnabled(1) = False
  End If
  
  If myUci.OpnBlks("RCHRES").Count > 0 Then
    SSTabPIR.TabEnabled(2) = True
    With myUci.OpnBlks("RCHRES").tables("ACTIVITY")
      'h = 1 + (.parms.Count / 2)
      h = .Parms.Count + 1
      For i = 1 To .Parms.Count
        If i > 1 Then
          load chkRch(i - 1)
          chkRch(i - 1).Top = chkRch(i - 2).Top + chkRch(i - 2).height
          If i >= h Then
            If i = h Then chkRch(i - 1).Top = chkRch(0).Top
            chkRch(i - 1).Left = chkRch(0).Left + chkRch(0).width
          End If
          chkRch(i - 1).Visible = True
        End If
        ctemp = Mid(.Parms(i).Def.define, 31, 6)
        bpos = InStr(1, ctemp, " ")
        If bpos > 0 Then
          ctemp = Mid(ctemp, 1, bpos)
        End If
        ctemp = Trim(ctemp)
        
        Select Case ctemp
          Case "HYDR":
            ctemp = ctemp & " - Hydraulic Behavior"
          Case "ADCALC":
            ctemp = ctemp & " - Advection of Fully Entrained Constituents"
          Case "CONS":
            ctemp = ctemp & " - Conservative Constituents"
          Case "HTRCH":
            ctemp = ctemp & " - Heat Exchange and Water Temperature"
          Case "SEDTRN":
            ctemp = ctemp & " - Behavior of Inorganic Sediment"
          Case "GQUAL":
            ctemp = ctemp & " - Generalized Quality Constituents"
          Case "OXRX":
            ctemp = ctemp & " - Primary DO and BOD Balances"
          Case "NUTRX":
            ctemp = ctemp & " - Primary Inorganic Nitrogen and Phosphorus Balances"
          Case "PLANK":
            ctemp = ctemp & " - Plankton Populations and Associated Reactions"
          Case "PHCARB":
            ctemp = ctemp & " - pH, Carbon Dioxide, Total Inorganic Carbon, and Alkalinity"
        End Select
        
        chkRch(i - 1).Caption = ctemp
        s = .Parms(i).Def.define
        chkRch(i - 1).tooltiptext = "Check" & Right(s, Len(s) - Len("Value of 1 "))
        chkRch(i - 1).Value = .Parms(i).Value
      Next i
    End With
  Else 'no rchres
    SSTabPIR.TabEnabled(2) = False
  End If
  
  If iChangedUponLoading Then
    iChange = True
  Else
    iChange = False
  End If
  If pChangedUponLoading Then
    pChange = True
  Else
    pChange = False
  End If
  If rChangedUponLoading Then
    rChange = True
  Else
    rChange = False
  End If
End Sub

Private Sub CheckIChange(Index As Integer)
  Select Case Index
    Case 0:              '0 - atemp
    Case 1:              '1 - snow
      If chkImp(0) <> 1 Then
        chkImp(0) = 1
        iChangedUponLoading = True
      End If
    Case 2:              '2 - iwater
    Case 3:              '3 - solids
      If chkImp(2) <> 1 Then
        chkImp(2) = 1
        iChangedUponLoading = True
      End If
    Case 4:              '4 - iwtgas
      If chkImp(0) <> 1 Then
        chkImp(0) = 1
        iChangedUponLoading = True
      End If
      If chkImp(2) <> 1 Then
        chkImp(2) = 1
        iChangedUponLoading = True
      End If
    Case 5:              '5 - iqual
      If chkImp(2) <> 1 Then
        chkImp(2) = 1
        iChangedUponLoading = True
      End If
      If chkImp(3) <> 1 Then
        chkImp(3) = 1
        iChangedUponLoading = True
      End If
  End Select
End Sub

Private Sub CheckPChange(Index As Integer)
  Select Case Index
    Case 0:              '0 - atemp
    Case 1:              '1 - snow
      If chkPer(0) <> 1 Then
        chkPer(0) = 1
        pChangedUponLoading = True
      End If
    Case 2:              '2 - pwater
    Case 3:              '3 - sedmnt
      If chkPer(2) <> 1 Then
        chkPer(2) = 1
        pChangedUponLoading = True
      End If
    Case 4:              '4 - pstemp
      If chkPer(0) <> 1 Then
        chkPer(0) = 1
        pChangedUponLoading = True
      End If
    Case 5:              '5 - pwtgas
      If chkPer(0) <> 1 Then
        chkPer(0) = 1
        pChangedUponLoading = True
      End If
      If chkPer(2) <> 1 Then
        chkPer(2) = 1
        pChangedUponLoading = True
      End If
      If chkPer(4) <> 1 Then
        chkPer(4) = 1
        pChangedUponLoading = True
      End If
    Case 6:              '6 - pqual
      If chkPer(2) <> 1 Then
        chkPer(2) = 1
        pChangedUponLoading = True
      End If
      If chkPer(3) <> 1 Then
        chkPer(3) = 1
        pChangedUponLoading = True
      End If
    Case 7:              '7 - mstlay
    Case 8:              '8 - pest
      If chkPer(2) <> 1 Then
        chkPer(2) = 1
        pChangedUponLoading = True
      End If
      If chkPer(3) <> 1 Then
        chkPer(3) = 1
        pChangedUponLoading = True
      End If
      If chkPer(7) <> 1 Then
        chkPer(7) = 1
        pChangedUponLoading = True
      End If
    Case 9:              '9 - nitr
      If chkPer(0) <> 1 Then
        chkPer(0) = 1
        pChangedUponLoading = True
      End If
      If chkPer(2) <> 1 Then
        chkPer(2) = 1
        pChangedUponLoading = True
      End If
      If chkPer(3) <> 1 Then
        chkPer(3) = 1
        pChangedUponLoading = True
      End If
      If chkPer(4) <> 1 Then
        chkPer(4) = 1
        pChangedUponLoading = True
      End If
      If chkPer(7) <> 1 Then
        chkPer(7) = 1
        pChangedUponLoading = True
      End If
    Case 10:             '10 - phos
      If chkPer(0) <> 1 Then
        chkPer(0) = 1
        pChangedUponLoading = True
      End If
      If chkPer(2) <> 1 Then
        chkPer(2) = 1
        pChangedUponLoading = True
      End If
      If chkPer(3) <> 1 Then
        chkPer(3) = 1
        pChangedUponLoading = True
      End If
      If chkPer(4) <> 1 Then
        chkPer(4) = 1
        pChangedUponLoading = True
      End If
      If chkPer(7) <> 1 Then
        chkPer(7) = 1
        pChangedUponLoading = True
      End If
    Case 11:             '11 - tracer
      If chkPer(2) <> 1 Then
        chkPer(2) = 1
        pChangedUponLoading = True
      End If
      If chkPer(7) <> 1 Then
        chkPer(7) = 1
        pChangedUponLoading = True
      End If
  End Select
End Sub

Private Sub CheckRChange(Index As Integer)
  Select Case Index
    Case 0:              '0 - hydr
    Case 1:              '1 - adcalc
      If chkRch(0) <> 1 Then
        chkRch(0) = 1
        rChangedUponLoading = True
      End If
    Case 2:              '2 - cons
      If chkRch(0) <> 1 Then
        chkRch(0) = 1
        rChangedUponLoading = True
      End If
      If chkRch(1) <> 1 Then
        chkRch(1) = 1
        rChangedUponLoading = True
      End If
    Case 3:              '3 - htrch
      If chkRch(0) <> 1 Then
        chkRch(0) = 1
        rChangedUponLoading = True
      End If
      If chkRch(1) <> 1 Then
        chkRch(1) = 1
        rChangedUponLoading = True
      End If
    Case 4:              '4 - sedtrn
      If chkRch(0) <> 1 Then
        chkRch(0) = 1
        rChangedUponLoading = True
      End If
      If chkRch(1) <> 1 Then
        chkRch(1) = 1
        rChangedUponLoading = True
      End If
      If chkRch(3) <> 1 Then
        chkRch(3) = 1
        rChangedUponLoading = True
      End If
    Case 5:              '5 - gqual
      If chkRch(0) <> 1 Then
        chkRch(0) = 1
        rChangedUponLoading = True
      End If
      If chkRch(1) <> 1 Then
        chkRch(1) = 1
        rChangedUponLoading = True
      End If
      If chkRch(3) <> 1 Then
        chkRch(3) = 1
        rChangedUponLoading = True
      End If
      If chkRch(4) <> 1 Then
        chkRch(4) = 1
        rChangedUponLoading = True
      End If
    Case 6:              '6 - oxrx
      If chkRch(0) <> 1 Then
        chkRch(0) = 1
        rChangedUponLoading = True
      End If
      If chkRch(1) <> 1 Then
        chkRch(1) = 1
        rChangedUponLoading = True
      End If
      If chkRch(3) <> 1 Then
        chkRch(3) = 1
        rChangedUponLoading = True
      End If
    Case 7:              '7 - nutrx
      If chkRch(0) <> 1 Then
        chkRch(0) = 1
        rChangedUponLoading = True
      End If
      If chkRch(1) <> 1 Then
        chkRch(1) = 1
        rChangedUponLoading = True
      End If
      If chkRch(3) <> 1 Then
        chkRch(3) = 1
        rChangedUponLoading = True
      End If
      If chkRch(4) <> 1 Then
        chkRch(4) = 1
        rChangedUponLoading = True
      End If
      If chkRch(6) <> 1 Then
        chkRch(6) = 1
        rChangedUponLoading = True
      End If
    Case 8:              '8 - plank
      If chkRch(0) <> 1 Then
        chkRch(0) = 1
        rChangedUponLoading = True
      End If
      If chkRch(1) <> 1 Then
        chkRch(1) = 1
        rChangedUponLoading = True
      End If
      If chkRch(3) <> 1 Then
        chkRch(3) = 1
        rChangedUponLoading = True
      End If
      If chkRch(4) <> 1 Then
        chkRch(4) = 1
        rChangedUponLoading = True
      End If
      If chkRch(6) <> 1 Then
        chkRch(6) = 1
        rChangedUponLoading = True
      End If
      If chkRch(7) <> 1 Then
        chkRch(7) = 1
        rChangedUponLoading = True
      End If
    Case 9:              '9 - phcarb
      If chkRch(0) <> 1 Then
        chkRch(0) = 1
        rChangedUponLoading = True
      End If
      If chkRch(1) <> 1 Then
        chkRch(1) = 1
        rChangedUponLoading = True
      End If
      If chkRch(2) <> 1 Then
        chkRch(2) = 1
        rChangedUponLoading = True
      End If
      If chkRch(3) <> 1 Then
        chkRch(3) = 1
        rChangedUponLoading = True
      End If
      If chkRch(4) <> 1 Then
        chkRch(4) = 1
        rChangedUponLoading = True
      End If
      If chkRch(6) <> 1 Then
        chkRch(6) = 1
        rChangedUponLoading = True
      End If
      If chkRch(7) <> 1 Then
        chkRch(7) = 1
        rChangedUponLoading = True
      End If
      If chkRch(8) <> 1 Then
        chkRch(8) = 1
        rChangedUponLoading = True
      End If
  End Select
End Sub

Private Sub Form_Resize()
  Dim i&
  If width > 500 And height > 1900 Then
    cmdControl(0).Top = height - cmdControl(0).height - 600
    cmdControl(1).Top = cmdControl(0).Top
    SSTabPIR.height = cmdControl(0).Top - 500
    SSTabPIR.width = width - 500
    fraControl.width = width
    fraControl.height = height
    fraImplnd.width = SSTabPIR.width - 200
    fraImplnd.height = SSTabPIR.height - 400
    fraPerlnd.width = SSTabPIR.width - 200
    fraPerlnd.height = SSTabPIR.height - 400
    fraRchres.width = SSTabPIR.width - 200
    fraRchres.height = SSTabPIR.height - 400
    For i = 1 To chkImp.UBound
      chkImp(i).width = SSTabPIR.width - 400
    Next i
    For i = 1 To chkPer.UBound
      chkPer(i).width = SSTabPIR.width - 400
    Next i
    For i = 1 To chkRch.UBound
      chkRch(i).width = SSTabPIR.width - 400
    Next i
    cmdControl(0).Left = width / 2 - cmdControl(0).width - 200
    cmdControl(1).Left = width / 2 + 200
    
  End If
End Sub

