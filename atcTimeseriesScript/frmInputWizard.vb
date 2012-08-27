Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports System.Drawing
Imports MapWinUtility
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

    'Private FoundDDF As Boolean
    Private WriteHeader As Boolean

    Private UnitDescFile As Short
    Private NameDescFile As String

    Private CharWidth As Single = 10.0
    Private SettingSelFromGrid As Boolean

    Private nFixedCols As Integer
    Private hSashDragging As Boolean
    Private SiteDataMapped As Boolean
    Private mbMoving As Boolean

    Private NameDataFile As String

    Private pTserFile As atcData.atcTimeseriesSource
    Private Script As clsATCscriptExpression

    Private RequiredFields() As String = {"", "Value", "Year", "Month", "Day", "Hour", "Minute", "Scenario", "Location", "Constituent", "Description", "Repeating", "Repeats"} 'Array of names
    Private nRequiredFields As Integer = RequiredFields.Length

    Private pDataMappingCol As Integer = -1
    Private pDataMappingRow As Integer = -1

    Public WriteOnly Property TserFile() As atcData.atcTimeseriesSource
        Set(ByVal Value As atcData.atcTimeseriesSource)
            pTserFile = Value
        End Set
    End Property

    Private Sub chkSkipHeader_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkSkipHeader.CheckStateChanged
        PopulateSample()
    End Sub

    Private Sub cmdReadData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReadData.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Script = New clsATCscriptExpression

        Logger.Dbg("(OPEN Reading " & NameDataFile & ")")
        Logger.Dbg("(BUTTOFF CANCEL)")
        Logger.Dbg("(BUTTOFF PAUSE)")

        Script.ParseExpression(ScriptStringFromWizard)
        pTserFile.DataSets.Clear()
        MsgBox(ScriptRun(Script, NameDataFile, pTserFile) & vbCrLf & "Dataset Count = " & pTserFile.DataSets.Count, vbOKOnly, "Ran Import Data Script")

        Logger.Dbg("(CLOSE)")
        Logger.Dbg("(BUTTON CANCEL)")
        Logger.Dbg("(BUTTON PAUSE)")

        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Close()
    End Sub

    Private Sub frmInputWizard_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Logger.Dbg() 'Avoid unhelpful error message when closing form instead of reading data
    End Sub

    'Displays the Input Wizard form and initializes
    'fields and objects used in the application.
    Private Sub frmInputWizard_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        If MappingSource.Rows < 2 Then InitDataMapping()

        CharWidth = 10 'TODO: Me.TextWidth("X")

        ' initialize Input File Property defaults.
        DisableFilePropertiesFields()

        ' Left justify major display areas on form
        'fraTextSample.Left = tabTop.Left
        'fraColSample.Left = tabTop.Left
        SizeControls() 'fraSash.Top)
    End Sub

    Private Sub frmInputWizard_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        SizeControls() 'fraSash.Top)
    End Sub

    'Resize all controls on the form
    Sub SizeControls() 'ByRef SashTop As Single)
        Dim SampleHeight As Integer
        Dim EachSampleHeight As Integer
        Dim BottomHeight As Integer

        If txtSample.Count = 0 Then txtSample.Add(_txtSample_0)
        Dim lNewSampleTextbox As Windows.Forms.TextBox = txtSample(0)

        BottomHeight = Me.Height - fraColSample.Top ' SashTop - fraSash.Height

        'fraTextSample.Height = VB6.TwipsToPixelsY(BottomHeight)
        SampleHeight = BottomHeight - txtRuler1.Height - txtRuler2.Height - HScrollSample.Height
        EachSampleHeight = lNewSampleTextbox.Height * 0.95

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

        SetRulers()

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
            RulerString2 &= RulerCount
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

    Private Sub optHeader_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optHeaderNone.CheckedChanged, optHeaderStartsWith.CheckedChanged, optHeaderLines.CheckedChanged
        If sender.Checked Then PopulateSample()
    End Sub

    Private Sub agdDataMapping_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdDataMapping.CellEdited
        SetSelFromGrid(aRow)
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

        If InRowColChange Then Exit Sub
        InRowColChange = True

        With MappingSource
            Dim newrow As Integer
            Dim newcol As Integer

            newrow = aRow
            newcol = aColumn

            Dim lUniqueValues As New ArrayList
            If newcol = ColMappingAttr Then
                lUniqueValues.Add("yes")
                lUniqueValues.Add("no")
            End If
            agdDataMapping.ValidValues = lUniqueValues

            pDataMappingRow = newrow
            pDataMappingCol = newcol

            SetSelFromGrid(newrow)

            .CellEditable(newrow, newcol) = (aRow > nRequiredFields OrElse aRow > 0 AndAlso aColumn <> ColMappingName)
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

            If Not FixedColumns Then 'Select column in agdSample
                SelStart = FixedColLeft(row)
                For lColumn As Integer = 1 To MappingSource.Columns
                    Dim lSelect As Boolean = (lColumn = SelStart)
                    For lRow As Integer = 0 To MappingSource.Rows - 1
                        agdSample.Source.CellSelected(lRow, lColumn - 1) = lSelect
                    Next
                Next
                agdSample.Refresh()
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
                Else
                    SelLength = 0
                    txtRuler1.SelectionLength = 0
                End If
                txtSampleAnyChange(-1)
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
                '  cmdSaveScript.Enabled = False
                '  cmdReadData.Enabled = False
                '  Exit Sub
                'End If
            Next

            cmdSaveScript.Enabled = True
        End With

    End Sub

    ''' <summary>Responds to press of "Browse" button by the script filename</summary>
    Private Sub cmdBrowseScript_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrowseScript.Click
        Dim dlgOpenFile As New System.Windows.Forms.OpenFileDialog
        dlgOpenFile.Filter = "Wizard Script Files (*.ws)|*.ws|All Files (*.*)|*.*"
        dlgOpenFile.DefaultExt = "ws"
        dlgOpenFile.Title = "Open Script File"

        If dlgOpenFile.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtScriptFile.Text = dlgOpenFile.FileName
            ReadScript()
        End If
    End Sub

    Public Sub ReadScript()
        If IO.File.Exists(txtScriptFile.Text) Then
            Script = ScriptFromString(WholeFileString((txtScriptFile.Text)))
            If Script Is Nothing Then
                MsgBox("Could not parse " & txtScriptFile.Text & vbCr & Err.Description, MsgBoxStyle.Exclamation, "Read Script")
            Else
                'FoundDDF = True
                SetWizardFromScript(Script)
                agdDataMapping.SizeAllColumnsToContents(agdDataMapping.Width, False)
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
            For lRow As Integer = 1 To nRequiredFields - 1
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
        agdDataMapping.SizeAllColumnsToContents(agdDataMapping.Width, False)
    End Sub

    Private Sub SetColumnFormatFromScript(ByRef scr As clsATCscriptExpression)
        Dim SubExpIndex, ColIndex, SubExpMax As Integer
        Dim ColName, tmpstr, ColCol As String
        Dim StartCol, ColWidth As Integer
        Dim colonPos, r As Integer
        ColIndex = 1
        InitDataMapping()

        Dim rule As String = scr.SubExpression(0).Printable.Trim("""")
        FixedColumns = False
        ColumnDelimiter = ""
        If IsNumeric(rule) Then
            ColumnDelimiter = Chr(CShort(rule))
        Else
            Dim lrule As String = Trim(LCase(rule))
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
                tmpstr = rule.Substring(0, 1)
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
                    tmpstr = rule.Substring(0, colonPos - 1)
                    If IsNumeric(tmpstr) Then
                        ColCol = tmpstr
                        rule = rule.Substring(colonPos)
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
            optDelimiterNone.Checked = True
            MappingSource.Rows = 1
            SetSelFromGrid(1)
        ElseIf ColumnDelimiter = " " Then
            optDelimiterSpace.Checked = True
        ElseIf ColumnDelimiter = vbTab Then
            optDelimiterTab.Checked = True
        Else
            txtDelimiter.Text = ColumnDelimiter
            optDelimiterChar.Checked = True
        End If
        'agdDataMapping.ColsSizeByContents() 
        'agdDataMapping.SizeColumnToContents() 'TODO: this seems to be the replacement of line above?? Apply to all columns?
    End Sub

    'Finds named row in grid by non-case-sensitive comparison ignoring whitespace
    'If a blank row is found at the end of the grid, that row is returned
    'If no row matches, .rows + 1 is returned
    Private Function RowNamed(ByRef FieldName As String) As Integer
        Dim srchName As String
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
        Dim scp As String
        Dim lRow As Integer = RowNamed(subexpName)
        If lRow <= MappingSource.Rows Then
            scp = TrimQuotes(scr.Printable)
            If LCase(scp) <> LCase(subexpName) Then
                MappingSource.CellValue(lRow, ColMappingConstant) = scp
            End If
        End If
    End Sub

    Private Sub SetWizardFromDate(ByRef scr As clsATCscriptExpression)
        Dim cnt As Integer = scr.SubExpressionCount
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
        Dim SubExp, ForMax, r As Integer
        Dim str1, str2 As String
        Select Case scr.Token
            Case clsATCscriptExpression.ATCsToken.tok_And
                ForMax = scr.SubExpressionCount - 1
                For SubExp = 0 To ForMax
                    SetWizardFromScript(scr.SubExpression(SubExp))
                Next
            Case clsATCscriptExpression.ATCsToken.tok_ATCScript
                ForMax = scr.SubExpressionCount - 1
                txtScriptDesc.Text = TrimQuotes(scr.SubExpression(0).Printable)
                For SubExp = 1 To ForMax
                    SetWizardFromScript(scr.SubExpression(SubExp))
                Next
            Case clsATCscriptExpression.ATCsToken.tok_Attribute
                str1 = scr.SubExpression(0).Printable
                r = RowNamed(str1)
                MappingSource.CellValue(r, ColMappingName) = str1
                MappingSource.CellValue(r, ColMappingAttr) = "yes"
                str2 = Trim(scr.SubExpression(1).Printable)
                If VB.Left(str2, 1) = """" Then str2 = Mid(str2, 2)
                If VB.Right(str2, 1) = """" Then str2 = VB.Left(str2, Len(str2) - 1)
                MappingSource.CellValue(r, ColMappingConstant) = str2
            Case clsATCscriptExpression.ATCsToken.tok_ColumnFormat
                SetColumnFormatFromScript(scr)
            Case clsATCscriptExpression.ATCsToken.tok_Dataset
                ForMax = scr.SubExpressionCount - 1
                SubExp = 0
                While SubExp < ForMax
                    str1 = scr.SubExpression(SubExp).Printable
                    r = RowNamed(str1)
                    MappingSource.CellValue(r, ColMappingName) = str1
                    MappingSource.CellValue(r, ColMappingAttr) = "yes"
                    SubExp = SubExp + 1
                    If scr.SubExpression(SubExp).Token = clsATCscriptExpression.ATCsToken.tok_Literal Then
                        str2 = TrimQuotes(scr.SubExpression(SubExp).Printable)
                        MappingSource.CellValue(r, ColMappingConstant) = str2
                    End If
                    SubExp = SubExp + 1
                End While
            Case clsATCscriptExpression.ATCsToken.tok_Date
                SetWizardFromDate(scr)
            Case clsATCscriptExpression.ATCsToken.tok_FatalError
            Case clsATCscriptExpression.ATCsToken.tok_For
                If LCase(Trim(scr.SubExpression(0).Printable)) = "repeat" _
                   AndAlso scr.SubExpression(2).Token = clsATCscriptExpression.ATCsToken.tok_Literal Then
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
                    str1 = ""
                    While SubExp <= ForMax
                        str1 = str1 & scr.SubExpression(SubExp).Printable
                        SubExp = SubExp + 1
                        If SubExp <= ForMax Then str1 = str1 & ","
                    End While
                    If ReverseLogic Then
                        MappingSource.CellValue(r, ColMappingSkip) = str1
                    End If
                End If
            Case clsATCscriptExpression.ATCsToken.tok_Increment
            Case clsATCscriptExpression.ATCsToken.tok_LineEnd
                str1 = UCase(scr.SubExpression(0).Printable)
                If IsNumeric(str1) Then
                    txtLineLen.Text = str1
                    optLineEndLength.Checked = True
                ElseIf VB.Left(str1, 1) = "A" And IsNumeric(Mid(str1, 2)) Then
                    txtLineEndChar.Text = Mid(str1, 2)
                    optLineEndASCII.Checked = True
                ElseIf str1 = "CR" Then
                    optLineEndCRLF.Checked = True
                ElseIf str1 = "LF" Then
                    optLineEndLF.Checked = True
                Else : MsgBox("Unknown LineEnd '" & str1 & "' in SetWizardFromScript")
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
                        str1 = scr.SubExpression(0).Printable
                        SubExp = Len("(= HeaderStart ")
                        If VB.Left(str1, SubExp) = "(= HeaderStart " Then
                            chkSkipHeader.CheckState = System.Windows.Forms.CheckState.Checked
                            optHeaderStartsWith.Checked = True
                            txtHeaderStart.Text = Mid(str1, SubExp + 1, Len(str1) - SubExp - 1)
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
        Dim tmpstr, tmpstr2 As String
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
        Dim ScriptBuilder As New System.Text.StringBuilder("(ATCScript ")
        tmpstr = Trim(txtScriptDesc.Text)
        If tmpstr = "" Then tmpstr = IO.Path.GetFileName((txtScriptFile.Text))
        If tmpstr = "" Then tmpstr = "ReadData"
        ScriptBuilder.Append("""" & tmpstr & """")
        ScriptBuilder.Append(PrintEOL & Space(indent) & "(LineEnd ")
        If optLineEndCRLF.Checked = True Then
            ScriptBuilder.Append("CR")
        ElseIf optLineEndLF.Checked = True Then
            ScriptBuilder.Append("LF")
        ElseIf optLineEndASCII.Checked = True Then
            ScriptBuilder.Append("A" & Trim(txtLineEndChar.Text))
        ElseIf optLineEndLength.Checked = True Then
            ScriptBuilder.Append(Trim(txtLineLen.Text))
        End If
        ScriptBuilder.Append(")" & PrintEOL)

        If chkSkipHeader.CheckState = System.Windows.Forms.CheckState.Checked Then
            If optHeaderStartsWith.Checked Then 'Starts With
                ScriptBuilder.Append(Space(indent) & "(While (= HeaderStart """ & txtHeaderStart.Text & """)" & PrintEOL)
                ScriptBuilder.Append(Space(indent) & "       (NextLine))" & PrintEOL)
            ElseIf optHeaderLines.Checked Then  'Number of lines
                ScriptBuilder.Append(Space(indent) & "(NextLine " & txtHeaderLines.Text & ")" & PrintEOL)
            End If
        End If

        ScriptBuilder.Append(Space(indent) & "(ColumnFormat ")
        indent = indent + Len("(ColumnFormat ")
        If FixedColumns Then
            ScriptBuilder.Append("Fixed")
        Else 'delimited
            For ParsePos = 0 To ColumnDelimiter.Length - 1
                tmpstr = ColumnDelimiter.Substring(ParsePos, 1)
                Select Case tmpstr
                    Case vbTab : ScriptBuilder.Append("tab")
                    Case " " : ScriptBuilder.Append("space")
                    Case Else : ScriptBuilder.Append(tmpstr)
                End Select
            Next
        End If
        If optHeaderStartsWith.Checked Then 'Starts With
            ScriptBuilder.Append(PrintEOL & Space(indent) & "1")
            If Len(txtHeaderStart.Text) > 1 Then ScriptBuilder.Append("-" & Len(txtHeaderStart.Text) - 1)
            ScriptBuilder.Append(":" & "HeaderStart")
        End If
        For r = 1 To MappingSource.Rows
            tmpstr = Trim(MappingSource.CellValue(r, ColMappingCol))
            If tmpstr <> "" Then
                tmpstr2 = Trim(MappingSource.CellValue(r, ColMappingName))
                ScriptBuilder.Append(PrintEOL & Space(indent) & tmpstr)
                ScriptBuilder.Append(":" & tmpstr2)

                If LCase(tmpstr2) = "repeating" Then
                    RepeatStart = FixedColLeft(r)
                    RepeatEnd = FixedColRight(r)
                End If
            End If
        Next
        ScriptBuilder.Append(")")
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
                            ScriptBuilder.Append(PrintEOL & Space(indent) & "(Attribute ")
                        Else
                            ScriptBuilder.Append(PrintEOL & Space(indent) & "(Set ")
                        End If
                        ScriptBuilder.Append(Trim(MappingSource.CellValue(r, ColMappingName)))
                        ScriptBuilder.Append(" """ & tmpstr & """)")
                    End If
                End If
            Next
        End If

        ScriptBuilder.Append(PrintEOL & Space(indent) & "(While (Not EOF)")
        indent = indent + Len("(While ")

        'Have to make sure this datum goes into the right dataset
        'UPGRADE_ISSUE: GoSub statement is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C5A1A479-AB8B-4D40-AAF4-DB19A2E5E77F"'
        If SomeAttribVaries And Not SomeAttribVariesRepeat Then
            SelectDataset(ScriptBuilder, indent)
        End If

        If RepeatStart > 0 And RepeatEnd >= RepeatStart Then
            '    str = str & printeol & Space(indent) & "(Set Repeat 0)"
            '    str = str & printeol & Space(indent) & "(While (Not EOL)"
            '    indent = indent + Len("(While ")
            '    str = str & printeol & Space(indent) & "(Increment Repeat)"
            'UPGRADE_WARNING: Couldn't resolve default property of object ConstOrCol(Repeats). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ScriptBuilder.Append(PrintEOL & Space(indent) & "(For Repeat = 1 to " & ConstOrCol("Repeats"))
            indent = indent + Len("(For ")
        End If

        NestedIfs = 0
        For r = 1 To MappingSource.Rows
            tmpstr = Trim(MappingSource.CellValue(r, ColMappingSkip))
            If tmpstr <> "" Then NestedIfs = NestedIfs + 1
        Next r

        If NestedIfs > 0 Then
            ScriptBuilder.Append(PrintEOL & Space(indent) & "(If ")
            indent = indent + Len("(If ")
            CurrentIf = 1
            If NestedIfs > 1 Then
                ScriptBuilder.Append("(And ")
                indent = indent + Len("(And ")
            End If
            For r = 1 To MappingSource.Rows
                tmpstr = Trim(MappingSource.CellValue(r, ColMappingSkip))
                If tmpstr <> "" Then
                    commaPos = InStr(tmpstr, ",")
                    If commaPos = 0 Then
                        ScriptBuilder.Append("(<> " & ConstOrCol(Trim(MappingSource.CellValue(r, ColMappingName))) & " " & tmpstr & ")")
                    Else
                        ScriptBuilder.Append("(Not (In " & ConstOrCol(Trim(MappingSource.CellValue(r, ColMappingName))))
                        While commaPos > 0
                            ScriptBuilder.Append(" " & VB.Left(tmpstr, commaPos - 1))
                            If commaPos > Len(tmpstr) Then
                                commaPos = 0
                            Else
                                tmpstr = Mid(tmpstr, commaPos + 1)
                                commaPos = InStr(tmpstr, ",")
                                If commaPos = 0 Then commaPos = Len(tmpstr) + 1
                            End If
                        End While
                        ScriptBuilder.Append("))")
                    End If
                    If CurrentIf < NestedIfs Then ScriptBuilder.Append(PrintEOL & Space(indent))
                    CurrentIf = CurrentIf + 1
                End If
            Next r
            If NestedIfs > 1 Then
                indent = indent - Len("(And ")
                ScriptBuilder.Append(PrintEOL & Space(indent) & ")")
            End If
        End If

        'Have to make sure this datum goes into the right dataset
        'UPGRADE_ISSUE: GoSub statement is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C5A1A479-AB8B-4D40-AAF4-DB19A2E5E77F"'
        If SomeAttribVariesRepeat Then
            SelectDataset(ScriptBuilder, indent)
        End If
        ScriptBuilder.Append(PrintEOL & Space(indent) & "(Date ")
        indent += "(Date ".Length
        ScriptBuilder.Append(ConstOrCol("Year"))
        ScriptBuilder.Append(PrintEOL & Space(indent) & ConstOrCol("Month"))
        ScriptBuilder.Append(PrintEOL & Space(indent) & ConstOrCol("Day"))
        ScriptBuilder.Append(PrintEOL & Space(indent) & ConstOrCol("Hour"))
        ScriptBuilder.Append(PrintEOL & Space(indent) & ConstOrCol("Minute") & ")")
        indent = indent - Len("(Date ")

        ScriptBuilder.Append(PrintEOL & Space(indent) & "(Value " & ConstOrCol("Value") & ")")

        'For r = 1 To NestedIfs
        If NestedIfs > 0 Then
            indent = indent - Len("(If ")
            ScriptBuilder.Append(PrintEOL & Space(indent) & ")")
        End If
        'Next r

        If RepeatStart > 0 And RepeatEnd >= RepeatStart Then
            indent = indent - Len("(For ")
            ScriptBuilder.Append(PrintEOL & Space(indent) & ")")
        End If

        ScriptBuilder.Append(PrintEOL & Space(indent) & "(NextLine)")
        indent = indent - Len("(While ")
        ScriptBuilder.Append(PrintEOL & Space(indent) & ")")

        ScriptBuilder.Append(PrintEOL & ")")

        Return ScriptBuilder.ToString
    End Function

    ''' <summary>
    ''' This is originally a GoSub section
    ''' </summary>
    ''' <param name="ScriptBuilder"></param>
    ''' <param name="aIndent">Number of spaces to indent</param>
    ''' <remarks></remarks>
    Private Sub SelectDataset(ByVal ScriptBuilder As System.Text.StringBuilder, ByVal aIndent As Integer)
        Const cDataset As String = "(Dataset "
        ScriptBuilder.Append(PrintEOL & Space(aIndent) & cDataset)
        Dim lIndent As String = Space(aIndent + cDataset.Length)
        Dim lAppendedAny As Boolean = False

        For r = 1 To MappingSource.Rows
            Dim tmpstr As String = MappingSource.CellValue(r, ColMappingAttr).Trim.ToLower
            If tmpstr.StartsWith("y") Then
                Dim tmpstr2 As String = MappingSource.CellValue(r, ColMappingName).Trim
                tmpstr = ConstOrCol(tmpstr2)
                If Not String.IsNullOrEmpty(tmpstr) Then
                    If lAppendedAny Then
                        ScriptBuilder.Append(PrintEOL & lIndent)
                    Else
                        lAppendedAny = True
                    End If
                    ScriptBuilder.Append(tmpstr2 & " " & tmpstr)
                End If
            End If
        Next
        ScriptBuilder.Append(")")
    End Sub

    Private Function ConstOrCol(ByRef FieldName As String) As String
        Dim r As Integer
        Dim constStr, colStr, retval As String
        retval = ""
        If FieldName.ToLower = "repeat" Then
            retval = FieldName
        Else
            r = RowNamed(FieldName)
            If r > MappingSource.Rows Then
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
        'FoundDDF = False
        ' +------------------------------------------------------------+
        ' | "sstInputWizard.Enabled = False"  <-  Disables everything!  |
        ' | Alternative: Handle this in "sstInputWizard_Click" event    |
        ' +------------------------------------------------------------+
        'lblDataDescFile.Enabled = False
        'txtScriptFile.Enabled = False
        'cmdBrowseScript.Enabled = False
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
        'cmdSaveScript.Enabled = False
    End Sub

    ' Subroutine ===============================================
    ' Name:      EnableFilePropertiesFields
    ' Purpose:   Enable fields on the File-Properties tab
    '            when data file has been opened
    '
    Private Sub EnableFilePropertiesFields()
        'zzz this is not needed ?  PopulateGridSample
        lblDataDescFile.Enabled = True
        txtScriptFile.Enabled = True
        cmdBrowseScript.Enabled = True
        'lblFileType.Enabled = True
        'lblDelimiter.Enabled = True
        txtDelimiter.Enabled = True
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

    ''' <summary>Responds to press of "Cancel" button</summary>
    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Responds to press of "Save Script" button
    ''' </summary>
    Private Sub cmdSaveScript_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSaveScript.Click
        Dim dlgSaveScript As New Windows.Forms.SaveFileDialog
        dlgSaveScript.Filter = "Wizard Script Files (*.ws)|*.ws|All Files|*.*"
        dlgSaveScript.DefaultExt = "ws"
        dlgSaveScript.Title = "Save Script As"
        dlgSaveScript.FileName = txtScriptFile.Text
        If dlgSaveScript.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtScriptFile.Text = dlgSaveScript.FileName
            If txtScriptDesc.Text.Trim.Length = 0 Then txtScriptDesc.Text = IO.Path.GetFileNameWithoutExtension(dlgSaveScript.FileName)
            SaveFileString(dlgSaveScript.FileName, ScriptStringFromWizard)
            SaveSetting("ATCTimeseriesImport", "Scripts", dlgSaveScript.FileName, txtScriptDesc.Text)
        End If
    End Sub

    'Returns position of next character from aChars in aString starting at position aStart
    'Returns aString.Length if none were found
    Private Function FirstCharPos(ByRef aStart As Integer, ByRef aString As String, ByRef aChars As String) As Integer
        Dim lRetval As Integer = aString.Length
        Dim lLastChar As Integer = aChars.Length - 1
        For CharPos As Integer = 0 To lLastChar
            Dim curval As Integer = aString.IndexOf(aChars.Substring(CharPos, 1), aStart)
            If curval > 0 AndAlso curval < lRetval Then lRetval = curval
        Next CharPos
        Return lRetval
    End Function

    ' Function  - - - - - - - - - - - - - - - - - - - - - - - - -
    ' Name:     ParseInputLine (3 arguments)
    ' Purpose:  Returns number of columns parsed from buffer into array
    '           Populates parsed array from element 1 to index returned
    '
    Private Function ParseInputLine(ByVal InBuf As String, ByRef parsed() As String) As Integer

        Dim parseCol As Integer = 0
        Dim fromCol As Integer = 0
        Dim toCol As Integer
        If Not FixedColumns Then 'parse delimited text
            While fromCol < InBuf.Length AndAlso parseCol < UBound(parsed)
                toCol = FirstCharPos(fromCol, InBuf, ColumnDelimiter)
                If toCol < fromCol Then toCol = Len(InBuf) + 1
                parseCol = parseCol + 1
                parsed(parseCol) = InBuf.Substring(fromCol, toCol - fromCol)
                fromCol = toCol + 1
                If ColumnDelimiter = " " Then 'treat multiple contiguous spaces as one delimiter
                    While fromCol + 1 < InBuf.Length AndAlso InBuf.Substring(fromCol, 1) = ColumnDelimiter
                        fromCol += 1
                    End While
                End If
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
        Return parseCol
    End Function

    ' Subroutine ===============================================
    ' Name:      PopulateGridTest
    ' Purpose:   Populates the Test-Mapping grid with data
    '            fields selected on the Data-Mapping tab
    '
    Private Sub PopulateGridTest()
        'Dim lines As Integer
        'Dim linecnt As Integer
        'Dim cbuff As String
        'Dim parsed(conMaxNumColumns) As String
        'Dim pcols As Integer
        'Dim cout As Integer
        'Dim cin As Integer ' column out (agdTestMapping), in (agdDataMapping)
        'Dim icl As Integer
        'Dim ics As String ' input column long, string

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
        Dim lNewSource As New atcControls.atcGridSource
        SetLineEndingFromForm()
        If LenDataFile > 0 Then
            NextLineStart = 1
            SkipHeader()
            With lNewSource
                Dim lines As Integer
                Dim linecnt As Integer
                Dim parsed(conMaxNumColumns) As String
                Dim pcols As Integer

                lines = conNumSampleLines
                linecnt = 0
                While linecnt < lines
                    pcols = ParseInputLine(CurrentLine, parsed)
                    linecnt += 1
                    For c As Integer = 1 To pcols
                        .CellValue(linecnt, c - 1) = parsed(c)
                    Next
                    If ScriptEndOfData() Then Exit While
                    ScriptNextLine()
                End While
                For c = 0 To .Columns - 1
                    If FixedColumns Then
                        .CellValue(0, c) = FixedColLeft(c + 1) & "-" & FixedColRight(c + 1)
                    Else
                        .CellValue(0, c) = c + 1
                    End If
                Next c
            End With
        End If
        agdSample.Initialize(lNewSource)
        agdSample.Refresh()
    End Sub

    Private Sub SetLineEndingFromForm()
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
    End Sub

    ''' <summary>
    ''' Advance ScriptNextLine until CurrentLine is the line after all header lines
    ''' </summary>
    ''' <remarks>
    ''' Even if there is no header to skip, still advances to next line in file
    ''' </remarks>
    Private Sub SkipHeader()
        ScriptNextLine()
        If chkSkipHeader.CheckState = System.Windows.Forms.CheckState.Checked Then
            If optHeaderStartsWith.Checked = True And Len(txtHeaderStart.Text) > 0 Then 'skip header lines starting with string
                While Not ScriptEndOfData() AndAlso CurrentLine.StartsWith(txtHeaderStart.Text)
                    ScriptNextLine()
                End While
            ElseIf optHeaderLines.Checked = True AndAlso IsNumeric(txtHeaderLines.Text) Then  'Skip number of header lines
                Dim lSkipNumLines As Integer = txtHeaderLines.Text
                While Not ScriptEndOfData() AndAlso lSkipNumLines > 0
                    ScriptNextLine()
                    lSkipNumLines -= 1
                End While
            End If
        End If
    End Sub

    ''' <summary>
    ''' For fixed format files, populates the Sample text box from the data file
    ''' </summary>
    Private Sub PopulateTxtSample()
        Dim linecnt As Integer = 0
        SetLineEndingFromForm()
        If LenDataFile > 0 Then
            Try
                NextLineStart = 1
                SkipHeader()

                linecnt = 1
                'Skip lines vertical scroll has scrolled past
                While Not ScriptEndOfData() AndAlso linecnt < VScrollSample.Value
                    ScriptNextLine()
                    linecnt += 1
                End While

                'Read portion of lines right of horizontal scroll position
                Dim nChars As Integer = _txtSample_0.Width / CharWidth - 1
                linecnt = 0
                While linecnt < txtSample.Count
                    txtSample(linecnt).Text = Mid(CurrentLine, HScrollSample.Value, nChars)
                    txtSample(linecnt).SelectionStart = txtRuler1.SelectionStart
                    txtSample(linecnt).SelectionLength = txtRuler1.SelectionLength
                    If ScriptEndOfData() Then Exit While
                    ScriptNextLine()
                    linecnt += 1
                End While
            Catch e As Exception

            End Try
            EnableFilePropertiesFields()
        End If
        While linecnt < txtSample.Count
            txtSample(linecnt).Text = ""
            linecnt = linecnt + 1
        End While
        Exit Sub
    End Sub


    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     optDelimiter_Click (1 argument)
    ' Purpose:  Responds to press of a "Delimiter-option" button
    '
    'UPGRADE_WARNING: Event optDelimiter.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optDelimiter_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optDelimiterNone.CheckedChanged, optDelimiterSpace.CheckedChanged, optDelimiterTab.CheckedChanged, optDelimiterChar.CheckedChanged
        If eventSender.Checked Then

            If optDelimiterNone.Checked Then
                FixedColumns = True
            ElseIf optDelimiterSpace.Checked Then
                FixedColumns = False : ColumnDelimiter = " "
            ElseIf optDelimiterTab.Checked Then
                FixedColumns = False : ColumnDelimiter = Chr(9)
            ElseIf optDelimiterChar.Checked Then
                FixedColumns = False : ColumnDelimiter = txtDelimiter.Text
            End If

            If Not FixedColumns Then
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
        If SelLength > 0 AndAlso Not SettingSelFromGrid AndAlso pDataMappingRow > -1 Then
            If SelLength < 2 Then
                MappingSource.CellValue(pDataMappingRow, ColMappingCol) = SelStart
            Else
                MappingSource.CellValue(pDataMappingRow, ColMappingCol) = SelStart & "-" & SelStart + SelLength - 1
            End If
            '    SetFixedWidthsFromDataMapping
            agdDataMapping.Refresh()
        End If
    End Sub

    Private Sub txtDataFile_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDataFile.Enter
        txtDataFile.SelectionStart = 0
        txtDataFile.SelectionLength = Len(txtDataFile.Text)
    End Sub

    Private Sub txtDataFile_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDataFile.TextChanged
        If txtDataFile.Text <> NameDataFile Then
            OpenDataFile()
        End If
    End Sub

    'Start with text in txtDataFile, try to open the file and set NameDataFile
    Private Sub OpenDataFile()
        NameDataFile = Trim(txtDataFile.Text)

        If Not txtDataFile.Text.Contains(IO.Path.DirectorySeparatorChar) Then
            NameDataFile = IO.Path.Combine(CurDir(), NameDataFile)
        End If

        OpenUnitDataFile()
    End Sub

    ' Event     <><><><><><><><><><><><><><><><><><><><><><><><>
    ' Name:     cmdBrowseData_Click
    ' Purpose:  Responds to press of the "Browse" button
    '           to obtain the input file name.
    '
    Private Sub cmdBrowseData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrowseData.Click
        Dim dlgOpenFileOpen As New Windows.Forms.OpenFileDialog
        dlgOpenFileOpen.Filter = "All Files (*.*)|*.*"
        dlgOpenFileOpen.DefaultExt = ""
        dlgOpenFileOpen.Title = "Open Data File"

        If dlgOpenFileOpen.ShowDialog = Windows.Forms.DialogResult.OK Then
            NameDataFile = dlgOpenFileOpen.FileName
            OpenUnitDataFile()
        End If
    End Sub

    Private Sub OpenUnitDataFile()
        Dim msg As String = ScriptOpenDataFile(NameDataFile)
        If msg = "OK" Then
            'UnitDataFileReader = New IO.StringReader(WholeDataFile)
            txtDataFile.Text = NameDataFile
            PopulateSample()
        Else
            Logger.Dbg("Could not open data file: " & msg)
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
        'Dim m_FilePath As String
        'Dim n As Integer

        '' If a filename was not entered, reset found flags and
        '' exit the subroutine:
        ''
        'If Len(Trim(txtScriptFile.Text)) = 0 Then
        '    '    FoundData = False
        '    FoundDDF = False
        '    '    txtLinesToSkip.SetFocus
        '    '    FoundPas = False
        '    Exit Sub
        'End If

        '' Something was entered in the script file text box:
        ''
        '' Try to find a \ in the name. If so then assume the user
        '' entered a full pathname. If not, then append the cur directory
        '' to get a fullpathname.
        ''
        'n = InStr(1, txtScriptFile.Text, "\", CompareMethod.Text)
        'If n > 0 Then
        '    NameDescFile = Trim(txtScriptFile.Text)
        'Else
        '    m_FilePath = CurDir()
        '    NameDescFile = m_FilePath & "\" & Trim(txtScriptFile.Text)
        'End If

        ''  FoundDDF = DoesFileExist(NameDescFile)
        ''
        ''  If FoundDDF = True Then
        ''    If UnitDescFile > 0 Then Close UnitDescFile
        ''    modFileIO.OpenFile UnitDescFile, NameDescFile, "Input"
        ''    If UnitDescFile > 0 Then
        ''        txtScriptFile.Text = NameDescFile
        ''        ReadDescFile
        ''    End If
        ''
        ''    PromptForDataMapping
        ''  Else
        ''    MsgBox "ERROR: FILE NOT FOUND:" & vbCrLf _
        ' ''           & Chr(34) & txtScriptFile.Text & Chr(34)
        ''    txtScriptFile.Text = ""
        ''    txtScriptFile.SetFocus
        ''
        ''  End If
    End Sub

    ''' <summary>Responds to any change in the "txtDelimiter" text box</summary>
    Private Sub txtDelimiter_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDelimiter.TextChanged
        optDelimiterChar.Checked = True
        ColumnDelimiter = txtDelimiter.Text
        If fraColSample.Visible Then PopulateGridSample()
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

    Private Sub HScrollSample_Scroll(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ScrollEventArgs) Handles HScrollSample.Scroll
        SetRulers()
        PopulateTxtSample()
    End Sub

    Private Sub VScrollSample_Scroll(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ScrollEventArgs) Handles VScrollSample.Scroll
        PopulateTxtSample()
    End Sub

    Private Sub agdSample_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdSample.MouseDownCell
        If pDataMappingCol > -1 AndAlso pDataMappingRow > -1 Then
            MappingSource.CellValue(pDataMappingRow, pDataMappingCol) = aColumn + 1
            agdDataMapping.Refresh()
        End If
    End Sub
End Class