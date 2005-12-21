Option Explicit On 

Imports System.Windows.Forms
Imports System.Math
Imports ATCutility

Public Class atcText
  Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call

  End Sub

  'UserControl overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents text1 As System.Windows.Forms.TextBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.text1 = New System.Windows.Forms.TextBox
    Me.SuspendLayout()
    '
    'text1
    '
    Me.text1.Location = New System.Drawing.Point(0, 0)
    Me.text1.Name = "text1"
    Me.text1.Size = New System.Drawing.Size(184, 22)
    Me.text1.TabIndex = 0
    Me.text1.Text = "Text1"
    '
    'ATCtext
    '
    Me.Controls.Add(Me.text1)
    Me.Name = "ATCtext"
    Me.Size = New System.Drawing.Size(184, 16)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Enum ATCoDataType
    ATCoTxt = 0
    ATCoInt = 1
    ATCoSng = 2
    ATCoClr = 10
    NONE = -999
  End Enum

  Private privDataType As ATCoDataType

  Private DefVal As Object
  Private privHardMax As Double
  Private privHardMin As Double
  Private privSoftMax As Double
  Private privSoftMin As Double
  Private privMaxWidth As Integer
  Private privMaxDecimal As Integer

  Private fgColor As System.Drawing.Color
  Private OkBg As System.Drawing.Color
  Private OutsideSoftBg As System.Drawing.Color
  Private OutsideHardBg As System.Drawing.Color

  Private Const slop As Double = 1.0001 'real numbers are screwy, so this is the slop factor

  Private Sub dbgMsg(ByVal msg As String, _
                     Optional ByVal len As Integer = 10, _
                     Optional ByVal title As String = "", _
                     Optional ByVal type As String = "")
    Debug.Write(msg)
  End Sub

  Public Event Change()

  Public Event CommitChange()

  Public Shadows Event KeyDown(ByVal KeyCode As Integer, ByVal Shift As Integer)

  Public Property Alignment() As System.Windows.Forms.HorizontalAlignment
    Get
      Alignment = text1.TextAlign
    End Get
    Set(ByVal newValue As HorizontalAlignment)
      text1.TextAlign = newValue
    End Set
  End Property

  Public Property DataType() As ATCoDataType
    Get
      DataType = privDataType
    End Get
    Set(ByVal newValue As ATCoDataType)
      privDataType = newValue
    End Set
  End Property

  Public Property HardMax() As Double
    Get
      HardMax = privHardMax
    End Get
    Set(ByVal newValue As Double)
      dbgMsg("HardMax:Let:" & newValue, 8, "ATCoText", "p")
      privHardMax = newValue
    End Set
  End Property

  Public Property HardMin() As Double
    Get
      HardMin = privHardMin
    End Get
    Set(ByVal NewValue As Double)
      dbgMsg("HardMin:Let:" & NewValue, 8, "ATCoText", "p")
      'If we store a more precise value for HardMin than we
      'allow to be typed, it causes problems.
      Dim FormattedNewValue As String
      Dim DecimalPlace As Integer

      FormattedNewValue = FormatValue(NewValue)
      privHardMin = CSng(FormattedNewValue)
      If privHardMin - NewValue > 0.0000001 Then 'I hate floating point numbers
        DecimalPlace = InStr(FormattedNewValue, "E-")
        If DecimalPlace > 0 Then
          privHardMin = privHardMin + CSng("0." & CStr(Mid(FormattedNewValue, DecimalPlace + 2) - 1) & "1")
        Else
          DecimalPlace = InStr(FormattedNewValue, ".")
          If DecimalPlace = 0 Then
            privHardMin = privHardMin + 1
          Else
            privHardMin = privHardMin + CSng("0." & CStr(Len(FormattedNewValue) - DecimalPlace - 1) & "1")
          End If
        End If
      End If
    End Set
  End Property

  Public Property SoftMax() As Double
    Get
      SoftMax = privSoftMax
    End Get
    Set(ByVal newValue As Double)
      dbgMsg("SoftMax:Let:" & newValue, 8, "ATCoText", "p")
      privSoftMax = newValue
    End Set
  End Property

  Public Property SoftMin() As Double
    Get
      SoftMin = privSoftMin
    End Get
    Set(ByVal newValue As Double)
      dbgMsg("SoftMin:Let:" & newValue, 8, "ATCoText", "p")
      privSoftMin = newValue
    End Set
  End Property

  Public Property InsideLimitsBackground() As System.Drawing.Color
    Get
      InsideLimitsBackground = OkBg
    End Get
    Set(ByVal newColor As System.Drawing.Color)
      OkBg = newColor
    End Set
  End Property

  Public Property OutsideSoftLimitBackground() As System.Drawing.Color
    Get
      OutsideSoftLimitBackground = OutsideSoftBg
    End Get
    Set(ByVal newColor As System.Drawing.Color)
      OutsideSoftBg = newColor
    End Set
  End Property

  Public Property OutsideHardLimitBackground() As System.Drawing.Color
    Get
      OutsideHardLimitBackground = OutsideHardBg
    End Get
    Set(ByVal newColor As System.Drawing.Color)
      OutsideHardBg = newColor
    End Set
  End Property

  Public Property maxWidth() As Integer
    Get
      maxWidth = privMaxWidth
    End Get
    Set(ByVal newValue As Integer)
      dbgMsg("MaxWidth:Let:" & newValue, 8, "ATCoText", "p")
      privMaxWidth = newValue
    End Set
  End Property

  Public Property MaxDecimal() As Integer
    Get
      MaxDecimal = privMaxDecimal
    End Get
    Set(ByVal newValue As Integer)
      dbgMsg("MaxDecimal:Let:" & newValue, 8, "ATCoText", "p")
      privMaxDecimal = newValue
    End Set
  End Property

  Public Property DefaultValue() As Object
    Get
      DefaultValue = DefVal
    End Get
    Set(ByVal newValue As Object)
      dbgMsg("DefaultValue:Let:" & newValue, 8, "ATCoText", "p")
      If CStr(DefVal) = text1.Text Then
        text1.Text = FormatValue(newValue)
      End If
      DefVal = newValue
    End Set
  End Property

  Overrides Property ForeColor() As System.Drawing.Color
    Get
      ForeColor = text1.ForeColor
    End Get
    Set(ByVal newColor As System.Drawing.Color)
      fgColor = newColor
      text1.ForeColor = newColor
    End Set
  End Property

  Shadows Property Enabled() As Boolean
    Get
      Enabled = text1.Enabled
    End Get
    Set(ByVal Enable As Boolean)
      dbgMsg("Enabled:Let:" & Enable, 8, "ATCoText", "p")
      text1.Enabled = Enable
      If Enable Then
        text1.BackColor = OkBg
        text1.ForeColor = fgColor
      Else
        text1.BackColor = System.Drawing.Color.LightGray 'vb3DLight
        text1.ForeColor = System.Drawing.Color.Gray      'vbGrayText
      End If
    End Set
  End Property

  Public Property SelStart() As Integer
    Get
      SelStart = text1.SelectionStart
    End Get
    Set(ByVal newValue As Integer)
      dbgMsg("SelStart:Let:" & newValue, 8, "ATCoText", "p")
      If newValue >= 0 And newValue <= Len(text1.Text) Then
        text1.SelectionStart = newValue
      End If
    End Set
  End Property

  Public Property SelLength() As Integer
    Get
      SelLength = text1.SelectionLength
    End Get
    Set(ByVal newValue As Integer)
      dbgMsg("SelLength:Let:" & newValue, 8, "ATCoText", "p")
      If newValue >= 0 And newValue + text1.SelectionStart <= Len(text1.Text) Then
        text1.SelectionLength = newValue
      End If
    End Set
  End Property

  Public Property Value() As Object
    Get
      Value = ATCoDataType.NONE
      If DataType = ATCoDataType.ATCoTxt Then
        Value = text1.Text
      ElseIf DataType = ATCoDataType.ATCoInt Then
        If IsNumeric(text1.Text) Then Value = CLng(text1.Text)
      ElseIf DataType = ATCoDataType.ATCoSng Then
        If IsNumeric(text1.Text) Then Value = CSng(text1.Text)
      ElseIf DataType = ATCoDataType.ATCoClr Then
        If text1.Text = "" Then
          Value = text1.BackColor
        Else
          Value = text1.Text
        End If
      End If
      If Value = ATCoDataType.NONE Then
        Value = DefVal
      End If
      'DefVal = Value
      'Text1.Text = FormatValue(Value)
    End Get
    Set(ByVal newValue As Object)
      dbgMsg("Value:Let:" & newValue, 7, "ATCoText", "p")
      If (DataType = ATCoDataType.ATCoInt Or _
          DataType = ATCoDataType.ATCoSng) And _
          IsNumeric(newValue) Then
        If newValue = ATCoDataType.NONE Then
          text1.Text = ""
          Exit Property
        End If
      End If

      newValue = Valid(newValue)
      text1.Text = FormatValue(newValue)
      DefVal = newValue
      text1.SelectionStart = Len(text1.Text)
    End Set
  End Property

  Public Sub ShowRange()
    If DataType = ATCoDataType.ATCoClr Then
      SetColorFromDialog()
    Else
      'mnuX(1).Caption = "No limits"
      'mnuX(2).Caption = ""
      'mnuX(1).Visible = True
      'mnuX(2).Visible = True
      'text1.Enabled = False
      'text1.Enabled = True
      'text1.Focus()
      'If DataType = ATCoDataType.ATCoTxt Then
      '    If HardMax <> ATCoDataType.NONE Then
      '        mnuX(1).Caption = "Max Length: " & HardMax
      '    End If
      'ElseIf DataType = ATCoDataType.ATCoInt Or _
      '       DataType = ATCoDataType.ATCoSng Then
      '    If HardMin = ATCoDataType.NONE And HardMax = ATCoDataType.NONE Then
      '        mnuX(1).Caption = "" '"No Hard Limits"
      '        mnuX(1).Visible = False
      '    ElseIf HardMin = ATCoDataType.NONE Then
      '        mnuX(1).Caption = "Maximum: " & HardMax
      '    ElseIf HardMax = ATCoDataType.NONE Then
      '        mnuX(1).Caption = "Minimum: " & HardMin
      '    Else
      '        mnuX(1).Caption = "Limits: " & HardMin & " to " & HardMax
      '    End If

      '    If SoftMin = ATCoDataType.NONE And SoftMax = ATCoDataType.NONE Then
      '        mnuX(2).Caption = "" '"No Soft Limits"
      '        If mnuX(1).Visible Then mnuX(2).Visible = False Else mnuX(2).Caption = "No limits"
      '    ElseIf SoftMin = ATCoDataType.NONE Then
      '        mnuX(2).Caption = "Soft Maximum: " & SoftMax
      '    ElseIf SoftMax = ATCoDataType.NONE Then
      '        mnuX(2).Caption = "Soft Minimum: " & SoftMin
      '    Else
      '        mnuX(2).Caption = "Soft limits: " & SoftMin & " to " & SoftMax
      '    End If
      'End If
      'PopupMenu(mnuText)
    End If
  End Sub

  Private Sub SetColorFromDialog()
    Dim cdlg As New Windows.Forms.ColorDialog

    cdlg.Color = text1.BackColor
    If cdlg.ShowDialog() = DialogResult.OK Then
      Me.Value = cdlg.Color
    End If
  End Sub

  Private Sub SetToolTip()
    'Dim softtext$
    'text1.ToolTipText = ""
    'If DataType = ATCoTxt Then
    '    If HardMax <> ATCoDataType.NONE Then text1.ToolTipText = "Max Length: " & HardMax
    'ElseIf DataType = ATCoInt Or DataType = ATCoSng Then
    '    If HardMin = ATCoDataType.NONE And HardMax = ATCoDataType.NONE Then
    '        text1.ToolTipText = ""
    '    ElseIf HardMin = ATCoDataType.NONE Then
    '        text1.ToolTipText = "Max: " & HardMax
    '    ElseIf HardMax = ATCoDataType.NONE Then
    '        text1.ToolTipText = "Min: " & HardMin
    '    Else
    '        text1.ToolTipText = "Min: " & HardMin & " Max: " & HardMax
    '    End If

    '    If SoftMin = ATCoDataType.NONE And SoftMax = ATCoDataType.NONE Then
    '        softtext = ""
    '    ElseIf SoftMin = ATCoDataType.NONE Then
    '        softtext = "Soft Max: " & SoftMax
    '    ElseIf SoftMax = ATCoDataType.NONE Then
    '        softtext = "Soft Min: " & SoftMin
    '    Else
    '        softtext = "Soft Min: " & SoftMin & " Soft Max: " & SoftMax
    '    End If
    '    If Len(softtext) > 0 Then
    '        If Len(text1.ToolTipText) > 0 Then
    '            text1.ToolTipText = text1.ToolTipText & ", " & softtext
    '        Else
    '            text1.ToolTipText = softtext
    '        End If
    '    End If
    'End If
  End Sub

  Private Sub Text1_Change()
    With text1

      If DataType = ATCoDataType.ATCoClr Then GoTo LeaveSub

      If Not .Enabled Then
        .BackColor = System.Drawing.Color.LightGray
      Else
        .BackColor = OkBg
      End If

      If Len(.Text) < 1 Then Exit Sub

      If DataType = ATCoDataType.ATCoTxt Then
        If HardMax <> ATCoDataType.NONE And Len(.Text) > HardMax Then .BackColor = OutsideHardBg
        GoTo LeaveSub

      ElseIf DataType = ATCoDataType.ATCoInt Then
        If Not IsNumeric(.Text) Then
          If .Text <> "-" Or HardMin >= 0 Then .BackColor = OutsideHardBg
          GoTo LeaveSub
        End If

      ElseIf DataType = ATCoDataType.ATCoSng Then
        If Not IsNumeric(.Text) Then
          If (.Text <> "-" And .Text <> "+" And .Text <> "-." And .Text <> "+." Or HardMin >= 0) And .Text <> "." Then .BackColor = OutsideHardBg
          GoTo LeaveSub
        End If
      End If

      Dim Val!
      Val = CSng(.Text)
      If BelowLimit(Val, SoftMin) Or AboveLimit(Val, SoftMax) Then .BackColor = OutsideSoftBg
      If BelowLimit(Val, HardMin) Or AboveLimit(Val, HardMax) Then .BackColor = OutsideHardBg
    End With
LeaveSub:
    RaiseEvent Change()
  End Sub

  Private Sub Text1_GotFocus()
    dbgMsg("Text1:GotFocus", 8, "ATCoText", "f")
    Tag = Value
    text1.SelectionStart = 0
    text1.SelectionLength = Len(text1.Text)
    SetToolTip()
  End Sub

  Private Sub Text1_KeyDown(ByVal KeyCode As Integer, ByVal Shift As Integer)
    dbgMsg("Text1:KeyDown: " & KeyCode, 8, "ATCoText", "k")
    Dim newName$, newColor&
    If KeyCode = Keys.Enter Then
      RaiseEvent CommitChange()
      KeyCode = 0
    ElseIf KeyCode = Keys.Escape Then
      Value = Tag
      RaiseEvent CommitChange()
      'ElseIf DataType = ATCoDataType.ATCoClr And KeyCode = Keys.Up Then
      '  testColor(False, text1.Text, newName, Color.FromArgb(newColor))
      'ElseIf DataType = ATCoDataType.ATCoClr And KeyCode = Keys.Down Then
      '  testColor(True, text1.Text, newName, Color.FromArgb(newColor))
    Else
      RaiseEvent KeyDown(KeyCode, Shift)
    End If
    If newName <> "" Then
      text1.Text = newName
      text1.BackColor = Color.FromArgb(newColor)
      If Color.FromArgb(newColor).GetBrightness > 0.4 Then
        text1.ForeColor = System.Drawing.Color.Black
      Else
        text1.ForeColor = System.Drawing.Color.White
      End If
      RaiseEvent CommitChange()
    End If
  End Sub

  Private Sub Text1_LostFocus()
    dbgMsg("Text1:LostFocus:" & text1.Text & " " & DefVal, 8, "ATCoText", "f")

    Dim newValue As Object = Valid(text1.Text)
    DefVal = newValue
    text1.Text = FormatValue(newValue)
    RaiseEvent CommitChange()
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

  Private Sub Text1_MouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal X As Single, ByVal Y As Single)
    If Button = Windows.Forms.MouseButtons.Right Then
      ShowRange()
    End If
  End Sub

  Private Sub UserControl_EnterFocus()
    dbgMsg("Control:EnterFocus", 8, "ATCoText", "f")
  End Sub

  Private Sub UserControl_Initialize()
    'Text1.Text = FormatValue(DefaultValue)
  End Sub

  Private Sub UserControl_InitProperties()
    HardMax = ATCoDataType.NONE
    HardMin = ATCoDataType.NONE
    SoftMax = ATCoDataType.NONE
    SoftMin = ATCoDataType.NONE
    maxWidth = ATCoDataType.NONE
    DataType = ATCoDataType.ATCoTxt
    DefaultValue = ATCoDataType.NONE
    InsideLimitsBackground = System.Drawing.Color.White
    OutsideHardLimitBackground = Color.FromArgb(8421631)
    OutsideSoftLimitBackground = Color.FromArgb(8454143)
    text1.Text = FormatValue(DefVal)
  End Sub

  Private Sub UserControl_Resize()
    text1.Width = Me.Width
    text1.Height = Me.Height
  End Sub

  'Private Sub UserControl_ReadProperties(ByVal PropBag As PropertyBag)
  '    InsideLimitsBackground = PropBag.ReadProperty("InsideLimitsBackground", System.Drawing.Color.White)
  '    OutsideHardLimitBackground = PropBag.ReadProperty("OutsideHardLimitBackground", 8421631)
  '    OutsideSoftLimitBackground = PropBag.ReadProperty("OutsideSoftLimitBackground", 8454143)
  '    HardMax = PropBag.ReadProperty("HardMax", ATCoDataType.NONE)
  '    HardMin = PropBag.ReadProperty("HardMin", ATCoDataType.NONE)
  '    SoftMax = PropBag.ReadProperty("SoftMax", ATCoDataType.NONE)
  '    SoftMin = PropBag.ReadProperty("SoftMin", ATCoDataType.NONE)
  '    maxWidth = PropBag.ReadProperty("MaxWidth", ATCoDataType.NONE)
  '    Alignment = PropBag.ReadProperty("Alignment", text1.TextAlign)
  '    DataType = PropBag.ReadProperty("DataType", 0)
  '    DefVal = PropBag.ReadProperty("DefaultValue", 0)
  '    text1.Text = PropBag.ReadProperty("Value", "")
  '    text1.Enabled = PropBag.ReadProperty("Enabled", True)
  '    If text1.Enabled Then
  '        text1.BackColor = OkBg
  '    Else
  '        text1.BackColor = System.Drawing.Color.LightGray
  '    End If
  'End Sub

  'Private Sub UserControl_WriteProperties(ByVal PropBag As PropertyBag)
  '    PropBag.WriteProperty("InsideLimitsBackground", InsideLimitsBackground)
  '    PropBag.WriteProperty("OutsideHardLimitBackground", OutsideHardLimitBackground)
  '    PropBag.WriteProperty("OutsideSoftLimitBackground", OutsideSoftLimitBackground)
  '    PropBag.WriteProperty("HardMax", HardMax)
  '    PropBag.WriteProperty("HardMin", HardMin)
  '    PropBag.WriteProperty("SoftMax", SoftMax)
  '    PropBag.WriteProperty("SoftMin", SoftMin)
  '    PropBag.WriteProperty("MaxWidth", maxWidth)
  '    PropBag.WriteProperty("Alignment", Alignment)
  '    PropBag.WriteProperty("DataType", DataType)
  '    PropBag.WriteProperty("DefaultValue", DefVal)
  '    PropBag.WriteProperty("Value", text1.Text)
  '    PropBag.WriteProperty("Enabled", (text1.Enabled))
  'End Sub

  Private Function Valid(ByVal newValue As Object) As Object
    Dim msgtext As String
    Dim sVal As Double

    msgtext = ""
    Valid = newValue

    If DataType = ATCoDataType.ATCoTxt Then
      If HardMax <> ATCoDataType.NONE And Len(newValue) > HardMax Then
        Valid = CStr(newValue).Substring(0, HardMax)
        msgtext = "The value '" & newValue & "' was too long." & vbCr
        msgtext = msgtext & "Values can be at most " & HardMax & " characters long."
      End If
    ElseIf DataType = ATCoDataType.ATCoInt Or _
           DataType = ATCoDataType.ATCoSng Then
      If IsNumeric(newValue) Then
        sVal = CDbl(newValue)
        newValue = sVal 'We don't want to compare 0.1 with .1 and think they are different
      Else
        msgtext = "The value '" & newValue & "' is not numeric." & vbCr
        sVal = ATCoDataType.NONE
      End If
      If IsNumeric(DefVal) Then
        If DefVal <> ATCoDataType.NONE And (HardMin = ATCoDataType.NONE Or DefVal >= HardMin) And (HardMax = ATCoDataType.NONE Or DefVal <= HardMax) Then
          If BelowLimit(sVal, HardMin) Or AboveLimit(sVal, HardMax) Then sVal = DefVal
        End If
      End If
      If BelowLimit(sVal, HardMin) Then sVal = HardMin
      If AboveLimit(sVal, HardMax) Then sVal = HardMax
      If CStr(sVal) <> CStr(newValue) Then
        If Len(msgtext) = 0 Then msgtext = "The value '" & newValue & "' is outside the valid range." & vbCr
        If HardMin = ATCoDataType.NONE Then
          msgtext = msgtext & "Values must be less than or equal to " & HardMax & Chr(13)
        ElseIf HardMax = ATCoDataType.NONE Then
          msgtext = msgtext & "Values must be greater than or equal to " & HardMin & Chr(13)
        Else : msgtext = msgtext & "Values must be between " & HardMin & " and " & HardMax & vbCr
        End If
      End If
      Valid = sVal
    End If
    If Len(msgtext) > 0 Then
      msgtext = msgtext & "Value has been reset to " & Valid
      MsgBox(msgtext, vbOKOnly, Name)
    End If
  End Function

  Private Function BelowLimit(ByVal testVal!, ByVal LimitVal!) As Boolean
    If LimitVal = ATCoDataType.NONE Then
      BelowLimit = False
    ElseIf LimitVal >= 0 Then
      If testVal * slop < LimitVal Then BelowLimit = True Else BelowLimit = False
    Else 'limitval < 0
      If testVal / slop < LimitVal Then BelowLimit = True Else BelowLimit = False
    End If
  End Function

  Private Function AboveLimit(ByVal testVal!, ByVal LimitVal!) As Boolean
    If LimitVal = ATCoDataType.NONE Then
      AboveLimit = False
    ElseIf LimitVal >= 0 Then
      If testVal / slop > LimitVal Then AboveLimit = True Else AboveLimit = False
    Else 'limitval < 0
      If testVal * slop > LimitVal Then AboveLimit = True Else AboveLimit = False
    End If
  End Function

  Private Function FormatValue(ByVal Val As Object) As String
    Dim retval$

    retval = CStr(Val)
    FormatValue = retval
    Select Case DataType
      Case ATCoDataType.ATCoTxt : Exit Function
      Case ATCoDataType.ATCoClr
        Dim clrName As String
        Dim tmpClr As Color = Color.Empty
        If IsNumeric(Val) Then
          clrName = colorName(Val)
          tmpClr = Color.FromArgb(Val)
        Else
          clrName = Val
        End If
        'Separate if statements in case colorName(val) comes back still numeric
        If tmpClr.Equals(Color.Empty) Then tmpClr = TextOrNumericColor(clrName)
        If IsNumeric(clrName) Then
          retval = ""
        Else
          retval = clrName
        End If
        text1.BackColor = tmpClr
        If tmpClr.GetBrightness > 0.4 Then
          text1.ForeColor = System.Drawing.Color.Black
        Else
          text1.ForeColor = System.Drawing.Color.White
        End If
        'Text1.ForeColor = tmpLng Xor &HFFFFFF
      Case Else 'numeric - ATCoInt or ATCoSng
        Dim expFormat As String, DecimalPlaces As Integer, LogVal As Double
        If Val <> 0 And maxWidth > 0 Then
          If Len(retval) > maxWidth Then 'First try to trim excess digits after decimal
            DecimalPlaces = InStr(retval, ".")
            If DecimalPlaces > 0 Then
              If DecimalPlaces - 1 <= maxWidth Then 'Can shrink string enough just by truncating
                retval = retval.Substring(maxWidth) '(retval, maxWidth)
              End If
            End If
          End If
          If Len(retval) > maxWidth Then
            LogVal = Abs(Log(Abs(Val)) / Log(10))
            If LogVal >= 100 Then
              expFormat = "e-###"
            ElseIf LogVal >= 10 Then
              expFormat = "e-##"
            Else
              expFormat = "e-#"
            End If
            DecimalPlaces = maxWidth - Len(expFormat) - 2
            If DecimalPlaces < 1 Then
              DecimalPlaces = 1
            End If
            retval = Format(Val, "#.") & Format(DecimalPlaces, "#") & expFormat
          End If
        End If
    End Select
    FormatValue = retval
  End Function

  Public Function ATCoTypeString(ByVal DataType As ATCoDataType) As String
    If DataType = ATCoDataType.ATCoTxt Then
      ATCoTypeString = "ATCoTxt"
    ElseIf DataType = ATCoDataType.ATCoInt Then
      ATCoTypeString = "ATCoInt"
    ElseIf DataType = ATCoDataType.ATCoSng Then
      ATCoTypeString = "ATCoSng"
    ElseIf DataType = ATCoDataType.NONE Then
      ATCoTypeString = "NONE"
    Else
      ATCoTypeString = "Undefined"
    End If
  End Function


End Class
