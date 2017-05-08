VERSION 5.00
Object = "{6B7E6392-850A-101B-AFC0-4210102A8DA7}#1.3#0"; "COMCTL32.OCX"
Begin VB.UserControl ATCoTree 
   ClientHeight    =   2385
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   4260
   ScaleHeight     =   2385
   ScaleWidth      =   4260
   Begin VB.CommandButton cmdDown 
      Caption         =   "Down"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   3480
      TabIndex        =   5
      Top             =   2040
      Width           =   735
   End
   Begin VB.CommandButton cmdUp 
      Caption         =   "Up"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   2160
      TabIndex        =   4
      Top             =   2040
      Width           =   735
   End
   Begin ComctlLib.TreeView tree 
      Height          =   2055
      Left            =   0
      TabIndex        =   3
      Top             =   240
      Width           =   2055
      _ExtentX        =   3625
      _ExtentY        =   3625
      _Version        =   327682
      Indentation     =   647
      LabelEdit       =   1
      Style           =   7
      ImageList       =   "iltree"
      Appearance      =   1
   End
   Begin VB.ListBox lstRight 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1695
      IntegralHeight  =   0   'False
      Left            =   2160
      TabIndex        =   0
      Top             =   240
      Width           =   2055
   End
   Begin ComctlLib.ImageList iltree 
      Left            =   1800
      Top             =   0
      _ExtentX        =   1005
      _ExtentY        =   1005
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   12632256
      _Version        =   327682
      BeginProperty Images {0713E8C2-850A-101B-AFC0-4210102A8DA7} 
         NumListImages   =   4
         BeginProperty ListImage1 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "ctlSelectFields.ctx":0000
            Key             =   ""
         EndProperty
         BeginProperty ListImage2 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "ctlSelectFields.ctx":031A
            Key             =   ""
         EndProperty
         BeginProperty ListImage3 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "ctlSelectFields.ctx":0634
            Key             =   ""
         EndProperty
         BeginProperty ListImage4 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "ctlSelectFields.ctx":094E
            Key             =   ""
         EndProperty
      EndProperty
   End
   Begin VB.Label lblRight 
      Caption         =   "Selected:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   2160
      TabIndex        =   2
      Top             =   0
      Width           =   1695
   End
   Begin VB.Label lblLeft 
      Caption         =   "Views:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   1695
   End
End
Attribute VB_Name = "ATCoTree"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
' Control: AtCoTree.oxc
' Control **************************************************************
' Name:        AtCoTree
' Purpose:     Selection control for picking a list of NWIS atributes.
' Author:      Mark Gray, AquaTerra
' Date:        8/1/98
'
' Notes:  The ATCoTree control provides the programmer a standard
'         method of obtaining a list of user selected NWIS field names
'         where the field names are organized in a hierarchial list
'         orgranized by database views.
'
'Dependencies: The ctlSelectFields control is dependent on the
'              microsoft common controls 5.0 (sp2) TreeView
'              control and ImageList control.
'
'Modifications:
'Date: 10/1/98  Author: Todd W. Augenstein, USGS
'Change: Added some documention.
'
'***********************************************************************
'Option Explicit
Private m_nViews As Long              ' Number of views.
Private m_maxFields As Long           ' Maximum number of fields.
Private m_Views()  As String          ' List of view names.
Private pnFields&()
Private pFields$()
Private pSelected() As Boolean
Private pCancelled As Boolean
Private pOkayed As Boolean
Private pOpened As Boolean
'
'
' Public Interface
'
' Subroutine ===============================================
' Name:      GetRightListIndices
' Purpose:
'
' Arguments:
'  (1) Inp  index  Integer -
'
'  (2) Inp  view   Integer -
'
'  (3) Inp  field  Integer -
'
' Notes: index = 1 to nSelected, view and field set according
'        to item in index postion in right list.
'
Public Sub GetRightListIndices(ByVal index&, view&, field&)
  Dim key As String
  key = lstRight.List(index - 1)
  view = CLng(Left(tree.Nodes(key).Tag, 5))
  field = CLng(Right(tree.Nodes(key).Tag, 5))
End Sub
'index = 1 to nSelected, view and field set according to item in index
'postiion in right list
Public Sub GetRightListStrings(ByVal index&, view$, field$)
  'index = 1 to nSelected, view and field set according to item in index postiion in right list
  Dim key As String, viewLng As Long
  key = lstRight.List(index - 1)
  viewLng = CLng(Left(tree.Nodes(key).Tag, 5))
  view = Views(viewLng)
  field = Fields(viewLng, CLng(Right(tree.Nodes(key).Tag, 5)))
End Sub
'  Method: Clear
' Purpose: Removes all the objects in the selection list (TreeView)
'          and the selected list (ListImage).
'  Syntax: object.Clear
'
' The object placeholder represents an object expression for a
' ctlSelectFields object.
'
Public Sub Clear()
  tree.Nodes.Clear
  lstRight.Clear
  ReDim m_Views(m_nViews)
  ReDim pnFields(m_nViews)
  ReDim pFields(m_nViews, m_maxFields)
  ReDim pSelected(m_nViews, m_maxFields)
  pCancelled = False
  pOkayed = False
  pOpened = False
End Sub
'
'  Method: InitLists
' Purpose: Assigns view and field nodes to the TreeView
'          by displaying the current field selection state
'          using the contents of the following properties:
'          Views, nViews, Fields, nFields, and Selected.
'  Syntax: object.InitLists
'
' The object placeholder represents an object expression for a
' ctlSelectFields object.
'
' Remarks:
'
' Set
' view -- Index used to loop through views.
' field -- Index used to loop through fields.
' viewName -- Temp variable to hold the current views name.
Public Sub InitLists()
  Dim view As Long, field As Long
  Dim nod As Node
  Dim viewName As String, tagView As String
  Dim tagField As String, key As String
  For view = 1 To m_nViews
    tagView = view
    tagView = SPACE(5 - Len(tagView)) & tagView
    viewName = m_Views(view)
    Set nod = tree.Nodes.add(, , viewName, viewName)
    nod.Tag = tagView & "    0"
    nod.Image = 1
    nod.ExpandedImage = 2
    For field = 1 To pnFields(view)
      tagField = field
      tagField = SPACE(5 - Len(tagField)) & tagField
      key = viewName & " - " & pFields(view, field)
      Set nod = tree.Nodes.add(viewName, tvwChild, key, pFields(view, field))
      nod.Selected = pSelected(view, field)
      If nod.Selected Then
        lstRight.AddItem key
        nod.Image = 3
      Else
        nod.Image = 4
      End If
      nod.Tag = tagView & tagField
    Next field
  Next view
End Sub

'
' An integer specifying the number of views displayed as the base
' nodes of the TreeView.
'
Public Property Let nViews(newvalue As Long)
  m_nViews = newvalue
End Property


Public Property Get nViews&()
  nViews = m_nViews
End Property
' An integer specifying the maximum number of fields that can be set
' for a given view.
Public Property Let maxFields(newvalue As Long)
  m_maxFields = newvalue
End Property
' An array of strings containing the view names.
'   index - an integer point to a single view name.
Public Property Get Views$(I As Long)
  Views = m_Views(I)
End Property

Public Property Let Views(I As Long, newvalue$)
  m_Views(I) = newvalue
End Property
'An array of the total number of field names in each view.
'   view -- an integer index representing one of the view names.
Public Property Get nFields&(view As Long)
  nFields = pnFields(view)
End Property

Public Property Let nFields(view&, newvalue&)
  pnFields(view) = newvalue
End Property
'A double array of strings used to indentify a field name for a view.
'   View -- integer index representing one of the view names.
'   Field -- integer index representing a field of a view.
Public Property Get Fields$(view&, field&)
  Fields = pFields(view, field)
End Property

Public Property Let Fields(view&, field&, newvalue$)
  pFields(view, field) = newvalue
End Property
'A double array of boolean flags used to indentify if a field has been
'selected.
'   True - field is selected
'   False - field has not been selected
'
'   View -- integer index representing one of the view names.
'   Field -- integer index representing a field of a view.
Public Property Get Selected(view&, field&) As Boolean
  Selected = pSelected(view, field)
End Property

Public Property Let Selected(view&, field&, newvalue As Boolean)
  pSelected(view, field) = newvalue
End Property
' Property + + + + + + + + + + + + + + + + + + + + + + + + +
' Name:    nSelected
' Purpose: The number of selected items displayed in the
'          right ImageList.
'
Public Property Get nSelected&()
  nSelected = lstRight.ListCount
End Property
'
'
' Private Routines
'
Private Sub cmdDown_Click()
  Dim tmp As String
  With lstRight
    If .ListIndex > -1 And .ListIndex < .ListCount - 1 Then
      tmp = .List(.ListIndex)
      .List(.ListIndex) = .List(.ListIndex + 1)
      .List(.ListIndex + 1) = tmp
      .ListIndex = .ListIndex + 1
    End If
  End With
End Sub

Private Sub cmdUp_Click()
  Dim tmp As String
  With lstRight
    If .ListIndex > 0 Then
      tmp = .List(.ListIndex)
      .List(.ListIndex) = .List(.ListIndex - 1)
      .List(.ListIndex - 1) = tmp
      .ListIndex = .ListIndex - 1
    End If
  End With
End Sub

Private Sub lstRight_DblClick() 'remove item from right list (de-select)
  Dim lri As Long, key As String
  Dim view As Long, field As Long
  lri = lstRight.ListIndex
  key = lstRight.List(lri)
  tree.Nodes(key).Image = 4
  view = CLng(Left(tree.Nodes(key).Tag, 5))
  field = CLng(Right(tree.Nodes(key).Tag, 5))
  pSelected(view, field) = False
  lstRight.RemoveItem lri
End Sub

Private Sub lstRight_KeyDown(KeyCode As Integer, Shift As Integer)
  If lstRight.ListIndex >= 0 And (KeyCode = vbKeyDelete Or KeyCode = vbKeyBack) Then
    lstRight_DblClick 'remove item from right list (de-select)
  End If
End Sub

Private Sub tree_NodeClick(ByVal Node As ComctlLib.Node)
  Dim view As Long, field As Long, I As Long, found As Boolean, key As String
  view = CLng(Left(Node.Tag, 5))
  field = CLng(Right(Node.Tag, 5))
  If field = 0 Then 'it is a view, not a field
    Node.Expanded = Not Node.Expanded
  Else
    pSelected(view, field) = Not pSelected(view, field)
    key = Node.key
    If pSelected(view, field) Then
      lstRight.AddItem key
      Node.Image = 3
    Else
      I = 0
      found = False
      While I < lstRight.ListCount And Not found
        If lstRight.List(I) = key Then found = True Else I = I + 1
      Wend
      If found Then lstRight.RemoveItem I
      Node.Image = 4
    End If
  End If
End Sub

Private Sub UserControl_Initialize()
  m_maxFields = 100
  m_nViews = 100
End Sub

Private Sub UserControl_Resize()
  tree.Width = Width / 2 - 75
  lstRight.Width = tree.Width
  lstRight.Left = tree.Width + 150
  lblRight.Left = lstRight.Left
  cmdUp.Left = lstRight.Left
  cmdDown.Left = cmdUp.Left + lstRight.Width - cmdDown.Width
  If Height > lblLeft.Height Then
    tree.Height = Height - lblLeft.Height
    cmdUp.Top = tree.Top + tree.Height - cmdUp.Height
    cmdDown.Top = cmdUp.Top
    If cmdUp.Top - 150 > lstRight.Top Then
      lstRight.Height = cmdUp.Top - lstRight.Top - 105
      lstRight.Visible = True
    Else
      lstRight.Visible = False
    End If
  End If
End Sub
