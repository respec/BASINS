VERSION 5.00
Begin VB.Form frmSelectScript 
   Caption         =   "Script Selection for Import"
   ClientHeight    =   3465
   ClientLeft      =   45
   ClientTop       =   270
   ClientWidth     =   9840
   HelpContextID   =   801
   LinkTopic       =   "Form1"
   ScaleHeight     =   3465
   ScaleWidth      =   9840
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   2772
      Left            =   8400
      TabIndex        =   7
      Top             =   120
      Width           =   1332
      Begin VB.CommandButton cmdRun 
         Caption         =   "&Run"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   0
         TabIndex        =   1
         Top             =   0
         Width           =   1332
      End
      Begin VB.CommandButton cmdWizard 
         Caption         =   "&Edit"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   0
         TabIndex        =   2
         Top             =   480
         Width           =   1332
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "&Cancel"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   0
         TabIndex        =   6
         Top             =   2400
         Width           =   1332
      End
      Begin VB.CommandButton cmdFind 
         Caption         =   "&Find..."
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   0
         TabIndex        =   3
         Top             =   960
         Width           =   1332
      End
      Begin VB.CommandButton cmdDelete 
         Caption         =   "For&get"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   0
         TabIndex        =   4
         Top             =   1440
         Width           =   1332
      End
      Begin VB.CommandButton cmdTest 
         Caption         =   "&Debug"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   372
         Left            =   0
         TabIndex        =   5
         Top             =   1920
         Width           =   1332
      End
   End
   Begin ATCoCtl.ATCoGrid agdScripts 
      Height          =   3252
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   8172
      _ExtentX        =   14420
      _ExtentY        =   5741
      SelectionToggle =   0   'False
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
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
      Header          =   ""
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   1
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483632
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
   Begin MSComDlg.CommonDialog dlgOpenFile 
      Left            =   0
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
End
Attribute VB_Name = "frmSelectScript"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Public ButtonPressed As String
Private pDataFilename As String
Private Const CanReadBackColor = 11861940 'RGB(180, 255, 180)

Private Sub agdScripts_Click()
  EnableButtons
End Sub

Private Sub cmdCancel_Click()
  ButtonPressed = cmdCancel.Caption
  Me.Hide
End Sub

Private Sub cmdFind_Click()
  Dim bgColor As Long
  ButtonPressed = cmdFind.Caption
  dlgOpenFile.Filter = "Wizard Script Files (*.ws)|*.ws|All Files (*.*)|*.*"
  dlgOpenFile.DefaultExt = "ws"
  dlgOpenFile.DialogTitle = "Open Script File"
  dlgOpenFile.ShowOpen
  If dlgOpenFile.filename <> "" Then
    Dim ScriptFilename$, ScriptDescription$, Script As clsATCscriptExpression
    ScriptFilename = dlgOpenFile.filename
    Set Script = ScriptFromString(WholeFileString(ScriptFilename))
    If Script Is Nothing Then
      ScriptDescription = err.Description
      bgColor = vbRed
    Else
      ScriptDescription = Script.SubExpression(1).Printable
      Set Script = Nothing
      SaveSetting "ATCTimeseriesImport", "Scripts", ScriptFilename, ScriptDescription
      bgColor = TestScriptColor(ScriptFilename)
    End If
    With agdScripts
      .rows = .rows + 1
      .row = .rows
      .Col = 0
      .Text = ScriptDescription
      .CellBackColor = bgColor
      .Col = 1
      .Text = ScriptFilename
      .CellBackColor = bgColor
      While Not .RowIsVisible(agdScripts.rows)
        .TopRow = .TopRow + 1
      Wend
      .Selected(.rows, 0) = True
    End With
    EnableButtons
  End If
End Sub

Private Sub cmdHelp_Click()
  MsgBox "Select a script that will recognize the data you are importing. " & vbCr _
  & "If no appropriate script is listed, select a similar one " & vbCr _
  & "and click 'Edit' to create a new script based on it." & vbCr _
  & "'Run' interprets the selected script and imports your data." & vbCr _
  & "'Edit' reads the selected script and presents an interface for customizing it." & vbCr _
  & "      Note: some complex scripts use features that can not yet be edited in the graphical " & vbCr _
  & "      interface. These scripts may be edited manually as text files before pressing 'Run'. " & vbCr _
  & "'Find' browses your disk for new scripts that are not in the list." & vbCr _
  & "'Forget' removes the selected script from the list, but leaves it on disk." & vbCr _
  & "'Debug' runs the selected script one step at a time." & vbCr _
  & "'Cancel' closes this window without importing any data" & vbCr _
  & "Green scripts have tested the current file and can probably read it." & vbCr _
  & "Pink scripts have tested the current file and probably can't read it." & vbCr _
  & "Red scripts contain errors or cannot be found on disk." & vbCr _
  & "Other scripts are unable to test files for readability.", vbOKOnly, "Help for Script Selection"
End Sub

Private Sub cmdDelete_Click()
  If agdScripts.TextMatrix(agdScripts.row, 1) <> "" Then
    If MsgBox("About to forget script:" & vbCr _
       & "Description: " & agdScripts.TextMatrix(agdScripts.row, 0) & vbCr _
       & "Filename: " & agdScripts.TextMatrix(agdScripts.row, 1), vbYesNo, "Confirm Forget") = vbYes Then
      DeleteSetting "ATCTimeseriesImport", "Scripts", agdScripts.TextMatrix(agdScripts.row, 1)
      LoadGrid pDataFilename 'This is inefficient, but easier than copying all the .textmatrix and .cellbackcolor
      'agdScripts.TextMatrix(agdScripts.row, 0) = ""
      'agdScripts.TextMatrix(agdScripts.row, 1) = ""
    End If
  End If
  EnableButtons
End Sub

Private Sub cmdRun_Click()
  ButtonPressed = cmdRun.Caption
  Me.Hide
End Sub

Private Sub cmdTest_Click()
  ButtonPressed = cmdTest.Caption
  Me.Hide
End Sub

Private Sub cmdWizard_Click()
  ButtonPressed = cmdWizard.Caption
  Me.Hide
End Sub

Public Sub LoadGrid(Optional DataFilename$ = "")
  Dim MySettings As Variant
  Dim intSettings As Long
  Dim bgColor As Long
  Dim CanReadRow As Long
  Dim RowsFilled As Long
  
  pDataFilename = DataFilename
  With agdScripts
    .ColTitle(0) = "Description"
    .ColTitle(1) = "Script File"
    MySettings = GetAllSettings("ATCTimeseriesImport", "Scripts")
    .rows = 0
    CanReadRow = 0
    .TextMatrix(1, 0) = "Blank Script"
    .TextMatrix(1, 1) = ""
    RowsFilled = 1
    If IsEmpty(MySettings) Then
      MsgBox "Use the Find button to locate scripts." & vbCr & _
             "Look for the Scripts directory where this program is installed.", vbOKOnly, "No Scripts Found Yet"
    Else
      .rows = UBound(MySettings, 1) - LBound(MySettings, 1) + 2
      For intSettings = LBound(MySettings, 1) To UBound(MySettings, 1)
        'Set filename in second column
        .row = RowsFilled + 1
        .Col = 1
        If FileExists(MySettings(intSettings, 0)) Then
          .Text = MySettings(intSettings, 0)
          
          'Set background of cell based on whether this script can read data file
          bgColor = TestScriptColor(.Text)
          .CellBackColor = bgColor
          
          'Set description in first column
          .Col = 0
          .Text = MySettings(intSettings, 1)
          .CellBackColor = bgColor
                
          If bgColor = CanReadBackColor Then CanReadRow = .row
          RowsFilled = RowsFilled + 1
        End If
      Next intSettings
    End If
    .rows = RowsFilled

    If CanReadRow > 0 Then
      .row = CanReadRow:
      .Selected(CanReadRow, 0) = True
      If Not .RowIsVisible(CanReadRow) Then .TopRow = CanReadRow
    End If
  End With
  EnableButtons
  
End Sub

Private Function TestScriptColor(ScriptFilename As String) As Long
  Dim Script As clsATCscriptExpression
  Dim TestResult As String
  Dim filename As String, ScriptString As String
  
  On Error GoTo ErrExit
  With agdScripts
    TestScriptColor = .InsideLimitsBackground
    If pDataFilename <> "" Then
      If FileExists(ScriptFilename) Then
        ScriptString = WholeFileString(ScriptFilename)
        If InStr(ScriptString, "(Test ") > 0 Then
          Set Script = ScriptFromString(ScriptString)
          If Script Is Nothing Then
            GoTo ErrExit
          Else
            TestResult = ScriptTest(Script, pDataFilename)
            Select Case TestResult
              Case "0": TestScriptColor = .OutsideHardLimitBackground ' No, this script can not read this data file
              Case "1": TestScriptColor = CanReadBackColor            ' Yes, this script can read this data file
              Case Else: MsgBox "Script '" & ScriptFilename & "' test says: " & vbCr & TestResult & vbCr & "(Expected 0 or 1)", vbOKOnly, "Script Test"
            End Select
          End If
        End If
      End If
    End If
  End With
  Exit Function
ErrExit:
  TestScriptColor = vbRed
End Function

Private Sub Form_Resize()
  If Height > 600 Then agdScripts.Height = Height - 576
  If Width > 450 Then
    fraButtons.Left = Width - fraButtons.Width - 192
    If fraButtons.Left > 300 Then agdScripts.Width = fraButtons.Left - 228
  End If
End Sub

Private Sub EnableButtons()
  If agdScripts.SelCount > 0 And Len(agdScripts.Text) > 0 Then
    cmdWizard.Enabled = True
    If agdScripts.row > 1 Then
      cmdRun.Enabled = True
    Else
      cmdRun.Enabled = False
    End If
  Else
    cmdWizard.Enabled = False
    cmdRun.Enabled = False
  End If
  cmdDelete.Enabled = cmdRun.Enabled
  cmdTest.Enabled = cmdRun.Enabled
End Sub
