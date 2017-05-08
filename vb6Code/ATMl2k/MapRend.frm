VERSION 5.00
Begin VB.Form frmMapRend 
   Caption         =   "Renderer for ?"
   ClientHeight    =   6252
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7704
   HelpContextID   =   524
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   6252
   ScaleWidth      =   7704
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraField 
      Caption         =   "Field"
      Height          =   552
      Left            =   60
      TabIndex        =   16
      Top             =   0
      Width           =   4992
      Begin VB.ComboBox comboField 
         Height          =   288
         Left            =   240
         Style           =   2  'Dropdown List
         TabIndex        =   0
         Top             =   180
         Width           =   2772
      End
   End
   Begin MSComDlg.CommonDialog cdlMapCov 
      Left            =   5220
      Top             =   120
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
   End
   Begin VB.Frame fraValue 
      Caption         =   "Value"
      Height          =   4572
      Left            =   5160
      TabIndex        =   13
      Top             =   1080
      Width           =   2292
   End
   Begin VB.Frame fraClass 
      Caption         =   "Class Break"
      Height          =   4572
      Left            =   60
      TabIndex        =   12
      Top             =   1080
      Width           =   4992
      Begin VB.VScrollBar sb 
         Height          =   4095
         Left            =   4680
         TabIndex        =   6
         Top             =   360
         Visible         =   0   'False
         Width           =   255
      End
      Begin VB.Frame fraVal 
         BorderStyle     =   0  'None
         Height          =   492
         Index           =   0
         Left            =   120
         TabIndex        =   15
         Top             =   420
         Width           =   4515
         Begin ATCoCtl.ATCoText attVal 
            Height          =   255
            Index           =   0
            Left            =   60
            TabIndex        =   7
            Top             =   180
            Width           =   1575
            _ExtentX        =   2773
            _ExtentY        =   445
            InsideLimitsBackground=   16777215
            OutsideHardLimitBackground=   8421631
            OutsideSoftLimitBackground=   8454143
            HardMax         =   -999
            HardMin         =   -999
            SoftMax         =   -999
            SoftMin         =   -999
            MaxWidth        =   5
            Alignment       =   1
            DataType        =   0
            DefaultValue    =   -999
            Value           =   "-999"
            Enabled         =   -1  'True
         End
         Begin VB.ComboBox comboStyle 
            Height          =   315
            Index           =   0
            Left            =   2655
            Style           =   2  'Dropdown List
            TabIndex        =   10
            Top             =   180
            Width           =   1812
         End
         Begin VB.CommandButton cmdColor 
            Appearance      =   0  'Flat
            BackColor       =   &H00FFFFFF&
            Height          =   255
            Index           =   0
            Left            =   1740
            MaskColor       =   &H00000002&
            Style           =   1  'Graphical
            TabIndex        =   8
            Top             =   180
            Width           =   375
         End
         Begin VB.CommandButton cmdOutColor 
            Appearance      =   0  'Flat
            BackColor       =   &H00000000&
            Height          =   255
            Index           =   0
            Left            =   2160
            MaskColor       =   &H00000002&
            Style           =   1  'Graphical
            TabIndex        =   9
            Top             =   180
            Width           =   375
         End
      End
      Begin VB.Label lblVal 
         Caption         =   "lblVal"
         Height          =   312
         Left            =   240
         TabIndex        =   14
         Top             =   180
         Width           =   4392
      End
   End
   Begin VB.CommandButton cmdClose 
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
      Height          =   372
      Index           =   0
      Left            =   1320
      TabIndex        =   4
      Top             =   5760
      Width           =   855
   End
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
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
      Index           =   1
      Left            =   2640
      TabIndex        =   5
      TabStop         =   0   'False
      Top             =   5760
      Width           =   855
   End
   Begin VB.Frame fraType 
      Caption         =   "Type"
      Height          =   552
      Left            =   60
      TabIndex        =   11
      Top             =   540
      Width           =   4992
      Begin VB.OptionButton optType 
         Caption         =   "None"
         Height          =   192
         Index           =   0
         Left            =   180
         TabIndex        =   1
         Top             =   240
         Width           =   732
      End
      Begin VB.OptionButton optType 
         Caption         =   "Class Break"
         Height          =   192
         Index           =   2
         Left            =   3360
         TabIndex        =   3
         Top             =   240
         Width           =   1212
      End
      Begin VB.OptionButton optType 
         Caption         =   "Value"
         Height          =   192
         Index           =   1
         Left            =   1740
         TabIndex        =   2
         Top             =   240
         Width           =   1092
      End
   End
End
Attribute VB_Name = "frmMapRend"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim l As New MapLayer
Dim vr As New MapObjectsLT.ValueMapRenderer
Dim cr As New MapObjectsLT.ClassBreaksRenderer
Dim Ind& 'index of map layer containing renderer
Dim rendtyp&  'renderer type(0-none,1-value,2-break)

'Returns true if successfulm false if frmMapRend should not be shown
Public Function SetRendererInfo(i&, layr As MapObjectsLT.MapLayer, lblLayer$, lblRend$) As Boolean
  Dim dbname$, locdb As Database, locrc As Recordset, DBFfilename$, j&, rendfld$
  SetRendererInfo = False
  On Error GoTo AbortFunction
  
  With frmMapRend
    .Caption = "Renderer for '" & lblLayer & "' layer"
    .comboField.Clear
    If lblRend = "None" Then
      rendfld = ""
      rendtyp = 0
      .fraType.Visible = False
    Else
      rendfld = layr.Renderer.field
     '.comboField.AddItem rendfld, 0
      If Left(lblRend, 1) = "V" Then
        rendtyp = 1
      ElseIf Left(lblRend, 1) = "B" Then
        rendtyp = 2
      End If
    End If
  End With
  
  dbname = GetDatabase(layr.File)
  DBFfilename = FilenameOnly(layr.File) & ".dbf"
  If Len(DBFfilename) > 12 Then
    If (MsgBox("The map database file " & layr.File & " has too long a name to be opened." & vbCr & "The maximum length is 8 characters plus .dbf" & vbCr & "Try anyway?", vbYesNo) = vbNo) Then Exit Function
  End If
  
  Set locdb = OpenDatabase(dbname, False, True, "DBASE IV")
  Set locrc = locdb.OpenRecordset(DBFfilename, dbOpenDynaset)
  For j = 0 To locrc.Fields.Count - 1
     's = locrc.Fields(j).Name
     'If s = rendfld Then ' need additional info
     '  frmMapRend.comboField.ItemData(0) = locrc.Fields(j).Type
     'Else
       frmMapRend.comboField.AddItem locrc.Fields(j).Name
       frmMapRend.comboField.ItemData(frmMapRend.comboField.NewIndex) = locrc.Fields(j).Type
     'End If
  Next j
  locrc.Close
  locdb.Close
  
  If Len(rendfld) > 0 Then frmMapRend.comboField.Text = rendfld ' default to current
  
  Ind = i
  Set l = layr
  If rendtyp = 1 Then 'value map
    Set vr = l.Renderer
  ElseIf rendtyp = 2 Then 'class break
    Set cr = l.Renderer
  End If
  optType(rendtyp) = True
  SetRendererInfo = True
  Exit Function
AbortFunction:
  'dbg.add "SetRendererInfo Error: " & Err.Description, 6, "ATCoMap", "e"
End Function

Private Sub fndValues(fld$)
  Dim mySet As Recordset
  Dim d$, i&, fmin#, fmax#, fval$, fnum As Boolean
  Dim s As New MapObjectsLT.Strings
  Dim MyDB As Database
  
  d = GetDatabase(l.File)
  Set MyDB = OpenDatabase(d, False, False, "DBASE IV")
  d = FilenameOnly(l.File) & ".dbf"
  Set mySet = MyDB.OpenRecordset(d, dbOpenDynaset)
  fmin = 1E+30
  fmax = -1E+30
  fnum = True
  Do Until mySet.EOF
    If Not IsNull(mySet(fld).Value) Then
      fval = mySet(fld).Value
      If IsNumeric(fval) Then
        If fval < fmin Then fmin = fval
        If fval > fmax Then fmax = fval
      Else
        fnum = False
      End If
      i = 0
      Do 'kludge to get around adding same value mult times
        If i = s.Count Then
          s.Add fval
        End If
        If fval = s(i) Then
          Exit Do
        End If
        i = i + 1
      Loop
    End If
    mySet.MoveNext
  Loop
  mySet.Close
  MyDB.Close
  
'    If s.Count < 9 Then
  lblVal.Caption = s.Count & " Values"
  'Debug.Print "fndValues " & l.symbol.SymbolType
  If fnum Then
    lblVal.Caption = lblVal.Caption & ",  Min : " & fmin
    lblVal.Caption = lblVal.Caption & ",  Max : " & fmax
  End If

  If optType(1) Then
    With vr
      .SymbolType = l.symbol.SymbolType
      .field = fld
      .ValueCount = s.Count
      For i = 0 To .ValueCount - 1
        If i <= fraVal.UBound Then 'Already have this frame
          fraVal(i).Visible = True
        Else
          'another frame
          Call addSymbolFrame(i)
        End If
        .Value(i) = s(i)
        attVal(i).Value = s(i)
        Call setColorAndStyle(i)
      Next i
      For i = .ValueCount To fraVal.UBound
        fraVal(i).Visible = False
      Next i
    End With
  ElseIf optType(2) Then
    With cr
      .SymbolType = l.symbol.SymbolType
      .field = fld
      .BreakCount = s.Count
      For i = 0 To .BreakCount - 1
        If i <= fraVal.UBound Then 'Already have this frame
          fraVal(i).Visible = True
        Else
          'another frame
          Call addSymbolFrame(i)
        End If
        .Break(i) = s(i)
        attVal(i).Value = s(i)
        Call setColorAndStyle(i)
      Next i
      For i = .BreakCount To fraVal.UBound
        fraVal(i).Visible = False
      Next i
    End With
  End If
'    Else
'      MsgBox "Too many values: " & s.Count
'    End If
  Set s = Nothing
End Sub

Private Sub setColorAndStyle(i&)

  'Debug.Print "setColorAndStyle: " & i & " " & l.Renderer.symbol(i).Color
  cmdColor(i).BackColor = l.Renderer.symbol(i).Color
  If l.Renderer.SymbolType = moFillSymbol And l.Renderer.symbol(i).Style = moTransparentFill Then
    cmdColor(i).Visible = False
  Else
    cmdColor(i).Visible = True
  End If
  
  If l.Renderer.symbol(i).SymbolType = moLineSymbol Then
    cmdOutColor(i).Visible = False
  Else
    cmdOutColor(i).BackColor = l.Renderer.symbol(i).OutlineColor
  End If
  
  comboStyle(i).Clear
  If l.Renderer.SymbolType = moPointSymbol Then 'point
    comboStyle(i).AddItem "Circle", 0
    comboStyle(i).AddItem "Square", 1
    comboStyle(i).AddItem "Triangle", 2
    comboStyle(i).AddItem "Cross", 3
  ElseIf l.Renderer.SymbolType = moLineSymbol Then 'line
    comboStyle(i).AddItem "Solid Line", 0
    comboStyle(i).AddItem "Dash Line", 1
    comboStyle(i).AddItem "Dot Line", 2
    comboStyle(i).AddItem "Dash-Dot Line", 3
    comboStyle(i).AddItem "Dash-Dot-Dot Line", 4
  ElseIf l.Renderer.SymbolType = moFillSymbol Then 'polygon
    comboStyle(i).AddItem "Solid Fill", 0
    comboStyle(i).AddItem "Transparent", 1
    comboStyle(i).AddItem "Horizontal", 2
    comboStyle(i).AddItem "Vertical", 3
    comboStyle(i).AddItem "Upward Diagonal", 4
    comboStyle(i).AddItem "Downward Diagonal", 5
    comboStyle(i).AddItem "Cross Fill", 6
    comboStyle(i).AddItem "Diagonal Cross", 7
    comboStyle(i).AddItem "Light Gray", 8
    comboStyle(i).AddItem "Gray", 9
    comboStyle(i).AddItem "Dark Gray", 10
  End If
  If l.Renderer.symbol(i).Style < comboStyle(i).ListCount Then comboStyle(i).ListIndex = l.symbol.Style

End Sub

Private Sub addSymbolFrame(i&)
  
  Load fraVal(i)
  Set fraVal(i).Container = fraVal(i - 1).Container
  fraVal(i).Top = fraVal(i - 1).Top + fraVal(i - 1).Height
  fraVal(i).Visible = True
  'another value
  Load attVal(i)
  Set attVal(i).Container = fraVal(i)
  attVal(i).Visible = True
  'another interior color
  Load cmdColor(i)
  Set cmdColor(i).Container = fraVal(i)
  cmdColor(i).Visible = True
  'another outline color
  Load cmdOutColor(i)
  Set cmdOutColor(i).Container = fraVal(i)
  cmdOutColor(i).Visible = True
  'another style
  Load comboStyle(i)
  Set comboStyle(i).Container = fraVal(i)
  comboStyle(i).Visible = True
  
End Sub

Private Sub comboField_Change()

   comboField_Click
   
End Sub

Private Sub comboField_Click() 'selected dataset field for renderer
  Dim lid&

  If comboField.ListIndex > -1 Then
    fraType.Visible = True
    lid = comboField.ItemData(comboField.ListIndex)
    'Debug.Print "combofield_click" & lid & comboField.ListIndex
    If lid = 10 Then
      If optType(2) Then optType(1) = True 'character, cant do break, try value
      optType(2).Visible = False
    Else
      optType(2).Visible = True
    End If
    If Len(l.File) > 0 Then Call fndValues(comboField.List(comboField.ListIndex))
  Else
    fraValue.Visible = False
    fraClass.Visible = False
    fraType.Visible = False
    rendtyp = 0
  End If

End Sub

Private Sub cmdClose_Click(Index As Integer)
  Dim i&, M&
  
  If Index = 0 Then 'ok - save new renderer
    If rendtyp > 0 Then
      If rendtyp = 1 Then
        M = l.Renderer.ValueCount
      Else
        M = l.Renderer.BreakCount
      End If
      For i = 0 To M - 1
        With l.Renderer.symbol(i)
          .Style = comboStyle(i).ListIndex
          If .SymbolType <> moFillSymbol Or .Style <> moTransparentFill Then .Color = cmdColor(i).BackColor
          If .SymbolType <> moLineSymbol Then .OutlineColor = cmdOutColor(i).BackColor
        End With
      Next i
    End If
    Call frmMapCov.UpdateRenderer(Ind, l, rendtyp)
  Else 'cancel
  End If
  Set vr = Nothing
  Set cr = Nothing
  Set l = Nothing
  Unload Me
End Sub

Private Sub comboStyle_Change(Index As Integer)

  comboStyle_Click Index

End Sub

Private Sub comboStyle_Click(Index As Integer)

  If l.Renderer.SymbolType = moFillSymbol And comboStyle(Index).ListIndex = moTransparentFill Then
    cmdColor(Index).Visible = False
  Else
    cmdColor(Index).Visible = True
  End If

End Sub

Private Sub Form_Load()
  'Me.Width = fraType.Width + 4 * fraType.Left
End Sub

Private Sub Form_Resize()
  Width = fraType.Width + 4 * fraType.Left
  If Height > 2088 Then
    Dim r&, TopRow&, BotRow&, pos&, dy&
    cmdClose(0).Top = Height - 900
    cmdClose(1).Top = cmdClose(0).Top
    fraClass.Height = Height - 2088
    fraValue.Height = fraClass.Height
    TopRow = 0
    dy = fraVal(0).Height
    If sb.Visible Then
      For r = fraVal.LBound To sb.Value - 1
        fraVal(r).Left = Width + 100
      Next r
      TopRow = sb.Value
    End If
    BotRow = fraVal.UBound
    pos = 420
    While r <= fraVal.UBound
      fraVal(r).Top = pos
      pos = pos + dy
      If pos < fraValue.Height Then
        fraVal(r).Left = 120
      Else
        fraVal(r).Left = Width + 100
        If r < BotRow Then BotRow = r - 1
      End If
      r = r + 1
    Wend
    If BotRow < fraVal.UBound Or TopRow > 0 Then 'need scrollbar
      If sb.Visible Then sb.Visible = False Else sb.Value = 0
      sb.Min = 0
      sb.Max = fraVal.UBound - (BotRow - TopRow)
      If (BotRow - TopRow > 1) Then
        sb.LargeChange = BotRow - TopRow
      Else
        sb.LargeChange = 2
      End If
      sb.Height = fraClass.Height - 475
      sb.Visible = True
    Else
      sb.Visible = False
    End If
  End If
End Sub

Private Sub optType_Click(Index As Integer)
  Dim i&, c&
  Dim tSym() As MapObjectsLT.symbol
  
  On Error GoTo errhand
  
  If TypeName(l.Renderer) <> "Nothing" Then
    On Error Resume Next
    If TypeName(l.Renderer.ValueCount) = "Nothing" Then
      c = l.Renderer.BreakCount
      Err.Clear
    Else
      c = l.Renderer.ValueCount
    End If
    On Error GoTo 0
    If c < 1 Then c = 1
    ReDim tSym(c - 1)
    For i = 0 To c - 1
      'Debug.Print "optType " & i & " "
      Set tSym(i) = l.Renderer.symbol(i)
    Next i
  Else ' no current renderer
    c = 0
  End If
  
  If Index = 1 Then 'value
    Set l.Renderer = vr
    fraValue.Left = fraClass.Left
    fraValue.Width = fraClass.Width
    fraClass.Visible = False
    fraValue.Visible = True
    Set lblVal.Container = fraValue
    Set sb.Container = fraValue
    For i = 0 To fraVal.UBound
      Set fraVal(i).Container = fraValue
    Next i
    rendtyp = 1
    If comboField.ListIndex > -1 Then
      Call fndValues(comboField.List(comboField.ListIndex))
      For i = 0 To c - 1
        Call copySymbol(tSym(i), l.Renderer.symbol(i))
        Call setColorAndStyle(i)
      Next i
      Call copySymbol(l.symbol, l.Renderer.DefaultSymbol)
    End If
  ElseIf Index = 2 Then 'class
    Set l.Renderer = cr
    fraValue.Visible = False
    fraClass.Visible = True
    Set lblVal.Container = fraClass
    Set sb.Container = fraClass
    For i = 0 To fraVal.UBound
      Set fraVal(i).Container = fraClass
    Next i
    rendtyp = 2
    If comboField.ListIndex > -1 Then
      Call fndValues(comboField.List(comboField.ListIndex))
      For i = 0 To c - 1
        Call copySymbol(tSym(i), l.Renderer.symbol(i))
        If i > fraVal.UBound Then
          Call addSymbolFrame(i)
        End If
        Call setColorAndStyle(i)
      Next i
    End If
  Else  'none
    Set l.Renderer = Nothing
    fraValue.Visible = False
    fraClass.Visible = False
    rendtyp = 0
  End If
  Exit Sub

errhand:
  MsgBox "Error adding frames to renderer window" & vbCr & Err.Description

End Sub

Private Sub sb_Change()
  Form_Resize
End Sub

Private Sub cmdColor_Click(Index As Integer)

  cdlMapCov.DialogTitle = "Choose a Fill Color"
  cdlMapCov.Color = cmdColor(Index).BackColor
  cdlMapCov.flags = &H1&
  cdlMapCov.ShowColor
  cmdColor(Index).BackColor = cdlMapCov.Color

End Sub

Private Sub cmdOutColor_Click(Index As Integer)

  cdlMapCov.DialogTitle = "Choose an Outline Color"
  cdlMapCov.Color = cmdOutColor(Index).BackColor
  cdlMapCov.flags = &H1&
  cdlMapCov.ShowColor
  cmdOutColor(Index).BackColor = cdlMapCov.Color

End Sub
