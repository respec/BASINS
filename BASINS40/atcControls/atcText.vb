Option Explicit On

Imports System.Windows.Forms
Imports System.Math
Imports ATCutility
Imports MapWinUtility

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
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtBox As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.txtBox = New System.Windows.Forms.TextBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'txtBox
        '
        Me.txtBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBox.Location = New System.Drawing.Point(0, 0)
        Me.txtBox.Name = "txtBox"
        Me.txtBox.Size = New System.Drawing.Size(184, 20)
        Me.txtBox.TabIndex = 0
        '
        'atcText
        '
        Me.Controls.Add(Me.txtBox)
        Me.Name = "atcText"
        Me.Size = New System.Drawing.Size(184, 20)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Enum ATCoDataType
        NONE = -999
        ATCoTxt = 0
        ATCoInt = 1
        ATCoDbl = 2
        ATCoClr = 10
    End Enum

    Private privDataType As ATCoDataType

    Private privNumericFormat As String = "0.#####"
    Private privDefaultValue As String = ""
    Private privHardMax As Double = ATCoDataType.NONE
    Private privHardMin As Double = ATCoDataType.NONE
    Private privSoftMax As Double = ATCoDataType.NONE
    Private privSoftMin As Double = ATCoDataType.NONE
    Private privMaxWidth As Integer = 20

    Private fgColor As System.Drawing.Color = Color.Black
    Private OkBg As System.Drawing.Color = Color.White
    Private OutsideSoftBg As System.Drawing.Color = Color.Yellow
    Private OutsideHardBg As System.Drawing.Color = Color.Coral

    Private Const slop As Double = 1.0001 'real numbers are screwy, so this is the slop factor

    Public Event Change()

    Public Event CommitChange()

    Public Shadows Event KeyDown(ByVal KeyCode As Integer, ByVal Shift As Integer)

    Public Property Alignment() As System.Windows.Forms.HorizontalAlignment
        Get
            Alignment = txtBox.TextAlign
        End Get
        Set(ByVal newValue As HorizontalAlignment)
            txtBox.TextAlign = newValue
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

    Public Property NumericFormat() As String
        Get
            Return privNumericFormat
        End Get
        Set(ByVal newValue As String)
            privNumericFormat = newValue
        End Set
    End Property

    Public Property HardMax() As Double
        Get
            HardMax = privHardMax
        End Get
        Set(ByVal newValue As Double)
            'Logger.Dbg("HardMax:Let:" & newValue)
            privHardMax = newValue
        End Set
    End Property

    Public Property HardMin() As Double
        Get
            HardMin = privHardMin
        End Get
        Set(ByVal NewValue As Double)
            'Logger.Dbg("HardMin:Let:" & NewValue)
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
            'Logger.Dbg("SoftMax:Let:" & newValue)
            privSoftMax = newValue
        End Set
    End Property

    Public Property SoftMin() As Double
        Get
            SoftMin = privSoftMin
        End Get
        Set(ByVal newValue As Double)
            'Logger.Dbg("SoftMin:Let:" & newValue)
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

    Public Property MaxWidth() As Integer
        Get
            MaxWidth = privMaxWidth
        End Get
        Set(ByVal newValue As Integer)
            'Logger.Dbg("MaxWidth:Let:" & newValue)
            privMaxWidth = newValue
        End Set
    End Property

    'Public Property MaxDecimal() As Integer
    '    Get
    '        MaxDecimal = privMaxDecimal
    '    End Get
    '    Set(ByVal newValue As Integer)
    '        'Logger.Dbg("MaxDecimal:Let:" & newValue)
    '        privMaxDecimal = newValue
    '    End Set
    'End Property

    Public Property DefaultValue() As String
        Get
            DefaultValue = privDefaultValue
        End Get
        Set(ByVal newValue As String)
            'Logger.Dbg("DefaultValue:Let:" & newValue)
            If privDefaultValue = txtBox.Text Then
                txtBox.Text = FormatValue(newValue)
            End If
            privDefaultValue = newValue
        End Set
    End Property

    Overrides Property ForeColor() As System.Drawing.Color
        Get
            ForeColor = txtBox.ForeColor
        End Get
        Set(ByVal newColor As System.Drawing.Color)
            fgColor = newColor
            txtBox.ForeColor = newColor
        End Set
    End Property

    Shadows Property Enabled() As Boolean
        Get
            Enabled = txtBox.Enabled
        End Get
        Set(ByVal Enable As Boolean)
            'Logger.Dbg("Enabled:Let:" & Enable)
            txtBox.Enabled = Enable
            If Enable Then
                txtBox.BackColor = OkBg
                txtBox.ForeColor = fgColor
            Else
                txtBox.BackColor = System.Drawing.Color.LightGray 'vb3DLight
                txtBox.ForeColor = System.Drawing.Color.Gray      'vbGrayText
            End If
        End Set
    End Property

    Public Property SelStart() As Integer
        Get
            SelStart = txtBox.SelectionStart
        End Get
        Set(ByVal newValue As Integer)
            'Logger.Dbg("SelStart:Let:" & newValue)
            If newValue >= 0 And newValue <= Len(txtBox.Text) Then
                txtBox.SelectionStart = newValue
            End If
        End Set
    End Property

    Public Property SelLength() As Integer
        Get
            SelLength = txtBox.SelectionLength
        End Get
        Set(ByVal newValue As Integer)
            'Logger.Dbg("SelLength:Let:" & newValue)
            If newValue >= 0 And newValue + txtBox.SelectionStart <= Len(txtBox.Text) Then
                txtBox.SelectionLength = newValue
            End If
        End Set
    End Property

    Public Property ValueInteger() As Integer
        Get
            Dim lInt As Integer = 0
            If Not Integer.TryParse(Text, lInt) Then
                Integer.TryParse(privDefaultValue, lInt)
            End If
            Return lInt
        End Get
        Set(ByVal newValue As Integer)
            Me.Text = newValue.ToString
        End Set
    End Property

    Public Property ValueDouble() As Double
        Get
            Dim lDbl As Double = 0
            If Not Double.TryParse(Text, lDbl) Then
                Double.TryParse(privDefaultValue, lDbl)
            End If
            Return lDbl
        End Get
        Set(ByVal newValue As Double)
            Me.Text = newValue.ToString
        End Set
    End Property

    Public Overrides Property Text() As String
        Get
            Dim lValue As String = ATCoDataType.NONE
            Select Case DataType
                Case ATCoDataType.ATCoTxt, ATCoDataType.NONE
                    lValue = txtBox.Text
                Case ATCoDataType.ATCoInt
                    If IsNumeric(txtBox.Text) Then
                        lValue = CInt(txtBox.Text)
                    Else
                        lValue = ""
                    End If
                Case ATCoDataType.ATCoDbl
                    If IsNumeric(txtBox.Text) Then
                        lValue = CSng(txtBox.Text)
                    Else
                        lValue = ""
                    End If
                Case ATCoDataType.ATCoClr
                    If txtBox.Text = "" Then
                        lValue = txtBox.BackColor.ToArgb
                    Else
                        lValue = txtBox.Text
                    End If
            End Select
            Return lValue
        End Get
        Set(ByVal newValue As String)
            'Logger.Dbg("Value:Let:" & newValue)
            'If (DataType = ATCoDataType.ATCoInt OrElse _
            '    DataType = ATCoDataType.ATCoDbl) AndAlso _
            '    IsNumeric(newValue) Then
            '    If newValue = ATCoDataType.NONE Then
            '        txtBox.Text = ""
            '        Exit Property
            '    End If
            'End If

            newValue = Valid(newValue)
            txtBox.Text = FormatValue(newValue)
            txtBox.SelectAll()
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
            'txtBox.Enabled = False
            'txtBox.Enabled = True
            'txtBox.Focus()
            'If DataType = ATCoDataType.ATCoTxt Then
            '    If HardMax <> ATCoDataType.NONE Then
            '        mnuX(1).Caption = "Max Length: " & HardMax
            '    End If
            'ElseIf DataType = ATCoDataType.ATCoInt Or _
            '       DataType = ATCoDataType.ATCoDbl Then
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

        cdlg.Color = txtBox.BackColor
        If cdlg.ShowDialog() = DialogResult.OK Then
            Me.Text = cdlg.Color.ToArgb
        End If
    End Sub

    Private Sub SetToolTip()
        Dim lToolText As String = ""
        Dim lSoftLimits As String
        If DataType = ATCoDataType.ATCoTxt Then
            If HardMax <> ATCoDataType.NONE Then lToolText = "Max Length: " & HardMax
        ElseIf DataType = ATCoDataType.ATCoInt OrElse DataType = ATCoDataType.ATCoDbl Then
            If HardMin = ATCoDataType.NONE AndAlso HardMax = ATCoDataType.NONE Then
                lToolText = ""
            ElseIf HardMin = ATCoDataType.NONE Then
                lToolText = "Max: " & HardMax
            ElseIf HardMax = ATCoDataType.NONE Then
                lToolText = "Min: " & HardMin
            Else
                lToolText = "Min: " & HardMin & " Max: " & HardMax
            End If

            Dim lSoftPrefix As String = "Soft "
            If lToolText.Length = 0 Then lSoftPrefix = "" 'Just label soft limits Min and Max if there are no hard limits

            If SoftMin = ATCoDataType.NONE AndAlso SoftMax = ATCoDataType.NONE Then
                lSoftLimits = ""
            ElseIf SoftMin = ATCoDataType.NONE Then
                lSoftLimits = lSoftPrefix & "Max: " & SoftMax
            ElseIf SoftMax = ATCoDataType.NONE Then
                lSoftLimits = lSoftPrefix & "Min: " & SoftMin
            Else
                lSoftLimits = lSoftPrefix & "Min: " & SoftMin & " " & lSoftPrefix & "Max: " & SoftMax
            End If
            If lSoftLimits.Length > 0 Then
                If lToolText.Length > 0 Then
                    lToolText &= ", " & lSoftLimits
                Else
                    lToolText = lSoftLimits
                End If
            End If
        End If
        ToolTip1.SetToolTip(txtBox, lToolText)
    End Sub

    Private Sub txtBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBox.GotFocus
        'Logger.Dbg("txtBox:GotFocus")
        Tag = Text
        txtBox.SelectionStart = 0
        txtBox.SelectionLength = Len(txtBox.Text)
        SetToolTip()
    End Sub

    Private Sub txtBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBox.KeyDown
        Dim lKeyCode As Windows.Forms.Keys = e.KeyCode
        'Logger.Dbg("txtBox:KeyDown: " & KeyCode)
        'Dim newName As String = ""
        'Dim newColor As Integer
        If lKeyCode = Keys.Enter Then
            RaiseEvent CommitChange()
            lKeyCode = 0
        ElseIf lKeyCode = Keys.Escape Then
            Text = Tag
            RaiseEvent CommitChange()
            'ElseIf DataType = ATCoDataType.ATCoClr And KeyCode = Keys.Up Then
            '  testColor(False, txtBox.Text, newName, Color.FromArgb(newColor))
            'ElseIf DataType = ATCoDataType.ATCoClr And KeyCode = Keys.Down Then
            '  testColor(True, txtBox.Text, newName, Color.FromArgb(newColor))
        Else
            RaiseEvent KeyDown(lKeyCode, e.Shift)
        End If
        'If newName <> "" Then
        '  txtBox.Text = newName
        '  txtBox.BackColor = Color.FromArgb(newColor)
        '  If Color.FromArgb(newColor).GetBrightness > 0.4 Then
        '    txtBox.ForeColor = System.Drawing.Color.Black
        '  Else
        '    txtBox.ForeColor = System.Drawing.Color.White
        '  End If
        RaiseEvent CommitChange()
        'End If
    End Sub

    Private Sub txtBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBox.LostFocus
        'Logger.Dbg("txtBox:LostFocus:" & txtBox.Text & " " & DefVal)
        Dim newValue As String = Valid(txtBox.Text)
        privDefaultValue = newValue
        txtBox.Text = FormatValue(newValue)
        RaiseEvent CommitChange()
        'Dim i&
        'If txtBox.BackColor = OutsideSoftBg Then
        '  i = Logger.Msg("Value is out of Normal range", vbOKCancel)
        'ElseIf txtBox.BackColor = OutsideHardBg Then
        '  i = Logger.Msg("Value is out of Acceptable range", vbRetryCancel)
        'Else
        '  i = vbOK
        'End If
        'If i = vbRetry Then
        '  txtBox.SetFocus
        'ElseIf i = vbCancel Then
        '  txtBox.Text = OldValStr
        'End If
    End Sub

    Private Sub txtBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtBox.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            ShowRange()
        End If
    End Sub

    Private Sub txtBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBox.TextChanged
        With txtBox

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

            ElseIf DataType = ATCoDataType.ATCoDbl Then
                If Not IsNumeric(.Text) Then
                    If (.Text <> "-" And .Text <> "+" And .Text <> "-." And .Text <> "+." Or HardMin >= 0) And .Text <> "." Then .BackColor = OutsideHardBg
                    GoTo LeaveSub
                End If
            End If

            Dim Val As Double
            If Double.TryParse(.Text, Val) Then
                If BelowLimit(Val, SoftMin) Or AboveLimit(Val, SoftMax) Then .BackColor = OutsideSoftBg
                If BelowLimit(Val, HardMin) Or AboveLimit(Val, HardMax) Then .BackColor = OutsideHardBg
            Else
                .BackColor = OutsideHardBg
            End If
        End With
LeaveSub:
        RaiseEvent Change()
    End Sub

    Private Sub UserControl_EnterFocus()
        'Logger.Dbg("Control:EnterFocus")
    End Sub

    Private Sub UserControl_Initialize()
        'txtBox.Text = FormatValue(DefaultValue)
    End Sub

    Private Sub UserControl_InitProperties()
        HardMax = ATCoDataType.NONE
        HardMin = ATCoDataType.NONE
        SoftMax = ATCoDataType.NONE
        SoftMin = ATCoDataType.NONE
        MaxWidth = 20
        DataType = ATCoDataType.ATCoTxt
        DefaultValue = ""
        InsideLimitsBackground = System.Drawing.Color.White
        OutsideHardLimitBackground = Color.FromArgb(8421631)
        OutsideSoftLimitBackground = Color.FromArgb(8454143)
        txtBox.Text = FormatValue(privDefaultValue)
    End Sub

    Private Sub UserControl_Resize()
        txtBox.Width = Me.Width
        txtBox.Height = Me.Height
    End Sub

    Private Function Valid(ByVal aValue As String) As String
        Dim lMsgText As String = ""
        Dim lValid As String = aValue

        If DataType = ATCoDataType.ATCoTxt Then
            If HardMax > 0 And aValue.Length > HardMax Then
                lValid = aValue.Substring(0, HardMax)
                lMsgText = "The value '" & aValue & "' was too long." & vbCr _
                         & "Values can be at most " & HardMax & " characters long."
            End If
        ElseIf DataType = ATCoDataType.ATCoInt OrElse _
               DataType = ATCoDataType.ATCoDbl Then
            If aValue.Length > 0 Then
                Dim lDoubleValue As Double
                Dim lMaxWidth As Integer = MaxWidth
                If lMaxWidth < 1 Then lMaxWidth = 255
                If Double.TryParse(aValue, lDoubleValue) Then
                    aValue = DoubleToString(lDoubleValue, lMaxWidth, privNumericFormat, , "") 'We don't want to compare 0.1 with .1 and think they are different
                Else
                    lValid = privDefaultValue
                    lMsgText = "The value '" & aValue & "' is not numeric."
                    If Not Double.TryParse(privDefaultValue, lDoubleValue) Then
                        lValid = ""
                        GoTo FoundValid
                    End If
                End If
                If IsNumeric(privDefaultValue) Then
                    If privDefaultValue <> ATCoDataType.NONE AndAlso _
                      (HardMin = ATCoDataType.NONE OrElse privDefaultValue >= HardMin) And _
                      (HardMax = ATCoDataType.NONE OrElse privDefaultValue <= HardMax) Then
                        If lDoubleValue = ATCoDataType.NONE _
                          OrElse BelowLimit(lDoubleValue, HardMin) _
                          OrElse AboveLimit(lDoubleValue, HardMax) Then
                            lValid = privDefaultValue
                        End If
                    End If
                End If
                If BelowLimit(lDoubleValue, HardMin) Then lDoubleValue = HardMin
                If AboveLimit(lDoubleValue, HardMax) Then lDoubleValue = HardMax
                lValid = DoubleToString(lDoubleValue, lMaxWidth, privNumericFormat, , "")
                If lValid <> aValue Then
                    If lMsgText.Length = 0 Then
                        lMsgText = "The value '" & aValue & "' is outside the valid range. "
                    End If
                    If HardMin = ATCoDataType.NONE Then
                        lMsgText &= "Values must be less than or equal to " & HardMax
                    ElseIf HardMax = ATCoDataType.NONE Then
                        lMsgText &= "Values must be greater than or equal to " & HardMin
                    Else
                        lMsgText &= "Values must be between " & HardMin & " and " & HardMax
                    End If
                End If
            End If
        End If
        If lMsgText.Length > 0 Then
            lMsgText &= vbCr & "Value has been reset to " & lValid
            Logger.Msg(lMsgText, vbOKOnly, Name)
        End If
FoundValid:
        Return lValid
    End Function

    Private Function BelowLimit(ByVal testVal As Double, ByVal LimitVal As Double) As Boolean
        If LimitVal = ATCoDataType.NONE Then
            BelowLimit = False
        ElseIf LimitVal >= 0 Then
            If testVal * slop < LimitVal Then BelowLimit = True Else BelowLimit = False
        Else 'limitval < 0
            If testVal / slop < LimitVal Then BelowLimit = True Else BelowLimit = False
        End If
    End Function

    Private Function AboveLimit(ByVal testVal As Double, ByVal LimitVal As Double) As Boolean
        If LimitVal = ATCoDataType.NONE Then
            AboveLimit = False
        ElseIf LimitVal >= 0 Then
            If testVal / slop > LimitVal Then AboveLimit = True Else AboveLimit = False
        Else 'limitval < 0
            If testVal * slop > LimitVal Then AboveLimit = True Else AboveLimit = False
        End If
    End Function

    Private Function FormatValue(ByVal Val As String) As String
        Dim lFormattedValue As String = Val
        Select Case DataType
            Case ATCoDataType.ATCoTxt, ATCoDataType.NONE
                'no formatting for these types
            Case ATCoDataType.ATCoClr
                Dim clrName As String
                Dim tmpClr As Color = Color.Empty
                If IsNumeric(Val) Then
                    tmpClr = Color.FromArgb(CInt(Val))
                    clrName = colorName(tmpClr)
                Else
                    clrName = Val
                End If
                'Separate if statements in case colorName(val) comes back still numeric
                If tmpClr.Equals(Color.Empty) Then tmpClr = TextOrNumericColor(clrName)
                If IsNumeric(clrName) Then
                    lFormattedValue = ""
                Else
                    lFormattedValue = clrName
                End If
                txtBox.BackColor = tmpClr
                If tmpClr.GetBrightness > 0.4 Then
                    txtBox.ForeColor = System.Drawing.Color.Black
                Else
                    txtBox.ForeColor = System.Drawing.Color.White
                End If
                'txtBox.ForeColor = tmpLng Xor &HFFFFFF
            Case Else 'numeric - ATCoInt or ATCoDbl
                Dim lValDouble As Double
                If TypeOf (Val) Is String AndAlso Double.TryParse(Val, lValDouble) Then
                    Dim lMaxWidth As Integer = MaxWidth
                    If lMaxWidth < 1 Then
                        lFormattedValue = DoubleToString(lValDouble, 255, NumericFormat)
                    Else
                        lFormattedValue = DoubleToString(lValDouble, lMaxWidth, NumericFormat)
                    End If
                End If
        End Select
        Return lFormattedValue
    End Function

    Public Function ATCoTypeString(ByVal DataType As ATCoDataType) As String
        If DataType = ATCoDataType.ATCoTxt Then
            ATCoTypeString = "ATCoTxt"
        ElseIf DataType = ATCoDataType.ATCoInt Then
            ATCoTypeString = "ATCoInt"
        ElseIf DataType = ATCoDataType.ATCoDbl Then
            ATCoTypeString = "ATCoDbl"
        ElseIf DataType = ATCoDataType.NONE Then
            ATCoTypeString = "NONE"
        Else
            ATCoTypeString = "Undefined"
        End If
    End Function
End Class
