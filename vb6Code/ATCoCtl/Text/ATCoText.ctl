VERSION 5.00
Begin VB.UserControl ATCoText 
   ClientHeight    =   270
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   2310
   ScaleHeight     =   270
   ScaleWidth      =   2310
   ToolboxBitmap   =   "ATCoText.ctx":0000
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   1800
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.TextBox Text1 
      Alignment       =   1  'Right Justify
      Height          =   288
      Left            =   0
      TabIndex        =   0
      Text            =   "Text1"
      Top             =   0
      Width           =   2292
   End
   Begin VB.Menu mnuText 
      Caption         =   "mnuText"
      Begin VB.Menu mnuX 
         Caption         =   "Min"
         Index           =   1
      End
      Begin VB.Menu mnuX 
         Caption         =   "Max"
         Index           =   2
      End
   End
End
Attribute VB_Name = "ATCoText"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Public Enum ATCoDataType
  ATCoTxt = 0
  ATCoInt = 1
  ATCoSng = 2
  ATCoClr = 10
  NONE = -999
End Enum

Private privDataType As ATCoDataType

Private DefVal As Variant
Private privHardMax!
Private privHardMin!
Private privSoftMax!
Private privSoftMin!
Private privMaxWidth&
Private privMaxDecimal&

Private fgColor As OLE_COLOR
Private OkBg As OLE_COLOR
Private OutsideSoftBg As OLE_COLOR
Private OutsideHardBg As OLE_COLOR

Private Const slop = 1.0001 'real numbers are screwy, so this is the slop factor

Public Event Change()

Public Event CommitChange()

Public Event KeyDown(KeyCode As Integer, Shift As Integer)

'VbLeftJustify, VbRightJustify, or VbCenter
Public Property Get Alignment() As AlignmentConstants
  Alignment = Text1.Alignment
End Property

Public Property Let Alignment(NewValue As AlignmentConstants)
  Select Case NewValue
    Case vbLeftJustify, vbRightJustify, vbCenter: Text1.Alignment = NewValue
  End Select
End Property

Public Property Get DataType() As ATCoDataType
  DataType = privDataType
End Property

Public Property Let DataType(ByVal NewValue As ATCoDataType)
  privDataType = NewValue
End Property

Public Property Get Font() As StdFont
  Set Font = Text1.Font
End Property

Public Property Let Font(ByVal NewValue As StdFont)
  Set Text1.Font = NewValue
End Property

Public Property Let HardMax(ByVal NewValue!)
  DbgMsg "HardMax:Let:" & NewValue, 8, "ATCoText", "p"
  privHardMax = NewValue
End Property

Public Property Get HardMax!()
  HardMax = privHardMax
End Property

Public Property Let HardMin(ByVal NewValue!)
  DbgMsg "HardMin:Let:" & NewValue, 8, "ATCoText", "p"
  'If we store a more precise value for HardMin than we
  'allow to be typed, it causes problems.
  Dim FormattedNewValue$, ch&, chind&, DecimalPlace&
  FormattedNewValue = FormatValue(NewValue)
  privHardMin = CSng(FormattedNewValue)
  If privHardMin - NewValue > 0.0000001 Then 'I hate floating point numbers
    DecimalPlace = InStr(FormattedNewValue, "E-")
    If DecimalPlace > 0 Then
      privHardMin = privHardMin + CSng("0." & String(Mid(FormattedNewValue, DecimalPlace + 2) - 1, "0") & "1")
    Else
      DecimalPlace = InStr(FormattedNewValue, ".")
      If DecimalPlace = 0 Then
        privHardMin = privHardMin + 1
      Else
        privHardMin = privHardMin + CSng("0." & String(Len(FormattedNewValue) - DecimalPlace - 1, "0") & "1")
      End If
    End If
  End If
End Property

Public Property Get HardMin!()
  HardMin = privHardMin
End Property

Public Property Let SoftMax(ByVal NewValue!)
  DbgMsg "SoftMax:Let:" & NewValue, 8, "ATCoText", "p"
  privSoftMax = NewValue
End Property

Public Property Get SoftMax!()
  SoftMax = privSoftMax
End Property

Public Property Let SoftMin(ByVal NewValue!)
  DbgMsg "SoftMin:Let:" & NewValue, 8, "ATCoText", "p"
  privSoftMin = NewValue
End Property

Public Property Get SoftMin!()
  SoftMin = privSoftMin
End Property

Public Property Let InsideLimitsBackground(ByVal newColor As OLE_COLOR)
  OkBg = newColor
End Property

Public Property Get InsideLimitsBackground() As OLE_COLOR
  InsideLimitsBackground = OkBg
End Property

Public Property Let OutsideSoftLimitBackground(ByVal newColor As OLE_COLOR)
  OutsideSoftBg = newColor
End Property

Public Property Get OutsideSoftLimitBackground() As OLE_COLOR
  OutsideSoftLimitBackground = OutsideSoftBg
End Property

Public Property Let OutsideHardLimitBackground(ByVal newColor As OLE_COLOR)
  OutsideHardBg = newColor
End Property

Public Property Get OutsideHardLimitBackground() As OLE_COLOR
  OutsideHardLimitBackground = OutsideHardBg
End Property

Public Property Let maxWidth(ByVal NewValue&)
  DbgMsg "MaxWidth:Let:" & NewValue, 8, "ATCoText", "p"
  privMaxWidth = NewValue
End Property

Public Property Get maxWidth&()
  maxWidth = privMaxWidth
End Property

Public Property Let MaxDecimal(ByVal NewValue&)
  DbgMsg "MaxDecimal:Let:" & NewValue, 8, "ATCoText", "p"
  privMaxDecimal = NewValue
End Property

Public Property Get MaxDecimal&()
  MaxDecimal = privMaxDecimal
End Property

Public Property Let DefaultValue(ByVal NewValue As Variant)
  DbgMsg "DefaultValue:Let:" & NewValue, 8, "ATCoText", "p"
  If CStr(DefVal) = Text1.Text Then Text1.Text = FormatValue(NewValue)
  DefVal = NewValue
End Property

Property Get DefaultValue() As Variant
  DefaultValue = DefVal
End Property

Property Get ForeColor() As OLE_COLOR
  ForeColor = Text1.ForeColor
End Property

Property Let ForeColor(ByVal newColor As OLE_COLOR)
  fgColor = newColor
  Text1.ForeColor = newColor
End Property

Property Get BackColor() As OLE_COLOR
  BackColor = Text1.BackColor
End Property

Property Let BackColor(ByVal newColor As OLE_COLOR)
  Text1.BackColor = newColor
End Property

Property Get Enabled() As Boolean
  Enabled = Not Text1.Locked
End Property


Property Let Enabled(ByVal Enable As Boolean)
  
  DbgMsg "Enabled:Let:" & Enable, 8, "ATCoText", "p"
  Text1.Locked = Not Enable
  If Enable Then
    Text1.BackColor = OkBg
    Text1.ForeColor = fgColor
  Else
    Text1.BackColor = vb3DLight
    Text1.ForeColor = vbGrayText
  End If
End Property

Public Property Get SelStart&()
  SelStart = Text1.SelStart
End Property

Public Property Let SelStart(ByVal NewValue&)
  DbgMsg "SelStart:Let:" & NewValue, 8, "ATCoText", "p"
  If NewValue >= 0 And NewValue <= Len(Text1.Text) Then Text1.SelStart = NewValue
End Property

Public Property Get SelLength&()
  SelLength = Text1.SelLength
End Property

Public Property Let SelLength(ByVal NewValue&)
  DbgMsg "SelLength:Let:" & NewValue, 8, "ATCoText", "p"
  If NewValue >= 0 And NewValue + Text1.SelStart <= Len(Text1.Text) Then Text1.SelLength = NewValue
End Property

Public Property Get Value() As Variant
  Value = Null
  If DataType = ATCoTxt Then
    Value = Text1.Text
  ElseIf DataType = ATCoInt Then
    If IsNumeric(Text1.Text) Then Value = CLng(Text1.Text)
  ElseIf DataType = ATCoSng Then
    If IsNumeric(Text1.Text) Then Value = CSng(Text1.Text)
  ElseIf DataType = ATCoClr Then
    If Text1.Text = "" Then
      Value = colorName(Text1.BackColor)
    Else
      Value = Text1.Text
    End If
  End If
  If IsNull(Value) Then Value = DefVal
  
  'DefVal = Value
  'Text1.Text = FormatValue(Value)
End Property

Public Property Let Value(ByVal NewValue As Variant)
  DbgMsg "Value:Let:" & NewValue, 7, "ATCoText", "p"
  If (DataType = ATCoInt Or DataType = ATCoSng) And IsNumeric(NewValue) Then
    If NewValue = NONE Then
      Text1.Text = ""
      Exit Property
    End If
  End If
  
  NewValue = Valid(NewValue)
  Text1.Text = FormatValue(NewValue)
  DefVal = NewValue
  Text1.SelStart = Len(Text1.Text)

End Property

Public Sub ShowRange()
  If DataType = ATCoClr Then
    SetColorFromDialog
  Else
    mnuX(1).Caption = "No limits"
    mnuX(2).Caption = ""
    mnuX(1).Visible = True
    mnuX(2).Visible = True
    Text1.Enabled = False
    Text1.Enabled = True
    Text1.SetFocus
    If DataType = ATCoTxt Then
      If HardMax <> NONE Then mnuX(1).Caption = "Max Length: " & HardMax
    ElseIf DataType = ATCoInt Or DataType = ATCoSng Then
      If HardMin = NONE And HardMax = NONE Then
        mnuX(1).Caption = "" '"No Hard Limits"
        mnuX(1).Visible = False
      ElseIf HardMin = NONE Then
        mnuX(1).Caption = "Maximum: " & HardMax
      ElseIf HardMax = NONE Then
        mnuX(1).Caption = "Minimum: " & HardMin
      Else
        mnuX(1).Caption = "Limits: " & HardMin & " to " & HardMax
      End If
  
      If SoftMin = NONE And SoftMax = NONE Then
        mnuX(2).Caption = "" '"No Soft Limits"
        If mnuX(1).Visible Then mnuX(2).Visible = False Else mnuX(2).Caption = "No limits"
      ElseIf SoftMin = NONE Then
        mnuX(2).Caption = "Soft Maximum: " & SoftMax
      ElseIf SoftMax = NONE Then
        mnuX(2).Caption = "Soft Minimum: " & SoftMin
      Else
        mnuX(2).Caption = "Soft limits: " & SoftMin & " to " & SoftMax
      End If
    End If
    PopupMenu mnuText
  End If
End Sub

Private Sub SetColorFromDialog()
  cdlg.Color = Text1.BackColor
  cdlg.DialogTitle = "Select a color"
  cdlg.CancelError = True
  On Error GoTo DontSetColor
  cdlg.ShowColor
  Me.Value = cdlg.Color
DontSetColor:
End Sub

Private Sub SetToolTip()
  Dim softtext$
  Text1.ToolTipText = ""
  If DataType = ATCoTxt Then
    If HardMax <> NONE Then Text1.ToolTipText = "Max Length: " & HardMax
  ElseIf DataType = ATCoInt Or DataType = ATCoSng Then
    If HardMin = NONE And HardMax = NONE Then
      Text1.ToolTipText = ""
    ElseIf HardMin = NONE Then
      Text1.ToolTipText = "Max: " & HardMax
    ElseIf HardMax = NONE Then
      Text1.ToolTipText = "Min: " & HardMin
    Else
      Text1.ToolTipText = "Min: " & HardMin & " Max: " & HardMax
    End If

    If SoftMin = NONE And SoftMax = NONE Then
      softtext = ""
    ElseIf SoftMin = NONE Then
      softtext = "Soft Max: " & SoftMax
    ElseIf SoftMax = NONE Then
      softtext = "Soft Min: " & SoftMin
    Else
      softtext = "Soft Min: " & SoftMin & " Soft Max: " & SoftMax
    End If
    If Len(softtext) > 0 Then
      If Len(Text1.ToolTipText) > 0 Then
        Text1.ToolTipText = Text1.ToolTipText & ", " & softtext
      Else
        Text1.ToolTipText = softtext
      End If
    End If
  End If
End Sub

Private Sub Text1_Change()
  With Text1
    
    If DataType = ATCoClr Then GoTo LeaveSub

    If .Locked Then .BackColor = vb3DLight Else .BackColor = OkBg
    If Len(.Text) < 1 Then Exit Sub
    
    If DataType = ATCoTxt Then
      If HardMax <> NONE And Len(.Text) > HardMax Then .BackColor = OutsideHardBg
      GoTo LeaveSub
    
    ElseIf DataType = ATCoInt Then
      If Not IsNumeric(.Text) Then
        If .Text <> "-" Or HardMin >= 0 Then .BackColor = OutsideHardBg
        GoTo LeaveSub
      End If
    
    ElseIf DataType = ATCoSng Then
      If Not IsNumeric(.Text) Then
        If (.Text <> "-" And .Text <> "+" And .Text <> "-." And .Text <> "+." Or HardMin >= 0) And .Text <> "." Then .BackColor = OutsideHardBg
        GoTo LeaveSub
      End If
    End If
    
    Dim val!
    val = CSng(.Text)
    If BelowLimit(val, SoftMin) Or AboveLimit(val, SoftMax) Then .BackColor = OutsideSoftBg
    If BelowLimit(val, HardMin) Or AboveLimit(val, HardMax) Then .BackColor = OutsideHardBg
  End With
LeaveSub:
  RaiseEvent Change
End Sub

Private Sub Text1_GotFocus()
  DbgMsg "Text1:GotFocus", 8, "ATCoText", "f"
  Tag = Value
  Text1.SelStart = 0
  Text1.SelLength = Len(Text1.Text)
  SetToolTip
End Sub

Private Sub Text1_KeyDown(KeyCode As Integer, Shift As Integer)
  DbgMsg "Text1:KeyDown: " & KeyCode, 8, "ATCoText", "k"
  Dim newName$, newColor&
  If KeyCode = vbKeyReturn Then
    RaiseEvent CommitChange
    KeyCode = 0
  ElseIf KeyCode = vbKeyEscape Then
    Value = Tag
    RaiseEvent CommitChange
  ElseIf DataType = ATCoClr And KeyCode = vbKeyUp Then
    testColor False, Text1.Text, newName, newColor
  ElseIf DataType = ATCoClr And KeyCode = vbKeyDown Then
    testColor True, Text1.Text, newName, newColor
  Else
    RaiseEvent KeyDown(KeyCode, Shift)
  End If
  If newName <> "" Then
    Text1.Text = newName
    Text1.BackColor = newColor
    If Brightness(newColor) > 0.4 Then Text1.ForeColor = vbBlack Else Text1.ForeColor = vbWhite
    RaiseEvent CommitChange
  End If
End Sub

Private Sub Text1_LostFocus()
  DbgMsg "Text1:LostFocus:" & Text1.Text & " " & DefVal, 8, "ATCoText", "f"
  
  Dim NewValue
  NewValue = Valid(Text1.Text)
  DefVal = NewValue
  Text1.Text = FormatValue(NewValue)
  RaiseEvent CommitChange
  'Dim i&
  'If Text1.BackColor = OutsideSoftBg Then
  '  i = MsgBox("Value is out of Normal range", vbOKCancel)
  'ElseIf Text1.BackColor = OutsideHardBg Then
  '  i = MsgBox("Value is out of Acceptable range", vbRetryCancel)
  'Else
  '  i = vbOK
  'End If
  'If i = vbRetry Then
  '  Text1.SetFocus
  'ElseIf i = vbCancel Then
  '  Text1.Text = OldValStr
  'End If
End Sub

Private Sub Text1_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = vbRightButton Then
    ShowRange
  End If
End Sub

Private Sub UserControl_EnterFocus()
  DbgMsg "Control:EnterFocus", 8, "ATCoText", "f"
End Sub

Private Sub UserControl_Initialize()
  'Text1.Text = FormatValue(DefaultValue)
End Sub

Private Sub UserControl_InitProperties()
  HardMax = NONE
  HardMin = NONE
  SoftMax = NONE
  SoftMin = NONE
  maxWidth = NONE
  DataType = ATCoTxt
  DefaultValue = NONE
  InsideLimitsBackground = vbWhite
  OutsideHardLimitBackground = 8421631
  OutsideSoftLimitBackground = 8454143
  Text1.Text = FormatValue(DefVal)
End Sub

Private Sub UserControl_Resize()
  Text1.Width = UserControl.Width
  Text1.Height = UserControl.Height
End Sub

Private Sub UserControl_ReadProperties(PropBag As PropertyBag)
  InsideLimitsBackground = PropBag.ReadProperty("InsideLimitsBackground", vbWhite)
  OutsideHardLimitBackground = PropBag.ReadProperty("OutsideHardLimitBackground", 8421631)
  OutsideSoftLimitBackground = PropBag.ReadProperty("OutsideSoftLimitBackground", 8454143)
  HardMax = PropBag.ReadProperty("HardMax", NONE)
  HardMin = PropBag.ReadProperty("HardMin", NONE)
  SoftMax = PropBag.ReadProperty("SoftMax", NONE)
  SoftMin = PropBag.ReadProperty("SoftMin", NONE)
  maxWidth = PropBag.ReadProperty("MaxWidth", NONE)
  Alignment = PropBag.ReadProperty("Alignment", Text1.Alignment)
  DataType = PropBag.ReadProperty("DataType", 0)
  DefVal = PropBag.ReadProperty("DefaultValue", 0)
  Text1.Text = PropBag.ReadProperty("Value", "")
  Text1.Locked = Not PropBag.ReadProperty("Enabled", True)
  If Text1.Locked Then Text1.BackColor = vb3DLight Else Text1.BackColor = OkBg
End Sub

Private Sub UserControl_WriteProperties(PropBag As PropertyBag)
  PropBag.WriteProperty "InsideLimitsBackground", InsideLimitsBackground
  PropBag.WriteProperty "OutsideHardLimitBackground", OutsideHardLimitBackground
  PropBag.WriteProperty "OutsideSoftLimitBackground", OutsideSoftLimitBackground
  PropBag.WriteProperty "HardMax", HardMax
  PropBag.WriteProperty "HardMin", HardMin
  PropBag.WriteProperty "SoftMax", SoftMax
  PropBag.WriteProperty "SoftMin", SoftMin
  PropBag.WriteProperty "MaxWidth", maxWidth
  PropBag.WriteProperty "Alignment", Alignment
  PropBag.WriteProperty "DataType", DataType
  PropBag.WriteProperty "DefaultValue", DefVal
  PropBag.WriteProperty "Value", Text1.Text
  PropBag.WriteProperty "Enabled", (Not Text1.Locked)
End Sub

Private Function Valid(NewValue)
  Dim msgtext$, sVal!
  msgtext = ""
  Valid = NewValue
  If DataType = ATCoTxt Then
    If HardMax <> NONE And Len(NewValue) > HardMax Then
      Valid = Left(NewValue, HardMax)
      msgtext = "The value '" & NewValue & "' was too long." & vbCr
      msgtext = msgtext & "Values can be at most " & HardMax & " characters long."
    End If
  ElseIf DataType = ATCoInt Or DataType = ATCoSng Then
    If IsNumeric(NewValue) Then
      sVal = CSng(NewValue)
      NewValue = sVal 'We don't want to compare 0.1 with .1 and think they are different
    Else
      msgtext = "The value '" & NewValue & "' is not numeric." & vbCr
      sVal = NONE
    End If
    If IsNumeric(DefVal) Then
      If DefVal <> NONE And (HardMin = NONE Or DefVal >= HardMin) And (HardMax = NONE Or DefVal <= HardMax) Then
        If BelowLimit(sVal, HardMin) Or AboveLimit(sVal, HardMax) Then sVal = DefVal
      End If
    End If
    If BelowLimit(sVal, HardMin) Then sVal = HardMin
    If AboveLimit(sVal, HardMax) Then sVal = HardMax
    If CStr(sVal) <> CStr(NewValue) Then
      If Len(msgtext) = 0 Then msgtext = "The value '" & NewValue & "' is outside the valid range." & vbCr
      If HardMin = NONE Then
        msgtext = msgtext & "Values must be less than or equal to " & HardMax & Chr$(13)
      ElseIf HardMax = NONE Then
        msgtext = msgtext & "Values must be greater than or equal to " & HardMin & Chr$(13)
      Else: msgtext = msgtext & "Values must be between " & HardMin & " and " & HardMax & vbCr
      End If
    End If
    Valid = sVal
  End If
  If Len(msgtext) > 0 Then
    msgtext = msgtext & "Value has been reset to " & Valid
    MsgBox msgtext, vbOKOnly, name
  End If
End Function

Private Function BelowLimit(testVal!, LimitVal!) As Boolean
  If LimitVal = NONE Then
    BelowLimit = False
  ElseIf LimitVal >= 0 Then
    If testVal * slop < LimitVal Then BelowLimit = True Else BelowLimit = False
  Else 'limitval < 0
    If testVal / slop < LimitVal Then BelowLimit = True Else BelowLimit = False
  End If
End Function

Private Function AboveLimit(testVal!, LimitVal!) As Boolean
  If LimitVal = NONE Then
    AboveLimit = False
  ElseIf LimitVal >= 0 Then
    If testVal / slop > LimitVal Then AboveLimit = True Else AboveLimit = False
  Else 'limitval < 0
    If testVal * slop > LimitVal Then AboveLimit = True Else AboveLimit = False
  End If
End Function

Private Function FormatValue$(ByVal val As Variant)
  Dim retval$
  
  retval = CStr(val)
  FormatValue = retval
  Select Case DataType
    Case ATCoTxt: Exit Function
    Case ATCoClr
      Dim clrName$, tmpLng&
      tmpLng = -1
      If IsNumeric(val) Then
        clrName = colorName(val)
        tmpLng = CLng(val)
      Else
        clrName = val
      End If
      'Separate if statements in case colorName(val) comes back still numeric
      If tmpLng = -1 Then tmpLng = TextOrNumericColor(clrName)
      If IsNumeric(clrName) Then
        retval = ""
      Else
        retval = clrName
      End If
      Text1.BackColor = tmpLng
      If Brightness(tmpLng) > 0.4 Then Text1.ForeColor = vbBlack Else Text1.ForeColor = vbWhite
      'Text1.ForeColor = tmpLng Xor &HFFFFFF
    Case Else 'numeric - ATCoInt or ATCoSng
      Dim expFormat$, DecimalPlaces&, LogVal As Double
      If val <> 0 And maxWidth > 0 Then
        If Len(retval) > maxWidth Then 'First try to trim excess digits after decimal
          DecimalPlaces = InStr(retval, ".")
          If DecimalPlaces > 0 Then
            If DecimalPlaces - 1 <= maxWidth Then 'Can shrink string enough just by truncating
              retval = Left(retval, maxWidth)
            End If
          End If
        End If
        If Len(retval) > maxWidth Then
          LogVal = Abs(Log(Abs(val)) / Log(10))
          If LogVal >= 100 Then
            expFormat = "e-###"
          ElseIf LogVal >= 10 Then
            expFormat = "e-##"
          Else
            expFormat = "e-#"
          End If
          DecimalPlaces = maxWidth - Len(expFormat) - 2
          If DecimalPlaces < 1 Then DecimalPlaces = 1
          retval = Format(val, "#." & String(DecimalPlaces, "#") & expFormat)
        End If
      End If
  End Select
  FormatValue = retval
End Function

Public Function ATCoTypeString$(ByVal DataType As ATCoDataType)
  If DataType = ATCoTxt Then
    ATCoTypeString = "ATCoTxt"
  ElseIf DataType = ATCoInt Then
    ATCoTypeString = "ATCoInt"
  ElseIf DataType = ATCoSng Then
    ATCoTypeString = "ATCoSng"
  ElseIf DataType = NONE Then
    ATCoTypeString = "NONE"
  Else
    ATCoTypeString = "Undefined"
  End If
End Function

