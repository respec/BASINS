VERSION 5.00
Begin VB.UserControl ATCoDate 
   BackColor       =   &H8000000B&
   ClientHeight    =   1356
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   6348
   ScaleHeight     =   1356
   ScaleWidth      =   6348
   ToolboxBitmap   =   "ATCoDate.ctx":0000
   Begin VB.Frame fraDates 
      Caption         =   "Dates"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1332
      Left            =   0
      TabIndex        =   16
      Top             =   0
      Width           =   6315
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   23
         Left            =   4580
         Locked          =   -1  'True
         TabIndex        =   35
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   22
         Left            =   4320
         Locked          =   -1  'True
         TabIndex        =   34
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   21
         Left            =   4060
         Locked          =   -1  'True
         TabIndex        =   33
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   20
         Left            =   3800
         Locked          =   -1  'True
         TabIndex        =   32
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   19
         Left            =   3540
         Locked          =   -1  'True
         TabIndex        =   31
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   17
         Left            =   2440
         Locked          =   -1  'True
         TabIndex        =   29
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   16
         Left            =   2180
         Locked          =   -1  'True
         TabIndex        =   28
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   15
         Left            =   1920
         Locked          =   -1  'True
         TabIndex        =   27
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   14
         Left            =   1660
         Locked          =   -1  'True
         TabIndex        =   26
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   13
         Left            =   1400
         Locked          =   -1  'True
         TabIndex        =   25
         TabStop         =   0   'False
         Text            =   "00"
         Top             =   900
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   11
         Left            =   4580
         MaxLength       =   2
         TabIndex        =   12
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   10
         Left            =   4320
         MaxLength       =   2
         TabIndex        =   11
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   9
         Left            =   4060
         MaxLength       =   2
         TabIndex        =   10
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   8
         Left            =   3800
         MaxLength       =   2
         TabIndex        =   9
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   7
         Left            =   3540
         MaxLength       =   2
         TabIndex        =   8
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   5
         Left            =   2440
         MaxLength       =   2
         TabIndex        =   5
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   4
         Left            =   2180
         MaxLength       =   2
         TabIndex        =   4
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   3
         Left            =   1920
         MaxLength       =   2
         TabIndex        =   3
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   2
         Left            =   1660
         MaxLength       =   2
         TabIndex        =   2
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   1
         Left            =   1400
         MaxLength       =   2
         TabIndex        =   1
         Text            =   "00"
         Top             =   540
         Width           =   272
      End
      Begin VB.CommandButton cmdCommLimt 
         Caption         =   "Common"
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
         Left            =   100
         TabIndex        =   36
         Top             =   920
         Width           =   860
      End
      Begin VB.VScrollBar VScroll1 
         Height          =   312
         LargeChange     =   6
         Left            =   2760
         Max             =   6
         Min             =   -6
         TabIndex        =   6
         Top             =   180
         Visible         =   0   'False
         Width           =   252
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   18
         Left            =   3100
         Locked          =   -1  'True
         TabIndex        =   30
         TabStop         =   0   'False
         Text            =   "1997"
         Top             =   900
         Width           =   452
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000000&
         Height          =   288
         Index           =   12
         Left            =   960
         Locked          =   -1  'True
         TabIndex        =   24
         TabStop         =   0   'False
         Text            =   "1997"
         Top             =   900
         Width           =   452
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   6
         Left            =   3100
         MaxLength       =   4
         TabIndex        =   7
         Text            =   "1997"
         Top             =   540
         Width           =   452
      End
      Begin VB.TextBox txtDat 
         Alignment       =   1  'Right Justify
         Height          =   288
         Index           =   0
         Left            =   960
         MaxLength       =   4
         TabIndex        =   0
         Text            =   "1997"
         Top             =   540
         Width           =   452
      End
      Begin VB.ComboBox lstAggr 
         Height          =   315
         ItemData        =   "ATCoDate.ctx":0312
         Left            =   4960
         List            =   "ATCoDate.ctx":0322
         Style           =   2  'Dropdown List
         TabIndex        =   15
         Top             =   900
         Width           =   1212
      End
      Begin VB.ComboBox lstTunits 
         Height          =   288
         ItemData        =   "ATCoDate.ctx":0344
         Left            =   5380
         List            =   "ATCoDate.ctx":035A
         Style           =   2  'Dropdown List
         TabIndex        =   14
         Top             =   540
         Width           =   792
      End
      Begin VB.TextBox txtTstep 
         Alignment       =   1  'Right Justify
         Height          =   288
         Left            =   4960
         TabIndex        =   13
         Text            =   "1"
         Top             =   540
         Width           =   432
      End
      Begin VB.CommandButton cmdReset 
         Caption         =   "Reset"
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
         TabIndex        =   20
         Top             =   240
         Width           =   732
      End
      Begin VB.Label lblDate 
         BackStyle       =   0  'Transparent
         Caption         =   "End"
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
         Index           =   3
         Left            =   3120
         TabIndex        =   17
         Top             =   240
         WhatsThisHelpID =   16
         Width           =   735
         WordWrap        =   -1  'True
      End
      Begin VB.Label lblDate 
         Alignment       =   1  'Right Justify
         Caption         =   "to"
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
         Index           =   6
         Left            =   2740
         TabIndex        =   23
         Top             =   960
         WhatsThisHelpID =   16
         Width           =   252
         WordWrap        =   -1  'True
      End
      Begin VB.Label lblDate 
         Alignment       =   1  'Right Justify
         Caption         =   "to"
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
         Index           =   5
         Left            =   2740
         TabIndex        =   22
         Top             =   600
         WhatsThisHelpID =   16
         Width           =   252
         WordWrap        =   -1  'True
      End
      Begin VB.Label lblDate 
         BackStyle       =   0  'Transparent
         Caption         =   "TStep,Units"
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
         Index           =   4
         Left            =   4960
         TabIndex        =   21
         Top             =   240
         WhatsThisHelpID =   16
         Width           =   1095
         WordWrap        =   -1  'True
      End
      Begin VB.Label lblDate 
         BackStyle       =   0  'Transparent
         Caption         =   "To Graph"
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
         Index           =   1
         Left            =   120
         TabIndex        =   19
         Top             =   600
         WhatsThisHelpID =   16
         Width           =   852
         WordWrap        =   -1  'True
      End
      Begin VB.Label lblDate 
         BackStyle       =   0  'Transparent
         Caption         =   "Start"
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
         Index           =   2
         Left            =   960
         TabIndex        =   18
         Top             =   240
         WhatsThisHelpID =   16
         Width           =   495
         WordWrap        =   -1  'True
      End
   End
End
Attribute VB_Name = "ATCoDate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants
    
Private DispLast&, DispTstep As Boolean
'Private DateFmtYr%, DateFmtMon%, DateFmtDisp$ 'not in use - future public
Private txtCurr% ', txtChanged As Boolean, txtOld$, txtTmp$ ', txtFlush As Boolean
Private pCommS As Date, pCommE As Date
Private pLimtS As Date, pLimtE As Date

Public Event Change()

'0-5 are Start, 6-11 are End, see Select below for other controls
Public Sub SetControlFocus(ByVal Control&)
  If Control >= 0 And Control <= 11 Then
    If txtDat(Control).Visible Then
      txtDat(Control).SetFocus
    ElseIf Control > 0 Then
      SetControlFocus Control - 1
    End If
  Else
    Select Case Control
      Case -1: If cmdReset.Visible Then cmdReset.SetFocus
      Case -2: If VScroll1.Visible Then VScroll1.SetFocus
      Case -3: If txtTstep.Visible Then txtTstep.SetFocus Else SetControlFocus -5
      Case -4: If lstTunits.Visible Then lstTunits.SetFocus Else SetControlFocus -5
      Case -5: If lstAggr.Visible Then lstAggr.SetFocus
    End Select
  End If
End Sub

Public Property Get TstepVisible() As Boolean
  TstepVisible = DispTstep
End Property

Public Property Let TstepVisible(ByVal NewValue As Boolean)
  Dim W&
  DispTstep = NewValue
  txtTstep.Visible = NewValue
  lstAggr.Visible = NewValue
  lstTunits.Visible = NewValue
  lblDate(4).Visible = NewValue
  W = MinWidth
  If UserControl.Width < W Then UserControl.Width = W
End Property

Public Property Get Caption() As String
    Caption = fraDates.Caption
    DbgMsg "Caption:Get:", 9, "ATCoDate", "o"
End Property
Public Property Let Caption(ByVal NewValue As String)
    fraDates.Caption = NewValue
    DbgMsg "Caption:Let:", 7, "ATCoDate", "o"
End Property

Private Sub OrderCurrSE()
    Dim tb& 'textbox index
    Dim checkCurrS As Date, checkCurrE As Date
    checkCurrS = MakeDate(0)
    checkCurrE = MakeDate(6)
    If checkCurrE < checkCurrS Then 'move CurrE to LimtE
      checkCurrE = MakeDate(18)
      For tb = 6 To 11
        txtDat(tb) = txtDat(tb + 12)
      Next
      If checkCurrE < checkCurrS Then 'move CurrS to LimtS
        checkCurrS = MakeDate(12)
        For tb = 0 To 5
          txtDat(tb) = txtDat(tb + 12)
        Next
      End If
    End If
    'If checkCurrE < checkCurrS Then MsgBox "Start date specified was after end date.", vbExclamation, "Date Problem"
End Sub

Public Property Get CurrE() As Date
  Dim retval As Date
  OrderCurrSE
  retval = MakeDate(6)
  CurrE = retval
  DbgMsg "CurrE:Get:" & retval, 9, "ATCoDate", "p"
End Property
Public Property Let CurrE(ByVal NewValue As Date)
    Call refreshDate(6, NewValue)
    DbgMsg "CurrE:Let:" & NewValue, 9, "ATCoDate", "p"
End Property

Public Property Get CurrS() As Date
  Dim retval As Date
  OrderCurrSE
  retval = MakeDate(0)
  DbgMsg "CurrS:Get:" & retval, 9, "ATCoDate", "p"
  CurrS = retval
End Property
Public Property Let CurrS(ByVal NewValue As Date)
  Call refreshDate(0, NewValue)
  DbgMsg "CurrS:Let:" & NewValue, 9, "ATCoDate", "p"
End Property

Public Property Get CommE() As Date
  DbgMsg "CommE:Get:" & MakeDate(18), 9, "ATCoDate", "p"
  CommE = pCommE 'MakeDate(18)
End Property
Public Property Let CommE(ByVal NewValue As Date)
  pCommE = NewValue
  If ShowingCommon Then Call refreshDate(18, NewValue)
  DbgMsg "CommE:Let:" & MakeDate(18), 9, "ATCoDate", "p"
End Property

Public Property Get CommS() As Date
  DbgMsg "CommS:Get:" & MakeDate(12), 9, "ATCoDate", "p"
  CommS = pCommS 'MakeDate(12)
End Property
Public Property Let CommS(ByVal NewValue As Date)
  pCommS = NewValue
  If ShowingCommon Then Call refreshDate(12, NewValue)
  DbgMsg "CommS:Get:" & MakeDate(12), 9, "ATCoDate", "p"
End Property

Public Property Get LimtE() As Date
  DbgMsg "LimtE:Get:" & MakeDate(18), 9, "ATCoDate", "p"
  LimtE = pLimtE 'MakeDate(18)
End Property
Public Property Let LimtE(ByVal NewValue As Date)
  pLimtE = NewValue
  If Not ShowingCommon Then Call refreshDate(18, NewValue)
  DbgMsg "LimtE:Let:" & MakeDate(18), 9, "ATCoDate", "p"
End Property

Public Property Get LimtS() As Date
  DbgMsg "LimtS:Get:" & MakeDate(12), 9, "ATCoDate", "p"
  LimtS = pLimtS 'MakeDate(12)
End Property
Public Property Let LimtS(ByVal NewValue As Date)
  pLimtS = NewValue
  If Not ShowingCommon Then Call refreshDate(12, NewValue)
  DbgMsg "LimtS:Get:" & MakeDate(12), 9, "ATCoDate", "p"
End Property

Public Property Get TUnit() As Integer
  TUnit = lstTunits.ListIndex + 1
End Property
Public Property Let TUnit(ByVal NewValue As Integer)
  If NewValue > 0 And NewValue < 7 Then
    lstTunits.ListIndex = NewValue - 1
  Else
    DbgMsg "TUnit:Let value not betw 1 & 6, is " & NewValue, 2, "ATCoDate", "e"
  End If
End Property

Public Property Get TNval() As Long
  Dim s$, i&
  i = lstTunits.ListIndex
  Select Case i
    Case 0: s = "s"
    Case 1: s = "n"
    Case 2: s = "h"
    Case 3: s = "d"
    Case 4: s = "m"
    Case 5: s = "yyyy"
  End Select
  DbgMsg "TNval: " & i & " " & s, 5, "ATCoDate", "t"
  TNval = CLng(DateDiff(s, CurrS, CurrE) / CInt(txtTstep))
End Property

Public Property Get TStep() As Integer
  TStep = CInt(txtTstep)
End Property
Public Property Let TStep(ByVal NewValue As Integer)
  If NewValue > 0 Then
    DbgMsg "TStep:Let New Value:" & NewValue & " Old: " & txtTstep, 6, "ATCoDate", "t"
    txtTstep = NewValue
  Else
    DbgMsg "TStep:Let value not > 0, is " & NewValue, 2, "ATCoDate", "e"
  End If
End Property

Public Property Get DispL() As Integer
  DispL = DispLast
  DbgMsg "DispL:Get " & DispLast, 9, "ATCoDate", "o"
End Property
Public Property Let DispL(ByVal NewValue As Integer)
  Dim f&, l&, c&, d&, p&, W&
  
  If NewValue > 0 And NewValue < 7 Then
    DispLast = NewValue
    f = 7 - NewValue
    l = 5
    DbgMsg "DispL:Let " & f & " " & l & " " & DispLast, 7, "ATCoDate", "t"
    For c = l To f Step -1
      txtDat(c).Visible = False
      txtDat(c + 6).Visible = False
      txtDat(c + 12).Visible = False
      txtDat(c + 18).Visible = False
    Next c
    For c = 0 To f - 1
      txtDat(c).Visible = True
      txtDat(c + 6).Visible = True
      txtDat(c + 12).Visible = True
      txtDat(c + 18).Visible = True
    Next c
    p = txtDat(f - 1).Left + txtDat(f - 1).Width + 108
    d = VScroll1.Left - p
    VScroll1.Left = p
    lblDate(5).Left = lblDate(5).Left - d
    lblDate(6).Left = lblDate(6).Left - d
    p = lblDate(3).Left - d
    lblDate(3).Left = p
    For c = 0 To 5
      txtDat(c + 6).Left = txtDat(c + 6).Left - d
      txtDat(c + 18).Left = txtDat(c + 18).Left - d
    Next c
    d = d * 2
    lblDate(4).Left = lblDate(4).Left - d
    txtTstep.Left = txtTstep.Left - d
    lstTunits.Left = lstTunits.Left - d
    lstAggr.Left = lstAggr.Left - d
    W = MinWidth
    If UserControl.Width < W Then UserControl.Width = W
    'w = lstTunits.Left + lstTunits.Width + 150
    'If w > UserControl.Width Then
    '  UserControl.Width = w
    'End If
  End If
End Property

Public Property Get MinWidth&()
  If TstepVisible Then
    MinWidth = lstTunits.Left + lstTunits.Width + 150
  Else
    MinWidth = txtDat(12 - DispL).Left + txtDat(12 - DispL).Width + 150
  End If
End Property

Public Property Get TAggr() As Integer
  TAggr = lstAggr.ListIndex
End Property
Public Property Let TAggr(ByVal NewValue As Integer)
  If NewValue >= 0 And NewValue < lstAggr.ListCount Then
    DbgMsg "TAggr:Let New Value:" & NewValue & " Old: " & lstAggr.ListIndex, 6, "ATCoDate", "t"
    lstAggr.ListIndex = NewValue
  Else
    DbgMsg "TAggr:Let value not between 0 & " & lstAggr.ListCount - 1 & ", is " & NewValue, 2, "ATCoDate", "e"
  End If
End Property

Public Property Get LabelCurrentRange() As String
  LabelCurrentRange = lblDate(1)
End Property
Public Property Let LabelCurrentRange(ByVal NewValue As String)
  DbgMsg "LabelCurrentRange:Let New Value:" & NewValue & " Old: " & lblDate(1), 6, "ATCoDate", "t"
  lblDate(1) = NewValue
End Property

Public Property Get LabelMaxRange() As String
  LabelMaxRange = cmdCommLimt.Caption
End Property
Public Property Let LabelMaxRange(ByVal NewValue As String)
  DbgMsg "LabelMaxRange:Let New Value:" & NewValue & " Old: " & lblDate(0), 6, "ATCoDate", "t"
  cmdCommLimt.Caption = NewValue
End Property

'Private Function daymon(y&, M&) As Integer
'  Dim d&
'
'  If M = 2 Then
'    d = 28
'    If y Mod 4 = 0 And y Mod 100 <> 0 Then
'      d = 29
'    End If
'  ElseIf M = 4 Or M = 6 Or M = 9 Or M = 11 Then
'    d = 30
'  Else
'    d = 31
'  End If
'  daymon = d
'End Function

Private Sub defaultDate(c As Date, l As Date, d As Date)
  If Not (IsDate(c)) Then
    If Not (IsDate(l)) Then
      l = d
    End If
    c = l
  Else
    If Not (IsDate(l)) Then
      l = c
    End If
  End If
End Sub

Public Sub refreshDateAll()
  Dim tb& 'textbox index
  For tb = 0 To 11
    txtDat(tb) = txtDat(tb + 12)
  Next
End Sub

Private Function MakeDate(b&) As Date
  MakeDate = DateSerial(txtDat(b), txtDat(b + 1), txtDat(b + 2))
  MakeDate = MakeDate + TimeSerial(txtDat(b + 3), txtDat(b + 4), txtDat(b + 5))
End Function

Private Sub refreshDate(base&, dateval As Date)
  txtDat(base) = Year(dateval)
  txtDat(base + 1) = Month(dateval)
  txtDat(base + 2) = Day(dateval)
  txtDat(base + 3) = Hour(dateval)
  txtDat(base + 4) = Minute(dateval)
  txtDat(base + 5) = Second(dateval)
End Sub

Private Function ShowingCommon() As Boolean
  If cmdCommLimt.Caption = "Common" Then ShowingCommon = True Else ShowingCommon = False
End Function

Private Sub cmdCommLimt_Click()
  If ShowingCommon Then
    cmdCommLimt.Caption = "All"
    Call refreshDate(12, pLimtS)
    Call refreshDate(18, pLimtE)
  Else
    cmdCommLimt.Caption = "Common"
    Call refreshDate(12, pCommS)
    Call refreshDate(18, pCommE)
  End If
End Sub

Private Sub cmdReset_Click()
  DbgMsg "Reset", 3, "ATCoDate", "m"
  Call refreshDateAll
End Sub

'Private Sub txtCheck(ByVal i&, ByVal incr&)  'check new value within a date field
'  Dim b&, t&, tmin&, tmax&, p&, DateTemp As Date
'
'  If i < 6 Then 'base for date
'    b = 0
'  Else
'    b = 6
'  End If
'  p = 1 + (i Mod 6)
'
'  t = CInt(txtDat(i)) + incr
'
'  tmin = 0
'  If p = 2 Or p = 3 Then 'month or day
'    tmin = 1
'  ElseIf p = 1 Then 'year
'    tmin = txtDat(12)
'  End If
'  If t < tmin Then
'    DbgMsg "Txt<Min " & t & " < " & tmin & " index:" & i, 3, "ATCoDate", "e"
'    t = tmin
'  End If
'
'  tmax = 60
'  If p = 4 Then 'hour
'    tmax = 24
'  ElseIf p = 3 Then 'day
'    tmax = daymon(CInt(txtDat(b)), CInt(txtDat(b + 1)))
'  ElseIf p = 2 Then 'month
'    tmax = 12
'  ElseIf p = 1 Then 'year
'    tmax = txtDat(18)
'  End If
'  If t > tmax Then
'    DbgMsg "Txt>Max " & t & " > " & tmax & " index:" & i, 3, "ATCoDate", "e"
'    t = tmax
'  End If
'
'  txtDat(i) = t
'
'  DateTemp = DateSerial(txtDat(b), txtDat(b + 1), txtDat(b + 2))
'  DateTemp = DateTemp + TimeSerial(txtDat(b + 3), txtDat(b + 4), txtDat(b + 5))
'  If b = 0 Then  'start date
'    If DateTemp < LimtS Then
'      DbgMsg "StartDate b4 MinDate", 3, "ATCoDate", "e"
'      txtDat(i) = txtOld
'    ElseIf DateTemp > MakeDate(6) Then
'      DbgMsg "StartDate aft MaxDate", 3, "ATCoDate", "e"
'      If i < 3 Then
'        txtDat(0) = txtDat(6)
'        txtDat(1) = txtDat(7)
'        txtDat(2) = txtDat(8)
'      Else
'        txtDat(i) = txtOld
'      End If
'    End If
'  Else 'end date
'    If DateTemp > LimtE Then
'      DbgMsg "EndDate aft MaxDate", 3, "ATCoDate", "e"
'      txtDat(i) = txtOld
'    ElseIf DateTemp < MakeDate(0) Then
'      DbgMsg "EndDate b4 StartDate", 3, "ATCoDate", "e"
'      If i < 9 Then
'        txtDat(6) = txtDat(0)
'        txtDat(7) = txtDat(1)
'        txtDat(8) = txtDat(2)
'      Else
'        txtDat(i) = txtOld
'      End If
'    End If
'  End If
'  If txtDat(i) <> txtOld Then 'change
'    If p = 1 Then 'year
'      If txtDat(i + 1) = 2 Then 'feb
'        If txtDat(i + 2) > 28 Then
'          txtDat(i + 2) = daymon(txtDat(i), txtDat(i + 1))
'        End If
'      End If
'    ElseIf p = 2 Then 'month
'      If txtDat(i + 1) > 27 Then 'may have too many days
'        txtDat(i + 1) = daymon(txtDat(i - 1), txtDat(i))
'      End If
'    End If
'  End If
'End Sub

'Private Sub txtDateNextField(Index)
'  Dim i&
'  If Index < 11 Then
'    i = 1
'    While Not (txtDat(Index + i).Visible)
'      i = i + 1
'    Wend
'    If Index + i < 12 Then
'      txtDat(Index + i).SetFocus
'    ElseIf txtTstep.Visible Then
'      txtTstep.SetFocus
'    ElseIf lstAggr.Visible Then
'      lstAggr.SetFocus
'    ElseIf lstTunits.Visible Then
'      lstTunits.SetFocus
'    End If
'  End If
'End Sub

Private Sub SelectText(txtBox As TextBox)
  'Debug.Print "i", Len(txtBox.Text), txtBox.SelLength
  If txtBox.SelLength < Len(txtBox.Text) Then
    txtBox.SelStart = 0
    txtBox.SelLength = Len(txtBox.Text)
  End If
  'Debug.Print "o", Len(txtBox.Text), txtBox.SelLength
End Sub

Private Sub lstAggr_Click()
  If lstAggr.Text = "Native" Then
    txtTstep.Visible = False
    lstTunits.Visible = False
    lblDate(4).Enabled = False
  Else
    txtTstep.Visible = lstAggr.Visible
    lstTunits.Visible = lstAggr.Visible
    lblDate(4).Enabled = True
  End If
  RaiseEvent Change
End Sub

Private Sub lstTunits_Click()
  RaiseEvent Change
End Sub

Private Sub txtDat_Change(Index As Integer)
  DbgMsg "txtDat Change(" & Index & ") = " & txtDat(Index), 7, "ATCoDate", "t"
'  If Len(txtDat(Index)) > 0 Then
'    If Not (IsNumeric(txtDat(Index))) Then
'      DbgMsg "Txt not Numeric", 3, "ATCoDate", "e"
'      txtDat(Index) = txtOld
'      txtChanged = False
'    End If
'  End If
  RaiseEvent Change
End Sub

Private Sub txtDat_GotFocus(Index As Integer)
  DbgMsg "txtDat:GotFocus:index: " & CStr(Index), 7, "ATCoDate", "f"
  If txtDat(Index).Visible Then
    VScroll1.Visible = True
    SelectText txtDat(Index)
  End If
  'txtChanged = False
  txtCurr = Index
End Sub

'Private Sub txtDat_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
'  Dim incr&, i&, n&
'
'  DbgMsg "txtDat:KeyDown:index: " & CStr(Index) & " KeyCode: " & CStr(KeyCode), 7, "ATCoDate", "k"
'  txtTmp = txtDat(Index)
''  txtFlush = False
'
'  If Index < 12 Then
'    If Not (txtChanged) Then
'      txtOld = txtDat(Index)
'    End If
'
'    If KeyCode >= vbKey0 And KeyCode <= vbKey9 Then
'      DbgMsg "txtDat:KeyDown: " & KeyCode & ":" & Len(txtDat(Index)) & ":" & txtDat(Index) & ":" & txtChanged, 6, "ATCoDate", "k"
''      If Not (txtChanged) Then
''        DbgMsg "txtDat:KeyDown:txtChanged set to TRUE", 7, "ATCoDate", "t"
''        txtDat(Index) = ""
'        txtChanged = True
''      End If
'    Else
'      DbgMsg "txtDat:KeyDown:non#key, finish change " & txtChanged, 8, "ATCoDate", "t"
'      If txtChanged Then
'        Call txtDat_LostFocus(Index)
'      End If
'      Select Case KeyCode
'        Case vbKeyDown:        incr = -1
'        Case vbKeySubtract:    incr = -1
'        Case vbKeyUp:          incr = 1
'        Case vbKeyAdd:         incr = 1
'        Case vbKeyPageUp:      incr = 6
'        Case vbKeyPageDown:    incr = -6
'        Case vbKeyDivide, 191:
'          'If txtChanged Then
'            Call txtDateNextField(Index)
'          'End If
'          'txtFlush = True
'        Case Else
'          If KeyCode <> vbKeyShift Then
'            DbgMsg "txtDat:KeyDown:unknown:" & KeyCode, 3, "ATCoDate", "k"
''            txtFlush = True
'          End If
'      End Select
'      If incr <> 0 Then  'Not txtFlush And
'        If Abs(incr) = 0 And Index Mod 6 = 0 Then
'          incr = incr * 1.66666 ' 10 years
'        End If
'        Call txtCheck(Index, incr) 'check incremental change
'      End If
'    End If
'  Else 'cant edit
''    txtFlush = True
'  End If
'End Sub

'Private Sub txtDat_KeyUp(Index As Integer, KeyCode As Integer, Shift As Integer)
'  DbgMsg "txtDat:KeyUp:flush " & txtFlush, 7, "ATCoDate", "k"
'  If txtFlush Then
'    txtDat(Index) = txtTmp
'    txtFlush = False
'  ElseIf KeyCode >= vbKey0 And KeyCode <= vbKey9 Then
'    If Len(txtDat(Index)) = txtDat(Index).MaxLength Then  ' this field is full
'      Call txtDateNextField(Index)
'    End If
'    DbgMsg "txtDat:KeyUp:" & KeyCode & ":" & Len(txtDat(Index)) & ":" & txtDat(Index), 5, "ATCoDate", "k"
'  End If
'End Sub

'Private Sub txtDat_LostFocus(Index As Integer)
'  Dim i&
'  DbgMsg "txtDat:LostFocus:index: " & CStr(Index) & " changed:" & txtChanged, 7, "ATCoDate", "i"
'  VScroll1.Visible = False
'  If txtChanged Then
'    If IsNumeric(txtDat(Index)) Then
'      i = CInt(txtDat(Index))
'      Call txtCheck(Index, 0)
'    Else
'      DbgMsg "txtDat:LostFocus:non # in date field", 2, "ATCoDate", "e"
'      txtDat(Index) = txtOld
'    End If
'  End If
'  txtChanged = False
'End Sub

'Private Sub txtDat_MouseDown(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
'  SelectText txtDat(Index)
'End Sub
'
'Private Sub txtDat_MouseMove(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
'  SelectText txtDat(Index)
'End Sub
'
'Private Sub txtDat_MouseUp(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
'  SelectText txtDat(Index)
'End Sub

Private Sub txtTstep_Change()
  RaiseEvent Change
End Sub

'Private Sub txtTstep_LostFocus()
'  DbgMsg "txtTstep:LostFocus: " & txtTstep, 8, "ATCoDate", "f"
'  If IsNumeric(txtTstep) Then
'    If txtTstep > 60 Then
'      txtTstep = 60
'    ElseIf txtTstep < 1 Then
'      txtTstep = 1
'    End If
'  Else
'    DbgMsg "txtTstep:LostFocus:non # in TStep field", 1, "ATCoDate", "e"
'    txtTstep = 1
'  End If
'End Sub

Private Sub UserControl_ReadProperties(PropBag As PropertyBag)
  Dim s$
  Let TUnit = PropBag.ReadProperty("TUnit", 4)
  Let TStep = PropBag.ReadProperty("TStep", 1)
  Let TAggr = PropBag.ReadProperty("TAggr", 1)
  Let CurrE = PropBag.ReadProperty("CurrE", CDate("12/31/1995"))
  Let CurrS = PropBag.ReadProperty("CurrS", CDate("1/1/1993"))
  Let LimtE = PropBag.ReadProperty("LimtE", CDate("12/31/1997"))
  Let LimtS = PropBag.ReadProperty("LimtS", CDate("1/1/1991"))
  Let DispL = PropBag.ReadProperty("DispL", 1)
  Let s = PropBag.ReadProperty("LabelCurrentRange", "Current")
  lblDate(1) = s
  'Let s = PropBag.ReadProperty("LabelMaxRange", "Available")
  'lblDate(0) = s
  Let TstepVisible = PropBag.ReadProperty("TstepVisible", True)
  DbgMsg "ReadProperties", 7, "ATCoDate", "p"
End Sub

Private Sub UserControl_Resize()
  Dim W&
  DbgMsg "Resize:WidIn: " & CStr(UserControl.Width), 7, "ATCoDate", "w"
  If UserControl.Height <> 1356 Then
    UserControl.Height = 1356
  End If
  W = MinWidth
  If UserControl.Width < W Then UserControl.Width = W
  
  fraDates.Width = UserControl.Width - 60
  DbgMsg "Resize:WidOut: " & CStr(UserControl.Width), 8, "ATCoDate", "w"
End Sub

Private Sub UserControl_Show()
  DbgMsg "Show", 7, "ATCoDate", "w"
  fraDates.Caption = Caption
  Call defaultDate(CurrS, LimtS, "1/1/1990")
  Call defaultDate(CurrE, LimtE, "12/31/2100 23:00:00")
  Call refreshDateAll
End Sub

Private Sub UserControl_Initialize()
  TAggr = 1
  TUnit = 4
  DispLast = 1
  Call refreshDate(0, CurrS)
  Call refreshDate(6, CurrE)
  Call refreshDate(12, LimtS)
  Call refreshDate(18, LimtE)
  lstAggr.Clear
  lstAggr.AddItem "Sum/Div":   lstAggr.ItemData(0) = 0
  lstAggr.AddItem "Aver/Same": lstAggr.ItemData(1) = 1
  lstAggr.AddItem "Max":       lstAggr.ItemData(2) = 2
  lstAggr.AddItem "Min":       lstAggr.ItemData(3) = 3
  lstAggr.AddItem "Native":    lstAggr.ItemData(4) = 4
  DbgMsg "Initialize", 8, "ATCoDate", "p"
End Sub

Private Sub UserControl_InitProperties()
   DbgMsg "InitProperties", 7, "ATCoDate", "p"
End Sub

Private Sub UserControl_WriteProperties(PropBag As PropertyBag)
   Dim s$
   PropBag.WriteProperty "TUnit", TUnit
   PropBag.WriteProperty "TAggr", TAggr
   PropBag.WriteProperty "TStep", TStep
   PropBag.WriteProperty "CurrE", CurrE
   PropBag.WriteProperty "CurrS", CurrS
   PropBag.WriteProperty "LimtE", LimtE
   PropBag.WriteProperty "LimtS", LimtS
   PropBag.WriteProperty "DispL", DispL
   s = lblDate(1)
   PropBag.WriteProperty "LabelCurrentRange", s
   's = lblDate(0)
   'PropBag.WriteProperty "LabelMaxRange", s
   PropBag.WriteProperty "TstepVisible", TstepVisible
   DbgMsg "WriteProperties", 8, "ATCoDate", "p"
End Sub

Private Sub VScroll1_Change()
  Dim oldval As Long, incr As Long

  incr = -VScroll1.Value
  If incr <> 0 Then
    If Not IsNumeric(txtDat(txtCurr).Text) Then
      txtDat(txtCurr).Text = "1"
    Else
      txtDat(txtCurr).Text = CLng(txtDat(txtCurr).Text) + incr
    End If
    VScroll1.Value = 0
  End If
'  Dim k%
'  With VScroll1
'    If .Value <> 0 Then
'      DbgMsg "VScroll: " & .Value, 8, "ATCoDate", "i"
'      If .Value < 0 Then
'        If Abs(.Value) = .LargeChange Then
'          k = vbKeyPageUp
'        Else
'          k = vbKeyUp
'        End If
'      ElseIf .Value > 0 Then
'        If .Value = .LargeChange Then
'          k = vbKeyPageDown
'        Else
'          k = vbKeyDown
'        End If
'      End If
'      DbgMsg "VScroll:key " & k & " current: " & txtCurr, 8, "ATCoDate", "t"
'      .Value = 0
'      Call txtDat_KeyDown(txtCurr, k, 0)
'      DoEvents
'      .Visible = True
'    End If
'  End With
End Sub

