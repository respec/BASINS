VERSION 5.00
Begin VB.Form frmPreferredUnits 
   Caption         =   "Preferred Display Units"
   ClientHeight    =   4800
   ClientLeft      =   60
   ClientTop       =   285
   ClientWidth     =   3795
   LinkTopic       =   "Form1"
   ScaleHeight     =   4800
   ScaleWidth      =   3795
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   1455
      Left            =   360
      TabIndex        =   1
      Top             =   3240
      Width           =   3135
      Begin VB.CheckBox chkRequired 
         Caption         =   "&Require units for all data"
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
         Left            =   120
         TabIndex        =   2
         Top             =   0
         Width           =   2535
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
         Left            =   1440
         TabIndex        =   6
         Top             =   960
         Width           =   1455
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "&OK"
         Default         =   -1  'True
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
         Top             =   960
         Width           =   1335
      End
      Begin VB.CommandButton cmdGetDefaults 
         Caption         =   "&Get Defaults"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   0
         TabIndex        =   3
         ToolTipText     =   "Retrieve defaults units from registry"
         Top             =   480
         Width           =   1335
      End
      Begin VB.CommandButton cmdSetDefaults 
         Caption         =   "&Save Defaults"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   1440
         TabIndex        =   4
         ToolTipText     =   "Save selected units as defaults in registry"
         Top             =   480
         Width           =   1455
      End
   End
   Begin ATCoCtl.ATCoGrid agd 
      Height          =   3135
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   3735
      _ExtentX        =   6588
      _ExtentY        =   5530
      SelectionToggle =   0   'False
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   32
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
      FixedRows       =   0
      FixedCols       =   1
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
End
Attribute VB_Name = "frmPreferredUnits"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public Sub SetFromProject()
  Dim PreferredUnitIndex As Long
  Dim UnitCategory As String
  Dim UnitCategoryIndex As Long
  Dim UnitName As String
  Dim UnitIndex As Long
  Dim UnitsOfType As FastCollection
  Dim vUnits As Variant
  Dim UnitCategories As FastCollection
  Set UnitCategories = GetAllUnitCategories
  agd.Rows = 0
  agd.Rows = UnitCategories.Count
  For UnitCategoryIndex = 1 To UnitCategories.Count
    UnitCategory = UnitCategories.ItemByIndex(UnitCategoryIndex)
    agd.TextMatrix(UnitCategoryIndex, 0) = UnitCategory
    PreferredUnitIndex = p.PreferredUnits.IndexFromKey(UnitCategory)
    If PreferredUnitIndex > 0 Then
      UnitName = p.PreferredUnits.ItemByIndex(PreferredUnitIndex)
    End If
    If UnitName <> "<none>" Then
      Set UnitsOfType = GetAllUnitsInCategory(UnitCategory)
      For UnitIndex = 1 To UnitsOfType.Count
        If UnitsOfType.ItemByIndex(UnitIndex) = UnitName Then GoTo UnitNameOk
      Next
      UnitName = "<none>" 'If we didn't know the default units, don't display them
      Set UnitsOfType = Nothing
    End If
UnitNameOk:
    agd.TextMatrix(UnitCategoryIndex, 1) = UnitName
  Next
  agd.ColEditable(1) = True
  agd.ComboCheckValidValues = True
  
  If p.UnitsRequired Then
    chkRequired.value = vbChecked
  Else
    chkRequired.value = vbUnchecked
  End If

End Sub

Private Sub agd_RowColChange()
  Dim UnitsOfType As FastCollection
  Dim vUnits As Variant
  Me.MousePointer = vbHourglass
  agd.ClearValues
  Set UnitsOfType = GetAllUnitsInCategory(agd.TextMatrix(agd.row, 0))
  agd.addValue "<none>"
  For Each vUnits In UnitsOfType
    agd.addValue CStr(vUnits)
  Next
  Me.MousePointer = vbDefault
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdGetDefaults_Click()
  Dim UnitIndex As Long
  Dim UnitCategory As String
  
  For UnitIndex = 1 To agd.Rows
    UnitCategory = agd.TextMatrix(UnitIndex, 0)
    agd.TextMatrix(UnitIndex, 1) = GetSetting("Aqua Terra Units", "Preferred Units", UnitCategory, "<none>")
  Next
End Sub

Private Sub cmdOk_Click()
  Dim UnitCategory As String
  Dim UnitName As String
  Dim UnitIndex As Long
  Dim OldUnitIndex As Long
  Dim Changed As Boolean
  
  Changed = False
  
  'First try to figure out whether actual changes have happened, so we don't set p.EditFlg unnecessarily
  If chkRequired.value = vbChecked And p.UnitsRequired = False Or _
     chkRequired.value = vbUnchecked And p.UnitsRequired = True Then
    Changed = True
  Else
    For UnitIndex = 1 To agd.Rows
      UnitCategory = agd.TextMatrix(UnitIndex, 0)
      UnitName = agd.TextMatrix(UnitIndex, 1)
      If Len(UnitCategory) > 0 Then
        OldUnitIndex = p.PreferredUnits.IndexFromKey(UnitCategory)
        If Len(UnitName) > 0 And UnitName <> "<none>" Then
          If OldUnitIndex <= 0 Then 'Did not used to have preferred units for this type, but now do
            Changed = True: Exit For
          ElseIf p.PreferredUnits.ItemByIndex(OldUnitIndex) <> UnitName Then
            Changed = True: Exit For 'Used to have different preferred units for this type
          End If
        Else
          If OldUnitIndex <> 0 Then 'Used to have preferred units for this type, but don't any more
            Changed = True: Exit For
          End If
        End If
      End If
    Next
  End If
  
  If Changed Then
    If chkRequired.value = vbChecked Then
      p.UnitsRequired = True
    Else
      p.UnitsRequired = False
    End If
    p.PreferredUnits.clear
    For UnitIndex = 1 To agd.Rows
      UnitCategory = agd.TextMatrix(UnitIndex, 0)
      UnitName = agd.TextMatrix(UnitIndex, 1)
      If Len(UnitCategory) > 0 And Len(UnitName) > 0 And UnitName <> "<none>" Then
        p.PreferredUnits.Add UnitName, UnitCategory
      End If
    Next
    p.EditFlg = True
  End If
  
  Unload Me
End Sub

Private Sub cmdSetDefaults_Click()
  Dim UnitIndex As Long
  Dim UnitCategory As String
  Dim UnitName As String
  
  For UnitIndex = 1 To agd.Rows
    UnitCategory = agd.TextMatrix(UnitIndex, 0)
    UnitName = agd.TextMatrix(UnitIndex, 1)
    If Len(UnitCategory) > 0 And Len(UnitName) > 0 And UnitName <> "<none>" Then
      SaveSetting "Aqua Terra Units", "Preferred Units", UnitCategory, UnitName
    End If
  Next

End Sub

Private Sub Form_Resize()
  Dim w As Long
  Dim h As Long
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > 1000 And h > fraButtons.Height + 220 Then
    fraButtons.Left = (w - fraButtons.Width) / 2
    fraButtons.Top = h - fraButtons.Height - 108
    agd.Width = w - 108
    agd.Height = fraButtons.Top - 108
  End If
End Sub
