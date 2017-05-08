VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmMain 
   Caption         =   "WERF Model Selection Tool"
   ClientHeight    =   9588
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   12960
   Icon            =   "frmMain.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   9588
   ScaleWidth      =   12960
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraIntro 
      BorderStyle     =   0  'None
      Height          =   6735
      Left            =   240
      TabIndex        =   2
      Top             =   480
      Width           =   9495
      Begin VB.PictureBox picWerf 
         AutoSize        =   -1  'True
         Height          =   792
         Left            =   0
         Picture         =   "frmMain.frx":08CA
         ScaleHeight     =   744
         ScaleWidth      =   2268
         TabIndex        =   3
         Top             =   120
         Width           =   2316
      End
      Begin VB.Label lblIntro 
         AutoSize        =   -1  'True
         Caption         =   "Introduction"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.6
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Left            =   120
         TabIndex        =   8
         Top             =   1320
         Width           =   7815
         WordWrap        =   -1  'True
      End
   End
   Begin VB.Frame fraCriteria 
      BorderStyle     =   0  'None
      Caption         =   " "
      Height          =   8895
      Left            =   240
      TabIndex        =   5
      Top             =   480
      Width           =   9495
      Begin VB.CheckBox chkValue 
         Caption         =   "Check Value"
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   6
         Top             =   360
         Width           =   1695
      End
      Begin VB.Label lblCriteria 
         Caption         =   "Label Criteria"
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
         Left            =   0
         TabIndex        =   7
         Top             =   120
         Width           =   3015
      End
   End
   Begin MSComctlLib.TabStrip tabType 
      Height          =   9375
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   9735
      _ExtentX        =   17166
      _ExtentY        =   16531
      _Version        =   393216
      BeginProperty Tabs {1EFB6598-857C-11D1-B16A-00C0F0283628} 
         NumTabs         =   1
         BeginProperty Tab1 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Introduction"
            ImageVarType    =   2
         EndProperty
      EndProperty
   End
   Begin VB.Frame fraMatch 
      Caption         =   "Models Meeting Criteria"
      Height          =   9252
      Left            =   9960
      TabIndex        =   0
      Top             =   120
      Width           =   2895
      Begin VB.ListBox lstModels 
         Height          =   8880
         ItemData        =   "frmMain.frx":1754
         Left            =   120
         List            =   "frmMain.frx":1756
         Sorted          =   -1  'True
         TabIndex        =   4
         Top             =   240
         Width           =   2655
      End
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim myDB As Database
Private MinHeight As Long

Private Sub chkValue_Click(Index As Integer)
  Dim c$, v$, t$, i&, j&, k&, a As Boolean, M As Boolean, model$
  Dim myDetails As Recordset
  Dim lMatches As Collection 'of lVMatch
  Dim lVMatch As Collection 'of matching models
  Dim lMatch As Collection 'of matching models (from lMatch)
  Static AutoUnchecking As Boolean
  
  If AutoUnchecking Then Exit Sub
  
  'Debug.Print Index, chkValue(Index).Tag, chkValue(Index).Caption
  
  Screen.MousePointer = vbHourglass
  Set lMatches = Nothing
  Set lMatches = New Collection
  Set myDetails = myDB.OpenRecordset("Details", dbOpenDynaset)
  t = Left(tabType.SelectedItem.Caption, 4)
  
  'Treat some checkboxes as radio buttons
  c = chkValue(Index).Tag
  Select Case c
    Case "Level of Effort", _
         "Data Requirements", _
         "Modeler Expertise", _
         "Model Availability"
        AutoUnchecking = True
        For i = 0 To chkValue.Count - 1
          If i <> Index Then
            If chkValue(i).Tag = c Then
              chkValue(i).Value = vbUnchecked
            End If
          End If
        Next
        AutoUnchecking = False
  End Select
  
  For i = 0 To chkValue.Count - 1
    If chkValue(i) Then
      Set lVMatch = New Collection
      myDetails.MoveFirst
      v = chkValue(i).Caption
      c = chkValue(i).Tag
      While Not myDetails.EOF
        If v = myDetails!Value And c = myDetails!criteria And t = Left(myDetails!Type, 4) Then
          model = myDetails!model
          lVMatch.Add model
        End If
        myDetails.MoveNext
      Wend
      lMatches.Add lVMatch
    End If
  Next i
  myDetails.Close
  
  lstModels.Clear
  If lMatches.Count = 0 Then 'nothing selected, add all
    RefreshModels (t)
  ElseIf lMatches.Count = 1 Then 'only one selected, add all matches
    Set lVMatch = lMatches(1)
    For i = 1 To lVMatch.Count
      lstModels.AddItem lVMatch(i)
    Next i
  Else 'did we match all criteria?
    Set lMatch = lMatches(1)
    For k = 1 To lMatch.Count
      a = True
      For j = 2 To lMatches.Count
        Set lVMatch = lMatches(j)
        M = False
        For i = 1 To lVMatch.Count
          If lVMatch(i) = lMatch(k) Then
            M = True
            Exit For
          End If
        Next i
        If Not (M) Then
          a = False
        End If
      Next j
      If a Then lstModels.AddItem lMatch(k)
    Next k
  End If
  Screen.MousePointer = vbDefault
  
End Sub

Private Sub Form_Load()
  Dim myRs As Recordset
  Dim s$, i&, hdle&, EXEName$, BasePath$
  Dim n As String * 80
  Dim RunningVB As Boolean
  
  hdle = GetModuleHandle("SelectModel")
  i = GetModuleFileName(hdle, n, 80)
  EXEName = UCase(Left(n, InStr(n, Chr(0)) - 1))
  If InStr(EXEName, "VB6.EXE") Then
    RunningVB = True
    EXEName = "c:\vbexperimental\werf\SelectModel.exe"
  Else
    RunningVB = False
  End If
  BasePath = PathNameOnly(EXEName)
  If UCase(Right(BasePath, 4)) = "\BIN" Then BasePath = Left(BasePath, Len(BasePath) - 4)
  App.HelpFile = BasePath & "\doc\Werf Models.chm"
  
  Set myDB = OpenDatabase(BasePath & "\data\modelselection.mdb", , True)
  Set myRs = myDB.OpenRecordset("Types", dbOpenDynaset)
  While Not myRs.EOF
    tabType.Tabs.Add tabType.Tabs.Count + 1, , myRs("Name")
    myRs.MoveNext
  Wend
  tabType_Click
  
  s = "The WERF Water Quality Model Selection Tool provides a means for water quality managers to become familiar with nearly 150 water quality models, and to select the model(s) that are most appropriate to satisfy a particular situation or need. "
  s = s & vbCr & vbCr & "Within this tool, model evaluation capabilities are organized to enable comparison of all models within any of six 'model classes'.  Models classes that are included are: hydrodynamic models, non-urban runoff models, urban runoff models, receiving water quality models, chemical fate and transport models, and groundwater models. "
  s = s & vbCr & vbCr & "Begin the model evaluation process by selecting the tab for the model class you wish to explore. "
  s = s & vbCr & vbCr & "On the resulting model selection tab designate the criteria that you wish your model(s) to satisfy. A unique set of evaluation criteria has been established for each model class. A 'checkbox' approach to identifying models is used.  Indicating 'yes' to multiple checkboxes dealing with a particular criterion is appropriate in numerous instances. "
  s = s & vbCr & vbCr & "Click on any model name in the 'models meeting criteria' frame of the model selection screen to access a description of that model."
  s = s & vbCr & vbCr & "DISCLAIMER: The information contained within this Model Selection Tool is based on publications and literature provided by model developers, as well as from model developers' web sites.  No verification or testing of model accuracy or function is implied by this review. Mention of trade names or commercial products does not constitute WERF endorsement or recommendation for use.  Similarly, omission of products or trade names indicates nothing concerning WERF's position regarding product effectiveness or applicability."
  lblIntro.Caption = s
  
End Sub

Private Sub Form_Resize()
  Dim w As Long
  Dim h As Long
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If Me.WindowState = vbNormal And h > 0 Then
    If h < MinHeight + fraCriteria.Top + tabType.Top Then
      Me.Height = MinHeight + fraCriteria.Top + tabType.Top * 2 + (Me.Height - h)
      Exit Sub
    End If
  End If
  If w > 4000 And h > 4000 Then
    tabType.Height = h - tabType.Top - 93
    If fraIntro.Visible Then
      tabType.Width = w - tabType.Left * 2
    Else
      tabType.Width = w - fraMatch.Width - tabType.Left * 3
      fraMatch.Left = w - fraMatch.Width - tabType.Left
      fraMatch.Height = tabType.Height
      lstModels.Height = fraMatch.Height - 372
    End If
    fraCriteria.Height = tabType.Height - 480
    fraCriteria.Width = tabType.Width - tabType.Left * 2
    fraIntro.Height = fraCriteria.Height
    fraIntro.Width = fraCriteria.Width
    lblIntro.Width = fraIntro.Width - lblIntro.Left * 2
  End If
End Sub

Private Sub lstModels_Click()
  Dim d As HH_AKLINK, k$
  
  k = lstModels.List(lstModels.ListIndex)
  d.pszKeywords = k
  d.fReserved = vbFalse
  d.cbStruct = LenB(d)
  'HtmlHelp Me.hwnd, App.HelpFile, HH_ALINK_LOOKUP, d
  HtmlHelp Me.hwnd, App.HelpFile, HH_KEYWORD_LOOKUP, d

End Sub

Private Sub tabType_Click()
  Dim modType$
  
  'Debug.Print tabType.SelectedItem.Index
  If tabType.SelectedItem.Index = 1 Then
    fraIntro.Visible = True
    fraCriteria.Visible = False
    Width = tabType.Width + (tabType.Left * 2) + 120
    fraIntro.Height = tabType.Height - 480
    'lblIntro.Height = fraIntro.Height - picWerf.Height - 2400
  Else
    fraIntro.Visible = False
    fraCriteria.Visible = True
    If Me.WindowState = vbNormal Then Width = fraMatch.Left + fraMatch.Width + 240
    modType = Left(tabType.SelectedItem.Caption, 4)
    RefreshModels modType
    RefreshCriteria modType
  End If
End Sub

Private Sub RefreshCriteria(t$)
  Dim myCriteria As Recordset
  Dim myValue As Recordset
  Dim l&, lTop&, lLeft&, lCcnt&, lVcnt&, lKcnt&, s$
  Dim wid As Long, maxRowWidth As Long
  Dim ResizeRow As Long, ResizeCol As Long
  Dim ResizeIndex As Long
  Dim UnResizeIndex As Long
  Dim maxWidth(50) As Long
  
  Me.MousePointer = vbHourglass
  While lblCriteria.Count > 1
    Unload lblCriteria(lblCriteria.Count - 1)
  Wend
  While chkValue.Count > 1
    Unload chkValue(chkValue.Count - 1)
  Wend
  chkValue(0) = False
  
  Set myCriteria = myDB.OpenRecordset("Criteria", dbOpenDynaset)
  Set myValue = myDB.OpenRecordset("Values", dbOpenDynaset)
  lCcnt = 0
  lVcnt = 0
  lTop = lblCriteria(0).Top
  lblCriteria(0).Visible = False
  chkValue(0).Visible = False
  While Not myCriteria.EOF
    If t = Left(myCriteria!Type, 4) Then
      If lCcnt > 0 Then Load lblCriteria(lCcnt): lblCriteria(lCcnt).Top = lTop
      lblCriteria(lCcnt).Caption = myCriteria!Name
      lblCriteria(lCcnt).Visible = True
      lTop = lTop + lblCriteria(0).Height
      lKcnt = 0
      myValue.MoveFirst
      While Not myValue.EOF
        If t = Left(myValue!Type, 4) And myCriteria!Name = myValue!criteria Then
          s = myValue!Name
          lKcnt = lKcnt + 1
          If lVcnt > 0 Then Load chkValue(lVcnt): chkValue(lVcnt).Top = lTop
          wid = Me.TextWidth(s) + 500
          chkValue(lVcnt).Width = wid
          'If lKcnt > 1 Then chkValue(lVcnt).Left = chkValue(lVcnt - 1).Left + chkValue(lVcnt - 1).Width
          chkValue(lVcnt).Caption = s
          chkValue(lVcnt).Tag = myCriteria!Name
          chkValue(lVcnt).Visible = True
          If wid > maxWidth(lKcnt) Then maxWidth(lKcnt) = wid
          lVcnt = lVcnt + 1
        End If
        myValue.MoveNext
      Wend
      If lKcnt > 0 Then lTop = lTop + lblCriteria(0).Height
      lCcnt = lCcnt + 1
    End If
    myCriteria.MoveNext
  Wend
  myCriteria.Close
  myValue.Close
  'Line up checkboxes
  maxRowWidth = fraCriteria.Width
  ResizeIndex = 0
  For ResizeRow = 0 To lCcnt - 1
    ResizeCol = 1
    s = chkValue(ResizeIndex).Tag
    wid = maxWidth(ResizeCol)
    chkValue(ResizeIndex).Width = wid
    If ResizeIndex + 1 < lVcnt Then
      While s = chkValue(ResizeIndex + 1).Tag
        ResizeCol = ResizeCol + 1
        ResizeIndex = ResizeIndex + 1
        chkValue(ResizeIndex).Left = chkValue(ResizeIndex - 1).Left + wid
        wid = maxWidth(ResizeCol)
        chkValue(ResizeIndex).Width = wid
        If ResizeIndex + 1 >= lVcnt Then Exit For
      Wend
      If chkValue(ResizeIndex).Left + wid > maxRowWidth Then 'maxRowWidth = chkValue(ResizeIndex).Left + wid
        wid = Me.TextWidth(chkValue(ResizeIndex - ResizeCol + 1).Caption) + 500
        chkValue(ResizeIndex - ResizeCol + 1).Width = wid
        For UnResizeIndex = ResizeIndex - ResizeCol + 2 To ResizeIndex
          chkValue(UnResizeIndex).Left = chkValue(UnResizeIndex - 1).Left + wid
          wid = Me.TextWidth(chkValue(UnResizeIndex).Caption) + 500
          chkValue(UnResizeIndex).Width = wid
        Next
      End If
      ResizeIndex = ResizeIndex + 1
    End If
  Next
  MinHeight = chkValue(lVcnt - 1).Top + chkValue(lVcnt - 1).Height
  Form_Resize
  Me.MousePointer = vbDefault
End Sub

Private Sub RefreshModels(t$)
  Dim myModels As Recordset
 
  'display all possible
  lstModels.Clear
  Set myModels = myDB.OpenRecordset("Models", dbOpenDynaset)
  While Not myModels.EOF
    If t = Left(myModels!Type, 4) Then
      lstModels.AddItem myModels!Name
    End If
    myModels.MoveNext
  Wend
  myModels.Close

End Sub
