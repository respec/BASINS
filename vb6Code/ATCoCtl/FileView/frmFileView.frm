VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.1#0"; "Comdlg32.OCX"
Begin VB.Form frmFileView 
   Caption         =   "File View"
   ClientHeight    =   4770
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   6570
   LinkTopic       =   "Form1"
   ScaleHeight     =   4770
   ScaleWidth      =   6570
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtMain 
      Height          =   2295
      Left            =   0
      MultiLine       =   -1  'True
      ScrollBars      =   1  'Horizontal
      TabIndex        =   3
      Text            =   "frmFileView.frx":0000
      Top             =   1920
      Width           =   4395
   End
   Begin VB.CommandButton cmdNavigate 
      Caption         =   ">>"
      Height          =   375
      Index           =   3
      Left            =   2400
      TabIndex        =   7
      Top             =   4320
      Width           =   375
   End
   Begin VB.CommandButton cmdNavigate 
      Caption         =   ">"
      Height          =   375
      Index           =   2
      Left            =   1920
      TabIndex        =   6
      Top             =   4320
      Width           =   375
   End
   Begin VB.CommandButton cmdNavigate 
      Caption         =   "<"
      Height          =   375
      Index           =   1
      Left            =   600
      TabIndex        =   5
      Top             =   4320
      Width           =   375
   End
   Begin VB.CommandButton cmdNavigate 
      Caption         =   "<<"
      Height          =   375
      Index           =   0
      Left            =   120
      TabIndex        =   4
      Top             =   4320
      Width           =   375
   End
   Begin MSComDlg.CommonDialog comDlg 
      Left            =   0
      Top             =   600
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   327681
   End
   Begin VB.TextBox txtPage 
      Height          =   375
      Left            =   1080
      TabIndex        =   1
      Text            =   "Page"
      Top             =   4320
      Width           =   735
   End
   Begin VB.TextBox txtProperties 
      Height          =   735
      Left            =   0
      Locked          =   -1  'True
      TabIndex        =   0
      Text            =   "Properties"
      Top             =   3480
      Width           =   5415
   End
   Begin VB.TextBox txtHeader 
      Height          =   1875
      Left            =   0
      MultiLine       =   -1  'True
      TabIndex        =   2
      Text            =   "frmFileView.frx":000A
      Top             =   0
      Visible         =   0   'False
      Width           =   6375
   End
   Begin VB.Menu mnuFileTop 
      Caption         =   "&File"
      Begin VB.Menu mnuOpen 
         Caption         =   "&Open..."
      End
      Begin VB.Menu mnuCompare 
         Caption         =   "&Compare..."
         Enabled         =   0   'False
      End
      Begin VB.Menu mnuSave 
         Caption         =   "&Save"
         Enabled         =   0   'False
         Index           =   0
      End
      Begin VB.Menu mnuSave 
         Caption         =   "Save &As..."
         Enabled         =   0   'False
         Index           =   1
      End
      Begin VB.Menu mnuFileSep 
         Caption         =   "-"
         Index           =   0
      End
      Begin VB.Menu mnuClose 
         Caption         =   "&Close"
      End
   End
   Begin VB.Menu mnuViewTop 
      Caption         =   "&View"
      Begin VB.Menu mnuOptions 
         Caption         =   "&Options..."
      End
      Begin VB.Menu mnuFont 
         Caption         =   "&Font..."
      End
      Begin VB.Menu mnuViewSep 
         Caption         =   "-"
         Index           =   0
      End
      Begin VB.Menu mnuView 
         Caption         =   "No Views Available"
         Enabled         =   0   'False
         Index           =   0
      End
   End
   Begin VB.Menu mnuEditTop 
      Caption         =   "&Edit"
   End
End
Attribute VB_Name = "frmFileView"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public Filename$
Public FileHandle%
Public Options As collOptions
Public Fv As clsFileView               'Current class being used to display page

Private LastSelStart&
Private fileTypes As New collFileTypes 'Complete set of available display classes

'Add a new private variable here for each new fileView class
Private TxtFv As New clsTextFileView
Private BinFv As New clsBinFileView

Private privFirstDispByte&
Private privNextDispByte&
Private privBytesInFile&
Private privUnitsInFile&

Public Function FirstDispByte()
  FirstDispByte = privFirstDispByte
End Function

Public Function NextDispByte()
  NextDispByte = privNextDispByte
End Function

Public Function BytesInFile()
  BytesInFile = privBytesInFile
End Function

Public Function UnitsInFile()
  UnitsInFile = privUnitsInFile
End Function

Public Sub SetFirstDispByte(newValue&)
  privFirstDispByte = newValue
End Sub

Public Sub SetNextDispByte(newValue&)
  privNextDispByte = newValue
End Sub

Public Sub SetBytesInFile(newValue&)
  privBytesInFile = newValue
End Sub

Public Sub SetUnitsInFile(newValue&)
  privUnitsInFile = newValue
End Sub

Public Sub RefreshMain()
  Fv.DispPage ""
End Sub

'Display first, previous, next, or last page OR
'Search for first, previous, next, or last occurence of text
'Index: 0=first, 1=prev, 2=next, 3=last, -1=current (used after txtPage change)
Private Sub cmdNavigate_Click(Index As Integer)
  Select Case Index
    Case 0: SetFirstDispByte 1
    Case 1: SetFirstDispByte Fv.ScrollPos(-1, 3, 1)
    Case 2: SetFirstDispByte Fv.ScrollPos(1, 3, 1)
    Case 3: SetFirstDispByte Fv.ScrollPos(1, 3, 100000)
  End Select
  If Index >= 0 Then RefreshMain
  'Else 'search for text
  
  'End If
End Sub

Private Sub Form_Load()
  Dim mnuNum&
  
  txtHeader = ""
  txtMain = ""
  txtProperties = ""
  txtPage = ""
  
  Set Options = New collOptions
  
  While fileTypes.Count > 0
    fileTypes.Remove 1
  Wend
  fileTypes.Add BinFv
  fileTypes.Add TxtFv
  
  mnuNum = 1
  For Each Fv In fileTypes ' Iterate through each element.
    On Error Resume Next
    Unload mnuView(mnuNum)
    On Error GoTo 0
    Load mnuView(mnuNum)
    mnuView(mnuNum).Caption = Fv.Desc
    mnuNum = mnuNum + 1
  Next
  If mnuNum > 1 Then mnuView(0).Visible = False Else mnuView(0).Visible = True
  
  If Len(Filename) < 1 Then mnuOpen_Click Else OpenFile True
    
  If Len(Filename) < 1 Then
    'Unload Me
  Else
    Show
  End If
End Sub

Private Sub Form_Resize()
  Dim but&          'button number of navigation buttons
  
  If Width > 120 Then txtHeader.Width = Width - 120
  txtMain.Width = txtHeader.Width
  If txtHeader.Visible Then
    txtHeader.Height = TextHeight(txtHeader.Text) + 45
    txtMain.Top = txtHeader.Top + txtHeader.Height + 45
  Else
    txtMain.Top = txtHeader.Top
  End If

  If txtPage.Visible And Height > 1200 Then
    txtPage.Top = Height - 1140
    For but = 0 To 3
      cmdNavigate(but).Top = txtPage.Top
    Next but
    If txtPage.Top > 200 + txtMain.Top Then txtMain.Height = txtPage.Top - txtMain.Top - 105
    
    cmdNavigate(0).Left = 60
    cmdNavigate(1).Left = cmdNavigate(0).Left + cmdNavigate(0).Width + 105
    txtPage.Left = cmdNavigate(1).Left + cmdNavigate(1).Width + 105
    ResizeTxtPage
  End If
  RefreshMain
End Sub

Private Sub ResizeTxtPage()
  If Len(txtPage) > 0 Then txtPage.Width = TextWidth(txtPage & "XX")
  cmdNavigate(2).Left = txtPage.Left + txtPage.Width + 105
  cmdNavigate(3).Left = cmdNavigate(2).Left + cmdNavigate(2).Width + 105
End Sub

Private Sub mnuClose_Click()
  Unload Me
End Sub

Private Sub mnuFont_Click()
  CopyFont Me, comDlg
  On Error GoTo ExitSub
  comDlg.CancelError = True
  comDlg.Flags = cdlCFBoth
  comDlg.ShowFont
  SetFont comDlg
  OpenFile False
ExitSub:
  comDlg.CancelError = False
End Sub

Public Sub SetFont(o As Object)
  CopyFont o, Me
  CopyFont o, txtMain
  CopyFont o, txtHeader
  CopyFont o, txtProperties
End Sub

Public Sub AddOption(Label$, ByVal Value As Variant)
  Dim opt As clsOption
  Set opt = Options(Label)
  If opt.Valid Then    'Already have this option in collection
    opt.Visible = True
  Else                 'Need to add this option to collection
    opt.DefaultValue = Value
    opt.Label = Label
    opt.Value = Value
    opt.Visible = True
    opt.Valid = True
    
    Select Case VarType(Value)
      Case vbBoolean:
        opt.ControlType = 1     'checkbox
      Case Is >= vbArray:
        opt.ControlType = 2     'combo box
        opt.Value = Value(LBound(Value)) 'default to first item
      Case Else
        opt.ControlType = 0     'textbox
    End Select
    Options.Add opt, Label
  End If
End Sub

Public Sub ParseDelimiterOption(ByVal OptName$, ByRef Value$, ByRef lenValue&, ByRef lenValueGT0 As Boolean)
  Dim opt As clsOption
    
  Set opt = Options(OptName)
  If opt.Valid Then Value = opt.Value Else Value = ""
  lenValue = Len(Value)
  If lenValue > 0 Then lenValueGT0 = True Else lenValueGT0 = False
End Sub

Private Sub mnuOpen_Click()
  comDlg.Filename = Filename
  comDlg.DialogTitle = "Open"
  comDlg.ShowOpen
  If Len(comDlg.Filename) > 0 Then
    Filename = comDlg.Filename
    OpenFile True
  End If
End Sub

Private Sub mnuOptions_Click()
  Dim opt As clsOption
  Dim ypos&, yinc&, i&
  
  With frmOptions
    .Show
    ypos = .lblOption(0).Top
    yinc = .lblOption(0).Height * 2
    i = 1
    For Each opt In Options
      Load .lblOption(i)
      Load .txtOption(i)
      Load .chkOption(i)
      If opt.Visible Then
        Select Case opt.ControlType
          Case 0:
            .lblOption(i).Caption = opt.Label
            .txtOption(i).Text = opt.Value
            .lblOption(i).Top = ypos
            .txtOption(i).Top = ypos
            .lblOption(i).Visible = True
            .txtOption(i).Visible = True
          Case 1:
            .chkOption(i).Caption = opt.Label
            .chkOption(i).Top = ypos
            If opt.Value = True Then
              .chkOption(i).Value = 1
            Else
              .chkOption(i).Value = 0
            End If
            .chkOption(i).Visible = True
        End Select
        ypos = ypos + yinc
      End If
      i = i + 1
    Next
    ypos = ypos + yinc
    .cmdOk.Top = ypos
    .cmdCancel.Top = ypos
    If .WindowState = vbNormal Then .Height = ypos + .cmdOk.Height * 3
  End With
End Sub

'Index = 0 for Save, 1 for Save As
Private Sub mnuSave_Click(Index As Integer)
  Call Fv.SaveChanges
End Sub

Sub OpenFile(SetDefaultType As Boolean)
  SetAvailTypesFromFilename SetDefaultType
  On Error GoTo OpenError
  If Len(Dir(Filename)) > 0 Then
    Dim opt As clsOption
    txtMain.Visible = True
    txtHeader.Visible = False
    txtProperties.Visible = False
    For Each opt In Options
      opt.Visible = False
    Next
    
    On Error Resume Next
    If FileHandle > 0 Then Close FileHandle
    On Error GoTo OpenError
    SetBytesInFile FileLen(Filename)
    SetUnitsInFile 0
    SetFirstDispByte 1
    SetNextDispByte 0
    FileHandle = FreeFile(0)
    Caption = Filename & " (" & Fv.Desc & ")"
    Fv.OpenFile Me
    RefreshMain
  Else
    MsgBox "File '" & Filename & "' not found.", vbOKOnly, "Error"
    Filename = ""
  End If
  Exit Sub
OpenError:
  MsgBox "Error opening '" & Filename & "'" & vbCr & Err.Description, vbOKOnly, "Error"
End Sub

Sub SetAvailTypesFromFilename(SetDefaultType As Boolean)
  Dim i&, lastCan&
  For i = 1 To fileTypes.Count ' Iterate through each element.
    mnuView(i).Enabled = fileTypes(i).CanOpen(Filename)
    If mnuView(i).Enabled Then lastCan = i
  Next
  If SetDefaultType Then
    mnuView(lastCan).Checked = True
    Set Fv = fileTypes(lastCan)
  End If
End Sub

Private Sub CopyFont(src As Object, dst As Object)
  dst.FontBold = src.FontBold
  dst.FontItalic = src.FontItalic
  dst.FontName = src.FontName
  dst.FontSize = src.FontSize
  dst.FontStrikethru = src.FontStrikethru
  dst.FontUnderline = src.FontUnderline
End Sub

Private Sub mnuView_Click(Index As Integer)
  Dim mnuNum&
  For mnuNum = 1 To mnuView.Count - 1
    mnuView(mnuNum).Checked = False
  Next mnuNum
  For Each Fv In fileTypes ' Iterate through each element.
    If Fv.Desc = mnuView(Index).Caption Then
      mnuView(Index).Checked = True
      OpenFile False
      Exit For
    End If
  Next
End Sub

Private Sub txtMain_Change()
  mnuSave(0).Enabled = True
  mnuSave(1).Enabled = True
End Sub

Private Sub txtMain_KeyDown(KeyCode As Integer, Shift As Integer)
  LastSelStart = txtMain.SelStart
End Sub

Private Sub txtMain_KeyUp(KeyCode As Integer, Shift As Integer)
  Dim OldFirstByte&
  OldFirstByte = FirstDispByte
  Select Case KeyCode
    Case vbKeyDown, vbKeyRight:
      If LastSelStart = txtMain.SelStart Then SetFirstDispByte Fv.ScrollPos(1, 2, 1)  'line down
    Case vbKeyUp, vbKeyLeft:
      If LastSelStart = txtMain.SelStart Then SetFirstDispByte Fv.ScrollPos(-1, 2, 1) 'line up
    Case vbKeyPageDown:                       SetFirstDispByte Fv.ScrollPos(1, 3, 1)  'page down
    Case vbKeyPageUp:                         SetFirstDispByte Fv.ScrollPos(-1, 3, 1) 'page up
  End Select
  If FirstDispByte <> OldFirstByte Then RefreshMain 'if we moved display, redraw page
  
  'move cursor to end if we scrolled down, or to old position on top line if we scrolled up
  If FirstDispByte > OldFirstByte Then txtMain.SelStart = Len(txtMain)
  If FirstDispByte < OldFirstByte Then txtMain.SelStart = LastSelStart

End Sub

Private Sub txtPage_Change()
  ResizeTxtPage
  cmdNavigate_Click -1
End Sub

