Imports System.Windows.Forms
Imports atcWASP

Public Class frmWASPSelectTimeSeries
    Friend pPlugIn As PlugIn

    Private project As WRDB.Project.clsProject
    Private datasource As WRDB.DataSource.clsData

    Private dgvStationMapping, dgvPCodeMapping As WRDB.Controls.DGVEditor

    Private _Selection As clsTimeSeriesSelection = Nothing
    Shared LastSelection As clsTimeSeriesSelection = Nothing

    ''' <summary>
    ''' Returns the selection that was made; use ToString to get concise string, and full class instance for all information
    ''' </summary>
    Public Property Selection() As clsTimeSeriesSelection
        Get
            Return _Selection
        End Get
        Set(ByVal value As clsTimeSeriesSelection)
            _Selection = value
            'lstStations.SelectedIndex = -1
            'lstPCodes.SelectedIndex = -1
            EnableControls()
        End Set
    End Property

    Public Sub New(ByVal aPlugin As atcWASPPlugIn.PlugIn, Optional ByVal AdditionalTitleText As String = "", Optional ByVal aSelection As clsTimeSeriesSelection = Nothing)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pPlugIn = aPlugin
        Me.Text &= " " & AdditionalTitleText
        Selection = aSelection
    End Sub

    Private Sub frmWASPSelectTimeSeries_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        SaveWindowPos(REGAPPNAME, Me)
    End Sub

    Private Sub frmWASPSelectTimeSeries_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetWindowPos(REGAPPNAME, Me)

        btnOK.Enabled = False

        'load list of WRDB projects

        Dim iniFile As New WRDB.Project.clsWRDBIniFile
        cboProjects.Items.AddRange(iniFile.GetProjectNames.ToArray)

        'load conversion factor list

        lstConversion.Items.Clear()
        lstConversion.Items.AddRange(clsTimeSeriesSelection.ConversionText)

        'see if selection has been set; if so fill in proper fields; if not, try to set to prior selection

        If Selection.SelectionType = clsTimeSeriesSelection.enumSelectionType.None Then Selection = LastSelection

        If Selection IsNot Nothing Then
            With Selection
                lstConversion.SelectedIndex = .ConversionType
                txtScaleFactor.Text = .ScaleFactor
                .StationID = TestNull(.StationID, "")
                .PCode = TestNull(.PCode, "")

                Select Case .SelectionType
                    Case clsTimeSeriesSelection.enumSelectionType.Constant
                        TabMain.SelectedTab = tpConstant
                        txtConstant.Text = .DataSource
                    Case clsTimeSeriesSelection.enumSelectionType.WRDB, clsTimeSeriesSelection.enumSelectionType.Database
                        TabMain.SelectedTab = tpDatabase
                        optWRDB.Checked = .SelectionType = clsTimeSeriesSelection.enumSelectionType.WRDB
                        optDatabase.Checked = .SelectionType = clsTimeSeriesSelection.enumSelectionType.Database
                        If optWRDB.Checked Then
                            If cboProjects.Items.Contains(.DataSource) Then cboProjects.Text = .DataSource
                        Else
                            txtFilename.Text = .DataSource
                        End If

                        If lstTables.Items.Contains(.Table) Then lstTables.Text = .Table
                        If lstStations.Items.Contains(.StationID) Then lstStations.Text = .StationID
                        If lstPCodes.Items.Contains(.PCode) Then lstPCodes.Text = .PCode
                        If .StationID = "" And .PCode = "" Then chkMapping.Checked = True
                End Select
            End With
        Else
            lstConversion.Text = "None"
            txtScaleFactor.Text = 1.0
        End If

        EnableControls()

    End Sub

    Private Sub SetupMappingGrids()
        'load mapping lists

        Dim s As String = ""

        dgvStationMapping = New WRDB.Controls.DGVEditor(dgStationMapping)

        With dgStationMapping

            'save prior entries so can restore after setup lists
            Dim vals(.RowCount - 1) As String
            For r As Integer = 0 To .RowCount - 1
                If .Item(1, r).Value IsNot Nothing Then vals(r) = .Item(1, r).Value
            Next

            .Rows.Clear()
            .RowCount = pPlugIn.WASPProject.Segments.Count + 1

            If _Selection IsNot Nothing AndAlso _Selection.SelectionType = clsTimeSeriesSelection.enumSelectionType.WRDB AndAlso _Selection.Table <> "" Then
                Dim lst As New Generic.List(Of String)
                lst.AddRange(project.DB.GetRecordList(_Selection.Table, "Station_ID"))
                For Each v As String In vals
                    If Not lst.Contains(v) Then lst.Add(v)
                Next
                dgvStationMapping.SetColumnFormat(WRDB.Controls.DGVEditor.enumColumnTypes.ComboBox, 1, lst)
                If vals.Length > 0 Then
                    For r As Integer = 0 To .RowCount - 1
                        .Item(1, r).Value = vals(r)
                    Next
                End If
            End If

            For lRow As Integer = 0 To pPlugIn.WASPProject.Segments.Count - 1
                Dim seg As atcWASPSegment = pPlugIn.WASPProject.Segments(lRow)
                .Item(0, lRow).Value = seg.WaspName
                If pPlugIn.WASPProject.StationMapping.TryGetValue(.Item(0, lRow).Value, s) Then .Item(1, lRow).Value = s
            Next

            With .Rows(.RowCount - 1)
                .Cells(0).Value = "Time Functions"
                If pPlugIn.WASPProject.StationMapping.TryGetValue(.Cells(0).Value, s) Then .Cells(1).Value = s
            End With
            .CurrentCell = .Item(1, 0)
        End With

        dgvPCodeMapping = New WRDB.Controls.DGVEditor(dgPCodeMapping)

        With dgPCodeMapping
            Dim vals(.RowCount - 1) As String
            For r As Integer = 0 To .RowCount - 1
                If .Item(1, r).Value IsNot Nothing Then vals(r) = .Item(1, r).Value
            Next

            .Rows.Clear()
            .RowCount = 1 + pPlugIn.WASPProject.WASPConstituents.Count + pPlugIn.WASPProject.WASPTimeFunctions.Count

            If _Selection IsNot Nothing AndAlso _Selection.SelectionType = clsTimeSeriesSelection.enumSelectionType.WRDB AndAlso _Selection.Table <> "" Then
                Dim lst As New Generic.List(Of String)
                'lst.Add(" ")
                lst.AddRange(project.DB.GetRecordList(_Selection.Table, "PCode"))
                For Each v As String In vals
                    If v IsNot Nothing AndAlso Not lst.Contains(v) Then lst.Add(v)
                Next
                dgvPCodeMapping.SetColumnFormat(WRDB.Controls.DGVEditor.enumColumnTypes.ComboBox, 1, lst)
                If vals.Length > 0 Then
                    For r As Integer = 0 To .RowCount - 1
                        .Item(1, r).Value = vals(r)
                    Next
                End If
            End If

            Dim lstConv As New Generic.List(Of String)
            For conv As clsTimeSeriesSelection.enumConversion = clsTimeSeriesSelection.enumConversion.None To clsTimeSeriesSelection.enumConversion.Hr_Sec
                lstConv.Add(clsTimeSeriesSelection.GetConversionName(conv))
            Next
            dgvPCodeMapping.SetColumnFormat(WRDB.Controls.DGVEditor.enumColumnTypes.ComboBox, 2, lstConv)
            dgPCodeMapping.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim lRow As Integer = 0

            .Item(0, lRow).Value = "Input Flows"
            Dim PCodeMap As clsPCodeMapping = Nothing
            If pPlugIn.WASPProject.PCodeMapping.TryGetValue(.Item(0, lRow).Value, PCodeMap) Then
                .Item(1, lRow).Value = PCodeMap.PCode
                .Item(2, lRow).Value = clsTimeSeriesSelection.GetConversionName(PCodeMap.ConversionType)
                .Item(3, lRow).Value = PCodeMap.ScaleFactor
            Else
                .Item(2, lRow).Value = "None"
                .Item(3, lRow).Value = 1.0
            End If
            lRow += 1

            For i As Integer = 0 To pPlugIn.WASPProject.WASPConstituents.Count - 1
                .Item(0, lRow).Value = pPlugIn.WASPProject.WASPConstituents(i).Description
                If pPlugIn.WASPProject.PCodeMapping.TryGetValue(.Item(0, lRow).Value, PCodeMap) Then
                    .Item(1, lRow).Value = PCodeMap.PCode
                    .Item(2, lRow).Value = clsTimeSeriesSelection.GetConversionName(PCodeMap.ConversionType)
                    .Item(3, lRow).Value = PCodeMap.ScaleFactor
                Else
                    .Item(2, lRow).Value = "None"
                    .Item(3, lRow).Value = 1.0
                End If
                lRow += 1
            Next

            For i As Integer = 0 To pPlugIn.WASPProject.WASPTimeFunctions.Count - 1
                .Item(0, lRow).Value = pPlugIn.WASPProject.WASPTimeFunctions(i).Description
                If pPlugIn.WASPProject.PCodeMapping.TryGetValue(.Item(0, lRow).Value, PCodeMap) Then
                    .Item(1, lRow).Value = PCodeMap.PCode
                    .Item(2, lRow).Value = clsTimeSeriesSelection.GetConversionName(PCodeMap.ConversionType)
                    .Item(3, lRow).Value = PCodeMap.ScaleFactor
                Else
                    .Item(2, lRow).Value = "None"
                    .Item(3, lRow).Value = 1.0
                End If
                lRow += 1
            Next

            .CurrentCell = .Item(1, 0)

        End With

    End Sub

    Private Sub lstConversion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstConversion.SelectedIndexChanged
        If Selection IsNot Nothing Then
            Selection.ConversionType = lstConversion.SelectedIndex
        End If
        ToolTip1.SetToolTip(lstConversion, "Conversion factor=" & clsTimeSeriesSelection.ConversionFactor(lstConversion.SelectedIndex))
    End Sub

    Private Sub txtConstant_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConstant.TextChanged
        Selection = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.Constant, clsTimeSeriesSelection.enumConversion.None, 1.0, txtConstant.Text)
    End Sub

    Private Sub cboProjects_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProjects.SelectedIndexChanged, optWRDB.CheckedChanged
        If Not optWRDB.Checked Then Exit Sub
        lstTables.Items.Clear()
        lstStations.Items.Clear()
        lstPCodes.Items.Clear()
        If cboProjects.SelectedIndex = -1 Then Exit Sub
        'update table list
        project = New WRDB.Project.clsProject(cboProjects.Text)
        WriteProgress("Getting table list...")
        lstTables.Items.AddRange(project.GetTableList(New WRDB.Project.clsProject.enumTableTypes() {WRDB.Project.clsProject.enumTableTypes.Working, WRDB.Project.clsProject.enumTableTypes.Master}).ToArray)
        EnableControls()
        ProgressComplete()
    End Sub

    Private Sub lstWRDBTables_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstTables.SelectedIndexChanged
        Try
            'update stations list
            lstStations.Items.Clear()
            lstPCodes.Items.Clear()
            WriteProgress("Getting station list...")
            If optWRDB.Checked Then
                lstStations.Items.AddRange(project.DB.GetRecordList(lstTables.Text, "Station_ID").ToArray)
                If chkMapping.Checked Then Selection = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.WRDB, lstConversion.SelectedIndex, Val(txtScaleFactor.Text), cboProjects.Text, lstTables.Text)
            Else
                Select Case datasource.DataType
                    Case WRDB.DataSource.clsData.enumDataType.LspcAir, WRDB.DataSource.clsData.enumDataType.LspcOut
                    Case Else
                        datasource.ActiveTable = lstTables.Text
                End Select
                With lstStations
                    .Items.Clear()
                    .Items.AddRange(datasource.StationIDs.ToArray)
                    If .Items.Count > 0 Then .SelectedIndex = 0
                End With
                If chkMapping.Checked Then Selection = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.Database, lstConversion.SelectedIndex, Val(txtScaleFactor.Text), txtFilename.Text, lstTables.Text)
            End If
        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            ProgressComplete()
        End Try
    End Sub

    Private Sub lstWRDBStations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstStations.SelectedIndexChanged
        Try
            If lstStations.SelectedIndex = -1 Then Exit Sub
            'update pcodes list
            lstPCodes.Items.Clear()
            WriteProgress("Getting pcode list...")
            If optWRDB.Checked Then
                lstPCodes.Items.AddRange(project.DB.GetRecordList(lstTables.Text, "PCode", "WHERE Station_ID='" & lstStations.Text & "'").ToArray)
                Selection = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.WRDB, lstConversion.SelectedIndex, Val(txtScaleFactor.Text), cboProjects.Text, lstTables.Text, lstStations.Text)
            Else
                With lstPCodes
                    .Items.Clear()
                    .Items.AddRange(datasource.PCodes.ToArray)
                    If .Items.Count > 0 Then .SelectedIndex = 0
                End With
            End If
        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            ProgressComplete()
        End Try
    End Sub

    Private Sub lstPCodes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstPCodes.DoubleClick, lstConversion.DoubleClick
        btnOK.PerformClick()
    End Sub

    Private Sub lstPCodes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPCodes.SelectedIndexChanged
        Try
            WriteProgress("Getting record count and date range...")
            If optWRDB.Checked Then
                Dim dr As Data.Common.DbDataReader = project.DB.ExecuteReader("SELECT COUNT(Result),MIN(Date_Time),MAX(Date_Time) FROM {0} WHERE Station_ID='{1}' AND PCode='{2}' GROUP BY Station_ID,PCode", lstTables.Text, lstStations.Text, lstPCodes.Text)
                Dim NumRecords As Integer = 0
                Dim MinDate, MaxDate As Date
                If dr IsNot Nothing AndAlso dr.Read Then
                    NumRecords = dr.GetValue(0)
                    MinDate = dr.GetValue(1)
                    MaxDate = dr.GetValue(2)
                Else
                    Exit Sub
                End If
                dr.Close()
                lblRecordCount.Text = String.Format("Record Count: {0}; Date Range: {1:MM/dd/yyyy}-{2:MM/dd/yyyy}", NumRecords, MinDate, MaxDate)
                Selection = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.WRDB, lstConversion.SelectedIndex, Val(txtScaleFactor.Text), cboProjects.Text, lstTables.Text, lstStations.Text, lstPCodes.Text)
            Else
                Dim MinDate As Date = datasource.MinDate
                Dim MaxDate As Date = datasource.MaxDate
                ProgressComplete()
                With lblRecordCount
                    .Text = String.Format("Date Range: {0:MM/dd/yyyy}-{1:MM/dd/yyyy}", MinDate, MaxDate)
                    .Visible = True
                End With
                Selection = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.Database, lstConversion.SelectedIndex, Val(txtScaleFactor.Text), txtFilename.Text, lstTables.Text, lstStations.Text, lstPCodes.Text)
            End If
            lblRecordCount.Visible = True
        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            ProgressComplete()
        End Try
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Static LastFilterIndex As Integer = 0
        With New OpenFileDialog
            .InitialDirectory = GetSetting(REGAPPNAME, "Import", "TableDir")
            .Filter = "All Import Files|*.bmd;*.db;*.dbf;*.xls*;*.txt;*.csv;*.air;*.out;*.mdb|" & _
                        "Binary Modeling File (*.bmd)|*.bmd|" & _
                        "Paradox (WRDB) Table (*.db)|*.db|" & _
                        "dBase Table (*.dbf)|*.dbf|" & _
                        "Excel (*.xls*)|*.xls*|" & _
                        "Tab-delimited Text (*.txt)|*.txt|" & _
                        "Comma-delimited Text (*.csv)|*.csv|" & _
                        "LSPC/EFDC Output Files (*.out)|*.out|" & _
                        "LSPC Input Files (*.air)|*.air|" & _
                        "Access Database (*.mdb)|*.mdb|" & _
                        "All Files (*.*)|*.*"
            .FilterIndex = LastFilterIndex
            .AutoUpgradeEnabled = True
            .CheckFileExists = True
            .CheckPathExists = True
            .Multiselect = False
            .Title = "Select Database File"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtFilename.Text = .FileName
                LastFilterIndex = .FilterIndex
                SaveSetting(REGAPPNAME, "Import", "TableDir", IO.Path.GetDirectoryName(.FileName))
            End If
        End With
    End Sub

    Private Sub txtFilename_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilename.TextChanged, optDatabase.CheckedChanged
        If Not optDatabase.Checked Then Exit Sub
        lstTables.Items.Clear()
        lstStations.Items.Clear()
        lstPCodes.Items.Clear()
        If Not My.Computer.FileSystem.FileExists(txtFilename.Text) Then Exit Sub
        datasource = WRDB.DataSource.clsData.Instantiate(txtFilename.Text)
        If datasource Is Nothing Then Exit Sub
        WriteProgress("Getting table list...")
        With lstTables
            Select Case IO.Path.GetExtension(txtFilename.Text).ToLower
                Case ".xls", ".xlsx", ".xlsm", ".mdb"
                    .Enabled = True
                    .Items.AddRange(datasource.TableNames.ToArray)
                    If .Items.Count > 0 Then .SelectedIndex = 0
                Case Else
                    .Enabled = False
                    .Items.Add(IO.Path.GetFileNameWithoutExtension(txtFilename.Text))
                    .SelectedIndex = 0
            End Select
        End With
        EnableControls()
        ProgressComplete()
    End Sub

    Private Sub txtScaleFactor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScaleFactor.TextChanged
        If Selection IsNot Nothing Then
            Selection.ScaleFactor = Val(txtScaleFactor.Text)
        End If
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click, btnApplyAll.Click
        LastSelection = Selection

        'save mapping lists

        With dgStationMapping
            For lRow As Integer = 0 To .RowCount - 1
                Dim seg As String = .Item(0, lRow).Value
                Dim sta As String = TestNull(.Item(1, lRow).Value, "")
                If Not pPlugIn.WASPProject.StationMapping.ContainsKey(seg) Then
                    pPlugIn.WASPProject.StationMapping.Add(seg, sta)
                Else
                    pPlugIn.WASPProject.StationMapping(seg) = sta
                End If
            Next
        End With

        With dgPCodeMapping
            For lRow As Integer = 0 To .RowCount - 1
                Dim con As String = .Item(0, lRow).Value
                Dim pcd As String = TestNull(.Item(1, lRow).Value, "")
                Dim conv As String = TestNull(.Item(2, lRow).Value, "None")
                Dim scal As Double = TestNull(.Item(3, lRow).Value, 1.0)
                If Not pPlugIn.WASPProject.PCodeMapping.ContainsKey(con) Then pPlugIn.WASPProject.PCodeMapping.Add(con, Nothing)
                pPlugIn.WASPProject.PCodeMapping(con) = New clsPCodeMapping(pcd, conv, scal)
            Next
        End With

        If sender Is btnApplyAll Then
            Me.DialogResult = System.Windows.Forms.DialogResult.Retry
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        End If
        Me.Close()
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        _Selection = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
        DialogResult = Windows.Forms.DialogResult.Abort
        Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub WriteProgress(ByVal Message As String)
        TabMain.Enabled = False
        btnOK.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        lblRecordCount.Text = Message
        lblRecordCount.Visible = True
        Application.DoEvents()
    End Sub

    Private Sub ProgressComplete()
        TabMain.Enabled = True
        'OK_Button.Enabled = True
        Me.Cursor = Cursors.Default
        If lblRecordCount.Text.EndsWith("...") Then lblRecordCount.Text = ""
        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' If mapping is checked, disable station and PCode list boxes; if unchecked, clear possible prior selection
    ''' </summary>
    Private Sub chkMapping_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMapping.CheckedChanged
        If chkMapping.Checked Then
            If optWRDB.Checked Then
                Selection = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.WRDB, lstConversion.SelectedIndex, Val(txtScaleFactor.Text), cboProjects.Text, lstTables.Text)
            Else
                Selection = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.Database, lstConversion.SelectedIndex, Val(txtScaleFactor.Text), txtFilename.Text, lstTables.Text)
            End If
        Else
            Selection = Nothing
        End If
        EnableControls()
    End Sub

    Private Sub EnableControls()
        lstStations.Enabled = Not chkMapping.Checked
        lstPCodes.Enabled = Not chkMapping.Checked
        lstConversion.Enabled = Not chkMapping.Checked
        txtScaleFactor.Enabled = Not chkMapping.Checked
        cboProjects.Enabled = optWRDB.Checked
        txtFilename.Enabled = optDatabase.Checked
        btnBrowse.Enabled = optDatabase.Checked
        If optWRDB.Checked Then
            lstTables.Enabled = True
        Else
            Select Case IO.Path.GetExtension(txtFilename.Text).ToLower
                Case ".xls", ".xlsx", ".xlsm", ".mdb"
                    lstTables.Enabled = True
                Case Else
                    lstTables.Enabled = False
            End Select
        End If
        btnApplyAll.Visible = False
        If _Selection Is Nothing Then
            btnOK.Enabled = False
        Else
            If (_Selection.SelectionType = clsTimeSeriesSelection.enumSelectionType.WRDB And String.IsNullOrEmpty(_Selection.Table)) Or (_Selection.SelectionType = clsTimeSeriesSelection.enumSelectionType.Database And String.IsNullOrEmpty(_Selection.DataSource)) Then
                btnOK.Enabled = False
            ElseIf Not String.IsNullOrEmpty(_Selection.StationID) AndAlso String.IsNullOrEmpty(_Selection.PCode) Then
                btnOK.Enabled = False
            Else
                btnOK.Enabled = True
                btnApplyAll.Visible = chkMapping.Checked And (_Selection.SelectionType = clsTimeSeriesSelection.enumSelectionType.WRDB Or _Selection.SelectionType = clsTimeSeriesSelection.enumSelectionType.Database)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Convert Station IDs and PCodes to upper case
    ''' </summary>
    Private Sub dgMapping_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgStationMapping.CellValidated, dgPCodeMapping.CellValidated
        With CType(sender, DataGridView)
            If .ColumnCount >= 2 AndAlso .Item(1, e.RowIndex).Value IsNot Nothing AndAlso Not IsDBNull(.Item(1, e.RowIndex).Value) Then .Item(1, e.RowIndex).Value = CStr(.Item(1, e.RowIndex).Value).ToUpper
        End With
    End Sub

    Private Sub TabMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabMain.SelectedIndexChanged
        If TabMain.SelectedTab Is tpMappings Then SetupMappingGrids()
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        If lstTables.SelectedIndex = -1 Then Exit Sub
        Cursor = Cursors.WaitCursor
        Application.DoEvents()
        Dim Tablename As String = lstTables.Text
        Dim StationID As String = lstStations.Text : If lstStations.SelectedIndex = -1 Then StationID = ""
        Dim PCode As String = lstPCodes.Text : If lstPCodes.SelectedIndex = -1 Then PCode = ""
        Dim ds As DataSet = Nothing
        With New frmTableViewer
            With .dgTable
                If optWRDB.Checked Then
                    Dim WhereClause As String = ""
                    If StationID <> "" Then WhereClause = String.Format(" WHERE Station_ID='{0}'", StationID)
                    If PCode <> "" Then WhereClause &= String.Format(" AND PCode='{0}'", PCode)
                    ds = New DataSet
                    .DefaultCellStyle.NullValue = "<Null>"
                    Dim Sql As String = String.Format("SELECT * from {0}{1} ORDER BY Station_ID,PCode,Date_Time", Tablename, WhereClause)
                    project.DB.AddTable(ds, Tablename, Sql)
                    .DataSource = ds.Tables(Tablename)
                Else
                    If StationID <> "" Then datasource.ActiveStationID = StationID
                    For Each c As String In datasource.FieldNames
                        .Columns.Add(c, c)
                    Next
                    datasource.Rewind()
                    While datasource.Read
                        If PCode <> "" AndAlso (Not datasource.ContainsFieldName(PCode) OrElse datasource.ContainsFieldName("PCode") AndAlso datasource.Items("PCode").ToUpper <> PCode.ToUpper) Then Continue While
                        Dim items(datasource.NumItems - 1) As String
                        For c As Integer = 0 To datasource.NumItems - 1
                            items(c) = datasource.Items(c)
                        Next
                        .Rows.Add(items)
                    End While
                End If
                .AutoSize = True
            End With
            If optWRDB.Checked Then
                .Text = String.Format("Table Viewer: {0} ({1} records)", Tablename, ds.Tables(Tablename).Rows.Count)
            Else
                .Text = String.Format("Table Viewer: {0} ({1} records)", Tablename, .dgTable.Rows.Count)
            End If
            .ShowDialog(Me)
            .Dispose()
            If ds IsNot Nothing Then ds.Dispose()
        End With
        Cursor = Cursors.Default
    End Sub

    Private Sub optWRDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optWRDB.CheckedChanged
        _Selection = Nothing
        EnableControls()
    End Sub

    Private Sub lnkImportMappings_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkImportMappings.LinkClicked
        Dim InitialDir As String = ""
        If pPlugIn.WASPProject.Filename = "" Then
            InitialDir = IO.Path.GetDirectoryName(pPlugIn.MapWin.Project.FileName)
        Else
            InitialDir = IO.Path.GetDirectoryName(pPlugIn.WASPProject.Filename)
        End If
        Dim FileName As String = OpenFile(InitialDir)
        If FileName <> "" Then
            Dim ImportProject As New atcWASP.atcWASPProject
            With ImportProject
                .LoadProject(FileName)
                If pPlugIn.WASPProject.ModelType <> .ModelType Then
                    WarningMsg("The WaspBuilder project file you selected does not have the same WASP model type.")
                Else
                    pPlugIn.WASPProject.StationMapping.Clear()
                    For Each s As String In .StationMapping.Keys
                        pPlugIn.WASPProject.StationMapping.Add(s, .StationMapping(s))
                    Next
                    pPlugIn.WASPProject.PCodeMapping.Clear()
                    For Each s As String In .PCodeMapping.Keys
                        pPlugIn.WASPProject.PCodeMapping.Add(s, .PCodeMapping(s))
                    Next
                End If
            End With
            SetupMappingGrids()
        End If
    End Sub
End Class

