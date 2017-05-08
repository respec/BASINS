VERSION 5.00
Begin VB.Form TSEdit 
   Caption         =   "Edit Time Series Attributes"
   ClientHeight    =   3450
   ClientLeft      =   1080
   ClientTop       =   1380
   ClientWidth     =   7230
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   955
   Icon            =   "TSEdit.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   3450
   ScaleWidth      =   7230
   Begin ATCoCtl.ATCoGrid grdTSEdit 
      Height          =   2772
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6972
      _ExtentX        =   12303
      _ExtentY        =   4895
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   3
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
      SelectionMode   =   0
      BackColor       =   -2147483633
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483637
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.Frame fraBottom 
      BorderStyle     =   0  'None
      Caption         =   "fraButtons"
      Height          =   372
      Left            =   120
      TabIndex        =   4
      Top             =   3000
      Width           =   6972
      Begin VB.CommandButton cmdAddAtt 
         Caption         =   "&Add Attribute"
         Height          =   372
         Left            =   2640
         TabIndex        =   3
         ToolTipText     =   "New Attribute"
         Top             =   0
         Width           =   1572
      End
      Begin VB.CommandButton cmdOkay 
         Caption         =   "&OK"
         Default         =   -1  'True
         Height          =   372
         Left            =   0
         TabIndex        =   1
         Top             =   0
         Width           =   1092
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "&Cancel"
         Height          =   372
         Left            =   1320
         TabIndex        =   2
         Top             =   0
         Width           =   1092
      End
      Begin VB.Label lblCantEditAtt 
         Caption         =   "This attribute cannot be edited"
         Height          =   252
         Left            =   4320
         TabIndex        =   5
         Top             =   100
         Visible         =   0   'False
         Width           =   3132
      End
   End
End
Attribute VB_Name = "TSEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Private lTs As Collection 'of tserdata
Private AvlAtts As Collection 'of ATCclsAttributeDefinition
Private pNotify As Object
Private Const EditableColor = vbWindowBackground

'pNotify.RaiseEdit will be called when attributes have been edited
Public Property Set Notify(newvalue As Object)
  Set pNotify = newvalue
End Property

Public Property Set TimeseriesToEdit(newvalue As Collection)

  Dim i&, k&, newAtt As Boolean
  Dim vAttr As Variant, lAttr As ATTimSerAttribute
  Set AvlAtts = Nothing
  Set lTs = newvalue
  'Set lAtts = New Collection
  With grdTSEdit
    .Rows = 7 'This will be increased automatically as attributes are added
    .cols = lTs.Count + 1
    .TextMatrix(0, 0) = "Attribute"
    .TextMatrix(1, 0) = "ID/DSN"
    .TextMatrix(2, 0) = "Scenario"
    .TextMatrix(3, 0) = "Location"
    .TextMatrix(4, 0) = "Constituent"
    .TextMatrix(5, 0) = "Description"
    .TextMatrix(6, 0) = "Time Units"
    .TextMatrix(7, 0) = "Time Step"
    For i = 1 To lTs.Count
      'put header info in grid
      .ColEditable(i) = True
      .ColAlignment(i) = 1 'left
      .ColTitle(i) = "DSN " & lTs(i).Header.ID
      .col = i
      .row = 1: .text = lTs(i).Header.ID:   .CellBackColor = EditableColor
      .row = 2: .text = lTs(i).Header.sen:  .CellBackColor = EditableColor
      .row = 3: .text = lTs(i).Header.loc:  .CellBackColor = EditableColor
      .row = 4: .text = lTs(i).Header.con:  .CellBackColor = EditableColor
      .row = 5: .text = lTs(i).Header.desc: .CellBackColor = EditableColor
      If lTs(i).dates.Summary.CIntvl Then
        Select Case lTs(i).dates.Summary.Tu
          Case TUSecond:  .TextMatrix(6, i) = "Second (" & lTs(i).dates.Summary.Tu & ")"
          Case TUMinute:  .TextMatrix(6, i) = "Minute (" & lTs(i).dates.Summary.Tu & ")"
          Case TUHour:    .TextMatrix(6, i) = "Hour (" & lTs(i).dates.Summary.Tu & ")"
          Case TUDay:     .TextMatrix(6, i) = "Day (" & lTs(i).dates.Summary.Tu & ")"
          Case TUMonth:   .TextMatrix(6, i) = "Month (" & lTs(i).dates.Summary.Tu & ")"
          Case TUYear:    .TextMatrix(6, i) = "Year (" & lTs(i).dates.Summary.Tu & ")"
          Case TUCentury: .TextMatrix(6, i) = "Century (" & lTs(i).dates.Summary.Tu & ")"
          Case Else:      .TextMatrix(6, i) = lTs(i).dates.Summary.Tu
        End Select
        .TextMatrix(7, i) = lTs(i).dates.Summary.ts
      Else
        .TextMatrix(6, i) = "Non-constant interval"
        .TextMatrix(7, i) = ""
      End If
      'now put other attribute info in grid
      For Each vAttr In lTs(i).Attribs
        lAttr = vAttr
        newAtt = True
        For k = 8 To .Rows
          If lAttr.Name = .TextMatrix(k, 0) Then
            newAtt = False
            .row = k
          End If
        Next k
        If newAtt Then
          .Rows = .Rows + 1
          .TextMatrix(.Rows, 0) = lAttr.Name
          .row = .Rows
        End If
        .text = lAttr.value
        If Not lAttr.Definition Is Nothing Then
          If lAttr.Definition.Editable Then
            .CellBackColor = EditableColor
          End If
        End If
      Next
    Next i
  End With

End Property

Private Sub cmdAddAtt_Click()
  Dim col&
  MousePointer = ccHourglass
  With grdTSEdit
    .Rows = .Rows + 1
    .row = .Rows
    While Not .RowIsVisible(.Rows)
      .TopRow = .TopRow + 1
    Wend
    For col = 0 To .cols - 1
      .col = col
      .CellBackColor = EditableColor
      .ColEditable(.col) = True
    Next
    .col = 0 'SetGridAttributeNames
  End With
  MousePointer = ccDefault
End Sub

Private Sub SetGridAttributeNames()
  Dim i As Long, j As Long
  If AvlAtts Is Nothing Then 'available attributes not read yet
    Set AvlAtts = New Collection
    i = 0
    Do While AvlAtts.Count = 0 And i < lTs.Count
      i = i + 1
      Set AvlAtts = lTs(i).File.AvailableAttributes
    Loop
  End If
  If AvlAtts.Count > 0 Then 'put available attributes in list
    For i = 1 To AvlAtts.Count
      If InStr(AvlAtts(i).Name, "Dummy") = 0 Then
        Select Case AvlAtts(i).Name
          Case "IDSCEN", "IDLOCN", "IDCONS", "STANAM"
            'Don't add these attributes, they are already available with other names
          Case Else
            For j = 1 To grdTSEdit.Rows
              If AvlAtts(i).Name = grdTSEdit.TextMatrix(j, 0) Then 'already have this attribute, don't make it available
                j = grdTSEdit.Rows + 2
              End If
            Next
            If j = grdTSEdit.Rows + 1 Then 'not already available, add to list
              grdTSEdit.addValue AvlAtts(i).Name
            End If
        End Select
      End If
    Next i
    grdTSEdit.ComboCheckValidValues = True
  End If
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOkay_Click()
  Dim i&, k&, s$, saval$, lid&
  Dim UpdateID As Boolean, GoodWrite As Boolean
  Dim vAttr As Variant, lAttr As ATTimSerAttribute
  Dim oldHeader As ATTimSerDataHeader
  Dim rsp As VbMsgBoxResult
  Dim saveAtts As Collection
  Dim swapAtts As Collection
  Dim NewAttrDef As ATCclsAttributeDefinition
  
  'cmdOkay.SetFocus 'Make sure grid has lost focus so pending change has been made
  'DoEvents
  
  rsp = vbYes
  For i = 1 To lTs.Count
    If Len(Trim(grdTSEdit.TextMatrix(1, i))) = 0 Or _
       Len(Trim(grdTSEdit.TextMatrix(2, i))) = 0 Or _
       Len(Trim(grdTSEdit.TextMatrix(3, i))) = 0 Then
      'changing attributes to blank, are you sure?
      rsp = MsgBox("Some Scenario, Location, and Constituent values are blank in column " & i & "." & vbCrLf & _
                    "Scenario, Location, and Constituent values help in selecting Time Series with which to work." & vbCrLf & _
                    "Are you sure you want to change this time series attribute to blank?", 4, "Edit Time Series List")
    End If
  Next i
  If rsp = vbYes Then
    For i = 1 To lTs.Count
      Set oldHeader = lTs(i).Header.Copy
      lid = grdTSEdit.TextMatrix(1, i)
      If lid <> lTs(i).Header.ID Then 'id/dsn changed, make copy of old header
        lTs(i).Header.ID = lid
        UpdateID = True
      Else
        UpdateID = False
      End If
      lTs(i).Header.sen = grdTSEdit.TextMatrix(2, i)
      lTs(i).Header.loc = grdTSEdit.TextMatrix(3, i)
      lTs(i).Header.con = grdTSEdit.TextMatrix(4, i)
      lTs(i).Header.desc = grdTSEdit.TextMatrix(5, i)
      For k = 8 To grdTSEdit.Rows
        saval = Trim(grdTSEdit.TextMatrix(k, i))
        If Len(saval) > 0 Then 'store attribute value
          s = grdTSEdit.TextMatrix(k, 0)
          On Error GoTo NewAttribute
          lAttr = lTs(i).Attribs(s)
          If lAttr.value <> saval Then
            Debug.Print "Changing attribute " & s & " from " & lAttr.value & " to " & saval
            lTs(i).AttribSet s, saval, lAttr.Definition ', lAtts(k - 7)
          End If
        End If
      Next k
      
      Set saveAtts = New Collection
      Set swapAtts = New Collection
      For k = 1 To lTs(i).Attribs.Count
        lAttr = lTs(i).Attribs(k)
        swapAtts.Add lAttr
        If lAttr.Definition Is Nothing Then
          'don't save it
        ElseIf lAttr.Definition.DataType = NONE Then
          'don't save it
        ElseIf lAttr.Definition.Editable = False Then
          'don't save it
        Else
          saveAtts.Add lAttr
        End If
      Next
      
      If saveAtts.Count > 0 Then
        Set lTs(i).Attribs = saveAtts
        
        If UpdateID Then
          GoodWrite = lTs(i).File.WriteDataHeader(lTs(i), oldHeader)
        Else
          GoodWrite = lTs(i).File.WriteDataHeader(lTs(i))
        End If
      Else
        GoodWrite = True
      End If
      
      Set lTs(i).Attribs = swapAtts
      Set saveAtts = Nothing
      Set swapAtts = Nothing
      
      If Not GoodWrite Then
        s = lTs(i).File.ErrorDescription
        MsgBox "Error Updating Header for " & lTs(i).Header.ID & vbCrLf & s
        Set lTs(i).Header = oldHeader
        Exit Sub
      End If
    Next i
    If Not pNotify Is Nothing Then pNotify.RaiseEdit
  End If
  
  Unload Me
  Exit Sub
  
NewAttribute:
  Debug.Print "Adding attribute " & s & " = " & saval
  Set NewAttrDef = New ATCclsAttributeDefinition
  NewAttrDef.DataType = ATCoTxt
  NewAttrDef.Editable = True
  NewAttrDef.Name = s
  lTs(i).AttribSet s, saval, NewAttrDef
  Set NewAttrDef = Nothing
  Resume
End Sub

Private Sub Form_Load()
  Set Notify = Nothing
End Sub

Private Sub Form_Resize()
  If Height > 1000 And Width > 500 Then
    fraBottom.Top = Height - 780
    grdTSEdit.Height = Height - 1000
    grdTSEdit.Width = Width - 360
  End If
End Sub

Private Sub grdTSEdit_RowColChange()
  Dim ValidValues As String
  Dim AttDef As ATCclsAttributeDefinition
  With grdTSEdit
    .ClearValues
    If .CellBackColor <> EditableColor Then
      .ColEditable(.col) = False
    Else
      .ColEditable(.col) = True
      If .col = 0 Then 'In column of attribute names
        SetGridAttributeNames
      Else
        On Error GoTo NoAttributeDefinition
        Set AttDef = lTs(.col).Attribs(.TextMatrix(.row, 0)).Definition
        ValidValues = AttDef.ValidValues
        While Len(ValidValues) > 0
          .addValue StrRetRem(ValidValues)
        Wend
        .ComboCheckValidValues = True
      End If
    End If
  End With
NoAttributeDefinition:
End Sub
