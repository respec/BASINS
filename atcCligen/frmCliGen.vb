Imports atcControls
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports System.Drawing

Public Class frmCliGen
    Inherits System.Windows.Forms.Form
    Dim pParmFileName As String
    Dim pOutFileName As String
    Dim pStartYear As Integer
    Dim pNumYears As Integer
    Dim pOk As Boolean
    Dim cHeader As String
    Dim cFooter As String
    Dim cTable As atcTableFixed
    Dim cParmStr As String 'string form of cTable
    Dim cParms As New atcCollection 'of booleans indicating which parameters to edit
    Dim cParmDescs As New atcCollection 'of strings describing parameters to edit
    Dim cParmsFile As String 'file containing list of parameters to edit
    Dim cParmFileFilter As String = "Cligen Parameter Files (*.par)|*.par"
    Dim cOutFileFilter As String = "Cligen Output Files (*.dat)|*.dat"
    Dim cParms2EditFilter As String = "Cligen Parameters to Edit Files (*.prm)|*.prm"
    Dim cModByPercent As Boolean = False

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
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
    Friend WithEvents agdSic As atcControls.atcGrid
    Friend WithEvents lblStation As System.Windows.Forms.Label
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnParmFile As System.Windows.Forms.Button
    Friend WithEvents lblParm As System.Windows.Forms.Label
    Friend WithEvents lblOut As System.Windows.Forms.Label
    Friend WithEvents btnOutFile As System.Windows.Forms.Button
    Friend WithEvents lblStart As System.Windows.Forms.Label
    Friend WithEvents lblNYrs As System.Windows.Forms.Label
    Friend WithEvents txtNumYrs As System.Windows.Forms.TextBox
    Friend WithEvents txtStartYear As System.Windows.Forms.TextBox
    Friend WithEvents lblCliGen As System.Windows.Forms.Label
    Friend WithEvents txtParmFile As System.Windows.Forms.TextBox
    Friend WithEvents txtOutFile As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents agdMonParms As atcControls.atcGrid
    Friend WithEvents btnSelParms As System.Windows.Forms.Button
    Friend WithEvents lblEditVals As System.Windows.Forms.Label
    Friend WithEvents rdoAbs As System.Windows.Forms.RadioButton
    Friend WithEvents rdoPct As System.Windows.Forms.RadioButton
    Friend WithEvents fraStaParms As System.Windows.Forms.GroupBox
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents lblData As System.Windows.Forms.Label
    Friend WithEvents chkDaily As System.Windows.Forms.CheckBox
    Friend WithEvents chkHourly As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCliGen))
        Me.fraStaParms = New System.Windows.Forms.GroupBox
        Me.btnReset = New System.Windows.Forms.Button
        Me.rdoPct = New System.Windows.Forms.RadioButton
        Me.rdoAbs = New System.Windows.Forms.RadioButton
        Me.lblEditVals = New System.Windows.Forms.Label
        Me.btnSelParms = New System.Windows.Forms.Button
        Me.agdMonParms = New atcControls.atcGrid
        Me.lblStation = New System.Windows.Forms.Label
        Me.btnParmFile = New System.Windows.Forms.Button
        Me.btnRun = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblCliGen = New System.Windows.Forms.Label
        Me.lblParm = New System.Windows.Forms.Label
        Me.lblOut = New System.Windows.Forms.Label
        Me.btnOutFile = New System.Windows.Forms.Button
        Me.lblStart = New System.Windows.Forms.Label
        Me.lblNYrs = New System.Windows.Forms.Label
        Me.txtNumYrs = New System.Windows.Forms.TextBox
        Me.txtStartYear = New System.Windows.Forms.TextBox
        Me.txtParmFile = New System.Windows.Forms.TextBox
        Me.txtOutFile = New System.Windows.Forms.TextBox
        Me.lblData = New System.Windows.Forms.Label
        Me.chkDaily = New System.Windows.Forms.CheckBox
        Me.chkHourly = New System.Windows.Forms.CheckBox
        Me.fraStaParms.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraStaParms
        '
        Me.fraStaParms.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraStaParms.Controls.Add(Me.btnReset)
        Me.fraStaParms.Controls.Add(Me.rdoPct)
        Me.fraStaParms.Controls.Add(Me.rdoAbs)
        Me.fraStaParms.Controls.Add(Me.lblEditVals)
        Me.fraStaParms.Controls.Add(Me.btnSelParms)
        Me.fraStaParms.Controls.Add(Me.agdMonParms)
        Me.fraStaParms.Controls.Add(Me.lblStation)
        Me.fraStaParms.Location = New System.Drawing.Point(8, 152)
        Me.fraStaParms.Name = "fraStaParms"
        Me.fraStaParms.Size = New System.Drawing.Size(876, 372)
        Me.fraStaParms.TabIndex = 14
        Me.fraStaParms.TabStop = False
        Me.fraStaParms.Text = "Station Parameters"
        '
        'btnReset
        '
        Me.btnReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReset.Enabled = False
        Me.btnReset.Location = New System.Drawing.Point(709, 332)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(136, 20)
        Me.btnReset.TabIndex = 21
        Me.btnReset.Text = "Reset to Original Values"
        '
        'rdoPct
        '
        Me.rdoPct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rdoPct.Enabled = False
        Me.rdoPct.Location = New System.Drawing.Point(789, 56)
        Me.rdoPct.Name = "rdoPct"
        Me.rdoPct.Size = New System.Drawing.Size(72, 16)
        Me.rdoPct.TabIndex = 18
        Me.rdoPct.Text = "Percent"
        '
        'rdoAbs
        '
        Me.rdoAbs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rdoAbs.Checked = True
        Me.rdoAbs.Enabled = False
        Me.rdoAbs.Location = New System.Drawing.Point(789, 40)
        Me.rdoAbs.Name = "rdoAbs"
        Me.rdoAbs.Size = New System.Drawing.Size(72, 16)
        Me.rdoAbs.TabIndex = 17
        Me.rdoAbs.TabStop = True
        Me.rdoAbs.Text = "Absolute"
        '
        'lblEditVals
        '
        Me.lblEditVals.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEditVals.AutoSize = True
        Me.lblEditVals.Location = New System.Drawing.Point(773, 24)
        Me.lblEditVals.Name = "lblEditVals"
        Me.lblEditVals.Size = New System.Drawing.Size(77, 13)
        Me.lblEditVals.TabIndex = 16
        Me.lblEditVals.Text = "Edit Values by:"
        '
        'btnSelParms
        '
        Me.btnSelParms.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelParms.Enabled = False
        Me.btnSelParms.Location = New System.Drawing.Point(32, 332)
        Me.btnSelParms.Name = "btnSelParms"
        Me.btnSelParms.Size = New System.Drawing.Size(152, 20)
        Me.btnSelParms.TabIndex = 20
        Me.btnSelParms.Text = "Select Parms to View/Edit"
        '
        'agdMonParms
        '
        Me.agdMonParms.AllowHorizontalScrolling = True
        Me.agdMonParms.AllowNewValidValues = False
        Me.agdMonParms.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdMonParms.BackColor = System.Drawing.SystemColors.Control
        Me.agdMonParms.CellBackColor = System.Drawing.Color.Empty
        Me.agdMonParms.Enabled = False
        Me.agdMonParms.LineColor = System.Drawing.Color.Empty
        Me.agdMonParms.LineWidth = 0.0!
        Me.agdMonParms.Location = New System.Drawing.Point(4, 80)
        Me.agdMonParms.Name = "agdMonParms"
        Me.agdMonParms.Size = New System.Drawing.Size(868, 236)
        Me.agdMonParms.Source = Nothing
        Me.agdMonParms.TabIndex = 19
        '
        'lblStation
        '
        Me.lblStation.Location = New System.Drawing.Point(8, 24)
        Me.lblStation.Name = "lblStation"
        Me.lblStation.Size = New System.Drawing.Size(400, 56)
        Me.lblStation.TabIndex = 15
        '
        'btnParmFile
        '
        Me.btnParmFile.Location = New System.Drawing.Point(95, 40)
        Me.btnParmFile.Name = "btnParmFile"
        Me.btnParmFile.Size = New System.Drawing.Size(48, 20)
        Me.btnParmFile.TabIndex = 2
        Me.btnParmFile.Text = "Select"
        '
        'btnRun
        '
        Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRun.Location = New System.Drawing.Point(738, 532)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(80, 24)
        Me.btnRun.TabIndex = 23
        Me.btnRun.Text = "Run CliGen"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(652, 532)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(80, 24)
        Me.btnSave.TabIndex = 22
        Me.btnSave.Text = "Save Parms"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(824, 532)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(56, 24)
        Me.btnCancel.TabIndex = 24
        Me.btnCancel.Text = "Cancel"
        '
        'lblCliGen
        '
        Me.lblCliGen.AutoSize = True
        Me.lblCliGen.Location = New System.Drawing.Point(12, 9)
        Me.lblCliGen.Name = "lblCliGen"
        Me.lblCliGen.Size = New System.Drawing.Size(100, 13)
        Me.lblCliGen.TabIndex = 0
        Me.lblCliGen.Text = "Specify CliGen Files"
        '
        'lblParm
        '
        Me.lblParm.AutoSize = True
        Me.lblParm.Location = New System.Drawing.Point(12, 43)
        Me.lblParm.Name = "lblParm"
        Me.lblParm.Size = New System.Drawing.Size(77, 13)
        Me.lblParm.TabIndex = 1
        Me.lblParm.Text = "Parameter File:"
        '
        'lblOut
        '
        Me.lblOut.AutoSize = True
        Me.lblOut.Location = New System.Drawing.Point(12, 75)
        Me.lblOut.Name = "lblOut"
        Me.lblOut.Size = New System.Drawing.Size(61, 13)
        Me.lblOut.TabIndex = 4
        Me.lblOut.Text = "Output File:"
        '
        'btnOutFile
        '
        Me.btnOutFile.Location = New System.Drawing.Point(95, 72)
        Me.btnOutFile.Name = "btnOutFile"
        Me.btnOutFile.Size = New System.Drawing.Size(48, 20)
        Me.btnOutFile.TabIndex = 5
        Me.btnOutFile.Text = "Select"
        '
        'lblStart
        '
        Me.lblStart.AutoSize = True
        Me.lblStart.Location = New System.Drawing.Point(12, 112)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(71, 13)
        Me.lblStart.TabIndex = 7
        Me.lblStart.Text = "Starting Year:"
        '
        'lblNYrs
        '
        Me.lblNYrs.AutoSize = True
        Me.lblNYrs.Location = New System.Drawing.Point(146, 112)
        Me.lblNYrs.Name = "lblNYrs"
        Me.lblNYrs.Size = New System.Drawing.Size(89, 13)
        Me.lblNYrs.TabIndex = 9
        Me.lblNYrs.Text = "Number of Years:"
        '
        'txtNumYrs
        '
        Me.txtNumYrs.Location = New System.Drawing.Point(241, 109)
        Me.txtNumYrs.Name = "txtNumYrs"
        Me.txtNumYrs.Size = New System.Drawing.Size(32, 20)
        Me.txtNumYrs.TabIndex = 10
        Me.txtNumYrs.Text = "1"
        '
        'txtStartYear
        '
        Me.txtStartYear.Location = New System.Drawing.Point(101, 109)
        Me.txtStartYear.Name = "txtStartYear"
        Me.txtStartYear.Size = New System.Drawing.Size(36, 20)
        Me.txtStartYear.TabIndex = 8
        Me.txtStartYear.Text = "2000"
        Me.txtStartYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtParmFile
        '
        Me.txtParmFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtParmFile.Location = New System.Drawing.Point(149, 40)
        Me.txtParmFile.Name = "txtParmFile"
        Me.txtParmFile.ReadOnly = True
        Me.txtParmFile.Size = New System.Drawing.Size(735, 20)
        Me.txtParmFile.TabIndex = 3
        '
        'txtOutFile
        '
        Me.txtOutFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutFile.Location = New System.Drawing.Point(149, 72)
        Me.txtOutFile.Name = "txtOutFile"
        Me.txtOutFile.ReadOnly = True
        Me.txtOutFile.Size = New System.Drawing.Size(735, 20)
        Me.txtOutFile.TabIndex = 6
        '
        'lblData
        '
        Me.lblData.AutoSize = True
        Me.lblData.Location = New System.Drawing.Point(309, 112)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(206, 13)
        Me.lblData.TabIndex = 11
        Me.lblData.Text = "Select Data to be Available after Running:"
        '
        'chkDaily
        '
        Me.chkDaily.Checked = True
        Me.chkDaily.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDaily.Location = New System.Drawing.Point(536, 98)
        Me.chkDaily.Name = "chkDaily"
        Me.chkDaily.Size = New System.Drawing.Size(176, 26)
        Me.chkDaily.TabIndex = 12
        Me.chkDaily.Text = "Original Daily Cligen"
        '
        'chkHourly
        '
        Me.chkHourly.BackColor = System.Drawing.SystemColors.Control
        Me.chkHourly.Location = New System.Drawing.Point(536, 120)
        Me.chkHourly.Name = "chkHourly"
        Me.chkHourly.Size = New System.Drawing.Size(176, 26)
        Me.chkHourly.TabIndex = 13
        Me.chkHourly.Text = "Disaggregated Hourly"
        Me.chkHourly.UseVisualStyleBackColor = False
        '
        'frmCliGen
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(892, 568)
        Me.Controls.Add(Me.chkDaily)
        Me.Controls.Add(Me.chkHourly)
        Me.Controls.Add(Me.lblData)
        Me.Controls.Add(Me.txtOutFile)
        Me.Controls.Add(Me.txtParmFile)
        Me.Controls.Add(Me.txtStartYear)
        Me.Controls.Add(Me.txtNumYrs)
        Me.Controls.Add(Me.lblNYrs)
        Me.Controls.Add(Me.lblStart)
        Me.Controls.Add(Me.btnOutFile)
        Me.Controls.Add(Me.lblOut)
        Me.Controls.Add(Me.lblCliGen)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.btnParmFile)
        Me.Controls.Add(Me.fraStaParms)
        Me.Controls.Add(Me.lblParm)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmCliGen"
        Me.Text = "CliGen Weather Generator"
        Me.fraStaParms.ResumeLayout(False)
        Me.fraStaParms.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Function AskUser(ByRef aParmFileName As String, _
                            ByRef aOutFileName As String, _
                            ByRef aStartYear As Integer, _
                            ByRef aNumYears As Integer, _
                            ByRef aIncludeDaily As Boolean, _
                            ByRef aIncludeHourly As Boolean) As Boolean
        Me.ShowDialog()
        If pOk Then
            aParmFileName = pParmFileName
            aOutFileName = pOutFileName
            aStartYear = pStartYear
            aNumYears = pNumYears
            aIncludeDaily = chkDaily.Checked
            aIncludeHourly = chkHourly.Checked
        End If
        Return pOk
    End Function

    Private Sub frmCliGen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        agdMonParms.Source = New atcGridSource
        agdMonParms.Clear()
        With agdMonParms.Source
            .Rows = 1
            .Columns = 15
            .FixedRows = 1
            .FixedColumns = 1
            .ColorCells = True
            For i As Integer = 0 To .Columns - 1
                .CellColor(0, i) = SystemColors.ControlLight
                If i > 0 Then .Alignment(0, i) = atcAlignment.HAlignCenter
            Next
        End With
        GetParmsToEdit() 'read file containing parameters to be edited
        Me.Refresh()
    End Sub

    Private Sub btnParmFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParmFile.Click
        pParmFileName = FindFile("Select CliGen Parameter file to open", , , cParmFileFilter, True, , 1)
        If Len(pParmFileName) > 0 Then
            txtParmFile.Text = pParmFileName
            If ReadParmFile(pParmFileName, cHeader, cTable, cFooter) Then
                LoadGrid()
                agdMonParms.Enabled = True
                btnSelParms.Enabled = True
                rdoAbs.Enabled = True
                rdoPct.Enabled = True
                btnReset.Enabled = True
                lblStation.Text = cHeader
            End If
        End If
    End Sub

    Private Sub btnOutFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutFile.Click
        Dim cdlg As New Windows.Forms.SaveFileDialog
        With cdlg
            .Title = "CliGen Output File"
            .FileName = pOutFileName
            .Filter = cOutFileFilter
            .OverwritePrompt = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                pOutFileName = AbsolutePath(.FileName, CurDir)
                txtOutFile.Text = pOutFileName
            End If
        End With
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    'Private Function ReadParmFile(ByVal aFileName As String, ByRef aHeader As String, ByRef aTable As atcTableFixed, ByRef aFooter As String) As Boolean
    '  Dim lStr As String
    '  Dim lpos As Integer
    '  lStr = WholeFileString(aFileName)
    '  lpos = InStr(lStr, " MEAN P ")
    '  If lpos > 0 Then 'start of monthly data found, save headers
    '    aHeader = Mid(lStr, 1, lpos - 1)
    '    lblStation.Text = aHeader
    '    lStr = Mid(lStr, lpos)
    '    lpos = InStr(lStr, "INTERPOLATED")
    '    If lpos > 0 Then 'start of footer found
    '      aFooter = Mid(lStr, lpos)
    '      lStr = Mid(lStr, 1, lpos - 1)
    '    End If
    '    If Len(lStr) > 0 Then 'only editable table parameters left
    '      aTable = New atcTableFixed
    '      cParmStr = lStr
    '      If aTable.OpenString(lStr) Then 'load table into grid
    '        If ReadParmTable(lStr) Then
    '          Return True
    '        Else
    '          Return False
    '        End If
    '      End If
    '    End If
    '  Else
    '    Logger.Msg("CliGen parameters not found in file " & pParmFileName & vbCrLf & _
    '               "Expecting to find parameters starting with 'MEAN P'", "CliGen Problem")
    '    Return False
    '  End If
    'End Function

    'Private Function ReadParmTable(ByVal aParmStr As String) As Boolean

    '  Dim lSCol() As Integer = {0, 1, 9, 15, 21, 27, 33, 39, 45, 51, 57, 63, 69, 75, 81}
    '  Dim lFLen() As Integer = {0, 8, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6}
    '  Dim lFldNames() As String = {"", "Cons", "Jan", "Feb", "Mar", "Apr", "May", _
    '                               "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"}
    '  Dim i As Integer
    '  Dim lRow As Integer = 0
    '  Dim s As String
    '  cTable = New atcTableFixed
    '  With cTable
    '    .NumFields = 13
    '    For i = 1 To .NumFields
    '      .FieldName(i) = lFldNames(i)
    '      .FieldLength(i) = lFLen(i)
    '      .FieldStart(i) = lSCol(i)
    '      agdMonParms.Source.CellValue(0, i - 1) = lFldNames(i)
    '    Next
    '  End With
    '  If cModByPercent Then
    '    agdMonParms.Source.CellValue(0, 14) = "Edit Row, Percent"
    '  Else
    '    agdMonParms.Source.CellValue(0, 14) = "Edit Row, Absolute"
    '  End If
    '  If LoadGrid(aParmStr) Then
    '    Return True
    '  Else
    '    Return False
    '  End If

    'End Function

    'Private Sub WriteParmFile(ByVal aFileName As String, ByRef aHeader As String, ByRef aTable As IatcTable, ByRef aFooter As String)
    '  Dim lStr As String
    '  Dim lPos As Integer
    '  Dim lVal As Double
    '  lStr = aHeader
    '  cTable.MoveFirst()
    '  For i As Integer = 1 To cTable.NumRecords
    '    For j As Integer = 2 To cTable.NumFields
    '      If IsNumeric(agdMonParms.Source.CellValue(i, j - 1)) Then
    '        lVal = CDbl(agdMonParms.Source.CellValue(i, j - 1))
    '        Select Case Trim(UCase(cTable.Value(1)))
    '          Case "SOL.RAD", "SD SOL"
    '            cTable.Value(j) = RightJustify(DoubleToString(lVal, , "####.0"), cTable.FieldLength(j))
    '          Case "TIME PK"
    '            cTable.Value(j) = RightJustify(DoubleToString(lVal, , "#.000"), cTable.FieldLength(j))
    '          Case Else
    '            cTable.Value(j) = RightJustify(DoubleToString(lVal, , "###.00"), cTable.FieldLength(j))
    '        End Select
    '      End If
    '    Next
    '    lStr += cTable.CurrentRecordAsDelimitedString("") & vbCrLf
    '    cTable.MoveNext()
    '  Next i
    '  lStr += aFooter
    '  SaveFileString(aFileName, lStr)
    'End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim cdlg As New Windows.Forms.SaveFileDialog
        With cdlg
            .Title = "Save CliGen Parameter File"
            .FileName = pParmFileName
            .Filter = cParmFileFilter
            .OverwritePrompt = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                pParmFileName = AbsolutePath(.FileName, CurDir)
                SaveGridToTable()
                WriteParmFile(pParmFileName, cHeader, cTable, cFooter)
                txtParmFile.Text = pParmFileName
            End If
        End With

    End Sub

    Private Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If Len(pParmFileName) > 0 And Len(pOutFileName) > 0 Then
            If IsNumeric(txtStartYear.Text) And IsNumeric(txtNumYrs.Text) Then
                pStartYear = txtStartYear.Text
                If pStartYear >= 0 And pStartYear < 10000 Then
                    pNumYears = txtNumYrs.Text
                    If pNumYears > 0 And pNumYears < 10000 Then
                        pOk = True
                        Close()
                    Else
                        Logger.Msg("'Number of Years' must be between 1 and 9999" & vbCrLf & _
                                   "Value specified is '" & pNumYears & "'", "Run CliGen Problem")
                    End If
                Else
                    Logger.Msg("Value for 'Start Year' must be between 0 and 9999" & vbCrLf & _
                               "Value specified is '" & pStartYear & "'", "Run CliGen Problem")
                End If
            Else
                Logger.Msg("Values must be specified for both 'Start Year' and 'Number of Years' before running CliGen" & vbCrLf & _
                           "At least one of these two values is currently not numeric.", "Run CliGen Problem")
            End If
        Else
            Logger.Msg("Both 'Parameter' and 'Output' file names must be specified before running CliGen." & vbCrLf & _
                       "Use the 'Select' buttons next to each file type to specify the file names.", "Run CliGen Problem")
        End If
    End Sub

    Private Sub LoadGrid()
        Dim i As Integer
        Dim lRow As Integer
        Dim lParm As String
        Dim lWind As String = ""
        With cTable
            For i = 1 To .NumFields
                agdMonParms.Source.CellValue(0, i - 1) = .FieldName(i)
            Next
            If cModByPercent Then
                agdMonParms.Source.CellValue(0, 14) = "Edit Row, Percent"
            Else
                agdMonParms.Source.CellValue(0, 14) = "Edit Row, Absolute"
            End If
            agdMonParms.Source.Rows = 1
            For lRecordNumber As Integer = 1 To .NumRecords
                .CurrentRecord = lRecordNumber
                lParm = Trim(.Value(1))
                If lParm.IndexOf("%") >= 0 Then lWind = lParm
                If lWind.Length > 0 And (lParm.StartsWith("MEAN") Or lParm.StartsWith("STD DEV") Or lParm.StartsWith("SKEW")) Then
                    lParm = lWind & "-" & lParm
                End If
                If cParms.IndexFromKey(lParm) = -1 Then 'this parm not in collection, assume editing
                    cParms.Add(lParm, True)
                End If
                If cParms.ItemByKey(lParm) Then 'this parm selected for editing
                    lRow += 1
                    With agdMonParms.Source
                        For i = 1 To cTable.NumFields
                            If i = 1 Then
                                If cParmDescs.IndexFromKey(lParm) = -1 Then 'no description, use label from parm file 
                                    .CellValue(lRow, i - 1) = Trim(cTable.Value(i))
                                Else
                                    .CellValue(lRow, i - 1) = cParmDescs.ItemByKey(lParm)
                                End If
                                .CellColor(lRow, i - 1) = SystemColors.ControlLight
                            Else
                                .CellValue(lRow, i - 1) = Trim(cTable.Value(i))
                                .CellEditable(lRow, i - 1) = True
                                .Alignment(lRow, i - 1) = atcAlignment.HAlignCenter
                            End If
                        Next i
                        .CellColor(lRow, cTable.NumFields) = SystemColors.ControlLight
                        .CellValue(lRow, cTable.NumFields + 1) = "0"
                        .CellEditable(lRow, cTable.NumFields + 1) = True
                        .Alignment(lRow, cTable.NumFields + 1) = atcAlignment.HAlignCenter
                    End With
                End If
            Next lRecordNumber
            agdMonParms.SizeAllColumnsToContents(fraStaParms.Width - 30)
            agdMonParms.Refresh()
        End With
    End Sub

    Private Sub SaveGridToTable()
        Dim lInd As Integer
        Dim lParm As String
        Dim lNewVal As Double
        For i As Integer = 1 To agdMonParms.Source.Rows - 1
            lParm = agdMonParms.Source.CellValue(i, 0)
            lInd = cParmDescs.IndexOf(lParm)
            If lInd >= 0 Then 'get parameter name from same index as description
                lParm = cParms.Keys(lInd)
            End If
            For iMon As Integer = 2 To 13
                lNewVal = agdMonParms.Source.CellValue(i, iMon - 1)
                UpdateParmTable(cTable, lParm, iMon, lNewVal)
            Next
            'With cTable
            '  If .FindFirst(1, lParm) Then
            '    For iMon As Integer = 2 To 13
            '      lNewVal = agdMonParms.Source.CellValue(i, iMon - 1)
            '      Select Case lParm
            '        Case "SOL.RAD", "SD SOL"
            '          .Value(iMon) = RightJustify(DoubleToString(lNewVal, , "####.0"), .FieldLength(iMon))
            '        Case "TIME PK"
            '          .Value(iMon) = RightJustify(DoubleToString(lNewVal, , "#.000"), .FieldLength(iMon))
            '        Case Else
            '          .Value(iMon) = RightJustify(DoubleToString(lNewVal, , "###.00"), .FieldLength(iMon))
            '      End Select
            '    Next
            '    .Update()
            '  End If
            'End With
        Next
    End Sub

    Private Sub GetParmsToEdit()
        cParmsFile = FindFile("Locate file containing CliGen parameters to be edited", "CliGenEdit.prm", "*.prm", cParms2EditFilter)
        If cParmsFile.Length > 0 Then 'read parms 2 edit file
            Dim lStr As String = WholeFileString(cParmsFile)
            Dim lParm As String
            Dim lDesc As String = ""
            Dim lPos As Integer
            cParms.Clear()
            While lStr.Length > 0
                lParm = StrSplit(lStr, vbCrLf, "")
                lPos = lParm.IndexOf("'")
                If lPos > 0 Then 'description exists for this parm
                    lDesc = Trim(lParm.Substring(lPos + 1))
                    lParm = Trim(lParm.Substring(0, lPos))
                End If
                If lParm.Chars(0) = "#" Then 'not currently editing this parm
                    lParm = lParm.TrimStart("#")
                    cParms.Add(lParm, False)
                Else
                    cParms.Add(lParm, True)
                End If
                If lDesc.Length > 0 Then cParmDescs.Add(lParm, lDesc)
            End While
        End If
    End Sub

    Private Sub WriteParmsToEdit()
        Dim lStr As String = ""
        If cParmsFile.Length = 0 Then
            Dim cdlg As New Windows.Forms.SaveFileDialog
            With cdlg
                .Title = "Save File of CliGen Parameters to edit"
                .FileName = cParmsFile
                .Filter = cParms2EditFilter
                .OverwritePrompt = True
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    cParmsFile = AbsolutePath(.FileName, CurDir)
                End If
            End With
        End If
        If cParmsFile.Length > 0 Then
            For i As Integer = 0 To cParms.Count - 1
                If cParms.ItemByIndex(i) Then
                    lStr += cParms.Keys.Item(i)
                Else
                    lStr += "#" & cParms.Keys.Item(i)
                End If
                If cParmDescs.Count = cParms.Count Then 'include description
                    lStr += " '" & cParmDescs.ItemByIndex(i)
                End If
                lStr += vbCrLf
            Next
            SaveFileString(cParmsFile, lStr)
        End If
    End Sub

    Private Sub btnSelParms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelParms.Click
        Dim lform As New frmCliGenParmList
        If lform.AskUser(cParms, cParmDescs) Then
            WriteParmsToEdit()
            LoadGrid()
        End If
    End Sub

    Private Sub agdMonParms_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdMonParms.CellEdited
        With agdMonParms
            If aColumn = 14 AndAlso IsNumeric(.Source.CellValue(aRow, aColumn)) Then
                Dim lModVal As Double = CDbl(.Source.CellValue(aRow, aColumn))
                Dim lCurVal As Double
                For i As Integer = 1 To 12
                    If IsNumeric(.Source.CellValue(aRow, aColumn)) Then
                        lCurVal = CDbl(.Source.CellValue(aRow, i))
                        If cModByPercent Then
                            lCurVal = lCurVal * (1 + lModVal / 100.0)
                        Else
                            lCurVal += lModVal
                        End If
                        .Source.CellValue(aRow, i) = lCurVal
                    Else
                        Logger.Msg("Value in Row '" & aRow & "' and Column '" & aColumn & "' is not numeric ('" & .Source.CellValue(aRow, aColumn) & "') and cannot be updated." & vbCrLf & _
                                   "Make sure all values are numeric before editing a row of values.", "CliGen Problem")
                    End If
                Next
                .Refresh()
            End If
        End With
    End Sub

    Private Sub rdoPct_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoPct.CheckedChanged
        cModByPercent = rdoPct.Checked
        If cModByPercent Then 'adjust 'Edit' column header
            agdMonParms.Source.CellValue(0, 14) = "Edit Row, Percent"
        Else
            agdMonParms.Source.CellValue(0, 14) = "Edit Row, Absolute"
        End If
        agdMonParms.Refresh()
    End Sub

    Private Sub fraStaParms_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles fraStaParms.Resize
        If Not agdMonParms.Source Is Nothing Then
            agdMonParms.SizeAllColumnsToContents(fraStaParms.Width - 30)
            agdMonParms.Refresh()
        End If
    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        LoadGrid()
    End Sub

    Private Sub chkDaily_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDaily.CheckedChanged

    End Sub

    Private Sub frmCliGen_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Compute\Cligen.html")
        End If
    End Sub
End Class
