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
  Dim cParmFileFilter As String = "Cligen Parameter Files (*.par)|*.par"
  Dim cOutFileFilter As String = "Cligen Output Files (*.dat)|*.dat"

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
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmCliGen))
    Me.GroupBox1 = New System.Windows.Forms.GroupBox
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
    Me.GroupBox1.Controls.Add(Me.agdMonParms)
    Me.GroupBox1.Controls.Add(Me.lblStation)
    Me.GroupBox1.Location = New System.Drawing.Point(8, 152)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(456, 256)
    Me.GroupBox1.TabIndex = 1
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "Station Parameters"
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
    Me.agdMonParms.Size = New System.Drawing.Size(448, 152)
    Me.agdMonParms.Source = Nothing
    Me.agdMonParms.TabIndex = 3
    '
    'lblStation
    '
    Me.lblStation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblStation.Location = New System.Drawing.Point(8, 24)
    Me.lblStation.Name = "lblStation"
    Me.lblStation.Size = New System.Drawing.Size(440, 56)
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
    Me.btnRun.Location = New System.Drawing.Point(208, 424)
    Me.btnRun.Name = "btnRun"
    Me.btnRun.Size = New System.Drawing.Size(80, 24)
    Me.btnRun.TabIndex = 4
    Me.btnRun.Text = "Run CliGen"
    '
    'btnSave
    '
    Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.btnSave.Location = New System.Drawing.Point(112, 424)
    Me.btnSave.Name = "btnSave"
    Me.btnSave.Size = New System.Drawing.Size(80, 24)
    Me.btnSave.TabIndex = 5
    Me.btnSave.Text = "Save Parms"
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.btnCancel.Location = New System.Drawing.Point(304, 424)
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
    Me.txtParmFile.Size = New System.Drawing.Size(320, 20)
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
    Me.txtOutFile.Size = New System.Drawing.Size(320, 20)
    Me.txtOutFile.TabIndex = 17
    Me.txtOutFile.Text = ""
    '
    'frmCliGen
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(473, 453)
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
      .Rows = 3
      .Columns = 13
      .FixedRows = 1
      .ColorCells = True
      .CellColor(0, 0) = SystemColors.ControlDark
    End With
    Me.Refresh()
  End Sub

  Private Sub btnParmFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParmFile.Click
    pParmFileName = FindFile("Select CliGen Parameter file to open", , , cParmFileFilter, True, , 1)
    If Len(pParmFileName) > 0 Then
      txtParmFile.Text = pParmFileName
      ReadParmFile(pParmFileName, cHeader, cTable, cFooter)
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

  Private Sub ReadParmFile(ByVal aFileName As String, ByRef aHeader As String, ByRef aTable As atcTableFixed, ByRef aFooter As String)
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
      If Len(lStr) > 0 Then
        aTable = New atcTableFixed
        If aTable.OpenString(lStr) Then 'load table into grid
          ReadParmTable(lStr)
        End If
      End If
    End If
  End Sub

  Private Sub ReadParmTable(ByVal aParmStr As String)

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
      If cTable.OpenString(aParmStr) Then
        While Not cTable.atEOF
          lRow += 1
          For i = 1 To .NumFields
            agdMonParms.Source.CellValue(lRow, i - 1) = .Value(i)
            If i > 1 Then agdMonParms.Source.CellEditable(lRow, i - 1) = True
          Next i
          .MoveNext()
        End While
        agdMonParms.SizeAllColumnsToContents()
        agdMonParms.Refresh()
      End If
    End With

  End Sub

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
          MsgBox("Value for Start Year must be between 0 and 9999", , "Run CliGen Problem")
        End If
      Else
        MsgBox("Values must be specified for both Start Year and Number of Years before running CliGen", , "Run CliGen Problem")
      End If
    Else
      MsgBox("Both Parameter and Output file names must be specified before running CliGen.", , "Run CliGen Problem")
    End If
  End Sub
End Class
