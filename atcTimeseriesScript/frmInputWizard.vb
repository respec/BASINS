Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports System.Drawing
Imports MapWinUtility.Strings

Friend Class frmInputWizard
    Inherits System.Windows.Forms.Form
    'Copyright 2002 by AQUA TERRA Consultants

    Public txtSample As New Generic.List(Of Windows.Forms.TextBox)

    Private Const conNumSampleLines As Short = 50

    Private Const conSashLimit As Short = 2100 'Sash can't get closer than this to top or bottom
    Private Const conMaxNumColumns As Short = 50

    Private Const ColMappingName As Short = 0
    Private Const ColMappingAttr As Short = 1
    Private Const ColMappingCol As Short = 2
    Private Const ColMappingConstant As Short = 3
    Private Const ColMappingSkip As Short = 4
    Private Const ColMappingLast As Short = 4

    Private FoundDDF As Boolean
    Private FoundPas As Boolean
    Private WriteHeader As Boolean

    Private UnitDescFile As Short
    Private NameDescFile As String

    Private CharWidth As Single = 10.0
    Private SettingSelFromGrid As Boolean

    Private nFixedCols As Integer
    Private hSashDragging As Boolean
    Private delim As String ' currently selected delimiter
    Private delimQ As Boolean ' boolean for whether file is delimited
    Private SiteDataMapped As Boolean
    Private DidMap As Boolean
    Private mbMoving As Boolean

    Private NameDataFile As String
    'Private UnitDataFile As Short 'file handle for reading data
    Private UnitDataFileReader As IO.StreamReader = Nothing
    'Private UnitDataFileWriter As IO.StreamWriter

    Private pMonitor As Object
    Private pMonitorSet As Boolean

    Private pTserFile As atcData.atcTimeseriesSource
    Private Script As clsATCscriptExpression

    Private RequiredFields As Object 'Array of names
    Private nRequiredFields As Integer

    Public WriteOnly Property Monitor() As Object
        Set(ByVal Value As Object)
            pMonitor = Value
            pMonitorSet = True
        End Set
    End Property

    Public WriteOnly Property TserFile() As atcData.atcTimeseriesSource
        Set(ByVal Value As atcData.atcTimeseriesSource)
            pTserFile = Value
        End Set
    End Property

    'UPGRADE_WARNING: Event chkSkipHeader.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub chkSkipHeader_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkSkipHeader.CheckStateChanged
        PopulateSample()
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOk.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        'UPGRADE_NOTE: Object Script may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        Script = Nothing
        Script = New clsATCscriptExpression

        If pMonitorSet Then
            'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pMonitor.SendMonitorMessage("(OPEN Reading " & NameDataFile & ")")
            'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pMonitor.SendMonitorMessage("(BUTTOFF CANCEL)")
            'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pMonitor.SendMonitorMessage("(BUTTOFF PAUSE)")
            ScriptSetMonitor(pMonitor)
        End If

        Script.ParseExpression(ScriptStringFromWizard)
        ScriptRun(Script, NameDataFile, pTserFile) ' MsgBox ScriptRun(Script, NameDataFile, pTserFile), vbOKOnly, "Ran Import Data Script"

        If pMonitorSet Then
            'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pMonitor.SendMonitorMessage("(CLOSE)")
            'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pMonitor.SendMonitorMessage("(BUTTON CANCEL)")
            'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pMonitor.SendMonitorMessage("(BUTTON PAUSE)")
        End If
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Close()
    End Sub

    'Displays the Input Wizard form and initializes
    'fields and objects used in the application.
    Private Sub frmInputWizard_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        'UPGRADE_WARNING: Couldn't resolve default property of object RequiredFields. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        RequiredFields = New Object() {"", "Value", "Year", "Month", "Day", "Hour", "Minute", "Scenario", "Location", "Constituent", "Description", "Repeating", "Repeats"}
        nRequiredFields = UBound(RequiredFields)
        InitDataMapping()

        'UPGRADE_ISSUE: Form method frmInputWizard.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        CharWidth = 10 'TODO: Me.TextWidth("X")


        ' initialize Input File Property defaults.
        'txtScriptFile.Text = ""
        'txtDataFile.Text = ""
        DidMap = False
        delim = txtDelimiter.Text
        DisableFilePropertiesFields()

        ' Left justify major display areas on form
        'fraTextSample.Left = tabTop.Left
        'fraColSample.Left = tabTop.Left
        SizeControls(fraSash.Top)
    End Sub

    'UPGRADE_WARNING: Event frmInputWizard.Resize may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    'Private Sub frmInputWizard_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
    '	SizeControls(VB6.PixelsToTwipsY(fraSash.Top))
    'End Sub

    'Resize all controls on the form
    Sub SizeControls(ByRef SashTop As Single)
        'Dim fraWidth As Integer
        'Dim tabWidth As Integer
        'Dim txtWidth As Integer
        Dim SampleHeight As Integer
        Dim EachSampleHeight As Integer
        'Dim sam As Integer
        ''Dim ButtonsTop As Long
        'Dim BotTop As Integer
        'Dim TopHeight As Integer
        Dim BottomHeight As Integer

        If txtSample.Count = 0 Then txtSample.Add(_txtSample_0)
        Dim lNewSampleTextbox As Windows.Forms.TextBox = txtSample(0)

        'On Error Resume Next

        'fraSash.Top = VB6.TwipsToPixelsY(SashTop)

        ''set the height of the top objects
        'tabTop.Height = VB6.TwipsToPixelsY(SashTop - 130)
        'TopHeight = SashTop - 700
        'fraTab(1).Height = VB6.TwipsToPixelsY(TopHeight)
        'fraTab(2).Height = VB6.TwipsToPixelsY(TopHeight)
        ''fraTab(3).Height = TopHeight
        'agdDataMapping.Height = VB6.TwipsToPixelsY(TopHeight)
        ''agdTestMapping.Height = TopHeight

        '' Set the top of frame containers for sample display.
        'fraTextSample.Top = VB6.TwipsToPixelsY(SashTop + VB6.PixelsToTwipsY(fraSash.Height))
        'fraColSample.Top = VB6.TwipsToPixelsY(SashTop + VB6.PixelsToTwipsY(fraSash.Height))

        '' Set the height of the bottom objects, do the frames
        '' then the items contained - 900 for buttons
        '' at the bottom.
        BottomHeight = Me.Height - SashTop - fraSash.Height

        'fraTextSample.Height = VB6.TwipsToPixelsY(BottomHeight)
        SampleHeight = BottomHeight - txtRuler1.Height - txtRuler2.Height - HScrollSample.Height
        'VScrollSample.Top = txtSample(0).Top
        'VScrollSample.Height = VB6.TwipsToPixelsY(SampleHeight)
        EachSampleHeight = lNewSampleTextbox.Height * 0.95
        'sam = txtSample.Count - 1
        'While sam > 0 And VB6.PixelsToTwipsY(txtSample(sam).Top) + EachSampleHeight > SampleHeight
        '	txtSample.Unload(sam)
        '	sam = sam - 1
        'End While

        While lNewSampleTextbox.Top < SampleHeight
            lNewSampleTextbox = New Windows.Forms.TextBox
            With lNewSampleTextbox
                fraTextSample.Controls.Add(lNewSampleTextbox)
                AddHandler .MouseDown, AddressOf txtSample_MouseDown
                AddHandler .MouseMove, AddressOf txtSample_MouseMove
                AddHandler .MouseUp, AddressOf txtSample_MouseUp
                .Top = txtSample(txtSample.Count - 1).Top + EachSampleHeight
                .Visible = True
                .Width = _txtSample_0.Width
                .Anchor = _txtSample_0.Anchor
                .Font = _txtSample_0.Font
                .Height = _txtSample_0.Height
                .BorderStyle = Windows.Forms.BorderStyle.None
                .HideSelection = False
                txtSample.Add(lNewSampleTextbox)
            End With
        End While
        PopulateTxtSample()
        'HScrollSample.Top = VB6.TwipsToPixelsY(BottomHeight - VB6.PixelsToTwipsY(HScrollSample.Height)) 'txtSample(txtSample.Count - 1).Top + txtSample(txtSample.Count - 1).Height
        'HScrollSample.BringToFront()
        'fraColSample.Height = VB6.TwipsToPixelsY(BottomHeight)
        'agdSample.Height = VB6.TwipsToPixelsY(BottomHeight - 300)

        'fraButtons.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 800) ' Position the bottom buttons

        ''set the width
        'tabWidth = VB6.PixelsToTwipsX(Me.Width) - 400
        'fraSash.Width = Me.Width
        'tabTop.Width = VB6.TwipsToPixelsX(tabWidth)
        'fraColSample.Width = VB6.TwipsToPixelsX(tabWidth)
        'agdSample.Width = VB6.TwipsToPixelsX(tabWidth)
        'fraTextSample.Width = VB6.TwipsToPixelsX(tabWidth)
        'txtWidth = tabWidth - 200
        'txtRuler1.Width = VB6.TwipsToPixelsX(txtWidth)
        'txtRuler2.Width = VB6.TwipsToPixelsX(txtWidth)
        'txtWidth = txtWidth - VB6.PixelsToTwipsX(VScrollSample.Width)
        'HScrollSample.Width = VB6.TwipsToPixelsX(txtWidth)
        'VScrollSample.Left = VB6.TwipsToPixelsX(txtWidth)
        'For sam = 0 To txtSample.Count - 1
        '	txtSample(sam).Width = VB6.TwipsToPixelsX(txtWidth)
        'Next sam

        'SetRulers()

        '' Expand both grids to slightly smaller than the tabTop width:
        'fraWidth = tabWidth - 325
        'fraTab(1).Width = VB6.TwipsToPixelsX(fraWidth)
        'fraTab(2).Width = VB6.TwipsToPixelsX(fraWidth)
        ''fraTab(3).Width = fraWidth
        'agdDataMapping.Width = VB6.TwipsToPixelsX(fraWidth)
        ''agdTestMapping.Width = fraWidth

    End Sub

    Private Sub SetRulers()
        Dim NumChars As Integer
        Dim RulerCount As Integer
        Dim RulerStringTemp As String
        Dim RulerString1 As String
        Dim RulerString2 As String

        RulerString1 = ""
        RulerString2 = ""
        NumChars = txtRuler2.Width / CharWidth

        'First, fill in possibly odd number of digits caused by scrolling
        For RulerCount = HScrollSample.Value Mod 10 To 9
            RulerString2 = RulerString2 & RulerCount
        Next RulerCount
        RulerString2 = RulerString2 & "0"

        RulerCount = (HScrollSample.Value + 10 - HScrollSample.Value Mod 10) \ 10
        RulerStringTemp = CStr(RulerCount)
        If Len(RulerString2) > Len(RulerStringTemp) Then
            RulerString1 = Space(Len(RulerString2) - Len(RulerStringTemp)) & RulerStringTemp
        Else
            RulerString1 = Space(Len(RulerString2))
        End If

        'Then fill in ten digits at a time until we have enough
        While Len(RulerString2) < NumChars
            RulerCount = RulerCount + 1
            RulerStringTemp = CStr(RulerCount)
            RulerStringTemp = Space(10 - Len(RulerStringTemp)) & RulerStringTemp
            RulerString1 = RulerString1 & RulerStringTemp
            RulerString2 = RulerString2 & "1234567890"
        End While
        RulerString2 = VB.Left(RulerString2, NumChars)
        RulerString1 = VB.Left(RulerString1, NumChars)
        txtRuler1.Text = RulerString1
        txtRuler2.Text = RulerString2
    End Sub

    'UPGRADE_NOTE: HScrollSample.Change was changed from an event to a procedure. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="4E2DC008-5EDA-4547-8317-C9316952674F"'
    'UPGRADE_WARNING: HScrollBar event HScrollSample.Change has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
    Private Sub HScrollSample_Change(ByVal newScrollValue As Integer)
        SetRulers()
        PopulateTxtSample()
    End Sub

    'UPGRADE_NOTE: HScrollSample.Scroll was changed from an event to a procedure. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="4E2DC008-5EDA-4547-8317-C9316952674F"'
    Private Sub HScrollSample_Scroll_Renamed(ByVal newScrollValue As Integer)
        HScrollSample_Change(0)
    End Sub

    Private Sub fraSash_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles fraSash.MouseDown
        'Dim Button As Short = eventArgs.Button \ &H100000
        'Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        'mbMoving = True
        'fraSash.BackColor = System.Drawing.SystemColors.ControlDark
    End Sub

    Private Sub fraSash_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles fraSash.MouseMove
        'Dim Button As Short = eventArgs.Button \ &H100000
        'Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        'Dim sglPos As Single

        'If mbMoving Then
        '	sglPos = y + VB6.PixelsToTwipsY(fraSash.Top)
        '	If sglPos < conSashLimit Then
        '		fraSash.Top = VB6.TwipsToPixelsY(conSashLimit)
        '	ElseIf sglPos > VB6.PixelsToTwipsY(Me.Height) - conSashLimit Then 
        '		fraSash.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - conSashLimit)
        '	Else
        '		fraSash.Top = VB6.TwipsToPixelsY(sglPos)
        '	End If
        'End If
    End Sub

    'time to move the controls
    Private Sub fraSash_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles fraSash.MouseUp
        'Dim Button As Short = eventArgs.Button \ &H100000
        'Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        'mbMoving = False
        'fraSash.BackColor = System.Drawing.SystemColors.Control
        'SizeControls(VB6.PixelsToTwipsY(fraSash.Top))
    End Sub

    Private Sub optHeader_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optHeaderNone.CheckedChanged, optHeaderStartsWith.CheckedChanged, optHeaderLines.CheckedChanged
        If sender.Checked Then PopulateSample()
    End Sub

    'ToDo: 
    'Private Sub agdDataMapping_CommitChange(ByVal eventSender As System.Object, ByVal eventArgs As AxATCoCtl.__ATCoGrid_CommitChangeEvent) Handles agdDataMapping.CommitChange
    '	If eventArgs.ChangeFromCol = ColMappingCol Then SetSelFromGrid(eventArgs.ChangeFromRow)
    '	With agdDataMapping
    '		If .MaxOccupiedRow = .rows Then .rows = .rows + 1
    '	End With
    'End Sub

    Private Sub agdDataMapping_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdDataMapping.MouseDownCell
        Static lastrow, lastcol As Integer
        Static InRowColChange As Boolean

        Dim ics As String
        Dim icl As Integer
        Dim newrow As Integer
        Dim newcol As Integer
        If InRowColChange Then Exit Sub
        InRowColChange = True
        With MappingSource

            If aRow <> lastrow Or aColumn <> lastcol Then
                newrow = aRow
                newcol = aColumn
                'If lastrow > nRequiredFields Or lastcol <> ColMappingName Then
                '    .Row = lastrow
                '    .Column = lastcol
                '    ''UPGRADE_NOTE: BackColor was upgraded to CtlBackColor. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
                '    '               .CellBackColor = .CtlBackColor
                '    ''UPGRADE_NOTE: ForeColor was upgraded to CtlForeColor. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
                '    '.CellForeColor = .CtlForeColor
                '    .Row = newrow
                '    .Column = newcol
                'End If
                If newrow > nRequiredFields Or newcol <> ColMappingName Then
                    '.CellBackColor = .BackColorSel
                    '.CellForeColor = .ForeColorSel
                    '.CellColor(newrow, newcol) = SystemColors.ControlDark
                End If
                Dim lUniqueValues As New ArrayList
                If newcol = ColMappingAttr Then
                    lUniqueValues.Add("yes")
                    lUniqueValues.Add("no")
                End If
                agdDataMapping.ValidValues = lUniqueValues
                lastrow = newrow
                lastcol = newcol
                SetSelFromGrid(newrow)
            End If
            .CellEditable(newrow, newcol) = (aRow > nRequiredFields OrElse aColumn <> ColMappingName)
        End With
        InRowColChange = False
    End Sub

    'Private Sub agdDataMapping_TextChange(ByVal eventSender As System.Object, ByVal eventArgs As AxATCoCtl.__ATCoGrid_TextChangeEvent) Handles agdDataMapping.TextChange
    '	EnableButtons()
    'End Sub

    Private Function FixedColLeft(ByRef index As Integer) As Integer
        FixedColLeft = ReadIntLeaveRest(Trim(MappingSource.CellValue(index, ColMappingCol)))
    End Function

    Private Function FixedColRight(ByRef index As Integer) As Integer
        'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim str_Renamed As String
        Dim pos As Integer
        str_Renamed = Trim(MappingSource.CellValue(index, ColMappingCol))
        pos = InStr(str_Renamed, "-")

        If pos > 0 Then
            FixedColRight = CInt(Mid(str_Renamed, pos + 1))
        Else
            pos = InStr(str_Renamed, "+")
            If pos > 0 Then
                FixedColRight = CInt(Mid(str_Renamed, pos + 1))
            ElseIf IsNumeric(str_Renamed) Then
                FixedColRight = CInt(str_Renamed)
            Else
                FixedColRight = 0
            End If
        End If
    End Function

    Private Sub SetSelFromGrid(ByRef row As Integer)

        Dim SelStart, SelLength As Integer
        SelStart = 0
        SelLength = 0

        If Not SettingSelFromGrid Then
            SettingSelFromGrid = True

            If delimQ Then 'Select column in agdSample

                SelStart = FixedColLeft(row)
                If SelStart > 0 Then
                    'If SelStart <> agdSample.SelStartCol Then
                    '    agdSample.set_Selected(1, SelStart - 1, True)
                    'End If
                End If

            Else 'Select column in txtSample
                SelStart = FixedColLeft(row) - 1
                If SelStart >= 0 Then
                    SelLength = FixedColRight(row) - SelStart
                    If SelStart < HScrollSample.Value Then
                        SelLength = SelLength - (HScrollSample.Value - SelStart) + 1
                        SelStart = HScrollSample.Value - 1
                    End If
                    If SelLength > 0 Then
                        txtRuler1.SelectionStart = SelStart - HScrollSample.Value + 1
                        txtRuler1.SelectionLength = SelLength
                    Else
                        txtRuler1.SelectionLength = 0
                    End If
                    txtSampleAnyChange(-1)
                End If
            End If

            SettingSelFromGrid = False
        End If
    End Sub


    ' Subroutine ===============================================
    ' Name:      EnableButtons
    ' Purpose:   Enables/Disables 3 command buttons at bottom
    '            of the form when fields are mapped.
    '
    ' Notes: This is the only place where DidMap can get set
    '        to true. DidMap true means all mapping enteries
    '        have been made and its OK for the program to
    '        proceed with data processing.
    '
    Private Sub EnableButtons()
        Dim Idx As Integer

        cmdCancel.Enabled = True

        With agdDataMapping
            For Idx = 1 To MappingSource.Rows
                'If Trim(Len(.TextMatrix(Idx, 3))) = 0 And _
                ''   Trim(Len(.TextMatrix(Idx, 4))) = 0 _
                ''Then
                '  DidMap = False
                '  cmdSaveDesc.Enabled = False
                '  cmdProcessData.Enabled = False
                '  cmdMakeGwsiTransactions.Enabled = False
                '  Exit Sub
                'End If
            Next
            DidMap = True

            cmdSaveDesc.Enabled = True
        End With

    End Sub
    ' Subroutine ===============================================
    ' Name:      DisableButtons
    ' Purpose:   Disables 3 command buttons at bottom
    '            of the form, used to turn off
    '            buttons as data is processed.
    '
    Private Sub DisableButtons()
        cmdSaveDesc.Enabled = False
        cmdCancel.Enabled = True
    End Sub

    ' Subroutine ===============================================
    ' Name:      SelectFieldsDone
    ' Purpose:   Signal passed from the SelectFields control
    '            to the form
    '
    Public Sub SelectFieldsDone()
        Dim m_Prompt As String
        Dim m_LenPro As Short

        EnableButtons()

        ' Reset and move the user prompt:
        '
        m_Prompt = " To map " & MappingSource.CellValue(1, ColMappingName) & " , click or enter a column number"
        m_LenPro = BoxWidth(m_Prompt)
        If m_LenPro > 7500 Then m_LenPro = 7500

        'ctlUserPrompt1.StackLines
        'ctlUserPrompt1.Width = m_LenPro
        'ctlUserPrompt1.Top = 0
        'ctlUserPrompt1.Left = cmdSelectFields.Left + _
        ''                      cmdSelectFields.Width + 35
        'ctlUserPrompt1.PromptLine1 m_Prompt
        'ctlUserPrompt1.Line2Off
        'ctlUserPrompt1.Visible = True

    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     agdSample_Click
    ' Purpose:  Responds to click on sample grid
    '
    Private Sub agdSample_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles agdSample.Click
        Dim m_Prompt As String
        Dim m_LenPro As Short
        '
        ' Don't change the mapping unless you are on the data mapping
        ' tab.
        '
        'If tabTop.SelectedItem.index <> 2 Then
        '    Exit Sub
        'End If

        ' Select column means put this column number in current row af mapping
        ' TODO: need replacements
        With agdSample
            'TODO: Determine if a single column in the grid is selected

            'If .SelStartCol = .SelEndCol And .SelStartRow = 1 And .SelEndRow = .Source.Rows Then
            '    MappingSource.CellValue(MappingSource.Rows, ColMappingCol) = .SelStartCol + 1
            '    'agdSample.ColTitle(agdSample.SelStartCol)
            'End If

        End With
        '
        ' This is called to enable or disable add and mod buttons
        ' it will only enable buttons when all data mapping is done.
        '
        EnableButtons()
    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     cmdBrowseDesc_Click
    ' Purpose:  Responds to press of lower "Browse" button
    '
    Private Sub cmdBrowseDesc_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrowseDesc.Click
        'UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        dlgOpenFileOpen.Filter = "Wizard Script Files (*.ws)|*.ws|All Files (*.*)|*.*"
        dlgOpenFileSave.Filter = "Wizard Script Files (*.ws)|*.ws|All Files (*.*)|*.*"
        dlgOpenFileOpen.DefaultExt = "ws"
        dlgOpenFileSave.DefaultExt = "ws"
        dlgOpenFileOpen.Title = "Open Script File"
        dlgOpenFileSave.Title = "Open Script File"
        dlgOpenFileOpen.ShowDialog()
        dlgOpenFileSave.FileName = dlgOpenFileOpen.FileName
        If dlgOpenFileOpen.FileName <> "" Then
            NameDescFile = dlgOpenFileOpen.FileName
            txtScriptFile.Text = NameDescFile
            ReadScript()
        End If
    End Sub

    Public Sub ReadScript()
        If Len(txtScriptFile.Text) > 0 Then
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Len(Dir(txtScriptFile.Text)) > 0 Then
                Script = ScriptFromString(WholeFileString((txtScriptFile.Text)))
                If Script Is Nothing Then
                    MsgBox("Could not parse " & txtScriptFile.Text & vbCr & Err.Description, MsgBoxStyle.Exclamation, "Read Script")
                Else
                    FoundDDF = True
                    SetWizardFromScript(Script)
                    agdDataMapping.SizeAllColumnsToContents(agdDataMapping.Width, True)
                End If
            End If
        End If
    End Sub

    Private Sub InitDataMapping()
        Dim lSource As New atcControls.atcGridSource
        With lSource
            '.ClearData() 'TODO: need replacement?
            .Columns = ColMappingLast + 1
            .Rows = nRequiredFields + 1
            .FixedRows = 1
            .CellValue(0, ColMappingName) = "Name"
            .CellValue(0, ColMappingAttr) = "Attribute"
            .CellValue(0, ColMappingCol) = "Input Column"
            .CellValue(0, ColMappingConstant) = "Constant"
            .CellValue(0, ColMappingSkip) = "Skip Values"
            For lRow As Integer = 1 To nRequiredFields
                .CellValue(lRow, 0) = RequiredFields(lRow)
                .CellColor(lRow, 0) = SystemColors.ControlDark
                .CellEditable(lRow, 0) = False
                Select Case RequiredFields(lRow)
                    Case "Year" : .CellValue(lRow, ColMappingConstant) = "1900"
                    Case "Day" : .CellValue(lRow, ColMappingConstant) = "1"
                    Case "Hour", "Minute", "Second" : .CellValue(lRow, ColMappingConstant) = "0"

                    Case "Scenario", "Location", "Constituent", "Description"
                        .CellValue(lRow, ColMappingAttr) = "yes"
                    Case Else : .CellValue(lRow, ColMappingAttr) = "no"
                End Select
            Next
        End With
        agdDataMapping.Initialize(lSource)
        agdDataMapping.SizeAllColumnsToContents(agdDataMapping.Width, True)
    End Sub

    Private Sub SetColumnFormatFromScript(ByRef scr As clsATCscriptExpression)
        Dim rule, lrule As String
        Dim SubExpIndex, ColIndex, SubExpMax As Integer
        Dim ColName, tmpstr, ColCol As String
        Dim StartCol, ColWidth As Integer
        Dim colonPos, r As Integer
        ColIndex = 1
        rule = scr.SubExpression(0).Printable.Trim("""")
        InitDataMapping()

        FixedColumns = False
        ColumnDelimiter = ""
        If IsNumeric(rule) Then
            ColumnDelimiter = Chr(CShort(rule))
        Else
            lrule = Trim(LCase(rule))
            If lrule = "fixed" Then
                FixedColumns = True
            Else
                If InStr(lrule, "tab") Then ColumnDelimiter = ColumnDelimiter & vbTab
                If InStr(lrule, "space") Then ColumnDelimiter = ColumnDelimiter & " "
                For StartCol = 33 To 126
                    Select Case StartCol
                        Case 48 : StartCol = 58
                        Case 65 : StartCol = 91
                        Case 97 : StartCol = 123
                    End Select
                    If InStr(lrule, Chr(StartCol)) > 0 Then
                        ColumnDelimiter = ColumnDelimiter & Chr(StartCol)
                    End If
                Next StartCol
                NumColumnDelimiters = Len(ColumnDelimiter)
            End If
        End If

        SubExpMax = scr.SubExpressionCount - 1
        SubExpIndex = 1
        While SubExpIndex <= SubExpMax
            rule = scr.SubExpression(SubExpIndex).Printable
            ColCol = ""
            If FixedColumns Then ' start-end:name or start+len:name
ParseFixedDef:
                StartCol = ReadIntLeaveRest(rule)
                tmpstr = VB.Left(rule, 1)
                If tmpstr = ":" Then
                    ColWidth = 1
                Else
                    rule = Mid(rule, 2)
                    ColWidth = ReadIntLeaveRest(rule)
                    If tmpstr = "-" Then ColWidth = ColWidth - StartCol + 1
                End If
                If ColWidth < 2 Then
                    ColCol = CStr(StartCol)
                Else
                    ColCol = StartCol & "-" & ColWidth + StartCol - 1
                End If
                rule = Mid(rule, 2)
            Else 'delimited definition - expect colNum:name or name
                colonPos = InStr(rule, ":")
                If colonPos > 0 Then
                    tmpstr = VB.Left(rule, colonPos - 1)
                    If IsNumeric(tmpstr) Then
                        ColCol = tmpstr
                        rule = Mid(rule, colonPos + 1)
                    End If
                End If
            End If
            ColName = rule
            r = RowNamed(ColName)
            With agdDataMapping
                .Source.CellValue(r, ColMappingName) = ColName
                .Source.CellValue(r, ColMappingCol) = ColCol
                .Source.CellValue(r, ColMappingConstant) = ""
            End With
            SubExpIndex = SubExpIndex + 1
        End While
        If FixedColumns Then
            optDelimiter_CheckedChanged(optDelimiterNone, New System.EventArgs())
            MappingSource.Rows = 1
            SetSelFromGrid(1)
        ElseIf ColumnDelimiter = " " Then
            optDelimiter_CheckedChanged(optDelimiterSpace, New System.EventArgs())
        ElseIf ColumnDelimiter = vbTab Then
            optDelimiter_CheckedChanged(optDelimiterTab, New System.EventArgs())
        Else
            txtDelimiter.Text = ColumnDelimiter
            optDelimiterChar.Checked = True
            'optDelimiter_CheckedChanged(optDelimiterChar, New System.EventArgs())
        End If
        'agdDataMapping.ColsSizeByContents() 
        'agdDataMapping.SizeColumnToContents() 'TODO: this seems to be the replacement of line above?? Apply to all columns?
    End Sub

    'Finds named row in grid by non-case-sensitive comparison ignoring whitespace
    'If a blank row is found at the end of the grid, that row is returned
    'If no row matches, .rows + 1 is returned
    Private Function RowNamed(ByRef FieldName As String) As Integer
        Dim srchName, thisName As String
        Dim r, maxRow As Integer

        With MappingSource
            maxRow = .Rows
            srchName = Trim(LCase(FieldName))
            For r = 1 To maxRow
                If Trim(LCase(.CellValue(r, ColMappingName))) = srchName Then Exit For
            Next r
            If r > maxRow Then
                If Trim(.CellValue(maxRow, ColMappingName)) = "" Then r = maxRow
            End If
        End With
        RowNamed = r
    End Function

    Private Sub SetDatePortion(ByRef scr As clsATCscriptExpression, ByRef subexpName As String)
        Dim r, otherRow As Integer
        Dim scp As String
        r = RowNamed(subexpName)
        If r <= MappingSource.Rows Then
            scp = TrimQuotes(scr.Printable)
            If LCase(scp) <> LCase(subexpName) Then
                MappingSource.CellValue(r, ColMappingConstant) = scp
            End If
        End If
    End Sub

    Private Sub SetWizardFromDate(ByRef scr As clsATCscriptExpression)
        Dim SubExp, cnt, r As Integer
        Dim subexpName As String
        cnt = scr.SubExpressionCount
        If cnt > 0 Then SetDatePortion(scr.SubExpression(0), "Year")
        If cnt > 1 Then SetDatePortion(scr.SubExpression(1), "Month")
        If cnt > 2 Then SetDatePortion(scr.SubExpression(2), "Day")
        If cnt > 3 Then SetDatePortion(scr.SubExpression(3), "Hour")
        If cnt > 4 Then SetDatePortion(scr.SubExpression(4), "Minute")
        If cnt > 5 Then SetDatePortion(scr.SubExpression(5), "Second")
    End Sub

    Public Property MappingSource() As atcControls.atcGridSource
        Get
            If agdDataMapping.Source Is Nothing Then
                InitDataMapping()
            End If
            Return agdDataMapping.Source
        End Get
        Set(ByVal value As atcControls.atcGridSource)
            agdDataMapping.Initialize(value)
        End Set
    End Property

    Public Sub SetWizardFromScript(ByRef scr As clsATCscriptExpression)
        Static ReverseLogic As Boolean
        'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim SubExp, ForMax, r As Integer
        Dim str_Renamed, str2 As String
        Select Case scr.Token
            Case clsATCscriptExpression.ATCsToken.tok_And
                ForMax = scr.SubExpressionCount - 1
                For SubExp = 0 To ForMax
                    SetWizardFromScript(scr.SubExpression(SubExp))
                Next
            Case clsATCscriptExpression.ATCsToken.tok_ATCScript
                ForMax = scr.SubExpressionCount - 1
                txtScriptDesc.Text = TrimQuotes(scr.SubExpression(1).Printable)
                For SubExp = 1 To ForMax
                    SetWizardFromScript(scr.SubExpression(SubExp))
                Next
            Case clsATCscriptExpression.ATCsToken.tok_Attribute
                str_Renamed = scr.SubExpression(0).Printable
                r = RowNamed(str_Renamed)
                MappingSource.CellValue(r, ColMappingName) = str_Renamed
                MappingSource.CellValue(r, ColMappingAttr) = "yes"
                str2 = Trim(scr.SubExpression(1).Printable)
                If VB.Left(str2, 1) = """" Then str2 = Mid(str2, 2)
                If VB.Right(str2, 1) = """" Then str2 = VB.Left(str2, Len(str2) - 1)
                MappingSource.CellValue(r, ColMappingConstant) = str2
            Case clsATCscriptExpression.ATCsToken.tok_ColumnFormat : SetColumnFormatFromScript(scr)
            Case clsATCscriptExpression.ATCsToken.tok_Dataset
                ForMax = scr.SubExpressionCount - 1
                SubExp = 0
                While SubExp < ForMax
                    str_Renamed = scr.SubExpression(SubExp).Printable
                    r = RowNamed(str_Renamed)
                    MappingSource.CellValue(r, ColMappingName) = str_Renamed
                    MappingSource.CellValue(r, ColMappingAttr) = "yes"
                    SubExp = SubExp + 1
                    If scr.SubExpression(SubExp).Token = clsATCscriptExpression.ATCsToken.tok_Literal Then
                        str2 = TrimQuotes(scr.SubExpression(SubExp).Printable)
                        MappingSource.CellValue(r, ColMappingConstant) = str2
                    End If
                    SubExp = SubExp + 1
                End While
            Case clsATCscriptExpression.ATCsToken.tok_Date : SetWizardFromDate(scr)
            Case clsATCscriptExpression.ATCsToken.tok_FatalError
            Case clsATCscriptExpression.ATCsToken.tok_For
                If LCase(Trim(scr.SubExpression(0).Printable)) = "repeat" AndAlso scr.SubExpression(2).Token = clsATCscriptExpression.ATCsToken.tok_Literal Then
                    r = RowNamed("Repeats")
                    MappingSource.CellValue(r, ColMappingConstant) = TrimQuotes(scr.SubExpression(2).Printable)
                End If
                ForMax = scr.SubExpressionCount - 1
                For SubExp = 3 To ForMax
                    SetWizardFromScript(scr.SubExpression(SubExp))
                Next
            Case clsATCscriptExpression.ATCsToken.tok_If
                ForMax = scr.SubExpressionCount - 1
                For SubExp = 0 To ForMax
                    SetWizardFromScript(scr.SubExpression(SubExp))
                Next
            Case clsATCscriptExpression.ATCsToken.tok_In
                r = RowNamed(scr.SubExpression(0).Printable)
                If r <= MappingSource.Rows Then
                    ForMax = scr.SubExpressionCount - 1
                    SubExp = 1
                    str_Renamed = ""
                    While SubExp <= ForMax
                        str_Renamed = str_Renamed & scr.SubExpression(SubExp).Printable
                        SubExp = SubExp + 1
                        If SubExp <= ForMax Then str_Renamed = str_Renamed & ","
                    End While
                    If ReverseLogic Then
                        MappingSource.CellValue(r, ColMappingSkip) = str_Renamed
                    End If
                End If
            Case clsATCscriptExpression.ATCsToken.tok_Increment
            Case clsATCscriptExpression.ATCsToken.tok_LineEnd
                str_Renamed = UCase(scr.SubExpression(0).Printable)
                If IsNumeric(str_Renamed) Then
                    txtLineLen.Text = str_Renamed
                    optLineEndLength.Checked = True
                ElseIf VB.Left(str_Renamed, 1) = "A" And IsNumeric(Mid(str_Renamed, 2)) Then
                    txtLineEndChar.Text = Mid(str_Renamed, 2)
                    optLineEndASCII.Checked = True
                ElseIf str_Renamed = "CR" Then
                    optLineEndCRLF.Checked = True
                ElseIf str_Renamed = "LF" Then
                    optLineEndLF.Checked = True
                Else : MsgBox("Unknown LineEnd '" & str_Renamed & "' in SetWizardFromScript")
                End If
            Case clsATCscriptExpression.ATCsToken.tok_Literal
            Case clsATCscriptExpression.ATCsToken.tok_Mid
            Case clsATCscriptExpression.ATCsToken.tok_Not
                ReverseLogic = Not ReverseLogic
                SetWizardFromScript(scr.SubExpression(0))
                ReverseLogic = Not ReverseLogic
            Case clsATCscriptExpression.ATCsToken.tok_NextLine
                If scr.SubExpressionCount = 1 Then
                    chkSkipHeader.CheckState = System.Windows.Forms.CheckState.Checked
                    optHeaderLines.Checked = True
                    txtHeaderLines.Text = scr.SubExpression(0).Printable
                End If
            Case clsATCscriptExpression.ATCsToken.tok_Set
            Case clsATCscriptExpression.ATCsToken.tok_Value
            Case clsATCscriptExpression.ATCsToken.tok_Variable
            Case clsATCscriptExpression.ATCsToken.tok_Warn
            Case clsATCscriptExpression.ATCsToken.tok_While
                If scr.SubExpressionCount = 2 Then
                    If scr.SubExpression(1).Token = clsATCscriptExpression.ATCsToken.tok_NextLine Then
                        str_Renamed = scr.SubExpression(0).Printable
                        SubExp = Len("(= HeaderStart ")
                        If VB.Left(str_Renamed, SubExp) = "(= HeaderStart " Then
                            chkSkipHeader.CheckState = System.Windows.Forms.CheckState.Checked
                            optHeaderStartsWith.Checked = True
                            txtHeaderStart.Text = Mid(str_Renamed, SubExp + 1, Len(str_Renamed) - SubExp - 1)
                            Exit Sub
                        End If
                    End If
                End If
                ForMax = scr.SubExpressionCount - 1
                For SubExp = 1 To ForMax
                    SetWizardFromScript(scr.SubExpression(SubExp))
                Next
            Case clsATCscriptExpression.ATCsToken.tok_GT
            Case clsATCscriptExpression.ATCsToken.tok_GE
            Case clsATCscriptExpression.ATCsToken.tok_LT
            Case clsATCscriptExpression.ATCsToken.tok_LE
            Case clsATCscriptExpression.ATCsToken.tok_NE
                r = RowNamed(scr.SubExpression(0).Printable)
                If r <= MappingSource.Rows Then
                    If Not ReverseLogic Then
                        MappingSource.CellValue(r, ColMappingSkip) = scr.SubExpression(1).Printable
                    End If
                End If
            Case clsATCscriptExpression.ATCsToken.tok_EQ
                r = RowNamed(scr.SubExpression(0).Printable)
                If r <= MappingSource.Rows Then
                    If ReverseLogic Then
                        MappingSource.CellValue(r, ColMappingSkip) = scr.SubExpression(1).Printable
                    End If
                End If
            Case Else
        End Select

    End Sub

    'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Function TrimQuotes(ByRef str_Renamed As String) As String
        Dim retval As String
        retval = Trim(str_Renamed)
        If VB.Left(retval, 1) = """" Then retval = Mid(retval, 2)
        If VB.Right(retval, 1) = """" Then retval = VB.Left(retval, Len(retval) - 1)
        TrimQuotes = retval
    End Function

    Private Function ScriptStringFromWizard() As String
        'UPGRADE_NOTE: str was upgraded to str_Renamed. 
        Dim tmpstr, str_Renamed, tmpstr2 As String
        Dim ParsePos As Integer
        Dim indent, indentIncrement As Integer
        Dim RepeatStart, RepeatEnd As Integer
        Dim NestedIfs, r, commaPos, CurrentIf As Integer
        Dim SomeAttribVaries As Boolean 'True if at least one attribute is not constant
        Dim SomeAttribVariesRepeat As Boolean 'True if an attribute varies within a line

        indent = 2
        indentIncrement = 2
        RepeatStart = 0
        RepeatEnd = 0
        str_Renamed = "(ATCScript "
        tmpstr = Trim(txtScriptDesc.Text)
        If tmpstr = "" Then tmpstr = IO.Path.GetFileName((txtScriptFile.Text))
        If tmpstr = "" Then tmpstr = "ReadData"
        str_Renamed = str_Renamed & """" & tmpstr & """"
        str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(LineEnd "
        If optLineEndCRLF.Checked = True Then
            str_Renamed = str_Renamed & "CR"
        ElseIf optLineEndLF.Checked = True Then
            str_Renamed = str_Renamed & "LF"
        ElseIf optLineEndASCII.Checked = True Then
            str_Renamed = str_Renamed & "A" & Trim(txtLineEndChar.Text)
        ElseIf optLineEndLength.Checked = True Then
            str_Renamed = str_Renamed & Trim(txtLineLen.Text)
        End If
        str_Renamed = str_Renamed & ")" & PrintEOL

        If chkSkipHeader.CheckState = System.Windows.Forms.CheckState.Checked Then
            If optHeaderStartsWith.Checked Then 'Starts With
                str_Renamed = str_Renamed & Space(indent) & "(While (= HeaderStart """ & txtHeaderStart.Text & """)" & PrintEOL
                str_Renamed = str_Renamed & Space(indent) & "       (NextLine))" & PrintEOL
            ElseIf optHeaderLines.Checked Then  'Number of lines
                str_Renamed = str_Renamed & Space(indent) & "(NextLine " & txtHeaderLines.Text & ")" & PrintEOL
            End If
        End If

        str_Renamed = str_Renamed & Space(indent) & "(ColumnFormat "
        indent = indent + Len("(ColumnFormat ")
        If Not delimQ Then
            str_Renamed = str_Renamed & "Fixed"
        Else 'delimited
            For ParsePos = 1 To Len(delim)
                tmpstr = Mid(delim, ParsePos, 1)
                Select Case tmpstr
                    Case vbTab : str_Renamed = str_Renamed & "tab"
                    Case " " : str_Renamed = str_Renamed & "space"
                    Case Else : str_Renamed = str_Renamed & tmpstr
                End Select
            Next
        End If
        If optHeaderStartsWith.Checked Then 'Starts With
            str_Renamed = str_Renamed & PrintEOL & Space(indent) & "1"
            If Len(txtHeaderStart.Text) > 1 Then str_Renamed = str_Renamed & "-" & Len(txtHeaderStart.Text) - 1
            str_Renamed = str_Renamed & ":" & "HeaderStart"
        End If
        For r = 1 To MappingSource.Rows
            tmpstr = Trim(MappingSource.CellValue(r, ColMappingCol))
            If tmpstr <> "" Then
                tmpstr2 = Trim(MappingSource.CellValue(r, ColMappingName))
                str_Renamed = str_Renamed & PrintEOL & Space(indent) & tmpstr
                str_Renamed = str_Renamed & ":" & tmpstr2

                If LCase(tmpstr2) = "repeating" Then
                    RepeatStart = FixedColLeft(r)
                    RepeatEnd = FixedColRight(r)
                End If
            End If
        Next
        str_Renamed = str_Renamed & ")"
        indent = indent - Len("(ColumnFormat ")

        'Figure out whether there is more than one dataset and if so whether it may change within a line
        SomeAttribVaries = False
        SomeAttribVariesRepeat = False
        For r = 1 To MappingSource.Rows
            tmpstr = LCase(Trim(MappingSource.CellValue(r, ColMappingAttr)))
            tmpstr2 = Trim(MappingSource.CellValue(r, ColMappingCol))
            If VB.Left(tmpstr, 1) = "y" And tmpstr2 <> "" Then
                SomeAttribVaries = True
                If RepeatStart <= FixedColLeft(r) And RepeatEnd >= FixedColRight(r) Then
                    SomeAttribVariesRepeat = True
                End If
            End If
        Next

        If Not SomeAttribVaries Then
            For r = 1 To MappingSource.Rows
                tmpstr = Trim(MappingSource.CellValue(r, ColMappingConstant))
                If tmpstr <> "" Then
                    tmpstr2 = Trim(MappingSource.CellValue(r, ColMappingCol))
                    If tmpstr2 = "" Then
                        If LCase(VB.Left(Trim(MappingSource.CellValue(r, ColMappingAttr)), 1)) = "y" Then
                            str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(Attribute "
                        Else
                            str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(Set "
                        End If
                        str_Renamed = str_Renamed & Trim(MappingSource.CellValue(r, ColMappingName))
                        str_Renamed = str_Renamed & " """ & tmpstr & """)"
                    End If
                End If
            Next
        End If

        str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(While (Not EOF)"
        indent = indent + Len("(While ")

        'Have to make sure this datum goes into the right dataset
        'UPGRADE_ISSUE: GoSub statement is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C5A1A479-AB8B-4D40-AAF4-DB19A2E5E77F"'
        If SomeAttribVaries And Not SomeAttribVariesRepeat Then
            SelectDataset((str_Renamed), (indent))
        End If

        If RepeatStart > 0 And RepeatEnd >= RepeatStart Then
            '    str = str & printeol & Space(indent) & "(Set Repeat 0)"
            '    str = str & printeol & Space(indent) & "(While (Not EOL)"
            '    indent = indent + Len("(While ")
            '    str = str & printeol & Space(indent) & "(Increment Repeat)"
            'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(Repeats). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(For Repeat = 1 to " & ConstOrCol("Repeats")
            indent = indent + Len("(For ")
        End If

        NestedIfs = 0
        For r = 1 To MappingSource.Rows
            tmpstr = Trim(MappingSource.CellValue(r, ColMappingSkip))
            If tmpstr <> "" Then NestedIfs = NestedIfs + 1
        Next r

        If NestedIfs > 0 Then
            str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(If "
            indent = indent + Len("(If ")
            CurrentIf = 1
            If NestedIfs > 1 Then
                str_Renamed = str_Renamed & "(And "
                indent = indent + Len("(And ")
            End If
            For r = 1 To MappingSource.Rows
                tmpstr = Trim(MappingSource.CellValue(r, ColMappingSkip))
                If tmpstr <> "" Then
                    commaPos = InStr(tmpstr, ",")
                    If commaPos = 0 Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        str_Renamed = str_Renamed & "(<> " & ConstOrCol(Trim(MappingSource.CellValue(r, ColMappingName))) & " " & tmpstr & ")"
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        str_Renamed = str_Renamed & "(Not (In " & ConstOrCol(Trim(MappingSource.CellValue(r, ColMappingName)))
                        While commaPos > 0
                            str_Renamed = str_Renamed & " " & VB.Left(tmpstr, commaPos - 1)
                            If commaPos > Len(tmpstr) Then
                                commaPos = 0
                            Else
                                tmpstr = Mid(tmpstr, commaPos + 1)
                                commaPos = InStr(tmpstr, ",")
                                If commaPos = 0 Then commaPos = Len(tmpstr) + 1
                            End If
                        End While
                        str_Renamed = str_Renamed & "))"
                    End If
                    If CurrentIf < NestedIfs Then str_Renamed = str_Renamed & PrintEOL & Space(indent)
                    CurrentIf = CurrentIf + 1
                End If
            Next r
            If NestedIfs > 1 Then
                indent = indent - Len("(And ")
                str_Renamed = str_Renamed & PrintEOL & Space(indent) & ")"
            End If
        End If

        'Have to make sure this datum goes into the right dataset
        'UPGRADE_ISSUE: GoSub statement is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C5A1A479-AB8B-4D40-AAF4-DB19A2E5E77F"'
        If SomeAttribVariesRepeat Then
            SelectDataset((str_Renamed), (indent))
        End If
        str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(Date "
        indent = indent + Len("(Date ")
        'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        str_Renamed = str_Renamed & ConstOrCol("Year")
        'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(Month). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        str_Renamed = str_Renamed & PrintEOL & Space(indent) & ConstOrCol("Month")
        'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(Day). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        str_Renamed = str_Renamed & PrintEOL & Space(indent) & ConstOrCol("Day")
        'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(Hour). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        str_Renamed = str_Renamed & PrintEOL & Space(indent) & ConstOrCol("Hour")
        'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(Minute). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        str_Renamed = str_Renamed & PrintEOL & Space(indent) & ConstOrCol("Minute") & ")"
        'str = str & printeol & Space(indent) & ConstOrCol("Second") & ")"
        indent = indent - Len("(Date ")

        'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(Value). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(Value " & ConstOrCol("Value") & ")"

        'For r = 1 To NestedIfs
        If NestedIfs > 0 Then
            indent = indent - Len("(If ")
            str_Renamed = str_Renamed & PrintEOL & Space(indent) & ")"
        End If
        'Next r

        If RepeatStart > 0 And RepeatEnd >= RepeatStart Then
            indent = indent - Len("(For ")
            str_Renamed = str_Renamed & PrintEOL & Space(indent) & ")"
        End If

        str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(NextLine)"
        indent = indent - Len("(While ")
        str_Renamed = str_Renamed & PrintEOL & Space(indent) & ")"

        str_Renamed = str_Renamed & PrintEOL & ")"

        ScriptStringFromWizard = str_Renamed
    End Function

    ''' <summary>
    ''' This is originally a GoSub section, hence the need for ByRef arguments
    ''' </summary>
    ''' <param name="str_Renamed"></param>
    ''' <param name="indent"></param>
    ''' <remarks></remarks>
    Private Sub SelectDataset(ByRef str_Renamed As String, ByRef indent As Integer)
        str_Renamed = str_Renamed & PrintEOL & Space(indent) & "(Dataset "
        indent = indent + Len("(Dataset ")
        Dim tmpstr As String = String.Empty
        Dim tmpstr2 As String = String.Empty

        For r = 1 To MappingSource.Rows
            tmpstr = LCase(Trim(MappingSource.CellValue(r, ColMappingAttr)))
            If VB.Left(tmpstr, 1) = "y" Then
                tmpstr2 = Trim(MappingSource.CellValue(r, ColMappingName))
                'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tmpstr = ConstOrCol(tmpstr2)
                If tmpstr <> "" Then
                    If VB.Right(str_Renamed, Len("(Dataset ")) <> "(Dataset " Then str_Renamed = str_Renamed & PrintEOL & Space(indent)
                    str_Renamed = str_Renamed & tmpstr2 & " " & tmpstr
                End If
            End If
        Next
        str_Renamed = str_Renamed & ")"
        indent = indent - Len("(Dataset ")
        'UPGRADE_WARNING: Return has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        'Return

    End Sub

    Private Function ConstOrCol(ByRef FieldName As String) As Object
        'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim r As Integer
        Dim constStr, searchstr, str_Renamed, colStr, retval As String
        retval = ""
        If LCase(FieldName) = "repeat" Then
            retval = FieldName
        Else
            r = RowNamed(FieldName)
            If r > MappingSource.Rows Then
                'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ConstOrCol = """" & FieldName & """"
            Else
                colStr = Trim(MappingSource.CellValue(r, ColMappingCol))
                If Len(colStr) > 0 Then colStr = FieldName
                retval = colStr
                constStr = Trim(MappingSource.CellValue(r, ColMappingConstant))
                If Len(constStr) > 0 Then
                    If LCase(constStr) = "repeat" Then
                        retval = constStr
                    Else
                        Select Case VB.Left(constStr, 1)
                            Case "+", "-", "*", "/", "^"
                                retval = "(" & VB.Left(constStr, 1)
                                If colStr <> "" Then retval = retval & " " & colStr
                                retval = retval & " " & Trim(Mid(constStr, 2)) & ")"
                            Case Else
                                If retval = "" Then retval = """" & constStr & """"
                        End Select
                    End If
                End If
            End If
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ConstOrCol = retval
    End Function

    ' Subroutine ===============================================
    ' Name:      DisableFilePropertiesFields
    ' Purpose:   Disable most fields on the "File-Properties"
    '            tab until the data file has been successfully
    '            opened.
    '
    ' Notes: this routine is called to initialize the program
    '        prior to the entry of an input data file name.
    '        once a valid name is entered the routine
    '        EnableFilePropertiesFields is called to allow the
    '        user to continue.
    '
    '        Disable "Data-Mapping" and "Test-Mapping" tabs
    '        until the data file has been successfully opened.
    '
    Private Sub DisableFilePropertiesFields()
        'FoundData = False
        FoundDDF = False
        FoundPas = False
        ' +------------------------------------------------------------+
        ' | "sstInputWizard.Enabled = False"  <-  Disables everything!  |
        ' | Alternative: Handle this in "sstInputWizard_Click" event    |
        ' +------------------------------------------------------------+
        'lblDataDescFile.Enabled = False
        'txtScriptFile.Enabled = False
        'cmdBrowseDesc.Enabled = False
        'lblFileType.Enabled = False
        'lblDelimiter.Enabled = False
        'txtDelimiter.Enabled = False
        'chkCollapseDelim.Enabled = False
        'optDelimiter.Item(0).Enabled = False
        'optDelimiter.Item(1).Enabled = False
        'optDelimiter.Item(2).Enabled = False
        'lblLinesToSkip.Enabled = False
        'txtLinesToSkip.Enabled = False
        'lblHeaderRecord.Enabled = False
        'chkHeaderRecord.Enabled = False
        'lblQuoteChar.Enabled = False
        'txtQuoteChar.Enabled = False
        'lblNullChar.Enabled = False
        'txtNullChar.Enabled = False
        'cmdSaveDesc.Enabled = False
    End Sub

    ' Subroutine ===============================================
    ' Name:      EnableFilePropertiesFields
    ' Purpose:   Enable fields on the File-Properties tab
    '            when data file has been opened
    '
    Private Sub EnableFilePropertiesFields()
        Dim m_Prompt As String
        Dim m_LenPro As Short

        'zzz this is not needed ?  PopulateGridSample
        lblDataDescFile.Enabled = True
        txtScriptFile.Enabled = True
        cmdBrowseDesc.Enabled = True
        'lblFileType.Enabled = True
        'lblDelimiter.Enabled = True
        'txtDelimiter.Enabled = True
        'chkCollapseDelim.Enabled = True
        'optDelimiter.Item(0).Enabled = True
        'optDelimiter.Item(1).Enabled = True
        'optDelimiter.Item(2).Enabled = True
        'lblLinesToSkip.Enabled = True
        'txtLinesToSkip.Enabled = True
        'lblHeaderRecord.Enabled = True
        'chkHeaderRecord.Enabled = True
        '
        ' Note: Leave these fields disabled until the functionality
        ' has been programmed.
        '
        ' lblQuoteChar.Enabled = True
        ' txtQuoteChar.Enabled = True
        ' lblNullChar.Enabled = True
        ' txtNullChar.Enabled = True

    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     cmdCancel_Click
    ' Purpose:  Responds to press of "Cancel" button
    '
    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     cmdSaveDesc_Click
    ' Purpose:  Responds to press of "Save Description" button
    '
    Private Sub cmdSaveDesc_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSaveDesc.Click
        Dim ScriptString, ScriptFilename As String
        'If MsgBox(ScriptString, vbYesNo, "Save this script?") = vbYes Then
        'UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        dlgOpenFileOpen.Filter = "Wizard Script Files (*.ws)|*.ws|All Files|*.*"
        dlgOpenFileSave.Filter = "Wizard Script Files (*.ws)|*.ws|All Files|*.*"
        dlgOpenFileOpen.DefaultExt = "ws"
        dlgOpenFileSave.DefaultExt = "ws"
        dlgOpenFileOpen.Title = "Save Script As"
        dlgOpenFileSave.Title = "Save Script As"
        dlgOpenFileOpen.FileName = txtScriptFile.Text
        dlgOpenFileSave.FileName = txtScriptFile.Text
        dlgOpenFileSave.ShowDialog()
        dlgOpenFileOpen.FileName = dlgOpenFileSave.FileName
        ScriptFilename = dlgOpenFileOpen.FileName
        'Dim OutFile As Short
        Dim lOutFileStream As IO.StreamWriter
        If ScriptFilename <> "" Then
            If InStr(ScriptFilename, ".") = 0 Then ScriptFilename = ScriptFilename & ".ws"
            If Trim(txtScriptDesc.Text) = "" Then txtScriptDesc.Text = IO.Path.GetFileName(ScriptFilename)
            ScriptString = ScriptStringFromWizard()
            'OutFile = FreeFile()
            'FileOpen(OutFile, ScriptFilename, OpenMode.Output)
            'PrintLine(OutFile, ScriptString)
            'FileClose(OutFile)
            lOutFileStream = New IO.StreamWriter(ScriptFilename, False)
            lOutFileStream.WriteLine(ScriptString)
            lOutFileStream.Close()
            SaveSetting("ATCTimeseriesImport", "Scripts", dlgOpenFileOpen.FileName, txtScriptDesc.Text)
        End If
        'End If

        'ctlUserPrompt1.Visible = False

        'modFileIO.OpenFile UnitDescFile, NameDescFile, "Output", _
        ''    "Data Description Files (*.ddf)|*.ddf", "Save", dlgOpenFile
        'If UnitDescFile > 0 Then
        '  WriteDescFile UnitDescFile
        '  txtScriptFile.Text = NameDescFile
        '  Close UnitDescFile
        'Else
        '  MsgBox "ERROR: CANNOT OPEN FILE:" & vbCrLf _
        ''         & Chr(34) & NameDescFile & Chr(34)
        'End If
    End Sub

    'Select valid column names from list of field names in active memory
    Private Sub SetValidInputColNames()
        'If Not fraColSample.Visible Then
        '  tabSample.SelectedItem = tabSample.Tabs.Item(2)
        '  tabSample_Click
        'End If
        'Dim c As Integer
        'Dim t As String
        'For c = 0 To agdSample.Source.Columns - 1
        '    t = agdSample.Source.CellValue(0, c)
        '    If delimQ Then
        '        If t <> CStr(c + 1) Then t = t & " (" & c + 1 & ")"
        '    End If
        '    agdDataMapping.addValue(t)
        'Next c
    End Sub

    'Returns position of next character from chars in str
    'Returns len(str) + 1 if none were found
    'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Function FirstCharPos(ByRef start As Integer, ByRef str_Renamed As String, ByRef chars As String) As Integer
        Dim CharPos, retval, curval, LenChars As Integer
        retval = Len(str_Renamed) + 1
        LenChars = Len(chars)
        For CharPos = 1 To LenChars
            curval = InStr(start, str_Renamed, Mid(chars, CharPos, 1))
            If curval > 0 And curval < retval Then retval = curval
        Next CharPos
        FirstCharPos = retval
    End Function

    ' Function  - - - - - - - - - - - - - - - - - - - - - - - - -
    ' Name:     ParseInputLine (3 arguments)
    ' Purpose:  Returns number of columns parsed from buffer into array
    '           Populates parsed array from element 1 to index returned
    '
    Private Function ParseInputLine(ByVal InBuf As String, ByRef parsed() As String) As Integer

        Dim parseCol As Integer
        Dim fromCol As Integer
        Dim toCol As Integer
        parseCol = 0
        fromCol = 1
        If delimQ Then 'parse delimited text
            While fromCol <= Len(InBuf) And parseCol < UBound(parsed)
                toCol = FirstCharPos(fromCol, InBuf, delim)
                'If chkCollapseDelim.Value = 1 Then 'treat multiple contiguous delimiters as one
                ' While toCol = fromCol And toCol < Len(inBuf)
                '  toCol = fromCol + 1
                ' toCol = InStr(fromCol, inBuf, delim)
                ' Wend
                'End If
                If toCol < fromCol Then toCol = Len(InBuf) + 1
                parseCol = parseCol + 1
                parsed(parseCol) = Mid(InBuf, fromCol, toCol - fromCol)
                fromCol = toCol + 1
            End While
        Else 'fixed columns
            While parseCol < nFixedCols
                parseCol = parseCol + 1
                toCol = FixedColRight(parseCol)
                If toCol > 0 Then
                    fromCol = FixedColLeft(parseCol)
                    parsed(parseCol) = Mid(InBuf, fromCol, toCol - fromCol + 1)
                Else
                    parsed(parseCol) = ""
                End If
            End While
        End If
        If parseCol > UBound(parsed) Then parseCol = UBound(parsed)
        ParseInputLine = parseCol
    End Function

    ' Subroutine ===============================================
    ' Name:      PopulateGridTest
    ' Purpose:   Populates the Test-Mapping grid with data
    '            fields selected on the Data-Mapping tab
    '
    Private Sub PopulateGridTest()
        Dim lines As Integer
        Dim linecnt As Integer
        Dim cbuff As String
        Dim parsed(conMaxNumColumns) As String
        Dim pcols As Integer
        Dim cout As Integer
        Dim cin As Integer ' column out (agdTestMapping), in (agdDataMapping)
        Dim icl As Integer
        Dim ics As String ' input column long, string

        'Call objParser.AssignProperties( _
        ''     cboFileType.text, _
        ''     delim, _
        ''     txtQuoteChar.text)

        '  Seek UnitDataFile, 1
        '  If IsNumeric(txtLinesToSkip.Text) Then
        '    linecnt = 0
        '    lines = CLng(txtLinesToSkip.Text)
        '    While linecnt < lines And Not ScriptEndOfData
        '      Line Input #UnitDataFile, cbuff
        '      linecnt = linecnt + 1
        '    Wend
        '  End If
        '  With agdTestMapping
        '    .Clear
        '    .Cols = agdDataMapping.MaxOccupiedRow
        '    .rows = 1
        '    lines = 50  'txtSample.Height / TextHeight("X")
        '    linecnt = 0
        '    For cout = 0 To .Cols - 1
        '      .ColTitle(cout) = agdDataMapping.TextMatrix(cout + 1, ColMappingCol) '& _
        ''                        '", " & agdDataMapping.TextMatrix(cout + 1, ColMappingLookup)
        '    Next cout
        '    'If chkHeaderRecord.Value = 1 Then Line Input #UnitDataFile, cbuff
        '    While Not ScriptEndOfData And linecnt < lines
        '      Line Input #UnitDataFile, cbuff
        '      pcols = ParseInputLine(cbuff, parsed)
        '      linecnt = linecnt + 1
        '      For cout = 0 To .Cols - 1
        '        ics = agdDataMapping.TextMatrix(cout + 1, ColMappingCol)
        '        If Len(ics) > 0 Then
        '          If delimQ Then
        '            If IsNumeric(ics) Then
        '              icl = CLng(ics)
        '              If icl <= pcols Then .TextMatrix(linecnt, cout) = parsed(icl)
        '            End If
        '          Else
        '            .TextMatrix(linecnt, cout) = parsed(cout + 1)
        '          End If
        '        Else
        '          .TextMatrix(linecnt, cout) = _
        ''           agdDataMapping.TextMatrix(cout + 1, ColMappingConstant)
        '        End If
        '      Next cout
        '    Wend
        '    Seek UnitDataFile, 1
        '  End With
    End Sub

    ' Subroutine ===============================================
    ' Name:      PopulateGridSample
    ' Purpose:   For delimited text files, populates the sample
    '            grid with conNumSampleLines of sample lines of
    '            data.
    '
    Private Sub PopulateGridSample()
        Dim lines As Integer
        Dim linecnt As Integer
        Dim cbuff As String
        Dim parsed(conMaxNumColumns) As String
        Dim pcols As Integer
        Dim c As Integer
        If UnitDataFileReader IsNot Nothing Then
            'Seek(UnitDataFile, 1)
            UnitDataFileReader.BaseStream.Seek(0, IO.SeekOrigin.Begin)
            '  If IsNumeric(txtLinesToSkip.Text) Then
            '    linecnt = 0
            '    lines = CLng(txtLinesToSkip.Text)
            '    While linecnt < lines And Not ScriptEndOfData
            '      Line Input #UnitDataFile, cbuff
            '      linecnt = linecnt + 1
            '    Wend
            '  End If
            Dim lNewSource As New atcControls.atcGridSource
            With lNewSource
                '.Clear() 'TODO: need replacement?
                lines = conNumSampleLines
                linecnt = 0
                '    If chkHeaderRecord.Value = 1 Then
                '      Line Input #UnitDataFile, cbuff
                '      pcols = ParseInputLine(cbuff, parsed)
                '      .Cols = pcols
                '      For c = 1 To pcols
                '        .ColTitle(c - 1) = parsed(c)
                '        .ColEditable(c - 1) = True
                '      Next c
                '    End If
                While Not UnitDataFileReader.EndOfStream And linecnt < lines
                    'cbuff = LineInput(UnitDataFile)
                    cbuff = UnitDataFileReader.ReadLine()
                    pcols = ParseInputLine(cbuff, parsed)
                    linecnt = linecnt + 1
                    For c = 1 To pcols
                        .CellValue(linecnt, c - 1) = parsed(c)
                    Next c
                End While
                'If chkHeaderRecord.Value <> 1 Then 'number columns
                For c = 0 To .Columns - 1
                    If delimQ Then .CellValue(0, c) = c + 1 Else .CellValue(0, c) = FixedColLeft(c + 1) & "-" & FixedColRight(c + 1)
                Next c
                'End If
                'Seek(UnitDataFile, 1)
                UnitDataFileReader.BaseStream.Seek(0, IO.SeekOrigin.Begin)
            End With
            agdSample.Initialize(lNewSource)
        End If
    End Sub

    ' Subroutine ===============================================
    ' Name:      PopulateTxtSample
    ' Purpose:   For fixed format files, populates the Sample
    '            text box with sample data
    Private Sub PopulateTxtSample()
        Dim linecnt As Integer
        Dim cbuff As String
        Dim nChars As Integer

        InputLineLen = 0
        If optLineEndCRLF.Checked = True Then
            InputEOL = vbCr
        ElseIf optLineEndLF.Checked = True Then
            InputEOL = vbLf
        ElseIf optLineEndASCII.Checked = True Then
            If IsNumeric(txtLineEndChar.Text) Then InputEOL = Chr(CInt(txtLineEndChar.Text))
        ElseIf optLineEndLength.Checked = True Then
            If IsNumeric(txtLineLen.Text) Then InputLineLen = CInt(Trim(txtLineLen.Text))
        End If
        LenInputEOL = Len(InputEOL)

        linecnt = 0
        NextLineStart = 1
        If LenDataFile > 0 Then

            'On Error GoTo exitsub
            'Debug.Print "PopulateTxtSample " & Time
            'Skip lines vertical scroll has scrolled past

            If chkSkipHeader.CheckState = System.Windows.Forms.CheckState.Checked Then
                If optHeaderStartsWith.Checked = True And Len(txtHeaderStart.Text) > 0 Then 'skip header lines starting with string
                    CurrentLine = txtHeaderStart.Text
                    While Not ScriptEndOfData() And VB.Left(CurrentLine, Len(txtHeaderStart.Text)) = txtHeaderStart.Text
                        ScriptNextLine()
                    End While
                ElseIf optHeaderLines.Checked = True And IsNumeric(txtHeaderLines.Text) Then  'Skip number of header lines
                    linecnt = CInt(txtHeaderLines.Text)
                    While Not ScriptEndOfData() And linecnt > 0
                        ScriptNextLine()
                        linecnt = linecnt - 1
                    End While
                End If
            End If

            While Not ScriptEndOfData() And linecnt < VScrollSample.Value
                ScriptNextLine()
                linecnt = linecnt + 1
            End While

            'Read portion of lines right of horizontal scroll position
            nChars = _txtSample_0.Width / CharWidth - 1
            linecnt = 0
            While Not ScriptEndOfData() And linecnt < txtSample.Count
                ScriptNextLine()
                txtSample(linecnt).Text = Mid(CurrentLine, HScrollSample.Value, nChars)
                txtSample(linecnt).SelectionStart = txtRuler1.SelectionStart
                txtSample(linecnt).SelectionLength = txtRuler1.SelectionLength
                linecnt = linecnt + 1
            End While
            EnableFilePropertiesFields()
        End If
        While linecnt < txtSample.Count
            txtSample(linecnt).Text = ""
            linecnt = linecnt + 1
        End While
exitsub:
        Exit Sub

    End Sub


    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     optDelimiter_Click (1 argument)
    ' Purpose:  Responds to press of a "Delimiter-option" button
    '
    'UPGRADE_WARNING: Event optDelimiter.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optDelimiter_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optDelimiterNone.CheckedChanged, optDelimiterSpace.CheckedChanged, optDelimiterTab.CheckedChanged, optDelimiterChar.CheckedChanged
        If eventSender.Checked Then
            Select Case eventSender.Name
                Case "optDelimiterNone" : delimQ = False
                Case "optDelimiterSpace" : delimQ = True : delim = " "
                Case "optDelimiterTab" : delimQ = True : delim = Chr(9)
                Case "optDelimiterChar" : delimQ = True : delim = txtDelimiter.Text
            End Select

            If delimQ Then
                fraTextSample.Visible = False
                fraColSample.Visible = True
                MappingSource.CellValue(0, ColMappingCol) = "Input Column"
            Else
                fraColSample.Visible = False
                fraTextSample.Visible = True
                'txtSample(0).Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(txtRuler2.Top) + VB6.PixelsToTwipsY(txtRuler2.Height))
                MappingSource.CellValue(0, ColMappingCol) = "Beg-End Column"
            End If
            PopulateSample()
        End If
    End Sub

    Private Sub PopulateSample()
        'If fraColSample.Visible Then PopulateGridSample()
        'If fraTextSample.Visible Then PopulateTxtSample()
        PopulateGridSample()
        PopulateTxtSample()
    End Sub

    Private Sub optLineEnd_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optLineEndCRLF.CheckedChanged, optLineEndLF.CheckedChanged, optLineEndASCII.CheckedChanged, optLineEndLength.CheckedChanged
        If eventSender.Checked Then
            PopulateSample()
        End If
    End Sub

    Private Sub txtScriptFile_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtScriptFile.Enter

        '-----------------------------------
        'highlight upon selection so that the
        'next user input key begins a new entry,
        'clearing the entire previous entry.
        '-----------------------------------
        'In the mytextbox gotfocus event:
        txtScriptFile.SelectionStart = 0
        txtScriptFile.SelectionLength = Len(txtDataFile.Text)

    End Sub

    'UPGRADE_WARNING: Event txtHeaderLines.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtHeaderLines_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtHeaderLines.TextChanged
        optHeaderLines.Checked = True
        PopulateSample()
    End Sub

    'UPGRADE_WARNING: Event txtHeaderStart.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtHeaderStart_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtHeaderStart.TextChanged
        optHeaderStartsWith.Checked = True
        PopulateSample()
    End Sub

    'UPGRADE_WARNING: Event txtLineEndChar.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtLineEndChar_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtLineEndChar.TextChanged
        optLineEndASCII.Checked = True
        PopulateSample()
    End Sub

    'UPGRADE_WARNING: Event txtLineLen.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtLineLen_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtLineLen.TextChanged
        optLineEndLength.Checked = True
        PopulateSample()
    End Sub

    Private Sub txtRuler1_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles txtRuler1.MouseDown
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button = 1 Then txtSampleAnyChange(-1)
    End Sub

    Private Sub txtRuler1_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles txtRuler1.MouseMove
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button = 1 Then txtSampleAnyChange(-1)
    End Sub

    Private Sub txtRuler1_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles txtRuler1.MouseUp
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button = 1 Then txtSampleAnyChange(-1)
    End Sub

    Private Sub txtRuler2_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles txtRuler2.MouseDown
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button = 1 Then txtSampleAnyChange(-2)
    End Sub

    Private Sub txtRuler2_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles txtRuler2.MouseMove
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button = 1 Then txtSampleAnyChange(-2)
    End Sub

    Private Sub txtRuler2_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles txtRuler2.MouseUp
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button = 1 Then txtSampleAnyChange(-2)
    End Sub

    Private Sub txtSample_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles _txtSample_0.MouseDown
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        Dim index As Short = txtSample.IndexOf(eventSender)
        If Button = 1 Then txtSampleAnyChange(index)
    End Sub

    Private Sub txtSample_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles _txtSample_0.MouseMove
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        Dim index As Short = txtSample.IndexOf(eventSender)
        If Button = 1 Then txtSampleAnyChange(index)
    End Sub

    Private Sub txtSample_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles _txtSample_0.MouseUp
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        Dim index As Short = txtSample.IndexOf(eventSender)
        If Button = 1 Then txtSampleAnyChange(index)
    End Sub

    Private Sub txtSampleAnyChange(ByRef whichChanged As Short)
        Dim SelLength, SelStart, sam As Integer
        Select Case whichChanged
            Case -1 : SelStart = txtRuler1.SelectionStart : SelLength = txtRuler1.SelectionLength
            Case -2 : SelStart = txtRuler2.SelectionStart : SelLength = txtRuler2.SelectionLength
            Case Else : SelStart = txtSample(whichChanged).SelectionStart
                SelLength = txtSample(whichChanged).SelectionLength
        End Select

        If txtRuler1.SelectionStart <> SelStart Then txtRuler1.SelectionStart = SelStart
        If txtRuler2.SelectionStart <> SelStart Then txtRuler2.SelectionStart = SelStart

        If txtRuler1.SelectionLength <> SelLength Then txtRuler1.SelectionLength = SelLength
        If txtRuler2.SelectionLength <> SelLength Then txtRuler2.SelectionLength = SelLength

        For sam = 0 To txtSample.Count - 1
            If txtSample(sam).SelectionStart <> SelStart Then txtSample(sam).SelectionStart = SelStart
            If txtSample(sam).SelectionLength <> SelLength Then txtSample(sam).SelectionLength = SelLength
        Next sam

        SelStart = SelStart + HScrollSample.Value
        If SelLength > 0 And Not SettingSelFromGrid Then
            If SelLength < 2 Then
                MappingSource.CellValue(MappingSource.Rows, ColMappingCol) = SelStart
            Else
                MappingSource.CellValue(MappingSource.Rows, ColMappingCol) = SelStart & "-" & SelStart + SelLength - 1
            End If
            '    SetFixedWidthsFromDataMapping
        End If
    End Sub

    Private Sub txtDataFile_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDataFile.Enter
        txtDataFile.SelectionStart = 0
        txtDataFile.SelectionLength = Len(txtDataFile.Text)
    End Sub

    'UPGRADE_WARNING: Event txtDataFile.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtDataFile_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDataFile.TextChanged

        ' If a filename was not entered, exit the subroutine:
        ' If a filename not changed, then exit the subroutine:
        '
        If txtDataFile.Text = "txtDataFile" Then
            'UnitDataFile = 0
            If UnitDataFileReader IsNot Nothing Then
                UnitDataFileReader.Close()
                UnitDataFileReader = Nothing
            End If
            PopulateSample()
        ElseIf txtDataFile.Text <> NameDataFile Or UnitDataFileReader Is Nothing Then
            OpenDataFile()
        End If

    End Sub

    'Start with text in txtDataFile, try to open the file and set NameDataFile
    Private Sub OpenDataFile()
        Dim n As Integer ', msg As String
        ' Try to find a \ in the name. If so then assume the user
        ' entered a full pathname. If not, then append the cur directory
        ' to get a fullpathname.
        '
        n = InStr(1, txtDataFile.Text, "\", CompareMethod.Text)
        If n > 0 Then
            NameDataFile = Trim(txtDataFile.Text)
        Else
            NameDataFile = CurDir() & "\" & Trim(txtDataFile.Text)
        End If
        txtDataFile.Text = NameDataFile
        '
        ' Open the input file.
        '
        '  If UnitDataFile > 0 Then Close UnitDataFile
        '  UnitDataFile = FreeFile
        '  On Error GoTo FileNotOpened
        '  Open NameDataFile For Input As UnitDataFile

        OpenUnitDataFile()
        '  Msg = ScriptOpenDataFile(NameDataFile)
        '  If Msg <> "OK" Then
        '    MsgBox Msg, vbOKOnly, "Data Import"
        '  Else
        '    PopulateSample
        '  End If
        '  Exit Sub
        '
        'FileNotOpened:
        '  UnitDataFile = 0
    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     cmdBrowseData_Click
    ' Purpose:  Responds to press of the "Browse" button
    '           to obtain the input file name.
    '
    Private Sub cmdBrowseData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrowseData.Click
        Dim m_FileName As String

        If UnitDataFileReader IsNot Nothing Then
            UnitDataFileReader.Close()
            UnitDataFileReader = Nothing
        End If

        'UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        dlgOpenFileOpen.Filter = "All Files (*.*)|*.*"
        dlgOpenFileSave.Filter = "All Files (*.*)|*.*"
        dlgOpenFileOpen.DefaultExt = ""
        dlgOpenFileSave.DefaultExt = ""
        dlgOpenFileOpen.Title = "Open Data File"
        dlgOpenFileSave.Title = "Open Data File"
        dlgOpenFileOpen.ShowDialog()
        dlgOpenFileSave.FileName = dlgOpenFileOpen.FileName
        NameDataFile = dlgOpenFileOpen.FileName
        OpenUnitDataFile()
    End Sub
    Private Sub OpenUnitDataFile()
        Dim msg As String
        msg = ScriptOpenDataFile(NameDataFile)
        If msg <> "OK" Then
            MsgBox(msg, MsgBoxStyle.OkOnly, "Data Import")
        Else
            'FreeFile gets the first free file number 1-511.
            'UnitDataFile = FreeFile()
            'FileOpen(UnitDataFile, NameDataFile, OpenMode.Input)
            UnitDataFileReader = New IO.StreamReader(NameDataFile)
            txtDataFile.Text = NameDataFile
            PopulateSample()
        End If
    End Sub

    ' Function  - - - - - - - - - - - - - - - - - - - - - - - - -
    ' Name:     BoxWidth (1 argument)
    ' Purpose:  Returns an optimum width for user-prompt box
    '           based on the length of the input text string
    '
    Private Function BoxWidth(ByRef TextStr As String) As Short
        BoxWidth = Len(Trim(TextStr)) * 100
    End Function

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     txtScriptFile_Click
    ' Purpose:  Responds to click in "txtScriptFile" text box
    '
    Private Sub txtScriptFile_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtScriptFile.Click
        '
        ' Get default file name and check for its existance. If there
        ' ask user if the want to use it, otherwise prompt for a different
        ' file.
        '
        '  NameDescFile = Mid(Trim(NameDataFile), 1, Len(NameDataFile) - 4) & ".ddf"
        '  FoundDDF = modFileIO.DoesFileExist(NameDescFile)
        '
        '  If FoundDDF = False Then
        '    PromptForFileName (2)
        '    Exit Sub
        '  End If
        '
        ' An existing ddf exists ask user "do you want to delete it ?"
        '
        '  Select Case MsgBox("Data-descriptor file exists:" & vbCrLf _
        ''              & NameDescFile & vbCrLf & "delete it?", vbYesNoCancel)
        '    Case vbYes
        '       modFileIO.DeleteFile NameDescFile
        '    txtScriptFile.Text = Dir(NameDescFile)
        '    modFileIO.OpenFile UnitDescFile, NameDescFile, "Input"
        '    If UnitDescFile > 0 Then
        '      txtScriptFile.Text = NameDescFile
        '      ReadDescFile
        '      txtLinesToSkip.SetFocus
        '    Else
        '      MsgBox "ERROR: CANNOT OPEN FILE:" & vbCrLf _
        ''             & Chr(34) & NameDescFile & Chr(34)
        '      txtScriptFile.SetFocus
        '    End If

        '    Case vbNo
        '    Case vbCancel
        '      txtScriptFile.Text = ""
        '      txtLinesToSkip.SetFocus
        '      Exit Sub
        '    End Select
        '
        '    txtScriptFile.Text = ""
        '    PromptForFileName (2)
        '    txtScriptFile.SetFocus
        '    Exit Sub
    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     txtScriptFile_KeyPress (1 argument)
    ' Purpose:  Responds to press of any key in txtScriptFile text box
    '
    Private Sub txtScriptFile_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtScriptFile.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'ctlUserPrompt1.PromptLine2 "  TAB TO END"
        'ctlUserPrompt1.Line2Red
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     txtScriptFile_LostFocus
    ' Purpose:  Responds to loss of focus in txtScriptFile text box
    '
    Private Sub txtScriptFile_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtScriptFile.Leave
        Dim m_FilePath As String
        Dim m_Prompt As String
        Dim m_LenPro As Short
        Dim n As Integer

        ' If a filename was not entered, reset found flags and
        ' exit the subroutine:
        '
        If Len(Trim(txtScriptFile.Text)) = 0 Then
            '    FoundData = False
            FoundDDF = False
            '    txtLinesToSkip.SetFocus
            '    FoundPas = False
            Exit Sub
        End If

        ' Something was entered in the data-description file text box:
        '
        ' Try to find a \ in the name. If so then assume the user
        ' entered a full pathname. If not, then append the cur directory
        ' to get a fullpathname.
        '
        n = InStr(1, txtScriptFile.Text, "\", CompareMethod.Text)
        If n > 0 Then
            NameDescFile = Trim(txtScriptFile.Text)
        Else
            m_FilePath = CurDir()
            NameDescFile = m_FilePath & "\" & Trim(txtScriptFile.Text)
        End If

        '  FoundDDF = DoesFileExist(NameDescFile)
        '
        '  If FoundDDF = True Then
        '    If UnitDescFile > 0 Then Close UnitDescFile
        '    modFileIO.OpenFile UnitDescFile, NameDescFile, "Input"
        '    If UnitDescFile > 0 Then
        '        txtScriptFile.Text = NameDescFile
        '        ReadDescFile
        '    End If
        '
        '    PromptForDataMapping
        '  Else
        '    MsgBox "ERROR: FILE NOT FOUND:" & vbCrLf _
        ''           & Chr(34) & txtScriptFile.Text & Chr(34)
        '    txtScriptFile.Text = ""
        '    txtScriptFile.SetFocus
        '
        '  End If
    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     txtDelimiter_Change
    ' Purpose:  Responds to any change in the "txtDelimiter" text box
    '
    Private Sub txtDelimiter_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDelimiter.TextChanged
        optDelimiterChar.Checked = True
        delim = txtDelimiter.Text
        If fraColSample.Visible Then PopulateGridSample()
    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     txtQuoteChar_KeyPress
    ' Purpose:  Responds to a key pressed in "txtQuoteChar" text box
    '
    'Private Sub txtQuoteChar_KeyPress(KeyAscii As Integer)
    '  If KeyAscii = Asc("n") Or KeyAscii = Asc("N") Then
    '    txtQuoteChar.Text = "none"
    '  ElseIf KeyAscii > 32 And KeyAscii < 127 Then
    '    txtQuoteChar.Text = Chr(KeyAscii)
    '  Else
    '    txtQuoteChar.Text = "none"
    '  End If
    '  KeyAscii = 0
    '  If fraColSample.Visible Then PopulateGridSample
    'End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     txtNullChar_KeyPress
    ' Purpose:  Responds to a key pressed in "txtNullChar" text box
    '
    'Private Sub txtNullChar_KeyPress(KeyAscii As Integer)
    '  If KeyAscii = Asc("n") Or KeyAscii = Asc("N") Then
    '    txtNullChar.Text = "none"
    '  ElseIf KeyAscii > 32 And KeyAscii < 127 Then
    '    txtNullChar.Text = Chr(KeyAscii)
    '  Else
    '    txtNullChar.Text = "none"
    '  End If
    '  KeyAscii = 0
    '  If fraColSample.Visible Then PopulateGridSample
    'End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     txtLinesToSkip_Change
    ' Purpose:  Responds to any change in the "txtLinesToSkip" text box
    '
    Private Sub txtLinesToSkip_Change()
        If fraColSample.Visible Then PopulateGridSample()
    End Sub




    ' Subroutine ===============================================
    ' Name:      AskForLookupFilename
    ' Purpose:   Queries user for name of lookup file
    '
    Private Sub AskForLookupFilename()
        'UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        dlgOpenFileOpen.Filter = "All Files (*.*)|*.*"
        dlgOpenFileSave.Filter = "All Files (*.*)|*.*"
        dlgOpenFileOpen.DefaultExt = ""
        dlgOpenFileSave.DefaultExt = ""
        dlgOpenFileOpen.Title = "Open lookup file"
        dlgOpenFileSave.Title = "Open lookup file"
        dlgOpenFileOpen.ShowDialog()
        dlgOpenFileSave.FileName = dlgOpenFileOpen.FileName
        With MappingSource
            .CellValue(.Rows, 0) = dlgOpenFileOpen.FileName
            If Len(dlgOpenFileOpen.FileName) > 0 Then .CellValue(.Rows, 4) = ""
        End With
    End Sub

    ' Subroutine ===============================================
    ' Name:      SetFixedWidthsFromDataMapping
    ' Purpose:   Sets widths of fields for fixed-format files
    '
    'Public Sub SetFixedWidthsFromDataMapping()
    '  Dim r As Long
    '  Dim colspec As String
    '  Dim dashpos As Long
    ' 'Dim existing As Long
    '
    '  nFixedCols = 0
    '  For r = 1 To agdDataMapping.MaxOccupiedRow
    '    colspec = agdDataMapping.TextMatrix(r, ColMappingCol)
    '    nFixedCols = nFixedCols + 1
    '    If Len(colspec) > 0 Then
    '      dashpos = InStr(colspec, "-")
    '      If dashpos > 0 Then   'range (left-right)
    '        FixedColLeft(nFixedCols) = Left(colspec, dashpos - 1)
    '        FixedColRight(nFixedCols) = Mid(colspec, dashpos + 1)
    '      Else
    '        dashpos = InStr(colspec, "+")
    '        If dashpos > 0 Then 'range (left+length)
    '          FixedColLeft(nFixedCols) = Left(colspec, dashpos - 1)
    '          FixedColRight(nFixedCols) = Mid(colspec, dashpos + 1) + _
    ''          FixedColLeft(nFixedCols)
    '        Else                'Single number, assume single character column
    '          FixedColLeft(nFixedCols) = colspec
    '          FixedColRight(nFixedCols) = colspec
    '        End If
    '      End If
    '    Else
    '      FixedColLeft(nFixedCols) = 0
    '      FixedColRight(nFixedCols) = 0
    '    End If
    '  Next r
    '  'Debug.Print "SetFixedWidthsFromDataMapping: nFixedCols=" & nFixedCols
    'End Sub

    ' Subroutine ===============================================
    ' Name:      WriteDescFile (1 argument)
    ' Purpose:   Saves the mapping information to a data-definition/
    '            descriptor file
    ' Modified:
    '
    Private Sub WriteDescFile(ByRef UnitOutfile As Short)
        Dim r, index As Integer
        Dim view As String
        Dim Field As String
        Dim tmp As String

        If UnitOutfile < 0 Then
            MsgBox("Cannot write to output file.", MsgBoxStyle.OkOnly, "Data Import")
        Else
            PrintLine(UnitOutfile, "#National Water Information System")
        End If
    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     objEditEngine_JobStatus
    ' Purpose:  Handles updating jobstatus and interupts.
    '
    ' Notes:
    ' Whenever the JobStatus event is raised in the objEditEngine,
    ' this event procedure displays the jobs current status. The
    ' DoEvents statement allows the GUI to repaint, and
    ' also gives the user the opportunity to click the
    ' StopJob (StopJob) button.
    '
    'Private Sub objEditEngine_JobStatus( _
    ''            ByVal NumRecProcessed As Long, _
    ''            ByVal NumErrWarning As Long, _
    ''            ByVal NumErrFatal As Long, _
    ''            StopJob As Boolean)
    '
    ' Display status in GUI ...
    '
    'lblWarn.caption = "Warn: " & CStr(NumErrWarning)
    'lblFatal.caption = "Fatal: " & CStr(NumErrFatal)
    'lblRead.caption = "Read: " & CStr(NumRecProcessed + CLng(txtLinesToSkip.text))
    '
    ' StopJob the job if the user has requested the process to StopJob.
    ' StopJob it by passing StopJob = true back to the objeditengine.
    '
    'If m_StopJob Then
    '  StopJob = True
    'End If
    '
    ' Allow the user to access to GUI to StopJob the processing.
    '
    'DoEvents
    'End Sub

    ' Subroutine ===============================================
    ' Name:      UpdateDataMap
    ' Purpose:   Updates the data-mapping grid (?)
    '
    Sub UpdateDataMap()
        Dim i As Short
        '
        ' Load views and columns if not already loaded.
        '
        '  If g_colView.Count = 0 Then
        '    LoadViewsAndColumns
        '  End If
        '
        ' Clear out any existing data map in the engine.
        '
        'objEditEngine.ClearDataMap
        '
        ' Pass in the current data map from the GUI.
        '
        ' Note: in real batch processing mode the data map information
        ' could be read in directly from a ddf file.
        '

        For i = 1 To MappingSource.Rows
            'Call objEditEngine.AddDataMapEntry( _
            ''     agdDataMapping.TextMatrix(i, 5), _
            ''     agdDataMapping.TextMatrix(i, 2), _
            ''     agdDataMapping.TextMatrix(i, 3), _
            ''     agdDataMapping.TextMatrix(i, 4), _
            ''     agdDataMapping.TextMatrix(i, 0), _
            ''     g_colVWCol.item(Trim(agdDataMapping.TextMatrix(i, 5)) & _
            ''      "_" & Trim(agdDataMapping.TextMatrix(i, 2))).EditCriteria, _
            ''     g_colVWCol.item(Trim(agdDataMapping.TextMatrix(i, 5)) & _
            ''      "_" & Trim(agdDataMapping.TextMatrix(i, 2))))
        Next
    End Sub

    'UPGRADE_NOTE: VScrollSample.Change was changed from an event to a procedure. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="4E2DC008-5EDA-4547-8317-C9316952674F"'
    'UPGRADE_WARNING: VScrollBar event VScrollSample.Change has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
    Private Sub VScrollSample_Change(ByVal newScrollValue As Integer)
        PopulateTxtSample()
    End Sub

    'UPGRADE_NOTE: VScrollSample.Scroll was changed from an event to a procedure. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="4E2DC008-5EDA-4547-8317-C9316952674F"'
    Private Sub VScrollSample_Scroll_Renamed(ByVal newScrollValue As Integer)
        VScrollSample_Change(0)
    End Sub
    Private Sub HScrollSample_Scroll(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ScrollEventArgs) Handles HScrollSample.Scroll
        Select Case eventArgs.Type
            Case System.Windows.Forms.ScrollEventType.ThumbTrack
                HScrollSample_Scroll_Renamed(eventArgs.NewValue)
            Case System.Windows.Forms.ScrollEventType.EndScroll
                HScrollSample_Change(eventArgs.NewValue)
        End Select
    End Sub
    Private Sub VScrollSample_Scroll(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ScrollEventArgs) Handles VScrollSample.Scroll
        Select Case eventArgs.Type
            Case System.Windows.Forms.ScrollEventType.ThumbTrack
                VScrollSample_Scroll_Renamed(eventArgs.NewValue)
            Case System.Windows.Forms.ScrollEventType.EndScroll
                VScrollSample_Change(eventArgs.NewValue)
        End Select
    End Sub
End Class