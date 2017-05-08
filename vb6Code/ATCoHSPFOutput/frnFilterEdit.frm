VERSION 5.00
Begin VB.Form frmFilterEdit 
   Caption         =   "HSPF Output Filter Edit"
   ClientHeight    =   4950
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   7215
   LinkTopic       =   "Form1"
   ScaleHeight     =   4950
   ScaleWidth      =   7215
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdOK 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Index           =   2
      Left            =   4080
      TabIndex        =   5
      Top             =   4440
      Width           =   1095
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&Apply"
      Height          =   375
      Index           =   1
      Left            =   2760
      TabIndex        =   4
      Top             =   4440
      Width           =   1095
   End
   Begin ATCoCtl.ATCoSelectList slsExclude 
      Height          =   3135
      Left            =   240
      TabIndex        =   3
      Top             =   840
      Width           =   6735
      _ExtentX        =   11880
      _ExtentY        =   5530
      RightLabel      =   "Exclude:"
      LeftLabel       =   "Include:"
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   375
      Index           =   0
      Left            =   1440
      TabIndex        =   2
      Top             =   4440
      Width           =   1095
   End
   Begin VB.ComboBox cboFilterType 
      Height          =   315
      Left            =   1560
      Style           =   2  'Dropdown List
      TabIndex        =   0
      Top             =   240
      Width           =   2295
   End
   Begin VB.Label lblFilter 
      Alignment       =   1  'Right Justify
      Caption         =   "Filter Type:"
      Height          =   255
      Left            =   240
      TabIndex        =   6
      Top             =   240
      Width           =   1215
   End
   Begin VB.Label lblValue 
      Caption         =   "Value"
      Height          =   495
      Left            =   5400
      TabIndex        =   1
      Top             =   120
      Width           =   1695
   End
End
Attribute VB_Name = "frmFilterEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Dim pParent As clsTSerFilter
Dim pCriteria As FastCollection 'local copy

Private Sub cboFilterType_Click()
  Dim pValues As CollString, vValue As Variant
  Dim vCriteria As Variant
  Dim lExclude As Boolean
  Dim s As String, i As Long
  
  Set pValues = New CollString
  Set pValues = uniqueAttributeValues(cboFilterType.List(cboFilterType.ListIndex), pParent.Parent.AllTser)
  s = pParent.Parent.DataCollection.Count & " " & pParent.Parent.AllTser.Count & " " & pValues.Count
  lblValue.Caption = s
  With slsExclude
    .ClearLeft
    .ClearRight
    For Each vValue In pValues
      lExclude = False
      For Each vCriteria In pParent.Criteria
        s = vCriteria
        If StrSplit(s, ":", "") = cboFilterType.List(cboFilterType.ListIndex) Then
          If s = vValue Then
            lExclude = True
          End If
        End If
      Next
      .LeftItemFastAdd vValue
      If lExclude Then
        .MoveRight (.LeftCount - 1)
      End If
    Next
  End With

End Sub

Private Sub cmdOK_Click(Index As Integer)
  Dim i As Long
  
  If Index <= 1 Then 'ok or apply
    With slsExclude
      For i = 1 To .RightCount
        pParent.changeCriteria cboFilterType.List(cboFilterType.ListIndex), .RightItem(i - 1)
      Next
      For i = 1 To .LeftCount
        pParent.changeCriteria cboFilterType.List(cboFilterType.ListIndex), .LeftItem(i - 1), False
      Next
    End With
    Set pParent.Parent.Tser = pParent.Filter(pParent.Parent.AllTser)
    cboFilterType_Click
  End If
  If Index Mod 2 = 0 Then 'ok or cancel
    If Index = 2 Then 'get old criteria
      Set pParent.Criteria = pCriteria
    End If
    Unload Me
  End If
End Sub

Public Property Set FilterTypes(newFilterTypes As Variant)
  Dim vFilterType As Variant
  
  With cboFilterType
    For Each vFilterType In newFilterTypes
      .AddItem vFilterType
    Next
    .ListIndex = 0
  End With
End Property

Public Property Set Parent(Parent As clsTSerFilter)
  Set pParent = Parent
  Set pCriteria = New FastCollection
  Set pCriteria = pParent.Criteria
End Property
