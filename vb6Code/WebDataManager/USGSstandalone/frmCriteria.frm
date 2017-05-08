VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmCriteria 
   Caption         =   "Criteria"
   ClientHeight    =   3345
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   9315
   Icon            =   "frmCriteria.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3345
   ScaleWidth      =   9315
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   612
      Left            =   120
      TabIndex        =   8
      Top             =   2640
      Width           =   7332
      Begin VB.Frame fraDetails 
         Caption         =   "Details"
         Height          =   612
         Left            =   4080
         TabIndex        =   12
         Top             =   0
         Visible         =   0   'False
         Width           =   3252
         Begin VB.CommandButton cmdProvidesTree 
            Caption         =   "Provides"
            Height          =   300
            Left            =   1200
            TabIndex        =   14
            ToolTipText     =   "View what Get Data will provide"
            Top             =   240
            Width           =   852
         End
         Begin VB.CommandButton cmdCriteriaTree 
            Caption         =   "Criteria"
            Height          =   300
            Left            =   2280
            TabIndex        =   15
            ToolTipText     =   "View Criteria specified so far in this window"
            Top             =   240
            Width           =   852
         End
         Begin VB.CommandButton cmdStatusTree 
            Caption         =   "Status"
            Height          =   300
            Left            =   120
            TabIndex        =   13
            ToolTipText     =   "View current system state"
            Top             =   240
            Width           =   852
         End
      End
      Begin VB.CommandButton cmdDetails 
         Caption         =   "Details..."
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
         Left            =   2640
         TabIndex        =   11
         Top             =   120
         Width           =   1212
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
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
         Left            =   1320
         TabIndex        =   10
         Top             =   120
         Width           =   1212
      End
      Begin VB.CommandButton cmdGetData 
         Caption         =   "Download"
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
         TabIndex        =   9
         Top             =   120
         Width           =   1212
      End
   End
   Begin VB.Frame fra 
      Caption         =   "fra"
      Height          =   284
      Index           =   0
      Left            =   0
      TabIndex        =   1
      Top             =   600
      Visible         =   0   'False
      Width           =   9255
   End
   Begin VB.CommandButton cmdEdit 
      Caption         =   "Edit"
      Height          =   255
      Index           =   0
      Left            =   8520
      TabIndex        =   6
      Top             =   360
      Visible         =   0   'False
      Width           =   732
   End
   Begin VB.CommandButton cmdSet 
      Caption         =   "Get From"
      Height          =   255
      Index           =   0
      Left            =   3240
      TabIndex        =   4
      Top             =   360
      Visible         =   0   'False
      Width           =   972
   End
   Begin VB.ComboBox cbo 
      Height          =   288
      Index           =   0
      Left            =   4320
      TabIndex        =   3
      Text            =   "User"
      Top             =   360
      Visible         =   0   'False
      Width           =   1692
   End
   Begin VB.OptionButton opt 
      Height          =   252
      Index           =   0
      Left            =   240
      TabIndex        =   0
      Top             =   1320
      Visible         =   0   'False
      Width           =   3015
   End
   Begin VB.CheckBox Check1 
      Height          =   252
      Index           =   0
      Left            =   240
      TabIndex        =   7
      Top             =   1680
      Visible         =   0   'False
      Width           =   3135
   End
   Begin VB.TextBox txt 
      BackColor       =   &H8000000F&
      Height          =   285
      Index           =   0
      Left            =   6120
      Locked          =   -1  'True
      TabIndex        =   5
      Top             =   360
      Visible         =   0   'False
      Width           =   2292
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   6120
      Top             =   1560
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
   End
   Begin VB.Label lblInstructions 
      Caption         =   "Enter the following criteria, then click Ok when finished. Bold items are required."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   1
      Left            =   120
      TabIndex        =   16
      Top             =   120
      Width           =   9132
   End
   Begin VB.Label lbl 
      Height          =   252
      Index           =   0
      Left            =   240
      TabIndex        =   2
      Top             =   360
      Visible         =   0   'False
      Width           =   2892
   End
End
Attribute VB_Name = "frmCriteria"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Declare Function SetParent Lib "user32" (ByVal hWndChild As Long, ByVal hWndNewParent As Long) As Long

Private Const UserLabel = "User"

Private pDataType As clsWebData
Private pParent As frmCriteria
Private pManager As clsWebDataManager

Private pSubIndex As Long
Private lastY As Long
Private pValues() As IXMLDOMNodeList

Public Sub SubCriteriaComplete(Cancelled As Boolean, frm As frmCriteria)
  Dim lblIndex As Long
  Dim itemIndex As Long
  
  If Not Cancelled Then
    For lblIndex = 1 To lbl.Count - 1
      If lblIndex = frm.SubIndex Or Len(txt(lblIndex).Text) = 0 Then
        Set pValues(lblIndex) = Nothing
        Set pValues(lblIndex) = pManager.CurrentStatus.getElementsByTagName(lbl(lblIndex).Tag)
        With pValues(lblIndex)
          If .Length > 0 Then
            txt(lblIndex) = .Item(0).Text
            If .Length > 1 Then
              txt(lblIndex) = txt(lblIndex) & " and " & .Length - 1 & " more"
            End If
            setSetByComboBox pValues(lblIndex), lblIndex
          End If
        End With
      End If
    Next
  End If
  Me.Visible = True
End Sub

Public Property Set Manager(newValue As clsWebDataManager)
  Set pManager = newValue
End Property

Public Property Get SubIndex() As Long
  SubIndex = pSubIndex
End Property
Public Property Let SubIndex(newValue As Long)
  pSubIndex = newValue
End Property

Private Sub LoadGroup(groupName As String, caption As String, attrOptional As String)
  Load fra(fra.Count)
  With fra(fra.Count - 1)
    lastY = lastY + .Height
    .Tag = groupName
    .caption = caption 'Want better caption
    .Top = lastY
    .Left = 0
    .Width = Me.ScaleWidth - 32
    .Visible = True
    Select Case attrOptional
      Case "", "false": .FontBold = True
      Case Else:        .FontBold = False
    End Select
  End With
End Sub

Private Sub AddNode(nod As IXMLDOMElement, DataType As String)
  Dim lblIndex As Long
  Dim cboIndex As Long
  Dim grpIndex As Long
  Dim ctlTop As Single
  Dim c As Collection
  Dim lMatchNodeList As IXMLDOMNodeList
  Dim attrGroup As String
  Dim attrMultiple As Boolean
  Dim attrFormat As String
  Dim attrLabel As String
  Dim attrType As String
  Dim attrUser As Boolean
  Dim attrOptional As String
  
  On Error Resume Next
  attrGroup = LCase(nod.getAttribute("group"))
  attrOptional = LCase(nod.getAttribute("optional"))
  attrFormat = nod.getAttribute("format")
  attrLabel = nod.getAttribute("label")
  attrType = nod.getAttribute("type")
  If LCase(nod.getAttribute("multiple")) = "true" Then attrMultiple = True
  If LCase(nod.getAttribute("user")) = "false" Then attrUser = False Else attrUser = True
  On Error GoTo 0
  If Len(attrLabel) = 0 Then attrLabel = nod.nodeName
  
  If LCase(nod.nodeName) = "group" Then
    If Len(attrGroup) > 0 Then LoadGroup attrGroup, attrLabel, attrOptional
  Else
    lblIndex = lbl.Count
    Load lbl(lblIndex)
    Load opt(lblIndex)
    Load cbo(lblIndex)
    Load txt(lblIndex)
    Load cmdSet(lblIndex)
    Load cmdEdit(lblIndex)
    
    cbo(lblIndex).Enabled = True
    cmdSet(lblIndex).Enabled = True
    cmdEdit(lblIndex).Enabled = True

    If attrMultiple Then
      cmdEdit(lblIndex).caption = "Edit..."
    Else
      cmdEdit(lblIndex).caption = "Edit"
    End If
    
    ReDim Preserve pValues(lblIndex)

    lbl(lblIndex).caption = attrLabel
    lbl(lblIndex).Tag = nod.nodeName
    txt(lblIndex).ToolTipText = attrFormat

    If Len(attrGroup) = 0 Then 'does not belong to a group
      lastY = lastY + lbl(lblIndex).Height * 1.4
      ctlTop = lastY
      lbl(lblIndex).Top = ctlTop
      lbl(lblIndex).Visible = True
      opt(lblIndex).Tag = -1
      opt(lblIndex).Visible = False
      opt(lblIndex).caption = ""
    Else
      For grpIndex = 0 To fra.Count - 1
        If fra(grpIndex).Tag = attrGroup Then GoTo AddToGrpIndex
      Next
      LoadGroup attrGroup, attrGroup, ""
AddToGrpIndex:
      With fra(grpIndex)
        .Height = .Height + 368
        ctlTop = .Height - 412
        lastY = .Top + ctlTop + 120

      'SetParent lbl(lblIndex).hWnd, .hWnd
        SetParent opt(lblIndex).hwnd, .hwnd
        SetParent cbo(lblIndex).hwnd, .hwnd
        SetParent txt(lblIndex).hwnd, .hwnd
        SetParent cmdSet(lblIndex).hwnd, .hwnd
        SetParent cmdEdit(lblIndex).hwnd, .hwnd
      End With
      opt(lblIndex).Tag = grpIndex
      opt(lblIndex).Visible = True
      lbl(lblIndex).Visible = False
      opt(lblIndex).caption = lbl(lblIndex).caption
    End If
    
    Set c = pManager.Provides(nod.nodeName)
    For cboIndex = 1 To c.Count
      If c(cboIndex) <> DataType Then
        cbo(lblIndex).AddItem c(cboIndex)
      End If
'      'If c(cboIndex) <> DataType Then
'        Dim lParent As frmCriteria, lRecursive As Boolean
'        Set lParent = Me
'        lRecursive = False
'        While Not lParent Is Nothing And Not lRecursive
'          If lParent.caption = c(cboIndex) Then
'            lRecursive = True
'          Else
'            Set lParent = lParent.ParentCriteria
'          End If
'        Wend
'        If Not lRecursive Then
'          cbo(lblIndex).AddItem c(cboIndex)
'        End If
'      'End If
    Next
    If cbo(lblIndex).ListCount = 0 Or attrUser Then
      cbo(lblIndex).AddItem UserLabel
    End If
    cbo(lblIndex).ListIndex = 0
    If cbo(lblIndex).ListCount = 1 Then cbo(lblIndex).Enabled = False
    
    'Everything not in a group is assumed to be required
    'unless optional=true is an attribute of the node
    Select Case LCase(nod.getAttribute("optional"))
      Case "":
                  If Len(attrGroup) = 0 Then
                    lbl(lblIndex).FontBold = True
                  End If
      Case "true":  lbl(lblIndex).FontBold = False
      Case Else:    lbl(lblIndex).FontBold = True
    End Select
    
    If nod.getAttribute("filter") <> vbNull Then
      txt(lblIndex).Tag = nod.getAttribute("filter") 'for file types
    End If
    
    Set lMatchNodeList = pManager.CurrentStatus.getElementsByTagName(nod.nodeName)
    If lMatchNodeList.Length > 0 Then 'have a current value
      Set pValues(lblIndex) = lMatchNodeList
      txt(lblIndex).Text = lMatchNodeList(0).Text
      If Len(opt(lblIndex).caption) > 0 Then opt(lblIndex).Value = True
      If lMatchNodeList.Length > 1 Then
        txt(lblIndex).Text = txt(lblIndex).Text & " and " & lMatchNodeList.Length - 1 & " more"
      End If
      setSetByComboBox lMatchNodeList, lblIndex
    End If
    
    cmdEdit(lblIndex).Tag = attrFormat
    cbo(lblIndex).Top = ctlTop
    txt(lblIndex).Top = ctlTop
    cmdSet(lblIndex).Top = ctlTop
    cmdEdit(lblIndex).Top = ctlTop
    opt(lblIndex).Top = ctlTop
    cbo(lblIndex).Visible = cbo(lblIndex).Enabled
    txt(lblIndex).Visible = True
    cmdSet(lblIndex).Visible = cbo(lblIndex).Enabled
    cmdEdit(lblIndex).Visible = True
      
    Select Case attrFormat
      Case "openpath", "openfile", "savepath", "savefile"
        cmdEdit(lblIndex).caption = "Browse"
        cmdSet(lblIndex).Visible = False
        cbo(lblIndex).Visible = False
    End Select
    
    If Len(txt(lblIndex).Text) = 0 And cbo(lblIndex).Text = "BASINS Project" Then
      'GetValueFromProject lblIndex
      If Len(pManager.CurrentStatusGetString("project_dir")) > 0 Then
        ShowCriteria lblIndex, False
      End If
    End If
  End If
  fraButtons.Top = lastY + 300
End Sub

'Private Sub GetValueFromProject(lblIndex As Long)
'End Sub

Private Sub setSetByComboBox(nodeList As IXMLDOMNodeList, lIndex As Long)
  Dim lNode As IXMLDOMNode, s As String, j As Long
  
  Set lNode = nodeList.Item(0).Attributes.getNamedItem("status")
  If Left(lNode.Text, 6) = "set by" Then
    s = Right(lNode.Text, Len(lNode.Text) - 6)
    For j = 0 To cbo(lIndex).ListCount
      If Trim(s) = Trim(cbo(lIndex).List(j)) Then
        cbo(lIndex).ListIndex = j
        Exit For
      End If
    Next
  End If

End Sub

Public Property Set DataType(newDataType As clsWebData)
  'Dim vDataType As Variant
  Dim vCategory As Variant
  Dim vItem As Variant
  Dim nod As IXMLDOMElement
  Dim i As Integer
    
  Set pDataType = Nothing
  Set pDataType = newDataType
  
  lastY = 150
  
  Me.caption = newDataType.Label
  
  ReDim pValues(1)
  For Each vCategory In newDataType.Provides.documentElement.childNodes
    Select Case LCase(vCategory.nodeName)
      Case "criteria"
        For Each vItem In vCategory.childNodes
          Set nod = vItem
          AddNode nod, pDataType.Label
        Next
'      Case "available"
'        For Each vItem In vCategory.childNodes
'          Set nod = vItem
'          lstAvail.AddItem nod.nodeName
'          If nod.getAttribute("default") = "true" Then lstAvail.Selected(lstAvail.ListCount - 1) = True
'        Next
    End Select
  Next
  Me.Height = lastY + 1280
  'For i = 1 To cbo.Count - 1
  '  cbo_Click i
  'Next
End Property

Private Sub BuildQuery(ByRef QueryDoc As DOMDocument, ByRef StatusVarsToGet As Collection)
  Dim lQuery As DOMDocument
  Dim Requested As DOMDocument
  Dim lNod As IXMLDOMNode
  Dim s As String, s2 As String, n As String, i As Long, val As Variant, fraIndex As Long
  Dim missing As String
  Dim found As Boolean
  
  Set StatusVarsToGet = New Collection
  Set Requested = pDataType.Provides
  
  s = "<" & pDataType.Name & "><criteria>"
  For i = 1 To lbl.Count - 1
    If lbl(i).Visible Or opt(i).Value Then
      If Len(txt(i).Text) > 0 Then
        
        s2 = "<" & lbl(i).Tag & ">" & txt(i).Text & "</" & lbl(i).Tag & ">"
        If Not pValues(i) Is Nothing Then
          If pValues(i).Length > 0 Then
            s2 = ""
            For Each val In pValues(i)
              s2 = s2 & val.xml
            Next
          End If
        End If
        s = s & s2
        If opt(i).Value Then SaveSetting "WebDataManager", pDataType.Name, fra(CInt(opt(i).Tag)).Tag, s2

      ElseIf lbl(i).FontBold = True Then
        missing = missing & vbCr & "   " & lbl(i).caption
      End If
    End If
  Next
  
  For fraIndex = 1 To fra.Count - 1
    If fra(fraIndex).FontBold Then
      found = False
      For i = 1 To lbl.Count - 1
        If opt(i).Tag = fraIndex And opt(i).Value And Len(txt(i).Text) > 0 Then
          found = True
        End If
      Next
      If Not found Then missing = missing & vbCr & "   " & fra(fraIndex).caption
    End If
  Next
  
  If Len(missing) > 0 Then
    MsgBox "Required Criteria Not Set:" & vbCr & missing, vbOKOnly, "Cannot get data"
  Else
    s = s & "</criteria><requested>"
    With pManager.CurrentStatus.getElementsByTagName("status_variables")
      .Reset
      With .nextNode
        For i = 0 To .childNodes.Length - 1
          If .childNodes(i).Attributes.Length > 0 Then
            Set lNod = .childNodes(i).Attributes.getNamedItem("status")
            If InStr(lNod.nodeValue, caption) And Left(lNod.nodeValue, 4) = "requ" Then
              n = .childNodes(i).nodeName
              StatusVarsToGet.Add n
              s2 = Requested.getElementsByTagName(n).Item(0).parentNode.nodeName
              s = s & "<" & s2 & "><" & n & "/></" & s2 & ">"
            End If
          End If
        Next i
      End With
    End With
    s = s & "</requested></" & pDataType.Name & ">"
    Set lQuery = New DOMDocument
    lQuery.loadXML s
    Set QueryDoc = lQuery
  End If
End Sub

Private Sub cbo_Click(Index As Integer)
  If cbo(Index).Text = UserLabel _
    And InStr(txt(Index).ToolTipText, "path") = 0 _
    And InStr(txt(Index).ToolTipText, "file") = 0 Then
      cmdSet(Index).Enabled = False
      txt(Index).BackColor = cbo(Index).BackColor
      txt(Index).Locked = False
      If txt(Index).Visible Then txt(Index).SetFocus
      If Right(cmdEdit(Index).caption, 3) <> "..." Then cmdEdit(Index).Enabled = False
  Else
    cmdSet(Index).Enabled = True
    txt(Index).BackColor = Me.BackColor
    txt(Index).Locked = True
    cmdEdit(Index).Enabled = True
  End If
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdDetails_Click()
  fraDetails.Visible = Not fraDetails.Visible
End Sub

Private Sub cmdEdit_Click(Index As Integer)
  Edit Index
End Sub
Private Sub Edit(Index As Integer, Optional TryJustOne As Boolean = False)
  Dim lStatusSet As Boolean
  Dim lvalue As Variant ', i As Long
  Dim Filename As String
  
  If opt(Index).Visible Then opt(Index).Value = True
  lStatusSet = False
  Select Case cmdEdit(Index).Tag
    Case "openpath"
      Filename = BrowseFolder(Me, "Choose a '" & lbl(Index).caption & "' folder", txt(Index).Text, False)
      If Len(Filename) > 0 Then txt(Index).Text = Filename: Set pValues(Index) = Nothing
    Case "savepath"
      Filename = BrowseFolder(Me, "Choose a '" & lbl(Index).caption & "' folder", txt(Index).Text, True)
      If Len(Filename) > 0 Then txt(Index).Text = Filename: Set pValues(Index) = Nothing
    Case "openfile"
      cdlg.DialogTitle = "Open " & lbl(Index).caption
      If Len(txt(Index).Tag) = 0 Then
        cdlg.Filter = "All files|*.*"
      Else
        cdlg.Filter = txt(Index).Tag
      End If
      cdlg.ShowOpen
      txt(Index).Text = cdlg.Filename
      Set pValues(Index) = Nothing
    Case "savefile"
      cdlg.DialogTitle = "Save " & lbl(Index).caption
      cdlg.Filter = "All files|*.*"
      cdlg.ShowSave
      txt(Index).Text = cdlg.Filename
      Set pValues(Index) = Nothing
    Case Else
      If Right(cmdEdit(Index).caption, 3) = "..." And (Not pValues(Index) Is Nothing Or Not TryJustOne) Then
        Me.MousePointer = vbHourglass
        frmMultiple.Tag = lbl(Index).Tag
        Set frmMultiple.lst = pValues(Index)
        frmMultiple.caption = lbl(Index).caption
        frmMultiple.Show vbModal
        If Not frmMultiple.lst Is Nothing Then
          Set pValues(Index) = Nothing
          Set pValues(Index) = frmMultiple.lst
          Select Case pValues(Index).Length
            Case 0:    txt(Index).Text = ""
            Case 1:    txt(Index).Text = pValues(Index).Item(0).Text
            Case Else: txt(Index).Text = pValues(Index).Item(0).Text _
                                       & " and " & pValues(Index).Length - 1 & " more"
              'i = 0
              'For Each lvalue In pValues(index)
              pManager.CurrentStatusUpdateList lbl(Index).Tag, pValues(Index), "set by " & caption
              '  i = 1
              'Next lvalue
              lStatusSet = True
          End Select
        End If
        Unload frmMultiple
        Me.MousePointer = vbDefault
      Else
        txt(Index).BackColor = cbo(Index).BackColor
        txt(Index).Locked = False
        txt(Index).SetFocus
      End If
  End Select
  If Not (lStatusSet) Then
    pManager.CurrentStatusUpdateString lbl(Index).Tag, txt(Index), "set by " & caption
  End If
End Sub

Private Sub cmdGetData_Click()
  If Me.Visible Then cmdGetData.SetFocus
  GetData
End Sub

Public Sub GetData()
  Dim lQuery As DOMDocument
  Dim lResult As DOMDocument
  Dim lStatus As Boolean
  Dim vStatusVar As Variant
  Dim lStatusVarsToGet As Collection
  
  Me.MousePointer = vbHourglass
  Set lResult = New DOMDocument
  BuildQuery lQuery, lStatusVarsToGet

  If Not lQuery Is Nothing Then
    'DomStatus lQuery, pManager.StatusFile
    
    lStatus = pDataType.GetData(lQuery, lResult)
    'MsgBox lResult.xml
    If lStatus Then
      'update Status variables
      For Each vStatusVar In lStatusVarsToGet
        pManager.CurrentStatusUpdateList (vStatusVar), lResult.getElementsByTagName(vStatusVar), "set by " & caption
      Next
      'cmdTree_Click (1) 'results tree
      If Not pParent Is Nothing Then pParent.SubCriteriaComplete False, Me
      Unload Me
    End If
  End If
  Me.MousePointer = vbDefault
End Sub

'Private Sub cmdCriteriaTree_Click()
'  Dim Query As DOMDocument, Statusvars As Collection
'  BuildQuery Query, Statusvars
'  If Not Query Is Nothing Then pManager.ShowTree Query, "Criteria"
'End Sub

'Private Sub cmdProvidesTree_Click()
'  pManager.ShowTree pDataType.Provides, "Provides"
'End Sub

Private Sub cmdSet_Click(Index As Integer)
  If cbo(Index).Text = UserLabel Then 'user spec
    Edit Index, True
  Else
    ShowCriteria Index, True
  End If
End Sub

Private Sub ShowCriteria(ByVal Index As Integer, ByVal TryToShow As Boolean)
  Dim lCriteria As Form
  Dim lWebData As clsWebData
  
  If opt(Index).Visible Then opt(Index).Value = True
  
  Set lWebData = pManager.DataTypeFromLabel(cbo(Index).List(cbo(Index).ListIndex))
  
  pManager.CurrentStatusUpdateString lbl(Index).Tag, "", "requested from " & lWebData.Label
'  On Error GoTo JustUseTheTextbox
'  If pValues(Index).Length > 0 Then
'    pManager.CurrentStatusUpdateList lbl(Index).Tag, pValues(Index), "requested from " & lWebData.Label
'  Else
'JustUseTheTextbox:
'    pManager.CurrentStatusUpdateString lbl(Index).Tag, txt(Index), "requested from " & lWebData.Label
'  End If
'  On Error GoTo 0
  
  pManager.collFrmCriteria.Add lCriteria
  If TryToShow Then
    lWebData.Specify Me, Index
  Else
    Set lCriteria = New frmCriteria
    Set lCriteria.Manager = pManager
    Set lCriteria.DataType = lWebData
    Set lCriteria.ParentCriteria = Me
    lCriteria.SubIndex = Index
    lCriteria.GetData
  End If
  Set lCriteria = Nothing
End Sub

'Private Sub cmdStatusTree_Click()
'  pManager.ShowTree pManager.CurrentStatus, "Current Status"
'End Sub

Private Sub Form_Resize()
  Dim Index As Integer
  Dim h As Single
  Dim w As Single
  h = Me.ScaleHeight
  w = Me.ScaleWidth
  
  If w > (txt(0).Left + cmdEdit(0).Width + 300) Then
    For Index = 0 To txt.Count - 1
      If Index < fra.Count Then fra(Index).Width = w
      cmdEdit(Index).Left = w - cmdEdit(0).Width - 120
      txt(Index).Width = cmdEdit(Index).Left - txt(Index).Left - 120
    Next
  End If
  
'  If pCreateOutput Then
'    fraOutput.Top = lastY + 300
'    'fraOutput.Top = lbl(lbl.Count - 1).Top + 300
'    If h > fraOutput.Top + 700 Then
'      fraOutput.Height = h - fraOutput.Top - 300
'      lstAvail.Height = fraOutput.Height - 400
'    End If
'  End If
'  fraOutput.Visible = pCreateOutput
End Sub

Private Sub Form_Unload(Cancel As Integer)
  If Not pParent Is Nothing Then
    pParent.SubCriteriaComplete True, Me
  Else
    Dim i As Long
    On Error Resume Next
    For i = 1 To pManager.collFrmCriteria.Count
      If pManager.collFrmCriteria(i).caption = Me.caption Then
        pManager.collFrmCriteria.Remove i
      End If
    Next
  End If
End Sub

Private Sub txt_KeyPress(Index As Integer, KeyAscii As Integer)
  If opt(Index).Visible Then opt(Index).Value = True
End Sub

Private Sub txt_LostFocus(Index As Integer)
  If Not txt(Index).Locked Then
    If txt(Index).Tag = "save_dir" Then
      If Len(txt(Index).Text) > 0 Then
        If Right(txt(Index).Text, 1) <> "\" Then
          txt(Index).Text = txt(Index).Text & "\"
        End If
      End If
    End If
    pManager.CurrentStatusUpdateString lbl(Index).Tag, txt(Index), "set by " & caption
    Set pValues(Index) = Nothing
  End If
  If cbo(Index).Text <> UserLabel Then
    txt(Index).BackColor = Me.BackColor
    txt(Index).Locked = True
  End If
End Sub

Public Property Get ParentCriteria() As frmCriteria
  Set ParentCriteria = pParent
End Property
Public Property Set ParentCriteria(newValue As frmCriteria)
  Set pParent = newValue
End Property
