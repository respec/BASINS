Imports atcControls
Imports atcUtility
Imports MapWinUtility
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
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
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
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmCliGen))
    Me.GroupBox1 = New System.Windows.Forms.GroupBox
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
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'GroupBox1
    '
    Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBox1.Controls.Add(Me.rdoPct)
    Me.GroupBox1.Controls.Add(Me.rdoAbs)
    Me.GroupBox1.Controls.Add(Me.lblEditVals)
    Me.GroupBox1.Controls.Add(Me.btnSelParms)
    Me.GroupBox1.Controls.Add(Me.agdMonParms)
    Me.GroupBox1.Controls.Add(Me.lblStation)
    Me.GroupBox1.Location = New System.Drawing.Point(8, 152)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(655, 248)
    Me.GroupBox1.TabIndex = 1
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "Station Parameters"
    '
    'rdoPct
    '
    Me.rdoPct.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.rdoPct.Enabled = False
    Me.rdoPct.Location = New System.Drawing.Point(576, 224)
    Me.rdoPct.Name = "rdoPct"
    Me.rdoPct.Size = New System.Drawing.Size(72, 16)
    Me.rdoPct.TabIndex = 16
    Me.rdoPct.Text = "Percent"
    '
    'rdoAbs
    '
    Me.rdoAbs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.rdoAbs.Checked = True
    Me.rdoAbs.Enabled = False
    Me.rdoAbs.Location = New System.Drawing.Point(576, 208)
    Me.rdoAbs.Name = "rdoAbs"
    Me.rdoAbs.Size = New System.Drawing.Size(72, 16)
    Me.rdoAbs.TabIndex = 15
    Me.rdoAbs.TabStop = True
    Me.rdoAbs.Text = "Absolute"
    '
    'lblEditVals
    '
    Me.lblEditVals.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblEditVals.Location = New System.Drawing.Point(496, 216)
    Me.lblEditVals.Name = "lblEditVals"
    Me.lblEditVals.Size = New System.Drawing.Size(88, 16)
    Me.lblEditVals.TabIndex = 14
    Me.lblEditVals.Text = "Edit Values by:"
    '
    'btnSelParms
    '
    Me.btnSelParms.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnSelParms.Enabled = False
    Me.btnSelParms.Location = New System.Drawing.Point(32, 212)
    Me.btnSelParms.Name = "btnSelParms"
    Me.btnSelParms.Size = New System.Drawing.Size(152, 20)
    Me.btnSelParms.TabIndex = 11
    Me.btnSelParms.Text = "Select Parms to View/Edit"
    '
    'agdMonParms
    '
    Me.agdMonParms.AllowHorizontalScrolling = True
    Me.agdMonParms.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdMonParms.CellBackColor = System.Drawing.Color.Empty
    Me.agdMonParms.LineColor = System.Drawing.Color.Empty
    Me.agdMonParms.LineWidth = 0.0!
    Me.agdMonParms.Location = New System.Drawing.Point(4, 88)
    Me.agdMonParms.Name = "agdMonParms"
    Me.agdMonParms.Size = New System.Drawing.Size(647, 112)
    Me.agdMonParms.Source = Nothing
    Me.agdMonParms.TabIndex = 3
    '
    'lblStation
    '
    Me.lblStation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblStation.Location = New System.Drawing.Point(8, 24)
    Me.lblStation.Name = "lblStation"
    Me.lblStation.Size = New System.Drawing.Size(639, 56)
    Me.lblStation.TabIndex = 2
    Me.lblStation.Text = "Name:"
    '
    'btnParmFile
    '
    Me.btnParmFile.Location = New System.Drawing.Point(88, 40)
    Me.btnParmFile.Name = "btnParmFile"
    Me.btnParmFile.Size = New System.Drawing.Size(48, 20)
    Me.btnParmFile.TabIndex = 3
    Me.btnParmFile.Text = "Select"
    '
    'btnRun
    '
    Me.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.btnRun.Location = New System.Drawing.Point(308, 416)
    Me.btnRun.Name = "btnRun"
    Me.btnRun.Size = New System.Drawing.Size(80, 24)
    Me.btnRun.TabIndex = 4
    Me.btnRun.Text = "Run CliGen"
    '
    'btnSave
    '
    Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.btnSave.Location = New System.Drawing.Point(212, 416)
    Me.btnSave.Name = "btnSave"
    Me.btnSave.Size = New System.Drawing.Size(80, 24)
    Me.btnSave.TabIndex = 5
    Me.btnSave.Text = "Save Parms"
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.btnCancel.Location = New System.Drawing.Point(404, 416)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(56, 24)
    Me.btnCancel.TabIndex = 6
    Me.btnCancel.Text = "Cancel"
    '
    'lblCliGen
    '
    Me.lblCliGen.Location = New System.Drawing.Point(8, 16)
    Me.lblCliGen.Name = "lblCliGen"
    Me.lblCliGen.Size = New System.Drawing.Size(136, 16)
    Me.lblCliGen.TabIndex = 7
    Me.lblCliGen.Text = "Specify CliGen Files"
    '
    'lblParm
    '
    Me.lblParm.Location = New System.Drawing.Point(8, 40)
    Me.lblParm.Name = "lblParm"
    Me.lblParm.Size = New System.Drawing.Size(88, 16)
    Me.lblParm.TabIndex = 8
    Me.lblParm.Text = "Parameter File:"
    '
    'lblOut
    '
    Me.lblOut.Location = New System.Drawing.Point(8, 72)
    Me.lblOut.Name = "lblOut"
    Me.lblOut.Size = New System.Drawing.Size(64, 16)
    Me.lblOut.TabIndex = 9
    Me.lblOut.Text = "Output File:"
    '
    'btnOutFile
    '
    Me.btnOutFile.Location = New System.Drawing.Point(88, 72)
    Me.btnOutFile.Name = "btnOutFile"
    Me.btnOutFile.Size = New System.Drawing.Size(48, 20)
    Me.btnOutFile.TabIndex = 10
    Me.btnOutFile.Text = "Select"
    '
    'lblStart
    '
    Me.lblStart.Location = New System.Drawing.Point(8, 112)
    Me.lblStart.Name = "lblStart"
    Me.lblStart.Size = New System.Drawing.Size(80, 16)
    Me.lblStart.TabIndex = 12
    Me.lblStart.Text = "Starting Year:"
    '
    'lblNYrs
    '
    Me.lblNYrs.Location = New System.Drawing.Point(144, 112)
    Me.lblNYrs.Name = "lblNYrs"
    Me.lblNYrs.Size = New System.Drawing.Size(96, 16)
    Me.lblNYrs.TabIndex = 13
    Me.lblNYrs.Text = "Number of Years:"
    '
    'txtNumYrs
    '
    Me.txtNumYrs.Location = New System.Drawing.Point(240, 112)
    Me.txtNumYrs.Name = "txtNumYrs"
    Me.txtNumYrs.Size = New System.Drawing.Size(32, 20)
    Me.txtNumYrs.TabIndex = 14
    Me.txtNumYrs.Text = "1"
    '
    'txtStartYear
    '
    Me.txtStartYear.Location = New System.Drawing.Point(88, 112)
    Me.txtStartYear.Name = "txtStartYear"
    Me.txtStartYear.Size = New System.Drawing.Size(32, 20)
    Me.txtStartYear.TabIndex = 15
    Me.txtStartYear.Text = "2000"
    '
    'txtParmFile
    '
    Me.txtParmFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtParmFile.Location = New System.Drawing.Point(144, 40)
    Me.txtParmFile.Name = "txtParmFile"
    Me.txtParmFile.ReadOnly = True
    Me.txtParmFile.Size = New System.Drawing.Size(519, 20)
    Me.txtParmFile.TabIndex = 16
    Me.txtParmFile.Text = ""
    '
    'txtOutFile
    '
    Me.txtOutFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtOutFile.Location = New System.Drawing.Point(144, 72)
    Me.txtOutFile.Name = "txtOutFile"
    Me.txtOutFile.ReadOnly = True
    Me.txtOutFile.Size = New System.Drawing.Size(519, 20)
    Me.txtOutFile.TabIndex = 17
    Me.txtOutFile.Text = ""
    '
    'frmCliGen
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(672, 445)
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
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.lblParm)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmCliGen"
    Me.Text = "CliGen Weather Generator"
    Me.GroupBox1.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Function AskUser(ByRef aParmFileName As String, ByRef aOutFileName As String, ByRef aStartYear As Integer, ByRef aNumYears As Integer) As Boolean
    Me.ShowDialog()
    If pOk Then
      aParmFileName = pParmFileName
      aOutFileName = pOutFileName
      aStartYear = pStartYear
      aNumYears = pNumYears
    End If
    Return pOk
  End Function

  Private Sub frmCliGen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

    agdMonParms.Source = New atcGridSource
    agdMonParms.Clear()
    With agdMonParms.Source
      .Rows = 1
      .Columns = 14
      .FixedRows = 1
      .ColorCells = True
      .CellColor(0, 0) = SystemColors.ControlDark
    End With
    GetParmsToEdit() 'read file containing parameters to be edited
    Me.Refresh()
  End Sub

  Private Sub btnParmFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParmFile.Click
    pParmFileName = FindFile("Select CliGen Parameter file to open", , , cParmFileFilter, True, , 1)
    If Len(pParmFileName) > 0 Then
      txtParmFile.Text = pParmFileName
      If ReadParmFile(pParmFileName, cHeader, cTable, cFooter) Then
        btnSelParms.Enabled = True
        rdoAbs.Enabled = True
        rdoPct.Enabled = True
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

  Private Function ReadParmFile(ByVal aFileName As String, ByRef aHeader As String, ByRef aTable As atcTableFixed, ByRef aFooter As String) As Boolean
    Dim lStr As String
    Dim lpos As Integer
    lStr = WholeFileString(aFileName)
    lpos = InStr(lStr, " MEAN P ")
    If lpos > 0 Then 'start of monthly data found, save headers
      aHeader = Mid(lStr, 1, lpos - 1)
      lblStation.Text = aHeader
      lStr = Mid(lStr, lpos)
      lpos = InStr(lStr, "INTERPOLATED")
      If lpos > 0 Then 'start of footer found
        aFooter = Mid(lStr, lpos)
        lStr = Mid(lStr, 1, lpos - 1)
      End If
      If Len(lStr) > 0 Then 'only editable table parameters left
        aTable = New atcTableFixed
        cParmStr = lStr
        If aTable.OpenString(lStr) Then 'load table into grid
          If ReadParmTable(lStr) Then
            Return True
          Else
            Return False
          End If
        End If
      End If
    Else
      Logger.Msg("CliGen parameters not found in file " & pParmFileName & vbCrLf & _
                 "Expecting to find parameters starting with 'MEAN P'", "CliGen Problem")
      Return False
    End If
  End Function

  Private Function ReadParmTable(ByVal aParmStr As String) As Boolean

    Dim lSCol() As Integer = {0, 1, 9, 15, 21, 27, 33, 39, 45, 51, 57, 63, 69, 75, 81}
    Dim lFLen() As Integer = {0, 8, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6}
    Dim lFldNames() As String = {"", "Cons", "Jan", "Feb", "Mar", "Apr", "May", _
                                 "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"}
    Dim i As Integer
    Dim lRow As Integer = 0
    Dim s As String
    cTable = New atcTableFixed
    With cTable
      .NumFields = 13
      For i = 1 To .NumFields
        .FieldName(i) = lFldNames(i)
        .FieldLength(i) = lFLen(i)
        .FieldStart(i) = lSCol(i)
        agdMonParms.Source.CellValue(0, i - 1) = lFldNames(i)
      Next
    End With
    agdMonParms.Source.CellValue(0, 13) = "Edit Row"
    If LoadGrid(aParmStr) Then
      Return True
    Else
      Return False
    End If

  End Function

  Private Sub WriteParmFile(ByVal aFileName As String, ByRef aHeader As String, ByRef aTable As IatcTable, ByRef aFooter As String)
    Dim lStr As String
    Dim lPos As Integer
    Dim lVal As Double
    lStr = aHeader
    cTable.MoveFirst()
    For i As Integer = 1 To cTable.NumRecords
      For j As Integer = 2 To cTable.NumFields
        If IsNumeric(agdMonParms.Source.CellValue(i, j - 1)) Then
          lVal = CDbl(agdMonParms.Source.CellValue(i, j - 1))
          Select Case Trim(UCase(cTable.Value(1)))
            Case "SOL.RAD", "SD SOL"
              cTable.Value(j) = RightJustify(DoubleToString(lVal, , "####.0"), cTable.FieldLength(j))
            Case "TIME PK"
              cTable.Value(j) = RightJustify(DoubleToString(lVal, , "#.000"), cTable.FieldLength(j))
            Case Else
              cTable.Value(j) = RightJustify(DoubleToString(lVal, , "###.00"), cTable.FieldLength(j))
          End Select
        End If
      Next
      lStr += cTable.CurrentRecordAsDelimitedString("") & vbCrLf
      cTable.MoveNext()
    Next i
    lStr += aFooter
    SaveFileString(aFileName, lStr)
  End Sub

  Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

    Dim cdlg As New Windows.Forms.SaveFileDialog
    With cdlg
      .Title = "Save CliGen Parameter File"
      .FileName = pParmFileName
      .Filter = cParmFileFilter
      .OverwritePrompt = True
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        pParmFileName = AbsolutePath(.FileName, CurDir)
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

  Private Function LoadGrid(ByVal aParmStr As String) As Boolean
    Dim lRow As Integer
    Dim lParm As String
    Dim lWind As String = ""
    With cTable
      If .OpenString(aParmStr) Then
        agdMonParms.Source.Rows = 1
        While Not .atEOF
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
              For i As Integer = 1 To cTable.NumFields
                .CellValue(lRow, i - 1) = cTable.Value(i)
                If i > 1 Then .CellEditable(lRow, i - 1) = True
              Next i
              .CellValue(lRow, cTable.NumFields) = "0"
              .CellEditable(lRow, cTable.NumFields) = True
            End With
          End If
          .MoveNext()
        End While
        agdMonParms.SizeAllColumnsToContents()
        agdMonParms.Refresh()
        Return True
      Else
        Logger.Msg("Problem reading parameter table from Cligen file " & pParmFileName, "CliGen Problem")
        Return False
      End If
    End With
  End Function

  Private Sub GetParmsToEdit()
    cParmsFile = FindFile("Locate file containing CliGen parameters to be edited", "CliGenEdit.prm", "*.prm", cParms2EditFilter)
    If cParmsFile.Length > 0 Then 'read parms 2 edit file
      Dim lStr As String = WholeFileString(cParmsFile)
      Dim lParm As String
      cParms.Clear()
      While lStr.Length > 0
        lParm = StrSplit(lStr, vbCrLf, "")
        If lParm.Chars(0) = "#" Then 'not currently editing this parm
          cParms.Add(lParm.TrimStart("#"), False)
        Else
          cParms.Add(lParm, True)
        End If
      End While
    End If
  End Sub

  Private Sub WriteParmsToEdit()
    Dim lStr As String
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
        lStr += vbCrLf
      Next
      SaveFileString(cParmsFile, lStr)
    End If
  End Sub

  Private Sub btnSelParms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelParms.Click
    Dim lform As New frmCliGenParmList
    If lform.AskUser(cParms) Then
      WriteParmsToEdit()
      LoadGrid(cParmStr)
    End If
  End Sub

  Private Sub agdMonParms_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdMonParms.CellEdited
    With agdMonParms
      If aColumn = 13 AndAlso IsNumeric(.Source.CellValue(aRow, aColumn)) Then
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
  End Sub
End Class
