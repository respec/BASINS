VERSION 5.00
Begin VB.UserControl ctlOperationEdit 
   ClientHeight    =   3576
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   11256
   ScaleHeight     =   3576
   ScaleWidth      =   11256
   Begin TabDlg.SSTab tabOperation 
      Height          =   2655
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   10935
      _ExtentX        =   19283
      _ExtentY        =   4678
      _Version        =   393216
      Tabs            =   4
      Tab             =   1
      TabsPerRow      =   4
      TabHeight       =   420
      TabCaption(0)   =   "Tables"
      TabPicture(0)   =   "ctlOperationEdit.ctx":0000
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "fraTableStatus"
      Tab(0).ControlCount=   1
      TabCaption(1)   =   "Special Actions"
      TabPicture(1)   =   "ctlOperationEdit.ctx":001C
      Tab(1).ControlEnabled=   -1  'True
      Tab(1).Control(0)=   "lblSpec"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).ControlCount=   1
      TabCaption(2)   =   "Input Timeseries"
      TabPicture(2)   =   "ctlOperationEdit.ctx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "fraInputStatus"
      Tab(2).ControlCount=   1
      TabCaption(3)   =   "Output Timeseries"
      TabPicture(3)   =   "ctlOperationEdit.ctx":0054
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "fraOutputStatus"
      Tab(3).ControlCount=   1
      Begin VB.Frame fraInputStatus 
         Caption         =   "Input Timeseries Status"
         Height          =   2172
         Left            =   -74880
         TabIndex        =   21
         Top             =   360
         Width           =   10695
         Begin VB.ListBox lstInputUnneededPresent 
            Height          =   816
            Left            =   2880
            TabIndex        =   25
            Top             =   480
            Width           =   2295
         End
         Begin VB.ListBox lstInputOptionalMissing 
            Height          =   816
            Left            =   8160
            TabIndex        =   24
            Top             =   480
            Width           =   2295
         End
         Begin VB.ListBox lstInputRequiredMissing 
            Height          =   816
            Left            =   5520
            TabIndex        =   23
            Top             =   480
            Width           =   2295
         End
         Begin VB.ListBox lstInputPresent 
            Height          =   816
            Left            =   240
            TabIndex        =   22
            Top             =   480
            Width           =   2295
         End
         Begin VB.Label lblInputUnneededPresent 
            Caption         =   "Unneeded Present:"
            Height          =   255
            Left            =   2760
            TabIndex        =   29
            Top             =   240
            Width           =   2655
         End
         Begin VB.Label lblInputOptionalMissing 
            Caption         =   "Missing Optional:"
            Height          =   255
            Left            =   8040
            TabIndex        =   28
            Top             =   240
            Width           =   2415
         End
         Begin VB.Label lblInputRequiredMissing 
            Caption         =   "Missing Required:"
            Height          =   255
            Left            =   5400
            TabIndex        =   27
            Top             =   240
            Width           =   2415
         End
         Begin VB.Label lblInputPresent 
            Caption         =   "Present "
            Height          =   255
            Left            =   120
            TabIndex        =   26
            Top             =   240
            Width           =   2415
         End
      End
      Begin VB.Frame fraOutputStatus 
         Caption         =   "Output Timeseries Status"
         Height          =   2172
         Left            =   -74880
         TabIndex        =   12
         Top             =   360
         Width           =   10695
         Begin VB.ListBox lstOutputPresent 
            Height          =   816
            Left            =   240
            TabIndex        =   16
            Top             =   480
            Width           =   2295
         End
         Begin VB.ListBox lstOutputRequiredMissing 
            Height          =   816
            Left            =   5520
            TabIndex        =   15
            Top             =   480
            Width           =   2295
         End
         Begin VB.ListBox lstOutputOptionalMissing 
            Height          =   816
            Left            =   8160
            TabIndex        =   14
            Top             =   480
            Width           =   2295
         End
         Begin VB.ListBox lstOutputUnneededPresent 
            Height          =   816
            Left            =   2880
            TabIndex        =   13
            Top             =   480
            Width           =   2295
         End
         Begin VB.Label lblOutputPresent 
            Caption         =   "Present "
            Height          =   255
            Left            =   120
            TabIndex        =   20
            Top             =   240
            Width           =   2415
         End
         Begin VB.Label lblOutputRequiredMissing 
            Caption         =   "Missing Required:"
            Height          =   255
            Left            =   5400
            TabIndex        =   19
            Top             =   240
            Width           =   2415
         End
         Begin VB.Label lblOutputOptionalMissing 
            Caption         =   "Missing Optional:"
            Height          =   255
            Left            =   8040
            TabIndex        =   18
            Top             =   240
            Width           =   2415
         End
         Begin VB.Label lblOutputUnneededPresent 
            Caption         =   "Unneeded Present:"
            Height          =   255
            Left            =   2760
            TabIndex        =   17
            Top             =   240
            Width           =   2655
         End
      End
      Begin VB.Frame fraTableStatus 
         Caption         =   "Table Status"
         Height          =   2172
         Left            =   -74880
         TabIndex        =   3
         Top             =   360
         Width           =   10695
         Begin VB.ListBox lstRequiredMissing 
            Height          =   816
            Left            =   5520
            TabIndex        =   7
            Top             =   480
            Width           =   2295
         End
         Begin VB.ListBox lstOptionalMissing 
            Height          =   816
            Left            =   8160
            TabIndex        =   6
            Top             =   480
            Width           =   2295
         End
         Begin VB.ListBox lstUnneededPresent 
            Height          =   816
            Left            =   2880
            TabIndex        =   5
            Top             =   480
            Width           =   2295
         End
         Begin VB.ListBox lstPresent 
            Height          =   816
            Left            =   240
            TabIndex        =   4
            Top             =   480
            Width           =   2295
         End
         Begin VB.Label lblRequiredMissing 
            Caption         =   "Missing Required:"
            Height          =   255
            Left            =   5400
            TabIndex        =   11
            Top             =   240
            Width           =   2415
         End
         Begin VB.Label lblOptionalMissing 
            Caption         =   "Missing Optional:"
            Height          =   255
            Left            =   8040
            TabIndex        =   10
            Top             =   240
            Width           =   2415
         End
         Begin VB.Label lblUnneededPresent 
            Caption         =   "Unneeded Present:"
            Height          =   255
            Left            =   2760
            TabIndex        =   9
            Top             =   240
            Width           =   2655
         End
         Begin VB.Label lblPresent 
            Caption         =   "Present:"
            Height          =   255
            Left            =   120
            TabIndex        =   8
            Top             =   240
            Width           =   2415
         End
      End
      Begin VB.Label lblSpec 
         Caption         =   "Edit Special Actions for this operation through the 'Edit Special Actions' menu option."
         Height          =   615
         Left            =   480
         TabIndex        =   30
         Top             =   720
         Width           =   6255
      End
   End
   Begin VB.Frame fraActive 
      Caption         =   "Active Sections"
      Height          =   612
      Left            =   120
      TabIndex        =   0
      Top             =   2880
      Width           =   10935
      Begin VB.CheckBox chkActive 
         Caption         =   "Check1"
         Height          =   252
         Index           =   0
         Left            =   240
         TabIndex        =   1
         Top             =   240
         Width           =   855
      End
   End
End
Attribute VB_Name = "ctlOperationEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pOperation As HspfOperation
Dim InRefresh
Dim haveActivity As Boolean
Private pFrm As Form

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
  pFrm.HelpContextID = 34
End Property

Public Property Set Owner(newOperation As HspfOperation)
  Set pOperation = newOperation 'should this be a copy?
  DisplayTableStatus
  DisplayInputStatus
  DisplayOutputStatus
End Property

Private Sub UpdateTabs()
  pOperation.TableStatus.Update
  DisplayTableStatus
  pOperation.InputTimeseriesStatus.Update
  DisplayInputStatus
  pOperation.OutputTimeseriesStatus.Update
  DisplayOutputStatus
End Sub

Private Sub DisplayTableStatus()
  Dim cTablesRequiredMissing As Collection 'of HspfStatusType
  Dim cTablesOptionalMissing As Collection 'of HspfStatusType
  Dim cTablesUnneededPresent As Collection 'of HspfStatusType
  Dim cTablesPresent As Collection 'of HspfStatusType
  Dim c As String
  
  c = "Table"
  fraTableStatus.Caption = c & " Status (" & pOperation.TableStatus.TotalPossible & " Possible)"
  
  'Set cTablesPresent = pOperation.TableStatus.GetInfo(HspfStatusRequired + hspfstatusoptional, HspfStatusPresent)
  Set cTablesPresent = pOperation.TableStatus.GetInfo(HspfStatusRequired, HspfStatusPresent)
  updateListBoxAndCaption "required", "present", c, cTablesPresent, lblPresent, lstPresent
  
  Set cTablesRequiredMissing = pOperation.TableStatus.GetInfo(HspfStatusRequired, HspfStatusMissing)
  updateListBoxAndCaption "required", "missing", c, cTablesRequiredMissing, lblRequiredMissing, lstRequiredMissing
  
  Set cTablesOptionalMissing = pOperation.TableStatus.GetInfo(HspfStatusOptional, HspfStatusMissing)
  updateListBoxAndCaption "optional", "missing", c, cTablesOptionalMissing, lblOptionalMissing, lstOptionalMissing
        
  'Set cTablesUnneededPresent = pOperation.TableStatus.GetInfo(HspfStatusUnneeded, HspfStatusPresent)
  Set cTablesUnneededPresent = pOperation.TableStatus.GetInfo(HspfStatusOptional, HspfStatusPresent)
  updateListBoxAndCaption "optional", "present", c, cTablesUnneededPresent, lblUnneededPresent, lstUnneededPresent
  
  ShowActivity pOperation.TableExists("ACTIVITY")
End Sub

Private Sub DisplayInputStatus()
  Dim cInputRequiredMissing As Collection 'of HspfStatusType
  Dim cInputOptionalMissing As Collection 'of HspfStatusType
  Dim cInputUnneededPresent As Collection 'of HspfStatusType
  Dim cInputPresent As Collection 'of HspfStatusType
  Dim c$
  
  c = "Timeseries"
  fraInputStatus.Caption = c & " Status (" & pOperation.InputTimeseriesStatus.TotalPossible & " Possible)"
  
  Set cInputPresent = pOperation.InputTimeseriesStatus.GetInfo(HspfStatusRequired, HspfStatusPresent)
  updateListBoxAndCaption "required", "present", c, cInputPresent, lblInputPresent, lstInputPresent
  
  Set cInputRequiredMissing = pOperation.InputTimeseriesStatus.GetInfo(HspfStatusRequired, HspfStatusMissing)
  updateListBoxAndCaption "required", "missing", c, cInputRequiredMissing, lblInputRequiredMissing, lstInputRequiredMissing
  
  Set cInputOptionalMissing = pOperation.InputTimeseriesStatus.GetInfo(HspfStatusOptional, HspfStatusMissing)
  updateListBoxAndCaption "optional", "missing", c, cInputOptionalMissing, lblInputOptionalMissing, lstInputOptionalMissing
        
  Set cInputUnneededPresent = pOperation.InputTimeseriesStatus.GetInfo(HspfStatusOptional, HspfStatusPresent)
  updateListBoxAndCaption "optional", "present", c, cInputUnneededPresent, lblInputUnneededPresent, lstInputUnneededPresent
  
End Sub

Private Sub DisplayOutputStatus()
  Dim cOutputRequiredMissing As Collection 'of HspfStatusType
  Dim cOutputOptionalMissing As Collection 'of HspfStatusType
  Dim cOutputUnneededPresent As Collection 'of HspfStatusType
  Dim cOutputPresent As Collection 'of HspfStatusType
  Dim c$
  
  c = "Timeseries"
  fraOutputStatus.Caption = c & " Status (" & pOperation.OutputTimeseriesStatus.TotalPossible & " Possible)"
  
  Set cOutputPresent = pOperation.OutputTimeseriesStatus.GetOutputInfo(HspfStatusRequired, HspfStatusPresent)
  updateListBoxAndCaption "required", "present", c, cOutputPresent, lblOutputPresent, lstOutputPresent
  
  Set cOutputRequiredMissing = pOperation.OutputTimeseriesStatus.GetOutputInfo(HspfStatusRequired, HspfStatusMissing)
  updateListBoxAndCaption "required", "missing", c, cOutputRequiredMissing, lblOutputRequiredMissing, lstOutputRequiredMissing
  
  Set cOutputOptionalMissing = pOperation.OutputTimeseriesStatus.GetOutputInfo(HspfStatusOptional, HspfStatusMissing)
  updateListBoxAndCaption "optional", "missing", c, cOutputOptionalMissing, lblOutputOptionalMissing, lstOutputOptionalMissing
        
  Set cOutputUnneededPresent = pOperation.OutputTimeseriesStatus.GetOutputInfo(HspfStatusOptional, HspfStatusPresent)
  updateListBoxAndCaption "optional", "present", c, cOutputUnneededPresent, lblOutputUnneededPresent, lstOutputUnneededPresent
End Sub

Private Sub updateListBoxAndCaption(s$, p$, c$, cStatus As Collection, lbl As Label, lst As ListBox)
  Dim vStatus As Variant, lStatus As HspfStatusType, cs$, lSub$

  If Right(c, 1) = "s" Then cs = c Else cs = c & "s"
  
  If cStatus.Count = 0 Then
    lbl = "No " & s & " " & LCase(cs) & " " & p & "."
    lst.Visible = False
  Else
    If cStatus.Count = 1 Then
      lbl = "1 " & s & " " & LCase(c) & " " & p & "."
    Else
      lbl = cStatus.Count & " " & s & " " & LCase(cs) & " " & p & "."
    End If
    lst.Visible = True
    lst.Clear
    For Each vStatus In cStatus
      Set lStatus = vStatus
      With lStatus
        If .Max = 1 Then
          lst.AddItem .Name
        Else
          If c = "Timeseries" Then
            lSub = "(" & .Occur Mod 1000 & "," & CLng(1 + (.Occur) / 1000) & ")"
          Else
            lSub = "(" & .Occur & ")"  'only one subscript for tables
          End If
          lst.AddItem .Name & lSub
          'lst.AddItem .Name & "(" & .occur & ")"
        End If
      End With
    Next vStatus
  End If
End Sub

Private Sub ShowActivity(b As Boolean)
  Dim i&, chkPerColumn
  
  If chkActive.Count = 1 Then
    InRefresh = True
    
    'With chkActive
    '  If .Count > 1 Then
    '    For i = .Count To 2 Step -1
    '      Unload chkActive(i - 1)
    '    Next i
    '  End If
    'End With
    
    If b Then
      Height = fraActive.Top + fraActive.Height + 10
      haveActivity = True
      With pOperation.Tables("ACTIVITY")
        chkPerColumn = 0
        For i = 0 To .Parms.Count - 1
          If i > 0 Then
            Load chkActive(i)
            chkActive(i).Top = chkActive(i - 1).Top + chkActive(i - 1).Height
            chkActive(i).Left = chkActive(i - 1).Left
            If chkActive(i).Top + chkActive(i).Height > fraActive.Height Then 'new column
              If chkPerColumn = 0 Then chkPerColumn = i
              chkActive(i).Left = chkActive(i - chkPerColumn).Left + chkActive(1).Width
              chkActive(i).Top = chkActive(0).Top
            End If
            chkActive(i).Visible = True
          End If
          chkActive(i).Caption = Left(.Parms(i + 1).Name, Len(.Parms(i + 1).Name) - 2)
          chkActive(i).Value = .Parms(i + 1).Value
        Next i
      End With
    Else
      haveActivity = False
      fraActive.Visible = False
      'Height = fraTableStatus.Height + 10
    End If
    
    InRefresh = False
  End If

End Sub

Public Sub Save()
  'update the operation here
End Sub

Private Sub chkActive_Click(Index As Integer)
  If Not (InRefresh) And haveActivity Then
    pOperation.Tables("ACTIVITY").Parms(Index + 1) = chkActive(Index).Value
    UpdateTabs
  End If
End Sub

Private Sub CurrentTableSelection(t$, i&)
  If lstRequiredMissing.ListIndex >= 0 Then
    t = lstRequiredMissing.List(lstRequiredMissing.ListIndex)
    i = 1
  ElseIf lstOptionalMissing.ListIndex >= 0 Then
    t = lstOptionalMissing.List(lstOptionalMissing.ListIndex)
    i = 2
  ElseIf lstPresent.ListIndex >= 0 Then
    t = lstPresent.List(lstPresent.ListIndex)
    i = 3
  ElseIf lstUnneededPresent.ListIndex >= 0 Then
    t = lstUnneededPresent.List(lstUnneededPresent.ListIndex)
    i = 4
  Else
    i = 0
  End If
End Sub

Private Sub AdjustTableName(t$, O&)
  Dim i&, l&
  
  i = InStr(t, "(")
  If i > 0 Then
    If Right(t, 3) = "(1)" Then 'remove first occur
      t = Left(t, Len(t) - 3)
      O = 1
    Else
      l = Len(t) - i - 1
      O = Mid(t, i + 1, l)
      t = Left(t, i - 1) & ":" & O
      'MsgBox "Adjusted string " & t
    End If
  Else
    O = 0 'only one possible
  End If
End Sub

Public Sub Help()
  SendKeys "{F1}"
  'HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
  'HtmlHelp pFrm.hwnd, App.HelpFile, HH_HELP_CONTEXT, 1&
End Sub

Public Sub Add()
  Dim s$
  If tabOperation.Tab = 0 Then
    AddTable
  ElseIf tabOperation.Tab = 1 Then
    s = "The Add option is not available for Special Actions." & vbCrLf & vbCrLf & _
       "Special Actions can be added through the Special Actions Block."
    Call MsgBox(s, vbOKOnly, "Add Problem")
  ElseIf tabOperation.Tab = 2 Then
    s = "The Add option is not available for Input Timeseries." & vbCrLf & vbCrLf & _
       "Input Timeseries can be added through the External Sources, Mass-Link, or Schematic Blocks."
    Call MsgBox(s, vbOKOnly, "Add Problem")
  ElseIf tabOperation.Tab = 3 Then
    s = "The Add option is not available for Output Timeseries." & vbCrLf & vbCrLf & _
       "Output Timeseries can be added through the External Targets, Mass-Link, or Schematic Blocks."
    Call MsgBox(s, vbOKOnly, "Add Problem")
  End If
End Sub

Public Sub AddTable()
  Dim tabNam$, tabOccur&, i&, j&, s$, tabNamSave$
  
  CurrentTableSelection tabNam, i
  
  If i = 1 Or i = 2 Then 'missing
    tabNamSave = tabNam
    AdjustTableName tabNam, tabOccur
    If tabOccur > 1 Then 'be sure to add other missing with same name before this one
      For j = 1 To tabOccur - 1
        If j > 1 Then
          s = Left(tabNam, InStr(tabNam, ":") - 1) & ":" & CStr(j)
        Else
          s = Left(tabNam, InStr(tabNam, ":") - 1)
        End If
        If Not (pOperation.TableExists(s)) Then
          pOperation.Uci.AddTable pOperation.Name, pOperation.Id, s
        End If
      Next j
    End If
    pOperation.Uci.AddTable pOperation.Name, pOperation.Id, tabNam
  ElseIf i = 3 Or i = 4 Then 'already have
    MsgBox "Table already exists, can't Add again", vbOKOnly, "Add Problem"
  Else
    MsgBox "Select a Table to Add", vbOKOnly, "Add Problem"
  End If
  UpdateTabs
End Sub

Public Sub Edit()
  Dim s$
  If tabOperation.Tab = 0 Then
    EditTable
  Else
    s = "The Edit option is not available for " & tabOperation.Caption & "." & vbCrLf & vbCrLf & _
       "Use Edit by Block Name instead"
    Call MsgBox(s, vbOKOnly, "Edit Problem")
  End If
End Sub
Public Sub EditTable()
  Dim doit&, ltable As HspfTable, t$, i&, O&
  Dim lEditAllSimilar As Boolean
  
  CurrentTableSelection t, i
  
  If i = 1 Then
    doit = MsgBox("Add required missing table before Edit?", vbYesNo, "Edit Table")
    If doit = vbYes Then
      AddTable
    End If
  ElseIf i = 2 Then
    doit = MsgBox("Add optional missing table before Edit?", vbYesNo, "Edit Table")
    If doit = vbYes Then
      AddTable
    End If
  ElseIf i = 3 Or i = 4 Then
    doit = vbYes
    doit = vbYes
  Else
    MsgBox "Select a Table to Edit", vbOKOnly, "Edit Table"
    doit = vbNo
  End If
  
  If doit = vbYes Then
    AdjustTableName t, O
    Set ltable = pOperation.Tables(t)
    lEditAllSimilar = ltable.EditAllSimilarChange(False)
    ltable.Edit
    ltable.EditAllSimilarChange (lEditAllSimilar)
    UpdateTabs
  End If
End Sub

Public Sub Remove()
  Dim s As String
  If tabOperation.Tab = 0 Then
    RemoveTable
  ElseIf tabOperation.Tab = 1 Then
    s = "The Remove option is not available for Special Actions." & vbCrLf & vbCrLf & _
       "Special Actions can be removed through the Special Actions Block."
    Call MsgBox(s, vbOKOnly, "Remove Problem")
  ElseIf tabOperation.Tab = 2 Then
    s = "The Remove option is not available for Input Timeseries." & vbCrLf & vbCrLf & _
       "Input Timeseries can be removed through the External Sources, Mass-Link, or Schematic Blocks."
    Call MsgBox(s, vbOKOnly, "Remove Problem")
  ElseIf tabOperation.Tab = 3 Then
    s = "The Remove option is not available for Output Timeseries." & vbCrLf & vbCrLf & _
       "Output Timeseries can be removed through the External Targets, Mass-Link, or Schematic Blocks."
    Call MsgBox(s, vbOKOnly, "Remove Problem")
  End If
End Sub
Public Sub RemoveTable()
  Dim t$, i&, O&
  
  CurrentTableSelection t, i
  If i = 4 Then
    If MsgBox("Are you sure you want to Remove " & t & "?", vbYesNo, "Remove Table") = vbYes Then
      AdjustTableName t, O
      pOperation.Tables.Remove t
      UpdateTabs
    End If
  ElseIf i = 3 Then
    MsgBox "Can't Remove a required table", vbOKOnly, "Remove Problem"
  ElseIf i = 1 Or i = 2 Then
    MsgBox "Can't Remove a table that does not exist", vbOKOnly, "Remove Problem"
  ElseIf i = 0 Then
    MsgBox "Select a table to Remove.", vbOKOnly, "Remove Problem"
  End If
End Sub

Private Sub lstOptionalMissing_Click()
  If Not (InRefresh) Then
    InRefresh = True
    lstUnneededPresent.ListIndex = -1
    lstRequiredMissing.ListIndex = -1
    lstPresent.ListIndex = -1
    InRefresh = False
  End If
End Sub

Private Sub lstOptionalMissing_DblClick()
  AddTable 'this table
End Sub

Private Sub lstPresent_Click()
  If Not (InRefresh) Then
    InRefresh = True
    lstUnneededPresent.ListIndex = -1
    lstOptionalMissing.ListIndex = -1
    lstRequiredMissing.ListIndex = -1
    InRefresh = False
  End If
End Sub

Private Sub lstPresent_DblClick()
  EditTable 'this table
End Sub

Private Sub lstRequiredMissing_Click()
  If Not (InRefresh) Then
    InRefresh = True
    lstUnneededPresent.ListIndex = -1
    lstOptionalMissing.ListIndex = -1
    lstPresent.ListIndex = -1
    InRefresh = False
  End If
End Sub

Private Sub lstRequiredMissing_DblClick()
  AddTable 'this table
End Sub

Private Sub lstUnneededPresent_Click()
  If Not (InRefresh) Then
    InRefresh = True
    lstRequiredMissing.ListIndex = -1
    lstOptionalMissing.ListIndex = -1
    lstPresent.ListIndex = -1
    InRefresh = False
  End If
End Sub

Private Sub lstUnneededPresent_DblClick()
  EditTable 'this table
End Sub

Private Sub UserControl_Initialize()
  tabOperation.Tab = 0
End Sub

Private Sub UserControl_Resize()

  Dim i&
  
  tabOperation.Width = Width - 300
  fraActive.Width = Width - 300
  fraTableStatus.Width = Width - 600
  fraInputStatus.Width = Width - 600
  fraOutputStatus.Width = Width - 600
  
  lstPresent.Width = (fraTableStatus.Width / 4) - 300
  lstUnneededPresent.Width = (fraTableStatus.Width / 4) - 300
  lstRequiredMissing.Width = (fraTableStatus.Width / 4) - 300
  lstOptionalMissing.Width = (fraTableStatus.Width / 4) - 300
  lstPresent.Left = 150
  lstUnneededPresent.Left = lstPresent.Width + 450
  lstRequiredMissing.Left = 2 * (lstPresent.Width) + 750
  lstOptionalMissing.Left = 3 * (lstPresent.Width) + 1050
  lblPresent.Width = lstPresent.Width
  lblUnneededPresent.Width = lstUnneededPresent.Width
  lblRequiredMissing.Width = lstRequiredMissing.Width
  lblOptionalMissing.Width = lstOptionalMissing.Width
  lblPresent.Left = lstPresent.Left
  lblUnneededPresent.Left = lstUnneededPresent.Left
  lblRequiredMissing.Left = lstRequiredMissing.Left
  lblOptionalMissing.Left = lstOptionalMissing.Left
  
  lstInputPresent.Width = (fraTableStatus.Width / 4) - 300
  lstInputUnneededPresent.Width = (fraTableStatus.Width / 4) - 300
  lstInputRequiredMissing.Width = (fraTableStatus.Width / 4) - 300
  lstInputOptionalMissing.Width = (fraTableStatus.Width / 4) - 300
  lstInputPresent.Left = 150
  lstInputUnneededPresent.Left = lstPresent.Width + 450
  lstInputRequiredMissing.Left = 2 * (lstPresent.Width) + 750
  lstInputOptionalMissing.Left = 3 * (lstPresent.Width) + 1050
  lblInputPresent.Width = lstPresent.Width
  lblInputUnneededPresent.Width = lstUnneededPresent.Width
  lblInputRequiredMissing.Width = lstRequiredMissing.Width
  lblInputOptionalMissing.Width = lstOptionalMissing.Width
  lblInputPresent.Left = lstPresent.Left
  lblInputUnneededPresent.Left = lstUnneededPresent.Left
  lblInputRequiredMissing.Left = lstRequiredMissing.Left
  lblInputOptionalMissing.Left = lstOptionalMissing.Left
  
  lstOutputPresent.Width = (fraTableStatus.Width / 4) - 300
  lstOutputUnneededPresent.Width = (fraTableStatus.Width / 4) - 300
  lstOutputRequiredMissing.Width = (fraTableStatus.Width / 4) - 300
  lstOutputOptionalMissing.Width = (fraTableStatus.Width / 4) - 300
  lstOutputPresent.Left = 150
  lstOutputUnneededPresent.Left = lstPresent.Width + 450
  lstOutputRequiredMissing.Left = 2 * (lstPresent.Width) + 750
  lstOutputOptionalMissing.Left = 3 * (lstPresent.Width) + 1050
  lblOutputPresent.Width = lstPresent.Width
  lblOutputUnneededPresent.Width = lstUnneededPresent.Width
  lblOutputRequiredMissing.Width = lstRequiredMissing.Width
  lblOutputOptionalMissing.Width = lstOptionalMissing.Width
  lblOutputPresent.Left = lstPresent.Left
  lblOutputUnneededPresent.Left = lstUnneededPresent.Left
  lblOutputRequiredMissing.Left = lstRequiredMissing.Left
  lblOutputOptionalMissing.Left = lstOptionalMissing.Left
  
  For i = 0 To chkActive.UBound
    chkActive(i).Width = ((fraActive.Width - 200) / (chkActive.UBound + 1)) - 50
    chkActive(i).Left = 100 + ((i + 1) * 50) + (chkActive(i).Width * i)
  Next i
  
  If Height > 2000 Then
    fraActive.Top = Height - 650
    tabOperation.Height = Height - 800
    fraTableStatus.Height = tabOperation.Height - 500
    fraInputStatus.Height = tabOperation.Height - 500
    fraOutputStatus.Height = tabOperation.Height - 500
    lstPresent.Height = fraTableStatus.Height - 700
    lstUnneededPresent.Height = lstPresent.Height
    lstRequiredMissing.Height = lstPresent.Height
    lstOptionalMissing.Height = lstPresent.Height
    lstInputPresent.Height = fraTableStatus.Height - 700
    lstInputUnneededPresent.Height = lstPresent.Height
    lstInputRequiredMissing.Height = lstPresent.Height
    lstInputOptionalMissing.Height = lstPresent.Height
    lstOutputPresent.Height = fraTableStatus.Height - 700
    lstOutputUnneededPresent.Height = lstPresent.Height
    lstOutputRequiredMissing.Height = lstPresent.Height
    lstOutputOptionalMissing.Height = lstPresent.Height
  End If
   
End Sub
