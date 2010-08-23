Option Strict Off
Option Explicit On

Imports atcUtility

Friend Class frmSelectScript
    Inherits System.Windows.Forms.Form
    'Copyright 2010 by AQUA TERRA Consultants

    Public SelectedScript As String
    Public ButtonPressed As String
    Private pDataFilename As String
    Private pCurrentRow As Integer = 1
    Private CanReadBackColor As Drawing.Color = Drawing.Color.FromArgb(11861940) 'RGB(180, 255, 180)
    Private NotReadableBackColor As Drawing.Color = Drawing.Color.Red

    Private Sub agdScripts_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdScripts.MouseDownCell
        SelectedScript = agdScripts.Source.CellValue(aRow, 1)
        Dim lLastRow As Integer = agdScripts.Source.Rows - 1
        For lRow As Integer = 1 To lLastRow
            If lRow = aRow Then
                agdScripts.Source.CellSelected(lRow, 0) = True
            Else
                agdScripts.Source.CellSelected(lRow, 0) = False
            End If

        Next
        agdScripts.Refresh()
        EnableButtons()
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        ButtonPressed = cmdCancel.Text
        Me.Hide()
    End Sub

    Private Sub cmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Click
        Dim bgColor As Drawing.Color
        ButtonPressed = cmdFind.Text
        dlgOpenFileOpen.Filter = "Wizard Script Files (*.ws)|*.ws|All Files (*.*)|*.*"
        dlgOpenFileOpen.DefaultExt = "ws"
        dlgOpenFileOpen.Title = "Open Script File"
        dlgOpenFileOpen.ShowDialog()
        Dim ScriptFilename, ScriptDescription As String
        Dim Script As clsATCscriptExpression
        If dlgOpenFileOpen.FileName <> "" Then
            ScriptFilename = dlgOpenFileOpen.FileName
            Script = ScriptFromString(WholeFileString(ScriptFilename))
            If Script Is Nothing Then
                ScriptDescription = Err.Description
                bgColor = NotReadableBackColor
            Else
                ScriptDescription = Script.SubExpression(0).Printable
                Script = Nothing
                SaveSetting("ATCTimeseriesImport", "Scripts", ScriptFilename, ScriptDescription)
                bgColor = TestScriptColor(ScriptFilename) 'TODO: debug this later
            End If
            With agdScripts.Source
                Dim lCurRow As Integer = .Rows
                .Rows = .Rows + 1
                .CellValue(lCurRow, 0) = ScriptDescription
                .CellColor(lCurRow, 0) = bgColor
                .CellValue(lCurRow, 1) = ScriptFilename
                .CellColor(lCurRow, 1) = bgColor
                'TODO: scroll down so this row is visible, translate from old grid code below:
                'While Not .get_RowIsVisible(agdScripts.rows)
                '    .TopRow = .TopRow + 1
                'End While
                '.set_Selected(.Rows, 0, True)
                .CellSelected(lCurRow, 0) = True ' had to add this to make the text be visible
                agdScripts.Refresh()
            End With
            EnableButtons()
        End If
    End Sub

    Private Sub cmdHelp_Click()
        MsgBox("Select a script that will recognize the data you are importing. " & vbCr & "If no appropriate script is listed, select a similar one " & vbCr & "and click 'Edit' to create a new script based on it." & vbCr & "'Run' interprets the selected script and imports your data." & vbCr & "'Edit' reads the selected script and presents an interface for customizing it." & vbCr & "      Note: some complex scripts use features that can not yet be edited in the graphical " & vbCr & "      interface. These scripts may be edited manually as text files before pressing 'Run'. " & vbCr & "'Find' browses your disk for new scripts that are not in the list." & vbCr & "'Forget' removes the selected script from the list, but leaves it on disk." & vbCr & "'Debug' runs the selected script one step at a time." & vbCr & "'Cancel' closes this window without importing any data" & vbCr & "Green scripts have tested the current file and can probably read it." & vbCr & "Pink scripts have tested the current file and probably can't read it." & vbCr & "Red scripts contain errors or cannot be found on disk." & vbCr & "Other scripts are unable to test files for readability.", MsgBoxStyle.OkOnly, "Help for Script Selection")
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        If agdScripts.Source.CellValue(pCurrentRow, 1) <> "" Then
            If MsgBox("About to forget script:" & vbCr & "Description: " & agdScripts.Source.CellValue(pCurrentRow, 0) & vbCr & "Filename: " & agdScripts.Source.CellValue(pCurrentRow, 1), MsgBoxStyle.YesNo, "Confirm Forget") = MsgBoxResult.Yes Then
                DeleteSetting("ATCTimeseriesImport", "Scripts", agdScripts.Source.CellValue(pCurrentRow, 1))
                LoadGrid(pDataFilename) 'This is inefficient, but easier than copying all the .textmatrix and .cellbackcolor
                'agdScripts.TextMatrix(pCurrentRow, 0) = ""
                'agdScripts.TextMatrix(pCurrentRow, 1) = ""
            End If
        End If
        EnableButtons()
    End Sub

    Private Sub cmdRun_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRun.Click
        ButtonPressed = cmdRun.Text
        Me.Hide()
    End Sub

    Private Sub cmdTest_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdTest.Click
        ButtonPressed = cmdTest.Text
        Me.Hide()
    End Sub

    Private Sub cmdWizard_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdWizard.Click
        ButtonPressed = cmdWizard.Text
        Me.Hide()
    End Sub

    Public Sub LoadGrid(Optional ByRef DataFilename As String = "")
        Dim MySettings As String(,)
        Dim intSettings As Integer
        Dim bgColor As Drawing.Color
        Dim CanReadRow As Integer
        Dim RowsFilled As Integer

        pDataFilename = DataFilename
        Dim lSource As New atcControls.atcGridSource

        With lSource
            MySettings = GetAllSettings("ATCTimeseriesImport", "Scripts")
            .Columns = 2
            .Rows = UBound(MySettings, 1) - LBound(MySettings, 1) + 3 'the number of scripts found plus title line and first blank line

            .CellValue(0, 0) = "Description"
            .CellValue(0, 1) = "Script File"
            CanReadRow = 0
            .CellValue(1, 0) = "Blank Script"
            .CellValue(1, 1) = ""
            RowsFilled = 1
            If MySettings Is Nothing Then
                MsgBox("Use the Find button to locate scripts." & vbCr & "Look for the Scripts directory where this program is installed.", MsgBoxStyle.OkOnly, "No Scripts Found Yet")
            Else
                For intSettings = LBound(MySettings, 1) To UBound(MySettings, 1)
                    'Set filename in second column
                    Dim lRow As Integer = RowsFilled + 1
                    If IO.File.Exists(MySettings(intSettings, 0)) Then
                        'Set filename in column 1
                        .CellValue(lRow, 1) = MySettings(intSettings, 0)

                        'Set description in column 0
                        .CellValue(lRow, 0) = MySettings(intSettings, 1).Trim("""")

                        'Set background of cell based on whether this script can read data file
                        'bgColor = TestScriptColor(MySettings(intSettings, 0)) 'TODO: debug this one later
                        .CellColor(lRow, 1) = bgColor
                        .CellColor(lRow, 0) = bgColor

                        If bgColor = CanReadBackColor Then CanReadRow = lRow
                        RowsFilled = RowsFilled + 1
                    End If
                Next intSettings
            End If
            .Rows = RowsFilled + 1 'only retain non-missing scripts
            .FixedRows = 1

            If CanReadRow > 0 Then
                pCurrentRow = CanReadRow
                .CellSelected(CanReadRow, 0) = True
                '.row = CanReadRow
                '.set_Selected(CanReadRow, 0, True)
                'If Not .get_RowIsVisible(CanReadRow) Then .TopRow = CanReadRow
            End If
        End With
        agdScripts.Initialize(lSource)
        agdScripts.SizeColumnToContents(0)
        agdScripts.ColumnWidth(1) = agdScripts.ClientSize.Width - agdScripts.ColumnWidth(0) - 15
        EnableButtons()

    End Sub

    Private Function TestScriptColor(ByRef ScriptFilename As String) As Drawing.Color
        Dim Script As clsATCscriptExpression
        Dim TestResult As String
        Dim filename, ScriptString As String

        On Error GoTo ErrExit
        With agdScripts
            TestScriptColor = .CellBackColor
            If pDataFilename <> "" Then
                If IO.File.Exists(ScriptFilename) Then
                    ScriptString = WholeFileString(ScriptFilename)
                    If InStr(ScriptString, "(Test ") > 0 Then
                        Script = ScriptFromString(ScriptString)
                        If Script Is Nothing Then
                            GoTo ErrExit
                        Else
                            TestResult = ScriptTest(Script, pDataFilename)
                            Select Case TestResult
                                Case "0" : TestScriptColor = NotReadableBackColor ' No, this script can not read this data file
                                Case "1" : TestScriptColor = CanReadBackColor ' Yes, this script can read this data file
                                Case Else : MsgBox("Script '" & ScriptFilename & "' test says: " & vbCr & TestResult & vbCr & "(Expected 0 or 1)", MsgBoxStyle.OkOnly, "Script Test")
                            End Select
                        End If
                    End If
                End If
            End If
        End With
        Exit Function
ErrExit:
        TestScriptColor = NotReadableBackColor
    End Function

    Private Sub frmSelectScript_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
        'If VB6.PixelsToTwipsY(Height) > 600 Then agdScripts.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 576)
        'If VB6.PixelsToTwipsX(Width) > 450 Then
        '	fraButtons.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - VB6.PixelsToTwipsX(fraButtons.Width) - 192)
        '	If VB6.PixelsToTwipsX(fraButtons.Left) > 300 Then agdScripts.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraButtons.Left) - 228)
        'End If
    End Sub

    Private Sub EnableButtons()
        'TODO: enable appropriate buttons
        '      If agdScripts.SelCount > 0 And Len(agdScripts.Ctlget_Text()) > 0 Then
        '          cmdWizard.Enabled = True
        '          If pCurrentRow > 1 Then
        '              cmdRun.Enabled = True
        '          Else
        '              cmdRun.Enabled = False
        '          End If
        '      Else
        '          cmdWizard.Enabled = False
        '          cmdRun.Enabled = False
        '      End If
        'cmdDelete.Enabled = cmdRun.Enabled
        'cmdTest.Enabled = cmdRun.Enabled
    End Sub
End Class